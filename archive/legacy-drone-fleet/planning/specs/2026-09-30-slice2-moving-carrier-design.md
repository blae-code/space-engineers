# Slice 2 — Moving-carrier docking + carrier PB: Design

Status: approved for build by Blae ("proceed through slice two yourself", 2026-09-30). Decisions marked
**(D)** were made by Claude without a round-trip and are open to reversal.

## 1. Goal and success criteria
A logistics **carrier** (large grid) can be **moving** — drifting, flown by a player, or on the vanilla
Remote Control autopilot — while its drones undock, mine and dock back. Slice 1's stationary carrier
keeps working unchanged, with or without the new carrier script.

1. A drone launches from, and docks back onto, a carrier moving in a straight line at up to ~10 m/s
   in space, and one turning slowly (≤ 2 °/s).
2. The job site stays where it is in the world while the carrier moves away from it.
3. If the carrier's position is lost (script off, out of range), the drone never flies at a guessed
   connector: it holds at a safe point and says why.
4. A reload while the carrier moves ends in Hold or a safe resume (slice 1 rules), never a collision.
5. Both scripts stay under 100,000 characters; no new allocation in the drone's per-tick paths.

Out of scope: carrier flight control (the carrier is flown by a player or vanilla autopilot), connector
queuing / multi-drone allocation (slice 3), obstacle avoidance (slice 5), planet-rotation effects.

## 2. The two problems a moving carrier creates
1. **Undocked, the drone cannot see its home connector.** The carrier's blocks are not on the drone's
   terminal system. Slice 1 kept the last known pose, which is only right for a static carrier.
2. **Slice 1 stored the job relative to the home connector** (spec §5.1). That was right for paths
   (they belong to the carrier) and wrong for the job: a moving carrier would drag the dig site through
   space with it. Asteroids and (in vanilla) planets do not move.

## 3. Carrier PB — `Fleet.Carrier` (new)
- **Bays:** every connector on the carrier's own grid whose name contains the tag (`[FM]`) is a bay.
- **Beacon:** every `Update10`, one IGC broadcast per bay on `FLEET/<Channel>/bay`, payload
  `MyTuple<long, Vector3D, Vector3D, Vector3D, Vector3D, Vector3D>` = (bay entity id, position,
  forward, up, **point velocity of the bay**, carrier angular velocity). Point velocity = `v + ω × r`
  (r from the carrier's centre of mass), from a ship controller on the carrier, else by differencing
  positions between ticks **(D)**. A struct payload, so the drone decodes it without allocating.
- **LCD:** per bay: name, free / docked drone (grid name), plus channel and beacon rate.
- Config (`[Carrier]` in its Custom Data): `Channel`, `Tag`. The console stays a separate script **(D)**
  — it can run on the same carrier in a second PB.

## 4. Drone changes
- **HomeTracker** (pure, tested): keeps the latest beacon for the drone's home bay and its receive
  time; `Predict(now)` extrapolates position by velocity and orientation by angular velocity over the
  beacon's age. Fresh while age < 2 s.
- **Home resolution:** docked → the real connector; undocked and fresh → predicted pose and velocity;
  undocked, never heard a beacon → last known pose (**slice 1 behaviour, static carrier**); undocked,
  heard before but stale → **home lost**.
- **Home lost** in any homeward state (RouteBack, DockPathIn, Dock, Undock) → Hold, note
  `carrier beacon lost`; CONT once it is back. Outbound and mining states carry on (the job is in
  world space) and only the return waits.
- **Helm velocity feed-forward:** `PoseTarget.Velocity` (world m/s of the frame the target lives in).
  The helm tracks `Velocity + braking-curve approach`; the braking curve runs on the relative distance.
  Zero velocity = exactly slice 1 behaviour, so every slice-1 test still holds.
  Home-relative targets (undock, dock path, docking, route home) carry the carrier's point velocity;
  job targets carry zero.
- **Moving route home:** `GpsRoute.Retarget` moves the end point every tick without restarting the
  climb/cruise phases.
- **Job in world space:** job origin/axes and the recorded **job route** are stored in world
  coordinates; the dock path stays home-relative. `StorageVersion` 3; version-2 storage is migrated by
  converting the job and job route with the saved home pose (spec §4 "compatible data is migrated").
- **Reconcile:** `HomeFound` additionally requires the beacon not to be stale (when one was ever heard).

## 5. Testing
Unit: beacon pack/unpack; point velocity `v + ω × r`; HomeTracker prediction (translation, rotation,
staleness, wrong bay ignored); helm steady state matches a moving target's velocity and still stops on
a static one; `GpsRoute.Retarget` keeps the phase; v2 → v3 storage migration; drone smoke test with a
fake carrier beacon (home pose follows the beacon, lost beacon holds the return).

In-game checklist (M):
- **M1** Carrier parked, carrier script running: slice 1 checklist still passes.
- **M2** Carrier drifting at ~5 m/s in space: undock, mine an asteroid, return, dock, unload, relaunch.
- **M3** Carrier yawing slowly (~1 °/s) while the drone docks.
- **M4** Carrier script switched off mid-RouteBack: the drone holds with `carrier beacon lost`; turned
  back on → CONT → docks.
- **M5** Reload while the carrier moves and the drone is out: drone holds or resumes, no collision.
- **M6** Carrier flown by a player during the whole cycle (realistic speeds, turns).
