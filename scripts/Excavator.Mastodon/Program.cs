// Excavator.Mastodon · Space Engineers 1 MDK2 PB Script
// Target Blocks: [MAS] PB Excavator & [MAS] PB RIG//OS-Lite
// Grid: Large Grid Consist (11 × 42 × 6 B · 10 Wheels · 2 Carriages) · Update: Update1 / Update10
// Subsystem Groups: MAS STAB, MAS BOOM, MAS TOOL, MAS WHEEL, MAS HITCH, MAS COMM, MAS HOLD, MAS MP, MAS DUMP, MAS POWER, MAS REFINERY, MAS BATTERY, MAS DOORS, MAS TURRETS, MAS WELDERS, MAS LIGHTS, MAS SEISMIC, MAS CLAMP, MAS DECOY, MAS AIRLOCK, MAS LIFE, MAS SCUTTLE, MAS BP, MAS PROJECTOR
// IGC Channels: RIG.C2.HEARTBEAT, RIG.C2.HANDOVER, TMS.MAS.STATUS, TMS.MULE.CALL, TMS.CAT.CALL, TMS.SCOUT.VEIN, FLT.MAS.CMD
// Rule C4: Native blocks provide safety; PB provides decision logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region State Constants & String Tables (Minifier-Safe Zero-GC)
        public const int ROLE_STANDALONE = 0;
        public const int ROLE_MOTHERSHIP = 1;
        public const int ROLE_WORKER = 2;
        public const int ROLE_FIELD_LEADER = 3;

        public const int STATE_TRANSIT = 0;
        public const int STATE_ANCHORING = 1;
        public const int STATE_EXCAVATING = 2;
        public const int STATE_RETRACTING = 3;

        public const int MACRO_NONE = 0;
        public const int MACRO_BENCH = 1;
        public const int MACRO_BORE = 2;
        public const int MACRO_TRENCH = 3;
        public const int MACRO_CRANE = 4;

        public const int POWER_MODE_SILENT = 0;
        public const int POWER_MODE_CRUISE = 1;
        public const int POWER_MODE_EXCAVATE = 2;
        public const int POWER_MODE_EMERGENCY = 3;
        public const int POWER_MODE_SHORE = 4;

        static readonly string[] RoleNames = new string[]
        {
            "STANDALONE",
            "MOTHERSHIP_ROOT",
            "WORKER_SLAVE",
            "FIELD_LEADER"
        };

        static readonly string[] StateNames = new string[]
        {
            "TRANSIT",
            "ANCHORING",
            "EXCAVATING",
            "RETRACTING"
        };

        static readonly string[] MacroNames = new string[]
        {
            "MANUAL_CTC",
            "BENCH_QUARRY",
            "VERTICAL_COLUMN_BORE",
            "TRENCH_CARVE",
            "CRANE_RECOVERY"
        };

        static readonly string[] PowerModeNames = new string[]
        {
            "SILENT_WATCHKEEPING",
            "TRACTIVE_CRUISE",
            "HEAVY_EXCAVATION",
            "EMERGENCY_FAILOVER",
            "SHORE_RECHARGE"
        };
        #endregion

        #region Telemetry & Cache
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(512);
        readonly StringBuilder _sbIgc = new StringBuilder(256);
        readonly List<string> _args = new List<string>(8);
        readonly HeartbeatWatchdog _c2Watchdog = new HeartbeatWatchdog(10.0);

        int _currentRole = ROLE_MOTHERSHIP; // Default genesis seed role
        int _currentState = STATE_TRANSIT;
        int _activeMacro = MACRO_NONE;
        int _currentPowerMode = POWER_MODE_CRUISE;
        int _tickCounter;
        bool _outriggersLocked;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            Echo("Excavator.Mastodon Initialized.");
        }

        public void Save()
        {
            // Save state to Storage
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                HandleCommand(argument);
                return;
            }

            if ((updateSource & UpdateType.Update1) != 0)
            {
                UpdateKinematicsTick();
            }

            if ((updateSource & UpdateType.Update100) != 0)
            {
                UpdateC2FleetTick();
            }
        }

        void UpdateKinematicsTick()
        {
            _tickCounter++;

            // Zero-GC periodic status echo
            if ((_tickCounter & 63) == 0)
            {
                _sbEcho.Clear();
                _sbEcho.Append("MASTODON-01 MKI · RIG//OS-LITE\n");
                _sbEcho.Append("Role: ").Append(RoleNames[_currentRole]).Append('\n');
                _sbEcho.Append("State: ").Append(StateNames[_currentState]).Append('\n');
                _sbEcho.Append("Macro: ").Append(MacroNames[_activeMacro]).Append('\n');
                _sbEcho.Append("Outriggers: ").Append(_outriggersLocked ? "BEDROCK LOCKED" : "STOWED").Append('\n');
                _sbEcho.Append("Power: HYBRID 18 MWh (35 MW + 72 MW Peak) · ").Append(PowerModeNames[_currentPowerMode]).Append('\n');
                _sbEcho.Append("Tick: ");
                SbFormat.AppendInt(_sbEcho, _tickCounter);
                Echo(_sbEcho.ToString());
            }
        }

        void UpdateC2FleetTick()
        {
            _c2Watchdog.Tick(1.666); // ≈ 100 ticks at 60 Hz

            // In Mothership role, emit C2 beacon heartbeat
            if (_currentRole == ROLE_MOTHERSHIP)
            {
                _sbIgc.Clear();
                _sbIgc.Append("MAS-01;ROLE=MOTHERSHIP;STATUS=ACTIVE");
                IGC.SendBroadcastMessage("RIG.C2.HEARTBEAT", _sbIgc.ToString());
            }
        }

        void HandleCommand(string argument)
        {
            IgcProtocol.Split(argument, _args);
            if (_args.Count == 0) return;

            string cmd = _args[0];

            if (string.Compare(cmd, "PING", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _c2Watchdog.Ping();
            }
            else if (string.Compare(cmd, "STABILIZE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentState = STATE_ANCHORING;
                _outriggersLocked = true;
            }
            else if (string.Compare(cmd, "UNSTABILIZE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentState = STATE_RETRACTING;
                _outriggersLocked = false;
                _activeMacro = MACRO_NONE;
            }
            else if (string.Compare(cmd, "MACRO", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                string macro = _args[1];
                if (string.Compare(macro, "BENCH", StringComparison.OrdinalIgnoreCase) == 0)
                    _activeMacro = MACRO_BENCH;
                else if (string.Compare(macro, "BORE", StringComparison.OrdinalIgnoreCase) == 0)
                    _activeMacro = MACRO_BORE;
                else if (string.Compare(macro, "TRENCH", StringComparison.OrdinalIgnoreCase) == 0)
                    _activeMacro = MACRO_TRENCH;
                else if (string.Compare(macro, "CRANE", StringComparison.OrdinalIgnoreCase) == 0)
                    _activeMacro = MACRO_CRANE;
                else
                    _activeMacro = MACRO_NONE;
            }
            else if (string.Compare(cmd, "CRANE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _activeMacro = MACRO_CRANE;
            }
            else if (string.Compare(cmd, "SEISMIC", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger acoustic subsurface survey pulse
            }
            else if (string.Compare(cmd, "LIGHTS", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Toggle worksite floodlight matrix
            }
            else if (string.Compare(cmd, "STOW", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Slew turntable to 180 deg aft (0 deg rel Z), fold shoulder/elbow/wrist to 0 deg, engage MAS Clamp Boom Stow ([X0, Z -7, L4])
                _activeMacro = MACRO_NONE;
                _currentState = STATE_TRANSIT;
            }
            else if (string.Compare(cmd, "DEPLOY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Disengage MAS Clamp Boom Stow, elevate shoulder and elbow to neutral ready state
                _currentState = STATE_TRANSIT;
                _activeMacro = MACRO_NONE;
            }
            else if (string.Compare(cmd, "BORE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Target-centric vertical column bore: maintains [0, -1, 0] downward lock across 6m-35m envelope
                _activeMacro = MACRO_BORE;
            }
            else if (string.Compare(cmd, "LEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger differential outrigger auto-leveling
                _currentState = STATE_ANCHORING;
                _outriggersLocked = true;
            }
            else if (string.Compare(cmd, "LADDER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Toggle crew boarding ladder
            }
            else if (string.Compare(cmd, "PURGE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger stone tailings ejector purge
            }
            else if (string.Compare(cmd, "POWER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Query or toggle Tri-Fuel & Hybrid 18 MWh battery power mode
                if (_args.Count > 1)
                {
                    string sub = _args[1];
                    if (string.Compare(sub, "MODE", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 2)
                    {
                        string modeArg = _args[2];
                        if (modeArg == "1" || string.Compare(modeArg, "SILENT", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(modeArg, "ECO", StringComparison.OrdinalIgnoreCase) == 0)
                            _currentPowerMode = POWER_MODE_SILENT;
                        else if (modeArg == "2" || string.Compare(modeArg, "CRUISE", StringComparison.OrdinalIgnoreCase) == 0)
                            _currentPowerMode = POWER_MODE_CRUISE;
                        else if (modeArg == "3" || string.Compare(modeArg, "EXCAVATE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(modeArg, "HEAVY", StringComparison.OrdinalIgnoreCase) == 0)
                            _currentPowerMode = POWER_MODE_EXCAVATE;
                        else if (modeArg == "4" || string.Compare(modeArg, "EMERGENCY", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(modeArg, "FAILOVER", StringComparison.OrdinalIgnoreCase) == 0)
                            _currentPowerMode = POWER_MODE_EMERGENCY;
                        else if (modeArg == "5" || string.Compare(modeArg, "SHORE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(modeArg, "RECHARGE", StringComparison.OrdinalIgnoreCase) == 0)
                            _currentPowerMode = POWER_MODE_SHORE;
                    }
                }
            }
            else if (string.Compare(cmd, "REFINERY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Toggle or check MAS Field Refinery in-situ metallurgy state
            }
            else if (string.Compare(cmd, "DEFENSE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "COMBAT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Toggle or trigger 360-degree CIWS perimeter defense & combat mode
            }
            else if (string.Compare(cmd, "DECOUPLE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "SEVER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger pyrotechnic drawbar severance (MAS Hitch Sever Tongue merge block disengage)
            }
            else if (string.Compare(cmd, "SCUTTLE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger 60 s anti-capture scuttle countdown loop via MAS Timer Scuttle
            }
            else if (string.Compare(cmd, "AIRLOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger active gas-reclamation decompression cycle on designated airlock
            }
            else if (string.Compare(cmd, "LIFE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Query Universal Life Support (ULSS) O2 tank reserves and multi-zone hermetic status
            }
            else if (string.Compare(cmd, "HANDOVER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Yield C2 root to Ground Base
                if (_currentRole == ROLE_MOTHERSHIP)
                {
                    _currentRole = ROLE_FIELD_LEADER;
                    _sbIgc.Clear();
                    _sbIgc.Append("MAS-01;GND-01;ACK");
                    IGC.SendBroadcastMessage("RIG.C2.HANDOVER", _sbIgc.ToString());
                }
            }
            else if (string.Compare(cmd, "ROLE", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                string r = _args[1];
                if (string.Compare(r, "MOTHERSHIP", StringComparison.OrdinalIgnoreCase) == 0)
                    _currentRole = ROLE_MOTHERSHIP;
                else if (string.Compare(r, "WORKER", StringComparison.OrdinalIgnoreCase) == 0)
                    _currentRole = ROLE_WORKER;
                else if (string.Compare(r, "STANDALONE", StringComparison.OrdinalIgnoreCase) == 0)
                    _currentRole = ROLE_STANDALONE;
            }
            else if (string.Compare(cmd, "RELOAD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                LoadConfiguration();
                InitHardware();
            }
        }

        void LoadConfiguration()
        {
            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                _currentRole = _ini.Get("RIG_OS", "Role").ToInt32(_currentRole);
            }
        }

        void InitHardware()
        {
            // Grid-scoped block caching: b => b.CubeGrid == Me.CubeGrid
        }
    }
}
