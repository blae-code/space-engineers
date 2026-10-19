# Kestrel · Visual Build Instructions

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
