using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    // Fleet console for the mothership / carrier (spec §10.4): lists drones heard on the channel,
    // shows a selected drone's live status, edits its settings and commands it. Commanded, not piloted.
    public partial class Program : MyGridProgram
    {
        const int MenuLines = 12;
        string _channel = "FM", _tag = "[FM]", _configText;
        string _statusTag, _cmdTag, _cfgTag, _ackTag;
        IMyBroadcastListener _status;
        readonly Roster _roster = new Roster();
        readonly Displays _displays = new Displays();
        readonly List<IMyTerminalBlock> _blocks = new List<IMyTerminalBlock>();
        readonly List<string> _targetNames = new List<string>(), _targetGps = new List<string>();
        readonly List<string> _scratch = new List<string>();
        readonly List<MyIniKey> _keys = new List<MyIniKey>();
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sb = new StringBuilder(2048);
        readonly Menu _menu;
        readonly MenuPage _fleet, _drone, _targets;
        long _selAddress;
        RosterEntry _sel;
        double _time;
        string _ack = "";
        double _ackTime = -100;

        public Program()
        {
            _fleet = new MenuPage("Fleet") { List = _roster };
            _drone = new MenuPage("drone");
            _drone.Parent = _fleet;
            var cmds = new MenuPage("Commands");
            cmds.AddCommand("Start job", "START")
                .AddCommand("Continue", "CONT")
                .AddCommand("Return home", "HOME", "Abort and return home?")
                .AddCommand("Stop here", "STOP", "Stop and hold here?")
                .AddCommand("Next hole", "NEXT")
                .AddCommand("Previous hole", "PREV")
                .AddCommand("Simulate full", "FULL")
                .AddCommand("Reset (clear SAFE)", "RESET", "Reset to Idle and clear SAFE?")
                .AddCommand("Rescan blocks", "REBOOT");
            _targets = new MenuPage("Send to target") { List = new TargetList(this) };
            _drone.AddPage("Commands", cmds).AddPage("Send to target", _targets).AddSettingsPages();
            _menu = new Menu(_fleet, null);
            LoadConfig();
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
        }

        class TargetList : IMenuList
        {
            readonly Program _p;
            public TargetList(Program p) { _p = p; }
            public int Count { get { return _p._targetNames.Count; } }
            public void AppendEntry(StringBuilder sb, int i) { sb.Append(_p._targetNames[i]); }
        }

        // Console Custom Data: [Console] Channel, Tag; [Targets] name=GPS:... (one per line).
        void LoadConfig()
        {
            MyIniParseResult res;
            _ini.Clear();
            if (!_ini.TryParse(Me.CustomData, out res)) { Ack("Custom Data is not valid INI"); _configText = Me.CustomData; return; }
            var ch = _ini.Get("Console", "Channel").ToString(_channel).Trim();
            _tag = _ini.Get("Console", "Tag").ToString(_tag).Trim();
            if (ch.Length == 0 || ch.IndexOf('/') >= 0) ch = "FM";
            _targetNames.Clear(); _targetGps.Clear();
            _ini.GetKeys("Targets", _keys);
            foreach (var k in _keys) { _targetNames.Add(k.Name); _targetGps.Add(_ini.Get(k).ToString("")); }
            if (!_ini.ContainsSection("Console") || !_ini.ContainsKey("Console", "Channel"))
            {
                _ini.Set("Console", "Channel", ch);
                _ini.Set("Console", "Tag", _tag);
                if (!_ini.ContainsSection("Targets"))
                {
                    _ini.AddSection("Targets");
                    _ini.SetSectionComment("Targets", " name=GPS:Name:X:Y:Z: (paste from the GPS screen)");
                }
                Me.CustomData = _ini.ToString();
            }
            _configText = Me.CustomData;
            if (ch != _channel || _status == null) Retag(ch);
            GridTerminalSystem.GetBlocksOfType(_blocks, b => b.CubeGrid == Me.CubeGrid);
            _displays.Refresh(_blocks, _tag);
            Me.GetSurface(0).ContentType = ContentType.TEXT_AND_IMAGE;
        }

        void Retag(string channel)
        {
            if (_status != null) IGC.DisableBroadcastListener(_status);
            _channel = channel;
            _statusTag = FleetLink.Tag(channel, FleetLink.Status);
            _cmdTag = FleetLink.Tag(channel, FleetLink.Command);
            _cfgTag = FleetLink.Tag(channel, FleetLink.Cfg);
            _ackTag = FleetLink.Tag(channel, FleetLink.Ack);
            _status = IGC.RegisterBroadcastListener(_statusTag);
        }

        public void Main(string argument, UpdateType src)
        {
            _time += Runtime.TimeSinceLastRun.TotalSeconds;
            _roster.Now = _time;
            if ((src & (UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script | UpdateType.Mod)) != 0
                && !string.IsNullOrWhiteSpace(argument))
                Run(argument.Trim());
            if ((src & (UpdateType.Update10 | UpdateType.IGC)) != 0) Receive();
            if ((src & UpdateType.Update100) != 0)
            {
                if (Me.CustomData != _configText) LoadConfig();
                _roster.Expire(_time);
                Render();
            }
        }

        void Receive()
        {
            while (_status.HasPendingMessage)
            {
                var m = _status.AcceptMessage();
                var line = m.Data as string;
                if (line != null) _roster.Update(m.Source, line, _time);
            }
            var inbox = IGC.UnicastListener;
            while (inbox.HasPendingMessage)
            {
                var m = inbox.AcceptMessage();
                var text = m.Data as string;
                if (text == null) continue;
                int i = _roster.IndexOf(m.Source);
                var e = i >= 0 ? _roster[i] : null;
                if (m.Tag == _cfgTag && e != null) { e.ApplyConfig(text, _scratch); if (e == _sel) Render(); }
                else if (m.Tag == _ackTag) Ack((e != null ? e.Status.Name + ": " : "") + text);
            }
        }

        // Run arguments: UP DOWN APPLY BACK · SEND <drone> <command...> · ALL <command...> · REFRESH
        void Run(string text)
        {
            string head = FirstWord(text), rest = text.Substring(head.Length).Trim();
            if (Is(head, "SEND"))
            {
                string name = FirstWord(rest);
                var e = _roster.Find(name);
                if (e == null) { Ack("no drone named " + name); return; }
                Send(e, rest.Substring(name.Length).Trim());
                return;
            }
            if (Is(head, "ALL"))
            {
                for (int i = 0; i < _roster.Count; i++) Send(_roster[i], rest);
                Ack("sent to " + _roster.Count + " drone(s): " + rest);
                return;
            }
            if (Is(head, "REFRESH")) { if (_sel != null) Send(_sel, "GETCFG"); return; }
            string ignored;
            var cmd = CommandParser.Parse(text, out ignored);
            if (cmd == Cmd.Up || cmd == Cmd.Down || cmd == Cmd.Apply || cmd == Cmd.Back) HandleMenu(cmd);
            else Ack("unknown: " + text + "  (UP DOWN APPLY BACK, SEND <drone> <cmd>, ALL <cmd>)");
        }

        void HandleMenu(Cmd c)
        {
            if (_sel != null && _roster.IndexOf(_selAddress) < 0) LoseSelection();
            var a = _menu.Handle(c);
            switch (a.Kind)
            {
                case MenuActionKind.Select:
                    if (_menu.Current == _fleet) Select(_roster[a.Index]);
                    else if (_menu.Current == _targets && _sel != null) Send(_sel, "GOTO " + _targetGps[a.Index]);
                    break;
                case MenuActionKind.Command:
                    if (_sel != null) Send(_sel, a.Command);
                    break;
                case MenuActionKind.SetValue:
                    if (_sel != null) { Send(_sel, FleetLink.SetCommand(a.Field, a.Value)); _sel.MarkSent(a.Field, a.Value); }
                    break;
            }
            Render();
        }

        void Select(RosterEntry e)
        {
            _sel = e;
            _selAddress = e.Address;
            _drone.Title = e.Status.Name;
            _menu.Values = e;
            _menu.Open(_drone);
            Send(e, "GETCFG");
        }

        void LoseSelection()
        {
            Ack("lost contact with " + _drone.Title);
            _sel = null;
            _menu.Values = null;
            _menu.Open(_fleet);
        }

        void Send(RosterEntry e, string command)
        {
            if (command.Length == 0) return;
            IGC.SendUnicastMessage(e.Address, _cmdTag, command);
            if (!Is(command, "GETCFG")) Ack(e.Status.Name + " <- " + command);
        }

        void Ack(string text) { _ack = text; _ackTime = _time; }

        void Render()
        {
            if (_sel != null && _roster.IndexOf(_selAddress) < 0) LoseSelection();
            var sb = _sb;
            sb.Clear();
            sb.Append("FLEET CONSOLE  ch ").Append(_channel).Append("  drones ");
            SbFormat.AppendInt(sb, _roster.Count).Append('\n');
            if (_sel != null) AppendDrone(sb, _sel);
            if (_time - _ackTime < 20 && _ack.Length > 0) sb.Append(">> ").Append(_ack).Append('\n');
            sb.Append('\n');
            _menu.Render(sb, MenuLines);
            _displays.Write(sb);
            Me.GetSurface(0).WriteText(sb);
        }

        void AppendDrone(StringBuilder sb, RosterEntry e)
        {
            var s = e.Status;
            sb.Append(s.Name).Append("  ").Append(s.State >= 0 && s.State < Names.State.Length ? Names.State[s.State] : "?");
            if (s.Reason > 0 && s.Reason < Names.Reason.Length) sb.Append(" (").Append(Names.Reason[s.Reason]).Append(')');
            if ((s.Flags & FleetLink.FlagConnected) != 0) sb.Append("  docked");
            if (_roster.IsStale(e, _time)) sb.Append("  STALE");
            sb.Append('\n');
            if (s.Holes > 0) { SbFormat.AppendInt(sb.Append("hole "), s.Hole).Append('/'); SbFormat.AppendInt(sb, s.Holes).Append("  "); }
            SbFormat.AppendInt(sb.Append("cargo "), s.Cargo).Append('%');
            if (s.Battery >= 0) SbFormat.AppendInt(sb.Append("  bat "), s.Battery).Append('%');
            if (s.Hydrogen >= 0) SbFormat.AppendInt(sb.Append("  H2 "), s.Hydrogen).Append('%');
            if (s.Lift100 >= 0) SbFormat.AppendFixed(sb.Append("  lift "), s.Lift100 / 100.0, 2);
            sb.Append('\n');
            bool safe = (s.Flags & FleetLink.FlagSafe) != 0;
            if (safe || s.Note.Length > 0) sb.Append(safe ? "SAFE: " : "").Append(s.Note).Append('\n');
            if (!e.HasConfig) sb.Append("(waiting for settings... REFRESH)\n");
        }

        static string FirstWord(string t)
        {
            int sp = t.IndexOf(' ');
            return sp < 0 ? t : t.Substring(0, sp);
        }

        static bool Is(string a, string b) { return string.Equals(a, b, StringComparison.OrdinalIgnoreCase); }
    }
}
