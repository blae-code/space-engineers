# XU-1 Automated Mining Drone · analysis for the Orca swarm

2026-10-16 · source: Workshop 2705006131 (pro100tv; 30,740 subs) · read with `tools/bp-inspect.py` + block-level parse · † = computed or assumed, verify in game.

## Spec sheet

| Item | Value |
| --- | --- |
| Grid | small, 1 grid, **no subgrids** (3 piston tops at the dock end are decorative; no bases) |
| Size | 11 × 11 × 22 small blocks = **5.5 × 5.5 × 11 m**, flies **nose-down** (long axis vertical) |
| Blocks | 484 (321 armour/structure) |
| Dry mass | **62.3 t** |
| Script | **PAM** (Path Auto Miner), 82,343 chars in the PB: no licence, so it is replaced, never reused |
| Drills | 8 small drills in a ring, all facing down (cut-out r 1.3 m each) |
| Lift | **4 large atmo thrusters**, symmetric, thrust line through the CoM (CoM offset ≈ 0.01 m) · 2,304 kN at sea-level air |
| Other thrust | 24 small atmo, 4 per face: 576 kN per lateral axis; **0 kN pushing down** (gravity does it: planet-only) |
| Atmo limit | thrust 0 at planetary influence ≤ 0.3, full at 1.0: no flight in thin air |
| Power | 6 Battery (1 MWh, 4 MW each) + 4 Small Battery → **6.2 MWh, 24.8 MW** |
| Cargo | Large + Medium small-grid container = **19,000 L at 1×**† (57,000 L at 3×) |
| Docking | **"Connector UP" at the top**, facing up · camera "DOCK" beside it |
| Stone | 8 small connectors as ejectors in a ring + 1 "STONE EJECT" connector + 16 sorters |
| Control | Remote Control (autopilot reference), 5 gyros, sensor `[PAM]`, ore detector, antenna, LCD, 4 cameras |
| Landing gear | **none** |

## Lift vs load (1 g, sea-level air)

| Load | Total mass | Weight | T/W (4 large atmo) |
| --- | ---: | ---: | ---: |
| Dry | 62.3 t | 611 kN | **3.77** |
| Full at 1× (51 t ore) | 113.7 t | 1,115 kN | **2.07** |
| Full at 3× (154 t ore) | 216.4 t | 2,123 kN | **1.09**: barely hovers, no reserve |
| Fleet `MinLiftMargin` 1.3 cap | 180.7 t | 1,772 kN | 1.30 → returns at ≈ 118 t ore (77 % of 3×) |

Ore at 0.37 L/kg. Air thinner than sea level cuts lift linearly between influence 1.0 and 0.3†.

**Endurance†** (thruster power ∝ throttle): hover dry ≈ 2.5 MW → ≈ 2.4 h · hover at the 1.3 cap ≈ 7.4 MW → ≈ 50 min.

## Strengths
- **Vertical, symmetric, single-grid.** No joints to clang, thrust through the CoM, simple to control.
- **Top dock connector.** It rises straight up into a bay, which is exactly the Orca's belly-bay geometry.
- **Strong lift margin at 1× inventory** (T/W 2 when full).
- **Compact:** 5.5 m square footprint, so 4–6 fit an Orca and adjacent holes stay clear.
- **Proven in play:** 30k subscriptions and a tutorial video.

## Weaknesses
- **PAM:** unlicensed, minified, 82k chars, single-drone. It cannot share a job: there are no leases, no shared bays and no carrier beacon.
- **3× inventory overloads it:** a full hold leaves T/W ≈ 1.09. It needs a mass-based return, not a %.
- **Atmo-only, planet-only:** no downward thrust, no flight above ≈ 0.3 influence. Fine under an Orca; it can never go to orbit.
- **Stone ejection under a swarm:** 9 ejectors throw rock around drones working next to each other, adding floating objects and sim-speed cost, and the rock lands back in the next hole.
- **Conveyor sprawl:** 66 conveyor parts + 16 sorters, mostly for the ejector ring.
- **No landing gear:** a dead drone cannot park; it falls.
- **No native fallback:** every return decision lives in PAM. That breaks S1.

## Adapting it for the Orca swarm

| # | Change | Why | Reuses |
| --- | --- | --- | --- |
| A1 | Replace PAM with **`Drone.Miner`** (ex-Fleet miner, 66,853 chars) | Shared job by leases, shared bays, moving-carrier docking, mass-based returns | Slices 1–3 |
| A2 | Return on **lift margin / tonnes**, not cargo % | Safe at 3× inventory | `MinLiftMargin`, `ReturnTriggers` |
| A3 | Keep the **top connector**; the Orca bay sits above the drone | Vertical rise into the bay; dock path stored local to the bay connector | Slice 2 |
| A4 | Rename to `[ID] Fn n`, groups per drone | RIG naming; Fleet's `[FM]` tag becomes the drone's own tag | — |
| A5 | **Native fallback:** EC rungs for battery low, damage and PB heartbeat lost → RC autopilot to the Orca's beacon, or hold | S1: safety never depends on the PB | D6 proposal |
| A6 | **Stone policy** (open decision): eject far from the swarm, or keep it and offload stone to the Orca | Ejecting under a swarm fouls the next holes | `Eject` setting |
| A7 | Optional: 2 landing gear on the dock end | A dead drone can park on the Orca or the ground | — |
| A8 | Re-derive lift at the Orca's working altitude and air density† | Atmo lift falls with altitude | Inspector |

**Verify in creative (RIG Test):**
- Hover T/W at the Orca's working height.
- Cargo volume at 3×.
- Dock rise into a moving bay.
- Drone-to-drone clearance in adjacent holes.
