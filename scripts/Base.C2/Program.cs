// Base.C2 · Space Engineers 1 MDK2 PB Script
// Target Block: [GND] PB FleetOps
// Grid: Large Grid (24 × 28 B Surface Station) · Update: Update10 + Update100
// Subsystem Groups: GND PAD, GND CAT, GND REF, GND ASM, GND C2, GND Power Batteries
// IGC Channels: TMS.LOGISTICS.JIT (in), TMS.MULE.DISPATCH / TMS.CAT.DISPATCH (out)
// Other: TMS.BASE.DEMAND / RIG.C2.STATUS (out), TMS.C2.HANDOVER / vehicle STATUS (in)
// Rule C4: Native blocks provide safety; PB provides decision logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
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
        readonly StringBuilder _sbGps = new StringBuilder(128);
        readonly StringBuilder _sbRequestId = new StringBuilder(32);
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
        readonly List<IMyTerminalBlock> _demandInventoryBlocks = new List<IMyTerminalBlock>(128);
        readonly List<MyInventoryItem> _demandInventoryItems = new List<MyInventoryItem>(512);
        readonly List<IMyTextPanel> _videoWallLcds = new List<IMyTextPanel>(6);
        readonly MyItemType[] _demandIngotTypes = new MyItemType[]
        {
            MyItemType.Parse("MyObjectBuilder_Ingot/Uranium"),
            MyItemType.Parse("MyObjectBuilder_Ingot/Platinum"),
            MyItemType.Parse("MyObjectBuilder_Ingot/Gold"),
            MyItemType.Parse("MyObjectBuilder_Ingot/Magnesium"),
            MyItemType.Parse("MyObjectBuilder_Ingot/Iron")
        };
        readonly double[] _demandStockKg = new double[5];
        readonly int[] _demandTargetsKg = new int[5];
        readonly int[] _demandDeficitsKg = new int[5];
        readonly int[] _demandPriorities = new int[5];
        IMyLaserAntenna _laserRelay;
        IMyRadioAntenna _fleetAntenna;

        // IGC Listeners
        IMyBroadcastListener _handoverListener;
        IMyBroadcastListener _muleStatusListener;
        IMyBroadcastListener _catStatusListener;
        IMyBroadcastListener _mastStatusListener;
        IMyBroadcastListener _orcaStatusListener;
        IMyBroadcastListener _jitListener;

        // Logistics & Operational Metrics
        string _mulePadStatus = "IDLE";
        string _catDockStatus = "READY";
        string _lastMulePayload = "0.0";
        string _lastCatPayload = "0.0";
        double _batteryStoredMWh;
        double _batteryMaxMWh = 18.0;
        int _activeRefineries;
        int _activeAssemblers;
        int _dispatchSequence;
        string _dispatchStatus = "IDLE";

        // Quotas (kg target reserves)
        int _demandUraniumKg = 500;
        int _demandPlatinumKg = 2000;
        int _demandGoldKg = 3000;
        int _demandMagnesiumKg = 5000;
        int _demandIronTons = 2500;
        bool _demandSettingsValid = true;
        string _demandStatus = "READY";

        static readonly string[] DemandOreTypes = new string[]
        {
            "URANIUM",
            "PLATINUM",
            "GOLD",
            "MAGNESIUM",
            "IRON"
        };
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
            _sbIgc.Clear().Append(_isC2Master ? "MASTER=1" : "MASTER=0").Append(";SEQ=");
            SbFormat.AppendInt(_sbIgc, _dispatchSequence);
            Storage = _sbIgc.ToString();
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

            // Refresh physical dock status
            RefreshDockSensors();
        }

        void UpdateTickSlow()
        {
            _tickSlow++;

            // Assess industrial and substation telemetry
            EvaluateIndustrialGrid();
            ProcessInboundIgc();

            // Broadcast demand and C2 state if master
            if (_isC2Master)
            {
                BroadcastC2Status();
                BroadcastBaseDemand();
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
            if (!_isC2Master) return;
            if (!_demandSettingsValid)
            {
                _demandStatus = "CONFIG INVALID";
                return;
            }
            if (_refineries.Count == 0 || !MeasureDemandStock())
            {
                _demandStatus = "INVENTORY UNAVAILABLE";
                return;
            }

            _demandTargetsKg[0] = _demandUraniumKg;
            _demandTargetsKg[1] = _demandPlatinumKg;
            _demandTargetsKg[2] = _demandGoldKg;
            _demandTargetsKg[3] = _demandMagnesiumKg;
            _demandTargetsKg[4] = _demandIronTons * 1000;

            for (int i = 0; i < DemandOreTypes.Length; i++)
            {
                if (!TmsDemandMath.TryGetPriority(DemandOreTypes[i], out _demandPriorities[i]) ||
                    !TmsDemandMath.TryCalculateDeficitKg(
                        _demandTargetsKg[i], _demandStockKg[i], out _demandDeficitsKg[i]))
                {
                    _demandStatus = "DEMAND DATA INVALID";
                    return;
                }
            }

            for (int i = 0; i < DemandOreTypes.Length; i++)
            {
                if (!TmsProtocol.TryWriteBaseDemand(
                    _sbIgc, DemandOreTypes[i], _demandPriorities[i],
                    _demandDeficitsKg[i], _demandTargetsKg[i]))
                {
                    _demandStatus = "DEMAND FORMAT ERROR";
                    return;
                }

                IGC.SendBroadcastMessage(TmsProtocol.BaseDemandTag, _sbIgc.ToString());
            }
            _demandStatus = "BROADCASTING";
        }

        private bool MeasureDemandStock()
        {
            for (int i = 0; i < _demandStockKg.Length; i++)
                _demandStockKg[i] = 0.0;

            int inventoryCount = 0;
            for (int i = 0; i < _demandInventoryBlocks.Count; i++)
            {
                IMyTerminalBlock block = _demandInventoryBlocks[i];
                if (block == null || block.CubeGrid != Me.CubeGrid) continue;

                for (int inventoryIndex = 0; inventoryIndex < block.InventoryCount; inventoryIndex++)
                {
                    IMyInventory inventory = block.GetInventory(inventoryIndex);
                    if (inventory == null) continue;
                    inventoryCount++;
                    _demandInventoryItems.Clear();
                    inventory.GetItems(_demandInventoryItems);

                    for (int itemIndex = 0; itemIndex < _demandInventoryItems.Count; itemIndex++)
                    {
                        MyInventoryItem item = _demandInventoryItems[itemIndex];
                        for (int oreIndex = 0; oreIndex < _demandIngotTypes.Length; oreIndex++)
                        {
                            if (item.Type.Equals(_demandIngotTypes[oreIndex]))
                            {
                                _demandStockKg[oreIndex] += (double)item.Amount;
                                break;
                            }
                        }
                    }
                }
            }

            return inventoryCount > 0;
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
            ProcessListener(_jitListener);
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

            if (tag == TmsProtocol.HandoverTag)
            {
                string targetNode;
                string timestamp;
                if (TmsProtocol.TryReadHandover(payload, out targetNode, out timestamp))
                {
                    if (targetNode == "GND-01")
                    {
                        _isC2Master = true;
                        _watchdog.Ping();
                    }
                    else if (targetNode == "MAS-01")
                    {
                        _isC2Master = false;
                    }
                }
                return;
            }

            if (tag == TmsProtocol.LogisticsJitTag)
            {
                HandleJitRequest(payload);
                return;
            }

            IgcProtocol.Split(payload, _args);
            if (_args.Count == 0) return;

            if (tag == "TMS.MULE.STATUS")
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

        void HandleJitRequest(string payload)
        {
            if (!_isC2Master) return;

            string requesterId;
            string tier;
            string etaSeconds;
            string oreType;
            string estimatedMassKg;
            string sourceGps;
            if (!TmsProtocol.TryReadJitRequest(payload, _args, out requesterId, out tier,
                out etaSeconds, out oreType, out estimatedMassKg, out sourceGps))
                return;
            int eta;
            int mass;
            if (requesterId != "MAS-01" || !int.TryParse(etaSeconds, out eta) || eta < 0 ||
                !int.TryParse(estimatedMassKg, out mass) || mass < 0 ||
                (tier != "AIR" && tier != "GND"))
                return;

            _dispatchSequence = (_dispatchSequence == int.MaxValue) ? 1 : _dispatchSequence + 1;
            _sbRequestId.Clear().Append("REQ-GND-01-");
            SbFormat.AppendInt(_sbRequestId, _dispatchSequence);
            string requestId = _sbRequestId.ToString();

            if (tier == "AIR")
            {
                if (_padConnector == null)
                {
                    _dispatchStatus = "MULE ROUTE ERROR: GND PAD CONNECTOR MISSING";
                    return;
                }

                Vector3D deliveryPosition = _padConnector.GetPosition();
                BuildGps("GND-01", deliveryPosition, _sbGps);
                if (!TmsProtocol.TryWriteMuleRouteDispatch(_sbIgc, "MUL-01", sourceGps,
                    _sbGps.ToString(), oreType, requestId))
                {
                    _dispatchStatus = "MULE ROUTE ERROR: INVALID REQUEST";
                    return;
                }
                IGC.SendBroadcastMessage(TmsProtocol.MuleDispatchTag, _sbIgc.ToString());
                _dispatchStatus = "MULE ASSIGNED " + requestId;
            }
            else if (tier == "GND")
            {
                if (!TmsProtocol.TryWriteCatDispatch(_sbIgc, "CAT-01", requesterId, sourceGps,
                    oreType, requestId))
                {
                    _dispatchStatus = "CAT ROUTE ERROR: INVALID REQUEST";
                    return;
                }
                IGC.SendBroadcastMessage(TmsProtocol.CatDispatchTag, _sbIgc.ToString());
                _dispatchStatus = "CAT ASSIGNED " + requestId;
            }
        }

        void BuildGps(string name, Vector3D position, StringBuilder buffer)
        {
            buffer.Clear().Append("GPS:").Append(name).Append(':');
            SbFormat.AppendDouble(buffer, position.X, 1).Append(':');
            SbFormat.AppendDouble(buffer, position.Y, 1).Append(':');
            SbFormat.AppendDouble(buffer, position.Z, 1).Append(':');
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
                _dispatchStatus = "MULE DISPATCH REQUIRES A VALID JIT ROUTE";
            }
            else if (string.Compare(cmd, "DISPATCH_CAT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _dispatchStatus = "CAT DISPATCH REQUIRES A VALID JIT ROUTE";
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
            if (!string.IsNullOrEmpty(Storage))
            {
                IgcProtocol.Split(Storage, _args);
                for (int i = 0; i < _args.Count; i++)
                {
                    if (_args[i] == "MASTER=1")
                    {
                        _isC2Master = true;
                    }
                    else if (_args[i] == "MASTER=0")
                    {
                        _isC2Master = false;
                    }
                    else if (_args[i].StartsWith("SEQ=", StringComparison.Ordinal))
                    {
                        int sequence;
                        if (int.TryParse(_args[i].Substring(4), out sequence) && sequence >= 0)
                            _dispatchSequence = sequence;
                        else
                            _dispatchStatus = "STORAGE ERROR: DISPATCH SEQUENCE RESET";
                    }
                }
            }

            _ini.Clear();
            MyIniParseResult result;
            _demandSettingsValid = _ini.TryParse(Me.CustomData, out result);
            if (_demandSettingsValid)
            {
                _demandSettingsValid =
                    TryReadDemandSetting("UraniumKg", _demandUraniumKg, out _demandUraniumKg) &&
                    TryReadDemandSetting("PlatinumKg", _demandPlatinumKg, out _demandPlatinumKg) &&
                    TryReadDemandSetting("GoldKg", _demandGoldKg, out _demandGoldKg) &&
                    TryReadDemandSetting("MagnesiumKg", _demandMagnesiumKg, out _demandMagnesiumKg) &&
                    TryReadDemandSetting("IronTons", _demandIronTons, out _demandIronTons) &&
                    _demandIronTons <= int.MaxValue / 1000;
            }
            _demandStatus = _demandSettingsValid ? "READY" : "CONFIG INVALID";
        }

        bool TryReadDemandSetting(string key, int currentValue, out int value)
        {
            string text = _ini.Get("Demand", key).ToString();
            if (string.IsNullOrEmpty(text))
            {
                value = currentValue;
                return true;
            }

            if (int.TryParse(text, out value) && value >= 0) return true;
            value = currentValue;
            return false;
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

            _demandInventoryBlocks.Clear();
            GridTerminalSystem.GetBlocksOfType(_demandInventoryBlocks, b => b.CubeGrid == Me.CubeGrid);

            _videoWallLcds.Clear();
            GridTerminalSystem.GetBlocksOfType(_videoWallLcds, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("GND LCD Video Wall"));
        }

        void InitNetwork()
        {
            _handoverListener = IGC.RegisterBroadcastListener("TMS.C2.HANDOVER");
            _muleStatusListener = IGC.RegisterBroadcastListener("TMS.MULE.STATUS");
            _catStatusListener = IGC.RegisterBroadcastListener("TMS.CAT.STATUS");
            _mastStatusListener = IGC.RegisterBroadcastListener("TMS.MAS.STATUS");
            _orcaStatusListener = IGC.RegisterBroadcastListener("TMS.ORCA.STATUS");
            _jitListener = IGC.RegisterBroadcastListener(TmsProtocol.LogisticsJitTag);
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
            _sbEcho.Append(" Assemblers Active\nDispatch: ");
            _sbEcho.Append(_dispatchStatus);
            _sbEcho.Append("\nDemand: ");
            _sbEcho.Append(_demandStatus);
            Echo(_sbEcho.ToString());
        }
    }
}
