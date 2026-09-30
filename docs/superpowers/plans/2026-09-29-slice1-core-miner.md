# Slice 1 — Core Engine + Solo Mining Drone: Implementation Plan

> **For agentic workers:** this plan is executed **card by card**. Each card in
> `docs/superpowers/plans/slice1-cards/` is self-contained and sized for a 32k-context local model
> (qwen3-coder-30b via Continue). Phase 0 and the checkpoints are done by Claude Code. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** a small-grid mining drone script (clean-room PAM successor) that mines planets and asteroids
from a docked home connector, survives reloads, and runs zero-GC — built from a shared engine that later
carrier/mothership scripts reuse.

**Architecture:** MDK2 solution with two shared-code **mixins** (`Fleet.Engine`, `Fleet.Flight`) imported
by a thin **PB project** (`Fleet.Drone.Miner`), plus an NUnit test project (`Fleet.Tests`, net48, runs on
Linux under Mono). Almost all logic lives in **pure classes** that take plain values and are unit-tested;
thin **adapters** are the only code touching game blocks.

**Tech Stack:** C# 6 (in-game compiler ceiling), MDK2 (`Mal.Mdk2.*` 2.2.x), .NET SDK 9 (pinned by
`global.json`), NUnit 4 + Microsoft.NET.Test.Sdk + NUnit3TestAdapter, Mono 6.12 (test host), VRageMath,
`MyIni`.

**Spec:** `docs/superpowers/specs/2026-09-29-slice1-core-miner-design.md`

## Global Constraints

These are copied into `.continue/rules/fleet-mdk.md` (auto-applied by Continue) and summarised in every card.

- All script code: `namespace IngameScript { public partial class Program { ... } }`, types **nested and `public`**.
- **C# 6 only** in script code: no `out var`, tuples, local functions, pattern matching, `is var`, throw
  expressions, discards `_`, `default` literal, digit separators, `in`/`readonly struct`/ref returns.
- **Banned namespaces:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`,
  `System.Globalization`. No LINQ in script code.
- **Zero-GC after construction:** no `new`, no string concatenation/interpolation, no `ToString()` on
  numbers/enums, no LINQ inside `Update1/Update10/Update100/Main` paths. Preallocate in constructors and
  `.Clear()`. Rare user-driven paths (commands, config edits, save at FSM transitions, load) may allocate.
- Every block query filtered with `b.CubeGrid == Me.CubeGrid` (adapters only).
- Tests (`Fleet.Tests`) may use any C# 7.3 feature; they start with `using static IngameScript.Program;`.
- Script size: deployed `script.cs` must stay < 100,000 chars (warn ≥ 90,000) — `tools/check-size.fish`.
- Enum names for display come from `Names.State[]` / `Names.Reason[]`, never `enum.ToString()`.

## Review Focus

Failure modes the spec implies that are easy to miss; each is pinned by a test in the owning card.

1. **Target orientation exactly opposite the current one** (180° turn) — a naive cross-product controller
   outputs zero and never turns. Pinned in **C12** (`AntiParallel_StillTurns`).
2. **Drone connector not facing ship-forward** (belly/back-mounted) — docking must orient the *connector*,
   not the ship nose. Pinned in **C12** (`ReferenceToShip_BellyConnector`) and used by **C18**.
3. **A thrust direction with zero capacity** (atmospheric thrusters in space, no reverse thrusters) — the
   helm must never ask for a speed it cannot brake from. Pinned in **C11** (`ZeroCapacityGroup`) and
   **C18** (`NoBrakingThrust_DoesNotAccelerate`).
4. **Empty, garbled or older-version Storage / Custom Data** (first run, hand edits, script update) — must
   fall back to defaults with a warning, never throw. Pinned in **C04** and **C05**.
5. **Gyro override sign/units differ from the derivation** — cannot be unit-tested; mitigated by the
   `GYROTEST` command (**C25/C30**) and in-game checklist item G1 before any autonomous flight.

---

## How to run a card with Continue

1. `code ~/Documents/Code/space-engineers` (repo root — never the vault).
2. Make sure the coder gear is resident (architect mode / `~/bin/sl-deepwork-terminal.fish`).
3. Open Continue in **Agent** mode, model *Coder (qwen3-coder-30b-A3B)*. Start a **new chat per card**
   (context is 65,536 tokens, but never reuse a chat across cards — a chat that has seen earlier cards
   or an older repo layout will freelance from memory instead of following the card).
4. Type `/card` (`.continue/prompts/card.md`), attach the card and the files it lists under **Attach**,
   and send. Constraints load automatically from `.continue/rules/fleet-mdk.md`.
5. The model finishes by running `python3 tools/card-check.py <Cxx>` and does **not** commit. Only a
   `RESULT: PASS` counts. Read any WARN lines (allocation tokens: fine in constructors/commands/config,
   a defect in `Update*`/`Main` paths). Then review `git diff` and commit with the card's commit command.
6. If the model fails twice on the same step, stop. Bring Claude Code **only** the card id and the
   `card-check` output — not the chat transcript.
7. At a milestone end, run `python3 tools/checkpoint.py <A|B|C|D>` and bring its output with
   "run checkpoint X".

`card-check` FAILs on: failing tests · the card's verbatim test file edited · any file outside the
card's `## Files` · a banned namespace · a token the card's "Done when" forbids. Proven against a
reference C01 and five mutations on 2026-09-30 (see Checkpoint log).

**Useful commands** (fish-safe):
- One card's tests: `dotnet test Fleet.Tests --filter "FullyQualifiedName~<ClassName>Tests" 2>&1 | tail -n 25`
- All tests: `dotnet test Fleet.Tests 2>&1 | tail -n 25`
- Build + deploy + size gate: `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15; fish tools/check-size.fish`

Note: every build of `Fleet.Drone.Miner` (including via `dotnet test`) deploys `script.cs` into the game's
local scripts folder. Harmless — it only replaces the `Fleet.Drone.Miner` workshop-local script.

---

## Phase 0 — Scaffold (Claude Code)

Produces the skeleton every card builds on. Nothing in it needs game knowledge from the local model.

### P0.1 Restructure projects
- `git mv FleetMiner_Core Fleet.Drone.Miner`; rename `FleetMiner_Core.csproj` → `Fleet.Drone.Miner.csproj`;
  delete `bin/` and `obj/` (build output, untracked).
- Create shared projects `Fleet.Engine/Fleet.Engine.{shproj,projitems}` and
  `Fleet.Flight/Fleet.Flight.{shproj,projitems}` (from `dotnet new mdk2mixin`), each `.projitems` with a
  wildcard: `<Compile Include="$(MSBuildThisFileDirectory)**/*.cs" />`.
- In `Fleet.Drone.Miner.csproj` add
  `<Import Project="../Fleet.Engine/Fleet.Engine.projitems" Label="Shared" />` and the same for Flight.
- Create `Fleet.Tests/Fleet.Tests.csproj`: `net48`, `x64`, packages `NUnit 4.3.2`,
  `Microsoft.NET.Test.Sdk 17.12.0`, `NUnit3TestAdapter 4.6.0`, `Mal.Mdk2.References 2.2.9` (PrivateAssets
  all), `ProjectReference ../Fleet.Drone.Miner/Fleet.Drone.Miner.csproj`; its own gitignored
  `Fleet.Tests/mdk.local.ini` with `binarypath=` only (the existing `mdk.local.ini` ignore rule covers it).
- `Fleet.sln` containing the three buildable projects (`dotnet sln add`).
- In `Fleet.Drone.Miner/mdk.ini` set `minify=lite` (strips comments and whitespace, keeps identifiers so
  in-game errors stay readable). Measured during plan validation: C01–C29 alone produce **71,113 chars**
  at `minify=none`, too close to the 90,000 warning line before C30 is added.

### P0.2 Contract files (compile-only, exact content below)

`Fleet.Engine/Core/Enums.cs`
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public enum MinerState : byte { Idle, Undock, DockPathOut, RouteOut, Position, Drill, Retract, RouteBack, DockPathIn, Dock, Unload, Charge, Recording, Hold, Safe }
        public enum ReturnReason : byte { None, CargoFull, LowLift, LowBattery, LowHydrogen, LowUranium, Damage, JobDone, Manual }
        public enum DamagePolicy : byte { Home, Job, Stop }
        public enum ReloadPolicy : byte { Resume, ReturnHome, Hold }
        public enum Cmd : byte { None, Unknown, Start, Stop, Home, Cont, Next, Prev, Full, RecordDock, RecordJob, StopRec, SetJob, Goto, Reboot, Reset, GyroTest, Up, Down, Apply, Back }

        public static class Names
        {
            public static readonly string[] State = { "Idle", "Undock", "DockPathOut", "RouteOut", "Position", "Drill", "Retract", "RouteBack", "DockPathIn", "Dock", "Unload", "Charge", "Recording", "Hold", "Safe" };
            public static readonly string[] Reason = { "", "CargoFull", "LowLift", "LowBattery", "LowHydrogen", "LowUranium", "Damage", "JobDone", "Manual" };
        }
    }
}
```

`Fleet.Engine/Core/ISubsystem.cs`
```csharp
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        public interface ISubsystem
        {
            string Name { get; }            // also its Storage section name
            int StorageVersion { get; }
            void Update1();                 // only called while a subsystem requested Update1
            void Update10();
            void Update100();
            void HandleMessage(MyIGCMessage msg);   // unused in slice 1
            void Save(MyIni ini);                   // write own keys into section Name
            bool Load(MyIni ini, int savedVersion); // false = incompatible -> section dropped and reported
            void Status(StringBuilder sb);          // append LCD status lines
        }
    }
}
```
(Spec §3.2's `Initialize(Blackboard, BlockAdapters)` is replaced by constructor injection — same intent,
simpler to fake.)

`Fleet.Engine/Core/Blackboard.cs`
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class Blackboard
        {
            public double Time;                 // seconds since script start
            public int SensingTicks;            // clean Update10 sensing passes since boot
            public MatrixD ShipMatrix = MatrixD.Identity; // controller orientation, Translation = centre of mass
            public Vector3D Velocity, AngularVelocity, Gravity;
            public bool InGravity;
            public double Mass;
            public double Elevation; public bool HasElevation;
            public double CargoFill;            // 0..1
            public double BatteryFill = 1, HydrogenFill = 1, UraniumKg;
            public bool HasBattery, HasHydrogen, HasReactor;
            public double LiftMargin = double.PositiveInfinity; // upward thrust / weight; +inf in space
            public bool Damaged;
            public bool HasHome; public long HomeConnectorId; public MatrixD HomeMatrix = MatrixD.Identity;
            public bool Connected, Connectable;
            public double ShipSize = 5;         // metres, bounding-box diagonal / 2
        }
    }
}
```

`Fleet.Engine/Core/PoseTarget.cs`
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public struct PoseTarget
        {
            public Vector3D Position;  // world: where the reference point should go
            public Vector3D Forward;   // world unit vector for the reference forward
            public Vector3D Up;        // world unit vector for the reference up
            public double SpeedCap;    // m/s
            public double BrakeDist;   // metres to brake against; < 0 = straight-line distance to Position
            public bool Precision;     // true = request Update1 (final docking)
        }
    }
}
```

`Fleet.Flight/Io/IShipIO.cs`
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public interface IShipIO
        {
            MatrixD WorldMatrix { get; }       // controller orientation, Translation = centre of mass
            Vector3D LinearVelocity { get; }   // world m/s
            Vector3D AngularVelocity { get; }  // world rad/s
            Vector3D Gravity { get; }          // world m/s^2, zero in space
            double Mass { get; }               // kg
            void GetMaxThrust(double[] maxByDir); // [6] by (int)Base6Directions.Direction: N the group can push the ship that way now
            void SetThrust(double[] ratioByDir);  // [6] override fraction 0..1
            void SetGyro(Vector3D worldOmega);    // desired world angular velocity rad/s (enables override)
            void ReleaseControls();               // overrides off, dampeners on
        }
    }
}
```

`Fleet.Engine/Config/Settings.cs`
```csharp
using System.Collections.Generic;

namespace IngameScript
{
    public partial class Program
    {
        public class Settings
        {
            // [Fleet]
            public string Name = "Miner-01";
            public string Tag = "[FM]";
            // [Miner]
            public int Width = 5;
            public int Height = 5;
            public double Depth = 30;
            public double StartDepth = 0;
            public double WorkSpeed = 1.5;
            public double RetractSpeed = 4;
            public double MaxLoad = 90;
            public double MinLiftMargin = 1.3;
            public readonly List<string> Eject = new List<string> { "Stone" };
            public bool Loop = true;
            public DamagePolicy OnDamage = DamagePolicy.Home;
            // [Flight]
            public double MaxSpeed = 60;
            public double ApproachSpeed = 5;
            public double DockSpeed = 0.5;
            public double SafeAltitude = 150;
            public double StuckSeconds = 5;
            // [Energy]
            public double MinBattery = 20;
            public double MinHydrogen = 30;
            public double MinUranium = 5;   // kg
            // [Reload]
            public ReloadPolicy OnReload = ReloadPolicy.Resume;
        }
    }
}
```

`Fleet.Tests/Engine/SettingsDefaultsTests.cs` — a smoke test asserting every default above equals the spec
value (so the test project has at least one test and the harness is proven in-repo).

`Fleet.Drone.Miner/Program.cs` — reduced to an empty `Program()`, `Save()`, `Main()` (C30 fills it).

### P0.3 Tooling and docs
- `tools/check-size.fish`: reads `output=` from `Fleet.Drone.Miner/mdk.local.ini`, counts characters
  (`wc -m`) of `<output>/Fleet.Drone.Miner/script.cs`; prints the count; exit 1 at ≥ 100,000, prints a
  warning at ≥ 90,000.
- `.continue/rules/fleet-mdk.md`: the Global Constraints above, with `alwaysApply: true` frontmatter.
- Update repo `CLAUDE.md`: new structure, test/build commands, extended `ISubsystem`, the `Update1` docking
  exception, the card workflow.

### P0.4 Verify and commit
- [ ] `dotnet build Fleet.Drone.Miner -c Release` → "successfully deployed"; `fish tools/check-size.fish` → OK.
- [ ] `dotnet test Fleet.Tests` → `Passed!` (SettingsDefaultsTests).
- [ ] Commit: `chore: restructure into Fleet.* mixins, test harness, contracts (phase 0)`.

---

## Card index

Execute in order within a milestone; a card may only depend on cards above it. "Executor" is a
recommendation: **local** = well-specified pure logic with full tests; **local+review** = feasible but
Claude reviews closely; **Claude** = game-API/integration work where a local model is likely to flounder.

| Card | File(s) | Depends on | Executor |
|---|---|---|---|
| **Milestone A — Engine** | | | |
| C01 | `Fleet.Engine/Util/SbFormat.cs` | — | local |
| C02 | `Fleet.Engine/Core/Profiler.cs` | — | local |
| C03 | `Fleet.Engine/Core/EventLog.cs` | C01 | local |
| C04 | `Fleet.Engine/Config/ConfigLoader.cs` | — | local |
| C05 | `Fleet.Engine/Core/StorageStore.cs` | — | local |
| C06 | `Fleet.Engine/Core/Kernel.cs` | C02 | local |
| C07 | `Fleet.Engine/Nav/Frames.cs` | — | local |
| C08 | `Fleet.Engine/Nav/Pid.cs` | — | local |
| C09 | `Fleet.Engine/Core/CommandParser.cs` | — | local |
| **Checkpoint A** (Claude) | | | |
| **Milestone B — Flight** | | | |
| C10 | `Fleet.Flight/Flight/BrakingCurve.cs` | — | local |
| C11 | `Fleet.Flight/Flight/ThrustAllocator.cs` | — | local |
| C12 | `Fleet.Flight/Flight/GyroMath.cs` | — | local+review |
| C13 | `Fleet.Flight/Flight/StuckDetector.cs` | — | local |
| C14 | `Fleet.Flight/Flight/PathRecorder.cs`, `PathCodec.cs` | — | local |
| C15 | `Fleet.Flight/Flight/PathFollower.cs` | — | local |
| C16 | `Fleet.Flight/Flight/GpsRoute.cs` | — | local |
| C17 | `Fleet.Flight/Flight/DockingPlanner.cs` | — | local |
| C18 | `Fleet.Flight/Flight/Helm.cs` | C08, C10, C11, C12 | local+review |
| **Checkpoint B** (Claude) | | | |
| **Milestone C — Mining** | | | |
| C19 | `Fleet.Drone.Miner/Mining/HoleGrid.cs` | — | local |
| C20 | `Fleet.Drone.Miner/Mining/DrillLogic.cs` | — | local |
| C21 | `Fleet.Drone.Miner/Mining/ReturnTriggers.cs` | — | local |
| C22 | `Fleet.Drone.Miner/Mining/Reconciler.cs` | — | local |
| C23 | `Fleet.Drone.Miner/Mining/MinerFsm.cs` | — | local+review |
| C24 | `Fleet.Drone.Miner/Ui/MenuModel.cs` | C01 | local |
| **Checkpoint C** (Claude) | | | |
| **Milestone D — Game adapters + integration** | | | |
| C25 | `Fleet.Flight/Io/ShipIO.cs` | C12 | Claude |
| C26 | `Fleet.Drone.Miner/Io/CargoIO.cs` | — | local+review |
| C27 | `Fleet.Drone.Miner/Io/EnergyIO.cs` | — | local+review |
| C28 | `Fleet.Drone.Miner/Io/DamageScanner.cs` | — | local+review |
| C29 | `Fleet.Engine/Ui/Displays.cs` | — | local+review |
| C30 | `Fleet.Drone.Miner/Subsystems/*.cs`, `Program.cs` | all | Claude |
| **Checkpoint D** (Claude) → **In-game checklist** | | | |

## Checkpoints (Claude Code)

Bring the branch back to Claude Code with "run checkpoint X". Each checkpoint:
1. Runs the full test suite and the Release build + size gate, and shows the output.
2. Reviews every card's diff against its card and the spec (C# 6 compliance, zero-GC in hot paths,
   interface names exactly as specified).
3. Fixes small defects directly, or re-issues a corrected card for the local model.
4. Records the result in this file under "Checkpoint log".

Checkpoint D additionally measures instruction usage in a stationary in-game test and runs the in-game
checklist below with Blae.

## In-game checklist (spec §8.3)

Test world: one planet, one asteroid, a large-grid stand-in carrier with a connector, and the drone
docked to it. Each item passes only when observed in-game.

- [ ] **G1 GYROTEST** — drone hovering: `GYROTEST` reports sign and scale per axis; fix
  `GyroMath` sign constants / `ShipIO.GyroScale` if reported. **Must pass before any autonomous flight.**
- [ ] G2 Setup diagnostics — remove the gyros: the LCD names the missing block; restore: clears.
- [ ] G3 `RECORD DOCK` out of the bay, `STOPREC`; `RECORD JOB` to an asteroid, `STOPREC`; `SETJOB`.
- [ ] G4 `START` → 3×3 asteroid job: undock, dock path, route, drill, fill, return, dock, unload, relaunch.
- [ ] G5 `GOTO <GPS>` job in space.
- [ ] G6 `GOTO <GPS>` job on the planet (climb, cruise at SafeAltitude, descend, drill down).
- [ ] G7 Planet overload: lift-margin return triggers before the drone can't climb.
- [ ] G8 Reload mid-transit, mid-drill, mid-dock: drone holds, reconciles, resumes.
- [ ] G9 Damage a drill: `OnDamage=Home` returns home.
- [ ] G10 Drain battery below MinBattery: returns and recharges.
- [ ] G11 Instruction average and peak shown; peak < 30 % of the limit during flight.
- [ ] G12 `tools/check-size.fish` < 90,000 chars.

## Self-review record
- Spec coverage: §2 → P0.1; §3.1 → C06/C30; §3.2 → P0.2; §3.3 → C04/C30; §3.4 → C06/C30; §3.5 → C01/C02/
  rules; §4 → C05/C22/C30; §5.1 → C07; §5.2 → C08/C10/C11/C12/C18/C25; §5.3 → C14/C15/C16; §5.4 → C17;
  §5.5 → C13/C18; §6.1 → C19; §6.2 → C20; §6.3 → C21/C27/C28; §6.4 → C26; §6.5 → C26/C27; §6.6 → C23/C03;
  §7.1 → C04; §7.2 → C25–C30; §7.3 → C09; §7.4 → C24/C29; §8 → P0/checkpoints/checklist.
- Gaps found and fixed in self-review: §5.5 "back off, retry twice" was missing (FSM held on first stuck) →
  added to C30; ejection limited to Drill → Drill + Retract; Unload's carrier-inventory source unstated → C30.
- Card tests validated by an independent reference implementation of C01–C29 (see Checkpoint log).
- Additions beyond the spec, flagged for review: `GYROTEST` command (Review Focus 5); `Cmd`/`Names` tables
  (zero-GC display); constructor injection instead of `Initialize(...)`.

## Checkpoint log
- **2026-09-29 — plan validation.** An independent agent implemented C01–C29 from the card text alone (C30
  excluded) in a scratch copy: **266/266 tests passed, no test edited**; Release build with MDK analyzers
  0 warnings / 0 errors (analyzer enforcement proven with a banned-namespace probe); every game API named
  by the adapter cards compiled against the real `Bin64`. Twelve ambiguities it reported were folded into
  C01, C04, C06, C12, C16, C23, C25, C26, C28, C29 (one new test case in C01); size finding → `minify=lite`
  in P0.1.
- **2026-09-30 — Phase 0 done (Claude).** P0.1–P0.4 as written, plus: `SpaceEngineersBinCopyLocal` in
  `Fleet.Tests.csproj` (from MDK's own tests template — game DLLs at test runtime); unique GUIDs per mixin
  (`mdk2mixin` stamps a fixed one). Release build 0 warnings; `SettingsDefaultsTests` 2/2 on Mono; mixin
  inclusion proven by a probe type reaching `script.cs` (without a reference, `minify=lite`'s trim drops
  unused types — expected). New gates `tools/card-check.py`, `tools/checkpoint.py`, `/card` prompt.
  card-check proven on a reference C01 (26/26, PASS) and five mutations, each FAIL on the right row:
  `ToString(` in impl · test edited to agree with a broken impl · broken impl · `System.Linq` · extra file.
  **Incident:** a pre-Phase-0 Continue chat freelanced HoleGrid/DrillLogic/ReturnTriggers/Reconciler into
  the old `FleetMiner_Core/Mining/` (no tests, LINQ); archived out of the repo. Hence rule 3's wording.
