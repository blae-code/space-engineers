# Building a FleetMiner mining drone

This guide covers what the script expects from the drone's hull: which blocks, where they go, and how to tag them. Everything uses vanilla blocks. The script finds blocks by their in-game *type* (thruster, drill, camera…), not by model, so every size, skin and DLC variant of a block works the same way.

## The mining cycle

```
Docked ──(unloaded, charged, drills intact)──▶ Undocking ──▶ Transit ──▶ Working ─┐
  ▲                                                          ▲   (next shaft)  │
  │                                                          └─────────────────┤
  └── FinalDock ◀── Approach ◀── RequestDock ◀──(hold full / low power / damaged / too heavy / 'return')
```

1. **Docked:** batteries recharge and H2 tanks stockpile. The drone pushes its ore into the carrier's cargo. It launches once the hold is empty, power is back up and every drill works. A damaged drone waits, so welders around the dock repair it automatically.
2. **Undocking:** backs straight off the connector, matching the carrier's velocity.
3. **Transit:** flies to a *staging point* `ApproachDistance` in front of the current shaft, then slides square-on to the shaft entry. Legs longer than `AutopilotRange` use the Remote Control autopilot, which has vanilla collision avoidance.
4. **Working:** one shaft at a time.
   - Glides at `ApproachSpeed` through open space and any hole it has already cut.
   - Drills at `MineSpeed` from the rock face to `MineDepth` past it.
   - Backs out along the shaft axis.
   - Shafts spiral outward from the site origin (`MaxShafts` of them).
   - If the drone goes home mid-shaft, it resumes the same shaft next trip.
   - A shaft that finds no rock within `ScanRange`, or makes no progress for `StallTime`, is skipped.
5. **RequestDock → Approach → FinalDock:** once clear of the face, the drone gets a dock slot. It then follows the carrier's streamed connector pose, even while the carrier is moving.

## Required blocks

| Block | Count | Notes |
|---|---|---|
| Programmable Block | 1 | Runs the script. Its own screen shows status. |
| **Remote Control** | 1 | **The reference block.** Its *forward* must point the way the drills cut, and its *up* is "up" while mining. Tag another controller `[FM Ref]` to use it instead, but autopilot needs a Remote Control. |
| Gyroscopes | 1+ | Any orientation. |
| Thrusters | all 6 directions | Any type mix (ion/atmo/hydrogen, any size or skin). Forward and backward thrust need to be strong enough for precise moves in and out of the shaft. |
| Connector | 1 | Tag `[FM Dock]` if there is more than one. Put it anywhere *except* the drill face. Rear or belly works well. Docking math is relative, so any position works. |
| Drills | 1+ | All facing Remote Control forward. A flat, compact bank cuts the cleanest shafts. |
| Batteries and/or H2 tanks | 1+ | Return thresholds: `ReturnCharge`, `ReturnHydrogen`. |
| Cargo containers | optional | Drills have inventory, but more cargo means fewer trips. |
| **Antenna** | 1 | Radio (or laser). Without one, the drone can't hear the carrier once undocked. |

## Recommended blocks (what makes it "feature complete")

### Forward camera — `[FM Cam]`
- **Placement:** facing Remote Control forward, flush with the drill face, with a clear view ahead. Next to the drill bank is ideal.
- **What it does:**
  - `setsite` raycasts the rock and moves the site plane to `SiteStandoff` m in front of the face. You can aim from 50–100 m away.
  - Each shaft is scanned first, so the drone glides fast to the face instead of creeping through open space.
- **Without a camera:** the drone detects contact when cargo starts rising, which works but is slower. If the camera isn't tagged, any camera facing forward is used.

### Stone dump — `[FM Eject]` on a sorter and on an ejector (or connector)
- **Setup:** conveyor sorter output → ejector. The script sets the sorter itself to whitelist Stone with *Drain All*.
- **When it runs:** only while drilling, and only after the rock face has been found, so the contact signal is never masked. It's off while docked and in transit, so stone is never thrown at the carrier.
- **Why:** the hold fills with ore, not gravel, and trips get much longer.
- **To keep stone:** leave these blocks off the drone.

### Status screens — `[FM LCD]`
Any LCD (any size or DLC variant) or block with screens (cockpit, console, button panel). The block's first screen is used.

### State timers — `[FM <State>]`
A timer block whose name contains a state tag is triggered when the drone enters that state. Use them for anything the script doesn't do itself:
- `[FM Working]`: turn on work lights or play a sound.
- `[FM Docked]`: switch on welders or start a sorter on the carrier.
- `[FM RequestDock]`: turn on beacon lights so you can see the drone coming home.
- `[FM Idle]`: send an alarm.

Tags: `[FM Idle]` `[FM Docked]` `[FM Undocking]` `[FM Transit]` `[FM Working]` `[FM RequestDock]` `[FM Approach]` `[FM FinalDock]`.

## Vanilla/DLC notes

- **Variants are free:** thrusters, LCDs, cockpits, cargo and drills from any DLC are the same block *types* to the script. Use whatever looks right.
- **Event Controllers:** these work well next to the script, e.g. for alarms, or to run the PB with `return` on a condition the script doesn't watch.
- **AI / autopilot blocks:** keep AI flight or task blocks *off* on the drone. Like a second autopilot, they would fight the script's gyro and thruster overrides. The script only drives the Remote Control's autopilot itself, and only for the long, static outbound leg.
- **Ore detector:** the in-game script API can't read ore positions, so the script doesn't use it. Fit one for your HUD or for antenna-broadcast ore markers if you want.
- **Ice and hydrogen:** an O2/H2 generator on a hydrogen drone can top up its own tanks from ice it mines. Ice in cargo is still unloaded at the carrier.

## Orientation checklist

1. Remote Control forward = drill direction, and Remote Control up = the drone's "up". `setsite` copies this pose.
2. Camera forward = Remote Control forward.
3. Sorter arrow points into the ejector.
4. Connector not on the drill face. Nothing protrudes ahead of the drill face except the drills.

## Carrier side

- Connectors tagged `[FM Dock]`: one per drone that may be home at the same time.
- Cargo containers on the carrier grid. Tag the ones drones should fill `[FM Unload]`. If none are tagged, any container on the carrier grid is used.
- Antenna, and a ship controller if the carrier moves.
- Optional: welders around each dock, so damaged drones get repaired before relaunch.

## Planet mining

- `Lift xN` on the status display is the drone's thrust against gravity divided by its weight, in its current attitude.
- If it drops below `MinLift` (1.25), the drone treats itself as full and heads home. That's before it gets too heavy to climb out of the shaft.
- A drone that can't make `MinLift` even when empty stops its cycle and asks for more thrust.
- Rule of thumb: build for at least 1.5× the full-cargo weight on whichever side faces "up" (per the Remote Control) while mining.

## Custom Data reference (`[FleetMiner]`)

| Key | Default | Meaning |
|---|---|---|
| `CargoFull` / `CargoEmpty` | 0.90 / 0.02 | Return at / relaunch below this cargo fill |
| `LaunchCharge` | 0.90 | Battery (and H2) needed to relaunch |
| `ReturnCharge` / `ReturnHydrogen` | 0.25 / 0.20 | Head home below these |
| `MaxSpeed` / `ApproachSpeed` / `DockSpeed` | 40 / 8 / 1.5 | m/s for long legs / near rock and carrier / last metres of docking |
| `MineSpeed` | 1.0 | m/s while cutting |
| `MineDepth` | 30 | m to drill past the rock face |
| `ShaftSpacing` | 0 | m between shafts. 0 = measured from the drill bank (with 10% overlap) |
| `MaxShafts` | 25 | Shafts per site (25 = 5×5). 0 = unlimited |
| `SiteStandoff` | 10 | m in front of the face for the site plane (camera `setsite`) |
| `FaceMargin` | 2 | m before the scanned face where the glide stops |
| `ScanRange` | 100 | m of camera range; also how far an empty shaft goes before it is skipped |
| `StallTime` | 20 | s without progress before a shaft is skipped |
| `MinLift` | 1.25 | Lift ratio that triggers return in gravity |
| `Autopilot` / `AutopilotRange` | true / 300 | Use the Remote Control autopilot for outbound legs longer than this |
| `Unload` | true | Push cargo into the carrier when docked |
| `CamTag` / `EjectTag` / `UnloadTag` | `[FM Cam]` / `[FM Eject]` / `[FM Unload]` | Name tags |

## First test (creative mode)

1. Build the drone docked to a carrier (or station) running `Role=Carrier`.
2. Undock by hand and fly 50–100 m from an asteroid. Aim the Remote Control at the face and run `setsite`. The status should say *Site set in front of the rock face*.
3. Dock again, then run `start`. Watch `Shaft n/25`, `seeking face` / `cut x/30 m`, `Cargo %`.
4. Things to check:
   - Transit arrives square-on.
   - Stone ejects only while cutting.
   - The drone backs fully out before turning for home.
   - Ore lands in the carrier.
   - It relaunches once empty.
5. `return` mid-shaft should back out first. `skip` should move to the next shaft.

## Known limits

- The site plane must be open space. The drone moves sideways along it between shafts.
- The flight home from the site to the carrier is a straight line.
- Several miners on one site would each mine the same spiral. Assigning shafts across the fleet is a carrier feature for later.
