# Mastodon · Visual Build Instructions

## Chapter 10 · MASTODON-01 (MKI) Expeditionary Mining Locomotive & Mobile FOB

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MASTODON-01 MKI**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 55, 56, 57, 58, 59, 60 & 61)
* **Role:** Long-range terrestrial expeditionary mining locomotive, self-contained mobile Forward Operating Base (FOB), and **Genesis Seed Fleet Mothership** running **`RIG//OS-Lite`**. In fresh survival campaigns prior to Ground Base refinery commissioning, the lead Mastodon serves as the regional C2 fleet root, commanding subordinate excavators (`MAS-02`), bulk land trains (`CAT-01`), and freight shuttles (`MUL-01`), with automated handover to Ground Base upon commissioning.
* **Architecture:** Segmented articulated train consist:
  1. **Carriage 1 (`MAS-01-T`): Locomotive & Excavator Unit (11 W × 20 L × 5 H B · 6 Wheels):** Heavy tractive engine, 3-operator armored panoramic C2 bridge (Helm, Boom CTC, Tactical Director), Tri-Fuel primary power plant (30 MW nuclear + 5 MW emergency H2 engine + 9 MWh battery reserve across 3× Large Batteries), dual-PB compute rack (`PB Excavator` + `PB RIG//OS-Lite`), 40 km laser comms mast, 4× hydraulic corner outrigger stabilizers with differential auto-leveling jacks, dorsal 45 m long-reach articulated 5-DOF laser mining boom, bow CIWS Gatling Turret (`MAS Turret Bow`), flank anti-personnel turrets, Port L3 maintenance Jefferies tube, and aft 2-axis motorized drawbar hitch.
  2. **Carriage 2 (`MAS-01-U`): Modular FOB & Utility Carriage (11 W × 18 L × 5 H B · 4 Wheels):** Field workshop (1× Basic Refinery for in-situ metallurgy, 1× Large Assembler, Survival Kit / Medical Bay), sub-floor battery vault (9 MWh across 3× Large Batteries), 4× roof flush solar panels (`≈ 640 kW` peak†), component reserve hold, atmospheric bunkering (2× Large H2 tanks, O2/H2 generators), 7 × 7 B MULE-01 Gantry Cradle Pad with motorized clamping arms, 2× KESTREL-01 scout drone landing pads, roof maintenance catwalks, stern CIWS Gatling Turret (`MAS Turret Stern`), and stern heavy dump bumper matching CATERPILLAR-01.
* **Grid:** Large Grid (1 B = 2.5 m) · **Consist Envelope:** 11 W × 42 L × 6 H B (27.5 × 105.0 × 15.0 m).
* **Mass:** Consist Dry Mass `≈ 943.9 t` (Tractor: 524.5 t, Utility: 419.4 t) · Max Laden Mass `≈ 3,444 t` (Earth 1 g).
* **Payload Capacity:** 2,500 t bulk ore across tractor buffer and utility holds.
* **Mechanical & Automation Suite (Decision 57):** 5-point immersive automation: auto-leveling bedrock jacks (sloped terrain compensation up to 20°), retractable crew boarding ladder (Port flank L2), active gravity-horizon tool head leveling with 4.0 m safety proximity stop, underbelly stone tailings aggregate spreader, and smart anti-jackknife drawbar dynamics.
* **Tri-Fuel Power & Closed-Loop Metallurgy Suite (Decision 58):** Self-sustaining field endurance without resupply: (1) 1× Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW) refining raw mined Fe, Ni, Co, Si, Mg on-site into ingots for the Large Assembler; (2) Tri-Fuel Power Architecture: Tier 1 Nuclear (2× Small Reactors, 30 MW), Tier 2 Renewable (4× Roof Flush Solar Panels, `≈ 640 kW` peak†), Tier 3 Emergency Hydrogen (1× Hydrogen Engine, 5 MW) drawing from 10,000,000 L H2 tank farm; (3) Automated resource watchdogs `MAS EC Power` (uranium watchdog auto-starting H2 engine) and `MAS EC Ice` (ice intake routing).
* **Player Circulation & Hybrid Power Suite (Decision 59):** Comprehensive survival engineer ergonomics and versatile power management: (1) Pressurized player circulation network with Port L2 boarding airlock, Port L3 Jefferies tube / crawlway (X −2) bypassing cargo buffer and power vault, enclosed inter-carriage articulated accordion gangway (Z −9 to −8), L3 central workshop corridor, vertical flight ladderwell to Level L6, and flight deck catwalks around Mule/Kestrel/solar decks; (2) Physical maintenance & manual inventory ports exposed on every major storage, reactor, engine, refinery, and sorter block; (3) Upgraded 18.0 MWh Energy Storage Buffer (6× Large Batteries: 3 in Tractor Power Vault, 3 in Utility Sub-Floor, delivering 72.0 MW peak instantaneous discharge); (4) 5 automated operational energy management modes (Silent Watchkeeping, Tractive Cruise, Heavy Excavation, Emergency Failover, and Shore Power Fast Recharge).
* **Terrestrial Concrete Civil Engineering (Decision 60):** Solves excess quarrying aggregate through integration of the Concrete Armor Blocks mod ([303959338](https://steamcommunity.com/sharedfiles/filedetails/?id=303959338)). Gravel from stone refining is assembled into Concrete Components, producing blast-resistant pavers for Caterpillar roadbeds and Ground Base foundations while preserving 100% vanilla rule C2 on aerial fleet craft and the Rig.
* **Perimeter Defense CIWS & Warfare 3 MKII Roadmap (Decision 61):** 360° close-in weapon system (CIWS) and anti-intrusion grid: (1) 2× Large Grid Gatling Turrets (`MAS Turret Bow` at Carriage 1 dorsal crown L5, Z +7, X 0; `MAS Turret Stern` at Carriage 2 stern gantry L5, Z −26, X 0) plumbed into conveyor spine for automated ammo feeds from `MAS Utility Assembler`; (2) 2× Flush Ground-Level Anti-Personnel Interior Turrets (`MAS Turret Flank Port/Stbd` at L2, X ±4, Z +2) covering boarding ladder and perimeter; (3) Automated combat alert integration via `RIG//OS-Lite` (`RIG.OS.MODE CBT`); (4) Pre-Warfare 3 operational baseline with architectural upgrade path to **MASTODON-02 MKII** upon Warfare 3 DLC release.
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

### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

```
               45 m ARTICULATED LASER EXCAVATION BOOM (Dorsal Pedestal)
                     [Tool Head: 2× Simple Laser Multitools]
                                         │
                                         ▼ (Internal Large Hinge / Rotor Pass-Thru)
                       [TRACTOR ORE BUFFER & SUMP] (3×3×3 B Large Cargo Hold)
                       ◄── (Physical Cargo Hatch: Port Jefferies Tube [X −2, Z +2, L3])
                                         │
                                         ▼ (2-Axis Drawbar Articulated Conveyor Pipe)
                       [UTILITY FOB CONVEYOR DISTRIBUTION MANIFOLD]
                                         │
        ┌────────────────────────────────┼────────────────────────────────┐
        ▼ (Upward Riser Trunk)           ▼ (Internal Workshop Loop)       ▼ (Stern Bumper Pipe)
 [MAS MP Sorter] (Drain ON)       [Field Basic Refinery]           [MAS Stern Sorter] (Drain ON)
 (Inspection Hatch: Deck L5)      (330 kW · In-Situ Metallurgy)    (Inspection Hatch: Aft L2)
        │                         (Manual Hopper: Aisle X −2)             │
        ▼                                │ (Ingots)                       ▼
 [MAS MP Conn] (Roof Flush L6)    [Field Large Assembler]          [MAS Stern Dump Conn] (L2 Bumper)
 (650 t rare ore to Mule)         (560 kW · Spare Parts/Repair)    (2,500 t bulk ore to Cat Train)
                                  (Dual Panels: Aisle X −2)
                                         │
                                  [O2/H2 Gen & 2× H2 Tanks]
                                  (10,000,000 L Fueling Line)
                                  (Manual Bunkering Valves: Aisle X +2)
```

#### Physical Maintenance Ports & Inspection Hatches

| Subsystem Component | Grid Location | Physical Access Interface | Maintenance & Survival Function |
|---|---|---|---|
| `MAS Tractor Buffer` | [X −2, Z +2, L3] | Large Cargo Access Hatch | Manual ore inspection, debris clearing, small tool withdrawal |
| `MAS Reactor 1, 2` | [X0, Z −2..−3, L3] | Front Maintenance Hatch | Manual Uranium fuel rod loading if conveyor line is disabled |
| `MAS Engine H2` | [X0, Z −4, L3] | Aft Valve & Sump Port | Manual hydrogen fuel shutoff, cold-start crank, leak inspection |
| `MAS Field Refinery`| [X −2, Z −12, L3] | Top Hopper Access Door | Manual high-grade ore insertion directly into refinery input |
| `MAS Utility Assembler`| [X −2, Z −14, L3] | Dual Inventory Terminals | Manual blueprint queuing, physical component withdrawal |
| `MAS Hold Components`| [X0, Z −22, L3] | Forward Large Port | Primary bulk parts terminal facing central workshop aisle |
| `MAS Sorter Tailings`| [X0, Z +2, L1] | Underbelly Service Flange| Ground-level access between Axles 2-3 to clear stone jams |
| `MAS MP Sorter` | [X0, Z −20, L5] | Riser Inspection Hatch | Flight deck ladder landing access to service rare ore sorter |
| `MAS Stern Sorter` | [X0, Z −26, L2] | Rear Bumper Access Door | Stern ladder descent access to service bulk ore dump sorter |

---

### 0.6 Master Electrical Single-Line Schematic & 5-Mode Energy Management Suite

```
               TRI-FUEL POWER GENERATION & ENERGY STORAGE ARCHITECTURE
   [TIER 1: NUCLEAR BASELOAD]       [TIER 2: RENEWABLE LIFELINE]      [TIER 3: EMERGENCY H2]
   2× Small Nuclear Reactors        4× Roof Flush Solar Panels        1× Hydrogen Engine
   (30.0 MW Continuous)             (≈ 640 kW Peak Daylight†)         (5.0 MW Standby / Failover)
               │                                │                                │
               └───────────────────────┬────────┴────────────────────────────────┘
                                       ▼
                       ENERGY STORAGE BUFFER (18.0 MWh Total · 72.0 MW Peak)
               3× Tractor Batteries (9 MWh) + 3× Utility Batteries (9 MWh)
                                       │
                                       ▼
                              PRIMARY TRACTIVE BUS
        ┌──────────────────────────────┼──────────────────────────────┐
        │                              │                              │
        ▼                              ▼                              ▼
 TRACTOR POWERTRAIN & HYD       ARTICULATED BOOM               UTILITY CARRIAGE UMBILICAL
 6× 5×5 Wheel Drive Motors      Turret Pan Rotor (Azimuth)     Field Basic Refinery (330 kW)
 4× Corner Outrigger Cylinders  Shoulder & Elbow Pitch Hinges  Field Assembler Core (560 kW)
 Steering Actuators (Axles 1-2) Laser Mining Head (4.0 MW)     Mule Float Charger (12 MW)
 Retractable Crew Ladder Hinge  Tool Head Wrist Roll / Pitch   Kestrel Fast Chargers (2× 1 MW)
```

#### 5 Operational Energy Management Modes

| Mode | Active Generation | Battery State | Primary Role & Automated Triggers |
|---|---|---|---|
| **Mode 1: Silent Watchkeeping (Eco)** | Solar Panels (`≈ 640 kW`†) · Nuclear & H2 OFF | `RECHARGE / FLOAT` | Zero-fuel parked mode. Maintains life support, event controllers, radio beacons, and float-charges batteries indefinitely. |
| **Mode 2: Tractive Cruise** | 1× Small Reactor (15 MW) + Solar | `AUTO` (Peak Shaving) | Overland cruising. Reactor powers 10× 5×5 wheel drives (`≈ 8-12 MW`†); batteries absorb acceleration spikes. |
| **Mode 3: Heavy Excavation** | 2× Small Reactors (30 MW) + Solar | `AUTO` (Burst Assist) | Full mining operations. Powers 45 m laser boom (4.0 MW beam + joint torques), outrigger voxel locks, Basic Refinery (330 kW), and Assembler (560 kW). |
| **Mode 4: Emergency Failover** | 1× H2 Engine (5.0 MW) · Nuclear OFF | `DISCHARGE` | Auto-triggered by `MAS EC Power` when uranium mass $< 1.0\text{ kg}$. Sheds refinery/assembler loads; preserves mobility and C2 life support. |
| **Mode 5: Shore Power / Fast Charge** | External Depot Grid via Roof or Stern | `RECHARGE` (100% Rate) | Rapid depot battery recharging and H2 bunkering when docked to Ground Base or Caterpillar train. |

---

### 0.7 Master Consist Bill of Materials (BOM) & Mass Budget

| System Subassembly | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Tractor Chassis & Hull** | Heavy armor full blocks, half-blocks, slopes (Palette 0) | 440 | 374.0 t | 440 |
| **Tractor Powertrain** | 6× 5×5 Heavy Suspension Wheels (Axles 1-3) | 6 | 48.0 t | 600 |
| **Corner Outriggers** | 4× Large Pistons + 4× Large Magnetic Plates (Palette 1) | 8 | 18.4 t | 260 |
| **Tractor Power Core** | 2× Small Nuclear Reactors, 1× Hydrogen Engine, 3× Large Batteries (Palette 6) | 6 | 31.6 t | 730 |
| **Articulated Boom** | 2× Adv Rotors, 3× Motor Hinges, 2× Lasers, Girders (Palette 3) | 18 | 34.5 t | 920 |
| **Armored Bridge & C2** | 1× Cockpit, 2× Stations, 2× PBs, Laser Antenna, Glass, Detector | 18 | 15.8 t | 410 |
| **Drawbar Hitch Assembly** | 1× Adv Rotor, 1× Motor Hinge, Heavy Armor Joint Blocks | 6 | 12.0 t | 225 |
| **Utility Carriage Hull** | Heavy armor frames, wheel wells, deck armor (Palette 0) | 360 | 306.0 t | 360 |
| **Utility Powertrain** | 4× 5×5 Heavy Suspension Wheels (Axles 4-5) | 4 | 32.0 t | 400 |
| **Utility Sub-Floor Battery Vault** | 3× Large Batteries (9.0 MWh, Palette 6) | 3 | 13.5 t | 120 |
| **Interior Walkways & Hatches** | Grated catwalks, sliding doors, ladder runs, interior lights (Palette 9) | 25 | 12.5 t | 140 |
| **Field Workshop & Hab** | 1× Basic Refinery, 1× Large Assembler, 1× Survival Kit, 2× O2/H2 Tanks, 2× Large H2 Tanks | 9 | 52.0 t | 630 |
| **Utility Cargo Hold** | 1× Large Cargo Container (Components), Conveyor network | 16 | 28.0 t | 160 |
| **Flight Decks & Solar** | 1× Mule Gantry Cradle Pad (7×7 B), 2× Kestrel Pads, Stern Dump, 4× Solar Panels | 62 | 56.0 t | 580 |
| **Perimeter Defense Suite** | 2× Gatling Turrets, 2× Interior Turrets, Ammunition Conveyor Feeds (Palette 7) | 12 | 9.6 t | 308 |
| **TOTAL CONSIST (DRY)** | Complete Expeditionary Mining Locomotive & Mobile FOB | **993** | **≈ 943.9 t** | **6,283** |

---

### 0.8 Master Player Circulation, Access Corridors & Jefferies Tubes Schematic

```
[LONGITUDINAL PROFILE · CREW CIRCULATION & MAINTENANCE PATHS (LOOKING STARBOARD)]

     ◄────────── CARRIAGE 1: TRACTOR / EXCAVATOR ──────────►     ◄────────── CARRIAGE 2: FOB / UTILITY ──────────►
     L6                                                         ┌──[KESTREL]──────[MULE PAD CATWALK]──────┐
     L5      ┌──[C2 BRIDGE]──────[BOOM PEDESTAL DECK]           │ (L6 Catwalks & Solar Array Access)      │
     L4      │   (3-Seat C2)     │ (Turret Service Hatch)       ├─────────────────────────────────────────┤
     L3 ┌────┴───────────────────┴─────────────────────────┐    │ [FIELD WORKSHOP & MEDICAL HABITAT]      │
        │ [PORT JEFFERIES TUBE] ──► [POWER CORE VAULT]      ╞════╡ (Refinery · Assembler · MedBay · Tanks) │
     L2 │ (Access to Buffer & Core) (Reactors / H2 Engine)  │    │ [SUB-FLOOR BATTERY VAULT (3x BAT)]      │
     L1 └─▲────────────────────────────────────────────────┘    └───────────────────────────────────────▲─┘
          │                                                       (Articulated Enclosed Gangway)         │
     [CREW BOARDING LADDER]                                            (Z -9 to -8, L3 Level)      [STERN DUMP LADDER]
     (Port Flank L2 Hinge)                                                                         (Aft Bumper L2-L3)
```

```
Plan view, Y = L3 (Tractor & Utility Pressurized Walkways & Jefferies Tubes)
        X: −3           X: −2            X0              X: +2           X: +3
    Z: +12 ┌───────────────┬───────────────────────────────┬───────────────┐
           │ [C2 BRIDGE]   │ [STATION 1]   [STATION 2]     │ [STATION 3]   │  ◄── Forward Bridge Deck
    Z:  +8 ├───────────────┴───────────────┬───────────────┴───────────────┤
           │ [AIRLOCK DOOR]                │ [BOOM PEDESTAL LADDER UP]     │  ◄── Airlock Vestibule
    Z:  +6 ├───────────────┬───────────────┴───────────────┬───────────────┤
           │ [PORT JEFF-   │                               │ [SOLID ARMOR] │
    Z:  +3 │  ERIES TUBE]  │ [MAS TRACTOR BUFFER (3×3×3)]  │               │  ◄── Ore Buffer Pass
           │ (Catwalk &    │ (Manual Port on Port Flank)   │               │
    Z:  +1 │  Lighting)    │                               │               │
           ├───────────────┼───────────────┬───────────────┼───────────────┤
    Z:  −2 │ [VAULT DOOR]  │ [BAT 1]       │ [REACTOR 1]   │ [BAT 2]       │
    Z:  −3 │               │ [BAT 3]       │ [REACTOR 2]   │ [ARMOR]       │  ◄── Power Core Vault
    Z:  −4 │               │ [ARMOR]       │ [H2 ENGINE]   │ [ARMOR]       │
    Z:  −7 ├───────────────┴───────────────┴───────────────┴───────────────┤
    Z:  −8 │ ══════════════ [GANGWAY FORWARD SLIDING DOOR] ═══════════════ │  ◄── Aft Bulkhead
           ╞═══════════════════════════════════════════════════════════════╡
           │  [INTER-CARRIAGE ARTICULATED ENCLOSED GANGWAY (Z −9 to −8)]   │  ◄── Flexible Walkway
           ╞═══════════════════════════════════════════════════════════════╡
    Z:  −9 │ ══════════════ [GANGWAY AFT SLIDING DOOR] ═══════════════════ │  ◄── Utility Fwd Bulkhead
           ├───────────────┬───────────────┬───────────────┬───────────────┤
    Z: −10 │ [SOLID ARMOR] │               │ [FLIGHT LADDER UP]            │  ◄── Stair/Ladder to L6
           ├───────────────┤ [CENTRAL      ├───────────────┬───────────────┤
    Z: −12 │ [REFINERY]    │  WORKSHOP     │ [MEDICAL BAY] │ [CRYO POD]    │  ◄── Field Metallurgy
    Z: −14 │ [ASSEMBLER]   │  AISLE]       │ [O2/H2 GEN 1] │ [O2/H2 GEN 2] │  ◄── Fabrication & O2
    Z: −18 │ [H2 TANK 1]   │ (X0 Walkway)  │ [H2 TANK 2]   │ [ARMOR]       │  ◄── Fuel Bunkering
    Z: −22 │ [ARMOR]       │ [MAS HOLD COMPONENTS (3×3×3)] │ [ARMOR]       │  ◄── Parts Hold
    Z: −25 ├───────────────┴───────────────┬───────────────┴───────────────┤
    Z: −26 │                               │ [STERN LADDER DOWN TO L2]     │  ◄── Stern Dump Access
           └───────────────────────────────┴───────────────────────────────┘
```

---

### 0.9 MASTODON-02 (MKII) Warfare 3 DLC Upgrade Architecture Roadmap

The current **MASTODON-01 MKI** design establishes a combat-capable survival baseline using vanilla and DLC blocks ahead of the upcoming **Warfare 3 DLC** release. Upon DLC launch, the consist will transition to the **MASTODON-02 MKII** blueprint, engineered with the following pre-allocated physical and power provisions:

| Subsystem Domain | MKI Pre-Warfare 3 Baseline | Planned MKII Warfare 3 Evolution | Pre-Allocated Sockets & Headroom |
|---|---|---|---|
| **Primary Dorsal Armament** | 2× Large Gatling Turrets (`MAS Turret Bow / Stern`) | Warfare 3 Heavy Kinetic / Directed-Energy Turrets | Centerline L5 dorsal hardpoints plumbed to large conveyor junctions. |
| **Close-In Anti-Personnel** | 2× Interior Turrets (`MAS Turret Flank Port/Stbd`) | Warfare 3 Point-Defense / Anti-Missile Systems | Flank L2 skirt sockets with 360° low-depression coverage. |
| **Electronic Warfare (EW)** | Passive laser antenna + 50 km radio beacon | Warfare 3 Active ECM / Jamming / Radar Intercept | Dorsal L6 comms mast socket pre-wired for EW transceivers. |
| **Power Grid Headroom** | 35 MW generation + 18 MWh battery reserve | Absorbs 40+ MW burst draws from heavy weapon systems | 72.0 MW peak instantaneous battery discharge (Mode 3/4). |
| **Armor Fairing Modularity** | Heavy armor composite hull with sloped glacis | Warfare 3 Modular Composite / Reactive Armor Plating | Wheel arch skirts and hitch drawbar pre-spaced for add-on armor tiles. |

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

---

### Step 10.2B · Tailings Aggregate & Roadbed Spreader Underbelly Manifold (L1)

To prevent cargo holds from clogging with thousands of tons of low-value crushed stone, install a selective tailings venting manifold beneath the chassis along centerline X0 at Z = 0:
* **Conveyor Tap:** 1× Conveyor Sorter (`MAS Sorter Tailings`) tapped into the central conveyor trunk at [X0, Z0, Y = L1] facing downward.
* **Discharge Nozzles:** 2× Conveyor Ejectors (`MAS Ejector Tailings 1, 2`) angled 45° aft (−Z) beneath Axle 3.
* **Sorter Filter:** Whitelist set to `Stone` and `Gravel` · `Drain All = ON`.
* **Automation:** Managed by Event Controller `MAS EC Tailings`. When Tractor ore buffer fill exceeds 50%, ejectors activate, laying down a smooth gravel roadbed behind the driving wheels while retaining high-value ores (Fe, Ni, Co, U).

```
Underbelly Profile · Tailings Aggregate Spreader (Z0, L1)
     Central Conveyor Raceway (L1)
                  │
                  ▼
       [MAS Sorter Tailings] (Filter: Stone / Gravel)
                  │
                  ▼
       [MAS Ejector Tailings 1, 2] ──► Angled Aft (−Z)
                  │
                  ▼ (Vents Stone Tailings Behind Axle 3)
     ══════════════════════════════════════════════════ Crushed Stone Roadbed
```

#### Pre-Enclosure Inspection Gate 10.1A: Tractor Powertrain & Belly Clearance Check
* [ ] Verify uninterrupted large conveyor pipe flow from Bow Throat (Z +12) to Aft Hitch (Z −8).
* [ ] Verify `MAS Sorter Tailings` drains stone and gravel without leaking metal ores.
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
* 1× Hydrogen Engine (5 MW, 1×1×2 B, Palette 6: High-Voltage Red)
* 3× Large Battery (3 MWh each, 9.0 MWh total, Palette 6)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 1× Large Advanced Rotor (Palette 2: Heavy Rail Crimson)
* 1× Large Motor Hinge (Palette 2)
* 12× Grated Catwalk Block & 2× Sliding Door (Palette 9: High-Viz Yellow)
* 18× Large Conveyor Pipe & 6× Curved Conveyor Tube (Palette 4: Processing Teal)
* 120× Heavy Armor Full Block & Slopes (Palette 0)

---

### Step 10.3 · Primary Power Core Vault & Electrical Shielding (Z −4 to −2, L2-L4)

Install 2 Small Nuclear Reactors, 1 Hydrogen Engine, and 3 Large Batteries behind the ore buffer hold. Enclose the plant in double-walled heavy armor bulkheads (Palette 6):

```
Plan view, Y = L3 (Tractor Power Core Vault)
        X: −2     0    +2
    Z: −2  ┌───────────────┐
           │[BAT 1][R 1][BAT 2]│  ◄── Reactor 1 (15 MW) flanked by Large Batteries
    Z: −3  │[BAT 3][R 2][Armor]│  ◄── Reactor 2 (15 MW) flanked by Battery 3
    Z: −4  │[Armor][H2E][Armor]│  ◄── Hydrogen Engine (5 MW standby failover)
           └───────────────┘
           Nuclear + H2 Baseload Total: 35.0 MW · Tractor Battery Reserve: 9.0 MWh (36 MW Peak)
```

1. Place Reactor 1 (`MAS Reactor 1`) at [X0, Y = L3, Z −2] and Reactor 2 (`MAS Reactor 2`) at [X0, Y = L3, Z −3].
2. Flank the reactors with Large Batteries (`MAS Battery 1, 2, 3`) at [X −2, Z −2], [X +2, Z −2], and [X −2, Z −3].
3. Connect reactor conveyor ports directly to the central conveyor spine along X0 for automated uranium fuel feeding.
4. Encase the compartment in Heavy Armor blocks painted Palette Slot 6 (High-Voltage Alert Red).
5. Install armored sliding door `MAS Door Vault` at [X −2, Z −2, L3] accessing the Port Jefferies tube.

---

### Step 10.3B · Standby Emergency Hydrogen Engine Installation (Z = −4, X0, L3)

Install 1 Large Grid Hydrogen Engine (`MAS Engine H2`, 1×1×2 B, 5.0 MW) directly aft of Reactor 2 on centerline X0. Connect its conveyor port to the central spine to draw fuel from Carriage 2's 10,000,000 L hydrogen reserve hold:
* **Function:** Standby cold-start generator providing instantaneous 5.0 MW electrical failover upon uranium fuel depletion, reactor maintenance, or peak tractive climbs.
* **Automation:** Supervised by Event Controller `MAS EC Power` (configured to auto-ignite `MAS Engine H2` when nuclear reactor fuel mass drops below 1.0 kg or battery state of charge drops below 20%).
* **Manual Port:** Features an exposed aft sump inspection port and fuel shutoff valve accessible from the vault interior.

---

### Step 10.3C · Port Maintenance Jefferies Tube & Physical Buffer Access Hatch (Z +6 to −7, X −2, L3)

Construct a continuous, pressurized 1-block wide maintenance crawlway running along the Port side of Carriage 1 at Level L3:
* **Circulation Route:** Runs from the forward airlock vestibule at Z +6 aft to the gangway door at Z −7, bypassing the Large Cargo buffer and Power Vault.
* **Catwalk Decking:** Paved with Grated Catwalk blocks painted Palette Slot 9 (High-Viz Safety Yellow) with recessed amber bulkhead lighting.
* **Physical Buffer Hatch:** Mount an exposed cargo access port on the Port face of `MAS Tractor Buffer` at [X −2, Z +2, L3]. Allows an engineer to manually inspect ore contents, clear conveyor clogs, or withdraw small hand tools.
* **Power Vault Access:** Sliding door `MAS Door Vault` at [X −2, Z −2, L3] provides direct walk-in access to the reactor and battery bank.

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

---

### Step 10.5B · Smart Anti-Jackknife Dynamics & High-Speed Sway Lockout

To eliminate trailer sway during high-speed transit and prevent destructive jackknifing when reversing down steep slopes:
* **High-Speed Cruising Mode (> 35 km/h):** Event Controller `MAS EC Hitch Speed` monitors vehicle speedometer. When velocity exceeds 35 km/h, `MAS Hitch Rotor` braking torque increases dynamically to `100 MN·m`, locking the drawbar straight and eliminating harmonic trailer snaking.
* **Reversing Jackknife Prevention:** When transmission shifts into reverse, Event Controller `MAS EC Hitch Rev` clamps rotor yaw limits from `±45.0°` down to `±15.0°` and commands Axle 5 to track in positive alignment, physically preventing the utility carriage from folding sideways.

---

### Step 10.5C · Inter-Carriage Articulated Enclosed Accordion Gangway (Z −9 to −8, L3)

Directly above the mechanical drawbar hitch, construct a heavy-duty flexible pressurized accordion gangway spanning the 1-block mechanical gap between Tractor Aft Bulkhead (Z −8) and Utility Forward Bulkhead (Z −9):
* **Structure:** Heavy-duty reinforced canvas and rubber bellows backed by sliding armor plates, accommodating up to ±45° yaw and ±25° pitch articulation without breaking airtight seal.
* **Isolation Doors:** Sliding door `MAS Door Gangway Fwd` at [X0, Z −8, L3] and sliding door `MAS Door Gangway Aft` at [X0, Z −9, L3]. Allows either carriage to maintain full atmosphere even if the other carriage is breached.
* **Walkway Deck:** Paved with slip-resistant flexible grated catwalks, allowing crew members to safely walk between carriages at full travel speed without exposure to drawbar pinch hazards or external dust storms.

#### Pre-Enclosure Inspection Gate 10.2A: Power Core Isolation, Jefferies Tube & Gangway Test
* [ ] Verify reactor automated uranium pull through the central conveyor trunk.
* [ ] Confirm `MAS Engine H2` cold-starts and delivers 5.0 MW upon manual toggle or simulated uranium depletion.
* [ ] Confirm total tractor power capacity reads `71.0 MW` peak (30 MW nuclear + 5 MW H2 engine + 36 MW battery discharge from 3× Large Batteries).
* [ ] Verify Port Jefferies tube allows unobstructed human walking from Bridge to Aft Bulkhead.
* [ ] Confirm physical maintenance port on `MAS Tractor Buffer` opens and allows manual inventory access.
* [ ] Confirm articulated inter-carriage gangway maintains airtight seal and flexes cleanly with hitch movement.
* [ ] Test 2-axis drawbar hitch: ensure rotor and hinge pivot freely without clashing with the chassis frame.
* [ ] Verify `MAS EC Hitch Speed` dynamically raises rotor braking torque to 100 MN·m when speed exceeds 35 km/h.

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

### Step 10.6B · Auto-Leveling Bedrock Jacks (Sloped Terrain Pitch & Roll Compensation)

When parking on sloped hillsides (up to 20° grade), the 4 corner outrigger pistons execute an automated differential extension cycle to bring the vehicle deck to true horizontal:
* **Level Sensing:** Event Controller `MAS EC Level` (or `[MAS] PB Excavator`) monitors the artificial horizon pitch and roll angles from `MAS Helm`.
* **Differential Stroke:** Downhill pistons extend further (up to 2.5 m stroke) while uphill pistons extend only until initial voxel contact, bringing the deck to $0.0^\circ \text{ Roll}$ and $0.0^\circ \text{ Pitch}$.
* **Lock State:** Once leveled, all 4 Magnetic Plates lock into bedrock simultaneously, creating a perfectly level, rigid foundation on hillsides.

```
TERRAIN-TILT AUTO-LEVELING (Looking Bow, Y = L1-L2)

 [UNLEVELED ON 15° SLOPE]                         [AUTO-LEVELED DECK (0.0° ROLL)]
      Chassis Tilted 15° Downhill                     Chassis Leveled to Gravity Horizon
         ┌───────────────────┐                           ┌───────────────────┐
        ╱                   ╱                           │     MAIN DECK     │ (0.0° Roll)
       ╱    TILTED CAB     ╱                            └─┬───────────────┬─┘
      └──────┬───────┬────┘                               │ [Piston 0.8m] │ [Piston 2.2m]
     [Piston]│       │[Piston]                           ▼               ▼
      (Short)▼       ▼(Short)                       [MAG LOCK]      [MAG LOCK]
      ═════════════════════════ 15° Hillside        ═════════════════════════ 15° Hillside
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

### Step 10.8B · Active Gravity-Horizon Tool Head Leveling & Safety Proximity Buffer

To prevent the laser mining head from gouging angled V-grooves during elevation changes:
* **Horizon Auto-Leveling:** As the Shoulder Hinge elevates from −20° to +75°, the Wrist Pitch Hinge automatically counter-pitches in exact inverse ratio. This keeps the laser beam strictly parallel to the planet's gravity horizon, cutting perfectly flat terrace floors.
* **Proximity Safety Buffer:** Sensor `MAS Sensor Boom` mounted on the tool head is configured with a 4.0 m forward detection sphere. If terrain voxels or vehicle blocks enter the buffer during rapid slewing, it cuts laser beam power and locks joint brakes within 1 tick, preventing tool head destruction.

```
BOOM ELEVATION WITH HORIZON-LEVELED TOOL HEAD

                  [SHOULDER ELEVATED +35°]
                         ╱
                        ╱  Main Arm
                       ╱
                      ▼
               [ELBOW FLEXED -20°]
                      ╲
                       ╲  Forearm
                        ╲
                         ▼
                  [WRIST PITCH HINGE] ── (Counter-Pitches -15° to cancel arm slope)
                         │
                         ▼
               ┌───────────────────┐
               │  [LASER MULTI]    │ ════════════► Level Horizontal Beam Cut
               └───────────────────┘               (Parallel to Planet Floor)
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
* 1× Large Gatling Turret (`MAS Turret Bow`, Palette 7: Fleet Command Navy)
* 2× Large Interior Turret (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`, Palette 7)

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

---

### Step 10.10B · Retractable Crew Boarding Ladder & Proximity Walkway (Port Flank L2)

To allow the player to enter and exit the 3.5 m high bridge cabin without consuming limited jetpack hydrogen fuel:
* **Hinge Mounting:** Install 1× Motor Hinge (`MAS Crew Ladder Hinge`) flush on the Port hull skirt at [X −4, Z +7, Y = L2] (between Axles 1 and 2).
* **Ladder Run:** Attach a 2-block narrow ladder run to the hinge head, painted Palette Slot 9 (High-Viz Safety Yellow).
* **Proximity Automation:** Sensor `MAS Sensor Ladder` set to 2.5 m radius at the Port flank. When a crew member on foot approaches, the hinge rotates downward 90° to touch ground level (L0).
* **Drive Interlock:** Event Controller `MAS EC Ladder` monitors the handbrake and wheel propulsion. When the handbrake is released or speed exceeds 0 km/h, the ladder automatically retracts flush into the armored hull recess.

```
CREW BOARDING LADDER (Port Flank Profile)

     [TRANSIT: FLUSH STOWED]                   [BOARDING: DEPLOYED TO GROUND]
           Deck L3                                   Deck L3
         ┌─────────┐                               ┌─────────┐
         │ CAB WALL│                               │ CAB WALL│
     L2  ├─────────┴─ [HINGE 0°]               L2  ├─────────┴─ [HINGE 90° ↓]
         │ WHEEL WELL │ [LADDER FOLDED]            │ WHEEL WELL │
     L1  │            │                            │            │   │ [LADDER RUN]
     ════╧════════════╧══════════ Ground       L0  ═════════════╧═══▼ (Touchdown)
```

---

### Step 10.10C · Bow CIWS Gatling Turret & Flank Anti-Personnel Sockets (Tractor Crown L5)

To protect the locomotive consist against hostile MES pirate drones and ground boarding raiders while anchored or mining:
* **Bow Gatling Turret (`MAS Turret Bow`):** Mount 1× Large Grid Gatling Turret (3×3×3 B footprint) on the centerline dorsal crown of Carriage 1 at [X0, Z +7, Y = L5], directly above the C2 compute rack rear bulkhead. Connect its bottom port directly to the tractor conveyor spine at L4. Provides full 360° hemispherical CIWS protection against aerial drones and ground gunships.
* **Flank Anti-Personnel Turrets (`MAS Turret Flank Port/Stbd`):** Mount 2× Interior Turrets (1×1×1 B) flush on the lower armored skirts at [X −4, Z +2, Y = L2] (Port) and [X +4, Z +2, Y = L2] (Starboard). Plumbed to the lower conveyor spine to deliver automated anti-infantry and anti-wildlife perimeter fire covering the boarding ladder and ground radius.
* **Combat Mode Integration:** Event Controller `MAS EC Combat` monitors chassis damage and enemy target acquisition. When engaged, `RIG//OS-Lite` elevates alert status to `RIG.OS.MODE CBT`, powers up all turrets, seals crew airlocks, and feeds ammunition from the assembler hold.

#### Pre-Enclosure Inspection Gate 10.4A: Bridge Airtightness & C2 Network Link Check
* [ ] Confirm bridge cabin pressurizes to 100% O2 via air vent.
* [ ] Verify `MAS Ore Detector` displays subterranean ore deposits out to 150 m range.
* [ ] Confirm CTC block maps mouse yaw to `MAS Turret Azimuth` and mouse pitch to `MAS Shoulder Pitch`.
* [ ] Confirm `MAS Comm Laser` and `MAS Comm Radio` establish two-way IGC telemetry ping.
* [ ] Verify `MAS Crew Ladder Hinge` automatically lowers when approached on foot and retracts upon handbrake release.
* [ ] Verify `MAS Turret Bow` and `MAS Turret Flank Port/Stbd` receive NATO 25×184mm ammunition through conveyor spine.

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
* 3× Large Battery (3 MWh each, 9.0 MWh total, Palette 6: High-Voltage Red)
* 1× Basic Refinery (1×1×2 B, 330 kW, Palette 4: Processing Teal)
* 1× Large Assembler (Palette 4: Processing Teal)
* 1× Medical Bay + 1× Cryo Chamber (Palette 7)
* 2× Large Hydrogen Tank (Palette 6)
* 2× O2/H2 Generator (Palette 4)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 14× Grated Catwalk Block, 4× Ladder Block, 2× Sliding Door (Palette 9)
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

### Step 10.11B · Sub-Floor Utility Battery Vault (Z −17 to −15, L2)

Install 3 Large Batteries (`MAS Battery 4, 5, 6`) in the Level L2 sub-floor cavity beneath the Field Workshop:
* **Storage Capacity:** Adds 9.0 MWh energy storage buffer (36.0 MW peak discharge) to Carriage 2, bringing total consist energy reserve to 18.0 MWh (72.0 MW peak discharge).
* **Electrical Integration:** Plumbed directly into the Utility carriage tractive bus, supplying float-charge buffer for the Mule Gantry Pad, Basic Refinery, Large Assembler, and rear steering axle.

---

### Step 10.12 · Mobile FOB Field Workshop & Medical Suite (L2-L4)

Outfit Carriage 2 as a self-sustaining expeditionary base centered along the Level L3 central aisle:
1. **Field Workshop Assembler (`MAS Utility Assembler`):** Large Assembler at [X −2, Z −14] configured to fabricate replacement plates, drill components, and motors directly in the field. Dual physical inventory panels face the central aisle for manual blueprint access.
2. **Medical & Survival Room (`MAS Medical Bay`):** Pressurized crew habitat at [X +2, Z −12 to −14] with Survival Kit, Cryo Chamber, and Medical Room for expedition respawn and health replenishment.
3. **Atmospheric Tank Farm:** 2× Large Hydrogen Tanks (refuels Kestrel scouts and Mule ferries) + 2× O2/H2 Generators at [X ±2, Z −18]. Manual shutoff valves exposed in the corridor wall.
4. **Field Component Storage (`MAS Hold Components`):** 1× Large Cargo Container (3×3×3 B) dedicated to stocking construction parts at Z −22. Features a physical terminal port facing the aisle at [X0, Z −21, L3].

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

---

### Step 10.12B · Closed-Loop In-Situ Field Metallurgy (Basic Refinery Integration)

In Space Engineers, an Assembler cannot convert raw ore directly into components; it requires refined ingots. Install 1 Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW, 13.5 t) adjacent to `MAS Utility Assembler` at [X −2, Y = L3, Z −12]:
* **Conveyor Routing:** Directly plumbed to the central conveyor spine. Ingests raw ore (Fe, Ni, Co, Si, Mg) delivered from `MAS Tractor Buffer` via the articulated drawbar pipe.
* **Closed Loop:** Outputs refined ingots directly into `MAS Utility Assembler` and `MAS Hold Components`, enabling complete autonomous field fabrication of replacement parts (steel plates, construction components, motors, bulletproof glass) without returning to base.
* **Manual Hopper Port:** Top maintenance door accessible from the central aisle allows manual dumping of rare ore batches.
* **Ice Automation:** Event Controller `MAS EC Ice` monitors raw ice levels, automatically prioritizing ice routing to the twin O2/H2 generators to replenish the 10,000,000 L hydrogen reserve.

```
Plan view, Y = L3 (Utility Carriage Interior Deck with Closed-Loop Metallurgy)
        X: −3          0          +3
    Z: −10 ═══════[DRAWBAR TONGUE]═══════  ◄── Articulated Hitch from Tractor
           │                             │
    Z: −12 │ [REFINERY]  │               │  ◄── Basic Refinery (330 kW In-Situ Metallurgy)
           ├─────────────┤ [MEDICAL BAY] │
    Z: −14 │ [ASSEMBLER] │               │  ◄── Large Assembler & Crew Medical Core
           ├─────────────┼───────────────┤
    Z: −18 │ [H2 TANK 1] │ [H2 TANK 2]   │  ◄── Fuel Bunkering Farm (10,000,000 L H2)
           ├─────────────┴───────────────┤
    Z: −22 │ [MAS HOLD COMPONENTS (3×3×3)│  ◄── Spare Parts Storage (Palette 5)
    Z: −26 └─────────────────────────────┘
```

---

### Step 10.12C · Vertical Flight Ladderwell & Stern Maintenance Descent (L3 to L6 & L3 to L2)

To allow physical passage from the workshop to the roof flight decks and stern train dump bumper:
* **Flight Deck Ladderwell (L3 → L6):** Install an enclosed vertical ladder at [X +2, Z −10] climbing from the forward workshop corner up through an armored hatch to the Level L6 Flight Deck.
* **Stern Dump Descent (L3 → L2):** Install a descending ladderwell at [X +2, Z −25] passing through an armored hatch down to Level L2. Allows an engineer on foot to physically inspect and service `MAS Stern Dump Conn`, `MAS Stern Sorter`, and rear guide wedges without leaving vehicle cover.

#### Pre-Enclosure Inspection Gate 10.5A: Field Workshop Fabrication, Sub-Floor Batteries & Circulation Check
* [ ] Confirm Utility sub-floor battery bank (`MAS Battery 4, 5, 6`) delivers 9.0 MWh storage and 36.0 MW discharge.
* [ ] Verify `MAS Field Refinery` ingests raw mined ore and deposits refined ingots into `MAS Utility Assembler`.
* [ ] Verify `MAS Utility Assembler` pulls ingots and queues components without manual transfer.
* [ ] Confirm physical cargo access ports on Basic Refinery, Assembler, and Component Hold allow manual inventory access.
* [ ] Verify vertical flight ladderwell provides unobstructed ascent to Level L6 Flight Deck.
* [ ] Confirm stern ladder provides safe descent to Level L2 dump bumper.
* [ ] Confirm crew medical bay allows successful respawn and O2/H2 replenishment.
* [ ] Confirm O2/H2 tanks maintain airtight seals and supply hydrogen lines.
* [ ] Verify `MAS EC Ice` automatically feeds ice to O2/H2 generators when H2 tanks drop below 90%.

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
* 4× Solar Panel (Palette 0: Industrial Dark Gray · Flush Roof Array)
* 18× Catwalk with Railing Block (Palette 9: High-Viz Safety Yellow)
* 2× Motor Hinge + 2× Large Magnetic Plate (Palette 1 · Cradle Clamp Arms)
* 1× Large Connector (`MAS MP Conn`, Palette 4)
* 2× Small-Grid Connector (`MAS Kestrel Dock 1, 2`, Palette 8: Scout White)
* 1× Large Connector (`MAS Stern Dump Conn`, Palette 2: Rail Crimson)
* 2× Large Conveyor Sorter (`MAS MP Sorter`, `MAS Stern Sorter`, Palette 4)
* 12× Heavy Armor Slopes & Alignment Bumpers (Palette 2)
* 1× Large Gatling Turret (`MAS Turret Stern`, Palette 7: Fleet Command Navy)

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

### Step 10.13B · Flush Solar Array for Autonomous Watchkeeping (Carriage 2 Roof L6)

Mount 4 Large Grid Solar Panels (`MAS Solar 1..4`, 1×4 B) flush with the Heavy Armor roof deck at Level L6, flanking the Mule Gantry Cradle Pad:
* **Positioning:** 2 Port panels at X = −4 (Z −24 to −20) and 2 Starboard panels at X = +4 (Z −24 to −20), keeping a 1-block clear buffer from the 7×7 B Mule landing footprint to avoid thruster plume erosion.
* **Output & Role:** Delivers `≈ 640 kW` peak daylight power† (160 kW per panel† under optimal solar alignment). Provides continuous zero-fuel watchkeeping power, maintaining life support, event controllers, communications, and float-charging the 18.0 MWh battery reserve without consuming nuclear fuel or hydrogen.

```
Plan view, Y = L6 (Mule Blast Deck Flanked by Flush Solar Array)
        X: −4     X: −3           0          +3     X: +4
    Z: −19 ┌────┐ ■════════════╤═══════════■ ┌────┐
           │SOL1│ ║  Hazard    │  Hazard   ║ │SOL3│  ◄── Flush Solar Array (≈ 640 kW Peak†)
    Z: −22 │    │ [CLAMP P] [MAS MP CONN] [CLAMP S]  │    │  ◄── Mule Clamping Rails
           │SOL2│ ║  Chevrons  │  Chevrons ║ │SOL4│
    Z: −25 └────┘ ■════════════╧═══════════■ └────┘
```

---

### Step 10.13C · Level L6 Flight Deck Maintenance Catwalk Network

To ensure safe human traversal and field maintenance across the upper flight surfaces:
* **Perimeter Safety Catwalk:** Surround the 7×7 B Mule Gantry Pad with industrial grated catwalks fitted with yellow safety handrails (Palette Slot 9). Provides an unhindered observation walkway for deck marshals directing landing MULE-01 shuttles.
* **Scout Service Bridges:** Extend forward catwalk branches to twin Kestrel landing pads (Port/Starboard), allowing flight technicians to physically inspect scout drone hulls, retrieve manual geological core samples, and verify connector seals.
* **Solar Inspection Gangways:** Run narrow service catwalks along Port (X −4) and Starboard (X +4) flanks, giving crew members direct walk-up access to inspect, wash dust from, or repair `MAS Solar 1..4` without treading upon the Mule heavy blast deck.

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

---

### Step 10.15B · Stern Gantry CIWS Gatling Turret (Utility Stern L5)

Mount the rear defense turret on the upper gantry overlooking the stern dump interface:
* **Stern Gatling Turret (`MAS Turret Stern`):** 1× Large Grid Gatling Turret mounted at [X0, Z −26, Y = L5], elevated 7.5 m above the Caterpillar dump bumper. Connect bottom conveyor port to the stern conveyor junction at L4.
* **Coverage:** Delivers 270° hemispherical defensive coverage over the rear quadrant, providing covering fire for incoming MULE-01 shuttles on approach to the gantry pad, protecting docked Caterpillar land trains during high-speed dump operations, and repelling pursuers from the rear.

#### Pre-Enclosure Inspection Gate 10.6A: Cradle Clamping Dynamics & Dual-Tier Sorter Routing
* [ ] Verify `MAS MP Sorter` whitelist passes only rare ores (U, Pt, Au, Mg) to the Mule shuttle.
* [ ] Verify `MAS Stern Sorter` whitelist passes bulk ores (Fe, Ni, Si, Stone) to the land train.
* [ ] Test cradle clamp hinges: confirm 30° inward swing firmly holds docking craft without grid damage.
* [ ] Confirm `MAS Solar 1..4` deliver daylight power and maintain battery float charge while nuclear reactors are toggled OFF.
* [ ] Verify Level L6 catwalk handrails and safety walkways allow complete perimeter access around Mule and Kestrel decks.
* [ ] Verify `MAS Turret Stern` receives NATO 25×184mm ammunition from utility cargo hold.

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
* `MAS Batteries`: 6× Large Batteries across Tractor Vault and Utility Sub-Floor (18.0 MWh total).
* `MAS Doors`: All internal airlock, vault, and inter-carriage gangway sliding doors.
* `MAS Turrets`: 2× Large Gatling Turrets (`MAS Turret Bow`, `MAS Turret Stern`) + 2× Interior Turrets (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`).
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
| Gatling Turret | `MAS Turret Bow` | Carriage 1 dorsal crown CIWS · 360° hemispherical anti-air/ground |
| Gatling Turret | `MAS Turret Stern` | Carriage 2 stern gantry CIWS · 270° rear hemispherical coverage |
| Interior Turret | `MAS Turret Flank Port` | Port lower skirt L2 · Anti-personnel / anti-spider perimeter guard |
| Interior Turret | `MAS Turret Flank Stbd` | Starboard lower skirt L2 · Anti-personnel / anti-spider perimeter guard |
| AI Move Block | `MAS AI Move` | Autonomous pathfinding & terrain navigation (uncrewed role) |
| AI Task Block | `MAS AI Task` | Autonomous waypoint execution & ore vein transit |
| AI Defense Block | `MAS AI Defense` | Threat avoidance · Auto-locks outriggers and stows boom |
| Piston (Large) | `MAS Outrigger Piston 1..4` | Corner bedrock stabilizer cylinders · `Share Inertia Tensor = ON` |
| Magnetic Plate | `MAS Outrigger Pad 1..4` | Corner bedrock voxel locking pads · `Auto-Lock = ON` |
| Motor Hinge | `MAS Crew Ladder Hinge` | Port flank boarding ladder articulator · 0° stowed to 90° down |
| Sensor | `MAS Sensor Ladder` | 2.5 m crew proximity sensor on Port flank (auto-deploy) |
| Advanced Rotor | `MAS Turret Azimuth` | Dorsal boom turntable · 360° pan · `Torque = 50 MN·m` |
| Motor Hinge | `MAS Shoulder Pitch` | Boom elevation joint · −20° to +75° · `Torque = 45 MN·m` |
| Motor Hinge | `MAS Elbow Pitch` | Boom mid-joint · −100° to +45° · `Torque = 45 MN·m` |
| Advanced Rotor | `MAS Wrist Roll` | Tool head roll alignment · ±180° · `Torque = 20 MN·m` |
| Motor Hinge | `MAS Wrist Pitch` | Tool head horizon leveling · −90° to +90° · `Torque = 20 MN·m` |
| Laser Multitool | `MAS Laser 1, 2` | Mining beam heads on tool head · Continuous extraction |
| Sensor | `MAS Sensor Boom` | 4.0 m forward proximity collision safety buffer |
| Connector (Large) | `MAS MP Conn` | Roof flush connector for MULE-01 shuttle docking |
| Connector (Large) | `MAS Stern Dump Conn` | Stern bumper connector for CATERPILLAR-01 bulk train docking |
| Conveyor Sorter | `MAS MP Sorter` | High-priority rare ore drain sorter (U, Pt, Au, Mg) |
| Conveyor Sorter | `MAS Stern Sorter` | High-volume bulk ore drain sorter (Fe, Ni, Si, Stone) |
| Conveyor Sorter | `MAS Sorter Tailings` | Underbelly stone & gravel purge filter (selective venting) |
| Conveyor Ejector | `MAS Ejector Tailings 1, 2` | Angled underbelly nozzles laying aggregate roadbed |
| Basic Refinery | `MAS Field Refinery` | Closed-loop field metallurgy · 330 kW · Ingests raw ore, outputs ingots |
| Hydrogen Engine | `MAS Engine H2` | Standby cold-start emergency generator · 5.0 MW failover |
| Solar Panel | `MAS Solar 1..4` | Flush roof solar array · 640 kW peak† · Zero-fuel watchkeeping |
| Battery (Large) | `MAS Battery 1..3` | Tractor Power Core Battery Bank · 9.0 MWh · 36 MW discharge |
| Battery (Large) | `MAS Battery 4..6` | Utility Sub-Floor Battery Bank · 9.0 MWh · 36 MW discharge |
| Sliding Door | `MAS Door Airlock` | Port L2/L3 entry airlock door |
| Sliding Door | `MAS Door Vault` | Power Core Vault maintenance entrance from Jefferies tube |
| Sliding Door | `MAS Door Gangway Fwd` | Tractor Aft Bulkhead gangway isolation door |
| Sliding Door | `MAS Door Gangway Aft` | Utility Forward Bulkhead gangway isolation door |
| Event Controller | `MAS EC Stabilizers` | Blocks boom arm deployment until outriggers report locked |
| Event Controller | `MAS EC Level` | Auto-leveling bedrock jack sequence on sloped terrain |
| Event Controller | `MAS EC Ladder` | Auto-deploys crew ladder on approach; retracts on drive |
| Event Controller | `MAS EC Tailings` | Pulses stone ejectors when tractor ore buffer > 50% |
| Event Controller | `MAS EC Hitch Speed` | Locks hitch yaw (100 MN·m torque) at speeds > 35 km/h |
| Event Controller | `MAS EC Hitch Rev` | Clamps hitch yaw to ±15° and inverts Axle 5 when reversing |
| Event Controller | `MAS EC Holds` | Monitors payload mass; alerts at 2,500 t limit |
| Event Controller | `MAS EC Handover` | Monitors Ground Base heartbeat; triggers C2 handover |
| Event Controller | `MAS EC Power` | Fuel watchdog: auto-starts `MAS Engine H2` on low uranium (< 1.0 kg) |
| Event Controller | `MAS EC Ice` | Ice intake watchdog: routes raw ice to O2/H2 generators when H2 < 90% |
| Event Controller | `MAS EC Combat` | Combat alert watchdog · Auto-triggers `RIG.OS.MODE CBT` on damage/target lock |

### 4. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MAS EC Stabilizers`| `MAS Outrigger Pad 1..4` | All 4 Pads = Locked | Enable `MAS Boom Joints` · Sound Ready Chime | Lock `MAS Boom Joints` · Fold Boom |
| `MAS EC Level` | `MAS Helm` Roll & Pitch | Roll / Pitch $> 1.0^\circ$ | Differential Piston Extension | Lock Mag Plates (0.0° Level) |
| `MAS EC Ladder` | `MAS Sensor Ladder` | Player within 2.5 m | Deploy Ladder (Hinge 90° Down) | Retract Ladder (Hinge 0° Flush) |
| `MAS EC Tailings` | Tractor Ore Buffer | Ore Fill $\ge 50\%$ | `MAS Sorter Tailings` Drain ON · Ejectors ON | Sorter & Ejectors OFF |
| `MAS EC Hitch Speed`| Consist Speedometer | Speed $> 35\text{ km/h}$ | Hitch Torque = 100 MN·m (Anti-Sway) | Normal Hitch Torque (45 MN·m) |
| `MAS EC Hitch Rev` | Transmission Gear | Gear = Reverse | Clamp Yaw ±15° · Axle 5 Positive Tracking | Normal Yaw ±45° · Counter-Steer |
| `MAS EC Holds` | Combined Ore Mass | Ore Mass $\ge 2,500\text{ t}$ | Sound Payload Horn · IGC `TMS.MULE.CALL` | Normal Extraction Mode |
| `MAS EC MP` | `MAS MP Conn` | Status = Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle Retract (0°) |
| `MAS EC Train` | `MAS Stern Dump Conn` | Status = Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Handover` | `GND-01` Heartbeat | Signal Detected | Run PB `RIG//OS-Lite` (Argument: `HANDOVER`) | Maintain Mothership Authority |
| `MAS EC Power` | `MAS Reactor 1, 2` Uranium | Uranium Mass $< 1.0\text{ kg}$ | `MAS Engine H2` Turn ON · Sound Fuel Horn | `MAS Engine H2` Turn OFF |
| `MAS EC Ice` | `MAS Tank H2 1, 2` Fill | Hydrogen Fill $< 90\%$ | Enable Ice Conveyor Sorter & O2/H2 Gens | Normal Conveyor Routing |
| `MAS EC Combat` | Hull Integrity / Target Lock | Damage Detected OR Hostile Lock | Engage `MAS Turrets` · Elevate `RIG.OS.MODE CBT` | Standby Watchkeeping |

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
* [ ] **Collision Sensor Stop:** Drive boom tool head toward obstacle; confirm `MAS Sensor Boom` stops joint movement at 4.0 m distance.

### C · Mobile FOB & Dual Logistics Verification
* [ ] **Mule Air Offload:** Land MULE-01 on utility roof pad. Verify cradle arms swing 30° inward, magnetic plates clamp, and rare ores drain at $> 150\text{ t/min}$.
* [ ] **Caterpillar Train Rail Dump:** Reverse Caterpillar train into stern bumper. Verify large connector locks and 2,500 t bulk ore dumps in $< 90\text{ seconds}$.
* [ ] **Field Workshop Operations:** Queue 1,000 Steel Plates in `MAS Utility Assembler`. Verify autonomous fabrication using mined iron ingots.

### D · C2 Fleet Orchestration & Handover Trials
* [ ] **Mothership Heartbeat Broadcast:** Confirm `MAS-01` broadcasts `RIG.C2.HEARTBEAT;ROLE=MOTHERSHIP` on 50 km radio and laser comms.
* [ ] **Subordinate Worker Binding:** Simulate subordinate worker `MAS-02`; confirm it binds to `MAS-01` and appears on Station 3 Tactical Video Wall.
* [ ] **Logistics Task Dispatch:** Trigger 90% ore buffer threshold; verify `RIG//OS-Lite` autonomously broadcasts `TMS.MULE.CALL` and `TMS.CAT.CALL`.
* [ ] **Ground Base Handover Protocol:** Simulate `GND-01` Ground Base C2 boot; verify `MAS-01` detects `ROLE=MASTER_BASE`, transmits fleet registry, and steps down to `FIELD_LEADER` without human intervention.

### E · 5-Point Immersive Automation Verification
* [ ] **Auto-Leveling Bedrock Jacks:** Stop consist on 15° slope; trigger `MAS EC Level`. Verify downhill pistons extend further and deck achieves 0.0° pitch and 0.0° roll before mag plates lock.
* [ ] **Retractable Boarding Ladder:** Approach Port flank on foot; verify ladder swings down 90°. Release handbrake; verify ladder auto-retracts flush with armor skirt.
* [ ] **Active Gravity-Horizon Tool Head:** Elevate shoulder hinge to +45°; confirm wrist pitch automatically counter-rotates -15° to maintain level cutting beam.
* [ ] **Tailings Aggregate Spreader:** Mine stone until buffer > 50%; verify `MAS Sorter Tailings` and ejectors vent gravel behind wheels.
* [ ] **Smart Anti-Jackknife Drawbar:** Accelerate above 35 km/h; verify rotor braking torque jumps to 100 MN·m. Shift to reverse; verify yaw clamp limits to ±15°.

### F · Tri-Fuel Power Redundancy & Closed-Loop Field Metallurgy Acceptance
* [ ] **Closed-Loop Metallurgy Cycle:** Laser-mine 50 t Iron Ore with articulated boom. Verify ore flows into `MAS Field Refinery`, refines into Iron Ingots, and transfers into `MAS Utility Assembler` to fabricate 250 Steel Plates with zero manual inventory handling.
* [ ] **Tier 1 to Tier 3 Power Failover:** Cut uranium supply to `MAS Reactor 1, 2`. Confirm `MAS EC Power` triggers, cold-starts `MAS Engine H2` within 2.0 s, and maintains full tractive and boom operational capability (5.0 MW).
* [ ] **Tier 2 Zero-Fuel Watchkeeping:** Turn off nuclear reactors and H2 engine during daylight. Confirm `MAS Solar 1..4` deliver daylight power, maintaining life support, telemetry, and battery float-charging.
* [ ] **Automated Ice Replenishment:** Mine 20 t Ice. Confirm `MAS EC Ice` routes ice to O2/H2 generators and begins refilling the 10,000,000 L hydrogen reserve.

### G · Player Circulation, Maintenance Access & Hybrid Power Modes Acceptance
* [ ] **Ground-to-Bridge Boarding Flow:** Step onto Port L2 boarding ladder from ground level (L0). Verify ladder elevates crew to L2, airlock door cycles, and interior ladder leads up to Station 1 Helm.
* [ ] **Jefferies Tube Traversal:** Walk full length of Port maintenance crawlway from Z +6 to Z −7. Confirm physical cargo hatch on `MAS Tractor Buffer` opens and allows manual inventory transfer.
* [ ] **Power Vault Ingress:** Open `MAS Door Vault`. Step inside power compartment; confirm walk-up access to manual fuel hatches on `MAS Reactor 1, 2` and manual shutoff valve on `MAS Engine H2`.
* [ ] **Inter-Carriage Gangway Transit:** Walk across flexible accordion gangway while consist is in motion at 35 km/h. Confirm gangway remains airtight, isolates crew from moving hitch joints, and permits unhindered passage between carriages.
* [ ] **Flight Deck Access & Catwalk Inspection:** Climb forward ladderwell from workshop to Level L6. Walk full perimeter catwalk with safety railings around Mule pad, forward to Kestrel pads, and along solar inspection gangways.
* [ ] **Stern Dump Descent:** Descend aft ladderwell to Level L2 stern bumper. Confirm safe physical access to `MAS Stern Dump Conn` and `MAS Stern Sorter`.
* [ ] **Hybrid Power 5-Mode Sequencing:**
  - Test Mode 1 (Silent Watchkeeping): Reactors and H2 engine OFF; confirm `MAS Solar 1..4` float-charge 18.0 MWh battery bank.
  - Test Mode 2 (Tractive Cruise): Single reactor active; verify batteries smooth acceleration spikes.
  - Test Mode 3 (Heavy Excavation): Twin reactors active; verify continuous 30 MW supply to boom lasers, outriggers, and workshop.
  - Test Mode 4 (Emergency Failover): Cut uranium; confirm `MAS EC Power` starts `MAS Engine H2` and sheds workshop loads.
  - Test Mode 5 (Shore Power Fast Charge): Connect to external grid; verify batteries switch to 100% recharge rate.

### H · Perimeter Defense, Combat Alert & Concrete Recycling Acceptance
* [ ] **Automated Combat Mode Elevate:** Simulate hostile target acquisition or hull damage. Confirm Event Controller `MAS EC Combat` transitions alert status to `RIG.OS.MODE CBT`, powers up all turrets, seals crew airlocks, and sounds internal alarms.
* [ ] **Bow & Stern CIWS Firing Arcs:** Confirm `MAS Turret Bow` tracks and engages targets across 360° forward/zenith hemisphere without clipping C2 bridge canopy or boom pedestal. Confirm `MAS Turret Stern` provides 270° coverage over aft hemisphere and landing approach.
* [ ] **Flank Anti-Personnel Coverage:** Verify `MAS Turret Flank Port` and `MAS Turret Flank Stbd` suppress ground-level targets around the boarding ladder and wheel wells.
* [ ] **Automated Ammo Delivery:** Confirm NATO 25×184mm ammunition pulls automatically from `MAS Hold Components` and `MAS Utility Assembler` through the conveyor spine.
* [ ] **Concrete Component Recycling (Decision 60):** Laser-mine 50 t stone. Verify gravel byproduct routes into `MAS Utility Assembler`, queued to fabricate `Concrete Components` for Caterpillar roadbed slabs and Ground Base foundation blocks.


