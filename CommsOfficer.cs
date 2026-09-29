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
            public const int Status = 1;        // drone -> all   Arg=state A=(cargo,batt,h2) B=pos D=vel
            public const int DockRequest = 2;   // drone -> carrier  Arg=preferred slot (-1 = any); also renews the lease
            public const int DockAssign = 3;    // carrier -> drone  Arg=slot A=pos B=fwd C=up D=carrier vel
            public const int DockBeacon = 4;    // carrier -> drone  same payload as DockAssign, streamed on Update10
            public const int DockDeny = 5;      // carrier -> drone  no free slot
            public const int DockRelease = 6;   // drone -> carrier  Arg=slot, drone is leaving
            public const int FleetCommand = 7;  // carrier -> all    Arg=Cmd.*
        }

        /// <summary>Arguments for Op.FleetCommand.</summary>
        public static class Cmd
        {
            public const int Launch = 1;
            public const int Recall = 2;
            public const int Halt = 3;
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
        }

        /// <summary>
        /// IGC mesh networking. Payloads travel as a MyTuple of value types, which
        /// IGC accepts natively: no string building on send, no parsing on receive.
        /// </summary>
        public class CommsOfficer : ISubsystem
        {
            public const int MaxPeers = 32;
            const double PeerTimeout = 30;

            readonly Program _p;
            readonly string _tag;
            readonly FleetMessage _inbox = new FleetMessage();
            IMyBroadcastListener _broadcast;

            // Peer table as parallel arrays so upserts never allocate.
            public int PeerCount { get; private set; }
            public readonly long[] PeerAddress = new long[MaxPeers];
            public readonly int[] PeerState = new int[MaxPeers];
            public readonly double[] PeerCargo = new double[MaxPeers];
            public readonly double[] PeerBattery = new double[MaxPeers];
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
                // Drop peers we have not heard from (swap-remove, no allocation).
                for (int i = PeerCount - 1; i >= 0; i--)
                {
                    if (_p.Clock - PeerSeen[i] <= PeerTimeout) continue;
                    int last = PeerCount - 1;
                    PeerAddress[i] = PeerAddress[last];
                    PeerState[i] = PeerState[last];
                    PeerCargo[i] = PeerCargo[last];
                    PeerBattery[i] = PeerBattery[last];
                    PeerSeen[i] = PeerSeen[last];
                    PeerCount = last;
                }
            }

            public void HandleMessage(FleetMessage m)
            {
                if (m.Op != Op.Status) return;

                int i = IndexOfPeer(m.Source);
                if (i < 0)
                {
                    if (PeerCount < MaxPeers) i = PeerCount++;
                    else i = OldestPeer();
                    PeerAddress[i] = m.Source;
                }
                PeerState[i] = m.Arg;
                PeerCargo[i] = m.A.X;
                PeerBattery[i] = m.A.Y;
                PeerSeen[i] = _p.Clock;
            }

            public bool IsPeerAlive(long address)
            {
                return IndexOfPeer(address) >= 0;
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
                if (!(msg.Data is MyTuple<int, int, Vector3D, Vector3D, Vector3D, Vector3D>)) return;

                var d = (MyTuple<int, int, Vector3D, Vector3D, Vector3D, Vector3D>)msg.Data;
                _inbox.Source = msg.Source;
                _inbox.Op = d.Item1;
                _inbox.Arg = d.Item2;
                _inbox.A = d.Item3;
                _inbox.B = d.Item4;
                _inbox.C = d.Item5;
                _inbox.D = d.Item6;

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

            // ---------------- UI ----------------

            public void AppendStatus(StringBuilder sb)
            {
                if (PeerCount == 0) return;
                sb.Append("Peers: ");
                Fmt.Int(sb, PeerCount).Append('\n');
                for (int i = 0; i < PeerCount && i < 8; i++)
                {
                    sb.Append(" #");
                    Fmt.Int(sb, PeerAddress[i] % 10000).Append(' ');
                    int s = PeerState[i];
                    sb.Append(s >= 0 && s < BrainFSM.StateNames.Length ? BrainFSM.StateNames[s] : "?");
                    sb.Append(" C:");
                    Fmt.Pct(sb, PeerCargo[i]).Append(" B:");
                    Fmt.Pct(sb, PeerBattery[i]).Append('\n');
                }
            }

            int IndexOfPeer(long address)
            {
                for (int i = 0; i < PeerCount; i++)
                    if (PeerAddress[i] == address) return i;
                return -1;
            }

            int OldestPeer()
            {
                int oldest = 0;
                for (int i = 1; i < PeerCount; i++)
                    if (PeerSeen[i] < PeerSeen[oldest]) oldest = i;
                return oldest;
            }
        }
    }
}
