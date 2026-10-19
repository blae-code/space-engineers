// Yard.Extruder · Space Engineers 1 MDK2 PB Script
// Target Block: [EXT] PB Yard Master
// Grid: Large Grid (12 × 78 B Shipyard) · Update: Update10 + Update100
// Subsystem Groups: EXT Welder Ring, EXT Pushers, EXT Yard Displays
// IGC Channels: RIG.EXT.STATUS (out), RIG.EXT.COMMAND (in), RIG.EXT.COMMISSION (out), TMS.BASE.QUOTAS (in)
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
        const string STATE_IDLE = "IDLE";
        const string STATE_ROOT_WELD = "ROOT_WELD";
        const string STATE_EXTRUDING = "EXTRUDING";
        const string STATE_PAUSED = "PAUSED";
        const string STATE_WELD_COMPLETE = "WELD_COMPLETE";
        const string STATE_FUELING = "FUELING";
        const string STATE_COMMISSIONING = "COMMISSIONING";
        const string STATE_RETRACTING = "RETRACTING";

        const double MAX_STROKE_METERS = 60.0;
        const float EXTRUSION_VELOCITY = 0.05f;
        const float RETRACT_VELOCITY = -0.20f;
        const float UMBILICAL_STOWED_RAD = 0.0f;
        const float UMBILICAL_ENGAGED_RAD = 0.785398f; // 45 degrees

        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(1024);
        readonly StringBuilder _sbIgc = new StringBuilder(512);
        readonly StringBuilder _sbLcd = new StringBuilder(2048);
        readonly List<string> _args = new List<string>(16);
        readonly HeartbeatWatchdog _watchdog = new HeartbeatWatchdog(5.0);

        int _tickFast;
        int _tickSlow;

        // Terminal Block Handles
        readonly List<IMyPistonBase> _pushers = new List<IMyPistonBase>(6);
        readonly List<IMyShipWelder> _welders = new List<IMyShipWelder>(24);
        IMyShipMergeBlock _mergeRelease;
        IMyProjector _prjHead;
        IMyConveyorSorter _sorterFeed;
        IMyShipConnector _fuelConn;
        IMyMotorAdvancedStator _umbilicalHinge;
        IMySoundBlock _soundAlert;
        IMyTextPanel _lcdStatus;

        // IGC Listeners
        IMyBroadcastListener _cmdListener;
        IMyBroadcastListener _quotaListener;

        // Operational State & Telemetry
        string _yardState = STATE_IDLE;
        string _activeBlueprint = "RIG-PYLON-S1";
        double _currentStrokeMeters;
        float _progressPct;
        double _estTimeRemainingSec;
        int _weldersActive;
        bool _umbilicalLocked;
        bool _safetyHold;
        int _stateTimerTicks;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Yard.Extruder (Master) Initialized.");
        }

        public void Save()
        {
            Storage = _yardState;
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
            MeasureExtrusionKinematics();
            UpdateStateMachine();
        }

        void UpdateSlowCycle()
        {
            BroadcastTelemetry();
            UpdateDisplays();
            RenderMonospaceEcho();
        }

        void MeasureExtrusionKinematics()
        {
            double stroke = 0.0;
            for (int i = 0; i < _pushers.Count; i++)
            {
                IMyPistonBase p = _pushers[i];
                if (p != null)
                {
                    stroke += (double)p.CurrentPosition;
                }
            }
            _currentStrokeMeters = stroke;

            if (_currentStrokeMeters < 0.0) _currentStrokeMeters = 0.0;
            if (_currentStrokeMeters > MAX_STROKE_METERS) _currentStrokeMeters = MAX_STROKE_METERS;

            _progressPct = (float)(_currentStrokeMeters / MAX_STROKE_METERS * 100.0);

            if (_yardState == STATE_EXTRUDING)
            {
                double remainingMeters = MAX_STROKE_METERS - _currentStrokeMeters;
                _estTimeRemainingSec = remainingMeters / (double)EXTRUSION_VELOCITY;
            }
            else
            {
                _estTimeRemainingSec = 0.0;
            }

            int wCount = 0;
            for (int i = 0; i < _welders.Count; i++)
            {
                IMyShipWelder w = _welders[i];
                if (w != null && w.Enabled)
                {
                    wCount++;
                }
            }
            _weldersActive = wCount;

            if (_fuelConn != null)
            {
                _umbilicalLocked = (_fuelConn.Status == MyShipConnectorStatus.Connected);
            }
        }

        void UpdateStateMachine()
        {
            switch (_yardState)
            {
                case STATE_IDLE:
                    SetPusherVelocity(0.0f);
                    SetWelderRing(false);
                    break;

                case STATE_ROOT_WELD:
                    _stateTimerTicks++;
                    SetPusherVelocity(0.0f);
                    SetWelderRing(true);
                    if (_mergeRelease != null) _mergeRelease.Enabled = true;
                    if (_prjHead != null) _prjHead.Enabled = true;

                    // 10 seconds root collar fusion dwell (60 ticks at Update10)
                    if (_stateTimerTicks >= 60)
                    {
                        _stateTimerTicks = 0;
                        _yardState = STATE_EXTRUDING;
                    }
                    break;

                case STATE_EXTRUDING:
                    if (_safetyHold)
                    {
                        SetPusherVelocity(0.0f);
                        SetWelderRing(false);
                    }
                    else
                    {
                        SetPusherVelocity(EXTRUSION_VELOCITY);
                        SetWelderRing(true);

                        if (_currentStrokeMeters >= (MAX_STROKE_METERS - 0.05))
                        {
                            SetPusherVelocity(0.0f);
                            SetWelderRing(false);
                            _yardState = STATE_WELD_COMPLETE;
                            if (_soundAlert != null) _soundAlert.Play();
                        }
                    }
                    break;

                case STATE_PAUSED:
                    SetPusherVelocity(0.0f);
                    SetWelderRing(false);
                    break;

                case STATE_WELD_COMPLETE:
                    SetPusherVelocity(0.0f);
                    SetWelderRing(false);
                    break;

                case STATE_FUELING:
                    SetPusherVelocity(0.0f);
                    SetWelderRing(false);
                    // Extend umbilical hinge
                    if (_umbilicalHinge != null)
                    {
                        _umbilicalHinge.TargetVelocityRad = 0.2f;
                        _umbilicalHinge.UpperLimitRad = UMBILICAL_ENGAGED_RAD;
                    }
                    if (_fuelConn != null && _fuelConn.Status == MyShipConnectorStatus.Connectable)
                    {
                        _fuelConn.Connect();
                    }
                    break;

                case STATE_COMMISSIONING:
                    _stateTimerTicks++;
                    // Disengage umbilical
                    if (_fuelConn != null && _fuelConn.Status == MyShipConnectorStatus.Connected)
                    {
                        _fuelConn.Disconnect();
                    }
                    if (_umbilicalHinge != null)
                    {
                        _umbilicalHinge.TargetVelocityRad = -0.2f;
                        _umbilicalHinge.LowerLimitRad = UMBILICAL_STOWED_RAD;
                    }

                    // 5-second commissioning sequence (30 ticks)
                    if (_stateTimerTicks == 10)
                    {
                        SendCommissionPulse();
                    }
                    else if (_stateTimerTicks >= 30)
                    {
                        if (_mergeRelease != null) _mergeRelease.Enabled = false;
                        _stateTimerTicks = 0;
                        _yardState = STATE_RETRACTING;
                    }
                    break;

                case STATE_RETRACTING:
                    SetWelderRing(false);
                    if (_currentStrokeMeters <= 0.1)
                    {
                        SetPusherVelocity(0.0f);
                        _yardState = STATE_IDLE;
                    }
                    else
                    {
                        SetPusherVelocity(RETRACT_VELOCITY);
                    }
                    break;
            }
        }

        void SetPusherVelocity(float vel)
        {
            for (int i = 0; i < _pushers.Count; i++)
            {
                IMyPistonBase p = _pushers[i];
                if (p != null)
                {
                    p.Velocity = vel;
                }
            }
        }

        void SetWelderRing(bool enable)
        {
            for (int i = 0; i < _welders.Count; i++)
            {
                IMyShipWelder w = _welders[i];
                if (w != null)
                {
                    w.Enabled = enable;
                }
            }
        }

        void SendCommissionPulse()
        {
            if (IGC == null) return;
            _sbIgc.Clear();
            _sbIgc.Append("COMMISSION;");
            _sbIgc.Append(_activeBlueprint);
            IGC.SendBroadcastMessage("RIG.EXT.COMMISSION", _sbIgc.ToString());
        }

        void BroadcastTelemetry()
        {
            if (IGC == null) return;

            _sbIgc.Clear();
            _sbIgc.Append("EXTRUDING;");
            _sbIgc.Append(_activeBlueprint);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _progressPct, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _estTimeRemainingSec, 0);
            _sbIgc.Append(";");
            _sbIgc.Append(_yardState);

            IGC.SendBroadcastMessage("RIG.EXT.STATUS", _sbIgc.ToString());
        }

        void UpdateDisplays()
        {
            if (_lcdStatus == null) return;

            _sbLcd.Clear();
            _sbLcd.Append("========================================\n");
            _sbLcd.Append(" RIG LINEAR EXTRUDER SHIPYARD STATUS   \n");
            _sbLcd.Append("========================================\n");
            _sbLcd.Append(" STATE:       ");
            _sbLcd.Append(_yardState);
            _sbLcd.Append("\n");
            _sbLcd.Append(" BLUEPRINT:   ");
            _sbLcd.Append(_activeBlueprint);
            _sbLcd.Append("\n");
            _sbLcd.Append(" STROKE:      ");
            SbFormat.AppendDouble(_sbLcd, _currentStrokeMeters, 2);
            _sbLcd.Append(" / 60.00 m\n");
            _sbLcd.Append(" PROGRESS:    [");

            int barWidth = 20;
            int filled = (int)(_progressPct / 100.0f * barWidth);
            if (filled < 0) filled = 0;
            if (filled > barWidth) filled = barWidth;
            for (int i = 0; i < filled; i++) _sbLcd.Append('=');
            for (int i = filled; i < barWidth; i++) _sbLcd.Append('.');
            _sbLcd.Append("] ");
            SbFormat.AppendDouble(_sbLcd, _progressPct, 1);
            _sbLcd.Append("%\n");

            _sbLcd.Append(" EST TIME:    ");
            SbFormat.AppendDouble(_sbLcd, _estTimeRemainingSec, 0);
            _sbLcd.Append(" s\n");
            _sbLcd.Append(" WELDERS:     ");
            SbFormat.AppendInt(_sbLcd, _weldersActive);
            _sbLcd.Append(" / ");
            SbFormat.AppendInt(_sbLcd, _welders.Count);
            _sbLcd.Append(" ACTIVE\n");
            _sbLcd.Append(" UMBILICAL:   ");
            _sbLcd.Append(_umbilicalLocked ? "LOCKED (REFUELING)" : "STOWED / READY");
            _sbLcd.Append("\n");
            _sbLcd.Append(" SAFETY HOLD: ");
            _sbLcd.Append(_safetyHold ? "ACTIVE (LOW STOCK)" : "CLEAR");
            _sbLcd.Append("\n");
            _sbLcd.Append("========================================\n");

            _lcdStatus.WriteText(_sbLcd);
        }

        void RenderMonospaceEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" [EXT] PB YARD MASTER TELEMETRY        \n");
            _sbEcho.Append("========================================\n");
            _sbEcho.Append(" STATE:       ");
            _sbEcho.Append(_yardState);
            _sbEcho.Append("\n");
            _sbEcho.Append(" STROKE:      ");
            SbFormat.AppendDouble(_sbEcho, _currentStrokeMeters, 2);
            _sbEcho.Append(" m (");
            SbFormat.AppendDouble(_sbEcho, _progressPct, 1);
            _sbEcho.Append("%)\n");
            _sbEcho.Append(" WELDERS:     ");
            SbFormat.AppendInt(_sbEcho, _weldersActive);
            _sbEcho.Append(" Active\n");
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

            if (_quotaListener != null)
            {
                while (_quotaListener.HasPendingMessage)
                {
                    MyIGCMessage msg = _quotaListener.AcceptMessage();
                    string data = msg.Data as string;
                    if (data != null)
                    {
                        // Check quota payload for low stock flag
                        if (data.IndexOf("LOW_STOCK", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            _safetyHold = true;
                        }
                        else if (data.IndexOf("STOCK_OK", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            _safetyHold = false;
                        }
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
            else if (string.Compare(cmd, "START", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1)
                {
                    _activeBlueprint = _args[1];
                }
                _stateTimerTicks = 0;
                _yardState = STATE_ROOT_WELD;
            }
            else if (string.Compare(cmd, "PAUSE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _yardState = STATE_PAUSED;
            }
            else if (string.Compare(cmd, "RESUME", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_currentStrokeMeters < (MAX_STROKE_METERS - 0.05))
                {
                    _yardState = STATE_EXTRUDING;
                }
            }
            else if (string.Compare(cmd, "ABORT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _yardState = STATE_IDLE;
            }
            else if (string.Compare(cmd, "FUEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _yardState = STATE_FUELING;
            }
            else if (string.Compare(cmd, "COMMISSION", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _stateTimerTicks = 0;
                _yardState = STATE_COMMISSIONING;
            }
            else if (string.Compare(cmd, "RETRACT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _yardState = STATE_RETRACTING;
            }
            else if (string.Compare(cmd, "RESET", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _yardState = STATE_IDLE;
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
                _activeBlueprint = _ini.Get("Yard", "DefaultBlueprint").ToString(_activeBlueprint);
            }
        }

        void InitHardware()
        {
            _pushers.Clear();
            _welders.Clear();
            _mergeRelease = null;
            _prjHead = null;
            _sorterFeed = null;
            _fuelConn = null;
            _umbilicalHinge = null;
            _soundAlert = null;
            _lcdStatus = null;

            // Scoped queries on primary yard grid
            GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(null, b =>
            {
                if (b.CubeGrid != Me.CubeGrid) return false;

                string name = b.CustomName;

                if (b is IMyPistonBase && name.IndexOf("EXT Pusher", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _pushers.Add((IMyPistonBase)b);
                }
                else if (b is IMyShipWelder && name.IndexOf("EXT WLD", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _welders.Add((IMyShipWelder)b);
                }
                else if (b is IMyConveyorSorter && name.IndexOf("EXT Sorter Feed", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _sorterFeed = (IMyConveyorSorter)b;
                }
                else if (b is IMyMotorAdvancedStator && name.IndexOf("EXT Umbilical Hinge", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _umbilicalHinge = (IMyMotorAdvancedStator)b;
                }
                else if (b is IMySoundBlock && name.IndexOf("EXT Sound Alert", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _soundAlert = (IMySoundBlock)b;
                }
                else if (b is IMyTextPanel && name.IndexOf("EXT LCD Status", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _lcdStatus = (IMyTextPanel)b;
                }

                return false;
            });

            // Locate pusher head and umbilical tip blocks on mechanical subgrids by exact name
            _mergeRelease = GridTerminalSystem.GetBlockWithName("EXT MRG Release") as IMyShipMergeBlock;
            _prjHead = GridTerminalSystem.GetBlockWithName("EXT PRJ Head") as IMyProjector;
            _fuelConn = GridTerminalSystem.GetBlockWithName("EXT Fuel Conn") as IMyShipConnector;

            // Fallback: If welders weren't found individually, check for the group
            if (_welders.Count == 0)
            {
                IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName("EXT Welder Ring");
                if (group != null)
                {
                    group.GetBlocksOfType(_welders);
                }
            }

            // Fallback: If pushers weren't found individually, check group
            if (_pushers.Count == 0)
            {
                IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName("EXT Pushers");
                if (group != null)
                {
                    group.GetBlocksOfType(_pushers);
                }
            }
        }

        void InitNetwork()
        {
            if (IGC == null) return;
            _cmdListener = IGC.RegisterBroadcastListener("RIG.EXT.COMMAND");
            _quotaListener = IGC.RegisterBroadcastListener("TMS.BASE.QUOTAS");
        }
    }
}
