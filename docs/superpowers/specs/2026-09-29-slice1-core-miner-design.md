# Slice 1 — Core Engine + Solo Mining Drone (design)

**Date:** 2026-09-29 · **Status:** approved in conversation, awaiting spec review · **Owner:** Blae

## 1. Context and intent

This project is a **clean-room successor to PAM** ([PAM] Path Auto Miner by Keks, workshop
1507646929, v1.3.1, 2019-12-20). PAM is published only in minified form and carries no license, so it
is used strictly as a **behavioural reference**: no PAM code is copied, translated or de-obfuscated.
Credit: "inspired by Keks' PAM".

The successor must reach PAM parity for mining and then exceed it across fleet orchestration, flight,
ore-aware mining, operator experience and server-grade efficiency. That scope is decomposed into six
slices, each with its own spec → plan → build cycle and an in-game-testable result:

| # | Slice | Summary |
|---|-------|---------|
| **1** | **Core engine + solo drone** | **this document** |
| 2 | Moving-carrier docking + carrier PB | relative approach onto a moving large-grid connector |
| 3 | Fleet orchestration + mothership PB | IGC protocol, job/hole allocation, connector queuing |
| 4 | Smarter mining | weight holes by where ore was found |
| 5 | Obstacle-aware flight | camera raycast / sensor detours |
| 6 | Operator UX polish | sprite dashboards, progress maps |

### Fleet model (from Blae)
- **Mothership** (large grid) launches **logistics carriers** (large grid), which house and deploy
  **mining drones** (small grid).
- Mining drones **only mine**. Salvage and transport are separate future drone types, so PAM's
  Grinder and Shuttle modes are out of scope for every slice.
- Environments: **planets and asteroids**.
- A future **semi-automated probe** will hand drones GPS targets. Vanilla constraint recorded for that
  design: a programmable block cannot read ore-detector results.

### Slice 1 success criteria
A single small-grid drone, docked to a stationary large-grid stand-in carrier, with near-zero
configuration:
1. Mines a hole grid on a planet **and** on an asteroid, reached by a recorded route **or** a GPS target.
2. Returns on full cargo, low lift margin, low energy, damage, or job completion; docks, unloads,
   recharges and relaunches on its own until the job is done.
3. Survives a world reload at any point (mid-transit, mid-drill, mid-dock) without flying off,
   falling, or losing the job.
4. Runs within a low, continuously displayed instruction budget with no allocations in the main loop.
5. Stays under the 100,000-character programmable-block script limit.

## 2. Project structure

MDK2 solution at the repository root, replacing the single `FleetMiner_Core` project:

```
Fleet.Engine/        mixin   kernel, ISubsystem, Blackboard, config, Storage, frames, PID, LCD, commands
Fleet.Flight/        mixin   helm, thrust allocation, gyro control, path record/replay, GPS routes, docking
Fleet.Drone.Miner/   PB      miner job, FSM, cargo, ejection, energy/damage triggers   (slice 1)
Fleet.Carrier/       PB      slice 2 (not created in slice 1)
Fleet.Mothership/    PB      slice 3 (not created in slice 1)
Fleet.Tests/         tests   mdk2pbtests project, run with `dotnet test`
```

- The existing `FleetMiner_Core` project is renamed to `Fleet.Drone.Miner`; its deploy folder name
  changes accordingly.
- Each PB project includes only the mixins it needs, so every role script stays below the 100k-character
  cap. This is the reason for the split: one all-roles script would not fit.
- All code stays in `namespace IngameScript`, inside `partial class Program`, C# 6 (per repo `CLAUDE.md`).
- The repo `CLAUDE.md` is updated during implementation to describe this structure, the extended
  `ISubsystem` contract and the `Update1` docking exception.

## 3. Engine kernel (`Fleet.Engine`)

### 3.1 Kernel loop
- `Program()` runs the **reload hold** first (§4), then constructs subsystems in a fixed order, parses
  Custom Data, restores `Storage` and sets `UpdateFrequency = Update10 | Update100`.
- `Main()` routes `Update10` / `Update100` / `Update1` ticks to every subsystem, and run arguments to
  the command router.
- Subsystems communicate only through a shared **`Blackboard`**: pose, velocity, gravity, mass,
  cargo fill, energy levels, lift margin and job state. Sensing subsystems write it; deciding subsystems
  read it. No subsystem calls another directly.

### 3.2 `ISubsystem` contract
```
void Initialize(Blackboard bb, BlockAdapters blocks)
void Update10();  void Update100();  void Update1();   // Update1 is a no-op except in the final dock phase
void HandleMessage(MyIGCMessage msg)                    // unused in slice 1, reserved for slice 3
void Save(MyIni storage);  void Load(MyIni storage)     // own section, own version key
void Status(StringBuilder sb)                           // appends LCD status lines
```
`Save`/`Load`/`Status` extend the contract documented in the repo `CLAUDE.md`.

### 3.3 Configuration
- Custom Data is parsed with `MyIni`, one section per concern (§7.1).
- Re-parsed only when the text changes (compared against a cached hash), never on every tick.
- Missing keys are written back with their defaults on first run.
- Invalid values fall back to defaults and raise a visible warning. They never halt the script.

### 3.4 Fail-safe
`Main()` is wrapped in a single `try/catch`. Any exception enters **SAFE**: release all thrust and gyro
overrides, dampeners on, drills off, show the exception and the throwing subsystem on the LCD. The FSM
stays halted until `RESET`.

### 3.5 Efficiency
- **Zero-GC rule:** no `new`, no LINQ, no string concatenation after `Program()`. Collections are
  preallocated and `.Clear()`ed; status text goes into reused `StringBuilder`s. User-driven, rare paths
  (config edits, menu writes, FSM-transition saves) may allocate.
- The kernel measures instructions per tick (current, average, peak against the limit) and last run
  time per subsystem. This is always visible on the status screen.
- Grid scoping: every block query is filtered with `b.CubeGrid == Me.CubeGrid`.

## 4. Reload and restart resilience

A world reload re-runs the constructor, restores `Storage`/Custom Data from the last save and
invalidates all block references. Thrust and gyro overrides persist in the saved blocks, and physics
resumes before the script is ready.

1. **Hold at the first instruction:** release all thrust and gyro overrides, dampeners on, drills off.
   The drone hovers or drifts to a stop wherever it is.
2. **Reconcile** the saved FSM state against the observed world. Examples:

   | Saved state | Observed | Result |
   |---|---|---|
   | Dock | connector locked | Docked → Unload |
   | Drill (hole n, depth d) | > 2× ship size from hole entrance | fly back, restart hole n from entrance |
   | RouteBack | cargo empty | the save was taken after unload → Idle/Charge |
   | any | home connector not found | **Hold** + warning |

   Anything it cannot reconcile ends in **Hold** with the reason shown, never in movement.
3. **Resume by policy** `OnReload = Resume | ReturnHome | Hold` (default `Resume`), only after at least
   two clean sensing ticks.

Supporting mechanisms:
- All positional data is local to the **home connector frame**. The connector is found again by entity
  id, with a tag fallback.
- **Versioned `Storage`:** each subsystem section carries a version. Compatible data (home, paths, job
  progress) is migrated on script updates. Only genuinely conflicting data is dropped, and it is
  reported by name.
- `Storage` is written at **every FSM transition** as well as in `Save()`.
- `REBOOT` runs hold → reconcile → resume on demand.
- Grid-concealment mods on servers freeze distant grids. This behaves like a reload and is handled the
  same way.

## 5. Flight and navigation (`Fleet.Flight`)

### 5.1 Frames
Stored vectors are local to the home connector:
`local = Vector3D.TransformNormal(world − origin, MatrixD.Transpose(connector.WorldMatrix))`. Live
world targets are recomputed each tick from the connector's current `WorldMatrix`. This keeps paths
valid across reloads and makes slice 2's moving carrier an extension, not a rewrite.

### 5.2 Helm
Input: a target pose (position, forward, up) for a selectable **reference point** (drone connector when
docking, drill face when mining) and a speed cap.
- **Translation:** braking-curve velocity target `v = min(cap, √(2·a_brake·dist)·margin)`, PI on
  velocity error → required acceleration, plus gravity compensation → force = mass × acceleration.
  The force is allocated across the six ship-local thrust directions using each thruster's **current
  effective** thrust (atmospheric, ion and hydrogen handled uniformly). `a_brake` uses the thrust
  actually available in the braking direction, with gravity included.
- **Rotation:** PD alignment of forward and up (up = anti-gravity in gravity, recorded up in space),
  transformed into each gyro's own frame.
- **Rate:** `Update10`, except the final ~5 m of docking, which runs on `Update1` (budgeted, and a
  documented exception to the repo rule).

### 5.3 Routes
Every trip is **dock path + job route**:
- **Dock path:** hangar ↔ open space, recorded once per home connector, shared by all jobs.
- **Job route**, one of:
  - *Recorded:* points sampled every ~5 m (spacing grows with speed), flown with look-ahead
    corner-cutting and speed reduced before sharp turns.
  - *GPS:* in space, a straight line. In gravity: climb along anti-gravity to `SafeAltitude` above the
    surface (`TryGetPlanetElevation`), cruise holding that altitude, then descend onto the target.
- The target abstraction is source-agnostic, so later probes and the mothership can supply GPS targets
  over IGC.
- Until slice 5 there is no obstacle sensing. GPS routes are only as safe as the space they cross.

### 5.4 Docking
Fly to a point in front of the home connector, align connector to connector, creep in at `DockSpeed`
while slowing further near contact, and `Connect()` when the status is `Connectable`. Undocking:
disconnect, back out along the connector axis, then take the dock path.

### 5.5 Failure handling
- **Stuck:** thrust applied but speed toward the target < 0.2 m/s for `StuckSeconds` (default 5) → back
  off, retry twice, then Hold.
- **Too heavy** is reported as a lift-margin signal to the job (§6.3).

## 6. Miner job (`Fleet.Drone.Miner`)

### 6.1 Job definition
Stored in the connector frame: job origin, mining forward and up, grid `Width × Height`, `Depth`,
`StartDepth`. **Hole spacing is derived from the drill layout's bounding area.** Holes are ordered in
a **centre-out spiral**.

### 6.2 Mining a hole
Align at the entrance → drills on → advance at `WorkSpeed`. **Stall** = forward speed below 10 % of `WorkSpeed`
for 3 s while thrusting: halve the feed speed. If it is still stalled after another 5 s, retract and mark the hole **blocked**. At `Depth`, retract at `RetractSpeed` → next
hole. Progress = hole index and depth.

### 6.3 Return triggers (`Update100`)
- Cargo fill ≥ `MaxLoad` %.
- **Lift margin** (upward effective thrust ÷ weight) < `MinLiftMargin` (default 1.3). This replaces
  PAM's "too heavy" error by leaving while the drone can still climb out.
- Battery / hydrogen / uranium below `MinBattery` / `MinHydrogen` / `MinUranium`.
- Damage: block count or total integrity below the baseline taken at `SETJOB`; response per
  `OnDamage = Home | Job | Stop`.
- Job complete.

### 6.4 Ejection
Ores on the `Eject` list (default `Stone`) are moved into tagged ejector connectors set to throw out,
**only inside the job area**. (PAM's stone/ice ejection and the ore whitelist both live here. Slice 4
keeps ore-aware hole weighting.)

### 6.5 At home
1. **Unload:** the drone transfers its ore into the carrier's containers itself (tagged container
   preferred). It does not depend on the carrier's sorters.
2. **Recharge/refuel:** batteries `Recharge` and tanks `Stockpile` until thresholds are met.
3. **Relaunch** automatically unless `Loop=false` or the job is complete.

### 6.6 State machine
```
Idle → Undock → DockPathOut → RouteOut → Position → Drill → Retract
     → (Position [next hole] | RouteBack) → DockPathIn → Dock → Unload → Charge → (Undock | Idle)
Recording, Hold, Safe: enterable from any state
```
Each transition saves `Storage` and appends to an on-LCD event history
(e.g. `14:02 Drill→Retract: hole 7 blocked`).

## 7. Operator interface

### 7.1 Custom Data (configuration source of truth)
```ini
[Fleet]   Name=Miner-01  Tag=[FM]
[Miner]   Width=5  Height=5  Depth=30  StartDepth=0  WorkSpeed=1.5  RetractSpeed=4
          MaxLoad=90  MinLiftMargin=1.3  Eject=Stone  Loop=true  OnDamage=Home
[Flight]  MaxSpeed=60  ApproachSpeed=5  DockSpeed=0.5  SafeAltitude=150  StuckSeconds=5
[Energy]  MinBattery=20  MinHydrogen=30  MinUranium=5
[Reload]  OnReload=Resume
```
(Shown compactly; the real file uses one key per line.) Runtime data (paths, job geometry, progress)
lives in `Storage`, not Custom Data.

### 7.2 Setup
The script auto-detects the ship controller, connector, thrusters, gyros, drills, batteries and tanks,
plus optional ejectors and LCDs, on the drone's own grid. Problems are reported by name with the fix.
The home connector is whatever the drone is locked to when recording or setting home.

### 7.3 Commands
`RECORD DOCK` · `RECORD JOB` · `STOPREC` · `SETJOB` · `GOTO <GPS>` · `START` · `HOME` · `CONT` ·
`STOP` · `NEXT` · `PREV` · `FULL` · `REBOOT` · `RESET` · menu: `UP` `DOWN` `APPLY` `BACK`.

### 7.4 LCD menu
Three pages: **Status** (state, hole x/y and depth, cargo, lift margin, energy, instruction avg/peak,
recent events), **Job** (edit grid, depth, speeds), **Commands**. Edits are written back to Custom Data.
Displays are chosen by tag: `[FM]` for an LCD, `[FM:n]` for cockpit surface *n*. Text only in slice 1.

## 8. Testing

1. **Unit tests** (`Fleet.Tests`, `dotnet test`): frame round-trips, braking curve, PID, thrust
   allocation, spiral order, hole spacing, config parsing and defaults, `Storage` migration, reload
   reconciliation (saved + observed → expected), FSM transitions. Logic reaches blocks only through
   thin adapters that tests replace with fakes.
   **Risk, resolved in plan task 1:** it is unproven that the MDK2 test template loads the game's
   .NET Framework 4.8 `VRageMath`/`MyIni` on Linux. Fallback: pure math on project-local structs,
   with game types confined to the adapters.
2. **Build gate:** `dotnet build -c Release` is clean with MDK analyzers, plus a script-size check that
   warns at 90,000 characters and fails at 100,000.
3. **In-game checklist**, all of which must pass before the slice is done. Test world: a planet, an
   asteroid, and a drone docked to a large-grid stand-in carrier.
   - Record dock path + job route → mine 3×3 → fill → return → unload → relaunch.
   - GPS job in space; GPS job on a planet.
   - Reload mid-drill, mid-transit, mid-dock.
   - Damage a drill; drain the battery; overload in gravity (lift-margin return).

## 9. Out of scope for slice 1
Moving-carrier docking, the carrier script, fleet orchestration (job/hole allocation, connector
queuing, multi-drone coordination), ore-aware hole weighting, obstacle avoidance, sprite UI,
Grinder/Shuttle modes, probes. (The remote console and its IGC link were pulled into slice 1 — §10.)

## 10. Addendum 2026-09-30 — remote console (pulled forward from slice 3)

**Decision (Blae):** drones are set up and commanded remotely from the mothership/carrier. Chosen
options: a remote console **now**; drones are **commanded, not piloted** (the console says where to go
and what to do, the drone flies itself; manual stick flight stays the vanilla Remote Control block);
the drone **keeps its own menu as a fallback**. Slice 3 keeps job allocation, queuing and coordination.

### 10.1 Pieces
| Piece | Where | What |
|---|---|---|
| `Menu` + `SettingsSchema` | `Fleet.Engine/Ui/` | one menu engine for both PBs (replaces C24's `MenuModel`) |
| `FleetLink` | `Fleet.Engine/Net/` | protocol: tags, status codec, command envelope (pure, tested) |
| `RemoteSubsystem` | `Fleet.Drone.Miner/Subsystems/` | drone side: status broadcast, command/config listener |
| `Fleet.Console` | new PB project | mothership/carrier console: roster, per-drone menu, commands |

### 10.2 Protocol (IGC)
- Channel `FLEET/<Channel>/…`, `Channel` a new `[Fleet]` key (default `FM`). The channel name is the
  only access control vanilla IGC offers: anyone in antenna range who knows it can command the drone.
- **status** — drone → broadcast, every `Update100`: one versioned `|`-separated line (name, state,
  return reason, hole n/total, cargo, battery, H2, lift, flags, note). The sender address *is* the drone
  id. This is the one bounded allocation in the drone's loop (one short string per ~1.7 s); IGC boxes
  payloads anyway, so a struct payload would not avoid it.
- **cmd** — console → drone **unicast only** (no broadcast command can start the whole fleet by
  accident): exactly the text of a PB run argument (`START`, `HOME`, `GOTO GPS:…`), plus `SET <section>
  <key> <value>` and `GETCFG`. The drone runs it through the same path as a terminal argument.
- **cfg** — drone → console unicast: the drone's Custom Data, sent on `GETCFG` and after every applied
  `SET`; it doubles as the acknowledgement. A rejected `SET` replies `ERR <reason>` on **ack**.
- The drone never depends on the link: a lost console changes nothing about a running job.

### 10.3 Menu (enhanced PAM-style; UP / DOWN / APPLY / BACK, PAM-compatible run arguments)
Beyond PAM's job page: **every** Custom Data setting is editable, grouped into Job / Flight / Energy /
Behaviour pages; typed fields (number with min/max/step, toggle, choice cycling for enums); a number's
step **accelerates** on repeated presses in one direction; destructive commands (Stop, Reset, Home
from a running job) ask for **confirmation**; long pages **scroll** in a fixed window with ▲/▼ markers
so small LCDs and cockpit screens work; a breadcrumb title and a live status header on every page.
Render is allocation-free. Edits are returned as actions, never applied by the menu: the drone writes
them to its Custom Data; the console sends them as `SET` and shows them as *pending* until the drone's
`cfg` echo arrives.

### 10.4 Console
- **Roster:** up to 16 drones from status broadcasts; *stale* after 10 s, dropped after 60 s.
- **Pages:** Fleet (one line per drone: name, state, cargo, battery) → a drone: live status, Commands,
  Job / Flight / Energy / Behaviour settings (from that drone's `cfg`), **Send to target** (GPS entries
  in the console's own Custom Data `[Targets]` section → `GOTO`).
- **Run arguments:** `UP` `DOWN` `APPLY` `BACK`; `SEND <drone> <command…>`; `ALL <command…>` (fleet-wide,
  sent as one unicast per known drone).
- Displays by the same tag rules as the drone (`[FM]` LCD, `[FM:n]` cockpit surface).

### 10.5 Testing additions
Unit: status codec round-trip and version rejection, command envelope parsing, menu navigation /
editing / acceleration / confirmation / scrolling, schema get/set round-trip for every key, roster
staleness. In-game: **R1** console lists the drone; **R2** editing Width on the console changes the
drone's Custom Data; **R3** START / HOME / STOP from the console; **R4** Send to target → GPS job;
**R5** console destroyed mid-job → the drone carries on; **R6** a second console on another channel
sees nothing.
