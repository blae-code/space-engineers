# Chapter 2 · REMORA-01 Vertical Shaft Driller (REM-01)
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
