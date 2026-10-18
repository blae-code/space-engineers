# Orca · Visual Build Instructions

Lego-style, step-by-step build manual for ORCA-01.
Each step: one ASCII diagram, a block list, a checkpoint.

Source spec: Blae Decisions 1-54 (Canon) · `lego-instructions-v1.0.md` Chapter 1

Last updated: 2026-10-17

---

## 0 · Reference grid

The Orca sits on a **17 × 19 × 7 B** bounding box (42.5 × 47.5 × 17.5 m).

```
Axes:  X = beam (port −, stbd +)    Y = up (L1 belly, L7 roof)    Z = length (bow +)

Rows:   r0 (bow) ─────────────────────────────────── r18 (stern)
Levels: L1 (belly) ─── L2 ─── L3 ─── L4 ─── L5 ─── L6 ─── L7 (roof)
X:      −8 (port tip) ──── −1.5 (port spine wall) ── 0 ── +1.5 (stbd spine wall) ──── +8 (stbd tip)
```

### Master plan view (roof, Y = L7)

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

### Master side elevation (X = 0, looking from stbd)

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

### Master cross-section (at r9, looking toward bow)

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


### 0.4 · Master Conveyor Routing Schematic

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

### 0.5 · Master Electrical Single-Line Schematic

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

### 0.6 · Master Pre-Flight Bill of Materials (BOM)

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


### 0.7 · Human Builder Guide · Visual Paint Palette & Spatial HUD

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

#### Universal Spatial Locator HUD (Included in Every Step)
Every build step begins with a standardized spatial banner:
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: <Name>  ·  Grid Zone: <Rows r0-r18>  ·  Level: <L1-L7>         │
│ [VIEWPORT] Looking: <Direction>  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

## Phase 1 · Spine and roof deck

### Step 1 · Belly frame rails (L1)

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

### Step 2 · Spine posts (L1 → L7)

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

### Step 3 · Spine roof beams (L7)

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

### Step 4 · Cross-frames at r0, r6, r12, r18 (L1 and L7)

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

### Step 5 · Outer frame rails (L1 → L7 at X = ±8)

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

### Step 6 · Inner frame rails (X = ±2, L1 → L7)

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

### Step 7 · Bay mid-height rings (L4, at each bay)

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

### Step 8 · Roof deck (L7, Structural Frame 3×1×3)

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

### Step 9 · Ore trunk (L7, centre line)

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

### Step 10 · Power core (PC, r3-r6)

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

### Step 11 · Ore hold (HD, r7-r15)

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

### Step 12 · Aft service section (AS, r16-r18)

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

### Step 13 · Mule pad (MP, roof, r8-r14) & Gantry Cradle

### 2.5D Isometric Callout · Pad-Mounted Gantry Cradle (r8-r14, X ±3)

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

### AI Scripting & Hardware Contract Card · Mule Pad

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

### Step 14 · Solar array (roof)

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

## Phase 1 checkpoint

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

## Phase 2 · Control core (CC)

### Step 15 · CC heavy armour shell (r0-r2)

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

### Step 16 · CC control blocks

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

### Step 17 · Antenna mast

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

## Phase 2 checkpoint

- [ ] CC armoured shell complete
- [ ] All 10 control blocks placed, named, in correct cells
- [ ] Both RCs face +Z with `HOME` waypoint
- [ ] Antenna mast through roof, broadcasting
- [ ] Bow and mast cameras work

---

## Phase 3 · Bay rows (BR-P, then mirror to BR-S)

### Step 18 · Bay ceiling connectors (port side)

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

### Step 19 · Central bay repair turret & L4 gallery pipe (port side)

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

### Step 19b · Folding Mechanical Guide Jaws (port side)

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

### Step 20 · Bay sensors and lights (port side)

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

### Step 21 · Mirror to starboard (BR-S)

Repeat Steps 18-20 for bays S1, S2, S3 at X = +5 (connectors), with 1 central repair turret tap at S2 (X +2, r9·L4), mirrored guide jaws (hinges at X +2 and X +8), and sensor/light corners.

**✓ Check:** all 6 bays have: ceiling connector, twin motorized guide jaws, sensor, spotlight, camera. Central bays P2 and S2 each host 1 Laser Multitool repair turret.
- Total in `ORC Bay Conns` = 6
- Total in `ORC Repair` = 2 (P2 and S2 at r9·L4)
- Total in `ORC Bay Jaws` = 12 hinges
- Total in `ORC Bay Jaw Clamps` = 12 magnetic plates
- Total in `ORC Bay Lights` = 6

---

## Phase 3 checkpoint

- [ ] 6 bay connectors (facing down), all on ore trunk branches
- [ ] 2 central repair turrets (P2 and S2 at r9, L4, plumbed to L4 service gallery)
- [ ] 12 motorized guide jaws (twin hinges per bay @ L5 with magnetic clamp tips)
- [ ] 6 bay sensors + 6 spotlights (sensor switches its own light)
- [ ] 6 bay cameras
- [ ] All named per §1 and in correct groups

---

## Phase 4 · Outriggers (OR)

### Step 22 · Corner atmo arms (OR-1P, OR-1S, OR-3P, OR-3S)

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

### Step 23 · Mid H2 arms (OR-2P, OR-2S)

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

## Phase 4 checkpoint

- [ ] 6 outrigger arms built (4 corner + 2 mid)
- [ ] `ORC Lift`: 4 large atmo, all facing down
- [ ] `ORC H2 Assist`: 2 large H2, facing down, OFF
- [ ] `ORC Gear`: 4 landing gear on corner arms
- [ ] H2 line connected AS → OR-2P and OR-2S
- [ ] No exhaust hits bay lanes, MP zone, or other arms
- [ ] Diagonal beam bracing on all arms

---

## Phase 5 · Lateral thrust and gyros

### Step 24 · Lateral thrusters

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

## Phase 6 · Instrumentation (cameras, sensors, lights)

### Step 25 · Remaining cameras and sensors

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

## Phase 7 · Event controllers and timers

### Step 26 · Wire the EC/TMR ladder

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

## Phase 8 · Commissioning

### Step 27 · Static tests

- [ ] Run `tools/bp-inspect.py "ORCA-01"` · record mass
- [ ] Every group from §1 exists and is complete
- [ ] Solar array mass ≤ 20 t? If not, strip it

### Step 28 · Hover tests

- [ ] Empty hover · record thrust %
- [ ] 90° yaw and pitch hold · add gyro pairs until acceptable
- [ ] Each lateral group alone · correct direction, no flame contact
- [ ] Each lift thruster with a test block under every lane · no damage

### Step 29 · Load tests

- [ ] Hold at cap, drones docked · thrust ≈ 77%
- [ ] Past cap · EC Lift turns H2 on; unload → off

### Step 30 · Docking tests

- [ ] Small-grid Connector locks to bay connector†
- [ ] Each bay: drone docks, ore passes Gate, drone recharges
- [ ] Hold at cap: Gate closes
- [ ] MP test: Drain sends ore only; Intake takes U + components only; both stop on release
- [ ] Ore never reaches U Store or Parts Store; uranium never reaches Hold
- [ ] Repair turret fixes a damaged docked drone block, touches nothing else
- [ ] Turret reaches every face of a docked drone (add outer turret if not)

### Step 31 · Safety tests

- [ ] PB off → dampeners hold → HOME after delay
- [ ] Reactors off at hover → batteries Auto, H2 on → controlled descent
- [ ] RC Spare as main controller → HOME works

### Step 32 · Full loop test

- [ ] One unattended loop: mine → offload → ferry → refine → return

### Commissioning record

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

## Block summary

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

