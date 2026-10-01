# Space Engineers MDK Project: Fleet (slice 1 — FleetMiner + remote console)

## Project Overview
This repository contains modular C# Ingame Scripts for Space Engineers, built with **MDK2**
(Malware Dev Kit 2, the NuGet/`dotnet`-based successor to the Visual Studio extension). A shared engine
drives a fleet of semi-autonomous drones (miners, cargo haulers, logistics carriers) via Intergrid
Communication (IGC). Slice 1 is a clean-room PAM successor: a solo small-grid mining drone, plus a
mothership **console** that sets up and commands drones remotely (spec §10 — commanded, not piloted).

**This local repo is authoritative.** `origin` is `github.com/blae-code/space-engineers`; push only
when a milestone is called, never on routine commits.

- Spec: `docs/superpowers/specs/2026-09-29-slice1-core-miner-design.md`
- Plan + card index + checkpoint log: `docs/superpowers/plans/2026-09-29-slice1-core-miner.md`
- Task cards: `docs/superpowers/plans/slice1-cards/C01…C30`

## Division of labour (Claude Code ↔ local model)
Work is split to save Claude credits. The **local model** (qwen3-coder-30b in Continue, Agent mode)
executes task cards: pure logic with verbatim tests. **Claude Code** owns framing (spec, plan, cards),
checkpoints A–D, escalations, and the game-API cards (C25 ShipIO, C30 integration).
- A card is gated by `python3 tools/card-check.py <Cxx>` (tests, verbatim-test tamper check, scope,
  banned namespaces, the card's own "Done when" tokens). Blae reviews `git diff` and commits.
- **The gate is enforced by git, not by the model's report:** `git config core.hooksPath tools/hooks`
  (repo-local, already set here) installs a pre-commit hook that refuses multi-card commits and any card
  commit whose `card-check --staged` is not PASS. `python3 tools/next-card.py` prints every card's status
  and the next one to hand out. `--no-verify` is for Claude's integration commits only, with the
  card-check output quoted in the commit message.
- A checkpoint starts from `python3 tools/checkpoint.py <A|B|C|D>` output — do not re-run what it ran.
- On escalation expect only the card id + card-check output. Fix small defects directly, or re-issue a
  corrected card; record the outcome in the plan's "Checkpoint log".
- Continue config in-repo: `.continue/rules/fleet-mdk.md` (auto-applied constraints),
  `.continue/prompts/card.md` (the `/card` command). Keep them in sync with the rules below.

## Build, test & deploy (Linux / Proton — MDK2)
The dev box is CachyOS Linux; the game runs under Proton via **Flatpak Steam**. There is no Visual
Studio — MDK2 builds and deploys through the .NET SDK; tests run on Mono.

```
dotnet build Fleet.Drone.Miner -c Release   # compile vs Bin64, minify, DEPLOY to the game
dotnet build Fleet.Console -c Release       # the mothership console script
fish tools/check-size.fish                  # every deployed script.cs < 100,000 chars (warn >= 90,000)
dotnet test Fleet.Tests                     # NUnit on Mono (net48)
dotnet test Fleet.Tests --filter "FullyQualifiedName~SbFormatTests"   # one class
```

* A successful build prints `Your script "Fleet.Drone.Miner" has been successfully deployed.` into the
  Proton prefix: `…/compatdata/244850/pfx/drive_c/users/steamuser/AppData/Roaming/SpaceEngineers/IngameScripts/local/Fleet.Drone.Miner/`.
  In-game: PB → Edit → Browse Scripts → `Fleet.Drone.Miner`. Every build of the PB project (including
  via `dotnet test`) redeploys; harmless.
* **`minify=full`** in both PB projects: the integrated drone measured 100,260 chars at `lite` (over the
  limit) and ~61k at `full`. Full includes **trim** (unreferenced types are dropped) and **renames every
  identifier, including enum members**.
* **Landmine — never depend on an identifier name at runtime.** Under `minify=full`, `enum.ToString()`,
  `Enum.GetValues`/`Parse` and `nameof` produce mangled names in-game while every unit test (which runs
  the unminified assembly) passes. Enum text comes from literal tables: `Names.State/Reason` for display,
  `SettingsSchema` choice names for Custom Data. Storage, INI and IGC keys are string literals.
* Paths live in `mdk.local.ini` (`output=`, `binarypath=`, `interactive=DoNothing`) — in
  `Fleet.Drone.Miner/`, `Fleet.Console/`, and `Fleet.Tests/` (binarypath only). Machine-specific and
  **gitignored**; `mdk.ini` is tracked. **`interactive=DoNothing` is required**: without it every build
  opens an "MDK Hub Not Found" page in the browser (the Hub is not installed on Linux).
* **.NET SDK is user-local in `~/.dotnet`** (installed with Microsoft's `dotnet-install.sh`, because the
  pacman mirrors were out of sync with the local db). Fish gets it from
  `~/.config/fish/conf.d/dotnet.fish`. Both SDK 9 and 10 are installed.
* **Landmine — SDK pin:** the MDK2 packager (`Mal.Mdk2.PbPackager`) is a self-contained **.NET 9** app;
  its MSBuild locator rejects SDK 10 with *"Unable to find a valid MSBuild instance"* and the build
  fails at the pack step even though compilation succeeded. `global.json` at the repo root pins SDK 9
  — do not remove it or bump it past 9 until the packager targets a newer runtime.
* **Landmine — mixin GUIDs:** `dotnet new mdk2mixin` stamps the same fixed GUID into every mixin. Give
  each new mixin a fresh GUID in both its `.shproj` and `.projitems`.
* A green build proves the C# compiles against the real API and passes MDK's analyzers. It does NOT
  prove in-game behaviour — verify in-game (plan's "In-game checklist").

## Structure
| Project | Kind | Contents |
|---|---|---|
| `Fleet.Engine/` | MDK2 mixin (shared) | `Core/` kernel, contracts, storage, log, commands · `Config/` · `Nav/` · `Util/` · `Ui/` |
| `Fleet.Flight/` | MDK2 mixin (shared) | `Flight/` pure flight math (braking, thrust, gyro, paths, docking, helm) · `Io/IShipIO` |
| `Fleet.Drone.Miner/` | PB script project | `Program.cs`, `Mining/`, `Io/` adapters, `Subsystems/` (Sense, Miner, Ui, Remote) |
| `Fleet.Console/` | PB script project | mothership remote console: roster, per-drone menu, commands |
| `Fleet.Tests/` | NUnit, net48 (Mono) | `Engine/`, `Flight/`, `Mining/` fixtures |

Mixins compile into the PB project through `<Import … .projitems>` with a `**/*.cs` wildcard — a new
file under a mixin needs no project edit. `Fleet.sln` holds the three buildable projects only.
Console-only logic that needs tests lives in `Fleet.Engine` (e.g. `Net/Roster.cs`): `Fleet.Tests` can
reference only one PB project, because every PB assembly defines `IngameScript.Program`; trim keeps
unused engine types out of the drone script.

## Architectural Rules & Guardrails
1. **Pure core, thin adapters.** Almost all logic lives in pure classes that take plain values and are
   unit-tested. Only adapters (`Io/`, `Subsystems/`) touch game blocks.
2. **Subsystems** implement `ISubsystem` (`Fleet.Engine/Core/ISubsystem.cs`): `Name`, `StorageVersion`,
   `Update1/10/100`, `HandleMessage`, `Save(MyIni)`, `Load(MyIni, int)`, `Status(StringBuilder)`.
   Dependencies arrive by **constructor injection** (replaces the spec's `Initialize(...)`). Shared
   per-tick state lives on `Blackboard`.
3. **Frequency throttling:** math/physics on `Update10`, network/UI on `Update100`. **Exception:** the
   final ~5 m of docking runs on `Update1`, requested via `PoseTarget.Precision`; `Update1` is a no-op
   otherwise.
4. **Zero-GC in the main loop:** never `new`, concatenate/interpolate strings, `ToString()` numbers or
   enums, or use LINQ inside `Update1/10/100` or `Main()`. Preallocate in `Program()`, `.Clear()` to
   reuse. Numbers → `SbFormat`; enum names → `Names.State[]`/`Names.Reason[]`. Rare user-driven paths
   (commands, config edits, save at FSM transitions, load) may allocate.
5. **Grid scoping:** always filter block queries with `b => b.CubeGrid == Me.CubeGrid`.
6. **Moving docking:** approaches use relative matrix transforms (`Vector3D.TransformNormal`,
   `WorldMatrix`), never static GPS coordinates.

7. **Remote console (spec §10):** the drone broadcasts one status line per `Update100` on
   `FLEET/<Channel>/status` and accepts commands only by **unicast** on `FLEET/<Channel>/cmd` (the same
   text as a PB run argument, plus `SET <section> <key> <value>` and `GETCFG`). Every command source —
   terminal, menu, IGC — goes through `Program.RunCommand`. The drone never depends on the link.
8. **The menu never applies edits.** `Menu.Handle` returns actions; the drone writes them to Custom Data
   (`UiSubsystem.ApplySetting`), the console sends `SET`. Settings are described once, in
   `SettingsSchema`, whose ranges are pinned against `ConfigLoader` by a test.

## API Restrictions (Space Engineers Sandbox)
* **Banned namespaces:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`,
  `System.Globalization`. **No LINQ** in script code.
* **State preservation:** volatile state is serialized to `Storage` in `Save()` and parsed in
  `Program()`; empty/garbled/older-version Storage or Custom Data falls back to defaults with a warning,
  never throws.
* **Namespace:** all script code lives in `namespace IngameScript` inside `public partial class Program`,
  types nested and `public` (MDK strips the namespace on deploy). Don't add other namespaces — the PB
  flattens them and same-named types collide.
* Language version is **C# 6** (`<LangVersion>6</LangVersion>`) — the in-game compiler's ceiling.
  Tests may use C# 7.3.
