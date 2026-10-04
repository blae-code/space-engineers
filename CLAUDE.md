# Space Engineers MDK Project: FleetMiner_Core

## Project Overview
This repository contains a modular C# Ingame Script for Space Engineers, built using the Malware Dev Kit (MDK) for Visual Studio. The script utilizes a component-based architecture to manage a fleet of semi-autonomous drones (miners, cargo haulers, logistics carriers) via Intergrid Communication (IGC). One script, four roles (`Role` in Custom Data): Miner, Hauler (site or shuttle), Carrier, Mothership. User-facing guide: `docs/MinerDrone.md`.

## Build & Deploy Process
* **Do not use standard `dotnet build`.**
* Deployment is handled exclusively by the MDK Visual Studio Extension.
* **To Deploy:** Click `Deploy MDK Script` in the Visual Studio extensions menu. This minifies the multi-file project, resolves `using` statements, and outputs a single `readme.txt` and `script.cs` to `%AppData%\SpaceEngineers\IngameScripts\local\`.

## Architectural Rules & Guardrails
1. **Component-Based:** Logic is split into modules (`GridManager`, `CommsOfficer`, `HelmController`, `BrainFSM`, `Display`) that implement the `ISubsystem` interface.
2. **Frequency Throttling:**
   * High-frequency math/physics runs on `Update10` (every 10 ticks).
   * Low-frequency network/UI runs on `Update100` (every 100 ticks).
   * Sole exception: a pad server (carrier, mothership, or site hauler on station) streams `DockBeacon` unicasts on `Update10`, and only to ships holding an active docking lease. Moving docking cannot converge on 1.6 s-old poses.
3. **Zero-GC in the Main Loop:**
   * **Never** instantiate new objects (`new List<T>()`, `new StringBuilder()`, etc.) or concatenate strings dynamically inside `Update10`, `Update100`, or `Main()`.
   * Pre-allocate all collections during initialization (`Program()`) and use `.Clear()` to reuse them.
   * `new Vector3D(...)` / `MatrixD` values are structs (stack), which is fine.
   * `StringBuilder.Append(int/double)` allocates a string internally. Use the `Fmt` helpers in `Program.cs`.
   * Use `IMyTextSurface.WriteText(StringBuilder)` for UI, not `Echo(sb.ToString())`.
   * Sprites need real strings: use literals, cached strings (callsigns, HUD labels built in `Initialize`) or `Fmt.Str` (cached 0..999).
   * Log entries (`Logbook.Add`) store references only; pass literals or existing strings, never built ones.
4. **Grid Scoping:** Always filter block queries with `b => b.CubeGrid == Me.CubeGrid` to avoid hijacking docked ships.
   * Sole exception: a docked ship moving cargo queries the other side's containers, reached only via `Connector.OtherConnector.CubeGrid` (`GridManager.Unload` / `Load`).
5. **Moving Docking:** Docking and flight approaches must rely on relative matrix transformations (`Vector3D.TransformNormal` and `WorldMatrix`), not static GPS coordinates.
   * Static points are fine for static things (the asteroid site, flee points) and as a last resort when out of contact (a lost-link drone flies to its carrier's last reported position, then waits for the link).
6. **Comms:** every message goes through `CommsOfficer`, which stamps the fleet-key header. Anything that must arrive (deliveries, site charts, shaft results, uplinks) uses `SendReliable`; anything repeated anyway (status, claims, beacons, survey) is sent best effort. Drone traffic stays on the local channel; carrier/mothership/shuttle traffic uses the HQ channel (`FleetMessage.Hq`).

## API Restrictions (Space Engineers Sandbox)
* **Banned Namespaces:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`.
* **LINQ:** Avoid LINQ in high-frequency loops (causes overhead and garbage collection).
* **Language level:** Stick to C# 6 (no tuples, `out var`, pattern matching, or local functions).
* **Packing drops `using` directives.** Only the game's default imports survive, and `System.Globalization` is not one of them: write `System.Globalization.CultureInfo` in full.
* **Size:** the packed script must stay under 100,000 characters (≈94k after the control menu). Trim or split roles before adding another large feature.
* **Mod integrations** (laser drills, WeaponCore, Defense Shields) are optional and found at run time by terminal property / PB API name (`GridManager.FindLasers`, `LinkModApis`). Cache `ITerminalProperty<T>` / delegates on rescan; never look them up per tick. A ToolCore tool must read back `ToolCore_Mode == 4` (Drill) before it is ever fired.
* **State Preservation:** Volatile state (home vectors, current FSM state) must be serialized to the `Storage` string in the `Save()` method and parsed in the `Program()` constructor to survive world reloads.

## File Structure
* `Program.cs` - The kernel. Initializes modules and routes ticks/IGC messages. Also holds `Config` (Custom Data, table-driven for the `set` command), `Ore` (ore catalogue), `Logbook` (comms log ring) and `Fmt` (allocation-free formatting).
* `ISubsystem.cs` - The contract (`Initialize`, `Update10`, `Update100`, `HandleMessage`).
* `GridManager.cs` - Block caching and terminal interactions: telemetry, ore counts, integrity, threats (with position), raycasts, unloading/loading, stone dump, hauler bays, decoys, AI flee, searchlights, antenna range, screens, lights, HUD text, timer/sound hooks.
* `CommsOfficer.cs` - IGC on two channels (local + HQ), fleet-key header, reliable outbox with acks and duplicate filtering, reachability, and the peer table.
* `HelmController.cs` - Matrix math, gyro overrides, thruster control, Remote Control autopilot legs, lift measurement, planet altitude floor.
* `SiteMap.cs` - A mining site: frame, spiral shaft layout, per-shaft status/yield, ore-aware shaft picking. Shared by drones and the carrier library.
* `BrainFSM.cs` - Drone state machine (miners, both hauler modes): mining sessions, survey, look-ahead, breadcrumbs, energy model, dock-target choice, holding, link watch/re-homing, traffic, evasion.
* `BrainFSM.Bases.cs` - Pad server (carrier, mothership, site-hauler bays), carrier flight control (queue, launch sequence), site library + map sync, carrier <-> mothership uplink.
* `BrainFSM.Io.cs` - Commands, self-test, status text, persistence (`Storage` as MyIni).
* `BrainFSM.Menu.cs` - The `[FM Menu]` control panel: up/down/select/back navigation (actions reuse the text commands) and the 40-column ASCII renderer.
* `Display.cs` - Every screen: text pages (status, detail, log, stats, menu) and sprite pages (gauges, site map, board).
