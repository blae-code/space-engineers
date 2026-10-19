// Base.C2 · Space Engineers 1 MDK2 PB Script
// Target Block: [GND] PB FleetOps
// Grid: Large Grid (24 × 28 B Surface Station) · Update: Update10 + Update100
// Subsystem Groups: GND PAD, GND CAT, GND REF, GND ASM, GND C2, GND Power Batteries
// IGC Channels: TMS.BASE.DEMAND (out), TMS.LOGISTICS.JIT (out), RIG.C2.STATUS (out), TMS.MULE.CALL (out)
// Incoming: TMS.C2.HANDOVER, TMS.MULE.STATUS, TMS.CAT.STATUS, TMS.MAST.STATUS, TMS.ORCA.STATUS
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
        #region Subsystem Configuration & Telemetry
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(1024);
        readonly StringBuilder _sbIgc = new StringBuilder(512);
        readonly List<string> _args = new List<string>(16);
        readonly HeartbeatWatchdog _watchdog = new HeartbeatWatchdog(5.0);

        int _tickFast;
        int _tickSlow;
        bool _isC2Master;

        // Terminal Block Handles (Grid-scoped: b => b.CubeGrid == Me.CubeGrid)
        IMyShipConnector _padConnector;
        IMyShipConnector _catDumpConnector;
        IMySensorBlock _padSensor;
        IMyBatteryBlock _c2UpsBattery;
        readonly List<IMyBatteryBlock> _substationBatteries = new List<IMyBatteryBlock>(8);
        readonly List<IMyRefinery> _refineries = new List<IMyRefinery>(4);
        readonly List<IMyAssembler> _assemblers = new List<IMyAssembler>(4);
        readonly List<IMyTextPanel> _videoWallLcds = new List<IMyTextPanel>(6);
        IMyLaserAntenna _laserRelay;
        IMyRadioAntenna _fleetAntenna;

        // IGC Listeners
        IMyBroadcastListener _handoverListener;
        IMyBroadcastListener _muleStatusListener;
        IMyBroadcastListener _catStatusListener;
        IMyBroadcastListener _mastStatusListener;
        IMyBroadcastListener _orcaStatusListener;

        // Logistics & Operational Metrics
        string _mulePadStatus = "IDLE";
        string _catDockStatus = "READY";
        string _lastMulePayload = "0.0";
        string _lastCatPayload = "0.0";
        double _batteryStoredMWh;
        double _batteryMaxMWh = 18.0;
        int _activeRefineries;
        int _activeAssemblers;
        bool _muleDispatchRequested;
        bool _catDispatchRequested;

        // Quotas (kg target reserves)
        int _demandUraniumKg = 500;
        int _demandPlatinumKg = 2000;
        int _demandGoldKg = 3000;
        int _demandMagnesiumKg = 5000;
        int _demandIronTons = 2500;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Base.C2 (FleetOps) Initialized.");
        }

        public void Save()
        {
            // Persist master status to Storage
            Storage = _isC2Master ? "MASTER=1" : "MASTER=0";
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
            RefreshDockSensors();
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Assess industrial and substation telemetry
            EvaluateIndustrialGrid();

            // Broadcast demand and C2 state if master
            if (_isC2Master)
            {
                BroadcastC2Status();
                BroadcastBaseDemand();
                EvaluateJitDispatch();
            }

            RenderEcho();
        }

        void RefreshDockSensors()
        {
            if (_padConnector != null)
            {
                if (_padConnector.Status == MyShipConnectorStatus.Connected)
                    _mulePadStatus = "LOCKED / OFFLOAD";
                else if (_padConnector.Status == MyShipConnectorStatus.Connectable)
                    _mulePadStatus = "CRADLE READY";
                else if (_padSensor != null && _padSensor.IsActive)
                    _mulePadStatus = "APPROACHING";
                else
                    _mulePadStatus = "IDLE";
            }

            if (_catDumpConnector != null)
            {
                if (_catDumpConnector.Status == MyShipConnectorStatus.Connected)
                    _catDockStatus = "LOCKED / DUMPING";
                else if (_catDumpConnector.Status == MyShipConnectorStatus.Connectable)
                    _catDockStatus = "ALIGNED";
                else
                    _catDockStatus = "READY";
            }
        }

        void EvaluateIndustrialGrid()
        {
            _batteryStoredMWh = 0.0;
            _batteryMaxMWh = 0.0;
            for (int i = 0; i < _substationBatteries.Count; i++)
            {
                IMyBatteryBlock bat = _substationBatteries[i];
                if (bat != null)
                {
                    _batteryStoredMWh += bat.CurrentStoredPower;
                    _batteryMaxMWh += bat.MaxStoredPower;
                }
            }

            _activeRefineries = 0;
            for (int i = 0; i < _refineries.Count; i++)
            {
                IMyRefinery r = _refineries[i];
                if (r != null && r.IsProducing) _activeRefineries++;
            }

            _activeAssemblers = 0;
            for (int i = 0; i < _assemblers.Count; i++)
            {
                IMyAssembler a = _assemblers[i];
                if (a != null && a.IsProducing) _activeAssemblers++;
            }
        }

        void EvaluateJitDispatch()
        {
            // If refinery active capacity drops and stocks are needed, dispatch transport
            if (_catDispatchRequested)
            {
                _sbIgc.Clear();
                _sbIgc.Append("DISPATCH;CAT-01;TRENCH-1;BULK_ORE");
                IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
                _catDispatchRequested = false;
            }

            if (_muleDispatchRequested)
            {
                _sbIgc.Clear();
                _sbIgc.Append("DISPATCH;MUL-01;PAD-1;RARE_ORE");
                IGC.SendBroadcastMessage("TMS.LOGISTICS.JIT", _sbIgc.ToString());
                _muleDispatchRequested = false;
            }
        }

        void BroadcastC2Status()
        {
            _sbIgc.Clear();
            _sbIgc.Append("GND-01;");
            _sbIgc.Append(_isC2Master ? "C2_MASTER" : "STANDBY");
            _sbIgc.Append(";BATS=");
            SbFormat.AppendDouble(_sbIgc, _batteryStoredMWh, 1);
            _sbIgc.Append("/");
            SbFormat.AppendDouble(_sbIgc, _batteryMaxMWh, 1);
            _sbIgc.Append(";REF=");
            SbFormat.AppendInt(_sbIgc, _activeRefineries);
            _sbIgc.Append(";ASM=");
            SbFormat.AppendInt(_sbIgc, _activeAssemblers);
            _sbIgc.Append(";PAD=");
            _sbIgc.Append(_mulePadStatus);
            _sbIgc.Append(";CAT=");
            _sbIgc.Append(_catDockStatus);

            IGC.SendBroadcastMessage("RIG.C2.STATUS", _sbIgc.ToString());
        }

        void BroadcastBaseDemand()
        {
            _sbIgc.Clear();
            _sbIgc.Append("DEMAND;U=");
            SbFormat.AppendInt(_sbIgc, _demandUraniumKg);
            _sbIgc.Append(";PT=");
            SbFormat.AppendInt(_sbIgc, _demandPlatinumKg);
            _sbIgc.Append(";AU=");
            SbFormat.AppendInt(_sbIgc, _demandGoldKg);
            _sbIgc.Append(";MG=");
            SbFormat.AppendInt(_sbIgc, _demandMagnesiumKg);
            _sbIgc.Append(";FE=");
            SbFormat.AppendInt(_sbIgc, _demandIronTons);

            IGC.SendBroadcastMessage("TMS.BASE.DEMAND", _sbIgc.ToString());
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
            ProcessListener(_handoverListener);
            ProcessListener(_muleStatusListener);
            ProcessListener(_catStatusListener);
            ProcessListener(_mastStatusListener);
            ProcessListener(_orcaStatusListener);
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

            if (tag == "TMS.C2.HANDOVER")
            {
                _isC2Master = true;
                _watchdog.Ping();
            }
            else if (tag == "TMS.MULE.STATUS")
            {
                if (_args.Count > 2) _lastMulePayload = _args[2];
                _watchdog.Ping();
            }
            else if (tag == "TMS.CAT.STATUS")
            {
                if (_args.Count > 2) _lastCatPayload = _args[2];
                _watchdog.Ping();
            }
            else if (tag == "TMS.MAST.STATUS")
            {
                _watchdog.Ping();
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
            else if (string.Compare(cmd, "HANDOVER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _isC2Master = true;
            }
            else if (string.Compare(cmd, "STANDBY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _isC2Master = false;
            }
            else if (string.Compare(cmd, "DISPATCH_MULE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _muleDispatchRequested = true;
            }
            else if (string.Compare(cmd, "DISPATCH_CAT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _catDispatchRequested = true;
            }
            else if (string.Compare(cmd, "DEMAND_SYNC", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_isC2Master) BroadcastBaseDemand();
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
            if (Storage != null && Storage.Contains("MASTER=1"))
                _isC2Master = true;

            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                _demandUraniumKg = _ini.Get("Demand", "UraniumKg").ToInt32(_demandUraniumKg);
                _demandPlatinumKg = _ini.Get("Demand", "PlatinumKg").ToInt32(_demandPlatinumKg);
                _demandGoldKg = _ini.Get("Demand", "GoldKg").ToInt32(_demandGoldKg);
                _demandMagnesiumKg = _ini.Get("Demand", "MagnesiumKg").ToInt32(_demandMagnesiumKg);
                _demandIronTons = _ini.Get("Demand", "IronTons").ToInt32(_demandIronTons);
            }
        }

        void InitHardware()
        {
            // Grid-scoped block query
            _padConnector = GridTerminalSystem.GetBlockWithName("GND PAD Conn") as IMyShipConnector;
            if (_padConnector != null && _padConnector.CubeGrid != Me.CubeGrid) _padConnector = null;

            _catDumpConnector = GridTerminalSystem.GetBlockWithName("GND CAT Dump Conn") as IMyShipConnector;
            if (_catDumpConnector != null && _catDumpConnector.CubeGrid != Me.CubeGrid) _catDumpConnector = null;

            _padSensor = GridTerminalSystem.GetBlockWithName("GND PAD Sensor") as IMySensorBlock;
            if (_padSensor != null && _padSensor.CubeGrid != Me.CubeGrid) _padSensor = null;

            _c2UpsBattery = GridTerminalSystem.GetBlockWithName("GND BAT C2 UPS") as IMyBatteryBlock;
            if (_c2UpsBattery != null && _c2UpsBattery.CubeGrid != Me.CubeGrid) _c2UpsBattery = null;

            _laserRelay = GridTerminalSystem.GetBlockWithName("GND Laser Rig Relay") as IMyLaserAntenna;
            if (_laserRelay != null && _laserRelay.CubeGrid != Me.CubeGrid) _laserRelay = null;

            _fleetAntenna = GridTerminalSystem.GetBlockWithName("GND Antenna Fleet") as IMyRadioAntenna;
            if (_fleetAntenna != null && _fleetAntenna.CubeGrid != Me.CubeGrid) _fleetAntenna = null;

            _substationBatteries.Clear();
            GridTerminalSystem.GetBlocksOfType(_substationBatteries, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("GND BAT"));

            _refineries.Clear();
            GridTerminalSystem.GetBlocksOfType(_refineries, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("GND REF"));

            _assemblers.Clear();
            GridTerminalSystem.GetBlocksOfType(_assemblers, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("GND ASM"));

            _videoWallLcds.Clear();
            GridTerminalSystem.GetBlocksOfType(_videoWallLcds, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("GND LCD Video Wall"));
        }

        void InitNetwork()
        {
            _handoverListener = IGC.RegisterBroadcastListener("TMS.C2.HANDOVER");
            _muleStatusListener = IGC.RegisterBroadcastListener("TMS.MULE.STATUS");
            _catStatusListener = IGC.RegisterBroadcastListener("TMS.CAT.STATUS");
            _mastStatusListener = IGC.RegisterBroadcastListener("TMS.MAST.STATUS");
            _orcaStatusListener = IGC.RegisterBroadcastListener("TMS.ORCA.STATUS");
        }

        void RenderEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("Base.C2 (FleetOps) · ACTIVE\n");
            _sbEcho.Append("Authority: ");
            _sbEcho.Append(_isC2Master ? "REGIONAL C2 ROOT (MASTER)\n" : "STANDBY (MASTODON MKI ROOT)\n");
            _sbEcho.Append("Watchdog: ");
            _sbEcho.Append(_watchdog.IsTimedOut ? "TIMED_OUT\n" : "SYNCED\n");
            _sbEcho.Append("Mule Pad: ");
            _sbEcho.Append(_mulePadStatus);
            _sbEcho.Append(" (Last: ");
            _sbEcho.Append(_lastMulePayload);
            _sbEcho.Append(" t)\nCat Dock: ");
            _sbEcho.Append(_catDockStatus);
            _sbEcho.Append(" (Last: ");
            _sbEcho.Append(_lastCatPayload);
            _sbEcho.Append(" t)\nSubstation: ");
            SbFormat.AppendDouble(_sbEcho, _batteryStoredMWh, 1);
            _sbEcho.Append(" / ");
            SbFormat.AppendDouble(_sbEcho, _batteryMaxMWh, 1);
            _sbEcho.Append(" MWh\nFoundry: ");
            SbFormat.AppendInt(_sbEcho, _activeRefineries);
            _sbEcho.Append(" Refineries · ");
            SbFormat.AppendInt(_sbEcho, _activeAssemblers);
            _sbEcho.Append(" Assemblers Active");
            Echo(_sbEcho.ToString());
        }
    }
}
