# Mule Build Guide

Oct 16, 2026 · @Blae

MU-1 is the TMS pod hauler: a straddle carrier that drives over a Bulk Pod, kneels to merge onto its roof, rises to lift it, and carries it between the Crawler and the Ground Depot.

## Key

Shared TMS codes and symbols are defined in the Crawler guide; MU adds these.

| Code | Meaning | Code | Meaning |
| --- | --- | --- | --- |
| MU | Mule, pod hauler | SP | Saddle Port (MRG-S under spine) |
| MRG-S | Saddle merge, faces down | MRG-T | Pod roof merge, faces up |
| CON-C | Charge / GD connector, front portal | KN | Kneel: suspension height lowered |
| S1, S2 | Staging pads at site | BAY | GD pod bay (FP-type connector) |
| RT | Route: recorded waypoint list | WP | Waypoint |
| E | Empty BP | F | Full BP |

**Units:** as Crawler guide; cargo at 1× inventory.

## Role in TMS

MU is the only ground link between CR and GD: it takes full pods from the Crawler's slots, delivers them to a GD bay, and returns empties. The subsystem map and BP container spec live in the Crawler guide; the pod sled frame is owned here.

**Duties**

| Duty | From → to | Trigger |
| --- | --- | --- |
| Collect F | CR slot A/B → GD BAY | `TMS.POD FULL` |
| Return E | GD → CR slot via S1/S2 | Same trip, inbound leg |
| Stage | GD BAY ↔ LP | GD request (Rig lift) |
| Recharge | GD CON-C | R-M7 on connect |

**Pod sled** (BP container + these blocks; identical on every pod)

| Item | Qty | Position | Purpose |
| --- | --- | --- | --- |
| ⊞ MRG-T | 1 | Roof centreline on a 1 b riser, faces up | MU saddle |
| ▼ LG | 4 | Belly corners, face down | Park; lock before unmerge (M7) |
| ⊕ CON-F | 1 | Short end A native port, 1 b adaptor | Fill at CR FP, drain at GD BAY |
| BCN | 1 | Roof, beside MRG-T | Pod id `BP-nn`; MU finds it |
| Battery (small) | 1 | Roof | Keeps BCN lit off-grid |
| Riser armor | ≈4 | Under MRG-T | Clears container lid detail |

Pods carry no PB and no EC (Crawler C1); whichever vehicle holds a pod reads it through ⊕ or ⊞.

## Design rules

| # | Rule | Why |
| --- | --- | --- |
| M1 | BP standard is the only cargo interface (Crawler guide, Subsystem context) | Same pod at CR, MU, GD, RL |
| M2 | One BP per trip, carried ⊞ merged under the spine; never towed on a connector | Merged = one grid: stable, ECs see BP cargo |
| M3 | Lift by KN (suspension height), no pistons; fallback 2 short spine pistons if T-M2 fails | Clang; fewer subgrids |
| M4 | Payload ladder 5% → 10% → 20% `fill_pct`; step up only after the load tests pass | Wheel load is unknown until tested |
| M5 | Speed governor: loaded ≤ 10 m/s, empty ≤ 20 m/s (start values, tune) | Braking distance with 100–450 t |
| M6 | ▲ native blocks own merge, lock and speed safety; ◆ PB drives and aligns | Script crash = MU stops, pod stays put |
| M7 | Unmerge only when BP LG ×4 are ▼ locked | Never drop a pod |
| M8 | Drop BP only on lockable ground (pad, bay, voxel) | LG must lock |
| M9 | Site swap via staging pads S1, S2; CR keeps cutting into the other slot | No mining downtime |
| M10 | MU recharges at GD on CON-C; CR never powers MU | CR power stays for CUT |
| M11 | v1 driven by hand with PB assist; v2 RT autopilot by wheel overrides | Earn trust before autonomy |
| M12 | Every EC/TB action *sets* a state, never toggles | EC re-trigger (Crawler C12) |

## General arrangement

MU-1 is an open-ended portal frame, 16 × 9 b, with a 4 b (10 m) clear tunnel that a 3 b pod passes through; the saddle merge hangs from the spine centre.

&#91;embedded content: MU-1 GA · front + side elevation · 8 callouts\]

**Callouts**

| # | Item | Note |
| --- | --- | --- |
| 1 | Wheel Suspension 5×5 | 3 per side, outboard (Drivetrain) |
| 2 | Cab | On spine, front; cockpit + LCD STAT |
| 3 | Spine | 1 b deep; conveyor not needed |
| 4 | ⊞ MRG-S over ⊞ MRG-T | Saddle pair; gap closed by KN |
| 5 | Portal post ×4 | Ends open; pod enters from either end |
| 6 | Side beam ×2 | At axle height; carries wheels |
| 7 | BP | Separate grid until merged |
| 8 | ⊕ CON-C | Front portal; charge at GD |

**Principal dimensions**

| Dim | Value |
| --- | --- |
| Length | 16 b (40 m) |
| Width over wheels | ≈ 9 b (22.5 m) |
| Tunnel W × H | 4 × 4 b (10 × 10 m) for a 3 × 3 b pod on skids |
| Saddle gap at ride | ≈ 0.5 b; KN must close it, then lift ≥ 0.5 m (T-M2) |
| Wheel ⌀ | ≈ 5 b assumed (Crawler T13) |

## Pod saddle

The pod is held by one merge pair; suspension height (KN) does all vertical motion, so the saddle has no moving parts of its own.

&#91;embedded content: Saddle pick sequence · 4 steps\]

**Saddle parts**

| Item | Block | Position | Note |
| --- | --- | --- | --- |
| ⊞ MRG-S | Merge Block (L) | Spine underside, centre, on 1 b drop block | Faces down; On by default |
| CAM-SP | Camera | Spine underside, beside MRG-S | Driver alignment view onto MRG-T |
| Spot | Spotlight | Spine underside | Night alignment |
| BCN target | On BP (Role in TMS) | Pod roof | Finds pod at range |

**Alignment budget:** merge pull-in tolerance is unknown (T-M3); until measured, align by CAM-SP to within one small-grid block before KN.

## Drivetrain

Six 5×5 wheels on two side beams carry the pod between them; payload is unproven until the load ladder (T-M1) is run, so the design reserves an 8-wheel option.

**Wheels**

| Item | Spec |
| --- | --- |
| Wheel Suspension 5×5 (L) | ×6 (3 per side); ×8 option (D-M2) |
| Each | 2,820 kg, 1.5 MW peak, 2 × 2 × 3 b suspension block |
| Steering | Front and rear axles steer, rear inverted (4-wheel steer); centre axle fixed |
| Mount | Outboard of side beams; side beams at axle height |

**Suspension settings** (start values; tune per payload step)

| Setting | Empty | Loaded | Set by |
| --- | --- | --- | --- |
| Strength | 40% | Raise until no bottoming | Hand, then PB table |
| Friction | 100% | 100% | Fixed |
| Power | 100% | 100% | Fixed |
| Speed limit | 20 m/s | 10 m/s | PB on EC-M1 merge event |
| Height offset | Ride | Ride | KN uses this (Saddle) |
| Propulsion / steering override | Off (v1) | Off (v1) | PB RT autopilot (v2) |

**Load ladder** (T-M1, each step must pass all four)

| Step | `fill_pct` | Ore | Pass criteria |
| --- | --- | --- | --- |
| L0 | 0% (empty BP) | 0 t | Suspension does not bottom; stops from 20 m/s |
| L1 | 5% | ≈111 t | Climbs 15° slope; stops from 10 m/s in ≤ 40 m |
| L2 | 10% | ≈222 t | Same; no wheel break on 1 m drop |
| L3 | 20% | ≈443 t | Same; else stay at L2 or fit 8 wheels |

At L3 the pod alone is roughly 450 t; the 40 m stop and 15° slope are design targets to measure against, not known capabilities.

## Power and mass budget

MU is all DRIVE: ≤ 9 MW peak on six wheels, fed by two large batteries that recharge at GD; payload dwarfs the vehicle from L2 up.

**Power**

| Item | Qty | Each | Total |
| --- | --- | --- | --- |
| Wheel Suspension 5×5 (L) | 6 (8) | 1.5 MW | 9.0 (12.0) MW peak |
| EC, TB, PB, ANT, lights | — | measure | < 0.5 MW |
| Battery (L) | 2 | 12 MW out · 3 MWh | 24 MW · 6 MWh |
| H2E (L), optional | 1 | 5 MW | Range extender |

Full-throttle endurance on batteries ≈ 6 MWh ÷ 9 MW ≈ 40 min; real duty is lower. Recharge at GD via CON-C (R-M7 sets batteries to Recharge on connect).

**Mass**

| Item | Mass |
| --- | --- |
| Wheels ×6 | 16.9 t |
| Batteries ×2 | 7.7 t |
| Frame, cab, logic | Measure in Info tab |
| BP empty (container only) | 5.2 t |
| Payload L1 / L2 / L3 | ≈111 / 222 / 443 t |

## Routes and docking

With two Crawler slots, one visit takes four short moves via staging pads S1 and S2; a third Crawler slot (D-M1) would cut that to one drop and one pick.

&#91;embedded content: Site swap + GD layout · 4 moves\]

**Site layout rules**

- S1, S2: flat, lockable, ≥ 1 b clear on all sides; within 30 m of the slots.
- Approach lanes: MU tunnel axis parallel to pod long axis at every slot, pad and bay.
- `TMS.SWAP RELEASED` from CR before move 2; `PLACED` to CR after move 3 (R5 docks it).

**GD bay spec** (mirrors the Crawler fill port)

| Item | Spec |
| --- | --- |
| BAY ⊕ | Connector on 1 piston ≤ 2 m, aligned to BP CON-F at short end A |
| Bays | 3 to start: 2 unloading, 1 empty-staging |
| Drain | Sorter whitelist by ore; rare ores ⇢ refineries, common ores stay in BP for RL |
| LP | Lockable pad sized for 1 BP; RL merges from above |
| CON-C | Front-height connector for MU charge (R-M7) |

**Route recording (v2)**

1. Drive GD → CR by hand with `rec gd-cr`; PB stores one WP per 20 m in CD.
2. `play gd-cr` outbound, `rev gd-cr` inbound; hand control for the last 30 m.
3. Re-record after terrain changes; keep the route off the cone's surface footprint.

## Control logic

Nine rungs keep the pod attached and the vehicle governed; the PB only drives, kneels and aligns. Unmerge exists on no toolbar: only TB-DROP fires it, and only the PB starts TB-DROP after R-M2.

**EC / TB ladder**

| Rung | Block | Event · condition | Watches | Slot 1 (true) | Slot 2 (false) | Type |
| --- | --- | --- | --- | --- | --- | --- |
| R-M1 | EC-MRG | Merge Block Merged | MRG-S | PB `loaded` | PB `empty` | ◆ |
| R-M2 | EC-BPL | Landing Gear Locked (AND) | BP LG ×4 (merged grid) | PB `bp_locked` | PB `bp_free` | ◆ |
| R-M3 | EC-GRD | Landing Gear Locked (any) | BP LG ×4 | — | MRG-S On | ▲ |
| R-M4 | EC-SPD | Grid Speed ≥ 22 m/s | — | Sound On · PB `overspeed` | Sound Off | ▲ |
| R-M5 | EC-PWL | Stored Power ≤ 20% (AND) | BAT ×2 | H2E On · Sound On · PB `low` | — | ▲ |
| R-M6 | EC-PWH | Stored Power ≥ 90% (AND) | BAT ×2 | H2E Off | — | ◆ |
| R-M7 | EC-CHG | Connector Connected | CON-C | BAT ×2 Recharge | BAT ×2 Auto | ▲ |
| R-M8 | EC-DMG | Block Integrity ≤ 80% (any) | Wheels, MRG-S, spine | Handbrake On · PB `fault` | — | ▲ |
| R-M9 | EC-SEAT | Cockpit Occupied | Cab seat | PB `manual` | PB `auto` | ◆ |
| TB-DROP | TB | Started by PB only | — | MRG-S Off | — | ◆▲ |
| TB-DM | TB | 5 s, re-armed by PB (v2 autopilot) | — | Handbrake On · wheels Off · BCN `MU FAULT` | — | ▲ |

**Interlock for unmerge:** R-M2 true · speed < 0.1 m/s · KN at drop height · PB in `drop`.

## Script

One PB, `MU-OS`, does three jobs: governs speed, runs the KN pick/drop sequence, and (v2) replays recorded routes with wheel overrides. It talks to CR on the `TMS.*` tags defined in the Crawler guide.

**CD schema**

```ini
[mu]
kn_ride=0          ; suspension height at ride, from T-M2
kn_merge=?         ; height that seats MRG-S on MRG-T, from T-M2
kn_lift=?          ; height that clears BP LG off ground, from T-M2
v_empty=20         ; m/s
v_loaded=10        ; m/s
[sites]
cr=GPS:CR-01:x:y:z:
s1=GPS:S1:x:y:z:
s2=GPS:S2:x:y:z:
gd=GPS:GD-01:x:y:z:
[rt.gd-cr]         ; written by `rec`, one WP per 20 m
```

**PB arguments**

`pick` · `drop` · `rec <name>` · `play <name>` · `rev <name>` · `stop` · `loaded` · `empty` · `bp_locked` · `bp_free` · `overspeed` · `low` · `fault` · `manual` · `auto`

**Pick / drop sequences** (◆ PB steps, ▲ guards)

| Step | Pick | Drop |
| --- | --- | --- |
| 1 | Straddle BP, speed < 0.1 m/s, handbrake | Over pad or BAY, speed < 0.1 m/s, handbrake |
| 2 | Height → `kn_merge`; wait R-M1 merged | Height → `kn_merge`; BP LG ×4 touch ground |
| 3 | BP LG ×4 Unlock | BP LG ×4 Lock; wait R-M2 |
| 4 | Height → `kn_lift`; speed limit `v_loaded` | Start TB-DROP (MRG-S Off) |
| 5 | IGC `TMS.SWAP` as needed | Height → `kn_ride`; speed limit `v_empty`; MRG-S On after clear |

**Autopilot (v2)**

- Pure pursuit over RT WPs; lookahead 15 m; steer override ∝ heading error, clamped.
- Propulsion override from a PI loop on speed toward `v_loaded` / `v_empty`.
- Last 30 m: 2 m/s; last 2 m by hand (v2) or by beacon + camera raycast (v3).

**IGC**

| Tag | Direction | Payload | MU action |
| --- | --- | --- | --- |
| `TMS.POD` | CR → MU | `FULL;A;<slot GPS>;<t>` | Queue job; reply ACK |
| `TMS.ACK` | MU → CR | `ENROUTE;A;<eta s>` | — |
| `TMS.SWAP` | MU → CR | `ARRIVED;A` | Wait for RELEASED |
| `TMS.SWAP` | CR → MU | `RELEASED;A` | Pick F from slot A |
| `TMS.SWAP` | MU → CR | `PLACED;A` | E docked in slot A |
| `TMS.STAT` | MU → broadcast, 10 s | `<mode>;<pod id>;<t>;<BAT%>` | — |

## Commissioning

KN travel (T-M2) gates the whole concept; run it first on a bare frame before building anything else.

**Tests**

| # | Question | Method | Result |
| --- | --- | --- | --- |
| T-M2 | Does KN travel seat MRG-S on MRG-T and then lift BP LG clear? | Bare frame + empty BP, flat ground | Open → else D-M3 pistons |
| T-M3 | Merge pull-in tolerance (lateral, yaw)? | Offset trials | Open → alignment aids |
| T-M4 | Merged BP LG lock/unlock from MU; EC sees them? | Merged trial | Open |
| T-M1 | Load ladder L0 to L3 | Drivetrain table | Open |
| T-M5 | BAY / FP connector mates after drop (± m)? | GD bay mock-up | Open |
| T-M6 | Handbrake and wheels Off available to EC/TB slots? | Toolbar check | Open |
| T-M7 | 4-wheel-steer turning circle, loaded | Cone course | Open → site layout |
| T-M8 | PB propulsion and steering overrides on L 5×5 | PB echo | Open (v2) |

**Build order**

- [ ] Bare frame: spine, 2 side beams, 4 portal posts, 6 wheels, batteries, cockpit
- [ ] Pod sled for 1 BP (Saddle section spec)
- [ ] T-M2, T-M3, T-M4 on flat ground
- [ ] Wire R-M1 to R-M9, TB-DROP, TB-DM; force each guard
- [ ] Load `MU-OS`; hand-driven `pick` / `drop` cycle × 5
- [ ] Load ladder L0 → L3 (T-M1)
- [ ] GD bay mock-up: T-M5
- [ ] Site swap with live CR (Routes section), then `rec` the GD ↔ CR route

## Open decisions

| # | Decision | Options | Blocked by |
| --- | --- | --- | --- |
| D-M1 | Site swap choreography | 2-slot shuffle via S1/S2 (current) · add FP-C to CR (swap = 1 drop + 1 pick) | CR stern width; changes Crawler C7 |
| D-M2 | Wheel count | 6 × 5×5 · 8 × 5×5 | T-M1 |
| D-M3 | Lift method | KN · 2 short spine pistons | T-M2 |
| D-M4 | GD unload | CON-F into BAY connector (current) · floor connectors on BP long-side ports | GD design |
| D-M5 | Crew | Cab · RC block + cameras only (v2) | Autopilot maturity |
| D-M6 | Cross-repair | Weld LZ on MU repairs CR, and CR repairs MU | ToolCore own-grid limit |

## Sources

- [Wheel Suspension 5x5](https://spaceengineers.wiki.gg/wiki/Wheel_Suspension_5x5) · [Battery](https://spaceengineers.wiki.gg/wiki/Battery) · [Event Controller](https://spaceengineers.wiki.gg/wiki/Event_Controller)
- [Bulk Cargo Containers](https://spaceengineers.wiki.gg/wiki/Bulk_Cargo_Containers) · [Economy 2 Update & Pack](https://www.spaceengineersgame.com/space-engineers-economy-2/)
- Crawler Build Guide (this subsystem's shared key, BP standard and `TMS.*` IGC tags)
