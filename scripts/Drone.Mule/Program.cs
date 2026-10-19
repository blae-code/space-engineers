// Drone.Mule · Space Engineers 1 MDK2 PB Script
// Target Block: [MUL] PB Shuttle
// Grid: Large Grid (7 × 7 B) · Update: Update10 + Update100
// Subsystem Groups: MUL Lift, MUL Maneuver, MUL Batteries, MUL Gyros, MUL Cams
// IGC Channels: TMS.MULE.STATUS (out), TMS.MULE.CALL (in), TMS.LOGISTICS.JIT (in), TMS.MULE.DISPATCH (in)
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
        IMyShipConnector _dockBelly;
        IMyRemoteControl _remoteControl;
        IMyCargoContainer _cargoHold;
        IMyRadioAntenna _antenna;
        readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(4);
        readonly List<IMyThrust> _liftThrusters = new List<IMyThrust>(4);
        readonly List<IMyThrust> _maneuverThrusters = new List<IMyThrust>(10);
        readonly List<IMyThrust> _pushDownThrusters = new List<IMyThrust>(2);
        readonly List<IMyGyro> _gyros = new List<IMyGyro>(2);

        // IGC Listeners
        IMyBroadcastListener _callListener;
        IMyBroadcastListener _jitListener;
        IMyBroadcastListener _dispatchListener;

        // Operational State & Telemetry
        string _flightState = "IDLE_DOCKED";
        string _destinationGps = "GPS:NONE:0:0:0:";
        double _grossMassKg = 157000.0;
        double _payloadOreKg;
        double _batteryStoredMWh;
        double _batteryMaxMWh = 12.0;
        float _batteryPct = 100.0f;
        bool _rechargeMode;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Drone.Mule (Shuttle) Initialized.");
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

            // Process inbound IGC messages
            ProcessInboundIgc();

            // Refresh physical dock status
            RefreshDockState();
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Assess mass, payload, and energy reserves
            EvaluateShipMassAndPower();

            // Broadcast periodic telemetry status
            BroadcastStatus();

            RenderEcho();
        }

        void RefreshDockState()
        {
            if (_dockBelly == null) return;

            if (_dockBelly.Status == MyShipConnectorStatus.Connected)
            {
                if (string.Compare(_flightState, "TRANSIT_OUT", StringComparison.OrdinalIgnoreCase) != 0 &&
                    string.Compare(_flightState, "TAKEOFF", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    _flightState = (_payloadOreKg > 1000.0) ? "DOCKED_LADEN" : "IDLE_DOCKED";
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
                    _payloadOreKg = (double)inv.CurrentMass;
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
            _sbIgc.Append("MUL-01;");
            _sbIgc.Append(_flightState);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _payloadOreKg * 0.001, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _batteryPct, 1);
            _sbIgc.Append(";");
            _sbIgc.Append(_destinationGps);

            IGC.SendBroadcastMessage("TMS.MULE.STATUS", _sbIgc.ToString());
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
            ProcessListener(_callListener);
            ProcessListener(_jitListener);
            ProcessListener(_dispatchListener);
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

            if (tag == "TMS.MULE.CALL" || tag == "TMS.LOGISTICS.JIT" || tag == "TMS.MULE.DISPATCH")
            {
                // DISPATCH;MUL-01;pad_id;payload_type
                if (_args.Count >= 2 && string.Compare(_args[1], "MUL-01", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _watchdog.Ping();
                    if (_args.Count > 3)
                    {
                        _destinationGps = _args[3];
                    }
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
            else if (string.Compare(cmd, "LAUNCH", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ExecuteLaunch();
            }
            else if (string.Compare(cmd, "DOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ExecuteDock();
            }
            else if (string.Compare(cmd, "EMERGENCY_HOVER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ExecuteEmergencyHover();
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

        void ExecuteLaunch()
        {
            _flightState = "TAKEOFF";
            SetBatteriesRecharge(false);
            SetThrustersEnabled(_liftThrusters, true);
            SetThrustersEnabled(_maneuverThrusters, true);

            if (_dockBelly != null && _dockBelly.Status == MyShipConnectorStatus.Connected)
            {
                _dockBelly.Disconnect();
            }
        }

        void ExecuteDock()
        {
            _flightState = "APPROACH";
            if (_dockBelly != null && _dockBelly.Status == MyShipConnectorStatus.Connectable)
            {
                _dockBelly.Connect();
                _flightState = "IDLE_DOCKED";
                SetBatteriesRecharge(true);
            }
        }

        void ExecuteEmergencyHover()
        {
            _flightState = "EMERGENCY_HOVER";
            if (_remoteControl != null)
            {
                _remoteControl.DampenersOverride = true;
                _remoteControl.SetAutoPilotEnabled(false);
            }
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
                _destinationGps = _ini.Get("Navigation", "DestinationGps").ToString(_destinationGps);
            }
        }

        void InitHardware()
        {
            _dockBelly = GridTerminalSystem.GetBlockWithName("MUL Dock Belly") as IMyShipConnector;
            if (_dockBelly != null && _dockBelly.CubeGrid != Me.CubeGrid) _dockBelly = null;

            _remoteControl = GridTerminalSystem.GetBlockWithName("MUL RC") as IMyRemoteControl;
            if (_remoteControl != null && _remoteControl.CubeGrid != Me.CubeGrid) _remoteControl = null;

            _cargoHold = GridTerminalSystem.GetBlockWithName("MUL Cargo Hold") as IMyCargoContainer;
            if (_cargoHold != null && _cargoHold.CubeGrid != Me.CubeGrid) _cargoHold = null;

            _antenna = GridTerminalSystem.GetBlockWithName("MUL Antenna") as IMyRadioAntenna;
            if (_antenna != null && _antenna.CubeGrid != Me.CubeGrid) _antenna = null;

            _batteries.Clear();
            GridTerminalSystem.GetBlocksOfType(_batteries, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("MUL Battery"));

            _liftThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_liftThrusters, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("MUL Lift"));

            _maneuverThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_maneuverThrusters, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("MUL Lat"));

            _pushDownThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_pushDownThrusters, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("MUL Thrust Down"));

            _gyros.Clear();
            GridTerminalSystem.GetBlocksOfType(_gyros, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("MUL Gyro"));
        }

        void InitNetwork()
        {
            _callListener = IGC.RegisterBroadcastListener("TMS.MULE.CALL");
            _jitListener = IGC.RegisterBroadcastListener("TMS.LOGISTICS.JIT");
            _dispatchListener = IGC.RegisterBroadcastListener("TMS.MULE.DISPATCH");
        }

        void RenderEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("Drone.Mule (Shuttle) · ACTIVE\n");
            _sbEcho.Append("State: ");
            _sbEcho.Append(_flightState);
            _sbEcho.Append("\nWatchdog: ");
            _sbEcho.Append(_watchdog.IsTimedOut ? "TIMED_OUT\n" : "SYNCED\n");
            _sbEcho.Append("Gross Mass (t): ");
            SbFormat.AppendDouble(_sbEcho, _grossMassKg * 0.001, 1);
            _sbEcho.Append("\nPayload Ore (t): ");
            SbFormat.AppendDouble(_sbEcho, _payloadOreKg * 0.001, 1);
            _sbEcho.Append("\nBattery: ");
            SbFormat.AppendDouble(_sbEcho, _batteryStoredMWh, 1);
            _sbEcho.Append(" / ");
            SbFormat.AppendDouble(_sbEcho, _batteryMaxMWh, 1);
            _sbEcho.Append(" MWh (");
            SbFormat.AppendDouble(_sbEcho, _batteryPct, 1);
            _sbEcho.Append("%)\nLift Array: ");
            SbFormat.AppendInt(_sbEcho, _liftThrusters.Count);
            _sbEcho.Append(" Large Flat Atmo (10.4 MN)");
            Echo(_sbEcho.ToString());
        }
    }
}
