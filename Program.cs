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

        /// <summary>Seconds since the script was constructed (monotonic).</summary>
        public double Clock { get; private set; }

        /// <summary>Seconds elapsed between the last two Update10 ticks.</summary>
        public double Dt10 { get; private set; }

        readonly GridManager _grid;
        readonly CommsOfficer _comms;
        readonly HelmController _helm;
        readonly BrainFSM _brain;
        readonly ISubsystem[] _subsystems;

        readonly StringBuilder _status = new StringBuilder(2048);
        readonly StringBuilder _save = new StringBuilder(512);
        double _last10 = -1;

        public Program()
        {
            Cfg = new Config();
            Cfg.Load(Me);

            _grid = new GridManager(this);
            _comms = new CommsOfficer(this);
            _helm = new HelmController(this, _grid);
            _brain = new BrainFSM(this, _grid, _comms, _helm);

            // Order matters: telemetry -> network -> decisions -> actuation.
            _subsystems = new ISubsystem[] { _grid, _comms, _brain, _helm };
            for (int i = 0; i < _subsystems.Length; i++)
                _subsystems[i].Initialize();

            _brain.Restore(Storage);

            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            Echo("FleetMiner Core online. Role: " + Config.RoleNames[(int)Cfg.Role]);
        }

        public void Save()
        {
            _save.Clear();
            _brain.Serialize(_save);
            Storage = _save.ToString();
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
            {
                for (int i = 0; i < _subsystems.Length; i++)
                    _subsystems[i].Update100();
                RenderStatus();
            }
        }

        void RenderStatus()
        {
            _status.Clear();
            _status.Append("== FleetMiner Core ==\n");
            _brain.AppendStatus(_status);
            _grid.AppendStatus(_status);
            _comms.AppendStatus(_status);
            _status.Append("CPU: ");
            Fmt.Fixed(_status, Runtime.LastRunTimeMs, 3).Append(" ms / ");
            Fmt.Int(_status, Runtime.CurrentInstructionCount).Append(" instr\n");
            _grid.WriteDisplays(_status);
        }

        // ------------------------------------------------------------------
        // Configuration (Custom Data, parsed once in Program()).
        // ------------------------------------------------------------------

        public enum FleetRole { Miner, Hauler, Carrier }

        public class Config
        {
            public static readonly string[] RoleNames = { "Miner", "Hauler", "Carrier" };
            const string Section = "FleetMiner";

            public FleetRole Role = FleetRole.Miner;
            public string Channel = "FLEETMINER";
            public string DockTag = "[FM Dock]";
            public string LcdTag = "[FM LCD]";
            public string RefTag = "[FM Ref]";
            public string CamTag = "[FM Cam]";
            public string EjectTag = "[FM Eject]";
            public string UnloadTag = "[FM Unload]";

            public double CargoFull = 0.90;
            public double CargoEmpty = 0.02;
            public double LaunchCharge = 0.90;
            public double ReturnCharge = 0.25;
            public double ReturnHydrogen = 0.20;

            public double MaxSpeed = 40;
            public double ApproachSpeed = 8;
            public double DockSpeed = 1.5;
            public double Decel = 4;
            public double ApproachDistance = 40;
            public double DockGap = 1.5;

            public double MineSpeed = 1.0;
            public double MineDepth = 30;
            public double ShaftSpacing = 0;     // 0 = derive from the drill layout
            public int MaxShafts = 25;          // 0 = unlimited
            public double SiteStandoff = 10;
            public double FaceMargin = 2;
            public double ScanRange = 100;
            public double StallTime = 20;
            public double MinLift = 1.25;

            public bool Autopilot = true;
            public double AutopilotRange = 300;
            public bool Unload = true;

            public double BeaconTimeout = 5;
            public double DockTimeout = 60;

            public void Load(IMyProgrammableBlock me)
            {
                var ini = new MyIni();
                MyIniParseResult result;
                if (!ini.TryParse(me.CustomData, out result))
                    return; // Keep defaults; never clobber Custom Data we could not parse.

                string role = ini.Get(Section, "Role").ToString(RoleNames[(int)Role]);
                for (int i = 0; i < RoleNames.Length; i++)
                    if (string.Equals(role, RoleNames[i], StringComparison.OrdinalIgnoreCase))
                        Role = (FleetRole)i;
                ini.Set(Section, "Role", RoleNames[(int)Role]);

                Channel = Str(ini, "Channel", Channel);
                DockTag = Str(ini, "DockTag", DockTag);
                LcdTag = Str(ini, "LcdTag", LcdTag);
                RefTag = Str(ini, "RefTag", RefTag);
                CamTag = Str(ini, "CamTag", CamTag);
                EjectTag = Str(ini, "EjectTag", EjectTag);
                UnloadTag = Str(ini, "UnloadTag", UnloadTag);

                CargoFull = Num(ini, "CargoFull", CargoFull);
                CargoEmpty = Num(ini, "CargoEmpty", CargoEmpty);
                LaunchCharge = Num(ini, "LaunchCharge", LaunchCharge);
                ReturnCharge = Num(ini, "ReturnCharge", ReturnCharge);
                ReturnHydrogen = Num(ini, "ReturnHydrogen", ReturnHydrogen);

                MaxSpeed = Num(ini, "MaxSpeed", MaxSpeed);
                ApproachSpeed = Num(ini, "ApproachSpeed", ApproachSpeed);
                DockSpeed = Num(ini, "DockSpeed", DockSpeed);
                Decel = Num(ini, "Decel", Decel);
                ApproachDistance = Num(ini, "ApproachDistance", ApproachDistance);
                DockGap = Num(ini, "DockGap", DockGap);

                MineSpeed = Num(ini, "MineSpeed", MineSpeed);
                MineDepth = Num(ini, "MineDepth", MineDepth);
                ShaftSpacing = Num(ini, "ShaftSpacing", ShaftSpacing);
                MaxShafts = Int(ini, "MaxShafts", MaxShafts);
                SiteStandoff = Num(ini, "SiteStandoff", SiteStandoff);
                FaceMargin = Num(ini, "FaceMargin", FaceMargin);
                ScanRange = Num(ini, "ScanRange", ScanRange);
                StallTime = Num(ini, "StallTime", StallTime);
                MinLift = Num(ini, "MinLift", MinLift);

                Autopilot = Bool(ini, "Autopilot", Autopilot);
                AutopilotRange = Num(ini, "AutopilotRange", AutopilotRange);
                Unload = Bool(ini, "Unload", Unload);

                BeaconTimeout = Num(ini, "BeaconTimeout", BeaconTimeout);
                DockTimeout = Num(ini, "DockTimeout", DockTimeout);

                // Write back so every key is discoverable in the terminal.
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

            static double Num(MyIni ini, string key, double def)
            {
                double v = ini.Get(Section, key).ToDouble(def);
                ini.Set(Section, key, v);
                return v;
            }

            static int Int(MyIni ini, string key, int def)
            {
                int v = ini.Get(Section, key).ToInt32(def);
                ini.Set(Section, key, v);
                return v;
            }

            static bool Bool(MyIni ini, string key, bool def)
            {
                bool v = ini.Get(Section, key).ToBoolean(def);
                ini.Set(Section, key, v);
                return v;
            }
        }

        // ------------------------------------------------------------------
        // Allocation-free number formatting. StringBuilder.Append(double)
        // calls ToString() internally, which allocates every tick.
        // ------------------------------------------------------------------

        public static class Fmt
        {
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
        }
    }
}
