# MULE-01 Heavy Freight Shuttle · Visual Build Instructions

## Chapter 3 · MULE-01 Heavy Freight Shuttle

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MULE-01 Heavy Freight Shuttle (MUL-01)**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 25, 34, 41, 42, 43, 44, 45, 51, 56, 62, 66 & 68)
* **Role:** Autonomous high-capacity bulk ore shuttle and rapid peer-resupply transport. Transports 650 t of raw ore (or specialized mission resupply kits) on high-speed point-to-point transit corridors between remote extraction sites (Orca Carrier MP, Mastodon MKI Consist) and the regional Ground Base & Refinery Complex.
* **Architecture:** Compact 7 × 7 B (17.5 × 17.5 m) square lifting slab with a 3 B (7.5 m) profile:
  1. **Core Payload Bay (X −1 to +1, Z −1 to +1, L1-L3):** Standard 3×3×3 B Large Cargo Container providing 1,386,000 L volume (at 3× inventory) with a direct, zero-sorter downward docking connector (`MUL Dock Belly`) at L1.
  2. **Cardinal Lift Cross (N, S, E, W):** 4× Large Flat Atmospheric Thrusters mounted flush around the hold perimeter, aligning Center of Lift perfectly with the geometric Center of Mass (CoM).
  3. **Corner Power & Avionics Pockets:** 4× Large Batteries (12 MWh pure electric), dual large gyroscopes, autonomous remote control core, and failover safety timers.
  4. **Waist Clamping Strike Rails (X ±3, L2):** 3-block continuous flush heavy armor rub rails designed for automated capture by motorized Gantry Cradles on carrier and base decks.
* **Grid:** Large Grid (1 B = 2.5 m) · **Footprint:** 7 W × 7 L × 3 H B (17.5 × 17.5 × 7.5 m).
* **Mass:** Dry Mass `≈ 142.8 t` · Maximum Payload Ore `650.0 t` · Gross Laden Mass `≈ 792.8 t`.
* **Propulsion:** Pure Electric Atmospheric (10.4 MN Downward Lift + 6.48 MN 6-Axis Maneuvering Array).

> [!NOTE]
> Dimensions and placements are provisional design intent, not a validated blueprint. The 7 × 7 × 3 B envelope is used for the model. Mass and BOM figures remain unresolved: this directive gives 142.8 t dry, a later summary gives 157.0 t, and the Studio registry reports 128.0 t with 142 blocks. Verify against a trusted blueprint or in-game build before use.

---

## 0 · Reference Grid, Coordinate System & Design Envelope

### 0.1 · Spatial Datum & Alignment Triad
All coordinates throughout this chapter reference the geometric centroid of the central Large Cargo Container:
```
           +Y (Dorsal / Roof Deck, L3)
           ▲
           │   +Z (Bow / Forward Flight)
           │  ▲
           │ /
           │/
───────────┼───────────► +X (Starboard Wing / Rail)
          /│
         / │
        /  │
       ▼   ▼
     −Z    −Y (Ventral / Belly Docking, L1)
   (Stern)
```
* **Origin [0, 0, 0]:** Centroid of `MUL Cargo Hold` (Large Cargo Container) at Level L2.
* **X-Axis (Beam):** Port (−3 B / −7.5 m) to Starboard (+3 B / +7.5 m). Total beam = 7 B (17.5 m).
* **Y-Axis (Height):** Ventral skid bottom (Level L1 / −3.75 m) to dorsal catwalk (Level L3 / +3.75 m). Total height = 3 B (7.5 m).
* **Z-Axis (Length):** Stern (−3 B / −7.5 m) to Bow (+3 B / +7.5 m). Total length = 7 B (17.5 m).
* **Center of Mass (CoM):**
  * Dry CoM: `[X: 0.00, Y: 0.00, Z: 0.00]` (Dead center of cargo hold).
  * Laden CoM (650 t ore): `[X: 0.00, Y: 0.00, Z: 0.00]`. Zero pitch/roll tip-torque under all loading conditions.

---

### 0.2A · Space Engineers 10-Slot Paint Palette Assignments
Standardized finish and color assignments for MULE-01 fabrication:

| Slot # | Color Name | Hex Code | RGB | Finish | Functional Subsystem / Target Surface |
|:---:|---|:---:|:---:|---|---|
| **0** | Frame Shadow | `#1E242B` | `30, 36, 43` | Battered | Structural chassis ribs, belly skids, keel brackets. |
| **1** | Hull Olive | `#3B4438` | `59, 68, 56` | Clean | Outer protective cowlings, lateral fuselage flanks. |
| **2** | Armor Drab | `#4A5243` | `74, 82, 67` | Welder | Structural corner bulkheads, battery bay covers. |
| **3** | Core Hold Gray | `#2F353B` | `47, 53, 59` | Clean | Central 3×3×3 B Large Cargo Container shell. |
| **4** | Pure White Lift | `#F5F6FA` | `245, 246, 250` | Clean | Cardinal lift thruster cowlings (visual balance check). |
| **5** | Hazard Yellow | `#E5A93C` | `229, 169, 60` | Heavy Rust | Waist clamp rub rails (Gantry Cradle strike zone). |
| **6** | Warning Red | `#A83232` | `168, 50, 50` | Clean | Downward exhaust nozzle borders and lift boundaries. |
| **7** | Battery Cobalt | `#2C3E50` | `44, 62, 80` | Clean | Corner electrical power bays and battery casings. |
| **8** | Avionics Navy | `#1F2D3D` | `31, 45, 61` | Clean | Dorsal navigation bay, RC housing, and PB deck. |
| **9** | Beacon White | `#ECF0F1` | `236, 240, 241` | Neon | Status marker lights and belly camera searchlight. |

---

### 0.2B · Exterior Livery & Two-Tone Color Blocking Map

```
TOP-DOWN DORSAL LIVERY (Level L3, Looking −Y):
      X: −3   −2   −1    0   +1   +2   +3
  Z: +3 ┌────┬────┬═════════════┬────┬────┐  BOW (+Z)
        │CB 7│NV 8│   PW 4      │NV 8│CB 7│  ◄── Corner Battery / Avionics (Cobalt/Navy)
  Z: +2 ├────┴────┤ LIFT FORWARD├────┴────┤
        │         ├─────────────┤         │  ◄── Pure White Cardinal Lift Wings
  Z: +1 │  PW 4   │             │  PW 4   │
        │ LIFT P  │   CH 3      │ LIFT S  │
     0  │[HZ 5]   │ CARGO CORE  │ [HZ 5]  │  ◄── Orange Hazard Waist Rub Rails (X ±3)
        │ LIFT P  │  (3×3×3 B)  │ LIFT S  │
  Z: −1 │  PW 4   │             │  PW 4   │
        ├─────────┼─────────────┼─────────┤
  Z: −2 │         │  LIFT AFT   │         │  ◄── Pure White Cardinal Lift Wings
  Z: −3 ├────┬────┤   PW 4      ├────┬────┤
        │CB 7│NV 8│═════════════│NV 8│CB 7│  ◄── Corner Battery / Avionics (Cobalt/Navy)
        └────┴────┴═════════════┴────┴────┘  STERN (−Z)

PORT ELEVATION LIVERY (Looking +X at Port Flank):
      Z: −3       −2       −1        0       +1       +2       +3
  L3    ┌────────┬───────────────────────────────────┬────────┐  ROOF (L3)
        │ NV 8   │   Small Maneuvering Array (OL 1)  │  NV 8  │
  L2    ╞════════╧═══════════════════════════════════╧════════╡  MIDLINE (L2)
        │ [HAZARD 5] FLUSH GANTRY CRADLE CLAMP RUB RAIL [HZ 5]│
  L1    └───┬────────┬───┬───────────────────┬───┬────────┬───┘  BELLY (L1)
           [SKID 0]     [NOZZLE 6]          [NOZZLE 6]    [SKID 0]
```

---

### 0.4B · 3/4 Isometric Perspective Wireframes

#### View 1 · Dorsal Bow-Port 3/4 Isometric Wireframe (Flight Stance)
```
                  [BOW NOZZLE FAIRING]
                     +Z  ▲
                        / \
                       /   \
          ┌═══════════▼═════▼═══════════┐
         / │      MUL Lift Fwd (3×2)    │ \
        /  │         (Pure White)       │  \
       /   └═════════════╤══════════════┘   \
      /                  │                   \
┌────▼──────┐     ┌──────┴──────┐     ┌───────▼───┐
│ MUL Bat NW│     │             │     │ MUL Bat NE│
│(Cobalt L2)│     │  MUL Cargo  │     │(Cobalt L2)│
├───────────┤     │  Hold Core  │     ├───────────┤
│[RUB RAIL P│     │  (3×3×3 B)  │     │[RUB RAIL S│
│ Hazard L2]│◄────┤  Charcoal   ├────►│ Hazard L2]│
├───────────┤     │             │     ├───────────┤
│ MUL Bat SW│     │             │     │ MUL Bat SE│
│(Cobalt L2)│     └──────┬──────┘     │(Cobalt L2)│
└────▲──────┘            │            └───────▲───┘
      \                  ▼                   /
       \   ┌═════════════╧══════════════┐   /
        \  │      MUL Lift Aft (3×2)    │  /
         \ │         (Pure White)       │ /
          └═════════════════════════════┘
                        \   /
                         \ /
                          ▼ −Z (STERN)
```

#### View 2 · Ventral Belly 3/4 Isometric Wireframe (Docking Stance)
```
          [SKID NW]                     [SKID NE]
           (2×1 Hvy)                     (2×1 Hvy)
             \                             /
              ▼                           ▼
        ┌───────────┐               ┌───────────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └─────┬─────┘               └─────┬─────┘
              │                           │
              │     ┌───────────────┐     │
              │     │  MUL Cam Belly│     │
              │     │  [X0, Z+1]    │     │
              │     ├───────────────┤     │
              └───► │MUL Dock Belly │ ◄───┘
                    │(Large Con DOWN│
                    │ Recessed 0.5B)│
              ┌───► ├───────────────┤ ◄───┐
              │     │[0, 0, L1]     │     │
              │     └───────────────┘     │
              │                           │
        ┌─────┴─────┐               ┌─────┴─────┐
        │ ░░░░░░░░░ │               │ ░░░░░░░░░ │
        └───────────┘               └───────────┘
              ▲                           ▲
             /                             \
          [SKID SW]                     [SKID SE]
```

---

### 0.4C · Transverse Hull Station Profiles

#### Station 1 · Forward Wing & Gyro Bays (Z = +2.5 B)
Transverse section looking forward (+Z) through the bow thruster wing and forward avionics bays:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L3      ┌────────┬────────┬═════════════════════┬────────┬────────┐
         │Lat Port│Gyro P1 │ Small Maneuver Fwd  │Gyro S1 │Lat Stbd│
 L2      ├────────┼────────┼─────────────────────┼────────┼────────┤
         │Bat NW  │Heavy 21│ MUL Lift Fwd (3×2)  │Heavy 21│Bat NE  │
 L1      └───┬────┴────────┴───┬─────────────┬───┴────────┴────┬───┘
             ▼                 ▼▼▼         ▼▼▼                 ▼
          [SKID NW]           [EXHAUST NOZZLE]              [SKID NE]
```

#### Station 2 · Midship Hold & Waist Clamping Rub Rails (Z = 0.0 B)
Transverse section looking forward (+Z) through the physical Center of Mass:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L3      ┌────────┬───────────────────────────────────────┬────────┐
         │Catwalk │ Top Inspection Port (Manual Access)   │Catwalk │
 L2      ╞════════╪════════════╤═════════════╤════════════╪════════╡
         │[RUB P] │MUL Lift P  │  MUL Cargo  │MUL Lift S  │[RUB S] │
 L1      └───┬────┴────────────┴──────┬──────┴────────────┴────┬───┘
             │                        │                        │
       Gantry Clamp              MUL Dock Belly           Gantry Clamp
       Strike Surface            Connector DOWN           Strike Surface
       (Hazard Rail)             Recessed 0.5 B           (Hazard Rail)
```

#### Station 3 · Stern Wing & Power Sub-Bay (Z = −2.5 B)
Transverse section looking forward (+Z) through the aft thruster wing and telemetry spire:
```
Level:   X: −3       −2       −1        0       +1       +2       +3
 L3      ┌────────┬────────┬═════════════════════┬────────┬────────┐
         │Lat Port│[MUL PB]│   [MUL RC] [ANT]    │[MUL EC]│Lat Stbd│
 L2      ├────────┼────────┼─────────────────────┼────────┼────────┤
         │Bat SW  │Heavy 21│ MUL Lift Aft (3×2)  │Heavy 21│Bat SE  │
 L1      └───┬────┴────────┴───┬─────────────┬───┴────────┴────┬───┘
             ▼                 ▼▼▼         ▼▼▼                 ▼
          [SKID SW]           [EXHAUST NOZZLE]              [SKID SE]
```

---

### 0.5 · Master Conveyor Routing Schematic

The Mule features a zero-sorter, ultra-low resistance direct gravity-extraction loop. Sorter-assisted suction is handled entirely by host receiving pads (`ORC MP Conn` on Orca carrier, `GND PAD Conn` on Ground Base, and `MAS MP Conn` on Mastodon MKI):

```
                       [MUL Cargo Hold]
           (Standard Large Cargo Container 3×3×3 B)
           (Located X −1..+1, Z −1..+1, Levels L1-L3)
           (Hold Capacity: 1,386,000 L @ 3× Inventory)
           (Physical Manual Maintenance Port on Dorsal L3)
                                 │
                                 ▼ (Bottom Center Large Port)
                          [MUL Dock Belly]
           (Large Grid Connector · Faces Straight DOWN)
           (Flush L1 @ X 0, Z 0 · Recessed 0.5 B above Skids)
                                 │
        ═════════════════════════╪═════════════════════════  Pad Touchdown Plane
                                 │
                                 ▼ (Mates to Host Pad)
         [ORC MP Conn]    OR    [GND PAD Conn]    OR    [MAS MP Conn]
        (Orca Roof Deck)        (Ground Base)           (Mastodon Roof)
```

---

### 0.6 · Master Electrical Single-Line Schematic & 5 Power Modes

```
                  4× LARGE / WARFARE BATTERIES (L2 Pockets)
                    12.0 MWh Stored Capacity · 48.0 MW Max Surge
                                  │
                                  ▼
                          PRIMARY 48 MW BUS
        ┌─────────────────────────┼─────────────────────────┐
        │                         │                         │
        ▼                         ▼                         ▼
  CARDINAL LIFT ARRAY       6-AXIS MANEUVER           AVIONICS & CONTROL
  4× Large Flat Atmo        10× Small Atmo            2× Large Gyros (0.80 MW)
  (26.80 MW Max Surge)      (6.48 MW Max Surge)       1× RC + 1× PB (0.05 MW)
  [Empty Hover: 3.62 MW]    [Braking Load: 2.60 MW]   Antenna + 2× Cam (0.02 MW)
  [Laden Hover: 20.05 MW]   [Push-Down: 1.30 MW]      3× EC + 2× Timer (0.00 MW)
```

#### Operational Energy Modes
1. **Mode 1 · Docked Shore Charge (Recharge = ON):**
   * Total Draw: `0.00 MW` · Shore Intake: `48.0 MW` · Full 0 to 100% recharge in `15.0 minutes`.
2. **Mode 2 · Empty Ballast Transit (142.8 t):**
   * Lift Load: 13.5% (3.62 MW) · Maneuver: 1.30 MW · Total: `4.92 MW` · Endurance: `2.44 hours`.
3. **Mode 3 · Fully Laden Hover (792.8 t · 650 t Ore):**
   * Lift Load: 74.8% (20.05 MW) · Maneuver: 2.60 MW · Total: `22.72 MW` · Endurance: `31.7 minutes`.
4. **Mode 4 · High-G Touchdown Flare / Push-Down Braking:**
   * 4× Flat Atmo (100% burst: 26.80 MW) + 2× Push-Down Atmo (1.30 MW) + Gyros (0.80 MW) = `28.90 MW` peak surge. Battery headroom margin = `+19.10 MW` (66% reserve).
5. **Mode 5 · Emergency Deadman Hover / Safe Return:**
   * Battery stored $\le 15\%$. Non-essential systems cut, speed capped at 30 m/s, autonomous return to nearest gantry cradle.

---

### 0.7 · Master Bill of Materials (BOM) & Physics Budget

| Category | Component Description | Block Qty | Mass (t) | PCU | Critical Mounting / Orientation |
|---|---|:---:|:---:|:---:|---|
| **Chassis** | Large Cargo Container (3×3×3 B) | 1 | 28.5 | 250 | Centered at [X0, Y0, Z0], L1-L3 |
| **Chassis** | Heavy Armor Corner Landing Skids (2×1) | 16 | 24.0 | 256 | L1 belly corners [X ±3, Z ±3] |
| **Chassis** | Heavy Armor Clamp Strike Rub Rails | 12 | 18.0 | 192 | L2 outer rub rails [X ±3, Z −1..+1] |
| **Chassis** | Light Armor Hull Fairings & Catwalks | 40 | 12.0 | 400 | L2-L3 cowlings, handrails, brow |
| **Propulsion** | Large Flat Atmospheric Thrusters (3×2 B) | 4 | 44.0 | 320 | N, S, E, W cardinal wings, facing DOWN |
| **Propulsion** | Small Atmospheric Thrusters (1×1 B) | 10 | 18.0 | 250 | 2 Bow, 2 Stern, 2 Port, 2 Stbd, 2 Roof |
| **Power** | Large Batteries (or Warfare Batteries) | 4 | 20.0 | 80 | 4 corner bays (3 MWh each = 12 MWh) |
| **Attitude** | Large Gyroscopes (33.6 MN·m each) | 2 | 5.6 | 100 | Forward corner bays [X ±2, Z +3, L2] |
| **Docking** | Large Grid Connector | 1 | 4.2 | 150 | Centered at [X0, Z0, L1], facing DOWN |
| **Avionics** | Remote Control Block | 1 | 0.5 | 25 | Aft bay [X0, Z −2, L3], Fwd = +Z |
| **Avionics** | Programmable Block | 1 | 0.5 | 25 | Aft bay [X −1, Z −2, L3] (`[MUL] PB Shuttle`) |
| **Avionics** | Radio Antenna | 1 | 0.8 | 100 | Aft bay [X +1, Z −2, L3] (5,000 m range†) |
| **Avionics** | Fixed Cameras | 2 | 0.1 | 20 | 1 Belly [X0, Z+1, L1], 1 Bow [X0, Z+3, L3] |
| **Logic** | Event Controllers | 3 | 0.3 | 75 | Aft bay L3 (Cargo, Batt, Dock) |
| **Logic** | Timer Blocks | 2 | 0.3 | 50 | Aft bay L3 (Heartbeat, Return) |
| **Lighting** | Aviation Strobe / Nav Lights | 4 | 0.1 | 40 | Corner cowlings L3 |
| **TOTAL** | **Complete MULE-01 Heavy Freight Shuttle** | **104** | **≈ 157.0 t** | **2,333** | **Zero-sorter, pure electric** |

#### Physics & Gravitational Lift Margin
* **Dry Mass:** `157.0 t` (Structure + Blocks)
* **Maximum Payload Ore:** `650.0 t` (1,386,000 L @ 3× inventory)
* **Gross Laden Mass:** `807.0 t`
* **Downward Lift Thrust (4× Large Flat Atmo):** $4 \times 2.6\text{ MN} = 10.40\text{ MN}$
* **Gravity Downward Force (Earth 1.0 g = 9.81 m/s²):**
  * Empty: $157.0\text{ t} \times 9.81\text{ m/s}^2 = 1.54\text{ MN}$ (Hover Load = 14.8%)
  * Laden: $807.0\text{ t} \times 9.81\text{ m/s}^2 = 7.92\text{ MN}$ (Hover Load = 76.2%)
* **Thrust-to-Weight (T/W) Ratio:**
  * Empty T/W: $10.40 / 1.54 = \mathbf{6.75}$
  * Laden T/W: $10.40 / 7.92 = \mathbf{1.313}$ (31.3% reserve margin in standard planetary atmosphere)

---

### 0.7B · Human Circulation, Catwalks & Maintenance Ergonomics
1. **Dorsal Catwalk & Service Ladder:** Level L3 features a recessed perimeter walkway with safety handrails surrounding the upper Small Atmo cluster.
2. **Manual Inventory Access Port:** The top port of `MUL Cargo Hold` is exposed via a flush trapdoor on Level L3, allowing engineers to manually deposit emergency tools, components, or uranium fuel without cycling docking connectors.
3. **Corner Boarding Rungs:** Retractable ladder rungs recessed into the heavy armor skirts at [X −3, Z −2, L1] and [X +3, Z −2, L1] allow EVA engineers to climb from planetary surface level onto the L3 deck when grounded.

---

## Phase 3-1 · Central Ore Hold & Belly Docking Interface (L1-L3)

### Step 3.1 · Large Cargo Core & Ventral Connector (L1-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Central Hold  ·  Footprint: X −1..+1, Z −1..+1  ·  Level: L1-L3 │
│ [PARTS TRAY] 1× Large Cargo Container (3×3×3 B), 1× Connector (Large), 1× Camera     │
│ [PALETTE] Cargo Shell: Slot 3 (#2F353B), Connector: Slot 0 (#1E242B)                 │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Place 1 standard Large Cargo Container (3×3×3 B) `MUL Cargo Hold` centered precisely at coordinate `[X: 0, Y: 0, Z: 0]`, spanning Levels L1 through L3.
2. Ensure the bottom center large conveyor port faces directly DOWN at Level L1.
3. Attach 1× Large Grid Connector `MUL Dock Belly` to this bottom port, oriented facing straight DOWN (−Y).
4. Mount 1× Camera `MUL Cam Belly` recessed immediately adjacent to the connector at [X 0, Z +1, L1] pointing straight DOWN.

```
Plan view, Level L1 (Ventral Core):
      X: −1     0    +1
  Z: +1 ┌─────┬─────┬─────┐
        │     │[CAM]│     │   ◄── [CAM] = MUL Cam Belly (facing DOWN)
     0  │     │[CON]│     │   ◄── [CON] = MUL Dock Belly (Connector DOWN)
        │     │     │     │
  Z: −1 └─────┴─────┴─────┘
              (3×3 B)
```

---

### Step 3.2 · Belly Landing Skids & Skid Foundation (L1)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Landing Skids  ·  Footprint: X ±3, Z ±3  ·  Level: L1           │
│ [PARTS TRAY] 16× Heavy Armor 2×1 Slopes, 8× Heavy Armor Half Blocks                  │
│ [PALETTE] Finish: Battered  ·  Color: Slot 0 (#1E242B)                               │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Construct 4 corner landing skids at [X ±3, Z ±3] on Level L1.
2. Shape each skid using 2×1 Heavy Armor Slopes angled outward to form a broad 17.5 × 17.5 m stance.
3. Tie the skids to the central cargo hold using under-chassis Heavy Armor beams spanning along Z = 0 and X = 0.

```
Plan view, Level L1 (Skid Layout):
      X: −3   −2   −1    0   +1   +2   +3
  Z: +3 [SKID]···                 ···[SKID]  ◄── Heavy Armor 2×1 Skid (NW / NE)
         │       ┌─────────────┐       │
     0   │       │  MUL Cargo  │       │     ◄── Under-chassis tie beams
         │       └─────────────┘       │
  Z: −3 [SKID]···                 ···[SKID]  ◄── Heavy Armor 2×1 Skid (SW / SE)
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 3.1A: CONNECTOR RECESS CLEARANCE**
> Stand at ground level and sight across the bottom faces of all 4 corner skids:
> 1. Verify `MUL Dock Belly` sits recessed exactly **0.5 blocks (1.25 m)** above the skid contact plane.
> 2. If the connector hangs flush with or below the skids, landing on uneven terrain will crush the connector!

---

### Phase 3-1 Visual Milestone
```
Standing 15 m away looking at Starboard Flank (Level L1-L3):
       L3  ┌───────────────────────────────────┐  ◄── Large Cargo Container Top
           │                                   │
       L2  │       LARGE CARGO CONTAINER       │      (3×3×3 B Center Core)
           │                                   │
       L1  └──[SKID]───────[BELLY]───────[SKID]┘  ◄── Corner Landing Skids
```

---

## Phase 3-2 · Symmetrical Cardinal Lift Propulsion (L1-L2)

### Step 3.3 · Cardinal Lift Thruster Cross (L1-L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Cardinal Lift  ·  Wings: N, S, E, W  ·  Level: L1-L2            │
│ [PARTS TRAY] 4× Large Flat Atmospheric Thrusters (3×2 B · Large Grid)                │
│ [PALETTE] Cowling: Slot 4 Pure White (#F5F6FA), Nozzle Border: Slot 6 (#A83232)      │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Mount 4 Large Flat Atmospheric Thrusters directly against the 4 vertical faces of `MUL Cargo Hold`, exhausting straight DOWN:
1. `MUL Lift Fwd` : Mounted on Bow face [Z +2 to +3, X −1 to +1, L1-L2].
2. `MUL Lift Aft` : Mounted on Stern face [Z −2 to −3, X −1 to +1, L1-L2].
3. `MUL Lift Port` : Mounted on Port face [X −2 to −3, Z −1 to +1, L1-L2].
4. `MUL Lift Stbd` : Mounted on Starboard face [X +2 to +3, Z −1 to +1, L1-L2].
5. Assign all 4 thrusters to terminal group `MUL Lift`.

```
Plan view, Level L2 (Cardinal Lift Cross):
      X: −3    −1     0    +1    +3
  Z: +3   ┌═════════════┐
          │MUL Lift Fwd │         ◄── Forward Wing (3×2 B)
  Z: +1  ┌┤ (Pure White)├┐
         │├─────────────┤│
     0   ││  MUL Cargo  ││        ◄── Port Wing (2×3 B) / Starboard Wing (2×3 B)
         ││  (3×3×3 B)  ││
  Z: −1  │├─────────────┤│
  Z: −3  └┤MUL Lift Aft ├┘
          │ (Pure White)│         ◄── Aft Wing (3×2 B)
          └═════════════┘
```

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 3.2A: DOWNWARD EXHAUST CLEARANCE**
> Ensure the bottom nozzle grills of all 4 Flat Atmo thrusters have a completely clear line of sight downward through Level L1. Any armor blocks placed directly underneath the nozzles will cause internal back-pressure damage!

---

## Phase 3-3 · Corner Power Pockets & Gyro Stabilization Bays (L1-L3)

### Step 3.4 · 4-Quadrant Large Battery Array (L1-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Power Core  ·  Pockets: NW, NE, SW, SE Corners  ·  Level: L1-L3 │
│ [PARTS TRAY] 4× Large Batteries (or Warfare Batteries · 3 MWh each)                  │
│ [PALETTE] Battery Shell: Slot 7 Cobalt (#2C3E50), Fairings: Slot 2 Armor Drab        │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Install 4 Large Batteries in the open 2 × 2 B corner cavities:
1. `MUL Battery NW` : Forward-Port corner [X −2 to −3, Z +2 to +3, L2].
2. `MUL Battery NE` : Forward-Starboard corner [X +2 to +3, Z +2 to +3, L2].
3. `MUL Battery SW` : Aft-Port corner [X −2 to −3, Z −2 to −3, L2].
4. `MUL Battery SE` : Aft-Starboard corner [X +2 to +3, Z −2 to −3, L2].
5. Assign all 4 to terminal group `MUL Batteries` (12.0 MWh aggregate capacity, 48.0 MW peak discharge).

```
Plan view, Level L2 (Power Pockets):
      X: −3   −2   −1     0    +1   +2   +3
  Z: +3  [BAT NW]     [MUL Lift Fwd]    [BAT NE]
  Z: +2  [ 3 MWh]                       [ 3 MWh]
         ········    ┌─────────────┐    ········
     0   [LIFT P]    │  MUL Cargo  │    [LIFT S]
         ········    └─────────────┘    ········
  Z: −2  [BAT SW]     [MUL Lift Aft]    [BAT SE]
  Z: −3  [ 3 MWh]                       [ 3 MWh]
```

---

### Step 3.5 · Flight Stabilization Gyroscopes (L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Attitude Control  ·  Pockets: Fwd P/S  ·  Level: L2             │
│ [PARTS TRAY] 2× Large Gyroscopes (33.6 MN·m each)                                    │
│ [PALETTE] Slot 0 Frame Shadow (#1E242B)                                              │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Install 1× Large Gyroscope `MUL Gyro Port` at [X −2, Z +2, L2] immediately aft of `BAT NW`.
2. Install 1× Large Gyroscope `MUL Gyro Stbd` at [X +2, Z +2, L2] immediately aft of `BAT NE`.
3. Assign both to group `MUL Gyros` (67.2 MN·m aggregate torque).

---

## Phase 3-4 · Waist Clamping Strike Rub Rails (L2)

### Step 3.6 · Heavy Armor Gantry Cradle Strike Rub Rails (L2)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Waist Rails  ·  Flanks: Port & Stbd  ·  Level: L2               │
│ [PARTS TRAY] 12× Heavy Armor Full Blocks (Large Grid)                                │
│ [PALETTE] Hazard Striping: Slot 5 Hazard Yellow (#E5A93C) with Heavy Rust            │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Per **Decision 45**, the Mule carries zero motorized clamping arms. External Gantry Cradles on host decks clamp directly onto the Mule's chassis:
1. Mount a 3-block continuous span of Heavy Armor Full Blocks along the Port flank at [X −3, Z −1 to +1, L2].
2. Mount a 3-block continuous span of Heavy Armor Full Blocks along the Starboard flank at [X +3, Z −1 to +1, L2].
3. Paint both spans in Palette Slot 5 (Hazard Yellow with chevron striping).

```
Section view, looking Bow at Port flank (X = −3):
  L3   ┌───────────────────────────┐
       │ Light Armor Cowling       │
  L2   ╞═══════════════════════════╡  ◄── [WAIST RUB RAIL PORT]
       │ Heavy Armor Strike Face   │      Flush magnetic clamping surface
  L1   ╞═══════════════════════════╡      engaged by Gantry Cradle plates
       │ Skid Base Line            │
       └───────────────────────────┘
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 3.3A: CRADLE STRIKE FLUSHNESS GATE**
> Verify the outer faces of the hazard rub rails at X = ±3 are completely smooth and flush with the surrounding armor. No conveyor hubs, buttons, or lights may protrude beyond X = ±3.0 m, or they will sheer off during cradle clamping!

---

## Phase 3-5 · 6-Axis Maneuvering Array & Roof Push-Down Thrusters (L2-L3)

### Step 3.7 · 6-Axis Small Atmospheric Thruster Perimeter (L2-L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Maneuvering Array  ·  Perimeter & Roof  ·  Level: L2-L3         │
│ [PARTS TRAY] 10× Small Atmospheric Thrusters (Large Grid · 648 kN each)              │
│ [PALETTE] Slot 1 Hull Olive (#3B4438), Exhaust Nozzle Trim: Slot 6 (#A83232)         │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
Mount 10 Small Atmospheric Thrusters to provide agile 6-axis translation and high-G push-down descent:
1. **Bow (+Z):** Mount 2× Small Atmo `MUL Lat Fwd 1, 2` facing Forward (exhausts +Z to push Aft) at [X ±1, Z +3, L3].
2. **Stern (−Z):** Mount 2× Small Atmo `MUL Lat Aft 1, 2` facing Aft (exhausts −Z to push Fwd) at [X ±1, Z −3, L3].
3. **Port (−X):** Mount 2× Small Atmo `MUL Lat Port 1, 2` facing Port (exhausts −X to push Stbd) at [X −3, Z ±1, L3].
4. **Starboard (+X):** Mount 2× Small Atmo `MUL Lat Stbd 1, 2` facing Starboard (exhausts +X to push Port) at [X +3, Z ±1, L3].
5. **Dorsal Roof (+Y):** Mount 2× Small Atmo `MUL Thrust Down 1, 2` facing UP (exhausts +Y to push straight DOWN) at [X ±1, Z 0, L3].
6. Assign all 10 to terminal group `MUL Maneuver`.

```
Plan view, Level L3 (Roof Line Perimeter & Push-Down Thrusters):
             Bow (Z = +3)
          ◄── [MUL Lat Fwd ×2] ──►   (Exhausts Fwd / Brakes Bow)
       ┌────────────────────────────┐
       │     [Push Down 1]          │
  Port │ [MUL Lat Port ×2]          │ Stbd [MUL Lat Stbd ×2]
  (−X) │                            │ (+X)
       │     [Push Down 2]          │
       └────────────────────────────┘
          ◄── [MUL Lat Aft ×2] ──►   (Exhausts Aft / Brakes Stern)
             Stern (Z = −3)
```

---

## Phase 3-6 · Autonomous Avionics Core, Sensors & Maintenance Deck (L3)

### Step 3.8 · Aft Avionics Vault & Comms (L3)
```
┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Avionics  ·  Aft Upper Deck  ·  Level: L3, Z −2 to −3           │
│ [PARTS TRAY] 1× Remote Control, 1× PB, 1× Radio Antenna, 1× Camera, 3× EC, 2× Timer  │
│ [PALETTE] Slot 8 Avionics Navy (#1F2D3D)                                             │
└──────────────────────────────────────────────────────────────────────────────────────┘
```
1. Mount 1× Remote Control `MUL RC` at [X 0, Z −2, L3]. Ensure Forward points Bow (+Z) and Up points Roof (+Y).
2. Mount 1× Programmable Block `[MUL] PB Shuttle` at [X −1, Z −2, L3].
3. Mount 1× Radio Antenna `MUL Antenna` at [X +1, Z −2, L3]. Set broadcast range to `5,000 m`†.
4. Mount 1× Camera `MUL Cam Fwd` at [X 0, Z +3, L3] facing Bow (+Z).
5. Mount logic block array along the northern equipment bulkhead:
   * `MUL EC Cargo` : Event Controller monitoring payload mass.
   * `MUL EC Batt` : Event Controller monitoring battery percentage.
   * `MUL EC Dock` : Event Controller monitoring belly connector lock.
   * `MUL TMR Heartbeat` : Safety watchdog timer set to `5.0 s`.
   * `MUL TMR Return` : Automated departure timer set to `2.0 s`.

```
Plan view, Level L3 (Aft Avionics Array):
      X: −2     −1       0       +1     +2
  Z: −2  [EC]  │[PB] │  [RC]  │ [ANT] │ [TMR]
  Z: −3  [EC]  │     │ [CAT]  │       │ [TMR]
```

---

## Phase 3-7 · Master Control Logic, AI Scripting Contract & Software Abstraction

### 1. Terminal Group Taxonomy
* `MUL Lift` : All 4 Large Flat Atmo Thrusters (`MUL Lift Fwd`, `Aft`, `Port`, `Stbd`).
* `MUL Maneuver` : All 10 Small Atmo Thrusters (Bow, Stern, Port, Stbd, Down).
* `MUL Batteries` : All 4 Large Batteries (`MUL Battery NW`, `NE`, `SW`, `SE`).
* `MUL Gyros` : All 2 Large Gyroscopes (`MUL Gyro Port`, `MUL Gyro Stbd`).
* `MUL Cams` : Both navigation cameras (`MUL Cam Belly`, `MUL Cam Fwd`).

---

### 2. Event Controller Automation Ladder (Rule C4)

| Controller | Monitored Parameter | Threshold / Event | Slot 1 Action (True) | Slot 2 Action (False) |
|---|---|---|---|---|
| `MUL EC Cargo` | Grid Total Mass | Mass $\ge 800.0\text{ t}$ (Payload full) | Start `MUL TMR Return` (2 s) | · |
| `MUL EC Batt` | `MUL Batteries` Stored % | Stored Power $\le 20.0\%$ | Trigger Emergency RTB / Pad abort | · |
| `MUL EC Dock` | `MUL Dock Belly` State | Connector Status = Locked | `MUL Lift` OFF · `MUL Batteries` Recharge | `MUL Lift` ON · `MUL Batteries` Auto |

---

### 3. Native Block Failsafe Matrix (Zero-PB Safety)
If `[MUL] PB Shuttle` crashes, freezes, or encounters an uncaught runtime exception:
1. `MUL TMR Heartbeat` (5.0 s loop) fails to receive a PB reset pulse and expires.
2. Immediate hardwired failsafe actions triggered by timer:
   * Remote Control Autopilot `Collision Avoidance` = ON.
   * Remote Control `Dampeners` = ON.
   * Gyro and Thrust Overrides = OFF.
   * Shuttle maintains stationary hover at current altitude until operator overrides via C2 console.
3. If docked at a Gantry Cradle when failure occurs:
   * Belly connector remains magnetically locked.
   * `MUL Lift` remains locked OFF, preventing accidental thruster tear-away against magnetic clamp arms.

---

### 4. IGC Broadcast Network Dictionary

```
TMS.MULE.STATUS (Broadcast every 1.66 s):
MUL-01;<state_enum>;<ore_mass_t>;<battery_pct>;<destination_gps>
Example: MUL-01;LADEN_TRANSIT;648.5;92.0;GPS:GND_PAD:12450.2:-34200.5:810.0:

TMS.MULE.CALL (Inbound dispatch trigger):
<requester_id>;<pad_id>;<payload_type>;<urgency>
Example: ORC-01;ORC_MP;RARE_ORE;HIGH

TMS.LOGISTICS.JIT (Inbound pre-dispatch trigger from Base C2 or Mastodon MKI):
DISPATCH;MUL-01;<pad_id>;<payload_type>
Example: DISPATCH;MUL-01;PAD-1;RARE_ORE
```

---

## Phase 3-8 · Commissioning Checklist & Rigorous Test Protocols (RIG Test Creative)

### Suite A · Dry Mass & CoM Zero-Tip Balance Calibration
* [ ] **Dry Mass Verification:** Spawn MULE-01 in creative; terminal reports dry mass `≈ 150-160 t`.
* [ ] **Zero-Torque Hover:** Lift off to 50 m altitude in Earth 1.0 g; release dampeners; confirm pitch and roll remain `0.00°` (CoM perfectly aligned with center of lift).
* [ ] **Empty Hover Power:** Verify 4× Flat Atmo thrusters operate at `14-16%` load (`≈ 3.6-4.2 MW` draw).

### Suite B · 650 t Ore Gravitational Lift Margin Test
* [ ] **Full Ore Spawn:** Load exactly 650 t of iron ore into `MUL Cargo Hold` (Gross mass `≈ 807 t`).
* [ ] **Laden Hover Power:** Verify 4× Flat Atmo thrusters hover cleanly at `76-78%` load (`≈ 20.0-20.5 MW` draw). Confirm T/W margin `≥ 1.30`.
* [ ] **Vertical Climb Rate:** Apply full upward collective; verify vertical climb rate reaches `≥ 12.0 m/s` within 3.0 seconds.

### Suite C · High-G Push-Down Braking & Flare Test
* [ ] **Vertical Descent Acceleration:** From 300 m altitude, apply downward thruster override (`MUL Thrust Down 1, 2`); verify descent acceleration reaches `13.5 m/s²`.
* [ ] **Terminal Touchdown Flare:** At 50 m altitude, cut push-down thrusters and engage full lift collective; verify descent arrests safely before ground impact.

### Suite D · Orca Roof MP Touchdown & Belly Connector Latch
* [ ] **Approach Alignment:** Guide Mule over Orca carrier roof pad (`ORC MP Conn`); verify `MUL Cam Belly` view aligns center marker.
* [ ] **Touchdown & Lock:** Touch down on pad; verify `MUL Dock Belly` locks cleanly with corner skids resting flat on blast deck.
* [ ] **Connector Recess Verification:** Confirm belly connector experiences zero mechanical bounce or compression damage.

### Suite E · Gantry Cradle Motorized Waist Clamping Engagement
* [ ] **Cradle Swing:** Trigger host Gantry Cradle hinges (swinging inward 30°); verify magnetic plates strike flush against `[WAIST RUB RAIL PORT/STBD]` at Level L2.
* [ ] **Magnetic Latch:** Lock magnetic plates; turn `MUL Lift` OFF; confirm Mule is clamped rigidly to carrier deck during 50 m/s carrier evasive maneuvers.

### Suite F · High-Speed Ore Transfer & Turnaround (< 45 s)
* [ ] **Rapid Drain Test:** Engage host pad intake sorters (`ORC MP Sorter` / `GND PAD Sorter`); verify 650 t ore evacuates through `MUL Dock Belly` in $< 45\text{ seconds}$† without conveyor back-pressure.
* [ ] **Departure Sequence:** On empty signal (`MUL EC Cargo`), verify Gantry Cradle unclutches, `MUL Lift` engages, and shuttle executes clean vertical ascent.

### Suite G · Point-to-Point 100 m/s Transit Corridor Navigation
* [ ] **Waypoint Cruise:** Fly 5 km transit corridor between Orca and Ground Base at 100 m/s; verify 10× Small Atmo array maintains course heading with zero lateral slip.
* [ ] **Battery Consumption:** Verify total energy consumed over 5 km laden flight is $< 1.8\text{ MWh}$ ($< 15\%$ battery capacity).

### Suite H · Ground Base Pad Touchdown, Fast Shore Recharge & Turnaround
* [ ] **Base Terminal Capture:** Approach `GND PAD`; auto-engage gantry clamping arms; lock `GND PAD Conn`.
* [ ] **Fast Shore Recharge:** Verify 4× Large Batteries switch to `Recharge` and pull full 48.0 MW shore power, reaching 100% state of charge in $< 15\text{ minutes}$.

### Suite I · Native Heartbeat Loss & Emergency Deadman Hover Fallback
* [ ] **Simulated PB Crash:** Recompile `[MUL] PB Shuttle` with a crash script during transit; verify `MUL TMR Heartbeat` times out at 5.0 s, disengages overrides, activates dampeners, and arrests flight into a stable altitude hold.

---

## Phase 3-9 · Unverified Values Tracking (†)

Tracked in [`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md):
* **V37:** Large Flat Atmospheric Thruster large-grid sea-level thrust rating (verify 2.6 MN rating and power consumption).
* **V38:** Ore transfer drain rate between large-grid containers through a single connector (measure seconds per 650 t).
* **V48:** MULE-01 laden descent deceleration on 2× Small Push-Down Atmo thrusters ($13.5\text{ m/s}^2$†).
