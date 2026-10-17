# Mule + Standard Cargo Container · sprue 1.0 review

2026-10-16 · blueprints `Mule and Container Sprue 1_0` and `Standard Cargo Container` (Steam Cloud) · `tools/bp-inspect.py` + block-level parse · † = computed or assumed, verify in RIG Test.

## What the blueprint holds

One large grid, **133 blocks, three parts welded together**:

| Part | Blocks | Mass | Notes |
| --- | ---: | ---: | --- |
| Build stand | 43 | ≈ 103 t (+ 2 small gas tanks, 6.4 t) | 9 Structural Platform Conveyors (37.5 m floor) + scaffold columns and rails |
| Container | 19 | **22.1 t** | Bulk Cargo Container C (6 × 3 × 3) + 4 Inset Connectors + armour caps |
| Mule | 71 | **≈ 129 t** | see below |

The standalone `Standard Cargo Container` blueprint is the stand + the Bulk C + 4 **Small** Inset Connectors (no caps).

## Mule spec sheet

| Item | Value |
| --- | --- |
| Lift | 4 × Large Flat Atmospheric Thruster (D) = **10.4 MN** sea-level air |
| Lateral | 4 × Atmospheric Thruster (large grid), **all facing the same way**: 2,592 kN pushing +x only · **0 for −x, 0 fore/aft, 0 down** |
| Gyros | 2 × large (33.6 MN·m each = 67.2 MN·m) |
| Power | 6 × Warfare Battery: **18 MWh, 72 MW** (lift at full = 26.8 MW) |
| Hook | 2 × Connector facing down, 7 blocks apart; 2 downward cameras between them |
| Control | Remote Control, PB (empty), Compact Antenna, 4 Automaton Sensors, top-mounted camera |
| Landing gear | none |
| T/W dry | **8.2** |

## Payload (1 g, sea-level air, return margin 1.3)

| Quantity | Value |
| --- | --- |
| Max total mass at T/W 1.3 | 816 t |
| → container + ore | 687 t |
| → ore | **≈ 664 t** = **29 %** of a Bulk C at 1×, **≈ 10 %** at 3× |
| Hover endurance† dry / at max payload | ≈ 5.5 h / ≈ 52 min |

**The Mule's lift, not the container's volume, sets the fill.** The Orca and the terminals stop filling at the Mule's tonnage.

## Findings

| # | Severity | Finding | Fix |
| --- | --- | --- | --- |
| F1 | **must** | All 4 lateral thrusters push +x. The Mule can't brake or hold against drift in −x or fore/aft. Neither dampeners nor the autopilot can stop it | Thrust on 4 horizontal axes (at least 1 small atmo per face), placed symmetric about the CoM |
| F2 | **must** | Lift centre is **8.75 m from the hook point** (lift centre x −8, hook x −4.5). At max payload the tip torque is ≈ 65 MN·m vs 67 MN·m of gyro: no control authority left. It looks like a mirrored pair of flat thrusters is missing | Put the lift centre over the hook point (mirror the pair, or re-centre all 4) |
| F3 | **must** | Mule, container and stand are one welded grid. A container can only be carried and dropped if it is a separate grid | Build and save the Mule and the container as separate grids/blueprints |
| F4 | high | Connector port mismatch†: the Mule hooks with **Connector**; the standalone container has **Small Inset Connectors** (small port); the sprue's container has normal Inset Connectors. They probably don't mate | One port standard for the hook (Connector ↔ Inset Connector) |
| F5 | high | Carrying 600 t+ on two connectors is springy† (connectors lock with a spring, not rigidly) | Test 200 / 400 / 600 t swings; if it wobbles, add a merge-block pair for the carry and keep connectors for power |
| F6 | med | The container has no landing gear, beacon or battery. A dropped container slides on slopes, is unpowered and has no ID | Add 4 landing gear or magnetic plates at the bottom corners, + beacon + small battery, named `[BP-nn]` |
| F7 | med | The top connectors sit at the container's upper edge, not on the Bulk C's conveyor port†. They hold and power the container but may not pass ore | Use the end connectors for ore transfer; check in game which ports are conveyor-linked |
| F8 | low | Mule has no landing gear | 2–4 gear so it can park without a container |
| F9 | low | No names or groups; PB empty | `[MU-01] …` naming when scripting starts |

## Verdict

**A good start, with the right concept:**
- A flying Mule with a swap-body standard container.
- A single cargo interface for the Orca, the terminals and later the Rig.
- Ample power, and downward cameras for hook alignment.

**Before the first flight test, fix F1–F3** (thrust axes, lift over the hook, separate grids). **Then settle F4–F5** (port standard, carry rigidity).
