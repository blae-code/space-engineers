using Sandbox.ModAPI.Ingame;
using System;
using System.Globalization;
using System.Text;
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
        /// Decides what the grid is doing. Drones (Miner/Hauler) run the mining cycle;
        /// a Carrier runs the dockmaster: it hands out dock slots and streams dock poses.
        /// </summary>
        public class BrainFSM : ISubsystem
        {
            public static readonly string[] StateNames =
            {
                "Idle", "Docked", "Undocking", "Transit", "Working", "RequestDock", "Approach", "FinalDock", "Carrier"
            };

            const string SaveVersion = "FM1";
            const int MaxSlots = 16;
            const double DockLease = 15;         // s a carrier keeps beaconing after the last DockRequest
            const double RequestInterval = 3;    // s between DockRequests (also renews the lease)
            const double SlotReleaseAge = 600;   // s before an absent drone loses its reserved slot

            // Static notes only: assigning a literal never allocates.
            const string NoteNoSite = "No site. Fly to the rock face, aim, run 'setsite'";
            const string NoteDenied = "Carrier has no free dock, retrying";
            const string NoteUnknown = "Unknown command";
            const string NoteKnockedLoose = "Lost connector lock";
            const string NoteNoLink = "Lost carrier beacon, re-requesting";

            readonly Program _p;
            readonly Config _cfg;
            readonly GridManager _grid;
            readonly CommsOfficer _comms;
            readonly HelmController _helm;

            public FleetState State { get; private set; }
            double _enteredAt;
            bool _autoCycle;
            string _note;

            // Home: carrier address, slot, and the latest dock pose + carrier velocity.
            long _carrierAddr;
            int _slot = -1;
            Vector3D _dockPos, _dockFwd, _dockUp, _carrierVel;
            double _dockStamp = double.MinValue;
            double _lastRequestAt = double.MinValue;

            // Work site. Asteroids never move, so the world frame IS the site's frame.
            bool _hasSite;
            Vector3D _sitePos, _siteFwd, _siteUp;
            int _shaft;
            int _entryShaft = -1;
            Vector3D _entry;
            double _depth;
            bool _retracting;

            // Undock: captured at the moment of release, extrapolated with the carrier's velocity.
            Vector3D _undockOrigin, _undockDir, _undockVel, _undockFwd, _undockUp;

            // Carrier dockmaster tables (fixed capacity, never reallocated).
            readonly long[] _slotOwner = new long[MaxSlots];
            readonly double[] _slotLeaseUntil = new double[MaxSlots];
            readonly double[] _slotLastSeen = new double[MaxSlots];

            bool IsCarrier { get { return _cfg.Role == FleetRole.Carrier; } }

            public BrainFSM(Program p, GridManager grid, CommsOfficer comms, HelmController helm)
            {
                _p = p;
                _cfg = p.Cfg;
                _grid = grid;
                _comms = comms;
                _helm = helm;
            }

            public void Initialize()
            {
                State = IsCarrier ? FleetState.Carrier : FleetState.Idle;
                _enteredAt = _p.Clock;
            }

            // =================================================================
            // Transitions
            // =================================================================

            void Enter(FleetState next)
            {
                State = next;
                _enteredAt = _p.Clock;

                // Every state except Docked flies (or may need dampeners), so thrusters must be live.
                if (next != FleetState.Docked)
                    _grid.SetDockedMode(false);

                switch (next)
                {
                    case FleetState.Idle:
                        _grid.SetDrills(false);
                        _helm.Disengage();
                        break;

                    case FleetState.Docked:
                        _note = null;
                        _grid.SetDrills(false);
                        _helm.Disengage();
                        _grid.SetDockedMode(true);
                        break;

                    case FleetState.Undocking:
                        BeginUndock();
                        break;

                    case FleetState.Transit:
                        _grid.SetDrills(false);
                        break;

                    case FleetState.Working:
                        _depth = 0;
                        _retracting = false;
                        _grid.SetDrills(_cfg.Role == FleetRole.Miner);
                        break;

                    case FleetState.RequestDock:
                        _grid.SetDrills(false);
                        _helm.HoldPosition();
                        SendDockRequest();
                        break;
                }
            }

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
                    case FleetState.Approach: TickApproach(); break;
                    case FleetState.FinalDock: TickFinalDock(); break;
                }
            }

            public void Update100()
            {
                if (IsCarrier)
                {
                    ExpireSlots();
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
                        else if (_autoCycle && _grid.Problem == null && ReadyToLaunch())
                        {
                            Enter(FleetState.Undocking);
                        }
                        break;

                    // RequestDock retries; Approach/FinalDock renew the carrier's beacon lease.
                    case FleetState.RequestDock:
                    case FleetState.Approach:
                    case FleetState.FinalDock:
                        if (_p.Clock - _lastRequestAt >= RequestInterval)
                            SendDockRequest();
                        break;
                }

                _comms.Broadcast(Op.Status, (int)State,
                    new Vector3D(_grid.CargoFill, _grid.BatteryCharge, _grid.HydrogenFill),
                    _grid.Controller != null ? _grid.Controller.GetPosition() : Vector3D.Zero,
                    Vector3D.Zero,
                    _grid.Controller != null ? _grid.Controller.GetShipVelocities().LinearVelocity : Vector3D.Zero);
            }

            void TickUndocking()
            {
                double t = _p.Clock - _enteredAt;
                Vector3D target = _undockOrigin + _undockVel * t + _undockDir * _cfg.ApproachDistance;
                _helm.SetTarget(target, _undockVel, _undockFwd, _undockUp, _cfg.ApproachSpeed);

                if (_helm.DistanceToTarget < 3)
                    Enter(_hasSite ? FleetState.Transit : FleetState.Idle);
            }

            void TickTransit()
            {
                if (EnergyLow() || _grid.CargoFill >= _cfg.CargoFull)
                {
                    Enter(FleetState.RequestDock);
                    return;
                }

                _helm.SetTarget(ShaftEntry(), Vector3D.Zero, _siteFwd, _siteUp, _cfg.MaxSpeed);
                if (_helm.DistanceToTarget < 2 && _helm.RelativeSpeed < 0.5 && _helm.AlignmentError < 0.05)
                    Enter(FleetState.Working);
            }

            void TickWorking()
            {
                bool full = _grid.CargoFill >= _cfg.CargoFull;
                bool low = EnergyLow();
                Vector3D entry = ShaftEntry();

                if (_cfg.Role != FleetRole.Miner)
                {
                    // Hauler: hold station at the site until someone fills the hold.
                    _helm.SetTarget(entry, Vector3D.Zero, _siteFwd, _siteUp, _cfg.ApproachSpeed);
                    if (full || low) Enter(FleetState.RequestDock);
                    return;
                }

                if (!_retracting)
                {
                    if (full || low || _depth >= _cfg.MineDepth)
                        _retracting = true;
                    else if (_helm.DistanceToTarget < 1.0) // only advance once the drills have caught up
                        _depth = Math.Min(_depth + _cfg.MineSpeed * _p.Dt10, _cfg.MineDepth);
                }

                if (_retracting)
                {
                    _helm.SetTarget(entry, Vector3D.Zero, _siteFwd, _siteUp, _cfg.MineSpeed * 3);
                    if (_helm.DistanceToTarget < 1.5)
                    {
                        _shaft++;
                        Enter(full || low ? FleetState.RequestDock : FleetState.Transit);
                    }
                    return;
                }

                _helm.SetTarget(entry + _siteFwd * _depth, Vector3D.Zero, _siteFwd, _siteUp, _cfg.MineSpeed);
            }

            void TickApproach()
            {
                if (_p.Clock - _dockStamp > _cfg.BeaconTimeout)
                {
                    _note = NoteNoLink;
                    Enter(FleetState.RequestDock);
                    return;
                }

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

            /// <summary>Entry point of the current shaft; shafts spiral out from the site origin.</summary>
            Vector3D ShaftEntry()
            {
                if (_entryShaft != _shaft)
                {
                    int dx, dy;
                    Spiral(_shaft, out dx, out dy);
                    Vector3D right = Vector3D.Cross(_siteFwd, _siteUp);
                    _entry = _sitePos + right * (dx * _cfg.ShaftSpacing) + _siteUp * (dy * _cfg.ShaftSpacing);
                    _entryShaft = _shaft;
                }
                return _entry;
            }

            /// <summary>Square spiral: 0 -> (0,0), 1 -> (1,0), 2 -> (1,1), 3 -> (0,1), ...</summary>
            static void Spiral(int n, out int x, out int y)
            {
                x = 0;
                y = 0;
                int dx = 1, dy = 0, segLen = 1, segDone = 0, turns = 0;
                for (int i = 0; i < n; i++)
                {
                    x += dx;
                    y += dy;
                    if (++segDone < segLen) continue;
                    segDone = 0;
                    int t = dx;
                    dx = -dy;
                    dy = t;
                    if (++turns % 2 == 0) segLen++;
                }
            }

            bool EnergyLow()
            {
                return _grid.BatteryCharge < _cfg.ReturnCharge
                    || (_grid.HasHydrogen && _grid.HydrogenFill < _cfg.ReturnHydrogen);
            }

            bool ReadyToLaunch()
            {
                return _grid.CargoFill <= _cfg.CargoEmpty
                    && _grid.BatteryCharge >= _cfg.LaunchCharge
                    && (!_grid.HasHydrogen || _grid.HydrogenFill >= _cfg.LaunchCharge);
            }

            // =================================================================
            // Networking
            // =================================================================

            void SendDockRequest()
            {
                _lastRequestAt = _p.Clock;
                if (_carrierAddr == 0 || !_comms.Unicast(_carrierAddr, Op.DockRequest, _slot))
                    _comms.Broadcast(Op.DockRequest, _slot);
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
                    case Op.DockAssign:
                        if (State == FleetState.RequestDock)
                        {
                            _carrierAddr = m.Source;
                            _slot = m.Arg;
                            _note = null;
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
                        if (State == FleetState.RequestDock)
                            _note = NoteDenied;
                        break;

                    case Op.FleetCommand:
                        if (_carrierAddr != 0 && m.Source != _carrierAddr) return;
                        if (m.Arg == Cmd.Launch) Start();
                        else if (m.Arg == Cmd.Recall) Recall();
                        else if (m.Arg == Cmd.Halt) Stop();
                        break;
                }
            }

            // =================================================================
            // Carrier (dockmaster)
            // =================================================================

            void HandleAsCarrier(FleetMessage m)
            {
                switch (m.Op)
                {
                    case Op.DockRequest:
                        int slot = AllocateSlot(m.Source, m.Arg);
                        if (slot < 0)
                        {
                            _comms.Unicast(m.Source, Op.DockDeny, 0);
                            return;
                        }
                        _slotLeaseUntil[slot] = _p.Clock + DockLease;
                        _slotLastSeen[slot] = _p.Clock;
                        SendDockPose(m.Source, Op.DockAssign, slot);
                        break;

                    case Op.DockRelease:
                        for (int i = 0; i < MaxSlots; i++)
                            if (_slotOwner[i] == m.Source)
                            {
                                _slotLeaseUntil[i] = 0;
                                _slotLastSeen[i] = _p.Clock;
                            }
                        break;

                    case Op.Status:
                        for (int i = 0; i < MaxSlots; i++)
                            if (_slotOwner[i] == m.Source) _slotLastSeen[i] = _p.Clock;
                        break;
                }
            }

            int SlotCount { get { return Math.Min(_grid.DockConnectors.Count, MaxSlots); } }

            bool SlotFree(int i)
            {
                return _grid.DockConnectors[i].Status != MyShipConnectorStatus.Connected
                    && (_slotOwner[i] == 0 || _p.Clock - _slotLastSeen[i] > SlotReleaseAge);
            }

            int AllocateSlot(long requester, int preferred)
            {
                int n = SlotCount;
                for (int i = 0; i < n; i++)
                    if (_slotOwner[i] == requester) return i;

                int pick = -1;
                if (preferred >= 0 && preferred < n && SlotFree(preferred)) pick = preferred;
                for (int i = 0; i < n && pick < 0; i++)
                    if (SlotFree(i)) pick = i;

                if (pick >= 0) _slotOwner[pick] = requester;
                return pick;
            }

            /// <summary>
            /// The one sanctioned Update10 network send: a drone on final approach
            /// needs fresh poses of a moving dock, so we stream them while its lease lasts.
            /// </summary>
            void StreamBeacons()
            {
                int n = SlotCount;
                for (int i = 0; i < n; i++)
                {
                    if (_slotLeaseUntil[i] <= _p.Clock) continue;
                    if (_grid.DockConnectors[i].Status == MyShipConnectorStatus.Connected)
                    {
                        _slotLeaseUntil[i] = 0;
                        continue;
                    }
                    SendDockPose(_slotOwner[i], Op.DockBeacon, i);
                }
            }

            void SendDockPose(long address, int op, int slot)
            {
                MatrixD m = _grid.DockConnectors[slot].WorldMatrix;
                Vector3D vel = _grid.Controller != null
                    ? _grid.Controller.GetShipVelocities().LinearVelocity
                    : Vector3D.Zero; // stations have no controller and never move
                _comms.Unicast(address, op, slot, m.Translation, m.Forward, m.Up, vel);
            }

            void ExpireSlots()
            {
                for (int i = 0; i < MaxSlots; i++)
                    if (_slotOwner[i] != 0 && _p.Clock - _slotLastSeen[i] > SlotReleaseAge
                        && (i >= SlotCount || _grid.DockConnectors[i].Status != MyShipConnectorStatus.Connected))
                    {
                        _slotOwner[i] = 0;
                        _slotLeaseUntil[i] = 0;
                    }
            }

            // =================================================================
            // Commands (terminal / toolbar / fleet)
            // =================================================================

            static bool Is(string argument, string command)
            {
                return string.Equals(argument, command, StringComparison.OrdinalIgnoreCase);
            }

            public void HandleCommand(string argument)
            {
                _note = null;

                if (IsCarrier)
                {
                    if (Is(argument, "launch")) _comms.Broadcast(Op.FleetCommand, Cmd.Launch);
                    else if (Is(argument, "recall")) _comms.Broadcast(Op.FleetCommand, Cmd.Recall);
                    else if (Is(argument, "halt")) _comms.Broadcast(Op.FleetCommand, Cmd.Halt);
                    else if (Is(argument, "reset")) ResetSlots();
                    else _note = NoteUnknown;
                    return;
                }

                if (Is(argument, "start")) Start();
                else if (Is(argument, "stop")) Stop();
                else if (Is(argument, "return")) Recall();
                else if (Is(argument, "setsite")) SetSite();
                else if (Is(argument, "resetsite")) { _shaft = 0; }
                else if (Is(argument, "forget")) { _carrierAddr = 0; _slot = -1; }
                else _note = NoteUnknown;
            }

            void Start()
            {
                if (!_hasSite)
                {
                    _note = NoteNoSite;
                    return;
                }
                _autoCycle = true;
                Enter(_grid.IsConnected ? FleetState.Docked : FleetState.Transit);
            }

            void Recall()
            {
                _autoCycle = false;
                if (_grid.IsConnected) Enter(FleetState.Docked);
                else if (State != FleetState.Approach && State != FleetState.FinalDock) Enter(FleetState.RequestDock);
            }

            void Stop()
            {
                _autoCycle = false;
                Enter(_grid.IsConnected ? FleetState.Docked : FleetState.Idle);
            }

            void SetSite()
            {
                var ctrl = _grid.Controller;
                if (ctrl == null) return;
                MatrixD m = ctrl.WorldMatrix;
                _sitePos = m.Translation;
                _siteFwd = m.Forward;
                _siteUp = m.Up;
                _hasSite = true;
                _shaft = 0;
                _entryShaft = -1;
            }

            void ResetSlots()
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    _slotOwner[i] = 0;
                    _slotLeaseUntil[i] = 0;
                }
            }

            // =================================================================
            // UI
            // =================================================================

            public void AppendStatus(StringBuilder sb)
            {
                sb.Append(Config.RoleNames[(int)_cfg.Role]).Append(" | ").Append(StateNames[(int)State]);
                if (_autoCycle) sb.Append(" (auto)");
                sb.Append('\n');
                if (_note != null) sb.Append(_note).Append('\n');

                if (IsCarrier)
                {
                    int n = SlotCount;
                    for (int i = 0; i < n; i++)
                    {
                        sb.Append(" Dock ");
                        Fmt.Int(sb, i).Append(": ");
                        if (_grid.DockConnectors[i].Status == MyShipConnectorStatus.Connected) sb.Append("occupied");
                        else if (_slotLeaseUntil[i] > _p.Clock) sb.Append("inbound");
                        else sb.Append(_slotOwner[i] != 0 ? "reserved" : "free");
                        if (_slotOwner[i] != 0)
                        {
                            sb.Append(" #");
                            Fmt.Int(sb, _slotOwner[i] % 10000);
                        }
                        sb.Append('\n');
                    }
                    return;
                }

                if (_hasSite)
                {
                    sb.Append("Shaft ");
                    Fmt.Int(sb, _shaft);
                    if (State == FleetState.Working && _cfg.Role == FleetRole.Miner)
                    {
                        sb.Append("  depth ");
                        Fmt.Fixed(sb, _depth, 1).Append('/');
                        Fmt.Fixed(sb, _cfg.MineDepth, 0).Append(" m");
                    }
                    sb.Append('\n');
                }
                if (_carrierAddr != 0)
                {
                    sb.Append("Home #");
                    Fmt.Int(sb, _carrierAddr % 10000).Append(" slot ");
                    Fmt.Int(sb, _slot).Append('\n');
                }
                if (_helm.Engaged)
                {
                    sb.Append("Target ");
                    Fmt.Fixed(sb, _helm.DistanceToTarget, 1).Append(" m  err ");
                    Fmt.Fixed(sb, _helm.AlignmentError * (180 / Math.PI), 1).Append(" deg\n");
                }
            }

            // =================================================================
            // Persistence (Save() / Program() only: allocation is fine here)
            // =================================================================

            public void Serialize(StringBuilder sb)
            {
                var ic = CultureInfo.InvariantCulture;
                sb.Append(SaveVersion)
                  .Append('|').Append((int)_cfg.Role)
                  .Append('|').Append((int)State)
                  .Append('|').Append(_autoCycle ? 1 : 0)
                  .Append('|').Append(_carrierAddr.ToString(ic))
                  .Append('|').Append(_slot)
                  .Append('|').Append(_hasSite ? 1 : 0);
                AppendVec(sb, _sitePos);
                AppendVec(sb, _siteFwd);
                AppendVec(sb, _siteUp);
                sb.Append('|').Append(_shaft).Append('|');
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(_slotOwner[i].ToString(ic));
                }
            }

            static void AppendVec(StringBuilder sb, Vector3D v)
            {
                var ic = CultureInfo.InvariantCulture;
                sb.Append('|').Append(v.X.ToString("R", ic))
                  .Append('|').Append(v.Y.ToString("R", ic))
                  .Append('|').Append(v.Z.ToString("R", ic));
            }

            public void Restore(string storage)
            {
                if (string.IsNullOrEmpty(storage)) return;
                string[] f = storage.Split('|');
                if (f.Length < 18 || f[0] != SaveVersion) return;
                if (Int(f[1]) != (int)_cfg.Role) return; // role changed in Custom Data: start clean

                var saved = (FleetState)Int(f[2]);
                _autoCycle = Int(f[3]) == 1;
                long.TryParse(f[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out _carrierAddr);
                _slot = Int(f[5]);
                _hasSite = Int(f[6]) == 1;
                _sitePos = new Vector3D(Dbl(f[7]), Dbl(f[8]), Dbl(f[9]));
                _siteFwd = new Vector3D(Dbl(f[10]), Dbl(f[11]), Dbl(f[12]));
                _siteUp = new Vector3D(Dbl(f[13]), Dbl(f[14]), Dbl(f[15]));
                _shaft = Int(f[16]);

                string[] owners = f[17].Split(',');
                for (int i = 0; i < owners.Length && i < MaxSlots; i++)
                {
                    long.TryParse(owners[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out _slotOwner[i]);
                    _slotLastSeen[i] = _p.Clock; // give absent drones a full grace period after reload
                }

                if (IsCarrier) return;

                // Dock poses are stale after a reload, so any docking phase restarts from a request.
                switch (saved)
                {
                    case FleetState.Transit:
                    case FleetState.Working:
                        Enter(_hasSite ? FleetState.Transit : FleetState.Idle);
                        break;
                    case FleetState.Docked:
                    case FleetState.Undocking:
                    case FleetState.RequestDock:
                    case FleetState.Approach:
                    case FleetState.FinalDock:
                        Enter(_grid.IsConnected ? FleetState.Docked : FleetState.RequestDock);
                        break;
                    default:
                        if (_grid.IsConnected) Enter(FleetState.Docked);
                        break;
                }
            }

            static int Int(string s)
            {
                int v;
                return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : 0;
            }

            static double Dbl(string s)
            {
                double v;
                return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : 0;
            }
        }
    }
}
