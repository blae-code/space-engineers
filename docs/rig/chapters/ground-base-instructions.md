# Ground Base & Fleet Command Center · Visual Build Instructions

## Chapter 4 · Ground Base, Refinery Complex & Fleet Command Center (C2)

Stationary terrestrial logistics terminal, high-throughput manufacturing foundry, and central Multi-Fleet Command & Control (C2) Operations Hub.

* **Grid:** Large Grid (1 B = 2.5 m).
* **Footprint:** 24 × 28 B foundation (60.0 × 70.0 m) · Height: 9 B (22.5 m to comms mast tip).
* **Role:** Primary planetary logistics nexus. Receives high-value rare ores from aerial shuttles (MULE-01) and ultra-high-volume bulk ores from land trains (CATERPILLAR-01); refines raw ores with maximum yield; fabricates ship components at 400% speed; feeds the Rig Linear Extruder Ring; and coordinates all terrestrial and aerial fleet operations via an elevated C2 bridge.
* **Operating Environment:** Earth 1 g baseline, surface level.

---

### 4.0 · Reference Grid, In-Game Paint Palette & Facility Zoning

#### 1. Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the foundation deck surface at Level L1.
* **X (Beam / Transverse):** West is negative (−), East is positive (+), Centerline is 0.
* **Y (Elevation):** Tarmac foundation deck is Level L1 (Y = 0), advancing upward (+).
* **Z (Length / Longitudinal):** South (toward Extruder Ring) is negative (−), North (toward Landing Pads & Rail Terminal) is positive (+).

```
Spatial Alignment Triad:
       [+Y: Zenith / Up]
              │
              │   [+Z: North / Approach / Pads]
              │   /
              │  /
              │ /
  [−X: West] ─┼───────── [+X: East]
             /│
            / │
           /  │
  [−Z: South / Extruder Ring]
              │
       [−Y: Bedrock Anchor]
```

#### 2. In-Game Paint Palette & Color Coding Scheme

To eliminate builder disorientation across a 60 × 70 m facility, all structural armor and functional blocks MUST be painted during assembly according to the standard Space Engineers 1 palette:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Heavy armor foundation tarmac, bedrock anchors, retaining walls |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | MULE-01 touchdown deck, gantry cradle arms, pad perimeter borders |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | CATERPILLAR-01 unload trench, stop bumpers, rail alignment curbs |
| **Slot 3** | Processing Teal | `#1ABC9C` | Bright Teal | 4× Large Refineries, 16× Yield Modules, raw ore intake manifold |
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | 4× Large Assemblers, 16× Speed Modules, refined ingot feed trunk |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Large Component Buffer container, Extruder feed conveyor trunk |
| **Slot 6** | High-Voltage Red | `#C0392B` | Alert Red | Power Vault, 6× Large Batteries, 2× Auxiliary Nuclear Reactors |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | C2 Operations Bridge exterior shell, server rack consoles |
| **Slot 8** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel catwalks, stairwells, maintenance access hatches |
| **Slot 9** | Clean Armor White | `#ECF0F1` | Bright White | C2 Bridge interior ceiling, panoramic window trim, comms spire |

#### 3. Master Facility Layout & Zoning Plan

```
Plan view, Y = L1 (Tarmac Deck Level, 24 × 28 B)
        X: −11  −9    −6    −3     0    +3    +6    +9   +11
    Z: +13 ┌─────┬─────┬─────┬═════════════┬─────┬─────┬─────┐
           │     │ CATERPILLAR│  MULE PAD   │     │     │     │
    Z: +11 │     │ UNLOAD BAY │ (7×7 B Deck)│     │     │     │
           │     │ (Trench)  │  [PAD CONN] │     │     │     │
    Z: +7  │     │ [DUMP CON]│  [CRADLES]  │     │     │     │
           │     ├─────┴─────┼═════╤═══════┴─────┼─────┼─────┤
    Z: +5  │ C2  │           │     │ Ore Trunk   │     │     │
           │ OPS │┌────────┐ │ ┌───┴────┐  ┌────┐│     │     │
    Z: +1  │ HUB ││ REF 1&2│ │ │ REF 3 │  │REF4││     │     │  (4× Large Refineries)
           │(L3) │└────────┘ │ └────────┘  └────┘│     │     │  (16× Yield Modules)
    Z: −3  ├─────┤           │                   │     │     │
           │COMMS│┌────────┐ │ ┌────────┐  ┌────┐│     │     │  (4× Large Assemblers)
    Z: −5  │MAST ││ ASM 1&2│ │ │ ASM 3  │  │ASM4││     │     │  (16× Speed Modules)
           ├─────┴┴────────┴═╪═╪════════┴──┴────┴┴─────┤     │
    Z: −9  │  [POWER VAULT]  │ │   [BUFFER CARGO]      │     │  (Batteries & Storage)
           │ (6× Bat + 2× Rct│ │ (Large Component Hold)│     │
    Z: −13 └─────────────────┴─┼───════════╤═══════════┴─────┘
                               │           │ Direct Feed Line to Extruder Ring
                               │           ▼
                       Bulk Ore Return   Pylon Feed (60 m South)
```

---

### 4.1 · Master Logistics & Process Flow Topology

The facility processes materials through two distinct speed-tiers before funneling fabricated components to the Rig Extruder Ring:

```
                           TERRESTRIAL LOGISTICS INTAKE
                                         │
         ┌───────────────────────────────┴───────────────────────────────┐
         ▼ (Tier 1: Air / Rare)                                          ▼ (Tier 2: Ground / Bulk)
  MULE-01 TOUCHDOWN (Z +9, X +3)                               CATERPILLAR-01 UNLOAD (Z +9, X −4)
  [GND PAD Conn] (Belly Lock)                                  [GND CAT Dump Conn] + 5× Collectors
         │                                                               │
         ▼ (Drain ON · High Priority)                                    ▼ (Drain ON · Bulk Sorter)
  [GND Sorter Rare] (U, Pt, Au, Mg)                            [GND Sorter Bulk] (Fe, Ni, Si, Stone)
         │                                                               │
         ▼                                                               ▼
  [GND REF 1 & 2] (High-Yield Smelting)                        [GND REF 3 & 4] (High-Yield Smelting)
  (8× Yield Modules · Rare Ingot Stream)                       (8× Yield Modules · Bulk Ingot Stream)
         │                                                               │
         └───────────────────────────────┬───────────────────────────────┘
                                         │ (Refined Ingots Trunk)
                                         ▼
         ┌───────────────────────────────┴───────────────────────────────┐
         ▼                                                               ▼
  [GND ASM 1 & 2] (Structural Hull Run)                        [GND ASM 3 & 4] (Machinery & Thrusters)
  (8× Speed Modules · Plates, Girders)                         (8× Speed Modules · Motors, Computers)
         │                                                               │
         └───────────────────────────────┬───────────────────────────────┘
                                         │ (Fabricated Components Trunk)
                                         ▼
                             [GND STR Components]
                             (Large Cargo Container · Z −10, X +3)
                                         │
                                         ▼ (Main High-Pressure Feed Line)
                       [EXT Welder Ring] (Chapter 5 Shipyard)
                       (22× Ship Welders · 6× Hydraulic Pistons)
```

---

### 4.2 · Master Electrical & Comms Bus Schematic

```
                        PRIMARY PLANETARY POWER GRID
           6× Large Grid Batteries (18 MWh Cap · 72 MW Peak Discharge)
           2× Auxiliary Nuclear Reactors (30 MW Emergency Standby)
                                      │
                                      ▼
                             MAIN DISTRIBUTION BUS
         ┌────────────────────────────┼────────────────────────────┐
         │                            │                            │
         ▼                            ▼                            ▼
  HEAVY INDUSTRIAL LOAD        SHIPYARD EXTRUSION LOAD      C2 COMMAND & CONTROL (UPS)
  · 4× Refineries (13.2 MW)    · 22× Welders (8.8 MW peak)  · 3× Dedicated Batteries (9 MWh)
  · 4× Assemblers (2.2 MW)     · 6× Pusher Pistons (20 kW)  · 3× Server Racks (PBs)
  · Mule Charger (12.0 MW)     · Projector Head (30 kW)     · Fleet Ops Video Wall (6× LCD)
  · Train Charger (6.0 MW)                                  · 50 km Antenna + Laser Relay
```

---

### 4.3 · Master Pre-Flight Bill of Materials (BOM)

| Subassembly Zone | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Foundation Tarmac** | Heavy armor full blocks, half-blocks, blast edges (Palette 0) | 540 | 460.0 t | 540 |
| **Mule Gantry Pad** | 7×7 B deck, 1× Connector, 2× Hinges, 2× Mag Plates, 1× Sensor | 55 | 22.5 t | 275 |
| **Caterpillar Unload Dock**| 1× Connector, 5× Collectors, 2× Shock Bumpers, Guide Curbs | 32 | 34.0 t | 320 |
| **Refining Core** | 4× Large Refineries, 16× Yield Modules, Sorter manifold | 23 | 188.0 t | 645 |
| **Manufacturing Core** | 4× Large Assemblers, 16× Speed Modules, Conveyors | 22 | 78.5 t | 590 |
| **Storage & Logistics**| 1× Large Cargo Container, 4× Sorters, Heavy Conveyor Trunk | 48 | 38.0 t | 260 |
| **Power Vault** | 6× Large Batteries, 2× Small Reactors, Substation Switchgear | 12 | 48.0 t | 340 |
| **C2 Fleet Ops Bridge**| 3× Control Seats, 6× LCDs, 3× PBs, Glass, Catwalks, Airtight Door | 64 | 42.0 t | 680 |
| **Comms & Relay Spire**| 1× Radio Antenna (50 km), 1× Laser Antenna, 2× Cameras | 6 | 11.0 t | 350 |
| **TOTAL GROUND FACILITY**| Complete Surface Logistics Terminal & C2 Hub | **802** | **≈ 922.0 t** | **4,000** |

---

### Phase 1 · Foundation Tarmac & Ground Logistics Interfaces (Level L1)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 24 W × 28 L × 3 H B (60.0 × 70.0 × 7.5 m)
       Coordinate Range: X −11 to +11, Z −13 to +13, Y = L1 to L3
       Primary Datums:
       · Tarmac Surface Deck: Level L1 (Y = 0)
       · Mule Touchdown Center: [X +3, Z +9]
       · Caterpillar Trench Center: [X −4, Z +9]
```

#### Step 4.1 · Heavy Foundation Deck & Sub-Surface Conveyor Trenches (L1)

Excavate a level bedrock plateau and lay the reinforced 24 × 28 B heavy armor blast floor. Recess two longitudinal conveyor trenches (3 B wide × 1 B deep) beneath the surface deck to house the main raw ore intake lines.

```
Plan view, Y = L1 (Foundation Sub-grade & Trenches)
        X: −9    −6    −3     0    +3    +6    +9
    Z: +13 ■═════╦═════╦═════╦═════╦═════╦═════╦═════■
           ║     ║ TRENCH A  ║     ║ TRENCH B  ║     ║
    Z: +9  ║     ║ (CAT ORE) ║     ║(MULE ORE) ║     ║  ◄── Recessed Pipe Raceways
           ║     ║           ║     ║           ║     ║
    Z: +5  ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣
           ║     ║     ║     ║     ║     ║     ║     ║
    Z:  0  ║     ║ REFINERY  ║ MAIN║ REFINERY  ║     ║  ◄── Industrial Sub-grade
           ║     ║ PORT MANI ║ SPINE STBD MANI ║     ║
    Z: −5  ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣
           ║     ║ ASM MANIFOLD    ║ COMPONENT ║     ║
    Z: −11 ║     ║                 ║ OUTFLOW   ║     ║
    Z: −13 ■═════╩═════╩═════╩═════╩═════╩═════╩═════■
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.1 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 420 │ 357.0 t / 420 │ Palette Slot 0 (Slate); level perimeter  │
│ Heavy Armor Slope   │ 80  │  40.0 t / 80  │ Ramp transitions around pad edges        │
│ Conveyor Junction   │ 24  │  19.2 t / 144 │ Recessed in sub-surface trenches (L0)    │
│ Conveyor Tube (Lrg) │ 16  │   9.6 t / 96  │ Large ports north-south alignment        │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.1A: SUB-SURFACE CONVEYOR INTEGRITY**
> Before welding the flush tarmac deck plates over Trenches A and B:
> 1. Verify 100% large-port conveyor continuity from the northern intake nodes to the southern industrial headers.
> 2. Ensure zero small-conveyor bottlenecks exist anywhere in the main line (ore transfer requires large-tube flow).
> 3. Perform a vacuum air-seal check on all junction cubes.

---

#### Step 4.2 · Mule Receiving Gantry Cradle Pad (Z +6 to +12, X 0 to +6, L1-L3)

Build the dedicated 7 × 7 B landing pad for the MULE-01 Heavy Freight Shuttle. The pad mounts a heavy-duty flush receiving connector and twin motorized cradle arms equipped with magnetic clamping plates per **Decision 45**.

```
Plan view, Y = L1 (Mule Gantry Deck)
        X:  0    +1    +2    +3    +4    +5    +6
    Z: +12 ■─────────────────────────────────────■
           │     ·     ·     ·     ·     ·     │
    Z: +10 │  [CRADLE P]     ·     [CRADLE S]  │  ◄── Motorized Clamp Hinges
           │  (Hinge L1)     ·     (Hinge L1)  │
    Z: +9  │     ·     · [GND PAD] ·     ·     │  ◄── Flush Receiving Connector
           │     ·     ·  (Conn ↑) ·     ·     │
    Z: +8  │  [MAG P]        ·        [MAG S]  │  ◄── Magnetic Clamping Plates
           │  (Swing 30°)    ·     (Swing 30°) │
    Z: +6  ■─────────────────────────────────────■
```

```
2.5D Isometric Callout · Mule Gantry Cradle Clamps (Z +9, X +3)

                        [MULE-01 HULL 7×7 B]
                                 │
     Port Flank                  ▼               Starboard Flank
   [CLAMP RUB RAIL]                        [CLAMP RUB RAIL]
          ▲                                       ▲
          │                                       │
     [MAG PLATE]  ◄── 30° Swing Inward ──►  [MAG PLATE]
          │                                       │
     [MOTOR HINGE]                           [MOTOR HINGE]
     (Base: X +1, Z +10)                     (Base: X +5, Z +10)
   ═══════╧═══════════════════════════════════════╧═══════
                      FLUSH HEAVY BLAST DECK (L1)
                             [GND PAD Conn]
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.2 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 49  │  41.6 t / 49  │ Palette Slot 1 (Amber); 7×7 B flush deck │
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `GND PAD Conn` (Facing UP at X +3, Z +9) │
│ Motor Hinge (Large) │ 2   │   8.0 t / 200 │ Share Inertia Tensor = ON · Limit: 30°   │
│ Magnetic Plate (Lrg)│ 2   │   4.8 t / 50  │ Auto-lock enabled; strength 100%         │
│ Sensor              │ 1   │   0.3 t / 40  │ `GND PAD Sensor` (15 m aerial detection) │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `GND PAD Sorter Rare` (Drain All = ON)   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 4.2A: CRADLE CLAMP CALIBRATION**
> 1. Set `Share Inertia Tensor = ON` on both `GND PAD Cradle Hinge Port` and `Stbd`.
> 2. Set Hinge Lower Limit to `0.0°` (fully retracted flush with deck) and Upper Limit to `30.0°` (angled inward to match Mule flank slope).
> 3. Verify `GND PAD Sorter Rare` whitelist contains: `Uranium`, `Platinum`, `Gold`, `Magnesium`, and `Scrap`.

---

#### Step 4.3 · Caterpillar Bulk Ore Receiving Trench Dock (Z +6 to +12, X −6 to −1, L1)

Construct the high-volume receiving dock for the CATERPILLAR-01 Land Train. It features a heavy guide curb, reinforced stop bumpers, a stern-docking receiving connector, and a 5-block flush collector trench for rapid hopper gravity discharge.

```
Plan view, Y = L1 (Caterpillar Unload Dock)
        X: −6    −5    −4    −3    −2    −1
    Z: +12 ║═════╧═════╧═════╧═════╧═════╧═════║  ◄── Train Entry Approach
           ║ [CURB P]               [CURB S]   ║      (Heavy armor wheel guides)
    Z: +10 ║   │    [COLL 1] [COLL 2]   │      ║
           ║   │    [COLL 3] [COLL 4]   │      ║  ◄── 5× In-Deck Collectors
    Z: +8  ║   │        [COLL 5]        │      ║      (Flush with deck, facing UP)
           ║   │                        │      ║
    Z: +6  ╠═══╪════════════════════════╪══════╣
           ║[BUMP P]   [GND CAT Conn] [BUMP S] ║  ◄── Stern Lock Connector &
    Z: +5  ╚═══════════════╤═══════════════════╝      Shock Absorber Bumpers
                           ▼
                 Main Bulk Ore Sorter (Trench A)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.3 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Settings & Orientation          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `GND CAT Dump Conn` (Facing North +Z)    │
│ Collector (Large)   │ 5   │  18.5 t / 125 │ `GND CAT Hopper Coll 1..5` (Facing UP)   │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `GND CAT Sorter Bulk` (Drain All = ON)   │
│ Heavy Armor Slopes  │ 18  │  15.3 t / 18  │ Palette Slot 2 (Crimson); wheel guide curb│
│ Half Armor Blocks   │ 8   │   3.4 t / 8   │ Shock bumper backstops at Z +5           │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 4.3A: TRAIN DOCK DISCHARGE CLEARANCE**
> 1. Ensure `GND CAT Dump Conn` is mounted at Level L2 centerline (matching `CAT Train Unload Conn` height).
> 2. Verify all 5 collectors feed directly into Trench A with large conveyor ports.
> 3. Verify `GND CAT Sorter Bulk` whitelist is set to: `Iron`, `Nickel`, `Silicon`, `Cobalt`, and `Stone`.

---

#### Visual Milestone 4-1 · Tarmac Foundation & Dual Logistics Terminals Complete

```
Visual Checkpoint Silhouette (Looking South from Approach Vector):
       X: −6 (Caterpillar Dock)       X: +3 (Mule Gantry Pad)
          ┌────────────────┐             ┌────────────────┐
          │  CRIMSON DOCK  │             │   AMBER PAD    │
          │ [COLL TRENCH]  │             │ [GANTRY CLAMPS]│
          │ [CAT DUMP CON] │             │ [GND PAD CONN] │
     ═════╧════════════════╧═════════════╧════════════════╧═════
               24 × 28 B REINFORCED SLATE TARMAC DECK (L1)
```

* [ ] Foundation blast floor is completely sealed with zero exposed bedrock gaps.
* [ ] Mule gantry cradle hinges operate smoothly between 0° and 30° without collision.
* [ ] Caterpillar dock guide curbs provide exactly 5.2 m clear width for 5×5 wheel entry.

---

### Phase 2 · Industrial Refining & Manufacturing Core (Levels L1-L2)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 20 W × 14 L × 3 H B (50.0 × 35.0 × 7.5 m)
       Coordinate Range: X −8 to +8, Z −11 to +5, Y = L1 to L2
       Key Centers:
       · Smelting Core (4 Refineries): Z +1 to +4, X ±3 to ±5
       · Fabrication Core (4 Assemblers): Z −5 to −1, X ±3 to ±5
       · Component Storage & Power Vault: Z −11 to −7, X −6 to +6
```

#### Step 4.4 · 4-Refinery Heavy Smelting Array (Z +1 to +4, X ±3 to ±5, L1-L2)

Install 4 Large Refineries paired with 16 Yield Modules (4 per refinery) to achieve 100% ore extraction yield bonus. The array is split into two banks: West Bank (Refineries 1 & 2) handles rare ores from Trench B; East Bank (Refineries 3 & 4) handles bulk ores from Trench A.

```
Plan view, Y = L1-L2 (Refinery Core Layout)
        X: −6   −5   −4   −3        +3   +4   +5   +6
    Z: +4  ┌───────────────┐        ┌───────────────┐
           │ [YLD] REF 1   │        │ REF 3   [YLD] │
    Z: +3  │ [YLD] [2×4 B] │        │ [2×4 B] [YLD] │  ◄── 4× Large Refineries
    Z: +2  │ [YLD] REF 2   │        │ REF 4   [YLD] │      (Painted Processing Teal)
    Z: +1  │ [YLD] [2×4 B] │        │ [2×4 B] [YLD] │
           └───────────────┘        └───────────────┘
                   │                        │
                   ▼                        ▼
           Refined Ingot Trunk      Refined Ingot Trunk
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.4 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Mounting Specifications         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Refinery      │ 4   │ 100.8 t / 360 │ Palette Slot 3 (Teal); ports facing N/S  │
│ Yield Module        │ 16  │  83.2 t / 240 │ 4 per refinery; mounted flush to sides   │
│ Conveyor Tube (Lrg) │ 12  │   7.2 t / 72  │ Dual intake and dual output manifolds    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!TIP]
> **PRE-ENCLOSURE INSPECTION GATE 4.4A: REFINERY YIELD VERIFICATION**
> 1. Check the terminal interface of all 4 refineries (`GND REF 1` to `4`).
> 2. Verify `Effectiveness` reads exactly `200%` (or baseline 1.0 + 8 upgrade ports connected).
> 3. Verify automated ingot pull into the central distribution trunk is clear.

---

#### Step 4.5 · 4-Assembler Manufacturing Core (Z −5 to −1, X ±3 to ±5, L1)

Install 4 Large Assemblers flanked by 16 Speed Modules (4 per assembler) to supply the massive component demand of the Rig Extruder Ring. Assemblers 1 & 2 are dedicated to structural components (Steel Plates, Interior Plates, Construction Components); Assemblers 3 & 4 handle machinery and propulsion hardware (Large Steel Tubes, Motors, Computers, Thruster Components).

```
Plan view, Y = L1 (Assembler Array Layout)
        X: −5   −4   −3   −2        +2   +3   +4   +5
    Z: −1  ┌────────────┐              ┌────────────┐
           │ [SPD] ASM 1│              │ ASM 3 [SPD]│
    Z: −2  │ [SPD]      │              │       [SPD]│  ◄── 4× Large Assemblers
    Z: −3  │ [SPD] ASM 2│              │ ASM 4 [SPD]│      (Painted Cobalt Blue)
    Z: −4  │ [SPD]      │              │       [SPD]│
           └────────────┘              └────────────┘
                 │                           │
                 └─────────────┬─────────────┘
                               │ Fabricated Components Bus
                               ▼
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.5 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Mounting Specifications         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Assembler     │ 4   │  38.8 t / 160 │ Palette Slot 4 (Cobalt); ports N/S/U/D   │
│ Speed Module        │ 16  │  38.4 t / 240 │ 4 per assembler; mounted on top/sides    │
│ Conveyor Junction   │ 6   │   4.8 t / 36  │ Central manufacturing aggregation bus    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 4.5A: COOPERATIVE FABRICATION LINK**
> 1. Enable `Master / Slave` (Cooperative Mode) on Assemblers 2, 3, and 4.
> 2. Set Assembler 1 as the Master Production Queue controller.
> 3. Verify all 16 speed modules register `Productivity: 400%`.

---

#### Step 4.6 · Component Buffer Vault & Main Power Plant (Z −11 to −7, X −6 to +6, L1-L2)

Construct the central component buffer warehouse and the high-output planetary power station. The storage container acts as the master supply reservoir for the Extruder Ring, while the battery bank guarantees blackout immunity.

```
Plan view, Y = L1 (Storage & Power Vault)
        X: −6   −4   −2     0    +2   +4   +6
    Z: −7  ┌──────────────┐     ┌──────────────┐
           │ [BAT 1][BAT 2│     │ [STR BUFFER] │  ◄── Large Cargo Container (3×3×3 B)
    Z: −9  │ [BAT 3][BAT 4│     │ (3×3×3 B)    │      (Palette Slot 5: Emerald)
           │ [BAT 5][BAT 6│     │ [GND STR]    │
    Z: −11 │ [RCT 1][RCT 2│     └──────╤───────┘
           └──────────────┘            │ Main Extruder Feed Line (South)
                                       ▼
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.6 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Capacity                   │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo (3×3×3) │ 1   │  28.5 t / 250 │ `GND STR Components` (421,875 L capacity)│
│ Large Battery       │ 6   │  30.0 t / 120 │ `GND BAT 1..6` (18 MWh total capacity)†  │
│ Small Reactor       │ 2   │   3.2 t / 50  │ `GND RCT Aux 1..2` (30 MW total backup)  │
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `GND Sorter Extruder Feed` (Push to Ring)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.6A: POWER BUS REDUNDANCY**
> 1. Verify `GND BAT 1..6` are set to `Auto` with Recharge/Discharge limits unconstrained.
> 2. Ensure `GND RCT Aux 1..2` are loaded with 50 kg Uranium each and assigned to group `GND Power Emergency`.
> 3. Verify the southern conveyor outlet terminates at X0, Z−13, facing directly toward the Chapter 5 Extruder Ring intake.

---

#### Visual Milestone 4-2 · Industrial Core Complete

```
Visual Checkpoint Silhouette (Looking West across Industrial Core):
        L2  ┌────────┐                     ┌────────┐
            │ REF 3  │   [INGOT TRUNK]     │ REF 4  │
        L1  ├────────┴──────────┬──────────┴────────┤
            │ 4× ASSEMBLERS     │ COMPONENT BUFFER  │
     ═══════╧═══════════════════╧═══════════════════╧═══════
                 LEVEL L1 REINFORCED SERVICE FOUNDATION
```

* [ ] All 4 refineries and 4 assemblers report green power indicators and zero blocked ports.
* [ ] Component buffer accepts continuous input from assemblers without back-pressure.
* [ ] Battery bank shows full charge availability of 72 MW peak.

---

### Phase 3 · Fleet Command & Control (C2) Operations Center (Levels L1-L5)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 5 W × 5 L × 9 H B (12.5 × 12.5 × 22.5 m)
       Coordinate Range: X −11 to −7, Z +1 to +5, Y = L1 to L9
       Decks:
       · L1: Heavy Structural Pylon Anchor
       · L2: C2 Server Room & Programmable Block Rack
       · L3-L4: Panoramic Observation Deck & Operator Stations
       · L5-L9: Long-Range Comms Mast & Station Laser Link
```

#### Step 4.7 · C2 Bridge Structure & Server Deck (Z +1 to +5, X −11 to −7, L1-L2)

Erect the reinforced 5 × 5 B support pillar on the western flank. Level L1 forms the blast-proof structural foundation; Level L2 houses the isolated Fleet Operations Server Room containing the RIG//OS Programmable Blocks and communication bus hardware.

```
Plan view, Y = L2 (Server Room Deck)
        X: −11  −10   −9   −8   −7
    Z: +5  ■─────■─────■────■────■
           │ [PB FLEET] [PB INV] │  ◄── Server Rack: 3× Programmable Blocks
    Z: +3  │ [PB DISP]  [EC C2]  │
           │      [UPS BAT 1]    │  ◄── Dedicated C2 Uninterruptible Power Supply
    Z: +1  ■─────■─────[DOOR]────■      (Guarantees comms during base blackouts)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.7 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Script Role / Hardware Function          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Programmable Block  │ 3   │   3.6 t / 75  │ `GND PB FleetOps`, `Inventory`, `Displays│
│ Event Controller    │ 2   │   0.6 t / 50  │ `GND EC PadControl`, `GND EC PowerWatch` │
│ Timer Block         │ 2   │   0.6 t / 50  │ `GND Timer Watchdog`, `GND Timer 100ms`  │
│ Large Battery       │ 1   │   5.0 t / 20  │ `GND BAT C2 UPS` (Dedicated bridge power)│
│ Sliding Airtight Dr │ 1   │   0.8 t / 25  │ Pressurized maintenance access hatch     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 4.7A: SERVER RACK INITIALIZATION**
> 1. Verify `GND PB FleetOps` compiles clean with zero warnings under MDK2.
> 2. Ensure `GND BAT C2 UPS` is completely isolated from the industrial grid via an automated switch event.
> 3. Verify server room is fully airtight prior to sealing the ceiling deck.

---

#### Step 4.8 · Elevated Panoramic Observation Deck & Multi-Fleet Consoles (L3-L4)

Build the elevated command deck (Y = L3-L4) cantilevering inward with panoramic reinforced glass. It provides unobstructed visual line-of-sight over the Mule Pad and Caterpillar Dock to the North, and the Rig Extruder Yard to the South.

```
Elevation, looking West into C2 Bridge (X = −7, Y = L3-L5):
       L5  ╔═══════════════════════════════════════════╗  Bridge Roof (Armor White)
           ║ [COMMS MAST]                              ║
       L4  ║ ┌───────────────────────────────────────┐ ║  Panoramic Sloped Glass
           ║ │         FLEET OPS VIDEO WALL          │ ║  (6× Large LCD Dashboards)
           ║ │  [ORCA 1..n]  [MULE TRANSIT]  [STOCKS]│ ║
       L3  ║ └───────────────────────────────────────┘ ║
           ║   [DESK 1: ORCA]   [DESK 2: MULE]   [DESK 3: YARD] ║  ◄── 3 Operator Desks
       L2  ╠═══════════════════════════════════════════╣  Server Room Deck
           ║   [GND PB FleetOps]   [GND PB Inv]   [GND PB Disp] ║
       L1  ■═══════════════════════════════════════════■  Tarmac Foundation
```

```
2.5D Isometric Callout · Multi-Fleet Video Wall & Operator Desks

      ┌─────────────────────────────────────────────────────────────┐
      │               FLEET OPERATIONS LCD VIDEO WALL               │
      ├──────────────────────┬──────────────────────┬───────────────┤
      │ LCD 1: AERIAL FLEET  │ LCD 2: GROUND FLEET  │ LCD 3: YARD   │
      │ Orca-01: 82% Hold    │ Mastodon: Digging    │ Extruder Ring:│
      │ Mule-01: Inbound Pad │ Cat-01: In Transit   │ 64% Complete  │
      │ Mule-02: At Carrier  │ Consist: 2,500 t Ore │ Piston Feed:  │
      │ 8 Active Mine Leases │ Speed: 52 km/h       │ 0.05 m/s      │
      ├──────────────────────┼──────────────────────┼───────────────┤
      │ LCD 4: REFINERY FLOW │ LCD 5: STOCKPILES    │ LCD 6: ALERTS │
      │ Rare Smelt: 100% Opt │ Fe Ingot: 142,000 kg │ Base Grid: NRM│
      │ Bulk Smelt: 100% Opt │ U Ingot:   1,850 kg  │ Defense: ARMED│
      │ Total Yield: 200%    │ Steel Plates: 85,000 │ Traffic: CLR  │
      └──────────────────────┴──────────────────────┴───────────────┘
                                     │
           ┌─────────────────────────┼─────────────────────────┐
           ▼                         ▼                         ▼
   [DESK 1: AIR OPS]         [DESK 2: GROUND OPS]      [DESK 3: YARD OPS]
   · Orca carrier leases     · Mastodon tusk CTC       · Extruder pusher sync
   · Remora shaft status     · Caterpillar routing     · Welder wall status
   · Mule gantry traffic     · Terminal dump cycling   · Component inventory
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.8 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Terminal Name & Configuration            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Control Station     │ 3   │   1.8 t / 45  │ `GND Desk AirOps`, `GroundOps`, `YardOps`│
│ Large LCD Panel     │ 6   │   2.4 t / 150 │ `GND LCD Video Wall 1..6` (Group Displays│
│ Sloped Armor Glass  │ 24  │   4.8 t / 24  │ 2-layer panoramic forward & rear windows │
│ Interior Light      │ 4   │   0.1 t / 20  │ Low-glare tactical amber interior lights │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Step 4.9 · Long-Range Comms Mast & Station Laser Link (Z +1, X −9, L5-L9)

Crown the C2 Bridge roof with a 10 m communications mast. It mounts a high-power omni-directional radio antenna for regional fleet command and a dedicated laser antenna hard-locked to the stationary Floating Rig hovering in the upper atmosphere.

```
Elevation, looking East at Comms Mast (X = −9, Y = L5-L9):
       L9             [LASER RELAY]  ◄── `GND Laser Rig Relay` (Locked to Rig)
                           │
       L8                  │
                           ├── [PERIMETER CAMERA]
       L7                  │
                           ├── [OMNI ANTENNA]  ◄── `GND Antenna Fleet` (50 km)
       L6                  │
       L5  ════════════════╧════════════════  ◄── Bridge Roof Deck
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 4.9 PARTS TRAY                                                                  │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Operating Settings & Parameters          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Radio Antenna (Lrg) │ 1   │   8.0 t / 100 │ `GND Antenna Fleet` (Range: 50,000 m)    │
│ Laser Antenna (Lrg) │ 1   │   2.5 t / 200 │ `GND Laser Rig Relay` (Rig GPS lock)     │
│ Camera              │ 2   │   0.2 t / 40  │ `GND Cam North Pads`, `GND Cam South Yard│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

#### Visual Milestone 4-3 · Complete Facility & C2 Bridge Commissioned

```
Visual Checkpoint Silhouette (Looking West at Complete Base Complex):
                        [COMMS MAST (L9)]
                               │
                       [C2 BRIDGE (L3-L5)]
                               │
           [MULE PAD]          │          [POWER & INDUSTRIAL]
         (Z +9, X +3)          │              (Z −5, X 0)
     ══════════╤═══════════════╧═══════════════════╤══════════
               24 × 28 B REINFORCED SURFACE COMPLEX (L1)
```

* [ ] C2 bridge video wall displays live telemetry streams from all 6 LCD panels.
* [ ] Laser antenna maintains unbroken connection with the high-altitude Floating Rig.
* [ ] Control stations allow instantaneous remote access to Orca, Mule, Mastodon, and Caterpillar systems.

---

### Phase 4 · Master AI Scripting Contract & Terminal Namespace (Claude Code)

#### 1. Group Taxonomy
* `GND Refineries` : All 4 Large Refineries (`GND REF 1` to `4`).
* `GND Assemblers` : All 4 Large Assemblers (`GND ASM 1` to `4`).
* `GND Yield Modules` : All 16 Yield upgrade modules.
* `GND Speed Modules` : All 16 Speed upgrade modules.
* `GND Fleet Displays` : All 6 Video Wall LCD panels (`GND LCD Video Wall 1..6`).
* `GND Power Batteries` : All 6 Large Storage Batteries (`GND BAT 1..6`).
* `GND PAD Cradle` : `GND PAD Cradle Hinge Port`, `Stbd`, `GND PAD Cradle Clamp Port`, `Stbd`.
* `GND CAT Hopper` : `GND CAT Hopper Coll 1..5`, `GND CAT Dump Conn`.

#### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `GND EC PadClamp` | `GND PAD Sensor` | Detects MULE $\le 5\text{ m}$ | `GND PAD Cradle` Swing In 30° | `GND PAD Cradle` Retract Flush 0° |
| `GND EC PadLock` | `GND PAD Conn` | Connector Status: Connected | `GND PAD Sorter Rare` Drain ON | `GND PAD Sorter Rare` Drain OFF |
| `GND EC CatDump` | `GND CAT Dump Conn`| Connector Status: Connected | `GND CAT Sorter Bulk` Drain ON | `GND CAT Sorter Bulk` Drain OFF |
| `GND EC PowerGuard`| `GND Power Batteries`| Grid Stored Power $< 15\%$ | `GND RCT Aux 1..2` Turn ON | `GND RCT Aux 1..2` Turn OFF |

#### 3. Inter-Grid Communication (IGC) Network Protocol

| IGC Tag / Channel | Transceiver | Direction | Message Payload Format | Functional Purpose |
|---|---|---|---|---|
| `TMS.AIR.PAD` | Radio Antenna | In/Out | `mule_id;status;req_land;dock_id` | Pad allocation, clamp automation, traffic sequencing |
| `TMS.GROUND.DOCK` | Radio Antenna | In/Out | `train_id;status;req_dump;tonnage` | Caterpillar receiving trench clearance & sorter arming |
| `TMS.FLEET.STATUS`| Radio Antenna | Inbound | `craft_id;type;gps;hold_pct;bat_pct`| Central telemetry feed for C2 6-screen video wall |
| `TMS.RIG.LASER` | Laser Antenna | Bidirectional | `req_ingots;stock_json;rig_power;alert`| High-security base-to-Rig uplink for production quotas |

---

### Phase 5 · Commissioning Checklist & Acceptance Trials

#### A · Dual Logistics Interface Verification
* [ ] **Mule Touchdown Test:** Land MULE-01 on pad; verify sensor triggers gantry cradle arms to clamp rub rails at 30°; verify `GND PAD Conn` locks; verify rare ores drain at $> 80\text{ t/min}$.
* [ ] **Caterpillar Unload Test:** Reverse CATERPILLAR-01 into dump dock; verify wheel guides center the consist; verify `GND CAT Dump Conn` locks; verify 2,500 t bulk ore drains through collectors in $< 90\text{ seconds}$.

#### B · Refining & Manufacturing Throughput Trial
* [ ] **Smelting Stress Test:** Feed 1,000 t Iron Ore and 200 t Uranium Ore simultaneously; confirm dual-tier sorters route ores to respective refinery banks; verify 200% yield effectiveness.
* [ ] **Pylon Component Burst:** Queue 10,000 Steel Plates and 2,000 Construction Components; verify 4 assemblers operate cooperatively at 400% productivity without tripping the power grid.

#### C · C2 Fleet Command & Control Validation
* [ ] **Live Multi-Fleet Tracking:** Deploy 1 Orca carrier, 2 Remora drillers, 1 Mule shuttle, 1 Mastodon excavator, and 1 Caterpillar train concurrently. Verify C2 Video Wall displays live, non-overlapping status for all 6 units.
* [ ] **Laser Uplink Resilience:** Toggle planetary storm / weather effects; verify `GND Laser Rig Relay` maintains continuous, unjammed data transfer with the Floating Rig.

