# Fleet — Space Engineers Drone Autonomy Scripts

Programmable Block scripts for Space Engineers that automate a mining drone swarm,
carrier bay beacon and mothership console. Built with [MDK2](https://github.com/malware-dev/MDK-SE)
on Linux/Proton.

## Projects

| Project | Kind | What it does |
|---|---|---|
| **Fleet.Engine** | MDK2 mixin (shared) | Kernel, config, storage, event log, commands, PID, UI, IGC networking, carrier dispatch |
| **Fleet.Flight** | MDK2 mixin (shared) | Pure flight math — braking curves, thrust allocation, gyro control, path following, docking, helm |
| **Fleet.Drone.Miner** | PB script | Autonomous mining drone — FSM, drill logic, hole grid, cargo/energy/damage IO, remote console |
| **Fleet.Console** | PB script | Mothership remote console — fleet roster, per-drone menu, SET/GETCFG commands |
| **Fleet.Carrier** | PB script | Carrier bay beacon — broadcasts connector poses & velocity for docking onto moving carriers |
| **Fleet.Tests** | NUnit (net48/Mono) | Unit tests for Engine, Flight and Mining layers |

Mixins compile into PB projects via `<Import … .projitems>` — adding a `.cs` file under a
mixin needs no project edit.

## Prerequisites

- **.NET SDK 9** — pinned by `global.json` (the MDK2 packager rejects SDK 10)
- **Space Engineers** running under Proton on Linux
- **Mono** (for running NUnit tests on net48)

## Build & Test

```bash
# Compile + deploy a PB script into the Proton prefix
dotnet build Fleet.Drone.Miner -c Release
dotnet build Fleet.Console     -c Release
dotnet build Fleet.Carrier     -c Release

# Size gate — every deployed script.cs must be < 100,000 chars
fish tools/check-size.fish

# Run the full test suite
dotnet test Fleet.Tests

# Run a single test class
dotnet test Fleet.Tests --filter "FullyQualifiedName~SbFormatTests"
```

A successful Release build deploys the minified `script.cs` into the Proton prefix.
In-game: PB → Edit → Browse Scripts → select the project name.

## Key Design Decisions

- **Pure core, thin adapters** — almost all logic is in pure, unit-testable classes;
  only `Io/` and `Subsystems/` touch game blocks.
- **Zero-GC in the main loop** — no allocations in `Update1/10/100` or `Main()`.
  Numbers go through `SbFormat`, enum names through literal tables.
- **`minify=full`** — the deployed script is fully minified (trim + rename).
  Never depend on identifier names at runtime.
- **C# 6** — the in-game compiler's ceiling. Tests may use C# 7.3.

## Repository Layout

```
CLAUDE.md              # Engineering workflow (build, test, card system, rules)
CLAUDE(RIG).md         # Project scope, design spec, merge workflow
reference/             # Canonical design docs (build guides, RIG//OS prototypes, sandbox)
docs/
  build-specs/         # Deployed script snapshot (enforced by checkpoint.py)
  rig/                 # Rig changelog, mod list, drone analysis
  merge/               # Merge tracking (fleet work → Rig integration)
  superpowers/         # Slice specs, plans and task cards
  ideas.md             # Idea backlog
tools/
  card-check.py        # Task card gate (tests, scope, banned namespaces)
  checkpoint.py        # Full milestone checkpoint (build + test + cards + snapshot)
  bp-inspect.py        # Blueprint inspection utility
  hooks/               # Git hooks enforcing the card system
```

## Workflow

Work is split between Claude Code (specs, plans, cards, checkpoints) and a local model
(code execution from task cards). See [`CLAUDE.md`](CLAUDE.md) for the full workflow,
and [`CLAUDE(RIG).md`](CLAUDE(RIG).md) for the project scope and design spec.

