using System;
using System.Text;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// The [FM Menu] terminal: a navigable menu driven by four toolbar arguments
        /// (up, down, select/apply, back), drawn as a 40-column ASCII control panel.
        /// Actions reuse the text commands, so the menu does nothing a command cannot.
        /// Rendering appends literals, cached strings and Fmt numbers only (zero-GC).
        /// </summary>
        public partial class BrainFSM
        {
            public const int MenuWidth = 40, MenuLines = 22;
            const int MenuRows = 9, NoteWidth = 36;
            static readonly string[] RoleTags = { "MINER", "HAULER", "CARRIER", "MOTHERSHIP" };
            static readonly string[] MenuMain = { "OPERATIONS", "SITE & SHAFTS", "SETTINGS", "DIAGNOSTICS" },
                OpsDrone = { "START CYCLE", "RETURN TO BASE", "HALT IN PLACE", "SKIP SHAFT" },
                OpsDroneCmd = { "start", "return", "stop", "skip" },
                OpsBase = { "LAUNCH FLEET", "RECALL FLEET", "HALT FLEET", "RESET PADS" },
                OpsBaseCmd = { "launch", "recall", "halt", "reset" },
                SiteDrone = { "SITE NUMBER", "JOIN SITE", "SET SITE HERE", "GO TO SHAFT", "RESET SITE" },
                SiteBase = { "SITE NUMBER", "ASSIGN SITE" },
                Diag = { "RUN SELF-TEST", "DETAIL PAGE", "RESET STATS", "HOLD TEST", "TURN YAW", "TURN PITCH", "TURN ROLL" },
                DiagCmd = { "selftest", "status", "resetstats", "hold", "turn", "turn up", "turn roll" };

            int _mPage, _mIdx, _mTop, _mSite = -1, _mShaft;
            bool _mEdit;
            double _mOld;
            readonly StringBuilder _mVal = new StringBuilder(32);

            string[] MenuItems(out int n)
            {
                bool b = _cfg.IsBase;
                var a = _mPage == 1 ? (b ? OpsBase : OpsDrone) : _mPage == 2 ? (b ? SiteBase : SiteDrone)
                    : _mPage == 3 ? Config.Keys : _mPage == 4 ? Diag : MenuMain;
                n = _mPage == 4 && b ? 3 : a.Length;
                return a;
            }

            /// <summary>up / down / select (apply) / back. False for any other argument.</summary>
            bool MenuCommand(string arg)
            {
                int dir = Is(arg, "up") ? -1 : Is(arg, "down") ? 1 : 0;
                bool sel = Is(arg, "select") || Is(arg, "apply"), back = Is(arg, "back");
                if (dir == 0 && !sel && !back) return false;
                int n, i = _mIdx;
                MenuItems(out n);
                if (dir != 0)
                {
                    if (_mEdit) MenuAdjust(-dir); // up raises the value
                    else _mIdx = (i + dir + n) % n;
                }
                else if (back)
                {
                    if (_mEdit) MenuSet(_mOld);
                    else if (_mPage > 0) { _mIdx = _mPage - 1; _mPage = 0; }
                    _mEdit = false;
                }
                else if (_mEdit)
                {
                    _mEdit = false;
                    if (_mPage == 3) { _cfg.Dirty = true; _note = NoteSet; }
                    else if (i == 3) Goto(_mShaft);
                }
                else if (_mPage == 0) { _mPage = i + 1; _mIdx = 0; }
                else if (_mPage == 1) HandleCommand(_cfg.IsBase ? OpsBaseCmd[i] : OpsDroneCmd[i]);
                else if (_mPage == 4) HandleCommand(DiagCmd[i]);
                else if (_mPage == 3 && Config.IsBool(i)) { _cfg.Put(i, 1 - _cfg.Get(i)); _cfg.Dirty = true; _note = NoteSet; }
                else if (_mPage == 3 || i == 0 || i == 3) { _mEdit = true; _mOld = MenuGet(); }
                else if (_cfg.IsBase) AssignSite(_mSite);
                else if (i == 1) AskForSite(_mSite);
                else if (i == 2) SetSite(_mSite);
                else ResetSite();
                return true;
            }

            double MenuGet() { return _mPage == 3 ? _cfg.Get(_mIdx) : _mIdx == 0 ? _mSite : _mShaft; }

            void MenuSet(double v)
            {
                if (_mPage == 3) _cfg.Put(_mIdx, v);
                else if (_mIdx == 0) _mSite = (int)v;
                else _mShaft = (int)v;
            }

            /// <summary>Sites wrap -1 (own) .. 7, shafts 0 .. limit-1; settings step by a tenth of their magnitude.</summary>
            void MenuAdjust(int dir)
            {
                double v = MenuGet();
                int n = Math.Max(1, Site.Limit);
                if (_mPage != 3) MenuSet(_mIdx == 0 ? (v + dir + 10) % 9 - 1 : (v + dir + n) % n);
                else
                {
                    double step = v == 0 ? 1 : Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(v))) - 1);
                    MenuSet(Math.Max(0, Math.Round(v + dir * step, 6)));
                }
            }

            /// <summary>The whole panel, MenuLines rows of MenuWidth characters.</summary>
            public void AppendMenu(StringBuilder sb)
            {
                int n, s = sb.Length;
                var items = MenuItems(out n);
                if (_mIdx >= n) _mIdx = n - 1;
                if (_mIdx < _mTop) _mTop = _mIdx;
                if (_mIdx >= _mTop + MenuRows) _mTop = _mIdx - MenuRows + 1;

                // Title plate with a heartbeat that turns once per screen update.
                sb.Append("+==[ FLEETMINER CORE ]");
                sb.Append('=', s + MenuWidth - 6 - sb.Length).Append('[').Append("|/-\\"[(int)(_p.Clock / 1.6) & 3]).Append("]==+\n");
                s = Row(sb);
                sb.Append("UNIT  ").Append(_cfg.Name);
                Right(sb, s, RoleTags[(int)_cfg.Role]);
                End(sb, s);
                Rule(sb, '-');

                s = Row(sb);
                sb.Append("STATE  ").Append(StateLabels[(int)State]);
                Right(sb, s, IsCarrier ? (MotherAddr == 0 ? "HQ --" : _comms.Reachable(MotherAddr) ? "HQ OK" : "HQ LOST")
                    : _cfg.IsBase ? "" : LinkLost ? "LINK LOST" : _carrierAddr == 0 ? "LINK --" : "LINK OK");
                End(sb, s);
                if (_cfg.IsBase)
                {
                    int used = 0, pads = SlotCount;
                    for (int i = 0; i < pads; i++) if (SlotState(i) != 0) used++;
                    s = Gauge(sb, "PADS   ", pads > 0 ? (double)used / pads : 0);
                    Fmt.Int(_mVal, used).Append('/');
                    Fmt.Int(_mVal, pads);
                    Val(sb, s, ' ');
                    End(sb, s);
                    double total = 0;
                    for (int i = 0; i < Ore.Count; i++) total += Delivered[i];
                    s = Row(sb);
                    sb.Append("QUEUE  ");
                    Fmt.Int(sb, DockQueueCount + LaunchQueueCount);
                    _mVal.Clear();
                    Fmt.Mass(_mVal.Append("DELIVERED "), total);
                    Val(sb, s, ' ');
                    End(sb, s);
                }
                else
                {
                    s = Gauge(sb, "CARGO  ", _grid.CargoFill);
                    Fmt.Pct(_mVal, _grid.CargoFill);
                    Val(sb, s, ' ');
                    End(sb, s);
                    double power = _grid.HasHydrogen ? Math.Min(_grid.BatteryCharge, _grid.HydrogenFill) : _grid.BatteryCharge;
                    s = Gauge(sb, "POWER  ", power);
                    Fmt.Pct(_mVal, power);
                    Val(sb, s, ' ');
                    End(sb, s);
                }

                // Section plate: title and position.
                s = sb.Length;
                sb.Append("+--[ ").Append(_mPage == 0 ? "MAIN MENU" : MenuMain[_mPage - 1]).Append(" ]");
                _mVal.Clear();
                Fmt.Int(_mVal.Append("[ "), _mIdx + 1).Append('/');
                Fmt.Int(_mVal, n).Append(" ]");
                sb.Append('-', Math.Max(1, s + MenuWidth - 2 - _mVal.Length - sb.Length));
                Copy(sb, _mVal).Append("-+\n");

                for (int r = 0; r < MenuRows; r++)
                {
                    int i = _mTop + r;
                    s = Row(sb);
                    if (i < n)
                    {
                        bool cur = i == _mIdx, edit = cur && _mEdit;
                        sb.Append(edit ? "=> " : cur ? ">> " : "   ").Append(items[i]);
                        _mVal.Clear();
                        if (edit) _mVal.Append("< ");
                        MenuValue(i);
                        if (edit) _mVal.Append(" >");
                        else if (cur && _mVal.Length == 0) _mVal.Append('<');
                        Val(sb, s, cur ? '.' : ' ');
                    }
                    End(sb, s);
                }
                Rule(sb, '-');

                // Last result, upper-cased in place over two rows.
                string note = _note ?? (_mEdit ? "Editing: SELECT keeps, BACK undoes" : "Ready");
                for (int k = 0; k < 2; k++)
                {
                    s = Row(sb);
                    sb.Append(k == 0 ? "> " : "  ");
                    for (int c = k * NoteWidth; c < note.Length && c < (k + 1) * NoteWidth; c++)
                        sb.Append(char.ToUpperInvariant(note[c]));
                    End(sb, s);
                }
                Rule(sb, '-');
                s = Row(sb);
                sb.Append(_mEdit ? "UP/DN ADJUST  SELECT OK  BACK UNDO" : "UP/DN MOVE  SELECT RUN  BACK EXIT");
                End(sb, s);

                // Hazard stripe.
                sb.Append('+');
                for (int k = 0; k < MenuWidth - 2; k++) sb.Append((k & 2) == 0 ? '/' : ' ');
                sb.Append("+\n");
            }

            void MenuValue(int i)
            {
                if (_mPage == 0) _mVal.Append("[>]");
                else if (_mPage == 2 && i == 0)
                {
                    if (_mSite < 0) _mVal.Append("OWN"); else Fmt.Int(_mVal, _mSite);
                }
                else if (_mPage == 2 && i == 3) Fmt.Int(_mVal, _mShaft);
                else if (_mPage == 3)
                {
                    double v = _cfg.Get(i);
                    if (Config.IsBool(i)) _mVal.Append(v != 0 ? "[ON]" : "[OFF]");
                    else Fmt.Fixed(_mVal, v, Math.Abs(v - Math.Round(v)) < 1e-6 ? 0 : v < 0.1 ? 3 : 2);
                }
            }

            static int Row(StringBuilder sb)
            {
                int s = sb.Length;
                sb.Append("| ");
                return s;
            }

            /// <summary>Pads or cuts the row begun at s to the panel width and closes it.</summary>
            static void End(StringBuilder sb, int s)
            {
                int e = s + MenuWidth - 2;
                if (sb.Length > e) sb.Length = e;
                else sb.Append(' ', e - sb.Length);
                sb.Append(" |\n");
            }

            static void Rule(StringBuilder sb, char c)
            {
                sb.Append('+').Append(c, MenuWidth - 2).Append("+\n");
            }

            static void Right(StringBuilder sb, int s, string text)
            {
                sb.Append(' ', Math.Max(1, s + MenuWidth - 2 - text.Length - sb.Length)).Append(text);
            }

            /// <summary>Right-aligns _mVal on the row begun at s, leader-filled with c.</summary>
            void Val(StringBuilder sb, int s, char c)
            {
                int pad = s + MenuWidth - 2 - _mVal.Length - sb.Length;
                sb.Append(' ');
                if (pad > 2) sb.Append(c, pad - 2);
                if (pad > 1) sb.Append(' ');
                Copy(sb, _mVal);
            }

            static StringBuilder Copy(StringBuilder sb, StringBuilder from)
            {
                for (int k = 0; k < from.Length; k++) sb.Append(from[k]);
                return sb;
            }

            /// <summary>Starts a row "LABEL  [#####-----]" and clears _mVal for its readout.</summary>
            int Gauge(StringBuilder sb, string label, double ratio)
            {
                int s = Row(sb);
                int f = (int)Math.Round(Math.Max(0, Math.Min(1, ratio)) * 20);
                sb.Append(label).Append('[').Append('#', f).Append('-', 20 - f).Append(']');
                _mVal.Clear();
                return s;
            }
        }
    }
}
