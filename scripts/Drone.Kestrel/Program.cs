// Drone.Kestrel · Space Engineers 1 MDK2 PB Script
// Target Block: [KES] PB Scout
// Grid: Small Grid (7 × 9 × 5 sb) · Update: Update10 + Update100
// Subsystem Groups: KES Thrusters All, KES Thrusters Lift, KES Thrusters Maneuver, KES Batteries, KES Lights, KES Cameras
// IGC Channels: TMS.SCOUT.STATUS (out), TMS.SCOUT.VEIN (out), TMS.SCOUT.RECALL (in), TMS.SCOUT.TASK (in)
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
        IMyOreDetector _oreDetector;
        IMyRadioAntenna _antenna;
        IMyReactor _reactor;
        readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(2);
        readonly List<IMyThrust> _liftThrusters = new List<IMyThrust>(4);
        readonly List<IMyThrust> _maneuverThrusters = new List<IMyThrust>(10);
        readonly List<IMyThrust> _pushDownThrusters = new List<IMyThrust>(2);

        // IGC Listeners
        IMyBroadcastListener _recallListener;
        IMyBroadcastListener _taskListener;

        // Operational State & Telemetry
        string _flightState = "IDLE_DOCKED";
        string _lastVeinGps = "GPS:NONE:0:0:0:";
        string _assignedPadGps = "GPS:PAD:0:0:0:";
        double _grossMassKg = 4800.0;
        double _batteryStoredMWh;
        double _batteryMaxMWh = 1.0;
        float _batteryPct = 100.0f;
        int _veinsDiscovered;
        bool _rechargeMode;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Drone.Kestrel (Scout) Initialized.");
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
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Assess energy reserves and mass
            EvaluateShipMassAndPower();

            // Check battery threshold for autonomous RTH
            if (_batteryPct < 20.0f && string.Compare(_flightState, "SURVEYING", StringComparison.OrdinalIgnoreCase) == 0)
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
                if (string.Compare(_flightState, "SURVEYING", StringComparison.OrdinalIgnoreCase) != 0 &&
                    string.Compare(_flightState, "TAKEOFF", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    _flightState = "IDLE_DOCKED";
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
            _sbIgc.Append("KES-01;");
            _sbIgc.Append(_flightState);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _batteryPct, 1);
            _sbIgc.Append(";");
            if (_remoteControl != null)
            {
                _sbIgc.Append("GPS:KES-01:");
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

            IGC.SendBroadcastMessage("TMS.SCOUT.STATUS", _sbIgc.ToString());
        }

        void BroadcastVein(string oreType, int depthM, string gpsCoords)
        {
            _veinsDiscovered++;
            _lastVeinGps = gpsCoords;

            _sbIgc.Clear();
            _sbIgc.Append(oreType);
            _sbIgc.Append(";");
            SbFormat.AppendInt(_sbIgc, depthM);
            _sbIgc.Append(";");
            _sbIgc.Append(gpsCoords);
            _sbIgc.Append(";HIGH");

            IGC.SendBroadcastMessage("TMS.SCOUT.VEIN", _sbIgc.ToString());
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
            ProcessListener(_recallListener);
            ProcessListener(_taskListener);
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

            if (tag == "TMS.SCOUT.RECALL")
            {
                // RECALL;KES-01;carrier_id;pad_gps
                if (_args.Count >= 2 && string.Compare(_args[1], "KES-01", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _watchdog.Ping();
                    if (_args.Count > 3) _assignedPadGps = _args[3];
                    ExecuteRth();
                }
            }
            else if (tag == "TMS.SCOUT.TASK")
            {
                // TASK;KES-01;SURVEY;target_gps
                if (_args.Count >= 2 && string.Compare(_args[1], "KES-01", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _watchdog.Ping();
                    ExecuteSurvey();
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
            else if (string.Compare(cmd, "SURVEY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ExecuteSurvey();
            }
            else if (string.Compare(cmd, "RTH", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ExecuteRth();
            }
            else if (string.Compare(cmd, "REPORT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                BroadcastVein("URANIUM", 35, _lastVeinGps);
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

        void ExecuteSurvey()
        {
            _flightState = "SURVEYING";
            SetBatteriesRecharge(false);
            SetThrustersEnabled(_liftThrusters, true);
            SetThrustersEnabled(_maneuverThrusters, true);

            if (_dockConn != null && _dockConn.Status == MyShipConnectorStatus.Connected)
            {
                _dockConn.Disconnect();
            }

            if (_oreDetector != null)
            {
                _oreDetector.Enabled = true;
            }
        }

        void ExecuteRth()
        {
            _flightState = "RTH";
            if (_remoteControl != null)
            {
                _remoteControl.DampenersOverride = true;
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
                _assignedPadGps = _ini.Get("Navigation", "AssignedPadGps").ToString(_assignedPadGps);
            }
        }

        void InitHardware()
        {
            _dockConn = GridTerminalSystem.GetBlockWithName("KES Dock Conn") as IMyShipConnector;
            if (_dockConn != null && _dockConn.CubeGrid != Me.CubeGrid) _dockConn = null;

            _remoteControl = GridTerminalSystem.GetBlockWithName("KES Remote Control") as IMyRemoteControl;
            if (_remoteControl != null && _remoteControl.CubeGrid != Me.CubeGrid) _remoteControl = null;

            _gyro = GridTerminalSystem.GetBlockWithName("KES Gyro") as IMyGyro;
            if (_gyro != null && _gyro.CubeGrid != Me.CubeGrid) _gyro = null;

            _oreDetector = GridTerminalSystem.GetBlockWithName("KES Ore Detector") as IMyOreDetector;
            if (_oreDetector != null && _oreDetector.CubeGrid != Me.CubeGrid) _oreDetector = null;

            _antenna = GridTerminalSystem.GetBlockWithName("KES Antenna") as IMyRadioAntenna;
            if (_antenna != null && _antenna.CubeGrid != Me.CubeGrid) _antenna = null;

            _reactor = GridTerminalSystem.GetBlockWithName("KES Reactor") as IMyReactor;
            if (_reactor != null && _reactor.CubeGrid != Me.CubeGrid) _reactor = null;

            _batteries.Clear();
            GridTerminalSystem.GetBlocksOfType(_batteries, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("KES Battery"));

            _liftThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_liftThrusters, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("KES Thruster Lift"));

            _maneuverThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_maneuverThrusters, b => b.CubeGrid == Me.CubeGrid && (b.CustomName.Contains("KES Thruster Fwd") || b.CustomName.Contains("KES Thruster Rev") || b.CustomName.Contains("KES Thruster Port") || b.CustomName.Contains("KES Thruster Stbd")));

            _pushDownThrusters.Clear();
            GridTerminalSystem.GetBlocksOfType(_pushDownThrusters, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("KES Thruster Down"));
        }

        void InitNetwork()
        {
            _recallListener = IGC.RegisterBroadcastListener("TMS.SCOUT.RECALL");
            _taskListener = IGC.RegisterBroadcastListener("TMS.SCOUT.TASK");
        }

        void RenderEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("Drone.Kestrel (Scout) · ACTIVE\n");
            _sbEcho.Append("State: ");
            _sbEcho.Append(_flightState);
            _sbEcho.Append("\nWatchdog: ");
            _sbEcho.Append(_watchdog.IsTimedOut ? "TIMED_OUT\n" : "SYNCED\n");
            _sbEcho.Append("Mass (t): ");
            SbFormat.AppendDouble(_sbEcho, _grossMassKg * 0.001, 2);
            _sbEcho.Append("\nBattery: ");
            SbFormat.AppendDouble(_sbEcho, _batteryStoredMWh, 2);
            _sbEcho.Append(" / ");
            SbFormat.AppendDouble(_sbEcho, _batteryMaxMWh, 2);
            _sbEcho.Append(" MWh (");
            SbFormat.AppendDouble(_sbEcho, _batteryPct, 1);
            _sbEcho.Append("%)\nVeins Discovered: ");
            SbFormat.AppendInt(_sbEcho, _veinsDiscovered);
            _sbEcho.Append("\nLift: 4× Small Atmo (320 kN · T/W 6.8)");
            Echo(_sbEcho.ToString());
        }
    }
}
