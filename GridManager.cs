using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

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
            const int MaxTransfersPerRun = 24; // bounds the instruction cost of unloading
            const string TimerPrefix = "[FM ";

            readonly Program _p;
            readonly Config _cfg;

            // Cached predicates (allocated once). Every query is scoped to our own grid
            // so we never grab blocks from a docked drone or carrier.
            readonly Func<IMyTerminalBlock, bool> _onThisGrid;
            readonly Func<IMyTerminalBlock, bool> _isCargoHolder;
            readonly Func<IMyTerminalBlock, bool> _isStatusPanel;
            readonly Func<IMyTerminalBlock, bool> _isEjectBlock;
            readonly Func<IMyTerminalBlock, bool> _isHookTimer;
            // The one deliberate cross-grid query: the carrier we are docked to, reached
            // through our own connector, never a grid that merely touches us.
            readonly Func<IMyTerminalBlock, bool> _isUnloadTarget;

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
            readonly List<IMyTerminalBlock> _panels = new List<IMyTerminalBlock>(8);
            readonly List<IMyTextSurface> _surfaces = new List<IMyTextSurface>(9);
            readonly List<IMyCameraBlock> _cameras = new List<IMyCameraBlock>(8);
            readonly List<IMyShipConnector> _ejectors = new List<IMyShipConnector>(8);
            readonly List<IMyConveyorSorter> _sorters = new List<IMyConveyorSorter>(8);
            readonly List<IMyTimerBlock> _timers = new List<IMyTimerBlock>(16);
            readonly List<IMyRadioAntenna> _antennas = new List<IMyRadioAntenna>(2);
            readonly List<IMyLaserAntenna> _lasers = new List<IMyLaserAntenna>(2);

            // Stone dumping: sorters whitelist stone and drain it into the ejectors.
            readonly List<MyInventoryItemFilter> _stoneFilter = new List<MyInventoryItemFilter>(1);
            bool _ejecting;

            // Unloading into the carrier (rebuilt whenever we dock to a different grid).
            IMyCubeGrid _unloadGrid;
            bool _unloadTagged;
            readonly List<IMyTerminalBlock> _unloadBlocks = new List<IMyTerminalBlock>(64);
            readonly List<IMyInventory> _unloadInventories = new List<IMyInventory>(64);
            readonly List<MyInventoryItem> _items = new List<MyInventoryItem>(32);

            int _rescanCountdown;

            public IMyShipController Controller { get; private set; }
            public IMyShipConnector Connector { get; private set; }
            /// <summary>Forward-facing camera used for rock-face raycasts (may be null).</summary>
            public IMyCameraBlock Camera { get; private set; }

            /// <summary>Incremented on every rescan so dependants can rebuild derived caches.</summary>
            public int Version { get; private set; }

            public double CargoFill { get; private set; }
            /// <summary>Raw cargo volume; a rise while drilling means the drills are cutting rock.</summary>
            public long CargoVolume { get; private set; }
            public double BatteryCharge { get; private set; } = 1;
            public double HydrogenFill { get; private set; } = 1;
            public bool HasHydrogen { get { return _h2Tanks.Count > 0; } }
            public bool CanEject { get { return _ejectors.Count > 0 && _sorters.Count > 0; } }
            public bool DrillsDamaged { get; private set; }

            /// <summary>
            /// Drill bank geometry in the reference block's frame, metres: how far the
            /// cutting face sits ahead of the reference, and the width/height it cuts.
            /// </summary>
            public double DrillReach { get; private set; }
            public double DrillWidth { get; private set; }
            public double DrillHeight { get; private set; }

            /// <summary>Null when the grid is flyable, otherwise a static description.</summary>
            public string Problem { get; private set; }
            /// <summary>Non-fatal setup issue, shown on the status display.</summary>
            public string Warning { get; private set; }

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
                _isStatusPanel = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(_cfg.LcdTag)
                    && (b is IMyTextSurface || b is IMyTextSurfaceProvider);
                _isEjectBlock = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(_cfg.EjectTag);
                _isHookTimer = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(TimerPrefix);
                _isUnloadTarget = b => b.CubeGrid == _unloadGrid && b.HasInventory && b.IsFunctional;
                _stoneFilter.Add(new MyInventoryItemFilter("MyObjectBuilder_Ore/Stone"));
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

                gts.GetBlocksOfType<IMyCameraBlock>(_cameras, _onThisGrid);
                Camera = PickCamera();
                if (Camera != null) Camera.EnableRaycast = true;

                gts.GetBlocksOfType<IMyShipConnector>(_ejectors, _isEjectBlock);
                gts.GetBlocksOfType<IMyConveyorSorter>(_sorters, _isEjectBlock);
                for (int i = 0; i < _sorters.Count; i++)
                {
                    var s = _sorters[i];
                    if (s.DrainAll && s.Mode == MyConveyorSorterMode.Whitelist) continue; // already ours
                    s.SetFilter(MyConveyorSorterMode.Whitelist, _stoneFilter);
                    s.DrainAll = true;
                }
                SetEjecting(_ejecting);

                gts.GetBlocksOfType<IMyTimerBlock>(_timers, _isHookTimer);
                gts.GetBlocksOfType<IMyRadioAntenna>(_antennas, _onThisGrid);
                gts.GetBlocksOfType<IMyLaserAntenna>(_lasers, _onThisGrid);

                gts.GetBlocksOfType<IMyTerminalBlock>(_panels, _isStatusPanel);
                _surfaces.Clear();
                _surfaces.Add(_p.Me.GetSurface(0));
                for (int i = 0; i < _panels.Count; i++)
                {
                    var b = _panels[i];
                    if (b == _p.Me) continue;
                    var surface = b as IMyTextSurface;
                    if (surface == null)
                    {
                        var provider = (IMyTextSurfaceProvider)b; // cockpits, consoles, button panels
                        if (provider.SurfaceCount > 0) surface = provider.GetSurface(0);
                    }
                    if (surface != null) _surfaces.Add(surface);
                }
                for (int i = 0; i < _surfaces.Count; i++)
                    _surfaces[i].ContentType = ContentType.TEXT_AND_IMAGE;

                MeasureDrills();
                Problem = Diagnose();
                Warning = Advise();
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

                // Untagged grids: drones use their first non-ejector connector, carriers offer all of them.
                for (int i = 0; i < _connectors.Count && Connector == null; i++)
                    if (!_connectors[i].CustomName.Contains(_cfg.EjectTag))
                        Connector = _connectors[i];
                if (DockConnectors.Count == 0)
                    for (int i = 0; i < _connectors.Count && DockConnectors.Count < MaxDockConnectors; i++)
                        if (!_connectors[i].CustomName.Contains(_cfg.EjectTag))
                            DockConnectors.Add(_connectors[i]);

                // A drone's dock connector must never spit cargo at the carrier.
                if (Connector != null && _cfg.Role != FleetRole.Carrier)
                {
                    Connector.ThrowOut = false;
                    Connector.CollectAll = false;
                }

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

            /// <summary>Tagged camera, else any camera looking along the reference forward.</summary>
            IMyCameraBlock PickCamera()
            {
                IMyCameraBlock fallback = null;
                for (int i = 0; i < _cameras.Count; i++)
                {
                    var c = _cameras[i];
                    if (c.CustomName.Contains(_cfg.CamTag)) return c;
                    if (fallback == null && Controller != null
                        && Vector3D.Dot(c.WorldMatrix.Forward, Controller.WorldMatrix.Forward) > 0.99)
                        fallback = c;
                }
                return fallback;
            }

            /// <summary>
            /// Projects every drill's bounding box onto the reference axes. Block size comes
            /// from Min/Max, so any drill variant (small/large grid, DLC skins) measures correctly.
            /// </summary>
            void MeasureDrills()
            {
                double size = _p.Me.CubeGrid.GridSize;
                DrillReach = 0;
                DrillWidth = size;
                DrillHeight = size;
                if (Controller == null || Drills.Count == 0) return;

                MatrixD refM = Controller.WorldMatrix;
                MatrixD gridT = MatrixD.Transpose(_p.Me.CubeGrid.WorldMatrix);
                Vector3D r = Vector3D.TransformNormal(refM.Right, gridT);
                Vector3D u = Vector3D.TransformNormal(refM.Up, gridT);
                Vector3D f = Vector3D.TransformNormal(refM.Forward, gridT);

                double minX = double.MaxValue, maxX = double.MinValue;
                double minY = double.MaxValue, maxY = double.MinValue;
                double maxZ = double.MinValue;
                for (int i = 0; i < Drills.Count; i++)
                {
                    var d = Drills[i];
                    Vector3I cells = d.Max - d.Min + Vector3I.One;
                    Vector3D half = new Vector3D(cells.X, cells.Y, cells.Z) * (size * 0.5);
                    double hx = Math.Abs(half.X * r.X) + Math.Abs(half.Y * r.Y) + Math.Abs(half.Z * r.Z);
                    double hy = Math.Abs(half.X * u.X) + Math.Abs(half.Y * u.Y) + Math.Abs(half.Z * u.Z);
                    double hz = Math.Abs(half.X * f.X) + Math.Abs(half.Y * f.Y) + Math.Abs(half.Z * f.Z);

                    Vector3D rel = d.GetPosition() - refM.Translation;
                    double x = Vector3D.Dot(rel, refM.Right);
                    double y = Vector3D.Dot(rel, refM.Up);
                    double z = Vector3D.Dot(rel, refM.Forward);
                    minX = Math.Min(minX, x - hx);
                    maxX = Math.Max(maxX, x + hx);
                    minY = Math.Min(minY, y - hy);
                    maxY = Math.Max(maxY, y + hy);
                    maxZ = Math.Max(maxZ, z + hz);
                }
                DrillReach = maxZ;
                DrillWidth = maxX - minX;
                DrillHeight = maxY - minY;
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

            string Advise()
            {
                if (_cfg.Role == FleetRole.Carrier) return null;
                if (_antennas.Count == 0 && _lasers.Count == 0) return "No antenna: carrier unreachable when undocked";
                if (_cfg.Role != FleetRole.Miner) return null;
                if (Camera == null) return "No forward camera: rock face found by touch";
                if ((_ejectors.Count > 0) != (_sorters.Count > 0)) return "Stone dump needs a sorter AND an ejector";
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
                CargoVolume = cur;
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

                bool damaged = false;
                for (int i = 0; i < Drills.Count && !damaged; i++)
                    damaged = Drills[i].Closed || !Drills[i].IsFunctional;
                DrillsDamaged = damaged;
            }

            // ---------------- Sensing ----------------

            /// <summary>
            /// Raycasts straight ahead for rock (asteroid or planet voxels only).
            /// Returns false while the camera is still charging; check <paramref name="hit"/> otherwise.
            /// </summary>
            public bool TryScanRock(double range, out bool hit, out Vector3D point)
            {
                hit = false;
                point = Vector3D.Zero;
                var cam = Camera;
                if (cam == null || !cam.IsWorking) return true; // nothing to wait for
                if (!cam.EnableRaycast) cam.EnableRaycast = true;
                if (!cam.CanScan(range)) return false;

                MyDetectedEntityInfo info = cam.Raycast(range, 0, 0);
                if (info.IsEmpty() || !info.HitPosition.HasValue) return true;
                if (info.Type != MyDetectedEntityType.Asteroid && info.Type != MyDetectedEntityType.Planet) return true;
                hit = true;
                point = info.HitPosition.Value;
                return true;
            }

            // ---------------- Terminal actions ----------------

            public void SetDrills(bool on)
            {
                for (int i = 0; i < Drills.Count; i++)
                    Drills[i].Enabled = on;
            }

            /// <summary>Stone dump on/off: sorters pull stone off the conveyors, ejectors throw it out.</summary>
            public void SetEjecting(bool on)
            {
                _ejecting = on;
                for (int i = 0; i < _sorters.Count; i++)
                    _sorters[i].Enabled = on;
                for (int i = 0; i < _ejectors.Count; i++)
                {
                    _ejectors[i].ThrowOut = on;
                    _ejectors[i].Enabled = on;
                }
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
                _unloadGrid = null; // next dock may be a different carrier
            }

            /// <summary>Triggers every timer whose name contains <paramref name="tag"/>, e.g. "[FM Docked]".</summary>
            public void FireTimers(string tag)
            {
                for (int i = 0; i < _timers.Count; i++)
                {
                    var t = _timers[i];
                    if (!t.Closed && t.IsWorking && t.CustomName.Contains(tag)) t.Trigger();
                }
            }

            /// <summary>
            /// Pushes ore out of our drills and cargo into the docked carrier's containers.
            /// Containers tagged with UnloadTag are preferred; otherwise any container on the
            /// carrier grid will do. The work per call is bounded; call it every Update100.
            /// </summary>
            public void Unload()
            {
                if (!_cfg.Unload || !IsConnected) return;
                var other = Connector.OtherConnector;
                if (other == null) return;
                if (other.CubeGrid != _unloadGrid) FindUnloadTargets(other.CubeGrid);
                if (_unloadInventories.Count == 0) return;

                int budget = MaxTransfersPerRun;
                for (int s = 0; s < _inventories.Count && budget > 0; s++)
                {
                    var src = _inventories[s];
                    if (src.ItemCount == 0) continue;
                    _items.Clear();
                    src.GetItems(_items);
                    for (int k = _items.Count - 1; k >= 0 && budget > 0; k--)
                    {
                        var item = _items[k];
                        for (int t = 0; t < _unloadInventories.Count; t++)
                        {
                            var dst = _unloadInventories[t];
                            if (dst.IsFull || !src.CanTransferItemTo(dst, item.Type)) continue;
                            budget--;
                            if (src.TransferItemTo(dst, item)) break;
                        }
                    }
                }
            }

            void FindUnloadTargets(IMyCubeGrid grid)
            {
                _unloadGrid = grid;
                _p.GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_unloadBlocks, _isUnloadTarget);

                _unloadTagged = false;
                for (int i = 0; i < _unloadBlocks.Count && !_unloadTagged; i++)
                    _unloadTagged = _unloadBlocks[i].CustomName.Contains(_cfg.UnloadTag);

                _unloadInventories.Clear();
                for (int i = 0; i < _unloadBlocks.Count; i++)
                {
                    var b = _unloadBlocks[i];
                    if (_unloadTagged && !b.CustomName.Contains(_cfg.UnloadTag)) continue;
                    _unloadInventories.Add(b.GetInventory(0));
                }
            }

            // ---------------- UI ----------------

            public void AppendStatus(StringBuilder sb)
            {
                if (Problem != null)
                    sb.Append("!! ").Append(Problem).Append('\n');
                if (Warning != null)
                    sb.Append("! ").Append(Warning).Append('\n');
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
                if (DrillsDamaged) sb.Append("!! Drill damaged\n");
            }

            public void WriteDisplays(StringBuilder sb)
            {
                for (int i = 0; i < _surfaces.Count; i++)
                    _surfaces[i].WriteText(sb);
            }
        }
    }
}
