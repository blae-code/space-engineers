# Space Engineers 1 · Floating Rig & Logistics Fleet
## Master Lego-Style Build Manual & RIG//OS Interface Contract
### Version 1.0 · Canon Engineering Specification · Earth 1 g Baseline

Last updated: 2026-10-18
Authority: Blae Decisions 1-64 (Canon) · `CLAUDE(RIG).md` · `Part 1 & 2 Handoff`

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

Source: `docs/rig/chapters/orca-instructions.md` · 17 × 19 × 7 B · Uncrewed Atmo Drone Carrier

### 0 · Reference grid

The Orca sits on a **17 × 19 × 7 B** bounding box (42.5 × 47.5 × 17.5 m).

```
Axes:  X = beam (port −, stbd +)    Y = up (L1 belly, L7 roof)    Z = length (bow +)

Rows:   r0 (bow) ─────────────────────────────────── r18 (stern)
Levels: L1 (belly) ─── L2 ─── L3 ─── L4 ─── L5 ─── L6 ─── L7 (roof)
X:      −8 (port tip) ──── −1.5 (port spine wall) ── 0 ── +1.5 (stbd spine wall) ──── +8 (stbd tip)
```

#### Master plan view (roof, Y = L7)

```
        X:  −8    −5    −2  −1.5  0  +1.5  +2    +5    +8
            ·     ·     ·    ║    ·    ║    ·     ·     ·
    r0  ────┬─────┬─────┬════╬════╬════┬─────┬─────┬────  BOW
            │     │     │    ║ CC ║    │     │     │
    r2  ····│·····│·····│····║····║····│·····│·····│····
            │     │     │    ║    ║    │     │     │
    r3  ────┼─────┼─────┼════╬════╬════┼─────┼─────┼────
            │     │     │    ║    ║    │     │     │
            │  P1 │     │    ║ PC ║    │     │ S1  │
    r6  ────┼─────┼─────┼════╬════╬════┼─────┼─────┼────
            │     │     │    ║    ║    │     │     │
            │  P2 │     │    ║    ║    │     │ S2  │
            │     │     │    ║ HD ║    │     │     │
    r12 ────┼─────┼─────┼════╬════╬════┼─────┼─────┼────
            │     │     │    ║    ║    │     │     │
            │  P3 │     │    ║    ║    │     │ S3  │
    r15 ····│·····│·····│····║····║····│·····│·····│····
            │     │     │    ║    ║    │     │     │
    r16 ────┼─────┼─────┼════╬════╬════┼─────┼─────┼────
            │     │     │    ║ AS ║    │     │     │
    r18 ────┴─────┴─────┴════╬════╬════┴─────┴─────┴────  STERN
                             ·    ·

        ║═══║  spine (3 B wide)
        │   │  bay frame (outer frame at X ±8)
        ·   ·  cross frames at r0, r6, r12, r18
```

#### Master side elevation (X = 0, looking from stbd)

```
        r0    r3      r6      r9      r12     r15   r18
        ┌─────┬───────┬───────┬───────┬───────┬─────┐  L7 roof
        │ CC  │  PC   │       │  MP   │       │ AS  │
        │     │       │       │ [CON] │       │     │
        │     │       │       │       │       │     │
        │     │  R R  │  ┌────┼────┐  │       │     │  L4
        │     │  R R  │  │ HOLD    │  │       │ H2  │
        │     │  R R  │  └────┼────┘  │       │     │  L2
        │ RC  │ BAT B │       │       │       │     │
        └─────┴───────┴───────┴───────┴───────┴─────┘  L1 belly
                                                        ▼
        OR-1          OR-2                        OR-3
        [A]           [H2]                        [A]
        gear                                      gear
```

#### Master cross-section (at r9, looking toward bow)

```
                  port                          stbd
        X: −8         −5    −1.5  0  +1.5    +5         +8
           ┌───────────┬──────╥────┬────╥──────┬───────────┐  L7 roof
           │           │      ║    │    ║      │           │
           │  bay P2   │      ║ trunk  ║      │  bay S2   │
           │  (5×5 B   │ inner║  3 B   ║inner │  (5×5 B   │
           │   clear)  │ frame║  wide  ║frame │   clear)  │
           │           │      ║    │    ║      │           │
           │     ↓CON  │      ║  HOLD  ║      │  CON↓     │
           │           │      ║    │    ║      │           │
           └───────────┴──────╨────┴────╨──────┴───────────┘  L1 belly
                  ↕ open                  ↕ open
                 to ground              to ground
```

---


#### 0.4 · Master Conveyor Routing Schematic

The Orca features three isolated conveyor circuits:
1. **Mining Loop:** Remora drone bays (P1-P3, S1-S3) ──► `ORC HD Gate` (whitelist: ore, stone) ──► `ORC Ore Hold` ──► `ORC HD Drain` (whitelist: ore) ──► `ORC MP Conn` (Mule Pad).
2. **Service & Fuel Loop:** `ORC MP Intake` (whitelist: U, components, ice) ──► `ORC U Store` (L2, r3) ──► 6× Small Reactors & H2 Tanks.
3. **Emergency H2 Assist:** 2× Large H2 Tanks ──► 4× Large H2 Thrusters (outriggers).

```
                        DRONE BAY CEILING CONNECTORS
        Bay P1 [CON]      Bay P2 [CON]      Bay P3 [CON]
             │                 │                 │
             └────────►┬───────┴────────◄────────┘
                       │ Port Collection Trunk (L6, X −5)
                       ▼
       ┌───────────────────────────────┐
       │ Bay Sorter Cross-Feed (r6, L5)│
       └───────────────┬───────────────┘
                       │
       ┌───────────────▼──────────────────────────────────────────────┐
       │ `ORC HD Gate` (Conveyor Sorter · Whitelist: Ore, Stone)      │
       └───────────────────────────────┬──────────────────────────────┘
                                       │
       ┌───────────────────────────────▼──────────────────────────────┐
       │ `ORC Ore Hold` (Large Cargo 3×3×3 B · r10-r12, X0, L2-L4)    │
       └───────────────────────────────┬──────────────────────────────┘
                                       │
       ┌───────────────────────────────▼──────────────────────────────┐
       │ `ORC HD Drain` (Conveyor Sorter · Whitelist: Ore · Drain ON) │
       └───────────────────────────────┬──────────────────────────────┘
                                       │
       ┌───────────────────────────────▼──────────────────────────────┐
       │ `ORC MP Conn` (Mule Pad Connector · Flush L7, r11, X0)       │
       └──────────────────────────────────────────────────────────────┘
                                       ▲
                                       │
       ┌───────────────────────────────┴──────────────────────────────┐
       │ `ORC MP Intake` (Conveyor Sorter · Whitelist: U, Components) │
       └───────────────────────────────┬──────────────────────────────┘
                                       │
       ┌───────────────────────────────▼──────────────────────────────┐
       │ `ORC U Store` (Small Cargo Container · r3, X0, L2)           │
       └───────────────────────────────┬──────────────────────────────┘
                                       │
                   ┌───────────────────┴───────────────────┐
                   ▼                                       ▼
       [6× Small Nuclear Reactors]            [2× Large Hydrogen Tanks]
```

---

#### 0.5 · Master Electrical Single-Line Schematic

```
                       POWER SOURCES (90 MW Nuclear + 6 MWh Battery)
         ┌───────────────────┬───────────────────┬───────────────────┐
         │ Small Reactor 1..3│ Small Reactor 4..6│ Large Battery 1..2│
         │ (45 MW Nominal)   │ (45 MW Nominal)   │ (48 MW Peak Burst)│
         └─────────┬─────────┴─────────┬─────────┴─────────┬─────────┘
                   │                   │                   │
                   └───────────────────┼───────────────────┘
                                       ▼
                              PRIMARY POWER BUS
        ┌──────────────────────────────┬──────────────────────────────┐
        │                              │                              │
        ▼                              ▼                              ▼
  LIFT PROPULSION              MANEUVER & ATTITUDE             DRONE SUPPORT
  4× Large Atmo                28× Small Atmo                  6× Bay Recharging
  (67.2 MW @ 100% Max)         (18.2 MW @ 100% Max)            (24.0 MW Max Float)
  [Normal Hover = 52.8 MW]     [Attitude Gyros = 0.4 MW]       [Turrets & Lights = 0.5 MW]
```

---

#### 0.6 · Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Sizes | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Structural Frame** | Beam Blocks, Heavy Armor Plates, Structural 3×1×3 | 482 | 410 t | 1,450 |
| **Power Core** | 6× Small Nuclear Reactors, 2× Large Batteries | 8 | 42 t | 220 |
| **Lift Propulsion** | 4× Large Atmospheric Thrusters (3×3×5 B) | 4 | 132 t | 400 |
| **Aux Thrust & H2** | 4× Large H2 Thrusters, 28× Small Atmo, 2× H2 Tanks | 34 | 86 t | 850 |
| **Cargo & Conveyors** | 1× Large Cargo, 1× Small Cargo, 3× Sorters, Pipes | 58 | 48 t | 290 |
| **Drone Bays** | 6× Connectors, 12× Hinges, 12× Mag Plates, 2× Turrets | 32 | 34 t | 980 |
| **Avionics & Logic** | 1× RC, 1× PB, 1× Antenna, 4× Timers, 4× ECs, 8× Cams | 19 | 12 t | 310 |
| **TOTAL (DRY)** | Complete Airframe (ORCA-01) | **637** | **≈ 764 t** | **4,500** |

---


#### 0.7 · Human Builder Guide · Visual Paint Palette & Spatial HUD

Building a 17 × 19 × 7 B large-grid skeleton in Space Engineers survival or creative easily causes spatial disorientation. To eliminate confusion, use this color-coding palette with the in-game Paint Tool (`[P]` key) during construction:

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ SE CONSTRUCTION PAINT PALETTE (Recommended Paint System)                            │
├─────────────────────┬──────────────┬───────────────┬─────────────────────────────────┤
│ Functional Role     │ Paint Finish │ In-Game Color │ Purpose / Visual Cue            │
├─────────────────────┼──────────────┼───────────────┼─────────────────────────────────┤
│ Master Spine Rails  │ Battered     │ Charcoal Gray │ Identifies X ±1.5 structural backbone
│ Conveyor Pipes/Hubs │ Clean        │ Safety Yellow │ Instant visual tracing of fuel/ore runs
│ Drone Bay Jaws/Clamps│ Hazard      │ Orange Stripe │ Highlights moving mechanical pinch points
│ Mule Pad Blast Deck │ Carbon/Worn  │ Matte Black   │ Outlines 7×7 B landing target from above
│ Mule Pad Perimeter  │ Battered     │ Safety Red    │ 1 B No-Solar thruster flame hazard boundary
│ Power Core Shielding│ Heavy Armor  │ Hazard White  │ Marks radioactive isolation vault
│ Flight Control Core │ Heavy Armor  │ Fleet Blue    │ Identifies armored bridge & avionics
└─────────────────────┴──────────────┴───────────────┴─────────────────────────────────┘
```

##### Universal Spatial Locator HUD (Included in Every Step)
Every build step begins with a standardized spatial banner:
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: <Name>  ·  Grid Zone: <Rows r0-r18>  ·  Level: <L1-L7>         │
│ [VIEWPORT] Looking: <Direction>  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

### Phase 1 · Spine and roof deck

#### Step 1 · Belly frame rails (L1)

Build the spine belly as three Z-axis rails at L1.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 1 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Beam Block Straight │ 20  │ 16.0 t / 20   │ Z-axis rails (r0-r18) & X cross-ties     │
│ Beam Block Junction │ 16  │ 12.8 t / 16   │ Corner and post tie nodes (X ±1.5)       │
│ Magnetic Plate      │ 2   │  4.8 t / 50   │ Face DOWN at X0, r3 and r15 (flush L1)   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘


```
Plan view, Y = L1 (belly)

    X:  −1.5    0    +1.5
         ■──────■──────■   r0 (Bow)
         │      │      │
         ■──[ANCHOR 1]─■   r3 (under PC)
         │      │      │
         ■      ·      ■   r6
         │      │      │
         ■      ·      ■   r9
         │      │      │
         ■      ·      ■   r12
         │      │      │
         ■──[ANCHOR 2]─■   r15 (under AS)
         │      │      │
         ■──────■──────■   r18 (Stern)
         └──────┴──────┘

    ■ = Beam Block (junction / corner)
    │ = Beam Block (straight, Z-axis)
    ─ = Beam Block (straight, X-axis tie beams)
    [ANCHOR 1, 2] = Magnetic Plate (facing down at X0, r3 and r15, flush at L1)
```

**Blocks:**
- 14 × Beam Block (straight, Z-axis) · 2 rails × 7 spans
- 6 × Beam Block (straight, X-axis) · cross ties at r0, r3, r15, r18
- 16 × Beam Block (junction or corner) · 8 points × 2 rails
- 2 × Magnetic Plate (facing down) · `ORC Belly Anchor Bow` (r3) & `ORC Belly Anchor Stern` (r15) → `ORC Ventral Anchors`

**✓ Check:** two parallel rails, 3 B apart, running 19 B from r0 to r18 at level L1. Two magnetic plates face down at r3 and r15, flush with the belly line to anchor to planet voxels.

---

#### Step 2 · Spine posts (L1 → L7)

At each junction from Step 1, build a vertical post up to L7 (roof). Posts stand at X = ±1.5, at rows r0, r3, r6, r9, r12, r15, r18.

```
Side elevation, X = −1.5 (port spine wall)

    r0    r3    r6    r9    r12   r15   r18
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L7
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L6
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L5
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L4
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L3
    ┃     ┃     ┃     ┃     ┃     ┃     ┃   L2
    ■     ■     ■     ■     ■     ■     ■   L1 (from step 1)

    ┃ = Beam Block (straight, Y-axis)
    ■ = existing junction
```

**Blocks:**
- 84 × Beam Block (straight) · 14 posts × 6 levels each (L2-L7)

**✓ Check:** 14 vertical posts, 7 B tall. The grid stands on its own.

---

#### Step 3 · Spine roof beams (L7)

Connect the post tops with Z-axis beams at L7, forming the roof spine. Also add the centre-line beams at L7 (X = 0) for the ore trunk.

```
Plan view, Y = L7 (roof)

    X:  −1.5    0    +1.5
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

    ■ = Beam Block (junction)
    ║ = Beam Block (straight)
```

**Blocks:**
- 21 × Beam Block (straight) · 3 rails × 7 spans
- 7 × Beam Block (junction) · centre-line junctions at r0, r3, r6, r9, r12, r15, r18

**✓ Check:** the spine is now a 3 × 19 × 7 B skeletal frame. You can walk along the roof.

---

#### Step 4 · Cross-frames at r0, r6, r12, r18 (L1 and L7)

Extend the belly and roof cross-beams from the spine out to X = ±8 (the outer frame line). These form the bay row dividers.

```
Plan view, Y = L1 (belly), showing cross-frames

    X: −8              −1.5  0  +1.5              +8
        ■════════════════■════■════■════════════════■  r0
        ·                ║    ·    ║                ·
        ·                ║    ·    ║                ·
        ·                ║    ·    ║                ·
        ■════════════════■════■════■════════════════■  r6
        ·                ║    ·    ║                ·
        ·                ║    ·    ║                ·
        ·                ║    ·    ║                ·
        ■════════════════■════■════■════════════════■  r12
        ·                ║    ·    ║                ·
        ·                ║    ·    ║                ·
        ·                ║    ·    ║                ·
        ■════════════════■════■════■════════════════■  r18

    New blocks: the ═══ runs from spine wall (±1.5) to outer edge (±8)
    = 6.5 B per side × 2 sides × 4 rows = 8 beams × 4 rows
```

**Blocks:**
- 52 × Beam Block (straight, X-axis) · belly + roof, 4 rows, both sides
- 16 × Beam Block (junction) · end-points at X = ±8, L1 and L7

**Repeat at L7** (roof level) with the same layout.

**✓ Check:** 4 cross-frames divide the hull into 3 bay zones per side. The outer corners at X = ±8 are where the outrigger arms will root.

---

#### Step 5 · Outer frame rails (L1 → L7 at X = ±8)

Connect the cross-frame end-points with Z-axis rails and vertical posts at X = ±8.

```
Side elevation, X = −8 (port outer frame)

    r0    r6    r12   r18
    ┌─────┬─────┬─────┐  L7
    │     │     │     │
    │     │     │     │
    │     │     │     │
    │     │     │     │
    │     │     │     │
    └─────┴─────┴─────┘  L1

    │ = Beam Block (Y-axis posts)
    ─ = Beam Block (Z-axis rails at L1 and L7)
```

**Blocks (per side):**
- 3 × Beam Block (Z-axis, L1) · r0→r6, r6→r12, r12→r18
- 3 × Beam Block (Z-axis, L7) · same
- 4 × 6 = 24 Beam Block (Y-axis) · posts L1→L7 at r0, r6, r12, r18

**Both sides total:** 60 Beam Blocks

**✓ Check:** the hull is now a rectangular skeleton, 17 × 19 × 7 B. Bays are open at the bottom on both sides.

---

#### Step 6 · Inner frame rails (X = ±2, L1 → L7)

The inner frame runs between the spine wall and the bay, separating the bay volume from the utility corridor.

```
Cross-section at r9, looking toward bow

    X: −8    −5    −2  −1.5  0  +1.5  +2    +5    +8
       ┃           ┃    ║         ║    ┃           ┃  L7
       ┃           ┃    ║         ║    ┃           ┃
       ┃    bay    ┃    ║  spine  ║    ┃    bay    ┃
       ┃           ┃    ║         ║    ┃           ┃
       ┃           ┃    ║         ║    ┃           ┃
       ┃           ┃    ║         ║    ┃           ┃
       ┃           ┃    ║         ║    ┃           ┃  L1

    New: ┃ at X = ±2 (inner frame posts and rails)
```

**Blocks (per side):**
- 7 × Beam Block (Y-axis posts) · at r0, r3, r6, r9, r12, r15, r18
- Per post: 6 blocks (L1→L7) = 42 posts
- 6 × Beam Block (Z-axis rails, L1) · r0→r3, r3→r6, ... r15→r18
- 6 × Beam Block (Z-axis rails, L7) · same

**Both sides total:** 108 Beam Blocks

**✓ Check:** looking down through a bay opening, you see the 5 B wide clear lane between the inner frame (X ±2) and the outer frame (X ±8), closed on top at L7, open below at L1.

---

#### Step 7 · Bay mid-height rings (L4, at each bay)

Add a horizontal ring at L4 in each bay frame for structural stiffness and turret/sensor mounting.

```
Cross-section at r3 (bay P1 / S1 centre), Y = L4

    X: −8         −2         +2         +8
       ■═══════════■          ■═══════════■  L4 ring
       ║           ║          ║           ║
       ║   bay P1  ║          ║   bay S1  ║
       ║   5×5 B   ║          ║   5×5 B   ║
       ║  clear    ║          ║  clear    ║
       ■═══════════■          ■═══════════■  L1
```

**Blocks per bay:** 4 × Beam Block (horizontal ring, X-axis at L4, fore and aft walls)

**6 bays total:** 24 Beam Blocks

**✓ Check:** each bay has a mid-height shelf for mounting sensors, lights, and repair turrets.

---

#### Step 8 · Roof deck (L7, Structural Frame 3×1×3)

Cover the entire roof from r0 to r18, X = −8 to +8 with Structural Frame 3×1×3 panels, resting on the L7 beams.

```
Plan view, Y = L7 (roof)

    X: −8                     0                     +8
       ┌─────────────────────────────────────────────┐  r0
       │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │
       │  ░░░░░░░░░░░  Structural   ░░░░░░░░░░░░░  │
       │  ░░░░░░░░░░░  Frame 3×1×3  ░░░░░░░░░░░░░  │  r0-r7
       │               ┌───────────┐                 │
       │  ░░░░░░░░░░░  │███████████│ ░░░░░░░░░░░░░  │  r8-r14
       │  ░░░░░░░░░░░  │MP BLAST   │ ░░░░░░░░░░░░░  │  (7×7 B Heavy Armour)
       │  ░░░░░░░░░░░  │DECK (L7)  │ ░░░░░░░░░░░░░  │
       │               └───────────┘                 │
       │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │  r15-r18
       └─────────────────────────────────────────────┘

    ░░ = Structural Frame 3×1×3 (lightweight roof deck)
    ██ = Heavy Armour Blocks (7×7 B blast deck at r8-r14, flush at L7)

    Leave openings for:
     · Antenna mast (X0, above r1)
     · MP connector (X0, r11)
     · Bay connectors (X ±5, r3 / r9 / r15) · trunk branches drop through
```

**Blocks:**
- ≈ 30 × Structural Frame 3×1×3† (general roof deck)
- 49 × Heavy Armour Block (7×7 B blast floor at r8-r14, X −3 to +3)

**✓ Check:** the roof is walkable. The MP zone (r8-r14) has a solid Heavy Armour floor flush with the surrounding frame panels. Openings exist at every connector and antenna position.

---

#### Step 9 · Ore trunk (L7, centre line)

Run the ore conveyor trunk along X = 0, L7 (just under the roof deck), from r3 to r15. Add branches at each bay centre.

```
Plan view, Y = L7 (under roof deck), ore trunk only

    X: −5              0              +5
                       ║
        ·──────────────╫──────────────·  r3  (P1/S1 branch)
                       ║
                       ║
                       ║
        ·──────────────╫──────────────·  r9  (P2/S2 branch)
                       ║
                       ║
                       ║
        ·──────────────╫──────────────·  r15 (P3/S3 branch)
                       ║

    ║ = Large conveyor tube / junction (Z-axis)
    · = Large conveyor tube / junction (X-axis branch to bay connector)
    ╫ = Large conveyor junction (T or cross)
```

**Blocks:**
- 12 × Large conveyor tube (Z-axis, r3→r15)
- 6 × Large conveyor tube (X-axis branches, ≈3 B each side)
- 3 × Large conveyor junction (T-pieces at r3, r9, r15)

**✓ Check:** a continuous conveyor path runs from every bay connector position down to the hold inlet position at r10. This is the **ore network only**; the service network is separate.

---

#### Step 10 · Power core (PC, r3-r6)

Fill the spine between r3 and r6 with the power core as a 1 B wide **Inline Stack at X = 0**, flanked by Heavy Armour side walls at X = ±1 isolating the nuclear core from the drone bays.

```
Cross-section at r4, looking toward bow

    X: −1 (port wall)     X = 0 (center rack)     X = +1 (stbd wall)
       ╔═════════════════╤═════════════════════╤═════════════════╗  L7 (under roof deck)
       ║  Heavy Armour   │  Service Conveyor   │  Heavy Armour   ║  L6
       ║  Heavy Armour   │  Small Reactor 2    │  Heavy Armour   ║  L5
       ║  Heavy Armour   │  Small Reactor 6    │  Heavy Armour   ║  L4
       ║  Heavy Armour   │  Large Battery 2    │  Heavy Armour   ║  L3
       ║  Heavy Armour   │  Gyroscope 2        │  Heavy Armour   ║  L2
       ╚═════════════════╧═════════════════════╧═════════════════╝  L1 (belly)
```

```
Side elevation at X = 0 (Centerline equipment rack)

          r3              r4              r5              r6
    ┌───────────────┬───────────────┬───────────────┬───────────────┐  L7 roof
    │ Serv Conveyor │ Serv Conveyor │ Serv Conveyor │ Serv Conveyor │  L6 (Service spine)
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │ Small Reac 1  │ Small Reac 2  │ Small Reac 3  │ Small Reac 4  │  L5
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │ Small Reac 5  │ Small Reac 6  │  ORC U Store  │ Serv Conveyor │  L4
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │ Large Batt 1  │ Large Batt 2  │  Gyroscope 1  │  Gyroscope 2  │  L3
    ├───────────────┼───────────────┼───────────────┼───────────────┤
    │  Gyroscope 3  │  Gyroscope 4  │ Serv Conveyor │ Serv Conveyor │  L2
    └───────────────┴───────────────┴───────────────┴───────────────┘  L1 belly

    Conveyor plumbing:
    - Service line runs along L6 (Z-axis).
    - Reactors 1-4 connect directly to L6 conveyor.
    - Reactors 5-6 connect to Reactors 1-2 and the `ORC U Store` at r5·L4.
    - `ORC U Store` auto-feeds fuel to all 6 reactors through the service line.
```

**Blocks:**

| Block | Qty | Name / group | Cell (row · level at X = 0) |
|-------|-----|-------------|-----------------------------|
| Small reactor | 6 | `ORC Reactors` | r3·L5, r4·L5, r5·L5, r6·L5, r3·L4, r4·L4 |
| Large battery | 2 | `ORC Batteries` (Recharge) | r3·L3, r4·L3 |
| Gyroscope (large) | 4 | `ORC Gyros` | r5·L3, r6·L3, r3·L2, r4·L2 |
| Small cargo container | 1 | `ORC U Store` | r5·L4 |
| Large conveyor junction/tube | 7 | Service line | r3-r6 at L6; r6·L4; r5-r6 at L2 |
| Heavy armour block | 48 | PC shell | Walls at X = ±1 (L2-L6), r3 & r6 bulkheads, L1 & L7 caps |

**✓ Check:** all 6 reactors, both batteries, and `ORC U Store` connect to the service conveyor. Heavy armour fully encloses the power vault. Batteries are set to Recharge. Gyros show in `ORC Gyros`.

---

#### Step 11 · Ore hold (HD, r7-r15)

Place the large cargo container in the spine at r10-r12, with the gate sorter above it and the drain sorter below.

```
Side elevation (X = 0), ore hold zone

    r7    r8    r9    r10   r11   r12   r13   r14   r15
    ║     ║     ║     ╔═════╤═════╗     ║     ║     ║   L7
    ║     ║     ║     ║     │     ║     ║     ║     ║
    ║     ║     ║     ║  CARGO    ║     ║     ║     ║   L4 centre
    ║     ║     ║     ║  CONTAINER║     ║     ║     ║
    ║     ║     ║     ╚═════╧═════╝     ║     ║     ║   L1
                      ↑ HD Gate         ↓ HD Drain
                      (from trunk)      (to MP)

    The large cargo container occupies 3 × 3 × 3 B
    centred at r11, X0, L3-L5.
```

**Blocks:**

| Block | Qty | Name |
|-------|-----|------|
| Large cargo container | 1 | `ORC Hold` |
| Conveyor sorter | 1 | `ORC HD Gate` (whitelist: ore; trunk → hold) |
| Conveyor sorter | 1 | `ORC HD Drain` (whitelist: ore; hold → MP) |
| Light armour | ~18 | HD shell |

**✓ Check:** ore path is continuous: trunk → Gate → Hold → Drain. Gate and Drain are both OFF at this stage (ECs wire them in Phase 7).

---

#### Step 12 · Aft service section (AS, r16-r18)

```
Side elevation (X = 0)

    r16   r17   r18
    ╔═════╤═════╗  L7
    ║     │     ║  L6
    ║  H2 TANK  ║  L5    ORC AS H2
    ║     │     ║  L4
    ║  PARTS    ║  L3    ORC Parts Store
    ║  STORE    ║  L2
    ╚═════╧═════╝  L1

    Light armour shell.
```

**Blocks:**

| Block | Qty | Name |
|-------|-----|------|
| Hydrogen tank | 1 | `ORC AS H2` |
| Small cargo container | 1 | `ORC Parts Store` |
| Light armour | ~18 | AS shell |

Connect the H2 tank to the service line (branches to OR-2P and OR-2S later).

**✓ Check:** H2 tank shows in terminal, service line reaches from PC through HD body to AS.

---

#### Step 13 · Mule pad (MP, roof, r8-r14) & Gantry Cradle

#### 2.5D Isometric Callout · Pad-Mounted Gantry Cradle (r8-r14, X ±3)

```
                       [Mule Hull 7×7 B]
                              │
    Port Flank                ▼               Starboard Flank
  [CLAMP RUB RAIL]                       [CLAMP RUB RAIL]
         ▲                                      ▲
         │                                      │
    [MAG PLATE] ◄── 30° Swing Inward ──► [MAG PLATE]
         │                                      │
    [MOTOR HINGE]                          [MOTOR HINGE]
    (Mounted X −3, r11)                    (Mounted X +3, r11)
  ═══════╧══════════════════════════════════════╧═══════
                    FLUSH HEAVY ARMOUR DECK (L7)
                           [ORC MP Conn]
```

#### AI Scripting & Hardware Contract Card · Mule Pad

| Device | Block Name | Group Membership | Action 1 (On True / Trigger) | Action 2 (On False) |
|---|---|---|---|---|
| **Event Controller** | `ORC EC MP` | `ORC Logic` | `ORC MP Cradle` Angle $
ightarrow$ 30° · `ORC MP Conn` Lock ON | `ORC MP Cradle` Angle $
ightarrow$ 0° · `ORC MP Conn` Lock OFF |
| **Approach Sensor** | `ORC Sensor MP` | `ORC Sensors` | `ORC MP Lights` $
ightarrow$ ON · Sound Chime | `ORC MP Lights` $
ightarrow$ OFF |
| **Ore Sorter** | `ORC HD Drain` | `ORC Sorters` | Drain All $
ightarrow$ ON (Pumps ore to Mule) | Drain All $
ightarrow$ OFF |
| **Service Sorter** | `ORC MP Intake` | `ORC Sorters` | Whitelist: Uranium, Components $
ightarrow$ Pull | Standby |


Build the standardized **Gantry Cradle** docking pad on the roof deck on top of the 7×7 B Heavy Armour blast floor.

```
Plan view, Y = L7 (roof), MP blast deck & Gantry Cradle

    X:  −3                  0                  +3
         ·                  ·                  ·    r8
                           PAD
         ·             ┌────▼────┐             ·    r9
                       │         │
    [CRADLE HINGE] ──► │  [CON]  │ ◄── [CRADLE HINGE] r11 (Motorized clamps)
    [MAG CLAMP]        │         │        [MAG CLAMP]
         ·             └────▲────┘             ·    r13
                           PAD
         ·                  ·                  ·    r14

    Floor = 49 × Heavy Armour Blocks (7 × 7 B, r8-r14, X −3 to +3), flush at L7
    [CON] = ORC MP Conn (connector, facing UP, at X0, r11)
    PAD   = Heavy armour foot target (4 total): X0·r8, X0·r14, X−3·r11, X+3·r11
    [CRADLE] = Twin motorized Hinges @ L7 with 1 B Beam Arm + Magnetic Plate (Auto-Lock)
               Outer port: X −3, r11 · Outer stbd: X +3, r11 → ORC MP Cradle
    ▼ ▲   = Below deck at L6: ORC HD Drain (hold → Mule) and ORC MP Intake (Mule → service line)
```

Below the connector, on a short riser at level L6:
- `ORC HD Drain` sorter → sends ore from hold to the Mule
- `ORC MP Intake` sorter → pulls uranium + components from the Mule into service line

**Blocks:**

| Block | Qty | Name / group | Purpose |
|-------|-----|-------------|---------|
| Heavy Armour Block | 49 | MP blast deck (placed in Step 8) | Blast & touchdown plate |
| Connector (facing up) | 1 | `ORC MP Conn` | Ore & power transfer |
| Hinge | 2 | `ORC MP Cradle Port` / `Stbd` → `ORC MP Cradle` | Clamps Mule chassis |
| Magnetic plate | 2 | `ORC MP Cradle Clamp Port` / `Stbd` | Rigid latch on Mule waist |
| Conveyor sorter | 1 | `ORC MP Intake` (whitelist: U ingot + components) | Service replenishment |
| Conveyor sorter | 1 | `ORC HD Drain` (whitelist: ore) | Ore drain to Mule |
| Camera | 1 | `ORC Cam MP` (looking aft over the pad) | Alignment visual |

**Automated EC Docking Sequence (wired in Step 26):**
1. Approach: Mule descends over pad $\rightarrow$ `ORC Sensor MP` detects Mule at 2 m $\rightarrow$ `ORC MP Lights` ON.
2. Clamp: Cradle hinges rotate 30° inward $\rightarrow$ Magnetic plates lock to Mule frame $\rightarrow$ `ORC MP Conn` locks.
3. Drain: `ORC HD Drain` ON $\rightarrow$ transfers 650 t ore.
4. Release: Hold empty $\rightarrow$ Cradle clamps release $\rightarrow$ Hinges swing 0° flush $\rightarrow$ Mule departs.

**✓ Check:** Gantry Cradle arms swing inward to clamp a test grid over `ORC MP Conn`. Magnetic plates lock firmly. Deck withstands thruster wash.

---

#### Step 14 · Solar array (roof)

Place large solar panels on the roof deck outside the MP zone, leaving a **1 B clear perimeter buffer** around the Mule Pad to prevent thruster scorch.

```
Plan view, Y = L7 (roof)

    X: −8            −4            0            +4            +8
       ┌─────────────────────────────────────────────────────────┐  r0
       │  ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀         ANT         ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀  │
       │  ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀  │  r0-r6
       │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │  r7 (1 B buffer)
       │  ☀ ☀ ☀ ☀ ░░░░  ┌─────────────────┐  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │
       │  ☀ ☀ ☀ ☀ ░░░░  │  MP BLAST DECK │  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │  r8-r14
       │  ☀ ☀ ☀ ☀ ░░░░  │  (Heavy Armour)│  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │  (X±4 buffer)
       │  ☀ ☀ ☀ ☀ ░░░░  └─────────────────┘  ░░░░ ☀ ☀ ☀ ☀ ☀ ☀  │
       │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │  r15 (1 B buffer)
       │  ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀ ☀  │  r16-r18
       └─────────────────────────────────────────────────────────┘

    ☀ = Large solar panel (facing up)
    ░░ = 1 B clear buffer (Structural Frame only, no solar panels within Mule flame envelope)
    Keep solar only if total array mass ≤ 20 t (commissioning test)
```

**Blocks:**
- ≈ 24 × Large solar panel† → `ORC Solar`

**✓ Check:** solar panels are flat on the roof, facing up. No solar panels sit within 1 B of the MP Heavy Armour deck perimeter. Record array mass at commissioning.

---

### Phase 1 checkpoint

At this point you have:

```
Side view of the complete spine + roof

    OR-1                                              OR-3
    (next)                                            (next)
      ↓                                                ↓
      ·    r0    r3    r6    r9    r12   r15   r18     ·
           ┌═════╤═════╤═════╤═════╤═════╤═════┐ L7  roof + solar
           │ CC  │█████│     │ MP  │     │ AS  │
           │(emp)│ PC  │     │[CON]│     │[H2] │
           │     │█████│     │     │     │[PRT]│
           │     │ R×6 │  ┌──┼──┐  │     │     │
           │     │BAT×2│  │HOLD │  │     │     │
           │     │GYR×4│  └──┼──┘  │     │     │
           └═════╧═════╧═════╧═════╧═════╧═════┘ L1  belly

    █ = heavy armour
    Everything else = beam skeleton + light armour
    Bays are framed but empty (connectors, turrets in Phase 3)
    Ore trunk runs along the roof, branches at r3, r9, r15
    Service line runs through the spine body
```

**Phase 1 is complete when:**
- [ ] Spine skeleton stands (beam frame 3 × 19 × 7 B)
- [ ] Bay frames stand (outer frame at X ±8, inner at X ±2, cross-frames at r0, r6, r12, r18)
- [ ] Roof deck covers the hull
- [ ] Ore trunk runs from r3 to r15 with 6 branches
- [ ] PC: 6 reactors, 2 batteries (Recharge), 4 gyros, U Store
- [ ] HD: hold with Gate and Drain sorters
- [ ] AS: H2 tank, Parts Store
- [ ] MP: connector, 4 pads, camera
- [ ] Service line runs PC → AS, separate from ore network
- [ ] Solar array on roof (mass recorded)

---

### Phase 2 · Control core (CC)

#### Step 15 · CC heavy armour shell (r0-r2)

Enclose the bow 3 rows of the spine in heavy armour.

```
Plan view, Y = L4 (mid-height), CC

    X: −1.5    0    +1.5
        ╔══════╤══════╗  r0  (bow face, heavy armour)
        ║      │      ║
        ║      │      ║  r1
        ║      │      ║
        ║      │      ║  r2
        ╚══════╧══════╝

    ╔═╗ = Heavy armour walls (all 4 sides + top + bottom)
    Interior: 1 × 2 × 6 B (1 column wide, 2 rows deep, 6 levels tall)
```

**Blocks:**
- ~36 × Heavy armour block · shell around r0-r2

**✓ Check:** CC is a sealed armoured box with access from the r2 (aft) face or the roof.

---

#### Step 16 · CC control blocks

Install all control blocks in the centre column (X = 0), rows r1-r2.

```
Cell layout, X = 0 (looking from stbd)

    r1         r2
    ┌──────────┬──────────┐  L6
    │          │          │
    ├──────────┼──────────┤  L5
    │ TMR      │ TMR      │  Heartbeat · Home
    │ Heartbt  │ Home     │
    ├──────────┼──────────┤  L4
    │ PB       │ EC       │  ORC CC PB · ORC EC Cargo
    │          │ Cargo    │
    ├──────────┼──────────┤  L3
    │ RC       │ RC       │  Main · Spare
    │ Main     │ Spare    │  (both facing +Z)
    ├──────────┼──────────┤  L2
    │ EC       │ EC       │  Lift · Power
    │ Lift     │ Power    │
    └──────────┴──────────┘  L1

    All blocks named per Part 2 §1
```

Also install at r1, L6: `ORC EC MP` (mule pad event controller)

**Blocks:**

| Block | Qty | Name | Cell |
|-------|-----|------|------|
| Remote Control | 2 | `ORC CC RC Main` · `ORC CC RC Spare` | r1·L3 · r2·L3 |
| Programmable Block | 1 | `ORC CC PB` | r1·L4 |
| Timer Block | 2 | `ORC TMR Heartbeat` · `ORC TMR Home` | r1·L5 · r2·L5 |
| Event Controller | 4 | `ORC EC Lift` · `Power` · `Cargo` · `MP` | r1·L2 · r2·L2 · r2·L4 · r1·L6 |
| Camera | 1 | `ORC Cam Bow` | r0·L4, in bow face |

**Settings:**
- Both RCs: forward = +Z, up = +Y. Main = main controller. Waypoint `HOME` = base GPS. Dampeners ON.
- Batteries (from Step 10): set to Recharge.

**✓ Check:** all 10 control blocks show in terminal with correct names. Both RCs face the bow (+Z). `ORC Cam Bow` sees forward.

---

#### Step 17 · Antenna mast

```
    Side view at X = 0

         ╤  antenna
         │  mast (1-2 B above roof)
    ═════╪═════  L7 roof deck
         │
         ║  CC interior
```

**Blocks:**

| Block | Qty | Name |
|-------|-----|------|
| Antenna | 1 | `ORC CC Antenna` (on roof above r1, X0) |
| Camera | 1 | `ORC Cam Mast` (on mast head, facing aft) |

**✓ Check:** antenna broadcasts. `ORC Cam Mast` shows a whole-roof overview looking aft.

---

### Phase 2 checkpoint

- [ ] CC armoured shell complete
- [ ] All 10 control blocks placed, named, in correct cells
- [ ] Both RCs face +Z with `HOME` waypoint
- [ ] Antenna mast through roof, broadcasting
- [ ] Bow and mast cameras work

---

### Phase 3 · Bay rows (BR-P, then mirror to BR-S)

#### Step 18 · Bay ceiling connectors (port side)

At each bay centre (X = −5, at r3, r9, r15), place a connector facing **down** on the ore trunk branch.

```
Cross-section at r3 (bay P1), looking toward bow

    X: −8         −5         −2   −1.5
       ┃           ┃          ┃    ║   L7
       ┃          [CON↓]      ┃    ║   ceiling connector
       ┃           │          ┃    ║   (ore trunk branch)
       ┃     P1    │    util  ┃    ║
       ┃    clear  │   corr.  ┃    ║
       ┃     5B    │          ┃    ║
       ┃           ┃          ┃    ║   L1
              open below

    [CON↓] = ORC BR-P1 Conn (connector facing down)
    Branch from the ore trunk (X = 0) runs to X = −5
```

**Blocks per bay:**

| Block | Qty | Name |
|-------|-----|------|
| Connector (facing down) | 1 | `ORC BR-P1 Conn` / `P2` / `P3` |

All 3 → group `ORC Bay Conns`

**Role allocation (Blae 2026-10-17):**
- **P1, S1, P3, S3:** Remora vertical drillers (primary ore extraction).
- **P2:** Kestrel (scout / deposit prospector).
- **S2:** Albatross (utility / survey craft).

**✓ Check:** each connector is centred in its bay ceiling, facing straight down, and connected to the ore trunk. P2/S2 connectors also have service line access.

---

#### Step 19 · Central bay repair turret & L4 gallery pipe (port side)

Install **1 central repair turret** at bay P2 (r9), mounted on the inner wall (X = −2) at L4 (mid-height ring), facing outboard (−X). Its range covers all three port bays (P1, P2, P3).

Run the service conveyor line horizontally along X = −2 at level L4 from r3 to r17, connecting the turret directly to `ORC Parts Store` in AS (r17).

```
Cross-section at r9 (bay P2), Y = L4

    X: −8 (outer frame)   X = −5 (P2 centre)   X = −2 (inner frame)
       ┃                                        ┃
       ┃                   bay P2          [RPR]■═══ [L4 Service Gallery Pipe]
       ┃                   (clear)              ┃    (connects r3 ↔ r9 ↔ r17)
       ┃                                        ┃

    [RPR] = Laser Multitool turret (mod), mounted at X −2, r9, L4
```

**Blocks (port side):**

| Block | Qty | Name / group | Cell |
|-------|-----|-------------|------|
| Laser multitool turret (mod) | 1 | `ORC Repair BR-P2` → `ORC Repair` | X −2, r9, L4 |
| Large conveyor tube (Z-axis) | 12 | Service gallery pipe | X −2, L4 (r3→r17) |
| Large conveyor junction | 1 | Turret tap | X −2, r9, L4 |

**Settings:** weld only, own grids, never grind†. Connected to service line (not ore trunk).

**✓ Check:** turret is plumbed to `ORC Parts Store`. From its central mount at r9, its laser beam reaches docked craft in P1 (r3), P2 (r9), and P3 (r15).

---

#### Step 19b · Folding Mechanical Guide Jaws (port side)

Install twin motorized guide jaws at L5 to physically clamp and center the ascending Remora drone.

```
Plan view at L5 (Bay P1 Guide Jaws in action)

 X: −8 (outer frame)            X = −5 (bay centre)            X = −2 (inner frame)
    ┃                                                          ┃
    [HINGE]                                                    [HINGE]
     ╲                                                        ╱
      ╲ [Beam Arm, 2 B]                                      ╱ [Beam Arm, 2 B]
       ╲                                                    ╱
        [MAG PLATE] ────────►  ┌──────────────┐  ◄──────── [MAG PLATE]
                               │ Remora Drone │
                               │  5.5 × 5.5 m │
                               └──────────────┘

    [HINGE]     = Motorized Hinge at level L5:
                  Outer: X −8, r3 · Inner: X −2, r3 (P1)
    [MAG PLATE] = Magnetic Plate (Auto-Lock ON) at arm tip
```

**Mechanical settings:**
- Hinges: Torque 1.0 MN·m, Velocity 15 RPM. Upper limit 45° (deployed), Lower limit 0° (parked flush against truss).
- Magnetic Plates: Auto-Lock ON, strength 100%.

**Blocks per bay:**

| Block | Qty | Name / group |
|-------|-----|-------------|
| Hinge | 2 | `ORC BR-P1 Hinge Outer` / `Inner` → `ORC Bay Jaws` |
| Beam Block (straight) | 4 | Jaw arms (2 B per arm) |
| Magnetic plate | 2 | `ORC BR-P1 Jaw Clamp Outer` / `Inner` → `ORC Bay Jaw Clamps` |

**✓ Check:** Hinges swing smoothly from 0° (flush with wall) to 45° (reaching X −4 and X −6 to grip the 5.5 m drone). Magnetic plates engage upon contact.

---

#### Step 20 · Bay sensors and lights (port side)

```
Bay ceiling plan, bay P1 (looking down)

    X: −8                    −5                    −2
       ┌──────────────────────┬──────────────────────┐  r1 (fore wall)
       │                     [CON]                   │
       │  inner-aft           │          inner-fore  │
       │  corner              │          corner      │
       │  [LIGHT]             │          [SENSOR]    │  r3 bay centre
       │                      │                      │
       │                      │                      │
       └──────────────────────┴──────────────────────┘  r5 (aft wall)

    [SENSOR] = ORC Sensor BR-P1 (inner-fore corner, ceiling)
    [LIGHT]  = Spotlight (inner-aft corner, aimed down the lane)
    Sensor switches its own spotlight: no EC needed
```

**Blocks per bay:**

| Block | Qty | Name / group |
|-------|-----|-------------|
| Sensor | 1 | `ORC Sensor BR-P1` / `P2` / `P3` |
| Spotlight | 1 | → `ORC Bay Lights` |
| Camera (facing down) | 1 | `ORC Cam BR-P1` → `ORC Cams` |

**Sensor settings:** field = bay volume + 15 m below the lane; detect own small grids†.
**Sensor action:** spotlight ON when detected, OFF when not.

**✓ Check:** fly a small grid under a bay → spotlight turns on and camera sees it.

---

#### Step 21 · Mirror to starboard (BR-S)

Repeat Steps 18-20 for bays S1, S2, S3 at X = +5 (connectors), with 1 central repair turret tap at S2 (X +2, r9·L4), mirrored guide jaws (hinges at X +2 and X +8), and sensor/light corners.

**✓ Check:** all 6 bays have: ceiling connector, twin motorized guide jaws, sensor, spotlight, camera. Central bays P2 and S2 each host 1 Laser Multitool repair turret.
- Total in `ORC Bay Conns` = 6
- Total in `ORC Repair` = 2 (P2 and S2 at r9·L4)
- Total in `ORC Bay Jaws` = 12 hinges
- Total in `ORC Bay Jaw Clamps` = 12 magnetic plates
- Total in `ORC Bay Lights` = 6

---

### Phase 3 checkpoint

- [ ] 6 bay connectors (facing down), all on ore trunk branches
- [ ] 2 central repair turrets (P2 and S2 at r9, L4, plumbed to L4 service gallery)
- [ ] 12 motorized guide jaws (twin hinges per bay @ L5 with magnetic clamp tips)
- [ ] 6 bay sensors + 6 spotlights (sensor switches its own light)
- [ ] 6 bay cameras
- [ ] All named per §1 and in correct groups

---

### Phase 4 · Outriggers (OR)

#### Step 22 · Corner atmo arms (OR-1P, OR-1S, OR-3P, OR-3S)

Build the template arm at OR-1P first as a **Cantilevered A-frame diagonal truss**, then copy and mirror.

```
Elevation of OR-1P (looking forward along Z-axis)

    Hull (outer frame)      Cantilevered A-frame truss           Tip
    X = −8                  X = −9      X = −10..−12             X = −12
      ┃                      ┌───────────┬────────────────────────┐  L7 roof
      ■══════════════════════╪═══════════╧════════════════════════■
      ║ ╲ Diagonal Beam      │           │ Top mounting cap       │
      ║   ╲                  │           │                        │
      ║     ╲                │ [GEAR]    │ [LARGE ATMO ↓]         │  L4 mid
      ║       ╲              │ 1×1×3 B   │ 3×3×5 B                │
      ║         ╲            │ (inboard) │                        │
      ■═══════════╧══════════╪═══════════╪════════════════════════■  L1 belly
      ║ ╱ Diagonal Beam      │           │ Nozzle line            │  ─── nozzle
      ║                      │           └────────────────────────┘
      ╨                      ▼ Foot at L0 (≥ 1 B below nozzle)

    Geometry:
    - Root: Outer frame (X = −8), row r1.
    - Projection: 4 B total. Thruster box sits at X = −10 to −12 (3 B wide).
    - Cantilever truss: Diagonal Beams link L1 belly and L7 roof to the thruster housing.
    - Landing gear: 1×1×3 B leg mounted at X = −9 (inboard), foot extends 1 B below nozzle.
```

Plan view of all 4 corner arms:

```
    X: −tip        −8         0         +8        +tip
                    ┃                    ┃
    ┌───────────────┤                    ├───────────────┐
    │  OR-1P  [A↓]  │                    │  [A↓] OR-1S   │  r1
    └───────────────┤                    ├───────────────┘
                    ┃                    ┃
                    ┃    (bays + spine)   ┃
                    ┃                    ┃
    ┌───────────────┤                    ├───────────────┐
    │  OR-3P  [A↓]  │                    │  [A↓] OR-3S   │  r17
    └───────────────┤                    ├───────────────┘
                    ┃                    ┃

    [A↓] = Large atmospheric thruster, facing down
    Arm length = thruster footprint† + 1 B gap
```

**Blocks per arm:**

| Block | Qty | Name / group |
|-------|-----|-------------|
| Large atmospheric thruster | 1 | → `ORC Lift` |
| Landing gear | 1 | → `ORC Gear` |
| Camera (facing down) | 1 | → `ORC Cams` |
| Beam Blocks | per arm | Arm structure |
| Diagonal Beams | per arm | Tip → roof and tip → belly bracing |

**4 corner arms total:** 4 large atmo (`ORC Lift`), 4 landing gear (`ORC Gear`), 4 cameras

**✓ Check:** `ORC Lift` group has 4 thrusters. All face down. No exhaust hits the bay lanes or MP zone. Landing gear feet are below the nozzle line.

---

#### Step 23 · Mid H2 arms (OR-2P, OR-2S)

Same arm template but with a large H2 thruster instead of atmo, no landing gear. Connect H2 line to AS tank.

```
Plan view

    X: −tip        −8         0         +8        +tip
                    ┃                    ┃
    ┌───────────────┤                    ├───────────────┐
    │  OR-2P  [H2↓] │                    │  [H2↓] OR-2S  │  r9
    └───────────────┤                    ├───────────────┘
                    ┃                    ┃

    [H2↓] = Large hydrogen thruster, facing down, DEFAULT OFF
```

**Blocks per arm:**

| Block | Qty | Name / group |
|-------|-----|-------------|
| Large hydrogen thruster | 1 | → `ORC H2 Assist` (set OFF) |
| Beam Blocks + Diagonal Beams | per arm | Structure + bracing |

**✓ Check:** `ORC H2 Assist` has 2 thrusters, both OFF. H2 line runs from AS tank to both arms.

---

### Phase 4 checkpoint

- [ ] 6 outrigger arms built (4 corner + 2 mid)
- [ ] `ORC Lift`: 4 large atmo, all facing down
- [ ] `ORC H2 Assist`: 2 large H2, facing down, OFF
- [ ] `ORC Gear`: 4 landing gear on corner arms
- [ ] H2 line connected AS → OR-2P and OR-2S
- [ ] No exhaust hits bay lanes, MP zone, or other arms
- [ ] Diagonal beam bracing on all arms

---

### Phase 5 · Lateral thrust and gyros

#### Step 24 · Lateral thrusters

```
Plan view, thruster positions

    X: −8.5                   0                   +8.5
       ┌───────────────────────────────────────────────┐
       │  ◄  ◄               bow                ►  ►  │  r0  Lat -Z (push bow)
       │                                               │
       │                                               │
       ▲ r5                                        ▼ r5│      Lat +X / -X
       │                                               │
       │                                               │
       │                                               │
       ▲ r13                                       ▼r13│
       │                                               │
       │                                               │
       │  ◄  ◄              stern                ►  ►  │  r18 Lat +Z (push stern)
       └───────────────────────────────────────────────┘

    ◄ ► ▲ ▼ = small atmo thrusters (4 per group, 16 total)
    Group names = direction they PUSH the Orca
    ▲ on port (X −8.5) = ORC Lat +X (pushes stbd)
    ▼ on stbd (X +8.5) = ORC Lat -X (pushes port)
```

**Blocks:**

| Group | Face | Thrusters | Positions |
|-------|------|-----------|-----------|
| `ORC Lat +X` | Port, X −8.5 | 4 small atmo | r5, r13 (2 each) |
| `ORC Lat -X` | Stbd, X +8.5 | 4 small atmo | r5, r13 (2 each) |
| `ORC Lat -Z` | Bow, r0 | 4 small atmo | X ±5 (2 each side) |
| `ORC Lat +Z` | Stern, r18 | 4 small atmo | X ±5 (2 each side) |

**Total:** 16 small atmospheric thrusters

**✓ Check:** fire each group alone. Orca moves in the correct direction. No flame hits arms, lanes or MP.

---

### Phase 6 · Instrumentation (cameras, sensors, lights)

#### Step 25 · Remaining cameras and sensors

Install the items not already placed in earlier steps.

| Block | Position | Name |
|-------|----------|------|
| Camera | Spine underside, r8, facing down | `ORC Cam Belly` |
| Sensor | Roof, r15, X+1 | `ORC Sensor MP` (field above MP ≈ 20 m; detect own grids) |
| Spotlight × 2 | Roof, r11, X ±side of MP conn | `ORC MP Lights` (aimed up 45°) |
| Ore detector | Spine underside, r15 | `ORC Ore Detector` (broadcast on) |

**`ORC Sensor MP` action:** switches `ORC MP Lights` on/off.

**✓ Check:** 14 cameras total in `ORC Cams`. MP sensor detects a grid approaching from above and lights up.

---

### Phase 7 · Event controllers and timers

#### Step 26 · Wire the EC/TMR ladder

All blocks placed in Step 16. Now configure them:

```
Ladder diagram

    ┌─────────────────┐     ┌──────────────────────────────┐
    │ ORC TMR         │     │ When timer expires:           │
    │ Heartbeat (5 s) │────►│  · Dampeners ON               │
    │ (PB ping 1.0 s) │     │  · ORC Gyros override OFF     │
    │                 │     │  · ORC Lift override OFF       │
    └─────────────────┘     │  · Start ORC TMR Home         │
                            └──────────────────────────────┘
                                         │
                                         ▼
    ┌─────────────────┐     ┌──────────────────────────────┐
    │ ORC TMR Home    │────►│ RC Main autopilot ON → HOME  │
    └─────────────────┘     └──────────────────────────────┘

    ┌─────────────────┐     ┌──────────────────────────────┐
    │ ORC EC Lift     │     │ TRUE:  ORC H2 Assist ON      │
    │ thrust ≥ 85%†   │────►│ FALSE: ORC H2 Assist OFF     │
    └─────────────────┘     └──────────────────────────────┘

    ┌─────────────────┐     ┌──────────────────────────────┐
    │ ORC EC Power    │     │ TRUE:                         │
    │ reactor fail or │────►│  · ORC Batteries → Auto       │
    │ uranium low†    │     │  · ORC H2 Assist ON           │
    └─────────────────┘     │  · Start ORC TMR Home         │
                            └──────────────────────────────┘

    ┌─────────────────┐     ┌──────────────────────────────┐
    │ ORC EC Cargo    │     │ TRUE:  ORC HD Gate OFF        │
    │ hold fill ≥ %   │────►│ FALSE: ORC HD Gate ON         │
    └─────────────────┘     └──────────────────────────────┘

    ┌─────────────────┐     ┌──────────────────────────────┐
    │ ORC EC MP       │     │ TRUE:  ORC HD Drain ON        │
    │ MP Conn locked  │────►│        ORC MP Intake ON       │
    └─────────────────┘     │ FALSE: both OFF               │
                            └──────────────────────────────┘
```

**✓ Check:** each EC/TMR fires in isolation (test by triggering its condition manually in creative).

---

### Phase 8 · Commissioning

#### Step 27 · Static tests

- [ ] Run `tools/bp-inspect.py "ORCA-01"` · record mass
- [ ] Every group from §1 exists and is complete
- [ ] Solar array mass ≤ 20 t? If not, strip it

#### Step 28 · Hover tests

- [ ] Empty hover · record thrust %
- [ ] 90° yaw and pitch hold · add gyro pairs until acceptable
- [ ] Each lateral group alone · correct direction, no flame contact
- [ ] Each lift thruster with a test block under every lane · no damage

#### Step 29 · Load tests

- [ ] Hold at cap, drones docked · thrust ≈ 77%
- [ ] Past cap · EC Lift turns H2 on; unload → off

#### Step 30 · Docking tests

- [ ] Small-grid Connector locks to bay connector†
- [ ] Each bay: drone docks, ore passes Gate, drone recharges
- [ ] Hold at cap: Gate closes
- [ ] MP test: Drain sends ore only; Intake takes U + components only; both stop on release
- [ ] Ore never reaches U Store or Parts Store; uranium never reaches Hold
- [ ] Repair turret fixes a damaged docked drone block, touches nothing else
- [ ] Turret reaches every face of a docked drone (add outer turret if not)

#### Step 31 · Safety tests

- [ ] PB off → dampeners hold → HOME after delay
- [ ] Reactors off at hover → batteries Auto, H2 on → controlled descent
- [ ] RC Spare as main controller → HOME works

#### Step 32 · Full loop test

- [ ] One unattended loop: mine → offload → ferry → refine → return

#### Commissioning record

| Measurement | Value |
|-------------|-------|
| Mass, empty (t) | |
| Empty hover thrust (%) | |
| `ORC EC Cargo` threshold (%) | |
| Arm length (B) | |
| Gyro count | |
| Solar panels fitted · noon output (MW) | |
| `ORC TMR Heartbeat` delay (s) | 5.0 s (1.0 s IGC beat) |
| `ORC TMR Home` delay (s) | |
| RC speed limit (m/s) | |

---

### Block summary

| Block | Total | Group |
|-------|-------|-------|
| Large atmospheric thruster | 4 | `ORC Lift` |
| Large hydrogen thruster | 2 | `ORC H2 Assist` |
| Small atmospheric thruster | 16 | `ORC Lat +X/-X/+Z/-Z` |
| Small reactor | 6 | `ORC Reactors` |
| Large battery | 2 | `ORC Batteries` |
| Gyroscope (large) | 4+ | `ORC Gyros` |
| Large cargo container | 1 | `ORC Hold` |
| Small cargo container | 2 | `ORC U Store` · `ORC Parts Store` |
| Hydrogen tank | 1 | `ORC AS H2` |
| Connector (down, bay ceiling) | 6 | `ORC Bay Conns` |
| Connector (up, MP) | 1 | `ORC MP Conn` |
| Conveyor sorter | 3 | `ORC HD Gate` · `ORC HD Drain` · `ORC MP Intake` |
| Laser multitool turret (mod) | 2 | `ORC Repair` (P2 & S2 @ r9·L4) |
| Hinge | 14 | `ORC Bay Jaws` (12) · `ORC MP Cradle` (2) |
| Magnetic plate | 16 | `ORC Bay Jaw Clamps` (12) · `ORC MP Cradle Clamps` (2) · `ORC Ventral Anchors` (2) |
| Landing gear | 4 | `ORC Gear` (corner outriggers) |
| Remote Control | 2 | `ORC CC RC Main/Spare` |
| Programmable Block | 1 | `ORC CC PB` |
| Timer Block | 2 | `ORC TMR Heartbeat/Home` |
| Event Controller | 4 | `ORC EC Lift/Power/Cargo/MP` |
| Antenna | 1 | `ORC CC Antenna` |
| Camera | 14 | `ORC Cams` |
| Sensor | 7 | 6 bay + 1 MP |
| Spotlight | 8 | 6 bay + 2 MP |
| Ore detector | 1 | `ORC Ore Detector` |
| Large solar panel | ≈24† | `ORC Solar` |
| Beam Block | ~320† | Structure & jaw arms |
| Structural Frame 3×1×3 | ~30† | Roof deck |
| Diagonal Beam | ~48† | Arm bracing (A-frame truss) |
| Heavy armour block | ~135† | CC (36) + PC (48) + MP blast deck (49) |
| Light armour block | ~36† | HD + AS shells |
| Large conveyor tube/junction | ~52† | Ore trunk (18) + PC (7) + L4 service pipe (26) + AS |



---

## Chapter 2 · REMORA-01 Vertical Shaft Driller

Source: `docs/rig/chapters/remora-instructions.md` · 11 × 11 sb · Small-Grid Vertical Miner (Top-Docking)

### 0 · Reference Grid & Envelope

The Remora is a small-grid vertical mining drone built on an **11 × 11 sb** footprint (5.5 × 5.5 m) and a height of **22 sb** (11.0 m) from drill tips to crown connector.

```
Axes:
  X = Beam:     Port (−5 sb) ◄── 0 (Core Axis) ──► Starboard (+5 sb)
  Z = Length:   Aft  (−5 sb) ◄── 0 (Core Axis) ──► Forward   (+5 sb)
  Y = Height:   Down (L0 drill tips) ────────────► Up        (L22 crown connector)

Scale: 1 sb (small block) = 0.5 m
```

#### Master Section Stack (L0 → L22)

```
Level (sb)   Sec   Description
─────────────────────────────────────────────────────────────────────────────
L20-L22      CR    Docking Crown · Connector ↑, Cam ↑, protective bumpers
L17-L20      AV    Avionics · RC, PB, Antenna, Timers, Event Controllers
L15-L17      PW    Power · 2× Large Batteries around core spine
L10-L15      LF    Lift & Maneuver · 4× Large Atmo (corners), 4× Large Flat Atmo
                   (faces), Gyros (center), Jaw Clamping Collar (perimeter)
L6-L10       CL    Collector & Hold · Central conveyor junction, stone retention
L0-L6        DH    Drill Head · 8× Drills in ring, nose down, Cam ↓ & Light ↓
```

#### Master Elevation (Looking Starboard, X = 0)

```
        Z:  −5 (Aft)             0 (Spine)             +5 (Fwd)
    L22     ·                   [REM Dock ↑]                 ·  CROWN
    L20     ·                    │ Cam Up │                  ·
            ┌────────────────────┼────────┼──────────────────┐
    L17     │ [Timers / ECs]     │ REM RC │   [REM PB / Ant] │  AVIONICS
            ├────────────────────┼────────┼──────────────────┤
    L15     │ [REM Battery 1]    │ Spine  │  [REM Battery 2] │  POWER
            ├───────┬────────────┼────────┼────────────┬─────┤
            │ [Lift]│[REM Lat A] │ Gyros  │[REM Lat F] │[Lift│
    L10     │  ▼▼▼  │[Jaw Collar]│ Spine  │[Jaw Collar]│ ▼▼▼ │  LIFT & COLLAR
            └───┬───┴────────────┼────────┼────────────┴──┬──┘
    L6          │ ◄── 4 sb Flame │ Gap ──►│               │
            ┌───┴────────────────┴────────┴───────────────┴──┐
            │               Collector Junction               │  COLLECTOR
    L4      ├────────────────────┬────────┬──────────────────┤
            │ [REM Drill Aft]    │Cam/Lite│ [REM Drill Fwd]  │  DRILL HEAD
    L0      │        ▼▼▼         │  ↓↓↓   │       ▼▼▼        │
```

---


#### 0.4 · Master Conveyor Routing Schematic

Per **Decision 32**, the Remora retains all mined stone and offloads it to the Orca. The conveyor circuit is a single vertical extraction pipe:

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

---

#### 0.5 · Master Electrical Single-Line Schematic

```
               POWER STORAGE (2× Small-Grid Large Batteries)
                    2.16 MWh Capacity · 8.0 MW Max Output
                                 │
                                 ▼
                         PRIMARY POWER BUS
        ┌────────────────────────┼────────────────────────┐
        │                        │                        │
        ▼                        ▼                        ▼
  LIFT PROPULSION          LATERAL THRUST           DRILLS & AVIONICS
  4× Large Atmo            4× Large Flat Atmo       8× Drills (0.16 MW)
  (9.6 MW Max Peak)        (0.92 MW Max Peak)       1× RC + 1× PB (0.05 MW)
  [Empty Hover: 2.0 MW]    [Attitude Gyros: 0.1 MW] 2× Cams + Spot (0.02 MW)
  [Laden Hover: 8.0 MW]
```

---

#### 0.6 · Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Sizes | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Drill Head** | 8× Drills (small grid, 3×3×6 sb), 1× Cam, 1× Light | 10 | 18.2 t | 850 |
| **Lift & Maneuver** | 4× Large Atmo (3×3×5 sb), 4× Large Flat Atmo | 8 | 13.8 t | 540 |
| **Power Plant** | 2× Large Battery (small grid, 3×3×2 sb) | 2 | 8.8 t | 40 |
| **Avionics & Logic** | 1× RC, 1× PB, 1× Antenna, 2× Timers, 3× ECs | 8 | 1.8 t | 140 |
| **Hold & Spine** | 4× Conveyor Hubs, 16× Tubes, 1× Connector | 21 | 3.4 t | 120 |
| **Armor & Collar** | Light armor slopes, 16× Heavy armor clamp pads | 84 | 4.0 t | 84 |
| **TOTAL (DRY)** | Complete Airframe (REMORA-01) | **133** | **≤ 50.0 t** | **1,774** |

---


#### 0.7 · Human Builder Guide · Visual Paint Palette & Small-Grid HUD

Small-grid construction (1 sb = 0.5 m) is notoriously prone to 1-block off-center errors and missed conveyor port orientations. Use this in-game paint palette (`[P]` key) during assembly:

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ REMORA CONSTRUCTION PAINT PALETTE (Recommended Paint System)                        │
├─────────────────────┬──────────────┬───────────────┬─────────────────────────────────┤
│ Functional Role     │ Paint Finish │ In-Game Color │ Purpose / Visual Cue            │
├─────────────────────┼──────────────┼───────────────┼─────────────────────────────────┤
│ Central Conveyor    │ Clean        │ Safety Yellow │ Traces vertical ore extraction spine
│ Waist Clamp Collar  │ Hazard       │ Orange Stripe │ Marks exact L12-L13 Orca jaw contact zone
│ Corner Lift Pods    │ Clean        │ Pure White    │ Visually separates 4 corner atmo pods
│ Lift Nozzle Ring    │ Battered     │ Safety Red    │ Highlights L10 thermal flame boundary
│ Drill Head Base     │ Carbon/Worn  │ Matte Black   │ Outlines 9×9 sb physical drill cutter
│ Avionics Deck       │ Heavy Armor  │ Fleet Blue    │ Identifies RC, PB, and safety timers
└─────────────────────┴──────────────┴───────────────┴─────────────────────────────────┘
```

##### Small-Grid Coordinate HUD
Every step includes an exact small-block coordinate banner:
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Section: <Name>  ·  Footprint: <sb −5 to +5>  ·  Level: <L0-L22>           │
│ [VIEWPORT] Looking: <Direction>  ·  ▲ FWD (+Z) · ◄ PORT (−X) · STBD (+X)► · ▼ AFT (−Z)│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

#### 0.8 · Gravity Scaffolding & Hanging Jig Setup

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
1. Place a large-grid test connector or small-grid rotor head 12 m above the tarmac.
2. Snap `REM Dock` (small-grid full-size connector) directly to it facing straight UP.
3. Build all remaining levels downward from L21 to L0. The drone remains locked in mid-air.

---

### Phase 1 · Core Spine & Collector (CL, L6-L20)

Build crown-up, hanging from a temporary test connector or shipyard head. The central vertical conveyor tube on axis (0, 0) carries ore from the drill head to the crown.

#### Step 1 · Jig & Crown Axis Mount (L20-L22)

Place the master jig block and upper docking connector facing UP at level L21-L22.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 1 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Facing              │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Full)    │ 1   │ 1.2 t / 150   │ Small-grid full-size; face UP @ L21-L22  │
│ Camera              │ 1   │ 0.05 t / 10   │ Face UP beside connector @ (X+1, Z0)     │
│ Light Armor Slope   │ 4   │ 0.16 t / 4    │ Chamfer collar around connector base     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘


```
Plan view, Y = L21 (Crown Level)

    X (sb): −1    0   +1
    Z: +1    ·   [   ] ·
        0   [   ][CON][CAM]   ◄── Connector centered at (0, 0)
       −1    ·   [   ] ·

    [CON] = Small-Grid Full-Size Connector (facing UP)
    [CAM] = Camera (facing UP at +1, 0)
    [   ] = Small-grid light armor corner collar
```

**Blocks:**
* 1 × Connector (small grid, full-size) · `REM Dock` (facing UP)
* 1 × Camera · `REM Cam Up` (facing UP, recessed 1 sb below connector rim)

**✓ Check:** Connector sits dead-center on the 11 × 11 footprint (sb 0, 0). Faces straight up.

---

#### Step 2 · Vertical Conveyor Spine (L6-L20)

Run a continuous conveyor line straight down from `REM Dock` through the center of levels L20 down to L6.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 2 PARTS TRAY & CONVEYOR PORT ORIENTATION GUIDE                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Conveyor Port Orientation       │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Conveyor Hub 3×3 sb │ 4   │ 1.2 t / 40    │ Side ports MUST face cardinal N/S/E/W    │
│ Conveyor Tube sb    │ 12  │ 0.8 t / 36    │ Continuous vertical tube along (0, 0)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION] CONVEYOR HUB PORT ALIGNMENT
> The small-grid conveyor hubs at L8, L12, L16, and L18 must align their large-port faces
> directly toward the 4 cardinal directions (Fwd, Aft, Port, Stbd) to feed drill tubes and avionics.


```
Side elevation, X = 0 (looking Stbd)

    L20   [REM Dock]
           │
    L18   [Conveyor Junction 3×3 sb]   (Avionics tap)
           │
    L16   [Conveyor Junction 3×3 sb]   (Battery tap)
           │
    L12   [Conveyor Junction 3×3 sb]   (Lift deck tie)
           │
    L8    [Collector Core Box 3×3 sb]  (Drill manifold)
           │
    L6    [Manifold Base]
```

**Blocks:**
* 3 × Small Grid Conveyor Junction (3×3×3 sb) or Medium/Large Conveyor Hubs
* 8 × Small Grid Conveyor Tubes (running along axis 0, 0)

**✓ Check:** Continuous conveyor path exists from L21 down to L6 with zero breaks.

---


> [!IMPORTANT] PRE-ENCLOSURE GATE 1 · COLLECTOR MANIFOLD FLOW CHECK
> Before attaching the 8 drills in Step 5:
> 1. Stand at Level L6 looking up into the collector manifold.
> 2. Verify all 8 branch tubes mate directly into the central vertical core hub.
> 3. Verify no branch tube was placed with its port misaligned.
> Once the heavy drill blocks are welded, reaching the internal tubes requires grinding drills!

#### Step 3 · Collector Manifold & Stone Retention (L6-L10)

Expand the spine into an 8-port collector hub at L6-L8. Per **Decision 32**, the stone ejector ring and sorters are omitted; all rock is retained in the collection loop.

```
Plan view, Y = L7 (Collector Hub)

    X (sb): −2    0   +2
    Z: +2   [OUT] [TUBE] [OUT]   ◄── Feeds Fwd Drills
        0   [TUBE][CORE] [TUBE]  ◄── Axis Conveyor
       −2   [OUT] [TUBE] [OUT]   ◄── Feeds Aft Drills

    [CORE] = Central Conveyor Junction (feeds upward)
    [TUBE] = Curved/Straight Conveyor Tubes radiating out to 8 drill ports
```

**Blocks:**
* 4 × Small Conveyor Junction (3×3×3 sb)
* 8 × Small Conveyor Tube (outlets aligned with drill mount points at L6)

**✓ Check:** 8 conveyor ports point downward at level L6, positioned to mate with the 8 ring drills.

---


#### Phase 1 Visual Milestone · What Your Grid Looks Like Now

```
Standing at Ground Level Looking Up at the Hanging Jig:
       L22 Jig ─── [REM Dock Connector ↑]
                       │
       L18             │ Central Conveyor Tube (Safety Yellow)
                       │ (Hanging like a mechanical plumb bob)
       L12             │
                       │
       L6              [8-Port Collector Manifold Hub]
   Visual Sanity Check: A single continuous vertical yellow pipe hanging 8 m down
   from the ceiling jig, terminating in a radiating 8-port hub at the bottom.
```

---

### Phase 2 · Drill Head (DH, L0-L6)

Eight drills arranged in a perimeter ring around a 3×3 sb central core. Nose down.

#### Step 4 · Drill Head Core & Sensor Eye (L0-L6)

#### 2.5D Isometric Callout · Drill Head Core & Sensor Eye (L0-L6)

```
        Y (Up)
          ▲
      L6  │         ┌───────────────────────────┐
          │         │ Collector Manifold (L6)   │
      L4  │         ├───────────┬───────────────┤
          │         │ [ARMOR]   │   [ARMOR]     │
          │         │           │ [REM Cam Down]│  ◄── Recessed Sensor Eye
          │         │           │ [REM Light Dn]│      Behind Armor Lip
      L0  │         ├───────────┴───────────────┤
          │         │ 8× Drills Perimeter Ring  │  ◄── 9×9 sb Cutting Head
          └─────────┴───────────────────────────┴► X/Z
```


Build the central protected sensor core at (0, 0) between levels L0 and L6.

```
Plan view, Y = L4 (Core Center)

    X (sb): −1    0   +1
    Z: +1   [ARM][ARM][ARM]
        0   [ARM][EYE][ARM]    ◄── Sensor recess behind armor
       −1   [ARM][ARM][ARM]

    [EYE] = 1× Camera ↓ (`REM Cam Down`) + 1× Spotlight ↓ (`REM Light Down`)
    [ARM] = Light armor slopes forming a conical protective nose
```

**Blocks:**
* 1 × Camera · `REM Cam Down` (facing straight DOWN)
* 1 × Spotlight · `REM Light Down` (facing straight DOWN, radius 15 m)
* 8 × Light Armor Slope / Block (small grid)

**✓ Check:** Camera and spotlight face down; light cone illuminates borehole floor without obstruction.

---

#### Step 5 · 8-Drill Perimeter Ring (L0-L6)

Mount 8 small-grid drills around the sensor core. Each drill occupies a 3×3 sb footprint.

```
Plan view, looking up from below (Y = L0)

    X (sb): −4      0     +4
    Z: +4  ┌─────┬─────┬─────┐
           │DRL 1│DRL 2│DRL 3│  (Forward row)
           ├─────┼─────┼─────┤
        0  │DRL 8│ EYE │DRL 4│  (Mid row + sensor core)
           ├─────┼─────┼─────┤
       −4  │DRL 7│DRL 6│DRL 5│  (Aft row)
           └─────┴─────┴─────┘
```

**Blocks:**
* 8 × Drill (small grid, 3×3×6 sb) · `REM DH Drill 1` … `REM DH Drill 8` → group `REM Drills`

**✓ Check:**
* All 8 drill conveyor ports mate directly with the L6 collector manifold.
* Outer cutting envelope spans 9 × 9 sb physical; drill voxel radius covers ≥ 11 sb shaft width.

---


#### Phase 2 Visual Milestone · What Your Grid Looks Like Now

```
Looking from Below Upward at the Cutting Face (Y = L0):
       ┌─────────────────────────────────────────────────────────┐
       │   [DRL 1]             [DRL 2]             [DRL 3]       │
       │   [DRL 8]       [RECESSED SENSOR EYE]     [DRL 4]       │
       │   [DRL 7]             [DRL 6]             [DRL 5]       │
       └─────────────────────────────────────────────────────────┘
   Visual Sanity Check: A solid 9 × 9 sb square cutting face. The central camera
   and spotlight sit safely recessed 1 block behind the armor nose cone.
```

---

### Phase 3 · Lift, Lateral Thrust & Waist Clamping Collar (LF, L10-L15)

Lift thrusters sit at levels L10-L15. A **4 sb flame gap** exists between thruster nozzles (L10) and drill tops (L6) to prevent thermal self-damage.


> [!CAUTION] PRE-ENCLOSURE GATE 2 · 4 sb FLAME GAP PHYSICAL MEASUREMENT
> Before welding outer armor cowlings around the lift pods in Step 6:
> 1. Count the small blocks between the bottom nozzle rim (Level L10) and the top flat 
>    surface of the drill casings (Level L6).
> 2. There MUST be exactly 4 small blocks (2.0 m) of completely open air!
> 3. If the distance is 3 sb or less, thruster flame damage will destroy the drills on liftoff!

#### Step 6 · Corner Large Atmo Lift Pods (L10-L15)

#### 2.5D Isometric Callout · Lift Thruster Pods & 4 sb Flame Gap

```
      L15 ┌───────────────────────────────────────────┐
          │ Corner Lift Pods (3×3×5 sb Large Atmo)    │
      L10 └───▼▼▼───────────────────────────────▼▼▼───┘
              Nozzle                            Nozzle
          ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  ◄── 4 sb FLAME CLEARANCE GAP
          ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░      (Zero thermal damage to drills)
      L6  ┌───────────────────────────────────────────┐
          │ Drill Head Top Mounts                     │
      L0  └───────────────────────────────────────────┘
```


Install four Large Atmospheric Thrusters (small grid, 3×3×5 sb) at the four exterior corners. Nozzles point down (D).

```
Plan view, Y = L12 (Lift Deck)

    X (sb): −5  −3      0      +3  +5
    Z: +5  [LIFT NW]   ···   [LIFT NE]   (Fwd corners)
           [ 3×3×5 ]         [ 3×3×5 ]
        0     ···    [SPINE]    ···
           [ 3×3×5 ]         [ 3×3×5 ]
       −5  [LIFT SW]   ···   [LIFT SE]   (Aft corners)

    Nozzles sit at L10 pointing DOWN (D).
```

**Blocks:**
* 4 × Large Atmospheric Thruster (small grid, 3×3×5 sb)  · 
  * `REM LF Lift NW` (Fwd-Port)
  * `REM LF Lift NE` (Fwd-Stbd)
  * `REM LF Lift SW` (Aft-Port)
  * `REM LF Lift SE` (Aft-Stbd)
  * Group → `REM Lift` (Total thrust: 2,304 kN)

**✓ Check:** Nozzles sit at L10. 4 sb clear vertical space exists between nozzle discharge and the top of the drills at L6.

---

#### Step 7 · Internal Gyroscope Cluster (L11-L14)

Mount 2 small-grid gyroscopes tightly against the central conveyor spine.

```
Plan view, Y = L13 (Spine Core)

    X (sb): −1    0   +1
    Z: +1   ··· [GYRO 1] ···
        0   ··· [SPINE]  ···
       −1   ··· [GYRO 2] ···
```

**Blocks:**
* 2 × Gyroscope (small grid) · `REM Gyro 1`, `REM Gyro 2` → group `REM Gyros`

**✓ Check:** Gyros mounted rigidly to the central conveyor line; roll/pitch/yaw respond crisply in RIG Test.

---

#### Step 8 · 4-Face Lateral Thruster Array (L11-L14)

Install 1 Large Flat Atmospheric Thruster on each of the 4 outer faces, centered between the corner lift pods.

```
Plan view, Y = L13 (Face Thrusters)

    X (sb): −5          0          +5
    Z: +5   ···   [REM Lat Fwd]   ···   ◄── Pushes Backward (Exhaust Fwd)
             │          │          │
        0  [LAT P]   [SPINE]    [LAT S] ◄── Pushes Port/Stbd
             │          │          │
       −5   ···   [REM Lat Aft]   ···   ◄── Pushes Forward (Exhaust Aft)
```

**Blocks:**
* 4 × Large Flat Atmospheric Thruster (small grid, 3×3×2 sb):
  * `REM Lat Fwd` (Mounted at Z = +5, pushing aft / exhaust forward)
  * `REM Lat Aft` (Mounted at Z = −5, pushing forward / exhaust aft)
  * `REM Lat Port` (Mounted at X = −5, pushing stbd / exhaust port)
  * `REM Lat Stbd` (Mounted at X = +5, pushing port / exhaust stbd)
  * Groups → `REM Lat F`, `REM Lat A`, `REM Lat P`, `REM Lat S`

**✓ Check:** Each lateral thruster delivers 230 kN. Placed flush with the 11 sb hull line.

---

#### Step 9 · Waist Magnetic Clamp Collar (L11-L14)

#### 2.5D Isometric Callout · Waist Magnetic Clamping Collar (L11-L14)

```
                       [Remora Waist Hull (11×11 sb)]
                                     │
    Port Flank                       ▼                      Starboard Flank
  [HEAVY ARMOR PAD]                                       [HEAVY ARMOR PAD]
  (At L12-L13, X −5)                                      (At L12-L13, X +5)
         ▲                                                       ▲
         │                                                       │
    [MAG PLATE] ◄── 45° Inward Swing of Orca Bay Jaws ──► [MAG PLATE]
         │                                                       │
    [HINGE ARM]                                             [HINGE ARM]
  ═══════╧═══════════════════════════════════════════════════════╧═══════
                    ORCA BAY FRAME INNER WALL (L5)
```


Per **Decisions 36 & 37**, the Orca bay features motorized folding guide jaws tipped with Magnetic Plates at L5. At the Remora waist (L11-L14), install reinforced heavy/light armor contact strips for the jaws to grip.

```
Section elevation, looking at Port face (X = −5)

    L15  ┌───────────────────────────────────┐
         │ Corner Lift │ Lateral │ Corner Lift │
    L14  │             ├─────────┤             │
    L13  │   [PAD]     │ [FLAT   │   [PAD]     │  ◄── CLAMP CONTACT ZONE
    L12  │   [PAD]     │  ATMO]  │   [PAD]     │  ◄── Orca jaws lock here
    L11  │             ├─────────┤             │
    L10  └─────▼▼▼─────┴─────────┴─────▼▼▼─────┘
             Nozzle                  Nozzle
```

**Blocks:**
* 16 × Heavy Armor Block (small grid) · flush outer contact pads on all 4 corners at L12-L13

**✓ Check:** Flat outer contact surface allows Orca's Magnetic Plates to achieve 100% surface lock without colliding with thruster cowlings.

---


#### Phase 3 Visual Milestone · What Your Grid Looks Like Now

```
Standing at Port Quarter Looking at the Drone Waist (Y = L10-L15):
       L15 ┌─────────────────────────────────────────────────┐
           │ Corner Lift Pod │ Flat Atmo (P) │ Corner Lift Pod │
       L13 │                 ├───────────────┤                 │
       L12 │ [ORANGE STRIPE] │ Exhaust Port  │ [ORANGE STRIPE] │ ◄── Orca Clamp Pad
       L10 └───▼▼▼───────────┴───────────────┴───────────▼▼▼───┘
               Nozzle                                    Nozzle
   Visual Sanity Check: The drone now has its full 11 × 11 sb profile. The 4 corner
   lift pods frame the central core, and flush orange hazard pads mark the waist.
```

---

### Phase 4 · Power Plant (PW, L15-L17)

All drone operations are pure electric. Batteries size to one full drilling sortie + 50% safety margin.

#### Step 10 · Twin Large Battery Bank (L15-L17)

Mount 2 small-grid Large Batteries symmetrically flanking the spine.

```
Plan view, Y = L16 (Power Deck)

    X (sb): −3    0   +3
    Z: +2   ···  ···  ···
        0  [BAT 1][SPINE][BAT 2]   (3×3×2 sb footprint each)
       −2   ···  ···  ···
```

**Blocks:**
* 2 × Large Battery (small grid, 3×3×2 sb, 1.08 MWh, 4 MW max each) · `REM Battery 1`, `REM Battery 2` → group `REM Batteries`

**✓ Check:** 2.16 MWh total storage, 8 MW peak power. Sufficient for 4 large lift thrusters (9.6 MW max, hovering at ~50% load = 4.8 MW).

---

### Phase 5 · Avionics Suite (AV, L17-L20)

Automated navigation, IGC comms, and native safety fallbacks.

#### Step 11 · Flight Control Core (L17-L19)

Mount Remote Control, Programmable Block, and Antenna directly above the battery deck.

```
Plan view, Y = L18 (Avionics Deck)

    X (sb): −2    0   +2
    Z: +2   ··· [REM RC] ···      ◄── RC orientation: Forward = +Z, Up = +Y
        0  [PB] [SPINE]  [ANT]
       −2   ···   ···   ···
```

**Blocks:**
* 1 × Remote Control (small grid) · `REM RC` (Forward = +Z / Fwd, Up = +Y / Crown)
* 1 × Programmable Block (small grid) · `REM PB` (Runs `Drone.Miner` script)
* 1 × Antenna (small grid) · `REM Antenna` (Range set to 1,500 m† to cover Orca lease radius)

**✓ Check:** `REM RC` orientation matches drone flight axis. Gyro override and dampener tests behave properly.

---

#### Step 12 · Safety Timers & Event Controllers (L18-L20)

Per **Decision 42**, configure native timer failover and hardware event triggers.

```
Plan view, Y = L19 (Logic Collar)

    X (sb): −2    0   +2
    Z: +2  [EC-BATT][ · ][EC-LIFT]
        0  [TMR-HB] [CON][TMR-RET]
       −2  [EC-DOCK][ · ][  ·   ]
```

**Blocks:**
* 2 × Timer Block (small grid):
  * `REM TMR Heartbeat` (Delay: 5.0 s per Decision 42)
  * `REM TMR Return` (Delay: 2.0 s)
* 3 × Event Controller (small grid):
  * `REM EC Batt` (Monitors `REM Batteries` ≤ 25%)
  * `REM EC Lift` (Monitors `REM Lift` thrust ≥ 90%)
  * `REM EC Dock` (Monitors `REM Dock` locked state)

**✓ Check:** Logic circuits armed. Heartbeat watchdog trips native return sequence if PB stops pulsing.

---

### Phase 6 · Docking Crown & Fairing (CR, L20-L22)

The top of the drone mates with the ceiling connector of an Orca bay.

#### Step 13 · Docking Head Finalization (L20-L22)

Connect `REM Dock` to the top of the conveyor spine. Add corner guide slopes to ensure smooth alignment into the Orca bay throat.

```
Side elevation, looking Port (X = 0)

    L22      [  REM Dock  ]
    L21      ▲▲  [Cam]   ▲▲    ◄── Guide chamfers
    L20   ┌──┴───────────┴──┐
          │ Avionics Deck   │
```

**Blocks:**
* 4 × Light Armor Slope 2×1×1 (small grid) · collar chamfers around the connector rim

**✓ Check:** Connector face stands 1 sb proud of all hull blocks. No adjacent block interferes with large-grid connector lock.

---

### Phase 7 · Control Logic & Failover Matrix


#### Step 12.1 · Master AI Scripting Contract & Terminal Namespace (`Drone.Miner`)

Every automated block on REMORA-01 conforms to this exact naming and grouping dictionary for Claude Code and `Drone.Miner`:

##### 1. Group Taxonomy
* `REM Lift` : 4× Large Atmospheric Thrusters (`REM LF Lift NW`, `NE`, `SW`, `SE`). Total thrust = 2,304 kN.
* `REM Lat F` : 1× Large Flat Atmospheric Thruster (face Z = +5, pushes aft / exhaust fwd). 230 kN.
* `REM Lat A` : 1× Large Flat Atmospheric Thruster (face Z = −5, pushes fwd / exhaust aft). 230 kN.
* `REM Lat P` : 1× Large Flat Atmospheric Thruster (face X = −5, pushes stbd / exhaust port). 230 kN.
* `REM Lat S` : 1× Large Flat Atmospheric Thruster (face X = +5, pushes port / exhaust stbd). 230 kN.
* `REM Drills` : 8× Drills (`REM DH Drill 1` … `8`).
* `REM Batteries` : 2× Large Batteries (`REM Battery 1, 2`).
* `REM Gyros` : 2× Gyroscopes (`REM Gyro 1, 2`).
* `REM Cams` : 2× Cameras (`REM Cam Down` at nose, `REM Cam Up` at crown).
* `REM Lights` : 1× Spotlight (`REM Light Down`).

##### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `REM EC Batt` | `REM Batteries` Stored % | Stored $\le 25\%$ | `REM Drills` OFF · Trigger `REM TMR Return` | · |
| `REM EC Lift` | `REM Lift` Thrust % | Thrust $\ge 90\%$ (Overload) | `REM Drills` OFF · Trigger `REM TMR Return` | · |
| `REM EC Dock` | `REM Dock` State | Connector Locked | `REM Drills` OFF · `REM Batteries` $
ightarrow$ Recharge | `REM Batteries` $
ightarrow$ Auto |

##### 3. Native Failover Cascades (Zero-PB Safety)
If `REM PB` crashes or loses execution:
1. `REM TMR Heartbeat` (delay 5.0 s per Decision 42) expires without PB pulse.
2. Immediate actions: `REM Drills` $
ightarrow$ OFF · Thrust overrides $
ightarrow$ OFF · Dampeners $
ightarrow$ ON · Trigger `REM TMR Return`.
3. `REM TMR Return` (delay 2.0 s) trips: `REM RC` Autopilot engages $
ightarrow$ ascends vertically to waypoint `PREDOCK` (3 m below Orca bay).
4. The Orca recovers the drone from `PREDOCK` via bay approach sensor and folding guide jaws.

##### 4. IGC Broadcast Channels
* Outbound: `TMS.REMO.STATUS`: `drone_id;battery_pct;cargo_pct;hole_depth_m;status_enum`
  * Example: `REM-01;82.4;61.0;18.5;DRILLING`
* Inbound: `FLT.DISP.LEASE`: `drone_id;hole_gps;bay_id;return_deadline_s`

#### Event Controller & Timer Mapping

| Block | Condition / Trigger | Action True | Action False |
|---|---|---|---|
| **`REM TMR Heartbeat`** | 5.0 s delay (restarted every 1.0 s by PB via IGC) | Drills OFF · Gyro/Thrust Overrides OFF · Dampeners ON · Trigger `REM TMR Return` | · |
| **`REM TMR Return`** | 2.0 s delay | `REM RC` Autopilot ON → Waypoint `PREDOCK` (3 m below bay) | · |
| **`REM EC Batt`** | `REM Batteries` ≤ 25% | Drills OFF · Trigger `REM TMR Return` | · |
| **`REM EC Lift`** | `REM Lift` thrust ≥ 90% (Ore Cap overload) | Drills OFF · Trigger `REM TMR Return` | · |
| **`REM EC Dock`** | `REM Dock` Locked | Drills OFF · `REM Batteries` → Recharge · IGC notify Orca | `REM Batteries` → Auto |

---

### Phase 8 · Commissioning Checklist (RIG Test Creative)

Execute in sequence before loading scripts:

#### A · Static Inspection
* [ ] Dry mass verified: `≤ 50 t`† via terminal / info tab.
* [ ] All groups verified: `REM Lift`, `REM Lat F/A/P/S`, `REM Drills`, `REM Batteries`, `REM Gyros`.
* [ ] Conveyor integrity: ore dumped into drills passes smoothly up to `REM Dock`.

#### B · Hover & Thrust Margin
* [ ] Empty hover: `REM Lift` thrust load ≈ 21% (Earth 1 g).
* [ ] Max laden test (195.7 t mass): `REM Lift` thrust load ≈ 83% (T/W 1.2 achieved).
* [ ] Flame clearance check: fire `REM Lift` at 100% override for 10 s; verify drill tops take 0 damage.

#### C · Boring Test
* [ ] Descend 20 m straight down on drills: body follows borehole cleanly without snagging walls.
* [ ] Stone handling check: stone accumulates in hold/drills; no ejectors dump gravel.

#### D · Orca Bay Docking
* [ ] Approach Orca bay from below to `PREDOCK` (3 m below L1 belly).
* [ ] Orca L5 guide jaws swing inward and Magnetic Plates clamp waist collar.
* [ ] Vertical ascent into L7 ceiling connector: `REM Dock` locks cleanly.
* [ ] Batteries automatically switch to `Recharge`; Orca repair turret reaches hull.

#### E · Safety Failover
* [ ] Kill `REM PB` mid-drill: verify heartbeat timer expires at 5.0 s, drills abort, dampeners catch, and drone rises to `PREDOCK`.

---

### Phase 9 · Verify Items (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V34:** Drill cutting radius vs 11 sb hull width (ensure 8-drill ring cuts clear borehole without hull snagging).
* **V35:** Large Atmo (small grid) flame reach: verify 4 sb gap provides 100% thermal clearance.
* **V36:** Small-grid connector lock to Large-Grid connector (verify port connection geometry).



---

## Chapter 3 · MULE-01 Heavy Freight Shuttle

Source: `docs/rig/chapters/mule-instructions.md` · 7 × 7 B · Pure Electric Cargo Ferry (650 t Payload)

### 0 · Reference Grid & Envelope

The Mule is a heavy large-grid freight shuttle built on a **7 × 7 B** square footprint (17.5 × 17.5 m) and a height of **3 B** (7.5 m) from belly skid to roofline.

```
Axes:
  X = Beam:     Port (−3 B) ◄── 0 (Hold Center) ──► Starboard (+3 B)
  Z = Length:   Aft  (−3 B) ◄── 0 (Hold Center) ──► Bow / Fwd (+3 B)
  Y = Height:   L1 (Belly & Docking) ──► L2 (Midline) ──► L3 (Roof Deck)

Scale: 1 B (large block) = 2.5 m
```

#### Master Plan View (Mid-deck, Y = L2)

```
        X:  −3    −2    −1     0    +1    +2    +3
            ·     ·      ·     ·     ·     ·     ·
    Z: +3  ┌─────┬─────┬═════════════┬─────┬─────┐  BOW
           │BAT 1│GYRO1│   LIFT FWD  │GYRO2│BAT 2│
    Z: +2  ├─────┴─────┤ (3×2 Flat)  ├─────┴─────┤
           │           ├─────────────┤           │
    Z: +1  │ LIFT PORT │             │ LIFT STBD │
           │ (2×3 Flat)│ LARGE CARGO │ (2×3 Flat)│
    Z:  0  │ [CLAMP P] │ (3×3×3 B)   │ [CLAMP S] │  ◄── CoM / Belly Port
           │           │             │           │
    Z: −1  │           ├─────────────┤           │
           ├─────┬─────┤  LIFT AFT   ├─────┬─────┤
    Z: −2  │BAT 3│ AV1 │ (3×2 Flat)  │ AV2 │BAT 4│
    Z: −3  └─────┴─────┴═════════════┴─────┴─────┘  STERN

    Center (X −1..+1, Z −1..+1): Standard Large Cargo Container
    Cross wings (N, S, E, W):   4× Large Flat Atmo Thrusters (Center of Lift = 0, 0)
    Corners (4 pockets):        4× Large Batteries + Gyros + Avionics
    Outer rub rails (X ±3):     Waist clamp contact zones for Gantry Cradle
```

#### Master Elevation (Looking Starboard, X = 0)

```
        Z:  −3 (Stern)        −1         0        +1         +3 (Bow)
    L3      ┌──────────────────┬───────────────────┬──────────────────┐  Roof
            │  [Small Atmos]   │ Large Cargo (top) │   [Small Atmos]  │
    L2      ├──────────────────┤ (X −1 to +1)      ├──────────────────┤  Midline
            │   Lift Aft (D)   │ Large Cargo (mid) │   Lift Fwd (D)   │
    L1      └───┬──────────┬───┴─────────┬─────────┴───┬──────────┬───┘  Belly
                ▼▼▼        │  [MUL Dock] │             ▼▼▼        │
              Nozzle       │   (Belly ↓) │           Nozzle       │
                           └─────────────┘
```

---


#### 0.4 · Master Conveyor Routing Schematic

The Mule features a zero-sorter, ultra-low resistance direct extraction loop. Pumping is handled entirely by external pad sorters (`ORC HD Drain` on the carrier and `GND PAD` intake on the base):

```
                   [MUL Cargo Hold]
           (Standard Large Cargo Container 3×3×3 B)
           (Located X −1..+1, Z −1..+1, Levels L1-L3)
           (Hold Capacity: 1,386,000 L @ 3× Inventory)
                         │
                         ▼ (Bottom Center Large Port)
                  [MUL Dock Belly]
           (Large Grid Connector · Faces Straight DOWN)
           (Flush L1 @ X 0, Z 0 · Recessed 0.5 B above Skids)
                         │
        ═════════════════╪═════════════════  Landing Plane
                         │
                         ▼ (Mates to Pad)
         [ORC MP Conn]       OR       [GND PAD Conn]
         (Orca Roof Deck)             (Ground Base Tarmac)
```

---

#### 0.5 · Master Electrical Single-Line Schematic

```
               POWER STORAGE (4× Large / Warfare Batteries)
                    12.0 MWh Capacity · 48.0 MW Max Output
                                 │
                                 ▼
                         PRIMARY POWER BUS
        ┌────────────────────────┼────────────────────────┐
        │                        │                        │
        ▼                        ▼                        ▼
  LIFT PROPULSION          MANEUVER ARRAY           AVIONICS & CONTROL
  4× Large Flat Atmo       10× Small Atmo           2× Large Gyros (0.8 MW)
  (26.8 MW Peak Burst)     (6.48 MW Peak Burst)     1× RC + 1× PB (0.05 MW)
  [Empty Hover: 3.8 MW]    [Braking load: 2.6 MW]   Antenna + 2× Cams (0.02 MW)
  [Laden Hover: 20.8 MW]
```

---

#### 0.6 · Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Sizes | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Chassis & Hold** | 1× Large Cargo (3×3×3 B), Heavy Armor skids | 49 | 48.0 t | 450 |
| **Lift Propulsion** | 4× Large Flat Atmo (3×2 B), 10.4 MN total | 4 | 44.0 t | 320 |
| **Maneuver Array** | 10× Small Atmo (large grid, 6-axis control) | 10 | 18.0 t | 250 |
| **Power Core** | 4× Large Battery (large grid, 3 MWh each) | 4 | 20.0 t | 80 |
| **Attitude & Gyros** | 2× Large Gyroscopes (67.2 MN·m torque) | 2 | 5.6 t | 100 |
| **Avionics & Logic** | 1× RC, 1× PB, 1× Antenna, 2× Timers, 3× ECs, 2× Cams | 10 | 2.4 t | 180 |
| **Docking Interface**| 1× Connector (down), 12× Heavy Armor clamp rails | 13 | 4.8 t | 170 |
| **TOTAL (DRY)** | Complete Airframe (MULE-01) | **92** | **≈ 142.8 t** | **1,550** |

---


#### 0.7 · Human Builder Guide · Visual Paint Palette & CoM Balance HUD

The Mule's primary flight safety relies on **100% symmetrical mass distribution**. Any off-center block placement creates tip-torque that exhausts gyro authority when carrying 650 tonnes of ore. Use this in-game paint palette (`[P]` key) to visually verify 4-quadrant balance:

```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ MULE CONSTRUCTION PAINT PALETTE (Recommended Paint System)                           │
├─────────────────────┬──────────────┬───────────────┬─────────────────────────────────┤
│ Functional Role     │ Paint Finish │ In-Game Color │ Purpose / Visual Cue            │
├─────────────────────┼──────────────┼───────────────┼─────────────────────────────────┤
│ Central Cargo Core  │ Battered     │ Charcoal Gray │ Defines the 3×3×3 B payload anchor
│ Cardinal Lift Wings │ Clean        │ Pure White    │ Visually checks N/S/E/W lift symmetry
│ Lift Nozzle Trim    │ Battered     │ Safety Red    │ Highlights downward exhaust boundaries
│ Waist Clamp Rails   │ Hazard       │ Orange Stripe │ Marks exact Gantry Cradle strike zone (L2)
│ Corner Battery Bays │ Heavy Armor  │ Cobalt Blue   │ Identifies 4 corner power pockets
│ Upper Avionics Deck │ Heavy Armor  │ Fleet Navy    │ Shelters RC, PB, and safety timers (L3)
└─────────────────────┴──────────────┴───────────────┴─────────────────────────────────┘
```

##### CoM Balance HUD & Quadrant Alignment
Every assembly step includes an exact quadrant symmetry check:
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: <Name>  ·  Footprint: <X −3..+3, Z −3..+3>  ·  Level: <L1-L3>   │
│ [BALANCE] Target CoM: (X: 0.00, Z: 0.00)  ·  4-Quadrant Symmetrical Check: REQUIRED │
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

#### 0.8 · Gantry Cradle Docking Shadow & Pad Alignment Guide

The Mule is built to land interchangeably on the **Orca roof MP** and the **Ground Base Gantry Pad**. Both pads share the identical 7 × 7 B footprint:

```
Top-Down Landing Shadow & Cradle Alignment (Level L1 Belly):
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3  [SKID] ··························· [SKID]  ◄── Corner Landing Skid
           │                                      │
    Z: +1  │  ┌────────────────────────────────┐  │
        0  │  │ [MAG P] ◄── [0, 0] ──► [MAG S] │  │  ◄── Cradle Clamp Strike Zone
    Z: −1  │  └───────── [MUL Dock] ───────────┘  │       (Belly Connector at Center)
           │                                      │
    Z: −3  [SKID] ··························· [SKID]  ◄── Corner Landing Skid
```
* **Landing Clearance:** Belly connector (`MUL Dock Belly`) sits recessed 0.5 B (1.25 m) above the bottom face of the corner skids.
* **Clamp Grip:** Gantry Cradle magnetic plates swing inward by 30° at X = ±3 to latch onto the flush orange hazard rub rails at L2.

---

### Phase 1 · Central Ore Hold & Belly Dock (L1-L3)

The payload hold forms the structural and geometric center of the ship.

#### Step 1 · Large Cargo Container & Belly Port (L1-L3)

Place 1 standard vanilla Large Cargo Container (3×3×3 B) at the exact grid center. Attach the primary belly docking connector to its bottom center port.

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 1 PARTS TRAY & CONVEYOR PORT ORIENTATION GUIDE                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Conveyor Port Orientation       │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo (3×3×3) │ 1   │ 28.5 t / 250  │ Centered at X0, Z0, L1-L3; 6 ports flush │
│ Connector (Large)   │ 1   │  4.2 t / 150  │ Bottom port facing straight DOWN at L1   │
│ Camera              │ 1   │ 0.05 t / 10   │ Facing straight DOWN at X0, Z+1, L1      │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!TIP] ZERO CoM OFFSET ALIGNMENT
> The Large Cargo Container's geometric center must coincide exactly with grid coordinate (0, 0).
> Placing the bottom connector at (0, 0) ensures that all payload weight passes directly
> through the geometric center of lift.


```
Plan view, Y = L1 (Belly Port)

    X (sb): −1     0    +1
    Z: +1  ┌─────┬─────┬─────┐
           │     │     │     │
        0  │     │[CON]│     │   ◄── Connector facing straight DOWN
           │     │     │     │
       −1  └─────┴─────┴─────┘
                 (3×3 B)

    [CON] = Large Grid Connector (`MUL Dock Belly`, facing DOWN at X0, Z0, L1)
```

**Blocks:**
* 1 × Large Cargo Container (3×3×3 B) · `MUL Cargo Hold` (centered at X 0, Z 0, L1-L3)
* 1 × Connector (large grid) · `MUL Dock Belly` (facing DOWN at X 0, Z 0, flush at L1)
* 1 × Camera · `MUL Cam Belly` (facing DOWN, recessed beside connector at X 0, Z +1)

**✓ Check:** Cargo container sits symmetrically between X −1..+1 and Z −1..+1. Connector faces down, centered exactly at (0, 0).

---


> [!IMPORTANT] PRE-ENCLOSURE GATE 1 · CONNECTOR RECESSED DEPTH CHECK
> Before completing the perimeter frame in Step 2:
> 1. Stand at ground level (L0) and sight across the bottom edges of the 4 corner skids.
> 2. Verify `MUL Dock Belly` sits recessed exactly 0.5 blocks (1.25 m) above the skid contact line.
> 3. If the connector hangs flush with or below the skids, touchdown on raw planetary voxels
>    will crush the connector!

#### Step 2 · Belly Landing Skids & Perimeter Frame (L1)

Build four heavy-armor landing skids around the base of the central cargo container to protect the belly connector when landing on raw terrain.

```
Plan view, Y = L1 (Skid Foundation)

    X: −3   −2   −1    0   +1   +2   +3
    Z: +3  [SKID]···                 ···[SKID]
           ······   ┌─────────────┐   ······
        0           │  MUL Cargo  │
           ······   └─────────────┘   ······
    Z: −3  [SKID]···                 ···[SKID]

    [SKID] = 2×1 Heavy Armor Slope / Block forming 4 corner landing feet
```

**Blocks:**
* 16 × Heavy Armor Block / Slope (large grid) · corner skids at X ±3, Z ±3
* 8 × Beam Block (straight) · connecting under-chassis ties

**✓ Check:** Belly connector sits 0.5 B recessed above the lowest edge of the corner skids, preventing ground impact damage.

---


#### Phase 1 Visual Milestone · What Your Grid Looks Like Now

```
Standing 15 m away looking at Starboard Flank (Level L1-L3):
       L3  ┌───────────────────────────────────┐  ◄── Large Cargo Container Top
           │                                   │
       L2  │       LARGE CARGO CONTAINER       │      (3×3×3 B Center Core)
           │                                   │
       L1  └──[SKID]───────[BELLY]───────[SKID]┘  ◄── Corner Landing Skids
   Visual Sanity Check: A central 3×3×3 B cargo cube sitting balanced on 4 corner
   heavy-armor landing feet. The downward docking connector sits tucked underneath.
```

---

### Phase 2 · Symmetrical Lift Propulsion (L1-L2)

Four Large Flat Atmospheric Thrusters mounted in a cross pattern around the hold. Center of Lift sits precisely over (0, 0), eliminating all pitch/roll torque regardless of cargo load.

#### Step 3 · Cardinal Lift Array (L1-L2)

#### 2.5D Isometric Callout · Symmetrical Cardinal Lift Cross (L1-L2)

```
                       Bow Wing (Z = +2 to +3)
                       ┌═════════════════════┐
                       │    MUL Lift Fwd     │
                       │    (3×2 Flat)       │
                       └══════════╤══════════┘
                                  │
       Port Wing                  ▼                  Starboard Wing
  ┌═════════════════┐    ┌─────────────────┐    ┌═════════════════┐
  │  MUL Lift Port  │◄───┤    MUL Cargo    ├───►│  MUL Lift Stbd  │
  │  (2×3 Flat)     │    │   (3×3×3 B)     │    │  (2×3 Flat)     │
  └═════════════════┘    └─────────────────┘    └═════════════════┘
                                  ▲
                                  │
                       ┌══════════╧══════════┐
                       │    MUL Lift Aft     │
                       │    (3×2 Flat)       │
                       └═════════════════════┘
                       Stern Wing (Z = −2 to −3)

   Center of Lift = (0, 0)  ·  Center of Mass (Empty/Laden) = (0, 0)
   Tip Torque = 0.00 MN·m across all cargo fill levels!
```


Install four Large Flat Atmospheric Thrusters (large grid, 10.4 MN total lift). All exhaust points straight down (D).

```
Plan view, Y = L2 (Lift Deck)

    X: −3    −1     0    +1    +3
    Z: +3   ┌═════════════┐
            │   LIFT FWD  │         (Forward wing: Z +2 to +3, X −1 to +1)
    Z: +1  ┌┤  (3×2 Flat) ├┐
           │├─────────────┤│
        0  ││ LARGE CARGO ││        (Port wing: X −2 to −3, Z −1 to +1)
           ││  (3×3×3 B)  ││        (Stbd wing: X +2 to +3, Z −1 to +1)
    Z: −1  │├─────────────┤│
    Z: −3  └┤  LIFT AFT   ├┘
            │  (3×2 Flat) │         (Aft wing: Z −2 to −3, X −1 to +1)
            └═════════════┘
```

**Blocks:**
* 4 × Large Flat Atmospheric Thruster (large grid, 2.6 MN each = 10.4 MN total):
  * `MUL Lift Fwd` (Mounted at Z +2, centered on X 0)
  * `MUL Lift Aft` (Mounted at Z −2, centered on X 0)
  * `MUL Lift Port` (Mounted at X −2, centered on Z 0)
  * `MUL Lift Stbd` (Mounted at X +2, centered on Z 0)
  * Group → `MUL Lift`

**✓ Check:** All four thrusters exhaust downward. Center of lift coincides exactly with the physical center of the cargo container.

---


#### Phase 2 Visual Milestone · What Your Grid Looks Like Now

```
Looking Down from Above (Top Plan View, Level L2):
                       Bow: [LIFT FWD (3×2)]
                                 │
     Port: [LIFT PORT (2×3)] ◄──[HOLD]──► Stbd: [LIFT STBD (2×3)]
                                 │
                      Stern: [LIFT AFT (3×2)]
   Visual Sanity Check: A bold, perfectly symmetrical white plus sign (+) forming
   the cardinal lift wings around the central cargo core.
```

---

### Phase 3 · Power Core & Gyros (L1-L3)

Four corner pockets (2 × 2 B each) house the batteries and flight stabilization gyroscopes.

#### Step 4 · Corner Battery Array (L1-L3)

Per **Decision 44**, install 4 Large Batteries (pure electric, 12 MWh total, 48 MW peak).

```
Plan view, Y = L2 (Power Pockets)

    X: −3   −2   −1     0    +1   +2   +3
    Z: +3  [BAT 1]   [ LIFT FWD  ]   [BAT 2]
    Z: +2  [ 3 MWh]                  [ 3 MWh]
           ·······   ┌───────────┐   ·······
        0  [LIFT P]  │ MUL Cargo │   [LIFT S]
           ·······   └───────────┘   ·······
    Z: −2  [BAT 3]   [ LIFT AFT  ]   [BAT 4]
    Z: −3  [ 3 MWh]                  [ 3 MWh]
```

**Blocks:**
* 4 × Large Battery (or Warfare Battery, large grid, 3 MWh, 12 MW each):
  * `MUL Battery NW` (Fwd-Port corner)
  * `MUL Battery NE` (Fwd-Stbd corner)
  * `MUL Battery SW` (Aft-Port corner)
  * `MUL Battery SE` (Aft-Stbd corner)
  * Group → `MUL Batteries` (12 MWh capacity, 48 MW max discharge)

**✓ Check:** Peak lift power draw is 26.8 MW; battery bank supplies 48 MW (179% coverage margin).

---


> [!IMPORTANT] PRE-ENCLOSURE GATE 2 · 4-BATTERY RECHARGE CIRCUIT TEST
> Before placing the upper light armor deck at L3 in Step 6:
> 1. Connect a temporary power line to `MUL Dock Belly`.
> 2. Open terminal and verify all 4 corner batteries (`MUL Battery NW, NE, SW, SE`) switch 
>    to Recharge mode simultaneously.
> 3. Verify total recharge input reads 48 MW across the bank.
> Once the roof cowlings are welded, accessing battery terminals requires cutting armor!

#### Step 5 · Flight Stabilization Gyroscopes (L2)

Install 2 Large Gyroscopes in the forward corner equipment bays.

```
Plan view, Y = L2 (Forward Corner Bays)

    X: −2         +2
    Z: +3  [GYRO 1] ···· [GYRO 2]
```

**Blocks:**
* 2 × Gyroscope (large grid, 33.6 MN·m each = 67.2 MN·m) · `MUL Gyro Port`, `MUL Gyro Stbd` → group `MUL Gyros`

**✓ Check:** Total torque (67.2 MN·m) provides agile attitude control up to maximum laden mass (816 t).

---

### Phase 4 · Maneuvering Array & Downward Thrust (L2-L3)

Install small atmospheric thrusters on all 4 faces and the roof to provide complete 6-axis translation and braking control (addressing legacy sprue finding F1).

#### Step 6 · 6-Axis Small Atmo Array (L2-L3)

#### 2.5D Isometric Callout · 6-Axis Braking Array & Push-Down Thrusters

```
        Y (Up)
          ▲
      L3  │     [MUL Thrust Down 1, 2] ◄── Pointing UP (Pushes DOWN)
          │            ■═══════■
          │            ║       ║
      L2  │  [Lat P]   ║ HULL  ║   [Lat S]   ◄── Lateral Face Thrusters
          │  (Push S)  ║       ║   (Push P)      (Push Port & Starboard)
      L1  │            ■═══════■
          └────────────────────────────────► X / Z
              [Lat Aft] (Push Fwd) · [Lat Fwd] (Push Aft)
```


Mount small atmospheric thrusters (large grid, 648 kN each) across the outer perimeter.

```
Plan view, Y = L3 (Roof Line Perimeter)

           Bow (Z = +3)
        ◄── [MUL Lat Fwd ×2] ──►   (Push Aft / Exhaust Forward)
     ┌────────────────────────────┐
     │                            │
Port │ [MUL Lat Port ×2]          │ Stbd
     │ (Push Stbd / Exhaust Port) │ (Push Port / Exhaust Stbd)
     │                            │ [MUL Lat Stbd ×2]
     │                            │
     └────────────────────────────┘
        ◄── [MUL Lat Aft ×2] ──►   (Push Fwd / Exhaust Aft)
           Stern (Z = −3)

Roof deck: 2× Small Atmo pointing UP (`MUL Thrust Down ×2`, pushes down)
```

**Blocks:**
* 2 × Small Atmospheric Thruster · `MUL Lat Fwd 1, 2` (facing Fwd) → group `MUL Lat F`
* 2 × Small Atmospheric Thruster · `MUL Lat Aft 1, 2` (facing Aft) → group `MUL Lat A`
* 2 × Small Atmospheric Thruster · `MUL Lat Port 1, 2` (facing Port) → group `MUL Lat P`
* 2 × Small Atmospheric Thruster · `MUL Lat Stbd 1, 2` (facing Stbd) → group `MUL Lat S`
* 2 × Small Atmospheric Thruster · `MUL Thrust Down 1, 2` (facing UP, exhausts up to push DOWN) → group `MUL Push Down`

**✓ Check:** Symmetric lateral braking on all horizontal axes (1.3 MN per face); downward thrust assists rapid descent without relying on gravity alone.

---


#### Phase 4 Visual Milestone · What Your Grid Looks Like Now

```
Standing at Ground Level looking at Port Flank (X = −3):
       L3  ┌───────────────────────────────────┐  ◄── Roof Small Atmo Array
           │                                   │
       L2  ╞═══════════════════════════════════╡  ◄── FLUSH ORANGE STRIKE RAIL
           │ Heavy Armor Clamp Rub Rail (3 B)  │      (Gantry Cradle Locks Here)
       L1  └─────[SKID]─────────────[SKID]─────┘  ◄── Skid Base Line
   Visual Sanity Check: The ship now forms a complete 7 × 7 B square slab (17.5 m).
   Both Port and Starboard flanks show a flat, flush 3-block-wide orange hazard rail
   ready for Gantry Cradle magnetic clamping.
```

---

### Phase 5 · Waist Clamp Rub Rails (L2)

Per **Decision 45**, the Mule carries zero mechanical docking arms. Instead, the landing pads (Orca MP and Ground Base) feature motorized Gantry Cradles with Magnetic Plates that clamp the Mule's chassis on touchdown.

#### Step 7 · Heavy Armor Clamp Rub Rails (L2)

#### 2.5D Isometric Callout · Waist Clamping Strike Rails (L2, X ±3)

```
                       [Mule 7×7 B Fuselage]
                                 │
    Port Flank (X = −3)          ▼           Starboard Flank (X = +3)
  [HEAVY ARMOR RAIL]                       [HEAVY ARMOR RAIL]
  (3 B span, Z −1..+1)                     (3 B span, Z −1..+1)
         ▲                                        ▲
         │                                        │
    [MAG PLATE] ◄── 30° Swing Inward ──► [MAG PLATE]
         │                                        │
    [HINGE ARM]                              [HINGE ARM]
  ═══════╧════════════════════════════════════════╧═══════
                 GANTRY CRADLE (ORCA MP / GND PAD)
```


Mount continuous heavy-armor plates along the center section of both outer flanks (Port and Starboard at X = ±3).

```
Section view, looking Bow at Port flank (X = −3)

    L3   ┌───────────────────────────┐
         │ Light Armor Cowling       │
    L2   ╞═══════════════════════════╡  ◄── [CLAMP ZONE PORT]
         │ Heavy Armor Rub Rail      │      Flush strike surface for
    L1   ╞═══════════════════════════╡      Gantry Cradle Magnetic Plate
         │ Skid Base Line            │
         └───────────────────────────┘
```

**Blocks:**
* 6 × Heavy Armor Full Block (large grid) · 3 B span at X = −3, Z −1 to +1 (Port)
* 6 × Heavy Armor Full Block (large grid) · 3 B span at X = +3, Z −1 to +1 (Starboard)

**✓ Check:** Completely flush 7.5 m flat strike surface on both flanks at L2. Gantry Cradle clamps engage without snagging thruster grills.

---

### Phase 6 · Avionics, Navigation & Comms (L3)

The brain of the Mule sits sheltered on the upper deck at L3.

#### Step 8 · Autonomous Avionics Core (L3)

Mount Remote Control, Programmable Block, Antenna, and forward navigation sensors.

```
Plan view, Y = L3 (Aft Equipment Bay)

    X: −2     0    +2
    Z: −3  [PB] [RC] [ANT]
```

**Blocks:**
* 1 × Remote Control (large grid) · `MUL RC` (Forward = +Z / Bow, Up = +Y / Roof)
* 1 × Programmable Block (large grid) · `MUL PB` (Ferry navigation & logistics state machine)
* 1 × Antenna (large grid) · `MUL Antenna` (Range set to 5,000 m† for Orca $\leftrightarrow$ Base relay)
* 1 × Camera · `MUL Cam Fwd` (facing forward at Bow, Z = +3, L3)

**✓ Check:** Remote Control orientation verified; antenna covers transit corridor between deposit and refinery base.

---

### Phase 7 · Control Logic & Hardware Failover

#### Step 9 · Safety Timers & Logic Mapping

```
Logic Block Array (Aft Bay, L3):
  [MUL TMR Heartbeat] · [MUL TMR Return] · [MUL EC Batt] · [MUL EC Cargo] · [MUL EC Dock]
```

**Blocks:**
* 2 × Timer Block: `MUL TMR Heartbeat` (5.0 s), `MUL TMR Return` (2.0 s)
* 3 × Event Controller: `MUL EC Batt` (monitors battery), `MUL EC Cargo` (monitors payload mass), `MUL EC Dock` (monitors belly connector)


#### Step 9.1 · Master AI Scripting Contract & Terminal Namespace (Claude Code)

Every automated device on MULE-01 conforms to this exact naming and grouping dictionary:

##### 1. Group Taxonomy
* `MUL Lift` : 4× Large Flat Atmospheric Thrusters (`MUL Lift Fwd`, `Aft`, `Port`, `Stbd`). Total thrust = 10.4 MN.
* `MUL Maneuver` : 10× Small Atmospheric Thrusters (All horizontal faces + roof push-down).
* `MUL Batteries` : 4× Large Batteries (`MUL Battery NW`, `NE`, `SW`, `SE`). Total capacity = 12 MWh.
* `MUL Gyros` : 2× Large Gyroscopes (`MUL Gyro Port`, `MUL Gyro Stbd`). Total torque = 67.2 MN·m.
* `MUL Cams` : 2× Cameras (`MUL Cam Belly` at L1, `MUL Cam Fwd` at Bow L3).

##### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MUL EC Cargo` | Grid Total Mass | Mass $\ge 815	ext{ t}$ (Ore Cap 650 t) | Notify Orca via IGC · Start departure sequence | · |
| `MUL EC Batt` | `MUL Batteries` Stored % | Stored $\le 20\%$ | Abort transit · Engage emergency landing | · |
| `MUL EC Dock` | `MUL Dock Belly` State | Connector Locked | `MUL Lift` OFF · `MUL Batteries` $
ightarrow$ Recharge | `MUL Lift` ON · `MUL Batteries` $
ightarrow$ Auto |

##### 3. Native Failover Cascades (Zero-PB Safety)
If `MUL PB` crashes or loses IGC heartbeat:
1. `MUL TMR Heartbeat` (delay 5.0 s per Decision 42) expires without PB pulse.
2. Immediate actions: `MUL Maneuver` overrides OFF · Dampeners ON · Autopilot altitude hold at current position.
3. If grid is docked at Gantry Cradle: `MUL Lift` remains locked OFF to prevent thruster tear-away against magnetic clamps.

##### 4. IGC Broadcast Channels
* Outbound: `TMS.MULE.STATUS`: `mule_id;state_enum;ore_mass_t;battery_pct;destination_gps`
  * Example: `MUL-01;LADEN_TRANSIT;648.5;92.0;GPS:GroundBase:...:;`
* Inbound: `TMS.MULE.CALL`: `requester_id;pad_id;payload_type;urgency`

#### Hardware Automation Matrix

| Block | Condition / Event | Action True | Action False |
|---|---|---|---|
| **`MUL TMR Heartbeat`** | 5.0 s timeout (IGC beat from Orca/Base) | Gyro/Thrust Overrides OFF · Dampeners ON · Autopilot hold | · |
| **`MUL EC Cargo`** | Total Grid Mass ≥ 815 t (Ore Cap 650 t) | Notify Orca via IGC · Trigger departure sequence | · |
| **`MUL EC Batt`** | `MUL Batteries` ≤ 20% | Abort transit · Engage emergency landing profile | · |
| **`MUL EC Dock`** | `MUL Dock Belly` Locked | `MUL Lift` OFF · `MUL Batteries` → Recharge | `MUL Lift` ON · `MUL Batteries` → Auto |

---

### Phase 8 · Commissioning Checklist (RIG Test Creative)

#### A · Mass & Lift Calibration
* [ ] Dry mass verification: terminal reports `≈ 130-150 t`.
* [ ] Empty hover: `MUL Lift` load ≈ 12-14% (Earth 1 g).
* [ ] Max laden test: spawn 650 t stone/iron into `MUL Cargo Hold` (Total mass ≈ 800 t).
* [ ] Laden hover: `MUL Lift` load ≈ 75-78% (T/W ≥ 1.3 confirmed).
* [ ] Zero-torque test: release dampeners in hover; verify pitch and roll remain 0.00° (CoM aligns perfectly with lift center).

#### B · Gantry Cradle & Docking Test
* [ ] Touchdown on Orca MP (roof deck): verify belly connector engages `ORC MP Conn` cleanly.
* [ ] Gantry Cradle engagement: trigger cradle hinge swing (30° inward); verify magnetic plates lock solidly to Mule waist rub rails.
* [ ] Lift cut test: turn `MUL Lift` OFF; verify Mule rests rigidly on cradle without shaking or drift.
* [ ] Ore transfer test: drain 650 t ore from Orca hold into `MUL Cargo Hold`; verify transfer completes in < 45 seconds†.

#### C · Base Terminal Transit
* [ ] Release sequence: `MUL Lift` ON $\rightarrow$ Gantry Cradle clamps release $\rightarrow$ hinges fold flat $\rightarrow$ Mule ascends vertically to 50 m altitude.
* [ ] Fly 2 km transit corridor to Ground Base at 100 m/s: verify lateral thrusters hold zero drift.
* [ ] Ground Base Gantry Cradle touchdown: auto-clamp, connector lock, batteries switch to `Recharge`, ore drains into refinery intake.

---

### Phase 9 · Verify Items (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V37:** Large Flat Atmospheric Thruster large-grid sea-level thrust (verify 2.6 MN rating and power consumption).
* **V38:** Ore transfer drain rate between large-grid containers through a single connector (measure seconds per 650 t).



---

## Chapter 12 · KESTREL-01 Prospector Scout Drone (KES-01)

Source: `docs/rig/chapters/kestrel-instructions.md` · 7 × 9 × 5 sb · High-Speed Recon & Ore Prospector

### 0 · Reference Grid, In-Game Paint Palette & Drone Envelope

#### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the belly docking connector at Level L1.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. (Range: X −3 to +3 sb).
* **Y (Elevation):** Belly landing skids and dock port are Level L1, advancing upward to Avionics Roof at Level L5.
* **Z (Length / Longitudinal):** Stern thruster box is negative (−), Bow sensor nose is positive (+). (Range: Z −4 to +4 sb).

```
Spatial Alignment Triad:
       [+Y: Zenith / Aero Roof L5]
              │
              │   [+Z: Bow / Sensor Nose & Camera]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Main Thrusters & Antenna]
              │
       [−Y: Belly Dock / Ground Skids L1]
```

#### 0.2 In-Game Paint Palette & Functional Color Coding

All structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 8** | Scout Flight White | `#ECF0F1` | Clean White | Aerodynamic outer fuselage, wing sponsons, top roof fairing |
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Belly landing skids, thruster exhaust casings, underbody frame |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Wingtip navigation beacons, dock alignment crosshairs |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Geological Ore Detector pod, forward survey sensor block |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Core (2× Small Grid Batteries, 1× Small Reactor) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Central Avionics Core, Remote Control, Gyro, PB Prospector |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Maintenance grab points, battery charge diagnostic port |

---

#### 0.3 Master Profile Elevation (Looking Starboard, X = 0)

```
        Z:  −4 (Stern)      −2          0          +2        +4 (Bow)
    L5      ┌────────────────┬──────────────────────┬───────────────┐  Roof Fairing
            │ [OMNI ANTENNA] │ Aero Spine Cover     │ [RECON CAM]   │  (Palette 8)
    L4      ├────────────────┼──────────────────────┼───────────────┤
            │ Main Drive Aft │ [PB] [GYRO] [REMOTE] │ [SEARCHLIGHT] │  Avionics Core
    L3      ├────────────────┼──────────────────────┼───────────────┤  (Palette 7)
            │ [REACTOR]      │ [BATTERY 1 & 2]      │ [ORE DETECTOR]│  Power & Sensor
    L2      ├────────────────┼──────────────────────┼───────────────┤  (Palette 4 & 6)
            │ [DAMP THRUSTER]│ [KES DOCK CONN]      │ [GROUND SENS] │  Belly Skid
    L1      └───[SKID P/S]───┴──────[CONN ↓]────────┴────[SKID P/S]─┘  (Palette 0)
```

---

#### 0.4 Master Conveyor & Avionics Schematic

```
                          BOW RECONNAISSANCE HEAD
             [KES Cam Nose] + [KES Ore Detector] + [Searchlights]
                                     │
                                     ▼
                     [KES Remote Control] + [KES PB]
                                     │
                                     ▼
        ┌─────────────────────────────────────────────────────────────┐
        │ POWER & INTERNAL SERVICE MANIFOLD (Level L2)                │
        │ 2× Small Batteries + 1× Small Reactor + 1× Small Cargo Hold │
        └──────────────────────────────┬──────────────────────────────┘
                                       │
                                       ▼ (Direct Belly Port)
                            [KES Dock Conn] (Level L1)
                                       │
                                       ▼ (Offloads Data & Recharges)
                  [MASTODON SCOUT PAD] / [ORCA BAY P2]
```

---

#### 0.5 Master Electrical & Thrust Budget

* **Power Generation**: 2× Small Grid Battery (2.0 MW max discharge, 1.0 MWh capacity) + 1× Small Reactor (0.5 MW continuous backup) = **2.5 MW Peak Power**.
* **Propulsion Power Draw**:
  * 4× Downward Lift Thrusters: 4 × 80 kN = 320 kN (Draw: 4 × 560 kW = 2.24 MW at 100% burst).
  * 2× Forward Thrusters (Aft-facing): 2 × 80 kN = 160 kN.
  * 2× Reverse Thrusters (Bow-facing): 2 × 80 kN = 160 kN.
  * 6× Maneuver Thrusters (Port/Stbd/Zenith): 6 × 80 kN = 480 kN total authority.
  * Cruise Hover Draw at Earth 1 g: 47.1 kN required / 320 kN available ≈ **14.7% Throttle (330 kW)**.
  * Cruise Endurance: $> 3.0\text{ hours}$ continuous flight on battery power alone.

---

#### 0.6 Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Fuselage & Skids** | Small armor blocks, half-blocks, slopes (Palette 8 & 0) | 38 | 1.8 t | 38 |
| **Atmospheric Thrusters**| 14× Small Atmospheric Thrusters (small grid) | 14 | 1.4 t | 210 |
| **Power Plant** | 2× Small Grid Batteries, 1× Small Reactor (Palette 6) | 3 | 0.8 t | 85 |
| **Avionics & Automation**| 1× Remote Control, 1× Gyro, 1× PB, 1× Event Controller | 4 | 0.3 t | 95 |
| **Sensors & Recon** | 1× Ore Detector, 2× Cameras, 2× Searchlights, 1× Sensor | 6 | 0.3 t | 140 |
| **Comms & Docking** | 1× Radio Antenna (10 km), 1× Small Connector, 1× Cargo | 3 | 0.2 t | 65 |
| **TOTAL (DRY)** | Complete Scout Drone (KESTREL-01) | **68** | **≈ 4.8 t** | **633** |

---

### Phase 1 · Belly Docking Skid & Sensor Foundation (Levels L1-L2)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 9 L × 2 H sb (3.5 × 4.5 × 1.0 m)
       Coordinate Range: X −3 to +3, Z −4 to +4, Y = L1 to L2
       Key Datums:
       · Belly Docking Connector: [X0, Z0, Y = L1] (Faces Downward −Y)
       · Ore Detector Pod: [X0, Z +2, Y = L2]
       · Port Landing Skid: X = −2, spanning Z −3 to +3
       · Starboard Landing Skid: X = +2, spanning Z −3 to +3
```

#### Step 12.1 · Belly Connector & Twin Landing Skids (L1)

Construct the reinforced lower landing skids using heavy armor half-blocks at X = ±2 sb. Mount the small grid docking connector at the center origin (X0, Z0, L1) facing downward (−Y) to mate with the carrier landing pads.

```
Plan view, Y = L1 (Belly Skids & Connector)
        X: −2   −1    0   +1   +2
    Z: +3  [SKID P] ······ [SKID S]  ◄── Bow Skid Contact Points (Palette 0: Slate)
           │                  │
    Z:  0  │      [CONN ↓]    │      ◄── Small Grid Connector (`KES Dock Conn`)
           │    (Facing Down) │          (Centered X0, Z0, L1)
    Z: −3  [SKID P] ······ [SKID S]  ◄── Stern Skid Contact Points
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.1 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Armor Half-Blk│ 12  │ 0.48 t / 12   │ Palette Slot 0 (Slate); skid base rails  │
│ Connector (Small)   │ 1   │ 0.16 t / 25   │ `KES Dock Conn` (Centered X0, Z0, face −Y│
│ Conveyor Junction sb│ 2   │ 0.08 t / 10   │ Center conveyor routing above connector  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 12.2 · Geological Ore Detector & Downward Lift Cluster (L1-L2)

Mount the small-grid Ore Detector in the forward belly bay (X0, Z +2) and install the 4 vertical atmospheric lift thrusters into the skid recesses.

```
Plan view, Y = L1-L2 (Lift Thrusters & Sensor Mount)
        X: −2   −1    0   +1   +2
    Z: +3  [LIFT 1] ────── [LIFT 2]  ◄── Forward Downward Thrusters (Face −Y)
                    [ORE DET]        ◄── Ore Detector (Palette Slot 4: Teal)
    Z:  0         [CONN ↓]
    Z: −3  [LIFT 3] ────── [LIFT 4]  ◄── Aft Downward Thrusters (Face −Y)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.2 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Operating Settings         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Ore Detector (sb)   │ 1   │ 0.12 t / 15   │ `KES Ore Detector` (Range: 50 m max)     │
│ Small Atmo Thruster │ 4   │ 0.40 t / 60   │ `KES Thruster Lift 1..4` (Face DOWN −Y)  │
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `KES Cam Ground` (Downward docking align)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 12.1A: DOCKING CLEARANCE & SENSOR CONDUIT**
> Before building Level L2 decks:
> 1. Ensure `KES Dock Conn` sits flush with the skid baseline (0.0 to 0.1 m clearance) so it locks effortlessly when skids rest on landing pads.
> 2. Verify `KES Ore Detector` conveyor pass-through connects directly to the internal cargo trunk.
> 3. Verify all 4 lift thrusters have a clear 1 sb exhaust gap beneath the nozzles.

---

##### Visual Milestone 12-1 · Belly Skid & Sensor Pod Complete

```
Visual Checkpoint Silhouette (Looking Starboard at Belly Skid):
              [ORE DETECTOR L2]       [CONVEYOR HUB]
                     \                      /
     ═══════[LIFT 1]──[SKID RUNNER]──[LIFT 3]═══════ (Level L1)
                             │
                      [KES DOCK CONN ↓]
```

* [ ] Skids rest completely stable on level decks without wobbling.
* [ ] Dock connector engages lock state immediately upon touching a carrier pad.
* [ ] Ore detector active scan confirms zero local grid interference.

---

### Phase 2 · Power Core & Propulsion Ring (Levels L2-L3)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 9 L × 2 H sb (3.5 × 4.5 × 1.0 m)
       Coordinate Range: X −3 to +3, Z −4 to +4, Y = L2 to L3
       Key Datums:
       · Battery Bank: X = ±1, Z 0, Y = L2
       · Small Reactor: X0, Z −2, Y = L2
       · Main Forward Thrusters: X = ±2, Z −4, Y = L3 (Face Aft)
       · Reverse Thrusters: X = ±2, Z +3, Y = L3 (Face Bow)
```

#### Step 12.3 · Power Vault & Fuel Storage (L2)

Install 2 Small Grid Batteries and 1 Small Nuclear Reactor directly above the conveyor junction cubes. Add 1 Small Cargo Container for spare uranium and emergency geological core samples.

```
Plan view, Y = L2 (Power & Internal Hold)
        X: −1    0   +1
    Z: +1  ┌───────────┐
           │[CARGO HOLD│  ◄── Small Cargo Container (Centered X0, Z +1)
    Z:  0  │[BAT 1][BAT│  ◄── 2× Small Batteries (Palette Slot 6: Red)
    Z: −2  │ [REACTOR] │  ◄── 1× Small Nuclear Reactor (15 kW / 0.5 MW burst)
           └───────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.3 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Capacity & Performance            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Grid Battery  │ 2   │ 0.50 t / 40   │ `KES Battery 1, 2` (1.0 MWh total cap)   │
│ Small Reactor (sb)  │ 1   │ 0.15 t / 25   │ `KES Reactor` (0.5 MW emergency backup)  │
│ Small Cargo Box     │ 1   │ 0.10 t / 10   │ `KES Hold Samples` (Sample ore cache)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 12.4 · 10-Thruster Omnidirectional Maneuver Ring (L2-L3)

Mount the 10 horizontal and vertical maneuvering thrusters into the fuselage outriggers:
* 2× Forward Thrusters (facing Aft −Z at Z −4): Primary cruise drive (160 kN).
* 2× Reverse Thrusters (facing Bow +Z at Z +3): High-speed braking (160 kN).
* 2× Port Thrusters (facing Port −X at X −3): Lateral translation (160 kN).
* 2× Starboard Thrusters (facing Stbd +X at X +3): Lateral translation (160 kN).
* 2× Down-Push Thrusters (facing Zenith +Y at Z 0): Inverted terrain huggers (160 kN).

```
Plan view, Y = L3 (Thruster Vector Alignment)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3       [REV 1]           [REV 2]       ◄── Facing Bow +Z (Braking)
    Z: +1  [PORT 1]                   [STBD 1]  ◄── Facing Outward (Lateral)
    Z: −1  [PORT 2]                   [STBD 2]
    Z: −4       [FWD 1]           [FWD 2]       ◄── Facing Aft −Z (Main Drive)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.4 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Vector Orientation & Role                │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Atmo Thruster │ 10  │ 1.00 t / 150  │ `KES Thruster Fwd/Rev/Port/Stbd/Zenith`  │
│ Small Armor Slopes  │ 16  │ 0.32 t / 16   │ Aerodynamic outrigger fairing frames     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 12.2A: THRUSTER AUTHORITY & POWER BUDGET**
> 1. Verify all 14 atmospheric thrusters respond correctly to manual flight override in all 6 degrees of freedom.
> 2. Confirm total power draw under 100% all-thruster burst does not exceed 2.4 MW (covered by 2× Batteries + Reactor).
> 3. Verify zero thruster exhaust impinges on armor blocks within 2 sb of nozzle exits.

---

### Phase 3 · Avionics Core & Aerodynamic Shell (Levels L3-L5)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 9 L × 3 H sb (3.5 × 4.5 × 1.5 m)
       Coordinate Range: X −3 to +3, Z −4 to +4, Y = L3 to L5
       Avionics Stack:
       · Remote Control: [X0, Z +1, Y = L3] (Faces Bow +Z, Up +Y)
       · Micro Gyroscope: [X0, Z 0, Y = L3]
       · PB Prospector: [X0, Z −1, Y = L3]
       · Long-Range Antenna: [X0, Z −3, Y = L4]
       · Forward Recon Camera: [X0, Z +4, Y = L4]
```

#### Step 12.5 · Autonomous Avionics Rack & Sensors (L3-L4)

Install the flight control avionics, autopilot brain, and forward reconnaissance optics along the vehicle centerline.

```
Elevation, looking West through Centerline (X0, Y = L3-L5):
       L5  ═════════[AERO SPINE HOOD]═════════  Roof Fairing (Palette 8: White)
       L4  [ANTENNA]             [CAM NOSE]     ◄── Forward Recon Optics & Comms
       L3  [PB PROSPECT] [GYRO] [REMOTE CTRL]   ◄── Central Avionics Brain (Palette 7)
       L2  [REACTOR]   [BAT 1]   [CARGO HOLD]   ◄── Power & Hold Deck
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.5 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Configuration & Operating Specs          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Remote Control (sb) │ 1   │ 0.05 t / 25   │ `KES Remote Control` (Primary Autopilot) │
│ Gyroscope (sb)      │ 1   │ 0.08 t / 15   │ `KES Gyro` (Roll/Pitch/Yaw Stabilization)│
│ Programmable Block  │ 1   │ 0.08 t / 25   │ `KES PB Prospector` (IGC & Waypoint Nav) │
│ Event Controller    │ 1   │ 0.03 t / 20   │ `KES EC AutoDock` (Failsafe RTH logic)   │
│ Radio Antenna (sb)  │ 1   │ 0.04 t / 25   │ `KES Antenna` (Range: 10,000 m broadcast)│
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `KES Cam Nose` (High-zoom target camera) │
│ Spotlight (Small)   │ 2   │ 0.06 t / 20   │ `KES Light Port / Stbd` (Forward beams)  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 12.6 · Aerodynamic Fuselage Fairing (L4-L5)

Enclose the drone in lightweight sloped armor fairings (Palette Slot 8: Clean White) with amber hazard wingtip markers. The aerodynamic wedge profile reduces drag and shields internal components from dust storms and minor tree collisions.

```
Plan view, Y = L4-L5 (Fuselage Canopy & Wing Fairings)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +4       [LIGHT P] [NOSE CAM] [LIGHT S]   ◄── Tapered Forward Wedge
    Z: +2  [TIP P] ┌─────────────────┐ [TIP S]   ◄── Amber Wingtip Markers (Palette 1)
    Z:  0  │       │  [AERO SPINE]   │       │
    Z: −2  │       │                 │       │
    Z: −4       [AFT]   [ANTENNA]   [AFT]        ◄── Dual Thruster Fairings
```

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 12.3A: AVIONICS ALIGNMENT & SCRIPT INITIALIZATION**
> 1. Verify `KES Remote Control` forward orientation points strictly toward Bow (+Z).
> 2. Check `KES PB Prospector`: confirm script compiles with zero allocations under MDK2 PB rules.
> 3. Verify `KES Antenna` broadcast range is set to `10,000 m` with HUD display name set to `KESTREL-01`.

---

##### Visual Milestone 12-2 · Complete KESTREL-01 Scout Drone Commissioned

```
Visual Checkpoint Silhouette (Looking Port at Complete Drone):
                         [AERO SPINE HOOD (L5)]
                                 │
         [ANTENNA]               │                 [NOSE CAM]
             \                   │                    /
      ════════╧══════════════════╧════════════════════╧═════════
          7 W × 9 L × 5 H sb FAST PROSPECTOR DRONE (KESTREL-01)
                             │
                      [KES DOCK CONN ↓]
```

* [ ] Drone dry mass confirmed: `≈ 4.8 t` in terminal info tab.
* [ ] T/W ratio verified: `≈ 6.8` (provides instant 0-100 m/s acceleration in ~3 seconds).
* [ ] Ore detector highlights all subsurface veins up to 50 m depth directly to the HUD.

---

### Phase 4 · Master Control Logic & Hardware Scripting Contract (Claude Code)

#### Step 12.7 · Terminal Namespace & Group Taxonomy

##### 1. Group Taxonomy
* `KES Thrusters All` : All 14 atmospheric thrusters.
* `KES Thrusters Lift` : 4× Downward lift thrusters (`KES Thruster Lift 1..4`).
* `KES Batteries` : 2× Small Grid Batteries (`KES Battery 1, 2`).
* `KES Lights` : 2× Forward Searchlights + 2× Wingtip Beacons.
* `KES Cameras` : `KES Cam Nose` (forward recon) + `KES Cam Ground` (docking align).

##### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `KES EC Battery` | `KES Batteries` Stored Power | Stored Charge $< 20\%$ | Trigger RTH (Return to Base) | Normal Survey |
| `KES EC Dock` | `KES Dock Conn` State | Connector: Connected | Batteries $\rightarrow$ Recharge · Thrusters OFF | Batteries $\rightarrow$ Auto · Thrusters ON |
| `KES EC VeinDetect`| `KES Ore Detector` | Ore Vein Detected | PB: Save GPS & Broadcast IGC | Continue Search |

##### 3. Inter-Grid Communication (IGC) Protocol (Fleet Recon Bus)

| IGC Tag / Channel | Transceiver | Direction | Message Payload Format | Functional Purpose |
|---|---|---|---|---|
| `TMS.SCOUT.STATUS` | Radio Antenna | Outbound | `drone_id;status;gps_coords;bat_pct` | Real-time position & telemetry to C2 Video Wall |
| `TMS.SCOUT.VEIN` | Radio Antenna | Outbound | `ore_type;depth_m;gps_coords;est_yield`| Transmits verified deposit GPS to Orca & Mastodon |
| `TMS.SCOUT.RECALL` | Radio Antenna | Inbound | `target_carrier_id;pad_id;abort_cmd` | Emergency recall command from C2 Bridge |

---

### Phase 5 · Commissioning Checklist & Acceptance Trials (RIG Test)

#### A · High-G Flight & Dynamics Trials
* [ ] **Vertical Climb Rate:** From stationary hover at sea level, apply 100% upward lift; verify climb rate reaches 100 m/s in $< 2.5\text{ seconds}$.
* [ ] **Terrain Contour Hugging:** Fly at 80 km/h at 10 m altitude over rolling hills; confirm autopilot maintains altitude without ground strike.

#### B · Prospecting & Target Tracking
* [ ] **50 m Vein Acquisition:** Fly over buried Uranium and Iron deposits; confirm `KES Ore Detector` registers both veins and transmits GPS coordinates via `TMS.SCOUT.VEIN`.
* [ ] **Recon Camera Zoom:** Target a terrain feature 2,000 m away; engage `KES Cam Nose` zoom; confirm crisp visual recognition without camera jitter.

#### C · Carrier Recovery & Docking Acceptance
* [ ] **Mastodon Scout Pad Dock:** Autopilot descent onto Mastodon Forward Pad 1 (`MAS Kestrel Dock 1`); verify `KES Dock Conn` locks on first attempt; verify battery switches to Recharge.
* [ ] **Orca Bay P2 Entry:** Approach Orca at hover; enter Bay P2; confirm clearance margin is $\ge 2\text{ B}$ on all sides; verify connector lock and power shutdown.


---

## Chapter 13 · ALBATROSS-01 Utility & Field-Repair Craft (ALB-01)

Source: `docs/rig/chapters/albatross-instructions.md` · 9 × 11 × 5 sb · Autonomous Field Repair & Component Ferry

### 0 · Reference Grid, In-Game Paint Palette & Craft Envelope

#### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the belly docking connector at Level L1.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. (Range: X −4 to +4 sb).
* **Y (Elevation):** Belly landing skids and dock port are Level L1, advancing upward to Avionics Roof at Level L5.
* **Z (Length / Longitudinal):** Stern thruster block is negative (−), Bow welder head is positive (+). (Range: Z −5 to +5 sb).

```
Spatial Alignment Triad:
       [+Y: Zenith / Utility Roof L5]
              │
              │   [+Z: Bow / Welder Head & Work Spotlights]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Main Drive Thrusters & Power]
              │
       [−Y: Belly Dock / Ground Skids L1]
```

#### 0.2 In-Game Paint Palette & Functional Color Coding

All structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | Main utility hull armor, forward tool sponsons, side outriggers |
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Lower skid frames, thruster exhaust cowlings, underbody armor |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Bow welder nose housing, wingtip safety lights, dock alignment marks |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Medium Cargo Container (component buffer hold), conveyor hub |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Core (4× Small Grid Batteries, 1× Small Nuclear Reactor) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Central Avionics Pod, Remote Control, Gyro, PB Utility |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel handrails, tow eyelets, side rub rails |

---

#### 0.3 Master Profile Elevation (Looking Starboard, X = 0)

```
        Z:  −5 (Stern)      −2          0          +2        +5 (Bow)
    L5      ┌────────────────┬──────────────────────┬───────────────┐  Roof Fairing
            │ [OMNI ANTENNA] │ Aero Spine Cover     │ [WORK LIGHTS] │  (Palette 4)
    L4      ├────────────────┼──────────────────────┼───────────────┤
            │ [PB] [GYRO]    │ [REMOTE CONTROL]     │ [WELDER RET]  │  Avionics Core
    L3      ├────────────────┼──────────────────────┼───────────────┤  (Palette 7)
            │ [4× BATTERIES] │ [MEDIUM CARGO HOLD]  │ [SHIP WELDER] │  Cargo & Tool
    L2      ├────────────────┼──────────────────────┼───────────────┤  (Palette 1 & 5)
            │ [LIFT 5..8]    │ [CONVEYOR HUB]       │ [LIFT 1..4]   │  Propulsion Ring
    L1      └───[SKID P/S]───┴──────[CONN ↓]────────┴────[SKID P/S]─┘  (Palette 0)
```

---

#### 0.4 Master Conveyor & Tool Head Flow Schematic

```
                          BOW WORK TOOL HEAD (Level L2-L3)
                [ALB Welder Nose] (Small-Grid Industrial Welder)
                [ALB Cam Welder] (Precision Reticle Target Camera)
                                     │
                                     ▼ (Large Conveyor Pass-Through)
        ┌─────────────────────────────────────────────────────────────┐
        │ MEDIUM CARGO CONTAINER (3×3×3 sb · Centered X0, Z0, L2-L4)  │
        │ Capacity: 3,375 L · Holds Steel Plates, Motors, Computers   │
        └──────────────────────────────┬──────────────────────────────┘
                                       │
         ┌─────────────────────────────┴─────────────────────────────┐
         │ (Internal Fuel Line)                                      │ (Belly Port)
         ▼                                                           ▼
  [ALB Reactor] (0.5 MW)                                    [ALB Dock Conn] (L1, Face ↓)
  (Automated Uranium Pull)                                  (Mates to Orca Bay S2 / Pad)
```

---

#### 0.5 Master Electrical & Thrust Budget

* **Power Generation**: 4× Small Grid Batteries (4.0 MW max burst, 2.0 MWh capacity) + 1× Small Nuclear Reactor (0.5 MW continuous backup) = **4.5 MW Peak Power**.
* **Propulsion Power Draw**:
  * 8× Downward Lift Thrusters: 8 × 80 kN = 640 kN (Draw: 8 × 560 kW = 4.48 MW at 100% burst).
  * 4× Forward Thrusters (Aft-facing): 4 × 80 kN = 320 kN.
  * 4× Reverse Thrusters (Bow-facing): 4 × 80 kN = 320 kN.
  * 6× Maneuver Thrusters (Port/Stbd/Zenith): 6 × 80 kN = 480 kN.
  * Empty Hover Draw at Earth 1 g: 83.4 kN required / 640 kN available ≈ **13.0% Throttle (580 kW)**.
  * Fully Laden Hover Draw (18.5 t): 181.5 kN required / 640 kN available ≈ **28.4% Throttle (1.27 MW)**.
  * Endurance: $> 1.5\text{ hours}$ continuous field welding and transport on battery power.

---

#### 0.6 Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Fuselage & Skids** | Small armor blocks, half-blocks, slopes (Palette 4 & 0) | 58 | 2.8 t | 58 |
| **Atmospheric Thrusters**| 22× Small Atmospheric Thrusters (small grid) | 22 | 2.2 t | 330 |
| **Tool Head** | 1× Ship Welder, 1× Target Camera, 2× Spotlights | 4 | 0.5 t | 165 |
| **Cargo & Logistics** | 1× Medium Cargo (3×3×3 sb), 4× Conveyor Junctions | 5 | 0.6 t | 50 |
| **Power Plant** | 4× Small Grid Batteries, 1× Small Reactor (Palette 6) | 5 | 1.2 t | 125 |
| **Avionics & Automation**| 1× Remote Control, 1× Gyro, 1× PB, 1× Event Controller | 4 | 0.3 t | 95 |
| **Comms & Docking** | 1× Radio Antenna (15 km), 1× Small Connector, 1× Sensor | 3 | 0.2 t | 65 |
| **TOTAL (DRY)** | Complete Field Utility Craft (ALBATROSS-01) | **101** | **≈ 8.5 t** | **888** |

---

### Phase 1 · Belly Docking Skid & Cargo Core (Levels L1-L2)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 9 W × 11 L × 2 H sb (4.5 × 5.5 × 1.0 m)
       Coordinate Range: X −4 to +4, Z −5 to +5, Y = L1 to L2
       Key Datums:
       · Belly Docking Connector: [X0, Z0, Y = L1] (Faces Downward −Y)
       · Medium Cargo Container Center: [X0, Z0, Y = L2-L4]
       · Port Landing Skid: X = −3, spanning Z −4 to +4
       · Starboard Landing Skid: X = +3, spanning Z −4 to +4
```

#### Step 13.1 · Belly Connector & Reinforced Skids (L1)

Construct the reinforced dual landing skids at X = ±3 sb using heavy armor half-blocks. Mount the small grid docking connector at origin (X0, Z0, L1) facing downward (−Y) to mate with the Orca Bay S2 floor dock or ground gantry pads.

```
Plan view, Y = L1 (Belly Skids & Connector)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +4  [SKID P] ················· [SKID S]  ◄── Bow Skid Contact Points (Palette 0)
           │                             │
    Z:  0  │           [CONN ↓]          │      ◄── Small Grid Connector (`ALB Dock Conn`)
           │        (Facing Down)        │          (Centered X0, Z0, L1)
    Z: −4  [SKID P] ················· [SKID S]  ◄── Stern Skid Contact Points
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.1 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Armor Half-Blk│ 16  │ 0.64 t / 16   │ Palette Slot 0 (Slate); skid base rails  │
│ Connector (Small)   │ 1   │ 0.16 t / 25   │ `ALB Dock Conn` (Centered X0, Z0, face −Y│
│ Conveyor Junction sb│ 4   │ 0.16 t / 20   │ Large-port conveyor routing cubes        │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 13.2 · Medium Cargo Container & 8-Thruster Lift Array (L1-L2)

Mount 1 Medium Cargo Container (3×3×3 sb) directly above the belly connector. Install 8 vertical atmospheric lift thrusters into the port and starboard skid outriggers (4 port, 4 starboard) facing downward (−Y).

```
Plan view, Y = L1-L2 (Lift Thrusters & Cargo Core)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3  [LIFT 1][LIFT 2] ┌───┐ [LIFT 3][LIFT 4]  ◄── Forward Downward Thrusters
    Z:  0         │         │MED│         │          ◄── Medium Cargo Hold (3×3×3 sb)
    Z: −3  [LIFT 5][LIFT 6] └───┘ [LIFT 7][LIFT 8]  ◄── Aft Downward Thrusters
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.2 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Capacity & Performance            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Medium Cargo (sb)   │ 1   │ 0.45 t / 30   │ `ALB Hold Components` (3,375 L volume)   │
│ Small Atmo Thruster │ 8   │ 0.80 t / 120  │ `ALB Thruster Lift 1..8` (Face DOWN −Y)  │
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `ALB Cam Belly` (Downward docking camera)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 13.1A: DOCKING CLEARANCE & LIFT GAP**
> Before building Level L3 structure:
> 1. Ensure `ALB Dock Conn` face is flush with the skid baseline so it locks effortlessly when landing.
> 2. Verify large conveyor ports on Medium Cargo connect directly to the bottom connector and the forward tool port.
> 3. Verify all 8 downward lift thrusters have a clear 1 sb exhaust gap beneath the nozzles.

---

##### Visual Milestone 13-1 · Belly Skids & Cargo Hold Complete

```
Visual Checkpoint Silhouette (Looking Starboard at Cargo Core):
               [MEDIUM CARGO HOLD L2-L4]
                          │
     ═══════[LIFT 1..2]──[SKID RUNNER]──[LIFT 5..6]═══════ (Level L1)
                          │
                   [ALB DOCK CONN ↓]
```

* [ ] Skids support the craft evenly with 0.5 m belly ground clearance.
* [ ] Medium cargo container accessible from terminal; inventory reads 3,375 L capacity.
* [ ] All 8 lift thrusters provide balanced upward thrust centered through the loaded CoM.

---

### Phase 2 · Bow Welder Head & Propulsion Ring (Levels L2-L4)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 9 W × 11 L × 3 H sb (4.5 × 5.5 × 1.5 m)
       Coordinate Range: X −4 to +4, Z −5 to +5, Y = L2 to L4
       Key Datums:
       · Ship Welder Tool Face: [X0, Z +5, Y = L2-L3] (Faces Bow +Z)
       · Work Lights: X = ±2, Z +5, Y = L4
       · Main Forward Thrusters (4×): X = ±2, Z −5, Y = L2-L3 (Face Aft)
       · Reverse Thrusters (4×): X = ±2, Z +4, Y = L2-L3 (Face Bow)
```

#### Step 13.3 · Industrial Welder Tool Head & Precision Recon Pod (L2-L3)

Mount the small-grid Ship Welder on the bow centerline (X0, Z +4 to +5) mating directly to the forward large conveyor port of the Medium Cargo Container. Flank the tool tip with twin high-intensity searchlights and a precision alignment reticle camera.

```
Front elevation, looking Stern at Welder Tool Head (Z = +5):
       L4       [SPOTLIGHT P]  [ALB CAM RETICLE]  [SPOTLIGHT S]
       L3     ┌─────────────────────────────────────────────────┐
       L2     │          [ALB WELDER NOSE (1×2×2 sb)]           │
       L1     ■═════════════════════════════════════════════════■
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.3 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Placement                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Ship Welder (sb)    │ 1   │ 0.40 t / 150  │ `ALB Welder Nose` (Centered X0, Z +5)    │
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `ALB Cam Welder` (Precision aim reticle) │
│ Spotlight (Small)   │ 2   │ 0.06 t / 20   │ `ALB Light Work Port / Stbd` (Floodlight)│
│ Sensor (Small)      │ 1   │ 0.03 t / 15   │ `ALB Sensor WeldZone` (2 m trigger field)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 13.4 · 14-Thruster 6-DOF Maneuvering Ring (L2-L4)

Install the 14 horizontal and vertical translation thrusters across the fuselage outriggers:
* 4× Forward Thrusters (facing Aft −Z at Z −5): 320 kN main cruise drive.
* 4× Reverse Thrusters (facing Bow +Z at Z +4): 320 kN high-speed precision braking.
* 2× Port Thrusters (facing Port −X at X −4): 160 kN lateral strafe.
* 2× Starboard Thrusters (facing Stbd +X at X +4): 160 kN lateral strafe.
* 2× Zenith Thrusters (facing Up +Y at Z 0): 160 kN downward push for inverted terrain docking.

```
Plan view, Y = L3 (Horizontal Thruster Layout)
        X: −4   −3   −2     0    +2   +3   +4
    Z: +4  [REV 1]   [REV 2] [WELDER] [REV 3]   [REV 4]  ◄── Facing Bow +Z (Braking)
    Z: +1  [PORT 1]                           [STBD 1]   ◄── Facing Outward (Lateral)
    Z: −1  [PORT 2]                           [STBD 2]
    Z: −5  [FWD 1]   [FWD 2]         [FWD 3]   [FWD 4]  ◄── Facing Aft −Z (Main Drive)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.4 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Vector Orientation & Operating Specs     │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Atmo Thruster │ 14  │ 1.40 t / 210  │ `ALB Thruster Fwd/Rev/Port/Stbd/Zenith`  │
│ Small Armor Slopes  │ 20  │ 0.40 t / 20   │ Palette Slot 4 (Cobalt); nacelle fairing │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 13.2A: WELDER PULL & PRECISION HOVER CHECK**
> 1. Deposit 500 Steel Plates into `ALB Hold Components`; verify `ALB Welder Nose` draws components automatically when welding test blocks.
> 2. Test reverse thruster brake distance: confirm craft decelerates from 50 km/h to dead stop in $< 12\text{ m}$.
> 3. Verify `ALB Sensor WeldZone` triggers welder activation only when damaged blocks are within 2.0 m.

---

### Phase 3 · Power Core, Avionics & Armor Hull (Levels L3-L5)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 9 W × 11 L × 3 H sb (4.5 × 5.5 × 1.5 m)
       Coordinate Range: X −4 to +4, Z −5 to +5, Y = L3 to L5
       Avionics & Power Datums:
       · 4× Small Batteries: X = ±1, Z −2 to −3, Y = L3
       · Small Reactor: X0, Z −4, Y = L3
       · Remote Control: [X0, Z +2, Y = L4] (Faces Bow +Z)
       · Micro Gyroscope: [X0, Z +1, Y = L4]
       · PB Utility: [X0, Z −1, Y = L4]
       · Comms Antenna: [X0, Z −4, Y = L5]
```

#### Step 13.5 · Power Vault & Autonomous Avionics Rack (L3-L4)

Install 4 Small Grid Batteries and 1 Small Nuclear Reactor directly aft of the Medium Cargo Hold. Mount the flight avionics, gyroscope, and RIG//OS Programmable Block along the upper center spine.

```
Plan view, Y = L4 (Power Vault & Avionics Core)
        X: −2   −1    0   +1   +2
    Z: +2  ┌─────┬───────────┬─────┐
           │     │ [REMOTE]  │     │  ◄── Autopilot Brain (`ALB Remote Control`)
    Z:  0  │     │  [GYRO]   │     │  ◄── Precision Stabilization Gyro
    Z: −1  │     │ [PB UTIL] │     │  ◄── Zero-GC Maintenance Script
    Z: −3  │[BAT 1][BAT 2][BAT 3][BAT 4]  ◄── 4× Small Batteries (Palette Slot 6: Red)
    Z: −4  └─────┴─[REACTOR]─┴─────┘  ◄── Small Nuclear Reactor (0.5 MW backup)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.5 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Operating Settings & Performance         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Grid Battery  │ 4   │ 1.00 t / 80   │ `ALB Battery 1..4` (2.0 MWh total cap)   │
│ Small Reactor (sb)  │ 1   │ 0.15 t / 25   │ `ALB Reactor` (0.5 MW continuous backup) │
│ Remote Control (sb) │ 1   │ 0.05 t / 25   │ `ALB Remote Control` (Autopilot helm)    │
│ Gyroscope (sb)      │ 1   │ 0.08 t / 15   │ `ALB Gyro` (Precision station keeping)   │
│ Programmable Block  │ 1   │ 0.08 t / 25   │ `ALB PB Utility` (Repair & logistics)    │
│ Event Controller    │ 1   │ 0.03 t / 20   │ `ALB EC CargoWatch` (Stock alert monitor)│
│ Radio Antenna (sb)  │ 1   │ 0.04 t / 25   │ `ALB Antenna` (Range: 15,000 m broadcast)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 13.6 · Heavy Protective Hull & Service Fairings (L4-L5)

Encase the craft in reinforced sloped armor fairings (Palette Slot 4: Manufacturing Cobalt) with high-visibility hazard yellow rub rails along the flanks. Add top armor louvers to shield the avionics pod from falling debris during vertical shaft rescue operations.

```
Plan view, Y = L5 (Utility Roof Canopy & Rub Rails)
        X: −4   −2    0   +2   +4
    Z: +5       ┌────[WELDER]────┐       ◄── Heavy Nose Bumper
    Z: +3  [RAIL]┌──────────────┐[RAIL]  ◄── High-Viz Yellow Side Rub Rails (Palette 9)
    Z:  0  │    │  ROOF CANOPY  │    │
    Z: −3  │    │ (COBALT BLUE) │    │
    Z: −5  [RAIL]└──[ANTENNA]───┘[RAIL]  ◄── Long-Range Comms Mast
```

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 13.3A: AVIONICS RIGGING & POWER FLOAT CHECK**
> 1. Verify `ALB Remote Control` horizon matches level landing baseline (pitch 0°, roll 0°).
> 2. Ensure `ALB PB Utility` compiles clean under MDK2 with zero runtime memory allocations.
> 3. Verify total electrical surge output delivers 4.5 MW peak with zero brownout under simultaneous all-thruster burst and welder operation.

---

##### Visual Milestone 13-2 · Complete ALBATROSS-01 Utility Craft Commissioned

```
Visual Checkpoint Silhouette (Looking Port at Complete Craft):
                         [ROOF CANOPY L5]
                                │
        [ANTENNA]               │                 [WELDER HEAD & CAM]
            \                   │                         /
     ════════╧══════════════════╧═════════════════════════╧═════════
          9 W × 11 L × 5 H sb UTILITY & REPAIR CRAFT (ALBATROSS-01)
                                │
                         [ALB DOCK CONN ↓]
```

* [ ] Craft empty mass confirmed: `≈ 8.5 t` via info tab.
* [ ] 22 atmospheric thrusters deliver instantaneous, vibration-free 6-DOF station keeping.
* [ ] Welder tool head operates with 100% component feed from the Medium Cargo hold.

---

### Phase 4 · Master Control Logic & Hardware Scripting Contract (Claude Code)

#### Step 13.7 · Terminal Namespace & Group Taxonomy

##### 1. Group Taxonomy
* `ALB Thrusters All` : All 22 atmospheric thrusters.
* `ALB Thrusters Lift` : 8× Downward lift thrusters (`ALB Thruster Lift 1..8`).
* `ALB Batteries` : 4× Small Grid Batteries (`ALB Battery 1..4`).
* `ALB Lights` : 2× Work Spotlights + 2× Wingtip Beacons.
* `ALB Tools` : `ALB Welder Nose` (main repair head).
* `ALB Cameras` : `ALB Cam Welder` (reticle target) + `ALB Cam Belly` (docking align).

##### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `ALB EC Battery` | `ALB Batteries` Stored Power | Stored Charge $< 25\%$ | Trigger RTH (Return to Base) | Normal Operation |
| `ALB EC Cargo` | `ALB Hold Components` Mass | Volume Filled $> 90\%$ | Alert C2 Bridge: Hold Full | Normal Capacity |
| `ALB EC Dock` | `ALB Dock Conn` State | Connector: Connected | Batteries $\rightarrow$ Recharge · Thrusters OFF | Batteries $\rightarrow$ Auto · Thrusters ON |
| `ALB EC Welder` | `ALB Sensor WeldZone` | Damaged Block $\le 2\text{ m}$ | `ALB Welder Nose` Turn ON | Welder Turn OFF |

##### 3. Inter-Grid Communication (IGC) Protocol (Fleet Maintenance Bus)

| IGC Tag / Channel | Transceiver | Direction | Message Payload Format | Functional Purpose |
|---|---|---|---|---|
| `TMS.UTIL.STATUS` | Radio Antenna | Outbound | `craft_id;status;gps_coords;hold_pct;bat_pct`| Telemetry feed to C2 Bridge Video Wall |
| `TMS.REPAIR.CALL` | Radio Antenna | Inbound | `caller_id;target_gps;damage_type;urgency` | Dispatches Albatross to damaged drone |
| `TMS.SUPPLY.RESTOCK`| Radio Antenna | Outbound | `craft_id;req_plates;req_motors;req_comps` | Orders component restock from Orca Bay S2 |

---

### Phase 5 · Commissioning Checklist & Acceptance Trials (RIG Test)

#### A · Precision Hover & Station Keeping Trials
* [ ] **Point-Blank Hover Stability:** Hover 1.5 m from an Orca hull block; release flight sticks; verify craft drifts $< 0.05\text{ m}$ over 60 seconds in 1 g wind.
* [ ] **Laden Flight Dynamics:** Fill Medium Cargo with 10 t Steel Plates (total mass ≈ 18.5 t); confirm upward lift thrusters maintain hover at 28.4% throttle without thermal fade.

#### B · Field Repair & Salvage Operations
* [ ] **Automated Welding Trial:** Approach a damaged Remora drill head; confirm `ALB Sensor WeldZone` detects damage at 2.0 m and auto-ignites `ALB Welder Nose`; verify full repair completes in $< 10\text{ seconds}$.
* [ ] **Component Restock at Orca:** Dock to Orca Bay S2 connector; verify sorter pulls 500 Steel Plates, 100 Construction Components, and 50 Motors into `ALB Hold Components`.

#### C · Carrier Recovery & Pad Validation
* [ ] **Orca Bay S2 Ingress:** Approach Orca Bay S2 at hover; align with bay guides; verify clearance margin is $\ge 1.5\text{ B}$ (3 sb) on all sides; verify belly connector lock.
* [ ] **Ground Base Gantry Landing:** Land on Ground Base gantry pad; confirm `ALB Dock Conn` locks flush and initiates rapid battery recharge.


---

# Volume II · Surface Infrastructure & Industrial Core

## Chapter 4 · Ground Base, Refinery Complex & Fleet Command Center (C2)

Stationary terrestrial logistics terminal, high-throughput manufacturing foundry, and central Multi-Fleet Command & Control (C2) Operations Hub.

* **Grid:** Large Grid (1 B = 2.5 m).
* **Footprint:** 24 × 28 B foundation (60.0 × 70.0 m) · Height: 9 B (22.5 m to comms mast tip).
* **Role:** Primary planetary logistics nexus. Receives high-value rare ores from aerial shuttles (MULE-01) and ultra-high-volume bulk ores from land trains (CATERPILLAR-01); refines raw ores with maximum yield; fabricates ship components at 400% speed; feeds the Rig Linear Extruder Ring; and coordinates all terrestrial and aerial fleet operations via an elevated C2 bridge.
* **Operating Environment:** Earth 1 g baseline, surface level.

---

### 4.0 · Reference Grid, In-Game Paint Palette & Facility Zoning

#### 1. Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the foundation deck surface at Level L1.
* **X (Beam / Transverse):** West is negative (−), East is positive (+), Centerline is 0.
* **Y (Elevation):** Tarmac foundation deck is Level L1 (Y = 0), advancing upward (+).
* **Z (Length / Longitudinal):** South (toward Extruder Ring) is negative (−), North (toward Landing Pads & Rail Terminal) is positive (+).

```
Spatial Alignment Triad:
       [+Y: Zenith / Up]
              │
              │   [+Z: North / Approach / Pads]
              │   /
              │  /
              │ /
  [−X: West] ─┼───────── [+X: East]
             /│
            / │
           /  │
  [−Z: South / Extruder Ring]
              │
       [−Y: Bedrock Anchor]
```

#### 2. In-Game Paint Palette & Color Coding Scheme

To eliminate builder disorientation across a 60 × 70 m facility, all structural armor and functional blocks MUST be painted during assembly according to the standard Space Engineers 1 palette:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Heavy armor foundation tarmac, bedrock anchors, retaining walls |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | MULE-01 touchdown deck, gantry cradle arms, pad perimeter borders |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | CATERPILLAR-01 unload trench, stop bumpers, rail alignment curbs |
| **Slot 3** | Processing Teal | `#1ABC9C` | Bright Teal | 4× Large Refineries, 16× Yield Modules, raw ore intake manifold |
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | 4× Large Assemblers, 16× Speed Modules, refined ingot feed trunk |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Large Component Buffer container, Extruder feed conveyor trunk |
| **Slot 6** | High-Voltage Red | `#C0392B` | Alert Red | Power Vault, 6× Large Batteries, 2× Auxiliary Nuclear Reactors |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | C2 Operations Bridge exterior shell, server rack consoles |
| **Slot 8** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel catwalks, stairwells, maintenance access hatches |
| **Slot 9** | Clean Armor White | `#ECF0F1` | Bright White | C2 Bridge interior ceiling, panoramic window trim, comms spire |

#### 3. Master Facility Layout & Zoning Plan

```
Plan view, Y = L1 (Tarmac Deck Level, 24 × 28 B)
        X: −11  −9    −6    −3     0    +3    +6    +9   +11
    Z: +13 ┌─────┬─────┬─────┬═════════════┬─────┬─────┬─────┐
           │     │ CATERPILLAR│  MULE PAD   │     │     │     │
    Z: +11 │     │ UNLOAD BAY │ (7×7 B Deck)│     │     │     │
           │     │ (Trench)  │  [PAD CONN] │     │     │     │
    Z: +7  │     │ [DUMP CON]│  [CRADLES]  │     │     │     │
           │     ├─────┴─────┼═════╤═══════┴─────┼─────┼─────┤
    Z: +5  │ C2  │           │     │ Ore Trunk   │     │     │
           │ OPS │┌────────┐ │ ┌───┴────┐  ┌────┐│     │     │
    Z: +1  │ HUB ││ REF 1&2│ │ │ REF 3 │  │REF4││     │     │  (4× Large Refineries)
           │(L3) │└────────┘ │ └────────┘  └────┘│     │     │  (16× Yield Modules)
    Z: −3  ├─────┤           │                   │     │     │
           │COMMS│┌────────┐ │ ┌────────┐  ┌────┐│     │     │  (4× Large Assemblers)
    Z: −5  │MAST ││ ASM 1&2│ │ │ ASM 3  │  │ASM4││     │     │  (16× Speed Modules)
           ├─────┴┴────────┴═╪═╪════════┴──┴────┴┴─────┤     │
    Z: −9  │  [POWER VAULT]  │ │   [BUFFER CARGO]      │     │  (Batteries & Storage)
           │ (6× Bat + 2× Rct│ │ (Large Component Hold)│     │
    Z: −13 └─────────────────┴─┼───════════╤═══════════┴─────┘
                               │           │ Direct Feed Line to Extruder Ring
                               │           ▼
                       Bulk Ore Return   Pylon Feed (60 m South)
```

---

### 4.1 · Master Logistics & Process Flow Topology

The facility processes materials through two distinct speed-tiers before funneling fabricated components to the Rig Extruder Ring:

```
                           TERRESTRIAL LOGISTICS INTAKE
                                         │
         ┌───────────────────────────────┴───────────────────────────────┐
         ▼ (Tier 1: Air / Rare)                                          ▼ (Tier 2: Ground / Bulk)
  MULE-01 TOUCHDOWN (Z +9, X +3)                               CATERPILLAR-01 UNLOAD (Z +9, X −4)
  [GND PAD Conn] (Belly Lock)                                  [GND CAT Dump Conn] + 5× Collectors
         │                                                               │
         ▼ (Drain ON · High Priority)                                    ▼ (Drain ON · Bulk Sorter)
  [GND Sorter Rare] (U, Pt, Au, Mg)                            [GND Sorter Bulk] (Fe, Ni, Si, Stone)
         │                                                               │
         ▼                                                               ▼
  [GND REF 1 & 2] (High-Yield Smelting)                        [GND REF 3 & 4] (High-Yield Smelting)
  (8× Yield Modules · Rare Ingot Stream)                       (8× Yield Modules · Bulk Ingot Stream)
         │                                                               │
         └───────────────────────────────┬───────────────────────────────┘
                                         │ (Refined Ingots Trunk)
                                         ▼
         ┌───────────────────────────────┴───────────────────────────────┐
         ▼                                                               ▼
  [GND ASM 1 & 2] (Structural Hull Run)                        [GND ASM 3 & 4] (Machinery & Thrusters)
  (8× Speed Modules · Plates, Girders)                         (8× Speed Modules · Motors, Computers)
         │                                                               │
         └───────────────────────────────┬───────────────────────────────┘
                                         │ (Fabricated Components Trunk)
                                         ▼
                             [GND STR Components]
                             (Large Cargo Container · Z −10, X +3)
                                         │
                                         ▼ (Main High-Pressure Feed Line)
                       [EXT Welder Ring] (Chapter 5 Shipyard)
                       (22× Ship Welders · 6× Hydraulic Pistons)
```

---

### 4.2 · Master Electrical & Comms Bus Schematic

```
                        PRIMARY PLANETARY POWER GRID
           6× Large Grid Batteries (18 MWh Cap · 72 MW Peak Discharge)
           2× Auxiliary Nuclear Reactors (30 MW Emergency Standby)
                                      │
                                      ▼
                             MAIN DISTRIBUTION BUS
         ┌────────────────────────────┼────────────────────────────┐
         │                            │                            │
         ▼                            ▼                            ▼
  HEAVY INDUSTRIAL LOAD        SHIPYARD EXTRUSION LOAD      C2 COMMAND & CONTROL (UPS)
  · 4× Refineries (13.2 MW)    · 22× Welders (8.8 MW peak)  · 3× Dedicated Batteries (9 MWh)
  · 4× Assemblers (2.2 MW)     · 6× Pusher Pistons (20 kW)  · 3× Server Racks (PBs)
  · Mule Charger (12.0 MW)     · Projector Head (30 kW)     · Fleet Ops Video Wall (6× LCD)
  · Train Charger (6.0 MW)                                  · 50 km Antenna + Laser Relay
```

---

### 4.3 · Master Pre-Flight Bill of Materials (BOM)

| Subassembly Zone | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Foundation Tarmac** | Heavy armor full blocks, half-blocks, blast edges (Palette 0) | 540 | 460.0 t | 540 |
| **Mule Gantry Pad** | 7×7 B deck, 1× Connector, 2× Hinges, 2× Mag Plates, 1× Sensor | 55 | 22.5 t | 275 |
| **Caterpillar Unload Dock**| 1× Connector, 5× Collectors, 2× Shock Bumpers, Guide Curbs | 32 | 34.0 t | 320 |
| **Refining Core** | 4× Large Refineries, 16× Yield Modules, Sorter manifold | 23 | 188.0 t | 645 |
| **Manufacturing Core** | 4× Large Assemblers, 16× Speed Modules, Conveyors | 22 | 78.5 t | 590 |
| **Storage & Logistics**| 1× Large Cargo Container, 4× Sorters, Heavy Conveyor Trunk | 48 | 38.0 t | 260 |
| **Power Vault** | 6× Large Batteries, 2× Small Reactors, Substation Switchgear | 12 | 48.0 t | 340 |
| **C2 Fleet Ops Bridge**| 3× Control Seats, 6× LCDs, 3× PBs, Glass, Catwalks, Airtight Door | 64 | 42.0 t | 680 |
| **Comms & Relay Spire**| 1× Radio Antenna (50 km), 1× Laser Antenna, 2× Cameras | 6 | 11.0 t | 350 |
| **TOTAL GROUND FACILITY**| Complete Surface Logistics Terminal & C2 Hub | **802** | **≈ 922.0 t** | **4,000** |

---

### Phase 1 · Foundation Tarmac & Ground Logistics Interfaces (Level L1)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 24 W × 28 L × 3 H B (60.0 × 70.0 × 7.5 m)
       Coordinate Range: X −11 to +11, Z −13 to +13, Y = L1 to L3
       Primary Datums:
       · Tarmac Surface Deck: Level L1 (Y = 0)
       · Mule Touchdown Center: [X +3, Z +9]
       · Caterpillar Trench Center: [X −4, Z +9]
```

#### Step 4.1 · Heavy Foundation Deck & Sub-Surface Conveyor Trenches (L1)

Excavate a level bedrock plateau and lay the reinforced 24 × 28 B heavy armor blast floor. Recess two longitudinal conveyor trenches (3 B wide × 1 B deep) beneath the surface deck to house the main raw ore intake lines.

```
Plan view, Y = L1 (Foundation Sub-grade & Trenches)
        X: −9    −6    −3     0    +3    +6    +9
    Z: +13 ■═════╦═════╦═════╦═════╦═════╦═════╦═════■
           ║     ║ TRENCH A  ║     ║ TRENCH B  ║     ║
    Z: +9  ║     ║ (CAT ORE) ║     ║(MULE ORE) ║     ║  ◄── Recessed Pipe Raceways
           ║     ║           ║     ║           ║     ║
    Z: +5  ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣
           ║     ║     ║     ║     ║     ║     ║     ║
    Z:  0  ║     ║ REFINERY  ║ MAIN║ REFINERY  ║     ║  ◄── Industrial Sub-grade
           ║     ║ PORT MANI ║ SPINE STBD MANI ║     ║
    Z: −5  ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣
           ║     ║ ASM MANIFOLD    ║ COMPONENT ║     ║
    Z: −11 ║     ║                 ║ OUTFLOW   ║     ║
    Z: −13 ■═════╩═════╩═════╩═════╩═════╩═════╩═════■
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.1 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 420 │ 357.0 t / 420 │ Palette Slot 0 (Slate); level perimeter  │
│ Heavy Armor Slope   │ 80  │  40.0 t / 80  │ Ramp transitions around pad edges        │
│ Conveyor Junction   │ 24  │  19.2 t / 144 │ Recessed in sub-surface trenches (L0)    │
│ Conveyor Tube (Lrg) │ 16  │   9.6 t / 96  │ Large ports north-south alignment        │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.1A: SUB-SURFACE CONVEYOR INTEGRITY**
> Before welding the flush tarmac deck plates over Trenches A and B:
> 1. Verify 100% large-port conveyor continuity from the northern intake nodes to the southern industrial headers.
> 2. Ensure zero small-conveyor bottlenecks exist anywhere in the main line (ore transfer requires large-tube flow).
> 3. Perform a vacuum air-seal check on all junction cubes.

---

#### Step 4.2 · Mule Receiving Gantry Cradle Pad (Z +6 to +12, X 0 to +6, L1-L3)

Build the dedicated 7 × 7 B landing pad for the MULE-01 Heavy Freight Shuttle. The pad mounts a heavy-duty flush receiving connector and twin motorized cradle arms equipped with magnetic clamping plates per **Decision 45**.

```
Plan view, Y = L1 (Mule Gantry Deck)
        X:  0    +1    +2    +3    +4    +5    +6
    Z: +12 ■─────────────────────────────────────■
           │     ·     ·     ·     ·     ·     │
    Z: +10 │  [CRADLE P]     ·     [CRADLE S]  │  ◄── Motorized Clamp Hinges
           │  (Hinge L1)     ·     (Hinge L1)  │
    Z: +9  │     ·     · [GND PAD] ·     ·     │  ◄── Flush Receiving Connector
           │     ·     ·  (Conn ↑) ·     ·     │
    Z: +8  │  [MAG P]        ·        [MAG S]  │  ◄── Magnetic Clamping Plates
           │  (Swing 30°)    ·     (Swing 30°) │
    Z: +6  ■─────────────────────────────────────■
```

```
2.5D Isometric Callout · Mule Gantry Cradle Clamps (Z +9, X +3)

                        [MULE-01 HULL 7×7 B]
                                 │
     Port Flank                  ▼               Starboard Flank
   [CLAMP RUB RAIL]                        [CLAMP RUB RAIL]
          ▲                                       ▲
          │                                       │
     [MAG PLATE]  ◄── 30° Swing Inward ──►  [MAG PLATE]
          │                                       │
     [MOTOR HINGE]                           [MOTOR HINGE]
     (Base: X +1, Z +10)                     (Base: X +5, Z +10)
   ═══════╧═══════════════════════════════════════╧═══════
                      FLUSH HEAVY BLAST DECK (L1)
                             [GND PAD Conn]
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.2 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 49  │  41.6 t / 49  │ Palette Slot 1 (Amber); 7×7 B flush deck │
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `GND PAD Conn` (Facing UP at X +3, Z +9) │
│ Motor Hinge (Large) │ 2   │   8.0 t / 200 │ Share Inertia Tensor = ON · Limit: 30°   │
│ Magnetic Plate (Lrg)│ 2   │   4.8 t / 50  │ Auto-lock enabled; strength 100%         │
│ Sensor              │ 1   │   0.3 t / 40  │ `GND PAD Sensor` (15 m aerial detection) │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `GND PAD Sorter Rare` (Drain All = ON)   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 4.2A: CRADLE CLAMP CALIBRATION**
> 1. Set `Share Inertia Tensor = ON` on both `GND PAD Cradle Hinge Port` and `Stbd`.
> 2. Set Hinge Lower Limit to `0.0°` (fully retracted flush with deck) and Upper Limit to `30.0°` (angled inward to match Mule flank slope).
> 3. Verify `GND PAD Sorter Rare` whitelist contains: `Uranium`, `Platinum`, `Gold`, `Magnesium`, and `Scrap`.

---

#### Step 4.3 · Caterpillar Bulk Ore Receiving Trench Dock (Z +6 to +12, X −6 to −1, L1)

Construct the high-volume receiving dock for the CATERPILLAR-01 Land Train. It features a heavy guide curb, reinforced stop bumpers, a stern-docking receiving connector, and a 5-block flush collector trench for rapid hopper gravity discharge.

```
Plan view, Y = L1 (Caterpillar Unload Dock)
        X: −6    −5    −4    −3    −2    −1
    Z: +12 ║═════╧═════╧═════╧═════╧═════╧═════║  ◄── Train Entry Approach
           ║ [CURB P]               [CURB S]   ║      (Heavy armor wheel guides)
    Z: +10 ║   │    [COLL 1] [COLL 2]   │      ║
           ║   │    [COLL 3] [COLL 4]   │      ║  ◄── 5× In-Deck Collectors
    Z: +8  ║   │        [COLL 5]        │      ║      (Flush with deck, facing UP)
           ║   │                        │      ║
    Z: +6  ╠═══╪════════════════════════╪══════╣
           ║[BUMP P]   [GND CAT Conn] [BUMP S] ║  ◄── Stern Lock Connector &
    Z: +5  ╚═══════════════╤═══════════════════╝      Shock Absorber Bumpers
                           ▼
                 Main Bulk Ore Sorter (Trench A)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.3 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Settings & Orientation          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `GND CAT Dump Conn` (Facing North +Z)    │
│ Collector (Large)   │ 5   │  18.5 t / 125 │ `GND CAT Hopper Coll 1..5` (Facing UP)   │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `GND CAT Sorter Bulk` (Drain All = ON)   │
│ Heavy Armor Slopes  │ 18  │  15.3 t / 18  │ Palette Slot 2 (Crimson); wheel guide curb│
│ Half Armor Blocks   │ 8   │   3.4 t / 8   │ Shock bumper backstops at Z +5           │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 4.3A: TRAIN DOCK DISCHARGE CLEARANCE**
> 1. Ensure `GND CAT Dump Conn` is mounted at Level L2 centerline (matching `CAT Train Unload Conn` height).
> 2. Verify all 5 collectors feed directly into Trench A with large conveyor ports.
> 3. Verify `GND CAT Sorter Bulk` whitelist is set to: `Iron`, `Nickel`, `Silicon`, `Cobalt`, and `Stone`.

---

#### Visual Milestone 4-1 · Tarmac Foundation & Dual Logistics Terminals Complete

```
Visual Checkpoint Silhouette (Looking South from Approach Vector):
       X: −6 (Caterpillar Dock)       X: +3 (Mule Gantry Pad)
          ┌────────────────┐             ┌────────────────┐
          │  CRIMSON DOCK  │             │   AMBER PAD    │
          │ [COLL TRENCH]  │             │ [GANTRY CLAMPS]│
          │ [CAT DUMP CON] │             │ [GND PAD CONN] │
     ═════╧════════════════╧═════════════╧════════════════╧═════
               24 × 28 B REINFORCED SLATE TARMAC DECK (L1)
```

* [ ] Foundation blast floor is completely sealed with zero exposed bedrock gaps.
* [ ] Mule gantry cradle hinges operate smoothly between 0° and 30° without collision.
* [ ] Caterpillar dock guide curbs provide exactly 5.2 m clear width for 5×5 wheel entry.

---

### Phase 2 · Industrial Refining & Manufacturing Core (Levels L1-L2)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 20 W × 14 L × 3 H B (50.0 × 35.0 × 7.5 m)
       Coordinate Range: X −8 to +8, Z −11 to +5, Y = L1 to L2
       Key Centers:
       · Smelting Core (4 Refineries): Z +1 to +4, X ±3 to ±5
       · Fabrication Core (4 Assemblers): Z −5 to −1, X ±3 to ±5
       · Component Storage & Power Vault: Z −11 to −7, X −6 to +6
```

#### Step 4.4 · 4-Refinery Heavy Smelting Array (Z +1 to +4, X ±3 to ±5, L1-L2)

Install 4 Large Refineries paired with 16 Yield Modules (4 per refinery) to achieve 100% ore extraction yield bonus. The array is split into two banks: West Bank (Refineries 1 & 2) handles rare ores from Trench B; East Bank (Refineries 3 & 4) handles bulk ores from Trench A.

```
Plan view, Y = L1-L2 (Refinery Core Layout)
        X: −6   −5   −4   −3        +3   +4   +5   +6
    Z: +4  ┌───────────────┐        ┌───────────────┐
           │ [YLD] REF 1   │        │ REF 3   [YLD] │
    Z: +3  │ [YLD] [2×4 B] │        │ [2×4 B] [YLD] │  ◄── 4× Large Refineries
    Z: +2  │ [YLD] REF 2   │        │ REF 4   [YLD] │      (Painted Processing Teal)
    Z: +1  │ [YLD] [2×4 B] │        │ [2×4 B] [YLD] │
           └───────────────┘        └───────────────┘
                   │                        │
                   ▼                        ▼
           Refined Ingot Trunk      Refined Ingot Trunk
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.4 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Mounting Specifications         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Refinery      │ 4   │ 100.8 t / 360 │ Palette Slot 3 (Teal); ports facing N/S  │
│ Yield Module        │ 16  │  83.2 t / 240 │ 4 per refinery; mounted flush to sides   │
│ Conveyor Tube (Lrg) │ 12  │   7.2 t / 72  │ Dual intake and dual output manifolds    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!TIP]
> **PRE-ENCLOSURE INSPECTION GATE 4.4A: REFINERY YIELD VERIFICATION**
> 1. Check the terminal interface of all 4 refineries (`GND REF 1` to `4`).
> 2. Verify `Effectiveness` reads exactly `200%` (or baseline 1.0 + 8 upgrade ports connected).
> 3. Verify automated ingot pull into the central distribution trunk is clear.

---

#### Step 4.5 · 4-Assembler Manufacturing Core (Z −5 to −1, X ±3 to ±5, L1)

Install 4 Large Assemblers flanked by 16 Speed Modules (4 per assembler) to supply the massive component demand of the Rig Extruder Ring. Assemblers 1 & 2 are dedicated to structural components (Steel Plates, Interior Plates, Construction Components); Assemblers 3 & 4 handle machinery and propulsion hardware (Large Steel Tubes, Motors, Computers, Thruster Components).

```
Plan view, Y = L1 (Assembler Array Layout)
        X: −5   −4   −3   −2        +2   +3   +4   +5
    Z: −1  ┌────────────┐              ┌────────────┐
           │ [SPD] ASM 1│              │ ASM 3 [SPD]│
    Z: −2  │ [SPD]      │              │       [SPD]│  ◄── 4× Large Assemblers
    Z: −3  │ [SPD] ASM 2│              │ ASM 4 [SPD]│      (Painted Cobalt Blue)
    Z: −4  │ [SPD]      │              │       [SPD]│
           └────────────┘              └────────────┘
                 │                           │
                 └─────────────┬─────────────┘
                               │ Fabricated Components Bus
                               ▼
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.5 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Mounting Specifications         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Assembler     │ 4   │  38.8 t / 160 │ Palette Slot 4 (Cobalt); ports N/S/U/D   │
│ Speed Module        │ 16  │  38.4 t / 240 │ 4 per assembler; mounted on top/sides    │
│ Conveyor Junction   │ 6   │   4.8 t / 36  │ Central manufacturing aggregation bus    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 4.5A: COOPERATIVE FABRICATION LINK**
> 1. Enable `Master / Slave` (Cooperative Mode) on Assemblers 2, 3, and 4.
> 2. Set Assembler 1 as the Master Production Queue controller.
> 3. Verify all 16 speed modules register `Productivity: 400%`.

---

#### Step 4.6 · Component Buffer Vault & Main Power Plant (Z −11 to −7, X −6 to +6, L1-L2)

Construct the central component buffer warehouse and the high-output planetary power station. The storage container acts as the master supply reservoir for the Extruder Ring, while the battery bank guarantees blackout immunity.

```
Plan view, Y = L1 (Storage & Power Vault)
        X: −6   −4   −2     0    +2   +4   +6
    Z: −7  ┌──────────────┐     ┌──────────────┐
           │ [BAT 1][BAT 2│     │ [STR BUFFER] │  ◄── Large Cargo Container (3×3×3 B)
    Z: −9  │ [BAT 3][BAT 4│     │ (3×3×3 B)    │      (Palette Slot 5: Emerald)
           │ [BAT 5][BAT 6│     │ [GND STR]    │
    Z: −11 │ [RCT 1][RCT 2│     └──────╤───────┘
           └──────────────┘            │ Main Extruder Feed Line (South)
                                       ▼
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.6 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Capacity                   │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo (3×3×3) │ 1   │  28.5 t / 250 │ `GND STR Components` (421,875 L capacity)│
│ Large Battery       │ 6   │  30.0 t / 120 │ `GND BAT 1..6` (18 MWh total capacity)†  │
│ Small Reactor       │ 2   │   3.2 t / 50  │ `GND RCT Aux 1..2` (30 MW total backup)  │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `GND Sorter Extruder Feed` (Push to Ring)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.6A: POWER BUS REDUNDANCY**
> 1. Verify `GND BAT 1..6` are set to `Auto` with Recharge/Discharge limits unconstrained.
> 2. Ensure `GND RCT Aux 1..2` are loaded with 50 kg Uranium each and assigned to group `GND Power Emergency`.
> 3. Verify the southern conveyor outlet terminates at X0, Z−13, facing directly toward the Chapter 5 Extruder Ring intake.

---

#### Visual Milestone 4-2 · Industrial Core Complete

```
Visual Checkpoint Silhouette (Looking West across Industrial Core):
        L2  ┌────────┐                     ┌────────┐
            │ REF 3  │   [INGOT TRUNK]     │ REF 4  │
        L1  ├────────┴──────────┬──────────┴────────┤
            │ 4× ASSEMBLERS     │ COMPONENT BUFFER  │
     ═══════╧═══════════════════╧═══════════════════╧═══════
                 LEVEL L1 REINFORCED SERVICE FOUNDATION
```

* [ ] All 4 refineries and 4 assemblers report green power indicators and zero blocked ports.
* [ ] Component buffer accepts continuous input from assemblers without back-pressure.
* [ ] Battery bank shows full charge availability of 72 MW peak.

---

### Phase 3 · Fleet Command & Control (C2) Operations Center (Levels L1-L5)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 5 W × 5 L × 9 H B (12.5 × 12.5 × 22.5 m)
       Coordinate Range: X −11 to −7, Z +1 to +5, Y = L1 to L9
       Decks:
       · L1: Heavy Structural Pylon Anchor
       · L2: C2 Server Room & Programmable Block Rack
       · L3-L4: Panoramic Observation Deck & Operator Stations
       · L5-L9: Long-Range Comms Mast & Station Laser Link
```

#### Step 4.7 · C2 Bridge Structure & Server Deck (Z +1 to +5, X −11 to −7, L1-L2)

Erect the reinforced 5 × 5 B support pillar on the western flank. Level L1 forms the blast-proof structural foundation; Level L2 houses the isolated Fleet Operations Server Room containing the RIG//OS Programmable Blocks and communication bus hardware.

```
Plan view, Y = L2 (Server Room Deck)
        X: −11  −10   −9   −8   −7
    Z: +5  ■─────■─────■────■────■
           │ [PB FLEET] [PB INV] │  ◄── Server Rack: 3× Programmable Blocks
    Z: +3  │ [PB DISP]  [EC C2]  │
           │      [UPS BAT 1]    │  ◄── Dedicated C2 Uninterruptible Power Supply
    Z: +1  ■─────■─────[DOOR]────■      (Guarantees comms during base blackouts)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.7 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Script Role / Hardware Function          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Programmable Block  │ 3   │   3.6 t / 75  │ `GND PB FleetOps`, `Inventory`, `Displays│
│ Event Controller    │ 2   │   0.6 t / 50  │ `GND EC PadControl`, `GND EC PowerWatch` │
│ Timer Block         │ 2   │   0.6 t / 50  │ `GND Timer Watchdog`, `GND Timer 100ms`  │
│ Large Battery       │ 1   │   5.0 t / 20  │ `GND BAT C2 UPS` (Dedicated bridge power)│
│ Sliding Airtight Dr │ 1   │   0.8 t / 25  │ Pressurized maintenance access hatch     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 4.7A: SERVER RACK INITIALIZATION**
> 1. Verify `GND PB FleetOps` compiles clean with zero warnings under MDK2.
> 2. Ensure `GND BAT C2 UPS` is completely isolated from the industrial grid via an automated switch event.
> 3. Verify server room is fully airtight prior to sealing the ceiling deck.

---

#### Step 4.8 · Elevated Panoramic Observation Deck & Multi-Fleet Consoles (L3-L4)

Build the elevated command deck (Y = L3-L4) cantilevering inward with panoramic reinforced glass. It provides unobstructed visual line-of-sight over the Mule Pad and Caterpillar Dock to the North, and the Rig Extruder Yard to the South.

```
Elevation, looking West into C2 Bridge (X = −7, Y = L3-L5):
       L5  ╔═══════════════════════════════════════════╗  Bridge Roof (Armor White)
           ║ [COMMS MAST]                              ║
       L4  ║ ┌───────────────────────────────────────┐ ║  Panoramic Sloped Glass
           ║ │         FLEET OPS VIDEO WALL          │ ║  (6× Large LCD Dashboards)
           ║ │  [ORCA 1..n]  [MULE TRANSIT]  [STOCKS]│ ║
       L3  ║ └───────────────────────────────────────┘ ║
           ║   [DESK 1: ORCA]   [DESK 2: MULE]   [DESK 3: YARD] ║  ◄── 3 Operator Desks
       L2  ╠═══════════════════════════════════════════╣  Server Room Deck
           ║   [GND PB FleetOps]   [GND PB Inv]   [GND PB Disp] ║
       L1  ■═══════════════════════════════════════════■  Tarmac Foundation
```

```
2.5D Isometric Callout · Multi-Fleet Video Wall & Operator Desks

      ┌─────────────────────────────────────────────────────────────┐
      │               FLEET OPERATIONS LCD VIDEO WALL               │
      ├──────────────────────┬──────────────────────┬───────────────┤
      │ LCD 1: AERIAL FLEET  │ LCD 2: GROUND FLEET  │ LCD 3: YARD   │
      │ Orca-01: 82% Hold    │ Mastodon: Digging    │ Extruder Ring:│
      │ Mule-01: Inbound Pad │ Cat-01: In Transit   │ 64% Complete  │
      │ Mule-02: At Carrier  │ Consist: 2,500 t Ore │ Piston Feed:  │
      │ 8 Active Mine Leases │ Speed: 52 km/h       │ 0.05 m/s      │
      ├──────────────────────┼──────────────────────┼───────────────┤
      │ LCD 4: REFINERY FLOW │ LCD 5: STOCKPILES    │ LCD 6: ALERTS │
      │ Rare Smelt: 100% Opt │ Fe Ingot: 142,000 kg │ Base Grid: NRM│
      │ Bulk Smelt: 100% Opt │ U Ingot:   1,850 kg  │ Defense: ARMED│
      │ Total Yield: 200%    │ Steel Plates: 85,000 │ Traffic: CLR  │
      └──────────────────────┴──────────────────────┴───────────────┘
                                     │
           ┌─────────────────────────┼─────────────────────────┐
           ▼                         ▼                         ▼
   [DESK 1: AIR OPS]         [DESK 2: GROUND OPS]      [DESK 3: YARD OPS]
   · Orca carrier leases     · Mastodon tusk CTC       · Extruder pusher sync
   · Remora shaft status     · Caterpillar routing     · Welder wall status
   · Mule gantry traffic     · Terminal dump cycling   · Component inventory
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.8 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Terminal Name & Configuration            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Control Station     │ 3   │   1.8 t / 45  │ `GND Desk AirOps`, `GroundOps`, `YardOps`│
│ Large LCD Panel     │ 6   │   2.4 t / 150 │ `GND LCD Video Wall 1..6` (Group Displays│
│ Sloped Armor Glass  │ 24  │   4.8 t / 24  │ 2-layer panoramic forward & rear windows │
│ Interior Light      │ 4   │   0.1 t / 20  │ Low-glare tactical amber interior lights │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 4.9 · Long-Range Comms Mast & Station Laser Link (Z +1, X −9, L5-L9)

Crown the C2 Bridge roof with a 10 m communications mast. It mounts a high-power omni-directional radio antenna for regional fleet command and a dedicated laser antenna hard-locked to the stationary Floating Rig hovering in the upper atmosphere.

```
Elevation, looking East at Comms Mast (X = −9, Y = L5-L9):
       L9             [LASER RELAY]  ◄── `GND Laser Rig Relay` (Locked to Rig)
                           │
       L8                  │
                           ├── [PERIMETER CAMERA]
       L7                  │
                           ├── [OMNI ANTENNA]  ◄── `GND Antenna Fleet` (50 km)
       L6                  │
       L5  ════════════════╧════════════════  ◄── Bridge Roof Deck
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.9 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Operating Settings & Parameters          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Radio Antenna (Lrg) │ 1   │   8.0 t / 100 │ `GND Antenna Fleet` (Range: 50,000 m)    │
│ Laser Antenna (Lrg) │ 1   │   2.5 t / 200 │ `GND Laser Rig Relay` (Rig GPS lock)     │
│ Camera              │ 2   │   0.2 t / 40  │ `GND Cam North Pads`, `GND Cam South Yard│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Visual Milestone 4-3 · Complete Facility & C2 Bridge Commissioned

```
Visual Checkpoint Silhouette (Looking West at Complete Base Complex):
                        [COMMS MAST (L9)]
                               │
                       [C2 BRIDGE (L3-L5)]
                               │
           [MULE PAD]          │          [POWER & INDUSTRIAL]
         (Z +9, X +3)          │              (Z −5, X 0)
     ══════════╤═══════════════╧═══════════════════╤══════════
               24 × 28 B REINFORCED SURFACE COMPLEX (L1)
```

* [ ] C2 bridge video wall displays live telemetry streams from all 6 LCD panels.
* [ ] Laser antenna maintains unbroken connection with the high-altitude Floating Rig.
* [ ] Control stations allow instantaneous remote access to Orca, Mule, Mastodon, and Caterpillar systems.

---

### Phase 4 · Master AI Scripting Contract & Terminal Namespace (Claude Code)

#### 1. Group Taxonomy
* `GND Refineries` : All 4 Large Refineries (`GND REF 1` to `4`).
* `GND Assemblers` : All 4 Large Assemblers (`GND ASM 1` to `4`).
* `GND Yield Modules` : All 16 Yield upgrade modules.
* `GND Speed Modules` : All 16 Speed upgrade modules.
* `GND Fleet Displays` : All 6 Video Wall LCD panels (`GND LCD Video Wall 1..6`).
* `GND Power Batteries` : All 6 Large Storage Batteries (`GND BAT 1..6`).
* `GND PAD Cradle` : `GND PAD Cradle Hinge Port`, `Stbd`, `GND PAD Cradle Clamp Port`, `Stbd`.
* `GND CAT Hopper` : `GND CAT Hopper Coll 1..5`, `GND CAT Dump Conn`.

#### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `GND EC PadClamp` | `GND PAD Sensor` | Detects MULE $\le 5\text{ m}$ | `GND PAD Cradle` Swing In 30° | `GND PAD Cradle` Retract Flush 0° |
| `GND EC PadLock` | `GND PAD Conn` | Connector Status: Connected | `GND PAD Sorter Rare` Drain ON | `GND PAD Sorter Rare` Drain OFF |
| `GND EC CatDump` | `GND CAT Dump Conn`| Connector Status: Connected | `GND CAT Sorter Bulk` Drain ON | `GND CAT Sorter Bulk` Drain OFF |
| `GND EC PowerGuard`| `GND Power Batteries`| Grid Stored Power $< 15\%$ | `GND RCT Aux 1..2` Turn ON | `GND RCT Aux 1..2` Turn OFF |

#### 3. Inter-Grid Communication (IGC) Network Protocol

| IGC Tag / Channel | Transceiver | Direction | Message Payload Format | Functional Purpose |
|---|---|---|---|---|
| `TMS.AIR.PAD` | Radio Antenna | In/Out | `mule_id;status;req_land;dock_id` | Pad allocation, clamp automation, traffic sequencing |
| `TMS.GROUND.DOCK` | Radio Antenna | In/Out | `train_id;status;req_dump;tonnage` | Caterpillar receiving trench clearance & sorter arming |
| `TMS.FLEET.STATUS`| Radio Antenna | Inbound | `craft_id;type;gps;hold_pct;bat_pct`| Central telemetry feed for C2 6-screen video wall |
| `TMS.RIG.LASER` | Laser Antenna | Bidirectional | `req_ingots;stock_json;rig_power;alert`| High-security base-to-Rig uplink for production quotas |

---

### Phase 5 · Commissioning Checklist & Acceptance Trials

#### A · Dual Logistics Interface Verification
* [ ] **Mule Touchdown Test:** Land MULE-01 on pad; verify sensor triggers gantry cradle arms to clamp rub rails at 30°; verify `GND PAD Conn` locks; verify rare ores drain at $> 80\text{ t/min}$.
* [ ] **Caterpillar Unload Test:** Reverse CATERPILLAR-01 into dump dock; verify wheel guides center the consist; verify `GND CAT Dump Conn` locks; verify 2,500 t bulk ore drains through collectors in $< 90\text{ seconds}$.

#### B · Refining & Manufacturing Throughput Trial
* [ ] **Smelting Stress Test:** Feed 1,000 t Iron Ore and 200 t Uranium Ore simultaneously; confirm dual-tier sorters route ores to respective refinery banks; verify 200% yield effectiveness.
* [ ] **Pylon Component Burst:** Queue 10,000 Steel Plates and 2,000 Construction Components; verify 4 assemblers operate cooperatively at 400% productivity without tripping the power grid.

#### C · C2 Fleet Command & Control Validation
* [ ] **Live Multi-Fleet Tracking:** Deploy 1 Orca carrier, 2 Remora drillers, 1 Mule shuttle, 1 Mastodon excavator, and 1 Caterpillar train concurrently. Verify C2 Video Wall displays live, non-overlapping status for all 6 units.
* [ ] **Laser Uplink Resilience:** Toggle planetary storm / weather effects; verify `GND Laser Rig Relay` maintains continuous, unjammed data transfer with the Floating Rig.

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

Source: `docs/rig/chapters/mastodon-instructions.md` · 11 × 42 × 6 B · 2-Segment Consist, Custom Walk-In Bridge, 35 m Boom, ULSS Multi-Planet Life Support, Failsafe Decoys & Scuttle

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
| **Drawbar Hitch & Severance** | 1× Adv Rotor, 1× Motor Hinge, 1× Merge Block (Hitch Sever Tongue), Heavy Armor Joint Blocks | 7 | 13.5 t | 235 |
| **Utility Carriage Hull** | Heavy armor frames, wheel wells, deck armor (Palette 0) | 360 | 306.0 t | 360 |
| **Utility Powertrain** | 4× 5×5 Heavy Suspension Wheels (Axles 4-5) | 4 | 32.0 t | 400 |
| **Utility Sub-Floor Battery Vault** | 3× Large Batteries (9.0 MWh, Palette 6) | 3 | 13.5 t | 120 |
| **Interior Walkways & Decoys** | Grated catwalks, sliding doors, ladder runs, interior lights, 3× Armored Decoys, 2× Keel Recovery Gyros (Palette 9) | 30 | 18.0 t | 240 |
| **Field Workshop & Universal Life Support** | 1× Basic Refinery, 1× Large Assembler, 1× Medical Room, 2× Cryo Pods, 1× Survival Kit, 2× Large O2 Tanks, 1× Quarantined Ice Hold, 2× Large H2 Tanks, 2× O2/H2 Gens, 6× Air Vents, 1× Master Projector, 1× Scuttle Warhead | 22 | 68.2 t | 815 |
| **Utility Cargo Hold** | 1× Large Cargo Container (Components), Conveyor network | 16 | 28.0 t | 160 |
| **Flight Decks, Welders & Solar** | 1× Mule Pad, 2× Kestrel Pads, 2× Drone Welders, Stern Dump, 4× Solar Panels, 4× Floodlights | 68 | 61.6 t | 980 |
| **Perimeter Defense Suite** | 2× Gatling Turrets, 2× Interior Turrets, Ammunition Conveyor Feeds (Palette 7) | 12 | 9.6 t | 308 |
| **External EVA, Lights & Failsafes** | 2× Ground EVA Lockers, 3× Keel Lights, 1× Black Box SOS Beacon & Isolated Battery, 4× In-Situ Button Panels (Palette 9) | 11 | 2.5 t | 110 |
| **TOTAL CONSIST (DRY)** | Complete Expeditionary Mining Locomotive & Mobile FOB | **1,039** | **≈ 978.5 t** | **7,198** |

---

#### 0.8 Master Player Circulation, Access Corridors & Jefferies Tubes Schematic

```
[LONGITUDINAL PROFILE · CREW CIRCULATION & MAINTENANCE PATHS (LOOKING STARBOARD)]

     ◄────────── CARRIAGE 1: TRACTOR / EXCAVATOR ──────────►     ◄────────── CARRIAGE 2: FOB / UTILITY ──────────►
     L6                                                         ┌──[KESTREL]──────[MULE PAD CATWALK]──────┐
     L5      ┌──[C2 BRIDGE]──────[BOOM PEDESTAL DECK]           │ (L6 Catwalks & Solar Array Access)      │
     L4      │   (3-Seat C2)     │ (Turret Service Hatch)       ├─────────────────────────────────────────┤
     L3 ┌────┴───────────────────┴─────────────────────────┐    │ [FIELD WORKSHOP & MEDICAL HABITAT]      │
        │ [PORT JEFFERIES TUBE] ──► [POWER CORE VAULT]      ╞════╡ (Refinery · Assembler · MedBay · Tanks) │
     L2 │ (Access to Buffer & Core) (Reactors / H2 Engine)  │    │ [SUB-FLOOR BATTERY VAULT (3x BAT)]      │
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
    Z:  +8 ├───────────────┴───────────────┬───────────────┴───────────────┤
           │ [AIRLOCK DOOR]                │ [BOOM PEDESTAL LADDER UP]     │  ◄── Airlock Vestibule
    Z:  +6 ├───────────────┬───────────────┴───────────────┬───────────────┤
           │ [PORT JEFF-   │                               │ [SOLID ARMOR] │
    Z:  +3 │  ERIES TUBE]  │ [MAS TRACTOR BUFFER (3×3×3)]  │               │  ◄── Ore Buffer Pass
           │ (Catwalk &    │ (Manual Port on Port Flank)   │               │
    Z:  +1 │  Lighting)    │                               │               │
           ├───────────────┼───────────────┬───────────────┼───────────────┤
    Z:  −2 │ [VAULT DOOR]  │ [BAT 1]       │ [REACTOR 1]   │ [BAT 2]       │
    Z:  −3 │               │ [BAT 3]       │ [REACTOR 2]   │ [ARMOR]       │  ◄── Power Core Vault
    Z:  −4 │               │ [ARMOR]       │ [H2 ENGINE]   │ [ARMOR]       │
    Z:  −7 ├───────────────┴───────────────┴───────────────┴───────────────┤
    Z:  −8 │ ══════════════ [GANGWAY FORWARD SLIDING DOOR] ═══════════════ │  ◄── Aft Bulkhead
           ╞═══════════════════════════════════════════════════════════════╡
           │  [INTER-CARRIAGE ARTICULATED ENCLOSED GANGWAY (Z −9 to −8)]   │  ◄── Flexible Walkway
           ╞═══════════════════════════════════════════════════════════════╡
    Z:  −9 │ ══════════════ [GANGWAY AFT SLIDING DOOR] ═══════════════════ │  ◄── Utility Fwd Bulkhead
           ├───────────────┬───────────────┬───────────────┬───────────────┤
    Z: −10 │ [SOLID ARMOR] │               │ [FLIGHT LADDER UP]            │  ◄── Stair/Ladder to L6
           ├───────────────┤ [CENTRAL      ├───────────────┬───────────────┤
    Z: −12 │ [REFINERY]    │  WORKSHOP     │ [MEDICAL BAY] │ [CRYO POD]    │  ◄── Field Metallurgy
    Z: −14 │ [ASSEMBLER]   │  AISLE]       │ [O2/H2 GEN 1] │ [O2/H2 GEN 2] │  ◄── Fabrication & O2
    Z: −18 │ [H2 TANK 1]   │ (X0 Walkway)  │ [H2 TANK 2]   │ [ARMOR]       │  ◄── Fuel Bunkering
    Z: −22 │ [ARMOR]       │ [MAS HOLD COMPONENTS (3×3×3)] │ [ARMOR]       │  ◄── Parts Hold
    Z: −25 ├───────────────┴───────────────┬───────────────┴───────────────┤
    Z: −26 │                               │ [STERN LADDER DOWN TO L2]     │  ◄── Stern Dump Access
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
       [PHASE 10-1 SPATIAL LOCATOR HUD]
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

#### Pre-Step Parts Tray: Phase 10-1
* 240× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 80× Heavy Armor Slope / Corner Blocks (Palette 0)
* 6× Wheel Suspension 5×5 (3 Port, 3 Starboard)
* 24× Large Conveyor Pipe (Palette 4: Processing Teal)
* 6× Large Conveyor Junction (Palette 4)
* 4× Large Piston (Palette 1: Hazard Amber)
* 4× Large Magnetic Plate (Palette 9: High-Viz Safety Yellow)

---

#### Step 10.1 · Twin Longitudinal Chassis Box Girders & Belly Skids (L1-L2)

Weld two continuous, reinforced heavy-armor box girders spanning 20 blocks from Aft Bulkhead (Z −8) to Bow Bulkhead (Z +12) at X = ±3. Connect them with transverse beam ties at each axle datum to form the central conveyor raceway channel:

```
Plan view, Y = L2 (Tractor Chassis Box Girder Deck)
        X: −3          0          +3
    Z: +12 ■═══════════╤═══════════■  BOW BULKHEAD (Z +12 Datum)
           ║           │           ║  Central Conveyor Trough (X0)
    Z:  +9 ║  BEAM TIE │  BEAM TIE ║  ◄── Axle 1 Cross-Tie (X = ±3, Z +9)
           ║═══════════╪═══════════║
    Z:  +4 ║  BEAM TIE │  BEAM TIE ║  ◄── Axle 2 Cross-Tie (X = ±3, Z +4)
           ║═══════════╪═══════════║
    Z:  −1 ║  BEAM TIE │  BEAM TIE ║  ◄── Axle 3 Cross-Tie (X = ±3, Z −1)
           ║           │           ║
    Z:  −8 ■═══════════╧═══════════■  AFT HITCH BULKHEAD (Z −8 Datum)
```

1. Lay 2 parallel runs of Heavy Armor Full Blocks at X = −3 and X = +3 from Z −8 to Z +12 at Level L2.
2. At Level L1 (underside), install continuous Heavy Armor Half-Slopes facing downward along X0 to form a beveled belly skid channel that deflects boulders during overland travel.
3. Install transverse Heavy Armor Ties connecting the box girders at Z = +9, Z = +4, and Z = −1.
4. Route the large conveyor raceway along X0 at Level L1, securing pipes with conveyor junctions at Z = +9, Z = +4, and Z = −1 for future branch taps.

---

#### Step 10.2 · 5×5 Off-Road Suspension Wheel Mounts & Master 3-Axle Tuning (L1)

Mount 3 pairs of 5×5 large suspension wheel units outward along the chassis girders at X = ±4. Configure steering geometry: front two axles steer in proportion to wheel turn; rear axle 3 remains locked at 0° to prevent sideways tire scrub:

```
Plan view, Y = L1 (Tractor Wheel Suspension Layout)
        X: −5          0          +5
    Z:  +9 [WH 1 (P)] ═══════════ [WH 2 (S)]   ◄── Axle 1 (+30.0° Steer)
    Z:  +4 [WH 3 (P)] ═══════════ [WH 4 (S)]   ◄── Axle 2 (+15.0° Steer)
    Z:  −1 [WH 5 (P)] ═══════════ [WH 6 (S)]   ◄── Axle 3 (0.0° Locked Drive)
```

```
Front Elevation Cross-Section, Y = L1 (Axle 1 Wheel Hub & Ground Clearance)
    Port [-X]                                                          Starboard [+X]
    X: -5            X: -3            X0              X: +3            X: +5
    [WHEEL 1] ────── [GIRDER] ─── [CONVEYOR] ─── [GIRDER] ────── [WHEEL 2]
    │ 5x5 Hub        │ Heavy      │ Large Pipe   │ Heavy        │ 5x5 Hub
    │                └────────────┴──────────────┴──────────────┘
    ▼                             ▼ Belly Skid (1.25 m Clearance)
    ════════════════════════════════════════════════════════════════════ Terrain
```

| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 1** | Z = +9  | `WH 1, 2` | `30.0°` | Normal | `100%` | `50% / 85%` | `70%` |
| **Axle 2** | Z = +4  | `WH 3, 4` | `15.0°` | Normal | `100%` | `50% / 85%` | `70%` |
| **Axle 3** | Z = −1  | `WH 5, 6` | `0.0°`  | OFF (Locked) | `100%` | `55% / 90%` | `75%` |

---

#### Step 10.2B · Tailings Aggregate & Roadbed Spreader Underbelly Manifold (L1)

To prevent cargo holds from clogging with thousands of tons of low-value crushed stone, install a selective tailings venting manifold beneath the chassis along centerline X0 at Z = 0:
* **Conveyor Tap:** 1× Conveyor Sorter (`MAS Sorter Tailings`) tapped into the central conveyor trunk at [X0, Z0, Y = L1] facing downward.
* **Discharge Nozzles:** 2× Conveyor Ejectors (`MAS Ejector Tailings 1, 2`) angled 45° aft (−Z) beneath Axle 3.
* **Sorter Filter:** Whitelist set to `Stone` and `Gravel` · `Drain All = ON`.
* **Automation:** Managed by Event Controller `MAS EC Tailings`. When Tractor ore buffer fill exceeds 50%, ejectors activate, laying down a smooth gravel roadbed behind the driving wheels while retaining high-value ores (Fe, Ni, Co, U).

```
Underbelly Profile · Tailings Aggregate Spreader (Z0, L1)
     Central Conveyor Raceway (L1)
                  │
                  ▼
       [MAS Sorter Tailings] (Filter: Stone / Gravel)
                  │
                  ▼
       [MAS Ejector Tailings 1, 2] ──► Angled Aft (−Z)
                  │
                  ▼ (Vents Stone Tailings Behind Axle 3)
     ══════════════════════════════════════════════════ Crushed Stone Roadbed
```

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
       [PHASE 10-2 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 14 L × 3 H B (17.5 × 35.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z −8 to +6, Y = L2 to L4
       Subsystem Datums:
       · Ore Buffer Hold: Z +1 to +3, centered at X0
       · Primary Power Core: Z −4 to −2, centered at X0
       · 2-Axis Drawbar Hitch Center: [X0, Z −8, Y = L2]
```

#### Pre-Step Parts Tray: Phase 10-2
* 2× Small Nuclear Reactor (15 MW each, Palette 6: High-Voltage Red)
* 1× Hydrogen Engine (5 MW, 1×1×2 B, Palette 6: High-Voltage Red)
* 3× Large Battery (3 MWh each, 9.0 MWh total, Palette 6)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 1× Large Advanced Rotor (Palette 2: Heavy Rail Crimson)
* 1× Large Motor Hinge (Palette 2)
* 12× Grated Catwalk Block & 2× Sliding Door (Palette 9: High-Viz Yellow)
* 18× Large Conveyor Pipe & 6× Curved Conveyor Tube (Palette 4: Processing Teal)
* 120× Heavy Armor Full Block & Slopes (Palette 0)

---

#### Step 10.3 · Primary Power Core Vault & Electrical Shielding (Z −4 to −2, L2-L4)

Install 2 Small Nuclear Reactors, 1 Hydrogen Engine, and 3 Large Batteries behind the ore buffer hold. Enclose the plant in double-walled heavy armor bulkheads (Palette 6):

```
Plan view, Y = L3 (Tractor Power Core Vault)
        X: −2     0    +2
    Z: −2  ┌───────────────┐
           │[BAT 1][R 1][BAT 2]│  ◄── Reactor 1 (15 MW) flanked by Large Batteries
    Z: −3  │[BAT 3][R 2][Armor]│  ◄── Reactor 2 (15 MW) flanked by Battery 3
    Z: −4  │[Armor][H2E][Armor]│  ◄── Hydrogen Engine (5 MW standby failover)
           └───────────────┘
           Nuclear + H2 Baseload Total: 35.0 MW · Tractor Battery Reserve: 9.0 MWh (36 MW Peak)
```

1. Place Reactor 1 (`MAS Reactor 1`) at [X0, Y = L3, Z −2] and Reactor 2 (`MAS Reactor 2`) at [X0, Y = L3, Z −3].
2. Flank the reactors with Large Batteries (`MAS Battery 1, 2, 3`) at [X −2, Z −2], [X +2, Z −2], and [X −2, Z −3].
3. Connect reactor conveyor ports directly to the central conveyor spine along X0 for automated uranium fuel feeding.
4. Encase the compartment in Heavy Armor blocks painted Palette Slot 6 (High-Voltage Alert Red).
5. Install armored sliding door `MAS Door Vault` at [X −2, Z −2, L3] accessing the Port Jefferies tube.

---

#### Step 10.3B · Standby Emergency Hydrogen Engine Installation (Z = −4, X0, L3)

Install 1 Large Grid Hydrogen Engine (`MAS Engine H2`, 1×1×2 B, 5.0 MW) directly aft of Reactor 2 on centerline X0. Connect its conveyor port to the central spine to draw fuel from Carriage 2's 10,000,000 L hydrogen reserve hold:
* **Function:** Standby cold-start generator providing instantaneous 5.0 MW electrical failover upon uranium fuel depletion, reactor maintenance, or peak tractive climbs.
* **Automation:** Supervised by Event Controller `MAS EC Power` (configured to auto-ignite `MAS Engine H2` when nuclear reactor fuel mass drops below 1.0 kg or battery state of charge drops below 20%).
* **Manual Port:** Features an exposed aft sump inspection port and fuel shutoff valve accessible from the vault interior.

---

#### Step 10.3C · Port Maintenance Jefferies Tube & Physical Buffer Access Hatch (Z +6 to −7, X −2, L3)

Construct a continuous, pressurized 1-block wide maintenance crawlway running along the Port side of Carriage 1 at Level L3:
* **Circulation Route:** Runs from the forward airlock vestibule at Z +6 aft to the gangway door at Z −7, bypassing the Large Cargo buffer and Power Vault.
* **Catwalk Decking:** Paved with Grated Catwalk blocks painted Palette Slot 9 (High-Viz Safety Yellow) with recessed amber bulkhead lighting.
* **Physical Buffer Hatch:** Mount an exposed cargo access port on the Port face of `MAS Tractor Buffer` at [X −2, Z +2, L3]. Allows an engineer to manually inspect ore contents, clear conveyor clogs, or withdraw small hand tools.
* **Power Vault Access:** Sliding door `MAS Door Vault` at [X −2, Z −2, L3] provides direct walk-in access to the reactor and battery bank.

---

#### Step 10.4 · Tractor Ore Buffer Hold (Z +1 to +3, L2-L4)

Mount 1 standard 3×3×3 B Large Cargo Container (`MAS Tractor Buffer`) on centerline X0. It receives raw ore directly from the boom intake pipe and buffers material during active excavation before routing to the trailing utility carriage:

```
Plan view, Y = L3 (Tractor Buffer Hold)
        X: −1    0   +1
    Z: +3  ┌───────────┐
           │MAS TRACTOR│  ◄── Large Cargo Container (3×3×3 B)
    Z: +1  │  BUFFER   │      (Palette Slot 5: Logistics Emerald)
           └───────────┘
```

1. Position the Large Cargo Container between Z +1 and Z +3, spanning X −1 to +1 and Y = L2 to L4.
2. Align bottom and top 3×3 large conveyor ports:
   * Bottom port connects directly to the central chassis conveyor trough (L1).
   * Top port aligns with the dorsal boom turret riser (L4).
   * Aft port connects to the power core uranium supply line and drawbar hitch pipe.

---

#### Step 10.5 · Heavy Motorized 2-Axis Drawbar Hitch & Pyrotechnic Severance (Z = −8, X0, L2)

At the aft bulkhead of the tractor, install a heavy-duty 2-axis articulated drawbar hitch that connects the tractor to the utility carriage mechanically and routes the main large conveyor line:
1. **Azimuth (Yaw) Joint:** 1× Large Advanced Rotor mounted at Z = −8 facing Aft (−Z). Allows the consist to articulate horizontally when turning.
2. **Pitch Joint:** 1× Motorized Large Hinge connected directly to the rotor head. Allows the consist to traverse crests and dips over rough terrain without lifting wheels.
3. **Pyrotechnic Severance Merge Block (`MAS Hitch Sever Tongue`):** Mount 1× Large Grid Merge Block directly on the drawbar tongue at [X0, Z −8, Y = L2]. In emergency catastrophic events (runaway fire, trailer rolling over a cliff, nuclear containment breach), triggering `Hitch Eject` physically decouples Carriage 1 and Carriage 2 instantly, allowing the tractor to escape as an independent 6×6 unit or leaving the utility carriage as a self-powered static FOB.
4. **Settings:** Share Inertia Tensor = `ON` · Braking Torque = `45 MN·m`.

```
2.5D Isometric Callout · 2-Axis Articulated Drawbar Hitch & Severance
    Tractor Aft Bulkhead (Z −8)
           ║
           ╠══[ADVANCED ROTOR] (Horizontal Yaw Articulation: −45° to +45°)
           ║        │
           ║        ▼
           ║   [MOTOR HINGE] (Vertical Pitch Articulation: −25° to +25°)
           ║        │
           ║        ▼
           ║   [MERGE BLOCK: MAS HITCH SEVER TONGUE] (Emergency Decoupling)
           ║        │
           ║        ▼
           ╚════════╪═════════════════════════════════════════► To Utility Carriage (Carriage 2)
                    ▼
           [LARGE CONVEYOR PASS-THROUGH PIPE]
```

```
Hitch Articulation Kinematic Envelope:
· Horizontal Yaw (Turning): Rotor limits set to Min: -45.0°, Max: +45.0°.
  Torque: 45 MN·m · Velocity: 0.5 rpm · Damping: 85%.
· Vertical Pitch (Crest / Dip): Hinge limits set to Min: -25.0°, Max: +25.0°.
  Torque: 45 MN·m · Velocity: 0.5 rpm · Damping: 90%.
· Roll (Twist): Locked rigid via structural heavy armor wishbone; terrain twist absorbed by wheel suspension travel.
```

---

#### Step 10.5B · Smart Anti-Jackknife Dynamics & High-Speed Sway Lockout

To eliminate trailer sway during high-speed transit and prevent destructive jackknifing when reversing down steep slopes:
* **High-Speed Cruising Mode (> 35 km/h):** Event Controller `MAS EC Hitch Speed` monitors vehicle speedometer. When velocity exceeds 35 km/h, `MAS Hitch Rotor` braking torque increases dynamically to `100 MN·m`, locking the drawbar straight and eliminating harmonic trailer snaking.
* **Reversing Jackknife Prevention:** When transmission shifts into reverse, Event Controller `MAS EC Hitch Rev` clamps rotor yaw limits from `±45.0°` down to `±15.0°` and commands Axle 5 to track in positive alignment, physically preventing the utility carriage from folding sideways.

---

#### Step 10.5C · Inter-Carriage Articulated Enclosed Accordion Gangway (Z −9 to −8, L3)

Directly above the mechanical drawbar hitch, construct a heavy-duty flexible pressurized accordion gangway spanning the 1-block mechanical gap between Tractor Aft Bulkhead (Z −8) and Utility Forward Bulkhead (Z −9):
* **Structure:** Heavy-duty reinforced canvas and rubber bellows backed by sliding armor plates, accommodating up to ±45° yaw and ±25° pitch articulation without breaking airtight seal.
* **Isolation Doors:** Sliding door `MAS Door Gangway Fwd` at [X0, Z −8, L3] and sliding door `MAS Door Gangway Aft` at [X0, Z −9, L3]. Allows either carriage to maintain full atmosphere even if the other carriage is breached.
* **Walkway Deck:** Paved with slip-resistant flexible grated catwalks, allowing crew members to safely walk between carriages at full travel speed without exposure to drawbar pinch hazards or external dust storms.

---

#### Step 10.5D · Ground-Level EVA Survival Lockers & Underbelly Inspection Lights (L1-L2)

To ensure field technicians can access emergency life support and tools without entering the pressurized habitat:
* **Flank EVA Lockers:** Mount 2× Small Cargo Containers (`MAS Locker Port` at [X −5, Z −4, Y = L2] and `MAS Locker Stbd` at [X +5, Z −4, Y = L2]), recessed flush into the lower armored skirts. Painted Palette Slot 9 (High-Viz Safety Yellow). Accessible directly from ground level; pre-stocked with emergency hydrogen suit bottles, oxygen bottles, tool welders, and 5.56mm/25mm ammunition. Plumbed with auto-refill bottle feeds.
* **Underbelly Keel Lighting:** Mount 3× Interior Lights (`MAS Lights Underbelly`) along the centerline chassis keel at [X0, Z +5], [X0, Z0], and [X0, Z −5] at Level L1. Casts low-intensity amber illumination across wheel suspension hubs, outrigger cylinders, and the tailings aggregate manifold for night maintenance.

---

#### Step 10.5E · Native Threat Decoys, Recovery Gyros & Black Box SOS Cell (L1-L3)

To guarantee survival and asset protection under severe attack or total vehicle immobilization (Rule C4 native block compliance):
* **Ballistic Decoy Sponsons (`MAS Decoy Flank Port/Stbd`, `MAS Decoy Stern`):** Mount 3× Decoys encased within heavy armor and blast door blocks at [X −5, Z −4, Y = L2] (Port skirt), [X +5, Z −4, Y = L2] (Starboard skirt), and [X0, Z −26, Y = L2] (Stern bumper). In Space Engineers, automated turret AI targets Decoys first, diverting artillery, rockets, and gatling rounds into the heavily armored lower chassis skirts away from the bridge canopy and reactors.
* **Self-Righting Keel Gyroscopes (`MAS Gyro Recovery 1, 2`):** Mount 2× Large Grid Gyroscopes in the lower chassis keel at [X0, Z0, Y = L1]. During normal transit, `Override = OFF`. If the consist rolls onto its side or inverts on steep terrain, toggling `Override = ON` (Roll +100%) applies instant angular torque to roll the consist back onto its wheels.
* **Armored Black Box & SOS Distress Beacon (`MAS Beacon Distress`):** Encase 1× Radio Antenna / Beacon (`MAS Beacon Distress`, 50 km broadcast) and 1× Small Battery (`MAS Battery BlackBox`, isolated, `Recharge = OFF`) in a 6-sided heavy armor capsule at [X0, Z +7, Y = L1]. Native Event Controller `MAS EC BlackBox` monitors main grid power: upon total electrical blackout (0.0 MW), the isolated battery auto-activates, broadcasting an emergency GPS distress ping `SOS-MASTODON-01-LOST`.
* **Power Vault Scuttle Charge (`MAS Warhead Scuttle Power`):** Mount 1× Large Grid Warhead between the two nuclear reactors at [X0, Z −3, Y = L3]. Disarmed by default. Linked to the bridge scuttle countdown loop for anti-capture asset destruction.
* **Power Vault In-Situ Control Panel (`MAS BP Power Vault`):** Mount 1× Sci-Fi 4-Button Panel on the Jefferies tube wall at [X −2, Z −1, Y = L3] (Button 1: Vault Door Toggle, Button 2: Reactor SCRAM, Button 3: H2 Engine Cold-Start, Button 4: Battery Bank Shore Charge).

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

---

### Phase 10-3 · Hydraulic Bedrock Outriggers & 45 m Long-Reach Excavation Boom (Levels L2-L8)

```
       [PHASE 10-3 SPATIAL LOCATOR HUD]
       Envelope: 15 W × 22 L × 6 H B (37.5 × 55.0 × 15.0 m operating envelope)
       Coordinate Range: X −7 to +7, Z +5 to +22, Y = L2 to L8
       Pedestal Datum: [X0, Z +6, Y = L4] (Dorsal Turret Ring)
       Outrigger Locations:
       · Port Forward Outrigger: [X −5, Z +11, Y = L1]
       · Starboard Forward Outrigger: [X +5, Z +11, Y = L1]
       · Port Aft Outrigger: [X −5, Z −6, Y = L1]
       · Starboard Aft Outrigger: [X +5, Z −6, Y = L1]
```

#### Pre-Step Parts Tray: Phase 10-3
* 4× Large Piston (Palette 1: Hazard Amber)
* 4× Seismic Surveying Landing Gear (`MAS Seismic Outrigger 1..4`, TMS Mod 2819263781, Palette 1)
* 1× Large Magnetic Plate (`MAS Tool Grapple`, Palette 9: High-Viz Safety Yellow)
* 1× Large Magnetic Plate (`MAS Clamp Boom Stow`, Palette 1: Hazard Amber · Travel Cradle at Z −7)
* 1× Large Conveyor Elbow (90° Downward Tool Head Adapter, Palette 4: Processing Teal)
* 2× Large Advanced Rotor (Palette 3: Excavator Ochre)
* 3× Large Motor Hinge (Palette 3)
* 2× Simple Laser Multitool (Mining Mode, Palette 3)
* 1× Camera + 2× Spotlight + 1× Sensor (Tool Head)
* 40× Large Conveyor Pipe (Palette 4: Processing Teal)
* 80× Heavy Armor Slope & Box Girder Blocks (Palette 3)

---

#### Step 10.6 · 4× Corner Bedrock Hydraulic Outriggers (L1-L2)

Install 4 hydraulic stabilizer legs at the outer corners of the tractor chassis. Each outrigger consists of a downward-facing Large Piston capped with a Large Magnetic Plate:
* Piston Settings: Velocity = `0.2 m/s` · Share Inertia Tensor = `ON`.
* Magnetic Plate: Auto-Lock = `ON`.
* Group: `MAS Outriggers`.
* **Operating Logic:** When deployed on site, pistons extend downward, pressing the magnetic plates into planet voxels and locking rigidly. This completely isolates the suspension, preventing any tipping or chassis flex during boom sweeps.

```
Front Cross-Section, Outriggers Deployed at Bedrock Baseline:
     [OUTRIGGER PORT]                                            [OUTRIGGER STBD]
       [PISTON ↓]                                                  [PISTON ↓]
     [MAG PLATE P] ─── [WH 1] ─── [BELLY SKIDS] ─── [WH 2] ─── [MAG PLATE S]
    ══════════════════════════════════════════════════════════════════════════ Terrain
```

```
Outrigger Deployment Sequence:
1. Chassis comes to complete stop. Handbrake = ON.
2. Wheel suspensions override: Strength = 100%, Friction = 100%.
3. Outrigger Pistons extend: Max distance = 2.5 m (1 B stroke).
4. Magnetic Plates contact terrain: Auto-Lock triggers (Green lock state).
5. Event Controller `MAS EC Stabilizers` verifies 4/4 locks -> releases Boom Joint brakes.
```

---

#### Step 10.6B · Auto-Leveling Bedrock Jacks (Sloped Terrain Pitch & Roll Compensation)

When parking on sloped hillsides (up to 20° grade), the 4 corner outrigger pistons execute an automated differential extension cycle to bring the vehicle deck to true horizontal:
* **Level Sensing:** Event Controller `MAS EC Level` (or `[MAS] PB Excavator`) monitors the artificial horizon pitch and roll angles from `MAS Helm`.
* **Differential Stroke:** Downhill pistons extend further (up to 2.5 m stroke) while uphill pistons extend only until initial voxel contact, bringing the deck to $0.0^\circ \text{ Roll}$ and $0.0^\circ \text{ Pitch}$.
* **Lock State:** Once leveled, all 4 Magnetic Plates lock into bedrock simultaneously, creating a perfectly level, rigid foundation on hillsides.

```
TERRAIN-TILT AUTO-LEVELING (Looking Bow, Y = L1-L2)

 [UNLEVELED ON 15° SLOPE]                         [AUTO-LEVELED DECK (0.0° ROLL)]
      Chassis Tilted 15° Downhill                     Chassis Leveled to Gravity Horizon
         ┌───────────────────┐                           ┌───────────────────┐
        ╱                   ╱                           │     MAIN DECK     │ (0.0° Roll)
       ╱    TILTED CAB     ╱                            └─┬───────────────┬─┘
      └──────┬───────┬────┘                               │ [Piston 0.8m] │ [Piston 2.2m]
     [Piston]│       │[Piston]                           ▼               ▼
      (Short)▼       ▼(Short)                       [MAG LOCK]      [MAG LOCK]
      ═════════════════════════ 15° Hillside        ═════════════════════════ 15° Hillside
```

---

#### Step 10.6C · Deep Seismic Subsurface Voxel Prospecting Array (TMS Mod Integration)

Equip each of the 4 corner outrigger feet with **Seismic Surveying Landing Gear** from the active TMS mod suite (`docs/rig/mods.md`, Mod ID `2819263781`):
* **Hardware Integration:** Replace standard magnetic plate heads with `MAS Seismic Outrigger 1..4` (Palette 1: Hazard Amber).
* **Automated Resonance Pulse:** When `MAS EC Level` locks the outriggers into bedrock at $0.0^\circ$ pitch and roll, Event Controller `MAS EC Seismic` triggers an active acoustic survey pulse.
* **Subsurface Detection:** Maps subterranean mineral density down to $500\text{ m}$† depth in a 300 m radius beneath the anchored consist, identifying deep rare ore deposits (Uranium, Platinum, Gold) far beyond the 150 m range of standard ore detectors.
* **IGC Telemetry:** Coordinates, estimated depth, and deposit signatures are automatically formatted and published over IGC to `TMS.SCOUT.VEIN` and populated on Station 3 Tactical Video Wall.

---

#### Step 10.7 · Heavy Turret Pedestal & Azimuth Advanced Rotor (Z = +6, X0, L4)

Mount an Advanced Rotor facing upward (+Y) on the dorsal deck at centerline [X0, Z +6, Y = L4]. This serves as the primary azimuth turntable, providing **360° continuous horizontal rotation** for the excavation boom with an unrestricted large-conveyor pass-through:
* **Mounting Coordinates:** [X0, Z +6, Y = L4] (Directly behind C2 Bridge aft wall, elevated 10.0 m above terrain).
* **Rotor Settings:** Share Inertia Tensor = `ON` · Braking Torque = `50 MN·m` · Velocity = `0.75 rpm`.
* **Symmetrical Flank Reach:** Because the turntable is mounted on the exact centerline (X0), the arm provides identical symmetrical reach to both Port (−X) and Starboard (+X):
  - Chassis Half-Width: 13.75 m (5.5 blocks from centerline to outer tire/skirt).
  - Physical Boom Reach: 35.0 m (14 blocks fully extended).
  - Reach Beyond Either Flank: **21.25 m (8.5 blocks)** of physical reach into open terrain off either side.
  - Effective Extraction Reach: **46.25 m to 56.25 m** when including laser mining beam projection.

---

#### Step 10.8 · 5-DOF Articulated Boom Kinematic Chain & Travel Stow Trough (L4-L8)

Assemble the high-articulation robotic excavation and recovery crane:
1. **Shoulder Pitch Hinge (`MAS Shoulder Pitch`):** Motorized Large Hinge connected to the azimuth rotor head. Pitch limits: `−20.0°` (undercutting terrain / stowed trough) to `+75.0°` (high reach). Torque: `45 MN·m`.
2. **Main Boom Arm (7 B Span):** Boxed heavy armor casing enclosing the primary large conveyor pipe (17.5 m length).
3. **Elbow Pitch Hinge (`MAS Elbow Pitch`):** Motorized Large Hinge at the mid-joint. Articulation limits: `−100.0°` (flexed descent) to `+45.0°` (extended reach). Torque: `45 MN·m`.
4. **Forearm Girder (5 B Span):** Tapered structural girder carrying internal piping and utility cabling (12.5 m length).
5. **90° Downward Conveyor Elbow Adapter:** Large conveyor elbow mounted at the tip of the forearm. Sets the neutral tool orientation strictly downward (−Y) toward planetary gravity.
6. **Wrist 2-Axis Articulator:**
   * Wrist Pitch Hinge (`MAS Wrist Pitch`): Motorized Large Hinge connected to the 90° elbow. Provides `−60.0°` to `+60.0°` pitch compensation around true vertical.
   * Wrist Roll Rotor (`MAS Wrist Roll`): Large Advanced Rotor providing continuous 360° tool roll (5.0 rpm) for circular column coring.
7. **Chassis Travel Stow Cradle (`MAS Clamp Boom Stow`):** Large Magnetic Plate mounted flush in the Tractor dorsal spine trough at [X0, Z −7, Y = L4]. Flanked by twin heavy armor coamings running from Z +5 to Z −8.

```
2.5D Isometric Callout · 35 m Long-Reach Boom with 90° Downward Tool Head
       Chassis Pedestal (Z +6, X0, L4)
              ║
              ╠══[TURRET AZIMUTH ROTOR] ────────► 360° Continuous Horizontal Pan
              ║        │
              ║        ▼
              ║   [SHOULDER PITCH HINGE] ───────► Elevation: −20° to +75°
              ║        │
              ║        ▼
              ║   [Main Boom Arm · 7 B Span] (17.5 m Heavy Cased Pipe)
              ║        │
              ║        ▼
              ║   [ELBOW PITCH HINGE] ──────────► Articulation: −100° to +45°
              ║        │
              ║        ▼
              ║   [Forearm Girder · 5 B Span] (12.5 m Tapered Casing)
              ║        │
              ║        ▼
              ║   [90° DOWN CONVEYOR ELBOW] ────► Neutral Reference = Straight Down (−Y)
              ║        │
              ║        ▼
              ║   [WRIST PITCH HINGE] ──────────► Pitch Compensation: −60° to +60°
              ║        │
              ║        ▼
              ║   [WRIST ROLL ROTOR] ───────────► 360° Continuous Spin (5 rpm Core Bore)
              ║        │
              ║        ▼
              ║   [LASER MINING TOOL HEAD]
              ║   (2× Laser Multitools + Tool Grapple + Cam + Lights)
```

```
Boom Operating Configurations:

1. STOWED TRANSIT CONFIGURATION (Traveling Overland at 35 km/h):
   Turret Azimuth: 180.0° (Slewed Aft along Centerline X0)
   Shoulder Pitch:  -5.0° (Resting flat in armored dorsal spine trough)
   Elbow Pitch:      0.0° (Flat inline with main boom arm)
   Wrist Pitch:      0.0° (Tool head locked into chassis deck clamp)
   Stow Clamp:     LOCKED (`MAS Clamp Boom Stow` at [X0, Z −7, L4] Green Lock)
   Total Length:   14 Blocks (35.0 m) from Z +6 to Z −8 (Spans Carriage 1 only)
   Hitch Gap:      Zero Overhang (7 blocks clear of hitch at Z −8 to −9)
   Cab Sightlines: 100% Unobstructed (Zero forward canopy blockage)
   Transit Height: 5.2 m above ground (Fits under 6.0 m hangar gates)

2. TARGET-CENTRIC VERTICAL COLUMN BORING CONFIGURATION (Active Mining):
   Turret Azimuth: Slew to Target Bearing φ = atan2(Xt, Zt)
   Shoulder Pitch: +15.0° to +55.0° (Adjusted per radial distance R)
   Elbow Pitch:    -10.0° to -70.0° (Adjusted per depth Y)
   Wrist Pitch:    Counter-Pitches to lock Tool Vector strictly vertical ([0, -1, 0])
   Wrist Roll:     Spins continuously at 5.0 rpm for circular core coring
   Lateral Sweeps: Arm executes concentric circular orbits (ΔX, ΔZ) around target (Xt, Zt)
   Operating Span: 6.0 m to 35.0 m from turret · Max Depth Reach: -25.0 m (-55 m with beams)
```

---

#### Step 10.8B · Active Gravity-Horizon Tool Head Leveling & True Vertical Lock

To enable precision vertical column boring and flat quarry terracing:
* **True Vertical Downward Lock (Column Boring):** With the 90° downward elbow adapter setting the tool neutral vector along −Y, the Wrist Pitch Hinge counter-articulates in exact opposite ratio to the net arm angle ($\theta_{\text{wrist}} = -(\theta_{\text{shoulder}} + \theta_{\text{elbow}})$). This locks the twin mining lasers strictly parallel to planetary gravity ($\vec{u}_{\text{tool}} = [0, -1, 0]$) across the entire radial operating envelope ($6.0\text{ m} \le R \le 35.0\text{ m}$).
* **Horizontal Terracing Mode (Bench Quarrying):** By rotating `MAS Wrist Pitch` by $+90.0^\circ$, the laser beams deflect forward, leveled strictly parallel to the planet surface for cutting flat bench floors.
* **Proximity Safety Buffer:** Sensor `MAS Sensor Boom` mounted on the tool head is configured with a 4.0 m forward detection sphere. If terrain voxels or vehicle blocks enter the buffer during rapid slewing, it cuts laser beam power and locks joint brakes within 1 tick, preventing tool head destruction.

```
BOOM ELEVATION WITH HORIZON-LEVELED TOOL HEAD

                  [SHOULDER ELEVATED +35°]
                         ╱
                        ╱  Main Arm
                       ╱
                      ▼
               [ELBOW FLEXED -20°]
                      ╲
                       ╲  Forearm
                        ╲
                         ▼
                  [90° DOWN ELBOW] ── (Base direction = −90° DOWN)
                         │
                         ▼
                  [WRIST PITCH HINGE] ── (Counter-Pitches -15° to cancel arm slope)
                         │
                         ▼
               ┌───────────────────┐
               │  [LASER MULTI]    │ ════════════► Fires Strictly DOWN (−Y)
               └───────────────────┘               (Parallel to Planetary Gravity)
```

---

#### Step 10.9 · Laser Mining Tool Head, 90° Downward Elbow & Heavy Recovery Grapple

At the tip of the wrist articulator, mount the unified excavation and field recovery head:
* 1 × Large Conveyor Elbow: Mounted at the tip of the forearm, routing the conveyor line 90° downward toward ground baseline.
* 1 × Motor Hinge (`MAS Wrist Pitch`): Mounted to the elbow head, providing `±60.0°` pitch leveling around the vertical axis.
* 1 × Large Advanced Rotor (`MAS Wrist Roll`): Providing continuous 360° axial spin (5.0 rpm) for circular column coring.
* 2 × Simple Laser Multitool (Mode: `Mining Beam`) · `MAS Laser 1, 2` mounted on the roll face, spaced 2.5 m apart.
* 1 × Large Magnetic Plate (`MAS Tool Grapple`, Auto-Lock = OFF, Palette 9: High-Viz Safety Yellow) mounted centrally between the laser heads. Provides heavy lifting, vehicle righting, and cargo positioning capability with 45 MN·m joint holding power.
* 1 × Camera · `MAS Cam Boom` (high-magnification targeting crosshairs looking directly down the drill bore).
* 2 × Spotlight · `MAS Light Boom 1, 2` (floodlights illuminating the cutting bench and salvage targets).
* 1 × Sensor · `MAS Sensor Boom` (4.0 m safety stop to prevent head-to-terrain collision).

```
Front Elevation · Laser Mining Tool Head & Recovery Grapple (Tip of Forearm)
                  [90° DOWN CONVEYOR ELBOW]
                            │
                  [WRIST PITCH HINGE]
                            │
                  [WRIST ROLL ROTOR]
                            │
        ┌───────────────────┴───────────────────┐
        │               [CAMERA]                │
        │             MAS Cam Boom              │
        ├───────────────┬───────┬───────────────┤
        │  [LASER 1]    │[MAG]  │  [LASER 2]    │
        │  Simple Laser │TOOL   │  Simple Laser │
        │  Multitool    │GRAPPLE│  Multitool    │
        ├───────────────┼───────┼───────────────┤
        │ [SPOTLIGHT 1] │       │ [SPOTLIGHT 2] │
        └───────────────┴───────┴───────────────┘
```

#### Step 10.9B · Automated Excavation Macro Modes & Kinematic Sweep Sequences

To maximize extraction efficiency and support uncrewed autonomous operation, `[MAS] PB Excavator` implements 4 automated sweep and crane recovery macros, plus automated travel stow/deploy sequencing:

```
[AUTOMATED BOOM EXCAVATION & CRANE SWEEP PATTERNS]

1. BENCH QUARRY MODE (Horizontal Terracing)
   · Azimuth Turret sweeps across horizontal face: -45.0° (Port) to +45.0° (Starboard).
   · Shoulder Hinge incrementally steps down -2.5° per pass (0.5 B bench cut depth).
   · Wrist Pitch elevates +90.0° to maintain flat, drivable terrace floors parallel to surface.
   · Produces clean stair-stepped quarry cuts into mountainsides.

2. TARGET-CENTRIC VERTICAL COLUMN BORE MODE (Subterranean Access)
   · Accepts target vein waypoint (Xt, Zt) from seismic scan or Station 2 operator.
   · Turret Azimuth slews to target bearing; Shoulder and Elbow extend tool head over target.
   · Wrist Pitch counter-pitches to lock laser beams strictly vertical ([0, -1, 0]).
   · Wrist Roll rotor spins twin offset lasers at 5.0 rpm, cutting an initial 3.0 m core.
   · Motion controller translates the arm in automated 2D concentric circular sweeps (ΔX, ΔZ) around (Xt, Zt) to expand the column diameter (up to 12.5 m wide).
   · Steps downward ΔY = -1.0 m per pass, descending down to -25.0 m physical depth (-55 m with laser beam reach) into deep rare veins.

3. TRENCH & ROAD CARVE MODE (Overland Right-of-Way)
   · Boom deploys along centerline (Z+), executing downward scoop arc (-5.0 m to grade).
   · Tractor crawls forward at 2.0 km/h; outriggers cycle in creeping advance pattern.
   · Automatically clears and grades a level 15.0 m wide roadbed for Caterpillar land trains.

4. CRANE & VEHICLE RECOVERY MODE (Overland Field Salvage)
   · Laser Multitools deactivated; `MAS Tool Grapple` energized.
   · Station 2 CTC mouse control provides direct 3-axis teleoperation of the 45 m boom arm.
   · Allows precision attachment to overturned Caterpillar trailers, scout drones, or supply skids up to 42.5 m away.
   · Utilizes the 45 MN·m joint holding torque and hydraulic outriggers to right rolled vehicles, hoist heavy cargo modules, or lift massive boulders obstructing the haul road.

[AUTOMATED TRAVEL STOW & DEPLOY SEQUENCES]

· TRAVEL STOW SEQUENCE (`STOW`):
  1. Mining lasers deactivate; tool head retracts vertically clear of borehole.
  2. Turret Azimuth slews to 180.0° (Aft centerline).
  3. Shoulder, Elbow, and Wrist hinges align flat (0.0°).
  4. Boom lowers into armored dorsal spine trough; `MAS Clamp Boom Stow` at [X0, Z −7, L4] engages (Green Lock).
  5. Joint holding brakes clamp at 100% torque (50 MN·m Azimuth, 45 MN·m Hinges).
  6. Outriggers retract; consist is cleared for 35 km/h overland transit.

· WORK DEPLOY SEQUENCE (`DEPLOY`):
  1. Consist comes to full stop; handbrake engages.
  2. Outriggers extend and lock bedrock (verified by `MAS EC Stabilizers`).
  3. `MAS Clamp Boom Stow` releases.
  4. Shoulder elevates +15.0° to clear spine coamings.
  5. Slew to target waypoint azimuth and begin commanded macro.
```

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
       [PHASE 10-4 SPATIAL LOCATOR HUD]
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

#### Pre-Step Parts Tray: Phase 10-4
* 3× Control Station (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`, Palette 7: Fleet Command Navy)
* 2× Programmable Block (`[MAS] PB Excavator`, `[MAS] PB RIG//OS-Lite`, Palette 7)
* 3× AI Automaton Block (`MAS AI Move`, `MAS AI Task`, `MAS AI Defense`)
* 1× Laser Antenna (`MAS Comm Laser`, Palette 7)
* 1× Radio Antenna (`MAS Comm Radio`, 50 km broadcast, Palette 7)
* 1× Large Ore Detector (150 m subterranean range, Palette 4)
* 1× Survival Kit (Tractor Field Respawn, Palette 7)
* 14× Sloped Heavy Armor Glass (Bridge canopy)
* 6× LCD Panel (Tactical video wall and console telemetry HUDs)
* 1× Air Vent (`MAS Vent Bridge`, Hermetic Zone 1)
* 1× Large Warhead (`MAS Warhead Scuttle Bridge`, disarmed anti-capture charge)
* 1× Timer Block (`MAS Timer Scuttle`, 60-second scuttle loop)
* 2× Sci-Fi 4-Button Panel (`MAS BP Scuttle Bridge`, `MAS BP Airlock Vestibule`)
* 1× Large Gatling Turret (`MAS Turret Bow`, Palette 7: Fleet Command Navy)
* 2× Large Interior Turret (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`, Palette 7)

---

#### Step 10.10 · 3-Operator Custom Walk-In Armored Bridge & Multi-Page Hotbars

Construct the forward command bridge overlooking the terrain. Instead of a cramped 3×3×2 enclosed cockpit prefab, the Mastodon utilizes a **Custom Walk-In Armored Bridge Cabin** (7 W × 6 L × 3 H B):
1. **Station 1 (Port): Helm Pilot (`MAS Helm`):** Open 1×1×1 Control Station at [X −2, Z +10, Y = L4]. Controls chassis traction, 3-axle steering, outrigger deploy/retract sequences, hill-climbing crawl, power modes, and worksite lights.
2. **Station 2 (Center): Excavation Master (`MAS Station Boom`):** Open 1×1×1 Control Station at [X0, Z +10, Y = L4]. Controls the 35 m articulated boom via Custom Turret Controller (CTC) mouse aim, tool head camera, and automated macro sweep execution.
3. **Station 3 (Starboard): Tactical Fleet Director (`MAS Station Fleet`):** Open 1×1×1 Control Station at [X +2, Z +10, Y = L4]. Runs `RIG//OS-Lite` fleet coordination, dispatching Caterpillar land trains, calling Mule shuttles, receiving Kestrel scout coordinates, and monitoring ore quotas.
4. **C2 Compute Rack (Level L3):** Houses dual Programmable Blocks (`[MAS] PB Excavator` and `[MAS] PB RIG//OS-Lite`) along with AI Automaton blocks for uncrewed autonomous operation.
5. **Dorsal Comms Mast (Level L6):** Houses the 40 km Laser Antenna (`MAS Comm Laser`) and 50 km Radio Antenna (`MAS Comm Radio`).

```
Plan view, Y = L4 (Custom Walk-In Bridge Deck & Tactical Video Wall)
        X: −3          X: −2           X0            X: +2          X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────┐
           │        45° SLOPED HEAVY ARMOR GLASS CANOPY (PANORAMIC)      │
    Z: +11 │   [STATION 1]             [STATION 2]           [STATION 3] │
           │   HELM / PILOT            BOOM EXCAVATOR        FLEET DIR   │
           │  (Control Station)       (Control Station)    (Control Stn) │
    Z: +10 │        │                      │                     │       │
           │   ◄────┴──────── OPEN WALKWAY AISLE (Palette 9) ────┴────►  │
    Z: +9  ├───────────────────────────┬─────────────┬───────────────────┤
           │ [SURVIVAL KIT]            │ [LCD WALL 1]│ [LCD WALL 2]      │
    Z: +8  │ Field Respawn / O2-H2     │ Consist Sys │ Tactical Fleet    │
           ├───────────────────────────┴──────┬──────┴───────────────────┤
    Z: +7  │ [AIRLOCK SLIDING DOOR]           │ [LADDER UP TO ROOF CROWN]│
           └──────────────────────────────────┴──────────────────────────┘
```

```
3-Operator Multi-Page Manual Hotbar Architecture:

================================================================================
STATION 1: HELM PILOT (`MAS Helm`)
================================================================================
[Hotbar 1 · Tractive Powertrain & Bedrock Anchoring]
· Slot 1: Group `MAS Outriggers` -> Reverse (Deploy / Retract)
· Slot 2: Group `MAS Outrigger Pads` -> Switch Lock (Bedrock 100% Voxel Lock)
· Slot 3: Handbrake -> Toggle
· Slot 4: Group `MAS Wheels Drive` -> Propulsion ON/OFF
· Slot 5: Group `MAS Wheels All` -> Increase Strength (+5%)
· Slot 6: Group `MAS Wheels All` -> Decrease Strength (-5%)
· Slot 7: Event Controller `MAS EC Level` -> Trigger Auto-Leveling
· Slot 8: Group `MAS Sorter Tailings` -> Power ON/OFF (Gravel Dump)
· Slot 9: Group `MAS Ejectors Tailings` -> Throw Out ON/OFF

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
· Slot 2: Group `MAS Lasers Tool` -> Shoot ON/OFF (Twin Mining Lasers)
· Slot 3: Camera `MAS Cam Tool Head` -> View (Bore Sump Alignment)
· Slot 4: Rotor `MAS Turret Azimuth` -> Reverse (Manual Pan Left/Right)
· Slot 5: Hinge `MAS Shoulder Pitch` -> Reverse (Manual Boom Elevation)
· Slot 6: Hinge `MAS Elbow Pitch` -> Reverse (Manual Reach Extension)
· Slot 7: Hinge `MAS Wrist Pitch` -> Reverse (Manual Tool Pitch)
· Slot 8: Rotor `MAS Wrist Roll` -> Power ON/OFF (5.0 rpm Cutting Head Roll)
· Slot 9: Magnetic Plate `MAS Tool Grapple` -> Switch Lock (Crane Hoisting)

[Hotbar 2 · Automated Kinematic Macros & Presets]
· Slot 1: PB `[MAS] PB Excavator` -> Run `DEPLOY` (Release Clamp & Raise to Ready)
· Slot 2: PB `[MAS] PB Excavator` -> Run `STOW` (14-Block Dorsal Spine Clamp Lock)
· Slot 3: Magnetic Plate `MAS Clamp Boom Stow` -> Switch Lock (Manual Clamp Override)
· Slot 4: PB `[MAS] PB Excavator` -> Run `MACRO BENCH` (Automated Quarry Sweep)
· Slot 5: PB `[MAS] PB Excavator` -> Run `BORE` (Target-Centric Column Shaft)
· Slot 6: PB `[MAS] PB Excavator` -> Run `MACRO TRENCH` (Roadbed Carve)
· Slot 7: PB `[MAS] PB Excavator` -> Run `CRANE` (Vehicle Recovery Grapple)
· Slot 8: Group `MAS Boom Joints` -> Rotor Lock ON/OFF (E-Stop Brake)
· Slot 9: Sensor `MAS Sensor Boom` -> Power ON/OFF (Collision Stop Override)

[Hotbar 3 · Worksite Illumination & Ore Buffer Telemetry]
· Slot 1: Group `MAS Light Boom` -> Toggle ON/OFF (Tool Head Spotlights)
· Slot 2: Camera `MAS Cam Boom Turret` -> View (Wide-Angle Turret Vantage)
· Slot 3: Sorter `MAS Sorter Tailings` -> Drain All ON/OFF
· Slot 4: Cargo Container `MAS Tractor Buffer` -> Show on HUD ON/OFF
· Slot 5: Cargo Container `MAS Hold Ore Cat` -> Show on HUD ON/OFF
· Slot 6: Event Controller `MAS EC Ore Full` -> Trigger Alert
· Slot 7: IGC Send: `TMS.MULE.CALL;MAS-01;HOLD_FULL`
· Slot 8: IGC Send: `TMS.CAT.CALL;MAS-01;HOLD_FULL`
· Slot 9: Group `MAS Drills Tool` -> Shoot ON/OFF (Backup Mechanical Drills)

================================================================================
STATION 3: TACTICAL FLEET DIRECTOR (`MAS Station Fleet`)
================================================================================
[Hotbar 1 · Logistics Dispatch & Ore Offload Automation]
· Slot 1: IGC Send: `TMS.MULE.CALL;MAS-01;RARE_ORE` (Call Air Cargo Ferry)
· Slot 2: IGC Send: `TMS.CAT.CALL;MAS-01;BULK_ORE` (Call Caterpillar Land Train)
· Slot 3: Connector `MAS MP Conn` -> Lock/Unlock (Mule Roof Pad Connector)
· Slot 4: Group `MAS MP Clamps` -> Switch Lock (Mule Cradle Rub Rails)
· Slot 5: Sorter `MAS MP Sorter` -> Drain All ON/OFF (Rare Ore Fast Drain)
· Slot 6: Connector `MAS Stern Dump Conn` -> Lock/Unlock (Caterpillar Stern Rail)
· Slot 7: Sorter `MAS Stern Sorter` -> Drain All ON/OFF (Bulk Hopper Dump)
· Slot 8: Group `MAS Welder Pad Kestrel` -> Power ON/OFF (Drone Repair Welders)
· Slot 9: Event Controller `MAS EC Handover` -> Trigger C2 Base Handover

[Hotbar 2 · Slave Links & Subordinate Machine Teleoperation]
· Slot 1: Remote Control `MAS-02 Remote` -> Control (Subordinate Worker Excavator)
· Slot 2: Remote Control `CAT-01 Remote` -> Control (Caterpillar Locomotive)
· Slot 3: Remote Control `KES-01 Remote` -> Control (Kestrel Scout Drone)
· Slot 4: Remote Control `ALB-01 Remote` -> Control (Albatross Repair Drone)
· Slot 5: Ore Detector `MAS Ore Detector` -> Range +10 m
· Slot 6: Ore Detector `MAS Ore Detector` -> Range -10 m
· Slot 7: PB `[MAS] PB Excavator` -> Run `SEISMIC` (Trigger Voxel Prospecting Pulse)
· Slot 8: LCD Panel `MAS LCD Video Wall 1` -> Cycle Page
· Slot 9: LCD Panel `MAS LCD Video Wall 2` -> Cycle Page

[Hotbar 3 · Perimeter CIWS Defense & Alert Status]
· Slot 1: Large Gatling Turret `MAS Turret Bow` -> Control (Direct CIWS Aim)
· Slot 2: Large Gatling Turret `MAS Turret Stern` -> Control (Stern CIWS Aim)
· Slot 3: Camera `MAS Cam Bow CIWS` -> View
· Slot 4: Camera `MAS Cam Stern CIWS` -> View
· Slot 5: Group `MAS Turrets Flank` -> Power ON/OFF (Anti-Personnel Skirt Guns)
· Slot 6: Group `MAS Turrets All` -> Target Meteors ON/OFF
· Slot 7: Group `MAS Turrets All` -> Target Hostiles ON/OFF
· Slot 8: PB `[MAS] PB RIG//OS-Lite` -> Run `ALERT CBT` (Lockdown / Combat Mode)
· Slot 9: PB `[MAS] PB RIG//OS-Lite` -> Run `ALERT NORM` (Clear Combat Alert)
```

---

#### Step 10.10D · Bridge Anti-Capture Scuttle System & Countdown Loop (L3-L4)

To prevent advanced technology, refining assets, and encryption keys from falling into enemy hands if the vehicle is overrun:
* **Bridge Scuttle Charge (`MAS Warhead Scuttle Bridge`):** Mount 1× Large Grid Warhead beneath the C2 compute rack at [X0, Z +8, Y = L3]. Disarmed by default.
* **Scuttle Control Panel (`MAS BP Scuttle Bridge`):** Mount 1× Sci-Fi 4-Button Panel on the bridge aft bulkhead at [X +1, Z +8, Y = L4] (Button 1: Arm Scuttle System, Button 2: Start 60 s Countdown, Button 3: Broadcast Abandon Ship SOS, Button 4: Emergency Scuttle Abort).
* **Native Timer Countdown Loop (`MAS Timer Scuttle`):** Set countdown to 60 seconds.
  * T − 60 s: Triggers bridge siren, switches interior lights to flashing red, and broadcasts `MASTODON-01 SCUTTLE INITIATED`.
  * T − 5 s: Arms both `MAS Warhead Scuttle Bridge` and `MAS Warhead Scuttle Power`.
  * T − 0 s: Simultaneous detonation, destroying the avionics, reactors, and C2 core.
  * Pressing Button 4 before T − 5 s stops the timer and returns warheads to safety disarm.

---

#### Step 10.10E · Physical In-Situ Interfaces (Sci-Fi 4-Button Panels)

To support foot operations without requiring cockpit entry, 6 wall-mounted Sci-Fi 4-Button Panels (1×1×1 B) provide physical local control:
* **`MAS BP Ground Boarding` (Port Skirt [X −4, Z +7, L1]):** Button 1: Ladder Lower/Raise · Button 2: Keel Lights · Button 3: Outer Airlock Door · Button 4: Beacon Toggle.
* **`MAS BP Airlock Vestibule` (Airlock [X −2, Z +7, L3]):** Button 1: Outer Door Cycle · Button 2: Inner Door Cycle · Button 3: Cabin Pressurize/Depress · Button 4: EVA Locker Access.
* **`MAS BP Power Vault` (Jefferies [X −2, Z −1, L3]):** Button 1: Vault Door Toggle · Button 2: Reactor SCRAM · Button 3: H2 Engine Cold-Start · Button 4: Battery Shore Charge.
* **`MAS BP Field Workshop` (Workshop [X0, Z −14, L3]):** Button 1: Assembler Repeat (Plates) · Button 2: Refinery Ore Flush · Button 3: O2/H2 Gen Toggle · Button 4: Workshop Lights.
* **`MAS BP Flight Deck` (Roof Hatch [X +2, Z −10, L6]):** Button 1: Mule Clamps Toggle · Button 2: Mule Connector Toggle · Button 3: Kestrel Welders Pulse · Button 4: Deck Floodlights.
* **`MAS BP Stern Dump` (Rear Platform [X +2, Z −25, L2]):** Button 1: Stern Connector Toggle · Button 2: Stern Bulk Sorter Drain All · Button 3: Cat Train Ping · Button 4: Rear Worksite Light.

---

#### Step 10.10B · Retractable Crew Boarding Ladder & Proximity Walkway (Port Flank L2)

To allow the player to enter and exit the 3.5 m high bridge cabin without consuming limited jetpack hydrogen fuel:
* **Hinge Mounting:** Install 1× Motor Hinge (`MAS Crew Ladder Hinge`) flush on the Port hull skirt at [X −4, Z +7, Y = L2] (between Axles 1 and 2).
* **Ladder Run:** Attach a 2-block narrow ladder run to the hinge head, painted Palette Slot 9 (High-Viz Safety Yellow).
* **Proximity Automation:** Sensor `MAS Sensor Ladder` set to 2.5 m radius at the Port flank. When a crew member on foot approaches, the hinge rotates downward 90° to touch ground level (L0).
* **Drive Interlock:** Event Controller `MAS EC Ladder` monitors the handbrake and wheel propulsion. When the handbrake is released or speed exceeds 0 km/h, the ladder automatically retracts flush into the armored hull recess.

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

---

#### Step 10.10C · Bow CIWS Gatling Turret & Flank Anti-Personnel Sockets (Tractor Crown L5)

To protect the locomotive consist against hostile MES pirate drones and ground boarding raiders while anchored or mining:
* **Bow Gatling Turret (`MAS Turret Bow`):** Mount 1× Large Grid Gatling Turret (3×3×3 B footprint) on the centerline dorsal crown of Carriage 1 at [X0, Z +7, Y = L5], directly above the C2 compute rack rear bulkhead. Connect its bottom port directly to the tractor conveyor spine at L4. Provides full 360° hemispherical CIWS protection against aerial drones and ground gunships.
* **Flank Anti-Personnel Turrets (`MAS Turret Flank Port/Stbd`):** Mount 2× Interior Turrets (1×1×1 B) flush on the lower armored skirts at [X −4, Z +2, Y = L2] (Port) and [X +4, Z +2, Y = L2] (Starboard). Plumbed to the lower conveyor spine to deliver automated anti-infantry and anti-wildlife perimeter fire covering the boarding ladder and ground radius.
* **Combat Mode Integration:** Event Controller `MAS EC Combat` monitors chassis damage and enemy target acquisition. When engaged, `RIG//OS-Lite` elevates alert status to `RIG.OS.MODE CBT`, powers up all turrets, seals crew airlocks, and feeds ammunition from the assembler hold.

##### Pre-Enclosure Inspection Gate 10.4A: Bridge Airtightness & C2 Network Link Check
* [ ] Confirm bridge cabin pressurizes to 100% O2 via air vent.
* [ ] Verify `MAS Ore Detector` displays subterranean ore deposits out to 150 m range.
* [ ] Confirm CTC block maps mouse yaw to `MAS Turret Azimuth` and mouse pitch to `MAS Shoulder Pitch`.
* [ ] Confirm `MAS Comm Laser` and `MAS Comm Radio` establish two-way IGC telemetry ping.
* [ ] Verify `MAS Crew Ladder Hinge` automatically lowers when approached on foot and retracts upon handbrake release.
* [ ] Verify `MAS Turret Bow` and `MAS Turret Flank Port/Stbd` receive NATO 25×184mm ammunition through conveyor spine.

---

### Phase 10-5 · Modular FOB & Utility Carriage Chassis & Workshop (Carriage 2)

```
       [PHASE 10-5 SPATIAL LOCATOR HUD]
       Envelope: 11 W × 18 L × 5 H B (27.5 × 45.0 × 12.5 m)
       Coordinate Range: X −5 to +5, Z −26 to −8, Y = L1 to L5
       Carriage Datums:
       · Front Drawbar Tongue: [X0, Z −8, Y = L2]
       · Field Workshop Core: Z −18 to −14, X −3 to +3
       · Component Hold: Z −22 to −19, X0
       · Axle 4 (Mid Drive): Z −13 (Locked: 0.0°)
       · Axle 5 (Aft Steer): Z −22 (Counter-Steer: −25.0°)
```

#### Pre-Step Parts Tray: Phase 10-5
* 200× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 4× Wheel Suspension 5×5 (2 Port, 2 Starboard)
* 3× Large Battery (3 MWh each, 9.0 MWh total, Palette 6: High-Voltage Red)
* 1× Basic Refinery (1×1×2 B, 330 kW, Palette 4: Processing Teal)
* 1× Large Assembler (Palette 4: Processing Teal)
* 1× Medical Bay + 2× Cryo Chamber (Palette 7: Fleet Command Navy)
* 2× Large Oxygen Tank (200,000 L O2, Palette 4: Processing Teal)
* 1× Small Cargo Container (`MAS Hold Ice LifeSupport`, 15,000 L Quarantined Ice, Palette 4)
* 2× Large Hydrogen Tank (Palette 6)
* 2× O2/H2 Generator (Palette 4)
* 4× Air Vent (`MAS Vent Workshop`, `MAS Vent Crawlway`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern`)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 1× Large Projector (`MAS Projector Master`, Palette 7)
* 2× Sci-Fi 4-Button Panel (`MAS BP Flight Deck`, `MAS BP Stern Dump`)
* 14× Grated Catwalk Block, 4× Ladder Block, 2× Sliding Door (Palette 9)
* 16× Large Conveyor Pipe & 6× Conveyor Junction (Palette 4)

---

#### Step 10.11 · Utility Carriage Box Girders & 4-Wheel Powertrain (L1-L2)

Construct the trailing chassis frame connected to the tractor hitch tongue at Z = −8. Mount 2 pairs of 5×5 large suspension wheels:
* **Axle 4 (Z −13):** Fixed drive axle (0.0° steer).
* **Axle 5 (Z −22):** Counter-steering axle (−25.0° inverted) to eliminate trailer corner-cutting during tight turns.

```
Plan view, Y = L1 (Utility Carriage 4-Wheel Layout)
        X: −5          0          +5
    Z: −13 [WH 7 (P)] ═══════════ [WH 8 (S)]   ◄── Axle 4 (0.0° Locked Drive)
    Z: −22 [WH 9 (P)] ═══════════ [WH 10 (S)]  ◄── Axle 5 (−25.0° Inverted Counter-Steer)
```

| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 4** | Z = −13 | `WH 7, 8`  | `0.0°`  | OFF (Locked) | `100%` | `50% / 90%` | `75%` |
| **Axle 5** | Z = −22 | `WH 9, 10` | `25.0°` | INVERTED     | `100%` | `55% / 90%` | `80%` |

---

#### Step 10.11B · Sub-Floor Utility Battery Vault (Z −17 to −15, L2)

Install 3 Large Batteries (`MAS Battery 4, 5, 6`) in the Level L2 sub-floor cavity beneath the Field Workshop:
* **Storage Capacity:** Adds 9.0 MWh energy storage buffer (36.0 MW peak discharge) to Carriage 2, bringing total consist energy reserve to 18.0 MWh (72.0 MW peak discharge).
* **Electrical Integration:** Plumbed directly into the Utility carriage tractive bus, supplying float-charge buffer for the Mule Gantry Pad, Basic Refinery, Large Assembler, and rear steering axle.

---

#### Step 10.12 · Mobile FOB Field Workshop, Medical Suite & Universal Life Support (L2-L4)

Outfit Carriage 2 as a self-sustaining expeditionary base centered along the Level L3 central aisle:
1. **Field Workshop Assembler (`MAS Utility Assembler`):** Large Assembler at [X −2, Z −14] configured to fabricate replacement plates, drill components, and motors directly in the field. Dual physical inventory panels face the central aisle for manual blueprint access.
2. **Medical & Crew Habitat Bay (`MAS Medical Bay`, `MAS Cryo Pod 1, 2`):** Pressurized crew habitat at [X +2, Z −12 to −14] with Medical Room, Survival Kit, and 2× Cryo Chambers plumbed directly to the life support oxygen line for long-distance transit hibernation.
3. **200,000 L Strategic Oxygen Tank Bank (`MAS Tank O2 1, 2`):** Mount 2× Large Grid Oxygen Tanks at [X −2, Z −16, Y = L3-L4]. Stores 200,000 L of pressurized breathing oxygen, enabling over 120 hours of vacuum surface operations.
4. **Quarantined Emergency Ice Reserve (`MAS Hold Ice LifeSupport`):** 1× Small Cargo Container (15,000 L capacity) at [X0, Z −16, L3] isolated by a one-way filter. Guarantees emergency ice reserves for life support even if bulk cargo holds are completely dumped.
5. **Atmospheric Tank Farm:** 2× Large Hydrogen Tanks (refuels Kestrel scouts and Mule ferries) + 2× O2/H2 Generators at [X ±2, Z −18]. Manual shutoff valves exposed in the corridor wall.
6. **Field Component Storage (`MAS Hold Components`):** 1× Large Cargo Container (3×3×3 B) dedicated to stocking construction parts at Z −22. Features a physical terminal port facing the aisle at [X0, Z −21, L3].

```
Plan view, Y = L3 (Utility Carriage Interior Deck with Closed-Loop Metallurgy & Life Support)
        X: −3          0          +3
    Z: −10 ═══════[DRAWBAR TONGUE]═══════  ◄── Articulated Hitch & Merge Block
           │                             │
    Z: −12 │ [REFINERY]  │ [CRYO POD 1]  │  ◄── Basic Refinery & 2-Crew Cryo Hibernation
           ├─────────────┤ [CRYO POD 2]  │
    Z: −14 │ [ASSEMBLER] │ [MEDICAL BAY] │  ◄── Large Assembler & Medical Trauma Center
           ├─────────────┼───────────────┤
    Z: −16 │ [O2 TANK 1] │ [O2 TANK 2]   │  ◄── 200,000 L Oxygen Bank & Quarantined Ice
           ├─────────────┼───────────────┤
    Z: −18 │ [H2 TANK 1] │ [H2 TANK 2]   │  ◄── Fuel Bunkering Farm (10,000,000 L H2)
           ├─────────────┴───────────────┤
    Z: −22 │ [MAS HOLD COMPONENTS (3×3×3)│  ◄── Spare Parts Storage (Palette 5)
    Z: −26 └─────────────────────────────┘
```

---

#### Step 10.12B · Closed-Loop In-Situ Field Metallurgy (Basic Refinery Integration)

In Space Engineers, an Assembler cannot convert raw ore directly into components; it requires refined ingots. Install 1 Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW, 13.5 t) adjacent to `MAS Utility Assembler` at [X −2, Y = L3, Z −12]:
* **Conveyor Routing:** Directly plumbed to the central conveyor spine. Ingests raw ore (Fe, Ni, Co, Si, Mg) delivered from `MAS Tractor Buffer` via the articulated drawbar pipe.
* **Closed Loop:** Outputs refined ingots directly into `MAS Utility Assembler` and `MAS Hold Components`, enabling complete autonomous field fabrication of replacement parts (steel plates, construction components, motors, bulletproof glass) without returning to base.
* **Manual Hopper Port:** Top maintenance door accessible from the central aisle allows manual dumping of rare ore batches.
* **Ice Automation:** Event Controller `MAS EC Ice` monitors raw ice levels, automatically prioritizing ice routing to the twin O2/H2 generators to replenish the 10,000,000 L hydrogen reserve.

---

#### Step 10.12C · Vertical Flight Ladderwell & Stern Maintenance Descent (L3 to L6 & L3 to L2)

To allow physical passage from the workshop to the roof flight decks and stern train dump bumper:
* **Flight Deck Airlock & Ladderwell (L3 → L6):** Install an enclosed vertical ladder at [X +2, Z −10] climbing from the forward workshop corner up through an armored hatch to the Level L6 Flight Deck. Equipped with `MAS Vent Airlock Roof` and button panel `MAS BP Flight Deck` for active vacuum gas reclamation.
* **Stern Dump Airlock & Descent (L3 → L2):** Install a descending ladderwell at [X +2, Z −25] passing through an armored hatch down to Level L2. Allows an engineer on foot to physically inspect and service `MAS Stern Dump Conn`, `MAS Stern Sorter`, and rear guide wedges without venting vehicle atmosphere.

---

#### Step 10.12D · Ambient Atmospheric Oxygen Scavenging Vents (Carriage 2 Flank L4)

To achieve indefinite planetary life support without consuming raw ice or running energy-intensive O2/H2 generators:
* **Hardware Integration:** Mount 2× Air Vents (`MAS Vent Atmo Intake Port` at [X −3, Z −16, Y = L4] and `MAS Vent Atmo Intake Stbd` at [X +3, Z −16, Y = L4]) recessed flush into the upper exterior flank armor.
* **Operating Configuration:** Set `Depressurize = ON`. In breathable planetary atmospheres, the vents pull ambient oxygen directly from outside air into the central life support trunk.
* **Closed-Loop Storage:** Fills internal oxygen tanks and pressurized crew compartments automatically at zero ice consumption and minimal parasitic power (30 kW per vent), preserving onboard ice stocks strictly for hydrogen fuel synthesis.

---

#### Step 10.12E · Genesis Master Projector & Multi-Zone Environmental Control (L3-L4)

To ensure field self-repair and complete multi-planetary survival:
* **Master Projector Self-Repair Core (`MAS Projector Master`):** Mount 1× Large Grid Projector inside the workshop ceiling at [X0, Z −14, Y = L4]. Pre-loaded with the complete 1,039-block blueprint of `MASTODON-01 MKI`, perfectly aligned to the physical chassis grid. When damaged, the projected blueprint allows repair drones (`ALBATROSS-01`) or crew to rebuild damaged blocks using parts from `MAS Hold Components`.
* **Universal Multi-Atmosphere Environmental Control:** Event Controller `MAS EC Atmosphere Mode` manages atmospheric adaptation:
  * On breathable worlds (Earthlike): Exterior vents set to `Depressurize = ON` to harvest free oxygen.
  * On airless moons (Moon, Europa) or toxic worlds (Mars, Alien): Exterior vents set to `Power = OFF`. Internal life support switches to closed-loop mode, feeding 100% O2 from `MAS Tank O2 1, 2` into all 4 sealed zones while airlocks cycle with zero-gas venting.

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

---

### Phase 10-6 · Flight Decks & Logistics Offload Interfaces (Levels L5-L6)

```
       [PHASE 10-6 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 18 L × 2 H B (17.5 × 45.0 × 5.0 m)
       Coordinate Range: X −3 to +3, Z −26 to −8, Y = L5 to L6
       Deck Datums:
       · Mule Gantry Cradle Pad: [X0, Z −22, Y = L6] (7×7 B Deck)
       · Kestrel Scout Pad Port: [X −3, Z −12, Y = L5]
       · Kestrel Scout Pad Starboard: [X +3, Z −12, Y = L5]
       · Stern Dump Connector: [X0, Z −26, Y = L2] (Centerline height = 2.5 m)
```

#### Pre-Step Parts Tray: Phase 10-6
* 49× Heavy Armor Full Block (Palette 1: Hazard Amber · Mule Pad Deck)
* 4× Solar Panel (Palette 0: Industrial Dark Gray · Flush Roof Array)
* 18× Catwalk with Railing Block (Palette 9: High-Viz Safety Yellow)
* 2× Motor Hinge + 2× Large Magnetic Plate (Palette 1 · Cradle Clamp Arms)
* 1× Large Connector (`MAS MP Conn`, Palette 4)
* 2× Small-Grid Connector (`MAS Kestrel Dock 1, 2`, Palette 8: Scout White)
* 2× Large Grid Ship Welder (`MAS Welder Pad Kestrel 1, 2`, Palette 4: Processing Teal)
* 4× Spotlight / Floodlight (`MAS Floodlight Tower 1..4`, Palette 7: Fleet Command Navy)
* 1× Large Connector (`MAS Stern Dump Conn`, Palette 2: Rail Crimson)
* 2× Large Conveyor Sorter (`MAS MP Sorter`, `MAS Stern Sorter`, Palette 4)
* 12× Heavy Armor Slopes & Alignment Bumpers (Palette 2)
* 1× Large Gatling Turret (`MAS Turret Stern`, Palette 7: Fleet Command Navy)

---

#### Step 10.13 · Roof Mule Receiving Gantry Cradle Pad (Z −25 to −19, L6)

Install the standardized 7 × 7 B Heavy Armor blast deck on the utility carriage roof. Equipped with twin motorized hinge arms and magnetic plates matching Decision 45:
* Inbound MULE-01 touches down; cradle arms swing 30° inward, clamping the Mule's chassis rub rails.
* Center belly connector (`MAS MP Conn`) locks. Sorter `MAS MP Sorter` (Drain All = ON) drains rare ores (U, Pt, Au, Mg) into the Mule's hold for immediate air transport to base.

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

---

#### Step 10.13B · Flush Solar Array for Autonomous Watchkeeping (Carriage 2 Roof L6)

Mount 4 Large Grid Solar Panels (`MAS Solar 1..4`, 1×4 B) flush with the Heavy Armor roof deck at Level L6, flanking the Mule Gantry Cradle Pad:
* **Positioning:** 2 Port panels at X = −4 (Z −24 to −20) and 2 Starboard panels at X = +4 (Z −24 to −20), keeping a 1-block clear buffer from the 7×7 B Mule landing footprint to avoid thruster plume erosion.
* **Output & Role:** Delivers `≈ 640 kW` peak daylight power† (160 kW per panel† under optimal solar alignment). Provides continuous zero-fuel watchkeeping power, maintaining life support, event controllers, communications, and float-charging the 18.0 MWh battery reserve without consuming nuclear fuel or hydrogen.

```
Plan view, Y = L6 (Mule Blast Deck Flanked by Flush Solar Array)
        X: −4     X: −3           0          +3     X: +4
    Z: −19 ┌────┐ ■════════════╤═══════════■ ┌────┐
           │SOL1│ ║  Hazard    │  Hazard   ║ │SOL3│  ◄── Flush Solar Array (≈ 640 kW Peak†)
    Z: −22 │    │ [CLAMP P] [MAS MP CONN] [CLAMP S]  │    │  ◄── Mule Clamping Rails
           │SOL2│ ║  Chevrons  │  Chevrons ║ │SOL4│
    Z: −25 └────┘ ■════════════╧═══════════■ └────┘
```

---

#### Step 10.13C · Level L6 Flight Deck Maintenance Catwalk Network

To ensure safe human traversal and field maintenance across the upper flight surfaces:
* **Perimeter Safety Catwalk:** Surround the 7×7 B Mule Gantry Pad with industrial grated catwalks fitted with yellow safety handrails (Palette Slot 9). Provides an unhindered observation walkway for deck marshals directing landing MULE-01 shuttles.
* **Scout Service Bridges:** Extend forward catwalk branches to twin Kestrel landing pads (Port/Starboard), allowing flight technicians to physically inspect scout drone hulls, retrieve manual geological core samples, and verify connector seals.
* **Solar Inspection Gangways:** Run narrow service catwalks along Port (X −4) and Starboard (X +4) flanks, giving crew members direct walk-up access to inspect, wash dust from, or repair `MAS Solar 1..4` without treading upon the Mule heavy blast deck.

---

#### Step 10.14 · Twin Kestrel Prospector Scout Drone Landing Pads (Z −13 to −11, L5)

Recess two small landing pads forward of the Mule pad on the utility roof:
* `MAS Kestrel Dock 1` (Port, X −3) & `MAS Kestrel Dock 2` (Starboard, X +3).
* Small-grid connectors flush with deck for recharging and downloading geological vein scans.
* Painted Palette Slot 8 (Scout Flight White) with perimeter inset yellow lighting.

---

#### Step 10.14B · Flight Deck Recessed Drone Maintenance Welders (Carriage 2 L5)

To maintain drone fleet operational readiness during extended deep-wilderness prospecting campaigns:
* **Recessed Welder Arrays:** Mount 2× Large Grid Ship Welders (`MAS Welder Pad Kestrel 1` at [X −3, Z −12, Y = L4] and `MAS Welder Pad Kestrel 2` at [X +3, Z −12, Y = L4]) positioned directly flush beneath the small-grid docking collars of the twin Kestrel landing pads.
* **Plumbed Logistics:** Connected directly to the utility workshop conveyor trunk, drawing steel plates, construction components, small tubes, and computer chips from `MAS Hold Components` and `MAS Utility Assembler`.
* **Automated Repair Cycle:** Event Controller `MAS EC Drone Repair` detects when a Kestrel dock reports `Connected = TRUE`. It pulses the respective pad welder for 5.0 seconds, automatically repairing atmospheric thruster burnout, micro-meteoroid hull pits, and antenna damage sustained during prospecting sorties before returning to standby.

---

#### Step 10.14C · Worksite 360° Floodlight Matrix (Tractor Bridge Crown & Dorsal Comms Mast L5-L6)

To illuminate hazardous night operations, quarry excavation faces, and fleet flight decks:
* **Tower Floodlight Array:** Mount 4× Spotlights (`MAS Floodlight Tower 1..4`, Palette 7: Fleet Command Navy) in a high-elevation 360° cardinal ring on the Tractor dorsal bridge crown and comms mast at Level L5-L6 facing Bow (+Z), Stern (−Z), Port (−X), and Starboard (+X).
* **Illumination Coverage:** Throws high-intensity illumination across a 100 m radius, blanketing the excavation pit, the Mule receiving cradle, and both Kestrel pads.
* **Integrated Control:** Bound to group `MAS Lights` alongside the boom spotlights (`MAS Light Boom 1, 2`) and underbelly maintenance lamps (`MAS Lights Underbelly`), allowing single-hotkey toggling from Station 1 Helm.

---

#### Step 10.15 · Stern Bumper & Caterpillar Land Train High-Speed Dump Port (Z = −26, L2)

Install the rear collision push-bumper and large connector (`MAS Stern Dump Conn`) at Level L2:
* Mates directly with the bow connector of the CATERPILLAR-01 tractor locomotive.
* Flanked by dual Heavy Armor angled guide wedges (Palette 2: Heavy Rail Crimson) to funnel the incoming locomotive into positive alignment.
* Sorter `MAS Stern Sorter` drains bulk ores (Fe, Ni, Si, Stone) at ultra-high transfer rates (< 90 seconds for 2,500 t bulk payload).

```
Rear Elevation Cross-Section, Y = L2 (Stern Caterpillar Dump Interface)
    Port [-X]                                                          Starboard [+X]
    X: -3            X: -1            X0              X: +1            X: +3
    [GUIDE WEDGE] ── [PUSH BLOCK] ─ [STERN CONN] ─ [PUSH BLOCK] ── [GUIDE WEDGE]
    │ Heavy Slope    │ Solid Block  │ MAS Stern     │ Solid Block  │ Heavy Slope
    │ Crimson        │ Crimson      │ Dump Conn     │ Crimson      │ Crimson
    └────────────────┴──────────────┴───────────────┴──────────────┴─────────────┘
```

---

#### Step 10.15B · Stern Gantry CIWS Gatling Turret (Utility Stern L5)

Mount the rear defense turret on the upper gantry overlooking the stern dump interface:
* **Stern Gatling Turret (`MAS Turret Stern`):** 1× Large Grid Gatling Turret mounted at [X0, Z −26, Y = L5], elevated 7.5 m above the Caterpillar dump bumper. Connect bottom conveyor port to the stern conveyor junction at L4.
* **Coverage:** Delivers 270° hemispherical defensive coverage over the rear quadrant, providing covering fire for incoming MULE-01 shuttles on approach to the gantry pad, protecting docked Caterpillar land trains during high-speed dump operations, and repelling pursuers from the rear.

##### Pre-Enclosure Inspection Gate 10.6A: Cradle Clamping Dynamics & Dual-Tier Sorter Routing
* [ ] Verify `MAS MP Sorter` whitelist passes only rare ores (U, Pt, Au, Mg) to the Mule shuttle.
* [ ] Verify `MAS Stern Sorter` whitelist passes bulk ores (Fe, Ni, Si, Stone) to the land train.
* [ ] Test cradle clamp hinges: confirm 30° inward swing firmly holds docking craft without grid damage.
* [ ] Confirm `MAS Solar 1..4` deliver daylight power and maintain battery float charge while nuclear reactors are toggled OFF.
* [ ] Verify Level L6 catwalk handrails and safety walkways allow complete perimeter access around Mule and Kestrel decks.
* [ ] Verify `MAS Turret Stern` receives NATO 25×184mm ammunition from utility cargo hold.
* [ ] Verify recessed drone welders (`MAS Welder Pad Kestrel 1, 2`) auto-repair docked Kestrel drones upon touchdown.
* [ ] Confirm worksite floodlight matrix (`MAS Floodlight Tower 1..4`) casts 360° illumination across mining zone and flight decks.

```
PHASE 10-6 VISUAL MILESTONE · COMPLETE EXPEDITIONARY MINING TRAIN FOB
                        [KESTREL PADS]         [MULE GANTRY PAD]
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
* `MAS Outriggers`: 4× Hydraulic stabilizer pistons + magnetic plates / seismic feet.
* `MAS Boom Joints`: 5× Rotors and Hinges articulating the long-reach arm.
* `MAS Lasers`: 2× Simple Laser Multitools on the tool head.
* `MAS Welders`: 2× Flush Large Grid Ship Welders (`MAS Welder Pad Kestrel 1, 2`) on Kestrel flight pads.
* `MAS Lights`: Worksite floodlights (`MAS Floodlight Tower 1..4`), boom spotlights (`MAS Light Boom 1, 2`), and underbelly lamps (`MAS Lights Underbelly`).
* `MAS Seismic`: 4× Seismic Surveying Landing Gear units (`MAS Seismic Outrigger 1..4`).
* `MAS Batteries`: 6× Large Batteries across Tractor Vault and Utility Sub-Floor (18.0 MWh total).
* `MAS Doors`: All internal airlock, vault, and inter-carriage gangway sliding doors.
* `MAS Turrets`: 2× Large Gatling Turrets (`MAS Turret Bow`, `MAS Turret Stern`) + 2× Interior Turrets (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`).
* `MAS MP Cradle`: 2× Motorized Hinges + 2× Magnetic Plates on the Mule pad.
* `MAS Displays`: All cockpit telemetry screens and tactical video wall LCDs.

#### 3. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Programmable Block | `[MAS] PB Excavator` | Kinematics & machine control · Update1 · Zero-GC |
| Programmable Block | `[MAS] PB RIG//OS-Lite` | Regional Fleet C2 Mothership engine · Update10 · IGC router |
| Control Station | `MAS Helm` | Station 1 (Port) · Pilot traction, steering, outrigger, power control |
| Control Station | `MAS Station Boom` | Station 2 (Center) · Excavation Master · CTC mouse aim & macros |
| Control Station | `MAS Station Fleet` | Station 3 (Stbd) · Tactical Fleet Director · Drone dispatch & C2 |
| Laser Antenna | `MAS Comm Laser` | 40 km point-to-point line-of-sight data link (L6 mast) |
| Radio Antenna | `MAS Comm Radio` | 50 km omnidirectional fleet broadcast & heartbeat beacon |
| Radio Antenna | `MAS Beacon Distress` | 50 km blackout SOS distress beacon in sub-bridge keel |
| Battery (Small) | `MAS Battery BlackBox` | Isolated cold-standby power cell for distress beacon |
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
| Seismic Landing Gear | `MAS Seismic Outrigger 1..4` | Deep voxel acoustic prospecting feet (TMS Mod 2819263781) |
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
| Laser Multitool | `MAS Laser 1, 2` | Mining beam heads on tool head · Continuous extraction |
| Sensor | `MAS Sensor Boom` | 4.0 m forward proximity collision safety buffer |
| Connector (Large) | `MAS MP Conn` | Roof flush connector for MULE-01 shuttle docking |
| Connector (Large) | `MAS Stern Dump Conn` | Stern bumper connector for CATERPILLAR-01 bulk train docking |
| Conveyor Sorter | `MAS MP Sorter` | High-priority rare ore drain sorter (U, Pt, Au, Mg) |
| Conveyor Sorter | `MAS Stern Sorter` | High-volume bulk ore drain sorter (Fe, Ni, Si, Stone) |
| Conveyor Sorter | `MAS Sorter Tailings` | Underbelly stone & gravel purge filter (selective venting) |
| Conveyor Ejector | `MAS Ejector Tailings 1, 2` | Angled underbelly nozzles laying aggregate roadbed |
| Basic Refinery | `MAS Field Refinery` | Closed-loop field metallurgy · 330 kW · Ingests raw ore, outputs ingots |
| Hydrogen Engine | `MAS Engine H2` | Standby cold-start emergency generator · 5.0 MW failover |
| Solar Panel | `MAS Solar 1..4` | Flush roof solar array · 640 kW peak† · Zero-fuel watchkeeping |
| Battery (Large) | `MAS Battery 1..3` | Tractor Power Core Battery Bank · 9.0 MWh · 36 MW discharge |
| Battery (Large) | `MAS Battery 4..6` | Utility Sub-Floor Battery Bank · 9.0 MWh · 36 MW discharge |
| Sliding Door | `MAS Door Airlock` | Port L2/L3 entry airlock door |
| Sliding Door | `MAS Door Vault` | Power Core Vault maintenance entrance from Jefferies tube |
| Sliding Door | `MAS Door Gangway Fwd` | Tractor Aft Bulkhead gangway isolation door |
| Sliding Door | `MAS Door Gangway Aft` | Utility Forward Bulkhead gangway isolation door |
| Event Controller | `MAS EC Stabilizers` | Blocks boom arm deployment until outriggers report locked |
| Event Controller | `MAS EC Level` | Auto-leveling bedrock jack sequence on sloped terrain |
| Event Controller | `MAS EC Ladder` | Auto-deploys crew ladder on approach; retracts on drive |
| Event Controller | `MAS EC Tailings` | Pulses stone ejectors when tractor ore buffer > 50% |
| Event Controller | `MAS EC Hitch Speed` | Locks hitch yaw (100 MN·m torque) at speeds > 35 km/h |
| Event Controller | `MAS EC Hitch Rev` | Clamps hitch yaw to ±15° and inverts Axle 5 when reversing |
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
| `MAS EC Stabilizers`| `MAS Outrigger Pad 1..4` | All 4 Pads = Locked | Enable `MAS Boom Joints` · Sound Ready Chime | Lock `MAS Boom Joints` · Fold Boom |
| `MAS EC Level` | `MAS Helm` Roll & Pitch | Roll / Pitch $> 1.0^\circ$ | Differential Piston Extension | Lock Mag Plates (0.0° Level) |
| `MAS EC Ladder` | `MAS Sensor Ladder` | Player within 2.5 m | Deploy Ladder (Hinge 90° Down) | Retract Ladder (Hinge 0° Flush) |
| `MAS EC Tailings` | Tractor Ore Buffer | Ore Fill $\ge 50\%$ | `MAS Sorter Tailings` Drain ON · Ejectors ON | Sorter & Ejectors OFF |
| `MAS EC Hitch Speed`| Consist Speedometer | Speed $> 35\text{ km/h}$ | Hitch Torque = 100 MN·m (Anti-Sway) | Normal Hitch Torque (45 MN·m) |
| `MAS EC Hitch Rev` | Transmission Gear | Gear = Reverse | Clamp Yaw ±15° · Axle 5 Positive Tracking | Normal Yaw ±45° · Counter-Steer |
| `MAS EC Holds` | Combined Ore Mass | Ore Mass $\ge 2,500\text{ t}$ | Sound Payload Horn · IGC `TMS.MULE.CALL` | Normal Extraction Mode |
| `MAS EC MP` | `MAS MP Conn` | Status = Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle Retract (0°) |
| `MAS EC Train` | `MAS Stern Dump Conn` | Status = Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Handover` | `GND-01` Heartbeat | Signal Detected | Run PB `RIG//OS-Lite` (Argument: `HANDOVER`) | Maintain Mothership Authority |
| `MAS EC Power` | `MAS Reactor 1, 2` Uranium | Uranium Mass $< 1.0\text{ kg}$ | `MAS Engine H2` Turn ON · Sound Fuel Horn | `MAS Engine H2` Turn OFF |
| `MAS EC Ice` | `MAS Tank H2 1, 2` Fill | Hydrogen Fill $< 90\%$ | Enable Ice Conveyor Sorter & O2/H2 Gens | Normal Conveyor Routing |
| `MAS EC Combat` | Hull Integrity / Target Lock | Damage Detected OR Hostile Lock | Engage `MAS Turrets` · Elevate `RIG.OS.MODE CBT` | Standby Watchkeeping |
| `MAS EC Seismic` | `MAS Seismic Outrigger 1..4` | All 4 Pads = Locked | Pulse Seismic Scan · Publish `TMS.SCOUT.VEIN` | Standby |
| `MAS EC Drone Repair` | `MAS Kestrel Dock 1, 2` | Either Pad = Connected | Pulse `MAS Welders` ON (5.0 s timer) | `MAS Welders` OFF |
| `MAS EC LifeSupport` | `MAS Tank O2 1, 2` / Pressure | O2 $< 25\%$ OR Pressure $< 0.5$ atm | Sound O2 Alarm · Seal Compartment Doors | Normal Atmospheric Circulation |
| `MAS EC BlackBox` | Consist Power Output | Total MW Output $= 0.0$ MW | Activate `MAS Battery BlackBox` & SOS Beacon | Standby Isolation |
| `MAS EC Atmosphere Mode` | Ambient Atmospheric O2 | Ambient O2 $\ge 100\%$ | `MAS Vent Atmo Intake` Depressurize ON | `MAS Vent Atmo Intake` Power OFF |

#### 5. IGC Channel Subscriptions (Ground Fleet & RIG//OS Bus)
* Network Beacon: `RIG.C2.HEARTBEAT` (`<origin_id>;<role:MOTHERSHIP|BASE|WORKER>;<gps_coords>`).
* Network Handover: `RIG.C2.HANDOVER` (`<source_id>;<target_id>;<asset_registry_payload>`).
* Logistics Outbound: `TMS.MULE.CALL` (`CALL;<requesting_pad>;<priority>;<payload_type>`).
* Logistics Outbound: `TMS.CAT.CALL` (`CALL;<consist_id>;<bulk_ore_t>;<gps_coords>`).
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





---

## Chapter 11 · CATERPILLAR-01 Land Train (CAT-01)

Source: `docs/rig/chapters/caterpillar-instructions.md` · 42 B Length · Articulated Drawbar Hitch & Modular Hoppers

### 0 · Reference Grid, In-Game Paint Palette & Consist Envelope

#### 0.1 Coordinate Triad & Axis Conventions
* **Consist Reference Origin (X0, Y0, Z0):** Locomotive center chassis at Level L1 (midpoint of locomotive wheelbase).
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0.
* **Y (Elevation):** Wheel hubs / belly frame is Level L1, advancing upward to Cab/Hopper Roof at Level L4.
* **Z (Length / Longitudinal):** Stern discharge bumper is negative (−), Bow push bumper is positive (+).

```
Spatial Alignment Triad:
       [+Y: Zenith / Consist Roof L4]
              │
              │   [+Z: Bow / Locomotive Push Bumper]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Final Car Discharge Port]
              │
       [−Y: Ground Contact / Level L0]
```

#### 0.2 In-Game Paint Palette & Functional Color Coding

To eliminate builder confusion across a 105 m multi-unit train, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Locomotive chassis, hopper undercarriages, wheel well frames |
| **Slot 1** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Locomotive forward push bumper, final car rear dump bumper |
| **Slot 2** | Hazard Amber | `#E6A100` | Bright Amber | 2-Axis Drawbar Hitches (`CAT Hitch Yaw & Pitch`), jumper hoses |
| **Slot 3** | Freight Transport Orange | `#D35400` | Deep Orange | Modular Hopper Car outer hull plating, top funnel intake chutes |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Train Conveyor Spine, large-port flexible drawbar pipes |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Modular Large Cargo Container holds (`CAT Hold 1, 2, 3`) |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Locomotive Power Core (1× Small Reactor, 4× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Locomotive Armored Cab shell, driver console framing |
| **Slot 8** | Clean Armor White | `#ECF0F1` | Clean White | Consist unit numbers, roof warning stripes, cab beacon base |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Side running boards, grab irons, brake reservoir tanks |

---

#### 0.3 Consist Profile Elevation & Unit Layout

```
Consist Layout (Looking Starboard, Total Length: 42 B / 105.0 m):
     Z: +25 (Bow)      +17      +15      +8       +6       −1       −3      −10 (Stern)
  L4    ┌───────────────┐        ┌────────┐        ┌────────┐        ┌────────┐
        │ LOCOMOTIVE    │        │ HOPPER │        │ HOPPER │        │ TENDER │
  L3    │ ARMOR CAB     │        │ CAR 1  │        │ CAR 2  │        │ CAR 3  │
  L2    ├───────────────┤ [HITCH]├────────┤ [HITCH]├────────┤ [HITCH]├────────┤
        │[BUMP][FRONT C]│ ═══╦═══│[HOLD 1]│ ═══╦═══│[HOLD 2]│ ═══╦═══│[HOLD 3]│[DUMP]
  L1    └──[WH 1]─[WH 2]┘    ║   └──[W][W]┘    ║   └──[W][W]┘    ║   └──[W][W]┘
        ◄── 7W×9L×4H B ─►    ▼   ◄5W×7L×4H►    ▼   ◄5W×7L×4H►    ▼   ◄5W×7L×4H►
                           Drawbar 1         Drawbar 2         Drawbar 3
```

---

#### 0.4 Master Train Conveyor Flow Schematic

The train functions as a single continuous conveyor pipeline from the locomotive front push bumper through all three hopper holds to the final car discharge connector:

```
                   [MASTODON STERN BUMPER]
                              │
                              ▼
      [CAT Loco Front Conn] (Locomotive Push Bumper, L2, X0)
                              │
                              ▼ (Locomotive Underbody Spine)
      [DRAWBAR HITCH 1] (Flexible Large Conveyor Pass-Through)
                              │
                              ▼
      [CAT Car 1 Hold] (Large Cargo Container 3×3×3 B)
                              │
                              ▼
      [DRAWBAR HITCH 2] (Flexible Large Conveyor Pass-Through)
                              │
                              ▼
      [CAT Car 2 Hold] (Large Cargo Container 3×3×3 B)
                              │
                              ▼
      [DRAWBAR HITCH 3] (Flexible Large Conveyor Pass-Through)
                              │
                              ▼
      [CAT Car 3 Hold] (Large Cargo Container 3×3×3 B)
                              │
                              ▼ (Stern Car Drain Line)
      [CAT Train Sorter] (Conveyor Sorter · Drain All = ON)
                              │
                              ▼
      [CAT Train Unload Conn] (Final Car Rear Bumper Connector, L2)
                              │
                              ▼ (Dumps 2,500 t ore in < 90 s)
                   [GROUND BASE RECEIVING HOPPER]
```

---

#### 0.5 Master Electrical Single-Line Schematic

```
               LOCOMOTIVE POWER PLANT (1× Small Reactor + 4× Large Batteries)
                    15 MW Nuclear · 12 MWh Battery · 48 MW Max Discharge
                                      │
                                      ▼
                             TRAIN-WIDE POWER BUS
        ┌─────────────────────────────┼─────────────────────────────┐
        │                             │                             │
        ▼                             ▼                             ▼
  LOCOMOTIVE DRIVE              HOPPER CAR DRIVE MOTORS       AUTOMATION & BRAKES
  4× 5×5 Heavy Wheels           12× 5×5 Wheel Drive Motors    Synchronized Air Brakes
  (8.0 MW Max Hill Climb)       (24.0 MW Distributed Traction) C2 Comms Relay (0.1 MW)
  [Cruising Load: 2.2 MW]       [100% Shared Torque Assist]   Unload Sorter Pumps (0.5 MW)
```

---

#### 0.6 Master Pre-Flight Bill of Materials (BOM)

| Consist Unit | Dimensions | Block Count | Dry Mass | PCU | Bulk Capacity |
|---|---|---|---|---|---|
| **Tractor Locomotive** | 7 W × 9 L × 4 H B | 142 | 146.5 t | 1,220 | Power, Cab & Steering |
| **Drawbar Hitch (ea ×3)**| 1 W × 2 L × 1 H B | 6 (18 total) | 8.2 t (24.6 t) | 200 (600) | 2-Axis Conveyor Link |
| **Hopper Car (ea ×3)** | 5 W × 7 L × 4 H B | 78 (234 total)| 64.0 t (192.0 t)| 650 (1,950)| 1× Large Cargo (833 t ore) |
| **COMPLETE 3-CAR TRAIN**| Total Length: 42 B | **394** | **≈ 363.1 t** | **3,770** | **≈ 2,500 t Bulk Ore** |

---

### Phase 1 · Tractor Locomotive Construction (Levels L1-L4)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Unit: Tractor Locomotive (CAT-LOCO)
       Envelope: 7 W × 9 L × 4 H B (17.5 × 22.5 × 10.0 m)
       Coordinate Range: X −3 to +3, Z +17 to +25, Y = L1 to L4
       Key Datums:
       · Front Bumper Face: Z = +25, Level L1-L2
       · Rear Hitch Bulkhead: Z = +17, Level L1-L2
       · Front Steering Axle: Z = +23 (X = ±3)
       · Rear Drive Axle: Z = +19 (X = ±3)
       · Engineer Cab Center: [X0, Z +22, Y = L3]
```

#### Step 11.1 · Locomotive Heavy Chassis & 4-Wheel Powertrain (L1-L2)

Build the 7 W × 9 L heavy-armor box chassis spanning Z +17 to +25. Mount 4× 5×5 heavy suspension wheels outward at X = ±3.

```
Plan view, Y = L1 (Locomotive Wheel Placement & Steering)
        X: −3          0          +3
    Z: +23 [WHEEL 1 (P)] ═══════ [WHEEL 2 (S)]   ◄── Front Steering Axle (±32.0°)
           ║                     ║
    Z: +21 ║   POWER VAULT CORE  ║
           ║                     ║
    Z: +19 [WHEEL 3 (P)] ═══════ [WHEEL 4 (S)]   ◄── Rear Fixed Axle (0.0° · Locked)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.1 PARTS TRAY & SUSPENSION TUNING GUIDE                                       │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Axle Settings & Parameters               │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 42  │ 126.0 t / 42  │ Palette Slot 0 (Slate); perimeter box    │
│ Wheel 5×5 Susp (L)  │ 2   │  16.0 t / 200 │ Port side (X = −3), face outward (−X)    │
│ Wheel 5×5 Susp (R)  │ 2   │  16.0 t / 200 │ Stbd side (X = +3), face outward (+X)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings for `CAT Loco Wheels`:**
* Front Axle (`WH 1, 2`): Steering = `ON` (Angle = `32.0°`), Propulsion = `ON` (Torque = `100%`), Friction = `70%`, Strength = `65%`
* Rear Axle (`WH 3, 4`): Steering = `OFF` (Locked 0°), Propulsion = `ON` (Torque = `100%`), Friction = `75%`, Strength = `70%`
* Speed Limit: `70 km/h`

---

#### Step 11.2 · Armored Engineer Cab & Power Plant (L2-L4)

Install the nuclear power generator, battery bank, and elevated operator cab. The power core delivers 15 MW continuous baseline with 48 MW battery surge capacity to feed all trailing hopper cars.

```
Elevation, looking Port at Locomotive (X = 0):
       L4  ┌───────────────────┐  Roof Deck (Beacon & Antenna)
           │ [INDUSTRIAL CAB]  │  (Palette Slot 7: Navy Blue)
       L3  ├───────────────────┤
           │ [RCT] [BAT 1..4]  │  ◄── Power Vault (Palette Slot 6: Red)
       L2  ├───────────────────┤
           │ [CONVEYOR SPINE]  │  ◄── Level L2 Conveyor Pass-Through
       L1  ═════[WH 3]═══[WH 1]═  Ground Contact Baseline
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.2 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Output                     │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Industrial Cockpit  │ 1   │   3.5 t / 15  │ `CAT Loco Helm` (Centered X0, Z +22, L3) │
│ Small Reactor       │ 1   │   1.6 t / 25  │ `CAT Loco Reactor` (15 MW baseline power)│
│ Large Battery       │ 4   │  20.0 t / 80  │ `CAT Loco Battery 1..4` (12 MWh, 48 MW)† │
│ Radio Antenna (Lrg) │ 1   │   8.0 t / 100 │ `CAT Loco Antenna` (Range: 25,000 m)     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 11.3 · Bow Push Bumper & Mastodon Docking Port (L1-L2)

Construct the reinforced forward push-bumper and large receiving connector. This allows the train to mate directly with the Mastodon stern bumper to take on bulk ore in the field.

```
Front elevation, looking Stern at Bow Bumper (Z = +25)
        X: −2     0    +2
    L3     ┌─────────────┐  ◄── Armored Cab Panoramic Viewport
    L2     ■──[CAT FRONT]─■  ◄── Large Connector (`CAT Loco Front Conn`)
    L1     ■═════════════■  ◄── Heavy Push Bumper (Palette Slot 1: Crimson)
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 11.1A: LOCOMOTIVE SPINE & POWER INTEGRITY**
> Before enclosing the locomotive side armor:
> 1. Verify uninterrupted large-conveyor flow from `CAT Loco Front Conn` (Z +25) straight through to the rear hitch bulkhead (Z +17).
> 2. Ensure reactor automated uranium pull is operational.
> 3. Verify driver cockpit controls: Handbrake, Group `CAT Train Wheels`, Group `CAT Hitches`, Connector lock/unlock.

---

##### Visual Milestone 11-1 · Tractor Locomotive Complete

```
Visual Checkpoint Silhouette (Looking Port Side at Locomotive):
                       [ARMORED CAB L3]
                              │
      [PUSH BUMPER L1-L2]     │             [REAR HITCH BULKHEAD]
             \                │                     /
     ═════════╧═══════════════╧═════════════════════╧═════════
                 7 W × 9 L × 4 H TRACTOR LOCOMOTIVE (L1)
```

* [ ] Locomotive empty mass verified: `≈ 146.5 t`.
* [ ] Front wheels steer smoothly through ±32° without chassis binding.
* [ ] Power plant delivers 63 MW combined surge power to the main bus.

---

### Phase 2 · 2-Axis Articulated Conveyor Drawbar Hitch (Levels L1-L2)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Unit: 2-Axis Drawbar Articulated Hitch (CAT-HITCH)
       Envelope: 1 W × 2 L × 1 H B (2.5 × 5.0 × 2.5 m)
       Coordinate Range: X0, Z spans 2 B between vehicle bulkheads, Y = Level L2
       Kinematic Axes:
       · Yaw Hinge (Vertical Pivot): Mounted to leading car rear bulkhead
       · Pitch Hinge (Horizontal Pivot): Mounted to trailing car forward bulkhead
       · Conveyor Tube: Flexible curved large pipes bridging the joint
```

#### Step 11.4 · 2-Axis Motorized Conveyor Drawbar Assembly

Install the dual motorized hinge mechanism connecting the locomotive to Hopper Car 1, and each hopper car to the next. The joint provides complete 2-axis articulation (horizontal steering yaw and vertical terrain cresting pitch) while carrying an airtight large-port conveyor line.

```
2.5D Isometric Callout · 2-Axis Conveyor Drawbar Hitch Kinematics

  Leading Unit Rear Bulkhead (Level L2)
            ║
            ╠══[CAT Hitch Yaw] (Motorized Hinge · Vertical Axis: −45° to +45°)
            ║         │
            ║         ▼
            ║    [Drawbar Link 1 B] (Palette Slot 2: Hazard Amber)
            ║         │
            ║         ▼
            ║   [CAT Hitch Pitch] (Motorized Hinge · Horizontal Axis: −25° to +25°)
            ║         │
            ║         ▼ (Flexible Large Curved Conveyor Tube Pass-Through)
            ╚═════════╪═════════════════════════════════════════════════════════════
                      ▼
        Trailing Car Forward Bulkhead (Level L2)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.4 PARTS TRAY & HITCH RIGGING GUIDE                                           │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Motor Hinge (Large) │ 2   │   8.0 t / 200 │ Share Inertia Tensor = ON · Torque: 30 MN│
│ Curved Conveyor Pipe│ 2   │   1.6 t / 20  │ Large-port flexible piping through hitch │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

##### Joint Settings for `CAT Hitch Yaw` & `CAT Hitch Pitch`:
* **Share Inertia Tensor = `ON`** (MANDATORY: eliminates trailer sway, phantom physics wobble, and violent oscillation across subgrids).
* **Yaw Hinge (`CAT Hitch Yaw`):**
  * Axis: Vertical (Yaw / Azimuth).
  * Limits: Min = `−45.0°`, Max = `+45.0°` (enables tight 90° intersection turns).
  * Braking Torque: `30 MN·m`†.
* **Pitch Hinge (`CAT Hitch Pitch`):**
  * Axis: Horizontal (Pitch / Elevation).
  * Limits: Min = `−25.0°` (hill trough flex), Max = `+25.0°` (hill crest flex).
  * Braking Torque: `30 MN·m`†.

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 11.2A: DRAWBAR JOINT ALIGNMENT**
> 1. Set `Share Inertia Tensor = ON` on BOTH hinges before connecting trailing car power.
> 2. Verify large conveyor pipe passes freely through the hinge body with zero port misalignment.
> 3. Perform manual flex test: pivot yaw through ±45° and pitch through ±25°; ensure zero block collision between vehicle bulkheads.

---

##### Visual Milestone 11-2 · 2-Axis Drawbar Hitch Rigged

```
Visual Checkpoint Silhouette (Top-Down View of Articulated Joint):
       [LEADING UNIT] ═══[YAW HINGE]──[LINK]──[PITCH HINGE]═══ [TRAILING CAR]
                                 \              /
                          Articulated Conveyor Jumper Line
```

* [ ] Drawbar allows smooth horizontal pivoting and vertical flex under load.
* [ ] Zero phantom physics torque observed while parked on slopes.
* [ ] Airtight conveyor vacuum maintained through the drawbar link.

---

### Phase 3 · Modular Hopper Car Assembly (Per Unit) (Levels L1-L4)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Unit: Modular Hopper Car (CAT-CAR)
       Envelope: 5 W × 7 L × 4 H B (12.5 × 17.5 × 10.0 m)
       Coordinate Range: X −2 to +2, Z spans 7 B per car, Y = L1 to L4
       Hold Datum:
       · Large Cargo Container (3×3×3 B): Centered at X0, Y = L2-L4
       · Axle 1: Front of car (X = ±2)
       · Axle 2: Rear of car (X = ±2)
```

#### Step 11.5 · Hopper Car Chassis & 4-Wheel Traction Assist (L1-L2)

Build a 5 W × 7 L heavy-armor chassis. Install 4× 5×5 suspension wheels outward at X = ±2. Configure all hopper wheels for distributed traction drive: steering is locked at 0° while drive motors provide 100% torque assist to prevent dragging dead weight.

```
Plan view, Y = L1 (Hopper Car Wheel Layout)
        X: −2          0          +2
    Z: +3  [WHEEL P1] ═══════════ [WHEEL S1]   ◄── Front Fixed Axle (0.0° · Propulsion ON)
           ║                     ║
        0  ║   CARGO HOLD BAY    ║
           ║                     ║
    Z: −3  [WHEEL P2] ═══════════ [WHEEL S2]   ◄── Rear Fixed Axle (0.0° · Propulsion ON)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.5 PARTS TRAY & WHEEL CONFIGURATION                                           │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Placement                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 32  │  96.0 t / 32  │ Palette Slot 0 (Slate); hopper frame box │
│ Wheel 5×5 Susp (L/R)│ 4   │  32.0 t / 400 │ 2 pairs (Axles grouped at Z ±2, X = ±2)  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings for `CAT Car Wheels`:**
* Steering = `OFF` (All hopper wheels track fixed straight ahead).
* Propulsion = `ON` (Distributes driving torque across all consist wheels, 100% power assist).
* Friction = `75%` · Strength = `70%` · Speed Limit = `70 km/h`.

---

#### Step 11.6 · Large Cargo Hold & Top Funnel Loading Chutes (L2-L4)

Mount 1 standard Large Cargo Container (3×3×3 B) centered in the car chassis. Frame the top intake port with angled heavy armor slopes to create an open funnel hopper chute for top-loading from overhead conveyor chutes.

```
Plan view, Y = L2 (Hopper Car Midline)
        X: −2     0    +2
    Z: +3  ■═════╤═════■  ◄── Drawbar Hitch Inbound (Level L2)
           │ ╲   │   ╱ │
        0  │  LARGE    │  ◄── Large Cargo Container (3×3×3 B · Hold)
           │  CARGO    │      (Palette Slot 5: Logistics Emerald)
           │ ╱   │   ╲ │      Top: Funnel Hopper Chutes (Palette Slot 3: Orange)
    Z: −3  ■═════╧═════■  ◄── Drawbar Hitch Outbound (Level L2)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.6 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Capacity & Mounting               │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo (3×3×3) │ 1   │  28.5 t / 250 │ `CAT Hold` (1,386,000 L @ 3× Inventory)  │
│ Conveyor Junction   │ 2   │   2.6 t / 26  │ Front and rear drawbar tie junctions     │
│ Heavy Armor Slopes  │ 24  │  36.0 t / 24  │ Palette Slot 3 (Orange); funnel chutes   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 11.3A: HOPPER CONVEYOR FLOW & TRACTION TEST**
> 1. Check hold capacity: verify each car adds `1,386,000 L` cargo volume (total 3-car hold = 4,158,000 L).
> 2. Ensure hopper wheel drive motors engage simultaneously with the locomotive throttle.
> 3. Verify top funnel intake connects directly to the container's upward large port.

---

##### Visual Milestone 11-3 · Modular Hopper Car Complete

```
Visual Checkpoint Silhouette (Looking Port at Single Hopper Car):
                       [TOP FUNNEL CHUTE L4]
                                 │
                       [LARGE CARGO HOLD L2-L3]
                                 │
     ═══════[WH P2]══════════════╧══════════════[WH P1]═══════
                 5 W × 7 L × 4 H MODULAR HOPPER CAR UNIT
```

* [ ] Car dry mass verified: `≈ 64.0 t`.
* [ ] All 4 wheels provide active propulsion assist under throttle command.
* [ ] Conveyor junctions provide airtight pass-through along the central axis.

---

### Phase 4 · High-Speed Terminal Discharge & Stern Bumper (Levels L1-L2)

```
       [PHASE 4 SPATIAL LOCATOR HUD]
       Unit: Stern Dump Interface (CAT-UNLOAD)
       Location: Aft face of Hopper Car 3 (Z = −10, Level L2, X0)
       Interface Height: 2.5 m above ground (matches Ground Base Hopper Connector)
```

#### Step 11.7 · Rear Train Discharge Connector & Sorter Pump (L1-L2)

Equip the rear bumper of the final hopper car (Car 3) with a high-capacity discharge connector and dedicated conveyor sorter pump.

```
Rear elevation, looking Bow at Final Car Bumper (Z = −10)
        X: −2     0    +2
    L3     ■═════╤═════■  ◄── Rear Collision Bumper (Palette Slot 1: Crimson)
    L2     │ [UNLOAD]  │  ◄── Large Connector (`CAT Train Unload Conn`)
    L1     ■═════╧═════■
           ◄── 1.25 m Ground Clearance ──►
```

```
2.5D Isometric Callout · Train Docking to Ground Base Receiving Hopper

   Final Hopper Car Bumper (Z −10)
                 │
                 ▼
     [CAT Train Unload Conn] (Connector Facing Stern −Z)
                 │
                 ▼ (Reversing into Terminal Dock)
     [GND CAT Dump Conn] (Ground Base Receiving Connector, Facing North +Z)
                 │
   ══════════════╪══════════════════════════════════════════════════════════
     [COLL 1] [COLL 2] [COLL 3] [COLL 4] [COLL 5] (5× In-Deck Collectors)
                 │
                 ▼ (Drains 2,500 t Bulk Ore in < 90 Seconds)
         Ground Base Industrial Refining Loop (Chapter 4)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.7 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Role & Operating Configuration           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `CAT Train Unload Conn` (Facing Stern −Z)│
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `CAT Train Sorter` (Drain All = ON)      │
│ Heavy Armor Slopes  │ 8   │  12.0 t / 8   │ Palette Slot 1 (Crimson); push bumpers   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 11.4A: DISCHARGE DRAIN VERIFICATION**
> 1. Set `CAT Train Sorter` whitelist to: `Iron`, `Nickel`, `Silicon`, `Cobalt`, and `Stone`.
> 2. Verify `CAT Train Unload Conn` centerline height is exactly at Level L2 (2.5 m above tarmac).
> 3. Verify sorter drain pulls from all 3 cars simultaneously when connector is locked.

---

##### Visual Milestone 11-4 · Complete 3-Car Land Train Consist Commissioned

```
Visual Checkpoint Silhouette (Looking Port at Complete 42 B Consist):
     [LOCOMOTIVE] ══[HITCH 1]══ [CAR 1] ══[HITCH 2]══ [CAR 2] ══[HITCH 3]══ [CAR 3/DUMP]
       4× Wheels                  4× Wheels              4× Wheels              4× Wheels
     ═════════════════════════════════════════════════════════════════════════════════════
                 CATERPILLAR-01 42 B LAND TRAIN (16 WHEELS TOTAL)
```

* [ ] Total consist dry mass verified: `≈ 363.1 t`.
* [ ] All 16 suspension wheels share synchronized drive torque across 4 subgrids.
* [ ] 3-car consist tracks through S-curves without trailer collision or jackknifing.

---

### Phase 5 · Master Control Logic & Hardware Scripting Contract (Claude Code)

#### Step 11.8 · Terminal Namespace & Group Taxonomy

##### 1. Group Taxonomy
* `CAT Train Wheels` : All 16 wheel units across locomotive and 3 hopper cars.
* `CAT Train Brakes` : Synchronized handbrake and parking brake group.
* `CAT Hitches` : All 6 motorized hitch hinges linking the consist.
* `CAT Holds` : All 3 Large Cargo Containers across the consist.
* `CAT Loco Power` : 1× Small Nuclear Reactor + 4× Large Batteries.

##### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `CAT EC Dump` | `CAT Train Unload Conn` | Connector: Connected | `CAT Train Sorter` Drain ON | Sorter OFF |
| `CAT EC AntiJackknife`| Vehicle Velocity | Velocity $\ge 45\text{ km/h}$ | `CAT Hitches` Damping $\rightarrow 85\%$ (Anti-sway) | Damping $\rightarrow 20\%$ (Flex) |
| `CAT EC Traction` | Consist Cargo Mass | Total Mass $\ge 1,500\text{ t}$ | `CAT Car Wheels` Torque $\rightarrow 100\%$ | Torque $\rightarrow 60\%$ |

##### 3. IGC Channel Subscriptions (Ground Fleet Bus)
* Outbound: `TMS.CATER.STATUS`: `train_id;ore_mass_t;car_count;speed_kmh;route_status`
  * Example: `CAT-01;2480.0;3;52.4;EN_ROUTE_BASE`
* Inbound: `TMS.BASE.RECEIVING`: signals Ground Base terminal hopper is clear to receive.
* Inbound: `TMS.MAST.TRANSFER`: signals Mastodon stern transfer is active.

---

### Phase 6 · Commissioning Checklist & Acceptance Trials (RIG Test)

#### A · Articulation & Maneuvering Trials
* [ ] **S-Curve Clearance:** Drive the 42 B consist through an S-curve at 30 km/h; verify zero wheel binding or chassis contact between cars.
* [ ] **Hill-Climb Under Full Load:** Fill all 3 hoppers with 2,500 t stone; climb a 20° voxel incline; confirm all 16 wheels share propulsion torque without stalling or spinning out.

#### B · High-Speed Stability & Anti-Jackknife
* [ ] **70 km/h Highway Run:** Drive along a flat surface at 70 km/h; confirm `CAT EC AntiJackknife` engages 85% hinge damping, preventing trailer sway or whip.
* [ ] **Emergency Brake Stop:** Apply full brakes from 60 km/h; confirm all trailers brake in unison without shoving the locomotive sideways.

#### C · Fast-Dump Terminal Validation
* [ ] **Field Loading at Mastodon:** Back locomotive push bumper into Mastodon stern connector; verify 2,500 t bulk ore fills all 3 hopper cars.
* [ ] **Terminal Discharge at Base:** Reverse consist into Ground Base receiving dock; verify `CAT Train Unload Conn` locks and empties 2,500 t of ore in $< 90\text{ seconds}$.


---

# Volume III · Floating Rig Structure (25 kt Fortress)

This volume details the physical construction of the Floating Rig: an aerial fortress stationed at Earth 1 g.

```
Rig Master Elevation (Looking North):
                       [H1 Command & Roof Pickets]          (S9)
                       [H2 Habitat & Life Support]          (S8)
                       [H3 Gas Farm & Core Shop]            (S7)
  ◄── Pylon S1 ──► ╔═════════════════════════════════╗ ◄── Pylon S2 ──►
  (8 W × 7 H × 22 L)║ H4 Power Core (3× Large React) ║ (8 W × 7 H × 22 L)
                    ║ H5 Pylon Sockets & Gyros       ║
                    ║ H6 Lift Ring (10 A + 6 H)      ║
                    ╚═════════════════════════════════╝
                                 ▼▼▼▼▼
```

---

## Chapter 6 · Section S0: HUB Base (H4-H6)

The HUB base is the first section welded and the first to fly. It carries the primary power plant, main lift ring, and the docking sockets for all 6 pylons.

### 6.0 · Reference Envelope & Master Geometry
* **Footprint:** Hexagonal core, 18 B diameter (45 m) × 12 B height (30 m).
* **Mass (dry base):** ≈ 7,200 t (including heavy armor shielding).
* **Seed Kit:** H6 lift thrusters, 4 Large Batteries, Remote Control on H5, 6 Gyroscopes, Antenna.

---

### Step 6.1 · H6 Lift Ring (Levels L1-L4)

Mount the central lift cluster providing 19% of the Rig's total atmospheric lift and blackout emergency hover.

```
Plan view, Y = L2 (Lift Deck Core)
        X: −4   −2    0   +2   +4
    Z: +4  ···  [A]  [H]  [A]  ···
    Z: +2  [A]  ···  [A]  ···  [A]
    Z:  0  [H]  [A]  [H]  [A]  [H]   ◄── 10× Large Atmo [A] + 6× Large H2 [H]
    Z: −2  [A]  ···  [A]  ···  [A]
    Z: −4  ···  [A]  [H]  [A]  ···
```

**Blocks:**
* 10 × Large Atmospheric Thruster (large grid, 6.48 MN each = 64.8 MN total) · `RIG H6 Lift Atmo 1..10` → group `RIG Lift Atmo HUB`
* 6 × Large Hydrogen Thruster (large grid, 7.2 MN each = 43.2 MN total) · `RIG H6 Lift H2 1..6` → group `RIG Lift H2 HUB`
* 72 × Heavy Armor Slopes & Corners framing the thruster exhaust nozzles.

**✓ Check:** Nozzles face straight down (D). Center of lift aligns within 0.1 m of the central vertical axis (0, 0).

---

### Step 6.2 · H5 Junction & Pylon Socket Deck (Levels L5-L8)

Houses the six pylon docking sockets, master conveyor trunks, and attitude gyroscopes.

```
Plan view, Y = L6 (Socket Ring)
             [SOCKET P-NW]       [SOCKET P-NE]
                    ╲                 ╱
                     ┌───────────────┐
      [SOCKET P-W] ══╡  TRUNK RING   ╞══ [SOCKET P-E]
                     │  12× GYROS    │
                     └───────────────┘
                    ╱                 ╲
             [SOCKET P-SW]       [SOCKET P-SE]
```

**Blocks:**
* 12 × Merge Block · 2 per socket (`RIG H5 Socket P-E MRG 1, 2` …)
* 6 × Large Grid Connector · 1 per socket center (`RIG H5 Socket P-E Conn` …)
* 2 × Conveyor Loop Ring · Trunk A (WNW spine) and Trunk B (ESE spine) feeding every deck
* 12 × Large Gyroscope (large grid) · `RIG H5 Gyro 1..12` → group `RIG Gyros`
* 6 × Spark Array Blocks (visual docking rungs)

**✓ Check:** Each of the 6 sockets presents an 8 W × 7 H receiving face with 2 Merge Blocks and 1 central Large Connector.

---

### Step 6.3 · H4 Nuclear Power Vault (Levels L9-L12)

Houses three Large Nuclear Reactors enclosed in double-walled heavy armor vaults.

```
Plan view, Y = L10 (Reactor Deck)
        X: −4    0   +4
    Z: +3  ···  [R1] ···
    Z: −2  [R2] ···  [R3]   ◄── 3× Large Nuclear Reactors (300 MW each = 900 MW)
```

**Blocks:**
* 3 × Large Nuclear Reactor · `RIG H4 Reactor 1, 2, 3` → group `RIG Reactors`
* 6 × Large Battery (emergency buffer) · `RIG H4 Battery 1..6` → group `RIG Batteries`
* 2 × Hydrogen Engine (auxiliary power) · `RIG H4 H2 Engine 1, 2`
* Double-walled Heavy Armor bulkheads isolating each reactor compartment with blast doors.

**✓ Check:** Total power output = 900 MW. $N-1$ loss leaves 600 MW, covering normal station hover with battery assistance.

---

## Chapter 7 · Sections S1-S6: Pylon Arms & Modular Bays

Six identical cantilevered arms (8 W × 7 H × 22 L blocks ≈ 20 × 17.5 × 55 m) radiating from the H5 sockets.

### 7.0 · Pylon Architecture & Clear Envelopes
* **Build Symmetry:** P-E, P-NE, and P-NW printed by extruder; opposite 3 rotated 180°.
* **Cross-Section:**
  * L1-L2: Reinforced Keel (Trunk A & B conveyors, 2 Large H2 tanks).
  * L3-L6: Hangar Bay Deck (3 modular bays, 6 W × 4 H × 5 L clear box each).
  * L7: Armored Roof Deck (Turret platforms, access gallery).
* **Cross-head Tip:** Plus-shaped end cap with lateral thrusters, 2 Interceptor bays, and defense turrets.

---

### Step 7.1 · Pylon Keel & Conveyor Trunks (L1-L2)

```
Cross-section, looking toward cross-head tip (Z = +10)
    L2   [TRUNK A]  [LARGE H2 TANK 1]  [LARGE H2 TANK 2]  [TRUNK B]
    L1   ■════════════════════════════════════════════════════════■  Keel Armor
```

**Blocks:**
* 2 × Continuous Large Conveyor lines (Trunk A on Port side, Trunk B on Stbd side)
* 2 × Large Hydrogen Tank · `RIG P1 Tank 1, 2` (embedded in center keel)
* 44 × Heavy Armor Sloped Blocks forming a deflective V-hull keel.

**✓ Check:** Both conveyor trunks run uninterrupted from the root socket collar to the tip cross-head.

---

### Step 7.2 · Modular Hangar Bays (L3-L6, Bays 1-3)

Three universal bays per arm, opening alternately into the flanking T-Sectors.

```
Plan view, Y = L4 (Hangar Level)
        Z: 0 (Root)      6           11          16       22 (Tip)
           ┌──────────┬───────────┬───────────┬───────────┬───────┐
    Port   │  COLLAR  │ [BAY 1]   │ Bay Wall  │ [BAY 3]   │ CROSS │
           │  (2 B)   │ (Door P)  │           │ (Door P)  │ HEAD  │
           ├──────────┤           ├───────────┤           │ (6 B) │
    Stbd   │  SOCKET  │ Bay Wall  │ [BAY 2]   │ Bay Wall  │       │
           │  COLLAR  │           │ (Door S)  │           │       │
           └──────────┴───────────┴───────────┴───────────┴───────┘
```

**Blocks per Bay (×3 bays per arm):**
* 1 × Large Grid Connector (wall or ceiling mounted) · `RIG P<n> Bay<m> Conn`
* 1 × Ship Welder (recessed behind grate) · `RIG P<n> Bay<m> Welder` (repair & maintenance)
* 1 × Sensor · `RIG P<n> Bay<m> Sensor` (detects incoming drone at 10 m)
* 4 × Hangar Door Blocks · `RIG P<n> Bay<m> Doors` (slides into roof/floor)
* 1 × Spotlight · `RIG P<n> Bay<m> Light` (switched by sensor)

**✓ Check:** Clear bay interior: 6 B wide × 4 B high × 5 B deep. Drone length $\le$ 5 B clears all door frames.

---

### Step 7.3 · Pylon Lift Pods & Tip Cross-Head (L1-L7)

Mount 7 Large Atmo thrusters and 4 Large H2 thrusters along the pylon span.

**Thruster Allocation per Pylon:**
* 7 × Large Atmospheric Thruster (D) · 3 along keel flanks + 4 in tip cross-head pod (45.36 MN lift).
* 4 × Large Hydrogen Thruster (D) · embedded in keel (28.8 MN emergency lift).
* 2 × Interceptor Bay (roof hatches) in the plus-shaped cross-head tip.
* 2 × Defense Turrets (1 top, 1 bottom) at the cross-head extremity.

**✓ Check:** Combined pylon lift = 45.36 MN Atmo (at sea level). Pylon can hover independently during transit to the HUB.

---

## Chapter 8 · Sections S7-S9: Upper Decks (H1-H3)

Stacked on the base by the Rig's construction projector or assembled in place.

### Step 8.1 · Section S7: H3 Gas & Utility Deck
* **Contents:** 4 Large O2/H2 Generators, 8 Hydrogen Tanks (gas farm), core machine shop, component buffer.
* **Conveyor Interface:** Feeds Trunk A & Trunk B across the MRG seam.

### Step 8.2 · Section S8: H2 Habitat & Life Support
* **Contents:** Pressurized crew quarters, medical room, cryo pods, hydroponics farm, Gravity Generator.
* **Armor:** Light armor exterior with interior composite blast bulkheads.

### Step 8.3 · Section S9: H1 Command Deck & Avionics
* **Contents:** Master armored bridge, Remote Control core, Antenna Mast, RIG//OS Programmable Block Rack (4 custom PBs).
* **Roof Deck:** 2 × Picket Drone landing pads (`PKT × 2`) + Point Defense Gatling Turrets.

---


---

---

---

---

---

---

# Volume IV · Hardware-to-Scripting Interface Contract

This volume defines the software contract for **Claude Code** and the **RIG//OS** Programmable Blocks.

```
RIG//OS Architecture:
  ┌─────────────────────────────────────────────────────────────┐
  │ [CMD] PB Rig OS (Core State Machine, Lift, Modes, Heartbeat)│
  └──────────────┬──────────────────────────────┬───────────────┘
                 │                              │
                 ▼                              ▼
  ┌─────────────────────────────┐┌──────────────────────────────┐
  │ [LOG] PB Dispatcher         ││ [MFG] PB Inventory           │
  │ (Bay Queue, Leases, Comms)  ││ (Quotas, Sorting, Assemblies)│
  └──────────────┬──────────────┘└──────────────┬───────────────┘
                 │                              │
                 └──────────────┬───────────────┘
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │ [CMD] PB Displays (Zero-GC LCD Dashboards & Fleet Status)   │
  └─────────────────────────────────────────────────────────────┘
```

---

## Chapter 9 · RIG//OS Hardware Abstraction & IGC Dictionary

### 9.1 Operating Modes & Timer Table
Modes are driven by hardware Timer Blocks setting system states:

| Mode | Trigger / Event | Lift State | Shield / Weapons | Bay Doors |
|---|---|---|---|---|
| **`NRM` (Normal)** | Standard station ops | 52 A @ 73% hover; H2 OFF | Shields UP; Weapons AUTO | Standard watchdog |
| **`CAU` (Caution)** | Approaching storm / unverified craft | 52 A @ 73%; H2 preheated | Shields UP; Weapons ARMED | Bays recall drones |
| **`SHD` (Shield)** | Incoming kinetic / artillery fire | Atmo hover | Shields 100% OVERDRIVE | All bays SEALED |
| **`EMG` (Emergency)** | Reactor failure / pylon loss | 52 A @ 100% + 30 H2 FIRE | Auxiliary batteries ON | Recall or abandon |
| **`CBT` (Combat)** | Station under active assault | High mobility thruster override | All turrets TARGETING | Interceptors SCRAMBLED |
| **`REP` (Repair)** | Station damage detected | Hover locked | Turrets defensive | Welder grids ACTIVE |
| **`REC` (Recovery)** | Re-entry / post-combat return | Stabilized descent | Standby | Recovery lights ON |

---

### 9.2 IGC Broadcast Channels & Message Schemas
All IGC messages use `;`-separated payloads. Tags strictly follow project namespaces:

#### Channel: `RIG.CMD.*` (Command & Heartbeat)
* `RIG.CMD.BEAT`: `station_id;timestamp;mode;power_pct;shield_pct`
  * Example: `RIG-01;123456.78;NRM;71.0;100.0`
* `RIG.CMD.SCRAMBLE`: `target_gps;threat_level;flight_lead`

#### Channel: `FLT.DISP.*` (Dispatcher & Bay Management)
* `FLT.DISP.LEASE`: `drone_id;bay_id;approach_waypoint;lease_time_s`
  * Example: `REM-01;P-E-BAY-1;GPS:Predock:...:;180`
* `FLT.DISP.STATUS`: `drone_id;status_enum;cargo_pct;battery_pct`
  * Example: `MUL-01;APPROACHING;100.0;88.5`

#### Channel: `TMS.*` (Terrestrial Mining System)
* `TMS.ORCA.STATUS`: `orca_id;hold_mass_t;ore_cap_t;drones_active`
  * Example: `ORC-01;450.2;650.0;4`
* `TMS.MULE.CALL`: `requester_id;pad_id;payload_type;urgency`
  * Example: `ORC-01;ORC-MP;ORE;NORMAL`

---

### 9.3 Hardware Failover Logic (Zero-PB Safety)
If all Programmable Blocks fail or crash:
1. **Watchdog Timer:** `RIG TMR Watchdog` expires after 5.0 s without PB pulse.
2. **Failover Execution:**
   * Drills & welders forced OFF.
   * Remote Control engaging station dampeners in 1 g position hold.
   * `RIG EC Power` trips: if reactor output drops < 500 MW, 30 Large H2 thrusters ignite automatically.
   * Interceptors and mining drones fall back to native `PREDOCK` hovering slots.
