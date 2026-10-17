# Merge 02 · Mapping and conflicts

2026-10-16 · CLAUDE(RIG).md §8 phase 2 · source: `01-inventory.md`.

## Mapping (the §8 draft, corrected)

| Fleet element | As found | Rig home | Target project (§9) |
| --- | --- | --- | --- |
| Mother-Forge | lore only, no files | **The Rig** | — |
| Mining drone | built, tested (498), vanilla | **PM-\*** in MNR bays (P-E, P-W); **OM-\*** variant in ICE bays (P-NE) | `scripts/Drone.Miner` (+ `Drone.Ice` = config/variant) |
| Carrier PB | bay beacon (U10) + Dispatcher core (unwired) | **`[LOG] PB Dispatcher`**. The Rig *is* the carrier; the moving-carrier code fits a drifting Rig on a moving planet | `scripts/Rig.Dispatcher` |
| Console PB | roster, menu, SET/GETCFG | **`[CMD] PB Displays`**: RIG//OS FleetOps ROSTER/COMMS/JOBS | `scripts/Rig.Displays` |
| Engine + Flight | kernel, config, IGC codecs, UI, PID, helm, docking | shared library | `scripts/Shared` |
| Survey probe | `jobreq` reserved only | RLY/survey role; FleetOps `PROSPECT` | new: `Drone.Survey` (later) |
| Cargo drones | none | Dispatcher jobs; ≤ 5 B | new or dropped (03) |
| Combat carriers + swarm | none | 12 INT (cross-heads) + 2 PKT (HUB roof); TMR CBT chain | new: `Drone.Interceptor`; PKT = AI blocks, no PB |
| — | — | `[CMD] PB Rig OS` (lift, modes, heartbeat) · `[MFG] PB Inventory` | new: `scripts/Rig.OS`, `scripts/Rig.Inventory` |

## Conflict check (§8 list)

| # | Check | Finding | Proposed resolution |
| --- | --- | --- | --- |
| 1 | Modded blocks on anything that docks | None. All Fleet designs are vanilla | ✓. Defense Shields stays on the Rig only (C2 amended) |
| 2 | Drone size vs 5 B / 9 B bay | Miner size is unconstrained in its build spec | Add to `mining-drone.md`: L ≤ 5 B (12.5 m); bay = bbox + 1 B all round + 2 B ahead of CON |
| 3 | Space-only thrust vs atmo | Miner handles any thrust type | PM-\*: atmo (+ H2 reserve). OM-\*: **H2 only, or atmo + ion**, so they can climb through the T sectors. Lift margin is checked per planet (RSS gravity†, Real Orbits falloff†) |
| 4 | IGC tags / payloads | `FLEET/<ch>/<kind>`, `|`-separated, v2. TMS uses `TMS.*`, `;` | Rename to `FLT.*` (drone↔Rig) and `RIG.*` (Rig↔Rig PBs/console). Separator: see 03 D1 |
| 5 | Naming vs `[ID] Fn n` | Drone tag `[FM]`; carrier finds bays by tag | Rig bays named `[P-E] CON 1…`, found via group `[HGR] Connectors`; bay id = `P-E/1`. The drone's own tag stays configurable |
| 6 | PBs doing safety | Miner returns (damage, energy, cargo) are PB-only | Add native fallback on every drone: D1–D4 ECs + AR RECALL (S1). **The D1–D4 diagram is a placeholder in the guide** → 03 D6 |
| 7 | Scripts duplicating native blocks | Dispatcher plans no door control (good). The guide opens bay doors with SNS + watchdog | Dispatcher never drives doors directly; it acts via groups/TMRs (S4). Interceptors and pickets use AI Offensive/Defensive blocks, not custom flight |
| 8 | Third-party code (C3) | None copied in. PAM was behaviour reference only | ✓ |

## Further conflicts found

| Conflict | Proposed resolution |
| --- | --- |
| Fleet rule 6 stores jobs in **world** space; planets now move (RSS) | Jobs, routes and TMS sites go **planet-relative** (centre from `TryGetPlanetPosition`; rotation basis TBD†). Spec before code |
| Slice-1/2 in-game gates G1–G12, M1–M8 were never run | Re-run in the new creative world after the move, as the Rig's drone acceptance test |
| The bridge console prototype shows 4 pylons | Update to 6 P, T sectors and the bay board when RIG//OS is built (§6) |
