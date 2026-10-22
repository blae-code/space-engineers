# Albatross · Visual Build Instructions

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
