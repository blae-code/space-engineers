// Rig.OS · Space Engineers 1 MDK2 PB Script
// Target Block: [RIG] PB Rig OS
// Grid: Large Grid (25 kt Floating Rig Fortress) · Update: Update10 + Update100
// Subsystem Groups: RIG Lift Atmo All, RIG Lift H2 All, RIG Reactors, RIG Batteries, RIG Gyros, RIG Pylon Sockets
// IGC Channels: RIG.OS.HEARTBEAT (out), RIG.OS.MODE (out), RIG.OS.STATUS (out), RIG.OS.COMMAND (in), TMS.BASE.HANDOVER (in)
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
        const string MODE_STATIONKEEPING = "STATIONKEEPING";
        const string MODE_TRANSIT = "TRANSIT";
        const string MODE_DEFENSE = "DEFENSE";
        const string MODE_ECO = "ECO";
        const string MODE_EMERGENCY = "EMERGENCY";

        const double STANDARD_GRAVITY = 9.81;
        const double TARGET_HOVER_THROTTLE = 0.728; // 72.8% at sea level Earth 1 g

        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(1024);
        readonly StringBuilder _sbIgc = new StringBuilder(512);
        readonly StringBuilder _sbLcd = new StringBuilder(2048);
        readonly List<string> _args = new List<string>(16);
        readonly HeartbeatWatchdog _watchdog = new HeartbeatWatchdog(5.0);

        int _tickFast;
        int _tickSlow;

        // Terminal Block Handles (Grid-scoped: b => b.CubeGrid == Me.CubeGrid)
        readonly List<IMyThrust> _atmoLift = new List<IMyThrust>(56);
        readonly List<IMyThrust> _h2Lift = new List<IMyThrust>(32);
        readonly List<IMyReactor> _reactors = new List<IMyReactor>(4);
        readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(20);
        readonly List<IMyGyro> _gyros = new List<IMyGyro>(28);
        readonly List<IMyShipMergeBlock> _pylonMerges = new List<IMyShipMergeBlock>(14);
        readonly List<IMyShipConnector> _pylonConnectors = new List<IMyShipConnector>(8);
        IMyRemoteControl _remoteControl;
        IMyRadioAntenna _antenna;
        IMyTextPanel _bridgeLcd;

        // IGC Listeners
        IMyBroadcastListener _cmdListener;
        IMyBroadcastListener _handoverListener;

        // Operational State & Telemetry
        string _operatingMode = MODE_STATIONKEEPING;
        double _fortressMassKg = 25000000.0;
        double _requiredHoverThrustN;
        double _currentAtmoThrustMaxN;
        double _currentH2ThrustMaxN;
        float _atmoThrottlePct;
        double _reactorOutputMW;
        double _reactorMaxMW = 900.0;
        double _batteryStoredMWh;
        double _batteryMaxMWh = 54.0;
        float _batteryPct = 100.0f;
        int _connectedPylons = 6;
        bool _h2EmergencyActive;
        bool _n1ReactorTrip;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Rig.OS (Master) Initialized.");
        }

        public void Save()
        {
            Storage = _operatingMode;
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
                _tickFast++;
                UpdateFastCycle();
            }

            if ((updateSource & UpdateType.Update100) != 0)
            {
                _tickSlow++;
                UpdateSlowCycle();
            }
        }

        void UpdateFastCycle()
        {
            _watchdog.Tick(1.0 / 6.0);
            DrainNetwork();
            MeasureHoverKinematics();
            MonitorPowerGrid();
            UpdateFlightSafety();
        }

        void UpdateSlowCycle()
        {
            BroadcastTelemetry();
            UpdateDisplays();
            RenderMonospaceEcho();
        }

        void MeasureHoverKinematics()
        {
            if (_remoteControl != null)
            {
                MyShipMass mass = _remoteControl.CalculateShipMass();
                _fortressMassKg = (double)mass.PhysicalMass;
            }

            _requiredHoverThrustN = _fortressMassKg * STANDARD_GRAVITY;

            double atmoMax = 0.0;
            for (int i = 0; i < _atmoLift.Count; i++)
            {
                IMyThrust t = _atmoLift[i];
                if (t != null && t.IsWorking)
                {
                    atmoMax += (double)t.MaxEffectiveThrust;
                }
            }
            _currentAtmoThrustMaxN = atmoMax;

            double h2Max = 0.0;
            for (int i = 0; i < _h2Lift.Count; i++)
            {
                IMyThrust t = _h2Lift[i];
                if (t != null && t.IsWorking)
                {
                    h2Max += (double)t.MaxEffectiveThrust;
                }
            }
            _currentH2ThrustMaxN = h2Max;

            if (_currentAtmoThrustMaxN > 1000.0)
            {
                _atmoThrottlePct = (float)(_requiredHoverThrustN / _currentAtmoThrustMaxN * 100.0);
            }
            else
            {
                _atmoThrottlePct = 100.0f;
            }
        }

        void MonitorPowerGrid()
        {
            double rOutput = 0.0;
            double rMax = 0.0;
            for (int i = 0; i < _reactors.Count; i++)
            {
                IMyReactor r = _reactors[i];
                if (r != null && r.IsWorking)
                {
                    rOutput += (double)r.CurrentOutput;
                    rMax += (double)r.MaxOutput;
                }
            }
            _reactorOutputMW = rOutput;
            _reactorMaxMW = rMax > 0.001 ? rMax : 900.0;

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
            _batteryMaxMWh = bMax > 0.001 ? bMax : 54.0;
            _batteryPct = (float)(_batteryStoredMWh / _batteryMaxMWh * 100.0);

            // N-1 Reactor Trip detection: Max available nuclear power < 600 MW
            if (_reactorMaxMW < 600.0)
            {
                _n1ReactorTrip = true;
                SetBatteryDischarge(true);
            }
            else
            {
                _n1ReactorTrip = false;
                SetBatteryDischarge(false);
            }

            // Pylon connection audit
            int socketsLocked = 0;
            for (int i = 0; i < _pylonConnectors.Count; i++)
            {
                IMyShipConnector c = _pylonConnectors[i];
                if (c != null && c.Status == MyShipConnectorStatus.Connected)
                {
                    socketsLocked++;
                }
            }
            _connectedPylons = socketsLocked;
        }

        void UpdateFlightSafety()
        {
            // Auto-engage H2 lift boost if atmo throttle exceeds 92% (density loss at altitude)
            // or if operating mode is DEFENSE or EMERGENCY
            if (_operatingMode == MODE_EMERGENCY || _operatingMode == MODE_DEFENSE || _atmoThrottlePct > 92.0f)
            {
                _h2EmergencyActive = true;
                SetH2LiftEnabled(true);
            }
            else
            {
                _h2EmergencyActive = false;
                SetH2LiftEnabled(false);
            }
        }

        void SetH2LiftEnabled(bool enable)
        {
            for (int i = 0; i < _h2Lift.Count; i++)
            {
                IMyThrust t = _h2Lift[i];
                if (t != null)
                {
                    t.Enabled = enable;
                }
            }
        }

        void SetBatteryDischarge(bool discharge)
        {
            for (int i = 0; i < _batteries.Count; i++)
            {
                IMyBatteryBlock b = _batteries[i];
                if (b != null)
                {
                    b.ChargeMode = discharge ? ChargeMode.Discharge : ChargeMode.Auto;
                }
            }
        }

        void BroadcastTelemetry()
        {
            if (IGC == null) return;

            // Heartbeat
            _sbIgc.Clear();
            _sbIgc.Append("RIG;");
            _sbIgc.Append(_operatingMode);
            IGC.SendBroadcastMessage("RIG.OS.HEARTBEAT", _sbIgc.ToString());

            // Master Status
            _sbIgc.Clear();
            _sbIgc.Append("HOVER;");
            _sbIgc.Append(_operatingMode);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _atmoThrottlePct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _reactorOutputMW, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _batteryPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendInt(_sbIgc, _connectedPylons);

            IGC.SendBroadcastMessage("RIG.OS.STATUS", _sbIgc.ToString());
        }

        void UpdateDisplays()
        {
            if (_bridgeLcd == null) return;

            _sbLcd.Clear();
            _sbLcd.Append("========================================\n");
            _sbLcd.Append(" FLOATING RIG 25 kt FORTRESS · RIG//OS  \n");
            _sbLcd.Append("========================================\n");
            _sbLcd.Append(" MODE:        ");
            _sbLcd.Append(_operatingMode);
            _sbLcd.Append("\n");
            _sbLcd.Append(" ATMO HOVER:  ");
            SbFormat.AppendDouble(_sbLcd, _atmoThrottlePct, 1);
            _sbLcd.Append("% (Target: 72.8%)\n");
            _sbLcd.Append(" LIFT STATUS: [");

            int barWidth = 20;
            int filled = (int)(_atmoThrottlePct / 100.0f * barWidth);
            if (filled < 0) filled = 0;
            if (filled > barWidth) filled = barWidth;
            for (int i = 0; i < filled; i++) _sbLcd.Append('=');
            for (int i = filled; i < barWidth; i++) _sbLcd.Append('.');
            _sbLcd.Append("]\n");

            _sbLcd.Append(" H2 BOOST:    ");
            _sbLcd.Append(_h2EmergencyActive ? "ENGAGED (30× ACTIVE)" : "STANDBY (READY)");
            _sbLcd.Append("\n");
            _sbLcd.Append(" POWER BUS:   ");
            SbFormat.AppendDouble(_sbLcd, _reactorOutputMW, 1);
            _sbLcd.Append(" / ");
            SbFormat.AppendDouble(_sbLcd, _reactorMaxMW, 1);
            _sbLcd.Append(" MW\n");
            _sbLcd.Append(" BATTERY UPS: ");
            SbFormat.AppendDouble(_sbLcd, _batteryPct, 1);
            _sbLcd.Append("% (");
            SbFormat.AppendDouble(_sbLcd, _batteryStoredMWh, 1);
            _sbLcd.Append(" / 54.0 MWh)\n");
            _sbLcd.Append(" PYLON ARMS:  ");
            SbFormat.AppendInt(_sbLcd, _connectedPylons);
            _sbLcd.Append(" / 6 Flown Sockets Locked\n");
            _sbLcd.Append(" N-1 ALARM:   ");
            _sbLcd.Append(_n1ReactorTrip ? "TRIPPED (BATTERY BOOST)" : "CLEAR");
            _sbLcd.Append("\n");
            _sbLcd.Append("========================================\n");

            _bridgeLcd.WriteText(_sbLcd);
        }

        void RenderMonospaceEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" [RIG] PB RIG OS TELEMETRY HUD         \n");
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" MODE:        ");
            _sbEcho.Append(_operatingMode);
            _sbEcho.Append("\n");
            _sbEcho.Append(" HOVER LOAD:  ");
            SbFormat.AppendDouble(_sbEcho, _atmoThrottlePct, 1);
            _sbEcho.Append("%\n");
            _sbEcho.Append(" NUCLEAR:     ");
            SbFormat.AppendDouble(_sbEcho, _reactorOutputMW, 1);
            _sbEcho.Append(" MW\n");
            _sbEcho.Append(" BATTERIES:   ");
            SbFormat.AppendDouble(_sbEcho, _batteryPct, 1);
            _sbEcho.Append("%\n");
            _sbEcho.Append(" PYLONS:      ");
            SbFormat.AppendInt(_sbEcho, _connectedPylons);
            _sbEcho.Append(" / 6 Locked\n");
            _sbEcho.Append(" WATCHDOG:    ");
            _sbEcho.Append(_watchdog.IsTimedOut ? "TRIPPED!" : "OK");
            _sbEcho.Append(" (Ticks: ");
            SbFormat.AppendInt(_sbEcho, _tickSlow);
            _sbEcho.Append(")\n");
            _sbEcho.Append("========================================\n");

            Echo(_sbEcho.ToString());
        }

        void DrainNetwork()
        {
            if (_cmdListener != null)
            {
                while (_cmdListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _cmdListener.AcceptMessage();
                    string data = msg.Data as string;
                    if (data != null)
                    {
                        HandleCommand(data);
                    }
                }
            }

            if (_handoverListener != null)
            {
                while (_handoverListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _handoverListener.AcceptMessage();
                    string data = msg.Data as string;
                    if (data != null && data.IndexOf("C2_MASTER", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Echo("C2 Handover Received from Ground Base.");
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
            else if (string.Compare(cmd, "MODE", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                string m = _args[1];
                if (string.Compare(m, "DEFENSE", StringComparison.OrdinalIgnoreCase) == 0)
                    _operatingMode = MODE_DEFENSE;
                else if (string.Compare(m, "TRANSIT", StringComparison.OrdinalIgnoreCase) == 0)
                    _operatingMode = MODE_TRANSIT;
                else if (string.Compare(m, "ECO", StringComparison.OrdinalIgnoreCase) == 0)
                    _operatingMode = MODE_ECO;
                else if (string.Compare(m, "EMERGENCY", StringComparison.OrdinalIgnoreCase) == 0)
                    _operatingMode = MODE_EMERGENCY;
                else
                    _operatingMode = MODE_STATIONKEEPING;
            }
            else if (string.Compare(cmd, "EMERGENCY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _operatingMode = MODE_EMERGENCY;
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
                _operatingMode = _ini.Get("Rig", "DefaultMode").ToString(_operatingMode);
            }
        }

        void InitHardware()
        {
            _atmoLift.Clear();
            _h2Lift.Clear();
            _reactors.Clear();
            _batteries.Clear();
            _gyros.Clear();
            _pylonMerges.Clear();
            _pylonConnectors.Clear();
            _remoteControl = null;
            _antenna = null;
            _bridgeLcd = null;

            GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(null, b =>
            {
                if (b.CubeGrid != Me.CubeGrid) return false;

                string name = b.CustomName;

                if (b is IMyThrust)
                {
                    if (name.IndexOf("Lift Atmo", StringComparison.OrdinalIgnoreCase) >= 0)
                        _atmoLift.Add((IMyThrust)b);
                    else if (name.IndexOf("Lift H2", StringComparison.OrdinalIgnoreCase) >= 0)
                        _h2Lift.Add((IMyThrust)b);
                }
                else if (b is IMyReactor && name.IndexOf("RIG H4 Reactor", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _reactors.Add((IMyReactor)b);
                }
                else if (b is IMyBatteryBlock && name.IndexOf("Battery", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _batteries.Add((IMyBatteryBlock)b);
                }
                else if (b is IMyGyro)
                {
                    _gyros.Add((IMyGyro)b);
                }
                else if (b is IMyShipMergeBlock && name.IndexOf("Socket", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _pylonMerges.Add((IMyShipMergeBlock)b);
                }
                else if (b is IMyShipConnector && name.IndexOf("Socket", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _pylonConnectors.Add((IMyShipConnector)b);
                }
                else if (b is IMyRemoteControl && name.IndexOf("RIG", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _remoteControl = (IMyRemoteControl)b;
                }
                else if (b is IMyRadioAntenna && name.IndexOf("RIG", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _antenna = (IMyRadioAntenna)b;
                }
                else if (b is IMyTextPanel && name.IndexOf("RIG H1 LCD", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _bridgeLcd = (IMyTextPanel)b;
                }

                return false;
            });

            // Fallback group queries
            if (_atmoLift.Count == 0)
            {
                IMyBlockGroup g = GridTerminalSystem.GetBlockGroupWithName("RIG Lift Atmo All");
                if (g != null) g.GetBlocksOfType(_atmoLift);
            }
            if (_h2Lift.Count == 0)
            {
                IMyBlockGroup g = GridTerminalSystem.GetBlockGroupWithName("RIG Lift H2 All");
                if (g != null) g.GetBlocksOfType(_h2Lift);
            }
            if (_reactors.Count == 0)
            {
                IMyBlockGroup g = GridTerminalSystem.GetBlockGroupWithName("RIG Reactors");
                if (g != null) g.GetBlocksOfType(_reactors);
            }
            if (_batteries.Count == 0)
            {
                IMyBlockGroup g = GridTerminalSystem.GetBlockGroupWithName("RIG Batteries");
                if (g != null) g.GetBlocksOfType(_batteries);
            }
        }

        void InitNetwork()
        {
            if (IGC == null) return;
            _cmdListener = IGC.RegisterBroadcastListener("RIG.OS.COMMAND");
            _handoverListener = IGC.RegisterBroadcastListener("TMS.BASE.HANDOVER");
        }
    }
}
