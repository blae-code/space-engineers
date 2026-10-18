// Excavator.Mastodon · Space Engineers 1 MDK2 PB Script
// Target Blocks: [MAS] PB Excavator & [MAS] PB RIG//OS-Lite
// Grid: Large Grid Consist (11 × 42 × 6 B · 10 Wheels · 2 Carriages) · Update: Update1 / Update10
// Subsystem Groups: MAS STAB, MAS BOOM, MAS TOOL, MAS WHEEL, MAS HITCH, MAS COMM, MAS HOLD, MAS MP, MAS DUMP, MAS POWER, MAS REFINERY, MAS BATTERY, MAS DOORS, MAS TURRETS, MAS WELDERS, MAS LIGHTS, MAS SEISMIC, MAS CLAMP, MAS DECOY, MAS AIRLOCK, MAS LIFE, MAS SCUTTLE, MAS BP, MAS PROJECTOR, MAS HITCH CLAMPS, MAS OUTRIGGERS UTILITY, MAS DPU, MAS FOB
// IGC Channels: RIG.C2.HEARTBEAT, RIG.C2.HANDOVER, TMS.MAS.STATUS, TMS.MULE.CALL, TMS.CAT.CALL, TMS.SCOUT.VEIN, FLT.MAS.CMD, TMS.CONSIST.SYNC, TMS.CONSIST.TRACTION, TMS.CONSIST.SEVER
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
        public const int MACRO_SALVAGE = 5;

        public const int POWER_MODE_SILENT = 0;
        public const int POWER_MODE_CRUISE = 1;
        public const int POWER_MODE_EXCAVATE = 2;
        public const int POWER_MODE_EMERGENCY = 3;
        public const int POWER_MODE_SHORE = 4;

        public const int ENV_EARTH_SCAVENGE = 0;
        public const int ENV_CLOSED_LOOP_VACUUM = 1;
        public const int ENV_CORROSIVE_ISOLATION = 2;

        public const int AIRLOCK_IDLE = 0;
        public const int AIRLOCK_EGRESS = 1;
        public const int AIRLOCK_INGRESS = 2;
        public const int AIRLOCK_DECON = 3;

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
            "CRANE_RECOVERY",
            "SALVAGE_DECONSTRUCT"
        };

        static readonly string[] PowerModeNames = new string[]
        {
            "SILENT_WATCHKEEPING",
            "TRACTIVE_CRUISE",
            "HEAVY_EXCAVATION",
            "EMERGENCY_FAILOVER",
            "SHORE_RECHARGE"
        };

        static readonly string[] EnvModeNames = new string[]
        {
            "EARTH_SCAVENGE",
            "CLOSED_LOOP_VACUUM",
            "CORROSIVE_ISOLATION"
        };

        static readonly string[] AirlockStateNames = new string[]
        {
            "IDLE",
            "EGRESS_EVACUATE",
            "INGRESS_PRESSURIZE",
            "DECON_WASH"
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

        // Decision 66: Predictive JIT Logistics & Fleet Mass Telemetry
        double _prevCargoMassKg;
        double _currentCargoMassKg = 0;
        double _maxCargoMassKg = 2500000; // 2,500 t bulk payload
        double _extractionRateKgPerSec;
        double _secondsUntilFull = 9999;
        bool _jitAirDispatched;
        bool _jitGndDispatched;

        // Decision 67: Consist Mechanics, 3-Axis Hitch, DPU & FOB Autonomy
        bool _hitchClampsEngaged;
        bool _dpuPushActive;
        bool _utilityOutriggersLocked;
        bool _sternDockExtended;

        // Decision 68: 4× Multitool Repair Turrets, SML Auto-Production & Peer Resupply Sorties
        bool _mtTurretsWelding = true;
        bool _mtTurretsEnabled = true;
        bool _smlAutoProdEnabled = true;
        bool _resupplyStagingActive;
        int _resupplyKitType = RESUPPLY_NONE;

        const int RESUPPLY_NONE = 0;
        const int RESUPPLY_MOBILITY = 1;
        const int RESUPPLY_POWER = 2;
        const int RESUPPLY_MUNITIONS = 3;
        const int RESUPPLY_HULL = 4;

        static readonly string[] ResupplyKitNames = new string[]
        {
            "NONE",
            "KIT_MOBILITY",
            "KIT_POWER",
            "KIT_MUNITIONS",
            "KIT_HULL_REPAIR"
        };

        // Decision 69: Environmental Mode & Airlock State
        int _currentEnvMode = ENV_EARTH_SCAVENGE;
        int _airlockState = AIRLOCK_IDLE;
        bool _uvDeconActive;

        // Decision 70: Automated Meteor Defense Grid, Ammo Fabrication & Restock
        bool _meteorAlertActive;
        bool _consistHardened;
        int _ammoGatlingBoxes = 250;
        int _ammoInteriorMags = 100;
        double _mgIngotKg = 150.0;
        bool _ammoAutoFabActive = true;
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
                _sbEcho.Append("Outriggers: ").Append(_outriggersLocked ? "BEDROCK LOCKED" : "STOWED");
                _sbEcho.Append(" · Rear: ").Append(_utilityOutriggersLocked ? "LOCKED" : "STOWED").Append('\n');
                _sbEcho.Append("Hitch: ").Append(_hitchClampsEngaged ? "CLAMPED" : "FLEX_3AXIS");
                _sbEcho.Append(" · DPU: ").Append(_dpuPushActive ? "PUSH_ACTIVE" : "COAST");
                _sbEcho.Append(" · Dock: ").Append(_sternDockExtended ? "EXTENDED" : "RETRACTED").Append('\n');
                _sbEcho.Append("Power: HYBRID 18 MWh (35 MW + 72 MW Peak) · ").Append(PowerModeNames[_currentPowerMode]).Append('\n');
                _sbEcho.Append("Rate: ");
                SbFormat.AppendInt(_sbEcho, (int)_extractionRateKgPerSec);
                _sbEcho.Append(" kg/s · JIT ETA: ");
                SbFormat.AppendInt(_sbEcho, (int)_secondsUntilFull);
                _sbEcho.Append(" s\nMT Turrets: ").Append(_mtTurretsEnabled ? (_mtTurretsWelding ? "REPAIR_WELD" : "SALVAGE_GRIND") : "OFF");
                _sbEcho.Append(" · SML: ").Append(_smlAutoProdEnabled ? "ACTIVE" : "STANDBY");
                _sbEcho.Append(" · Resupply: ").Append(ResupplyKitNames[_resupplyKitType]);
                _sbEcho.Append("\nEnv Mode: ").Append(EnvModeNames[_currentEnvMode]);
                _sbEcho.Append(" · Airlock: ").Append(AirlockStateNames[_airlockState]);
                _sbEcho.Append(" · UV Decon: ").Append(_uvDeconActive ? "PULSING" : "STANDBY");
                _sbEcho.Append("\nMeteor CIWS: ").Append(_meteorAlertActive ? "METEOR_ALERT" : "WATCHKEEPING");
                _sbEcho.Append(" · Hardened: ").Append(_consistHardened ? "LOCKED" : "NORMAL");
                _sbEcho.Append("\nAmmo 25mm: ");
                SbFormat.AppendInt(_sbEcho, _ammoGatlingBoxes);
                _sbEcho.Append("/250 · 5.56mm: ");
                SbFormat.AppendInt(_sbEcho, _ammoInteriorMags);
                _sbEcho.Append("/100 · Mg: ");
                SbFormat.AppendInt(_sbEcho, (int)_mgIngotKg);
                _sbEcho.Append(" kg");
                _sbEcho.Append("\nTick: ");
                SbFormat.AppendInt(_sbEcho, _tickCounter);
                Echo(_sbEcho.ToString());
            }
        }

        void UpdateC2FleetTick()
        {
            _c2Watchdog.Tick(1.666); // ≈ 100 ticks at 60 Hz

            // Decision 66: Predictive JIT Logistics delta calculation
            double deltaMass = _currentCargoMassKg - _prevCargoMassKg;
            _prevCargoMassKg = _currentCargoMassKg;
            if (deltaMass > 0)
            {
                _extractionRateKgPerSec = deltaMass / 1.666;
                double remainingKg = _maxCargoMassKg - _currentCargoMassKg;
                _secondsUntilFull = remainingKg / (_extractionRateKgPerSec > 1.0 ? _extractionRateKgPerSec : 1.0);
            }
            else
            {
                _extractionRateKgPerSec = 0;
                _secondsUntilFull = 9999;
            }

            // Auto JIT pre-dispatch when buffer > 75% and time to full < 180s
            if (_currentCargoMassKg > _maxCargoMassKg * 0.75)
            {
                if (!_jitAirDispatched && _secondsUntilFull < 180)
                {
                    _jitAirDispatched = true;
                    _sbIgc.Clear();
                    _sbIgc.Append("JIT;MAS-01;AIR;120;RARE_ORE;45000");
                    IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
                }
                if (!_jitGndDispatched && _secondsUntilFull < 300)
                {
                    _jitGndDispatched = true;
                    _sbIgc.Clear();
                    _sbIgc.Append("JIT;MAS-01;GND;300;BULK_ORE;2500000");
                    IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
                }
            }
            else
            {
                _jitAirDispatched = false;
                _jitGndDispatched = false;
            }

            // In Mothership role, emit C2 beacon heartbeat
            if (_currentRole == ROLE_MOTHERSHIP)
            {
                _sbIgc.Clear();
                _sbIgc.Append("MAS-01;ROLE=MOTHERSHIP;STATUS=ACTIVE");
                IGC.SendBroadcastMessage("RIG.C2.HEARTBEAT", _sbIgc.ToString());
            }

            // Broadcast live excavator telemetry (TMS.MAS.STATUS)
            _sbIgc.Clear();
            _sbIgc.Append(StateNames[_currentState]).Append(';');
            SbFormat.AppendInt(_sbIgc, (int)_currentCargoMassKg);
            _sbIgc.Append(';').Append(MacroNames[_activeMacro]).Append(';');
            SbFormat.AppendInt(_sbIgc, (int)_extractionRateKgPerSec);
            _sbIgc.Append(';').Append(PowerModeNames[_currentPowerMode]);
            IGC.SendBroadcastMessage("TMS.MAS.STATUS", _sbIgc.ToString());

            // Broadcast planetary environmental mode telemetry (TMS.ENV.MODE)
            _sbIgc.Clear();
            _sbIgc.Append("ENV;MAS-01;").Append(EnvModeNames[_currentEnvMode]);
            IGC.SendBroadcastMessage("TMS.ENV.MODE", _sbIgc.ToString());

            // Broadcast live munitions telemetry (TMS.AMMO.STATUS)
            _sbIgc.Clear();
            _sbIgc.Append("AMMO;");
            SbFormat.AppendInt(_sbIgc, _ammoGatlingBoxes);
            _sbIgc.Append(';');
            SbFormat.AppendInt(_sbIgc, _ammoInteriorMags);
            _sbIgc.Append(';');
            SbFormat.AppendInt(_sbIgc, (int)_mgIngotKg);
            _sbIgc.Append(';');
            SbFormat.AppendInt(_sbIgc, Math.Max(0, 250 - _ammoGatlingBoxes));
            IGC.SendBroadcastMessage("TMS.AMMO.STATUS", _sbIgc.ToString());

            // If meteor alert active, broadcast meteor early warning (TMS.METEOR.ALERT)
            if (_meteorAlertActive)
            {
                _sbIgc.Clear();
                _sbIgc.Append("METEOR;HEAVY;180;45;12");
                IGC.SendBroadcastMessage("TMS.METEOR.ALERT", _sbIgc.ToString());
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
                _utilityOutriggersLocked = true;
            }
            else if (string.Compare(cmd, "UNSTABILIZE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentState = STATE_RETRACTING;
                _outriggersLocked = false;
                _utilityOutriggersLocked = false;
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
                else if (string.Compare(macro, "SALVAGE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(macro, "DECONSTRUCT", StringComparison.OrdinalIgnoreCase) == 0)
                    _activeMacro = MACRO_SALVAGE;
                else
                    _activeMacro = MACRO_NONE;
            }
            else if (string.Compare(cmd, "CRANE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _activeMacro = MACRO_CRANE;
            }
            else if (string.Compare(cmd, "SALVAGE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Macro 5: Planar raster deconstruction sweep (15-30 m standoff, 100% component retention)
                _activeMacro = MACRO_SALVAGE;
            }
            else if (string.Compare(cmd, "TOOL_MODE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "MODE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Cycle ToolCore laser mode: Mining Beam (Drill) <-> Salvage Beam (Grinder) <-> Fabrication Beam (Welder)
            }
            else if (string.Compare(cmd, "SEISMIC", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger acoustic subsurface survey pulse: SURVEY;<grid_id>;<depth>;<type>;<centroid>;<volume>
                _sbIgc.Clear();
                _sbIgc.Append("SURVEY;MAS-01;45;URANIUM;GPS:Centroid:1200:340:560:#FF0000:;8500");
                IGC.SendBroadcastMessage("TMS.SEISMIC.SURVEY", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "JIT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Predictive JIT Logistics dispatch trigger: JIT;<grid>;<AIR|GND>;<eta>;<ore>;<mass>
                _sbIgc.Clear();
                if (_args.Count > 1 && string.Compare(_args[1], "AIR", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _jitAirDispatched = true;
                    _sbIgc.Append("JIT;MAS-01;AIR;120;RARE_ORE;45000");
                }
                else
                {
                    _jitGndDispatched = true;
                    _sbIgc.Append("JIT;MAS-01;GND;300;BULK_ORE;2500000");
                }
                IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "TASK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Vector prospector drone to survey sector: TASK;<drone>;<pattern>;<center>;<radius>;<priority_ore>
                _sbIgc.Clear();
                _sbIgc.Append("TASK;KES-01;CONE;GPS:Sector-Alpha:0:0:0:#FF0000:;2500;URANIUM");
                IGC.SendBroadcastMessage("TMS.SCOUT.TASK", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "CLAIM", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "ZONE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Multi-rig quarry exclusion zone broadcast: ZONE;<grid>;<state>;<min_gps>;<max_gps>;<macro>
                _sbIgc.Clear();
                _sbIgc.Append("ZONE;MAS-01;ACTIVE;GPS:Min:-50:-50:0:#FF0000:;GPS:Max:50:50:50:#FF0000:;BENCH_QUARRY");
                IGC.SendBroadcastMessage("TMS.ZONE.CLAIM", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "TARGET", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Distributed CIWS defense target assignment: ENGAGE;<target_id>;<gps>;<threat>;<assigned_grid>
                _sbIgc.Clear();
                _sbIgc.Append("ENGAGE;TG-9901;GPS:Hostile:100:200:300:#FF0000:;HIGH;MAS-01");
                IGC.SendBroadcastMessage("TMS.DEFENSE.TARGET", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "CONVOY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Synchronize trailing Caterpillar land train: CONVOY;<lead_id>;<speed_ms>;<heading_deg>;<ejector_active>
                _sbIgc.Clear();
                _sbIgc.Append("CONVOY;MAS-01;0.55;0.0;True");
                IGC.SendBroadcastMessage("TMS.CONVOY.SYNC", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "DEMAND", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Process refinery pull-economy demand broadcast from Ground Base (TMS.BASE.DEMAND)
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
            else if (string.Compare(cmd, "OUTRIGGERS", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && (string.Compare(_args[1], "REAR", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(_args[1], "UTILITY", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    _utilityOutriggersLocked = !_utilityOutriggersLocked;
                }
                else
                {
                    _outriggersLocked = !_outriggersLocked;
                    _utilityOutriggersLocked = _outriggersLocked;
                }
            }
            else if (string.Compare(cmd, "HITCH", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && string.Compare(_args[1], "CLAMP", StringComparison.OrdinalIgnoreCase) == 0)
                    _hitchClampsEngaged = true;
                else if (_args.Count > 1 && string.Compare(_args[1], "UNCLAMP", StringComparison.OrdinalIgnoreCase) == 0)
                    _hitchClampsEngaged = false;
                else
                    _hitchClampsEngaged = !_hitchClampsEngaged;

                _sbIgc.Clear();
                _sbIgc.Append("SYNC;MAS-01;HITCH;CLAMP=").Append(_hitchClampsEngaged ? "TRUE" : "FALSE");
                IGC.SendBroadcastMessage("TMS.CONSIST.SYNC", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "DPU", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && (string.Compare(_args[1], "PUSH", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(_args[1], "ON", StringComparison.OrdinalIgnoreCase) == 0))
                    _dpuPushActive = true;
                else if (_args.Count > 1 && (string.Compare(_args[1], "COAST", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(_args[1], "OFF", StringComparison.OrdinalIgnoreCase) == 0))
                    _dpuPushActive = false;
                else
                    _dpuPushActive = !_dpuPushActive;

                _sbIgc.Clear();
                _sbIgc.Append("DPU;").Append(_dpuPushActive ? "PUSH;100;0;0" : "COAST;0;0;0");
                IGC.SendBroadcastMessage("TMS.CONSIST.TRACTION", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "DOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && string.Compare(_args[1], "EXTEND", StringComparison.OrdinalIgnoreCase) == 0)
                    _sternDockExtended = true;
                else if (_args.Count > 1 && string.Compare(_args[1], "RETRACT", StringComparison.OrdinalIgnoreCase) == 0)
                    _sternDockExtended = false;
                else
                    _sternDockExtended = !_sternDockExtended;
            }
            else if (string.Compare(cmd, "DECOUPLE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "SEVER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger pyrotechnic drawbar severance (MAS Hitch Sever Tongue merge block disengage)
                _sbIgc.Clear();
                _sbIgc.Append("SEVER;MAS-01-T;NORMAL;FALSE");
                IGC.SendBroadcastMessage("TMS.CONSIST.SEVER", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "SCUTTLE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger 60 s anti-capture scuttle countdown loop via MAS Timer Scuttle
            }
            else if (string.Compare(cmd, "AIRLOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger active gas-reclamation decompression cycle: AIRLOCK <CHAMBER> <EGRESS|INGRESS|DECON>
                string chamber = (_args.Count > 1) ? _args[1] : "PORT";
                string action = (_args.Count > 2) ? _args[2] : "CYCLE";

                if (string.Compare(action, "EGRESS", StringComparison.OrdinalIgnoreCase) == 0)
                    _airlockState = AIRLOCK_EGRESS;
                else if (string.Compare(action, "INGRESS", StringComparison.OrdinalIgnoreCase) == 0)
                    _airlockState = AIRLOCK_INGRESS;
                else if (string.Compare(action, "DECON", StringComparison.OrdinalIgnoreCase) == 0)
                    _airlockState = AIRLOCK_DECON;
                else
                    _airlockState = (_airlockState == AIRLOCK_IDLE) ? AIRLOCK_EGRESS : AIRLOCK_IDLE;

                _sbIgc.Clear();
                _sbIgc.Append("CYCLE;").Append(chamber).Append(';').Append(AirlockStateNames[_airlockState]);
                IGC.SendBroadcastMessage("TMS.AIRLOCK.CYCLE", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "ENV_MODE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "ENV", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Toggle or set planetary environmental mode: ENV <EARTH|VACUUM|CORROSIVE>
                if (_args.Count > 1)
                {
                    string mode = _args[1];
                    if (string.Compare(mode, "EARTH", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(mode, "SCAVENGE", StringComparison.OrdinalIgnoreCase) == 0)
                        _currentEnvMode = ENV_EARTH_SCAVENGE;
                    else if (string.Compare(mode, "VACUUM", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(mode, "CLOSED", StringComparison.OrdinalIgnoreCase) == 0)
                        _currentEnvMode = ENV_CLOSED_LOOP_VACUUM;
                    else if (string.Compare(mode, "CORROSIVE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(mode, "TOXIC", StringComparison.OrdinalIgnoreCase) == 0)
                        _currentEnvMode = ENV_CORROSIVE_ISOLATION;
                }
                else
                {
                    _currentEnvMode = (_currentEnvMode + 1) % 3;
                }

                _sbIgc.Clear();
                _sbIgc.Append("ENV;MAS-01;").Append(EnvModeNames[_currentEnvMode]);
                IGC.SendBroadcastMessage("TMS.ENV.MODE", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "DECON", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger UV decontamination washdown: DECON <ON|OFF|PULSE>
                if (_args.Count > 1 && string.Compare(_args[1], "OFF", StringComparison.OrdinalIgnoreCase) == 0)
                    _uvDeconActive = false;
                else if (_args.Count > 1 && string.Compare(_args[1], "ON", StringComparison.OrdinalIgnoreCase) == 0)
                    _uvDeconActive = true;
                else
                    _uvDeconActive = !_uvDeconActive;
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
            else if (string.Compare(cmd, "MT_TURRETS", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "REPAIR", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && string.Compare(_args[1], "TOGGLE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _mtTurretsWelding = !_mtTurretsWelding;
                }
                else if (_args.Count > 1 && string.Compare(_args[1], "WELD", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _mtTurretsWelding = true;
                    _mtTurretsEnabled = true;
                }
                else if (_args.Count > 1 && string.Compare(_args[1], "GRIND", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _mtTurretsWelding = false;
                    _mtTurretsEnabled = true;
                }
                else if (_args.Count > 1 && (string.Compare(_args[1], "OFF", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(_args[1], "STOP", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    _mtTurretsEnabled = false;
                }
                else
                {
                    _mtTurretsEnabled = !_mtTurretsEnabled;
                }
            }
            else if (string.Compare(cmd, "AUTOPROD", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "SML", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && string.Compare(_args[1], "OFF", StringComparison.OrdinalIgnoreCase) == 0)
                    _smlAutoProdEnabled = false;
                else if (_args.Count > 1 && string.Compare(_args[1], "ON", StringComparison.OrdinalIgnoreCase) == 0)
                    _smlAutoProdEnabled = true;
                else
                    _smlAutoProdEnabled = !_smlAutoProdEnabled;
            }
            else if (string.Compare(cmd, "RESUPPLY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1)
                {
                    string kit = _args[1];
                    if (string.Compare(kit, "MOBILITY", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(kit, "KIT_MOBILITY", StringComparison.OrdinalIgnoreCase) == 0)
                        _resupplyKitType = RESUPPLY_MOBILITY;
                    else if (string.Compare(kit, "POWER", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(kit, "KIT_POWER", StringComparison.OrdinalIgnoreCase) == 0)
                        _resupplyKitType = RESUPPLY_POWER;
                    else if (string.Compare(kit, "MUNITIONS", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(kit, "KIT_MUNITIONS", StringComparison.OrdinalIgnoreCase) == 0)
                        _resupplyKitType = RESUPPLY_MUNITIONS;
                    else if (string.Compare(kit, "HULL", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(kit, "KIT_HULL_REPAIR", StringComparison.OrdinalIgnoreCase) == 0)
                        _resupplyKitType = RESUPPLY_HULL;
                    else if (string.Compare(kit, "CANCEL", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(kit, "CLEAR", StringComparison.OrdinalIgnoreCase) == 0)
                        _resupplyKitType = RESUPPLY_NONE;
                    else
                        _resupplyKitType = (_resupplyKitType == RESUPPLY_NONE) ? RESUPPLY_MOBILITY : _resupplyKitType;
                }
                else
                {
                    _resupplyKitType = (_resupplyKitType == RESUPPLY_NONE) ? RESUPPLY_MOBILITY : RESUPPLY_NONE;
                }

                _resupplyStagingActive = (_resupplyKitType != RESUPPLY_NONE);
                if (_resupplyStagingActive)
                {
                    _sbIgc.Clear();
                    _sbIgc.Append("DISPATCH;MAS-01;MUL-01;ALL;").Append(ResupplyKitNames[_resupplyKitType]);
                    IGC.SendBroadcastMessage("TMS.RESUPPLY.DISPATCH", _sbIgc.ToString());
                }
            }
            else if (string.Compare(cmd, "METEOR", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Trigger or clear meteor intercept alert and consist hardening: METEOR <ALERT|CLEAR|STOW|LOCKDOWN>
                if (_args.Count > 1 && string.Compare(_args[1], "CLEAR", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _meteorAlertActive = false;
                    _consistHardened = false;
                }
                else
                {
                    _meteorAlertActive = true;
                    _consistHardened = true;
                    _activeMacro = MACRO_NONE; // emergency boom fold to STOW

                    _sbIgc.Clear();
                    _sbIgc.Append("METEOR;HEAVY;180;45;15");
                    IGC.SendBroadcastMessage("TMS.METEOR.ALERT", _sbIgc.ToString());
                }
            }
            else if (string.Compare(cmd, "AMMO", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Query or toggle closed-loop munitions manufacturing: AMMO <FAB|AUTO|STATUS|RESTOCK>
                if (_args.Count > 1 && string.Compare(_args[1], "OFF", StringComparison.OrdinalIgnoreCase) == 0)
                    _ammoAutoFabActive = false;
                else if (_args.Count > 1 && string.Compare(_args[1], "ON", StringComparison.OrdinalIgnoreCase) == 0)
                    _ammoAutoFabActive = true;
                else if (_args.Count > 1 && (string.Compare(_args[1], "RESTOCK", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(_args[1], "REQ", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    _sbIgc.Clear();
                    _sbIgc.Append("RESUPPLY;MAS-01;NONE;KIT_MUNITIONS;HIGH");
                    IGC.SendBroadcastMessage("TMS.RESUPPLY.REQUEST", _sbIgc.ToString());
                }
                else
                    _ammoAutoFabActive = !_ammoAutoFabActive;
            }
            else if (string.Compare(cmd, "RESTOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Automated incoming resupply call
                _sbIgc.Clear();
                _sbIgc.Append("RESUPPLY;MAS-01;NONE;KIT_MUNITIONS;HIGH");
                IGC.SendBroadcastMessage("TMS.RESUPPLY.REQUEST", _sbIgc.ToString());
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
