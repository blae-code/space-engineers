using Sandbox.ModAPI.Ingame;
using System;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public enum FleetState
        {
            Idle, Docked, Undocking, Transit, Working, RequestDock, Approach, FinalDock, Carrier, Mothership, Evading
        }

        /// <summary>
        /// Decides what the grid is doing. Drones (miners, haulers) fly this file's state
        /// machine; bases (carrier, mothership) run BrainFSM.Bases.cs. Commands, self-test,
        /// status text and persistence live in BrainFSM.Io.cs.
        /// </summary>
        public partial class BrainFSM : ISubsystem
        {
            public static readonly string[] StateNames =
            {
                "Idle", "Docked", "Undocking", "Transit", "Working", "RequestDock", "Approach", "FinalDock",
                "Carrier", "Mothership", "Evading"
            };

            /// <summary>Radio-style labels for HUD markers and screens.</summary>
            public static readonly string[] StateLabels =
            {
                "IDLE", "DOCKED", "LAUNCHING", "EN ROUTE", "MINING", "RTB", "INBOUND", "DOCKING",
                "FLIGHT CONTROL", "FLEET HQ", "EVADING"
            };

            /// <summary>Hooks, by state: a timer or sound block named "... [FM Docked]" fires on entering Docked.</summary>
            static readonly string[] StateTags =
            {
                "[FM Idle]", "[FM Docked]", "[FM Undocking]", "[FM Transit]", "[FM Working]",
                "[FM RequestDock]", "[FM Approach]", "[FM FinalDock]", "[FM Carrier]", "[FM Mothership]", "[FM Evading]"
            };
            const string HookDistress = "[FM Distress]", HookBackout = "[FM Backout]";
            const string HookRich = "[FM Rich]", HookLaunch = "[FM Launch]", HookHolding = "[FM Holding]";
            const string HookLinkLost = "[FM LinkLost]";

            static readonly Color[] StateColor =
            {
                new Color(160, 160, 160), new Color(0, 200, 60), new Color(0, 180, 255), new Color(0, 180, 255),
                new Color(255, 255, 230), new Color(255, 200, 0), new Color(255, 200, 0), new Color(0, 200, 60),
                Color.Black, Color.Black, new Color(255, 60, 0)
            };
            static readonly float[] StateBlink = { 0, 0, 1, 0, 0, 0, 0, 0.5f, 0, 0, 0.3f };
            static readonly Color Amber = new Color(255, 120, 0), Red = new Color(255, 0, 0);

            // Where a drone docks: its home carrier, a hauler at the site, or the mothership.
            const int DockHome = 0, DockHauler = 1, DockMother = 2;

            const double DockLease = 15;         // s a pad server keeps beaconing after the last DockRequest
            const double RequestInterval = 3;    // s between DockRequests (also renews the lease)
            const double DenyMemory = 10;        // s a DockDeny keeps us in the holding pattern
            const double PeerFresh = 30;         // s a peer's position report stays usable
            const double LaunchAskInterval = 8;  // s between LaunchRequests
            const double LaunchGiveUp = 30;      // s without clearance before launching anyway
            const double SiteSyncGrace = 5;      // s to collect a shared site's map before picking a shaft
            const double HaulerFresh = 10;       // s a HaulerOffer stays valid
            const double LoadStall = 20;         // s without cargo gain before a shuttle leaves part-loaded
            const double MaxYield = 20;          // s a drone gives way to traffic before pressing on
            const int MaxCrumbs = 32;

            // Static notes only: assigning a literal never allocates.
            const string NoteNoSite = "No site. Aim at the rock face, run 'setsite'";
            const string NoteDenied = "Docks full, holding";
            const string NoteUnknown = "Unknown command";
            const string NoteKnockedLoose = "Lost connector lock";
            const string NoteNoLink = "Lost dock beacon, re-requesting";
            const string NoteStalled = "Shaft blocked, skipping it";
            const string NoteSiteDone = "Site exhausted. 'setsite' a new one";
            const string NoteDamaged = "Drill damaged, returning for repair";
            const string NoteHeavy = "Too heavy to climb, returning";
            const string NoteWeak = "Can't lift even when empty: add thrust";
            const string NoteLowPower = "Power low, returning";
            const string NoteFull = "Hold full, returning";
            const string NoteHostile = "MAYDAY: hostile contact";
            const string NoteHullDamage = "MAYDAY: hull damage, returning";
            const string NoteBarren = "Barren rock, moving on";
            const string NoteLinkLost = "Lost link to carrier, heading for its last position";
            const string NoteDetour = "Obstacle ahead, detouring";

            // Log lines (who is the speaker; these are the words).
            const string LogLaunch = "launching";
            const string LogOnStation = "on station, shaft";
            const string LogRtbFull = "hold full, RTB";
            const string LogRtbPower = "power low, RTB";
            const string LogHandoff = "handing off ore to hauler";
            const string LogRich = "rich vein, shaft";
            const string LogBarren = "barren, abandoning shaft";
            const string LogBlocked = "shaft blocked";
            const string LogDocked = "docked";
            const string LogDelivered = "delivered";
            const string LogExhausted = "site exhausted";
            const string LogMayday = "MAYDAY";
            const string LogEvading = "evading hostile";
            const string LogCleared = "cleared for launch, T-";
            const string LogAdopted = "assigned to site";
            const string LogLinkLost = "lost link to carrier";
            const string LogLinkBack = "link restored";
            const string LogRehomed = "re-homed to nearest carrier";
            const string LogHaulerStation = "hauler on station, site";
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

            // Home carrier, and the pad server we are docking with right now (home, hauler or mothership).
            long _carrierAddr;
            long _dockAddr;
            int _dockKind;
            bool _dockHq;
            int _slot = -1;
            Vector3D _dockPos, _dockFwd, _dockUp, _carrierVel;
            double _dockStamp = double.MinValue;
            double _lastRequestAt = double.MinValue, _requestAt;
            // Dock queue / holding pattern.
            public int QueuePlace { get; private set; }
            double _deniedAt = double.MinValue;
            Vector3D _holdDir;
            // Launch clearance.
            double _launchAt = -1, _launchAskedAt = double.MinValue, _launchFirstAsk = -1;

            // Link to home: reachability, last known position, re-homing.
            public bool LinkLost { get; private set; }
            double _homeHeardAt;
            Vector3D _lastHomePos;
            bool _hasLastHome;

            // Haulers at our site advertising free bays.
            long _haulerAddr;
            double _haulerSeen = double.MinValue;
            int _haulerBays, _haulerSite = -1;
            // Site hauler: leaving station once its bays are clear. Shuttle: load progress.
            bool _closing;
            long _lastLoadVolume;
            double _loadProgressAt;

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
            // Charting a new site from a GPS target ('chart N GPS:...').
            bool _charting, _chartRockKnown;
            int _chartSite;
            Vector3D _chartTarget, _chartRock;
            // Camera survey of the site's shafts.
            int _surveyIdx;
            public int Surveyed { get; private set; }

            // Current shaft. Depths are along the site forward from the shaft entry, measured at
            // the reference block. They persist, so a shaft interrupted by a full hold is resumed.
            public bool FaceKnown { get { return _grid.HasLasers ? _laserFace >= 0 : _faceKnown; } }
            // Laser mining: face distance from the shaft entry (-1 unknown), firing state and timing.
            // The cut so far is kept in _minedDepth, so an interrupted shaft resumes where it stopped.
            double _laserFace = -1, _laserStart, _laserContactAt, _laserFired;
            bool _laserFiring;
            bool _faceKnown, _faceFromSurvey;
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

            // Look-ahead avoidance and traffic.
            Vector3D _detour;
            double _detourUntil = double.MinValue, _yieldSince = -1;
            Vector3D _yieldPos;

            // Energy model: fraction of battery / hydrogen burned per metre flown, learned outbound.
            // Burn is per metre per tonne, so a full hold predicts the heavier trip home. Learned on
            // both legs (outbound light, inbound laden); hover-heavy legs are discarded.
            public double BattPerMetre { get; private set; }
            public double H2PerMetre { get; private set; }
            /// <summary>Fraction of battery / H2 used per second while working, and a typical shaft's duration.</summary>
            public double WorkDrainBatt { get; private set; }
            public double WorkDrainH2 { get; private set; }
            public double ShaftSeconds { get; private set; }
            bool _learning;
            double _learnBatt, _learnH2, _learnDist, _learnMass, _learnStart;
            Vector3D _learnLast;
            double _workAt = -1, _workBatt, _workH2, _sessionStart;

            // Self-preservation.
            public int DistressReason { get; private set; }
            double _integrityBaseline = 1, _distressSentAt = double.MinValue, _shieldBaseline = -1;
            const double ShieldDrop = 25; // shield points lost since launch that count as being under attack
            Vector3D _fleeTarget, _fleeDir;
            bool _aiFleeing;

            // Production: ore on board when we docked, and lifetime deliveries.
            readonly double[] _oreAtDock = new double[Ore.Count];
            public readonly double[] Delivered = new double[Ore.Count];
            double _statsBase; // seconds of stats time from previous sessions

            // HUD markers, built once (callsign + label).
            readonly string[] _hud = new string[StateLabels.Length];
            string _hudMayday, _hudHolding, _hudLinkLost;

            bool IsCarrier { get { return _cfg.Role == FleetRole.Carrier; } }
            bool IsMiner { get { return _cfg.Role == FleetRole.Miner; } }
            bool IsShuttle { get { return _cfg.Role == FleetRole.Hauler && _cfg.Shuttle; } }
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
                _hudLinkLost = _cfg.Name + " | NO LINK";

                State = IsCarrier ? FleetState.Carrier : IsMothership ? FleetState.Mothership : FleetState.Idle;
                _enteredAt = _p.Clock;
                _homeHeardAt = _p.Clock;
                RefreshSignals();
            }

            // =================================================================
            // Transitions
            // =================================================================

            void Enter(FleetState next)
            {
                if (State == FleetState.Working && _sessionOpen)
                    CloseSession();
                if (State == FleetState.Evading && _aiFleeing)
                {
                    _grid.StopAiFlee();
                    _aiFleeing = false;
                }

                State = next;
                _enteredAt = _p.Clock;

                // Every state except Docked flies (or may need dampeners), so thrusters must be live.
                if (next != FleetState.Docked)
                    _grid.SetDockedMode(false);
                if (next != FleetState.Working)
                {
                    _grid.SetEjecting(false);
                    StopLasers();
                }

                switch (next)
                {
                    case FleetState.Idle:
                        _grid.SetDrills(false);
                        _helm.Disengage();
                        break;

                    case FleetState.Docked:
                        FinishLearning(); // inbound leg done, before charging muddies it
                        _grid.SetDrills(false);
                        _helm.Disengage();
                        _grid.SetDockedMode(true);
                        _grid.SetDecoys(false);
                        DistressReason = 0;
                        _launchAt = -1;
                        _launchFirstAsk = -1;
                        _launchAskedAt = double.MinValue;
                        _loadProgressAt = _p.Clock;
                        _lastLoadVolume = _grid.CargoVolume;
                        Array.Copy(_grid.OreKg, _oreAtDock, Ore.Count);
                        _p.Log.Add(_cfg.Name, LogDocked);
                        break;

                    case FleetState.Undocking:
                        _note = null; // keep the reason we came home visible until the next trip
                        if (IsMiner) ReportDelivery();
                        _integrityBaseline = _grid.Integrity;
                        _shieldBaseline = _grid.ShieldPercent;
                        _grid.ReleaseGear();
                        if (_dockKind == DockHome)
                        {
                            CrumbCount = 0;
                            _crumbSpacing = _cfg.CrumbSpacing;
                        }
                        _p.Log.Add(_cfg.Name, LogLaunch);
                        BeginUndock();
                        break;

                    case FleetState.Transit:
                        _grid.SetDrills(false);
                        _staged = InFrontOfFace();
                        break;

                    case FleetState.Working:
                        _workAt = -1;
                        if (IsMiner) OpenSession(); // haulers hold station; they never touch the map
                        else _p.Log.Add(_cfg.Name, LogHaulerStation, SiteId);
                        _closing = false;
                        _grid.SetDrills(IsMiner);
                        break;

                    case FleetState.RequestDock:
                        _grid.SetDrills(false);
                        _helm.HoldPosition();
                        _deniedAt = double.MinValue;
                        _requestAt = _p.Clock;
                        _yieldSince = -1;
                        SendDockRequest();
                        break;

                    case FleetState.Evading:
                        _grid.SetDrills(false);
                        _grid.SetDecoys(true);
                        Vector3D me = _grid.Controller.GetPosition();
                        Vector3D away = me - _grid.ThreatPos;
                        _fleeDir = away.LengthSquared() > 1 ? Vector3D.Normalize(away) : _grid.Controller.WorldMatrix.Backward;
                        _fleeTarget = me + _fleeDir * _cfg.FleeDistance;
                        _aiFleeing = _grid.StartAiFlee();
                        if (_aiFleeing) _helm.Disengage(); // the AI Defensive block flies now
                        _p.Log.Add(_cfg.Name, LogEvading);
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
                    _grid.SetHud(LinkLost ? _hudLinkLost : _hud[(int)State]);
                    _grid.SetLights(StateColor[(int)State], StateBlink[(int)State]);
                }
            }

            bool Holding { get { return _p.Clock - _deniedAt < DenyMemory; } }

            void BeginUndock()
            {
                var ctrl = _grid.Controller;
                var conn = _grid.Connector;
                MatrixD refM = ctrl.WorldMatrix;

                // Everything is relative to the pad: its velocity at release and our
                // connector's axis. No absolute waypoint is ever stored.
                _undockVel = ctrl.GetShipVelocities().LinearVelocity;
                _undockOrigin = refM.Translation;
                _undockDir = conn.WorldMatrix.Backward; // away from the mating face
                _undockFwd = refM.Forward;
                _undockUp = refM.Up;

                _grid.Disconnect();
                if (_dockAddr != 0)
                    _comms.Unicast(_dockAddr, Op.DockRelease, _slot, default(Vector3D), default(Vector3D),
                        default(Vector3D), default(Vector3D), _dockHq);
            }

            // =================================================================
            // Ticks
            // =================================================================

            public void Update10()
            {
                if (_cfg.IsBase)
                {
                    StreamBeacons();
                    return;
                }
                if (ServesBays) StreamBeacons();

                if (_grid.Problem != null && State != FleetState.Idle && State != FleetState.Docked)
                {
                    _note = _grid.Problem;
                    Enter(FleetState.Idle);
                    return;
                }

                _helm.AltitudeFloor = FloorApplies() ? _cfg.MinAltitude : 0;
                if (_learning && _grid.Controller != null) Learn(_grid.Controller.GetPosition());
                switch (State)
                {
                    case FleetState.Undocking: TickUndocking(); break;
                    case FleetState.Transit: TickTransit(); break;
                    case FleetState.Working: TickWorking(); break;
                    case FleetState.RequestDock: TickRequestDock(); break;
                    case FleetState.Approach: TickApproach(); break;
                    case FleetState.FinalDock: TickFinalDock(); break;
                    case FleetState.Evading: TickEvading(); break;
                }
                SurveyStep();
            }

            public void Update100()
            {
                if (_cfg.IsBase)
                {
                    BaseUpdate100();
                    return;
                }

                WatchLink();
                switch (State)
                {
                    case FleetState.Docked:
                        if (!_grid.IsConnected)
                        {
                            _note = NoteKnockedLoose;
                            Enter(FleetState.Idle);
                        }
                        else DockedUpdate();
                        break;

                    case FleetState.Working:
                        LearnWorkDrain();
                        if (IsMiner) MiningUpdate();
                        else HaulerOnStation();
                        break;

                    case FleetState.RequestDock:
                        // A hauler that doesn't answer is no use: go home instead.
                        if (_dockKind == DockHauler && !Holding && _p.Clock - _requestAt > 10)
                        {
                            SetDock(DockHome);
                            SendDockRequest();
                        }
                        else if (_p.Clock - _lastRequestAt >= RequestInterval) SendDockRequest();
                        break;

                    // Approach/FinalDock renew the pad server's beacon lease.
                    case FleetState.Approach:
                    case FleetState.FinalDock:
                        if (_p.Clock - _lastRequestAt >= RequestInterval)
                            SendDockRequest();
                        break;
                }

                if (State != FleetState.Idle && State != FleetState.Docked)
                    WatchForDanger();
                AutoAntenna();

                var ctrl = _grid.Controller;
                var cargo = new Vector3D(_grid.CargoFill, _grid.BatteryCharge, _grid.HydrogenFill);
                var pos = ctrl != null ? ctrl.GetPosition() : Vector3D.Zero;
                var work = new Vector3D(SiteId, Shaft, DistressReason);
                var vel = ctrl != null ? ctrl.GetShipVelocities().LinearVelocity : Vector3D.Zero;
                _comms.Broadcast(Op.Status, (int)State, cargo, pos, work, vel);
                if (IsShuttle) _comms.Broadcast(Op.Status, (int)State, cargo, pos, work, vel, true);
            }

            void TickUndocking()
            {
                double t = _p.Clock - _enteredAt;
                Vector3D target = _undockOrigin + _undockVel * t + _undockDir * _cfg.ApproachDistance;
                _helm.SetTarget(target, _undockVel, _undockFwd, _undockUp, _cfg.ApproachSpeed);
                if (_helm.DistanceToTarget >= 3) return;

                if (IsShuttle)
                {
                    // Loaded: on to the mothership. Empty: back to the carrier for the next load.
                    SetDock(_grid.CargoFill > _cfg.CargoEmpty && MotherAddr != 0 ? DockMother : DockHome);
                    Enter(FleetState.RequestDock);
                    return;
                }
                if (!Site.Defined && !_charting)
                {
                    Enter(FleetState.Idle);
                    return;
                }
                if (_dockKind == DockHome) StartLearning();
                Enter(FleetState.Transit);
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
                if (_charting)
                {
                    TickChart(pos);
                    return;
                }

                // Haulers fly to their holding point off the face; miners to a shaft.
                if (!IsMiner)
                {
                    Vector3D hold = HoldPoint();
                    if (!_staged && LongLeg(pos, hold)) return;
                    _helm.SetTarget(hold, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                    if (_helm.DistanceToTarget < 3 && _helm.RelativeSpeed < 0.5) Enter(FleetState.Working);
                    return;
                }

                // Shared site just adopted: give the carrier a moment to send its map.
                if (Shaft < 0 && _p.Clock - _siteAdoptedAt > SiteSyncGrace && !PickShaft())
                {
                    SiteExhausted();
                    return;
                }
                Vector3D entry = Shaft >= 0 ? ShaftEntry() : Site.Pos;
                Vector3D stage = entry - Site.Fwd * _cfg.ApproachDistance;

                if (!_staged)
                {
                    // Arrive square-on to the face from a staging point out in clear space,
                    // so the final leg never cuts across the rock.
                    if (LongLeg(pos, stage)) return;
                    _helm.SetTarget(stage, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                    if (_helm.DistanceToTarget < 3 && _helm.RelativeSpeed < 1)
                    {
                        _staged = true;
                        FinishLearning();
                    }
                    return;
                }

                if (Shaft < 0)
                {
                    _helm.SetTarget(stage, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                    return;
                }
                _helm.SetTarget(entry, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                if (_helm.DistanceToTarget < 1 && _helm.RelativeSpeed < 0.5 && _helm.AlignmentError < 0.05)
                {
                    if (CanFinishShaft()) Enter(FleetState.Working);
                    else
                    {
                        _note = NoteLowPower; // a shaft started now would strand us
                        GoHome();
                    }
                }
            }

            /// <summary>
            /// The outbound leg to a static point near the site. Records breadcrumbs and the energy
            /// model; long legs use the Remote Control autopilot (collision avoidance), the rest fly
            /// nose-first with camera look-ahead. False once within 100 m (caller does the final leg).
            /// </summary>
            bool LongLeg(Vector3D pos, Vector3D target)
            {
                DropCrumb(pos);
                Vector3D to = target - pos;
                double far = to.Length();
                if (far < 100) return false;
                if (_cfg.Autopilot && far > _cfg.AutopilotRange && _helm.AutopilotTo(target, _cfg.MaxSpeed))
                    return true;
                FlyTo(target, Vector3D.Zero, to / far, _grid.Controller.WorldMatrix.Up, _cfg.MaxSpeed);
                return true;
            }

            /// <summary>
            /// One shaft: glide through open space and already-cut hole, drill at MineSpeed from the
            /// rock face to MineDepth past it, then back straight out along the shaft axis.
            /// Site haulers: hold station off the face while miners use the bays.
            /// </summary>
            void TickWorking()
            {
                if (!IsMiner)
                {
                    _helm.SetTarget(HoldPoint(), Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                    return;
                }
                if (_grid.HasLasers)
                {
                    TickLaser();
                    return;
                }

                Vector3D entry = ShaftEntry();
                bool goHome = _recall || NeedsHome();
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
                var ctrl = _grid.Controller;
                Vector3D server, serverVel;
                // Pads full: wait in a holding ring around the pad server instead of where we
                // stopped, staggered by queue place so holding drones never share a spot.
                if (Holding && PeerPosition(_dockAddr, out server, out serverVel))
                {
                    double r = _cfg.HoldDistance + 30 * Math.Max(0, QueuePlace - 1);
                    _helm.SetTarget(server + _holdDir * r, serverVel, -_holdDir, ctrl.WorldMatrix.Up, _cfg.MaxSpeed);
                    return;
                }
                // No link home: head for where the carrier was last heard, then wait in range.
                if (LinkLost && _hasLastHome && _dockKind == DockHome)
                {
                    Vector3D away = ctrl.GetPosition() - _lastHomePos;
                    double d = away.Length();
                    if (d > _cfg.HoldDistance + 20)
                        FlyTo(_lastHomePos + away / d * _cfg.HoldDistance, Vector3D.Zero, -away / d, ctrl.WorldMatrix.Up, _cfg.MaxSpeed);
                }
            }

            void TickApproach()
            {
                if (_p.Clock - _dockStamp > _cfg.BeaconTimeout)
                {
                    _note = NoteNoLink;
                    Enter(FleetState.RequestDock);
                    return;
                }
                if (GiveWay()) return;
                if (_dockKind == DockHome && FollowCrumbs()) return;

                Vector3D pos, fwd, up;
                SolveDockPose(_cfg.ApproachDistance, out pos, out fwd, out up);
                bool far = _helm.DistanceToTarget > _cfg.ApproachDistance * 2;
                if (far) FlyTo(pos, _carrierVel, fwd, up, _cfg.MaxSpeed);
                else _helm.SetTarget(pos, _carrierVel, fwd, up, _cfg.ApproachSpeed);

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

                // Connector standoffs are centre to centre: mated connectors of the same size sit two
                // half-lengths apart. Start DockGap beyond that and creep in until it reports ready to lock.
                double contact = 2 * _grid.ConnectorHalf;
                double gap = Math.Max(contact - 0.25, contact + _cfg.DockGap - (_p.Clock - _enteredAt) * 0.3);
                Vector3D pos, fwd, up;
                SolveDockPose(gap, out pos, out fwd, out up);
                _helm.SetTarget(pos, _carrierVel, fwd, up, _cfg.DockSpeed);
            }

            /// <summary>Run directly away from the threat (or let an AI Defensive block do it), then go home.</summary>
            void TickEvading()
            {
                if (_p.Clock - _enteredAt > _cfg.FleeTime)
                {
                    GoHome();
                    return;
                }
                if (!_aiFleeing)
                    FlyTo(_fleeTarget, Vector3D.Zero, _fleeDir, _grid.Controller.WorldMatrix.Up, _cfg.MaxSpeed);
            }

            /// <summary>
            /// Charting: fly to the GPS target, find the rock with the camera (or use gravity on a
            /// planet: dig straight down), stop SiteStandoff short of it, aim, and record the site.
            /// </summary>
            void TickChart(Vector3D pos)
            {
                var ctrl = _grid.Controller;
                Vector3D g = ctrl.GetNaturalGravity();
                Vector3D to = _chartTarget - pos;
                double d = to.Length();
                Vector3D dir = g.LengthSquared() > 0.25 ? Vector3D.Normalize(g) : to / Math.Max(1, d);
                bool hit, voxel;
                Vector3D point;
                double look = _cfg.ScanRange * 2;
                if (d < look && _grid.RaycastAt(pos + dir * Math.Min(d + 50, look), out hit, out voxel, out point) && hit && voxel)
                {
                    _chartRock = point;
                    _chartRockKnown = true;
                }
                Vector3D stand = (_chartRockKnown ? _chartRock : _chartTarget) - dir * (_cfg.SiteStandoff + _grid.DrillReach + 10);
                if (LongLeg(pos, stand)) return;

                Vector3D up = ctrl.WorldMatrix.Up - dir * Vector3D.Dot(ctrl.WorldMatrix.Up, dir);
                if (up.LengthSquared() < 0.01) up = ctrl.WorldMatrix.Right;
                _helm.SetTarget(stand, Vector3D.Zero, dir, Vector3D.Normalize(up), _cfg.ApproachSpeed);
                if (_helm.DistanceToTarget > 2 || _helm.RelativeSpeed > 0.3 || _helm.AlignmentError > 0.03) return;

                _charting = false;
                SetSite(_chartSite); // camera scan places the plane; charted with the carrier
                if (Site.Defined && Shaft >= 0) Enter(FleetState.Transit);
                else Enter(FleetState.Idle);
            }

            bool FloorApplies()
            {
                return (State == FleetState.Transit && !_staged) || State == FleetState.RequestDock
                    || State == FleetState.Evading || (State == FleetState.Approach && CrumbCount > 0);
            }

            // =================================================================
            // Docked: unload / load and launch decisions per role
            // =================================================================

            void DockedUpdate()
            {
                bool home = _dockKind == DockHome;
                if (IsShuttle)
                {
                    if (home)
                    {
                        _grid.Load();
                        if (_grid.CargoVolume > _lastLoadVolume)
                        {
                            _lastLoadVolume = _grid.CargoVolume;
                            _loadProgressAt = _p.Clock;
                        }
                        bool loaded = _grid.CargoFill >= _cfg.CargoFull
                            || (_grid.CargoFill > _cfg.CargoEmpty && _p.Clock - _loadProgressAt > LoadStall);
                        if (loaded && MotherAddr != 0) TryLaunch(PoweredUp());
                    }
                    else
                    {
                        _grid.Unload();
                        if (_autoCycle && _grid.CargoFill <= _cfg.CargoEmpty && !EnergyLow()) Enter(FleetState.Undocking);
                    }
                    return;
                }

                _grid.Unload();
                if (!home)
                {
                    // Miner at a hauler: hand the ore over and get straight back to work.
                    if (_autoCycle && _grid.CargoFill <= _cfg.CargoEmpty && !EnergyLow()) Enter(FleetState.Undocking);
                    return;
                }
                TryLaunch(ReadyToLaunch());
            }

            /// <summary>Site hauler on station: advertise free bays; leave once full and the bays are clear.</summary>
            void HaulerOnStation()
            {
                if (!_closing && (_recall || NeedsHome() || DistressReason != 0)) _closing = true;
                if (!_closing)
                {
                    var ctrl = _grid.Controller;
                    _comms.Broadcast(Op.HaulerOffer, SiteId, new Vector3D(FreeSlots(), _grid.CargoFill, 0),
                        ctrl != null ? ctrl.GetPosition() : Vector3D.Zero);
                }
                else if (!BaysBusy()) GoHome();
            }

            Vector3D HoldPoint()
            {
                return Site.Pos - Site.Fwd * (_cfg.ApproachDistance + 40) + Site.Up * 30;
            }

            // =================================================================
            // Mining: shaft sessions, ore yield, face finding, survey
            // =================================================================

            // =================================================================
            // Laser mining (Adjustable Mining Laser, ToolCore multitools)
            // =================================================================

            const string NoteBeamBlocked = "Ship or person in the beam path: holding fire";
            const string NoteLaserMode = "Laser not in Drill mode: holding fire";

            /// <summary>
            /// Lasers bore from outside: hover just short of the rock face, aim down the shaft axis
            /// and fire until the shaft is MineDepth deep, never flying into the hole.
            /// </summary>
            void TickLaser()
            {
                Vector3D entry = ShaftEntry();
                bool goHome = _recall || NeedsHome();
                double hover = _laserFace >= 0 ? Math.Max(0, _laserFace - _grid.LaserForward - _cfg.FaceMargin) : 0;
                if (!_retracting)
                {
                    if (goHome) Retract(false, SiteMap.Partial);
                    else
                    {
                        _helm.SetTarget(entry + Site.Fwd * hover, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                        LaserStep(entry, hover);
                        return;
                    }
                }
                Vector3D exit = goHome ? entry - Site.Fwd * _cfg.ApproachDistance : entry;
                _helm.SetTarget(exit, Vector3D.Zero, Site.Fwd, Site.Up, _cfg.ApproachSpeed);
                if (_helm.DistanceToTarget < 1.5)
                {
                    CloseSession();
                    if (goHome) GoHome();
                    else Enter(FleetState.Transit);
                }
            }

            void LaserStep(Vector3D entry, double hover)
            {
                // Fire only when held steady on the axis.
                if (_helm.DistanceToTarget > 1.5 || _helm.AlignmentError > 0.04)
                {
                    StopLasers();
                    return;
                }
                double reach = _grid.LaserRange;
                double goal = Math.Min(_cfg.MineDepth, reach - _cfg.FaceMargin - 1);
                double emitter = hover + _grid.LaserForward; // emitter depth along the axis

                // Camera down the axis: safety (nothing but rock in the beam) and, for ToolCore, depth.
                double camDepth = -1;
                bool hit, voxel;
                Vector3D point;
                var cam = _grid.Camera;
                if (cam != null && _grid.RaycastAt(cam.GetPosition() + Site.Fwd * (reach + _grid.LaserForward), out hit, out voxel, out point) && hit)
                {
                    double along = Vector3D.Dot(point - entry, Site.Fwd);
                    if (!voxel)
                    {
                        StopLasers();
                        _note = NoteBeamBlocked;
                        if (_p.Clock - _progressAt > _cfg.StallTime) Retract(true, SiteMap.Blocked);
                        return;
                    }
                    camDepth = along;
                    if (_laserFace < 0) SetLaserFace(along);
                }
                if (_laserFace < 0 && _grid.LasersToolCore && cam == null) SetLaserFace(emitter); // blind: time-based

                if (!_grid.SetLasers(true))
                {
                    _laserFiring = false;
                    _note = NoteLaserMode;
                    return;
                }
                if (!_laserFiring)
                {
                    _laserFiring = true;
                    _laserStart = _laserContactAt = _p.Clock;
                }
                _laserFired += _p.Dt10;

                if (!_grid.LasersToolCore)
                {
                    // Adjustable Mining Laser reports where it is cutting; cap the beam at the shaft bottom.
                    double contact = _grid.LaserContact;
                    if (contact >= 0)
                    {
                        double d = emitter + contact;
                        if (_laserFace < 0) SetLaserFace(d);
                        _minedDepth = Math.Max(_minedDepth, d - _laserFace);
                        _laserContactAt = _progressAt = _p.Clock;
                    }
                    _grid.SetLaserRange((_laserFace >= 0 ? _laserFace + goal : emitter + reach) - emitter + 1);
                    if (_laserFace >= 0 && _p.Clock - _laserContactAt > 5)
                    {
                        Retract(true, SiteMap.Done); // the beam reaches the bottom and finds nothing left
                        return;
                    }
                }
                else
                {
                    // ToolCore: depth from the camera when it can see down the hole, else from time.
                    if (_laserFace >= 0)
                    {
                        double cut = camDepth > _laserFace + 0.5 ? camDepth - _laserFace : _laserFired * _cfg.LaserSpeed;
                        _minedDepth = Math.Max(_minedDepth, cut);
                    }
                    _progressAt = _p.Clock;
                }

                if (_laserFace >= 0 && _minedDepth >= goal - 0.5) Retract(true, SiteMap.Done);
                else if (_laserFace < 0 && _p.Clock - _laserStart > 8) Retract(true, SiteMap.Empty);
            }

            void SetLaserFace(double depth)
            {
                _laserFace = Math.Max(0, depth);
                Site.Face[Shaft] = _laserFace;
            }

            void StopLasers()
            {
                if (_grid.HasLasers) _grid.SetLasers(false);
                _laserFiring = false;
            }

            void MiningUpdate()
            {
                if (_grid.Controller == null || !_sessionOpen) return;
                if (_grid.HasLasers)
                {
                    CheckBarren();
                    _grid.SetEjecting(_laserFiring && _grid.CanEject);
                    return;
                }
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
            }

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
                _sessionStart = _p.Clock;
                _depth = 0;
                _retracting = false;
                _shaftDone = false;
                _progressAt = _p.Clock;
                _lastCargoVolume = _grid.CargoVolume;
                // A surveyed face lets us glide in at once; a fresh scan still refines it.
                if (!_faceKnown && Site.Face[Shaft] >= 0)
                {
                    _faceKnown = _faceFromSurvey = true;
                    _faceDepth = Math.Max(0, Site.Face[Shaft] - _grid.DrillReach);
                }
                _scanPending = (!_faceKnown || _faceFromSurvey) && _grid.Camera != null && !_grid.HasLasers;
                _laserFace = Site.Face[Shaft];
                _laserFiring = false;
                _laserFired = _cfg.LaserSpeed > 0 ? _minedDepth / _cfg.LaserSpeed : 0; // ToolCore resume
                _shaftBase = Site.Value[Shaft];
                Array.Copy(_grid.OreKg, _oreAtStart, Ore.Count);
                Site.Apply(Shaft, SiteMap.Claimed, _shaftBase, -1, _p.IGC.Me, _p.Clock);
                ReportShaft(Shaft, SiteMap.Claimed, false);
                _p.Log.Add(_cfg.Name, LogOnStation, Shaft);
            }

            /// <summary>Leaving Working: record what this shaft yielded and tell the fleet.</summary>
            void CloseSession()
            {
                _sessionOpen = false;
                double mean = Site.MeanValue();
                int status = _shaftDone ? _shaftResult : SiteMap.Partial;
                if (status == SiteMap.Done)
                {
                    double s = _p.Clock - _sessionStart;
                    ShaftSeconds = ShaftSeconds > 0 ? ShaftSeconds * 0.7 + s * 0.3 : s;
                }
                Site.Apply(Shaft, status, ShaftValue(), -1, _p.IGC.Me, _p.Clock);
                ReportShaft(Shaft, status, true);

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
                StopLasers();
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
                double cut = _grid.HasLasers ? (_laserFace >= 0 ? _minedDepth : -1) : (_faceKnown ? CurrentDepth() - _faceDepth : -1);
                if (_retracting || cut < _cfg.BarrenDepth || _cfg.BarrenDepth <= 0 || ShaftValue() >= 1) return;
                _note = NoteBarren;
                Retract(true, SiteMap.Barren);
            }

            /// <summary>
            /// Claims and survey data are broadcast (best effort, drone to drone). Results go to the
            /// carrier reliably; it applies them, passes them up to HQ and re-broadcasts them to us all.
            /// </summary>
            void ReportShaft(int shaft, int status, bool result)
            {
                if (SiteId < 0) return;
                var report = new Vector3D(shaft, status, Site.Value[shaft]);
                // B.Y = 1 marks survey data: bases apply it but don't forward it up to HQ.
                var face = new Vector3D(Site.Face[shaft], result ? 0 : 1, 0);
                if (result && _carrierAddr != 0) _comms.SendReliable(_carrierAddr, Op.ShaftReport, SiteId, report, face);
                else _comms.Broadcast(Op.ShaftReport, SiteId, report, face);
            }

            /// <summary>Raycast the face once per shaft, as soon as the camera is aimed down the axis.</summary>
            void ScanFace(Vector3D entry)
            {
                if (_faceKnown && !_faceFromSurvey)
                {
                    _scanPending = false;
                    return;
                }
                if (_helm.AlignmentError > 0.05) return; // wait until the camera looks down the axis
                bool hit;
                Vector3D point;
                if (!_grid.TryScanRock(_cfg.ScanRange, out hit, out point)) return; // camera charging
                _scanPending = false;
                if (!hit) return; // fall back to the survey value or contact detection
                _faceKnown = true;
                _faceFromSurvey = false;
                _faceDepth = Math.Max(0, Vector3D.Dot(point - entry, Site.Fwd) - _grid.DrillReach);
            }

            /// <summary>
            /// Camera survey, one ray per tick while near the face: a hit gives that shaft's face
            /// distance before anyone flies there; a near-axial miss marks it as having no rock.
            /// </summary>
            void SurveyStep()
            {
                // Never while a laser fires: its beam-safety ray needs the camera's charge.
                if (!_cfg.Survey || !IsMiner || !Site.Defined || _grid.Camera == null || _scanPending || _laserFiring) return;
                if (State != FleetState.Working && !(State == FleetState.Transit && _staged)) return;
                Vector3D cam = _grid.Camera.GetPosition();
                for (int tries = 0; tries < 4; tries++)
                {
                    int i = _surveyIdx;
                    _surveyIdx = (_surveyIdx + 1) % Site.Limit;
                    if (Site.Status[i] != SiteMap.Untouched || Site.Face[i] >= 0) continue;
                    Vector3D entry = Site.Entry(i);
                    Vector3D target = entry + Site.Fwd * _cfg.ScanRange;
                    bool hit;
                    Vector3D point;
                    if (!_grid.RaycastAt(target, true, out hit, out point)) continue; // outside the cone or charging
                    if (hit)
                        Site.Apply(i, SiteMap.Untouched, 0, Math.Max(0, Vector3D.Dot(point - entry, Site.Fwd)), _p.IGC.Me, _p.Clock);
                    else if (Vector3D.Dot(Vector3D.Normalize(target - cam), Site.Fwd) > 0.96)
                        Site.Apply(i, SiteMap.Empty, 0, -1, _p.IGC.Me, _p.Clock);
                    else return;
                    Surveyed++;
                    ReportShaft(i, Site.Status[i], false);
                    return;
                }
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
            public double CutDepth
            {
                get { return _grid.HasLasers ? _minedDepth : _faceKnown ? Math.Max(0, _depth - _faceDepth) : 0; }
            }

            void ResetShaft()
            {
                _faceKnown = false;
                _faceFromSurvey = false;
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
                if (!Site.Defined || _grid.Controller == null) return false;
                Vector3D target = IsMiner ? (Shaft >= 0 ? ShaftEntry() : Site.Pos) : HoldPoint();
                Vector3D rel = _grid.Controller.GetPosition() - target;
                double along = Vector3D.Dot(rel, Site.Fwd);
                double lateral = (rel - Site.Fwd * along).Length();
                return along > -_cfg.ApproachDistance - 5 && along < 2 && lateral < _cfg.ApproachDistance;
            }

            // =================================================================
            // Navigation: look-ahead, traffic, breadcrumbs, energy, going home
            // =================================================================

            /// <summary>
            /// Flies towards a target while the camera looks ahead along the way (within its cone).
            /// Anything in the path, short of the target itself, gets a detour around it.
            /// </summary>
            void FlyTo(Vector3D target, Vector3D vel, Vector3D fwd, Vector3D up, double speed)
            {
                var ctrl = _grid.Controller;
                Vector3D me = ctrl.GetPosition();
                if (_p.Clock < _detourUntil)
                {
                    if (Vector3D.DistanceSquared(me, _detour) > 100)
                    {
                        _helm.SetTarget(_detour, Vector3D.Zero, fwd, up, speed);
                        return;
                    }
                    _detourUntil = double.MinValue;
                }

                Vector3D to = target - me;
                double dist = to.Length();
                double v = ctrl.GetShipSpeed();
                if (v > 5 && dist > 60)
                {
                    Vector3D dir = to / dist;
                    double look = Math.Min(dist - 30, Math.Max(60, v * 5));
                    bool hit;
                    Vector3D point;
                    if (_grid.RaycastAt(me + dir * look, false, out hit, out point) && hit)
                    {
                        Vector3D side = ctrl.WorldMatrix.Up - dir * Vector3D.Dot(ctrl.WorldMatrix.Up, dir);
                        if (side.LengthSquared() < 0.01) side = ctrl.WorldMatrix.Right;
                        _detour = point - dir * 30 + Vector3D.Normalize(side) * 80;
                        _detourUntil = _p.Clock + 20;
                        _note = NoteDetour;
                        _helm.SetTarget(_detour, Vector3D.Zero, fwd, up, speed);
                        return;
                    }
                }
                _helm.SetTarget(target, vel, fwd, up, speed);
            }

            /// <summary>
            /// Traffic around the pads: within Separation of another drone, the one with the higher
            /// address (or anyone near a drone on final) holds still, for at most MaxYield seconds.
            /// </summary>
            bool GiveWay()
            {
                Vector3D me = _grid.Controller.GetPosition();
                long self = _p.IGC.Me;
                bool yield = false;
                for (int i = 0; i < _comms.PeerCount && !yield; i++)
                {
                    int st = _comms.PeerState[i];
                    if (_comms.PeerRole[i] >= (int)FleetRole.Carrier || _p.Clock - _comms.PeerSeen[i] > 5
                        || st == (int)FleetState.Docked || st == (int)FleetState.Idle) continue;
                    if (Vector3D.Distance(me, _comms.PeerNow(i)) > _cfg.Separation) continue;
                    yield = _comms.PeerState[i] == (int)FleetState.FinalDock || _comms.PeerAddress[i] < self;
                }
                if (!yield)
                {
                    _yieldSince = -1;
                    return false;
                }
                if (_yieldSince < 0)
                {
                    _yieldSince = _p.Clock;
                    _yieldPos = me;
                }
                if (_p.Clock - _yieldSince > MaxYield) return false;
                _helm.SetTarget(_yieldPos, Vector3D.Zero, _grid.Controller.WorldMatrix.Forward, _grid.Controller.WorldMatrix.Up, _cfg.ApproachSpeed);
                return true;
            }

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
                FlyTo(crumb, Vector3D.Zero, dir / d, ctrl.WorldMatrix.Up, _cfg.MaxSpeed);
                return true;
            }

            void StartLearning()
            {
                _learning = true;
                _learnBatt = _grid.BatteryCharge;
                _learnH2 = _grid.HydrogenFill;
                _learnDist = 0;
                _learnMass = MassTonnes();
                _learnStart = _p.Clock;
                _learnLast = _grid.Controller.GetPosition();
            }

            double MassTonnes()
            {
                var ctrl = _grid.Controller;
                return ctrl != null ? Math.Max(0.1, ctrl.CalculateShipMass().PhysicalMass / 1000) : 1;
            }

            /// <summary>Working: blend the battery / H2 drain per second (Update100 samples).</summary>
            void LearnWorkDrain()
            {
                if (_workAt >= 0)
                {
                    double dt = _p.Clock - _workAt;
                    double db = (_workBatt - _grid.BatteryCharge) / dt;
                    double dh = (_workH2 - _grid.HydrogenFill) / dt;
                    if (dt > 0.5 && db >= 0) WorkDrainBatt = WorkDrainBatt > 0 ? WorkDrainBatt * 0.9 + db * 0.1 : db;
                    if (dt > 0.5 && dh >= 0 && _grid.HasHydrogen) WorkDrainH2 = WorkDrainH2 > 0 ? WorkDrainH2 * 0.9 + dh * 0.1 : dh;
                }
                _workAt = _p.Clock;
                _workBatt = _grid.BatteryCharge;
                _workH2 = _grid.HydrogenFill;
            }

            /// <summary>Would we still have enough to get home after a typical shaft? (true until learned)</summary>
            bool CanFinishShaft()
            {
                if (ShaftSeconds <= 0) return true;
                if (WorkDrainBatt > 0 && _grid.BatteryCharge - WorkDrainBatt * ShaftSeconds < BatteryNeeded()) return false;
                return !(_grid.HasHydrogen && WorkDrainH2 > 0 && _grid.HydrogenFill - WorkDrainH2 * ShaftSeconds < HydrogenNeeded());
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
                // Too short, or mostly hovering (holding, giving way): it would overstate the burn.
                if (_learnDist < 200 || _learnDist / Math.Max(1, _p.Clock - _learnStart) < 3) return;
                double batt = (_learnBatt - _grid.BatteryCharge) / _learnDist / _learnMass;
                double h2 = (_learnH2 - _grid.HydrogenFill) / _learnDist / _learnMass;
                if (batt > 0) BattPerMetre = BattPerMetre > 0 ? (BattPerMetre + batt) / 2 : batt;
                if (h2 > 0) H2PerMetre = H2PerMetre > 0 ? (H2PerMetre + h2) / 2 : h2;
            }

            /// <summary>A peer's current position and velocity, when it reported recently.</summary>
            bool PeerPosition(long address, out Vector3D pos, out Vector3D vel)
            {
                int i = _comms.IndexOfPeer(address);
                if (address == 0 || i < 0 || _p.Clock - _comms.PeerSeen[i] > PeerFresh)
                {
                    pos = vel = Vector3D.Zero;
                    return false;
                }
                pos = _comms.PeerNow(i);
                vel = _comms.PeerVel[i];
                return true;
            }

            /// <summary>Straight-line distance home (last known position if out of contact), -1 if never heard.</summary>
            public double HomeDistance()
            {
                if (_grid.Controller == null) return -1;
                Vector3D home, vel;
                if (PeerPosition(_carrierAddr, out home, out vel))
                    return Vector3D.Distance(_grid.Controller.GetPosition(), home);
                return _hasLastHome ? Vector3D.Distance(_grid.Controller.GetPosition(), _lastHomePos) : -1;
            }

            /// <summary>Battery fraction needed to get home: learned burn x distance x margin, plus reserve.</summary>
            public double BatteryNeeded()
            {
                double home = HomeDistance();
                if (home < 0 || BattPerMetre <= 0) return _cfg.ReturnCharge;
                return _cfg.ReserveCharge + home * BattPerMetre * MassTonnes() * _cfg.EnergyMargin;
            }

            public double HydrogenNeeded()
            {
                double home = HomeDistance();
                if (home < 0 || H2PerMetre <= 0) return _cfg.ReturnHydrogen;
                return _cfg.ReserveCharge + home * H2PerMetre * MassTonnes() * _cfg.EnergyMargin;
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
                if (LinkLost)
                {
                    _note = NoteLinkLost;
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

            /// <summary>Hauler at our site with free bays, when the only reason to go is a full hold.</summary>
            int ChooseDock()
            {
                if (IsMiner && _note == NoteFull && !_recall && DistressReason == 0 && !LinkLost
                    && _haulerAddr != 0 && _p.Clock - _haulerSeen < HaulerFresh && _haulerSite == SiteId && _haulerBays > 0)
                    return DockHauler;
                if (IsShuttle && _grid.CargoFill > _cfg.CargoEmpty && MotherAddr != 0) return DockMother;
                return DockHome;
            }

            void SetDock(int kind)
            {
                long addr = kind == DockHauler ? _haulerAddr : kind == DockMother ? MotherAddr : _carrierAddr;
                if (addr != _dockAddr) _slot = -1;
                _dockKind = kind;
                _dockAddr = addr;
                _dockHq = kind == DockMother;
            }

            void GoHome()
            {
                if (!_learning && _grid.Controller != null && (State == FleetState.Working || State == FleetState.Transit))
                    StartLearning(); // inbound leg (laden)
                int kind = ChooseDock();
                _recall = false;
                if (kind == DockHauler) _p.Log.Add(_cfg.Name, LogHandoff);
                else if (_note == NoteFull) _p.Log.Add(_cfg.Name, LogRtbFull);
                else if (_note == NoteLowPower) _p.Log.Add(_cfg.Name, LogRtbPower);
                SetDock(kind);
                Enter(FleetState.RequestDock);
            }

            bool PoweredUp()
            {
                return _grid.BatteryCharge >= _cfg.LaunchCharge
                    && (!_grid.HasHydrogen || _grid.HydrogenFill >= _cfg.LaunchCharge);
            }

            bool ReadyToLaunch()
            {
                return !_grid.DrillsDamaged && _grid.CargoFill <= _cfg.CargoEmpty && PoweredUp();
            }

            /// <summary>Docked and ready: ask flight control for a slot in the launch sequence.</summary>
            void TryLaunch(bool ready)
            {
                if (!_autoCycle || _grid.Problem != null || !ready) return;
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

            /// <summary>What we unloaded since docking, reported to our home carrier (reliably).</summary>
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
                    _comms.SendReliable(_carrierAddr, Op.Delivery, 0,
                        new Vector3D(_oreAtDock[0], _oreAtDock[1], _oreAtDock[2]),
                        new Vector3D(_oreAtDock[3], _oreAtDock[4], _oreAtDock[5]),
                        new Vector3D(_oreAtDock[6], _oreAtDock[7], _oreAtDock[8]),
                        new Vector3D(_oreAtDock[9], _oreAtDock[10], _oreAtDock[11]));
            }

            // =================================================================
            // Link, antenna, self-preservation
            // =================================================================

            /// <summary>
            /// Is home reachable? If not for LinkTimeout, re-home to the nearest carrier we can
            /// reach; failing that, mark the link lost (fly home to its last known position).
            /// </summary>
            void WatchLink()
            {
                var ctrl = _grid.Controller;
                if (ctrl == null) return;
                if (_comms.Reachable(_carrierAddr))
                {
                    _homeHeardAt = _p.Clock;
                    Vector3D home, vel;
                    if (PeerPosition(_carrierAddr, out home, out vel))
                    {
                        _lastHomePos = home;
                        _hasLastHome = true;
                    }
                    if (LinkLost)
                    {
                        LinkLost = false;
                        _p.Log.Add(_cfg.Name, LogLinkBack);
                        RefreshSignals();
                    }
                    return;
                }
                if (_carrierAddr != 0 && (_p.Clock - _homeHeardAt < _cfg.LinkTimeout
                    || State == FleetState.Docked || State == FleetState.Idle)) return;

                long alt = _comms.Nearest(FleetRole.Carrier, ctrl.GetPosition(), PeerFresh);
                if (alt != 0 && alt != _carrierAddr && _comms.Reachable(alt))
                {
                    bool orphan = _carrierAddr == 0;
                    _carrierAddr = alt;
                    _homeHeardAt = _p.Clock;
                    LinkLost = false;
                    if (!orphan) _p.Log.Add(_cfg.Name, LogRehomed);
                    if (State == FleetState.RequestDock && _dockKind == DockHome)
                    {
                        SetDock(DockHome);
                        SendDockRequest();
                    }
                    return;
                }
                if (!LinkLost && _carrierAddr != 0)
                {
                    LinkLost = true;
                    _p.Log.Add(_cfg.Name, LogLinkLost);
                    _grid.FireHooks(HookLinkLost);
                    RefreshSignals();
                }
            }

            /// <summary>Home carrier reports a shield with charge left (its Status A.Y is shield charge, -1 without).</summary>
            bool HomeShielded()
            {
                int i = _comms.IndexOfPeer(_carrierAddr);
                return i >= 0 && _p.Clock - _comms.PeerSeen[i] < PeerFresh && _comms.PeerBattery[i] > 0.1;
            }

            /// <summary>Antenna range: short when docked, distance home x 1.5 in flight, full when lost.</summary>
            void AutoAntenna()
            {
                if (!_cfg.AntennaAuto) return;
                double r;
                if (State == FleetState.Docked) r = 500;
                else
                {
                    double home = HomeDistance();
                    r = home < 0 || LinkLost ? _cfg.AntennaMax : Math.Min(_cfg.AntennaMax, Math.Max(1000, home * 1.5 + 500));
                }
                _grid.SetAntennaRange((float)r);
            }

            void WatchForDanger()
            {
                int reason = Distress.None;
                if (_grid.ThreatDetected) reason = Distress.Hostile;
                // Defense Shields: our shield draining is an attack, even before anything is seen.
                else if (_shieldBaseline > 0 && _grid.ShieldPercent >= 0 && _grid.ShieldPercent < _shieldBaseline - ShieldDrop)
                    reason = Distress.Hostile;
                else if (_grid.Integrity < _integrityBaseline - _cfg.DamageTolerance) reason = Distress.Damage;

                if (reason != Distress.None && DistressReason == Distress.None)
                {
                    DistressReason = reason;
                    _autoCycle = false;
                    _note = reason == Distress.Hostile ? NoteHostile : NoteHullDamage;
                    _p.Log.Add(_cfg.Name, LogMayday);
                    _grid.FireHooks(HookDistress);
                    if (State == FleetState.Working) _recall = true; // back out of the shaft first
                    else if (reason == Distress.Hostile && HomeShielded())
                        GoHome(); // safe harbour: inside the carrier's shield beats open space
                    else if (reason == Distress.Hostile && _grid.ThreatLocated && State != FleetState.FinalDock)
                        Enter(FleetState.Evading);
                    else if (State == FleetState.Transit || State == FleetState.Undocking) GoHome();
                    RefreshSignals();
                }

                if (DistressReason != Distress.None && _p.Clock - _distressSentAt > 5)
                {
                    _distressSentAt = _p.Clock;
                    var ctrl = _grid.Controller;
                    _comms.Broadcast(Op.Distress, DistressReason, Vector3D.Zero,
                        ctrl != null ? ctrl.GetPosition() : Vector3D.Zero, _grid.ThreatPos, _grid.ThreatVel);
                }
            }

            // =================================================================
            // Geometry
            // =================================================================

            /// <summary>
            /// Where the reference block must be so that OUR connector faces the pad's connector,
            /// <paramref name="standoff"/> metres out along its normal. Pure relative matrix math:
            /// the pad pose is extrapolated by its velocity, then our connector's fixed offset from
            /// the reference block is undone.
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
                if (_dockAddr == 0 && _dockKind != DockHome) SetDock(DockHome);
                _comms.Send(_dockAddr, Op.DockRequest, (_slot + 1) | (Urgent() ? 0x100 : 0), default(Vector3D),
                    default(Vector3D), default(Vector3D), default(Vector3D), _dockHq);
            }

            /// <summary>Needs a pad first: in distress, damaged, or down to its power reserve.</summary>
            bool Urgent()
            {
                return DistressReason != 0 || _grid.DrillsDamaged
                    || _grid.Integrity < _integrityBaseline - _cfg.DamageTolerance
                    || _grid.BatteryCharge < _cfg.ReserveCharge
                    || (_grid.HasHydrogen && _grid.HydrogenFill < _cfg.ReserveCharge);
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
                if (_cfg.IsBase)
                {
                    HandleAsBase(m);
                    return;
                }
                if (ServesBays && !m.Hq)
                {
                    if (m.Op == Op.DockRequest) { OnDockRequest(m.Source, m.Arg); return; }
                    if (m.Op == Op.DockRelease) { ReleaseSlots(m.Source); return; }
                    if (m.Op == Op.Status) TouchSlots(m.Source);
                }

                switch (m.Op)
                {
                    case Op.Status:
                        if (m.Hq && m.Arg == (int)FleetState.Mothership) MotherAddr = m.Source;
                        break;

                    case Op.HaulerOffer:
                        if (!IsMiner || m.Arg != SiteId) break;
                        // Keep the hauler with the most free bays (or refresh the current one).
                        if (m.Source == _haulerAddr || _p.Clock - _haulerSeen > HaulerFresh || m.A.X > _haulerBays)
                        {
                            _haulerAddr = m.Source;
                            _haulerSeen = _p.Clock;
                            _haulerBays = (int)m.A.X;
                            _haulerSite = m.Arg;
                        }
                        break;

                    case Op.DockAssign:
                        if (State == FleetState.RequestDock && (m.Source == _dockAddr || (_dockAddr == 0 && m.Hq == _dockHq)))
                        {
                            if (_dockAddr == 0)
                            {
                                _dockAddr = m.Source;
                                if (_dockKind == DockHome)
                                {
                                    _carrierAddr = m.Source;
                                    _homeHeardAt = _p.Clock;
                                }
                            }
                            _slot = m.Arg;
                            _note = null;
                            _deniedAt = double.MinValue;
                            StoreDockPose(m);
                            Enter(FleetState.Approach);
                        }
                        else if (m.Source == _dockAddr && m.Arg == _slot)
                        {
                            StoreDockPose(m); // lease renewal reply
                        }
                        break;

                    case Op.DockBeacon:
                        if (m.Source == _dockAddr && m.Arg == _slot)
                            StoreDockPose(m);
                        break;

                    case Op.DockDeny:
                        if (State != FleetState.RequestDock || (_dockAddr != 0 && m.Source != _dockAddr)) break;
                        if (_dockKind == DockHauler)
                        {
                            SetDock(DockHome); // hauler full: take the ore home after all
                            SendDockRequest();
                            break;
                        }
                        bool wasHolding = Holding;
                        _note = NoteDenied;
                        QueuePlace = m.Arg;
                        _deniedAt = _p.Clock;
                        if (_dockAddr == 0) _dockAddr = m.Source;
                        if (!wasHolding)
                        {
                            Vector3D server, serverVel;
                            Vector3D away = PeerPosition(_dockAddr, out server, out serverVel)
                                ? _grid.Controller.GetPosition() - server : Vector3D.Zero;
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
                        Site.Apply(shaft, status, m.A.Z, m.B.X, m.Source, _p.Clock);
                        // Two drones picked the same shaft: the lower address keeps it.
                        if (status == SiteMap.Claimed && shaft == Shaft && State == FleetState.Transit
                            && m.Source < _p.IGC.Me && !PickShaft())
                            SiteExhausted();
                        break;

                    case Op.SiteOffer:
                        if (m.Hq || (_carrierAddr != 0 && m.Source != _carrierAddr)) break;
                        _adoptId = m.Arg;
                        _adoptPos = m.A;
                        _adoptFwd = m.B;
                        _adoptUp = m.C;
                        _adoptSpacing = m.D;
                        _adoptPending = true;
                        if (State != FleetState.Working || !IsMiner) ApplyAdoption();
                        break;

                    case Op.ChartOrder:
                        if (!m.Hq && (_carrierAddr == 0 || m.Source == _carrierAddr)) BeginChart(m.Arg, m.A);
                        break;

                    case Op.FleetCommand:
                        if (m.Hq || (_carrierAddr != 0 && m.Source != _carrierAddr)) return;
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
