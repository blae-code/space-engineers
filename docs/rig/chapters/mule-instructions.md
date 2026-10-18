# Mule · Visual Build Instructions

Lego-style, step-by-step build manual for MULE-01.
Each step: one ASCII diagram, a block list, and a verification check.

Source specs:
* Master Architecture: `docs/rig/lego-instructions-v1.0.md` Chapter 3
* Canon Decisions: 25 (Earth 1 g), 34 (Flush MP blast deck), 41 (3× Inventory), 42 (1.0 s Beat / 5.0 s Timeout), 43 (7×7 B Hull, 1 Large Cargo, 4 Flat Atmo, 650 t payload), 44 (Pure Electric, 4 Large Batteries), 45 (Gantry Cradle clamps)

Last updated: 2026-10-17

---

## 0 · Reference Grid & Envelope

The Mule is a heavy large-grid freight shuttle built on a **7 × 7 B** square footprint (17.5 × 17.5 m) and a height of **3 B** (7.5 m) from belly skid to roofline.

```
Axes:
  X = Beam:     Port (−3 B) ◄── 0 (Hold Center) ──► Starboard (+3 B)
  Z = Length:   Aft  (−3 B) ◄── 0 (Hold Center) ──► Bow / Fwd (+3 B)
  Y = Height:   L1 (Belly & Docking) ──► L2 (Midline) ──► L3 (Roof Deck)

Scale: 1 B (large block) = 2.5 m
```

### Master Plan View (Mid-deck, Y = L2)

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

### Master Elevation (Looking Starboard, X = 0)

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


### 0.4 · Master Conveyor Routing Schematic

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

### 0.5 · Master Electrical Single-Line Schematic

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

### 0.6 · Master Pre-Flight Bill of Materials (BOM)

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


### 0.7 · Human Builder Guide · Visual Paint Palette & CoM Balance HUD

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

#### CoM Balance HUD & Quadrant Alignment
Every assembly step includes an exact quadrant symmetry check:
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: <Name>  ·  Footprint: <X −3..+3, Z −3..+3>  ·  Level: <L1-L3>   │
│ [BALANCE] Target CoM: (X: 0.00, Z: 0.00)  ·  4-Quadrant Symmetrical Check: REQUIRED │
└──────────────────────────────────────────────────────────────────────────────────────┘
```

---

### 0.8 · Gantry Cradle Docking Shadow & Pad Alignment Guide

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

## Phase 1 · Central Ore Hold & Belly Dock (L1-L3)

The payload hold forms the structural and geometric center of the ship.

### Step 1 · Large Cargo Container & Belly Port (L1-L3)

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

### Step 2 · Belly Landing Skids & Perimeter Frame (L1)

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


### Phase 1 Visual Milestone · What Your Grid Looks Like Now

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

## Phase 2 · Symmetrical Lift Propulsion (L1-L2)

Four Large Flat Atmospheric Thrusters mounted in a cross pattern around the hold. Center of Lift sits precisely over (0, 0), eliminating all pitch/roll torque regardless of cargo load.

### Step 3 · Cardinal Lift Array (L1-L2)

### 2.5D Isometric Callout · Symmetrical Cardinal Lift Cross (L1-L2)

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


### Phase 2 Visual Milestone · What Your Grid Looks Like Now

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

## Phase 3 · Power Core & Gyros (L1-L3)

Four corner pockets (2 × 2 B each) house the batteries and flight stabilization gyroscopes.

### Step 4 · Corner Battery Array (L1-L3)

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

### Step 5 · Flight Stabilization Gyroscopes (L2)

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

## Phase 4 · Maneuvering Array & Downward Thrust (L2-L3)

Install small atmospheric thrusters on all 4 faces and the roof to provide complete 6-axis translation and braking control (addressing legacy sprue finding F1).

### Step 6 · 6-Axis Small Atmo Array (L2-L3)

### 2.5D Isometric Callout · 6-Axis Braking Array & Push-Down Thrusters

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


### Phase 4 Visual Milestone · What Your Grid Looks Like Now

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

## Phase 5 · Waist Clamp Rub Rails (L2)

Per **Decision 45**, the Mule carries zero mechanical docking arms. Instead, the landing pads (Orca MP and Ground Base) feature motorized Gantry Cradles with Magnetic Plates that clamp the Mule's chassis on touchdown.

### Step 7 · Heavy Armor Clamp Rub Rails (L2)

### 2.5D Isometric Callout · Waist Clamping Strike Rails (L2, X ±3)

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

## Phase 6 · Avionics, Navigation & Comms (L3)

The brain of the Mule sits sheltered on the upper deck at L3.

### Step 8 · Autonomous Avionics Core (L3)

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

## Phase 7 · Control Logic & Hardware Failover

### Step 9 · Safety Timers & Logic Mapping

```
Logic Block Array (Aft Bay, L3):
  [MUL TMR Heartbeat] · [MUL TMR Return] · [MUL EC Batt] · [MUL EC Cargo] · [MUL EC Dock]
```

**Blocks:**
* 2 × Timer Block: `MUL TMR Heartbeat` (5.0 s), `MUL TMR Return` (2.0 s)
* 3 × Event Controller: `MUL EC Batt` (monitors battery), `MUL EC Cargo` (monitors payload mass), `MUL EC Dock` (monitors belly connector)


### Step 9.1 · Master AI Scripting Contract & Terminal Namespace (Claude Code)

Every automated device on MULE-01 conforms to this exact naming and grouping dictionary:

#### 1. Group Taxonomy
* `MUL Lift` : 4× Large Flat Atmospheric Thrusters (`MUL Lift Fwd`, `Aft`, `Port`, `Stbd`). Total thrust = 10.4 MN.
* `MUL Maneuver` : 10× Small Atmospheric Thrusters (All horizontal faces + roof push-down).
* `MUL Batteries` : 4× Large Batteries (`MUL Battery NW`, `NE`, `SW`, `SE`). Total capacity = 12 MWh.
* `MUL Gyros` : 2× Large Gyroscopes (`MUL Gyro Port`, `MUL Gyro Stbd`). Total torque = 67.2 MN·m.
* `MUL Cams` : 2× Cameras (`MUL Cam Belly` at L1, `MUL Cam Fwd` at Bow L3).

#### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MUL EC Cargo` | Grid Total Mass | Mass $\ge 815	ext{ t}$ (Ore Cap 650 t) | Notify Orca via IGC · Start departure sequence | · |
| `MUL EC Batt` | `MUL Batteries` Stored % | Stored $\le 20\%$ | Abort transit · Engage emergency landing | · |
| `MUL EC Dock` | `MUL Dock Belly` State | Connector Locked | `MUL Lift` OFF · `MUL Batteries` $
ightarrow$ Recharge | `MUL Lift` ON · `MUL Batteries` $
ightarrow$ Auto |

#### 3. Native Failover Cascades (Zero-PB Safety)
If `MUL PB` crashes or loses IGC heartbeat:
1. `MUL TMR Heartbeat` (delay 5.0 s per Decision 42) expires without PB pulse.
2. Immediate actions: `MUL Maneuver` overrides OFF · Dampeners ON · Autopilot altitude hold at current position.
3. If grid is docked at Gantry Cradle: `MUL Lift` remains locked OFF to prevent thruster tear-away against magnetic clamps.

#### 4. IGC Broadcast Channels
* Outbound: `TMS.MULE.STATUS`: `mule_id;state_enum;ore_mass_t;battery_pct;destination_gps`
  * Example: `MUL-01;LADEN_TRANSIT;648.5;92.0;GPS:GroundBase:...:;`
* Inbound: `TMS.MULE.CALL`: `requester_id;pad_id;payload_type;urgency`

### Hardware Automation Matrix

| Block | Condition / Event | Action True | Action False |
|---|---|---|---|
| **`MUL TMR Heartbeat`** | 5.0 s timeout (IGC beat from Orca/Base) | Gyro/Thrust Overrides OFF · Dampeners ON · Autopilot hold | · |
| **`MUL EC Cargo`** | Total Grid Mass ≥ 815 t (Ore Cap 650 t) | Notify Orca via IGC · Trigger departure sequence | · |
| **`MUL EC Batt`** | `MUL Batteries` ≤ 20% | Abort transit · Engage emergency landing profile | · |
| **`MUL EC Dock`** | `MUL Dock Belly` Locked | `MUL Lift` OFF · `MUL Batteries` → Recharge | `MUL Lift` ON · `MUL Batteries` → Auto |

---

## Phase 8 · Commissioning Checklist (RIG Test Creative)

### A · Mass & Lift Calibration
* [ ] Dry mass verification: terminal reports `≈ 130-150 t`.
* [ ] Empty hover: `MUL Lift` load ≈ 12-14% (Earth 1 g).
* [ ] Max laden test: spawn 650 t stone/iron into `MUL Cargo Hold` (Total mass ≈ 800 t).
* [ ] Laden hover: `MUL Lift` load ≈ 75-78% (T/W ≥ 1.3 confirmed).
* [ ] Zero-torque test: release dampeners in hover; verify pitch and roll remain 0.00° (CoM aligns perfectly with lift center).

### B · Gantry Cradle & Docking Test
* [ ] Touchdown on Orca MP (roof deck): verify belly connector engages `ORC MP Conn` cleanly.
* [ ] Gantry Cradle engagement: trigger cradle hinge swing (30° inward); verify magnetic plates lock solidly to Mule waist rub rails.
* [ ] Lift cut test: turn `MUL Lift` OFF; verify Mule rests rigidly on cradle without shaking or drift.
* [ ] Ore transfer test: drain 650 t ore from Orca hold into `MUL Cargo Hold`; verify transfer completes in < 45 seconds†.

### C · Base Terminal Transit
* [ ] Release sequence: `MUL Lift` ON $\rightarrow$ Gantry Cradle clamps release $\rightarrow$ hinges fold flat $\rightarrow$ Mule ascends vertically to 50 m altitude.
* [ ] Fly 2 km transit corridor to Ground Base at 100 m/s: verify lateral thrusters hold zero drift.
* [ ] Ground Base Gantry Cradle touchdown: auto-clamp, connector lock, batteries switch to `Recharge`, ore drains into refinery intake.

---

## Phase 9 · Verify Items (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V37:** Large Flat Atmospheric Thruster large-grid sea-level thrust (verify 2.6 MN rating and power consumption).
* **V38:** Ore transfer drain rate between large-grid containers through a single connector (measure seconds per 650 t).

