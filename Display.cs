using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Every screen, every Update100. Text pages (status, detail, log, stats) are built
        /// into reused StringBuilders; sprite pages (gauges, site map, flight-control board)
        /// only use literals, callsigns and Fmt.Str's cached numbers, so nothing allocates.
        /// Layout is done in a 512 x 512 virtual square, centred and scaled per screen.
        /// </summary>
        public class Display : ISubsystem
        {
            public const int ModeText = 0, ModeGauges = 1, ModeMap = 2, ModeBoard = 3, ModeLog = 4, ModeStats = 5;
            /// <summary>Name tags per mode (index = mode). "[FM Map" takes an optional site number: "[FM Map 2]".</summary>
            public static readonly string[] Tags = { "[FM LCD]", "[FM Gauges]", "[FM Map", "[FM Board]", "[FM Log]", "[FM Stats]" };
            public static bool IsSpriteMode(int mode) { return mode >= ModeGauges && mode <= ModeBoard; }

            const string Font = "White", Square = "SquareSimple";
            const int LogLines = 14, BoardRows = 8;
            static readonly Color Ink = new Color(220, 230, 240), Dim = new Color(110, 120, 130);
            static readonly Color Track = new Color(35, 40, 48), Accent = new Color(0, 180, 255);
            static readonly Color Good = new Color(0, 200, 60), Warn = new Color(255, 190, 0), Bad = new Color(230, 40, 30);
            static readonly Color Gold = new Color(255, 215, 0), Partial = new Color(255, 140, 0);
            static readonly Color Claimed = new Color(40, 120, 255), BarrenC = new Color(110, 80, 50);
            static readonly Color EmptyC = new Color(20, 20, 24), BlockedC = new Color(150, 20, 20);
            static readonly Color DistressBg = new Color(90, 0, 0);

            readonly Program _p;
            readonly GridManager _grid;
            readonly CommsOfficer _comms;
            readonly HelmController _helm;
            readonly BrainFSM _brain;
            readonly StringBuilder _text = new StringBuilder(2048);
            readonly StringBuilder _detail = new StringBuilder(1024);
            readonly StringBuilder _log = new StringBuilder(2048);
            readonly StringBuilder _stats = new StringBuilder(1024);

            MySpriteDrawFrame _f;
            Vector2 _o;
            float _s;

            public Display(Program p, GridManager grid, CommsOfficer comms, HelmController helm, BrainFSM brain)
            {
                _p = p;
                _grid = grid;
                _comms = comms;
                _helm = helm;
                _brain = brain;
            }

            public void Initialize() { }
            public void Update10() { }
            public void HandleMessage(FleetMessage message) { }

            public void Update100()
            {
                _text.Clear();
                _detail.Clear();
                _log.Clear();
                _stats.Clear();
                bool detail = _brain.DetailUntil > _p.Clock;

                for (int i = 0; i < _grid.Screens.Count; i++)
                {
                    var s = _grid.Screens[i];
                    switch (_grid.ScreenMode[i])
                    {
                        case ModeText:
                            // The PB's own screen (always first) doubles as the detail page.
                            if (i == 0 && detail) s.WriteText(Detail());
                            else s.WriteText(Text());
                            break;
                        case ModeLog: s.WriteText(Log()); break;
                        case ModeStats: s.WriteText(Stats()); break;
                        case ModeGauges: Draw(s, ModeGauges, -1); break;
                        case ModeMap: Draw(s, ModeMap, _grid.ScreenArg[i]); break;
                        case ModeBoard: Draw(s, ModeBoard, -1); break;
                    }
                }
            }

            // ---------------- Text pages (built at most once per update) ----------------

            StringBuilder Text()
            {
                if (_text.Length > 0) return _text;
                _text.Append("== FleetMiner Core ==\n");
                _brain.AppendStatus(_text);
                _grid.AppendStatus(_text);
                _comms.AppendStatus(_text);
                Cpu(_text);
                return _text;
            }

            StringBuilder Detail()
            {
                if (_detail.Length > 0) return _detail;
                _brain.AppendDetail(_detail);
                Cpu(_detail);
                return _detail;
            }

            void Cpu(StringBuilder sb)
            {
                sb.Append("CPU: ");
                Fmt.Fixed(sb, _p.Runtime.LastRunTimeMs, 3).Append(" ms / ");
                Fmt.Int(sb, _p.Runtime.CurrentInstructionCount).Append(" instr\n");
            }

            StringBuilder Log()
            {
                if (_log.Length > 0) return _log;
                _log.Append("== COMMS LOG ==\n");
                _p.Log.Render(_log, LogLines);
                return _log;
            }

            StringBuilder Stats()
            {
                if (_stats.Length > 0) return _stats;
                var d = _brain.Delivered;
                _stats.Append("== PRODUCTION ==\n");
                double total = 0;
                for (int i = 0; i < Ore.Count; i++)
                {
                    if (d[i] < 1) continue;
                    total += d[i];
                    _stats.Append(Ore.Names[i]).Append(": ");
                    Fmt.Mass(_stats, d[i]).Append('\n');
                }
                _stats.Append("Total: ");
                Fmt.Mass(_stats, total).Append("\nRate: ");
                double hours = Math.Max(1.0 / 60, _brain.StatsSeconds / 3600);
                Fmt.Mass(_stats, total / hours).Append(" / h over ");
                Fmt.Clock(_stats, _brain.StatsSeconds).Append('\n');

                bool header = false;
                for (int i = 0; i < _comms.PeerCount; i++)
                {
                    if (_comms.PeerDelivered[i] < 1) continue;
                    if (!header) _stats.Append("-- By drone --\n");
                    header = true;
                    _stats.Append(_comms.PeerName[i]).Append(": ");
                    Fmt.Mass(_stats, _comms.PeerDelivered[i]).Append('\n');
                }
                return _stats;
            }

            // ---------------- Sprite pages ----------------

            void Draw(IMyTextSurface s, int mode, int arg)
            {
                Vector2 size = s.SurfaceSize;
                Vector2 offset = (s.TextureSize - size) / 2;
                _s = Math.Min(size.X, size.Y) / 512f;
                _o = offset + (size - new Vector2(512 * _s)) / 2;

                _f = s.DrawFrame();
                if (mode == ModeGauges) Gauges();
                else if (mode == ModeMap) Map(arg);
                else Board();
                _f.Dispose();
            }

            void Rect(float x, float y, float w, float h, Color c)
            {
                _f.Add(new MySprite(SpriteType.TEXTURE, Square,
                    _o + new Vector2(x + w / 2, y + h / 2) * _s, new Vector2(w, h) * _s, c));
            }

            void Text(string t, float x, float y, float size, Color c, TextAlignment align = TextAlignment.LEFT)
            {
                var sprite = MySprite.CreateText(t, Font, c, size * _s, align);
                sprite.Position = _o + new Vector2(x, y) * _s;
                _f.Add(sprite);
            }

            /// <summary>Labelled bar: fraction filled, optional marker (e.g. charge needed to get home).</summary>
            void Bar(float y, string label, double fraction, Color fill, double marker = -1, double number = -1)
            {
                fraction = Math.Max(0, Math.Min(1, fraction));
                Text(label, 16, y, 0.75f, Dim);
                Text(Fmt.Str(number >= 0 ? number : fraction * 100), 496, y, 0.75f, Ink, TextAlignment.RIGHT);
                Rect(16, y + 30, 480, 16, Track);
                Rect(16, y + 30, (float)(480 * fraction), 16, fill);
                if (marker >= 0 && marker <= 1) Rect(16 + (float)(480 * marker) - 1, y + 26, 3, 24, Ink);
            }

            void Header(string title, string label, Color labelColor)
            {
                Text(title, 16, 8, 1.1f, Ink);
                Text(label, 496, 14, 0.8f, labelColor, TextAlignment.RIGHT);
                Rect(16, 48, 480, 2, Accent);
            }

            /// <summary>Drone gauges: cargo, power vs what it takes to get home, hull, lift, shaft progress.</summary>
            void Gauges()
            {
                var cfg = _p.Cfg;
                Color stateColor = _brain.DistressReason != 0 ? Bad : Accent;
                Header(cfg.Name, BrainFSM.StateLabels[(int)_brain.State], stateColor);
                if (_brain.Note != null) Text(_brain.Note, 16, 56, 0.6f, _brain.DistressReason != 0 ? Bad : Warn);

                float y = 90;
                Bar(y, "CARGO %", _grid.CargoFill, _grid.CargoFill >= cfg.CargoFull ? Warn : Good, cfg.CargoFull);
                y += 62;
                Bar(y, "BATTERY %", _grid.BatteryCharge, _grid.BatteryCharge < _brain.BatteryNeeded() ? Bad : Good,
                    _brain.BatteryNeeded());
                y += 62;
                if (_grid.HasHydrogen)
                {
                    Bar(y, "HYDROGEN %", _grid.HydrogenFill, _grid.HydrogenFill < _brain.HydrogenNeeded() ? Bad : Good,
                        _brain.HydrogenNeeded());
                    y += 62;
                }
                Bar(y, "HULL %", _grid.Integrity, _grid.Integrity < 0.97 ? Warn : Good);
                y += 62;
                if (!double.IsInfinity(_helm.LiftRatio))
                {
                    // Number = % of the minimum lift; full bar = twice the minimum, marker = the minimum.
                    Bar(y, "LIFT % OF MIN", _helm.LiftRatio / (2 * cfg.MinLift), _helm.LiftRatio < cfg.MinLift ? Bad : Good,
                        0.5, _helm.LiftRatio / cfg.MinLift * 100);
                    y += 62;
                }
                if (_brain.State == FleetState.Working && _brain.FaceKnown)
                    Bar(y, "SHAFT %", _brain.CutDepth / Math.Max(1, cfg.MineDepth), Accent);
                else if (_brain.State == FleetState.Working)
                    Text("SEEKING ROCK FACE", 16, y, 0.75f, Warn);
            }

            /// <summary>Site map: one cell per shaft, coloured by result and yield.</summary>
            void Map(int arg)
            {
                bool carrier = _p.Cfg.Role == FleetRole.Carrier;
                int id = carrier ? (arg >= 0 ? arg : _brain.MapSite) : _brain.SiteId;
                SiteMap site = carrier ? _brain.Library(id) : _brain.Site;

                Text("SITE", 16, 8, 1.1f, Ink);
                if (id >= 0) Text(Fmt.Str(id), 110, 8, 1.1f, Ink);
                Rect(16, 48, 480, 2, Accent);
                if (site == null || !site.Defined)
                {
                    Text("NO SITE", 256, 230, 1.2f, Dim, TextAlignment.CENTER);
                    return;
                }

                int r = 0;
                for (int i = 0; i < site.Limit; i++)
                    r = Math.Max(r, Math.Max(Math.Abs(SiteMap.X[i]), Math.Abs(SiteMap.Y[i])));
                float cell = 380f / (2 * r + 1);
                float cx = 256, cy = 250;
                double mean = site.MeanValue();
                long me = _p.IGC.Me;

                for (int i = 0; i < site.Limit; i++)
                {
                    float x = cx + SiteMap.X[i] * cell - cell / 2;
                    float y = cy - SiteMap.Y[i] * cell - cell / 2;
                    if (!carrier && i == _brain.Shaft) Rect(x - 2, y - 2, cell + 4, cell + 4, Ink);
                    Rect(x + 1, y + 1, cell - 2, cell - 2, CellColor(site, i, mean, me));
                }

                Text("DONE", 16, 450, 0.7f, Dim);
                Text(Fmt.Str(site.Count(SiteMap.Done)), 100, 450, 0.7f, Ink);
                Text("ACTIVE", 170, 450, 0.7f, Dim);
                Text(Fmt.Str(site.Count(SiteMap.Claimed)), 280, 450, 0.7f, Ink);
                Text("BARREN", 340, 450, 0.7f, Dim);
                Text(Fmt.Str(site.Count(SiteMap.Barren)), 496, 450, 0.7f, Ink, TextAlignment.RIGHT);
                Text("gold rich | green done | orange partial | blue active | brown barren", 256, 484, 0.45f, Dim, TextAlignment.CENTER);
            }

            static Color CellColor(SiteMap site, int i, double mean, long me)
            {
                switch (site.Status[i])
                {
                    case SiteMap.Claimed: return site.Claimant[i] == me ? Accent : Claimed;
                    case SiteMap.Partial: return Partial;
                    case SiteMap.Done:
                        if (site.IsRich(i, mean)) return Gold;
                        // Brighter green for richer shafts.
                        double t = mean > 0 ? Math.Min(1, site.Value[i] / (2 * mean)) : 0.5;
                        return new Color(0, (int)(80 + 150 * t), (int)(30 + 40 * t));
                    case SiteMap.Barren: return BarrenC;
                    case SiteMap.Empty: return EmptyC;
                    case SiteMap.Blocked: return BlockedC;
                }
                return Track;
            }

            /// <summary>Carrier flight-control board: pads, launch countdown, one row per drone.</summary>
            void Board()
            {
                Header(_p.Cfg.Name, "FLIGHT CONTROL", Accent);

                int free = 0;
                for (int i = 0; i < _brain.SlotCount; i++)
                    if (_brain.SlotState(i) == 0) free++;
                Text("PADS FREE", 16, 58, 0.7f, Dim);
                Text(Fmt.Str(free), 160, 58, 0.7f, free > 0 ? Good : Warn);
                Text("HOLDING", 220, 58, 0.7f, Dim);
                Text(Fmt.Str(_brain.DockQueueCount), 340, 58, 0.7f, _brain.DockQueueCount > 0 ? Warn : Ink);
                if (_brain.LaunchingAt > _p.Clock)
                {
                    Text("LAUNCH", 16, 86, 0.7f, Warn);
                    Text(_comms.NameOf(_brain.Launching), 120, 86, 0.7f, Ink);
                    Text("T-", 430, 86, 0.7f, Warn);
                    Text(Fmt.Str(Math.Ceiling(_brain.LaunchingAt - _p.Clock)), 496, 86, 0.7f, Warn, TextAlignment.RIGHT);
                }

                var ctrl = _grid.Controller;
                Vector3D home = ctrl != null ? ctrl.GetPosition() : _p.Me.GetPosition();
                float y = 120;
                int rows = 0;
                for (int i = 0; i < _comms.PeerCount && rows < BoardRows; i++)
                {
                    if (_comms.PeerRole[i] == (int)FleetRole.Carrier) continue;
                    rows++;
                    if (_comms.PeerDistress[i] != 0) Rect(8, y - 2, 496, 44, DistressBg);

                    Text(_comms.PeerName[i], 16, y, 0.7f, Ink);
                    int st = _comms.PeerState[i];
                    int hold = _brain.DockQueuePlace(_comms.PeerAddress[i]);
                    if (_comms.PeerDistress[i] != 0) Text("MAYDAY", 16, y + 20, 0.55f, Bad);
                    else if (hold > 0)
                    {
                        Text("HOLDING #", 16, y + 20, 0.55f, Warn);
                        Text(Fmt.Str(hold), 120, y + 20, 0.55f, Warn);
                    }
                    else Text(st >= 0 && st < BrainFSM.StateLabels.Length ? BrainFSM.StateLabels[st] : "?", 16, y + 20, 0.55f, Accent);

                    // Cargo (top) and battery (bottom).
                    Rect(250, y + 6, 120, 10, Track);
                    Rect(250, y + 6, (float)(120 * Math.Min(1, _comms.PeerCargo[i])), 10, Warn);
                    Rect(250, y + 24, 120, 10, Track);
                    Rect(250, y + 24, (float)(120 * Math.Min(1, _comms.PeerBattery[i])), 10, Good);

                    // ETA when inbound, distance otherwise.
                    double dist = Vector3D.Distance(home, _comms.PeerPos[i]);
                    bool inbound = st == (int)FleetState.RequestDock || st == (int)FleetState.Approach || st == (int)FleetState.FinalDock;
                    if (st == (int)FleetState.Docked) Text("ON PAD", 496, y + 10, 0.6f, Good, TextAlignment.RIGHT);
                    else if (inbound)
                    {
                        double speed = Math.Max(_comms.PeerVel[i].Length(), 5);
                        Text("ETA", 390, y + 10, 0.6f, Dim);
                        Text(Fmt.Str(dist / speed), 496, y + 10, 0.6f, Ink, TextAlignment.RIGHT);
                    }
                    else if (dist < 1000) Text(Fmt.Str(dist), 496, y + 10, 0.6f, Ink, TextAlignment.RIGHT);
                    else
                    {
                        Text(Fmt.Str(dist / 1000), 470, y + 10, 0.6f, Ink, TextAlignment.RIGHT);
                        Text("km", 496, y + 10, 0.6f, Dim, TextAlignment.RIGHT);
                    }
                    y += 46;
                }
                if (rows == 0) Text("NO DRONES IN RANGE", 256, 240, 0.9f, Dim, TextAlignment.CENTER);
            }
        }
    }
}
