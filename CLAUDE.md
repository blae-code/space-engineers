# Space Engineers MDK Project: Floating Rig, RIG//OS and drone fleet

**`CLAUDE(RIG).md` governs: project scope, constraints C1–C7, the Rig design and the merge workflow
(§8).** This file keeps the engineering workflow: the card split with the local model, build/test/deploy,
MDK landmines and the PB sandbox rules. Where they conflict, `CLAUDE(RIG).md` wins; flag the conflict.

The original root-level `FleetMiner_Core` MDK project is retained alongside the refactored repository.
Its project-specific guidance is in `CLAUDE-MDK.md`.

- Canonical design: `reference/` (build guide, Crawler/Mule guides, RIG//OS prototypes, sandbox).
- Spec changes are logged in `docs/rig/CHANGELOG.md` (`| date | change |`). Mods: `docs/rig/mods.md`.
- The Fleet code (`Fleet.*`, PAM-successor miner, console, carrier, dispatcher) is frozen at tag
  `fleet-freeze` and is being merged into the Rig (`docs/merge/`). Its slice specs/plans/cards stay in
  `docs/superpowers/` until they move to `legacy/drone-fleet/`.
- In-game build specs: `docs/build-specs/`. `mining-drone.script.cs` is a **snapshot** of the deployed
  `script.cs`: after any drone build that changes the script, run `fish tools/snapshot.fish`
  (`checkpoint.py` FAILs while it is stale).

**This local repo is authoritative.** `origin` is `github.com/blae-code/space-engineers`; push only
when a milestone is called, never on routine commits.

## Division of labour (Gemini ↔ Claude Code ↔ local model)
Work is split to save Claude credits. **Gemini drafts, Claude reviews and gates, the local model
executes cards.**
- **Gemini** (Antigravity extension, 2026-10-17) does the token-heavy work first: spec and plan
  drafts, card decks, docs, research, bulk reading and bulk code drafts. Its rules: `GEMINI.md`
  (+ `.agent/rules/se-project.md` pointer). It commits through the same hooks with an `Agent: Gemini`
  trailer and adds a `pending` row to `docs/review-queue.md`.
- **Claude's default is review, not authorship.** At session start read `docs/review-queue.md`; for
  each pending row run the gate (`card-check`, `checkpoint.py`, `dotnet test`, `check-size.fish`) and
  read its output. Read the diff only for docs/specs (no gate) or a WARN/FAIL. Set the status:
  `reviewed` · `fixed` (≤ ≈10 lines, Claude commit) · `bounced` (reason + failing test for code).
  A token-heavy task that comes up mid-session goes to the queue or `docs/ideas.md` as a brief for
  Gemini instead of being done here.
- **A Gemini card deck is not executed until its row reads `reviewed`**, so one agent never writes
  both the tests and the code. `tools/hooks/commit-msg` refuses an `Agent: Gemini`/`Agent: Qwen`
  commit touching `tools/`, `.continue/`, `.agent/`, `CLAUDE*.md` or `GEMINI.md`. It keys off the
  trailer, so it is a guardrail; the review is the boundary.
- **Bulk read work goes to the local model directly** (2026-10-16): when `lms ps` shows
  `qwen/qwen3-coder-30b` resident, Claude may call it on `127.0.0.1:1234` for per-file extraction or
  summaries (scripts in the session scratchpad), then spot-check the output against the source.
  Never name a non-resident model (that JIT-loads it and evicts the resident gear).
The **local model** (qwen3-coder-30b-a3b in Continue, Agent mode, 64k ctx) executes task cards and
answers read-only codebase questions (`/ask`). **Claude Code** owns the final say on specs, plans
and cards (interface + tests/case tables; Gemini drafts them, Claude reviews), game-API adapters, the
gates in `tools/`, checkpoints, escalations and in-game debugging. Evidence for the split: carded work went 28/28 (C01–C30). Uncarded work on 2026-10-02 broke
zero-GC in a render path and produced speculative roadmaps, so it was reverted.
- **Cards** live in any `docs/superpowers/plans/*-cards/` deck, numbered across decks (C31 onward
  after slice 1). Each card has `Kind:` on its header line:
  `logic` = new file to an interface, verbatim tests ·
  `wire` = edits existing files at named anchors, verbatim test (usually on the real `Program`) ·
  `cases` = Claude gives a `| K1 | … |` table, the local model writes the tests ·
  `doc` = docs from bullets. `## Hot paths` lists per-tick methods, and any allocation token inside
  them FAILs. Aim for ≥75 % of a deck to be local cards. Prefer `cases` and `wire` over Claude writing
  the code: a table or an anchor costs fewer tokens than code.
- **Proving a card costs tokens, so keep it proportional.** Cards are proven by their tests, and a
  `cases` table needs only its expected column checked. Never write a full reference implementation
  to prove a card. If the code had to be written to know the tests are right, commit that code
  (Claude commit, `--no-verify`, test output quoted) and do not card it. (2026-10-02: the slice-3
  pure layer went this way, and every test expectation held on the first run.) Wire cards are
  written only when their gate opens, so their anchors are current.
- A card is gated by `python3 tools/card-check.py <Cxx>` (tests, verbatim-test tamper check, case ids,
  scope, banned namespaces, hot paths, the card's own "Done when" tokens). Blae reviews `git diff` and
  commits. **Claude reads gate output, not diffs, unless a gate says WARN or FAIL.**
- **The gate is enforced by git, not by the model's report:** `git config core.hooksPath tools/hooks`
  (repo-local, already set here). The pre-commit hook refuses script code that no card covers, banned
  namespaces anywhere, multi-card commits, and any card commit whose `card-check --staged` is not
  PASS. The commit-msg hook stamps a `Card: Cxx` trailer, which `--committed` uses to find `wire`
  cards. `python3 tools/next-card.py` prints every card's status and the next one to hand out.
  `--no-verify` is for Claude's own commits only, with the card-check/test output quoted in the
  commit message.
- A checkpoint starts from `python3 tools/checkpoint.py <A|B|C|D|Cnn-Cmm>` output — do not re-run what
  it ran. It also FAILs a stale `docs/build-specs/mining-drone.script.cs` (fix: `fish tools/snapshot.fish`).
- **Escalation contract:** Blae brings only the card id, the card-check output and, for FAILs, the
  failing test names. For a fix of ~10 lines or fewer, Claude fixes it. Above that, Claude re-issues
  the card with a failing test and a one-line hint rather than writing the fix. Record the outcome in
  the plan's "Checkpoint log".
- Ideas from anyone go to `docs/ideas.md`, one line each, until Claude turns them into a spec or card.
- Continue config in-repo: `.continue/rules/fleet-mdk.md` (auto-applied constraints),
  `.continue/prompts/card.md` (`/card`), `.continue/prompts/ask.md` (`/ask`). Keep them in sync with
  the rules below.

## Build, test & deploy (Linux / Proton — MDK2)
The dev box is CachyOS Linux; the game runs under Proton via **native Steam** (`~/.local/share/Steam`; Flatpak Steam is gone, 2026-10-16). There is no Visual
Studio — MDK2 builds and deploys through the .NET SDK; tests run on Mono.

```
dotnet build Fleet.Drone.Miner -c Release   # compile vs Bin64, minify, DEPLOY to the game
dotnet build Fleet.Console -c Release       # the mothership console script
dotnet build Fleet.Carrier -c Release       # the carrier bay beacon (slice 2)
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
| `Fleet.Carrier/` | PB script project | carrier bay beacon: tagged connectors' pose + velocity every Update10 |
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
6. **Frames (slice 2):** the **dock path** is stored local to the home connector (it belongs to the
   carrier); the **job and job route** are stored in **world** space (asteroids don't move, carriers
   do). Undocked, the home pose comes from the carrier's beacon (`HomeTracker`, extrapolated); never
   heard a beacon = static carrier (slice 1); heard then silent = home LOST → homeward states Hold.
   Home-relative targets carry `PoseTarget.Velocity` = the carrier's point velocity; job targets carry
   zero. Never fly at a guessed connector.

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
