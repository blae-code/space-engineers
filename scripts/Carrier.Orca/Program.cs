// Carrier.Orca · Space Engineers 1 MDK2 PB Script
// Target Block: [ORC] PB Carrier Core
// Grid: Large Grid (17 × 19 × 7 B) · Update: Update10 + Update100
// Subsystem Groups: ORC CC, ORC PC, ORC HD, ORC AS, ORC MP, ORC BR, ORC JAWS, ORC OR, ORC LAT
// IGC Channels: TMS.ORCA.STATUS (out), TMS.MULE.CALL (out), FLT.REM.STATUS (in), TMS.MULE.STATUS (in), TMS.SCOUT.VEIN (in)
// Rule C4: Native blocks provide safety; PB provides decision logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region Subsystem Configuration & Telemetry
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(1024);
        readonly StringBuilder _sbIgc = new StringBuilder(512);
        readonly List<string> _args = new List<string>(16);
        readonly HeartbeatWatchdog _watchdog = new HeartbeatWatchdog(5.0);

        int _tickFast;
        int _tickSlow;

        // Core Terminal Block Handles (Grid-scoped: b => b.CubeGrid == Me.CubeGrid)
        IMyRemoteControl _remoteControl;
        IMyRadioAntenna _antenna;
        IMyCargoContainer _oreHold;
        IMyConveyorSorter _gateSorter;
        IMyConveyorSorter _drainSorter;
        IMyConveyorSorter _muleIntakeSorter;
        IMyShipConnector _muleConnector;
        IMySensorBlock _muleSensor;

        readonly List<IMyMotorAdvancedStator> _muleCradle = new List<IMyMotorAdvancedStator>(2);
        readonly List<IMyLandingGear> _muleClamps = new List<IMyLandingGear>(2);
        readonly List<IMyShipConnector> _bayConnectors = new List<IMyShipConnector>(6);
        readonly List<IMySensorBlock> _baySensors = new List<IMySensorBlock>(6);
        readonly List<IMyLightingBlock> _bayLights = new List<IMyLightingBlock>(6);
        readonly List<IMyMotorAdvancedStator> _bayJaws = new List<IMyMotorAdvancedStator>(12);
        readonly List<IMyLandingGear> _bayJawClamps = new List<IMyLandingGear>(12);

        readonly List<IMyReactor> _reactors = new List<IMyReactor>(6);
        readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(2);
        readonly List<IMyThrust> _liftThrusters = new List<IMyThrust>(4);
        readonly List<IMyThrust> _h2Thrusters = new List<IMyThrust>(2);
        readonly List<IMyThrust> _latThrusters = new List<IMyThrust>(28);
        readonly List<IMyGyro> _gyros = new List<IMyGyro>(4);

        // IGC Listeners
        IMyBroadcastListener _remoraListener;
        IMyBroadcastListener _muleListener;
        IMyBroadcastListener _scoutListener;

        // Operational State & Telemetry
        string _carrierState = "HOVER_STANDBY";
        double _oreHoldMassKg;
        double _lastOreMassKg;
        double _oreRateKgPerSec;
        float _oreHoldFillPct;
        double _reactorOutputMW;
        double _batteryStoredMWh;
        double _batteryMaxMWh = 6.0;
        float _batteryPct = 100.0f;
        int _freeBays = 6;
        bool _muleClamped;
        bool _manualJawsOpen;
        bool _emergencyMode;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Carrier.Orca (Core) Initialized.");
        }

        public void Save()
        {
            Storage = _carrierState;
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
                UpdateTickFast();
            }

            if ((updateSource & UpdateType.Update100) != 0)
            {
                UpdateTickSlow();
            }
        }

        void UpdateTickFast()
        {
            _tickFast++;
            _watchdog.Tick(1.0 / 6.0);

            // Inbound IGC processing
            ProcessInboundIgc();

            // Drone bay proximity & guide jaw kinematics
            UpdateBayKinematics();

            // Mule flight deck clamping & transfer interlock
            UpdateMuleDeckInterlock();
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Refresh cargo hold telemetry & dM/dt rate
            UpdateCargoHoldRate();

            // Refresh power generation & battery status
            UpdatePowerStatus();

            // Evaluate state machine transitions
            UpdateStateMachine();

            // Broadcast periodic telemetry packet
            BroadcastTelemetry();

            // Monospace Echo HUD
            RenderMonospaceEcho();
        }

        void ProcessInboundIgc()
        {
            if (_remoraListener != null)
            {
                while (_remoraListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _remoraListener.AcceptMessage();
                    // Process Remora status: parse fill%, status
                }
            }

            if (_muleListener != null)
            {
                while (_muleListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _muleListener.AcceptMessage();
                    // Process Mule shuttle status
                }
            }

            if (_scoutListener != null)
            {
                while (_scoutListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _scoutListener.AcceptMessage();
                    // Process vein survey coordinates
                }
            }
        }

        void UpdateBayKinematics()
        {
            int occupiedCount = 0;
            bool anyBayActive = false;

            for (int i = 0; i < _bayConnectors.Count; i++)
            {
                IMyShipConnector conn = _bayConnectors[i];
                if (conn == null) continue;

                if (conn.Status == MyShipConnectorStatus.Connected)
                {
                    occupiedCount++;
                }
            }
            _freeBays = _bayConnectors.Count - occupiedCount;

            // Check bay approach sensors
            for (int i = 0; i < _baySensors.Count; i++)
            {
                IMySensorBlock sensor = _baySensors[i];
                if (sensor == null) continue;

                if (sensor.IsActive)
                {
                    anyBayActive = true;
                    if (i < _bayLights.Count && _bayLights[i] != null)
                    {
                        _bayLights[i].Enabled = true;
                    }
                }
                else
                {
                    if (i < _bayLights.Count && _bayLights[i] != null)
                    {
                        _bayLights[i].Enabled = false;
                    }
                }
            }

            // Command guide jaw hinges
            float targetAngleDeg = (_manualJawsOpen || anyBayActive) ? 45.0f : 0.0f;
            float targetRpm = 15.0f;

            for (int i = 0; i < _bayJaws.Count; i++)
            {
                IMyMotorAdvancedStator hinge = _bayJaws[i];
                if (hinge == null) continue;

                if (targetAngleDeg > 0.0f)
                {
                    hinge.UpperLimitDeg = targetAngleDeg;
                    hinge.LowerLimitDeg = 0.0f;
                    hinge.TargetVelocityRPM = targetRpm;
                }
                else
                {
                    hinge.UpperLimitDeg = 45.0f;
                    hinge.LowerLimitDeg = 0.0f;
                    hinge.TargetVelocityRPM = -targetRpm;
                }
            }
        }

        void UpdateMuleDeckInterlock()
        {
            if (_muleConnector == null) return;

            bool isConnected = (_muleConnector.Status == MyShipConnectorStatus.Connected);

            if (isConnected)
            {
                _muleClamped = true;
                // Engage cradle clamping hinges to 30 deg
                for (int i = 0; i < _muleCradle.Count; i++)
                {
                    IMyMotorAdvancedStator hinge = _muleCradle[i];
                    if (hinge == null) continue;
                    hinge.UpperLimitDeg = 30.0f;
                    hinge.LowerLimitDeg = 0.0f;
                    hinge.TargetVelocityRPM = 10.0f;
                }

                // Lock magnetic clamps
                for (int i = 0; i < _muleClamps.Count; i++)
                {
                    IMyLandingGear clamp = _muleClamps[i];
                    if (clamp != null && clamp.LockMode == LandingGearMode.ReadyToLock)
                    {
                        clamp.Lock();
                    }
                }

                // Enable drain sorter to push ore into Mule
                if (_drainSorter != null)
                {
                    _drainSorter.DrainAll = true;
                    _drainSorter.Enabled = true;
                }

                if (_muleIntakeSorter != null)
                {
                    _muleIntakeSorter.Enabled = true;
                }
            }
            else
            {
                _muleClamped = false;
                // Retract cradle hinges to 0 deg
                for (int i = 0; i < _muleCradle.Count; i++)
                {
                    IMyMotorAdvancedStator hinge = _muleCradle[i];
                    if (hinge == null) continue;
                    hinge.UpperLimitDeg = 30.0f;
                    hinge.LowerLimitDeg = 0.0f;
                    hinge.TargetVelocityRPM = -10.0f;
                }

                // Unlock magnetic clamps
                for (int i = 0; i < _muleClamps.Count; i++)
                {
                    IMyLandingGear clamp = _muleClamps[i];
                    if (clamp != null && clamp.LockMode == LandingGearMode.Locked)
                    {
                        clamp.Unlock();
                    }
                }

                // Standby drain sorter
                if (_drainSorter != null)
                {
                    _drainSorter.DrainAll = false;
                }
            }
        }

        void UpdateCargoHoldRate()
        {
            if (_oreHold == null) return;

            IMyInventory inv = _oreHold.GetInventory(0);
            if (inv == null) return;

            _oreHoldMassKg = (double)inv.CurrentMass;
            double maxVol = (double)inv.MaxVolume;
            double curVol = (double)inv.CurrentVolume;
            _oreHoldFillPct = maxVol > 0.001 ? (float)(curVol / maxVol * 100.0) : 0.0f;

            double delta = _oreHoldMassKg - _lastOreMassKg;
            _lastOreMassKg = _oreHoldMassKg;
            // 100 ticks = ≈ 1.667 seconds
            _oreRateKgPerSec = delta / (100.0 / 60.0);

            // JIT Logistics dispatch: when hold exceeds 75% capacity, call Mule
            if (_oreHoldFillPct >= 75.0f && !_muleClamped)
            {
                TriggerMuleDispatch();
            }
        }

        void TriggerMuleDispatch()
        {
            if (IGC == null) return;

            _sbIgc.Clear();
            _sbIgc.Append("ORC-01;HOLD_FULL;");
            SbFormat.AppendDouble(_sbIgc, _oreHoldFillPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _oreHoldMassKg / 1000.0, 1);
            _sbIgc.Append(";");

            if (_remoteControl != null)
            {
                Vector3D pos = _remoteControl.GetPosition();
                _sbIgc.Append("GPS:ORCA-01:");
                SbFormat.AppendDouble(_sbIgc, pos.X, 1);
                _sbIgc.Append(":");
                SbFormat.AppendDouble(_sbIgc, pos.Y, 1);
                _sbIgc.Append(":");
                SbFormat.AppendDouble(_sbIgc, pos.Z, 1);
                _sbIgc.Append(":");
            }
            else
            {
                _sbIgc.Append("GPS:NONE:0:0:0:");
            }

            IGC.SendBroadcastMessage("TMS.MULE.CALL", _sbIgc.ToString());
        }

        void UpdatePowerStatus()
        {
            double rSum = 0.0;
            for (int i = 0; i < _reactors.Count; i++)
            {
                IMyReactor r = _reactors[i];
                if (r != null && r.IsWorking)
                {
                    rSum += (double)r.CurrentOutput;
                }
            }
            _reactorOutputMW = rSum;

            double bStored = 0.0;
            double bMax = 0.0;
            for (int i = 0; i < _batteries.Count; i++)
            {
                IMyBatteryBlock b = _batteries[i];
                if (b != null)
                {
                    bStored += (double)b.CurrentStoredPower;
                    bMax += (double)b.MaxStoredPower;
                }
            }
            _batteryStoredMWh = bStored;
            _batteryMaxMWh = bMax > 0.001 ? bMax : 6.0;
            _batteryPct = (float)(_batteryStoredMWh / _batteryMaxMWh * 100.0);
        }

        void UpdateStateMachine()
        {
            if (_emergencyMode)
            {
                _carrierState = "EMERGENCY_ASCENT";
            }
            else if (_muleClamped)
            {
                _carrierState = "MULE_TRANSSHIP";
            }
            else if (_manualJawsOpen || _freeBays < _bayConnectors.Count)
            {
                _carrierState = "BAY_OPERATION";
            }
            else
            {
                _carrierState = "HOVER_STANDBY";
            }
        }

        void BroadcastTelemetry()
        {
            if (IGC == null) return;

            _sbIgc.Clear();
            _sbIgc.Append("ORC-01;");
            _sbIgc.Append(_carrierState);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _oreHoldFillPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _batteryPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _reactorOutputMW, 2);
            _sbIgc.Append(";");
            SbFormat.AppendInt(_sbIgc, _freeBays);

            IGC.SendBroadcastMessage("TMS.ORCA.STATUS", _sbIgc.ToString());
        }

        void RenderMonospaceEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" ORCA-01 HEAVY MINING CARRIER CORE C2  \n");
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" STATE:       ");
            _sbEcho.Append(_carrierState);
            _sbEcho.Append("\n");
            _sbEcho.Append(" ORE HOLD:    ");
            SbFormat.AppendDouble(_sbEcho, _oreHoldFillPct, 1);
            _sbEcho.Append("% (");
            SbFormat.AppendDouble(_sbEcho, _oreHoldMassKg / 1000.0, 1);
            _sbEcho.Append(" t)\n");
            _sbEcho.Append(" EXTRACTION:  ");
            SbFormat.AppendDouble(_sbEcho, _oreRateKgPerSec, 1);
            _sbEcho.Append(" kg/s\n");
            _sbEcho.Append(" POWER BUS:   ");
            SbFormat.AppendDouble(_sbEcho, _reactorOutputMW, 2);
            _sbEcho.Append(" MW (Nuclear)\n");
            _sbEcho.Append(" BATTERY:     ");
            SbFormat.AppendDouble(_sbEcho, _batteryPct, 1);
            _sbEcho.Append("% (");
            SbFormat.AppendDouble(_sbEcho, _batteryStoredMWh, 2);
            _sbEcho.Append(" / 6.0 MWh)\n");
            _sbEcho.Append(" DRONE BAYS:  ");
            SbFormat.AppendInt(_sbEcho, _freeBays);
            _sbEcho.Append(" / 6 Available\n");
            _sbEcho.Append(" MULE DECK:   ");
            _sbEcho.Append(_muleClamped ? "LOCKED (DRAIN ACTIVE)" : "READY / STANDBY");
            _sbEcho.Append("\n");
            _sbEcho.Append(" WATCHDOG:    ");
            _sbEcho.Append(_watchdog.IsTimedOut ? "TRIPPED!" : "OK");
            _sbEcho.Append(" (Ticks: ");
            SbFormat.AppendInt(_sbEcho, _tickSlow);
            _sbEcho.Append(")\n");
            _sbEcho.Append("========================================\n");

            Echo(_sbEcho.ToString());
        }

        void HandleCommand(string argument)
        {
            IgcProtocol.Split(argument, _args);
            if (_args.Count == 0) return;

            string cmd = _args[0];
            if (string.Compare(cmd, "PING", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _watchdog.Ping();
            }
            else if (string.Compare(cmd, "JAWS", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                if (string.Compare(_args[1], "OPEN", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _manualJawsOpen = true;
                }
                else if (string.Compare(_args[1], "CLOSE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _manualJawsOpen = false;
                }
            }
            else if (string.Compare(cmd, "CALL_MULE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                TriggerMuleDispatch();
            }
            else if (string.Compare(cmd, "EMERGENCY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _emergencyMode = !_emergencyMode;
                for (int i = 0; i < _h2Thrusters.Count; i++)
                {
                    if (_h2Thrusters[i] != null)
                    {
                        _h2Thrusters[i].Enabled = _emergencyMode;
                    }
                }
            }
            else if (string.Compare(cmd, "RELOAD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                LoadConfiguration();
                InitHardware();
                InitNetwork();
            }
        }

        void LoadConfiguration()
        {
            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                // Parse CustomData override parameters if present
            }
        }

        void InitHardware()
        {
            _remoteControl = null;
            _antenna = null;
            _oreHold = null;
            _gateSorter = null;
            _drainSorter = null;
            _muleIntakeSorter = null;
            _muleConnector = null;
            _muleSensor = null;

            _muleCradle.Clear();
            _muleClamps.Clear();
            _bayConnectors.Clear();
            _baySensors.Clear();
            _bayLights.Clear();
            _bayJaws.Clear();
            _bayJawClamps.Clear();

            _reactors.Clear();
            _batteries.Clear();
            _liftThrusters.Clear();
            _h2Thrusters.Clear();
            _latThrusters.Clear();
            _gyros.Clear();

            GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(null, b =>
            {
                if (b.CubeGrid != Me.CubeGrid) return false;

                string name = b.CustomName;

                if (b is IMyRemoteControl && name.IndexOf("RC Main", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _remoteControl = (IMyRemoteControl)b;
                }
                else if (b is IMyRadioAntenna && name.IndexOf("ORC CC Antenna", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _antenna = (IMyRadioAntenna)b;
                }
                else if (b is IMyCargoContainer && name.IndexOf("ORC Ore Hold", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _oreHold = (IMyCargoContainer)b;
                }
                else if (b is IMyConveyorSorter)
                {
                    if (name.IndexOf("ORC HD Gate", StringComparison.OrdinalIgnoreCase) >= 0)
                        _gateSorter = (IMyConveyorSorter)b;
                    else if (name.IndexOf("ORC HD Drain", StringComparison.OrdinalIgnoreCase) >= 0)
                        _drainSorter = (IMyConveyorSorter)b;
                    else if (name.IndexOf("ORC MP Intake", StringComparison.OrdinalIgnoreCase) >= 0)
                        _muleIntakeSorter = (IMyConveyorSorter)b;
                }
                else if (b is IMyShipConnector)
                {
                    if (name.IndexOf("ORC MP Conn", StringComparison.OrdinalIgnoreCase) >= 0)
                        _muleConnector = (IMyShipConnector)b;
                    else if (name.IndexOf("ORC BR-", StringComparison.OrdinalIgnoreCase) >= 0)
                        _bayConnectors.Add((IMyShipConnector)b);
                }
                else if (b is IMySensorBlock)
                {
                    if (name.IndexOf("ORC Sensor MP", StringComparison.OrdinalIgnoreCase) >= 0)
                        _muleSensor = (IMySensorBlock)b;
                    else if (name.IndexOf("ORC Sensor BR-", StringComparison.OrdinalIgnoreCase) >= 0)
                        _baySensors.Add((IMySensorBlock)b);
                }
                else if (b is IMyLightingBlock && name.IndexOf("ORC Bay Lights", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _bayLights.Add((IMyLightingBlock)b);
                }
                else if (b is IMyMotorAdvancedStator)
                {
                    if (name.IndexOf("ORC MP Cradle", StringComparison.OrdinalIgnoreCase) >= 0)
                        _muleCradle.Add((IMyMotorAdvancedStator)b);
                    else if (name.IndexOf("ORC Bay Jaws", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("Hinge Outer", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("Hinge Inner", StringComparison.OrdinalIgnoreCase) >= 0)
                        _bayJaws.Add((IMyMotorAdvancedStator)b);
                }
                else if (b is IMyLandingGear)
                {
                    if (name.IndexOf("ORC MP Cradle Clamp", StringComparison.OrdinalIgnoreCase) >= 0)
                        _muleClamps.Add((IMyLandingGear)b);
                    else if (name.IndexOf("ORC Bay Jaw Clamps", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("Jaw Clamp", StringComparison.OrdinalIgnoreCase) >= 0)
                        _bayJawClamps.Add((IMyLandingGear)b);
                }
                else if (b is IMyReactor && name.IndexOf("ORC Reactors", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _reactors.Add((IMyReactor)b);
                }
                else if (b is IMyBatteryBlock && name.IndexOf("ORC Batteries", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _batteries.Add((IMyBatteryBlock)b);
                }
                else if (b is IMyThrust)
                {
                    if (name.IndexOf("ORC Lift", StringComparison.OrdinalIgnoreCase) >= 0)
                        _liftThrusters.Add((IMyThrust)b);
                    else if (name.IndexOf("ORC H2 Assist", StringComparison.OrdinalIgnoreCase) >= 0)
                        _h2Thrusters.Add((IMyThrust)b);
                    else if (name.IndexOf("ORC Lat", StringComparison.OrdinalIgnoreCase) >= 0)
                        _latThrusters.Add((IMyThrust)b);
                }
                else if (b is IMyGyro && name.IndexOf("ORC Gyros", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _gyros.Add((IMyGyro)b);
                }

                return false;
            });
        }

        void InitNetwork()
        {
            if (IGC == null) return;
            _remoraListener = IGC.RegisterBroadcastListener("FLT.REM.STATUS");
            _muleListener = IGC.RegisterBroadcastListener("TMS.MULE.STATUS");
            _scoutListener = IGC.RegisterBroadcastListener("TMS.SCOUT.VEIN");
        }
    }
}
