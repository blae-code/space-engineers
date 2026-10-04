# Building and flying a FleetMiner fleet

This guide covers what the script expects from your ships (which blocks, where they go, how to tag them) and what the fleet does once it's flying. Everything uses vanilla blocks. The script finds blocks by their in-game *type* (thruster, drill, camera…), not by model, so every size, skin and DLC variant of a block works the same way.

## The fleet

```
                 MOTHERSHIP  (Role=Mothership)
        fleet HQ: site library, fleet totals, orders
                      ▲   HQ channel   │
   status, deliveries,│                │ launch / recall / halt,
   sites, results     │                ▼ site assignments, map data
                 CARRIER  (Role=Carrier) ◀─── shuttle HAULERs ───▶ mothership pads
         flight control: pads, queue, launches
                      ▲  local channel │
                      │                ▼
              MINERs  ◀── hand off ore ──▶  site HAULERs (bays at the work site)
```

- **Miners** cut shafts, then deliver ore to a site hauler (if one has a free bay) or to their carrier.
- **Site haulers** (`Role=Hauler`, `HaulMode=Site`) hold station off the rock face with `[FM Bay]` connectors. Miners unload into them and go straight back to work. When full, the hauler flies the ore home.
- **Shuttle haulers** (`Role=Hauler`, `HaulMode=Shuttle`) load ore from their carrier and fly it to the mothership's pads, then come back for more.
- **Carriers** run flight control for their drones and report up to the mothership.
- **The mothership** sees every carrier. It holds the master site library and fleet production, and its `launch`, `recall`, `halt` and `assign N` commands reach every drone through their carriers.

All of it is optional. A single carrier and a few miners works on its own, as before.

## The mining cycle

```
Docked ──(unloaded, charged, repaired, launch clearance)──▶ Undocking ──▶ Transit ──▶ Working ─┐
  ▲                                                                       ▲  (next shaft)  │
  │                                                                       └────────────────┤
  └── FinalDock ◀── Approach ◀── RequestDock ◀──(hold full / power / damage / heavy / link lost / 'return')
                    (retraces     (holds in a ring          │
                     breadcrumbs,  if pads are full)        └── to a hauler's bay instead, when one has room
                     gives way)
            Evading ◀── hostile located (runs away, decoys on) ──▶ RequestDock after FleeTime
```

1. **Docked:**
   - Recharge and unload. Then wait for launch clearance from flight control: drones leave one at a time, with a countdown.
   - Damaged drones wait on the pad until welders fix them.
2. **Transit:**
   - Flies nose-first to a staging point in front of the shaft.
   - The camera looks ahead and plots a **detour** around anything in the way.
   - Legs longer than `AutopilotRange` use the Remote Control autopilot.
   - Breadcrumbs and the drone's burn rate are recorded on the way.
   - On planets an **altitude floor** (`MinAltitude`) keeps it off the ground.
3. **Working:**
   - **Camera survey:** while near the face, the camera sweeps every untouched shaft. Each one gets its rock-face distance, or is marked "no rock", before anyone flies there.
   - **Ore-aware shaft choice:** the next shaft is the one whose neighbours yielded the most, with `Prefer` ores weighted 5×.
   - **When it gives a shaft up:** after `BarrenDepth` m of ore-free rock (barren), with no rock within range, or when it stalls.
4. **Going home:**
   - **Full hold and a site hauler has room:** it docks there instead and is back to work within a minute.
   - **Otherwise:** it heads home, gives way to other drones near the pads, and holds in a ring if all pads are full.
   - **Breadcrumbs:** it retraces its outbound path while the crumbs still lead toward the carrier.

### When it goes home

| Reason | Trigger |
|---|---|
| Hold full | Cargo ≥ `CargoFull` (a site hauler is preferred if one has a free bay) |
| Power | Battery or H2 below what it takes to get home: learned burn per metre × distance × `EnergyMargin`, plus `ReserveCharge` |
| Too heavy | In gravity, lift drops below `MinLift` |
| Drill damaged | It waits on the pad for repairs |
| **Link lost** | Home carrier unreachable for `LinkTimeout`, and no other carrier to re-home to. It flies to where the carrier was last heard and waits there in antenna range. |
| **MAYDAY** | Hull loss beyond `DamageTolerance`, or a turret, turret controller or `[FM Sensor]` sees an enemy. If the threat's position is known, it evades first (see below). |

## Mining lasers

A miner with forward-facing mining lasers (instead of, or as well as, drills) **bores shafts from outside**:
- It hovers just short of the rock face on each shaft's axis and fires down it, never flying into the hole.
- It stops when the shaft is `MineDepth` past the face, or as deep as the beam reaches, whichever is less.
- Shaft spacing comes from the beam's diameter.
- Everything else (survey, ore-aware choice, barren/empty detection, partial resume, hand-offs) works as with drills.

Two laser mods are supported, detected automatically:

| | Adjustable Mining Laser | ToolCore tools (e.g. Simple Laser Multitools) |
|---|---|---|
| Depth | Exact: the laser reports its contact distance, and the script sets the beam's range so it stops at the shaft bottom | From the camera when it can see down the hole, else time × `LaserSpeed` |
| Range / radius | Radius read from the block; range defaults to the mod's 60 m / 20 m maximum (`LaserRange` to change) | `LaserRange` / `LaserRadius`, default 300 m × 2.5 m (large) or 100 m × 1.5 m (small) |
| Stone | `KeepStone=false` (default) sets the laser's own *Ignore Stone* | Use the `[FM Eject]` stone dump |
| Safety | — | **Switched to Drill mode and read back before every shot.** A ToolCore beam in Grind mode would cut through ships, so the script refuses to fire if the mode doesn't read *Drill*. |

**Beam safety:**
- With a forward camera, the script checks the beam path every ~0.2 s and holds fire while anything other than rock (a ship, a person) is in it.
- A ToolCore beam in Drill mode doesn't harm ships, but it does hurt people.
- Fit a camera next to the laser. The self-test warns if there isn't one.

**Never put ToolCore laser *turrets* on drones or near the pads.** They auto-target anything they can weld or grind.

## Charting sites from GPS

```
chart 2 GPS:Cobalt node:12345.6:-2345.1:98765.4:#FF75C9F1:
```

The GPS can be pasted straight from your GPS list.
- **On a miner:** it charts the site itself.
- **On a carrier:** the nearest docked or idle miner is sent.

What the miner does:
1. Flies to the target.
2. Finds the rock with its camera: the near face of an asteroid, or the ground on a planet, where it points straight down.
3. Stops `SiteStandoff` short of the rock.
4. Records library site N and starts mining it.

Other drones join with `assign N`. This pairs well with **Seismic Surveying** (planet ore nodes) and **Radio Spectrometry** (asteroid composition): read their screens, copy a GPS, and send a drone.

## Communication

| | |
|---|---|
| **Two channels** | `Channel` is local: a carrier and its drones. `Channel/HQ` connects carriers, the mothership and shuttle haulers. Drone chatter never reaches HQ. |
| **Fleet key** | Set the same `FleetKey` on every ship. Messages without it are ignored, so a stranger who guesses your channel name can't read or command your fleet. |
| **Reliable messages** | Deliveries, site charts and shaft results are acknowledged and resent until they arrive, for up to 5 minutes. Reports made out of range are delivered once the drone is back in range. Duplicates are filtered, so nothing is counted twice. |
| **Best effort** | Status, claims, survey data and beacons are sent once. They repeat often, so a lost one doesn't matter. |
| **Link awareness** | Drones check every ~2 s whether home is reachable. Out of reach for `LinkTimeout`: they **re-home to the nearest reachable carrier**, or if there is none, fly to home's last known position. An orphan drone with no home adopts the nearest carrier. |
| **Antenna range** | With `AntennaAuto` on, drones set their antenna range to distance-home × 1.5 (short when docked, full when the link is lost). That saves power and makes them less conspicuous. Turn it off if you rely on drones relaying for each other. |
| **Traffic** | Near the pads, a drone within `Separation` of another gives way (the higher address yields, and anyone yields to a drone on final), for at most 20 s. |

## Sensing

| Block | What the script does with it |
|---|---|
| Camera `[FM Cam]` | Rock-face ranging, the site survey, `setsite` from range, and look-ahead obstacle detection |
| Sensor `[FM Sensor]` | Set to *enemies only* automatically (`ConfigureSensors`). Detected enemies trigger MAYDAY with their position. |
| Turrets / turret controllers | Anything they target is a threat, with its position and velocity |
| AI Defensive block `[FM Flee]` | Evasion: the script disables its own helm and calls the block's `Flee()`. It needs an AI Move (flight) block on the same grid. |
| Decoys | Switched on during MAYDAY, off once docked |
| Searchlights `[FM Searchlight]` (carrier/mothership) | Set to track friendly small ships, on while any pad has a ship on final approach |
| Antenna | Range managed as above; HUD name shows callsign and state |
| Ship controller | Planet elevation for the altitude floor, gravity for lift |
| Batteries | Live drain → "minutes left" on the detail page |
| Ore detector | *No script API for ore positions*; fit one for your HUD only |

**Evading:** with a located hostile, the drone flies `FleeDistance` m directly away from it (or hands over to an `[FM Flee]` block), turns on decoys, then heads home after `FleeTime` s. A drone caught mid-shaft backs out first. Carriers log the MAYDAY, fire `[FM Distress]` hooks, pass it up to HQ and, with `RecallOnDistress`, recall their fleet.

## Blocks and tags

**Drone, required:**
- **Programmable Block.**
- **Remote Control:** its forward is the drill direction.
- **Gyros.**
- **Thrusters on all 6 sides.**
- **Connector:** tag it `[FM Dock]`.
- **Batteries and/or H2 tanks.**
- **A broadcasting antenna.**
- **Miners:** drills facing forward.
- **Site haulers:** one or more `[FM Bay]` connectors for miners to dock to, plus cargo space.

**Drone, recommended:**

| Block | Tag |
|---|---|
| Forward camera | `[FM Cam]` |
| Sorter + ejector (stone dump) | `[FM Eject]` |
| Sensor | `[FM Sensor]` |
| Decoy | (none needed) |
| AI Defensive + AI Move blocks | `[FM Flee]` on the AI Defensive block |
| Status lights | `[FM Light]` |
| Screens | see *Screens* |
| Timers / sound blocks | see *Hooks* |

**Carrier / mothership:**

| Block | Tag / notes |
|---|---|
| Pad connectors | `[FM Dock]` |
| Pad lights | `[FM Pad 0]`, `[FM Pad 1]`, … |
| Searchlights | `[FM Searchlight]` |
| Cargo | `[FM Unload]` marks the containers to fill (drones unload into them; shuttles load from them) |
| Antenna | Required |
| Ship controller | Needed if the base moves |
| Screens | `[FM Board]`, `[FM Map]`, `[FM Log]`, `[FM Stats]` |
| Welders | Around the pads, for repairs |

## Screens

| Tag | Shows |
|---|---|
| `[FM LCD]` | Status text (includes link state, outbox, survey count, threat range) |
| `[FM Gauges]` | Drone gauges + a footer showing LINK OK/LOST, queued reports, threat distance |
| `[FM Map]` / `[FM Map N]` | Shaft grid by result and yield (bases: library site N) |
| `[FM Board]` | One tier down: a carrier lists its drones, the mothership lists its carriers and shuttles |
| `[FM Log]` | Comms log |
| `[FM Stats]` | Production by ore, per hour, per drone (carrier) or per carrier (mothership) |

## Hooks

A timer triggers, and a sound block plays, when its name contains one of these tags:

- **States:** `[FM Idle]` `[FM Docked]` `[FM Undocking]` `[FM Transit]` `[FM Working]` `[FM RequestDock]` `[FM Approach]` `[FM FinalDock]` `[FM Evading]`
- **Events:** `[FM Backout]` `[FM Rich]` `[FM Holding]` `[FM Launch]` `[FM Distress]` `[FM LinkLost]`

## Commands

| Drone | |
|---|---|
| `setsite` / `setsite N` | Record the site (private, or charted as library site N, sent reliably up to HQ) |
| `site N` | Join carrier library site N |
| `start` / `return` / `stop` | Self-test then run the cycle / come home / halt. Shuttle haulers need no site. |
| `skip` / `goto N` / `resetsite` / `forget` | Shaft and home management |
| `chart N GPS:…` | Fly to the GPS, find the rock, chart it as library site N, mine it |

| Carrier / mothership | |
|---|---|
| `launch` / `recall` / `halt` | Fleet orders (from the mothership they go to every carrier, which passes them on) |
| `assign N` | Send everyone below to library site N, with its map |
| `chart N GPS:…` | Carrier: send the nearest docked/idle miner to chart a GPS target as site N |
| `reset` | Clear pad reservations and queues |

| Any | |
|---|---|
| `status` | Detail page for 30 s: self-test, burn rates, outbox, battery minutes, elevation, site counts |
| `selftest` | Build check. Adds a hauler check: bays fitted, or a mothership heard for shuttles. |
| `set <Key> <value>` | Change a number or switch live. It's saved into Custom Data when the world saves. |
| `resetstats` | Zero production counters |

## Custom Data reference (`[FleetMiner]`)

| Key | Default | Meaning |
|---|---|---|
| `Role` | Miner | Miner, Hauler, Carrier, Mothership |
| `HaulMode` | Site | Haulers: Site (bays at the work site) or Shuttle (carrier → mothership) |
| `Channel` / `FleetKey` / `Callsign` | FLEETMINER / *(empty)* / *(grid name)* | Channel name (HQ uses `Channel/HQ`), shared secret, display name |
| `Prefer` | Platinum,Uranium,Gold | Ores worth 5× when ranking shafts |
| `CargoFull` / `CargoEmpty` | 0.90 / 0.02 | Return at / relaunch below this cargo fill |
| `LaunchCharge` | 0.90 | Battery (and H2) needed to relaunch |
| `ReturnCharge` / `ReturnHydrogen` | 0.25 / 0.20 | Return thresholds until the burn rate is learned |
| `ReserveCharge` / `EnergyMargin` | 0.10 / 1.5 | Reserve kept, safety factor on the trip home |
| `MaxSpeed` / `ApproachSpeed` / `DockSpeed` | 40 / 8 / 1.5 | m/s |
| `Decel` | 4 | m/s² the helm plans braking with |
| `ApproachDistance` / `DockGap` | 40 / 1.5 | Staging distance / connector gap on final approach |
| `MineSpeed` / `MineDepth` | 1.0 / 30 | m/s while cutting / m past the face |
| `ShaftSpacing` / `MaxShafts` | 0 / 25 | 0 = from the drill bank / shafts per site (max 121) |
| `SiteStandoff` / `FaceMargin` / `ScanRange` | 10 / 2 / 100 | Site plane offset / glide stop / camera and survey range |
| `StallTime` / `BarrenDepth` / `ClaimTimeout` | 20 / 10 / 900 | Give-up rules, claim expiry |
| `MinLift` / `MinAltitude` | 1.25 / 50 | Lift ratio to return at / planet surface clearance in transit (0 = off) |
| `Autopilot` / `AutopilotRange` | on / 300 | Remote Control autopilot for long outbound legs |
| `CrumbSpacing` / `HoldDistance` | 100 / 150 | Breadcrumb spacing / holding ring radius (+30 m per queue place) |
| `Separation` | 25 | m that counts as traffic near the pads |
| `LaunchInterval` / `LaunchCountdown` | 10 / 5 | Launch sequence timing |
| `DamageTolerance` / `RecallOnDistress` | 0.03 / on | MAYDAY trigger, carrier recalls its fleet on one |
| `FleeDistance` / `FleeTime` | 1500 / 30 | Evasion run |
| `LinkTimeout` | 60 | s without reaching home before re-homing / lost-link return |
| `AntennaAuto` / `AntennaMax` | on / 50000 | Antenna range management |
| `ConfigureSensors` / `Survey` | on / on | Enemy-only sensor setup / camera site survey |
| `Unload` | on | Push cargo into the pad server's containers when docked |
| `LaserRange` / `LaserRadius` | 0 / 0 | Mining lasers: 0 = auto (see *Mining lasers*) |
| `LaserSpeed` | 2 | m/s a ToolCore laser cuts, for depth when the camera can't see down the hole |
| `KeepStone` | off | Adjustable Mining Laser: keep stone instead of its *Ignore Stone* |
| `BeaconTimeout` / `DockTimeout` | 5 / 60 | Docking timeouts |

## First test (creative mode)

1. Build a mothership (`Role=Mothership`) and a carrier (`Role=Carrier`) a few km apart. Give each pads, an antenna, `[FM Board]` and `[FM Log]` screens, and the same `FleetKey`.
2. Dock two miners and one site hauler (with `[FM Bay]` connectors) to the carrier. Put a shuttle hauler there too.
3. Run `selftest` on each drone.
4. With one miner, `setsite 0` at an asteroid. On the mothership, run `assign 0`, then `launch`.
5. Watch:
   - The carrier passes both orders down, and the miners leave one at a time.
   - The survey fills the `[FM Map]`.
   - The site hauler parks off the face, and miners hand ore over to it.
   - The shuttle carries ore from the carrier to the mothership.
   - The mothership's `[FM Stats]` totals rise.
6. Stress tests:
   - Fly a drone out of antenna range: it should report "lost link" and come back.
   - Put an asteroid between a drone and its target: it should detour.
   - Spawn an enemy ship near a sensor-equipped drone: it should MAYDAY and evade.

## Using with other mods and scripts

| Mod / script | Effect | What to do |
|---|---|---|
| **Adjustable Mining Laser**, **ToolCore** + **Simple Laser Multitools** | Supported as mining lasers | See *Mining lasers*. Keep laser turrets off drones. |
| **WeaponCore** | Replaces vanilla turrets, so vanilla "has a target" goes quiet | Supported: threats come from WeaponCore's script API automatically (the status page says "WeaponCore threat feed active") |
| **Defense Shields** | Shields on drones and bases | Supported: a drone's shield dropping 25 points since launch is a MAYDAY. If the home carrier's shield is up, a drone under attack heads for it (safe harbour). Bases report shield % (the mothership's board shows it as each carrier's second bar). |
| **Seismic Surveying**, **Radio Spectrometry** | Ore readings on special LCD screens that scripts can't read | Copy a GPS from their results and use `chart`. The thumper is a landing gear: build it on a survey ship, not a drone. |
| **Spug's Easy Auto-Docking 2** | Its own docking script | Never on fleet drones (two scripts would fight over the gyros). On ships you fly, fine. Tag drone pads `[FM Dock]` and leave your own docking connectors untagged. |
| **Isy's Inventory Manager** | Sorts cargo, feeds refineries | Add `[FM Unload]` to the names of its ore containers so shuttles find ore. Keep it from stuffing docked drones (its exclusion keywords). Decide whether carrier refineries or the mothership get the ore. |
| **Isy's Solar Alignment** | Turns panels or whole ships | Rotor mode only on bases with docking traffic (gyro mode rotates the pads). Keep `[FM Light]` / `[FM Pad N]` lights out of its light control. |
| **Isy's Block Renaming** | Bulk renames | Great for adding `[FM …]` tags. "Restore default names" strips them, and its custom-data delete would wipe a drone's settings. |
| **Isy's Docked Ships Info**, **Connector Alignment App**, **LandingComputer** | LCD displays | Don't also give those LCDs an `[FM …]` tag. A Connector Alignment screen next to each pad is a nice way to watch drones dock. |
| **Definition Extension API**, **Rich HUD Master**, **Text HUD API**, **Visual Overrides API**, **ModAdjusterV2** | Libraries | No effect. If an adjustment changes the laser tools' range or radius, set `LaserRange` / `LaserRadius` to match. |
| **Build Vision**, **BuildInfo**, **Leak Finder**, **Automatic Ore Pickup** | Player tools (ore pickup only affects hand drills now) | No effect |

Every script needs its own programmable block. The fleet script owns its block's `[FleetMiner]` Custom Data section and leaves other sections alone.

## Known limits

- The site plane must be open space. Drones move sideways along it between shafts.
- Look-ahead only sees within the camera's cone (about 45°). Drones fly nose-first on long legs so it points the right way.
- Lost-link and holding positions are based on the carrier's last report. A carrier that keeps moving while out of contact will be somewhere else.
- A shuttle hauler waits for the mothership's pad assignment before it starts flying, so the mothership must be reachable over IGC (antennas or relays in between).
- Antenna auto-range can break drone-to-drone relay chains. Turn off `AntennaAuto` if you rely on them.
- Saved state from earlier versions of this script is discarded once on upgrade: run `setsite` again.
- ToolCore lasers can't report how deep they've cut. Without a camera that can see down the hole, depth is estimated from time × `LaserSpeed`.
- Landing gears on drones are unlocked at launch and auto-lock is turned off, so a drone never grabs the rock it is cutting.
