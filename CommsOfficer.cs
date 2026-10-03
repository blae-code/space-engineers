using Sandbox.ModAPI.Ingame;
using System.Text;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>Fleet message opcodes (FleetMessage.Op).</summary>
        public static class Op
        {
            public const int Status = 1;        // all -> all     Arg=state A=(cargo,batt,h2) B=pos C=(site,shaft,distress) D=vel
            public const int DockRequest = 2;   // drone -> carrier  Arg=preferred slot (-1 = any); also renews the lease
            public const int DockAssign = 3;    // carrier -> drone  Arg=slot A=pos B=fwd C=up D=carrier vel
            public const int DockBeacon = 4;    // carrier -> drone  same payload as DockAssign, streamed on Update10
            public const int DockDeny = 5;      // carrier -> drone  Arg=place in the dock queue (1 = next)
            public const int DockRelease = 6;   // drone -> carrier  Arg=slot, drone is leaving
            public const int FleetCommand = 7;  // carrier -> all    Arg=Cmd.*
            public const int Hello = 8;         // all -> all     Text=callsign Arg=role
            public const int SiteDef = 9;       // drone -> carrier  Arg=site A=pos B=fwd C=up D=(spacingX,spacingY,maxShafts): store it
            public const int SiteQuery = 10;    // drone -> carrier  Arg=site: please offer it to me
            public const int SiteOffer = 11;    // carrier -> drone(s) same payload as SiteDef: adopt it
            public const int ShaftReport = 12;  // all -> all     Arg=site A=(shaft,status,value)
            public const int LaunchRequest = 13;// drone -> carrier  ready to launch, waiting for clearance
            public const int LaunchClear = 14;  // carrier -> drone  Arg=countdown seconds
            public const int Delivery = 15;     // drone -> carrier  A..D = kg delivered per ore (Ore.Names order, 3 per vector)
            public const int Distress = 16;     // drone -> all     Arg=Distress.* B=pos
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
            public static readonly string[] Names = { "none", "hull damage", "hostile contact" };
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
        }

        /// <summary>
        /// IGC mesh networking. Payloads travel as MyTuples, which IGC accepts natively:
        /// no string building on send, no parsing on receive. Also keeps the peer table
        /// (who is out there, what they are doing) that the carrier's board shows.
        /// </summary>
        public class CommsOfficer : ISubsystem
        {
            public const int MaxPeers = 32;
            const double PeerTimeout = 30;
            const int HelloEvery = 10;           // Update100 ticks between callsign announcements
            const string Unknown = "UNKNOWN";
            const string LostContact = "lost contact";

            readonly Program _p;
            readonly string _tag;
            readonly FleetMessage _inbox = new FleetMessage();
            IMyBroadcastListener _broadcast;
            int _helloCountdown;

            // Peer table as parallel arrays so upserts never allocate.
            public int PeerCount { get; private set; }
            public readonly long[] PeerAddress = new long[MaxPeers];
            public readonly string[] PeerName = new string[MaxPeers];
            public readonly int[] PeerRole = new int[MaxPeers];
            public readonly int[] PeerState = new int[MaxPeers];
            public readonly double[] PeerCargo = new double[MaxPeers];
            public readonly double[] PeerBattery = new double[MaxPeers];
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
            }

            public void Initialize()
            {
                _broadcast = _p.IGC.RegisterBroadcastListener(_tag);
                _broadcast.SetMessageCallback(_tag);
                _p.IGC.UnicastListener.SetMessageCallback(_tag);
            }

            public void Update10() { }

            public void Update100()
            {
                if (--_helloCountdown <= 0)
                {
                    _helloCountdown = HelloEvery;
                    _p.IGC.SendBroadcastMessage(_tag, MyTuple.Create(Op.Hello, (int)_p.Cfg.Role, _p.Cfg.Name));
                }

                // Drop peers we have not heard from (swap-remove, no allocation).
                for (int i = PeerCount - 1; i >= 0; i--)
                {
                    if (_p.Clock - PeerSeen[i] <= PeerTimeout) continue;
                    if (PeerState[i] != (int)FleetState.Docked && PeerRole[i] != (int)FleetRole.Carrier)
                        _p.Log.Add(PeerName[i], LostContact);
                    int last = PeerCount - 1;
                    PeerAddress[i] = PeerAddress[last];
                    PeerName[i] = PeerName[last];
                    PeerRole[i] = PeerRole[last];
                    PeerState[i] = PeerState[last];
                    PeerCargo[i] = PeerCargo[last];
                    PeerBattery[i] = PeerBattery[last];
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
                        PeerPos[i] = m.B;
                        PeerSite[i] = (int)m.C.X;
                        PeerShaft[i] = (int)m.C.Y;
                        PeerDistress[i] = (int)m.C.Z;
                        PeerVel[i] = m.D;
                        if (m.Arg == (int)FleetState.Carrier) PeerRole[i] = (int)FleetRole.Carrier;
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
                while (_broadcast.HasPendingMessage)
                    Dispatch(_broadcast.AcceptMessage(), subsystems);

                var unicast = _p.IGC.UnicastListener;
                while (unicast.HasPendingMessage)
                    Dispatch(unicast.AcceptMessage(), subsystems);
            }

            void Dispatch(MyIGCMessage msg, ISubsystem[] subsystems)
            {
                if (msg.Tag != _tag || msg.Source == _p.IGC.Me) return;

                object data = msg.Data;
                if (data is MyTuple<int, int, Vector3D, Vector3D, Vector3D, Vector3D>)
                {
                    var d = (MyTuple<int, int, Vector3D, Vector3D, Vector3D, Vector3D>)data;
                    _inbox.Op = d.Item1;
                    _inbox.Arg = d.Item2;
                    _inbox.A = d.Item3;
                    _inbox.B = d.Item4;
                    _inbox.C = d.Item5;
                    _inbox.D = d.Item6;
                    _inbox.Text = null;
                }
                else if (data is MyTuple<int, int, string>)
                {
                    var t = (MyTuple<int, int, string>)data;
                    _inbox.Op = t.Item1;
                    _inbox.Arg = t.Item2;
                    _inbox.Text = t.Item3;
                    _inbox.A = _inbox.B = _inbox.C = _inbox.D = Vector3D.Zero;
                }
                else return;

                _inbox.Source = msg.Source;
                for (int i = 0; i < subsystems.Length; i++)
                    subsystems[i].HandleMessage(_inbox);
            }

            // ---------------- Send ----------------

            public void Broadcast(int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D))
            {
                _p.IGC.SendBroadcastMessage(_tag, MyTuple.Create(op, arg, a, b, c, d));
            }

            /// <summary>Returns false when the recipient is out of range or unknown.</summary>
            public bool Unicast(long address, int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D))
            {
                return _p.IGC.SendUnicastMessage(address, _tag, MyTuple.Create(op, arg, a, b, c, d));
            }

            /// <summary>Unicast when the address is known and reachable, broadcast otherwise.</summary>
            public void Send(long address, int op, int arg,
                Vector3D a = default(Vector3D), Vector3D b = default(Vector3D),
                Vector3D c = default(Vector3D), Vector3D d = default(Vector3D))
            {
                if (address == 0 || !Unicast(address, op, arg, a, b, c, d))
                    Broadcast(op, arg, a, b, c, d);
            }

            // ---------------- UI ----------------

            public void AppendStatus(StringBuilder sb)
            {
                if (PeerCount == 0) return;
                sb.Append("Peers: ");
                Fmt.Int(sb, PeerCount).Append('\n');
                for (int i = 0; i < PeerCount && i < 8; i++)
                {
                    sb.Append(' ').Append(PeerName[i]).Append(' ');
                    int s = PeerState[i];
                    sb.Append(s >= 0 && s < BrainFSM.StateNames.Length ? BrainFSM.StateNames[s] : "?");
                    if (PeerRole[i] != (int)FleetRole.Carrier)
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
