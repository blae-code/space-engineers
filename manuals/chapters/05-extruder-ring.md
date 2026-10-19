# Rig Linear Extruder Ring Shipyard · Visual Build Instructions

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
