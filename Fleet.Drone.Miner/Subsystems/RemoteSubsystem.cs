using System;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    // Drone side of the remote console link (spec §10.2): broadcasts status every Update100 and
    // executes commands / SET / GETCFG that arrive by unicast on this drone's channel.
    public partial class Program
    {
        public class RemoteSubsystem : ISubsystem
        {
            readonly Rig _r;
            readonly MinerSubsystem _miner;
            readonly UiSubsystem _ui;
            readonly IMyIntergridCommunicationSystem _igc;
            readonly StringBuilder _sb = new StringBuilder(160);
            string _channel, _statusTag, _cmdTag, _cfgTag, _ackTag;
            IMyBroadcastListener _bays;
            DroneStatus _status;

            public RemoteSubsystem(Rig r, MinerSubsystem miner, UiSubsystem ui, IMyIntergridCommunicationSystem igc)
            {
                _r = r; _miner = miner; _ui = ui; _igc = igc;
                Retag();
            }

            public string Name { get { return "Remote"; } }
            public int StorageVersion { get { return 1; } }
            public void Update1() { }
            public void HandleMessage(MyIGCMessage msg) { }
            public void Save(MyIni ini) { }
            public bool Load(MyIni ini, int savedVersion) { return true; }
            public void Status(StringBuilder sb) { }

            // Channel changes take effect immediately (called after every config reload).
            public void Retag()
            {
                if (_channel == _r.Settings.Channel) return;
                _channel = _r.Settings.Channel;
                _statusTag = FleetLink.Tag(_channel, FleetLink.Status);
                _cmdTag = FleetLink.Tag(_channel, FleetLink.Command);
                _cfgTag = FleetLink.Tag(_channel, FleetLink.Cfg);
                _ackTag = FleetLink.Tag(_channel, FleetLink.Ack);
                if (_bays != null) _igc.DisableBroadcastListener(_bays);
                _bays = _igc.RegisterBroadcastListener(FleetLink.Tag(_channel, Beacon.Kind));
                _bays.SetMessageCallback("");   // Main runs with UpdateType.IGC on arrival: beacon age ~1 tick
            }

            public void Update10() { Poll(); PollBays(); }

            // Carrier bay beacons (slice 2): only the home bay's is kept. Struct payload, no allocation.
            public void PollBays()
            {
                while (_bays.HasPendingMessage)
                {
                    var msg = _bays.AcceptMessage();
                    BayPose p;
                    if (Beacon.TryUnpack(msg.Data, out p)) _r.Home.Offer(ref p, _r.Bb.Time);
                }
            }

            public void Update100() { Broadcast(); }

            // Also called by Program while the kernel is SAFE, so a console can still see and RESET it.
            public void Poll()
            {
                var inbox = _igc.UnicastListener;
                while (inbox.HasPendingMessage)
                {
                    var msg = inbox.AcceptMessage();
                    if (msg.Tag != _cmdTag) continue;
                    var text = msg.Data as string;
                    if (string.IsNullOrWhiteSpace(text)) continue;
                    Execute(msg.Source, text.Trim());
                }
            }

            // Rare path (one operator action): allocation is fine.
            void Execute(long from, string text)
            {
                if (string.Equals(text, "GETCFG", StringComparison.OrdinalIgnoreCase))
                {
                    SendCfg(from);
                    return;
                }
                if (text.StartsWith("SET ", StringComparison.OrdinalIgnoreCase))
                {
                    int field; double value; string error;
                    if (FleetLink.TryParseSet(text, out field, out value, out error) && _ui.ApplySetting(field, value)) SendCfg(from);
                    else _igc.SendUnicastMessage(from, _ackTag, "ERR " + (error ?? "Custom Data is not valid INI"));
                    return;
                }
                // Report what actually happened: a refused command leaves a note, which goes back instead.
                var before = _r.LastNote;
                _r.RunCommand(text);
                bool noted = !ReferenceEquals(before, _r.LastNote);
                _igc.SendUnicastMessage(from, _ackTag, noted ? text + ": " + _r.LastNote : "OK " + text);
            }

            void SendCfg(long to) { _igc.SendUnicastMessage(to, _cfgTag, _r.Me.CustomData); }

            public void Broadcast()
            {
                var bb = _r.Bb;
                var fsm = _miner.Fsm;
                var safe = _r.Kernel != null && _r.Kernel.IsSafe;
                _status.Name = _r.Settings.Name;
                _status.State = (int)fsm.State;
                _status.Reason = (int)fsm.PendingReason;
                _status.Holes = _miner.HasJob ? _miner.HoleCount : 0;
                _status.Hole = Math.Min(_miner.HoleIndex + 1, _status.Holes);
                _status.Cargo = Pct(bb.CargoFill);
                _status.Battery = bb.HasBattery ? Pct(bb.BatteryFill) : -1;
                _status.Hydrogen = bb.HasHydrogen ? Pct(bb.HydrogenFill) : -1;
                _status.Lift100 = double.IsInfinity(bb.LiftMargin) ? -1 : (int)Math.Min(99999, Math.Round(bb.LiftMargin * 100));
                _status.Flags = (bb.Connected ? FleetLink.FlagConnected : 0) | (_miner.HasJob ? FleetLink.FlagHasJob : 0)
                    | (safe ? FleetLink.FlagSafe : 0) | (_r.Scan.Ready ? 0 : FleetLink.FlagNotReady)
                    | (fsm.State == MinerState.Recording ? FleetLink.FlagRecording : 0);
                _status.Note = safe ? _r.Kernel.SafeReason
                    : fsm.State == MinerState.Hold && fsm.Note != null ? fsm.Note
                    : !_r.Scan.Ready ? _r.Scan.Diagnostics[0] : "";
                _sb.Clear();
                FleetLink.Encode(_sb, ref _status);
                // The one bounded allocation of the loop (spec §10.2): one short string per ~1.7 s.
                _igc.SendBroadcastMessage(_statusTag, _sb.ToString());
            }

            static int Pct(double f) { return (int)Math.Round(Math.Max(0, Math.Min(1, f)) * 100); }
        }
    }
}
