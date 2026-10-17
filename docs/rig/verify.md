# Verify in game (†)

Items marked † in any project doc. Checked = verified in the RIG Test world or by source reading.

| # | Item | Source | Checked |
| --- | --- | --- | --- |
| V1 | Thrust acts through CoM (1 P OFF, no roll) | CLAUDE(RIG).md §11 | |
| V2 | A severed P keeps its dampeners | CLAUDE(RIG).md §11 | |
| V3 | H2 drain time at full H; large H thrust value (≈7.2 MN) | CLAUDE(RIG).md §5 | |
| V4 | Damper drift vs drone routes | CLAUDE(RIG).md §11 | |
| V5 | Consumers pull through SRT | CLAUDE(RIG).md §11 | |
| V6 | MRG pull-in tolerance for P docking | CLAUDE(RIG).md §11 | |
| V7 | Antenna/IGC range for the relay chain | CLAUDE(RIG).md §11 | |
| V8 | Real Orbits: gravity falloff is inverse-square | CHANGELOG 2026-10-16 | |
| V9 | RSS: world GPS stable inside a surface zone | CHANGELOG 2026-10-16 (confirmed by source read) | ✓ |
| V10 | Ore at 0.37 L/kg | xu1-analysis.md | |
| V11 | Small-grid connector locks to large-grid connector (port size) | xu1-analysis.md | |
| V12 | Atmo thrust 0 at planetary influence ≤ 0.3, full at 1.0 | xu1-analysis.md | |
| V13 | XU-1 hover T/W at the Orca's working altitude | xu1-analysis.md | |
| V14 | XU-1 cargo volume at 3× inventory | xu1-analysis.md | |
| V15 | Dock rise into a moving bay | xu1-analysis.md | |
| V16 | Drone-to-drone clearance in adjacent holes | xu1-analysis.md | |
| V17 | Orca lift: 4 large atmo ≈ 25.9 MN | orca-instructions.md §3 | |
| V18 | Small reactor output 15 MW | Orca Part 1 §3 | |
| V19 | Large H2 thruster 7.2 MN | Orca Part 1 §3, §7 | |
| V20 | Small atmo (large grid) ≈ 0.65 MN | Orca Part 1 §8 | |
| V21 | Large atmo thruster footprint (3×3×5 B) | Orca Part 1 §7 | |
| V22 | Large solar panel footprint, mass, output | Orca Part 1 §4 | |
| V23 | Sorter whitelist blocks items both ways | Orca Part 1 §4 | |
| V24 | H2 passes connectors and sorters | Orca Part 1 §4 | |
| V25 | Multitool turret range; weld-only and ownership settings | Orca Part 1 §6 | |
| V26 | Atmo effectiveness vs altitude | Orca Part 1 §3 | |
| V27 | Mule flame damage on MP roof | Orca Part 1 §4 | |
| V28 | Sensor field extent; own-grid detection | Orca Part 1 §9 | |
| V29 | Ore detector range, large grid | Orca Part 1 §9 | |
| V30 | Large battery output and capacity | Orca Part 1 §4 | |
| V31 | EC events: thrust %, reactor, connector | Orca Part 1 §10 | |
| V32 | Timer restart while running (Stop then Start) | Orca Part 1 §10 | |
| V33 | Large grid ship welder active weld radius (≈1.75 m / adjacent block reach) | Ground Base Rig Yard | |
| V34 | Small drill ring cutting diameter (9×9 sb ring vs 11 sb hull clearance) | remora-instructions.md §2 | |
| V35 | Small-grid large atmo flame reach (4 sb clear gap under nozzle) | remora-instructions.md §3 | |
| V37 | Large Flat Atmospheric Thruster large-grid sea-level thrust (2.6 MN rating & power) | mule-instructions.md §2 | |
| V38 | Ore transfer drain rate between large-grid containers via single connector (s/650 t) | mule-instructions.md §8 | |
| V39 | Hover power draw at 25 kt mass (≈638 MW† on 52 Large Atmo at 73% T%) | Rig lift budget | |
| V40 | N−1 reactor loss (600 MW) hover deficit (≈38 MW deficit covered by battery/H2) | Rig power budget | |
| V41 | Simple Laser Multitool mining mode power draw (≈2.0 MW each†) & extraction rate | Mastodon Tusk Booms | |
| V42 | Land train 2-axis drawbar hinge torque (30 MN·m†) vs 2,500 t trailer drag on 15° slope | Caterpillar Train Hitch | |
| V43 | Large-grid collector intake throughput (5× collectors vs 2,500 t bulk ore in < 90 s†) | Ground Base Caterpillar Dock | |

