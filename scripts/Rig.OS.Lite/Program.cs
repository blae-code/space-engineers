// Rig.OS.Lite · Space Engineers 1 MDK2 PB Script
// Target Block: [MAS] PB RIG//OS-Lite
// Grid: Large Grid Tractor Unit (MAS-01-T) · Update: Update10 / Update100
// Subsystem Groups: MAS COMM, MAS HOLD, MAS TURRETS, MAS LIGHTS, MAS SCREENS
// IGC Channels: RIG.C2.HEARTBEAT, RIG.C2.HANDOVER, TMS.LOGISTICS.JIT, TMS.MULE.CALL, TMS.CAT.CALL, TMS.SCOUT.TASK, TMS.SCOUT.VEIN, TMS.BASE.DEMAND, TMS.DEFENSE.TARGET, TMS.METEOR.ALERT
// Rule C4: Native blocks provide safety; PB provides decision & C2 logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region Role & State Constants (Minifier-Safe Zero-GC)
        public const int ROLE_STANDALONE = 0;
        public const int ROLE_MOTHERSHIP = 1;
        public const int ROLE_WORKER = 2;
        public const int ROLE_FIELD_LEADER = 3;

        public const int ASSET_NONE = 0;
        public const int ASSET_SCOUT = 1;      // Kestrel
        public const int ASSET_SHUTTLE = 2;    // Mule
        public const int ASSET_TRAIN = 3;      // Caterpillar
        public const int ASSET_EXCAVATOR = 4;  // Sister Mastodon

        static readonly string[] RoleNames = new string[]
        {
            "STANDALONE",
            "MOTHERSHIP_ROOT",
            "REGIONAL_WORKER",
            "FIELD_LEADER"
        };

        static readonly string[] AssetTypeNames = new string[]
        {
            "NONE",
            "SCOUT",
            "SHUTTLE",
            "TRAIN",
            "EXCAVATOR"
        };
        #endregion

        #region Fleet Asset Registry Record
        public struct AssetRecord
        {
            public bool Active;
            public string Id;
            public int Type;
            public string Status;
            public double OreMassKg;
            public double GrossMassKg;
            public double LastSeenSec;
            public bool Stale;
        }

        const int MAX_ASSETS = 16;
        readonly AssetRecord[] _assets = new AssetRecord[MAX_ASSETS];
        int _assetCount = 0;
        #endregion

        #region Predictive JIT Logistics State
        double _prevCargoMassKg;
        double _currentCargoMassKg;
        double _maxCargoCapacityKg = 2500000.0; // 2,500 t bulk payload
        double _extractionRateKgSec;
        double _secondsUntilFull = 9999.0;
        bool _jitAirDispatched;
        bool _jitGndDispatched;
        double _rareOreRatio = 0.0; // Percentage of rare minerals in buffer
        #endregion

        #region Hardware References & Display Viewports
        IMyTextPanel _lcdHelm;
        IMyTextPanel _lcdBoom;
        IMyTextPanel _lcdFleet;

        readonly List<IMyCargoContainer> _cargoHolds = new List<IMyCargoContainer>(4);
        readonly List<IMyLargeTurretBase> _ciwsTurrets = new List<IMyLargeTurretBase>(4);
        IMyRadioAntenna _commAntenna;
        IMyFunctionalBlock _alarmMeteor;
        #endregion

        #region Internal Telemetry & Buffers
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(512);
        readonly StringBuilder _sbIgc = new StringBuilder(256);
        readonly StringBuilder _sbLcd = new StringBuilder(1024);
        readonly List<string> _args = new List<string>(8);
        readonly HeartbeatWatchdog _c2Watchdog = new HeartbeatWatchdog(10.0);

        int _currentRole = ROLE_MOTHERSHIP; // Starts as Genesis Seed root
        int _tickCounter;
        bool _meteorAlertActive;
        bool _defenseLockActive;
        string _kinematicsState = "TRANSIT";
        string _kinematicsMacro = "TRAVEL_STOW";
        float _vehicleSpeedKmh;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            InitHardware();
            LoadConfiguration();
            InitRegistry();
            Echo("Rig.OS.Lite Initialized.");
        }

        public void Save()
        {
            // Persist registry state
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                HandleCommand(argument);
                return;
            }

            if ((updateSource & UpdateType.Update10) != 0)
            {
                UpdateFleetBusTick();
            }

            if ((updateSource & UpdateType.Update100) != 0)
            {
                UpdateTelemetryScreensTick();
            }

            // Inbound IGC message processing
            ProcessInboundIgcMessages();
        }

        #region Fleet Bus & Logic Loop (10 Hz)
        void UpdateFleetBusTick()
        {
            _tickCounter++;
            _c2Watchdog.Tick(0.166);

            // 1. Update Asset Liveness & Stale Flagging
            for (int i = 0; i < MAX_ASSETS; i++)
            {
                if (_assets[i].Active)
                {
                    _assets[i].LastSeenSec += 0.166;
                    if (_assets[i].LastSeenSec > 30.0)
                    {
                        _assets[i].Stale = true;
                    }
                }
            }

            // 2. Measure Local Cargo Mass & Extraction Rate (dM/dt)
            MeasureCargoBuffer();

            // 3. Predictive JIT Logistics Auto-Dispatch
            CheckJitDispatchThresholds();

            // 4. Genesis Root Heartbeat Broadcast
            if ((_tickCounter % 30) == 0 && _currentRole == ROLE_MOTHERSHIP)
            {
                BroadcastFleetHeartbeat();
            }

            // 5. Tactical CIWS Threat Coordination
            MonitorTacticalPerimeter();
        }

        void MeasureCargoBuffer()
        {
            double totalMass = 0.0;
            for (int i = 0; i < _cargoHolds.Count; i++)
            {
                if (_cargoHolds[i] != null && _cargoHolds[i].IsWorking)
                {
                    totalMass += (double)_cargoHolds[i].GetInventory().CurrentMass;
                }
            }

            double deltaMass = totalMass - _prevCargoMassKg;
            _prevCargoMassKg = totalMass;
            _currentCargoMassKg = totalMass;

            if (deltaMass > 0.0)
            {
                // Instantaneous rate over 0.166 s
                double instantRate = deltaMass / 0.166;
                _extractionRateKgSec = (_extractionRateKgSec * 0.9) + (instantRate * 0.1); // Smooth low-pass filter

                double remainingCapKg = Math.Max(0.0, _maxCargoCapacityKg - _currentCargoMassKg);
                _secondsUntilFull = (_extractionRateKgSec > 1.0) ? (remainingCapKg / _extractionRateKgSec) : 9999.0;
            }
            else
            {
                _extractionRateKgSec = Math.Max(0.0, _extractionRateKgSec * 0.95);
            }
        }

        void CheckJitDispatchThresholds()
        {
            double fillRatio = _currentCargoMassKg / _maxCargoCapacityKg;

            // Trigger JIT Transport pre-dispatch at 75-80% capacity
            if (fillRatio >= 0.75 && !_jitGndDispatched && !_jitAirDispatched)
            {
                if (_rareOreRatio > 0.20)
                {
                    // High-value rare ore: pre-dispatch MULE-01 to top-deck gantry
                    _jitAirDispatched = true;
                    _sbIgc.Clear();
                    _sbIgc.Append("JIT;MAS-01;AIR;");
                    SbFormat.AppendInt(_sbIgc, (int)_secondsUntilFull);
                    _sbIgc.Append(";RARE_ORE;45000");
                    IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
                }
                else
                {
                    // Bulk ore: pre-dispatch CATERPILLAR-01 to stern bumper
                    _jitGndDispatched = true;
                    _sbIgc.Clear();
                    _sbIgc.Append("JIT;MAS-01;GND;");
                    SbFormat.AppendInt(_sbIgc, (int)_secondsUntilFull);
                    _sbIgc.Append(";BULK_ORE;2500000");
                    IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
                }
            }
            else if (fillRatio < 0.20)
            {
                // Reset dispatch flags after hauler offload
                _jitAirDispatched = false;
                _jitGndDispatched = false;
            }
        }

        void MonitorTacticalPerimeter()
        {
            bool hostileDetected = false;
            for (int i = 0; i < _ciwsTurrets.Count; i++)
            {
                if (_ciwsTurrets[i] != null && _ciwsTurrets[i].HasTarget)
                {
                    hostileDetected = true;
                    break;
                }
            }

            if (hostileDetected && !_defenseLockActive)
            {
                _defenseLockActive = true;
                _sbIgc.Clear();
                _sbIgc.Append("DEFENSE;HOSTILE_ENGAGE;MAS-01;HIGH");
                IGC.SendBroadcastMessage("TMS.DEFENSE.TARGET", _sbIgc.ToString());
            }
            else if (!hostileDetected && _defenseLockActive)
            {
                _defenseLockActive = false;
            }
        }

        void BroadcastFleetHeartbeat()
        {
            _sbIgc.Clear();
            _sbIgc.Append("HEARTBEAT;MAS-01;");
            _sbIgc.Append(RoleNames[_currentRole]).Append(';');
            SbFormat.AppendInt(_sbIgc, (int)_currentCargoMassKg);
            _sbIgc.Append(';');
            SbFormat.AppendInt(_sbIgc, _assetCount);

            IGC.SendBroadcastMessage("RIG.C2.HEARTBEAT", _sbIgc.ToString());
        }
        #endregion

        #region Telemetry & Screen Rendering (1 Hz Loop)
        void UpdateTelemetryScreensTick()
        {
            RenderHelmScreen();
            RenderBoomScreen();
            RenderFleetScreen();

            // Terminal status echo
            _sbEcho.Clear();
            _sbEcho.Append("RIG//OS-LITE · FLEET MOTHERSHIP C2\n");
            _sbEcho.Append("Role: ").Append(RoleNames[_currentRole]).Append('\n');
            _sbEcho.Append("Rate: ");
            SbFormat.AppendInt(_sbEcho, (int)_extractionRateKgSec);
            _sbEcho.Append(" kg/s · JIT ETA: ");
            SbFormat.AppendInt(_sbEcho, (int)_secondsUntilFull);
            _sbEcho.Append(" s\nTracked Drones: ");
            SbFormat.AppendInt(_sbEcho, _assetCount);
            _sbEcho.Append(" · CIWS: ").Append(_defenseLockActive ? "ENGAGING" : "STANDBY");
            Echo(_sbEcho.ToString());
        }

        void RenderHelmScreen()
        {
            if (_lcdHelm == null) return;

            _sbLcd.Clear();
            _sbLcd.Append("╔══════════════════════════════════════════╗\n");
            _sbLcd.Append("║ MASTODON-01 MKI · HELM & POWERTRAIN HUD  ║\n");
            _sbLcd.Append("╠══════════════════════════════════════════╣\n");
            _sbLcd.Append("║ Speed:        ");
            SbFormat.AppendDouble(_sbLcd, _vehicleSpeedKmh, 1);
            _sbLcd.Append(" km/h\n");
            _sbLcd.Append("║ Consist State: ").Append(_kinematicsState).Append('\n');
            _sbLcd.Append("║ Active Macro:  ").Append(_kinematicsMacro).Append('\n');
            _sbLcd.Append("║ Highway Clamp: ").Append((_vehicleSpeedKmh > 30.0f) ? "ENGAGED" : "FREE").Append('\n');
            _sbLcd.Append("║ DPU Assist:    ").Append((_vehicleSpeedKmh > 5.0f) ? "PUSH ACTIVE" : "COAST").Append('\n');
            _sbLcd.Append("║ Defense Grid:  ").Append(_defenseLockActive ? "COMBAT ENGAGED" : "WATCHKEEPING").Append('\n');
            _sbLcd.Append("╚══════════════════════════════════════════╝");

            _lcdHelm.WriteText(_sbLcd);
        }

        void RenderBoomScreen()
        {
            if (_lcdBoom == null) return;

            _sbLcd.Clear();
            _sbLcd.Append("╔══════════════════════════════════════════╗\n");
            _sbLcd.Append("║ 35M EXTRACTION ARM · KINEMATICS & JIT    ║\n");
            _sbLcd.Append("╠══════════════════════════════════════════╣\n");
            _sbLcd.Append("║ Macro:       ").Append(_kinematicsMacro).Append('\n');
            _sbLcd.Append("║ Rate (kg/s): ");
            SbFormat.AppendInt(_sbLcd, (int)_extractionRateKgSec);
            _sbLcd.Append('\n');
            _sbLcd.Append("║ Buffer Load: ");
            SbFormat.AppendInt(_sbLcd, (int)(_currentCargoMassKg * 0.001));
            _sbLcd.Append(" / 2500 t (");
            SbFormat.AppendPct(_sbLcd, _currentCargoMassKg / _maxCargoCapacityKg);
            _sbLcd.Append(")\n║ JIT ETA:     ");
            SbFormat.AppendInt(_sbLcd, (int)_secondsUntilFull);
            _sbLcd.Append(" s\n");
            _sbLcd.Append("║ Air Pre-Call: ").Append(_jitAirDispatched ? "DISPATCHED" : "READY").Append('\n');
            _sbLcd.Append("║ Gnd Pre-Call: ").Append(_jitGndDispatched ? "DISPATCHED" : "READY").Append('\n');
            _sbLcd.Append("╚══════════════════════════════════════════╝");

            _lcdBoom.WriteText(_sbLcd);
        }

        void RenderFleetScreen()
        {
            if (_lcdFleet == null) return;

            _sbLcd.Clear();
            _sbLcd.Append("╔══════════════════════════════════════════╗\n");
            _sbLcd.Append("║ RIG//OS-LITE · REGIONAL FLEET ROSTER     ║\n");
            _sbLcd.Append("╠══════════════════════════════════════════╣\n");
            _sbLcd.Append("║ ID      TYPE      STATUS       SEEN(s)   ║\n");
            _sbLcd.Append("╟──────────────────────────────────────────╢\n");

            for (int i = 0; i < MAX_ASSETS; i++)
            {
                if (_assets[i].Active)
                {
                    _sbLcd.Append("║ ");
                    SbFormat.AppendPadded(_sbLcd, _assets[i].Id, 7);
                    SbFormat.AppendPadded(_sbLcd, AssetTypeNames[_assets[i].Type], 10);
                    SbFormat.AppendPadded(_sbLcd, _assets[i].Stale ? "STALE" : _assets[i].Status, 13);
                    SbFormat.AppendInt(_sbLcd, (int)_assets[i].LastSeenSec);
                    _sbLcd.Append("s\n");
                }
            }

            _sbLcd.Append("╚══════════════════════════════════════════╝");
            _lcdFleet.WriteText(_sbLcd);
        }
        #endregion

        #region Inbound IGC Protocol Listener
        void ProcessInboundIgcMessages()
        {
            // Process IGC uncast messages
            while (IGC.UnicastListener.HasPendingMessage)
            {
                MyIGCMessage msg = IGC.UnicastListener.AcceptMessage();
                HandleIgcPacket(msg.Tag, msg.Data as string);
            }
        }

        void HandleIgcPacket(string tag, string payload)
        {
            if (payload == null) return;
            IgcProtocol.Split(payload, _args);
            if (_args.Count == 0) return;

            if (tag == "TMS.CAT.STATUS")
            {
                // CAT-01;gross_t;payload_t;speed_kmh;status
                RegisterOrUpdateAsset("CAT-01", ASSET_TRAIN, (_args.Count > 4) ? _args[4] : "EN_ROUTE");
            }
            else if (tag == "TMS.MULE.STATUS")
            {
                // MUL-01;gross_t;payload_t;altitude_m;status
                RegisterOrUpdateAsset("MUL-01", ASSET_SHUTTLE, (_args.Count > 4) ? _args[4] : "AIRBORNE");
            }
            else if (tag == "TMS.MAS.STATUS")
            {
                // KINEMATICS;state;macro;outriggers;hitch;dpu;speed
                if (_args.Count > 2)
                {
                    _kinematicsState = _args[1];
                    _kinematicsMacro = _args[2];
                }
                if (_args.Count > 6)
                {
                    float.TryParse(_args[6], out _vehicleSpeedKmh);
                }
            }
            else if (tag == "TMS.BASE.DEMAND")
            {
                // DEMAND;ORE=URANIUM;DEFICIT=15000
                // Vector scout drones to search for deficit mineral
            }
            else if (tag == "RIG.C2.HANDOVER")
            {
                // Yield C2 root to Ground Base
                if (_currentRole == ROLE_MOTHERSHIP)
                {
                    _currentRole = ROLE_WORKER;
                    _sbIgc.Clear();
                    _sbIgc.Append("MAS-01;GND-01;ACK");
                    IGC.SendBroadcastMessage("RIG.C2.HANDOVER", _sbIgc.ToString());
                }
            }
        }

        void RegisterOrUpdateAsset(string id, int type, string status)
        {
            for (int i = 0; i < MAX_ASSETS; i++)
            {
                if (_assets[i].Active && _assets[i].Id == id)
                {
                    _assets[i].Status = status;
                    _assets[i].LastSeenSec = 0.0;
                    _assets[i].Stale = false;
                    return;
                }
            }

            // Register new asset
            for (int i = 0; i < MAX_ASSETS; i++)
            {
                if (!_assets[i].Active)
                {
                    _assets[i].Active = true;
                    _assets[i].Id = id;
                    _assets[i].Type = type;
                    _assets[i].Status = status;
                    _assets[i].LastSeenSec = 0.0;
                    _assets[i].Stale = false;
                    _assetCount++;
                    return;
                }
            }
        }

        void InitRegistry()
        {
            for (int i = 0; i < MAX_ASSETS; i++)
            {
                _assets[i].Active = false;
                _assets[i].Id = "";
                _assets[i].Type = ASSET_NONE;
                _assets[i].Status = "IDLE";
                _assets[i].LastSeenSec = 0.0;
                _assets[i].Stale = false;
            }
            _assetCount = 0;

            // Pre-seed known ecosystem fleet units
            RegisterOrUpdateAsset("KES-01", ASSET_SCOUT, "READY");
            RegisterOrUpdateAsset("MUL-01", ASSET_SHUTTLE, "CRADLED");
            RegisterOrUpdateAsset("CAT-01", ASSET_TRAIN, "EN_ROUTE");
        }
        #endregion

        #region Command Handling
        void HandleCommand(string argument)
        {
            IgcProtocol.Split(argument, _args);
            if (_args.Count == 0) return;

            string cmd = _args[0];

            if (string.Compare(cmd, "HANDOVER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentRole = ROLE_WORKER;
                _sbIgc.Clear();
                _sbIgc.Append("MAS-01;GND-01;HANDOVER_EXEC");
                IGC.SendBroadcastMessage("RIG.C2.HANDOVER", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "JIT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && string.Compare(_args[1], "AIR", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _jitAirDispatched = true;
                    _sbIgc.Clear();
                    _sbIgc.Append("JIT;MAS-01;AIR;60;RARE_ORE;45000");
                }
                else
                {
                    _jitGndDispatched = true;
                    _sbIgc.Clear();
                    _sbIgc.Append("JIT;MAS-01;GND;180;BULK_ORE;2500000");
                }
                IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "TASK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string targetOre = (_args.Count > 1) ? _args[1] : "URANIUM";
                _sbIgc.Clear();
                _sbIgc.Append("TASK;KES-01;SECTOR;").Append(targetOre);
                IGC.SendBroadcastMessage("TMS.SCOUT.TASK", _sbIgc.ToString());
            }
            else if (string.Compare(cmd, "METEOR", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _meteorAlertActive = !_meteorAlertActive;
                if (_alarmMeteor != null) _alarmMeteor.Enabled = _meteorAlertActive;

                _sbIgc.Clear();
                _sbIgc.Append("METEOR;").Append(_meteorAlertActive ? "ALERT;HEAVY" : "CLEAR");
                IGC.SendBroadcastMessage("TMS.METEOR.ALERT", _sbIgc.ToString());
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
                _currentRole = _ini.Get("RigOS", "Role").ToInt32(_currentRole);
            }
        }

        void InitHardware()
        {
            _lcdHelm = GridTerminalSystem.GetBlockWithName("MAS LCD Helm") as IMyTextPanel;
            _lcdBoom = GridTerminalSystem.GetBlockWithName("MAS LCD Boom") as IMyTextPanel;
            _lcdFleet = GridTerminalSystem.GetBlockWithName("MAS LCD Fleet") as IMyTextPanel;

            _commAntenna = GridTerminalSystem.GetBlockWithName("MAS Antenna Laser") as IMyRadioAntenna;
            _alarmMeteor = GridTerminalSystem.GetBlockWithName("MAS Alarm Meteor");

            _cargoHolds.Clear();
            GridTerminalSystem.GetBlocksOfType(_cargoHolds, b => b.CubeGrid == Me.CubeGrid && b.CustomName.StartsWith("MAS Hold"));

            _ciwsTurrets.Clear();
            GridTerminalSystem.GetBlocksOfType(_ciwsTurrets, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Turret"));

            // Subscribe to regional broadcast channels
            IGC.RegisterBroadcastListener("TMS.CAT.STATUS");
            IGC.RegisterBroadcastListener("TMS.MULE.STATUS");
            IGC.RegisterBroadcastListener("TMS.MAS.STATUS");
            IGC.RegisterBroadcastListener("TMS.BASE.DEMAND");
            IGC.RegisterBroadcastListener("RIG.C2.HANDOVER");
        }
        #endregion
    }
}
