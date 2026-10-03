using System;
using System.Text;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>Commands, self-test, status text and persistence.</summary>
        public partial class BrainFSM
        {
            const string NoteSelfTestFail = "Self-test failed: see 'status'";
            const string NoteSet = "Setting changed (saved with the world)";
            const string NoteBadValue = "Usage: set <Key> <number|on|off>";
            const string NoteUnknownSite = "Unknown site (0-7, charted with 'setsite N')";
            const string NoteAsked = "Asked flight control for the site";
            const string NoteSiteScanned = "Site set in front of the rock face";
            const string NoteSiteManual = "Site set here (no rock seen by camera)";
            const string NoteGoto = "Next shaft set";
            const string NoteStatsReset = "Production stats reset";
            const double DetailSeconds = 30;

            public static readonly string[] TestNames =
            {
                "Controller", "Gyroscopes", "Thrust, 6 axes", "Lift vs gravity", "Dock connector",
                "Drills", "Antenna", "Forward camera", "Stone dump path", "Battery / H2", "Flight control"
            };
            public const int TestNa = -1, TestPass = 0, TestWarn = 1, TestFail = 2;
            /// <summary>Last self-test result per TestNames entry.</summary>
            public readonly int[] Tests = new int[11];
            public bool TestsRun { get; private set; }
            /// <summary>The PB screen shows the detail page (self-test, models) until this time.</summary>
            public double DetailUntil { get; private set; } = double.MinValue;
            int _gotoShaft = -1;

            // =================================================================
            // Commands (terminal / toolbar / fleet). Arguments are parsed in
            // place, so a command never allocates.
            // =================================================================

            /// <summary>True when argument starts with word followed by end or a space; next = index after it.</summary>
            static bool Is(string argument, string word, out int next)
            {
                next = word.Length;
                if (argument.Length < next
                    || string.Compare(argument, 0, word, 0, next, StringComparison.OrdinalIgnoreCase) != 0)
                    return false;
                return argument.Length == next || argument[next] == ' ';
            }

            static bool Is(string argument, string word)
            {
                int next;
                return Is(argument, word, out next);
            }

            public void HandleCommand(string argument)
            {
                _note = null;
                int at;

                if (Is(argument, "status")) { DetailUntil = _p.Clock + DetailSeconds; return; }
                if (Is(argument, "selftest")) { RunSelfTest(); return; }
                if (Is(argument, "set", out at)) { SetValue(argument, at); return; }
                if (Is(argument, "resetstats"))
                {
                    Array.Clear(Delivered, 0, Ore.Count);
                    _statsBase = -_p.Clock;
                    _note = NoteStatsReset;
                    return;
                }

                if (IsCarrier)
                {
                    if (Is(argument, "launch")) _comms.Broadcast(Op.FleetCommand, Cmd.Launch);
                    else if (Is(argument, "recall")) _comms.Broadcast(Op.FleetCommand, Cmd.Recall);
                    else if (Is(argument, "halt")) _comms.Broadcast(Op.FleetCommand, Cmd.Halt);
                    else if (Is(argument, "reset")) ResetSlots();
                    else if (Is(argument, "assign", out at)) AssignSite(GridManager.ParseInt(argument, at));
                    else _note = NoteUnknown;
                    return;
                }

                if (Is(argument, "start")) Start();
                else if (Is(argument, "stop")) Stop();
                else if (Is(argument, "return")) Recall();
                else if (Is(argument, "setsite", out at)) SetSite(GridManager.ParseInt(argument, at));
                else if (Is(argument, "site", out at)) AskForSite(GridManager.ParseInt(argument, at));
                else if (Is(argument, "resetsite")) ResetSite();
                else if (Is(argument, "skip")) Skip();
                else if (Is(argument, "goto", out at)) Goto(GridManager.ParseInt(argument, at));
                else if (Is(argument, "forget")) { _carrierAddr = 0; _slot = -1; }
                else _note = NoteUnknown;
            }

            /// <summary>"set Key value": value is a number, or on/off/true/false for switches.</summary>
            void SetValue(string argument, int at)
            {
                while (at < argument.Length && argument[at] == ' ') at++;
                int keyStart = at;
                while (at < argument.Length && argument[at] != ' ') at++;
                int key = Config.Find(argument, keyStart, at - keyStart);
                while (at < argument.Length && argument[at] == ' ') at++;

                double v;
                if (key < 0 || !ParseValue(argument, at, out v))
                {
                    _note = NoteBadValue;
                    return;
                }
                _cfg.Put(key, v);
                _cfg.Dirty = true;
                _note = NoteSet;
            }

            static bool ParseValue(string s, int at, out double v)
            {
                v = 0;
                if (at >= s.Length) return false;
                char c = char.ToLowerInvariant(s[at]);
                if (c == 't' || (c == 'o' && at + 1 < s.Length && char.ToLowerInvariant(s[at + 1]) == 'n')) { v = 1; return true; }
                if (c == 'f' || c == 'o') { v = 0; return true; }

                bool neg = s[at] == '-';
                if (neg) at++;
                bool any = false;
                double scale = 0;
                for (; at < s.Length; at++)
                {
                    char d = s[at];
                    if (d == '.' && scale == 0) { scale = 1; continue; }
                    if (d < '0' || d > '9') break;
                    any = true;
                    if (scale == 0) v = v * 10 + (d - '0');
                    else { scale *= 10; v += (d - '0') / scale; }
                }
                if (neg) v = -v;
                return any;
            }

            void Start()
            {
                if (!Site.Defined)
                {
                    _note = NoteNoSite;
                    return;
                }
                if (!RunSelfTest())
                {
                    _note = NoteSelfTestFail;
                    return;
                }
                if (Shaft < 0 && !PickShaft())
                {
                    _note = NoteSiteDone;
                    return;
                }
                _autoCycle = true;
                _recall = false;
                if (!_grid.IsConnected) CrumbCount = 0; // no outbound path from a mid-flight start
                Enter(_grid.IsConnected ? FleetState.Docked : FleetState.Transit);
            }

            void Recall()
            {
                _autoCycle = false;
                if (_grid.IsConnected) Enter(FleetState.Docked);
                else if (State == FleetState.Working) _recall = true; // back out of the shaft first
                else if (State != FleetState.Approach && State != FleetState.FinalDock
                    && State != FleetState.RequestDock) GoHome();
            }

            void Stop()
            {
                _autoCycle = false;
                _recall = false;
                Enter(_grid.IsConnected ? FleetState.Docked : FleetState.Idle);
            }

            /// <summary>Abandon the current shaft (e.g. it hit something the drills cannot cut).</summary>
            void Skip()
            {
                if (State == FleetState.Working)
                {
                    Retract(true, SiteMap.Blocked);
                }
                else if (Shaft >= 0)
                {
                    Site.Apply(Shaft, SiteMap.Blocked, Site.Value[Shaft], _p.IGC.Me, _p.Clock);
                    ReportShaft(SiteMap.Blocked);
                    PickShaft();
                }
            }

            void Goto(int shaft)
            {
                if (!Site.Defined || shaft < 0 || shaft >= Site.Limit)
                {
                    _note = NoteUnknown;
                    return;
                }
                _note = NoteGoto;
                if (State == FleetState.Working)
                {
                    _gotoShaft = shaft;
                    Retract(true, SiteMap.Partial); // this one stays resumable
                    return;
                }
                Shaft = shaft;
                ResetShaft();
                _staged = false;
            }

            void ResetSite()
            {
                if (!Site.Defined) return;
                Site.Define(Site.Pos, Site.Fwd, Site.Up, Site.SpacingX, Site.SpacingY, Site.Limit);
                PickShaft();
            }

            /// <summary>
            /// Records the site from the current pose. With a camera the site plane slides to
            /// SiteStandoff in front of the rock, so this works from any distance. A site number
            /// (0-7) charts it in the carrier's library so other drones can share it.
            /// </summary>
            void SetSite(int id)
            {
                var ctrl = _grid.Controller;
                if (ctrl == null) return;
                if (id >= LibrarySize)
                {
                    _note = NoteUnknownSite;
                    return;
                }
                MatrixD m = ctrl.WorldMatrix;
                Vector3D pos = m.Translation;
                bool hit;
                Vector3D point;
                _note = NoteSiteManual;
                if (_grid.TryScanRock(_cfg.ScanRange, out hit, out point) && hit)
                {
                    double face = Vector3D.Dot(point - m.Translation, m.Forward) - _grid.DrillReach;
                    pos = m.Translation + m.Forward * (face - _cfg.SiteStandoff);
                    _note = NoteSiteScanned;
                }

                // Auto spacing: the drill bank's cut, with 10% overlap so no ribs are left standing.
                double sx = _cfg.ShaftSpacing > 0 ? _cfg.ShaftSpacing : Math.Max(1, _grid.DrillWidth * 0.9);
                double sy = _cfg.ShaftSpacing > 0 ? _cfg.ShaftSpacing : Math.Max(1, _grid.DrillHeight * 0.9);
                Site.Define(pos, m.Forward, m.Up, sx, sy, (int)_cfg.MaxShafts);
                SiteId = id;
                _siteAdoptedAt = double.MinValue;
                PickShaft();

                if (id >= 0)
                    _comms.Send(_carrierAddr, Op.SiteDef, id, Site.Pos, Site.Fwd, Site.Up,
                        new Vector3D(Site.SpacingX, Site.SpacingY, Site.Limit));
            }

            void AskForSite(int id)
            {
                if (id < 0 || id >= LibrarySize)
                {
                    _note = NoteUnknownSite;
                    return;
                }
                _comms.Send(_carrierAddr, Op.SiteQuery, id);
                _note = NoteAsked;
            }

            // =================================================================
            // Self-test
            // =================================================================

            /// <summary>Checks the build; results show on the PB screen. False when anything fails.</summary>
            public bool RunSelfTest()
            {
                TestsRun = true;
                DetailUntil = _p.Clock + DetailSeconds;
                bool drone = !IsCarrier;
                var ctrl = _grid.Controller;

                Tests[0] = !drone ? TestNa : ctrl != null ? TestPass : TestFail;
                Tests[1] = !drone ? TestNa : _grid.Gyros.Count > 0 ? TestPass : TestFail;
                Tests[2] = !drone ? TestNa : _helm.ThrustOnAllAxes() ? TestPass : TestFail;
                double lift = _helm.LiftRatio;
                Tests[3] = !drone || double.IsInfinity(lift) ? TestNa
                    : lift >= _cfg.MinLift ? TestPass : lift >= 1 ? TestWarn : TestFail;
                Tests[4] = drone ? (_grid.Connector != null ? TestPass : TestFail)
                    : (_grid.DockConnectors.Count > 0 ? TestPass : TestFail);
                Tests[5] = !IsMiner ? TestNa : _grid.Drills.Count > 0 && !_grid.DrillsDamaged ? TestPass : TestFail;
                Tests[6] = _grid.AntennaReady ? TestPass : TestWarn;
                Tests[7] = !IsMiner ? TestNa : _grid.CameraReady ? TestPass : TestWarn;
                Tests[8] = !IsMiner || !_grid.CanEject ? TestNa : _grid.StonePathReady ? TestPass : TestWarn;
                Tests[9] = !drone ? TestNa : _grid.HasPowerStore ? TestPass : TestWarn;
                Tests[10] = !drone ? TestNa : _carrierAddr != 0 || CarrierKnown() ? TestPass : TestWarn;

                for (int i = 0; i < Tests.Length; i++)
                    if (Tests[i] == TestFail) return false;
                return true;
            }

            // =================================================================
            // Status text (text screens; Update100 only, allocation-free)
            // =================================================================

            public void AppendStatus(StringBuilder sb)
            {
                sb.Append(_cfg.Name).Append(" | ").Append(StateLabels[(int)State]);
                if (_autoCycle) sb.Append(" (auto)");
                sb.Append('\n');
                if (_note != null) sb.Append(_note).Append('\n');

                if (IsCarrier)
                {
                    for (int i = 0; i < SlotCount; i++)
                    {
                        sb.Append(" Pad ");
                        Fmt.Int(sb, i).Append(": ");
                        switch (SlotState(i))
                        {
                            case 0: sb.Append("free"); break;
                            case 1: sb.Append("inbound "); break;
                            case 2: sb.Append("occupied "); break;
                            default: sb.Append("reserved "); break;
                        }
                        if (_slotOwner[i] != 0 && SlotState(i) != 0) sb.Append(_comms.NameOf(_slotOwner[i]));
                        sb.Append('\n');
                    }
                    if (DockQueueCount > 0)
                    {
                        sb.Append("Holding: ");
                        Fmt.Int(sb, DockQueueCount).Append('\n');
                    }
                    if (LaunchingAt > _p.Clock)
                    {
                        sb.Append("Launch: ").Append(_comms.NameOf(Launching)).Append(" T-");
                        Fmt.Int(sb, (long)Math.Ceiling(LaunchingAt - _p.Clock)).Append('\n');
                    }
                    if (LaunchQueueCount > 0)
                    {
                        sb.Append("Launch queue: ");
                        Fmt.Int(sb, LaunchQueueCount).Append('\n');
                    }
                    double total = 0;
                    for (int i = 0; i < Ore.Count; i++) total += Delivered[i];
                    sb.Append("Delivered ");
                    Fmt.Mass(sb, total).Append('\n');
                    return;
                }

                if (Site.Defined)
                {
                    sb.Append("Site ");
                    if (SiteId >= 0) Fmt.Int(sb, SiteId); else sb.Append("(own)");
                    sb.Append("  shaft ");
                    if (Shaft >= 0) Fmt.Int(sb, Shaft); else sb.Append('-');
                    sb.Append('/');
                    Fmt.Int(sb, Site.Limit);
                    if (State == FleetState.Working && IsMiner)
                    {
                        if (_faceKnown)
                        {
                            sb.Append("  cut ");
                            Fmt.Fixed(sb, CutDepth, 1).Append('/');
                            Fmt.Fixed(sb, _cfg.MineDepth, 0).Append(" m");
                        }
                        else sb.Append("  seeking face");
                    }
                    sb.Append('\n');
                }
                if (State == FleetState.RequestDock && Holding)
                {
                    sb.Append("Holding, queue #");
                    Fmt.Int(sb, QueuePlace).Append('\n');
                }
                double home = HomeDistance();
                if (home >= 0)
                {
                    sb.Append("Home ");
                    Fmt.Fixed(sb, home, 0).Append(" m  needs ");
                    Fmt.Pct(sb, BatteryNeeded()).Append(" batt\n");
                }
                if (!double.IsInfinity(_helm.LiftRatio))
                {
                    sb.Append("Lift x");
                    Fmt.Fixed(sb, _helm.LiftRatio, 2).Append('\n');
                }
                if (_helm.OnAutopilot)
                {
                    sb.Append("Autopilot ");
                    Fmt.Fixed(sb, _helm.DistanceToTarget, 0).Append(" m\n");
                }
                else if (_helm.Engaged)
                {
                    sb.Append("Target ");
                    Fmt.Fixed(sb, _helm.DistanceToTarget, 1).Append(" m  err ");
                    Fmt.Fixed(sb, _helm.AlignmentError * (180 / Math.PI), 1).Append(" deg\n");
                }
            }

            /// <summary>Detail page: self-test results and the learned models ('status' / 'selftest').</summary>
            public void AppendDetail(StringBuilder sb)
            {
                sb.Append(_cfg.Name).Append(" | DETAIL\n");
                if (_note != null) sb.Append(_note).Append('\n');
                if (TestsRun)
                {
                    sb.Append("-- Self-test --\n");
                    for (int i = 0; i < Tests.Length; i++)
                    {
                        if (Tests[i] == TestNa) continue;
                        sb.Append(Tests[i] == TestPass ? "[ OK ] " : Tests[i] == TestWarn ? "[WARN] " : "[FAIL] ");
                        sb.Append(TestNames[i]).Append('\n');
                    }
                }
                if (IsCarrier)
                {
                    for (int i = 0; i < LibrarySize; i++)
                    {
                        if (!_library[i].Defined) continue;
                        sb.Append("Site ");
                        Fmt.Int(sb, i);
                        AppendSiteCounts(sb, _library[i]);
                    }
                    return;
                }
                sb.Append("-- Models --\nBurn/km: batt ");
                Fmt.Fixed(sb, BattPerMetre * 1e5, 2).Append("%  H2 ");
                Fmt.Fixed(sb, H2PerMetre * 1e5, 2).Append("%\nBreadcrumbs ");
                Fmt.Int(sb, CrumbCount).Append("  drill bank ");
                Fmt.Fixed(sb, _grid.DrillWidth, 1).Append('x');
                Fmt.Fixed(sb, _grid.DrillHeight, 1).Append(" m\n");
                if (Site.Defined)
                {
                    sb.Append("Site");
                    AppendSiteCounts(sb, Site);
                }
            }

            static void AppendSiteCounts(StringBuilder sb, SiteMap s)
            {
                sb.Append(": done ");
                Fmt.Int(sb, s.Count(SiteMap.Done)).Append(" part ");
                Fmt.Int(sb, s.Count(SiteMap.Partial)).Append(" barren ");
                Fmt.Int(sb, s.Count(SiteMap.Barren)).Append(" empty ");
                Fmt.Int(sb, s.Count(SiteMap.Empty)).Append(" / ");
                Fmt.Int(sb, s.Limit).Append('\n');
            }

            // =================================================================
            // Persistence (Save() / Program() only: allocation is fine here)
            // =================================================================

            const string SaveSection = "FleetMinerState";
            const int SaveVersion = 3;

            public string Serialize()
            {
                var ic = System.Globalization.CultureInfo.InvariantCulture;
                var ini = new MyIni();
                ini.Set(SaveSection, "Version", SaveVersion);
                ini.Set(SaveSection, "Role", (int)_cfg.Role);
                ini.Set(SaveSection, "State", (int)State);
                ini.Set(SaveSection, "Auto", _autoCycle);
                ini.Set(SaveSection, "Carrier", _carrierAddr.ToString(ic));
                ini.Set(SaveSection, "Slot", _slot);
                ini.Set(SaveSection, "SiteId", SiteId);
                ini.Set(SaveSection, "Shaft", Shaft);
                ini.Set(SaveSection, "FaceKnown", _faceKnown);
                ini.Set(SaveSection, "FaceDepth", _faceDepth);
                ini.Set(SaveSection, "MinedDepth", _minedDepth);
                ini.Set(SaveSection, "BattPerMetre", BattPerMetre);
                ini.Set(SaveSection, "H2PerMetre", H2PerMetre);
                ini.Set(SaveSection, "StatsSeconds", StatsSeconds);
                ini.Set(SaveSection, "MapSite", MapSite);

                var sb = new StringBuilder();
                for (int i = 0; i < Ore.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(Math.Round(Delivered[i]).ToString(ic));
                }
                ini.Set(SaveSection, "Delivered", sb.ToString());

                sb.Clear();
                Site.Write(sb);
                ini.Set(SaveSection, "Site", sb.ToString());

                if (IsCarrier)
                {
                    sb.Clear();
                    for (int i = 0; i < MaxSlots; i++)
                    {
                        if (i > 0) sb.Append(',');
                        sb.Append(_slotOwner[i].ToString(ic));
                    }
                    ini.Set(SaveSection, "Owners", sb.ToString());
                    for (int i = 0; i < LibrarySize; i++)
                    {
                        if (!_library[i].Defined) continue;
                        sb.Clear();
                        _library[i].Write(sb);
                        ini.Set(SaveSection, "Library" + i, sb.ToString());
                    }
                }
                return ini.ToString();
            }

            public void Restore(string storage)
            {
                var ini = new MyIni();
                MyIniParseResult result;
                if (string.IsNullOrEmpty(storage) || !ini.TryParse(storage, out result)) return;
                if (ini.Get(SaveSection, "Version").ToInt32() != SaveVersion) return;
                if (ini.Get(SaveSection, "Role").ToInt32() != (int)_cfg.Role) return; // role changed: start clean

                var saved = (FleetState)ini.Get(SaveSection, "State").ToInt32();
                _autoCycle = ini.Get(SaveSection, "Auto").ToBoolean();
                long.TryParse(ini.Get(SaveSection, "Carrier").ToString("0"), System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out _carrierAddr);
                _slot = ini.Get(SaveSection, "Slot").ToInt32(-1);
                SiteId = ini.Get(SaveSection, "SiteId").ToInt32(-1);
                Site.Read(ini.Get(SaveSection, "Site").ToString());
                Shaft = Site.Defined ? ini.Get(SaveSection, "Shaft").ToInt32(-1) : -1;
                _faceKnown = ini.Get(SaveSection, "FaceKnown").ToBoolean();
                _faceDepth = ini.Get(SaveSection, "FaceDepth").ToDouble();
                _minedDepth = ini.Get(SaveSection, "MinedDepth").ToDouble();
                BattPerMetre = ini.Get(SaveSection, "BattPerMetre").ToDouble();
                H2PerMetre = ini.Get(SaveSection, "H2PerMetre").ToDouble();
                _statsBase = ini.Get(SaveSection, "StatsSeconds").ToDouble();
                MapSite = ini.Get(SaveSection, "MapSite").ToInt32(-1);

                string[] delivered = ini.Get(SaveSection, "Delivered").ToString().Split(',');
                for (int i = 0; i < Ore.Count && i < delivered.Length; i++)
                    Delivered[i] = SiteMap.Dbl(delivered[i]);

                if (IsCarrier)
                {
                    string[] owners = ini.Get(SaveSection, "Owners").ToString().Split(',');
                    for (int i = 0; i < owners.Length && i < MaxSlots; i++)
                    {
                        long.TryParse(owners[i], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out _slotOwner[i]);
                        _slotLastSeen[i] = _p.Clock; // give absent drones a full grace period after reload
                    }
                    for (int i = 0; i < LibrarySize; i++)
                        _library[i].Read(ini.Get(SaveSection, "Library" + i).ToString());
                    return;
                }

                // Dock poses are stale after a reload, so any docking phase restarts from a request.
                switch (saved)
                {
                    case FleetState.Transit:
                    case FleetState.Working:
                        Enter(Site.Defined ? FleetState.Transit : FleetState.Idle);
                        break;
                    case FleetState.Docked:
                    case FleetState.Undocking:
                    case FleetState.RequestDock:
                    case FleetState.Approach:
                    case FleetState.FinalDock:
                        Enter(_grid.IsConnected ? FleetState.Docked : FleetState.RequestDock);
                        break;
                    default:
                        if (_grid.IsConnected) Enter(FleetState.Docked);
                        break;
                }
            }
        }
    }
}
