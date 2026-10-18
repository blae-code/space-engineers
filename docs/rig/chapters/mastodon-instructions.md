# Mastodon · Visual Build Instructions

## Chapter 10 · MASTODON-01 Heavy Wheeled Excavator

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for MASTODON-01.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

Source specs:
* Authority: Blae Decisions 4 (Simple Laser Multitools), 10 (Vanilla 5×5 wheels), 25 (Earth 1 g), 35 (Mechanical subgrids permitted), 41 (3× Inventory), 45 (Gantry Cradle Pad), 50 (Mastodon excavator), 52 (Dual-tier logistics)
* Master Manual Reference: `docs/rig/lego-instructions-v1.0.md` Chapter 10
* Grid: Large Grid (1 B = 2.5 m) · Bounding Envelope: 11 W × 25 L × 6 H blocks (27.5 × 62.5 × 15.0 m)

Last updated: 2026-10-17

---

## 0 · Reference Grid, In-Game Paint Palette & Master Envelope

### 0.1 Coordinate Triad & Axis Conventions
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

### 0.2 In-Game Paint Palette & Functional Color Coding

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

### 0.3 Master Profile Elevation (Looking Starboard, X = 0)

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

### 0.4 Master Conveyor Routing Schematic

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

### 0.5 Master Electrical Single-Line Schematic

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

### 0.6 Master Pre-Flight Bill of Materials (BOM)

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

## Phase 1 · Lower Chassis & 10-Wheel Suspension (Levels L1-L2)

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

### Step 1 · Twin Longitudinal Chassis Box Girders (L1-L2)

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

### Step 2 · 5×5 Off-Road Suspension Wheel Mounts & Master Steering Rig (L1)

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

#### Master 5-Axle Suspension Parameter Table

| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 1** | Z = +10 | `WH 1, 2` | `30.0°` | Normal | `100%` | `45% / 85%` | `65%` |
| **Axle 2** | Z = +5  | `WH 3, 4` | `15.0°` | Normal | `100%` | `45% / 85%` | `65%` |
| **Axle 3** | Z = 0   | `WH 5, 6` | `0.0°`  | OFF (Locked) | `100%` | `50% / 90%` | `70%` |
| **Axle 4** | Z = −5  | `WH 7, 8` | `15.0°` | INVERTED | `100%` | `45% / 85%` | `65%` |
| **Axle 5** | Z = −10 | `WH 9, 10`| `30.0°` | INVERTED | `100%` | `45% / 85%` | `65%` |

---

### Step 3 · Underbody Skid Plates & Voxel Deflectors (L1)

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

#### Visual Milestone 10-1 · Lower Chassis & 10-Wheel Suspension Complete

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

## Phase 2 · Triple Large Cargo Hold & Central Spine (Levels L2-L4)

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

### Step 4 · Triple In-Line Large Cargo Hold (L2-L4)

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

### Step 5 · Power Core Vault & Electrical Isolation (Z −10 to −7, L2-L4)

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

#### Visual Milestone 10-2 · Cargo Spine & Power Core Vault Enclosed

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

## Phase 3 · Twin Articulated "Tusk" Booms & CTC Rigging (Levels L2-L4)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 15 W × 10 L × 4 H B (37.5 × 25.0 × 10.0 m articulated reach)
       Coordinate Range: X −6 to +6, Z +12 to +18, Y = L2 to L5
       Shoulder Datums:
       · Port Shoulder Rotor: [X −3, Z +12, Y = L2]
       · Starboard Shoulder Rotor: [X +3, Z +12, Y = L2]
       · Bow Conveyor Throat: [X0, Z +12, Y = L2]
```

### Step 6 · Tusk Shoulder Advanced Rotors (Z = +12, X = ±3, L2)

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

### Step 7 · Boom & Forearm Articulation Hinges (L2-L4)

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

### Step 8 · Twin Laser Mining Tool Heads (Head Assembly)

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

### Step 9 · Custom Turret Controller (CTC) Mouse-Aim Rigging

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

#### Visual Milestone 10-3 · Twin Articulated Tusks Rigged & Calibrated

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

## Phase 4 · Control Bridge & Operating Deck (Levels L3-L5)

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

### Step 10 · Forward Armored Bridge & Sensor Core (Z +6 to +11, L3-L5)

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

## Phase 5 · Flight Decks & Air Extraction Interfaces (Levels L5-L6)

```
       [PHASE 5 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 20 L × 2 H B (17.5 × 50.0 × 5.0 m)
       Coordinate Range: X −3 to +3, Z −11 to +9, Y = L5 to L6
       Landing Pad Datums:
       · Kestrel Scout Pad Port: [X −3, Z +8, Y = L5]
       · Kestrel Scout Pad Starboard: [X +3, Z +8, Y = L5]
       · Mule Gantry Pad Center: [X0, Z −8, Y = L6]
```

### Step 11 · Forward Kestrel Scout Drone Landing Pads (Z +7 to +9, L5)

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

### Step 12 · Aft Mule Receiving Gantry Cradle Pad (Z −11 to −5, L6)

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

## Phase 6 · Stern Bumper & Land Train Interface (Levels L1-L3)

```
       [PHASE 6 SPATIAL LOCATOR HUD]
       Envelope: 5 W × 3 L × 3 H B (12.5 × 7.5 × 7.5 m)
       Coordinate Range: X −2 to +2, Z −13 to −11, Y = L1 to L3
       Docking Datum:
       · Rear Dump Connector: [X0, Z −12, Y = L2] (Centerline height = 2.5 m above ground)
```

### Step 13 · Rear Train Dump Port & Push Bumper (Z = −12, X = 0, L2)

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

#### Visual Milestone 10-4 · Complete Mobile Excavator Commissioned

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

## Phase 7 · Master Control Logic & Hardware Scripting Contract (Claude Code)

### Step 14 · Terminal Namespace & Group Taxonomy

#### 1. Group Taxonomy
* `MAS Wheels` : All 10× 5×5 heavy off-road suspension wheel units.
* `MAS Tusks` : 6× Hinge and Rotor articulation blocks across both boom arms.
* `MAS Lasers` : 4× Simple Laser Multitools in mining mode.
* `MAS Holds` : 3× Large Cargo Containers (`MAS Hold 1, 2, 3`).
* `MAS Reactors` : 2× Small Nuclear Reactors.
* `MAS Batteries` : 4× Large Batteries.
* `MAS MP Cradle` : 2× Motorized Hinges + 2× Magnetic Plates on the roof pad.

#### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MAS EC Holds` | `MAS Holds` Total Mass | Total Mass $\ge 2,500\text{ t}$ | Chime Alert · IGC `TMS.MAST.FULL` | Normal mining |
| `MAS EC MP` | `MAS MP Conn` | Connector: Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle 0° |
| `MAS EC Train` | `MAS Stern Dump Conn` | Connector: Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Speed` | Vehicle Speed | Speed $> 45\text{ km/h}$ | Wheel Strength $\rightarrow 80\%$ (Anti-roll) | Wheel Strength $\rightarrow 50\%$ |

#### 3. IGC Channel Subscriptions (Ground Fleet Bus)
* Outbound: `TMS.MAST.STATUS`: `mastodon_id;ore_mass_t;payload_pct;gps_coords;active_drones`
  * Example: `MAS-01;2150.4;86.0;GPS:DepositAlpha:...:;2`
* Outbound: `TMS.MULE.CALL`: `requester_id;pad_id;payload_type;urgency`
* Inbound: `TMS.CATER.ARRIVED`: signals land train is locked into stern bumper.

---

## Phase 8 · Commissioning & Field Acceptance Checklist (RIG Test)

### A · Powertrain & Maneuvering Trials
* [ ] **Dry Mass Verification:** Confirm empty mass reads `≈ 694.4 t` in info tab.
* [ ] **5-Axle Counter-Steer Trial:** Drive forward at 30 km/h; verify Axles 1 & 2 steer, Axle 3 tracks straight, and Axles 4 & 5 counter-steer. Confirm zero lateral tire scrub.
* [ ] **Laden Grade Climb:** Fill holds with 2,500 t stone; verify all 10 wheels climb a 25° voxel incline at $> 25\text{ km/h}$ without power stalling.

### B · Tusk Boom & Laser Excavation Trials
* [ ] **Kinematic Envelope Verification:** Articulate tusks through full 105° vertical pitch arc (−45° to +60°) and 90° azimuth sweep (−30° to +60°).
* [ ] **CTC Mouse-Aim Response:** Sit in bridge control seats; engage CTC; verify smooth, instantaneous mouse aiming of both arms.
* [ ] **37.5 m Swath Excavation:** Drive forward at 0.5 m/s while carving with all 4 laser beams; confirm a clean 37.5 m wide by 12.5 m deep trench is cleared with zero un-mined pillars.

### C · Dual Logistics Offload Acceptance
* [ ] **Roof Mule Landing:** Land MULE-01 on roof deck; verify cradle arms swing 30° inward and clamp rub rails; verify rare ores drain into Mule hold.
* [ ] **Stern Train Fast-Dump:** Mate Caterpillar tractor into rear bumper connector; verify `MAS Stern Sorter` discharges 2,500 t bulk ore in $< 90\text{ seconds}$.
