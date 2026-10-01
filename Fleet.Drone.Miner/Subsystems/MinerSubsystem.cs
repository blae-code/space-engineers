using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // Mission orchestration (C30 §4): feeds the FSM its observations, runs each state's entry and
        // per-tick actions, owns the job, the paths and the helm. Per-tick paths do not allocate.
        public class MinerSubsystem : ISubsystem
        {
            const double Dt10 = 1.0 / 6, Dt1 = 1.0 / 60, ReachDist = 3, BackoffSeconds = 3, UnloadTimeout = 60;

            readonly Rig _r;
            public readonly MinerFsm Fsm = new MinerFsm(MinerState.Idle);
            public readonly Helm Helm;
            readonly DrillLogic _drill = new DrillLogic();
            readonly List<Vector2I> _holes = new List<Vector2I>();
            readonly PathRecorder _recorder = new PathRecorder(500);
            readonly List<Vector3D> _dockPath = new List<Vector3D>(), _jobPath = new List<Vector3D>();
            readonly PathFollower _follower = new PathFollower();
            readonly GpsRoute _gps = new GpsRoute();
            readonly DockingPlanner _docking = new DockingPlanner();
            readonly List<IMyInventory> _carrierInv = new List<IMyInventory>();
            readonly List<IMyCargoContainer> _containers = new List<IMyCargoContainer>();
            StuckDetector _stuck;

            // Job, all local to the home connector frame (Frames).
            bool _hasJob, _jobIsGps;
            Vector3D _jobOrigin, _jobFwd, _jobUp, _jobRight;
            double _spacing;
            int _jobW, _jobH;
            public int HoleIndex { get; private set; }
            public int HoleCount { get { return _holes.Count; } }
            bool _redoHole, _manualFull, _holeDone, _useGps, _pendingReconcile = true;
            long _homeId;
            int _recording;                         // 0 none, 1 dock path, 2 job route
            MinerState _savedState = MinerState.Idle;
            MatrixD _ref = MatrixD.Identity;
            int _stuckRetries;
            double _backoffUntil = -1, _unloadStart;
            Vector3D _backoffTarget;
            bool _unloadTimedOut;
            MinerInput _obs;                        // observations gathered by this tick's state action
            PoseTarget _t;
            public bool WantsUpdate1 { get; private set; }
            public double Depth { get; private set; }

            public MinerSubsystem(Rig r)
            {
                _r = r;
                Helm = new Helm(r.Ship);
                _stuck = new StuckDetector(r.Settings.StuckSeconds, 0.2);
            }

            public string Name { get { return "Miner"; } }
            public int StorageVersion { get { return 1; } }
            public bool HasJob { get { return _hasJob; } }
            public void HandleMessage(MyIGCMessage msg) { }
            public void Status(StringBuilder sb) { }

            Blackboard Bb { get { return _r.Bb; } }
            Settings S { get { return _r.Settings; } }
            Vector3D ShipPos { get { return Bb.ShipMatrix.Translation; } }
            Vector3D Local(Vector3D world) { return Frames.ToLocalPoint(Bb.HomeMatrix, world); }
            Vector3D World(Vector3D local) { return Frames.ToWorldPoint(Bb.HomeMatrix, local); }
            Vector3D WorldDir(Vector3D local) { return Frames.ToWorldDir(Bb.HomeMatrix, local); }
            MatrixD RefWorld(MatrixD refInShip) { return refInShip * Bb.ShipMatrix; }
            Vector3D DrillFace { get { return RefWorld(_r.Scan.RefDrillInShip).Translation; } }
            double Standoff { get { return Math.Max(3, 1.5 * Bb.ShipSize); } }

            // ---------- ticks ----------

            public void Update1()
            {
                if (!WantsUpdate1 || Fsm.State != MinerState.Dock) return;
                DockTick();
                Helm.Update(Dt1);
            }

            public void Update100() { }

            public void Update10()
            {
                ResolveHome();
                if (_pendingReconcile)
                {
                    if (Bb.SensingTicks < 2) { Helm.Release(); return; }
                    Reconcile();
                }

                var input = _obs;
                input.Command = Cmd.None;
                input.HasJob = _hasJob;
                input.HasDockPath = _dockPath.Count > 1;
                input.Connected = Bb.Connected;
                if (Fsm.State != MinerState.Retract) input.JobComplete = HoleIndex >= _holes.Count;
                var ti = new TriggerInput
                {
                    CargoFill = Bb.CargoFill, LiftMargin = Bb.LiftMargin, BatteryFill = Bb.BatteryFill,
                    HydrogenFill = Bb.HydrogenFill, UraniumKg = Bb.UraniumKg, HasBattery = Bb.HasBattery,
                    HasHydrogen = Bb.HasHydrogen, HasReactor = Bb.HasReactor, Damaged = _r.Damage.Damaged,
                    JobDone = HoleIndex >= _holes.Count, ManualFull = _manualFull
                };
                input.Trigger = ReturnTriggers.Evaluate(ti, S);
                Step(input);

                _obs = new MinerInput();
                StateTick();
                if (!WantsUpdate1) Helm.Update(Dt10);
            }

            void Step(MinerInput input)
            {
                var from = Fsm.State;
                if (!Fsm.Step(input, S) || Fsm.State == from) return;
                Enter(from, Fsm.State);
            }

            // ---------- home + reload ----------

            void ResolveHome()
            {
                var c = _r.Scan.Connector;
                if (c != null && c.Status == MyShipConnectorStatus.Connected && c.OtherConnector != null)
                {
                    _homeId = c.OtherConnector.EntityId;
                    Bb.HomeMatrix = c.OtherConnector.WorldMatrix;
                    Bb.HomeConnectorId = _homeId;
                }
                else if (_homeId != 0)
                {
                    // Slice 1 carriers are stationary: the last known home pose stays valid while away.
                    var b = _r.Gts.GetBlockWithId(_homeId) as IMyShipConnector;
                    if (b != null) Bb.HomeMatrix = b.WorldMatrix;
                }
                Bb.HasHome = _homeId != 0;
            }

            void Reconcile()
            {
                _pendingReconcile = false;
                var ri = new ReconcileInput
                {
                    Saved = _savedState, HomeFound = Bb.HasHome, Connected = Bb.Connected, HasJob = _hasJob,
                    CargoFill = Bb.CargoFill, ShipSize = Bb.ShipSize,
                    DistToHoleEntrance = _hasJob && HoleIndex < _holes.Count ? Vector3D.Distance(DrillFace, Entrance()) : 0
                };
                string note; bool redo;
                var state = Reconciler.Resolve(ri, S.OnReload, out note, out redo);
                _redoHole = redo;
                var from = Fsm.State;
                Fsm.Force(state, note);
                if (_hasJob) _r.Damage.TakeBaseline();
                _r.Events.Add(Bb.Time, _savedState, state, ReturnReason.None, note ?? "reconciled after reload");
                Enter(from, state);
            }

            // Re-run the reload reconciliation (REBOOT): holds controls until sensing is fresh again.
            public void Reboot()
            {
                _savedState = Fsm.State;
                _pendingReconcile = true;
                Bb.SensingTicks = 0;
                Helm.Release();
            }

            // ---------- transitions ----------

            void Enter(MinerState from, MinerState to)
            {
                _r.Events.Add(Bb.Time, from, to, Fsm.PendingReason, Fsm.Note);
                _r.RequestSave();
                if (from == MinerState.Charge) _r.Energy.SetCharging(false);
                if (from == MinerState.Retract) FinishHole();
                if (from == MinerState.Recording && to != MinerState.Recording) KeepRecording();
                WantsUpdate1 = false;
                _stuckRetries = 0;
                _backoffUntil = -1;
                if (_stuck.StuckSeconds != S.StuckSeconds) _stuck = new StuckDetector(S.StuckSeconds, 0.2);
                _stuck.Reset();

                switch (to)
                {
                    case MinerState.Idle:
                    case MinerState.Hold:
                    case MinerState.Safe:
                        Helm.Release(); _r.DrillsOn(false); break;
                    case MinerState.Recording:
                        Helm.Release(); _recorder.Begin(Local(ShipPos)); break;
                    case MinerState.Undock:
                        UseRef(_r.Scan.RefConnInShip);
                        if (_r.Scan.Connector != null) _r.Scan.Connector.Disconnect();
                        break;
                    case MinerState.DockPathOut:
                    case MinerState.DockPathIn:
                        UseRef(MatrixD.Identity);
                        _follower.StartNearest(_dockPath, to == MinerState.DockPathIn, Local(ShipPos));
                        break;
                    case MinerState.RouteOut:
                    case MinerState.RouteBack:
                        UseRef(MatrixD.Identity);
                        bool back = to == MinerState.RouteBack;
                        _useGps = _jobIsGps || _jobPath.Count < 2;
                        if (_useGps) _gps.Start(back ? DockApproachPoint() : World(_jobOrigin), Bb.InGravity);
                        else _follower.StartNearest(_jobPath, back, Local(ShipPos));
                        break;
                    case MinerState.Position:
                        UseRef(_r.Scan.RefDrillInShip); break;
                    case MinerState.Drill:
                        UseRef(_r.Scan.RefDrillInShip);
                        _holeDone = false;
                        _r.DrillsOn(true);
                        _drill.Start(S.Depth - S.StartDepth, S.WorkSpeed);
                        break;
                    case MinerState.Retract:
                        if (_drill.Current == DrillLogic.Phase.Advance) _drill.BeginRetract();
                        break;
                    case MinerState.Dock:
                        UseRef(_r.Scan.RefConnInShip);
                        _docking.Start(Bb.Time);
                        break;
                    case MinerState.Unload:
                        Helm.Release();
                        _r.DrillsOn(false);
                        _manualFull = false;
                        _unloadStart = Bb.Time;
                        _unloadTimedOut = false;
                        CollectCarrierInventories();
                        break;
                    case MinerState.Charge:
                        _r.Energy.SetCharging(true); break;
                }
            }

            void UseRef(MatrixD refInShip) { _ref = refInShip; Helm.SetReference(refInShip); }

            void FinishHole()
            {
                _r.DrillsOn(false);
                if (_drill.WasBlocked) _r.Note("hole blocked, skipped");
                if (_holeDone && !_redoHole) HoleIndex++;
                _redoHole = false;
                _holeDone = false;
            }

            // Where a GPS route home ends: the dock path's outer end, else the dock standoff.
            Vector3D DockApproachPoint()
            {
                if (_dockPath.Count > 1) return World(_dockPath[_dockPath.Count - 1]);
                return DockingPlanner.UndockTarget(Bb.HomeMatrix, Standoff);
            }

            void CollectCarrierInventories()
            {
                _carrierInv.Clear();
                var c = _r.Scan.Connector;
                if (c == null || c.OtherConnector == null) return;
                var grid = c.OtherConnector.CubeGrid;
                _r.Gts.GetBlocksOfType(_containers, b => b.CubeGrid == grid);   // rare path (state entry)
                for (int pass = 0; pass < 2; pass++)
                    for (int i = 0; i < _containers.Count; i++)
                        if (_containers[i].CustomName.Contains(S.Tag) == (pass == 0))
                            _carrierInv.Add(_containers[i].GetInventory(0));
            }

            // ---------- per-state actions ----------

            Vector3D Entrance()
            {
                var cell = _holes[Math.Min(HoleIndex, _holes.Count - 1)];
                return HoleGrid.HoleEntrance(World(_jobOrigin), WorldDir(_jobRight), WorldDir(_jobUp), cell, _spacing)
                    + WorldDir(_jobFwd) * S.StartDepth;
            }

            void StateTick()
            {
                var now = Bb.Time;
                var refW = RefWorld(_ref);
                switch (Fsm.State)
                {
                    case MinerState.Recording:
                        if (_recorder.Update(Local(ShipPos), Bb.Velocity.Length()) == false && _recorder.IsFull)
                            _r.Note("path recorder full");
                        break;

                    case MinerState.Undock:
                    {
                        var target = DockingPlanner.UndockTarget(Bb.HomeMatrix, Standoff);
                        Fly(target, refW.Forward, refW.Up, S.ApproachSpeed, -1, false);
                        _obs.UndockClear = Vector3D.Distance(refW.Translation, target) < 1;
                        break;
                    }

                    case MinerState.DockPathOut:
                    case MinerState.DockPathIn:
                        if (_dockPath.Count < 2) { _obs.PathDone = true; break; }
                        FollowPath(Math.Min(S.ApproachSpeed * 2, S.MaxSpeed));
                        _obs.PathDone = _follower.Done;
                        break;

                    case MinerState.RouteOut:
                    case MinerState.RouteBack:
                        if (_useGps)
                        {
                            var carrot = _gps.Update(ShipPos, Bb.Gravity, Bb.HasElevation ? Bb.Elevation : 1e9, S.SafeAltitude, ReachDist);
                            Vector3D f, u;
                            TravelAxes(carrot, out f, out u);
                            Fly(carrot, f, u, S.MaxSpeed, _gps.Remaining, false);
                            _obs.RouteDone = _gps.Current == GpsRoute.Phase.Done;
                        }
                        else
                        {
                            FollowPath(S.MaxSpeed);
                            _obs.RouteDone = _follower.Done;
                        }
                        break;

                    case MinerState.Position:
                        if (HoleIndex >= _holes.Count) break;
                        Fly(Entrance(), WorldDir(_jobFwd), WorldDir(_jobUp), S.ApproachSpeed, -1, false);
                        _obs.AtHole = Helm.Distance < 0.5 && Helm.AlignmentError < 0.035;
                        break;

                    case MinerState.Drill:
                    case MinerState.Retract:
                    {
                        var entrance = Entrance();
                        var fwd = WorldDir(_jobFwd);
                        Depth = Vector3D.Dot(DrillFace - entrance, fwd);
                        var phase = _drill.Update(now, Depth, Vector3D.Dot(Bb.Velocity, fwd));
                        _r.Cargo.EjectStep(S);
                        if (Fsm.State == MinerState.Drill)
                        {
                            double full = S.Depth - S.StartDepth;
                            Engage(entrance + fwd * full, fwd, WorldDir(_jobUp), _drill.Feed, Math.Max(0, full - Depth), false);
                            if (phase != DrillLogic.Phase.Advance) { _holeDone = true; _obs.DrillFinished = true; }
                        }
                        else
                        {
                            Fly(entrance, fwd, WorldDir(_jobUp), S.RetractSpeed, -1, false);
                            _obs.RetractDone = phase == DrillLogic.Phase.Done;
                            _obs.JobComplete = HoleIndex + (_holeDone && !_redoHole ? 1 : 0) >= _holes.Count;
                        }
                        break;
                    }

                    case MinerState.Dock:
                        DockTick();
                        _obs.Connected = Bb.Connected;
                        _obs.DockFailed = _docking.Current == DockingPlanner.Phase.Failed;
                        break;

                    case MinerState.Unload:
                        _r.Cargo.UnloadStep(_carrierInv);
                        if (!_unloadTimedOut && now - _unloadStart > UnloadTimeout) { _unloadTimedOut = true; _r.Note("unload timeout"); }
                        _obs.Unloaded = !_r.Cargo.HasOre() || _unloadTimedOut;
                        break;

                    case MinerState.Charge:
                        _obs.Charged = _r.Energy.IsCharged();
                        _obs.UraniumLow = Bb.HasReactor && Bb.UraniumKg < S.MinUranium;
                        break;
                }
            }

            void DockTick()
            {
                var conn = _r.Scan.Connector;
                if (conn == null) { _obs.DockFailed = true; return; }
                var cw = conn.WorldMatrix;
                _t = _docking.Update(Bb.Time, Bb.HomeMatrix, cw.Translation, cw.Forward, Bb.Connectable, Bb.Connected,
                    Standoff, S.ApproachSpeed, S.DockSpeed);
                Helm.Engage(_t);
                WantsUpdate1 = _t.Precision;
                if (_docking.Current == DockingPlanner.Phase.Connect && Bb.Connectable) conn.Connect();
            }

            void FollowPath(double maxSpeed)
            {
                double cap, remaining;
                var carrot = World(_follower.Update(Local(ShipPos), ReachDist, maxSpeed, out cap, out remaining));
                Vector3D f, u;
                TravelAxes(carrot, out f, out u);
                Fly(carrot, f, u, Math.Min(maxSpeed, cap), remaining, false);
            }

            // Nose along the travel direction (level on planets), up away from gravity or kept as is.
            void TravelAxes(Vector3D target, out Vector3D fwd, out Vector3D up)
            {
                var m = Bb.ShipMatrix;
                var d = target - m.Translation;
                if (Bb.InGravity)
                {
                    up = Vector3D.Normalize(-Bb.Gravity);
                    d -= up * Vector3D.Dot(d, up);
                    fwd = d.LengthSquared() > 1 ? Vector3D.Normalize(d) : m.Forward - up * Vector3D.Dot(m.Forward, up);
                    if (fwd.LengthSquared() < 1e-6) fwd = Vector3D.CalculatePerpendicularVector(up);
                    fwd = Vector3D.Normalize(fwd);
                }
                else
                {
                    fwd = d.LengthSquared() > 1 ? Vector3D.Normalize(d) : m.Forward;
                    up = GyroMath.PerpendicularUp(fwd, m.Up, m.Forward);
                }
            }

            // Engage with stuck handling: back off and retry twice, then report Stuck (spec §5.5).
            void Fly(Vector3D pos, Vector3D fwd, Vector3D up, double cap, double brake, bool precision)
            {
                var refPos = RefWorld(_ref).Translation;
                if (Bb.Time < _backoffUntil) { Engage(_backoffTarget, fwd, up, S.ApproachSpeed, -1, false); return; }
                if (_backoffUntil > 0) { _backoffUntil = -1; _stuck.Reset(); }
                Engage(pos, fwd, up, cap, brake, precision);
                var toTarget = pos - refPos;
                double dist = toTarget.Length();
                double progress = dist > 1e-6 ? Vector3D.Dot(Bb.Velocity, toTarget / dist) : 0;
                if (!_stuck.Update(Bb.Time, progress, dist > 1.0)) return;
                if (_stuckRetries < 2)
                {
                    _stuckRetries++;
                    _backoffTarget = refPos - (dist > 1e-6 ? toTarget / dist : fwd) * 3;
                    _backoffUntil = Bb.Time + BackoffSeconds;
                    _r.Note("stuck, backing off");
                }
                else _obs.Stuck = true;
            }

            void Engage(Vector3D pos, Vector3D fwd, Vector3D up, double cap, double brake, bool precision)
            {
                _t.Position = pos; _t.Forward = fwd; _t.Up = up; _t.SpeedCap = cap; _t.BrakeDist = brake; _t.Precision = precision;
                Helm.Engage(_t);
            }

            // ---------- commands ----------

            public void Command(Cmd cmd, string rest)
            {
                switch (cmd)
                {
                    case Cmd.SetJob: SetJobHere(); break;
                    case Cmd.Goto: SetJobGps(rest); break;
                    case Cmd.RecordDock:
                    case Cmd.RecordJob:
                        if (!Bb.HasHome) { _r.Note("dock first: paths are recorded relative to home"); return; }
                        _recording = cmd == Cmd.RecordDock ? 1 : 2;
                        break;
                    case Cmd.Full: _manualFull = true; break;
                    case Cmd.Next: HoleIndex = Math.Min(HoleIndex + 1, _holes.Count); break;
                    case Cmd.Prev: HoleIndex = Math.Max(HoleIndex - 1, 0); break;
                    case Cmd.Reboot: _r.Rescan(); Reboot(); return;
                    case Cmd.GyroTest:
                        if ((Fsm.State == MinerState.Idle || Fsm.State == MinerState.Hold) && !Bb.Connected)
                            _r.Ship.BeginGyroTest(Bb.Time);
                        else _r.Note("GYROTEST: Idle/Hold and undocked only");
                        return;
                }
                if (cmd == Cmd.Start && !_r.Scan.Ready) { _r.Note(_r.Scan.Diagnostics[0]); return; }
                var input = new MinerInput
                {
                    Command = cmd, HasJob = _hasJob, HasDockPath = _dockPath.Count > 1, Connected = Bb.Connected,
                    JobComplete = HoleIndex >= _holes.Count
                };
                Step(input);
                _r.RequestSave();
            }

            // Leaving Recording (STOPREC, STOP, RESET): close the path and keep it as the dock path or job route.
            void KeepRecording()
            {
                _recorder.End(Local(ShipPos));
                if (_recording == 0) return;
                var path = _recording == 1 ? _dockPath : _jobPath;
                path.Clear();
                path.AddRange(_recorder.Points);
                _r.Note(_recording == 1 ? "dock path saved" : "job route saved");
                _recording = 0;
            }

            bool CanRedefineJob()
            {
                if (Fsm.State == MinerState.Idle || Fsm.State == MinerState.Hold) return true;
                _r.Note("STOP before changing the job");
                return false;
            }

            void SetJobHere()
            {
                if (!CanRedefineJob()) return;
                if (!Bb.HasHome) { _r.Note("SETJOB needs a home: dock once first"); return; }
                var m = Bb.ShipMatrix;
                DefineJob(Local(DrillFace), m.Forward, m.Up, false);
                _r.Note("job set here");
            }

            void SetJobGps(string rest)
            {
                if (!CanRedefineJob()) return;
                Vector3D target;
                if (!Gps.TryParse(rest, out target)) { _r.Note("bad GPS"); return; }
                if (!Bb.HasHome) { _r.Note("GOTO needs a home: dock once first"); return; }
                Vector3D fwd = Bb.InGravity ? Vector3D.Normalize(Bb.Gravity) : Vector3D.Normalize(target - Bb.HomeMatrix.Translation);
                var up = GyroMath.PerpendicularUp(fwd, Bb.ShipMatrix.Up, Bb.ShipMatrix.Forward);
                DefineJob(Local(target), fwd, up, true);
                _r.Note("GPS job set");
            }

            void DefineJob(Vector3D originLocal, Vector3D fwdWorld, Vector3D upWorld, bool gps)
            {
                var h = Bb.HomeMatrix;
                _jobOrigin = originLocal;
                _jobFwd = Frames.ToLocalDir(h, fwdWorld);
                _jobUp = Frames.ToLocalDir(h, upWorld);
                _jobRight = Frames.ToLocalDir(h, Vector3D.Cross(fwdWorld, upWorld));
                _spacing = HoleGrid.SpacingFromDrills(_r.Scan.DrillLocal);
                _jobW = S.Width; _jobH = S.Height;
                HoleGrid.Spiral(_jobW, _jobH, _holes);
                HoleIndex = 0;
                _redoHole = false;
                _jobIsGps = gps;
                _hasJob = true;
                _r.Damage.TakeBaseline();
                _r.RequestSave();
            }

            // ---------- Storage ----------

            public void Save(MyIni ini)
            {
                var n = Name;
                ini.Set(n, "state", (int)Fsm.State);
                ini.Set(n, "note", Fsm.Note ?? "");
                ini.Set(n, "holeIndex", HoleIndex);
                ini.Set(n, "redoHole", _redoHole);
                ini.Set(n, "jobIsGps", _jobIsGps);
                ini.Set(n, "hasJob", _hasJob);
                SetVec(ini, n, "o", _jobOrigin); SetVec(ini, n, "f", _jobFwd);
                SetVec(ini, n, "u", _jobUp); SetVec(ini, n, "r", _jobRight);
                ini.Set(n, "spacing", _spacing);
                ini.Set(n, "width", _jobW);
                ini.Set(n, "height", _jobH);
                ini.Set(n, "homeId", _homeId);
                var h = Bb.HomeMatrix;
                SetVec(ini, n, "hp", h.Translation); SetVec(ini, n, "hf", h.Forward); SetVec(ini, n, "hu", h.Up);
                PathCodec.Save(ini, "Miner.DockPath", _dockPath);
                PathCodec.Save(ini, "Miner.JobPath", _jobPath);
            }

            public bool Load(MyIni ini, int savedVersion)
            {
                if (savedVersion != StorageVersion) return false;
                var n = Name;
                _savedState = (MinerState)ini.Get(n, "state").ToInt32(0);
                HoleIndex = ini.Get(n, "holeIndex").ToInt32(0);
                _redoHole = ini.Get(n, "redoHole").ToBoolean(false);
                _jobIsGps = ini.Get(n, "jobIsGps").ToBoolean(false);
                _hasJob = ini.Get(n, "hasJob").ToBoolean(false);
                _jobOrigin = GetVec(ini, n, "o"); _jobFwd = GetVec(ini, n, "f");
                _jobUp = GetVec(ini, n, "u"); _jobRight = GetVec(ini, n, "r");
                _spacing = ini.Get(n, "spacing").ToDouble(3);
                _jobW = ini.Get(n, "width").ToInt32(1);
                _jobH = ini.Get(n, "height").ToInt32(1);
                _homeId = ini.Get(n, "homeId").ToInt64(0);
                var hp = GetVec(ini, n, "hp"); var hf = GetVec(ini, n, "hf"); var hu = GetVec(ini, n, "hu");
                if (hf.LengthSquared() > 0.5 && hu.LengthSquared() > 0.5) Bb.HomeMatrix = MatrixD.CreateWorld(hp, hf, hu);
                Bb.HasHome = _homeId != 0;
                if (_hasJob) HoleGrid.Spiral(_jobW, _jobH, _holes);
                if (HoleIndex > _holes.Count) HoleIndex = _holes.Count;
                PathCodec.Load(ini, "Miner.DockPath", _dockPath);
                PathCodec.Load(ini, "Miner.JobPath", _jobPath);
                return true;
            }

            static void SetVec(MyIni ini, string sec, string k, Vector3D v)
            {
                ini.Set(sec, k + "x", v.X); ini.Set(sec, k + "y", v.Y); ini.Set(sec, k + "z", v.Z);
            }

            static Vector3D GetVec(MyIni ini, string sec, string k)
            {
                return new Vector3D(ini.Get(sec, k + "x").ToDouble(0), ini.Get(sec, k + "y").ToDouble(0), ini.Get(sec, k + "z").ToDouble(0));
            }
        }
    }
}
