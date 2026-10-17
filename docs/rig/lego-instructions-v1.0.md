# Space Engineers 1 · Floating Rig & Logistics Fleet
## Master Lego-Style Build Manual & RIG//OS Interface Contract
### Version 1.0 · Canon Engineering Specification · Earth 1 g Baseline

Last updated: 2026-10-17
Authority: Blae Decisions 1-54 (Canon) · `CLAUDE(RIG).md` · `Part 1 & 2 Handoff`

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
  * [Chapter 5 · Rig Linear Extruder Ring](#chapter-5-rig-linear-extruder-ring)
* [Volume V · Terrestrial Surface Heavy Extraction (Ground Tier · Bulk / Mass)](#volume-v-terrestrial-surface-heavy-extraction-ground-tier-bulk-mass)
  * [Chapter 10 · MASTODON-01 Heavy Wheeled Excavator (11 × 25 × 6 B)](#chapter-10-mastodon-01-heavy-wheeled-excavator)
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
| `MAS` | Large | MASTODON-01 Heavy Excavator | `TUSK` booms, `HOLD` triple cargo, `WHEEL` 5×5 axles, `MP` mule pad, `DUMP` stern bumper |
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

Source: `docs/rig/orca-instructions.md` · 17 × 19 × 7 B · Uncrewed Atmo Drone Carrier

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

Source: `docs/rig/remora-instructions.md` · 11 × 11 sb · Small-Grid Vertical Miner (Top-Docking)

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

Source: `docs/rig/mule-instructions.md` · 7 × 7 B · Pure Electric Cargo Ferry (650 t Payload)

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

Source: `docs/rig/kestrel-instructions.md` · 7 × 9 × 5 sb · High-Speed Recon & Ore Prospector

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

Source: `docs/rig/albatross-instructions.md` · 9 × 11 × 5 sb · Autonomous Field Repair & Component Ferry

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

## Chapter 5 · Rig Linear Extruder Ring

The heavy automated shipyard that prints the Floating Rig's flown sections:
1. **S0 HUB Base (H4-H6):** Flown first on H6 lift.
2. **S1-S6 Pylons (8 W × 7 H × 22 L B):** Printed sequentially, fly up to dock at H5 sockets.

### 5.0 · Reference Grid & Operating Principle

Per **Decision 47**, the extruder uses a stationary U-shaped welder ring. A hydraulic piston pushes the projected blueprint horizontally outward over low-friction ground skid rails.

```
Extruder Elevation (Looking Starboard, X = 0):
                 Stationary Welder Ring
                     ┌──[WELDERS]──┐
                     │    (L1-L8)  │
  [HYD PISTON 1..6] ═╡             │═════════════════════════════════► Launch Direction (+Z)
  [◄ Push Tail ───]  │   [PYLON    │    [Completed Pylon 22 B Span]
  [Projector Head]   │  Extruding] │    [On Skid Rails L1]
  ═══════════════════╧═════════════╧═════════════════════════════════
```

---

### Step 5.1 · 8 W × 7 H Welder U-Ring (Y = L1 to L8)

Build a rigid arch framing an 8 W × 7 H opening. Mount ship welders pointing inward on all 3 interior faces.

```
Front Elevation, looking down the launch axis (Z = 0)
        X: −5  −4  −3  −2  −1   0  +1  +2  +3  +4  +5
    L8     ■═══■═══■═══■═══■═══■═══■═══■═══■═══■═══■  Roof Arch
    L7     ║  [W] [W] [W] [W] [W] [W] [W] [W]  ║  (8 welders pointing DOWN)
    L6     ║[W]                             [W]║
    L5     ║[W]      CLEAR PRINT THROAT     [W]║  (7 welders pointing STBD)
    L4     ║[W]       (8 W × 7 H blocks)    [W]║  (7 welders pointing PORT)
    L3     ║[W]                             [W]║
    L2     ║[W]                             [W]║
    L1     ■═══■═══[SKID ROLLER TRACK]══■═══■═══■  Tarmac Base
```

```
2.5D Isometric Callout · 8 W × 7 H Welder Arch Architecture

                   ┌───────────────────────────────────┐
                   │  Top Welder Rake (8× Ship Welders)│
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

**Blocks:**
* 22 × Ship Welder (large grid)  · 
  * 8 × Top Welder (facing DOWN at L7, X −4 to +3)
  * 7 × Port Welder (facing STBD at X −5, L2 to L8)
  * 7 × Starboard Welder (facing PORT at X +4, L2 to L8)
  * Group ──► `EXT Welder Ring`
* 24 × Conveyor Junction & Heavy Frame blocks supplying parts from `GND STR Components`

**✓ Check:** Welders provide overlapping 1.75 m active weld spheres covering the entire 8 × 7 perimeter and interior blocks.

---

### Step 5.2 · Hydraulic Pusher & Projector Carriage

A horizontal pusher rail at the tail advances the blueprint through the welder throat.

```
Plan view, Y = L2 (Pusher Axis)
        Z: −25         −15          −5        0 (Welder Ring)
  Rail:  ■──[PIST 1]──[PIST 2]──[PIST 3]──[HEAD] ──► Pushes toward +Z
```

```
2.5D Isometric Callout · Hydraulic Pusher & Merge Head

  [ANCHOR WALL]
        ║
        ╠══[PISTON 1]══[PISTON 2]══[PISTON 3]══[PISTON 4]══[PISTON 5]══[PISTON 6]
        ║                                                              │
        ║                                                              ▼
        ║                                                    [EXT PRJ Projector]
        ║                                                    [EXT MRG Release]
        ║                                                              │
        ╚══════════════════════════════════════════════════════════════╪═══════
                                                                       ▼
                                                             [PYLON ROOT COLLAR]
```

**Blocks:**
* 6 × Piston (large grid, mounted in series along Z-axis) · `EXT Pusher 1` … `EXT Pusher 6` ──► group `EXT Pushers`
  * Settings: Velocity = `0.05 m/s`, Share Inertia Tensor = `ON`, Max Distance = `10.0 m` each (60 m total stroke)
* 1 × Projector (large grid) · `EXT PRJ Head` (mounted on pusher tip)
* 1 × Merge Block · `EXT MRG Release` (connects pusher head to pylon root)

**✓ Check:** Pistons extend smoothly at 0.05 m/s without binding. `EXT MRG Release` cleanly disconnects the welded pylon when complete.

---

### Step 5.3 · Low-Friction Skid Roller Track (Z 0 to +25)

A 25 B long ground launch way built from heavy-armor half blocks and blast plates supports the pylon as it emerges.

```
Cross-section, looking toward Launch (+Z)
    L2   [Extruded Pylon Keel]
    L1   [BLAST][HALF] ··········· [HALF][BLAST]  ◄── 2 low-friction guide skids
    L0   ═══════════════════════════════════════  Foundation Tarmac
```

**Blocks:**
* 50 × Heavy Armor Half Block (2 parallel guide tracks along X = −3 and X = +2)
* 50 × Blast Door Block (low friction contact surface)

**✓ Check:** Pylon keel slides freely along the blast door rails with zero deformation or snagging.

---

### Step 5.4 · Pylon Flight Commissioning Sequence

When the 22 B pylon reaches full extension:
1. **Conveyor Lock:** Keel conveyor ports mate with temporary refueling line; fill H2 tanks to 100%.
2. **Seed Kit Activation:** Turn on pylon battery bank, Remote Control, gyros, and antenna.
3. **Lift Thruster Test:** Spool 7 Large Atmo thrusters to 100% override for 2 seconds to confirm lift authority.
4. **Disconnect:** Turn `EXT MRG Release` OFF. Pylon is now an independent grid.
5. **Ascent:** Remote Control engages vertical climb to 500 m altitude; navigates autonomously to the Floating Rig H5 socket.

---

### Step 5.5 · Master AI Scripting Contract & Terminal Namespace (Claude Code)

Every automated device on the Ground Base and Extruder Ring conforms to this exact naming and grouping dictionary:

#### 1. Group Taxonomy
* `GND Refineries` : 4× Large Refineries (`GND REF 1` … `4`).
* `GND Assemblers` : 4× Large Assemblers (`GND ASM 1` … `4`).
* `GND Fleet Displays` : 6× Large LCD Panels (`GND LCD Video Wall 1` … `6`).
* `EXT Welder Ring` : 22× Ship Welders on the printer arch.
* `EXT Pushers` : 6× Hydraulic Extension Pistons.

#### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `GND EC PAD` | `GND PAD Sensor` | Mule Detected at 15 m | `GND PAD Cradle` Angle $
ightarrow$ 30° · `GND PAD Conn` Lock ON | `GND PAD Cradle` Angle $
ightarrow$ 0° · `GND PAD Conn` Lock OFF |
| `EXT EC Print` | `EXT Pusher 6` Position | Extension = Max (60 m) | `EXT Welder Ring` OFF · Sound Print Chime | `EXT Welder Ring` ON |

#### 3. IGC Channel Subscriptions (Fleet Operations Hub)
* Inbound: `TMS.ORCA.STATUS` (Carrier hold capacity, fuel burn, active leases).
* Inbound: `TMS.MULE.STATUS` (Shuttle payload, battery percentage, transit ETA).
* Outbound: `TMS.MULE.CALL` (Automated dispatch of empty/laden freight ferries).
* Outbound: `TMS.BASE.QUOTAS` (Refined component thresholds for shipyard printing).

---

---

# Volume V · Terrestrial Surface Heavy Extraction (Ground Tier · Bulk / Mass)

This volume details the heavy wheeled surface extraction machines that form the high-volume bulk industrial tier of the terrestrial extraction loop.

## Chapter 10 · MASTODON-01 Heavy Wheeled Excavator

Source: `docs/rig/mastodon-instructions.md` · 11 × 25 × 6 B · Twin 3-DOF Laser Tusks & 10× 5×5 Wheels

### 0 · Reference Grid, In-Game Paint Palette & Master Envelope

#### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Intersection of vehicle midline (Z0), belly foundation (L1), and center conveyor spine (X0).
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0.
* **Y (Elevation):** Wheel hubs / belly skid deck is Level L1, advancing upward to Flight Roof at Level L6.
* **Z (Length / Longitudinal):** Stern bumper is negative (−), Bow excavator throat is positive (+), Midline axle is 0.

```
Spatial Alignment Triad:
       [+Y: Zenith / Flight Roof L6]
              │
              │   [+Z: Bow / Tusk Excavator Heads]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Train Dump Bumper]
              │
       [−Y: Ground Contact / Level L0]
```

#### 0.2 In-Game Paint Palette & Functional Color Coding

To prevent disorientation across this 62.5 m mobile behemoth, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Lower chassis box girders, wheel arch frames, belly skid plates |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Roof Mule Gantry Pad (7×7 B), cradle arms, deck edge borders |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Stern train dump bumper, rear alignment push-blocks |
| **Slot 3** | Excavator Ochre | `#E67E22` | Deep Orange | Twin 3-DOF Tusk Booms, arm casings, laser tool heads |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Ore Conveyor Spine, bow intake throat, manifold junctions |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Triple Large Cargo Container holds (`MAS Hold 1, 2, 3`) |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Core Vault (2× Small Nuclear Reactors, 4× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Forward Armored Bridge exterior shell, operator consoles |
| **Slot 8** | Scout Flight White | `#ECF0F1` | Clean White | Forward twin Kestrel scout drone landing pads (P1, S1) |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Exterior catwalks, maintenance ladders, side rub rails |

---

#### 0.3 Master Profile Elevation (Looking Starboard, X = 0)

```
        Z:  −12 (Stern)      −6          0          +6        +12 (Bow)
    L6      ┌─────────────────┬──────────────────────┬──────────────────┐  Flight Roof
            │ MULE GANTRY PAD │ [Roof Armor Walkway] │ 2× KESTREL PADS  │  (Palette 1 & 8)
    L5      ├─────────────────┼──────────────────────┼──────────────────┤
            │ [MAS STR Sorter]│ Large Cargo Hold 1   │ Armored Bridge   │  (Palette 7)
    L4      │ Power Core (2×R)│ Large Cargo Hold 2   │ [ORE DETECTOR]   │
    L3      │ 4× Batteries    │ Large Cargo Hold 3   │ Tusk Shoulder    │  (Palette 3)
    L2      ├─────────────────┴──────────────────────┴──────────────────┤  Chassis Frame
            │ [STERN DUMP] ◄── Continuous Conveyor Spine ──► [BOW THROAT│  (Palette 4)
    L1      └───[WH 9..10]──────[WH 7..8]──[WH 5..6]──[WH 3..4]──[WH 1..2]──┘  Belly Skids
             Axle 5 (−30°)  Axle 4 (−15°) Axle 3 (0°) Axle 2 (+15°) Axle 1 (+30°)
```

---

#### 0.4 Master Conveyor Routing Schematic

```
               TWIN ARTICULATED TUSK BOOMS (3-DOF Booms · Bow)
      [Port Head: 2× Lasers]                 [Stbd Head: 2× Lasers]
                 │                                      │
                 ▼ (Internal Large Hinge Pipe)          ▼ (Internal Large Hinge Pipe)
       ┌─────────────────────────────────────────────────────────────┐
       │ Bow Collector Manifold & Riser (L2, Z +12, X0)              │
       └──────────────────────────────┬──────────────────────────────┘
                                      │
                                      ▼
       ┌─────────────────────────────────────────────────────────────┐
       │ TRIPLE ORE HOLD (3× Large Cargo Containers · In-Line Spine) │
       │ Hold 1 (Z +2..+4) ──► Hold 2 (Z −2..0) ──► Hold 3 (Z −6..−4)│
       └──────────────────────────────┬──────────────────────────────┘
                                      │
         ┌────────────────────────────┴────────────────────────────┐
         │ (Upward Riser Trunk)                                    │ (Stern Bumper Pipe)
         ▼                                                         ▼
  [MAS MP Sorter] (Drain ON)                                [MAS Stern Sorter] (Drain ON)
         │                                                         │
         ▼                                                         ▼
  [MAS MP Conn] (Roof Flush L6)                             [MAS Stern Dump Conn] (L2 Bumper)
  (Offloads 650 t rare ore to Mule)                         (Offloads 2,500 t bulk ore to Train)
```

---

#### 0.5 Master Electrical Single-Line Schematic

```
               POWER CORE (2× Small Nuclear Reactors + 4× Large Batteries)
                    30 MW Nuclear · 12 MWh Battery · 48 MW Max Discharge
                                      │
                                      ▼
                              PRIMARY POWER BUS
        ┌─────────────────────────────┼─────────────────────────────┐
        │                             │                             │
        ▼                             ▼                             ▼
  WHEEL DRIVE POWERTRAIN        TUSK MINING HEADS             FLIGHT & AUXILIARY
  10× 5×5 Wheel Drive Motors    4× Simple Laser Multitools    Mule Float Charger (12 MW)
  (18.0 MW Max Hill Climb)      (8.0 MW Continuous Beam)      2× Kestrel Dock Chargers (2 MW)
  [Cruising Load: 4.5 MW]       2× CTC Control Units          Ore Detector + Comms (0.2 MW)
```

---

#### 0.6 Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Chassis & Hull** | Heavy armor full blocks, slopes, half-blocks (Palette 0) | 520 | 442.0 t | 520 |
| **Powertrain** | 10× 5×5 Heavy Suspension Wheels (large grid) | 10 | 80.0 t | 1,000 |
| **Tusk Booms** | 2× Adv Rotors, 4× Hinges, 4× Laser Multitools (Palette 3) | 10 | 28.0 t | 850 |
| **Cargo Holds** | 3× Large Cargo (3×3×3 B), Conveyor Tubes (Palette 5) | 42 | 88.5 t | 780 |
| **Power Core** | 2× Small Reactors, 4× Large Batteries (Palette 6) | 6 | 33.2 t | 170 |
| **Flight Decks** | 1× Mule Gantry Cradle, 2× Small Connectors (Palette 1 & 8) | 7 | 12.5 t | 250 |
| **Control & C2** | 1× Bridge Cockpit, 2× CTCs, 1× PB, 1× Antenna (Palette 7) | 8 | 10.2 t | 320 |
| **TOTAL (DRY)** | Complete Mobile Excavator (MASTODON-01) | **603** | **≈ 694.4 t** | **3,890** |

---

### Phase 1 · Lower Chassis & 10-Wheel Suspension (Levels L1-L2)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 11 W × 25 L × 2 H B (27.5 × 62.5 × 5.0 m)
       Coordinate Range: X −5 to +5, Z −12 to +12, Y = L1 to L2
       Key Datums:
       · Centerline Spine: X0, spanning Z −12 to +12
       · Longitudinal Girder Port: X = −3
       · Longitudinal Girder Starboard: X = +3
       · Axle Spacing: 5 B (12.5 m) between adjacent wheel centers
```

#### Step 1 · Twin Longitudinal Chassis Box Girders (L1-L2)

Weld two rigid, continuous heavy-armor box girders spanning 25 blocks from Stern (Z −12) to Bow (Z +12) at X = ±3. Connect them with transverse beam ties at each axle datum.

```
Plan view, Y = L2 (Chassis Box Girder Deck)
        X: −3          0          +3
    Z: +12 ■═══════════╤═══════════■  BOW BULKHEAD (r0 Datum)
           ║           │           ║
    Z:  +6 ║  BEAM TIE │  BEAM TIE ║  (Center Conveyor Raceway Channel)
           ║═══════════╪═══════════║
    Z:   0 ║  BEAM TIE │  BEAM TIE ║  ◄── Vehicle Center Midline Datum
           ║═══════════╪═══════════║
    Z:  −6 ║  BEAM TIE │  BEAM TIE ║
           ║           │           ║
    Z: −12 ■═══════════╧═══════════■  STERN BULKHEAD (r24 Datum)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 1 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 50  │ 150.0 t / 50  │ Palette Slot 0 (Slate); 2 rails × 25 B   │
│ Heavy Armor Slope   │ 24  │  36.0 t / 24  │ Wheel well arch outward reinforcement    │
│ Beam Block Straight │ 16  │  12.8 t / 16  │ Transverse cross-ties between girders    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 2 · 5×5 Off-Road Suspension Wheel Mounts & Master Steering Rig (L1)

Mount 5 pairs of 5×5 suspension wheel units outward along the chassis girders at X = ±4. Configure the 5-axle counter-steering geometry: front two axles steer with steering input; rear two axles counter-steer in the opposite direction; center axle 3 remains locked at 0° to prevent tire scrub.

```
Plan view, Y = L1 (Wheel Suspension Placement & Steering Limits)
        X: −5          0          +5
    Z: +10 [WH 1 (P)] ═══════════ [WH 2 (S)]   ◄── Axle 1 (Steering Angle: +30°)
    Z:  +5 [WH 3 (P)] ═══════════ [WH 4 (S)]   ◄── Axle 2 (Steering Angle: +15°)
    Z:   0 [WH 5 (P)] ═══════════ [WH 6 (S)]   ◄── Axle 3 (Neutral 0° · Fixed Drive)
    Z:  −5 [WH 7 (P)] ═══════════ [WH 8 (S)]   ◄── Axle 4 (Counter-Steer: −15°)
    Z: −10 [WH 9 (P)] ═══════════ [WH 10(S)]   ◄── Axle 5 (Counter-Steer: −30°)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 2 PARTS TRAY & MASTER 5-AXLE TUNING GUIDE                                       │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Axle Geometry & Tuning Settings          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Wheel 5×5 Susp (L)  │ 5   │ 40.0 t / 500  │ Port flank (X = −4), face outward (−X)   │
│ Wheel 5×5 Susp (R)  │ 5   │ 40.0 t / 500  │ Stbd flank (X = +4), face outward (+X)   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

##### Master 5-Axle Suspension Parameter Table

| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 1** | Z = +10 | `WH 1, 2` | `30.0°` | Normal | `100%` | `45% / 85%` | `65%` |
| **Axle 2** | Z = +5  | `WH 3, 4` | `15.0°` | Normal | `100%` | `45% / 85%` | `65%` |
| **Axle 3** | Z = 0   | `WH 5, 6` | `0.0°`  | OFF (Locked) | `100%` | `50% / 90%` | `70%` |
| **Axle 4** | Z = −5  | `WH 7, 8` | `15.0°` | INVERTED | `100%` | `45% / 85%` | `65%` |
| **Axle 5** | Z = −10 | `WH 9, 10`| `30.0°` | INVERTED | `100%` | `45% / 85%` | `65%` |

---

#### Step 3 · Underbody Skid Plates & Voxel Deflectors (L1)

Install heavy armor half-blocks along the belly centerline (X = −1 to +1) to shield the primary conveyor spine and prevent high-centering on rocky outcrops.

```
Cross-section, looking Bow (Z = 0)
    L2   [GIRDER P] ─── [CONVEYOR SPARK LINE] ─── [GIRDER S]
    L1   [WHEEL P] ─── [BLAST HALF][BLAST HALF] ─── [WHEEL S]  ◄── 1.25 m Ground Clearance
    L0   ═══════════════════════════════════════════════════  Terrain Surface
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 10.1A: BELLY CONVEYOR CONTINUITY & CLEARANCE**
> Before welding the lower blast skid plates:
> 1. Verify uninterrupted large conveyor pipe flow from Bow Throat (Z +12) to Stern Dump (Z −12).
> 2. Ensure exactly 1.25 m (0.5 B) ground clearance under belly skids at 50% suspension travel.
> 3. Verify all 10 suspension wheel hubs respond to propulsion and handbrake commands in group `MAS Wheels`.

---

##### Visual Milestone 10-1 · Lower Chassis & 10-Wheel Suspension Complete

```
Visual Checkpoint Silhouette (Looking Port Side at Ground Baseline):
       Z: −10 (Axle 5)   Z: −5 (Axle 4)   Z: 0 (Axle 3)   Z: +5 (Axle 2)   Z: +10 (Axle 1)
           [WH 9]           [WH 7]           [WH 5]          [WH 3]           [WH 1]
     ═══════╧════════════════╧════════════════╧═══════════════╧════════════════╧═══════
                 25 B CONTINUOUS HEAVY ARMOR BOX GIRDER RUN (L2)
```

* [ ] Turning steering wheel causes front and rear wheels to pivot symmetrically in opposite directions.
* [ ] Turning radius measures approximately 35 m on flat ground without wheel hop or sideways dragging.
* [ ] Belly skid plates completely enclose the central conveyor tube raceway.

---

### Phase 2 · Triple Large Cargo Hold & Central Spine (Levels L2-L4)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 16 L × 3 H B (17.5 × 40.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z −10 to +6, Y = L2 to L4
       Hold Positions:
       · Hold 1: Z +2 to +4, centered at X0 (Forward Cargo)
       · Hold 2: Z −2 to 0, centered at X0 (Mid Cargo)
       · Hold 3: Z −6 to −4, centered at X0 (Aft Cargo)
       · Power Core Vault: Z −10 to −7, X −2 to +2
```

#### Step 4 · Triple In-Line Large Cargo Hold (L2-L4)

Mount three standard 3×3×3 B Large Cargo Containers end-to-end along the vehicle centerline (X = −1 to +1). Join their forward and rear large conveyor ports directly with intermediate Conveyor Junction cubes.

```
Plan view, Y = L3 (Hold Mid-line Layout)
        X: −1    0   +1
    Z: +4  ┌───────────┐
           │  HOLD 1   │  ◄── Large Cargo Container 1 (Z +2 to +4)
    Z: +2  ├───────────┤      (Palette Slot 5: Logistics Emerald)
    Z:  0  │  HOLD 2   │  ◄── Large Cargo Container 2 (Z −2 to 0)
    Z: −2  ├───────────┤
    Z: −4  │  HOLD 3   │  ◄── Large Cargo Container 3 (Z −6 to −4)
    Z: −6  └───────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4 PARTS TRAY & CONVEYOR PORT ORIENTATION GUIDE                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Conveyor Port Orientation       │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo (3×3×3) │ 3   │ 85.5 t / 750  │ Palette Slot 5; large ports facing N/S/U │
│ Conveyor Junction   │ 6   │  7.8 t / 78   │ Tie junctions between containers & spine │
│ Heavy Armor Slopes  │ 36  │ 54.0 t / 36   │ Lateral sponson protection over holds    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 5 · Power Core Vault & Electrical Isolation (Z −10 to −7, L2-L4)

Install 2 Small Nuclear Reactors and 4 Large Batteries directly aft of Hold 3. Enclose them in double-walled heavy armor bulkheads to shield the power plant from kinetic collision and debris.

```
Plan view, Y = L3 (Power Core Vault)
        X: −2     0    +2
    Z: −8  [BAT 1][R 1][BAT 2]   ◄── 2× Small Reactors (15 MW each = 30 MW)
    Z: −9  [BAT 3][R 2][BAT 4]   ◄── 4× Large Batteries (3 MWh each = 12 MWh)
                                     (Palette Slot 6: High-Voltage Red)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 5 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Output & Operating Parameters            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Reactor       │ 2   │   3.2 t / 50  │ `MAS Reactor 1, 2` (30 MW total baseline)│
│ Large Battery       │ 4   │  20.0 t / 80  │ `MAS Battery 1..4` (12 MWh, 48 MW burst)†│
│ Heavy Armor Bulkhead│ 16  │  24.0 t / 16  │ Airtight shielding isolating power vault │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 10.2A: CARGO VOLUME & POWER SEAL CHECK**
> 1. Check inventory terminal: verify combined hold capacity reads exactly `4,158,000 L` (3× Inventory).
> 2. Ensure `MAS Reactor 1 & 2` have automated uranium pull enabled via the center conveyor trunk.
> 3. Verify total electrical output capacity reads `78.0 MW` peak with zero battery drain at idle.

---

##### Visual Milestone 10-2 · Cargo Spine & Power Core Vault Enclosed

```
Visual Checkpoint Silhouette (Looking West at Mid-Section Sponsons):
        L4  ┌─────────────────────────┬──────────────┐
            │ TRIPLE LARGE CARGO HOLD │ POWER VAULT  │
        L3  │   (HOLD 1 · 2 · 3)      │ [2×R] [4×BAT]│
        L2  ├─────────────────────────┴──────────────┤
            │ CONTINUOUS RUNNING BOARD & SIDE RUB RAIL
     ═══════╧════════════════════════════════════════╧═══════
                 10-WHEEL DRIVE POWERTRAIN ACTIVE (L1)
```

* [ ] Triple cargo hold passes conveyor pressure test from bow throat to power vault.
* [ ] Battery bank and reactor output register green status on diagnostic LCDs.
* [ ] Lateral heavy armor sponsons flush-mounted at X = ±3.

---

### Phase 3 · Twin Articulated "Tusk" Booms & CTC Rigging (Levels L2-L4)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 15 W × 10 L × 4 H B (37.5 × 25.0 × 10.0 m articulated reach)
       Coordinate Range: X −6 to +6, Z +12 to +18, Y = L2 to L5
       Shoulder Datums:
       · Port Shoulder Rotor: [X −3, Z +12, Y = L2]
       · Starboard Shoulder Rotor: [X +3, Z +12, Y = L2]
       · Bow Conveyor Throat: [X0, Z +12, Y = L2]
```

#### Step 6 · Tusk Shoulder Advanced Rotors (Z = +12, X = ±3, L2)

Mount two Advanced Rotors on the forward bow frame facing forward (+Z). These provide the horizontal pan axis (azimuth sweep) for the excavator tusks and feed ore directly into the bow manifold.

```
Plan view, Y = L2 (Bow Shoulder Mounts)
        X: −3          0          +3
    Z: +12 [ROTOR PORT] ═[CONV THROAT]═ [ROTOR STBD]  ◄── Advanced Rotors face +Z
           (Pan Axis)    (Intake Line)  (Pan Axis)        (Palette Slot 3: Ochre)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 6 PARTS TRAY & ROTOR CONFIGURATION                                              │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Advanced Rotor      │ 2   │ 18.0 t / 200  │ Share Inertia Tensor = ON · Torque: 40 MN│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings for `MAS Rotor Pan Port / Stbd`:**
* Share Inertia Tensor = `ON` (MANDATORY: prevents arm wobble and chassis rocking)
* `MAS Rotor Pan Port` Limits: Min = `−30.0°` (inward toward center), Max = `+60.0°` (outboard)
* `MAS Rotor Pan Stbd` Limits: Min = `−60.0°` (outboard), Max = `+30.0°` (inward toward center)
* Braking Torque: `40 MN·m`

---

#### Step 7 · Boom & Forearm Articulation Hinges (L2-L4)

Construct the articulated boom arms using motorized hinges for elevation pitch and wrist tilt.

```
2.5D Isometric Callout · Boom Articulation Kinematics (Per Arm)

     Bow Bulkhead (Z +12, X ±3)
            ║
            ╠══[ADVANCED ROTOR] (Pan: Horizontal Sweep −60° to +60°)
            ║        │
            ║        ▼
            ║   [Upper Boom Arm 4 B] (Heavy Armor Casing · Palette 3)
            ║        │
            ║        ▼
            ║   [MOTOR HINGE 1] (Pitch: Elevation −45° Dig to +60° Raise)
            ║        │
            ║        ▼
            ║   [Forearm Girder 3 B]
            ║        │
            ║        ▼
            ║   [MOTOR HINGE 2] (Wrist: Tool Tilt −45° to +45°)
            ║        │
            ║        ▼
            ║   [MINING HEAD] (Twin Laser Multitools)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 7 PARTS TRAY                                                                    │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings & Limits         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Motor Hinge (Large) │ 4   │ 16.0 t / 400  │ Share Inertia Tensor = ON · Torque: 45 MN│
│ Heavy Armor Block   │ 14  │ 42.0 t / 14   │ Palette Slot 3 (Ochre); arm shell casing │
│ Conveyor Tube (Lrg) │ 8   │  4.8 t / 48   │ Internal flexible conveyor feed line     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 8 · Twin Laser Mining Tool Heads (Head Assembly)

Mount 2 Simple Laser Multitools in mining mode, 1 camera, 1 spotlight, and 1 sensor on each wrist tip.

```
Front view, looking at Tool Head Face
        ┌───────────────────────────────────┐
        │ [LASER 1]   [CAM]   [LASER 2]     │  ◄── 2× Simple Laser Multitools
        │ [SPOTLIGHT]         [SENSOR 5 m]  │      (Mode: Mining Beam)
        └───────────────────────────────────┘
```

**Blocks per Head (×2 Heads):**
* 2 × Simple Laser Multitool · `MAS Laser Port 1, 2` / `MAS Laser Stbd 1, 2` (Mode: `Mining Beam`)
* 1 × Camera · `MAS Cam Tusk Port / Stbd` (aiming crosshairs)
* 1 × Spotlight · `MAS Light Tusk Port / Stbd` (high-intensity beam)
* 1 × Sensor · `MAS Sensor Tusk Port / Stbd` (range: 5 m forward terrain detection)

---

#### Step 9 · Custom Turret Controller (CTC) Mouse-Aim Rigging

Configure two Custom Turret Controller blocks on the bridge to allow instantaneous, smooth mouse aiming of each excavator arm directly from the operator stations.

```
Custom Turret Controller (CTC) Mapping Architecture:
   [OPERATOR DESK MOUSE] ──► [CTC BLOCK] ──► Azimuth Rotor: `MAS Rotor Pan`
                                         └──► Elevation Hinge: `MAS Hinge Pitch`
                                         └──► Tool Trigger: `MAS Lasers`
```

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 10.3A: TUSK BOOM MECHANICAL ALIGNMENT**
> 1. Set `Share Inertia Tensor = ON` across all 6 articulation joints (Rotors and Hinges).
> 2. Verify pitch range allows digging down to −45° (undercutting terrain 12.5 m below wheels) and raising to +60° (clearing high walls).
> 3. Verify laser multitool extraction feeds directly through internal hinge conveyor ports into Hold 1.

---

##### Visual Milestone 10-3 · Twin Articulated Tusks Rigged & Calibrated

```
Visual Checkpoint Silhouette (Top-Down View of Tusk Sweep Arc):
        X: −7 (Port Outboard)     X0 (Center)     X: +7 (Stbd Outboard)
                 \                                     /
                  \                                   /
                   [PORT TUSK BOOM]   [STBD TUSK BOOM]  ◄── 37.5 m Total Swath Width
                          \                   /
     ══════════════════════[BOW BULKHEAD (Z +12)]══════════════════════
```

* [ ] CTC mouse control provides silky, zero-recoil arm movement without chassis tilt.
* [ ] Both booms synchronized to dig a continuous 37.5 m wide, 12.5 m deep trench.
* [ ] Lasers extract raw ore and stone with 100% conveyor collection efficiency.

---

### Phase 4 · Control Bridge & Operating Deck (Levels L3-L5)

```
       [PHASE 4 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 6 L × 3 H B (17.5 × 15.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z +6 to +11, Y = L3 to L5
       Key Datums:
       · Helm Station: [X0, Z +9, Y = L4]
       · Tusk Operator Port: [X −2, Z +8, Y = L4]
       · Tusk Operator Starboard: [X +2, Z +8, Y = L4]
       · Deep Ore Detector: [X0, Z +6, Y = L3]
```

#### Step 10 · Forward Armored Bridge & Sensor Core (Z +6 to +11, L3-L5)

Construct the forward panoramic bridge cantilevering over the chassis girders. Level L3 houses the heavy ore detector and avionics; Level L4 accommodates the 3-seat operator flight bridge.

```
Plan view, Y = L4 (Bridge Operator Deck)
        X: −3   −1    0   +1   +3
    Z: +10 ┌─────┬───────────┬─────┐
           │     │  [HELM]   │     │  ◄── Master Driver / Helm Station
    Z: +8  │     ├───────────┤     │      (Palette Slot 7: Navy Blue)
           │CTC-P│ [COCKPIT] │CTC-S│  ◄── Dedicated Tusk Operator Consoles
    Z: +6  └─────┴───────────┴─────┘
                 │ [ORE DET] │        ◄── Large Grid Ore Detector (L3)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 10 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Role & Operating Configuration           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Cockpit (Armored)   │ 1   │   3.5 t / 15  │ `MAS Helm` (Master wheel steering & drive│
│ Control Station     │ 2   │   1.2 t / 30  │ `MAS Station Tusk Port`, `Stbd` (CTC aim)│
│ Ore Detector (Lrg)  │ 1   │   2.5 t / 25  │ `MAS Ore Detector` (150 m sub-surface)   │
│ Sloped Armor Glass  │ 14  │   2.8 t / 14  │ Forward 45° reinforced viewports         │
│ Radio Antenna (Lrg) │ 1   │   8.0 t / 100 │ `MAS Antenna` (Range: 25,000 m regional) │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!TIP]
> **PRE-ENCLOSURE INSPECTION GATE 10.4A: BRIDGE AIRTIGHTNESS & SENSOR REACH**
> 1. Seal bridge glass frame; confirm air vent pressurizes cabin to 100% O2.
> 2. Verify `MAS Ore Detector` is set to 150 m maximum scan range.
> 3. Verify hotkey bar in `MAS Helm` has toggles for: Handbrake, Group `MAS Wheels`, CTC Port Control, CTC Stbd Control.

---

### Phase 5 · Flight Decks & Air Extraction Interfaces (Levels L5-L6)

```
       [PHASE 5 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 20 L × 2 H B (17.5 × 50.0 × 5.0 m)
       Coordinate Range: X −3 to +3, Z −11 to +9, Y = L5 to L6
       Landing Pad Datums:
       · Kestrel Scout Pad Port: [X −3, Z +8, Y = L5]
       · Kestrel Scout Pad Starboard: [X +3, Z +8, Y = L5]
       · Mule Gantry Pad Center: [X0, Z −8, Y = L6]
```

#### Step 11 · Forward Kestrel Scout Drone Landing Pads (Z +7 to +9, L5)

Recess two small landing pads into the roof flanks forward of the cargo holds to host two KESTREL prospecting drones.

```
Plan view, Y = L5 (Forward Flight Roof)
        X: −3     0    +3
    Z: +9  [KESTREL 1] ··· [KESTREL 2]  ◄── Small-grid docking connectors
    Z: +7  (Port Pad)  ··· (Stbd Pad)       flush with deck (Palette Slot 8: White)
```

**Blocks:**
* 2 × Small Grid Connector (adapted via rotor head) · `MAS Kestrel Dock 1, 2`
* 2 × Sorter · `MAS Kestrel Feeder 1, 2` (pulls uranium and spare parts for scouts)

---

#### Step 12 · Aft Mule Receiving Gantry Cradle Pad (Z −11 to −5, L6)

Per **Decision 45**, install a standardized 7 × 7 B Heavy Armor blast deck on the roof, equipped with twin motorized cradle arms and magnetic clamping plates matching the Orca and Ground Base standards.

```
Plan view, Y = L6 (Aft Flight Deck)
        X: −3   −2   −1     0    +1   +2   +3
    Z: −5  ■─────────────────────────────────■
           │   [CRADLE P]       [CRADLE S]   │
           │   (Hinge Arm)      (Hinge Arm)  │
           │        │                │       │
    Z: −8  │        ▼     [MAS CON]  ▼       │   ◄── Center Belly Connector
           │        │     (Facing ↑) │       │        (Palette Slot 1: Amber)
           │   [MAG P]          [MAG S]      │
    Z: −11 ■─────────────────────────────────■
```

```
2.5D Isometric Callout · Mule Gantry Cradle on Mastodon Roof

                        [MULE-01 HULL 7×7 B]
                                 │
     Port Flank                  ▼               Starboard Flank
   [CLAMP RUB RAIL]                        [CLAMP RUB RAIL]
          ▲                                       ▲
          │                                       │
     [MAG PLATE]  ◄── 30° Swing Inward ──►  [MAG PLATE]
          │                                       │
     [MOTOR HINGE]                           [MOTOR HINGE]
     (Base: X −3, Z −8)                      (Base: X +3, Z −8)
   ═══════╧═══════════════════════════════════════╧═══════
                     FLUSH HEAVY BLAST ROOF (L6)
                            [MAS MP Conn]
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 49  │  41.6 t / 49  │ Palette Slot 1 (Amber); 7×7 B blast deck │
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `MAS MP Conn` (Facing UP at X0, Z−8)     │
│ Motor Hinge (Large) │ 2   │   8.0 t / 200 │ Share Inertia Tensor = ON · Limit: 30°   │
│ Magnetic Plate (Lrg)│ 2   │   4.8 t / 50  │ Auto-lock enabled; strength 100%         │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `MAS MP Sorter` (Drain All = ON)         │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 10.5A: CRADLE CLAMP RIGGING ON MOBILE DECK**
> 1. Set `Share Inertia Tensor = ON` on both `MAS MP Cradle Port` and `Stbd`.
> 2. Verify hinge limits: `0.0°` (retracted flush) to `30.0°` (inward clamp).
> 3. Verify `MAS MP Sorter` whitelist contains only rare ores (`Uranium`, `Platinum`, `Gold`, `Magnesium`).

---

### Phase 6 · Stern Bumper & Land Train Interface (Levels L1-L3)

```
       [PHASE 6 SPATIAL LOCATOR HUD]
       Envelope: 5 W × 3 L × 3 H B (12.5 × 7.5 × 7.5 m)
       Coordinate Range: X −2 to +2, Z −13 to −11, Y = L1 to L3
       Docking Datum:
       · Rear Dump Connector: [X0, Z −12, Y = L2] (Centerline height = 2.5 m above ground)
```

#### Step 13 · Rear Train Dump Port & Push Bumper (Z = −12, X = 0, L2)

Construct the reinforced rear collision bumper and high-speed dump connector. It allows the Caterpillar train's tractor locomotive to mate directly with the Mastodon's stern to offload bulk ore.

```
Rear elevation, looking Bow at Stern Bumper (Z = −12)
        X: −2     0    +2
    L3     ■═════╤═════■  ◄── Heavy Bumper Armor (Palette Slot 2: Crimson)
    L2     │ [MAS DUMP]│  ◄── Large Connector (Faces Stern −Z, Height = Level L2)
    L1     ■═════╧═════■
           ◄── 1.25 m Ground Clearance ──►
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13 PARTS TRAY                                                                   │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Placement                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `MAS Stern Dump Conn` (Facing Stern −Z)  │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `MAS Stern Sorter` (Drain All = ON)      │
│ Heavy Armor Slopes  │ 12  │  18.0 t / 12  │ Sloped collision deflection push-blocks  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 10.6A: TRAIN DUMP DISCHARGE FLOW**
> 1. Ensure `MAS Stern Dump Conn` centerline is mounted at exactly Level L2 (matches Caterpillar locomotive bow height).
> 2. Verify `MAS Stern Sorter` whitelist contains: `Iron`, `Nickel`, `Silicon`, `Cobalt`, and `Stone`.
> 3. Verify dump line draws from all 3 Large Cargo containers without bottleneck.

---

##### Visual Milestone 10-4 · Complete Mobile Excavator Commissioned

```
Visual Checkpoint Silhouette (Looking Starboard at Complete Unit):
                        [KESTREL PADS]       [MULE GANTRY PAD]
                               │                     │
                     [ARMORED BRIDGE L4]     [POWER VAULT L3]
                               │                     │
      [TUSK BOOMS L2]          │                     │          [STERN DUMP L2]
             \                 │                     │                 /
     ═════════╧════════════════╧═════════════════════╧═════════════════╧═════════
                 11 × 25 × 6 B HEAVY MOBILE MINING BEHEMOTH (MASTODON-01)
```

* [ ] Vehicle dry mass verified: `≈ 694.4 t` via terminal info tab.
* [ ] All 10 suspension wheels, 6 tusk articulation joints, and 2 roof clamp hinges functional.
* [ ] Both aerial flight decks (Kestrel/Mule) and rear land train dump port operational.

---

### Phase 7 · Master Control Logic & Hardware Scripting Contract (Claude Code)

#### Step 14 · Terminal Namespace & Group Taxonomy

##### 1. Group Taxonomy
* `MAS Wheels` : All 10× 5×5 heavy off-road suspension wheel units.
* `MAS Tusks` : 6× Hinge and Rotor articulation blocks across both boom arms.
* `MAS Lasers` : 4× Simple Laser Multitools in mining mode.
* `MAS Holds` : 3× Large Cargo Containers (`MAS Hold 1, 2, 3`).
* `MAS Reactors` : 2× Small Nuclear Reactors.
* `MAS Batteries` : 4× Large Batteries.
* `MAS MP Cradle` : 2× Motorized Hinges + 2× Magnetic Plates on the roof pad.

##### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MAS EC Holds` | `MAS Holds` Total Mass | Total Mass $\ge 2,500\text{ t}$ | Chime Alert · IGC `TMS.MAST.FULL` | Normal mining |
| `MAS EC MP` | `MAS MP Conn` | Connector: Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle 0° |
| `MAS EC Train` | `MAS Stern Dump Conn` | Connector: Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Speed` | Vehicle Speed | Speed $> 45\text{ km/h}$ | Wheel Strength $\rightarrow 80\%$ (Anti-roll) | Wheel Strength $\rightarrow 50\%$ |

##### 3. IGC Channel Subscriptions (Ground Fleet Bus)
* Outbound: `TMS.MAST.STATUS`: `mastodon_id;ore_mass_t;payload_pct;gps_coords;active_drones`
  * Example: `MAS-01;2150.4;86.0;GPS:DepositAlpha:...:;2`
* Outbound: `TMS.MULE.CALL`: `requester_id;pad_id;payload_type;urgency`
* Inbound: `TMS.CATER.ARRIVED`: signals land train is locked into stern bumper.

---

### Phase 8 · Commissioning & Field Acceptance Checklist (RIG Test)

#### A · Powertrain & Maneuvering Trials
* [ ] **Dry Mass Verification:** Confirm empty mass reads `≈ 694.4 t` in info tab.
* [ ] **5-Axle Counter-Steer Trial:** Drive forward at 30 km/h; verify Axles 1 & 2 steer, Axle 3 tracks straight, and Axles 4 & 5 counter-steer. Confirm zero lateral tire scrub.
* [ ] **Laden Grade Climb:** Fill holds with 2,500 t stone; verify all 10 wheels climb a 25° voxel incline at $> 25\text{ km/h}$ without power stalling.

#### B · Tusk Boom & Laser Excavation Trials
* [ ] **Kinematic Envelope Verification:** Articulate tusks through full 105° vertical pitch arc (−45° to +60°) and 90° azimuth sweep (−30° to +60°).
* [ ] **CTC Mouse-Aim Response:** Sit in bridge control seats; engage CTC; verify smooth, instantaneous mouse aiming of both arms.
* [ ] **37.5 m Swath Excavation:** Drive forward at 0.5 m/s while carving with all 4 laser beams; confirm a clean 37.5 m wide by 12.5 m deep trench is cleared with zero un-mined pillars.

#### C · Dual Logistics Offload Acceptance
* [ ] **Roof Mule Landing:** Land MULE-01 on roof deck; verify cradle arms swing 30° inward and clamp rub rails; verify rare ores drain into Mule hold.
* [ ] **Stern Train Fast-Dump:** Mate Caterpillar tractor into rear bumper connector; verify `MAS Stern Sorter` discharges 2,500 t bulk ore in $< 90\text{ seconds}$.


---

## Chapter 11 · CATERPILLAR-01 Land Train (CAT-01)

Source: `docs/rig/caterpillar-instructions.md` · 42 B Length · Articulated Drawbar Hitch & Modular Hoppers

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
