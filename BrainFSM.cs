using Sandbox.ModAPI.Ingame;
using System;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public enum FleetState
        {
            Idle, Docked, Undocking, Transit, Working, RequestDock, Approach, FinalDock, Carrier
        }

        /// <summary>
        /// Decides what the grid is doing. Drones (Miner/Hauler) run the mining cycle
        /// (this file); a Carrier runs flight control (BrainFSM.Carrier.cs). Commands,
        /// self-test, status text and persistence live in BrainFSM.Io.cs.
        /// </summary>
        public partial class BrainFSM : ISubsystem
        {
            public static readonly string[] StateNames =
            {
                "Idle", "Docked", "Undocking", "Transit", "Working", "RequestDock", "Approach", "FinalDock", "Carrier"
            };

            /// <summary>Radio-style labels for HUD markers and screens.</summary>
            public static readonly string[] StateLabels =
            {
                "IDLE", "DOCKED", "LAUNCHING", "EN ROUTE", "MINING", "RTB", "INBOUND", "DOCKING", "FLIGHT CONTROL"
            };

            /// <summary>Hooks, by state: a timer or sound block named "... [FM Docked]" fires on entering Docked.</summary>
            static readonly string[] StateTags =
            {
                "[FM Idle]", "[FM Docked]", "[FM Undocking]", "[FM Transit]", "[FM Working]",
                "[FM RequestDock]", "[FM Approach]", "[FM FinalDock]", "[FM Carrier]"
            };
            const string HookDistress = "[FM Distress]", HookBackout = "[FM Backout]";
            const string HookRich = "[FM Rich]", HookLaunch = "[FM Launch]", HookHolding = "[FM Holding]";

            static readonly Color[] StateColor =
            {
                new Color(160, 160, 160), new Color(0, 200, 60), new Color(0, 180, 255), new Color(0, 180, 255),
                new Color(255, 255, 230), new Color(255, 200, 0), new Color(255, 200, 0), new Color(0, 200, 60), Color.Black
            };
            static readonly float[] StateBlink = { 0, 0, 1, 0, 0, 0, 0, 0.5f, 0 };
            static readonly Color Amber = new Color(255, 120, 0), Red = new Color(255, 0, 0);

            const double DockLease = 15;         // s a carrier keeps beaconing after the last DockRequest
            const double RequestInterval = 3;    // s between DockRequests (also renews the lease)
            const double DenyMemory = 10;        // s a DockDeny keeps us in the holding pattern
            const double CarrierMemory = 30;     // s a carrier position report stays usable
            const double LaunchAskInterval = 8;  // s between LaunchRequests
            const double LaunchGiveUp = 30;      // s without clearance before launching anyway
            const double SiteSyncGrace = 5;      // s to collect a shared site's map before picking a shaft
            const int MaxCrumbs = 32;

            // Static notes only: assigning a literal never allocates.
            const string NoteNoSite = "No site. Aim at the rock face, run 'setsite'";
            const string NoteDenied = "Docks full, holding";
            const string NoteUnknown = "Unknown command";
            const string NoteKnockedLoose = "Lost connector lock";
            const string NoteNoLink = "Lost carrier beacon, re-requesting";
            const string NoteStalled = "Shaft blocked, skipping it";
            const string NoteSiteDone = "Site exhausted. 'setsite' a new one";
            const string NoteDamaged = "Drill damaged, returning for repair";
            const string NoteHeavy = "Too heavy to climb, returning";
            const string NoteWeak = "Can't lift even when empty: add thrust";
            const string NoteLowPower = "Power low, returning";
            const string NoteFull = "Hold full, returning";
            const string NoteHostile = "MAYDAY: hostile contact, returning";
            const string NoteHullDamage = "MAYDAY: hull damage, returning";
            const string NoteBarren = "Barren rock, moving on";

            // Log lines (who is the speaker; these are the words).
            const string LogLaunch = "launching";
            const string LogOnStation = "on station, shaft";
            const string LogRtbFull = "hold full, RTB";
            const string LogRtbPower = "power low, RTB";
            const string LogRich = "rich vein, shaft";
            const string LogBarren = "barren, abandoning shaft";
            const string LogBlocked = "shaft blocked";
            const string LogDocked = "docked";
            const string LogDelivered = "delivered";
            const string LogExhausted = "site exhausted";
            const string LogMayday = "MAYDAY";
            const string LogCleared = "cleared for launch, T-";
            const string LogAdopted = "assigned to site";
            const string Kg = " kg", Sec = " s";

            readonly Program _p;
            readonly Config _cfg;
            readonly GridManager _grid;
            readonly CommsOfficer _comms;
            readonly HelmController _helm;

            public FleetState State { get; private set; }
            public string Note { get { return _note; } }
            public bool AutoCycle { get { return _autoCycle; } }
            double _enteredAt;
            bool _autoCycle;
            string _note;

            // Home: carrier address, slot, and the latest dock pose + carrier velocity.
            long _carrierAddr;
            int _slot = -1;
            Vector3D _dockPos, _dockFwd, _dockUp, _carrierVel;
            double _dockStamp = double.MinValue;
            double _lastRequestAt = double.MinValue;
            // Carrier position from its Status broadcasts (for holding, energy and ETA).
            Vector3D _carrierPos, _carrierPosVel;
            double _carrierSeen = double.MinValue;
            // Dock queue / holding pattern.
            public int QueuePlace { get; private set; }
            double _deniedAt = double.MinValue;
            Vector3D _holdDir;
            // Launch clearance.
            double _launchAt = -1, _launchAskedAt = double.MinValue, _launchFirstAsk = -1;

            // Work site. Asteroids never move, so the world frame IS the site's frame.
            public readonly SiteMap Site = new SiteMap();
            public int SiteId { get; private set; } = -1;
            public int Shaft { get; private set; } = -1;
            int _entryShaft = -1;
            Vector3D _entry;
            bool _staged;
            double _siteAdoptedAt = double.MinValue;
            // A site offered while we were mid-shaft, applied once we leave the shaft.
            bool _adoptPending;
            int _adoptId;
            Vector3D _adoptPos, _adoptFwd, _adoptUp, _adoptSpacing;

            // Current shaft. Depths are along the site forward from the shaft entry, measured at
            // the reference block. They persist, so a shaft interrupted by a full hold is resumed.
            public bool FaceKnown { get { return _faceKnown; } }
            bool _faceKnown;
            double _faceDepth;       // reference depth at which the drills touch rock
            double _minedDepth;      // deepest point reached so far (already a clear hole)
            double _depth;           // commanded depth (the carrot)
            bool _retracting, _shaftDone, _recall, _scanPending, _sessionOpen;
            int _shaftResult;
            double _progressAt, _shaftBase;
            long _lastCargoVolume;
            readonly double[] _oreAtStart = new double[Ore.Count];

            // Undock: captured at the moment of release, extrapolated with the carrier's velocity.
            Vector3D _undockOrigin, _undockDir, _undockVel, _undockFwd, _undockUp;

            // Breadcrumbs: the outbound path, flown in reverse on the way home.
            readonly Vector3D[] _crumbs = new Vector3D[MaxCrumbs];
            public int CrumbCount { get; private set; }
            double _crumbSpacing;

            // Energy model: fraction of battery / hydrogen burned per metre flown, learned outbound.
            public double BattPerMetre { get; private set; }
            public double H2PerMetre { get; private set; }
            bool _learning;
            double _learnBatt, _learnH2, _learnDist;
            Vector3D _learnLast;

            // Self-preservation.
            public int DistressReason { get; private set; }
            double _integrityBaseline = 1, _distressSentAt = double.MinValue;

            // Production: ore on board when we docked, and lifetime deliveries.
            readonly double[] _oreAtDock = new double[Ore.Count];
            public readonly double[] Delivered = new double[Ore.Count];
            double _statsBase; // seconds of stats time from previous sessions

            // HUD markers, built once (callsign + label).
            readonly string[] _hud = new string[StateLabels.Length];
            string _hudMayday, _hudHolding;

            bool IsCarrier { get { return _cfg.Role == FleetRole.Carrier; } }
            bool IsMiner { get { return _cfg.Role == FleetRole.Miner; } }
            public double StatsSeconds { get { return _statsBase + _p.Clock; } }

            public BrainFSM(Program p, GridManager grid, CommsOfficer comms, HelmController helm)
            {
                _p = p;
                _cfg = p.Cfg;
                _grid = grid;
                _comms = comms;
                _helm = helm;
                for (int i = 0; i < _library.Length; i++)
                    _library[i] = new SiteMap();
            }

            public void Initialize()
            {
                for (int i = 0; i < _hud.Length; i++)
                    _hud[i] = _cfg.Name + " | " + StateLabels[i];
                _hudMayday = _cfg.Name + " | MAYDAY";
                _hudHolding = _cfg.Name + " | HOLDING";

                State = IsCarrier ? FleetState.Carrier : FleetState.Idle;
                _enteredAt = _p.Clock;
                _grid.SetHud(_hud[(int)State]);
                _grid.SetLights(StateColor[(int)State], StateBlink[(int)State]);
            }

            // =================================================================
            // Transitions
            // =================================================================

            void Enter(FleetState next)
            {
                if (State == FleetState.Working && _sessionOpen)
                    CloseSession();

                State = next;
                _enteredAt = _p.Clock;

                // Every state except Docked flies (or may need dampeners), so thrusters must be live.
                if (next != FleetState.Docked)
                    _grid.SetDockedMode(false);
                if (next != FleetState.Working)
                    _grid.SetEjecting(false);

                switch (next)
                {
                    case FleetState.Idle:
                        _grid.SetDrills(false);
                        _helm.Disengage();
                        break;

                    case FleetState.Docked:
                        _grid.SetDrills(false);
                        _helm.Disengage();
                        _grid.SetDockedMode(true);
                        DistressReason = 0;
                        _launchAt = -1;
                        _launchFirstAsk = -1;
                        _launchAskedAt = double.MinValue;
                        Array.Copy(_grid.OreKg, _oreAtDock, Ore.Count);
                        _p.Log.Add(_cfg.Name, LogDocked);
                        break;

                    case FleetState.Undocking:
                        _note = null; // keep the reason we came home visible until the next trip
                        ReportDelivery();
                        _integrityBaseline = _grid.Integrity;
                        CrumbCount = 0;
                        _crumbSpacing = _cfg.CrumbSpacing;
                        _p.Log.Add(_cfg.Name, LogLaunch);
                        BeginUndock();
                        break;

                    case FleetState.Transit:
                        _grid.SetDrills(false);
                        _staged = InFrontOfFace();
                        break;

                    case FleetState.Working:
                        if (IsMiner) OpenSession(); // haulers only hold station; they never touch the map
                        _grid.SetDrills(IsMiner);
                        break;

                    case FleetState.RequestDock:
                        _grid.SetDrills(false);
                        _helm.HoldPosition();
                        _deniedAt = double.MinValue;
                        SendDockRequest();
                        break;
                }

                _grid.FireHooks(StateTags[(int)next]);
                RefreshSignals();
            }

            /// <summary>HUD marker and status lights for the current state (distress overrides).</summary>
            void RefreshSignals()
            {
                if (DistressReason != 0)
                {
                    _grid.SetHud(_hudMayday);
                    _grid.SetLights(Red, 0.4f);
                }
                else if (State == FleetState.RequestDock && Holding)
                {
                    _grid.SetHud(_hudHolding);
                    _grid.SetLights(StateColor[(int)State], 2f);
                }
                else
                {
                    _grid.SetHud(_hud[(int)State]);
                    _grid.SetLights(StateColor[(int)State], StateBlink[(int)State]);
                }
            }

            bool Holding { get { return _p.Clock - _deniedAt < DenyMemory; } }

            void BeginUndock()
            {
                var ctrl = _grid.Controller;
                var conn = _grid.Connector;
                MatrixD refM = ctrl.WorldMatrix;

                // Everything is relative to the carrier: its velocity at release and our
                // connector's axis. No absolute waypoint is ever stored.
                _undockVel = ctrl.GetShipVelocities().LinearVelocity;
                _undockOrigin = refM.Translation;
                _undockDir = conn.WorldMatrix.Backward; // away from the mating face
                _undockFwd = refM.Forward;
                _undockUp = refM.Up;

                _grid.Disconnect();
                if (_carrierAddr != 0)
                    _comms.Unicast(_carrierAddr, Op.DockRelease, _slot);
            }

            // =================================================================
            // Ticks
            // =================================================================

            public void Update10()
            {
                if (IsCarrier)
                {
                    StreamBeacons();
                    return;
                }

                if (_grid.Problem != null && State != FleetState.Idle && State != FleetState.Docked)
                {
                    _note = _grid.Problem;
                    Enter(FleetState.Idle);
                    return;
                }

                switch (State)
                {
                    case FleetState.Undocking: TickUndocking(); break;
                    case FleetState.Transit: TickTransit(); break;
                    case FleetState.Working: TickWorking(); break;
                    case FleetState.RequestDock: TickRequestDock(); break;
                    case FleetState.Approach: TickApproach(); break;
                    case FleetState.FinalDock: TickFinalDock(); break;
                }
            }

            public void Update100()
            {
                if (IsCarrier)
                {
                    CarrierUpdate100();
                    return;
                }

                switch (State)
                {
                    case FleetState.Docked:
                        if (!_grid.IsConnected)
                        {
                            _note = NoteKnockedLoose;
                            Enter(FleetState.Idle);
                        }
                        else
                        {
                            _grid.Unload();
                            TryLaunch();
                        }
                        break;

                    case FleetState.Working:
                        if (_grid.Controller == null || !_sessionOpen || !IsMiner) break;
                        // No camera fix: the first cargo gain means the drills have reached rock.
                        long volume = _grid.CargoVolume;
                        if (!_faceKnown && !_retracting && volume > _lastCargoVolume)
                        {
                            _faceKnown = true;
                            _faceDepth = Math.Max(0, CurrentDepth() - _cfg.MineSpeed * 2);
                        }
                        _lastCargoVolume = volume;
                        CheckBarren();
                        // Dump stone only once in rock, so the contact signal above is never masked.
                        _grid.SetEjecting(_faceKnown && !_retracting && _grid.CanEject);
                        break;

                    // RequestDock retries; Approach/FinalDock renew the carrier's beacon lease.
                    case FleetState.RequestDock:
                    case FleetState.Approach:
                    case FleetState.FinalDock:
                        if (_p.Clock - _lastRequestAt >= RequestInterval)
                            SendDockRequest();
                        break;
                }

                if (State != FleetState.Idle && State != FleetState.Docked)
                    WatchForDanger();

                var ctrl = _grid.Controller;
                _comms.Broadcast(Op.Status, (int)State,
                    new Vector3D(_grid.CargoFill, _grid.BatteryCharge, _grid.HydrogenFill),
                    ctrl != null ? ctrl.GetPosition() : Vector3D.Zero,
                    new Vector3D(SiteId, Shaft, DistressReason),
                    ctrl != null ? ctrl.GetShipVelocities().LinearVelocity : Vector3D.Zero);
            }

            void TickUndocking()
            {
                double t = _p.Clock - _enteredAt;
                Vector3D target = _undockOrigin + _undockVel * t + _undockDir * _cfg.ApproachDistance;
                _helm.SetTarget(target, _undockVel, _undockFwd, _undockUp, _cfg.ApproachSpeed);

                if (_helm.DistanceToTarget < 3)
                {
                    if (!Site.Defined)
                    {
                        Enter(FleetState.Idle);
                        return;
                    }
                    StartLearning();
                    Enter(FleetState.Transit);
                }
            }

            void TickTransit()
            {
                if (_recall || NeedsHome())
                {
                    GoHome();
                    return;
                }
                var ctrl = _grid.Controller;
                Vector3D pos = ctrl.GetPosition();

                // Shared site just adopted: give the carrier a moment to send its map.
                if (Shaft < 0 && _p.Clock - _siteAdoptedAt > SiteSyncGrace && !PickShaft())
                {
                    SiteExhausted();
                    return;
                }
                Vector3D entry = Shaft >= 0 ? ShaftEntry() : Site.Pos;

                if (!_staged)
                {
                    // Arrive square-on to the face from a staging point out in clear space,
                    // so the final leg never cuts across the rock. Long legs go to the vanilla
                    // autopilot for its collision avoidance; the site is static, so that is safe.
                    Learn(pos);
                    DropCrumb(pos);
                    Vector3D stage = entry - Site.Fwd * _cfg.ApproachDistance;
                    double far = Vector3D.Distance(pos, stage);
                    if (_cfg.Autopilot && far > _cfg.AutopilotRange && _helm.AutopilotTo(stage, _cfg.MaxSpeed))
                        return;

                    _helm.SetTarget(stage, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.MaxSpeed);
                    if (_helm.DistanceToTarget < 3 && _helm.RelativeSpeed < 1)
                    {
                        _staged = true;
                        FinishLearning();
                    }
                    return;
                }

                if (Shaft < 0)
                {
                    _helm.SetTarget(entry - Site.Fwd * _cfg.ApproachDistance, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                    return;
                }
                _helm.SetTarget(entry, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                if (_helm.DistanceToTarget < 1 && _helm.RelativeSpeed < 0.5 && _helm.AlignmentError < 0.05)
                    Enter(FleetState.Working);
            }

            /// <summary>
            /// One shaft: glide through open space and already-cut hole, drill at MineSpeed from the
            /// rock face to MineDepth past it, then back straight out along the shaft axis.
            /// </summary>
            void TickWorking()
            {
                Vector3D entry = ShaftEntry();
                bool goHome = _recall || NeedsHome();

                if (!IsMiner)
                {
                    // Hauler: hold station at the site until someone fills the hold.
                    _helm.SetTarget(entry, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                    if (goHome) GoHome();
                    return;
                }

                double depth = CurrentDepth();
                if (depth > _minedDepth) _minedDepth = depth;
                if (_scanPending) ScanFace(entry);

                if (!_retracting)
                {
                    double end = EndDepth();
                    if (goHome)
                        Retract(false, SiteMap.Partial);
                    else if (_depth >= end)
                        Retract(true, _faceKnown ? SiteMap.Done : SiteMap.Empty); // off the rock's edge if no face
                    else if (_p.Clock - _progressAt > _cfg.StallTime)
                    {
                        _note = NoteStalled;
                        Retract(true, SiteMap.Blocked);
                    }
                    else if (_helm.DistanceToTarget < 1.0) // only advance once the drills have caught up
                    {
                        _progressAt = _p.Clock;
                        double glide = GlideDepth();
                        _depth = _depth < glide
                            ? Math.Min(glide, end)
                            : Math.Min(_depth + _cfg.MineSpeed * _p.Dt10, end);
                    }
                }

                if (_retracting)
                {
                    // Back out along the axis; heading home, keep going until clear of the face.
                    Vector3D exit = goHome ? entry - Site.Fwd * _cfg.ApproachDistance : entry;
                    bool inRock = depth > (_faceKnown ? _faceDepth - _cfg.FaceMargin : 0);
                    double speed = inRock ? _cfg.MineSpeed * 3 : _cfg.ApproachSpeed;
                    _helm.SetTarget(exit, Vector3D.Zero, Site.Fwd, Site.Up, speed);
                    if (_helm.DistanceToTarget < 1.5)
                    {
                        CloseSession();
                        if (goHome) GoHome();
                        else Enter(FleetState.Transit);
                    }
                    return;
                }

                // Far behind the carrot means we are gliding, not cutting.
                double limit = _helm.DistanceToTarget > 2 ? _cfg.ApproachSpeed : _cfg.MineSpeed;
                _helm.SetTarget(entry + Site.Fwd * _depth, Vector3D.Zero, Site.Fwd, Site.Up, limit);
            }

            void TickRequestDock()
            {
                // Docks full: wait in a holding ring around the carrier instead of where we stopped,
                // staggered by queue place so holding drones never share a spot.
                if (!Holding || !CarrierKnown()) return;
                var ctrl = _grid.Controller;
                double r = _cfg.HoldDistance + 30 * Math.Max(0, QueuePlace - 1);
                Vector3D target = CarrierNow() + _holdDir * r;
                _helm.SetTarget(target, _carrierPosVel, -_holdDir, ctrl.WorldMatrix.Up, _cfg.MaxSpeed);
            }

            void TickApproach()
            {
                if (_p.Clock - _dockStamp > _cfg.BeaconTimeout)
                {
                    _note = NoteNoLink;
                    Enter(FleetState.RequestDock);
                    return;
                }

                if (FollowCrumbs()) return;

                Vector3D pos, fwd, up;
                SolveDockPose(_cfg.ApproachDistance, out pos, out fwd, out up);
                double speed = _helm.DistanceToTarget > _cfg.ApproachDistance * 2 ? _cfg.MaxSpeed : _cfg.ApproachSpeed;
                _helm.SetTarget(pos, _carrierVel, fwd, up, speed);

                if (_helm.DistanceToTarget < 2 && _helm.RelativeSpeed < 1 && _helm.AlignmentError < 0.05)
                    Enter(FleetState.FinalDock);
            }

            void TickFinalDock()
            {
                if (_grid.IsConnected)
                {
                    Enter(FleetState.Docked);
                    return;
                }
                if (_grid.IsConnectable)
                {
                    _grid.Connect();
                    return;
                }
                if (_p.Clock - _enteredAt > _cfg.DockTimeout || _p.Clock - _dockStamp > _cfg.BeaconTimeout)
                {
                    Enter(FleetState.Approach); // back out along the dock axis and try again
                    return;
                }

                Vector3D pos, fwd, up;
                SolveDockPose(_cfg.DockGap, out pos, out fwd, out up);
                _helm.SetTarget(pos, _carrierVel, fwd, up, _cfg.DockSpeed);
            }

            // =================================================================
            // Mining: shaft sessions, ore yield, face finding
            // =================================================================

            bool PickShaft()
            {
                Shaft = Site.Pick(_p.IGC.Me, _p.Clock, _cfg.ClaimTimeout);
                ResetShaft();
                return Shaft >= 0;
            }

            void SiteExhausted()
            {
                _note = NoteSiteDone;
                _p.Log.Add(_cfg.Name, LogExhausted);
                _autoCycle = false;
                GoHome();
            }

            /// <summary>Entering Working: snapshot ore, claim the shaft, start face finding.</summary>
            void OpenSession()
            {
                _sessionOpen = true;
                _depth = 0;
                _retracting = false;
                _shaftDone = false;
                _progressAt = _p.Clock;
                _lastCargoVolume = _grid.CargoVolume;
                _scanPending = !_faceKnown && _grid.Camera != null;
                _shaftBase = Site.Value[Shaft];
                Array.Copy(_grid.OreKg, _oreAtStart, Ore.Count);
                Site.Apply(Shaft, SiteMap.Claimed, _shaftBase, _p.IGC.Me, _p.Clock);
                ReportShaft(SiteMap.Claimed);
                _p.Log.Add(_cfg.Name, LogOnStation, Shaft);
            }

            /// <summary>Leaving Working: record what this shaft yielded and tell the fleet.</summary>
            void CloseSession()
            {
                _sessionOpen = false;
                double mean = Site.MeanValue();
                int status = _shaftDone ? _shaftResult : SiteMap.Partial;
                Site.Apply(Shaft, status, ShaftValue(), _p.IGC.Me, _p.Clock);
                ReportShaft(status);

                if (status == SiteMap.Done && Site.IsRich(Shaft, mean))
                {
                    _p.Log.Add(_cfg.Name, LogRich, Shaft);
                    _grid.FireHooks(HookRich);
                }
                else if (status == SiteMap.Barren) _p.Log.Add(_cfg.Name, LogBarren, Shaft);
                else if (status == SiteMap.Blocked) _p.Log.Add(_cfg.Name, LogBlocked, Shaft);

                // Never changes state itself (it runs inside Enter): an exhausted site leaves
                // Shaft = -1, which TickTransit turns into the trip home.
                if (_adoptPending) ApplyAdoption();
                else if (_gotoShaft >= 0)
                {
                    Shaft = _gotoShaft;
                    _gotoShaft = -1;
                    ResetShaft();
                }
                else if (_shaftDone) PickShaft();
            }

            void Retract(bool done, int result)
            {
                _retracting = true;
                _shaftDone = done;
                _shaftResult = result;
                _grid.SetEjecting(false);
                _grid.FireHooks(HookBackout);
                if (DistressReason == 0) _grid.SetLights(Amber, 1f);
            }

            /// <summary>Weighted ore mined in this shaft: preferred ores count 5x, stone not at all.</summary>
            double ShaftValue()
            {
                double v = _shaftBase;
                for (int i = 1; i < Ore.Count; i++)
                {
                    double mined = _grid.OreKg[i] - _oreAtStart[i];
                    if (mined > 0) v += mined * (_cfg.PreferOre[i] ? 5 : 1);
                }
                return v;
            }

            /// <summary>BarrenDepth metres into rock with no ore at all: give the shaft up.</summary>
            void CheckBarren()
            {
                if (_retracting || !_faceKnown || _cfg.BarrenDepth <= 0) return;
                if (CurrentDepth() - _faceDepth < _cfg.BarrenDepth || ShaftValue() >= 1) return;
                _note = NoteBarren;
                Retract(true, SiteMap.Barren);
            }

            void ReportShaft(int status)
            {
                if (SiteId < 0) return;
                _comms.Broadcast(Op.ShaftReport, SiteId, new Vector3D(Shaft, status, Site.Value[Shaft]));
            }

            /// <summary>Raycast the face once per shaft, as soon as the camera is aimed down the axis.</summary>
            void ScanFace(Vector3D entry)
            {
                if (_faceKnown)
                {
                    _scanPending = false;
                    return;
                }
                if (_helm.AlignmentError > 0.05) return; // wait until the camera looks down the axis
                bool hit;
                Vector3D point;
                if (!_grid.TryScanRock(_cfg.ScanRange, out hit, out point)) return; // camera charging
                _scanPending = false;
                if (!hit) return; // fall back to contact detection
                _faceKnown = true;
                _faceDepth = Math.Max(0, Vector3D.Dot(point - entry, Site.Fwd) - _grid.DrillReach);
            }

            /// <summary>Depth we can cover at ApproachSpeed: open space before the face, plus cut hole.</summary>
            double GlideDepth()
            {
                double glide = _minedDepth - 0.5;
                if (_faceKnown) glide = Math.Max(glide, _faceDepth - _cfg.FaceMargin);
                return Math.Max(0, glide);
            }

            /// <summary>Bottom of the shaft. Until the face is found, ScanRange of open space ends it.</summary>
            double EndDepth()
            {
                return _faceKnown ? _faceDepth + _cfg.MineDepth : _cfg.ScanRange;
            }

            double CurrentDepth()
            {
                return Vector3D.Dot(_grid.Controller.GetPosition() - ShaftEntry(), Site.Fwd);
            }

            /// <summary>Metres cut past the face on the current shaft (for screens).</summary>
            public double CutDepth { get { return _faceKnown ? Math.Max(0, _depth - _faceDepth) : 0; } }

            void ResetShaft()
            {
                _faceKnown = false;
                _faceDepth = 0;
                _minedDepth = 0;
                _shaftDone = false;
                _entryShaft = -1;
            }

            /// <summary>Entry point of the current shaft (cached; shafts are fixed once a site exists).</summary>
            Vector3D ShaftEntry()
            {
                if (_entryShaft != Shaft)
                {
                    _entry = Site.Entry(Math.Max(0, Shaft));
                    _entryShaft = Shaft;
                }
                return _entry;
            }

            /// <summary>True when we are already hovering in front of the face (between shafts).</summary>
            bool InFrontOfFace()
            {
                if (!Site.Defined || _grid.Controller == null || Shaft < 0) return false;
                Vector3D rel = _grid.Controller.GetPosition() - ShaftEntry();
                double along = Vector3D.Dot(rel, Site.Fwd);
                double lateral = (rel - Site.Fwd * along).Length();
                return along > -_cfg.ApproachDistance - 5 && along < 2 && lateral < _cfg.ApproachDistance;
            }

            // =================================================================
            // Navigation: breadcrumbs, energy model, going home
            // =================================================================

            /// <summary>Records the outbound path; when full, keeps every other crumb and doubles the spacing.</summary>
            void DropCrumb(Vector3D pos)
            {
                if (CrumbCount > 0 && Vector3D.DistanceSquared(pos, _crumbs[CrumbCount - 1]) < _crumbSpacing * _crumbSpacing)
                    return;
                if (CrumbCount == MaxCrumbs)
                {
                    for (int i = 0; i < MaxCrumbs / 2; i++)
                        _crumbs[i] = _crumbs[i * 2];
                    CrumbCount = MaxCrumbs / 2;
                    _crumbSpacing *= 2;
                }
                _crumbs[CrumbCount++] = pos;
            }

            /// <summary>
            /// Inbound: retrace the outbound path while each crumb still brings us closer to the
            /// dock (the carrier may have moved), then hand over to the relative docking approach.
            /// </summary>
            bool FollowCrumbs()
            {
                if (CrumbCount == 0) return false;
                var ctrl = _grid.Controller;
                Vector3D me = ctrl.GetPosition();
                Vector3D dock = _dockPos + _carrierVel * (_p.Clock - _dockStamp);
                double meToDock = Vector3D.Distance(me, dock);
                while (CrumbCount > 0 && Vector3D.Distance(_crumbs[CrumbCount - 1], dock) >= meToDock - 1)
                    CrumbCount--;
                if (CrumbCount == 0) return false;

                Vector3D crumb = _crumbs[CrumbCount - 1];
                Vector3D dir = crumb - me;
                double d = dir.Length();
                double reach = Math.Max(10, ctrl.GetShipSpeed() * 2);
                if (d < reach)
                {
                    CrumbCount--;
                    return CrumbCount > 0;
                }
                _helm.SetTarget(crumb, Vector3D.Zero, dir / d, ctrl.WorldMatrix.Up, _cfg.MaxSpeed);
                return true;
            }

            void StartLearning()
            {
                _learning = true;
                _learnBatt = _grid.BatteryCharge;
                _learnH2 = _grid.HydrogenFill;
                _learnDist = 0;
                _learnLast = _grid.Controller.GetPosition();
            }

            void Learn(Vector3D pos)
            {
                if (!_learning) return;
                _learnDist += Vector3D.Distance(pos, _learnLast);
                _learnLast = pos;
            }

            /// <summary>Outbound leg done: blend the measured burn per metre into the model.</summary>
            void FinishLearning()
            {
                if (!_learning) return;
                _learning = false;
                if (_learnDist < 200) return; // too short to say anything
                double batt = (_learnBatt - _grid.BatteryCharge) / _learnDist;
                double h2 = (_learnH2 - _grid.HydrogenFill) / _learnDist;
                if (batt > 0) BattPerMetre = BattPerMetre > 0 ? (BattPerMetre + batt) / 2 : batt;
                if (h2 > 0) H2PerMetre = H2PerMetre > 0 ? (H2PerMetre + h2) / 2 : h2;
            }

            bool CarrierKnown() { return _p.Clock - _carrierSeen < CarrierMemory; }

            Vector3D CarrierNow() { return _carrierPos + _carrierPosVel * (_p.Clock - _carrierSeen); }

            /// <summary>Straight-line distance home, or -1 when the carrier's position is unknown.</summary>
            public double HomeDistance()
            {
                if (!CarrierKnown() || _grid.Controller == null) return -1;
                return Vector3D.Distance(_grid.Controller.GetPosition(), CarrierNow());
            }

            /// <summary>Battery fraction needed to get home: learned burn x distance x margin, plus reserve.</summary>
            public double BatteryNeeded()
            {
                double home = HomeDistance();
                if (home < 0 || BattPerMetre <= 0) return _cfg.ReturnCharge;
                return _cfg.ReserveCharge + home * BattPerMetre * _cfg.EnergyMargin;
            }

            public double HydrogenNeeded()
            {
                double home = HomeDistance();
                if (home < 0 || H2PerMetre <= 0) return _cfg.ReturnHydrogen;
                return _cfg.ReserveCharge + home * H2PerMetre * _cfg.EnergyMargin;
            }

            bool EnergyLow()
            {
                return _grid.BatteryCharge < BatteryNeeded()
                    || (_grid.HasHydrogen && _grid.HydrogenFill < HydrogenNeeded());
            }

            /// <summary>Any reason to abandon the site and head for the carrier. Sets the note.</summary>
            bool NeedsHome()
            {
                if (_grid.DrillsDamaged && IsMiner)
                {
                    _note = NoteDamaged;
                    return true;
                }
                if (_helm.LiftRatio < _cfg.MinLift)
                {
                    // Empty and still too weak: relaunching would only loop, so stay home after docking.
                    if (_grid.CargoFill <= _cfg.CargoEmpty) _autoCycle = false;
                    _note = _autoCycle ? NoteHeavy : NoteWeak;
                    return true;
                }
                if (EnergyLow())
                {
                    _note = NoteLowPower;
                    return true;
                }
                if (_grid.CargoFill >= _cfg.CargoFull)
                {
                    _note = NoteFull;
                    return true;
                }
                return false;
            }

            void GoHome()
            {
                _recall = false;
                if (_note == NoteFull) _p.Log.Add(_cfg.Name, LogRtbFull);
                else if (_note == NoteLowPower) _p.Log.Add(_cfg.Name, LogRtbPower);
                Enter(FleetState.RequestDock);
            }

            bool ReadyToLaunch()
            {
                return !_grid.DrillsDamaged
                    && _grid.CargoFill <= _cfg.CargoEmpty
                    && _grid.BatteryCharge >= _cfg.LaunchCharge
                    && (!_grid.HasHydrogen || _grid.HydrogenFill >= _cfg.LaunchCharge);
            }

            /// <summary>Docked and ready: ask flight control for a slot in the launch sequence.</summary>
            void TryLaunch()
            {
                if (!_autoCycle || _grid.Problem != null || !ReadyToLaunch()) return;
                if (_launchAt >= 0)
                {
                    if (_p.Clock >= _launchAt) Enter(FleetState.Undocking);
                    return;
                }
                if (_carrierAddr == 0 || (_launchFirstAsk >= 0 && _p.Clock - _launchFirstAsk > LaunchGiveUp))
                {
                    Enter(FleetState.Undocking); // no flight control answering: go on our own
                    return;
                }
                if (_p.Clock - _launchAskedAt < LaunchAskInterval) return;
                _launchAskedAt = _p.Clock;
                if (_launchFirstAsk < 0) _launchFirstAsk = _p.Clock;
                _comms.Unicast(_carrierAddr, Op.LaunchRequest, 0);
            }

            void ReportDelivery()
            {
                double total = 0;
                for (int i = 0; i < Ore.Count; i++)
                {
                    double d = Math.Max(0, _oreAtDock[i] - _grid.OreKg[i]);
                    _oreAtDock[i] = d; // reuse as the per-ore delivery for the message below
                    Delivered[i] += d;
                    total += d;
                }
                if (total < 1) return;
                _p.Log.Add(_cfg.Name, LogDelivered, (long)total, Kg);
                if (_carrierAddr != 0)
                    _comms.Unicast(_carrierAddr, Op.Delivery, 0,
                        new Vector3D(_oreAtDock[0], _oreAtDock[1], _oreAtDock[2]),
                        new Vector3D(_oreAtDock[3], _oreAtDock[4], _oreAtDock[5]),
                        new Vector3D(_oreAtDock[6], _oreAtDock[7], _oreAtDock[8]),
                        new Vector3D(_oreAtDock[9], _oreAtDock[10], _oreAtDock[11]));
            }

            // =================================================================
            // Self-preservation
            // =================================================================

            void WatchForDanger()
            {
                int reason = Distress.None;
                if (_grid.ThreatDetected) reason = Distress.Hostile;
                else if (_grid.Integrity < _integrityBaseline - _cfg.DamageTolerance) reason = Distress.Damage;

                if (reason != Distress.None && DistressReason == Distress.None)
                {
                    DistressReason = reason;
                    _autoCycle = false;
                    _note = reason == Distress.Hostile ? NoteHostile : NoteHullDamage;
                    _p.Log.Add(_cfg.Name, LogMayday);
                    _grid.FireHooks(HookDistress);
                    if (State == FleetState.Working) _recall = true; // back out of the shaft first
                    else if (State == FleetState.Transit || State == FleetState.Undocking) GoHome();
                    RefreshSignals();
                }

                if (DistressReason != Distress.None && _p.Clock - _distressSentAt > 5)
                {
                    _distressSentAt = _p.Clock;
                    var ctrl = _grid.Controller;
                    _comms.Broadcast(Op.Distress, DistressReason, Vector3D.Zero,
                        ctrl != null ? ctrl.GetPosition() : Vector3D.Zero);
                }
            }

            // =================================================================
            // Geometry
            // =================================================================

            /// <summary>
            /// Where the reference block must be so that OUR connector faces the carrier's
            /// dock connector, <paramref name="standoff"/> metres out along its normal.
            /// Pure relative matrix math: the dock pose is extrapolated by the carrier's
            /// velocity, then our connector's fixed offset from the reference block is undone.
            /// </summary>
            void SolveDockPose(double standoff, out Vector3D pos, out Vector3D fwd, out Vector3D up)
            {
                Vector3D dockPos = _dockPos + _carrierVel * (_p.Clock - _dockStamp);

                // Desired world matrix for our connector: facing back into the dock, rolled to its up.
                MatrixD connDesired = MatrixD.CreateWorld(dockPos + _dockFwd * standoff, -_dockFwd, _dockUp);

                // Our connector expressed in the reference block's frame (constant for a rigid grid).
                MatrixD refWorld = _grid.Controller.WorldMatrix;
                MatrixD refInv;
                MatrixD.Invert(ref refWorld, out refInv);
                MatrixD connLocal = _grid.Connector.WorldMatrix * refInv;

                // connWorld = connLocal * refWorld  =>  refDesired = connLocal^-1 * connDesired
                MatrixD connLocalInv;
                MatrixD.Invert(ref connLocal, out connLocalInv);
                MatrixD refDesired = connLocalInv * connDesired;

                pos = refDesired.Translation;
                fwd = refDesired.Forward;
                up = refDesired.Up;
            }

            // =================================================================
            // Networking
            // =================================================================

            void SendDockRequest()
            {
                _lastRequestAt = _p.Clock;
                _comms.Send(_carrierAddr, Op.DockRequest, _slot);
            }

            void StoreDockPose(FleetMessage m)
            {
                _dockPos = m.A;
                _dockFwd = m.B;
                _dockUp = m.C;
                _carrierVel = m.D;
                _dockStamp = _p.Clock;
            }

            public void HandleMessage(FleetMessage m)
            {
                if (IsCarrier)
                {
                    HandleAsCarrier(m);
                    return;
                }

                switch (m.Op)
                {
                    case Op.Status:
                        if (m.Arg == (int)FleetState.Carrier && (m.Source == _carrierAddr || _carrierAddr == 0))
                        {
                            _carrierPos = m.B;
                            _carrierPosVel = m.D;
                            _carrierSeen = _p.Clock;
                        }
                        break;

                    case Op.DockAssign:
                        if (State == FleetState.RequestDock)
                        {
                            _carrierAddr = m.Source;
                            _slot = m.Arg;
                            _note = null;
                            _deniedAt = double.MinValue;
                            StoreDockPose(m);
                            Enter(FleetState.Approach);
                        }
                        else if (m.Source == _carrierAddr && m.Arg == _slot)
                        {
                            StoreDockPose(m); // lease renewal reply
                        }
                        break;

                    case Op.DockBeacon:
                        if (m.Source == _carrierAddr && m.Arg == _slot)
                            StoreDockPose(m);
                        break;

                    case Op.DockDeny:
                        if (State != FleetState.RequestDock) break;
                        bool wasHolding = Holding;
                        _note = NoteDenied;
                        QueuePlace = m.Arg;
                        _deniedAt = _p.Clock;
                        if (_carrierAddr == 0) _carrierAddr = m.Source;
                        if (!wasHolding)
                        {
                            Vector3D away = _grid.Controller.GetPosition() - CarrierNow();
                            _holdDir = away.LengthSquared() > 1 ? Vector3D.Normalize(away) : _grid.Controller.WorldMatrix.Backward;
                            _grid.FireHooks(HookHolding);
                            RefreshSignals();
                        }
                        break;

                    case Op.LaunchClear:
                        if (State != FleetState.Docked || _launchAt >= 0) break;
                        _launchAt = _p.Clock + m.Arg;
                        _p.Log.Add(_cfg.Name, LogCleared, m.Arg, Sec);
                        _grid.FireHooks(HookLaunch);
                        break;

                    case Op.ShaftReport:
                        if (m.Arg != SiteId || SiteId < 0) break;
                        int shaft = (int)m.A.X, status = (int)m.A.Y;
                        Site.Apply(shaft, status, m.A.Z, m.Source, _p.Clock);
                        // Two drones picked the same shaft: the lower address keeps it.
                        if (status == SiteMap.Claimed && shaft == Shaft && State == FleetState.Transit
                            && m.Source < _p.IGC.Me && !PickShaft())
                            SiteExhausted();
                        break;

                    case Op.SiteOffer:
                        if (_carrierAddr != 0 && m.Source != _carrierAddr) break;
                        _adoptId = m.Arg;
                        _adoptPos = m.A;
                        _adoptFwd = m.B;
                        _adoptUp = m.C;
                        _adoptSpacing = m.D;
                        _adoptPending = true;
                        if (State != FleetState.Working) ApplyAdoption();
                        break;

                    case Op.FleetCommand:
                        if (_carrierAddr != 0 && m.Source != _carrierAddr) return;
                        if (m.Arg == Cmd.Launch) Start();
                        else if (m.Arg == Cmd.Recall) Recall();
                        else if (m.Arg == Cmd.Halt) Stop();
                        break;
                }
            }

            /// <summary>Switches to a shared site from flight control; its map arrives as ShaftReports.</summary>
            void ApplyAdoption()
            {
                _adoptPending = false;
                Site.Define(_adoptPos, _adoptFwd, _adoptUp, _adoptSpacing.X, _adoptSpacing.Y, (int)_adoptSpacing.Z);
                SiteId = _adoptId;
                Shaft = -1;
                ResetShaft();
                _siteAdoptedAt = _p.Clock;
                _staged = false;
                _p.Log.Add(_cfg.Name, LogAdopted, SiteId);
            }
        }
    }
}
