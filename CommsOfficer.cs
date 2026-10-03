using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>Fleet message opcodes (FleetMessage.Op, 8 bits).</summary>
        public static class Op
        {
            public const int Status = 1;        // all -> all     Arg=state A=(cargo,batt,h2) B=pos C=(site,shaft,distress) D=vel
                                                //   bases: A=(fill, 1, drones or carriers), C=(mapSite,-1,0)
            public const int DockRequest = 2;   // -> pad server  Arg=preferred slot (-1 = any); also renews the lease
            public const int DockAssign = 3;    // pad server ->  Arg=slot A=pos B=fwd C=up D=server vel
            public const int DockBeacon = 4;    // pad server ->  same payload as DockAssign, streamed on Update10
            public const int DockDeny = 5;      // pad server ->  Arg=place in the dock queue (1 = next)
            public const int DockRelease = 6;   // -> pad server  Arg=slot, leaving
            public const int FleetCommand = 7;  // base -> all    Arg=Cmd.*
            public const int Hello = 8;         // all -> all     Text=callsign Arg=role
            public const int SiteDef = 9;       // -> base        Arg=site A=pos B=fwd C=up D=(spacingX,spacingY,maxShafts): chart it   [reliable]
            public const int SiteQuery = 10;    // drone -> carrier  Arg=site: please offer it to me
            public const int SiteOffer = 11;    // base -> below  same payload as SiteDef: adopt it
            public const int ShaftReport = 12;  // all -> all     Arg=site A=(shaft,status,value) B.X=face distance (-1 unknown)  [reliable to bases]
            public const int LaunchRequest = 13;// drone -> carrier  ready to launch, waiting for clearance
            public const int LaunchClear = 14;  // carrier -> drone  Arg=countdown seconds
            public const int Delivery = 15;     // -> base        A..D = kg delivered per ore (Ore.Names order, 3 per vector)  [reliable]
            public const int Distress = 16;     // drone -> all   Arg=Distress.* B=pos C=threat pos D=threat vel
            public const int Ack = 17;          // -> sender      Arg=sequence number being acknowledged
            public const int HaulerOffer = 18;  // hauler -> miners  Arg=site A=(free bays, fill, 0) B=pos
        }

        /// <summary>Arguments for Op.FleetCommand.</summary>
        public static class Cmd
        {
            public const int Launch = 1;
            public const int Recall = 2;
            public const int Halt = 3;
        }

        /// <summary>Reasons carried by Op.Distress.</summary>
        public static class Distress
        {
            public const int None = 0, Damage = 1, Hostile = 2;
        }

        /// <summary>
        /// Decoded fleet message. A single instance is reused for every message,
        /// so handlers must copy anything they want to keep.
        /// </summary>
        public class FleetMessage
        {
            public long Source;
            public int Op;
            public int Arg;
            public Vector3D A, B, C, D;
            public string Text;
            /// <summary>Arrived on the HQ channel (carrier / mothership layer).</summary>
            public bool Hq;
        }

        /// <summary>
        /// IGC networking on two channels: the local one (a carrier and its drones) and the HQ
        /// one (carriers, the mothership, shuttle haulers). Every message carries a header
        /// [op:8 | fleet key:24 | sequence:31]; messages with another key are ignored. Reliable
        /// messages sit in an outbox and are resent until acknowledged, so reports made out of
        /// range are delivered when the link comes back (store and forward).
        /// </summary>
        public class CommsOfficer : ISubsystem
        {
            public const int MaxPeers = 32;
            const int OutboxSize = 24, SeenSize = 48;
            const double PeerTimeout = 30, RetryInterval = 4, MessageLife = 300;
            const int HelloEvery = 10;           // Update100 ticks between callsign announcements
            const string Unknown = "UNKNOWN";
            const string LostContact = "lost contact";

            readonly Program _p;
            readonly string _tag, _hqTag;
            readonly FleetMessage _inbox = new FleetMessage();
            IMyBroadcastListener _broadcast, _hqBroadcast;
            bool _local, _hq;
            int _helloCountdown, _seq;

            // Outbox (parallel arrays; seq 0 = free slot).
            readonly long[] _obDest = new long[OutboxSize];
            readonly int[] _obSeq = new int[OutboxSize], _obOp = new int[OutboxSize], _obArg = new int[OutboxSize];
            readonly bool[] _obHq = new bool[OutboxSize];
            readonly Vector3D[] _obA = new Vector3D[OutboxSize], _obB = new Vector3D[OutboxSize];
            readonly Vector3D[] _obC = new Vector3D[OutboxSize], _obD = new Vector3D[OutboxSize];
            readonly double[] _obNext = new double[OutboxSize], _obExpire = new double[OutboxSize];

            // Recently seen reliable messages, so a resend whose ack was lost is not applied twice.
            readonly long[] _seenSource = new long[SeenSize];
            readonly int[] _seenSeq = new int[SeenSize];
            int _seenNext;

            // Peer table as parallel arrays so upserts never allocate.
            public int PeerCount { get; private set; }
            public readonly long[] PeerAddress = new long[MaxPeers];
            public readonly string[] PeerName = new string[MaxPeers];
            public readonly int[] PeerRole = new int[MaxPeers];
            public readonly int[] PeerState = new int[MaxPeers];
            public readonly double[] PeerCargo = new double[MaxPeers];
            public readonly double[] PeerBattery = new double[MaxPeers];
            /// <summary>Status A.Z: hydrogen for drones, fleet size for bases.</summary>
            public readonly double[] PeerInfo = new double[MaxPeers];
            public readonly Vector3D[] PeerPos = new Vector3D[MaxPeers];
            public readonly Vector3D[] PeerVel = new Vector3D[MaxPeers];
            public readonly int[] PeerSite = new int[MaxPeers];
            public readonly int[] PeerShaft = new int[MaxPeers];
            public readonly int[] PeerDistress = new int[MaxPeers];
            public readonly double[] PeerDelivered = new double[MaxPeers];
            public readonly double[] PeerSeen = new double[MaxPeers];

            public CommsOfficer(Program p)
            {
                _p = p;
                _tag = p.Cfg.Channel;
                _hqTag = p.Cfg.Channel + "/HQ";
            }

            public int OutboxCount
            {
                get
                {
                    int n = 0;
                    for (int i = 0; i < OutboxSize; i++)
                        if (_obSeq[i] != 0) n++;
                    return n;
                }
            }

            public void Initialize()
            {
                var cfg = _p.Cfg;
                _local = cfg.Role != FleetRole.Mothership;
                _hq = cfg.IsBase || (cfg.Role == FleetRole.Hauler && cfg.Shuttle);
                if (_local)
                {
                    _broadcast = _p.IGC.RegisterBroadcastListener(_tag);
                    _broadcast.SetMessageCallback(_tag);
                }
                if (_hq)
                {
                    _hqBroadcast = _p.IGC.RegisterBroadcastListener(_hqTag);
                    _hqBroadcast.SetMessageCallback(_hqTag);
                }
                _p.IGC.UnicastListener.SetMessageCallback(_tag);
                // Sequence numbers restart after a reload; start somewhere new so receivers'
                // duplicate filters don't mistake fresh messages for old ones.
                _seq = (int)((DateTime.UtcNow.Ticks >> 16) & 0x3FFFFFFF);
            }

            public void Update10() { }

            public void Update100()
            {
                if (--_helloCountdown <= 0)
                {
                    _helloCountdown = HelloEvery;
                    if (_local) _p.IGC.SendBroadcastMessage(_tag, MyTuple.Create(Header(Op.Hello, 0), (int)_p.Cfg.Role, _p.Cfg.Name));
                    if (_hq) _p.IGC.SendBroadcastMessage(_hqTag, MyTuple.Create(Header(Op.Hello, 0), (int)_p.Cfg.Role, _p.Cfg.Name));
                }

                for (int i = 0; i < OutboxSize; i++)
                {
                    if (_obSeq[i] == 0) continue;
                    if (_p.Clock > _obExpire[i]) _obSeq[i] = 0;
                    else if (_p.Clock >= _obNext[i]) Transmit(i);
                }

                // Drop peers we have not heard from (swap-remove, no allocation).
                for (int i = PeerCount - 1; i >= 0; i--)
                {
                    if (_p.Clock - PeerSeen[i] <= PeerTimeout) continue;
                    if (PeerState[i] != (int)FleetState.Docked && PeerRole[i] < (int)FleetRole.Carrier)
                        _p.Log.Add(PeerName[i], LostContact);
                    int last = PeerCount - 1;
                    PeerAddress[i] = PeerAddress[last];
                    PeerName[i] = PeerName[last];
                    PeerRole[i] = PeerRole[last];
                    PeerState[i] = PeerState[last];
                    PeerCargo[i] = PeerCargo[last];
                    PeerBattery[i] = PeerBattery[last];
                    PeerInfo[i] = PeerInfo[last];
                    PeerPos[i] = PeerPos[last];
                    PeerVel[i] = PeerVel[last];
                    PeerSite[i] = PeerSite[last];
                    PeerShaft[i] = PeerShaft[last];
                    PeerDistress[i] = PeerDistress[last];
                    PeerDelivered[i] = PeerDelivered[last];
                    PeerSeen[i] = PeerSeen[last];
                    PeerCount = last;
                }
            }

            public void HandleMessage(FleetMessage m)
            {
                int i;
                switch (m.Op)
                {
                    case Op.Hello:
                        i = Upsert(m.Source);
                        PeerName[i] = m.Text ?? Unknown;
                        PeerRole[i] = m.Arg;
                        break;

                    case Op.Status:
                        i = Upsert(m.Source);
                        PeerState[i] = m.Arg;
                        PeerCargo[i] = m.A.X;
                        PeerBattery[i] = m.A.Y;
                        PeerInfo[i] = m.A.Z;
                        PeerPos[i] = m.B;
                        PeerSite[i] = (int)m.C.X;
                        PeerShaft[i] = (int)m.C.Y;
                        PeerDistress[i] = (int)m.C.Z;
                        PeerVel[i] = m.D;
                        if (m.Arg == (int)FleetState.Carrier) PeerRole[i] = (int)FleetRole.Carrier;
                        else if (m.Arg == (int)FleetState.Mothership) PeerRole[i] = (int)FleetRole.Mothership;
                        break;

                    case Op.Delivery:
                        i = Upsert(m.Source);
                        PeerDelivered[i] += m.A.X + m.A.Y + m.A.Z + m.B.X + m.B.Y + m.B.Z
                            + m.C.X + m.C.Y + m.C.Z + m.D.X + m.D.Y + m.D.Z;
                        break;

                    case Op.Distress:
                        i = Upsert(m.Source);
                        PeerDistress[i] = m.Arg;
                        PeerPos[i] = m.B;
                        break;
                }
            }

            public int IndexOfPeer(long address)
            {
                for (int i = 0; i < PeerCount; i++)
                    if (PeerAddress[i] == address) return i;
                return -1;
            }

            public string NameOf(long address)
            {
                int i = IndexOfPeer(address);
                return i >= 0 ? PeerName[i] : Unknown;
            }

            /// <summary>Peer's position now, extrapolated from its last report.</summary>
            public Vector3D PeerNow(int i)
            {
                return PeerPos[i] + PeerVel[i] * Math.Min(10, _p.Clock - PeerSeen[i]);
            }

            /// <summary>Nearest peer of a role heard within maxAge seconds (0 = none).</summary>
            public long Nearest(FleetRole role, Vector3D from, double maxAge)
            {
                long best = 0;
                double bestDist = double.MaxValue;
                for (int i = 0; i < PeerCount; i++)
                {
                    if (PeerRole[i] != (int)role || _p.Clock - PeerSeen[i] > maxAge) continue;
                    double d = Vector3D.DistanceSquared(from, PeerPos[i]);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        best = PeerAddress[i];
                    }
                }
                return best;
            }

            /// <summary>True when IGC can currently route a unicast to the address.</summary>
            public bool Reachable(long address)
            {
                return address != 0 && _p.IGC.IsEndpointReachable(address, TransmissionDistance.AntennaRelay);
            }

            int Upsert(long address)
            {
                int i = IndexOfPeer(address);
                if (i >= 0)
                {
                    PeerSeen[i] = _p.Clock;
                    return i;
                }
                if (PeerCount < MaxPeers) i = PeerCount++;
                else i = OldestPeer();
                PeerAddress[i] = address;
                PeerName[i] = Unknown;
                PeerRole[i] = 0;
                PeerState[i] = 0;
                PeerSite[i] = -1;
                PeerShaft[i] = -1;
                PeerDistress[i] = 0;
                PeerDelivered[i] = 0;
                PeerSeen[i] = _p.Clock;
                return i;
            }

            int OldestPeer()
            {
                int oldest = 0;
                for (int i = 1; i < PeerCount; i++)
                    if (PeerSeen[i] < PeerSeen[oldest]) oldest = i;
                return oldest;
            }

            // ---------------- Receive ----------------

            /// <summary>Accepts every pending message and routes it to all subsystems.</summary>
            public void Drain(ISubsystem[] subsystems)
            {
                while (_local && _broadcast.HasPendingMessage)
                    Dispatch(_broadcast.AcceptMessage(), subsystems);
                while (_hq && _hqBroadcast.HasPendingMessage)
                    Dispatch(_hqBroadcast.AcceptMessage(), subsystems);

                var unicast = _p.IGC.UnicastListener;
                while (unicast.HasPendingMessage)
                    Dispatch(unicast.AcceptMessage(), subsystems);
            }

            void Dispatch(MyIGCMessage msg, ISubsystem[] subsystems)
            {
                bool hq;
                if (_local && msg.Tag == _tag) hq = false;
                else if (_hq && msg.Tag == _hqTag) hq = true;
                else return;
                if (msg.Source == _p.IGC.Me) return;

                long header;
                object data = msg.Data;
                if (data is MyTuple<long, int, Vector3D, Vector3D, Vector3D, Vector3D>)
                {
                    var d = (MyTuple<long, int, Vector3D, Vector3D, Vector3D, Vector3D>)data;
                    header = d.Item1;
                    _inbox.Arg = d.Item2;
                    _inbox.A = d.Item3;
                    _inbox.B = d.Item4;
                    _inbox.C = d.Item5;
                    _inbox.D = d.Item6;
                    _inbox.Text = null;
                }
                else if (data is MyTuple<long, int, string>)
                {
                    var t = (MyTuple<long, int, string>)data;
                    header = t.Item1;
                    _inbox.Arg = t.Item2;
                    _inbox.Text = t.Item3;
                    _inbox.A = _inbox.B = _inbox.C = _inbox.D = Vector3D.Zero;
                }
                else return;

                if ((int)((header >> 8) & 0xFFFFFF) != _p.Cfg.KeyHash) return; // not our fleet
                int op = (int)(header & 0xFF);
                int seq = (int)(header >> 32);

                if (op == Op.Ack)
                {
                    for (int i = 0; i < OutboxSize; i++)
                        if (_obSeq[i] == _inbox.Arg && _obDest[i] == msg.Source) _obSeq[i] = 0;
                    return;
                }
                if (seq != 0)
                {
                    _p.IGC.SendUnicastMessage(msg.Source, hq ? _hqTag : _tag,
                        MyTuple.Create(Header(Op.Ack, 0), seq, Vector3D.Zero, Vector3D.Zero, Vector3D.Zero, Vector3D.Zero));
                    if (Seen(msg.Source, seq)) return;
                }

                _inbox.Source = msg.Source;
                _inbox.Op = op;
                _inbox.Hq = hq;
                for (int i = 0; i < subsystems.Length; i++)
                    subsystems[i].HandleMessage(_inbox);
            }

            bool Seen(long source, int seq)
            {
                for (int i = 0; i < SeenSize; i++)
                    if (_seenSeq[i] == seq && _seenSource[i] == source) return true;
                _seenSource[_seenNext] = source;
                _seenSeq[_seenNext] = seq;
                _seenNext = (_seenNext + 1) % SeenSize;
                return false;
            }

            // ---------------- Send ----------------

            long Header(int op, int seq)
            {
                return ((long)op & 0xFF) | ((long)_p.Cfg.KeyHash << 8) | ((long)seq << 32);
            }

            public void Broadcast(int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D), bool hq = false)
            {
                _p.IGC.SendBroadcastMessage(hq ? _hqTag : _tag, MyTuple.Create(Header(op, 0), arg, a, b, c, d));
            }

            /// <summary>Returns false when the recipient is out of range or unknown.</summary>
            public bool Unicast(long address, int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D), bool hq = false)
            {
                return _p.IGC.SendUnicastMessage(address, hq ? _hqTag : _tag, MyTuple.Create(Header(op, 0), arg, a, b, c, d));
            }

            /// <summary>Unicast when the address is known and reachable, broadcast otherwise.</summary>
            public void Send(long address, int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D), bool hq = false)
            {
                if (address == 0 || !Unicast(address, op, arg, a, b, c, d, hq))
                    Broadcast(op, arg, a, b, c, d, hq);
            }

            /// <summary>
            /// Queues a message that must arrive: resent every few seconds until the recipient
            /// acknowledges it, for up to five minutes. With no known address it is broadcast once.
            /// </summary>
            public void SendReliable(long address, int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D), bool hq = false)
            {
                if (address == 0)
                {
                    Broadcast(op, arg, a, b, c, d, hq);
                    return;
                }
                // Free slot, else the oldest message, sparing deliveries (production totals) if possible.
                int slot = -1, oldest = 0, oldestOther = -1;
                for (int i = 0; i < OutboxSize && slot < 0; i++)
                {
                    if (_obSeq[i] == 0) { slot = i; break; }
                    if (_obExpire[i] < _obExpire[oldest]) oldest = i;
                    if (_obOp[i] != Op.Delivery && (oldestOther < 0 || _obExpire[i] < _obExpire[oldestOther])) oldestOther = i;
                }
                if (slot < 0) slot = oldestOther >= 0 ? oldestOther : oldest;
                _seq = _seq % 0x3FFFFFFF + 1;
                _obSeq[slot] = _seq;
                _obDest[slot] = address;
                _obOp[slot] = op;
                _obArg[slot] = arg;
                _obA[slot] = a;
                _obB[slot] = b;
                _obC[slot] = c;
                _obD[slot] = d;
                _obHq[slot] = hq;
                _obExpire[slot] = _p.Clock + MessageLife;
                Transmit(slot);
            }

            void Transmit(int i)
            {
                _obNext[i] = _p.Clock + RetryInterval;
                _p.IGC.SendUnicastMessage(_obDest[i], _obHq[i] ? _hqTag : _tag,
                    MyTuple.Create(Header(_obOp[i], _obSeq[i]), _obArg[i], _obA[i], _obB[i], _obC[i], _obD[i]));
            }

            // ---------------- UI ----------------

            public void AppendStatus(StringBuilder sb)
            {
                int pending = OutboxCount;
                if (pending > 0)
                {
                    sb.Append("Outbox: ");
                    Fmt.Int(sb, pending).Append(" waiting for link\n");
                }
                if (PeerCount == 0) return;
                sb.Append("Peers: ");
                Fmt.Int(sb, PeerCount).Append('\n');
                for (int i = 0; i < PeerCount && i < 8; i++)
                {
                    sb.Append(' ').Append(PeerName[i]).Append(' ');
                    int s = PeerState[i];
                    sb.Append(s >= 0 && s < BrainFSM.StateLabels.Length ? BrainFSM.StateLabels[s] : "?");
                    if (PeerRole[i] < (int)FleetRole.Carrier)
                    {
                        sb.Append(" C:");
                        Fmt.Pct(sb, PeerCargo[i]).Append(" B:");
                        Fmt.Pct(sb, PeerBattery[i]);
                    }
                    sb.Append('\n');
                }
            }
        }
    }
}
