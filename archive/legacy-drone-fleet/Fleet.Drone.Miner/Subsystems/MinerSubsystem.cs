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
        //
        // Hardened after the 2026-09-30 adversarial review (findings in the plan's checkpoint log):
        // observations are computed from fresh geometry, not from last tick's helm; references are
        // re-derived every tick from the controller block (the CoM moves as ore loads); a hole only
        // advances when its retract really finishes; the drill face clears the rock before any lateral
        // move; HOME means "come home and stay"; reload restores ResumeState / PendingReason.
        public class MinerSubsystem : ISubsystem
        {
            const double Dt10 = 1.0 / 6, Dt1 = 1.0 / 60, ReachDist = 3, BackoffSeconds = 3,
                UnloadTimeout = 60, ChargeTimeout = 300, Clearance = 1.5;
            const int RefShip = 0, RefConn = 1, RefDrill = 2;

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

            // Job in WORLD space (slice 2: a moving carrier must not drag the dig site along). The dock
            // path stays local to the home connector (it belongs to the carrier); the job route is world.
            bool _hasJob, _jobIsGps;
            Vector3D _jobOrigin, _jobFwd, _jobUp, _jobRight;
            double _spacing;
            int _jobW, _jobH;
            public int HoleIndex { get; private set; }
            public int HoleCount { get { return _holes.Count; } }
            bool _manualFull, _holeDone, _useGps, _pendingReconcile = true, _stayHome;
            long _homeId;
            int _recording;                         // 0 none, 1 dock path, 2 job route
            int _refKind = RefShip;
            MinerState _savedState = MinerState.Idle, _savedResume = MinerState.Idle;
            ReturnReason _savedPending = ReturnReason.None;
            int _stuckRetries;
            double _backoffUntil = -1, _stateStart;
            Vector3D _backoffTarget, _holdFwd, _holdUp, _frameVel, _evadeLocal;
            bool _homeFrame, _backoffHome;          // targets move with the carrier / back-off stored home-local
            double _evadeUntil = -1;
            MatrixD _routeHome = MatrixD.Identity;  // home pose when the job route was recorded
            bool _undockNoted;
            bool _timedOut;
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
            public int StorageVersion { get { return 3; } }
            public bool HasJob { get { return _hasJob; } }
            public void HandleMessage(MyIGCMessage msg) { }
            public void Status(StringBuilder sb) { }

            Blackboard Bb { get { return _r.Bb; } }
            Settings S { get { return _r.Settings; } }
            Vector3D ShipPos { get { return Bb.ShipMatrix.Translation; } }
            Vector3D Local(Vector3D world) { return Frames.ToLocalPoint(Bb.HomeMatrix, world); }
            Vector3D World(Vector3D local) { return Frames.ToWorldPoint(Bb.HomeMatrix, local); }
            Vector3D WorldDir(Vector3D local) { return Frames.ToWorldDir(Bb.HomeMatrix, local); }
            double Standoff { get { return Math.Max(3, 1.5 * Bb.ShipSize); } }

            // World pose of a reference point, straight from the controller block (no CoM involved).
            MatrixD RefWorld(int kind)
            {
                var ctrl = _r.Scan.Controller;
                if (kind == RefShip || ctrl == null) return Bb.ShipMatrix;
                return (kind == RefConn ? _r.Scan.RefConnInCtrl : _r.Scan.RefDrillInCtrl) * ctrl.WorldMatrix;
            }

            // The same reference expressed in the helm's CoM frame, re-derived every tick.
            MatrixD RefInShip(int kind)
            {
                if (kind == RefShip || _r.Scan.Controller == null) return MatrixD.Identity;
                return RefWorld(kind) * MatrixD.Invert(Bb.ShipMatrix);
            }

            Vector3D DrillFace { get { return RefWorld(RefDrill).Translation; } }

            // ---------- ticks ----------

            public void Update1()
            {
                if (!WantsUpdate1 || Fsm.State != MinerState.Dock) return;
                ResolveHome();                                   // carrier pose extrapolated at 60 Hz
                Helm.SetReference(RefInShip(_refKind));
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
                Helm.SetReference(RefInShip(_refKind));
                StateTick();
                if (!WantsUpdate1) Helm.Update(Dt10);
            }

            void Step(MinerInput input)
            {
                var from = Fsm.State;
                if (!Fsm.Step(input, S) || Fsm.State == from) return;
                _obs = new MinerInput();            // observations belonged to the state just left
                Enter(from, Fsm.State);
            }

            void Force(MinerState s, string note)
            {
                var from = Fsm.State;
                Fsm.Force(s, note);
                _obs = new MinerInput();
                if (from != s) Enter(from, s);
            }

            // ---------- home + reload ----------

            // Docked: the real connector. Undocked: the carrier's beacon, extrapolated (slice 2). Never heard a
            // beacon: last known pose (slice 1, static carrier). Heard one, now stale: home LOST.
            void ResolveHome()
            {
                var c = _r.Scan.Connector;
                var now = Bb.Time;
                Bb.HomeTracked = false;
                Bb.HomeLost = _r.Home.Lost(now);   // also while docked: it blocks the next undock
                if (c != null && c.Status == MyShipConnectorStatus.Connected && c.OtherConnector != null)
                {
                    _homeId = c.OtherConnector.EntityId;
                    Bb.HomeMatrix = c.OtherConnector.WorldMatrix;
                    Bb.HomeConnectorId = _homeId;
                    Bb.HomeVelocity = Bb.Velocity;
                    Bb.HomeAngularVelocity = Bb.AngularVelocity;
                }
                else if (_r.Home.CanDeadReckon(now))
                {
                    // Fresh: tracked. Stale but recent: extrapolated, good for steering away, never for docking.
                    MatrixD pose; Vector3D vel;
                    _r.Home.Predict(now, out pose, out vel);
                    Bb.HomeMatrix = pose;
                    Bb.HomeVelocity = vel;
                    Bb.HomeAngularVelocity = _r.Home.AngularVelocity;
                    Bb.HomeTracked = !Bb.HomeLost;
                }
                else
                {
                    Bb.HomeVelocity = Vector3D.Zero;
                    Bb.HomeAngularVelocity = Vector3D.Zero;
                }
                _r.Home.SetHome(_homeId);
                Bb.HasHome = _homeId != 0;
            }

            bool HomeMoving { get { return Bb.HomeVelocity.LengthSquared() > 0.25; } }

            // World velocity of a point that moves with the carrier (translation + rotation).
            Vector3D HomePointVel(Vector3D p)
            {
                return Bb.HomeVelocity + Vector3D.Cross(Bb.HomeAngularVelocity, p - Bb.HomeMatrix.Translation);
            }

            // Why the drone must not leave the connector now, or null. Undocking blind from a moving carrier
            // would stop the drone in WORLD space right next to the hull.
            string UndockBlocked()
            {
                if (!Bb.Connected) return null;
                if (Bb.HomeLost) return "carrier beacon lost: staying docked";
                if (!_r.Home.Heard && Bb.HomeVelocity.LengthSquared() > 0.25) return "carrier moving, no bay beacon: run Fleet.Carrier";
                return null;
            }

            // A recorded job route is only valid while the carrier is where it was when it was recorded.
            bool JobRouteValid()
            {
                return _jobPath.Count >= 2 && !HomeMoving
                    && Vector3D.Distance(Bb.HomeMatrix.Translation, _routeHome.Translation) < 5
                    && Vector3D.Dot(Bb.HomeMatrix.Forward, _routeHome.Forward) > 0.996;
            }

            static bool IsHomeward(MinerState s)
            {
                return s == MinerState.Undock || s == MinerState.DockPathOut || s == MinerState.DockPathIn
                    || s == MinerState.Dock || s == MinerState.RouteBack;
            }

            void Reconcile()
            {
                _pendingReconcile = false;
                var ri = new ReconcileInput
                {
                    Saved = _savedState, HomeFound = Bb.HasHome && !Bb.HomeLost, Connected = Bb.Connected, HasJob = _hasJob,
                    CargoFill = Bb.CargoFill, ShipSize = Bb.ShipSize,
                    DistToHoleEntrance = _hasJob && HoleIndex < _holes.Count ? Vector3D.Distance(DrillFace, Entrance()) : 0
                };
                string note; bool redoHole;   // redo is implicit: a hole only advances when a drill completes
                var state = Reconciler.Resolve(ri, S.OnReload, out note, out redoHole);
                var resume = _savedState == MinerState.Hold ? _savedResume : _savedState;
                if (resume == MinerState.Drill) resume = MinerState.Retract;   // never resume mid-rock
                Force(state, note);
                Fsm.Restore(state == MinerState.Hold ? resume : Fsm.ResumeState, _savedPending);
                if (_hasJob && !_r.Damage.HasBaseline) _r.Damage.TakeBaseline();
                _r.Events.Add(Bb.Time, _savedState, state, ReturnReason.None, note ?? "reconciled after reload");
            }

            // Re-run the reload reconciliation (REBOOT): holds controls until sensing is fresh again.
            public void Reboot()
            {
                _savedState = Fsm.State;
                _savedResume = Fsm.ResumeState;
                _savedPending = Fsm.PendingReason;
                _pendingReconcile = true;
                Bb.SensingTicks = 0;
                Helm.Release();
            }

            // ---------- transitions ----------

            void Enter(MinerState from, MinerState to)
            {
                _r.Events.Add(Bb.Time, from, to, Fsm.PendingReason, Fsm.Note);
                if (from == MinerState.Charge) _r.Energy.SetCharging(false);
                if (from == MinerState.Retract && (to == MinerState.Position || to == MinerState.RouteBack)) FinishHole();
                if ((from == MinerState.Drill || from == MinerState.Retract) && to != MinerState.Retract) _r.DrillsOn(false);
                if (from == MinerState.Recording && to != MinerState.Recording) KeepRecording();
                WantsUpdate1 = false;
                _stuckRetries = 0;
                _backoffUntil = -1;
                _stateStart = Bb.Time;
                _timedOut = false;
                if (_stuck.StuckSeconds != S.StuckSeconds) _stuck = new StuckDetector(S.StuckSeconds, 0.2);
                _stuck.Reset();

                // HOME means "come home and stay": the loop's relaunch becomes Idle.
                if (to == MinerState.Undock && from == MinerState.Charge && _stayHome)
                {
                    _stayHome = false;
                    Force(MinerState.Idle, "home by operator");
                    return;
                }

                switch (to)
                {
                    case MinerState.Idle:
                    case MinerState.Hold:
                    case MinerState.Safe:
                        Helm.Release(); _r.DrillsOn(false); break;
                    case MinerState.Recording:
                        Helm.Release(); _recorder.Begin(RecPos()); break;
                    case MinerState.Undock:
                        _r.Energy.SetCharging(false);   // never leave with batteries on Recharge / tanks on Stockpile
                        _refKind = RefConn;
                        if (_r.Scan.Connector != null) _r.Scan.Connector.Disconnect();
                        break;
                    case MinerState.DockPathOut:
                    case MinerState.DockPathIn:
                        _refKind = RefShip;
                        // Attitude held RELATIVE TO THE CARRIER: no swinging near the hull, turns with it.
                        _holdFwd = Frames.ToLocalDir(Bb.HomeMatrix, Bb.ShipMatrix.Forward);
                        _holdUp = Frames.ToLocalDir(Bb.HomeMatrix, Bb.ShipMatrix.Up);
                        _follower.StartNearest(_dockPath, to == MinerState.DockPathIn, Local(ShipPos));
                        break;
                    case MinerState.RouteOut:
                    case MinerState.RouteBack:
                        _refKind = RefShip;
                        bool back = to == MinerState.RouteBack;
                        // A recorded job route assumes the carrier is where it was: with a moving carrier the
                        // way home is a direct route to the (moving) dock approach point.
                        _useGps = _jobIsGps || !JobRouteValid();
                        if (_useGps) _gps.Start(back ? DockApproachPoint() : JobApproachPoint(), Bb.InGravity);
                        else _follower.StartNearest(_jobPath, back, ShipPos);
                        break;
                    case MinerState.Position:
                        _refKind = RefDrill; break;
                    case MinerState.Drill:
                        _refKind = RefDrill;
                        _holeDone = false;
                        _r.DrillsOn(true);
                        _drill.Start(S.Depth - S.StartDepth, S.WorkSpeed);
                        break;
                    case MinerState.Retract:
                        _refKind = RefDrill;
                        if (_drill.Current == DrillLogic.Phase.Advance) _drill.BeginRetract();
                        break;
                    case MinerState.Dock:
                        _refKind = RefConn;
                        _docking.Start(Bb.Time);
                        break;
                    case MinerState.Unload:
                        Helm.Release();
                        _manualFull = false;
                        CollectCarrierInventories();
                        break;
                    case MinerState.Charge:
                        _r.Energy.SetCharging(true); break;
                }
            }

            void FinishHole()
            {
                if (_drill.WasBlocked) _r.Note("hole blocked, skipped");
                if (_holeDone) HoleIndex = Math.Min(HoleIndex + 1, _holes.Count);
                _holeDone = false;
            }

            // Where an outbound GPS / straight route ends: standing off the first hole along the job axis,
            // so the drill face arrives in front of the rock, not the centre of mass inside it.
            Vector3D JobApproachPoint()
            {
                if (_holes.Count == 0) return _jobOrigin;
                return Entrance() - _jobFwd * (Standoff + Clearance);
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
                return HoleGrid.HoleEntrance(_jobOrigin, _jobRight, _jobUp, cell, _spacing) + _jobFwd * S.StartDepth;
            }

            // Clear of the rock in front of the current hole: Position flies here, Retract returns here.
            Vector3D HoleApproach() { return Entrance() - _jobFwd * Clearance; }

            void StateTick()
            {
                var now = Bb.Time;
                var refW = RefWorld(_refKind);
                var st = Fsm.State;
                if (_evadeUntil > 0)
                {
                    // Beacon lost close to the carrier: keep moving with it (dead-reckoned) out along the bay
                    // axis for a few seconds, THEN hold. Stopping dead next to a moving hull is the danger.
                    if (now < _evadeUntil && _r.Home.CanDeadReckon(now))
                    {
                        _homeFrame = true;
                        Engage(World(_evadeLocal), refW.Forward, refW.Up, S.ApproachSpeed, -1);
                        return;
                    }
                    _evadeUntil = -1;
                    Helm.Release();
                }
                // RouteBack keeps flying toward the last known approach point (the beacon usually returns as
                // the drone comes back into antenna range); only the close-in states stop.
                if (Bb.HomeLost && !Bb.Connected && IsHomeward(st) && st != MinerState.RouteBack)
                {
                    var pending = Fsm.PendingReason;
                    Force(MinerState.Hold, "carrier beacon lost: CONT once it is back");
                    Fsm.Restore(st, pending);
                    if (_r.Home.CanDeadReckon(now))
                    {
                        _evadeLocal = Frames.ToLocalPoint(Bb.HomeMatrix, ShipPos) + Vector3D.Forward * (2 * Standoff);
                        _evadeUntil = now + 4;
                    }
                    return;
                }
                // Home-relative targets move with the carrier; job targets are fixed in the world. Leaving the
                // carrier, its velocity is blended out over a few ship lengths so the drone never slides back.
                _homeFrame = IsHomeward(st);
                _frameVel = Vector3D.Zero;
                if (st == MinerState.RouteOut && Bb.HasHome)
                {
                    double near = Standoff + 2 * Bb.ShipSize, far = Standoff + 8 * Bb.ShipSize;
                    double f = (Vector3D.Distance(ShipPos, Bb.HomeMatrix.Translation) - near) / (far - near);
                    _frameVel = HomePointVel(ShipPos) * (1 - Math.Max(0, Math.Min(1, f)));
                }
                switch (Fsm.State)
                {
                    case MinerState.Recording:
                        if (!_recorder.Update(RecPos(), (Bb.Velocity - Bb.HomeVelocity).Length()) && _recorder.IsFull)
                            _r.Note("path recorder full");
                        break;

                    case MinerState.Undock:
                    {
                        var target = DockingPlanner.UndockTarget(Bb.HomeMatrix, Standoff);
                        Fly(target, refW.Forward, refW.Up, S.ApproachSpeed, -1);
                        _obs.UndockClear = Vector3D.Distance(refW.Translation, target) < 1;
                        break;
                    }

                    case MinerState.DockPathOut:
                    case MinerState.DockPathIn:
                    {
                        if (_dockPath.Count < 2) { _obs.PathDone = true; break; }
                        double cap, remaining;
                        var carrot = World(_follower.Update(Local(ShipPos), ReachDist, S.ApproachSpeed * 2, out cap, out remaining));
                        Fly(carrot, WorldDir(_holdFwd), WorldDir(_holdUp), Math.Min(S.ApproachSpeed * 2, cap), remaining);
                        _obs.PathDone = _follower.Done;
                        // Inbound: hand over to Dock at the standoff instead of flying into the docked pose.
                        if (Fsm.State == MinerState.DockPathIn)
                        {
                            var standoff = DockingPlanner.UndockTarget(Bb.HomeMatrix, Standoff);
                            if (Vector3D.Distance(RefWorld(RefConn).Translation, standoff) < Standoff) _obs.PathDone = true;
                        }
                        break;
                    }

                    case MinerState.RouteOut:
                    case MinerState.RouteBack:
                        if (_useGps)
                        {
                            if (Fsm.State == MinerState.RouteBack) _gps.Retarget(DockApproachPoint());
                            var carrot = _gps.Update(ShipPos, Bb.Gravity, Bb.HasElevation ? Bb.Elevation : 1e9, S.SafeAltitude, ReachDist);
                            Vector3D f, u;
                            TravelAxes(carrot, out f, out u);
                            Fly(carrot, f, u, S.MaxSpeed, _gps.Remaining);
                            _obs.RouteDone = _gps.Current == GpsRoute.Phase.Done;
                        }
                        else
                        {
                            double cap, remaining;
                            var carrot = _follower.Update(ShipPos, ReachDist, S.MaxSpeed, out cap, out remaining);
                            Vector3D f, u;
                            TravelAxes(carrot, out f, out u);
                            Fly(carrot, f, u, Math.Min(S.MaxSpeed, cap), remaining);
                            _obs.RouteDone = _follower.Done;
                        }
                        break;

                    case MinerState.Position:
                    {
                        if (HoleIndex >= _holes.Count) break;
                        var target = HoleApproach();
                        var fwd = _jobFwd;
                        Fly(target, fwd, _jobUp, S.ApproachSpeed, -1);
                        // Fresh geometry, never last tick's helm state.
                        _obs.AtHole = Vector3D.Distance(refW.Translation, target) < 0.5
                            && GyroMath.AngleBetween(refW.Forward, fwd) < 0.035;
                        break;
                    }

                    case MinerState.Drill:
                    case MinerState.Retract:
                    {
                        var entrance = Entrance();
                        var fwd = _jobFwd;
                        Depth = Vector3D.Dot(refW.Translation - entrance, fwd);
                        var phase = _drill.Update(now, Depth, Vector3D.Dot(Bb.Velocity, fwd));
                        _r.Cargo.EjectStep(S);
                        if (Fsm.State == MinerState.Drill)
                        {
                            double full = S.Depth - S.StartDepth;
                            Engage(entrance + fwd * full, fwd, _jobUp, _drill.Feed, Math.Max(0, full - Depth));
                            if (phase != DrillLogic.Phase.Advance) { _holeDone = true; _obs.DrillFinished = true; }
                        }
                        else
                        {
                            Fly(entrance - fwd * Clearance, fwd, _jobUp, S.RetractSpeed, -1);
                            // Done only once the face is clear of the rock (DrillLogic stops counting at 0.5 m).
                            _obs.RetractDone = phase == DrillLogic.Phase.Done && Depth <= -0.5 * Clearance;
                            _obs.JobComplete = HoleIndex + (_holeDone ? 1 : 0) >= _holes.Count;
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
                        if (!_timedOut && now - _stateStart > UnloadTimeout)
                        {
                            _timedOut = true;
                            if (Bb.CargoFill * 100 >= S.MaxLoad)
                            {
                                // Relaunching would only trip CargoFull again: an endless dock loop.
                                Force(MinerState.Hold, "carrier full: free space, then CONT");
                                Fsm.Restore(MinerState.Unload, Fsm.PendingReason);
                                break;
                            }
                            _r.Note("unload timeout");
                        }
                        _obs.Unloaded = !_r.Cargo.HasOre() || _timedOut;
                        break;

                    case MinerState.Charge:
                        _obs.Charged = _r.Energy.IsCharged();
                        if (!_obs.Charged && !_timedOut && now - _stateStart > ChargeTimeout)
                        {
                            _timedOut = true;
                            _r.Note("charge timeout: leaving partly charged");
                        }
                        _obs.Charged |= _timedOut;
                        var blocked = UndockBlocked();
                        if (blocked != null)
                        {
                            _obs.Charged = false;
                            if (!_undockNoted) { _r.Note(blocked); _undockNoted = true; }
                        }
                        else _undockNoted = false;
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
                // Connectors mate at any roll: keep the drone's current roll instead of the home block's Up.
                _t.Up = GyroMath.PerpendicularUp(_t.Forward, cw.Up, cw.Forward);
                _t.Velocity = HomePointVel(_t.Position);
                Helm.Engage(_t);
                WantsUpdate1 = _t.Precision;
                if (_docking.Current == DockingPlanner.Phase.Connect && Bb.Connectable) conn.Connect();
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
                    fwd = d.LengthSquared() > 1 ? d : m.Forward - up * Vector3D.Dot(m.Forward, up);
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
            void Fly(Vector3D pos, Vector3D fwd, Vector3D up, double cap, double brake)
            {
                var refPos = RefWorld(_refKind).Translation;
                if (Bb.Time < _backoffUntil) { Engage(_backoffHome ? World(_backoffTarget) : _backoffTarget, fwd, up, S.ApproachSpeed, -1); return; }
                if (_backoffUntil > 0) { _backoffUntil = -1; _stuck.Reset(); }
                Engage(pos, fwd, up, cap, brake);
                var toTarget = pos - refPos;
                double dist = toTarget.Length();
                double progress = dist > 1e-6 ? Vector3D.Dot(Bb.Velocity - _t.Velocity, toTarget / dist) : 0;
                if (!_stuck.Update(Bb.Time, progress, dist > 1.0)) return;
                if (_stuckRetries < 2)
                {
                    _stuckRetries++;
                    _backoffTarget = refPos - (dist > 1e-6 ? toTarget / dist : fwd) * 3;
                    _backoffHome = _homeFrame;
                    if (_homeFrame) _backoffTarget = Local(_backoffTarget);
                    _backoffUntil = Bb.Time + BackoffSeconds;
                    _r.Note("stuck, backing off");
                }
                else _obs.Stuck = true;
            }

            void Engage(Vector3D pos, Vector3D fwd, Vector3D up, double cap, double brake)
            {
                _t.Position = pos; _t.Forward = fwd; _t.Up = up; _t.SpeedCap = cap; _t.BrakeDist = brake; _t.Precision = false;
                _t.Velocity = _homeFrame ? HomePointVel(pos) : _frameVel;
                Helm.Engage(_t);
            }

            // ---------- commands ----------

            public void Command(Cmd cmd, string rest)
            {
                var st = Fsm.State;
                bool parked = st == MinerState.Idle || st == MinerState.Hold;
                switch (cmd)
                {
                    case Cmd.SetJob: SetJobHere(); break;
                    case Cmd.Goto: SetJobGps(rest); break;
                    case Cmd.RecordDock:
                    case Cmd.RecordJob:
                        if (!Bb.HasHome) { _r.Note("dock first: paths are recorded relative to home"); return; }
                        if (parked) _recording = cmd == Cmd.RecordDock ? 1 : 2;
                        break;
                    case Cmd.Full: _manualFull = true; break;
                    case Cmd.Next:
                    case Cmd.Prev:
                        // Never while a hole is being cut or pulled out of: the target would jump through rock.
                        if (!parked && st != MinerState.Position) { _r.Note("NEXT/PREV: stop or wait for Position"); return; }
                        HoleIndex = cmd == Cmd.Next ? Math.Min(HoleIndex + 1, _holes.Count) : Math.Max(HoleIndex - 1, 0);
                        return;
                    case Cmd.Reboot: _r.Rescan(); Reboot(); return;
                    case Cmd.GyroTest:
                        if (parked && !Bb.Connected) _r.Ship.BeginGyroTest(Bb.Time);
                        else _r.Note("GYROTEST: Idle/Hold and undocked only");
                        return;
                    case Cmd.Home: _stayHome = st != MinerState.Idle; break;
                    case Cmd.Start:
                    case Cmd.Cont:
                        _stayHome = false; break;
                    case Cmd.Reset:
                        _stayHome = false;
                        _evadeUntil = -1;
                        _r.Home.Forget();   // carrier script gone for good: fall back to a static home
                        break;
                }
                if ((cmd == Cmd.Start || cmd == Cmd.Cont) && st == MinerState.Idle && UndockBlocked() != null)
                {
                    _r.Note(UndockBlocked());
                    return;
                }
                if ((cmd == Cmd.Start || cmd == Cmd.Cont) && st == MinerState.Idle && !_r.Scan.Ready)
                {
                    _r.Note("not ready: " + _r.Scan.Diagnostics[0]);
                    return;
                }
                var input = new MinerInput
                {
                    Command = cmd, HasJob = _hasJob, HasDockPath = _dockPath.Count > 1, Connected = Bb.Connected,
                    JobComplete = HoleIndex >= _holes.Count
                };
                Step(input);
            }

            // Leaving Recording (STOPREC, STOP, RESET): close the path and keep it as the dock path or job route.
            void KeepRecording()
            {
                _recorder.End(RecPos());
                if (_recording == 0) return;
                var path = _recording == 1 ? _dockPath : _jobPath;
                path.Clear();
                path.AddRange(_recorder.Points);
                if (_recording == 2) _routeHome = Bb.HomeMatrix;
                _r.Note(_recording == 1 ? "dock path saved" : "job route saved");
                _recording = 0;
            }

            Vector3D RecPos() { return _recording == 2 ? ShipPos : Local(ShipPos); }

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
                var m = RefWorld(RefDrill);
                DefineJob(m.Translation, m.Forward, m.Up, false);
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
                DefineJob(target, fwd, up, true);
                _r.Note("GPS job set");
            }

            void DefineJob(Vector3D origin, Vector3D fwd, Vector3D up, bool gps)
            {
                _jobOrigin = origin;
                _jobFwd = fwd;
                _jobUp = up;
                _jobRight = Vector3D.Cross(fwd, up);
                _spacing = HoleGrid.SpacingFromDrills(_r.Scan.DrillLocal);
                _jobW = S.Width; _jobH = S.Height;
                HoleGrid.Spiral(_jobW, _jobH, _holes);
                HoleIndex = 0;
                _jobIsGps = gps;
                _hasJob = true;
                _r.Damage.TakeBaseline();
            }

            // ---------- Storage ----------

            public void Save(MyIni ini)
            {
                var n = Name;
                ini.Set(n, "state", (int)Fsm.State);
                ini.Set(n, "resume", (int)Fsm.ResumeState);
                ini.Set(n, "pending", (int)Fsm.PendingReason);
                ini.Set(n, "note", Fsm.Note ?? "");
                ini.Set(n, "holeIndex", HoleIndex);
                ini.Set(n, "jobIsGps", _jobIsGps);
                ini.Set(n, "hasJob", _hasJob);
                ini.Set(n, "stayHome", _stayHome);
                SetVec(ini, n, "o", _jobOrigin); SetVec(ini, n, "f", _jobFwd);
                SetVec(ini, n, "u", _jobUp); SetVec(ini, n, "r", _jobRight);
                ini.Set(n, "spacing", _spacing);
                ini.Set(n, "width", _jobW);
                ini.Set(n, "height", _jobH);
                ini.Set(n, "homeId", _homeId);
                ini.Set(n, "tracked", _r.Home.Heard);
                SetVec(ini, n, "rp", _routeHome.Translation); SetVec(ini, n, "rf", _routeHome.Forward); SetVec(ini, n, "ru", _routeHome.Up);
                var h = Bb.HomeMatrix;
                SetVec(ini, n, "hp", h.Translation); SetVec(ini, n, "hf", h.Forward); SetVec(ini, n, "hu", h.Up);
                PathCodec.Save(ini, "Miner.DockPath", _dockPath);
                PathCodec.Save(ini, "Miner.JobPath", _jobPath);
            }

            public bool Load(MyIni ini, int savedVersion)
            {
                if (savedVersion != StorageVersion && savedVersion != 2) return false;
                var n = Name;
                _savedState = (MinerState)ini.Get(n, "state").ToInt32(0);
                _savedResume = (MinerState)ini.Get(n, "resume").ToInt32(0);
                _savedPending = (ReturnReason)ini.Get(n, "pending").ToInt32(0);
                HoleIndex = ini.Get(n, "holeIndex").ToInt32(0);
                _jobIsGps = ini.Get(n, "jobIsGps").ToBoolean(false);
                _hasJob = ini.Get(n, "hasJob").ToBoolean(false);
                _stayHome = ini.Get(n, "stayHome").ToBoolean(false);
                _jobOrigin = GetVec(ini, n, "o"); _jobFwd = GetVec(ini, n, "f");
                _jobUp = GetVec(ini, n, "u"); _jobRight = GetVec(ini, n, "r");
                _spacing = ini.Get(n, "spacing").ToDouble(3);
                _jobW = ini.Get(n, "width").ToInt32(1);
                _jobH = ini.Get(n, "height").ToInt32(1);
                _homeId = ini.Get(n, "homeId").ToInt64(0);
                // Accept the home bay's beacon from the first tick. A home that WAS tracked is lost until a
                // fresh beacon arrives: its saved pose is not where a moving carrier is now.
                if (ini.Get(n, "tracked").ToBoolean(false)) _r.Home.AssumeLost(_homeId);
                else _r.Home.SetHome(_homeId);
                var rp = GetVec(ini, n, "rp"); var rf = GetVec(ini, n, "rf"); var ru = GetVec(ini, n, "ru");
                if (rf.LengthSquared() > 0.5 && ru.LengthSquared() > 0.5) _routeHome = MatrixD.CreateWorld(rp, rf, ru);
                var hp = GetVec(ini, n, "hp"); var hf = GetVec(ini, n, "hf"); var hu = GetVec(ini, n, "hu");
                if (hf.LengthSquared() > 0.5 && hu.LengthSquared() > 0.5) Bb.HomeMatrix = MatrixD.CreateWorld(hp, hf, hu);
                Bb.HasHome = _homeId != 0;
                if (_hasJob) HoleGrid.Spiral(_jobW, _jobH, _holes);
                if (HoleIndex > _holes.Count) HoleIndex = _holes.Count;
                PathCodec.Load(ini, "Miner.DockPath", _dockPath);
                PathCodec.Load(ini, "Miner.JobPath", _jobPath);
                if (savedVersion == 2) { MigrateJobToWorld(Bb.HomeMatrix); _routeHome = Bb.HomeMatrix; }
                return true;
            }

            // Version 2 stored the job and job route local to the home connector; version 3 stores them in
            // world space. Converted with the home pose saved alongside them (spec §4: migrate, don't drop).
            void MigrateJobToWorld(MatrixD home)
            {
                _jobOrigin = Frames.ToWorldPoint(home, _jobOrigin);
                _jobFwd = Frames.ToWorldDir(home, _jobFwd);
                _jobUp = Frames.ToWorldDir(home, _jobUp);
                _jobRight = Frames.ToWorldDir(home, _jobRight);
                for (int i = 0; i < _jobPath.Count; i++) _jobPath[i] = Frames.ToWorldPoint(home, _jobPath[i]);
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
