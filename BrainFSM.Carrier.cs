using Sandbox.ModAPI.Ingame;
using System;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Carrier role: flight control. Hands out dock pads and queues drones when they are
        /// full, sequences launches, keeps the shared site library, and tallies deliveries.
        /// </summary>
        public partial class BrainFSM
        {
            public const int MaxSlots = 16, LibrarySize = 8;
            const int MaxQueue = 16, SyncBatch = 30;
            const double SlotReleaseAge = 600;   // s before an absent drone loses its reserved slot
            const double QueueTimeout = 10;      // s a queued drone may go quiet before losing its place
            const double LaunchQueueTimeout = 20;
            const double MaydayRepeat = 60;      // s before the same drone's mayday is acted on again

            const string LogDockCleared = "cleared to dock, pad";
            const string LogHold = "docks full, holding at";
            const string LogCharted = "charted site";
            const string LogAssigned = "all drones to site";
            const string LogFleetRecall = "fleet recall";
            static readonly string[] LogMaydays = { "MAYDAY", "MAYDAY: hull damage", "MAYDAY: hostile contact" };

            static readonly Color PadFree = new Color(0, 200, 60), PadInbound = new Color(0, 255, 80);
            static readonly Color PadBusy = new Color(0, 120, 255), PadReserved = new Color(255, 200, 0);

            // Dock pads (fixed capacity, never reallocated).
            readonly long[] _slotOwner = new long[MaxSlots];
            readonly double[] _slotLeaseUntil = new double[MaxSlots];
            readonly double[] _slotLastSeen = new double[MaxSlots];

            // Queues keep arrival order; entries are removed by shifting (n <= 16).
            readonly long[] _dockQueue = new long[MaxQueue];
            readonly double[] _dockQueueSeen = new double[MaxQueue];
            readonly long[] _launchQueue = new long[MaxQueue];
            readonly double[] _launchQueueSeen = new double[MaxQueue];
            public int DockQueueCount { get; private set; }
            public int LaunchQueueCount { get; private set; }
            double _nextLaunchAt;
            /// <summary>Drone currently counting down to launch, and when it goes.</summary>
            public long Launching { get; private set; }
            public double LaunchingAt { get; private set; }

            // Site library and the map sync job that follows a SiteOffer.
            readonly SiteMap[] _library = new SiteMap[LibrarySize];
            /// <summary>Site shown on carrier map screens without an explicit [FM Map N].</summary>
            public int MapSite { get; private set; } = -1;
            int _syncSite = -1, _syncIdx;
            long _syncTarget;

            long _maydayFrom;
            double _maydayAt = double.MinValue;

            public SiteMap Library(int id) { return id >= 0 && id < LibrarySize ? _library[id] : null; }

            void CarrierUpdate100()
            {
                ExpireSlots();
                Expire(_dockQueue, _dockQueueSeen, QueueTimeout, true);
                Expire(_launchQueue, _launchQueueSeen, LaunchQueueTimeout, false);
                RunLaunchSequence();
                RunSync();
                UpdatePads();

                var ctrl = _grid.Controller;
                _comms.Broadcast(Op.Status, (int)FleetState.Carrier, Vector3D.Zero,
                    ctrl != null ? ctrl.GetPosition() : _p.Me.GetPosition(),
                    new Vector3D(MapSite, -1, 0),
                    ctrl != null ? ctrl.GetShipVelocities().LinearVelocity : Vector3D.Zero);
            }

            void HandleAsCarrier(FleetMessage m)
            {
                switch (m.Op)
                {
                    case Op.DockRequest:
                        OnDockRequest(m.Source, m.Arg);
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

                    case Op.LaunchRequest:
                        Touch(_launchQueue, _launchQueueSeen, false, m.Source);
                        break;

                    case Op.Delivery:
                        double total = Add(0, m.A) + Add(3, m.B) + Add(6, m.C) + Add(9, m.D);
                        _p.Log.Add(_comms.NameOf(m.Source), LogDelivered, (long)total, Kg);
                        break;

                    case Op.SiteDef:
                        var def = Library(m.Arg);
                        if (def == null) break;
                        def.Define(m.A, m.B, m.C, m.D.X, m.D.Y, (int)m.D.Z);
                        MapSite = m.Arg;
                        _p.Log.Add(_comms.NameOf(m.Source), LogCharted, m.Arg);
                        break;

                    case Op.SiteQuery:
                        var query = Library(m.Arg);
                        if (query != null && query.Defined) OfferSite(m.Source, m.Arg);
                        break;

                    case Op.ShaftReport:
                        var site = Library(m.Arg);
                        if (site == null || !site.Defined) break;
                        int shaft = (int)m.A.X, status = (int)m.A.Y;
                        double mean = site.MeanValue();
                        site.Apply(shaft, status, m.A.Z, m.Source, _p.Clock);
                        if (status == SiteMap.Done && shaft >= 0 && shaft < SiteMap.Cap && site.IsRich(shaft, mean))
                        {
                            _p.Log.Add(_comms.NameOf(m.Source), LogRich, shaft);
                            _grid.FireHooks(HookRich);
                        }
                        break;

                    case Op.Distress:
                        if (m.Source == _maydayFrom && _p.Clock - _maydayAt < MaydayRepeat) break;
                        _maydayFrom = m.Source;
                        _maydayAt = _p.Clock;
                        _p.Log.Add(_comms.NameOf(m.Source), LogMaydays[Math.Max(0, Math.Min(2, m.Arg))]);
                        _grid.FireHooks(HookDistress);
                        if (_cfg.RecallOnDistress)
                        {
                            _comms.Broadcast(Op.FleetCommand, Cmd.Recall);
                            _p.Log.Add(_cfg.Name, LogFleetRecall);
                        }
                        break;
                }
            }

            double Add(int first, Vector3D v)
            {
                Delivered[first] += Math.Max(0, v.X);
                Delivered[first + 1] += Math.Max(0, v.Y);
                Delivered[first + 2] += Math.Max(0, v.Z);
                return v.X + v.Y + v.Z;
            }

            // ---------------- Dock pads and queue ----------------

            /// <summary>
            /// Free pads go to queued drones first, in arrival order. Everyone else is told
            /// their place in the queue and holds off the carrier until called.
            /// </summary>
            void OnDockRequest(long source, int preferred)
            {
                int slot = OwnedSlot(source);
                bool fresh = slot < 0 || _slotLeaseUntil[slot] <= _p.Clock;
                if (slot < 0)
                {
                    int place = Find(_dockQueue, DockQueueCount, source);
                    if (DockQueueCount == 0 || (place >= 0 && place < FreeSlots()))
                        slot = AllocateSlot(source, preferred);
                }

                if (slot < 0)
                {
                    bool queued = Find(_dockQueue, DockQueueCount, source) >= 0;
                    Touch(_dockQueue, _dockQueueSeen, true, source);
                    int place = Find(_dockQueue, DockQueueCount, source) + 1;
                    if (!queued) _p.Log.Add(_comms.NameOf(source), LogHold, place);
                    _comms.Unicast(source, Op.DockDeny, place);
                    return;
                }

                RemoveAt(_dockQueue, _dockQueueSeen, true, Find(_dockQueue, DockQueueCount, source));
                _slotLeaseUntil[slot] = _p.Clock + DockLease;
                _slotLastSeen[slot] = _p.Clock;
                SendDockPose(source, Op.DockAssign, slot);
                if (fresh) _p.Log.Add(_comms.NameOf(source), LogDockCleared, slot);
            }

            public int SlotCount { get { return Math.Min(_grid.DockConnectors.Count, MaxSlots); } }

            /// <summary>0 free, 1 inbound, 2 occupied, 3 reserved (for screens and pad lights).</summary>
            public int SlotState(int i)
            {
                if (_grid.DockConnectors[i].Status == MyShipConnectorStatus.Connected) return 2;
                if (_slotLeaseUntil[i] > _p.Clock) return 1;
                return _slotOwner[i] != 0 ? 3 : 0;
            }

            bool SlotFree(int i)
            {
                return _grid.DockConnectors[i].Status != MyShipConnectorStatus.Connected
                    && (_slotOwner[i] == 0 || _p.Clock - _slotLastSeen[i] > SlotReleaseAge);
            }

            int FreeSlots()
            {
                int n = 0;
                for (int i = 0; i < SlotCount; i++)
                    if (SlotFree(i)) n++;
                return n;
            }

            int OwnedSlot(long requester)
            {
                for (int i = 0; i < SlotCount; i++)
                    if (_slotOwner[i] == requester) return i;
                return -1;
            }

            int AllocateSlot(long requester, int preferred)
            {
                int n = SlotCount;
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

            void ResetSlots()
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    _slotOwner[i] = 0;
                    _slotLeaseUntil[i] = 0;
                }
                DockQueueCount = 0;
                LaunchQueueCount = 0;
            }

            void UpdatePads()
            {
                for (int i = 0; i < SlotCount; i++)
                {
                    switch (SlotState(i))
                    {
                        case 0: _grid.SetPadLight(i, PadFree, 0); break;
                        case 1: _grid.SetPadLight(i, PadInbound, 0.5f); break;
                        case 2: _grid.SetPadLight(i, PadBusy, 0); break;
                        default: _grid.SetPadLight(i, PadReserved, 0); break;
                    }
                }
            }

            // ---------------- Launch sequencing ----------------

            /// <summary>One drone at a time, LaunchInterval apart, each with a countdown.</summary>
            void RunLaunchSequence()
            {
                if (LaunchQueueCount == 0 || _p.Clock < _nextLaunchAt) return;
                long drone = _launchQueue[0];
                RemoveAt(_launchQueue, _launchQueueSeen, false, 0);
                int countdown = (int)Math.Round(Math.Max(0, _cfg.LaunchCountdown));
                _comms.Unicast(drone, Op.LaunchClear, countdown);
                Launching = drone;
                LaunchingAt = _p.Clock + countdown;
                _nextLaunchAt = _p.Clock + Math.Max(_cfg.LaunchInterval, countdown);
                _p.Log.Add(_comms.NameOf(drone), LogCleared, countdown, Sec);
                _grid.FireHooks(HookLaunch);
            }

            /// <summary>Place of a drone in the dock or launch queue (1-based), 0 if not queued.</summary>
            public int DockQueuePlace(long address) { return Find(_dockQueue, DockQueueCount, address) + 1; }
            public int LaunchQueuePlace(long address) { return Find(_launchQueue, LaunchQueueCount, address) + 1; }

            // ---------------- Queue helpers (dock = true picks the dock queue) ----------------

            static int Find(long[] q, int n, long address)
            {
                for (int i = 0; i < n; i++)
                    if (q[i] == address) return i;
                return -1;
            }

            /// <summary>Refreshes a queued drone, or appends it.</summary>
            void Touch(long[] q, double[] seen, bool dock, long address)
            {
                int n = dock ? DockQueueCount : LaunchQueueCount;
                int i = Find(q, n, address);
                if (i < 0)
                {
                    if (n == MaxQueue) return;
                    i = n++;
                    q[i] = address;
                    if (dock) DockQueueCount = n; else LaunchQueueCount = n;
                }
                seen[i] = _p.Clock;
            }

            void RemoveAt(long[] q, double[] seen, bool dock, int index)
            {
                int n = dock ? DockQueueCount : LaunchQueueCount;
                if (index < 0 || index >= n) return;
                for (int i = index; i < n - 1; i++)
                {
                    q[i] = q[i + 1];
                    seen[i] = seen[i + 1];
                }
                if (dock) DockQueueCount = n - 1; else LaunchQueueCount = n - 1;
            }

            void Expire(long[] q, double[] seen, double timeout, bool dock)
            {
                int n = dock ? DockQueueCount : LaunchQueueCount;
                for (int i = n - 1; i >= 0; i--)
                    if (_p.Clock - seen[i] > timeout) RemoveAt(q, seen, dock, i);
            }

            // ---------------- Site library ----------------

            /// <summary>Offers a library site (target 0 = every drone), then streams its map.</summary>
            void OfferSite(long target, int id)
            {
                var s = _library[id];
                var spacing = new Vector3D(s.SpacingX, s.SpacingY, s.Limit);
                if (target == 0) _comms.Broadcast(Op.SiteOffer, id, s.Pos, s.Fwd, s.Up, spacing);
                else _comms.Unicast(target, Op.SiteOffer, id, s.Pos, s.Fwd, s.Up, spacing);
                _syncSite = id;
                _syncTarget = target;
                _syncIdx = 0;
            }

            void RunSync()
            {
                if (_syncSite < 0) return;
                var s = _library[_syncSite];
                int sent = 0;
                while (_syncIdx < SiteMap.Cap && sent < SyncBatch)
                {
                    int i = _syncIdx++;
                    if (s.Status[i] == SiteMap.Untouched) continue;
                    var report = new Vector3D(i, s.Status[i], s.Value[i]);
                    if (_syncTarget == 0) _comms.Broadcast(Op.ShaftReport, _syncSite, report);
                    else _comms.Unicast(_syncTarget, Op.ShaftReport, _syncSite, report);
                    sent++;
                }
                if (_syncIdx >= SiteMap.Cap) _syncSite = -1;
            }

            void AssignSite(int id)
            {
                var s = Library(id);
                if (s == null || !s.Defined)
                {
                    _note = NoteUnknownSite;
                    return;
                }
                MapSite = id;
                OfferSite(0, id);
                _p.Log.Add(_cfg.Name, LogAssigned, id);
            }
        }
    }
}
