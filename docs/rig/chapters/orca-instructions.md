# Chapter 1 · ORCA-01 Heavy Mining Carrier (ORC-01)
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
