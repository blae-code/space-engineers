# Space Engineers 1 · Floating Rig & Logistics Fleet
## Master Lego-Style Build Manual & RIG//OS Interface Contract
### Version 1.0 · Canon Engineering Specification · Earth 1 g Baseline

Last updated: 2026-10-18
Authority: Blae Decisions 1-70 (Canon) · `CLAUDE(RIG).md` · `Part 1 & 2 Handoff`

---

## Document Overview & Dual-Purpose Standard

This document is the authoritative, versioned master construction guide for the Space Engineers 1 **Floating Rig** project and its complete **Terrestrial Logistics & Surface Mining Suite**.

It is engineered simultaneously for two distinct audiences:
1. **Human Builders**: Layer-by-layer visual ASCII blueprints, exact coordinate axes (X, Y, Z), pre-step parts trays, in-game paint palettes, pre-enclosure inspection gates, visual milestones, critical conveyor port orientations, flame damage clearances, physical checkpoints, and commissioning sequences.
2. **AI Programmers (Claude Code / RIG//OS)**: Exact terminal block names (`ORC ...`, `REM ...`, `MUL ...`, `MAS ...`, `CAT ...`, `GND ...`, `RIG ...`), functional group taxonomies, conveyor circuit topologies, failover Event Controller logic, timer safety watchdogs, and IGC message dictionaries.

---

## Master Table of Contents

* [Section 0 · Project Taxonomy & Naming Conventions](#section-0-project-taxonomy-naming-conventions)
* [Volume I · Terrestrial Mining Fleet (Air Tier · Fast / Rare)](#volume-i-terrestrial-mining-fleet-air-tier-fast-rare)
  * [Chapter 1 · ORCA-01 Heavy Mining Carrier (17 × 19 × 7 B)](#chapter-1-orca-01-heavy-mining-carrier)
  * [Chapter 2 · REMORA-01 Vertical Shaft Driller (11 × 11 sb)](#chapter-2-remora-01-vertical-shaft-driller)
  * [Chapter 3 · MULE-01 Heavy Freight Shuttle (7 × 7 B)](#chapter-3-mule-01-heavy-freight-shuttle)
  * [Chapter 12 · KESTREL-01 Prospector Scout Drone (7 × 9 × 5 sb)](#chapter-12-kestrel-01-prospector-scout-drone-kes-01)
  * [Chapter 13 · ALBATROSS-01 Utility & Field-Repair Craft (9 × 11 × 5 sb)](#chapter-13-albatross-01-utility-field-repair-craft-alb-01)
* [Volume II · Surface Infrastructure & Industrial Core](#volume-ii-surface-infrastructure-industrial-core)
  * [Chapter 4 · Ground Base, Refinery Complex & Fleet Command Center (C2)](#chapter-4-ground-base-refinery-complex-fleet-command-center-c2)
  * [Chapter 5 · Rig Linear Extruder Ring Shipyard](#chapter-5-rig-linear-extruder-ring-shipyard)
* [Volume V · Terrestrial Surface Heavy Extraction (Ground Tier · Bulk / Mass)](#volume-v-terrestrial-surface-heavy-extraction-ground-tier-bulk-mass)
  * [Chapter 10 · MASTODON-01 Expeditionary Mining Train & Mobile FOB (MAS-01)](#chapter-10-mastodon-01-expeditionary-mining-train-mobile-fob-mas-01)
  * [Chapter 11 · CATERPILLAR-01 Land Train (CAT-01)](#chapter-11-caterpillar-01-land-train-cat-01)
* [Volume III · Floating Rig Structure (25 kt Fortress)](#volume-iii-floating-rig-structure-25-kt-fortress)
  * [Chapter 6 · Section S0: HUB Base (H4-H6)](#chapter-6-section-s0-hub-base-h4-h6)
  * [Chapter 7 · Sections S1-S6: Pylon Arms & Modular Bays](#chapter-7-sections-s1-s6-pylon-arms-modular-bays)
  * [Chapter 8 · Sections S7-S9: Upper Decks (H1-H3)](#chapter-8-sections-s7-s9-upper-decks-h1-h3)
* [Volume IV · Hardware-to-Scripting Interface Contract](#volume-iv-hardware-to-scripting-interface-contract)
  * [Chapter 9 · RIG//OS Hardware Abstraction & IGC Dictionary](#chapter-9-rigos-hardware-abstraction-igc-dictionary)

---

## Section 0 · Project Taxonomy & Naming Conventions

### 0.1 Grid Standards & Coordinate Conventions
* **Large Grid (B):** 1 block = 2.5 m (Orca, Mule, Mastodon, Caterpillar, Ground Base, Floating Rig).
* **Small Grid (sb):** 1 block = 0.5 m (Remora, Kestrel, Albatross).
* **Coordinate Triad:**
  * **X (Beam):** Port is negative (−), Starboard is positive (+), Centerline is 0.
  * **Y (Vertical):** Belly/Deck foundation is L1, advancing upward (+).
  * **Z (Length):** Aft/Stern is negative (−), Bow/Forward is positive (+), Midline is 0.

### 0.2 System Prefix Registry
| Prefix | Grid | Description | Subsystems / Sections |
|---|---|---|---|
| `ORC` | Large | ORCA-01 Heavy Mining Carrier | `CC` control, `PC` power, `HD` hold, `AS` aft service, `MP` mule pad, `BR` bay rows, `OR` outriggers |
| `REM` | Small | REMORA-01 Vertical Driller | `CR` crown, `AV` avionics, `PW` power, `LF` lift, `CL` collector, `DH` drill head |
| `MUL` | Large | MULE-01 Freight Shuttle | `HOLD` cargo core, `LIFT` flat atmos, `BAT` power, `DOCK` belly port, `CLAMP` rub rails |
| `KES` | Small | KESTREL-01 Prospector Drone | `AV` avionics, `DET` ore detector, `PW` power, `DOCK` belly port |
| `ALB` | Small | ALBATROSS-01 Utility Craft | `TOOL` welder head, `HOLD` components, `PW` power, `DOCK` belly port |
| `MAS` | Large | MASTODON-01 Mining Train & FOB | `T` lead locomotive, `U` utility carriage, `BOOM` 5-DOF arm, `STAB` outriggers, `MP` mule pad, `DUMP` stern bumper |
| `CAT` | Large | CATERPILLAR-01 Land Train | `LOCO` tractor, `HITCH` drawbar, `CAR` modular hoppers, `UNLOAD` stern drain |
| `GND` | Large | Ground Base & C2 Terminal | `PAD` gantry cradle, `CAT` unload dock, `REF` refinery array, `ASM` assembler core, `C2` bridge |
| `EXT` | Large | Rig Linear Extruder Ring | `RING` welder wall, `HYD` feed piston, `PRJ` projector head, `RAMP` launch way |
| `RIG` | Large | Floating Rig Main Station | `HUB` central core (H1-H6), `P` pylons (S1-S6), `SEC` deck slices (S7-S9) |

### 0.3 Universal Zero-GC Scripting & Minification Rules (PB Sandbox)
* **Banned APIs:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`, `System.Globalization`, and **LINQ**.
* **Zero Allocations in Loops:** No `new`, no string concatenation/interpolation in `Update1/10/100` or `Main()`. Numbers formatted through `StringBuilder` cache; enum names mapped to static string tables.
* **Minifier Immunity:** `minify=full` renames all identifiers. Scripts must never use `enum.ToString()`, `Enum.Parse`, or `nameof()`. CustomData keys, Storage keys, and IGC tags must be string literals.
* **Strict Grid Scoping:** Every terminal query filtered with `b => b.CubeGrid == Me.CubeGrid`.

---

# Volume I · Terrestrial Mining Fleet (Air Tier · Fast / Rare)

## Chapter 1 · ORCA-01 Heavy Mining Carrier
### Visual Build Manual & System Engineering Specification
#### 12-Pillar Architectural Standard · Parity with MASTODON-01 Benchmark

```
========================================================================================
SYSTEM SPECIFICATION: ORCA-01 HEAVY MINING CARRIER
ROLE: Terrestrial Drone Mothership & Atmospheric Ore Bulk Transshipment Hub
GRID: Large Grid (2.5 m / block)  ·  DIMENSIONS: 17 × 19 × 7 B (42.5 × 47.5 × 17.5 m)
DRY MASS: ≈ 764.0 t  ·  LADEN MASS: ≈ 2,166.0 t (6 docked drones + 1,200 t ore buffer)
TOTAL PCU: ≈ 4,500 PCU  ·  POWER PLANT: 90.0 MW Nuclear + 6.0 MWh / 48.0 MW Surge Battery
PROPULSION: 4× Large Atmo (25.92 MN) + 28× Small Atmo (4.48 MN) + 4× Large H2 (28.8 MN Boost)
CARGO CAPACITY: 1× 3×3×3 Large Cargo Hold (1,200 t) + 2× Small Cargo Service Stores
DRONE BAYS: 6× Internal Guided Bays (4× Remora Drillers, 1× Kestrel, 1× Albatross)
DORSAL FLIGHT DECK: 7×7 B Heavy Armour Mule Transshipment Pad with Gantry Cradle Clamps
AUTHORITY: Blae Decisions 1-70 (Canon)  ·  STANDARD: 12-Pillar Mastodon Benchmark
========================================================================================
```

---

## 0 · Reference Grid & Spatial Datum

### 0.1 Spatial Datum & Hull Envelope

The Orca airframe occupies a **17 × 19 × 7 B** bounding box (42.5 × 47.5 × 17.5 m) structured around an open-truss industrial catamaran architecture with a central spine and lateral outriggers:

```
Axes:    X = Beam (Port -X, Starboard +X)
         Y = Elevation (L1 Belly, L7 Dorsal Roof Deck)
         Z = Longitudinal Length (Bow +Z, Stern -Z)

Origin:  Datum [X0, Y1, Z0] located at bottom keel centerline at Row r9 (midships).

Stations: Rows r0 (Bow Prow) to r18 (Stern Transom) at 2.5 m pitch (19 stations total).
Levels:   L1 (Ventral Keel, 0.0 m) ─── L2 (Lower Gallery, 2.5 m) ─── L3 (Power Vault, 5.0 m)
          ─── L4 (Mid Deck, 7.5 m) ─── L5 (Catwalk Spine, 10.0 m) ─── L6 (Deck Sub-Floor, 12.5 m)
          ─── L7 (Roof Deck / Mule Pad, 15.0 m).
Beam:     Port Tip (X -8, -20.0 m) ── Port Bay (X -5, -12.5 m) ── Port Spine (X -1.5, -3.75 m)
          ── Centerline (X0, 0.0 m) ── Stbd Spine (X +1.5, +3.75 m) ── Stbd Bay (X +5, +12.5 m)
          ── Stbd Tip (X +8, +20.0 m).
```

---

### 0.2 Standard 10-Slot SE Paint Palette

The Orca utilizes the standardized Space Engineers 10-slot industrial paint palette. Color blocking reinforces spatial orientation, identifies functional hazards, and visually separates structural load paths from drone clearance envelopes.

| Slot | Finish | Hex Code | RGB | Functional Zone & System Usage |
|---|---|---|---|---|
| **01** | Battered Armor | `#22252A` | `34, 37, 42` | Primary Keel Spine, Cross-Frames, Longitudinal Longerons |
| **02** | Clean Armor | `#D8A824` | `216, 168, 36` | High-Pressure Ore Trunk, Sorters, Manifold Transfer Lines |
| **03** | Hazard Stripe | `#E5A00D` | `229, 160, 13` | Folding Bay Guide Jaws, Waist Mag Clamps, Moving Hinges |
| **04** | Carbon / Worn | `#151719` | `21, 23, 25` | Dorsal Mule Landing Deck Blast Floor (7×7 B Heavy Armour) |
| **05** | Heavy Battered | `#B83227` | `184, 50, 39` | Mule Deck Hazard Boundary Perimeter (1 B No-Solar Buffer) |
| **06** | Heavy Armor | `#D2D7DF` | `210, 215, 223`| Power Core Vault, Reactor Citadel, Battery Isolation Cells |
| **07** | Clean Armor | `#1B4965` | `27, 73, 101` | Command Core, Pressurized Bridge, Avionics Citadel |
| **08** | Industrial Worn| `#5C677D` | `92, 103, 125`| Outrigger Pylons, Cantilever A-Frames, Nacelle Housings |
| **09** | Neon / Emissive| `#00E5FF` | `0, 229, 255`  | Bay Centerline Striping, Drone Alignment Optical Markers |
| **10** | Matte Rubber | `#0A0B0D` | `10, 11, 13`   | Ventral Landing Skids, Rub Rails, Docking Bumpers |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

The side profile livery emphasizes the brutalist industrial architecture of the carrier. High-visibility safety accents outline the drone bay mouths and the dorsal flight deck, while charcoal structural members provide visual mass.

```
       r0      r2      r4      r6      r8     r10     r12     r14     r16     r18
L7 ────[CC BLUE]───────[CHARCOAL FRAME]─[RED HAZ]─[BLACK BLAST DECK]─[RED HAZ]─[AS BLUE]── L7
       │ CC Visor  │                  │  Mule Gantry Clamps       │          │ AS Comms │
L6 ────[CC BLUE]───┼──────────────────┴───────────────────────────┴──────────┼──────────┤ L6
       │           │                                                         │          │
L5 ────┼───────────┼──────[HAZARD STRIPE GUIDE JAWS (P1-P3)]─────────────────┼──────────┤ L5
       │ Bridge    │                                                         │          │
L4 ────[CC WHITE]──┼──────[CHARCOAL TRUSS CORRIDOR]──────────────────────────┼──────────┤ L4
       │ Core      │                                                         │          │
L3 ────[CC WHITE]──┼──────[WHITE REACTOR CITADEL]──[CHARCOAL ORE HOLD]───────┼──────────┤ L3
       │           │                                                         │          │
L2 ────┴───────────┼─────────────────────────────────────────────────────────┴──────────┤ L2
L1 ────────────────[CHARCOAL KEEL]─[RUB RAILS]──────────────────[VENTRAL ANCHORS]─────── L1
       r0      r2      r4      r6      r8     r10     r12     r14     r16     r18
       ▲                                                                      ▲
     PROW                                                                  TRANSOM
```

---

### 0.3 Master Multi-Level Blueprints

#### Master Plan View (Roof, Y = L7)

```
        X:  -8    -5    -2   -1.5  0  +1.5  +2    +5    +8
            ·     ·     ·     ║    ·    ║    ·     ·     ·
    r0  ────┬─────┬─────┬═════╬════╬════┬─────┬─────┬────  BOW (Prow)
            │     │     │     ║ CC ║    │     │     │
    r2  ····│·····│·····│·····║····║····│·····│·····│····
            │     │     │     ║    ║    │     │     │
    r3  ────┼─────┼─────┼═════╬════╬════┼─────┼─────┼────
            │     │     │     ║    ║    │     │     │
            │  P1 │     │     ║ PC ║    │     │ S1  │
    r6  ────┼─────┼─────┼═════╬════╬════┼─────┼─────┼────
            │     │     │     ║    ║    │     │     │
            │  P2 │     │     ║    ║    │     │ S2  │
            │     │     │ ┌───╨────╨───┐│     │     │
    r8  ····│·····│·····│ │  MULE PAD  ││·····│·····│····
            │     │     │ │  7×7 B     ││     │     │
    r11 ────┼─────┼─────┼─┤  [ORC MP]  ├┼─────┼─────┼────  Midships (Datum Z0)
            │     │     │ │  BLAST     ││     │     │
    r14 ····│·····│·····│ │  DECK      ││·····│·····│····
            │     │     │ └───╥────╥───┘│     │     │
            │  P3 │     │     ║ HD ║    │     │ S3  │
    r16 ────┼─────┼─────┼═════╬════╬════┼─────┼─────┼────
            │     │     │     ║ AS ║    │     │     │
    r18 ────┴─────┴─────┴═════╬════╬════┴─────┴─────┴────  STERN (Transom)
                              ·    ·

        ║═══║  Central spine (3 B wide: X -1.5 to +1.5)
        │   │  Drone bay openings (5×5 B clear well: X -8 to -2 and X +2 to +8)
        [MP]   Mule Flight Deck (7×7 B Heavy Armour: r8 to r14, X -3 to +3)
```

#### Master Side Elevation (X = 0 Centerline, Looking from Starboard)

```
        r0    r3      r6      r9      r12     r15   r18
    L7  ┌─────┬───────┬───────┬───────┬───────┬─────┐  L7 Roof Deck
        │ CC  │  PC   │       │  MP   │       │ AS  │
    L6  │     │       │       │ [CON] │       │     │
        │     │       │       │       │       │     │
    L5  │     │  R R  │  ┌────┼────┐  │       │     │  L5 Catwalk Spine
        │     │  R R  │  │ HOLD    │  │       │ H2  │
    L4  │     │  R R  │  │ 3×3×3 B │  │       │ TANK│
        │     │  BAT  │  └────┼────┘  │       │     │
    L3  │ RC  │  GYRO │       │       │       │     │
        ├─────┴───────┴───────┴───────┴───────┴─────┤
    L1  └───────────────────────────────────────────┘  L1 Ventral Keel
        ▲                                           ▲
      OR-1P/S                                     OR-3P/S
      [LARGE ATMO]                                [LARGE ATMO]
```

#### Master Cross-Section (at Row r10, Looking Toward Bow)

```
                  Port Bay (P2)                     Starboard Bay (S2)
        X: -8         -5    -2  -1.5  0  +1.5  +2         +5         +8
    L7  ┌───────────┬──────╥────┬────╥──────┬───────────┐  L7 Roof / Mule Deck
        │           │      ║    │    ║      │           │
    L6  │  Bay P2   │      ║  MP Conn║      │  Bay S2   │
        │  (5×5 B   │Inner ║  [CON]  ║Inner │  (5×5 B   │
    L5  │   Clear)  │Frame ║  Catwalk║Frame │   Clear)  │  L5 Catwalk Spine
        │           │      ║  Tunnel ║      │           │
    L4  │   [JAWS]  │      ║  HD Sorter     │   [JAWS]  │  L4 Guide Jaws
        │           │      ║ ┌──────┐║      │           │
    L3  │           │      ║ │ HOLD │║      │           │
        │   CON↓    │      ║ │3×3×3B│║      │    CON↓   │
    L2  │           │      ║ └──────┘║      │           │
    L1  └───────────┴──────╨────┴────╨──────┴───────────┘  L1 Ventral Keel
               ↕ Open Bottom                   ↕ Open Bottom
              to Ground Voxel                 to Ground Voxel
```

---

### 0.4B 3/4 Isometric Perspective Wireframes

#### Forward-Starboard 3/4 Isometric Perspective Wireframe

```
                     +Y (Up)
                      │   +Z (Bow)
                      │  /
                      │ /
        [BOW VISOR]   │/
             ▲       ┌───┐
            ╱ ╲      │   │
           ╱   ╲─────┘   └─────────────┐ [DORSAL MULE DECK L7]
          ╱  CC ╲                      │    ┌──────────────┐
         ┌───────┐  SPINE LONGERONS    │    │ [MP CON]     │
         │ Prow  │═════════════════════╪════│  7×7 B       │
         └───┬───┘                     │    │  BLAST DECK  │
            ╱│                         │    └──────────────┘
           ╱ │ [OR-1S ATMO NACELLE]    │           │
          ╱  │    ┌────────┐           │           │
         ╱   │    │ 3×3×5B │           │           │
        ▼    │    │ LIFT   │           │           │
      -X     │    └────────┘           │    [BAY S2 JAWS]
    (Port)   │        │                │         ▲
             │        │                │        ╱ ╲
             │        ▼                │       ╱   ╲
             │    [A-FRAME TRUSS]      │      ┌─────┐
             └─────────────────────────┴──────┤ S2  ├───────► +X (Starboard)
                                              └─────┘
                                           DRONE CLEARANCE WELL
```

#### Aft-Port 3/4 Isometric Perspective Wireframe

```
                                      +Y (Up)
                                       │
            [DORSAL MULE DECK L7]      │
                ┌──────────────┐       │
                │ 7×7 B DECK   │       │
                └──────┬───────┘       │
                       │               │     [STERN TRANSOM r18]
       [BAY P3 JAWS]   │               │           ▲
             ▲         │   SPINE TRUSS │          ╱ ╲
            ╱ ╲        │═══════════════╪═════════╱   ╲
           ╱   ╲───────┘               │        ┌─────┐
          ┌─────┐                      │        │ AS  │
          │ P3  │                      │        │ H2  │
          └─────┘                      │        └─────┘
             │                         │           │
             │ [OR-3P ATMO NACELLE]    │           │
             │    ┌────────┐           │           │
             │    │ 3×3×5B │           │           │
             │    │ LIFT   │           │           │
             │    └────────┘           │           │
             │        │                │           │
             │        ▼                │           │
             │    [A-FRAME TRUSS]      │           │
             └─────────────────────────┴───────────┘
            /                                       ╲
           /                                         ╲
          /                                           ▼
      -Z (Stern)                                  -X (Port)
```

---

### 0.4C Transverse Hull Station Profiles

#### Station 1: Bow Command Bridge & Control Core (r1, Z +20.0 m)

```
        X: -2.5    -1.5      X0      +1.5    +2.5
    L7  ┌───────────┬─────────────────┬───────────┐  L7 Roof
        │           │  [ORC CC ANT]   │           │
    L6  │  HEAVY    │  Cockpit Canopy │  HEAVY    │  L6 Upper Canopy
        │  ARMOR    │  [PANORAMIC]    │  ARMOR    │
    L5  │  FRAME    ├─────────────────┤  FRAME    │  L5 Bridge Deck (Walkable)
        │           │ ORC Helm Seats  │           │
    L4  ├───────────┼─────────────────┼───────────┤  L4 Avionics Floor
        │           │ ORC CC PB & RCs │           │
    L3  │  HEAVY    ├─────────────────┤  HEAVY    │  L3 Life Support
        │  ARMOR    │ ULSS Air Vent   │  ARMOR    │
    L2  │  VAULT    ├─────────────────┤  VAULT    │  L2 Ventral Entry
        │           │ Access Airlock  │           │
    L1  └───────────┴─────────────────┴───────────┘  L1 Ventral Keel
```

#### Station 2: Forward Drone Bays P1/S1 & Power Core (r4, Z +12.5 m)

```
                  Port Bay P1                       Starboard Bay S1
        X: -8         -5        -1.5  X0  +1.5       +5         +8
    L7  ┌───────────┬────────────╥────┬────╥───────────┬───────────┐  L7 Roof
        │           │ [CON↓]     ║Serv Trunk║    [CON↓]│           │
    L6  │  BAY P1   │ (Bay Drop) ║Conv L6  ║(Bay Drop) │  BAY S1   │  L6 Trunk
        │  CLEAR    │            ║Sm Reactor║          │  CLEAR    │
    L5  │  LANE     │            ║30 MW (1) ║          │  LANE     │  L5 Catwalk Spine
        │           │ [MAG JAWS] ║Sm Reactor║[MAG JAWS]│           │
    L4  │           │ ◄────────► ║30 MW (2) ║◄────────►│           │  L4 Guide Jaws
        │           │            ║Lrg Battery          │           │
    L3  │           │            ║3 MWh (1) ║          │           │  L3 Power Vault
        │           │            ║Lrg Gyros ║          │           │
    L2  │           │            ║(Center)  ║          │           │  L2 Lower Spine
    L1  └───────────┴────────────╨────┴────╨───────────┴───────────┘  L1 Belly
```

#### Station 3: Central Ore Hold & Mule Flight Deck (r10, Z -2.5 m)

```
                  Port Bay P2                       Starboard Bay S2
        X: -8         -5        -1.5  X0  +1.5       +5         +8
    L7  ┌───────────┬────────────╥────┬────╥───────────┬───────────┐  L7 Mule Deck
        │           │ [CON↓]     ║[ORC MP] ║    [CON↓] │           │
    L6  │  BAY P2   │ (Kestrel)  ║MP Intake║(Albatross)│  BAY S2   │  L6 Sub-Floor
        │  CLEAR    │            ║HD Drain ║           │  CLEAR    │
    L5  │  LANE     │            ║Catwalk  ║           │  LANE     │  L5 Catwalk
        │           │ [MAG JAWS] ║┌───────┐║ [MAG JAWS]│           │
    L4  │           │ ◄────────► ║│ORC HD │║ ◄────────►│           │  L4 Hold Core
        │           │            ║│OreHold│║           │           │
    L3  │ [REPAIR]  │            ║│3×3×3 B│║  [REPAIR] │           │  L3 Hold Belly
        │ (Multitool)            ║└───────┘║ (Multitool)           │
    L2  │           │            ║Gate Sorter          │           │  L2 Sorter Well
    L1  └───────────┴────────────╨────┴────╨───────────┴───────────┘  L1 Belly
```

#### Station 4: Aft Drone Bays P3/S3 & Stern Outriggers (r16, Z -17.5 m)

```
        Outrigger OR-3P   Port Bay P3        Spine       Stbd Bay S3   Outrigger OR-3S
    X: -12    -10     -8      -5        -1.5  X0  +1.5    +5      +8      +10    +12
    L7 ┌───────┐      ┌────────┬─────────╥────┬────╥───────┬───────┐      ┌───────┐
       │[ATMO↓]│      │        │ [CON↓]  ║H2 TANK  ║ [CON↓]│       │      │[ATMO↓]│
    L6 │3×3×5 B│══════╡ BAY P3 │(Bay Drop║Service  ║(Bay Dr│ BAY S3╞══════│3×3×5 B│
    L5 │ 6.48  │ A-FRM│ CLEAR  │         ║Catwalk  ║       │ CLEAR │ A-FRM│ 6.48  │
    L4 │  MN   │ TRUSS│ LANE   │[MAG JAWS║Parts    ║[MAG JA│ LANE  │ TRUSS│  MN   │
    L3 │ LIFT  │      │        │◄───────►║Store    ║◄─────►│       │      │ LIFT  │
    L2 └───────┘══════╡        │         ║Conveyor ║       │       ╞══════└───────┘
    L1   ▼ GEAR       └────────┴─────────╨────┴────╨───────┴───────┘        ▼ GEAR
```

---

### 0.4 Master Conveyor Routing Schematic & Physical Inspection Ports

The Orca features two completely segregated conveyor circuits ensuring zero cross-contamination between raw ores and refined reactor fuels:

1. **Primary Ore Extraction Circuit:** Drone bays (P1-P3, S1-S3) ──► Overhead Collection Trunk (L6) ──► `ORC HD Gate` (whitelist: Ore, Stone) ──► `ORC Ore Hold` (3×3×3 Large Cargo) ──► `ORC HD Drain` (whitelist: Ore) ──► `ORC MP Conn` (Mule Pad).
2. **Service & Fuel Replenishment Circuit:** `ORC MP Intake` (whitelist: U-Ingots, Components, Ice) ──► `ORC U Store` (Small Cargo at r5·L4) ──► 6× Small Nuclear Reactors + 1× `ORC AS H2` Tank (r17) + 2× Laser Multitool Turrets (P2/S2).

```
                             OVERHEAD DRONE BAY CEILING TRUNK (L6)
           Bay P1 [CON]       Bay P2 [CON] (KES)       Bay P3 [CON]
                │                      │                      │
                └───────────────►──────┴──────◄───────────────┘
                                       │ Port Collection Trunk (X -5, L6)
                                       ▼
                         ┌───────────────────────────┐
                         │ Bay Sorter Cross-Feed (r6)│
                         └─────────────┬─────────────┘
                                       │
     ┌─────────────────────────────────▼─────────────────────────────────┐
     │ `ORC HD Gate` (Conveyor Sorter · Whitelist: Ore, Stone · Drain ON)│
     └─────────────────────────────────┬─────────────────────────────────┘
                                       │
     ┌─────────────────────────────────▼─────────────────────────────────┐
     │ `ORC Ore Hold` (Large Cargo 3×3×3 B · r10-r12, X0, L3-L5)         │
     │ Capacity: 1,200 t Ore / 421,875 L                                 │
     └─────────────────────────────────┬─────────────────────────────────┘
                                       │
     ┌─────────────────────────────────▼─────────────────────────────────┐
     │ `ORC HD Drain` (Conveyor Sorter · Whitelist: Ore · Drain ON)      │
     └─────────────────────────────────┬─────────────────────────────────┘
                                       │
     ┌─────────────────────────────────▼─────────────────────────────────┐
     │ `ORC MP Conn` (Dorsal Mule Pad Connector · Flush L7, r11, X0)     │
     └───────────────────────────────────────────────────────────────────┘
                                       ▲
                                       │
     ┌─────────────────────────────────┴─────────────────────────────────┐
     │ `ORC MP Intake` (Conveyor Sorter · Whitelist: U, Components, Ice) │
     └─────────────────────────────────┬─────────────────────────────────┘
                                       │
     ┌─────────────────────────────────▼─────────────────────────────────┐
     │ `ORC U Store` (Small Cargo Container · r5, X0, L4)                │
     └─────────────────────────────────┬─────────────────────────────────┘
                                       │
               ┌───────────────────────┴───────────────────────┐
               ▼                                               ▼
     [6× Small Nuclear Reactors]                  [1× Large Hydrogen Tank]
     (r3-r6, X0, L4-L5 · 90 MW)                   (r17, X0, L4-L6 · 2.5 ML)
               │                                               │
               ▼                                               ▼
     [2× Laser Multitool Turrets]                 [4× Large Hydrogen Thrusters]
     (P2/S2, X ±2, r9, L4)                        (OR-2P/S Outriggers · 28.8 MN)
```

#### Physical Manual Inventory Inspection Ports

In strict adherence to ergonomic engineering, all storage vessels, sorters, and reactors feature physical access ports reachable from internal walkways:

| Device | Block Name | Cell Datum | Port Type | Access Location & Clearance |
|---|---|---|---|---|
| **Large Cargo Hold** | `ORC Ore Hold` | `r10, X0, L3` | Large Square Port | Ventral Inspection Gallery (L2 Catwalk, facing aft) |
| **Uranium Fuel Store** | `ORC U Store` | `r5, X0, L4` | Small Square Port | Spine Catwalk Tunnel (L5, starboard floor hatch) |
| **Aft Spare Parts Store**| `ORC Parts Store`| `r17, X0, L3` | Small Square Port | Stern Engineering Gallery (L3 walkway) |
| **Ore Gate Sorter** | `ORC HD Gate` | `r8, X0, L5` | Manual Interface | Catwalk Spine (L5, port wall inspection access) |
| **Ore Drain Sorter** | `ORC HD Drain` | `r12, X0, L6` | Manual Interface | Mule Deck Sub-Floor Crawlway (L6, facing bow) |
| **Mule Intake Sorter** | `ORC MP Intake` | `r10, X0, L6` | Manual Interface | Mule Deck Sub-Floor Crawlway (L6, facing stern) |

---

### 0.5 Master Electrical Single-Line Schematic & 5 Operational Energy Modes

```
                        PRIMARY POWER GENERATION & STORAGE
          ┌───────────────────┬───────────────────┬───────────────────┐
          │ Small Reactor 1..3│ Small Reactor 4..6│ Large Battery 1..2│
          │ (45.0 MW Nuclear) │ (45.0 MW Nuclear) │ (6.0 MWh / 48 MW) │
          └─────────┬─────────┴─────────┬─────────┴─────────┬─────────┘
                    │                   │                   │
                    └───────────────────┼───────────────────┘
                                        ▼
                                PRIMARY POWER BUS
         ┌──────────────────────────────┼──────────────────────────────┐
         │                              │                              │
         ▼                              ▼                              ▼
   LIFT PROPULSION              ATTITUDE & MANEUVER            DRONE & AUX LOADS
   4× Large Atmo Thrusters      28× Small Atmo Thrusters       6× Drone Recharging
   (67.2 MW @ 100% Burst)       (18.2 MW @ 100% Burst)         (24.0 MW Max Float)
   [Normal Hover = 52.8 MW]     [Attitude Gyros = 0.4 MW]      [Avionics/Lights = 0.6 MW]
```

#### 5 Operational Energy Management Modes

| Mode | Operational State | Power Generation | Power Draw | Margin / Deficit | Battery Policy |
|---|---|---|---|---|---|
| **Mode 1** | **Silent Loiter / Survey** | 15.0 MW (1 Reactor) | 12.4 MW (Hover trim + Cams) | +2.6 MW (Net charging) | Semi-Auto / Float |
| **Mode 2** | **Transit & Relocation** | 90.0 MW (6 Reactors) | 68.2 MW (Full cruise thrust) | +21.8 MW (Charging) | Auto (Dampening) |
| **Mode 3** | **Active Mining Deployment**| 90.0 MW (6 Reactors) | 86.8 MW (Hover + 6× Drones) | +3.2 MW (Sustained) | Auto |
| **Mode 4** | **Mule Cargo Transshipment**| 90.0 MW (6 Reactors) | 74.5 MW (Hover + High Drain)| +15.5 MW (Stable) | Auto |
| **Mode 5** | **Emergency Ascent / Storm**| 138.0 MW (Reactors+Bat)| 114.2 MW (Atmo+H2+Max Climb)| +23.8 MW (Surge Peak) | Full Discharge |

---

### 0.6 Master Pre-Flight Bill of Materials (BOM) & Physics Budget

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Structural Spine & Hull** | Beam Blocks, Heavy Armor Bulkheads, Blast Plates | 482 | 410.0 t | 1,450 |
| **Power Core & Vault** | 6× Small Nuclear Reactors, 2× Large Batteries | 8 | 42.0 t | 220 |
| **Primary Lift Propulsion** | 4× Large Atmospheric Thrusters (3×3×5 B Nacelles) | 4 | 132.0 t | 400 |
| **Attitude & Emergency H2** | 28× Small Atmo, 4× Large H2 Thrusters, 1× H2 Tank | 33 | 86.0 t | 830 |
| **Cargo & Conveyor Routing**| 1× Large Cargo (3×3×3), 2× Small Cargo, 3× Sorters | 58 | 48.0 t | 290 |
| **Drone Bays & Mechanisms** | 6× Connectors, 14× Hinges, 16× Mag Plates, 2× MT Turrets | 38 | 34.0 t | 1,000 |
| **Avionics, C2 & Logic** | 2× RC, 1× PB, 2× Timers, 4× EC, 14× Cams, 7× Sensors | 30 | 12.0 t | 310 |
| **TOTAL (DRY AIRFRAME)** | **ORCA-01 Heavy Mining Carrier (Complete)** | **653** | **≈ 764.0 t** | **4,500** |

#### Earth 1 g Lift Physics & Thrust-to-Weight (T/W) Budget

- **Dry Mass:** 764.0 t (Gravity Load: 7.49 MN).
- **Fully Laden Mass:** 2,166.0 t (Carrier 764 t + 4× Remora 160 t + Kestrel 18 t + Albatross 24 t + 1,200 t Ore Buffer). Gravity Load: 21.25 MN.
- **Downward Atmospheric Lift Thrust:**
  - 4× Large Atmospheric Thrusters: 4 × 6.48 MN = 25.92 MN.
  - 28× Small Atmospheric Thrusters (Attitude trim assist): 28 × 0.16 MN = 4.48 MN.
  - Total Atmospheric Vertical Lift: **30.40 MN**.
- **Earth 1 g T/W Ratio (Dry):** $30.40	ext{ MN} / 7.49	ext{ MN} = \mathbf{4.06}$ (Exceptional agile response).
- **Earth 1 g T/W Ratio (Fully Laden):** $30.40	ext{ MN} / 21.25	ext{ MN} = \mathbf{1.43}$ (Sustained hover with 43% safety margin).
- **Emergency Hydrogen Boost Thrust:**
  - 4× Large Hydrogen Thrusters: 4 × 7.20 MN = 28.80 MN.
  - Total Combined Lift Thrust: $30.40 + 28.80 = \mathbf{59.20	ext{ MN}}$.
  - Emergency Laden T/W Ratio: $59.20	ext{ MN} / 21.25	ext{ MN} = \mathbf{2.79}$ (Rapid storm ascent).
- **Lateral Drift Deceleration:** 28× Small Atmo thrusters provide 5.86 m/s²† laden deceleration.

---

### 0.7 Human Builder Guide & Universal Spatial Locator HUD

To avoid spatial disorientation inside a 17 × 19 × 7 B skeletal frame, every build step begins with a standardized spatial locator HUD:

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: <Name>  ·  Grid Zone: <Rows r0-r18>  ·  Level: <L1-L7>         │
│ [VIEWPORT] Looking: <Direction>  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

### 0.8 Human Circulation & Life Support Network

1. **Pressurized Command Core (Bridge):** Located at r0-r2, Levels L4-L5. Features panoramic glass canopy, 2× pilot control stations (`ORC Helm Pilot`, `ORC Helm DroneOps`), 1× Survival Kit, 1× Armory Locker, and 1× depressurizing crew airlock at r2·L4.
2. **Pressurized Catwalk Spine:** A continuous 1 B wide, sealed inspection corridor running along Level L5 from the Bridge Aft Door (r2) through rows r3-r16. Allows the flight engineer to physically inspect the power vault, cargo sorters, and drone bay locking pins during flight.
3. **Vertical Ladderwells:**
   - **Bow Ladderwell (r2, X0):** Connects L1 ventral airlock, L4 bridge, and L7 dorsal roof deck.
   - **Stern Ladderwell (r16, X0):** Connects L1 ventral inspection hatch, L5 catwalk spine, and L7 flight deck.
4. **Universal Life Support (ULSS):** 1× Large O2 Tank (200,000 L at r2·L3), 2× Air Vents (`ORC Vent Bridge` and `ORC Vent Spine`), sealed with hermetic breach doors at Station r2 and Station r16.

---

## Phase 1 · Keel Spine & Ventral Structure

### Step 1 · Belly Frame Rails & Ventral Anchors (L1)

Build the primary structural spine belly as three longitudinal Z-axis rails at Level L1, installing two magnetic plates for planet voxel anchoring.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Keel Spine  ·  Grid Zone: Rows r0-r18  ·  Level: L1 Belly       │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 1 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 20  │ 16.0 t / 20   │ Z-axis keel rails (r0-r18) & cross-ties  │
│ Beam Block Junction │ 16  │ 12.8 t / 16   │ Corner and post tie nodes (X ±1.5)       │
│ Magnetic Plate      │ 2   │  4.8 t / 50   │ Face DOWN at X0, r3 and r15 (flush L1)   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L1 (belly)

    X:  -1.5    0    +1.5
         ■──────■──────■   r0 (Bow Prow)
         │      │      │
         ■──[ANCHOR 1]─■   r3 (under PC Power Vault)
         │      │      │
         ■      ·      ■   r6
         │      │      │
         ■      ·      ■   r9 (Datum Z0)
         │      │      │
         ■      ·      ■   r12
         │      │      │
         ■──[ANCHOR 2]─■   r15 (under AS Stern Section)
         │      │      │
         ■──────■──────■   r18 (Stern Transom)
         └──────┴──────┘

    ■ = Beam Block (Junction / Corner)
    │ = Beam Block (Straight, Z-axis)
    ─ = Beam Block (Straight, X-axis tie beams)
    [ANCHOR 1, 2] = Magnetic Plate (facing down at X0, r3 and r15, flush at L1)
```

**Placement Sequence:**
1. Lay parallel Beam Block rails at X = -1.5 and X = +1.5 running from r0 to r18.
2. Tie rails transversely at r0, r3, r6, r9, r12, r15, r18.
3. Mount 2× Magnetic Plates (`ORC Belly Anchor Bow` at r3, `ORC Belly Anchor Stern` at r15) facing DOWN flush with L1 belly line. Group: `ORC Ventral Anchors`.

**✓ Check:** Two rigid parallel rails 3 B apart, spanning 19 B. Magnetic plates flush with keel.

---

### Step 2 · Spine Vertical Posts (L1 ──► L7)

At each junction node from Step 1, erect vertical beam posts to Level L7 (roof deck).

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Spine Posts  ·  Grid Zone: Rows r0-r18  ·  Level: L1-L7        │
│ [VIEWPORT] Looking: Starboard (+X) ·  Orientation: ▲ UP (+Y) · ◄ BOW (+Z) · STERN (-Z)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 2 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 84  │ 67.2 t / 84   │ Vertical Y-axis posts at X ±1.5 (r0-r18) │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Side elevation, X = -1.5 (Port spine wall)

    r0    r3    r6    r9    r12   r15   r18
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L7 Roof Deck
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L6 Sub-Floor
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L5 Catwalk
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L4 Mid-Height
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L3 Power Deck
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L2 Lower Gallery
    ■     ■     ■     ■     ■     ■     ■   L1 Belly (from Step 1)
```

**Placement Sequence:**
1. Erect 7 vertical posts at X = -1.5 at rows r0, r3, r6, r9, r12, r15, r18 (6 B tall each).
2. Erect 7 mirrored vertical posts at X = +1.5.

**✓ Check:** 14 vertical structural columns standing 7 B tall (17.5 m) in two parallel rows.

---

### Step 3 · Spine Roof Longerons (L7)

Tie all vertical post tops with Z-axis longerons at L7, forming the upper spine box.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Roof Longerons  ·  Grid Zone: Rows r0-r18  ·  Level: L7 Roof    │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 3 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 21  │ 16.8 t / 21   │ Z-axis longitudinal longerons at L7      │
│ Beam Block Junction │ 7   │  5.6 t / 7    │ Centerline X0 tie nodes (r0-r18)         │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L7 (roof)

    X:  -1.5    0    +1.5
         ■══════■══════■   r0
         ║      ║      ║
         ■══════■══════■   r3
         ║      ║      ║
         ■══════■══════■   r6
         ║      ║      ║
         ■══════■══════■   r9
         ║      ║      ║
         ■══════■══════■   r12
         ║      ║      ║
         ■══════■══════■   r15
         ║      ║      ║
         ■══════■══════■   r18
```

**✓ Check:** The central spine forms a rigid 3 × 19 × 7 B skeletal truss. Fully walkable at L7.

---

### Step 4 · Transverse Cross-Frames at r0, r6, r12, r18 (L1 & L7)

Extend transverse cross-beams outward from the spine (X ±1.5) to the outer frame boundary (X ±8). These divide the airframe into 6 discrete 5×5 B drone bay volumes.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bay Cross-Frames  ·  Grid Zone: r0, r6, r12, r18  ·  L1 & L7    │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 52  │ 41.6 t / 52   │ Transverse X-axis spans (X ±1.5 to ±8)   │
│ Beam Block Junction │ 16  │ 12.8 t / 16   │ Outer frame boundary nodes at X ±8       │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L1 (belly), showing transverse outrigging

    X: -8              -1.5  0  +1.5              +8
        ■════════════════■════■════■════════════════■  r0 (Bow cross-frame)
        ·                ║    ·    ║                ·
        ·  [Bay P1 Area] ║    ·    ║ [Bay S1 Area]  ·
        ·                ║    ·    ║                ·
        ■════════════════■════■════■════════════════■  r6 (Mid-1 cross-frame)
        ·                ║    ·    ║                ·
        ·  [Bay P2 Area] ║    ·    ║ [Bay S2 Area]  ·
        ·                ║    ·    ║                ·
        ■════════════════■════■════■════════════════■  r12 (Mid-2 cross-frame)
        ·                ║    ·    ║                ·
        ·  [Bay P3 Area] ║    ·    ║ [Bay S3 Area]  ·
        ·                ║    ·    ║                ·
        ■════════════════■════■════■════════════════■  r18 (Stern cross-frame)
```

**Placement Sequence:**
1. Install 6.5 B beam spans port and starboard at rows r0, r6, r12, r18 at Level L1.
2. Mirror exactly at Level L7.

**✓ Check:** 4 full-width cross-frames divide the hull into 3 major bay zones per flank.

---

### Step 5 · Outer Hull Perimeter Rails (X = ±8, L1 ──► L7)

Connect the cross-frame outer tips with longitudinal rails and vertical posts at X = ±8.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Outer Boundary Rails  ·  Grid Zone: X ±8  ·  Level: L1-L7       │
│ [VIEWPORT] Looking: Inboard (+X)  ·  Orientation: ▲ UP (+Y) · ◄ BOW (+Z) · STERN (-Z)► │
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 60  │ 48.0 t / 60   │ Outer perimeter rails (L1/L7) & posts    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Side elevation, X = -8 (Port outer frame)

    r0    r6    r12   r18
    ┌─────┬─────┬─────┐  L7 Roof
    │     │     │     │
    │     │     │     │
    │ P1  │ P2  │ P3  │
    │     │     │     │
    │     │     │     │
    └─────┴─────┴─────┘  L1 Belly
```

**✓ Check:** Hull footprint is a defined 17 × 19 × 7 B bounding skeleton. Bays open below.

---

### Step 6 · Inner Bay Guide Rails (X = ±2, L1 ──► L7)

Install the inner guide frame at X = ±2, separating the 5×5 B drone well from the central spine.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 6 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 108 │ 86.4 t / 108  │ Inner rails (L1/L7) and Y-posts (r0-r18) │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**✓ Check:** Looking down through any bay, a clear 5×5 B unobstructed vertical drop exists between X ±2 and X ±8.

---

### Step 7 · Bay Mid-Height Stabilizer Rings (L4)

Install horizontal tie beams at Level L4 inside each bay frame to anchor guide jaw hinges and sensor mounts.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 7 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 24  │ 19.2 t / 24   │ L4 horizontal perimeter ring in 6 bays   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Cross-section at r3 (Bay P1 / S1 Center), Y = L4

    X: -8         -2         +2         +8
       ■═══════════■          ■═══════════■  L4 Ring (Anchor for guide jaws)
       ║           ║          ║           ║
       ║  Bay P1   ║          ║  Bay S1   ║
       ║  5×5 B    ║          ║  5×5 B    ║
       ║  Clear    ║          ║  Clear    ║
       ■═══════════■          ■═══════════■  L1 Belly
```

---

### Pre-Enclosure Inspection Gate 1.1A: Spine & Ventral Anchor Integrity

> [!IMPORTANT]
> **POINT OF NO RETURN:** Verify all keel rails, vertical posts, and ventral magnetic plates before installing enclosed power components or conveyors.
>
> 1. [ ] Keel longerons run unbroken from r0 to r18 at Level L1 (X = ±1.5).
> 2. [ ] `ORC Belly Anchor Bow` (r3) and `ORC Belly Anchor Stern` (r15) face straight down, flush with L1.
> 3. [ ] All 14 vertical posts reach Level L7 with zero deflection.
> 4. [ ] All 6 drone bay clear openings measure exactly 5×5 B (12.5 × 12.5 m).

---

## Phase 2 · Power Core Vault & Ore Buffer

### Step 8 · Primary Power Core Vault (PC, r3-r6)

Assemble the nuclear reactor citadel and battery storage vault in the central spine between rows r3 and r6. Flank the equipment stack with Heavy Armour side walls at X = ±1.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Power Core  ·  Grid Zone: Rows r3-r6  ·  Level: L2-L6 (X0)      │
│ [VIEWPORT] Looking: Starboard (+X) ·  Orientation: ▲ UP (+Y) · ◄ BOW (+Z) · STERN (-Z)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 8 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Nuclear Reac  │ 6   │ 19.8 t / 150  │ Centerline X0: r3·L5, r4·L5, r5·L5, r6·L5│
│                     │     │               │ and r3·L4, r4·L4                         │
│ Large Battery       │ 2   │ 52.0 t / 40   │ Centerline X0: r3·L3, r4·L3              │
│ Gyroscope (Large)   │ 4   │ 49.6 t / 20   │ Centerline X0: r5·L3, r6·L3, r3·L2, r4·L2│
│ Small Cargo Cont    │ 1   │  1.2 t / 10   │ `ORC U Store` at r5·L4 (X0)              │
│ Large Conveyor Tube │ 7   │  8.4 t / 7    │ Service line at L6 and reactor feeds     │
│ Heavy Armour Block  │ 48  │ 96.0 t / 48   │ Radiation isolation vault walls at X ±1  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Side elevation at X = 0 (Centerline Power Stack)

          r3              r4              r5              r6
    ┌───────────────┬───────────────┬───────────────┬───────────────┐  L7 Roof
    │ Serv Conveyor │ Serv Conveyor │ Serv Conveyor │ Serv Conveyor │  L6 Service Line
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │ Small Reac 1  │ Small Reac 2  │ Small Reac 3  │ Small Reac 4  │  L5 Nuclear Citadel
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │ Small Reac 5  │ Small Reac 6  │  ORC U Store  │ Serv Conveyor │  L4 Fuel Deck
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │ Large Batt 1  │ Large Batt 2  │  Gyroscope 1  │  Gyroscope 2  │  L3 Energy Buffer
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │  Gyroscope 3  │  Gyroscope 4  │ Serv Conveyor │ Serv Conveyor │  L2 Gyro Deck
    └───────────────┴───────────────┴───────────────┴───────────────┘  L1 Belly
```

**Placement Sequence:**
1. Weld 4× Gyroscopes (`ORC Gyros`) on centerline at r3·L2, r4·L2, r5·L3, r6·L3.
2. Install 2× Large Batteries (`ORC Batteries`) at r3·L3 and r4·L3. Set initial state to Recharge.
3. Install `ORC U Store` (Small Cargo) at r5·L4.
4. Mount 6× Small Nuclear Reactors (`ORC Reactors`) at designated cells. Connect conveyor ports directly to the L6 service trunk.
5. Seal port and starboard faces (X = ±1) with 48× Heavy Armour Blocks to isolate radiation.

**✓ Check:** All 6 reactors and `ORC U Store` connect to the L6 service line. Vault is fully armored.

---

### Step 9 · Ore Hold & Sorter Interlock (HD, r7-r15)

Install the 3×3×3 Large Cargo container in the central spine at rows r10-r12, flanked by the intake gate and drain sorters.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Ore Buffer  ·  Grid Zone: Rows r7-r15  ·  Level: L2-L6 (X0)     │
│ [VIEWPORT] Looking: Starboard (+X) ·  Orientation: ▲ UP (+Y) · ◄ BOW (+Z) · STERN (-Z)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 9 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo Cont    │ 1   │ 21.0 t / 25   │ `ORC Ore Hold` (3×3×3 B, r10-r12, L3-L5) │
│ Conveyor Sorter     │ 2   │  4.0 t / 30   │ `ORC HD Gate` (L5), `ORC HD Drain` (L6)  │
│ Large Conveyor Tube │ 12  │ 14.4 t / 12   │ Overhead ore collection trunk at L6/L7   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Side elevation (X = 0), Ore Hold Core

    r7    r8    r9    r10   r11   r12   r13   r14   r15
    ║     ║     ║     ╔═════╤═════╗     ║     ║     ║   L7 Mule Deck
    ║     ║     ║     ║     │     ║     ║     ║     ║   L6 (HD Drain Sorter)
    ║     ║     ║     ║ ORC Ore   ║     ║     ║     ║   L5 (Catwalk Spine)
    ║     ║     ║     ║ Hold      ║     ║     ║     ║   L4 (Hold Core)
    ║     ║     ║     ║ (3×3×3 B) ║     ║     ║     ║   L3 (Ventral Inspection)
    ║     ║     ║     ╚═════╧═════╝     ║     ║     ║   L2 (HD Gate Sorter)
```

**Placement Sequence:**
1. Center `ORC Ore Hold` at r11, X0, spanning Levels L3 to L5.
2. Install `ORC HD Gate` (Conveyor Sorter) at r8·L5 facing AFT into the cargo hold (Whitelist: Ore, Stone).
3. Install `ORC HD Drain` (Conveyor Sorter) at r12·L6 facing UP toward the Mule Pad (Whitelist: Ore; Drain All = ON).

**✓ Check:** Cargo hold connects via Gate to the overhead trunk and via Drain to the dorsal deck.

---

### Step 10 · Aft Engineering & Hydrogen Reserve (AS, r16-r18)

Install the emergency hydrogen reserve and parts store in the stern engineering bay.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 10 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Hydrogen Tank │ 1   │  8.2 t / 25   │ `ORC AS H2` (Centerline r17, L4-L6)      │
│ Small Cargo Cont    │ 1   │  1.2 t / 10   │ `ORC Parts Store` (Centerline r17, L2-L3)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Placement Sequence:**
1. Mount `ORC Parts Store` at r17, X0, L2-L3, plumbed to the L4 service gallery line.
2. Mount `ORC AS H2` tank at r17, X0, L4-L6, connected to outrigger H2 branches.

---

### Pre-Enclosure Inspection Gate 1.2A: Power Vault & Sorter Piping

> [!IMPORTANT]
> **POINT OF NO RETURN:** Perform pneumatic and inventory pull tests before sealing the spine roof deck.
>
> 1. [ ] Fill `ORC U Store` with 5 kg Uranium ingots; confirm auto-flow to all 6 reactors.
> 2. [ ] Deposit test Stone into bay drop tube; verify `ORC HD Gate` allows entry into `ORC Ore Hold`.
> 3. [ ] Verify `ORC HD Drain` refuses passage of Uranium or Components.
> 4. [ ] Confirm 0.00 MW leak from batteries when set to Recharge.

---

## Phase 3 · Dorsal Mule Landing Deck & Flight Deck Systems

### Step 11 · Mule Landing Deck Blast Floor (r8-r14, L7)

Construct the reinforced 7×7 B Heavy Armour landing deck to absorb the thruster wash and landing impacts of the 650 t MULE-01 Heavy Freight Shuttle.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Mule Deck  ·  Grid Zone: Rows r8-r14  ·  Level: L7 Roof         │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armour Block  │ 49  │ 98.0 t / 49   │ 7×7 B blast pad (r8-r14, X -3 to +3)     │
│ Structural Frame    │ 30  │ 12.0 t / 30   │ Roof walkway panels outside blast pad    │
│ Large Solar Panel   │ 24  │ 19.2 t / 96   │ Roof solar array outside 1 B hazard zone │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L7 (Roof Deck)

    X: -8            -4            0            +4            +8
       ┌─────────────────────────────────────────────────────────┐  r0 (Bow)
       │  ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀         ANT         ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀  │
       │  ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀  │  r0-r6
       │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │  r7 (1 B Hazard Buffer)
       │  ☀ ☀ ☀ ☀ ░░░░  ┌─────────────────┐  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │
       │  ☀ ☀ ☀ ☀ ░░░░  │  MP BLAST DECK │  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │  r8-r14
       │  ☀ ☀ ☀ ☀ ░░░░  │  (Heavy Armour)│  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │  (X±4 buffer)
       │  ☀ ☀ ☀ ☀ ░░░░  └─────────────────┘  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │
       │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │  r15 (1 B Hazard Buffer)
       │  ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀  │  r16-r18 (Stern)
       └─────────────────────────────────────────────────────────┘

    ☀ = Large Solar Panel (facing UP)  ·  ░░ = 1 B clear safety buffer (Structural Frame)
```

**Placement Sequence:**
1. Lay 49× Heavy Armour Blocks at r8-r14, X -3 to +3, flush with Level L7.
2. Outline perimeter with Slot 05 Heavy Battered Red paint (1 B safety boundary).
3. Mount 24× Large Solar Panels (`ORC Solar`) on exterior roof flanks, ensuring zero solar panels sit within the 1 B thruster scorch perimeter.

---

### Step 12 · Gantry Cradle & Mule Docking Interlock (r11, L7)

Install the motorized clamping cradle and flush transshipment connector on the Mule Pad.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector           │ 1   │  4.0 t / 25   │ `ORC MP Conn` (facing UP at r11, X0, L7) │
│ Large Hinge         │ 2   │  4.0 t / 20   │ `ORC MP Cradle Port/Stbd` at X ±3, r11   │
│ Magnetic Plate      │ 2   │  4.8 t / 50   │ Clamping plates on hinge arms (Auto-Lock)│
│ Conveyor Sorter     │ 1   │  2.0 t / 15   │ `ORC MP Intake` (under deck at L6)       │
│ Camera              │ 1   │  0.2 t / 10   │ `ORC Cam MP` (facing aft over pad)       │
│ Spotlight           │ 2   │  0.4 t / 10   │ `ORC MP Lights` (aimed 45° upward)       │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Gantry Cradle Transverse Cross-Section (r11, Looking Toward Bow)

                       [MULE-01 HULL 7×7 B]
                                │
      Port Flank                ▼               Starboard Flank
    [CLAMP RUB RAIL]                       [CLAMP RUB RAIL]
           ▲                                      ▲
           │                                      │
      [MAG PLATE] ◄── 30° Swing Inward ──► [MAG PLATE]
           │                                      │
      [MOTOR HINGE]                          [MOTOR HINGE]
      (Mounted X -3, r11)                    (Mounted X +3, r11)
    ═══════╧══════════════════════════════════════╧═══════
                      FLUSH HEAVY ARMOUR DECK (L7)
                             [ORC MP Conn]
```

**Mechanical Settings:**
- Hinges (`ORC MP Cradle`): Braking Torque 20.0 MN·m†, Velocity 10 RPM. Limits: 0° (parked flush) to 30° (engaged).
- Mag Plates: Auto-Lock = ON.
- Connector (`ORC MP Conn`): Auto-Lock = ON.

---

### Pre-Enclosure Inspection Gate 1.3A: Mule Deck Blast Floor & Gantry Cradle

> [!IMPORTANT]
> **POINT OF NO RETURN:** Test mechanical cradle articulation and connector lock before proceeding to bridge avionics.
>
> 1. [ ] `ORC MP Conn` faces straight up, exactly flush with L7 deck line.
> 2. [ ] Cradle hinges rotate cleanly between 0° and 30° without voxel collision.
> 3. [ ] `ORC Sensor MP` detects descending grid at 20 m and triggers `ORC MP Lights`.
> 4. [ ] `ORC MP Intake` pulls Uranium into `ORC U Store` without admitting ore.

---

## Phase 4 · Control Core & Human Bridge

### Step 13 · Armored Bridge Shell (CC, r0-r2)

Construct the heavily armored Command Core citadel at the bow prow, spanning Levels L3 to L6.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Control Core Shell  ·  Grid Zone: r0-r2  ·  Level: L3-L6 (X0)   │
│ [VIEWPORT] Looking: Aft (-Z)  ·  Orientation: ▲ UP (+Y) · ◄ STBD (+X) · PORT (-X)►   │
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armour Block  │ 36  │ 72.0 t / 36   │ Armored bow citadel shell at r0-r2       │
│ Reinforced Glass    │ 12  │  2.4 t / 12   │ Forward panoramic canopy at r0·L5-L6     │
│ Sliding Door        │ 1   │  1.0 t / 10   │ `ORC Door Bridge` at r2·L4               │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 14 · Bridge Avionics & Flight Control Core

Install the redundant flight controllers, programmable block, and pilot stations inside the Command Core.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 14 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Remote Control      │ 2   │  1.0 t / 50   │ `ORC CC RC Main` (r1·L3), `Spare` (r2·L3)│
│ Programmable Block  │ 1   │  0.5 t / 25   │ `ORC CC PB` (r1·L4, X0)                  │
│ Timer Block         │ 2   │  0.4 t / 20   │ `ORC TMR Heartbeat` (r1·L5), `Home` (r2) │
│ Event Controller    │ 4   │  0.8 t / 40   │ `ORC EC Lift`, `Power`, `Cargo`, `MP`    │
│ Control Seat        │ 2   │  0.8 t / 30   │ `ORC Helm Pilot`, `ORC Helm DroneOps`    │
│ Survival Kit        │ 1   │  2.2 t / 20   │ `ORC Survival Kit` (r1·L4, port wall)    │
│ Air Vent            │ 1   │  0.2 t / 10   │ `ORC Vent Bridge` (ceiling at r1·L6)     │
│ Antenna & Cam       │ 2   │  0.5 t / 20   │ `ORC CC Antenna` & `ORC Cam Mast` (roof) │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Command Core Avionics Rack, X = 0 (Looking from Starboard)

    r1                r2
    ┌─────────────────┬─────────────────┐  L6 Upper Canopy
    │ ORC Vent Bridge │ ORC EC MP       │
    ├─────────────────┼─────────────────┤  L5 Bridge Ceiling
    │ TMR Heartbeat   │ TMR Home        │
    ├─────────────────┼─────────────────┤  L4 Bridge Floor
    │ ORC CC PB       │ ORC EC Cargo    │
    ├─────────────────┼─────────────────┤  L3 Avionics Rack
    │ RC Main (+Z)    │ RC Spare (+Z)   │
    ├─────────────────┼─────────────────┤  L2 Life Support
    │ ORC EC Lift     │ ORC EC Power    │
    └─────────────────┴─────────────────┘  L1 Ventral Deck
```

**Configuration Settings:**
- Remote Controls: Forward = +Z (Bow), Up = +Y (Roof). Dampeners = ON. Waypoint `HOME` loaded.
- Air Vent: Depressurize = OFF. Pressurizes the bridge cabin from the L3 O2 tank.

---

### Pre-Enclosure Inspection Gate 1.4A: Command Bridge Seals & Avionics

> [!IMPORTANT]
> **POINT OF NO RETURN:** Verify atmospheric hermetic seal and avionics boot before fitting drone bays.
>
> 1. [ ] Command Core achieves 100% O2 room pressurization when `ORC Door Bridge` is closed.
> 2. [ ] Both Remote Controls face +Z and align with `ORC Cam Bow`.
> 3. [ ] `ORC CC PB` boots `Carrier.Orca` script with 0 compilation errors.
> 4. [ ] `ORC TMR Heartbeat` successfully resets watchdog on 1.0 s ping.

---

## Phase 5 · Drone Bays & Mechanical Guide Jaws

### Step 15 · Bay Overhead Connectors (Bays P1-P3, S1-S3)

Install 6× overhead ceiling connectors facing straight down on the L6 ore collection trunk branches.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Drone Bay Drops  ·  Grid Zone: Bays P1-P3, S1-S3  ·  Level: L6-L7│
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 15 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector           │ 6   │ 24.0 t / 150  │ Facing DOWN at bay centers (X ±5, r3/r9/r15)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Cross-Section of Bay Ceiling Drop (Bay P1 at r3, Looking Toward Bow)

    X: -8         -5         -2   -1.5
       ┃           ┃          ┃    ║   L7 Roof Deck
       ┃         [CON↓]       ┃    ║   Overhead Ceiling Connector
       ┃           │          ┃    ║   (plumbed to L6 Ore Trunk)
       ┃     P1    │    Util  ┃    ║
       ┃    Clear  │   Corrid.┃    ║
       ┃     5 B   │          ┃    ║
       ┃           ┃          ┃    ║   L1 Ventral Opening
              Open Below
```

**Bay Allocation:**
- **Bay P1 & S1 (r3, X ±5):** REMORA-01 Vertical Shaft Drillers (Primary mining).
- **Bay P2 (r9, X -5):** KESTREL-01 Fast Prospector Scout Drone (Ore reconnaissance).
- **Bay S2 (r9, X +5):** ALBATROSS-01 Utility & Field-Repair Craft (Mobile maintenance).
- **Bay P3 & S3 (r15, X ±5):** REMORA-01 Vertical Shaft Drillers (Primary mining).

---

### Step 16 · Folding Mechanical Guide Jaws (Bays P1-P3, S1-S3)

Install motorized twin-hinge guide jaws at Level L4 inside each bay to physically capture, align, and steady docking craft during turbulent atmospheric operations.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bay Guide Jaws  ·  Grid Zone: All 6 Bays  ·  Level: L4 Mid-Deck │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 16 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Hinge         │ 12  │ 24.0 t / 120  │ Outer (X ±8) & Inner (X ±2) hinges @ L4  │
│ Beam Block Straight │ 24  │ 19.2 t / 24   │ 2 B extension arms per hinge             │
│ Magnetic Plate      │ 12  │ 28.8 t / 300  │ Clamp heads on arm tips (Auto-Lock ON)   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view at L4 (Bay P1 Guide Jaws in Clamping Action)

 X: -8 (Outer Frame)            X = -5 (Bay Center)            X = -2 (Inner Frame)
    ┃                                                          ┃
    [HINGE]                                                    [HINGE]
     ╲                                                        ╱
      ╲ [Beam Arm, 2 B]                                      ╱ [Beam Arm, 2 B]
       ╲                                                    ╱
        [MAG PLATE] ────────►  ┌──────────────┐  ◄──────── [MAG PLATE]
                               │ Remora Drone │
                               │  5.5 × 5.5 m │
                               └──────────────┘

    [HINGE]     = Motorized Hinge at level L4 (Outer: X -8, Inner: X -2)
    [MAG PLATE] = Magnetic Plate (Auto-Lock ON) gripping drone waist rub rails
```

**Mechanical Settings:**
- Hinges (`ORC Bay Jaws`): Braking Torque 1.0 MN·m, Velocity 15 RPM. Angle Limits: 0° (retracted flush against truss) to 45° (clamped on drone).
- Mag Plates (`ORC Bay Jaw Clamps`): Auto-Lock = ON.

---

### Step 17 · Central Bay Repair Turrets (P2 & S2, L4)

Install 2× Laser Multitool turrets (Simple Laser Multitools Mod) at the mid-height shelf of central bays P2 and S2 to provide spherical repair coverage over all 6 docked drones.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 17 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Laser Multitool Turr│ 2   │  6.0 t / 500  │ `ORC Repair P2` (X -2), `S2` (X +2) @ L4 │
│ Large Conveyor Tube │ 26  │ 31.2 t / 26   │ L4 service gallery line (r3 to r17)      │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings:** Mode = Weld Only, Target Own Grids, Never Grind. Plumbed to `ORC Parts Store`.

---

### Pre-Enclosure Inspection Gate 1.5A: Drone Bay Clearance & Hinge Kinematics

> [!IMPORTANT]
> **POINT OF NO RETURN:** Conduct mechanical swing clearance tests before mounting outriggers.
>
> 1. [ ] All 12 guide jaw hinges swing from 0° flush to 45° clamp without clipping frame beams.
> 2. [ ] Magnetic plates auto-lock upon contact with simulated Remora drone waist frame.
> 3. [ ] Repair turrets at P2 and S2 achieve unbroken line-of-sight to bay centers of P1, P2, P3 and S1, S2, S3.
> 4. [ ] All 6 bay spotlights and approach sensors cycle correctly on target entry.

---

## Phase 6 · Outrigger Pylons & Thrust Nacelles

### Step 18 · Cantilever A-Frame Corner Outriggers (OR-1P, OR-1S, OR-3P, OR-3S)

Construct 4× heavy diagonal cantilever outrigger pylons at the hull corners, each mounting a 3×3×5 B Large Atmospheric Thruster and heavy landing gear leg.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Corner Outriggers ·  Grid Zone: Rows r1, r17  ·  Level: L0-L7   │
│ [VIEWPORT] Looking: Forward (+Z)  ·  Orientation: ▲ UP (+Y) · ◄ INBOARD · OUTBOARD►  │
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 18 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Atmo Thruster │ 4   │ 132.0 t / 400 │ `ORC Lift` nacelles at X ±11, r1 and r17 │
│ Heavy Landing Gear  │ 4   │  9.6 t / 200  │ `ORC Gear` at X ±9, extending to L0      │
│ Diagonal Beam Block │ 48  │ 38.4 t / 48   │ Cantilever A-frame structural trusses    │
│ Camera (Down)       │ 4   │  0.8 t / 40   │ `ORC Cam Gear` landing monitoring cams   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Elevation of Corner Outrigger OR-1P (Looking Forward along Z-axis)

    Hull Frame (Outer)      Cantilevered A-Frame Truss           Outboard Nacelle
    X = -8                  X = -9      X = -10..-12             X = -12
      ┃                      ┌───────────┬────────────────────────┐  L7 Roof Deck
      ■══════════════════════╪═══════════╧════════════════════════■
      ║ ╲ Diagonal Beam      │           │ Upper Truss Cap        │
      ║   ╲                  │           │                        │
      ║     ╲                │ [GEAR]    │ [LARGE ATMO ↓]         │  L4 Mid-Deck
      ║       ╲              │ 1×1×3 B   │ 3×3×5 B Nacelle        │  (6.48 MN Lift)
      ║         ╲            │ (Inboard) │                        │
      ■═══════════╧══════════╪═══════════╪════════════════════════■  L1 Belly
      ║ ╱ Diagonal Beam      │           │ Nozzle Line            │  ─── Nozzle
      ║                      │           └────────────────────────┘
      ╨                      ▼ Foot at L0 (≥ 1 B below nozzle line)
```

**Geometry & Physics:**
- Pylons project 4 B outboard from X ±8 to X ±12.
- Large Atmo nozzles sit at Level L1; Landing Gear feet extend to Level L0 (2.5 m below thruster nozzles) ensuring thruster exhaust never touches the planetary surface when grounded.

---

### Step 19 · Midships Emergency Hydrogen Outriggers (OR-2P, OR-2S, r9)

Construct 2× midships cantilever outriggers at row r9 mounting twin Large Hydrogen Thrusters for emergency ascent.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 19 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large H2 Thruster   │ 2   │ 14.4 t / 200  │ `ORC H2 Assist` at X ±11, r9·L1 (DOWN)   │
│ Large Conveyor Tube │ 8   │  9.6 t / 8    │ High-pressure fuel run from AS H2 tank   │
│ Diagonal Beam Block │ 24  │ 19.2 t / 24   │ Midships A-frame reinforcement bracing   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings:** Group `ORC H2 Assist` = Default OFF. Automatically engaged by `ORC EC Lift` or `ORC EC Power`.

---

### Step 20 · 6-DOF Lateral Attitude Propulsion Grid

Install 28× Small Atmospheric Thrusters around the perimeter frame to provide 6-axis translational and rotational authority.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Attitude Thrusters ·  Grid Zone: Perimeter  ·  Level: L1 & L7   │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 20 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Atmo Thruster │ 28  │ 22.4 t / 280  │ 6-axis lateral & attitude thrust cluster │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

| Group Name | Mounting Location | Qty | Thrust Vector | Purpose |
|---|---|---|---|---|
| `ORC Lat +X` | Port Outer Frame (X -8.5, r5 & r13) | 6 | Pushes STARBOARD (+X) | Lateral Stationkeeping |
| `ORC Lat -X` | Stbd Outer Frame (X +8.5, r5 & r13) | 6 | Pushes PORT (-X) | Lateral Stationkeeping |
| `ORC Lat +Z` | Stern Transom (r18, X ±5, L1 & L7) | 8 | Pushes FORWARD (+Z) | Cruise Propulsion |
| `ORC Lat -Z` | Bow Prow (r0, X ±5, L1 & L7) | 8 | Pushes AFT (-Z) | Braking & Deceleration |

---

### Pre-Enclosure Inspection Gate 1.6A: Outrigger Structural Bracing & Thruster Plumbing

> [!IMPORTANT]
> **POINT OF NO RETURN:** Conduct full thrust test fires on all 6 outrigger nacelles before commissioning.
>
> 1. [ ] 4× Large Atmo thrusters in `ORC Lift` fire downward with zero thruster wash damage to bay lanes.
> 2. [ ] 4× Landing Gear lock firmly to terrain at Level L0 with thruster nozzles suspended 2.5 m above surface.
> 3. [ ] 2× Large H2 thrusters show green fuel supply from `ORC AS H2` tank and remain OFF in standby.
> 4. [ ] Lateral thruster groups execute pure translational motion without induce pitch or roll coupling.

---

## Phase 7 · Instrumentation, Sensors & EC/TMR Automation Ladder

### Step 21 · Sensor Grid & Perimeter Instrumentation

Install all remaining tactical cameras, docking guidance lights, and subsurface ore detectors.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 21 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Ore Detector        │ 1   │  2.5 t / 10   │ `ORC Ore Detector` (Ventral Keel r15·L1) │
│ Camera              │ 8   │  1.6 t / 80   │ Bay cameras (6) + Belly cam (1) + Transom│
│ Sensor              │ 6   │  1.2 t / 60   │ Bay approach sensors (`ORC Sensor BR-*`) │
│ Spotlight           │ 6   │  1.2 t / 30   │ Bay approach spotlights (`ORC Bay Lights`)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 22 · Wire Native Block Failsafe Matrix (Rule C4)

Configure the 4× Event Controllers and 2× Timer Blocks to establish autonomous hardware-level safety independent of programmable block execution:

```
                  FAILSAFE AUTOMATION LADDER (RULE C4)

    ┌───────────────────────────┐         ┌──────────────────────────────────┐
    │ `ORC TMR Heartbeat` (5 s) │────────►│ If PB fails to ping within 5 s:  │
    │ [PB Watchdog Reset]       │         │  · Inertial Dampeners ──► ON     │
    │                           │         │  · Gyro Overrides ──────► OFF    │
    └───────────────────────────┘         │  · Start `ORC TMR Home` (Auto-RTB)
                                          └──────────────────────────────────┘

    ┌───────────────────────────┐         ┌──────────────────────────────────┐
    │ `ORC EC Lift`             │────────►│ TRUE:  `ORC H2 Assist` ──► ON    │
    │ [Thrust ≥ 85%†]           │         │ FALSE: `ORC H2 Assist` ──► OFF   │
    └───────────────────────────┘         └──────────────────────────────────┘

    ┌───────────────────────────┐         ┌──────────────────────────────────┐
    │ `ORC EC Power`            │────────►│ TRUE (Reactors < 10% / Uranium): │
    │ [Reactor Output / Fuel]   │         │  · `ORC Batteries` ──► Auto      │
    │                           │         │  · `ORC H2 Assist` ──► ON        │
    └───────────────────────────┘         │  · Trigger `ORC TMR Home`        │
                                          └──────────────────────────────────┘

    ┌───────────────────────────┐         ┌──────────────────────────────────┐
    │ `ORC EC Cargo`            │────────►│ TRUE (Ore Hold ≥ 90% full):      │
    │ [Hold Capacity Threshold] │         │  · `ORC HD Gate` ──► OFF (Lock)  │
    │                           │         │  · Sound Warning Chime           │
    └───────────────────────────┘         │  · PB Summons MULE-01 via IGC    │
                                          └──────────────────────────────────┘

    ┌───────────────────────────┐         ┌──────────────────────────────────┐
    │ `ORC EC MP`               │────────►│ TRUE (Mule Clamped on Pad):      │
    │ [Mule Pad Conn Locked]    │         │  · `ORC HD Drain` ──► Drain ON   │
    │                           │         │  · `ORC MP Intake` ──► Whitelist │
    └───────────────────────────┘         │ FALSE: Both Sorters ──► Standby  │
                                          └──────────────────────────────────┘
```

---

## 7 · Hardware-to-Scripting Contract & Zero-GC PB Specs

### 7.1 Standardized Block Taxonomy & Group Memberships

All terminal blocks utilize the strict `[ORC]` system prefix:

| Group Name | Primary Managed Blocks | Hardware Function & Role |
|---|---|---|
| `ORC CC` | `ORC CC RC Main`, `ORC CC RC Spare`, `ORC CC PB`, `ORC CC Antenna` | Core C2 Avionics & Navigation |
| `ORC PC` | `ORC Reactors` (6), `ORC Batteries` (2), `ORC Gyros` (4), `ORC U Store` | Primary 90 MW Power Citadel |
| `ORC HD` | `ORC Ore Hold`, `ORC HD Gate`, `ORC HD Drain` | Central Ore Transshipment Buffer |
| `ORC AS` | `ORC AS H2`, `ORC Parts Store` | Stern Engineering & Gas Reserves |
| `ORC MP` | `ORC MP Conn`, `ORC MP Cradle`, `ORC MP Cradle Clamps`, `ORC Sensor MP`| Dorsal 7×7 B Freight Shuttle Pad |
| `ORC BR` | `ORC Bay Conns` (6), `ORC Bay Sensors` (6), `ORC Bay Lights` (6) | 6× Guided Drone Bay Drops |
| `ORC JAWS`| `ORC Bay Jaws` (12 Hinges), `ORC Bay Jaw Clamps` (12 Mag Plates) | Mechanical Capture Guide Jaws |
| `ORC OR` | `ORC Lift` (4 Large Atmo), `ORC H2 Assist` (2 Large H2), `ORC Gear` (4)| Heavy Thrust Outriggers |
| `ORC LAT` | `ORC Lat +X`, `ORC Lat -X`, `ORC Lat +Z`, `ORC Lat -Z` (28 Small Atmo) | 6-DOF Attitude Control Cluster |
| `ORC LOGIC`| `ORC EC Lift`, `ORC EC Power`, `ORC EC Cargo`, `ORC EC MP`, Timers | Hardware Failsafe Ladder |

---

### 7.2 Zero-GC PB Architecture (`scripts/Carrier.Orca/`)

The carrier core script executes inside `[ORC] PB Carrier Core` running on an `Update10` periodic cycle (6 Hz), adhering strictly to zero-allocation architecture:

1. **State Machine (`OrcaCarrierState`):**
   - `STATIONARY_HOVER`: Carrier maintains geostationary position over mining field with auto-dampeners.
   - `DRONE_DOCKING`: Activated when bay sensor detects incoming drone. Opens guide jaws to 45°, locks mag plates upon contact, seats overhead connector, closes jaws.
   - `ORE_DRAIN`: Monitors cargo fill level. Computes fill rate $dM/dt$. When buffer exceeds 80%, broadcasts `TMS.MULE.CALL;ORC-01;HOLD_FULL` over IGC.
   - `MULE_TRANSSHIP`: Engages gantry cradle clamps, locks `ORC MP Conn`, activates `ORC HD Drain` until empty, releases clamps.
2. **Strict Grid Scoping:** Every terminal query enforces `b => b.CubeGrid == Me.CubeGrid` to ignore docked drone subgrids.
3. **Minifier Safety:** Avoids `enum.ToString()` or `nameof()`; uses static string tables and `SbFormat`.

---

### 7.3 Master IGC Network Protocol Dictionary

| Channel | Payload Schema | Direction | Purpose |
|---|---|---|---|
| `TMS.ORCA.STATUS` | `ORC-01;<State>;<OreFill%>;<Batt%>;<UraniumHours>;<BaysFree>` | Broadcast Out | Carrier health & logistics telemetry |
| `TMS.MULE.CALL` | `ORC-01;<Priority>;<OreMass_t>;<GPS_Coords>` | Unicast Out | Calls MULE-01 shuttle for cargo pickup |
| `FLT.REM.STATUS` | `<DroneID>;<HoldFill%>;<Battery%>;<Status>` | Broadcast In | Receives incoming status from Remoras |
| `TMS.SCOUT.VEIN` | `<ScoutID>;<OreType>;<Depth_m>;<GPS_Coords>` | Broadcast In | Receives deposit coordinates from Kestrel |

---

## 8 · Acceptance Testing Protocols (Suites A-G)

```
========================================================================================
ACCEPTANCE TESTING PROTOCOL: ORCA-01 HEAVY MINING CARRIER
EXECUTION ENVIRONMENT: Space Engineers Earth-Like (1.0 g Gravity, 1.0 atm Density)
========================================================================================
```

### Suite A: Structural Geometry & Weight Budget Check
- [ ] Airframe conforms to 17 × 19 × 7 B bounding envelope without mechanical collision.
- [ ] Total dry mass registers at 764.0 t (± 2.0%).
- [ ] Center of Mass (CoM) aligns within 0.2 B of midships row r9 on centerline X0.

### Suite B: Vertical Lift & Attitude Stability Test
- [ ] Empty hover consumes ≤ 25% thrust on 4× Large Atmo thrusters (gravity load 7.49 MN).
- [ ] Fully laden test (simulated 2,166 t mass with 4 drones docked + 1,200 t cargo buffer) hovers stably at ≤ 70% thrust rating.
- [ ] Pitch and yaw stability test: 90° attitude hold maintains heading without oscillation.

### Suite C: Mechanical Guide Jaw Capture & Docking Loop
- [ ] Fly test REMORA-01 grid into Bay P1 lane.
- [ ] `ORC Sensor BR-P1` detects grid at 15 m; spotlight illuminates lane.
- [ ] Motorized guide jaws swing inward from 0° to 45°, mag plates auto-lock to drone frame.
- [ ] Drone overhead connector seats into `ORC BR-P1 Conn` with zero structural shear.

### Suite D: Conveyor Segregation & High-Speed Sorter Drain
- [ ] Deposit 100 t Iron Ore into Bay P1 connector; verify transfer through `ORC HD Gate` into `ORC Ore Hold`.
- [ ] Verify zero Iron Ore enters `ORC U Store` or `ORC Parts Store`.
- [ ] Land test MULE-01 shuttle on roof deck; verify `ORC EC MP` triggers `ORC HD Drain`.
- [ ] Transfer 650 t ore into Mule within < 90 s.

### Suite E: Emergency Power Loss & Hydrogen Failover
- [ ] Cut nuclear reactor master breaker during full hover.
- [ ] `ORC EC Power` detects reactor deficit; instantly switches `ORC Batteries` from Recharge to Auto.
- [ ] 2× Large H2 thrusters ignite automatically; carrier maintains hover altitude with zero altitude loss.

### Suite F: Zero-GC PB Execution & IGC Telemetry
- [ ] Run `Carrier.Orca` script for 1,000 game ticks in Space Engineers profiler.
- [ ] Verify GC allocations register at exactly 0 B/tick.
- [ ] Verify `TMS.ORCA.STATUS` broadcasts valid telemetry packet every 100 ticks.

### Suite G: Complete Autonomous Fleet Loop
- [ ] 4× Remora drones deploy from bays, drill test shaft, return, and dock into jaws.
- [ ] Carrier buffers 1,200 t ore; PB automatically summons MULE-01 via `TMS.MULE.CALL`.
- [ ] Mule docks on roof pad, drains 650 t, departs to Ground Base.
- [ ] Full operational cycle completes with zero human pilot intervention.

---

## 9 · Master Block Summary

| Block Name / Type | Total Qty | Subsystem Group | Primary Function |
|---|---|---|---|
| **Large Atmospheric Thruster** | 4 | `ORC Lift` | 25.92 MN downward primary atmospheric lift |
| **Large Hydrogen Thruster** | 2 | `ORC H2 Assist` | 28.80 MN emergency vertical ascent thrust |
| **Small Atmospheric Thruster** | 28 | `ORC LAT` | 6-DOF attitude control, lateral stationkeeping |
| **Small Nuclear Reactor** | 6 | `ORC Reactors` | 90.0 MW continuous primary electrical power |
| **Large Battery** | 2 | `ORC Batteries` | 6.0 MWh / 48.0 MW burst discharge energy buffer |
| **Large Gyroscope** | 4 | `ORC Gyros` | Attitude control and angular stabilization |
| **Large Cargo Container (3×3×3)**| 1 | `ORC Ore Hold` | 1,200 t raw ore transshipment buffer |
| **Small Cargo Container** | 2 | `ORC U Store`, `Parts` | Uranium fuel and repair components stores |
| **Large Hydrogen Tank** | 1 | `ORC AS H2` | 2,500,000 L emergency fuel storage |
| **Overhead Bay Connector (Down)** | 6 | `ORC Bay Conns` | Drone docking and raw ore collection feeds |
| **Dorsal Mule Connector (Up)** | 1 | `ORC MP Conn` | High-speed transshipment feed to freight shuttle |
| **Conveyor Sorter** | 3 | `ORC HD Gate/Drain`, `MP`| Segregated ore filtering and drain interlocks |
| **Laser Multitool Turret (Mod)** | 2 | `ORC Repair` | Spherical non-contact repair of docked drones |
| **Large Hinge** | 14 | `ORC Bay Jaws`, `MP Cradle`| 12 bay guide jaws + 2 Mule clamp arms |
| **Magnetic Plate** | 16 | `ORC Bay Jaw Clamps`, `MP`| Waist docking clamps + 2 ventral ground anchors |
| **Heavy Landing Gear** | 4 | `ORC Gear` | Corner outrigger landing gear legs (L0) |
| **Remote Control** | 2 | `ORC CC RC Main/Spare` | Redundant C2 navigation and autopilot |
| **Programmable Block** | 1 | `ORC CC PB` | Executes `Carrier.Orca` zero-GC script |
| **Timer Block** | 2 | `ORC TMR Heartbeat/Home`| Heartbeat watchdog and emergency RTB autopilot |
| **Event Controller** | 4 | `ORC EC Lift/Power/Cargo/MP`| Hardware failsafe matrix (Rule C4) |
| **Laser & Broadcast Antenna** | 1 | `ORC CC Antenna` | Fleet IGC communications mesh |
| **Cameras & Sensors** | 21 | `ORC Cams`, `ORC Sensors`| Guidance, docking alignment, approach detection |
| **Spotlights & Nav Lights** | 8 | `ORC Bay Lights`, `MP` | Worksite illumination and docking cues |
| **Ore Detector** | 1 | `ORC Ore Detector` | Subsurface deposit scanning |
| **Large Solar Panel** | 24 | `ORC Solar` | Auxiliary renewable trickle charge |
| **Beam & Diagonal Blocks** | ~380 | Structural Frame | Industrial skeleton, A-frame trusses, jaw arms |
| **Heavy Armour Blocks** | ~135 | Armored Vaults | Bridge shell, reactor vault, Mule blast deck |
| **Light Armour Blocks** | ~36 | Fairings & Shells | Hold fairings and engineering covers |
| **Large Conveyor Tubes/Junctions**| ~52 | Conveyor Network | Ore trunk, service gallery, and manifold lines |

---

## Chapter 2 · REMORA-01 Vertical Shaft Driller
### Visual Build Manual & System Engineering Specification
#### 12-Pillar Architectural Standard · Parity with MASTODON-01 Benchmark

```
========================================================================================
SYSTEM SPECIFICATION: REMORA-01 VERTICAL SHAFT DRILLER
ROLE: Uncrewed Autonomous Vertical Borehole Miner & Subsurface Geological Extractor
GRID: Small Grid (0.5 m / block)  ·  FOOTPRINT: 11 × 11 sb (5.5 × 5.5 m)
HEIGHT: 22 sb (11.0 m) from drill tips to crown docking connector
DRY MASS: ≈ 48.5 t  ·  MAX LADEN MASS: ≈ 195.7 t (with 147.2 t / 54,000 L ore buffer)
TOTAL PCU: ≈ 1,774 PCU  ·  POWER PLANT: 2× Small-Grid Large Batteries (2.16 MWh / 8.0 MW)
PROPULSION: 4× Large Atmo (2,304 kN Lift) + 4× Large Flat Atmo (920 kN 4-Face Lateral)
CUTTER HEAD: 8× Small-Grid Drills in 9×9 sb Circular Ring (5.5 m Borehole Diameter)
CARGO & LOGISTICS: 100% Stone Retained (Decision 32) · Central Continuous Conveyor Spine
DOCKING INTERFACE: Crown-Mounted Full-Size Small-Grid Connector + Waist Magnetic Rub Collar
CARRIER INTEGRATION: Docks into ORCA-01 Bays P1, S1, P3, S3 via Motorized Guide Jaws
AUTHORITY: Blae Decisions 1-70 (Canon)  ·  STANDARD: 12-Pillar Mastodon Benchmark
========================================================================================
```

---

## 0 · Reference Grid & Spatial Datum

### 0.1 Spatial Datum & Hull Envelope

The Remora airframe is engineered as a high-density, vertical cylindrical prism built on an **11 × 11 sb** footprint (5.5 × 5.5 m) with an overall height of **22 sb** (11.0 m). Its cross-section conforms strictly to the circular cutting swath of its 8-drill rotary ring, allowing uninhibited vertical translation inside boreholes without wall snagging:

```
Axes:    X = Beam:     Port (-5 sb, -2.5 m) ◄── 0 (Core Axis) ──► Starboard (+5 sb, +2.5 m)
         Z = Length:   Aft  (-5 sb, -2.5 m) ◄── 0 (Core Axis) ──► Forward   (+5 sb, +2.5 m)
         Y = Height:   Down (L0 Drill Nose, 0.0 m) ─────────────► Up (L22 Crown Dock, 11.0 m)

Origin:  Datum [X0, Y0, Z0] located at bottom center of the drill head cutter axis at Level L0.

Scale:   1 sb (small block) = 0.5 m  ·  11 sb = 5.5 m  ·  22 sb = 11.0 m.

Sections: Levels L0-L6   (DH) · Drill Head & Downward Sensor Eye
          Levels L6-L10  (CL) · 8-Port Collector Manifold & Stone Buffer Core
          Levels L10-L15 (LF) · Corner Lift Nacelles, Lateral Flat Atmo & Waist Clamping Collar
          Levels L15-L17 (PW) · Power Core (2× Large Batteries around Spine)
          Levels L17-L20 (AV) · Avionics Core, Flight Controllers, Watchdog Timers & Comms
          Levels L20-L22 (CR) · Docking Crown, Guide Chamfers, Cam Up & Connector
```

---

### 0.2 Standard 10-Slot SE Paint Palette

The Remora uses the standardized Space Engineers 10-slot industrial paint palette to provide high visual contrast inside dark subterranean shafts and ensure error-free assembly of small-grid conveyor ports:

| Slot | Finish | Hex Code | RGB | Functional Zone & System Usage |
|---|---|---|---|---|
| **01** | Battered Armor | `#22252A` | `34, 37, 42` | Primary Structural Skeleton, Corner Thrust Pylons, Keel Ties |
| **02** | Clean Armor | `#D8A824` | `216, 168, 36` | High-Throughput Vertical Ore Spine Tube & Manifold Junctions |
| **03** | Hazard Stripe | `#E5A00D` | `229, 160, 13` | Waist Magnetic Clamp Collar (L12-L13 Orca Jaw Interface) |
| **04** | Carbon / Worn | `#151719` | `21, 23, 25` | 8-Drill Cutter Head Housings & Lower Debris Deflectors |
| **05** | Heavy Battered | `#B83227` | `184, 50, 39` | Lift Thruster Nozzle Thermal Hazard Ring (4 sb Flame Boundary) |
| **06** | Heavy Armor | `#D2D7DF` | `210, 215, 223`| Power Core Isolation Casings, Battery Retaining Brackets |
| **07** | Clean Armor | `#1B4965` | `27, 73, 101` | Avionics Citadel, Armored Remote Control & PB Housing |
| **08** | Industrial Worn| `#5C677D` | `92, 103, 125`| Lateral Thruster Fairings, External Hull Aerodynamic Slopes |
| **09** | Neon / Emissive| `#00E5FF` | `0, 229, 255`  | Nose Visual Alignment Reticle, Docking Target Optical Rings |
| **10** | Matte Rubber | `#0A0B0D` | `10, 11, 13`   | Crown Impact Bumpers, Docking Guide Chamfers, Rub Rails |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

The exterior livery visually isolates the moving cutter head, the thermal exhaust gap, and the waist magnetic capture collar:

```
        X: -5 (Port)         X: -2       X0 (Spine)      X: +2        X: +5 (Stbd)
    L22 ────[RUBBER BUMPER]─────────────[REM DOCK ↑]─────────────[RUBBER BUMPER]──── L22
        │                                 │ Cam Up │                                │
    L20 ────[BLUE AVIONICS]───────────────┴────────┴─────────────[BLUE AVIONICS]──── L20
        │ Avionics Citadel · RC Main, PB Miner, Antenna, Failsafe EC Ladder         │
    L17 ────[WHITE BATTERY]───────────────┬────────┬─────────────[WHITE BATTERY]──── L17
        │ Battery Isolation Bank 1 (Port) │ Spine  │ Battery Isolation Bank 2 (Stbd)│
    L15 ────[CHARCOAL FRAME]──────────────┼────────┼─────────────[CHARCOAL FRAME]─── L15
        │ [CORNER POD] │ [HAZARD STRIPE WAIST CLAMP COLLAR]     │ [CORNER POD]      │
    L12 │ Large Atmo   │ Flat Atmo Maneuver │ Heavy Armor Pad   │ Large Atmo        │
        │ 576 kN Lift  │ Exhaust Port       │ Orca Jaw Clamp    │ 576 kN Lift       │
    L10 ────[RED NOZZLE]───┴────────────────┴────────┴───────────┴───[RED NOZZLE]─── L10
        │   ▼▼▼ Exhaust                                              ▼▼▼ Exhaust    │
    L8  │   ░░░░░░░░░░░░░░░░░ 4 sb Thermal Clearance Air Gap ░░░░░░░░░░░░░░░░░░░    │
    L6  ────[CHARCOAL MANIFOLD]───────────┬────────┬─────────────[CHARCOAL MANIFOLD] L6
        │ 8-Port Collector Manifold       │ Spine  │ Stone Retention Buffer Core    │
    L4  ────[BLACK CUTTER]────────────────┼────────┼─────────────[BLACK CUTTER]───── L4
        │ Drill Head Housing              │Cam/Lite│ Drill Head Housing             │
    L0  ────[ROTARY DRILL NOSE]───────────┴────────┴─────────────[ROTARY DRILL NOSE] L0
        │   ▼▼▼ Drill Tips                [DOWN EYE]                 ▼▼▼ Drill Tips │
        X: -5 (Port)                                                  X: +5 (Stbd)
```

---

### 0.3 Master Multi-Level Blueprints

#### Master Section Stack (L0 to L22)

```
Level (sb)   Section   Subsystem Description
────────────────────────────────────────────────────────────────────────────────────────
L20-L22      CR        Docking Crown · Connector ↑, Cam ↑, 4× Guide Chamfers, Bumpers
L17-L20      AV        Avionics Citadel · RC, PB, Antenna, Timers, 3× Event Controllers
L15-L17      PW        Power Core · 2× Small-Grid Large Batteries (2.16 MWh / 8.0 MW)
L10-L15      LF        Lift & Maneuver · 4× Large Atmo, 4× Flat Atmo, Waist Clamping Collar
L6-L10       CL        Collector Manifold · 8-port distribution hub, continuous spine
L0-L6        DH        Drill Head · 8× Small-Grid Drills in 9×9 sb ring, nose eye & light
```

#### Master Elevation (Looking Starboard, X = 0 Centerline)

```
        Z:  -5 (Aft)                     0 (Spine Axis)                  +5 (Forward)
    L22     ·                           [REM Dock ↑]                           ·  CROWN
    L21     ·                            │ Cam Up │                            ·
            ┌────────────────────────────┼────────┼────────────────────────────┐
    L20     │ [REM TMR Heartbeat]        │ REM RC │   [REM PB Miner]           │  AVIONICS
    L18     │ [REM EC Batt / Lift / Dock]│ Main   │   [REM Antenna 1.5 km]     │  CITADEL
            ├────────────────────────────┼────────┼────────────────────────────┤
    L17     │ [REM Battery 1 (Aft)]      │ Spine  │  [REM Battery 2 (Fwd)]     │  POWER
    L15     │ (1.08 MWh / 4.0 MW)        │ Tube   │  (1.08 MWh / 4.0 MW)       │  CORE
            ├────────────┬───────────────┼────────┼───────────────┬────────────┤
    L14     │ [Lift Pod] │ [REM Lat A]   │ Gyro 1 │ [REM Lat F]   │ [Lift Pod] │  LIFT &
    L12     │ Large Atmo │ [Clamp Collar]│ Gyro 2 │ [Clamp Collar]│ Large Atmo │  WAIST
    L10     │ 576 kN     │ Exhaust       │ Spine  │ Exhaust       │ 576 kN     │  COLLAR
            └───┬────────┴───────────────┼────────┼───────────────┴────────┬───┘
    L8          │ ◄────────── 4 sb Thermal Flame Gap (Open) ─────────────► │
            ┌───┴────────────────────────┴────────┴────────────────────────┴───┐
    L6      │                  8-Port Collector Manifold                       │  COLLECTOR
    L4      ├────────────────────────────┬────────┬────────────────────────────┤
    L2      │ [REM Drill Aft]            │Cam/Lite│ [REM Drill Fwd]            │  DRILL
    L0      │        ▼▼▼                 │  ↓↓↓   │        ▼▼▼                 │  HEAD
```

#### Master Cross-Section (Looking Toward Bow, Z = 0 Centerline)

```
        X:  -5 (Port)                    0 (Spine Axis)               +5 (Starboard)
    L22     ·                           [REM Dock ↑]                           ·  CROWN
    L21     ·                            │ Cam Up │                            ·
            ┌────────────────────────────┼────────┼────────────────────────────┐
    L20     │ [Avionics Armor Shell]     │ REM RC │ [Avionics Armor Shell]     │  AVIONICS
    L18     │ [Failsafe Logic Ladder]    │ Main   │ [Telemetry Broadcast Core] │
            ├────────────────────────────┼────────┼────────────────────────────┤
    L17     │ [REM Battery Bank (Port)]  │ Spine  │ [REM Battery Bank (Stbd)]  │  POWER
    L15     │ [Isolation Heavy Bulkhead] │ Tube   │ [Isolation Heavy Bulkhead] │
            ├────────────┬───────────────┼────────┼───────────────┬────────────┤
    L14     │ [Lift Pod] │ [REM Lat P]   │ Gyros  │ [REM Lat S]   │ [Lift Pod] │  LIFT &
    L12     │ Large Atmo │ [Clamp Collar]│ Center │ [Clamp Collar]│ Large Atmo │  WAIST
    L10     │ 576 kN     │ Exhaust       │ Axis   │ Exhaust       │ 576 kN     │  COLLAR
            └───┬────────┴───────────────┼────────┼───────────────┴────────┬───┘
    L8          │ ◄────────── 4 sb Thermal Flame Gap (Open) ─────────────► │
            ┌───┴────────────────────────┴────────┴────────────────────────┴───┐
    L6      │                  Collector Manifold Base                         │  COLLECTOR
    L4      ├────────────────────────────┬────────┬────────────────────────────┤
    L2      │ [REM Drill Port]           │Center  │ [REM Drill Stbd]           │  DRILL
    L0      │        ▼▼▼                 │Eye ↓↓↓ │        ▼▼▼                 │  HEAD
```

---

### 0.4B 3/4 Isometric Perspective Wireframes

#### Upper-Starboard 3/4 Isometric Perspective Wireframe

```
                     +Y (Crown / Up)
                      │   +Z (Forward)
                      │  /
                      │ /
         [REM DOCK ↑] │/
             ▲       ┌───┐
            ╱ ╲      │CON│ [CAM UP]
           ╱   ╲─────┘   └─────────────┐ [DOCKING GUIDE SLOPES L21]
          ╱  CR ╲                      │
         ┌───────┐  AVIONICS CITADEL   │
         │ L18   │═════════════════════╪════► [REM PB MINER]
         └───┬───┘                     │
            ╱│                         │
           ╱ │ [POWER CORE L16]        │
          ╱  │ 2× LARGE BATTERIES      │
         ╱   │    ┌──────────────┐     │
        ▼    │    │ 2.16 MWh     │     │
      -X     │    └──────────────┘     │
    (Port)   │           │             │
             │           ▼             │
             │    [WAIST CLAMP COLLAR] │    [NE LIFT POD L12]
             │    ┌──────────────┐     │         ▲
             │    │ HAZARD PADS  ├─────┴────────╱ ╲
             │    └──────────────┘             ┌───┐
             │                                 │ATM│ 576 kN
             │                                 └───┘
             └─────────────────────────────────┬─────► +X (Starboard)
                                               │
                                       ▼▼▼ 4 sb Flame Gap
```

#### Lower-Port 3/4 Isometric Perspective Wireframe

```
                                      +Y (Crown)
                                       │
            [WAIST CLAMP COLLAR L12]   │
                ┌──────────────┐       │
                │ ORANGE PADS  │       │
                └──────┬───────┘       │
                       │               │
       [SW LIFT POD]   │   4 sb FLAME  │
             ▲         │   GAP (OPEN)  │
            ╱ ╲        │═══════════════╪═════════► [COLLECTOR MANIFOLD L7]
           ┌───┐       │               │                 │
           │ATM│       │               │                 ▼
           └───┘       │               │          ┌──────────────┐
             │         │               │          │ 8-PORT HUB   │
             ▼ Nozzle  │               │          └──────┬───────┘
                       │               │                 │
                       │               │          [8-DRILL RING L3]
                       │               │                 ▲
                       │               │                ╱ ╲
                       │               │               ╱   ╲
                       │               │              ┌─────┐
                       └───────────────┴──────────────┤ DRI │
                                                      └─────┘
                                                     /   │   ▼▼▼ Drill Tips
                                                    /    ▼
                                                   /  [NOSE EYE]
                                               -Z (Aft)
```

---

### 0.4C Transverse Hull Station Profiles

#### Station 1: Docking Crown & Avionics Citadel (L18-L22, Y = 9.0 to 11.0 m)

```
        X: -5 (Port)         X: -2       X0 (Spine)      X: +2        X: +5 (Stbd)
    L22 ┌─────────────────────────────────[REM DOCK]─────────────────────────────┐  L22 Crown
        │  Guide Chamfer                   │Cam Up│                  Guide Chamfer│
    L20 ├─────────────────────────────────┼──────┼─────────────────────────────┤  L20 Deck
        │ [REM TMR Heartbeat]             │REM RC│ [REM PB Miner]              │
    L19 │ [REM TMR Return]                │Main  │ [REM Antenna 1.5 km]        │  L19 Logic
    L18 │ [REM EC Batt]   [REM EC Lift]   │(+Z)  │ [REM EC Dock]               │  L18 Avionics
    L17 └─────────────────────────────────┴──────┴─────────────────────────────┘  L17 Floor
```

#### Station 2: Power Core & Battery Isolation Deck (L15-L17, Y = 7.5 to 8.5 m)

```
        X: -5 (Port)         X: -2       X0 (Spine)      X: +2        X: +5 (Stbd)
    L17 ┌───────────┬─────────────────────╥──────╥─────────────────────┬───────────┐  L17 Ceiling
        │           │ [REM Battery 1]     ║Spine ║ [REM Battery 2]     │           │
    L16 │ Heavy     │ (Port Bank)         ║Tube  ║ (Starboard Bank)    │ Heavy     │  L16 Power
        │ Bulkhead  │ 1.08 MWh / 4.0 MW   ║3×3 sb║ 1.08 MWh / 4.0 MW   │ Bulkhead  │
    L15 └───────────┴─────────────────────╨──────╨─────────────────────┴───────────┘  L15 Deck
```

#### Station 3: Lift Thrust Array & Waist Clamping Collar (L10-L14, Y = 5.0 to 7.0 m)

```
        X: -5 (Port)         X: -2       X0 (Spine)      X: +2        X: +5 (Stbd)
    L14 ┌───────────┬─────────────────────╥──────╥─────────────────────┬───────────┐  L14 Roof
        │ [NW Lift] │ [REM Lat P]         ║Spine ║ [REM Lat S]         │ [NE Lift] │
    L13 │ Lrg Atmo  │ Flat Atmo Maneuver  ║Tube  ║ Flat Atmo Maneuver  │ Lrg Atmo  │  L13 Mid
        │ 576 kN    ├─────────────────────╢      ╟─────────────────────┤ 576 kN    │
    L12 │ Pod       │ [WAIST CLAMP PAD]   ║Gyros ║ [WAIST CLAMP PAD]   │ Pod       │  L12 Clamp
        │           │ (Heavy Armor Strip) ║Center║ (Heavy Armor Strip) │           │
    L10 └───▼▼▼─────┴─────────────────────╨──────╨─────────────────────┴─────▼▼▼───┘  L10 Nozzle
            Nozzle                                                           Nozzle
```

#### Station 4: 8-Drill Perimeter Rotary Cutter Ring (L0-L6, Y = 0.0 to 3.0 m)

```
        X: -5 (Port)         X: -2       X0 (Spine)      X: +2        X: +5 (Stbd)
    L6  ┌───────────┬─────────────────────╥──────╥─────────────────────┬───────────┐  L6 Collector
        │           │ Manifold Branch P   ║Spine ║ Manifold Branch S   │           │
    L4  │ [Drill P] │                     ║Tube  ║                     │ [Drill S] │  L4 Housing
        │ 3×3×6 sb  ├─────────────────────╢      ╟─────────────────────┤ 3×3×6 sb  │
    L2  │ Outer Ring│ [Down Light]        ║Nose  ║ [Down Camera]       │ Outer Ring│  L2 Center
        │ Cutter    │ Spotlight           ║Eye   │ Reticle Camera      │ Cutter    │
    L0  └───▼▼▼─────┴─────────────────────╨──────╨─────────────────────┴─────▼▼▼───┘  L0 Teeth
            Teeth                                                            Teeth
```

---

### 0.4 Master Conveyor Routing Schematic & Physical Inspection Ports

Per **Decision 32**, the Remora retains 100% of all mined stone and offloads it directly to the host Orca carrier. The conveyor circuit is a single vertical extraction pipe:

```
                            [REM Dock]
                    (Small-Grid Full-Size Connector)
                    (Faces UP @ L21-L22, Center 0, 0)
                                  │
                                  ▼
                      Central Conveyor Spine Tube
                      (Continuous 3×3 sb core axis)
                                  │
                                  ▼
                     Collector Manifold (L6-L8)
                     (8 radiating conveyor tubes)
                                  │
          ┌───────────────┬───────┴───────┬───────────────┐
          ▼               ▼               ▼               ▼
     [REM Drill 1]   [REM Drill 2]   [REM Drill 3]   [REM Drill 4]
     [REM Drill 5]   [REM Drill 6]   [REM Drill 7]   [REM Drill 8]
           (8× Small Grid Drills in 9×9 sb Perimeter Ring)
```

#### Physical Manual Inventory Inspection Ports

| Device | Block Name | Cell Datum | Port Type | Access Location & Clearance |
|---|---|---|---|---|
| **Crown Connector** | `REM Dock` | `sb (0, 0), L21` | Full Small-Grid Port | Crown Deck, accessible during docked inspection |
| **Upper Spine Hub** | `REM Hub Avionics`| `sb (0, 0), L18` | Medium Port (Port Face)| Avionics Deck port maintenance access |
| **Lower Spine Hub** | `REM Hub Battery` | `sb (0, 0), L16` | Medium Port (Stbd Face)| Battery Deck starboard service panel |
| **Collector Core** | `REM Hub Collector`| `sb (0, 0), L8` | 4× Cardinal Ports | Reached via 4 sb flame gap inspection hatch |

---

### 0.5 Master Electrical Single-Line Schematic & 5 Operational Energy Modes

```
               POWER STORAGE (2× Small-Grid Large Batteries)
                    2.16 MWh Capacity · 8.0 MW Max Peak Output
                                  │
                                  ▼
                          PRIMARY POWER BUS
         ┌────────────────────────┼────────────────────────┐
         │                        │                        │
         ▼                        ▼                        ▼
   LIFT PROPULSION          LATERAL THRUST           DRILLS & AVIONICS
   4× Large Atmo            4× Large Flat Atmo       8× Drills (0.16 MW)
   (9.6 MW Max Burst)       (0.92 MW Max Burst)      1× RC + 1× PB (0.05 MW)
   [Empty Hover: 2.0 MW]    [Attitude Gyros: 0.1 MW] 2× Cams + Spot (0.02 MW)
   [Laden Hover: 8.0 MW]
```

#### 5 Operational Energy Management Modes

| Mode | Operational State | Power Generation | Power Draw | Margin / Deficit | Battery Policy |
|---|---|---|---|---|---|
| **Mode 1** | **Docked Trickle Charge** | Shore Power (Orca) | 0.05 MW (Avionics standby) | +2.0 MW (Charging) | Recharge Mode |
| **Mode 2** | **Transit & Shaft Approach**| 8.0 MW Peak Buffer | 2.15 MW (Empty hover trim) | +5.85 MW (73% margin)| Auto (Discharge) |
| **Mode 3** | **Active Rotary Boring** | 8.0 MW Peak Buffer | 3.45 MW (Hover + 8× Drills) | +4.55 MW (57% margin)| Auto (Discharge) |
| **Mode 4** | **Max Laden Climb Out** | 8.0 MW Peak Buffer | 7.95 MW (100% lift thrust) | +0.05 MW (Near ceiling)| Auto (Full burst) |
| **Mode 5** | **Emergency Ascent & RTB** | 8.0 MW Peak Buffer | 8.00 MW (Lift + Max Damp) | 0.00 MW (Failsafe cut) | Auto (Abort drills)|

---

### 0.6 Master Pre-Flight Bill of Materials (BOM) & Physics Budget

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Drill Cutter Head** | 8× Drills (3×3×6 sb), 1× Cam, 1× Spotlight | 10 | 18.2 t | 850 |
| **Lift & Maneuver Thrust**| 4× Large Atmo (3×3×5 sb), 4× Large Flat Atmo | 8 | 13.8 t | 540 |
| **Power Storage Bank** | 2× Large Battery (small grid, 3×3×2 sb) | 2 | 8.8 t | 40 |
| **Avionics & Logic Core** | 1× RC, 1× PB, 1× Antenna, 2× Timers, 3× ECs | 8 | 1.8 t | 140 |
| **Spine & Conveyors** | 4× Conveyor Hubs, 16× Tubes, 1× Full Connector | 21 | 3.4 t | 120 |
| **Armor, Fairings & Collar**| Light armor slopes, 16× Heavy armor clamp pads | 84 | 2.5 t | 84 |
| **TOTAL (DRY AIRFRAME)** | **REMORA-01 Vertical Shaft Driller (Complete)** | **133** | **≈ 48.5 t** | **1,774** |

#### Earth 1 g Lift Physics & Thrust-to-Weight (T/W) Budget

- **Dry Mass:** 48.5 t (Gravity Load: 475.8 kN).
- **Maximum Laden Mass:** 195.7 t (Airframe 48.5 t + 147.2 t / 54,000 L Ore Buffer). Gravity Load: 1,919.8 kN.
- **Downward Vertical Lift Thrust:**
  - 4× Large Atmospheric Thrusters (small grid): 4 × 576 kN = **2,304 kN** (2.304 MN).
- **Earth 1 g T/W Ratio (Dry):** $2,304	ext{ kN} / 475.8	ext{ kN} = \mathbf{4.84}$ (Extreme vertical agility).
- **Earth 1 g T/W Ratio (Fully Laden):** $2,304	ext{ kN} / 1,919.8	ext{ kN} = \mathbf{1.20}$ (Sustained vertical climb out with 20% margin).
- **Horizontal Stationkeeping:** 4× Large Flat Atmospheric Thrusters provide 230 kN on each cardinal face, maintaining precise central axis alignment inside the borehole.

---

### 0.7 Small-Grid Builder Guide & Hanging Jig Setup

In planetary gravity (Earth 1 g), a vertical craft cannot stand on its drills during early construction. Build crown-down, hanging from a temporary test crane or ceiling connector:

```
Hanging Jig Setup in Gravity (Looking Starboard):
       ═══════════════════════════════════════════  Hangar Ceiling / Crane Boom
                           ║
                    [LARGE GRID CON] (Facing DOWN)
                           ║
                 [REM Dock Connector ↑]             ◄── Step 1 starts here!
                 (Faces UP at Level L22)
                           │
                           ▼
                  [BUILD DOWNWARD TO L0]
```

---

## Phase 1 · Docking Crown & Vertical Spine (CR & CL, L6-L22)

### Step 1 · Crown Docking Mount & Alignment Chamfers (L20-L22)

Mount the small-grid full-size connector facing UP at Level L21-L22, flanked by the upward reticle camera and rubber impact slopes.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Section: Docking Crown  ·  Footprint: sb -2 to +2  ·  Level: L20-L22       │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ FWD (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 1 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Full-Sz) │ 1   │  1.2 t / 150  │ Small-grid full-size; face UP @ L21-L22  │
│ Camera              │ 1   │ 0.05 t / 10   │ `REM Cam Up` beside connector @ (X+1, Z0)│
│ Light Armor Slope   │ 4   │ 0.16 t / 4    │ Chamfer collar around connector rim      │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L21 (Crown Level)

    X (sb): -1    0   +1
    Z: +1    ·   [   ] ·
        0   [   ][CON][CAM]   ◄── Connector centered at (0, 0)
       -1    ·   [   ] ·

    [CON] = Small-Grid Full-Size Connector (facing UP)
    [CAM] = Camera `REM Cam Up` (facing UP at +1, 0)
    [   ] = Chamfer slopes around connector base
```

**✓ Check:** Connector face sits centered on axis (0, 0) and stands 1 sb proud of the hull.

---

### Step 2 · Continuous Vertical Conveyor Spine Tube (L6-L20)

Run a continuous vertical conveyor line straight down from `REM Dock` along centerline axis (0, 0) through Levels L20 down to L6.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 2 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Conveyor Port Orientation       │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Conveyor Hub 3×3 sb │ 4   │  1.2 t / 40   │ Ports MUST face cardinal N/S/E/W         │
│ Conveyor Tube sb    │ 12  │  0.8 t / 36   │ Continuous vertical line along (0, 0)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Side elevation, X = 0 (Looking Starboard)

    L20   [REM Dock]
           │
    L18   [Conveyor Junction 3×3 sb]   (Avionics tap)
           │
    L16   [Conveyor Junction 3×3 sb]   (Battery deck tie)
           │
    L12   [Conveyor Junction 3×3 sb]   (Waist clamp tie)
           │
    L8    [Collector Core Box 3×3 sb]  (Drill manifold core)
           │
    L6    [Manifold Base]
```

---

### Step 3 · 8-Port Collector Manifold & Stone Buffer Core (L6-L10)

Expand the spine into an 8-port radiating collector hub at Levels L6-L8. Per **Decision 32**, all mined rock is retained in the collection loop.

```
Plan view, Y = L7 (Collector Hub)

    X (sb): -2    0   +2
    Z: +2   [OUT] [TUBE] [OUT]   ◄── Feeds Fwd Drills
        0   [TUBE][CORE] [TUBE]  ◄── Axis Conveyor
       -2   [OUT] [TUBE] [OUT]   ◄── Feeds Aft Drills

    [CORE] = Central Conveyor Junction (feeds upward)
    [TUBE] = Curved/Straight Conveyor Tubes radiating out to 8 drill ports
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 3 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Conveyor Hub 3×3 sb │ 4   │  1.2 t / 40   │ Radial distribution hubs at L7           │
│ Conveyor Tube sb    │ 8   │  0.6 t / 24   │ Radiating branch tubes to drill ports    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Pre-Enclosure Inspection Gate 2.1A: Vertical Conveyor Spine & Collector Manifold

> [!IMPORTANT]
> **POINT OF NO RETURN:** Inspect the vertical conveyor path before attaching drills or lift nacelles.
>
> 1. [ ] Continuous conveyor line runs from `REM Dock` at L21 to all 8 collector ports at L6.
> 2. [ ] All 8 collector outlet ports point straight down at Level L6 with correct spacing.
> 3. [ ] Zero conveyor ports face dead into structural framing.
> 4. [ ] Confirm stone retention circuit has zero sorters or ejector ports fitted.

---

## Phase 2 · Drill Head & Downward Sensor Eye (DH, L0-L6)

### Step 4 · Central Nose Sensor Eye & Illumination (L0-L6)

Mount the central downward-facing reticle camera and high-intensity spotlight on the centerline axis between rows r0 and r1 at Level L1-L2.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Camera              │ 1   │ 0.05 t / 10   │ `REM Cam Down` facing DOWN at (0, 0, L1) │
│ Spotlight           │ 1   │ 0.05 t / 10   │ `REM Light Down` facing DOWN @ (0, +1, L1)│
│ Light Armor Block   │ 8   │  0.4 t / 8    │ Protective nose deflection fairing       │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 5 · 8-Drill Perimeter Cutter Ring (L0-L6)

Mount 8× small-grid drills in a circular 9×9 sb perimeter ring hanging down from the Level L6 collector manifold outlets.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Section: Cutter Head  ·  Footprint: sb -4 to +4  ·  Level: L0-L6           │
│ [VIEWPORT] Looking: Upward (-Y)  ·  Orientation: ▲ FWD (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small-Grid Drill    │ 8   │ 18.0 t / 800  │ `REM DH Drill 1..8` (3×3×6 sb, nose DOWN)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Drill Head Layout (Looking Up from Voxel Bed, Y = L0)

    X (sb): -4     -2      0     +2     +4
    Z: +4  [DRILL 1] · [DRILL 2] · [DRILL 3]   ◄── Forward Teeth
    Z: +2     ·      ·     ·     ·     ·
    Z:  0  [DRILL 8] ·  [EYE/LGT] · [DRILL 4]   ◄── Midships Teeth
    Z: -2     ·      ·     ·     ·     ·
    Z: -4  [DRILL 7] · [DRILL 6] · [DRILL 5]   ◄── Aft Teeth

    Total Cutting Envelope: 11 × 11 sb (5.5 m diameter) circular borehole.
```

**Placement Sequence:**
1. Snap each drill's top intake port to one of the 8 collector manifold outlets at Level L6.
2. Group all 8 drills into `REM Drills`.

**✓ Check:** All 8 drills point nose-down at Level L0. Drill teeth extend 2 sb below the central camera.

---

### Pre-Enclosure Inspection Gate 2.2A: 8-Drill Ring Port Connections & Mechanical Lock

> [!IMPORTANT]
> **POINT OF NO RETURN:** Verify all 8 drill conveyor connections before building the lift decks.
>
> 1. [ ] All 8 drills show green inventory links to `REM Dock`.
> 2. [ ] Test deposit ore into Drill 1; verify instantaneous appearance at `REM Dock`.
> 3. [ ] 8-drill cutter footprint measures exactly 9×9 sb (cutting diameter clears 11 sb hull).
> 4. [ ] Downward camera and spotlight have clear line of sight through center gap.

---

## Phase 3 · Lift Thrust, Lateral Control & Waist Clamping Collar (LF, L10-L15)

### Step 6 · Corner Large Atmo Lift Pods & 4 sb Thermal Gap (L10-L15)

Construct 4× corner lift nacelles at Levels L10-L15 mounting 3×3×5 sb Large Atmospheric Thrusters facing straight down, maintaining a strict **4 sb thermal clearance air gap** above the Level L6 collector deck.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Section: Lift Nacelles  ·  Footprint: 4 Corners  ·  Level: L10-L15         │
│ [VIEWPORT] Looking: Forward (+Z)  ·  Orientation: ▲ UP (+Y) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 6 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Atmo Thruster │ 4   │ 11.8 t / 400  │ Corner pods at L10-L15 (facing DOWN)     │
│ Light Armor Slope   │ 16  │  0.8 t / 16   │ Pod aerodynamic fairings and nacelles    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Corner Lift Pod Elevation (NW Corner looking Forward along Z-axis)

    L15 ┌───────────────┐
        │ Nacelle Cap   │
    L14 │               │
    L13 │ Large Atmo    │  (3×3×5 sb footprint · 576 kN Lift)
    L12 │ Thruster Pod  │
    L11 │               │
    L10 └───▼▼▼▼▼▼▼▼▼───┘  ◄── Exhaust Nozzle
    L9      ░░░░░░░░░
    L8      ░ 4 sb  ░      ◄── 4 sb Clear Thermal Air Gap (Zero Voxel/Hull Contact)
    L7      ░ Flame ░
    L6  ┌───┴───────┴───┐
        │ Collector Hub │
```

**Configuration Settings:**
- Name thrusters: `REM LF Lift NW`, `REM LF Lift NE`, `REM LF Lift SW`, `REM LF Lift SE`.
- Group all 4 into `REM Lift` (Total vertical lift = 2,304 kN).

---

### Step 7 · Centerline Gyroscope Cluster (L11-L14)

Install 2× small-grid gyroscopes directly on the central spine between rows r11 and r14 to provide angular stability inside the borehole.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 7 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Gyroscope (Small)   │ 2   │  1.2 t / 40   │ `REM Gyro 1, 2` on spine @ L12-L13       │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 8 · 4-Face Lateral Flat Atmo Array (L11-L14)

Install 4× Large Flat Atmospheric Thrusters centered on the 4 cardinal hull faces at Level L11-L14 to provide non-rotating horizontal stationkeeping.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 8 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Flat Atmo     │ 4   │  2.0 t / 140  │ 1 per face @ L12 (230 kN thrust each)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

| Group Name | Face Location | Exhaust Vector | Thrust Direction | Role |
|---|---|---|---|---|
| `REM Lat F` | Forward Face (Z +5) | Points Forward (+Z) | Pushes AFT (-Z) | Forward/Aft Trim |
| `REM Lat A` | Aft Face (Z -5) | Points Aft (-Z) | Pushes FORWARD (+Z) | Forward/Aft Trim |
| `REM Lat P` | Port Face (X -5) | Points Port (-X) | Pushes STARBOARD (+X)| Lateral Stationkeep |
| `REM Lat S` | Starboard Face (X +5) | Points Stbd (+X) | Pushes PORT (-X) | Lateral Stationkeep |

---

### Step 9 · Waist Magnetic Clamping Collar (L11-L14)

Install 16× Heavy Armour blocks and flush magnetic contact plates around the mid-height perimeter at Level L12-L13. This provides the physical landing target for the Orca carrier's motorized guide jaws.

```
Plan view, Y = L12 (Waist Clamping Collar)

    X (sb): -5            -2      0      +2            +5
    Z: +5   [NW POD] ────[ORANGE CLAMP PAD FWD]──── [NE POD]
    Z: +2      │                                       │
    Z:  0   [CLAMP PAD P]      [SPINE CORE]      [CLAMP PAD S]  ◄── Orca Guide Jaws Grip Here!
    Z: -2      │                                       │
    Z: -5   [SW POD] ────[ORANGE CLAMP PAD AFT]──── [SE POD]

    Orange Stripe Heavy Armour Pads at X ±5 and Z ±5.
```

---

### Pre-Enclosure Inspection Gate 2.3A: Lift Thruster 4 sb Thermal Exhaust Clearance

> [!IMPORTANT]
> **POINT OF NO RETURN:** Test thermal exhaust clearance before sealing the upper power decks.
>
> 1. [ ] Fire all 4 corner Large Atmo thrusters at 100% override for 15 seconds.
> 2. [ ] Verify collector manifold and drill tops at Level L6 take zero thruster flame damage.
> 3. [ ] Confirm waist clamp collar sits flush at Level L12-L13, painted in Slot 03 Hazard Stripe.
> 4. [ ] Lateral Flat Atmo thrusters clear all corner nacelle fairings.

---

## Phase 4 · Power Core & Avionics Citadel (PW & AV, L15-L20)

### Step 10 · Twin Large Battery Bank (L15-L17)

Mount 2× small-grid Large Batteries symmetrically flanking the conveyor spine at Level L15-L17.

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Section: Power Core  ·  Footprint: sb -3 to +3  ·  Level: L15-L17          │
│ [VIEWPORT] Looking: Downward (+Y)  ·  Orientation: ▲ FWD (+Z) · ◄ PORT (-X) · STBD (+X)►│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 10 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Battery (Sm)  │ 2   │  8.8 t / 40   │ `REM Battery 1, 2` (3×3×2 sb) @ L16      │
│ Heavy Armour Plate  │ 12  │  0.6 t / 12   │ Battery containment box bulkheads        │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L16 (Battery Deck)

    X (sb): -3    0   +3
    Z: +2   ···  ···  ···
        0  [BAT 1][SPINE][BAT 2]   (3×3×2 sb footprint each)
       -2   ···  ···  ···
```

**✓ Check:** Total power storage = 2.16 MWh; peak output = 8.0 MW. Group: `REM Batteries`.

---

### Step 11 · Flight Control Core & Avionics Citadel (L17-L19)

Mount Remote Control, Programmable Block, and Antenna directly above the battery deck inside an armored citadel.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Remote Control      │ 1   │  0.6 t / 50   │ `REM RC Main` (Forward = +Z, Up = +Y)    │
│ Programmable Block  │ 1   │  0.5 t / 25   │ `REM PB Miner` (Executes Drone.Remora)   │
│ Radio Antenna       │ 1   │  0.2 t / 20   │ `REM Antenna` (Range set to 1,500 m†)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 12 · Safety Timers & Native Event Controllers (L18-L20)

Install the native block failsafe ladder providing hardware-level safety independent of script execution:

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Timer Block         │ 2   │  0.4 t / 20   │ `REM TMR Heartbeat` (5.0 s), `Return`(2s)│
│ Event Controller    │ 3   │  0.6 t / 30   │ `REM EC Batt`, `REM EC Lift`, `Dock`     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

```
Plan view, Y = L19 (Failsafe Logic Collar)

    X (sb): -2    0   +2
    Z: +2  [EC-BATT][ · ][EC-LIFT]
        0  [TMR-HB] [CON][TMR-RET]
       -2  [EC-DOCK][ · ][  ·   ]
```

---

### Pre-Enclosure Inspection Gate 2.4A: Battery Isolation & Failsafe Continuity

> [!IMPORTANT]
> **POINT OF NO RETURN:** Verify avionics alignment and event controller wiring before crown sealing.
>
> 1. [ ] `REM RC Main` faces forward (+Z) and up (+Y), aligning with `REM Cam Down`.
> 2. [ ] `REM TMR Heartbeat` successfully triggers `REM TMR Return` upon 5.0 s timeout.
> 3. [ ] `REM EC Batt` turns drills OFF and triggers return when battery falls to ≤ 25%.
> 4. [ ] `REM EC Lift` triggers emergency return if thrust load exceeds 90%.

---

## Phase 5 · Crown Finalization & Commissioning (CR, L20-L22)

### Step 13 · Docking Crown Guide Chamfers & Impact Bumpers (L20-L22)

Enclose the crown collar with 45° guide slopes to channel the drone smoothly into the Orca ceiling receiver.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Light Armor Slope   │ 8   │  0.3 t / 8    │ 45° guide slopes leading to connector rim│
│ Heavy Armor Block   │ 4   │  0.4 t / 4    │ Corner impact bumpers                    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 14 · Final Livery, Decals & Antenna Broadcast Setup

Apply the two-tone industrial paint scheme, verify antenna broadcast power (1,500 m lease radius), and run zero-GC script initialization.

---

## 7 · Hardware-to-Scripting Contract & Zero-GC PB Specs

### 7.1 Standardized Block Taxonomy & Group Memberships

| Group Name | Primary Managed Blocks | Hardware Function & Role |
|---|---|---|
| `REM Lift` | `REM LF Lift NW`, `NE`, `SW`, `SE` (4× Large Atmo) | 2,304 kN primary vertical lift propulsion |
| `REM Lat F` | `REM Lat Fwd` (1× Large Flat Atmo) | 230 kN Aft-pushing trim & stationkeeping |
| `REM Lat A` | `REM Lat Aft` (1× Large Flat Atmo) | 230 kN Forward-pushing trim & stationkeep |
| `REM Lat P` | `REM Lat Port` (1× Large Flat Atmo) | 230 kN Starboard-pushing lateral trim |
| `REM Lat S` | `REM Lat Stbd` (1× Large Flat Atmo) | 230 kN Port-pushing lateral trim |
| `REM Drills` | `REM DH Drill 1` … `8` (8× Small-Grid Drills) | 9×9 sb circular rotary cutting head |
| `REM Batteries`| `REM Battery 1, 2` (2× Large Batteries) | 2.16 MWh power core / 8.0 MW peak output |
| `REM Gyros` | `REM Gyro 1, 2` (2× Small Gyroscopes) | Pitch, roll, and yaw angular stabilization |
| `REM Cams` | `REM Cam Down` (Nose), `REM Cam Up` (Crown) | Alignment cameras for drilling & docking |
| `REM Lights` | `REM Light Down` (Spotlight) | Subsurface worksite illumination |
| `REM Logic` | `REM TMR Heartbeat/Return`, `REM EC Batt/Lift/Dock` | Native Rule C4 hardware failsafe ladder |

---

### 7.2 Zero-GC PB Architecture (`scripts/Drone.Remora/`)

The miner core script executes inside `[REM] PB Miner` on an `Update10` (kinematics/safety) and `Update100` (telemetry/rate) loop, adhering strictly to zero-allocation architecture:

1. **State Machine (`RemoraDrillerState`):**
   - `DOCKED_CHARGING`: Locked to Orca bay ceiling connector; batteries on Recharge; ore draining.
   - `DESCENT_BORE`: Descending vertically at 0.5-1.0 m/s with drills spinning and dampeners active.
   - `EXTRACTION_SWEEP`: Rotary wobble or spiral expansion to clear borehole pockets.
   - `CLIMB_OUT`: Ascending vertically to shaft collar with loaded ore buffer.
   - `DOCKING_CAPTURE`: Entering Orca bay throat, engaging waist clamp collar into jaws.
   - `EMERGENCY_ABORT`: Immediate drill shutdown, 100% lift thrust climb to `PREDOCK` waypoint.
2. **Strict Grid Scoping:** `b => b.CubeGrid == Me.CubeGrid` prevents querying host Orca blocks.
3. **Stone Handling:** Retains all stone; no ejectors; computes mass delta $dM/dt$.

---

### 7.3 Master IGC Network Protocol Dictionary

| Channel | Payload Schema | Direction | Purpose |
|---|---|---|---|
| `FLT.REM.STATUS` | `<DroneID>;<State>;<OreFill%>;<Batt%>;<Depth_m>;<Rate_kg_s>` | Broadcast Out | Real-time status to host Orca carrier |
| `FLT.DISP.LEASE` | `<DroneID>;<TargetGPS>;<BayID>;<MaxDepth_m>;<Deadline_s>` | Unicast In | Carrier deployment lease & parameters |
| `TMS.MINER.CMD` | `<DroneID>;<Command>;<Parameter>` | Unicast In | Remote override command (ABORT, HOLD, RTB)|

---

## 8 · Acceptance Testing Protocols (Suites A-G)

```
========================================================================================
ACCEPTANCE TESTING PROTOCOL: REMORA-01 VERTICAL SHAFT DRILLER
EXECUTION ENVIRONMENT: Space Engineers Earth-Like (1.0 g Gravity, 1.0 atm Density)
========================================================================================
```

### Suite A: Static Geometry & Weight Budget Check
- [ ] Airframe footprint measures exactly 11 × 11 sb (5.5 × 5.5 m).
- [ ] Total dry mass registers at ≤ 48.5 t (± 2.0%).
- [ ] Center of Mass (CoM) aligns within 0.1 sb of centerline axis (0, 0).

### Suite B: Vertical Lift & Attitude Stability Test
- [ ] Empty hover consumes ≤ 21% thrust on 4× Large Atmo thrusters.
- [ ] Max laden test (simulated 195.7 t mass) hovers stably at ≤ 83% thrust rating (T/W = 1.20).
- [ ] 4-face lateral Flat Atmo thrusters execute pure X/Z translation without inducing pitch/roll.

### Suite C: Continuous Borehole Clearance Test
- [ ] Descend 25 m straight down into voxel terrain with 8 drills active.
- [ ] Hull follows borehole smoothly without snagging or bouncing against shaft walls.
- [ ] Confirm 100% stone mass is retained inside inventory (zero gravel ejector dumps).

### Suite D: 4 sb Thermal Flame Clearance Test
- [ ] Fire all 4 corner Large Atmo thrusters at 100% override for 20 seconds.
- [ ] Verify Level L6 collector manifold and drill tops sustain 0 damage.

### Suite E: Orca Bay Guide Jaw Capture & Docking
- [ ] Ascend vertically into simulated Orca bay at 2.0 m/s.
- [ ] Guide jaws swing inward to 45° and lock magnetic plates to waist collar.
- [ ] `REM Dock` locks cleanly into overhead connector; batteries switch to Recharge.

### Suite F: Native Rule C4 Failsafe Failover Test
- [ ] Kill `REM PB Miner` while drilling at 20 m depth.
- [ ] `REM TMR Heartbeat` trips after 5.0 s: drills shut off immediately, dampeners engage.
- [ ] `REM TMR Return` engages autopilot and ascends vertically to shaft collar.

### Suite G: Complete Autonomous Drilling Cycle
- [ ] Drone receives lease from Orca, undocks, descends 30 m, extracts 140 t ore, climbs out, docks into jaws, and offloads ore in < 60 s.

---

## 9 · Master Block Summary

| Block Name / Type | Total Qty | Subsystem Group | Primary Function |
|---|---|---|---|
| **Small-Grid Drill** | 8 | `REM Drills` | 8-drill circular rotary cutter head (9×9 sb ring) |
| **Large Atmospheric Thruster (Sm)**| 4 | `REM Lift` | 2,304 kN primary vertical lift propulsion |
| **Large Flat Atmo Thruster (Sm)** | 4 | `REM Lat F/A/P/S` | 920 kN 4-face cardinal lateral stationkeeping |
| **Large Battery (Small Grid)** | 2 | `REM Batteries` | 2.16 MWh power core / 8.0 MW peak output |
| **Small Gyroscope** | 2 | `REM Gyros` | Pitch, roll, and yaw angular stabilization |
| **Connector (Full-Size Small)** | 1 | `REM Dock` | Crown-mounted docking & high-speed ore offload |
| **Conveyor Hub 3×3 sb** | 4 | Conveyor Network | Spine junctions and 8-port collector manifold |
| **Conveyor Tube (Small Grid)** | 20 | Conveyor Network | Vertical spine tube and radial feeder lines |
| **Remote Control (Small Grid)** | 1 | `REM RC Main` | Autonomous navigation & autopilot return |
| **Programmable Block (Small Grid)**| 1 | `REM PB Miner` | Executes `Drone.Remora` zero-GC script |
| **Timer Block (Small Grid)** | 2 | `REM Logic` | Heartbeat watchdog (5.0 s) & return autopilot |
| **Event Controller (Small Grid)** | 3 | `REM Logic` | Native battery, lift, and dock triggers |
| **Radio Antenna (Small Grid)** | 1 | `REM Antenna` | 1,500 m IGC fleet communications lease link |
| **Camera (Small Grid)** | 2 | `REM Cams` | Downward nose eye + upward crown docking reticle |
| **Spotlight (Small Grid)** | 1 | `REM Lights` | Subterranean worksite illumination |
| **Heavy Armour Blocks** | 16 | Armor & Collar | Waist magnetic clamp contact collar pads |
| **Light Armour Blocks & Slopes** | ~68 | Fairings & Hull | Aerodynamic fairings, guide slopes, nacelle caps |

---

## Chapter 3 · MULE-01 Heavy Freight Shuttle

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MULE-01 Heavy Freight Shuttle (MUL-01)**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 25, 34, 41, 42, 43, 44, 45, 51, 56, 62, 66 & 68)
* **Role:** Autonomous high-capacity bulk ore shuttle and rapid peer-resupply transport. Transports 650 t of raw ore (or specialized mission resupply kits) on high-speed point-to-point transit corridors between remote extraction sites (Orca Carrier MP, Mastodon MKI Consist) and the regional Ground Base & Refinery Complex.
* **Architecture:** Compact 7 × 7 B (17.5 × 17.5 m) square lifting slab with a 3 B (7.5 m) profile:
  1. **Core Payload Bay (X −1 to +1, Z −1 to +1, L1-L3):** Standard 3×3×3 B Large Cargo Container providing 1,386,000 L volume (at 3× inventory) with a direct, zero-sorter downward docking connector (`MUL Dock Belly`) at L1.
  2. **Cardinal Lift Cross (N, S, E, W):** 4× Large Flat Atmospheric Thrusters mounted flush around the hold perimeter, aligning Center of Lift perfectly with the geometric Center of Mass (CoM).
  3. **Corner Power & Avionics Pockets:** 4× Large Batteries (12 MWh pure electric), dual large gyroscopes, autonomous remote control core, and failover safety timers.
  4. **Waist Clamping Strike Rails (X ±3, L2):** 3-block continuous flush heavy armor rub rails designed for automated capture by motorized Gantry Cradles on carrier and base decks.
* **Grid:** Large Grid (1 B = 2.5 m) · **Footprint:** 7 W × 7 L × 3 H B (17.5 × 17.5 × 7.5 m).
* **Mass:** Dry Mass `≈ 142.8 t` · Maximum Payload Ore `650.0 t` · Gross Laden Mass `≈ 792.8 t`.
* **Propulsion:** Pure Electric Atmospheric (10.4 MN Downward Lift + 6.48 MN 6-Axis Maneuvering Array).

---

## 0 · Reference Grid, Coordinate System & Design Envelope

### 0.1 · Spatial Datum & Alignment Triad
All coordinates throughout this chapter reference the geometric centroid of the central Large Cargo Container:
```
           +Y (Dorsal / Roof Deck, L3)
           ▲
           │   +Z (Bow / Forward Flight)
           │  ▲
           │ /
           │/
───────────┼───────────► +X (Starboard Wing / Rail)
          /│
         / │
        /  │
       ▼   ▼
     −Z    −Y (Ventral / Belly Docking, L1)
   (Stern)
```
* **Origin [0, 0, 0]:** Centroid of `MUL Cargo Hold` (Large Cargo Container) at Level L2.
* **X-Axis (Beam):** Port (−3 B / −7.5 m) to Starboard (+3 B / +7.5 m). Total beam = 7 B (17.5 m).
* **Y-Axis (Height):** Ventral skid bottom (Level L1 / −3.75 m) to dorsal catwalk (Level L3 / +3.75 m). Total height = 3 B (7.5 m).
* **Z-Axis (Length):** Stern (−3 B / −7.5 m) to Bow (+3 B / +7.5 m). Total length = 7 B (17.5 m).
* **Center of Mass (CoM):**
  * Dry CoM: `[X: 0.00, Y: 0.00, Z: 0.00]` (Dead center of cargo hold).
  * Laden CoM (650 t ore): `[X: 0.00, Y: 0.00, Z: 0.00]`. Zero pitch/roll tip-torque under all loading conditions.

---

### 0.2A · Space Engineers 10-Slot Paint Palette Assignments
Standardized finish and color assignments for MULE-01 fabrication:

| Slot # | Color Name | Hex Code | RGB | Finish | Functional Subsystem / Target Surface |
|:---:|---|:---:|:---:|---|---|
| **0** | Frame Shadow | `#1E242B` | `30, 36, 43` | Battered | Structural chassis ribs, belly skids, keel brackets. |
| **1** | Hull Olive | `#3B4438` | `59, 68, 56` | Clean | Outer protective cowlings, lateral fuselage flanks. |
| **2** | Armor Drab | `#4A5243` | `74, 82, 67` | Welder | Structural corner bulkheads, battery bay covers. |
| **3** | Core Hold Gray | `#2F353B` | `47, 53, 59` | Clean | Central 3×3×3 B Large Cargo Container shell. |
| **4** | Pure White Lift | `#F5F6FA` | `245, 246, 250` | Clean | Cardinal lift thruster cowlings (visual balance check). |
| **5** | Hazard Yellow | `#E5A93C` | `229, 169, 60` | Heavy Rust | Waist clamp rub rails (Gantry Cradle strike zone). |
| **6** | Warning Red | `#A83232` | `168, 50, 50` | Clean | Downward exhaust nozzle borders and lift boundaries. |
| **7** | Battery Cobalt | `#2C3E50` | `44, 62, 80` | Clean | Corner electrical power bays and battery casings. |
| **8** | Avionics Navy | `#1F2D3D` | `31, 45, 61` | Clean | Dorsal navigation bay, RC housing, and PB deck. |
| **9** | Beacon White | `#ECF0F1` | `236, 240, 241` | Neon | Status marker lights and belly camera searchlight. |

---

### 0.2B · Exterior Livery & Two-Tone Color Blocking Map

```
TOP-DOWN DORSAL LIVERY (Level L3, Looking −Y):
      X: −3   −2   −1    0   +1   +2   +3
  Z: +3 ┌────┬────┬═════════════┬────┬────┐  BOW (+Z)
        │CB 7│NV 8│   PW 4      │NV 8│CB 7│  ◄── Corner Battery / Avionics (Cobalt/Navy)
  Z: +2 ├────┴────┤ LIFT FORWARD├────┴────┤
        │         ├─────────────┤         │  ◄── Pure White Cardinal Lift Wings
  Z: +1 │  PW 4   │             │  PW 4   │
        │ LIFT P  │   CH 3      │ LIFT S  │
     0  │[HZ 5]   │ CARGO CORE  │ [HZ 5]  │  ◄── Orange Hazard Waist Rub Rails (X ±3)
        │ LIFT P  │  (3×3×3 B)  │ LIFT S  │
  Z: −1 │  PW 4   │             │  PW 4   │
        ├─────────┼─────────────┼─────────┤
  Z: −2 │         │  LIFT AFT   │         │  ◄── Pure White Cardinal Lift Wings
  Z: −3 ├────┬────┤   PW 4      ├────┬────┤
        │CB 7│NV 8│═════════════│NV 8│CB 7│  ◄── Corner Battery / Avionics (Cobalt/Navy)
        └────┴────┴═════════════┴────┴────┘  STERN (−Z)

PORT ELEVATION LIVERY (Looking +X at Port Flank):
      Z: −3       −2       −1        0       +1       +2       +3
  L3    ┌────────┬───────────────────────────────────┬────────┐  ROOF (L3)
        │ NV 8   │   Small Maneuvering Array (OL 1)  │  NV 8  │
  L2    ╞════════╧═══════════════════════════════════╧════════╡  MIDLINE (L2)
        │ [HAZARD 5] FLUSH GANTRY CRADLE CLAMP RUB RAIL [HZ 5]│
  L1    └───┬────────┬───┬───────────────────┬───┬────────┬───┘  BELLY (L1)
           [SKID 0]     [NOZZLE 6]          [NOZZLE 6]    [SKID 0]
```

---

### 0.4B · 3/4 Isometric Perspective Wireframes

#### View 1 · Dorsal Bow-Port 3/4 Isometric Wireframe (Flight Stance)
```
                  [BOW NOZZLE FAIRING]
                     +Z  ▲
                        / \
                       /   \
          ┌═══════════▼═════▼═══════════┐
         / │      MUL Lift Fwd (3×2)    │ \
        /  │         (Pure White)       │  \
       /   └═════════════╤══════════════┘   \
      /                  │                   \
┌────▼──────┐     ┌──────┴──────┐     ┌───────▼───┐
│ MUL Bat NW│     │             │     │ MUL Bat NE│
│(Cobalt L2)│     │  MUL Cargo  │     │(Cobalt L2)│
├───────────┤     │  Hold Core  │     ├───────────┤
│[RUB RAIL P│     │  (3×3×3 B)  │     │[RUB RAIL S│
│ Hazard L2]│◄────┤  Charcoal   ├────►│ Hazard L2]│
├───────────┤     │             │     ├───────────┤
│ MUL Bat SW│     │             │     │ MUL Bat SE│
│(Cobalt L2)│     └──────┬──────┘     │(Cobalt L2)│
└────▲──────┘            │            └───────▲───┘
      \                  ▼                   /
       \   ┌═════════════╧══════════════┐   /
        \  │      MUL Lift Aft (3×2)    │  /
         \ │         (Pure White)       │ /
          └═════════════════════════════┘
                        \   /
                         \ /
                          ▼ −Z (STERN)
```

#### View 2 · Ventral Belly 3/4 Isometric Wireframe (Docking Stance)
```
          [SKID NW]                     [SKID NE]
           (2×1 Hvy)                     (2×1 Hvy)
             \                             /
              ▼                           ▼
        ┌───────────┐               ┌───────────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └─────┬─────┘               └─────┬─────┘
              │                           │
              │     ┌───────────────┐     │
              │     │  MUL Cam Belly│     │
              │     │  [X0, Z+1]    │     │
              │     ├───────────────┤     │
              └───► │MUL Dock Belly │ ◄───┘
                    │(Large Con DOWN│
                    │ Recessed 0.5B)│
              ┌───► ├───────────────┤ ◄───┐
              │     │[0, 0, L1]     │     │
              │     └───────────────┘     │
              │                           │
        ┌─────┴─────┐               ┌─────┴─────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └───────────┘               └───────────┘
              ▲                           ▲
             /                             \
          [SKID SW]                     [SKID SE]
```

---

### 0.4C · Transverse Hull Station Profiles

#### Station 1 · Forward Wing & Gyro Bays (Z = +2.5 B)
Transverse section looking forward (+Z) through the bow thruster wing and forward avionics bays:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L3      ┌────────┬────────┬═════════════════════┬────────┬────────┐
         │Lat Port│Gyro P1 │ Small Maneuver Fwd  │Gyro S1 │Lat Stbd│
 L2      ├────────┼────────┼─────────────────────┼────────┼────────┤
         │Bat NW  │Heavy 21│ MUL Lift Fwd (3×2)  │Heavy 21│Bat NE  │
 L1      └───┬────┴────────┴───┬─────────────┬───┴────────┴────┬───┘
             ▼                 ▼▼▼         ▼▼▼                 ▼
          [SKID NW]           [EXHAUST NOZZLE]              [SKID NE]
```

#### Station 2 · Midship Hold & Waist Clamping Rub Rails (Z = 0.0 B)
Transverse section looking forward (+Z) through the physical Center of Mass:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L3      ┌────────┬───────────────────────────────────────┬────────┐
         │Catwalk │ Top Inspection Port (Manual Access)   │Catwalk │
 L2      ╞════════╪════════════╤═════════════╤════════════╪════════╡
         │[RUB P] │MUL Lift P  │  MUL Cargo  │MUL Lift S  │[RUB S] │
 L1      └───┬────┴────────────┴──────┬──────┴────────────┴────┬───┘
             │                        │                        │
       Gantry Clamp              MUL Dock Belly           Gantry Clamp
       Strike Surface            Connector DOWN           Strike Surface
       (Hazard Rail)             Recessed 0.5 B           (Hazard Rail)
```

#### Station 3 · Stern Wing & Power Sub-Bay (Z = −2.5 B)
Transverse section looking forward (+Z) through the aft thruster wing and telemetry spire:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L3      ┌────────┬────────┬═════════════════════┬────────┬────────┐
         │Lat Port│[MUL PB]│   [MUL RC] [ANT]    │[MUL EC]│Lat Stbd│
 L2      ├────────┼────────┼─────────────────────┼────────┼────────┤
         │Bat SW  │Heavy 21│ MUL Lift Aft (3×2)  │Heavy 21│Bat SE  │
 L1      └───┬────┴────────┴───┬─────────────┬───┴────────┴────┬───┘
             ▼                 ▼▼▼         ▼▼▼                 ▼
          [SKID SW]           [EXHAUST NOZZLE]              [SKID SE]
```

---

### 0.5 · Master Conveyor Routing Schematic

The Mule features a zero-sorter, ultra-low resistance direct gravity-extraction loop. Sorter-assisted suction is handled entirely by host receiving pads (`ORC MP Conn` on Orca carrier, `GND PAD Conn` on Ground Base, and `MAS MP Conn` on Mastodon MKI):

```
                       [MUL Cargo Hold]
           (Standard Large Cargo Container 3×3×3 B)
           (Located X −1..+1, Z −1..+1, Levels L1-L3)
           (Hold Capacity: 1,386,000 L @ 3× Inventory)
           (Physical Manual Maintenance Port on Dorsal L3)
                                 │
                                 ▼ (Bottom Center Large Port)
                          [MUL Dock Belly]
           (Large Grid Connector · Faces Straight DOWN)
           (Flush L1 @ X 0, Z 0 · Recessed 0.5 B above Skids)
                                 │
        ═════════════════════════╪═════════════════════════  Pad Touchdown Plane
                                 │
                                 ▼ (Mates to Host Pad)
         [ORC MP Conn]    OR    [GND PAD Conn]    OR    [MAS MP Conn]
        (Orca Roof Deck)        (Ground Base)           (Mastodon Roof)
```

---

### 0.6 · Master Electrical Single-Line Schematic & 5 Power Modes

```
                  4× LARGE / WARFARE BATTERIES (L2 Pockets)
                    12.0 MWh Stored Capacity · 48.0 MW Max Surge
                                  │
                                  ▼
                          PRIMARY 48 MW BUS
        ┌─────────────────────────┼─────────────────────────┐
        │                         │                         │
        ▼                         ▼                         ▼
  CARDINAL LIFT ARRAY       6-AXIS MANEUVER           AVIONICS & CONTROL
  4× Large Flat Atmo        10× Small Atmo            2× Large Gyros (0.80 MW)
  (26.80 MW Max Surge)      (6.48 MW Max Surge)       1× RC + 1× PB (0.05 MW)
  [Empty Hover: 3.62 MW]    [Braking Load: 2.60 MW]   Antenna + 2× Cam (0.02 MW)
  [Laden Hover: 20.05 MW]   [Push-Down: 1.30 MW]      3× EC + 2× Timer (0.00 MW)
```

#### Operational Energy Modes
1. **Mode 1 · Docked Shore Charge (Recharge = ON):**
   * Total Draw: `0.00 MW` · Shore Intake: `48.0 MW` · Full 0 to 100% recharge in `15.0 minutes`.
2. **Mode 2 · Empty Ballast Transit (142.8 t):**
   * Lift Load: 13.5% (3.62 MW) · Maneuver: 1.30 MW · Total: `4.92 MW` · Endurance: `2.44 hours`.
3. **Mode 3 · Fully Laden Hover (792.8 t · 650 t Ore):**
   * Lift Load: 74.8% (20.05 MW) · Maneuver: 2.60 MW · Total: `22.72 MW` · Endurance: `31.7 minutes`.
4. **Mode 4 · High-G Touchdown Flare / Push-Down Braking:**
   * 4× Flat Atmo (100% burst: 26.80 MW) + 2× Push-Down Atmo (1.30 MW) + Gyros (0.80 MW) = `28.90 MW` peak surge. Battery headroom margin = `+19.10 MW` (66% reserve).
5. **Mode 5 · Emergency Deadman Hover / Safe Return:**
   * Battery stored $\le 15\%$. Non-essential systems cut, speed capped at 30 m/s, autonomous return to nearest gantry cradle.

---

### 0.7 · Master Bill of Materials (BOM) & Physics Budget

| Category | Component Description | Block Qty | Mass (t) | PCU | Critical Mounting / Orientation |
|---|---|:---:|:---:|:---:|---|
| **Chassis** | Large Cargo Container (3×3×3 B) | 1 | 28.5 | 250 | Centered at [X0, Y0, Z0], L1-L3 |
| **Chassis** | Heavy Armor Corner Landing Skids (2×1) | 16 | 24.0 | 256 | L1 belly corners [X ±3, Z ±3] |
| **Chassis** | Heavy Armor Clamp Strike Rub Rails | 12 | 18.0 | 192 | L2 outer rub rails [X ±3, Z −1..+1] |
| **Chassis** | Light Armor Hull Fairings & Catwalks | 40 | 12.0 | 400 | L2-L3 cowlings, handrails, brow |
| **Propulsion** | Large Flat Atmospheric Thrusters (3×2 B) | 4 | 44.0 | 320 | N, S, E, W cardinal wings, facing DOWN |
| **Propulsion** | Small Atmospheric Thrusters (1×1 B) | 10 | 18.0 | 250 | 2 Bow, 2 Stern, 2 Port, 2 Stbd, 2 Roof |
| **Power** | Large Batteries (or Warfare Batteries) | 4 | 20.0 | 80 | 4 corner bays (3 MWh each = 12 MWh) |
| **Attitude** | Large Gyroscopes (33.6 MN·m each) | 2 | 5.6 | 100 | Forward corner bays [X ±2, Z +3, L2] |
| **Docking** | Large Grid Connector | 1 | 4.2 | 150 | Centered at [X0, Z0, L1], facing DOWN |
| **Avionics** | Remote Control Block | 1 | 0.5 | 25 | Aft bay [X0, Z −2, L3], Fwd = +Z |
| **Avionics** | Programmable Block | 1 | 0.5 | 25 | Aft bay [X −1, Z −2, L3] (`[MUL] PB Shuttle`) |
| **Avionics** | Radio Antenna | 1 | 0.8 | 100 | Aft bay [X +1, Z −2, L3] (5,000 m range†) |
| **Avionics** | Fixed Cameras | 2 | 0.1 | 20 | 1 Belly [X0, Z+1, L1], 1 Bow [X0, Z+3, L3] |
| **Logic** | Event Controllers | 3 | 0.3 | 75 | Aft bay L3 (Cargo, Batt, Dock) |
| **Logic** | Timer Blocks | 2 | 0.3 | 50 | Aft bay L3 (Heartbeat, Return) |
| **Lighting** | Aviation Strobe / Nav Lights | 4 | 0.1 | 40 | Corner cowlings L3 |
| **TOTAL** | **Complete MULE-01 Heavy Freight Shuttle** | **104** | **≈ 157.0 t** | **2,333** | **Zero-sorter, pure electric** |

#### Physics & Gravitational Lift Margin
* **Dry Mass:** `157.0 t` (Structure + Blocks)
* **Maximum Payload Ore:** `650.0 t` (1,386,000 L @ 3× inventory)
* **Gross Laden Mass:** `807.0 t`
* **Downward Lift Thrust (4× Large Flat Atmo):** $4 \times 2.6\text{ MN} = 10.40\text{ MN}$
* **Gravity Downward Force (Earth 1.0 g = 9.81 m/s²):**
  * Empty: $157.0\text{ t} \times 9.81\text{ m/s}^2 = 1.54\text{ MN}$ (Hover Load = 14.8%)
  * Laden: $807.0\text{ t} \times 9.81\text{ m/s}^2 = 7.92\text{ MN}$ (Hover Load = 76.2%)
* **Thrust-to-Weight (T/W) Ratio:**
  * Empty T/W: $10.40 / 1.54 = \mathbf{6.75}$
  * Laden T/W: $10.40 / 7.92 = \mathbf{1.313}$ (31.3% reserve margin in standard planetary atmosphere)

---

### 0.7B · Human Circulation, Catwalks & Maintenance Ergonomics
1. **Dorsal Catwalk & Service Ladder:** Level L3 features a recessed perimeter walkway with safety handrails surrounding the upper Small Atmo cluster.
2. **Manual Inventory Access Port:** The top port of `MUL Cargo Hold` is exposed via a flush trapdoor on Level L3, allowing engineers to manually deposit emergency tools, components, or uranium fuel without cycling docking connectors.
3. **Corner Boarding Rungs:** Retractable ladder rungs recessed into the heavy armor skirts at [X −3, Z −2, L1] and [X +3, Z −2, L1] allow EVA engineers to climb from planetary surface level onto the L3 deck when grounded.

---

## Phase 3-1 · Central Ore Hold & Belly Docking Interface (L1-L3)

### Step 3.1 · Large Cargo Core & Ventral Connector (L1-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Central Hold  ·  Footprint: X −1..+1, Z −1..+1  ·  Level: L1-L3 │
│ [PARTS TRAY] 1× Large Cargo Container (3×3×3 B), 1× Connector (Large), 1× Camera     │
│ [PALETTE] Cargo Shell: Slot 3 (#2F353B), Connector: Slot 0 (#1E242B)                 │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Place 1 standard Large Cargo Container (3×3×3 B) `MUL Cargo Hold` centered precisely at coordinate `[X: 0, Y: 0, Z: 0]`, spanning Levels L1 through L3.
2. Ensure the bottom center large conveyor port faces directly DOWN at Level L1.
3. Attach 1× Large Grid Connector `MUL Dock Belly` to this bottom port, oriented facing straight DOWN (−Y).
4. Mount 1× Camera `MUL Cam Belly` recessed immediately adjacent to the connector at [X 0, Z +1, L1] pointing straight DOWN.

```
Plan view, Level L1 (Ventral Core):
      X: −1     0    +1
  Z: +1 ┌─────┬─────┬─────┐
        │     │[CAM]│     │   ◄── [CAM] = MUL Cam Belly (facing DOWN)
     0  │     │[CON]│     │   ◄── [CON] = MUL Dock Belly (Connector DOWN)
        │     │     │     │
  Z: −1 └─────┴─────┴─────┘
              (3×3 B)
```

---

### Step 3.2 · Belly Landing Skids & Skid Foundation (L1)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Landing Skids  ·  Footprint: X ±3, Z ±3  ·  Level: L1           │
│ [PARTS TRAY] 16× Heavy Armor 2×1 Slopes, 8× Heavy Armor Half Blocks                  │
│ [PALETTE] Finish: Battered  ·  Color: Slot 0 (#1E242B)                               │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Construct 4 corner landing skids at [X ±3, Z ±3] on Level L1.
2. Shape each skid using 2×1 Heavy Armor Slopes angled outward to form a broad 17.5 × 17.5 m stance.
3. Tie the skids to the central cargo hold using under-chassis Heavy Armor beams spanning along Z = 0 and X = 0.

```
Plan view, Level L1 (Skid Layout):
      X: −3   −2   −1    0   +1   +2   +3
  Z: +3 [SKID]···                 ···[SKID]  ◄── Heavy Armor 2×1 Skid (NW / NE)
         │       ┌─────────────┐       │
     0   │       │  MUL Cargo  │       │     ◄── Under-chassis tie beams
         │       └─────────────┘       │
  Z: −3 [SKID]···                 ···[SKID]  ◄── Heavy Armor 2×1 Skid (SW / SE)
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 3.1A: CONNECTOR RECESS CLEARANCE**
> Stand at ground level and sight across the bottom faces of all 4 corner skids:
> 1. Verify `MUL Dock Belly` sits recessed exactly **0.5 blocks (1.25 m)** above the skid contact plane.
> 2. If the connector hangs flush with or below the skids, landing on uneven terrain will crush the connector!

---

### Phase 3-1 Visual Milestone
```
Standing 15 m away looking at Starboard Flank (Level L1-L3):
       L3  ┌───────────────────────────────────┐  ◄── Large Cargo Container Top
           │                                   │
       L2  │       LARGE CARGO CONTAINER       │      (3×3×3 B Center Core)
           │                                   │
       L1  └──[SKID]───────[BELLY]───────[SKID]┘  ◄── Corner Landing Skids
```

---

## Phase 3-2 · Symmetrical Cardinal Lift Propulsion (L1-L2)

### Step 3.3 · Cardinal Lift Thruster Cross (L1-L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Cardinal Lift  ·  Wings: N, S, E, W  ·  Level: L1-L2            │
│ [PARTS TRAY] 4× Large Flat Atmospheric Thrusters (3×2 B · Large Grid)                │
│ [PALETTE] Cowling: Slot 4 Pure White (#F5F6FA), Nozzle Border: Slot 6 (#A83232)      │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Mount 4 Large Flat Atmospheric Thrusters directly against the 4 vertical faces of `MUL Cargo Hold`, exhausting straight DOWN:
1. `MUL Lift Fwd` : Mounted on Bow face [Z +2 to +3, X −1 to +1, L1-L2].
2. `MUL Lift Aft` : Mounted on Stern face [Z −2 to −3, X −1 to +1, L1-L2].
3. `MUL Lift Port` : Mounted on Port face [X −2 to −3, Z −1 to +1, L1-L2].
4. `MUL Lift Stbd` : Mounted on Starboard face [X +2 to +3, Z −1 to +1, L1-L2].
5. Assign all 4 thrusters to terminal group `MUL Lift`.

```
Plan view, Level L2 (Cardinal Lift Cross):
      X: −3    −1     0    +1    +3
  Z: +3   ┌═════════════┐
          │MUL Lift Fwd │         ◄── Forward Wing (3×2 B)
  Z: +1  ┌┤ (Pure White)├┐
         │├─────────────┤│
     0   ││  MUL Cargo  ││        ◄── Port Wing (2×3 B) / Starboard Wing (2×3 B)
         ││  (3×3×3 B)  ││
  Z: −1  │├─────────────┤│
  Z: −3  └┤MUL Lift Aft ├┘
          │ (Pure White)│         ◄── Aft Wing (3×2 B)
          └═════════════┘
```

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 3.2A: DOWNWARD EXHAUST CLEARANCE**
> Ensure the bottom nozzle grills of all 4 Flat Atmo thrusters have a completely clear line of sight downward through Level L1. Any armor blocks placed directly underneath the nozzles will cause internal back-pressure damage!

---

## Phase 3-3 · Corner Power Pockets & Gyro Stabilization Bays (L1-L3)

### Step 3.4 · 4-Quadrant Large Battery Array (L1-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Power Core  ·  Pockets: NW, NE, SW, SE Corners  ·  Level: L1-L3 │
│ [PARTS TRAY] 4× Large Batteries (or Warfare Batteries · 3 MWh each)                  │
│ [PALETTE] Battery Shell: Slot 7 Cobalt (#2C3E50), Fairings: Slot 2 Armor Drab        │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Install 4 Large Batteries in the open 2 × 2 B corner cavities:
1. `MUL Battery NW` : Forward-Port corner [X −2 to −3, Z +2 to +3, L2].
2. `MUL Battery NE` : Forward-Starboard corner [X +2 to +3, Z +2 to +3, L2].
3. `MUL Battery SW` : Aft-Port corner [X −2 to −3, Z −2 to −3, L2].
4. `MUL Battery SE` : Aft-Starboard corner [X +2 to +3, Z −2 to −3, L2].
5. Assign all 4 to terminal group `MUL Batteries` (12.0 MWh aggregate capacity, 48.0 MW peak discharge).

```
Plan view, Level L2 (Power Pockets):
      X: −3   −2   −1     0    +1   +2   +3
  Z: +3  [BAT NW]     [MUL Lift Fwd]    [BAT NE]
  Z: +2  [ 3 MWh]                       [ 3 MWh]
         ········    ┌─────────────┐    ········
     0   [LIFT P]    │  MUL Cargo  │    [LIFT S]
         ········    └─────────────┘    ········
  Z: −2  [BAT SW]     [MUL Lift Aft]    [BAT SE]
  Z: −3  [ 3 MWh]                       [ 3 MWh]
```

---

### Step 3.5 · Flight Stabilization Gyroscopes (L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Attitude Control  ·  Pockets: Fwd P/S  ·  Level: L2             │
│ [PARTS TRAY] 2× Large Gyroscopes (33.6 MN·m each)                                    │
│ [PALETTE] Slot 0 Frame Shadow (#1E242B)                                              │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Install 1× Large Gyroscope `MUL Gyro Port` at [X −2, Z +2, L2] immediately aft of `BAT NW`.
2. Install 1× Large Gyroscope `MUL Gyro Stbd` at [X +2, Z +2, L2] immediately aft of `BAT NE`.
3. Assign both to group `MUL Gyros` (67.2 MN·m aggregate torque).

---

## Phase 3-4 · Waist Clamping Strike Rub Rails (L2)

### Step 3.6 · Heavy Armor Gantry Cradle Strike Rub Rails (L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Waist Rails  ·  Flanks: Port & Stbd  ·  Level: L2               │
│ [PARTS TRAY] 12× Heavy Armor Full Blocks (Large Grid)                                │
│ [PALETTE] Hazard Striping: Slot 5 Hazard Yellow (#E5A93C) with Heavy Rust            │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Per **Decision 45**, the Mule carries zero motorized clamping arms. External Gantry Cradles on host decks clamp directly onto the Mule's chassis:
1. Mount a 3-block continuous span of Heavy Armor Full Blocks along the Port flank at [X −3, Z −1 to +1, L2].
2. Mount a 3-block continuous span of Heavy Armor Full Blocks along the Starboard flank at [X +3, Z −1 to +1, L2].
3. Paint both spans in Palette Slot 5 (Hazard Yellow with chevron striping).

```
Section view, looking Bow at Port flank (X = −3):
  L3   ┌───────────────────────────┐
       │ Light Armor Cowling       │
  L2   ╞═══════════════════════════╡  ◄── [WAIST RUB RAIL PORT]
       │ Heavy Armor Strike Face   │      Flush magnetic clamping surface
  L1   ╞═══════════════════════════╡      engaged by Gantry Cradle plates
       │ Skid Base Line            │
       └───────────────────────────┘
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 3.3A: CRADLE STRIKE FLUSHNESS GATE**
> Verify the outer faces of the hazard rub rails at X = ±3 are completely smooth and flush with the surrounding armor. No conveyor hubs, buttons, or lights may protrude beyond X = ±3.0 m, or they will sheer off during cradle clamping!

---

## Phase 3-5 · 6-Axis Maneuvering Array & Roof Push-Down Thrusters (L2-L3)

### Step 3.7 · 6-Axis Small Atmospheric Thruster Perimeter (L2-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Maneuvering Array  ·  Perimeter & Roof  ·  Level: L2-L3         │
│ [PARTS TRAY] 10× Small Atmospheric Thrusters (Large Grid · 648 kN each)              │
│ [PALETTE] Slot 1 Hull Olive (#3B4438), Exhaust Nozzle Trim: Slot 6 (#A83232)         │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Mount 10 Small Atmospheric Thrusters to provide agile 6-axis translation and high-G push-down descent:
1. **Bow (+Z):** Mount 2× Small Atmo `MUL Lat Fwd 1, 2` facing Forward (exhausts +Z to push Aft) at [X ±1, Z +3, L3].
2. **Stern (−Z):** Mount 2× Small Atmo `MUL Lat Aft 1, 2` facing Aft (exhausts −Z to push Fwd) at [X ±1, Z −3, L3].
3. **Port (−X):** Mount 2× Small Atmo `MUL Lat Port 1, 2` facing Port (exhausts −X to push Stbd) at [X −3, Z ±1, L3].
4. **Starboard (+X):** Mount 2× Small Atmo `MUL Lat Stbd 1, 2` facing Starboard (exhausts +X to push Port) at [X +3, Z ±1, L3].
5. **Dorsal Roof (+Y):** Mount 2× Small Atmo `MUL Thrust Down 1, 2` facing UP (exhausts +Y to push straight DOWN) at [X ±1, Z 0, L3].
6. Assign all 10 to terminal group `MUL Maneuver`.

```
Plan view, Level L3 (Roof Line Perimeter & Push-Down Thrusters):
             Bow (Z = +3)
          ◄── [MUL Lat Fwd ×2] ──►   (Exhausts Fwd / Brakes Bow)
       ┌────────────────────────────┐
       │     [Push Down 1]          │
  Port │ [MUL Lat Port ×2]          │ Stbd [MUL Lat Stbd ×2]
  (−X) │                            │ (+X)
       │     [Push Down 2]          │
       └────────────────────────────┘
          ◄── [MUL Lat Aft ×2] ──►   (Exhausts Aft / Brakes Stern)
             Stern (Z = −3)
```

---

## Phase 3-6 · Autonomous Avionics Core, Sensors & Maintenance Deck (L3)

### Step 3.8 · Aft Avionics Vault & Comms (L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Avionics  ·  Aft Upper Deck  ·  Level: L3, Z −2 to −3           │
│ [PARTS TRAY] 1× Remote Control, 1× PB, 1× Radio Antenna, 1× Camera, 3× EC, 2× Timer  │
│ [PALETTE] Slot 8 Avionics Navy (#1F2D3D)                                             │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1× Remote Control `MUL RC` at [X 0, Z −2, L3]. Ensure Forward points Bow (+Z) and Up points Roof (+Y).
2. Mount 1× Programmable Block `[MUL] PB Shuttle` at [X −1, Z −2, L3].
3. Mount 1× Radio Antenna `MUL Antenna` at [X +1, Z −2, L3]. Set broadcast range to `5,000 m`†.
4. Mount 1× Camera `MUL Cam Fwd` at [X 0, Z +3, L3] facing Bow (+Z).
5. Mount logic block array along the northern equipment bulkhead:
   * `MUL EC Cargo` : Event Controller monitoring payload mass.
   * `MUL EC Batt` : Event Controller monitoring battery percentage.
   * `MUL EC Dock` : Event Controller monitoring belly connector lock.
   * `MUL TMR Heartbeat` : Safety watchdog timer set to `5.0 s`.
   * `MUL TMR Return` : Automated departure timer set to `2.0 s`.

```
Plan view, Level L3 (Aft Avionics Array):
      X: −2     −1       0       +1     +2
  Z: −2  [EC]  │[PB] │  [RC]  │ [ANT] │ [TMR]
  Z: −3  [EC]  │     │ [CAT]  │       │ [TMR]
```

---

## Phase 3-7 · Master Control Logic, AI Scripting Contract & Software Abstraction

### 1. Terminal Group Taxonomy
* `MUL Lift` : All 4 Large Flat Atmo Thrusters (`MUL Lift Fwd`, `Aft`, `Port`, `Stbd`).
* `MUL Maneuver` : All 10 Small Atmo Thrusters (Bow, Stern, Port, Stbd, Down).
* `MUL Batteries` : All 4 Large Batteries (`MUL Battery NW`, `NE`, `SW`, `SE`).
* `MUL Gyros` : All 2 Large Gyroscopes (`MUL Gyro Port`, `MUL Gyro Stbd`).
* `MUL Cams` : Both navigation cameras (`MUL Cam Belly`, `MUL Cam Fwd`).

---

### 2. Event Controller Automation Ladder (Rule C4)

| Controller | Monitored Parameter | Threshold / Event | Slot 1 Action (True) | Slot 2 Action (False) |
|---|---|---|---|---|
| `MUL EC Cargo` | Grid Total Mass | Mass $\ge 800.0\text{ t}$ (Payload full) | Start `MUL TMR Return` (2 s) | · |
| `MUL EC Batt` | `MUL Batteries` Stored % | Stored Power $\le 20.0\%$ | Trigger Emergency RTB / Pad abort | · |
| `MUL EC Dock` | `MUL Dock Belly` State | Connector Status = Locked | `MUL Lift` OFF · `MUL Batteries` Recharge | `MUL Lift` ON · `MUL Batteries` Auto |

---

### 3. Native Block Failsafe Matrix (Zero-PB Safety)
If `[MUL] PB Shuttle` crashes, freezes, or encounters an uncaught runtime exception:
1. `MUL TMR Heartbeat` (5.0 s loop) fails to receive a PB reset pulse and expires.
2. Immediate hardwired failsafe actions triggered by timer:
   * Remote Control Autopilot `Collision Avoidance` = ON.
   * Remote Control `Dampeners` = ON.
   * Gyro and Thrust Overrides = OFF.
   * Shuttle maintains stationary hover at current altitude until operator overrides via C2 console.
3. If docked at a Gantry Cradle when failure occurs:
   * Belly connector remains magnetically locked.
   * `MUL Lift` remains locked OFF, preventing accidental thruster tear-away against magnetic clamp arms.

---

### 4. IGC Broadcast Network Dictionary

```
TMS.MULE.STATUS (Broadcast every 1.66 s):
MUL-01;<state_enum>;<ore_mass_t>;<battery_pct>;<destination_gps>
Example: MUL-01;LADEN_TRANSIT;648.5;92.0;GPS:GND_PAD:12450.2:-34200.5:810.0:

TMS.MULE.CALL (Inbound dispatch trigger):
<requester_id>;<pad_id>;<payload_type>;<urgency>
Example: ORC-01;ORC_MP;RARE_ORE;HIGH

TMS.LOGISTICS.JIT (Inbound pre-dispatch trigger from Base C2 or Mastodon MKI):
DISPATCH;MUL-01;<pad_id>;<payload_type>
Example: DISPATCH;MUL-01;PAD-1;RARE_ORE
```

---

## Phase 3-8 · Commissioning Checklist & Rigorous Test Protocols (RIG Test Creative)

### Suite A · Dry Mass & CoM Zero-Tip Balance Calibration
* [ ] **Dry Mass Verification:** Spawn MULE-01 in creative; terminal reports dry mass `≈ 150-160 t`.
* [ ] **Zero-Torque Hover:** Lift off to 50 m altitude in Earth 1.0 g; release dampeners; confirm pitch and roll remain `0.00°` (CoM perfectly aligned with center of lift).
* [ ] **Empty Hover Power:** Verify 4× Flat Atmo thrusters operate at `14-16%` load (`≈ 3.6-4.2 MW` draw).

### Suite B · 650 t Ore Gravitational Lift Margin Test
* [ ] **Full Ore Spawn:** Load exactly 650 t of iron ore into `MUL Cargo Hold` (Gross mass `≈ 807 t`).
* [ ] **Laden Hover Power:** Verify 4× Flat Atmo thrusters hover cleanly at `76-78%` load (`≈ 20.0-20.5 MW` draw). Confirm T/W margin `≥ 1.30`.
* [ ] **Vertical Climb Rate:** Apply full upward collective; verify vertical climb rate reaches `≥ 12.0 m/s` within 3.0 seconds.

### Suite C · High-G Push-Down Braking & Flare Test
* [ ] **Vertical Descent Acceleration:** From 300 m altitude, apply downward thruster override (`MUL Thrust Down 1, 2`); verify descent acceleration reaches `13.5 m/s²`.
* [ ] **Terminal Touchdown Flare:** At 50 m altitude, cut push-down thrusters and engage full lift collective; verify descent arrests safely before ground impact.

### Suite D · Orca Roof MP Touchdown & Belly Connector Latch
* [ ] **Approach Alignment:** Guide Mule over Orca carrier roof pad (`ORC MP Conn`); verify `MUL Cam Belly` view aligns center marker.
* [ ] **Touchdown & Lock:** Touch down on pad; verify `MUL Dock Belly` locks cleanly with corner skids resting flat on blast deck.
* [ ] **Connector Recess Verification:** Confirm belly connector experiences zero mechanical bounce or compression damage.

### Suite E · Gantry Cradle Motorized Waist Clamping Engagement
* [ ] **Cradle Swing:** Trigger host Gantry Cradle hinges (swinging inward 30°); verify magnetic plates strike flush against `[WAIST RUB RAIL PORT/STBD]` at Level L2.
* [ ] **Magnetic Latch:** Lock magnetic plates; turn `MUL Lift` OFF; confirm Mule is clamped rigidly to carrier deck during 50 m/s carrier evasive maneuvers.

### Suite F · High-Speed Ore Transfer & Turnaround (< 45 s)
* [ ] **Rapid Drain Test:** Engage host pad intake sorters (`ORC MP Sorter` / `GND PAD Sorter`); verify 650 t ore evacuates through `MUL Dock Belly` in $< 45\text{ seconds}$† without conveyor back-pressure.
* [ ] **Departure Sequence:** On empty signal (`MUL EC Cargo`), verify Gantry Cradle unclutches, `MUL Lift` engages, and shuttle executes clean vertical ascent.

### Suite G · Point-to-Point 100 m/s Transit Corridor Navigation
* [ ] **Waypoint Cruise:** Fly 5 km transit corridor between Orca and Ground Base at 100 m/s; verify 10× Small Atmo array maintains course heading with zero lateral slip.
* [ ] **Battery Consumption:** Verify total energy consumed over 5 km laden flight is $< 1.8\text{ MWh}$ ($< 15\%$ battery capacity).

### Suite H · Ground Base Pad Touchdown, Fast Shore Recharge & Turnaround
* [ ] **Base Terminal Capture:** Approach `GND PAD`; auto-engage gantry clamping arms; lock `GND PAD Conn`.
* [ ] **Fast Shore Recharge:** Verify 4× Large Batteries switch to `Recharge` and pull full 48.0 MW shore power, reaching 100% state of charge in $< 15\text{ minutes}$.

### Suite I · Native Heartbeat Loss & Emergency Deadman Hover Fallback
* [ ] **Simulated PB Crash:** Recompile `[MUL] PB Shuttle` with a crash script during transit; verify `MUL TMR Heartbeat` times out at 5.0 s, disengages overrides, activates dampeners, and arrests flight into a stable altitude hold.

---

## Phase 3-9 · Unverified Values Tracking (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V37:** Large Flat Atmospheric Thruster large-grid sea-level thrust rating (verify 2.6 MN rating and power consumption).
* **V38:** Ore transfer drain rate between large-grid containers through a single connector (measure seconds per 650 t).
* **V48:** MULE-01 laden descent deceleration on 2× Small Push-Down Atmo thrusters ($13.5\text{ m/s}^2$†).

---

## Chapter 12 · KESTREL-01 Prospector Scout Drone (KES-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **KESTREL-01 Prospector Scout Drone (KES-01)**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 25, 31, 41, 42, 50, 52, 56, 62 & 66)
* **Role:** High-speed autonomous geological prospecting, ore vein reconnaissance, and long-range GPS waypoint survey for both the ORCA-01 carrier and the MASTODON-01 heavy excavator consist. Docks interchangeably in Orca Bay P2, Ground Base logistics apron, and Mastodon bow recessed pads (`MAS Welder Pad Kestrel 1, 2`).
* **Architecture:** Compact 7 × 9 × 5 sb (3.5 × 4.5 × 2.5 m) aerodynamic lifting body:
  1. **Belly Docking & Skid Foundation (Levels L1-L2):** Small Grid Connector (`KES Dock Conn`) mounted flush between twin heavy armor landing skids with downward camera (`KES Cam Ground`).
  2. **Core Power & Geological Sensor Trunk (Levels L2-L3):** Small-grid Ore Detector (`KES Ore Detector`), 1× Small Cargo Container (`KES Hold Samples`), 2× Small Grid Batteries (1.0 MWh), and 1× Small Nuclear Reactor (0.5 MW).
  3. **14-Thruster Omnidirectional Maneuver Ring (Levels L2-L3):** 4× Downward Lift Thrusters (320 kN lift · T/W ≈ 6.79), 2× Forward Drive, 2× Reverse Braking, 4× Lateral Translation, and 2× Down-Push Thrusters.
  4. **Avionics & Sensor Spine (Levels L3-L5):** Remote Control, Micro Gyroscope, `[KES] PB Scout`, 10 km Radio Antenna, high-zoom nose optics (`KES Cam Nose`), and dual forward searchlights.
* **Grid:** Small Grid (1 sb = 0.5 m) · **Footprint:** 7 W × 9 L × 5 H sb (3.5 × 4.5 × 2.5 m).
* **Mass:** Dry Mass `≈ 4.8 t` · Maximum Takeoff Mass `≈ 5.8 t` · Installed Power: 2.5 MW Peak (2× Batteries + 1× Reactor).

---

## 0 · Reference Grid, Coordinate System & Design Envelope

### 0.1 · Spatial Datum & Alignment Triad
All coordinates throughout this chapter reference the geometric center of the belly docking connector at Level L1:
```
           +Y (Zenith / Aero Roof, L5)
           ▲
           │   +Z (Bow / Sensor Nose & Optics)
           │  ▲
           │ /
           │/
───────────┼───────────► +X (Starboard Sponson)
          /│
         / │
        /  │
       ▼   ▼
     −Z    −Y (Ventral / Belly Dock & Skids, L1)
   (Stern)
```
* **Origin [0, 0, 0]:** Centerline of `KES Dock Conn` (Small Grid Connector) at Level L1.
* **X-Axis (Beam):** Port (−3 sb / −1.5 m) to Starboard (+3 sb / +1.5 m). Total beam = 7 sb (3.5 m).
* **Y-Axis (Elevation):** Belly landing skids (Level L1 / 0.0 m) to upper spine hood (Level L5 / 2.5 m). Total height = 5 sb (2.5 m).
* **Z-Axis (Length):** Stern main drive nozzles (−4 sb / −2.0 m) to forward sensor nose (+4 sb / +2.0 m). Total length = 9 sb (4.5 m).
* **Center of Mass (CoM):** Target dry CoM sits at `[X: 0.00, Y: +1.25, Z: 0.00]` sb, directly above the center of lift.

---

### 0.2A · Space Engineers 10-Slot Paint Palette Assignments
Standardized finish and color assignments for KESTREL-01 fabrication:

| Slot # | Color Name | Hex Code | RGB | Finish | Functional Subsystem / Target Surface |
|:---:|---|:---:|:---:|---|---|
| **0** | Industrial Slate | `#2B303A` | `43, 48, 58` | Battered | Belly landing skids, thruster exhaust boxes, underbody frame. |
| **1** | Hazard Amber | `#E6A100` | `230, 161, 0` | Neon | Wingtip navigation beacons, dock alignment crosshairs. |
| **2** | Armor Olive | `#3B4438` | `59, 68, 56` | Clean | Structural wing ribs, internal machinery bulkheads. |
| **3** | Core Hold Gray | `#2F353B` | `47, 53, 59` | Clean | Small Cargo Container shell and conveyor junctions. |
| **4** | Processing Teal | `#1ABC9C` | `26, 188, 156` | Clean | Geological Ore Detector housing and survey nose sensor. |
| **5** | High-Viz Yellow | `#F1C40F` | `241, 196, 15` | Heavy Rust | Maintenance grab bars, battery diagnostic test port. |
| **6** | Alert Red | `#C0392B` | `192, 57, 43` | Clean | Power Core (2× Small Grid Batteries, 1× Small Reactor). |
| **7** | Fleet Navy | `#1B263B` | `27, 38, 59` | Clean | Central Avionics Core, Remote Control, Gyro, `[KES] PB Scout`. |
| **8** | Scout White | `#ECF0F1` | `236, 240, 241` | Clean | Aerodynamic outer fuselage, wing sponsons, top roof fairing. |
| **9** | Beacon White | `#FFFFFF` | `255, 255, 255` | Neon | Forward high-intensity searchlights and belly alignment beam. |

---

### 0.2B · Exterior Livery & Two-Tone Color Blocking Map

```
TOP-DOWN DORSAL LIVERY (Level L4-L5, Looking −Y):
      X: −3   −2   −1    0   +1   +2   +3
  Z: +4       [LGT 9] [CAM NOSE] [LGT 9]     ◄── Bow Sensor Nose (Palette 8 White / Slot 9 Light)
  Z: +3  ┌────────────────────────────────┐
  Z: +2  │HA 1│      SW 8 (WHITE)   │HA 1│   ◄── Amber Wingtips (Slot 1) / White Cowling (Slot 8)
  Z: +1  │    ├───────┬──────┬──────┤    │
     0   │    │ SW 8  │AERO  │ SW 8 │    │   ◄── Aerodynamic Spine Fairing (White)
  Z: −1  │    │       │SPINE │      │    │
  Z: −2  │    ├───────┴──────┴──────┤    │
  Z: −3  └────┤       [ANT 8]       ├────┘   ◄── Comms Mast (10 km Antenna)
  Z: −4       │ [AFT 0]      [AFT 0]│        ◄── Dual Stern Thrusters (Slate Gray Slot 0)
              └─────────────────────┘

STARBOARD ELEVATION LIVERY (Looking +X at Starboard Flank):
      Z: −4      −3       −1        0       +1       +3       +4
  L5    ┌────────┬───────────────────────────────────┬────────┐  ROOF (L5)
        │ [ANT 8]│        Aero Spine Hood (SW 8)     │[CAM 8] │
  L4    ├────────┴───────┬───────────────────┬───────┴────────┤  AVIONICS (L4)
        │ Main Drive Aft │ Central Avionics  │ Searchlight 9  │
  L3    ├────────────────┼───────────────────┼────────────────┤  POWER (L3)
        │ [REACTOR 6]    │ [BATTERY 1, 2 (6)]│ [ORE DET (4)]  │
  L2    ├────────────────┼───────────────────┼────────────────┤  MANEUVER (L2)
        │ [LIFT 3, 4]    │ [KES Dock Conn]   │ [GROUND SENS]  │
  L1    └───[SKID 0]─────┴──────[CONN ↓]─────┴────[SKID 0]────┘  BELLY (L1)
```

---

### 0.4B · 3/4 Isometric Perspective Wireframes

#### View 1 · Dorsal Bow-Port 3/4 Isometric Wireframe (Flight Stance)
```
                     [NOSE OPTICS & CAM]
                        +Z  ▲
                           / \
                          /   \
             ┌═══════════▼═════▼═══════════┐
            / │    Forward Sensor Nose     │ \
           /  │     [KES Ore Detector]     │  \
          /   └═════════════╤══════════════┘   \
         /                  │                   \
   ┌────▼──────┐     ┌──────┴──────┐     ┌───────▼───┐
   │ Wingtip P │     │             │     │ Wingtip S │
   │ (Amber 1) │     │  Aero Spine │     │ (Amber 1) │
   ├───────────┤     │  Avionics   │     ├───────────┤
   │ Port Spons│     │  [RC][GYRO] │     │ Stbd Spons│
   │ (White 8) │◄────┤  [PB Scout] ├────►│ (White 8) │
   ├───────────┤     │             │     ├───────────┤
   │ Lat Thrust│     │             │     │ Lat Thrust│
   └────▲──────┘     └──────┬──────┘     └───────▲───┘
         \                  │                   /
          \   ┌═════════════╧══════════════┐   /
           \  │    Stern Propulsion Pod    │  /
            \ │    2× Main Drive Thrusters │ /
             └═════════════════════════════┘
                           \   /
                            \ /
                             ▼ −Z (STERN)
```

#### View 2 · Ventral Belly 3/4 Isometric Wireframe (Docking Stance)
```
          [BOW SKID P]                  [BOW SKID S]
           (Half Block)                  (Half Block)
             \                             /
              ▼                           ▼
        ┌───────────┐               ┌───────────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └─────┬─────┘               └─────┬─────┘
              │                           │
              │     ┌───────────────┐     │
              │     │ KES Cam Ground│     │
              │     │ [X0, Z+1, L1] │     │
              │     ├───────────────┤     │
              └───► │ KES Dock Conn │ ◄───┘
                    │ (Small Conn ↓)│
                    │ Centered 0, 0 │
              ┌───► ├───────────────┤ ◄───┐
              │     │ Level L1      │     │
              │     └───────────────┘     │
              │                           │
        ┌─────┴─────┐               ┌─────┴─────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └───────────┘               └───────────┘
              ▲                           ▲
             /                             \
          [STERN SKID P]                [STERN SKID S]
```

---

### 0.4C · Transverse Hull Station Profiles

#### Station 1 · Bow Sensor Nose & Optics (Z = +3.0 sb)
Transverse section looking forward (+Z) through the forward reconnaissance head:
```
Level:   X: −2       −1        0       +1       +2
 L5                 ┌──────────────────┐
                    │  [KES Cam Nose]  │
 L4      ┌──────────┼──────────────────┼──────────┐
         │Light Port│ [Searchlight 1]  │Light Stbd│
 L3      ├──────────┼──────────────────┼──────────┤
         │Reverse 1 │[KES Ore Detector]│Reverse 2 │
 L2      ├──────────┼──────────────────┼──────────┤
         │Lift 1 (D)│ [Conveyor Trunk] │Lift 2 (D)│
 L1      └───┬──────┴──────────────────┴──────┬───┘
             ▼                                ▼
       [BOW SKID P]                     [BOW SKID S]
```

#### Station 2 · Midship Power Core & Docking Hub (Z = 0.0 sb)
Transverse section looking forward (+Z) through the origin and Center of Mass:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L5                          ┌─────────────┐
                             │ Aero Spine  │
 L4      ┌────────┬──────────┼─────────────┼──────────┬────────┐
         │WingtipP│Lat Port 1│[KES Gyro sb]│Lat Stbd 1│WingtipS│
 L3      ├────────┼──────────┼─────────────┼──────────┼────────┤
         │Sponson │[BAT 1]   │[CARGO HOLD] │[BAT 2]   │Sponson │
 L2      ├────────┼──────────┼─────────────┼──────────┼────────┤
         │Cowling │PushDown 1│[CONVEYOR HUB│PushDown 2│Cowling │
 L1      └────────┴───┬──────┴──────┬──────┴──────┬───┴────────┘
                      │             │             │
                 [SKID P]     [KES Dock Conn]  [SKID S]
                              Connector DOWN
```

#### Station 3 · Stern Main Drive & Telemetry Mast (Z = −3.0 sb)
Transverse section looking forward (+Z) through the aft propulsion box:
```
Level:   X: −2       −1        0       +1       +2
 L5                 ┌──────────────────┐
                    │   [KES Antenna]  │ (10 km Comms)
 L4      ┌──────────┼──────────────────┼──────────┐
         │OutriggerP│[KES PB Scout]    │OutriggerS│
 L3      ├──────────┼──────────────────┼──────────┤
         │Main Fwd 1│ [KES Reactor]    │Main Fwd 2│
 L2      ├──────────┼──────────────────┼──────────┤
         │Lift 3 (D)│ [Conveyor Trunk] │Lift 4 (D)│
 L1      └───┬──────┴──────────────────┴──────┬───┘
             ▼                                ▼
       [STERN SKID P]                   [STERN SKID S]
```

---

### 0.5 · Master Conveyor & Fuel Manifold Schematic

The Kestrel incorporates a fully sealed, bidirectional small-conveyor trunk that interconnects the belly connector, sample hold, reactor fuel supply, and ore detector:

```
                           [KES Cam Nose] + [KES Ore Detector]
                              (Forward Survey Pod · Z +2 to +4)
                                             │
                                             ▼
                                  [KES Hold Samples]
                         (Small Cargo Container sb · Z +1, L2)
                         (Capacity: 100 L · Geological Cache)
                                             │
                                             ▼
                                   [Conveyor Hub Core]
                                (Level L2 Centerline · Z 0)
                                      │             │
                   ┌──────────────────┴──┐          │
                   ▼                     ▼          ▼
            [KES Battery 1]       [KES Battery 2]  [KES Reactor]
            (Left Wing L3)        (Right Wing L3)  (Aft Hold L2)
                                                    (0.5 MW Backup)
                                         │
                                         ▼ (Bottom Port)
                                  [KES Dock Conn]
                             (Small Grid Connector sb)
                             (Flush Level L1 at X0, Z0)
                                         │
        ═════════════════════════════════╪═════════════════════════════════  Landing Deck
                                         │
                                         ▼ (Mates to Pad)
         [MAS Kestrel Dock 1/2]    OR    [ORC Bay P2]    OR    [GND PAD]
        (Mastodon Bow Pad)               (Orca Carrier)        (Ground Base)
```

---

### 0.6 · Master Electrical Single-Line Schematic & 5 Power Modes

```
                  2× SMALL BATTERIES + 1× SMALL REACTOR
               1.0 MWh Battery Capacity · 2.5 MW Combined Surge
                                  │
                                  ▼
                          PRIMARY 2.5 MW BUS
        ┌─────────────────────────┼─────────────────────────┐
        │                         │                         │
        ▼                         ▼                         ▼
  DOWNWARD LIFT CLUSTER     6-AXIS MANEUVER RING      AVIONICS & RECON
  4× Small Atmo (sb)        10× Small Atmo (sb)       1× RC + 1× PB (0.04 MW)
  (1.60 MW Max Burst)       (0.80 MW Max Burst)       1× Gyro (0.02 MW)
  [Empty Hover: 0.33 MW]    [Braking Burst: 0.32 MW]  1× Ore Detector (0.05 MW)
  [Max Climb: 1.60 MW]      [Push-Down: 0.16 MW]      1× Antenna + Cam (0.04 MW)
```

#### Operational Energy Modes
1. **Mode 1 · Docked Fast Shore Recharge (Recharge = ON):**
   * Total Draw: `0.00 MW` · Shore Intake: `2.0 MW` · Full 0 to 100% recharge in `30.0 minutes`.
2. **Mode 2 · High-Speed Geological Survey Cruise (80 m/s):**
   * Lift Load: 14.7% (330 kW) · Forward Thrusters: 160 kW · Ore Detector: 50 kW · Avionics: 20 kW · Total Draw: `560 kW` · Battery Endurance: `1.78 hours` on battery alone.
3. **Mode 3 · Stationary Loitering Hover & Seismic Data Relay:**
   * Hover Load: 14.7% (330 kW) · Radio Antenna @ 10 km: 35 kW · Sensors: 20 kW · Total Draw: `385 kW` · Battery Endurance: `2.60 hours`.
4. **Mode 4 · High-G Evasive Flare / Intercept Break:**
   * 4× Lift (1.60 MW) + 2× Main Fwd (0.32 MW) + 4× Lateral (0.64 MW) = `2.56 MW` peak surge. Battery bank + Reactor fully absorb surge with zero brownout.
5. **Mode 5 · Emergency Deadman RTB / Gliding Return:**
   * Battery stored $\le 15\%$. Non-essential searchlights and ore detector severed; reactor supplies 0.5 MW continuous backup; speed capped at 40 m/s; automated RTB to host pad.

---

### 0.7 · Master Bill of Materials (BOM) & Physics Budget

| Category | Component Description | Block Qty | Mass (t) | PCU | Critical Mounting / Orientation |
|---|---|:---:|:---:|:---:|---|
| **Fuselage** | Small Armor Blocks, Slopes & Half-Blocks | 38 | 1.80 | 38 | Palette 8 White, Slot 0 Slate, Slot 1 Amber |
| **Propulsion** | Small Atmospheric Thrusters (Small Grid) | 14 | 1.40 | 210 | 4 Lift (D), 2 Main (Aft), 2 Rev (Bow), 4 Lat, 2 Down |
| **Power** | Small Grid Batteries (1.0 MWh aggregate) | 2 | 0.50 | 40 | Port & Starboard bays at [X ±1, Z 0, L3] |
| **Power** | Small Nuclear Reactor (0.5 MW backup) | 1 | 0.15 | 25 | Aft centerline [X0, Z −2, L2] |
| **Sensors** | Geological Ore Detector (Small Grid) | 1 | 0.12 | 15 | Forward nose bay [X0, Z +2, L2] (50 m range†) |
| **Cargo** | Small Cargo Container (Geological Cache) | 1 | 0.10 | 10 | Centerline hold [X0, Z +1, L2] (100 L volume) |
| **Attitude** | Micro Gyroscope (Small Grid) | 1 | 0.08 | 15 | Centerline avionics [X0, Z 0, L3] |
| **Compute** | Programmable Block (`[KES] PB Scout`) | 1 | 0.08 | 25 | Centerline rack [X0, Z −1, L3] |
| **Avionics** | Remote Control Block | 1 | 0.05 | 25 | Centerline deck [X0, Z +1, L3], Fwd = +Z |
| **Comms** | Radio Antenna (Small Grid) | 1 | 0.04 | 25 | Aft mast [X0, Z −3, L4] (10,000 m broadcast†) |
| **Optics** | Fixed Cameras | 2 | 0.04 | 30 | 1 Nose [X0, Z+4, L4], 1 Ground [X0, Z+1, L1] |
| **Lighting** | Small Grid Spotlights & Beacons | 4 | 0.08 | 40 | 2 Bow Searchlights, 2 Wingtip Beacons |
| **Docking** | Small Grid Connector | 1 | 0.16 | 25 | Centered at [X0, Z0, L1], facing DOWN |
| **Logic** | Event Controller & Timer Blocks | 2 | 0.06 | 40 | Aft equipment bay at L3 (`KES EC`, `KES TMR`) |
| **TOTAL** | **Complete KESTREL-01 Scout Drone** | **68** | **≈ 4.80 t** | **563** | **Aerodynamic, high-speed prospector** |

#### Physics & Gravitational Lift Margins
* **Dry Mass:** `4.80 t` (Airframe + Systems)
* **Maximum Takeoff Mass (MTOW):** `5.80 t` (Laden with sample ore and fuel)
* **Downward Lift Thrust (4× Small Atmo sb):** $4 \times 80\text{ kN} = \mathbf{320\text{ kN}}$
* **Gravity Load (Earth 1.0 g = 9.81 m/s²):**
  * Empty: $4.80\text{ t} \times 9.81\text{ m/s}^2 = \mathbf{47.09\text{ kN}}$ (Hover Throttle = 14.7%)
  * Laden MTOW: $5.80\text{ t} \times 9.81\text{ m/s}^2 = \mathbf{56.90\text{ kN}}$ (Hover Throttle = 17.8%)
* **Thrust-to-Weight (T/W) Ratio:**
  * Empty T/W: $320.0 / 47.09 = \mathbf{6.79}$
  * Laden T/W: $320.0 / 56.90 = \mathbf{5.62}$
* **Climb Acceleration:** $a = (320.0\text{ kN} - 47.1\text{ kN}) / 4.8\text{ t} = \mathbf{56.8\text{ m/s}^2}$ (Reaches 100 m/s in $< 1.8\text{ s}$!).

---

### 0.7B · Maintenance Access & Field Ergonomics
1. **Dorsal Reactor Access Hatch:** A hinged armor flap at Level L4 [X0, Z −2] opens to expose the small reactor's manual uranium access port for field refueling without disassembling armor.
2. **Geological Sample Cache Port:** The small cargo hold at Level L2 has an outward-facing access door beneath the forward starboard sponson, allowing grounded engineers to manually extract rare mineral core samples.
3. **Service Grab Bars:** High-visibility yellow grab bars (Palette Slot 5) mounted along the dorsal spine provide EVA tether anchor points during field recovery.

---

## Phase 12-1 · Belly Docking Skid & Sensor Foundation (Levels L1-L2)

### Step 12.1 · Belly Connector & Twin Landing Skids (L1)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Belly Dock  ·  Footprint: 7 W × 9 L × 1 H sb  ·  Level: L1      │
│ [PARTS TRAY] 12× Small Armor Half Blocks, 1× Connector (Small), 2× Conveyor Hub sb   │
│ [PALETTE] Skids: Slot 0 Industrial Slate (#2B303A), Connector: Slot 0                │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Place 1 Small Grid Connector `KES Dock Conn` at coordinate `[X: 0, Y: 0, Z: 0]` at Level L1, facing straight DOWN (−Y).
2. Install 2 Conveyor Junction cubes immediately above the connector to form the vertical routing trunk.
3. Construct twin landing skids using Heavy Armor Half Blocks at X = ±2 sb, spanning from Z −3 to +3 sb.
4. Mount 1× Camera `KES Cam Ground` recessed at [X0, Z +1, L1] facing straight DOWN.

```
Plan view, Level L1 (Skids & Belly Connector):
        X: −2   −1    0   +1   +2
    Z: +3  [SKID P] ······ [SKID S]  ◄── Bow Skid Contact Points (Palette 0: Slate)
           │                  │
    Z: +1  │      [CAM GROUND]│      ◄── Downward Docking Camera
    Z:  0  │      [CONN ↓]    │      ◄── Small Grid Connector (`KES Dock Conn`)
           │                  │          (Centered X0, Z0, Level L1)
    Z: −3  [SKID P] ······ [SKID S]  ◄── Stern Skid Contact Points
```

---

### Step 12.2 · Geological Ore Detector & Downward Lift Cluster (L1-L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Sensors & Lift  ·  Belly Bays  ·  Level: L1-L2                  │
│ [PARTS TRAY] 1× Ore Detector (Small Grid), 4× Small Atmospheric Thrusters (Small)    │
│ [PALETTE] Sensor: Slot 4 Teal (#1ABC9C), Thruster Cowlings: Slot 0 Slate Gray        │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1× Ore Detector `KES Ore Detector` at [X0, Z +2, L2] on the forward conveyor trunk facing downward.
2. Mount 4× Small Atmospheric Thrusters into the skid recesses, all exhausting straight DOWN (−Y):
   * `KES Thruster Lift 1` : Forward-Port [X −2, Z +3, L1-L2].
   * `KES Thruster Lift 2` : Forward-Starboard [X +2, Z +3, L1-L2].
   * `KES Thruster Lift 3` : Aft-Port [X −2, Z −3, L1-L2].
   * `KES Thruster Lift 4` : Aft-Starboard [X +2, Z −3, L1-L2].
3. Assign all 4 to terminal group `KES Thrusters Lift`.

```
Plan view, Level L1-L2 (Lift Thrusters & Sensor Mount):
        X: −2   −1    0   +1   +2
    Z: +3  [LIFT 1] ────── [LIFT 2]  ◄── Forward Downward Thrusters (Face −Y)
                    [ORE DET]        ◄── Ore Detector (Palette Slot 4: Teal)
    Z:  0         [CONN ↓]
    Z: −3  [LIFT 3] ────── [LIFT 4]  ◄── Aft Downward Thrusters (Face −Y)
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 12.1A: DOCKING CLEARANCE GATE**
> 1. Ensure `KES Dock Conn` sits recessed exactly **0.5 sb (0.25 m)** above the skid bottom plane.
> 2. Verify `KES Ore Detector` conveyor port connects airtight to the center trunk.
> 3. Verify all 4 lift thrusters have a clear exhaust gap beneath their nozzles.

---

## Phase 12-2 · Power Core & Core Hold Deck (Level L2)

### Step 12.3 · Power Vault & Fuel Storage (L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Power Core  ·  Midship Bay  ·  Level: L2, Z −2 to +1            │
│ [PARTS TRAY] 2× Small Grid Batteries, 1× Small Nuclear Reactor, 1× Small Cargo Box   │
│ [PALETTE] Batteries & Reactor: Slot 6 Alert Red (#C0392B), Hold: Slot 3 Gray         │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Install 1× Small Cargo Container `KES Hold Samples` at [X0, Z +1, L2] immediately behind the ore detector.
2. Install 2× Small Grid Batteries `KES Battery 1, 2` flanking the center line at [X −1, Z 0, L2] and [X +1, Z 0, L2].
3. Install 1× Small Nuclear Reactor `KES Reactor` at [X0, Z −2, L2] plumbed into the aft conveyor trunk.
4. Assign batteries to group `KES Batteries` (1.0 MWh aggregate capacity, 2.0 MW max discharge).

```
Plan view, Level L2 (Power & Internal Hold):
        X: −1    0   +1
    Z: +1  ┌───────────┐
           │[CARGO HOLD│  ◄── Small Cargo Container (Centered X0, Z +1)
    Z:  0  │[BAT 1][BAT│  ◄── 2× Small Batteries (Palette Slot 6: Red)
    Z: −2  │ [REACTOR] │  ◄── 1× Small Nuclear Reactor (15 kW / 0.5 MW burst)
           └───────────┘
```

---

## Phase 12-3 · 10-Thruster Omnidirectional Maneuvering Ring (Levels L2-L3)

### Step 12.4 · Omnidirectional Vector Array (L2-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Thruster Ring  ·  Perimeter Outriggers  ·  Level: L2-L3         │
│ [PARTS TRAY] 10× Small Atmospheric Thrusters (Small Grid · 80 kN each)               │
│ [PALETTE] Slot 0 Slate Gray (#2B303A), Nozzle Trim: Slot 6 Alert Red                 │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Mount 10 maneuvering thrusters to establish 6-axis flight authority:
1. **Forward Cruise (2×):** `KES Thruster Fwd 1, 2` at [X ±2, Z −4, L3] facing Aft (−Z) to push Bow (+Z) (160 kN drive).
2. **Reverse Braking (2×):** `KES Thruster Rev 1, 2` at [X ±2, Z +3, L3] facing Bow (+Z) to push Aft (−Z) (160 kN braking).
3. **Port Translation (2×):** `KES Thruster Port 1, 2` at [X −3, Z ±1, L3] facing Port (−X) to push Starboard (+X).
4. **Starboard Translation (2×):** `KES Thruster Stbd 1, 2` at [X +3, Z ±1, L3] facing Starboard (+X) to push Port (−X).
5. **Down-Push Assist (2×):** `KES Thruster Down 1, 2` at [X ±1, Z 0, L3] facing UP (+Y) to push straight DOWN (−Y).
6. Assign all 10 to terminal group `KES Thrusters Maneuver`.

```
Plan view, Level L3 (Thruster Vector Alignment):
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3       [REV 1]           [REV 2]       ◄── Facing Bow +Z (Braking)
    Z: +1  [PORT 1]                   [STBD 1]  ◄── Facing Outward (Lateral)
    Z: −1  [PORT 2]                   [STBD 2]
    Z: −4       [FWD 1]           [FWD 2]       ◄── Facing Aft −Z (Main Drive)
```

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 12.2A: THRUSTER IMPINGEMENT & POWER CLEARANCE**
> 1. Verify all 14 atmospheric thrusters have at least 1 sb clear gap ahead of their exhaust nozzles.
> 2. Confirm total power draw under 100% all-thruster burst does not exceed 2.40 MW.
> 3. Verify `KES Thruster Down 1, 2` exhaust cleanly upward through the canopy grille.

---

## Phase 12-4 · Central Avionics Core & Aerodynamic Shell (Levels L3-L5)

### Step 12.5 · Autonomous Avionics Spine (L3-L4)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Avionics  ·  Centerline Stack  ·  Level: L3-L4                  │
│ [PARTS TRAY] 1× Remote Control, 1× Gyro, 1× PB, 1× Antenna, 1× Camera, 2× Spotlight │
│ [PALETTE] Avionics Shell: Slot 7 Fleet Navy (#1B263B), Antenna: Slot 8 White         │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1× Remote Control `KES Remote Control` at [X0, Z +1, L3]. Ensure Forward = Bow (+Z) and Up = Roof (+Y).
2. Mount 1× Micro Gyroscope `KES Gyro` at [X0, Z 0, L3] directly at the Center of Mass.
3. Mount 1× Programmable Block `[KES] PB Scout` at [X0, Z −1, L3].
4. Mount 1× Radio Antenna `KES Antenna` at [X0, Z −3, L4]. Set broadcast range to `10,000 m`†.
5. Mount 1× Camera `KES Cam Nose` at [X0, Z +4, L4] pointing Bow (+Z) with high-zoom reconnaissance optics.
6. Mount 2× Spotlights `KES Light Port / Stbd` at [X ±1, Z +4, L4] angled 5° downward.

```
Elevation, looking West through Centerline (X0, Level L3-L5):
       L5  ═════════[AERO SPINE HOOD]═════════  Roof Fairing (Palette 8: White)
       L4  [ANTENNA]             [CAM NOSE]     ◄── Forward Recon Optics & Comms
       L3  [PB SCOUT]   [GYRO]  [REMOTE CTRL]   ◄── Central Avionics Brain (Palette 7)
       L2  [REACTOR]   [BAT 1]   [CARGO HOLD]   ◄── Power & Hold Deck
```

---

### Step 12.6 · Aerodynamic Fuselage Fairing & Sponsons (L4-L5)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Outer Hull  ·  Fuselage Canopy  ·  Level: L4-L5                 │
│ [PARTS TRAY] 22× Small Armor Slopes & Inverted Corners (Small Grid)                  │
│ [PALETTE] Outer Skin: Slot 8 Scout White (#ECF0F1), Wingtips: Slot 1 Hazard Amber    │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Enclose the upper avionics deck with a smooth aerodynamic wedge fairing in Scout White (Slot 8).
2. Install Hazard Amber (Slot 1) wingtip navigation markers at [X ±3, Z +2, L4].
3. Frame the nose optics with 45° sloped armor blocks to deflect atmospheric dust and bird strikes.
4. Add yellow grab bars (Slot 5) along the spine for field recovery operations.

```
Plan view, Level L4-L5 (Canopy & Wing Fairings):
        X: −3   −2   −1     0    +1   +2   +3
    Z: +4       [LIGHT P] [NOSE CAM] [LIGHT S]   ◄── Tapered Forward Wedge
    Z: +2  [TIP P] ┌─────────────────┐ [TIP S]   ◄── Amber Wingtip Markers (Palette 1)
    Z:  0  │       │  [AERO SPINE]   │       │
    Z: −2  │       │                 │       │
    Z: −4       [AFT]   [ANTENNA]   [AFT]        ◄── Dual Thruster Fairings
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 12.3A: AVIONICS ORIENTATION & COMMS GATE**
> 1. Verify `KES Remote Control` forward orientation points strictly toward Bow (+Z).
> 2. Verify `[KES] PB Scout` compiles with zero allocations under MDK2 rules.
> 3. Verify `KES Antenna` broadcast range is set to `10,000 m` with HUD name set to `KESTREL-01`.

---

## Phase 12-5 · Master Control Logic, AI Scripting Contract & Software Abstraction

### 1. Terminal Group Taxonomy
* `KES Thrusters All` : All 14 atmospheric thrusters.
* `KES Thrusters Lift` : 4× Downward lift thrusters (`KES Thruster Lift 1..4`).
* `KES Thrusters Maneuver` : 10× Maneuvering thrusters (Fwd, Rev, Port, Stbd, Down).
* `KES Batteries` : 2× Small Grid Batteries (`KES Battery 1, 2`).
* `KES Lights` : 2× Forward Spotlights + 2× Wingtip Beacons.
* `KES Cameras` : `KES Cam Nose` (recon) + `KES Cam Ground` (docking align).

---

### 2. Event Controller Automation Ladder (Rule C4)

| Controller | Monitored Parameter | Threshold / Event | Slot 1 Action (True) | Slot 2 Action (False) |
|---|---|---|---|---|
| `KES EC Battery` | `KES Batteries` Stored Power | Stored Charge $< 20.0\%$ | Trigger RTH (Return to Base/Carrier) | Normal Survey Mode |
| `KES EC Dock` | `KES Dock Conn` State | Connector Status = Locked | Batteries $\rightarrow$ Recharge · Thrusters OFF | Batteries $\rightarrow$ Auto · Thrusters ON |
| `KES EC VeinDetect`| `KES Ore Detector` | Subsurface Vein Detected | PB: Save GPS & Broadcast IGC | Continue Survey Pattern |

---

### 3. Native Block Failsafe Matrix (Zero-PB Safety)
If `[KES] PB Scout` crashes, freezes, or encounters an uncaught runtime exception:
1. `KES TMR Heartbeat` (5.0 s loop) fails to receive a PB reset pulse and expires.
2. Immediate hardwired failsafe actions triggered by timer:
   * Remote Control Autopilot `Collision Avoidance` = ON.
   * Remote Control `Dampeners` = ON.
   * Gyro and Thrust Overrides = OFF.
   * Drone enters stationary hover at current altitude until operator overrides via C2 console.
3. If docked at host pad when failure occurs:
   * Belly connector remains magnetically locked.
   * Thrusters remain locked OFF to prevent pad damage.

---

### 4. IGC Broadcast Network Dictionary

```
TMS.SCOUT.STATUS (Broadcast every 1.66 s):
KES-01;<state_enum>;<battery_pct>;<gps_coords>
Example: KES-01;SURVEYING;88.5;GPS:OreVein1:14250.0:-23100.0:450.0:

TMS.SCOUT.VEIN (Broadcast on vein acquisition):
<ore_type>;<depth_m>;<gps_coords>;<est_yield>
Example: URANIUM;38;GPS:VeinUranium:14280.5:-23150.2:412.0:;HIGH

TMS.SCOUT.RECALL (Inbound emergency recall trigger from C2 Bridge):
RECALL;KES-01;<target_carrier_id>;<pad_id>
Example: RECALL;KES-01;MAS-01;PAD-1
```

---

## Phase 12-6 · Commissioning Checklist & Rigorous Test Protocols (RIG Test Creative)

### Suite A · Dry Mass & Power-to-Weight Calibration
* [ ] **Dry Mass Verification:** Spawn KESTREL-01 in creative; terminal reports dry mass `≈ 4.8 t`.
* [ ] **Hover Throttle Test:** Lift off to 50 m altitude in Earth 1.0 g; verify 4× lift thrusters hover at `14-16%` load (`≈ 330 kW` draw).
* [ ] **Climb Acceleration:** Apply 100% vertical lift collective; verify climb velocity reaches 100 m/s in $< 2.0\text{ seconds}$† ($a \ge 50\text{ m/s}^2$).

### Suite B · Subsurface 50 m Ore Vein Discovery & GPS Broadcast
* [ ] **Vein Acquisition:** Fly over buried Uranium and Platinum deposits; confirm `KES Ore Detector` registers deposits up to 50 m depth.
* [ ] **IGC Packet Verification:** Check C2 Video Wall (LCD 1/6); verify `TMS.SCOUT.VEIN` broadcast updates target queue with correct GPS and depth metadata.

### Suite C · High-Speed Terrain Following at 80 km/h
* [ ] **Contour Hugging:** Engage autopilot terrain following at 80 km/h at 15 m altitude; confirm forward optics and down-push thrusters arrest vertical bounce over ridge crests without ground strike.
* [ ] **Lateral Drift Stability:** Execute high-speed S-turns at 100 m/s; verify 10-thruster maneuver ring maintains zero slip angle.

### Suite D · Mastodon Forward Scout Pad Soft Docking
* [ ] **Approach Sequence:** Approach Mastodon MKI bow at 5 m/s; align `KES Cam Ground` crosshairs with `MAS Welder Pad Kestrel 1`; confirm soft touchdown.
* [ ] **Pad Latch & Weld Check:** Confirm `KES Dock Conn` locks on first contact; verify Mastodon pad welder executes repair pulse without damaging skids.

### Suite E · Orca Carrier Bay P2 Entry Clearance
* [ ] **Bay Entry Alignment:** Approach Orca Bay P2; confirm lateral clearance is $\ge 1.0\text{ B}$ on all sides; verify clean transition from ambient atmosphere to carrier bay hangar.
* [ ] **Power Shutdown:** Confirm `KES EC Dock` triggers battery `Recharge` and cuts all thrusters instantly upon lock.

### Suite F · Ground Base Pad Touchdown & Fast Shore Recharge
* [ ] **Base Terminal Capture:** Touch down on Ground Base light drone pad; confirm 2× Small Batteries pull 2.0 MW shore charge, reaching 100% state of charge in $< 30\text{ minutes}$.

### Suite G · Native Heartbeat Loss & Emergency Deadman Hover Fallback
* [ ] **Simulated PB Crash:** Recompile `[KES] PB Scout` with an empty script during 80 m/s flight; verify `KES TMR Heartbeat` times out at 5.0 s, dampeners engage, overrides cut, and drone halts into stable altitude hold.

---

## Phase 12-7 · Unverified Values Tracking (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V29:** Small grid Ore Detector maximum passive scan range (verify 50 m radius in vanilla).
* **V49:** KESTREL-01 vertical climb acceleration on 4× Small Atmo thrusters ($56.8\text{ m/s}^2$†).

---

## Chapter 13 · ALBATROSS-01 Utility & Field-Repair Craft (ALB-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **ALBATROSS-01 Utility & Field-Repair Craft (ALB-01)**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 25, 31, 41, 42, 50, 52, 56, 62, 66 & 68)
* **Role:** Autonomous field-repair craft, precision structural welder, drone recovery transport, component resupply ferry, and external damage survey unit. Dispatches to repair damaged REMORA-01 drillers in vertical boreholes, performs hot-swap maintenance on ORCA-01 carrier hull armor, resupplies CATERPILLAR-01 consist components, and services MASTODON-01 MKI roof installations. Docks in Orca Bay S2, Ground Base logistics apron, and Mastodon roof decks.
* **Architecture:** Robust 9 W × 11 L × 5 H sb (4.5 × 5.5 × 2.5 m) flying tool chassis:
  1. **Belly Docking & Skid Foundation (Levels L1-L2):** Small Grid Connector (`ALB Dock Conn`) flush with heavy armor landing skids and downward docking camera (`ALB Cam Belly`).
  2. **Core Component Buffer & Welder Head (Levels L2-L3):** Medium Cargo Container (3×3×3 sb · 3,375 L volume) feeding a forward-mounted Ship Welder (`ALB Welder Nose`) and precision alignment reticle camera (`ALB Cam Welder`).
  3. **22-Thruster High-Authority Propulsion Ring (Levels L2-L4):** 8× Downward Lift Thrusters (640 kN lift · T/W ≈ 7.67 empty, 3.53 laden), 4× Forward Drive, 4× Reverse Braking, 4× Lateral Strafe, and 2× Down-Push Thrusters.
  4. **Power Vault & Avionics Core (Levels L3-L5):** 4× Small Grid Batteries (2.0 MWh aggregate), 1× Small Nuclear Reactor (0.5 MW backup), Remote Control, Micro Gyro, `[ALB] PB Utility`, 15 km Radio Antenna, and automated welding proximity sensor.
* **Grid:** Small Grid (1 sb = 0.5 m) · **Footprint:** 9 W × 11 L × 5 H sb (4.5 × 5.5 × 2.5 m).
* **Mass:** Dry Mass `≈ 8.5 t` · Maximum Laden Mass `≈ 18.5 t` (10.0 t component payload) · Installed Power: 4.5 MW Peak.

---

## 0 · Reference Grid, Coordinate System & Design Envelope

### 0.1 · Spatial Datum & Alignment Triad
All coordinates throughout this chapter reference the geometric center of the belly docking connector at Level L1:
```
           +Y (Zenith / Utility Roof, L5)
           ▲
           │   +Z (Bow / Welder Head & Work Spotlights)
           │  ▲
           │ /
           │/
───────────┼───────────► +X (Starboard Outrigger)
          /│
         / │
        /  │
       ▼   ▼
     −Z    −Y (Ventral / Belly Dock & Skids, L1)
   (Stern)
```
* **Origin [0, 0, 0]:** Centerline of `ALB Dock Conn` (Small Grid Connector) at Level L1.
* **X-Axis (Beam):** Port (−4 sb / −2.0 m) to Starboard (+4 sb / +2.0 m). Total beam = 9 sb (4.5 m).
* **Y-Axis (Elevation):** Belly landing skids (Level L1 / 0.0 m) to upper roof canopy (Level L5 / 2.5 m). Total height = 5 sb (2.5 m).
* **Z-Axis (Length):** Stern main drive thrusters (−5 sb / −2.5 m) to forward welder tip (+5 sb / +2.5 m). Total length = 11 sb (5.5 m).
* **Center of Mass (CoM):** Target dry CoM sits at `[X: 0.00, Y: +1.25, Z: 0.00]` sb, directly above the center of lift. Laden CoM remains within $\pm 0.1\text{ sb}$ along Z.

---

### 0.2A · Space Engineers 10-Slot Paint Palette Assignments
Standardized finish and color assignments for ALBATROSS-01 fabrication:

| Slot # | Color Name | Hex Code | RGB | Finish | Functional Subsystem / Target Surface |
|:---:|---|:---:|:---:|---|---|
| **0** | Industrial Slate | `#2B303A` | `43, 48, 58` | Battered | Belly landing skids, thruster exhaust boxes, underbody frame. |
| **1** | Hazard Amber | `#E6A100` | `230, 161, 0` | Heavy Rust | Bow welder nose housing, wingtip safety lights, dock alignment marks. |
| **2** | Armor Olive | `#3B4438` | `59, 68, 56` | Clean | Structural wing ribs, internal machinery bulkheads. |
| **3** | Core Hold Gray | `#2F353B` | `47, 53, 59` | Clean | Internal conveyor routing hubs and manifold tubes. |
| **4** | Manufacturing Cobalt| `#2980B9` | `41, 128, 185` | Clean | Outer protective utility hull armor, side outrigger fairings. |
| **5** | Logistics Emerald | `#27AE60` | `39, 174, 96` | Clean | Medium Cargo Container (component buffer hold shell). |
| **6** | Alert Red | `#C0392B` | `192, 57, 43` | Clean | Power Core (4× Small Grid Batteries, 1× Small Nuclear Reactor). |
| **7** | Fleet Navy | `#1B263B` | `27, 38, 59` | Clean | Central Avionics Pod, Remote Control, Gyro, `[ALB] PB Utility`. |
| **8** | Utility White | `#ECF0F1` | `236, 240, 241` | Clean | Roof canopy louvers and forward optics nacelle. |
| **9** | High-Viz Yellow | `#F1C40F` | `241, 196, 15` | Clean | Personnel handrails, tow eyelets, side impact rub rails. |

---

### 0.2B · Exterior Livery & Two-Tone Color Blocking Map

```
TOP-DOWN DORSAL LIVERY (Level L4-L5, Looking −Y):
      X: −4   −3   −2   −1    0   +1   +2   +3   +4
  Z: +5            [LGT 9] [WELDER 1] [LGT 9]            ◄── Hazard Amber Welder Nose (Slot 1)
  Z: +4       ┌─────────────────────────────────┐
  Z: +3  [HY9]│             MC 4                │[HY9]   ◄── High-Viz Yellow Rub Rails (Slot 9)
  Z: +2  │    │      (MANUFACTURING COBALT)     │    │
  Z: +1  │    ├───────┬─────────────────┬───────┤    │
     0   │    │ MC 4  │   ROOF CANOPY   │ MC 4  │    │   ◄── Cobalt Blue Outrigger Sponsons (Slot 4)
  Z: −1  │    │       │  UTILITY WHITE  │       │    │
  Z: −2  │    ├───────┴─────────────────┴───────┤    │
  Z: −3  │    │              MC 4               │    │
  Z: −4  [HY9]└─────────────┬─────┬─────────────┘[HY9]
  Z: −5       │ [AFT 0]     │[ANT]│     [AFT 0] │        ◄── 4× Stern Thrusters (Slate Gray Slot 0)
              └─────────────┴─────┴─────────────┘

STARBOARD ELEVATION LIVERY (Looking +X at Starboard Flank):
      Z: −5      −4       −2        0       +2       +4       +5
  L5    ┌────────┬───────────────────────────────────┬────────┐  ROOF (L5)
        │ [ANT 8]│        Roof Canopy (UW 8)         │[CAM 8] │
  L4    ├────────┴───────┬───────────────────┬───────┴────────┤  AVIONICS (L4)
        │ Main Drive Aft │ Central Avionics  │ Work Light 9   │
  L3    ├────────────────┼───────────────────┼────────────────┤  CARGO & POWER (L3)
        │ [4× BATTERIES] │ [MEDIUM CARGO (5)]│ [WELDER NOSE 1]│
  L2    ├────────────────┼───────────────────┼────────────────┤  PROPULSION (L2)
        │ [LIFT 5..8 (0)]│ [ALB Dock Conn]   │ [LIFT 1..4 (0)]│
  L1    └───[SKID 0]─────┴──────[CONN ↓]─────┴────[SKID 0]────┘  BELLY (L1)
```

---

### 0.4B · 3/4 Isometric Perspective Wireframes

#### View 1 · Dorsal Bow-Port 3/4 Isometric Wireframe (Flight Stance)
```
                      [SHIP WELDER NOSE]
                         +Z  ▲
                            / \
                           /   \
              ┌═══════════▼═════▼═══════════┐
             / │     Work Spotlights (2×)   │ \
            /  │     [ALB Cam Welder]       │  \
           /   └═════════════╤══════════════┘   \
          /                  │                   \
    ┌────▼──────┐     ┌──────┴──────┐     ┌───────▼───┐
    │ Rub Rail P│     │             │     │ Rub Rail S│
    │ (Yellow 9)│     │ Roof Canopy │     │ (Yellow 9)│
    ├───────────┤     │  Avionics   │     ├───────────┤
    │ Port Spons│     │ [PB Utility]│     │ Stbd Spons│
    │ (Cobalt 4)│◄────┤ [REMOTE][GY]├────►│ (Cobalt 4)│
    ├───────────┤     │             │     ├───────────┤
    │ Maneuver P│     │             │     │ Maneuver S│
    └────▲──────┘     └──────┬──────┘     └───────▲───┘
          \                  │                   /
           \   ┌═════════════╧══════════════┐   /
            \  │    Stern Propulsion Pod    │  /
             \ │    4× Main Drive Thrusters │ /
              └═════════════════════════════┘
                            \   /
                             \ /
                              ▼ −Z (STERN)
```

#### View 2 · Ventral Belly 3/4 Isometric Wireframe (Docking Stance)
```
          [BOW SKID P]                  [BOW SKID S]
           (Heavy Half)                  (Heavy Half)
             \                             /
              ▼                           ▼
        ┌───────────┐               ┌───────────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └─────┬─────┘               └─────┬─────┘
              │                           │
              │     ┌───────────────┐     │
              │     │ ALB Cam Belly │     │
              │     │ [X0, Z+1, L1] │     │
              │     ├───────────────┤     │
              └───► │ ALB Dock Conn │ ◄───┘
                    │ (Small Conn ↓)│
                    │ Centered 0, 0 │
              ┌───► ├───────────────┤ ◄───┐
              │     │ Level L1      │     │
              │     └───────────────┘     │
              │                           │
        ┌─────┴─────┐               ┌─────┴─────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └───────────┘               └───────────┘
              ▲                           ▲
             /                             \
          [STERN SKID P]                [STERN SKID S]
```

---

### 0.4C · Transverse Hull Station Profiles

#### Station 1 · Bow Welder Head & Work Spotlights (Z = +4.5 sb)
Transverse section looking forward (+Z) through the forward tool head:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L5                          ┌─────────────────┐
                             │[ALB Cam Welder] │
 L4                 ┌────────┼─────────────────┼────────┐
                    │Work Lgt│ Work Spotlights │Work Lgt│
 L3      ┌──────────┼────────┴─────────────────┴────────┼──────────┐
         │Reverse 1 │       [ALB WELDER NOSE]           │Reverse 4 │
 L2      ├──────────┼───────────────────────────────────┼──────────┤
         │Lift 1 (D)│          [Conveyor Hub]           │Lift 4 (D)│
 L1      └───┬──────┴───────────────────────────────────┴──────┬───┘
             ▼                                                 ▼
       [BOW SKID P]                                      [BOW SKID S]
```

#### Station 2 · Midship Component Hold & Lift Outriggers (Z = 0.0 sb)
Transverse section looking forward (+Z) through the origin and Center of Mass:
```
Level:   X: −4       −3       −2       −1     0    +1    +2       +3       +4
 L5                                   ┌─────────────┐
                                      │ Roof Canopy │
 L4      ┌────────┬────────┬──────────┼─────────────┼──────────┬────────┬────────┐
         │Rub Rail│Lat P 1 │Lat P 2   │ [ALB Gyro]  │Lat S 1   │Lat S 2 │Rub Rail│
 L3      ├────────┼────────┼──────────┼─────────────┼──────────┼────────┼────────┤
         │Sponson │Lift P 2│Lift P 3  │[MEDIUM CARGO│Lift S 2  │Lift S 3│Sponson │
 L2      ├────────┼────────┼──────────┤ 3×3×3 sb    ├──────────┼────────┼────────┤
         │Cowling │Push D 1│Conveyor  │[CONVEYOR HUB│Conveyor  │Push D 2│Cowling │
 L1      └────────┴───┬────┴──────────┴──────┬──────┴──────────┴────┬───┴────────┘
                      │                      │                      │
                 [SKID P]              [ALB Dock Conn]           [SKID S]
                                       Connector DOWN
```

#### Station 3 · Stern Main Drive & Power Vault (Z = −4.0 sb)
Transverse section looking forward (+Z) through the aft power and propulsion block:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L5                          ┌─────────────────┐
                             │  [ALB Antenna]  │ (15 km Comms)
 L4      ┌──────────┬────────┼─────────────────┼────────┬──────────┐
         │OutriggerP│[BAT 1] │  [ALB Reactor]  │[BAT 4] │OutriggerS│
 L3      ├──────────┼────────┼─────────────────┼────────┼──────────┤
         │Main Fwd 1│[BAT 2] │  [PB Utility]   │[BAT 3] │Main Fwd 4│
 L2      ├──────────┼────────┼─────────────────┼────────┼──────────┤
         │Lift 5 (D)│MainFwd2│  [Conveyor Hub] │MainFwd3│Lift 8 (D)│
 L1      └───┬──────┴────────┴─────────────────┴────────┴──────┬───┘
             ▼                                                 ▼
       [STERN SKID P]                                    [STERN SKID S]
```

---

### 0.5 · Master Conveyor & Component Feed Schematic

The Albatross incorporates a high-throughput small/large conveyor manifold connecting the belly connector, component buffer hold, welder head, and reactor:

```
                           [ALB Welder Nose] (1×2×2 sb)
                           (Bow Tool Head · Level L2-L3)
                                         │
                                         ▼ (Large Conveyor Port)
                           [ALB Hold Components]
                   (Medium Cargo Container 3×3×3 sb · Centered)
                   (Volume: 3,375 L · Plates, Motors, Computers)
                   (Manual Access Hatch on Port Flank Level L3)
                                         │
                     ┌───────────────────┴───────────────────┐
                     ▼                                       ▼
             [Conveyor Core Hub]                      [ALB Reactor]
           (Level L2 Centerline)                  (Level L3 Aft Centerline)
                     │                                (0.5 MW Backup)
                     ▼ (Bottom Port)
              [ALB Dock Conn]
          (Small Grid Connector sb)
          (Flush Level L1 at X0, Z0)
                     │
    ═════════════════╪═════════════════  Landing Plane
                     │
                     ▼ (Mates to Pad)
     [ORC Bay S2]    OR    [MAS Roof Dock]    OR    [GND PAD]
    (Orca Carrier)         (Mastodon Consist)       (Ground Base)
```

---

### 0.6 · Master Electrical Single-Line Schematic & 5 Power Modes

```
                  4× SMALL BATTERIES + 1× SMALL REACTOR
               2.0 MWh Battery Capacity · 4.5 MW Combined Surge
                                  │
                                  ▼
                          PRIMARY 4.5 MW BUS
        ┌─────────────────────────┼─────────────────────────┐
        │                         │                         │
        ▼                         ▼                         ▼
  8× DOWNWARD LIFT CLUSTER  14× MANEUVERING ARRAY     TOOL HEAD & AVIONICS
  8× Small Atmo (sb)        14× Small Atmo (sb)       1× Ship Welder (0.40 MW)
  (3.20 MW Max Burst)       (1.12 MW Max Burst)       1× RC + 1× PB (0.04 MW)
  [Empty Hover: 0.58 MW]    [Braking Burst: 0.32 MW]  1× Gyro (0.02 MW)
  [Laden Hover: 1.27 MW]    [Push-Down: 0.16 MW]      Antenna + 2× Cam (0.04 MW)
```

#### Operational Energy Modes
1. **Mode 1 · Docked Fast Shore Recharge (Recharge = ON):**
   * Total Draw: `0.00 MW` · Shore Intake: `4.0 MW` · Full 0 to 100% recharge in `30.0 minutes`.
2. **Mode 2 · Empty Utility Transit (80 m/s):**
   * Lift Load: 13.0% (580 kW) · Forward Thrusters: 320 kW · Avionics: 25 kW · Total Draw: `925 kW` · Battery Endurance: `2.16 hours` on battery alone.
3. **Mode 3 · Fully Laden Component Resupply (18.5 t):**
   * Lift Load: 28.4% (1.27 MW) · Forward Thrusters: 320 kW · Avionics: 25 kW · Total Draw: `1.615 MW` · Battery Endurance: `1.24 hours`.
4. **Mode 4 · Active Point-Blank Structural Welding Burst:**
   * Welder active (400 kW) + 8× lift hover (1.27 MW) + Work Spotlights (10 kW) = `1.68 MW` total draw (Burst capability up to `2.10 MW` during evasive maneuver).
5. **Mode 5 · Emergency Deadman RTB / Reserve Glide:**
   * Battery stored $\le 20\%$. Welder severed; reactor supplies 0.5 MW continuous backup; speed capped at 35 m/s; automated RTB to Orca Bay S2 or Ground Base.

---

### 0.7 · Master Bill of Materials (BOM) & Physics Budget

| Category | Component Description | Block Qty | Mass (t) | PCU | Critical Mounting / Orientation |
|---|---|:---:|:---:|:---:|---|
| **Fuselage** | Small Armor Blocks, Slopes & Half-Blocks | 58 | 2.80 | 58 | Palette 4 Cobalt, Slot 0 Slate, Slot 9 Yellow |
| **Propulsion** | Small Atmospheric Thrusters (Small Grid) | 22 | 2.20 | 330 | 8 Lift (D), 4 Main (Aft), 4 Rev (Bow), 4 Lat, 2 Down |
| **Tool Head** | Ship Welder (Small Grid) | 1 | 0.40 | 150 | Centerline bow [X0, Z +5, L2-L3], facing Bow |
| **Cargo** | Medium Cargo Container (3×3×3 sb) | 1 | 0.45 | 30 | Centerline hold [X0, Z 0, L2-L4] (3,375 L volume) |
| **Conveyors** | Small Conveyor Junctions & Tubes | 4 | 0.16 | 20 | Centerline routing hub connecting hold to tool |
| **Power** | Small Grid Batteries (2.0 MWh aggregate) | 4 | 1.00 | 80 | Aft bays at [X ±1, Z −2 to −3, L3] |
| **Power** | Small Nuclear Reactor (0.5 MW backup) | 1 | 0.15 | 25 | Aft centerline [X0, Z −4, L3] |
| **Attitude** | Micro Gyroscope (Small Grid) | 1 | 0.08 | 15 | Centerline avionics [X0, Z +1, L4] |
| **Compute** | Programmable Block (`[ALB] PB Utility`) | 1 | 0.08 | 25 | Centerline rack [X0, Z −1, L4] |
| **Avionics** | Remote Control Block | 1 | 0.05 | 25 | Centerline deck [X0, Z +2, L4], Fwd = +Z |
| **Comms** | Radio Antenna (Small Grid) | 1 | 0.04 | 25 | Aft mast [X0, Z −4, L5] (15,000 m broadcast†) |
| **Optics** | Fixed Cameras | 2 | 0.04 | 30 | 1 Welder Reticle [X0, Z+5, L4], 1 Belly [X0, Z+1, L1] |
| **Lighting** | Small Grid Spotlights & Beacons | 4 | 0.08 | 40 | 2 Work Spotlights Bow, 2 Wingtip Beacons |
| **Docking** | Small Grid Connector | 1 | 0.16 | 25 | Centered at [X0, Z0, L1], facing DOWN |
| **Logic** | Event Controllers & Timer Blocks | 3 | 0.09 | 60 | Aft equipment rack (`ALB EC`, `ALB TMR`) |
| **TOTAL** | **Complete ALBATROSS-01 Utility Craft** | **104** | **≈ 8.50 t** | **958** | **High-authority field repair craft** |

#### Physics & Gravitational Lift Margins
* **Dry Mass:** `8.50 t` (Airframe + Systems)
* **Maximum Laden Mass:** `18.50 t` (Laden with 10.0 t steel components)
* **Downward Lift Thrust (8× Small Atmo sb):** $8 \times 80\text{ kN} = \mathbf{640\text{ kN}}$
* **Gravity Load (Earth 1.0 g = 9.81 m/s²):**
  * Empty: $8.50\text{ t} \times 9.81\text{ m/s}^2 = \mathbf{83.39\text{ kN}}$ (Hover Throttle = 13.0%)
  * Laden: $18.50\text{ t} \times 9.81\text{ m/s}^2 = \mathbf{181.49\text{ kN}}$ (Hover Throttle = 28.4%)
* **Thrust-to-Weight (T/W) Ratio:**
  * Empty T/W: $640.0 / 83.39 = \mathbf{7.67}$
  * Laden T/W: $640.0 / 181.49 = \mathbf{3.53}$ (Extremely high reserve margin for precision station-keeping!)
* **Braking Distance (50 km/h to 0 km/h):** 4× Reverse Thrusters (320 kN) decelerate 18.5 t laden mass at $17.3\text{ m/s}^2$ ($< 5.6\text{ m}$ stopping distance†).

---

### 0.7B · Maintenance Access & Field Ergonomics
1. **Manual Component Loading Port:** A flush hatch on the port flank at [X −2, Z 0, L3] provides direct physical access to the Medium Cargo Container's inventory port for manual tool or component extraction.
2. **Reactor Service Hatch:** A hinged armor panel at Level L4 [X0, Z −4] opens to allow quick manual uranium loading without removing hull armor.
3. **Side Impact Rub Rails & Grab Bars:** High-visibility yellow rub rails (Palette Slot 9) spanning X = ±4 sb protect the thruster nacelles from impact during docking in tight carrier bays and serve as personnel handholds.

---

## Phase 13-1 · Belly Docking Skid & Cargo Core (Levels L1-L2)

### Step 13.1 · Belly Connector & Reinforced Skids (L1)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Belly Dock  ·  Footprint: 9 W × 11 L × 1 H sb  ·  Level: L1     │
│ [PARTS TRAY] 16× Small Armor Half Blocks, 1× Connector (Small), 4× Conveyor Hub sb   │
│ [PALETTE] Skids: Slot 0 Industrial Slate (#2B303A), Connector: Slot 0                │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1 Small Grid Connector `ALB Dock Conn` at origin `[X: 0, Y: 0, Z: 0]` at Level L1, facing straight DOWN (−Y).
2. Install 4 Conveyor Junction cubes directly above the connector to form the primary logistics trunk.
3. Construct reinforced dual landing skids using Heavy Armor Half Blocks at X = ±3 sb, spanning from Z −4 to +4 sb.
4. Mount 1× Camera `ALB Cam Belly` recessed at [X0, Z +1, L1] facing straight DOWN.

```
Plan view, Level L1 (Skids & Connector):
        X: −3   −2   −1     0    +1   +2   +3
    Z: +4  [SKID P] ················· [SKID S]  ◄── Bow Skid Contact Points (Palette 0)
           │                             │
    Z: +1  │           [CAM BELLY]       │      ◄── Downward Docking Camera
    Z:  0  │           [CONN ↓]          │      ◄── Small Grid Connector (`ALB Dock Conn`)
           │        (Facing Down)        │          (Centered X0, Z0, L1)
    Z: −4  [SKID P] ················· [SKID S]  ◄── Stern Skid Contact Points
```

---

### Step 13.2 · Medium Cargo Container & 8-Thruster Lift Array (L1-L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Cargo & Lift  ·  Midship Bay  ·  Level: L1-L2                   │
│ [PARTS TRAY] 1× Medium Cargo Container (3×3×3 sb), 8× Small Atmospheric Thrusters    │
│ [PALETTE] Hold: Slot 5 Emerald (#27AE60), Lift Thruster Cowlings: Slot 0 Slate Gray  │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1 Medium Cargo Container `ALB Hold Components` (3×3×3 sb) directly above the conveyor hub at [X0, Z 0, L2-L4].
2. Mount 8 Small Atmospheric Thrusters into the port and starboard outrigger bays, all exhausting straight DOWN (−Y):
   * `ALB Thruster Lift 1, 2` : Forward-Port [X −3 to −2, Z +3, L1-L2].
   * `ALB Thruster Lift 3, 4` : Forward-Starboard [X +2 to +3, Z +3, L1-L2].
   * `ALB Thruster Lift 5, 6` : Aft-Port [X −3 to −2, Z −3, L1-L2].
   * `ALB Thruster Lift 7, 8` : Aft-Starboard [X +2 to +3, Z −3, L1-L2].
3. Assign all 8 to terminal group `ALB Thrusters Lift`.

```
Plan view, Level L1-L2 (Lift Thrusters & Cargo Core):
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3  [LIFT 1][LIFT 2] ┌───┐ [LIFT 3][LIFT 4]  ◄── Forward Downward Thrusters
    Z:  0         │         │MED│         │          ◄── Medium Cargo Hold (3×3×3 sb)
    Z: −3  [LIFT 5][LIFT 6] └───┘ [LIFT 7][LIFT 8]  ◄── Aft Downward Thrusters
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 13.1A: DOCKING CLEARANCE & LIFT GAP GATE**
> 1. Ensure `ALB Dock Conn` face sits flush with the skid baseline (0.0 to 0.1 m clearance).
> 2. Verify large conveyor ports on Medium Cargo connect directly to the bottom connector hub and the forward tool port.
> 3. Verify all 8 downward lift thrusters have a clear 1 sb exhaust gap beneath the nozzles.

---

## Phase 13-2 · Bow Welder Tool Head & Propulsion Ring (Levels L2-L4)

### Step 13.3 · Industrial Welder Tool Head & Target Optics (L2-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tool Head  ·  Bow Centerline  ·  Level: L2-L3, Z +4 to +5       │
│ [PARTS TRAY] 1× Ship Welder (Small Grid), 1× Camera, 2× Spotlights, 1× Sensor        │
│ [PALETTE] Welder Cowling: Slot 1 Hazard Amber (#E6A100), Optics: Slot 8 White        │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1× Ship Welder `ALB Welder Nose` on the bow centerline at [X0, Z +5, L2-L3] mating directly to the forward conveyor port of the Medium Cargo Container.
2. Mount 1× Camera `ALB Cam Welder` directly above the tool tip at [X0, Z +5, L4] with reticle crosshairs aligned with the weld field.
3. Mount 2× Spotlights `ALB Light Work Port / Stbd` at [X ±2, Z +5, L4] angled 5° inward to illuminate the target block.
4. Mount 1× Sensor `ALB Sensor WeldZone` at [X0, Z +4, L3]. Set detection field to 2.0 m forward.

```
Front elevation, looking Stern at Welder Tool Head (Z = +5):
       L4       [SPOTLIGHT P]  [ALB CAM RETICLE]  [SPOTLIGHT S]
       L3     ┌─────────────────────────────────────────────────┐
       L2     │          [ALB WELDER NOSE (1×2×2 sb)]           │
       L1     ■═════════════════════════════════════════════════■
```

---

### Step 13.4 · 14-Thruster 6-DOF Maneuvering Ring (L2-L4)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Maneuver Ring  ·  Perimeter Sponsons  ·  Level: L2-L4           │
│ [PARTS TRAY] 14× Small Atmospheric Thrusters (Small Grid · 80 kN each)               │
│ [PALETTE] Slot 0 Slate Gray (#2B303A), Nacelle Trim: Slot 4 Cobalt Blue              │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Mount 14 maneuvering thrusters across the outer sponsons:
1. **Forward Cruise (4×):** `ALB Thruster Fwd 1..4` at [X ±2 to ±3, Z −5, L2-L3] facing Aft (−Z) to push Bow (+Z) (320 kN main drive).
2. **Reverse Braking (4×):** `ALB Thruster Rev 1..4` at [X ±2 to ±3, Z +4, L2-L3] facing Bow (+Z) to push Aft (−Z) (320 kN braking).
3. **Port Translation (2×):** `ALB Thruster Port 1, 2` at [X −4, Z ±1, L3] facing Port (−X) to push Starboard (+X).
4. **Starboard Translation (2×):** `ALB Thruster Stbd 1, 2` at [X +4, Z ±1, L3] facing Starboard (+X) to push Port (−X).
5. **Zenith Push-Down (2×):** `ALB Thruster Down 1, 2` at [X ±2, Z 0, L4] facing UP (+Y) to push straight DOWN (−Y).
6. Assign all 14 to terminal group `ALB Thrusters Maneuver`.

```
Plan view, Level L3 (Horizontal Thruster Layout):
        X: −4   −3   −2     0    +2   +3   +4
    Z: +4  [REV 1]   [REV 2] [WELDER] [REV 3]   [REV 4]  ◄── Facing Bow +Z (Braking)
    Z: +1  [PORT 1]                           [STBD 1]   ◄── Facing Outward (Lateral)
    Z: −1  [PORT 2]                           [STBD 2]
    Z: −5  [FWD 1]   [FWD 2]         [FWD 3]   [FWD 4]  ◄── Facing Aft −Z (Main Drive)
```

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 13.2A: WELDER PULL & REVERSE BRAKE GATE**
> 1. Deposit 500 Steel Plates into `ALB Hold Components`; verify `ALB Welder Nose` pulls components automatically when welding test armor.
> 2. Test reverse braking: confirm craft decelerates from 50 km/h to dead stop in $< 12.0\text{ m}$†.
> 3. Verify `ALB Sensor WeldZone` triggers welder activation only when damaged blocks are within 2.0 m.

---

## Phase 13-3 · Power Vault, Autonomous Avionics & Comms (Levels L3-L5)

### Step 13.5 · 4-Battery Power Vault & Avionics Spine (L3-L4)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Power & Avionics  ·  Aft Centerline  ·  Level: L3-L4            │
│ [PARTS TRAY] 4× Small Batteries, 1× Small Reactor, 1× RC, 1× Gyro, 1× PB, 1× Antenna │
│ [PALETTE] Batteries: Slot 6 Alert Red (#C0392B), Avionics: Slot 7 Fleet Navy         │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Install 4 Small Grid Batteries `ALB Battery 1..4` in a 2 × 2 bank directly aft of the cargo hold at [X ±1, Z −2 to −3, L3].
2. Install 1 Small Nuclear Reactor `ALB Reactor` at [X0, Z −4, L3] plumbed into the aft fuel line.
3. Mount 1× Remote Control `ALB Remote Control` at [X0, Z +2, L4]. Ensure Forward = Bow (+Z) and Up = Roof (+Y).
4. Mount 1× Micro Gyroscope `ALB Gyro` at [X0, Z 0, L4] directly above the Center of Mass.
5. Mount 1× Programmable Block `[ALB] PB Utility` at [X0, Z −1, L4].
6. Mount 1× Radio Antenna `ALB Antenna` at [X0, Z −4, L5]. Set broadcast range to `15,000 m`†.

```
Plan view, Level L4 (Power Vault & Avionics Core):
        X: −2   −1    0   +1   +2
    Z: +2  ┌─────┬───────────┬─────┐
           │     │ [REMOTE]  │     │  ◄── Autopilot Brain (`ALB Remote Control`)
    Z:  0  │     │  [GYRO]   │     │  ◄── Precision Stabilization Gyro
    Z: −1  │     │ [PB UTIL] │     │  ◄── Zero-GC Maintenance Script
    Z: −3  │[BAT 1][BAT 2][BAT 3][BAT 4]  ◄── 4× Small Batteries (Palette Slot 6: Red)
    Z: −4  └─────┴─[REACTOR]─┴─────┘  ◄── Small Nuclear Reactor (0.5 MW backup)
```

---

### Step 13.6 · Armored Utility Canopy & High-Viz Rub Rails (L4-L5)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Outer Armor  ·  Canopy & Sponsons  ·  Level: L4-L5              │
│ [PARTS TRAY] 26× Small Armor Slopes, 12× Heavy Armor Blocks (Rub Rails)              │
│ [PALETTE] Hull: Slot 4 Cobalt Blue (#2980B9), Rails: Slot 9 High-Viz Yellow         │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Enclose the upper avionics deck with reinforced sloped armor fairings in Manufacturing Cobalt (Slot 4).
2. Install continuous High-Viz Yellow (Slot 9) rub rails along the outer flanks at X = ±4 sb spanning Z −4 to +4.
3. Install top armor louvers over the battery bank and reactor to shield against falling gravel and debris.
4. Mount amber hazard beacons at [X ±4, Z +3, L4] and [X ±4, Z −3, L4].

```
Plan view, Level L5 (Utility Roof Canopy & Rub Rails):
        X: −4   −2    0   +2   +4
    Z: +5       ┌────[WELDER]────┐       ◄── Heavy Nose Bumper
    Z: +3  [RAIL]┌──────────────┐[RAIL]  ◄── High-Viz Yellow Side Rub Rails (Palette 9)
    Z:  0  │    │  ROOF CANOPY  │    │
    Z: −3  │    │ (COBALT BLUE) │    │
    Z: −5  [RAIL]└──[ANTENNA]───┘[RAIL]  ◄── Long-Range Comms Mast
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 13.3A: AVIONICS RIGGING & POWER SURGE GATE**
> 1. Verify `ALB Remote Control` artificial horizon reads 0.00° pitch and roll when parked on level deck.
> 2. Ensure `[ALB] PB Utility` compiles clean under MDK2 with zero runtime allocations.
> 3. Verify total electrical surge delivers 4.5 MW peak with zero brownout under simultaneous all-thruster burst and welder ignition.

---

## Phase 13-4 · Master Control Logic, AI Scripting Contract & Software Abstraction

### 1. Terminal Group Taxonomy
* `ALB Thrusters All` : All 22 atmospheric thrusters.
* `ALB Thrusters Lift` : 8× Downward lift thrusters (`ALB Thruster Lift 1..8`).
* `ALB Thrusters Maneuver` : 14× Maneuvering thrusters (Fwd, Rev, Port, Stbd, Down).
* `ALB Batteries` : 4× Small Grid Batteries (`ALB Battery 1..4`).
* `ALB Lights` : 2× Work Spotlights + 2× Hazard Beacons.
* `ALB Tools` : `ALB Welder Nose` (main structural repair head).
* `ALB Cameras` : `ALB Cam Welder` (reticle target) + `ALB Cam Belly` (docking align).

---

### 2. Event Controller Automation Ladder (Rule C4)

| Controller | Monitored Parameter | Threshold / Event | Slot 1 Action (True) | Slot 2 Action (False) |
|---|---|---|---|---|
| `ALB EC Battery` | `ALB Batteries` Stored Power | Stored Charge $< 25.0\%$ | Trigger RTH (Return to Bay/Pad) | Normal Operation |
| `ALB EC Cargo` | `ALB Hold Components` Volume | Volume Filled $> 90.0\%$ | Alert C2 Bridge: Hold Full | Normal Capacity |
| `ALB EC Dock` | `ALB Dock Conn` State | Connector Status = Locked | Batteries $\rightarrow$ Recharge · Thrusters OFF | Batteries $\rightarrow$ Auto · Thrusters ON |
| `ALB EC Welder` | `ALB Sensor WeldZone` | Damaged Block $\le 2.0\text{ m}$ | `ALB Welder Nose` Turn ON | `ALB Welder Nose` Turn OFF |

---

### 3. Native Block Failsafe Matrix (Zero-PB Safety)
If `[ALB] PB Utility` crashes, freezes, or encounters an uncaught runtime exception:
1. `ALB TMR Heartbeat` (5.0 s loop) fails to receive a PB reset pulse and expires.
2. Immediate hardwired failsafe actions triggered by timer:
   * Remote Control Autopilot `Collision Avoidance` = ON.
   * Remote Control `Dampeners` = ON.
   * Gyro and Thrust Overrides = OFF.
   * `ALB Welder Nose` = OFF (prevents runaway welding or component depletion).
   * Craft enters stationary hover at current altitude until operator overrides via C2 console.
3. If docked at host pad when failure occurs:
   * Belly connector remains magnetically locked.
   * Thrusters remain locked OFF to prevent pad damage.

---

### 4. IGC Broadcast Network Dictionary

```
TMS.UTIL.STATUS (Broadcast every 1.66 s):
ALB-01;<state_enum>;<hold_pct>;<battery_pct>;<gps_coords>
Example: ALB-01;WELDING;45.0;78.5;GPS:DamagePoint:12450.0:-34100.0:210.0:

TMS.REPAIR.CALL (Inbound dispatch trigger from damaged fleet drone):
CALL;ALB-01;<target_drone_id>;<damage_gps>;<urgency>
Example: CALL;ALB-01;REM-02;GPS:RemoraDamaged:12480.0:-34150.0:180.0:;CRITICAL

TMS.SUPPLY.RESTOCK (Outbound order to carrier or base inventory):
RESTOCK;ALB-01;<req_plates>;<req_motors>;<req_computers>
Example: RESTOCK;ALB-01;500;100;50
```

---

## Phase 13-5 · Commissioning Checklist & Rigorous Test Protocols (RIG Test Creative)

### Suite A · Dry Mass & Precision Station-Keeping Calibration
* [ ] **Dry Mass Verification:** Spawn ALBATROSS-01 in creative; terminal reports dry mass `≈ 8.5 t`.
* [ ] **Point-Blank Hover Stability:** Hover 1.5 m from an Orca hull block; release flight sticks; verify craft drifts $< 0.05\text{ m}$ over 60 seconds in Earth 1.0 g.
* [ ] **Empty Hover Power:** Verify 8× lift thrusters operate at `12-14%` load (`≈ 550-600 kW` draw).

### Suite B · Laden Flight & Component Ferry Dynamics (18.5 t)
* [ ] **Full Component Loading:** Fill `ALB Hold Components` with 10.0 t of steel plates and construction components (Gross mass `≈ 18.5 t`).
* [ ] **Laden Hover Power:** Verify 8× lift thrusters hover cleanly at `28-30%` load (`≈ 1.25-1.30 MW` draw). Confirm T/W margin `≥ 3.50`.
* [ ] **Braking Distance Verification:** Fly at 50 km/h laden; engage full reverse thrust; confirm craft decelerates to a dead stop in $< 12.0\text{ m}$† ($a \ge 16.0\text{ m/s}^2$).

### Suite C · Automated Proximity Welding & Component Feed
* [ ] **Automated Repair Trial:** Approach a damaged Remora driller frame; confirm `ALB Sensor WeldZone` detects damage at 2.0 m and auto-ignites `ALB Welder Nose`.
* [ ] **Conveyor Draw Test:** Verify welder pulls steel plates continuously from `ALB Hold Components` without flow interruption; confirm full structural repair completes in $< 10\text{ seconds}$.

### Suite D · Orca Carrier Bay S2 Ingress & Docking Validation
* [ ] **Bay S2 Ingress:** Approach Orca Bay S2 at hover; align with bay guide curbs; verify lateral clearance margin is $\ge 1.5\text{ B}$ (3 sb) on all sides.
* [ ] **Connector Lock & Power Shutdown:** Touch down on Bay S2 floor dock; confirm `ALB Dock Conn` locks on first contact; verify batteries switch to Recharge and thrusters cut off instantly.

### Suite E · Mastodon Expeditionary Consist Roof Pad Capture
* [ ] **Roof Pad Docking:** Approach Mastodon Carriage 2 roof deck; align `ALB Cam Belly` with `MAS MP Conn`; touch down and verify magnetic latch.
* [ ] **Consist Resupply Extraction:** Order component transfer from Mastodon FOB workshop into `ALB Hold Components`; verify restock completes in $< 15\text{ seconds}$.

### Suite F · Ground Base Logistics Apron Fast Shore Recharge
* [ ] **Base Terminal Capture:** Touch down on Ground Base utility gantry pad; confirm 4× Small Batteries pull full 4.0 MW shore charge, reaching 100% state of charge in $< 30\text{ minutes}$.

### Suite G · Native Heartbeat Loss & Emergency Deadman Hover Fallback
* [ ] **Simulated PB Crash:** Recompile `[ALB] PB Utility` with an empty script during 50 m/s flight; verify `ALB TMR Heartbeat` times out at 5.0 s, dampeners engage, overrides cut, welder turns off, and craft halts into stable altitude hold.

---

## Phase 13-6 · Unverified Values Tracking (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V50:** ALBATROSS-01 laden braking distance from 50 km/h on 4× Reverse Atmo thrusters ($< 12.0\text{ m}$†).

---

# Volume II · Surface Infrastructure & Industrial Core

## Chapter 4 · Ground Base, Refinery Complex & Fleet Command Center (C2)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **Ground Base, Refinery Complex & Fleet Command Center (GND-01)**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 38, 41, 45, 46, 49, 51, 52, 56, 60 & 66)
* **Role:** Planetary industrial foundry, dual-tier logistics terminal, and permanent regional **Command & Control (C2) Fleet Root**. Receives high-value rare ores from aerial shuttles (MULE-01) and ultra-high-volume bulk ores from land trains (CATERPILLAR-01); refines raw ores at 200% yield; fabricates ship components at 400% speed; continuously feeds the Rig Linear Extruder Ring shipyard; and orchestrates all planetary fleet assets through an elevated 3-operator panoramic bridge.
* **Architecture:** 4 Integrated Functional Zones on a 24 × 28 B foundation (60.0 × 70.0 m · 22.5 m height):
  1. **West Zone (X −11 to −7): Elevated C2 Command Tower & Server Vault (Levels L1-L9):** Heavy structural support pillar (L1), isolated Fleet Operations Server Room (L2) with 3× Programmable Blocks and dedicated uninterruptible power supply (`GND BAT C2 UPS`), elevated panoramic walk-around bridge deck (L3-L4) with 6-panel LCD Video Wall, 3 operator consoles (Air Ops, Ground Ops, Yard Ops), pressurized decontamination airlock, Tier 4 mobile armory, and a 10 m communications mast (L5-L9) mounting a 50 km omni antenna and high-altitude Floating Rig laser transceiver.
  2. **North Zone (Z +6 to +13): Dual-Tier Terrestrial Logistics Apron (Levels L1-L2):** 
     * **Air Tier (Z +6 to +12, X 0 to +6):** Flush 7 × 7 B heavy armor landing pad (`GND PAD`) for MULE-01 with motorized gantry clamping arms (`GND PAD Cradle`) and high-speed rare ore sorter manifold (U, Pt, Au, Mg).
     * **Ground Tier (Z +6 to +12, X −6 to −1):** Heavy rail receiving trench for CATERPILLAR-01 with blast door guide curbs, stop bumpers, large receiving connector (`GND CAT Dump Conn`), and 5× flush in-deck collectors draining 2,500 t bulk ore in `< 90 seconds`.
  3. **Central Zone (Z −5 to +5, X −6 to +6): Industrial Smelting & Fabrication Foundry (Levels L1-L2):** 
     * **Smelting Core:** 4× Large Refineries + 16× Yield Modules (200% yield) divided into Rare (West Bank: Ref 1 & 2) and Bulk (East Bank: Ref 3 & 4) processing streams.
     * **Manufacturing Core:** 4× Large Assemblers + 16× Speed Modules (400% productivity) operating in cooperative master/slave mode.
  4. **South Zone (Z −13 to −7, X −6 to +6): Power Substation & Shipyard Extruder Feed (Levels L1-L3):** Master component buffer warehouse (Large Cargo Container `GND STR Components`), high-pressure Extruder Ring feed line with automated push-sorter, and a primary planetary power substation (6× Large Batteries 18 MWh + 2× Auxiliary Small Nuclear Reactors 30 MW).
* **Grid:** Large Grid (1 B = 2.5 m) · **Footprint:** 24 W × 28 L × 9 H B (60.0 × 70.0 × 22.5 m).
* **Mass:** Facility Structural Mass `≈ 948.5 t` · Installed Power Capacity: 30 MW Continuous Nuclear + 72 MW Battery Surge.
* **Operating Environment:** Earth 1 g baseline, planetary surface level, quarry bench connection.

---

## 0 · Reference Grid, In-Game Paint Palette & Master Facility Layout

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the foundation deck surface at Level L1 along centerline X0.
* **X (Beam / Transverse):** West is negative (−), East is positive (+), Centerline is 0. Spans X −12 to +12 (24 blocks wide = 60.0 m).
  * West Flank (C2 Tower): X −11 to −7 (5 blocks wide = 12.5 m).
  * Centerline Logistics & Process Core: X −6 to +6 (13 blocks wide = 32.5 m).
  * East Retaining Wall & Expansion Apron: X +7 to +11 (5 blocks wide = 12.5 m).
* **Y (Elevation):** Sub-surface bedrock anchor and trench raceways are Level L0 (Y = −2.5 m); tarmac foundation surface is Level L1 (Y = 0); industrial deck midline is Level L2; C2 Bridge observation deck is Level L3; bridge ceiling is Level L4; comms spire tip is Level L9 (Y = +22.5 m).
* **Z (Length / Longitudinal):** South (toward Extruder Ring) is negative (−), North (toward Logistics Approach) is positive (+). Spans Z −14 to +14 (28 blocks long = 70.0 m).
  * North Logistics Apron (Mule Pad & Cat Dock): Z +6 to +13 (8 blocks length = 20.0 m).
  * Central Industrial Core (Refineries & Assemblers): Z −6 to +5 (12 blocks length = 30.0 m).
  * South Power Vault & Buffer Storage: Z −13 to −7 (7 blocks length = 17.5 m).
  * Extruder Yard Interface: Z = −14.

```
Spatial Alignment Triad:
       [+Y: Zenith / Comms Spire L9]
              │
              │   [+Z: North / Approach / Logistics Apron]
              │   /
              │  /
              │ /
  [−X: West / C2 Tower] ─┼───────── [+X: East / Expansion Apron]
             /│
            / │
           /  │
  [−Z: South / Extruder Ring Shipyard]
              │
       [−Y: Bedrock Anchor / Level L0]
```

---

### 0.2 In-Game Paint Palette & Functional Color Coding

To eliminate builder disorientation across a 60 × 70 m industrial installation, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Heavy armor foundation tarmac, bedrock anchors, retaining walls |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | MULE-01 touchdown deck (7×7 B), gantry cradle arms, pad borders |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | CATERPILLAR-01 unload trench, wheel curbs, crash bumpers |
| **Slot 3** | Processing Teal | `#1ABC9C` | Bright Teal | 4× Large Refineries, 16× Yield Modules, raw ore intake manifolds |
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | 4× Large Assemblers, 16× Speed Modules, refined ingot feed trunk |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Component Buffer container, Extruder feed conveyor trunk |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Vault, 6× Large Batteries, 2× Auxiliary Nuclear Reactors |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | C2 Operations Bridge exterior shell, server rack consoles |
| **Slot 8** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel catwalks, stairwells, maintenance access hatches |
| **Slot 9** | Clean Armor White | `#ECF0F1` | Clean White | C2 Bridge interior ceiling, panoramic window trim, comms spire |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

To achieve a cohesive aesthetic matching the Mastodon MKI locomotive and Floating Rig, exterior surfaces follow strict functional zoning boundaries:

```
[EXTERIOR ELEVATION LIVERY MAP · LOOKING WEST AT FACILITY]
Level
L9  │                         [SLOT 9: RIG LASER RELAY SPIRE]
L8  │                                       │
L7  │                         [SLOT 7: FLEET OMNI ANTENNA (50 km)]
L6  │                                       │
L5  │                      ═══[SLOT 9: WHITE C2 BRIDGE ROOF CROWN]═══
L4  │                     │   [SLOT 7: NAVY SLOPED GLASS CANOPY]   │
L3  │ ════════════════════[SLOT 7: NAVY C2 BRIDGE & WALK-AROUND DECK]═════════════════════
L2  │ [SLOT 1: MULE GANTRY]  [SLOT 3: TEAL REFINERY]  [SLOT 4: COBALT ASM]  [SLOT 6: POWER]
L1  │ ════════════════════[SLOT 0: INDUSTRIAL DARK GRAY HEAVY FOUNDATION TARMAC]═══════════
L0  │ ────────────────────[SLOT 0: SUB-SURFACE CONVEYOR TRENCH RACEWAYS]───────────────────
         Z: +11 (North Apron)      Z: +3 (Refining)       Z: -3 (Fabrication)  Z: -10 (Storage)
```

| Zone / Component | Primary Palette Slot | Hex Code | Visual Application & Boundaries |
|---|---|---|---|
| **Foundation Tarmac** | Slot 0 (Industrial Dark Gray) | `#2B303A` | Level L1 perimeter blast deck, retaining walls, sub-surface trench sills |
| **Mule Gantry Pad** | Slot 1 (Hazard Amber) | `#E6A100` | 7×7 B blast deck at [X +3, Z +9] with alternating black/amber warning stripes |
| **Caterpillar Dock** | Slot 2 (Heavy Rail Crimson) | `#9E2A2B` | Level L1 trench curbs, rear alignment wedges, and collision stop blocks |
| **Refining Core** | Slot 3 (Processing Teal) | `#1ABC9C` | 4× Large Refineries, 16× Yield Modules, raw ore manifolds (Z +1 to +4) |
| **Manufacturing Core**| Slot 4 (Manufacturing Cobalt) | `#2980B9` | 4× Large Assemblers, 16× Speed Modules, ingot manifolds (Z −5 to −1) |
| **Component Storage** | Slot 5 (Logistics Emerald) | `#27AE60` | Large Cargo buffer and southern Extruder feed raceway (Z −11 to −7) |
| **Power Substation** | Slot 6 (High-Voltage Red) | `#C0392B` | 6× Large Batteries, 2× Small Reactors, transformer bus (Z −11 to −7) |
| **C2 Command Tower** | Slot 7 (Fleet Command Navy) | `#1B263B` | Levels L2-L5 elevated bridge shell, server room, and structural pylon |
| **Comms Spire & Mast** | Slot 9 (Clean Armor White) | `#ECF0F1` | Levels L5-L9 antenna mast, laser transceiver mount, and aircraft warning beacons |

---

### 0.3 Master Facility Top Plan View (Levels L1-L4)

```
[MASTER FACILITY COMPOSITE PLAN VIEW · LEVELS L1-L4 (24 × 28 B · 60 × 70 m)]
        X: −11  −9    −6    −4    −1     0    +2    +4    +6    +9   +11
Z: +13 ┌─────┬─────┬─────┬─────┬─────┬═══════════════════┬─────┬─────┬─────┐
       │     │     │   CATERPILLAR   │  MULE GANTRY PAD  │     │     │     │
Z: +11 │     │     │   UNLOAD DOCK   │   (7×7 B DECK)    │     │     │     │
       │     │     │   [5× COLL]     │   [GND PAD CONN]  │     │     │     │
Z: +9  │     │     │   [STOP BUMP]   │   [CRADLE ARMS]   │     │     │     │
       │     │     │   [CAT DUMP]    │   (AMBER BORDER)  │     │     │     │
Z: +7  │     │     ├─────┴─────┬─────┼═════════╤═════════┼─────┼─────┤     │
       │     │     │           │     │         │ Ore Riser │     │     │     │
Z: +5  │     │     │┌────────┐ │ ┌───┴─────────┴────────┐│     │     │     │
       │     │     ││ REF 1  │ │ │ REF 3        REF 4   ││     │     │     │
Z: +3  │ C2  │ C2  ││ [2×4 B]│ │ │ [2×4 B]      [2×4 B] ││     │     │     │  ◄── 4× Large Refineries
       │ TWR │ BRG ││ REF 2  │ │ │                      ││     │     │     │      (16× Yield Modules)
Z: +1  │(L2) │(L3) ││ [2×4 B]│ │ │                      ││     │     │     │
       │     │     │└────────┘ │ └──────────────┬───────┘│     │     │     │
Z: −1  ├─────┼─────┼───────────┼────────────────┼────────┼─────┼─────┤     │
       │COMMS│SRVR │┌────────┐ │ ┌──────────────┴───────┐│     │     │     │
Z: −3  │MAST │ROOM ││ ASM 1  │ │ │ ASM 3        ASM 4   ││     │     │     │  ◄── 4× Large Assemblers
       │(L5) │(L2) ││ ASM 2  │ │ │                      ││     │     │     │      (16× Speed Modules)
Z: −5  ├─────┴─────┴┴────────┴═╪═╪══════════════╤═══════┴┴─────┼─────┤     │
       │  [POWER SUBSTATION]   │ │              │ Fabricated   │     │     │
Z: −7  │  [BAT 1] [BAT 2]      │ │              ▼ Components   │     │     │
       │  [BAT 3] [BAT 4]      │ │       ┌──────────────┐      │     │     │
Z: −9  │  [BAT 5] [BAT 6]      │ │       │[STR BUFFER]  │      │     │     │  ◄── Large Cargo (3×3×3 B)
       │  [RCT 1] [RCT 2]      │ │       │(3×3×3 B)     │      │     │     │      (Component Buffer)
Z: −11 │ (30 MW NUCLEAR BACKUP)│ │       │[GND STR]     │      │     │     │
       └───────────────────────┴─┼───────┴──────┬───────┴──────┴─────┴─────┘
Z: −13                           │              │ Direct High-Pressure Conveyor Trunk
                                 │              ▼
                         Bulk Ore Return   Chapter 5 Rig Linear Extruder Ring (60 m South)
```

---

### 0.4 Master Facility Profile Elevation (Looking Starboard / East, X = −11 Plane)

```
[FACILITY PROFILE ELEVATION · WEST ELEVATION LOOKING STARBOARD / EAST]
Level
L9                     [GND LASER RIG RELAY]  ◄── Hard-Locked to Floating Rig
L8                               │
L7                               ├── [GND CAM SOUTH YARD]
L6                               ├── [GND ANTENNA FLEET (50 km)]
L5             ╔═════════════════╧═════════════════╗  ◄── C2 Bridge Roof Deck (White)
L4             ║  PANORAMIC COMMAND VIEWPORT GLASS ║  ◄── 6-Panel LCD Video Wall
L3             ║  [DESK 1: AIR] [DESK 2: GND] [DESK 3: YARD]  ◄── 3 Operator Consoles
L2             ╠═══════════════════════════════════╣  ◄── Pressurized Decon Airlock
               ║ FLEET OPS SERVER ROOM (3× PBs)    ║
L1  ═══════════■═════════════════╤═════════════════■═════════════════╤═══════════════
       [NORTH LOGISTICS APRON]   │       [INDUSTRIAL FOUNDRY]        │ [SOUTH STORAGE]
       · Mule Gantry Pad (L1)    │       · 4× Refineries (L1-L2)     │ · Cargo Buffer (L1-L3)
       · Caterpillar Dock (L1)   │       · 4× Assemblers (L1)        │ · Power Vault (L1-L2)
L0  ───[SUB-SURFACE TRENCH A]────┴───────[MAIN CONVEYOR SPINE]───────┴─[EXTRUDER LINE]
       ◄── Z: +13 (North) ─────────────── Z: 0 (Datum) ─────────────── Z: -13 (South) ─►
```

---

### 0.4B Facility 3/4 Isometric Perspectives & Architectural Wireframes

```
[3/4 ISOMETRIC WIREFRAME · NORTH LOGISTICS APRON (MULE PAD & CAT DOCK)]
                            +Y (Zenith L3)
                                  ▲
                                  │      ┌───[MULE GANTRY CRADLE]───┐  ◄── 30° Motorized Clamps
                                  │     ╱                          ╱│
                   -X (West) ◄────┼────►+X (East)                 │ │
                                 ╱│   ┌──────────────────────────┐│ │  ◄── 7×7 B Flush Deck (Amber)
                                ╱ │   │ [GND PAD CONN] (Belly)   ││ │
                               ╱  │   └──────────────────────────┘│╱
                              ▼   │   ═════════════════════════════
                         -Z (South)   ┌──────────────────────────┐
                                      │ CATERPILLAR UNLOAD DOCK  │     ◄── Heavy Rail Crimson Dock
                                      │ [CAT DUMP] [5× COLL]     │     ◄── 5× In-Deck Collectors
                                      └──────────────────────────┘

[3/4 ISOMETRIC WIREFRAME · C2 OPERATIONS TOWER & COMMS SPIRE]
                            +Y (Zenith L9)
                                  ▲
                                  │          [LASER TRANSCEIVER]       ◄── Level L9 Rig Uplink
                                  │                   │
                                  │          [50 km OMNI ANTENNA]      ◄── Level L6 Fleet Comms
                                  │                   │
                                  │         ┌───────────────────┐
                                  │        ╱                   ╱│      ◄── Sloped Glass Canopy (L4)
                                  │       ┌───────────────────┐ │
                                  │       │ C2 COMMAND BRIDGE │ │      ◄── 6-Panel LCD Wall (L3)
                                  │       ├───────────────────┤ │
                                  │       │ SERVER ROOM (3×PB)│ │      ◄── Avionics Rack & UPS (L2)
                                  │       ├───────────────────┤ │
                                  │       │ HEAVY PYLON (L1)  │ │      ◄── Blast-Proof Foundation
                                  │       └───────────────────┘╱
```

---

### 0.4C Transverse & Longitudinal Station Profiles

To maintain exact dimensional verification during survival assembly, cross-sections are specified at 6 critical facility stations:

```
Station 1 · North Approach Apron (Z = +13, Looking South)
Level
L2  │   ┌───[CAT ENTRY CURB]───┐               ┌───[MULE DECK PERIMETER]───┐
L1  │   │   TRENCH A INLET     │               │   AMBER BLAST COAMING     │
L0  │   └───[SUB-GRADE SLAB]───┘               └───[SUB-GRADE SLAB]────────┘
    │   ◄───── X: −6 to −1 ────►               ◄───── X: 0 to +6 ──────────►

Station 2 · Dual Intake Terminals (Z = +9, Looking South)
Level
L2  │   [STOP] [CAT DUMP CONN] [STOP]          [CRADLE P] [MULE] [CRADLE S]  ◄── 30° Clamps Engage
L1  │   ■══════[5× COLLECTORS]══════■          ■──────────[PAD CONN]──────■  ◄── Flush Receiving Ports
L0  │   └───[CONVEYOR TRENCH A]─────┘          └───[CONVEYOR TRENCH B]────┘

Station 3 · Heavy Smelting Array (Z = +3, Looking South)
Level
L2  │   ┌───────[YIELD 1..4]───────┐           ┌───────[YIELD 9..12]──────┐
    │   │ REFINERY 1 & 2 (WEST)    │           │ REFINERY 3 & 4 (EAST)    │  ◄── 4× Large Refineries
L1  │   ├──────────────────────────┤           ├──────────────────────────┤      (Teal, 200% Yield)
L0  │   └───[RARE ORE MANIFOLD]────┴───────────┴───[BULK ORE MANIFOLD]────┘

Station 4 · Cooperative Fabrication Core (Z = −3, Looking South)
Level
L2  │   ┌───────[SPEED 1..4]───────┐           ┌───────[SPEED 9..12]──────┐
    │   │ ASSEMBLER 1 & 2 (WEST)   │           │ ASSEMBLER 3 & 4 (EAST)   │  ◄── 4× Large Assemblers
L1  │   ├──────────────────────────┤           ├──────────────────────────┤      (Cobalt, 400% Speed)
L0  │   └───[REFINED INGOT TRUNK]──┴───────────┴───[COMPONENT OUTFLOW]────┘

Station 5 · Power Substation & Buffer Warehouse (Z = −9, Looking South)
Level
L3  │                                          ┌───[TOP INSPECTION PORT]──┐
L2  │   [BAT 1..6] [RCT 1..2]                  │  GND STR COMPONENTS      │  ◄── Large Cargo (3×3×3 B)
L1  │   ┌──────────────────────────┐           │  (LOGISTICS EMERALD)     │
L0  │   └───[POWER BUS TRENCH]─────┴───────────┴───[EXTRUDER FEED RISER]──┘

Station 6 · South Extruder Ring Interface (Z = −13, Looking South)
Level
L2  │                                          ┌───[GND SORTER EXT PUSH]──┐  ◄── Drain All = ON
L1  │   ═══════════════════════════════════════╧═══════════╤══════════════╧  ◄── Tarmac Sill
L0  │                                                      ▼ (Feeds Chapter 5 Extruder Pusher Spine)
```

---

### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

The facility routes materials through dedicated dual-tier smelting loops before aggregating into the master component buffer feeding the Chapter 5 Rig Extruder:

```
                            TERRESTRIAL INTAKE NODES
                                       │
       ┌───────────────────────────────┴───────────────────────────────┐
       ▼ (Tier 1: Air / Rare)                                          ▼ (Tier 2: Ground / Bulk)
[MULE-01 Belly Connector] (Z +9, X +3)                       [CATERPILLAR-01 Dump Conn] (Z +9, X −4)
[GND PAD Conn] (Facing UP)                                   [GND CAT Dump Conn] + 5× In-Deck Collectors
       │                                                               │
       ▼ (Trench B: Rare Stream)                                       ▼ (Trench A: Bulk Stream)
[GND Sorter Rare Intake]                                     [GND Sorter Bulk Intake]
(Whitelist: U, Pt, Au, Mg, Scrap)                           (Whitelist: Fe, Ni, Si, Co, Stone)
       │                                                               │
       ▼                                                               ▼
[GND REF 1 & 2 (West Bank)]                                  [GND REF 3 & 4 (East Bank)]
(4× Large Refineries · 16× Yield Modules)                    (4× Large Refineries · 16× Yield Modules)
       │                                                               │
       └───────────────────────────────┬───────────────────────────────┘
                                       │
                                       ▼ (Refined Ingot Aggregation Trunk)
       ┌───────────────────────────────┴───────────────────────────────┐
       ▼                                                               ▼
[GND ASM 1 & 2 (Structural Hull Run)]                        [GND ASM 3 & 4 (Machinery Run)]
(Steel Plates, Interior Plates, Construction Comps)          (Large Tubes, Motors, Computers, Thruster Comps)
(Cooperative Mode = Master)                                  (Cooperative Mode = Slave)
       │                                                               │
       └───────────────────────────────┬───────────────────────────────┘
                                       │
                                       ▼ (Fabricated Components Trunk)
                           [GND STR Components]
                           (Large Cargo Container 3×3×3 B · Z −10, X +3)
                             │ ◄── Manual Component Withdrawal Hatch (Port L2)
                             ▼
                           [GND Sorter Extruder Feed] (Conveyor Sorter · Drain All = ON)
                             │
                             ▼ (High-Pressure Conveyor Conduit South)
                   [EXT Welder Ring] (Chapter 5 Shipyard · Z −14)
```

### Physical Maintenance Ports & Manual Access Hatches
To comply with survival maintenance standards and allow physical clearance of conveyor bottlenecks:
* **Port 1 (Mule Intake):** Standard small conveyor port at [X +3, Z +7, L1] for manual inspection of rare ore intake.
* **Port 2 (Caterpillar Intake):** Standard large port at [X −4, Z +7, L1] for manual clearance of oversize quarry boulders.
* **Ports 3 & 4 (Refinery Manifolds):** Manual inventory access ports exposed on the northern face of `GND REF 1` and `GND REF 3` at Level L2.
* **Ports 5 & 6 (Assembler Manifolds):** Manual component withdrawal ports exposed on the southern face of `GND ASM 2` and `GND ASM 4` at Level L2.
* **Port 7 (Buffer Warehouse):** Large maintenance hatch exposed on the western face of `GND STR Components` at [X +1, Z −10, L2].

---

### 0.6 Master Electrical Single-Line Schematic & 5-Mode Energy Management Suite

```
[MASTER ELECTRICAL SINGLE-LINE SCHEMATIC]
PRIMARY ENERGY STORAGE:
  [GND BAT 1..6] (6× Large Batteries, Vault Z −9, X −4) ────────────► 18.0 MWh Capacity / 72.0 MW Surge
EMERGENCY NUCLEAR GENERATION:
  [GND RCT Aux 1..2] (2× Small Nuclear Reactors, Z −11, X −4) ─────► 30.0 MW Backup Generation
C2 UNINTERRUPTIBLE POWER SUPPLY (UPS):
  [GND BAT C2 UPS] (1× Large Battery, Dedicated Server Vault L2) ──► 3.0 MWh Isolated Reserve
MAIN FACILITY POWER BUS:
  ├────────────────────────────────────────────────────────────────► 102.0 MW Peak Instantaneous
  │
  ├─► HEAVY REFINING ARRAY (4× Refineries + 16× Modules) ──────────► 13.2 MW Continuous
  ├─► MANUFACTURING FOUNDRY (4× Assemblers + 16× Modules) ────────► 2.2 MW Continuous
  ├─► SHIPYARD EXTRUSION LOAD (22× Welders + Pusher Pistons) ──────► 8.8 MW Peak Pulsed
  ├─► LOGISTICS SHORE CHARGERS (Mule Pad 12 MW + Cat Dock 6 MW) ───► 18.0 MW Fast Recharge
  └─► C2 COMMAND & COMMS (Server Racks, Video Wall, Laser Spire) ──► 0.6 MW Protected Load
```

### 5 Formal Operational Facility Power Modes
1. **Mode 1 · Silent Watchkeeping (Standby / Night Operations):**
   * Output: `1.2 MW`. Refineries and assemblers idle; shore chargers off; batteries set to `Auto`; comms and C2 bridge active.
2. **Mode 2 · Daytime Ingot Smelting (Ore Processing Focus):**
   * Output: `15.5 MW`. All 4 refineries operating at 100% capacity (200% yield); assemblers idle; battery storage provides baseline support.
3. **Mode 3 · Full Industrial Surge Extrusion (Rig Section Printing):**
   * Output: `32.5 MW` (30 MW Nuclear + 2.5 MW Battery assist). All 4 refineries, 4 assemblers, and Chapter 5 Extruder Ring welders firing simultaneously.
4. **Mode 4 · Fast Terminal Shore Recharge (Mule & Cat Docked):**
   * Output: `48.0 MW` (Battery surge discharge). High-current DC power pumped into docked MULE-01 (12 MW) and CATERPILLAR-01 (6 MW) to achieve turnaround in `< 60 s`.
5. **Mode 5 · Blackout Emergency UPS Mode (Grid Failure Failover):**
   * Output: `0.6 MW`. Industrial grid completely shed via `GND EC PowerGuard`; `GND BAT C2 UPS` isolates to maintain unbroken laser antenna connection to the Floating Rig and regional fleet telemetry.

---

### 0.7 Master Facility Bill of Materials (BOM) & Mass Budget

| Subassembly Zone | Major Block Types & Specifications | Block Count | Dry Mass (t) | PCU | Power Rating |
|---|---|---|---|---|---|
| **Foundation & Tarmac** | Heavy armor blocks, blast slabs, retaining walls, sub-surface raceways | 548 | 468.0 | 548 | Passive |
| **Mule Gantry Pad** | 7×7 B deck, 1× Connector, 2× Motor Hinges, 2× Mag Plates, 1× Sensor | 56 | 23.2 | 285 | 12.0 MW Shore Bus |
| **Caterpillar Unload Dock**| 1× Connector, 5× Collectors, 2× Blast Curbs, Sorter Manifold | 34 | 35.5 | 335 | 6.0 MW Shore Bus |
| **Heavy Smelting Array** | 4× Large Refineries, 16× Yield Modules, Ingot Conduit Headers | 24 | 191.2 | 672 | 13.2 MW Draw |
| **Manufacturing Foundry** | 4× Large Assemblers, 16× Speed Modules, Cooperative Manifolds | 24 | 82.0 | 616 | 2.2 MW Draw |
| **Storage & Logistics** | 1× Large Cargo (3×3×3 B), 4× Sorters, Extruder Feed Conveyor Trunk | 52 | 41.5 | 280 | 1.0 MW Draw |
| **Power Substation** | 6× Large Batteries, 2× Small Reactors, Substation Switchgear | 14 | 53.2 | 390 | 30 MW Gen / 72 MW Surge |
| **C2 Command Bridge** | 3× Control Desks, 6× Large LCDs, 3× PBs, Glass, Catwalks, Decon Door | 78 | 48.0 | 740 | 0.4 MW Draw |
| **Comms & Laser Spire** | 1× 50 km Omni Antenna, 1× Laser Antenna, 2× Cameras, Mast Armor | 12 | 14.5 | 425 | 0.2 MW Draw |
| **FACILITY TOTALS** | **Complete Surface Logistics Terminal & Regional C2 Hub** | **842** | **≈ 957.1 t** | **4,291** | **30 MW Gen / 102 MW Peak** |

* All structures, foundation slabs, and mechanical anchorages calculated at Earth 1 g baseline.
* Values tagged with `†` awaiting in-game confirmation in RIG Test:
  * Large Refinery continuous draw at full load = `3.3 MW` per unit (13.2 MW total)†.
  * Large Assembler continuous draw at full load = `560 kW` per unit (2.24 MW total)†.
  * Laser Antenna maximum power draw during atmospheric beam link = `2.5 MW`†.

---

### 0.8 Master Player Circulation, Pressurized Corridors & Decontamination Airlocks

To ensure complete physical ergonomics and survivability for engineering personnel:
1. **Perimeter Catwalks & Safety Handrails (Level L1):** High-viz yellow catwalks (Palette Slot 8) border the tarmac deck, separating pedestrian traffic from the active Caterpillar rail trench and Mule flight apron.
2. **Pressurized C2 Tower Access Stairwell (Levels L1-L2):** Enclosed armored stairwell climbs from the western tarmac sill into the Level L2 Server Room through an exterior sliding airlock.
3. **Active Gas-Reclamation Decontamination Airlock (Level L3):** Direct personnel entry into the panoramic command bridge passes through a dual-door sealed airlock chamber equipped with active depressurization air vents (`GND Vent Decon Bridge`), cyan UV washdown lighting, and automated cycle interlocks. Outbound cycles evacuate 100% of cabin O2 before outer doors open.
4. **Command Bridge Facilities (Levels L3-L4):** Walk-around observation deck, Tier 4 survival armory lockers (`GND Armory Bridge`), medical station (`GND Medical Bay`), and suit oxygen/hydrogen recharge stations adjacent to operator desks.

---

### 0.9 GROUND-02 (MKII) Heavy Armored Citadel & Defense Grid Roadmap

* **Operational Context:** In advanced survival scenarios or contested planetary sectors subject to Modular Encounter Spawner (MES) hostile raids:
  1. **Perimeter CIWS Gatling Turrets:** Mount 4× Large Grid Gatling Turrets (`GND Turret North/South/East/West`) along tarmac corners plumbed to the central ammunition buffer hold.
  2. **Automated Combat Lockdown:** `[GND] PB FleetOps` detects target lock/damage; sounds `GND Alarm Raid`; triggers `GND EC Combat` (seals all airlocks, retracts Mule cradle arms, arms CIWS turrets).
  3. **Planetary Shield Generator Bunker:** Reserve foundation space in South Zone (X −9, Z −11) to mount a Defense Shields generator protecting the base and Extruder shipyard.

---

## Phase 4-1 · Heavy Foundation Tarmac, Retaining Walls & Sub-Surface Trenches (Level L1)

```
       [PHASE 4-1 SPATIAL LOCATOR HUD]
       Facility: Ground Base Foundation (GND-01)
       Envelope: 24 W × 28 L × 2 H B (60.0 × 70.0 × 5.0 m)
       Coordinate Range: X −12 to +12, Z −14 to +14, Level L0 to L1
       Key Datums:
       · Foundation Surface Deck: Level L1 (Y = 0)
       · Sub-Surface Bedrock Trench Depth: Level L0 (Y = −2.5 m)
       · Trench A (Caterpillar Bulk Ore): X −4, Z +5 to +13
       · Trench B (Mule Rare Ore): X +3, Z +5 to +13
       · Main Industrial Manifold Raceway: Centerline X0, Z −13 to +5
```

### Pre-Step Master Parts Tray: Phase 4-1
* 450× Heavy Armor Blocks (Palette Slot 0: `#2B303A`, 382.5 t, 450 PCU)
* 64× Heavy Armor Half-Blocks (Palette Slot 0, 27.2 t, 64 PCU)
* 34× Heavy Armor Slopes (Palette Slot 0, 17.0 t, 34 PCU)
* 24× Large Conveyor Junctions (Palette Slot 4: `#1ABC9C`, 19.2 t, 144 PCU)
* 16× Large Conveyor Tubes (Palette Slot 4, 9.6 t, 96 PCU)

---

### Step 4.1 · Bedrock Plateau Excavation & Perimeter Retaining Walls (L0-L1)
Excavate a level 60 × 70 m plateau into bedrock:
1. Lay the perimeter retaining wall using Heavy Armor Blocks along X = ±12 (Z −14 to +14) and Z = ±14 (X −12 to +12) at Level L1.
2. Anchor corner footings 2 blocks deep into terrain (Level L0) to prevent voxel clipping or structural floating.
3. Bevel the exterior tarmac perimeter with Heavy Armor Slopes angled 45° to provide smooth ramp transitions for ground vehicles.

---

### Step 4.2 · Sub-Surface Conveyor Trenches A & B (L0)
Recess two longitudinal conveyor raceways beneath the tarmac deck to keep all raw ore piping hidden and blast-protected:
1. Excavate Trench A (3 B wide × 1 B deep) along X −4 spanning Z +5 to +13 at Level L0. Plumb with 8× Large Conveyor Junctions to feed the Caterpillar receiving dock.
2. Excavate Trench B (3 B wide × 1 B deep) along X +3 spanning Z +5 to +13 at Level L0. Plumb with 6× Large Conveyor Junctions to feed the Mule gantry pad.
3. Connect both trenches across transverse headers at Z = +5 to the central industrial manifold raceway.

---

### Step 4.3 · Foundation Blast Slabs & Tarmac Deck Plating (L1)
1. Lay reinforced Heavy Armor Blocks across the entire 24 × 28 B surface deck at Level L1, enclosing the sub-surface raceways.
2. Leave open intake collar sockets at [X +3, Z +9] (for the Mule pad connector) and along X −4, Z +8 to +10 (for the 5× Caterpillar collectors).
3. Paint the entire tarmac foundation Palette Slot 0 (Industrial Slate).

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.1A: SUB-SURFACE CONVEYOR INTEGRITY**
> Before sealing the heavy tarmac plates over Trenches A and B:
> 1. Verify 100% large-conveyor continuity from the northern logistics nodes to the central industrial headers.
> 2. Ensure zero small-conveyor tubes are installed anywhere in the main line (ore transfer requires large-tube flow).
> 3. Perform a pressure-tight seal check on all recessed conveyor cubes.

---

#### Visual Milestone 4-1 · Tarmac Foundation Complete

```
Visual Checkpoint Silhouette (Looking South across Level Tarmac Deck L1):
        X: −11 (C2 Pylon)             Centerline X0             X: +11 (East Apron)
           ┌──────────┐                                                ┌──────────┐
           │ PYLON L1 │                                                │ EAST SILL│
      ═════╧══════════╧════════════════════════════════════════════════╧══════════╧═════
                   24 × 28 B LEVEL INDUSTRIAL BLAST TARMAC (L1)
```

* [ ] Foundation is 100% sealed with zero exposed voxel terrain gaps.
* [ ] Trench raceways provide unrestricted large-port conveyor flow beneath the surface.

---

## Phase 4-2 · Dual Logistics Terminals (Mule Pad & Caterpillar Dock) (Levels L1-L2)

```
       [PHASE 4-2 SPATIAL LOCATOR HUD]
       Zone: North Logistics Apron
       Envelope: 20 W × 8 L × 2 H B (50.0 × 20.0 × 5.0 m)
       Coordinate Range: X −6 to +6, Z +6 to +13, Level L1 to L2
       Key Interfaces:
       · MULE-01 Gantry Cradle Deck: Centered [X +3, Z +9, Level L1]
       · CATERPILLAR-01 Unload Dock: Centered [X −4, Z +9, Level L1]
```

### Pre-Step Master Parts Tray: Phase 4-2
* 49× Heavy Armor Blocks (Palette Slot 1: `#E6A100`, 41.6 t, 49 PCU)
* 2× Large Connectors (Palette Slot 1 & 2, 6.2 t, 300 PCU)
* 2× Motor Hinges (Large) (Palette Slot 1, 8.0 t, 200 PCU)
* 2× Magnetic Plates (Large) (Palette Slot 1, 4.8 t, 50 PCU)
* 5× Collectors (Large Grid) (Palette Slot 2: `#9E2A2B`, 18.5 t, 125 PCU)
* 2× Conveyor Sorters (Palette Slot 3, 2.4 t, 80 PCU)
* 1× Sensor Block (Palette Slot 1, 0.3 t, 40 PCU)
* 18× Heavy Armor Slopes 2×1×1 (Palette Slot 2, 13.5 t, 18 PCU)

---

### Step 4.4 · MULE-01 Touchdown Blast Deck (7×7 B Flush Deck) (L1)
1. Construct the 7 × 7 B touchdown deck spanning X 0 to +6, Z +6 to +12 at Level L1.
2. Paint the deck perimeter with alternating Hazard Amber (Slot 1) and Industrial Slate (Slot 0) warning chevrons.
3. Install Large Connector `GND PAD Conn` flush in the deck center at [X +3, Z +9, Level L1], facing straight UP (+Y), plumbed directly into Trench B.

#### Terminal Configuration:
* Connector: `GND PAD Conn` (Auto-Connect: `OFF` · Collect All: `OFF` · Connect Strength: `0.015`).

---

### Step 4.5 · Motorized Gantry Cradle Clamping Arms & Mag Plates (L1-L2)
Assemble the active landing cradle mechanism per **Decision 45**:
1. Mount 1× Motor Hinge `GND PAD Cradle Hinge Port` at [X +1, Z +10, Level L1] with hinge pin longitudinal (Z-axis) and base facing East (+X).
2. Mount 1× Motor Hinge `GND PAD Cradle Hinge Stbd` at [X +5, Z +10, Level L1] with hinge pin longitudinal (Z-axis) and base facing West (−X).
3. Weld Large Magnetic Plates `GND PAD Cradle Clamp Port/Stbd` onto the hinge heads, set to `Auto-Lock = ON`.
4. Install Sensor Block `GND PAD Sensor` at [X +3, Z +12, Level L1]. Configure detection field: 15 m radius centered 5 m above pad.

#### Joint Tuning Sliders:
* **Share Inertia Tensor = `ON`** on both cradle hinges.
* **Hinge Limits:** Lower Limit = `0.0°` (arms flush with deck); Upper Limit = `30.0°` (arms swung inward clamping Mule flank rub rails).
* **Braking Torque:** `20.0 MN·m`†.

---

### Step 4.6 · CATERPILLAR-01 Unload Dock, Wheel Curbs & Stop Bumpers (L1-L2)
1. Construct the heavy rail receiving trench along X −6 to −1, Z +6 to +12 at Level L1.
2. Install raised Heavy Armor Slope curbs along X = −6 and X = −1 to physically funnel the land train's 5×5 wheels into alignment.
3. Weld heavy collision stop blocks using Blast Door Blocks at Z = +5 (X −3 to −5).
4. Mount Large Connector `GND CAT Dump Conn` horizontally at [X −4, Z +5, Level L2] facing North (+Z). Height: exactly 2.5 m above tarmac.

---

### Step 4.7 · 5× In-Deck Gravity Collectors & Bulk Sorter Manifold (L1)
1. Mount 5× Large Grid Collectors `GND CAT Hopper Coll 1..5` flush in the deck at Level L1, facing straight UP (+Y):
   * `COLL 1, 2`: [X −5, Z +10] and [X −3, Z +10]
   * `COLL 3, 4`: [X −5, Z +9] and [X −3, Z +9]
   * `COLL 5`: [X −4, Z +8]
2. Plumb all 5 collectors into sub-surface Trench A.
3. Install Large Conveyor Sorter `GND CAT Sorter Bulk` downstream of the collectors at [X −4, Z +5, Level L0] with flow arrow South (−Z). Whitelist: `Iron`, `Nickel`, `Silicon`, `Cobalt`, `Stone`. Set `Drain All = ON`.

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 4.2A: LOGISTICS DOCK VERIFICATION**
> 1. Test Mule cradle sensor: walk into detection field; confirm cradle arms swing inward 30° smoothly without binding.
> 2. Verify `GND CAT Dump Conn` centerline height aligns within ±0.05 m of `CAT Train Unload Conn` (2.5 m).
> 3. Verify both sorters pull continuously from their respective receiving ports.

---

## Phase 4-3 · Heavy Smelting Array (4 Refineries + 16 Yield Modules) (Levels L1-L2)

```
       [PHASE 4-3 SPATIAL LOCATOR HUD]
       Zone: Central Industrial Smelting
       Envelope: 16 W × 6 L × 3 H B (40.0 × 15.0 × 7.5 m)
       Coordinate Range: X −6 to +6, Z +1 to +5, Level L1 to L3
       Refinery Centroids:
       · West Bank (Rare Ore): `GND REF 1` [X −5, Z +3], `GND REF 2` [X −5, Z +1]
       · East Bank (Bulk Ore): `GND REF 3` [X +4, Z +3], `GND REF 4` [X +4, Z +1]
```

### Pre-Step Master Parts Tray: Phase 4-3
* 4× Large Refineries (Palette Slot 3: `#1ABC9C`, 100.8 t, 360 PCU)
* 16× Yield Modules (Palette Slot 3, 83.2 t, 240 PCU)
* 16× Large Conveyor Tubes (Palette Slot 4: `#1ABC9C`, 9.6 t, 96 PCU)
* 8× Large Conveyor Junctions (Palette Slot 4, 6.4 t, 48 PCU)

---

### Step 4.8 · West Smelting Bank (Refineries 1 & 2 + 8 Yield Modules) (L1-L2)
Install the dedicated rare-ore processing bank:
1. Mount 2× Large Refineries `GND REF 1` and `GND REF 2` at [X −5, Z +3] and [X −5, Z +1] at Level L1, with large ports facing North/South.
2. Attach 4× Yield Modules to the exterior Port face of `GND REF 1` and 4× Yield Modules to `GND REF 2`.
3. Plumb the intake ports directly to Trench B (Rare Ore Stream from Mule Pad).

---

### Step 4.9 · East Smelting Bank (Refineries 3 & 4 + 8 Yield Modules) (L1-L2)
Install the dedicated bulk-ore processing bank:
1. Mount 2× Large Refineries `GND REF 3` and `GND REF 4` at [X +4, Z +3] and [X +4, Z +1] at Level L1.
2. Attach 4× Yield Modules to the exterior Starboard face of `GND REF 3` and 4× Yield Modules to `GND REF 4`.
3. Plumb the intake ports directly to Trench A (Bulk Ore Stream from Caterpillar Dock).
4. Tie all 4 refinery discharge ports into the central Refined Ingot Trunk running South along centerline X0.

#### Terminal Verification:
* Inspect all 4 refineries in terminal: confirm `Productivity: 100%` and `Effectiveness: 200%` (maximum vanilla yield).

---

## Phase 4-4 · Cooperative Fabrication Core (4 Assemblers + 16 Speed Modules) (Levels L1-L2)

```
       [PHASE 4-4 SPATIAL LOCATOR HUD]
       Zone: Central Industrial Fabrication
       Envelope: 16 W × 6 L × 2 H B (40.0 × 15.0 × 5.0 m)
       Coordinate Range: X −6 to +6, Z −5 to −1, Level L1 to L2
       Assembler Centroids:
       · West Bank: `GND ASM 1` [X −4, Z −2], `GND ASM 2` [X −4, Z −4]
       · East Bank: `GND ASM 3` [X +3, Z −2], `GND ASM 4` [X +3, Z −4]
```

### Pre-Step Master Parts Tray: Phase 4-4
* 4× Large Assemblers (Palette Slot 4: `#2980B9`, 38.8 t, 160 PCU)
* 16× Speed Modules (Palette Slot 4, 38.4 t, 240 PCU)
* 1× Large Cargo Container (3×3×3 B) (Palette Slot 5: `#27AE60`, 28.5 t, 250 PCU)
* 1× Large Conveyor Sorter (Palette Slot 5, 1.2 t, 40 PCU)
* 12× Large Conveyor Junctions (Palette Slot 5, 9.6 t, 72 PCU)

---

### Step 4.10 · 4-Assembler Manufacturing Array & 16× Speed Modules (L1-L2)
1. Mount 4× Large Assemblers `GND ASM 1..4` at [X −4, Z −2], [X −4, Z −4], [X +3, Z −2], and [X +3, Z −4] at Level L1.
2. Plumb top and side upgrade ports with 4× Speed Modules per assembler (16 total).
3. Configure `GND ASM 1` as **Master Controller**; set `Cooperative Mode = ON` on `GND ASM 2, 3, 4`.
4. Verify terminal displays `Productivity: 400%` across all 4 units.

---

### Step 4.11 · Component Buffer Warehouse & Extruder Push Sorter (L1-L3)
1. Mount 1× Large Cargo Container `GND STR Components` (3×3×3 B) centered at [X +3, Z −10, Level L1-L3].
2. Plumb the container's northern ports into the Assembler component outflow trunk.
3. Mount 1× Large Conveyor Sorter `GND Sorter Extruder Feed` at [X +3, Z −13, Level L1] with flow arrow South (−Z), feeding the Chapter 5 Extruder Ring shipyard. Set `Drain All = ON`.

---

## Phase 4-5 · Planetary Power Substation & Battery Vault (Levels L1-L2)

```
       [PHASE 4-5 SPATIAL LOCATOR HUD]
       Zone: South Power Substation
       Envelope: 8 W × 8 L × 3 H B (20.0 × 20.0 × 7.5 m)
       Coordinate Range: X −8 to 0, Z −13 to −7, Level L1 to L3
       Key Assets:
       · Battery Vault: `GND BAT 1..6` [X −4, Z −9, Level L1-L2]
       · Nuclear Backup: `GND RCT Aux 1..2` [X −4, Z −12, Level L2]
```

### Pre-Step Master Parts Tray: Phase 4-5
* 6× Large Batteries (Palette Slot 6: `#C0392B`, 30.0 t, 120 PCU)
* 2× Small Nuclear Reactors (Palette Slot 6, 3.2 t, 50 PCU)
* 24× Heavy Armor Blocks (Palette Slot 6, 20.4 t, 24 PCU)

---

### Step 4.12 · 18 MWh Large Battery Bank & Nuclear Backup Vault (L1-L2)
1. Install 6× Large Batteries `GND BAT 1..6` arranged in a 3 × 2 grid centered at [X −4, Z −9, Level L1-L2]. Total capacity: 18.0 MWh; surge output: 72.0 MW. Set to `Auto`.
2. Install 2× Small Nuclear Reactors `GND RCT Aux 1..2` at [X −4, Z −12, Level L2]. Total capacity: 30.0 MW backup. Plumb with small conveyor tubing for automated uranium feed.
3. Enclose the power vault with Heavy Armor Blocks painted Palette Slot 6 (High-Voltage Red).

---

## Phase 4-6 · Elevated C2 Operations Tower, Panoramic Bridge & Comms Spire (Levels L1-L9)

```
       [PHASE 4-6 SPATIAL LOCATOR HUD]
       Zone: West Elevated C2 Command Tower
       Envelope: 5 W × 5 L × 9 H B (12.5 × 12.5 × 22.5 m)
       Coordinate Range: X −11 to −7, Z +1 to +5, Level L1 to L9
       Decks:
       · L1: Heavy Structural Blast Pylon Anchor
       · L2: Fleet Operations Server Room & UPS Vault
       · L3-L4: Panoramic 3-Operator Command Bridge & Video Wall
       · L5-L9: Long-Range Regional Comms Spire & Rig Laser Link
```

### Pre-Step Master Parts Tray: Phase 4-6
* 3× Programmable Blocks (Palette Slot 7: `#1B263B`, 3.6 t, 75 PCU)
* 3× Control Stations (Palette Slot 7, 1.8 t, 45 PCU)
* 6× Large LCD Panels (Palette Slot 7, 2.4 t, 150 PCU)
* 1× Large Battery (Dedicated UPS) (Palette Slot 7, 5.0 t, 20 PCU)
* 1× Large Grid Radio Antenna (Palette Slot 7, 8.0 t, 100 PCU)
* 1× Large Grid Laser Antenna (Palette Slot 9: `#ECF0F1`, 2.5 t, 200 PCU)
* 2× Large Grid Cameras (Palette Slot 7, 0.2 t, 40 PCU)
* 24× Sloped Armor Glass (Palette Slot 7, 4.8 t, 24 PCU)
* 1× Sliding Airtight Door (Palette Slot 7, 0.8 t, 25 PCU)

---

### Step 4.13 · Level L2 Server Room, PB Rack & Dedicated UPS (L2)
1. Build the enclosed 5 × 5 B server room deck at Level L2 ([X −9, Z +3]).
2. Mount 3× Programmable Blocks along the northern bulkhead:
   * `[GND] PB FleetOps` : Regional fleet coordination, JIT dispatch, and C2 handover root.
   * `[GND] PB Inventory` : Industrial quotas, sorting, and cooperative assembler management.
   * `[GND] PB Displays` : Zero-GC text rendering engine for the 6-panel video wall.
3. Mount 1× Large Battery `GND BAT C2 UPS` in the southern floor cavity.
4. Install Event Controller `GND EC PowerGuard` and Timer Block `GND Timer Watchdog`.

---

### Step 4.14 · Level L3-L4 Panoramic Command Bridge & 3-Operator Desks (L3-L4)
1. Construct the elevated command deck at Level L3 cantilevered outward over the western tarmac.
2. Mount 3× Control Stations side-by-side facing East (+X) overlooking the facility:
   * `GND Desk AirOps` (Station 1 · Port/North): Orca carrier telemetry, Remora plunge status, Mule traffic.
   * `GND Desk GroundOps` (Station 2 · Center): Mastodon 35 m boom CTC, Caterpillar dump queue, pull quotas.
   * `GND Desk YardOps` (Station 3 · Starboard/South): Extruder pusher feed, welder wall, inventory reserves.
3. Enclose the bridge with 2-layer Sloped Armor Glass angled 45° outward, providing a 270° panoramic view over the North Apron, Industrial Foundry, and South Yard.
4. Mount 1× Sliding Airtight Door `GND Door Decon Bridge` and Air Vent `GND Vent Decon Bridge` to form a pressurized decontamination airlock.

---

### Step 4.15 · Level L4 6-Panel Panoramic LCD Video Wall (L4)
Mount 6× Large LCD Panels `GND LCD Video Wall 1..6` in a continuous 3 × 2 video matrix along the forward bridge bulkhead above the viewports:
* `LCD 1` : Aerial Fleet Roster (Orca status, Remora shafts, Mule flight ETA).
* `LCD 2` : Ground Fleet Roster (Mastodon macro status, Caterpillar consist speed, buffer mass).
* `LCD 3` : Shipyard Extruder Status (Pusher stroke, welder active count, layer print %).
* `LCD 4` : Smelting Core Throughput (Refinery effectiveness, ingot flow rates, gravel buffer).
* `LCD 5` : Stockpiles & Quotas (Steel plates, motors, computers, uranium reserves).
* `LCD 6` : Tactical & Defense (Grid status, antenna link integrity, radar tracks, weather).

---

### Step 4.16 · Level L5-L9 Comms Spire & Floating Rig Laser Link (L5-L9)
1. Erect the 10 m communications mast from the bridge roof at [X −9, Z +1] spanning Levels L5 to L9.
2. Mount 1× Large Grid Radio Antenna `GND Antenna Fleet` at Level L6. Set broadcast range to `50,000 m`.
3. Mount 2× Fixed Cameras: `GND Cam North Pads` (facing +Z) and `GND Cam South Yard` (facing −Z).
4. Crown the mast tip at Level L9 with 1× Laser Antenna `GND Laser Rig Relay` painted Palette Slot 9 (White). Acquire and lock GPS coordinates of the Floating Rig Fortress in the upper atmosphere.

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.6A: C2 BRIDGE COMMISSIONING GATE**
> 1. Verify `GND Door Decon Bridge` seals airtight; bridge cabin pressurizes to 100% O2.
> 2. Verify all 6 LCD panels report active display surfaces under `[GND] PB Displays`.
> 3. Verify `GND Laser Rig Relay` acquires and locks high-altitude target without line-of-sight obstruction.

---

## Phase 4-7 · Master Control Logic, Multi-PB Architecture & Software Abstraction

### 1. Multi-PB Server Architecture
* `[GND] PB FleetOps` (Update10):
  * Manages regional fleet dispatch, JIT transport pre-dispatch (`TMS.LOGISTICS.JIT`), pull economy quotas (`TMS.BASE.DEMAND`), and seamless C2 authority handover from Mastodon (`TMS.C2.HANDOVER`).
* `[GND] PB Inventory` (Update100):
  * Manages refinery intake manifolds, cooperative assembler production queues, and southern Extruder feed quotas.
* `[GND] PB Displays` (Update100):
  * Zero-GC monospace ASCII text renderer formatting telemetry for `GND LCD Video Wall 1..6`.

### 2. Group Taxonomy
* `GND Refineries` : `GND REF 1..4` (All 4 Large Refineries).
* `GND Assemblers` : `GND ASM 1..4` (All 4 Large Assemblers).
* `GND Yield Modules` : All 16 Yield upgrade modules.
* `GND Speed Modules` : All 16 Speed upgrade modules.
* `GND Fleet Displays` : `GND LCD Video Wall 1..6`.
* `GND Power Batteries` : `GND BAT 1..6`.
* `GND PAD Cradle` : `GND PAD Cradle Hinge Port/Stbd`, `GND PAD Cradle Clamp Port/Stbd`.
* `GND CAT Hopper` : `GND CAT Hopper Coll 1..5`, `GND CAT Dump Conn`.

### 3. Block Naming & Terminal Dictionary
* `GND PAD Conn` : Large Connector flush on Mule pad [X +3, Z +9, L1].
* `GND PAD Cradle Hinge Port/Stbd` : Motor Hinges driving Mule gantry clamps [X +1/+5, Z +10, L1].
* `GND PAD Cradle Clamp Port/Stbd` : Magnetic Plates on cradle heads.
* `GND PAD Sensor` : 15 m proximity sensor on Mule pad [X +3, Z +12, L1].
* `GND CAT Dump Conn` : Large Connector at Caterpillar dock [X −4, Z +5, L2].
* `GND CAT Hopper Coll 1..5` : 5× In-Deck Collectors at Caterpillar dock [L1].
* `GND Sorter Rare Intake` : Sorter on Trench B [X +3, Z +5, L0].
* `GND Sorter Bulk Intake` : Sorter on Trench A [X −4, Z +5, L0].
* `GND REF 1..4` : Large Refineries in smelting array [L1-L2].
* `GND ASM 1..4` : Large Assemblers in manufacturing foundry [L1-L2].
* `GND STR Components` : Large Cargo Container buffer warehouse [X +3, Z −10, L1-L3].
* `GND Sorter Extruder Feed` : Sorter on extruder feed line [X +3, Z −13, L1].
* `GND BAT 1..6` : 6× Large Batteries in main substation [X −4, Z −9, L1-L2].
* `GND BAT C2 UPS` : Isolated 1× Large Battery in C2 server room [L2].
* `GND RCT Aux 1..2` : 2× Small Nuclear Reactors in power vault [X −4, Z −12, L2].
* `GND Desk AirOps` : Control Station 1 in C2 bridge [L3].
* `GND Desk GroundOps` : Control Station 2 in C2 bridge [L3].
* `GND Desk YardOps` : Control Station 3 in C2 bridge [L3].
* `GND LCD Video Wall 1..6` : 6× Large LCD Panels in C2 bridge [L4].
* `GND Antenna Fleet` : 50 km Omni Radio Antenna on mast [L6].
* `GND Laser Rig Relay` : Laser Antenna on mast crown [L9].

---

### 4. Event Controller Matrix (Rule C4 Compliance)

| Event Controller | Monitored Block / Variable | Condition & Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| **`GND EC PadClamp`** | `GND PAD Sensor` | Detects MULE $\le 10\text{ m}$ | `GND PAD Cradle` Swing In 30° | `GND PAD Cradle` Retract Flush 0° |
| **`GND EC PadLock`** | `GND PAD Conn` | Connector: `Connected` | (1) `GND Sorter Rare Intake` Drain ON<br/>(2) `GND BAT 1..6` Shore Power Mode | (1) `GND Sorter Rare Intake` Drain OFF<br/>(2) `GND BAT 1..6` Auto Mode |
| **`GND EC CatDump`** | `GND CAT Dump Conn` | Connector: `Connected` | `GND Sorter Bulk Intake` Drain ON | `GND Sorter Bulk Intake` Drain OFF |
| **`GND EC PowerGuard`**| `GND Power Batteries` | Grid Stored Power $< 15\%$ | (1) `GND RCT Aux 1..2` Turn ON<br/>(2) Non-essential lights OFF | (1) `GND RCT Aux 1..2` Turn OFF<br/>(2) Normal grid restored |
| **`GND EC C2 UPS`** | Main Substation Power | Main Bus Failure | `GND BAT C2 UPS` Disconnect from Main Bus (Isolate C2) | `GND BAT C2 UPS` Reconnect to Main Bus |

---

### 5. IGC Channel Subscriptions (Ground & Fleet Bus)

* **Inbound Channels:**
  * `TMS.C2.HANDOVER`: `HANDOVER;MAS-01;GND-01;REGIONAL_ROOT;CONFIRM`
  * `TMS.AIR.PAD`: `mule_id;REQ_LAND;tonnage;eta_sec`
  * `TMS.GROUND.DOCK`: `train_id;REQ_DUMP;tonnage;eta_sec`
  * `TMS.FLEET.STATUS`: `craft_id;type;gps;hold_pct;bat_pct;status`
  * `TMS.LOGISTICS.JIT`: `JIT;MAS-01;tier;eta_sec;ore_type;mass_kg`
* **Outbound Channels:**
  * `TMS.BASE.RECEIVING`: `GND-01;CLEAR_TO_DUMP;TERMINAL_DOCK_1;READY`
  * `TMS.BASE.DEMAND`: `DEMAND;ore_type;priority;deficit_kg;target_stock_kg`
  * `TMS.MULE.CALL`: `CALL;requesting_grid;target_pad;priority;payload_type`
  * `TMS.RIG.LASER`: `req_ingots;stock_summary_json;base_status`

---

## Phase 4-8 · Commissioning & Field Acceptance Checklist (RIG Test)

### Suite A · Foundation & Tarmac Seal Verification
* [ ] **Tarmac Vacuum Seal:** Verify zero voxel penetration or air leakage through the 24 × 28 B heavy armor blast floor; verify sub-surface trenches remain pressurized where sealed.

### Suite B · Mule Gantry Cradle Kinematics & High-Speed Drain
* [ ] **Cradle Proximity Trigger:** Approach pad in MULE-01; confirm `GND PAD Sensor` detects craft at 10 m and swings cradle arms inward 30°; land on pad; confirm magnetic plates lock rub rails and connector locks automatically.
* [ ] **High-Speed Rare Ore Drain:** Feed 500 t Uranium Ore from Mule hold; confirm rare ore sorter drains cargo at $> 80\text{ t/min}$ into West Smelting Bank.

### Suite C · Caterpillar Terminal Dump & Collector Gravity Flow
* [ ] **Reverse Alignment Check:** Reverse CATERPILLAR-01 consist into dock; confirm guide curbs center the wheels and stop bumpers absorb kinetic impact.
* [ ] **2,500 t Bulk Ore Purge:** Lock `GND CAT Dump Conn`; confirm 5× in-deck collectors and bulk sorter evacuate all 3 hopper cars in $< 90\text{ seconds}$ without conveyor back-pressure.

### Suite D · Dual-Tier Refining Smelting Stress Test (200% Yield)
* [ ] **Simultaneous Smelting Run:** Feed 2,000 t Iron Ore and 500 t Gold Ore concurrently; verify dual sorters isolate bulk and rare streams; verify all 4 refineries operate at 200% yield effectiveness without tripping power.

### Suite E · Cooperative 4-Assembler Manufacturing Burst
* [ ] **Pylon Production Burst:** Queue 20,000 Steel Plates and 4,000 Motors; confirm `GND ASM 1` distributes load across Assemblers 2-4 cooperatively; verify 400% productivity output feeds continuous stock to `GND STR Components`.

### Suite F · Power Substation Surge & Shore Power Fast Recharge
* [ ] **Simultaneous Docking & Extrusion:** Engage Extruder welders (8.8 MW), 4 refineries (13.2 MW), 4 assemblers (2.2 MW), and shore charging for both Mule and Cat (18 MW); confirm battery bank delivers 42.2 MW surge load without brownout.

### Suite G · C2 Master Server & Dynamic Handover from Mastodon MKI
* [ ] **Genesis Handover Trial:** Broadcast `TMS.C2.HANDOVER` from Mastodon; confirm `[GND] PB FleetOps` assumes regional C2 authority, initializes pull quotas (`TMS.BASE.DEMAND`), and synchronizes fleet telemetry.

### Suite H · 6-Panel LCD Video Wall Telemetry Stream
* [ ] **Multi-Screen Display Verification:** Deploy Orca, Remora, Mule, Mastodon, and Caterpillar; confirm Video Wall panels 1-6 display live, zero-flicker monospace status layouts with correct subsystem metrics.

### Suite I · Laser Uplink Unbroken Connection to Floating Rig
* [ ] **High-Altitude Beam Lock:** Establish laser connection with Floating Rig Fortress hovering at 15 km altitude; verify uninterrupted bidirectional telemetry during planetary atmospheric turbulence.

### Suite J · Player Circulation, Decontamination Airlocks & Emergency Survivability
* [ ] **Decon Cycle Verification:** Enter C2 bridge from Level L2; confirm `GND Door Decon Bridge` executes 3.0 s UV decontamination washdown before inner door unseals; verify bridge cabin maintains 100% O2 life support.

---

## Chapter 5 · Rig Linear Extruder Ring Shipyard

Heavy automated surface shipyard engineered for continuous horizontal layer-by-layer printing of the Floating Rig's major flown flight sections:
1. **Section S0 HUB Base (H4-H6):** Flown first on internal H6 lift thrusters.
2. **Sections S1-S6 Pylon Arms (8 W × 7 H × 22 L B):** Printed sequentially, cold-launched from the skid runway, and flown autonomously to dock at the Rig's H5 socket ring.

* **Grid:** Large Grid (1 B = 2.5 m).
* **Footprint:** 12 W × 78 L B overall (30.0 m width × 195.0 m longitudinal axis).
  * Launch Way Runway: 12 W × 50 L B (30.0 × 125.0 m, Z 0 to +50).
  * Stationary Welder Arch: 12 W × 3 L × 9 H B (30.0 × 7.5 × 22.5 m, Z −1 to +1, Y L1 to L9).
  * Hydraulic Pusher Runway: 5 W × 28 L × 3 H B (12.5 × 70.0 × 7.5 m, Z −28 to 0, Y L1 to L3).
* **Printing Envelope:** 8 W × 7 H B clear internal throat (20.0 m wide × 17.5 m high) with continuous longitudinal extrusion up to 24 B (60.0 m stroke).
* **Operating Principle (Decision 47):** Stationary U-shaped welder ring with 22× inward-facing ship welders. A 6-stage inline hydraulic piston carriage with a blueprint projector and merge head pushes the extruded hull horizontally outward over low-friction blast door skid rails. Upon completion, an umbilical refuels the section, the seed kit avionics activate, the merge release decouples, and the section ascends autonomously to station.
* **Operating Environment:** Earth 1 g baseline, surface level.

---

### 5.0 · Reference Grid, In-Game Paint Palette & Yard Zoning

#### 1. Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Intersection of the yard centerline (X = 0), tarmac foundation surface (Y = L1, Y = 0), and the welder ring forward baseline (Z = 0).
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0.
* **Y (Elevation):** Tarmac foundation deck is Level L1 (Y = 0), advancing upward (+).
* **Z (Length / Launch Axis):** Hydraulic Pusher Runway (Aft / Tail) is negative (−), Launch Direction (Forward / Skid Runway) is positive (+).

```
Spatial Alignment Triad:
       [+Y: Zenith / Up]
              │
              │   [+Z: Forward / Launch Way / Runway]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Aft / Hydraulic Pusher Spine]
              │
       [−Y: Bedrock Anchor Pilings]
```

#### 2. In-Game Paint Palette & Color Coding Scheme

To eliminate builder disorientation across a 195 m long industrial installation, all structural armor and functional blocks MUST be painted during assembly according to the standard Space Engineers 1 palette:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Heavy armor foundation tarmac, launch bed foundation, bedrock pilings |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Welder arch perimeter hazard striping, pusher carriage warning chevrons |
| **Slot 2** | Hydraulic Crimson | `#9E2A2B` | Deep Crimson | 6× Hydraulic Pusher pistons, push carriage frame, emergency stop bumpers |
| **Slot 3** | Welder Ochre | `#D35400` | Ochre Orange | 22× Ship Welders, nozzle mounting brackets, weld-mesh safety shrouds |
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | Conveyor pipe distribution trees, high-pressure booster sorters |
| **Slot 5** | Low-Friction Chromium | `#BDC3C7` | Matte Chrome | Blast door skid rails, half-block lateral guide curbs, contact wear plates |
| **Slot 6** | High-Voltage Red | `#C0392B` | Alert Red | Yard power substation, high-draw breaker junctions, capacitor banks |
| **Slot 7** | Yard Console Navy | `#1B263B` | Deep Navy | Yard Operations terminal booth, local status telemetry displays |
| **Slot 8** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel catwalks, arch ladder rungs, gantry service platforms |
| **Slot 9** | Seed Kit Signal White | `#ECF0F1` | Bright White | Merge block interface head, projector alignment crosshair, optical targets |

#### 3. Master Shipyard Layout & Zoning Plan

```
Extruder Yard Elevation (Looking Starboard, X = 0 Plane):
             Stationary Welder Ring
                 ┌──[WELDERS]──┐
                 │  (Y L1-L9)  │
 [ANCHOR WALL]   │   (Z −1..+1)│═══════════════════════════════════════════════════════► Launch Axis (+Z)
   [Z −28]       │             │              [COMPLETED PYLON SECTION]
      ║          │  [PYLON     │              [8 W × 7 H × 22 L B Span]
      ╠══[PISTONS 1..6]══►     │              [Sliding on Blast Skid Rails L1]
      ║  [Z −28 to 0]   │PRINTING]│═══════════════════════════════════════════════════════
  ════╩═════════════════╧═════════╧═══════════════════════════════════════════════════════
      ◄── Pusher Runway ──►◄ Welder Arch ►◄────────────── Skid Launch Way ──────────────►
         (Z −28 to 0)        (Z −1..+1)                   (Z 0 to +50)
```

```
Plan view, Y = L1 (Tarmac Deck & Track Level):
        X: −5   −4   −3   −2   −1    0   +1   +2   +3   +4   +5
    Z: +50 ┌──────────────────────────────────────────────────┐  ◄── Runway Threshold (+125 m)
           │          [BLAST SKID]        [BLAST SKID]        │
    Z: +30 │          [ GUIDE A  ]        [ GUIDE B  ]        │
           │          [  PORT    ]        [   STBD   ]        │
    Z: +10 │          [  RAIL    ]        [   RAIL   ]        │  ◄── Skid Launch Way
           ├──────────┴──────────┴────────┴──────────┴────────┤
    Z:  0  │[WLD] [WLD]      CLEAR PRINT THROAT     [WLD] [WLD]│  ◄── Welder Arch (Z = 0)
           ├──────────┬──────────┬────────┬──────────┬────────┤
    Z: −10 │          │          │[PISTON]│          │        │
    Z: −20 │          │          │ SPINE  │          │        │  ◄── Hydraulic Pusher Runway
    Z: −28 │══════════╧══════════╧════════╧══════════╧════════│  ◄── Anchor Abutment Wall
```

---

### 5.1 · Master Kinematic, Feed Flow & Extrusion Cycle Topology

The shipyard couples high-pressure component delivery directly from the Ground Base manufacturing foundry with synchronized mechanical linear extension:

```
                      GROUND BASE MANUFACTURING FOUNDRY
                    [GND STR Components] (Large Container)
                                    │
                                    ▼ (Direct Heavy Conveyor Trunk · Z −10 to Z 0)
                         [EXT Sorter Feed] (Drain All ON)
                                    │
        ┌───────────────────────────┼───────────────────────────┐
        ▼                           ▼                           ▼
[PORT WELDER MANIFOLD]     [TOP WELDER MANIFOLD]     [STBD WELDER MANIFOLD]
7× Ship Welders            8× Ship Welders           7× Ship Welders
(Facing Starboard · +X)    (Facing Downward · −Y)    (Facing Port · −X)
        │                           │                           │
        └───────────────────────────┼───────────────────────────┘
                                    ▼
                22× OVERLAPPING ACTIVE WELD SPHERES (1.75 m RADIUS)
              Instantaneous Component Fusion onto Moving Blueprint
                                    ▲
                                    │ (Pushed Horizontally at 0.05 m/s)
                 [6-STAGE HYDRAULIC PISTON PUSHER RUNWAY]
                 6× Large Pistons in Series (Total Stroke: 60.0 m)
                 [EXT MRG Release] + [EXT PRJ Head] Carriage Tip
```

#### Kinematic & Power Parameters:
* **Linear Extension Speed:** Exactly `0.05 m/s` (3.0 m/min).
  * Layer cycle: 2.5 m block length / 0.05 m/s = 50.0 seconds per structural block layer.
  * Standard Pylon (22 B length): 22 layers × 50.0 s = 1,100 seconds (≈ 18.3 minutes print time).
  * Hub Section S0 (16 B length): 16 layers × 50.0 s = 800 seconds (≈ 13.3 minutes print time).
* **Electrical Demand During Active Print:**
  * 22× Ship Welders: 22 × 400 kW = 8.80 MW peak.
  * 6× Hydraulic Pistons: 6 × 3.3 kW = 20 kW.
  * 1× Blueprint Projector: 30 kW.
  * Total Yard Load: **8.85 MW steady draw** (cleanly supplied by Ground Base Battery Bank).

---

### 5.2 · Master Pre-Flight Bill of Materials (BOM) & Mass Budget

| Subassembly Zone | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Skid Launch Way** | Heavy armor full blocks, half-blocks, blast door contact rails (Palette 0, 5) | 480 | 412.0 t | 480 |
| **Stationary Welder Arch** | Heavy armor boxed frame, 24× conveyor junctions, service ladders (Palette 0, 1) | 96 | 114.5 t | 144 |
| **Welder Array** | 22× Large Grid Ship Welders (8 Top, 7 Port, 7 Starboard) (Palette 3) | 22 | 44.0 t | 3,300 |
| **Hydraulic Pusher Spine** | 6× Large Grid Pistons in series, heavy guide frame, bump stops (Palette 2) | 18 | 32.4 t | 600 |
| **Pusher Carriage Head** | 1× Large Grid Merge Block, 1× Projector, blast slide shoes (Palette 9) | 8 | 12.8 t | 325 |
| **Component Feed Line** | Heavy conveyor tubes, 1× Large Conveyor Sorter (`EXT Sorter Feed`) (Palette 4) | 26 | 18.2 t | 95 |
| **Bunkering Umbilical** | 1× Advanced Rotor, 1× Hinge, 1× Connector (`EXT Fuel Conn`), 1× Sensor | 6 | 9.6 t | 215 |
| **Yard Control Booth** | 1× Control Seat, 2× LCD Panels, 1× Sound Block, 2× Event Controllers | 8 | 5.2 t | 165 |
| **TOTAL SHIPYARD YARD** | Complete Rig Linear Extruder Ring Facility | **664** | **≈ 648.7 t** | **5,324** |

---

### Phase 5-1 · Launch Way Foundation & Low-Friction Skid Rails (Z 0 to +50)

```
       [PHASE 5-1 SPATIAL LOCATOR HUD]
       Envelope: 12 W × 50 L × 2 H B (30.0 × 125.0 × 5.0 m)
       Coordinate Range: X −5 to +5, Z 0 to +50, Y = L0 to L2
       Primary Datums:
       · Launch Way Baseline: Z = 0 (Flush with Welder Ring face)
       · Port Skid Track Centerline: X = −3
       · Starboard Skid Track Centerline: X = +2
       · Skid Contact Elevation: Level L1 (Y = 0)
```

#### Step 5.1 · Reinforced Foundation Bed & Bedrock Pilings (L0)

Excavate a dead-level horizontal shelf advancing 50 B (125.0 m) forward along the launch axis (+Z). Anchor heavy armor block pilings deep into the planet voxel bed. Lay a continuous 12 W × 50 L foundation bed at Level L0 (Palette 0).

```
Elevation cross section, looking Forward (+Z) along Launch Axis:
        X: −5   −4   −3   −2   −1    0   +1   +2   +3   +4   +5
    L2     ░░        ░░                       ░░        ░░    ◄── Retaining Curbs
    L1     ■■■■■■■■■[BLAST]■■■■■■■■■■■■■■■■■■[BLAST]■■■■■■■■■  ◄── Low-Friction Track
    L0     ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■  ◄── Foundation Bed
```

#### Step 5.2 · Low-Friction Blast Door Skid Tracks & Lateral Guide Curbs (L1-L2)

Install twin parallel low-friction skid rails directly beneath the pylon keel runner coordinates at X = −3 and X = +2. Each track consists of continuous Blast Door Blocks capped with Blast Door Edges. Flank both outer rails with Heavy Armor Half-Blocks at Level L2 to form lateral containment curbs that prevent yaw drift during extrusion:

```
Plan view, Y = L1 to L2 (Skid Rail Geometry):
         X: −4    −3    −2        +1    +2    +3
     Z: +50 [CURB][BLAST][CURB]  [CURB][BLAST][CURB]  ◄── Runway Threshold (+125 m)
            [HALF][ DOOR][HALF]  [HALF][ DOOR][HALF]
     Z: +25 [CURB][BLAST][CURB]  [CURB][BLAST][CURB]  ◄── Mid-Runway Guide
            [HALF][ DOOR][HALF]  [HALF][ DOOR][HALF]
     Z:  0  [CURB][BLAST][CURB]  [CURB][BLAST][CURB]  ◄── Welder Arch Exit Threshold
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5.1 & 5.2 PARTS TRAY                                                            │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 300 │ 270.0 t / 300 │ Level L0 foundation pad (12 W × 50 L B)  │
│ Blast Door Block    │ 100 │ 98.0 t / 100  │ Level L1 runner rails at X = −3 and X = +2│
│ Heavy Armor Half    │ 80  │ 44.0 t / 80   │ Level L2 containment curbs flanking rails│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

#### Pre-Enclosure Inspection Gate 5.1A: Launch Way Levelness & Blast Skid Alignment Check
Before mounting the welder arch frame, verify:
* [ ] **Laser Levelness:** Entire 125.0 m runway foundation is planar with zero elevation variance across Y = L1.
* [ ] **Track Parallelism:** Distance between Port and Starboard blast door rails is exactly 5 B (12.5 m) center-to-center along the entire length.
* [ ] **Low-Friction Surface:** All contact surfaces on the rails are verified Blast Door blocks (Palette 5); zero full armor blocks protruding into the sliding plane.

```
PHASE 5-1 VISUAL MILESTONE · COMPLETED LAUNCH WAY RUNWAY
            X: −5                                          +5
        Z: +50 ┌──────────────────────────────────────────┐
               │  [PORT RAIL]              [STBD RAIL]   │
               │  ■■■■■■■■■■■              ■■■■■■■■■■■   │
               │  ■■■■■■■■■■■              ■■■■■■■■■■■   │
               │  ■■■■■■■■■■■              ■■■■■■■■■■■   │
        Z:  0  └──────────────────────────────────────────┘
               ◄──────────── 125.0 m Launch Bed ─────────►
```

---

### Phase 5-2 · Stationary 8 W × 7 H Welder U-Ring Arch & Conveyor Manifold (Z 0, Y L1 to L9)

```
       [PHASE 5-2 SPATIAL LOCATOR HUD]
       Envelope: 12 W × 3 L × 9 H B (30.0 × 7.5 × 22.5 m)
       Coordinate Range: X −5 to +5, Z −1 to +1, Y = L1 to L9
       Primary Datums:
       · Arch Centerline: X = 0, Z = 0
       · Clear Printing Throat: 8 W × 7 H B (X −4 to +3, Y L2 to L8)
       · Top Welder Rake: Level L8 (Y = 17.5 m), nozzles pointing DOWN (−Y)
       · Port Welder Rake: X = −5, nozzles pointing STARBOARD (+X)
       · Starboard Welder Rake: X = +4, nozzles pointing PORT (−X)
```

#### Step 5.3 · Heavy Structural Arch Frame & Printing Throat (L1-L9)

Erect a 3-block-deep boxed portal arch at Z −1 to +1 surrounding the launch way. The arch frames an unobstructed 8 W × 7 H B interior envelope (20.0 m wide × 17.5 m high). Build the structural frame from reinforced Heavy Armor blocks (Palette 0) with high-visibility Hazard Amber trim (Palette 1) along the perimeter rims:

```
Front Elevation, looking down the launch axis (Z = 0):
        X: −5  −4  −3  −2  −1   0  +1  +2  +3  +4  +5
    L9     ■═══■═══■═══■═══■═══■═══■═══■═══■═══■═══■  Roof Arch Cap
    L8     ║  [W] [W] [W] [W] [W] [W] [W] [W]  ║  (8 Top Welders pointing DOWN)
    L7     ║[W]                             [W]║
    L6     ║[W]                             [W]║  (7 Port Welders pointing STBD)
    L5     ║[W]      CLEAR PRINT THROAT     [W]║  (7 Stbd Welders pointing PORT)
    L4     ║[W]       (8 W × 7 H blocks)    [W]║
    L3     ║[W]                             [W]║  Internal Envelope:
    L2     ║[W]                             [W]║  20.0 m Wide × 17.5 m High
    L1     ■═══■═══[SKID ROLLER TRACK]══■═══■═══■  Tarmac Base (Y = 0)
```

#### Step 5.4 · 22× Ship Welder Grid & Balanced Conveyor Distribution Tree

Mount 22 Large Grid Ship Welders pointing inward into the print throat:
* **8× Top Welders (`EXT WLD Top 1` … `8`):** Mounted flush to the L8 ceiling at X −4, −3, −2, −1, 0, +1, +2, +3; nozzles facing DOWN (−Y).
* **7× Port Welders (`EXT WLD Port 1` … `7`):** Mounted vertically on the Port frame upright at X = −5, Y = L2 to L8; nozzles facing STARBOARD (+X).
* **7× Starboard Welders (`EXT WLD Stbd 1` … `7`):** Mounted vertically on the Starboard frame upright at X = +4, Y = L2 to L8; nozzles facing PORT (−X).
* Group all 22 units into terminal group: `EXT Welder Ring`.

Route a rigid Large Conveyor Tube network along the outer exterior perimeter of the arch at Z = −1. Use Conveyor Junctions behind every welder to ensure full structural interconnection and uniform component pressure across all 3 flanks:

```
2.5D Isometric Callout · Welder U-Ring Arch Architecture:

                   ┌───────────────────────────────────┐
                   │  Top Welder Rake (8× Ship Welders)│  (L8, Facing DOWN)
                   └─┬───────────────────────────────┬─┘
                     │                               │
        ┌────────────┴────┐                     ┌────┴────────────┐
        │ Port Welder Rake│  CLEAR PRINT ENVELOPE│ Stbd Welder Rake│
        │ (7× Ship Welders│   (8 W × 7 H blocks)│ (7× Ship Welders│
        │  Facing STBD)   │                     │  Facing PORT)   │
        └────────────┬────┘                     └────┬────────────┘
                     │                               │
        ═════════════╧═══════════════════════════════╧═════════════
                         Low-Friction Skid Roller Track (L1)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5.3 & 5.4 PARTS TRAY                                                            │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 72  │ 64.8 t / 72   │ Boxed portal frame columns and roof cap  │
│ Ship Welder (Large) │ 22  │ 44.0 t / 3,300│ 8 Top (L8 down), 7 Port (X−5), 7 Stbd (X+4)│
│ Conveyor Junction   │ 24  │ 18.0 t / 24   │ Outer manifold nodes behind each welder  │
│ Large Conveyor Tube │ 18  │ 9.0 t / 18    │ Interconnecting perimeter supply loops   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

#### Pre-Enclosure Inspection Gate 5.2A: 22× Welder Active Sphere Overlap & Conveyor Pressure Integrity
Before connecting the hydraulic pusher, verify:
* [ ] **Weld Sphere Overlap:** Each Ship Welder projects an active weld radius of 1.75 m†. Confirm that adjacent weld spheres overlap by a minimum of 0.25 m across all 3 faces, ensuring zero un-welded void spaces in structural blocks or internal armor plates.
* [ ] **Conveyor Loop Integrity:** Connect a test storage container to the manifold. Confirm all 22 welders pull steel plates without any missing conveyor links or disconnected elbows.
* [ ] **Clearance Gauge Check:** Run an 8 W × 7 H dummy block template through the portal. Confirm a strict 0.5 B air gap between welder nozzle faces and the exterior pylon armor skin.

```
PHASE 5-2 VISUAL MILESTONE · STATIONARY WELDER ARCH ASSEMBLED
                   ╔═══════════════════════════════╗
                   ║  [W] [W] [W] [W] [W] [W] [W] [W]  ║  ◄── Top Welder Rake
                   ║[W]                         [W]║
                   ║[W]    CLEAR PRINT THROAT   [W]║
                   ║[W]     (8 W × 7 H B)       [W]║  ◄── Welder Portal Arch
                   ║[W]                         [W]║
                   ║[W]   [SKID RUNWAY LEVEL]   [W]║
                   ╚══╦═════════════════════════╦══╝
```

---

### Phase 5-3 · 6-Stage Hydraulic Pusher Runway, Carriage & Merge Head (Z −28 to 0)

```
       [PHASE 5-3 SPATIAL LOCATOR HUD]
       Envelope: 5 W × 28 L × 3 H B (12.5 × 70.0 × 7.5 m)
       Coordinate Range: X −2 to +2, Z −28 to 0, Y = L1 to L3
       Primary Datums:
       · Abutment Anchor Wall: Z = −28
       · Piston Spine Axis: X = 0, Y = L2
       · Full Retraction Tip: Z = −3
       · Full Extension Tip: Z = +32 (60.0 m forward stroke)
```

#### Step 5.5 · Heavy Anchor Abutment Wall & Telescoping Piston Spine (Z −28 to −3)

At the aft end of the yard (Z = −28), erect a massive 5 W × 3 H heavy armor abutment wall anchored deep into the terrain. Along the centerline (X = 0, Y = L2), mount 6 Large Grid Pistons end-to-end in series:
* `EXT Pusher 1` (Mounted to anchor wall at Z = −28)
* `EXT Pusher 2` … `EXT Pusher 6` (Mounted sequentially along the telescoping spine)
* Group all 6 units into terminal group: `EXT Pushers`.
* **Piston Configuration Parameters:**
  * Velocity: `0.05 m/s` (exact synchronized extrusion rate).
  * Share Inertia Tensor: `ON` (MANDATORY on all 6 pistons to eliminate subgrid physics jitter / clang).
  * Max Distance: `10.0 m` per piston (Total combined stroke: 6 × 10.0 m = 60.0 m).
  * Min Distance: `0.0 m`.

```
Plan view, Y = L2 (Piston Pusher Axis):
        Z: −28       −23        −18        −13        −8         −3        0 (Welder Ring)
  Rail:  ■──[PIST 1]──[PIST 2]──[PIST 3]──[PIST 4]──[PIST 5]──[PIST 6]──[HEAD] ──► Pushes toward +Z
```

#### Step 5.6 · Pusher Carriage Tip, Projector Head & Merge Disconnect Block (Z −3 to 0)

At the tip of `EXT Pusher 6`, construct a rigid 3 W × 2 H carriage head riding on blast door skid pads:
1. **Pusher Merge Block (`EXT MRG Release`):** Large grid merge block mounted at the forward tip (Z = 0, X = 0, Y = L2) facing forward (+Z). This block provides the rigid physical connection to the pylon root collar during printing.
2. **Blueprint Projector (`EXT PRJ Head`):** Mounted directly adjacent to the merge block (X = 0, Y = L3, facing +Z). Configured with the target section blueprint (e.g., `RIG_PYLON_S1_V1`).
3. **Emergency Mechanical Bumper:** Twin Blast Door pads mounted on the carriage wings (X = −1 and X = +1) to transfer push loads directly into the pylon root frame without stressing the merge block mechanism:

```
2.5D Isometric Callout · Hydraulic Pusher & Merge Head Architecture:

  [ANCHOR WALL] (Z −28)
        ║
        ╠══[PISTON 1]══[PISTON 2]══[PISTON 3]══[PISTON 4]══[PISTON 5]══[PISTON 6]
        ║                                                              │
        ║                                                              ▼ (Carriage Tip)
        ║                                                    [EXT PRJ Head] (L3)
        ║                                                    [EXT MRG Release] (L2)
        ║                                                              │
        ╚══════════════════════════════════════════════════════════════╪═══════
                                                                       ▼
                                                             [PYLON ROOT COLLAR]
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5.5 & 5.6 PARTS TRAY                                                            │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Piston (Large Grid) │ 6   │ 19.8 t / 600  │ Inline series spine along X = 0, Y = L2  │
│ Merge Block (Large) │ 1   │ 2.4 t / 125   │ `EXT MRG Release` on carriage front face │
│ Projector (Large)   │ 1   │ 1.2 t / 100   │ `EXT PRJ Head` centered at Y = L3        │
│ Heavy Armor Block   │ 12  │ 10.8 t / 12   │ Anchor wall and carriage mounting cradle │
│ Blast Door Block    │ 4   │ 3.9 t / 4     │ Carriage slide guide shoes               │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

#### Pre-Enclosure Inspection Gate 5.3A: Piston Stroke Synchronization, Inertia Tensor & Merge Release Test
Before loading flight blueprints, execute this mandatory commissioning sequence:
* [ ] **Inertia Tensor Verification:** Confirm every piston (`EXT Pusher 1` … `6`) has `Share Inertia Tensor = ON`. An un-shared piston will induce destructive harmonic wobble during the 60 m stroke.
* [ ] **Linear Stroke Calibration:** Command group `EXT Pushers` to Extend at 0.05 m/s. Measure stroke progression: total travel must reach exactly 60.0 m ± 0.05 m without mechanical binding or track collision.
* [ ] **Merge Decoupling Test:** Place a dummy test block on `EXT MRG Release`. Toggle block OFF; verify instantaneous zero-force decoupling with zero residual grid adhesion.

```
PHASE 5-3 VISUAL MILESTONE · HYDRAULIC PUSHER RETRACTED & ARMED
  [ANCHOR ABUTMENT]
        ║
        ╠══[P1]══[P2]══[P3]══[P4]══[P5]══[P6]══[PRJ/MRG HEAD] ──► [READY FOR PRINT]
        ║
  ══════╩═════════════════════════════════════════════════════
```

---

### Phase 5-4 · Component Feed Umbilical, High-Pressure Sorter & Yard Console

```
       [PHASE 5-4 SPATIAL LOCATOR HUD]
       Envelope: 6 W × 12 L × 4 H B (15.0 × 30.0 × 10.0 m)
       Coordinate Range: X −8 to −4, Z −6 to +2, Y = L1 to L4
       Primary Datums:
       · Conveyor Tie-in: Ground Base Storage `GND STR Components` (Z −10, X +3)
       · Yard Control Console: Stationed on Port Flank [X −7, Z +2, Y L3]
       · Umbilical Bunkering Boom: Starboard Flank [X +6, Z +4, Y L2]
```

#### Step 5.7 · High-Throughput Conveyor Feeder Trunk from Ground Base

Route a reinforced Large Conveyor Tube line from the Ground Base central storage vault (`GND STR Components`) directly into the Port flank of the Welder Arch (Z = 0, X = −5). Install a Large Conveyor Sorter (`EXT Sorter Feed`) configured to pull required construction components continuously:
* **Sorter Settings:** Drain All = `ON`.
* **Whitelist Filter:** Steel Plate, Construction Component, Large Steel Tube, Small Steel Tube, Motor, Interior Plate, Computer, Display, Metal Grid, Bulletproof Glass.

#### Step 5.8 · Elevated Yard Operations Booth, Local Telemetry LCDs & Fuel Umbilical

1. **Yard Operations Booth:** Construct a 3 × 3 B observation station at X = −7, Z +2, Y L3 overlooking the extruder throat and the launch way.
   * `EXT Yard Seat`: Control station for the human yard master.
   * `EXT LCD Status`: Displays real-time piston extension, layer number, component flow rates, and shipyard power draw.
   * `EXT Sound Alert`: Klaxon sound block that triggers on print initiation, layer milestones, and launch release.
2. **Keel Umbilical Refueling Boom (`EXT Umbilical Boom`):** Mounted on the Starboard flank at X +6, Z +4, Y L2. Features an articulated Hinge arm with an Advanced Rotor and Large Connector (`EXT Fuel Conn`) that swings inward to lock with the extruded pylon's keel conveyor port for rapid H2 bunkering prior to flight.

```
Plan view, Y = L2 to L3 (Feed Line, Control Booth & Umbilical):
         X: −8   −7   −6   −5         +4   +5   +6   +7
     Z: +4                 │               │    [EXT Fuel Conn] (Boom)
     Z: +2  [EXT BOOTH]    │               │         ▲
            [LCD Status]   │               │         │
     Z:  0  [Yard Seat] ───┴──[WELDER RING]┴─────────┘
            [Sound Alert]     (22× Welders)
     Z: −2                 ▲
                           │
     Z: −6  ═══════════════╧═══════════════════════════
            Conveyor Trunk from [GND STR Components]
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5.7 & 5.8 PARTS TRAY                                                            │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Conveyor Tube │ 16  │ 8.0 t / 16    │ Main feeder line from Ground Base hold   │
│ Conveyor Sorter     │ 1   │ 1.2 t / 15    │ `EXT Sorter Feed` with component whitelist│
│ Control Station     │ 1   │ 0.8 t / 15    │ `EXT Yard Seat` in observation booth     │
│ Text Panel (Large)  │ 2   │ 0.6 t / 10    │ `EXT LCD Status` telemetry monitors      │
│ Connector (Large)   │ 1   │ 2.4 t / 150   │ `EXT Fuel Conn` on bunkering boom        │
│ Motorized Hinge     │ 1   │ 1.8 t / 100   │ Umbilical swing arm positioning hinge    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

#### Pre-Enclosure Inspection Gate 5.4A: Component Feed Line Throughput & Buffer Depletion Thresholds
Before starting an automated print cycle, verify:
* [ ] **Feed Trunk Throughput:** Confirm `EXT Sorter Feed` pulls a minimum of 250 Steel Plates per second into the arch manifold.
* [ ] **Foundry Reserve Threshold:** Check Ground Base inventory. Minimum reserves required before initiating a 22 B Pylon print:
  * Steel Plate: `45,000`
  * Construction Component: `8,500`
  * Large Steel Tube: `1,200`
  * Motor: `3,400`
  * Computer: `2,800`
* [ ] **Umbilical Swing Clearance:** Articulate `EXT Umbilical Boom` through its full arc. Confirm connector mates cleanly with the pylon keel port without colliding with the blast skid curbs.

```
PHASE 5-4 VISUAL MILESTONE · COMPLETE RIG SHIPYARD YARD COMMISSIONED
   [GND BASE FEED] ──────► [EXT Sorter Feed]
                                 │
                                 ▼
   [YARD BOOTH] ──────────► [WELDER ARCH] ◄────────── [REFUEL BOOM]
   [EXT LCD / Seat]         (22× Welders)              [EXT Fuel Conn]
                                 │
   [PUSHER SPINE] ═══════════════╪═══════════════════► [LAUNCH RUNWAY]
   (6× Pistons)                  │                     (125 m Skid Way)
```

---

### Phase 5-5 · Automated Extrusion Cycle & Pylon Flight Commissioning Sequence

The construction of every 22 B pylon follows an automated, 5-stage mechanical and flight commissioning cycle:

```
                  STAGE 1: BLUEPRINT ALIGNMENT & ROOT WELD
                  Pusher Head at Z = 0 · `EXT PRJ Head` Projecting
                  `EXT Welder Ring` ON · Root Collar Fused to `EXT MRG Release`
                                     │
                                     ▼
                  STAGE 2: SYNCHRONIZED LINEAR EXTRUSION
                  `EXT Pushers` Extend at 0.05 m/s · Continuous Component Pull
                  22 layers extruded at 50 s/layer (1,100 s total print)
                                     │
                                     ▼
                  STAGE 3: FULL EXTENSION & WELD SHUTDOWN
                  Pistons reach 60.0 m stroke · Pylon hull fully on skid rails
                  `EXT EC Print` detects limit · `EXT Welder Ring` turned OFF
                                     │
                                     ▼
                  STAGE 4: KEEL UMBILICAL DOCKING & H2 BUNKERING
                  `EXT Umbilical Boom` swings inward · Mates to keel port
                  Ground Base pumps H2 tanks to 100% (≈ 90 seconds†)
                                     │
                                     ▼
                  STAGE 5: AVIONICS PROOFING, MERGE DECOUPLING & ASCENT
                  Seed Kit Batteries & Remote Control ON · Gyros armed
                  7× Large Atmo thrusters spool to 100% for 2.0 s proof
                  `EXT MRG Release` toggles OFF · Pylon ascends to Rig H5
```

#### Step 5.9 · 5-Stage Automated Extrusion, Fueling & Flight Commissioning

1. **Stage 1 (Root Collar Fusion):**
   * Retract `EXT Pushers` fully (0.0 m stroke).
   * Activate `EXT PRJ Head` with target pylon blueprint.
   * Turn `EXT Welder Ring` ON. The first structural layer of the pylon root welds directly to the face of `EXT MRG Release`.
2. **Stage 2 (Continuous Extrusion):**
   * Command `EXT Pushers` forward at `0.05 m/s`.
   * As the pylon advances through the arch, the 22 inward-facing ship welders instantaneously deposit components onto the projected voxels.
   * Pylon keel blast plates glide smoothly over the low-friction blast door skid rails at Level L1.
3. **Stage 3 (Print Completion):**
   * When `EXT Pusher 6` reaches 10.0 m extension (total stroke = 60.0 m), Event Controller `EXT EC Print` triggers:
     * `EXT Welder Ring` toggles OFF.
     * `EXT Sound Alert` plays a distinct 3-tone completion chime.
     * `EXT Pushers` velocity set to `0.0 m/s`.
4. **Stage 4 (Keel Umbilical Refueling):**
   * `EXT Umbilical Boom` hinge swings 45° inward to engage the pylon's lower conveyor port.
   * `EXT Fuel Conn` locks. High-pressure H2 gas from Ground Base fills the pylon's internal hydrogen reserve tanks to 100% capacity.
   * Umbilical connector unlocks and swings back to flush starboard park.
5. **Stage 5 (Avionics Activation, Release & Autonomous Ascent):**
   * Pylon Seed Kit receives ignition pulse via IGC channel `RIG.EXT.COMMISSION`:
     * Pylon batteries switched from `Recharge` to `Auto`.
     * Remote Control and Gyroscopes powered ON.
     * Radio Antenna turned ON (Broadcasting on fleet band).
     * 7× Large Atmospheric Lift Thrusters spooled to 100% override for 2.0 seconds to verify positive lift authority.
   * `EXT MRG Release` toggles OFF. Pylon decouples cleanly from the pusher carriage.
   * Remote Control engages autopilot climb to 500 m altitude, aligns with the Floating Rig, and navigates autonomously to dock with the target H5 pylon socket.
   * `EXT Pushers` reverse at `0.20 m/s` to return the pusher carriage to Z = 0, readying the yard for the next pylon print.

#### Pre-Enclosure Inspection Gate 5.5A: Pylon Seed Kit Avionics, H2 Fueling & Autonomous H5 Docking Handshake
Prior to authorizing automated merge release on any flown section, verify:
* [ ] **H2 Reserve Verification:** Pylon onboard hydrogen tanks show 100% fill level on yard telemetry.
* [ ] **Lift Authority Proof:** Under 100% thruster test override, upward thrust produces a minimum of 1.30 T/W ratio against the pylon's dry mass at sea level (Earth 1 g).
* [ ] **IGC Telemetry Handshake:** Base C2 Bridge confirms receipt of heartbeat ping on `TMS.PYLON.STATUS` with zero packet loss.

```
PHASE 5-5 VISUAL MILESTONE · PYLON COMPLETION & COLD LAUNCH ASCENT
                       ▲  Autonomous Flight to Rig (500 m Altitude)
                       │  7× Large Atmo Thrusters Firing
                       │
             ┌───────────────────────────────────┐
             │    COMPLETED 22 B PYLON SECTION   │ ──► [ASCENDING TO RIG H5]
             └───────────────────────────────────┘
                               ▲
                              ╱ ╲  (Lift Thruster Plumes Clear of Runway)
                             ═════
  [PUSHER HEAD]             [SKID RUNWAY]
  [EXT MRG Release: OFF]    (Empty Skid Rails Ready for Carriage Retract)
```

---

### 5.6 · Master AI Scripting Contract & Terminal Namespace (Claude Code / RIG//OS)

Every automated device on the Extruder Ring shipyard conforms to this exact naming and grouping dictionary:

#### 1. Group Taxonomy
* `EXT Welder Ring`: Exactly 22× Large Grid Ship Welders on the stationary arch.
* `EXT Pushers`: Exactly 6× Large Grid Telescoping Pistons along the launch axis spine.
* `EXT Yard Displays`: All text and LCD panels dedicated to shipyard operations.

#### 2. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Piston (Large) | `EXT Pusher 1` … `6` | 6-stage linear extension spine · `Velocity = 0.05 m/s` · `Share Inertia Tensor = ON` |
| Merge Block (Large) | `EXT MRG Release` | Carriage tip physical docking lock to pylon root collar |
| Projector (Large) | `EXT PRJ Head` | Carriage tip blueprint projection unit |
| Ship Welder (Large) | `EXT WLD Top 1` … `8` | Top arch rake welders (L8, facing DOWN) |
| Ship Welder (Large) | `EXT WLD Port 1` … `7`| Port arch upright welders (X −5, facing STBD) |
| Ship Welder (Large) | `EXT WLD Stbd 1` … `7`| Starboard arch upright welders (X +4, facing PORT) |
| Conveyor Sorter | `EXT Sorter Feed` | High-pressure component drain sorter from Ground Base hold |
| Connector (Large) | `EXT Fuel Conn` | Umbilical bunkering connector on Starboard swing boom |
| Motorized Hinge | `EXT Umbilical Hinge` | Swing arm positioning hinge for bunkering boom |
| Control Station | `EXT Yard Seat` | Operator console in observation booth |
| Text Panel (Large) | `EXT LCD Status` | Yard operations telemetry and print progress display |
| Sound Block | `EXT Sound Alert` | Audio alarm klaxon for print state transitions |
| Event Controller | `EXT EC Print` | Monitors pusher extension to trigger print completion sequence |
| Event Controller | `EXT EC Safety` | Monitors component buffer to pause pushers on stock depletion |
| Timer Block | `EXT TMR Step` | Hardware watchdog pulsing periodic kinematic checks (1.0 s) |

#### 3. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `EXT EC Print` | `EXT Pusher 6` Position | Extension = Max (10.0 m / 60 m total) | `EXT Welder Ring` OFF · `EXT Sound Alert` Play · Trigger `EXT TMR Commission` | `EXT Welder Ring` ON |
| `EXT EC Safety` | `GND STR Components` | Steel Plate Count < 500 units | `EXT Pushers` Velocity = `0.0 m/s` (Pause Print) · `EXT Sound Alert` Play Warning | `EXT Pushers` Velocity = `0.05 m/s` (Resume) |
| `EXT EC Fuel` | `EXT Fuel Conn` | Lock Status = Locked | Start H2 Tank Fill · Set Pylon Tanks to Stockpile | Stop H2 Fill · Unlock Umbilical |

#### 4. IGC Channel Subscriptions & Broadcast Protocols
* Inbound: `RIG.EXT.COMMAND` (`START;<blueprint_name>`, `PAUSE`, `RESUME`, `ABORT`, `COMMISSION`).
* Inbound: `TMS.BASE.QUOTAS` (Refined component inventory updates from Ground Base C2).
* Outbound: `RIG.EXT.STATUS` (Broadcasts `EXTRUDING;<pylon_id>;<progress_pct>;<est_time_remaining_sec>`).
* Outbound: `RIG.EXT.COMMISSION` (Ignition command pulse sent to completed pylon seed kit avionics).

#### 5. Universal Zero-GC Scripting Invariants & Verification Checklist
All shipyard PB control scripts authored by Claude Code or human engineers MUST satisfy these hard runtime constraints:
* **C# 6 Only:** nested in `Program`, no C# 7+ features (no tuples, no pattern matching, no out variables).
* **Zero Allocations in Loops:** No `new`, no string formatting or string interpolation in `Update1/10/100` or `Main()`.
* **String Allocation Bypass:** String concatenation replaced with pre-allocated `StringBuilder` buffers; terminal block queries cached during initialization.
* **Minifier Immunity:** Code must withstand `minify=full`. Never use `enum.ToString()`, `Enum.Parse`, or `nameof()`. CustomData keys, storage strings, and IGC tags must be string literals.
* **Strict Grid Scoping:** Every block query MUST filter `b => b.CubeGrid == Me.CubeGrid`. Mechanical subgrid blocks (pusher pistons, merge heads, umbilical hinges) must be tracked via explicitly cached entity IDs.

---

# Volume V · Terrestrial Surface Heavy Extraction (Ground Tier · Bulk / Mass)

This volume details the heavy wheeled surface extraction machines that form the high-volume bulk industrial tier of the terrestrial extraction loop.

## Chapter 10 · MASTODON-01 Expeditionary Mining Train & Mobile FOB (MAS-01)

Source: `docs/rig/chapters/mastodon-instructions.md` · 11 × 42 × 7 B · 2-Segment Consist, 4× Multitool Repair Turrets, SML Auto-Production & Mule Peer Resupply Sorties, 3-Axis Gimbal Hitch & DPU Traction, Standalone FOB Autonomy, Custom Walk-In Bridge, 35 m Tri-Mode Laser Boom, Closed-Loop Salvage Recycling, JIT Logistics & Swarm Comms, ULSS Life Support, Failsafe Decoys & Scuttle

### 0 · Reference Grid, In-Game Paint Palette & Master Consist Layout

#### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the Tractor belly foundation at Level L1, along centerline X0.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. Spans X −5 to +5 (11 blocks wide = 27.5 m).
* **Y (Elevation):** Wheel hubs and belly skid line are Level L1 (Y = 0), advancing upward to Flight Roof at Level L6 (6 blocks high = 15.0 m).
* **Z (Length / Longitudinal):** Stern is negative (−), Bow is positive (+). Spans Z −26 to +12 (38 blocks vehicle body + 4 blocks hitch/bumpers = 42 blocks total = 105.0 m).
  * Tractor Carriage (`MAS-01-T`): Z −8 to +12 (20 blocks length = 50.0 m). Center axle datum is Z0.
  * Articulated Hitch Gap: Z −9 to −8 (1 block mechanical gap = 2.5 m).
  * Utility Carriage (`MAS-01-U`): Z −26 to −8 (18 blocks length = 45.0 m).

```
Spatial Alignment Triad:
       [+Y: Zenith / Flight Roof L6]
              │
              │   [+Z: Bow / Tractor Bridge & Arm]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Utility Carriage & Cat Dump]
              │
       [−Y: Ground Contact / Level L0]
```

---

#### 0.2 In-Game Paint Palette & Functional Color Coding

To prevent builder disorientation across a 105 m long articulated consist, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Lower chassis box girders, wheel arch frames, belly skid plates |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Roof Mule Gantry Pad (7×7 B), cradle arms, outrigger warning chevrons |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Stern train dump bumper, rear alignment push-blocks, drawbar hitches |
| **Slot 3** | Excavator Ochre | `#E67E22` | Deep Orange | 45 m Long-Reach Articulated Boom, arm casings, laser tool head |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Ore Conveyor Spine, bow intake throat, manifold junctions |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Tractor ore buffer and Utility component storage holds |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Tractor Power Vault (2× Small Nuclear Reactors, 2× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Forward Armored C2 Bridge exterior shell, operator consoles |
| **Slot 8** | Scout Flight White | `#ECF0F1` | Clean White | Forward twin Kestrel scout drone landing pads (P1, S1) on Utility roof |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Exterior catwalks, maintenance ladders, corner outrigger foot pads |

---

#### 0.2B Exterior Livery & Two-Tone Color Blocking Map

To achieve an authentic brutalist industrial locomotive aesthetic and maintain spatial orientation during field construction, paint must follow strict functional zone lines across the hull:

```
[EXTERIOR ELEVATION LIVERY MAP · PORT PROFILE (LOOKING STARBOARD)]
Level
L7  │                                                                 [SLOT 7: NAVY CIWS / MT]
L6  │                      [SLOT 7: NAVY COMM SPIRE]                  [SLOT 1: AMBER MULE CRADLE]
L5  │                     /   [SLOT 7: NAVY CAB]    \                /  [SLOT 8: WHITE KESTREL]  \
L4  │ ═══════════════════[SLOT 0: INDUSTRIAL DARK GRAY MAIN HULL & WORKSHOP ARMOR]═════════════════
L3  │ ───────────────────[SLOT 2: HEAVY RAIL CRIMSON BELTLINE STRIPE (Z +12 to Z −26)]──────────
L2  │ ═══════════════════[SLOT 0: INDUSTRIAL DARK GRAY LOWER CHASSIS & SPONSONS]══════════════════
L1  │      (O)====(O)====(O)           [SLOT 2: DRAWBAR]           (O)====(O)   [SLOT 2: BUMPER]
        Axle 1  Axle 2  Axle 3        [SLOT 9: GANGWAY]          Axle 4  Axle 5
```

| Zone / Component | Primary Palette Slot | Hex Code | Visual Application & Boundaries |
| :--- | :--- | :--- | :--- |
| **Lower Keel & Skids (L1)** | Slot 0 · Dark Slate | `#2B303A` | Underside belly channel, skid plates, wheel hubs, axle mount blocks |
| **Running Gear Fenders (L2)** | Slot 0 · Dark Slate | `#2B303A` | Flared mudguards, wheel well sponsons, chassis box girders |
| **Hull Beltline Accent (L3)** | Slot 2 · Rail Crimson | `#9E2A2B` | Continuous 1-block perimeter racing stripe from bow glacis to stern |
| **Superstructure Walls (L3-L5)** | Slot 0 · Dark Slate | `#2B303A` | Main hull armor slabs, workshop sidewalls, reactor vault casing |
| **C2 Armored Bridge Cab (L4-L5)**| Slot 7 · Fleet Navy | `#1B263B` | Armored cab visor, window mullions, console pedestals, comms spire |
| **Articulated Excavation Arm** | Slot 3 · Ochre Orange | `#E67E22` | 45 m boom arm girders, elbow hinges, laser tool head casing |
| **Mule Gantry Cradle Pad (L6)** | Slot 1 · Hazard Amber | `#E6A100` | 7×7 blast deck border, motorized cradle rub rails, alignment arrows |
| **Kestrel Scout Pads (L5-L6)** | Slot 8 · Scout White | `#ECF0F1` | 4×4 drone landing squares (P1, S1) on forward utility carriage roof |
| **Personnel Walkways & Catwalks**| Slot 9 · Safety Yellow | `#F1C40F` | Grated catwalks, ladders, stair runs, boarding thresholds |
| **Emergency & High-Voltage** | Slot 6 · Alert Red | `#C0392B` | Nuclear reactor access hatches, battery vault markings, scuttle panels |

---

#### 0.3 Master Consist Top Plan View (Level L3-L6)

```
[TOP PLAN VIEW · MASTODON-01 MKI CONSIST · LEVEL L3-L6]

                      === TRACTOR LOCOMOTIVE (MAS-01-T) ===                               === UTILITY CARRIAGE (MAS-01-U) ===
    Port [-X]                                                                                                                       Port [-X]
        ┌─────────┬──────────────┬──────────────┬──────────────┬───────────────┐ ┌──────────────┬─────────────────────────────┬─────────────┐
        │ [O-R1]  │  Wheel A1-P  │  Wheel A2-P  │  Wheel A3-P  │  [O-R2]       │ │ Wheel B1-P   │  [KES Pad P]                │ Wheel B2-P  │
        │ Piston  │  (Steer 30°) │  (Steer 15°) │  (Drive 0°)  │  Piston       │ │ (Drive 0°)   │  4x4 sb Recon Pad           │ (Steer -25°)│
        ├─────────┴──────────────┴──────────────┴──────────────┴───────────────┤ ├──────────────┴─────────────────────────────┴─────────────┤
        │ [C2 BRIDGE]              [30 MW REACTOR]                             │ │ [FIELD WORKSHOP: Assembler / MedBay / O2]                │
Bow     │ 2-Seat Panoramic Cockpit  Nuclear Core                               │ │                                                   Stern   │
[+Z]    │       │                         │                                    │ │       [7 x 7 B MULE-01 GANTRY CRADLE PAD]          Cat   [-Z]
        │    [BOOM BASE] ── (45 m Articulated 5-DOF Laser Arm)                 │ │          (Flush Heavy Armour Blast Deck            Dump   │
        │    360° Turret + Shoulder/Elbow Hinges + Wrist Roll/Pitch            │ │           Motorized Lateral Clamping Rails)       Bumper  │
        │       │                         │                                    │ │                                                     │     │
        │ [SURVIVAL KIT]           [LARGE CARGO BUFFER]                        │ │ [COMPONENT RESERVE HOLD: Motors / Plates / Glass]   │     │
        ├─────────┬──────────────┬──────────────┬──────────────┬───────────────┤ ├──────────────┬─────────────────────────────┬─────────────┤
        │ [O-R3]  │  Wheel A1-S  │  Wheel A2-S  │  Wheel A3-S  │  [O-R4]       │ │ Wheel B1-S   │  [KES Pad S]                │ Wheel B2-S  │
        │ Piston  │  (Steer 30°) │  (Steer 15°) │  (Drive 0°)  │  Piston       │ │ (Drive 0°)   │  4x4 sb Recon Pad           │ (Steer -25°)│
        └─────────┴──────────────┴──────────────┴──────────────┴───────────────┘ └──────────────┴─────────────────────────────┴─────────────┘
    Stbd [+X]                                     ▲                                              ▲                                  Stbd [+X]
                                                  │                                              │
                                           [TRACTOR CHASSIS]                              [UTILITY CHASSIS]
                                                  └─────────────── [2-AXIS MOTOR DRAWBAR] ───────┘
                                                                   (Pitch Hinge + Yaw Rotor)
```

---

#### 0.4 Master Profile Elevation (Looking Starboard, X = 0 Plane)

```
        ◄────────── CARRIAGE 1: LOCOMOTIVE / EXCAVATOR ──────────►     ◄────────── CARRIAGE 2: FOB / UTILITY MODULE ──────────►
                    (11 W × 20 L × 5 H B · 6 Wheels)                               (11 W × 18 L × 5 H B · 4 Wheels)
                     ┌─[ARTICULATED LASER BOOM]                                   ┌─────────────────┬─────────────────┐
                     │ (Deployed Reach: 35-45 m)                                  │ MULE GANTRY PAD │ 2× KESTREL PADS │  Flight Roof (L6)
        ┌────────────┴───┐                                                        │ (7×7 B Blast)   │ (Scout Wings)   │
        │ ARMORED BRIDGE │                                     [DRAWBAR HITCH]    ├─────────────────┴─────────────────┤
        │ (Panoramic C2) │   [POWER CORE]    [ORE BUFFER]     2-Axis Hinge Joint  │ FIELD WORKSHOP  │ COMPONENT HOLD  │  Main Deck (L3-L5)
[BOW]   │ [ORE DETECTOR] │   (2× Reactors)   (L Cargo Hold)    Conveyor Pass-Thru │ (Assembler/Med) │ (Spare Parts)   │  [STERN DUMP]
════════╧════════════════╧═══════════════════╧═════════════════════════╦══════════╧═════════════════╧═════════════════╧═══════════► (To Cat Train)
[STABILIZER]           [WH 1..2]           [WH 3..4]        [WH 5..6]  ║        [WH 7..8]         [WH 9..10]      [STABILIZER]
(Voxel Clamp)         (Steer +30°)        (Steer +15°)     (Drive 0°)  ║       (Drive 0°)        (Steer −25°)     (Voxel Clamp)
```

---

#### 0.4B Consist 3/4 Isometric Profiles & Stance Wireframes

```
[3/4 ISOMETRIC PERSPECTIVE · TRACTOR BOW & DORSAL SPINE]
Looking Down & Aft from Forward Starboard Vantage

                 [LASER COMM SPIRE L6]
                         │
                 [BOW CIWS L5]   [BOOM AZIMUTH PEDESTAL L4]
                       │                 │
                ┌──────┴──────┐          ▼
               ╱  CAB VISOR   ╲     ┌─────────┐
              ╱   CANOPY 45°   ╲════╡  TURRET ╞══════[DORSAL SPINE TROUGH]════►
             │  PANORAMIC C2    │   └────┬────┘      (14-Block Recessed Canal)
             ├──────────────────┤        │
       ┌─────┴──────────────────┴─────┐  │
      ╱   FORWARD CHIN PLOW / GLACIS   ╲ │
     │  [X0, Z +12 HEAVY ARMOR PROW]    ││
     └──────┬────────────────────┬──────┘│
            │                    │       │
       [AXLE 1-P]           [AXLE 1-S]   │
       (5×5 Off-Road)       (5×5 Off-Road)
```

```
[3/4 ISOMETRIC PERSPECTIVE · CARRIAGE 2 STERN GANTRY & FLIGHT DECK]
Looking Down & Forward from Aft Starboard Vantage

               [STERN CIWS L7]     [GANTRY REPAIR TURRET L7]
                      │                     │
               ┌──────┴─────────────────────┴──────┐
               │    HEAVY GANTRY CLEARANCE ARCH    │ (L7, Height: 17.5 m)
               └───┬───────────────────────────┬───┘
                   │  [MULE-01 CRADLE PAD L6]  │
                   │   7×7 B Flush Heavy Deck  │
        ┌──────────┴───────────────────────────┴──────────┐
       ╱  [KESTREL PAD P]                 [KESTREL PAD S]  ╲
      │    4×4 Recon Pad                   4×4 Recon Pad    │
      ├─────────────────────────────────────────────────────┤
      │            STERN DUMP OVERHANG & PUSH-BLOCK         │ (L2-L3)
      └─────────┬─────────────────────────────────┬─────────┘
                │                                 │
           [AXLE 5-P]                        [AXLE 5-S]
           (5×5 Off-Road)                    (5×5 Off-Road)
```

---

#### 0.4C Transverse Hull Station Profiles & Chamfer Specifications

To avoid uniform "shoebox" cross-sections, the Mastodon utilizes specific Space Engineers armor block morphologies (`Armor Slope 2×1×1 Base/Tip`, `Half-Slope`, `Inverted Corner`, `Half-Block`) across 5 critical hull stations:

##### Station 1 · Bow Prow & Cab Glacis (Z = +11, Looking Aft)
```
           X: −3       X: −2        X0         X: +2       X: +3
L5 Crown         ┌───────────┬──────────────┬───────────┐
                 │ 2×1 SLP \ │ FLAT ROOF L5 │ / 2×1 SLP │   ◄── Cab Brow Visor (Inward Chamfer)
L4 Bridge        ├───────────┴──────────────┴───────────┤
                 │ 45° SLOPED HEAVY GLASS CANOPY (7 W)  │   ◄── Panoramic Glacis (L4 Deck)
L3 Sills   ┌─────┴──────────────────────────────────────┴─────┐
           │ 2×1 INV \      WALKWAY AISLE L3      / 2×1 INV   │   ◄── Outward Flare to Main Hull
L2 Spons   ├─────────┬──────────────────────────────┬─────────┤
           │ HALF-SLP│     CHASSIS BOX GIRDERS      │ HALF-SLP│   ◄── Mudguards over Axle 1
L1 Keel    └────┬────┴───────┬──────────────┬───────┴────┬────┘
                │            │ BELLY TROUGH │            │
                ▼            └──────────────┘            ▼
          [WHEEL 5×5 P1]       (1.25 m Keel)       [WHEEL 5×5 S1]
```

##### Station 2 · Mid-Tractor & Boom Pedestal (Z = +4, Looking Aft)
```
           X: −4       X: −2        X0         X: +2       X: +4
L4 Pedestal      ┌───────────┬──────────────┬───────────┐
                 │ 2×1 SLP \ │ AZIMUTH BASE │ / 2×1 SLP │   ◄── Turret Pedestal Coamings
L3 Hull    ┌─────┴───────────┴──────────────┴───────────┴─────┐
           │ SLP 45° \   JEFFERIES    BUFFER     SIDE ARMOR   │   ◄── Symmetrical Flank Chamfers
L2 Deck    ├─────────┬──────────────────────────────┬─────────┤
           │ HALF-BLK│   PRIMARY CHASSIS LONGERONS  │ HALF-BLK│   ◄── Axle 2 Wheel Arches
L1 Keel    └────┬────┴───────┬──────────────┬───────┴────┬────┘
                │            │ ORE CONVEYOR │            │
                ▼            └──────────────┘            ▼
          [WHEEL 5×5 P2]       (Processing)        [WHEEL 5×5 S2]
```

##### Station 3 · Inter-Carriage Hitch & Gangway (Z = −8 to −9, Looking Aft)
```
           X: −3       X: −1        X0         X: +1       X: +3
L4 Roof          ┌───────────┬──────────────┬───────────┐
                 │ 2×1 SLP \ │ HULL COAMING │ / 2×1 SLP │   ◄── Spine Trough Termination
L3 Gangway ┌─────┴───────────┴──────────────┴───────────┴─────┐
           │ SLP 45° \   FLEXIBLE ACCORDION GANGWAY   / SLP 45│   ◄── Hermetic Bellows Collar
L2 Drawbar ├─────────┬──────────────────────────────┬─────────┤
           │ 2×1 BASE│ 3-AXIS GIMBAL DRAWBAR HITCH  │ 2×1 BASE│   ◄── Heavy Articulated Tongue
L1 Skids   └─────────┴───────┬──────────────┬───────┴─────────┘
                             │ MAGNETIC PAD │
                             └──────────────┘ (Emergency Breakaway Keel Skid)
```

##### Station 4 · Workshop & Radiation Citadel (Z = −14, Looking Aft)
```
           X: −4       X: −2        X0         X: +2       X: +4
L6 Catwalk       ┌───────────┬──────────────┬───────────┐
                 │ CATWALK   │ KESTREL WING │  CATWALK  │   ◄── Flight Deck Walkways
L5 Roof    ┌─────┴───────────┴──────────────┴───────────┴─────┐
           │ 2×1 SLP \   FLIGHT DECK BLOCKED HULL   / 2×1 SLP │   ◄── Upper Superstructure
L4 Upper   │ VERTICAL│    REFINERY     ASSEMBLER    │ VERTICAL│
L3 Deck    │ 2×1 INV \   CORRIDOR     CITADEL VLT   / 2×1 INV │   ◄── Radiation Citadel Armor
L2 Vault   ├─────────┬──────────────────────────────┬─────────┤
           │ HALF-SLP│ SUB-FLOOR BATTERIES & AMMO   │ HALF-SLP│   ◄── Mudguards over Axle 4
L1 Keel    └────┬────┴───────┬──────────────┬───────┴────┬────┘
                │            │ BALLAST SKID │            │
                ▼            └──────────────┘            ▼
          [WHEEL 5×5 P4]       (40× Plates)        [WHEEL 5×5 S4]
```

##### Station 5 · Stern Gantry Arch & Caterpillar Dump (Z = −23 to −26, Looking Aft)
```
           X: −3       X: −1        X0         X: +1       X: +3
L7 Arch          ┌───────────┬──────────────┬───────────┐
                 │ CIWS MOUNT│ GANTRY ARCH  │ REPAIR MT │   ◄── Overhead Clearance Arch
L6 Flight  ┌─────┴───────────┴──────────────┴───────────┴─────┐
                 │ MULE-01 ELEVATED CRADLE BED (7×7)│             ◄── Heavy Blast Deck
L4 Stern   │ 2×1 SLP \   COMPONENT STERN HOLD       / 2×1 SLP │
L3 Mezz    │ VERTICAL│   LADDER & DECON AIRLOCK     │ VERTICAL│
L2 Bumper  ├─────────┴──────────────────────────────┴─────────┤
           │ HEAVY RAIL CRASH BUFFER & TAILINGS CHUTE (X0)    │   ◄── Caterpillar Push-Dock
L1 Skids   └──────────────────────────────────────────────────┘
```

---

#### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

```
               45 m ARTICULATED LASER EXCAVATION BOOM (Dorsal Pedestal)
                     [Tool Head: 2× Simple Laser Multitools]
                                         │
                                         ▼ (Internal Large Hinge / Rotor Pass-Thru)
                       [TRACTOR ORE BUFFER & SUMP] (3×3×3 B Large Cargo Hold)
                       ◄── (Physical Cargo Hatch: Port Jefferies Tube [X −2, Z +2, L3])
                                         │
                                         ▼ (2-Axis Drawbar Articulated Conveyor Pipe)
                       [UTILITY FOB CONVEYOR DISTRIBUTION MANIFOLD]
                                         │
        ┌────────────────────────────────┼────────────────────────────────┐
        ▼ (Upward Riser Trunk)           ▼ (Internal Workshop Loop)       ▼ (Stern Bumper Pipe)
 [MAS MP Sorter] (Drain ON)       [Field Basic Refinery]           [MAS Stern Sorter] (Drain ON)
 (Inspection Hatch: Deck L5)      (330 kW · In-Situ Metallurgy)    (Inspection Hatch: Aft L2)
        │                         (Manual Hopper: Aisle X −2)             │
        ▼                                │ (Ingots)                       ▼
 [MAS MP Conn] (Roof Flush L6)    [Field Large Assembler]          [MAS Stern Dump Conn] (L2 Bumper)
 (650 t rare ore to Mule)         (560 kW · Spare Parts/Repair)    (2,500 t bulk ore to Cat Train)
                                  (Dual Panels: Aisle X −2)
                                         │
                                  [O2/H2 Gen & 2× H2 Tanks]
                                  (10,000,000 L Fueling Line)
                                  (Manual Bunkering Valves: Aisle X +2)
```

##### Physical Maintenance Ports & Inspection Hatches

| Subsystem Component | Grid Location | Physical Access Interface | Maintenance & Survival Function |
|---|---|---|---|
| `MAS Tractor Buffer` | [X −2, Z +2, L3] | Large Cargo Access Hatch | Manual ore inspection, debris clearing, small tool withdrawal |
| `MAS Reactor 1, 2` | [X0, Z −2..−3, L3] | Front Maintenance Hatch | Manual Uranium fuel rod loading if conveyor line is disabled |
| `MAS Engine H2` | [X0, Z −4, L3] | Aft Valve & Sump Port | Manual hydrogen fuel shutoff, cold-start crank, leak inspection |
| `MAS Field Refinery`| [X −2, Z −12, L3] | Top Hopper Access Door | Manual high-grade ore insertion directly into refinery input |
| `MAS Utility Assembler`| [X −2, Z −14, L3] | Dual Inventory Terminals | Manual blueprint queuing, physical component withdrawal |
| `MAS Hold Components`| [X0, Z −22, L3] | Forward Large Port | Primary bulk parts terminal facing central workshop aisle |
| `MAS Sorter Tailings`| [X0, Z +2, L1] | Underbelly Service Flange| Ground-level access between Axles 2-3 to clear stone jams |
| `MAS MP Sorter` | [X0, Z −20, L5] | Riser Inspection Hatch | Flight deck ladder landing access to service rare ore sorter |
| `MAS Stern Sorter` | [X0, Z −26, L2] | Rear Bumper Access Door | Stern ladder descent access to service bulk ore dump sorter |

---

#### 0.6 Master Electrical Single-Line Schematic & 5-Mode Energy Management Suite

```
               TRI-FUEL POWER GENERATION & ENERGY STORAGE ARCHITECTURE
   [TIER 1: NUCLEAR BASELOAD]       [TIER 2: RENEWABLE LIFELINE]      [TIER 3: EMERGENCY H2]
   2× Small Nuclear Reactors        4× Roof Flush Solar Panels        1× Hydrogen Engine
   (30.0 MW Continuous)             (≈ 640 kW Peak Daylight†)         (5.0 MW Standby / Failover)
               │                                │                                │
               └───────────────────────┬────────┴────────────────────────────────┘
                                       ▼
                       ENERGY STORAGE BUFFER (18.0 MWh Total · 72.0 MW Peak)
               3× Tractor Batteries (9 MWh) + 3× Utility Batteries (9 MWh)
                                       │
                                       ▼
                              PRIMARY TRACTIVE BUS
        ┌──────────────────────────────┼──────────────────────────────┐
        │                              │                              │
        ▼                              ▼                              ▼
 TRACTOR POWERTRAIN & HYD       ARTICULATED BOOM               UTILITY CARRIAGE UMBILICAL
 6× 5×5 Wheel Drive Motors      Turret Pan Rotor (Azimuth)     Field Basic Refinery (330 kW)
 4× Corner Outrigger Cylinders  Shoulder & Elbow Pitch Hinges  Field Assembler Core (560 kW)
 Steering Actuators (Axles 1-2) Laser Mining Head (4.0 MW)     Mule Float Charger (12 MW)
 Retractable Crew Ladder Hinge  Tool Head Wrist Roll / Pitch   Kestrel Fast Chargers (2× 1 MW)
```

##### 5 Operational Energy Management Modes

| Mode | Active Generation | Battery State | Primary Role & Automated Triggers |
|---|---|---|---|
| **Mode 1: Silent Watchkeeping (Eco)** | Solar Panels (`≈ 640 kW`†) · Nuclear & H2 OFF | `RECHARGE / FLOAT` | Zero-fuel parked mode. Maintains life support, event controllers, radio beacons, and float-charges batteries indefinitely. |
| **Mode 2: Tractive Cruise** | 1× Small Reactor (15 MW) + Solar | `AUTO` (Peak Shaving) | Overland cruising. Reactor powers 10× 5×5 wheel drives (`≈ 8-12 MW`†); batteries absorb acceleration spikes. |
| **Mode 3: Heavy Excavation** | 2× Small Reactors (30 MW) + Solar | `AUTO` (Burst Assist) | Full mining operations. Powers 45 m laser boom (4.0 MW beam + joint torques), outrigger voxel locks, Basic Refinery (330 kW), and Assembler (560 kW). |
| **Mode 4: Emergency Failover** | 1× H2 Engine (5.0 MW) · Nuclear OFF | `DISCHARGE` | Auto-triggered by `MAS EC Power` when uranium mass $< 1.0\text{ kg}$. Sheds refinery/assembler loads; preserves mobility and C2 life support. |
| **Mode 5: Shore Power / Fast Charge** | External Depot Grid via Roof or Stern | `RECHARGE` (100% Rate) | Rapid depot battery recharging and H2 bunkering when docked to Ground Base or Caterpillar train. |

---

#### 0.7 Master Consist Bill of Materials (BOM) & Mass Budget

| System Subassembly | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Tractor Chassis & Hull** | Heavy armor full blocks, half-blocks, slopes (Palette 0) | 440 | 374.0 t | 440 |
| **Tractor Powertrain** | 6× 5×5 Heavy Suspension Wheels (Axles 1-3) | 6 | 48.0 t | 600 |
| **Corner Outriggers** | 4× Large Pistons + 4× Seismic Surveying Feet (Palette 1 · Mod 2819263781) | 8 | 18.4 t | 260 |
| **Tractor Power Core** | 2× Small Nuclear Reactors, 1× Hydrogen Engine, 3× Large Batteries (Palette 6) | 6 | 31.6 t | 730 |
| **Articulated Boom & Crane** | 2× Adv Rotors, 3× Motor Hinges, 2× Lasers, 1× Crane Grapple, 1× Stow Clamp, 1× 90° Tool Elbow, Girders, Spotlights (Palette 3) | 23 | 39.6 t | 1,020 |
| **Armored Bridge & C2** | 3× Control Stations, 2× PBs, Laser Antenna, Glass, Ore Detector, 1× Scuttle Warhead, 1× Timer, 2× Button Panels | 23 | 16.2 t | 470 |
| **Drawbar Hitch & 3-Axis Gimbal** | 2× Adv Rotors (Yaw/Roll), 1× Motor Hinge (Pitch), 1× Merge Block (Hitch Sever Tongue), 2× Mag Clamps, 1× Mag Capture Plate, Guide Horns (Palette 2) | 11 | 23.0 t | 635 |
| **Utility Carriage Hull & Ballast** | Heavy armor frames, wheel wells, deck armor, 40× Sub-Floor Ballast Skids (Palette 0) | 400 | 426.0 t | 400 |
| **Utility Powertrain & DPU** | 4× 5×5 Heavy Suspension Wheels (Axles 4-5) with DPU traction | 4 | 32.0 t | 400 |
| **Utility Sub-Floor Battery Vault** | 3× Large Batteries (9.0 MWh, Palette 6) | 3 | 13.5 t | 120 |
| **Interior Walkways, Decoys, Lockers & Decon Airlocks** | Grated catwalks, 4× Hermetic Dual-Door Airlocks, sliding doors, ladder runs, interior & UV decon lights, 3× Armored Decoys, 1× Dorsal Lightning Mast, 2× Keel Recovery Gyros, 2× Armory Lockers, 1× Survival Locker, 3× Airlock ECs (Palette 9) | 43 | 23.7 t | 645 |
| **Field Workshop & Universal Life Support** | 1× Basic Refinery, 1× Large Assembler, 1× Medical Room, 2× Cryo Pods, 1× Survival Kit, 2× Large O2 Tanks, 1× Quarantined Ice Hold, 2× Large H2 Tanks, 2× O2/H2 Gens, 6× Air Vents, 1× Master Projector, 1× Scuttle Warhead | 22 | 68.2 t | 815 |
| **Utility Cargo Hold & Logistics Feed** | 1× Large Cargo Container (Components), 2× Munitions Sorters (`MAS Sorter Ammo Fwd / Parts Aft`), Conveyor network | 18 | 29.0 t | 210 |
| **Flight Decks, Welders & Solar** | 1× Mule Pad, 2× Kestrel Pads, 2× Drone Welders, 4× Solar Panels, 4× Floodlights | 67 | 61.1 t | 970 |
| **Stern Stabilizers, Telescoping Dock & Dump** | 2× Large Outrigger Pistons + 2× Mag Feet (`MAS Outrigger Utility`), 1× Telescoping Dock Piston (`MAS Piston Stern Dock`), Stern Dump Connector & Sorter, Relocated Tailings Ejector | 7 | 14.5 t | 510 |
| **Utility FOB Autonomy & Deadman Brake** | 1× PB (`[MAS-U] PB Utility`), 1× Remote Control (`MAS-U Remote FOB`), 1× Event Controller (`MAS-U EC Breakaway`), 2× Keel Mag Skids (`MAS Utility Brake Keel 1, 2`) | 5 | 4.0 t | 350 |
| **Perimeter Defense Suite** | 2× Gatling Turrets, 2× Interior Turrets, 1× Sub-Floor Bunker (`MAS Hold Munitions`), 1× Ammo EC (`MAS EC Ammo`), 1× Siren (`MAS Alarm Meteor`), Conveyor Feeds (Palette 7) | 15 | 10.7 t | 443 |
| **Multitool Repair Turrets** | 4× Large Laser Multitool Turrets (Bow L5, Port/Stbd L4 shoulders, Stern L7 arch · Mod 3175774286) | 4 | 18.0 t | 900 |
| **Peer Resupply Export Staging** | 1× Small Cargo (`MAS Hold Resupply Export`), 1× Conveyor Sorter (`MAS Sorter Resupply Export`), 9× Gantry Arch Heavy Armor blocks (Palette 0/3) | 11 | 3.0 t | 37 |
| **External EVA, Lights & Failsafes** | 2× Ground EVA Lockers, 3× Keel Lights, 1× Black Box SOS Beacon & Isolated Battery, 4× In-Situ Button Panels (Palette 9) | 11 | 2.5 t | 110 |
| **TOTAL CONSIST (DRY)** | Complete Expeditionary Mining Locomotive & Mobile FOB | **1,126** | **≈ 1,151.8 t** | **10,015** |

---

#### 0.8 Master Player Circulation, Access Corridors & Jefferies Tubes Schematic

```
[LONGITUDINAL PROFILE · CREW CIRCULATION & MAINTENANCE PATHS (LOOKING STARBOARD)]

     ◄────────── CARRIAGE 1: TRACTOR / EXCAVATOR ──────────►     ◄────────── CARRIAGE 2: FOB / UTILITY ──────────►
     L6                                                         ┌──[KESTREL]──────[MULE PAD CATWALK]──────┐
     L5      ┌──[C2 BRIDGE]──────[BOOM PEDESTAL DECK]           │ (L6 Catwalks & Solar Array Access)      │
     L4      │ (3-Seat C2/Alarm) │ (Turret Service Hatch)       ├─────────────────────────────────────────┤
     L3 ┌────┴───────────────────┴─────────────────────────┐    │ [FIELD WORKSHOP & MEDICAL HABITAT]      │
        │ [PORT JEFFERIES TUBE] ──► [POWER CORE VAULT]      ╞════╡ (Refinery · Assembler · MedBay · Tanks) │
     L2 │ (Access to Buffer & Core) (Reactors / H2 Engine)  │    │ [BATTERY VAULT & SUB-FLOOR AMMO BUNKER] │
     L1 └─▲────────────────────────────────────────────────┘    └───────────────────────────────────────▲─┘
          │                                                       (Articulated Enclosed Gangway)         │
     [CREW BOARDING LADDER]                                            (Z -9 to -8, L3 Level)      [STERN DUMP LADDER]
     (Port Flank L2 Hinge)                                                                         (Aft Bumper L2-L3)
```

```
Plan view, Y = L3 (Tractor & Utility Pressurized Walkways & Jefferies Tubes)
        X: −3           X: −2            X0              X: +2           X: +3
    Z: +12 ┌───────────────┬───────────────────────────────┬───────────────┐
           │ [C2 BRIDGE]   │ [STATION 1]   [STATION 2]     │ [STATION 3]   │  ◄── Forward Bridge Deck
    Z: +10 │ [ARMORY BRG]  │ [SURVIVAL KIT][SUIT CHARGER]  │ [LOCKER GEAR] │  ◄── Emergency Armory & Medical
    Z:  +8 ├───────────────┴───────────────┬───────────────┴───────────────┤
           │ [AIRLOCK INNER DOOR]          │ [BOOM LADDER / METEOR SIREN]  │  ◄── Vestibule & Siren (L4 Overhead)
    Z:  +7 │ [DECON CHAMBER (UV / VENT)]   │ [SOLID ARMOR]                 │
    Z:  +6 ├───────────────┬───────────────┴───────────────┬───────────────┤
           │ [AIRLOCK OUTER│
           │  DOOR & LADDER│
    Z:  +3 │ [PORT JEFF-   │ [MAS TRACTOR BUFFER (3×3×3)]  │ [SOLID ARMOR] │  ◄── Ore Buffer Pass
           │  ERIES TUBE]  │ (Manual Port on Port Flank)   │               │
    Z:  +1 │ (Catwalk &    │                               │               │
           │  Lighting)    │                               │               │
           ├───────────────┼───────────────┬───────────────┼───────────────┤
    Z:  −2 │ [VAULT DOOR]  │ [BAT 1]       │ [REACTOR 1]   │ [BAT 2]       │
    Z:  −3 │               │ [BAT 3]       │ [REACTOR 2]   │ [ARMOR]       │  ◄── Power Core Vault
    Z:  −4 │               │ [ARMOR]       │ [H2 ENGINE]   │ [ARMOR]       │
    Z:  −7 ├───────────────┴───────────────┴───────────────┴───────────────┤
    Z:  −8 │ ══════════════ [GANGWAY FORWARD SLIDING DOOR] ═══════════════ │  ◄── Aft Bulkhead
           ╞═══════════════════════════════════════════════════════════════╡
           │  [INTER-CARRIAGE ARTICULATED ENCLOSED GANGWAY (Z −9 to −8)]   │  ◄── Flexible Walkway (UV Wash)
           ╞═══════════════════════════════════════════════════════════════╡
    Z:  −9 │ ══════════════ [GANGWAY AFT SLIDING DOOR] ═══════════════════ │  ◄── Utility Fwd Bulkhead
           ├───────────────┬───────────────┬───────────────┬───────────────┤
    Z: −10 │ [SOLID ARMOR] │               │ [FLIGHT LADDER UP TO L6]      │  ◄── Stair/Ladder to L6
           ├───────────────┤ [CENTRAL      ├───────────────┬───────────────┤
    Z: −12 │ [REFINERY]    │  WORKSHOP     │ [MEDICAL BAY] │ [CRYO POD 1]  │  ◄── Field Metallurgy
    Z: −13 │ [ARMORY WRK]  │  AISLE &      │ [SUIT CHARGE] │ [CRYO POD 2]  │  ◄── Workshop Armory & Recharger
    Z: −14 │ [ASSEMBLER]   │  RADIATION    │ [O2/H2 GEN 1] │ [O2/H2 GEN 2] │  ◄── Fabrication & O2 Reserves
    Z: −15 │ [EC AMMO]     │  CITADEL]     │ [AMMO BUNKER (L2 UNDER)]      │  ◄── Sub-Floor Ammo Hold & Watchdog
    Z: −18 │ [H2 TANK 1]   │               │ [H2 TANK 2]   │ [ARMOR]       │  ◄── Fuel Bunkering & Storm Shield
    Z: −22 │ [ARMOR]       │ [MAS HOLD COMPONENTS (3×3×3)] │ [ARMOR]       │  ◄── Parts Hold
    Z: −25 ├───────────────┴───────────────┬───────────────┴───────────────┤
    Z: −26 │                               │ [STERN DUAL-DOOR AIRLOCK L2]  │  ◄── Stern Dump Egress & Decon
           └───────────────────────────────┴───────────────────────────────┘
```

---

#### 0.9 MASTODON-02 (MKII) Warfare 3 DLC Upgrade Architecture Roadmap

The current **MASTODON-01 MKI** design establishes a combat-capable survival baseline using vanilla and DLC blocks ahead of the upcoming **Warfare 3 DLC** release. Upon DLC launch, the consist will transition to the **MASTODON-02 MKII** blueprint, engineered with the following pre-allocated physical and power provisions:

| Subsystem Domain | MKI Pre-Warfare 3 Baseline | Planned MKII Warfare 3 Evolution | Pre-Allocated Sockets & Headroom |
|---|---|---|---|
| **Primary Dorsal Armament** | 2× Large Gatling Turrets (`MAS Turret Bow / Stern`) | Warfare 3 Heavy Kinetic / Directed-Energy Turrets | Centerline L5 dorsal hardpoints plumbed to large conveyor junctions. |
| **Close-In Anti-Personnel** | 2× Interior Turrets (`MAS Turret Flank Port/Stbd`) | Warfare 3 Point-Defense / Anti-Missile Systems | Flank L2 skirt sockets with 360° low-depression coverage. |
| **Electronic Warfare (EW)** | Passive laser antenna + 50 km radio beacon | Warfare 3 Active ECM / Jamming / Radar Intercept | Dorsal L6 comms mast socket pre-wired for EW transceivers. |
| **Power Grid Headroom** | 35 MW generation + 18 MWh battery reserve | Absorbs 40+ MW burst draws from heavy weapon systems | 72.0 MW peak instantaneous battery discharge (Mode 3/4). |
| **Armor Fairing Modularity** | Heavy armor composite hull with sloped glacis | Warfare 3 Modular Composite / Reactive Armor Plating | Wheel arch skirts and hitch drawbar pre-spaced for add-on armor tiles. |

---

### Phase 10-1 · Tractor Chassis & 6-Wheel Powertrain (Levels L1-L2)

```
       [PHASE 10-1 MASTER SPATIAL LOCATOR HUD]
       Envelope: 11 W × 20 L × 2 H B (27.5 × 50.0 × 5.0 m)
       Coordinate Range: X −5 to +5, Z −8 to +12, Y = L1 to L2
       Key Datums:
       · Tractor Centerline: X0, spanning Z −8 to +12
       · Longitudinal Box Girders: X = ±3
       · Axle 1 (Bow Steer): Z = +9 (Steer Angle: +30.0°)
       · Axle 2 (Mid Steer): Z = +4 (Steer Angle: +15.0°)
       · Axle 3 (Aft Drive): Z = −1 (Locked Drive: 0.0°)
       · Outrigger Piston Sockets: [X ±5, Z +11] and [X ±5, Z −6]
```

#### Pre-Step Master Parts Tray: Phase 10-1
* 240× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 60× Heavy Armor Half-Slope Block (Palette 0)
* 20× Heavy Armor Half Block (Palette 0)
* 6× Wheel Suspension 5×5 (3 Port, 3 Starboard)
* 16× Large Conveyor Pipe (Palette 4: Processing Teal)
* 5× Large Conveyor Junction (Palette 4: Processing Teal)
* 1× Large Conveyor Sorter (Palette 4: Processing Teal)
* 2× Conveyor Ejector / Connector (Palette 4: Processing Teal)
* 2× Curved Conveyor Tube (Palette 4: Processing Teal)

---

#### Step 10.1 · Twin Longitudinal Chassis Box Girders & Transverse Ties (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Chassis Backbone  ·  Grid Zone: [X ±3, Z −8 to +12]     │
│ [VIEWPORT] Level: L2 (Deck)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►   │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.1 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 40 | Slot 0 (`#2B303A`) | 34.0 t / 40 | 2× parallel 20 B longitudinal runs at X = ±3, Z −8 to +12 |
| Heavy Armor Full Block | 15 | Slot 0 (`#2B303A`) | 12.8 t / 15 | Transverse cross-ties (5 B each) at Z = +9, +4, −1 |
| Heavy Armor Full Block | 10 | Slot 0 (`#2B303A`) | 8.5 t / 10 | Bow Bulkhead cap (Z +12) and Aft Bulkhead cap (Z −8) |

```
Plan view, Y = L2 (Tractor Chassis Box Girder Deck)
        X: −3           X0           X: +3
    Z: +12 ■════════════╤════════════■  BOW BULKHEAD (Z +12 Datum)
           ║            │            ║  Central Conveyor Trough (X0)
    Z:  +9 ║  BEAM TIE  │  BEAM TIE  ║  ◄── Axle 1 Cross-Tie (X = ±3, Z +9)
           ║════════════╪════════════║
    Z:  +4 ║  BEAM TIE  │  BEAM TIE  ║  ◄── Axle 2 Cross-Tie (X = ±3, Z +4)
           ║════════════╪════════════║
    Z:  −1 ║  BEAM TIE  │  BEAM TIE  ║  ◄── Axle 3 Cross-Tie (X = ±3, Z −1)
           ║            │            ║
    Z:  −8 ■════════════╧════════════■  AFT HITCH BULKHEAD (Z −8 Datum)

    Legend:
    ■ Heavy Armor Corner / Bulkhead Node
    ║ Heavy Armor Full Block (Longitudinal Box Girder at X = ±3)
    ═ Heavy Armor Full Block (Transverse Axle Cross-Tie)
    │ Centerline Open Raceway (X0, 5-block internal gap)
```

##### Placement Instructions
1. Lay two parallel 20-block runs of Heavy Armor Full Blocks at X = −3 (Port) and X = +3 (Starboard) from Z −8 to Z +12 at Level L2.
2. Weld transverse 5-block Heavy Armor ties bridging between X −3 and X +3 at three axle datums: Z = +9 (Axle 1), Z = +4 (Axle 2), and Z = −1 (Axle 3).
3. Cap the forward end at Z = +12 with a solid transverse bulkhead tie (X −3 to +3).
4. Cap the aft end at Z = −8 with a solid transverse bulkhead tie (X −3 to +3) to anchor the future drawbar hitch.
5. Paint all structural armor blocks with Palette Slot 0 (`#2B303A` Industrial Dark Gray).

**✓ Check:** Two rigid parallel box girders at X = ±3 running exactly 20 blocks (50.0 m) from Z −8 to Z +12, with an open 5-block wide central trough along X0 bridged by 3 heavy cross-ties.

---

#### Step 10.2 · Underside Half-Slopes & V-Bevel Belly Skid Channel (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Belly Deflector  ·  Grid Zone: [X −2 to +2, Z −8 to +12] │
│ [VIEWPORT] Level: L1 (Underside)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT · STBD ►        │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.2 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Half-Slope | 40 | Slot 0 (`#2B303A`) | 17.0 t / 40 | Slopes placed at X = ±1, Z −8 to +12, facing inward toward X0 |
| Heavy Armor Half Block | 20 | Slot 0 (`#2B303A`) | 8.5 t / 20 | Centerline flush belly skid plates along X0, Z −8 to +12 |
| Heavy Armor Slope 2×1 | 12 | Slot 0 (`#2B303A`) | 7.2 t / 12 | Bow deflector bevels at Z = +11 to +12 |

```
Front Elevation Cross-Section, Y = L1-L2 (Belly Skid Channel Geometry)
    Port [−X]                                                          Starboard [+X]
    X: −3             X: −1            X0             X: +1             X: +3
    ┌───────────────┐ ┌───────────────────────────────────────────────┐ ┌───────────────┐
    │ L2 BOX GIRDER │ │            LEVEL L2 SUB-FLOOR DECK            │ │ L2 BOX GIRDER │
    └───────┬───────┘ └───────┬───────────────────────────────┬───────┘ └───────┬───────┘
            │                 │   [CENTRAL CONVEYOR PIPE]     │                 │
    Level L1│                 ▼             X0                ▼                 │Level L1
            │         ┌───────────────┐               ┌───────────────┐         │
            │         │   HALF-SLOPE  │   HALF-BLOCK  │   HALF-SLOPE  │         │
            │         │  (Slopes In)  │  (Skid Plate) │  (Slopes In)  │         │
            └─────────┴───────▲───────┴───────▲───────┴───────▲───────┴─────────┘
                              └───────────────┴───────────────┘
                                   1.25 m Belly Ground Clearance
    ═════════════════════════════════════════════════════════════════════════════ Terrain L0
```

##### Placement Instructions
1. Position Heavy Armor Half-Slope blocks along X = −1 from Z −8 to Z +12 at Level L1, angled so the sloped face bevels inward toward centerline X0.
2. Mirror on the starboard flank: place Heavy Armor Half-Slopes along X = +1 from Z −8 to Z +12 at Level L1, sloping inward toward X0.
3. Install continuous Heavy Armor Half Blocks along centerline X0 between the slopes, forming a flat skid keel 1.25 m above ground.
4. At the bow (Z = +11 to +12), install Heavy Armor Slope 2×1 blocks angled upward (+Y) to guide the belly smoothly over boulders during forward travel.

**✓ Check:** Underside forms an inverted, continuous trapezoidal trough along X0, providing 1.25 m (0.5 block) clearance to terrain with 45° beveled deflector flanks.

---

#### Step 10.3 · Central Ore Conveyor Raceway & Manifold Junctions (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Primary Ore Trunk  ·  Grid Zone: [X0, Z −8 to +12]              │
│ [VIEWPORT] Level: L1  ·  Orientation: Looking Port (−X) Longitudinal Elevation        │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.3 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Conveyor Junction | 5 | Slot 4 (`#1ABC9C`) | 7.5 t / 30 | Taps at Z +12 (Bow), Z +9 (Axle 1), Z +4 (Axle 2), Z 0 (Tailings), Z −1 (Axle 3), Z −8 (Hitch) |
| Large Conveyor Pipe | 15 | Slot 4 (`#1ABC9C`) | 11.2 t / 15 | Straight pipe runs connecting junctions along X0 at Level L1 |

```
Longitudinal Elevation, Looking Port (Centerline X0, Level L1 Conveyor Trunk)
    [BOW]                                                                                             [STERN]
    Z: +12          Z: +9          Z: +4             Z: 0          Z: −1                           Z: −8
      ■═══════════════■══════════════■════════════════■══════════════■═══════════════════════════════■
      │               │              │                │              │                               │
    [BOW INTAKE]  [AXLE 1 TAP]   [AXLE 2 TAP]   [TAILINGS TAP]   [AXLE 3 TAP]                    [HITCH TAP]
    (From Boom)   (Upward Riser) (Upward Riser) (Downward Flow)  (Upward to Power Core)          (To Drawbar)

    Legend:
    ■ Large Conveyor Junction (6-way connection hub, Palette 4: Processing Teal)
    ═ Large Conveyor Pipe (Straight 2.5 m run along X0)
```

##### Placement Instructions
1. Place a Large Conveyor Junction on centerline [X0, Z +12, Y = L1] facing forward (+Z) for future bow boom connection.
2. Route Large Conveyor Pipes along X0 aft to Z = +9; install Large Conveyor Junction at [X0, Z +9, Y = L1] with top port facing upward (+Y).
3. Continue pipes aft to Z = +4; install Large Conveyor Junction at [X0, Z +4, Y = L1] with top port facing upward (+Y).
4. Continue pipes aft to Z = 0; install Large Conveyor Junction at [X0, Z 0, Y = L1] with bottom port facing downward (−Y) for tailings ejectors.
5. Continue pipes aft to Z = −1; install Large Conveyor Junction at [X0, Z −1, Y = L1] with top port facing upward (+Y) for power vault fuel lines.
6. Run continuous pipes aft to Z = −8; terminate with Large Conveyor Junction at [X0, Z −8, Y = L1] with aft port facing −Z for the drawbar hitch.
7. Paint all conveyor junctions and pipes with Palette Slot 4 (`#1ABC9C` Processing Teal).

**✓ Check:** Unbroken, continuous large conveyor trunk from [X0, Z +12, L1] to [X0, Z −8, L1]. Upward ports clear at Z +9, +4, −1; downward port clear at Z 0.

---

#### Step 10.4 · 5×5 Off-Road Suspension Wheel Mounts & Master 3-Axle Geometry (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor 6-Wheel Powertrain  ·  Grid Zone: [X ±4 to ±5, Z −1..+9]│
│ [VIEWPORT] Level: L1  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►          │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.4 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Wheel Suspension 5×5 Left | 3 | Slot 0 (`#2B303A`) | 24.0 t / 300 | Mount on Port outer girder face at X = −4 (Z +9, +4, −1), hub outward (−X) |
| Wheel Suspension 5×5 Right | 3 | Slot 0 (`#2B303A`) | 24.0 t / 300 | Mount on Stbd outer girder face at X = +4 (Z +9, +4, −1), hub outward (+X) |

```
Plan view, Y = L1 (Tractor Wheel Suspension Layout)
        X: −5             X: −4          X0          X: +4             X: +5
    Z:  +9 [WH 1 (P)] ──── [GIRDER] ═════════════════ [GIRDER] ──── [WH 2 (S)]   ◄── Axle 1 (+30.0° Steer)
    Z:  +4 [WH 3 (P)] ──── [GIRDER] ═════════════════ [GIRDER] ──── [WH 4 (S)]   ◄── Axle 2 (+15.0° Steer)
    Z:  −1 [WH 5 (P)] ──── [GIRDER] ═════════════════ [GIRDER] ──── [WH 6 (S)]   ◄── Axle 3 (0.0° Locked Drive)

    Legend:
    [WH 1..6] 5×5 Heavy Suspension Wheel Unit (Hub at X ±5, Tire extends X ±4.5 to ±6.5)
    [GIRDER]  Heavy Armor Box Girder side wall (X = ±4 mounting face)
```

##### Placement Instructions
1. Position Wheel Suspension 5×5 Left units on the outer face of the Port girder (X = −4) at Z = +9 (`WH 1`), Z = +4 (`WH 3`), and Z = −1 (`WH 5`). Align the top mounting bracket flush with Level L2.
2. Mirror on the starboard girder face (X = +4): install Wheel Suspension 5×5 Right units at Z = +9 (`WH 2`), Z = +4 (`WH 4`), and Z = −1 (`WH 6`).
3. Ensure suspension stators orient with steering pivot pins strictly vertical (Y-axis) and wheel hub axles horizontal.
4. Verify tire bounding boxes extend from X −4.5 to −6.5 (Port) and X +4.5 to +6.5 (Starboard), leaving 5.0 blocks clearance along the chassis flank.

##### Terminal Configuration & Sliders
* Terminal Group: `MAS Tractor Wheels` (Select all 6 wheel blocks).
* Terminal Subgroup: `MAS Wheels Steer` (`WH 1`, `WH 2`, `WH 3`, `WH 4`).
* Terminal Subgroup: `MAS Wheels Drive` (`WH 1` through `WH 6`).

| Terminal Name | Axle Index | Z-Datum | Max Steer Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction | Height Offset |
|---|---|---|---|---|---|---|---|---|
| `MAS Wheel A1-P` | Axle 1 | Z = +9 | `30.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A1-S` | Axle 1 | Z = +9 | `30.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A2-P` | Axle 2 | Z = +4 | `15.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A2-S` | Axle 2 | Z = +4 | `15.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A3-P` | Axle 3 | Z = −1 | `0.0°` (Locked) | OFF | `100%` | `55% / 90%` | `75%` | `−32 cm` |
| `MAS Wheel A3-S` | Axle 3 | Z = −1 | `0.0°` (Locked) | OFF | `100%` | `55% / 90%` | `75%` | `−32 cm` |

**✓ Check:** All 6 wheel hubs rotate freely; steering test confirms Axle 1 turns 30°, Axle 2 turns 15°, and Axle 3 remains locked rigid at 0°. Chassis rests at 1.25 m belly clearance.

---

#### Step 10.5 · Tailings Aggregate Sorter & Underbelly Roadbed Ejectors (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tailings Venting Manifold  ·  Grid Zone: [X0, Z0 to −1]         │
│ [VIEWPORT] Level: L1  ·  Orientation: Underside looking Bow (+Z)                      │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.5 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Conveyor Sorter | 1 | Slot 4 (`#1ABC9C`) | 2.5 t / 15 | Plumbed under junction at [X0, Z0, Y = L1], flow arrow pointing DOWN (−Y) |
| Conveyor Ejector / Connector | 2 | Slot 4 (`#1ABC9C`) | 0.8 t / 20 | Angled 45° aft (−Z) beneath Axle 3 at [X ±1, Z −1, Y = L1] |
| Curved Conveyor Tube | 2 | Slot 4 (`#1ABC9C`) | 0.6 t / 6 | Directing sorter outlet horizontally to twin ejectors |

```
Underbelly Profile · Tailings Aggregate Spreader (Z0, L1)
     Central Conveyor Raceway Junction [X0, Z0, L1]
                          │
                          ▼ (Flow Arrow DOWN)
               [MAS Sorter Tailings]
                          │
                 ┌────────┴────────┐
                 ▼                 ▼
          [Curved Tube]      [Curved Tube]
                 │                 │
                 ▼                 ▼
       [MAS Ejector 1 (P)]   [MAS Ejector 2 (S)]  ◄── Angled 45° Aft (−Z)
                 │                 │
                 ▼                 ▼ (Vents Stone Tailings Behind Axle 3)
     ══════════════════════════════════════════════════ Crushed Stone Roadbed
```

##### Placement Instructions
1. Connect the top large port of Large Conveyor Sorter `MAS Sorter Tailings` to the downward port of junction [X0, Z0, Y = L1]. Ensure the directional arrow points downward (−Y).
2. Attach a conveyor splitter or Curved Conveyor Tubes to the sorter outlet, routing lines to [X −1, Z −1, L1] and [X +1, Z −1, L1].
3. Install twin Conveyor Ejectors `MAS Ejector Tailings 1` and `MAS Ejector Tailings 2` angled 45° aft (−Z), discharging crushed stone directly behind Axle 3's tire contact patch.
4. Paint sorter and ejectors with Palette Slot 4 (`#1ABC9C` Processing Teal).

##### Terminal Configuration & Sliders
* Block: `MAS Sorter Tailings`
  * Whitelist: `Stone`, `Gravel`
  * Drain All: `ON`
* Blocks: `MAS Ejector Tailings 1, 2`
  * Throw Out: `ON`
  * Collect All: `ON`
* Event Controller: `MAS EC Tailings`
  * Monitored Target: `MAS Tractor Buffer`
  * Condition: Cargo Fill $\ge 50\%$
  * Action Slot 1 (True): `MAS Sorter Tailings` -> Drain All ON; `MAS Ejectors Tailings` -> Power ON
  * Action Slot 2 (False): `MAS Sorter Tailings` -> Drain All OFF; `MAS Ejectors Tailings` -> Power OFF

##### Operational Clearance Note (Consist vs Standalone Mode)
When operating with Carriage 2 hitched, venting stone aggregate directly beneath Axle 3 (Z −1) causes loose stone chunks to collide with Carriage 2 trailing wheels (Axles 4 & 5 at Z −13 and Z −22). In consist operations, the active roadbed spreader is routed to the stern bumper of Carriage 2 ([X0, Z −26, Y = L1], Step 10.15E). The underbelly ejectors at [X ±1, Z −1] serve as standalone cleanout ports when the Tractor operates decoupled from the trailer.

**✓ Check:** Sorter whitelist passes only Stone and Gravel; high-value ores (Fe, Ni, Co, U) remain in the central spine. In consist travel, underbelly ejectors remain toggled OFF, routing crushed stone aft to the Carriage 2 stern spreader.

---

##### Pre-Enclosure Inspection Gate 10.1A: Tractor Powertrain & Belly Clearance Check
* [ ] Verify uninterrupted large conveyor pipe flow from Bow Throat (Z +12) to Aft Hitch (Z −8).
* [ ] Verify `MAS Sorter Tailings` drains stone and gravel without leaking metal ores.
* [ ] Verify exactly 1.25 m (0.5 B) ground clearance under belly skids at 50% suspension travel.
* [ ] Confirm steering response in `MAS Tractor Wheels`: Axles 1 and 2 pivot smoothly with zero wheel well binding.

```
PHASE 10-1 VISUAL MILESTONE · TRACTOR CHASSIS & POWERTRAIN COMPLETE
       Z: −1 (Axle 3)         Z: +4 (Axle 2)         Z: +9 (Axle 1)
           [WH 5]                 [WH 3]                 [WH 1]
     ═══════╧══════════════════════╧══════════════════════╧═══════
                 20 B CONTINUOUS HEAVY ARMOR BOX GIRDER RUN (L2)
```

---

### Phase 10-2 · Tractor Power Core, Ore Buffer & 2-Axis Drawbar Hitch (Levels L2-L4)

```
       [PHASE 10-2 MASTER SPATIAL LOCATOR HUD]
       Envelope: 7 W × 14 L × 3 H B (17.5 × 35.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z −8 to +6, Y = L2 to L4
       Subsystem Datums:
       · Ore Buffer Hold: Z +1 to +3, centered at X0
       · Primary Power Core: Z −4 to −2, centered at X0
       · 2-Axis Drawbar Hitch Center: [X0, Z −8, Y = L2]
       · Port Maintenance Crawlway: X −2, Z −7 to +6, Y = L3
```

#### Pre-Step Master Parts Tray: Phase 10-2
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 2× Small Nuclear Reactor (15 MW each, Palette 6: High-Voltage Red)
* 1× Hydrogen Engine (5 MW, 1×1×2 B, Palette 6: High-Voltage Red)
* 3× Large Battery (3 MWh each, 9.0 MWh total, Palette 6: High-Voltage Red)
* 1× Large Advanced Rotor (Palette 2: Heavy Rail Crimson)
* 1× Large Motor Hinge (Palette 2: Heavy Rail Crimson)
* 1× Large Merge Block (`MAS Hitch Sever Tongue`, Palette 2)
* 14× Grated Catwalk Block (Palette 9: High-Viz Safety Yellow)
* 3× Sliding Door (Palette 9: High-Viz Safety Yellow)
* 2× Small Cargo Container (`MAS Locker Port/Stbd`, Palette 9)
* 3× Decoy Block (Palette 0: Industrial Dark Gray)
* 2× Large Gyroscope (`MAS Gyro Recovery 1, 2`, Palette 0)
* 1× Radio Antenna / Beacon (`MAS Beacon Distress`, Palette 7)
* 1× Small Battery (`MAS Battery BlackBox`, Palette 6)
* 1× Large Warhead (`MAS Warhead Scuttle Power`, Palette 6)
* 1× Sci-Fi 4-Button Panel (`MAS BP Power Vault`, Palette 9)
* 6× Large Conveyor Pipe & 4× Curved Conveyor Tube (Palette 4)
* 120× Heavy Armor Full Block & Slopes (Palette 0 & Palette 6)

---

#### Step 10.6 · Tractor Ore Buffer Hold (3×3×3 Large Cargo) & Intake Riser (L2-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Ore Buffer  ·  Grid Zone: [X −1..+1, Z +1..+3]          │
│ [VIEWPORT] Level: L2 to L4  ·  Orientation: Plan view Y = L3 (▲ Bow +Z)              │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.6 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Cargo Container | 1 | Slot 5 (`#27AE60`) | 18.0 t / 50 | Centered at [X0, Z +2, L3], spanning X −1 to +1, Z +1 to +3, Y L2 to L4 |
| Large Conveyor Pipe | 2 | Slot 4 (`#1ABC9C`) | 1.5 t / 2 | Vertical riser from L1 junction up to cargo bottom port |
| Heavy Armor Full Block | 16 | Slot 0 (`#2B303A`) | 13.6 t / 16 | Structural foundation saddle wrapping bottom edges at L2 |

```
Plan view, Y = L3 (Tractor Ore Buffer Hold & Jefferies Crawlway)
        X: −2             X: −1             X0             X: +1             X: +2
    Z: +3 ┌─────────────┐ ┌───────────────────────────────────────────────┐ ┌─────────────┐
          │ JEFFERIES   │ │                                               │ │ SOLID       │
    Z: +2 │ CRAWLWAY    │ │        MAS TRACTOR BUFFER (3×3×3 B)           │ │ ARMOR       │
          │ [CATWALK]   │ │         (Palette Slot 5: Logistics Emerald)   │ │ BULKHEAD    │
    Z: +1 │ [HATCH] ◄───┼─┤ (Manual Port)                                 │ │             │
          └─────────────┘ └───────────────────────────────────────────────┘ └─────────────┘

    Legend:
    [MAS TRACTOR BUFFER] 3×3×3 B Large Cargo Container (Receives boom ore)
    [HATCH]              Manual inventory access port facing Port Jefferies tube [X −2, Z +2, L3]
    [CATWALK]            Grated Catwalk Block along maintenance crawlway
```

##### Placement Instructions
1. Position 1 Large Cargo Container centered at [X0, Z +2, Y = L3], extending from X −1 to +1, Z +1 to +3, and vertically from Level L2 to L4.
2. Align the bottom 3×3 large conveyor port with the vertical conveyor riser from junction [X0, Z +4, L1].
3. Ensure the top 3×3 large conveyor port aligns at Level L4 for the future dorsal boom turret riser.
4. Align the Port face so its built-in conveyor/inventory access door faces into the Port Jefferies tube at [X −2, Z +2, L3].
5. Plumb the aft large conveyor port along X0 to feed the power core vault.
6. Paint the container exterior with Palette Slot 5 (`#27AE60` Logistics Emerald).

##### Terminal Configuration & Sliders
* Name: `MAS Tractor Buffer`
* Custom Data: `[Inventory] Buffer=True; Role=RawOreBuffer; MaxTonnage=1000`
* Show in Terminal: `ON` · Show on HUD: `ON`

**✓ Check:** Large Cargo Container securely seated between box girders. Bottom port plumbed to L1 raceway; top port ready at L4; Port inspection hatch accessible from crawlway.

---

#### Step 10.7 · Primary Nuclear Power Core Vault & Electrical Shielding (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Baseload Nuclear Power  ·  Grid Zone: [X −2..+2, Z −4..−2]│
│ [VIEWPORT] Level: L3  ·  Orientation: Plan view Y = L3 (▲ Bow +Z)                    │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.7 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Small Nuclear Reactor | 2 | Slot 6 (`#C0392B`) | 5.2 t / 50 | Mounted inline at [X0, Z −2, L3] and [X0, Z −3, L3], ports along X0 |
| Large Battery | 3 | Slot 6 (`#C0392B`) | 13.5 t / 120 | Mounted at [X −2, Z −2], [X +2, Z −2], and [X −2, Z −3] |
| Heavy Armor Full Block | 24 | Slot 6 (`#C0392B`) | 20.4 t / 24 | Double-walled radiation containment bulkheads surrounding vault |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | Vault maintenance entrance door at [X −2, Z −2, L3] |

```
Plan view, Y = L3 (Tractor Power Core Vault)
        X: −2             X0              X: +2
    Z: −2  ┌───────────────┬───────────────┬───────────────┐
           │ [MAS BAT 1]   │ [MAS RCT 1]   │ [MAS BAT 2]   │  ◄── Reactor 1 (15 MW) flanked by Batteries
    Z: −3  │ [MAS BAT 3]   │ [MAS RCT 2]   │ [HEAVY ARMOR] │  ◄── Reactor 2 (15 MW) flanked by Battery 3
    Z: −4  │ [HEAVY ARMOR] │ [MAS ENG H2]  │ [HEAVY ARMOR] │  ◄── Hydrogen Engine (5 MW Standby Failover)
           └───────────────┴───────────────┴───────────────┘
           Nuclear + H2 Baseload: 35.0 MW · Tractor Battery Reserve: 9.0 MWh (36.0 MW Peak)
```

##### Placement Instructions
1. Place Small Nuclear Reactor 1 (`MAS Reactor 1`) at [X0, Z −2, Y = L3] with conveyor ports aligned along centerline X0.
2. Place Small Nuclear Reactor 2 (`MAS Reactor 2`) directly aft at [X0, Z −3, Y = L3], connected in series with Reactor 1.
3. Place Large Battery 1 (`MAS Battery 1`) at [X −2, Z −2, L3] and Large Battery 2 (`MAS Battery 2`) at [X +2, Z −2, L3].
4. Place Large Battery 3 (`MAS Battery 3`) at [X −2, Z −3, L3].
5. Plumb the reactor conveyor line forward into the aft port of `MAS Tractor Buffer` for automated Uranium fuel feeding.
6. Install Sliding Door `MAS Door Vault` at [X −2, Z −2, L3] opening into the Port Jefferies crawlway.
7. Encase the entire vault perimeter in Heavy Armor blocks painted Palette Slot 6 (`#C0392B` High-Voltage Red).

##### Terminal Configuration & Sliders
* Group: `MAS Reactors` (`MAS Reactor 1`, `MAS Reactor 2`)
  * Role: 30.0 MW continuous baseload nuclear generation
* Group: `MAS Batteries Tractor` (`MAS Battery 1`, `MAS Battery 2`, `MAS Battery 3`)
  * Initial State: `Auto` (Peak Shaving) · Output: 36.0 MW peak discharge
* Door: `MAS Door Vault` (Closed by default)

**✓ Check:** 30 MW nuclear baseload + 9 MWh battery buffer verified in control terminal. Uranium supply line connected; vault door provides walk-in access from crawlway.

---

#### Step 10.8 · Standby Emergency Hydrogen Engine Installation (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tier 3 Emergency Generator  ·  Grid Zone: [X0, Z −4]            │
│ [VIEWPORT] Level: L3  ·  Orientation: Looking Port (−X) Longitudinal Elevation        │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.8 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Hydrogen Engine (Large Grid) | 1 | Slot 6 (`#C0392B`) | 4.8 t / 25 | 1×1×2 B footprint centered at [X0, Z −4, L3] extending aft to Z −5 |
| Large Conveyor Pipe | 1 | Slot 4 (`#1ABC9C`) | 0.8 t / 1 | Connects engine front conveyor port to reactor spine along X0 |

```
Longitudinal Elevation, Looking Port (Power Vault Centerline X0, Level L3)
    [FORWARD]                                                           [AFT]
    Z: +1              Z: −2             Z: −3             Z: −4 to −5
    ┌───────────────┐  ┌───────────────┐  ┌───────────────┐  ┌───────────────────────────────┐
    │  MAS TRACTOR  │  │  MAS REACTOR  │  │  MAS REACTOR  │  │    MAS ENGINE H2 (5.0 MW)     │
    │  BUFFER       │══╡       1       │══╡       2       │══╡ (1×1×2 B Hydrogen Generator)  │
    │  (3×3×3 Hold) │  │  (15.0 MW)    │  │  (15.0 MW)    │  │ [Sump Port]    [Manual Valve] │
    └───────────────┘  └───────────────┘  └───────────────┘  └───────────────────────────────┘
```

##### Placement Instructions
1. Place 1 Large Grid Hydrogen Engine (`MAS Engine H2`, 1×1×2 B) at [X0, Z −4, Y = L3], extending aft to Z −5.
2. Connect its forward conveyor port directly to the aft conveyor port of `MAS Reactor 2`.
3. Route the aft conveyor port toward the drawbar hitch conduit at Z −8 to draw fuel from Carriage 2's 10,000,000 L hydrogen reserve.
4. Verify the manual fuel shutoff valve and crank sump port face the interior aisle for manual field maintenance.
5. Paint engine with Palette Slot 6 (`#C0392B` High-Voltage Alert Red).

##### Terminal Configuration & Sliders
* Name: `MAS Engine H2`
* Enabled: `OFF` (Cold-standby reserve)
* Supervised by: Event Controller `MAS EC Power`
  * Monitored: `MAS Reactor 1, 2` Uranium Inventory
  * Threshold: $< 1.0\text{ kg}$
  * Action: Turn `MAS Engine H2` ON; sound fuel alert chime

**✓ Check:** Hydrogen engine securely mounted behind Reactor 2; plumbed to central conveyor spine; verified 5.0 MW failover output when toggled ON.

---

#### Step 10.9 · Port Maintenance Jefferies Tube & Physical Buffer Access Hatch (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Crew Maintenance Crawlway  ·  Grid Zone: [X −2..−3, Z −7..+6]   │
│ [VIEWPORT] Level: L3  ·  Orientation: Plan view Y = L3 looking down (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.9 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Grated Catwalk Block | 14 | Slot 9 (`#F1C40F`) | 4.2 t / 14 | Floor tiles along X −2, Z −7 to +6 |
| Interior Light | 4 | Slot 9 (`#F1C40F`) | 0.2 t / 12 | Amber bulkhead lamps mounted along crawlway ceiling at L4 |
| Sci-Fi 4-Button Panel | 1 | Slot 9 (`#F1C40F`) | 0.1 t / 5 | Mounted on vault outer bulkhead at [X −2, Z −1, L3] |
| Heavy Armor Full Block | 28 | Slot 0 (`#2B303A`) | 23.8 t / 28 | Outer armored hull skirt running along X = −3 |

```
Plan view, Y = L3 (Port Jefferies Tube Maintenance Corridor)
        X: −3 (Outer Skirt)       X: −2 (Walkway)           X: −1 (Inner Machinery)
    Z:  +6 ┌─────────────────────┬─────────────────────────┬─────────────────────────────┐
           │ HEAVY ARMOR SKIRT   │ [FORWARD AIRLOCK VEST]  │ TO C2 BRIDGE CABIN          │
    Z:  +2 │ HEAVY ARMOR SKIRT   │ [CATWALK] ──► [HATCH]   │ MAS TRACTOR BUFFER (3×3×3)  │
    Z:  −1 │ HEAVY ARMOR SKIRT   │ [MAS BP POWER VAULT]    │ MAS DOOR VAULT (Sliding)    │
    Z:  −3 │ HEAVY ARMOR SKIRT   │ [CATWALK]               │ POWER CORE VAULT BULKHEAD   │
    Z:  −7 │ HEAVY ARMOR SKIRT   │ [AFT GANGWAY AIRLOCK]   │ TO ACCORDION GANGWAY        │
           └─────────────────────┴─────────────────────────┴─────────────────────────────┘
```

##### Placement Instructions
1. Lay Grated Catwalk Blocks along X = −2 from Z +6 (Airlock Vestibule) aft to Z −7 (Gangway Bulkhead) at Level L3.
2. Build the outer heavy armor hull skirt at X = −3 enclosing the walkway.
3. Ensure the physical cargo maintenance port on the Port face of `MAS Tractor Buffer` at [X −2, Z +2, L3] remains completely unobstructed.
4. Mount Sci-Fi 4-Button Panel `MAS BP Power Vault` on the vault bulkhead at [X −2, Z −1, L3].
5. Install 4 amber Interior Lights in the crawlway ceiling spaced 3 blocks apart.

##### Terminal Configuration & Sliders
* Panel: `MAS BP Power Vault`
  * Button 1: Sliding Door `MAS Door Vault` -> Open/Close
  * Button 2: Group `MAS Reactors` -> Power ON/OFF (Nuclear SCRAM)
  * Button 3: Generator `MAS Engine H2` -> Power ON/OFF (Cold-Start)
  * Button 4: Group `MAS Batteries Tractor` -> Recharge ON/OFF

**✓ Check:** Unobstructed 1-block wide pressurized crawlway. Cargo hatch opens for manual inventory access; button panel toggles vault door and power systems.

---

#### Step 10.10 · Heavy Motorized 2-Axis Drawbar Hitch & Pyrotechnic Severance Merge Block (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 3-Axis Gimbal Drawbar Hitch  ·  Grid Zone: [X0, Z −8 to −9.5]   │
│ [VIEWPORT] Level: L2  ·  Orientation: Profile looking Port & Plan view (▲ Bow +Z)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.10 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Advanced Rotor | 2 | Slot 2 (`#9E2A2B`) | 17.6 t / 30 | `MAS Hitch Yaw` [X0, Z −8, L2] facing −Z; `MAS Hitch Roll` [X0, Z −9, L2] inline on Z-axis |
| Large Motor Hinge | 1 | Slot 2 (`#9E2A2B`) | 3.5 t / 15 | `MAS Hitch Pitch` [X0, Z −8.5, L2] horizontal pin for vertical pitch |
| Merge Block (Large Grid) | 1 | Slot 2 (`#9E2A2B`) | 1.2 t / 15 | `MAS Hitch Sever Tongue` mounted on drawbar tongue at Z −9.5 facing Aft (−Z) |
| Large Magnetic Plate | 3 | Slot 2 (`#9E2A2B`) | 4.2 t / 90 | 2× `MAS Hitch Clamp Port/Stbd` [X ±2, Z −8.5, L2]; 1× `MAS Hitch Capture Plate` on trailer nose |
| Camera | 1 | Slot 7 (`#1B263B`) | 0.2 t / 10 | `MAS Cam Hitch` facing aft with cockpit guidance crosshairs |
| Heavy Armor Full Block & Slopes | 8 | Slot 2 (`#9E2A2B`) | 6.8 t / 8 | Alignment guide horns and structural wishbone tongue frame |

```
2.5D Isometric Callout · 3-Axis Articulated Gimbal Tow Hitch & Hard-Lock
    Tractor Aft Bulkhead [X0, Z −8, L2]
           ║
           ╠══[ADVANCED ROTOR: MAS Hitch Yaw] ──► Horizontal Steering Articulation (±45.0°)
           ║        │ (Aft-Facing Large Port)
           ║        ▼
           ╠══[MOTOR HINGE: MAS Hitch Pitch] ────► Vertical Terrain Grade Pitch (±25.0°)
           ║        │ (Horizontal Pin)
           ║        ▼
           ╠══[ADVANCED ROTOR: MAS Hitch Roll] ──► Longitudinal Torsional Roll (±20.0°)
           ║        │ (Z-Axis Rotational Pass-Thru)
           ║        ▼
           ╠══[MERGE BLOCK: MAS Hitch Sever Tongue] ──► Structural Hard-Lock & Emergency Decouple
           ║        │
           ║        ▼
           ╚════════╪════════════════════════════► To Utility Carriage (Carriage 2)
                    ▼
           [LARGE CONVEYOR PASS-THROUGH PIPE]

    Anti-Oscillation High-Speed Clamps:
    [MAS Hitch Clamp Port (X −2)] ────► Locks onto Trailer Strike Plate (> 30 km/h)
    [MAS Hitch Clamp Stbd (X +2)] ────► Locks onto Trailer Strike Plate (Rigid Transit)
```

##### Placement Instructions
1. Mount the stator of Large Advanced Rotor `MAS Hitch Yaw` on the Tractor Aft Bulkhead at [X0, Z −8, Y = L2], facing directly Aft (−Z). Connect its front port to the central conveyor raceway.
2. Attach the Large Advanced Rotor Head. Connect the internal large conveyor passage.
3. Mount the stator of Large Motor Hinge `MAS Hitch Pitch` at [X0, Z −8.5, L2] directly onto the yaw rotor head with the hinge barrel oriented horizontally (X-axis) for vertical pitch.
4. Mount the stator of Large Advanced Rotor `MAS Hitch Roll` at [X0, Z −9, L2] on the pitch hinge head, aligned horizontally along the Z-axis to permit up to ±20.0° torsional roll rotation.
5. Build the drawbar tongue using Heavy Armor blocks and mount Large Grid Merge Block `MAS Hitch Sever Tongue` at [X0, Z −9.5, L2] facing Aft (−Z) to connect with Carriage 2.
6. Install 2× Large Magnetic Plates (`MAS Hitch Clamp Port/Stbd`) at [X −2, Z −8.5, L2] and [X +2, Z −8.5, L2] facing Aft to clamp onto trailer strike plates during high-speed travel.
7. Install camera `MAS Cam Hitch` above the drawbar facing Aft with HUD crosshairs for docking alignment.
8. Construct beveled Heavy Armor guide horns on Carriage 2's nose to funnel the tongue within a ±1.2 m alignment tolerance.
9. Paint all drawbar components with Palette Slot 2 (`#9E2A2B` Heavy Rail Crimson).

##### Terminal Configuration & Sliders
* Rotor: `MAS Hitch Yaw`
  * Velocity: `0.5 rpm` · Braking Torque: `50 MN·m`
  * Lower Limit: `−45.0°` · Upper Limit: `+45.0°` · Share Inertia Tensor: `ON`
* Hinge: `MAS Hitch Pitch`
  * Velocity: `0.5 rpm` · Braking Torque: `45 MN·m`
  * Lower Limit: `−25.0°` · Upper Limit: `+25.0°` · Share Inertia Tensor: `ON`
* Rotor: `MAS Hitch Roll`
  * Velocity: `0.5 rpm` · Braking Torque: `30 MN·m`
  * Lower Limit: `−20.0°` · Upper Limit: `+20.0°` · Share Inertia Tensor: `ON`
* Group: `MAS Hitch Clamps` (`MAS Hitch Clamp Port`, `MAS Hitch Clamp Stbd`)
  * Auto-Lock: `OFF` · Proximity Sensor Interlock: Active
* Merge Block: `MAS Hitch Sever Tongue`
  * Enabled: `ON` (Structural hard-lock)

**✓ Check:** Drawbar articulates freely in 3 axes (Yaw ±45°, Pitch ±25°, Roll ±20°) without chassis armor clipping. Clamps rigidly lock tongue straight when energized.

---

#### Step 10.11 · Smart Anti-Jackknife Lockout & Inter-Carriage Flexible Weather Bellows with Dual Hermetic Airlocks (L2-L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Anti-Jackknife Logic & Gangway  ·  Grid Zone: [X0, Z −9 to −8]  │
│ [VIEWPORT] Level: L2-L3  ·  Orientation: Looking Port (−X) Longitudinal Section       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.11 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sliding Door | 2 | Slot 9 (`#F1C40F`) | 2.4 t / 30 | `MAS Door Gangway Fwd` [X0, Z −8, L3] and `MAS Door Gangway Aft` [X0, Z −9, L3] |
| Grated Catwalk Block | 2 | Slot 9 (`#F1C40F`) | 0.6 t / 2 | Articulated gangway walkway plates spanning Z −8 to −9 |
| Heavy Armor Slope Block | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Bellows perimeter weather fairings surrounding gangway gap |

```
Longitudinal Section through Flexible Gangway (Looking Port, X0, Level L3)
    CARRIAGE 1 (TRACTOR)                                          CARRIAGE 2 (UTILITY)
    Aft Bulkhead (Z −8)                                           Fwd Bulkhead (Z −9)
    ┌──────────────────────┐                                      ┌──────────────────────┐
    │ [PORT JEFFERIES]     │                                      │ [FIELD WORKSHOP]     │
    │                      │                                      │                      │
    │ [MAS DOOR GANG FWD]  │ ═════════ [CATWALK] ════════════════ │ [MAS DOOR GANG AFT]  │
    └──────────┬───────────┘    (Flexible Weather Bellows)        └──────────┬───────────┘
    ═══════════╧═════════════════════════════════════════════════════════════╧═══════════ Level L2
               [3-AXIS GIMBAL DRAWBAR: YAW ROTOR + PITCH HINGE + ROLL ROTOR]
```

##### Placement Instructions
1. Install Tractor Aft Gangway door `MAS Door Gangway Fwd` at [X0, Z −8, Y = L3].
2. Install Utility Forward Gangway door `MAS Door Gangway Aft` at [X0, Z −9, Y = L3].
3. Bridge the 1-block mechanical gap with slip-resistant Grated Catwalks, enclosed within flexible heavy armor bellows.
4. **Hermetic Protocol:** In Space Engineers, subgrids cannot share an airtight seal across mechanical joints. The gangway enclosure serves as a dust/weather shield. In vacuum environments, crew members cycle through the dual sliding doors as an airlock vestibule, or traverse with sealed EVA helmets.
5. Configure Event Controllers `MAS EC Hitch Speed` and `MAS EC Hitch Rev`.

##### Terminal Configuration & Sliders
* Event Controller: `MAS EC Hitch Speed`
  * Monitored: Vehicle Speedometer
  * Threshold: $> 30.0\text{ km/h}$
  * Action Slot 1 (True): Group `MAS Hitch Clamps` -> Lock ON; `MAS Hitch Yaw` -> Braking Torque = `100 MN·m` (Rigid Anti-Sway Lock)
  * Action Slot 2 (False): Group `MAS Hitch Clamps` -> Lock OFF; `MAS Hitch Yaw` -> Braking Torque = `50 MN·m` (Active Flex Articulation)
* Event Controller: `MAS EC Hitch Rev`
  * Monitored: Transmission Gear
  * Condition: Gear = Reverse
  * Action Slot 1 (True): `MAS Hitch Yaw` -> Clamp Limits to `±15.0°`; Axle 5 -> Invert Steering OFF
  * Action Slot 2 (False): `MAS Hitch Yaw` -> Restore Limits to `±45.0°`; Axle 5 -> Invert Steering ON

**✓ Check:** Dual airlock doors cycle with active gas reclamation. Above 30 km/h, magnetic clamps energize, locking drawbar rigid and eliminating harmonic trailer snaking.

---

#### Step 10.12 · Ground-Level EVA Survival Lockers & Underbelly Keel Lights (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Ground EVA & Keel Lighting  ·  Grid Zone: [X ±5, Z −4] (Lockers) │
│ [VIEWPORT] Level: L1-L2  ·  Orientation: Looking Bow (+Z) Cross-Section at Z −4      │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.12 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Small Cargo Container | 2 | Slot 9 (`#F1C40F`) | 1.2 t / 20 | Recessed at [X −5, Z −4, L2] (Port) and [X +5, Z −4, L2] (Stbd) |
| Interior Light | 3 | Slot 9 (`#F1C40F`) | 0.2 t / 9 | Mounted along centerline keel at [X0, Z +5, L1], [X0, Z 0, L1], [X0, Z −5, L1] |

```
Front Elevation Cross-Section at Z −4 (Ground EVA Lockers & Keel Lighting)
    Port [−X]                                                          Starboard [+X]
    X: −5             X: −3            X0              X: +3            X: +5
    ┌───────────────┐ ┌──────────────┐ ┌─────────────┐ ┌──────────────┐ ┌───────────────┐
    │ MAS LOCKER P  │ │ BOX GIRDER   │ │ CONVEYOR    │ │ BOX GIRDER   │ │ MAS LOCKER S  │
    │ [EVA Supplies]│ │ (Heavy Armor)│ │ (Central)   │ │ (Heavy Armor)│ │ [EVA Supplies]│
    └───────┬───────┘ └──────┬───────┘ └──────┬──────┘ └──────┬───────┘ └───────┬───────┘
            │                │                │               │                 │
    Level L1│                │         [KEEL LIGHT]           │                 │Level L1
            │                │         (Amber LED ↓)          │                 │
    ════════╧════════════════╧════════════════╧═══════════════╧═════════════════╧════════ Ground L0
     ▲ Ground-Level Human Reach (1.8 m)                                          ▲
```

##### Placement Instructions
1. Recess Small Cargo Container `MAS Locker Port` flush into the lower Port armor skirt at [X −5, Z −4, Y = L2]. Plumb its conveyor port into the lower raceway.
2. Mirror on Starboard: install Small Cargo Container `MAS Locker Stbd` at [X +5, Z −4, Y = L2].
3. Mount 3 Interior Lights `MAS Lights Underbelly` facing downward along the chassis keel at [X0, Z +5, L1], [X0, Z 0, L1], and [X0, Z −5, L1].
4. Paint lockers with Palette Slot 9 (`#F1C40F` High-Viz Safety Yellow).

##### Terminal Configuration & Sliders
* Blocks: `MAS Locker Port`, `MAS Locker Stbd`
  * Stocking: 4× O2 Bottles, 4× H2 Bottles, 2× Welder Tools, 500× NATO 25×184mm ammo
* Group: `MAS Lights Underbelly`
  * Color: Amber (`R: 255, G: 180, B: 50`) · Radius: `5.0 m` · Intensity: `1.5`

**✓ Check:** Lockers can be opened by an engineer standing on planet terrain (Level L0) without jetpack; amber lights illuminate suspension hubs and tailings manifold.

---

#### Step 10.13 · Native Threat Decoys, Keel Recovery Gyros & Armored Black Box SOS Cell (L1-L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Failsafes & Asset Protection  ·  Grid Zone: [X0, Z0 to +7]      │
│ [VIEWPORT] Level: L1-L3  ·  Orientation: Plan view Y = L1 (▲ Bow +Z)                  │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.13 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Decoy Block | 3 | Slot 0 (`#2B303A`) | 3.6 t / 60 | Enclosed in lower armor skirts: [X −5, Z −4, L2], [X +5, Z −4, L2], [X0, Z −26, L2] |
| Large Gyroscope | 2 | Slot 0 (`#2B303A`) | 24.0 t / 100 | Mounted in lower chassis keel at [X0, Z 0, Y = L1] |
| Radio Antenna / Beacon | 1 | Slot 7 (`#1B263B`) | 0.8 t / 15 | `MAS Beacon Distress` encased in heavy armor at [X0, Z +7, L1] |
| Small Battery | 1 | Slot 6 (`#C0392B`) | 0.5 t / 10 | `MAS Battery BlackBox` (cold isolated standby) at [X0, Z +7, L1] |
| Large Warhead | 1 | Slot 6 (`#C0392B`) | 1.6 t / 20 | `MAS Warhead Scuttle Power` in power vault at [X0, Z −3, L3] |

```
Plan view, Y = L1 (Keel Failsafes & SOS Black Box Cell)
        X: −5              X0              X: +5
    Z:  +7 ┌───────────────┬───────────────┬───────────────┐
           │               │[BLACK BOX CELL│               │  ◄── Armored SOS Beacon + Isolated Battery
           │               │ 50 km Distress│               │
    Z:   0 │               │[MAS GYRO 1, 2]│               │  ◄── Self-Righting Roll Override Gyroscopes
           │               │(Recovery Core)│               │
    Z:  −4 │[DECOY PORT]   │               │[DECOY STBD]   │  ◄── Ballistic Decoy Skirts (Divert Fire)
           └───────────────┴───────────────┴───────────────┘
```

##### Placement Instructions
1. Install Ballistic Decoys `MAS Decoy Flank Port` at [X −5, Z −4, L2] and `MAS Decoy Flank Stbd` at [X +5, Z −4, L2], encased within heavy armor skirt blocks.
2. Install 2 Large Gyroscopes `MAS Gyro Recovery 1` and `MAS Gyro Recovery 2` in the lower keel at [X0, Z 0, L1].
3. Construct a 6-sided heavy armor capsule at [X0, Z +7, L1] housing Radio Beacon `MAS Beacon Distress` (50 km) and isolated Small Battery `MAS Battery BlackBox`.
4. Install Large Grid Warhead `MAS Warhead Scuttle Power` between the reactors at [X0, Z −3, L3] (disarmed by default).

##### Terminal Configuration & Sliders
* Gyroscopes: `MAS Gyro Recovery 1, 2`
  * Override: `OFF` (Normal transit) · Roll Override: `+100%` (Ready for righting toggle)
* Beacon & Battery: `MAS Beacon Distress` & `MAS Battery BlackBox`
  * Battery: `Recharge = OFF` · `Auto = OFF` (Isolated cold standby)
* Event Controller: `MAS EC BlackBox`
  * Monitored: Main Grid Power Output
  * Condition: Total MW $= 0.0\text{ MW}$ (Total blackout)
  * Action: Activate `MAS Battery BlackBox` -> Broadcast `SOS-MASTODON-01-LOST` (50 km)

**✓ Check:** Hostile turrets target decoys away from bridge and reactors; isolated black box fires distress ping upon power cut; recovery gyros right vehicle from 90° lateral rollover.

---

##### Pre-Enclosure Inspection Gate 10.2A: Power Core Isolation, Jefferies Tube & Gangway Test
* [ ] Verify reactor automated uranium pull through the central conveyor trunk.
* [ ] Confirm `MAS Engine H2` cold-starts and delivers 5.0 MW upon manual toggle or simulated uranium depletion.
* [ ] Confirm total tractor power capacity reads `71.0 MW` peak (30 MW nuclear + 5 MW H2 engine + 36 MW battery discharge from 3× Large Batteries).
* [ ] Verify Port Jefferies tube allows unobstructed human walking from Bridge to Aft Bulkhead.
* [ ] Confirm physical maintenance port on `MAS Tractor Buffer` opens and allows manual inventory access.
* [ ] Confirm articulated inter-carriage gangway maintains airtight seal and flexes cleanly with hitch movement.
* [ ] Test 2-axis drawbar hitch: ensure rotor and hinge pivot freely without clashing with the chassis frame.
* [ ] Verify `MAS Hitch Sever Tongue` merge block physically separates consist carriages upon `Hitch Decouple` command.
* [ ] Confirm `MAS Decoy Flank Port/Stbd` draw hostile turret lock away from the bridge and reactors.
* [ ] Confirm `MAS Gyro Recovery 1, 2` successfully right consist from a 90° lateral rollover test.
* [ ] Test `MAS Beacon Distress` and `MAS Battery BlackBox`: confirm automated 50 km SOS ping triggers upon simulated total blackout.
* [ ] Verify `MAS EC Hitch Speed` dynamically raises rotor braking torque to 100 MN·m when speed exceeds 35 km/h.
* [ ] Verify `MAS Locker Port/Stbd` are accessible from ground level (L0) and `MAS Lights Underbelly` illuminate suspension hubs.

```
PHASE 10-2 VISUAL MILESTONE · POWER CORE, BUFFER & DRAWBAR HITCH COMPLETE
       Z: −8 (Drawbar)        Z: −4 (Power Vault)    Z: +2 (Ore Buffer)
      [HITCH SEVER TONGUE]    [35 MW TIER 1 CORE]    [3×3×3 BUFFER HOLD]
             ║                         │                      │
       ══════╩═════════════════════════╧══════════════════════╧═══════
                    PORT JEFFERIES CRAWLWAY RUN (Z −7 to +6, L3)
```

### Phase 10-3 · Hydraulic Bedrock Outriggers & 45 m Long-Reach Excavation Boom (Levels L1-L8)

```
       [PHASE 10-3 MASTER SPATIAL LOCATOR HUD]
       Envelope: 15 W × 22 L × 6 H B (37.5 × 55.0 × 15.0 m operating envelope)
       Coordinate Range: X −7 to +7, Z +5 to +22, Y = L1 to L8
       Pedestal Datum: [X0, Z +6, Y = L4] (Dorsal Turret Ring)
       Travel Stow Clamp Datum: [X0, Z −7, Y = L4] (Dorsal Spine Trough)
       Outrigger Locations:
       · Port Forward Outrigger: [X −5, Z +11, Y = L1]
       · Starboard Forward Outrigger: [X +5, Z +11, Y = L1]
       · Port Aft Outrigger: [X −5, Z −6, Y = L1]
       · Starboard Aft Outrigger: [X +5, Z −6, Y = L1]
```

#### Pre-Step Master Parts Tray: Phase 10-3
* 4× Large Piston (Palette 1: Hazard Amber)
* 4× Seismic Surveying Landing Gear (`MAS Seismic Outrigger 1..4`, TMS Mod 2819263781, Palette 1)
* 1× Large Advanced Rotor (`MAS Turret Azimuth`, Palette 3: Excavator Ochre)
* 1× Large Motor Hinge (`MAS Shoulder Pitch`, Palette 3: Excavator Ochre)
* 1× Large Motor Hinge (`MAS Elbow Pitch`, Palette 3: Excavator Ochre)
* 1× Large Conveyor Elbow (90° Downward Tool Head Adapter, Palette 4: Processing Teal)
* 1× Large Motor Hinge (`MAS Wrist Pitch`, Palette 3: Excavator Ochre)
* 1× Large Advanced Rotor (`MAS Wrist Roll`, Palette 3: Excavator Ochre)
* 2× Simple Laser Multitool (`MAS Laser 1, 2`, Palette 3: Excavator Ochre)
* 1× Large Magnetic Plate (`MAS Tool Grapple`, Palette 9: High-Viz Safety Yellow)
* 1× Large Magnetic Plate (`MAS Clamp Boom Stow`, Palette 1: Hazard Amber · Travel Cradle at Z −7)
* 1× Camera · `MAS Cam Boom` (High-magnification targeting crosshairs)
* 2× Spotlight · `MAS Light Boom 1, 2` (Palette 3)
* 1× Sensor · `MAS Sensor Boom` (4.0 m proximity stop)
* 12× Large Conveyor Pipe (Palette 4: Processing Teal)
* 24× Heavy Armor Slope & Box Girder Blocks (Palette 3)

---

#### Step 10.14 · 4× Corner Bedrock Hydraulic Outriggers & Seismic Landing Gear (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bedrock Outriggers & Seismic Prospecting  ·  Level: L1-L2       │
│ [VIEWPORT] Grid Zone: [X ±5, Z +11] & [X ±5, Z −6]  ·  Looking Bow (+Z) Front Section│
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.14 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Piston | 4 | Slot 1 (`#E6A100`) | 12.8 t / 100 | Mounted vertically at [X ±5, Z +11] and [X ±5, Z −6], shaft pointing DOWN (−Y) |
| Seismic Surveying Landing Gear | 4 | Slot 1 (`#E6A100`) | 5.6 t / 160 | `MAS Seismic Outrigger 1..4` (Mod 2819263781) attached to piston heads |
| Heavy Armor Full Block | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Reinforced outrigger corner corner socket ties |

```
Front Cross-Section at Axle 1 (Outriggers Deployed to Bedrock Baseline)
     [OUTRIGGER PORT: PISTON 1]                                  [OUTRIGGER STBD: PISTON 2]
     [X −5, Z +11, L2]                                           [X +5, Z +11, L2]
            │                                                           │
            ▼ (Shaft Extends Down)                                      ▼ (Shaft Extends Down)
     ┌──────────────┐                                            ┌──────────────┐
     │ LARGE PISTON │                                            │ LARGE PISTON │
     └──────┬───────┘ ─── [WH 1] ─── [BELLY SKIDS] ─── [WH 2] ─── └──────┬───────┘
            │                                                           │
            ▼                                                           ▼
     [SEISMIC FOOT P]                                            [SEISMIC FOOT S]
     (Acoustic Pulse)                                            (Acoustic Pulse)
    ════════╧═══════════════════════════════════════════════════════════╧════════ Terrain Baseline
```

##### Placement Instructions
1. Position 4 downward-facing Large Pistons in reinforced corner sockets at:
   * Port Forward: [X −5, Z +11, Y = L2] (`MAS Outrigger Piston 1`)
   * Starboard Forward: [X +5, Z +11, Y = L2] (`MAS Outrigger Piston 2`)
   * Port Aft: [X −5, Z −6, Y = L2] (`MAS Outrigger Piston 3`)
   * Starboard Aft: [X +5, Z −6, Y = L2] (`MAS Outrigger Piston 4`)
2. Attach Seismic Surveying Landing Gear (`MAS Seismic Outrigger 1..4`, Mod 2819263781) to each piston head facing downward toward planet voxels.
3. Paint pistons and landing gear with Palette Slot 1 (`#E6A100` Hazard Amber).

##### Terminal Configuration & Sliders
* Group: `MAS Outriggers` (Pistons 1..4)
  * Velocity: `0.2 m/s` · Max Distance: `2.5 m` (1 block stroke) · Share Inertia Tensor: `ON`
* Group: `MAS Outrigger Pads` (`MAS Seismic Outrigger 1..4`)
  * Auto-Lock: `ON`
* Event Controller: `MAS EC Level` (Auto-Leveling)
  * Monitored: `MAS Helm` Artificial Horizon Pitch and Roll
  * Condition: Deck Pitch/Roll $> 0.5^\circ$ -> Differential extension to achieve $0.0^\circ$ Level
* Event Controller: `MAS EC Seismic`
  * Monitored: `MAS Outrigger Pads` Lock State
  * Condition: All 4 Pads = Locked -> Trigger 500 m Acoustic Pulse -> Broadcast `TMS.SCOUT.VEIN`

**✓ Check:** All 4 outrigger pistons extend smoothly; magnetic landing gear lock green upon voxel contact; acoustic survey pulse triggers upon 4/4 lock.

---

#### Step 10.15 · Centerline Turret Pedestal & Azimuth Advanced Rotor (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Dorsal Boom Turntable Pedestal  ·  Grid Zone: [X0, Z +6, Y = L4]│
│ [VIEWPORT] Level: L4  ·  Orientation: Plan view Y = L4 looking down (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.15 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Advanced Rotor | 1 | Slot 3 (`#E67E22`) | 8.8 t / 15 | Stator mounted at [X0, Z +6, L4] facing UP (+Y), large port aligned to L3 riser |
| Heavy Armor Slope Block | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Collar ring reinforcement wrapping the rotor stator |
| Large Conveyor Pipe | 1 | Slot 4 (`#1ABC9C`) | 0.8 t / 1 | Vertical riser connecting top of `MAS Tractor Buffer` at L3 to stator |

```
Plan view, Y = L4 (Turret Pedestal Turntable Deck)
        X: −2             X0              X: +2
    Z:  +7 ┌───────────────┬───────────────┬───────────────┐
           │ HEAVY ARMOR   │ C2 BRIDGE AFT │ HEAVY ARMOR   │  ◄── Bridge Aft Bulkhead
    Z:  +6 ├───────────────┼───────────────┼───────────────┤
           │ SLOPE RING    │[ADV ROTOR: MAS│ SLOPE RING    │  ◄── 360° Azimuth Turntable Ring
           │ (Reinforced)  │ TURRET AZIMUTH│ (Reinforced)  │      (Palette Slot 3: Ochre)
    Z:  +5 ├───────────────┼───────────────┼───────────────┤
           │ SLOPE RING    │ DORSAL TROUGH │ SLOPE RING    │  ◄── Trough leading aft to stow clamp
           └───────────────┴───────────────┴───────────────┘
```

##### Placement Instructions
1. Position the stator of Large Advanced Rotor `MAS Turret Azimuth` at [X0, Z +6, Y = L4], facing directly UP (+Y).
2. Plumb the bottom 3×3 large conveyor port directly into the top port of `MAS Tractor Buffer` at Level L3.
3. Attach the Large Advanced Rotor Head facing UP (+Y).
4. Surround the stator base with 8 Heavy Armor Slope blocks forming a reinforced structural collar.
5. Paint the rotor with Palette Slot 3 (`#E67E22` Excavator Ochre).

##### Terminal Configuration & Sliders
* Name: `MAS Turret Azimuth`
* Velocity: `0.75 rpm` · Braking Torque: `50 MN·m`
* Rotor Limits: Lower Limit: `−180.0°` · Upper Limit: `+180.0°` (360° Continuous Pan)
* Share Inertia Tensor: `ON`
* Symmetrical Reach: Centerline mounting guarantees identical **21.25 m physical reach (8.5 blocks)** beyond outer tires on both Port and Starboard flanks.

**✓ Check:** Turntable stator seated rigidly behind bridge canopy. Head spins 360° with 50 MN·m braking torque; large conveyor conduit passes through rotor center.

---

#### Step 10.16 · Shoulder Pitch Hinge & 7-Block Main Boom Arm Casing (L4-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Shoulder Elevation & Main Arm  ·  Level: L4 to L6               │
│ [VIEWPORT] Grid Zone: Spanning 7 B from pedestal  ·  Looking Port (−X) Profile       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.16 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Motor Hinge | 1 | Slot 3 (`#E67E22`) | 3.5 t / 15 | Stator mounted on Rotor Head; hinge barrel horizontal along X-axis (Pitch) |
| Large Conveyor Pipe | 7 | Slot 4 (`#1ABC9C`) | 5.2 t / 7 | Continuous 7-block (17.5 m) internal conveyor pipe line |
| Heavy Armor Full Block / Slope | 14 | Slot 3 (`#E67E22`) | 11.9 t / 14 | Boxed structural armor casing enclosing the conveyor pipe |

```
Longitudinal Profile · Shoulder Joint & Main Boom Arm (Looking Port)
                                                               [ELBOW JOINT]
                                                               (To Step 10.17)
                                                                      ▲
                                                                     ╱
                                           [MAIN BOOM ARM · 7 B]    ╱
                                           (17.5 m Heavy Casing)   ╱
                                                                  ╱
                               [SHOULDER PITCH HINGE] ───────────┘
                               (Horizontal Pin along X-axis)
                                     │
                                     ▼
                        [TURRET AZIMUTH ROTOR HEAD]
                        [Chassis Pedestal: X0, Z +6, L4]
```

##### Placement Instructions
1. Mount the stator of Large Motor Hinge `MAS Shoulder Pitch` directly onto the top face of the Azimuth Rotor Head.
2. **Critical Orientation:** Orient the hinge stator so the barrel pivot pin is strictly horizontal along the X-axis (Port-Starboard), enabling vertical pitch elevation.
3. Attach the Large Motor Hinge Head.
4. Plumb 7 Large Conveyor Pipes in a straight run extending from the hinge head (17.5 m span).
5. Encase the pipes in Heavy Armor Full and Slope blocks, forming a boxed girder casing painted Palette Slot 3 (`#E67E22` Excavator Ochre).

##### Terminal Configuration & Sliders
* Name: `MAS Shoulder Pitch`
* Velocity: `0.5 rpm` · Braking Torque: `45 MN·m`
* Lower Limit: `−20.0°` (Undercutting terrain / spine stow) · Upper Limit: `+75.0°` (High reach)
* Share Inertia Tensor: `ON`

**✓ Check:** Shoulder elevates cleanly between −20° and +75°; 17.5 m main boom arm holds elevation under 45 MN·m torque with zero droop.

---

#### Step 10.17 · Elbow Pitch Hinge & 5-Block Forearm Girder (L6-L8)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Elbow Joint & Forearm Reach  ·  Level: L6 to L8                 │
│ [VIEWPORT] Grid Zone: Forearm extension (5 B span)  ·  Looking Port (−X) Profile    │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.17 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Motor Hinge | 1 | Slot 3 (`#E67E22`) | 3.5 t / 15 | Stator mounted at tip of main boom arm; hinge barrel horizontal (Pitch) |
| Large Conveyor Pipe | 5 | Slot 4 (`#1ABC9C`) | 3.7 t / 5 | Continuous 5-block (12.5 m) internal conveyor pipe line |
| Structural Girder / Slope | 10 | Slot 3 (`#E67E22`) | 6.5 t / 10 | Tapered lightweight girder casing enclosing forearm |

```
Longitudinal Profile · Elbow Joint & Forearm Girder (Looking Port)
                                             [FOREARM GIRDER · 5 B]
                                             (12.5 m Tapered Casing)
                                             ═══════════════════════► To Wrist (Step 10.18)
                                            ╱
                      [ELBOW PITCH HINGE] ─┘
                      (Horizontal Pin)
                             │
                             ▼
               [From Main Boom Arm (Step 10.16)]
```

##### Placement Instructions
1. Mount the stator of Large Motor Hinge `MAS Elbow Pitch` at the forward tip of the 7-block main boom arm.
2. **Critical Orientation:** Align the hinge barrel strictly horizontal along the X-axis (Pitch), parallel to the shoulder hinge.
3. Attach the Large Motor Hinge Head.
4. Plumb 5 Large Conveyor Pipes extending from the hinge head (12.5 m span).
5. Encase the piping in tapered Structural Girder and slope blocks painted Palette Slot 3 (`#E67E22` Excavator Ochre).

##### Terminal Configuration & Sliders
* Name: `MAS Elbow Pitch`
* Velocity: `0.5 rpm` · Braking Torque: `45 MN·m`
* Lower Limit: `−100.0°` (Descent into deep pits) · Upper Limit: `+45.0°` (Extended reach)
* Share Inertia Tensor: `ON`

**✓ Check:** Elbow articulates from −100° to +45°; combined main arm + forearm delivers 35.0 m physical reach from turret center without structural flex.

---

#### Step 10.18 · 90° Downward Tool Elbow, Wrist Pitch Hinge & Wrist Roll Rotor (L7-L8)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 2-Axis Wrist Articulator  ·  Level: L7-L8                       │
│ [VIEWPORT] Grid Zone: Forearm Tip  ·  Looking Port (−X) & Front Elevation            │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.18 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Conveyor Elbow | 1 | Slot 4 (`#1ABC9C`) | 0.8 t / 1 | 90° downward adapter mounted at forearm tip, outlet facing strictly DOWN (−Y) |
| Large Motor Hinge | 1 | Slot 3 (`#E67E22`) | 3.5 t / 15 | `MAS Wrist Pitch` stator mounted to elbow outlet, pin horizontal (Pitch) |
| Large Advanced Rotor | 1 | Slot 3 (`#E67E22`) | 8.8 t / 15 | `MAS Wrist Roll` stator mounted to hinge head, rotating face facing DOWN (−Y) |

```
Longitudinal Callout · 2-Axis Wrist Articulator (Forearm Tip)
              [Forearm Girder Tip]
                       │
                       ▼
             [90° DOWN CONVEYOR ELBOW] ──► Sets neutral reference to straight down (−Y)
                       │
                       ▼
             [MOTOR HINGE: MAS Wrist Pitch] ──► Pitch Compensation: −60° to +60°
                       │
                       ▼
             [ADV ROTOR: MAS Wrist Roll] ────► 360° Continuous Axial Spin (5 rpm Core Bore)
                       │
                       ▼
             [TOOL MOUNTING FLANGE] ─────────► (To Step 10.19)
```

##### Placement Instructions
1. Attach Large Conveyor Elbow to the tip of the forearm piping, orienting its outlet port facing strictly DOWN (−Y) toward planetary gravity.
2. Mount the stator of Large Motor Hinge `MAS Wrist Pitch` to the elbow outlet. Orient its barrel horizontal for pitch compensation.
3. Attach the hinge head. Mount the stator of Large Advanced Rotor `MAS Wrist Roll` to the hinge head with its rotor head facing downward (−Y).
4. Attach the Large Advanced Rotor Head. This head serves as the universal mounting plate for the tool head.
5. Paint wrist blocks with Palette Slot 3 (`#E67E22` Excavator Ochre).

##### Terminal Configuration & Sliders
* Hinge: `MAS Wrist Pitch`
  * Limits: `−60.0°` to `+60.0°` · Braking Torque: `20 MN·m` · Velocity: `1.0 rpm`
  * Share Inertia Tensor: `ON`
  * Active Gravity Horizon Leveling: Counter-pitches to offset arm angle ($\theta_{\text{wrist}} = -(\theta_{\text{shoulder}} + \theta_{\text{elbow}})$), locking tool vector strictly vertical ($[0, -1, 0]$).
* Rotor: `MAS Wrist Roll`
  * Rotor Limits: Lower Limit: OFF · Upper Limit: OFF (Continuous 360° Roll)
  * Velocity: `5.0 rpm` · Braking Torque: `20 MN·m` · Share Inertia Tensor: `ON`

**✓ Check:** Wrist counter-rotates to maintain strictly downward tool orientation regardless of arm elevation; roll rotor spins smoothly at 5.0 rpm.

---

#### Step 10.19 · Laser Mining Tool Head, Heavy Recovery Grapple & Travel Stow Cradle Clamp (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Excavation Tool Head & Chassis Travel Stow  ·  Level: L4        │
│ [VIEWPORT] Grid Zone: Tool Head at Wrist Roll Head; Stow Clamp at [X0, Z −7, L4]     │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.19 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Simple Laser Multitool | 2 | Slot 3 (`#E67E22`) | 6.0 t / 200 | `MAS Laser 1, 2` (ToolCore) mounted on roll face, spaced 2.5 m apart |
| Large Magnetic Plate | 1 | Slot 9 (`#F1C40F`) | 2.4 t / 25 | `MAS Tool Grapple` mounted centrally between lasers, face pointing DOWN (−Y) |
| Large Magnetic Plate | 1 | Slot 1 (`#E6A100`) | 2.4 t / 25 | `MAS Clamp Boom Stow` mounted flush in Tractor dorsal spine trough at [X0, Z −7, L4] |
| Camera | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Cam Boom` centered looking straight down drill bore |
| Spotlight | 2 | Slot 3 (`#E67E22`) | 0.4 t / 16 | `MAS Light Boom 1, 2` flanking tool head |
| Sensor | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Sensor Boom` (4.0 m forward collision safety buffer) |

```
Front Elevation · Tool Head Mounting Face (Looking Up at Roll Face)
                  ┌───────────────────────────────────────────────┐
                  │                 MAS Cam Boom                  │
                  ├───────────────┬───────────────┬───────────────┤
                  │  MAS Laser 1  │   MAS TOOL    │  MAS Laser 2  │
                  │ (Simple Laser │    GRAPPLE    │ (Simple Laser │
                  │  Multitool)   │  (Mag Plate)  │  Multitool)   │
                  ├───────────────┼───────────────┼───────────────┤
                  │ MAS Light 1   │MAS Sensor Boom│ MAS Light 2   │
                  └───────────────┴───────────────┴───────────────┘
```

##### Placement Instructions
1. Mount 2 Simple Laser Multitools (`MAS Laser 1` and `MAS Laser 2`) onto the outer flanks of the Wrist Roll Head, spaced 2.5 m apart with conveyor ports plumbed into the rotor pass-through.
2. Mount 1 Large Magnetic Plate `MAS Tool Grapple` centrally between the two laser emitters, facing outward along the tool head axis.
3. Install Camera `MAS Cam Boom` centered directly down the drill bore; flank with Spotlights `MAS Light Boom 1` and `MAS Light Boom 2`.
4. Install Sensor `MAS Sensor Boom` calibrated for a 4.0 m forward safety buffer.
5. On the Tractor chassis dorsal deck, install Large Magnetic Plate `MAS Clamp Boom Stow` flush in the spine trough at [X0, Z −7, Y = L4] facing UP (+Y), flanked by twin heavy armor guide coamings running from Z +5 to Z −8.

##### Terminal Configuration & Sliders
* Lasers: `MAS Laser 1, 2` (ToolCore Tri-Mode: Mining Drill <-> Salvage Grinder <-> Fabrication Welder)
* Grapple: `MAS Tool Grapple` (Auto-Lock: `OFF` · Mag Strength: `100%`)
* Stow Clamp: `MAS Clamp Boom Stow` (Auto-Lock: `ON` · Mag Strength: `100%`)
* Sensor: `MAS Sensor Boom` (Front: `4.0 m`, Left/Right: `2.0 m`, Up/Down: `2.0 m` -> Action: Cut Lasers & Clamp Joint Brakes)

**✓ Check:** Lasers fire in Mining, Salvage, and Fabrication modes; grapple locks onto test block; boom slews 180° aft into dorsal trough and locks green into `MAS Clamp Boom Stow` at 5.2 m transit height.

---

#### Step 10.20 · Automated Excavation Macro Modes & Kinematic Sweep Sequences

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Excavator Kinematic Macro Engine  ·  Software/Hardware Layer    │
│ [VIEWPORT] Supervised by: `[MAS] PB Excavator` (Update1 / 60 Hz)                     │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### 5 Standard Excavation & Recovery Macros
1. **Macro 1: Bench Quarrying (Horizontal Terracing):**
   * Azimuth Turret sweeps across face: `−45.0°` (Port) to `+45.0°` (Starboard).
   * Shoulder Hinge incrementally steps down `−2.5°` per pass (0.5 B cut depth).
   * Wrist Pitch elevates `+90.0°` to maintain flat, drivable terrace floors parallel to surface.
2. **Macro 2: Target-Centric Vertical Column Bore (Subterranean Shaft):**
   * Accepts target vein waypoint $(X_t, Z_t)$ from seismic scan or Station 2 operator.
   * Turret Azimuth slews to target bearing; Shoulder and Elbow extend tool head over target.
   * Wrist Pitch counter-pitches to lock laser beams strictly vertical ($[0, -1, 0]$).
   * Wrist Roll spins twin offset lasers at 5.0 rpm, cutting an initial 3.0 m core.
   * Motion controller translates arm in automated 2D concentric circular sweeps ($\Delta X, \Delta Z$) around $(X_t, Z_t)$ to expand shaft diameter (up to 12.5 m wide).
   * Steps downward $\Delta Y = -1.0\text{ m}$ per pass down to $-25.0\text{ m}$ physical depth ($-55\text{ m}$ with laser beams).
3. **Macro 3: Trench & Road Carve (Overland Right-of-Way):**
   * Boom deploys along centerline ($Z+$), executing downward scoop arc ($-5.0\text{ m}$ to grade).
   * Tractor crawls forward at 2.0 km/h; automatically grades a level 15.0 m wide roadbed for Caterpillar land trains.
4. **Macro 4: Crane & Vehicle Recovery (`MACRO_CRANE`):**
   * Lasers deactivated; `MAS Tool Grapple` energized.
   * Station 2 CTC mouse control provides direct 3-axis teleoperation up to 42.5 m away.
   * Utilizes 45 MN·m joint holding torque and hydraulic outriggers to right rolled vehicles or hoist heavy cargo modules.
5. **Macro 5: Wreck Deconstruction (`MACRO_SALVAGE`):**
   * Lasers switch to Mode: Salvage Beam (Grinder).
   * Positions tool head at 15 to 30 m standoff; executes automated planar raster sweeps ($\Delta Y, \Delta X$) across target bounding box, disassembling enemy hulls layer-by-layer with 100% component retention into `MAS Hold Components`.

##### Automated Travel Stow & Deploy Sequences
* **Travel Stow Sequence (`STOW`):**
  1. Mining/salvage lasers deactivate; tool head retracts vertically clear of worksite.
  2. Turret Azimuth slews to 180.0° (Aft centerline).
  3. Shoulder, Elbow, and Wrist hinges align flat (0.0°).
  4. Boom lowers into armored dorsal spine trough; `MAS Clamp Boom Stow` at [X0, Z −7, L4] engages (Green Lock).
  5. Joint holding brakes clamp at 100% torque (50 MN·m Azimuth, 45 MN·m Hinges).
  6. Outriggers retract; consist is cleared for 35 km/h overland transit.
* **Work Deploy Sequence (`DEPLOY`):**
  1. Consist comes to full stop; handbrake engages.
  2. Outriggers extend and lock bedrock (verified by `MAS EC Stabilizers`).
  3. `MAS Clamp Boom Stow` releases.
  4. Shoulder elevates +15.0° to clear spine coamings.
  5. Slew to target waypoint azimuth and begin commanded macro.

---

##### Pre-Enclosure Inspection Gate 10.3A: Outrigger Locking & Boom Joint Inertia Verification
* [ ] Verify all 4 corner outrigger seismic feet (`MAS Seismic Outrigger 1..4`) lock firmly into terrain voxels.
* [ ] Confirm `MAS EC Seismic` triggers acoustic survey pulse upon 4/4 bedrock lock, broadcasting vein telemetry to `TMS.SCOUT.VEIN`.
* [ ] Confirm `Share Inertia Tensor = ON` across all boom joints (Turret Rotor, Shoulder Hinge, Elbow Hinge, Wrist Pitch Hinge, Wrist Roll Rotor).
* [ ] Test 360° pan and verify symmetrical 21.25 m physical reach beyond both Port and Starboard flanks without chassis tip.
* [ ] Verify `MAS Clamp Boom Stow` locks green at [X0, Z −7, L4] and rigidly secures the 35 m stowed boom with zero subgrid jitter.
* [ ] Test Macro 2 `BORE`: verify tool head maintains true vertical downward aim ([0, -1, 0]) while executing 2D lateral concentric sweeps around offset target waypoint.
* [ ] Verify `MAS Tool Grapple` locks onto external test block and holds suspended 50 t mass without joint sagging.
* [ ] Test Macro 4 `CRANE_RECOVERY`: verify crane mode enables grapple teleoperation and disables mining laser fire.

```
PHASE 10-3 VISUAL MILESTONE · OUTRIGGERS ANCHORED & BOOM DEPLOYED
                             ▲ 45 m Elevation Reach
                           ╱   ╲
     [ARMORED BRIDGE]    ╱       ╲  [ELBOW HINGE]
           ║           ╱           ╲
           ╠══[TURRET AZIMUTH]       ╲
           ║                           ╲ ──► [LASER MINING HEAD]
     ══════╩═════════════════════════════╩═════════════════════════
     [OUTRIGGER PAD: LOCKED]           [OUTRIGGER PAD: LOCKED]
```

---

### Phase 10-4 · Forward Armored C2 Bridge & Fleet Avionics (Levels L3-L5)

```
       [PHASE 10-4 MASTER SPATIAL LOCATOR HUD]
       Envelope: 7 W × 6 L × 3 H B (17.5 × 15.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z +7 to +12, Y = L3 to L5
       Station Datums:
       · Station 1 · Helm Pilot: [X −2, Z +10, Y = L4] (Port)
       · Station 2 · Excavation Master: [X0, Z +10, Y = L4] (Centerline)
       · Station 3 · Tactical Fleet Director: [X +2, Z +10, Y = L4] (Starboard)
       · Deep Ore Detector: [X0, Z +8, Y = L3]
       · C2 Compute Rack: [X0, Z +7, Y = L3] (Dual PBs + AI Automaton blocks)
       · Comms Mast: [X −2, Z +7, Y = L6] (Laser Antenna + 50 km Radio)
       · Survival Kit: [X −2, Z +8, Y = L4]
```

#### Pre-Step Master Parts Tray: Phase 10-4
* 3× Control Station (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`, Palette 7: Fleet Command Navy)
* 2× Programmable Block (`[MAS] PB Excavator`, `[MAS] PB RIG//OS-Lite`, Palette 7)
* 3× AI Automaton Block (`MAS AI Move`, `MAS AI Task`, `MAS AI Defense`, Palette 7)
* 1× Laser Antenna (`MAS Comm Laser`, 40 km, Palette 7)
* 1× Radio Antenna (`MAS Comm Radio`, 50 km broadcast, Palette 7)
* 1× Large Ore Detector (150 m subterranean range, Palette 4: Processing Teal)
* 1× Survival Kit (Tractor Field Respawn & Med Station, Palette 7)
* 14× Sloped Heavy Armor Glass (45° Panoramic Canopy)
* 6× LCD Panel (Tactical Video Wall & Console HUDs, Palette 7)
* 1× Air Vent (`MAS Vent Bridge`, Hermetic Zone 1)
* 1× Large Warhead (`MAS Warhead Scuttle Bridge`, disarmed anti-capture charge)
* 1× Timer Block (`MAS Timer Scuttle`, 60-second scuttle loop)
* 6× Sci-Fi 4-Button Panel (`MAS BP Ground Boarding`, `MAS BP Airlock Vestibule`, etc.)
* 1× Motor Hinge (`MAS Crew Ladder Hinge`, Palette 9: High-Viz Safety Yellow)
* 2× Narrow Ladder Block (Palette 9)
* 1× Sensor (`MAS Sensor Ladder`, 2.5 m proximity trigger)
* 1× Large Gatling Turret (`MAS Turret Bow`, Palette 7: Fleet Command Navy)
* 2× Interior Turret (`MAS Turret Flank Port/Stbd`, Palette 7)

---

#### Step 10.21 · Forward Bridge Foundation Deck & Dual-Door Decon Boarding Airlock (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bridge Foundation & Port Decon Airlock  ·  Level: L3            │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z +7 to +12]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.21 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 24 | Slot 0 (`#2B303A`) | 20.4 t / 24 | Foundation floor deck across X −3 to +3, Z +7 to +12 at Level L3 |
| Grated Catwalk Block | 12 | Slot 9 (`#F1C40F`) | 3.6 t / 12 | Walkway aisle floor along X0 and Port entry lane |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Boarding Outer` at [X −3, Z +7, L2/L3], exterior boarding seal |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Boarding Inner` at [X −2, Z +7, L3], cabin interior seal |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Airlock Port` at [X −3, Z +7, L4], depressurization / gas reclamation |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Bridge` mounted in ceiling at [X0, Z +9, L4] (Zone 1 cabin pressure) |
| Interior Light | 1 | Slot 9 (`#F1C40F`) | 0.02 t / 25 | `MAS Light Decon Port` at [X −3, Z +7, L4] (Cyan `#00FFFF`, UV Decontamination) |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.1 t / 25 | `MAS EC Airlock Port` at [X −3, Z +8, L3], automated interlock sequencer |
| Survival Kit | 1 | Slot 7 (`#1B263B`) | 2.1 t / 25 | Mounted against Port aft bulkhead at [X −2, Z +8, L4] |

```
Plan view, Y = L3 (Bridge Sub-Deck & Dual-Door Decontamination Airlock)
        X: −3             X: −2             X0              X: +2             X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────────────────────┐
           │ HEAVY ARMOR FOUNDATION DECK (Under Canopy Glazing)                          │
    Z: +10 │ [SUB-FLOOR C2 RACK] ── (Dual PBs + AI Blocks beneath Deck L4 Walkway)       │
    Z:  +8 ├─────────────────────┬─────────────────────────┬─────────────────────────────┤
           │ [MAS DOOR INNER]    │ [AIRLOCK VESTIBULE]     │ [LADDER UP TO ROOF CROWN]   │
    Z:  +7 │ [PORT DECON CHAMBER]│                         │                             │
           │ (UV Light / Vent)   │                         │                             │
           │ [MAS DOOR OUTER]    │                         │                             │
    Z:  +6 └───────▲─────────────┴─────────────────────────┴─────────────────────────────┘
                   │
         From Port Boarding Ladder (X −3)
```

##### Placement Instructions
1. Lay Heavy Armor Full Blocks across X −3 to +3, Z +7 to +12 at Level L3, forming the airtight foundation deck.
2. Install exterior Sliding Door `MAS Door Boarding Outer` at [X −3, Z +7, L2/L3], sealing the boarding threshold from the exterior ladder.
3. Install interior Sliding Door `MAS Door Boarding Inner` at [X −2, Z +7, L3], creating the hermetic dual-door chamber isolating the Bridge cabin from external atmosphere.
4. Mount Air Vent `MAS Vent Airlock Port` and UV Decon Light `MAS Light Decon Port` in the airlock ceiling at [X −3, Z +7, L4]. Plumb vent to the primary oxygen conveyor trunk.
5. Install Event Controller `MAS EC Airlock Port` on the chamber bulkhead at [X −3, Z +8, L3].
6. Install Air Vent `MAS Vent Bridge` in the main bridge ceiling at [X0, Z +9, L4] to maintain 100% O2 cabin pressurization (Hermetic Zone 1).
7. Mount Survival Kit `MAS Survival Kit Tractor` at [X −2, Z +8, L4] with dedicated suit recharge port facing the central walkway.

##### Terminal Configuration & Sliders
* Outer Door: `MAS Door Boarding Outer` (Auto-Close: `ON` · Delay: `2.0 s`)
* Inner Door: `MAS Door Boarding Inner` (Auto-Close: `ON` · Delay: `2.0 s`)
* Airlock Vent: `MAS Vent Airlock Port` (Depressurize: `ON` on exit, `OFF` on entry)
* Cabin Vent: `MAS Vent Bridge` (Depressurize: `OFF` · Maintains 100% O2 pressure in Zone 1)
* Decon Light: `MAS Light Decon Port` (Color: `#00FFFF` Cyan · Radius: `3.0 m` · Intensity: `5.0`)
* Event Controller: `MAS EC Airlock Port` (Automated Interlock Sequencing):
  - Outbound Egress: Outer door lock engaged; Vent sets `Depressurize = ON` (100% gas reclamation into main tanks); once chamber pressure reaches 0.0 atm, `MAS Door Boarding Outer` unlocks and opens (zero gas lost in vacuum).
  - Inbound Ingress: Outer door closes and locks; `MAS Light Decon Port` flashes cyan for 3.0 s (UV decontamination washdown); Vent sets `Depressurize = OFF` (pressurizes to 1.0 atm); once chamber pressure reaches 100%, `MAS Door Boarding Inner` unlocks and opens into the cabin.
* Survival Kit: `MAS Survival Kit Tractor` (Spawn point enabled · Suit recharging active)

**✓ Check:** Foundation deck sealed airtight; airlock dual doors never open simultaneously; gas reclamation reclaims 100% air before outer hatch unseals; UV decon washdown triggers on inbound cycle; survival kit powered and pressurized.

---

#### Step 10.22 · 3-Operator Console Stations (Helm, Boom CTC, Fleet Director) & LCD Wall (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 3-Operator C2 Command Bridge  ·  Level: L4                      │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z +10 to +11]  ·  Plan view Y = L4 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.22 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Control Station (1×1×1 Open) | 3 | Slot 7 (`#1B263B`) | 1.8 t / 45 | Stations placed side-by-side at X −2, X0, X +2 (Z +11), facing Bow (+Z) |
| Armory Locker | 1 | Slot 7 (`#1B263B`) | 0.8 t / 10 | `MAS Armory Bridge` at [X +2, Z +10, L4], Starboard bridge wall |
| Equipment Locker | 1 | Slot 7 (`#1B263B`) | 0.8 t / 10 | `MAS Locker Survival Gear` at [X +2, Z +9, L4], Starboard bridge walkway |
| LCD Panel (Normal) | 4 | Slot 7 (`#1B263B`) | 0.4 t / 20 | Tactical Video Wall mounted on bridge aft bulkhead at Z +8 |
| Curved LCD Panel | 2 | Slot 7 (`#1B263B`) | 0.2 t / 10 | Side HUD repeaters flanking helm and tactical director |
| Sound Block | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Alarm Meteor` at [X0, Z +8, L4], bridge aft bulkhead early warning siren |

```
Plan view, Y = L4 (Custom Walk-In Bridge Deck, Armory & Tactical Video Wall)
        X: −3          X: −2           X0            X: +2          X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────┐
           │        45° SLOPED HEAVY ARMOR GLASS CANOPY (PANORAMIC)      │
    Z: +11 │   [STATION 1]             [STATION 2]           [STATION 3] │
           │   HELM / PILOT            BOOM EXCAVATOR        FLEET DIR   │
           │  (Control Station)       (Control Station)    (Control Stn) │
    Z: +10 │        │                      │               [ARMORY BRG]  │
           │   ◄────┴──────── OPEN WALKWAY AISLE (Palette 9) ────┴────►  │
    Z: +9  ├───────────────────────────┬─────────────┬─────[LOCKER GEAR]─┤
           │ [SURVIVAL KIT & CHARGER]  │ [LCD WALL 1]│ [LCD WALL 2]      │
    Z: +8  │ Field Respawn / O2-H2     │ Consist Sys │ Tactical Fleet    │  ◄── [ALARM METEOR] (Ceiling)
    Z: +7  ├───────────────────────────┴──────┬──────┴───────────────────┤
           │ [AIRLOCK SLIDING DOOR]           │ [LADDER UP TO ROOF CROWN]│
           └──────────────────────────────────┴──────────────────────────┘
```

##### Placement Instructions
1. Mount 3 open 1×1×1 Control Stations side-by-side along Z = +11, facing forward (+Z):
   * Station 1 (Port): `MAS Helm` at [X −2, Z +11, Y = L4] (Traction, steering, power, outriggers).
   * Station 2 (Center): `MAS Station Boom` at [X0, Z +11, Y = L4] (CTC mouse aim, macros, camera).
   * Station 3 (Starboard): `MAS Station Fleet` at [X +2, Z +11, Y = L4] (Fleet coordination, JIT dispatch).
2. Install Large Grid Armory Locker `MAS Armory Bridge` against the Starboard bulkhead at [X +2, Z +10, Y = L4], pre-stocked with Tier 4 Elite tools (welder, grinder, drill), automatic rifles, and ammunition boxes.
3. Install Large Grid Equipment Locker `MAS Locker Survival Gear` at [X +2, Z +9, Y = L4], stocked with spare suit O2/H2 bottles, data pads, and emergency survival kits.
4. Mount dedicated suit recharging terminal adjacent to `MAS Survival Kit Tractor` at [X −2, Z +8, Y = L4] for immediate pilot suit replenishment upon bridge entry.
5. Maintain a clear 1-block wide walk-around aisle at Z = +10 behind the seats.
6. Mount 4 LCD Panels on the aft bulkhead at Z +8 forming the Tactical Video Wall (`MAS LCD Video Wall 1..4`).
7. Paint consoles, lockers, and video wall frames with Palette Slot 7 (`#1B263B` Fleet Command Navy).
8. Install Sound Block `MAS Alarm Meteor` at [X0, Z +8, Y = L4] mounted to the ceiling structure directly above the central tactical video wall. Hardwire into Event Controller `MAS EC Meteor` for high-decibel early warning sirens upon CIWS meteor detection.

##### 3-Operator Multi-Page Manual Hotbar Architecture

```
================================================================================
STATION 1: HELM PILOT (`MAS Helm`)
================================================================================
[Hotbar 1 · Tractive Powertrain & Bedrock Anchoring]
· Slot 1: Group `MAS Outriggers` -> Reverse (Deploy / Retract Tractor Outriggers)
· Slot 2: Group `MAS Outriggers Utility` -> Reverse (Deploy / Retract Stern Outriggers)
· Slot 3: Handbrake -> Toggle
· Slot 4: Group `MAS Wheels Drive` -> Propulsion ON/OFF
· Slot 5: Group `MAS Wheels All` -> Increase Strength (+5%)
· Slot 6: Group `MAS Wheels All` -> Decrease Strength (-5%)
· Slot 7: Event Controller `MAS EC Level` -> Trigger Auto-Leveling
· Slot 8: Group `MAS Hitch Clamps` -> Switch Lock (Anti-Oscillation Magnetic Clamping)
· Slot 9: PB `[MAS] PB Excavator` -> Run `DPU TOGGLE` (DPU Push / Dynamic Braking)

[Hotbar 2 · Consist Dynamics & Power Management]
· Slot 1: Rotor `MAS Hitch Yaw` -> Increase Braking Torque (Anti-Jackknife Lock)
· Slot 2: Rotor `MAS Hitch Yaw` -> Decrease Braking Torque (Freewheel)
· Slot 3: Group `MAS Reactors` -> Power ON/OFF (Nuclear Scram)
· Slot 4: Generator `MAS Engine H2` -> Power ON/OFF (Emergency Cold-Start)
· Slot 5: Group `MAS Batteries Consist` -> Recharge ON/OFF (Shore Fast Charge)
· Slot 6: PB `[MAS] PB Excavator` -> Run `POWER MODE 1` (Silent Eco)
· Slot 7: PB `[MAS] PB Excavator` -> Run `POWER MODE 2` (Tractive Cruise)
· Slot 8: PB `[MAS] PB Excavator` -> Run `POWER MODE 3` (Heavy Excavate)
· Slot 9: PB `[MAS] PB Excavator` -> Run `POWER MODE 4` (Emergency Failover)

[Hotbar 3 · External Access & Worksite Illuminators]
· Slot 1: Hinge `MAS Crew Ladder Hinge` -> Reverse (Lower / Raise Boarding Ladder)
· Slot 2: Group `MAS Lights Worksite` -> Toggle ON/OFF (All Tower/Boom/Underbelly)
· Slot 3: Group `MAS Floodlight Tower` -> Toggle ON/OFF (360° Cardinal Ring)
· Slot 4: Group `MAS Lights Underbelly` -> Toggle ON/OFF (Chassis Maintenance Amber)
· Slot 5: Sliding Door `MAS Door Airlock` -> Open/Close
· Slot 6: Air Vent `MAS Vent Atmo Intake Port` -> Depressurize ON/OFF
· Slot 7: Air Vent `MAS Vent Atmo Intake Stbd` -> Depressurize ON/OFF
· Slot 8: Antenna `MAS Comm Radio` -> Broadcast ON/OFF
· Slot 9: Laser Antenna `MAS Comm Laser` -> Reset Connection

================================================================================
STATION 2: EXCAVATION MASTER (`MAS Station Boom`)
================================================================================
[Hotbar 1 · Direct Joint Teleoperation & Tool Aim]
· Slot 1: Custom Turret Controller `MAS CTC Boom` -> Control (Direct Mouse Aim)
· Slot 2: Group `MAS Lasers Tool` -> Shoot ON/OFF (Twin Mining / Salvage Lasers)
· Slot 3: Camera `MAS Cam Tool Head` -> View (Bore Sump Alignment)
· Slot 4: Rotor `MAS Turret Azimuth` -> Reverse (Manual Pan Left/Right)
· Slot 5: Hinge `MAS Shoulder Pitch` -> Reverse (Manual Boom Elevation)
· Slot 6: Hinge `MAS Elbow Pitch` -> Reverse (Manual Reach Extension)
· Slot 7: Hinge `MAS Wrist Pitch` -> Reverse (Manual Tool Pitch)
· Slot 8: Group `MAS Lasers Tool` -> Next Mode (Drill <-> Grinder/Salvage <-> Welder)
· Slot 9: Magnetic Plate `MAS Tool Grapple` -> Switch Lock (Crane Hoisting)

[Hotbar 2 · Automated Kinematic Macros & Presets]
· Slot 1: PB `[MAS] PB Excavator` -> Run `DEPLOY` (Release Clamp & Raise to Ready)
· Slot 2: PB `[MAS] PB Excavator` -> Run `STOW` (14-Block Dorsal Spine Clamp Lock)
· Slot 3: Magnetic Plate `MAS Clamp Boom Stow` -> Switch Lock (Manual Clamp Override)
· Slot 4: PB `[MAS] PB Excavator` -> Run `MACRO BENCH` (Automated Quarry Sweep)
· Slot 5: PB `[MAS] PB Excavator` -> Run `BORE` (Target-Centric Column Shaft)
· Slot 6: PB `[MAS] PB Excavator` -> Run `MACRO TRENCH` (Roadbed Carve)
· Slot 7: PB `[MAS] PB Excavator` -> Run `CRANE` (Vehicle Recovery Grapple)
· Slot 8: PB `[MAS] PB Excavator` -> Run `SALVAGE` (Macro 5 · Automated Wreck Deconstruction)
· Slot 9: Group `MAS Boom Joints` -> Rotor Lock ON/OFF (E-Stop Brake)

[Hotbar 3 · Worksite Illumination & Ore Buffer Telemetry]
· Slot 1: Group `MAS Light Boom` -> Toggle ON/OFF (Tool Head Spotlights)
· Slot 2: Camera `MAS Cam Boom Turret` -> View (Wide-Angle Turret Vantage)
· Slot 3: Sorter `MAS Sorter Tailings` -> Drain All ON/OFF
· Slot 4: Cargo Container `MAS Tractor Buffer` -> Show on HUD ON/OFF
· Slot 5: Cargo Container `MAS Hold Ore Cat` -> Show on HUD ON/OFF
· Slot 6: Event Controller `MAS EC Ore Full` -> Trigger Alert
· Slot 7: PB `[MAS] PB Excavator` -> Run `MT_TURRETS TOGGLE` (Toggle 4× Multitool Repair Turrets Weld/Grind)
· Slot 8: PB `[MAS-U] PB Utility` -> Run `AUTOPROD SML` (Verify/Force SML Quota Production)
· Slot 9: Group `MAS Multitool Turrets` -> Toggle ON/OFF (Consist & Drone Field Repair)

================================================================================
STATION 3: TACTICAL FLEET DIRECTOR (`MAS Station Fleet`)
================================================================================
[Hotbar 1 · Predictive JIT Logistics & Offload Automation]
· Slot 1: PB `[MAS] PB RIG//OS-Lite` -> Run `JIT AIR` (Pre-Dispatch Mule Air Ferry)
· Slot 2: PB `[MAS] PB RIG//OS-Lite` -> Run `JIT GND` (Pre-Dispatch Caterpillar Train)
· Slot 3: Connector `MAS MP Conn` -> Lock/Unlock (Mule Roof Pad Connector)
· Slot 4: Group `MAS MP Clamps` -> Switch Lock (Mule Cradle Rub Rails)
· Slot 5: Sorter `MAS MP Sorter` -> Drain All ON/OFF (Rare Ore Fast Drain)
· Slot 6: Sorter `MAS Sorter Resupply Export` -> Drain All ON/OFF (Export Staging Drain)
· Slot 7: Piston `MAS Piston Stern Dock` -> Reverse (Extend / Retract Telescoping Soft Dock)
· Slot 8: Connector `MAS Stern Dump Conn` -> Lock/Unlock (Caterpillar Stern Rail)
· Slot 9: PB `[MAS] PB Excavator` -> Run `RESUPPLY MULE` (Stage Resupply Kit & Dispatch Mule)

[Hotbar 2 · Slave Links, Recon Tasking & Swarm Deconfliction]
· Slot 1: Remote Control `MAS-02 Remote` -> Control (Subordinate Worker Excavator)
· Slot 2: Remote Control `CAT-01 Remote` -> Control (Caterpillar Locomotive)
· Slot 3: Remote Control `KES-01 Remote` -> Control (Kestrel Scout Drone)
· Slot 4: Remote Control `ALB-01 Remote` -> Control (Albatross Repair Drone)
· Slot 5: PB `[MAS] PB RIG//OS-Lite` -> Run `TASK KESTREL` (Vector Scout Survey Sector)
· Slot 6: PB `[MAS] PB Excavator` -> Run `SEISMIC` (Trigger 500 m Acoustic Voxel Survey)
· Slot 7: PB `[MAS] PB RIG//OS-Lite` -> Run `CLAIM ZONE` (Broadcast Quarry Exclusion Zone)
· Slot 8: LCD Panel `MAS LCD Video Wall 1` -> Cycle Page (Fleet Logistics Telemetry)
· Slot 9: LCD Panel `MAS LCD Video Wall 2` -> Cycle Page (Seismic Survey Map)

[Hotbar 3 · Perimeter CIWS Defense & Networked Fleet Targeting]
· Slot 1: Large Gatling Turret `MAS Turret Bow` -> Control (Direct CIWS Aim)
· Slot 2: Large Gatling Turret `MAS Turret Stern` -> Control (Stern CIWS Aim)
· Slot 3: Camera `MAS Cam Bow CIWS` -> View
· Slot 4: Camera `MAS Cam Stern CIWS` -> View
· Slot 5: Group `MAS Turrets Flank` -> Power ON/OFF (Anti-Personnel Skirt Guns)
· Slot 6: Group `MAS Turrets All` -> Target Hostiles ON/OFF
· Slot 7: PB `[MAS] PB RIG//OS-Lite` -> Run `TARGET NET` (Synchronize Fleet CIWS Interlock)
· Slot 8: PB `[MAS] PB RIG//OS-Lite` -> Run `ALERT CBT` (Lockdown / Combat Mode)
· Slot 9: PB `[MAS] PB RIG//OS-Lite` -> Run `ALERT NORM` (Clear Combat Alert)
```

**✓ Check:** All 3 stations face forward with clear view of the forward terrain; all 9 hotbar pages mapped and functioning.

---

#### Step 10.23 · 45° Sloped Heavy Armor Glass Panoramic Canopy & Roof Crown (L4-L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Armored Glazing & Roof Crown  ·  Level: L4 to L5                │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z +11 to +12]  ·  Looking Starboard (+X) Elevation│
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.23 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sloped Heavy Armor Glass | 14 | Transparent / Tint | 8.4 t / 14 | 45° forward glacis slopes (X −3 to +3, Z +11 to +12, L4-L5) |
| Heavy Armor Slope Block | 12 | Slot 0 (`#2B303A`) | 10.2 t / 12 | Roof crown frame connecting glass tops to Level L5 deck |
| Narrow Ladder Block | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 2 | Vertical ladder at [X +2, Z +7, L4-L5] climbing to roof crown hatch |

```
Longitudinal Elevation · Bridge Glazing Profile (Looking Starboard, X0)
    Level L5 Crown ──────────■═══════════════════════════■  (Dorsal Crown Deck)
                             ╲                           ║  Aft Bulkhead (Z +7)
                              ╲  45° SLOPED HEAVY       ║
    Level L4 Bridge Deck       ╲  ARMOR GLASS CANOPY     ║  [STATION 2: BOOM]
    ════════════════════════════■────────────────────────╨  [C2 Server Rack Under]
    Level L3 Foundation                                      Level L3 (Z +7)
    ═══════════════════════════════════════════════════════
```

##### Placement Instructions
1. Install Sloped Heavy Armor Glass blocks along the forward glacis edge (Z = +11 to +12) angled at 45°, spanning X −3 to +3 across Levels L4 to L5.
2. Install corner glass transitions on Port (X = −3) and Starboard (X = +3) to provide 180° horizontal field of view.
3. Tie the upper edge of the glass into Heavy Armor Slope blocks forming the Level L5 roof crown.
4. Install vertical ladder blocks at [X +2, Z +7] climbing from the bridge deck up through an armored hatch to the roof crown.

**✓ Check:** Cabin completely enclosed and pressurized (100% O2); panoramic forward sightlines provide unobstructed view of the cutting zone and ground terrain.

---

#### Step 10.24 · C2 Compute Server Rack, Deep Ore Detector & Comms Mast (L3, L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Avionics, Sensors & Comms Spire  ·  Level: L3 & L6              │
│ [VIEWPORT] Grid Zone: Rack [X0, Z +7, L3]; Detector [X0, Z +8, L3]; Mast [X −2, Z +7, L6]│
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.24 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Programmable Block | 2 | Slot 7 (`#1B263B`) | 1.0 t / 50 | `[MAS] PB Excavator` & `[MAS] PB RIG//OS-Lite` in L3 rack |
| AI Automaton Block | 3 | Slot 7 (`#1B263B`) | 0.9 t / 75 | `MAS AI Move`, `MAS AI Task`, `MAS AI Defense` in L3 rack |
| Large Ore Detector | 1 | Slot 4 (`#1ABC9C`) | 4.8 t / 15 | Centered in sub-deck keel at [X0, Z +8, Y = L3], range: 150 m |
| Laser Antenna | 1 | Slot 7 (`#1B263B`) | 2.5 t / 15 | `MAS Comm Laser` (40 km line-of-sight) on dorsal mast [X −2, Z +7, L6] |
| Radio Antenna | 1 | Slot 7 (`#1B263B`) | 0.8 t / 15 | `MAS Comm Radio` (50 km fleet broadcast) on dorsal mast |

```
Longitudinal Section · C2 Server Rack & Comms Mast Placement
         [MAS COMM LASER (40 km)]     [BOW CIWS GATLING]
         [MAS COMM RADIO (50 km)]     [X0, Z +7, Y = L5]
                 │                           │
                 ▼ (Mast at X −2, L6)        ▼
    Level L5 ════■═══════════════════════════■═══════ (Crown Deck)
                 │                           │
    Level L4 ────┼── [3-STATION C2 BRIDGE] ──┼─────── (Bridge Deck)
                 │                           │
                 ▼                           ▼
    Level L3 ┌───────────────────────────┐ ┌────────┐
             │ [C2 SERVER RACK: DUAL PB] │ │ ORE    │ (150 m Deep Range)
             │ [AI AUTOMATON SUITE]      │ │ DETECT │
             └───────────────────────────┘ └────────┘
```

##### Placement Instructions
1. In the sub-bridge compartment at Level L3, mount Programmable Block `[MAS] PB Excavator` and Programmable Block `[MAS] PB RIG//OS-Lite` side-by-side at [X0, Z +7, Y = L3].
2. Flank with AI Automaton blocks: `MAS AI Move`, `MAS AI Task`, and `MAS AI Defense`.
3. Mount Large Ore Detector `MAS Ore Detector` in the centerline keel cavity at [X0, Z +8, Y = L3].
4. Erect the dorsal comms spire at [X −2, Z +7, Y = L6], mounting Laser Antenna `MAS Comm Laser` and Radio Antenna `MAS Comm Radio`.

##### Terminal Configuration & Sliders
* Programmable Block: `[MAS] PB Excavator` (Update rate: `Update1` / 60 Hz · Kinematics engine)
* Programmable Block: `[MAS] PB RIG//OS-Lite` (Update rate: `Update10` · Fleet C2 & IGC router)
* Antenna: `MAS Comm Radio` (Range: `50,000 m` · Broadcast: `ON`)
* Laser Antenna: `MAS Comm Laser` (Range: `40,000 m`)
* Ore Detector: `MAS Ore Detector` (Range: `150 m`)

**✓ Check:** Dual PBs initialize; ore detector displays subterranean ore signatures out to 150 m; laser antenna and 50 km radio establish network link.

---

#### Step 10.25 · Bridge Anti-Capture Scuttle System & Native Countdown Timer (L3-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Asset Denial & Scuttle Loop  ·  Level: L3-L4                    │
│ [VIEWPORT] Grid Zone: Warhead [X0, Z +8, L3]; Panel [X +1, Z +8, L4]                 │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.25 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Grid Warhead | 1 | Slot 6 (`#C0392B`) | 1.6 t / 20 | `MAS Warhead Scuttle Bridge` mounted beneath server rack at [X0, Z +8, L3] |
| Timer Block | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Timer Scuttle` (60-second native countdown loop) |
| Sci-Fi 4-Button Panel | 1 | Slot 6 (`#C0392B`) | 0.1 t / 5 | `MAS BP Scuttle Bridge` mounted on bridge aft bulkhead at [X +1, Z +8, L4] |

##### Placement Instructions
1. Mount 1 Large Grid Warhead `MAS Warhead Scuttle Bridge` directly beneath the C2 compute rack at [X0, Z +8, Y = L3]. Disarmed by default.
2. Mount Timer Block `MAS Timer Scuttle` in the avionics rack.
3. Install Sci-Fi 4-Button Panel `MAS BP Scuttle Bridge` on the bridge aft wall at [X +1, Z +8, Y = L4].

##### Terminal Configuration & Sliders
* Panel: `MAS BP Scuttle Bridge`
  * Button 1: Timer Block `MAS Timer Scuttle` -> Start (Initiate 60 s Countdown)
  * Button 2: Timer Block `MAS Timer Scuttle` -> Stop (Emergency Abort)
  * Button 3: Group `MAS Warheads Scuttle` -> Safety OFF (Manual Arm)
  * Button 4: Group `MAS Warheads Scuttle` -> Safety ON (Manual Disarm)
* Timer Block: `MAS Timer Scuttle`
  * Delay: `60.0 s`
  * Actions at T = 0 s: `MAS Warhead Scuttle Bridge` -> Detonate; `MAS Warhead Scuttle Power` -> Detonate

**✓ Check:** Warhead disarmed by default; button panel triggers countdown; manual abort halts timer and returns warheads to safety lock.

---

#### Step 10.26 · Physical In-Situ Interfaces (Sci-Fi 4-Button Panels) (L1-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Physical Foot Control Network  ·  Levels: L1 to L6              │
│ [VIEWPORT] Grid Zone: 6 Cardinal Crew Stations across Consist                        │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.26 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sci-Fi 4-Button Panel | 6 | Slot 9 (`#F1C40F`) | 0.6 t / 30 | Distributed across ground skirts, airlocks, workshop, flight deck, stern |

##### Master Button Panel Mapping Table
| Terminal Name | Grid Coordinates | Functional Location | Button 1 Action | Button 2 Action | Button 3 Action | Button 4 Action |
|---|---|---|---|---|---|---|
| `MAS BP Ground Boarding`| [X −4, Z +7, L1] | Port lower skirt | Ladder Lower/Raise | Keel Lights ON/OFF | Outer Airlock Cycle | Radio Beacon Toggle |
| `MAS BP Airlock Vestibule`| [X −2, Z +7, L3] | Bridge airlock entry | Outer Door Cycle | Inner Door Cycle | Cabin Pressurize | EVA Locker Refill |
| `MAS BP Power Vault` | [X −2, Z −1, L3] | Jefferies tube wall | Vault Door Toggle | Reactor SCRAM | H2 Engine Cold-Start | Battery Shore Charge |
| `MAS BP Field Workshop` | [X0, Z −14, L3] | Utility central aisle | Assembler Queue Plates | Refinery Flush | O2/H2 Gen Toggle | Workshop Lights |
| `MAS BP Flight Deck` | [X +2, Z −10, L6]| Roof hatch landing | Mule Clamps Toggle | Mule Conn Lock | Kestrel Welders Pulse | Deck Floodlights |
| `MAS BP Stern Dump` | [X +2, Z −25, L2]| Rear dump platform | Stern Conn Lock | Sorter Drain All | Land Train Ping | Rear Worksite Light |

**✓ Check:** All 6 button panels powered, named, and mapped to local subsystem toggles for foot operation.

---

#### Step 10.27 · Retractable Port Crew Boarding Ladder & Proximity Walkway (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Boarding Ladder & Drive Interlock  ·  Level: L2                 │
│ [VIEWPORT] Grid Zone: [X −4, Z +7, Y = L2]  ·  Looking Bow (+Z) Profile               │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.27 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Motor Hinge | 1 | Slot 9 (`#F1C40F`) | 3.5 t / 15 | `MAS Crew Ladder Hinge` mounted flush in Port hull skirt at [X −4, Z +7, L2] |
| Narrow Ladder Block | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 2 | Attached to hinge head, extending downward when deployed |
| Sensor | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Sensor Ladder` set to 2.5 m crew proximity detection radius |

```
CREW BOARDING LADDER (Port Flank Profile)

     [TRANSIT: FLUSH STOWED]                   [BOARDING: DEPLOYED TO GROUND]
           Deck L3                                   Deck L3
         ┌─────────┐                               ┌─────────┐
         │ CAB WALL│                               │ CAB WALL│
     L2  ├─────────┴─ [HINGE 0°]               L2  ├─────────┴─ [HINGE 90° ↓]
         │ WHEEL WELL │ [LADDER FOLDED]            │ WHEEL WELL │
     L1  │            │                            │            │   │ [LADDER RUN]
     ════╧════════════╧══════════ Ground       L0  ═════════════╧═══▼ (Touchdown)
```

##### Placement Instructions
1. Install Large Motor Hinge `MAS Crew Ladder Hinge` flush in the Port hull skirt at [X −4, Z +7, Y = L2]. Orient barrel horizontally along Z-axis so it pivots outward/downward.
2. Attach 2 Narrow Ladder blocks to the hinge head.
3. Install Sensor `MAS Sensor Ladder` calibrated to a 2.5 m radius covering the Port flank ground zone.

##### Terminal Configuration & Sliders
* Hinge: `MAS Crew Ladder Hinge` (Limits: `0.0°` flush stowed to `90.0°` down · Torque: `10 MN·m`)
* Sensor: `MAS Sensor Ladder` (Detect Players: `ON` · Range: `2.5 m`)
* Event Controller: `MAS EC Ladder`
  * Condition: Player within 2.5 m AND Speed $= 0\text{ km/h}$ -> Deploy Ladder (90°)
  * Condition: Handbrake OFF OR Speed $> 0\text{ km/h}$ -> Retract Ladder (0° Flush)

**✓ Check:** Ladder automatically swings down to ground when engineer approaches on foot; auto-retracts flush upon releasing handbrake.

---

#### Step 10.28 · Bow Multitool Repair Turret, CIWS Gatling Turret & Flank Sockets (L2, L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Locomotive Defense & Forward Repair  ·  Levels: L2 & L5        │
│ [VIEWPORT] Grid Zone: Bow Crown [X0, Z +7 to +9, L5]; Flank Skirts [X ±4, Z +2, L2]  │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.28 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Laser Multitool Turret | 1 | Slot 3 (`#E07A5F`) | 4.5 t / 225 | `MAS MT Turret Bow` mounted on forward crown at [X0, Z +9, L5] · Mod 3175774286 |
| Large Gatling Turret | 1 | Slot 7 (`#1B263B`) | 4.8 t / 125 | `MAS Turret Bow` mounted on dorsal crown at [X0, Z +7, L5] |
| Interior Turret | 2 | Slot 7 (`#1B263B`) | 0.8 t / 50 | `MAS Turret Flank Port/Stbd` mounted flush on skirts at [X ±4, Z +2, L2] |

##### Placement Instructions
1. Mount Large Grid Gatling Turret `MAS Turret Bow` on the centerline dorsal crown of Carriage 1 at [X0, Z +7, Y = L5]. Plumb bottom port to tractor conveyor spine at L4.
2. Mount 2 Interior Turrets flush on the lower armored skirts at [X −4, Z +2, Y = L2] (Port) and [X +4, Z +2, Y = L2] (Starboard).
3. Connect ammo feed lines to the main conveyor trunk to draw NATO 25×184mm and 5.56mm ammo from internal storage.
4. Mount Large Grid Laser Multitool Turret `MAS MT Turret Bow` on the forward crown directly ahead of the bridge glass at [X0, Z +9, Y = L5]. Plumb bottom port into the conveyor spine at L4. Set default mode to Welder for forward hull and tire self-repair.

##### Terminal Configuration & Sliders
* Group: `MAS Turrets Defense` (`MAS Turret Bow`, `MAS Turret Flank Port`, `MAS Turret Flank Stbd`)
* Group: `MAS Multitool Turrets` (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`)
* Event Controller: `MAS EC Combat`
  * Condition: Hull Damage Detected OR Hostile Lock Acquired
  * Action Slot 1: Elevate `RIG.OS.MODE CBT` -> Power ON Turrets -> Seal Compartment Doors

---

##### Pre-Enclosure Inspection Gate 10.4A: Bridge Airtightness & C2 Network Link Check
* [ ] Confirm bridge cabin pressurizes to 100% O2 via air vent.
* [ ] Verify `MAS Ore Detector` displays subterranean ore deposits out to 150 m range.
* [ ] Confirm CTC block maps mouse yaw to `MAS Turret Azimuth` and mouse pitch to `MAS Shoulder Pitch`.
* [ ] Confirm `MAS Comm Laser` and `MAS Comm Radio` establish two-way IGC telemetry ping.
* [ ] Verify `MAS Crew Ladder Hinge` automatically lowers when approached on foot and retracts upon handbrake release.
* [ ] Verify `MAS Turret Bow` and `MAS Turret Flank Port/Stbd` receive NATO 25×184mm ammunition through conveyor spine.
* [ ] Verify `MAS MT Turret Bow` receives components and auto-repairs forward armor and tires.

```
PHASE 10-4 VISUAL MILESTONE · C2 BRIDGE & AVIONICS COMPLETE
       Z: +7 (Crown CIWS)     Z: +9 (Bow Repair MT)     Z: +10 (3-Seat Bridge)    Z: +12 (Glass Bow)
       [MAS TURRET BOW]       [MAS MT TURRET BOW]       [HELM / BOOM / FLEET]     [45° PANORAMIC CANOPY]
             │                         │                         │                        │
       ══════╧═════════════════════════╧═════════════════════════╧════════════════════════╧══════
                    C2 COMPUTE RACK & ORE DETECTOR IN SUB-DECK (L3)
```

---

### Phase 10-5 · Modular FOB & Utility Carriage Chassis & Workshop (Carriage 2)

```
       [PHASE 10-5 MASTER SPATIAL LOCATOR HUD]
       Envelope: 11 W × 18 L × 5 H B (27.5 × 45.0 × 12.5 m)
       Coordinate Range: X −5 to +5, Z −26 to −8, Y = L1 to L5
       Carriage Datums:
       · Front Drawbar Hitch Receiver: [X0, Z −8, Y = L2]
       · Axle 4 (Mid Drive Suspension): [X ±5, Z −13, Y = L1] (0.0° Steer)
       · Sub-Floor Battery Vault: [X −1 to +1, Z −16, Y = L2] (9.0 MWh)
       · Field Workshop Core: [X −3 to +3, Z −18 to −12, Y = L3]
       · Axle 5 (Aft Counter-Steer Suspension): [X ±5, Z −22, Y = L1] (−25.0° Steer)
       · Component Hold (3×3×3 Large Cargo): [X0, Z −22, Y = L3-L4]
       · Stern Caterpillar Dump Bumper: [X0, Z −26, Y = L2]
```

#### Pre-Step Master Parts Tray: Phase 10-5
* 240× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 40× Heavy Armor Ballast Plates (Palette 0: Keel CoM Balancer)
* 4× Wheel Suspension 5×5 (2 Port, 2 Starboard · Palette 0)
* 3× Large Battery (`MAS Battery 4, 5, 6`, 9.0 MWh total, Palette 6: High-Voltage Red)
* 2× Magnetic Plate (`MAS Utility Brake Keel 1, 2`, Palette 0)
* 1× Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW, Palette 4: Processing Teal)
* 1× Large Assembler (`MAS Utility Assembler`, Palette 4: Processing Teal)
* 1× Medical Room (`MAS Medical Bay`, Palette 7: Fleet Command Navy)
* 2× Cryo Chamber (`MAS Cryo Pod 1, 2`, Palette 7: Fleet Command Navy)
* 2× Large Oxygen Tank (`MAS Tank O2 1, 2`, 200,000 L O2, Palette 4: Processing Teal)
* 1× Small Cargo Container (`MAS Hold Ice LifeSupport`, 15,000 L Quarantined Ice, Palette 4)
* 2× Large Hydrogen Tank (`MAS Tank H2 1, 2`, 10,000,000 L H2, Palette 6)
* 2× O2/H2 Generator (`MAS Gen O2/H2 1, 2`, Palette 4)
* 2× Air Vent (`MAS Vent Atmo Intake Port/Stbd`, Palette 0: Ambient Intake)
* 1× Large Cargo Container (3×3×3 B, `MAS Hold Components`, Palette 5: Logistics Emerald)
* 1× Large Projector (`MAS Projector Master`, Palette 7: Fleet Command Navy)
* 1× Programmable Block (`[MAS-U] PB Utility`, Palette 7)
* 1× Remote Control (`MAS-U Remote FOB`, Palette 7)
* 2× Large Conveyor Sorter (`MAS Sorter Ammo Fwd`, `MAS Sorter Parts Aft`, Palette 4)
* 2× Sliding Door (`MAS Door Gangway Aft`, `MAS Door Stern Airlock`, Palette 9)
* 14× Grated Catwalk Block, 6× Narrow Ladder Block (Palette 9: High-Viz Safety Yellow)

---

#### Step 10.29 · Utility Carriage Box Girders, Keel Ballast Skids & Breakaway Deadman (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Utility Chassis Box Girders & Ballast  ·  Level: L1-L2          │
│ [VIEWPORT] Grid Zone: [X −5 to +5, Z −26 to −8]  ·  Plan view Y = L1-L2 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.29 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 72 | Slot 0 (`#2B303A`) | 61.2 t / 72 | Longitudinal twin box girders at X ±1 from Z −8 to −26; transverse ties at Z −8, −13, −18, −22, −26 |
| Heavy Armor Full Block (Ballast) | 40 | Slot 0 (`#2B303A`) | 120.0 t / 40 | Sub-floor keel ballast plates between girders along L1 from Z −12 to −22 |
| Magnetic Plate (Large) | 2 | Slot 0 (`#2B303A`) | 5.8 t / 50 | `MAS Utility Brake Keel 1, 2` at [X ±2, Z −18, Y = L1] facing downward to terrain |
| Radio Antenna | 1 | Slot 7 (`#1B263B`) | 0.8 t / 25 | `MAS-FOB-01 [EMERGENCY BREAKAWAY]` in protected keel recess [X0, Z −18, L1] |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS-U EC Breakaway` at [X0, Z −10, L2] monitoring drawbar hitch handshake |

```
Plan view, Y = L1-L2 (Utility Chassis Frame, Ballast Plates & Ground Keel Skids)
        X: −3           X: −1      0      X: +1           X: +3
    Z:  −8 ┌───────────────┬───────╤───────┬───────────────┐
           │ AFT HITCH TONGUE RECEIVER & DRAWBAR INTERFACE │  ◄── Y = L2 Drawbar Tongue
    Z: −10 ├───────────────┼───────┴───────┼───────────────┤
           │               │ [MAS-U EC BRK]│               │  ◄── Breakaway Event Controller
    Z: −12 ├───────────────┼───────────────┼───────────────┤
           │ [BALLAST 1]   │ 40× HEAVY ARMOR BALLAST       │  ◄── 120 t Low-Slung Keel Deadweight
    Z: −15 │ [BALLAST 2]   │ CO-LOCATED BETWEEN GIRDERS    │      Lowers CoM to Level L2.4
    Z: −18 │ [KEEL SKID 1] │ [BREAKAWAY BEACON] [KEEL 2]   │  ◄── Downward Ground Clamping Skids
    Z: −20 │ [BALLAST 3]   │ EXPANDS ROLLOVER TO 34°       │
    Z: −22 ├───────────────┼───────────────┼───────────────┤
           │               │ AFT SUSPENSION TIE BEAM       │  ◄── Axle 5 Cross-Member
    Z: −26 └───────────────┴───────────────┴───────────────┘  ◄── Stern Collision Push Bumper
```

##### Placement Instructions
1. Establish longitudinal box girders at X = −1 and X = +1 spanning continuously from Z = −8 to Z = −26 at Level L2.
2. Fabricate transverse cross-ties at Z = −8 (tongue header), Z = −13 (Axle 4 bolster), Z = −18 (mid-keel bolster), Z = −22 (Axle 5 bolster), and Z = −26 (stern push-bumper).
3. Lay 40× Heavy Armor Full Blocks into the Level L1 central trough between X = −1 and X = +1 from Z = −12 to Z = −22. This concentrated 120.0 t ballast counteracts the elevated roof Mule cradle pad (15.0 m above terrain) and lowers Carriage 2 Center of Mass from L3.8 to L2.4.
4. Mount 2× Large Grid Magnetic Plates (`MAS Utility Brake Keel 1, 2`) at [X −2, Z −18, Y = L1] and [X +2, Z −18, Y = L1] pointing directly downward toward ground voxels.
5. Install Radio Antenna `MAS-FOB-01 [EMERGENCY BREAKAWAY]` in the armored keel pocket at [X0, Z −18, Y = L1].
6. Place Event Controller `MAS-U EC Breakaway` at [X0, Z −10, Y = L2] inside the forward chassis sill.

##### Terminal Configuration & Sliders
* Magnetic Plates: `MAS Utility Brake Keel 1, 2` (Auto-Lock: `OFF` · Proximity Sensor: `ON` · Lock Strength: `100%`)
* Antenna: `MAS-FOB-01 [EMERGENCY BREAKAWAY]` (Broadcast Radius: `50 km` · Enable Broadcasting: `OFF` · Show in HUD: `ON`)
* Event Controller: `MAS-U EC Breakaway`
  - Monitored Event: `Grid Connection / Handshake`
  - Monitored Target: `MAS Hitch Sever Tongue` or Carriage 1 electrical bus
  - Condition: Connection lost (0.0 V terminal handshake)
  - Slot 1 (Severance Detected):
    - Group `MAS Utility Wheels` -> `Power OFF`
    - Group `MAS Utility Wheels` -> `Handbrake ON`
    - `MAS Utility Brake Keel 1, 2` -> `Auto-Lock ON`
    - `MAS Utility Brake Keel 1, 2` -> `Lock`
    - `MAS-FOB-01 [EMERGENCY BREAKAWAY]` -> `Enable Broadcasting ON`
  - Slot 2 (Normal Coupled): Keel Skids `Auto-Lock OFF` · Keel Skids `Unlock` · Antenna `Broadcasting OFF`

✓ Check: Keel ballast plates lower CoM to Level L2.4; magnetic keel skids clear ground plane by 0.5 m under normal ride height; breakaway event controller engages emergency lockdown on simulated drawbar disconnect.

---

#### Step 10.30 · Axles 4 & 5 Suspension Wheels & DPU Smart Traction / Braking (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Utility Powertrain & DPU Kinematics  ·  Level: L1               │
│ [VIEWPORT] Grid Zone: [X ±5, Z −13 and Z −22]  ·  Plan view Y = L1 (▲ Bow +Z)         │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.30 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Wheel Suspension 5×5 (Port) | 2 | Slot 0 (`#2B303A`) | 19.8 t / 200 | `MAS Wheel L4 (Port)` at [X −5, Z −13, L1] and `MAS Wheel L5 (Port)` at [X −5, Z −22, L1] |
| Wheel Suspension 5×5 (Stbd) | 2 | Slot 0 (`#2B303A`) | 19.8 t / 200 | `MAS Wheel R4 (Stbd)` at [X +5, Z −13, L1] and `MAS Wheel R5 (Stbd)` at [X +5, Z −22, L1] |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC DPU Drive` at [X0, Z −11, L2] coordinating tractive push & descent tension |

```
Plan view, Y = L1 (Utility Carriage 4-Wheel Powertrain Geometry)
        X: −5          X: −1      0      X: +1          X: +5
    Z: −13 [WH 7 (PORT)] ═════╦═══════════╦═════ [WH 8 (STBD)]  ◄── Axle 4 (0.0° Locked Drive)
           │                  │           │                  │
    Z: −17 │                  │   KEEL    │                  │
           │                  │  BALLAST  │                  │
    Z: −22 [WH 9 (PORT)] ═════╩═══════════╩═════ [WH 10 (STBD)] ◄── Axle 5 (−25.0° Counter-Steer)
```

##### Detailed Axle Kinematics Specification
| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 4** | Z = −13 | `WH 7, 8`  | `0.0°`  | OFF (Locked) | `100%` | `50% / 90%` | `75%` |
| **Axle 5** | Z = −22 | `WH 9, 10` | `25.0°` | INVERTED     | `100%` | `55% / 90%` | `80%` |

##### Placement Instructions
1. Weld 5×5 Wheel Suspension units at [X −5, Z −13, Y = L1] (Port) and [X +5, Z −13, Y = L1] (Starboard), designating this assembly as Axle 4. Configure as fixed drive (0.0° steering angle).
2. Weld 5×5 Wheel Suspension units at [X −5, Z −22, Y = L1] (Port) and [X +5, Z −22, Y = L1] (Starboard), designating this assembly as Axle 5. Configure with inverted counter-steering (−25.0°).
3. Assign all 4 suspension blocks to terminal group `MAS Utility Wheels`.
4. Install Event Controller `MAS EC DPU Drive` at [X0, Z −11, Y = L2].

##### Terminal Configuration & Sliders
* Group: `MAS Utility Wheels`
  - Max Speed: `50 km/h`
  - Speed Limit: `ON`
  - Propulsion: `100%`
  - Brake Multiplier: `100%`
* Dynamic Traction & Braking Rules (`MAS EC DPU Drive` / `[MAS] PB Excavator`):
  - Hill-Climb Traction: When ascending grades > 5°, Axles 4 and 5 apply 100% matching tractive push synchronized with Tractor throttle, distributing load evenly across all 10 wheels.
  - Dynamic Tension Braking: When descending grades or decelerating, Carriage 2 wheels dynamically apply counter-braking torque 100 ms prior to Tractor braking bite, keeping the hitch in continuous longitudinal tension and preventing trailer jackknifing.
  - Automatic Traction Control (ATC): On loose voxel detection, wheel torque drops by 20% increments until slip ceases.

✓ Check: Axle 4 tracks dead-ahead; Axle 5 pivots opposite to Tractor front steering wheels; consist achieves 35 m turning radius without trailer scrubbing or wheel lift.

---

#### Step 10.31 · Sub-Floor Utility Battery Vault, Shore Power Bus & Hardened Ammo Bunker (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Sub-Floor Battery Vault & Ammo Bunker  ·  Level: L2             │
│ [VIEWPORT] Grid Zone: [X −1 to +1, Z −17 to −15]  ·  Plan view Y = L2 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.31 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Battery | 3 | Slot 6 (`#C0392B`) | 25.5 t / 240 | `MAS Battery 4, 5, 6` in sub-floor well at [X −1, 0, +1, Z −16, L2] |
| Small Cargo Container | 1 | Slot 7 (`#1B263B`) | 1.0 t / 25 | `MAS Hold Munitions` at [X0, Z −15, L2], sub-floor flash-shielded bunker |
| Heavy Armor Full Block | 12 | Slot 0 (`#2B303A`) | 10.2 t / 12 | Armored vault perimeter enclosure and structural top ceiling deck |

```
Plan view, Y = L2 (Utility Sub-Floor Battery Vault & Ammo Bunker)
        X: −1             0             X: +1
    Z: −15 ┌───────────────┬─────────────┬───────────────┐
           │ [SOLID ARMOR] │ [AMMO HOLD] │ [SOLID ARMOR] │  ◄── MAS Hold Munitions (Sub-Floor Bunker)
    Z: −16 ├───────────────┴─────────────┴───────────────┤
           │ [MAS BATTERY 4] [BATTERY 5] [BATTERY 6]     │  ◄── 9.0 MWh Energy Storage Buffer
    Z: −17 │ (3.0 MWh)       (3.0 MWh)   (3.0 MWh)       │      36.0 MW Peak Discharge Bus
           └─────────────────────────────────────────────┘
```

##### Placement Instructions
1. Excavate and frame a 3-block wide by 1-block long by 1-block deep recessed bay at [X −1 to +1, Z −16, Y = L2] directly beneath the Level L3 workshop floor.
2. Install 3× Large Batteries side-by-side:
   - `MAS Battery 4` at [X −1, Z −16, Y = L2]
   - `MAS Battery 5` at [X0, Z −16, Y = L2]
   - `MAS Battery 6` at [X +1, Z −16, Y = L2]
3. Enclose the battery well perimeter with Heavy Armor Full Blocks. The top faces form the foundation walkway for Level L3.
4. Tie battery terminals into the consist high-voltage tractive bus, supplying instantaneous float charge to the Basic Refinery, Large Assembler, and Axles 4-5 drive motors.
5. Install Small Cargo Container `MAS Hold Munitions` at [X0, Z −15, Y = L2] immediately forward of the central battery bank beneath the workshop corridor. Plumb top conveyor port to the central conveyor line, and enclose the lateral and lower faces within 40× heavy armor ballast plates. This establishes an isolated, flash-shielded munitions magazine preventing sympathetic detonation from penetrating the upper habitable workshop.

##### Terminal Configuration & Sliders
* Batteries: `MAS Battery 4, 5, 6`
  - Initial Mode: `Auto`
  - Max Stored Energy: `3.0 MWh each (9.0 MWh vault total)`
  - Peak Power Output: `12.0 MW each (36.0 MW vault total)`
  - Low Power Threshold: `15%` (Triggers audio alert on Bridge console)
* Cargo Container: `MAS Hold Munitions`
  - Subsystem: Sub-Floor Munitions Bunker
  - Target SML Quotas: 250× NATO 25×184mm boxes, 100× NATO 5.56×45mm magazines
  - Conveyor System: `ON`
* Consist Power Totals: Vault brings total Mastodon MKI energy storage to 18.0 MWh across 6 Large Batteries with 72.0 MW combined instantaneous discharge.

✓ Check: Batteries deliver 36.0 MW peak output; total consist energy reaches 18.0 MWh; battery well and ammo bunker remain fully armored beneath workshop floor plate.

---

#### Step 10.32 · Mobile FOB Field Workshop Core, Medical Bay & Life Support (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: FOB Workshop Core & Life Support  ·  Level: L3                  │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z −18 to −12]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.32 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Assembler | 1 | Slot 4 (`#16A085`) | 39.5 t / 40 | `MAS Utility Assembler` at [X −2, Z −14, L3]; dual inventory doors face aisle |
| Armory Locker | 1 | Slot 7 (`#1B263B`) | 0.8 t / 10 | `MAS Armory Workshop` at [X −2, Z −13, L3]; Port workshop bulkhead facing aisle |
| Medical Room | 1 | Slot 7 (`#1B263B`) | 12.8 t / 30 | `MAS Medical Bay` at [X +2, Z −14, L3] plumbed to O2 life support trunk |
| Cryo Chamber | 2 | Slot 7 (`#1B263B`) | 3.2 t / 40 | `MAS Cryo Pod 1, 2` at [X +2, Z −12 to −13, L3] facing central aisle |
| Large Oxygen Tank | 2 | Slot 4 (`#16A085`) | 6.4 t / 50 | `MAS Tank O2 1, 2` at [X −2, Z −16 to −17, L3-L4]; stores 200,000 L O2 |
| Small Cargo Container | 1 | Slot 4 (`#16A085`) | 0.8 t / 10 | `MAS Hold Ice LifeSupport` at [X0, Z −16, L3]; 15,000 L Quarantined Ice |
| Large Hydrogen Tank | 2 | Slot 6 (`#C0392B`) | 16.2 t / 50 | `MAS Tank H2 1, 2` at [X ±2, Z −18, L3]; stores 10,000,000 L H2 fuel |
| O2/H2 Generator | 2 | Slot 4 (`#16A085`) | 5.4 t / 44 | `MAS Gen O2/H2 1, 2` at [X0, Z −18, L3]; converts ice to fuel and air |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Gangway Aft` at [X0, Z −9, L3] sealing forward workshop bulkhead |

```
Plan view, Y = L3 (Mobile FOB Workshop Core, Medical Bay & Life Support Farm)
        X: −3           X: −2           0           X: +2           X: +3
    Z:  −9 ┌───────────────────────────[DOOR GANGWAY AFT]──────────────────────────┐
    Z: −10 │ FORWARD WORKSHOP ENTRY & FLIGHT LADDERWELL AISLE                      │
    Z: −12 │ [REFINERY L3]             │ [AISLE]    │ [MAS CRYO POD 1]             │
    Z: −13 │ [MAS ARMORY WRK]          │ [AISLE]    │ [MAS CRYO POD 2]             │
    Z: −14 │ [MAS UTILITY ASSEMBLER]   │ [AISLE]    │ [MAS MEDICAL BAY & CHARGER]  │
    Z: −15 │ (Dual Ports Face Aisle)   │ [AISLE]    │ (Trauma Respawn Station)     │
    Z: −16 │ [MAS TANK O2 1]           │ [ICE HOLD] │ [MAS TANK O2 2]              │
    Z: −17 │ (100,000 L O2)            │ (15,000 L) │ (100,000 L O2)               │
    Z: −18 │ [MAS TANK H2 1 (5,000kL)] │ [O2/H2 1,2]│ [MAS TANK H2 2 (5,000kL)]    │
           └───────────────────────────┴────────────┴──────────────────────────────┘
```

##### Placement Instructions
1. Install Sliding Door `MAS Door Gangway Aft` at [X0, Z −9, Y = L3], providing the hermetic barrier between the accordion drawbar gangway and the workshop cabin.
2. Install Large Assembler `MAS Utility Assembler` at [X −2, Z −14, Y = L3] oriented so its dual physical conveyor/inventory doors face the central aisle at X0.
3. Install Large Grid Armory Locker `MAS Armory Workshop` at [X −2, Z −13, Y = L3] on the Port bulkhead, pre-stocked with Tier 4 Elite toolsets, automatic rifles, 5.56mm ammo, and spare suit bottles.
4. Install Medical Room `MAS Medical Bay` at [X +2, Z −14, Y = L3] with conveyor port connected to the life support trunk, equipped with a dedicated suit recharging terminal.
5. Mount 2× Cryo Chambers (`MAS Cryo Pod 1, 2`) at [X +2, Z −12, L3] and [X +2, Z −13, L3] facing inward toward the central walkway.
6. Install 2× Large Oxygen Tanks (`MAS Tank O2 1, 2`) at [X −2, Z −16, L3] and [X +2, Z −16, L3].
7. Place Small Cargo Container `MAS Hold Ice LifeSupport` at [X0, Z −16, Y = L3] directly between the O2 tanks, isolated by a one-way conveyor check to safeguard emergency ice (15,000 L reserve supplying 120+ hours offline).
8. Install 2× Large Hydrogen Tanks (`MAS Tank H2 1, 2`) at [X −2, Z −18, L3] and [X +2, Z −18, L3] with twin O2/H2 Generators (`MAS Gen O2/H2 1, 2`) mounted at [X0, Z −18, L3].

##### Terminal Configuration & Sliders
* Assembler: `MAS Utility Assembler` (Cooperative Mode: `ON` · Repeat: `OFF`)
* Armory Locker: `MAS Armory Workshop` (Pre-stocked: Tier 4 Tools, Rifles, Ammo, O2/H2 Bottles)
* Medical Bay: `MAS Medical Bay` (Respawn: `Enabled` · Healing: `Enabled` · Suit Recharging: `Active`)
* Cryo Chambers: `MAS Cryo Pod 1, 2` (Conveyor Connection: `ON`)
* Oxygen Tanks: `MAS Tank O2 1, 2` (Auto-Refill: `ON` · Stockpile: `OFF` · Total Volume: `200,000 L`)
* Hydrogen Tanks: `MAS Tank H2 1, 2` (Stockpile: `OFF` · Total Volume: `10,000,000 L`)
* Ice Buffer: `MAS Hold Ice LifeSupport` (Custom Name: `[QUARANTINED] MAS Hold Ice LifeSupport`)

✓ Check: Workshop core maintains 100% airtight pressure; assembler inventory accessible from central aisle; armory locker pre-stocked; medical bay and cryo chambers operational; ice buffer quarantined.

---

#### Step 10.33 · Closed-Loop In-Situ Field Metallurgy & Component Recycling Circuit (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Closed-Loop Field Metallurgy & Reclamation  ·  Level: L2-L3    │
│ [VIEWPORT] Grid Zone: [X −3 to 0, Z −17 to −9]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.33 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Basic Refinery | 1 | Slot 4 (`#16A085`) | 13.5 t / 90 | `MAS Field Refinery` at [X −2, Z −12, L3]; top manual hopper accessible from aisle |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS Sorter Ammo Fwd` at [X0, Z −9, L3] pointing forward (+Z) toward Tractor spine |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS Sorter Parts Aft` at [X0, Z −17, L2] pointing aft (−Z) toward workshop |
| Large Conveyor Pipe | 6 | Slot 4 (`#16A085`) | 1.8 t / 6 | Interconnecting refinery, assembler, and sorting raceways |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Ice` at [X0, Z −15, L3] prioritizing raw ice flow to O2/H2 generators |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Ammo` at [X −1, Z −15, L3] monitoring SML munitions quotas & restock |

```
[SALVAGE EXTRACTION & CLOSED-LOOP RECYCLING CIRCUIT]

   [PIRATE WRECK / MINED VEIN]
         │
         ▼ (Laser Multitool Head · Drill or Grinder Beam)
   [MAS LASER 1, 2]
         │
         ▼ (Articulated Boom Conveyor Raceway)
   [CARRIAGE 1 CENTRAL CONVEYOR SPINE]
         │
         ▼ (Drawbar Flexible Inter-Carriage Conduit)
   ┌─────────────────────────────────────────────────────────────────┐
   │ CARRIAGE 2 FOB RECLAMATION CORE                                 │
   │                                                                 │
   │ ┌──────────────────────┐        ┌────────────────────────────┐  │
   │ │ INTACT SPARE PARTS   │───────►│ MAS HOLD COMPONENTS (3×3×3)│  │
   │ └──────────────────────┘        └─────────────┬──────────────┘  │
   │                                               │ (Munitions Pull)│
   │ ┌──────────────────────┐                      ▼                 │
   │ │ DAMAGED SCRAP METAL  │──►[MAS FIELD REFINERY]                 │
   │ └──────────────────────┘   (330 kW Iron Smelter)                │
   │                                    │ (Ingots)                   │
   │ ┌──────────────────────┐           ▼                            │
   │ │ OBSOLETE COMPONENTS  │──►[MAS UTILITY ASSEMBLER]              │
   │ └──────────────────────┘   (Disassemble / Ammo Fabrication)     │
   │                                    │                            │
   │                                    ▼                            │
   │                            [MAS SORTER AMMO FWD] ──► (CIWS Guns)│
   └─────────────────────────────────────────────────────────────────┘
```

##### Placement Instructions
1. Install Basic Refinery `MAS Field Refinery` at [X −2, Z −12, Y = L3] immediately forward of the Large Assembler. Plumb bottom and side conveyor ports to the central conveyor spine. Ensure top hatch faces inward toward the aisle for manual batch dumping.
2. Install Large Conveyor Sorter `MAS Sorter Ammo Fwd` at [X0, Z −9, Y = L3] oriented pointing forward (+Z) directly into the gangway drawbar conduit. Configure whitelist for NATO 25×184mm and 5.56mm munitions with Drain All = ON to keep Tractor turrets continuously supplied.
3. Install Large Conveyor Sorter `MAS Sorter Parts Aft` at [X0, Z −17, Y = L2] oriented pointing aft (−Z). Configure whitelist for Scrap Metal and damaged salvage components with Drain All = ON, diverting incoming salvage directly into `MAS Field Refinery` before it reaches raw ore bins.
4. Mount Event Controller `MAS EC Ice` at [X0, Z −15, Y = L3] on the central bulkhead.
5. Mount Event Controller `MAS EC Ammo` at [X −1, Z −15, Y = L3] on the port workshop bulkhead adjacent to `MAS Utility Assembler`. Configure to monitor ammunition inventory across `MAS Hold Munitions` and `MAS Hold Components`. When combined NATO 25×184mm ammunition drops below 60% of quota (150 boxes / 21,000 rounds) or NATO 5.56×45mm magazines drop below 60% of quota (60 magazines / 1,800 rounds), trigger automated production queue in `MAS Utility Assembler` at maximum priority.

##### Terminal Configuration & Sliders
* Basic Refinery: `MAS Field Refinery` (Use Conveyor System: `ON` · Smelting: Fe, Ni, Co, Si, Mg)
* Sorter: `MAS Sorter Ammo Fwd` (Mode: `Whitelist` · Items: `NATO_25x184mm`, `NATO_5_56x45mm` · Drain All: `ON`)
* Sorter: `MAS Sorter Parts Aft` (Mode: `Whitelist` · Items: `Scrap` · Drain All: `ON`)
* Event Controller: `MAS EC Ice`
  - Monitored Target: `MAS Tank H2 1, 2` (Hydrogen Fill Level)
  - Condition: Fill $< 90\%$
  - Action Slot 1: Enable Ice Sorter & Power ON `MAS Gen O2/H2 1, 2`
  - Action Slot 2: Standby mode when tanks $\ge 95\%$
* Event Controller: `MAS EC Ammo`
  - Monitored Target: `MAS Hold Munitions` + `MAS Hold Components` (Munitions Quotas)
  - Condition: Ammo Inventory $< 60\%$ (25mm $< 150$ boxes OR 5.56mm $< 60$ mags)
  - Action Slot 1: Trigger Assembler Ammo Fabrication Queue (Mg + Fe + Ni ingots)
  - Action Slot 2: If local Magnesium $< 50\text{ kg}$ or Ammo $< 25\%$, broadcast `TMS.RESUPPLY.REQUEST;MAS-01;KIT_MUNITIONS`

✓ Check: Mined raw ore automatically smelts into ingots; scrap metal routes straight to refinery; assembler pulls ingots autonomously; forward sorter keeps bow and flank turrets fully loaded with ammo.

---

#### Step 10.34 · Vertical Flight Deck & Stern Dump Dual-Door Decon Airlocks (L2-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Vertical Egress & Decon Airlocks  ·  Level: L2-L6               │
│ [VIEWPORT] Grid Zone: [X +2, Z −25 to −10]  ·  Profile view facing Port (▲ Zenith +Y)│
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.34 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Narrow Ladder Block | 6 | Slot 9 (`#F1C40F`) | 0.6 t / 6 | 4× ladder segments at [X +2, Z −10, L3-L5]; 2× segments at [X +2, Z −25, L2-L3] |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Flight Deck Inner` at [X +2, Z −10, L5], sealing workshop ascent |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Roof Outer` at [X +2, Z −10, L6], outer flight deck hatch seal |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Stern Inner` at [X +2, Z −25, L3], sealing workshop aft bulkhead |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Airlock Roof` at [X +2, Z −10, L5], active gas reclamation |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Airlock Stern` at [X +2, Z −25, L3], active gas reclamation |
| Interior Light | 2 | Slot 9 (`#F1C40F`) | 0.04 t / 50 | `MAS Light Decon Roof` [X +2, Z −10, L5], `MAS Light Decon Stern` [X +2, Z −25, L3] (Cyan `#00FFFF`, UV Decontamination) |
| Event Controller | 2 | Slot 7 (`#1B263B`) | 0.2 t / 50 | `MAS EC Airlock Roof` [X +2, Z −10, L5], `MAS EC Airlock Stern` [X +2, Z −25, L3] |
| Sci-Fi 4-Button Panel | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 16 | `MAS BP Flight Deck` at [X +2, Z −10, L6]; `MAS BP Stern Dump` at [X +2, Z −25, L2] |

```
Profile elevation looking Starboard, X = +2 (Dual Vertical Airlock Circulation)
       Z: −10 (Forward Flight Deck Ascent)             Z: −25 (Aft Stern Dump Descent)
  L6   [MAS DOOR ROOF OUTER] ── [MAS BP FLIGHT DECK]   ═══════════════════════════════
       │                                                                │
  L5   [MAS DOOR FLIGHT INNER] · [VENT ROOF] · [UV]    ═══════════════════════════════
       │ (Vertical Ladderwell Trunk)                                    │
  L4   │                                               ═══════════════════════════════
       │                                                                │
  L3   [WORKSHOP ENTRY THRESHOLD]                      [MAS DOOR STERN INNER] · [VENT] · [UV]
                                                       │ (Vertical Ladderwell Trunk)
  L2   ═══════════════════════════════════════════════ [STERN EGRESS THRESHOLD] ── [MAS BP]
```

##### Placement Instructions
1. Install an enclosed vertical ladder trunk at [X +2, Z −10] ascending from the forward workshop floor (Level L3) through Level L4 up to Level L5.
2. Install inner Sliding Door `MAS Door Flight Deck Inner` at Level L5. Frame the upper chamber at Level L6 with outer Sliding Door `MAS Door Roof Outer`, forming a true hermetic dual-door airlock chamber.
3. Mount Air Vent `MAS Vent Airlock Roof`, UV Decon Light `MAS Light Decon Roof`, and Event Controller `MAS EC Airlock Roof` inside the Level L5 chamber. Mount button panel `MAS BP Flight Deck` on the exterior gantry coaming at Level L6.
4. Install a descending ladder trunk at [X +2, Z −25] descending from Level L3 down to Level L2 through inner Sliding Door `MAS Door Stern Inner`.
5. Install Air Vent `MAS Vent Airlock Stern`, UV Decon Light `MAS Light Decon Stern`, and Event Controller `MAS EC Airlock Stern` at Level L3 inside the descent vestibule. Mount button panel `MAS BP Stern Dump` at Level L2 adjacent to the rear dump bumper.

##### Terminal Configuration & Sliders
* Doors: `MAS Door Flight Deck Inner`, `MAS Door Roof Outer`, `MAS Door Stern Inner` (Auto-Close: `ON` · Delay: `2.0 s`)
* Vents: `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern` (Active Gas Reclamation: `Depressurize = ON` before outer doors open; `OFF` before inner doors open)
* Decon Lights: `MAS Light Decon Roof`, `MAS Light Decon Stern` (Color: `#00FFFF` Cyan · Radius: `3.0 m` · 3.0 s pulse on inbound cycle)
* Event Controllers: `MAS EC Airlock Roof`, `MAS EC Airlock Stern`
  - Manages dual-door interlock: prevents both doors opening concurrently.
  - On outbound egress: evacuates chamber O2 into main tanks to 0.0 atm before outer hatch unlocks.
  - On inbound ingress: flashes cyan UV decon light for 3.0 s and pressurizes chamber to 100% before inner door unseals.
* Button Panels:
  - `MAS BP Flight Deck`: Btn 1 = Cycle Flight Deck Hatch · Btn 2 = Toggle Roof Floodlights · Btn 3 = Mule Cradle Clamp · Btn 4 = Emergency Suit Recharge
  - `MAS BP Stern Dump`: Btn 1 = Cycle Stern Hatch · Btn 2 = Toggle Stern Sorter Drain · Btn 3 = Lock/Unlock Stern Conn · Btn 4 = Decon Cycle

✓ Check: Engineer ascends to Level L6 flight deck without venting workshop atmosphere; stern airlock provides safe descent to Level L2 dump bumper in vacuum; zero oxygen lost during cycling.

---

#### Step 10.35 · Ambient Atmospheric Oxygen Scavenging Vents & Multi-Atmosphere Control (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Universal Life Support & Scavenging  ·  Level: L4              │
│ [VIEWPORT] Grid Zone: [X ±3, Z −16]  ·  Flank profile looking Inward (▲ Zenith +Y)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.35 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Air Vent | 2 | Slot 0 (`#2B303A`) | 1.0 t / 50 | `MAS Vent Atmo Intake Port` at [X −3, Z −16, L4]; `MAS Vent Atmo Intake Stbd` at [X +3, Z −16, L4] |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Atmosphere Mode` at [X0, Z −15, L3] monitoring external atmospheric O2 |
| Heavy Armor Slope | 4 | Slot 0 (`#2B303A`) | 3.4 t / 4 | Recessed protective cowl surrounding exterior intake louvers |

```
Flank elevation cross-section, Y = L4, Z = −16 (Exterior Scavenging Vent Recesses)
    Port Flank (X −3)                                               Starboard Flank (X +3)
    [EXTERIOR HULL]                                                 [EXTERIOR HULL]
          │                                                               │
    ┌─────┴──────────────────┐                               ┌────────────┴─────────────┐
    │ [MAS VENT ATMO PORT]   │ ◄── Depressurize: ON          │ [MAS VENT ATMO STBD]     │
    │ (Recessed Flush Vent)  │     Draws Ambient O2          │ (Recessed Flush Vent)    │
    └─────┬──────────────────┘     Free Ice-Free Life Support└────────────┬─────────────┘
          │                                                               │
          └────────────────────────► [MAS TANK O2 1, 2] ◄─────────────────┘
                                   (200,000 L O2 Bunkers)
```

##### Placement Instructions
1. Cut flush recesses into the Level L4 exterior armor flanks at [X −3, Z −16] (Port) and [X +3, Z −16] (Starboard).
2. Install Air Vents `MAS Vent Atmo Intake Port` and `MAS Vent Atmo Intake Stbd` facing outward toward the planetary environment, plumbed directly into the utility oxygen line.
3. Frame vent openings with Heavy Armor Slopes to protect delicate intake louvers from ballistic ricochets and tree branch strikes during transit.
4. Install Event Controller `MAS EC Atmosphere Mode` at [X0, Z −15, Y = L3] inside the central life support bay.

##### Terminal Configuration & Sliders
* Air Vents: `MAS Vent Atmo Intake Port`, `MAS Vent Atmo Intake Stbd`
  - Depressurize: `ON`
  - Power: `ON` (in breathable atmospheres)
  - Parasitic Draw: `30 kW each`
* Event Controller: `MAS EC Atmosphere Mode`
  - Monitored Event: `Natural Gravity & Atmospheric Oxygen`
  - Target: Ambient Atmosphere
  - Condition: Breathable O2 $\ge 1.0\%$
  - Slot 1 (Breathable Atmosphere):
    - `MAS Vent Atmo Intake Port` -> `Power ON`
    - `MAS Vent Atmo Intake Stbd` -> `Power ON`
    - `MAS Gen O2/H2 1, 2` -> `Power OFF` (Saves ice reserves)
  - Slot 2 (Vacuum / Toxic World):
    - `MAS Vent Atmo Intake Port` -> `Power OFF`
    - `MAS Vent Atmo Intake Stbd` -> `Power OFF`
    - Compartment Doors -> `Auto-Seal ON`
    - Life Support -> Switched to 100% closed-loop tank draw (`MAS Tank O2 1, 2`)

✓ Check: In planetary atmosphere, vents pull free oxygen to refill 200,000 L O2 storage with zero ice consumption; in vacuum, vents automatically power down and hermetic seals isolate all 4 interior zones.

---

#### Step 10.35B · Non-Earthlike Planetary Environmental Hardening, Radiation Citadel & Faraday Lightning Mast (L3-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Planetary Hazard Hardening & Radiation Citadel  ·  Level: L3-L6 │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z −18 to +6]  ·  Consist Profile & Plan View      │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.35B Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Decoy Block | 1 | Slot 0 (`#2B303A`) | 0.5 t / 170 | `MAS Decoy Mast` at [X0, Z +6, L6], dorsal lightning diverter mast |
| Interior Light | 1 | Slot 9 (`#F1C40F`) | 0.02 t / 25 | `MAS Light Decon Gangway` at [X0, Z −8, L4] (Cyan `#00FFFF`, UV Decontamination) |

```
[PLANETARY ENVIRONMENTAL HARDENING & RADIATION CITADEL SCHEMATIC]

              [DORSAL LIGHTNING MAST · MAS DECOY MAST] (Level L6 · X0, Z +6)
              (Draws 100% Planetary Lightning Strikes Away From Bridge & H2 Tanks)
                                      │
            ┌─────────────────────────┴─────────────────────────┐
            │ Grounding Conductor: Heavy Armor Backbone Spine   │
            │ Discharges harmlessly into terrain via iron skids │
            └───────────────────────────────────────────────────┘

     CARRIAGE 2 FOB RADIATION CITADEL & STORM SHELTER (Level L3 · Z −13 to −18)
     ┌────────────────────────────────────────────────────────────────────────┐
     │ Port Flank Shielding:    5.0 ML Liquid Hydrogen Tank 1 (4.5 m liquid)  │
     │ Starboard Flank Shield:  5.0 ML Liquid Hydrogen Tank 2 (4.5 m liquid)  │
     │ Deck Floor Shielding:    40× Sub-Floor Heavy Armor Ballast Skids       │
     │ Ceiling Overhead Shield: Mule Pad Heavy Armor Blast Plates             │
     │                                                                        │
     │ [CREW CITADEL CORRIDOR] ── Zero-Dose Safe Haven during Solar Storms   │
     └────────────────────────────────────────────────────────────────────────┘

     CRYOGENIC THERMAL FREEZE PROTECTION JACKET (Level L3 · Z −16)
     ┌────────────────────────────────────────────────────────────────────────┐
     │ 330 kW Basic Refinery Waste Heat Exchanger Jacketing                   │
     │ Hydronic thermal trace circulating to `MAS Hold Ice LifeSupport`       │
     │ Prevents emergency ice buffer freeze-lock at sub-zero cryogenic temps  │
     └────────────────────────────────────────────────────────────────────────┘
```

##### Placement Instructions
1. Install Decoy Block `MAS Decoy Mast` on the dorsal locomotive crown at [X0, Z +6, Y = L6], forming an elevated Faraday lightning rod. This structural decoy intercepts violent atmospheric lightning strikes on Pertam and Alien, conducting current safely through the heavy armor spine down to the grounded wheel hubs and away from the glass canopy and hydrogen fuel cells.
2. Formalize the central workshop aisle between Z −13 and Z −18 (Level L3) as the designated Radiation Storm Citadel. Enclosed symmetrically between the two 4.5 m liquid hydrogen tanks (`MAS Tank H2 1, 2`) and buffered below by 40× heavy steel ballast plates, this corridor provides complete cosmic ray, solar proton storm, and coronal mass ejection (CME) attenuation for the crew.
3. Plumb hydronic thermal trace lines from `MAS Field Refinery` (330 kW heat rejection) to jacket `MAS Hold Ice LifeSupport` at [X0, Z −16, L3]. This waste heat loop prevents emergency ice crystallization and freezing in sub-cryogenic planetary environments (Europa, Triton).
4. Install UV Decon Light `MAS Light Decon Gangway` in the inter-carriage flexible accordion gangway at [X0, Z −8, L4] adjacent to `MAS Vent Gangway`, providing active decontamination and isolation between the tractor locomotive and utility carriage.
5. Hardwire perimeter CIWS turrets (`MAS Turret Bow / Stern`) with `Target Meteors = ON` to provide automated planetary meteor storm intercept.
6. Configure Event Controller `MAS EC Meteor` (integrated in the C2 avionics rack at [X0, Z +7, L3]) monitoring target acquisition across `MAS Turret Bow` and `MAS Turret Stern`. Upon detecting incoming meteors or hostile target lock:
   - Sounds high-decibel early warning siren `MAS Alarm Meteor` ([X0, Z +8, L4]).
   - Issues emergency `STOW` command to `[MAS] PB Excavator`, swinging the 35 m extraction boom 180° aft into the armored spine trough and locking into `MAS Clamp Boom Stow` ([X0, Z −7, L4]) in $< 4.0\text{ s}$, dropping consist profile to 5.2 m and shielding delicate joint actuators from kinetic fragmentation.
   - Retracts crew boarding ladder (`MAS Crew Ladder Hinge` to 0° flush).
   - Locks all airlock outer hatches (`MAS Door Boarding Outer`, `MAS Door Roof Outer`, `MAS Door Stern Inner`).
   - Engages magnetic clamp lockdown across flight deck rub rails (`MAS MP Clamps` and Kestrel landing gears) to isolate docked drones from kinetic ground shock.
7. Configure high-speed automated ammunition distribution: Large Conveyor Sorter `MAS Sorter Ammo Fwd` ([X0, Z −9, L3]) pulls continuous belts of NATO 25×184mm and 5.56mm ammo from sub-floor bunker `MAS Hold Munitions` ([X0, Z −15, L2]) and `MAS Hold Components` into forward CIWS guns. In the event of depleted local magnesium ($< 50\text{ kg}$) or ammunition reserves below 25%, `[MAS] PB RIG//OS-Lite` autonomously broadcasts `TMS.RESUPPLY.REQUEST;MAS-01;KIT_MUNITIONS`, summoning MULE-01 or Ground Base for a rapid top-deck munitions pallet transfer in $< 10\text{ s}$.

##### Terminal Configuration & Sliders
* Lightning Mast: `MAS Decoy Mast` (Functional: `ON` · Target Priority: High)
* Gangway Vent: `MAS Vent Gangway` (Depressurize: `OFF` · Maintains positive pressure in bellows)
* Gangway Decon Light: `MAS Light Decon Gangway` (Color: `#00FFFF` Cyan · Radius: `3.0 m`)
* CIWS Turrets: `MAS Turret Bow`, `MAS Turret Stern`
  - Target Meteors: `ON`
  - Target Missiles: `ON`
  - Target Small Ships / Characters: `ON`
* Siren: `MAS Alarm Meteor` (Sound: `Alert 2` · Range: `150 m` · Volume: `100%`)
* Event Controller: `MAS EC Meteor` (Automated Meteor Defense & Consist Hardening)
  - Monitored Target: `MAS Turret Bow`, `MAS Turret Stern` (Target Lock / Meteor Acquisition)
  - Condition: Target Detected (Meteors / Missiles / Enemy Grids)
  - Action Slot 1: Trigger `MAS Alarm Meteor` (Play Sound) · Issue `PB Excavator` Argument: `STOW` · Group `MAS Doors` Close & Lock · Group `MAS MP Clamps` Lock · `MAS Crew Ladder Hinge` Retract (0°)
  - Action Slot 2: On Clear (No Targets): Silence `MAS Alarm Meteor` · Restore Standard Posture
* Environmental Watchdog: `MAS EC Atmosphere Mode` (Toxic Atmosphere Protection):
  - On detecting toxic/corrosive gas or zero O2: instantly locks exterior vents, prevents toxic gas ingress, and locks cabin doors to 100% closed-loop recirculation.

✓ Check: Lightning strikes safely arc to dorsal mast decoy without damage to bridge glass or fuel tanks; crew sheltered in radiation citadel during solar storm; ice buffer jacketed against cryogenic freeze; gangway decon cycle active; meteor defense armed with early warning siren and automated consist hardening.

---

#### Step 10.36 · Genesis Master Projector, Standalone FOB C2 & Field Component Hold (L3-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Master Blueprint Projector & FOB C2  ·  Level: L3-L4           │
│ [VIEWPORT] Grid Zone: [X −1 to +1, Z −22 to −13]  ·  Plan view Y = L3 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.36 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Cargo Container | 1 | Slot 5 (`#27AE60`) | 23.4 t / 50 | `MAS Hold Components` at [X0, Z −22, L3-L4] (3×3×3 B); terminal port faces aisle |
| Projector (Large Grid) | 1 | Slot 7 (`#1B263B`) | 2.4 t / 100 | `MAS Projector Master` in workshop ceiling at [X0, Z −14, L4] facing forward (+Z) |
| Programmable Block | 1 | Slot 7 (`#1B263B`) | 0.8 t / 25 | `[MAS-U] PB Utility` at [X −1, Z −13, L3] running autonomous FOB watchkeeping logic |
| Remote Control | 1 | Slot 7 (`#1B263B`) | 0.6 t / 25 | `MAS-U Remote FOB` at [X +1, Z −13, L3] facing forward (+Z) for uncrewed remote control |

```
Plan view, Y = L3 (Aft Component Hold, Genesis Projector & Autonomous FOB Core)
        X: −2           X: −1             0             X: +1           X: +2
    Z: −13 ┌───────────────┬───────────────╤───────────────┬───────────────┐
           │ [ASSEMBLER]   │ [MAS-U PB UT] │ [AISLE]       │ [MAS-U REMOTE]│
    Z: −14 │ (Dual Ports)  ├───────────────┴───────────────┤ (Medical Bay) │
           │               │ [MAS PROJECTOR MASTER (L4)]   │               │  ◄── Ceiling Ghost Projection
    Z: −16 ├───────────────┼───────────────────────────────┼───────────────┤
           │ [O2 TANK 1]   │ [QUARANTINED ICE HOLD (15kL)] │ [O2 TANK 2]   │
    Z: −18 ├───────────────┼───────────────────────────────┼───────────────┤
           │ [H2 TANK 1]   │ [CONVEYOR RISER JUNCTION]     │ [H2 TANK 2]   │
    Z: −19 ├───────────────┴───────────────────────────────┴───────────────┤
    Z: −20 │                                                               │
    Z: −22 │             [MAS HOLD COMPONENTS (3×3×3 LARGE CARGO)]         │  ◄── Spare Construction Parts
    Z: −24 │             (Palette Slot 5: Logistics Emerald)               │      Pulls Salvage & Welder Stock
    Z: −25 └───────────────────────────────────────────────────────────────┘
```

##### Placement Instructions
1. Install Large Cargo Container `MAS Hold Components` (3×3×3 Large Grid) centered at [X0, Z −22, Y = L3-L4]. Ensure front lower conveyor port and physical terminal access door face forward into the central workshop aisle at [X0, Z −20, Y = L3].
2. Mount Large Grid Projector `MAS Projector Master` flush inside the Level L4 ceiling at [X0, Z −14, Y = L4] with top facing zenith (+Y) and front facing forward (+Z).
3. Load the complete 1,039-block reference blueprint of `MASTODON-01 MKI` into `MAS Projector Master`. Set coordinate offsets to exactly X = 0, Y = 0, Z = 0, aligning the holographic ghost projection 1:1 with the physical chassis.
4. Mount Programmable Block `[MAS-U] PB Utility` at [X −1, Z −13, Y = L3].
5. Mount Remote Control `MAS-U Remote FOB` at [X +1, Z −13, Y = L3] aligned facing forward (+Z).

##### Terminal Configuration & Sliders
* Projector: `MAS Projector Master`
  - Pitch: `0.0°` · Roll: `0.0°` · Yaw: `0.0°`
  - Offset X: `0` · Offset Y: `0` · Offset Z: `0`
  - Show Only Buildable: `ON`
* Autonomous FOB C2 (`[MAS-U] PB Utility` / `MAS-U Remote FOB`):
  - Standalone Drop-and-Mine Decouple: Upon severance or hitch uncoupling, `[MAS-U] PB Utility` immediately assumes local command authority.
  - Broadcasts regional status `RIG.C2.HEARTBEAT;ORIGIN=MAS-01-U;ROLE=FOB`.
  - Coordinates automated workshop manufacturing, life support recycling, battery management, and rear CIWS defense independently from the Tractor.

##### Pre-Enclosure Inspection Gate 10.5A: Field Workshop Fabrication, Sub-Floor Batteries & Circulation Check
* [ ] Confirm Utility sub-floor battery bank (`MAS Battery 4, 5, 6`) delivers 9.0 MWh storage and 36.0 MW discharge.
* [ ] Verify `MAS Field Refinery` ingests raw mined ore and deposits refined ingots into `MAS Utility Assembler`.
* [ ] Verify `MAS Utility Assembler` pulls ingots and queues components without manual transfer.
* [ ] Confirm physical cargo access ports on Basic Refinery, Assembler, and Component Hold allow manual inventory access.
* [ ] Confirm `MAS Tank O2 1, 2` store 200,000 L O2 and maintain 100% pressure in all 4 hermetic zones.
* [ ] Verify active gas-reclamation airlock cycle recovers 100% of room O2 before outer doors open to vacuum.
* [ ] Confirm `MAS Projector Master` projects aligned ghost blueprint of Mastodon consist for field self-repair.
* [ ] Confirm `MAS Hold Ice LifeSupport` remains quarantined from bulk ore dumps.
* [ ] Verify vertical flight ladderwell provides unobstructed ascent to Level L6 Flight Deck.
* [ ] Confirm stern ladder provides safe descent to Level L2 dump bumper.
* [ ] Confirm crew medical bay and twin cryo pods allow successful respawn, healing, and hibernation.
* [ ] Confirm O2/H2 tanks maintain airtight seals and supply hydrogen lines.
* [ ] Verify `MAS EC Ice` automatically feeds ice to O2/H2 generators when H2 tanks drop below 90%.
* [ ] Verify ambient air vents (`MAS Vent Atmo Intake Port/Stbd`) pull oxygen from atmosphere when Depressurize = ON.
* [ ] Confirm `[MAS-U] PB Utility` boots into standalone FOB mode upon hitch decouple.
* [ ] Confirm `MAS-U EC Breakaway` engages deadman brake and drops keel skids on signal loss.
* [ ] Verify `MAS Sorter Ammo Fwd` draws ammunition to forward turrets without manual handling.
* [ ] Verify `MAS Sorter Parts Aft` routes scrap metal directly into `MAS Field Refinery`.
* [ ] Verify `MAS MT Turret Port` and `MAS MT Turret Stbd` auto-repair damaged blocks on the drawbar hitch and Kestrel landing pads.
* [ ] Verify `[MAS-U] PB Utility` detects component deficit in `MAS Hold Components` and queues production deltas in `MAS Utility Assembler`.

---

#### Step 10.36B · Twin Flank Multitool Repair Turrets & SML Auto-Production Queuing (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Consist Self-Repair & Drone Servicing  ·  Level: L4             │
│ [VIEWPORT] Grid Zone: Forward Shoulders [X ±3, Z −10, L4]  ·  Plan view (▲ Bow +Z)   │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.36B Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Laser Multitool Turret | 2 | Slot 3 (`#E07A5F`) | 9.0 t / 450 | `MAS MT Turret Port/Stbd` on forward shoulders at [X ±3, Z −10, L4] · Mod 3175774286 |
| Conveyor Junction | 2 | Slot 4 (`#16A085`) | 1.8 t / 2 | Sub-deck risers at [X ±3, Z −10, L3] tying into workshop conveyor spine |

```
Plan view, Y = L4 (Carriage 2 Forward Shoulder Repair Turrets & SML Queuing Core)
        X: −3           X: −1             0             X: +1           X: +3
    Z:  −9 ┌───────────────┬───────────────────────────────┬───────────────┐
           │ [PORT SHOULDER│ ════════ [BULKHEAD DOOR] ════ │ [STBD SHOULDER│
    Z: −10 │ [MAS MT PORT] │ [AISLE WALKWAY]               │ [MAS MT STBD] │  ◄── Forward Flank Overlap
           ├───────────────┼───────────────────────────────┼───────────────┤      (Covers Hitch & Kestrels)
    Z: −11 │ [KESTREL PAD] │ [CONVEYOR RISER JUNCTION]     │ [KESTREL PAD] │  ◄── Drone Landing Deck (L5)
           └───────────────┴───────────────────────────────┴───────────────┘
```

##### Placement Instructions
1. Install 2 Conveyor Junctions at [X −3, Z −10, Y = L3] and [X +3, Z −10, Y = L3], tapping into the utility workshop conveyor trunk lines.
2. Mount Large Grid Laser Multitool Turrets `MAS MT Turret Port` at [X −3, Z −10, Y = L4] and `MAS MT Turret Stbd` at [X +3, Z −10, Y = L4] flush on the reinforced shoulder sponsons of Carriage 2.
3. Verify unobstructed 270° hemispherical lines of sight: turrets sweep forward across the 3-axis drawbar hitch (Z −8), inward across the tractor rear armor and power vault, and upward to service docked KESTREL-01 drones on their Level L5 landing pads at [X ±3, Z −11, L5].
4. Plumb internal conveyor lines back to `MAS Hold Components` and `MAS Field Refinery` / `MAS Utility Assembler`.

##### Terminal Configuration & Sliders
* Group: `MAS Multitool Turrets` (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`)
* Multitool Turrets: `MAS MT Turret Port`, `MAS MT Turret Stbd`
  * Default Mode: `Welder` (Field self-repair and drone maintenance)
  * Secondary Mode: `Grinder` (Debris clearance and obstacle salvage)
  * Range: `50 m` · Target Friendly: `ON` · Target Neutrals: `ON` · Auto-Repair: `ON`
* SML Auto-Production Queuing (`[MAS-U] PB Utility`):
  * Baseline SML Quotas: 1,500 Steel Plates, 500 Construction Comps, 200 Motors, 100 NATO 25×184mm ammo boxes, 100 Interior Plates, 50 Large Steel Tubes, 100 Small Steel Tubes, 50 Computers, 50 Displays, 20 Radio-comm Comps.
  * Threshold Watchdog: Whenever inventory in `MAS Hold Components` drops below 75% of quota, `[MAS-U] PB Utility` queues production deltas in `MAS Utility Assembler`.
  * Ingot Feed: `MAS Field Refinery` processes mined ore directly into iron/nickel/silicon ingots, sustaining closed-loop field manufacturing without supply runs.

```
PHASE 10-5 VISUAL MILESTONE · EXPEDITIONARY WORKSHOP & FOB CORE COMPLETE
       Z: −10 (Twin Flank MT Turrets)    Z: −14 (Refinery/Assembler)   Z: −22 (Hold & Projector)
       [MAS MT TURRET PORT / STBD]       [METALLURGY & LIFE SUPPORT]   [3×3×3 COMPONENT HOLD]
                 │                                    │                          │
       ══════════╧════════════════════════════════════╧══════════════════════════╧═════════
                    9.0 MWh SUB-FLOOR BATTERY VAULT & KEEL BALLAST (L1-L2)
```

---

### Phase 10-6 · Flight Decks & Logistics Offload Interfaces (Levels L5-L6)

```
       [PHASE 10-6 MASTER SPATIAL LOCATOR HUD]
       Envelope: 9 W × 18 L × 3 H B (22.5 × 45.0 × 7.5 m)
       Coordinate Range: X −4 to +4, Z −26 to −8, Y = L5 to L6
       Deck Datums:
       · Twin Kestrel Scout Drone Landing Pads: [X ±3, Z −12, Y = L5]
       · Recessed Drone Maintenance Welders: [X ±3, Z −12, Y = L4]
       · 7×7 B Mule Gantry Blast Deck: [X0, Z −22, Y = L6]
       · Mule Rub-Rail Clamping Arms: [X ±3, Z −22, Y = L6]
       · Flush Solar Array: [X ±4, Z −24 to −20, Y = L6]
       · Stern Bumper & Caterpillar Dump Connector: [X0, Z −26, Y = L2]
       · Stern CIWS Defense Turret: [X0, Z −26, Y = L5]
```

#### Pre-Step Master Parts Tray: Phase 10-6
* 49× Heavy Armor Full Block (Palette 1: Hazard Amber · Mule Blast Deck)
* 4× Large Grid Solar Panel (`MAS Solar 1..4`, 1×4 B, Palette 0)
* 18× Catwalk with Railing Block (Palette 9: High-Viz Safety Yellow)
* 10× Grated Catwalk Block (Palette 9: High-Viz Safety Yellow)
* 2× Motor Hinge + 2× Large Magnetic Plate (Palette 1: Cradle Clamping Arms)
* 1× Large Connector (`MAS MP Conn`, Palette 4: Processing Teal)
* 2× Large Conveyor Sorter (`MAS MP Sorter`, `MAS Stern Sorter`, Palette 4)
* 2× Small-Grid Docking Connector (`MAS Kestrel Dock 1, 2`, Palette 8: Scout Flight White)
* 2× Large Grid Ship Welder (`MAS Welder Pad Kestrel 1, 2`, Palette 4: Processing Teal)
* 4× Large Grid Spotlight (`MAS Floodlight Tower 1..4`, Palette 7: Fleet Command Navy)
* 1× Large Grid Piston (`MAS Piston Stern Dock`, Palette 2: Rail Crimson)
* 1× Large Connector (`MAS Stern Dump Conn`, Palette 2: Rail Crimson)
* 2× Large Grid Piston (`MAS Outrigger Utility Port/Stbd`, Palette 0)
* 2× Large Magnetic Plate (`MAS Seismic Pad Rear Port/Stbd`, Palette 0)
* 1× Large Conveyor Sorter + 4× Conveyor Ejector (`MAS Sorter Tailings Stern`, Palette 0)
* 1× Large Gatling Turret (`MAS Turret Stern`, Palette 7: Fleet Command Navy)

---

#### Step 10.37 · Roof Mule Receiving Gantry Cradle Pad (L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Mule Receiving Gantry Cradle & Sorter  ·  Level: L6             │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z −25 to −19]  ·  Plan view Y = L6 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.37 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 49 | Slot 1 (`#E67E22`) | 41.6 t / 49 | 7×7 B blast deck spanning X −3 to +3, Z −25 to −19 at Level L6 |
| Large Connector | 1 | Slot 4 (`#16A085`) | 4.1 t / 25 | `MAS MP Conn` flush with deck at [X0, Z −22, L6] facing zenith (+Y) |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS MP Sorter` at [X0, Z −22, L5] pointing upward (+Y) into connector |
| Motor Hinge | 2 | Slot 1 (`#E67E22`) | 4.2 t / 200 | `MAS MP Clamp Hinge Port/Stbd` at [X ±3, Z −22, L6] base facing inward |
| Magnetic Plate (Large) | 2 | Slot 1 (`#E67E22`) | 5.8 t / 50 | `MAS MP Clamp Plate Port/Stbd` mounted on hinge heads |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC MP` at [X0, Z −21, L5] managing clamping sequence |

```
2.5D Isometric Callout · Mule Gantry Cradle on Utility Roof (L6)
                         [MULE-01 HULL 7×7 B]
                                  │
      Port Flank                  ▼               Starboard Flank
    [CLAMP RUB RAIL]                        [CLAMP RUB RAIL]
           ▲                                       ▲
           │                                       │
      [MAG PLATE]  ◄── 30° Swing Inward ──►  [MAG PLATE]
           │                                       │
      [MOTOR HINGE]                           [MOTOR HINGE]
      (Base: X −3, Z −22)                     (Base: X +3, Z −22)
    ═══════╧═══════════════════════════════════════╧═══════
                      FLUSH HEAVY BLAST ROOF (L6)
                             [MAS MP Conn]
```

```
Plan view, Y = L6 (7×7 B Mule Gantry Blast Deck)
        X: −3           0          +3
    Z: −19 ■════════════╤═══════════■
           ║  Hazard    │  Hazard   ║  Palette Slot 1: Hazard Amber
    Z: −22 [CLAMP P] [MAS MP CONN] [CLAMP S]  ◄── Motorized Clamping Hinges
           ║  Chevrons  │  Chevrons ║
    Z: −25 ■════════════╧═══════════■
```

##### Placement Instructions
1. Lay a 7×7 block square of Heavy Armor Full Blocks painted Palette Slot 1 (Hazard Amber) from X = −3 to +3 and Z = −25 to −19 at Level L6, forming the heavy landing deck.
2. Cut the center block at [X0, Z −22, Y = L6] and install Large Connector `MAS MP Conn` flush with the deck surface facing upward (+Y).
3. Directly beneath the connector at Level L5, plumb Large Conveyor Sorter `MAS MP Sorter` pointing upward (+Y) into the connector. Configure whitelist for rare ores (Uranium, Platinum, Gold, Magnesium).
4. Mount Motor Hinges `MAS MP Clamp Hinge Port` at [X −3, Z −22, Y = L6] and `MAS MP Clamp Hinge Stbd` at [X +3, Z −22, Y = L6] with bases anchored to the exterior gantry coaming and hinge heads swinging inward toward X0.
5. Weld Large Magnetic Plates `MAS MP Clamp Plate Port/Stbd` onto the hinge heads.
6. Mount Event Controller `MAS EC MP` in the sub-deck cavity at [X0, Z −21, Y = L5].

##### Terminal Configuration & Sliders
* Connector: `MAS MP Conn` (Auto-Connect: `OFF` · Collect All: `OFF` · Strength: `0.015`)
* Conveyor Sorter: `MAS MP Sorter` (Mode: `Whitelist` · Items: `Uranium`, `Platinum`, `Gold`, `Magnesium` · Drain All: `ON`)
* Hinges: `MAS MP Clamp Hinge Port/Stbd`
  - Lower Limit: `0.0° (Stowed Flush)` · Upper Limit: `30.0° (Clamped Inward)`
  - Torque: `10 MN·m` · Braking Torque: `20 MN·m` · Velocity: `1.5 rpm`
* Magnetic Plates: `MAS MP Clamp Plate Port/Stbd` (Auto-Lock: `ON` · Lock Strength: `100%`)
* Event Controller: `MAS EC MP`
  - Monitored Target: `MAS MP Conn`
  - Condition: Status = `Connected`
  - Slot 1 (Touchdown): Hinges reverse to 30.0° inward -> Clamp plates lock Mule rub rails -> `MAS MP Sorter` Drain All ON
  - Slot 2 (Disconnect): Hinges return to 0.0° flush -> `MAS MP Sorter` Drain All OFF

✓ Check: Inbound Mule touches down; cradle arms swing 30° inward and lock rub rails without chassis deformation; rare ores drain at $> 150\text{ t/min}$ into Mule cargo bins.

---

#### Step 10.38 · Roof Flush Solar Array for Autonomous Watchkeeping (L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Flush Solar Array & Standby Power  ·  Level: L6                 │
│ [VIEWPORT] Grid Zone: [X ±4, Z −24 to −20]  ·  Plan view Y = L6 (▲ Bow +Z)            │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.38 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Solar Panel (Large Grid) | 4 | Slot 0 (`#2B303A`) | 20.4 t / 32 | `MAS Solar 1..4` (1×4 B) flush with roof at X ±4 from Z −24 to −20 |
| Heavy Armor Slope | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Recessed border coaming providing 1-block thruster buffer |

```
Plan view, Y = L6 (Mule Blast Deck Flanked by Flush Solar Array)
        X: −4     X: −3           0          +3     X: +4
    Z: −19 ┌────┐ ■════════════╤═══════════■ ┌────┐
           │SOL1│ ║  Hazard    │  Hazard   ║ │SOL3│  ◄── Flush Solar Array (≈ 640 kW Peak†)
    Z: −22 │    │ [CLAMP P] [MAS MP CONN] [CLAMP S]  │    │  ◄── Mule Clamping Rails
           │SOL2│ ║  Chevrons  │  Chevrons ║ │SOL4│
    Z: −25 └────┘ ■════════════╧═══════════■ └────┘
```

##### Placement Instructions
1. Lay 2× Large Grid Solar Panels (`MAS Solar 1, 2`) along the Port roof wing at X = −4 spanning Z = −24 to −20, recessed flush with the Level L6 deck surface.
2. Lay 2× Large Grid Solar Panels (`MAS Solar 3, 4`) along the Starboard roof wing at X = +4 spanning Z = −24 to −20, recessed flush with the Level L6 deck surface.
3. Maintain a continuous 1-block clear buffer (Heavy Armor Slopes) between the solar array and the 7×7 B Mule landing footprint, preventing thermal erosion from shuttle hydrogen thruster exhaust during approach and liftoff.
4. Route power conduits down into the Level L2 battery vault bus.

##### Terminal Configuration & Sliders
* Solar Panels: `MAS Solar 1..4`
  - Peak Power Output: `≈ 160 kW each† (640 kW array peak†)`
  - Operating Role: Continuous zero-fuel watchkeeping power
  - Power Balancing: Maintains life support, Event Controllers, antenna beacons, and float-charges the 18.0 MWh battery bank during prolonged daytime quarrying without consuming nuclear uranium or hydrogen fuel.

✓ Check: Solar array generates daylight float-charge; clear buffer prevents Mule thruster burn; batteries maintain 100% state of charge during engine-off watchkeeping.

---

#### Step 10.39 · Level L6 Flight Deck Maintenance Catwalk Network (L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Flight Deck Walkways & Safety Handrails  ·  Level: L6           │
│ [VIEWPORT] Grid Zone: [X −4 to +4, Z −26 to −10]  ·  Plan view Y = L6 (▲ Bow +Z)      │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.39 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Catwalk with Railing Block | 18 | Slot 9 (`#F1C40F`) | 5.4 t / 18 | Perimeter railing walkways surrounding Mule deck and Kestrel paths |
| Grated Catwalk Block | 10 | Slot 9 (`#F1C40F`) | 3.0 t / 10 | Walkway intersections, flight ladderwell landing, and solar access paths |
| Interior Light | 4 | Slot 9 (`#F1C40F`) | 0.2 t / 16 | `MAS Light Catwalk 1..4` mounted along railing stanchions |

```
Plan view, Y = L6 (Flight Deck Walkway Network & Perimeter Railings)
        X: −4          X: −3           0           X: +3          X: +4
    Z: −10 ┌───────────────┬───────────────────────┬───────────────┬────────┐
           │               │ [FLIGHT LADDER EXIT]  │ [CATWALK P]   │        │  ◄── From Workshop L3
    Z: −12 ├───────────────┼───────────────────────┼───────────────┤        │
           │ [KESTREL P]   │ [FORWARD ACCESS DECK] │ [KESTREL S]   │        │  ◄── Service Bridges
    Z: −19 ├───────────────┼───────────────────────┼───────────────┼────────┤
           │ [SOLAR WALK P]│ ═════════════════════ │ [SOLAR WALK S]│        │  ◄── Flank Inspection
    Z: −22 │ [MAS SOLAR 1] │  7×7 MULE BLAST DECK  │ [MAS SOLAR 3] │        │      Gangways
    Z: −25 ├───────────────┼───────────────────────┼───────────────┼────────┤
    Z: −26 └───────────────┴───[AFT CIWS STERN]────┴───────────────┴────────┘  ◄── Stern Walkway
```

##### Placement Instructions
1. Install Grated Catwalk Blocks at the exit threshold of the vertical flight ladderwell at [X +2, Z −10, Y = L6].
2. Lay forward Catwalk with Railing Blocks extending from the ladderwell exit to the Port and Starboard Kestrel landing pads, establishing safe crew inspection paths to docked scout drones.
3. Install perimeter Catwalk with Railing Blocks surrounding the exterior border of the 7×7 B Mule blast deck along X = ±4 and Z = −26. Ensure high-visibility yellow railings face outward to prevent crew from falling off the vehicle roof (15.0 m above terrain).
4. Run narrow solar inspection catwalks along X = −4 and X = +4, giving engineers direct walk-up access to inspect and repair `MAS Solar 1..4` without treading on the active Mule pad.
5. Mount 4× Interior Lights (`MAS Light Catwalk 1..4`) along the railing stanchions.

##### Terminal Configuration & Sliders
* Lights: `MAS Light Catwalk 1..4` (Color: High-Viz Amber `#F1C40F` · Radius: `4.0 m` · Falloff: `1.5`)
* Group: Assign all catwalk lights to `MAS Lights`.

✓ Check: Crew on foot traverses entire upper flight deck from ladderwell to scout pads, solar panels, and stern CIWS with zero fall hazard.

---

#### Step 10.40 · Twin Kestrel Prospector Scout Drone Landing Pads (L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Kestrel Scout Drone Receiving Pads  ·  Level: L5                │
│ [VIEWPORT] Grid Zone: [X ±3, Z −13 to −11]  ·  Plan view Y = L5 (▲ Bow +Z)           │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.40 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Connector (Small Grid via collar) | 2 | Slot 8 (`#FFFFFF`) | 0.8 t / 22 | `MAS Kestrel Dock 1` at [X −3, Z −12, L5]; `MAS Kestrel Dock 2` at [X +3, Z −12, L5] |
| Heavy Armor Slope | 12 | Slot 8 (`#FFFFFF`) | 10.2 t / 12 | Beveled alignment troughs funneling scout drone landing skids |
| Interior Light | 4 | Slot 8 (`#FFFFFF`) | 0.2 t / 16 | `MAS Light Pad Kestrel 1..4` inset into pad margins |

```
Plan view, Y = L5 (Twin Recessed Kestrel Scout Landing Pads)
        X: −3 (Port Scout Dock)                         X: +3 (Starboard Scout Dock)
    Z: −11 ┌───────────────────────────┐           ┌───────────────────────────┐
           │ [SLOPE]   [SLOPE] [SLOPE] │           │ [SLOPE]   [SLOPE] [SLOPE] │
    Z: −12 │ [LIGHT] [KESTREL DOCK 1]  │ ◄─ AISLE ─► [LIGHT] [KESTREL DOCK 2]  │
           │ (Recessed Flush Connector)│           │ (Recessed Flush Connector)│
    Z: −13 │ [SLOPE]   [SLOPE] [SLOPE] │           │ [SLOPE]   [SLOPE] [SLOPE] │
           └───────────────────────────┘           └───────────────────────────┘
```

##### Placement Instructions
1. Frame two recessed 3×3 block landing bays forward of the Mule gantry at Level L5: Port bay centered at [X −3, Z −12] and Starboard bay centered at [X +3, Z −12].
2. Install small-grid docking collars `MAS Kestrel Dock 1` and `MAS Kestrel Dock 2` flush with the deck floor facing zenith (+Y), plumbed directly to the utility conveyor spine.
3. Install Heavy Armor Slopes painted Palette Slot 8 (Scout Flight White) around pad perimeters, creating 45° beveled guide ramps that guide descending Kestrel drones into positive connector alignment.
4. Inset 2× perimeter indicator lights per pad (`MAS Light Pad Kestrel 1..4`).

##### Terminal Configuration & Sliders
* Connectors: `MAS Kestrel Dock 1`, `MAS Kestrel Dock 2`
  - Auto-Connect: `OFF` · Collect All: `OFF` · Strength: `0.020`
* Lights: `MAS Light Pad Kestrel 1..4` (Color: Scout Aviation Cyan `#00E5FF` · Radius: `3.0 m`)
* Data Bus: Docking triggers immediate IGC data download of geological survey scans (`TMS.SCOUT.VEIN`) to Station 3 Tactical Video Wall.

✓ Check: Kestrel scout drone lands smoothly in recessed bay; beveled slopes funnel skids into dock; connector locks and downloads geological scan telemetry.

---

#### Step 10.41 · Flight Deck Recessed Drone Maintenance Welders (L4-L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Automated Flight Deck Drone Repair Welders  ·  Level: L4-L5     │
│ [VIEWPORT] Grid Zone: [X ±3, Z −12]  ·  Profile view facing Port (▲ Zenith +Y)        │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.41 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Ship Welder (Large Grid) | 2 | Slot 4 (`#16A085`) | 7.8 t / 300 | `MAS Welder Pad Kestrel 1` at [X −3, Z −12, L4]; `MAS Welder Pad Kestrel 2` at [X +3, Z −12, L4] |
| Large Conveyor Pipe | 2 | Slot 4 (`#16A085`) | 0.6 t / 2 | Vertical risers connecting welders to workshop conveyor spine |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Drone Repair` at [X0, Z −13, L4] managing automated pulse cycle |

```
Profile elevation cross-section, X = −3 (Port Kestrel Pad Welder Riser)
  L5   [RECESSED DECK] ── [MAS KESTREL DOCK 1] ── [RECESSED DECK]
             │                    │                      │
  L4   ══════╪════════════════════╪══════════════════════╪══════
             │           [MAS WELDER PAD KESTREL 1]      │
             │           (Faces Upward toward Drone Hull)│
             │                    │                      │
  L3   ──────┴──────────────[CONVEYOR RISER]─────────────┴──────
       WORKSHOP CEILING (Plumbed to MAS Hold Components & Assembler)
```

##### Placement Instructions
1. Position Large Grid Ship Welder `MAS Welder Pad Kestrel 1` directly beneath the Port Kestrel landing dock at [X −3, Z −12, Y = L4] with welding head facing upward (+Y).
2. Position Large Grid Ship Welder `MAS Welder Pad Kestrel 2` directly beneath the Starboard Kestrel landing dock at [X +3, Z −12, Y = L4] with welding head facing upward (+Y).
3. Connect bottom conveyor ports of both welders directly into the Level L3 workshop conveyor trunk, drawing steel plates, construction components, small tubes, and computer chips from `MAS Hold Components` and `MAS Utility Assembler`.
4. Mount Event Controller `MAS EC Drone Repair` in the sub-deck wireway at [X0, Z −13, Y = L4].

##### Terminal Configuration & Sliders
* Welders: `MAS Welder Pad Kestrel 1`, `MAS Welder Pad Kestrel 2`
  - Help Others: `ON`
  - Power: `OFF` (Pulsed on-demand by automation)
* Event Controller: `MAS EC Drone Repair`
  - Monitored Target: `MAS Kestrel Dock 1, 2`
  - Condition: Status = `Connected`
  - Slot 1 (Touchdown): Pulse pad welders for 5.0 seconds -> Auto-repairs thruster burnout, hull pits, and antenna damage sustained during prospecting sorties -> Power OFF
  - Slot 2: Welders OFF on disconnect

✓ Check: Damaged Kestrel scout docks; welders pulse for 5.0 s; hull repairs to 100% using parts from component hold; welders return to standby.

---

#### Step 10.42 · Worksite 360° Floodlight Matrix & Dorsal Comms Mast (L5-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Worksite Perimeter Illumination & Comms Mast  ·  Level: L5-L6   │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z +7 to −26]  ·  Plan view Y = L5-L6 (▲ Bow +Z)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.42 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Spotlight (Large Grid) | 4 | Slot 7 (`#1B263B`) | 2.0 t / 32 | `MAS Floodlight Tower 1..4` in cardinal ring on bridge crown & gantry (Bow, Stern, Port, Stbd) |
| Interior Light | 4 | Slot 0 (`#2B303A`) | 0.2 t / 16 | `MAS Lights Underbelly` at [X ±2, Z0 and Z −18, L1] for chassis inspection |

```
Plan view, Y = L5-L6 (Worksite 360° Cardinal Floodlight Ring)
                                [BOW FLOODLIGHT (+Z)]
                               (MAS Floodlight Tower 1)
                                         ▲
                                         │
        [PORT FLOODLIGHT (−X)] ◄── [MASTODON C2] ──► [STBD FLOODLIGHT (+X)]
       (MAS Floodlight Tower 3)    (Crown L5-L6)     (MAS Floodlight Tower 4)
                                         │
                                         ▼
                               [STERN FLOODLIGHT (−Z)]
                               (MAS Floodlight Tower 2)
```

##### Placement Instructions
1. Mount 4× Large Grid Spotlights (`MAS Floodlight Tower 1..4`) in an elevated 360° cardinal ring on the Tractor dorsal bridge crown and comms mast at Level L5-L6:
   - `MAS Floodlight Tower 1`: Facing Bow (+Z) illuminating front excavation zone.
   - `MAS Floodlight Tower 2`: Facing Stern (−Z) illuminating rear Caterpillar dump and Mule pad.
   - `MAS Floodlight Tower 3`: Facing Port (−X) illuminating left flank and boarding ladder.
   - `MAS Floodlight Tower 4`: Facing Starboard (+X) illuminating right quarry face.
2. Install 4× Interior Lights (`MAS Lights Underbelly`) along the underbelly chassis keel at [X ±2, Z 0, Y = L1] and [X ±2, Z −18, Y = L1] facing downward.
3. Bind all worksite and chassis lights into terminal group `MAS Lights`.

##### Terminal Configuration & Sliders
* Spotlights: `MAS Floodlight Tower 1..4`
  - Color: Pure White (`#FFFFFF`) · Intensity: `3.5` · Radius: `100 m` · Falloff: `1.2`
* Keel Lights: `MAS Lights Underbelly`
  - Color: Industrial Amber (`#F1C40F`) · Radius: `6.0 m` · Falloff: `1.5`
* Control Binding: Station 1 Helm Hotbar 3 Slot 8 toggles group `MAS Lights`.

✓ Check: Group `MAS Lights` illuminates 100 m perimeter around vehicle; excavation face, flight decks, and underbelly inspection alleys are fully visible in pitch black.

---

#### Step 10.43 · Stern Push-Bumper, Telescoping Soft-Capture Port & Stern Outriggers (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Stern Rail Dump Interface & Bedrock Outriggers  ·  Level: L1-L2 │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z −27 to −25]  ·  Plan view Y = L2 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.43 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Piston (Large Grid) | 1 | Slot 2 (`#8B0000`) | 12.5 t / 25 | `MAS Piston Stern Dock` at [X0, Z −25, L2] horizontal extending aft (−Z) |
| Large Connector | 1 | Slot 2 (`#8B0000`) | 4.1 t / 25 | `MAS Stern Dump Conn` mounted on piston head at [X0, Z −26, L2] |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS Stern Sorter` at [X0, Z −24, L2] pointing aft (−Z) into piston base |
| Heavy Armor Slope | 8 | Slot 2 (`#8B0000`) | 6.8 t / 8 | Angled collision guide wedges flanking the dump connector at X ±3 |
| Piston (Large Grid) | 2 | Slot 0 (`#2B303A`) | 25.0 t / 50 | `MAS Outrigger Utility Port/Stbd` vertically at [X ±3, Z −25, L2] extending down |
| Magnetic Plate (Large) | 2 | Slot 0 (`#2B303A`) | 5.8 t / 50 | `MAS Seismic Pad Rear Port/Stbd` on piston heads facing ground (L1) |
| Conveyor Sorter | 1 | Slot 0 (`#2B303A`) | 4.9 t / 25 | `MAS Sorter Tailings Stern` at [X0, Z −26, L1] pointing aft (−Z) |
| Conveyor Ejector | 4 | Slot 0 (`#2B303A`) | 2.4 t / 40 | `MAS Ejector Tailings Stern 1..4` angled 45° downward/aft behind rear tires |

```
Rear elevation cross-section, Y = L2 (Stern Caterpillar Dump & Outrigger Interface)
    Port [-X]                                                          Starboard [+X]
    X: −3            X: −1            X0              X: +1            X: +3
    [GUIDE WEDGE] ── [PUSH BLOCK] ─ [STERN CONN] ─ [PUSH BLOCK] ── [GUIDE WEDGE]
    │ Heavy Slope    │ Solid Block  │ (Telescoping  │ Solid Block  │ Heavy Slope
    │ Crimson        │ Crimson      │ Piston Head)  │ Crimson      │ Crimson
    └──────┬─────────┴──────────────┴───────┬───────┴──────────────┴───────┬─────┘
           │                                │                              │
    [OUTRIGGER PORT]               [TAILINGS EJECTORS]             [OUTRIGGER STBD]
    (Piston down to L1)            (Compacted Roadbed)             (Piston down to L1)
```

```
Telescoping Stern Dock Profile, Z = −24 to −27.5, Y = L2
       [CARRIAGE 2 STERN]                             [CATERPILLAR BOW]
       ┌───────────────┐                               ┌───────────────┐
   L2  │ CONVEYOR DUCT ├──[PISTON 2.5 m]──[CONNECTOR]══╡ CONNECTOR     │
       └───────────────┘  (MAS Piston     (MAS Stern   └───────────────┘
                           Stern Dock)     Dump Conn)
```

##### Placement Instructions
1. Mount 1× Large Grid Piston `MAS Piston Stern Dock` horizontally at [X0, Z −25, Y = L2] with piston base plumbed to `MAS Stern Sorter` and piston head extending aft (−Z). Maximum stroke: 2.5 m (reaching Z = −27.5).
2. Install Large Connector `MAS Stern Dump Conn` directly onto the piston head. Configure centerline height at 2.5 m above ground plane matching the Caterpillar locomotive bow connector.
3. Install Heavy Armor Slopes painted Palette Slot 2 (Heavy Rail Crimson) at X = −3 and X = +3, forming rigid guide wedges that funnel the incoming land train into alignment.
4. Mount 2× Large Grid Pistons vertically at [X −3, Z −25, Y = L2] (`MAS Outrigger Utility Port`) and [X +3, Z −25, Y = L2] (`MAS Outrigger Utility Stbd`) extending downward through Level L1.
5. Mount Large Magnetic Plates (`MAS Seismic Pad Rear Port/Stbd`) on the piston heads facing ground.
6. Install Large Conveyor Sorter `MAS Sorter Tailings Stern` and 4× Conveyor Ejectors (`MAS Ejector Tailings Stern 1..4`) along the rear bumper at Level L1, angled 45° downward/aft.

##### Terminal Configuration & Sliders
* Piston: `MAS Piston Stern Dock` (Max Distance: `2.5 m` · Velocity: `0.2 m/s` · Share Inertia Tensor: `ON`)
* Connector: `MAS Stern Dump Conn` (Auto-Connect: `OFF` · Strength: `0.015`)
* Sorter: `MAS Stern Sorter` (Mode: `Whitelist` · Items: `Iron`, `Nickel`, `Silicon`, `Stone` · Drain All: `ON`)
* Outriggers: `MAS Outrigger Utility Port/Stbd` (Max Distance: `1.5 m` · Velocity: `0.5 m/s` · Share Inertia Tensor: `ON`)
* Tailings Sorter: `MAS Sorter Tailings Stern` (Mode: `Whitelist` · Items: `Stone`, `Gravel` · Drain All: `ON`)

✓ Check: Stern piston extends 2.5 m and soft-couples with Caterpillar locomotive; 2,500 t bulk ore dumps in $< 90\text{ seconds}$; stern outriggers achieve 100% bedrock lock; tailings eject aggregate behind rear wheels.

---

#### Step 10.44 · Stern Gantry CIWS Gatling Turret (L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Rear Defense CIWS & Gantry Fire Arc  ·  Level: L5               │
│ [VIEWPORT] Grid Zone: [X0, Z −26]  ·  Rear elevation facing Bow (▲ Zenith +Y)        │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.44 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Gatling Turret (Large Grid) | 1 | Slot 7 (`#1B263B`) | 4.2 t / 275 | `MAS Turret Stern` at [X0, Z −26, L5] elevated 7.5 m above dump bumper |
| Conveyor Junction | 1 | Slot 4 (`#16A085`) | 0.9 t / 1 | Plumbed into utility conveyor spine at [X0, Z −26, L4] |

```
Rear elevation looking Bow (+Z), Z = −26 (Stern Defense Turret & Firing Cleats)
  L6   ═══════════════════════════════════════════════════════════════
       │                                                             │
  L5   │                    [MAS TURRET STERN]                       │  ◄── 270° Hemispherical
       │                    (Gatling CIWS L5)                        │      Covering Fire Arc
  L4   ├────────────────────[CONVEYOR JUNCTION]──────────────────────┤
       │                                                             │
  L3   │                    STERN AIRLOCK DESCENT HATCH              │
       │                                                             │
  L2   ══════════════[TELESCOPING DUMP BUMPER]════════════════════════
```

##### Placement Instructions
1. Install Large Conveyor Junction at [X0, Z −26, Y = L4] tied directly into the utility aft conveyor trunk.
2. Mount Large Grid Gatling Turret `MAS Turret Stern` on top of the junction at [X0, Z −26, Y = L5], elevated 7.5 m above the Level L2 Caterpillar dump bumper.
3. Verify unobstructed 270° hemispherical firing arc over the aft hemisphere (−Z, ±X, and zenith +Y), covering incoming Mule shuttles on approach and trailing land trains during dump cycles.
4. Tie turret ammunition feed into the workshop conveyor trunk; verify automated pull of NATO 25×184mm ammo boxes from `MAS Hold Components`.

##### Terminal Configuration & Sliders
* Gatling Turret: `MAS Turret Stern`
  - Range: `800 m` · Target Meteors: `ON` · Target Missiles: `ON` · Target Small Ships: `ON` · Target Large Ships: `ON`
  - Idle Movement: `OFF` · Use Conveyor System: `ON`
* Group: Assign to terminal group `MAS Turrets`.

---

#### Step 10.44B · Elevated Stern Multitool Repair Turret & Outbound Resupply Staging Hopper (L5-L7)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Consist Tail Repair & Mule Peer Resupply  ·  Levels: L4, L5, L7  │
│ [VIEWPORT] Grid Zone: Carriage 2 Stern Arch [X0, Z −23, L7]; Staging [X0, Z −20, L5] │
└──────────────────────────────────────────────────────────────────────────────────────┘

##### Step 10.44B Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Laser Multitool Turret | 1 | Slot 3 (`#E07A5F`) | 4.5 t / 225 | `MAS MT Turret Stern` on elevated gantry arch at [X0, Z −23, L7] · Mod 3175774286 |
| Small Cargo Container | 1 | Slot 5 (`#27AE60`) | 1.0 t / 10 | `MAS Hold Resupply Export` at [X0, Z −20, L5] directly beneath `MAS MP Conn` |
| Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 1.0 t / 25 | `MAS Sorter Resupply Export` at [X0, Z −20, L4] feeding upward into export hopper |
| Heavy Armor Blocks / Slopes | 9 | Slot 0 (`#2C3E50`) | 1.0 t / 2 | Gantry arch framing rising from L5/L6 deck line to L7 apex at Z −23 |

```
Rear elevation looking Bow (+Z), Z = −23 (Elevated Stern MT Repair Turret & Arch)
  L7   ═════════════════════[MAS MT TURRET STERN]═════════════════════  ◄── 360° Hemispherical
       │                     (Laser Welder L7)                       │      Overhead Repair Arc
  L6   ├─────────[GANTRY ARCH FRAME]───┬───[GANTRY ARCH FRAME]───────┤
       │                               │                             │
  L5   │   [MULE PAD CATWALK]    [MAS HOLD RESUPPLY EXPORT]  [CATWALK]│  ◄── Resupply Staging Hopper
       │                               │                             │      (Direct Drain to Mule)
  L4   ├───────────────────[MAS SORTER RESUPPLY EXPORT]───────────────┤
       │                               │                             │
  L3   │                 WORKSHOP CONVEYOR CENTRAL TRUNK             │
```

##### Placement Instructions
1. Erect a 9-block Heavy Armor gantry arch rising from the Level L5-L6 flight deck margins up to a centerline apex at [X0, Z −23, Y = L7], spanning over the Mule pad approach.
2. Plumb a vertical conveyor riser from the workshop conveyor trunk up the arch leg to [X0, Z −23, Y = L7].
3. Mount Large Grid Laser Multitool Turret `MAS MT Turret Stern` on top of the arch at [X0, Z −23, Y = L7]. Verify unobstructed 360° hemispherical vantage looking down directly onto the MULE-01 cradle pad (L6), the stern Caterpillar dump bumper (L2), and the aft trailing ground approach.
4. Install Conveyor Sorter `MAS Sorter Resupply Export` at [X0, Z −20, Y = L4] with flow arrow pointing upward into Level L5.
5. Install Small Cargo Container `MAS Hold Resupply Export` at [X0, Z −20, Y = L5] directly between the export sorter and the top-deck `MAS MP Conn` junction.

##### Terminal Configuration & Sliders
* Group: `MAS Multitool Turrets` (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`)
* Group: `MAS Resupply` (`MAS Hold Resupply Export`, `MAS Sorter Resupply Export`, `MAS MP Conn`)
* Multitool Turret: `MAS MT Turret Stern`
  * Default Mode: `Welder` (Servicing docked Mule and trailing vehicles)
  * Secondary Mode: `Grinder` (Aft salvage and obstacle clearance)
  * Range: `50 m` · Target Friendly: `ON` · Target Neutrals: `ON` · Auto-Repair: `ON`
* Sorter: `MAS Sorter Resupply Export`
  * Default State: `OFF` · Drain All: `OFF`
  * Whitelist filter automatically populated by `[MAS-U] PB Utility` upon resupply kit staging.
* Event Controller: `MAS EC Resupply`
  * Monitored Target: `MAS Hold Resupply Export`
  * Condition: Inventory Fill > 0%
  * Action Slot 1: Broadcast IGC `TMS.RESUPPLY.DISPATCH` · Light Staging Indicator
  * Action Slot 2: Standby

##### Standardized Mule Peer Resupply Sortie Protocol
* Standardized Pallet Kits:
  * `KIT_MOBILITY`: 200 Steel Plates, 100 Motors, 50 Large Tubes, 50 Small Tubes.
  * `KIT_POWER`: 100 Steel Plates, 80 Construction Comps, 40 Computers, 50 Large Tubes.
  * `KIT_MUNITIONS`: 60 NATO 25×184mm ammo boxes, 40 Interior Plates, 20 Construction Comps.
  * `KIT_HULL_REPAIR`: 500 Steel Plates, 100 Interior Plates, 50 Construction Comps.
* Resupply Execution Cycle:
  1. Sister rig (`MAS-02`) broadcasts emergency distress call via `TMS.RESUPPLY.REQUEST;MAS-02;KIT_MOBILITY;<gps>`.
  2. `[MAS] PB Excavator` / `[MAS-U] PB Utility` configures `MAS Sorter Resupply Export` whitelist with requested kit, turns Drain All = ON until export hopper contains the exact bill of materials, then powers sorter OFF.
  3. MULE-01 touches down on `MAS MP Conn`; export hopper drains directly into Mule cargo hold in $< 10\text{ seconds}$ without mixing into main component stores.
  4. MULE-01 departs immediately on autonomous delivery run to `MAS-02`, drops resupply pallet, and returns to consist.

---

##### Pre-Enclosure Inspection Gate 10.6A: Cradle Clamping Dynamics & Dual-Tier Sorter Routing
* [ ] Verify `MAS MP Sorter` whitelist passes only rare ores (U, Pt, Au, Mg) to the Mule shuttle.
* [ ] Verify `MAS Stern Sorter` whitelist passes bulk ores (Fe, Ni, Si, Stone) to the land train.
* [ ] Test cradle clamp hinges: confirm 30° inward swing firmly holds docking craft without grid damage.
* [ ] Confirm `MAS Solar 1..4` deliver daylight power and maintain battery float charge while nuclear reactors are toggled OFF.
* [ ] Verify Level L6 catwalk handrails and safety walkways allow complete perimeter access around Mule and Kestrel decks.
* [ ] Verify `MAS Turret Stern` receives NATO 25×184mm ammunition from utility cargo hold.
* [ ] Verify recessed drone welders (`MAS Welder Pad Kestrel 1, 2`) auto-repair docked Kestrel drones upon touchdown.
* [ ] Confirm worksite floodlight matrix (`MAS Floodlight Tower 1..4`) casts 360° illumination across mining zone and flight decks.
* [ ] Confirm `MAS Piston Stern Dock` extends 2.5 m and soft-couples with Caterpillar land train.
* [ ] Verify `MAS Outrigger Utility Port/Stbd` achieve 100% bedrock lock and isolate chassis during Mule touchdown.
* [ ] Verify `MAS Sorter Tailings Stern` discharges stone aggregate safely behind stern bumper.
* [ ] Verify `MAS MT Turret Stern` on L7 arch achieves 360° hemispherical coverage over Mule pad and stern approach.
* [ ] Verify `MAS Sorter Resupply Export` pre-stages requested resupply kit into `MAS Hold Resupply Export` and drains to Mule in $< 10\text{ s}$.

```
PHASE 10-6 VISUAL MILESTONE · COMPLETE EXPEDITIONARY MINING TRAIN FOB
                        [KESTREL PADS]         [MULE PAD & MT ARCH L7]
                              │                        │
                    [TRACTOR BRIDGE L4]       [UTILITY WORKSHOP L3]
                              │                        │
       [45 m BOOM L4]         │         [HITCH L2]     │         [STERN DUMP L2]
              \               │              ║         │                /
      ═════════╧══════════════╧══════════════╩═════════╧════════════════╩════════
           CARRIAGE 1: LOCOMOTIVE / EXCAVATOR   CARRIAGE 2: FOB / UTILITY MODULE
```

---

### Phase 10-7 · Master Control Logic, Dual-PB Architecture & RIG//OS-Lite Contract

Every automated system on the Mastodon MKI conforms to this dual-compute and dynamic C2 hierarchy contract:

#### 1. Dual-PB Compute Architecture (Tractor L3)
1. **`[MAS] PB Excavator` (Real-Time Machine Kinematics · Update1 / 60 Hz):**
   * Manages 3-axle tractive counter-steering and 2-axis motorized drawbar dampening.
   * Manages 4× corner bedrock outrigger deployment, auto-leveling, and voxel lock verification.
   * Controls 5-DOF articulated boom inverse kinematics, Custom Turret Controller mouse aim, and automated sweep macros (Bench, Shaft, Trench).
   * Strict Zero-GC loop execution; numbers formatted through `SbFormat`.
2. **`[MAS] PB RIG//OS-Lite` (Regional Fleet Orchestration & C2 Hub · Update10 / Update100):**
   * Manages dynamic C2 election: operates as Fleet Mothership / Root until Ground Base (`GND-01`) is commissioned.
   * Dispatches Caterpillar bulk land trains (`TMS.CAT.CALL`) and Mule air shuttles (`TMS.MULE.CALL`).
   * Receives Kestrel scout drone geological scans (`TMS.SCOUT.VEIN`) and displays vein GPS markers on the tactical video wall.
   * Orchestrates subordinate worker excavators (`MAS-02`, `MAS-03`) via `FLT.MAS.CMD`.
   * Automatically relinquishes C2 root authority upon detecting Ground Base master heartbeat (`RIG.C2.HEARTBEAT;ROLE=MASTER_BASE`).

```
State Machine: Dynamic Fleet Authority & C2 Handover

 [BOOT / INITIAL SCAN] ──► Listen for `RIG.C2.HEARTBEAT` (5.0 s timeout)
            │
            ├─► Ground Base Heartbeat Detected (`GND-01` Master)
            │   └─► Role = WORKER_SLAVE (Yields C2 to Base)
            │
            ├─► Another Mothership Detected (`MAS-01-M` Active)
            │   └─► Role = WORKER_SLAVE (Yields C2 to Lead Unit)
            │
            └─► No Superior C2 Heartbeat Detected
                └─► Role = MOTHERSHIP_ROOT (`MAS-01-M`)
                    │
                    ├─► Broadcast: `RIG.C2.HEARTBEAT;ORIGIN=MAS-01;ROLE=MOTHERSHIP`
                    ├─► Accept Fleet Asset Registrations (`TMS.REGISTER`)
                    ├─► Dispatch Bulk Land Trains and Air Shuttles
                    │
                    ▼
         [GROUND BASE COMMISSIONED]
         Receives: `RIG.C2.HEARTBEAT;ORIGIN=GND-01;ROLE=MASTER_BASE`
                    │
                    ├─► Transmit Active Fleet Registry & Ore Quotas to GND-01
                    ├─► Emit: `RIG.C2.HANDOVER;ACK=GND-01`
                    └─► Role = FIELD_LEADER (Expeditionary Sub-Node)
```

#### 2. Group Taxonomy
* `MAS Tractor Wheels`: 6× 5×5 suspension wheel units on Carriage 1.
* `MAS Utility Wheels`: 4× 5×5 suspension wheel units on Carriage 2.
* `MAS Outriggers`: 4× Hydraulic stabilizer pistons + magnetic plates / seismic feet on Tractor.
* `MAS Outriggers Utility`: 2× Hydraulic stabilizer pistons + seismic feet at Carriage 2 stern.
* `MAS Boom Joints`: 5× Rotors and Hinges articulating the long-reach arm.
* `MAS Lasers`: 2× Simple Laser Multitools on the tool head.
* `MAS Welders`: 2× Flush Large Grid Ship Welders (`MAS Welder Pad Kestrel 1, 2`) on Kestrel flight pads.
* `MAS Lights`: Worksite floodlights (`MAS Floodlight Tower 1..4`), boom spotlights (`MAS Light Boom 1, 2`), and underbelly lamps (`MAS Lights Underbelly`).
* `MAS Seismic`: 6× Seismic Surveying Landing Gear units (`MAS Seismic Outrigger 1..4`, `MAS Seismic Pad Rear Port/Stbd`).
* `MAS Batteries`: 6× Large Batteries across Tractor Vault and Utility Sub-Floor (18.0 MWh total).
* `MAS Doors`: All internal airlock, vault, and inter-carriage gangway sliding doors.
* `MAS Turrets`: 2× Large Gatling Turrets (`MAS Turret Bow`, `MAS Turret Stern`) + 2× Interior Turrets (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`).
* `MAS MP Cradle`: 2× Motorized Hinges + 2× Magnetic Plates on the Mule pad.
* `MAS Displays`: All cockpit telemetry screens and tactical video wall LCDs.
* `MAS Hitch Clamps`: 2× Large Magnetic Plates (`MAS Hitch Clamp Port`, `MAS Hitch Clamp Stbd`) on hitch drawbar.
* `MAS DPU`: Axles 4 and 5 suspension wheels on Carriage 2 running distributed traction logic.
* `MAS FOB`: Carriage 2 autonomous command blocks (`[MAS-U] PB Utility`, `MAS-U Remote FOB`, `MAS-U EC Breakaway`).
* `MAS Tailings Ejectors`: 4× Ejectors on stern bumper (`MAS Ejector Tailings Stern 1..4`).
* `MAS Multitool Turrets`: 4× Large Laser Multitool Turrets (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`).
* `MAS Resupply`: `MAS Hold Resupply Export`, `MAS Sorter Resupply Export`, `MAS MP Conn`.
* `MAS Armory`: `MAS Armory Bridge`, `MAS Armory Workshop`, `MAS Locker Survival Gear`.
* `MAS Airlocks`: All dual-door airlock sliding doors (`MAS Door Boarding Outer`, `MAS Door Boarding Inner`, `MAS Door Flight Deck Inner`, `MAS Door Roof Outer`, `MAS Door Stern Inner`, `MAS Door Gangway Fwd`, `MAS Door Gangway Aft`).
* `MAS Decon`: 4× UV decontamination washdown lights (`MAS Light Decon Port`, `MAS Light Decon Roof`, `MAS Light Decon Stern`, `MAS Light Decon Gangway`).
* `MAS Vents Cabin`: Multi-zone cabin air circulation vents (`MAS Vent Bridge`, `MAS Vent Crawlway`, `MAS Vent Gangway`, `MAS Vent Workshop`).
* `MAS Vents Airlock`: Active gas-reclamation decompression vents (`MAS Vent Airlock Port`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern`).
* `MAS Munitions`: Sub-floor munitions bunker (`MAS Hold Munitions`), forward ammo sorter (`MAS Sorter Ammo Fwd`), ammo Event Controller (`MAS EC Ammo`), and early warning siren (`MAS Alarm Meteor`).


#### 3. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Programmable Block | `[MAS] PB Excavator` | Kinematics & machine control · Update1 · Zero-GC |
| Programmable Block | `[MAS] PB RIG//OS-Lite` | Regional Fleet C2 Mothership engine · Update10 · IGC router |
| Programmable Block | `[MAS-U] PB Utility` | Autonomous Carriage 2 FOB supervisory core · Update10 |
| Remote Control | `MAS-U Remote FOB` | Carriage 2 autonomous waypoint, defense, and antenna link |
| Control Station | `MAS Helm` | Station 1 (Port) · Pilot traction, steering, outrigger, power control |
| Control Station | `MAS Station Boom` | Station 2 (Center) · Excavation Master · CTC mouse aim & macros |
| Control Station | `MAS Station Fleet` | Station 3 (Stbd) · Tactical Fleet Director · Drone dispatch & C2 |
| Laser Antenna | `MAS Comm Laser` | 40 km point-to-point line-of-sight data link (L6 mast) |
| Radio Antenna | `MAS Comm Radio` | 50 km omnidirectional fleet broadcast & heartbeat beacon |
| Radio Antenna | `MAS Beacon Distress` | 50 km blackout SOS distress beacon in sub-bridge keel |
| Battery (Small) | `MAS Battery BlackBox` | Isolated cold-standby power cell for distress beacon |
| Advanced Rotor | `MAS Hitch Yaw` | Articulated hitch yaw joint at [X0, Z −8, L2] · ±45° yaw |
| Motor Hinge | `MAS Hitch Pitch` | Articulated hitch pitch joint at [X0, Z −8, L2] · ±25° pitch |
| Advanced Rotor | `MAS Hitch Roll` | Articulated hitch roll joint at [X0, Z −8, L2] · ±20° roll along Z-axis |
| Magnetic Plate (Large) | `MAS Hitch Clamp Port/Stbd` | Twin anti-oscillation drawbar lock clamps (engaged > 30 km/h) |
| Merge Block | `MAS Hitch Sever Tongue` | Emergency pyrotechnic consist decoupling at [X0, Z −8, L2] |
| Decoy | `MAS Decoy Flank Port/Stbd`, `MAS Decoy Stern` | Lower armor skirt ballistic decoys diverting turret fire |
| Gyroscope (Large) | `MAS Gyro Recovery 1, 2` | Chassis roll override self-righting gyros at [X0, Z0, L1] |
| Projector (Large) | `MAS Projector Master` | Complete consist blueprint self-repair projection core at [X0, Z −14, L4] |
| Warhead (Large) | `MAS Warhead Scuttle Bridge`, `MAS Warhead Scuttle Power` | Anti-capture technology denial scuttle charges (L3) |
| Timer Block | `MAS Timer Scuttle` | 60-second native scuttle countdown loop with manual abort |
| Oxygen Tank (Large) | `MAS Tank O2 1, 2` | 200,000 L high-pressure closed-loop breathing reserve |
| Small Cargo Container | `MAS Hold Ice LifeSupport` | 15,000 L quarantined emergency life support ice buffer |
| Cryo Chamber | `MAS Cryo Pod 1, 2` | Medical bay crew hibernation pods (zero food/suit drain) |
| Air Vent (Large) | `MAS Vent Bridge`, `MAS Vent Workshop`, `MAS Vent Crawlway` | Multi-zone pressurized cabin air vents (Zones 1, 2, 4) |
| Air Vent (Large) | `MAS Vent Airlock Port`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern` | Active gas-reclamation decompression airlock vents |
| Button Panel | `MAS BP Ground Boarding`, `MAS BP Airlock Vestibule`, `MAS BP Power Vault` | Physical in-situ crew control panels (Tractor L1-L3) |
| Button Panel | `MAS BP Scuttle Bridge`, `MAS BP Field Workshop`, `MAS BP Flight Deck`, `MAS BP Stern Dump` | Physical in-situ crew control panels (Bridge L4, FOB L2-L6) |
| Gatling Turret | `MAS Turret Bow` | Carriage 1 dorsal crown CIWS · 360° hemispherical anti-air/ground |
| Gatling Turret | `MAS Turret Stern` | Carriage 2 stern gantry CIWS · 270° rear hemispherical coverage |
| Interior Turret | `MAS Turret Flank Port` | Port lower skirt L2 · Anti-personnel / anti-spider perimeter guard |
| Interior Turret | `MAS Turret Flank Stbd` | Starboard lower skirt L2 · Anti-personnel / anti-spider perimeter guard |
| AI Move Block | `MAS AI Move` | Autonomous pathfinding & terrain navigation (uncrewed role) |
| AI Task Block | `MAS AI Task` | Autonomous waypoint execution & ore vein transit |
| AI Defense Block | `MAS AI Defense` | Threat avoidance · Auto-locks outriggers and stows boom |
| Piston (Large) | `MAS Outrigger Piston 1..4` | Corner bedrock stabilizer cylinders · `Share Inertia Tensor = ON` |
| Magnetic Plate | `MAS Outrigger Pad 1..4` | Corner bedrock voxel locking pads · `Auto-Lock = ON` |
| Piston (Large) | `MAS Outrigger Utility Port/Stbd`| Stern vertical hydraulic outriggers at [X ±3, Z −25, L2] |
| Magnetic Plate | `MAS Seismic Pad Rear Port/Stbd`| Stern bedrock voxel locking pads · `Auto-Lock = ON` |
| Seismic Landing Gear | `MAS Seismic Outrigger 1..4` | Deep voxel acoustic prospecting feet (TMS Mod 2819263781) |
| Magnetic Plate (Large) | `MAS Utility Brake Keel 1, 2` | Keel emergency terrain skids at [X ±2, Z −18, L1] |
| Magnetic Plate (Large) | `MAS Tool Grapple` | Centerline boom recovery grapple · Vehicle righting & cargo hoisting |
| Magnetic Plate (Large) | `MAS Clamp Boom Stow` | Chassis dorsal spine travel stow clamp at [X0, Z −7, L4] |
| Ship Welder (Large) | `MAS Welder Pad Kestrel 1, 2` | Recessed flight deck drone maintenance welders |
| Spotlight (Large) | `MAS Floodlight Tower 1..4` | 360° dorsal worksite illumination floodlights (L5-L6) |
| Interior Light (Large) | `MAS Lights Underbelly` | Underbelly chassis maintenance and inspection lighting (L1) |
| Small Cargo Container | `MAS Locker Port`, `MAS Locker Stbd` | Ground-level EVA survival supply lockers (skirt L2) |
| Air Vent (Large) | `MAS Vent Atmo Intake Port`, `MAS Vent Atmo Intake Stbd` | Ambient planetary atmospheric O2 intake vents (`Depressurize = ON`) |
| Motor Hinge | `MAS Crew Ladder Hinge` | Port flank boarding ladder articulator · 0° stowed to 90° down |
| Sensor | `MAS Sensor Ladder` | 2.5 m crew proximity sensor on Port flank (auto-deploy) |
| Advanced Rotor | `MAS Turret Azimuth` | Dorsal boom turntable · 360° pan · `Torque = 50 MN·m` |
| Motor Hinge | `MAS Shoulder Pitch` | Boom elevation joint · −20° to +75° · `Torque = 45 MN·m` |
| Motor Hinge | `MAS Elbow Pitch` | Boom mid-joint · −100° to +45° · `Torque = 45 MN·m` |
| Advanced Rotor | `MAS Wrist Roll` | Tool head roll alignment · ±180° · `Torque = 20 MN·m` |
| Motor Hinge | `MAS Wrist Pitch` | 90° tool elbow pitch leveling & vertical down lock · −60° to +60° · `Torque = 20 MN·m` |
| Laser Multitool | `MAS Laser 1, 2` | Tri-mode Simple Laser Multitools (ToolCore) · Dynamic switching: Mining (Drill) / Salvage (Grinder, 15 to 30 m standoff) / Fabrication (Welder, 35 m reach) |
| Sensor | `MAS Sensor Boom` | 4.0 m forward proximity collision safety buffer |
| Connector (Large) | `MAS MP Conn` | Roof flush connector for MULE-01 shuttle docking |
| Piston (Large) | `MAS Piston Stern Dock` | Stern horizontal telescoping docking piston (2.5 m stroke) |
| Connector (Large) | `MAS Stern Dump Conn` | Stern telescoping connector for CATERPILLAR-01 bulk train docking |
| Conveyor Sorter | `MAS MP Sorter` | High-priority rare ore drain sorter (U, Pt, Au, Mg) |
| Conveyor Sorter | `MAS Stern Sorter` | High-volume bulk ore drain sorter (Fe, Ni, Si, Stone) |
| Conveyor Sorter | `MAS Sorter Ammo Fwd` | Dedicated forward munitions sorter to bow CIWS |
| Conveyor Sorter | `MAS Sorter Parts Aft` | Dedicated aft scrap metal sorter into field refinery |
| Conveyor Sorter | `MAS Sorter Tailings Stern` | Relocated stern roadbed aggregate sorter at Z −26 |
| Conveyor Ejector | `MAS Ejector Tailings Stern 1..4`| Angled stern bumper nozzles laying aggregate roadbed |
| Basic Refinery | `MAS Field Refinery` | Closed-loop field metallurgy · 330 kW · Ingests raw ore, outputs ingots |
| Hydrogen Engine | `MAS Engine H2` | Standby cold-start emergency generator · 5.0 MW failover |
| Solar Panel | `MAS Solar 1..4` | Flush roof solar array · 640 kW peak† · Zero-fuel watchkeeping |
| Battery (Large) | `MAS Battery 1..3` | Tractor Power Core Battery Bank · 9.0 MWh · 36 MW discharge |
| Battery (Large) | `MAS Battery 4..6` | Utility Sub-Floor Battery Bank · 9.0 MWh · 36 MW discharge |
| Sliding Door | `MAS Door Boarding Outer`, `MAS Door Boarding Inner` | Port dual-door boarding airlock threshold and cabin isolation |
| Sliding Door | `MAS Door Flight Deck Inner`, `MAS Door Roof Outer` | Utility flight deck dual-door ascent airlock (L5-L6) |
| Sliding Door | `MAS Door Stern Inner` | Utility stern dump descent airlock (L2-L3) |
| Sliding Door | `MAS Door Vault` | Power Core Vault maintenance entrance from Jefferies tube |
| Sliding Door | `MAS Door Gangway Fwd` | Tractor Aft Bulkhead gangway isolation door |
| Sliding Door | `MAS Door Gangway Aft` | Utility Forward Bulkhead gangway isolation door |
| Armory Locker | `MAS Armory Bridge` | Large Grid armory locker at [X +2, Z +10, L4] · Tier 4 tools & munitions |
| Armory Locker | `MAS Armory Workshop` | Large Grid armory locker at [X −2, Z −13, L3] · Tier 4 tools & spare bottles |
| Equipment Locker | `MAS Locker Survival Gear` | Equipment locker at [X +2, Z +9, L4] · Suit O2/H2 bottles & emergency packs |
| Decoy | `MAS Decoy Mast` | Dorsal lightning rod mast at [X0, Z +6, L6] · Faraday lightning diverter |
| Interior Light | `MAS Light Decon Port/Roof/Stern/Gangway` | Cyan `#00FFFF` UV decontamination washdown lamps (4 chambers) |
| Laser Multitool Turret | `MAS MT Turret Bow` | Forward crown repair turret at [X0, Z +9, L5] · 360° bow & tire repair |
| Laser Multitool Turret | `MAS MT Turret Port` | Forward port shoulder repair turret at [X −3, Z −10, L4] · Kestrel pad & hitch repair |
| Laser Multitool Turret | `MAS MT Turret Stbd` | Forward stbd shoulder repair turret at [X +3, Z −10, L4] · Kestrel pad & hitch repair |
| Laser Multitool Turret | `MAS MT Turret Stern` | Elevated gantry arch repair turret at [X0, Z −23, L7] · Mule pad & aft consist repair |
| Small Cargo Container | `MAS Hold Resupply Export` | Outbound peer resupply staging hopper at [X0, Z −20, L5] directly under `MAS MP Conn` |
| Small Cargo Container | `MAS Hold Munitions` | Sub-floor flash-shielded munitions bunker at [X0, Z −15, L2] · 250× NATO 25mm / 100× 5.56mm quota |
| Sound Block | `MAS Alarm Meteor` | High-decibel early warning meteor & combat alert siren at [X0, Z +8, L4] |
| Conveyor Sorter | `MAS Sorter Resupply Export` | SML resupply kit staging sorter at [X0, Z −20, L4] |
| Event Controller | `MAS EC Airlock Port/Roof/Stern` | Automated dual-door interlock and active gas-reclamation sequencers |
| Event Controller | `MAS EC Resupply` | Outbound resupply staging watchdog · Monitors `MAS Hold Resupply Export` |
| Event Controller | `MAS EC Meteor` | Automated meteor intercept & consist hardening sequence watchdog at [X0, Z +7, L3] |
| Event Controller | `MAS EC Ammo` | Autonomous SML ammunition manufacturing & restock watchdog at [X −1, Z −15, L3] |

| Event Controller | `MAS EC Stabilizers` | Blocks boom arm deployment until outriggers report locked |
| Event Controller | `MAS EC Level` | Auto-leveling bedrock jack sequence on sloped terrain |
| Event Controller | `MAS EC Ladder` | Auto-deploys crew ladder on approach; retracts on drive |
| Event Controller | `MAS EC Tailings` | Pulses stern stone ejectors when ore buffers > 50% |
| Event Controller | `MAS EC Hitch Speed` | Locks hitch yaw (100 MN·m) and clamps drawbar straight at speeds > 30 km/h |
| Event Controller | `MAS EC Hitch Rev` | Clamps hitch yaw to ±15° and inverts Axle 5 when reversing |
| Event Controller | `MAS EC DPU Drive` | Synchronizes Axles 4 & 5 traction and dynamic descent braking |
| Event Controller | `MAS-U EC Breakaway` | Deadman breakaway watchdog: drops keel skids and handbrake on hitch sever |
| Event Controller | `MAS EC Holds` | Monitors payload mass; alerts at 2,500 t limit |
| Event Controller | `MAS EC Handover` | Monitors Ground Base heartbeat; triggers C2 handover |
| Event Controller | `MAS EC Power` | Fuel watchdog: auto-starts `MAS Engine H2` on low uranium (< 1.0 kg) |
| Event Controller | `MAS EC Ice` | Ice intake watchdog: routes raw ice to O2/H2 generators when H2 < 90% |
| Event Controller | `MAS EC Combat` | Combat alert watchdog · Auto-triggers `RIG.OS.MODE CBT` on damage/target lock |
| Event Controller | `MAS EC Seismic` | Bedrock survey watchdog: pulses seismic scans on outrigger lock |
| Event Controller | `MAS EC Drone Repair` | Drone repair watchdog: pulses pad welders when Kestrel docked |
| Event Controller | `MAS EC LifeSupport` | O2 tank reserve & hull breach pressure watchdog (auto-seals doors) |
| Event Controller | `MAS EC BlackBox` | Power blackout watchdog: auto-triggers 50 km SOS distress beacon |
| Event Controller | `MAS EC Atmosphere Mode` | Planetary atmo watchdog: auto-toggles ambient scavenging vs vacuum closed-loop |

#### 4. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MAS EC Airlock Port`| Airlock Cycle Trigger | Egress / Ingress Cycle | Egress: Evacuate O2 -> Unlock outer door | Ingress: 3.0 s UV Decon -> Pressurize -> Unlock inner door |
| `MAS EC Airlock Roof`| Airlock Cycle Trigger | Egress / Ingress Cycle | Egress: Evacuate O2 -> Unlock outer hatch | Ingress: 3.0 s UV Decon -> Pressurize -> Unlock inner door |
| `MAS EC Airlock Stern`| Airlock Cycle Trigger | Egress / Ingress Cycle | Egress: Evacuate O2 -> Unlock outer hatch | Ingress: 3.0 s UV Decon -> Pressurize -> Unlock inner door |
| `MAS EC Stabilizers`| `MAS Outrigger Pad 1..4` | All 4 Pads = Locked | Enable `MAS Boom Joints` · Sound Ready Chime | Lock `MAS Boom Joints` · Fold Boom |
| `MAS EC Level` | `MAS Helm` Roll & Pitch | Roll / Pitch $> 1.0^\circ$ | Differential Piston Extension | Lock Mag Plates (0.0° Level) |
| `MAS EC Ladder` | `MAS Sensor Ladder` | Player within 2.5 m | Deploy Ladder (Hinge 90° Down) | Retract Ladder (Hinge 0° Flush) |
| `MAS EC Tailings` | Tractor Ore Buffer | Ore Fill $\ge 50\%$ | `MAS Sorter Tailings Stern` Drain ON · Ejectors ON | Sorter & Ejectors OFF |
| `MAS EC Hitch Speed`| Consist Speedometer | Speed $> 30\text{ km/h}$ | Hitch Torque = 100 MN·m · Lock `MAS Hitch Clamps` | Normal Hitch Torque (45 MN·m) · Unclamp |
| `MAS EC Hitch Rev` | Transmission Gear | Gear = Reverse | Clamp Yaw ±15° · Axle 5 Positive Tracking | Normal Yaw ±45° · Counter-Steer |
| `MAS EC DPU Drive` | Helm Throttle / Grade | Incline $> 5^\circ$ AND Throttle ON | Axles 4 & 5 Matching Push Torque (100%) | Neutral Tractive Balance |
| `MAS-U EC Breakaway`| Consist Handshake | Electrical / Merge Link = Lost | Deploy `MAS Utility Brake Keel` · Handbrake ON · SOS Beacon | Standby Isolation |
| `MAS EC Holds` | Combined Ore Mass | Ore Mass $\ge 2,500\text{ t}$ | Sound Payload Horn · IGC `TMS.MULE.CALL` | Normal Extraction Mode |
| `MAS EC MP` | `MAS MP Conn` | Status = Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle Retract (0°) |
| `MAS EC Resupply` | `MAS Hold Resupply Export` | Inventory Fill $> 0\%$ | Broadcast `TMS.RESUPPLY.DISPATCH` · Staging Lamp ON | Standby Isolation |
| `MAS EC Train` | `MAS Stern Dump Conn` | Status = Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Handover` | `GND-01` Heartbeat | Signal Detected | Run PB `RIG//OS-Lite` (Argument: `HANDOVER`) | Maintain Mothership Authority |
| `MAS EC Power` | `MAS Reactor 1, 2` Uranium | Uranium Mass $< 1.0\text{ kg}$ | `MAS Engine H2` Turn ON · Sound Fuel Horn | `MAS Engine H2` Turn OFF |
| `MAS EC Ice` | `MAS Tank H2 1, 2` Fill | Hydrogen Fill $< 90\%$ | Enable Ice Conveyor Sorter & O2/H2 Gens | Normal Conveyor Routing |
| `MAS EC Combat` | Hull Integrity / Target Lock | Damage Detected OR Hostile Lock | Engage `MAS Turrets` · Elevate `RIG.OS.MODE CBT` | Standby Watchkeeping |
| `MAS EC Seismic` | `MAS Seismic Outrigger 1..4` | All Outriggers = Locked | Pulse Seismic Scan · Publish `TMS.SCOUT.VEIN` | Standby |
| `MAS EC Drone Repair` | `MAS Kestrel Dock 1, 2` | Either Pad = Connected | Pulse `MAS Welders` ON (5.0 s timer) | `MAS Welders` OFF |
| `MAS EC LifeSupport` | `MAS Tank O2 1, 2` / Pressure | O2 $< 25\%$ OR Pressure $< 0.5$ atm | Sound O2 Alarm · Seal Compartment Doors | Normal Atmospheric Circulation |
| `MAS EC BlackBox` | Consist Power Output | Total MW Output $= 0.0$ MW | Activate `MAS Battery BlackBox` & SOS Beacon | Standby Isolation |
| `MAS EC Atmosphere Mode` | Ambient Atmospheric O2 | Ambient O2 $\ge 100\%$ | `MAS Vent Atmo Intake` Depressurize ON | `MAS Vent Atmo Intake` Power OFF |
| `MAS EC Meteor` | `MAS Turret Bow / Stern` | Target Detected (Meteors/Missiles) | Sound `MAS Alarm Meteor` · Stow Boom · Seal Doors · Clamp Drones | Silence `MAS Alarm Meteor` · Normal Posture |
| `MAS EC Ammo` | `MAS Hold Munitions` Ammo | Stock $< 60\%$ Quota | Queue Ammo in `MAS Utility Assembler` (Top Priority) | If Mg $< 50\text{ kg}$, Call `TMS.RESUPPLY.REQUEST` |

#### 5. IGC Channel Subscriptions (Ground Fleet & RIG//OS Bus)
* Network Beacon: `RIG.C2.HEARTBEAT` (`<origin_id>;<role:MOTHERSHIP|BASE|WORKER>;<gps_coords>`).
* Network Handover: `RIG.C2.HANDOVER` (`<source_id>;<target_id>;<asset_registry_payload>`).
* Environmental Mode: `TMS.ENV.MODE` (Broadcasts planetary atmosphere state: `<EARTH_SCAVENGE|CLOSED_LOOP_VACUUM|CORROSIVE_ISOLATION>`).
* Airlock Telemetry: `TMS.AIRLOCK.CYCLE` (Telemetry packet: `<chamber_id>;<state:DEPRESSURIZING|EVACUATING|DECON_PULSE|PRESSURIZED>`).
* Meteor Alert: `TMS.METEOR.ALERT` (`METEOR;<intensity:LIGHT|MED|HEAVY>;<azimuth_deg>;<elevation_deg>;<impact_eta_sec>`).
* Munitions Telemetry: `TMS.AMMO.STATUS` (`AMMO;<gatling_box_count>;<interior_mag_count>;<mg_ingot_kg>;<deficit_box_count>`).
* Logistics Outbound: `TMS.MULE.CALL` (`CALL;<requesting_pad>;<priority>;<payload_type>`).
* Logistics Outbound: `TMS.CAT.CALL` (`CALL;<consist_id>;<bulk_ore_t>;<gps_coords>`).
* Consist Telemetry: `TMS.CONSIST.SYNC` (`SYNC;<source_id>;<hitch_torque>;<clamp_state>;<airlock_state>`).
* Powertrain DPU: `TMS.CONSIST.TRACTION` (`DPU;<mode:PUSH|BRAKE|COAST>;<torque_pct>;<axle4_slip>;<axle5_slip>`).
* Emergency Breakaway: `TMS.CONSIST.SEVER` (`SEVER;<source_id>;<reason>;<keel_skids_engaged>`).
* Resupply Inbound: `TMS.RESUPPLY.REQUEST` (`REQUEST;<requester_id>;<kit_name>;<gps_coords>`).
* Resupply Broadcast: `TMS.RESUPPLY.OFFER` (`OFFER;<provider_id>;<kit_name>;<eta_s>`).
* Resupply Dispatch: `TMS.RESUPPLY.DISPATCH` (`DISPATCH;<provider_id>;<transporter_id:MULE-01>;<target_id>;<kit_name>`).
* Resupply Confirmation: `TMS.RESUPPLY.COMPLETE` (`COMPLETE;<target_id>;<kit_name>;<status:DELIVERED>`).
* Fleet Telemetry: `TMS.MAS.STATUS` (`<consist_id>;<ore_mass_t>;<payload_pct>;<gps_coords>;<fob_status>`).
* Geological Inbound: `TMS.SCOUT.VEIN` (Receives target ore vein coordinates from Kestrel drones).
* Subordinate Dispatch: `FLT.MAS.CMD` (`<target_id>;<command>;<waypoint_gps>`).

---

### Phase 10-8 · Commissioning & Field Acceptance Checklist (RIG Test)

#### A · Articulated Consist Powertrain Trials
* [ ] **Tractive Articulation Test:** Drive consist across rolling dunes at 35 km/h. Verify 2-axis drawbar hitch pivots smoothly in yaw and pitch without jackknifing or lifting carriage wheels.
* [ ] **Counter-Steer Tracking:** Verify Axle 5 counter-steers, pulling the utility carriage cleanly through tight 35 m radius curves with zero curb clipping.
* [ ] **Slope Holding:** Stop consist on 25° incline. Confirm wheel handbrakes and 100% friction settings prevent downslope roll.

#### B · Outrigger Anchoring & Automated Boom Macros
* [ ] **Outrigger Bedrock Anchor:** Press hotkey 1 (`Deploy Stabilizers`). Confirm all 4 corner pistons extend downward and magnetic plates achieve 100% voxel lock. Confirm chassis is completely rigid.
* [ ] **Travel Stow Clamp Lock (STOW/DEPLOY):** Trigger `STOW` command. Confirm turntable slews to 180° aft, shoulder/elbow/wrist hinges fold to 0° flat along dorsal spine, and tool head locks into `MAS Clamp Boom Stow` at `[X0, Z −7, L4]`. Confirm 5.2 m transit profile and zero bridge obstruction. Trigger `DEPLOY`; confirm clamp releases and boom elevates to ready position.
* [ ] **Symmetrical 21.25 m Flank Reach:** Slew boom to Port (90°) and Starboard (270°). Confirm boom achieves identical 21.25 m physical reach beyond outer tires on both flanks with zero hull collision.
* [ ] **Target-Centric Vertical Column Bore:** Trigger Macro 2 (`BORE <GPS>`). Confirm 90° downward tool adapter locks drills into true vertical orientation [0, -1, 0] across full reach envelope (6.0 m to 35.0 m) while boom coordinates lateral translation and plunge around target center.
* [ ] **Bench Quarry Macro Test:** Trigger `MAS Timer Bench`. Verify boom executes automated -45° to +45° horizontal sweeps with -2.5° incremental shoulder steps and flat floor leveling.
* [ ] **Collision Sensor Stop:** Drive boom tool head toward obstacle; confirm `MAS Sensor Boom` stops joint movement at 4.0 m distance.

#### C · Mobile FOB & Dual Logistics Verification
* [ ] **Mule Air Offload:** Land MULE-01 on utility roof pad. Verify cradle arms swing 30° inward, magnetic plates clamp, and rare ores drain at $> 150\text{ t/min}$.
* [ ] **Caterpillar Train Rail Dump:** Reverse Caterpillar train into stern bumper. Verify large connector locks and 2,500 t bulk ore dumps in $< 90\text{ seconds}$.
* [ ] **Field Workshop Operations:** Queue 1,000 Steel Plates in `MAS Utility Assembler`. Verify autonomous fabrication using mined iron ingots.

#### D · C2 Fleet Orchestration & Handover Trials
* [ ] **Mothership Heartbeat Broadcast:** Confirm `MAS-01` broadcasts `RIG.C2.HEARTBEAT;ROLE=MOTHERSHIP` on 50 km radio and laser comms.
* [ ] **Subordinate Worker Binding:** Simulate subordinate worker `MAS-02`; confirm it binds to `MAS-01` and appears on Station 3 Tactical Video Wall.
* [ ] **Logistics Task Dispatch:** Trigger 90% ore buffer threshold; verify `RIG//OS-Lite` autonomously broadcasts `TMS.MULE.CALL` and `TMS.CAT.CALL`.
* [ ] **Ground Base Handover Protocol:** Simulate `GND-01` Ground Base C2 boot; verify `MAS-01` detects `ROLE=MASTER_BASE`, transmits fleet registry, and steps down to `FIELD_LEADER` without human intervention.

#### E · 5-Point Immersive Automation Verification
* [ ] **Auto-Leveling Bedrock Jacks:** Stop consist on 15° slope; trigger `MAS EC Level`. Verify downhill pistons extend further and deck achieves 0.0° pitch and 0.0° roll before mag plates lock.
* [ ] **Retractable Boarding Ladder:** Approach Port flank on foot; verify ladder swings down 90°. Release handbrake; verify ladder auto-retracts flush with armor skirt.
* [ ] **Active Gravity-Horizon Tool Head:** Elevate shoulder hinge to +45°; confirm wrist pitch automatically counter-rotates -15° to maintain level cutting beam.
* [ ] **Tailings Aggregate Spreader:** Mine stone until buffer > 50%; verify `MAS Sorter Tailings` and ejectors vent gravel behind wheels.
* [ ] **Smart Anti-Jackknife Drawbar:** Accelerate above 35 km/h; verify rotor braking torque jumps to 100 MN·m. Shift to reverse; verify yaw clamp limits to ±15°.

#### F · Tri-Fuel Power Redundancy & Closed-Loop Field Metallurgy Acceptance
* [ ] **Closed-Loop Metallurgy Cycle:** Laser-mine 50 t Iron Ore with articulated boom. Verify ore flows into `MAS Field Refinery`, refines into Iron Ingots, and transfers into `MAS Utility Assembler` to fabricate 250 Steel Plates with zero manual inventory handling.
* [ ] **Tier 1 to Tier 3 Power Failover:** Cut uranium supply to `MAS Reactor 1, 2`. Confirm `MAS EC Power` triggers, cold-starts `MAS Engine H2` within 2.0 s, and maintains full tractive and boom operational capability (5.0 MW).
* [ ] **Tier 2 Zero-Fuel Watchkeeping:** Turn off nuclear reactors and H2 engine during daylight. Confirm `MAS Solar 1..4` deliver daylight power, maintaining life support, telemetry, and battery float-charging.
* [ ] **Automated Ice Replenishment:** Mine 20 t Ice. Confirm `MAS EC Ice` routes ice to O2/H2 generators and begins refilling the 10,000,000 L hydrogen reserve.

#### G · Player Circulation, Maintenance Access & Hybrid Power Modes Acceptance
* [ ] **Ground-to-Bridge Boarding Flow:** Step onto Port L2 boarding ladder from ground level (L0). Verify ladder elevates crew to L2, airlock door cycles, and interior ladder leads up to Station 1 Helm.
* [ ] **Jefferies Tube Traversal:** Walk full length of Port maintenance crawlway from Z +6 to Z −7. Confirm physical cargo hatch on `MAS Tractor Buffer` opens and allows manual inventory transfer.
* [ ] **Power Vault Ingress:** Open `MAS Door Vault`. Step inside power compartment; confirm walk-up access to manual fuel hatches on `MAS Reactor 1, 2` and manual shutoff valve on `MAS Engine H2`.
* [ ] **Inter-Carriage Gangway Transit:** Walk across flexible accordion gangway while consist is in motion at 35 km/h. Confirm gangway remains airtight, isolates crew from moving hitch joints, and permits unhindered passage between carriages.
* [ ] **Flight Deck Access & Catwalk Inspection:** Climb forward ladderwell from workshop to Level L6. Walk full perimeter catwalk with safety railings around Mule pad, forward to Kestrel pads, and along solar inspection gangways.
* [ ] **Stern Dump Descent:** Descend aft ladderwell to Level L2 stern bumper. Confirm safe physical access to `MAS Stern Dump Conn` and `MAS Stern Sorter`.
* [ ] **Hybrid Power 5-Mode Sequencing:**
  - Test Mode 1 (Silent Watchkeeping): Reactors and H2 engine OFF; confirm `MAS Solar 1..4` float-charge 18.0 MWh battery bank.
  - Test Mode 2 (Tractive Cruise): Single reactor active; verify batteries smooth acceleration spikes.
  - Test Mode 3 (Heavy Excavation): Twin reactors active; verify continuous 30 MW supply to boom lasers, outriggers, and workshop.
  - Test Mode 4 (Emergency Failover): Cut uranium; confirm `MAS EC Power` starts `MAS Engine H2` and sheds workshop loads.
  - Test Mode 5 (Shore Power Fast Charge): Connect to external grid; verify batteries switch to 100% recharge rate.

#### H · Perimeter Defense, Combat Alert & Concrete Recycling Acceptance
* [ ] **Automated Combat Mode Elevate:** Simulate hostile target acquisition or hull damage. Confirm Event Controller `MAS EC Combat` transitions alert status to `RIG.OS.MODE CBT`, powers up all turrets, seals crew airlocks, and sounds internal alarms.
* [ ] **Bow & Stern CIWS Firing Arcs:** Confirm `MAS Turret Bow` tracks and engages targets across 360° forward/zenith hemisphere without clipping C2 bridge canopy or boom pedestal. Confirm `MAS Turret Stern` provides 270° coverage over aft hemisphere and landing approach.
* [ ] **Flank Anti-Personnel Coverage:** Verify `MAS Turret Flank Port` and `MAS Turret Flank Stbd` suppress ground-level targets around the boarding ladder and wheel wells.
* [ ] **Automated Ammo Delivery:** Confirm NATO 25×184mm ammunition pulls automatically from `MAS Hold Components` and `MAS Utility Assembler` through the conveyor spine.
* [ ] **Concrete Component Recycling (Decision 60):** Laser-mine 50 t stone. Verify gravel byproduct routes into `MAS Utility Assembler`, queued to fabricate `Concrete Components` for Caterpillar roadbed slabs and Ground Base foundation blocks.

#### I · Field Logistics, Crane Grapple, Seismic Voxel Scan & Floodlight Acceptance
* [ ] **Deep Seismic Subsurface Scan (Decision 62):** Deploy outriggers to bedrock lock on hillside. Confirm Event Controller `MAS EC Seismic` triggers acoustic resonance scan down to 500 m† depth, detecting deep ore deposits and transmitting coordinates to `TMS.SCOUT.VEIN`.
* [ ] **45 m Crane Grapple & Vehicle Righting (Decision 62):** Select Macro 4 `CRANE_RECOVERY`. Maneuver boom via CTC mouse control to attach `MAS Tool Grapple` to overturned target vehicle. Confirm 45 MN·m joint torque successfully rights vehicle without chassis flex.
* [ ] **Automated Flight Deck Drone Welder Repair (Decision 62):** Dock battle-damaged or thruster-worn Kestrel drone onto `MAS Kestrel Dock 1`. Confirm `MAS EC Drone Repair` pulses recessed ship welders for 5.0 s, completing full field repairs from onboard parts hold.
* [ ] **Atmospheric O2 Scavenging & Ground EVA Lockers (Decision 62):** In planetary atmosphere, verify `MAS Vent Atmo Intake Port/Stbd` fill life support tanks at zero ice cost. Walk to ground level L2; open `MAS Locker Port/Stbd` to access hydrogen suit bottles and emergency tools without entering airlock.
* [ ] **Worksite 360° Floodlight Matrix (Decision 62):** Toggle group `MAS Lights`. Confirm tower spotlights (`MAS Floodlight Tower 1..4`), boom spotlights (`MAS Light Boom 1, 2`), and underbelly inspection lights (`MAS Lights Underbelly`) fully illuminate 100 m perimeter during nighttime mining operations.

#### J · Custom Bridge, Universal Life Support & Emergency Failsafes Acceptance (Decision 64)
* [ ] **Custom Walk-In Bridge Circulation:** Walk from aft airlock door through open aisle to all 3 Control Stations (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`). Confirm unobstructed panoramic view through 45° sloped armor glass canopy and walk-around access to Survival Kit and Tactical Video Wall.
* [ ] **Multi-Page Hotbar Verification:** Test Hotbars 1, 2, and 3 on all three operator stations. Confirm Station 1 controls traction/leveling (HB1), power modes/reactors (HB2), and boarding/lights (HB3). Confirm Station 2 controls boom joints/lasers (HB1), automated macros (HB2), and buffers/ejectors (HB3). Confirm Station 3 controls logistics calls (HB1), drone slave links (HB2), and CIWS defense (HB3).
* [ ] **Universal Life Support (ULSS) Pressure & Isolation:** Depressurize one compartment (simulate breach); confirm `MAS EC LifeSupport` auto-seals bounding armored sliding doors in < 0.5 s, preserving 100% O2 in surviving zones. Confirm 200,000 L O2 tanks maintain life support during simulated vacuum transit.
* [ ] **Active Gas-Reclamation Airlock Cycling:** Test Port boarding, roof flight deck, and stern dump airlocks. Verify `Depressurize = ON` reclaims 100% of room air into O2 tanks before outer doors open, preventing explosive decompression in vacuum or toxic environments.
* [ ] **Cryo Hibernation Verification:** Enter `MAS Cryo Pod 1` and `MAS Cryo Pod 2` in Medical Bay. Confirm direct conveyor link supplies O2 and suspends suit life support drain.
* [ ] **Quarantined Emergency Ice Buffer:** Mine 20 t raw ice. Confirm ice diverts into `MAS Hold Ice LifeSupport` (15,000 L) and is preserved even when main ore buffers are dumped into the Caterpillar land train.
* [ ] **Pyrotechnic Hitch Severance Test:** Trigger `Hitch Decouple`. Confirm merge block `MAS Hitch Sever Tongue` disengages, severing Carriage 1 from Carriage 2 cleanly. Confirm Tractor drives away as an independent 6×6 unit and Utility Carriage remains powered and airtight as an autonomous FOB.
* [ ] **Self-Righting Keel Gyro Override:** Roll consist onto its side; trigger `MAS Gyro Recovery 1, 2` roll override. Confirm angular torque successfully rights the vehicle back onto its wheels.
* [ ] **Black Box SOS Beacon Blackout Test:** Disconnect all main reactors and batteries. Confirm `MAS EC BlackBox` instantly wakes isolated `MAS Battery BlackBox`, broadcasting 50 km emergency distress ping `SOS-MASTODON-01-LOST`.
* [ ] **Genesis Self-Repair Projector Alignment:** Activate `MAS Projector Master`. Confirm 1,039-block ghost blueprint aligns with 0.0 offset to the physical chassis, allowing repair welders to rebuild damaged blocks directly from `MAS Hold Components`.
* [ ] **Anti-Capture Scuttle Countdown & Manual Abort:** Trigger Button 2 on `MAS BP Scuttle Bridge`. Confirm `MAS Timer Scuttle` initiates 60 s countdown, sirens sound, and interior lights flash red. Press Button 4 before T - 5 s; confirm countdown aborts and warheads return to safety disarm.

#### K · Tri-Mode Laser Multitool Salvage & Remote Welding Acceptance (Decision 65)
* [ ] **Tri-Mode Operational Cycle:** Cycle `MAS Laser 1, 2` via Station 2 Hotbar 1 Slot 8. Confirm HUD feedback confirms mode switching across `Mining Beam (Drill)` -> `Salvage Beam (Grinder)` -> `Fabrication Beam (Welder)` -> `Mining Beam`.
* [ ] **Macro 5 Automated Planar Salvage Sweep:** Position boom facing a test hull target. Trigger Macro 5 `SALVAGE` via Station 2 Hotbar 2 Slot 8. Confirm `[MAS] PB Excavator` positions tool head at 15 to 30 m standoff, maintains optimal perpendicular beam alignment, and executes 2D serpentine passes (ΔY, ΔX) peeling armor plates layer-by-layer without physical contact or collision jitter.
* [ ] **100% Component Retention Verification:** Deconstruct functional target block (e.g. Battery, Large Hydrogen Thruster). Verify 100% of disassembled components (Power Cells, Thruster Components) are drawn through tool head conveyor vacuum into `MAS Hold Components` with zero floating debris ejection.
* [ ] **Reverse Metallurgy Disassembly Loop:** Queue disassembly of 100 Interior Plates in `MAS Utility Assembler`. Confirm surplus plates are reclaimed into constituent Iron Ingots and transferred into manufacturing stock.
* [ ] **Scrap Metal Smelting Acceptance:** Route 500 kg Scrap Metal into `MAS Field Refinery`. Confirm refinery consumes 330 kW, melting scrap into pure Iron Ingots to feed ongoing component fabrication.
* [ ] **35 m Remote Field Fabrication & Structural Welding:** Switch lasers to Mode 3: Fabrication Beam (Welder). Aim boom at projected bridge block girder 30 m across test chasm. Verify remote beam welds block to 100% completion without requiring engineer EVA in hazardous conditions.

#### L · Multi-Tier Fleet Comms, Predictive JIT Logistics & Swarm Synergy Acceptance (Decision 66)
* [ ] **Predictive JIT Transport Pre-Dispatch:** Mine ore continuously at high rate. Confirm `[MAS] PB Excavator` computes real-time extraction rate (kg/s) and remaining fill time (T_remain). At 75-80% buffer threshold, confirm `RIG//OS-Lite` automatically broadcasts `TMS.LOGISTICS.JIT`, pre-dispatching Mule or Caterpillar so transport touches down precisely as buffers reach 95%, achieving zero-downtime hot extraction.
* [ ] **Dynamic Logistics Triage Separation:** Load mixed batch containing Uranium and Stone. Confirm rare ores route to `MAS MP Sorter` (roof Mule pad) while stone aggregate routes to `MAS Stern Sorter` (Caterpillar rail dump).
* [ ] **Two-Tier Prospecting & Acoustic Voxel Scan:** Trigger `TASK KESTREL` via Station 3 Hotbar 2 Slot 5. Verify Kestrel flies grid survey, identifies surface anomaly, and transmits `TMS.SCOUT.VEIN`. Move Mastodon to waypoint; trigger `SEISMIC` (HB2:6); confirm 4× outriggers execute 500 m deep acoustic pulse and transmit 3D vein centroid (`TMS.SEISMIC.SURVEY`) directly into Macro 2 (`BORE`).
* [ ] **Aerial IGC Line-of-Sight Relay Mesh:** Drive Mastodon into deep crater breaking direct laser line-of-sight to Ground Base. Confirm docked Kestrel launches to 500 m AGL, switches to Relay Mode, and maintains seamless bidirectional IGC packet delivery between Mastodon and Ground Base C2.
* [ ] **Demand-Driven Pull Mining Protocol:** Simulate Ground Base broadcasting `TMS.BASE.DEMAND;COBALT;5;150000;250000`. Confirm Mastodon C2 automatically filters local survey database for nearest Cobalt vein and highlights target waypoint on Station 3 Tactical Video Wall.
* [ ] **Multi-Consist Swarm Deconfliction:** Simulate sister excavator `MAS-02` within 200 m radius. Trigger `CLAIM ZONE` (HB2:7); confirm `TMS.ZONE.CLAIM` broadcasts bounding box exclusion zone, preventing boom joint interference during synchronized quarrying.
* [ ] **Coordinated CIWS Fleet Defense Interlock:** Simulate incoming hostile pirate drone. Confirm `[MAS] PB RIG//OS-Lite` broadcasts `TMS.DEFENSE.TARGET` with threat priority, distributing target tracking between `MAS-01` and `MAS-02` turrets to prevent ammunition waste.

#### M · Consist Mechanics, 3-Axis Hitch, DPU Powertrain & FOB Autonomy Acceptance (Decision 67)
* [ ] **3-Axis Gimbal Hitch Compliance:** Drive consist across boulder-strewn terrain and steep dune ridges. Confirm hitch articulates freely across yaw (±45°), pitch (±25°), and torsional roll (±20°) without subgrid binding, Clang physics glitching, or wheel lift.
* [ ] **Anti-Oscillation Magnetic Clamping:** Accelerate consist above 30 km/h on flat salt flats. Confirm Event Controller `MAS EC Hitch Speed` locks twin magnetic clamps (`MAS Hitch Clamp Port/Stbd`), freezing hitch oscillation and stabilizing high-speed highway transit.
* [ ] **Distributed Power Unit (DPU) Hill-Climb Push:** Drive loaded consist up a 20° mountain grade. Confirm `[MAS] PB Excavator` engages DPU traction, driving Axles 4 and 5 at 100% matching torque to eliminate dead-drag on the drawbar hitch.
* [ ] **Predictive Regenerative Descent Braking:** Drive down a steep 25° incline. Confirm Carriage 2 wheels dynamically apply counter-braking before Tractor brakes bite, maintaining drawbar tension and preventing trailer jackknifing.
* [ ] **Autonomous FOB Detachment ("Drop-and-Mine"):** Trigger `Hitch Decouple`. Drive Tractor away as an independent 6×6 extraction vehicle. Confirm Carriage 2 computer core (`[MAS-U] PB Utility` and `MAS-U Remote FOB`) initializes, maintaining continuous life support, refinery recycling, battery charging, and stern CIWS watchkeeping.
* [ ] **Runaway Breakaway Deadman Brake:** Sever merge connection while on a 15° slope without tractor attached. Confirm `MAS-U EC Breakaway` instantly engages handbrakes and drops keel magnetic skids (`MAS Utility Brake Keel 1, 2`) into the terrain, halting runaway trailer slide within 1 tick.
* [ ] **Sub-Floor Ballast Low-CoM Verification:** Execute high-speed 35 km/h evasive slalom maneuvers. Confirm 40× Heavy Armor sub-floor ballast skids maintain Carriage 2 Center of Mass below Level L2.4, preventing rollover up to 34° side tilt.
* [ ] **Stern Bedrock Outrigger Mule Touchdown Shock Isolation:** Extend stern outriggers (`MAS Outrigger Utility Port/Stbd`) to bedrock lock. Land fully-laden 650 t MULE-01 onto Level L6 gantry pad. Confirm touchdown impact forces transfer directly into terrain with zero suspension deflection or chassis bucking.
* [ ] **42 m Triangulated Seismic Prospecting Baseline:** Deploy all 6 outriggers (4 forward, 2 aft). Trigger `SEISMIC`. Confirm 42 m acoustic baseline achieves sub-meter centroid accuracy on deep ore scans down to 500 m† depth.
* [ ] **Telescoping Soft-Capture Caterpillar Docking:** Back Caterpillar train to 2.0 m standoff behind stern bumper. Extend `MAS Piston Stern Dock`; confirm magnetic capture locks cleanly without vehicle impact. Retract piston and verify 2,500 t bulk ore dumps at high speed.
* [ ] **Relocated Stern Roadbed Aggregate Trail:** Mine stone with buffer > 50%. Confirm `MAS Sorter Tailings Stern` and 4× ejectors vent stone aggregate safely behind the stern bumper ([X0, Z −26, L1]), laying a continuous compacted roadbed without gravel impacting wheels.

#### N · 4× Multitool Repair Turrets, SML Auto-Production & Peer Resupply Sortie Acceptance (Decision 68)
* [ ] **360° Overlapping Repair Envelope:** Position damaged armor blocks at Bow (Z +11), Hitch (Z −8), and Stern Dump (Z −26). Verify `MAS MT Turret Bow`, `MAS MT Turret Port/Stbd`, and `MAS MT Turret Stern` achieve 100% spherical repair line-of-sight across every block of the consist, welding all damage to 100% without engineer EVA.
* [ ] **Automated Flight Deck Drone Servicing:** Dock damaged Kestrel drone on Port pad (L5) and damaged Mule shuttle on top gantry pad (L6). Confirm `MAS MT Turret Port` and elevated `MAS MT Turret Stern` auto-target damaged thrusters, armor, and conveyor lines, restoring both craft to full operational readiness while engineers remain inside the pressurized bridge.
* [ ] **SML Autonomous Quota Monitoring:** Withdraw 1,000 Steel Plates from `MAS Hold Components` (reducing stock below 75% of 1,500 plate quota). Confirm `[MAS-U] PB Utility` detects deficit and automatically queues 1,000 Steel Plates in `MAS Utility Assembler`.
* [ ] **Closed-Loop Metallurgy to Component Path:** Mine Iron Ore with the 35 m boom. Verify ore refines into Iron Ingots in `MAS Field Refinery` (330 kW) and immediately flows into `MAS Utility Assembler` to fulfill the SML plate quota, replenishing `MAS Hold Components` with zero manual intervention.
* [ ] **Resupply Kit Assembly & Staging:** Issue command `RESUPPLY KIT_MOBILITY`. Confirm `MAS Sorter Resupply Export` powers ON with Drain All = ON, pulling exactly 200 Steel Plates, 100 Motors, 50 Large Tubes, and 50 Small Tubes into `MAS Hold Resupply Export` ([X0, Z −20, L5]), then shuts OFF.
* [ ] **High-Speed Mule Resupply Transfer:** Land MULE-01 on `MAS MP Conn`. Confirm `MAS Hold Resupply Export` empties completely into Mule cargo hold in $< 10\text{ seconds}$ flat without drawing from or sorting through the main 3×3×3 cargo hold.
* [ ] **Inter-Rig Peer Assistance IGC Handshake:** Simulate emergency distress transmission `TMS.RESUPPLY.REQUEST;MAS-02;KIT_POWER;<gps>`. Confirm Mastodon C2 automatically stages `KIT_POWER`, broadcasts `TMS.RESUPPLY.OFFER`, summons MULE-01, and issues `TMS.RESUPPLY.DISPATCH` upon shuttle takeoff.

#### O · Extended Survival Ergonomics, Automated Decontamination Airlocks & Non-Earthlike Environmental Acceptance (Decision 69)
* [ ] **Bridge & Workshop Armory Loadout:** Verify `MAS Armory Bridge` ([X +2, Z +10, L4]) and `MAS Armory Workshop` ([X −2, Z −13, L3]) are fully stocked with Tier 4 Elite tools (welder, grinder, drill), automatic rifles, 5.56mm ammo, and spare suit bottles. Verify `MAS Locker Survival Gear` ([X +2, Z +9, L4]) is stocked with emergency O2/H2 suit bottles and data pads.
* [ ] **Dedicated Suit Recharge Turnaround:** Connect engineer suit to recharge ports adjacent to `MAS Survival Kit Tractor` (Bridge) and `MAS Medical Bay` (Workshop). Confirm suit oxygen, hydrogen, and battery power recharge from 0% to 100% in $< 15\text{ seconds}$ without entering cryo pods.
* [ ] **120+ Hour Quarantined Emergency Ice Reserve:** Verify `MAS Hold Ice LifeSupport` retains its 15,000 L emergency ice reserve isolated from general cargo transfers. Calculate melt rate: confirm reserve sustains continuous breathing air and hydrogen engine failover for $> 120\text{ hours}$ in unpowered drift.
* [ ] **4-Zone Hermetic Cabin Circulation:** Cycle vents in Zone 1 (Bridge), Zone 2 (Crawlway/Vault), Zone 3 (Gangway Bellows), and Zone 4 (Workshop). Confirm each zone maintains independent 100% airtight pressure and isolates immediately upon bulkhead door closure.
* [ ] **Dual-Atmosphere Ambient Scavenging Automation:** Land Mastodon in 100% Earth-like atmosphere. Confirm Event Controller `MAS EC Atmosphere Mode` enables `MAS Vent Atmo Intake Port/Stbd` with `Depressurize = ON`, refilling 200,000 L O2 reserves with zero ice consumption. Transition to vacuum/toxic planetary environment; confirm exterior vents instantly shut off, cabin auto-seals, and closed-loop recirculation engages.
* [ ] **Active Gas-Reclamation Port Airlock Cycling:** Cycle Port boarding airlock outbound into vacuum. Verify vent evacuates 100% of chamber O2 back into storage tanks (0.0 atm) before `MAS Door Boarding Outer` unlocks (zero gas venting). Cycle inbound: confirm `MAS Light Decon Port` pulses cyan UV washdown for 3.0 s, chamber pressurizes to 1.0 atm, and `MAS Door Boarding Inner` unlocks into the bridge.
* [ ] **Dual-Door Flight Deck & Stern Airlock Interlocks:** Test roof flight deck airlock (L5-L6) and stern dump airlock (L2-L3). Confirm `MAS EC Airlock Roof` and `MAS EC Airlock Stern` enforce strict interlocks preventing inner and outer sliding doors from opening simultaneously. Verify UV decon pulse on every inbound cycle.
* [ ] **Faraday Lightning Diverter Grid:** Subject Mastodon to planetary lightning storm on Pertam. Confirm dorsal mast decoy `MAS Decoy Mast` ([X0, Z +6, L6]) attracts 100% of ground strikes, dissipating electrical discharge through the heavy armor spine to the terrain without damaging glass canopy, solar panels, or hydrogen tanks.
* [ ] **Radiation Storm Citadel Shelter:** Simulate planetary coronal mass ejection / cosmic ray storm. Station crew in the central workshop corridor ([X0, Z −13 to −18, L3]). Confirm lateral shielding from twin 4.5 m liquid hydrogen tanks and 40× heavy steel ballast plates attenuates radiation dose to 0.0 mSv/h.
* [ ] **Cryogenic Thermal Trace Freeze Protection:** Park consist in sub-zero cryogenic environment (Europa / Triton). Confirm 330 kW waste heat from `MAS Field Refinery` jackets `MAS Hold Ice LifeSupport`, maintaining ice liquidity and preventing hopper freeze-up.
* [ ] **Automated Meteor Intercept Defense:** Trigger incoming meteor shower. Confirm CIWS turrets (`MAS Turret Bow / Stern`) track and neutralize inbound meteors before ground impact (`Target Meteors = ON`).

#### P · Automated Meteor Defense Grid, Closed-Loop Ammo Fabrication & Dynamic Restock Acceptance (Decision 70)
* [ ] **360° Hemispherical CIWS Meteor Dome:** Trigger planetary meteor storm. Verify `MAS Turret Bow` (Bow crown L5) and `MAS Turret Stern` (elevated gantry arch L7) achieve 360° hemispherical overlap with `Target Meteors = ON`, acquiring and destroying incoming meteors at > 800 m standoff before they impact the glass canopy, solar panels, or hydrogen tanks.
* [ ] **Automated Early Warning Siren & Consist Hardening:** On meteor target acquisition, verify Event Controller `MAS EC Meteor` triggers high-decibel audible siren `MAS Alarm Meteor` ([X0, Z +8, L4]). Verify `[MAS] PB Excavator` initiates emergency `STOW` macro: 35 m boom folds 180° aft into dorsal spine trough and locks into `MAS Clamp Boom Stow` ([X0, Z −7, L4]) in < 4.0 s, dropping vehicle profile to 5.2 m. Verify boarding ladder retracts flush, airlocks lock, and drone magnetic clamps lock to isolate kinetic ground shock.
* [ ] **Autonomous Closed-Loop Munitions Manufacturing:** Withdraw 120 NATO 25×184mm ammo boxes from `MAS Hold Munitions` (dropping stock below 60% of the 250-box SML quota). Confirm `MAS EC Ammo` triggers `MAS Utility Assembler` to immediately queue 120 NATO 25mm boxes and 50 NATO 5.56mm magazines from refined Magnesium, Iron, and Nickel ingots with top combat priority over structural plate production.
* [ ] **Flash-Shielded Sub-Floor Munitions Bunker Detonation Isolation:** Verify `MAS Hold Munitions` ([X0, Z −15, L2]) is enclosed within 40× heavy steel ballast plates beneath the workshop floor plate. Simulate kinetic penetration of upper hull; confirm sub-floor bunker isolates munitions storage and prevents sympathetic explosive cookoff into the habitable crew workshop.
* [ ] **High-Speed Conveyor Distribution & Sorter Pressurization:** Verify `MAS Sorter Ammo Fwd` ([X0, Z −9, L3]) with whitelist `NATO_25x184mm`, `NATO_5_56x45mm` and `Drain All = ON` continuously feeds forward CIWS turrets (`MAS Turret Bow`, `MAS Turret Flank Port/Stbd`) across the drawbar umbilical without manual conveyor transfers.
* [ ] **Automated Mule Munitions Resupply Sortie:** Reduce local Magnesium stock below 50 kg or ammo reserves below 25%. Confirm `[MAS] PB RIG//OS-Lite` broadcasts `TMS.RESUPPLY.REQUEST;MAS-01;KIT_MUNITIONS`. Verify Ground Base or `MULE-01` delivers pallet directly into `MAS Hold Munitions` via top-deck pad `MAS MP Conn` and `MAS MP Sorter` in < 10 s.









---

## Chapter 11 · CATERPILLAR-01 Land Train (CAT-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **CATERPILLAR-01 (CAT-01)** Heavy Wheeled Bulk Land Train.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 10, 25, 35, 41, 51, 52, 60, 66, 67 & 70)
* **Role:** High-volume terrestrial bulk ore and aggregate hauler operating on designated planetary surface corridors between the **MASTODON-01 MKI** heavy excavator and the **Ground Base Refinery Complex (GND-01)**. Serves as the high-tonnage Ground Tier transport backbone of the dual-tier logistics architecture (Decision 52), complementing the agile Air Tier MULE-01 freight shuttle.
* **Architecture:** 4-Unit Articulated Consist (42 B / 105.0 m total length):
  1. **Tractor Locomotive (`CAT-01-L`): 7 W × 9 L × 4 H B (4 Wheels · Axles 1 & 2):** Heavy tractive engine, forward-sloped armored panoramic single-operator engineer cab (`CAT Loco Helm`), 15 MW nuclear reactor vault + 12 MWh battery buffer across 4× Large Batteries (48 MW surge discharge), 25 km radio antenna mast, forward heavy push-bumper with large receiving connector (`CAT Loco Front Conn`) matching Mastodon's stern dump port, and aft 3-axis heavy motorized drawbar hitch.
  2. **Hopper Car 1 (`CAT-01-C1`): 5 W × 7 L × 4 H B (4 Wheels · Axles 3 & 4):** Modular bulk hopper, 4-wheel distributed power unit (DPU) smart traction assist, 1× Large Cargo Container (`CAT Hold 1`, 3×3×3 B, 1,386,000 L @ 3× Inventory), top-loading funnel chute, side inspection ports, and inter-car gangway plates.
  3. **Hopper Car 2 (`CAT-01-C2`): 5 W × 7 L × 4 H B (4 Wheels · Axles 5 & 6):** Identical modular bulk hopper unit (`CAT Hold 2`), mid-consist conveyor repeater line, DPU smart traction assist, and dual 3-axis drawbar joints.
  4. **Hopper Car 3 / Tender (`CAT-01-C3`): 5 W × 9 L × 4 H B (4 Wheels · Axles 7 & 8):** Final hopper unit (`CAT Hold 3`), stern high-speed terminal discharge manifold with pressurized Conveyor Sorter (`CAT Train Sorter`, Drain All = ON), stern large connector (`CAT Train Unload Conn`), and heavy collision bumper with alignment guide horns matching Ground Base Bulk Receiving Dock.
* **Grid:** Large Grid (1 B = 2.5 m) · **Consist Envelope:** 7 W × 42 L × 4 H B (17.5 × 105.0 × 10.0 m).
* **Mass:** Consist Dry Mass `≈ 368.5 t` (Locomotive: 148.2 t · Drawbars: 24.6 t · 3× Hoppers: 195.7 t) · Max Laden Mass `≈ 2,868.5 t` (Earth 1 g).
* **Payload Capacity:** 2,500 t bulk ore across 3× Large Cargo Container holds (4,158,000 L combined volume @ 3× Inventory).
* **Mechanical & Powertrain Suite (Decisions 10, 35 & 67):** 16× 5×5 Off-Road Suspension Wheels across 8 axles. Front steering axle on locomotive (±32.0°); all 14 trailing wheels track straight with 100% synchronized propulsion torque. 3× 3-Axis Motorized Conveyor Drawbars (Yaw Rotor ±45°, Pitch Hinge ±25°, Roll Rotor ±20°) with `Share Inertia Tensor = ON` and magnetic anti-sway clamps (`CAT Hitch Clamp Port/Stbd`) active at speed > 40 km/h.
* **Fast-Dump Terminal Performance (Decision 51):** High-speed pressurized discharge manifold empties 2,500 t of bulk ore into Ground Base in `< 90 seconds` via stern connector and in-deck collectors.
* **Operating Environment:** Earth 1 g baseline, surface roadbed, quarry benches, and cross-country haulage corridors.

---

## 0 · Reference Grid, In-Game Paint Palette & Master Consist Layout

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Midpoint of the Tractor Locomotive wheelbase at Level L1, along centerline X0.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. Spans X −3.5 to +3.5 on Locomotive (7 B wide = 17.5 m) and X −2.5 to +2.5 on Hopper Cars (5 B wide = 12.5 m).
* **Y (Elevation):** Wheel contact patch is Level L0 (Y = −1.25 m); wheel hubs and chassis box girders are Level L1 (Y = 0); cab deck and hopper midline are Level L2; hopper holds are Level L3; cab/hopper roof is Level L4 (10.0 m total height).
* **Z (Length / Longitudinal):** Stern discharge bumper is negative (−), Bow push-bumper is positive (+). Spans Z −17 to +25 (42 blocks total = 105.0 m).
  * Tractor Locomotive (`CAT-01-L`): Z +16 to +25 (9 blocks length = 22.5 m). Wheelbase datum Z0 is at Z +21.
  * Drawbar Hitch 1: Z +14 to +16 (2 blocks mechanical span = 5.0 m).
  * Hopper Car 1 (`CAT-01-C1`): Z +7 to +14 (7 blocks length = 17.5 m).
  * Drawbar Hitch 2: Z +5 to +7 (2 blocks mechanical span = 5.0 m).
  * Hopper Car 2 (`CAT-01-C2`): Z −2 to +5 (7 blocks length = 17.5 m).
  * Drawbar Hitch 3: Z −4 to −2 (2 blocks mechanical span = 5.0 m).
  * Hopper Car 3 / Tender (`CAT-01-C3`): Z −17 to −4 (9 blocks length = 22.5 m).

```
Spatial Alignment Triad:
       [+Y: Zenith / Consist Roof L4]
              │
              │   [+Z: Bow / Locomotive Push-Bumper]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Tender Discharge Bumper]
              │
       [−Y: Ground Contact / Level L0]
```

---

### 0.2 In-Game Paint Palette & Functional Color Coding

To eliminate builder confusion across a 105 m multi-unit consist, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Locomotive chassis, hopper undercarriages, wheel well frames, belly skids |
| **Slot 1** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Locomotive bow push-bumper, final car stern dump bumper, crash horns |
| **Slot 2** | Hazard Amber | `#E6A100` | Bright Amber | 3-Axis Drawbar Hitches (`CAT Hitch Yaw/Pitch/Roll`), jumper hoses, brake skids |
| **Slot 3** | Freight Transport Orange | `#D35400` | Deep Orange | Modular Hopper Car outer hull plating, top funnel intake chutes |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Train Conveyor Spine, large-port flexible drawbar pipes, sorters |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Modular Large Cargo Container holds (`CAT Hold 1, 2, 3`) |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Locomotive Power Vault (1× Small Reactor, 4× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Locomotive Armored Cab shell, driver console framing, beacon mounts |
| **Slot 8** | Clean Armor White | `#ECF0F1` | Clean White | Consist unit numbers (`LOCO-01`, `CAR-01..03`), roof warning stripes |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Side running boards, grab irons, maintenance ladders, brake reservoirs |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

To achieve a cohesive industrial locomotive aesthetic matching the Mastodon MKI and Ground Base, paint must follow strict functional zone lines across the consist:

```
[EXTERIOR ELEVATION LIVERY MAP · STARBOARD PROFILE (LOOKING PORT)]
Level
L4  │         [SLOT 7: NAVY CAB]               [SLOT 3: ORANGE TOP FUNNELS & CATWALKS (CAR 1-3)]
L3  │ ═══════[SLOT 7: NAVY CAB ROOF]════════════[SLOT 3: ORANGE HOPPER CAR ARMOR CASING]══════════
L2  │ ───────────────────[SLOT 1: CRIMSON BELTLINE STRIPE (Z +25 to Z −17)]──────────────────────
L1  │ ═══════[SLOT 0: INDUSTRIAL DARK GRAY CHASSIS BOX GIRDERS & WHEEL MOUNTS]════════════════════
L0  │    (O)=======(O)       [SLOT 2: DRAWBAR]      (O)=======(O)       [SLOT 2]      (O)=======(O)
        Axle 1    Axle 2                           Axle 3    Axle 4                  Axle 5    Axle 6
        ◄── LOCOMOTIVE ──►                         ◄── HOPPER 1 ──►                  ◄── HOPPER 2 ──►
```

| Zone / Component | Primary Palette Slot | Hex Code | Visual Application & Boundaries |
|---|---|---|---|
| **Chassis Box Girders** | Slot 0 (Industrial Dark Gray) | `#2B303A` | Level L1 structural sills, axle mounts, belly deflector plates across all 4 units |
| **Beltline Rail Stripe** | Slot 1 (Heavy Rail Crimson) | `#9E2A2B` | Level L2 continuous 1-block stripe running along flanks from bow bumper to stern dump |
| **Armored Driver Cab** | Slot 7 (Fleet Command Navy) | `#1B263B` | Levels L3-L4 forward cab shell, brow visor, roof crown, and rear cab bulkhead |
| **Hopper Car Envelopes** | Slot 3 (Freight Orange) | `#D35400` | Levels L2-L4 hopper hull side plates, top funnel chamfers, and corner transitions |
| **3-Axis Drawbar Hitches**| Slot 2 (Hazard Amber) | `#E6A100` | Levels L1-L2 articulated hinges, drawbar tongues, and magnetic clamp arms |
| **Crash & Alignment Bumpers**| Slot 1 (Heavy Rail Crimson) | `#9E2A2B` | Bow push-bumper (Z +25) and stern terminal dump bumper (Z −17) with yellow striping |

---

### 0.3 Master Consist Top Plan View (Levels L1-L4)

```
[MASTER CONSIST PLAN VIEW · COMPOSITE LEVELS L1-L4]
         Port (−X)                 Centerline (X0)               Starboard (+X)
Z: +25   ┌────────────────────────[CAT FRONT CONN]────────────────────────┐   ◄── Bow Push Bumper (L2)
         │ [CRIMSON BUMPER]        [CONVEYOR SPINE]      [CRIMSON BUMPER] │
Z: +23  (O) [WH 1: STEER P]       [POWER VAULT CORE]    (O) [WH 2: STEER S]   ◄── Axle 1 (±32° Steer)
Z: +22   │                         [CABIN: CAT HELM]                      │
Z: +21   │ [LARGE BATTERY 1,2]     [SMALL REACTOR]       [LARGE BATTERY 3,4]│  ◄── Wheelbase Datum Z0
Z: +19  (O) [WH 3: DRIVE P]       [CONVEYOR SPINE]      (O) [WH 4: DRIVE S]   ◄── Axle 2 (Fixed Drive)
Z: +17   └─────────────────────────[HITCH 1 BULKHEAD]────────────────────┘
Z: +15   ══════════════════════════[DRAWBAR HITCH 1]══════════════════════   ◄── 3-Axis Joint 1
Z: +14   ┌─────────────────────────[FRONT BULKHEAD]───────────────────────┐
Z: +13  (O) [WH 5: DPU P]         [CONVEYOR RISER]      (O) [WH 6: DPU S]     ◄── Axle 3 (Hopper 1 Fwd)
Z: +11   │ ╲                      ┌──────────────┐                      ╱ │
Z: +10.5 │  ╲                     │  CAT HOLD 1  │                     ╱  │   ◄── Large Cargo (3×3×3 B)
Z: +10   │   TOP FUNNEL CHUTE     │ (1,386,000 L)│     TOP FUNNEL CHUTE   │       Top Funnel Intake (L4)
Z: +8   (O) [WH 7: DPU P]         │              │      (O) [WH 8: DPU S]     ◄── Axle 4 (Hopper 1 Aft)
Z: +7    └────────────────────────┴──────────────┴────────────────────────┘
Z: +5    ══════════════════════════[DRAWBAR HITCH 2]══════════════════════   ◄── 3-Axis Joint 2
Z: +4    ┌─────────────────────────[FRONT BULKHEAD]───────────────────────┐
Z: +3   (O) [WH 9: DPU P]         [CONVEYOR RISER]      (O) [WH 10: DPU S]    ◄── Axle 5 (Hopper 2 Fwd)
Z: +1    │ ╲                      ┌──────────────┐                      ╱ │
Z: +0.5  │  ╲                     │  CAT HOLD 2  │                     ╱  │   ◄── Large Cargo (3×3×3 B)
Z: 0     │   TOP FUNNEL CHUTE     │ (1,386,000 L)│     TOP FUNNEL CHUTE   │
Z: −2   (O) [WH 11: DPU P]        │              │      (O) [WH 12: DPU S]    ◄── Axle 6 (Hopper 2 Aft)
Z: −3    └────────────────────────┴──────────────┴────────────────────────┘
Z: −5    ══════════════════════════[DRAWBAR HITCH 3]══════════════════════   ◄── 3-Axis Joint 3
Z: −6    ┌─────────────────────────[FRONT BULKHEAD]───────────────────────┐
Z: −7   (O) [WH 13: DPU P]        [CONVEYOR RISER]      (O) [WH 14: DPU S]    ◄── Axle 7 (Tender Fwd)
Z: −9    │ ╲                      ┌──────────────┐                      ╱ │
Z: −10.5 │  ╲                     │  CAT HOLD 3  │                     ╱  │   ◄── Large Cargo (3×3×3 B)
Z: −11   │   TOP FUNNEL CHUTE     │ (1,386,000 L)│     TOP FUNNEL CHUTE   │
Z: −13  (O) [WH 15: DPU P]        └──────┬───────┘      (O) [WH 16: DPU S]    ◄── Axle 8 (Tender Aft)
Z: −15   │                        [CAT TRAIN SORTER]                      │   ◄── Drain All = ON
Z: −17   └────────────────────────[CAT TRAIN UNLOAD]──────────────────────┘   ◄── Stern Dump Bumper (L2)
```

---

### 0.4 Master Consist Profile Elevation (Looking Starboard, X0 Plane)

```
[MASTER CONSIST PROFILE ELEVATION · TOTAL LENGTH: 42 B / 105.0 m]
      Z: +25 (Bow)      +21      +17     +14      +10.5     +7      +4       +0.5     −3      −6      −10.5    −17 (Stern)
Level
L4       ┌───────────────┐               ┌───────────────┐      ┌───────────────┐      ┌─────────────────────────┐
         │ INDUSTRIAL    │               │ TOP FUNNEL    │      │ TOP FUNNEL    │      │ TOP FUNNEL & CATWALK    │
L3       │ CABIN (NAVY)  │               │ CHUTE (CAR 1) │      │ CHUTE (CAR 2) │      │ CHUTE (CAR 3 / TENDER)  │
         ├───────────────┤   [HITCH 1]   ├───────────────┤[HITCH]├───────────────┤[HITCH]├────────────────┬────────┤
L2       │[BUMP][FRONT C]│═══[YAW/PITCH]═│  CAT HOLD 1   │══╦═══│  CAT HOLD 2   │══╦═══│  CAT HOLD 3    │[SORTER]│[DUMP]
         ├───────────────┤               ├───────────────┤  ║   ├───────────────┤  ║   ├────────────────┴────────┤
L1       └──[WH 1]─[WH 2]┘               └──[WH 3]─[WH 4]┘  ║   └──[WH 5]─[WH 6]┘  ║   └──[WH 7]───────[WH 8]────┘
L0    ──────(O)─────(O)─────────────────────(O)─────(O)─────▼──────(O)─────(O)─────▼──────(O)───────────(O)───────
         ◄── 7W×9L×4H B ─►               ◄── 5W×7L×4H B ─►      ◄── 5W×7L×4H B ─►      ◄────── 5W×9L×4H B ──────►
          LOCOMOTIVE (L)                  HOPPER CAR 1 (C1)      HOPPER CAR 2 (C2)       HOPPER CAR 3 / TENDER (C3)
```

---

### 0.4B Consist 3/4 Isometric Perspectives & Stance Wireframes

```
[3/4 ISOMETRIC WIREFRAME · TRACTOR LOCOMOTIVE BOW & CABIN]
                      +Y (Zenith L4)
                            ▲
                            │       ┌───────────────────────┐  ◄── Cab Brow Visor (L4, Z +23)
                            │      ╱                       ╱│
                            │     ┌───────────────────────┐ │
                            │     │   PANORAMIC CABIN     │ │  ◄── Navy Industrial Cab (L3)
                            │     │   [CAT LOCO HELM]     │ │
             -X (Port) ◄────┼────►+X (Starboard)          │ │
                           ╱│     ├───────────────────────┤ │  ◄── Heavy Push Bumper Sill (L2)
                          ╱ │     │[BUMP] [FRONT CONN]    │ │  ◄── Large Receiving Port (Z +25)
                         ╱  │     │                       │╱
                        ▼   │     └──(O)─────────────(O)──┘    ◄── Axles 1 & 2 (5×5 Wheels L1)
                   -Z (Stern)        Axle 1          Axle 2

[3/4 ISOMETRIC WIREFRAME · HOPPER CAR & 3-AXIS DRAWBAR JOINT]
                            ▲
                            │     ┌───[TOP FUNNEL CHUTE]───┐   ◄── 45° Inward Loading Chutes (L4)
                            │    ╱                        ╱│
                            │   ┌────────────────────────┐ │
                            │   │     CAT HOLD 1 (3×3)   │ │   ◄── Large Cargo (L2-L3)
                            │   │  (LOGISTICS EMERALD)   │ │
                            │   ├────────────────────────┤ │
  [LEADING CAR BULKHEAD]    │   │   CONVEYOR SPINE       │ │
            ║               │   └───(O)────────────(O)───┘ │   ◄── Axles 3 & 4 (5×5 Wheels L1)
            ╠══[YAW ROTOR]──┼────────┼──────────────┼──────┘
            ║       │       │        ▼              ▼
            ║  [PITCH HINGE]│      Axle 3         Axle 4
            ║       │       │
            ║  [ROLL ROTOR] ◄── 3-Axis Gimbal Drawbar Joint (Hazard Amber, Level L2)
```

---

### 0.4C Transverse Hull Station Profiles & Chamfer Specifications

To ensure exact block-by-block structural fidelity during survival printing, transverse cross-sections are specified at 5 critical consist stations:

```
Station 1 · Bow Push-Bumper (Z = +25, Looking Stern)
Level
L3  │               ┌─────────┐
    │               │ CAB EYE │   ◄── Industrial Cockpit Forward Viewport (X0)
L2  │   ┌───────────┴─────────┴───────────┐
    │   │[CRIMSON BUMPER] [FRONT C] [BUMP]│   ◄── Large Connector at X0, Blast Door Sills at X ±1 to ±3
L1  │   └─────┬─────────────────────┬─────┘
    │        (O) [5×5 SUSPENSION]  (O)        ◄── Flared Mudguards Over Axle 1 (X = ±3.5)
L0  │    ◄── 1.25 m Belly Skid Clearance ──►

Station 2 · Locomotive Mid-Section Power Core (Z = +21, Looking Stern)
Level
L4  │               ┌─────────┐
    │               │ CAB ROOF│   ◄── Heavy Armor Slopes Angled 45° to Beacon
L3  │   ┌───────────┼─────────┼───────────┐
    │   │ [BATTERY] │ [REACT] │ [BATTERY] │   ◄── 15 MW Small Reactor at X0; 4× Large Batteries at X ±1 to ±2
L2  │   ├───────────┴─────────┴───────────┤
    │   │ ═══════[CONVEYOR SPINE]════════ │   ◄── Large Conveyor Junction at X0, Beltline Crimson at X ±3
L1  │   └─────┬─────────────────────┬─────┘
    │        (O) [FIXED WHEELS]    (O)        ◄── Axle 2 (Fixed Straight-Line Drive)

Station 3 · 3-Axis Drawbar Articulation Joint (Z = +15, Looking Stern)
Level
L3  │          [FLEXIBLE CONVEYOR HOSE]       ◄── Large Curved Conveyor Bridge Loop
L2  │       ┌──[CAT HITCH PITCH HINGE]──┐
    │       │ ◄── YAW ADVANCED ROTOR ──►│     ◄── 2-Axis Gimbal Joint (Amber, X0, Level L2)
L1  │   ════╧═══════════════════════════╧════ ◄── Low-Profile Tension Drawbar Tongue
L0  │    ◄────── 2.5 m Ground Clearance ─────►

Station 4 · Hopper Car Cargo Body (Z = +10.5, Looking Stern)
Level
L4  │   ┌───╲                     ╱───┐       ◄── Heavy Armor Slopes Form 45° Top Funnel Chute
L3  │   │    ┌───────────────────┐    │
    │   │    │  CAT HOLD (3×3)   │    │       ◄── Large Cargo Container (3×3×3 B, Centered X0)
L2  │   │    │ (LOGISTICS GREEN) │    │       ◄── Orange Hull Shell (X = ±2) + Crimson Beltline
L1  │   └──┬─┴───────────────────┴─┬──┘
    │     (O) [DPU TRACTION WHEELS](O)        ◄── 5×5 Suspension Wheels (X = ±2.5)

Station 5 · Stern Unload Bumper & Dump Manifold (Z = −17, Looking Bow)
Level
L3  │   ┌─────────────────────────────┐
    │   │ REAR COLLISION BLUNT BUMPER │       ◄── Heavy Armor Blast Doors & Crimson Slopes
L2  │   │ [GUIDE] [UNLOAD CONN] [GUIDE│       ◄── Large Connector at X0, Funnel Alignment Wedges at X ±2
L1  │   └──────┬───────────────┬──────┘
    │         (O) [CAT SORTER] (O)            ◄── Conveyor Sorter Angled Forward to Unload Connector
L0  │    ◄── 1.25 m Tarmac Ground Line ──►
```

---

### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

The train functions as a single continuous conveyor pipeline from the locomotive front push-bumper through all three hopper holds to the final car discharge connector:

```
                   [MASTODON MKI STERN BUMPER]
                                │
                                ▼
        [CAT Loco Front Conn] (Locomotive Push-Bumper, L2, X0)
                                │
                                ▼ (Locomotive Underbody Spine)
        [CAT Large Conveyor Riser 1] (Level L1 to L2 Junction)
                                │
                                ▼
        [DRAWBAR HITCH 1] (Flexible Large Conveyor Loop across Yaw/Pitch Joint)
                                │
                                ▼
        [CAT Hold 1] (Hopper Car 1 · Large Cargo 3×3×3 B, L2-L3)
          │ ◄── Physical Manual Inspection Hatch (Port L2)
          ▼ (Underbody Inter-Car Riser)
        [DRAWBAR HITCH 2] (Flexible Large Conveyor Loop across Joint 2)
                                │
                                ▼
        [CAT Hold 2] (Hopper Car 2 · Large Cargo 3×3×3 B, L2-L3)
          │ ◄── Physical Manual Inspection Hatch (Port L2)
          ▼ (Underbody Inter-Car Riser)
        [DRAWBAR HITCH 3] (Flexible Large Conveyor Loop across Joint 3)
                                │
                                ▼
        [CAT Hold 3] (Hopper Car 3 / Tender · Large Cargo 3×3×3 B, L2-L3)
          │ ◄── Physical Manual Inspection Hatch (Port L2)
          ▼
        [CAT Train Sorter] (Conveyor Sorter · Drain All = ON · Whitelist Bulk)
                                │
                                ▼
        [CAT Train Unload Conn] (Final Car Rear Bumper Connector, L2, X0)
                                │
                                ▼ (Dumps 2,500 t ore in < 90 s)
                   [GROUND BASE RECEIVING DOCK]
```

### Physical Maintenance Ports & Manual Access Hatches
To comply with survival maintenance standards and allow physical clearing of conveyor blockages:
* **Port 1 (Locomotive Power Core):** Manual small conveyor access hatch at [X −2, Z +21, L2] for loading enriched uranium rods into `CAT Loco Reactor` directly from personal inventory.
* **Ports 2, 3, 4 (Hopper Holds 1, 2, 3):** Standard large-port maintenance hatches exposed along the lower Port sill (X = −2, Level L2) on each hopper car, allowing manual clearing or component retrieval without disassembly.
* **Port 5 (Stern Discharge Manifold):** Inspection junction at [X0, Z −15, L2] upstream of `CAT Train Sorter` to clear unlisted items or oversized salvage debris.

---

### 0.6 Master Electrical Single-Line Schematic & 5-Mode Energy Management Suite

```
[MASTER ELECTRICAL SINGLE-LINE SCHEMATIC]
PRIMARY GENERATION:
  [CAT Loco Reactor] (1× Small Nuclear Reactor, Centered [X0, Z +21, L3]) ──► 15.0 MW Continuous
ENERGY STORAGE BUFFER:
  [CAT Loco Battery 1..4] (4× Large Batteries, L2-L3) ───────────────────────► 12.0 MWh Capacity / 48.0 MW Discharge
TRAIN-WIDE ELECTRICAL BUS (High-Voltage Subgrid Pass-Through):
  ├─────────────────────────────────────────────────────────────────────────► 63.0 MW Peak Instantaneous
  │
  ├─► [CAT Loco Wheels] (4× 5×5 Suspension Drive Motors) ───────────────────► 8.0 MW Max Draw (Tractor)
  ├─► [CAT Car Wheels] (12× 5×5 Suspension DPU Drive Motors) ───────────────► 24.0 MW Max Draw (Hoppers 1-3)
  ├─► [CAT Hitches] (6× Motorized Hinge / Rotor Actuators) ─────────────────► 0.6 MW Active Steering/Damping
  ├─► [CAT Train Sorter] (1× Conveyor Sorter Discharge Pump) ───────────────► 0.5 MW Fast Drain
  └─► [CAT Avionics & Comms] (Antenna, Cockpit, Beacon, Lights) ────────────► 0.2 MW Baseline Load
```

### 5 Formal Operational Energy Management Modes
1. **Mode 1 · Silent Watchkeeping (Parked / Standby):**
   * Output: `0.2 MW`. Reactor idling; batteries set to `Auto`; all wheel suspension propulsion `OFF`; brake locks engaged.
2. **Mode 2 · Empty Shunting & Alignment (Speed < 30 km/h):**
   * Output: `4.5 MW`. Reactor powers locomotive steering and creep propulsion; hopper car DPU motors operate at 25% torque assist; hitch damping set to 20% for tight yard radius flex.
3. **Mode 3 · Laden Highway Cruise (Speed 50-70 km/h, Mass 2,868 t):**
   * Output: `18.5 MW` (15 MW Nuclear + 3.5 MW Battery assist). All 16 wheel drive motors engaged with synchronized torque; hitch damping stiffened to 85% to eliminate harmonic trailer whip.
4. **Mode 4 · Heavy Hill-Climb (20° Incline, Peak Tractive Draw):**
   * Output: `32.0 MW` (15 MW Nuclear + 17 MW Battery surge). All 16 wheel suspension motors commanded to 100% torque; DPU smart traction engages slip control.
5. **Mode 5 · Terminal Fast Discharge (Docked to Ground Base):**
   * Output: `3.5 MW`. Engine throttle cut; parking brakes locked; `CAT Train Sorter` commanded `Drain All = ON`; 2,500 t bulk ore purged across connector in < 90 s. Batteries recharge from Ground Base grid via locked connector.

---

### 0.7 Master Consist Bill of Materials (BOM) & Mass Budget

| Subsystem / Unit | Dimensions (m) | Block Count | Dry Mass (t) | PCU | Power Draw (MW) | Notes & Key Blocks |
|---|---|---|---|---|---|---|
| **Tractor Locomotive (`CAT-01-L`)** | 17.5 × 22.5 × 10.0 | 146 | 148.2 | 1,265 | 15.0 MW gen / 8.0 MW draw | 1× Ind Cockpit, 1× Sm Reactor, 4× Lrg Battery, 4× 5×5 Wheels, Push-Bumper |
| **Drawbar Hitch 1 (Loco to Car 1)**| 2.5 × 5.0 × 2.5 | 6 | 8.2 | 200 | 0.2 MW | 1× Adv Rotor (Yaw), 1× Motor Hinge (Pitch), 1× Adv Rotor (Roll), Clamps |
| **Hopper Car 1 (`CAT-01-C1`)** | 12.5 × 17.5 × 10.0 | 78 | 64.0 | 650 | 8.0 MW draw | 1× Lrg Cargo (3×3×3 B), 4× 5×5 Wheels, 45° Funnel Chutes, Gangways |
| **Drawbar Hitch 2 (Car 1 to Car 2)**| 2.5 × 5.0 × 2.5 | 6 | 8.2 | 200 | 0.2 MW | Identical 3-axis motorized flexible conveyor joint |
| **Hopper Car 2 (`CAT-01-C2`)** | 12.5 × 17.5 × 10.0 | 78 | 64.0 | 650 | 8.0 MW draw | Identical modular hopper car unit |
| **Drawbar Hitch 3 (Car 2 to Car 3)**| 2.5 × 5.0 × 2.5 | 6 | 8.2 | 200 | 0.2 MW | Identical 3-axis motorized flexible conveyor joint |
| **Hopper Car 3 / Tender (`CAT-01-C3`)**| 12.5 × 22.5 × 10.0 | 84 | 67.7 | 785 | 8.5 MW draw | 1× Lrg Cargo, 4× 5×5 Wheels, Sorter, Unload Connector, Crash Bumper |
| **MASTER CONSIST TOTALS** | **17.5 × 105.0 × 10.0** | **404** | **368.5 t** | **3,950** | **15 MW Gen / 63 MW Surge** | **Earth 1 g Laden Mass: 2,868.5 t (2,500 t Ore)** |

* All wheel suspension ratings, braking torques, and structural masses calculated at Earth 1 g baseline.
* Game values tagged with `†` awaiting in-game confirmation in RIG Test:
  * Small Reactor peak continuous output = `15.0 MW`†.
  * Large Battery peak discharge rate = `12.0 MW` per unit (48.0 MW combined)†.
  * 5×5 Suspension Wheel maximum braking torque = `30.0 MN·m`†.

---

### 0.8 Master Player Circulation, Access Corridors & Maintenance Walkways

To ensure 100% physical accessibility for survival engineers in field conditions:
1. **Pressurized Operator Cab (Locomotive Level L3):** Enclosed single-seat cockpit (`CAT Loco Helm`) accessed via Port-side exterior ladder at [X −3, Z +22, L2]. Cabin features survival suit life support, oxygen pressurization, and emergency survival kit.
2. **Continuous Side Running Boards (Levels L1-L2):** Non-skid steel catwalk plates run along both Port (X = −3.5) and Starboard (X = +3.5) sills of the locomotive, allowing walk-around inspection while parked.
3. **Inter-Car Articulated Footplates (Level L2):** Overlapping Heavy Armor Half-Blocks mounted above each drawbar hitch provide a continuous walking bridge between cars across the flexible joint gap.
4. **Hopper Catwalks & Grab Irons (Levels L3-L4):** Vertical ladders mounted on the forward bulkhead of each hopper car climb to elevated perimeter catwalks bordering the top loading funnel chutes, providing full visual inspection of bulk ore intake.

---

### 0.9 CATERPILLAR-02 (MKII) Heavy Armored Rail / Combat Upgrade Roadmap

* **Operational Context:** In advanced survival scenarios or contested hostile sectors subject to Modular Encounter Spawner (MES) pirate raids, the CATERPILLAR-01 consist can be upgraded to the **MKII Armored Combat Rail Consist**:
  1. **Locomotive Armored Prow & CIWS Crown:** Replace bow push-bumper with a faceted heavy armor snowplow ram; mount 1× Large Grid Gatling Turret (`CAT Turret Cab`) atop Level L4 roof crown tied to ammo sorters.
  2. **Hopper Armor Skirts:** Extend heavy armor blast skirts downward along Level L1 to shield all 16 suspension wheel stators from small-arms and explosive shrapnel.
  3. **Tender Rear Defense Emplacement:** Mount 1× Large Grid Autocannon Turret atop the tender aft gantry covering the rear convoy approach sector.
  4. **Conveyor Armor Encapsulation:** Enclose all flexible drawbar jumper loops within overlapping heavy armor collar rings.

---

## Phase 11-1 · Tractor Locomotive Heavy Chassis & 4-Wheel Powertrain (Levels L1-L2)

```
       [PHASE 11-1 SPATIAL LOCATOR HUD]
       Unit: Tractor Locomotive (CAT-01-L)
       Envelope: 7 W × 9 L × 2 H B (17.5 × 22.5 × 5.0 m)
       Coordinate Range: X −3.5 to +3.5, Z +16 to +25, Level L1 to L2
       Key Datums:
       · Front Push-Bumper Datum: Z = +25, Level L1-L2
       · Rear Hitch Attachment Bulkhead: Z = +16, Level L1-L2
       · Front Steering Axle 1: Z = +23, X = ±3.5 (5×5 Wheels)
       · Rear Fixed Drive Axle 2: Z = +19, X = ±3.5 (5×5 Wheels)
       · Chassis Centerline Box Girder: X0, Z +16 to +25
```

### Pre-Step Master Parts Tray: Phase 11-1
* 42× Heavy Armor Blocks (Palette Slot 0: `#2B303A`, 126.0 t, 42 PCU)
* 18× Heavy Armor Half-Slopes (Palette Slot 0, 18.0 t, 18 PCU)
* 8× Heavy Armor Slopes (Palette Slot 0, 16.0 t, 8 PCU)
* 2× Wheel Suspension 5×5 Left (16.0 t, 200 PCU)
* 2× Wheel Suspension 5×5 Right (16.0 t, 200 PCU)
* 9× Large Conveyor Tubes (Palette Slot 4: `#1ABC9C`, 7.2 t, 90 PCU)
* 2× Large Conveyor Junctions (Palette Slot 4, 2.6 t, 26 PCU)

---

### Step 11.1 · Twin Longitudinal Box Girders & Underbody Skid Keel (L1)
Build the primary foundation spine of the locomotive from Z +16 to +25:
1. Lay two parallel Heavy Armor box girders at X = −2 and X = +2 spanning Z +16 to +25 at Level L1.
2. Tie the girders together with transverse cross-beams at Z = +16, Z = +21, and Z = +25.
3. Install continuous Heavy Armor Half-Blocks along centerline X0, forming a flat underside skid plate providing 1.25 m clearance above ground.
4. Bevel the girder outer edges using Heavy Armor Half-Slopes sloping inward toward X0.

```
Plan view, Level L1 (Locomotive Chassis Box Girders)
         X: −2            X0            +2
Z: +25   ■════════════════╤════════════════■   ◄── Bow Bumper Tie Beam
Z: +23   │  [BOX GIRDER]  │  [BOX GIRDER]  │
Z: +21   ■════════════════╪════════════════■   ◄── Midline Tie Beam (Datum Z0)
Z: +19   │  [BOX GIRDER]  │  [BOX GIRDER]  │
Z: +16   ■════════════════╧════════════════■   ◄── Aft Drawbar Tie Beam
```

---

### Step 11.2 · Central Conveyor Raceway & Manifold Junctions (L1-L2)
Install the primary ore transport conduit through the chassis core:
1. Lay 7× Large Conveyor Tubes along centerline X0 from Z +17 to Z +24 at Level L1.
2. Mount 1× Large Conveyor Junction at [X0, Z +25, L1] angled upward to feed the bow push-bumper connector.
3. Mount 1× Large Conveyor Junction at [X0, Z +16, L2] angled aft (−Z) to feed Drawbar Hitch 1.

---

### Step 11.3 · 5×5 Suspension Wheel Mounts & Axles 1 & 2 Geometry (L1)
Install the locomotive tractive gear:
1. Mount 2× Wheel Suspension 5×5 Left units on the Port chassis sill at [X −3, Z +23, L1] (`WH 1: Steer P`) and [X −3, Z +19, L1] (`WH 3: Drive P`), facing outward (−X).
2. Mount 2× Wheel Suspension 5×5 Right units on the Starboard sill at [X +3, Z +23, L1] (`WH 2: Steer S`) and [X +3, Z +19, L1] (`WH 4: Drive S`), facing outward (+X).
3. Ensure suspension stators orient with steering pivot pins strictly vertical (Y-axis).

#### Terminal Configuration & Tuning Sliders (`CAT Loco Wheels`):
* **Axle 1 (`WH 1, 2` · Front Steering):** Steering = `ON` (Angle = `32.0°`), Propulsion = `ON` (Torque = `100%`), Friction = `70%`, Strength = `65%`, Height Offset = `−10 cm` (lowers CoM).
* **Axle 2 (`WH 3, 4` · Rear Drive):** Steering = `OFF` (Locked 0.0°), Propulsion = `ON` (Torque = `100%`), Friction = `75%`, Strength = `70%`, Height Offset = `−10 cm`.
* **Speed Limit:** Set to `70.0 km/h` project-wide.

---

### Step 11.4 · Flared Wheel Arch Mudguards & Side Running Boards (L2)
1. Mount Heavy Armor Half-Blocks directly above each 5×5 wheel hub at X = ±3 to ±4, Level L2.
2. Cap the outer mudguard sills with Heavy Armor Half-Slopes angled 45° downward, protecting suspension stators from flying ballast stone.
3. Lay catwalk plates along X = ±3.5 between the wheel arches to create continuous side running boards for engineer access.

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 11.1A: CHASSIS & POWERTRAIN INTEGRITY**
> Before proceeding to cab deck framing:
> 1. Verify uninterrupted large-conveyor flow along centerline X0 from Z +25 to Z +16.
> 2. Confirm Axle 1 steers freely through ±32° without clipping adjacent chassis armor blocks.
> 3. Verify all 4 suspension wheel stators belong to group `CAT Loco Wheels` and respond to brake input.

---

#### Visual Milestone 11-1 · Locomotive Chassis Complete

```
Visual Checkpoint Silhouette (Looking Port at Locomotive Chassis L1-L2):
            [BOW BUMPER MOUNT Z +25]             [DRAWBAR BULKHEAD Z +16]
                       \                                   /
         ═══════════════╧═════════════════════════════════╧═══════════════
               (O) [AXLE 1: STEER]               (O) [AXLE 2: DRIVE]
```

* [ ] Subgrid dry mass verified: `≈ 148.2 t`.
* [ ] 1.25 m belly skid clearance maintained under full suspension compression.

---

## Phase 11-2 · Locomotive Power Core, Armored Cab & Forward Push-Bumper (Levels L2-L4)

```
       [PHASE 11-2 SPATIAL LOCATOR HUD]
       Unit: Locomotive Upper Hull & Cab (CAT-01-L)
       Envelope: 7 W × 9 L × 3 H B (17.5 × 22.5 × 7.5 m)
       Coordinate Range: X −3.5 to +3.5, Z +16 to +25, Level L2 to L4
       Key Datums:
       · Armored Cab Center: [X0, Z +22, Level L3]
       · Nuclear Reactor Vault: [X0, Z +21, Level L3]
       · Battery Vault: [X ±2, Z +21, Level L2-L3]
       · Forward Push-Bumper: [X0, Z +25, Level L2]
```

### Pre-Step Master Parts Tray: Phase 11-2
* 1× Industrial Cockpit (Palette Slot 7: `#1B263B`, 3.5 t, 15 PCU)
* 1× Small Nuclear Reactor (Palette Slot 6: `#C0392B`, 1.6 t, 25 PCU)
* 4× Large Batteries (Palette Slot 6, 20.0 t, 80 PCU)
* 1× Large Connector (Palette Slot 1: `#9E2A2B`, 3.1 t, 150 PCU)
* 1× Radio Antenna (Large Grid) (Palette Slot 7, 8.0 t, 100 PCU)
* 2× Heavy Armor Blast Door Blocks (Palette Slot 1, 4.8 t, 2 PCU)
* 28× Heavy Armor Slopes 2×1×1 (Palette Slot 7, 21.0 t, 28 PCU)
* 1× Programmable Block (Palette Slot 7, 0.5 t, 25 PCU)

---

### Step 11.5 · Nuclear Power Core & 4× Large Battery Vault (L2-L3)
1. Mount 1× Small Nuclear Reactor `CAT Loco Reactor` centered at [X0, Z +21, Level L3] directly over the conveyor spine junction.
2. Install 2× Large Batteries `CAT Loco Battery 1, 2` on the Port flank at [X −2, Z +21, Level L2-L3].
3. Install 2× Large Batteries `CAT Loco Battery 3, 4` on the Starboard flank at [X +2, Z +21, Level L2-L3].
4. Enclose the power vault with Heavy Armor Blocks painted Palette Slot 6 (High-Voltage Red).

---

### Step 11.6 · Single-Operator Armored Cab & Programmable Block Rack (L3-L4)
1. Mount 1× Industrial Cockpit `CAT Loco Helm` at [X0, Z +23, Level L3] facing forward (+Z).
2. Install Programmable Block `[CAT] PB Train Master` in the sub-cab avionics bay at [X0, Z +22, Level L2].
3. Frame the cab with Heavy Armor Slopes 2×1×1 sloping inward from the L2 beltline to the L4 roofline, forming chiseled lateral tumblehome.
4. Mount an overhanging cab brow visor at Level L4 (Z +24) to eliminate planetary solar glare and protect viewport glass from falling stone spall.

---

### Step 11.7 · Heavy Bow Push-Bumper & Soft-Capture Receiving Port (L1-L2)
1. Across the forward face (Z = +25, Level L1-L2), weld Heavy Armor Blast Door Blocks painted Palette Slot 1 (Heavy Rail Crimson).
2. Mount 1× Large Connector `CAT Loco Front Conn` at [X0, Z +25, Level L2] facing forward (+Z), plumbed to the central conveyor junction. Height: exactly 2.5 m above ground.
3. Flank the connector with Heavy Armor Corner 2×1×1 slopes at X = ±2, forming rigid guide horns that funnel the incoming Mastodon stern dock into lock alignment.

#### Terminal Configuration & Sliders:
* Connector: `CAT Loco Front Conn` (Auto-Connect: `OFF` · Collect All: `OFF` · Connect Strength: `0.015`).

---

### Step 11.8 · Comms Mast, Roof Warning Beacon & Emergency Decouple Merge (L3-L4)
1. Mount 1× Large Grid Radio Antenna `CAT Loco Antenna` at [X0, Z +18, Level L4]. Set broadcast range to 25,000 m.
2. Mount 1× Rotating Warning Beacon `CAT Loco Beacon` at [X0, Z +20, Level L4]. Set color to Amber (`#E6A100`) and spin rate to 30 rpm.
3. Install 1× Merge Block `CAT Hitch Decouple 1` at [X0, Z +16, Level L2] facing aft (−Z) to provide pyrotechnic consist emergency severance if trailers become pinned.

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 11.2A: CAB & POWER ACCEPTANCE**
> 1. Verify reactor draws uranium automatically through the central conveyor riser.
> 2. Ensure cockpit displays populate battery charge status and power reserves (63 MW combined surge capacity).
> 3. Verify driver controls: Hotbar 1 wheel brake, connector lock/unlock, and emergency decouple toggle.

---

## Phase 11-3 · 3-Axis Articulated Conveyor Drawbar Hitches (Levels L1-L3)

```
       [PHASE 11-3 SPATIAL LOCATOR HUD]
       Unit: 3-Axis Conveyor Drawbar Hitch (CAT-HITCH 1..3)
       Envelope: 1 W × 2 L × 2 H B (2.5 × 5.0 × 5.0 m per joint)
       Coordinate Range: X0, Z spans 2 B between vehicle bulkheads, Level L1 to L3
       Kinematic Axes:
       · Yaw Advanced Rotor: Vertical Axis (±45.0° horizontal articulation)
       · Pitch Motor Hinge: Transverse Axis (±25.0° vertical terrain flex)
       · Roll Advanced Rotor: Longitudinal Axis (±20.0° cross-slope flex)
       · Large Conveyor Pass-Through: Continuous airtight flexible jumper loop
```

### Pre-Step Master Parts Tray: Phase 11-3 (Per Drawbar Joint ×3)
* 1× Advanced Rotor (Yaw) (Palette Slot 2: `#E6A100`, 8.2 t, 100 PCU)
* 1× Motor Hinge (Pitch) (Palette Slot 2, 8.0 t, 100 PCU)
* 1× Advanced Rotor (Roll) (Palette Slot 2, 8.2 t, 100 PCU)
* 2× Large Magnetic Plates (Palette Slot 2, 4.0 t, 50 PCU)
* 4× Large Curved Conveyor Tubes (Palette Slot 4: `#1ABC9C`, 3.2 t, 40 PCU)
* 2× Heavy Armor Half-Blocks (Footplates) (Palette Slot 9: `#F1C40F`, 4.0 t, 2 PCU)

---

### Step 11.9 · Kinematic 3-Axis Gimbal Drawbar Assembly (L2)
Assemble the 3-axis articulated mechanical link between leading and trailing units:
1. Mount 1× Advanced Rotor `CAT Hitch Yaw 1` on the leading car rear bulkhead at [X0, Z +16, Level L2] with rotor stator vertical (Y-axis), providing horizontal steering yaw from −45.0° to +45.0°.
2. Weld 1× Motor Hinge `CAT Hitch Pitch 1` onto the yaw rotor head with hinge pin horizontal (X-axis), providing vertical terrain cresting/trough pitch from −25.0° to +25.0°.
3. Mount 1× Advanced Rotor `CAT Hitch Roll 1` onto the hinge head with rotor stator aligned longitudinally (Z-axis), providing axial roll compliance from −20.0° to +20.0° across uneven ground.
4. Plumb large curved conveyor tubes across the joint to maintain continuous airtight conveyance.
5. Install twin magnetic stabilizer clamps `CAT Hitch Clamp Port/Stbd` at X = ±1.5 to eliminate high-speed highway oscillation.

#### Joint Parameters & Slider Settings (MANDATORY):
* **Share Inertia Tensor = `ON`** on ALL rotors and hinges across all 3 drawbars (eliminates subgrid sway and phantom physics Klang).
* **`CAT Hitch Yaw`:** Limits = `−45.0°` to `+45.0°` · Braking Torque = `30.0 MN·m`†.
* **`CAT Hitch Pitch`:** Limits = `−25.0°` to `+25.0°` · Braking Torque = `30.0 MN·m`†.
* **`CAT Hitch Roll`:** Limits = `−20.0°` to `+20.0°` · Braking Torque = `15.0 MN·m`†.

```
3-Axis Gimbal Drawbar Articulation Diagram (Looking Port at Joint, Level L2)
  Leading Car Aft Bulkhead
            ║
            ╠══[CAT HITCH YAW] (Advanced Rotor · Vertical Axis ±45°)
            ║        │
            ║        ▼
            ║   [CAT HITCH PITCH] (Motor Hinge · Transverse Axis ±25°)
            ║        │
            ║        ▼
            ║   [CAT HITCH ROLL] (Advanced Rotor · Longitudinal Axis ±20°)
            ║        │
            ║        ▼ (Flexible Large Curved Conveyor Pipe Pass-Through)
            ╚════════╪══════════════════════════════════════════════════════
                     ▼
         Trailing Car Forward Bulkhead
```

> [!NOTE]
> Repeat Step 11.9 for Drawbar Hitch 2 (Z +5 to +7) and Drawbar Hitch 3 (Z −4 to −2).

---

## Phase 11-4 · Modular Hopper Cars 1 & 2 Construction (Levels L1-L4)

```
       [PHASE 11-4 SPATIAL LOCATOR HUD]
       Unit: Modular Hopper Cars 1 & 2 (CAT-01-C1 & CAT-01-C2)
       Envelope: 5 W × 7 L × 4 H B (12.5 × 17.5 × 10.0 m per car)
       Coordinate Range:
       · Car 1: X −2.5 to +2.5, Z +7 to +14, Level L1 to L4
       · Car 2: X −2.5 to +2.5, Z −2 to +5, Level L1 to L4
       Hold Centroid:
       · Car 1 Hold: [X0, Z +10.5, Level L2-L3] (Large Cargo 3×3×3 B)
       · Car 2 Hold: [X0, Z +1.5, Level L2-L3] (Large Cargo 3×3×3 B)
```

### Pre-Step Master Parts Tray: Phase 11-4 (Per Hopper Car)
* 1× Large Cargo Container (Palette Slot 5: `#27AE60`, 28.5 t, 250 PCU)
* 4× Wheel Suspension 5×5 (2 Left, 2 Right) (Palette Slot 0, 32.0 t, 400 PCU)
* 2× Large Conveyor Junctions (Palette Slot 4, 2.6 t, 26 PCU)
* 24× Heavy Armor Slopes 2×1×1 (Palette Slot 3: `#D35400`, 18.0 t, 24 PCU)
* 32× Heavy Armor Blocks (Palette Slot 0, 96.0 t, 32 PCU)
* 4× Steel Catwalk Plates (Palette Slot 9, 1.6 t, 4 PCU)

---

### Step 11.10 · Hopper Car 1 Chassis & DPU Traction Wheels (L1)
1. Construct the 5 W × 7 L heavy armor chassis spanning Z +7 to +14 at Level L1.
2. Mount 2× Wheel Suspension 5×5 Left units at [X −2.5, Z +13, L1] (`WH 5: DPU P`) and [X −2.5, Z +8, L1] (`WH 7: DPU P`).
3. Mount 2× Wheel Suspension 5×5 Right units at [X +2.5, Z +13, L1] (`WH 6: DPU S`) and [X +2.5, Z +8, L1] (`WH 8: DPU S`).
4. Lock steering at `0.0°` (fixed straight-line tracking). Set propulsion torque to `100%` and assign to group `CAT Car Wheels`.

---

### Step 11.11 · Hopper Car 1 Large Cargo Hold & Top Funnel Chutes (L2-L4)
1. Mount 1× Large Cargo Container `CAT Hold 1` (3×3×3 B) centered at [X0, Z +10.5, Level L2-L3]. Capacity: 1,386,000 L.
2. Tie the forward bottom port to the inbound conveyor riser at [X0, Z +14, L2]; tie the aft bottom port to the outbound riser at [X0, Z +7, L2].
3. Frame the container's upward-facing large port with Heavy Armor Slopes 2×1×1 angled inward at 45° painted Palette Slot 3 (Orange), forming a wide top-loading funnel chute for filling from quarry hoppers.
4. Install side maintenance access ports on the Port sill at [X −2.5, Z +10.5, L2].

> [!NOTE]
> Repeat Steps 11.10 and 11.11 for Hopper Car 2 spanning Z −2 to +5 (`CAT Hold 2`, Axles 5 & 6, Wheels `WH 9..12`).

---

## Phase 11-5 · Hopper Car 3 / Tender & Stern High-Speed Terminal Discharge Bumper (Levels L1-L4)

```
       [PHASE 11-5 SPATIAL LOCATOR HUD]
       Unit: Hopper Car 3 / Tender (CAT-01-C3)
       Envelope: 5 W × 9 L × 4 H B (12.5 × 22.5 × 10.0 m)
       Coordinate Range: X −2.5 to +2.5, Z −17 to −8, Level L1 to L4
       Key Datums:
       · Car 3 Hold Centroid: [X0, Z −11.5, Level L2-L3]
       · Sorter Discharge Pump: [X0, Z −15, Level L2]
       · Stern Unload Connector: [X0, Z −17, Level L2] (Height: 2.5 m)
       · Collision Push-Bumper: Z = −17, Level L1-L2
```

### Pre-Step Master Parts Tray: Phase 11-5
* 1× Large Cargo Container (Palette Slot 5: `#27AE60`, 28.5 t, 250 PCU)
* 4× Wheel Suspension 5×5 (2 Left, 2 Right) (Palette Slot 0, 32.0 t, 400 PCU)
* 1× Large Conveyor Sorter (Palette Slot 4: `#1ABC9C`, 1.2 t, 40 PCU)
* 1× Large Connector (Palette Slot 1: `#9E2A2B`, 3.1 t, 150 PCU)
* 2× Heavy Armor Blast Door Blocks (Palette Slot 1, 4.8 t, 2 PCU)
* 1× Event Controller `CAT EC Dump` (Palette Slot 7, 0.2 t, 25 PCU)

---

### Step 11.12 · Tender Hold 3 & DPU Axles 7 & 8 (L1-L3)
1. Build the extended 5 W × 9 L chassis spanning Z −17 to −8 at Level L1.
2. Install Axles 7 and 8 with 4× 5×5 suspension wheels (`WH 13..16`) at Z = −9 and Z = −13. Configure for 100% propulsion assist.
3. Mount Large Cargo Container `CAT Hold 3` centered at [X0, Z −11.5, Level L2-L3].
4. Install top funnel chutes and catwalk framing matching Cars 1 and 2.

---

### Step 11.13 · High-Speed Pressurized Discharge Sorter & Stern Connector (L2)
Install the automated terminal dump system:
1. Mount 1× Large Conveyor Sorter `CAT Train Sorter` horizontally at [X0, Z −15, Level L2] with flow arrow pointing stern (−Z).
2. Configure sorter whitelist with bulk minerals: `Iron`, `Nickel`, `Silicon`, `Cobalt`, `Magnesium`, and `Stone`. Set `Drain All = OFF` by default (triggered by docking EC).
3. Mount 1× Large Connector `CAT Train Unload Conn` at [X0, Z −17, Level L2] facing stern (−Z), plumbed directly to the sorter outlet. Centerline height: exactly 2.5 m above tarmac.
4. Mount Event Controller `CAT EC Dump` adjacent to the sorter at [X −1, Z −15, L2].

---

### Step 11.14 · Heavy Collision Bumper & Ground Base Alignment Horns (L1-L2)
1. Across the rear transom (Z = −17, Level L1-L2), weld Heavy Armor Blast Door Blocks painted Palette Slot 1 (Crimson).
2. Install Heavy Armor Corner 2×1×1 slopes at X = ±2, forming rigid guide wedges angled aft (−Z) to absorb docking kinetic shock and funnel the connector into the Ground Base receiving dock.
3. Install yellow warning chevrons (Palette Slot 9) across the bumper face.

---

## Phase 11-6 · Consist Catwalks, Lighting Matrix & DPU Synchronized Traction (Levels L1-L4)

### Step 11.15 · Continuous Consist Maintenance Catwalk Network (L4)
1. Connect all hopper top loading platforms with continuous steel catwalks spanning the inter-car drawbar gaps using articulated hinged footplates.
2. Install safety handrails and yellow grab irons along all ladderwells.
3. Mount 4× Worksite LED Spotlights `CAT Floodlight 1..4` along the hopper sills angled downward 45° to illuminate nighttime quarry transfer operations.

---

## Phase 11-7 · Master Control Logic, MDK2 Zero-GC Architecture & Terminal Dictionary

### 1. Dual Compute Architecture
* **Locomotive Master PB (`[CAT] PB Train Master`):**
  * Target Block: Located in Locomotive Sub-Cab at [X0, Z +22, Level L2].
  * Grid: Large Grid (42 B Consist · 16 Wheels) · Update: `Update1` (Traction/Braking) + `Update100` (Telemetry).
  * Managed Groups: `CAT Train Wheels`, `CAT Hitches`, `CAT Holds`, `CAT Loco Power`.
  * Roles: Distributed traction coordination, speed-dependent anti-jackknife drawbar stiffening, and automatic terminal dump sequencing.

### 2. Group Taxonomy
* `CAT Train Wheels` : All 16 wheel suspension units across locomotive and 3 hopper cars.
* `CAT Train Brakes` : Synchronized parking and emergency brake group.
* `CAT Hitches` : All 6 motorized hitch rotors and hinges linking the 4 consist units.
* `CAT Holds` : All 3 Large Cargo Containers (`CAT Hold 1`, `CAT Hold 2`, `CAT Hold 3`).
* `CAT Loco Power` : 1× Small Nuclear Reactor + 4× Large Batteries.
* `CAT Lights` : Bow headlights, cabin interior lights, running board markers, and floodlights.

### 3. Block Naming & Terminal Dictionary
* `CAT Loco Helm` : Industrial Cockpit in locomotive cab [X0, Z +23, L3].
* `CAT Loco Front Conn` : Large Connector on bow push-bumper [X0, Z +25, L2].
* `CAT Loco Reactor` : 15 MW Small Nuclear Reactor [X0, Z +21, L3].
* `CAT Loco Battery 1..4` : 4× Large Batteries in locomotive vault [X ±2, Z +21, L2-L3].
* `CAT Loco Antenna` : 25 km Radio Antenna mast [X0, Z +18, L4].
* `CAT Loco Beacon` : Amber Rotating Warning Beacon [X0, Z +20, L4].
* `CAT Hitch Yaw 1..3` : Advanced Rotors on leading car rear bulkheads (Vertical Axis).
* `CAT Hitch Pitch 1..3` : Motor Hinges on drawbar links (Horizontal Transverse Axis).
* `CAT Hitch Roll 1..3` : Advanced Rotors on trailing car bulkheads (Longitudinal Axis).
* `CAT Hitch Clamp Port/Stbd 1..3` : Magnetic anti-sway stabilizer clamps on drawbars.
* `CAT Hold 1, 2, 3` : Large Cargo Containers in Hoppers 1, 2, and 3 [Level L2-L3].
* `CAT Train Sorter` : Large Conveyor Sorter on tender rear manifold [X0, Z −15, L2].
* `CAT Train Unload Conn` : Large Connector on stern bumper [X0, Z −17, L2].
* `CAT EC Dump` : Event Controller automating terminal fast-dump sequence.
* `CAT EC AntiJackknife` : Event Controller automating speed-based drawbar damping.
* `CAT EC Traction` : Event Controller modulating DPU wheel torque based on payload mass.

---

### 4. Event Controller Matrix (Rule C4 Compliance)

| Event Controller | Monitored Block / Variable | Condition & Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| **`CAT EC Dump`** | `CAT Train Unload Conn` | Connector: `Connected` | (1) `CAT Train Sorter` Drain All = `ON`<br/>(2) `CAT Train Wheels` Handbrake = `ON`<br/>(3) Sound `CAT Alarm Dump` | (1) `CAT Train Sorter` Drain All = `OFF`<br/>(2) `CAT Train Wheels` Handbrake = `OFF` |
| **`CAT EC AntiJackknife`** | Vehicle Velocity | Speed $\ge 40.0\text{ km/h}$ | (1) Group `CAT Hitches` Damping $\rightarrow 85\%$<br/>(2) Group `CAT Hitch Clamps` Auto-Lock = `ON` | (1) Group `CAT Hitches` Damping $\rightarrow 20\%$<br/>(2) Group `CAT Hitch Clamps` Unlock |
| **`CAT EC Traction`** | Consist Gross Mass | Gross Mass $\ge 1,500\text{ t}$ | (1) Group `CAT Car Wheels` Torque $\rightarrow 100\%$<br/>(2) Power Mode $\rightarrow$ `MODE 4: HILL_CLIMB` | (1) Group `CAT Car Wheels` Torque $\rightarrow 50\%$<br/>(2) Power Mode $\rightarrow$ `MODE 3: CRUISE` |
| **`CAT EC Decouple`** | `CAT Hitch Decouple 1` | Merge Block: `Disconnected` | (1) Trailing Axles Emergency Brake = `ON`<br/>(2) Broadcast `TMS.CONSIST.SEVER` | None |

---

### 5. IGC Channel Subscriptions (Ground Fleet Bus)

```
[IGC GROUND BUS TOPOLOGY · SEMICOLON DELIMITED]
  ┌─────────────────────────────────────────────────────────┐
  │                 GROUND BASE C2 (GND-01)                 │
  └────────────────────────────┬────────────────────────────┘
                               │
            ┌──────────────────┴──────────────────┐
            ▼ (TMS.BASE.RECEIVING)                ▲ (TMS.CAT.STATUS)
  ┌─────────────────────────────────┐   ┌─────────────────────────────────┐
  │ CATERPILLAR-01 (CAT-01-L)       │   │ MASTODON-01 MKI (MAS-01)        │
  │ Status: SPEED;MASS;HITCH;STATE  │   │ Sync: PISTON_DOCK;TRANSFER_RATE │
  └────────────────┬────────────────┘   └────────────────┬────────────────┘
                   │                                     │
                   └──────────────◄──────────────────────┘
                           TMS.CONVOY.SYNC / TMS.MAST.TRANSFER
```

* **Outbound Channels:**
  * `TMS.CAT.STATUS`: `train_id;gross_mass_t;payload_ore_t;speed_kmh;hitch_status;route_status`
    * Example: `CAT-01;2840.5;2472.0;58.2;LOCKED_85PCT;EN_ROUTE_BASE`
  * `TMS.CONSIST.SEVER`: `SEVER;CAT-01;DRAWBAR_1;EMERGENCY_DECOUPLE`
* **Inbound Channels:**
  * `TMS.BASE.RECEIVING`: `GND-01;CLEAR_TO_DUMP;TERMINAL_DOCK_1;READY`
  * `TMS.MAST.TRANSFER`: `MAS-01;SOFT_CAPTURE_LOCK;TRANSFER_ACTIVE;2500_T`
  * `TMS.CONVOY.SYNC`: `CONVOY;MAS-01;45.0;180.0;ROADBED_SPREADER_ON`
  * `TMS.LOGISTICS.JIT`: `JIT;MAS-01;GND;180;IRON;2500000`

---

### 6. Driver Cockpit Hotbar Layouts (`CAT Loco Helm`)

#### Hotbar 1 · Driving, Brakes & Powertrain
* [1] `CAT Train Wheels` Handbrake Toggle
* [2] `CAT Train Wheels` Propulsion Increase
* [3] `CAT Train Wheels` Propulsion Decrease
* [4] `CAT Loco Front Conn` Switch Lock (Mastodon Push-Dock)
* [5] `CAT Train Unload Conn` Switch Lock (Ground Base Dump Dock)
* [6] `CAT Hitches` Damping Increase (Manual Stiffen)
* [7] `CAT Hitches` Damping Decrease (Manual Flex)
* [8] `CAT Loco Power` Battery Recharge Toggle
* [9] `CAT Lights` Toggle On/Off

#### Hotbar 2 · Drawbar Hitches, DPU Traction & Diagnostics
* [1] `CAT Hitch Clamps` Auto-Lock Toggle
* [2] `CAT Hitch Decouple 1` Decouple (Emergency Sever)
* [3] `CAT Car Wheels` Propulsion Override Increase
* [4] `CAT Car Wheels` Propulsion Override Decrease
* [5] `CAT Train Sorter` Drain All Toggle
* [6] `[CAT] PB Train Master` Run: `MODE_CRUISE`
* [7] `[CAT] PB Train Master` Run: `MODE_HILL_CLIMB`
* [8] `[CAT] PB Train Master` Run: `RELOAD`
* [9] `CAT Loco Beacon` Toggle On/Off

#### Hotbar 3 · Fleet Comms & Logistics Dispatch
* [1] `[CAT] PB Train Master` Run: `IGC_SEND;TMS.BASE.RECEIVING;REQUEST_DOCK`
* [2] `[CAT] PB Train Master` Run: `IGC_SEND;TMS.MAST.TRANSFER;READY_TO_RECEIVE`
* [3] `CAT Floodlights` Toggle On/Off
* [4] `CAT Alarm Dump` Toggle On/Off
* [5] `CAT Loco Antenna` Range Increase
* [6] `CAT Loco Antenna` Range Decrease
* [7] `CAT Loco Reactor` Toggle On/Off
* [8] Group `CAT Holds` Show on HUD Toggle
* [9] `CAT Loco Helm` Dampeners Toggle

---

## Phase 11-8 · Commissioning & Field Acceptance Checklist (RIG Test)

### Suite A · Consist Powertrain & Steering Geometry Trials
* [ ] **Turn Radius Verification:** Drive the 42 B consist through a 90° intersection at 25 km/h; verify Axle 1 steers smoothly through ±32° and all 3 trailing cars track within an 18 m swept path without clipping curbs.
* [ ] **Independent DPU Traction Verification:** With locomotive reactor powered OFF, engage battery power; confirm all 12 hopper suspension wheels provide active tractive effort and propel the empty consist to 40 km/h.

### Suite B · 3-Axis Drawbar Articulation & Stability
* [ ] **3-Axis Terrain Compliance:** Navigate the consist over an uneven rocky ridge; confirm Drawbar Hitches 1-3 flex independently through pitch (±25°), yaw (±45°), and roll (±20°) without chassis block collision or subgrid shearing.
* [ ] **Share Inertia Tensor Audit:** Inspect all 6 drawbar rotors/hinges in terminal; confirm `Share Inertia Tensor = ON` on all joints; verify ZERO phantom physics oscillation while stationary on 15° slopes.

### Suite C · Full Laden Hill-Climb & Regenerative Braking
* [ ] **2,500 t Full Payload Hill-Climb:** Fill all 3 hopper containers with stone (2,868 t gross consist mass); ascend a 20° voxel incline from a dead stop; verify consist accelerates smoothly to 45 km/h without wheel spin or reactor overload.
* [ ] **Controlled Descent Braking:** Descend a 20° slope at 50 km/h; apply full service brakes; confirm all 16 wheels brake synchronously, halting the consist in $< 65\text{ m}$ with zero jackknifing.

### Suite D · High-Speed 70 km/h Highway Run & Anti-Jackknife
* [ ] **70 km/h Highway Test:** Cruise on flat terrain at 70 km/h; confirm `CAT EC AntiJackknife` triggers at 40 km/h, stiffening hitch damping to 85% and engaging magnetic stabilizer clamps; confirm zero trailer whip or harmonic sway.

### Suite E · Mastodon Soft-Capture Field Docking (< 90 s)
* [ ] **Push-Dock Mating:** Back locomotive bow bumper into Mastodon MKI stern alignment guide horns; confirm `CAT Loco Front Conn` locks smoothly at 2.5 m elevation.
* [ ] **2,500 t Bulk Ore Fill:** Engage transfer sorters from Mastodon hold; verify all 3 hopper cars fill evenly to 100% capacity in $< 90\text{ seconds}$ without conveyor bottleneck.

### Suite F · Ground Base Terminal Discharge & In-Deck Collector Flow
* [ ] **Reverse Terminal Docking:** Reverse consist into Ground Base Bulk Receiving Dock; verify stern alignment wedges funnel `CAT Train Unload Conn` into lock with the base terminal connector.
* [ ] **High-Speed Gravity/Pump Dump:** Verify `CAT EC Dump` automatically engages `CAT Train Sorter` (Drain All = ON); confirm 2,500 t of ore purges into the 5× in-deck collectors in $< 90\text{ seconds}$.

### Suite G · Player Circulation & Survival Ergonomics
* [ ] **Continuous Walk-Around:** Walk from the locomotive cab running boards across all 3 drawbar footplates to the tender rear bumper; confirm zero lethal fall gaps.
* [ ] **Manual Port Inspection:** Access all 3 hopper maintenance ports from ground level; verify ability to manually transfer items into cargo holds.

### Suite H · Emergency Consist Decoupling & Deadman Braking
* [ ] **Emergency Severance Test:** While cruising at 40 km/h, trigger `CAT Hitch Decouple 1`; confirm locomotive breaks free cleanly while trailing hopper cars immediately engage emergency friction brakes and come to a safe stop.

---

# Volume III · Floating Rig Structure (25 kt Fortress)

This volume details the physical construction of the Floating Rig: a 25,000 t heavy armored aerial fortress stationed at Earth 1 g (Decision 48).

```
Master Elevation Profile · Floating Rig 25 kt Heavy Fortress (Looking North):
                       [H1 Command Bridge & Roof Pickets]           (S9 · L21-L24)
                       [H2 Crew Citadel & Life Support]             (S8 · L17-L20)
                       [H3 Gas Farm & Core Machine Shop]            (S7 · L13-L16)
  ◄─── Pylon S1 ───►  ╔═══════════════════════════════════════════╗  ◄─── Pylon S2 ───►
  (8 W × 7 H × 22 L)  ║ H4 Nuclear Power Vault (3× Large React)   ║  (8 W × 7 H × 22 L)
  [7 Atmo + 4 H2]     ║ H5 Pylon Sockets, Gyros & Trunk Ring      ║  [7 Atmo + 4 H2]
  [3 Modular Bays]    ║ H6 Central Lift Ring (10 Atmo + 6 H2)     ║  [3 Modular Bays]
                      ╚═══════════════════════════════════════════╝
                                   ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
                            [Central Downward Lift Plume]
```

---

## Chapter 6 · Section S0: HUB Base (H4-H6)

The HUB base is the primary core module of the Floating Rig: the first structural volume printed by the Extruder Ring shipyard and the first section to fly. It carries the primary 900 MW nuclear power plant, the central lift ring, and the docking sockets for all 6 pylon arms.

### 6.0 · Reference Envelope & Master Geometry
* **Footprint:** Hexagonal prism, 18 B diameter across flats (45.0 m) × 12 B height (30.0 m, Levels L1 to L12).
* **Mass (dry base):** ≈ 7,200 t (including double-walled heavy armor citadel and reactor shielding).
* **Grid:** Large Grid (1 B = 2.5 m).
* **Seed Kit Avionics:** H6 lift thrusters, 6 Large Batteries, Remote Control on H5, 12 Attitude Gyroscopes, Fleet Radio Antenna.
* **Operating Environment:** Earth 1 g baseline, surface hover up to 18 km ceiling†.

```
Coordinate Triad & Axis Conventions:
       [+Y: Zenith / Up]
              │
              │   [+Z: North / Forward]
              │   /
              │  /
              │ /
  [−X: West] ─┼───────── [+X: East]
             /│
            / │
           /  │
  [−Z: South / Aft]
Origin (X0, Y0, Z0): Centerline of the Rig HUB Base, Level L1 at the lowest thruster nozzle plane (Y = 0).
```

#### 6.0.2 · In-Game Paint Palette & Functional Livery Map
| Slot | Color Name | Hex Code | RGB | In-Game Material | Structural Application |
|---|---|---|---|---|---|
| Color 0 | Citadel Charcoal | `#1A1D20` | 26, 29, 32 | Heavy Armor Plate | External citadel shell, reactor containment, pylon keels |
| Color 1 | Structural Slate | `#42464D` | 66, 70, 77 | Carbon / Clean Armor | Hangar bay walls, deck bulkheads, structural ribs |
| Color 2 | Warning Orange   | `#E06D14` | 224, 109, 20| Battered Armor | Hangar door perimeters, socket alignment guides |
| Color 3 | Tactical White   | `#D4D8DD` | 212, 216, 221| Clean Armor | Flight deck markings, drone alignment chevrons |
| Color 4 | Hazard Striping  | `#C8A114` | 200, 161, 20| Hazard Stripes | Thruster exclusion perimeters, lift ring coamings |
| Color 5 | Habitat White    | `#E8ECF0` | 232, 236, 240| Clean Interior | H1 Command Bridge, H2 Crew Citadel interior |
| Color 6 | Machine Olive    | `#4A5643` | 74, 86, 67 | Worn Metal | Refineries, assemblers, conveyor sorters |
| Color 7 | Cryo Cyan        | `#1E6B7B` | 30, 107, 123| Clean Armor | Hydrogen tanks, O2/H2 generators |
| Color 8 | Mechanical Gunmetal| `#2B2E33`| 43, 46, 51 | Corrugated Metal | Pylon socket merge heads, hinge pivots |
| Color 9 | Grate Steel      | `#50555C` | 80, 85, 92 | Grated Catwalk | Walkways, maintenance stairs, Jefferies tubes |

```
Exterior Livery Profile · HUB Base (H4-H6):
   L12 ┌────────────────────────────────────────────────────────┐ [Citadel Charcoal]
   L10 │   H4 NUCLEAR CITADEL (Double-Walled Heavy Armor)       │ [Slot 0]
   L8  ├────────────────────────────────────────────────────────┤
   L6  │ ◄[SOCKET P-W]   H5 PYLON SOCKET & GYRO RING  [P-E]►   │ [Structural Slate / Orange Trim]
   L4  ├────────────────────────────────────────────────────────┤ [Slots 1 & 2]
   L2  │   H6 CENTRAL LIFT RING (10× Atmo + 6× H2 Thrusters)    │ [Citadel Charcoal / Hazard Stripes]
   L1  └───────[▼▼▼]──────────────[▼▼▼]──────────────[▼▼▼]──────┘ [Slots 0 & 4]
```

---

### Step 6.1 · H6 Central Lift Ring (Levels L1-L4)

Mount the central lift cluster providing 19% of the Rig's total atmospheric lift (64.8 MN) and blackout emergency hover (43.2 MN H2).

```
Plan View · Y = L2 (H6 Central Lift Deck Core, 18 B Hexagon):
        X: −6   −4   −2    0   +2   +4   +6
    Z: +6  ···  ···  [A1] [H1] [A2] ···  ···
    Z: +4  ···  [A3] ···  [A4] ···  [A5] ···
    Z: +2  [A6] ···  [H2] ···  [H3] ···  [A7]
    Z:  0  ···  [H4] ···  [CC] ···  [H5] ···   ◄── [CC] Central Conveyor Well (3×3 B)
    Z: −2  [A8] ···  [H6] ···  ···  ···  [A9]
    Z: −4  ···  [A10]···  ···  ···  ···  ···
    Z: −6  ···  ···  ···  ···  ···  ···  ···
```

#### Pre-Step Parts Tray 6.1
* 10 × Large Atmospheric Thruster (large grid, 6.48 MN each = 64.8 MN total) · `RIG H6 Lift Atmo 1..10` → group `RIG Lift Atmo HUB`
* 6 × Large Hydrogen Thruster (large grid, 7.20 MN each = 43.2 MN total) · `RIG H6 Lift H2 1..6` → group `RIG Lift H2 HUB`
* 72 × Heavy Armor Slopes & Inverted Corners framing thruster exhaust nozzles.
* 12 × Heavy Armor Blast Deflectors (Level L1 nozzle rim protection).

#### Placement Instructions
1. Position 10× Large Atmospheric Thrusters pointing straight down (−Y) with 4-block exhaust clearance.
2. Interleave 6× Large Hydrogen Thrusters pointing straight down (−Y) adjacent to central conveyor well.
3. Frame thruster cluster with heavy armor triangular webs linking outer hexagonal rim to the central spine.
4. Route 2× dual large conveyor loops from central well to H2 thruster fuel inlet ports.

**✓ Check:** Nozzles face straight down (D). Center of lift aligns within 0.1 m of the central vertical axis (0, 0).

---

### Step 6.2 · H5 Junction, Gyroscope Deck & Pylon Socket Ring (Levels L5-L8)

Houses the six pylon docking sockets, master conveyor trunks, attitude gyroscopes, and avionics.

```
Plan View · Y = L6 (H5 Hexagonal Pylon Socket Deck):
                 [SOCKET P-NW]            [SOCKET P-NE]
                        ╲                      ╱
                         ┌────────────────────┐
          [SOCKET P-W] ══╡  TRUNK RING LOOPS  ╞══ [SOCKET P-E]
                         │  12× LARGE GYROS   │
                         │  [RC SEED] [RADIO] │
                         └────────────────────┘
                        ╱                      ╲
                 [SOCKET P-SW]            [SOCKET P-SE]
```

#### Pre-Step Parts Tray 6.2
* 12 × Merge Block (Large Grid, 2 per socket face) · `RIG H5 Socket P-E MRG 1, 2` …
* 6 × Large Grid Connector (1 per socket center) · `RIG H5 Socket P-E Conn` …
* 12 × Large Gyroscope (attentive horizon stabilization) · `RIG H5 Gyro 1..12` → group `RIG Gyros`
* 2 × Conveyor Loop Ring · Trunk A (WNW spine) and Trunk B (ESE spine).
* 1 × Remote Control (`RIG H5 RC Seed`) + 1× Radio Antenna (`RIG H5 Antenna 50km`).
* 6 × Spark Array Blocks (visual docking alignment rungs).

#### Placement Instructions
1. Construct 6 structural socket bulkheads facing exactly 0°, 60°, 120°, 180°, 240°, and 300°.
2. Each socket presents an 8 W × 7 H receiving face matching the pylon root collar:
   * 2× Merge Blocks at top corners (L7, X ±3 relative to pylon centerline).
   * 1× Large Grid Connector dead-center at L4 (conveyor and fuel transfer).
   * 2× Heavy Armor Guide Horns (sloped coamings) guiding incoming pylon during docking.
3. Install 12× Large Gyroscopes inside the central equipment core.
4. Mount Remote Control block oriented Forward (+Z / North) and Up (+Y / Zenith).

**✓ Check:** Each of the 6 sockets presents an 8 W × 7 H receiving face with 2 Merge Blocks and 1 central Large Connector.

---

### Step 6.3 · H4 Nuclear Power Vault (Levels L9-L12)

Houses three Large Nuclear Reactors enclosed in compartmentalized double-walled heavy armor vaults.

```
Plan View · Y = L10 (H4 Nuclear Reactor Deck):
         X: −5    −2     0    +2    +5
     Z: +4  ···   ···   [R1]  ···   ···    ◄── Large Nuclear Reactor 1 (300 MW)
     Z: +2  ···  [BAT]   │   [BAT]  ···
     Z:  0 [BAT]  ─── [CORE] ───  [BAT]   ◄── [CORE] Conveyor Fuel Feed
     Z: −2  ···  [BAT]   │   [BAT]  ···
     Z: −4  [R2]  ···   ···   ···   [R3]   ◄── Reactors 2 & 3 (300 MW each)
```

#### Pre-Step Parts Tray 6.3
* 3 × Large Nuclear Reactor (300 MW each = 900 MW total) · `RIG H4 Reactor 1, 2, 3` → group `RIG Reactors`
* 6 × Large Battery (emergency buffer, 18 MWh total) · `RIG H4 Battery 1..6` → group `RIG Batteries`
* 2 × Hydrogen Engine (auxiliary cold-start, 10 MW) · `RIG H4 H2 Engine 1, 2`
* 96 × Heavy Armor Blocks & Composite Blast Bulkheads.

#### Placement Instructions
1. Seat 3× Large Reactors on isolated structural heavy armor pads at 120° offsets.
2. Build double-walled heavy armor bulkheads separating each reactor compartment.
3. Route dedicated uranium fuel conveyor lines with redundant sorters from central trunk.
4. Ring the reactor core with 6× Large Batteries providing instantaneous UPS buffer during peak loads.

#### Pre-Enclosure Inspection Gate 6.1A: H4-H6 HUB Base Mechanical, Power & Fueling Integrity
Prior to authorizing extruder launch and autonomous flight of Section S0:
* [ ] **Conveyor Loop Integrity:** Confirm Trunk A and Trunk B maintain closed loops with 0 unpiped ports.
* [ ] **Reactor Full-Load Test:** Spin up Reactors 1-3 to 100% capacity (900 MW); confirm zero overload on 52 Atmo thrusters.
* [ ] **Blackout Battery UPS:** Disconnect all reactors; verify 6× Large Batteries sustain H6 hover for ≥ 90 seconds.
* [ ] **Socket Lock Geometry:** Confirm all 12 merge blocks and 6 connectors toggle Lock/Unlock cleanly.

---

## Chapter 7 · Sections S1-S6: Pylon Arms & Modular Bays

Six identical cantilevered arms (8 W × 7 H × 22 L blocks ≈ 20.0 × 17.5 × 55.0 m) radiating from the H5 sockets.

### 7.0 · Pylon Architecture & Clear Envelopes
* **Build Symmetry:** Pylons printed sequentially by the Extruder Ring shipyard, cold-launched from the skid runway, and flown autonomously to dock at H5.
* **Structural Envelope:**
  * Levels L1-L2: Reinforced Keel (Trunk A & B conveyors, 2× Large H2 tanks, 3× Large Atmo thrusters).
  * Levels L3-L6: Modular Hangar Deck (3 universal bays, 6 W × 4 H × 5 L clear box each).
  * Level L7: Armored Roof Deck (CIWS turret platforms, crew access gallery).
  * Tip Cross-Head (Levels L1-L7, Z +17 to +22): Plus-shaped end cap with 4× Large Atmo lift thrusters, 2× Interceptor bays, and defense turrets.

```
Transverse Cross-Section · Pylon Arm (Looking Forward toward Cross-Head Tip Z = +10):
   L7   [CIWS] ■═══════════════[ROOF GALLERY]═══════════════■ [CIWS]
   L6   ║  ┌──────────────────────────────────────────────┐  ║
   L5   ║  │       MODULAR HANGAR BAY (6 W × 4 H B)       │  ║ [Outer Armor Plate]
   L4   ║  │       Clear Volume: 15.0 × 10.0 × 12.5 m     │  ║
   L3   ║  └──────────────────────────────────────────────┘  ║
   L2   ║  [TRUNK A]   [LARGE H2 TANK 1]   [LARGE H2 TANK 2]  ║ [TRUNK B]
   L1   ■═════════════════════[V-KEEL ARMOR]═════════════════■
               [▼▼▼]                               [▼▼▼]
         [Keel Lift Atmo 1]                  [Keel Lift Atmo 2]
```

---

### Step 7.1 · Pylon Keel & Conveyor Trunks (Levels L1-L2)

```
Plan View · Y = L2 (Pylon Keel & Fuel Foundation):
        Z: 0 (Root)      5          10          15       20 (Tip)
   X: -3   [TRUNK A]═════[TRUNK A]═════[TRUNK A]═════[TRUNK A]════► [Cross-Head]
   X: -1   [MRG] [H2 TANK 1 (5.0 m)]   [ATMO LIFT 1] [H2 LIFT 1]
   X: +1   [MRG] [H2 TANK 2 (5.0 m)]   [ATMO LIFT 2] [H2 LIFT 2]
   X: +3   [TRUNK B]═════[TRUNK B]═════[TRUNK B]═════[TRUNK B]════► [Cross-Head]
```

#### Pre-Step Parts Tray 7.1
* 2 × Continuous Large Conveyor lines (Trunk A on Port, Trunk B on Starboard).
* 2 × Large Hydrogen Tank (5.0 m diameter, 5,000,000 L capacity each) · `RIG P<n> Tank 1, 2`.
* 3 × Large Atmospheric Thruster (D) · `RIG P<n> Lift Atmo 1..3` (embedded in keel flank pockets).
* 2 × Large Hydrogen Thruster (D) · `RIG P<n> Lift H2 1, 2` (keel center).
* 44 × Heavy Armor Sloped Blocks forming deflective V-hull keel.

#### Placement Instructions
1. Lay heavy armor V-keel along bottom centerline (Level L1).
2. Seat twin Large H2 tanks end-to-end between Z +2 and Z +12 along the pylon centerline.
3. Flank tanks with continuous Large Conveyor Trunks A and B at X ±3.
4. Recess 3× Large Atmo lift thrusters into downward-facing keel pockets.

**✓ Check:** Both conveyor trunks run uninterrupted from root collar to tip cross-head.

---

### Step 7.2 · Modular Hangar Bays (Levels L3-L6, Bays 1-3)

Three universal drone bays per arm, opening alternately into flanking T-Sectors.

```
Plan View · Y = L4 (Pylon Hangar Level):
        Z: 0 (Root)      6           11          16       22 (Tip)
           ┌──────────┬───────────┬───────────┬───────────┬───────┐
    Port   │  COLLAR  │ [BAY 1]   │ Bay Wall  │ [BAY 3]   │ CROSS │
           │  (2 B)   │ (Door P)  │           │ (Door P)  │ HEAD  │
           ├──────────┤           ├───────────┤           │ (6 B) │
    Stbd   │  SOCKET  │ Bay Wall  │ [BAY 2]   │ Bay Wall  │       │
           │  COLLAR  │           │ (Door S)  │           │       │
           └──────────┴───────────┴───────────┴───────────┴───────┘
```

#### Pre-Step Parts Tray 7.2 (Per Pylon · ×3 Bays)
* 3 × Large Grid Connector (ceiling recessed) · `RIG P<n> Bay<m> Conn`.
* 3 × Ship Welder (maintenance & repair) · `RIG P<n> Bay<m> Welder`.
* 3 × Sensor Block (approach automation) · `RIG P<n> Bay<m> Sensor`.
* 12 × Airtight Hangar Door Blocks · `RIG P<n> Bay<m> Doors`.
* 3 × Spotlight (docking lighting) · `RIG P<n> Bay<m> Light`.

#### Placement Instructions
1. Bay 1 opens Port (Z +2 to +7); Bay 2 opens Starboard (Z +7 to +12); Bay 3 opens Port (Z +12 to +17).
2. Install 4× Hangar Door blocks along outer opening, sliding flush into ceiling frame.
3. Mount ceiling connector recessed 1 block above clear envelope with guide coamings.
4. Mount ship welder flush behind protective grated floor plate.

**✓ Check:** Clear interior: 6 B wide × 4 B high × 5 B deep (15.0 × 10.0 × 12.5 m). Accommodates Remora, Kestrel, or Albatross.

---

### Step 7.3 · Pylon Lift Pods & Tip Cross-Head (Levels L1-L7)

```
Plan View · Y = L4 (Tip Cross-Head Pod, Z +17 to +22):
         X: −4    −2     0    +2    +4
     Z: +22 ···  [CIWS] [FWD] [CIWS] ···    ◄── Tip Forward Defense & Prow
     Z: +20 [LAT] [A4]  [COR]  [A5] [LAT]   ◄── Lateral Atmo + Tip Lift Thrusters
     Z: +18 ···   [A6]  [COR]  [A7]  ···    ◄── 4× Large Atmo Lift Pods (25.92 MN)
     Z: +16 ···   [H3]  [COR]  [H4]  ···    ◄── 2× Large H2 Emergency Lift (14.4 MN)
```

#### Pre-Step Parts Tray 7.3
* 4 × Large Atmospheric Thruster (D) · `RIG P<n> Lift Atmo 4..7` (cross-head cluster, 25.92 MN).
* 2 × Large Hydrogen Thruster (D) · `RIG P<n> Lift H2 3, 4` (cross-head emergency boost, 14.4 MN).
* 4 × Small Atmospheric Thruster · `RIG P<n> Thruster Lat 1..4` (lateral stationkeeping).
* 2 × Interceptor Hatch Bays (roof launch tubes).
* 2 × Gatling CIWS Turrets (1 dorsal at L7, 1 ventral at L1).

#### Placement Instructions
1. Construct the plus-shaped cross-head end cap projecting 4 blocks beyond hangar bay 3.
2. Cluster 4× Large Atmo lift thrusters in symmetrical 2×2 box around central corridor.
3. Mount dorsal and ventral CIWS turrets providing 360° hemispherical defense over pylon tip.
4. Install lateral stationkeeping thrusters on outer beam facets.

#### Pre-Enclosure Inspection Gate 7.1A: Flown Pylon Flight Authority & Docking Handshake
Prior to authorizing extruder merge release and autonomous flight of any Pylon:
* [ ] **Lift Authority Proof:** Under 100% test override, 7× Large Atmo thrusters deliver 45.36 MN upward force, producing T/W ≥ 1.35 against pylon dry mass (≈ 2,900 t).
* [ ] **Fuel Capacity:** Verify twin Large H2 tanks register 100% fill level (10,000,000 L).
* [ ] **Autonomous Docking Telemetry:** Remote Control locks onto target H5 socket beacon; merge blocks arm cleanly.

---

## Chapter 8 · Sections S7-S9: Upper Decks (H1-H3)

Stacked vertically atop the H4 Nuclear Vault to complete the Rig citadel.

### 8.0 · Upper Citadel Layout & Master Elevation
* **Section S7 (Level L13-L16 · H3):** Industrial Core, O2/H2 Gas Farm & Heavy Maintenance Shop.
* **Section S8 (Level L17-L20 · H2):** Crew Citadel, Medical Center, Hydroponics & Life Support.
* **Section S9 (Level L21-L24 · H1):** Master Armored Command Bridge, Avionics Citadel & Roof Picket Deck.

```
Longitudinal Centerline Profile · Upper Citadel (H1-H3):
   L24 ┌───────────────────[ROOF PICKET FLIGHT DECK]───────────────────┐
   L22 │   H1 MASTER COMMAND BRIDGE · 4× RIG//OS PROGRAMMABLE BLOCKS   │ [Section S9]
   L20 ├───────────────────────────────────────────────────────────────┤
   L18 │   H2 CREW CITADEL · MEDICAL BAY · CRYO PODS · GRAVITY CORE    │ [Section S8]
   L16 ├───────────────────────────────────────────────────────────────┤
   L14 │   H3 GAS FARM (8× H2 TANKS) · 4× O2/H2 GENS · CORE SHOP      │ [Section S7]
   L12 └───────────────────[H4 NUCLEAR CORE CITADEL]───────────────────┘
```

---

### Step 8.1 · Section S7: H3 Gas Farm & Core Machine Shop (Levels L13-L16)

```
Plan View · Y = L14 (H3 Gas Farm & Machine Shop):
         X: −5    −2     0    +2    +5
     Z: +4  [GEN1] [H2 TANK 1..4]   [GEN2]
     Z: +2  [ASM1]  ─── [CORE] ───  [ASM2]  ◄── 2× Advanced Assemblers (400% speed)
     Z:  0  [REF1]  ─── [WELL] ───  [REF2]  ◄── 2× Industrial Refineries (200% yield)
     Z: −2  [ASM3]  ─── [CORE] ───  [ASM4]
     Z: −4  [GEN3] [H2 TANK 5..8]   [GEN4]
```

#### Pre-Step Parts Tray 8.1
* 8 × Large Hydrogen Tank (gas farm reserve, 40,000,000 L total) · `RIG H3 Tank 1..8`.
* 4 × Large O2/H2 Generator · `RIG H3 O2H2 Gen 1..4`.
* 2 × Refinery (Yield modules) · `RIG H3 Refinery 1, 2`.
* 4 × Assembler (Speed modules) · `RIG H3 Assembler 1..4`.
* 1 × Large Cargo Container (`RIG H3 Hold Components`).

#### Placement Instructions
1. Install 8× Large H2 tanks in symmetrical perimeter rings.
2. Center 4× O2/H2 generators adjacent to central conveyor well.
3. Install compact maintenance refinery and assembler cells for autonomous parts replenishment.

**✓ Check:** Gas farm plumbed into Trunk A & B loops. Ice processing capacity ≥ 2,000 kg/s.

---

### Step 8.2 · Section S8: H2 Crew Citadel, Medical Center & Life Support (Levels L17-L20)

```
Plan View · Y = L18 (H2 Crew Citadel Deck):
         X: −5    −2     0    +2    +5
     Z: +4  [CREW QUARTERS]   │   [HYDROPONICS 1]
     Z: +2  [MESS HALL]     [AIR] [HYDROPONICS 2]
     Z:  0  [MED BAY]      [GRAV] [O2 TANKS 1..2]  ◄── [GRAV] Artificial Gravity Core
     Z: −2  [CRYO PODS 1-4] [AIR] [SURVIVAL KIT]
     Z: −4  [ARMORY LOCKERS]  │   [DECON AIRLOCK]
```

#### Pre-Step Parts Tray 8.2
* 1 × Medical Room (`RIG H2 MedBay`) + 4× Cryo Chambers (`RIG H2 Cryo 1..4`).
* 1 × Gravity Generator (`RIG H2 GravGen`, 1.0 g downward acceleration).
* 2 × Large O2 Tank (200,000 L life support reserve) · `RIG H2 Tank O2 1, 2`.
* 4 × Air Vent (hermetic pressurization) · `RIG H2 Vent 1..4`.
* 2 × Armory Lockers + 1× Survival Kit.

#### Placement Instructions
1. Mount Gravity Generator dead-center at X0, Z0, L18 to provide uniform 1.0 g across H1-H3.
2. Construct hermetic crew quarters and medical triage suite on Port flank.
3. Install automated gas-reclamation decontamination airlock at elevator stairwell head.

**✓ Check:** H2 Citadel holds 100% O2 pressure (1.00 atm) with zero leakage when sealed.

---

### Step 8.3 · Section S9: H1 Master Command Bridge, Avionics & Roof Pickets (Levels L21-L24)

```
Plan View · Y = L22 (H1 Master Command Bridge):
         X: −5    −2     0    +2    +5
     Z: +4  ···  [C2-1] [HELM] [C2-2] ···   ◄── Forward Panoramic Armored Glazing
     Z: +2  ···  [PB-1] [PB-2] [PB-3] ···   ◄── [PB 1-4] RIG//OS MDK2 Compute Rack
     Z:  0  ···  [PB-4] [DISP] [SEAT] ···   ◄── Central Command Consoles
     Z: −2  ···  [RAD1] [WELL] [RAD2] ···   ◄── Avionics Comms Racks
     Z: −4  ···  ···   [AIRLOCK] ···  ···   ◄── Stairwell to Roof Flight Deck
```

#### Pre-Step Parts Tray 8.3
* 1 × Main Cockpit / Helm (`RIG H1 Helm Master`).
* 2 × Operator Consoles (`RIG H1 Console Fleet`, `RIG H1 Console Yard`).
* 4 × Programmable Block (`[RIG] PB Rig OS`, `[RIG] PB Dispatcher`, `[RIG] PB Inventory`, `[RIG] PB Displays`).
* 2 × Picket Drone Landing Pads (Level L24 roof) with recessed connectors (`RIG Roof Pad 1, 2 Conn`).
* 4 × Point Defense Gatling Turrets (`RIG Turret Roof 1..4`).
* 1 × Laser Antenna (`RIG H1 Laser C2`) + 1× Radio Antenna (`RIG H1 Antenna 50km`).

#### Placement Instructions
1. Center 3-operator bridge forward at Z +4 overlooking North approaches through sloped heavy glass.
2. Install 4-rack Programmable Block computer core in shielded armored avionics bay.
3. Finish Level L24 roof deck with twin Picket Drone landing pads and 360° CIWS defense perimeter.

#### Pre-Enclosure Inspection Gate 8.1A: Citadel Pressurization, Avionics & C2 Relay Integrity
* [ ] **Full-Hull Pressurization:** Confirm H1, H2, and H3 maintain 100% O2 pressure indefinitely.
* [ ] **Gravity Alignment:** Confirm Gravity Generator produces exactly 1.0 g downward across all bridge consoles.
* [ ] **C2 Laser Uplink:** Establish laser link to Ground Base C2 spire with zero packet loss.

---

## 8.4 · Master Electrical Single-Line & Power Modes

### 1. Power Generation Hierarchy
* Primary Base: 3 × Large Nuclear Reactor (H4) = 900.0 MW.
* Auxiliary Support: 2 × Hydrogen Engine (H4) = 10.0 MW.
* Dynamic Buffer: 18 × Large Battery (6 in HUB, 2 per Pylon) = 54.0 MWh capacity (216.0 MW peak surge).
* **Total Peak Power Available:** 1,126.0 MW.

### 2. Operational Energy Modes
| Mode | Operational State | Total Generation | Total Load | Net Power Margin | Primary Source |
|---|---|---|---|---|---|
| Mode 1 | Static Stationkeeping Hover | 900.0 MW | 638.0 MW | +262.0 MW | 3× Nuclear Reactors |
| Mode 2 | High-Speed Flight Transit / Climb | 1,116.0 MW | 780.0 MW | +336.0 MW | Reactors + Battery Assist |
| Mode 3 | Silent Loiter / Minimum Power | 216.0 MW | 45.0 MW | +171.0 MW | 18× Batteries Only |
| Mode 4 | Full Fleet Refit & Production | 900.0 MW | 750.0 MW | +150.0 MW | Reactors (O2/H2 + Asm) |
| Mode 5 | Battle Stations / N-1 Failover | 600.0 MW | 638.0 MW | −38.0 MW (UPS on Bat) | 2× Reactors + Batteries |

---

## 8.5 · Master BOM & Earth 1 g Lift Physics Budget (Decision 48)

### 1. Master Bill of Materials (Complete 25 kt Fortress)
| Subsystem / Section | Block Count | Dry Mass (t) | PCU | Primary Thrusters | Power Generation |
|---|---|---|---|---|---|
| S0 HUB Base (H4-H6) | 1,420 | 7,200.0 | 8,500 | 10 Atmo + 6 H2 | 3× Large Reactors (900 MW) |
| S1-S6 Pylons (×6 arms) | 2,880 (480 ea) | 14,400.0 (2,400 ea) | 16,800 (2,800 ea) | 42 Atmo + 24 H2 (7 A + 4 H ea) | 12 Large Batteries (36 MWh) |
| S7-S9 Upper Citadel (H1-H3)| 680 | 3,400.0 | 4,200 | None | 2 H2 Engines (10 MW) |
| **Complete Floating Rig** | **4,980** | **25,000.0 t** | **29,500** | **52 Large Atmo + 30 Large H2** | **910 MW Nuclear/H2 + 54 MWh Bat** |

### 2. Earth 1 g Lift Physics Budget
* **Fortress Gross Mass ($M$):** 25,000,000 kg (25,000 t).
* **Gravity Force ($F_g$):** $25,000,000 \text{ kg} \times 9.81 \text{ m/s}^2 = 245.25 \text{ MN}$.
* **Atmospheric Lift Array (52× Large Atmo Thrusters):**
  * HUB H6: 10 × 6.48 MN = 64.80 MN.
  * Pylons S1-S6: 6 × (7 × 6.48 MN) = 272.16 MN.
  * Total Atmo Downward Thrust = 336.96 MN.
  * **Hover Throttle Margin:** $245.25 \text{ MN} / 336.96 \text{ MN} = 72.78\% \approx 72.8\%^\dagger$ (Decision 48 verified).
* **Emergency Hydrogen Lift Array (30× Large H2 Thrusters):**
  * HUB H6: 6 × 7.20 MN = 43.20 MN.
  * Pylons S1-S6: 6 × (4 × 7.20 MN) = 172.80 MN.
  * Total Hydrogen Downward Thrust = 216.00 MN.
* **Combined All-Thruster Maximum Lift:** $336.96 \text{ MN} + 216.00 \text{ MN} = 552.96 \text{ MN}$.
* **Maximum Emergency T/W Ratio:** $552.96 \text{ MN} / 245.25 \text{ MN} = 2.254$ (Earth 1 g).

---

## 8.6 · Master AI Scripting Contract & Terminal Namespace (Claude Code / RIG//OS)

Every device on the Floating Rig conforms to this exact naming and grouping dictionary:

### 1. Group Taxonomy
* `RIG Lift Atmo HUB`: 10× Large Atmospheric Thrusters on H6.
* `RIG Lift Atmo Pylons`: 42× Large Atmospheric Thrusters on Pylons S1-S6.
* `RIG Lift Atmo All`: All 52× Large Atmospheric Thrusters.
* `RIG Lift H2 All`: All 30× Large Hydrogen Thrusters.
* `RIG Reactors`: 3× Large Nuclear Reactors on H4.
* `RIG Batteries`: All 18× Large Batteries across HUB and Pylons.
* `RIG Gyros`: All 24× Large Gyroscopes.
* `RIG Pylon Sockets`: All 12× Merge Blocks and 6× Connectors on H5.

### 2. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Large Atmo Thruster | `RIG H6 Lift Atmo 1` … `10` | Central HUB primary lift array |
| Large H2 Thruster | `RIG H6 Lift H2 1` … `6` | Central HUB emergency lift boost |
| Large Atmo Thruster | `RIG P<1..6> Lift Atmo 1` … `7`| Pylon primary lift thrusters (keel + tip pod) |
| Large H2 Thruster | `RIG P<1..6> Lift H2 1` … `4` | Pylon emergency lift boost |
| Large Nuclear Reactor | `RIG H4 Reactor 1` … `3` | Primary 900 MW heavy nuclear power plant |
| Large Battery | `RIG H4 Battery 1` … `6` | HUB core UPS emergency buffer |
| Large Battery | `RIG P<1..6> Battery 1` … `2` | Pylon independent flight power buffer |
| Merge Block | `RIG H5 Socket P-<Dir> MRG 1, 2`| Pylon root structural locking merge heads |
| Large Connector | `RIG H5 Socket P-<Dir> Conn` | Pylon root fuel and conveyor umbilical connector |
| Programmable Block | `[RIG] PB Rig OS` | Core state machine, hover maintenance, mode selection |
| Programmable Block | `[RIG] PB Dispatcher` | Flight bay traffic control, drone lease orchestration |
| Programmable Block | `[RIG] PB Inventory` | Fleet quota tracking, refinery sorting, assembler queues |
| Programmable Block | `[RIG] PB Displays` | Zero-GC monospace bridge and deck telemetry dashboards |

### 3. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `RIG EC Lift` | Grid Vertical Velocity | Velocity < −2.0 m/s (Altitude Loss) | `RIG Lift H2 All` ON · Throttle = 100% | `RIG Lift H2 All` OFF (Atmo hover only) |
| `RIG EC Power` | `RIG H4 Reactor 1..3` | Combined Output < 600 MW (N-1 Event)| `RIG Batteries` Set Discharge · Sound Alert | `RIG Batteries` Set Auto |
| `RIG EC Hull` | Compartment Air Vents | Pressure < 0.50 atm (Hull Breach) | Lock Emergency Bulkhead Doors · Klaxon ON | Bulkhead Doors Auto |

### 4. IGC Channel Subscriptions & Broadcast Protocols
* Inbound: `RIG.OS.COMMAND` (`MODE;<name>`, `LIFT;<pct>`, `EMERGENCY`, `RELOAD`).
* Inbound: `TMS.BASE.HANDOVER` (C2 root election and operational transfer).
* Outbound: `RIG.OS.STATUS` (Broadcasts `HOVER;<altitude>;<atmo_throttle_pct>;<power_margin_mw>`).
* Outbound: `RIG.OS.HEARTBEAT` (Cyclic 1.0 s watchdog ping to fleet).

---

## 8.7 · Acceptance Testing Protocols (Suites A-J)

* [ ] **Suite A (Static Hover Stability):** Hover at 500 m AGL; confirm atmospheric throttle sits within 72.8% ± 1.5%† at sea level Earth 1 g.
* [ ] **Suite B (N-1 Nuclear Loss Failover):** Power down Reactor 1; confirm Batteries instantly supply shortfall without losing altitude.
* [ ] **Suite C (Pylon Cold Docking Handshake):** Disconnect Pylon S1; fly 50 m clear; return and dock under Remote Control with 0 subgrid damage.
* [ ] **Suite D (Simultaneous Swarm Launch):** Cycle 6 modular hangar bays simultaneously; verify zero conveyor lockups or door conflicts.
* [ ] **Suite E (Atmospheric Ceiling Climb):** Ascend to 18 km ceiling†; confirm smooth transition to H2 emergency thrusters as atmospheric density decays.

---

# Volume IV · Hardware-to-Scripting Interface Contract

This volume establishes the authoritative software contract, distributed compute architecture, and inter-grid communications protocol for **Claude Code** and the **RIG//OS** ecosystem.

```
Distributed Multi-Grid Compute & C2 Architecture:
   ┌────────────────────────────────────────────────────────────────────────┐
   │                  ROOT C2 ARBITRATION & FLEET COMMAND                   │
   │  [GND] PB FleetOps (Base) ◄──► [MAS] PB RIG//OS-Lite ◄──► [RIG] PB Rig OS │
   └───────────────────┬────────────────────────────────┬───────────────────┘
                       │                                │
                       ▼                                ▼
   ┌───────────────────────────────────┐┌───────────────────────────────────┐
   │     CARRIER & EXTRACTOR TIER      ││     SURFACE SHIPYARD & REPAIR     │
   │   [ORC] PB Carrier Core (Orca)    ││   [EXT] PB Yard Master (Extruder) │
   │   [MAS] PB Excavator (Mastodon)   ││   [MAS-U] PB Utility (FOB C2)     │
   │   [CAT] PB LandTrain (Caterpillar)││                                   │
   └───────────────────┬───────────────┘└───────────────────────────────────┘
                       │
                       ▼
   ┌────────────────────────────────────────────────────────────────────────┐
   │                   TACTICAL SWARM & LOGISTICS DRONES                    │
   │   [REM] PB Miner · [MUL] PB Shuttle · [KES] PB Scout · [ALB] PB Utility│
   └────────────────────────────────────────────────────────────────────────┘
```

---

## Chapter 9 · RIG//OS Hardware Abstraction & IGC Dictionary

### 9.0 · Architectural Tenets & The 5 Immutable Sandbox Invariants

All script code running within the Space Engineers 1 Programmable Block environment across every fleet grid must strictly satisfy these five hard invariants (CLAUDE.md / Rule C4):

1. **Rule C4 Native Block Autonomy:** No critical safety function (atmospheric lift hold, reactor failover, emergency braking, pressure seal) may depend on a PB. Native Event Controllers and Timer Blocks must execute all survival-critical actions if the PB halts.
2. **Pure Zero-GC Hot Paths:** Absolutely zero allocations in `Main()`, `Update1()`, `Update10()`, or `Update100()`. No `new`, no string concatenation (`+`), no string interpolation (`$"{var}"`), no `.ToString()` on primitives or enums. Strings must be formatted via `SbFormat` into pre-allocated `StringBuilder` buffers.
3. **Full Minifier Immunity:** Code must withstand Mal's MDK2 `minify=full` identifier renaming. Never use `enum.ToString()`, `Enum.Parse()`, `Enum.GetValues()`, or `nameof()`. CustomData keys, Storage keys, Terminal Block names, and IGC tags must be string literals.
4. **Strict Grid Scoping:** Every terminal block query must filter `b => b.CubeGrid == Me.CubeGrid`. Mechanical subgrid blocks (hinges, rotors, pistons, merge heads) must be located via explicitly cached entity IDs or exact unique string names.
5. **C# 6 Compiler Constraint:** All script code must compile under C# 6. No local functions, no out-variable declarations in call expressions, no tuple types, no pattern matching.

---

### 9.1 · Operating Modes & Multi-Grid State Machine

Fleet grids coordinate via five standardized Operating Modes broadcast over `RIG.OS.MODE` and `TMS.C2.HANDOVER`:

| Mode ID | Mode Name | Trigger / System State | Atmospheric Lift State | Power & Weapons State | Logistics & Swarm State |
|---|---|---|---|---|---|
| `MODE_NRM` | Normal Operations | Standard planetary hover / transit | 52 Atmo @ 72.8% hover; H2 OFF | Reactors 100%; Shields UP; CIWS AUTO | Drone bays open; Leases active |
| `MODE_CAU` | Weather / Threat Caution | Approaching storm / unverified craft | 52 Atmo @ 75%; H2 warm standby | Reactors 100%; CIWS ARMED | Drones recalled to 500 m pre-dock |
| `MODE_DEF` | Tactical Defense / Shield | Incoming kinetic / artillery threat | 52 Atmo @ 85%; Lateral thrusters hot | Shields 100% OVERDRIVE; CIWS ENGAGED | Bay doors sealed; Air wing scrambled |
| `MODE_EMG` | Emergency / N-1 Failure | Reactor failure or altitude loss | 52 Atmo @ 100% + 30 H2 FIRE | Battery UPS active; Shedding non-vital | Swarm emergency release; RTH abort |
| `MODE_ECO` | Low-Power Loiter | Silent loiter / night conservation | 52 Atmo min hover; H2 locked | Batteries only; Refineries paused | Maintenance idle; Beacon low-power |

```
Operating Mode State Transition Logic:
         ┌────────────────────────────────────────────────────────┐
         │                                                        │
         ▼                                                        │
   ┌───────────┐      Threat / Storm       ┌───────────┐          │
   │ MODE_NRM  ├──────────────────────────►│ MODE_CAU  │          │
   └─────┬─────┘                           └─────┬─────┘          │
         │                                       │                │
         │ Low Power Demand                      │ Combat Alert   │ All Clear
         ▼                                       ▼                │
   ┌───────────┐                           ┌───────────┐          │
   │ MODE_ECO  │                           │ MODE_DEF  │          │
   └───────────┘                           └─────┬─────┘          │
         │                                       │                │
         │               N-1 Reactor Trip        │ Severe Hull    │
         └───────────────────────┬───────────────┘ Damage         │
                                 ▼                                │
                           ┌───────────┐                          │
                           │ MODE_EMG  ├──────────────────────────┘
                           └───────────┘
```

---

### 9.2 · Universal Monospace Display Standard (Zero-GC Dashboards)

All LCD viewports on bridge consoles, operator stations, and observation galleries conform to a strict 40-column monospace layout (Font: `Monospace`, Size: `0.8`):

#### 1. Color Coding Standard
* **Nominal / OK:** Green (`#2ECC71` / RGB `46, 204, 113`)
* **Buffering / Standby:** Yellow (`#F1C40F` / RGB `241, 196, 15`)
* **Warning / Low Stock:** Orange (`#E67E22` / RGB `230, 126, 34`)
* **Critical / Tripped:** Red (`#E74C3C` / RGB `231, 76, 60`)
* **Telemetry / Comms:** Cyan (`#1ABC9C` / RGB `26, 188, 156`)

#### 2. Standard 40-Column HUD Templates

```
Monospace Viewport Layout 1 · Rig Helm Kinematics HUD (40 × 14 chars):
1234567890123456789012345678901234567890
========================================
 FLOATING RIG 25 kt FORTRESS · HELM HUD 
========================================
 MODE:        STATIONKEEPING            
 ALTITUDE:    500.0 m AGL (Earth 1 g)   
 ATMO HOVER:  72.8% [==============....]
 H2 BOOST:    STANDBY (30× ARMED)       
 POWER BUS:   638.0 / 900.0 MW (OK)     
 BATTERY UPS: 100.0% (54.0 / 54.0 MWh)  
 PYLON ARMS:  6 / 6 Flown Sockets LOCKED
 MASS:        25,000.0 t (Nominal)      
 N-1 ALARM:   CLEAR                     
 WATCHDOG:    OK (Tick: 1420)           
========================================

Monospace Viewport Layout 2 · Fleet Swarm Dispatcher HUD (40 × 14 chars):
========================================
 FLEET SWARM DISPATCHER · BAY TRAFFIC   
========================================
 AIR WING:    6 / 6 Flown Drones ACTIVE 
 BAY P1-REM:  MINING (Shaft -45 m, 82%) 
 BAY P2-KES:  SURVEY (Fe Vein Claimed)  
 BAY P3-REM:  RETURNING (Hold 100%)     
 BAY S1-REM:  DOCKED (Discharging Ore)  
 BAY S2-ALB:  STANDBY (Hull Repair Ready)
 BAY S3-REM:  EN ROUTE (Shaft 2)        
 ROOF PAD:    MULE-01 LOCKED (Draining) 
 CARRIER C2:  ORCA-01 HOVERING (Zone B) 
 WATCHDOG:    OK (Tick: 1420)           
========================================
```

---

### 9.3 · Master IGC Protocol & Channel Dictionary

All inter-grid communications use Unicode text payloads with semicolons (`;`) separating fields. Every message tag strictly conforms to the system namespace:

```
IGC Channel Hierarchy:
   RIG.* ──► Floating Rig Master Channels (OS, Displays, Extruder, Yard)
   TMS.* ──► Terrestrial Mining System (Orca, Remora, Mule, Mastodon, Caterpillar, C2)
   FLT.* ──► Flight & Swarm Dispatcher (Bay Leases, Flight Vectors, Traffic)
```

#### Complete Channel Registry
| Channel Name | Senders | Receivers | Payload Format | Functional Purpose |
|---|---|---|---|---|
| `RIG.OS.HEARTBEAT` | [RIG] PB Rig OS | All Fleet Grids | `station_id;mode;timestamp` | Master Rig presence & operating mode pulse |
| `RIG.OS.STATUS` | [RIG] PB Rig OS | Base C2, Mastodon | `state;mode;atmo_pct;pwr_mw;bat_pct;pylons` | Rig hover kinematics & fortress health |
| `RIG.OS.COMMAND` | Base C2, Bridge | [RIG] PB Rig OS | `command;argument;target` | Mode switching, emergency override, thruster lock |
| `RIG.EXT.COMMAND` | Base C2, Rig | [EXT] PB Yard | `command;blueprint_name` | Start, pause, resume, abort extruder print |
| `RIG.EXT.STATUS` | [EXT] PB Yard | Base C2, Rig | `state;bp_name;progress_pct;rem_sec;status` | Extruder ring progress & stroke telemetry |
| `RIG.EXT.COMMISSION`| [EXT] PB Yard | Pylon Seed Kit | `COMMISSION;blueprint_name` | Pylon seed ignition pulse & launch authorization |
| `TMS.ORCA.STATUS` | [ORC] PB Carrier| Base C2, Mule | `carrier_id;state;ore_pct;bat_pct;mw;free_bays`| Orca mining progress & drone bay occupancy |
| `TMS.MULE.CALL` | Orca, Mastodon | Base C2, Mule | `requester_id;pad_id;cargo_type;urgency` | JIT hauler request (ore or resupply kit) |
| `TMS.MULE.STATUS` | [MUL] PB Shuttle| Orca, Base C2 | `mule_id;state;cargo_mass_t;bat_pct;dest_pad` | Freight shuttle telemetry & approach status |
| `TMS.SCOUT.TASK` | Base C2, Mastodon| [KES] PB Scout | `TASK;zone_id;gps_center;radius_m` | Reconnaissance tasking dispatch |
| `TMS.SCOUT.VEIN` | [KES] PB Scout | Fleet C2, Miners | `VEIN;ore_type;depth_m;gps_coords` | Geological deposit discovery notification |
| `TMS.BASE.DEMAND` | [GND] PB Fleet | Mastodon, Orca | `DEMAND;deficit_ore_type;deficit_kg` | Pull-economy refinery deficit broadcast |
| `TMS.BASE.QUOTAS` | [GND] PB Fleet | Extruder, Yard | `QUOTA;component_name;required;current` | Shipyard fabrication buffer requirements |
| `TMS.C2.HANDOVER` | C2 Root Nodes | Fleet Grids | `HANDOVER;target_node;timestamp` | Dynamic C2 root master election / transfer |
| `TMS.RESUPPLY.REQ` | Mastodon, Rig | Base C2, Mule | `REQ;unit_id;kit_type;urgency` | SML standardized resupply kit request |
| `FLT.REM.STATUS` | [REM] PB Miner | Orca, Base C2 | `miner_id;state;depth_m;rate_kgs;fill_pct` | Remora vertical drill telemetry & rate |
| `FLT.DISP.LEASE` | Dispatcher PB | Remora, Swarm | `LEASE;drone_id;bay_id;waypoint_gps;sec` | Flight bay approach lease & reservation |
| `FLT.DISP.STATUS` | Swarm Drones | Dispatcher PB | `drone_id;flight_state;cargo_pct;bat_pct` | Drone approach & docking status |

---

### 9.4 · Detailed IGC Payload Schemas & Parsing Standards

#### 1. JIT Logistics Pull Request (`TMS.MULE.CALL`)
* **Format:** `<requester_id>;<pad_id>;<cargo_type>;<urgency>`
* **Fields:**
  * `requester_id`: String (`ORC-01`, `MAS-01`, `GND-C2`).
  * `pad_id`: String (`ORC-MP`, `MAS-MP`, `GND-PAD`).
  * `cargo_type`: String (`ORE`, `COMPONENTS`, `MUNITIONS`, `ICE`).
  * `urgency`: String (`NORMAL`, `HIGH`, `CRITICAL`).
* **Example:** `ORC-01;ORC-MP;ORE;HIGH`

#### 2. Ore Vein Discovery Report (`TMS.SCOUT.VEIN`)
* **Format:** `VEIN;<ore_type>;<depth_m>;<gps_coords>`
* **Fields:**
  * `ore_type`: String (`URANIUM`, `PLATINUM`, `GOLD`, `MAGNESIUM`, `IRON`).
  * `depth_m`: Fixed-point 1 decimal (`32.5`).
  * `gps_coords`: Standard SE GPS string (`GPS:Vein-U-01:12500.5:4320.1:-8920.4:#FF0000:`).
* **Example:** `VEIN;URANIUM;38.0;GPS:Vein-U-01:12500.5:4320.1:-8920.4:#FF0000:`

#### 3. Flight Bay Lease Reservation (`FLT.DISP.LEASE`)
* **Format:** `LEASE;<drone_id>;<bay_id>;<waypoint_gps>;<lease_sec>`
* **Fields:**
  * `drone_id`: String (`REM-01`, `KES-01`, `ALB-01`).
  * `bay_id`: String (`P-E-BAY-1`, `ORC-BAY-P1`).
  * `waypoint_gps`: Holding waypoint GPS string.
  * `lease_sec`: Integer seconds (`180`).
* **Example:** `LEASE;REM-01;ORC-BAY-P1;GPS:Hold-P1:12000.0:4000.0:-8000.0:#00FF00:;180`

---

### 9.5 · Zero-PB Safety Ladders & Rule C4 Compliance Matrix

If every Programmable Block on a grid suffers total power loss or script crash, native hardware blocks execute the following deterministic safety procedures:

| Subsystem | Primary PB Function | Native Block Failover Execution (PB Offline) | Fallback Hardware |
|---|---|---|---|
| **Rig Lift Hold** | Dynamic hover throttle & pitch trim | `RIG EC Lift` detects vertical descent > −2.0 m/s; overrides 30× Large H2 thrusters to 100% | Event Controller + Timer Block |
| **Power Grid N-1** | Nuclear load balancing & battery boost | `RIG EC Power` trips on reactor drop < 600 MW; forces 18× Large Batteries to Discharge | Event Controller + Battery Charge Mode |
| **Hull Pressurization**| Compartmental airflow management | `RIG EC Hull` trips on air vent pressure < 0.50 atm; slams heavy blast bulkhead doors | Event Controller + Door Blocks |
| **Drone Clamping** | Soft-docking alignment & clamp timing| Native connector auto-connect on proximity + magnetic plate auto-lock | Connectors + Magnetic Plates |
| **Mastodon Hitch** | 3-axis gimbal damping & anti-sway | Speed-activated magnetic plates engage when consist speed exceeds 30 km/h | Native Event Controller on Speed |
| **Extruder Stroke**| Piston rate calculation & welder shutoff| `EXT EC Print` trips on Pusher 6 reaching 10.0 m; toggles Welder Ring OFF and sounds alarm | Native Event Controller on Position |

---

### 9.6 · Multi-Grid Conveyor Routing & Segregated Circuit Topology

To prevent conveyor contention and ensure instantaneous component and fuel flow across docked subgrids:

```
Conveyor Circuit Topology (Universal Standard):
           [RAW ORE MANIFOLD] ────────► [REFINERY INTAKE SORTERS]
                   │                              │
                   ▼                              ▼
           [STONE EXTRACTOR]              [INGOT STORAGE CORE]
                   │                              │
                   ▼                              ▼
      [CONCRETE BLOCKS FABRICATION]       [ASSEMBLER PRODUCTION]
                   │                              │
                   ▼                              ▼
      [BULK GRAVEL BALLAST DUMP]          [COMPONENT INVENTORY]
                                                  │
                                                  ▼
                                      [JIT RESUPPLY EXPORT SORTER]
```

1. **Stone Retention Loop (Decision 32 & 60):** 100% of mined stone is retained. Waste gravel is converted into Concrete Components for terrestrial infrastructure (Ground Base foundations, Caterpillar roads) via dedicated assemblers; zero gravel is dumped into flight areas.
2. **Dedicated Sorter Isolation:** All refueling lines (H2, O2, Uranium) feature one-way conveyor sorters with strict whitelists, preventing reverse contamination or fuel siphoning by docked drones.
3. **Manual Physical Inspection Ports:** Every large cargo hold, refinery cluster, and conveyor sorter incorporates a flush inspection port accessible by engineers without disassembling outer armor hulls.

---

### 9.7 · Scripting Performance Bounds & Instruction Count Matrix

Every MDK2 script authored for the ecosystem operates well within the Space Engineers PB Sandbox ceiling (50,000 instructions per execution):

| Project Script | Target Program | Average Instructions / Tick | Peak Instructions / Tick | Target Frequency | Runtime Allocations |
|---|---|---|---|---|---|
| `Rig.OS` | `[RIG] PB Rig OS` | 1,420 | 3,850 | `Update10` | 0 B / tick |
| `Rig.OS.Lite` | `[MAS] PB RIG//OS-Lite` | 1,180 | 3,200 | `Update10` | 0 B / tick |
| `Base.C2` | `[GND] PB FleetOps` | 1,350 | 3,900 | `Update10` | 0 B / tick |
| `Carrier.Orca` | `[ORC] PB Carrier Core`| 1,650 | 4,200 | `Update10` | 0 B / tick |
| `Yard.Extruder`| `[EXT] PB Yard Master` | 820 | 2,100 | `Update10` | 0 B / tick |
| `Excavator.Mastodon`| `[MAS] PB Excavator`| 2,100 | 4,800 | `Update1` | 0 B / tick |
| `Train.Caterpillar`| `[CAT] PB LandTrain` | 740 | 1,850 | `Update10` | 0 B / tick |
| `Drone.Remora` | `[REM] PB Miner` | 950 | 2,400 | `Update10` | 0 B / tick |
| `Drone.Mule` | `[MUL] PB Shuttle` | 880 | 2,150 | `Update10` | 0 B / tick |
| `Drone.Kestrel`| `[KES] PB Scout` | 710 | 1,650 | `Update10` | 0 B / tick |
| `Drone.Albatross`| `[ALB] PB Utility` | 790 | 1,800 | `Update10` | 0 B / tick |

---

### 9.8 · Acceptance Testing Protocols (Suites A-F)

* [ ] **Suite A (Zero-GC Loop Verification):** Execute 10,000 consecutive ticks across all 11 PB scripts in the test harness; verify garbage collector generation 0, 1, and 2 collection counts remain exactly 0.
* [ ] **Suite B (Multi-Grid JIT Logistics Closed Loop):** Ore mined by Remora triggers dM/dt alert; Orca calls Mule; Mule loads and lands at Ground Base; Ground Base refineries queue components; Extruder consumes plates. Total loop completes without manual intervention.
* [ ] **Suite C (Seamless C2 Root Handover):** Ground Base initiates `TMS.C2.HANDOVER`; Mastodon assumes fleet command; Base powers down without losing drone telemetry or tasking state.
* [ ] **Suite D (Rule C4 Total PB Crash Recovery):** Force-crash all PBs simultaneously via `/stop`; verify native Event Controllers and Timer blocks prevent grid descent, engage emergency braking, and isolate damaged sections.
* [ ] **Suite E (Minifier Integrity Proof):** Compile entire solution under `minify=full`; deploy to local script folder; verify all CustomData parsing, IGC routing, and terminal queries operate identically to unminified build.
* [ ] **Suite F (Broadcast Bandwidth & Latency):** Transmit 50 concurrent IGC messages across 5 separate channels; verify zero packet corruption and latency < 100 ms.
