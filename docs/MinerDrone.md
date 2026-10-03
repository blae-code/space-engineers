# Building and flying a FleetMiner fleet

This guide covers what the script expects from your ships (which blocks, where they go, how to tag them) and what the fleet does once it's flying. Everything uses vanilla blocks. The script finds blocks by their in-game *type* (thruster, drill, camera…), not by model, so every size, skin and DLC variant of a block works the same way.

## The mining cycle

```
Docked ──(unloaded, charged, repaired, launch clearance)──▶ Undocking ──▶ Transit ──▶ Working ─┐
  ▲                                                                       ▲  (next shaft)  │
  │                                                                       └────────────────┤
  └── FinalDock ◀── Approach ◀── RequestDock ◀──(hold full / power / damage / heavy / MAYDAY / 'return')
                    (retraces     (holds in a ring
                     breadcrumbs)  if pads are full)
```

1. **Docked:**
   - Batteries recharge and H2 stockpiles. The drone pushes its ore into the carrier's cargo and reports the delivery.
   - When it's empty, charged and has working drills, it asks flight control for **launch clearance**. Drones leave one at a time, with a countdown.
   - A damaged drone waits on the pad, so welders around the pad repair it.
2. **Undocking:** backs straight off the connector, matching the carrier's velocity.
3. **Transit:**
   - Flies to a *staging point* in front of the shaft, then slides square-on to the shaft entry.
   - Drops **breadcrumbs** along the way and learns how much battery and H2 it burns per metre.
   - Legs longer than `AutopilotRange` use the Remote Control autopilot, which has collision avoidance.
4. **Working:** one shaft at a time.
   - Glides through open space and any hole already cut, then drills `MineDepth` past the rock face, then backs out.
   - **What gets mined next follows the ore.** Every shaft records its yield. The next shaft is the untouched one whose neighbours yielded the most, weighted 5× for ores in `Prefer`. Barren rock and the asteroid's edge are avoided, and interrupted shafts are always finished first.
   - A shaft is given up early in these cases:
     - **Barren:** `BarrenDepth` m into rock with no ore at all.
     - **No rock:** no rock within `ScanRange`.
     - **Blocked:** no progress for `StallTime`.
5. **RequestDock → Approach → FinalDock:**
   - If every pad is taken, the drone holds in a ring around the carrier, spaced out by its place in the queue.
   - Once a pad is assigned, it retraces its outbound breadcrumbs (skipping any that no longer lead toward the carrier). It then docks by following the carrier's streamed connector pose, even while the carrier moves.

### When it goes home

| Reason | Trigger |
|---|---|
| Hold full | Cargo ≥ `CargoFull` |
| Power | Battery (or H2) below what it takes to get home: learned burn per metre × distance × `EnergyMargin`, plus `ReserveCharge`. Before the drone has learned its burn rate, `ReturnCharge` / `ReturnHydrogen` are used instead. |
| Too heavy | In gravity, lift drops below `MinLift`. If it can't lift even when empty, the cycle stops. |
| Drill damaged | It waits on the pad until repaired. |
| **MAYDAY** | Hull integrity drops by more than `DamageTolerance` since launch, *or* a turret, turret controller or `[FM Sensor]` sensor sees something. The drone broadcasts a distress call. The carrier logs it, fires `[FM Distress]` hooks and, with `RecallOnDistress`, recalls the whole fleet. |

## Drone: required blocks

| Block | Notes |
|---|---|
| Programmable Block | Runs the script. Its screen shows status, or the detail page after `status` / `selftest`. |
| **Remote Control** | **The reference block.** Its *forward* is the drill direction and its *up* is "up" while mining. It's needed for the autopilot. Tag another controller `[FM Ref]` to use that one instead. |
| Gyroscopes | Any orientation. |
| Thrusters, all 6 directions | Any type mix. The self-test checks all six directions. |
| Connector `[FM Dock]` | Anywhere except the drill face. |
| Drills | All facing Remote Control forward. |
| Batteries and/or H2 tanks | Power for the trip and the return budget. |
| **Antenna** (broadcasting) | Without one the drone can't hear flight control once undocked. |

## Drone: recommended blocks

| Block / tag | What it adds |
|---|---|
| Camera `[FM Cam]`, facing forward | Finds the rock face, so the drone glides in fast. `setsite` works from 50–100 m away. |
| Sorter + ejector `[FM Eject]` | Dumps stone while drilling, so only ore fills the hold. The sorter is set up automatically. |
| Sensor `[FM Sensor]` | Threat detection. Set its detection filters yourself (e.g. enemies only). |
| Turrets / turret controller | Anything they target counts as a threat. |
| Lights `[FM Light]` | Colour by state. See *Signals*. |
| Screens | See *Screens*. |
| Timers / sound blocks | See *Hooks*. |

## Carrier

- Connectors `[FM Dock]`: one pad each. Lights named `[FM Pad 0]`, `[FM Pad 1]`, … show each pad's state.
- Cargo containers, optionally tagged `[FM Unload]` to choose which ones drones fill.
- Antenna, and a ship controller if the carrier moves.
- Screens: `[FM Board]` is the flight-control board. `[FM Map]` or `[FM Map 2]` shows a site map. `[FM Log]` and `[FM Stats]` show the comms log and production.
- Welders around the pads repair drones before relaunch.

## Sharing a site between drones

1. Fly one drone to the rock, aim, and run `setsite 0`. The number (0–7) charts the site in the carrier's library.
2. On the carrier, run `assign 0`. Every drone in range adopts site 0, and the carrier sends them its map.
3. Alternatively, run `site 0` on a single drone to have just that one join.

Drones broadcast every shaft they claim and finish, so they never drill the same shaft. If two drones pick the same shaft at the same moment, the one with the lower address keeps it and the other picks again. A plain `setsite` with no number makes a private site that isn't shared.

## Screens

Tag any LCD, or any block with a screen (cockpit, console…). The block's first screen is used.

| Tag | Shows |
|---|---|
| `[FM LCD]` | Status text |
| `[FM Gauges]` | Drone gauges: cargo, battery (with a marker for the charge needed to get home), H2, hull, lift, shaft progress |
| `[FM Map]` / `[FM Map N]` | Shaft grid. Gold = rich, green = done (brighter is richer), orange = partial, blue = in progress, brown = barren, dark = no rock, red = blocked, white outline = current shaft. On the carrier, `N` picks the library site. |
| `[FM Board]` | Carrier flight control: free pads, holding queue, launch countdown, and a row per drone (state, cargo and battery bars, ETA or distance, MAYDAY highlighted) |
| `[FM Log]` | Comms log, e.g. `[12:04] MINER-2: hold full, RTB` |
| `[FM Stats]` | Ore delivered by type, total, rate per hour, per-drone totals |

## Signals: HUD, lights, hooks

- **HUD:** antennas and beacons are renamed to `<callsign> | <state>`, e.g. `MINER-2 | MINING`, `MINER-2 | HOLDING`, `MINER-2 | MAYDAY`. Set `Callsign` in Custom Data; otherwise the grid name is used.
- **Lights `[FM Light]`:**

  | Situation | Colour |
  |---|---|
  | Docked | Green |
  | Launching | Blinking cyan |
  | En route | Cyan |
  | Mining | White |
  | Backing out of a shaft | Blinking amber |
  | Returning / holding | Yellow (slow blink while holding) |
  | Final docking | Blinking green |
  | MAYDAY | Fast red strobe |

- **Pad lights `[FM Pad N]` (carrier):**

  | Pad | Colour |
  |---|---|
  | Free | Green |
  | Drone inbound | Blinking green |
  | Occupied | Blue |
  | Reserved | Yellow |

- **Hooks:** a timer is *triggered*, and a sound block *plays*, when its name contains:
  - **State tags:** `[FM Idle]` `[FM Docked]` `[FM Undocking]` `[FM Transit]` `[FM Working]` `[FM RequestDock]` `[FM Approach]` `[FM FinalDock]`
  - **Events:** `[FM Backout]` (leaving a shaft), `[FM Rich]` (rich vein found), `[FM Holding]` (pads full), `[FM Launch]` (launch clearance), `[FM Distress]` (MAYDAY; on the carrier too)

  Pick the sound in the sound block itself. Timers can do anything else: doors, spotlights, alarms.

## Commands

| Drone | |
|---|---|
| `setsite` / `setsite N` | Record the site from the current pose (private, or charted as library site N) |
| `site N` | Ask flight control for library site N |
| `start` | Run the self-test, then begin the automatic cycle |
| `return` | Back out of the shaft, fly home, dock; the cycle stops |
| `stop` | Halt in place, dampeners on |
| `skip` | Abandon the current shaft (marked blocked) |
| `goto N` | Mine shaft N next (the current one stays resumable) |
| `resetsite` | Forget the shaft results; start the spiral again |
| `forget` | Forget the home carrier |

| Carrier | |
|---|---|
| `launch` / `recall` / `halt` | Fleet commands (launches are still sequenced) |
| `assign N` | Send every drone to library site N |
| `reset` | Clear pad reservations and queues |

| Both | |
|---|---|
| `status` | Detail page on the PB screen for 30 s: self-test results, learned burn rates, site counts |
| `selftest` | Check the build (controller, gyros, 6-direction thrust, lift, connector, drills, antenna, camera, stone-dump conveyor path, power, flight control). `start` refuses to run while anything fails. |
| `set <Key> <value>` | Change any number or switch below live, e.g. `set MineDepth 50`, `set Autopilot off`. Written back to Custom Data on the next world save. |
| `resetstats` | Zero the production counters |

## Custom Data reference (`[FleetMiner]`)

| Key | Default | Meaning |
|---|---|---|
| `Role` / `Channel` / `Callsign` | Miner / FLEETMINER / *(grid name)* | Role, fleet channel, name on HUD and screens |
| `Prefer` | Platinum,Uranium,Gold | Ores worth 5× when ranking shafts |
| `CargoFull` / `CargoEmpty` | 0.90 / 0.02 | Return at / relaunch below this cargo fill |
| `LaunchCharge` | 0.90 | Battery (and H2) needed to relaunch |
| `ReturnCharge` / `ReturnHydrogen` | 0.25 / 0.20 | Return thresholds until the burn rate is learned |
| `ReserveCharge` / `EnergyMargin` | 0.10 / 1.5 | Learned budget: reserve kept + safety factor on the trip home |
| `MaxSpeed` / `ApproachSpeed` / `DockSpeed` | 40 / 8 / 1.5 | m/s: long legs / near rock and carrier / last metres of docking |
| `Decel` | 4 | m/s² of braking the helm plans with |
| `ApproachDistance` / `DockGap` | 40 / 1.5 | Staging distance (site and dock) / connector gap on final approach |
| `MineSpeed` / `MineDepth` | 1.0 / 30 | m/s while cutting / m to drill past the face |
| `ShaftSpacing` / `MaxShafts` | 0 / 25 | 0 = measured from the drill bank / shafts per site (max 121) |
| `SiteStandoff` / `FaceMargin` / `ScanRange` | 10 / 2 / 100 | Site plane in front of the face / glide stop before it / camera range and empty-shaft limit |
| `StallTime` / `BarrenDepth` | 20 / 10 | s without progress / m of ore-free rock before giving a shaft up |
| `ClaimTimeout` | 900 | s before another drone's unfinished claim can be taken over |
| `MinLift` | 1.25 | Lift ratio that triggers a return in gravity |
| `Autopilot` / `AutopilotRange` | on / 300 | Remote Control autopilot for outbound legs longer than this |
| `CrumbSpacing` | 100 | m between breadcrumbs (spacing doubles if a trip runs out of slots) |
| `HoldDistance` | 150 | m from the carrier for the holding ring (+30 m per queue place) |
| `LaunchInterval` / `LaunchCountdown` | 10 / 5 | s between launches / countdown before each one |
| `DamageTolerance` / `RecallOnDistress` | 0.03 / on | Hull loss that triggers MAYDAY / carrier recalls the fleet on a MAYDAY |
| `Unload` | on | Push cargo into the carrier when docked |
| `BeaconTimeout` / `DockTimeout` | 5 / 60 | s without a dock beacon / s allowed for final docking |
| `DockTag` `LcdTag` `RefTag` `CamTag` `EjectTag` `UnloadTag` `SensorTag` | `[FM …]` | Name tags |

## First test (creative mode)

1. Build a carrier (`Role=Carrier`) with two pads, `[FM Board]` and `[FM Log]` screens, and pad lights. Build two drones and dock them.
2. On each drone, run `selftest` and fix anything marked FAIL. WARN is fine to start with.
3. Undock one drone and aim it at an asteroid 50–100 m away. Run `setsite 0`, then dock it again.
4. On the carrier, run `assign 0` and then `launch`. The drones should leave one at a time with a countdown, then split the shafts between them.
5. Watch these:
   - `[FM Map]` fills in.
   - The log reports deliveries.
   - Holding works: give the carrier one pad for two drones.
   - MAYDAY works: grind a block off a drone mid-flight.

## Known limits

- The site plane must be open space. Drones move sideways along it between shafts.
- Breadcrumbs cover the outbound path from the carrier's position *at launch*. If the carrier has moved far since then, the drone flies straight once no crumb leads closer.
- The production rate counts time since the script started plus saved time. It's reset with `resetstats`.
- Saved state from earlier versions of this script is discarded once on upgrade: run `setsite` again.
