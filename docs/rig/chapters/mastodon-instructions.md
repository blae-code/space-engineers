# Mastodon · Visual Build Instructions

## Chapter 10 · MASTODON-01 (MKI) Expeditionary Mining Locomotive & Mobile FOB

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MASTODON-01 MKI**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 55 & 56)
* **Role:** Long-range terrestrial expeditionary mining locomotive, self-contained mobile Forward Operating Base (FOB), and **Genesis Seed Fleet Mothership** running **`RIG//OS-Lite`**. In fresh survival campaigns prior to Ground Base refinery commissioning, the lead Mastodon serves as the regional C2 fleet root, commanding subordinate excavators (`MAS-02`), bulk land trains (`CAT-01`), and freight shuttles (`MUL-01`), with automated handover to Ground Base upon commissioning.
* **Architecture:** Segmented articulated train consist:
  1. **Carriage 1 (`MAS-01-T`): Locomotive & Excavator Unit (11 W × 20 L × 5 H B · 6 Wheels):** Heavy tractive engine, 3-operator armored panoramic C2 bridge (Helm, Boom CTC, Tactical Director), primary power plant (30 MW nuclear), dual-PB compute rack (`PB Excavator` + `PB RIG//OS-Lite`), 40 km laser comms mast, 4× hydraulic corner outrigger stabilizers (bedrock voxel clamps), dorsal 45 m long-reach articulated 5-DOF laser mining boom, and aft 2-axis motorized drawbar hitch.
  2. **Carriage 2 (`MAS-01-U`): Modular FOB & Utility Carriage (11 W × 18 L × 5 H B · 4 Wheels):** Field workshop (1× Large Assembler, Survival Kit / Medical Bay), component reserve hold, atmospheric bunkering (2× Large H2 tanks, O2/H2 generators), 7 × 7 B MULE-01 Gantry Cradle Pad with motorized clamping arms, 2× KESTREL-01 scout drone landing pads, and stern heavy dump bumper matching CATERPILLAR-01.
* **Grid:** Large Grid (1 B = 2.5 m) · **Consist Envelope:** 11 W × 42 L × 6 H B (27.5 × 105.0 × 15.0 m).
* **Mass:** Consist Dry Mass `≈ 876.2 t` (Tractor: 499.8 t, Utility: 376.4 t) · Max Laden Mass `≈ 3,375 t` (Earth 1 g).
* **Payload Capacity:** 2,500 t bulk ore across tractor buffer and utility holds.
* **Extraction Gear:** Dorsal 5-DOF articulated boom with 2× Simple Laser Multitools (mining mode) supporting manual CTC aim and 3 automated macro modes (Bench quarry, shaft bore, trench carve).
* **Operating Environment:** Earth 1 g baseline, rough terrestrial terrain.

---

## 0 · Reference Grid, In-Game Paint Palette & Master Consist Layout

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the Tractor belly foundation at Level L1, along centerline X0.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. Spans X −5 to +5 (11 blocks wide = 27.5 m).
* **Y (Elevation):** Wheel hubs and belly skid line are Level L1 (Y = 0), advancing upward to Flight Roof at Level L6 (6 blocks high = 15.0 m).
* **Z (Length / Longitudinal):** Stern is negative (−), Bow is positive (+). Spans Z −26 to +12 (38 blocks vehicle body + 4 blocks hitch/bumpers = 42 blocks total = 105.0 m).
  * Tractor Carriage (`MAS-01-T`): Z −8 to +12 (20 blocks length = 50.0 m). Center axle datum is Z0.
  * Articulated Hitch Gap: Z −9 to −8 (1 block mechanical gap = 2.5 m).
  * Utility Carriage (`MAS-01-U`): Z −26 to −8 (18 blocks length = 45.0 m).

```
Spatial Alignment Triad:
       [+Y: Zenith / Flight Roof L6]
              │
              │   [+Z: Bow / Tractor Bridge & Arm]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Utility Carriage & Cat Dump]
              │
       [−Y: Ground Contact / Level L0]
```

---

### 0.2 In-Game Paint Palette & Functional Color Coding

To prevent builder disorientation across a 105 m long articulated consist, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Lower chassis box girders, wheel arch frames, belly skid plates |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Roof Mule Gantry Pad (7×7 B), cradle arms, outrigger warning chevrons |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Stern train dump bumper, rear alignment push-blocks, drawbar hitches |
| **Slot 3** | Excavator Ochre | `#E67E22` | Deep Orange | 45 m Long-Reach Articulated Boom, arm casings, laser tool head |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Ore Conveyor Spine, bow intake throat, manifold junctions |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Tractor ore buffer and Utility component storage holds |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Tractor Power Vault (2× Small Nuclear Reactors, 2× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Forward Armored C2 Bridge exterior shell, operator consoles |
| **Slot 8** | Scout Flight White | `#ECF0F1` | Clean White | Forward twin Kestrel scout drone landing pads (P1, S1) on Utility roof |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Exterior catwalks, maintenance ladders, corner outrigger foot pads |

---

### 0.3 Master Consist Top Plan View (Level L3-L6)

```
[TOP PLAN VIEW · MASTODON-01 MKI CONSIST · LEVEL L3-L6]

                      === TRACTOR LOCOMOTIVE (MAS-01-T) ===                               === UTILITY CARRIAGE (MAS-01-U) ===
    Port [-X]                                                                                                                       Port [-X]
        ┌─────────┬──────────────┬──────────────┬──────────────┬───────────────┐ ┌──────────────┬─────────────────────────────┬─────────────┐
        │ [O-R1]  │  Wheel A1-P  │  Wheel A2-P  │  Wheel A3-P  │  [O-R2]       │ │ Wheel B1-P   │  [KES Pad P]                │ Wheel B2-P  │
        │ Piston  │  (Steer 30°) │  (Steer 15°) │  (Drive 0°)  │  Piston       │ │ (Drive 0°)   │  4x4 sb Recon Pad           │ (Steer -25°)│
        ├─────────┴──────────────┴──────────────┴──────────────┴───────────────┤ ├──────────────┴─────────────────────────────┴─────────────┤
        │ [C2 BRIDGE]              [30 MW REACTOR]                             │ │ [FIELD WORKSHOP: Assembler / MedBay / O2]                │
Bow     │ 2-Seat Panoramic Cockpit  Nuclear Core                               │ │                                                   Stern   │
[+Z]    │       │                         │                                    │ │       [7 x 7 B MULE-01 GANTRY CRADLE PAD]          Cat   [-Z]
        │    [BOOM BASE] ── (45 m Articulated 5-DOF Laser Arm)                 │ │          (Flush Heavy Armour Blast Deck            Dump   │
        │    360° Turret + Shoulder/Elbow Hinges + Wrist Roll/Pitch            │ │           Motorized Lateral Clamping Rails)       Bumper  │
        │       │                         │                                    │ │                                                     │     │
        │ [SURVIVAL KIT]           [LARGE CARGO BUFFER]                        │ │ [COMPONENT RESERVE HOLD: Motors / Plates / Glass]   │     │
        ├─────────┬──────────────┬──────────────┬──────────────┬───────────────┤ ├──────────────┬─────────────────────────────┬─────────────┤
        │ [O-R3]  │  Wheel A1-S  │  Wheel A2-S  │  Wheel A3-S  │  [O-R4]       │ │ Wheel B1-S   │  [KES Pad S]                │ Wheel B2-S  │
        │ Piston  │  (Steer 30°) │  (Steer 15°) │  (Drive 0°)  │  Piston       │ │ (Drive 0°)   │  4x4 sb Recon Pad           │ (Steer -25°)│
        └─────────┴──────────────┴──────────────┴──────────────┴───────────────┘ └──────────────┴─────────────────────────────┴─────────────┘
    Stbd [+X]                                     ▲                                              ▲                                  Stbd [+X]
                                                  │                                              │
                                           [TRACTOR CHASSIS]                              [UTILITY CHASSIS]
                                                  └─────────────── [2-AXIS MOTOR DRAWBAR] ───────┘
                                                                   (Pitch Hinge + Yaw Rotor)
```

---

### 0.4 Master Profile Elevation (Looking Starboard, X = 0 Plane)

```
        ◄────────── CARRIAGE 1: LOCOMOTIVE / EXCAVATOR ──────────►     ◄────────── CARRIAGE 2: FOB / UTILITY MODULE ──────────►
                    (11 W × 20 L × 5 H B · 6 Wheels)                               (11 W × 18 L × 5 H B · 4 Wheels)
                     ┌─[ARTICULATED LASER BOOM]                                   ┌─────────────────┬─────────────────┐
                     │ (Deployed Reach: 35-45 m)                                  │ MULE GANTRY PAD │ 2× KESTREL PADS │  Flight Roof (L6)
        ┌────────────┴───┐                                                        │ (7×7 B Blast)   │ (Scout Wings)   │
        │ ARMORED BRIDGE │                                     [DRAWBAR HITCH]    ├─────────────────┴─────────────────┤
        │ (Panoramic C2) │   [POWER CORE]    [ORE BUFFER]     2-Axis Hinge Joint  │ FIELD WORKSHOP  │ COMPONENT HOLD  │  Main Deck (L3-L5)
[BOW]   │ [ORE DETECTOR] │   (2× Reactors)   (L Cargo Hold)    Conveyor Pass-Thru │ (Assembler/Med) │ (Spare Parts)   │  [STERN DUMP]
════════╧════════════════╧═══════════════════╧═════════════════════════╦══════════╧═════════════════╧═════════════════╧═══════════► (To Cat Train)
[STABILIZER]           [WH 1..2]           [WH 3..4]        [WH 5..6]  ║        [WH 7..8]         [WH 9..10]      [STABILIZER]
(Voxel Clamp)         (Steer +30°)        (Steer +15°)     (Drive 0°)  ║       (Drive 0°)        (Steer −25°)     (Voxel Clamp)
```

---

### 0.5 Master Conveyor Routing Schematic

```
               45 m ARTICULATED LASER EXCAVATION BOOM (Dorsal Pedestal)
                     [Tool Head: 2× Simple Laser Multitools]
                                         │
                                         ▼ (Internal Large Hinge / Rotor Pass-Thru)
                       [TRACTOR ORE BUFFER & SUMP] (3×3×3 B Large Cargo Hold)
                                         │
                                         ▼ (2-Axis Drawbar Articulated Conveyor Pipe)
                       [UTILITY FOB CONVEYOR DISTRIBUTION MANIFOLD]
                                         │
        ┌────────────────────────────────┼────────────────────────────────┐
        ▼ (Upward Riser Trunk)           ▼ (Internal Workshop Loop)       ▼ (Stern Bumper Pipe)
 [MAS MP Sorter] (Drain ON)       [Field Assembler & Tanks]        [MAS Stern Sorter] (Drain ON)
        │                                │                                │
        ▼                                ▼                                ▼
 [MAS MP Conn] (Roof Flush L6)    [O2/H2 Fueling Line]             [MAS Stern Dump Conn] (L2 Bumper)
 (650 t rare ore to Mule)                                          (2,500 t bulk ore to Cat Train)
```

---

### 0.6 Master Electrical Single-Line Schematic

```
             TRACTOR POWER CORE (2× Small Nuclear Reactors + 2× Large Batteries)
                                30 MW Nuclear · 6 MWh Battery
                                              │
                                              ▼
                                     PRIMARY TRACTIVE BUS
        ┌─────────────────────────────────────┼─────────────────────────────────────┐
        │                                     │                                     │
        ▼                                     ▼                                     ▼
 TRACTOR POWERTRAIN & HYD              ARTICULATED BOOM                   UTILITY CARRIAGE UMBILICAL
 6× 5×5 Wheel Drive Motors             Turret Pan Rotor (Azimuth)         2× Auxiliary Batteries (6 MWh)
 4× Corner Outrigger Cylinders         Shoulder & Elbow Pitch Hinges      Deployable Solar Wing Racks
 Steering Actuators (Axles 1 & 2)      Laser Mining Head (4.0 MW)         Field Assembler Core (560 kW)
                                                                          Mule Float Charger (12 MW)
```

---

### 0.7 Master Consist Bill of Materials (BOM) & Mass Budget

| System Subassembly | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Tractor Chassis & Hull** | Heavy armor full blocks, half-blocks, slopes (Palette 0) | 440 | 374.0 t | 440 |
| **Tractor Powertrain** | 6× 5×5 Heavy Suspension Wheels (Axles 1-3) | 6 | 48.0 t | 600 |
| **Corner Outriggers** | 4× Large Pistons + 4× Large Magnetic Plates (Palette 1) | 8 | 18.4 t | 260 |
| **Tractor Power Core** | 2× Small Nuclear Reactors, 2× Large Batteries (Palette 6) | 4 | 16.6 t | 90 |
| **Articulated Boom** | 2× Adv Rotors, 3× Motor Hinges, 2× Lasers, Girders (Palette 3) | 18 | 34.5 t | 920 |
| **Armored Bridge & C2** | 1× Cockpit, 2× Stations, 2× PBs, Laser Antenna, Glass, Detector | 18 | 15.8 t | 410 |
| **Drawbar Hitch Assembly** | 1× Adv Rotor, 1× Motor Hinge, Heavy Armor Joint Blocks | 6 | 12.0 t | 225 |
| **Utility Carriage Hull** | Heavy armor frames, wheel wells, deck armor (Palette 0) | 360 | 306.0 t | 360 |
| **Utility Powertrain** | 4× 5×5 Heavy Suspension Wheels (Axles 4-5) | 4 | 32.0 t | 400 |
| **Field Workshop & Hab** | 1× Large Assembler, 1× Survival Kit, 2× O2/H2 Tanks | 8 | 38.5 t | 540 |
| **Utility Cargo Hold** | 1× Large Cargo Container (Components), Conveyor network | 16 | 28.0 t | 160 |
| **Flight Decks & Dumps** | 1× Mule Gantry Cradle Pad (7×7 B), 2× Kestrel Pads, Stern Dump | 58 | 52.4 t | 480 |
| **TOTAL CONSIST (DRY)** | Complete Expeditionary Mining Locomotive & Mobile FOB | **946** | **≈ 876.2 t** | **4,885** |

---

## Phase 10-1 · Tractor Chassis & 6-Wheel Powertrain (Levels L1-L2)

```
       [PHASE 10-1 SPATIAL LOCATOR HUD]
       Envelope: 11 W × 20 L × 2 H B (27.5 × 50.0 × 5.0 m)
       Coordinate Range: X −5 to +5, Z −8 to +12, Y = L1 to L2
       Key Datums:
       · Tractor Centerline: X0, spanning Z −8 to +12
       · Longitudinal Box Girders: X = ±3
       · Axle 1 (Bow Steer): Z = +9 (Steer Angle: +30.0°)
       · Axle 2 (Mid Steer): Z = +4 (Steer Angle: +15.0°)
       · Axle 3 (Aft Drive): Z = −1 (Locked Drive: 0.0°)
       · Outrigger Piston Sockets: [X ±5, Z +11] and [X ±5, Z −6]
```

### Pre-Step Parts Tray: Phase 10-1
* 240× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 80× Heavy Armor Slope / Corner Blocks (Palette 0)
* 6× Wheel Suspension 5×5 (3 Port, 3 Starboard)
* 24× Large Conveyor Pipe (Palette 4: Processing Teal)
* 6× Large Conveyor Junction (Palette 4)
* 4× Large Piston (Palette 1: Hazard Amber)
* 4× Large Magnetic Plate (Palette 9: High-Viz Safety Yellow)

---

### Step 10.1 · Twin Longitudinal Chassis Box Girders & Belly Skids (L1-L2)

Weld two continuous, reinforced heavy-armor box girders spanning 20 blocks from Aft Bulkhead (Z −8) to Bow Bulkhead (Z +12) at X = ±3. Connect them with transverse beam ties at each axle datum to form the central conveyor raceway channel:

```
Plan view, Y = L2 (Tractor Chassis Box Girder Deck)
        X: −3          0          +3
    Z: +12 ■═══════════╤═══════════■  BOW BULKHEAD (Z +12 Datum)
           ║           │           ║  Central Conveyor Trough (X0)
    Z:  +9 ║  BEAM TIE │  BEAM TIE ║  ◄── Axle 1 Cross-Tie (X = ±3, Z +9)
           ║═══════════╪═══════════║
    Z:  +4 ║  BEAM TIE │  BEAM TIE ║  ◄── Axle 2 Cross-Tie (X = ±3, Z +4)
           ║═══════════╪═══════════║
    Z:  −1 ║  BEAM TIE │  BEAM TIE ║  ◄── Axle 3 Cross-Tie (X = ±3, Z −1)
           ║           │           ║
    Z:  −8 ■═══════════╧═══════════■  AFT HITCH BULKHEAD (Z −8 Datum)
```

1. Lay 2 parallel runs of Heavy Armor Full Blocks at X = −3 and X = +3 from Z −8 to Z +12 at Level L2.
2. At Level L1 (underside), install continuous Heavy Armor Half-Slopes facing downward along X0 to form a beveled belly skid channel that deflects boulders during overland travel.
3. Install transverse Heavy Armor Ties connecting the box girders at Z = +9, Z = +4, and Z = −1.
4. Route the large conveyor raceway along X0 at Level L1, securing pipes with conveyor junctions at Z = +9, Z = +4, and Z = −1 for future branch taps.

---

### Step 10.2 · 5×5 Off-Road Suspension Wheel Mounts & Master 3-Axle Tuning (L1)

Mount 3 pairs of 5×5 large suspension wheel units outward along the chassis girders at X = ±4. Configure steering geometry: front two axles steer in proportion to wheel turn; rear axle 3 remains locked at 0° to prevent sideways tire scrub:

```
Plan view, Y = L1 (Tractor Wheel Suspension Layout)
        X: −5          0          +5
    Z:  +9 [WH 1 (P)] ═══════════ [WH 2 (S)]   ◄── Axle 1 (+30.0° Steer)
    Z:  +4 [WH 3 (P)] ═══════════ [WH 4 (S)]   ◄── Axle 2 (+15.0° Steer)
    Z:  −1 [WH 5 (P)] ═══════════ [WH 6 (S)]   ◄── Axle 3 (0.0° Locked Drive)
```

```
Front Elevation Cross-Section, Y = L1 (Axle 1 Wheel Hub & Ground Clearance)
    Port [-X]                                                          Starboard [+X]
    X: -5            X: -3            X0              X: +3            X: +5
    [WHEEL 1] ────── [GIRDER] ─── [CONVEYOR] ─── [GIRDER] ────── [WHEEL 2]
    │ 5x5 Hub        │ Heavy      │ Large Pipe   │ Heavy        │ 5x5 Hub
    │                └────────────┴──────────────┴──────────────┘
    ▼                             ▼ Belly Skid (1.25 m Clearance)
    ════════════════════════════════════════════════════════════════════ Terrain
```

| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 1** | Z = +9  | `WH 1, 2` | `30.0°` | Normal | `100%` | `50% / 85%` | `70%` |
| **Axle 2** | Z = +4  | `WH 3, 4` | `15.0°` | Normal | `100%` | `50% / 85%` | `70%` |
| **Axle 3** | Z = −1  | `WH 5, 6` | `0.0°`  | OFF (Locked) | `100%` | `55% / 90%` | `75%` |

#### Pre-Enclosure Inspection Gate 10.1A: Tractor Powertrain & Belly Clearance Check
* [ ] Verify uninterrupted large conveyor pipe flow from Bow Throat (Z +12) to Aft Hitch (Z −8).
* [ ] Verify exactly 1.25 m (0.5 B) ground clearance under belly skids at 50% suspension travel.
* [ ] Confirm steering response in `MAS Tractor Wheels`: Axles 1 and 2 pivot smoothly with zero wheel well binding.

```
PHASE 10-1 VISUAL MILESTONE · TRACTOR CHASSIS & POWERTRAIN COMPLETE
       Z: −1 (Axle 3)         Z: +4 (Axle 2)         Z: +9 (Axle 1)
           [WH 5]                 [WH 3]                 [WH 1]
     ═══════╧══════════════════════╧══════════════════════╧═══════
                 20 B CONTINUOUS HEAVY ARMOR BOX GIRDER RUN (L2)
```

---

## Phase 10-2 · Tractor Power Core, Ore Buffer & 2-Axis Drawbar Hitch (Levels L2-L4)

```
       [PHASE 10-2 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 14 L × 3 H B (17.5 × 35.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z −8 to +6, Y = L2 to L4
       Subsystem Datums:
       · Ore Buffer Hold: Z +1 to +3, centered at X0
       · Primary Power Core: Z −4 to −2, centered at X0
       · 2-Axis Drawbar Hitch Center: [X0, Z −8, Y = L2]
```

### Pre-Step Parts Tray: Phase 10-2
* 2× Small Nuclear Reactor (15 MW each, Palette 6: High-Voltage Red)
* 2× Large Battery (3 MWh each, Palette 6)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 1× Large Advanced Rotor (Palette 2: Heavy Rail Crimson)
* 1× Large Motor Hinge (Palette 2)
* 18× Large Conveyor Pipe & 6× Curved Conveyor Tube (Palette 4: Processing Teal)
* 120× Heavy Armor Full Block & Slopes (Palette 0)

---

### Step 10.3 · Primary Power Core Vault & Electrical Shielding (Z −4 to −2, L2-L4)

Install 2 Small Nuclear Reactors and 2 Large Batteries behind the ore buffer hold. Enclose the plant in double-walled heavy armor bulkheads (Palette 6):

```
Plan view, Y = L3 (Tractor Power Core Vault)
        X: −2     0    +2
    Z: −2  ┌───────────────┐
           │[BAT 1][R 1][BAT 2]│  ◄── Reactor 1 (15 MW) flanked by Large Batteries
    Z: −3  │[Armor][R 2][Armor]│  ◄── Reactor 2 (15 MW) enclosed in blast plate
    Z: −4  └───────────────┘
           Nuclear Core Total: 30.0 MW · Battery Reserve: 6.0 MWh
```

1. Place Reactor 1 (`MAS Reactor 1`) at [X0, Y = L3, Z −2] and Reactor 2 (`MAS Reactor 2`) at [X0, Y = L3, Z −3].
2. Flank the reactors with Large Batteries (`MAS Battery 1, 2`) at X = −2 and X = +2.
3. Connect reactor conveyor ports directly to the central conveyor spine along X0 for automated uranium fuel feeding.
4. Encase the compartment in Heavy Armor blocks painted Palette Slot 6 (High-Voltage Alert Red).

---

### Step 10.4 · Tractor Ore Buffer Hold (Z +1 to +3, L2-L4)

Mount 1 standard 3×3×3 B Large Cargo Container (`MAS Tractor Buffer`) on centerline X0. It receives raw ore directly from the boom intake pipe and buffers material during active excavation before routing to the trailing utility carriage:

```
Plan view, Y = L3 (Tractor Buffer Hold)
        X: −1    0   +1
    Z: +3  ┌───────────┐
           │MAS TRACTOR│  ◄── Large Cargo Container (3×3×3 B)
    Z: +1  │  BUFFER   │      (Palette Slot 5: Logistics Emerald)
           └───────────┘
```

1. Position the Large Cargo Container between Z +1 and Z +3, spanning X −1 to +1 and Y = L2 to L4.
2. Align bottom and top 3×3 large conveyor ports:
   * Bottom port connects directly to the central chassis conveyor trough (L1).
   * Top port aligns with the dorsal boom turret riser (L4).
   * Aft port connects to the power core uranium supply line and drawbar hitch pipe.

---

### Step 10.5 · Heavy Motorized 2-Axis Drawbar Hitch (Z = −8, X0, L2)

At the aft bulkhead of the tractor, install a heavy-duty 2-axis articulated drawbar hitch that connects the tractor to the utility carriage mechanically and routes the main large conveyor line:
1. **Azimuth (Yaw) Joint:** 1× Large Advanced Rotor mounted at Z = −8 facing Aft (−Z). Allows the consist to articulate horizontally when turning.
2. **Pitch Joint:** 1× Motorized Large Hinge connected directly to the rotor head. Allows the consist to traverse crests and dips over rough terrain without lifting wheels.
3. **Settings:** Share Inertia Tensor = `ON` · Braking Torque = `45 MN·m`.

```
2.5D Isometric Callout · 2-Axis Articulated Drawbar Hitch
    Tractor Aft Bulkhead (Z −8)
           ║
           ╠══[ADVANCED ROTOR] (Horizontal Yaw Articulation: −45° to +45°)
           ║        │
           ║        ▼
           ║   [MOTOR HINGE] (Vertical Pitch Articulation: −25° to +25°)
           ║        │
           ║        ▼
           ╚════════╪═════════════════════════════════════════► To Utility Carriage (Carriage 2)
                    ▼
           [LARGE CONVEYOR PASS-THROUGH PIPE]
```

```
Hitch Articulation Kinematic Envelope:
· Horizontal Yaw (Turning): Rotor limits set to Min: -45.0°, Max: +45.0°.
  Torque: 45 MN·m · Velocity: 0.5 rpm · Damping: 85%.
· Vertical Pitch (Crest / Dip): Hinge limits set to Min: -25.0°, Max: +25.0°.
  Torque: 45 MN·m · Velocity: 0.5 rpm · Damping: 90%.
· Roll (Twist): Locked rigid via structural heavy armor wishbone; terrain twist absorbed by wheel suspension travel.
```

#### Pre-Enclosure Inspection Gate 10.2A: Power Core Isolation & Hitch Articulation Test
* [ ] Verify reactor automated uranium pull through the central conveyor trunk.
* [ ] Confirm total tractor power capacity reads `54.0 MW` peak (30 MW nuclear + 24 MW battery).
* [ ] Test 2-axis drawbar hitch: ensure rotor and hinge pivot freely without clashing with the chassis frame.

---

## Phase 10-3 · Hydraulic Bedrock Outriggers & 45 m Long-Reach Excavation Boom (Levels L2-L8)

```
       [PHASE 10-3 SPATIAL LOCATOR HUD]
       Envelope: 15 W × 22 L × 6 H B (37.5 × 55.0 × 15.0 m operating envelope)
       Coordinate Range: X −7 to +7, Z +5 to +22, Y = L2 to L8
       Pedestal Datum: [X0, Z +6, Y = L4] (Dorsal Turret Ring)
       Outrigger Locations:
       · Port Forward Outrigger: [X −5, Z +11, Y = L1]
       · Starboard Forward Outrigger: [X +5, Z +11, Y = L1]
       · Port Aft Outrigger: [X −5, Z −6, Y = L1]
       · Starboard Aft Outrigger: [X +5, Z −6, Y = L1]
```

### Pre-Step Parts Tray: Phase 10-3
* 4× Large Piston (Palette 1: Hazard Amber)
* 4× Large Magnetic Plate (Palette 9: High-Viz Safety Yellow)
* 2× Large Advanced Rotor (Palette 3: Excavator Ochre)
* 3× Large Motor Hinge (Palette 3)
* 2× Simple Laser Multitool (Mining Mode, Palette 3)
* 1× Camera + 2× Spotlight + 1× Sensor (Tool Head)
* 40× Large Conveyor Pipe (Palette 4: Processing Teal)
* 80× Heavy Armor Slope & Box Girder Blocks (Palette 3)

---

### Step 10.6 · 4× Corner Bedrock Hydraulic Outriggers (L1-L2)

Install 4 hydraulic stabilizer legs at the outer corners of the tractor chassis. Each outrigger consists of a downward-facing Large Piston capped with a Large Magnetic Plate:
* Piston Settings: Velocity = `0.2 m/s` · Share Inertia Tensor = `ON`.
* Magnetic Plate: Auto-Lock = `ON`.
* Group: `MAS Outriggers`.
* **Operating Logic:** When deployed on site, pistons extend downward, pressing the magnetic plates into planet voxels and locking rigidly. This completely isolates the suspension, preventing any tipping or chassis flex during boom sweeps.

```
Front Cross-Section, Outriggers Deployed at Bedrock Baseline:
     [OUTRIGGER PORT]                                            [OUTRIGGER STBD]
       [PISTON ↓]                                                  [PISTON ↓]
     [MAG PLATE P] ─── [WH 1] ─── [BELLY SKIDS] ─── [WH 2] ─── [MAG PLATE S]
    ══════════════════════════════════════════════════════════════════════════ Terrain
```

```
Outrigger Deployment Sequence:
1. Chassis comes to complete stop. Handbrake = ON.
2. Wheel suspensions override: Strength = 100%, Friction = 100%.
3. Outrigger Pistons extend: Max distance = 2.5 m (1 B stroke).
4. Magnetic Plates contact terrain: Auto-Lock triggers (Green lock state).
5. Event Controller `MAS EC Stabilizers` verifies 4/4 locks -> releases Boom Joint brakes.
```

---

### Step 10.7 · Heavy Turret Pedestal & Azimuth Advanced Rotor (Z = +6, X0, L4)

Mount an Advanced Rotor facing upward (+Y) on the dorsal deck at Z = +6. This serves as the azimuth turntable, providing **360° continuous horizontal rotation** for the excavation boom with an unrestricted large-conveyor pass-through:
* Rotor Settings: Share Inertia Tensor = `ON` · Braking Torque = `50 MN·m` · Velocity = `0.75 rpm`.

---

### Step 10.8 · 5-DOF Articulated Boom Kinematic Chain (L4-L8)

Assemble the high-articulation robotic excavation crane:
1. **Shoulder Pitch Hinge:** Motorized Large Hinge connected to the azimuth rotor head. Pitch limits: `−20.0°` (undercutting terrain) to `+75.0°` (high reach).
2. **Main Boom Arm (7 B Span):** Boxed heavy armor casing enclosing the primary large conveyor pipe (17.5 m length).
3. **Elbow Pitch Hinge:** Motorized Large Hinge at the mid-joint. Articulation limits: `−100.0°` (folded tuck) to `+45.0°` (extended reach).
4. **Forearm Girder (5 B Span):** Tapered structural girder carrying internal piping and utility cabling (12.5 m length).
5. **Wrist 2-Axis Articulator:**
   * Wrist Roll: Advanced Rotor providing `±180°` tool roll.
   * Wrist Pitch: Motorized Large Hinge providing `−90°` to `+90°` tool attack angle leveling.

```
2.5D Isometric Callout · 45 m Long-Reach Articulated Boom Architecture

       Chassis Pedestal (Z +6, X0, L4)
              ║
              ╠══[TURRET AZIMUTH ROTOR] ────────► 360° Continuous Horizontal Pan
              ║        │
              ║        ▼
              ║   [SHOULDER PITCH HINGE] ───────► Elevation: −20° to +75°
              ║        │
              ║        ▼
              ║   [Main Boom Arm · 7 B Span] (Heavy Cased Conveyor Pipe)
              ║        │
              ║        ▼
              ║   [ELBOW PITCH HINGE] ──────────► Articulation: −100° to +45°
              ║        │
              ║        ▼
              ║   [Forearm Girder · 5 B Span]
              ║        │
              ║        ▼
              ║   [WRIST ROLL ROTOR] ───────────► Tool Roll: ±180°
              ║        │
              ║        ▼
              ║   [WRIST PITCH HINGE] ──────────► Tool Pitch: −90° to +90°
              ║        │
              ║        ▼
              ║   [LASER MINING TOOL HEAD]
              ║   (2× Simple Laser Multitools + Camera + Floodlights)
```

```
Boom Operating Configurations:

1. STOWED TRANSIT CONFIGURATION (Traveling Overland):
   Turret Azimuth: 0.0° (Facing Forward +Z)
   Shoulder Pitch: -15.0° (Resting on forward cab crutch)
   Elbow Pitch:    -95.0° (Forearm folded flat along dorsal spine)
   Wrist Pitch:     0.0° (Tool head locked in protective armored cradle)
   Total Height:    5.2 m above ground (Fits under standard hangar gates)

2. FULL EXTENDED REACH CONFIGURATION (Max Reach Excavation):
   Turret Azimuth: Sweeping 360°
   Shoulder Pitch: +45.0°
   Elbow Pitch:    +20.0°
   Wrist Pitch:    -65.0° (Tool head leveled parallel to ground)
   Horizontal Radius: 42.5 m from turret center · Max Depth Reach: -12.5 m
```

---

### Step 10.9 · Laser Mining Tool Head & Sensor Suite

At the tip of the wrist articulator, mount the unified excavation head:
* 2 × Simple Laser Multitool (Mode: `Mining Beam`) · `MAS Laser 1, 2`.
* 1 × Camera · `MAS Cam Boom` (high-magnification targeting crosshairs).
* 2 × Spotlight · `MAS Light Boom 1, 2` (floodlights illuminating the cutting bench).
* 1 × Sensor · `MAS Sensor Boom` (5 m safety stop to prevent head-to-terrain collision).

```
Front Elevation · Laser Mining Tool Head (Tip of Forearm)
                  [WRIST PITCH HINGE]
                           │
       ┌───────────────────┴───────────────────┐
       │               [CAMERA]                │
       │             MAS Cam Boom              │
       ├───────────────────┬───────────────────┤
       │  [LASER 1]        │        [LASER 2]  │
       │  Simple Laser     │        Simple Laser
       │  Multitool (Mine) │        Multitool  │
       ├───────────────────┼───────────────────┤
       │  [SPOTLIGHT 1]    │    [SPOTLIGHT 2]  │
       └───────────────────┴───────────────────┘
```

### Step 10.9B · Automated Excavation Macro Modes & Kinematic Sweep Sequences

To maximize extraction efficiency and support uncrewed autonomous operation, `[MAS] PB Excavator` implements 3 automated excavation sweep macros:

```
[AUTOMATED BOOM EXCAVATION SWEEP PATTERNS]

1. BENCH QUARRY MODE (Horizontal Terracing)
   · Azimuth Turret sweeps across horizontal face: -45.0° (Port) to +45.0° (Starboard).
   · Shoulder Hinge incrementally steps down -2.5° per pass (0.5 B bench cut depth).
   · Wrist Pitch automatically levels tools to maintain flat, drivable terrace floors.
   · Produces clean stair-stepped quarry cuts into mountainsides.

2. VERTICAL SHAFT BORE MODE (Subterranean Access)
   · Azimuth Turret rotates continuously at 0.5 rpm.
   · Shoulder and Elbow hinges articulate in synchronized descent vector.
   · Carves a precise 12.5 m diameter circular vertical shaft down to -25.0 m depth.
   · Enables extraction of deep uranium/platinum veins below surface overburden.

3. TRENCH & ROAD CARVE MODE (Overland Right-of-Way)
   · Boom deploys along centerline (Z+), executing downward scoop arc (-5.0 m to grade).
   · Tractor crawls forward at 2.0 km/h; outriggers cycle in creeping advance pattern.
   · Automatically clears and grades a level 15.0 m wide roadbed for Caterpillar land trains.
```

#### Pre-Enclosure Inspection Gate 10.3A: Outrigger Locking & Boom Joint Inertia Verification
* [ ] Verify all 4 corner outrigger magnetic plates lock firmly into terrain voxels.
* [ ] Confirm `Share Inertia Tensor = ON` across all 6 boom joints (Turret Rotor, Shoulder Hinge, Elbow Hinge, Wrist Rotor, Wrist Hinge).
* [ ] Test 360° pan and full extension: boom must achieve a 45.0 m reach radius without inducing chassis tip or physics jitter.

```
PHASE 10-3 VISUAL MILESTONE · OUTRIGGERS ANCHORED & BOOM DEPLOYED
                             ▲ 45 m Elevation Reach
                           ╱   ╲
     [ARMORED BRIDGE]    ╱       ╲  [ELBOW HINGE]
           ║           ╱           ╲
           ╠══[TURRET AZIMUTH]       ╲
           ║                           ╲ ──► [LASER MINING HEAD]
     ══════╩═════════════════════════════╩═════════════════════════
     [OUTRIGGER PAD: LOCKED]           [OUTRIGGER PAD: LOCKED]
```

---

## Phase 10-4 · Forward Armored C2 Bridge & Fleet Avionics (Levels L3-L5)

```
       [PHASE 10-4 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 6 L × 3 H B (17.5 × 15.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z +7 to +12, Y = L3 to L5
       Station Datums:
       · Station 1 · Helm Pilot: [X −2, Z +10, Y = L4] (Port)
       · Station 2 · Excavation Master: [X0, Z +10, Y = L4] (Centerline)
       · Station 3 · Tactical Fleet Director: [X +2, Z +10, Y = L4] (Starboard)
       · Deep Ore Detector: [X0, Z +8, Y = L3]
       · C2 Compute Rack: [X0, Z +7, Y = L3] (Dual PBs + AI Automaton blocks)
       · Comms Mast: [X −2, Z +7, Y = L6] (Laser Antenna + 50 km Radio)
       · Survival Kit: [X −2, Z +8, Y = L4]
```

### Pre-Step Parts Tray: Phase 10-4
* 1× Cockpit (`MAS Helm`, Palette 7: Fleet Command Navy)
* 2× Control Station (`MAS Station Boom`, `MAS Station Fleet`, Palette 7)
* 2× Programmable Block (`[MAS] PB Excavator`, `[MAS] PB RIG//OS-Lite`, Palette 7)
* 3× AI Automaton Block (`MAS AI Move`, `MAS AI Task`, `MAS AI Defense`)
* 1× Laser Antenna (`MAS Comm Laser`, Palette 7)
* 1× Radio Antenna (`MAS Comm Radio`, 50 km broadcast, Palette 7)
* 1× Large Ore Detector (150 m subterranean range, Palette 4)
* 1× Survival Kit (Tractor Field Respawn, Palette 7)
* 14× Sloped Heavy Armor Glass (Bridge canopy)
* 6× LCD Panel (Tactical video wall and console telemetry HUDs)
* 1× Air Vent + 1× Small O2 Tank (Pressurization)

---

### Step 10.10 · 3-Operator Panoramic C2 Command Bridge & Tactical Avionics

Construct the forward command bridge overlooking the terrain. Level L3 accommodates the deep ore detector and avionics compute rack; Level L4 accommodates the 3-operator command deck:
1. **Station 1 (Port): Helm Pilot (`MAS Helm`):** Controls chassis traction, 3-axle steering, outrigger deploy/retract sequences, and hill-climbing crawl.
2. **Station 2 (Center): Excavation Master (`MAS Station Boom`):** Controls the 45 m articulated boom via Custom Turret Controller (CTC) mouse aim, tool head camera, and automated macro sweep execution.
3. **Station 3 (Starboard): Tactical Fleet Director (`MAS Station Fleet`):** Runs `RIG//OS-Lite` fleet coordination, dispatching Caterpillar land trains, calling Mule shuttles, receiving Kestrel scout coordinates, and monitoring ore quotas.
4. **C2 Compute Rack (Level L3):** Houses dual Programmable Blocks (`[MAS] PB Excavator` and `[MAS] PB RIG//OS-Lite`) along with AI Automaton blocks for uncrewed autonomous operation.
5. **Dorsal Comms Mast (Level L6):** Houses the 40 km Laser Antenna (`MAS Comm Laser`) and 50 km Radio Antenna (`MAS Comm Radio`).

```
Plan view, Y = L4 (Bridge Operator Deck & Tactical Video Wall)
        X: −2          0          +2
    Z: +11 ┌───────────────────────────┐
           │ [HELM]   [BOOM CTC]  [FLEET]│  ◄── 3-Operator Consoles (Navy Blue)
    Z: +9  │ PILOT     EXCAVATE   TACTIC │      Reinforced 45° Sloped Armor Glass
           ├───────────┬───────────┬───┤
    Z: +8  │ [SURV KIT]│ [TAC LCD] │LCD│  ◄── Survival Kit (Port) / 3-Panel Video Wall
           └───────────┴─────┬─────┴───┘
    Z: +7  [ORE DETECTOR]    │ [C2 COMPUTE RACK] (L3 Core: Dual PBs + AI Blocks)
```

```
3-Operator Bridge Hotbar Setup:

Station 1: Helm Pilot (`MAS Helm`)
· Slot 1: Group `MAS Outriggers` -> Reverse (Deploy / Retract)
· Slot 2: Group `MAS Outrigger Pads` -> Switch Lock (Voxel Lock)
· Slot 3: Handbrake -> Toggle
· Slot 4: Group `MAS Tractor Wheels` -> Propulsion ON/OFF
· Slot 5: Group `MAS Tractor Wheels` -> Increase Strength (+5%)
· Slot 6: Group `MAS Tractor Wheels` -> Decrease Strength (-5%)
· Slot 7: Event Controller `MAS EC Stabilizers` -> Trigger
· Slot 8: Antenna `MAS Comm Radio` -> Broadcast ON/OFF
· Slot 9: Connector `MAS MP Conn` -> Lock / Unlock

Station 2: Excavation Master (`MAS Station Boom`)
· Slot 1: Custom Turret Controller `MAS CTC Boom` -> Control (Mouse Aim)
· Slot 2: Group `MAS Lasers` -> Shoot ON/OFF
· Slot 3: Camera `MAS Cam Boom` -> View
· Slot 4: Group `MAS Boom Lights` -> ON/OFF
· Slot 5: Boom Macro 1: Bench Quarry Mode (Run Timer `MAS Timer Bench`)
· Slot 6: Boom Macro 2: Vertical Shaft Bore (Run Timer `MAS Timer Bore`)
· Slot 7: Boom Macro 3: Trench Carve Mode (Run Timer `MAS Timer Trench`)
· Slot 8: Boom Preset: Stowed Transit (Run Timer `MAS Timer Stow`)
· Slot 9: Boom Preset: Emergency Stop / Joint Brakes ON

Station 3: Tactical Fleet Director (`MAS Station Fleet`)
· Slot 1: IGC Send: `TMS.MULE.CALL;MAS-01;RARE_ORE` (Request Air Ferry)
· Slot 2: IGC Send: `TMS.CAT.CALL;MAS-01;BULK_ORE` (Request Bulk Land Train)
· Slot 3: Sorter `MAS MP Sorter` -> Drain All ON/OFF
· Slot 4: Sorter `MAS Stern Sorter` -> Drain All ON/OFF
· Slot 5: Laser Antenna `MAS Comm Laser` -> Connect Ground Base / Drone
· Slot 6: LCD Video Wall -> Cycle Page (Fleet Map / Quotas / Holds)
· Slot 7: Remote Control `MAS-02` (Subordinate Excavator Slave Link)
· Slot 8: Remote Control `CAT-01` (Caterpillar Land Train Slave Link)
· Slot 9: Event Controller `MAS EC Handover` -> Trigger C2 Base Handover
```

#### Pre-Enclosure Inspection Gate 10.4A: Bridge Airtightness & C2 Network Link Check
* [ ] Confirm bridge cabin pressurizes to 100% O2 via air vent.
* [ ] Verify `MAS Ore Detector` displays subterranean ore deposits out to 150 m range.
* [ ] Confirm CTC block maps mouse yaw to `MAS Turret Azimuth` and mouse pitch to `MAS Shoulder Pitch`.
* [ ] Confirm `MAS Comm Laser` and `MAS Comm Radio` establish two-way IGC telemetry ping.

---

## Phase 10-5 · Modular FOB & Utility Carriage Chassis & Workshop (Carriage 2)

```
       [PHASE 10-5 SPATIAL LOCATOR HUD]
       Envelope: 11 W × 18 L × 5 H B (27.5 × 45.0 × 12.5 m)
       Coordinate Range: X −5 to +5, Z −26 to −8, Y = L1 to L5
       Carriage Datums:
       · Front Drawbar Tongue: [X0, Z −8, Y = L2]
       · Field Workshop Core: Z −18 to −14, X −3 to +3
       · Component Hold: Z −22 to −19, X0
       · Axle 4 (Mid Drive): Z −13 (Locked: 0.0°)
       · Axle 5 (Aft Steer): Z −22 (Counter-Steer: −25.0°)
```

### Pre-Step Parts Tray: Phase 10-5
* 200× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 4× Wheel Suspension 5×5 (2 Port, 2 Starboard)
* 1× Large Assembler (Palette 4: Processing Teal)
* 1× Medical Bay + 1× Cryo Chamber (Palette 7)
* 2× Large Hydrogen Tank (Palette 6)
* 2× O2/H2 Generator (Palette 4)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 16× Large Conveyor Pipe & 6× Conveyor Junction (Palette 4)

---

### Step 10.11 · Utility Carriage Box Girders & 4-Wheel Powertrain (L1-L2)

Construct the trailing chassis frame connected to the tractor hitch tongue at Z = −8. Mount 2 pairs of 5×5 large suspension wheels:
* **Axle 4 (Z −13):** Fixed drive axle (0.0° steer).
* **Axle 5 (Z −22):** Counter-steering axle (−25.0° inverted) to eliminate trailer corner-cutting during tight turns.

```
Plan view, Y = L1 (Utility Carriage 4-Wheel Layout)
        X: −5          0          +5
    Z: −13 [WH 7 (P)] ═══════════ [WH 8 (S)]   ◄── Axle 4 (0.0° Locked Drive)
    Z: −22 [WH 9 (P)] ═══════════ [WH 10 (S)]  ◄── Axle 5 (−25.0° Inverted Counter-Steer)
```

| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 4** | Z = −13 | `WH 7, 8`  | `0.0°`  | OFF (Locked) | `100%` | `50% / 90%` | `75%` |
| **Axle 5** | Z = −22 | `WH 9, 10` | `25.0°` | INVERTED     | `100%` | `55% / 90%` | `80%` |

---

### Step 10.12 · Mobile FOB Field Workshop & Medical Suite (L2-L4)

Outfit Carriage 2 as a self-sustaining expeditionary base:
1. **Field Workshop Assembler (`MAS Utility Assembler`):** Large Assembler configured to fabricate replacement plates, drill components, and motors directly in the field.
2. **Medical & Survival Room (`MAS Medical Bay`):** Pressurized crew habitat with Survival Kit, Cryo Chamber, and Medical Room for expedition respawn.
3. **Atmospheric Tank Farm:** 2× Large Hydrogen Tanks (refuels Kestrel scouts and Mule ferries) + 2× O2/H2 Generators.
4. **Field Component Storage (`MAS Hold Components`):** 1× Large Cargo Container (3×3×3 B) dedicated to stocking construction parts.

```
Plan view, Y = L3 (Utility Carriage Interior Deck)
        X: −3          0          +3
    Z: −10 ═══════[DRAWBAR TONGUE]═══════  ◄── Articulated Hitch from Tractor
           │                             │
    Z: −14 │ [ASSEMBLER] │ [MEDICAL BAY] │  ◄── Field Workshop Core
           ├─────────────┼───────────────┤
    Z: −18 │ [H2 TANK 1] │ [H2 TANK 2]   │  ◄── Fuel Bunkering Farm
           ├─────────────┴───────────────┤
    Z: −22 │ [MAS HOLD COMPONENTS (3×3×3)│  ◄── Spare Parts Storage (Palette 5)
    Z: −26 └─────────────────────────────┘
```

#### Pre-Enclosure Inspection Gate 10.5A: Field Workshop Fabrication & Component Flow Check
* [ ] Verify `MAS Utility Assembler` pulls ingots and queues components without manual transfer.
* [ ] Confirm crew medical bay allows successful respawn and O2/H2 replenishment.
* [ ] Confirm O2/H2 tanks maintain airtight seals and supply hydrogen lines.

---

## Phase 10-6 · Flight Decks & Logistics Offload Interfaces (Levels L5-L6)

```
       [PHASE 10-6 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 18 L × 2 H B (17.5 × 45.0 × 5.0 m)
       Coordinate Range: X −3 to +3, Z −26 to −8, Y = L5 to L6
       Deck Datums:
       · Mule Gantry Cradle Pad: [X0, Z −22, Y = L6] (7×7 B Deck)
       · Kestrel Scout Pad Port: [X −3, Z −12, Y = L5]
       · Kestrel Scout Pad Starboard: [X +3, Z −12, Y = L5]
       · Stern Dump Connector: [X0, Z −26, Y = L2] (Centerline height = 2.5 m)
```

### Pre-Step Parts Tray: Phase 10-6
* 49× Heavy Armor Full Block (Palette 1: Hazard Amber · Mule Pad Deck)
* 2× Motor Hinge + 2× Large Magnetic Plate (Palette 1 · Cradle Clamp Arms)
* 1× Large Connector (`MAS MP Conn`, Palette 4)
* 2× Small-Grid Connector (`MAS Kestrel Dock 1, 2`, Palette 8: Scout White)
* 1× Large Connector (`MAS Stern Dump Conn`, Palette 2: Rail Crimson)
* 2× Large Conveyor Sorter (`MAS MP Sorter`, `MAS Stern Sorter`, Palette 4)
* 12× Heavy Armor Slopes & Alignment Bumpers (Palette 2)

---

### Step 10.13 · Roof Mule Receiving Gantry Cradle Pad (Z −25 to −19, L6)

Install the standardized 7 × 7 B Heavy Armor blast deck on the utility carriage roof. Equipped with twin motorized hinge arms and magnetic plates matching Decision 45:
* Inbound MULE-01 touches down; cradle arms swing 30° inward, clamping the Mule's chassis rub rails.
* Center belly connector (`MAS MP Conn`) locks. Sorter `MAS MP Sorter` (Drain All = ON) drains rare ores (U, Pt, Au, Mg) into the Mule's hold for immediate air transport to base.

```
2.5D Isometric Callout · Mule Gantry Cradle on Utility Roof (L6)
                         [MULE-01 HULL 7×7 B]
                                  │
      Port Flank                  ▼               Starboard Flank
    [CLAMP RUB RAIL]                        [CLAMP RUB RAIL]
           ▲                                       ▲
           │                                       │
      [MAG PLATE]  ◄── 30° Swing Inward ──►  [MAG PLATE]
           │                                       │
      [MOTOR HINGE]                           [MOTOR HINGE]
      (Base: X −3, Z −22)                     (Base: X +3, Z −22)
    ═══════╧═══════════════════════════════════════╧═══════
                      FLUSH HEAVY BLAST ROOF (L6)
                             [MAS MP Conn]
```

```
Plan view, Y = L6 (7×7 B Mule Gantry Blast Deck)
        X: −3           0          +3
    Z: −19 ■════════════╤═══════════■
           ║  Hazard    │  Hazard   ║  Palette Slot 1: Hazard Amber
    Z: −22 [CLAMP P] [MAS MP CONN] [CLAMP S]  ◄── Motorized Clamping Hinges
           ║  Chevrons  │  Chevrons ║
    Z: −25 ■════════════╧═══════════■
```

---

### Step 10.14 · Twin Kestrel Prospector Scout Drone Landing Pads (Z −13 to −11, L5)

Recess two small landing pads forward of the Mule pad on the utility roof:
* `MAS Kestrel Dock 1` (Port, X −3) & `MAS Kestrel Dock 2` (Starboard, X +3).
* Small-grid connectors flush with deck for recharging and downloading geological vein scans.
* Painted Palette Slot 8 (Scout Flight White) with perimeter inset yellow lighting.

---

### Step 10.15 · Stern Bumper & Caterpillar Land Train High-Speed Dump Port (Z = −26, L2)

Install the rear collision push-bumper and large connector (`MAS Stern Dump Conn`) at Level L2:
* Mates directly with the bow connector of the CATERPILLAR-01 tractor locomotive.
* Flanked by dual Heavy Armor angled guide wedges (Palette 2: Heavy Rail Crimson) to funnel the incoming locomotive into positive alignment.
* Sorter `MAS Stern Sorter` drains bulk ores (Fe, Ni, Si, Stone) at ultra-high transfer rates (< 90 seconds for 2,500 t bulk payload).

```
Rear Elevation Cross-Section, Y = L2 (Stern Caterpillar Dump Interface)
    Port [-X]                                                          Starboard [+X]
    X: -3            X: -1            X0              X: +1            X: +3
    [GUIDE WEDGE] ── [PUSH BLOCK] ─ [STERN CONN] ─ [PUSH BLOCK] ── [GUIDE WEDGE]
    │ Heavy Slope    │ Solid Block  │ MAS Stern     │ Solid Block  │ Heavy Slope
    │ Crimson        │ Crimson      │ Dump Conn     │ Crimson      │ Crimson
    └────────────────┴──────────────┴───────────────┴──────────────┴─────────────┘
```

#### Pre-Enclosure Inspection Gate 10.6A: Cradle Clamping Dynamics & Dual-Tier Sorter Routing
* [ ] Verify `MAS MP Sorter` whitelist passes only rare ores (U, Pt, Au, Mg) to the Mule shuttle.
* [ ] Verify `MAS Stern Sorter` whitelist passes bulk ores (Fe, Ni, Si, Stone) to the land train.
* [ ] Test cradle clamp hinges: confirm 30° inward swing firmly holds docking craft without grid damage.

```
PHASE 10-6 VISUAL MILESTONE · COMPLETE EXPEDITIONARY MINING TRAIN FOB
                        [KESTREL PADS]         [MULE GANTRY PAD]
                              │                        │
                    [TRACTOR BRIDGE L4]       [UTILITY WORKSHOP L3]
                              │                        │
       [45 m BOOM L4]         │         [HITCH L2]     │         [STERN DUMP L2]
              \               │              ║         │                /
      ═════════╧══════════════╧══════════════╩═════════╧════════════════╩════════
           CARRIAGE 1: LOCOMOTIVE / EXCAVATOR   CARRIAGE 2: FOB / UTILITY MODULE
```

---

## Phase 10-7 · Master Control Logic, Dual-PB Architecture & RIG//OS-Lite Contract

Every automated system on the Mastodon MKI conforms to this dual-compute and dynamic C2 hierarchy contract:

### 1. Dual-PB Compute Architecture (Tractor L3)
1. **`[MAS] PB Excavator` (Real-Time Machine Kinematics · Update1 / 60 Hz):**
   * Manages 3-axle tractive counter-steering and 2-axis motorized drawbar dampening.
   * Manages 4× corner bedrock outrigger deployment, auto-leveling, and voxel lock verification.
   * Controls 5-DOF articulated boom inverse kinematics, Custom Turret Controller mouse aim, and automated sweep macros (Bench, Shaft, Trench).
   * Strict Zero-GC loop execution; numbers formatted through `SbFormat`.
2. **`[MAS] PB RIG//OS-Lite` (Regional Fleet Orchestration & C2 Hub · Update10 / Update100):**
   * Manages dynamic C2 election: operates as Fleet Mothership / Root until Ground Base (`GND-01`) is commissioned.
   * Dispatches Caterpillar bulk land trains (`TMS.CAT.CALL`) and Mule air shuttles (`TMS.MULE.CALL`).
   * Receives Kestrel scout drone geological scans (`TMS.SCOUT.VEIN`) and displays vein GPS markers on the tactical video wall.
   * Orchestrates subordinate worker excavators (`MAS-02`, `MAS-03`) via `FLT.MAS.CMD`.
   * Automatically relinquishes C2 root authority upon detecting Ground Base master heartbeat (`RIG.C2.HEARTBEAT;ROLE=MASTER_BASE`).

```
State Machine: Dynamic Fleet Authority & C2 Handover

 [BOOT / INITIAL SCAN] ──► Listen for `RIG.C2.HEARTBEAT` (5.0 s timeout)
            │
            ├─► Ground Base Heartbeat Detected (`GND-01` Master)
            │   └─► Role = WORKER_SLAVE (Yields C2 to Base)
            │
            ├─► Another Mothership Detected (`MAS-01-M` Active)
            │   └─► Role = WORKER_SLAVE (Yields C2 to Lead Unit)
            │
            └─► No Superior C2 Heartbeat Detected
                └─► Role = MOTHERSHIP_ROOT (`MAS-01-M`)
                    │
                    ├─► Broadcast: `RIG.C2.HEARTBEAT;ORIGIN=MAS-01;ROLE=MOTHERSHIP`
                    ├─► Accept Fleet Asset Registrations (`TMS.REGISTER`)
                    ├─► Dispatch Bulk Land Trains and Air Shuttles
                    │
                    ▼
         [GROUND BASE COMMISSIONED]
         Receives: `RIG.C2.HEARTBEAT;ORIGIN=GND-01;ROLE=MASTER_BASE`
                    │
                    ├─► Transmit Active Fleet Registry & Ore Quotas to GND-01
                    ├─► Emit: `RIG.C2.HANDOVER;ACK=GND-01`
                    └─► Role = FIELD_LEADER (Expeditionary Sub-Node)
```

### 2. Group Taxonomy
* `MAS Tractor Wheels`: 6× 5×5 suspension wheel units on Carriage 1.
* `MAS Utility Wheels`: 4× 5×5 suspension wheel units on Carriage 2.
* `MAS Outriggers`: 4× Hydraulic stabilizer pistons + magnetic plates.
* `MAS Boom Joints`: 5× Rotors and Hinges articulating the long-reach arm.
* `MAS Lasers`: 2× Simple Laser Multitools on the tool head.
* `MAS MP Cradle`: 2× Motorized Hinges + 2× Magnetic Plates on the Mule pad.
* `MAS Displays`: All cockpit telemetry screens and tactical video wall LCDs.

### 3. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Programmable Block | `[MAS] PB Excavator` | Kinematics & machine control · Update1 · Zero-GC |
| Programmable Block | `[MAS] PB RIG//OS-Lite` | Regional Fleet C2 Mothership engine · Update10 · IGC router |
| Cockpit (Armored) | `MAS Helm` | Station 1 (Port) · Pilot traction, steering, outrigger control |
| Control Station | `MAS Station Boom` | Station 2 (Center) · Excavation Master · CTC mouse aim & macros |
| Control Station | `MAS Station Fleet` | Station 3 (Stbd) · Tactical Fleet Director · Drone dispatch & C2 |
| Laser Antenna | `MAS Comm Laser` | 40 km point-to-point line-of-sight data link (L6 mast) |
| Radio Antenna | `MAS Comm Radio` | 50 km omnidirectional fleet broadcast & heartbeat beacon |
| AI Move Block | `MAS AI Move` | Autonomous pathfinding & terrain navigation (uncrewed role) |
| AI Task Block | `MAS AI Task` | Autonomous waypoint execution & ore vein transit |
| AI Defense Block | `MAS AI Defense` | Threat avoidance · Auto-locks outriggers and stows boom |
| Piston (Large) | `MAS Outrigger Piston 1..4` | Corner bedrock stabilizer cylinders · `Share Inertia Tensor = ON` |
| Magnetic Plate | `MAS Outrigger Pad 1..4` | Corner bedrock voxel locking pads · `Auto-Lock = ON` |
| Advanced Rotor | `MAS Turret Azimuth` | Dorsal boom turntable · 360° pan · `Torque = 50 MN·m` |
| Motor Hinge | `MAS Shoulder Pitch` | Boom elevation joint · −20° to +75° · `Torque = 45 MN·m` |
| Motor Hinge | `MAS Elbow Pitch` | Boom mid-joint · −100° to +45° · `Torque = 45 MN·m` |
| Advanced Rotor | `MAS Wrist Roll` | Tool head roll alignment · ±180° · `Torque = 20 MN·m` |
| Motor Hinge | `MAS Wrist Pitch` | Tool head angle leveling · −90° to +90° · `Torque = 20 MN·m` |
| Laser Multitool | `MAS Laser 1, 2` | Mining beam heads on tool head · Continuous extraction |
| Connector (Large) | `MAS MP Conn` | Roof flush connector for MULE-01 shuttle docking |
| Connector (Large) | `MAS Stern Dump Conn` | Stern bumper connector for CATERPILLAR-01 bulk train docking |
| Conveyor Sorter | `MAS MP Sorter` | High-priority rare ore drain sorter (U, Pt, Au, Mg) |
| Conveyor Sorter | `MAS Stern Sorter` | High-volume bulk ore drain sorter (Fe, Ni, Si, Stone) |
| Event Controller | `MAS EC Stabilizers` | Blocks boom arm deployment until outriggers report locked |
| Event Controller | `MAS EC Holds` | Monitors payload mass; alerts at 2,500 t limit |
| Event Controller | `MAS EC Handover` | Monitors Ground Base heartbeat; triggers C2 handover |

### 4. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MAS EC Stabilizers`| `MAS Outrigger Pad 1..4` | All 4 Pads = Locked | Enable `MAS Boom Joints` · Sound Ready Chime | Lock `MAS Boom Joints` · Fold Boom |
| `MAS EC Holds` | Combined Ore Mass | Ore Mass $\ge 2,500\text{ t}$ | Sound Payload Horn · IGC `TMS.MULE.CALL` | Normal Extraction Mode |
| `MAS EC MP` | `MAS MP Conn` | Status = Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle Retract (0°) |
| `MAS EC Train` | `MAS Stern Dump Conn` | Status = Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Handover` | `GND-01` Heartbeat | Signal Detected | Run PB `RIG//OS-Lite` (Argument: `HANDOVER`) | Maintain Mothership Authority |

### 5. IGC Channel Subscriptions (Ground Fleet & RIG//OS Bus)
* Network Beacon: `RIG.C2.HEARTBEAT` (`<origin_id>;<role:MOTHERSHIP|BASE|WORKER>;<gps_coords>`).
* Network Handover: `RIG.C2.HANDOVER` (`<source_id>;<target_id>;<asset_registry_payload>`).
* Logistics Outbound: `TMS.MULE.CALL` (`CALL;<requesting_pad>;<priority>;<payload_type>`).
* Logistics Outbound: `TMS.CAT.CALL` (`CALL;<consist_id>;<bulk_ore_t>;<gps_coords>`).
* Fleet Telemetry: `TMS.MAS.STATUS` (`<consist_id>;<ore_mass_t>;<payload_pct>;<gps_coords>;<fob_status>`).
* Geological Inbound: `TMS.SCOUT.VEIN` (Receives target ore vein coordinates from Kestrel drones).
* Subordinate Dispatch: `FLT.MAS.CMD` (`<target_id>;<command>;<waypoint_gps>`).

---

## Phase 10-8 · Commissioning & Field Acceptance Checklist (RIG Test)

### A · Articulated Consist Powertrain Trials
* [ ] **Tractive Articulation Test:** Drive consist across rolling dunes at 35 km/h. Verify 2-axis drawbar hitch pivots smoothly in yaw and pitch without jackknifing or lifting carriage wheels.
* [ ] **Counter-Steer Tracking:** Verify Axle 5 counter-steers, pulling the utility carriage cleanly through tight 35 m radius curves with zero curb clipping.
* [ ] **Slope Holding:** Stop consist on 25° incline. Confirm wheel handbrakes and 100% friction settings prevent downslope roll.

### B · Outrigger Anchoring & Automated Boom Macros
* [ ] **Outrigger Bedrock Anchor:** Press hotkey 1 (`Deploy Stabilizers`). Confirm all 4 corner pistons extend downward and magnetic plates achieve 100% voxel lock. Confirm chassis is completely rigid.
* [ ] **3D Hemispheric Sweep:** Engage CTC mouse aim. Sweep boom through 360° horizontal pan, −20° terrain undercutting, and +75° high bench cuts. Confirm zero chassis wobble or physics jitter.
* [ ] **Bench Quarry Macro Test:** Trigger `MAS Timer Bench`. Verify boom executes automated -45° to +45° horizontal sweeps with -2.5° incremental shoulder steps and flat floor leveling.
* [ ] **Shaft Bore Macro Test:** Trigger `MAS Timer Bore`. Verify 360° continuous rotation and synchronized helical vertical descent to -25.0 m.
* [ ] **Collision Sensor Stop:** Drive boom tool head toward obstacle; confirm `MAS Sensor Boom` stops joint movement at 5.0 m distance.

### C · Mobile FOB & Dual Logistics Verification
* [ ] **Mule Air Offload:** Land MULE-01 on utility roof pad. Verify cradle arms swing 30° inward, magnetic plates clamp, and rare ores drain at $> 150\text{ t/min}$.
* [ ] **Caterpillar Train Rail Dump:** Reverse Caterpillar train into stern bumper. Verify large connector locks and 2,500 t bulk ore dumps in $< 90\text{ seconds}$.
* [ ] **Field Workshop Operations:** Queue 1,000 Steel Plates in `MAS Utility Assembler`. Verify autonomous fabrication using mined iron ingots.

### D · C2 Fleet Orchestration & Handover Trials
* [ ] **Mothership Heartbeat Broadcast:** Confirm `MAS-01` broadcasts `RIG.C2.HEARTBEAT;ROLE=MOTHERSHIP` on 50 km radio and laser comms.
* [ ] **Subordinate Worker Binding:** Simulate subordinate worker `MAS-02`; confirm it binds to `MAS-01` and appears on Station 3 Tactical Video Wall.
* [ ] **Logistics Task Dispatch:** Trigger 90% ore buffer threshold; verify `RIG//OS-Lite` autonomously broadcasts `TMS.MULE.CALL` and `TMS.CAT.CALL`.
* [ ] **Ground Base Handover Protocol:** Simulate `GND-01` Ground Base C2 boot; verify `MAS-01` detects `ROLE=MASTER_BASE`, transmits fleet registry, and steps down to `FIELD_LEADER` without human intervention.

