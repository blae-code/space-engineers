// Fob.Utility · Space Engineers 1 MDK2 PB Script
// Target Block: [MAS-U] PB Utility
// Grid: Large Grid Utility Carriage (MAS-01-U) · Update: Update10 / Update100
// Subsystem Groups: MAS FOB, MAS REFINERY, MAS ASSEMBLER, MAS RESUPPLY, MAS DPU, MAS MP, MAS DUMP
// IGC Channels: TMS.CONSIST.TRACTION, TMS.CONSIST.SEVER, TMS.CONSIST.SYNC, TMS.RESUPPLY.REQUEST, TMS.RESUPPLY.OFFER, TMS.RESUPPLY.DISPATCH, TMS.RESUPPLY.COMPLETE
// Rule C4: Native blocks provide safety; PB provides decision & FOB logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region SML Quota & Resupply Constants (Minifier-Safe Zero-GC)
        public const int RESUPPLY_NONE = 0;
        public const int RESUPPLY_MOBILITY = 1;
        public const int RESUPPLY_POWER = 2;
        public const int RESUPPLY_MUNITIONS = 3;
        public const int RESUPPLY_HULL = 4;

        static readonly string[] ResupplyKitNames = new string[]
        {
            "NONE",
            "KIT_MOBILITY",
            "KIT_POWER",
            "KIT_MUNITIONS",
            "KIT_HULL_REPAIR"
        };

        public struct SmlItem
        {
            public string Subtype;
            public int TargetQuota;
            public int CurrentStock;
        }

        const int SML_COUNT = 8;
        readonly SmlItem[] _smlQuotas = new SmlItem[SML_COUNT]
        {
            new SmlItem { Subtype = "SteelPlate", TargetQuota = 1500, CurrentStock = 0 },
            new SmlItem { Subtype = "Construction", TargetQuota = 500, CurrentStock = 0 },
            new SmlItem { Subtype = "InteriorPlate", TargetQuota = 400, CurrentStock = 0 },
            new SmlItem { Subtype = "Motor", TargetQuota = 200, CurrentStock = 0 },
            new SmlItem { Subtype = "SmallTube", TargetQuota = 150, CurrentStock = 0 },
            new SmlItem { Subtype = "LargeTube", TargetQuota = 50, CurrentStock = 0 },
            new SmlItem { Subtype = "NATO_25x184mm", TargetQuota = 100, CurrentStock = 0 },
            new SmlItem { Subtype = "NATO_5_56x45mm", TargetQuota = 50, CurrentStock = 0 }
        };
        #endregion

        #region Hardware References
        IMyRemoteControl _remoteFob;
        IMyAssembler _utilityAssembler;
        IMyRefinery _fieldRefinery;
        IMyCargoContainer _holdComponents;
        IMyCargoContainer _holdResupplyExport;
        IMyConveyorSorter _sorterResupplyExport;
        IMyPistonBase _pistonSternDock;
        IMyShipConnector _connSternDump;
        IMyConveyorSorter _sorterSternDump;

        readonly List<IMyMotorSuspension> _dpuWheels = new List<IMyMotorSuspension>(4);
        readonly List<IMyLandingGear> _keelSkids = new List<IMyLandingGear>(2);
        readonly List<IMyShipConnector> _topDeckMulePads = new List<IMyShipConnector>(2);
        #endregion

        #region Internal Telemetry & Buffers
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(512);
        readonly StringBuilder _sbIgc = new StringBuilder(256);
        readonly List<string> _args = new List<string>(8);
        readonly List<MyInventoryItem> _invItemsBuffer = new List<MyInventoryItem>(64);

        int _tickCounter;
        bool _isDecoupled;
        bool _deadmanBrakeEngaged;
        bool _smlAutoProdEnabled = true;
        int _activeResupplyKit = RESUPPLY_NONE;
        bool _resupplyTransferActive;
        double _resupplyTimer = 0.0;
        int _dpuPushDemand = 0;
        int _dpuBrakeDemand = 0;
        float _carriageSpeedKmh;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            Echo("Fob.Utility Initialized.");
        }

        public void Save()
        {
            // Persist decoupled and resupply states
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
                UpdateFobTick();
            }

            if ((updateSource & UpdateType.Update100) != 0)
            {
                UpdateSmlSupervisionTick();
            }

            // Inbound IGC message processing
            ProcessInboundIgcMessages();
        }

        #region FOB Control & Traction Loop (10 Hz)
        void UpdateFobTick()
        {
            _tickCounter++;

            // 1. Measure carriage motion
            if (_remoteFob != null)
            {
                _carriageSpeedKmh = (float)(_remoteFob.GetShipSpeed() * 3.6);
            }

            // 2. Autonomous Detachment & Deadman Breakaway Watchdog (Decision 67)
            if (_isDecoupled)
            {
                // If decoupled and carriage starts rolling uncommanded (> 1.0 km/h)
                if (_carriageSpeedKmh > 1.0f && !_deadmanBrakeEngaged)
                {
                    EngageDeadmanEmergencyBrake();
                }
            }
            else
            {
                // In consist mode: apply DPU synchronized traction/braking to Axles 4 & 5
                ApplyDpuTraction();
            }

            // 3. Outbound Resupply Sortie Timer & Staging
            if (_resupplyTransferActive)
            {
                UpdateResupplySortie();
            }

            // 4. Zero-GC Status Output
            if ((_tickCounter % 10) == 0)
            {
                RenderFobEcho();
            }
        }

        void ApplyDpuTraction()
        {
            float propulsionOverride = (_dpuPushDemand > 0) ? (_dpuPushDemand * 0.01f) : 0.0f;
            float brakeOverride = (_dpuBrakeDemand > 0) ? (_dpuBrakeDemand * 0.01f) : 0.0f;

            for (int i = 0; i < _dpuWheels.Count; i++)
            {
                if (_dpuWheels[i] != null && _dpuWheels[i].IsWorking)
                {
                    _dpuWheels[i].PropulsionOverride = propulsionOverride;
                    _dpuWheels[i].Brake = (brakeOverride > 0.0f);
                }
            }
        }

        void EngageDeadmanEmergencyBrake()
        {
            _deadmanBrakeEngaged = true;

            // Lock keel magnetic skids to ground voxels
            for (int i = 0; i < _keelSkids.Count; i++)
            {
                if (_keelSkids[i] != null)
                {
                    _keelSkids[i].Lock();
                }
            }

            // Lock all wheel brakes
            for (int i = 0; i < _dpuWheels.Count; i++)
            {
                if (_dpuWheels[i] != null)
                {
                    _dpuWheels[i].Brake = true;
                    _dpuWheels[i].PropulsionOverride = 0.0f;
                }
            }

            _sbIgc.Clear();
            _sbIgc.Append("DEADMAN_BREAKAWAY;MAS-01-U;ENGAGED");
            IGC.SendBroadcastMessage("TMS.CONSIST.SEVER", _sbIgc.ToString());
        }

        void UpdateResupplySortie()
        {
            _resupplyTimer += 0.166;

            // Sorter transfer into export hopper (simulated / gated at 3 s transfer time)
            if (_resupplyTimer >= 3.0)
            {
                _resupplyTransferActive = false;
                _resupplyTimer = 0.0;

                // Dispatch top-deck Mule shuttle
                _sbIgc.Clear();
                _sbIgc.Append("DISPATCH;MAS-01;MUL-01;ALL;").Append(ResupplyKitNames[_activeResupplyKit]);
                IGC.SendBroadcastMessage("TMS.RESUPPLY.DISPATCH", _sbIgc.ToString());
            }
        }
        #endregion

        #region SML Quota Supervision & Auto-Production (1 Hz Loop)
        void UpdateSmlSupervisionTick()
        {
            if (!_smlAutoProdEnabled || _holdComponents == null || _utilityAssembler == null) return;

            // 1. Audit current inventory in MAS Hold Components
            IMyInventory inv = _holdComponents.GetInventory();
            _invItemsBuffer.Clear();
            inv.GetItems(_invItemsBuffer);

            // Zero out current stock counters
            for (int i = 0; i < SML_COUNT; i++)
            {
                _smlQuotas[i].CurrentStock = 0;
            }

            for (int j = 0; j < _invItemsBuffer.Count; j++)
            {
                string subtype = _invItemsBuffer[j].Type.SubtypeId;
                int amount = (int)_invItemsBuffer[j].Amount;

                for (int k = 0; k < SML_COUNT; k++)
                {
                    if (_smlQuotas[k].Subtype == subtype)
                    {
                        _smlQuotas[k].CurrentStock += amount;
                        break;
                    }
                }
            }

            // 2. Queue production deltas if stock < 75% of target quota
            for (int i = 0; i < SML_COUNT; i++)
            {
                int threshold = (_smlQuotas[i].TargetQuota * 3) / 4; // 75%
                if (_smlQuotas[i].CurrentStock < threshold)
                {
                    int delta = _smlQuotas[i].TargetQuota - _smlQuotas[i].CurrentStock;
                    QueueAssemblerProduction(_smlQuotas[i].Subtype, delta);
                }
            }
        }

        void QueueAssemblerProduction(string subtype, int amount)
        {
            if (_utilityAssembler == null || !_utilityAssembler.IsWorking) return;

            // Zero-GC production queuing using VRage MyDefinitionId
            MyItemType itemType = MyItemType.MakeComponent(subtype);
            if (subtype.StartsWith("NATO"))
            {
                itemType = MyItemType.MakeAmmo(subtype);
            }

            MyDefinitionId defId;
            if (MyDefinitionId.TryParse("MyObjectBuilder_BlueprintDefinition/" + subtype, out defId))
            {
                _utilityAssembler.AddQueueItem(defId, (MyFixedPoint)amount);
            }
        }
        #endregion

        #region Inbound IGC Protocol Listener
        void ProcessInboundIgcMessages()
        {
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

            if (tag == "TMS.CONSIST.TRACTION")
            {
                // DPU;pushPct;brakePct
                if (_args.Count > 2)
                {
                    int.TryParse(_args[1], out _dpuPushDemand);
                    int.TryParse(_args[2], out _dpuBrakeDemand);
                }
            }
            else if (tag == "TMS.CONSIST.SEVER")
            {
                // SEVER;MAS-01-T;NORMAL;FALSE
                _isDecoupled = true;
                if (_remoteFob != null) _remoteFob.SetAutoPilotEnabled(false);
            }
            else if (tag == "TMS.RESUPPLY.REQUEST")
            {
                // RESUPPLY;target_grid;peer;KIT_NAME;priority
                if (_args.Count > 3)
                {
                    string kitName = _args[3];
                    PrepareResupplyKit(kitName);
                }
            }
        }

        void PrepareResupplyKit(string kitName)
        {
            if (string.Compare(kitName, "KIT_MOBILITY", StringComparison.OrdinalIgnoreCase) == 0)
                _activeResupplyKit = RESUPPLY_MOBILITY;
            else if (string.Compare(kitName, "KIT_POWER", StringComparison.OrdinalIgnoreCase) == 0)
                _activeResupplyKit = RESUPPLY_POWER;
            else if (string.Compare(kitName, "KIT_MUNITIONS", StringComparison.OrdinalIgnoreCase) == 0)
                _activeResupplyKit = RESUPPLY_MUNITIONS;
            else if (string.Compare(kitName, "KIT_HULL_REPAIR", StringComparison.OrdinalIgnoreCase) == 0)
                _activeResupplyKit = RESUPPLY_HULL;
            else
                _activeResupplyKit = RESUPPLY_NONE;

            if (_activeResupplyKit != RESUPPLY_NONE)
            {
                _resupplyTransferActive = true;
                _resupplyTimer = 0.0;
                if (_sorterResupplyExport != null) _sorterResupplyExport.DrainAll = true;

                // Send offer acknowledgement
                _sbIgc.Clear();
                _sbIgc.Append("OFFER;MAS-01-U;MUL-01;").Append(ResupplyKitNames[_activeResupplyKit]);
                IGC.SendBroadcastMessage("TMS.RESUPPLY.OFFER", _sbIgc.ToString());
            }
        }
        #endregion

        #region Command Handling
        void HandleCommand(string argument)
        {
            IgcProtocol.Split(argument, _args);
            if (_args.Count == 0) return;

            string cmd = _args[0];

            if (string.Compare(cmd, "DECOUPLE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _isDecoupled = true;
            }
            else if (string.Compare(cmd, "COUPLE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _isDecoupled = false;
                _deadmanBrakeEngaged = false;
                for (int i = 0; i < _keelSkids.Count; i++) _keelSkids[i].Unlock();
            }
            else if (string.Compare(cmd, "SML", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _smlAutoProdEnabled = !_smlAutoProdEnabled;
            }
            else if (string.Compare(cmd, "RESUPPLY", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                PrepareResupplyKit(_args[1]);
            }
            else if (string.Compare(cmd, "DOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Toggle telescoping stern push-bumper docking tongue
                if (_pistonSternDock != null)
                {
                    _pistonSternDock.Velocity = (_pistonSternDock.Status == PistonStatus.Extended) ? -0.5f : 0.5f;
                }
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
                _smlAutoProdEnabled = _ini.Get("FOB", "SML_AutoProd").ToBoolean(_smlAutoProdEnabled);
            }
        }

        void InitHardware()
        {
            _remoteFob = GetBlockOnGrid<IMyRemoteControl>("MAS-U Remote FOB");
            _utilityAssembler = GetBlockOnGrid<IMyAssembler>("MAS Utility Assembler");
            _fieldRefinery = GetBlockOnGrid<IMyRefinery>("MAS Field Refinery");
            _holdComponents = GetBlockOnGrid<IMyCargoContainer>("MAS Hold Components");
            _holdResupplyExport = GetBlockOnGrid<IMyCargoContainer>("MAS Hold Resupply Export");
            _sorterResupplyExport = GetBlockOnGrid<IMyConveyorSorter>("MAS Sorter Resupply Export");

            _pistonSternDock = GetBlockOnGrid<IMyPistonBase>("MAS Piston Stern Dock");
            _connSternDump = GetBlockOnGrid<IMyShipConnector>("MAS Stern Conn");
            _sorterSternDump = GetBlockOnGrid<IMyConveyorSorter>("MAS Sorter Stern Dump");

            _dpuWheels.Clear();
            GridTerminalSystem.GetBlocksOfType(_dpuWheels, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Axle"));

            _keelSkids.Clear();
            GridTerminalSystem.GetBlocksOfType(_keelSkids, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Keel Skid"));

            // Subscribe to consist & resupply broadcast channels
            IGC.RegisterBroadcastListener("TMS.CONSIST.TRACTION");
            IGC.RegisterBroadcastListener("TMS.CONSIST.SEVER");
            IGC.RegisterBroadcastListener("TMS.RESUPPLY.REQUEST");
        }

        T GetBlockOnGrid<T>(string name) where T : class
        {
            IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName(name);
            if (block == null || block.CubeGrid != Me.CubeGrid) return null;
            return block as T;
        }

        void RenderFobEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("CARRIAGE 2 · MODULAR FOB CORE\n");
            _sbEcho.Append("Status: ").Append(_isDecoupled ? "DETACHED FOB" : "CONSIST TRAILING").Append('\n');
            _sbEcho.Append("Deadman: ").Append(_deadmanBrakeEngaged ? "EMERGENCY TRIPPED" : "ARMED").Append('\n');
            _sbEcho.Append("DPU Push: ");
            SbFormat.AppendInt(_sbEcho, _dpuPushDemand);
            _sbEcho.Append("% · Brake: ");
            SbFormat.AppendInt(_sbEcho, _dpuBrakeDemand);
            _sbEcho.Append("%\nSML Auto-Prod: ").Append(_smlAutoProdEnabled ? "ACTIVE" : "STANDBY").Append('\n');
            _sbEcho.Append("Resupply Sortie: ").Append(ResupplyKitNames[_activeResupplyKit]).Append('\n');
            _sbEcho.Append("Speed: ");
            SbFormat.AppendDouble(_sbEcho, _carriageSpeedKmh, 1);
            _sbEcho.Append(" km/h");
            Echo(_sbEcho.ToString());
        }
        #endregion
    }
}
