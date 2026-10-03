using Sandbox.ModAPI.Ingame;
using System;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Bases. One pad server serves three cases: a carrier's pads (its drones, local channel),
        /// the mothership's pads (shuttle haulers, HQ channel) and a site hauler's bays (miners
        /// handing off ore). Carriers also run flight control (queue, launch sequence, site
        /// library) and the uplink to the mothership: status, deliveries, sites and shaft results
        /// go up (reliably); fleet commands, site assignments and map data come down.
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
            const string LogHqAssigned = "HQ: assigned site";
            static readonly string[] LogMaydays = { "MAYDAY", "MAYDAY: hull damage", "MAYDAY: hostile contact" };

            static readonly Color PadFree = new Color(0, 200, 60), PadInbound = new Color(0, 255, 80);
            static readonly Color PadBusy = new Color(0, 120, 255), PadReserved = new Color(255, 200, 0);

            // Pads (fixed capacity, never reallocated).
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
            /// <summary>Site shown on base map screens without an explicit [FM Map N].</summary>
            public int MapSite { get; private set; } = -1;
            int _syncSite = -1, _syncIdx;
            long _syncTarget;
            bool _syncHq;

            /// <summary>The mothership (learned from its HQ status), 0 when none.</summary>
            public long MotherAddr { get; private set; }
            long _maydayFrom;
            double _maydayAt = double.MinValue;

            bool IsMothership { get { return _cfg.Role == FleetRole.Mothership; } }
            /// <summary>Pads answer on the HQ channel only on the mothership.</summary>
            bool PadHq { get { return IsMothership; } }
            /// <summary>Site haulers serve bays while on station.</summary>
            bool ServesBays { get { return _cfg.Role == FleetRole.Hauler && !_cfg.Shuttle && State == FleetState.Working && !_closing; } }

            public SiteMap Library(int id) { return id >= 0 && id < LibrarySize ? _library[id] : null; }

            void BaseUpdate100()
            {
                ExpireSlots();
                Expire(_dockQueue, _dockQueueSeen, QueueTimeout, true);
                if (!IsMothership)
                {
                    Expire(_launchQueue, _launchQueueSeen, LaunchQueueTimeout, false);
                    RunLaunchSequence();
                }
                RunSync();
                UpdatePads();

                // Status down to our fleet (carriers) and up/across the HQ channel.
                var ctrl = _grid.Controller;
                Vector3D pos = ctrl != null ? ctrl.GetPosition() : _p.Me.GetPosition();
                Vector3D vel = ctrl != null ? ctrl.GetShipVelocities().LinearVelocity : Vector3D.Zero;
                var summary = new Vector3D(_grid.CargoFill, 1, CountBelow());
                var site = new Vector3D(MapSite, -1, 0);
                if (!IsMothership) _comms.Broadcast(Op.Status, (int)FleetState.Carrier, summary, pos, site, vel);
                _comms.Broadcast(Op.Status, (int)State, summary, pos, site, vel, true);
            }

            /// <summary>Peers one tier down (drones for a carrier, carriers and haulers for the mothership).</summary>
            int CountBelow()
            {
                int n = 0;
                for (int i = 0; i < _comms.PeerCount; i++)
                    if (_comms.PeerRole[i] < (int)_cfg.Role) n++;
                return n;
            }

            void HandleAsBase(FleetMessage m)
            {
                switch (m.Op)
                {
                    case Op.DockRequest:
                        if (m.Hq == PadHq) OnDockRequest(m.Source, m.Arg);
                        break;

                    case Op.DockRelease:
                        ReleaseSlots(m.Source);
                        break;

                    case Op.Status:
                        TouchSlots(m.Source);
                        if (m.Hq && m.Arg == (int)FleetState.Mothership) MotherAddr = m.Source;
                        break;

                    case Op.LaunchRequest:
                        if (!m.Hq) Touch(_launchQueue, _launchQueueSeen, false, m.Source);
                        break;

                    case Op.Delivery:
                        double total = Add(0, m.A) + Add(3, m.B) + Add(6, m.C) + Add(9, m.D);
                        _p.Log.Add(_comms.NameOf(m.Source), LogDelivered, (long)total, Kg);
                        if (!m.Hq) Uplink(Op.Delivery, 0, m.A, m.B, m.C, m.D);
                        break;

                    case Op.SiteDef:
                        var def = Library(m.Arg);
                        if (def == null) break;
                        def.Define(m.A, m.B, m.C, m.D.X, m.D.Y, (int)m.D.Z);
                        MapSite = m.Arg;
                        _p.Log.Add(_comms.NameOf(m.Source), LogCharted, m.Arg);
                        if (!m.Hq) Uplink(Op.SiteDef, m.Arg, m.A, m.B, m.C, m.D);
                        break;

                    case Op.SiteQuery:
                        var query = Library(m.Arg);
                        if (!m.Hq && query != null && query.Defined) OfferSite(m.Source, m.Arg, false);
                        break;

                    case Op.SiteOffer:
                        // Carrier: the mothership assigns a site; chart it and send our drones there.
                        if (IsMothership || !m.Hq || (MotherAddr != 0 && m.Source != MotherAddr)) break;
                        var offered = Library(m.Arg);
                        if (offered == null) break;
                        offered.Define(m.A, m.B, m.C, m.D.X, m.D.Y, (int)m.D.Z);
                        MapSite = m.Arg;
                        OfferSite(0, m.Arg, false);
                        _p.Log.Add(_cfg.Name, LogHqAssigned, m.Arg);
                        break;

                    case Op.ShaftReport:
                        var site = Library(m.Arg);
                        if (site == null || !site.Defined) break;
                        int shaft = (int)m.A.X, status = (int)m.A.Y;
                        double mean = site.MeanValue();
                        site.Apply(shaft, status, m.A.Z, m.B.X, m.Source, _p.Clock);
                        if (status == SiteMap.Done && shaft >= 0 && shaft < SiteMap.Cap && site.IsRich(shaft, mean))
                        {
                            _p.Log.Add(_comms.NameOf(m.Source), LogRich, shaft);
                            _grid.FireHooks(HookRich);
                        }
                        if (IsMothership) break;
                        if (!m.Hq && status >= SiteMap.Partial && m.B.Y == 0) Uplink(Op.ShaftReport, m.Arg, m.A, m.B, Vector3D.Zero, Vector3D.Zero);
                        // Map data from HQ reaches our drones too.
                        if (m.Hq && m.Arg == MapSite) _comms.Broadcast(Op.ShaftReport, m.Arg, m.A, m.B);
                        break;

                    case Op.Distress:
                        if (m.Source == _maydayFrom && _p.Clock - _maydayAt < MaydayRepeat) break;
                        _maydayFrom = m.Source;
                        _maydayAt = _p.Clock;
                        _p.Log.Add(_comms.NameOf(m.Source), LogMaydays[Math.Max(0, Math.Min(2, m.Arg))]);
                        _grid.FireHooks(HookDistress);
                        if (IsMothership) break;
                        _comms.Broadcast(Op.Distress, m.Arg, m.A, m.B, m.C, m.D, true); // tell HQ
                        if (_cfg.RecallOnDistress)
                        {
                            _comms.Broadcast(Op.FleetCommand, Cmd.Recall);
                            _p.Log.Add(_cfg.Name, LogFleetRecall);
                        }
                        break;

                    case Op.FleetCommand:
                        // Carrier: orders from the mothership go down to our drones.
                        if (!IsMothership && m.Hq && (MotherAddr == 0 || m.Source == MotherAddr))
                            _comms.Broadcast(Op.FleetCommand, m.Arg);
                        break;
                }
            }

            /// <summary>Carrier -> mothership, reliably (queued until the mothership is in range).</summary>
            void Uplink(int op, int arg, Vector3D a, Vector3D b, Vector3D c, Vector3D d)
            {
                if (!IsMothership && MotherAddr != 0)
                    _comms.SendReliable(MotherAddr, op, arg, a, b, c, d, true);
            }

            double Add(int first, Vector3D v)
            {
                Delivered[first] += Math.Max(0, v.X);
                Delivered[first + 1] += Math.Max(0, v.Y);
                Delivered[first + 2] += Math.Max(0, v.Z);
                return v.X + v.Y + v.Z;
            }

            // ---------------- Pad server ----------------

            /// <summary>
            /// Bases: free pads go to queued drones first, in arrival order; everyone else is told
            /// their place and holds off. Site haulers: only miners working our site, no queue.
            /// </summary>
            void OnDockRequest(long source, int preferred)
            {
                bool bays = !_cfg.IsBase;
                if (bays)
                {
                    int peer = _comms.IndexOfPeer(source);
                    if (peer < 0 || _comms.PeerSite[peer] != SiteId) return;
                }

                int slot = OwnedSlot(source);
                bool fresh = slot < 0 || _slotLeaseUntil[slot] <= _p.Clock;
                if (slot < 0)
                {
                    int place = Find(_dockQueue, DockQueueCount, source);
                    if (bays || DockQueueCount == 0 || (place >= 0 && place < FreeSlots()))
                        slot = AllocateSlot(source, preferred);
                }

                if (slot < 0)
                {
                    if (bays)
                    {
                        _comms.Unicast(source, Op.DockDeny, 0);
                        return;
                    }
                    bool queued = Find(_dockQueue, DockQueueCount, source) >= 0;
                    Touch(_dockQueue, _dockQueueSeen, true, source);
                    int place = Find(_dockQueue, DockQueueCount, source) + 1;
                    if (!queued) _p.Log.Add(_comms.NameOf(source), LogHold, place);
                    _comms.Unicast(source, Op.DockDeny, place, default(Vector3D), default(Vector3D), default(Vector3D), default(Vector3D), PadHq);
                    return;
                }

                RemoveAt(_dockQueue, _dockQueueSeen, true, Find(_dockQueue, DockQueueCount, source));
                _slotLeaseUntil[slot] = _p.Clock + DockLease;
                _slotLastSeen[slot] = _p.Clock;
                SendDockPose(source, Op.DockAssign, slot);
                if (fresh) _p.Log.Add(_comms.NameOf(source), LogDockCleared, slot);
            }

            void ReleaseSlots(long source)
            {
                for (int i = 0; i < MaxSlots; i++)
                    if (_slotOwner[i] == source)
                    {
                        _slotLeaseUntil[i] = 0;
                        _slotLastSeen[i] = _p.Clock;
                    }
            }

            void TouchSlots(long source)
            {
                for (int i = 0; i < MaxSlots; i++)
                    if (_slotOwner[i] == source) _slotLastSeen[i] = _p.Clock;
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

            /// <summary>Site hauler: bays busy (a miner connected or on final approach).</summary>
            bool BaysBusy()
            {
                for (int i = 0; i < SlotCount; i++)
                    if (SlotState(i) == 1 || SlotState(i) == 2) return true;
                return false;
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
            /// The one sanctioned Update10 network send: a ship on final approach needs fresh
            /// poses of a moving pad, so we stream them while its lease lasts.
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
                _comms.Unicast(address, op, slot, m.Translation, m.Forward, m.Up, vel, PadHq);
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
                bool inbound = false;
                for (int i = 0; i < SlotCount; i++)
                {
                    switch (SlotState(i))
                    {
                        case 0: _grid.SetPadLight(i, PadFree, 0); break;
                        case 1: _grid.SetPadLight(i, PadInbound, 0.5f); inbound = true; break;
                        case 2: _grid.SetPadLight(i, PadBusy, 0); break;
                        default: _grid.SetPadLight(i, PadReserved, 0); break;
                    }
                }
                _grid.SetSearchlights(inbound); // [FM Searchlight]s track ships on final approach
            }

            // ---------------- Launch sequencing (carrier) ----------------

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

            /// <summary>Place of a drone in the dock queue (1-based), 0 if not queued.</summary>
            public int DockQueuePlace(long address) { return Find(_dockQueue, DockQueueCount, address) + 1; }

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

            /// <summary>Offers a library site (target 0 = everyone on that channel), then streams its map.</summary>
            void OfferSite(long target, int id, bool hq)
            {
                var s = _library[id];
                var spacing = new Vector3D(s.SpacingX, s.SpacingY, s.Limit);
                if (target == 0) _comms.Broadcast(Op.SiteOffer, id, s.Pos, s.Fwd, s.Up, spacing, hq);
                else _comms.Unicast(target, Op.SiteOffer, id, s.Pos, s.Fwd, s.Up, spacing, hq);
                _syncSite = id;
                _syncTarget = target;
                _syncHq = hq;
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
                    if (s.Status[i] == SiteMap.Untouched && s.Face[i] < 0) continue;
                    var report = new Vector3D(i, s.Status[i], s.Value[i]);
                    var face = new Vector3D(s.Face[i], 1, 0); // map sync: apply, don't forward
                    if (_syncTarget == 0) _comms.Broadcast(Op.ShaftReport, _syncSite, report, face, default(Vector3D), default(Vector3D), _syncHq);
                    else _comms.Unicast(_syncTarget, Op.ShaftReport, _syncSite, report, face, default(Vector3D), default(Vector3D), _syncHq);
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
                OfferSite(0, id, IsMothership);
                _p.Log.Add(_cfg.Name, LogAssigned, id);
            }
        }
    }
}
