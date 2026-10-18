# Mastodon · Visual Build Instructions

## Chapter 10 · MASTODON-01 (MKI) Expeditionary Mining Locomotive & Mobile FOB

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MASTODON-01 MKI**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65 & 66)
* **Role:** Long-range terrestrial expeditionary mining locomotive, self-contained mobile Forward Operating Base (FOB), and **Genesis Seed Fleet Mothership** running **`RIG//OS-Lite`**. In fresh survival campaigns prior to Ground Base refinery commissioning, the lead Mastodon serves as the regional C2 fleet root, commanding subordinate excavators (`MAS-02`), bulk land trains (`CAT-01`), and freight shuttles (`MUL-01`), with automated handover to Ground Base upon commissioning.
* **Architecture:** Segmented articulated train consist:
  1. **Carriage 1 (`MAS-01-T`): Locomotive & Excavator Unit (11 W × 20 L × 5 H B · 6 Wheels):** Heavy tractive engine, 3-operator armored panoramic C2 bridge (Helm, Boom CTC, Tactical Director), Tri-Fuel primary power plant (30 MW nuclear + 5 MW emergency H2 engine + 9 MWh battery reserve across 3× Large Batteries), dual-PB compute rack (`PB Excavator` + `PB RIG//OS-Lite`), 40 km laser comms mast, 4× hydraulic corner outriggers with differential auto-leveling and **deep seismic voxel sensors** (Mod 2819263781), dorsal 35 m spine-stowed articulated 5-DOF laser mining boom with **heavy magnetic crane grapple**, chassis travel stow clamp (`MAS Clamp Boom Stow`), bow CIWS Gatling Turret (`MAS Turret Bow`), flank anti-personnel turrets, Port L3 maintenance Jefferies tube, ground-level EVA survival lockers, and aft 2-axis motorized drawbar hitch.
  2. **Carriage 2 (`MAS-01-U`): Modular FOB & Utility Carriage (11 W × 18 L × 5 H B · 4 Wheels):** Field workshop (1× Basic Refinery for in-situ metallurgy, 1× Large Assembler, Survival Kit / Medical Bay), free planetary atmospheric O2 intake vents, sub-floor battery vault (9 MWh across 3× Large Batteries), 4× roof flush solar panels (`≈ 640 kW` peak†), component reserve hold, atmospheric bunkering (2× Large H2 tanks, O2/H2 generators), 7 × 7 B MULE-01 Gantry Cradle Pad with motorized clamping arms, 2× KESTREL-01 scout drone landing pads with **recessed automated repair welders**, roof maintenance catwalks, 360° dorsal worksite floodlight tower, stern CIWS Gatling Turret (`MAS Turret Stern`), and stern heavy dump bumper matching CATERPILLAR-01.
* **Grid:** Large Grid (1 B = 2.5 m) · **Consist Envelope:** 11 W × 42 L × 6 H B (27.5 × 105.0 × 15.0 m).
* **Mass:** Consist Dry Mass `≈ 978.5 t` (Tractor: 543.2 t, Utility: 435.3 t) · Max Laden Mass `≈ 3,456 t` (Earth 1 g).
* **Payload Capacity:** 2,500 t bulk ore across tractor buffer and utility holds.
* **Mechanical & Automation Suite (Decision 57):** 5-point immersive automation: auto-leveling bedrock jacks (sloped terrain compensation up to 20°), retractable crew boarding ladder (Port flank L2), active gravity-horizon tool head leveling with 4.0 m safety proximity stop, underbelly stone tailings aggregate spreader, and smart anti-jackknife drawbar dynamics.
* **Tri-Fuel Power & Closed-Loop Metallurgy Suite (Decision 58):** Self-sustaining field endurance without resupply: (1) 1× Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW) refining raw mined Fe, Ni, Co, Si, Mg on-site into ingots for the Large Assembler; (2) Tri-Fuel Power Architecture: Tier 1 Nuclear (2× Small Reactors, 30 MW), Tier 2 Renewable (4× Roof Flush Solar Panels, `≈ 640 kW` peak†), Tier 3 Emergency Hydrogen (1× Hydrogen Engine, 5 MW) drawing from 10,000,000 L H2 tank farm; (3) Automated resource watchdogs `MAS EC Power` (uranium watchdog auto-starting H2 engine) and `MAS EC Ice` (ice intake routing).
* **Player Circulation & Hybrid Power Suite (Decision 59):** Comprehensive survival engineer ergonomics and versatile power management: (1) Pressurized player circulation network with Port L2 boarding airlock, Port L3 Jefferies tube / crawlway (X −2) bypassing cargo buffer and power vault, enclosed inter-carriage articulated accordion gangway (Z −9 to −8), L3 central workshop corridor, vertical flight ladderwell to Level L6, and flight deck catwalks around Mule/Kestrel/solar decks; (2) Physical maintenance & manual inventory ports exposed on every major storage, reactor, engine, refinery, and sorter block; (3) Upgraded 18.0 MWh Energy Storage Buffer (6× Large Batteries: 3 in Tractor Power Vault, 3 in Utility Sub-Floor, delivering 72.0 MW peak instantaneous discharge); (4) 5 automated operational energy management modes (Silent Watchkeeping, Tractive Cruise, Heavy Excavation, Emergency Failover, and Shore Power Fast Recharge).
* **Terrestrial Concrete Civil Engineering (Decision 60):** Solves excess quarrying aggregate through integration of the Concrete Armor Blocks mod ([303959338](https://steamcommunity.com/sharedfiles/filedetails/?id=303959338)). Gravel from stone refining is assembled into Concrete Components, producing blast-resistant pavers for Caterpillar roadbeds and Ground Base foundations while preserving 100% vanilla rule C2 on aerial fleet craft and the Rig.
* **Perimeter Defense CIWS & Warfare 3 MKII Roadmap (Decision 61):** 360° close-in weapon system (CIWS) and anti-intrusion grid: (1) 2× Large Grid Gatling Turrets (`MAS Turret Bow` at Carriage 1 dorsal crown L5, Z +7, X 0; `MAS Turret Stern` at Carriage 2 stern gantry L5, Z −26, X 0) plumbed into conveyor spine for automated ammo feeds from `MAS Utility Assembler`; (2) 2× Flush Ground-Level Anti-Personnel Interior Turrets (`MAS Turret Flank Port/Stbd` at L2, X ±4, Z +2) covering boarding ladder and perimeter; (3) Automated combat alert integration via `RIG//OS-Lite` (`RIG.OS.MODE CBT`); (4) Pre-Warfare 3 operational baseline with architectural upgrade path to **MASTODON-02 MKII** upon Warfare 3 DLC release.
* **5-Point Field Logistics & Recovery Suite (Decision 62):** Complete expeditionary autonomy: (1) Deep Seismic Voxel Prospecting (4× Seismic Surveying outrigger feet, Mod 2819263781, 500 m pulse); (2) Flight Deck Drone Maintenance Welders (2× recessed roof welders under Kestrel pads); (3) 45 m Boom Heavy Crane & Recovery Grapple (1× wrist magnetic plate, Macro 4 `MACRO_CRANE`); (4) Atmospheric Scavenging & Ground EVA Lockers (2× intake vents for free O2, 2× flank ground-level lockers); (5) Coordinated Worksite Floodlight Matrix (boom spotlights, 360° tower floodlights, underbelly inspection lights).
* **Extraction Arm Kinematics & Travel Stow Architecture (Decision 63):** Symmetrical heavy excavation and overland transit: (1) Centerline Turret Pedestal at [X0, Z +6, Y = L4] on 1× Large Advanced Rotor (`MAS Turret Azimuth`), providing 360° horizontal rotation and 21.25 m physical reach (50+ m laser reach) off both Port and Starboard flanks; (2) Travel Mode (Rigid Dorsal Spine Stow): Arm slews 180° Aft along the 14-block Tractor spine trough (Z +6 to Z −8) at Level L4; tool head locks into chassis Magnetic Plate `MAS Clamp Boom Stow` at [X0, Z −7, L4] with all hinges at 0°, zero hitch overhang, zero cab obstruction, and maximum transit height 5.2 m; (3) 90° Downward Elbow Tool Adapter: Sets neutral reference to straight down (−Y) with ±60° pitch compensation, ensuring true vertical downward aim (vector [0, -1, 0]) across 100% of the operating radius (6.0 m to 35.0 m) and depth down to −25 m (−55 m with laser beams); (4) Target-Centric Vertical Column Boring (Macro 2): `[MAS] PB Excavator` translates the arm in 2D concentric circular sweeps (ΔX, ΔZ) centered on an offset target waypoint (Xt, Zt) while `MAS Wrist Roll` spins twin lasers at 5.0 rpm for cylindrical shaft coring.
* **Custom Bridge, ULSS & Emergency Survivability Suite (Decision 64):** Comprehensive interior spaces, multi-planetary life support, and native block failsafes (Rule C4): (1) Custom Walk-In Armored Bridge replacing prefab cockpit with three 1×1×1 open Control Stations (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`) side-by-side at L4 within a 45° sloped heavy armor glass canopy, walk-around deck, 3-panel Tactical Video Wall, and 3 fully populated 9-slot hotbars per station (Hotbars 1-3); (2) Universal Closed-Loop Life Support (ULSS): Upgrades consist for vacuum (Moon, Europa) and toxic (Mars, Alien) worlds with 2× Large Oxygen Tanks (200,000 L O2), 1× Quarantined Emergency Ice Hold (`MAS Hold Ice LifeSupport`, 15,000 L), 2× Cryo Chambers (`MAS Cryo Pod 1, 2`), 4-zone hermetic isolation with automated breach seals, and 3 active gas-reclamation decompression airlocks; (3) Native Threat & Mobility Failsafes (Rule C4): 3× Armored Decoys (`MAS Decoy Flank Port/Stbd`, `MAS Decoy Stern`), 1× Pyrotechnic Hitch Severing Merge Block (`MAS Hitch Sever Tongue`), 2× Inverted Counter-Weight Gyros (`MAS Gyro Recovery 1, 2`), and an armored Black Box 50 km SOS Beacon (`MAS Beacon Distress`) with isolated cold battery (`MAS Battery BlackBox`); (4) Genesis Reconstruction & Anti-Capture Scuttle: 1× Master Projector (`MAS Projector Master`) for field self-repair from internal parts hold, plus 2× Armored Warheads (`MAS Warhead Scuttle Bridge/Power`) with 60 s native countdown timer and manual abort; (5) Physical In-Situ Controls: 6× wall-mounted Sci-Fi 4-Button Panels; (6) Updated BOM: 1,039 blocks, dry mass ≈ 978.5 t, 7,198 PCU.
* **Tri-Mode Laser Multitool Salvage & Remote Welding Suite (Decision 65):** Full lifecycle deconstruction and remote engineering via ToolCore mod (Mods 3175774286 / 3040438530): (1) Tri-Mode Operation: 2× Simple Laser Multitools (`MAS Laser 1, 2`) switch between Mining (Drill / raw ore intake), Salvage (Grinder / non-contact wreck deconstruction at 15-30 m standoff with 100% component retention), and Fabrication (Welder / remote structural welding and bridge assembly at 35 m); (2) Closed-Loop Field Recycling Circuit: Salvaged components route across drawbar hitch into `MAS Hold Components` (3×3×3 Large Cargo), scrap metal melts into pure iron ingots in `MAS Field Refinery` (1×1×2 B, 330 kW), and surplus parts disassemble in `MAS Utility Assembler` to constituent ingots (Fe, Ni, Co, Si, Pt, Au) for immediate ammo and repair plate fabrication; (3) Macro 5 Wreck Deconstruction (`MACRO_SALVAGE`): `[MAS] PB Excavator` executes planar raster sweeps (ΔY, ΔX) across target bounding boxes, peeling enemy wrecks and obsolete structures layer-by-layer; (4) Station 2 Hotbar updates (HB1:8 Next Mode, HB2:8 Macro 5 SALVAGE); (5) Zero hardware impact: 0 new blocks, 0 additional PCU, 0 additional mass (BOM remains 1,039 blocks, ≈ 978.5 t, 7,198 PCU).
* **Multi-Tier Fleet Comms, Predictive JIT Logistics & Swarm Synergy (Decision 66):** Strengthens inter-grid communication (IGC) and cross-fleet operations across Mastodon, support drones (Kestrel, Albatross), cargo transports (Mule, Caterpillar), Ground Base (GND-01), and peer Mastodons: (1) Predictive Just-In-Time (JIT) Logistics Dispatch: Real-time mass tracking (ΔMass / Δt) computes buffer time-to-full (T_remain) and pre-dispatches transports at 75-80% capacity via `TMS.LOGISTICS.JIT`, eliminating excavator idle waiting time; (2) Dynamic Logistics Triage: Segregates high-value rare ores (U, Pt, Au, Mg) and salvaged high-tech components to fast Air Tier (Mule gantry cradle pad) while routing bulk ores (Fe, Ni, Si, stone) to mass Ground Tier (Caterpillar stern dump bumper); (3) Two-Tier Geological Triangulation & Aerial IGC Relay: Mastodon tasks Kestrel via `TMS.SCOUT.TASK` for wide-area sweeps, followed by Mastodon 500 m deep acoustic seismic survey (`TMS.SEISMIC.SURVEY`) to lock 3D drill centroids; airborne Kestrels serve as dynamic line-of-sight (LOS) IGC repeaters when Mastodons operate in deep pits; (4) Demand-Driven Pull Economy: Ground Base C2 broadcasts refinery deficit matrix via `TMS.BASE.DEMAND`; Mastodon automatically vectors quarry operations to match base mineral deficits; (5) Multi-Mastodon Swarm Quarrying & Coordinated CIWS: Spatial exclusion zone broadcast via `TMS.ZONE.CLAIM` deconflicts bench quarrying envelopes, while distributed targeting via `TMS.DEFENSE.TARGET` synchronizes anti-air/missile defense across consists.
* **Operating Environment:** Universal planetary & terrestrial surface operations (Earth 1 g baseline, airless vacuum moons, thin-atmosphere deserts, and hostile alien terrain).

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
| **Corner Outriggers** | 4× Large Pistons + 4× Seismic Surveying Feet (Palette 1 · Mod 2819263781) | 8 | 18.4 t | 260 |
| **Tractor Power Core** | 2× Small Nuclear Reactors, 1× Hydrogen Engine, 3× Large Batteries (Palette 6) | 6 | 31.6 t | 730 |
| **Articulated Boom & Crane** | 2× Adv Rotors, 3× Motor Hinges, 2× Lasers, 1× Crane Grapple, 1× Stow Clamp, 1× 90° Tool Elbow, Girders, Spotlights (Palette 3) | 23 | 39.6 t | 1,020 |
| **Armored Bridge & C2** | 3× Control Stations, 2× PBs, Laser Antenna, Glass, Ore Detector, 1× Scuttle Warhead, 1× Timer, 2× Button Panels | 23 | 16.2 t | 470 |
| **Drawbar Hitch & Severance** | 1× Adv Rotor, 1× Motor Hinge, 1× Merge Block (Hitch Sever Tongue), Heavy Armor Joint Blocks | 7 | 13.5 t | 235 |
| **Utility Carriage Hull** | Heavy armor frames, wheel wells, deck armor (Palette 0) | 360 | 306.0 t | 360 |
| **Utility Powertrain** | 4× 5×5 Heavy Suspension Wheels (Axles 4-5) | 4 | 32.0 t | 400 |
| **Utility Sub-Floor Battery Vault** | 3× Large Batteries (9.0 MWh, Palette 6) | 3 | 13.5 t | 120 |
| **Interior Walkways & Decoys** | Grated catwalks, sliding doors, ladder runs, interior lights, 3× Armored Decoys, 2× Keel Recovery Gyros (Palette 9) | 30 | 18.0 t | 240 |
| **Field Workshop & Universal Life Support** | 1× Basic Refinery, 1× Large Assembler, 1× Medical Room, 2× Cryo Pods, 1× Survival Kit, 2× Large O2 Tanks, 1× Quarantined Ice Hold, 2× Large H2 Tanks, 2× O2/H2 Gens, 6× Air Vents, 1× Master Projector, 1× Scuttle Warhead | 22 | 68.2 t | 815 |
| **Utility Cargo Hold** | 1× Large Cargo Container (Components), Conveyor network | 16 | 28.0 t | 160 |
| **Flight Decks, Welders & Solar** | 1× Mule Pad, 2× Kestrel Pads, 2× Drone Welders, Stern Dump, 4× Solar Panels, 4× Floodlights | 68 | 61.6 t | 980 |
| **Perimeter Defense Suite** | 2× Gatling Turrets, 2× Interior Turrets, Ammunition Conveyor Feeds (Palette 7) | 12 | 9.6 t | 308 |
| **External EVA, Lights & Failsafes** | 2× Ground EVA Lockers, 3× Keel Lights, 1× Black Box SOS Beacon & Isolated Battery, 4× In-Situ Button Panels (Palette 9) | 11 | 2.5 t | 110 |
| **TOTAL CONSIST (DRY)** | Complete Expeditionary Mining Locomotive & Mobile FOB | **1,039** | **≈ 978.5 t** | **7,198** |

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

### Step 10.5 · Heavy Motorized 2-Axis Drawbar Hitch & Pyrotechnic Severance (Z = −8, X0, L2)

At the aft bulkhead of the tractor, install a heavy-duty 2-axis articulated drawbar hitch that connects the tractor to the utility carriage mechanically and routes the main large conveyor line:
1. **Azimuth (Yaw) Joint:** 1× Large Advanced Rotor mounted at Z = −8 facing Aft (−Z). Allows the consist to articulate horizontally when turning.
2. **Pitch Joint:** 1× Motorized Large Hinge connected directly to the rotor head. Allows the consist to traverse crests and dips over rough terrain without lifting wheels.
3. **Pyrotechnic Severance Merge Block (`MAS Hitch Sever Tongue`):** Mount 1× Large Grid Merge Block directly on the drawbar tongue at [X0, Z −8, Y = L2]. In emergency catastrophic events (runaway fire, trailer rolling over a cliff, nuclear containment breach), triggering `Hitch Eject` physically decouples Carriage 1 and Carriage 2 instantly, allowing the tractor to escape as an independent 6×6 unit or leaving the utility carriage as a self-powered static FOB.
4. **Settings:** Share Inertia Tensor = `ON` · Braking Torque = `45 MN·m`.

```
2.5D Isometric Callout · 2-Axis Articulated Drawbar Hitch & Severance
    Tractor Aft Bulkhead (Z −8)
           ║
           ╠══[ADVANCED ROTOR] (Horizontal Yaw Articulation: −45° to +45°)
           ║        │
           ║        ▼
           ║   [MOTOR HINGE] (Vertical Pitch Articulation: −25° to +25°)
           ║        │
           ║        ▼
           ║   [MERGE BLOCK: MAS HITCH SEVER TONGUE] (Emergency Decoupling)
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

---

### Step 10.5D · Ground-Level EVA Survival Lockers & Underbelly Inspection Lights (L1-L2)

To ensure field technicians can access emergency life support and tools without entering the pressurized habitat:
* **Flank EVA Lockers:** Mount 2× Small Cargo Containers (`MAS Locker Port` at [X −5, Z −4, Y = L2] and `MAS Locker Stbd` at [X +5, Z −4, Y = L2]), recessed flush into the lower armored skirts. Painted Palette Slot 9 (High-Viz Safety Yellow). Accessible directly from ground level; pre-stocked with emergency hydrogen suit bottles, oxygen bottles, tool welders, and 5.56mm/25mm ammunition. Plumbed with auto-refill bottle feeds.
* **Underbelly Keel Lighting:** Mount 3× Interior Lights (`MAS Lights Underbelly`) along the centerline chassis keel at [X0, Z +5], [X0, Z0], and [X0, Z −5] at Level L1. Casts low-intensity amber illumination across wheel suspension hubs, outrigger cylinders, and the tailings aggregate manifold for night maintenance.

---

### Step 10.5E · Native Threat Decoys, Recovery Gyros & Black Box SOS Cell (L1-L3)

To guarantee survival and asset protection under severe attack or total vehicle immobilization (Rule C4 native block compliance):
* **Ballistic Decoy Sponsons (`MAS Decoy Flank Port/Stbd`, `MAS Decoy Stern`):** Mount 3× Decoys encased within heavy armor and blast door blocks at [X −5, Z −4, Y = L2] (Port skirt), [X +5, Z −4, Y = L2] (Starboard skirt), and [X0, Z −26, Y = L2] (Stern bumper). In Space Engineers, automated turret AI targets Decoys first, diverting artillery, rockets, and gatling rounds into the heavily armored lower chassis skirts away from the bridge canopy and reactors.
* **Self-Righting Keel Gyroscopes (`MAS Gyro Recovery 1, 2`):** Mount 2× Large Grid Gyroscopes in the lower chassis keel at [X0, Z0, Y = L1]. During normal transit, `Override = OFF`. If the consist rolls onto its side or inverts on steep terrain, toggling `Override = ON` (Roll +100%) applies instant angular torque to roll the consist back onto its wheels.
* **Armored Black Box & SOS Distress Beacon (`MAS Beacon Distress`):** Encase 1× Radio Antenna / Beacon (`MAS Beacon Distress`, 50 km broadcast) and 1× Small Battery (`MAS Battery BlackBox`, isolated, `Recharge = OFF`) in a 6-sided heavy armor capsule at [X0, Z +7, Y = L1]. Native Event Controller `MAS EC BlackBox` monitors main grid power: upon total electrical blackout (0.0 MW), the isolated battery auto-activates, broadcasting an emergency GPS distress ping `SOS-MASTODON-01-LOST`.
* **Power Vault Scuttle Charge (`MAS Warhead Scuttle Power`):** Mount 1× Large Grid Warhead between the two nuclear reactors at [X0, Z −3, Y = L3]. Disarmed by default. Linked to the bridge scuttle countdown loop for anti-capture asset destruction.
* **Power Vault In-Situ Control Panel (`MAS BP Power Vault`):** Mount 1× Sci-Fi 4-Button Panel on the Jefferies tube wall at [X −2, Z −1, Y = L3] (Button 1: Vault Door Toggle, Button 2: Reactor SCRAM, Button 3: H2 Engine Cold-Start, Button 4: Battery Bank Shore Charge).

#### Pre-Enclosure Inspection Gate 10.2A: Power Core Isolation, Jefferies Tube & Gangway Test
* [ ] Verify reactor automated uranium pull through the central conveyor trunk.
* [ ] Confirm `MAS Engine H2` cold-starts and delivers 5.0 MW upon manual toggle or simulated uranium depletion.
* [ ] Confirm total tractor power capacity reads `71.0 MW` peak (30 MW nuclear + 5 MW H2 engine + 36 MW battery discharge from 3× Large Batteries).
* [ ] Verify Port Jefferies tube allows unobstructed human walking from Bridge to Aft Bulkhead.
* [ ] Confirm physical maintenance port on `MAS Tractor Buffer` opens and allows manual inventory access.
* [ ] Confirm articulated inter-carriage gangway maintains airtight seal and flexes cleanly with hitch movement.
* [ ] Test 2-axis drawbar hitch: ensure rotor and hinge pivot freely without clashing with the chassis frame.
* [ ] Verify `MAS Hitch Sever Tongue` merge block physically separates consist carriages upon `Hitch Decouple` command.
* [ ] Confirm `MAS Decoy Flank Port/Stbd` draw hostile turret lock away from the bridge and reactors.
* [ ] Confirm `MAS Gyro Recovery 1, 2` successfully right consist from a 90° lateral rollover test.
* [ ] Test `MAS Beacon Distress` and `MAS Battery BlackBox`: confirm automated 50 km SOS ping triggers upon simulated total blackout.
* [ ] Verify `MAS EC Hitch Speed` dynamically raises rotor braking torque to 100 MN·m when speed exceeds 35 km/h.
* [ ] Verify `MAS Locker Port/Stbd` are accessible from ground level (L0) and `MAS Lights Underbelly` illuminate suspension hubs.

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
* 4× Seismic Surveying Landing Gear (`MAS Seismic Outrigger 1..4`, TMS Mod 2819263781, Palette 1)
* 1× Large Magnetic Plate (`MAS Tool Grapple`, Palette 9: High-Viz Safety Yellow)
* 1× Large Magnetic Plate (`MAS Clamp Boom Stow`, Palette 1: Hazard Amber · Travel Cradle at Z −7)
* 1× Large Conveyor Elbow (90° Downward Tool Head Adapter, Palette 4: Processing Teal)
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

### Step 10.6C · Deep Seismic Subsurface Voxel Prospecting Array (TMS Mod Integration)

Equip each of the 4 corner outrigger feet with **Seismic Surveying Landing Gear** from the active TMS mod suite (`docs/rig/mods.md`, Mod ID `2819263781`):
* **Hardware Integration:** Replace standard magnetic plate heads with `MAS Seismic Outrigger 1..4` (Palette 1: Hazard Amber).
* **Automated Resonance Pulse:** When `MAS EC Level` locks the outriggers into bedrock at $0.0^\circ$ pitch and roll, Event Controller `MAS EC Seismic` triggers an active acoustic survey pulse.
* **Subsurface Detection:** Maps subterranean mineral density down to $500\text{ m}$† depth in a 300 m radius beneath the anchored consist, identifying deep rare ore deposits (Uranium, Platinum, Gold) far beyond the 150 m range of standard ore detectors.
* **IGC Telemetry:** Coordinates, estimated depth, and deposit signatures are automatically formatted and published over IGC to `TMS.SCOUT.VEIN` and populated on Station 3 Tactical Video Wall.

---

### Step 10.7 · Heavy Turret Pedestal & Azimuth Advanced Rotor (Z = +6, X0, L4)

Mount an Advanced Rotor facing upward (+Y) on the dorsal deck at centerline [X0, Z +6, Y = L4]. This serves as the primary azimuth turntable, providing **360° continuous horizontal rotation** for the excavation boom with an unrestricted large-conveyor pass-through:
* **Mounting Coordinates:** [X0, Z +6, Y = L4] (Directly behind C2 Bridge aft wall, elevated 10.0 m above terrain).
* **Rotor Settings:** Share Inertia Tensor = `ON` · Braking Torque = `50 MN·m` · Velocity = `0.75 rpm`.
* **Symmetrical Flank Reach:** Because the turntable is mounted on the exact centerline (X0), the arm provides identical symmetrical reach to both Port (−X) and Starboard (+X):
  - Chassis Half-Width: 13.75 m (5.5 blocks from centerline to outer tire/skirt).
  - Physical Boom Reach: 35.0 m (14 blocks fully extended).
  - Reach Beyond Either Flank: **21.25 m (8.5 blocks)** of physical reach into open terrain off either side.
  - Effective Extraction Reach: **46.25 m to 56.25 m** when including laser mining beam projection.

---

### Step 10.8 · 5-DOF Articulated Boom Kinematic Chain & Travel Stow Trough (L4-L8)

Assemble the high-articulation robotic excavation and recovery crane:
1. **Shoulder Pitch Hinge (`MAS Shoulder Pitch`):** Motorized Large Hinge connected to the azimuth rotor head. Pitch limits: `−20.0°` (undercutting terrain / stowed trough) to `+75.0°` (high reach). Torque: `45 MN·m`.
2. **Main Boom Arm (7 B Span):** Boxed heavy armor casing enclosing the primary large conveyor pipe (17.5 m length).
3. **Elbow Pitch Hinge (`MAS Elbow Pitch`):** Motorized Large Hinge at the mid-joint. Articulation limits: `−100.0°` (flexed descent) to `+45.0°` (extended reach). Torque: `45 MN·m`.
4. **Forearm Girder (5 B Span):** Tapered structural girder carrying internal piping and utility cabling (12.5 m length).
5. **90° Downward Conveyor Elbow Adapter:** Large conveyor elbow mounted at the tip of the forearm. Sets the neutral tool orientation strictly downward (−Y) toward planetary gravity.
6. **Wrist 2-Axis Articulator:**
   * Wrist Pitch Hinge (`MAS Wrist Pitch`): Motorized Large Hinge connected to the 90° elbow. Provides `−60.0°` to `+60.0°` pitch compensation around true vertical.
   * Wrist Roll Rotor (`MAS Wrist Roll`): Large Advanced Rotor providing continuous 360° tool roll (5.0 rpm) for circular column coring.
7. **Chassis Travel Stow Cradle (`MAS Clamp Boom Stow`):** Large Magnetic Plate mounted flush in the Tractor dorsal spine trough at [X0, Z −7, Y = L4]. Flanked by twin heavy armor coamings running from Z +5 to Z −8.

```
2.5D Isometric Callout · 35 m Long-Reach Boom with 90° Downward Tool Head
       Chassis Pedestal (Z +6, X0, L4)
              ║
              ╠══[TURRET AZIMUTH ROTOR] ────────► 360° Continuous Horizontal Pan
              ║        │
              ║        ▼
              ║   [SHOULDER PITCH HINGE] ───────► Elevation: −20° to +75°
              ║        │
              ║        ▼
              ║   [Main Boom Arm · 7 B Span] (17.5 m Heavy Cased Pipe)
              ║        │
              ║        ▼
              ║   [ELBOW PITCH HINGE] ──────────► Articulation: −100° to +45°
              ║        │
              ║        ▼
              ║   [Forearm Girder · 5 B Span] (12.5 m Tapered Casing)
              ║        │
              ║        ▼
              ║   [90° DOWN CONVEYOR ELBOW] ────► Neutral Reference = Straight Down (−Y)
              ║        │
              ║        ▼
              ║   [WRIST PITCH HINGE] ──────────► Pitch Compensation: −60° to +60°
              ║        │
              ║        ▼
              ║   [WRIST ROLL ROTOR] ───────────► 360° Continuous Spin (5 rpm Core Bore)
              ║        │
              ║        ▼
              ║   [LASER MINING TOOL HEAD]
              ║   (2× Laser Multitools + Tool Grapple + Cam + Lights)
```

```
Boom Operating Configurations:

1. STOWED TRANSIT CONFIGURATION (Traveling Overland at 35 km/h):
   Turret Azimuth: 180.0° (Slewed Aft along Centerline X0)
   Shoulder Pitch:  -5.0° (Resting flat in armored dorsal spine trough)
   Elbow Pitch:      0.0° (Flat inline with main boom arm)
   Wrist Pitch:      0.0° (Tool head locked into chassis deck clamp)
   Stow Clamp:     LOCKED (`MAS Clamp Boom Stow` at [X0, Z −7, L4] Green Lock)
   Total Length:   14 Blocks (35.0 m) from Z +6 to Z −8 (Spans Carriage 1 only)
   Hitch Gap:      Zero Overhang (7 blocks clear of hitch at Z −8 to −9)
   Cab Sightlines: 100% Unobstructed (Zero forward canopy blockage)
   Transit Height: 5.2 m above ground (Fits under 6.0 m hangar gates)

2. TARGET-CENTRIC VERTICAL COLUMN BORING CONFIGURATION (Active Mining):
   Turret Azimuth: Slew to Target Bearing φ = atan2(Xt, Zt)
   Shoulder Pitch: +15.0° to +55.0° (Adjusted per radial distance R)
   Elbow Pitch:    -10.0° to -70.0° (Adjusted per depth Y)
   Wrist Pitch:    Counter-Pitches to lock Tool Vector strictly vertical ([0, -1, 0])
   Wrist Roll:     Spins continuously at 5.0 rpm for circular core coring
   Lateral Sweeps: Arm executes concentric circular orbits (ΔX, ΔZ) around target (Xt, Zt)
   Operating Span: 6.0 m to 35.0 m from turret · Max Depth Reach: -25.0 m (-55 m with beams)
```

---

### Step 10.8B · Active Gravity-Horizon Tool Head Leveling & True Vertical Lock

To enable precision vertical column boring and flat quarry terracing:
* **True Vertical Downward Lock (Column Boring):** With the 90° downward elbow adapter setting the tool neutral vector along −Y, the Wrist Pitch Hinge counter-articulates in exact opposite ratio to the net arm angle ($\theta_{\text{wrist}} = -(\theta_{\text{shoulder}} + \theta_{\text{elbow}})$). This locks the twin mining lasers strictly parallel to planetary gravity ($\vec{u}_{\text{tool}} = [0, -1, 0]$) across the entire radial operating envelope ($6.0\text{ m} \le R \le 35.0\text{ m}$).
* **Horizontal Terracing Mode (Bench Quarrying):** By rotating `MAS Wrist Pitch` by $+90.0^\circ$, the laser beams deflect forward, leveled strictly parallel to the planet surface for cutting flat bench floors.
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
                  [90° DOWN ELBOW] ── (Base direction = −90° DOWN)
                         │
                         ▼
                  [WRIST PITCH HINGE] ── (Counter-Pitches -15° to cancel arm slope)
                         │
                         ▼
               ┌───────────────────┐
               │  [LASER MULTI]    │ ════════════► Fires Strictly DOWN (−Y)
               └───────────────────┘               (Parallel to Planetary Gravity)
```

---

### Step 10.9 · Laser Mining Tool Head, 90° Downward Elbow & Heavy Recovery Grapple

At the tip of the wrist articulator, mount the unified excavation and field recovery head:
* 1 × Large Conveyor Elbow: Mounted at the tip of the forearm, routing the conveyor line 90° downward toward ground baseline.
* 1 × Motor Hinge (`MAS Wrist Pitch`): Mounted to the elbow head, providing `±60.0°` pitch leveling around the vertical axis.
* 1 × Large Advanced Rotor (`MAS Wrist Roll`): Providing continuous 360° axial spin (5.0 rpm) for circular column coring.
* 2 × Simple Laser Multitool (`MAS Laser 1, 2`, Mod 3175774286 / ToolCore 3040438530): Mounted on the roll face, spaced 2.5 m apart. Powered by ToolCore, these units operate dynamically as tri-mode energy emitters:
  1. `Mode 1: Mining Beam (Drill)`: Voxel pulverization and continuous raw ore intake.
  2. `Mode 2: Salvage Beam (Grinder)`: Non-contact thermal deconstruction of target grids at 15 to 30 m standoff, yielding 100% component recovery without physical blade collision damage or tension snagging.
  3. `Mode 3: Fabrication Beam (Welder)`: High-speed remote structural welding from up to 35 m away, enabling chasm bridge assembly and fleet craft repair without crew EVA.
* 1 × Large Magnetic Plate (`MAS Tool Grapple`, Auto-Lock = OFF, Palette 9: High-Viz Safety Yellow) mounted centrally between the laser heads. Provides heavy lifting, vehicle righting, and cargo positioning capability with 45 MN·m joint holding power.
* 1 × Camera · `MAS Cam Boom` (high-magnification targeting crosshairs looking directly down the drill bore).
* 2 × Spotlight · `MAS Light Boom 1, 2` (floodlights illuminating the cutting bench and salvage targets).
* 1 × Sensor · `MAS Sensor Boom` (4.0 m safety stop to prevent head-to-terrain collision).

```
Front Elevation · Laser Mining Tool Head & Recovery Grapple (Tip of Forearm)
                  [90° DOWN CONVEYOR ELBOW]
                            │
                  [WRIST PITCH HINGE]
                            │
                  [WRIST ROLL ROTOR]
                            │
        ┌───────────────────┴───────────────────┐
        │               [CAMERA]                │
        │             MAS Cam Boom              │
        ├───────────────┬───────┬───────────────┤
        │  [LASER 1]    │[MAG]  │  [LASER 2]    │
        │  Simple Laser │TOOL   │  Simple Laser │
        │  Multitool    │GRAPPLE│  Multitool    │
        ├───────────────┼───────┼───────────────┤
        │ [SPOTLIGHT 1] │       │ [SPOTLIGHT 2] │
        └───────────────┴───────┴───────────────┘
```

### Step 10.9B · Automated Excavation Macro Modes & Kinematic Sweep Sequences

To maximize extraction efficiency and support uncrewed autonomous operation, `[MAS] PB Excavator` implements 5 automated sweep, crane recovery, and salvage macros, plus automated travel stow/deploy sequencing:

```
[AUTOMATED BOOM EXCAVATION & CRANE SWEEP PATTERNS]

1. BENCH QUARRY MODE (Horizontal Terracing)
   · Azimuth Turret sweeps across horizontal face: -45.0° (Port) to +45.0° (Starboard).
   · Shoulder Hinge incrementally steps down -2.5° per pass (0.5 B bench cut depth).
   · Wrist Pitch elevates +90.0° to maintain flat, drivable terrace floors parallel to surface.
   · Produces clean stair-stepped quarry cuts into mountainsides.

2. TARGET-CENTRIC VERTICAL COLUMN BORE MODE (Subterranean Access)
   · Accepts target vein waypoint (Xt, Zt) from seismic scan or Station 2 operator.
   · Turret Azimuth slews to target bearing; Shoulder and Elbow extend tool head over target.
   · Wrist Pitch counter-pitches to lock laser beams strictly vertical ([0, -1, 0]).
   · Wrist Roll rotor spins twin offset lasers at 5.0 rpm, cutting an initial 3.0 m core.
   · Motion controller translates the arm in automated 2D concentric circular sweeps (ΔX, ΔZ) around (Xt, Zt) to expand the column diameter (up to 12.5 m wide).
   · Steps downward ΔY = -1.0 m per pass, descending down to -25.0 m physical depth (-55 m with laser beam reach) into deep rare veins.

3. TRENCH & ROAD CARVE MODE (Overland Right-of-Way)
   · Boom deploys along centerline (Z+), executing downward scoop arc (-5.0 m to grade).
   · Tractor crawls forward at 2.0 km/h; outriggers cycle in creeping advance pattern.
   · Automatically clears and grades a level 15.0 m wide roadbed for Caterpillar land trains.

4. CRANE & VEHICLE RECOVERY MODE (Overland Field Salvage)
   · Laser Multitools deactivated; `MAS Tool Grapple` energized.
   · Station 2 CTC mouse control provides direct 3-axis teleoperation of the 45 m boom arm.
   · Allows precision attachment to overturned Caterpillar trailers, scout drones, or supply skids up to 42.5 m away.
   · Utilizes the 45 MN·m joint holding torque and hydraulic outriggers to right rolled vehicles, hoist heavy cargo modules, or lift massive boulders obstructing the haul road.

5. SALVAGE & WRECK DECONSTRUCTION MODE (Macro 5: MACRO_SALVAGE)
   · Simple Laser Multitools cycle to Mode: Salvage Beam (Grinder).
   · Accepts target wreck/structure perimeter or bounding box coordinates.
   · Turret Azimuth, Shoulder, and Elbow position tool head at optimal 15 to 30 m standoff.
   · [MAS] PB Excavator executes automated planar raster sweeps (ΔY, ΔX) across target profile.
   · Non-contact laser grinder rapidly peels armor plates and disassembles internal blocks layer-by-layer.
   · 100% of disassembled components are drawn through the tool head conveyor vacuum into onboard storage.
   · Wrist roll spins at 2.0 rpm to widen deconstruction swath.

[REMOTE FIELD FABRICATION & INFRASTRUCTURE WELDING]
· Station 2 operator or [MAS] PB Excavator switches lasers to Mode 3: Fabrication Beam (Welder).
· Transforms the 35 m arm into a mobile construction gantry for remote chasm bridge building, retaining wall assembly, and fleet craft repair without crew EVA.

[AUTOMATED TRAVEL STOW & DEPLOY SEQUENCES]

· TRAVEL STOW SEQUENCE (`STOW`):
  1. Mining/salvage lasers deactivate; tool head retracts vertically clear of worksite.
  2. Turret Azimuth slews to 180.0° (Aft centerline).
  3. Shoulder, Elbow, and Wrist hinges align flat (0.0°).
  4. Boom lowers into armored dorsal spine trough; `MAS Clamp Boom Stow` at [X0, Z −7, L4] engages (Green Lock).
  5. Joint holding brakes clamp at 100% torque (50 MN·m Azimuth, 45 MN·m Hinges).
  6. Outriggers retract; consist is cleared for 35 km/h overland transit.

· WORK DEPLOY SEQUENCE (`DEPLOY`):
  1. Consist comes to full stop; handbrake engages.
  2. Outriggers extend and lock bedrock (verified by `MAS EC Stabilizers`).
  3. `MAS Clamp Boom Stow` releases.
  4. Shoulder elevates +15.0° to clear spine coamings.
  5. Slew to target waypoint azimuth and begin commanded macro.
```

#### Pre-Enclosure Inspection Gate 10.3A: Outrigger Locking & Boom Joint Inertia Verification
* [ ] Verify all 4 corner outrigger seismic feet (`MAS Seismic Outrigger 1..4`) lock firmly into terrain voxels.
* [ ] Confirm `MAS EC Seismic` triggers acoustic survey pulse upon 4/4 bedrock lock, broadcasting vein telemetry to `TMS.SCOUT.VEIN`.
* [ ] Confirm `Share Inertia Tensor = ON` across all boom joints (Turret Rotor, Shoulder Hinge, Elbow Hinge, Wrist Pitch Hinge, Wrist Roll Rotor).
* [ ] Test 360° pan and verify symmetrical 21.25 m physical reach beyond both Port and Starboard flanks without chassis tip.
* [ ] Verify `MAS Clamp Boom Stow` locks green at [X0, Z −7, L4] and rigidly secures the 35 m stowed boom with zero subgrid jitter.
* [ ] Test Macro 2 `BORE`: verify tool head maintains true vertical downward aim ([0, -1, 0]) while executing 2D lateral concentric sweeps around offset target waypoint.
* [ ] Verify `MAS Tool Grapple` locks onto external test block and holds suspended 50 t mass without joint sagging.
* [ ] Test Macro 4 `CRANE_RECOVERY`: verify crane mode enables grapple teleoperation and disables mining laser fire.

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
* 3× Control Station (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`, Palette 7: Fleet Command Navy)
* 2× Programmable Block (`[MAS] PB Excavator`, `[MAS] PB RIG//OS-Lite`, Palette 7)
* 3× AI Automaton Block (`MAS AI Move`, `MAS AI Task`, `MAS AI Defense`)
* 1× Laser Antenna (`MAS Comm Laser`, Palette 7)
* 1× Radio Antenna (`MAS Comm Radio`, 50 km broadcast, Palette 7)
* 1× Large Ore Detector (150 m subterranean range, Palette 4)
* 1× Survival Kit (Tractor Field Respawn, Palette 7)
* 14× Sloped Heavy Armor Glass (Bridge canopy)
* 6× LCD Panel (Tactical video wall and console telemetry HUDs)
* 1× Air Vent (`MAS Vent Bridge`, Hermetic Zone 1)
* 1× Large Warhead (`MAS Warhead Scuttle Bridge`, disarmed anti-capture charge)
* 1× Timer Block (`MAS Timer Scuttle`, 60-second scuttle loop)
* 2× Sci-Fi 4-Button Panel (`MAS BP Scuttle Bridge`, `MAS BP Airlock Vestibule`)
* 1× Large Gatling Turret (`MAS Turret Bow`, Palette 7: Fleet Command Navy)
* 2× Large Interior Turret (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`, Palette 7)

---

### Step 10.10 · 3-Operator Custom Walk-In Armored Bridge & Multi-Page Hotbars

Construct the forward command bridge overlooking the terrain. Instead of a cramped 3×3×2 enclosed cockpit prefab, the Mastodon utilizes a **Custom Walk-In Armored Bridge Cabin** (7 W × 6 L × 3 H B):
1. **Station 1 (Port): Helm Pilot (`MAS Helm`):** Open 1×1×1 Control Station at [X −2, Z +10, Y = L4]. Controls chassis traction, 3-axle steering, outrigger deploy/retract sequences, hill-climbing crawl, power modes, and worksite lights.
2. **Station 2 (Center): Excavation Master (`MAS Station Boom`):** Open 1×1×1 Control Station at [X0, Z +10, Y = L4]. Controls the 35 m articulated boom via Custom Turret Controller (CTC) mouse aim, tool head camera, and automated macro sweep execution.
3. **Station 3 (Starboard): Tactical Fleet Director (`MAS Station Fleet`):** Open 1×1×1 Control Station at [X +2, Z +10, Y = L4]. Runs `RIG//OS-Lite` fleet coordination, dispatching Caterpillar land trains, calling Mule shuttles, receiving Kestrel scout coordinates, and monitoring ore quotas.
4. **C2 Compute Rack (Level L3):** Houses dual Programmable Blocks (`[MAS] PB Excavator` and `[MAS] PB RIG//OS-Lite`) along with AI Automaton blocks for uncrewed autonomous operation.
5. **Dorsal Comms Mast (Level L6):** Houses the 40 km Laser Antenna (`MAS Comm Laser`) and 50 km Radio Antenna (`MAS Comm Radio`).

```
Plan view, Y = L4 (Custom Walk-In Bridge Deck & Tactical Video Wall)
        X: −3          X: −2           X0            X: +2          X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────┐
           │        45° SLOPED HEAVY ARMOR GLASS CANOPY (PANORAMIC)      │
    Z: +11 │   [STATION 1]             [STATION 2]           [STATION 3] │
           │   HELM / PILOT            BOOM EXCAVATOR        FLEET DIR   │
           │  (Control Station)       (Control Station)    (Control Stn) │
    Z: +10 │        │                      │                     │       │
           │   ◄────┴──────── OPEN WALKWAY AISLE (Palette 9) ────┴────►  │
    Z: +9  ├───────────────────────────┬─────────────┬───────────────────┤
           │ [SURVIVAL KIT]            │ [LCD WALL 1]│ [LCD WALL 2]      │
    Z: +8  │ Field Respawn / O2-H2     │ Consist Sys │ Tactical Fleet    │
           ├───────────────────────────┴──────┬──────┴───────────────────┤
    Z: +7  │ [AIRLOCK SLIDING DOOR]           │ [LADDER UP TO ROOF CROWN]│
           └──────────────────────────────────┴──────────────────────────┘
```

```
3-Operator Multi-Page Manual Hotbar Architecture:

================================================================================
STATION 1: HELM PILOT (`MAS Helm`)
================================================================================
[Hotbar 1 · Tractive Powertrain & Bedrock Anchoring]
· Slot 1: Group `MAS Outriggers` -> Reverse (Deploy / Retract)
· Slot 2: Group `MAS Outrigger Pads` -> Switch Lock (Bedrock 100% Voxel Lock)
· Slot 3: Handbrake -> Toggle
· Slot 4: Group `MAS Wheels Drive` -> Propulsion ON/OFF
· Slot 5: Group `MAS Wheels All` -> Increase Strength (+5%)
· Slot 6: Group `MAS Wheels All` -> Decrease Strength (-5%)
· Slot 7: Event Controller `MAS EC Level` -> Trigger Auto-Leveling
· Slot 8: Group `MAS Sorter Tailings` -> Power ON/OFF (Gravel Dump)
· Slot 9: Group `MAS Ejectors Tailings` -> Throw Out ON/OFF

[Hotbar 2 · Consist Dynamics & Power Management]
· Slot 1: Rotor `MAS Hitch Yaw` -> Increase Braking Torque (Anti-Jackknife Lock)
· Slot 2: Rotor `MAS Hitch Yaw` -> Decrease Braking Torque (Freewheel)
· Slot 3: Group `MAS Reactors` -> Power ON/OFF (Nuclear Scram)
· Slot 4: Generator `MAS Engine H2` -> Power ON/OFF (Emergency Cold-Start)
· Slot 5: Group `MAS Batteries Consist` -> Recharge ON/OFF (Shore Fast Charge)
· Slot 6: PB `[MAS] PB Excavator` -> Run `POWER MODE 1` (Silent Eco)
· Slot 7: PB `[MAS] PB Excavator` -> Run `POWER MODE 2` (Tractive Cruise)
· Slot 8: PB `[MAS] PB Excavator` -> Run `POWER MODE 3` (Heavy Excavate)
· Slot 9: PB `[MAS] PB Excavator` -> Run `POWER MODE 4` (Emergency Failover)

[Hotbar 3 · External Access & Worksite Illuminators]
· Slot 1: Hinge `MAS Crew Ladder Hinge` -> Reverse (Lower / Raise Boarding Ladder)
· Slot 2: Group `MAS Lights Worksite` -> Toggle ON/OFF (All Tower/Boom/Underbelly)
· Slot 3: Group `MAS Floodlight Tower` -> Toggle ON/OFF (360° Cardinal Ring)
· Slot 4: Group `MAS Lights Underbelly` -> Toggle ON/OFF (Chassis Maintenance Amber)
· Slot 5: Sliding Door `MAS Door Airlock` -> Open/Close
· Slot 6: Air Vent `MAS Vent Atmo Intake Port` -> Depressurize ON/OFF
· Slot 7: Air Vent `MAS Vent Atmo Intake Stbd` -> Depressurize ON/OFF
· Slot 8: Antenna `MAS Comm Radio` -> Broadcast ON/OFF
· Slot 9: Laser Antenna `MAS Comm Laser` -> Reset Connection

================================================================================
STATION 2: EXCAVATION MASTER (`MAS Station Boom`)
================================================================================
[Hotbar 1 · Direct Joint Teleoperation & Tool Aim]
· Slot 1: Custom Turret Controller `MAS CTC Boom` -> Control (Direct Mouse Aim)
· Slot 2: Group `MAS Lasers Tool` -> Shoot ON/OFF (Twin Mining / Salvage Lasers)
· Slot 3: Camera `MAS Cam Tool Head` -> View (Bore Sump Alignment)
· Slot 4: Rotor `MAS Turret Azimuth` -> Reverse (Manual Pan Left/Right)
· Slot 5: Hinge `MAS Shoulder Pitch` -> Reverse (Manual Boom Elevation)
· Slot 6: Hinge `MAS Elbow Pitch` -> Reverse (Manual Reach Extension)
· Slot 7: Hinge `MAS Wrist Pitch` -> Reverse (Manual Tool Pitch)
· Slot 8: Group `MAS Lasers Tool` -> Next Mode (Drill <-> Grinder/Salvage <-> Welder)
· Slot 9: Magnetic Plate `MAS Tool Grapple` -> Switch Lock (Crane Hoisting)

[Hotbar 2 · Automated Kinematic Macros & Presets]
· Slot 1: PB `[MAS] PB Excavator` -> Run `DEPLOY` (Release Clamp & Raise to Ready)
· Slot 2: PB `[MAS] PB Excavator` -> Run `STOW` (14-Block Dorsal Spine Clamp Lock)
· Slot 3: Magnetic Plate `MAS Clamp Boom Stow` -> Switch Lock (Manual Clamp Override)
· Slot 4: PB `[MAS] PB Excavator` -> Run `MACRO BENCH` (Automated Quarry Sweep)
· Slot 5: PB `[MAS] PB Excavator` -> Run `BORE` (Target-Centric Column Shaft)
· Slot 6: PB `[MAS] PB Excavator` -> Run `MACRO TRENCH` (Roadbed Carve)
· Slot 7: PB `[MAS] PB Excavator` -> Run `CRANE` (Vehicle Recovery Grapple)
· Slot 8: PB `[MAS] PB Excavator` -> Run `SALVAGE` (Macro 5 · Automated Wreck Deconstruction)
· Slot 9: Group `MAS Boom Joints` -> Rotor Lock ON/OFF (E-Stop Brake)

[Hotbar 3 · Worksite Illumination & Ore Buffer Telemetry]
· Slot 1: Group `MAS Light Boom` -> Toggle ON/OFF (Tool Head Spotlights)
· Slot 2: Camera `MAS Cam Boom Turret` -> View (Wide-Angle Turret Vantage)
· Slot 3: Sorter `MAS Sorter Tailings` -> Drain All ON/OFF
· Slot 4: Cargo Container `MAS Tractor Buffer` -> Show on HUD ON/OFF
· Slot 5: Cargo Container `MAS Hold Ore Cat` -> Show on HUD ON/OFF
· Slot 6: Event Controller `MAS EC Ore Full` -> Trigger Alert
· Slot 7: IGC Send: `TMS.MULE.CALL;MAS-01;HOLD_FULL`
· Slot 8: IGC Send: `TMS.CAT.CALL;MAS-01;HOLD_FULL`
· Slot 9: Group `MAS Drills Tool` -> Shoot ON/OFF (Backup Mechanical Drills)

================================================================================
STATION 3: TACTICAL FLEET DIRECTOR (`MAS Station Fleet`)
================================================================================
[Hotbar 1 · Predictive JIT Logistics & Offload Automation]
· Slot 1: PB `[MAS] PB RIG//OS-Lite` -> Run `JIT AIR` (Pre-Dispatch Mule Air Ferry)
· Slot 2: PB `[MAS] PB RIG//OS-Lite` -> Run `JIT GND` (Pre-Dispatch Caterpillar Train)
· Slot 3: Connector `MAS MP Conn` -> Lock/Unlock (Mule Roof Pad Connector)
· Slot 4: Group `MAS MP Clamps` -> Switch Lock (Mule Cradle Rub Rails)
· Slot 5: Sorter `MAS MP Sorter` -> Drain All ON/OFF (Rare Ore Fast Drain)
· Slot 6: Connector `MAS Stern Dump Conn` -> Lock/Unlock (Caterpillar Stern Rail)
· Slot 7: Sorter `MAS Stern Sorter` -> Drain All ON/OFF (Bulk Hopper Dump)
· Slot 8: Group `MAS Welder Pad Kestrel` -> Power ON/OFF (Drone Repair Welders)
· Slot 9: Event Controller `MAS EC Handover` -> Trigger C2 Base Handover

[Hotbar 2 · Slave Links, Recon Tasking & Swarm Deconfliction]
· Slot 1: Remote Control `MAS-02 Remote` -> Control (Subordinate Worker Excavator)
· Slot 2: Remote Control `CAT-01 Remote` -> Control (Caterpillar Locomotive)
· Slot 3: Remote Control `KES-01 Remote` -> Control (Kestrel Scout Drone)
· Slot 4: Remote Control `ALB-01 Remote` -> Control (Albatross Repair Drone)
· Slot 5: PB `[MAS] PB RIG//OS-Lite` -> Run `TASK KESTREL` (Vector Scout Survey Sector)
· Slot 6: PB `[MAS] PB Excavator` -> Run `SEISMIC` (Trigger 500 m Acoustic Voxel Survey)
· Slot 7: PB `[MAS] PB RIG//OS-Lite` -> Run `CLAIM ZONE` (Broadcast Quarry Exclusion Zone)
· Slot 8: LCD Panel `MAS LCD Video Wall 1` -> Cycle Page (Fleet Logistics Telemetry)
· Slot 9: LCD Panel `MAS LCD Video Wall 2` -> Cycle Page (Seismic Survey Map)

[Hotbar 3 · Perimeter CIWS Defense & Networked Fleet Targeting]
· Slot 1: Large Gatling Turret `MAS Turret Bow` -> Control (Direct CIWS Aim)
· Slot 2: Large Gatling Turret `MAS Turret Stern` -> Control (Stern CIWS Aim)
· Slot 3: Camera `MAS Cam Bow CIWS` -> View
· Slot 4: Camera `MAS Cam Stern CIWS` -> View
· Slot 5: Group `MAS Turrets Flank` -> Power ON/OFF (Anti-Personnel Skirt Guns)
· Slot 6: Group `MAS Turrets All` -> Target Hostiles ON/OFF
· Slot 7: PB `[MAS] PB RIG//OS-Lite` -> Run `TARGET NET` (Synchronize Fleet CIWS Interlock)
· Slot 8: PB `[MAS] PB RIG//OS-Lite` -> Run `ALERT CBT` (Lockdown / Combat Mode)
· Slot 9: PB `[MAS] PB RIG//OS-Lite` -> Run `ALERT NORM` (Clear Combat Alert)
```

---

### Step 10.10D · Bridge Anti-Capture Scuttle System & Countdown Loop (L3-L4)

To prevent advanced technology, refining assets, and encryption keys from falling into enemy hands if the vehicle is overrun:
* **Bridge Scuttle Charge (`MAS Warhead Scuttle Bridge`):** Mount 1× Large Grid Warhead beneath the C2 compute rack at [X0, Z +8, Y = L3]. Disarmed by default.
* **Scuttle Control Panel (`MAS BP Scuttle Bridge`):** Mount 1× Sci-Fi 4-Button Panel on the bridge aft bulkhead at [X +1, Z +8, Y = L4] (Button 1: Arm Scuttle System, Button 2: Start 60 s Countdown, Button 3: Broadcast Abandon Ship SOS, Button 4: Emergency Scuttle Abort).
* **Native Timer Countdown Loop (`MAS Timer Scuttle`):** Set countdown to 60 seconds.
  * T − 60 s: Triggers bridge siren, switches interior lights to flashing red, and broadcasts `MASTODON-01 SCUTTLE INITIATED`.
  * T − 5 s: Arms both `MAS Warhead Scuttle Bridge` and `MAS Warhead Scuttle Power`.
  * T − 0 s: Simultaneous detonation, destroying the avionics, reactors, and C2 core.
  * Pressing Button 4 before T − 5 s stops the timer and returns warheads to safety disarm.

---

### Step 10.10E · Physical In-Situ Interfaces (Sci-Fi 4-Button Panels)

To support foot operations without requiring cockpit entry, 6 wall-mounted Sci-Fi 4-Button Panels (1×1×1 B) provide physical local control:
* **`MAS BP Ground Boarding` (Port Skirt [X −4, Z +7, L1]):** Button 1: Ladder Lower/Raise · Button 2: Keel Lights · Button 3: Outer Airlock Door · Button 4: Beacon Toggle.
* **`MAS BP Airlock Vestibule` (Airlock [X −2, Z +7, L3]):** Button 1: Outer Door Cycle · Button 2: Inner Door Cycle · Button 3: Cabin Pressurize/Depress · Button 4: EVA Locker Access.
* **`MAS BP Power Vault` (Jefferies [X −2, Z −1, L3]):** Button 1: Vault Door Toggle · Button 2: Reactor SCRAM · Button 3: H2 Engine Cold-Start · Button 4: Battery Shore Charge.
* **`MAS BP Field Workshop` (Workshop [X0, Z −14, L3]):** Button 1: Assembler Repeat (Plates) · Button 2: Refinery Ore Flush · Button 3: O2/H2 Gen Toggle · Button 4: Workshop Lights.
* **`MAS BP Flight Deck` (Roof Hatch [X +2, Z −10, L6]):** Button 1: Mule Clamps Toggle · Button 2: Mule Connector Toggle · Button 3: Kestrel Welders Pulse · Button 4: Deck Floodlights.
* **`MAS BP Stern Dump` (Rear Platform [X +2, Z −25, L2]):** Button 1: Stern Connector Toggle · Button 2: Stern Bulk Sorter Drain All · Button 3: Cat Train Ping · Button 4: Rear Worksite Light.

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
* 1× Medical Bay + 2× Cryo Chamber (Palette 7: Fleet Command Navy)
* 2× Large Oxygen Tank (200,000 L O2, Palette 4: Processing Teal)
* 1× Small Cargo Container (`MAS Hold Ice LifeSupport`, 15,000 L Quarantined Ice, Palette 4)
* 2× Large Hydrogen Tank (Palette 6)
* 2× O2/H2 Generator (Palette 4)
* 4× Air Vent (`MAS Vent Workshop`, `MAS Vent Crawlway`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern`)
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 1× Large Projector (`MAS Projector Master`, Palette 7)
* 2× Sci-Fi 4-Button Panel (`MAS BP Flight Deck`, `MAS BP Stern Dump`)
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

### Step 10.12 · Mobile FOB Field Workshop, Medical Suite & Universal Life Support (L2-L4)

Outfit Carriage 2 as a self-sustaining expeditionary base centered along the Level L3 central aisle:
1. **Field Workshop Assembler (`MAS Utility Assembler`):** Large Assembler at [X −2, Z −14] configured to fabricate replacement plates, drill components, and motors directly in the field. Dual physical inventory panels face the central aisle for manual blueprint access.
2. **Medical & Crew Habitat Bay (`MAS Medical Bay`, `MAS Cryo Pod 1, 2`):** Pressurized crew habitat at [X +2, Z −12 to −14] with Medical Room, Survival Kit, and 2× Cryo Chambers plumbed directly to the life support oxygen line for long-distance transit hibernation.
3. **200,000 L Strategic Oxygen Tank Bank (`MAS Tank O2 1, 2`):** Mount 2× Large Grid Oxygen Tanks at [X −2, Z −16, Y = L3-L4]. Stores 200,000 L of pressurized breathing oxygen, enabling over 120 hours of vacuum surface operations.
4. **Quarantined Emergency Ice Reserve (`MAS Hold Ice LifeSupport`):** 1× Small Cargo Container (15,000 L capacity) at [X0, Z −16, L3] isolated by a one-way filter. Guarantees emergency ice reserves for life support even if bulk cargo holds are completely dumped.
5. **Atmospheric Tank Farm:** 2× Large Hydrogen Tanks (refuels Kestrel scouts and Mule ferries) + 2× O2/H2 Generators at [X ±2, Z −18]. Manual shutoff valves exposed in the corridor wall.
6. **Field Component Storage (`MAS Hold Components`):** 1× Large Cargo Container (3×3×3 B) dedicated to stocking construction parts at Z −22. Features a physical terminal port facing the aisle at [X0, Z −21, L3].

```
Plan view, Y = L3 (Utility Carriage Interior Deck with Closed-Loop Metallurgy & Life Support)
        X: −3          0          +3
    Z: −10 ═══════[DRAWBAR TONGUE]═══════  ◄── Articulated Hitch & Merge Block
           │                             │
    Z: −12 │ [REFINERY]  │ [CRYO POD 1]  │  ◄── Basic Refinery & 2-Crew Cryo Hibernation
           ├─────────────┤ [CRYO POD 2]  │
    Z: −14 │ [ASSEMBLER] │ [MEDICAL BAY] │  ◄── Large Assembler & Medical Trauma Center
           ├─────────────┼───────────────┤
    Z: −16 │ [O2 TANK 1] │ [O2 TANK 2]   │  ◄── 200,000 L Oxygen Bank & Quarantined Ice
           ├─────────────┼───────────────┤
    Z: −18 │ [H2 TANK 1] │ [H2 TANK 2]   │  ◄── Fuel Bunkering Farm (10,000,000 L H2)
           ├─────────────┴───────────────┤
    Z: −22 │ [MAS HOLD COMPONENTS (3×3×3)│  ◄── Spare Parts Storage (Palette 5)
    Z: −26 └─────────────────────────────┘
```

---

### Step 10.12B · Closed-Loop In-Situ Field Metallurgy & Component Recycling Circuit (Decision 65)

In Space Engineers, an Assembler cannot convert raw ore directly into components; it requires refined ingots. Install 1 Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW, 13.5 t) adjacent to `MAS Utility Assembler` at [X −2, Y = L3, Z −12]:
* **Conveyor Routing:** Directly plumbed to the central conveyor spine. Ingests raw ore (Fe, Ni, Co, Si, Mg) delivered from `MAS Tractor Buffer` via the articulated drawbar pipe.
* **Closed-Loop Ore Smelting:** Outputs refined ingots directly into `MAS Utility Assembler` and `MAS Hold Components`, enabling complete autonomous field fabrication of replacement parts (steel plates, construction components, motors, bulletproof glass) without returning to base.
* **Manual Hopper Port:** Top maintenance door accessible from the central aisle allows manual dumping of rare ore batches.
* **Ice Automation:** Event Controller `MAS EC Ice` monitors raw ice levels, automatically prioritizing ice routing to the twin O2/H2 generators to replenish the 10,000,000 L hydrogen reserve.

#### Closed-Loop Salvage & Component Recycling Circuit:
When `MAS Laser 1, 2` deconstruct target grids via Macro 5 (`MACRO_SALVAGE`), the FOB workshop operates as a closed-loop parts reclamation facility:
1. **Intact Component Intake:** Undamaged functional components (Large Steel Tubes, Motors, Computers, Thruster Components, Metal Grids) route through the drawbar pipe into `MAS Hold Components` (3×3×3 Large Cargo, Palette 5) to replenish fleet repair inventories.
2. **Scrap Metal Smelting:** Damaged scrap metal recovered during deconstruction is routed to `MAS Field Refinery`, melting down at 330 kW into pure Iron Ingots.
3. **Assembler Disassembly (Reverse Metallurgy):** Surplus or obsolete salvaged parts are routed into `MAS Utility Assembler` configured in Disassemble Mode, breaking them down into constituent ingots (Fe, Ni, Co, Si, Pt, Au) to feed immediate NATO 25mm ammo fabrication or repair plates.

```
[SALVAGE EXTRACTION & CLOSED-LOOP RECYCLING CIRCUIT]

   [PIRATE WRECK / TARGET GRID]
         │
         ▼ (Laser Deconstruction Beam · Mode: Grinder)
   [MAS LASER 1, 2]
         │
         ▼ (Forearm & Turntable Conveyor Conduit)
   [CARRIAGE 1 CENTRAL CONVEYOR SPINE]
         │
         ▼ (Flexible Articulated Drawbar Pipe)
   ┌────────────────────────────────────────────────────────┐
   │ CARRIAGE 2 FOB WORKSHOP CORE                           │
   │                                                        │
   │  ┌──────────────────┐   ┌───────────────────────────┐  │
   │  │ INTACT COMPONENTS│──►│ MAS HOLD COMPONENTS (3×3) │  │
   │  └──────────────────┘   └─────────────┬─────────────┘  │
   │                                       │ (Parts Pull)   │
   │  ┌──────────────────┐                 ▼                │
   │  │ DAMAGED SCRAP    │──►[MAS FIELD REFINERY] (Ingots)  │
   │  └──────────────────┘                 │                │
   │                                       ▼                │
   │  ┌──────────────────┐   [MAS UTILITY ASSEMBLER]        │
   │  │ SURPLUS BLOCKS   │──►(Disassembly or Ammo Fab)      │
   │  └──────────────────┘                                  │
   └────────────────────────────────────────────────────────┘
```

---

### Step 10.12C · Vertical Flight Ladderwell & Stern Maintenance Descent (L3 to L6 & L3 to L2)

To allow physical passage from the workshop to the roof flight decks and stern train dump bumper:
* **Flight Deck Airlock & Ladderwell (L3 → L6):** Install an enclosed vertical ladder at [X +2, Z −10] climbing from the forward workshop corner up through an armored hatch to the Level L6 Flight Deck. Equipped with `MAS Vent Airlock Roof` and button panel `MAS BP Flight Deck` for active vacuum gas reclamation.
* **Stern Dump Airlock & Descent (L3 → L2):** Install a descending ladderwell at [X +2, Z −25] passing through an armored hatch down to Level L2. Allows an engineer on foot to physically inspect and service `MAS Stern Dump Conn`, `MAS Stern Sorter`, and rear guide wedges without venting vehicle atmosphere.

---

### Step 10.12D · Ambient Atmospheric Oxygen Scavenging Vents (Carriage 2 Flank L4)

To achieve indefinite planetary life support without consuming raw ice or running energy-intensive O2/H2 generators:
* **Hardware Integration:** Mount 2× Air Vents (`MAS Vent Atmo Intake Port` at [X −3, Z −16, Y = L4] and `MAS Vent Atmo Intake Stbd` at [X +3, Z −16, Y = L4]) recessed flush into the upper exterior flank armor.
* **Operating Configuration:** Set `Depressurize = ON`. In breathable planetary atmospheres, the vents pull ambient oxygen directly from outside air into the central life support trunk.
* **Closed-Loop Storage:** Fills internal oxygen tanks and pressurized crew compartments automatically at zero ice consumption and minimal parasitic power (30 kW per vent), preserving onboard ice stocks strictly for hydrogen fuel synthesis.

---

### Step 10.12E · Genesis Master Projector & Multi-Zone Environmental Control (L3-L4)

To ensure field self-repair and complete multi-planetary survival:
* **Master Projector Self-Repair Core (`MAS Projector Master`):** Mount 1× Large Grid Projector inside the workshop ceiling at [X0, Z −14, Y = L4]. Pre-loaded with the complete 1,039-block blueprint of `MASTODON-01 MKI`, perfectly aligned to the physical chassis grid. When damaged, the projected blueprint allows repair drones (`ALBATROSS-01`) or crew to rebuild damaged blocks using parts from `MAS Hold Components`.
* **Universal Multi-Atmosphere Environmental Control:** Event Controller `MAS EC Atmosphere Mode` manages atmospheric adaptation:
  * On breathable worlds (Earthlike): Exterior vents set to `Depressurize = ON` to harvest free oxygen.
  * On airless moons (Moon, Europa) or toxic worlds (Mars, Alien): Exterior vents set to `Power = OFF`. Internal life support switches to closed-loop mode, feeding 100% O2 from `MAS Tank O2 1, 2` into all 4 sealed zones while airlocks cycle with zero-gas venting.

#### Pre-Enclosure Inspection Gate 10.5A: Field Workshop Fabrication, Sub-Floor Batteries & Circulation Check
* [ ] Confirm Utility sub-floor battery bank (`MAS Battery 4, 5, 6`) delivers 9.0 MWh storage and 36.0 MW discharge.
* [ ] Verify `MAS Field Refinery` ingests raw mined ore and deposits refined ingots into `MAS Utility Assembler`.
* [ ] Verify `MAS Utility Assembler` pulls ingots and queues components without manual transfer.
* [ ] Confirm physical cargo access ports on Basic Refinery, Assembler, and Component Hold allow manual inventory access.
* [ ] Confirm `MAS Tank O2 1, 2` store 200,000 L O2 and maintain 100% pressure in all 4 hermetic zones.
* [ ] Verify active gas-reclamation airlock cycle recovers 100% of room O2 before outer doors open to vacuum.
* [ ] Confirm `MAS Projector Master` projects aligned ghost blueprint of Mastodon consist for field self-repair.
* [ ] Confirm `MAS Hold Ice LifeSupport` remains quarantined from bulk ore dumps.
* [ ] Verify vertical flight ladderwell provides unobstructed ascent to Level L6 Flight Deck.
* [ ] Confirm stern ladder provides safe descent to Level L2 dump bumper.
* [ ] Confirm crew medical bay and twin cryo pods allow successful respawn, healing, and hibernation.
* [ ] Confirm O2/H2 tanks maintain airtight seals and supply hydrogen lines.
* [ ] Verify `MAS EC Ice` automatically feeds ice to O2/H2 generators when H2 tanks drop below 90%.
* [ ] Verify ambient air vents (`MAS Vent Atmo Intake Port/Stbd`) pull oxygen from atmosphere when Depressurize = ON.

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
* 2× Large Grid Ship Welder (`MAS Welder Pad Kestrel 1, 2`, Palette 4: Processing Teal)
* 4× Spotlight / Floodlight (`MAS Floodlight Tower 1..4`, Palette 7: Fleet Command Navy)
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

### Step 10.14B · Flight Deck Recessed Drone Maintenance Welders (Carriage 2 L5)

To maintain drone fleet operational readiness during extended deep-wilderness prospecting campaigns:
* **Recessed Welder Arrays:** Mount 2× Large Grid Ship Welders (`MAS Welder Pad Kestrel 1` at [X −3, Z −12, Y = L4] and `MAS Welder Pad Kestrel 2` at [X +3, Z −12, Y = L4]) positioned directly flush beneath the small-grid docking collars of the twin Kestrel landing pads.
* **Plumbed Logistics:** Connected directly to the utility workshop conveyor trunk, drawing steel plates, construction components, small tubes, and computer chips from `MAS Hold Components` and `MAS Utility Assembler`.
* **Automated Repair Cycle:** Event Controller `MAS EC Drone Repair` detects when a Kestrel dock reports `Connected = TRUE`. It pulses the respective pad welder for 5.0 seconds, automatically repairing atmospheric thruster burnout, micro-meteoroid hull pits, and antenna damage sustained during prospecting sorties before returning to standby.

---

### Step 10.14C · Worksite 360° Floodlight Matrix (Tractor Bridge Crown & Dorsal Comms Mast L5-L6)

To illuminate hazardous night operations, quarry excavation faces, and fleet flight decks:
* **Tower Floodlight Array:** Mount 4× Spotlights (`MAS Floodlight Tower 1..4`, Palette 7: Fleet Command Navy) in a high-elevation 360° cardinal ring on the Tractor dorsal bridge crown and comms mast at Level L5-L6 facing Bow (+Z), Stern (−Z), Port (−X), and Starboard (+X).
* **Illumination Coverage:** Throws high-intensity illumination across a 100 m radius, blanketing the excavation pit, the Mule receiving cradle, and both Kestrel pads.
* **Integrated Control:** Bound to group `MAS Lights` alongside the boom spotlights (`MAS Light Boom 1, 2`) and underbelly maintenance lamps (`MAS Lights Underbelly`), allowing single-hotkey toggling from Station 1 Helm.

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
* [ ] Verify recessed drone welders (`MAS Welder Pad Kestrel 1, 2`) auto-repair docked Kestrel drones upon touchdown.
* [ ] Confirm worksite floodlight matrix (`MAS Floodlight Tower 1..4`) casts 360° illumination across mining zone and flight decks.

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
* `MAS Outriggers`: 4× Hydraulic stabilizer pistons + magnetic plates / seismic feet.
* `MAS Boom Joints`: 5× Rotors and Hinges articulating the long-reach arm.
* `MAS Lasers`: 2× Simple Laser Multitools on the tool head.
* `MAS Welders`: 2× Flush Large Grid Ship Welders (`MAS Welder Pad Kestrel 1, 2`) on Kestrel flight pads.
* `MAS Lights`: Worksite floodlights (`MAS Floodlight Tower 1..4`), boom spotlights (`MAS Light Boom 1, 2`), and underbelly lamps (`MAS Lights Underbelly`).
* `MAS Seismic`: 4× Seismic Surveying Landing Gear units (`MAS Seismic Outrigger 1..4`).
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
| Control Station | `MAS Helm` | Station 1 (Port) · Pilot traction, steering, outrigger, power control |
| Control Station | `MAS Station Boom` | Station 2 (Center) · Excavation Master · CTC mouse aim & macros |
| Control Station | `MAS Station Fleet` | Station 3 (Stbd) · Tactical Fleet Director · Drone dispatch & C2 |
| Laser Antenna | `MAS Comm Laser` | 40 km point-to-point line-of-sight data link (L6 mast) |
| Radio Antenna | `MAS Comm Radio` | 50 km omnidirectional fleet broadcast & heartbeat beacon |
| Radio Antenna | `MAS Beacon Distress` | 50 km blackout SOS distress beacon in sub-bridge keel |
| Battery (Small) | `MAS Battery BlackBox` | Isolated cold-standby power cell for distress beacon |
| Merge Block | `MAS Hitch Sever Tongue` | Emergency pyrotechnic consist decoupling at [X0, Z −8, L2] |
| Decoy | `MAS Decoy Flank Port/Stbd`, `MAS Decoy Stern` | Lower armor skirt ballistic decoys diverting turret fire |
| Gyroscope (Large) | `MAS Gyro Recovery 1, 2` | Chassis roll override self-righting gyros at [X0, Z0, L1] |
| Projector (Large) | `MAS Projector Master` | Complete consist blueprint self-repair projection core at [X0, Z −14, L4] |
| Warhead (Large) | `MAS Warhead Scuttle Bridge`, `MAS Warhead Scuttle Power` | Anti-capture technology denial scuttle charges (L3) |
| Timer Block | `MAS Timer Scuttle` | 60-second native scuttle countdown loop with manual abort |
| Oxygen Tank (Large) | `MAS Tank O2 1, 2` | 200,000 L high-pressure closed-loop breathing reserve |
| Small Cargo Container | `MAS Hold Ice LifeSupport` | 15,000 L quarantined emergency life support ice buffer |
| Cryo Chamber | `MAS Cryo Pod 1, 2` | Medical bay crew hibernation pods (zero food/suit drain) |
| Air Vent (Large) | `MAS Vent Bridge`, `MAS Vent Workshop`, `MAS Vent Crawlway` | Multi-zone pressurized cabin air vents (Zones 1, 2, 4) |
| Air Vent (Large) | `MAS Vent Airlock Port`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern` | Active gas-reclamation decompression airlock vents |
| Button Panel | `MAS BP Ground Boarding`, `MAS BP Airlock Vestibule`, `MAS BP Power Vault` | Physical in-situ crew control panels (Tractor L1-L3) |
| Button Panel | `MAS BP Scuttle Bridge`, `MAS BP Field Workshop`, `MAS BP Flight Deck`, `MAS BP Stern Dump` | Physical in-situ crew control panels (Bridge L4, FOB L2-L6) |
| Gatling Turret | `MAS Turret Bow` | Carriage 1 dorsal crown CIWS · 360° hemispherical anti-air/ground |
| Gatling Turret | `MAS Turret Stern` | Carriage 2 stern gantry CIWS · 270° rear hemispherical coverage |
| Interior Turret | `MAS Turret Flank Port` | Port lower skirt L2 · Anti-personnel / anti-spider perimeter guard |
| Interior Turret | `MAS Turret Flank Stbd` | Starboard lower skirt L2 · Anti-personnel / anti-spider perimeter guard |
| AI Move Block | `MAS AI Move` | Autonomous pathfinding & terrain navigation (uncrewed role) |
| AI Task Block | `MAS AI Task` | Autonomous waypoint execution & ore vein transit |
| AI Defense Block | `MAS AI Defense` | Threat avoidance · Auto-locks outriggers and stows boom |
| Piston (Large) | `MAS Outrigger Piston 1..4` | Corner bedrock stabilizer cylinders · `Share Inertia Tensor = ON` |
| Magnetic Plate | `MAS Outrigger Pad 1..4` | Corner bedrock voxel locking pads · `Auto-Lock = ON` |
| Seismic Landing Gear | `MAS Seismic Outrigger 1..4` | Deep voxel acoustic prospecting feet (TMS Mod 2819263781) |
| Magnetic Plate (Large) | `MAS Tool Grapple` | Centerline boom recovery grapple · Vehicle righting & cargo hoisting |
| Magnetic Plate (Large) | `MAS Clamp Boom Stow` | Chassis dorsal spine travel stow clamp at [X0, Z −7, L4] |
| Ship Welder (Large) | `MAS Welder Pad Kestrel 1, 2` | Recessed flight deck drone maintenance welders |
| Spotlight (Large) | `MAS Floodlight Tower 1..4` | 360° dorsal worksite illumination floodlights (L5-L6) |
| Interior Light (Large) | `MAS Lights Underbelly` | Underbelly chassis maintenance and inspection lighting (L1) |
| Small Cargo Container | `MAS Locker Port`, `MAS Locker Stbd` | Ground-level EVA survival supply lockers (skirt L2) |
| Air Vent (Large) | `MAS Vent Atmo Intake Port`, `MAS Vent Atmo Intake Stbd` | Ambient planetary atmospheric O2 intake vents (`Depressurize = ON`) |
| Motor Hinge | `MAS Crew Ladder Hinge` | Port flank boarding ladder articulator · 0° stowed to 90° down |
| Sensor | `MAS Sensor Ladder` | 2.5 m crew proximity sensor on Port flank (auto-deploy) |
| Advanced Rotor | `MAS Turret Azimuth` | Dorsal boom turntable · 360° pan · `Torque = 50 MN·m` |
| Motor Hinge | `MAS Shoulder Pitch` | Boom elevation joint · −20° to +75° · `Torque = 45 MN·m` |
| Motor Hinge | `MAS Elbow Pitch` | Boom mid-joint · −100° to +45° · `Torque = 45 MN·m` |
| Advanced Rotor | `MAS Wrist Roll` | Tool head roll alignment · ±180° · `Torque = 20 MN·m` |
| Motor Hinge | `MAS Wrist Pitch` | 90° tool elbow pitch leveling & vertical down lock · −60° to +60° · `Torque = 20 MN·m` |
| Laser Multitool | `MAS Laser 1, 2` | Tri-mode Simple Laser Multitools (ToolCore) · Dynamic switching: Mining (Drill) / Salvage (Grinder, 15 to 30 m standoff) / Fabrication (Welder, 35 m reach) |
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
| Event Controller | `MAS EC Seismic` | Bedrock survey watchdog: pulses seismic scans on outrigger lock |
| Event Controller | `MAS EC Drone Repair` | Drone repair watchdog: pulses pad welders when Kestrel docked |
| Event Controller | `MAS EC LifeSupport` | O2 tank reserve & hull breach pressure watchdog (auto-seals doors) |
| Event Controller | `MAS EC BlackBox` | Power blackout watchdog: auto-triggers 50 km SOS distress beacon |
| Event Controller | `MAS EC Atmosphere Mode` | Planetary atmo watchdog: auto-toggles ambient scavenging vs vacuum closed-loop |

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
| `MAS EC Seismic` | `MAS Seismic Outrigger 1..4` | All 4 Pads = Locked | Pulse Seismic Scan · Publish `TMS.SCOUT.VEIN` | Standby |
| `MAS EC Drone Repair` | `MAS Kestrel Dock 1, 2` | Either Pad = Connected | Pulse `MAS Welders` ON (5.0 s timer) | `MAS Welders` OFF |
| `MAS EC LifeSupport` | `MAS Tank O2 1, 2` / Pressure | O2 $< 25\%$ OR Pressure $< 0.5$ atm | Sound O2 Alarm · Seal Compartment Doors | Normal Atmospheric Circulation |
| `MAS EC BlackBox` | Consist Power Output | Total MW Output $= 0.0$ MW | Activate `MAS Battery BlackBox` & SOS Beacon | Standby Isolation |
| `MAS EC Atmosphere Mode` | Ambient Atmospheric O2 | Ambient O2 $\ge 100\%$ | `MAS Vent Atmo Intake` Depressurize ON | `MAS Vent Atmo Intake` Power OFF |

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
* [ ] **Travel Stow Clamp Lock (STOW/DEPLOY):** Trigger `STOW` command. Confirm turntable slews to 180° aft, shoulder/elbow/wrist hinges fold to 0° flat along dorsal spine, and tool head locks into `MAS Clamp Boom Stow` at `[X0, Z −7, L4]`. Confirm 5.2 m transit profile and zero bridge obstruction. Trigger `DEPLOY`; confirm clamp releases and boom elevates to ready position.
* [ ] **Symmetrical 21.25 m Flank Reach:** Slew boom to Port (90°) and Starboard (270°). Confirm boom achieves identical 21.25 m physical reach beyond outer tires on both flanks with zero hull collision.
* [ ] **Target-Centric Vertical Column Bore:** Trigger Macro 2 (`BORE <GPS>`). Confirm 90° downward tool adapter locks drills into true vertical orientation [0, -1, 0] across full reach envelope (6.0 m to 35.0 m) while boom coordinates lateral translation and plunge around target center.
* [ ] **Bench Quarry Macro Test:** Trigger `MAS Timer Bench`. Verify boom executes automated -45° to +45° horizontal sweeps with -2.5° incremental shoulder steps and flat floor leveling.
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

### I · Field Logistics, Crane Grapple, Seismic Voxel Scan & Floodlight Acceptance
* [ ] **Deep Seismic Subsurface Scan (Decision 62):** Deploy outriggers to bedrock lock on hillside. Confirm Event Controller `MAS EC Seismic` triggers acoustic resonance scan down to 500 m† depth, detecting deep ore deposits and transmitting coordinates to `TMS.SCOUT.VEIN`.
* [ ] **45 m Crane Grapple & Vehicle Righting (Decision 62):** Select Macro 4 `CRANE_RECOVERY`. Maneuver boom via CTC mouse control to attach `MAS Tool Grapple` to overturned target vehicle. Confirm 45 MN·m joint torque successfully rights vehicle without chassis flex.
* [ ] **Automated Flight Deck Drone Welder Repair (Decision 62):** Dock battle-damaged or thruster-worn Kestrel drone onto `MAS Kestrel Dock 1`. Confirm `MAS EC Drone Repair` pulses recessed ship welders for 5.0 s, completing full field repairs from onboard parts hold.
* [ ] **Atmospheric O2 Scavenging & Ground EVA Lockers (Decision 62):** In planetary atmosphere, verify `MAS Vent Atmo Intake Port/Stbd` fill life support tanks at zero ice cost. Walk to ground level L2; open `MAS Locker Port/Stbd` to access hydrogen suit bottles and emergency tools without entering airlock.
* [ ] **Worksite 360° Floodlight Matrix (Decision 62):** Toggle group `MAS Lights`. Confirm tower spotlights (`MAS Floodlight Tower 1..4`), boom spotlights (`MAS Light Boom 1, 2`), and underbelly inspection lights (`MAS Lights Underbelly`) fully illuminate 100 m perimeter during nighttime mining operations.

### J · Custom Bridge, Universal Life Support & Emergency Failsafes Acceptance (Decision 64)
* [ ] **Custom Walk-In Bridge Circulation:** Walk from aft airlock door through open aisle to all 3 Control Stations (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`). Confirm unobstructed panoramic view through 45° sloped armor glass canopy and walk-around access to Survival Kit and Tactical Video Wall.
* [ ] **Multi-Page Hotbar Verification:** Test Hotbars 1, 2, and 3 on all three operator stations. Confirm Station 1 controls traction/leveling (HB1), power modes/reactors (HB2), and boarding/lights (HB3). Confirm Station 2 controls boom joints/lasers (HB1), automated macros (HB2), and buffers/ejectors (HB3). Confirm Station 3 controls logistics calls (HB1), drone slave links (HB2), and CIWS defense (HB3).
* [ ] **Universal Life Support (ULSS) Pressure & Isolation:** Depressurize one compartment (simulate breach); confirm `MAS EC LifeSupport` auto-seals bounding armored sliding doors in < 0.5 s, preserving 100% O2 in surviving zones. Confirm 200,000 L O2 tanks maintain life support during simulated vacuum transit.
* [ ] **Active Gas-Reclamation Airlock Cycling:** Test Port boarding, roof flight deck, and stern dump airlocks. Verify `Depressurize = ON` reclaims 100% of room air into O2 tanks before outer doors open, preventing explosive decompression in vacuum or toxic environments.
* [ ] **Cryo Hibernation Verification:** Enter `MAS Cryo Pod 1` and `MAS Cryo Pod 2` in Medical Bay. Confirm direct conveyor link supplies O2 and suspends suit life support drain.
* [ ] **Quarantined Emergency Ice Buffer:** Mine 20 t raw ice. Confirm ice diverts into `MAS Hold Ice LifeSupport` (15,000 L) and is preserved even when main ore buffers are dumped into the Caterpillar land train.
* [ ] **Pyrotechnic Hitch Severance Test:** Trigger `Hitch Decouple`. Confirm merge block `MAS Hitch Sever Tongue` disengages, severing Carriage 1 from Carriage 2 cleanly. Confirm Tractor drives away as an independent 6×6 unit and Utility Carriage remains powered and airtight as an autonomous FOB.
* [ ] **Self-Righting Keel Gyro Override:** Roll consist onto its side; trigger `MAS Gyro Recovery 1, 2` roll override. Confirm angular torque successfully rights the vehicle back onto its wheels.
* [ ] **Black Box SOS Beacon Blackout Test:** Disconnect all main reactors and batteries. Confirm `MAS EC BlackBox` instantly wakes isolated `MAS Battery BlackBox`, broadcasting 50 km emergency distress ping `SOS-MASTODON-01-LOST`.
* [ ] **Genesis Self-Repair Projector Alignment:** Activate `MAS Projector Master`. Confirm 1,039-block ghost blueprint aligns with 0.0 offset to the physical chassis, allowing repair welders to rebuild damaged blocks directly from `MAS Hold Components`.
* [ ] **Anti-Capture Scuttle Countdown & Manual Abort:** Trigger Button 2 on `MAS BP Scuttle Bridge`. Confirm `MAS Timer Scuttle` initiates 60 s countdown, sirens sound, and interior lights flash red. Press Button 4 before T - 5 s; confirm countdown aborts and warheads return to safety disarm.

### K · Tri-Mode Laser Multitool Salvage & Remote Welding Acceptance (Decision 65)
* [ ] **Tri-Mode Operational Cycle:** Cycle `MAS Laser 1, 2` via Station 2 Hotbar 1 Slot 8. Confirm HUD feedback confirms mode switching across `Mining Beam (Drill)` -> `Salvage Beam (Grinder)` -> `Fabrication Beam (Welder)` -> `Mining Beam`.
* [ ] **Macro 5 Automated Planar Salvage Sweep:** Position boom facing a test hull target. Trigger Macro 5 `SALVAGE` via Station 2 Hotbar 2 Slot 8. Confirm `[MAS] PB Excavator` positions tool head at 15 to 30 m standoff, maintains optimal perpendicular beam alignment, and executes 2D serpentine passes (ΔY, ΔX) peeling armor plates layer-by-layer without physical contact or collision jitter.
* [ ] **100% Component Retention Verification:** Deconstruct functional target block (e.g. Battery, Large Hydrogen Thruster). Verify 100% of disassembled components (Power Cells, Thruster Components) are drawn through tool head conveyor vacuum into `MAS Hold Components` with zero floating debris ejection.
* [ ] **Reverse Metallurgy Disassembly Loop:** Queue disassembly of 100 Interior Plates in `MAS Utility Assembler`. Confirm surplus plates are reclaimed into constituent Iron Ingots and transferred into manufacturing stock.
* [ ] **Scrap Metal Smelting Acceptance:** Route 500 kg Scrap Metal into `MAS Field Refinery`. Confirm refinery consumes 330 kW, melting scrap into pure Iron Ingots to feed ongoing component fabrication.
* [ ] **35 m Remote Field Fabrication & Structural Welding:** Switch lasers to Mode 3: Fabrication Beam (Welder). Aim boom at projected bridge block girder 30 m across test chasm. Verify remote beam welds block to 100% completion without requiring engineer EVA in hazardous conditions.

### L · Multi-Tier Fleet Comms, Predictive JIT Logistics & Swarm Synergy Acceptance (Decision 66)
* [ ] **Predictive JIT Transport Pre-Dispatch:** Mine ore continuously at high rate. Confirm `[MAS] PB Excavator` computes real-time extraction rate (kg/s) and remaining fill time (T_remain). At 75-80% buffer threshold, confirm `RIG//OS-Lite` automatically broadcasts `TMS.LOGISTICS.JIT`, pre-dispatching Mule or Caterpillar so transport touches down precisely as buffers reach 95%, achieving zero-downtime hot extraction.
* [ ] **Dynamic Logistics Triage Separation:** Load mixed batch containing Uranium and Stone. Confirm rare ores route to `MAS MP Sorter` (roof Mule pad) while stone aggregate routes to `MAS Stern Sorter` (Caterpillar rail dump).
* [ ] **Two-Tier Prospecting & Acoustic Voxel Scan:** Trigger `TASK KESTREL` via Station 3 Hotbar 2 Slot 5. Verify Kestrel flies grid survey, identifies surface anomaly, and transmits `TMS.SCOUT.VEIN`. Move Mastodon to waypoint; trigger `SEISMIC` (HB2:6); confirm 4× outriggers execute 500 m deep acoustic pulse and transmit 3D vein centroid (`TMS.SEISMIC.SURVEY`) directly into Macro 2 (`BORE`).
* [ ] **Aerial IGC Line-of-Sight Relay Mesh:** Drive Mastodon into deep crater breaking direct laser line-of-sight to Ground Base. Confirm docked Kestrel launches to 500 m AGL, switches to Relay Mode, and maintains seamless bidirectional IGC packet delivery between Mastodon and Ground Base C2.
* [ ] **Demand-Driven Pull Mining Protocol:** Simulate Ground Base broadcasting `TMS.BASE.DEMAND;COBALT;5;150000;250000`. Confirm Mastodon C2 automatically filters local survey database for nearest Cobalt vein and highlights target waypoint on Station 3 Tactical Video Wall.
* [ ] **Multi-Consist Swarm Deconfliction:** Simulate sister excavator `MAS-02` within 200 m radius. Trigger `CLAIM ZONE` (HB2:7); confirm `TMS.ZONE.CLAIM` broadcasts bounding box exclusion zone, preventing boom joint interference during synchronized quarrying.
* [ ] **Coordinated CIWS Fleet Defense Interlock:** Simulate incoming hostile pirate drone. Confirm `[MAS] PB RIG//OS-Lite` broadcasts `TMS.DEFENSE.TARGET` with threat priority, distributing target tracking between `MAS-01` and `MAS-02` turrets to prevent ammunition waste.





