using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Owns every block reference. Other modules read the cached lists and
        /// call the helpers here instead of touching GridTerminalSystem.
        /// </summary>
        public class GridManager : ISubsystem
        {
            const int RescanInterval = 30;     // Update100 ticks, ~50 s
            const int MaxDockConnectors = 16;

            readonly Program _p;
            readonly Config _cfg;

            // Cached predicates (allocated once). Every query is scoped to our own grid
            // so we never grab blocks from a docked drone or carrier.
            readonly Func<IMyTerminalBlock, bool> _onThisGrid;
            readonly Func<IMyTerminalBlock, bool> _isCargoHolder;
            readonly Func<IMyTerminalBlock, bool> _isStatusPanel;

            public readonly List<IMyGyro> Gyros = new List<IMyGyro>(32);
            public readonly List<IMyThrust> Thrusters = new List<IMyThrust>(64);
            public readonly List<IMyShipDrill> Drills = new List<IMyShipDrill>(32);
            /// <summary>Carrier only: connectors handed out as dock slots, sorted by EntityId.</summary>
            public readonly List<IMyShipConnector> DockConnectors = new List<IMyShipConnector>(MaxDockConnectors);

            readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(16);
            readonly List<IMyGasTank> _tanks = new List<IMyGasTank>(16);
            readonly List<IMyGasTank> _h2Tanks = new List<IMyGasTank>(16);
            readonly List<IMyShipController> _controllers = new List<IMyShipController>(8);
            readonly List<IMyShipConnector> _connectors = new List<IMyShipConnector>(MaxDockConnectors);
            readonly List<IMyTerminalBlock> _cargoBlocks = new List<IMyTerminalBlock>(64);
            readonly List<IMyInventory> _inventories = new List<IMyInventory>(96);
            readonly List<IMyTextPanel> _panels = new List<IMyTextPanel>(8);
            readonly List<IMyTextSurface> _surfaces = new List<IMyTextSurface>(9);

            int _rescanCountdown;

            public IMyShipController Controller { get; private set; }
            public IMyShipConnector Connector { get; private set; }

            /// <summary>Incremented on every rescan so dependants can rebuild derived caches.</summary>
            public int Version { get; private set; }

            public double CargoFill { get; private set; }
            public double BatteryCharge { get; private set; } = 1;
            public double HydrogenFill { get; private set; } = 1;
            public bool HasHydrogen { get { return _h2Tanks.Count > 0; } }

            /// <summary>Null when the grid is flyable, otherwise a static description.</summary>
            public string Problem { get; private set; }

            public bool IsConnected
            {
                get { return Connector != null && Connector.Status == MyShipConnectorStatus.Connected; }
            }

            public bool IsConnectable
            {
                get { return Connector != null && Connector.Status == MyShipConnectorStatus.Connectable; }
            }

            public GridManager(Program p)
            {
                _p = p;
                _cfg = p.Cfg;
                _onThisGrid = b => b.CubeGrid == _p.Me.CubeGrid;
                _isCargoHolder = b => b.CubeGrid == _p.Me.CubeGrid && b.HasInventory
                    && (b is IMyCargoContainer || b is IMyShipDrill || b is IMyShipConnector);
                _isStatusPanel = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(_cfg.LcdTag);
            }

            public void Initialize()
            {
                Rescan();
                RefreshTelemetry();
            }

            public void Update10() { }

            public void Update100()
            {
                if (--_rescanCountdown <= 0)
                    Rescan();
                RefreshTelemetry();
            }

            public void HandleMessage(FleetMessage message) { }

            // ---------------- Block discovery ----------------

            void Rescan()
            {
                _rescanCountdown = RescanInterval;
                var gts = _p.GridTerminalSystem;

                gts.GetBlocksOfType<IMyGyro>(Gyros, _onThisGrid);
                gts.GetBlocksOfType<IMyThrust>(Thrusters, _onThisGrid);
                gts.GetBlocksOfType<IMyShipDrill>(Drills, _onThisGrid);
                gts.GetBlocksOfType<IMyBatteryBlock>(_batteries, _onThisGrid);

                gts.GetBlocksOfType<IMyGasTank>(_tanks, _onThisGrid);
                _h2Tanks.Clear();
                for (int i = 0; i < _tanks.Count; i++)
                    if (_tanks[i].BlockDefinition.SubtypeName.Contains("Hydrogen"))
                        _h2Tanks.Add(_tanks[i]);

                gts.GetBlocksOfType<IMyShipController>(_controllers, _onThisGrid);
                Controller = PickController();

                gts.GetBlocksOfType<IMyShipConnector>(_connectors, _onThisGrid);
                PickConnectors();

                gts.GetBlocksOfType<IMyTerminalBlock>(_cargoBlocks, _isCargoHolder);
                _inventories.Clear();
                for (int i = 0; i < _cargoBlocks.Count; i++)
                {
                    var b = _cargoBlocks[i];
                    for (int j = 0; j < b.InventoryCount; j++)
                        _inventories.Add(b.GetInventory(j));
                }

                gts.GetBlocksOfType<IMyTextPanel>(_panels, _isStatusPanel);
                _surfaces.Clear();
                _surfaces.Add(_p.Me.GetSurface(0));
                for (int i = 0; i < _panels.Count; i++)
                    _surfaces.Add(_panels[i]);
                for (int i = 0; i < _surfaces.Count; i++)
                    _surfaces[i].ContentType = ContentType.TEXT_AND_IMAGE;

                Problem = Diagnose();
                Version++;
            }

            IMyShipController PickController()
            {
                IMyShipController fallback = null;
                for (int i = 0; i < _controllers.Count; i++)
                {
                    var c = _controllers[i];
                    if (c.CustomName.Contains(_cfg.RefTag)) return c;
                    if (fallback == null || (c is IMyRemoteControl && !(fallback is IMyRemoteControl)))
                        fallback = c;
                }
                return fallback;
            }

            void PickConnectors()
            {
                Connector = null;
                DockConnectors.Clear();
                for (int i = 0; i < _connectors.Count; i++)
                {
                    var c = _connectors[i];
                    if (!c.CustomName.Contains(_cfg.DockTag)) continue;
                    if (Connector == null) Connector = c;
                    if (DockConnectors.Count < MaxDockConnectors) DockConnectors.Add(c);
                }

                // Untagged grids: drones use their first connector, carriers offer all of them.
                if (Connector == null && _connectors.Count > 0)
                    Connector = _connectors[0];
                if (DockConnectors.Count == 0)
                    for (int i = 0; i < _connectors.Count && i < MaxDockConnectors; i++)
                        DockConnectors.Add(_connectors[i]);

                // Stable slot numbering across rescans. Insertion sort avoids the
                // comparer allocation of List.Sort(Comparison).
                for (int i = 1; i < DockConnectors.Count; i++)
                {
                    var key = DockConnectors[i];
                    int j = i - 1;
                    while (j >= 0 && DockConnectors[j].EntityId > key.EntityId)
                    {
                        DockConnectors[j + 1] = DockConnectors[j];
                        j--;
                    }
                    DockConnectors[j + 1] = key;
                }
            }

            string Diagnose()
            {
                if (_cfg.Role == FleetRole.Carrier)
                    return DockConnectors.Count == 0 ? "No connectors to offer as docks" : null;
                if (Controller == null) return "No ship controller (add a Remote Control)";
                if (Gyros.Count == 0) return "No gyroscopes";
                if (Thrusters.Count == 0) return "No thrusters";
                if (Connector == null) return "No connector";
                if (_cfg.Role == FleetRole.Miner && Drills.Count == 0) return "No drills";
                return null;
            }

            // ---------------- Telemetry ----------------

            void RefreshTelemetry()
            {
                long cur = 0, max = 0;
                for (int i = 0; i < _inventories.Count; i++)
                {
                    cur += _inventories[i].CurrentVolume.RawValue;
                    max += _inventories[i].MaxVolume.RawValue;
                }
                CargoFill = max > 0 ? (double)cur / max : 0;

                double stored = 0, capacity = 0;
                for (int i = 0; i < _batteries.Count; i++)
                {
                    stored += _batteries[i].CurrentStoredPower;
                    capacity += _batteries[i].MaxStoredPower;
                }
                BatteryCharge = capacity > 0 ? stored / capacity : 1;

                double h2 = 0;
                for (int i = 0; i < _h2Tanks.Count; i++)
                    h2 += _h2Tanks[i].FilledRatio;
                HydrogenFill = _h2Tanks.Count > 0 ? h2 / _h2Tanks.Count : 1;
            }

            // ---------------- Terminal actions ----------------

            public void SetDrills(bool on)
            {
                for (int i = 0; i < Drills.Count; i++)
                    Drills[i].Enabled = on;
            }

            /// <summary>
            /// Docked: batteries recharge, H2 stockpiles, thrusters sleep.
            /// Undocked: everything back to flight mode.
            /// </summary>
            public void SetDockedMode(bool docked)
            {
                var mode = docked ? ChargeMode.Recharge : ChargeMode.Auto;
                for (int i = 0; i < _batteries.Count; i++)
                    _batteries[i].ChargeMode = mode;
                for (int i = 0; i < _h2Tanks.Count; i++)
                    _h2Tanks[i].Stockpile = docked;
                for (int i = 0; i < Thrusters.Count; i++)
                    Thrusters[i].Enabled = !docked;
            }

            public void Connect()
            {
                if (Connector != null) Connector.Connect();
            }

            public void Disconnect()
            {
                if (Connector != null) Connector.Disconnect();
            }

            // ---------------- UI ----------------

            public void AppendStatus(StringBuilder sb)
            {
                if (Problem != null)
                    sb.Append("!! ").Append(Problem).Append('\n');
                if (_cfg.Role == FleetRole.Carrier) return;

                sb.Append("Cargo ");
                Fmt.Pct(sb, CargoFill).Append("  Batt ");
                Fmt.Pct(sb, BatteryCharge);
                if (HasHydrogen)
                {
                    sb.Append("  H2 ");
                    Fmt.Pct(sb, HydrogenFill);
                }
                sb.Append('\n');
            }

            public void WriteDisplays(StringBuilder sb)
            {
                for (int i = 0; i < _surfaces.Count; i++)
                    _surfaces[i].WriteText(sb);
            }
        }
    }
}
