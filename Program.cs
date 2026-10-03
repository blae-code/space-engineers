using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // ------------------------------------------------------------------
        // Kernel: owns the subsystems and routes ticks, commands and IGC.
        // ------------------------------------------------------------------

        const UpdateType CommandSources =
            UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script | UpdateType.Mod;

        public readonly Config Cfg;
        public readonly Logbook Log;

        /// <summary>Seconds since the script was constructed (monotonic).</summary>
        public double Clock { get; private set; }

        /// <summary>Seconds elapsed between the last two Update10 ticks.</summary>
        public double Dt10 { get; private set; }

        readonly GridManager _grid;
        readonly CommsOfficer _comms;
        readonly HelmController _helm;
        readonly BrainFSM _brain;
        readonly ISubsystem[] _subsystems;

        double _last10 = -1;

        public Program()
        {
            Fmt.Init();
            Cfg = new Config();
            Cfg.Sync(Me, true);
            Log = new Logbook(this);

            _grid = new GridManager(this);
            _comms = new CommsOfficer(this);
            _helm = new HelmController(this, _grid);
            _brain = new BrainFSM(this, _grid, _comms, _helm);
            var display = new Display(this, _grid, _comms, _helm, _brain);

            // Order matters: telemetry -> network -> decisions -> actuation -> screens.
            _subsystems = new ISubsystem[] { _grid, _comms, _brain, _helm, display };
            for (int i = 0; i < _subsystems.Length; i++)
                _subsystems[i].Initialize();

            _brain.Restore(Storage);

            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            Echo("FleetMiner Core online: " + Cfg.Name + " (" + Config.RoleNames[(int)Cfg.Role] + ")");
        }

        public void Save()
        {
            Storage = _brain.Serialize();
            if (Cfg.Dirty) Cfg.Sync(Me, false); // persist 'set' changes into Custom Data
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Clock += Runtime.TimeSinceLastRun.TotalSeconds;

            if ((updateSource & CommandSources) != 0 && !string.IsNullOrEmpty(argument))
                _brain.HandleCommand(argument);

            if ((updateSource & (UpdateType.IGC | UpdateType.Update10)) != 0)
                _comms.Drain(_subsystems);

            if ((updateSource & UpdateType.Update10) != 0)
            {
                Dt10 = _last10 < 0 ? 1.0 / 6.0 : Clock - _last10;
                _last10 = Clock;
                for (int i = 0; i < _subsystems.Length; i++)
                    _subsystems[i].Update10();
            }

            if ((updateSource & UpdateType.Update100) != 0)
                for (int i = 0; i < _subsystems.Length; i++)
                    _subsystems[i].Update100();
        }

        // ------------------------------------------------------------------
        // Configuration (Custom Data). Numeric and boolean keys live in one
        // table so the 'set' command can change any of them by name.
        // ------------------------------------------------------------------

        public enum FleetRole { Miner, Hauler, Carrier, Mothership }

        public class Config
        {
            public static readonly string[] RoleNames = { "Miner", "Hauler", "Carrier", "Mothership" };
            const string Section = "FleetMiner";

            public FleetRole Role = FleetRole.Miner;
            public string Channel = "FLEETMINER";
            public string Callsign = "";            // empty = use the grid name
            public string Prefer = "Platinum,Uranium,Gold";
            public string FleetKey = "";            // shared secret: messages without it are ignored
            public string HaulMode = "Site";        // Hauler: Site (bays at the work site) | Shuttle (carrier -> mothership)
            public string DockTag = "[FM Dock]";
            public string LcdTag = "[FM LCD]";
            public string RefTag = "[FM Ref]";
            public string CamTag = "[FM Cam]";
            public string EjectTag = "[FM Eject]";
            public string UnloadTag = "[FM Unload]";
            public string SensorTag = "[FM Sensor]";

            /// <summary>Display name: Callsign, or the grid name when none is set.</summary>
            public string Name;
            /// <summary>Ore types listed in Prefer (indexed like Ore.Names).</summary>
            public readonly bool[] PreferOre = new bool[Ore.Count];
            /// <summary>Set by 'set'; Save() writes the values back to Custom Data.</summary>
            public bool Dirty;
            /// <summary>24-bit FNV-1a hash of FleetKey, stamped on every message.</summary>
            public int KeyHash;
            public bool Shuttle;
            /// <summary>Carrier or mothership: serves pads, never flies the mining cycle.</summary>
            public bool IsBase { get { return Role >= FleetRole.Carrier; } }

            public double CargoFull = 0.90, CargoEmpty = 0.02, LaunchCharge = 0.90;
            public double ReturnCharge = 0.25, ReturnHydrogen = 0.20, ReserveCharge = 0.10, EnergyMargin = 1.5;
            public double MaxSpeed = 40, ApproachSpeed = 8, DockSpeed = 1.5, Decel = 4;
            public double ApproachDistance = 40, DockGap = 1.5;
            public double MineSpeed = 1.0, MineDepth = 30, ShaftSpacing = 0, MaxShafts = 25;
            public double SiteStandoff = 10, FaceMargin = 2, ScanRange = 100, StallTime = 20;
            public double BarrenDepth = 10, ClaimTimeout = 900, MinLift = 1.25;
            public double AutopilotRange = 300, CrumbSpacing = 100, HoldDistance = 150;
            public double LaunchInterval = 10, LaunchCountdown = 5, DamageTolerance = 0.03;
            public double BeaconTimeout = 5, DockTimeout = 60;
            public double AntennaMax = 50000, MinAltitude = 50, FleeDistance = 1500, FleeTime = 30;
            public double Separation = 25, LinkTimeout = 60;
            public bool Autopilot = true, Unload = true, RecallOnDistress = true;
            public bool AntennaAuto = true, ConfigureSensors = true, Survey = true;

            public static readonly string[] Keys =
            {
                "CargoFull", "CargoEmpty", "LaunchCharge", "ReturnCharge", "ReturnHydrogen",
                "ReserveCharge", "EnergyMargin", "MaxSpeed", "ApproachSpeed", "DockSpeed",
                "Decel", "ApproachDistance", "DockGap", "MineSpeed", "MineDepth",
                "ShaftSpacing", "MaxShafts", "SiteStandoff", "FaceMargin", "ScanRange",
                "StallTime", "BarrenDepth", "ClaimTimeout", "MinLift", "AutopilotRange",
                "CrumbSpacing", "HoldDistance", "LaunchInterval", "LaunchCountdown", "DamageTolerance",
                "BeaconTimeout", "DockTimeout", "AntennaMax", "MinAltitude", "FleeDistance",
                "FleeTime", "Separation", "LinkTimeout",
                "Autopilot", "Unload", "RecallOnDistress", "AntennaAuto", "ConfigureSensors", "Survey"
            };
            const int FirstBool = 38; // Keys from this index on are booleans (stored as 0/1)

            public double Get(int i)
            {
                switch (i)
                {
                    case 0: return CargoFull;
                    case 1: return CargoEmpty;
                    case 2: return LaunchCharge;
                    case 3: return ReturnCharge;
                    case 4: return ReturnHydrogen;
                    case 5: return ReserveCharge;
                    case 6: return EnergyMargin;
                    case 7: return MaxSpeed;
                    case 8: return ApproachSpeed;
                    case 9: return DockSpeed;
                    case 10: return Decel;
                    case 11: return ApproachDistance;
                    case 12: return DockGap;
                    case 13: return MineSpeed;
                    case 14: return MineDepth;
                    case 15: return ShaftSpacing;
                    case 16: return MaxShafts;
                    case 17: return SiteStandoff;
                    case 18: return FaceMargin;
                    case 19: return ScanRange;
                    case 20: return StallTime;
                    case 21: return BarrenDepth;
                    case 22: return ClaimTimeout;
                    case 23: return MinLift;
                    case 24: return AutopilotRange;
                    case 25: return CrumbSpacing;
                    case 26: return HoldDistance;
                    case 27: return LaunchInterval;
                    case 28: return LaunchCountdown;
                    case 29: return DamageTolerance;
                    case 30: return BeaconTimeout;
                    case 31: return DockTimeout;
                    case 32: return AntennaMax;
                    case 33: return MinAltitude;
                    case 34: return FleeDistance;
                    case 35: return FleeTime;
                    case 36: return Separation;
                    case 37: return LinkTimeout;
                    case 38: return Autopilot ? 1 : 0;
                    case 39: return Unload ? 1 : 0;
                    case 40: return RecallOnDistress ? 1 : 0;
                    case 41: return AntennaAuto ? 1 : 0;
                    case 42: return ConfigureSensors ? 1 : 0;
                    case 43: return Survey ? 1 : 0;
                }
                return 0;
            }

            public void Put(int i, double v)
            {
                switch (i)
                {
                    case 0: CargoFull = v; break;
                    case 1: CargoEmpty = v; break;
                    case 2: LaunchCharge = v; break;
                    case 3: ReturnCharge = v; break;
                    case 4: ReturnHydrogen = v; break;
                    case 5: ReserveCharge = v; break;
                    case 6: EnergyMargin = v; break;
                    case 7: MaxSpeed = v; break;
                    case 8: ApproachSpeed = v; break;
                    case 9: DockSpeed = v; break;
                    case 10: Decel = v; break;
                    case 11: ApproachDistance = v; break;
                    case 12: DockGap = v; break;
                    case 13: MineSpeed = v; break;
                    case 14: MineDepth = v; break;
                    case 15: ShaftSpacing = v; break;
                    case 16: MaxShafts = Math.Round(v); break;
                    case 17: SiteStandoff = v; break;
                    case 18: FaceMargin = v; break;
                    case 19: ScanRange = v; break;
                    case 20: StallTime = v; break;
                    case 21: BarrenDepth = v; break;
                    case 22: ClaimTimeout = v; break;
                    case 23: MinLift = v; break;
                    case 24: AutopilotRange = v; break;
                    case 25: CrumbSpacing = v; break;
                    case 26: HoldDistance = v; break;
                    case 27: LaunchInterval = v; break;
                    case 28: LaunchCountdown = v; break;
                    case 29: DamageTolerance = v; break;
                    case 30: BeaconTimeout = v; break;
                    case 31: DockTimeout = v; break;
                    case 32: AntennaMax = v; break;
                    case 33: MinAltitude = v; break;
                    case 34: FleeDistance = v; break;
                    case 35: FleeTime = v; break;
                    case 36: Separation = v; break;
                    case 37: LinkTimeout = v; break;
                    case 38: Autopilot = v != 0; break;
                    case 39: Unload = v != 0; break;
                    case 40: RecallOnDistress = v != 0; break;
                    case 41: AntennaAuto = v != 0; break;
                    case 42: ConfigureSensors = v != 0; break;
                    case 43: Survey = v != 0; break;
                }
            }

            public static bool IsBool(int i) { return i >= FirstBool; }

            /// <summary>Index of the key named by argument[start, start+len), or -1. Allocation-free.</summary>
            public static int Find(string argument, int start, int len)
            {
                for (int i = 0; i < Keys.Length; i++)
                    if (Keys[i].Length == len
                        && string.Compare(argument, start, Keys[i], 0, len, StringComparison.OrdinalIgnoreCase) == 0)
                        return i;
                return -1;
            }

            /// <summary>
            /// read=true: load Custom Data (constructor). read=false: write current values back
            /// (Save). Either way every key ends up in Custom Data so it is discoverable.
            /// </summary>
            public void Sync(IMyProgrammableBlock me, bool read)
            {
                var ini = new MyIni();
                MyIniParseResult result;
                if (!ini.TryParse(me.CustomData, out result))
                {
                    if (read) Name = Callsign.Length > 0 ? Callsign : me.CubeGrid.CustomName;
                    return; // Keep defaults; never clobber Custom Data we could not parse.
                }

                string role = ini.Get(Section, "Role").ToString(RoleNames[(int)Role]);
                for (int i = 0; i < RoleNames.Length; i++)
                    if (string.Equals(role, RoleNames[i], StringComparison.OrdinalIgnoreCase))
                        Role = (FleetRole)i;
                ini.Set(Section, "Role", RoleNames[(int)Role]);

                Channel = Str(ini, "Channel", Channel);
                Callsign = Str(ini, "Callsign", Callsign);
                Prefer = Str(ini, "Prefer", Prefer);
                FleetKey = Str(ini, "FleetKey", FleetKey);
                HaulMode = Str(ini, "HaulMode", HaulMode);
                DockTag = Str(ini, "DockTag", DockTag);
                LcdTag = Str(ini, "LcdTag", LcdTag);
                RefTag = Str(ini, "RefTag", RefTag);
                CamTag = Str(ini, "CamTag", CamTag);
                EjectTag = Str(ini, "EjectTag", EjectTag);
                UnloadTag = Str(ini, "UnloadTag", UnloadTag);
                SensorTag = Str(ini, "SensorTag", SensorTag);

                for (int i = 0; i < Keys.Length; i++)
                {
                    if (IsBool(i))
                    {
                        bool b = read ? ini.Get(Section, Keys[i]).ToBoolean(Get(i) != 0) : Get(i) != 0;
                        Put(i, b ? 1 : 0);
                        ini.Set(Section, Keys[i], b);
                    }
                    else
                    {
                        double v = read ? ini.Get(Section, Keys[i]).ToDouble(Get(i)) : Get(i);
                        Put(i, v);
                        ini.Set(Section, Keys[i], v);
                    }
                }

                Name = Callsign.Length > 0 ? Callsign : me.CubeGrid.CustomName;
                Shuttle = string.Equals(HaulMode, "Shuttle", StringComparison.OrdinalIgnoreCase);
                uint h = 2166136261;
                for (int i = 0; i < FleetKey.Length; i++)
                    h = (h ^ FleetKey[i]) * 16777619;
                KeyHash = (int)(h & 0xFFFFFF);
                string[] prefer = Prefer.Split(',');
                for (int i = 0; i < Ore.Count; i++)
                {
                    PreferOre[i] = false;
                    for (int j = 0; j < prefer.Length; j++)
                        if (string.Equals(prefer[j].Trim(), Ore.Names[i], StringComparison.OrdinalIgnoreCase))
                            PreferOre[i] = true;
                }

                Dirty = false;
                string text = ini.ToString();
                if (text != me.CustomData)
                    me.CustomData = text;
            }

            static string Str(MyIni ini, string key, string def)
            {
                string v = ini.Get(Section, key).ToString(def);
                ini.Set(Section, key, v);
                return v;
            }
        }

        // ------------------------------------------------------------------
        // Ore catalogue: vanilla ore subtypes, in a fixed order used by every
        // per-ore array (yields, deliveries, the Delivery message layout).
        // ------------------------------------------------------------------

        public static class Ore
        {
            public const int Count = 12, Stone = 0;
            public const string TypeId = "MyObjectBuilder_Ore";
            public static readonly string[] Names =
            {
                "Stone", "Iron", "Nickel", "Cobalt", "Magnesium", "Silicon",
                "Silver", "Gold", "Platinum", "Uranium", "Ice", "Scrap"
            };

            public static int Index(string subtype)
            {
                for (int i = 0; i < Count; i++)
                    if (subtype == Names[i]) return i;
                return -1;
            }
        }

        // ------------------------------------------------------------------
        // Logbook: a fixed ring of events. Entries only hold references to
        // strings that already exist (literals, callsigns), so adding is free.
        // ------------------------------------------------------------------

        public class Logbook
        {
            const int Size = 16;
            public const long NoNumber = long.MinValue;

            readonly Program _p;
            readonly double[] _time = new double[Size];
            readonly string[] _who = new string[Size];
            readonly string[] _what = new string[Size];
            readonly long[] _num = new long[Size];
            readonly string[] _unit = new string[Size];
            int _next, _count;

            public Logbook(Program p) { _p = p; }

            public void Add(string who, string what, long num = NoNumber, string unit = null)
            {
                _time[_next] = _p.Clock;
                _who[_next] = who;
                _what[_next] = what;
                _num[_next] = num;
                _unit[_next] = unit;
                _next = (_next + 1) % Size;
                if (_count < Size) _count++;
            }

            /// <summary>Appends the newest <paramref name="lines"/> entries, oldest first.</summary>
            public void Render(StringBuilder sb, int lines)
            {
                int n = Math.Min(lines, _count);
                for (int k = n; k > 0; k--)
                {
                    int i = (_next - k + Size) % Size;
                    sb.Append('[');
                    Fmt.Clock(sb, _time[i]).Append("] ");
                    if (_who[i] != null) sb.Append(_who[i]).Append(": ");
                    sb.Append(_what[i]);
                    if (_num[i] != NoNumber)
                    {
                        sb.Append(' ');
                        Fmt.Int(sb, _num[i]);
                        if (_unit[i] != null) sb.Append(_unit[i]);
                    }
                    sb.Append('\n');
                }
            }
        }

        // ------------------------------------------------------------------
        // Allocation-free number formatting. StringBuilder.Append(double)
        // calls ToString() internally, which allocates every tick. Sprites need
        // real strings, so 0..999 are cached once at start-up.
        // ------------------------------------------------------------------

        public static class Fmt
        {
            static readonly string[] _cache = new string[1000];

            public static void Init()
            {
                for (int i = 0; i < _cache.Length; i++)
                    _cache[i] = i.ToString();
            }

            /// <summary>Cached string for 0..999 (clamped), safe to hand to a sprite.</summary>
            public static string Str(double v)
            {
                if (double.IsNaN(v) || v < 0) return "0";
                return v >= 999.5 ? "999" : _cache[(int)Math.Round(v)];
            }

            public static StringBuilder Int(StringBuilder sb, long v)
            {
                if (v < 0) { sb.Append('-'); v = -v; }
                if (v == 0) return sb.Append('0');

                int start = sb.Length;
                while (v > 0)
                {
                    sb.Append((char)('0' + (int)(v % 10)));
                    v /= 10;
                }
                for (int end = sb.Length - 1; start < end; start++, end--)
                {
                    char t = sb[start];
                    sb[start] = sb[end];
                    sb[end] = t;
                }
                return sb;
            }

            public static StringBuilder Fixed(StringBuilder sb, double v, int decimals)
            {
                if (double.IsNaN(v) || double.IsInfinity(v)) return sb.Append("--");
                if (v < 0) { sb.Append('-'); v = -v; }
                if (v > 1e12) v = 1e12;

                long scale = 1;
                for (int i = 0; i < decimals; i++) scale *= 10;
                long scaled = (long)Math.Round(v * scale);

                Int(sb, scaled / scale);
                if (decimals > 0)
                {
                    sb.Append('.');
                    long frac = scaled % scale;
                    for (long d = scale / 10; d > 0; d /= 10)
                    {
                        sb.Append((char)('0' + (int)(frac / d)));
                        frac %= d;
                    }
                }
                return sb;
            }

            public static StringBuilder Pct(StringBuilder sb, double ratio)
            {
                return Fixed(sb, ratio * 100, 0).Append('%');
            }

            /// <summary>Kilograms as "850 kg", "12.3 t" or "4.56 kt".</summary>
            public static StringBuilder Mass(StringBuilder sb, double kg)
            {
                if (kg < 1000) return Fixed(sb, kg, 0).Append(" kg");
                if (kg < 1e6) return Fixed(sb, kg / 1000, 1).Append(" t");
                return Fixed(sb, kg / 1e6, 2).Append(" kt");
            }

            /// <summary>Seconds as m:ss, or h:mm:ss past an hour.</summary>
            public static StringBuilder Clock(StringBuilder sb, double seconds)
            {
                long s = (long)Math.Max(0, seconds);
                if (s >= 3600)
                {
                    Int(sb, s / 3600).Append(':');
                    Pad2(sb, s / 60 % 60);
                }
                else
                {
                    Int(sb, s / 60);
                }
                sb.Append(':');
                return Pad2(sb, s % 60);
            }

            static StringBuilder Pad2(StringBuilder sb, long v)
            {
                if (v < 10) sb.Append('0');
                return Int(sb, v);
            }
        }
    }
}
