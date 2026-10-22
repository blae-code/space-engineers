# Fleet Miner drone — build spec

How to build a mining drone for `Fleet.Drone.Miner` from scratch: the parts it needs, the orientation
rules, the home station, and the steps to its first job. Everything here comes from the script's own
block discovery (`Fleet.Drone.Miner/Subsystems/BlockScan.cs`) and settings (`Fleet.Engine/Config/`).
If the code and this page disagree, the code wins. Fix the page when that happens.

## 1. Drone parts (small grid)

Every block must be on the **same grid as the programmable block**. Blocks behind rotors, pistons or
hinges are on another grid and are ignored. If a required block is missing, the drone's LCD names it
and `START` is refused.

| Part | Required | Notes |
|---|---|---|
| Programmable block | yes | Runs `Fleet.Drone.Miner`. |
| Cockpit or remote control | yes | Must be able to control the ship. **Its forward is the drilling direction** (see §2). If there are several, the one with the tag in its name wins. |
| Connector (docking) | yes | Used to dock at home. Any orientation. If there are several, the one with the tag wins. Connectors with "Eject" in the name are never used for docking. |
| Gyroscopes | yes, at least 1 | Use enough to turn the ship briskly at full load. Gyro authority limits how fast it aligns at each hole. |
| Thrusters | yes, at least 1 | **Cover all six directions.** The autopilot groups thrusters by push direction, and a missing direction means no control along that axis. On a planet, the upward thrust must lift the full ship with margin (see Lift below). |
| Drills | yes, at least 1 | All facing the cockpit's forward. Hole spacing is worked out from the drill layout. |
| Power | yes, one of these | Battery, hydrogen tank (for H2 thrusters) or reactor. Batteries and tanks are recharged at home. |
| Cargo containers | strongly advised | Every block with an inventory counts as cargo, and the fill % is measured across all of them. |
| Ejector connectors | optional | Name each one with the tag **and** `Eject`, e.g. `Connector Eject [FM]`. The script switches on Throw Out and dumps the `[Miner] Eject` ores (default `Stone`) while drilling. |
| LCDs | optional | A text panel whose name contains `[FM]` shows the status and menu. For a cockpit or other multi-screen block, put `[FM:n]` in its name, where `n` is the screen index (starting at 0). |
| Antenna | optional | Only needed for the mothership console (`Fleet.Console`) or a moving carrier (`Fleet.Carrier`). Without one you get a warning, nothing more. |
| Button panel / toolbar | optional | For the menu: run the PB with `UP`, `DOWN`, `APPLY`, `BACK`. |

**Lift (planets).** The drone works out its lift margin (upward thrust ÷ current weight) continuously.
If it drops below `[Miner] MinLiftMargin` (default **1.3**), it returns home before it gets too heavy to
climb. Build so the margin stays above 1.3 **with the cargo full**, or the drone will go home well
before `MaxLoad`. Under-powered lift shows up as early "low lift" returns. In space the margin is
infinite.

## 2. Orientation rules

- **The cockpit or remote control must face the way the drills cut.** The script places the "drill
  face" 1.5 m ahead of the drills' mean position, along the cockpit's forward. `SETJOB` takes the
  hole direction from that frame. If the cockpit faces sideways, the drone mines sideways.
- The cockpit's **up** becomes the job's up. Width runs along its right, height along its up.
- The docking connector can sit anywhere and face any way. The drone lines that connector up with the
  home connector.
- Hole spacing: the widest spread of the drills (across the cockpit's right/up) plus 2 × 1.4 m × 0.9
  overlap. A wider drill head means fewer, wider holes. It is fixed when the job is set, so re-run
  `SETJOB`/`GOTO` after changing the drills.

## 3. Home (the dock)

| Part | Required | Notes |
|---|---|---|
| Connector | yes | The drone docks here. The approach runs along the connector's forward, so leave a clear lane in front of it, at least ~1.5 × the drone's size. |
| Cargo containers on the same grid | yes, for unloading | Ore goes into **cargo containers only** (not refineries or assemblers) on the home connector's grid. Containers whose name contains the tag are filled first. If everything is full, the drone holds with "carrier full: free space, then CONT". |
| Power | yes, for charging | While docked, batteries switch to Recharge and tanks to Stockpile. Charging gives up after 5 minutes and leaves partly charged. |
| Antenna + `Fleet.Console` PB | optional | Remote control from the mothership. Its `[Console] Channel` must match the drone's `[Fleet] Channel`. |
| `Fleet.Carrier` PB + antenna | only if home moves | Tag the docking connectors `[FM]`. The drone refuses to undock from a moving home that has no carrier script. |

A **static** station needs no script. The drone remembers where its connector was.

## 4. Setup steps

1. **Build the drone and name its blocks.** Put the tag (default `[FM]`) in the names of the LCDs, any
   ejectors (plus `Eject`), and the cockpit or connector if there is more than one. To reuse a ship
   built for PAM, either rename its `[PAM]` LCDs or set `Tag=[PAM]` in step 3.
2. **Load the script.** Open the PB → Edit → Browse Scripts → `Fleet.Drone.Miner` → OK. Or, if MDK
   hasn't deployed it on this machine: open the PB → Edit, select all, and paste the whole of
   [`mining-drone.script.cs`](mining-drone.script.cs) (a snapshot of the Release build, so re-copy it
   after any rebuild; see the file list in `CLAUDE.md`). Either way, do this on a
   PB with **empty Custom Data**. Old text that isn't INI (PAM leftovers, notes) makes the drone run
   on defaults, never write its settings, and refuse every menu or remote edit. Old Storage is
   discarded harmlessly ("storage dropped").
3. **Check Custom Data.** The first run writes every setting. Edit, then save. The drone reloads
   Custom Data when it changes, or you can use the LCD menu. Set at least:
   - `[Fleet] Name`: defaults to the grid name with spaces turned into `-`. Must be unique per channel.
   - `[Miner] Width`, `Height` (holes), `Depth` (m).
   - `[Flight] SafeAltitude` on planets: cruise height above the ground.
4. **Read the LCD.** It must show no setup diagnostics. If it does, add the named block and run
   `REBOOT`.
5. **GYROTEST (mandatory before the first autonomous flight).** Undock, hover clear of everything and
   run `GYROTEST`. It spins pitch, yaw and roll for 2 s each and prints one verdict per axis. **All
   three must say OK.** `SIGN FLIPPED`, `UNITS: RPM?` or `UNEXPECTED` means a code constant
   (`GyroMath.*Sign` / `ShipIO.GyroScale`) needs fixing first. Do not start a job.
6. **Dock once.** Paths and jobs are stored relative to home, so `RECORD`, `SETJOB` and `GOTO` all say
   "dock first" until the drone has been connected.
7. **Record the dock path.** While docked, run `RECORD DOCK`, fly out of the bay by hand to open space,
   then run `STOPREC`. The drone retraces this path to leave and return.
8. **Set the job.** Use one of these:
   - Fly to the dig site with the drills facing the rock, then `SETJOB`. The current drill face
     becomes the corner hole, and the cockpit's orientation becomes the job's orientation.
   - Or `GOTO GPS:name:X:Y:Z:`. On a planet it drills straight down along gravity. In space it drills
     along the line from home to the target.
   - Optional: `RECORD JOB` … `STOPREC` records a route around obstacles to the site. A recorded route
     is only valid while home has not moved.
9. **Dock, then `START`.** The drone undocks, flies the dock path and the route, drills the holes in a
   spiral, and returns when full, low on power, low on lift or damaged. It then unloads, recharges,
   and relaunches while `Loop` is on.

## 5. Settings reference (Custom Data, defaults)

| Section | Key | Default | Range | Meaning |
|---|---|---|---|---|
| Fleet | Name | grid name | — | Name shown on the console and used by `SEND` |
| Fleet | Tag | `[FM]` | — | Name tag for LCDs, ejectors, preferred blocks, home containers |
| Fleet | Channel | `FM` | — | IGC channel shared with console and carrier |
| Miner | Width / Height | 5 / 5 | 1–50 | Holes across / up |
| Miner | Depth | 30 | 1–500 m | Hole depth |
| Miner | StartDepth | 0 | 0–500 m | Start drilling this far in (skip the surface) |
| Miner | WorkSpeed | 1.5 | 0.1–10 m/s | Drill feed speed |
| Miner | RetractSpeed | 4 | 0.1–20 m/s | Pull-out speed |
| Miner | MaxLoad | 90 | 10–100 % | Return when cargo is this full |
| Miner | MinLiftMargin | 1.3 | 1.05–5 | Return when lift ÷ weight falls below this |
| Miner | Eject | `Stone` | — | Comma-separated ore subtypes to dump through ejectors |
| Miner | Loop | true | — | Relaunch after unloading |
| Miner | OnDamage | Home | Home / Job / Stop | What to do when a block is damaged |
| Flight | MaxSpeed | 60 | 1–500 m/s | Cruise speed |
| Flight | ApproachSpeed | 5 | 0.5–50 m/s | Near holes and the dock |
| Flight | DockSpeed | 0.5 | 0.1–2 m/s | Final creep onto the connector |
| Flight | SafeAltitude | 150 | 20–5000 m | Planet cruise height |
| Flight | StuckSeconds | 5 | 1–60 s | No progress for this long counts as stuck: back off and retry twice, then hold |
| Energy | MinBattery | 20 | 0–90 % | Return below this |
| Energy | MinHydrogen | 30 | 0–90 % | Return below this |
| Energy | MinUranium | 5 | 0–1000 kg | Return below this |
| Reload | OnReload | Resume | Resume / ReturnHome / Hold | After a world reload |

## 6. Commands

Run these as the PB argument from the terminal, a toolbar or a timer, or send them from the console.
Case does not matter.

`START` `STOP` `HOME` `CONT` · `SETJOB` `GOTO GPS:…` · `RECORD DOCK` `RECORD JOB` `STOPREC` ·
`NEXT` `PREV` (skip holes; only when stopped or positioning) · `FULL` (treat cargo as full) ·
`GYROTEST` · `REBOOT` (rescan blocks) · `RESET` · `UP` `DOWN` `APPLY` `BACK` (menu) ·
`SET <section> <key> <value>`.

## 7. Before the first job

- [ ] LCD shows no diagnostics
- [ ] GYROTEST: pitch, yaw and roll all say OK
- [ ] Lift margin above 1.3 with full cargo (planets)
- [ ] Clear lane in front of the home connector
- [ ] Cargo containers with free space on the home grid
- [ ] Dock path recorded, job set
- [ ] Size gate: `fish tools/check-size.fish` passes

After that, work through the in-game checklist in
`docs/superpowers/plans/2026-09-29-slice1-core-miner.md` (G1–G12, R1–R6).
