using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
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
            const string HookPrefix = "[FM ";
            const string LightTag = "[FM Light]";
            const string PadTag = "[FM Pad ";
            const string BayTag = "[FM Bay]", FleeTag = "[FM Flee]", SearchlightTag = "[FM Searchlight]";

            readonly Program _p;
            readonly Config _cfg;

            // Cached predicates (allocated once). Every query is scoped to our own grid
            // so we never grab blocks from a docked drone or carrier.
            readonly Func<IMyTerminalBlock, bool> _onThisGrid;
            readonly Func<IMyTerminalBlock, bool> _isCargoHolder;
            readonly Func<IMyTerminalBlock, bool> _isScreen;
            readonly Func<IMyTerminalBlock, bool> _isEjectBlock;
            readonly Func<IMyTerminalBlock, bool> _isHook;
            readonly Func<IMyTerminalBlock, bool> _isSensor;
            // The one deliberate cross-grid query: the carrier we are docked to, reached
            // through our own connector, never a grid that merely touches us.
            readonly Func<IMyTerminalBlock, bool> _isUnloadTarget;

            public readonly List<IMyGyro> Gyros = new List<IMyGyro>(32);
            public readonly List<IMyThrust> Thrusters = new List<IMyThrust>(64);
            public readonly List<IMyShipDrill> Drills = new List<IMyShipDrill>(32);
            /// <summary>Carrier only: connectors handed out as dock slots, sorted by EntityId.</summary>
            public readonly List<IMyShipConnector> DockConnectors = new List<IMyShipConnector>(MaxDockConnectors);

            /// <summary>Screens and what each shows (Display.Mode*), plus an argument (map site).</summary>
            public readonly List<IMyTextSurface> Screens = new List<IMyTextSurface>(16);
            public readonly List<int> ScreenMode = new List<int>(16);
            public readonly List<int> ScreenArg = new List<int>(16);
            readonly StringBuilder _probe = new StringBuilder().Append('0', BrainFSM.MenuWidth); // menu font sizing

            readonly List<IMyTerminalBlock> _all = new List<IMyTerminalBlock>(256);
            readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>(16);
            readonly List<IMyGasTank> _tanks = new List<IMyGasTank>(16);
            readonly List<IMyGasTank> _h2Tanks = new List<IMyGasTank>(16);
            readonly List<IMyShipController> _controllers = new List<IMyShipController>(8);
            readonly List<IMyShipConnector> _connectors = new List<IMyShipConnector>(MaxDockConnectors);
            readonly List<IMyTerminalBlock> _cargoBlocks = new List<IMyTerminalBlock>(64);
            readonly List<IMyInventory> _inventories = new List<IMyInventory>(96);
            readonly List<IMyTerminalBlock> _screenBlocks = new List<IMyTerminalBlock>(16);
            readonly List<IMyCameraBlock> _cameras = new List<IMyCameraBlock>(8);
            readonly List<IMyShipConnector> _ejectors = new List<IMyShipConnector>(8);
            readonly List<IMyConveyorSorter> _sorters = new List<IMyConveyorSorter>(8);
            readonly List<IMyTimerBlock> _timers = new List<IMyTimerBlock>(16);
            readonly List<IMySoundBlock> _sounds = new List<IMySoundBlock>(8);
            readonly List<IMyLightingBlock> _lightBlocks = new List<IMyLightingBlock>(32);
            readonly List<IMyLightingBlock> _lights = new List<IMyLightingBlock>(16);
            readonly List<IMyLightingBlock> _padLights = new List<IMyLightingBlock>(16);
            readonly List<int> _padSlot = new List<int>(16);
            readonly List<IMyRadioAntenna> _antennas = new List<IMyRadioAntenna>(2);
            readonly List<IMyLaserAntenna> _lasers = new List<IMyLaserAntenna>(2);
            readonly List<IMyBeacon> _beacons = new List<IMyBeacon>(2);
            readonly List<IMyLargeTurretBase> _turrets = new List<IMyLargeTurretBase>(8);
            readonly List<IMyTurretControlBlock> _turretControllers = new List<IMyTurretControlBlock>(4);
            readonly List<IMySensorBlock> _sensors = new List<IMySensorBlock>(4);
            readonly List<MyDetectedEntityInfo> _detected = new List<MyDetectedEntityInfo>(16);
            readonly List<IMyDecoy> _decoys = new List<IMyDecoy>(4);
            readonly List<IMyDefensiveCombatBlock> _fleeBlocks = new List<IMyDefensiveCombatBlock>(2);
            readonly List<IMySearchlight> _searchlights = new List<IMySearchlight>(4);
            readonly List<IMyInventory> _holds = new List<IMyInventory>(32);
            readonly List<IMyLandingGear> _gears = new List<IMyLandingGear>(8);

            // ---- Mod integrations: found at run time by terminal property name, all optional ----
            // Adjustable Mining Laser (welder-based) and ToolCore tools such as Simple Laser
            // Multitools (sorter-based). Only lasers facing the reference forward are used.
            const string AldFire = "AdjustableLaserDrill.ContinuousFire", AldStone = "AdjustableLaserDrill.IgnoreStone";
            const string AldContact = "AdjustableLaserDrill.LastContactDistance", AldBlocked = "AdjustableLaserDrill.InventoryBlocked";
            const string AldNoPower = "AdjustableLaserDrill.InsufficientPower";
            const string TcShoot = "ToolCore_Shoot", TcMode = "ToolCore_Mode";
            const long TcDrill = 4; // ToolComp.ToolMode.Drill
            readonly List<IMyTerminalBlock> _toolBlocks = new List<IMyTerminalBlock>(16);
            public readonly List<IMyTerminalBlock> Lasers = new List<IMyTerminalBlock>(8);
            ITerminalProperty<bool> _fireProp, _blockedProp, _noPowerProp;
            ITerminalProperty<float> _rangeProp, _radiusProp, _contactProp;
            ITerminalProperty<long> _modeProp;
            // WeaponCore threat list and Defense Shields status (programmable block APIs).
            Action<IMyTerminalBlock, IDictionary<MyDetectedEntityInfo, float>> _wcThreats;
            readonly Dictionary<MyDetectedEntityInfo, float> _threats = new Dictionary<MyDetectedEntityInfo, float>(16);
            Func<IMyEntity, IMyTerminalBlock> _dsShieldBlock;
            Func<IMyTerminalBlock, float> _dsPercent;
            IMyTerminalBlock _shield;

            // Stone dumping: sorters whitelist stone and drain it into the ejectors.
            readonly List<MyInventoryItemFilter> _stoneFilter = new List<MyInventoryItemFilter>(1);
            bool _ejecting;

            // Unloading into the carrier (rebuilt whenever we dock to a different grid).
            IMyCubeGrid _unloadGrid;
            readonly List<IMyTerminalBlock> _unloadBlocks = new List<IMyTerminalBlock>(64);
            readonly List<IMyInventory> _unloadInventories = new List<IMyInventory>(64);
            readonly List<MyInventoryItem> _items = new List<MyInventoryItem>(32);

            int _rescanCountdown;
            string _hudText;
            Color _lightColor;
            float _lightBlink;

            public IMyShipController Controller { get; private set; }
            public IMyShipConnector Connector { get; private set; }
            /// <summary>Forward-facing camera used for rock-face raycasts (may be null).</summary>
            public IMyCameraBlock Camera { get; private set; }

            /// <summary>Incremented on every rescan so dependants can rebuild derived caches.</summary>
            public int Version { get; private set; }

            public double CargoFill { get; private set; }
            /// <summary>Raw cargo volume; a rise while drilling means the drills are cutting rock.</summary>
            public long CargoVolume { get; private set; }
            /// <summary>Kilograms of each ore on board (Ore.Names order).</summary>
            public readonly double[] OreKg = new double[Ore.Count];
            public double BatteryCharge { get; private set; } = 1;
            public double HydrogenFill { get; private set; } = 1;
            public bool HasHydrogen { get { return _h2Tanks.Count > 0; } }
            public bool HasPowerStore { get { return _batteries.Count > 0 || _h2Tanks.Count > 0; } }
            public bool CanEject { get { return _ejectors.Count > 0 && _sorters.Count > 0; } }
            public bool DrillsDamaged { get; private set; }
            /// <summary>Fraction of our terminal blocks still functional.</summary>
            public double Integrity { get; private set; } = 1;
            /// <summary>A turret, turret controller or tagged sensor sees something.</summary>
            public bool ThreatDetected { get; private set; }
            /// <summary>Where the threat is, when a sensor or turret reported it (else Zero).</summary>
            public Vector3D ThreatPos { get; private set; }
            public Vector3D ThreatVel { get; private set; }
            public bool ThreatLocated { get; private set; }
            /// <summary>Battery net drain in MW (negative while charging).</summary>
            public double BatteryDrainMW { get; private set; }
            double _batteryStoredMWh;
            /// <summary>Minutes until the batteries are flat at the current drain (infinity if not draining).</summary>
            public double BatteryMinutes
            {
                get { return BatteryDrainMW > 0.001 ? _batteryStoredMWh / BatteryDrainMW * 60 : double.PositiveInfinity; }
            }

            /// <summary>
            /// Drill bank geometry in the reference block's frame, metres: how far the
            /// cutting face sits ahead of the reference, and the width/height it cuts.
            /// </summary>
            public double DrillReach { get; private set; }
            /// <summary>Half our dock connector's length along its facing: two mated connectors' centres sit 2x this apart.</summary>
            public double ConnectorHalf { get; private set; }

            public bool HasLasers { get { return Lasers.Count > 0; } }
            /// <summary>ToolCore lasers: no contact readout, range/radius come from config.</summary>
            public bool LasersToolCore { get; private set; }
            /// <summary>How far the laser emitters sit ahead of the reference block, along its forward.</summary>
            public double LaserForward { get; private set; }
            public double LaserRadius { get; private set; }
            public double LaserRange { get; private set; }
            /// <summary>False when a ToolCore tool could not be confirmed in Drill mode (never fired then).</summary>
            public bool LaserModeOk { get; private set; } = true;
            public bool WeaponCore { get { return _wcThreats != null; } }
            /// <summary>Our Defense Shields charge, 0-100, or -1 without a shield.</summary>
            public double ShieldPercent { get; private set; } = -1;
            public bool GearLocked { get; private set; }
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
                _isScreen = b => b.CubeGrid == _p.Me.CubeGrid && b != _p.Me
                    && (b is IMyTextSurface || b is IMyTextSurfaceProvider)
                    && (b.CustomName.Contains(_cfg.LcdTag) || b.CustomName.Contains(HookPrefix));
                _isEjectBlock = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(_cfg.EjectTag);
                _isHook = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(HookPrefix);
                _isSensor = b => b.CubeGrid == _p.Me.CubeGrid && b.CustomName.Contains(_cfg.SensorTag);
                _isUnloadTarget = b => b.CubeGrid == _unloadGrid && b.HasInventory && b.IsFunctional;
                _stoneFilter.Add(new MyInventoryItemFilter(Ore.TypeId + "/Stone"));
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

                gts.GetBlocksOfType<IMyTerminalBlock>(_all, _onThisGrid);
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
                _holds.Clear();
                for (int i = 0; i < _cargoBlocks.Count; i++)
                {
                    var b = _cargoBlocks[i];
                    for (int j = 0; j < b.InventoryCount; j++)
                        _inventories.Add(b.GetInventory(j));
                    if (b is IMyCargoContainer) _holds.Add(b.GetInventory(0));
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

                gts.GetBlocksOfType<IMyTimerBlock>(_timers, _isHook);
                gts.GetBlocksOfType<IMySoundBlock>(_sounds, _isHook);
                gts.GetBlocksOfType<IMyLightingBlock>(_lightBlocks, _isHook);
                SortLights();

                gts.GetBlocksOfType<IMyRadioAntenna>(_antennas, _onThisGrid);
                gts.GetBlocksOfType<IMyLaserAntenna>(_lasers, _onThisGrid);
                gts.GetBlocksOfType<IMyBeacon>(_beacons, _onThisGrid);
                gts.GetBlocksOfType<IMyLargeTurretBase>(_turrets, _onThisGrid);
                gts.GetBlocksOfType<IMyTurretControlBlock>(_turretControllers, _onThisGrid);
                gts.GetBlocksOfType<IMySensorBlock>(_sensors, _isSensor);
                if (_cfg.ConfigureSensors)
                    for (int i = 0; i < _sensors.Count; i++)
                    {
                        // Enemies only: friends, asteroids and loose ore are not threats.
                        var s = _sensors[i];
                        s.DetectEnemy = true;
                        s.DetectNeutral = false;
                        s.DetectFriendly = false;
                        s.DetectOwner = false;
                        s.DetectAsteroids = false;
                        s.DetectFloatingObjects = false;
                        s.DetectPlayers = true;
                        s.DetectSmallShips = true;
                        s.DetectLargeShips = true;
                        s.DetectStations = true;
                    }
                gts.GetBlocksOfType<IMyDecoy>(_decoys, _onThisGrid);
                gts.GetBlocksOfType<IMyDefensiveCombatBlock>(_fleeBlocks, _isHook);
                for (int i = _fleeBlocks.Count - 1; i >= 0; i--)
                    if (!_fleeBlocks[i].CustomName.Contains(FleeTag)) _fleeBlocks.RemoveAt(i);
                gts.GetBlocksOfType<IMySearchlight>(_searchlights, _isHook);
                for (int i = _searchlights.Count - 1; i >= 0; i--)
                {
                    var l = _searchlights[i];
                    if (!l.CustomName.Contains(SearchlightTag)) { _searchlights.RemoveAt(i); continue; }
                    // Track friendly small ships: our drones on final approach.
                    l.TargetFriends = true;
                    l.TargetSmallShips = true;
                    l.TargetEnemy = false;
                    l.TargetNeutrals = false;
                    l.TargetLargeShips = false;
                    l.TargetStations = false;
                    l.TargetCharacters = false;
                }

                FindScreens();
                FindLasers();
                LinkModApis();
                gts.GetBlocksOfType<IMyLandingGear>(_gears, _onThisGrid);
                if (!_cfg.IsBase)
                    for (int i = 0; i < _gears.Count; i++)
                        _gears[i].AutoLock = false; // a drone must never grab the rock it is cutting
                MeasureDrills();
                ConnectorHalf = 0;
                if (Connector != null)
                {
                    Vector3I cells = Connector.Max - Connector.Min + Vector3I.One;
                    Vector3D half = new Vector3D(cells.X, cells.Y, cells.Z) * (_p.Me.CubeGrid.GridSize * 0.5);
                    Vector3D f = Vector3D.TransformNormal(Connector.WorldMatrix.Forward, MatrixD.Transpose(_p.Me.CubeGrid.WorldMatrix));
                    ConnectorHalf = Math.Abs(half.X * f.X) + Math.Abs(half.Y * f.Y) + Math.Abs(half.Z * f.Z);
                }
                Problem = Diagnose();
                Warning = Advise();
                if (_hudText != null) ApplyHud();
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
                bool hauler = _cfg.Role == FleetRole.Hauler;
                for (int i = 0; i < _connectors.Count; i++)
                {
                    var c = _connectors[i];
                    // Haulers offer [FM Bay] connectors to miners; bases offer [FM Dock] ones.
                    if (hauler && c.CustomName.Contains(BayTag))
                    {
                        if (DockConnectors.Count < MaxDockConnectors) DockConnectors.Add(c);
                        continue;
                    }
                    if (!c.CustomName.Contains(_cfg.DockTag)) continue;
                    if (Connector == null) Connector = c;
                    if (!hauler && DockConnectors.Count < MaxDockConnectors) DockConnectors.Add(c);
                }

                // Untagged grids: drones use their first plain connector, bases offer all of them.
                for (int i = 0; i < _connectors.Count && Connector == null; i++)
                    if (Plain(_connectors[i]))
                        Connector = _connectors[i];
                if (DockConnectors.Count == 0 && _cfg.IsBase)
                    for (int i = 0; i < _connectors.Count && DockConnectors.Count < MaxDockConnectors; i++)
                        if (Plain(_connectors[i]))
                            DockConnectors.Add(_connectors[i]);

                // A drone's dock connector must never spit cargo at the carrier.
                if (Connector != null && !_cfg.IsBase)
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

            bool Plain(IMyShipConnector c)
            {
                return !c.CustomName.Contains(_cfg.EjectTag) && !c.CustomName.Contains(BayTag);
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

            /// <summary>Status lights ([FM Light]) and carrier dock-pad lights ([FM Pad 0], [FM Pad 1], ...).</summary>
            void SortLights()
            {
                _lights.Clear();
                _padLights.Clear();
                _padSlot.Clear();
                for (int i = 0; i < _lightBlocks.Count; i++)
                {
                    var l = _lightBlocks[i];
                    string name = l.CustomName;
                    if (name.Contains(LightTag)) _lights.Add(l);
                    int pad = name.IndexOf(PadTag, StringComparison.Ordinal);
                    if (pad < 0) continue;
                    int slot = ParseInt(name, pad + PadTag.Length);
                    if (slot < 0) continue;
                    _padLights.Add(l);
                    _padSlot.Add(slot);
                }
            }

            /// <summary>Every tagged screen and its mode. The PB's own screen always shows the status text.</summary>
            void FindScreens()
            {
                Screens.Clear();
                ScreenMode.Clear();
                ScreenArg.Clear();
                AddScreen(_p.Me.GetSurface(0), Display.ModeText, -1);

                _p.GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(_screenBlocks, _isScreen);
                for (int i = 0; i < _screenBlocks.Count; i++)
                {
                    var b = _screenBlocks[i];
                    string name = b.CustomName;
                    int mode = -1, arg = -1;
                    for (int m = 1; m < Display.Tags.Length && mode < 0; m++)
                    {
                        int at = name.IndexOf(Display.Tags[m], StringComparison.Ordinal);
                        if (at < 0) continue;
                        mode = m;
                        arg = ParseInt(name, at + Display.Tags[m].Length);
                    }
                    if (mode < 0 && name.Contains(_cfg.LcdTag)) mode = Display.ModeText;
                    if (mode < 0) continue;

                    var surface = b as IMyTextSurface;
                    if (surface == null)
                    {
                        var provider = (IMyTextSurfaceProvider)b; // cockpits, consoles, button panels
                        if (provider.SurfaceCount > 0) surface = provider.GetSurface(0);
                    }
                    if (surface != null) AddScreen(surface, mode, arg);
                }
            }

            void AddScreen(IMyTextSurface s, int mode, int arg)
            {
                bool sprites = Display.IsSpriteMode(mode);
                s.ContentType = sprites ? ContentType.SCRIPT : ContentType.TEXT_AND_IMAGE;
                if (sprites)
                {
                    s.Script = "";
                    s.ScriptBackgroundColor = Color.Black;
                }
                if (mode == Display.ModeMenu)
                {
                    // Amber monospace, sized so the panel's columns and rows fill the screen.
                    s.Font = "Monospace";
                    s.FontColor = new Color(255, 170, 0);
                    s.BackgroundColor = Color.Black;
                    s.Alignment = TextAlignment.LEFT;
                    s.TextPadding = 2;
                    Vector2 m = s.MeasureStringInPixels(_probe, "Monospace", 1), a = s.SurfaceSize * 0.96f;
                    s.FontSize = Math.Min(a.X / m.X, a.Y / (m.Y * BrainFSM.MenuLines));
                }
                Screens.Add(s);
                ScreenMode.Add(mode);
                ScreenArg.Add(arg);
            }

            /// <summary>Parses the integer starting at s[start] (skipping spaces), or -1. No allocation.</summary>
            public static int ParseInt(string s, int start)
            {
                while (start < s.Length && s[start] == ' ') start++;
                int v = -1;
                while (start < s.Length && s[start] >= '0' && s[start] <= '9')
                {
                    v = (v < 0 ? 0 : v * 10) + (s[start] - '0');
                    start++;
                }
                return v;
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
                if (_cfg.IsBase)
                    return DockConnectors.Count == 0 ? "No connectors to offer as docks" : null;
                if (Controller == null) return "No ship controller (add a Remote Control)";
                if (Gyros.Count == 0) return "No gyroscopes";
                if (Thrusters.Count == 0) return "No thrusters";
                if (Connector == null) return "No connector";
                if (_cfg.Role == FleetRole.Miner && Drills.Count == 0 && Lasers.Count == 0) return "No drills or lasers";
                return null;
            }

            string Advise()
            {
                if (!AntennaReady) return "No broadcasting antenna: fleet can't hear us";
                if (_cfg.Role != FleetRole.Miner) return null;
                if (Camera == null) return "No forward camera: rock face found by touch";
                if ((_ejectors.Count > 0) != (_sorters.Count > 0)) return "Stone dump needs a sorter AND an ejector";
                return null;
            }

            // ---------------- Telemetry ----------------

            void RefreshTelemetry()
            {
                long cur = 0, max = 0;
                for (int i = 0; i < Ore.Count; i++) OreKg[i] = 0;
                bool countOre = !_cfg.IsBase;
                for (int i = 0; i < _inventories.Count; i++)
                {
                    var inv = _inventories[i];
                    cur += inv.CurrentVolume.RawValue;
                    max += inv.MaxVolume.RawValue;
                    if (!countOre || inv.ItemCount == 0) continue;
                    _items.Clear();
                    inv.GetItems(_items);
                    for (int k = 0; k < _items.Count; k++)
                    {
                        var item = _items[k];
                        if (item.Type.TypeId != Ore.TypeId) continue;
                        int ore = Ore.Index(item.Type.SubtypeId);
                        if (ore >= 0) OreKg[ore] += item.Amount.RawValue * 1e-6;
                    }
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
                double drain = 0;
                for (int i = 0; i < _batteries.Count; i++)
                    drain += _batteries[i].CurrentOutput - _batteries[i].CurrentInput;
                BatteryDrainMW = drain;
                _batteryStoredMWh = stored;

                double h2 = 0;
                for (int i = 0; i < _h2Tanks.Count; i++)
                    h2 += _h2Tanks[i].FilledRatio;
                HydrogenFill = _h2Tanks.Count > 0 ? h2 / _h2Tanks.Count : 1;

                bool damaged = false;
                for (int i = 0; i < Drills.Count && !damaged; i++)
                    damaged = Drills[i].Closed || !Drills[i].IsFunctional;
                DrillsDamaged = damaged;

                int working = 0;
                for (int i = 0; i < _all.Count; i++)
                    if (!_all[i].Closed && _all[i].IsFunctional) working++;
                Integrity = _all.Count > 0 ? (double)working / _all.Count : 1;

                // Threats: anything a turret targets, or an enemy a tagged sensor sees. Keep the
                // nearest located one so a drone can run away from it.
                bool threat = false;
                MyDetectedEntityInfo found = new MyDetectedEntityInfo();
                for (int i = 0; i < _turrets.Count; i++)
                    if (_turrets[i].IsWorking && _turrets[i].HasTarget)
                    {
                        threat = true;
                        Nearer(ref found, _turrets[i].GetTargetedEntity());
                    }
                for (int i = 0; i < _turretControllers.Count; i++)
                    if (_turretControllers[i].IsWorking && _turretControllers[i].HasTarget)
                    {
                        threat = true;
                        Nearer(ref found, _turretControllers[i].GetTargetedEntity());
                    }
                for (int i = 0; i < _sensors.Count; i++)
                {
                    if (!_sensors[i].IsWorking || !_sensors[i].IsActive) continue;
                    _detected.Clear();
                    _sensors[i].DetectedEntities(_detected);
                    for (int k = 0; k < _detected.Count; k++)
                    {
                        if (_detected[k].Relationship != MyRelationsBetweenPlayerAndBlock.Enemies) continue;
                        threat = true;
                        Nearer(ref found, _detected[k]);
                    }
                }
                if (_wcThreats != null)
                {
                    _threats.Clear();
                    _wcThreats(_p.Me, _threats);
                    foreach (var kv in _threats)
                    {
                        threat = true;
                        Nearer(ref found, kv.Key);
                    }
                }
                ThreatDetected = threat;
                ShieldPercent = _shield != null && !_shield.Closed && _dsPercent != null ? _dsPercent(_shield) : -1;
                bool locked = false;
                for (int i = 0; i < _gears.Count && !locked; i++)
                    locked = _gears[i].IsLocked;
                GearLocked = locked;
                ThreatLocated = !found.IsEmpty();
                ThreatPos = ThreatLocated ? found.Position : Vector3D.Zero;
                ThreatVel = ThreatLocated ? (Vector3D)found.Velocity : Vector3D.Zero;
            }

            void Nearer(ref MyDetectedEntityInfo best, MyDetectedEntityInfo candidate)
            {
                if (candidate.IsEmpty()) return;
                Vector3D me = _p.Me.GetPosition();
                if (best.IsEmpty() || Vector3D.DistanceSquared(me, candidate.Position) < Vector3D.DistanceSquared(me, best.Position))
                    best = candidate;
            }

            // ---------------- Sensing ----------------

            /// <summary>
            /// Raycasts at a world point (any direction inside the camera's cone). Returns false when
            /// that is not possible right now (no camera, outside the cone, still charging).
            /// voxelOnly: count only asteroids and planets as hits; otherwise ships count too.
            /// </summary>
            public bool RaycastAt(Vector3D target, bool voxelOnly, out bool hit, out Vector3D point)
            {
                bool voxel;
                bool done = RaycastAt(target, out hit, out voxel, out point);
                if (voxelOnly && !voxel) hit = false;
                return done;
            }

            /// <summary>As above, reporting whether the hit was rock (voxel) or something else (ship, person).</summary>
            public bool RaycastAt(Vector3D target, out bool hit, out bool voxel, out Vector3D point)
            {
                hit = voxel = false;
                point = Vector3D.Zero;
                var cam = Camera;
                if (cam == null || !cam.IsWorking || !cam.CanScan(target)) return false;
                MyDetectedEntityInfo info = cam.Raycast(target);
                if (info.IsEmpty() || !info.HitPosition.HasValue) return true;
                voxel = info.Type == MyDetectedEntityType.Asteroid || info.Type == MyDetectedEntityType.Planet;
                hit = true;
                point = info.HitPosition.Value;
                return true;
            }

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

            // ---------------- Self-test probes ----------------

            public bool AntennaReady
            {
                get
                {
                    for (int i = 0; i < _antennas.Count; i++)
                        if (_antennas[i].IsWorking && _antennas[i].EnableBroadcasting) return true;
                    for (int i = 0; i < _lasers.Count; i++)
                        if (_lasers[i].IsWorking) return true;
                    return false;
                }
            }

            /// <summary>True when the conveyors can carry stone from the drills to an ejector.</summary>
            public bool StonePathReady
            {
                get
                {
                    if (!CanEject || Drills.Count == 0) return true;
                    var stone = MyItemType.MakeOre("Stone");
                    var src = Drills[0].GetInventory(0);
                    for (int i = 0; i < _ejectors.Count; i++)
                        if (src.CanTransferItemTo(_ejectors[i].GetInventory(0), stone)) return true;
                    return false;
                }
            }

            public bool CameraReady { get { return Camera != null && Camera.IsWorking; } }

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

            /// <summary>
            /// Event hooks: triggers timers and plays sound blocks whose name contains
            /// <paramref name="tag"/> (e.g. "[FM Docked]", "[FM Distress]").
            /// </summary>
            public void FireHooks(string tag)
            {
                for (int i = 0; i < _timers.Count; i++)
                {
                    var t = _timers[i];
                    if (!t.Closed && t.IsWorking && t.CustomName.Contains(tag)) t.Trigger();
                }
                for (int i = 0; i < _sounds.Count; i++)
                {
                    var s = _sounds[i];
                    if (!s.Closed && s.IsWorking && s.CustomName.Contains(tag)) s.Play();
                }
            }

            // ---------------- Mod integrations ----------------

            /// <summary>Forward-facing lasers of one kind (Adjustable Mining Laser preferred over ToolCore).</summary>
            void FindLasers()
            {
                Lasers.Clear();
                if (Controller == null || _cfg.Role != FleetRole.Miner) return;
                _p.GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(_toolBlocks, _onThisGrid);
                bool large = _p.Me.CubeGrid.GridSizeEnum == MyCubeSize.Large;
                MatrixD refM = Controller.WorldMatrix;
                LaserForward = 0;
                for (int pass = 0; pass < 2 && Lasers.Count == 0; pass++)
                {
                    string id = pass == 0 ? AldFire : TcShoot;
                    for (int i = 0; i < _toolBlocks.Count; i++)
                    {
                        var b = _toolBlocks[i];
                        if (!(b is IMyShipWelder || b is IMyConveyorSorter) || b.GetProperty(id) == null) continue;
                        if (Vector3D.Dot(b.WorldMatrix.Forward, refM.Forward) < 0.98) continue;
                        Lasers.Add(b);
                        LaserForward = Math.Max(LaserForward, Vector3D.Dot(b.GetPosition() - refM.Translation, refM.Forward));
                    }
                }
                if (Lasers.Count == 0) return;

                var first = Lasers[0];
                LasersToolCore = first.GetProperty(AldFire) == null;
                if (LasersToolCore)
                {
                    _fireProp = first.GetProperty(TcShoot).As<bool>();
                    var mode = first.GetProperty(TcMode);
                    _modeProp = mode != null ? mode.As<long>() : null;
                    _rangeProp = _radiusProp = _contactProp = null;
                    _blockedProp = _noPowerProp = null;
                    // Simple Laser Multitools: 300 m x 2.5 m (large), 100 m x 1.5 m (small).
                    LaserRange = _cfg.LaserRange > 0 ? _cfg.LaserRange : large ? 300 : 100;
                    LaserRadius = _cfg.LaserRadius > 0 ? _cfg.LaserRadius : large ? 2.5 : 1.5;
                    return;
                }

                _fireProp = first.GetProperty(AldFire).As<bool>();
                _modeProp = null;
                _rangeProp = Prop<float>(first, large ? "AdjustableLaserDrill.RangeLarge" : "AdjustableLaserDrill.RangeSmall");
                _radiusProp = Prop<float>(first, large ? "AdjustableLaserDrill.RadiusLarge" : "AdjustableLaserDrill.RadiusSmall");
                _contactProp = Prop<float>(first, AldContact);
                _blockedProp = Prop<bool>(first, AldBlocked);
                _noPowerProp = Prop<bool>(first, AldNoPower);
                // Mod defaults: max range 60 m (large) / 20 m (small). The terminal "radius" slider is a diameter.
                LaserRange = _cfg.LaserRange > 0 ? _cfg.LaserRange : large ? 60 : 20;
                LaserRadius = _cfg.LaserRadius > 0 ? _cfg.LaserRadius
                    : _radiusProp != null ? _radiusProp.GetValue(first) * 0.5 : large ? 2.5 : 1.5;
                var stone = first.GetProperty(AldStone);
                if (stone != null)
                    for (int i = 0; i < Lasers.Count; i++)
                        stone.As<bool>().SetValue(Lasers[i], !_cfg.KeepStone);
            }

            static ITerminalProperty<T> Prop<T>(IMyTerminalBlock b, string id)
            {
                var p = b.GetProperty(id);
                return p != null ? p.As<T>() : null;
            }

            /// <summary>
            /// Fires or stops every laser. ToolCore tools are switched to Drill mode first and only
            /// fire if the mode reads back as Drill: a grinding beam would cut through ships.
            /// Returns whether the lasers are now firing.
            /// </summary>
            public bool SetLasers(bool on)
            {
                if (Lasers.Count == 0 || _fireProp == null) return false;
                bool ok = true;
                if (on && LasersToolCore)
                    for (int i = 0; i < Lasers.Count; i++)
                    {
                        var l = Lasers[i];
                        if (_modeProp == null) { ok = false; break; }
                        if (_modeProp.GetValue(l) != TcDrill) _modeProp.SetValue(l, TcDrill);
                        if (_modeProp.GetValue(l) != TcDrill) ok = false;
                    }
                LaserModeOk = ok;
                bool fire = on && ok;
                for (int i = 0; i < Lasers.Count; i++)
                {
                    var l = Lasers[i];
                    if (fire && !((IMyFunctionalBlock)l).Enabled) ((IMyFunctionalBlock)l).Enabled = true;
                    if (_fireProp.GetValue(l) != fire) _fireProp.SetValue(l, fire);
                }
                return fire;
            }

            /// <summary>Adjustable Mining Laser: beam length in metres (stops it at the shaft bottom).</summary>
            public void SetLaserRange(double metres)
            {
                if (_rangeProp == null) return;
                float r = (float)Math.Max(1, Math.Min(LaserRange, metres));
                for (int i = 0; i < Lasers.Count; i++)
                    if (Math.Abs(_rangeProp.GetValue(Lasers[i]) - r) > 0.5f) _rangeProp.SetValue(Lasers[i], r);
            }

            /// <summary>Distance from the emitter to the rock it is cutting, -1 when unknown (Adjustable Mining Laser).</summary>
            public double LaserContact
            {
                get
                {
                    double best = -1;
                    if (_contactProp == null) return best;
                    for (int i = 0; i < Lasers.Count; i++)
                    {
                        double c = _contactProp.GetValue(Lasers[i]);
                        if (c >= 0 && (best < 0 || c < best)) best = c;
                    }
                    return best;
                }
            }

            /// <summary>Adjustable Mining Laser reports a full drill buffer or too little power.</summary>
            public bool LaserBlocked
            {
                get
                {
                    for (int i = 0; i < Lasers.Count; i++)
                        if ((_blockedProp != null && _blockedProp.GetValue(Lasers[i]))
                            || (_noPowerProp != null && _noPowerProp.GetValue(Lasers[i]))) return true;
                    return false;
                }
            }

            /// <summary>
            /// Links the WeaponCore and Defense Shields programmable block APIs when those mods are
            /// loaded. Retried on every rescan, since mods may finish loading after the script.
            /// </summary>
            void LinkModApis()
            {
                Delegate d;
                if (_wcThreats == null)
                {
                    var api = Api("WcPbAPI");
                    if (api != null && api.TryGetValue("GetSortedThreats", out d))
                        _wcThreats = d as Action<IMyTerminalBlock, IDictionary<MyDetectedEntityInfo, float>>;
                }
                if (_dsPercent == null)
                {
                    var api = Api("DefenseSystemsPbAPI");
                    if (api != null && api.TryGetValue("GetShieldPercent", out d)) _dsPercent = d as Func<IMyTerminalBlock, float>;
                    if (api != null && api.TryGetValue("GetShieldBlock", out d)) _dsShieldBlock = d as Func<IMyEntity, IMyTerminalBlock>;
                }
                if (_dsShieldBlock != null) _shield = _dsShieldBlock(_p.Me.CubeGrid);
            }

            IReadOnlyDictionary<string, Delegate> Api(string property)
            {
                var prop = _p.Me.GetProperty(property);
                return prop != null ? prop.As<IReadOnlyDictionary<string, Delegate>>().GetValue(_p.Me) : null;
            }

            /// <summary>Drones: unlock every landing gear before flying (thumpers are landing gears too).</summary>
            public void ReleaseGear()
            {
                for (int i = 0; i < _gears.Count; i++)
                    if (_gears[i].IsLocked) _gears[i].Unlock();
            }

            public void SetDecoys(bool on)
            {
                for (int i = 0; i < _decoys.Count; i++)
                    if (_decoys[i].Enabled != on) _decoys[i].Enabled = on;
            }

            /// <summary>Hands the ship to an AI Defensive block tagged [FM Flee]. False when there is none.</summary>
            public bool StartAiFlee()
            {
                bool any = false;
                for (int i = 0; i < _fleeBlocks.Count; i++)
                {
                    var b = _fleeBlocks[i];
                    if (b.Closed || !b.IsFunctional) continue;
                    b.Enabled = true;
                    b.Flee();
                    any = true;
                }
                return any;
            }

            public void StopAiFlee()
            {
                for (int i = 0; i < _fleeBlocks.Count; i++)
                    _fleeBlocks[i].Enabled = false;
            }

            public void SetSearchlights(bool on)
            {
                for (int i = 0; i < _searchlights.Count; i++)
                    if (_searchlights[i].Enabled != on) _searchlights[i].Enabled = on;
            }

            /// <summary>Radio antenna range; only written when it changes by more than 10%.</summary>
            public void SetAntennaRange(float radius)
            {
                for (int i = 0; i < _antennas.Count; i++)
                {
                    float r = _antennas[i].Radius;
                    if (Math.Abs(r - radius) > r * 0.1f) _antennas[i].Radius = radius;
                }
            }

            /// <summary>Colours every [FM Light]; blink is the interval in seconds (0 = steady).</summary>
            public void SetLights(Color color, float blink)
            {
                _lightColor = color;
                _lightBlink = blink;
                for (int i = 0; i < _lights.Count; i++)
                {
                    var l = _lights[i];
                    l.Color = color;
                    l.BlinkIntervalSeconds = blink;
                    l.BlinkLength = 50f;
                }
            }

            /// <summary>Carrier: colours the pad lights of one dock slot.</summary>
            public void SetPadLight(int slot, Color color, float blink)
            {
                for (int i = 0; i < _padLights.Count; i++)
                {
                    if (_padSlot[i] != slot) continue;
                    var l = _padLights[i];
                    if (l.Color != color) l.Color = color;
                    if (l.BlinkIntervalSeconds != blink) l.BlinkIntervalSeconds = blink;
                }
            }

            /// <summary>Name shown on HUD for our antennas and beacons. Pass a cached string.</summary>
            public void SetHud(string text)
            {
                if (ReferenceEquals(text, _hudText)) return;
                _hudText = text;
                ApplyHud();
            }

            void ApplyHud()
            {
                for (int i = 0; i < _antennas.Count; i++) _antennas[i].HudText = _hudText;
                for (int i = 0; i < _beacons.Count; i++) _beacons[i].HudText = _hudText;
                // Lights found by a rescan pick up the current colour too.
                if (_lights.Count > 0) SetLights(_lightColor, _lightBlink);
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
                Transfer(_inventories, _unloadInventories, false);
            }

            /// <summary>
            /// Shuttle haulers at their carrier: pull ore out of the carrier's [FM Unload] containers
            /// (or all of them) into our cargo containers. Same cross-grid exception as Unload.
            /// </summary>
            public void Load()
            {
                if (!IsConnected) return;
                var other = Connector.OtherConnector;
                if (other == null) return;
                if (other.CubeGrid != _unloadGrid) FindUnloadTargets(other.CubeGrid);
                Transfer(_unloadInventories, _holds, true);
            }

            void Transfer(List<IMyInventory> from, List<IMyInventory> to, bool oreOnly)
            {
                if (to.Count == 0) return;
                int budget = MaxTransfersPerRun;
                for (int s = 0; s < from.Count && budget > 0; s++)
                {
                    var src = from[s];
                    if (src.ItemCount == 0) continue;
                    _items.Clear();
                    src.GetItems(_items);
                    for (int k = _items.Count - 1; k >= 0 && budget > 0; k--)
                    {
                        var item = _items[k];
                        if (oreOnly && item.Type.TypeId != Ore.TypeId) continue;
                        for (int t = 0; t < to.Count; t++)
                        {
                            var dst = to[t];
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

                bool tagged = false;
                for (int i = 0; i < _unloadBlocks.Count && !tagged; i++)
                    tagged = _unloadBlocks[i].CustomName.Contains(_cfg.UnloadTag);

                _unloadInventories.Clear();
                for (int i = 0; i < _unloadBlocks.Count; i++)
                {
                    var b = _unloadBlocks[i];
                    if (tagged && !b.CustomName.Contains(_cfg.UnloadTag)) continue;
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
                if (_cfg.IsBase) return;

                sb.Append("Cargo ");
                Fmt.Pct(sb, CargoFill).Append("  Batt ");
                Fmt.Pct(sb, BatteryCharge);
                if (HasHydrogen)
                {
                    sb.Append("  H2 ");
                    Fmt.Pct(sb, HydrogenFill);
                }
                sb.Append("  Hull ");
                Fmt.Pct(sb, Integrity).Append('\n');
                if (DrillsDamaged) sb.Append("!! Drill damaged\n");
                if (HasLasers)
                {
                    sb.Append(LasersToolCore ? "ToolCore lasers " : "Mining lasers ");
                    Fmt.Int(sb, Lasers.Count).Append("  r ");
                    Fmt.Fixed(sb, LaserRadius, 1).Append(" m  range ");
                    Fmt.Fixed(sb, LaserRange, 0).Append(" m\n");
                    if (!LaserModeOk) sb.Append("!! Laser not in Drill mode: holding fire\n");
                }
                if (ShieldPercent >= 0)
                {
                    sb.Append("Shield ");
                    Fmt.Fixed(sb, ShieldPercent, 0).Append("%\n");
                }
                if (GearLocked) sb.Append("! Landing gear locked\n");
            }
        }
    }
}
