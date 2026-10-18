# Project map · Floating Rig / RIG//OS

Living map: systems, subsystems, open questions, dependencies, build order.
Updated as decisions are made during brainstorm sessions.

Last updated: 2026-10-17

---

## Status summary

### Decided

| # | Decision | Date | Source |
| --- | --- | --- | --- |
| 1 | Universe: RSS (moving planets) + Real Stars + Real Orbits | 2026-10-16 | CHANGELOG |
| 2 | C2 amended: Defense Shields on Rig (only exception) | 2026-10-16 | CHANGELOG |
| 3 | C3 amended: all PB scripts custom | 2026-10-16 | CHANGELOG |
| 4 | TMS: Simple Laser Multitools head, vanilla wheels | 2026-10-16 | CHANGELOG |
| 5 | NPCs: MES + vanilla-block encounter packs | 2026-10-16 | CHANGELOG |
| 6 | Fleet frozen at `fleet-freeze`, merged into Rig project | 2026-10-16 | CHANGELOG |
| 7 | Scope: terrestrial ops first (Orca → Mule → ground base) | 2026-10-16 | CHANGELOG |
| 8 | IGC: `;`-separated payloads; tags `FLT.<ch>.*`, `RIG.<ch>.*`, `TMS.*` | 2026-10-16 | CHANGELOG |
| 9 | Surface GPS stable in RSS zones (no planet-relative nav for TMS) | 2026-10-16 | CHANGELOG |
| 10 | Lore: flavour only (boot screen, callsigns); C6 practical industrial wins | 2026-10-16 | 03-decisions |
| 11 | No orbital tender v1; OM fly home through T sectors | 2026-10-16 | 03-decisions |
| 12 | Miner class only for first slice; survey/interceptor next; cargo dropped | 2026-10-16 | 03-decisions |
| 13 | Orca chassis A (flatbed, 2×3 bays), uncrewed, open truss frame | 2026-10-16 | CHANGELOG |
| 14 | Swap-body containers dropped; built-in cargo, ore via docked connectors | 2026-10-16 | CHANGELOG |
| 15 | Crawler crane dropped; second arm = articulated hose | 2026-10-16 | CHANGELOG |
| 16 | Build order: Crawler → Mule → ground base → Rig (per D9 answer) | 2026-10-16 | 03-decisions |
| 17 | Home planet: RSS Earth, 1 g baseline | 2026-10-17 | CHANGELOG |
| 18 | Crawler shelved; Orca + drones is phase 1 miner | 2026-10-17 | CHANGELOG |
| 19 | v1 loop: Mules from day one; Orca stays on station | 2026-10-17 | CHANGELOG |
| 20 | Orca power: 6 small reactors, H2 assist, 2 batteries standby, solar conditional | 2026-10-17 | CHANGELOG |
| 21 | Orca propulsion: lateral baseline + optional tilt-transit | 2026-10-17 | CHANGELOG |
| 22 | Orca drone support: bay recharge, repair turrets, U store, parts store | 2026-10-17 | CHANGELOG |
| 23 | Orca drone complement: 4 Remoras (P1/S1, P3/S3) + 2 support (P2 Kestrel, S2 Albatross) | 2026-10-17 | CHANGELOG |
| 24 | Drone stone policy: Retain & offload (no Remora ejectors; all stone to base) | 2026-10-17 | CHANGELOG |
| 25 | Orca outriggers: Cantilevered A-frame truss with diagonal beams | 2026-10-17 | CHANGELOG |
| 26 | Orca Mule Pad: Flush Heavy Armour blast deck (7×7 B, r8–r14) + 1 B solar buffer | 2026-10-17 | CHANGELOG |
| 27 | C5 amended: C5 removed as hard constraint; rotors/hinges/pistons permitted | 2026-10-17 | CHANGELOG |
| 28 | Orca bay docking: Folding Mechanical Guide Jaws (twin hinges @ L5 + sensor trigger) | 2026-10-17 | CHANGELOG |
| 29 | Orca bay jaw tips: Magnetic Plates (Auto-Lock clamp at L5) | 2026-10-17 | CHANGELOG |
| 30 | Orca Power Core: Inline Stack at X = 0 (armour at X ±1; reactors/batteries in center line) | 2026-10-17 | CHANGELOG |
| 31 | Orca ventral anchors: Twin Magnetic Plates (r3 & r15 @ L1) for voxel locking | 2026-10-17 | CHANGELOG |
| 32 | Orca repair turrets: Reduced to 2 (P2 & S2 @ r9·L4); L4 Inner Frame service pipe | 2026-10-17 | CHANGELOG |
| 33 | Inventory multiplier: 3× Inventory (canon setting, lift margins cap mass not volume) | 2026-10-17 | CHANGELOG |
| 34 | Comms & safety heartbeat: 1.0 s beat / 5.0 s timeout (native HOME/PREDOCK fallback) | 2026-10-17 | CHANGELOG |
| 35 | Mule chassis & hold: 1 Large Cargo (3×3×3 B) in 7×7 B hull (4 Flat Atmo, ~650 t ore) | 2026-10-17 | CHANGELOG |
| 36 | Mule power source: Pure Electric (4 × Large Batteries, 12 MWh, 48 MW max) | 2026-10-17 | CHANGELOG |
| 37 | Standardized Mule landing: Pad-Mounted Gantry Cradle (twin hinges + mag clamps) | 2026-10-17 | CHANGELOG |
| 38 | Ground Base & Refinery: Integrated Gantry & Construction Yard (1 Mule pad + 4 Ref/4 Asm + Rig weld yard) | 2026-10-17 | CHANGELOG |
| 39 | Rig construction method: Linear Extruder Ring (fixed 8×7 ring + horizontal piston feed) | 2026-10-17 | CHANGELOG |
| 40 | Target Rig mass: 25 kt Heavy Armored Fortress (245 MN hover @ 73% T%, 638 MW hover power) | 2026-10-17 | CHANGELOG |
| 41 | Ground Base C2 facility: Fleet Operations Hub (elevated bridge, video wall, multi-fleet dispatch) | 2026-10-17 | CHANGELOG |
| 42 | Heavy wheeled excavator: MASTODON-01 (10× 5×5 wheels, twin 3-DOF laser tusks, 3× L Cargo, roof Mule pad) | 2026-10-17 | CHANGELOG |
| 43 | Bulk land train: CATERPILLAR-01 (Tractor locomotive + 2–3 hopper cars, 2-axis drawbar hinge pass-through) | 2026-10-17 | CHANGELOG |
| 44 | Standardized Mule landing: Pad-Mounted Gantry Cradle (twin hinges + mag clamps) | 2026-10-17 | CHANGELOG |
| 45 | Ground Base & Refinery: Integrated Gantry & Construction Yard (1 Mule pad + 4 Ref/4 Asm + Rig weld yard) | 2026-10-17 | CHANGELOG |
| 46 | Rig construction method: Linear Extruder Ring (fixed 8×7 ring + horizontal piston feed) | 2026-10-17 | CHANGELOG |
| 47 | Target Rig mass: 25 kt Heavy Armored Fortress (245 MN hover @ 73% T%, 638 MW hover power) | 2026-10-17 | CHANGELOG |
| 48 | Ground Base C2 facility: Fleet Operations Hub (elevated bridge, video wall, multi-fleet dispatch) | 2026-10-17 | CHANGELOG |
| 49 | Heavy wheeled excavator: MASTODON-01 (spiritual successor to Crawler, 10 wheels, twin 3-DOF tusks) | 2026-10-17 | CHANGELOG |
| 50 | Bulk land train: CATERPILLAR-01 (42 B consist, 2-axis drawbar, high-speed 90 s bulk terminal dump) | 2026-10-17 | CHANGELOG |
| 51 | Ground Base dual logistics interfaces & 3-operator C2 console (Mule pad + Cat dock + 3 desks) | 2026-10-17 | CHANGELOG |
| 52 | Dual-tier logistics architecture (Air tier rare/fast + Ground tier bulk/mass) | 2026-10-17 | CHANGELOG |
| 53 | Prospector scout drone specification: KESTREL-01 (7×9×5 sb, ore detector, T/W 6.8, belly dock) | 2026-10-17 | CHANGELOG |
| 54 | Utility and field-repair craft specification: ALBATROSS-01 (9×11×5 sb, ship welder, Medium cargo, 640 kN lift) | 2026-10-17 | CHANGELOG |
| 55 | Mastodon MKI expeditionary mining locomotive & mobile FOB (segmented 2-car train, 45 m articulated laser boom, outriggers) | 2026-10-18 | CHANGELOG |
| 56 | Mastodon MKI Genesis Seed & RIG//OS-Lite Mothership architecture (Dual PB, 3-operator bridge, automated macros, C2 handover) | 2026-10-18 | CHANGELOG |
| 57 | Mastodon MKI 5-Point Immersive Automation Suite (Auto-level jacks, boarding ladder, horizon tool leveling, tailings spreader, anti-jackknife) | 2026-10-18 | CHANGELOG |

### Open (§10 + mining concept + merge)



| # | Question | Blocking | Notes |
| --- | --- | --- | --- |
| O2 | Survival or creative (play mode) | Build order | C7 says test in creative first |
| O4 | Drone classes → bay size → arm/pylon length | Pylon geometry (S1–S6) | Drone ≤ 5 B; bay W = drone L + 4 B, max 9 B |
| O5 | MFG home (P-NW + P-SE proposed, or HUB) | Bay allocation | |
| O6 | Bay allocation (§5 board is a proposal) | Pylon build | |
| O7 | Deck sections: stacked by PRJ or delivered by tug | S7–S9 design | |
| O8 | H5 and H6 deck chapters | S0 build | Not written yet |
| O9 | Drone status message format | RIG//OS FleetOps, relay chain | IGC payload + beat interval |
| O10 | Relay chain (antenna ranges) | FleetOps STALE detection | Atmosphere → surface → orbit |

---

## Systems and subsystems

```
FLOATING RIG
 ├─ Structure
 │   ├─ HUB (H1–H6)
 │   │   ├─ H1 Command ·· MC, comms, PB rack, panels
 │   │   ├─ H2 Habitat ·· medical, quarters, farms
 │   │   ├─ H3 Gas & utility ·· GEN, tank farm, core shop
 │   │   ├─ H4 Power ·· R×3, BAT, H2E, WLD ring
 │   │   ├─ H5 Junction ·· pylon sockets, gyros, SPK   [chapter not written]
 │   │   └─ H6 Lift ·· 10A + 6H                        [chapter not written]
 │   ├─ Pylons P×6 (S1–S6) ·· arm + bays + keel + cross-head
 │   └─ Sections S0 (base) + S7–S9 (decks H3, H2, H1)
 │
 ├─ RIG//OS
 │   ├─ [CMD] PB Rig OS ·· lift, modes, anchor, heartbeat
 │   ├─ [MFG] PB Inventory ·· quotas, sorting, ASM queues
 │   ├─ [LOG] PB Dispatcher ·· jobs, bays, launch/land queue
 │   └─ [CMD] PB Displays ·· dashboards on LCD surfaces
 │
 ├─ Fleet (deferred: orbital + combat)
 │   ├─ PM planetary miners → MNR bays
 │   ├─ OM orbital miners → ICE bays
 │   ├─ INT interceptors → cross-head bays
 │   └─ PKT pickets → HUB roof
 │
 └─ Defense Shields (only mod block on Rig)

TERRESTRIAL MINING (phase 1 scope)
 ├─ Orca (uncrewed carrier, 2×3 belly bays, 17×19×7 B)
 │   ├─ Remora vertical miners ×4 (P1/S1, P3/S3; top-dock, retain stone)
 │   ├─ Support craft bays ×2 (P2 Kestrel scout, S2 Albatross utility)
 │   └─ Spine hold → 1 Large Cargo (r10–r12) + roof MP blast deck (r8–r14)
 ├─ Mule (flying cargo ferry, 7×7 B square hull)
 │   ├─ 1 Large Cargo (3×3×3 B), 4 Large Flat Atmo (~650 t ore capacity)
 │   └─ Docks Orca roof MP ↔ Ground Base refinery terminal
 ├─ Mastodon (segmented expeditionary mining train FOB: Tractor/Excavator + Utility Carriage, 45 m boom)
 ├─ Caterpillar (heavy bulk land train, 5×5 wheeled locomotive + modular hopper cars)
 └─ Ground base (Integrated Gantry, 4 Ref / 4 Asm, C2 Hub, Rig Extruder Shipyard)

CODE (frozen at fleet-freeze)
 ├─ Shared/ (MDK2 mixin: config, block cache, IGC, heartbeat, state machine, LCD)
 ├─ Drone.Miner/ (ex-Fleet miner, 66,853 chars, slices 1–3)
 └─ 498 NUnit tests, C# 6
```

---

## Dependencies

```
                    ┌──────────────┐
                    │ ✓ Earth 1 g  │
                    └──┬───────┬──┘
                       │       │
              ┌────────▼──┐ ┌─▼──────────┐
              │ Rig lift  │ │ Orca lift  │
              │(✓ 25 kt)  │ │(✓reactors) │
              └────┬──────┘ └──┬─────────┘
                   │           │
         ┌─────────▼───┐   ┌──▼──────────────┐
         │ O4 drone    │   │ ✓ 4 Remora + 2  │
         │ class/size  │   │ ✓ Retain stone  │
         └──────┬──────┘   └──┬───────────────┘
         ┌──────▼─────────────▼──┐
         │ O6 bay allocation     │
         │ O5 MFG home           │
         └───────────┬──────────┘
                     │
         ┌───────────▼──────────┐
         │ Pylon geometry       │
         │ (S1–S6 build)        │
         └──────────────────────┘

  ✓ Mules from day one ──► Mule design (parallel w/ Orca) ──► ✓ Integrated Ground Base
```

---

## Build order (decided, per D9)

| Step | What | Depends on | Status |
| --- | --- | --- | --- |
| 0 | Creative test world (RIG Test) | — | exists (13 mods) |
| 1 | Orca design + build | O1, O13, O14 | instructions complete (lego-instructions-v1.0.md Ch 1) |
| 2 | Mining drone (Remora REM-01) | Orca bays | instructions complete (lego-instructions-v1.0.md Ch 2) |
| 3 | v1 loop: mine → offload → ferry | O12 | architecture locked |
| 4 | Mule design + build | O12 | instructions complete (lego-instructions-v1.0.md Ch 3) |
| 4a | Heavy excavator (Mastodon) | Mule | instructions complete (lego-instructions-v1.0.md Ch 10) |
| 4b | Bulk land train (Caterpillar) | Mastodon | instructions complete (lego-instructions-v1.0.md Ch 11) |
| 4c | Prospector scout drone (Kestrel) | Orca P2 / Mastodon | instructions complete (lego-instructions-v1.0.md Ch 12) |
| 4d | Utility & repair craft (Albatross)| Orca S2 / Base | instructions complete (lego-instructions-v1.0.md Ch 13) |
| 5 | Ground base & extruder yard | Mule | instructions complete (lego-instructions-v1.0.md Ch 4-5) |
| 6 | S0 HUB base (H4–H6) | O1, O3, O8 | blueprints mapped (lego-instructions-v1.0.md Ch 6) |
| 7 | S1–S6 Pylons | O4, O5, O6, S0 | blueprints mapped (lego-instructions-v1.0.md Ch 7) |
| 8 | S7–S9 Decks | O7, S0 | blueprints mapped (lego-instructions-v1.0.md Ch 8) |
| 9 | RIG//OS | O9, O10, S0 | hardware contract locked (lego-instructions-v1.0.md Ch 9) |

**Note:** D9 answer says Crawler first, but CHANGELOG also shows the concept changed to Orca + drones. Build order as listed above follows the latest concept. Open question O11 may reintroduce the Crawler.

