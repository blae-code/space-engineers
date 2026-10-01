# C30 — Integration (subsystems + Program.cs)

Milestone D · Difficulty: hard · Executor: **Claude** (touches every component; a local model's 32k
context cannot hold the interfaces it must combine)

## Goal
Wire every component into a working drone script: the reload hold, block discovery and diagnostics,
sensing, the mission orchestration (FSM → helm/drills/cargo per state), commands, the LCD, `Storage`, and
Custom Data reloads.

## Files
- Create: `Fleet.Drone.Miner/Subsystems/BlockScan.cs` — block discovery + diagnostics (not an ISubsystem)
- Create: `Fleet.Drone.Miner/Subsystems/SenseSubsystem.cs` — `ISubsystem` "Sense"
- Create: `Fleet.Drone.Miner/Subsystems/MinerSubsystem.cs` — `ISubsystem` "Miner"
- Create: `Fleet.Drone.Miner/Subsystems/UiSubsystem.cs` — `ISubsystem` "Ui"
- Create: `Fleet.Drone.Miner/Subsystems/RemoteSubsystem.cs` — `ISubsystem` "Remote" (added 2026-09-30 by spec §10, remote console)
- Modify: `Fleet.Drone.Miner/Program.cs`
- Create: `Fleet.Tests/Mining/ProgramSmokeTests.cs`

## 1. Program constructor (order matters)
1. **Reload hold** — before anything else: on `Me.CubeGrid` only, every `IMyThrust.ThrustOverridePercentage = 0`,
   every `IMyGyro.GyroOverride = false` (Pitch/Yaw/Roll 0), every `IMyShipDrill.Enabled = false`, every
   `IMyShipController.DampenersOverride = true`.
2. `ConfigLoader.Load(Me.CustomData, settings, warnings)`; if the written form differs from the current text
   (missing keys), set `Me.CustomData = ConfigLoader.Write(...)`. Remember the text for change detection.
3. `BlockScan.Scan(...)` → adapters: `ShipIO.Refresh`, `CargoIO.Refresh`, `EnergyIO.Refresh`,
   `DamageScanner.Refresh`, `Displays.Refresh`.
4. Subsystems in order `[Sense, Miner, Ui]`; `StorageStore.Load(Storage, subs, ini, dropped)`; each dropped
   name is logged as an event note `"storage dropped: <name>"`.
5. `Miner` starts with `pendingReconcile = true` (see §4).
6. `kernel.OnSafe = () => { ship.ReleaseControls(); drills off; }`.
7. `Runtime.UpdateFrequency = Update10 | Update100`.

## 2. Main(argument, updateSource)
- `bb.Time += Runtime.TimeSinceLastRun.TotalSeconds`.
- If `(updateSource & (Terminal | Trigger | Script | Mod)) != 0` and the argument is non-empty:
  `CommandParser.Parse` → `Up/Down/Apply/Back` → `ui.Menu(cmd)`; `Reset` → `kernel.ResetSafe()` then
  `miner.Command(Cmd.Reset, rest)`; everything else → `miner.Command(cmd, rest)`; `Unknown` → UI note
  `"unknown command"`.
- `kernel.Tick(updateSource, Runtime.LastRunTimeMs)`.
- `Runtime.UpdateFrequency = Update10 | Update100 | (miner.WantsUpdate1 ? Update1 : None)`.
- `Save()`: `Storage = StorageStore.Save(subs, ini)`. The Miner also calls a `requestSave` callback on every
  FSM transition (Program implements it with the same line).
- Custom Data change detection runs in `Ui.Update100`: if `Me.CustomData` differs from the remembered text,
  reload settings, re-run `Displays.Refresh` with the (possibly new) tag, log `"config reloaded"`.

## 3. BlockScan (grid-scoped; rescanned every 10th Update100, on `REBOOT`, and at construction)
| Need | Rule | Diagnostic if missing |
|---|---|---|
| Ship controller | an `IMyShipController` whose name contains the tag, else the first one (`CanControlShip`) | `No cockpit or remote control` |
| Drone connector | an `IMyShipConnector` without "Eject" in the name (tagged preferred) | `No connector` |
| Ejectors | connectors whose name contains the tag **and** "Eject" | (optional) |
| Gyros / thrusters / drills | all on the grid | `No gyros` / `No thrusters` / `No drills` |
| Power | batteries, hydrogen tanks, reactors | `No power source` if none |
| Inventory blocks | every block with `HasInventory` | — |
| Displays | via `Displays.Refresh` | — |
`Ready` = no diagnostics. The Miner refuses to leave Idle while not ready (Hold note = first diagnostic).
Derived at scan: `refConnInShip = conn.WorldMatrix * MatrixD.Invert(shipMatrix)`;
`refDrillInShip` = a matrix at (mean drill ship-local position + ship-local Forward × 1.5) with the ship's
orientation; drill ship-local positions list for `HoleGrid.SpacingFromDrills`; `bb.ShipSize` = half the
grid bounding-box diagonal.

## 4. MinerSubsystem ("Miner", StorageVersion 1)
**Owns:** `MinerFsm`, `DrillLogic`, `List<Vector2I> holes`, `PathRecorder` (500), dock/job paths,
`PathFollower`, `GpsRoute`, `DockingPlanner`, `StuckDetector` (Settings.StuckSeconds, 0.2), `Helm`, the job
(origin, forward, up, right — all **local to the home connector** via `Frames`), `holeIndex`, `redoHole`,
`jobIsGps`, `manualFull`, `homeConnectorId`, `EventLog` (shared with Ui).

**Update10 (in this order):**
1. Resolve the home connector matrix: if the drone connector is `Connected`, the home is its
   `OtherConnector` (store its `EntityId`); otherwise find the block by stored id via
   `GridTerminalSystem.GetBlockWithId`. Write `bb.HasHome`, `bb.HomeMatrix`.
2. If `pendingReconcile` and `bb.SensingTicks >= 2`: build `ReconcileInput`, `Reconciler.Resolve`,
   `fsm.Force(result, note)`, set `redoHole`, log, `pendingReconcile = false`. Until then the helm is released.
3. Build `MinerInput` from the per-state observations below plus `ReturnTriggers.Evaluate` (TriggerInput
   from `bb`, `manualFull`, `holeIndex >= holes.Count`, `damage.Damaged`); `fsm.Step`; on change → entry
   actions, `EventLog.Add`, `requestSave()`.
4. Run the **per-state action** for the current state.
5. `helm.Update(1.0 / 6)` unless the helm is in `Update1` mode (then `Update1` drives it at `1.0 / 60`).

**Per-state entry actions and per-tick actions** (world targets come from `Frames.ToWorldPoint/Dir` with
`bb.HomeMatrix`):
| State | Entry | Each tick | Observation fed to the FSM |
|---|---|---|---|
| Idle / Hold | `helm.Release()`, drills off | — | — |
| Recording | `recorder.Begin(local pos)`; helm released | `recorder.Update(local pos, speed)` | — |
| Undock | `connector.Disconnect()`; helm reference = connector | target `DockingPlanner.UndockTarget(home, 1.5 × ShipSize)`, forward = current, cap ApproachSpeed | `UndockClear` = distance < 1 m |
| DockPathOut / DockPathIn | `follower.StartNearest(dockPath, reverse: In)` | carrot → world; cap = min(ApproachSpeed×2, speedCap); `BrakeDist = remaining` | `PathDone` = follower.Done (or no dock path) |
| RouteOut / RouteBack | recorded: `StartNearest(jobPath, reverse: Back)`; GPS: `gps.Start(Out ? jobOrigin : dockPath outer end or dock standoff, inGravity)` | recorded as above with cap MaxSpeed; GPS: `gps.Update(...)`, `BrakeDist = gps.Remaining`; forward = travel direction (horizontal on planets), up = −gravity or current up | `RouteDone` = follower.Done / gps Done |
| Position | helm reference = drill face | target = current hole entrance (`HoleGrid.HoleEntrance` + forward × StartDepth), forward = job forward, up = job up, cap ApproachSpeed | `AtHole` = distance < 0.5 m and `helm.AlignmentError < 0.035` |
| Drill | drills on; `drill.Start(Depth − StartDepth, WorkSpeed)` | depth = Dot(face − entrance, fwd); `drill.Update`; target = entrance + fwd × (Depth − StartDepth), cap = `drill.Feed`, `BrakeDist` = remaining depth; `cargo.EjectStep` (also in Retract) | `DrillFinished` = drill.Current != Advance |
| Retract | if entered by a trigger: `drill.BeginRetract()` | target = entrance, cap RetractSpeed; `drill.Update` | `RetractDone` = drill Done; on exit to Position: `holeIndex++` unless `redoHole` (then clear it); log `"hole n blocked"` if `drill.WasBlocked` |
| Dock | `docking.Start(now)`; helm reference = connector | `docking.Update(...)` → `helm.Engage`; `WantsUpdate1 = target.Precision`; if phase Connect → `connector.Connect()` | `Connected`, `DockFailed` = phase Failed |
| Unload | helm released; `manualFull = false` | `cargo.UnloadStep(carrier inventories)`; 60 s timeout → log `"unload timeout"` | `Unloaded` = !cargo.HasOre() or timeout |
| Charge | `energy.SetCharging(true)` | — | `Charged` = energy.IsCharged(); `UraniumLow` = HasReactor && UraniumKg < MinUranium; on exit `SetCharging(false)` |
**Stuck handling (spec §5.5 — back off, retry twice, then Hold).** All moving states except Drill run
`stuck.Update(now, Dot(velocity, dirToTarget), true)` (reset on each state entry). When it fires:
if `stuckRetries < 2` → `stuckRetries++`, fly a **back-off** for 3 s (target = current position − travel
direction × 3 m, cap ApproachSpeed), then `stuck.Reset()` and resume the state's normal target; otherwise
feed `Stuck = true` to the FSM (→ Hold "stuck"). `stuckRetries` resets on every state entry.

**Ejection** (`cargo.EjectStep`) runs in Drill **and** Retract — inside the job area only (spec §6.4).

**Carrier inventories for Unload:** on entering Unload, collect the inventories of every
`IMyCargoContainer` on `connector.OtherConnector.CubeGrid` (containers whose name contains the tag first)
into a preallocated `List<IMyInventory>`; pass it to `cargo.UnloadStep`.

**Commands (`Command(cmd, rest)`):** `SetJob` — needs a home; job origin = drill-face world position →
local; forward/up/right = ship axes → local; spacing from drills; `HoleGrid.Spiral`; `holeIndex = 0`;
`jobIsGps = false`; `damage.TakeBaseline()`. `Goto` — `Gps.TryParse(rest)` (else note `"bad GPS"`);
origin = GPS → local; forward = gravity direction in gravity, else `Normalize(target − home)`;
up = `GyroMath.PerpendicularUp(forward, ship up, ship forward)`; `jobIsGps = true`; spiral + baseline as
SetJob. `RecordDock` / `RecordJob` remember which path is being recorded; `StopRec` copies
`recorder.Points` into that path. `Full` → `manualFull = true`. `Next` / `Prev` → `holeIndex ± 1` clamped to
`[0, holes.Count]`. `Reboot` → rescan + `pendingReconcile = true`. `GyroTest` → only in Idle/Hold and not
connected → `ship.BeginGyroTest(now)`. All commands are also passed to `fsm.Step` via `MinerInput.Command`.

**Save/Load:** `state`, `resume`, `pending`, `note` (the note text, or empty),
`holeIndex`, `redoHole`, `jobIsGps`, `hasJob`, job vectors (x/y/z keys), `spacing`, `width`, `height`,
`homeId`, and both paths via `PathCodec` in sections `Miner.DockPath` / `Miner.JobPath`. `Load` of any
other version returns false.

## 5. SenseSubsystem ("Sense", no saved state)
Update10: from `ShipIO` + controller: `bb.ShipMatrix`, velocities, gravity (`InGravity` = |g| > 0.05),
mass, elevation; `bb.CargoFill = cargo.Fill()`; energy fills; `bb.LiftMargin` = in gravity:
`ThrustAllocator.MaxForceAlong(local(-gravity dir), max) / (mass × |g|)`, else +∞; `damage.Step()`;
connector status → `bb.Connected` / `bb.Connectable`; `bb.SensingTicks++`.

## 6. UiSubsystem ("Ui", no saved state)
Update100 (and immediately after a menu command): build one reused `StringBuilder`:
```
Miner-01  Drill                      <- Name, Names.State[state]
hole 3/25  depth 12.4/30 m           <- or the Hold note / "SAFE: <reason>"
cargo 64%  lift 1.82  bat 88%  H2 71%
instr avg 1234 peak 4567 (9%)        <- Profiler, % of Runtime.MaxInstructionCount
!! No drills                         <- each diagnostic and config warning, prefixed "!! "
<menu>                               <- MenuModel.Render
<last 6 events>                      <- EventLog.Render
```
Write it to `Displays` and to `Me.GetSurface(0)`. Menu `SetValue` → parse `Me.CustomData` with `MyIni`,
`Set(section, key, value)`, write back (the change detector reloads settings). Menu `Command` → forwarded
to the Miner like a run argument. While `ship.GyroTestRunning`, call `ship.UpdateGyroTest(now, report)` and
show the report line.

## 7. Smoke tests — `Fleet.Tests/Mining/ProgramSmokeTests.cs`
Using the MDK test gateway pattern (`FormatterServices.GetUninitializedObject`, set `Runtime`, `Echo`,
`Me`, `Storage`, `GridTerminalSystem`, `IGC_ContextGetter` via `Sandbox.ModAPI.IMyGridProgram`, then invoke
the constructor), with FakeItEasy fakes (non-strict) for an **empty grid**:
- the constructor does not throw;
- `Main("", UpdateType.Update10 | UpdateType.Update100)` does not throw and the kernel is not SAFE;
- the rendered status contains `No cockpit or remote control`;
- `Main("START", UpdateType.Terminal)` leaves the state Idle or Hold (never a moving state).
(Add `FakeItEasy 9.0.1` to `Fleet.Tests.csproj` in this card.)

## Steps
- [ ] 1. Write the smoke tests; run them → fail (types missing).
- [ ] 2. Implement BlockScan, Sense, Miner, Ui, Program in that order, building after each file.
- [ ] 3. `dotnet test Fleet.Tests 2>&1 | tail -n 25` → `Passed!` (all cards' tests + smoke).
- [ ] 4. `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15; fish tools/check-size.fish` → deployed, < 90,000 chars.
- [ ] 5. Commit: `feat(miner): integrate subsystems into the drone script`.

## Done when
All tests pass, the size gate passes, and Checkpoint D's in-game checklist can begin.
