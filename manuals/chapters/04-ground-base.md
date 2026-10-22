# Ground Base & Fleet Command Center · Visual Build Instructions

## Chapter 4 · Ground Base, Refinery Complex & Fleet Command Center (C2)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **Ground Base, Refinery Complex & Fleet Command Center (GND-01)**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 38, 41, 45, 46, 49, 51, 52, 56, 60 & 66)
* **Role:** Planetary industrial foundry, dual-tier logistics terminal, and permanent regional **Command & Control (C2) Fleet Root**. Receives high-value rare ores from aerial shuttles (MULE-01) and ultra-high-volume bulk ores from land trains (CATERPILLAR-01); refines raw ores at 200% yield; fabricates ship components at 400% speed; continuously feeds the Rig Linear Extruder Ring shipyard; and orchestrates all planetary fleet assets through an elevated 3-operator panoramic bridge.
* **Architecture:** 4 Integrated Functional Zones on a 24 × 28 B foundation (60.0 × 70.0 m · 22.5 m height):
  1. **West Zone (X −11 to −7): Elevated C2 Command Tower & Server Vault (Levels L1-L9):** Heavy structural support pillar (L1), isolated Fleet Operations Server Room (L2) with 3× Programmable Blocks and dedicated uninterruptible power supply (`GND BAT C2 UPS`), elevated panoramic walk-around bridge deck (L3-L4) with 6-panel LCD Video Wall, 3 operator consoles (Air Ops, Ground Ops, Yard Ops), pressurized decontamination airlock, Tier 4 mobile armory, and a 10 m communications mast (L5-L9) mounting a 50 km omni antenna and high-altitude Floating Rig laser transceiver.
  2. **North Zone (Z +6 to +13): Dual-Tier Terrestrial Logistics Apron (Levels L1-L2):** 
     * **Air Tier (Z +6 to +12, X 0 to +6):** Flush 7 × 7 B heavy armor landing pad (`GND PAD`) for MULE-01 with motorized gantry clamping arms (`GND PAD Cradle`) and high-speed rare ore sorter manifold (U, Pt, Au, Mg).
     * **Ground Tier (Z +6 to +12, X −6 to −1):** Heavy rail receiving trench for CATERPILLAR-01 with blast door guide curbs, stop bumpers, large receiving connector (`GND CAT Dump Conn`), and 5× flush in-deck collectors draining 2,500 t bulk ore in `< 90 seconds`.
  3. **Central Zone (Z −5 to +5, X −6 to +6): Industrial Smelting & Fabrication Foundry (Levels L1-L2):** 
     * **Smelting Core:** 4× Large Refineries + 16× Yield Modules (200% yield) divided into Rare (West Bank: Ref 1 & 2) and Bulk (East Bank: Ref 3 & 4) processing streams.
     * **Manufacturing Core:** 4× Large Assemblers + 16× Speed Modules (400% productivity) operating in cooperative master/slave mode.
  4. **South Zone (Z −13 to −7, X −6 to +6): Power Substation & Shipyard Extruder Feed (Levels L1-L3):** Master component buffer warehouse (Large Cargo Container `GND STR Components`), high-pressure Extruder Ring feed line with automated push-sorter, and a primary planetary power substation (6× Large Batteries 18 MWh + 2× Auxiliary Small Nuclear Reactors 30 MW).
* **Grid:** Large Grid (1 B = 2.5 m) · **Footprint:** 24 W × 28 L × 9 H B (60.0 × 70.0 × 22.5 m).
* **Mass:** Facility Structural Mass `≈ 948.5 t` · Installed Power Capacity: 30 MW Continuous Nuclear + 72 MW Battery Surge.
* **Operating Environment:** Earth 1 g baseline, planetary surface level, quarry bench connection.

---

## 0 · Reference Grid, In-Game Paint Palette & Master Facility Layout

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the foundation deck surface at Level L1 along centerline X0.
* **X (Beam / Transverse):** West is negative (−), East is positive (+), Centerline is 0. Spans X −12 to +12 (24 blocks wide = 60.0 m).
  * West Flank (C2 Tower): X −11 to −7 (5 blocks wide = 12.5 m).
  * Centerline Logistics & Process Core: X −6 to +6 (13 blocks wide = 32.5 m).
  * East Retaining Wall & Expansion Apron: X +7 to +11 (5 blocks wide = 12.5 m).
* **Y (Elevation):** Sub-surface bedrock anchor and trench raceways are Level L0 (Y = −2.5 m); tarmac foundation surface is Level L1 (Y = 0); industrial deck midline is Level L2; C2 Bridge observation deck is Level L3; bridge ceiling is Level L4; comms spire tip is Level L9 (Y = +22.5 m).
* **Z (Length / Longitudinal):** South (toward Extruder Ring) is negative (−), North (toward Logistics Approach) is positive (+). Spans Z −14 to +14 (28 blocks long = 70.0 m).
  * North Logistics Apron (Mule Pad & Cat Dock): Z +6 to +13 (8 blocks length = 20.0 m).
  * Central Industrial Core (Refineries & Assemblers): Z −6 to +5 (12 blocks length = 30.0 m).
  * South Power Vault & Buffer Storage: Z −13 to −7 (7 blocks length = 17.5 m).
  * Extruder Yard Interface: Z = −14.

```
Spatial Alignment Triad:
       [+Y: Zenith / Comms Spire L9]
              │
              │   [+Z: North / Approach / Logistics Apron]
              │   /
              │  /
              │ /
  [−X: West / C2 Tower] ─┼───────── [+X: East / Expansion Apron]
             /│
            / │
           /  │
  [−Z: South / Extruder Ring Shipyard]
              │
       [−Y: Bedrock Anchor / Level L0]
```

---

### 0.2 In-Game Paint Palette & Functional Color Coding

To eliminate builder disorientation across a 60 × 70 m industrial installation, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Heavy armor foundation tarmac, bedrock anchors, retaining walls |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | MULE-01 touchdown deck (7×7 B), gantry cradle arms, pad borders |
| **Slot 2** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | CATERPILLAR-01 unload trench, wheel curbs, crash bumpers |
| **Slot 3** | Processing Teal | `#1ABC9C` | Bright Teal | 4× Large Refineries, 16× Yield Modules, raw ore intake manifolds |
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | 4× Large Assemblers, 16× Speed Modules, refined ingot feed trunk |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Component Buffer container, Extruder feed conveyor trunk |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Vault, 6× Large Batteries, 2× Auxiliary Nuclear Reactors |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | C2 Operations Bridge exterior shell, server rack consoles |
| **Slot 8** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel catwalks, stairwells, maintenance access hatches |
| **Slot 9** | Clean Armor White | `#ECF0F1` | Clean White | C2 Bridge interior ceiling, panoramic window trim, comms spire |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

To achieve a cohesive aesthetic matching the Mastodon MKI locomotive and Floating Rig, exterior surfaces follow strict functional zoning boundaries:

```
[EXTERIOR ELEVATION LIVERY MAP · LOOKING WEST AT FACILITY]
Level
L9  │                         [SLOT 9: RIG LASER RELAY SPIRE]
L8  │                                       │
L7  │                         [SLOT 7: FLEET OMNI ANTENNA (50 km)]
L6  │                                       │
L5  │                      ═══[SLOT 9: WHITE C2 BRIDGE ROOF CROWN]═══
L4  │                     │   [SLOT 7: NAVY SLOPED GLASS CANOPY]   │
L3  │ ════════════════════[SLOT 7: NAVY C2 BRIDGE & WALK-AROUND DECK]═════════════════════
L2  │ [SLOT 1: MULE GANTRY]  [SLOT 3: TEAL REFINERY]  [SLOT 4: COBALT ASM]  [SLOT 6: POWER]
L1  │ ════════════════════[SLOT 0: INDUSTRIAL DARK GRAY HEAVY FOUNDATION TARMAC]═══════════
L0  │ ────────────────────[SLOT 0: SUB-SURFACE CONVEYOR TRENCH RACEWAYS]───────────────────
         Z: +11 (North Apron)      Z: +3 (Refining)       Z: -3 (Fabrication)  Z: -10 (Storage)
```

| Zone / Component | Primary Palette Slot | Hex Code | Visual Application & Boundaries |
|---|---|---|---|
| **Foundation Tarmac** | Slot 0 (Industrial Dark Gray) | `#2B303A` | Level L1 perimeter blast deck, retaining walls, sub-surface trench sills |
| **Mule Gantry Pad** | Slot 1 (Hazard Amber) | `#E6A100` | 7×7 B blast deck at [X +3, Z +9] with alternating black/amber warning stripes |
| **Caterpillar Dock** | Slot 2 (Heavy Rail Crimson) | `#9E2A2B` | Level L1 trench curbs, rear alignment wedges, and collision stop blocks |
| **Refining Core** | Slot 3 (Processing Teal) | `#1ABC9C` | 4× Large Refineries, 16× Yield Modules, raw ore manifolds (Z +1 to +4) |
| **Manufacturing Core**| Slot 4 (Manufacturing Cobalt) | `#2980B9` | 4× Large Assemblers, 16× Speed Modules, ingot manifolds (Z −5 to −1) |
| **Component Storage** | Slot 5 (Logistics Emerald) | `#27AE60` | Large Cargo buffer and southern Extruder feed raceway (Z −11 to −7) |
| **Power Substation** | Slot 6 (High-Voltage Red) | `#C0392B` | 6× Large Batteries, 2× Small Reactors, transformer bus (Z −11 to −7) |
| **C2 Command Tower** | Slot 7 (Fleet Command Navy) | `#1B263B` | Levels L2-L5 elevated bridge shell, server room, and structural pylon |
| **Comms Spire & Mast** | Slot 9 (Clean Armor White) | `#ECF0F1` | Levels L5-L9 antenna mast, laser transceiver mount, and aircraft warning beacons |

---

### 0.3 Master Facility Top Plan View (Levels L1-L4)

```
[MASTER FACILITY COMPOSITE PLAN VIEW · LEVELS L1-L4 (24 × 28 B · 60 × 70 m)]
        X: −11  −9    −6    −4    −1     0    +2    +4    +6    +9   +11
Z: +13 ┌─────┬─────┬─────┬─────┬─────┬═══════════════════┬─────┬─────┬─────┐
       │     │     │   CATERPILLAR   │  MULE GANTRY PAD  │     │     │     │
Z: +11 │     │     │   UNLOAD DOCK   │   (7×7 B DECK)    │     │     │     │
       │     │     │   [5× COLL]     │   [GND PAD CONN]  │     │     │     │
Z: +9  │     │     │   [STOP BUMP]   │   [CRADLE ARMS]   │     │     │     │
       │     │     │   [CAT DUMP]    │   (AMBER BORDER)  │     │     │     │
Z: +7  │     │     ├─────┴─────┬─────┼═════════╤═════════┼─────┼─────┤     │
       │     │     │           │     │         │ Ore Riser │     │     │     │
Z: +5  │     │     │┌────────┐ │ ┌───┴─────────┴────────┐│     │     │     │
       │     │     ││ REF 1  │ │ │ REF 3        REF 4   ││     │     │     │
Z: +3  │ C2  │ C2  ││ [2×4 B]│ │ │ [2×4 B]      [2×4 B] ││     │     │     │  ◄── 4× Large Refineries
       │ TWR │ BRG ││ REF 2  │ │ │                      ││     │     │     │      (16× Yield Modules)
Z: +1  │(L2) │(L3) ││ [2×4 B]│ │ │                      ││     │     │     │
       │     │     │└────────┘ │ └──────────────┬───────┘│     │     │     │
Z: −1  ├─────┼─────┼───────────┼────────────────┼────────┼─────┼─────┤     │
       │COMMS│SRVR │┌────────┐ │ ┌──────────────┴───────┐│     │     │     │
Z: −3  │MAST │ROOM ││ ASM 1  │ │ │ ASM 3        ASM 4   ││     │     │     │  ◄── 4× Large Assemblers
       │(L5) │(L2) ││ ASM 2  │ │ │                      ││     │     │     │      (16× Speed Modules)
Z: −5  ├─────┴─────┴┴────────┴═╪═╪══════════════╤═══════┴┴─────┼─────┤     │
       │  [POWER SUBSTATION]   │ │              │ Fabricated   │     │     │
Z: −7  │  [BAT 1] [BAT 2]      │ │              ▼ Components   │     │     │
       │  [BAT 3] [BAT 4]      │ │       ┌──────────────┐      │     │     │
Z: −9  │  [BAT 5] [BAT 6]      │ │       │[STR BUFFER]  │      │     │     │  ◄── Large Cargo (3×3×3 B)
       │  [RCT 1] [RCT 2]      │ │       │(3×3×3 B)     │      │     │     │      (Component Buffer)
Z: −11 │ (30 MW NUCLEAR BACKUP)│ │       │[GND STR]     │      │     │     │
       └───────────────────────┴─┼───────┴──────┬───────┴──────┴─────┴─────┘
Z: −13                           │              │ Direct High-Pressure Conveyor Trunk
                                 │              ▼
                         Bulk Ore Return   Chapter 5 Rig Linear Extruder Ring (60 m South)
```

---

### 0.4 Master Facility Profile Elevation (Looking Starboard / East, X = −11 Plane)

```
[FACILITY PROFILE ELEVATION · WEST ELEVATION LOOKING STARBOARD / EAST]
Level
L9                     [GND LASER RIG RELAY]  ◄── Hard-Locked to Floating Rig
L8                               │
L7                               ├── [GND CAM SOUTH YARD]
L6                               ├── [GND ANTENNA FLEET (50 km)]
L5             ╔═════════════════╧═════════════════╗  ◄── C2 Bridge Roof Deck (White)
L4             ║  PANORAMIC COMMAND VIEWPORT GLASS ║  ◄── 6-Panel LCD Video Wall
L3             ║  [DESK 1: AIR] [DESK 2: GND] [DESK 3: YARD]  ◄── 3 Operator Consoles
L2             ╠═══════════════════════════════════╣  ◄── Pressurized Decon Airlock
               ║ FLEET OPS SERVER ROOM (3× PBs)    ║
L1  ═══════════■═════════════════╤═════════════════■═════════════════╤═══════════════
       [NORTH LOGISTICS APRON]   │       [INDUSTRIAL FOUNDRY]        │ [SOUTH STORAGE]
       · Mule Gantry Pad (L1)    │       · 4× Refineries (L1-L2)     │ · Cargo Buffer (L1-L3)
       · Caterpillar Dock (L1)   │       · 4× Assemblers (L1)        │ · Power Vault (L1-L2)
L0  ───[SUB-SURFACE TRENCH A]────┴───────[MAIN CONVEYOR SPINE]───────┴─[EXTRUDER LINE]
       ◄── Z: +13 (North) ─────────────── Z: 0 (Datum) ─────────────── Z: -13 (South) ─►
```

---

### 0.4B Facility 3/4 Isometric Perspectives & Architectural Wireframes

```
[3/4 ISOMETRIC WIREFRAME · NORTH LOGISTICS APRON (MULE PAD & CAT DOCK)]
                            +Y (Zenith L3)
                                  ▲
                                  │      ┌───[MULE GANTRY CRADLE]───┐  ◄── 30° Motorized Clamps
                                  │     ╱                          ╱│
                   -X (West) ◄────┼────►+X (East)                 │ │
                                 ╱│   ┌──────────────────────────┐│ │  ◄── 7×7 B Flush Deck (Amber)
                                ╱ │   │ [GND PAD CONN] (Belly)   ││ │
                               ╱  │   └──────────────────────────┘│╱
                              ▼   │   ═════════════════════════════
                         -Z (South)   ┌──────────────────────────┐
                                      │ CATERPILLAR UNLOAD DOCK  │     ◄── Heavy Rail Crimson Dock
                                      │ [CAT DUMP] [5× COLL]     │     ◄── 5× In-Deck Collectors
                                      └──────────────────────────┘

[3/4 ISOMETRIC WIREFRAME · C2 OPERATIONS TOWER & COMMS SPIRE]
                            +Y (Zenith L9)
                                  ▲
                                  │          [LASER TRANSCEIVER]       ◄── Level L9 Rig Uplink
                                  │                   │
                                  │          [50 km OMNI ANTENNA]      ◄── Level L6 Fleet Comms
                                  │                   │
                                  │         ┌───────────────────┐
                                  │        ╱                   ╱│      ◄── Sloped Glass Canopy (L4)
                                  │       ┌───────────────────┐ │
                                  │       │ C2 COMMAND BRIDGE │ │      ◄── 6-Panel LCD Wall (L3)
                                  │       ├───────────────────┤ │
                                  │       │ SERVER ROOM (3×PB)│ │      ◄── Avionics Rack & UPS (L2)
                                  │       ├───────────────────┤ │
                                  │       │ HEAVY PYLON (L1)  │ │      ◄── Blast-Proof Foundation
                                  │       └───────────────────┘╱
```

---

### 0.4C Transverse & Longitudinal Station Profiles

To maintain exact dimensional verification during survival assembly, cross-sections are specified at 6 critical facility stations:

```
Station 1 · North Approach Apron (Z = +13, Looking South)
Level
L2  │   ┌───[CAT ENTRY CURB]───┐               ┌───[MULE DECK PERIMETER]───┐
L1  │   │   TRENCH A INLET     │               │   AMBER BLAST COAMING     │
L0  │   └───[SUB-GRADE SLAB]───┘               └───[SUB-GRADE SLAB]────────┘
    │   ◄───── X: −6 to −1 ────►               ◄───── X: 0 to +6 ──────────►

Station 2 · Dual Intake Terminals (Z = +9, Looking South)
Level
L2  │   [STOP] [CAT DUMP CONN] [STOP]          [CRADLE P] [MULE] [CRADLE S]  ◄── 30° Clamps Engage
L1  │   ■══════[5× COLLECTORS]══════■          ■──────────[PAD CONN]──────■  ◄── Flush Receiving Ports
L0  │   └───[CONVEYOR TRENCH A]─────┘          └───[CONVEYOR TRENCH B]────┘

Station 3 · Heavy Smelting Array (Z = +3, Looking South)
Level
L2  │   ┌───────[YIELD 1..4]───────┐           ┌───────[YIELD 9..12]──────┐
    │   │ REFINERY 1 & 2 (WEST)    │           │ REFINERY 3 & 4 (EAST)    │  ◄── 4× Large Refineries
L1  │   ├──────────────────────────┤           ├──────────────────────────┤      (Teal, 200% Yield)
L0  │   └───[RARE ORE MANIFOLD]────┴───────────┴───[BULK ORE MANIFOLD]────┘

Station 4 · Cooperative Fabrication Core (Z = −3, Looking South)
Level
L2  │   ┌───────[SPEED 1..4]───────┐           ┌───────[SPEED 9..12]──────┐
    │   │ ASSEMBLER 1 & 2 (WEST)   │           │ ASSEMBLER 3 & 4 (EAST)   │  ◄── 4× Large Assemblers
L1  │   ├──────────────────────────┤           ├──────────────────────────┤      (Cobalt, 400% Speed)
L0  │   └───[REFINED INGOT TRUNK]──┴───────────┴───[COMPONENT OUTFLOW]────┘

Station 5 · Power Substation & Buffer Warehouse (Z = −9, Looking South)
Level
L3  │                                          ┌───[TOP INSPECTION PORT]──┐
L2  │   [BAT 1..6] [RCT 1..2]                  │  GND STR COMPONENTS      │  ◄── Large Cargo (3×3×3 B)
L1  │   ┌──────────────────────────┐           │  (LOGISTICS EMERALD)     │
L0  │   └───[POWER BUS TRENCH]─────┴───────────┴───[EXTRUDER FEED RISER]──┘

Station 6 · South Extruder Ring Interface (Z = −13, Looking South)
Level
L2  │                                          ┌───[GND SORTER EXT PUSH]──┐  ◄── Drain All = ON
L1  │   ═══════════════════════════════════════╧═══════════╤══════════════╧  ◄── Tarmac Sill
L0  │                                                      ▼ (Feeds Chapter 5 Extruder Pusher Spine)
```

---

### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

The facility routes materials through dedicated dual-tier smelting loops before aggregating into the master component buffer feeding the Chapter 5 Rig Extruder:

```
                            TERRESTRIAL INTAKE NODES
                                       │
       ┌───────────────────────────────┴───────────────────────────────┐
       ▼ (Tier 1: Air / Rare)                                          ▼ (Tier 2: Ground / Bulk)
[MULE-01 Belly Connector] (Z +9, X +3)                       [CATERPILLAR-01 Dump Conn] (Z +9, X −4)
[GND PAD Conn] (Facing UP)                                   [GND CAT Dump Conn] + 5× In-Deck Collectors
       │                                                               │
       ▼ (Trench B: Rare Stream)                                       ▼ (Trench A: Bulk Stream)
[GND Sorter Rare Intake]                                     [GND Sorter Bulk Intake]
(Whitelist: U, Pt, Au, Mg, Scrap)                           (Whitelist: Fe, Ni, Si, Co, Stone)
       │                                                               │
       ▼                                                               ▼
[GND REF 1 & 2 (West Bank)]                                  [GND REF 3 & 4 (East Bank)]
(4× Large Refineries · 16× Yield Modules)                    (4× Large Refineries · 16× Yield Modules)
       │                                                               │
       └───────────────────────────────┬───────────────────────────────┘
                                       │
                                       ▼ (Refined Ingot Aggregation Trunk)
       ┌───────────────────────────────┴───────────────────────────────┐
       ▼                                                               ▼
[GND ASM 1 & 2 (Structural Hull Run)]                        [GND ASM 3 & 4 (Machinery Run)]
(Steel Plates, Interior Plates, Construction Comps)          (Large Tubes, Motors, Computers, Thruster Comps)
(Cooperative Mode = Master)                                  (Cooperative Mode = Slave)
       │                                                               │
       └───────────────────────────────┬───────────────────────────────┘
                                       │
                                       ▼ (Fabricated Components Trunk)
                           [GND STR Components]
                           (Large Cargo Container 3×3×3 B · Z −10, X +3)
                             │ ◄── Manual Component Withdrawal Hatch (Port L2)
                             ▼
                           [GND Sorter Extruder Feed] (Conveyor Sorter · Drain All = ON)
                             │
                             ▼ (High-Pressure Conveyor Conduit South)
                   [EXT Welder Ring] (Chapter 5 Shipyard · Z −14)
```

### Physical Maintenance Ports & Manual Access Hatches
To comply with survival maintenance standards and allow physical clearance of conveyor bottlenecks:
* **Port 1 (Mule Intake):** Standard small conveyor port at [X +3, Z +7, L1] for manual inspection of rare ore intake.
* **Port 2 (Caterpillar Intake):** Standard large port at [X −4, Z +7, L1] for manual clearance of oversize quarry boulders.
* **Ports 3 & 4 (Refinery Manifolds):** Manual inventory access ports exposed on the northern face of `GND REF 1` and `GND REF 3` at Level L2.
* **Ports 5 & 6 (Assembler Manifolds):** Manual component withdrawal ports exposed on the southern face of `GND ASM 2` and `GND ASM 4` at Level L2.
* **Port 7 (Buffer Warehouse):** Large maintenance hatch exposed on the western face of `GND STR Components` at [X +1, Z −10, L2].

---

### 0.6 Master Electrical Single-Line Schematic & 5-Mode Energy Management Suite

```
[MASTER ELECTRICAL SINGLE-LINE SCHEMATIC]
PRIMARY ENERGY STORAGE:
  [GND BAT 1..6] (6× Large Batteries, Vault Z −9, X −4) ────────────► 18.0 MWh Capacity / 72.0 MW Surge
EMERGENCY NUCLEAR GENERATION:
  [GND RCT Aux 1..2] (2× Small Nuclear Reactors, Z −11, X −4) ─────► 30.0 MW Backup Generation
C2 UNINTERRUPTIBLE POWER SUPPLY (UPS):
  [GND BAT C2 UPS] (1× Large Battery, Dedicated Server Vault L2) ──► 3.0 MWh Isolated Reserve
MAIN FACILITY POWER BUS:
  ├────────────────────────────────────────────────────────────────► 102.0 MW Peak Instantaneous
  │
  ├─► HEAVY REFINING ARRAY (4× Refineries + 16× Modules) ──────────► 13.2 MW Continuous
  ├─► MANUFACTURING FOUNDRY (4× Assemblers + 16× Modules) ────────► 2.2 MW Continuous
  ├─► SHIPYARD EXTRUSION LOAD (22× Welders + Pusher Pistons) ──────► 8.8 MW Peak Pulsed
  ├─► LOGISTICS SHORE CHARGERS (Mule Pad 12 MW + Cat Dock 6 MW) ───► 18.0 MW Fast Recharge
  └─► C2 COMMAND & COMMS (Server Racks, Video Wall, Laser Spire) ──► 0.6 MW Protected Load
```

### 5 Formal Operational Facility Power Modes
1. **Mode 1 · Silent Watchkeeping (Standby / Night Operations):**
   * Output: `1.2 MW`. Refineries and assemblers idle; shore chargers off; batteries set to `Auto`; comms and C2 bridge active.
2. **Mode 2 · Daytime Ingot Smelting (Ore Processing Focus):**
   * Output: `15.5 MW`. All 4 refineries operating at 100% capacity (200% yield); assemblers idle; battery storage provides baseline support.
3. **Mode 3 · Full Industrial Surge Extrusion (Rig Section Printing):**
   * Output: `32.5 MW` (30 MW Nuclear + 2.5 MW Battery assist). All 4 refineries, 4 assemblers, and Chapter 5 Extruder Ring welders firing simultaneously.
4. **Mode 4 · Fast Terminal Shore Recharge (Mule & Cat Docked):**
   * Output: `48.0 MW` (Battery surge discharge). High-current DC power pumped into docked MULE-01 (12 MW) and CATERPILLAR-01 (6 MW) to achieve turnaround in `< 60 s`.
5. **Mode 5 · Blackout Emergency UPS Mode (Grid Failure Failover):**
   * Output: `0.6 MW`. Industrial grid completely shed via `GND EC PowerGuard`; `GND BAT C2 UPS` isolates to maintain unbroken laser antenna connection to the Floating Rig and regional fleet telemetry.

---

### 0.7 Master Facility Bill of Materials (BOM) & Mass Budget

| Subassembly Zone | Major Block Types & Specifications | Block Count | Dry Mass (t) | PCU | Power Rating |
|---|---|---|---|---|---|
| **Foundation & Tarmac** | Heavy armor blocks, blast slabs, retaining walls, sub-surface raceways | 548 | 468.0 | 548 | Passive |
| **Mule Gantry Pad** | 7×7 B deck, 1× Connector, 2× Motor Hinges, 2× Mag Plates, 1× Sensor | 56 | 23.2 | 285 | 12.0 MW Shore Bus |
| **Caterpillar Unload Dock**| 1× Connector, 5× Collectors, 2× Blast Curbs, Sorter Manifold | 34 | 35.5 | 335 | 6.0 MW Shore Bus |
| **Heavy Smelting Array** | 4× Large Refineries, 16× Yield Modules, Ingot Conduit Headers | 24 | 191.2 | 672 | 13.2 MW Draw |
| **Manufacturing Foundry** | 4× Large Assemblers, 16× Speed Modules, Cooperative Manifolds | 24 | 82.0 | 616 | 2.2 MW Draw |
| **Storage & Logistics** | 1× Large Cargo (3×3×3 B), 4× Sorters, Extruder Feed Conveyor Trunk | 52 | 41.5 | 280 | 1.0 MW Draw |
| **Power Substation** | 6× Large Batteries, 2× Small Reactors, Substation Switchgear | 14 | 53.2 | 390 | 30 MW Gen / 72 MW Surge |
| **C2 Command Bridge** | 3× Control Desks, 6× Large LCDs, 3× PBs, Glass, Catwalks, Decon Door | 78 | 48.0 | 740 | 0.4 MW Draw |
| **Comms & Laser Spire** | 1× 50 km Omni Antenna, 1× Laser Antenna, 2× Cameras, Mast Armor | 12 | 14.5 | 425 | 0.2 MW Draw |
| **FACILITY TOTALS** | **Complete Surface Logistics Terminal & Regional C2 Hub** | **842** | **≈ 957.1 t** | **4,291** | **30 MW Gen / 102 MW Peak** |

* All structures, foundation slabs, and mechanical anchorages calculated at Earth 1 g baseline.
* Values tagged with `†` awaiting in-game confirmation in RIG Test:
  * Large Refinery continuous draw at full load = `3.3 MW` per unit (13.2 MW total)†.
  * Large Assembler continuous draw at full load = `560 kW` per unit (2.24 MW total)†.
  * Laser Antenna maximum power draw during atmospheric beam link = `2.5 MW`†.

---

### 0.8 Master Player Circulation, Pressurized Corridors & Decontamination Airlocks

To ensure complete physical ergonomics and survivability for engineering personnel:
1. **Perimeter Catwalks & Safety Handrails (Level L1):** High-viz yellow catwalks (Palette Slot 8) border the tarmac deck, separating pedestrian traffic from the active Caterpillar rail trench and Mule flight apron.
2. **Pressurized C2 Tower Access Stairwell (Levels L1-L2):** Enclosed armored stairwell climbs from the western tarmac sill into the Level L2 Server Room through an exterior sliding airlock.
3. **Active Gas-Reclamation Decontamination Airlock (Level L3):** Direct personnel entry into the panoramic command bridge passes through a dual-door sealed airlock chamber equipped with active depressurization air vents (`GND Vent Decon Bridge`), cyan UV washdown lighting, and automated cycle interlocks. Outbound cycles evacuate 100% of cabin O2 before outer doors open.
4. **Command Bridge Facilities (Levels L3-L4):** Walk-around observation deck, Tier 4 survival armory lockers (`GND Armory Bridge`), medical station (`GND Medical Bay`), and suit oxygen/hydrogen recharge stations adjacent to operator desks.

---

### 0.9 GROUND-02 (MKII) Heavy Armored Citadel & Defense Grid Roadmap

* **Operational Context:** In advanced survival scenarios or contested planetary sectors subject to Modular Encounter Spawner (MES) hostile raids:
  1. **Perimeter CIWS Gatling Turrets:** Mount 4× Large Grid Gatling Turrets (`GND Turret North/South/East/West`) along tarmac corners plumbed to the central ammunition buffer hold.
  2. **Automated Combat Lockdown:** `[GND] PB FleetOps` detects target lock/damage; sounds `GND Alarm Raid`; triggers `GND EC Combat` (seals all airlocks, retracts Mule cradle arms, arms CIWS turrets).
  3. **Planetary Shield Generator Bunker:** Reserve foundation space in South Zone (X −9, Z −11) to mount a Defense Shields generator protecting the base and Extruder shipyard.

---

## Phase 4-1 · Heavy Foundation Tarmac, Retaining Walls & Sub-Surface Trenches (Level L1)

```
       [PHASE 4-1 SPATIAL LOCATOR HUD]
       Facility: Ground Base Foundation (GND-01)
       Envelope: 24 W × 28 L × 2 H B (60.0 × 70.0 × 5.0 m)
       Coordinate Range: X −12 to +12, Z −14 to +14, Level L0 to L1
       Key Datums:
       · Foundation Surface Deck: Level L1 (Y = 0)
       · Sub-Surface Bedrock Trench Depth: Level L0 (Y = −2.5 m)
       · Trench A (Caterpillar Bulk Ore): X −4, Z +5 to +13
       · Trench B (Mule Rare Ore): X +3, Z +5 to +13
       · Main Industrial Manifold Raceway: Centerline X0, Z −13 to +5
```

### Pre-Step Master Parts Tray: Phase 4-1
* 450× Heavy Armor Blocks (Palette Slot 0: `#2B303A`, 382.5 t, 450 PCU)
* 64× Heavy Armor Half-Blocks (Palette Slot 0, 27.2 t, 64 PCU)
* 34× Heavy Armor Slopes (Palette Slot 0, 17.0 t, 34 PCU)
* 24× Large Conveyor Junctions (Palette Slot 4: `#1ABC9C`, 19.2 t, 144 PCU)
* 16× Large Conveyor Tubes (Palette Slot 4, 9.6 t, 96 PCU)

---

### Step 4.1 · Bedrock Plateau Excavation & Perimeter Retaining Walls (L0-L1)
Excavate a level 60 × 70 m plateau into bedrock:
1. Lay the perimeter retaining wall using Heavy Armor Blocks along X = ±12 (Z −14 to +14) and Z = ±14 (X −12 to +12) at Level L1.
2. Anchor corner footings 2 blocks deep into terrain (Level L0) to prevent voxel clipping or structural floating.
3. Bevel the exterior tarmac perimeter with Heavy Armor Slopes angled 45° to provide smooth ramp transitions for ground vehicles.

---

### Step 4.2 · Sub-Surface Conveyor Trenches A & B (L0)
Recess two longitudinal conveyor raceways beneath the tarmac deck to keep all raw ore piping hidden and blast-protected:
1. Excavate Trench A (3 B wide × 1 B deep) along X −4 spanning Z +5 to +13 at Level L0. Plumb with 8× Large Conveyor Junctions to feed the Caterpillar receiving dock.
2. Excavate Trench B (3 B wide × 1 B deep) along X +3 spanning Z +5 to +13 at Level L0. Plumb with 6× Large Conveyor Junctions to feed the Mule gantry pad.
3. Connect both trenches across transverse headers at Z = +5 to the central industrial manifold raceway.

---

### Step 4.3 · Foundation Blast Slabs & Tarmac Deck Plating (L1)
1. Lay reinforced Heavy Armor Blocks across the entire 24 × 28 B surface deck at Level L1, enclosing the sub-surface raceways.
2. Leave open intake collar sockets at [X +3, Z +9] (for the Mule pad connector) and along X −4, Z +8 to +10 (for the 5× Caterpillar collectors).
3. Paint the entire tarmac foundation Palette Slot 0 (Industrial Slate).

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.1A: SUB-SURFACE CONVEYOR INTEGRITY**
> Before sealing the heavy tarmac plates over Trenches A and B:
> 1. Verify 100% large-conveyor continuity from the northern logistics nodes to the central industrial headers.
> 2. Ensure zero small-conveyor tubes are installed anywhere in the main line (ore transfer requires large-tube flow).
> 3. Perform a pressure-tight seal check on all recessed conveyor cubes.

---

#### Visual Milestone 4-1 · Tarmac Foundation Complete

```
Visual Checkpoint Silhouette (Looking South across Level Tarmac Deck L1):
        X: −11 (C2 Pylon)             Centerline X0             X: +11 (East Apron)
           ┌──────────┐                                                ┌──────────┐
           │ PYLON L1 │                                                │ EAST SILL│
      ═════╧══════════╧════════════════════════════════════════════════╧══════════╧═════
                   24 × 28 B LEVEL INDUSTRIAL BLAST TARMAC (L1)
```

* [ ] Foundation is 100% sealed with zero exposed voxel terrain gaps.
* [ ] Trench raceways provide unrestricted large-port conveyor flow beneath the surface.

---

## Phase 4-2 · Dual Logistics Terminals (Mule Pad & Caterpillar Dock) (Levels L1-L2)

```
       [PHASE 4-2 SPATIAL LOCATOR HUD]
       Zone: North Logistics Apron
       Envelope: 20 W × 8 L × 2 H B (50.0 × 20.0 × 5.0 m)
       Coordinate Range: X −6 to +6, Z +6 to +13, Level L1 to L2
       Key Interfaces:
       · MULE-01 Gantry Cradle Deck: Centered [X +3, Z +9, Level L1]
       · CATERPILLAR-01 Unload Dock: Centered [X −4, Z +9, Level L1]
```

### Pre-Step Master Parts Tray: Phase 4-2
* 49× Heavy Armor Blocks (Palette Slot 1: `#E6A100`, 41.6 t, 49 PCU)
* 2× Large Connectors (Palette Slot 1 & 2, 6.2 t, 300 PCU)
* 2× Motor Hinges (Large) (Palette Slot 1, 8.0 t, 200 PCU)
* 2× Magnetic Plates (Large) (Palette Slot 1, 4.8 t, 50 PCU)
* 5× Collectors (Large Grid) (Palette Slot 2: `#9E2A2B`, 18.5 t, 125 PCU)
* 2× Conveyor Sorters (Palette Slot 3, 2.4 t, 80 PCU)
* 1× Sensor Block (Palette Slot 1, 0.3 t, 40 PCU)
* 18× Heavy Armor Slopes 2×1×1 (Palette Slot 2, 13.5 t, 18 PCU)

---

### Step 4.4 · MULE-01 Touchdown Blast Deck (7×7 B Flush Deck) (L1)
1. Construct the 7 × 7 B touchdown deck spanning X 0 to +6, Z +6 to +12 at Level L1.
2. Paint the deck perimeter with alternating Hazard Amber (Slot 1) and Industrial Slate (Slot 0) warning chevrons.
3. Install Large Connector `GND PAD Conn` flush in the deck center at [X +3, Z +9, Level L1], facing straight UP (+Y), plumbed directly into Trench B.

#### Terminal Configuration:
* Connector: `GND PAD Conn` (Auto-Connect: `OFF` · Collect All: `OFF` · Connect Strength: `0.015`).

---

### Step 4.5 · Motorized Gantry Cradle Clamping Arms & Mag Plates (L1-L2)
Assemble the active landing cradle mechanism per **Decision 45**:
1. Mount 1× Motor Hinge `GND PAD Cradle Hinge Port` at [X +1, Z +10, Level L1] with hinge pin longitudinal (Z-axis) and base facing East (+X).
2. Mount 1× Motor Hinge `GND PAD Cradle Hinge Stbd` at [X +5, Z +10, Level L1] with hinge pin longitudinal (Z-axis) and base facing West (−X).
3. Weld Large Magnetic Plates `GND PAD Cradle Clamp Port/Stbd` onto the hinge heads, set to `Auto-Lock = ON`.
4. Install Sensor Block `GND PAD Sensor` at [X +3, Z +12, Level L1]. Configure detection field: 15 m radius centered 5 m above pad.

#### Joint Tuning Sliders:
* **Share Inertia Tensor = `ON`** on both cradle hinges.
* **Hinge Limits:** Lower Limit = `0.0°` (arms flush with deck); Upper Limit = `30.0°` (arms swung inward clamping Mule flank rub rails).
* **Braking Torque:** `20.0 MN·m`†.

---

### Step 4.6 · CATERPILLAR-01 Unload Dock, Wheel Curbs & Stop Bumpers (L1-L2)
1. Construct the heavy rail receiving trench along X −6 to −1, Z +6 to +12 at Level L1.
2. Install raised Heavy Armor Slope curbs along X = −6 and X = −1 to physically funnel the land train's 5×5 wheels into alignment.
3. Weld heavy collision stop blocks using Blast Door Blocks at Z = +5 (X −3 to −5).
4. Mount Large Connector `GND CAT Dump Conn` horizontally at [X −4, Z +5, Level L2] facing North (+Z). Height: exactly 2.5 m above tarmac.

---

### Step 4.7 · 5× In-Deck Gravity Collectors & Bulk Sorter Manifold (L1)
1. Mount 5× Large Grid Collectors `GND CAT Hopper Coll 1..5` flush in the deck at Level L1, facing straight UP (+Y):
   * `COLL 1, 2`: [X −5, Z +10] and [X −3, Z +10]
   * `COLL 3, 4`: [X −5, Z +9] and [X −3, Z +9]
   * `COLL 5`: [X −4, Z +8]
2. Plumb all 5 collectors into sub-surface Trench A.
3. Install Large Conveyor Sorter `GND CAT Sorter Bulk` downstream of the collectors at [X −4, Z +5, Level L0] with flow arrow South (−Z). Whitelist: `Iron`, `Nickel`, `Silicon`, `Cobalt`, `Stone`. Set `Drain All = ON`.

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 4.2A: LOGISTICS DOCK VERIFICATION**
> 1. Test Mule cradle sensor: walk into detection field; confirm cradle arms swing inward 30° smoothly without binding.
> 2. Verify `GND CAT Dump Conn` centerline height aligns within ±0.05 m of `CAT Train Unload Conn` (2.5 m).
> 3. Verify both sorters pull continuously from their respective receiving ports.

---

## Phase 4-3 · Heavy Smelting Array (4 Refineries + 16 Yield Modules) (Levels L1-L2)

```
       [PHASE 4-3 SPATIAL LOCATOR HUD]
       Zone: Central Industrial Smelting
       Envelope: 16 W × 6 L × 3 H B (40.0 × 15.0 × 7.5 m)
       Coordinate Range: X −6 to +6, Z +1 to +5, Level L1 to L3
       Refinery Centroids:
       · West Bank (Rare Ore): `GND REF 1` [X −5, Z +3], `GND REF 2` [X −5, Z +1]
       · East Bank (Bulk Ore): `GND REF 3` [X +4, Z +3], `GND REF 4` [X +4, Z +1]
```

### Pre-Step Master Parts Tray: Phase 4-3
* 4× Large Refineries (Palette Slot 3: `#1ABC9C`, 100.8 t, 360 PCU)
* 16× Yield Modules (Palette Slot 3, 83.2 t, 240 PCU)
* 16× Large Conveyor Tubes (Palette Slot 4: `#1ABC9C`, 9.6 t, 96 PCU)
* 8× Large Conveyor Junctions (Palette Slot 4, 6.4 t, 48 PCU)

---

### Step 4.8 · West Smelting Bank (Refineries 1 & 2 + 8 Yield Modules) (L1-L2)
Install the dedicated rare-ore processing bank:
1. Mount 2× Large Refineries `GND REF 1` and `GND REF 2` at [X −5, Z +3] and [X −5, Z +1] at Level L1, with large ports facing North/South.
2. Attach 4× Yield Modules to the exterior Port face of `GND REF 1` and 4× Yield Modules to `GND REF 2`.
3. Plumb the intake ports directly to Trench B (Rare Ore Stream from Mule Pad).

---

### Step 4.9 · East Smelting Bank (Refineries 3 & 4 + 8 Yield Modules) (L1-L2)
Install the dedicated bulk-ore processing bank:
1. Mount 2× Large Refineries `GND REF 3` and `GND REF 4` at [X +4, Z +3] and [X +4, Z +1] at Level L1.
2. Attach 4× Yield Modules to the exterior Starboard face of `GND REF 3` and 4× Yield Modules to `GND REF 4`.
3. Plumb the intake ports directly to Trench A (Bulk Ore Stream from Caterpillar Dock).
4. Tie all 4 refinery discharge ports into the central Refined Ingot Trunk running South along centerline X0.

#### Terminal Verification:
* Inspect all 4 refineries in terminal: confirm `Productivity: 100%` and `Effectiveness: 200%` (maximum vanilla yield).

---

## Phase 4-4 · Cooperative Fabrication Core (4 Assemblers + 16 Speed Modules) (Levels L1-L2)

```
       [PHASE 4-4 SPATIAL LOCATOR HUD]
       Zone: Central Industrial Fabrication
       Envelope: 16 W × 6 L × 2 H B (40.0 × 15.0 × 5.0 m)
       Coordinate Range: X −6 to +6, Z −5 to −1, Level L1 to L2
       Assembler Centroids:
       · West Bank: `GND ASM 1` [X −4, Z −2], `GND ASM 2` [X −4, Z −4]
       · East Bank: `GND ASM 3` [X +3, Z −2], `GND ASM 4` [X +3, Z −4]
```

### Pre-Step Master Parts Tray: Phase 4-4
* 4× Large Assemblers (Palette Slot 4: `#2980B9`, 38.8 t, 160 PCU)
* 16× Speed Modules (Palette Slot 4, 38.4 t, 240 PCU)
* 1× Large Cargo Container (3×3×3 B) (Palette Slot 5: `#27AE60`, 28.5 t, 250 PCU)
* 1× Large Conveyor Sorter (Palette Slot 5, 1.2 t, 40 PCU)
* 12× Large Conveyor Junctions (Palette Slot 5, 9.6 t, 72 PCU)

---

### Step 4.10 · 4-Assembler Manufacturing Array & 16× Speed Modules (L1-L2)
1. Mount 4× Large Assemblers `GND ASM 1..4` at [X −4, Z −2], [X −4, Z −4], [X +3, Z −2], and [X +3, Z −4] at Level L1.
2. Plumb top and side upgrade ports with 4× Speed Modules per assembler (16 total).
3. Configure `GND ASM 1` as **Master Controller**; set `Cooperative Mode = ON` on `GND ASM 2, 3, 4`.
4. Verify terminal displays `Productivity: 400%` across all 4 units.

---

### Step 4.11 · Component Buffer Warehouse & Extruder Push Sorter (L1-L3)
1. Mount 1× Large Cargo Container `GND STR Components` (3×3×3 B) centered at [X +3, Z −10, Level L1-L3].
2. Plumb the container's northern ports into the Assembler component outflow trunk.
3. Mount 1× Large Conveyor Sorter `GND Sorter Extruder Feed` at [X +3, Z −13, Level L1] with flow arrow South (−Z), feeding the Chapter 5 Extruder Ring shipyard. Set `Drain All = ON`.

---

## Phase 4-5 · Planetary Power Substation & Battery Vault (Levels L1-L2)

```
       [PHASE 4-5 SPATIAL LOCATOR HUD]
       Zone: South Power Substation
       Envelope: 8 W × 8 L × 3 H B (20.0 × 20.0 × 7.5 m)
       Coordinate Range: X −8 to 0, Z −13 to −7, Level L1 to L3
       Key Assets:
       · Battery Vault: `GND BAT 1..6` [X −4, Z −9, Level L1-L2]
       · Nuclear Backup: `GND RCT Aux 1..2` [X −4, Z −12, Level L2]
```

### Pre-Step Master Parts Tray: Phase 4-5
* 6× Large Batteries (Palette Slot 6: `#C0392B`, 30.0 t, 120 PCU)
* 2× Small Nuclear Reactors (Palette Slot 6, 3.2 t, 50 PCU)
* 24× Heavy Armor Blocks (Palette Slot 6, 20.4 t, 24 PCU)

---

### Step 4.12 · 18 MWh Large Battery Bank & Nuclear Backup Vault (L1-L2)
1. Install 6× Large Batteries `GND BAT 1..6` arranged in a 3 × 2 grid centered at [X −4, Z −9, Level L1-L2]. Total capacity: 18.0 MWh; surge output: 72.0 MW. Set to `Auto`.
2. Install 2× Small Nuclear Reactors `GND RCT Aux 1..2` at [X −4, Z −12, Level L2]. Total capacity: 30.0 MW backup. Plumb with small conveyor tubing for automated uranium feed.
3. Enclose the power vault with Heavy Armor Blocks painted Palette Slot 6 (High-Voltage Red).

---

## Phase 4-6 · Elevated C2 Operations Tower, Panoramic Bridge & Comms Spire (Levels L1-L9)

```
       [PHASE 4-6 SPATIAL LOCATOR HUD]
       Zone: West Elevated C2 Command Tower
       Envelope: 5 W × 5 L × 9 H B (12.5 × 12.5 × 22.5 m)
       Coordinate Range: X −11 to −7, Z +1 to +5, Level L1 to L9
       Decks:
       · L1: Heavy Structural Blast Pylon Anchor
       · L2: Fleet Operations Server Room & UPS Vault
       · L3-L4: Panoramic 3-Operator Command Bridge & Video Wall
       · L5-L9: Long-Range Regional Comms Spire & Rig Laser Link
```

### Pre-Step Master Parts Tray: Phase 4-6
* 3× Programmable Blocks (Palette Slot 7: `#1B263B`, 3.6 t, 75 PCU)
* 3× Control Stations (Palette Slot 7, 1.8 t, 45 PCU)
* 6× Large LCD Panels (Palette Slot 7, 2.4 t, 150 PCU)
* 1× Large Battery (Dedicated UPS) (Palette Slot 7, 5.0 t, 20 PCU)
* 1× Large Grid Radio Antenna (Palette Slot 7, 8.0 t, 100 PCU)
* 1× Large Grid Laser Antenna (Palette Slot 9: `#ECF0F1`, 2.5 t, 200 PCU)
* 2× Large Grid Cameras (Palette Slot 7, 0.2 t, 40 PCU)
* 24× Sloped Armor Glass (Palette Slot 7, 4.8 t, 24 PCU)
* 1× Sliding Airtight Door (Palette Slot 7, 0.8 t, 25 PCU)

---

### Step 4.13 · Level L2 Server Room, PB Rack & Dedicated UPS (L2)
1. Build the enclosed 5 × 5 B server room deck at Level L2 ([X −9, Z +3]).
2. Mount 3× Programmable Blocks along the northern bulkhead:
   * `[GND] PB FleetOps` : Regional fleet coordination, JIT dispatch, and C2 handover root.
   * `[GND] PB Inventory` : Industrial quotas, sorting, and cooperative assembler management.
   * `[GND] PB Displays` : Zero-GC text rendering engine for the 6-panel video wall.
3. Mount 1× Large Battery `GND BAT C2 UPS` in the southern floor cavity.
4. Install Event Controller `GND EC PowerGuard` and Timer Block `GND Timer Watchdog`.

---

### Step 4.14 · Level L3-L4 Panoramic Command Bridge & 3-Operator Desks (L3-L4)
1. Construct the elevated command deck at Level L3 cantilevered outward over the western tarmac.
2. Mount 3× Control Stations side-by-side facing East (+X) overlooking the facility:
   * `GND Desk AirOps` (Station 1 · Port/North): Orca carrier telemetry, Remora plunge status, Mule traffic.
   * `GND Desk GroundOps` (Station 2 · Center): Mastodon 35 m boom CTC, Caterpillar dump queue, pull quotas.
   * `GND Desk YardOps` (Station 3 · Starboard/South): Extruder pusher feed, welder wall, inventory reserves.
3. Enclose the bridge with 2-layer Sloped Armor Glass angled 45° outward, providing a 270° panoramic view over the North Apron, Industrial Foundry, and South Yard.
4. Mount 1× Sliding Airtight Door `GND Door Decon Bridge` and Air Vent `GND Vent Decon Bridge` to form a pressurized decontamination airlock.

---

### Step 4.15 · Level L4 6-Panel Panoramic LCD Video Wall (L4)
Mount 6× Large LCD Panels `GND LCD Video Wall 1..6` in a continuous 3 × 2 video matrix along the forward bridge bulkhead above the viewports:
* `LCD 1` : Aerial Fleet Roster (Orca status, Remora shafts, Mule flight ETA).
* `LCD 2` : Ground Fleet Roster (Mastodon macro status, Caterpillar consist speed, buffer mass).
* `LCD 3` : Shipyard Extruder Status (Pusher stroke, welder active count, layer print %).
* `LCD 4` : Smelting Core Throughput (Refinery effectiveness, ingot flow rates, gravel buffer).
* `LCD 5` : Stockpiles & Quotas (Steel plates, motors, computers, uranium reserves).
* `LCD 6` : Tactical & Defense (Grid status, antenna link integrity, radar tracks, weather).

---

### Step 4.16 · Level L5-L9 Comms Spire & Floating Rig Laser Link (L5-L9)
1. Erect the 10 m communications mast from the bridge roof at [X −9, Z +1] spanning Levels L5 to L9.
2. Mount 1× Large Grid Radio Antenna `GND Antenna Fleet` at Level L6. Set broadcast range to `50,000 m`.
3. Mount 2× Fixed Cameras: `GND Cam North Pads` (facing +Z) and `GND Cam South Yard` (facing −Z).
4. Crown the mast tip at Level L9 with 1× Laser Antenna `GND Laser Rig Relay` painted Palette Slot 9 (White). Acquire and lock GPS coordinates of the Floating Rig Fortress in the upper atmosphere.

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 4.6A: C2 BRIDGE COMMISSIONING GATE**
> 1. Verify `GND Door Decon Bridge` seals airtight; bridge cabin pressurizes to 100% O2.
> 2. Verify all 6 LCD panels report active display surfaces under `[GND] PB Displays`.
> 3. Verify `GND Laser Rig Relay` acquires and locks high-altitude target without line-of-sight obstruction.

---

## Phase 4-7 · Master Control Logic, Multi-PB Architecture & Software Abstraction

### 1. Multi-PB Server Architecture
* `[GND] PB FleetOps` (Update10):
  * Manages regional fleet dispatch, JIT transport pre-dispatch (`TMS.LOGISTICS.JIT`), pull economy quotas (`TMS.BASE.DEMAND`), and seamless C2 authority handover from Mastodon (`TMS.C2.HANDOVER`).
* `[GND] PB Inventory` (Update100):
  * Manages refinery intake manifolds, cooperative assembler production queues, and southern Extruder feed quotas.
* `[GND] PB Displays` (Update100):
  * Zero-GC monospace ASCII text renderer formatting telemetry for `GND LCD Video Wall 1..6`.

### 2. Group Taxonomy
* `GND Refineries` : `GND REF 1..4` (All 4 Large Refineries).
* `GND Assemblers` : `GND ASM 1..4` (All 4 Large Assemblers).
* `GND Yield Modules` : All 16 Yield upgrade modules.
* `GND Speed Modules` : All 16 Speed upgrade modules.
* `GND Fleet Displays` : `GND LCD Video Wall 1..6`.
* `GND Power Batteries` : `GND BAT 1..6`.
* `GND PAD Cradle` : `GND PAD Cradle Hinge Port/Stbd`, `GND PAD Cradle Clamp Port/Stbd`.
* `GND CAT Hopper` : `GND CAT Hopper Coll 1..5`, `GND CAT Dump Conn`.

### 3. Block Naming & Terminal Dictionary
* `GND PAD Conn` : Large Connector flush on Mule pad [X +3, Z +9, L1].
* `GND PAD Cradle Hinge Port/Stbd` : Motor Hinges driving Mule gantry clamps [X +1/+5, Z +10, L1].
* `GND PAD Cradle Clamp Port/Stbd` : Magnetic Plates on cradle heads.
* `GND PAD Sensor` : 15 m proximity sensor on Mule pad [X +3, Z +12, L1].
* `GND CAT Dump Conn` : Large Connector at Caterpillar dock [X −4, Z +5, L2].
* `GND CAT Hopper Coll 1..5` : 5× In-Deck Collectors at Caterpillar dock [L1].
* `GND Sorter Rare Intake` : Sorter on Trench B [X +3, Z +5, L0].
* `GND Sorter Bulk Intake` : Sorter on Trench A [X −4, Z +5, L0].
* `GND REF 1..4` : Large Refineries in smelting array [L1-L2].
* `GND ASM 1..4` : Large Assemblers in manufacturing foundry [L1-L2].
* `GND STR Components` : Large Cargo Container buffer warehouse [X +3, Z −10, L1-L3].
* `GND Sorter Extruder Feed` : Sorter on extruder feed line [X +3, Z −13, L1].
* `GND BAT 1..6` : 6× Large Batteries in main substation [X −4, Z −9, L1-L2].
* `GND BAT C2 UPS` : Isolated 1× Large Battery in C2 server room [L2].
* `GND RCT Aux 1..2` : 2× Small Nuclear Reactors in power vault [X −4, Z −12, L2].
* `GND Desk AirOps` : Control Station 1 in C2 bridge [L3].
* `GND Desk GroundOps` : Control Station 2 in C2 bridge [L3].
* `GND Desk YardOps` : Control Station 3 in C2 bridge [L3].
* `GND LCD Video Wall 1..6` : 6× Large LCD Panels in C2 bridge [L4].
* `GND Antenna Fleet` : 50 km Omni Radio Antenna on mast [L6].
* `GND Laser Rig Relay` : Laser Antenna on mast crown [L9].

---

### 4. Event Controller Matrix (Rule C4 Compliance)

| Event Controller | Monitored Block / Variable | Condition & Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| **`GND EC PadClamp`** | `GND PAD Sensor` | Detects MULE $\le 10\text{ m}$ | `GND PAD Cradle` Swing In 30° | `GND PAD Cradle` Retract Flush 0° |
| **`GND EC PadLock`** | `GND PAD Conn` | Connector: `Connected` | (1) `GND Sorter Rare Intake` Drain ON<br/>(2) `GND BAT 1..6` Shore Power Mode | (1) `GND Sorter Rare Intake` Drain OFF<br/>(2) `GND BAT 1..6` Auto Mode |
| **`GND EC CatDump`** | `GND CAT Dump Conn` | Connector: `Connected` | `GND Sorter Bulk Intake` Drain ON | `GND Sorter Bulk Intake` Drain OFF |
| **`GND EC PowerGuard`**| `GND Power Batteries` | Grid Stored Power $< 15\%$ | (1) `GND RCT Aux 1..2` Turn ON<br/>(2) Non-essential lights OFF | (1) `GND RCT Aux 1..2` Turn OFF<br/>(2) Normal grid restored |
| **`GND EC C2 UPS`** | Main Substation Power | Main Bus Failure | `GND BAT C2 UPS` Disconnect from Main Bus (Isolate C2) | `GND BAT C2 UPS` Reconnect to Main Bus |

---

### 5. IGC Channel Subscriptions (Ground & Fleet Bus)

* **Inbound Channels:**
  * `TMS.C2.HANDOVER`: `HANDOVER;MAS-01;GND-01;REGIONAL_ROOT;CONFIRM`
  * `TMS.AIR.PAD`: `mule_id;REQ_LAND;tonnage;eta_sec`
  * `TMS.GROUND.DOCK`: `train_id;REQ_DUMP;tonnage;eta_sec`
  * `TMS.FLEET.STATUS`: `craft_id;type;gps;hold_pct;bat_pct;status`
  * `TMS.LOGISTICS.JIT`: `JIT;MAS-01;tier;eta_sec;ore_type;mass_kg`
* **Outbound Channels:**
  * `TMS.BASE.RECEIVING`: `GND-01;CLEAR_TO_DUMP;TERMINAL_DOCK_1;READY`
  * `TMS.BASE.DEMAND`: `DEMAND;ore_type;priority;deficit_kg;target_stock_kg`
  * `TMS.MULE.CALL`: `CALL;requesting_grid;target_pad;priority;payload_type`
  * `TMS.RIG.LASER`: `req_ingots;stock_summary_json;base_status`

---

## Phase 4-8 · Commissioning & Field Acceptance Checklist (RIG Test)

### Suite A · Foundation & Tarmac Seal Verification
* [ ] **Tarmac Vacuum Seal:** Verify zero voxel penetration or air leakage through the 24 × 28 B heavy armor blast floor; verify sub-surface trenches remain pressurized where sealed.

### Suite B · Mule Gantry Cradle Kinematics & High-Speed Drain
* [ ] **Cradle Proximity Trigger:** Approach pad in MULE-01; confirm `GND PAD Sensor` detects craft at 10 m and swings cradle arms inward 30°; land on pad; confirm magnetic plates lock rub rails and connector locks automatically.
* [ ] **High-Speed Rare Ore Drain:** Feed 500 t Uranium Ore from Mule hold; confirm rare ore sorter drains cargo at $> 80\text{ t/min}$ into West Smelting Bank.

### Suite C · Caterpillar Terminal Dump & Collector Gravity Flow
* [ ] **Reverse Alignment Check:** Reverse CATERPILLAR-01 consist into dock; confirm guide curbs center the wheels and stop bumpers absorb kinetic impact.
* [ ] **2,500 t Bulk Ore Purge:** Lock `GND CAT Dump Conn`; confirm 5× in-deck collectors and bulk sorter evacuate all 3 hopper cars in $< 90\text{ seconds}$ without conveyor back-pressure.

### Suite D · Dual-Tier Refining Smelting Stress Test (200% Yield)
* [ ] **Simultaneous Smelting Run:** Feed 2,000 t Iron Ore and 500 t Gold Ore concurrently; verify dual sorters isolate bulk and rare streams; verify all 4 refineries operate at 200% yield effectiveness without tripping power.

### Suite E · Cooperative 4-Assembler Manufacturing Burst
* [ ] **Pylon Production Burst:** Queue 20,000 Steel Plates and 4,000 Motors; confirm `GND ASM 1` distributes load across Assemblers 2-4 cooperatively; verify 400% productivity output feeds continuous stock to `GND STR Components`.

### Suite F · Power Substation Surge & Shore Power Fast Recharge
* [ ] **Simultaneous Docking & Extrusion:** Engage Extruder welders (8.8 MW), 4 refineries (13.2 MW), 4 assemblers (2.2 MW), and shore charging for both Mule and Cat (18 MW); confirm battery bank delivers 42.2 MW surge load without brownout.

### Suite G · C2 Master Server & Dynamic Handover from Mastodon MKI
* [ ] **Genesis Handover Trial:** Broadcast `TMS.C2.HANDOVER` from Mastodon; confirm `[GND] PB FleetOps` assumes regional C2 authority, initializes pull quotas (`TMS.BASE.DEMAND`), and synchronizes fleet telemetry.

### Suite H · 6-Panel LCD Video Wall Telemetry Stream
* [ ] **Multi-Screen Display Verification:** Deploy Orca, Remora, Mule, Mastodon, and Caterpillar; confirm Video Wall panels 1-6 display live, zero-flicker monospace status layouts with correct subsystem metrics.

### Suite I · Laser Uplink Unbroken Connection to Floating Rig
* [ ] **High-Altitude Beam Lock:** Establish laser connection with Floating Rig Fortress hovering at 15 km altitude; verify uninterrupted bidirectional telemetry during planetary atmospheric turbulence.

### Suite J · Player Circulation, Decontamination Airlocks & Emergency Survivability
* [ ] **Decon Cycle Verification:** Enter C2 bridge from Level L2; confirm `GND Door Decon Bridge` executes 3.0 s UV decontamination washdown before inner door unseals; verify bridge cabin maintains 100% O2 life support.
