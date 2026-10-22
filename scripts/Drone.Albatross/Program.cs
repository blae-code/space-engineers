// Drone.Albatross · Space Engineers 1 MDK2 PB Script
// Target Block: [ALB] PB Utility
// Grid: Small Grid (9 × 11 × 5 sb) · Update: Update10 + Update100
// Subsystem Groups: ALB Thrusters All, ALB Thrusters Lift, ALB Thrusters Maneuver, ALB Batteries, ALB Lights, ALB Tools, ALB Cameras
// IGC Channels: TMS.UTIL.STATUS (out), TMS.SUPPLY.RESTOCK (out), TMS.REPAIR.CALL (in), TMS.REPAIR.STATUS (in)
// Rule C4: Native blocks provide safety; PB provides decision logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

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

        // Terminal Block Handles (Grid-scoped: b => b.CubeGrid == Me.CubeGrid)
        IMyShipConnector _dockConn;
        IMyRemoteControl _remoteControl;
        IMyGyro _gyro;
        IMyShipWelder _welderNose;
        IMySensorBlock _weldSensor;
        IMyCargoContainer _cargoHold;
        IMyRadioAntenna _antenna;
        IMyReactor _reactor;
        readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(4);
        readonly List<IMyThrust> _liftThrusters = new List<IMyThrust>(8);
        readonly List<IMyThrust> _maneuverThrusters = new List<IMyThrust>(14);
        readonly List<IMyLightingBlock> _workLights = new List<IMyLightingBlock>(2);

        // IGC Listeners
        IMyBroadcastListener _repairCallListener;
        IMyBroadcastListener _repairStatusListener;

        // Operational State & Telemetry
        string _flightState = "IDLE_DOCKED";
        string _targetDamageGps = "GPS:NONE:0:0:0:";
        string _assignedHostPad = "GPS:PAD:0:0:0:";
        double _grossMassKg = 8500.0;
        double _holdCurrentMassKg;
        float _holdVolumePct;
        double _batteryStoredMWh;
        double _batteryMaxMWh = 2.0;
        float _batteryPct = 100.0f;
        int _repairsCompleted;
        bool _manualWeldOverride;
        bool _rechargeMode;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Drone.Albatross (Utility) Initialized.");
        }

        public void Save()
        {
            Storage = _flightState;
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

            // Process inbound IGC packets
            ProcessInboundIgc();

            // Refresh physical dock status
            RefreshDockState();

            // Check auto-weld sensor in welding zone
            EvaluateWelderAutomation();
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Assess energy reserves, inventory volume, and mass
            EvaluateShipMassAndPower();

            // Check battery threshold for autonomous RTH
            if (_batteryPct < 25.0f && (string.Compare(_flightState, "STATION_KEEPING", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(_flightState, "WELDING", StringComparison.OrdinalIgnoreCase) == 0))
            {
                ExecuteRth();
            }

            // Broadcast periodic telemetry status
            BroadcastStatus();

            RenderEcho();
        }

        void RefreshDockState()
        {
            if (_dockConn == null) return;

            if (_dockConn.Status == MyShipConnectorStatus.Connected)
            {
                if (string.Compare(_flightState, "TRANSIT_OUT", StringComparison.OrdinalIgnoreCase) != 0 &&
                    string.Compare(_flightState, "WELDING", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    _flightState = "IDLE_DOCKED";
                }
            }
        }

        void EvaluateWelderAutomation()
        {
            if (_welderNose == null) return;

            if (_manualWeldOverride)
            {
                _welderNose.Enabled = true;
                _flightState = "WELDING";
                return;
            }

            if (_weldSensor != null && _weldSensor.IsActive)
            {
                _welderNose.Enabled = true;
                if (string.Compare(_flightState, "IDLE_DOCKED", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    _flightState = "WELDING";
                }
            }
            else
            {
                if (!_manualWeldOverride && _welderNose.Enabled)
                {
                    _welderNose.Enabled = false;
                    if (string.Compare(_flightState, "WELDING", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        _flightState = "STATION_KEEPING";
                        _repairsCompleted++;
                    }
                }
            }
        }

        void EvaluateShipMassAndPower()
        {
            if (_remoteControl != null)
            {
                MyShipMass mass = _remoteControl.CalculateShipMass();
                _grossMassKg = mass.TotalMass;
            }

            if (_cargoHold != null)
            {
                IMyInventory inv = _cargoHold.GetInventory(0);
                if (inv != null)
                {
                    _holdCurrentMassKg = (double)inv.CurrentMass;
                    _holdVolumePct = (float)((double)inv.CurrentVolume / (double)inv.MaxVolume * 100.0);
                }
            }

            _batteryStoredMWh = 0.0;
            _batteryMaxMWh = 0.0;
            for (int i = 0; i < _batteries.Count; i++)
            {
                IMyBatteryBlock bat = _batteries[i];
                if (bat != null)
                {
                    _batteryStoredMWh += bat.CurrentStoredPower;
                    _batteryMaxMWh += bat.MaxStoredPower;
                }
            }

            if (_batteryMaxMWh > 0.0)
            {
                _batteryPct = (float)((_batteryStoredMWh / _batteryMaxMWh) * 100.0);
            }
        }

        void BroadcastStatus()
        {
            _sbIgc.Clear();
            _sbIgc.Append("ALB-01;");
            _sbIgc.Append(_flightState);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _holdVolumePct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _batteryPct, 1);
            _sbIgc.Append(";");
            if (_remoteControl != null)
            {
                _sbIgc.Append("GPS:ALB-01:");
                SbFormat.AppendDouble(_sbIgc, _remoteControl.GetPosition().X, 1);
                _sbIgc.Append(":");
                SbFormat.AppendDouble(_sbIgc, _remoteControl.GetPosition().Y, 1);
                _sbIgc.Append(":");
                SbFormat.AppendDouble(_sbIgc, _remoteControl.GetPosition().Z, 1);
                _sbIgc.Append(":");
            }
            else
            {
                _sbIgc.Append("GPS:NONE:0:0:0:");
            }

            IGC.SendBroadcastMessage("TMS.UTIL.STATUS", _sbIgc.ToString());
        }

        void BroadcastRestockOrder(int plates, int motors, int comps)
        {
            _sbIgc.Clear();
            _sbIgc.Append("RESTOCK;ALB-01;");
            SbFormat.AppendInt(_sbIgc, plates);
            _sbIgc.Append(";");
            SbFormat.AppendInt(_sbIgc, motors);
            _sbIgc.Append(";");
            SbFormat.AppendInt(_sbIgc, comps);

            IGC.SendBroadcastMessage("TMS.SUPPLY.RESTOCK", _sbIgc.ToString());
        }

        void ProcessInboundIgc()
        {
            // Process Unicast Listener
            while (IGC.UnicastListener.HasPendingMessage)
            {
                MyIGCMessage msg = IGC.UnicastListener.AcceptMessage();
                HandleIgcPacket(msg.Tag, msg.Data as string);
            }

            // Process Broadcast Listeners
            ProcessListener(_repairCallListener);
            ProcessListener(_repairStatusListener);
        }

        void ProcessListener(IMyBroadcastListener listener)
        {
            if (listener == null) return;
            while (listener.HasPendingMessage)
            {
                MyIGCMessage msg = listener.AcceptMessage();
                HandleIgcPacket(msg.Tag, msg.Data as string);
            }
        }

        void HandleIgcPacket(string tag, string payload)
        {
            if (payload == null) return;
            IgcProtocol.Split(payload, _args);
            if (_args.Count == 0) return;

            if (tag == "TMS.REPAIR.CALL")
            {
                // CALL;ALB-01;target_drone_id;damage_gps;urgency
                if (_args.Count >= 2 && string.Compare(_args[1], "ALB-01", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _watchdog.Ping();
                    if (_args.Count > 3) _targetDamageGps = _args[3];
                    DispatchToRepair();
                }
            }
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
            else if (string.Compare(cmd, "WELD_ON", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _manualWeldOverride = true;
            }
            else if (string.Compare(cmd, "WELD_OFF", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _manualWeldOverride = false;
                if (_welderNose != null) _welderNose.Enabled = false;
            }
            else if (string.Compare(cmd, "RTH", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ExecuteRth();
            }
            else if (string.Compare(cmd, "RESTOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                BroadcastRestockOrder(500, 100, 50);
            }
            else if (string.Compare(cmd, "RECHARGE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ToggleRecharge();
            }
            else if (string.Compare(cmd, "RELOAD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                LoadConfiguration();
                InitHardware();
                InitNetwork();
            }
        }

        void DispatchToRepair()
        {
            _flightState = "TRANSIT_OUT";
            SetBatteriesRecharge(false);
            SetThrustersEnabled(_liftThrusters, true);
            SetThrustersEnabled(_maneuverThrusters, true);

            if (_dockConn != null && _dockConn.Status == MyShipConnectorStatus.Connected)
            {
                _dockConn.Disconnect();
            }
        }

        void ExecuteRth()
        {
            _flightState = "RTH";
            _manualWeldOverride = false;
            if (_welderNose != null) _welderNose.Enabled = false;
            if (_remoteControl != null) _remoteControl.DampenersOverride = true;
        }

        void ToggleRecharge()
        {
            _rechargeMode = !_rechargeMode;
            SetBatteriesRecharge(_rechargeMode);
        }

        void SetBatteriesRecharge(bool recharge)
        {
            ChargeMode mode = recharge ? ChargeMode.Recharge : ChargeMode.Auto;
            for (int i = 0; i < _batteries.Count; i++)
            {
                IMyBatteryBlock b = _batteries[i];
                if (b != null) b.ChargeMode = mode;
            }
        }

        void SetThrustersEnabled(List<IMyThrust> list, bool enabled)
        {
            for (int i = 0; i < list.Count; i++)
            {
                IMyThrust t = list[i];
                if (t != null) t.Enabled = enabled;
            }
        }

        void LoadConfiguration()
        {
            if (Storage != null && Storage.Length > 0)
            {
                _flightState = Storage;
            }

            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                _assignedHostPad = _ini.Get("Navigation", "AssignedHostPad").ToString(_assignedHostPad);
            }
        }

        void InitHardware()
        {
            _dockConn = GridTerminalSystem.GetBlockWithName("ALB Dock Conn") as IMyShipConnector;
            if (_dockConn != null && _dockConn.CubeGrid != Me.CubeGrid) _dockConn = null;

            _remoteControl = GridTerminalSystem.GetBlockWithName("ALB Remote Control") as IMyRemoteControl;
            if (_remoteControl != null && _remoteControl.CubeGrid != Me.CubeGrid) _remoteControl = null;

            _gyro = GridTerminalSystem.GetBlockWithName("ALB Gyro") as IMyGyro;
            if (_gyro != null && _gyro.CubeGrid != Me.CubeGrid) _gyro = null;

            _welderNose = GridTerminalSystem.GetBlockWithName("ALB Welder Nose") as IMyShipWelder;
            if (_welderNose != null && _welderNose.CubeGrid != Me.CubeGrid) _welderNose = null;

            _weldSensor = GridTerminalSystem.GetBlockWithName("ALB Sensor WeldZone") as IMySensorBlock;
            if (_weldSensor != null && _weldSensor.CubeGrid != Me.CubeGrid) _weldSensor = null;

            _cargoHold = GridTerminalSystem.GetBlockWithName("ALB Hold Components") as IMyCargoContainer;
            if (_cargoHold != null && _cargoHold.CubeGrid != Me.CubeGrid) _cargoHold = null;

            _antenna = GridTerminalSystem.GetBlockWithName("ALB Antenna") as IMyRadioAntenna;
            if (_antenna != null && _antenna.CubeGrid != Me.CubeGrid) _antenna = null;

            _reactor = GridTerminalSystem.GetBlockWithName("ALB Reactor") as IMyReactor;
            if (_reactor != null && _reactor.CubeGrid != Me.CubeGrid) _reactor = null;

            _batteries.Clear();
            GridTerminalSystem.GetBlocksOfType(_batteries, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("ALB Battery"));

            _liftThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_liftThrusters, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("ALB Thruster Lift"));

            _maneuverThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_maneuverThrusters, b => b.CubeGrid == Me.CubeGrid && (b.CustomName.Contains("ALB Thruster Fwd") || b.CustomName.Contains("ALB Thruster Rev") || b.CustomName.Contains("ALB Thruster Port") || b.CustomName.Contains("ALB Thruster Stbd") || b.CustomName.Contains("ALB Thruster Down")));

            _workLights.Clear();
            GridTerminalSystem.GetBlocksOfType(_workLights, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("ALB Light Work"));
        }

        void InitNetwork()
        {
            _repairCallListener = IGC.RegisterBroadcastListener("TMS.REPAIR.CALL");
            _repairStatusListener = IGC.RegisterBroadcastListener("TMS.REPAIR.STATUS");
        }

        void RenderEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("Drone.Albatross (Utility) · ACTIVE\n");
            _sbEcho.Append("State: ");
            _sbEcho.Append(_flightState);
            _sbEcho.Append("\nWatchdog: ");
            _sbEcho.Append(_watchdog.IsTimedOut ? "TIMED_OUT\n" : "SYNCED\n");
            _sbEcho.Append("Mass (t): ");
            SbFormat.AppendDouble(_sbEcho, _grossMassKg * 0.001, 2);
            _sbEcho.Append("\nComponent Hold: ");
            SbFormat.AppendDouble(_sbEcho, _holdVolumePct, 1);
            _sbEcho.Append("% (");
            SbFormat.AppendDouble(_sbEcho, _holdCurrentMassKg * 0.001, 2);
            _sbEcho.Append(" t)\nBattery: ");
            SbFormat.AppendDouble(_sbEcho, _batteryStoredMWh, 2);
            _sbEcho.Append(" / ");
            SbFormat.AppendDouble(_sbEcho, _batteryMaxMWh, 2);
            _sbEcho.Append(" MWh (");
            SbFormat.AppendDouble(_sbEcho, _batteryPct, 1);
            _sbEcho.Append("%)\nRepairs: ");
            SbFormat.AppendInt(_sbEcho, _repairsCompleted);
            _sbEcho.Append(" Completed");
            Echo(_sbEcho.ToString());
        }
    }
}
