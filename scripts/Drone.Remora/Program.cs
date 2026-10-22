// Drone.Remora · Space Engineers 1 MDK2 PB Script
// Target Block: [REM] PB Miner
// Grid: Small Grid (11 × 11 sb) · Update: Update10 + Update100
// Subsystem Groups: REM Lift, REM Lat F/A/P/S, REM Drills, REM Batteries, REM Gyros, REM Cams, REM Lights, REM Logic
// IGC Channels: FLT.REM.STATUS (out), FLT.DISP.LEASE (in), TMS.MINER.CMD (in)
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
        IMyShipConnector _dockCrown;
        IMyCameraBlock _camDown;
        IMyCameraBlock _camUp;
        IMyLightingBlock _lightDown;

        readonly List<IMyShipDrill> _drills = new List<IMyShipDrill>(8);
        readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(2);
        readonly List<IMyThrust> _liftThrusters = new List<IMyThrust>(4);
        readonly List<IMyThrust> _latThrusters = new List<IMyThrust>(4);
        readonly List<IMyGyro> _gyros = new List<IMyGyro>(2);

        // IGC Listeners
        IMyBroadcastListener _leaseListener;
        IMyBroadcastListener _cmdListener;

        // Operational State & Telemetry
        string _drillerState = "DOCKED_CHARGING";
        double _totalOreMassKg;
        double _lastOreMassKg;
        double _oreRateKgPerSec;
        float _cargoFillPct;
        double _batteryStoredMWh;
        double _batteryMaxMWh = 2.16;
        float _batteryPct = 100.0f;
        double _currentDepthMeters;
        double _targetDepthMeters = 30.0;
        Vector3D _collarGps;
        bool _drillingActive;
        bool _rechargeMode = true;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Drone.Remora (Miner) Initialized.");
        }

        public void Save()
        {
            Storage = _drillerState;
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

            // Inbound IGC message processing
            ProcessInboundIgc();

            // Refresh physical dock status
            RefreshDockState();

            // Fast drilling kinematic supervisory check
            SuperviseDrillDescent();
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Refresh cargo inventory mass & accumulation rate dM/dt
            UpdateCargoRate();

            // Refresh power storage telemetry
            UpdatePowerStatus();

            // Evaluate state machine transitions
            UpdateStateMachine();

            // Broadcast real-time status to host Orca carrier
            BroadcastTelemetry();

            // Monospace Echo HUD
            RenderMonospaceEcho();
        }

        void ProcessInboundIgc()
        {
            if (_leaseListener != null)
            {
                while (_leaseListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _leaseListener.AcceptMessage();
                    // Parse lease target GPS, max depth, return deadline
                }
            }

            if (_cmdListener != null)
            {
                while (_cmdListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _cmdListener.AcceptMessage();
                    if (msg.Data is string)
                    {
                        HandleCommand((string)msg.Data);
                    }
                }
            }
        }

        void RefreshDockState()
        {
            if (_dockCrown == null) return;

            bool isConnected = (_dockCrown.Status == MyShipConnectorStatus.Connected);

            if (isConnected)
            {
                if (!_rechargeMode)
                {
                    SetBatteryRecharge(true);
                }
                SetDrills(false);
                if (_drillerState != "DOCKED_CHARGING")
                {
                    _drillerState = "DOCKED_CHARGING";
                }
            }
            else
            {
                if (_rechargeMode)
                {
                    SetBatteryRecharge(false);
                }
            }
        }

        void SuperviseDrillDescent()
        {
            if (!_drillingActive) return;

            // Compute depth delta from shaft collar if collar GPS recorded
            if (_remoteControl != null && _collarGps != Vector3D.Zero)
            {
                Vector3D curPos = _remoteControl.GetPosition();
                _currentDepthMeters = Vector3D.Distance(curPos, _collarGps);
            }

            // Check depth target
            if (_currentDepthMeters >= _targetDepthMeters)
            {
                // Reached target depth; abort drilling and start climb out
                SetDrills(false);
                _drillingActive = false;
                _drillerState = "CLIMB_OUT";
            }
        }

        void UpdateCargoRate()
        {
            double massSum = 0.0;
            double curVolSum = 0.0;
            double maxVolSum = 0.0;

            for (int i = 0; i < _drills.Count; i++)
            {
                IMyShipDrill drill = _drills[i];
                if (drill == null) continue;

                IMyInventory inv = drill.GetInventory(0);
                if (inv != null)
                {
                    massSum += (double)inv.CurrentMass;
                    curVolSum += (double)inv.CurrentVolume;
                    maxVolSum += (double)inv.MaxVolume;
                }
            }

            _totalOreMassKg = massSum;
            _cargoFillPct = maxVolSum > 0.001 ? (float)(curVolSum / maxVolSum * 100.0) : 0.0f;

            double delta = _totalOreMassKg - _lastOreMassKg;
            _lastOreMassKg = _totalOreMassKg;
            _oreRateKgPerSec = delta / (100.0 / 60.0);

            // Payload cap: if fill exceeds 85%, abort drilling and climb out
            if (_cargoFillPct >= 85.0f && _drillingActive)
            {
                SetDrills(false);
                _drillingActive = false;
                _drillerState = "CLIMB_OUT";
            }
        }

        void UpdatePowerStatus()
        {
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
            _batteryMaxMWh = bMax > 0.001 ? bMax : 2.16;
            _batteryPct = (float)(_batteryStoredMWh / _batteryMaxMWh * 100.0);

            // Battery threshold failsafe: if <= 25%, abort drilling
            if (_batteryPct <= 25.0f && _drillingActive)
            {
                SetDrills(false);
                _drillingActive = false;
                _drillerState = "EMERGENCY_ABORT";
            }
        }

        void UpdateStateMachine()
        {
            if (_dockCrown != null && _dockCrown.Status == MyShipConnectorStatus.Connected)
            {
                _drillerState = "DOCKED_CHARGING";
            }
            else if (_batteryPct <= 25.0f)
            {
                _drillerState = "EMERGENCY_ABORT";
            }
            else if (_drillingActive)
            {
                _drillerState = "DESCENT_BORE";
            }
            else if (_cargoFillPct >= 85.0f)
            {
                _drillerState = "CLIMB_OUT";
            }
        }

        void BroadcastTelemetry()
        {
            if (IGC == null) return;

            _sbIgc.Clear();
            _sbIgc.Append("REM-01;");
            _sbIgc.Append(_drillerState);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _cargoFillPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _batteryPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _currentDepthMeters, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _oreRateKgPerSec, 1);

            IGC.SendBroadcastMessage("FLT.REM.STATUS", _sbIgc.ToString());
        }

        void RenderMonospaceEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" REMORA-01 VERTICAL DRILLER CORE C2    \n");
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" STATE:       ");
            _sbEcho.Append(_drillerState);
            _sbEcho.Append("\n");
            _sbEcho.Append(" CARGO FILL:  ");
            SbFormat.AppendDouble(_sbEcho, _cargoFillPct, 1);
            _sbEcho.Append("% (");
            SbFormat.AppendDouble(_sbEcho, _totalOreMassKg / 1000.0, 1);
            _sbEcho.Append(" t)\n");
            _sbEcho.Append(" EXTRACTION:  ");
            SbFormat.AppendDouble(_sbEcho, _oreRateKgPerSec, 1);
            _sbEcho.Append(" kg/s\n");
            _sbEcho.Append(" DEPTH:       ");
            SbFormat.AppendDouble(_sbEcho, _currentDepthMeters, 1);
            _sbEcho.Append(" / ");
            SbFormat.AppendDouble(_sbEcho, _targetDepthMeters, 1);
            _sbEcho.Append(" m\n");
            _sbEcho.Append(" BATTERY:     ");
            SbFormat.AppendDouble(_sbEcho, _batteryPct, 1);
            _sbEcho.Append("% (");
            SbFormat.AppendDouble(_sbEcho, _batteryStoredMWh, 2);
            _sbEcho.Append(" / 2.16 MWh)\n");
            _sbEcho.Append(" DRILLS:      ");
            _sbEcho.Append(_drillingActive ? "ACTIVE (8x CUTTER)" : "STANDBY / OFF");
            _sbEcho.Append("\n");
            _sbEcho.Append(" DOCK:        ");
            _sbEcho.Append((_dockCrown != null && _dockCrown.Status == MyShipConnectorStatus.Connected) ? "LOCKED (ORCA)" : "UNDOCKED");
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
            else if (string.Compare(cmd, "DRILL", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                if (string.Compare(_args[1], "START", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (_remoteControl != null)
                    {
                        _collarGps = _remoteControl.GetPosition();
                    }
                    _drillingActive = true;
                    SetDrills(true);
                    _drillerState = "DESCENT_BORE";
                }
                else if (string.Compare(_args[1], "STOP", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _drillingActive = false;
                    SetDrills(false);
                    _drillerState = "CLIMB_OUT";
                }
            }
            else if (string.Compare(cmd, "ABORT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _drillingActive = false;
                SetDrills(false);
                _drillerState = "EMERGENCY_ABORT";
            }
            else if (string.Compare(cmd, "RELOAD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                LoadConfiguration();
                InitHardware();
                InitNetwork();
            }
        }

        void SetDrills(bool enabled)
        {
            for (int i = 0; i < _drills.Count; i++)
            {
                if (_drills[i] != null)
                {
                    _drills[i].Enabled = enabled;
                }
            }
            if (_lightDown != null)
            {
                _lightDown.Enabled = enabled;
            }
        }

        void SetBatteryRecharge(bool recharge)
        {
            _rechargeMode = recharge;
            for (int i = 0; i < _batteries.Count; i++)
            {
                if (_batteries[i] != null)
                {
                    _batteries[i].ChargeMode = recharge ? ChargeMode.Recharge : ChargeMode.Auto;
                }
            }
        }

        void LoadConfiguration()
        {
            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                // Parse CustomData parameters
            }
        }

        void InitHardware()
        {
            _remoteControl = null;
            _antenna = null;
            _dockCrown = null;
            _camDown = null;
            _camUp = null;
            _lightDown = null;

            _drills.Clear();
            _batteries.Clear();
            _liftThrusters.Clear();
            _latThrusters.Clear();
            _gyros.Clear();

            GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(null, b =>
            {
                if (b.CubeGrid != Me.CubeGrid) return false;

                string name = b.CustomName;

                if (b is IMyRemoteControl && name.IndexOf("REM RC", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _remoteControl = (IMyRemoteControl)b;
                }
                else if (b is IMyRadioAntenna && name.IndexOf("REM Antenna", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _antenna = (IMyRadioAntenna)b;
                }
                else if (b is IMyShipConnector && name.IndexOf("REM Dock", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _dockCrown = (IMyShipConnector)b;
                }
                else if (b is IMyCameraBlock)
                {
                    if (name.IndexOf("REM Cam Down", StringComparison.OrdinalIgnoreCase) >= 0)
                        _camDown = (IMyCameraBlock)b;
                    else if (name.IndexOf("REM Cam Up", StringComparison.OrdinalIgnoreCase) >= 0)
                        _camUp = (IMyCameraBlock)b;
                }
                else if (b is IMyLightingBlock && name.IndexOf("REM Light Down", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _lightDown = (IMyLightingBlock)b;
                }
                else if (b is IMyShipDrill && (name.IndexOf("REM DH Drill", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("REM Drill", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    _drills.Add((IMyShipDrill)b);
                }
                else if (b is IMyBatteryBlock && name.IndexOf("REM Battery", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _batteries.Add((IMyBatteryBlock)b);
                }
                else if (b is IMyThrust)
                {
                    if (name.IndexOf("REM LF Lift", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("REM Lift", StringComparison.OrdinalIgnoreCase) >= 0)
                        _liftThrusters.Add((IMyThrust)b);
                    else if (name.IndexOf("REM Lat", StringComparison.OrdinalIgnoreCase) >= 0)
                        _latThrusters.Add((IMyThrust)b);
                }
                else if (b is IMyGyro && name.IndexOf("REM Gyro", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _gyros.Add((IMyGyro)b);
                }

                return false;
            });
        }

        void InitNetwork()
        {
            if (IGC == null) return;
            _leaseListener = IGC.RegisterBroadcastListener("FLT.DISP.LEASE");
            _cmdListener = IGC.RegisterBroadcastListener("TMS.MINER.CMD");
        }
    }
}
