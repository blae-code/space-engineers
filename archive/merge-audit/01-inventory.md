# Merge 01 · Inventory of the fleet work

2026-10-16 · read-only · CLAUDE(RIG).md §8 phase 1. Per-file roles were drafted by the local model (qwen3-coder-30b) and checked by Claude. Tags, keys and block lists are taken from the source by grep.

## Scope found

| Item from §8 notes | In this repo? | Where |
| --- | --- | --- |
| Mining drone (custom, PAM-class) | **Yes** | `Fleet.Drone.Miner` + `Fleet.Engine` + `Fleet.Flight` |
| Logistics vessel / carrier | **Partly**: carrier *PB* (bay beacon + dispatcher core), no ship design | `Fleet.Carrier`, `Fleet.Engine/Dispatch` |
| Mothership console | **Yes** (remote console PB) | `Fleet.Console` |
| Survey probe | **No.** Only the `jobreq` message is reserved for it | slice 3 spec §3 |
| Cargo drones | **No** | — |
| Combat carriers + swarm drones | **No** | — |
| Mother-Forge lore / Amarr styling | **No** files (not in repo, not under `~/Documents/ROOT/Code`) | Blae's notes only |
| Block mods | **None.** Every Fleet design is vanilla | — |

→ The merge is really **one drone class + 3 PBs + a shared library**. Probe, cargo and combat classes are new work for the Rig, not ports.

## Classes

| Class | Role | Grid | Size | Thrust | Cargo / power | Docking | PB | Mods |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Mining drone | Drill holes in a W×H grid, haul home, repeat | Small | Free; spec asks only for 6-way thrust and lift margin ≥ 1.3 | Any (atmo/ion/H2 handled uniformly) | Any inventories; BAT / H2 tank / reactor | Recorded dock path → connector, moving-carrier aware (bay beacon + HomeTracker) | `Fleet.Drone.Miner` (U1/U10/U100) | none |
| Carrier (PB only) | Broadcast bay poses (U10); host for Dispatcher (slice 3, unwired: C44) | Large | — | (flown by player / AI) | — | Tagged connectors = bays; `[FM] Hold` marker | `Fleet.Carrier` | none |
| Console (PB only) | Roster, per-drone menu, SET/GETCFG, commands | Any | — | — | — | — | `Fleet.Console` | none |

## Protocol (as built)

- Channel: `FLEET/<Channel>/<kind>`, default channel `FM`. Payloads are `|`-separated and versioned (`FleetLink.Version` = 2). Numbers are integers: mm, millionths.
- Kinds:

| Kind | Direction | Payload head |
| --- | --- | --- |
| `status` | drone → broadcast, U100 | `2|name|state|reason|hole|holes|cargo|bat|h2|lift|flags|job|lease|mask|note` |
| `cmd` | console → drone, unicast | PB-argument text; `SET <sec> <key> <val>`, `GETCFG` |
| `cfg` / `ack` | drone → console | Custom Data dump / echo |
| `bay` | carrier → broadcast, U10 | bay poses + velocity; hold marker = BayId −1 |
| `job` · `jobreq` · `lease` · `dock` | slice 3 (codec + Dispatcher built, carrier unwired) | `J|2|…` · `Q|2|…` · `N/L|2|…` · `R/G/W|2|…` |

## Config (Custom Data, `MyIni`)
- Drone sections: `Fleet` (Name, Tag `[FM]`, Channel), `Miner`, `Flight`, `Energy`, `Reload`.
- One schema (`SettingsSchema`) is pinned to `ConfigLoader` ranges by a test.
- Console: `[Console]` Channel, Tag, `[Targets]`. Carrier: `[Carrier]` Channel, Tag, DisplayTag.
- Name tag: `[FM]` for LCDs, ejectors and home containers; `[FM:n]` selects a cockpit screen.

## Budgets and quality
- Script size: miner 66,853 / 100,000 chars (`minify=full`) · console 21,694 · carrier 5,949.
- Tests: 498 green (NUnit on Mono). Gates in `tools/`: card-check, hot-path allocation scan, size, snapshot, git hooks.
- Zero-GC per-tick rule enforced by gate. Enum names via literal tables (minifier landmine).
- No third-party code copied in. PAM was a behavioural reference only.
- Safety: the miner's damage/low-energy returns are PB logic. There is **no native D1–D4 / AR RECALL fallback yet** (needed for S1).

## Files

| File | Lines | Role | Game blocks | IGC |
| --- | ---: | --- | --- | :---: |
| `Program.cs` (Carrier) | 151 | Broadcasts carrier bay poses and velocities for drone docking on a moving carrier | ShipConnector, ShipController, TerminalBlock | yes |
| `Program.cs` (Console) | 262 | A fleet console for managing and communicating with drones in a Space Engineers game, displaying their status and allowing command execution | TerminalBlock | yes |
| `Io/CargoIO.cs` (Drone.Miner) | 174 | Manages inventory operations for a drone miner, including filling status calculation, ore detection, and transferring items to ejectors or unload targets | Inventory, ShipConnector, TerminalBlock |  |
| `Io/DamageScanner.cs` (Drone.Miner) | 85 | Scans blocks for damage by comparing current integrity against a baseline | SlimBlock, TerminalBlock |  |
| `Io/EnergyIO.cs` (Drone.Miner) | 104 | Manages energy-related operations for batteries, hydrogen tanks, and reactors in a Space Engineers drone miner | BatteryBlock, GasTank, Inventory, Reactor |  |
| `Mining/DrillLogic.cs` (Drone.Miner) | 83 | Manages the drilling logic for a mining drone, controlling advance and retract phases based on depth and speed | — |  |
| `Mining/HoleGrid.cs` (Drone.Miner) | 87 | Provides utility functions for calculating drill hole patterns and positions in a mining grid | — |  |
| `Mining/LeaseCursor.cs` (Drone.Miner) | 67 | Manages the state of leased mining holes for a drone in a fleet job, tracking which holes are done and providing methods to query and update this state | — |  |
| `Mining/MinerFsm.cs` (Drone.Miner) | 265 | Manages the finite state machine for a mining drone's operational logic, handling commands, faults, and state transitions | — |  |
| `Mining/MiningStats.cs` (Drone.Miner) | 108 | Tracks mining statistics for a drone, including trips, ore mass, and job duration | — |  |
| `Mining/Reconciler.cs` (Drone.Miner) | 93 | Resolves the appropriate miner state based on input conditions and reload policy | — |  |
| `Mining/ReturnTriggers.cs` (Drone.Miner) | 31 | Evaluates conditions to determine when a mining drone should return based on cargo fill, resource levels, damage status, and lift margin | — |  |
| `Program.cs` (Drone.Miner) | 157 | Controls a fleet of mining drones with remote capabilities, inventory management, and UI feedback | Gyro, ShipController, ShipDrill, Thrust |  |
| `Subsystems/BlockScan.cs` (Drone.Miner) | 160 | Scans and organizes ship blocks for drone mining operations, including controllers, connectors, drills, power sources, and inventories | BatteryBlock, CubeGrid, GasTank, Gyro, LaserAntenna, ProgrammableBlock, RadioAntenna, Reactor, ShipConnector, ShipController, ShipDrill, TerminalBlock, Thrust |  |
| `Subsystems/MinerSubsystem.cs` (Drone.Miner) | 829 | Manages the drilling and mining operations for a fleet drone, handling job definition, path planning, state machine execution, and communication with carrier systems | CargoContainer, Inventory |  |
| `Subsystems/RemoteSubsystem.cs` (Drone.Miner) | 133 | Manages remote communication for a drone miner, handling status broadcasting, command execution, and configuration updates via IGC | — | yes |
| `Subsystems/SenseSubsystem.cs` (Drone.Miner) | 71 | The SenseSubsystem updates the blackboard with ship dynamics, resource levels, and connectivity status every 10 ticks | — |  |
| `Subsystems/UiSubsystem.cs` (Drone.Miner) | 173 | Manages the user interface and menu system for a drone miner, including rendering status information and handling user commands | — |  |
| `Config/ConfigLoader.cs` (Engine) | 243 | The ConfigLoader class provides functionality to load and write configuration settings for a Space Engineers fleet from and to INI-formatted text, including validation and error reporting | — |  |
| `Config/Settings.cs` (Engine) | 39 | Defines configuration settings for a fleet mining operation in Space Engineers | — |  |
| `Config/SettingsSchema.cs` (Engine) | 186 | Defines a schema for editable configuration settings used in a Space Engineers programmable block fleet management system | — |  |
| `Core/Blackboard.cs` (Engine) | 29 | Defines a Blackboard class to store and manage various ship-related state information for use in a Space Engineers MDK2 programmable block script | — |  |
| `Core/CommandParser.cs` (Engine) | 80 | Parses command strings into enumerated command types and returns the remaining unparsed string | — |  |
| `Core/Enums.cs` (Engine) | 17 | Defines enumerations and constant string arrays used to represent states, reasons, and commands for fleet mining operations | — |  |
| `Core/EventLog.cs` (Engine) | 77 | Manages a circular buffer log of miner state transition events with timing and optional notes | — |  |
| `Core/ISubsystem.cs` (Engine) | 22 | Defines the interface for subsystems within a Space Engineers programmable block fleet management system | — |  |
| `Core/Kernel.cs` (Engine) | 96 | Manages the execution of subsystems within a Space Engineers programmable block, handling safe mode entry and profiling | — |  |
| `Core/PoseTarget.cs` (Engine) | 18 | Defines a struct for specifying pose targets with position, orientation, speed, and motion parameters for space vessel navigation | — |  |
| `Core/Profiler.cs` (Engine) | 106 | A circular buffer-based profiler for tracking instruction counts and execution time over a sliding window | — |  |
| `Core/StorageStore.cs` (Engine) | 48 | Provides methods to serialize and deserialize subsystem storage data using MyIni for a Space Engineers programmable block | — |  |
| `Dispatch/BayQueue.cs` (Engine) | 147 | Manages a queue of drones waiting to dock on available bays, with reservation logic and FIFO queuing | — |  |
| `Dispatch/Dispatcher.cs` (Engine) | 181 | Manages fleet coordination tasks including job distribution, lease management, docking requests, and status updates for a Space Engineers drone fleet | — |  |
| `Dispatch/HoldSlots.cs` (Engine) | 28 | Provides functionality for calculating and retrieving the 3D positions of drone holding slots relative to a marker block in a fleet dispatch system | — |  |
| `Dispatch/LeaseTable.cs` (Engine) | 190 | Manages the leasing of mining holes in a shared job between drones and carriers, tracking lease states and expiration | — |  |
| `Nav/Frames.cs` (Engine) | 29 | Provides methods for converting between world and local coordinate frames using matrix transformations | — |  |
| `Nav/Pid.cs` (Engine) | 133 | Provides PID (Proportional-Integral-Derivative) control logic for navigation and motion control in a Space Engineers fleet management system | — |  |
| `Net/Beacon.cs` (Engine) | 113 | Defines beacon data structures and tracking logic for fleet carrier bay poses in a Space Engineers MDK2 project | — |  |
| `Net/FleetLink.cs` (Engine) | 144 | Defines the drone status structure and IGC communication protocol for fleet coordination in Space Engineers | — |  |
| `Net/FleetMsg.cs` (Engine) | 139 | Defines message encoding and decoding logic for fleet coordination in Space Engineers, including job requests, lease allocations, and docking commands | — |  |
| `Net/JobCodec.cs` (Engine) | 105 | Encodes and decodes fleet mining job data between string representations for network transmission | — |  |
| `Net/Roster.cs` (Engine) | 145 | Manages a fleet roster of drones with their status, configuration, and pending edits | — |  |
| `Ui/Displays.cs` (Engine) | 81 | Manages text displays on a Space Engineers grid by collecting and updating text surfaces based on block names and tags | TerminalBlock, TextPanel, TextSurface, TextSurfaceProvider |  |
| `Ui/Menu.cs` (Engine) | 306 | Provides a menu system for drone configuration with settings, commands, and navigation | — |  |
| `Util/SbFormat.cs` (Engine) | 113 | Provides utility methods for formatting numbers and time values into StringBuilder objects | — |  |
| `Flight/BrakingCurve.cs` (Flight) | 37 | Provides methods for calculating safe braking speeds and stopping distances based on distance, acceleration, and margin parameters | — |  |
| `Flight/DockingPlanner.cs` (Flight) | 99 | Manages the docking process for a drone by planning its approach, alignment, creeping, and connection phases | — |  |
| `Flight/GpsRoute.cs` (Flight) | 175 | Provides GPS parsing utilities and a flight path routing system for space vessel navigation | — |  |
| `Flight/GyroMath.cs` (Flight) | 84 | Provides mathematical utilities for calculating angular velocity and gyro override values based on orientation and target vectors | — |  |
| `Flight/Helm.cs` (Flight) | 92 | Controls ship thrusters and gyros based on target pose using PID control and thrust allocation | — |  |
| `Flight/PathCodec.cs` (Flight) | 46 | Provides methods to save and load a list of 3D coordinates to/from an INI file format with rounded decimal precision | — |  |
| `Flight/PathFollower.cs` (Flight) | 143 | Manages path following for a fleet of ships, calculating waypoints and speed adjustments based on corner angles | — |  |
| `Flight/PathRecorder.cs` (Flight) | 57 | Records flight path points with dynamic spacing based on speed, up to a maximum number of points | — |  |
| `Flight/StuckDetector.cs` (Flight) | 62 | Detects when a ship has been moving at less than a minimum speed for a specified duration while thrusting, indicating it may be stuck | — |  |
| `Flight/ThrustAllocator.cs` (Flight) | 195 | Allocates thrust ratios for a ship's thrusters based on desired local force and maximum thrust capabilities, while calculating saturation levels | — |  |
| `Io/IShipIO.cs` (Flight) | 20 | Defines an interface for controlling ship physics and thrusters in a Space Engineers programmable block | — |  |
| `Io/ShipIO.cs` (Flight) | 245 | Provides a wrapper for ship control blocks (thrusters and gyros) to manage thrust and gyro overrides, including a calibration test routine | Gyro, ShipController, Thrust |  |
