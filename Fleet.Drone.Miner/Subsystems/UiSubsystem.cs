using System;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        // Drone LCD (C30 §6) with the enhanced menu as the local fallback for the remote console
        // (spec §10.3), plus Custom Data hot-reload and the one place settings are written.
        public class UiSubsystem : ISubsystem
        {
            public const int MenuLines = 9, EventLines = 4;
            readonly Rig _r;
            readonly MinerSubsystem _miner;
            readonly StringBuilder _sb = new StringBuilder(1024);
            readonly StringBuilder _gyroReport = new StringBuilder(160);
            readonly MyIni _ini = new MyIni();
            public readonly Menu Menu;
            readonly MenuPage _root;
            string _configText;
            public Action OnConfigReloaded;

            public UiSubsystem(Rig r, MinerSubsystem miner)
            {
                _r = r;
                _miner = miner;
                _root = new MenuPage(r.Settings.Name);
                var job = new MenuPage("Job");
                job.AddCommand("Start job", "START")
                   .AddCommand("Continue", "CONT")
                   .AddCommand("Return home", "HOME", "Abort and return home?")
                   .AddCommand("Stop here", "STOP", "Stop and hold here?")
                   .AddCommand("Set job here", "SETJOB", "Replace the job with one here?")
                   .AddCommand("Next hole", "NEXT")
                   .AddCommand("Previous hole", "PREV")
                   .AddCommand("Simulate full", "FULL");
                var setup = new MenuPage("Setup");
                setup.AddCommand("Record dock path", "RECORD DOCK")
                     .AddCommand("Record job route", "RECORD JOB")
                     .AddCommand("Stop recording", "STOPREC")
                     .AddCommand("Gyro test", "GYROTEST")
                     .AddCommand("Rescan blocks", "REBOOT")
                     .AddCommand("Reset (clear SAFE)", "RESET", "Reset to Idle and clear SAFE?");
                _root.AddPage("Job control", job).AddPage("Setup", setup).AddSettingsPages();
                Menu = new Menu(_root, new SettingsValues(r.Settings));
                _configText = r.Me.CustomData;
                var surface = r.Me.GetSurface(0);
                surface.ContentType = ContentType.TEXT_AND_IMAGE;
            }

            public string Name { get { return "Ui"; } }
            public int StorageVersion { get { return 1; } }
            public void Update1() { }
            public void Update10()
            {
                if (_r.Ship.GyroTestRunning) _r.Ship.UpdateGyroTest(_r.Bb.Time, _gyroReport);
            }
            public void HandleMessage(MyIGCMessage msg) { }
            public void Save(MyIni ini) { }
            public bool Load(MyIni ini, int savedVersion) { return true; }
            public void Status(StringBuilder sb) { }

            public void Update100()
            {
                if (_r.Me.CustomData != _configText) ReloadConfig("config reloaded");
                Render();
            }

            // UP / DOWN / APPLY / BACK from the terminal, a button panel or the console.
            public void HandleMenu(Cmd c)
            {
                var a = Menu.Handle(c);
                if (a.Kind == MenuActionKind.SetValue) ApplySetting(a.Field, a.Value);
                else if (a.Kind == MenuActionKind.Command) _r.RunCommand(a.Command);
                Render();
            }

            // Writes one setting into Custom Data (the source of truth) and reloads. Rare path.
            public bool ApplySetting(int field, double value)
            {
                var f = SettingsSchema.Fields[field];
                MyIniParseResult res;
                _ini.Clear();
                if (!_ini.TryParse(_r.Me.CustomData, out res)) { _r.Note("Custom Data is not valid INI"); return false; }
                _ini.Set(f.Section, f.Key, SettingsSchema.ToIniText(field, value));
                _r.Me.CustomData = _ini.ToString();
                ReloadConfig(null);
                _r.Note(f.Label + " set");
                return true;
            }

            public void ReloadConfig(string note)
            {
                var s = _r.Settings;
                string oldTag = s.Tag;
                _r.ConfigWarnings.Clear();
                ConfigLoader.Load(_r.Me.CustomData, s, _r.ConfigWarnings);
                var full = ConfigLoader.Write(_r.Me.CustomData, s);
                if (full != _r.Me.CustomData && _r.ConfigWarnings.Count == 0) _r.Me.CustomData = full;
                _configText = _r.Me.CustomData;
                _root.Title = s.Name;
                if (s.Tag != oldTag) _r.Displays.Refresh(_r.Scan.All, s.Tag);
                if (note != null) _r.Note(note);
                if (OnConfigReloaded != null) OnConfigReloaded();
            }

            public void Render()
            {
                var sb = _sb;
                var bb = _r.Bb;
                var s = _r.Settings;
                var fsm = _miner.Fsm;
                sb.Clear();
                sb.Append(s.Name).Append("  ").Append(Names.State[(int)fsm.State]);
                if (fsm.PendingReason != ReturnReason.None) sb.Append(" (").Append(Names.Reason[(int)fsm.PendingReason]).Append(')');
                sb.Append('\n');

                if (_r.Kernel != null && _r.Kernel.IsSafe) sb.Append("SAFE: ").Append(_r.Kernel.SafeReason).Append("  -> RESET\n");
                else if (fsm.State == MinerState.Hold && fsm.Note != null) sb.Append("hold: ").Append(fsm.Note).Append('\n');
                else if (_miner.HasJob)
                {
                    SbFormat.AppendInt(sb.Append("hole "), Math.Min(_miner.HoleIndex + 1, _miner.HoleCount)).Append('/');
                    SbFormat.AppendInt(sb, _miner.HoleCount);
                    if (fsm.State == MinerState.Drill || fsm.State == MinerState.Retract)
                    {
                        SbFormat.AppendFixed(sb.Append("  depth "), Math.Max(0, _miner.Depth), 1).Append('/');
                        SbFormat.AppendFixed(sb, s.Depth - s.StartDepth, 0).Append(" m");
                    }
                    sb.Append('\n');
                }
                else sb.Append("no job: SETJOB or GOTO\n");

                SbFormat.AppendPercent(sb.Append("cargo "), bb.CargoFill);
                sb.Append("  lift ");
                if (double.IsInfinity(bb.LiftMargin)) sb.Append("--"); else SbFormat.AppendFixed(sb, bb.LiftMargin, 2);
                if (bb.HasBattery) SbFormat.AppendPercent(sb.Append("  bat "), bb.BatteryFill);
                if (bb.HasHydrogen) SbFormat.AppendPercent(sb.Append("  H2 "), bb.HydrogenFill);
                if (bb.HasReactor) SbFormat.AppendFixed(sb.Append("  U "), bb.UraniumKg, 1).Append("kg");
                sb.Append('\n');

                if (_r.Profiler != null && _r.Runtime != null)
                {
                    SbFormat.AppendInt(sb.Append("instr avg "), (long)_r.Profiler.Average);
                    SbFormat.AppendInt(sb.Append(" peak "), _r.Profiler.Peak);
                    SbFormat.AppendPercent(sb.Append(" ("), (double)_r.Profiler.Peak / Math.Max(1, _r.Runtime.MaxInstructionCount)).Append(")\n");
                }

                for (int i = 0; i < _r.Scan.Diagnostics.Count; i++) sb.Append("!! ").Append(_r.Scan.Diagnostics[i]).Append('\n');
                for (int i = 0; i < _r.ConfigWarnings.Count; i++) sb.Append("!! ").Append(_r.ConfigWarnings[i]).Append('\n');
                if (_gyroReport.Length > 0) sb.Append(_gyroReport);
                if (_r.LastNote.Length > 0 && bb.Time - _r.LastNoteTime < 30) sb.Append(">> ").Append(_r.LastNote).Append('\n');

                sb.Append('\n');
                Menu.Render(sb, MenuLines);
                sb.Append('\n');
                _r.Events.Render(sb, EventLines);

                _r.Displays.Write(sb);
                _r.Me.GetSurface(0).WriteText(sb);
            }
        }
    }
}
