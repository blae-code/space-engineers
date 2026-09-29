# Space Engineers MDK Project: FleetMiner_Core

## Project Overview
This repository contains a modular C# Ingame Script for Space Engineers, built using the Malware Dev Kit (MDK) for Visual Studio. The script utilizes a component-based architecture to manage a fleet of semi-autonomous drones (miners, cargo haulers, logistics carriers) via Intergrid Communication (IGC).

## Build & Deploy Process
* **Do not use standard `dotnet build`.**
* Deployment is handled exclusively by the MDK Visual Studio Extension.
* **To Deploy:** Click `Deploy MDK Script` in the Visual Studio extensions menu. This minifies the multi-file project, resolves `using` statements, and outputs a single `readme.txt` and `script.cs` to `%AppData%\SpaceEngineers\IngameScripts\local\`.

## Architectural Rules & Guardrails
1. **Component-Based:** Logic is split into modules (`GridManager`, `CommsOfficer`, `HelmController`, `BrainFSM`) that implement the `ISubsystem` interface.
2. **Frequency Throttling:**
   * High-frequency math/physics runs on `Update10` (every 10 ticks).
   * Low-frequency network/UI runs on `Update100` (every 100 ticks).
   * Sole exception: a carrier streams `DockBeacon` unicasts on `Update10`, and only to drones holding an active docking lease. Moving docking cannot converge on 1.6 s-old poses.
3. **Zero-GC in the Main Loop:**
   * **Never** instantiate new objects (`new List<T>()`, `new StringBuilder()`, etc.) or concatenate strings dynamically inside `Update10`, `Update100`, or `Main()`.
   * Pre-allocate all collections during initialization (`Program()`) and use `.Clear()` to reuse them.
   * `new Vector3D(...)` / `MatrixD` values are structs (stack), which is fine.
   * `StringBuilder.Append(int/double)` allocates a string internally. Use the `Fmt` helpers in `Program.cs`.
   * Use `IMyTextSurface.WriteText(StringBuilder)` for UI, not `Echo(sb.ToString())`.
4. **Grid Scoping:** Always filter block queries with `b => b.CubeGrid == Me.CubeGrid` to avoid hijacking docked ships.
5. **Moving Docking:** Docking and flight approaches must rely on relative matrix transformations (`Vector3D.TransformNormal` and `WorldMatrix`), not static GPS coordinates.

## API Restrictions (Space Engineers Sandbox)
* **Banned Namespaces:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`.
* **LINQ:** Avoid LINQ in high-frequency loops (causes overhead and garbage collection).
* **Language level:** Stick to C# 6 (no tuples, `out var`, pattern matching, or local functions).
* **State Preservation:** Volatile state (home vectors, current FSM state) must be serialized to the `Storage` string in the `Save()` method and parsed in the `Program()` constructor to survive world reloads.

## File Structure
* `Program.cs` - The kernel. Initializes modules and routes ticks/IGC messages. Also holds `Config` (Custom Data) and `Fmt` (allocation-free number formatting).
* `ISubsystem.cs` - The contract (`Initialize`, `Update10`, `Update100`, `HandleMessage`).
* `GridManager.cs` - Block caching and terminal interactions.
* `CommsOfficer.cs` - IGC mesh networking, payload serialization/deserialization.
* `HelmController.cs` - Matrix math, gyro overrides, and thruster control.
* `BrainFSM.cs` - Finite state machine dictating the drone's current objective.
