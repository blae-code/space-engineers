# Mastodon · Visual Build Instructions

## Chapter 10 · MASTODON-01 (MKI) Expeditionary Mining Locomotive & Mobile FOB

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MASTODON-01 MKI**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67 & 68)
* **Role:** Long-range terrestrial expeditionary mining locomotive, self-contained mobile Forward Operating Base (FOB), and **Genesis Seed Fleet Mothership** running **`RIG//OS-Lite`**. In fresh survival campaigns prior to Ground Base refinery commissioning, the lead Mastodon serves as the regional C2 fleet root, commanding subordinate excavators (`MAS-02`), bulk land trains (`CAT-01`), and freight shuttles (`MUL-01`), with automated handover to Ground Base upon commissioning.
* **Architecture:** Segmented articulated train consist:
  1. **Carriage 1 (`MAS-01-T`): Locomotive & Excavator Unit (11 W × 20 L × 5 H B · 6 Wheels):** Heavy tractive engine, 3-operator armored panoramic C2 bridge (Helm, Boom CTC, Tactical Director), Tri-Fuel primary power plant (30 MW nuclear + 5 MW emergency H2 engine + 9 MWh battery reserve across 3× Large Batteries), dual-PB compute rack (`PB Excavator` + `PB RIG//OS-Lite`), 40 km laser comms mast, 4× hydraulic corner outriggers with differential auto-leveling and **deep seismic voxel sensors** (Mod 2819263781), dorsal 35 m spine-stowed articulated 5-DOF laser mining boom with **heavy magnetic crane grapple**, chassis travel stow clamp (`MAS Clamp Boom Stow`), bow CIWS Gatling Turret (`MAS Turret Bow`), forward Multitool Repair Turret (`MAS MT Turret Bow`), flank anti-personnel turrets, Port L3 maintenance Jefferies tube, ground-level EVA survival lockers, and aft 3-axis heavy gimbal drawbar hitch.
  2. **Carriage 2 (`MAS-01-U`): Modular FOB & Utility Carriage (11 W × 18 L × 5 H B · 4 Wheels):** Field workshop (1× Basic Refinery for in-situ metallurgy, 1× Large Assembler, Survival Kit / Medical Bay), standalone FOB compute core (`[MAS-U] PB Utility` + `MAS-U Remote FOB`), DPU smart wheel traction and dynamic regenerative descent braking, runaway breakaway deadman brake (`MAS-U EC Breakaway` + 2× keel magnetic skids), 40× Heavy Armor sub-floor ballast skids, free planetary atmospheric O2 intake vents, sub-floor battery vault (9 MWh across 3× Large Batteries), 4× roof flush solar panels (`≈ 640 kW` peak†), component reserve hold, atmospheric bunkering (2× Large H2 tanks, O2/H2 generators), 7 × 7 B MULE-01 Gantry Cradle Pad with motorized clamping arms, elevated stern Multitool Repair Turret (`MAS MT Turret Stern` on L7 gantry arch), twin flank Multitool Repair Turrets (`MAS MT Turret Port/Stbd` on L4 forward shoulders), dedicated outbound resupply export hopper (`MAS Hold Resupply Export`), 2× KESTREL-01 scout drone landing pads with **recessed automated repair welders**, roof maintenance catwalks, 360° dorsal worksite floodlight tower, stern CIWS Gatling Turret (`MAS Turret Stern`), 2× stern hydraulic bedrock outriggers (`MAS Outrigger Utility Port/Stbd`), telescoping soft-capture docking tongue (`MAS Piston Stern Dock`), and stern heavy dump bumper with relocated stone tailings ejector chute matching CATERPILLAR-01.
* **Grid:** Large Grid (1 B = 2.5 m) · **Consist Envelope:** 11 W × 42 L × 7 H B (27.5 × 105.0 × 17.5 m with L7 gantry arch).
* **Mass:** Consist Dry Mass `≈ 1,145.0 t` (Tractor: 558.2 t, Utility: 586.8 t) · Max Laden Mass `≈ 3,650 t` (Earth 1 g).
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
* **Consist Mechanics, 3-Axis Hitch & FOB Autonomy (Decision 67):** Optimizes cross-carriage cooperation and mechanical coupling: (1) 3-Axis Heavy Gimbal Tow Hitch: Upgrades articulated drawbar at Z −8 with 3 degrees of freedom (Yaw Advanced Rotor ±45°, Pitch Motor Hinge ±25°, Roll Advanced Rotor ±20° on Z-axis) for full 3D terrain compliance with zero subgrid shear, backed by twin magnetic stabilizer clamps (`MAS Hitch Clamp Port/Stbd`) to eliminate high-speed harmonic oscillation; (2) Distributed Power Unit (DPU) Smart Traction & Dynamic Braking: Synchronizes Carriage 2 Axles 4 and 5 with tractor throttle input for active hill-climb pushing, paired with predictive regenerative descent braking to keep the hitch in tension and prevent jackknifing; (3) Autonomous FOB Detachment ("Drop-and-Mine"): Equips Carriage 2 with standalone compute core (`[MAS-U] PB Utility`, `MAS-U Remote FOB`) and runaway deadman brake (`MAS-U EC Breakaway` + 2× keel magnetic skids), allowing the tractor to decouple as an agile 6×6 extraction unit while the trailer operates as an independent stationary outpost; (4) Stabilized Hauler Interchange: Adds 2× hydraulic stern outriggers (`MAS Outrigger Utility Port/Stbd` at Z −25) to isolate the chassis during 650 t Mule touchdowns, a telescoping soft-capture docking tongue (`MAS Piston Stern Dock`) for effortless Caterpillar coupling, and relocates the stone tailings aggregate ejector to the stern bumper; (5) Environmental & Ballast Hardening: Formalizes the inter-carriage gangway as an unpressurized flexible bellows flanked by dual hermetic airlocks (`MAS Door Gangway Fwd/Aft`) and adds 40× Heavy Armor ballast skids in Carriage 2 sub-floor; (6) Updated BOM: 1,095 blocks (+56 blocks), dry mass ≈ 1,124.0 t, 8,538 PCU.
* **4× Multitool Repair Turrets, SML Auto-Production & Mule Peer Resupply Sorties (Decision 68):** Comprehensive self-repair, docked drone maintenance, autonomous component queuing, and inter-rig peer assistance: (1) 4× Laser Multitool Turrets (Simple Laser Multitools Mod 3175774286 / ToolCore 3040438530) positioned for overlapping 360° consist and flight deck coverage (`MAS MT Turret Bow` on Tractor crown L5 Z +9, `MAS MT Turret Port/Stbd` on Carriage 2 forward shoulders L4 Z −10, `MAS MT Turret Stern` on elevated gantry arch L7 Z −23); turrets draw components from `MAS Hold Components` and reference `MAS Projector Master` to automatically repair damaged blocks on the Tractor, Carriage 2, docked Kestrel drones (L5 pads), docked Mule shuttle (L6 gantry pad), docked Caterpillar train, or external ground vehicles without engineer EVA; (2) Standard Mission Loadout (SML) Autonomous Production Queuing: `[MAS-U] PB Utility` monitors component inventory in `MAS Hold Components`; whenever stock falls below 75% of SML quotas (e.g. 1,500 Steel Plates, 500 Construction Comps, 200 Motors, 100 NATO 25mm ammo boxes), it automatically queues production deltas in `MAS Utility Assembler`, fed continuously by iron ingots from `MAS Field Refinery`; (3) Dedicated Outbound Resupply Staging Hopper: Adds 1× Small Cargo Container `MAS Hold Resupply Export` and Conveyor Sorter `MAS Sorter Resupply Export` directly below `MAS MP Conn` at [X0, Z −20, L5], allowing pre-packaging of standardized resupply kits without mixing into main hold; (4) Standardized Mule Peer Resupply Sortie Protocol: Implements 4 standardized mission pallets (`KIT_MOBILITY`, `KIT_POWER`, `KIT_MUNITIONS`, `KIT_HULL_REPAIR`) and IGC channels (`TMS.RESUPPLY.REQUEST`, `TMS.RESUPPLY.OFFER`, `TMS.RESUPPLY.DISPATCH`, `TMS.RESUPPLY.COMPLETE`); upon receiving an emergency call from a disabled sister rig (`MAS-02`), Mastodon pre-packs the kit into the export hopper, summons MULE-01 for an immediate top-deck transfer (< 10 s), and dispatches the shuttle on a rapid delivery flight; (5) Updated BOM: 1,110 blocks (+15 blocks), dry mass ≈ 1,145.0 t, 9,475 PCU.
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
| **Drawbar Hitch & 3-Axis Gimbal** | 2× Adv Rotors (Yaw/Roll), 1× Motor Hinge (Pitch), 1× Merge Block (Hitch Sever Tongue), 2× Mag Clamps, 1× Mag Capture Plate, Guide Horns (Palette 2) | 11 | 23.0 t | 635 |
| **Utility Carriage Hull & Ballast** | Heavy armor frames, wheel wells, deck armor, 40× Sub-Floor Ballast Skids (Palette 0) | 400 | 426.0 t | 400 |
| **Utility Powertrain & DPU** | 4× 5×5 Heavy Suspension Wheels (Axles 4-5) with DPU traction | 4 | 32.0 t | 400 |
| **Utility Sub-Floor Battery Vault** | 3× Large Batteries (9.0 MWh, Palette 6) | 3 | 13.5 t | 120 |
| **Interior Walkways, Decoys & Airlocks** | Grated catwalks, 2× Hermetic Gangway Airlocks, sliding doors, ladder runs, interior lights, 3× Armored Decoys, 2× Keel Recovery Gyros (Palette 9) | 30 | 18.0 t | 240 |
| **Field Workshop & Universal Life Support** | 1× Basic Refinery, 1× Large Assembler, 1× Medical Room, 2× Cryo Pods, 1× Survival Kit, 2× Large O2 Tanks, 1× Quarantined Ice Hold, 2× Large H2 Tanks, 2× O2/H2 Gens, 6× Air Vents, 1× Master Projector, 1× Scuttle Warhead | 22 | 68.2 t | 815 |
| **Utility Cargo Hold & Logistics Feed** | 1× Large Cargo Container (Components), 2× Munitions Sorters (`MAS Sorter Ammo Fwd / Parts Aft`), Conveyor network | 18 | 29.0 t | 210 |
| **Flight Decks, Welders & Solar** | 1× Mule Pad, 2× Kestrel Pads, 2× Drone Welders, 4× Solar Panels, 4× Floodlights | 67 | 61.1 t | 970 |
| **Stern Stabilizers, Telescoping Dock & Dump** | 2× Large Outrigger Pistons + 2× Mag Feet (`MAS Outrigger Utility`), 1× Telescoping Dock Piston (`MAS Piston Stern Dock`), Stern Dump Connector & Sorter, Relocated Tailings Ejector | 7 | 14.5 t | 510 |
| **Utility FOB Autonomy & Deadman Brake** | 1× PB (`[MAS-U] PB Utility`), 1× Remote Control (`MAS-U Remote FOB`), 1× Event Controller (`MAS-U EC Breakaway`), 2× Keel Mag Skids (`MAS Utility Brake Keel 1, 2`) | 5 | 4.0 t | 350 |
| **Perimeter Defense Suite** | 2× Gatling Turrets, 2× Interior Turrets, Ammunition Conveyor Feeds (Palette 7) | 12 | 9.6 t | 308 |
| **Multitool Repair Turrets** | 4× Large Laser Multitool Turrets (Bow L5, Port/Stbd L4 shoulders, Stern L7 arch · Mod 3175774286) | 4 | 18.0 t | 900 |
| **Peer Resupply Export Staging** | 1× Small Cargo (`MAS Hold Resupply Export`), 1× Conveyor Sorter (`MAS Sorter Resupply Export`), 9× Gantry Arch Heavy Armor blocks (Palette 0/3) | 11 | 3.0 t | 37 |
| **External EVA, Lights & Failsafes** | 2× Ground EVA Lockers, 3× Keel Lights, 1× Black Box SOS Beacon & Isolated Battery, 4× In-Situ Button Panels (Palette 9) | 11 | 2.5 t | 110 |
| **TOTAL CONSIST (DRY)** | Complete Expeditionary Mining Locomotive & Mobile FOB | **1,110** | **≈ 1,145.0 t** | **9,475** |

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
       [PHASE 10-1 MASTER SPATIAL LOCATOR HUD]
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

### Pre-Step Master Parts Tray: Phase 10-1
* 240× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 60× Heavy Armor Half-Slope Block (Palette 0)
* 20× Heavy Armor Half Block (Palette 0)
* 6× Wheel Suspension 5×5 (3 Port, 3 Starboard)
* 16× Large Conveyor Pipe (Palette 4: Processing Teal)
* 5× Large Conveyor Junction (Palette 4: Processing Teal)
* 1× Large Conveyor Sorter (Palette 4: Processing Teal)
* 2× Conveyor Ejector / Connector (Palette 4: Processing Teal)
* 2× Curved Conveyor Tube (Palette 4: Processing Teal)

---

### Step 10.1 · Twin Longitudinal Chassis Box Girders & Transverse Ties (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Chassis Backbone  ·  Grid Zone: [X ±3, Z −8 to +12]     │
│ [VIEWPORT] Level: L2 (Deck)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►   │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.1 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 40 | Slot 0 (`#2B303A`) | 34.0 t / 40 | 2× parallel 20 B longitudinal runs at X = ±3, Z −8 to +12 |
| Heavy Armor Full Block | 15 | Slot 0 (`#2B303A`) | 12.8 t / 15 | Transverse cross-ties (5 B each) at Z = +9, +4, −1 |
| Heavy Armor Full Block | 10 | Slot 0 (`#2B303A`) | 8.5 t / 10 | Bow Bulkhead cap (Z +12) and Aft Bulkhead cap (Z −8) |

```
Plan view, Y = L2 (Tractor Chassis Box Girder Deck)
        X: −3           X0           X: +3
    Z: +12 ■════════════╤════════════■  BOW BULKHEAD (Z +12 Datum)
           ║            │            ║  Central Conveyor Trough (X0)
    Z:  +9 ║  BEAM TIE  │  BEAM TIE  ║  ◄── Axle 1 Cross-Tie (X = ±3, Z +9)
           ║════════════╪════════════║
    Z:  +4 ║  BEAM TIE  │  BEAM TIE  ║  ◄── Axle 2 Cross-Tie (X = ±3, Z +4)
           ║════════════╪════════════║
    Z:  −1 ║  BEAM TIE  │  BEAM TIE  ║  ◄── Axle 3 Cross-Tie (X = ±3, Z −1)
           ║            │            ║
    Z:  −8 ■════════════╧════════════■  AFT HITCH BULKHEAD (Z −8 Datum)

    Legend:
    ■ Heavy Armor Corner / Bulkhead Node
    ║ Heavy Armor Full Block (Longitudinal Box Girder at X = ±3)
    ═ Heavy Armor Full Block (Transverse Axle Cross-Tie)
    │ Centerline Open Raceway (X0, 5-block internal gap)
```

#### Placement Instructions
1. Lay two parallel 20-block runs of Heavy Armor Full Blocks at X = −3 (Port) and X = +3 (Starboard) from Z −8 to Z +12 at Level L2.
2. Weld transverse 5-block Heavy Armor ties bridging between X −3 and X +3 at three axle datums: Z = +9 (Axle 1), Z = +4 (Axle 2), and Z = −1 (Axle 3).
3. Cap the forward end at Z = +12 with a solid transverse bulkhead tie (X −3 to +3).
4. Cap the aft end at Z = −8 with a solid transverse bulkhead tie (X −3 to +3) to anchor the future drawbar hitch.
5. Paint all structural armor blocks with Palette Slot 0 (`#2B303A` Industrial Dark Gray).

**✓ Check:** Two rigid parallel box girders at X = ±3 running exactly 20 blocks (50.0 m) from Z −8 to Z +12, with an open 5-block wide central trough along X0 bridged by 3 heavy cross-ties.

---

### Step 10.2 · Underside Half-Slopes & V-Bevel Belly Skid Channel (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Belly Deflector  ·  Grid Zone: [X −2 to +2, Z −8 to +12] │
│ [VIEWPORT] Level: L1 (Underside)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT · STBD ►        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.2 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Half-Slope | 40 | Slot 0 (`#2B303A`) | 17.0 t / 40 | Slopes placed at X = ±1, Z −8 to +12, facing inward toward X0 |
| Heavy Armor Half Block | 20 | Slot 0 (`#2B303A`) | 8.5 t / 20 | Centerline flush belly skid plates along X0, Z −8 to +12 |
| Heavy Armor Slope 2×1 | 12 | Slot 0 (`#2B303A`) | 7.2 t / 12 | Bow deflector bevels at Z = +11 to +12 |

```
Front Elevation Cross-Section, Y = L1-L2 (Belly Skid Channel Geometry)
    Port [−X]                                                          Starboard [+X]
    X: −3             X: −1            X0             X: +1             X: +3
    ┌───────────────┐ ┌───────────────────────────────────────────────┐ ┌───────────────┐
    │ L2 BOX GIRDER │ │            LEVEL L2 SUB-FLOOR DECK            │ │ L2 BOX GIRDER │
    └───────┬───────┘ └───────┬───────────────────────────────┬───────┘ └───────┬───────┘
            │                 │   [CENTRAL CONVEYOR PIPE]     │                 │
    Level L1│                 ▼             X0                ▼                 │Level L1
            │         ┌───────────────┐               ┌───────────────┐         │
            │         │   HALF-SLOPE  │   HALF-BLOCK  │   HALF-SLOPE  │         │
            │         │  (Slopes In)  │  (Skid Plate) │  (Slopes In)  │         │
            └─────────┴───────▲───────┴───────▲───────┴───────▲───────┴─────────┘
                              └───────────────┴───────────────┘
                                   1.25 m Belly Ground Clearance
    ═════════════════════════════════════════════════════════════════════════════ Terrain L0
```

#### Placement Instructions
1. Position Heavy Armor Half-Slope blocks along X = −1 from Z −8 to Z +12 at Level L1, angled so the sloped face bevels inward toward centerline X0.
2. Mirror on the starboard flank: place Heavy Armor Half-Slopes along X = +1 from Z −8 to Z +12 at Level L1, sloping inward toward X0.
3. Install continuous Heavy Armor Half Blocks along centerline X0 between the slopes, forming a flat skid keel 1.25 m above ground.
4. At the bow (Z = +11 to +12), install Heavy Armor Slope 2×1 blocks angled upward (+Y) to guide the belly smoothly over boulders during forward travel.

**✓ Check:** Underside forms an inverted, continuous trapezoidal trough along X0, providing 1.25 m (0.5 block) clearance to terrain with 45° beveled deflector flanks.

---

### Step 10.3 · Central Ore Conveyor Raceway & Manifold Junctions (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Primary Ore Trunk  ·  Grid Zone: [X0, Z −8 to +12]              │
│ [VIEWPORT] Level: L1  ·  Orientation: Looking Port (−X) Longitudinal Elevation        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.3 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Conveyor Junction | 5 | Slot 4 (`#1ABC9C`) | 7.5 t / 30 | Taps at Z +12 (Bow), Z +9 (Axle 1), Z +4 (Axle 2), Z 0 (Tailings), Z −1 (Axle 3), Z −8 (Hitch) |
| Large Conveyor Pipe | 15 | Slot 4 (`#1ABC9C`) | 11.2 t / 15 | Straight pipe runs connecting junctions along X0 at Level L1 |

```
Longitudinal Elevation, Looking Port (Centerline X0, Level L1 Conveyor Trunk)
    [BOW]                                                                                             [STERN]
    Z: +12          Z: +9          Z: +4             Z: 0          Z: −1                           Z: −8
      ■═══════════════■══════════════■════════════════■══════════════■═══════════════════════════════■
      │               │              │                │              │                               │
    [BOW INTAKE]  [AXLE 1 TAP]   [AXLE 2 TAP]   [TAILINGS TAP]   [AXLE 3 TAP]                    [HITCH TAP]
    (From Boom)   (Upward Riser) (Upward Riser) (Downward Flow)  (Upward to Power Core)          (To Drawbar)

    Legend:
    ■ Large Conveyor Junction (6-way connection hub, Palette 4: Processing Teal)
    ═ Large Conveyor Pipe (Straight 2.5 m run along X0)
```

#### Placement Instructions
1. Place a Large Conveyor Junction on centerline [X0, Z +12, Y = L1] facing forward (+Z) for future bow boom connection.
2. Route Large Conveyor Pipes along X0 aft to Z = +9; install Large Conveyor Junction at [X0, Z +9, Y = L1] with top port facing upward (+Y).
3. Continue pipes aft to Z = +4; install Large Conveyor Junction at [X0, Z +4, Y = L1] with top port facing upward (+Y).
4. Continue pipes aft to Z = 0; install Large Conveyor Junction at [X0, Z 0, Y = L1] with bottom port facing downward (−Y) for tailings ejectors.
5. Continue pipes aft to Z = −1; install Large Conveyor Junction at [X0, Z −1, Y = L1] with top port facing upward (+Y) for power vault fuel lines.
6. Run continuous pipes aft to Z = −8; terminate with Large Conveyor Junction at [X0, Z −8, Y = L1] with aft port facing −Z for the drawbar hitch.
7. Paint all conveyor junctions and pipes with Palette Slot 4 (`#1ABC9C` Processing Teal).

**✓ Check:** Unbroken, continuous large conveyor trunk from [X0, Z +12, L1] to [X0, Z −8, L1]. Upward ports clear at Z +9, +4, −1; downward port clear at Z 0.

---

### Step 10.4 · 5×5 Off-Road Suspension Wheel Mounts & Master 3-Axle Geometry (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor 6-Wheel Powertrain  ·  Grid Zone: [X ±4 to ±5, Z −1..+9]│
│ [VIEWPORT] Level: L1  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►          │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.4 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Wheel Suspension 5×5 Left | 3 | Slot 0 (`#2B303A`) | 24.0 t / 300 | Mount on Port outer girder face at X = −4 (Z +9, +4, −1), hub outward (−X) |
| Wheel Suspension 5×5 Right | 3 | Slot 0 (`#2B303A`) | 24.0 t / 300 | Mount on Stbd outer girder face at X = +4 (Z +9, +4, −1), hub outward (+X) |

```
Plan view, Y = L1 (Tractor Wheel Suspension Layout)
        X: −5             X: −4          X0          X: +4             X: +5
    Z:  +9 [WH 1 (P)] ──── [GIRDER] ═════════════════ [GIRDER] ──── [WH 2 (S)]   ◄── Axle 1 (+30.0° Steer)
    Z:  +4 [WH 3 (P)] ──── [GIRDER] ═════════════════ [GIRDER] ──── [WH 4 (S)]   ◄── Axle 2 (+15.0° Steer)
    Z:  −1 [WH 5 (P)] ──── [GIRDER] ═════════════════ [GIRDER] ──── [WH 6 (S)]   ◄── Axle 3 (0.0° Locked Drive)

    Legend:
    [WH 1..6] 5×5 Heavy Suspension Wheel Unit (Hub at X ±5, Tire extends X ±4.5 to ±6.5)
    [GIRDER]  Heavy Armor Box Girder side wall (X = ±4 mounting face)
```

#### Placement Instructions
1. Position Wheel Suspension 5×5 Left units on the outer face of the Port girder (X = −4) at Z = +9 (`WH 1`), Z = +4 (`WH 3`), and Z = −1 (`WH 5`). Align the top mounting bracket flush with Level L2.
2. Mirror on the starboard girder face (X = +4): install Wheel Suspension 5×5 Right units at Z = +9 (`WH 2`), Z = +4 (`WH 4`), and Z = −1 (`WH 6`).
3. Ensure suspension stators orient with steering pivot pins strictly vertical (Y-axis) and wheel hub axles horizontal.
4. Verify tire bounding boxes extend from X −4.5 to −6.5 (Port) and X +4.5 to +6.5 (Starboard), leaving 5.0 blocks clearance along the chassis flank.

#### Terminal Configuration & Sliders
* Terminal Group: `MAS Tractor Wheels` (Select all 6 wheel blocks).
* Terminal Subgroup: `MAS Wheels Steer` (`WH 1`, `WH 2`, `WH 3`, `WH 4`).
* Terminal Subgroup: `MAS Wheels Drive` (`WH 1` through `WH 6`).

| Terminal Name | Axle Index | Z-Datum | Max Steer Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction | Height Offset |
|---|---|---|---|---|---|---|---|---|
| `MAS Wheel A1-P` | Axle 1 | Z = +9 | `30.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A1-S` | Axle 1 | Z = +9 | `30.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A2-P` | Axle 2 | Z = +4 | `15.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A2-S` | Axle 2 | Z = +4 | `15.0°` | Normal | `100%` | `50% / 85%` | `70%` | `−32 cm` |
| `MAS Wheel A3-P` | Axle 3 | Z = −1 | `0.0°` (Locked) | OFF | `100%` | `55% / 90%` | `75%` | `−32 cm` |
| `MAS Wheel A3-S` | Axle 3 | Z = −1 | `0.0°` (Locked) | OFF | `100%` | `55% / 90%` | `75%` | `−32 cm` |

**✓ Check:** All 6 wheel hubs rotate freely; steering test confirms Axle 1 turns 30°, Axle 2 turns 15°, and Axle 3 remains locked rigid at 0°. Chassis rests at 1.25 m belly clearance.

---

### Step 10.5 · Tailings Aggregate Sorter & Underbelly Roadbed Ejectors (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tailings Venting Manifold  ·  Grid Zone: [X0, Z0 to −1]         │
│ [VIEWPORT] Level: L1  ·  Orientation: Underside looking Bow (+Z)                      │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.5 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Conveyor Sorter | 1 | Slot 4 (`#1ABC9C`) | 2.5 t / 15 | Plumbed under junction at [X0, Z0, Y = L1], flow arrow pointing DOWN (−Y) |
| Conveyor Ejector / Connector | 2 | Slot 4 (`#1ABC9C`) | 0.8 t / 20 | Angled 45° aft (−Z) beneath Axle 3 at [X ±1, Z −1, Y = L1] |
| Curved Conveyor Tube | 2 | Slot 4 (`#1ABC9C`) | 0.6 t / 6 | Directing sorter outlet horizontally to twin ejectors |

```
Underbelly Profile · Tailings Aggregate Spreader (Z0, L1)
     Central Conveyor Raceway Junction [X0, Z0, L1]
                          │
                          ▼ (Flow Arrow DOWN)
               [MAS Sorter Tailings]
                          │
                 ┌────────┴────────┐
                 ▼                 ▼
          [Curved Tube]      [Curved Tube]
                 │                 │
                 ▼                 ▼
       [MAS Ejector 1 (P)]   [MAS Ejector 2 (S)]  ◄── Angled 45° Aft (−Z)
                 │                 │
                 ▼                 ▼ (Vents Stone Tailings Behind Axle 3)
     ══════════════════════════════════════════════════ Crushed Stone Roadbed
```

#### Placement Instructions
1. Connect the top large port of Large Conveyor Sorter `MAS Sorter Tailings` to the downward port of junction [X0, Z0, Y = L1]. Ensure the directional arrow points downward (−Y).
2. Attach a conveyor splitter or Curved Conveyor Tubes to the sorter outlet, routing lines to [X −1, Z −1, L1] and [X +1, Z −1, L1].
3. Install twin Conveyor Ejectors `MAS Ejector Tailings 1` and `MAS Ejector Tailings 2` angled 45° aft (−Z), discharging crushed stone directly behind Axle 3's tire contact patch.
4. Paint sorter and ejectors with Palette Slot 4 (`#1ABC9C` Processing Teal).

#### Terminal Configuration & Sliders
* Block: `MAS Sorter Tailings`
  * Whitelist: `Stone`, `Gravel`
  * Drain All: `ON`
* Blocks: `MAS Ejector Tailings 1, 2`
  * Throw Out: `ON`
  * Collect All: `ON`
* Event Controller: `MAS EC Tailings`
  * Monitored Target: `MAS Tractor Buffer`
  * Condition: Cargo Fill $\ge 50\%$
  * Action Slot 1 (True): `MAS Sorter Tailings` -> Drain All ON; `MAS Ejectors Tailings` -> Power ON
  * Action Slot 2 (False): `MAS Sorter Tailings` -> Drain All OFF; `MAS Ejectors Tailings` -> Power OFF

#### Operational Clearance Note (Consist vs Standalone Mode)
When operating with Carriage 2 hitched, venting stone aggregate directly beneath Axle 3 (Z −1) causes loose stone chunks to collide with Carriage 2 trailing wheels (Axles 4 & 5 at Z −13 and Z −22). In consist operations, the active roadbed spreader is routed to the stern bumper of Carriage 2 ([X0, Z −26, Y = L1], Step 10.15E). The underbelly ejectors at [X ±1, Z −1] serve as standalone cleanout ports when the Tractor operates decoupled from the trailer.

**✓ Check:** Sorter whitelist passes only Stone and Gravel; high-value ores (Fe, Ni, Co, U) remain in the central spine. In consist travel, underbelly ejectors remain toggled OFF, routing crushed stone aft to the Carriage 2 stern spreader.

---

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
       [PHASE 10-2 MASTER SPATIAL LOCATOR HUD]
       Envelope: 7 W × 14 L × 3 H B (17.5 × 35.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z −8 to +6, Y = L2 to L4
       Subsystem Datums:
       · Ore Buffer Hold: Z +1 to +3, centered at X0
       · Primary Power Core: Z −4 to −2, centered at X0
       · 2-Axis Drawbar Hitch Center: [X0, Z −8, Y = L2]
       · Port Maintenance Crawlway: X −2, Z −7 to +6, Y = L3
```

### Pre-Step Master Parts Tray: Phase 10-2
* 1× Large Cargo Container (3×3×3 B, Palette 5: Logistics Emerald)
* 2× Small Nuclear Reactor (15 MW each, Palette 6: High-Voltage Red)
* 1× Hydrogen Engine (5 MW, 1×1×2 B, Palette 6: High-Voltage Red)
* 3× Large Battery (3 MWh each, 9.0 MWh total, Palette 6: High-Voltage Red)
* 1× Large Advanced Rotor (Palette 2: Heavy Rail Crimson)
* 1× Large Motor Hinge (Palette 2: Heavy Rail Crimson)
* 1× Large Merge Block (`MAS Hitch Sever Tongue`, Palette 2)
* 14× Grated Catwalk Block (Palette 9: High-Viz Safety Yellow)
* 3× Sliding Door (Palette 9: High-Viz Safety Yellow)
* 2× Small Cargo Container (`MAS Locker Port/Stbd`, Palette 9)
* 3× Decoy Block (Palette 0: Industrial Dark Gray)
* 2× Large Gyroscope (`MAS Gyro Recovery 1, 2`, Palette 0)
* 1× Radio Antenna / Beacon (`MAS Beacon Distress`, Palette 7)
* 1× Small Battery (`MAS Battery BlackBox`, Palette 6)
* 1× Large Warhead (`MAS Warhead Scuttle Power`, Palette 6)
* 1× Sci-Fi 4-Button Panel (`MAS BP Power Vault`, Palette 9)
* 6× Large Conveyor Pipe & 4× Curved Conveyor Tube (Palette 4)
* 120× Heavy Armor Full Block & Slopes (Palette 0 & Palette 6)

---

### Step 10.6 · Tractor Ore Buffer Hold (3×3×3 Large Cargo) & Intake Riser (L2-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Ore Buffer  ·  Grid Zone: [X −1..+1, Z +1..+3]          │
│ [VIEWPORT] Level: L2 to L4  ·  Orientation: Plan view Y = L3 (▲ Bow +Z)              │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.6 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Cargo Container | 1 | Slot 5 (`#27AE60`) | 18.0 t / 50 | Centered at [X0, Z +2, L3], spanning X −1 to +1, Z +1 to +3, Y L2 to L4 |
| Large Conveyor Pipe | 2 | Slot 4 (`#1ABC9C`) | 1.5 t / 2 | Vertical riser from L1 junction up to cargo bottom port |
| Heavy Armor Full Block | 16 | Slot 0 (`#2B303A`) | 13.6 t / 16 | Structural foundation saddle wrapping bottom edges at L2 |

```
Plan view, Y = L3 (Tractor Ore Buffer Hold & Jefferies Crawlway)
        X: −2             X: −1             X0             X: +1             X: +2
    Z: +3 ┌─────────────┐ ┌───────────────────────────────────────────────┐ ┌─────────────┐
          │ JEFFERIES   │ │                                               │ │ SOLID       │
    Z: +2 │ CRAWLWAY    │ │        MAS TRACTOR BUFFER (3×3×3 B)           │ │ ARMOR       │
          │ [CATWALK]   │ │         (Palette Slot 5: Logistics Emerald)   │ │ BULKHEAD    │
    Z: +1 │ [HATCH] ◄───┼─┤ (Manual Port)                                 │ │             │
          └─────────────┘ └───────────────────────────────────────────────┘ └─────────────┘

    Legend:
    [MAS TRACTOR BUFFER] 3×3×3 B Large Cargo Container (Receives boom ore)
    [HATCH]              Manual inventory access port facing Port Jefferies tube [X −2, Z +2, L3]
    [CATWALK]            Grated Catwalk Block along maintenance crawlway
```

#### Placement Instructions
1. Position 1 Large Cargo Container centered at [X0, Z +2, Y = L3], extending from X −1 to +1, Z +1 to +3, and vertically from Level L2 to L4.
2. Align the bottom 3×3 large conveyor port with the vertical conveyor riser from junction [X0, Z +4, L1].
3. Ensure the top 3×3 large conveyor port aligns at Level L4 for the future dorsal boom turret riser.
4. Align the Port face so its built-in conveyor/inventory access door faces into the Port Jefferies tube at [X −2, Z +2, L3].
5. Plumb the aft large conveyor port along X0 to feed the power core vault.
6. Paint the container exterior with Palette Slot 5 (`#27AE60` Logistics Emerald).

#### Terminal Configuration & Sliders
* Name: `MAS Tractor Buffer`
* Custom Data: `[Inventory] Buffer=True; Role=RawOreBuffer; MaxTonnage=1000`
* Show in Terminal: `ON` · Show on HUD: `ON`

**✓ Check:** Large Cargo Container securely seated between box girders. Bottom port plumbed to L1 raceway; top port ready at L4; Port inspection hatch accessible from crawlway.

---

### Step 10.7 · Primary Nuclear Power Core Vault & Electrical Shielding (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Baseload Nuclear Power  ·  Grid Zone: [X −2..+2, Z −4..−2]│
│ [VIEWPORT] Level: L3  ·  Orientation: Plan view Y = L3 (▲ Bow +Z)                    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.7 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Small Nuclear Reactor | 2 | Slot 6 (`#C0392B`) | 5.2 t / 50 | Mounted inline at [X0, Z −2, L3] and [X0, Z −3, L3], ports along X0 |
| Large Battery | 3 | Slot 6 (`#C0392B`) | 13.5 t / 120 | Mounted at [X −2, Z −2], [X +2, Z −2], and [X −2, Z −3] |
| Heavy Armor Full Block | 24 | Slot 6 (`#C0392B`) | 20.4 t / 24 | Double-walled radiation containment bulkheads surrounding vault |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | Vault maintenance entrance door at [X −2, Z −2, L3] |

```
Plan view, Y = L3 (Tractor Power Core Vault)
        X: −2             X0              X: +2
    Z: −2  ┌───────────────┬───────────────┬───────────────┐
           │ [MAS BAT 1]   │ [MAS RCT 1]   │ [MAS BAT 2]   │  ◄── Reactor 1 (15 MW) flanked by Batteries
    Z: −3  │ [MAS BAT 3]   │ [MAS RCT 2]   │ [HEAVY ARMOR] │  ◄── Reactor 2 (15 MW) flanked by Battery 3
    Z: −4  │ [HEAVY ARMOR] │ [MAS ENG H2]  │ [HEAVY ARMOR] │  ◄── Hydrogen Engine (5 MW Standby Failover)
           └───────────────┴───────────────┴───────────────┘
           Nuclear + H2 Baseload: 35.0 MW · Tractor Battery Reserve: 9.0 MWh (36.0 MW Peak)
```

#### Placement Instructions
1. Place Small Nuclear Reactor 1 (`MAS Reactor 1`) at [X0, Z −2, Y = L3] with conveyor ports aligned along centerline X0.
2. Place Small Nuclear Reactor 2 (`MAS Reactor 2`) directly aft at [X0, Z −3, Y = L3], connected in series with Reactor 1.
3. Place Large Battery 1 (`MAS Battery 1`) at [X −2, Z −2, L3] and Large Battery 2 (`MAS Battery 2`) at [X +2, Z −2, L3].
4. Place Large Battery 3 (`MAS Battery 3`) at [X −2, Z −3, L3].
5. Plumb the reactor conveyor line forward into the aft port of `MAS Tractor Buffer` for automated Uranium fuel feeding.
6. Install Sliding Door `MAS Door Vault` at [X −2, Z −2, L3] opening into the Port Jefferies crawlway.
7. Encase the entire vault perimeter in Heavy Armor blocks painted Palette Slot 6 (`#C0392B` High-Voltage Red).

#### Terminal Configuration & Sliders
* Group: `MAS Reactors` (`MAS Reactor 1`, `MAS Reactor 2`)
  * Role: 30.0 MW continuous baseload nuclear generation
* Group: `MAS Batteries Tractor` (`MAS Battery 1`, `MAS Battery 2`, `MAS Battery 3`)
  * Initial State: `Auto` (Peak Shaving) · Output: 36.0 MW peak discharge
* Door: `MAS Door Vault` (Closed by default)

**✓ Check:** 30 MW nuclear baseload + 9 MWh battery buffer verified in control terminal. Uranium supply line connected; vault door provides walk-in access from crawlway.

---

### Step 10.8 · Standby Emergency Hydrogen Engine Installation (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tier 3 Emergency Generator  ·  Grid Zone: [X0, Z −4]            │
│ [VIEWPORT] Level: L3  ·  Orientation: Looking Port (−X) Longitudinal Elevation        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.8 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Hydrogen Engine (Large Grid) | 1 | Slot 6 (`#C0392B`) | 4.8 t / 25 | 1×1×2 B footprint centered at [X0, Z −4, L3] extending aft to Z −5 |
| Large Conveyor Pipe | 1 | Slot 4 (`#1ABC9C`) | 0.8 t / 1 | Connects engine front conveyor port to reactor spine along X0 |

```
Longitudinal Elevation, Looking Port (Power Vault Centerline X0, Level L3)
    [FORWARD]                                                           [AFT]
    Z: +1              Z: −2             Z: −3             Z: −4 to −5
    ┌───────────────┐  ┌───────────────┐  ┌───────────────┐  ┌───────────────────────────────┐
    │  MAS TRACTOR  │  │  MAS REACTOR  │  │  MAS REACTOR  │  │    MAS ENGINE H2 (5.0 MW)     │
    │  BUFFER       │══╡       1       │══╡       2       │══╡ (1×1×2 B Hydrogen Generator)  │
    │  (3×3×3 Hold) │  │  (15.0 MW)    │  │  (15.0 MW)    │  │ [Sump Port]    [Manual Valve] │
    └───────────────┘  └───────────────┘  └───────────────┘  └───────────────────────────────┘
```

#### Placement Instructions
1. Place 1 Large Grid Hydrogen Engine (`MAS Engine H2`, 1×1×2 B) at [X0, Z −4, Y = L3], extending aft to Z −5.
2. Connect its forward conveyor port directly to the aft conveyor port of `MAS Reactor 2`.
3. Route the aft conveyor port toward the drawbar hitch conduit at Z −8 to draw fuel from Carriage 2's 10,000,000 L hydrogen reserve.
4. Verify the manual fuel shutoff valve and crank sump port face the interior aisle for manual field maintenance.
5. Paint engine with Palette Slot 6 (`#C0392B` High-Voltage Alert Red).

#### Terminal Configuration & Sliders
* Name: `MAS Engine H2`
* Enabled: `OFF` (Cold-standby reserve)
* Supervised by: Event Controller `MAS EC Power`
  * Monitored: `MAS Reactor 1, 2` Uranium Inventory
  * Threshold: $< 1.0\text{ kg}$
  * Action: Turn `MAS Engine H2` ON; sound fuel alert chime

**✓ Check:** Hydrogen engine securely mounted behind Reactor 2; plumbed to central conveyor spine; verified 5.0 MW failover output when toggled ON.

---

### Step 10.9 · Port Maintenance Jefferies Tube & Physical Buffer Access Hatch (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Crew Maintenance Crawlway  ·  Grid Zone: [X −2..−3, Z −7..+6]   │
│ [VIEWPORT] Level: L3  ·  Orientation: Plan view Y = L3 looking down (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.9 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Grated Catwalk Block | 14 | Slot 9 (`#F1C40F`) | 4.2 t / 14 | Floor tiles along X −2, Z −7 to +6 |
| Interior Light | 4 | Slot 9 (`#F1C40F`) | 0.2 t / 12 | Amber bulkhead lamps mounted along crawlway ceiling at L4 |
| Sci-Fi 4-Button Panel | 1 | Slot 9 (`#F1C40F`) | 0.1 t / 5 | Mounted on vault outer bulkhead at [X −2, Z −1, L3] |
| Heavy Armor Full Block | 28 | Slot 0 (`#2B303A`) | 23.8 t / 28 | Outer armored hull skirt running along X = −3 |

```
Plan view, Y = L3 (Port Jefferies Tube Maintenance Corridor)
        X: −3 (Outer Skirt)       X: −2 (Walkway)           X: −1 (Inner Machinery)
    Z:  +6 ┌─────────────────────┬─────────────────────────┬─────────────────────────────┐
           │ HEAVY ARMOR SKIRT   │ [FORWARD AIRLOCK VEST]  │ TO C2 BRIDGE CABIN          │
    Z:  +2 │ HEAVY ARMOR SKIRT   │ [CATWALK] ──► [HATCH]   │ MAS TRACTOR BUFFER (3×3×3)  │
    Z:  −1 │ HEAVY ARMOR SKIRT   │ [MAS BP POWER VAULT]    │ MAS DOOR VAULT (Sliding)    │
    Z:  −3 │ HEAVY ARMOR SKIRT   │ [CATWALK]               │ POWER CORE VAULT BULKHEAD   │
    Z:  −7 │ HEAVY ARMOR SKIRT   │ [AFT GANGWAY AIRLOCK]   │ TO ACCORDION GANGWAY        │
           └─────────────────────┴─────────────────────────┴─────────────────────────────┘
```

#### Placement Instructions
1. Lay Grated Catwalk Blocks along X = −2 from Z +6 (Airlock Vestibule) aft to Z −7 (Gangway Bulkhead) at Level L3.
2. Build the outer heavy armor hull skirt at X = −3 enclosing the walkway.
3. Ensure the physical cargo maintenance port on the Port face of `MAS Tractor Buffer` at [X −2, Z +2, L3] remains completely unobstructed.
4. Mount Sci-Fi 4-Button Panel `MAS BP Power Vault` on the vault bulkhead at [X −2, Z −1, L3].
5. Install 4 amber Interior Lights in the crawlway ceiling spaced 3 blocks apart.

#### Terminal Configuration & Sliders
* Panel: `MAS BP Power Vault`
  * Button 1: Sliding Door `MAS Door Vault` -> Open/Close
  * Button 2: Group `MAS Reactors` -> Power ON/OFF (Nuclear SCRAM)
  * Button 3: Generator `MAS Engine H2` -> Power ON/OFF (Cold-Start)
  * Button 4: Group `MAS Batteries Tractor` -> Recharge ON/OFF

**✓ Check:** Unobstructed 1-block wide pressurized crawlway. Cargo hatch opens for manual inventory access; button panel toggles vault door and power systems.

---

### Step 10.10 · Heavy Motorized 2-Axis Drawbar Hitch & Pyrotechnic Severance Merge Block (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 3-Axis Gimbal Drawbar Hitch  ·  Grid Zone: [X0, Z −8 to −9.5]   │
│ [VIEWPORT] Level: L2  ·  Orientation: Profile looking Port & Plan view (▲ Bow +Z)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.10 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Advanced Rotor | 2 | Slot 2 (`#9E2A2B`) | 17.6 t / 30 | `MAS Hitch Yaw` [X0, Z −8, L2] facing −Z; `MAS Hitch Roll` [X0, Z −9, L2] inline on Z-axis |
| Large Motor Hinge | 1 | Slot 2 (`#9E2A2B`) | 3.5 t / 15 | `MAS Hitch Pitch` [X0, Z −8.5, L2] horizontal pin for vertical pitch |
| Merge Block (Large Grid) | 1 | Slot 2 (`#9E2A2B`) | 1.2 t / 15 | `MAS Hitch Sever Tongue` mounted on drawbar tongue at Z −9.5 facing Aft (−Z) |
| Large Magnetic Plate | 3 | Slot 2 (`#9E2A2B`) | 4.2 t / 90 | 2× `MAS Hitch Clamp Port/Stbd` [X ±2, Z −8.5, L2]; 1× `MAS Hitch Capture Plate` on trailer nose |
| Camera | 1 | Slot 7 (`#1B263B`) | 0.2 t / 10 | `MAS Cam Hitch` facing aft with cockpit guidance crosshairs |
| Heavy Armor Full Block & Slopes | 8 | Slot 2 (`#9E2A2B`) | 6.8 t / 8 | Alignment guide horns and structural wishbone tongue frame |

```
2.5D Isometric Callout · 3-Axis Articulated Gimbal Tow Hitch & Hard-Lock
    Tractor Aft Bulkhead [X0, Z −8, L2]
           ║
           ╠══[ADVANCED ROTOR: MAS Hitch Yaw] ──► Horizontal Steering Articulation (±45.0°)
           ║        │ (Aft-Facing Large Port)
           ║        ▼
           ╠══[MOTOR HINGE: MAS Hitch Pitch] ────► Vertical Terrain Grade Pitch (±25.0°)
           ║        │ (Horizontal Pin)
           ║        ▼
           ╠══[ADVANCED ROTOR: MAS Hitch Roll] ──► Longitudinal Torsional Roll (±20.0°)
           ║        │ (Z-Axis Rotational Pass-Thru)
           ║        ▼
           ╠══[MERGE BLOCK: MAS Hitch Sever Tongue] ──► Structural Hard-Lock & Emergency Decouple
           ║        │
           ║        ▼
           ╚════════╪════════════════════════════► To Utility Carriage (Carriage 2)
                    ▼
           [LARGE CONVEYOR PASS-THROUGH PIPE]

    Anti-Oscillation High-Speed Clamps:
    [MAS Hitch Clamp Port (X −2)] ────► Locks onto Trailer Strike Plate (> 30 km/h)
    [MAS Hitch Clamp Stbd (X +2)] ────► Locks onto Trailer Strike Plate (Rigid Transit)
```

#### Placement Instructions
1. Mount the stator of Large Advanced Rotor `MAS Hitch Yaw` on the Tractor Aft Bulkhead at [X0, Z −8, Y = L2], facing directly Aft (−Z). Connect its front port to the central conveyor raceway.
2. Attach the Large Advanced Rotor Head. Connect the internal large conveyor passage.
3. Mount the stator of Large Motor Hinge `MAS Hitch Pitch` at [X0, Z −8.5, L2] directly onto the yaw rotor head with the hinge barrel oriented horizontally (X-axis) for vertical pitch.
4. Mount the stator of Large Advanced Rotor `MAS Hitch Roll` at [X0, Z −9, L2] on the pitch hinge head, aligned horizontally along the Z-axis to permit up to ±20.0° torsional roll rotation.
5. Build the drawbar tongue using Heavy Armor blocks and mount Large Grid Merge Block `MAS Hitch Sever Tongue` at [X0, Z −9.5, L2] facing Aft (−Z) to connect with Carriage 2.
6. Install 2× Large Magnetic Plates (`MAS Hitch Clamp Port/Stbd`) at [X −2, Z −8.5, L2] and [X +2, Z −8.5, L2] facing Aft to clamp onto trailer strike plates during high-speed travel.
7. Install camera `MAS Cam Hitch` above the drawbar facing Aft with HUD crosshairs for docking alignment.
8. Construct beveled Heavy Armor guide horns on Carriage 2's nose to funnel the tongue within a ±1.2 m alignment tolerance.
9. Paint all drawbar components with Palette Slot 2 (`#9E2A2B` Heavy Rail Crimson).

#### Terminal Configuration & Sliders
* Rotor: `MAS Hitch Yaw`
  * Velocity: `0.5 rpm` · Braking Torque: `50 MN·m`
  * Lower Limit: `−45.0°` · Upper Limit: `+45.0°` · Share Inertia Tensor: `ON`
* Hinge: `MAS Hitch Pitch`
  * Velocity: `0.5 rpm` · Braking Torque: `45 MN·m`
  * Lower Limit: `−25.0°` · Upper Limit: `+25.0°` · Share Inertia Tensor: `ON`
* Rotor: `MAS Hitch Roll`
  * Velocity: `0.5 rpm` · Braking Torque: `30 MN·m`
  * Lower Limit: `−20.0°` · Upper Limit: `+20.0°` · Share Inertia Tensor: `ON`
* Group: `MAS Hitch Clamps` (`MAS Hitch Clamp Port`, `MAS Hitch Clamp Stbd`)
  * Auto-Lock: `OFF` · Proximity Sensor Interlock: Active
* Merge Block: `MAS Hitch Sever Tongue`
  * Enabled: `ON` (Structural hard-lock)

**✓ Check:** Drawbar articulates freely in 3 axes (Yaw ±45°, Pitch ±25°, Roll ±20°) without chassis armor clipping. Clamps rigidly lock tongue straight when energized.

---

### Step 10.11 · Smart Anti-Jackknife Lockout & Inter-Carriage Flexible Weather Bellows with Dual Hermetic Airlocks (L2-L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Anti-Jackknife Logic & Gangway  ·  Grid Zone: [X0, Z −9 to −8]  │
│ [VIEWPORT] Level: L2-L3  ·  Orientation: Looking Port (−X) Longitudinal Section       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.11 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sliding Door | 2 | Slot 9 (`#F1C40F`) | 2.4 t / 30 | `MAS Door Gangway Fwd` [X0, Z −8, L3] and `MAS Door Gangway Aft` [X0, Z −9, L3] |
| Grated Catwalk Block | 2 | Slot 9 (`#F1C40F`) | 0.6 t / 2 | Articulated gangway walkway plates spanning Z −8 to −9 |
| Heavy Armor Slope Block | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Bellows perimeter weather fairings surrounding gangway gap |

```
Longitudinal Section through Flexible Gangway (Looking Port, X0, Level L3)
    CARRIAGE 1 (TRACTOR)                                          CARRIAGE 2 (UTILITY)
    Aft Bulkhead (Z −8)                                           Fwd Bulkhead (Z −9)
    ┌──────────────────────┐                                      ┌──────────────────────┐
    │ [PORT JEFFERIES]     │                                      │ [FIELD WORKSHOP]     │
    │                      │                                      │                      │
    │ [MAS DOOR GANG FWD]  │ ═════════ [CATWALK] ════════════════ │ [MAS DOOR GANG AFT]  │
    └──────────┬───────────┘    (Flexible Weather Bellows)        └──────────┬───────────┘
    ═══════════╧═════════════════════════════════════════════════════════════╧═══════════ Level L2
               [3-AXIS GIMBAL DRAWBAR: YAW ROTOR + PITCH HINGE + ROLL ROTOR]
```

#### Placement Instructions
1. Install Tractor Aft Gangway door `MAS Door Gangway Fwd` at [X0, Z −8, Y = L3].
2. Install Utility Forward Gangway door `MAS Door Gangway Aft` at [X0, Z −9, Y = L3].
3. Bridge the 1-block mechanical gap with slip-resistant Grated Catwalks, enclosed within flexible heavy armor bellows.
4. **Hermetic Protocol:** In Space Engineers, subgrids cannot share an airtight seal across mechanical joints. The gangway enclosure serves as a dust/weather shield. In vacuum environments, crew members cycle through the dual sliding doors as an airlock vestibule, or traverse with sealed EVA helmets.
5. Configure Event Controllers `MAS EC Hitch Speed` and `MAS EC Hitch Rev`.

#### Terminal Configuration & Sliders
* Event Controller: `MAS EC Hitch Speed`
  * Monitored: Vehicle Speedometer
  * Threshold: $> 30.0\text{ km/h}$
  * Action Slot 1 (True): Group `MAS Hitch Clamps` -> Lock ON; `MAS Hitch Yaw` -> Braking Torque = `100 MN·m` (Rigid Anti-Sway Lock)
  * Action Slot 2 (False): Group `MAS Hitch Clamps` -> Lock OFF; `MAS Hitch Yaw` -> Braking Torque = `50 MN·m` (Active Flex Articulation)
* Event Controller: `MAS EC Hitch Rev`
  * Monitored: Transmission Gear
  * Condition: Gear = Reverse
  * Action Slot 1 (True): `MAS Hitch Yaw` -> Clamp Limits to `±15.0°`; Axle 5 -> Invert Steering OFF
  * Action Slot 2 (False): `MAS Hitch Yaw` -> Restore Limits to `±45.0°`; Axle 5 -> Invert Steering ON

**✓ Check:** Dual airlock doors cycle with active gas reclamation. Above 30 km/h, magnetic clamps energize, locking drawbar rigid and eliminating harmonic trailer snaking.

---

### Step 10.12 · Ground-Level EVA Survival Lockers & Underbelly Keel Lights (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Ground EVA & Keel Lighting  ·  Grid Zone: [X ±5, Z −4] (Lockers) │
│ [VIEWPORT] Level: L1-L2  ·  Orientation: Looking Bow (+Z) Cross-Section at Z −4      │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.12 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Small Cargo Container | 2 | Slot 9 (`#F1C40F`) | 1.2 t / 20 | Recessed at [X −5, Z −4, L2] (Port) and [X +5, Z −4, L2] (Stbd) |
| Interior Light | 3 | Slot 9 (`#F1C40F`) | 0.2 t / 9 | Mounted along centerline keel at [X0, Z +5, L1], [X0, Z 0, L1], [X0, Z −5, L1] |

```
Front Elevation Cross-Section at Z −4 (Ground EVA Lockers & Keel Lighting)
    Port [−X]                                                          Starboard [+X]
    X: −5             X: −3            X0              X: +3            X: +5
    ┌───────────────┐ ┌──────────────┐ ┌─────────────┐ ┌──────────────┐ ┌───────────────┐
    │ MAS LOCKER P  │ │ BOX GIRDER   │ │ CONVEYOR    │ │ BOX GIRDER   │ │ MAS LOCKER S  │
    │ [EVA Supplies]│ │ (Heavy Armor)│ │ (Central)   │ │ (Heavy Armor)│ │ [EVA Supplies]│
    └───────┬───────┘ └──────┬───────┘ └──────┬──────┘ └──────┬───────┘ └───────┬───────┘
            │                │                │               │                 │
    Level L1│                │         [KEEL LIGHT]           │                 │Level L1
            │                │         (Amber LED ↓)          │                 │
    ════════╧════════════════╧════════════════╧═══════════════╧═════════════════╧════════ Ground L0
     ▲ Ground-Level Human Reach (1.8 m)                                          ▲
```

#### Placement Instructions
1. Recess Small Cargo Container `MAS Locker Port` flush into the lower Port armor skirt at [X −5, Z −4, Y = L2]. Plumb its conveyor port into the lower raceway.
2. Mirror on Starboard: install Small Cargo Container `MAS Locker Stbd` at [X +5, Z −4, Y = L2].
3. Mount 3 Interior Lights `MAS Lights Underbelly` facing downward along the chassis keel at [X0, Z +5, L1], [X0, Z 0, L1], and [X0, Z −5, L1].
4. Paint lockers with Palette Slot 9 (`#F1C40F` High-Viz Safety Yellow).

#### Terminal Configuration & Sliders
* Blocks: `MAS Locker Port`, `MAS Locker Stbd`
  * Stocking: 4× O2 Bottles, 4× H2 Bottles, 2× Welder Tools, 500× NATO 25×184mm ammo
* Group: `MAS Lights Underbelly`
  * Color: Amber (`R: 255, G: 180, B: 50`) · Radius: `5.0 m` · Intensity: `1.5`

**✓ Check:** Lockers can be opened by an engineer standing on planet terrain (Level L0) without jetpack; amber lights illuminate suspension hubs and tailings manifold.

---

### Step 10.13 · Native Threat Decoys, Keel Recovery Gyros & Armored Black Box SOS Cell (L1-L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Failsafes & Asset Protection  ·  Grid Zone: [X0, Z0 to +7]      │
│ [VIEWPORT] Level: L1-L3  ·  Orientation: Plan view Y = L1 (▲ Bow +Z)                  │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.13 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Decoy Block | 3 | Slot 0 (`#2B303A`) | 3.6 t / 60 | Enclosed in lower armor skirts: [X −5, Z −4, L2], [X +5, Z −4, L2], [X0, Z −26, L2] |
| Large Gyroscope | 2 | Slot 0 (`#2B303A`) | 24.0 t / 100 | Mounted in lower chassis keel at [X0, Z 0, Y = L1] |
| Radio Antenna / Beacon | 1 | Slot 7 (`#1B263B`) | 0.8 t / 15 | `MAS Beacon Distress` encased in heavy armor at [X0, Z +7, L1] |
| Small Battery | 1 | Slot 6 (`#C0392B`) | 0.5 t / 10 | `MAS Battery BlackBox` (cold isolated standby) at [X0, Z +7, L1] |
| Large Warhead | 1 | Slot 6 (`#C0392B`) | 1.6 t / 20 | `MAS Warhead Scuttle Power` in power vault at [X0, Z −3, L3] |

```
Plan view, Y = L1 (Keel Failsafes & SOS Black Box Cell)
        X: −5              X0              X: +5
    Z:  +7 ┌───────────────┬───────────────┬───────────────┐
           │               │[BLACK BOX CELL│               │  ◄── Armored SOS Beacon + Isolated Battery
           │               │ 50 km Distress│               │
    Z:   0 │               │[MAS GYRO 1, 2]│               │  ◄── Self-Righting Roll Override Gyroscopes
           │               │(Recovery Core)│               │
    Z:  −4 │[DECOY PORT]   │               │[DECOY STBD]   │  ◄── Ballistic Decoy Skirts (Divert Fire)
           └───────────────┴───────────────┴───────────────┘
```

#### Placement Instructions
1. Install Ballistic Decoys `MAS Decoy Flank Port` at [X −5, Z −4, L2] and `MAS Decoy Flank Stbd` at [X +5, Z −4, L2], encased within heavy armor skirt blocks.
2. Install 2 Large Gyroscopes `MAS Gyro Recovery 1` and `MAS Gyro Recovery 2` in the lower keel at [X0, Z 0, L1].
3. Construct a 6-sided heavy armor capsule at [X0, Z +7, L1] housing Radio Beacon `MAS Beacon Distress` (50 km) and isolated Small Battery `MAS Battery BlackBox`.
4. Install Large Grid Warhead `MAS Warhead Scuttle Power` between the reactors at [X0, Z −3, L3] (disarmed by default).

#### Terminal Configuration & Sliders
* Gyroscopes: `MAS Gyro Recovery 1, 2`
  * Override: `OFF` (Normal transit) · Roll Override: `+100%` (Ready for righting toggle)
* Beacon & Battery: `MAS Beacon Distress` & `MAS Battery BlackBox`
  * Battery: `Recharge = OFF` · `Auto = OFF` (Isolated cold standby)
* Event Controller: `MAS EC BlackBox`
  * Monitored: Main Grid Power Output
  * Condition: Total MW $= 0.0\text{ MW}$ (Total blackout)
  * Action: Activate `MAS Battery BlackBox` -> Broadcast `SOS-MASTODON-01-LOST` (50 km)

**✓ Check:** Hostile turrets target decoys away from bridge and reactors; isolated black box fires distress ping upon power cut; recovery gyros right vehicle from 90° lateral rollover.

---

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

```
PHASE 10-2 VISUAL MILESTONE · POWER CORE, BUFFER & DRAWBAR HITCH COMPLETE
       Z: −8 (Drawbar)        Z: −4 (Power Vault)    Z: +2 (Ore Buffer)
      [HITCH SEVER TONGUE]    [35 MW TIER 1 CORE]    [3×3×3 BUFFER HOLD]
             ║                         │                      │
       ══════╩═════════════════════════╧══════════════════════╧═══════
                    PORT JEFFERIES CRAWLWAY RUN (Z −7 to +6, L3)
```

## Phase 10-3 · Hydraulic Bedrock Outriggers & 45 m Long-Reach Excavation Boom (Levels L1-L8)

```
       [PHASE 10-3 MASTER SPATIAL LOCATOR HUD]
       Envelope: 15 W × 22 L × 6 H B (37.5 × 55.0 × 15.0 m operating envelope)
       Coordinate Range: X −7 to +7, Z +5 to +22, Y = L1 to L8
       Pedestal Datum: [X0, Z +6, Y = L4] (Dorsal Turret Ring)
       Travel Stow Clamp Datum: [X0, Z −7, Y = L4] (Dorsal Spine Trough)
       Outrigger Locations:
       · Port Forward Outrigger: [X −5, Z +11, Y = L1]
       · Starboard Forward Outrigger: [X +5, Z +11, Y = L1]
       · Port Aft Outrigger: [X −5, Z −6, Y = L1]
       · Starboard Aft Outrigger: [X +5, Z −6, Y = L1]
```

### Pre-Step Master Parts Tray: Phase 10-3
* 4× Large Piston (Palette 1: Hazard Amber)
* 4× Seismic Surveying Landing Gear (`MAS Seismic Outrigger 1..4`, TMS Mod 2819263781, Palette 1)
* 1× Large Advanced Rotor (`MAS Turret Azimuth`, Palette 3: Excavator Ochre)
* 1× Large Motor Hinge (`MAS Shoulder Pitch`, Palette 3: Excavator Ochre)
* 1× Large Motor Hinge (`MAS Elbow Pitch`, Palette 3: Excavator Ochre)
* 1× Large Conveyor Elbow (90° Downward Tool Head Adapter, Palette 4: Processing Teal)
* 1× Large Motor Hinge (`MAS Wrist Pitch`, Palette 3: Excavator Ochre)
* 1× Large Advanced Rotor (`MAS Wrist Roll`, Palette 3: Excavator Ochre)
* 2× Simple Laser Multitool (`MAS Laser 1, 2`, Palette 3: Excavator Ochre)
* 1× Large Magnetic Plate (`MAS Tool Grapple`, Palette 9: High-Viz Safety Yellow)
* 1× Large Magnetic Plate (`MAS Clamp Boom Stow`, Palette 1: Hazard Amber · Travel Cradle at Z −7)
* 1× Camera · `MAS Cam Boom` (High-magnification targeting crosshairs)
* 2× Spotlight · `MAS Light Boom 1, 2` (Palette 3)
* 1× Sensor · `MAS Sensor Boom` (4.0 m proximity stop)
* 12× Large Conveyor Pipe (Palette 4: Processing Teal)
* 24× Heavy Armor Slope & Box Girder Blocks (Palette 3)

---

### Step 10.14 · 4× Corner Bedrock Hydraulic Outriggers & Seismic Landing Gear (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bedrock Outriggers & Seismic Prospecting  ·  Level: L1-L2       │
│ [VIEWPORT] Grid Zone: [X ±5, Z +11] & [X ±5, Z −6]  ·  Looking Bow (+Z) Front Section│
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.14 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Piston | 4 | Slot 1 (`#E6A100`) | 12.8 t / 100 | Mounted vertically at [X ±5, Z +11] and [X ±5, Z −6], shaft pointing DOWN (−Y) |
| Seismic Surveying Landing Gear | 4 | Slot 1 (`#E6A100`) | 5.6 t / 160 | `MAS Seismic Outrigger 1..4` (Mod 2819263781) attached to piston heads |
| Heavy Armor Full Block | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Reinforced outrigger corner corner socket ties |

```
Front Cross-Section at Axle 1 (Outriggers Deployed to Bedrock Baseline)
     [OUTRIGGER PORT: PISTON 1]                                  [OUTRIGGER STBD: PISTON 2]
     [X −5, Z +11, L2]                                           [X +5, Z +11, L2]
            │                                                           │
            ▼ (Shaft Extends Down)                                      ▼ (Shaft Extends Down)
     ┌──────────────┐                                            ┌──────────────┐
     │ LARGE PISTON │                                            │ LARGE PISTON │
     └──────┬───────┘ ─── [WH 1] ─── [BELLY SKIDS] ─── [WH 2] ─── └──────┬───────┘
            │                                                           │
            ▼                                                           ▼
     [SEISMIC FOOT P]                                            [SEISMIC FOOT S]
     (Acoustic Pulse)                                            (Acoustic Pulse)
    ════════╧═══════════════════════════════════════════════════════════╧════════ Terrain Baseline
```

#### Placement Instructions
1. Position 4 downward-facing Large Pistons in reinforced corner sockets at:
   * Port Forward: [X −5, Z +11, Y = L2] (`MAS Outrigger Piston 1`)
   * Starboard Forward: [X +5, Z +11, Y = L2] (`MAS Outrigger Piston 2`)
   * Port Aft: [X −5, Z −6, Y = L2] (`MAS Outrigger Piston 3`)
   * Starboard Aft: [X +5, Z −6, Y = L2] (`MAS Outrigger Piston 4`)
2. Attach Seismic Surveying Landing Gear (`MAS Seismic Outrigger 1..4`, Mod 2819263781) to each piston head facing downward toward planet voxels.
3. Paint pistons and landing gear with Palette Slot 1 (`#E6A100` Hazard Amber).

#### Terminal Configuration & Sliders
* Group: `MAS Outriggers` (Pistons 1..4)
  * Velocity: `0.2 m/s` · Max Distance: `2.5 m` (1 block stroke) · Share Inertia Tensor: `ON`
* Group: `MAS Outrigger Pads` (`MAS Seismic Outrigger 1..4`)
  * Auto-Lock: `ON`
* Event Controller: `MAS EC Level` (Auto-Leveling)
  * Monitored: `MAS Helm` Artificial Horizon Pitch and Roll
  * Condition: Deck Pitch/Roll $> 0.5^\circ$ -> Differential extension to achieve $0.0^\circ$ Level
* Event Controller: `MAS EC Seismic`
  * Monitored: `MAS Outrigger Pads` Lock State
  * Condition: All 4 Pads = Locked -> Trigger 500 m Acoustic Pulse -> Broadcast `TMS.SCOUT.VEIN`

**✓ Check:** All 4 outrigger pistons extend smoothly; magnetic landing gear lock green upon voxel contact; acoustic survey pulse triggers upon 4/4 lock.

---

### Step 10.15 · Centerline Turret Pedestal & Azimuth Advanced Rotor (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Dorsal Boom Turntable Pedestal  ·  Grid Zone: [X0, Z +6, Y = L4]│
│ [VIEWPORT] Level: L4  ·  Orientation: Plan view Y = L4 looking down (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.15 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Advanced Rotor | 1 | Slot 3 (`#E67E22`) | 8.8 t / 15 | Stator mounted at [X0, Z +6, L4] facing UP (+Y), large port aligned to L3 riser |
| Heavy Armor Slope Block | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Collar ring reinforcement wrapping the rotor stator |
| Large Conveyor Pipe | 1 | Slot 4 (`#1ABC9C`) | 0.8 t / 1 | Vertical riser connecting top of `MAS Tractor Buffer` at L3 to stator |

```
Plan view, Y = L4 (Turret Pedestal Turntable Deck)
        X: −2             X0              X: +2
    Z:  +7 ┌───────────────┬───────────────┬───────────────┐
           │ HEAVY ARMOR   │ C2 BRIDGE AFT │ HEAVY ARMOR   │  ◄── Bridge Aft Bulkhead
    Z:  +6 ├───────────────┼───────────────┼───────────────┤
           │ SLOPE RING    │[ADV ROTOR: MAS│ SLOPE RING    │  ◄── 360° Azimuth Turntable Ring
           │ (Reinforced)  │ TURRET AZIMUTH│ (Reinforced)  │      (Palette Slot 3: Ochre)
    Z:  +5 ├───────────────┼───────────────┼───────────────┤
           │ SLOPE RING    │ DORSAL TROUGH │ SLOPE RING    │  ◄── Trough leading aft to stow clamp
           └───────────────┴───────────────┴───────────────┘
```

#### Placement Instructions
1. Position the stator of Large Advanced Rotor `MAS Turret Azimuth` at [X0, Z +6, Y = L4], facing directly UP (+Y).
2. Plumb the bottom 3×3 large conveyor port directly into the top port of `MAS Tractor Buffer` at Level L3.
3. Attach the Large Advanced Rotor Head facing UP (+Y).
4. Surround the stator base with 8 Heavy Armor Slope blocks forming a reinforced structural collar.
5. Paint the rotor with Palette Slot 3 (`#E67E22` Excavator Ochre).

#### Terminal Configuration & Sliders
* Name: `MAS Turret Azimuth`
* Velocity: `0.75 rpm` · Braking Torque: `50 MN·m`
* Rotor Limits: Lower Limit: `−180.0°` · Upper Limit: `+180.0°` (360° Continuous Pan)
* Share Inertia Tensor: `ON`
* Symmetrical Reach: Centerline mounting guarantees identical **21.25 m physical reach (8.5 blocks)** beyond outer tires on both Port and Starboard flanks.

**✓ Check:** Turntable stator seated rigidly behind bridge canopy. Head spins 360° with 50 MN·m braking torque; large conveyor conduit passes through rotor center.

---

### Step 10.16 · Shoulder Pitch Hinge & 7-Block Main Boom Arm Casing (L4-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Shoulder Elevation & Main Arm  ·  Level: L4 to L6               │
│ [VIEWPORT] Grid Zone: Spanning 7 B from pedestal  ·  Looking Port (−X) Profile       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.16 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Motor Hinge | 1 | Slot 3 (`#E67E22`) | 3.5 t / 15 | Stator mounted on Rotor Head; hinge barrel horizontal along X-axis (Pitch) |
| Large Conveyor Pipe | 7 | Slot 4 (`#1ABC9C`) | 5.2 t / 7 | Continuous 7-block (17.5 m) internal conveyor pipe line |
| Heavy Armor Full Block / Slope | 14 | Slot 3 (`#E67E22`) | 11.9 t / 14 | Boxed structural armor casing enclosing the conveyor pipe |

```
Longitudinal Profile · Shoulder Joint & Main Boom Arm (Looking Port)
                                                               [ELBOW JOINT]
                                                               (To Step 10.17)
                                                                      ▲
                                                                     ╱
                                           [MAIN BOOM ARM · 7 B]    ╱
                                           (17.5 m Heavy Casing)   ╱
                                                                  ╱
                               [SHOULDER PITCH HINGE] ───────────┘
                               (Horizontal Pin along X-axis)
                                     │
                                     ▼
                        [TURRET AZIMUTH ROTOR HEAD]
                        [Chassis Pedestal: X0, Z +6, L4]
```

#### Placement Instructions
1. Mount the stator of Large Motor Hinge `MAS Shoulder Pitch` directly onto the top face of the Azimuth Rotor Head.
2. **Critical Orientation:** Orient the hinge stator so the barrel pivot pin is strictly horizontal along the X-axis (Port-Starboard), enabling vertical pitch elevation.
3. Attach the Large Motor Hinge Head.
4. Plumb 7 Large Conveyor Pipes in a straight run extending from the hinge head (17.5 m span).
5. Encase the pipes in Heavy Armor Full and Slope blocks, forming a boxed girder casing painted Palette Slot 3 (`#E67E22` Excavator Ochre).

#### Terminal Configuration & Sliders
* Name: `MAS Shoulder Pitch`
* Velocity: `0.5 rpm` · Braking Torque: `45 MN·m`
* Lower Limit: `−20.0°` (Undercutting terrain / spine stow) · Upper Limit: `+75.0°` (High reach)
* Share Inertia Tensor: `ON`

**✓ Check:** Shoulder elevates cleanly between −20° and +75°; 17.5 m main boom arm holds elevation under 45 MN·m torque with zero droop.

---

### Step 10.17 · Elbow Pitch Hinge & 5-Block Forearm Girder (L6-L8)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Elbow Joint & Forearm Reach  ·  Level: L6 to L8                 │
│ [VIEWPORT] Grid Zone: Forearm extension (5 B span)  ·  Looking Port (−X) Profile    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.17 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Motor Hinge | 1 | Slot 3 (`#E67E22`) | 3.5 t / 15 | Stator mounted at tip of main boom arm; hinge barrel horizontal (Pitch) |
| Large Conveyor Pipe | 5 | Slot 4 (`#1ABC9C`) | 3.7 t / 5 | Continuous 5-block (12.5 m) internal conveyor pipe line |
| Structural Girder / Slope | 10 | Slot 3 (`#E67E22`) | 6.5 t / 10 | Tapered lightweight girder casing enclosing forearm |

```
Longitudinal Profile · Elbow Joint & Forearm Girder (Looking Port)
                                             [FOREARM GIRDER · 5 B]
                                             (12.5 m Tapered Casing)
                                             ═══════════════════════► To Wrist (Step 10.18)
                                            ╱
                      [ELBOW PITCH HINGE] ─┘
                      (Horizontal Pin)
                             │
                             ▼
               [From Main Boom Arm (Step 10.16)]
```

#### Placement Instructions
1. Mount the stator of Large Motor Hinge `MAS Elbow Pitch` at the forward tip of the 7-block main boom arm.
2. **Critical Orientation:** Align the hinge barrel strictly horizontal along the X-axis (Pitch), parallel to the shoulder hinge.
3. Attach the Large Motor Hinge Head.
4. Plumb 5 Large Conveyor Pipes extending from the hinge head (12.5 m span).
5. Encase the piping in tapered Structural Girder and slope blocks painted Palette Slot 3 (`#E67E22` Excavator Ochre).

#### Terminal Configuration & Sliders
* Name: `MAS Elbow Pitch`
* Velocity: `0.5 rpm` · Braking Torque: `45 MN·m`
* Lower Limit: `−100.0°` (Descent into deep pits) · Upper Limit: `+45.0°` (Extended reach)
* Share Inertia Tensor: `ON`

**✓ Check:** Elbow articulates from −100° to +45°; combined main arm + forearm delivers 35.0 m physical reach from turret center without structural flex.

---

### Step 10.18 · 90° Downward Tool Elbow, Wrist Pitch Hinge & Wrist Roll Rotor (L7-L8)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 2-Axis Wrist Articulator  ·  Level: L7-L8                       │
│ [VIEWPORT] Grid Zone: Forearm Tip  ·  Looking Port (−X) & Front Elevation            │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.18 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Conveyor Elbow | 1 | Slot 4 (`#1ABC9C`) | 0.8 t / 1 | 90° downward adapter mounted at forearm tip, outlet facing strictly DOWN (−Y) |
| Large Motor Hinge | 1 | Slot 3 (`#E67E22`) | 3.5 t / 15 | `MAS Wrist Pitch` stator mounted to elbow outlet, pin horizontal (Pitch) |
| Large Advanced Rotor | 1 | Slot 3 (`#E67E22`) | 8.8 t / 15 | `MAS Wrist Roll` stator mounted to hinge head, rotating face facing DOWN (−Y) |

```
Longitudinal Callout · 2-Axis Wrist Articulator (Forearm Tip)
              [Forearm Girder Tip]
                       │
                       ▼
             [90° DOWN CONVEYOR ELBOW] ──► Sets neutral reference to straight down (−Y)
                       │
                       ▼
             [MOTOR HINGE: MAS Wrist Pitch] ──► Pitch Compensation: −60° to +60°
                       │
                       ▼
             [ADV ROTOR: MAS Wrist Roll] ────► 360° Continuous Axial Spin (5 rpm Core Bore)
                       │
                       ▼
             [TOOL MOUNTING FLANGE] ─────────► (To Step 10.19)
```

#### Placement Instructions
1. Attach Large Conveyor Elbow to the tip of the forearm piping, orienting its outlet port facing strictly DOWN (−Y) toward planetary gravity.
2. Mount the stator of Large Motor Hinge `MAS Wrist Pitch` to the elbow outlet. Orient its barrel horizontal for pitch compensation.
3. Attach the hinge head. Mount the stator of Large Advanced Rotor `MAS Wrist Roll` to the hinge head with its rotor head facing downward (−Y).
4. Attach the Large Advanced Rotor Head. This head serves as the universal mounting plate for the tool head.
5. Paint wrist blocks with Palette Slot 3 (`#E67E22` Excavator Ochre).

#### Terminal Configuration & Sliders
* Hinge: `MAS Wrist Pitch`
  * Limits: `−60.0°` to `+60.0°` · Braking Torque: `20 MN·m` · Velocity: `1.0 rpm`
  * Share Inertia Tensor: `ON`
  * Active Gravity Horizon Leveling: Counter-pitches to offset arm angle ($\theta_{\text{wrist}} = -(\theta_{\text{shoulder}} + \theta_{\text{elbow}})$), locking tool vector strictly vertical ($[0, -1, 0]$).
* Rotor: `MAS Wrist Roll`
  * Rotor Limits: Lower Limit: OFF · Upper Limit: OFF (Continuous 360° Roll)
  * Velocity: `5.0 rpm` · Braking Torque: `20 MN·m` · Share Inertia Tensor: `ON`

**✓ Check:** Wrist counter-rotates to maintain strictly downward tool orientation regardless of arm elevation; roll rotor spins smoothly at 5.0 rpm.

---

### Step 10.19 · Laser Mining Tool Head, Heavy Recovery Grapple & Travel Stow Cradle Clamp (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Excavation Tool Head & Chassis Travel Stow  ·  Level: L4        │
│ [VIEWPORT] Grid Zone: Tool Head at Wrist Roll Head; Stow Clamp at [X0, Z −7, L4]     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.19 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Simple Laser Multitool | 2 | Slot 3 (`#E67E22`) | 6.0 t / 200 | `MAS Laser 1, 2` (ToolCore) mounted on roll face, spaced 2.5 m apart |
| Large Magnetic Plate | 1 | Slot 9 (`#F1C40F`) | 2.4 t / 25 | `MAS Tool Grapple` mounted centrally between lasers, face pointing DOWN (−Y) |
| Large Magnetic Plate | 1 | Slot 1 (`#E6A100`) | 2.4 t / 25 | `MAS Clamp Boom Stow` mounted flush in Tractor dorsal spine trough at [X0, Z −7, L4] |
| Camera | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Cam Boom` centered looking straight down drill bore |
| Spotlight | 2 | Slot 3 (`#E67E22`) | 0.4 t / 16 | `MAS Light Boom 1, 2` flanking tool head |
| Sensor | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Sensor Boom` (4.0 m forward collision safety buffer) |

```
Front Elevation · Tool Head Mounting Face (Looking Up at Roll Face)
                  ┌───────────────────────────────────────────────┐
                  │                 MAS Cam Boom                  │
                  ├───────────────┬───────────────┬───────────────┤
                  │  MAS Laser 1  │   MAS TOOL    │  MAS Laser 2  │
                  │ (Simple Laser │    GRAPPLE    │ (Simple Laser │
                  │  Multitool)   │  (Mag Plate)  │  Multitool)   │
                  ├───────────────┼───────────────┼───────────────┤
                  │ MAS Light 1   │MAS Sensor Boom│ MAS Light 2   │
                  └───────────────┴───────────────┴───────────────┘
```

#### Placement Instructions
1. Mount 2 Simple Laser Multitools (`MAS Laser 1` and `MAS Laser 2`) onto the outer flanks of the Wrist Roll Head, spaced 2.5 m apart with conveyor ports plumbed into the rotor pass-through.
2. Mount 1 Large Magnetic Plate `MAS Tool Grapple` centrally between the two laser emitters, facing outward along the tool head axis.
3. Install Camera `MAS Cam Boom` centered directly down the drill bore; flank with Spotlights `MAS Light Boom 1` and `MAS Light Boom 2`.
4. Install Sensor `MAS Sensor Boom` calibrated for a 4.0 m forward safety buffer.
5. On the Tractor chassis dorsal deck, install Large Magnetic Plate `MAS Clamp Boom Stow` flush in the spine trough at [X0, Z −7, Y = L4] facing UP (+Y), flanked by twin heavy armor guide coamings running from Z +5 to Z −8.

#### Terminal Configuration & Sliders
* Lasers: `MAS Laser 1, 2` (ToolCore Tri-Mode: Mining Drill <-> Salvage Grinder <-> Fabrication Welder)
* Grapple: `MAS Tool Grapple` (Auto-Lock: `OFF` · Mag Strength: `100%`)
* Stow Clamp: `MAS Clamp Boom Stow` (Auto-Lock: `ON` · Mag Strength: `100%`)
* Sensor: `MAS Sensor Boom` (Front: `4.0 m`, Left/Right: `2.0 m`, Up/Down: `2.0 m` -> Action: Cut Lasers & Clamp Joint Brakes)

**✓ Check:** Lasers fire in Mining, Salvage, and Fabrication modes; grapple locks onto test block; boom slews 180° aft into dorsal trough and locks green into `MAS Clamp Boom Stow` at 5.2 m transit height.

---

### Step 10.20 · Automated Excavation Macro Modes & Kinematic Sweep Sequences

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Excavator Kinematic Macro Engine  ·  Software/Hardware Layer    │
│ [VIEWPORT] Supervised by: `[MAS] PB Excavator` (Update1 / 60 Hz)                     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### 5 Standard Excavation & Recovery Macros
1. **Macro 1: Bench Quarrying (Horizontal Terracing):**
   * Azimuth Turret sweeps across face: `−45.0°` (Port) to `+45.0°` (Starboard).
   * Shoulder Hinge incrementally steps down `−2.5°` per pass (0.5 B cut depth).
   * Wrist Pitch elevates `+90.0°` to maintain flat, drivable terrace floors parallel to surface.
2. **Macro 2: Target-Centric Vertical Column Bore (Subterranean Shaft):**
   * Accepts target vein waypoint $(X_t, Z_t)$ from seismic scan or Station 2 operator.
   * Turret Azimuth slews to target bearing; Shoulder and Elbow extend tool head over target.
   * Wrist Pitch counter-pitches to lock laser beams strictly vertical ($[0, -1, 0]$).
   * Wrist Roll spins twin offset lasers at 5.0 rpm, cutting an initial 3.0 m core.
   * Motion controller translates arm in automated 2D concentric circular sweeps ($\Delta X, \Delta Z$) around $(X_t, Z_t)$ to expand shaft diameter (up to 12.5 m wide).
   * Steps downward $\Delta Y = -1.0\text{ m}$ per pass down to $-25.0\text{ m}$ physical depth ($-55\text{ m}$ with laser beams).
3. **Macro 3: Trench & Road Carve (Overland Right-of-Way):**
   * Boom deploys along centerline ($Z+$), executing downward scoop arc ($-5.0\text{ m}$ to grade).
   * Tractor crawls forward at 2.0 km/h; automatically grades a level 15.0 m wide roadbed for Caterpillar land trains.
4. **Macro 4: Crane & Vehicle Recovery (`MACRO_CRANE`):**
   * Lasers deactivated; `MAS Tool Grapple` energized.
   * Station 2 CTC mouse control provides direct 3-axis teleoperation up to 42.5 m away.
   * Utilizes 45 MN·m joint holding torque and hydraulic outriggers to right rolled vehicles or hoist heavy cargo modules.
5. **Macro 5: Wreck Deconstruction (`MACRO_SALVAGE`):**
   * Lasers switch to Mode: Salvage Beam (Grinder).
   * Positions tool head at 15 to 30 m standoff; executes automated planar raster sweeps ($\Delta Y, \Delta X$) across target bounding box, disassembling enemy hulls layer-by-layer with 100% component retention into `MAS Hold Components`.

#### Automated Travel Stow & Deploy Sequences
* **Travel Stow Sequence (`STOW`):**
  1. Mining/salvage lasers deactivate; tool head retracts vertically clear of worksite.
  2. Turret Azimuth slews to 180.0° (Aft centerline).
  3. Shoulder, Elbow, and Wrist hinges align flat (0.0°).
  4. Boom lowers into armored dorsal spine trough; `MAS Clamp Boom Stow` at [X0, Z −7, L4] engages (Green Lock).
  5. Joint holding brakes clamp at 100% torque (50 MN·m Azimuth, 45 MN·m Hinges).
  6. Outriggers retract; consist is cleared for 35 km/h overland transit.
* **Work Deploy Sequence (`DEPLOY`):**
  1. Consist comes to full stop; handbrake engages.
  2. Outriggers extend and lock bedrock (verified by `MAS EC Stabilizers`).
  3. `MAS Clamp Boom Stow` releases.
  4. Shoulder elevates +15.0° to clear spine coamings.
  5. Slew to target waypoint azimuth and begin commanded macro.

---

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
       [PHASE 10-4 MASTER SPATIAL LOCATOR HUD]
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

### Pre-Step Master Parts Tray: Phase 10-4
* 3× Control Station (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`, Palette 7: Fleet Command Navy)
* 2× Programmable Block (`[MAS] PB Excavator`, `[MAS] PB RIG//OS-Lite`, Palette 7)
* 3× AI Automaton Block (`MAS AI Move`, `MAS AI Task`, `MAS AI Defense`, Palette 7)
* 1× Laser Antenna (`MAS Comm Laser`, 40 km, Palette 7)
* 1× Radio Antenna (`MAS Comm Radio`, 50 km broadcast, Palette 7)
* 1× Large Ore Detector (150 m subterranean range, Palette 4: Processing Teal)
* 1× Survival Kit (Tractor Field Respawn & Med Station, Palette 7)
* 14× Sloped Heavy Armor Glass (45° Panoramic Canopy)
* 6× LCD Panel (Tactical Video Wall & Console HUDs, Palette 7)
* 1× Air Vent (`MAS Vent Bridge`, Hermetic Zone 1)
* 1× Large Warhead (`MAS Warhead Scuttle Bridge`, disarmed anti-capture charge)
* 1× Timer Block (`MAS Timer Scuttle`, 60-second scuttle loop)
* 6× Sci-Fi 4-Button Panel (`MAS BP Ground Boarding`, `MAS BP Airlock Vestibule`, etc.)
* 1× Motor Hinge (`MAS Crew Ladder Hinge`, Palette 9: High-Viz Safety Yellow)
* 2× Narrow Ladder Block (Palette 9)
* 1× Sensor (`MAS Sensor Ladder`, 2.5 m proximity trigger)
* 1× Large Gatling Turret (`MAS Turret Bow`, Palette 7: Fleet Command Navy)
* 2× Interior Turret (`MAS Turret Flank Port/Stbd`, Palette 7)

---

### Step 10.21 · Forward Bridge Foundation Deck, Walkways & Airlock Vestibule (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bridge Foundation & Entry Vestibule  ·  Level: L3              │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z +7 to +12]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.21 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 24 | Slot 0 (`#2B303A`) | 20.4 t / 24 | Foundation floor deck across X −3 to +3, Z +7 to +12 at Level L3 |
| Grated Catwalk Block | 12 | Slot 9 (`#F1C40F`) | 3.6 t / 12 | Walkway aisle floor along X0 and Port entry lane |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Airlock` at [X −2, Z +7, L3], opening into Jefferies tube |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Bridge` mounted in ceiling at [X0, Z +9, L4] |
| Survival Kit | 1 | Slot 7 (`#1B263B`) | 2.1 t / 25 | Mounted against Port aft bulkhead at [X −2, Z +8, L4] |

```
Plan view, Y = L3 (Bridge Sub-Deck & Airlock Vestibule)
        X: −3             X: −2             X0              X: +2             X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────────────────────┐
           │ HEAVY ARMOR FOUNDATION DECK (Under Canopy Glazing)                          │
    Z: +10 │ [SUB-FLOOR C2 RACK] ── (Dual PBs + AI Blocks beneath Deck L4 Walkway)       │
    Z:  +8 ├─────────────────────┬─────────────────────────┬─────────────────────────────┤
           │ [MAS DOOR AIRLOCK]  │ [AIRLOCK VESTIBULE]     │ [LADDER UP TO ROOF CROWN]   │
    Z:  +7 └───────▲─────────────┴─────────────────────────┴─────────────────────────────┘
                   │
         From Port Jefferies Tube (X −2)
```

#### Placement Instructions
1. Lay Heavy Armor Full Blocks across X −3 to +3, Z +7 to +12 at Level L3, forming the foundation deck.
2. Install Sliding Door `MAS Door Airlock` at [X −2, Z +7, Y = L3], creating the hermetic seal between the Port Jefferies tube and the Bridge cabin.
3. Lay Grated Catwalk Blocks along the vestibule threshold and central walkway.
4. Mount Survival Kit on the Port bulkhead at [X −2, Z +8, Y = L4] with conveyor port plumbed to the tractor oxygen trunk.
5. Install Air Vent `MAS Vent Bridge` in the vestibule ceiling to manage cabin pressurization.

#### Terminal Configuration & Sliders
* Door: `MAS Door Airlock` (Auto-Close: `ON` · Delay: `3.0 s`)
* Vent: `MAS Vent Bridge` (Depressurize: `OFF` · Maintains 100% O2 cabin pressure)
* Block: `MAS Survival Kit Tractor` (Spawn point enabled)

**✓ Check:** Foundation deck sealed airtight; airlock sliding door cycles cleanly; survival kit powered and pressurized.

---

### Step 10.22 · 3-Operator Console Stations (Helm, Boom CTC, Fleet Director) & LCD Wall (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 3-Operator C2 Command Bridge  ·  Level: L4                      │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z +10 to +11]  ·  Plan view Y = L4 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.22 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Control Station (1×1×1 Open) | 3 | Slot 7 (`#1B263B`) | 1.8 t / 45 | Stations placed side-by-side at X −2, X0, X +2 (Z +10), facing Bow (+Z) |
| LCD Panel (Normal) | 4 | Slot 7 (`#1B263B`) | 0.4 t / 20 | Tactical Video Wall mounted on bridge aft bulkhead at Z +8 |
| Curved LCD Panel | 2 | Slot 7 (`#1B263B`) | 0.2 t / 10 | Side HUD repeaters flanking helm and tactical director |

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
    Z: +7  ├───────────────────────────┴──────┬──────┴───────────────────┤
           │ [AIRLOCK SLIDING DOOR]           │ [LADDER UP TO ROOF CROWN]│
           └──────────────────────────────────┴──────────────────────────┘
```

#### Placement Instructions
1. Mount 3 open 1×1×1 Control Stations side-by-side along Z = +10, facing forward (+Z):
   * Station 1 (Port): `MAS Helm` at [X −2, Z +10, Y = L4] (Traction, steering, power, outriggers).
   * Station 2 (Center): `MAS Station Boom` at [X0, Z +10, Y = L4] (CTC mouse aim, macros, camera).
   * Station 3 (Starboard): `MAS Station Fleet` at [X +2, Z +10, Y = L4] (Fleet coordination, JIT dispatch).
2. Maintain a clear 1-block wide walk-around aisle at Z = +9 behind the seats.
3. Mount 4 LCD Panels on the aft bulkhead at Z +8 forming the Tactical Video Wall (`MAS LCD Video Wall 1..4`).
4. Paint consoles and video wall frames with Palette Slot 7 (`#1B263B` Fleet Command Navy).

#### 3-Operator Multi-Page Manual Hotbar Architecture

```
================================================================================
STATION 1: HELM PILOT (`MAS Helm`)
================================================================================
[Hotbar 1 · Tractive Powertrain & Bedrock Anchoring]
· Slot 1: Group `MAS Outriggers` -> Reverse (Deploy / Retract Tractor Outriggers)
· Slot 2: Group `MAS Outriggers Utility` -> Reverse (Deploy / Retract Stern Outriggers)
· Slot 3: Handbrake -> Toggle
· Slot 4: Group `MAS Wheels Drive` -> Propulsion ON/OFF
· Slot 5: Group `MAS Wheels All` -> Increase Strength (+5%)
· Slot 6: Group `MAS Wheels All` -> Decrease Strength (-5%)
· Slot 7: Event Controller `MAS EC Level` -> Trigger Auto-Leveling
· Slot 8: Group `MAS Hitch Clamps` -> Switch Lock (Anti-Oscillation Magnetic Clamping)
· Slot 9: PB `[MAS] PB Excavator` -> Run `DPU TOGGLE` (DPU Push / Dynamic Braking)

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
· Slot 7: PB `[MAS] PB Excavator` -> Run `MT_TURRETS TOGGLE` (Toggle 4× Multitool Repair Turrets Weld/Grind)
· Slot 8: PB `[MAS-U] PB Utility` -> Run `AUTOPROD SML` (Verify/Force SML Quota Production)
· Slot 9: Group `MAS Multitool Turrets` -> Toggle ON/OFF (Consist & Drone Field Repair)

================================================================================
STATION 3: TACTICAL FLEET DIRECTOR (`MAS Station Fleet`)
================================================================================
[Hotbar 1 · Predictive JIT Logistics & Offload Automation]
· Slot 1: PB `[MAS] PB RIG//OS-Lite` -> Run `JIT AIR` (Pre-Dispatch Mule Air Ferry)
· Slot 2: PB `[MAS] PB RIG//OS-Lite` -> Run `JIT GND` (Pre-Dispatch Caterpillar Train)
· Slot 3: Connector `MAS MP Conn` -> Lock/Unlock (Mule Roof Pad Connector)
· Slot 4: Group `MAS MP Clamps` -> Switch Lock (Mule Cradle Rub Rails)
· Slot 5: Sorter `MAS MP Sorter` -> Drain All ON/OFF (Rare Ore Fast Drain)
· Slot 6: Sorter `MAS Sorter Resupply Export` -> Drain All ON/OFF (Export Staging Drain)
· Slot 7: Piston `MAS Piston Stern Dock` -> Reverse (Extend / Retract Telescoping Soft Dock)
· Slot 8: Connector `MAS Stern Dump Conn` -> Lock/Unlock (Caterpillar Stern Rail)
· Slot 9: PB `[MAS] PB Excavator` -> Run `RESUPPLY MULE` (Stage Resupply Kit & Dispatch Mule)

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

**✓ Check:** All 3 stations face forward with clear view of the forward terrain; all 9 hotbar pages mapped and functioning.

---

### Step 10.23 · 45° Sloped Heavy Armor Glass Panoramic Canopy & Roof Crown (L4-L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Armored Glazing & Roof Crown  ·  Level: L4 to L5                │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z +11 to +12]  ·  Looking Starboard (+X) Elevation│
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.23 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sloped Heavy Armor Glass | 14 | Transparent / Tint | 8.4 t / 14 | 45° forward glacis slopes (X −3 to +3, Z +11 to +12, L4-L5) |
| Heavy Armor Slope Block | 12 | Slot 0 (`#2B303A`) | 10.2 t / 12 | Roof crown frame connecting glass tops to Level L5 deck |
| Narrow Ladder Block | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 2 | Vertical ladder at [X +2, Z +7, L4-L5] climbing to roof crown hatch |

```
Longitudinal Elevation · Bridge Glazing Profile (Looking Starboard, X0)
    Level L5 Crown ──────────■═══════════════════════════■  (Dorsal Crown Deck)
                             ╲                           ║  Aft Bulkhead (Z +7)
                              ╲  45° SLOPED HEAVY       ║
    Level L4 Bridge Deck       ╲  ARMOR GLASS CANOPY     ║  [STATION 2: BOOM]
    ════════════════════════════■────────────────────────╨  [C2 Server Rack Under]
    Level L3 Foundation                                      Level L3 (Z +7)
    ═══════════════════════════════════════════════════════
```

#### Placement Instructions
1. Install Sloped Heavy Armor Glass blocks along the forward glacis edge (Z = +11 to +12) angled at 45°, spanning X −3 to +3 across Levels L4 to L5.
2. Install corner glass transitions on Port (X = −3) and Starboard (X = +3) to provide 180° horizontal field of view.
3. Tie the upper edge of the glass into Heavy Armor Slope blocks forming the Level L5 roof crown.
4. Install vertical ladder blocks at [X +2, Z +7] climbing from the bridge deck up through an armored hatch to the roof crown.

**✓ Check:** Cabin completely enclosed and pressurized (100% O2); panoramic forward sightlines provide unobstructed view of the cutting zone and ground terrain.

---

### Step 10.24 · C2 Compute Server Rack, Deep Ore Detector & Comms Mast (L3, L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Avionics, Sensors & Comms Spire  ·  Level: L3 & L6              │
│ [VIEWPORT] Grid Zone: Rack [X0, Z +7, L3]; Detector [X0, Z +8, L3]; Mast [X −2, Z +7, L6]│
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.24 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Programmable Block | 2 | Slot 7 (`#1B263B`) | 1.0 t / 50 | `[MAS] PB Excavator` & `[MAS] PB RIG//OS-Lite` in L3 rack |
| AI Automaton Block | 3 | Slot 7 (`#1B263B`) | 0.9 t / 75 | `MAS AI Move`, `MAS AI Task`, `MAS AI Defense` in L3 rack |
| Large Ore Detector | 1 | Slot 4 (`#1ABC9C`) | 4.8 t / 15 | Centered in sub-deck keel at [X0, Z +8, Y = L3], range: 150 m |
| Laser Antenna | 1 | Slot 7 (`#1B263B`) | 2.5 t / 15 | `MAS Comm Laser` (40 km line-of-sight) on dorsal mast [X −2, Z +7, L6] |
| Radio Antenna | 1 | Slot 7 (`#1B263B`) | 0.8 t / 15 | `MAS Comm Radio` (50 km fleet broadcast) on dorsal mast |

```
Longitudinal Section · C2 Server Rack & Comms Mast Placement
         [MAS COMM LASER (40 km)]     [BOW CIWS GATLING]
         [MAS COMM RADIO (50 km)]     [X0, Z +7, Y = L5]
                 │                           │
                 ▼ (Mast at X −2, L6)        ▼
    Level L5 ════■═══════════════════════════■═══════ (Crown Deck)
                 │                           │
    Level L4 ────┼── [3-STATION C2 BRIDGE] ──┼─────── (Bridge Deck)
                 │                           │
                 ▼                           ▼
    Level L3 ┌───────────────────────────┐ ┌────────┐
             │ [C2 SERVER RACK: DUAL PB] │ │ ORE    │ (150 m Deep Range)
             │ [AI AUTOMATON SUITE]      │ │ DETECT │
             └───────────────────────────┘ └────────┘
```

#### Placement Instructions
1. In the sub-bridge compartment at Level L3, mount Programmable Block `[MAS] PB Excavator` and Programmable Block `[MAS] PB RIG//OS-Lite` side-by-side at [X0, Z +7, Y = L3].
2. Flank with AI Automaton blocks: `MAS AI Move`, `MAS AI Task`, and `MAS AI Defense`.
3. Mount Large Ore Detector `MAS Ore Detector` in the centerline keel cavity at [X0, Z +8, Y = L3].
4. Erect the dorsal comms spire at [X −2, Z +7, Y = L6], mounting Laser Antenna `MAS Comm Laser` and Radio Antenna `MAS Comm Radio`.

#### Terminal Configuration & Sliders
* Programmable Block: `[MAS] PB Excavator` (Update rate: `Update1` / 60 Hz · Kinematics engine)
* Programmable Block: `[MAS] PB RIG//OS-Lite` (Update rate: `Update10` · Fleet C2 & IGC router)
* Antenna: `MAS Comm Radio` (Range: `50,000 m` · Broadcast: `ON`)
* Laser Antenna: `MAS Comm Laser` (Range: `40,000 m`)
* Ore Detector: `MAS Ore Detector` (Range: `150 m`)

**✓ Check:** Dual PBs initialize; ore detector displays subterranean ore signatures out to 150 m; laser antenna and 50 km radio establish network link.

---

### Step 10.25 · Bridge Anti-Capture Scuttle System & Native Countdown Timer (L3-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Asset Denial & Scuttle Loop  ·  Level: L3-L4                    │
│ [VIEWPORT] Grid Zone: Warhead [X0, Z +8, L3]; Panel [X +1, Z +8, L4]                 │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.25 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Grid Warhead | 1 | Slot 6 (`#C0392B`) | 1.6 t / 20 | `MAS Warhead Scuttle Bridge` mounted beneath server rack at [X0, Z +8, L3] |
| Timer Block | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Timer Scuttle` (60-second native countdown loop) |
| Sci-Fi 4-Button Panel | 1 | Slot 6 (`#C0392B`) | 0.1 t / 5 | `MAS BP Scuttle Bridge` mounted on bridge aft bulkhead at [X +1, Z +8, L4] |

#### Placement Instructions
1. Mount 1 Large Grid Warhead `MAS Warhead Scuttle Bridge` directly beneath the C2 compute rack at [X0, Z +8, Y = L3]. Disarmed by default.
2. Mount Timer Block `MAS Timer Scuttle` in the avionics rack.
3. Install Sci-Fi 4-Button Panel `MAS BP Scuttle Bridge` on the bridge aft wall at [X +1, Z +8, Y = L4].

#### Terminal Configuration & Sliders
* Panel: `MAS BP Scuttle Bridge`
  * Button 1: Timer Block `MAS Timer Scuttle` -> Start (Initiate 60 s Countdown)
  * Button 2: Timer Block `MAS Timer Scuttle` -> Stop (Emergency Abort)
  * Button 3: Group `MAS Warheads Scuttle` -> Safety OFF (Manual Arm)
  * Button 4: Group `MAS Warheads Scuttle` -> Safety ON (Manual Disarm)
* Timer Block: `MAS Timer Scuttle`
  * Delay: `60.0 s`
  * Actions at T = 0 s: `MAS Warhead Scuttle Bridge` -> Detonate; `MAS Warhead Scuttle Power` -> Detonate

**✓ Check:** Warhead disarmed by default; button panel triggers countdown; manual abort halts timer and returns warheads to safety lock.

---

### Step 10.26 · Physical In-Situ Interfaces (Sci-Fi 4-Button Panels) (L1-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Physical Foot Control Network  ·  Levels: L1 to L6              │
│ [VIEWPORT] Grid Zone: 6 Cardinal Crew Stations across Consist                        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.26 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sci-Fi 4-Button Panel | 6 | Slot 9 (`#F1C40F`) | 0.6 t / 30 | Distributed across ground skirts, airlocks, workshop, flight deck, stern |

#### Master Button Panel Mapping Table
| Terminal Name | Grid Coordinates | Functional Location | Button 1 Action | Button 2 Action | Button 3 Action | Button 4 Action |
|---|---|---|---|---|---|---|
| `MAS BP Ground Boarding`| [X −4, Z +7, L1] | Port lower skirt | Ladder Lower/Raise | Keel Lights ON/OFF | Outer Airlock Cycle | Radio Beacon Toggle |
| `MAS BP Airlock Vestibule`| [X −2, Z +7, L3] | Bridge airlock entry | Outer Door Cycle | Inner Door Cycle | Cabin Pressurize | EVA Locker Refill |
| `MAS BP Power Vault` | [X −2, Z −1, L3] | Jefferies tube wall | Vault Door Toggle | Reactor SCRAM | H2 Engine Cold-Start | Battery Shore Charge |
| `MAS BP Field Workshop` | [X0, Z −14, L3] | Utility central aisle | Assembler Queue Plates | Refinery Flush | O2/H2 Gen Toggle | Workshop Lights |
| `MAS BP Flight Deck` | [X +2, Z −10, L6]| Roof hatch landing | Mule Clamps Toggle | Mule Conn Lock | Kestrel Welders Pulse | Deck Floodlights |
| `MAS BP Stern Dump` | [X +2, Z −25, L2]| Rear dump platform | Stern Conn Lock | Sorter Drain All | Land Train Ping | Rear Worksite Light |

**✓ Check:** All 6 button panels powered, named, and mapped to local subsystem toggles for foot operation.

---

### Step 10.27 · Retractable Port Crew Boarding Ladder & Proximity Walkway (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Boarding Ladder & Drive Interlock  ·  Level: L2                 │
│ [VIEWPORT] Grid Zone: [X −4, Z +7, Y = L2]  ·  Looking Bow (+Z) Profile               │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.27 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Motor Hinge | 1 | Slot 9 (`#F1C40F`) | 3.5 t / 15 | `MAS Crew Ladder Hinge` mounted flush in Port hull skirt at [X −4, Z +7, L2] |
| Narrow Ladder Block | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 2 | Attached to hinge head, extending downward when deployed |
| Sensor | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Sensor Ladder` set to 2.5 m crew proximity detection radius |

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

#### Placement Instructions
1. Install Large Motor Hinge `MAS Crew Ladder Hinge` flush in the Port hull skirt at [X −4, Z +7, Y = L2]. Orient barrel horizontally along Z-axis so it pivots outward/downward.
2. Attach 2 Narrow Ladder blocks to the hinge head.
3. Install Sensor `MAS Sensor Ladder` calibrated to a 2.5 m radius covering the Port flank ground zone.

#### Terminal Configuration & Sliders
* Hinge: `MAS Crew Ladder Hinge` (Limits: `0.0°` flush stowed to `90.0°` down · Torque: `10 MN·m`)
* Sensor: `MAS Sensor Ladder` (Detect Players: `ON` · Range: `2.5 m`)
* Event Controller: `MAS EC Ladder`
  * Condition: Player within 2.5 m AND Speed $= 0\text{ km/h}$ -> Deploy Ladder (90°)
  * Condition: Handbrake OFF OR Speed $> 0\text{ km/h}$ -> Retract Ladder (0° Flush)

**✓ Check:** Ladder automatically swings down to ground when engineer approaches on foot; auto-retracts flush upon releasing handbrake.

---

### Step 10.28 · Bow Multitool Repair Turret, CIWS Gatling Turret & Flank Sockets (L2, L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Locomotive Defense & Forward Repair  ·  Levels: L2 & L5        │
│ [VIEWPORT] Grid Zone: Bow Crown [X0, Z +7 to +9, L5]; Flank Skirts [X ±4, Z +2, L2]  │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.28 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Laser Multitool Turret | 1 | Slot 3 (`#E07A5F`) | 4.5 t / 225 | `MAS MT Turret Bow` mounted on forward crown at [X0, Z +9, L5] · Mod 3175774286 |
| Large Gatling Turret | 1 | Slot 7 (`#1B263B`) | 4.8 t / 125 | `MAS Turret Bow` mounted on dorsal crown at [X0, Z +7, L5] |
| Interior Turret | 2 | Slot 7 (`#1B263B`) | 0.8 t / 50 | `MAS Turret Flank Port/Stbd` mounted flush on skirts at [X ±4, Z +2, L2] |

#### Placement Instructions
1. Mount Large Grid Gatling Turret `MAS Turret Bow` on the centerline dorsal crown of Carriage 1 at [X0, Z +7, Y = L5]. Plumb bottom port to tractor conveyor spine at L4.
2. Mount 2 Interior Turrets flush on the lower armored skirts at [X −4, Z +2, Y = L2] (Port) and [X +4, Z +2, Y = L2] (Starboard).
3. Connect ammo feed lines to the main conveyor trunk to draw NATO 25×184mm and 5.56mm ammo from internal storage.
4. Mount Large Grid Laser Multitool Turret `MAS MT Turret Bow` on the forward crown directly ahead of the bridge glass at [X0, Z +9, Y = L5]. Plumb bottom port into the conveyor spine at L4. Set default mode to Welder for forward hull and tire self-repair.

#### Terminal Configuration & Sliders
* Group: `MAS Turrets Defense` (`MAS Turret Bow`, `MAS Turret Flank Port`, `MAS Turret Flank Stbd`)
* Group: `MAS Multitool Turrets` (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`)
* Event Controller: `MAS EC Combat`
  * Condition: Hull Damage Detected OR Hostile Lock Acquired
  * Action Slot 1: Elevate `RIG.OS.MODE CBT` -> Power ON Turrets -> Seal Compartment Doors

---

#### Pre-Enclosure Inspection Gate 10.4A: Bridge Airtightness & C2 Network Link Check
* [ ] Confirm bridge cabin pressurizes to 100% O2 via air vent.
* [ ] Verify `MAS Ore Detector` displays subterranean ore deposits out to 150 m range.
* [ ] Confirm CTC block maps mouse yaw to `MAS Turret Azimuth` and mouse pitch to `MAS Shoulder Pitch`.
* [ ] Confirm `MAS Comm Laser` and `MAS Comm Radio` establish two-way IGC telemetry ping.
* [ ] Verify `MAS Crew Ladder Hinge` automatically lowers when approached on foot and retracts upon handbrake release.
* [ ] Verify `MAS Turret Bow` and `MAS Turret Flank Port/Stbd` receive NATO 25×184mm ammunition through conveyor spine.
* [ ] Verify `MAS MT Turret Bow` receives components and auto-repairs forward armor and tires.

```
PHASE 10-4 VISUAL MILESTONE · C2 BRIDGE & AVIONICS COMPLETE
       Z: +7 (Crown CIWS)     Z: +9 (Bow Repair MT)     Z: +10 (3-Seat Bridge)    Z: +12 (Glass Bow)
       [MAS TURRET BOW]       [MAS MT TURRET BOW]       [HELM / BOOM / FLEET]     [45° PANORAMIC CANOPY]
             │                         │                         │                        │
       ══════╧═════════════════════════╧═════════════════════════╧════════════════════════╧══════
                    C2 COMPUTE RACK & ORE DETECTOR IN SUB-DECK (L3)
```

---

## Phase 10-5 · Modular FOB & Utility Carriage Chassis & Workshop (Carriage 2)

```
       [PHASE 10-5 MASTER SPATIAL LOCATOR HUD]
       Envelope: 11 W × 18 L × 5 H B (27.5 × 45.0 × 12.5 m)
       Coordinate Range: X −5 to +5, Z −26 to −8, Y = L1 to L5
       Carriage Datums:
       · Front Drawbar Hitch Receiver: [X0, Z −8, Y = L2]
       · Axle 4 (Mid Drive Suspension): [X ±5, Z −13, Y = L1] (0.0° Steer)
       · Sub-Floor Battery Vault: [X −1 to +1, Z −16, Y = L2] (9.0 MWh)
       · Field Workshop Core: [X −3 to +3, Z −18 to −12, Y = L3]
       · Axle 5 (Aft Counter-Steer Suspension): [X ±5, Z −22, Y = L1] (−25.0° Steer)
       · Component Hold (3×3×3 Large Cargo): [X0, Z −22, Y = L3-L4]
       · Stern Caterpillar Dump Bumper: [X0, Z −26, Y = L2]
```

### Pre-Step Master Parts Tray: Phase 10-5
* 240× Heavy Armor Full Block (Palette 0: Industrial Dark Gray)
* 40× Heavy Armor Ballast Plates (Palette 0: Keel CoM Balancer)
* 4× Wheel Suspension 5×5 (2 Port, 2 Starboard · Palette 0)
* 3× Large Battery (`MAS Battery 4, 5, 6`, 9.0 MWh total, Palette 6: High-Voltage Red)
* 2× Magnetic Plate (`MAS Utility Brake Keel 1, 2`, Palette 0)
* 1× Basic Refinery (`MAS Field Refinery`, 1×1×2 B, 330 kW, Palette 4: Processing Teal)
* 1× Large Assembler (`MAS Utility Assembler`, Palette 4: Processing Teal)
* 1× Medical Room (`MAS Medical Bay`, Palette 7: Fleet Command Navy)
* 2× Cryo Chamber (`MAS Cryo Pod 1, 2`, Palette 7: Fleet Command Navy)
* 2× Large Oxygen Tank (`MAS Tank O2 1, 2`, 200,000 L O2, Palette 4: Processing Teal)
* 1× Small Cargo Container (`MAS Hold Ice LifeSupport`, 15,000 L Quarantined Ice, Palette 4)
* 2× Large Hydrogen Tank (`MAS Tank H2 1, 2`, 10,000,000 L H2, Palette 6)
* 2× O2/H2 Generator (`MAS Gen O2/H2 1, 2`, Palette 4)
* 2× Air Vent (`MAS Vent Atmo Intake Port/Stbd`, Palette 0: Ambient Intake)
* 1× Large Cargo Container (3×3×3 B, `MAS Hold Components`, Palette 5: Logistics Emerald)
* 1× Large Projector (`MAS Projector Master`, Palette 7: Fleet Command Navy)
* 1× Programmable Block (`[MAS-U] PB Utility`, Palette 7)
* 1× Remote Control (`MAS-U Remote FOB`, Palette 7)
* 2× Large Conveyor Sorter (`MAS Sorter Ammo Fwd`, `MAS Sorter Parts Aft`, Palette 4)
* 2× Sliding Door (`MAS Door Gangway Aft`, `MAS Door Stern Airlock`, Palette 9)
* 14× Grated Catwalk Block, 6× Narrow Ladder Block (Palette 9: High-Viz Safety Yellow)

---

### Step 10.29 · Utility Carriage Box Girders, Keel Ballast Skids & Breakaway Deadman (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Utility Chassis Box Girders & Ballast  ·  Level: L1-L2          │
│ [VIEWPORT] Grid Zone: [X −5 to +5, Z −26 to −8]  ·  Plan view Y = L1-L2 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.29 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 72 | Slot 0 (`#2B303A`) | 61.2 t / 72 | Longitudinal twin box girders at X ±1 from Z −8 to −26; transverse ties at Z −8, −13, −18, −22, −26 |
| Heavy Armor Full Block (Ballast) | 40 | Slot 0 (`#2B303A`) | 120.0 t / 40 | Sub-floor keel ballast plates between girders along L1 from Z −12 to −22 |
| Magnetic Plate (Large) | 2 | Slot 0 (`#2B303A`) | 5.8 t / 50 | `MAS Utility Brake Keel 1, 2` at [X ±2, Z −18, Y = L1] facing downward to terrain |
| Radio Antenna | 1 | Slot 7 (`#1B263B`) | 0.8 t / 25 | `MAS-FOB-01 [EMERGENCY BREAKAWAY]` in protected keel recess [X0, Z −18, L1] |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS-U EC Breakaway` at [X0, Z −10, L2] monitoring drawbar hitch handshake |

```
Plan view, Y = L1-L2 (Utility Chassis Frame, Ballast Plates & Ground Keel Skids)
        X: −3           X: −1      0      X: +1           X: +3
    Z:  −8 ┌───────────────┬───────╤───────┬───────────────┐
           │ AFT HITCH TONGUE RECEIVER & DRAWBAR INTERFACE │  ◄── Y = L2 Drawbar Tongue
    Z: −10 ├───────────────┼───────┴───────┼───────────────┤
           │               │ [MAS-U EC BRK]│               │  ◄── Breakaway Event Controller
    Z: −12 ├───────────────┼───────────────┼───────────────┤
           │ [BALLAST 1]   │ 40× HEAVY ARMOR BALLAST       │  ◄── 120 t Low-Slung Keel Deadweight
    Z: −15 │ [BALLAST 2]   │ CO-LOCATED BETWEEN GIRDERS    │      Lowers CoM to Level L2.4
    Z: −18 │ [KEEL SKID 1] │ [BREAKAWAY BEACON] [KEEL 2]   │  ◄── Downward Ground Clamping Skids
    Z: −20 │ [BALLAST 3]   │ EXPANDS ROLLOVER TO 34°       │
    Z: −22 ├───────────────┼───────────────┼───────────────┤
           │               │ AFT SUSPENSION TIE BEAM       │  ◄── Axle 5 Cross-Member
    Z: −26 └───────────────┴───────────────┴───────────────┘  ◄── Stern Collision Push Bumper
```

#### Placement Instructions
1. Establish longitudinal box girders at X = −1 and X = +1 spanning continuously from Z = −8 to Z = −26 at Level L2.
2. Fabricate transverse cross-ties at Z = −8 (tongue header), Z = −13 (Axle 4 bolster), Z = −18 (mid-keel bolster), Z = −22 (Axle 5 bolster), and Z = −26 (stern push-bumper).
3. Lay 40× Heavy Armor Full Blocks into the Level L1 central trough between X = −1 and X = +1 from Z = −12 to Z = −22. This concentrated 120.0 t ballast counteracts the elevated roof Mule cradle pad (15.0 m above terrain) and lowers Carriage 2 Center of Mass from L3.8 to L2.4.
4. Mount 2× Large Grid Magnetic Plates (`MAS Utility Brake Keel 1, 2`) at [X −2, Z −18, Y = L1] and [X +2, Z −18, Y = L1] pointing directly downward toward ground voxels.
5. Install Radio Antenna `MAS-FOB-01 [EMERGENCY BREAKAWAY]` in the armored keel pocket at [X0, Z −18, Y = L1].
6. Place Event Controller `MAS-U EC Breakaway` at [X0, Z −10, Y = L2] inside the forward chassis sill.

#### Terminal Configuration & Sliders
* Magnetic Plates: `MAS Utility Brake Keel 1, 2` (Auto-Lock: `OFF` · Proximity Sensor: `ON` · Lock Strength: `100%`)
* Antenna: `MAS-FOB-01 [EMERGENCY BREAKAWAY]` (Broadcast Radius: `50 km` · Enable Broadcasting: `OFF` · Show in HUD: `ON`)
* Event Controller: `MAS-U EC Breakaway`
  - Monitored Event: `Grid Connection / Handshake`
  - Monitored Target: `MAS Hitch Sever Tongue` or Carriage 1 electrical bus
  - Condition: Connection lost (0.0 V terminal handshake)
  - Slot 1 (Severance Detected):
    - Group `MAS Utility Wheels` -> `Power OFF`
    - Group `MAS Utility Wheels` -> `Handbrake ON`
    - `MAS Utility Brake Keel 1, 2` -> `Auto-Lock ON`
    - `MAS Utility Brake Keel 1, 2` -> `Lock`
    - `MAS-FOB-01 [EMERGENCY BREAKAWAY]` -> `Enable Broadcasting ON`
  - Slot 2 (Normal Coupled): Keel Skids `Auto-Lock OFF` · Keel Skids `Unlock` · Antenna `Broadcasting OFF`

✓ Check: Keel ballast plates lower CoM to Level L2.4; magnetic keel skids clear ground plane by 0.5 m under normal ride height; breakaway event controller engages emergency lockdown on simulated drawbar disconnect.

---

### Step 10.30 · Axles 4 & 5 Suspension Wheels & DPU Smart Traction / Braking (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Utility Powertrain & DPU Kinematics  ·  Level: L1               │
│ [VIEWPORT] Grid Zone: [X ±5, Z −13 and Z −22]  ·  Plan view Y = L1 (▲ Bow +Z)         │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.30 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Wheel Suspension 5×5 (Port) | 2 | Slot 0 (`#2B303A`) | 19.8 t / 200 | `MAS Wheel L4 (Port)` at [X −5, Z −13, L1] and `MAS Wheel L5 (Port)` at [X −5, Z −22, L1] |
| Wheel Suspension 5×5 (Stbd) | 2 | Slot 0 (`#2B303A`) | 19.8 t / 200 | `MAS Wheel R4 (Stbd)` at [X +5, Z −13, L1] and `MAS Wheel R5 (Stbd)` at [X +5, Z −22, L1] |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC DPU Drive` at [X0, Z −11, L2] coordinating tractive push & descent tension |

```
Plan view, Y = L1 (Utility Carriage 4-Wheel Powertrain Geometry)
        X: −5          X: −1      0      X: +1          X: +5
    Z: −13 [WH 7 (PORT)] ═════╦═══════════╦═════ [WH 8 (STBD)]  ◄── Axle 4 (0.0° Locked Drive)
           │                  │           │                  │
    Z: −17 │                  │   KEEL    │                  │
           │                  │  BALLAST  │                  │
    Z: −22 [WH 9 (PORT)] ═════╩═══════════╩═════ [WH 10 (STBD)] ◄── Axle 5 (−25.0° Counter-Steer)
```

#### Detailed Axle Kinematics Specification
| Axle Index | Z-Datum | Wheel Pair | Steering Angle | Steering Invert | Propulsion Torque | Strength (Empty / Laden) | Friction |
|---|---|---|---|---|---|---|---|
| **Axle 4** | Z = −13 | `WH 7, 8`  | `0.0°`  | OFF (Locked) | `100%` | `50% / 90%` | `75%` |
| **Axle 5** | Z = −22 | `WH 9, 10` | `25.0°` | INVERTED     | `100%` | `55% / 90%` | `80%` |

#### Placement Instructions
1. Weld 5×5 Wheel Suspension units at [X −5, Z −13, Y = L1] (Port) and [X +5, Z −13, Y = L1] (Starboard), designating this assembly as Axle 4. Configure as fixed drive (0.0° steering angle).
2. Weld 5×5 Wheel Suspension units at [X −5, Z −22, Y = L1] (Port) and [X +5, Z −22, Y = L1] (Starboard), designating this assembly as Axle 5. Configure with inverted counter-steering (−25.0°).
3. Assign all 4 suspension blocks to terminal group `MAS Utility Wheels`.
4. Install Event Controller `MAS EC DPU Drive` at [X0, Z −11, Y = L2].

#### Terminal Configuration & Sliders
* Group: `MAS Utility Wheels`
  - Max Speed: `50 km/h`
  - Speed Limit: `ON`
  - Propulsion: `100%`
  - Brake Multiplier: `100%`
* Dynamic Traction & Braking Rules (`MAS EC DPU Drive` / `[MAS] PB Excavator`):
  - Hill-Climb Traction: When ascending grades > 5°, Axles 4 and 5 apply 100% matching tractive push synchronized with Tractor throttle, distributing load evenly across all 10 wheels.
  - Dynamic Tension Braking: When descending grades or decelerating, Carriage 2 wheels dynamically apply counter-braking torque 100 ms prior to Tractor braking bite, keeping the hitch in continuous longitudinal tension and preventing trailer jackknifing.
  - Automatic Traction Control (ATC): On loose voxel detection, wheel torque drops by 20% increments until slip ceases.

✓ Check: Axle 4 tracks dead-ahead; Axle 5 pivots opposite to Tractor front steering wheels; consist achieves 35 m turning radius without trailer scrubbing or wheel lift.

---

### Step 10.31 · Sub-Floor Utility Battery Vault & Shore Power Bus (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Sub-Floor Utility Battery Vault  ·  Level: L2                   │
│ [VIEWPORT] Grid Zone: [X −1 to +1, Z −17 to −15]  ·  Plan view Y = L2 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.31 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Battery | 3 | Slot 6 (`#C0392B`) | 25.5 t / 240 | `MAS Battery 4, 5, 6` in sub-floor well at [X −1, 0, +1, Z −16, L2] |
| Heavy Armor Full Block | 12 | Slot 0 (`#2B303A`) | 10.2 t / 12 | Armored vault perimeter enclosure and structural top ceiling deck |

```
Plan view, Y = L2 (Utility Sub-Floor Battery Vault & Electrical Spine)
        X: −1             0             X: +1
    Z: −15 ┌─────────────────────────────────────────┐
           │ HEAVY ARMOR VAULT BULKHEAD (Ceiling L3) │
    Z: −16 │ [MAS BATTERY 4] [BATTERY 5] [BATTERY 6] │  ◄── 9.0 MWh Energy Storage Buffer
    Z: −17 │ (3.0 MWh)       (3.0 MWh)   (3.0 MWh)   │      36.0 MW Peak Discharge Bus
           └─────────────────────────────────────────┘
```

#### Placement Instructions
1. Excavate and frame a 3-block wide by 1-block long by 1-block deep recessed bay at [X −1 to +1, Z −16, Y = L2] directly beneath the Level L3 workshop floor.
2. Install 3× Large Batteries side-by-side:
   - `MAS Battery 4` at [X −1, Z −16, Y = L2]
   - `MAS Battery 5` at [X0, Z −16, Y = L2]
   - `MAS Battery 6` at [X +1, Z −16, Y = L2]
3. Enclose the battery well perimeter with Heavy Armor Full Blocks. The top faces form the foundation walkway for Level L3.
4. Tie battery terminals into the consist high-voltage tractive bus, supplying instantaneous float charge to the Basic Refinery, Large Assembler, and Axles 4-5 drive motors.

#### Terminal Configuration & Sliders
* Batteries: `MAS Battery 4, 5, 6`
  - Initial Mode: `Auto`
  - Max Stored Energy: `3.0 MWh each (9.0 MWh vault total)`
  - Peak Power Output: `12.0 MW each (36.0 MW vault total)`
  - Low Power Threshold: `15%` (Triggers audio alert on Bridge console)
* Consist Power Totals: Vault brings total Mastodon MKI energy storage to 18.0 MWh across 6 Large Batteries with 72.0 MW combined instantaneous discharge.

✓ Check: Batteries deliver 36.0 MW peak output; total consist energy reaches 18.0 MWh; battery well remains fully armored beneath workshop floor plate.

---

### Step 10.32 · Mobile FOB Field Workshop Core, Medical Bay & Life Support (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: FOB Workshop Core & Life Support  ·  Level: L3                  │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z −18 to −12]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.32 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Assembler | 1 | Slot 4 (`#16A085`) | 39.5 t / 40 | `MAS Utility Assembler` at [X −2, Z −14, L3]; dual inventory doors face aisle |
| Medical Room | 1 | Slot 7 (`#1B263B`) | 12.8 t / 30 | `MAS Medical Bay` at [X +2, Z −14, L3] plumbed to O2 life support trunk |
| Cryo Chamber | 2 | Slot 7 (`#1B263B`) | 3.2 t / 40 | `MAS Cryo Pod 1, 2` at [X +2, Z −12 to −13, L3] facing central aisle |
| Large Oxygen Tank | 2 | Slot 4 (`#16A085`) | 6.4 t / 50 | `MAS Tank O2 1, 2` at [X −2, Z −16 to −17, L3-L4]; stores 200,000 L O2 |
| Small Cargo Container | 1 | Slot 4 (`#16A085`) | 0.8 t / 10 | `MAS Hold Ice LifeSupport` at [X0, Z −16, L3]; 15,000 L Quarantined Ice |
| Large Hydrogen Tank | 2 | Slot 6 (`#C0392B`) | 16.2 t / 50 | `MAS Tank H2 1, 2` at [X ±2, Z −18, L3]; stores 10,000,000 L H2 fuel |
| O2/H2 Generator | 2 | Slot 4 (`#16A085`) | 5.4 t / 44 | `MAS Gen O2/H2 1, 2` at [X0, Z −18, L3]; converts ice to fuel and air |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Gangway Aft` at [X0, Z −9, L3] sealing forward workshop bulkhead |

```
Plan view, Y = L3 (Mobile FOB Workshop Core, Medical Bay & Life Support Farm)
        X: −3           X: −2           0           X: +2           X: +3
    Z:  −9 ┌───────────────────────────[DOOR GANGWAY AFT]──────────────────────────┐
    Z: −10 │ FORWARD WORKSHOP ENTRY & FLIGHT LADDERWELL AISLE                      │
    Z: −12 │ [REFINERY L3]             │ [AISLE]    │ [MAS CRYO POD 1]             │
    Z: −13 │ (Smelting Core)           │ [AISLE]    │ [MAS CRYO POD 2]             │
    Z: −14 │ [MAS UTILITY ASSEMBLER]   │ [AISLE]    │ [MAS MEDICAL BAY]            │
    Z: −15 │ (Dual Ports Face Aisle)   │ [AISLE]    │ (Trauma Respawn Station)     │
    Z: −16 │ [MAS TANK O2 1]           │ [ICE HOLD] │ [MAS TANK O2 2]              │
    Z: −17 │ (100,000 L O2)            │ (15,000 L) │ (100,000 L O2)               │
    Z: −18 │ [MAS TANK H2 1 (5,000kL)] │ [O2/H2 1,2]│ [MAS TANK H2 2 (5,000kL)]    │
           └───────────────────────────┴────────────┴──────────────────────────────┘
```

#### Placement Instructions
1. Install Sliding Door `MAS Door Gangway Aft` at [X0, Z −9, Y = L3], providing the hermetic barrier between the accordion drawbar gangway and the workshop cabin.
2. Install Large Assembler `MAS Utility Assembler` at [X −2, Z −14, Y = L3] oriented so its dual physical conveyor/inventory doors face the central aisle at X0.
3. Install Medical Room `MAS Medical Bay` at [X +2, Z −14, Y = L3] with conveyor port connected to the life support trunk.
4. Mount 2× Cryo Chambers (`MAS Cryo Pod 1, 2`) at [X +2, Z −12, L3] and [X +2, Z −13, L3] facing inward toward the central walkway.
5. Install 2× Large Oxygen Tanks (`MAS Tank O2 1, 2`) at [X −2, Z −16, L3] and [X +2, Z −16, L3].
6. Place Small Cargo Container `MAS Hold Ice LifeSupport` at [X0, Z −16, Y = L3] directly between the O2 tanks, isolated by a one-way conveyor check to safeguard emergency ice.
7. Install 2× Large Hydrogen Tanks (`MAS Tank H2 1, 2`) at [X −2, Z −18, L3] and [X +2, Z −18, L3] with twin O2/H2 Generators (`MAS Gen O2/H2 1, 2`) mounted at [X0, Z −18, L3].

#### Terminal Configuration & Sliders
* Assembler: `MAS Utility Assembler` (Cooperative Mode: `ON` · Repeat: `OFF`)
* Medical Bay: `MAS Medical Bay` (Respawn: `Enabled` · Healing: `Enabled`)
* Cryo Chambers: `MAS Cryo Pod 1, 2` (Conveyor Connection: `ON`)
* Oxygen Tanks: `MAS Tank O2 1, 2` (Auto-Refill: `ON` · Stockpile: `OFF` · Total Volume: `200,000 L`)
* Hydrogen Tanks: `MAS Tank H2 1, 2` (Stockpile: `OFF` · Total Volume: `10,000,000 L`)
* Ice Buffer: `MAS Hold Ice LifeSupport` (Custom Name: `[QUARANTINED] MAS Hold Ice LifeSupport`)

✓ Check: Workshop core maintains 100% airtight pressure; assembler inventory accessible from central aisle; medical bay and cryo chambers operational; ice buffer quarantined.

---

### Step 10.33 · Closed-Loop In-Situ Field Metallurgy & Component Recycling Circuit (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Closed-Loop Field Metallurgy & Reclamation  ·  Level: L2-L3    │
│ [VIEWPORT] Grid Zone: [X −3 to 0, Z −17 to −9]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.33 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Basic Refinery | 1 | Slot 4 (`#16A085`) | 13.5 t / 90 | `MAS Field Refinery` at [X −2, Z −12, L3]; top manual hopper accessible from aisle |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS Sorter Ammo Fwd` at [X0, Z −9, L3] pointing forward (+Z) toward Tractor spine |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS Sorter Parts Aft` at [X0, Z −17, L2] pointing aft (−Z) toward workshop |
| Large Conveyor Pipe | 6 | Slot 4 (`#16A085`) | 1.8 t / 6 | Interconnecting refinery, assembler, and sorting raceways |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Ice` at [X0, Z −15, L3] prioritizing raw ice flow to O2/H2 generators |

```
[SALVAGE EXTRACTION & CLOSED-LOOP RECYCLING CIRCUIT]

   [PIRATE WRECK / MINED VEIN]
         │
         ▼ (Laser Multitool Head · Drill or Grinder Beam)
   [MAS LASER 1, 2]
         │
         ▼ (Articulated Boom Conveyor Raceway)
   [CARRIAGE 1 CENTRAL CONVEYOR SPINE]
         │
         ▼ (Drawbar Flexible Inter-Carriage Conduit)
   ┌─────────────────────────────────────────────────────────────────┐
   │ CARRIAGE 2 FOB RECLAMATION CORE                                 │
   │                                                                 │
   │ ┌──────────────────────┐        ┌────────────────────────────┐  │
   │ │ INTACT SPARE PARTS   │───────►│ MAS HOLD COMPONENTS (3×3×3)│  │
   │ └──────────────────────┘        └─────────────┬──────────────┘  │
   │                                               │ (Munitions Pull)│
   │ ┌──────────────────────┐                      ▼                 │
   │ │ DAMAGED SCRAP METAL  │──►[MAS FIELD REFINERY]                 │
   │ └──────────────────────┘   (330 kW Iron Smelter)                │
   │                                    │ (Ingots)                   │
   │ ┌──────────────────────┐           ▼                            │
   │ │ OBSOLETE COMPONENTS  │──►[MAS UTILITY ASSEMBLER]              │
   │ └──────────────────────┘   (Disassemble / Ammo Fabrication)     │
   │                                    │                            │
   │                                    ▼                            │
   │                            [MAS SORTER AMMO FWD] ──► (CIWS Guns)│
   └─────────────────────────────────────────────────────────────────┘
```

#### Placement Instructions
1. Install Basic Refinery `MAS Field Refinery` at [X −2, Z −12, Y = L3] immediately forward of the Large Assembler. Plumb bottom and side conveyor ports to the central conveyor spine. Ensure top hatch faces inward toward the aisle for manual batch dumping.
2. Install Large Conveyor Sorter `MAS Sorter Ammo Fwd` at [X0, Z −9, Y = L3] oriented pointing forward (+Z) directly into the gangway drawbar conduit. Configure whitelist for NATO 25×184mm and 5.56mm munitions with Drain All = ON to keep Tractor turrets continuously supplied.
3. Install Large Conveyor Sorter `MAS Sorter Parts Aft` at [X0, Z −17, Y = L2] oriented pointing aft (−Z). Configure whitelist for Scrap Metal and damaged salvage components with Drain All = ON, diverting incoming salvage directly into `MAS Field Refinery` before it reaches raw ore bins.
4. Mount Event Controller `MAS EC Ice` at [X0, Z −15, Y = L3] on the central bulkhead.

#### Terminal Configuration & Sliders
* Basic Refinery: `MAS Field Refinery` (Use Conveyor System: `ON` · Smelting: Fe, Ni, Co, Si, Mg)
* Sorter: `MAS Sorter Ammo Fwd` (Mode: `Whitelist` · Items: `NATO_25x184mm`, `NATO_5_56x45mm` · Drain All: `ON`)
* Sorter: `MAS Sorter Parts Aft` (Mode: `Whitelist` · Items: `Scrap` · Drain All: `ON`)
* Event Controller: `MAS EC Ice`
  - Monitored Target: `MAS Tank H2 1, 2` (Hydrogen Fill Level)
  - Condition: Fill $< 90\%$
  - Action Slot 1: Enable Ice Sorter & Power ON `MAS Gen O2/H2 1, 2`
  - Action Slot 2: Standby mode when tanks $\ge 95\%$

✓ Check: Mined raw ore automatically smelts into ingots; scrap metal routes straight to refinery; assembler pulls ingots autonomously; forward sorter keeps bow and flank turrets fully loaded with ammo.

---

### Step 10.34 · Vertical Flight Ladderwell & Stern Maintenance Descent (L2-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Crew Inter-Deck Vertical Circulation  ·  Level: L2-L6           │
│ [VIEWPORT] Grid Zone: [X +2, Z −25 to −10]  ·  Profile view facing Port (▲ Zenith +Y)│
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.34 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Narrow Ladder Block | 6 | Slot 9 (`#F1C40F`) | 0.6 t / 6 | 4× ladder segments at [X +2, Z −10, L3-L5]; 2× segments at [X +2, Z −25, L2-L3] |
| Sliding Door | 2 | Slot 9 (`#F1C40F`) | 2.4 t / 30 | `MAS Door Flight Deck Airlock` at [X +2, Z −10, L5]; `MAS Door Stern Airlock` at [X +2, Z −25, L3] |
| Air Vent | 2 | Slot 7 (`#1B263B`) | 1.0 t / 50 | `MAS Vent Airlock Roof` at [X +2, Z −10, L5]; `MAS Vent Airlock Stern` at [X +2, Z −25, L3] |
| Sci-Fi 4-Button Panel | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 16 | `MAS BP Flight Deck` at [X +2, Z −10, L6]; `MAS BP Stern Dump` at [X +2, Z −25, L2] |

```
Profile elevation looking Starboard, X = +2 (Dual Vertical Airlock Circulation)
       Z: −10 (Forward Flight Deck Ascent)             Z: −25 (Aft Stern Dump Descent)
  L6   [HATCH] ── [MAS BP FLIGHT DECK]                 ═══════════════════════════════
       │                                                                │
  L5   [AIRLOCK DOOR] · [MAS VENT AIRLOCK ROOF]        ═══════════════════════════════
       │ (Vertical Ladderwell)                                          │
  L4   │                                               ═══════════════════════════════
       │                                                                │
  L3   [WORKSHOP ENTRY THRESHOLD]                      [AIRLOCK DOOR] · [MAS VENT STERN]
                                                       │ (Vertical Ladderwell)
  L2   ═══════════════════════════════════════════════ [HATCH] ── [MAS BP STERN DUMP]
```

#### Placement Instructions
1. Install an enclosed vertical ladder trunk at [X +2, Z −10] ascending from the forward workshop floor (Level L3) through Level L4 up to Level L5.
2. Install Sliding Door `MAS Door Flight Deck Airlock` and Air Vent `MAS Vent Airlock Roof` at Level L5, forming a sealed airlock chamber before the roof hatch at Level L6. Mount button panel `MAS BP Flight Deck` on the exterior gantry coaming.
3. Install a descending ladder trunk at [X +2, Z −25] descending from Level L3 down to Level L2 through Sliding Door `MAS Door Stern Airlock`.
4. Install Air Vent `MAS Vent Airlock Stern` at Level L3 inside the descent vestibule and mount button panel `MAS BP Stern Dump` at Level L2 adjacent to the rear dump bumper.

#### Terminal Configuration & Sliders
* Doors: `MAS Door Flight Deck Airlock`, `MAS Door Stern Airlock` (Auto-Close: `ON` · Delay: `2.5 s`)
* Vents: `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern` (Depressurize: `OFF` · Cycle airlock pressure before outer hatch unlatches)
* Button Panels:
  - `MAS BP Flight Deck`: Btn 1 = Cycle Flight Deck Hatch · Btn 2 = Toggle Roof Floodlights · Btn 3 = Mule Cradle Clamp
  - `MAS BP Stern Dump`: Btn 1 = Cycle Stern Hatch · Btn 2 = Toggle Stern Sorter Drain · Btn 3 = Lock/Unlock Stern Conn

✓ Check: Engineer on foot ascends to Level L6 flight deck without venting workshop atmosphere; stern airlock provides safe descent to Level L2 dump bumper in vacuum.

---

### Step 10.35 · Ambient Atmospheric Oxygen Scavenging Vents & Multi-Atmosphere Control (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Universal Life Support & Scavenging  ·  Level: L4              │
│ [VIEWPORT] Grid Zone: [X ±3, Z −16]  ·  Flank profile looking Inward (▲ Zenith +Y)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.35 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Air Vent | 2 | Slot 0 (`#2B303A`) | 1.0 t / 50 | `MAS Vent Atmo Intake Port` at [X −3, Z −16, L4]; `MAS Vent Atmo Intake Stbd` at [X +3, Z −16, L4] |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Atmosphere Mode` at [X0, Z −15, L3] monitoring external atmospheric O2 |
| Heavy Armor Slope | 4 | Slot 0 (`#2B303A`) | 3.4 t / 4 | Recessed protective cowl surrounding exterior intake louvers |

```
Flank elevation cross-section, Y = L4, Z = −16 (Exterior Scavenging Vent Recesses)
    Port Flank (X −3)                                               Starboard Flank (X +3)
    [EXTERIOR HULL]                                                 [EXTERIOR HULL]
          │                                                               │
    ┌─────┴──────────────────┐                               ┌────────────┴─────────────┐
    │ [MAS VENT ATMO PORT]   │ ◄── Depressurize: ON          │ [MAS VENT ATMO STBD]     │
    │ (Recessed Flush Vent)  │     Draws Ambient O2          │ (Recessed Flush Vent)    │
    └─────┬──────────────────┘     Free Ice-Free Life Support└────────────┬─────────────┘
          │                                                               │
          └────────────────────────► [MAS TANK O2 1, 2] ◄─────────────────┘
                                   (200,000 L O2 Bunkers)
```

#### Placement Instructions
1. Cut flush recesses into the Level L4 exterior armor flanks at [X −3, Z −16] (Port) and [X +3, Z −16] (Starboard).
2. Install Air Vents `MAS Vent Atmo Intake Port` and `MAS Vent Atmo Intake Stbd` facing outward toward the planetary environment, plumbed directly into the utility oxygen line.
3. Frame vent openings with Heavy Armor Slopes to protect delicate intake louvers from ballistic ricochets and tree branch strikes during transit.
4. Install Event Controller `MAS EC Atmosphere Mode` at [X0, Z −15, Y = L3] inside the central life support bay.

#### Terminal Configuration & Sliders
* Air Vents: `MAS Vent Atmo Intake Port`, `MAS Vent Atmo Intake Stbd`
  - Depressurize: `ON`
  - Power: `ON` (in breathable atmospheres)
  - Parasitic Draw: `30 kW each`
* Event Controller: `MAS EC Atmosphere Mode`
  - Monitored Event: `Natural Gravity & Atmospheric Oxygen`
  - Target: Ambient Atmosphere
  - Condition: Breathable O2 $\ge 1.0\%$
  - Slot 1 (Breathable Atmosphere):
    - `MAS Vent Atmo Intake Port` -> `Power ON`
    - `MAS Vent Atmo Intake Stbd` -> `Power ON`
    - `MAS Gen O2/H2 1, 2` -> `Power OFF` (Saves ice reserves)
  - Slot 2 (Vacuum / Toxic World):
    - `MAS Vent Atmo Intake Port` -> `Power OFF`
    - `MAS Vent Atmo Intake Stbd` -> `Power OFF`
    - Compartment Doors -> `Auto-Seal ON`
    - Life Support -> Switched to 100% closed-loop tank draw (`MAS Tank O2 1, 2`)

✓ Check: In planetary atmosphere, vents pull free oxygen to refill 200,000 L O2 storage with zero ice consumption; in vacuum, vents automatically power down and hermetic seals isolate all 4 interior zones.

---

### Step 10.36 · Genesis Master Projector, Standalone FOB C2 & Field Component Hold (L3-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Master Blueprint Projector & FOB C2  ·  Level: L3-L4           │
│ [VIEWPORT] Grid Zone: [X −1 to +1, Z −22 to −13]  ·  Plan view Y = L3 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.36 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Cargo Container | 1 | Slot 5 (`#27AE60`) | 23.4 t / 50 | `MAS Hold Components` at [X0, Z −22, L3-L4] (3×3×3 B); terminal port faces aisle |
| Projector (Large Grid) | 1 | Slot 7 (`#1B263B`) | 2.4 t / 100 | `MAS Projector Master` in workshop ceiling at [X0, Z −14, L4] facing forward (+Z) |
| Programmable Block | 1 | Slot 7 (`#1B263B`) | 0.8 t / 25 | `[MAS-U] PB Utility` at [X −1, Z −13, L3] running autonomous FOB watchkeeping logic |
| Remote Control | 1 | Slot 7 (`#1B263B`) | 0.6 t / 25 | `MAS-U Remote FOB` at [X +1, Z −13, L3] facing forward (+Z) for uncrewed remote control |

```
Plan view, Y = L3 (Aft Component Hold, Genesis Projector & Autonomous FOB Core)
        X: −2           X: −1             0             X: +1           X: +2
    Z: −13 ┌───────────────┬───────────────╤───────────────┬───────────────┐
           │ [ASSEMBLER]   │ [MAS-U PB UT] │ [AISLE]       │ [MAS-U REMOTE]│
    Z: −14 │ (Dual Ports)  ├───────────────┴───────────────┤ (Medical Bay) │
           │               │ [MAS PROJECTOR MASTER (L4)]   │               │  ◄── Ceiling Ghost Projection
    Z: −16 ├───────────────┼───────────────────────────────┼───────────────┤
           │ [O2 TANK 1]   │ [QUARANTINED ICE HOLD (15kL)] │ [O2 TANK 2]   │
    Z: −18 ├───────────────┼───────────────────────────────┼───────────────┤
           │ [H2 TANK 1]   │ [CONVEYOR RISER JUNCTION]     │ [H2 TANK 2]   │
    Z: −19 ├───────────────┴───────────────────────────────┴───────────────┤
    Z: −20 │                                                               │
    Z: −22 │             [MAS HOLD COMPONENTS (3×3×3 LARGE CARGO)]         │  ◄── Spare Construction Parts
    Z: −24 │             (Palette Slot 5: Logistics Emerald)               │      Pulls Salvage & Welder Stock
    Z: −25 └───────────────────────────────────────────────────────────────┘
```

#### Placement Instructions
1. Install Large Cargo Container `MAS Hold Components` (3×3×3 Large Grid) centered at [X0, Z −22, Y = L3-L4]. Ensure front lower conveyor port and physical terminal access door face forward into the central workshop aisle at [X0, Z −20, Y = L3].
2. Mount Large Grid Projector `MAS Projector Master` flush inside the Level L4 ceiling at [X0, Z −14, Y = L4] with top facing zenith (+Y) and front facing forward (+Z).
3. Load the complete 1,039-block reference blueprint of `MASTODON-01 MKI` into `MAS Projector Master`. Set coordinate offsets to exactly X = 0, Y = 0, Z = 0, aligning the holographic ghost projection 1:1 with the physical chassis.
4. Mount Programmable Block `[MAS-U] PB Utility` at [X −1, Z −13, Y = L3].
5. Mount Remote Control `MAS-U Remote FOB` at [X +1, Z −13, Y = L3] aligned facing forward (+Z).

#### Terminal Configuration & Sliders
* Projector: `MAS Projector Master`
  - Pitch: `0.0°` · Roll: `0.0°` · Yaw: `0.0°`
  - Offset X: `0` · Offset Y: `0` · Offset Z: `0`
  - Show Only Buildable: `ON`
* Autonomous FOB C2 (`[MAS-U] PB Utility` / `MAS-U Remote FOB`):
  - Standalone Drop-and-Mine Decouple: Upon severance or hitch uncoupling, `[MAS-U] PB Utility` immediately assumes local command authority.
  - Broadcasts regional status `RIG.C2.HEARTBEAT;ORIGIN=MAS-01-U;ROLE=FOB`.
  - Coordinates automated workshop manufacturing, life support recycling, battery management, and rear CIWS defense independently from the Tractor.

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
* [ ] Confirm `[MAS-U] PB Utility` boots into standalone FOB mode upon hitch decouple.
* [ ] Confirm `MAS-U EC Breakaway` engages deadman brake and drops keel skids on signal loss.
* [ ] Verify `MAS Sorter Ammo Fwd` draws ammunition to forward turrets without manual handling.
* [ ] Verify `MAS Sorter Parts Aft` routes scrap metal directly into `MAS Field Refinery`.
* [ ] Verify `MAS MT Turret Port` and `MAS MT Turret Stbd` auto-repair damaged blocks on the drawbar hitch and Kestrel landing pads.
* [ ] Verify `[MAS-U] PB Utility` detects component deficit in `MAS Hold Components` and queues production deltas in `MAS Utility Assembler`.

---

### Step 10.36B · Twin Flank Multitool Repair Turrets & SML Auto-Production Queuing (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Consist Self-Repair & Drone Servicing  ·  Level: L4             │
│ [VIEWPORT] Grid Zone: Forward Shoulders [X ±3, Z −10, L4]  ·  Plan view (▲ Bow +Z)   │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.36B Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Laser Multitool Turret | 2 | Slot 3 (`#E07A5F`) | 9.0 t / 450 | `MAS MT Turret Port/Stbd` on forward shoulders at [X ±3, Z −10, L4] · Mod 3175774286 |
| Conveyor Junction | 2 | Slot 4 (`#16A085`) | 1.8 t / 2 | Sub-deck risers at [X ±3, Z −10, L3] tying into workshop conveyor spine |

```
Plan view, Y = L4 (Carriage 2 Forward Shoulder Repair Turrets & SML Queuing Core)
        X: −3           X: −1             0             X: +1           X: +3
    Z:  −9 ┌───────────────┬───────────────────────────────┬───────────────┐
           │ [PORT SHOULDER│ ════════ [BULKHEAD DOOR] ════ │ [STBD SHOULDER│
    Z: −10 │ [MAS MT PORT] │ [AISLE WALKWAY]               │ [MAS MT STBD] │  ◄── Forward Flank Overlap
           ├───────────────┼───────────────────────────────┼───────────────┤      (Covers Hitch & Kestrels)
    Z: −11 │ [KESTREL PAD] │ [CONVEYOR RISER JUNCTION]     │ [KESTREL PAD] │  ◄── Drone Landing Deck (L5)
           └───────────────┴───────────────────────────────┴───────────────┘
```

#### Placement Instructions
1. Install 2 Conveyor Junctions at [X −3, Z −10, Y = L3] and [X +3, Z −10, Y = L3], tapping into the utility workshop conveyor trunk lines.
2. Mount Large Grid Laser Multitool Turrets `MAS MT Turret Port` at [X −3, Z −10, Y = L4] and `MAS MT Turret Stbd` at [X +3, Z −10, Y = L4] flush on the reinforced shoulder sponsons of Carriage 2.
3. Verify unobstructed 270° hemispherical lines of sight: turrets sweep forward across the 3-axis drawbar hitch (Z −8), inward across the tractor rear armor and power vault, and upward to service docked KESTREL-01 drones on their Level L5 landing pads at [X ±3, Z −11, L5].
4. Plumb internal conveyor lines back to `MAS Hold Components` and `MAS Field Refinery` / `MAS Utility Assembler`.

#### Terminal Configuration & Sliders
* Group: `MAS Multitool Turrets` (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`)
* Multitool Turrets: `MAS MT Turret Port`, `MAS MT Turret Stbd`
  * Default Mode: `Welder` (Field self-repair and drone maintenance)
  * Secondary Mode: `Grinder` (Debris clearance and obstacle salvage)
  * Range: `50 m` · Target Friendly: `ON` · Target Neutrals: `ON` · Auto-Repair: `ON`
* SML Auto-Production Queuing (`[MAS-U] PB Utility`):
  * Baseline SML Quotas: 1,500 Steel Plates, 500 Construction Comps, 200 Motors, 100 NATO 25×184mm ammo boxes, 100 Interior Plates, 50 Large Steel Tubes, 100 Small Steel Tubes, 50 Computers, 50 Displays, 20 Radio-comm Comps.
  * Threshold Watchdog: Whenever inventory in `MAS Hold Components` drops below 75% of quota, `[MAS-U] PB Utility` queues production deltas in `MAS Utility Assembler`.
  * Ingot Feed: `MAS Field Refinery` processes mined ore directly into iron/nickel/silicon ingots, sustaining closed-loop field manufacturing without supply runs.

```
PHASE 10-5 VISUAL MILESTONE · EXPEDITIONARY WORKSHOP & FOB CORE COMPLETE
       Z: −10 (Twin Flank MT Turrets)    Z: −14 (Refinery/Assembler)   Z: −22 (Hold & Projector)
       [MAS MT TURRET PORT / STBD]       [METALLURGY & LIFE SUPPORT]   [3×3×3 COMPONENT HOLD]
                 │                                    │                          │
       ══════════╧════════════════════════════════════╧══════════════════════════╧═════════
                    9.0 MWh SUB-FLOOR BATTERY VAULT & KEEL BALLAST (L1-L2)
```

---

## Phase 10-6 · Flight Decks & Logistics Offload Interfaces (Levels L5-L6)

```
       [PHASE 10-6 MASTER SPATIAL LOCATOR HUD]
       Envelope: 9 W × 18 L × 3 H B (22.5 × 45.0 × 7.5 m)
       Coordinate Range: X −4 to +4, Z −26 to −8, Y = L5 to L6
       Deck Datums:
       · Twin Kestrel Scout Drone Landing Pads: [X ±3, Z −12, Y = L5]
       · Recessed Drone Maintenance Welders: [X ±3, Z −12, Y = L4]
       · 7×7 B Mule Gantry Blast Deck: [X0, Z −22, Y = L6]
       · Mule Rub-Rail Clamping Arms: [X ±3, Z −22, Y = L6]
       · Flush Solar Array: [X ±4, Z −24 to −20, Y = L6]
       · Stern Bumper & Caterpillar Dump Connector: [X0, Z −26, Y = L2]
       · Stern CIWS Defense Turret: [X0, Z −26, Y = L5]
```

### Pre-Step Master Parts Tray: Phase 10-6
* 49× Heavy Armor Full Block (Palette 1: Hazard Amber · Mule Blast Deck)
* 4× Large Grid Solar Panel (`MAS Solar 1..4`, 1×4 B, Palette 0)
* 18× Catwalk with Railing Block (Palette 9: High-Viz Safety Yellow)
* 10× Grated Catwalk Block (Palette 9: High-Viz Safety Yellow)
* 2× Motor Hinge + 2× Large Magnetic Plate (Palette 1: Cradle Clamping Arms)
* 1× Large Connector (`MAS MP Conn`, Palette 4: Processing Teal)
* 2× Large Conveyor Sorter (`MAS MP Sorter`, `MAS Stern Sorter`, Palette 4)
* 2× Small-Grid Docking Connector (`MAS Kestrel Dock 1, 2`, Palette 8: Scout Flight White)
* 2× Large Grid Ship Welder (`MAS Welder Pad Kestrel 1, 2`, Palette 4: Processing Teal)
* 4× Large Grid Spotlight (`MAS Floodlight Tower 1..4`, Palette 7: Fleet Command Navy)
* 1× Large Grid Piston (`MAS Piston Stern Dock`, Palette 2: Rail Crimson)
* 1× Large Connector (`MAS Stern Dump Conn`, Palette 2: Rail Crimson)
* 2× Large Grid Piston (`MAS Outrigger Utility Port/Stbd`, Palette 0)
* 2× Large Magnetic Plate (`MAS Seismic Pad Rear Port/Stbd`, Palette 0)
* 1× Large Conveyor Sorter + 4× Conveyor Ejector (`MAS Sorter Tailings Stern`, Palette 0)
* 1× Large Gatling Turret (`MAS Turret Stern`, Palette 7: Fleet Command Navy)

---

### Step 10.37 · Roof Mule Receiving Gantry Cradle Pad (L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Mule Receiving Gantry Cradle & Sorter  ·  Level: L6             │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z −25 to −19]  ·  Plan view Y = L6 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.37 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 49 | Slot 1 (`#E67E22`) | 41.6 t / 49 | 7×7 B blast deck spanning X −3 to +3, Z −25 to −19 at Level L6 |
| Large Connector | 1 | Slot 4 (`#16A085`) | 4.1 t / 25 | `MAS MP Conn` flush with deck at [X0, Z −22, L6] facing zenith (+Y) |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS MP Sorter` at [X0, Z −22, L5] pointing upward (+Y) into connector |
| Motor Hinge | 2 | Slot 1 (`#E67E22`) | 4.2 t / 200 | `MAS MP Clamp Hinge Port/Stbd` at [X ±3, Z −22, L6] base facing inward |
| Magnetic Plate (Large) | 2 | Slot 1 (`#E67E22`) | 5.8 t / 50 | `MAS MP Clamp Plate Port/Stbd` mounted on hinge heads |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC MP` at [X0, Z −21, L5] managing clamping sequence |

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

#### Placement Instructions
1. Lay a 7×7 block square of Heavy Armor Full Blocks painted Palette Slot 1 (Hazard Amber) from X = −3 to +3 and Z = −25 to −19 at Level L6, forming the heavy landing deck.
2. Cut the center block at [X0, Z −22, Y = L6] and install Large Connector `MAS MP Conn` flush with the deck surface facing upward (+Y).
3. Directly beneath the connector at Level L5, plumb Large Conveyor Sorter `MAS MP Sorter` pointing upward (+Y) into the connector. Configure whitelist for rare ores (Uranium, Platinum, Gold, Magnesium).
4. Mount Motor Hinges `MAS MP Clamp Hinge Port` at [X −3, Z −22, Y = L6] and `MAS MP Clamp Hinge Stbd` at [X +3, Z −22, Y = L6] with bases anchored to the exterior gantry coaming and hinge heads swinging inward toward X0.
5. Weld Large Magnetic Plates `MAS MP Clamp Plate Port/Stbd` onto the hinge heads.
6. Mount Event Controller `MAS EC MP` in the sub-deck cavity at [X0, Z −21, Y = L5].

#### Terminal Configuration & Sliders
* Connector: `MAS MP Conn` (Auto-Connect: `OFF` · Collect All: `OFF` · Strength: `0.015`)
* Conveyor Sorter: `MAS MP Sorter` (Mode: `Whitelist` · Items: `Uranium`, `Platinum`, `Gold`, `Magnesium` · Drain All: `ON`)
* Hinges: `MAS MP Clamp Hinge Port/Stbd`
  - Lower Limit: `0.0° (Stowed Flush)` · Upper Limit: `30.0° (Clamped Inward)`
  - Torque: `10 MN·m` · Braking Torque: `20 MN·m` · Velocity: `1.5 rpm`
* Magnetic Plates: `MAS MP Clamp Plate Port/Stbd` (Auto-Lock: `ON` · Lock Strength: `100%`)
* Event Controller: `MAS EC MP`
  - Monitored Target: `MAS MP Conn`
  - Condition: Status = `Connected`
  - Slot 1 (Touchdown): Hinges reverse to 30.0° inward -> Clamp plates lock Mule rub rails -> `MAS MP Sorter` Drain All ON
  - Slot 2 (Disconnect): Hinges return to 0.0° flush -> `MAS MP Sorter` Drain All OFF

✓ Check: Inbound Mule touches down; cradle arms swing 30° inward and lock rub rails without chassis deformation; rare ores drain at $> 150\text{ t/min}$ into Mule cargo bins.

---

### Step 10.38 · Roof Flush Solar Array for Autonomous Watchkeeping (L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Flush Solar Array & Standby Power  ·  Level: L6                 │
│ [VIEWPORT] Grid Zone: [X ±4, Z −24 to −20]  ·  Plan view Y = L6 (▲ Bow +Z)            │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.38 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Solar Panel (Large Grid) | 4 | Slot 0 (`#2B303A`) | 20.4 t / 32 | `MAS Solar 1..4` (1×4 B) flush with roof at X ±4 from Z −24 to −20 |
| Heavy Armor Slope | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Recessed border coaming providing 1-block thruster buffer |

```
Plan view, Y = L6 (Mule Blast Deck Flanked by Flush Solar Array)
        X: −4     X: −3           0          +3     X: +4
    Z: −19 ┌────┐ ■════════════╤═══════════■ ┌────┐
           │SOL1│ ║  Hazard    │  Hazard   ║ │SOL3│  ◄── Flush Solar Array (≈ 640 kW Peak†)
    Z: −22 │    │ [CLAMP P] [MAS MP CONN] [CLAMP S]  │    │  ◄── Mule Clamping Rails
           │SOL2│ ║  Chevrons  │  Chevrons ║ │SOL4│
    Z: −25 └────┘ ■════════════╧═══════════■ └────┘
```

#### Placement Instructions
1. Lay 2× Large Grid Solar Panels (`MAS Solar 1, 2`) along the Port roof wing at X = −4 spanning Z = −24 to −20, recessed flush with the Level L6 deck surface.
2. Lay 2× Large Grid Solar Panels (`MAS Solar 3, 4`) along the Starboard roof wing at X = +4 spanning Z = −24 to −20, recessed flush with the Level L6 deck surface.
3. Maintain a continuous 1-block clear buffer (Heavy Armor Slopes) between the solar array and the 7×7 B Mule landing footprint, preventing thermal erosion from shuttle hydrogen thruster exhaust during approach and liftoff.
4. Route power conduits down into the Level L2 battery vault bus.

#### Terminal Configuration & Sliders
* Solar Panels: `MAS Solar 1..4`
  - Peak Power Output: `≈ 160 kW each† (640 kW array peak†)`
  - Operating Role: Continuous zero-fuel watchkeeping power
  - Power Balancing: Maintains life support, Event Controllers, antenna beacons, and float-charges the 18.0 MWh battery bank during prolonged daytime quarrying without consuming nuclear uranium or hydrogen fuel.

✓ Check: Solar array generates daylight float-charge; clear buffer prevents Mule thruster burn; batteries maintain 100% state of charge during engine-off watchkeeping.

---

### Step 10.39 · Level L6 Flight Deck Maintenance Catwalk Network (L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Flight Deck Walkways & Safety Handrails  ·  Level: L6           │
│ [VIEWPORT] Grid Zone: [X −4 to +4, Z −26 to −10]  ·  Plan view Y = L6 (▲ Bow +Z)      │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.39 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Catwalk with Railing Block | 18 | Slot 9 (`#F1C40F`) | 5.4 t / 18 | Perimeter railing walkways surrounding Mule deck and Kestrel paths |
| Grated Catwalk Block | 10 | Slot 9 (`#F1C40F`) | 3.0 t / 10 | Walkway intersections, flight ladderwell landing, and solar access paths |
| Interior Light | 4 | Slot 9 (`#F1C40F`) | 0.2 t / 16 | `MAS Light Catwalk 1..4` mounted along railing stanchions |

```
Plan view, Y = L6 (Flight Deck Walkway Network & Perimeter Railings)
        X: −4          X: −3           0           X: +3          X: +4
    Z: −10 ┌───────────────┬───────────────────────┬───────────────┬────────┐
           │               │ [FLIGHT LADDER EXIT]  │ [CATWALK P]   │        │  ◄── From Workshop L3
    Z: −12 ├───────────────┼───────────────────────┼───────────────┤        │
           │ [KESTREL P]   │ [FORWARD ACCESS DECK] │ [KESTREL S]   │        │  ◄── Service Bridges
    Z: −19 ├───────────────┼───────────────────────┼───────────────┼────────┤
           │ [SOLAR WALK P]│ ═════════════════════ │ [SOLAR WALK S]│        │  ◄── Flank Inspection
    Z: −22 │ [MAS SOLAR 1] │  7×7 MULE BLAST DECK  │ [MAS SOLAR 3] │        │      Gangways
    Z: −25 ├───────────────┼───────────────────────┼───────────────┼────────┤
    Z: −26 └───────────────┴───[AFT CIWS STERN]────┴───────────────┴────────┘  ◄── Stern Walkway
```

#### Placement Instructions
1. Install Grated Catwalk Blocks at the exit threshold of the vertical flight ladderwell at [X +2, Z −10, Y = L6].
2. Lay forward Catwalk with Railing Blocks extending from the ladderwell exit to the Port and Starboard Kestrel landing pads, establishing safe crew inspection paths to docked scout drones.
3. Install perimeter Catwalk with Railing Blocks surrounding the exterior border of the 7×7 B Mule blast deck along X = ±4 and Z = −26. Ensure high-visibility yellow railings face outward to prevent crew from falling off the vehicle roof (15.0 m above terrain).
4. Run narrow solar inspection catwalks along X = −4 and X = +4, giving engineers direct walk-up access to inspect and repair `MAS Solar 1..4` without treading on the active Mule pad.
5. Mount 4× Interior Lights (`MAS Light Catwalk 1..4`) along the railing stanchions.

#### Terminal Configuration & Sliders
* Lights: `MAS Light Catwalk 1..4` (Color: High-Viz Amber `#F1C40F` · Radius: `4.0 m` · Falloff: `1.5`)
* Group: Assign all catwalk lights to `MAS Lights`.

✓ Check: Crew on foot traverses entire upper flight deck from ladderwell to scout pads, solar panels, and stern CIWS with zero fall hazard.

---

### Step 10.40 · Twin Kestrel Prospector Scout Drone Landing Pads (L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Kestrel Scout Drone Receiving Pads  ·  Level: L5                │
│ [VIEWPORT] Grid Zone: [X ±3, Z −13 to −11]  ·  Plan view Y = L5 (▲ Bow +Z)           │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.40 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Connector (Small Grid via collar) | 2 | Slot 8 (`#FFFFFF`) | 0.8 t / 22 | `MAS Kestrel Dock 1` at [X −3, Z −12, L5]; `MAS Kestrel Dock 2` at [X +3, Z −12, L5] |
| Heavy Armor Slope | 12 | Slot 8 (`#FFFFFF`) | 10.2 t / 12 | Beveled alignment troughs funneling scout drone landing skids |
| Interior Light | 4 | Slot 8 (`#FFFFFF`) | 0.2 t / 16 | `MAS Light Pad Kestrel 1..4` inset into pad margins |

```
Plan view, Y = L5 (Twin Recessed Kestrel Scout Landing Pads)
        X: −3 (Port Scout Dock)                         X: +3 (Starboard Scout Dock)
    Z: −11 ┌───────────────────────────┐           ┌───────────────────────────┐
           │ [SLOPE]   [SLOPE] [SLOPE] │           │ [SLOPE]   [SLOPE] [SLOPE] │
    Z: −12 │ [LIGHT] [KESTREL DOCK 1]  │ ◄─ AISLE ─► [LIGHT] [KESTREL DOCK 2]  │
           │ (Recessed Flush Connector)│           │ (Recessed Flush Connector)│
    Z: −13 │ [SLOPE]   [SLOPE] [SLOPE] │           │ [SLOPE]   [SLOPE] [SLOPE] │
           └───────────────────────────┘           └───────────────────────────┘
```

#### Placement Instructions
1. Frame two recessed 3×3 block landing bays forward of the Mule gantry at Level L5: Port bay centered at [X −3, Z −12] and Starboard bay centered at [X +3, Z −12].
2. Install small-grid docking collars `MAS Kestrel Dock 1` and `MAS Kestrel Dock 2` flush with the deck floor facing zenith (+Y), plumbed directly to the utility conveyor spine.
3. Install Heavy Armor Slopes painted Palette Slot 8 (Scout Flight White) around pad perimeters, creating 45° beveled guide ramps that guide descending Kestrel drones into positive connector alignment.
4. Inset 2× perimeter indicator lights per pad (`MAS Light Pad Kestrel 1..4`).

#### Terminal Configuration & Sliders
* Connectors: `MAS Kestrel Dock 1`, `MAS Kestrel Dock 2`
  - Auto-Connect: `OFF` · Collect All: `OFF` · Strength: `0.020`
* Lights: `MAS Light Pad Kestrel 1..4` (Color: Scout Aviation Cyan `#00E5FF` · Radius: `3.0 m`)
* Data Bus: Docking triggers immediate IGC data download of geological survey scans (`TMS.SCOUT.VEIN`) to Station 3 Tactical Video Wall.

✓ Check: Kestrel scout drone lands smoothly in recessed bay; beveled slopes funnel skids into dock; connector locks and downloads geological scan telemetry.

---

### Step 10.41 · Flight Deck Recessed Drone Maintenance Welders (L4-L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Automated Flight Deck Drone Repair Welders  ·  Level: L4-L5     │
│ [VIEWPORT] Grid Zone: [X ±3, Z −12]  ·  Profile view facing Port (▲ Zenith +Y)        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.41 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Ship Welder (Large Grid) | 2 | Slot 4 (`#16A085`) | 7.8 t / 300 | `MAS Welder Pad Kestrel 1` at [X −3, Z −12, L4]; `MAS Welder Pad Kestrel 2` at [X +3, Z −12, L4] |
| Large Conveyor Pipe | 2 | Slot 4 (`#16A085`) | 0.6 t / 2 | Vertical risers connecting welders to workshop conveyor spine |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Drone Repair` at [X0, Z −13, L4] managing automated pulse cycle |

```
Profile elevation cross-section, X = −3 (Port Kestrel Pad Welder Riser)
  L5   [RECESSED DECK] ── [MAS KESTREL DOCK 1] ── [RECESSED DECK]
             │                    │                      │
  L4   ══════╪════════════════════╪══════════════════════╪══════
             │           [MAS WELDER PAD KESTREL 1]      │
             │           (Faces Upward toward Drone Hull)│
             │                    │                      │
  L3   ──────┴──────────────[CONVEYOR RISER]─────────────┴──────
       WORKSHOP CEILING (Plumbed to MAS Hold Components & Assembler)
```

#### Placement Instructions
1. Position Large Grid Ship Welder `MAS Welder Pad Kestrel 1` directly beneath the Port Kestrel landing dock at [X −3, Z −12, Y = L4] with welding head facing upward (+Y).
2. Position Large Grid Ship Welder `MAS Welder Pad Kestrel 2` directly beneath the Starboard Kestrel landing dock at [X +3, Z −12, Y = L4] with welding head facing upward (+Y).
3. Connect bottom conveyor ports of both welders directly into the Level L3 workshop conveyor trunk, drawing steel plates, construction components, small tubes, and computer chips from `MAS Hold Components` and `MAS Utility Assembler`.
4. Mount Event Controller `MAS EC Drone Repair` in the sub-deck wireway at [X0, Z −13, Y = L4].

#### Terminal Configuration & Sliders
* Welders: `MAS Welder Pad Kestrel 1`, `MAS Welder Pad Kestrel 2`
  - Help Others: `ON`
  - Power: `OFF` (Pulsed on-demand by automation)
* Event Controller: `MAS EC Drone Repair`
  - Monitored Target: `MAS Kestrel Dock 1, 2`
  - Condition: Status = `Connected`
  - Slot 1 (Touchdown): Pulse pad welders for 5.0 seconds -> Auto-repairs thruster burnout, hull pits, and antenna damage sustained during prospecting sorties -> Power OFF
  - Slot 2: Welders OFF on disconnect

✓ Check: Damaged Kestrel scout docks; welders pulse for 5.0 s; hull repairs to 100% using parts from component hold; welders return to standby.

---

### Step 10.42 · Worksite 360° Floodlight Matrix & Dorsal Comms Mast (L5-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Worksite Perimeter Illumination & Comms Mast  ·  Level: L5-L6   │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z +7 to −26]  ·  Plan view Y = L5-L6 (▲ Bow +Z)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.42 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Spotlight (Large Grid) | 4 | Slot 7 (`#1B263B`) | 2.0 t / 32 | `MAS Floodlight Tower 1..4` in cardinal ring on bridge crown & gantry (Bow, Stern, Port, Stbd) |
| Interior Light | 4 | Slot 0 (`#2B303A`) | 0.2 t / 16 | `MAS Lights Underbelly` at [X ±2, Z0 and Z −18, L1] for chassis inspection |

```
Plan view, Y = L5-L6 (Worksite 360° Cardinal Floodlight Ring)
                                [BOW FLOODLIGHT (+Z)]
                               (MAS Floodlight Tower 1)
                                         ▲
                                         │
        [PORT FLOODLIGHT (−X)] ◄── [MASTODON C2] ──► [STBD FLOODLIGHT (+X)]
       (MAS Floodlight Tower 3)    (Crown L5-L6)     (MAS Floodlight Tower 4)
                                         │
                                         ▼
                               [STERN FLOODLIGHT (−Z)]
                               (MAS Floodlight Tower 2)
```

#### Placement Instructions
1. Mount 4× Large Grid Spotlights (`MAS Floodlight Tower 1..4`) in an elevated 360° cardinal ring on the Tractor dorsal bridge crown and comms mast at Level L5-L6:
   - `MAS Floodlight Tower 1`: Facing Bow (+Z) illuminating front excavation zone.
   - `MAS Floodlight Tower 2`: Facing Stern (−Z) illuminating rear Caterpillar dump and Mule pad.
   - `MAS Floodlight Tower 3`: Facing Port (−X) illuminating left flank and boarding ladder.
   - `MAS Floodlight Tower 4`: Facing Starboard (+X) illuminating right quarry face.
2. Install 4× Interior Lights (`MAS Lights Underbelly`) along the underbelly chassis keel at [X ±2, Z 0, Y = L1] and [X ±2, Z −18, Y = L1] facing downward.
3. Bind all worksite and chassis lights into terminal group `MAS Lights`.

#### Terminal Configuration & Sliders
* Spotlights: `MAS Floodlight Tower 1..4`
  - Color: Pure White (`#FFFFFF`) · Intensity: `3.5` · Radius: `100 m` · Falloff: `1.2`
* Keel Lights: `MAS Lights Underbelly`
  - Color: Industrial Amber (`#F1C40F`) · Radius: `6.0 m` · Falloff: `1.5`
* Control Binding: Station 1 Helm Hotbar 3 Slot 8 toggles group `MAS Lights`.

✓ Check: Group `MAS Lights` illuminates 100 m perimeter around vehicle; excavation face, flight decks, and underbelly inspection alleys are fully visible in pitch black.

---

### Step 10.43 · Stern Push-Bumper, Telescoping Soft-Capture Port & Stern Outriggers (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Stern Rail Dump Interface & Bedrock Outriggers  ·  Level: L1-L2 │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z −27 to −25]  ·  Plan view Y = L2 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.43 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Piston (Large Grid) | 1 | Slot 2 (`#8B0000`) | 12.5 t / 25 | `MAS Piston Stern Dock` at [X0, Z −25, L2] horizontal extending aft (−Z) |
| Large Connector | 1 | Slot 2 (`#8B0000`) | 4.1 t / 25 | `MAS Stern Dump Conn` mounted on piston head at [X0, Z −26, L2] |
| Large Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 4.9 t / 25 | `MAS Stern Sorter` at [X0, Z −24, L2] pointing aft (−Z) into piston base |
| Heavy Armor Slope | 8 | Slot 2 (`#8B0000`) | 6.8 t / 8 | Angled collision guide wedges flanking the dump connector at X ±3 |
| Piston (Large Grid) | 2 | Slot 0 (`#2B303A`) | 25.0 t / 50 | `MAS Outrigger Utility Port/Stbd` vertically at [X ±3, Z −25, L2] extending down |
| Magnetic Plate (Large) | 2 | Slot 0 (`#2B303A`) | 5.8 t / 50 | `MAS Seismic Pad Rear Port/Stbd` on piston heads facing ground (L1) |
| Conveyor Sorter | 1 | Slot 0 (`#2B303A`) | 4.9 t / 25 | `MAS Sorter Tailings Stern` at [X0, Z −26, L1] pointing aft (−Z) |
| Conveyor Ejector | 4 | Slot 0 (`#2B303A`) | 2.4 t / 40 | `MAS Ejector Tailings Stern 1..4` angled 45° downward/aft behind rear tires |

```
Rear elevation cross-section, Y = L2 (Stern Caterpillar Dump & Outrigger Interface)
    Port [-X]                                                          Starboard [+X]
    X: −3            X: −1            X0              X: +1            X: +3
    [GUIDE WEDGE] ── [PUSH BLOCK] ─ [STERN CONN] ─ [PUSH BLOCK] ── [GUIDE WEDGE]
    │ Heavy Slope    │ Solid Block  │ (Telescoping  │ Solid Block  │ Heavy Slope
    │ Crimson        │ Crimson      │ Piston Head)  │ Crimson      │ Crimson
    └──────┬─────────┴──────────────┴───────┬───────┴──────────────┴───────┬─────┘
           │                                │                              │
    [OUTRIGGER PORT]               [TAILINGS EJECTORS]             [OUTRIGGER STBD]
    (Piston down to L1)            (Compacted Roadbed)             (Piston down to L1)
```

```
Telescoping Stern Dock Profile, Z = −24 to −27.5, Y = L2
       [CARRIAGE 2 STERN]                             [CATERPILLAR BOW]
       ┌───────────────┐                               ┌───────────────┐
   L2  │ CONVEYOR DUCT ├──[PISTON 2.5 m]──[CONNECTOR]══╡ CONNECTOR     │
       └───────────────┘  (MAS Piston     (MAS Stern   └───────────────┘
                           Stern Dock)     Dump Conn)
```

#### Placement Instructions
1. Mount 1× Large Grid Piston `MAS Piston Stern Dock` horizontally at [X0, Z −25, Y = L2] with piston base plumbed to `MAS Stern Sorter` and piston head extending aft (−Z). Maximum stroke: 2.5 m (reaching Z = −27.5).
2. Install Large Connector `MAS Stern Dump Conn` directly onto the piston head. Configure centerline height at 2.5 m above ground plane matching the Caterpillar locomotive bow connector.
3. Install Heavy Armor Slopes painted Palette Slot 2 (Heavy Rail Crimson) at X = −3 and X = +3, forming rigid guide wedges that funnel the incoming land train into alignment.
4. Mount 2× Large Grid Pistons vertically at [X −3, Z −25, Y = L2] (`MAS Outrigger Utility Port`) and [X +3, Z −25, Y = L2] (`MAS Outrigger Utility Stbd`) extending downward through Level L1.
5. Mount Large Magnetic Plates (`MAS Seismic Pad Rear Port/Stbd`) on the piston heads facing ground.
6. Install Large Conveyor Sorter `MAS Sorter Tailings Stern` and 4× Conveyor Ejectors (`MAS Ejector Tailings Stern 1..4`) along the rear bumper at Level L1, angled 45° downward/aft.

#### Terminal Configuration & Sliders
* Piston: `MAS Piston Stern Dock` (Max Distance: `2.5 m` · Velocity: `0.2 m/s` · Share Inertia Tensor: `ON`)
* Connector: `MAS Stern Dump Conn` (Auto-Connect: `OFF` · Strength: `0.015`)
* Sorter: `MAS Stern Sorter` (Mode: `Whitelist` · Items: `Iron`, `Nickel`, `Silicon`, `Stone` · Drain All: `ON`)
* Outriggers: `MAS Outrigger Utility Port/Stbd` (Max Distance: `1.5 m` · Velocity: `0.5 m/s` · Share Inertia Tensor: `ON`)
* Tailings Sorter: `MAS Sorter Tailings Stern` (Mode: `Whitelist` · Items: `Stone`, `Gravel` · Drain All: `ON`)

✓ Check: Stern piston extends 2.5 m and soft-couples with Caterpillar locomotive; 2,500 t bulk ore dumps in $< 90\text{ seconds}$; stern outriggers achieve 100% bedrock lock; tailings eject aggregate behind rear wheels.

---

### Step 10.44 · Stern Gantry CIWS Gatling Turret (L5)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Rear Defense CIWS & Gantry Fire Arc  ·  Level: L5               │
│ [VIEWPORT] Grid Zone: [X0, Z −26]  ·  Rear elevation facing Bow (▲ Zenith +Y)        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.44 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Gatling Turret (Large Grid) | 1 | Slot 7 (`#1B263B`) | 4.2 t / 275 | `MAS Turret Stern` at [X0, Z −26, L5] elevated 7.5 m above dump bumper |
| Conveyor Junction | 1 | Slot 4 (`#16A085`) | 0.9 t / 1 | Plumbed into utility conveyor spine at [X0, Z −26, L4] |

```
Rear elevation looking Bow (+Z), Z = −26 (Stern Defense Turret & Firing Cleats)
  L6   ═══════════════════════════════════════════════════════════════
       │                                                             │
  L5   │                    [MAS TURRET STERN]                       │  ◄── 270° Hemispherical
       │                    (Gatling CIWS L5)                        │      Covering Fire Arc
  L4   ├────────────────────[CONVEYOR JUNCTION]──────────────────────┤
       │                                                             │
  L3   │                    STERN AIRLOCK DESCENT HATCH              │
       │                                                             │
  L2   ══════════════[TELESCOPING DUMP BUMPER]════════════════════════
```

#### Placement Instructions
1. Install Large Conveyor Junction at [X0, Z −26, Y = L4] tied directly into the utility aft conveyor trunk.
2. Mount Large Grid Gatling Turret `MAS Turret Stern` on top of the junction at [X0, Z −26, Y = L5], elevated 7.5 m above the Level L2 Caterpillar dump bumper.
3. Verify unobstructed 270° hemispherical firing arc over the aft hemisphere (−Z, ±X, and zenith +Y), covering incoming Mule shuttles on approach and trailing land trains during dump cycles.
4. Tie turret ammunition feed into the workshop conveyor trunk; verify automated pull of NATO 25×184mm ammo boxes from `MAS Hold Components`.

#### Terminal Configuration & Sliders
* Gatling Turret: `MAS Turret Stern`
  - Range: `800 m` · Target Meteors: `ON` · Target Missiles: `ON` · Target Small Ships: `ON` · Target Large Ships: `ON`
  - Idle Movement: `OFF` · Use Conveyor System: `ON`
* Group: Assign to terminal group `MAS Turrets`.

---

### Step 10.44B · Elevated Stern Multitool Repair Turret & Outbound Resupply Staging Hopper (L5-L7)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Consist Tail Repair & Mule Peer Resupply  ·  Levels: L4, L5, L7  │
│ [VIEWPORT] Grid Zone: Carriage 2 Stern Arch [X0, Z −23, L7]; Staging [X0, Z −20, L5] │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.44B Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Laser Multitool Turret | 1 | Slot 3 (`#E07A5F`) | 4.5 t / 225 | `MAS MT Turret Stern` on elevated gantry arch at [X0, Z −23, L7] · Mod 3175774286 |
| Small Cargo Container | 1 | Slot 5 (`#27AE60`) | 1.0 t / 10 | `MAS Hold Resupply Export` at [X0, Z −20, L5] directly beneath `MAS MP Conn` |
| Conveyor Sorter | 1 | Slot 4 (`#16A085`) | 1.0 t / 25 | `MAS Sorter Resupply Export` at [X0, Z −20, L4] feeding upward into export hopper |
| Heavy Armor Blocks / Slopes | 9 | Slot 0 (`#2C3E50`) | 1.0 t / 2 | Gantry arch framing rising from L5/L6 deck line to L7 apex at Z −23 |

```
Rear elevation looking Bow (+Z), Z = −23 (Elevated Stern MT Repair Turret & Arch)
  L7   ═════════════════════[MAS MT TURRET STERN]═════════════════════  ◄── 360° Hemispherical
       │                     (Laser Welder L7)                       │      Overhead Repair Arc
  L6   ├─────────[GANTRY ARCH FRAME]───┬───[GANTRY ARCH FRAME]───────┤
       │                               │                             │
  L5   │   [MULE PAD CATWALK]    [MAS HOLD RESUPPLY EXPORT]  [CATWALK]│  ◄── Resupply Staging Hopper
       │                               │                             │      (Direct Drain to Mule)
  L4   ├───────────────────[MAS SORTER RESUPPLY EXPORT]───────────────┤
       │                               │                             │
  L3   │                 WORKSHOP CONVEYOR CENTRAL TRUNK             │
```

#### Placement Instructions
1. Erect a 9-block Heavy Armor gantry arch rising from the Level L5-L6 flight deck margins up to a centerline apex at [X0, Z −23, Y = L7], spanning over the Mule pad approach.
2. Plumb a vertical conveyor riser from the workshop conveyor trunk up the arch leg to [X0, Z −23, Y = L7].
3. Mount Large Grid Laser Multitool Turret `MAS MT Turret Stern` on top of the arch at [X0, Z −23, Y = L7]. Verify unobstructed 360° hemispherical vantage looking down directly onto the MULE-01 cradle pad (L6), the stern Caterpillar dump bumper (L2), and the aft trailing ground approach.
4. Install Conveyor Sorter `MAS Sorter Resupply Export` at [X0, Z −20, Y = L4] with flow arrow pointing upward into Level L5.
5. Install Small Cargo Container `MAS Hold Resupply Export` at [X0, Z −20, Y = L5] directly between the export sorter and the top-deck `MAS MP Conn` junction.

#### Terminal Configuration & Sliders
* Group: `MAS Multitool Turrets` (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`)
* Group: `MAS Resupply` (`MAS Hold Resupply Export`, `MAS Sorter Resupply Export`, `MAS MP Conn`)
* Multitool Turret: `MAS MT Turret Stern`
  * Default Mode: `Welder` (Servicing docked Mule and trailing vehicles)
  * Secondary Mode: `Grinder` (Aft salvage and obstacle clearance)
  * Range: `50 m` · Target Friendly: `ON` · Target Neutrals: `ON` · Auto-Repair: `ON`
* Sorter: `MAS Sorter Resupply Export`
  * Default State: `OFF` · Drain All: `OFF`
  * Whitelist filter automatically populated by `[MAS-U] PB Utility` upon resupply kit staging.
* Event Controller: `MAS EC Resupply`
  * Monitored Target: `MAS Hold Resupply Export`
  * Condition: Inventory Fill > 0%
  * Action Slot 1: Broadcast IGC `TMS.RESUPPLY.DISPATCH` · Light Staging Indicator
  * Action Slot 2: Standby

#### Standardized Mule Peer Resupply Sortie Protocol
* Standardized Pallet Kits:
  * `KIT_MOBILITY`: 200 Steel Plates, 100 Motors, 50 Large Tubes, 50 Small Tubes.
  * `KIT_POWER`: 100 Steel Plates, 80 Construction Comps, 40 Computers, 50 Large Tubes.
  * `KIT_MUNITIONS`: 60 NATO 25×184mm ammo boxes, 40 Interior Plates, 20 Construction Comps.
  * `KIT_HULL_REPAIR`: 500 Steel Plates, 100 Interior Plates, 50 Construction Comps.
* Resupply Execution Cycle:
  1. Sister rig (`MAS-02`) broadcasts emergency distress call via `TMS.RESUPPLY.REQUEST;MAS-02;KIT_MOBILITY;<gps>`.
  2. `[MAS] PB Excavator` / `[MAS-U] PB Utility` configures `MAS Sorter Resupply Export` whitelist with requested kit, turns Drain All = ON until export hopper contains the exact bill of materials, then powers sorter OFF.
  3. MULE-01 touches down on `MAS MP Conn`; export hopper drains directly into Mule cargo hold in $< 10\text{ seconds}$ without mixing into main component stores.
  4. MULE-01 departs immediately on autonomous delivery run to `MAS-02`, drops resupply pallet, and returns to consist.

---

#### Pre-Enclosure Inspection Gate 10.6A: Cradle Clamping Dynamics & Dual-Tier Sorter Routing
* [ ] Verify `MAS MP Sorter` whitelist passes only rare ores (U, Pt, Au, Mg) to the Mule shuttle.
* [ ] Verify `MAS Stern Sorter` whitelist passes bulk ores (Fe, Ni, Si, Stone) to the land train.
* [ ] Test cradle clamp hinges: confirm 30° inward swing firmly holds docking craft without grid damage.
* [ ] Confirm `MAS Solar 1..4` deliver daylight power and maintain battery float charge while nuclear reactors are toggled OFF.
* [ ] Verify Level L6 catwalk handrails and safety walkways allow complete perimeter access around Mule and Kestrel decks.
* [ ] Verify `MAS Turret Stern` receives NATO 25×184mm ammunition from utility cargo hold.
* [ ] Verify recessed drone welders (`MAS Welder Pad Kestrel 1, 2`) auto-repair docked Kestrel drones upon touchdown.
* [ ] Confirm worksite floodlight matrix (`MAS Floodlight Tower 1..4`) casts 360° illumination across mining zone and flight decks.
* [ ] Confirm `MAS Piston Stern Dock` extends 2.5 m and soft-couples with Caterpillar land train.
* [ ] Verify `MAS Outrigger Utility Port/Stbd` achieve 100% bedrock lock and isolate chassis during Mule touchdown.
* [ ] Verify `MAS Sorter Tailings Stern` discharges stone aggregate safely behind stern bumper.
* [ ] Verify `MAS MT Turret Stern` on L7 arch achieves 360° hemispherical coverage over Mule pad and stern approach.
* [ ] Verify `MAS Sorter Resupply Export` pre-stages requested resupply kit into `MAS Hold Resupply Export` and drains to Mule in $< 10\text{ s}$.

```
PHASE 10-6 VISUAL MILESTONE · COMPLETE EXPEDITIONARY MINING TRAIN FOB
                        [KESTREL PADS]         [MULE PAD & MT ARCH L7]
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
* `MAS Outriggers`: 4× Hydraulic stabilizer pistons + magnetic plates / seismic feet on Tractor.
* `MAS Outriggers Utility`: 2× Hydraulic stabilizer pistons + seismic feet at Carriage 2 stern.
* `MAS Boom Joints`: 5× Rotors and Hinges articulating the long-reach arm.
* `MAS Lasers`: 2× Simple Laser Multitools on the tool head.
* `MAS Welders`: 2× Flush Large Grid Ship Welders (`MAS Welder Pad Kestrel 1, 2`) on Kestrel flight pads.
* `MAS Lights`: Worksite floodlights (`MAS Floodlight Tower 1..4`), boom spotlights (`MAS Light Boom 1, 2`), and underbelly lamps (`MAS Lights Underbelly`).
* `MAS Seismic`: 6× Seismic Surveying Landing Gear units (`MAS Seismic Outrigger 1..4`, `MAS Seismic Pad Rear Port/Stbd`).
* `MAS Batteries`: 6× Large Batteries across Tractor Vault and Utility Sub-Floor (18.0 MWh total).
* `MAS Doors`: All internal airlock, vault, and inter-carriage gangway sliding doors.
* `MAS Turrets`: 2× Large Gatling Turrets (`MAS Turret Bow`, `MAS Turret Stern`) + 2× Interior Turrets (`MAS Turret Flank Port`, `MAS Turret Flank Stbd`).
* `MAS MP Cradle`: 2× Motorized Hinges + 2× Magnetic Plates on the Mule pad.
* `MAS Displays`: All cockpit telemetry screens and tactical video wall LCDs.
* `MAS Hitch Clamps`: 2× Large Magnetic Plates (`MAS Hitch Clamp Port`, `MAS Hitch Clamp Stbd`) on hitch drawbar.
* `MAS DPU`: Axles 4 and 5 suspension wheels on Carriage 2 running distributed traction logic.
* `MAS FOB`: Carriage 2 autonomous command blocks (`[MAS-U] PB Utility`, `MAS-U Remote FOB`, `MAS-U EC Breakaway`).
* `MAS Tailings Ejectors`: 4× Ejectors on stern bumper (`MAS Ejector Tailings Stern 1..4`).
* `MAS Multitool Turrets`: 4× Large Laser Multitool Turrets (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`).
* `MAS Resupply`: `MAS Hold Resupply Export`, `MAS Sorter Resupply Export`, `MAS MP Conn`.

### 3. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Programmable Block | `[MAS] PB Excavator` | Kinematics & machine control · Update1 · Zero-GC |
| Programmable Block | `[MAS] PB RIG//OS-Lite` | Regional Fleet C2 Mothership engine · Update10 · IGC router |
| Programmable Block | `[MAS-U] PB Utility` | Autonomous Carriage 2 FOB supervisory core · Update10 |
| Remote Control | `MAS-U Remote FOB` | Carriage 2 autonomous waypoint, defense, and antenna link |
| Control Station | `MAS Helm` | Station 1 (Port) · Pilot traction, steering, outrigger, power control |
| Control Station | `MAS Station Boom` | Station 2 (Center) · Excavation Master · CTC mouse aim & macros |
| Control Station | `MAS Station Fleet` | Station 3 (Stbd) · Tactical Fleet Director · Drone dispatch & C2 |
| Laser Antenna | `MAS Comm Laser` | 40 km point-to-point line-of-sight data link (L6 mast) |
| Radio Antenna | `MAS Comm Radio` | 50 km omnidirectional fleet broadcast & heartbeat beacon |
| Radio Antenna | `MAS Beacon Distress` | 50 km blackout SOS distress beacon in sub-bridge keel |
| Battery (Small) | `MAS Battery BlackBox` | Isolated cold-standby power cell for distress beacon |
| Advanced Rotor | `MAS Hitch Yaw` | Articulated hitch yaw joint at [X0, Z −8, L2] · ±45° yaw |
| Motor Hinge | `MAS Hitch Pitch` | Articulated hitch pitch joint at [X0, Z −8, L2] · ±25° pitch |
| Advanced Rotor | `MAS Hitch Roll` | Articulated hitch roll joint at [X0, Z −8, L2] · ±20° roll along Z-axis |
| Magnetic Plate (Large) | `MAS Hitch Clamp Port/Stbd` | Twin anti-oscillation drawbar lock clamps (engaged > 30 km/h) |
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
| Piston (Large) | `MAS Outrigger Utility Port/Stbd`| Stern vertical hydraulic outriggers at [X ±3, Z −25, L2] |
| Magnetic Plate | `MAS Seismic Pad Rear Port/Stbd`| Stern bedrock voxel locking pads · `Auto-Lock = ON` |
| Seismic Landing Gear | `MAS Seismic Outrigger 1..4` | Deep voxel acoustic prospecting feet (TMS Mod 2819263781) |
| Magnetic Plate (Large) | `MAS Utility Brake Keel 1, 2` | Keel emergency terrain skids at [X ±2, Z −18, L1] |
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
| Piston (Large) | `MAS Piston Stern Dock` | Stern horizontal telescoping docking piston (2.5 m stroke) |
| Connector (Large) | `MAS Stern Dump Conn` | Stern telescoping connector for CATERPILLAR-01 bulk train docking |
| Conveyor Sorter | `MAS MP Sorter` | High-priority rare ore drain sorter (U, Pt, Au, Mg) |
| Conveyor Sorter | `MAS Stern Sorter` | High-volume bulk ore drain sorter (Fe, Ni, Si, Stone) |
| Conveyor Sorter | `MAS Sorter Ammo Fwd` | Dedicated forward munitions sorter to bow CIWS |
| Conveyor Sorter | `MAS Sorter Parts Aft` | Dedicated aft scrap metal sorter into field refinery |
| Conveyor Sorter | `MAS Sorter Tailings Stern` | Relocated stern roadbed aggregate sorter at Z −26 |
| Conveyor Ejector | `MAS Ejector Tailings Stern 1..4`| Angled stern bumper nozzles laying aggregate roadbed |
| Basic Refinery | `MAS Field Refinery` | Closed-loop field metallurgy · 330 kW · Ingests raw ore, outputs ingots |
| Hydrogen Engine | `MAS Engine H2` | Standby cold-start emergency generator · 5.0 MW failover |
| Solar Panel | `MAS Solar 1..4` | Flush roof solar array · 640 kW peak† · Zero-fuel watchkeeping |
| Battery (Large) | `MAS Battery 1..3` | Tractor Power Core Battery Bank · 9.0 MWh · 36 MW discharge |
| Battery (Large) | `MAS Battery 4..6` | Utility Sub-Floor Battery Bank · 9.0 MWh · 36 MW discharge |
| Sliding Door | `MAS Door Airlock` | Port L2/L3 entry airlock door |
| Sliding Door | `MAS Door Vault` | Power Core Vault maintenance entrance from Jefferies tube |
| Sliding Door | `MAS Door Gangway Fwd` | Tractor Aft Bulkhead gangway isolation door |
| Sliding Door | `MAS Door Gangway Aft` | Utility Forward Bulkhead gangway isolation door |
| Laser Multitool Turret | `MAS MT Turret Bow` | Forward crown repair turret at [X0, Z +9, L5] · 360° bow & tire repair |
| Laser Multitool Turret | `MAS MT Turret Port` | Forward port shoulder repair turret at [X −3, Z −10, L4] · Kestrel pad & hitch repair |
| Laser Multitool Turret | `MAS MT Turret Stbd` | Forward stbd shoulder repair turret at [X +3, Z −10, L4] · Kestrel pad & hitch repair |
| Laser Multitool Turret | `MAS MT Turret Stern` | Elevated gantry arch repair turret at [X0, Z −23, L7] · Mule pad & aft consist repair |
| Small Cargo Container | `MAS Hold Resupply Export` | Outbound peer resupply staging hopper at [X0, Z −20, L5] directly under `MAS MP Conn` |
| Conveyor Sorter | `MAS Sorter Resupply Export` | SML resupply kit staging sorter at [X0, Z −20, L4] |
| Event Controller | `MAS EC Resupply` | Outbound resupply staging watchdog · Monitors `MAS Hold Resupply Export` |
| Event Controller | `MAS EC Stabilizers` | Blocks boom arm deployment until outriggers report locked |
| Event Controller | `MAS EC Level` | Auto-leveling bedrock jack sequence on sloped terrain |
| Event Controller | `MAS EC Ladder` | Auto-deploys crew ladder on approach; retracts on drive |
| Event Controller | `MAS EC Tailings` | Pulses stern stone ejectors when ore buffers > 50% |
| Event Controller | `MAS EC Hitch Speed` | Locks hitch yaw (100 MN·m) and clamps drawbar straight at speeds > 30 km/h |
| Event Controller | `MAS EC Hitch Rev` | Clamps hitch yaw to ±15° and inverts Axle 5 when reversing |
| Event Controller | `MAS EC DPU Drive` | Synchronizes Axles 4 & 5 traction and dynamic descent braking |
| Event Controller | `MAS-U EC Breakaway` | Deadman breakaway watchdog: drops keel skids and handbrake on hitch sever |
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
| `MAS EC Tailings` | Tractor Ore Buffer | Ore Fill $\ge 50\%$ | `MAS Sorter Tailings Stern` Drain ON · Ejectors ON | Sorter & Ejectors OFF |
| `MAS EC Hitch Speed`| Consist Speedometer | Speed $> 30\text{ km/h}$ | Hitch Torque = 100 MN·m · Lock `MAS Hitch Clamps` | Normal Hitch Torque (45 MN·m) · Unclamp |
| `MAS EC Hitch Rev` | Transmission Gear | Gear = Reverse | Clamp Yaw ±15° · Axle 5 Positive Tracking | Normal Yaw ±45° · Counter-Steer |
| `MAS EC DPU Drive` | Helm Throttle / Grade | Incline $> 5^\circ$ AND Throttle ON | Axles 4 & 5 Matching Push Torque (100%) | Neutral Tractive Balance |
| `MAS-U EC Breakaway`| Consist Handshake | Electrical / Merge Link = Lost | Deploy `MAS Utility Brake Keel` · Handbrake ON · SOS Beacon | Standby Isolation |
| `MAS EC Holds` | Combined Ore Mass | Ore Mass $\ge 2,500\text{ t}$ | Sound Payload Horn · IGC `TMS.MULE.CALL` | Normal Extraction Mode |
| `MAS EC MP` | `MAS MP Conn` | Status = Connected | `MAS MP Sorter` Drain ON · Cradle Clamp | Sorter OFF · Cradle Retract (0°) |
| `MAS EC Resupply` | `MAS Hold Resupply Export` | Inventory Fill $> 0\%$ | Broadcast `TMS.RESUPPLY.DISPATCH` · Staging Lamp ON | Standby Isolation |
| `MAS EC Train` | `MAS Stern Dump Conn` | Status = Connected | `MAS Stern Sorter` Drain ON | Sorter OFF |
| `MAS EC Handover` | `GND-01` Heartbeat | Signal Detected | Run PB `RIG//OS-Lite` (Argument: `HANDOVER`) | Maintain Mothership Authority |
| `MAS EC Power` | `MAS Reactor 1, 2` Uranium | Uranium Mass $< 1.0\text{ kg}$ | `MAS Engine H2` Turn ON · Sound Fuel Horn | `MAS Engine H2` Turn OFF |
| `MAS EC Ice` | `MAS Tank H2 1, 2` Fill | Hydrogen Fill $< 90\%$ | Enable Ice Conveyor Sorter & O2/H2 Gens | Normal Conveyor Routing |
| `MAS EC Combat` | Hull Integrity / Target Lock | Damage Detected OR Hostile Lock | Engage `MAS Turrets` · Elevate `RIG.OS.MODE CBT` | Standby Watchkeeping |
| `MAS EC Seismic` | `MAS Seismic Outrigger 1..4` | All Outriggers = Locked | Pulse Seismic Scan · Publish `TMS.SCOUT.VEIN` | Standby |
| `MAS EC Drone Repair` | `MAS Kestrel Dock 1, 2` | Either Pad = Connected | Pulse `MAS Welders` ON (5.0 s timer) | `MAS Welders` OFF |
| `MAS EC LifeSupport` | `MAS Tank O2 1, 2` / Pressure | O2 $< 25\%$ OR Pressure $< 0.5$ atm | Sound O2 Alarm · Seal Compartment Doors | Normal Atmospheric Circulation |
| `MAS EC BlackBox` | Consist Power Output | Total MW Output $= 0.0$ MW | Activate `MAS Battery BlackBox` & SOS Beacon | Standby Isolation |
| `MAS EC Atmosphere Mode` | Ambient Atmospheric O2 | Ambient O2 $\ge 100\%$ | `MAS Vent Atmo Intake` Depressurize ON | `MAS Vent Atmo Intake` Power OFF |

### 5. IGC Channel Subscriptions (Ground Fleet & RIG//OS Bus)
* Network Beacon: `RIG.C2.HEARTBEAT` (`<origin_id>;<role:MOTHERSHIP|BASE|WORKER>;<gps_coords>`).
* Network Handover: `RIG.C2.HANDOVER` (`<source_id>;<target_id>;<asset_registry_payload>`).
* Logistics Outbound: `TMS.MULE.CALL` (`CALL;<requesting_pad>;<priority>;<payload_type>`).
* Logistics Outbound: `TMS.CAT.CALL` (`CALL;<consist_id>;<bulk_ore_t>;<gps_coords>`).
* Consist Telemetry: `TMS.CONSIST.SYNC` (`SYNC;<source_id>;<hitch_torque>;<clamp_state>;<airlock_state>`).
* Powertrain DPU: `TMS.CONSIST.TRACTION` (`DPU;<mode:PUSH|BRAKE|COAST>;<torque_pct>;<axle4_slip>;<axle5_slip>`).
* Emergency Breakaway: `TMS.CONSIST.SEVER` (`SEVER;<source_id>;<reason>;<keel_skids_engaged>`).
* Resupply Inbound: `TMS.RESUPPLY.REQUEST` (`REQUEST;<requester_id>;<kit_name>;<gps_coords>`).
* Resupply Broadcast: `TMS.RESUPPLY.OFFER` (`OFFER;<provider_id>;<kit_name>;<eta_s>`).
* Resupply Dispatch: `TMS.RESUPPLY.DISPATCH` (`DISPATCH;<provider_id>;<transporter_id:MULE-01>;<target_id>;<kit_name>`).
* Resupply Confirmation: `TMS.RESUPPLY.COMPLETE` (`COMPLETE;<target_id>;<kit_name>;<status:DELIVERED>`).
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

### M · Consist Mechanics, 3-Axis Hitch, DPU Powertrain & FOB Autonomy Acceptance (Decision 67)
* [ ] **3-Axis Gimbal Hitch Compliance:** Drive consist across boulder-strewn terrain and steep dune ridges. Confirm hitch articulates freely across yaw (±45°), pitch (±25°), and torsional roll (±20°) without subgrid binding, Clang physics glitching, or wheel lift.
* [ ] **Anti-Oscillation Magnetic Clamping:** Accelerate consist above 30 km/h on flat salt flats. Confirm Event Controller `MAS EC Hitch Speed` locks twin magnetic clamps (`MAS Hitch Clamp Port/Stbd`), freezing hitch oscillation and stabilizing high-speed highway transit.
* [ ] **Distributed Power Unit (DPU) Hill-Climb Push:** Drive loaded consist up a 20° mountain grade. Confirm `[MAS] PB Excavator` engages DPU traction, driving Axles 4 and 5 at 100% matching torque to eliminate dead-drag on the drawbar hitch.
* [ ] **Predictive Regenerative Descent Braking:** Drive down a steep 25° incline. Confirm Carriage 2 wheels dynamically apply counter-braking before Tractor brakes bite, maintaining drawbar tension and preventing trailer jackknifing.
* [ ] **Autonomous FOB Detachment ("Drop-and-Mine"):** Trigger `Hitch Decouple`. Drive Tractor away as an independent 6×6 extraction vehicle. Confirm Carriage 2 computer core (`[MAS-U] PB Utility` and `MAS-U Remote FOB`) initializes, maintaining continuous life support, refinery recycling, battery charging, and stern CIWS watchkeeping.
* [ ] **Runaway Breakaway Deadman Brake:** Sever merge connection while on a 15° slope without tractor attached. Confirm `MAS-U EC Breakaway` instantly engages handbrakes and drops keel magnetic skids (`MAS Utility Brake Keel 1, 2`) into the terrain, halting runaway trailer slide within 1 tick.
* [ ] **Sub-Floor Ballast Low-CoM Verification:** Execute high-speed 35 km/h evasive slalom maneuvers. Confirm 40× Heavy Armor sub-floor ballast skids maintain Carriage 2 Center of Mass below Level L2.4, preventing rollover up to 34° side tilt.
* [ ] **Stern Bedrock Outrigger Mule Touchdown Shock Isolation:** Extend stern outriggers (`MAS Outrigger Utility Port/Stbd`) to bedrock lock. Land fully-laden 650 t MULE-01 onto Level L6 gantry pad. Confirm touchdown impact forces transfer directly into terrain with zero suspension deflection or chassis bucking.
* [ ] **42 m Triangulated Seismic Prospecting Baseline:** Deploy all 6 outriggers (4 forward, 2 aft). Trigger `SEISMIC`. Confirm 42 m acoustic baseline achieves sub-meter centroid accuracy on deep ore scans down to 500 m† depth.
* [ ] **Telescoping Soft-Capture Caterpillar Docking:** Back Caterpillar train to 2.0 m standoff behind stern bumper. Extend `MAS Piston Stern Dock`; confirm magnetic capture locks cleanly without vehicle impact. Retract piston and verify 2,500 t bulk ore dumps at high speed.
* [ ] **Relocated Stern Roadbed Aggregate Trail:** Mine stone with buffer > 50%. Confirm `MAS Sorter Tailings Stern` and 4× ejectors vent stone aggregate safely behind the stern bumper ([X0, Z −26, L1]), laying a continuous compacted roadbed without gravel impacting wheels.

### N · 4× Multitool Repair Turrets, SML Auto-Production & Peer Resupply Sortie Acceptance (Decision 68)
* [ ] **360° Overlapping Repair Envelope:** Position damaged armor blocks at Bow (Z +11), Hitch (Z −8), and Stern Dump (Z −26). Verify `MAS MT Turret Bow`, `MAS MT Turret Port/Stbd`, and `MAS MT Turret Stern` achieve 100% spherical repair line-of-sight across every block of the consist, welding all damage to 100% without engineer EVA.
* [ ] **Automated Flight Deck Drone Servicing:** Dock damaged Kestrel drone on Port pad (L5) and damaged Mule shuttle on top gantry pad (L6). Confirm `MAS MT Turret Port` and elevated `MAS MT Turret Stern` auto-target damaged thrusters, armor, and conveyor lines, restoring both craft to full operational readiness while engineers remain inside the pressurized bridge.
* [ ] **SML Autonomous Quota Monitoring:** Withdraw 1,000 Steel Plates from `MAS Hold Components` (reducing stock below 75% of 1,500 plate quota). Confirm `[MAS-U] PB Utility` detects deficit and automatically queues 1,000 Steel Plates in `MAS Utility Assembler`.
* [ ] **Closed-Loop Metallurgy to Component Path:** Mine Iron Ore with the 35 m boom. Verify ore refines into Iron Ingots in `MAS Field Refinery` (330 kW) and immediately flows into `MAS Utility Assembler` to fulfill the SML plate quota, replenishing `MAS Hold Components` with zero manual intervention.
* [ ] **Resupply Kit Assembly & Staging:** Issue command `RESUPPLY KIT_MOBILITY`. Confirm `MAS Sorter Resupply Export` powers ON with Drain All = ON, pulling exactly 200 Steel Plates, 100 Motors, 50 Large Tubes, and 50 Small Tubes into `MAS Hold Resupply Export` ([X0, Z −20, L5]), then shuts OFF.
* [ ] **High-Speed Mule Resupply Transfer:** Land MULE-01 on `MAS MP Conn`. Confirm `MAS Hold Resupply Export` empties completely into Mule cargo hold in $< 10\text{ seconds}$ flat without drawing from or sorting through the main 3×3×3 cargo hold.
* [ ] **Inter-Rig Peer Assistance IGC Handshake:** Simulate emergency distress transmission `TMS.RESUPPLY.REQUEST;MAS-02;KIT_POWER;<gps>`. Confirm Mastodon C2 automatically stages `KIT_POWER`, broadcasts `TMS.RESUPPLY.OFFER`, summons MULE-01, and issues `TMS.RESUPPLY.DISPATCH` upon shuttle takeoff.





