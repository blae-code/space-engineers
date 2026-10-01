# Slice 2 — Moving carrier: plan record and in-game checklist

**Spec:** `docs/superpowers/specs/2026-09-30-slice2-moving-carrier-design.md`. Built by Claude directly
(Blae: "proceed through slice two yourself"), no task cards; every component landed with tests.

## What was built
| Piece | Where | Proven by |
|---|---|---|
| Helm velocity feed-forward + linear near-target gain | `Fleet.Flight/Flight/Helm.cs` | `HelmMovingTargetTests` (point-mass sim: converges on a 5 m/s target; without feed-forward it lags > 0.5 m; static target still stops) + all C18 tests |
| `PoseTarget.Velocity` | `Fleet.Engine/Core/PoseTarget.cs` | as above |
| Bay beacon codec, point velocity, `HomeTracker` | `Fleet.Engine/Net/Beacon.cs` | `BeaconTests` (round trip, right-handed rotation prediction, staleness) |
| `GpsRoute.Retarget` | `Fleet.Flight/Flight/GpsRoute.cs` | `GpsRetargetTests` + C16 tests |
| Drone: beacon-tracked home, world-space job, home-lost hold, Storage v3 + v2 migration | `Fleet.Drone.Miner/Subsystems/*` | smoke tests `CarrierBeacon_TrackedThenLost`, `StorageV2_JobMigratesToWorld` |
| Carrier PB | `Fleet.Carrier/Program.cs` | build vs Bin64 (no unit harness: a second `IngameScript.Program`) |

## In-game checklist (spec §5) — run after slice 1's G1 (GYROTEST) passes
Test world: a carrier (large grid, connectors tagged `[FM]`, antenna, a cockpit or remote control,
`Fleet.Carrier` running), the drone docked to one bay, an asteroid nearby.
- [ ] **M1** Carrier parked, carrier script running: the slice-1 checklist still passes; the drone LCD
  shows `carrier tracked 0.0 m/s` while out.
- [ ] **M2** Carrier drifting at ~5 m/s in space: undock, mine the asteroid, return, dock, unload, relaunch.
- [ ] **M3** Carrier yawing slowly (~1 °/s) while the drone docks.
- [ ] **M4** Carrier script switched off mid-RouteBack: drone holds with `carrier beacon lost`; switched
  back on → CONT → docks.
- [ ] **M5** Reload while the carrier moves and the drone is out: Hold or safe resume, no collision.
- [ ] **M6** Carrier flown by a player through a full cycle.

## Log
