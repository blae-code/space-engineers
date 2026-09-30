# Space Engineers MDK Project: FleetMiner_Core

## Project Overview
This repository contains a modular C# Ingame Script for Space Engineers, built with **MDK2**
(Malware Dev Kit 2, the NuGet/`dotnet`-based successor to the Visual Studio extension). The script
uses a component-based architecture to manage a fleet of semi-autonomous drones (miners, cargo
haulers, logistics carriers) via Intergrid Communication (IGC).

**This local repo is authoritative.** `origin` is `github.com/blae-code/space-engineers`; push only
when a milestone is called, never on routine commits.

## Build & Deploy Process (Linux / Proton — MDK2)
The dev box is CachyOS Linux; the game runs under Proton via **Flatpak Steam**. There is no Visual
Studio — MDK2 builds and deploys through the .NET SDK.

```
cd FleetMiner_Core
dotnet build -c Release     # compile vs the game's Bin64, merge/minify, DEPLOY to the game
```

* A successful build prints `Your script "FleetMiner_Core" has been successfully deployed.` and writes
  `script.cs` + `thumb.png` to the Proton prefix:
  `~/.var/app/com.valvesoftware.Steam/.local/share/Steam/steamapps/compatdata/244850/pfx/drive_c/users/steamuser/AppData/Roaming/SpaceEngineers/IngameScripts/local/FleetMiner_Core/`
  In-game: PB → Edit → Browse Scripts → `FleetMiner_Core`.
* Paths live in `FleetMiner_Core/mdk.local.ini` (`output=` deploy folder, `binarypath=` game `Bin64`).
  That file is machine-specific and **gitignored**; `mdk.ini` (minify level, ignores, namespaces) is
  tracked.
* **.NET SDK is user-local in `~/.dotnet`** (installed with Microsoft's `dotnet-install.sh`, because the
  pacman mirrors were out of sync with the local db). Fish gets it from
  `~/.config/fish/conf.d/dotnet.fish`. Both SDK 9 and 10 are installed.
* **Landmine — SDK pin:** the MDK2 packager (`Mal.Mdk2.PbPackager`) is a self-contained **.NET 9** app;
  its MSBuild locator rejects SDK 10 with *"Unable to find a valid MSBuild instance"* and the build
  fails at the pack step even though compilation succeeded. `global.json` at the repo root pins SDK 9
  — do not remove it or bump it past 9 until the packager targets a newer runtime.
* A green build proves the C# compiles against the real API and passes MDK's analyzers. It does NOT
  prove in-game behaviour — verify in-game.

## Architectural Rules & Guardrails
1. **Component-Based:** Logic is split into modules (`GridManager`, `CommsOfficer`, `HelmController`,
   `BrainFSM`) that implement the `ISubsystem` interface.
2. **Frequency Throttling:**
   * High-frequency math/physics runs on `Update10` (every 10 ticks).
   * Low-frequency network/UI runs on `Update100` (every 100 ticks).
3. **Zero-GC in the Main Loop:**
   * **Never** instantiate new objects (`new List<T>()`, `new StringBuilder()`, etc.) or concatenate
     strings dynamically inside `Update10`, `Update100`, or `Main()`.
   * Pre-allocate all collections during initialization (`Program()`) and use `.Clear()` to reuse them.
4. **Grid Scoping:** Always filter block queries with `b => b.CubeGrid == Me.CubeGrid` to avoid
   hijacking docked ships.
5. **Moving Docking:** Docking and flight approaches must rely on relative matrix transformations
   (`Vector3D.TransformNormal` and `WorldMatrix`), not static GPS coordinates.

## API Restrictions (Space Engineers Sandbox)
* **Banned Namespaces:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`.
* **LINQ:** Avoid LINQ in high-frequency loops (causes overhead and garbage collection).
* **State Preservation:** Volatile state (home vectors, current FSM state) must be serialized to the
  `Storage` string in the `Save()` method and parsed in the `Program()` constructor to survive world
  reloads.
* **Namespace:** all code lives in `namespace IngameScript` inside `partial class Program` (MDK2
  convention, `namespaces=IngameScript` in `mdk.ini`); MDK strips it on deploy. Don't add other
  namespaces — the PB flattens them and same-named types collide.
* Language version is **C# 6** (`<LangVersion>6</LangVersion>`) — the in-game compiler's ceiling.

## File Structure (`FleetMiner_Core/`)
* `Program.cs` - The kernel. Initializes modules and routes ticks/IGC messages.
* `ISubsystem.cs` - The contract (`Initialize`, `Update10`, `Update100`, `HandleMessage`).
* `GridManager.cs` - Block caching and terminal interactions.
* `CommsOfficer.cs` - IGC mesh networking, payload serialization/deserialization.
* `HelmController.cs` - Matrix math, gyro overrides, and thruster control.
* `BrainFSM.cs` - Finite state machine dictating the drone's current objective.
