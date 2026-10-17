# Remora · Visual Build Instructions

Lego-style, step-by-step build manual for REMORA-01.
Each step: one ASCII diagram, a block list, and a verification check.

Source specs:
* `Part 1 · Build Manual.pdf` (Remora Small-Grid Vertical Miner)
* `Part 2 · Scripting & Claude Code.md` (Orca Dispatcher & Remora PB)
* Canon Decisions: 25 (Earth 1 g), 31 (4 Remoras), 32 (Retain Stone), 36-37 (Guide Jaws & Mag Clamps), 41 (3× Inventory), 42 (1.0 s Beat / 5.0 s Timeout)

Last updated: 2026-10-17

---

## 0 · Reference Grid & Envelope

The Remora is a small-grid vertical mining drone built on an **11 × 11 sb** footprint (5.5 × 5.5 m) and a height of **22 sb** (11.0 m) from drill tips to crown connector.

```
Axes:
  X = Beam:     Port (−5 sb) ◄── 0 (Core Axis) ──► Starboard (+5 sb)
  Z = Length:   Aft  (−5 sb) ◄── 0 (Core Axis) ──► Forward   (+5 sb)
  Y = Height:   Down (L0 drill tips) ────────────► Up        (L22 crown connector)

Scale: 1 sb (small block) = 0.5 m
```

### Master Section Stack (L0 → L22)

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

### Master Elevation (Looking Starboard, X = 0)

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


### 0.4 · Master Conveyor Routing Schematic

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

### 0.5 · Master Electrical Single-Line Schematic

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

### 0.6 · Master Pre-Flight Bill of Materials (BOM)

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


### 0.7 · Human Builder Guide · Visual Paint Palette & Small-Grid HUD

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

#### Small-Grid Coordinate HUD
Every step includes an exact small-block coordinate banner:
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Section: <Name>  ·  Footprint: <sb −5 to +5>  ·  Level: <L0-L22>           │
│ [VIEWPORT] Looking: <Direction>  ·  ▲ FWD (+Z) · ◄ PORT (−X) · STBD (+X)► · ▼ AFT (−Z)│
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

### 0.8 · Gravity Scaffolding & Hanging Jig Setup

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

## Phase 1 · Core Spine & Collector (CL, L6-L20)

Build crown-up, hanging from a temporary test connector or shipyard head. The central vertical conveyor tube on axis (0, 0) carries ore from the drill head to the crown.

### Step 1 · Jig & Crown Axis Mount (L20-L22)

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

### Step 2 · Vertical Conveyor Spine (L6-L20)

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

### Step 3 · Collector Manifold & Stone Retention (L6-L10)

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


### Phase 1 Visual Milestone · What Your Grid Looks Like Now

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

## Phase 2 · Drill Head (DH, L0-L6)

Eight drills arranged in a perimeter ring around a 3×3 sb central core. Nose down.

### Step 4 · Drill Head Core & Sensor Eye (L0-L6)

### 2.5D Isometric Callout · Drill Head Core & Sensor Eye (L0-L6)

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

### Step 5 · 8-Drill Perimeter Ring (L0-L6)

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


### Phase 2 Visual Milestone · What Your Grid Looks Like Now

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

## Phase 3 · Lift, Lateral Thrust & Waist Clamping Collar (LF, L10-L15)

Lift thrusters sit at levels L10-L15. A **4 sb flame gap** exists between thruster nozzles (L10) and drill tops (L6) to prevent thermal self-damage.


> [!CAUTION] PRE-ENCLOSURE GATE 2 · 4 sb FLAME GAP PHYSICAL MEASUREMENT
> Before welding outer armor cowlings around the lift pods in Step 6:
> 1. Count the small blocks between the bottom nozzle rim (Level L10) and the top flat 
>    surface of the drill casings (Level L6).
> 2. There MUST be exactly 4 small blocks (2.0 m) of completely open air!
> 3. If the distance is 3 sb or less, thruster flame damage will destroy the drills on liftoff!

### Step 6 · Corner Large Atmo Lift Pods (L10-L15)

### 2.5D Isometric Callout · Lift Thruster Pods & 4 sb Flame Gap

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

### Step 7 · Internal Gyroscope Cluster (L11-L14)

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

### Step 8 · 4-Face Lateral Thruster Array (L11-L14)

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

### Step 9 · Waist Magnetic Clamp Collar (L11-L14)

### 2.5D Isometric Callout · Waist Magnetic Clamping Collar (L11-L14)

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


### Phase 3 Visual Milestone · What Your Grid Looks Like Now

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

## Phase 4 · Power Plant (PW, L15-L17)

All drone operations are pure electric. Batteries size to one full drilling sortie + 50% safety margin.

### Step 10 · Twin Large Battery Bank (L15-L17)

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

## Phase 5 · Avionics Suite (AV, L17-L20)

Automated navigation, IGC comms, and native safety fallbacks.

### Step 11 · Flight Control Core (L17-L19)

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

### Step 12 · Safety Timers & Event Controllers (L18-L20)

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

## Phase 6 · Docking Crown & Fairing (CR, L20-L22)

The top of the drone mates with the ceiling connector of an Orca bay.

### Step 13 · Docking Head Finalization (L20-L22)

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

## Phase 7 · Control Logic & Failover Matrix


### Step 12.1 · Master AI Scripting Contract & Terminal Namespace (`Drone.Miner`)

Every automated block on REMORA-01 conforms to this exact naming and grouping dictionary for Claude Code and `Drone.Miner`:

#### 1. Group Taxonomy
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

#### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `REM EC Batt` | `REM Batteries` Stored % | Stored $\le 25\%$ | `REM Drills` OFF · Trigger `REM TMR Return` | · |
| `REM EC Lift` | `REM Lift` Thrust % | Thrust $\ge 90\%$ (Overload) | `REM Drills` OFF · Trigger `REM TMR Return` | · |
| `REM EC Dock` | `REM Dock` State | Connector Locked | `REM Drills` OFF · `REM Batteries` $
ightarrow$ Recharge | `REM Batteries` $
ightarrow$ Auto |

#### 3. Native Failover Cascades (Zero-PB Safety)
If `REM PB` crashes or loses execution:
1. `REM TMR Heartbeat` (delay 5.0 s per Decision 42) expires without PB pulse.
2. Immediate actions: `REM Drills` $
ightarrow$ OFF · Thrust overrides $
ightarrow$ OFF · Dampeners $
ightarrow$ ON · Trigger `REM TMR Return`.
3. `REM TMR Return` (delay 2.0 s) trips: `REM RC` Autopilot engages $
ightarrow$ ascends vertically to waypoint `PREDOCK` (3 m below Orca bay).
4. The Orca recovers the drone from `PREDOCK` via bay approach sensor and folding guide jaws.

#### 4. IGC Broadcast Channels
* Outbound: `TMS.REMO.STATUS`: `drone_id;battery_pct;cargo_pct;hole_depth_m;status_enum`
  * Example: `REM-01;82.4;61.0;18.5;DRILLING`
* Inbound: `FLT.DISP.LEASE`: `drone_id;hole_gps;bay_id;return_deadline_s`

### Event Controller & Timer Mapping

| Block | Condition / Trigger | Action True | Action False |
|---|---|---|---|
| **`REM TMR Heartbeat`** | 5.0 s delay (restarted every 1.0 s by PB via IGC) | Drills OFF · Gyro/Thrust Overrides OFF · Dampeners ON · Trigger `REM TMR Return` | · |
| **`REM TMR Return`** | 2.0 s delay | `REM RC` Autopilot ON → Waypoint `PREDOCK` (3 m below bay) | · |
| **`REM EC Batt`** | `REM Batteries` ≤ 25% | Drills OFF · Trigger `REM TMR Return` | · |
| **`REM EC Lift`** | `REM Lift` thrust ≥ 90% (Ore Cap overload) | Drills OFF · Trigger `REM TMR Return` | · |
| **`REM EC Dock`** | `REM Dock` Locked | Drills OFF · `REM Batteries` → Recharge · IGC notify Orca | `REM Batteries` → Auto |

---

## Phase 8 · Commissioning Checklist (RIG Test Creative)

Execute in sequence before loading scripts:

### A · Static Inspection
* [ ] Dry mass verified: `≤ 50 t`† via terminal / info tab.
* [ ] All groups verified: `REM Lift`, `REM Lat F/A/P/S`, `REM Drills`, `REM Batteries`, `REM Gyros`.
* [ ] Conveyor integrity: ore dumped into drills passes smoothly up to `REM Dock`.

### B · Hover & Thrust Margin
* [ ] Empty hover: `REM Lift` thrust load ≈ 21% (Earth 1 g).
* [ ] Max laden test (195.7 t mass): `REM Lift` thrust load ≈ 83% (T/W 1.2 achieved).
* [ ] Flame clearance check: fire `REM Lift` at 100% override for 10 s; verify drill tops take 0 damage.

### C · Boring Test
* [ ] Descend 20 m straight down on drills: body follows borehole cleanly without snagging walls.
* [ ] Stone handling check: stone accumulates in hold/drills; no ejectors dump gravel.

### D · Orca Bay Docking
* [ ] Approach Orca bay from below to `PREDOCK` (3 m below L1 belly).
* [ ] Orca L5 guide jaws swing inward and Magnetic Plates clamp waist collar.
* [ ] Vertical ascent into L7 ceiling connector: `REM Dock` locks cleanly.
* [ ] Batteries automatically switch to `Recharge`; Orca repair turret reaches hull.

### E · Safety Failover
* [ ] Kill `REM PB` mid-drill: verify heartbeat timer expires at 5.0 s, drills abort, dampeners catch, and drone rises to `PREDOCK`.

---

## Phase 9 · Verify Items (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V34:** Drill cutting radius vs 11 sb hull width (ensure 8-drill ring cuts clear borehole without hull snagging).
* **V35:** Large Atmo (small grid) flame reach: verify 4 sb gap provides 100% thermal clearance.
* **V36:** Small-grid connector lock to Large-Grid connector (verify port connection geometry).

