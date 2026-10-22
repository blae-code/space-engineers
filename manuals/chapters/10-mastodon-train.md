# Mastodon · Visual Build Instructions

## Chapter 10 · MASTODON-01 (MKI) Expeditionary Mining Locomotive & Mobile FOB

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **MASTODON-01 MKI**.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

> **Build status:** This chapter is a design specification, not a validated in-game blueprint. No `.sbc` blueprint is supplied. Dimensions below are design targets; block count, dry mass, PCU, capacity, airtightness, subgrid clearances, conveyor continuity, and load-bearing claims must be verified on the completed grids in Creative before treating them as final.

### Design Directives & Operational Scope (Decisions 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69 & 70)
* **Role:** Long-range terrestrial mining locomotive and mobile Forward Operating Base (FOB), with hardware provisions for a regional fleet-command role. `RIG//OS-Lite` fleet-root election, subordinate commands, and Ground Base handover require compatible PB scripts and protocols; this chapter does not supply them.
* **Architecture:** Segmented articulated train consist:
  1. **Carriage 1 (`MAS-01-T`): Locomotive & Excavator Unit (11 W × 20 L × 5 H B target · 6 wheels):** Heavy traction, three-operator C2 bridge, 2× Small Reactors (15 MW each), 3× Large Batteries (9 MWh total), 1× Hydrogen Engine (5 MW), dual-PB compute rack, four corner outriggers, modded seismic feet, dorsal 5-DOF boom with twin laser tools and a recovery attachment, bow CIWS, one forward repair turret, and the aft articulated drawbar.
  2. **Carriage 2 (`MAS-01-U`): Modular FOB & Utility Carriage (11 W × 18 L × 5 H B target · 4 wheels):** Field workshop, independent FOB PB and Remote Control, utility batteries, life support, component hold, two Kestrel pads and welders, Mule receiving deck, repair/defense turrets, stern outriggers, and Caterpillar-facing dump interface.
* **Grid:** Large Grid (1 B = 2.5 m). The two armored carriage bodies target a combined 11 W × 38 L × 5 H B envelope. The maximum equipped envelope target is 11 W × 42 L × 7 H B (27.5 × 105 × 17.5 m), including the drawbar, stern interfaces, and L7 gantry. The boom is a deployed operating envelope, not part of the travel envelope.
* **Mass and PCU:** Not established. Read these from the completed in-game grids; do not use the provisional estimates in this chapter for survival load planning.
* **Payload:** 2,500 t is a design target, not a verified capacity. Confirm inventory capacity and wheel, frame, hitch, and outrigger load limits with the intended ore mix and support craft before operating at this load.
* **Mechanical & Automation Suite (Decision 57):** Planned functions include bedrock jack deployment, a retractable Port boarding ladder, tool-head leveling, tailings discharge, and drawbar control. These are not native behaviors of the listed blocks: any automatic leveling, proximity stop, aggregate placement, or anti-jackknife control requires compatible PB logic and commissioning. No validated slope, clearance, or stability limit is established here.
* **Tri-Fuel Power & Closed-Loop Metallurgy Suite (Decision 58):** 2× Small Reactors provide 30 MW total, 1× Hydrogen Engine provides up to 5 MW, 4× Large Grid Solar Panels provide up to 640 kW in ideal conditions, and 6× Large Batteries provide 18 MWh storage and up to 72 MW combined discharge. Actual output varies with conditions and block state. The Basic Refinery and Large Assembler provide the planned field-processing loop; timers/PBs must implement any automatic power or inventory policy.
* **Player Circulation & Hybrid Power Suite (Decision 59):** The two carriages have separate airtight interiors. The articulated inter-carriage bellows is an exterior weather/dust guard only; it is not airtight and is not a pressurized passage. Crew must not traverse the hitch while the consist is moving or the joint is articulated. Use the designated airlocks, ladders, and catwalks when parked and secured.
* **Terrestrial Concrete Civil Engineering (Decision 60):** Solves excess quarrying aggregate through integration of the Concrete Armor Blocks mod ([303959338](https://steamcommunity.com/sharedfiles/filedetails/?id=303959338)). Gravel from stone refining is assembled into Concrete Components, producing blast-resistant pavers for Caterpillar roadbeds and Ground Base foundations while preserving 100% vanilla rule C2 on aerial fleet craft and the Rig.
* **Perimeter Defense CIWS & Warfare 3 MKII Roadmap (Decision 61):** Planned defense fit: 2× Large Grid Gatling Turrets (`MAS Turret Bow`, `MAS Turret Stern`) and 2× Interior Turrets (`MAS Turret Flank Port/Stbd`), with ammunition conveyors and tested firing arcs. Actual coverage, targeting, ammunition routing, and combat alerts depend on placement, game settings, and optional PB/timer logic; turret settings do not provide a fleet-wide command interface.
* **5-Point Field Logistics & Recovery Suite (Decision 62):** Planned features include modded seismic survey feet, Kestrel pad welders, a recovery attachment on the boom, atmospheric oxygen intake where supported by the vents, ground-level EVA lockers, and worksite lighting. Scan depth, recovery load, landing load, and coverage are acceptance-test targets; none are guaranteed by the parts list alone.
* **Extraction Arm Kinematics & Travel Stow Architecture (Decision 63):** The centerline pedestal and 5-DOF arm target a 35 m maximum tool reach. Final reach, stow clearance, joint stops, and stability are to be measured on the assembled subgrids. Vertical drilling and automated sweeps require a dedicated, tested controller; the block list alone does not provide inverse kinematics or collision avoidance.
* **Custom Bridge, Life Support & Survivability Suite (Decision 64):** Three control stations, a protected interior, two O2 tanks, emergency ice, cryo chambers, independently sealable compartments, airlocks, decoys, a blackout beacon, projector, and optional scuttle blocks. These provide only the behavior supported by the installed vanilla/DLC/TMS blocks and configured scripts. There is no validated block total for this revision.
* **Tri-Mode Laser Multitool Salvage & Remote Welding Suite (Decision 65):** Two Simple Laser Multitools use the listed ToolCore/Simple Laser Multitool mods. Mode switching, salvage collection, raster sweeps, and remote fabrication depend on mod behavior and a compatible controller. Do not assume 100% component retention or a 35 m beam range; test the installed version and report measured results.
* **Multi-Tier Fleet Comms & Logistics (Decision 66):** IGC dispatch, JIT logistics, ore triage, scouting, and swarm coordination are software protocols, not native block functions. Their messages must match the fleet protocol and be implemented in compatible PB scripts on every participating grid. Keep the listed message names as proposed interfaces until verified against the shared IGC contract.
* **Consist Mechanics, 3-Axis Hitch & FOB Autonomy (Decision 67):** The articulated hitch, DPU controls, and detached-FOB behavior require validation and/or PB scripts. Keep all grids stopped and secured for crew movement across any external gangway; the moving joint is neither airtight nor walkable. Do not land a shuttle on the utility deck until the deployed support configuration has passed a measured load test.
* **4× Multitool Repair Turrets, SML Auto-Production & Mule Peer Resupply Sorties (Decision 68):** Planned capabilities include four modded laser multitool turrets, PB-managed component quotas, an export cargo container and sorter, and peer resupply. Turret reach/coverage, cross-grid repairs, automatic production, and transfer rates require the installed mods, compatible scripts, and in-game tests; the block list does not guarantee them. Use the Chapter 9 `TMS.RESUPPLY.REQ` contract for requests. Offer/dispatch/complete channels and mission-kit schemas are not defined there and remain proposed until documented. No validated block/mass/PCU total is established for this revision.
* **Extended Survival Life Support & Environmental Hardening (Decision 69):** The separate carriage interiors can be airtight if built and tested as independent rooms. Airlocks can recover some room oxygen when vented into connected tanks; there is no UV decontamination, lightning-diversion, toxic-gas filtering, radiation-shielding, or waste-heat jacket mechanic in the listed blocks. Treat those as visual/role-play concepts, not functional protections. Validate suit-support duration against actual inventory consumption.
* **Automated Meteor Defense & Munitions (Decision 70):** Turret settings, ammunition production, event handling, and resupply are separate systems. Turrets and sorters do not manufacture ammunition, broadcast IGC messages, or execute PB arguments on their own; add and commission the corresponding PB/timer wiring before relying on those behaviors. Meteor interception, coverage, and resupply rate are test targets, not guarantees.

### Design Constraints and Build-Order Rules

* **Grid coordinates:** Coordinates identify block cells on the tractor datum. Use whole-block coordinates for armor and block placement. Values ending in `.5` in mechanical callouts describe an intended joint midpoint only; mount subgrid blocks to the appropriate attachment face and verify the actual pivot axis in-game.
* **Carriages and pressure:** Build and pressure-test the tractor and utility interiors separately. The external bellows, hitch, and any conveyor/rotor connection do not create a shared airtight room. Never cross the hitch while moving, under load, or with the joint unlocked.
* **Subgrids:** Every rotor, hinge, piston, connector, and merge interface must be built and tested as a mechanical connection. Confirm each intended conveyor path and power path through the actual block ports; an ASCII line is not proof of a working connection.
* **Automation boundary:** Event Controllers can observe supported block conditions and invoke configured terminal actions. They do not run arbitrary PB arguments, execute IGC protocols, perform inverse kinematics, or coordinate multiple grids. Those behaviors require installed PB scripts and/or explicitly configured timer chains. Each listed command is a software requirement until its controller is installed and acceptance-tested.
* **Mods:** The seismic feet and laser multitools require their listed TMS mods and dependencies. Keep the Mastodon structural blocks vanilla/DLC; Concrete Armor is for the Caterpillar/Ground Base roadbed use described by the fleet manual, not a required Mastodon block.
* **No unsupported environmental protection:** Decoys are targeting countermeasures, not lightning rods. Armor, hydrogen tanks, lights, and vents do not provide radiation shielding, toxic-gas filtration, UV sterilization, or thermal-jacket behavior. Do not use those labels as a survival guarantee.
* **Operating Environment:** Universal planetary & terrestrial surface operations (Earth 1 g baseline, airless vacuum moons, thin-atmosphere deserts, and hostile alien terrain).

### Script-Ready Physical Interface Contract

The following are physical provisions and future integration contracts, not installed automation. Mount PBs on the grid whose blocks they control, give each controlled device a stable terminal name, and retain manual controls and a tested stop path. Keep local machine control separate from fleet messaging. Physics and actuator state belong on `Update10`; low-rate IGC and display work belongs on `Update100`. Persist only the volatile state required to resume safely. Do not use an Event Controller as a substitute for PB logic, inter-grid communications, or numeric actuator control.

| Bespoke system | Physical build / interface | Future PB responsibility | Safe commissioning boundary |
|---|---|---|---|
| Tractor traction, utility DPU, and articulated hitch | Separate wheel groups; named yaw, pitch, and roll joints; connector/merge and clamp state; accessible driver station and stop control (Steps 10.4, 10.10–10.11, 10.29–10.30) | Read joint and connector state; coordinate speed and joint targets only when a compatible controller is fitted; inhibit motion on invalid/stale state | Manual steering and braking remain available. Test each joint at crawl speed before coupling; do not energize high-speed clamps or severance while moving. |
| Outriggers and seismic feet | Individually named pistons/feet with clear deployment envelopes and service access (Steps 10.14, 10.43) | Sequence deploy/retract and report piston limits; mod-specific scanning needs a compatible mod/controller | Manual controls remain available. No leveling, anchoring, ground lock, or load capacity is assumed. |
| 5-DOF excavation and recovery arm | Named yaw/shoulder/elbow/wrist joints, tool mounts, accessible local control, and stow clearance (Steps 10.15–10.20) | Optional inverse kinematics, tool modes, collision checks, and travel-stow sequence; read joint limits and operator enable state | Manual joint/tool control remains possible. Require low-speed tests, explicit operator enable, joint-limit checks, and an independent stop before any macro. |
| Nuclear, hydrogen, solar, and battery power | Named generators, batteries, breakers/sorters where fitted, and local manual controls (Steps 10.7–10.8, 10.31, 10.38) | Optional measured-load policy and generator sequencing; report measured state rather than assumed capacity | No automatic failover is implied. Verify start/stop, fuel, charge, and load shedding manually before enabling any sequence. |
| Ore, tailings, and conveyor distribution | Named cargo, sorter, ejector, and refinery/assembler blocks with physically tested ports (Steps 10.3, 10.5–10.6, 10.32–10.33, 10.43) | Optional inventory quotas, routing, and jam/overflow alerts | Keep transfer manual until each path is tested on the assembled grids. A line in a schematic or across a joint does not establish conveyor continuity. |
| Field workshop and component production | Refinery, assembler, input/output holds, and accessible inventory terminals (Steps 10.32–10.33, 10.36) | Optional recipe queues and stock policies through supported terminal interfaces | Verify recipes, conveyor access, and output capacity in small batches; never promise zero-manual-handling production. |
| Crew circulation, airlocks, and life support | Separate pressure-testable rooms; inner/outer doors, vents, ladders, suit supplies, and manual door controls (Steps 10.9, 10.21, 10.27, 10.32, 10.34–10.35) | Optional door interlock and pressure sequencing; monitor only available block state | The hitch is not a passage. Keep a manual recovery route; test seals and door interlocks before occupied use. No decontamination or complete oxygen recovery is implied. |
| Bridge C2 and machine controls | Three stations, local PB rack, named actuator groups, displays, physical button panels, and reachable manual stop controls (Steps 10.22, 10.24–10.26) | Optional command binding and status presentation; separate high-frequency machine control from low-frequency display/network work | Buttons and LCDs do nothing until explicitly bound. Confirm controls on each station and test loss-of-PB/manual takeover. |
| Fleet communications and handover | Radio/laser antennas and a dedicated C2 PB on each participating grid (Steps 10.24, 10.36) | Use only Chapter 9 channels/payloads that are defined and implemented; any election, Caterpillar dispatch, or resupply lifecycle extension needs a documented shared contract | Verify range, line of sight, stale-message handling, and duplicate-message behavior. Never infer a supported channel from a label or diagram. |
| Kestrel/Mule handling and repair | Marked pads/cradles, connectors or landing interfaces, landing clearances, welders, and access walkways (Steps 10.37–10.41) | Optional dock-state monitoring, welder enable, and transfer coordination | Manual dock/undock and welder cutoff remain available. Test with an empty craft; do not infer landing load, repair coverage, or automatic docking. |
| Defense and ammunition | Named turrets, ammunition storage, tested conveyors, and operator-accessible enable controls (Steps 10.13, 10.25, 10.28, 10.31, 10.35B, 10.36B, 10.44–10.44B) | Optional local warning or stock reporting; PB/timer integration is required for production, restock, and fleet coordination | Verify arcs and ammunition in a controlled world. No automatic meteor interception, lightning protection, or fleet defense is promised. |
| Resupply and Caterpillar transfer | Named export hold, sorter, stern connector, alignment markings, and physical approach clearance (Steps 10.36, 10.43–10.44B) | Use `TMS.RESUPPLY.REQ` for requests; offer/dispatch/completion and Caterpillar dispatch require a separately documented protocol | Begin with stationary, small inventory transfers; monitor connector state and manually stop if alignment or inventory behavior is unexpected. |
| Emergency signaling and scuttle hardware | Isolated beacon power, accessible alarm/abort controls, and physically separate warning controls (Steps 10.13, 10.25) | Optional status transmission only where a documented channel exists; no automatic detonation sequence is part of the interface contract | Test with destructive blocks absent or disarmed. Preserve an independent manual abort and never rely on an unvalidated timer chain. |

**Common interface rules:** identify a PB, its controlled grid, block groups, inputs, outputs, operator override, and failure behavior before commissioning it. Store persistent state in `Storage`; keep periodic work allocation-aware; and do not let loss of IGC, antenna, or script execution silently enable hazardous motion. Any future script must conform to the shared Chapter 9 contract and the project’s Space Engineers PB runtime constraints.

---

## 0 · Reference Grid, In-Game Paint Palette & Master Consist Layout

### 0.1 Coordinate Triad & Axis Conventions
* **Datum:** Coordinates are whole-block cell indices on the Tractor grid. `[X0, Z0]` is a named centerline reference on the Tractor, not the geometric center of the complete consist. `L1` is the chassis datum; each higher level is one 2.5 m block step. Half-cell values denote mechanical pivot midpoints only, never armor placement cells.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. The armored body is 11 blocks wide (X −5 through +5); wheel/tire protrusions extend beyond that body and must be included when checking total width and clearance.
* **Y (Elevation):** `L1` is the chassis reference. Increase one level per 2.5 m block. The L7 gantry is seven level steps above L1; the overall height target includes the wheels and gantry, not just armored blocks.
* **Z (Length / Longitudinal):** Stern is negative (−), Bow is positive (+). The Tractor body uses 20 block cells (Z −7 through +12); the Utility body uses 18 block cells (Z −26 through −9); Z −8 is the nominal 1-block mechanical gap. The bodies total 38 blocks; the gap and external hitch/bumper hardware bring the target overall length to 42 blocks. Verify actual extents against the built grid.
  * Tractor Carriage (`MAS-01-T`): 20 block cells, Z −7 through +12. Axle positions remain the local callouts in Phase 10-1.
  * Articulated Hitch Gap: Z −8 is reserved for the parent-grid attachment and clearance envelope. Rotor/hinge stack positions are defined from mating faces in Step 10.10.
  * Utility Carriage (`MAS-01-U`): 18 block cells, Z −26 through −9.

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
| **Slot 3** | Excavator Ochre | `#E67E22` | Deep Orange | Articulated boom arm casings and laser tool head |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Ore Conveyor Spine, bow intake throat, manifold junctions |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Tractor ore buffer and Utility component storage holds |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Tractor Power Vault (2× Small Nuclear Reactors, 2× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Forward Armored C2 Bridge exterior shell, operator consoles |
| **Slot 8** | Scout Flight White | `#ECF0F1` | Clean White | Forward twin Kestrel scout drone landing pads (P1, S1) on Utility roof |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Exterior catwalks, maintenance ladders, corner outrigger foot pads |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

To achieve an authentic brutalist industrial locomotive aesthetic and maintain spatial orientation during field construction, paint must follow strict functional zone lines across the hull:

```
[EXTERIOR ELEVATION LIVERY MAP · PORT PROFILE (LOOKING STARBOARD)]
Level
L7  │                                                                 [SLOT 7: NAVY CIWS / MT]
L6  │                      [SLOT 7: NAVY COMM SPIRE]                  [SLOT 1: AMBER MULE CRADLE]
L5  │                     /   [SLOT 7: NAVY CAB]    \                /  [SLOT 8: WHITE KESTREL]  \
L4  │ ═══════════════════[SLOT 0: INDUSTRIAL DARK GRAY MAIN HULL & WORKSHOP ARMOR]═════════════════
L3  │ ───────────────────[SLOT 2: HEAVY RAIL CRIMSON BELTLINE STRIPE (Z +12 to Z −26)]──────────
L2  │ ═══════════════════[SLOT 0: INDUSTRIAL DARK GRAY LOWER CHASSIS & SPONSONS]══════════════════
L1  │      (O)====(O)====(O)           [SLOT 2: DRAWBAR]           (O)====(O)   [SLOT 2: BUMPER]
        Axle 1  Axle 2  Axle 3        [SLOT 9: GANGWAY]          Axle 4  Axle 5
```

| Zone / Component | Primary Palette Slot | Hex Code | Visual Application & Boundaries |
| :--- | :--- | :--- | :--- |
| **Lower Keel & Skids (L1)** | Slot 0 · Dark Slate | `#2B303A` | Underside belly channel, skid plates, wheel hubs, axle mount blocks |
| **Running Gear Fenders (L2)** | Slot 0 · Dark Slate | `#2B303A` | Flared mudguards, wheel well sponsons, chassis box girders |
| **Hull Beltline Accent (L3)** | Slot 2 · Rail Crimson | `#9E2A2B` | Continuous 1-block perimeter racing stripe from bow glacis to stern |
| **Superstructure Walls (L3-L5)** | Slot 0 · Dark Slate | `#2B303A` | Main hull armor slabs, workshop sidewalls, reactor vault casing |
| **C2 Armored Bridge Cab (L4-L5)**| Slot 7 · Fleet Navy | `#1B263B` | Armored cab visor, window mullions, console pedestals, comms spire |
| **Articulated Excavation Arm** | Slot 3 · Ochre Orange | `#E67E22` | Boom arm girders, elbow hinges, laser tool head casing |
| **Mule Gantry Cradle Pad (L6)** | Slot 1 · Hazard Amber | `#E6A100` | 7×7 blast deck border, motorized cradle rub rails, alignment arrows |
| **Kestrel Scout Pads (L5-L6)** | Slot 8 · Scout White | `#ECF0F1` | 4×4 drone landing squares (P1, S1) on forward utility carriage roof |
| **Personnel Walkways & Catwalks**| Slot 9 · Safety Yellow | `#F1C40F` | Grated catwalks, ladders, stair runs, boarding thresholds |
| **Emergency & High-Voltage** | Slot 6 · Alert Red | `#C0392B` | Nuclear reactor access hatches, battery vault markings, scuttle panels |

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
        │    [BOOM BASE] ── (35 m target, articulated 5-DOF laser arm)         │ │          (Flush Heavy Armour Blast Deck            Dump   │
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
                     │ (Deployed reach target: up to 35 m)                         │ MULE GANTRY PAD │ 2× KESTREL PADS │  Flight Roof (L6)
        ┌────────────┴───┐                                                        │ (7×7 B Blast)   │ (Scout Wings)   │
        │ ARMORED BRIDGE │                                     [DRAWBAR HITCH]    ├─────────────────┴─────────────────┤
        │ (Panoramic C2) │   [POWER CORE]    [ORE BUFFER]     2-Axis Hinge Joint  │ FIELD WORKSHOP  │ COMPONENT HOLD  │  Main Deck (L3-L5)
[BOW]   │ [ORE DETECTOR] │   (2× Reactors)   (L Cargo Hold)    3-Axis Drawbar     │ (Assembler/Med) │ (Spare Parts)   │  [STERN DUMP]
════════╧════════════════╧═══════════════════╧═════════════════════════╦══════════╧═════════════════╧═════════════════╧═══════════► (To Cat Train)
[STABILIZER]           [WH 1..2]           [WH 3..4]        [WH 5..6]  ║        [WH 7..8]         [WH 9..10]      [STABILIZER]
(Voxel Clamp)         (Steer +30°)        (Steer +15°)     (Drive 0°)  ║       (Drive 0°)        (Steer −25°)     (Voxel Clamp)
```

---

### 0.4B Consist 3/4 Isometric Profiles & Stance Wireframes

```
[3/4 ISOMETRIC PERSPECTIVE · TRACTOR BOW & DORSAL SPINE]
Looking Down & Aft from Forward Starboard Vantage

                 [LASER COMM SPIRE L6]
                         │
                 [BOW CIWS L5]   [BOOM AZIMUTH PEDESTAL L4]
                       │                 │
                ┌──────┴──────┐          ▼
               ╱  CAB VISOR   ╲     ┌─────────┐
              ╱   CANOPY 45°   ╲════╡  TURRET ╞══════[DORSAL SPINE TROUGH]════►
             │  PANORAMIC C2    │   └────┬────┘      (14-Block Recessed Canal)
             ├──────────────────┤        │
       ┌─────┴──────────────────┴─────┐  │
      ╱   FORWARD CHIN PLOW / GLACIS   ╲ │
     │  [X0, Z +12 HEAVY ARMOR PROW]    ││
     └──────┬────────────────────┬──────┘│
            │                    │       │
       [AXLE 1-P]           [AXLE 1-S]   │
       (5×5 Off-Road)       (5×5 Off-Road)
```

```
[3/4 ISOMETRIC PERSPECTIVE · CARRIAGE 2 STERN GANTRY & FLIGHT DECK]
Looking Down & Forward from Aft Starboard Vantage

               [STERN CIWS L7]     [GANTRY REPAIR TURRET L7]
                      │                     │
               ┌──────┴─────────────────────┴──────┐
               │    HEAVY GANTRY CLEARANCE ARCH    │ (L7, Height: 17.5 m)
               └───┬───────────────────────────┬───┘
                   │  [MULE-01 CRADLE PAD L6]  │
                   │   7×7 B Flush Heavy Deck  │
        ┌──────────┴───────────────────────────┴──────────┐
       ╱  [KESTREL PAD P]                 [KESTREL PAD S]  ╲
      │    4×4 Recon Pad                   4×4 Recon Pad    │
      ├─────────────────────────────────────────────────────┤
      │            STERN DUMP OVERHANG & PUSH-BLOCK         │ (L2-L3)
      └─────────┬─────────────────────────────────┬─────────┘
                │                                 │
           [AXLE 5-P]                        [AXLE 5-S]
           (5×5 Off-Road)                    (5×5 Off-Road)
```

---

### 0.4C Transverse Hull Station Profiles & Chamfer Specifications

To avoid uniform "shoebox" cross-sections, the Mastodon utilizes specific Space Engineers armor block morphologies (`Armor Slope 2×1×1 Base/Tip`, `Half-Slope`, `Inverted Corner`, `Half-Block`) across 5 critical hull stations:

#### Station 1 · Bow Prow & Cab Glacis (Z = +11, Looking Aft)
```
           X: −3       X: −2        X0         X: +2       X: +3
L5 Crown         ┌───────────┬──────────────┬───────────┐
                 │ 2×1 SLP \ │ FLAT ROOF L5 │ / 2×1 SLP │   ◄── Cab Brow Visor (Inward Chamfer)
L4 Bridge        ├───────────┴──────────────┴───────────┤
                 │ 45° SLOPED HEAVY GLASS CANOPY (7 W)  │   ◄── Panoramic Glacis (L4 Deck)
L3 Sills   ┌─────┴──────────────────────────────────────┴─────┐
           │ 2×1 INV \      WALKWAY AISLE L3      / 2×1 INV   │   ◄── Outward Flare to Main Hull
L2 Spons   ├─────────┬──────────────────────────────┬─────────┤
           │ HALF-SLP│     CHASSIS BOX GIRDERS      │ HALF-SLP│   ◄── Mudguards over Axle 1
L1 Keel    └────┬────┴───────┬──────────────┬───────┴────┬────┘
                │            │ BELLY TROUGH │            │
                ▼            └──────────────┘            ▼
          [WHEEL 5×5 P1]       (1.25 m Keel)       [WHEEL 5×5 S1]
```

#### Station 2 · Mid-Tractor & Boom Pedestal (Z = +4, Looking Aft)
```
           X: −4       X: −2        X0         X: +2       X: +4
L4 Pedestal      ┌───────────┬──────────────┬───────────┐
                 │ 2×1 SLP \ │ AZIMUTH BASE │ / 2×1 SLP │   ◄── Turret Pedestal Coamings
L3 Hull    ┌─────┴───────────┴──────────────┴───────────┴─────┐
           │ SLP 45° \   JEFFERIES    BUFFER     SIDE ARMOR   │   ◄── Symmetrical Flank Chamfers
L2 Deck    ├─────────┬──────────────────────────────┬─────────┤
           │ HALF-BLK│   PRIMARY CHASSIS LONGERONS  │ HALF-BLK│   ◄── Axle 2 Wheel Arches
L1 Keel    └────┬────┴───────┬──────────────┬───────┴────┬────┘
                │            │ ORE CONVEYOR │            │
                ▼            └──────────────┘            ▼
          [WHEEL 5×5 P2]       (Processing)        [WHEEL 5×5 S2]
```

#### Station 3 · Inter-Carriage Hitch & Gangway (Z = −8 to −9, Looking Aft)
```
           X: −3       X: −1        X0         X: +1       X: +3
L4 Roof          ┌───────────┬──────────────┬───────────┐
                 │ 2×1 SLP \ │ HULL COAMING │ / 2×1 SLP │   ◄── Spine Trough Termination
L3 Gangway ┌─────┴───────────┴──────────────┴───────────┴─────┐
           │ SLP 45° \   FLEXIBLE ACCORDION GANGWAY   / SLP 45│   ◄── Weather/Dust Guard Only
L2 Drawbar ├─────────┬──────────────────────────────┬─────────┤
           │ 2×1 BASE│ 3-AXIS GIMBAL DRAWBAR HITCH  │ 2×1 BASE│   ◄── Heavy Articulated Tongue
L1 Skids   └─────────┴───────┬──────────────┬───────┴─────────┘
                             │ MAGNETIC PAD │
                             └──────────────┘ (Emergency Breakaway Keel Skid)
```

#### Station 4 · Workshop & Radiation Citadel (Z = −14, Looking Aft)
```
           X: −4       X: −2        X0         X: +2       X: +4
L6 Catwalk       ┌───────────┬──────────────┬───────────┐
                 │ CATWALK   │ KESTREL WING │  CATWALK  │   ◄── Flight Deck Walkways
L5 Roof    ┌─────┴───────────┴──────────────┴───────────┴─────┐
           │ 2×1 SLP \   FLIGHT DECK BLOCKED HULL   / 2×1 SLP │   ◄── Upper Superstructure
L4 Upper   │ VERTICAL│    REFINERY     ASSEMBLER    │ VERTICAL│
L3 Deck    │ 2×1 INV \   CORRIDOR     CITADEL VLT   / 2×1 INV │   ◄── Radiation Citadel Armor
L2 Vault   ├─────────┬──────────────────────────────┬─────────┤
           │ HALF-SLP│ SUB-FLOOR BATTERIES & AMMO   │ HALF-SLP│   ◄── Mudguards over Axle 4
L1 Keel    └────┬────┴───────┬──────────────┬───────┴────┬────┘
                │            │ BALLAST SKID │            │
                ▼            └──────────────┘            ▼
          [WHEEL 5×5 P4]       (40× Plates)        [WHEEL 5×5 S4]
```

#### Station 5 · Stern Gantry Arch & Caterpillar Dump (Z = −23 to −26, Looking Aft)
```
           X: −3       X: −1        X0         X: +1       X: +3
L7 Arch          ┌───────────┬──────────────┬───────────┐
                 │ CIWS MOUNT│ GANTRY ARCH  │ REPAIR MT │   ◄── Overhead Clearance Arch
L6 Flight  ┌─────┴───────────┴──────────────┴───────────┴─────┐
                 │ MULE-01 ELEVATED CRADLE BED (7×7)│             ◄── Heavy Blast Deck
L4 Stern   │ 2×1 SLP \   COMPONENT STERN HOLD       / 2×1 SLP │
L3 Mezz    │ VERTICAL│   LADDER & DECON AIRLOCK     │ VERTICAL│
L2 Bumper  ├─────────┴──────────────────────────────┴─────────┤
           │ HEAVY RAIL CRASH BUFFER & TAILINGS CHUTE (X0)    │   ◄── Caterpillar Push-Dock
L1 Skids   └──────────────────────────────────────────────────┘
```

---

### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

```
               ARTICULATED LASER BOOM (35 m reach target; Dorsal Pedestal)
                     [Tool Head: 2× Simple Laser Multitools]
                                         │
                                         ▼ (Internal Large Hinge / Rotor Pass-Thru)
                       [TRACTOR ORE BUFFER & SUMP] (3×3×3 B Large Cargo Hold)
                       ◄── (Physical Cargo Hatch: Port Jefferies Tube [X −2, Z +2, L3])
                                         │
                                         ▼ (3-Axis Drawbar Articulated Conveyor Pipe)
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
                                  (Capacity to be verified in game)
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
| `MAS Stern Sorter` | [X0, Z −24, L2] | Rear Bumper Access Door | Stern ladder descent access to service bulk ore dump sorter |

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
| **Mode 1: Silent Watchkeeping (Eco)** | Solar Panels (up to `≈ 640 kW` in ideal conditions) · Nuclear & H2 OFF | `RECHARGE / FLOAT` | Intended parked mode. Verify generation against the actual loads and daylight; do not assume indefinite life support or battery charging. |
| **Mode 2: Tractive Cruise** | 1× Small Reactor (15 MW) + Solar | `AUTO` (Peak Shaving) | Overland cruising. Reactor powers 10× 5×5 wheel drives (`≈ 8-12 MW`†); batteries absorb acceleration spikes. |
| **Mode 3: Heavy Excavation** | 2× Small Reactors (30 MW) + Solar | `AUTO` (Burst Assist) | Powers the boom joints, tools, outriggers, refinery, and assembler subject to measured grid load; laser-tool draw depends on the installed mod. |
| **Mode 4: Emergency Failover** | 1× H2 Engine (up to 5.0 MW) · Nuclear OFF | `DISCHARGE` | Optional operator/PB failover. Test startup, available power, and load shedding; no automatic response is supplied by the block list. |
| **Mode 5: Shore Power / Fast Charge** | External Depot Grid via Roof or Stern | `RECHARGE` (as configured) | Test battery charge behavior at the selected connector. Hydrogen transfer requires a separate compatible conveyor/tank path. |

---

### 0.7 Provisional Master Consist Parts Schedule (BOM)

The table is the current planning schedule, not a certified blueprint manifest. Its subassembly estimates are not a substitute for counting blocks in the finished grid. The listed row totals are 1,128 entries, approximately 1,258.2 t, and 10,080 PCU by arithmetic; those values remain unverified and must not be used for load or survival planning. Replace them with the in-game totals after the complete consist is built.

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
| **Interior Walkways, Decoys, Lockers & Airlock Vestibules** | Grated catwalks, door vestibules to pressure-test, sliding doors, ladder runs, standard work lights, 3× Decoys, 2× Keel Recovery Gyros, 2× Armory Lockers, 1× Survival Locker, 3× Event Controllers (Palette 9) | 44 | 24.9 t | 660 |
| **Field Workshop & Universal Life Support** | 1× Basic Refinery, 1× Large Assembler, 1× Medical Room, 2× Cryo Pods, 1× Survival Kit, 2× Large O2 Tanks, 1× Quarantined Ice Hold, 2× Large H2 Tanks, 2× O2/H2 Gens, 6× Air Vents, 1× Master Projector, 1× Scuttle Warhead | 22 | 68.2 t | 815 |
| **Utility Cargo Hold & Logistics Feed** | 1× Large Cargo Container (Components), 2× Munitions Sorters (`MAS Sorter Ammo Fwd / Parts Aft`), Conveyor network | 18 | 29.0 t | 210 |
| **Flight Decks, Welders & Solar** | 1× Mule Pad, 2× Kestrel Pads, 2× Drone Welders, 4× Solar Panels, 4× Floodlights | 67 | 61.1 t | 970 |
| **Stern Stabilizers, Telescoping Dock & Dump** | 2× Large Outrigger Pistons + 2× Mag Feet (`MAS Outrigger Utility`), 1× Telescoping Dock Piston (`MAS Piston Stern Dock`), Stern Dump Connector & Sorter, Relocated Tailings Ejector | 7 | 14.5 t | 510 |
| **Utility FOB Autonomy & Deadman Brake** | 1× PB (`[MAS-U] PB Utility`), 1× Remote Control (`MAS-U Remote FOB`), 1× Event Controller (`MAS-U EC Breakaway`), 2× Keel Mag Skids (`MAS Utility Brake Keel 1, 2`) | 5 | 4.0 t | 350 |
| **Perimeter Defense Suite** | 2× Gatling Turrets, 2× Interior Turrets, 1× Sub-Floor Bunker (`MAS Hold Munitions`), 1× Ammo EC (`MAS EC Ammo`), 1× Siren (`MAS Alarm Meteor`), Conveyor Feeds (Palette 7) | 15 | 10.7 t | 443 |
| **Multitool Repair Turrets** | 4× Large Laser Multitool Turrets (Bow L5, Port/Stbd L4 shoulders, Stern L7 arch · Mod 3175774286) | 4 | 18.0 t | 900 |
| **Peer Resupply Export Staging** | 1× Small Cargo (`MAS Hold Resupply Export`), 1× Conveyor Sorter (`MAS Sorter Resupply Export`), 9× Gantry Arch Heavy Armor blocks (Palette 0/3) | 11 | 3.0 t | 37 |
| **External EVA, Lights & Failsafes** | 2× Ground EVA Lockers, 3× Keel Lights, 1× Black Box SOS Beacon & Isolated Battery, 4× In-Situ Button Panels (Palette 9) | 11 | 2.5 t | 110 |
| **PLANNING TOTAL (UNVERIFIED)** | Arithmetic sum of current subassembly estimates; reconcile from completed blueprint | **1,128** | **≈ 1,258.2 t** | **10,080** |

---

### 0.8 Master Player Circulation, Access Corridors & Jefferies Tubes Schematic

```
[LONGITUDINAL PROFILE · CREW CIRCULATION & MAINTENANCE PATHS (LOOKING STARBOARD)]

     ◄────────── CARRIAGE 1: TRACTOR / EXCAVATOR ──────────►     ◄────────── CARRIAGE 2: FOB / UTILITY ──────────►
     L6                                                         ┌──[KESTREL]──────[MULE PAD CATWALK]──────┐
     L5      ┌──[C2 BRIDGE]──────[BOOM PEDESTAL DECK]           │ (L6 Catwalks & Solar Array Access)      │
     L4      │ (3-Seat C2/Alarm) │ (Turret Service Hatch)       ├─────────────────────────────────────────┤
     L3 ┌────┴───────────────────┴─────────────────────────┐    │ [FIELD WORKSHOP & MEDICAL HABITAT]      │
        │ [PORT JEFFERIES TUBE] ──► [POWER CORE VAULT]      ╞════╡ (Refinery · Assembler · MedBay · Tanks) │
     L2 │ (Access to Buffer & Core) (Reactors / H2 Engine)  │    │ [BATTERY VAULT & SUB-FLOOR AMMO BUNKER] │
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
    Z: +10 │ [ARMORY BRG]  │ [SURVIVAL KIT][SUIT CHARGER]  │ [LOCKER GEAR] │  ◄── Emergency Armory & Medical
    Z:  +8 ├───────────────┴───────────────┬───────────────┴───────────────┤
           │ [AIRLOCK INNER DOOR]          │ [BOOM LADDER / METEOR SIREN]  │  ◄── Vestibule & Siren (L4 Overhead)
    Z:  +7 │ [AIRLOCK VESTIBULE (VENT)]    │ [SOLID ARMOR]                 │
    Z:  +6 ├───────────────┬───────────────┴───────────────┬───────────────┤
           │ [AIRLOCK OUTER│
           │  DOOR & LADDER│
    Z:  +3 │ [PORT JEFF-   │ [MAS TRACTOR BUFFER (3×3×3)]  │ [SOLID ARMOR] │  ◄── Ore Buffer Pass
           │  ERIES TUBE]  │ (Manual Port on Port Flank)   │               │
    Z:  +1 │ (Catwalk &    │                               │               │
           │  Lighting)    │                               │               │
           ├───────────────┼───────────────┬───────────────┼───────────────┤
    Z:  −2 │ [VAULT DOOR]  │ [BAT 1]       │ [REACTOR 1]   │ [BAT 2]       │
    Z:  −3 │               │ [BAT 3]       │ [REACTOR 2]   │ [ARMOR]       │  ◄── Power Core Vault
    Z:  −4 │               │ [ARMOR]       │ [H2 ENGINE]   │ [ARMOR]       │
    Z:  −7 ├───────────────┴───────────────┴───────────────┴───────────────┤
    Z:  −8 │ ══════════════ [GANGWAY FORWARD SLIDING DOOR] ═══════════════ │  ◄── Aft Bulkhead
           ╞═══════════════════════════════════════════════════════════════╡
           │  [EXTERNAL ARTICULATED GANGWAY (NOT PRESSURIZED)]             │  ◄── Cross only when stopped/secured
           ╞═══════════════════════════════════════════════════════════════╡
    Z:  −9 │ ══════════════ [GANGWAY AFT SLIDING DOOR] ═══════════════════ │  ◄── Utility Fwd Bulkhead
           ├───────────────┬───────────────┬───────────────┬───────────────┤
    Z: −10 │ [SOLID ARMOR] │               │ [FLIGHT LADDER UP TO L6]      │  ◄── Stair/Ladder to L6
           ├───────────────┤ [CENTRAL      ├───────────────┬───────────────┤
    Z: −12 │ [REFINERY]    │  WORKSHOP     │ [MEDICAL BAY] │ [CRYO POD 1]  │  ◄── Field Metallurgy
    Z: −13 │ [ARMORY WRK]  │  AISLE        │ [SUIT CHARGE] │ [CRYO POD 2]  │  ◄── Workshop Armory & Recharger
    Z: −14 │ [ASSEMBLER]   │               │ [O2/H2 GEN 1] │ [O2/H2 GEN 2] │  ◄── Fabrication & O2 Reserves
    Z: −15 │ [EC AMMO]     │               │ [AMMO BUNKER (L2 UNDER)]      │  ◄── Sub-Floor Ammo Hold & Watchdog
    Z: −18 │ [H2 TANK 1]   │               │ [H2 TANK 2]   │ [ARMOR]       │  ◄── Fuel Bunkering & Storm Shield
    Z: −22 │ [ARMOR]       │ [MAS HOLD COMPONENTS (3×3×3)] │ [ARMOR]       │  ◄── Parts Hold
    Z: −25 ├───────────────┴───────────────┬───────────────┴───────────────┤
    Z: −26 │                               │ [STERN DUAL-DOOR AIRLOCK L2]  │  ◄── Stern Dump Egress & Decon
           └───────────────────────────────┴───────────────────────────────┘
```

---

### 0.9 MASTODON-02 (MKII) Future Upgrade Concepts

The following are unverified upgrade concepts, not claims about released game content or a committed MKII build. Implement only when the installed game, DLC, mods, and tested block interfaces support the feature. The current MKI retains only the named baseline blocks and the physical hardpoints explicitly listed in this chapter.

| Subsystem Domain | MKI Pre-Warfare 3 Baseline | Planned MKII Warfare 3 Evolution | Pre-Allocated Sockets & Headroom |
|---|---|---|---|
| **Primary Dorsal Armament** | 2× Large Gatling Turrets (`MAS Turret Bow / Stern`) | Warfare 3 Heavy Kinetic / Directed-Energy Turrets | Centerline L5 dorsal hardpoints plumbed to large conveyor junctions. |
| **Close-In Anti-Personnel** | 2× Interior Turrets (`MAS Turret Flank Port/Stbd`) | Warfare 3 Point-Defense / Anti-Missile Systems | Flank L2 skirt sockets with 360° low-depression coverage. |
| **Electronic Warfare (EW)** | Passive radio/laser communication hardware; effective range unverified | Possible future active ECM / jamming / radar systems; availability and behavior unverified | Dorsal L6 comms mast socket reserved for future equipment; no EW capability is claimed. |
| **Power Grid Headroom** | 35 MW generation and 18 MWh battery reserve are design targets | Future weapon load capability depends on tested installed blocks | 72.0 MW peak battery discharge is an unverified target; measure available output under load. |
| **Armor Fairing Modularity** | Heavy armor composite hull with sloped glacis | Warfare 3 Modular Composite / Reactive Armor Plating | Wheel arch skirts and hitch drawbar pre-spaced for add-on armor tiles. |

---

## Phase 10-1 · Tractor Chassis & 6-Wheel Powertrain (Levels L1-L2)

```
       [PHASE 10-1 MASTER SPATIAL LOCATOR HUD]
       Envelope: 11 W × 20 L × 2 H B (27.5 × 50.0 × 5.0 m)
       Coordinate Range: X −5 to +5, Z −7 to +12, Y = L1 to L2
       Key Datums:
       · Tractor Body: X0, block cells Z −7 through +12 (20 cells)
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
│ [LOCATOR] Subsystem: Tractor Chassis Backbone  ·  Grid Zone: [X ±3, Z −7 to +12]     │
│ [VIEWPORT] Level: L2 (Deck)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT (−X) · STBD (+X)►   │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.1 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 40 | Slot 0 (`#2B303A`) | 34.0 t / 40 | 2× parallel 20 B longitudinal runs at X = ±3, Z −7 to +12 |
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
1. Lay two parallel 20-block runs of Heavy Armor Full Blocks at X = −3 (Port) and X = +3 (Starboard) from Z −7 through Z +12 at Level L2.
2. Weld transverse 5-block Heavy Armor ties bridging between X −3 and X +3 at three axle datums: Z = +9 (Axle 1), Z = +4 (Axle 2), and Z = −1 (Axle 3).
3. Cap the forward end at Z = +12 with a solid transverse bulkhead tie (X −3 to +3).
4. Cap the aft end at Z = −8 with a solid transverse bulkhead tie (X −3 to +3) to anchor the future drawbar hitch.
5. Paint all structural armor blocks with Palette Slot 0 (`#2B303A` Industrial Dark Gray).

**✓ Check:** Two rigid parallel box girders at X = ±3 run 20 body cells from Z −7 to Z +12. The separate aft hitch cross-tie at Z −8 is interface hardware, not an additional body cell; keep the rotor/hinge clearance shown in Step 10.10.

---

### Step 10.2 · Underside Half-Slopes & V-Bevel Belly Skid Channel (L1)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Tractor Belly Deflector  ·  Grid Zone: [X −2 to +2, Z −7 to +12] │
│ [VIEWPORT] Level: L1 (Underside)  ·  Orientation: ▲ BOW (+Z) · ◄ PORT · STBD ►        │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.2 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Half-Slope | 40 | Slot 0 (`#2B303A`) | 17.0 t / 40 | Slopes placed at X = ±1, Z −7 through +12, facing inward toward X0 |
| Heavy Armor Half Block | 20 | Slot 0 (`#2B303A`) | 8.5 t / 20 | Centerline flush belly skid plates along X0, Z −7 through +12 |
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
1. Position Heavy Armor Half-Slope blocks along X = −1 from Z −7 to Z +12 at Level L1, angled so the sloped face bevels inward toward centerline X0.
2. Mirror on the starboard flank: place Heavy Armor Half-Slopes along X = +1 from Z −7 to Z +12 at Level L1, sloping inward toward X0.
3. Install continuous Heavy Armor Half Blocks along centerline X0 between the slopes, forming a flat skid keel 1.25 m above ground.
4. At the bow prow (Z = +11 to +12), construct the faceted bulldozer cow-catcher plow: install Heavy Armor Slope 2×1×1 Base blocks along Level L1 angled upward (+Y), capped with Slope 2×1×1 Tip blocks at Level L2, flanked by Heavy Armor Corner 2×1×1 blocks at X = ±2. This forms a compound 26.5° / 45° chiseled prow wedge deflecting boulders under the belly skid and clearing quarry rubble.

**✓ Check:** Underside forms an inverted, continuous trapezoidal trough along X0, providing 1.25 m (0.5 block) clearance to terrain with 45° beveled deflector flanks and a rigid bulldozer prow wedge.

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
5. Build Flared Wheel Arch Mudguards (L2-L3): Along X = ±4 to ±5 over each wheel hub (Z +9, +4, −1), cap the suspension brackets with Heavy Armor Half-Blocks at Level L2. Install Heavy Armor Half-Slopes along the outer sill at X = ±5 sloping downward 45°, framed by Inverted Slopes at the arch corners. This shields the suspension stators from quarry projectiles while leaving the 5×5 tire faces open for inspection and maintenance.

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

## Phase 10-2 · Tractor Power Core, Ore Buffer & 3-Axis Drawbar Hitch (Levels L2-L4)

```
       [PHASE 10-2 MASTER SPATIAL LOCATOR HUD]
       Envelope: 7 W × 14 L × 3 H B (17.5 × 35.0 × 7.5 m)
       Coordinate Range: X −3 to +3, Z −8 to +6, Y = L2 to L4
       Subsystem Datums:
       · Ore Buffer Hold: Z +1 to +3, centered at X0
       · Primary Power Core: Z −4 to −2, centered at X0
       · 3-Axis Drawbar Hitch Datum: [X0, Z −8, Y = L2]
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
| Heavy Armor Full Block | 24 | Slot 6 (`#C0392B`) | 20.4 t / 24 | Reinforced power-vault bulkheads; no radiation-shielding function |
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
3. Route the aft conveyor port toward the drawbar only if the assembled 3-axis joints provide a continuous, tested conveyor path. Verify actual connected hydrogen tank capacity in the terminal; no reserve volume is specified here.
4. Verify the manual fuel shutoff valve and crank sump port face the interior aisle for manual field maintenance.
5. Paint engine with Palette Slot 6 (`#C0392B` High-Voltage Alert Red).

#### Terminal Configuration & Sliders
* Name: `MAS Engine H2`
* Enabled: `OFF` (Cold-standby reserve)
* Supervised by: Event Controller `MAS EC Power`
  * Monitored: `MAS Reactor 1, 2` Uranium Inventory
  * Threshold: $< 1.0\text{ kg}$
  * Action: Turn `MAS Engine H2` ON; sound fuel alert chime

**✓ Check:** Hydrogen engine is securely mounted and its conveyor connection is confirmed. Test actual output and connected tank access in game before relying on failover.

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

**✓ Check:** Verify the crawlway is physically clear and pressure-test any intended room boundary in game. Cargo hatch opens for manual inventory access; button panel actions must be configured and tested.

---

### Step 10.10 · Heavy Motorized 3-Axis Drawbar Hitch & Emergency Separation Interface (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 3-Axis Gimbal Drawbar Hitch  ·  Grid Zone: [X ±2, Z −10 to −6] │
│ [VIEWPORT] Level: L2  ·  Orientation: Profile looking Port & Plan view (▲ Bow +Z)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.10 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Advanced Rotor | 2 | Slot 2 (`#9E2A2B`) | 17.6 t / 30 | `MAS Hitch Yaw` [X0, Z −8, L2] facing −Z; `MAS Hitch Roll` [X0, Z −9, L2] inline on Z-axis |
| Large Motor Hinge | 1 | Slot 2 (`#9E2A2B`) | 3.5 t / 15 | `MAS Hitch Pitch` [X0, Z −8, L3] horizontal pin for vertical pitch |
| Merge Block (Large Grid) | 1 | Slot 2 (`#9E2A2B`) | 1.2 t / 15 | `MAS Hitch Sever Tongue` mounted on drawbar tongue at [X0, Z −7, L3] facing Aft (−Z) |
| Large Magnetic Plate | 3 | Slot 2 (`#9E2A2B`) | 4.2 t / 90 | 2× `MAS Hitch Clamp Port/Stbd` [X ±2, Z −8, L2], used only while parked; 1× `MAS Hitch Capture Plate` on the utility nose at [X0, Z −10, L2] |
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

    Parked alignment restraints only:
    [MAS Hitch Clamp Port (X −2)] ────► Use only while parked and aligned
    [MAS Hitch Clamp Stbd (X +2)] ────► Keep clear of the articulation envelope during travel
```

#### Placement Instructions
1. Mount the stator of Large Advanced Rotor `MAS Hitch Yaw` on the Tractor Aft Bulkhead at [X0, Z −8, Y = L2], facing directly Aft (−Z). Connect its front port to the central conveyor raceway.
2. Attach the Large Advanced Rotor Head. Connect the internal large conveyor passage.
3. Mount the stator of Large Motor Hinge `MAS Hitch Pitch` at [X0, Z −8, L3] directly onto the yaw rotor head with the hinge barrel oriented horizontally (X-axis) for vertical pitch.
4. Mount the stator of Large Advanced Rotor `MAS Hitch Roll` at [X0, Z −9, L2] on the pitch hinge head, aligned horizontally along the Z-axis to permit up to ±20.0° torsional roll rotation.
5. Build the drawbar tongue using Heavy Armor blocks and mount Large Grid Merge Block `MAS Hitch Sever Tongue` at [X0, Z −7, L3] facing Aft (−Z) to connect with Carriage 2.
6. Install 2× Large Magnetic Plates (`MAS Hitch Clamp Port/Stbd`) at [X −2, Z −8, L2] and [X +2, Z −8, L2]. Engage only while both carriages are stationary and aligned; keep them clear of the joint articulation envelope during travel.
7. Install camera `MAS Cam Hitch` above the drawbar at [X0, Z −6, L4], facing Aft for manual docking alignment.
8. Construct beveled Heavy Armor guide horns on Carriage 2's nose to funnel the tongue within a ±1.2 m alignment tolerance.
9. Paint all drawbar components with Palette Slot 2 (`#9E2A2B` Heavy Rail Crimson).

#### Terminal Configuration & Sliders
* Rotor: `MAS Hitch Yaw`
  * Start with a low manual velocity; establish safe travel and braking from in-game tests.
  * Lower Limit: `−45.0°` · Upper Limit: `+45.0°` (design limits; verify clearance) · Share Inertia Tensor: test on the assembled subgrid
* Hinge: `MAS Hitch Pitch`
  * Start with a low manual velocity; establish safe travel and braking from in-game tests.
  * Lower Limit: `−25.0°` · Upper Limit: `+25.0°` (design limits; verify clearance) · Share Inertia Tensor: test on the assembled subgrid
* Rotor: `MAS Hitch Roll`
  * Start with a low manual velocity; establish safe travel and braking from in-game tests.
  * Lower Limit: `−20.0°` · Upper Limit: `+20.0°` (design limits; verify clearance) · Share Inertia Tensor: test on the assembled subgrid
* Group: `MAS Hitch Clamps` (`MAS Hitch Clamp Port`, `MAS Hitch Clamp Stbd`)
  * Keep unlocked during travel unless an independently tested mechanical design requires engagement; do not use clamps as a scripted high-speed articulation lock.
* Merge Block: `MAS Hitch Sever Tongue`
  * Enabled: `ON` (Structural hard-lock)

**✓ Check:** With both carriages supported and stationary, cycle each joint independently through a conservative range and inspect the full assembly for interference. Increase the range only after a low-speed coupled test. Confirm the three axes remain free during travel; the angle callouts are design limits, not a safe operating envelope.

---

### Step 10.11 · Hitch Articulation Clearance & Inter-Carriage Weather Guard (L2-L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Hitch Clearance & External Gangway Guard · Grid Zone: [X ±3, Z −9 to −8] │
│ [VIEWPORT] Level: L2-L3  ·  Orientation: Looking Port (−X) Longitudinal Section     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.11 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Sliding Door | 2 | Slot 9 (`#F1C40F`) | 2.4 t / 30 | `MAS Door Gangway Fwd` [X −2, Z −8, L3] and `MAS Door Gangway Aft` [X +2, Z −9, L3] |
| Grated Catwalk Block | 2 | Slot 9 (`#F1C40F`) | 0.6 t / 2 | External guard plates; not a pressurized crew passage |
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
1. Install Tractor Aft Gangway door `MAS Door Gangway Fwd` at [X −2, Z −8, Y = L3].
2. Install Utility Forward Gangway door `MAS Door Gangway Aft` at [X +2, Z −9, Y = L3].
3. Fit removable guard plates and flexible-looking armor fairings only where they clear the full hitch articulation range. Do not bridge the gap with a walkable floor or enclose the mechanical joint as an airlock.
4. The two doors seal their respective carriage interiors when closed; the space between them remains exterior. Crew movement across the gap is not part of normal operations. If maintenance requires access, stop and secure both carriages, depressurize as needed, and use a suit.
5. If a hitch-control PB is later installed, bind it to the named joint group and speed/gear state exposed by the vehicle controller. Do not assign numerical control to an Event Controller.

#### Terminal Configuration & Sliders
* Future PB inputs: driver-requested speed/reverse state and measured yaw/pitch/roll joint angles. The PB must command only named joints on the Tractor grid and must not engage the parked alignment clamps during travel.
* Manual fallback: disable the PB-controlled group and use the driver station controls. No automatic jackknife prevention or validated reverse-steering behavior is supplied.

**✓ Check:** Both carriage doors close independently without collision through the hitch's full test range. If the speed interlock and clamps are scripted, verify their configured threshold and fail-safe state; do not assume they eliminate trailer oscillation.

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
| Decoy Block | 3 | Slot 0 (`#2B303A`) | 3.6 t / 60 | Enclosed in lower armor skirts: flank pair [X ±4, Z −4, L2] and stern unit [X0, Z −28, L2] |
| Large Gyroscope | 2 | Slot 0 (`#2B303A`) | 24.0 t / 100 | Mounted above the centerline conveyor at [X ±1, Z +6, Y = L2] |
| Radio Antenna / Beacon | 1 | Slot 7 (`#1B263B`) | 0.8 t / 15 | `MAS Beacon Distress` encased in heavy armor at [X0, Z +7, L2] |
| Small Battery | 1 | Slot 6 (`#C0392B`) | 0.5 t / 10 | `MAS Battery BlackBox` (cold isolated standby) adjacent at [X +1, Z +7, L2] |
| Large Warhead | 1 | Slot 6 (`#C0392B`) | 1.6 t / 20 | `MAS Warhead Scuttle Power` in a dedicated power-vault mount at [X0, Z −1, L3], separated from the reactor cells |

```
Plan view, Y = L2 (Keel Failsafes & SOS Black Box Cell)
        X: −5              X0              X: +5
    Z:  +7 ┌───────────────┬───────────────┬───────────────┐
           │               │[BLACK BOX CELL│               │  ◄── Armored SOS Beacon + Isolated Battery
           │               │ Distress Beacon│              │
    Z:  +6 │          [GYRO 1] [GYRO 2]                    │  ◄── Recovery Pair Above Conveyor
    Z:  −4 │    [DECOY PORT] │               │ [DECOY STBD]│  ◄── Ballistic Decoy Skirts
    Z: −28 │                  [DECOY STERN]                 │  ◄── Aft Lower Skirt
           └───────────────┴───────────────┴───────────────┘
```

#### Placement Instructions
1. Install Ballistic Decoys `MAS Decoy Flank Port` at [X −4, Z −4, L2], `MAS Decoy Flank Stbd` at [X +4, Z −4, L2], and `MAS Decoy Stern` at [X0, Z −28, L2]. Keep them clear of the EVA lockers and connector stroke; decoy attraction is a test target, not guaranteed behavior.
2. Install 2 Large Gyroscopes `MAS Gyro Recovery 1` and `MAS Gyro Recovery 2` above the centerline conveyor run at [X −1 and +1, Z +6, L2]. Keep the pair clear of the conveyor and test any recovery override at low speed.
3. Construct a protected two-cell armor enclosure spanning [X0 to +1, Z +7, L2], with Radio Beacon `MAS Beacon Distress` at X0 and Small Battery `MAS Battery BlackBox` at X +1. Test power isolation and antenna range in game.
4. Install Large Grid Warhead `MAS Warhead Scuttle Power` in the dedicated [X0, Z −1, L3] vault cell, clear of the reactor blocks (disarmed by default).

#### Terminal Configuration & Sliders
* Gyroscopes: `MAS Gyro Recovery 1, 2`
  * Override: `OFF` (Normal transit) · Roll Override: `+100%` (Ready for righting toggle)
* Beacon & Battery: `MAS Beacon Distress` & `MAS Battery BlackBox`
  * Battery: `Recharge = OFF` · `Auto = OFF` (Isolated cold standby)
* Event Controller: `MAS EC BlackBox` (placeholder; use only an installed-version power condition/action if available. Automatic blackout detection and beacon activation require PB/timer wiring.)

**✓ Check:** Confirm the decoys respond as expected in a controlled test. Test beacon backup power and manual/PB activation independently. Verify gyro response on a small test grid before considering vehicle recovery.

---

#### Pre-Enclosure Inspection Gate 10.2A: Power Core Isolation, Jefferies Tube & Gangway Test
* [ ] Verify reactor automated uranium pull through the central conveyor trunk.
* [ ] Confirm `MAS Engine H2` cold-starts and delivers 5.0 MW upon manual toggle or simulated uranium depletion.
* [ ] Confirm total tractor power capacity reads `71.0 MW` peak (30 MW nuclear + 5 MW H2 engine + 36 MW battery discharge from 3× Large Batteries).
* [ ] Verify Port Jefferies tube allows unobstructed human walking from Bridge to Aft Bulkhead.
* [ ] Confirm physical maintenance port on `MAS Tractor Buffer` opens and allows manual inventory access.
* [ ] Confirm the inter-carriage gangway clears the hitch through its measured range. It is a weather/dust guard, not an airtight or pressurized passage.
* [ ] Test the 3-axis drawbar hitch: exercise yaw, pitch, and roll independently at low speed and confirm the assembly clears both carriage frames.
* [ ] With both carriages secured, verify the operator can disable `MAS Hitch Sever Tongue` and separate only the intended grids.
* [ ] Confirm `MAS Decoy Flank Port/Stbd` draw hostile turret lock away from the bridge and reactors.
* [ ] Test gyro override on a small test grid. Do not deliberately roll the completed consist; recovery torque and stability are unverified.
* [ ] Test `MAS Beacon Distress` and `MAS Battery BlackBox` independently. Verify backup power and manual/PB activation, then measure actual antenna range; do not assume automatic blackout detection.
* [ ] Verify any configured hitch-speed response at low speed first. Event Controllers cannot implement numeric torque control; that behavior requires a compatible PB/timer system and measured safe limits.
* [ ] Verify `MAS Locker Port/Stbd` are accessible from ground level (L0) and `MAS Lights Underbelly` illuminate suspension hubs.

```
PHASE 10-2 VISUAL MILESTONE · POWER CORE, BUFFER & DRAWBAR HITCH COMPLETE
       Z: −8 (Drawbar)        Z: −4 (Power Vault)    Z: +2 (Ore Buffer)
      [HITCH SEVER TONGUE]    [35 MW TIER 1 CORE]    [3×3×3 BUFFER HOLD]
             ║                         │                      │
       ══════╩═════════════════════════╧══════════════════════╧═══════
                    PORT JEFFERIES CRAWLWAY RUN (Z −7 to +6, L3)
```

## Phase 10-3 · Hydraulic Bedrock Outriggers & 35 m Reach-Target Excavation Boom (Levels L1-L8)

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
* Event Controller: `MAS EC Seismic` (placeholder only; mod-specific survey behavior and IGC reporting require the installed mod/controller and compatible PB. An Event Controller cannot generate an acoustic scan or broadcast IGC.)

**✓ Check:** Test outrigger motion and pad clearance individually. If a seismic mod is installed, commission it using its documented procedure and verify any PB integration separately.

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
* Start with low manual velocity; establish safe speed and braking from in-game tests.
* Rotor Limits: Lower Limit: `−180.0°` · Upper Limit: `+180.0°` (360° Continuous Pan)
* Share Inertia Tensor: `ON`
* **Reach target:** The arm is designed around up to 35 m radial tool reach. The actual safe working envelope must be measured after assembly; the centerline mount alone does not guarantee equal flank reach.

**✓ Check:** Turntable stator is secured and its head rotates through the tested clearance range. Confirm intended conveyor ports align through the actual rotor; no torque or full-rotation capacity is assumed.

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
* Start with low manual velocity; establish safe speed and braking from in-game tests.
* Lower Limit: `−20.0°` (Undercutting terrain / spine stow) · Upper Limit: `+75.0°` (High reach)
* Share Inertia Tensor: `ON`

**✓ Check:** Test the shoulder through a conservative unloaded range first. Measure the tool reach and load response on the completed boom; no holding torque, zero-droop, or stability rating is assumed.

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
* Start with low manual velocity; establish safe speed and braking from in-game tests.
* Lower Limit: `−100.0°` (Descent into deep pits) · Upper Limit: `+45.0°` (Extended reach)
* Share Inertia Tensor: test on the assembled subgrid

**✓ Check:** Test a conservative unloaded elbow range and measure the combined arm reach. Increase travel only after confirming subgrid, terrain, and chassis clearance.

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
             [ADV ROTOR: MAS Wrist Roll] ────► Tool roll axis; rotation range to be tested
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
  * Design limits: `−60.0°` to `+60.0°`; verify the installed hinge range and clearance.
  * Begin with low manual velocity; measure braking and holding behavior on the assembled arm.
* Rotor: `MAS Wrist Roll`
  * Test a limited rotation range first; enable continuous rotation only if the cable/conveyor path and tool mount support it.
  * Begin with low manual velocity; measure braking and holding behavior.

**✓ Check:** Verify the wrist joints can be operated manually through the tested range without collision. Automatic gravity-leveling requires a compatible controller and is not supplied by the blocks.

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
│ [VIEWPORT] Optional future control: `[MAS] PB Excavator` (Update10)                  │
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
   * Vehicle recovery and heavy lifting are not validated uses. Test the arm and support loads on an empty sacrificial test rig before attempting any recovery task.
5. **Macro 5: Wreck Deconstruction (`MACRO_SALVAGE`):**
   * Lasers switch to Mode: Salvage Beam (Grinder).
   * Planned controller behavior: hold a measured standoff and sweep a test target only if compatible PB logic and mod controls are installed. Component recovery depends on target blocks, mod behavior, conveyors, and inventory capacity; no retention percentage is guaranteed.

#### Automated Travel Stow & Deploy Sequences
* **Travel Stow Sequence (`STOW`):**
  1. Mining/salvage lasers deactivate; tool head retracts vertically clear of worksite.
  2. Turret Azimuth slews to 180.0° (Aft centerline).
  3. Shoulder, Elbow, and Wrist hinges align flat (0.0°).
  4. Boom lowers into armored dorsal spine trough; `MAS Clamp Boom Stow` at [X0, Z −7, L4] engages (Green Lock).
  5. Begin with conservative joint limits and unloaded motion; measure holding and braking behavior before applying a tool load.
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
* [ ] If a seismic mod/controller is installed, test it using the mod's documented procedure. PB logic is required to report results through `TMS.SCOUT.VEIN`.
* [ ] Confirm `Share Inertia Tensor = ON` across all boom joints (Turret Rotor, Shoulder Hinge, Elbow Hinge, Wrist Pitch Hinge, Wrist Roll Rotor).
* [ ] Test the full azimuth range at low speed and record measured reach and ground clearance on both flanks; stop at the first collision or stability limit.
* [ ] Verify `MAS Clamp Boom Stow` locks green at [X0, Z −7, L4] and rigidly secures the 35 m stowed boom with zero subgrid jitter.
* [ ] Test Macro 2 `BORE`: verify tool head maintains true vertical downward aim ([0, -1, 0]) while executing 2D lateral concentric sweeps around offset target waypoint.
* [ ] Verify `MAS Tool Grapple` locks onto external test block and holds suspended 50 t mass without joint sagging.
* [ ] Test Macro 4 `CRANE_RECOVERY`: verify crane mode enables grapple teleoperation and disables mining laser fire.

```
PHASE 10-3 VISUAL MILESTONE · OUTRIGGERS ANCHORED & BOOM DEPLOYED
                             ▲ 35 m reach target (measure after assembly)
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
       · Comms Mast: [X −2, Z +7, Y = L6] (Laser Antenna + Radio; test link range)
       · Survival Kit: [X −2, Z +8, Y = L4]
```

### Pre-Step Master Parts Tray: Phase 10-4
* 3× Control Station (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`, Palette 7: Fleet Command Navy)
* 2× Programmable Block (`[MAS] PB Excavator`, `[MAS] PB RIG//OS-Lite`, Palette 7)
* 3× AI Automaton Block (`MAS AI Move`, `MAS AI Task`, `MAS AI Defense`, Palette 7)
* 1× Laser Antenna (`MAS Comm Laser`, range to be measured, Palette 7)
* 1× Radio Antenna (`MAS Comm Radio`, desired 50 km range target; verify in game, Palette 7)
* 1× Large Ore Detector (150 m detection-range target; verify installed game/mod behavior, Palette 4: Processing Teal)
* 1× Survival Kit (Tractor Field Respawn & Med Station, Palette 7)
* 14× Sloped Heavy Armor Glass (45° Panoramic Canopy)
* 6× LCD Panel (Tactical Video Wall & Console HUDs, Palette 7)
* 1× Air Vent (`MAS Vent Bridge`, Bridge compartment)
* 1× Large Warhead (`MAS Warhead Scuttle Bridge`, disarmed anti-capture charge)
* 1× Timer Block (`MAS Timer Scuttle`, 60-second scuttle loop)
* 6× Sci-Fi 4-Button Panel (`MAS BP Ground Boarding`, `MAS BP Airlock Vestibule`, etc.)
* 1× Motor Hinge (`MAS Crew Ladder Hinge`, Palette 9: High-Viz Safety Yellow)
* 2× Narrow Ladder Block (Palette 9)
* 1× Sensor (`MAS Sensor Ladder`, 2.5 m proximity trigger)
* 1× Large Gatling Turret (`MAS Turret Bow`, Palette 7: Fleet Command Navy)
* 2× Interior Turret (`MAS Turret Flank Port/Stbd`, Palette 7)

---

### Step 10.21 · Forward Bridge Foundation Deck & Dual-Door Boarding Airlock (L3)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Bridge Foundation & Port Airlock  ·  Level: L3                   │
│ [VIEWPORT] Grid Zone: [X −3 to +3, Z +7 to +12]  ·  Plan view Y = L3 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.21 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 24 | Slot 0 (`#2B303A`) | 20.4 t / 24 | Foundation floor deck across X −3 to +3, Z +7 to +12 at Level L3 |
| Grated Catwalk Block | 12 | Slot 9 (`#F1C40F`) | 3.6 t / 12 | Walkway aisle floor along X0 and Port entry lane |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Boarding Outer` at [X −3, Z +7, L2/L3], exterior boarding seal |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Boarding Inner` at [X −2, Z +7, L3], cabin interior seal |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Airlock Port` at [X −3, Z +7, L4], depressurization / gas reclamation |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Bridge` mounted in ceiling at [X0, Z +9, L4] (Zone 1 cabin pressure) |
| Interior Light | 1 | Slot 9 (`#F1C40F`) | 0.02 t / 25 | `MAS Light Decon Port` at [X −3, Z +7, L4] (work light; no decontamination function) |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.1 t / 25 | `MAS EC Airlock Port` at [X −3, Z +8, L3], automated interlock sequencer |
| Survival Kit | 1 | Slot 7 (`#1B263B`) | 2.1 t / 25 | Mounted against Port aft bulkhead at [X −2, Z +8, L4] |

```
Plan view, Y = L3 (Bridge Sub-Deck & Dual-Door Boarding Airlock)
        X: −3             X: −2             X0              X: +2             X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────────────────────┐
           │ HEAVY ARMOR FOUNDATION DECK (Under Canopy Glazing)                          │
    Z: +10 │ [SUB-FLOOR C2 RACK] ── (Dual PBs + AI Blocks beneath Deck L4 Walkway)       │
    Z:  +8 ├─────────────────────┬─────────────────────────┬─────────────────────────────┤
           │ [MAS DOOR INNER]    │ [AIRLOCK VESTIBULE]     │ [LADDER UP TO ROOF CROWN]   │
    Z:  +7 │ [PORT AIRLOCK VESTIBULE]│                      │                             │
           │ (Work Light / Vent) │                         │                             │
           │ [MAS DOOR OUTER]    │                         │                             │
    Z:  +6 └───────▲─────────────┴─────────────────────────┴─────────────────────────────┘
                   │
         From Port Boarding Ladder (X −3)
```

#### Placement Instructions
1. Lay Heavy Armor Full Blocks across X −3 to +3, Z +7 to +12 at Level L3. Seal and pressure-test the finished room in game; block placement alone does not guarantee airtightness.
2. Install exterior Sliding Door `MAS Door Boarding Outer` at [X −3, Z +7, L2/L3], sealing the boarding threshold from the exterior ladder.
3. Install interior Sliding Door `MAS Door Boarding Inner` at [X −2, Z +7, L3], creating a two-door vestibule between the exterior and Bridge. Verify the seal and door clearance in game.
4. Mount Air Vent `MAS Vent Airlock Port` and work light `MAS Light Decon Port` in the airlock ceiling at [X −3, Z +7, L4]. Connect the vent to the oxygen conveyor system if gas recovery is desired, then test its behavior.
5. Install Event Controller `MAS EC Airlock Port` on the chamber bulkhead at [X −3, Z +8, L3].
6. Install Air Vent `MAS Vent Bridge` in the main bridge ceiling at [X0, Z +9, L4]. Pressure-test the Bridge independently before installing its interior.
7. Mount Survival Kit `MAS Survival Kit Tractor` at [X −2, Z +8, L4] with dedicated suit recharge port facing the central walkway.

#### Terminal Configuration & Sliders
* Outer Door: `MAS Door Boarding Outer` (Auto-Close: `ON` · Delay: `2.0 s`)
* Inner Door: `MAS Door Boarding Inner` (Auto-Close: `ON` · Delay: `2.0 s`)
* Airlock Vent: `MAS Vent Airlock Port` (Depressurize: `ON` on exit, `OFF` on entry)
* Cabin Vent: `MAS Vent Bridge` (set for the intended atmosphere and verify pressure in the completed room)
* Work Light: `MAS Light Decon Port` (standard illumination only)
* Event Controller: `MAS EC Airlock Port` (optional door interlock; configure only actions and conditions available in the installed game version; use a timer/PB for unsupported sequencing)
* Survival Kit: `MAS Survival Kit Tractor` (Spawn point enabled · Suit recharging active)

**✓ Check:** Pressure-test the Bridge and vestibule separately. Cycle the doors and confirm they cannot be opened together under the configured interlock. Measure oxygen recovery; do not assume complete recovery. Confirm the Survival Kit is powered and connected.

---

### Step 10.22 · 3-Operator Console Stations (Helm, Boom CTC, Fleet Director) & LCD Wall (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: 3-Operator C2 Command Bridge  ·  Level: L4                      │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z +10 to +11]  ·  Plan view Y = L4 (▲ Bow +Z)     │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.22 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Control Station (1×1×1 Open) | 3 | Slot 7 (`#1B263B`) | 1.8 t / 45 | Stations placed side-by-side at X −2, X0, X +2 (Z +11), facing Bow (+Z) |
| Armory Locker | 1 | Slot 7 (`#1B263B`) | 0.8 t / 10 | `MAS Armory Bridge` at [X +2, Z +10, L4], Starboard bridge wall |
| Equipment Locker | 1 | Slot 7 (`#1B263B`) | 0.8 t / 10 | `MAS Locker Survival Gear` at [X +2, Z +9, L4], Starboard bridge walkway |
| LCD Panel (Normal) | 4 | Slot 7 (`#1B263B`) | 0.4 t / 20 | Tactical Video Wall mounted on bridge aft bulkhead at Z +8 |
| Curved LCD Panel | 2 | Slot 7 (`#1B263B`) | 0.2 t / 10 | Side HUD repeaters flanking helm and tactical director |
| Sound Block | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Alarm Meteor` at [X0, Z +8, L4], bridge aft bulkhead early warning siren |

```
Plan view, Y = L4 (Custom Walk-In Bridge Deck, Armory & Tactical Video Wall)
        X: −3          X: −2           X0            X: +2          X: +3
    Z: +12 ┌─────────────────────────────────────────────────────────────┐
           │        45° SLOPED HEAVY ARMOR GLASS CANOPY (PANORAMIC)      │
    Z: +11 │   [STATION 1]             [STATION 2]           [STATION 3] │
           │   HELM / PILOT            BOOM EXCAVATOR        FLEET DIR   │
           │  (Control Station)       (Control Station)    (Control Stn) │
    Z: +10 │        │                      │               [ARMORY BRG]  │
           │   ◄────┴──────── OPEN WALKWAY AISLE (Palette 9) ────┴────►  │
    Z: +9  ├───────────────────────────┬─────────────┬─────[LOCKER GEAR]─┤
           │ [SURVIVAL KIT & CHARGER]  │ [LCD WALL 1]│ [LCD WALL 2]      │
    Z: +8  │ Field Respawn / O2-H2     │ Consist Sys │ Tactical Fleet    │  ◄── [ALARM METEOR] (Ceiling)
    Z: +7  ├───────────────────────────┴──────┬──────┴───────────────────┤
           │ [AIRLOCK SLIDING DOOR]           │ [LADDER UP TO ROOF CROWN]│
           └──────────────────────────────────┴──────────────────────────┘
```

#### Placement Instructions
1. Mount 3 open 1×1×1 Control Stations side-by-side along Z = +11, facing forward (+Z):
   * Station 1 (Port): `MAS Helm` at [X −2, Z +11, Y = L4] (Traction, steering, power, outriggers).
   * Station 2 (Center): `MAS Station Boom` at [X0, Z +11, Y = L4] (CTC mouse aim, macros, camera).
   * Station 3 (Starboard): `MAS Station Fleet` at [X +2, Z +11, Y = L4] (Fleet coordination, JIT dispatch).
2. Install Large Grid Armory Locker `MAS Armory Bridge` against the Starboard bulkhead at [X +2, Z +10, Y = L4], pre-stocked with Tier 4 Elite tools (welder, grinder, drill), automatic rifles, and ammunition boxes.
3. Install Large Grid Equipment Locker `MAS Locker Survival Gear` at [X +2, Z +9, Y = L4], stocked with spare suit O2/H2 bottles, data pads, and emergency survival kits.
4. Mount dedicated suit recharging terminal adjacent to `MAS Survival Kit Tractor` at [X −2, Z +8, Y = L4] for immediate pilot suit replenishment upon bridge entry.
5. Maintain a clear 1-block wide walk-around aisle at Z = +10 behind the seats.
6. Mount 4 LCD Panels on the aft bulkhead at Z +8 forming the Tactical Video Wall (`MAS LCD Video Wall 1..4`).
7. Paint consoles, lockers, and video wall frames with Palette Slot 7 (`#1B263B` Fleet Command Navy).
8. Install Sound Block `MAS Alarm Meteor` at [X0, Z +8, Y = L4] mounted to the ceiling structure directly above the central tactical video wall. Hardwire into Event Controller `MAS EC Meteor` for high-decibel early warning sirens upon CIWS meteor detection.

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
· Slot 7: PB `[MAS] PB Excavator` -> Run `LEVEL` (only after a leveling controller is installed and tested)
· Slot 8: Group `MAS Hitch Clamps` -> Unlock (parked restraint release; do not toggle during travel)
· Slot 9: Driver braking / handbrake control (manual fallback)

[Hotbar 2 · Consist Dynamics & Power Management]
· Slot 1: `MAS Hitch Yaw` -> Decrease Upper Limit (manual maneuvering, stationary test only)
· Slot 2: `MAS Hitch Yaw` -> Increase Upper Limit (restore configured design limit)
· Slot 3: Group `MAS Reactors` -> Power ON/OFF (Nuclear Scram)
· Slot 4: Generator `MAS Engine H2` -> Power ON/OFF (Emergency Cold-Start)
· Slot 5: Group `MAS Batteries Consist` -> Recharge ON/OFF (Shore Fast Charge)
· Slot 6: PB `[MAS] PB Excavator` -> Run `POWER MODE 1` (Silent Eco)
· Slot 7: PB `[MAS] PB Excavator` -> Run `POWER MODE 2` (Tractive Cruise)
· Slot 8: PB `[MAS] PB Excavator` -> Run `POWER MODE 3` (Heavy Excavate)
· Slot 9: PB `[MAS] PB Excavator` -> Run `POWER MODE 4` (Emergency Failover)

[Hotbar 3 · External Access & Worksite Illuminators]
· Slot 1: Hinge `MAS Crew Ladder Hinge` -> Reverse (Lower / Raise Boarding Ladder)
· Slot 2: Group `MAS Lights` -> Toggle ON/OFF
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
5. Construct Overhanging Cab Brow Visor (L5): Place Heavy Armor Inverted Slope 2×1×1 blocks along the top edge of the glass canopy (Z +11 to +12, X −2 to +2), cantilevered forward 0.5 m (+Z). This cuts direct planetary sun glare and shields the panoramic glazing from quarry spall and ceiling stone strikes.
6. Sculpt Lateral Cab Tumblehome (L3-L5): On Port (X = −3) and Starboard (X = +3) cab sidewalls, install Heavy Armor Slope 2×1×1 blocks angled inward from the L3 deck sill to the L5 roofline, transitioning at the forward corners with Inverted Corner 2×1×1 blocks. This narrows the cab crown from 7 blocks at L3 to 5 blocks at L5, giving the command cab an aggressive, chiseled locomotive profile and reducing aerodynamic drag.

**✓ Check:** Cabin completely enclosed and pressurized (100% O2); panoramic forward sightlines provide unobstructed view of the cutting zone and ground terrain; brow visor and tumblehome profiles form a chiseled aerodynamic cab.

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
| Large Ore Detector | 1 | Slot 4 (`#1ABC9C`) | 4.8 t / 15 | Centered in sub-deck keel at [X0, Z +8, Y = L3]; 150 m range is an unverified target |
| Laser Antenna | 1 | Slot 7 (`#1B263B`) | 2.5 t / 15 | `MAS Comm Laser` (40 km line-of-sight) on dorsal mast [X −2, Z +7, L6] |
| Radio Antenna | 1 | Slot 7 (`#1B263B`) | 0.8 t / 15 | `MAS Comm Radio` on dorsal mast; 50 km is an unverified range target |

```
Longitudinal Section · C2 Server Rack & Comms Mast Placement
         [MAS COMM LASER (40 km)]     [BOW CIWS GATLING]
         [MAS COMM RADIO (RANGE TEST)] [X0, Z +7, Y = L5]
                 │                           │
                 ▼ (Mast at X −2, L6)        ▼
    Level L5 ════■═══════════════════════════■═══════ (Crown Deck)
                 │                           │
    Level L4 ────┼── [3-STATION C2 BRIDGE] ──┼─────── (Bridge Deck)
                 │                           │
                 ▼                           ▼
    Level L3 ┌───────────────────────────┐ ┌────────┐
             │ [C2 SERVER RACK: DUAL PB] │ │ ORE    │ (150 m Range Target · Unverified)
             │ [AI AUTOMATON SUITE]      │ │ DETECT │
             └───────────────────────────┘ └────────┘
```

#### Placement Instructions
1. In the sub-bridge compartment at Level L3, mount Programmable Block `[MAS] PB Excavator` and Programmable Block `[MAS] PB RIG//OS-Lite` side-by-side at [X0, Z +7, Y = L3].
2. Flank with AI Automaton blocks: `MAS AI Move`, `MAS AI Task`, and `MAS AI Defense`.
3. Mount Large Ore Detector `MAS Ore Detector` in the centerline keel cavity at [X0, Z +8, Y = L3].
4. Erect the dorsal comms spire at [X −2, Z +7, Y = L6], mounting Laser Antenna `MAS Comm Laser` and Radio Antenna `MAS Comm Radio`.

#### Terminal Configuration & Sliders
* Programmable Block: `[MAS] PB Excavator` (Update rate target: `Update10` · Local machine control)
* Programmable Block: `[MAS] PB RIG//OS-Lite` (Update rate target: `Update100` · Fleet C2 & IGC router)
* Antenna: `MAS Comm Radio` (50 km is an unverified range target · Broadcast: `ON`)
* Laser Antenna: `MAS Comm Laser` (Range: `40,000 m`)
* Ore Detector: `MAS Ore Detector` (150 m is an unverified test target; record measured range)

**✓ Check:** Confirm PB mounting, power, terminal access, and manual control; measure ore-detector response and range; test radio/laser links and record actual operating range.

---

### Step 10.25 · Bridge Anti-Capture Scuttle System & Native Countdown Timer (L3-L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Asset Denial & Scuttle Loop  ·  Level: L3-L4                    │
│ [VIEWPORT] Grid Zone: Warhead [X +2, Z +7, L4]; Panel [X +1, Z +8, L4]              │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.25 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Grid Warhead | 1 | Slot 6 (`#C0392B`) | 1.6 t / 20 | `MAS Warhead Scuttle Bridge` on the dedicated bridge mount at [X +2, Z +7, L4], clear of both PBs |
| Timer Block | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Timer Scuttle` (60-second native countdown loop) |
| Sci-Fi 4-Button Panel | 1 | Slot 6 (`#C0392B`) | 0.1 t / 5 | `MAS BP Scuttle Bridge` mounted on bridge aft bulkhead at [X +1, Z +8, L4] |

#### Placement Instructions
1. Mount 1 Large Grid Warhead `MAS Warhead Scuttle Bridge` at [X +2, Z +7, Y = L4], clear of the C2 PBs and crew access. Keep it disarmed by default and validate the mount and separation in game.
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
| `MAS BP Flight Deck` | [X +3, Z −10, L6]| Roof hatch landing | Mule Clamps Toggle | Mule Conn Lock | Kestrel Welders Pulse | Deck Floodlights |
| `MAS BP Stern Dump` | [X +4, Z −25, L2]| Rear dump platform | Stern Conn Lock | Sorter Drain All | Land Train Ping | Rear Worksite Light |

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
| Sensor | 1 | Slot 7 (`#1B263B`) | 0.1 t / 10 | `MAS Sensor Ladder` at [X −5, Z +7, L2]; 2.5 m is a starting detection-radius target |

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
3. Install Sensor `MAS Sensor Ladder` at [X −5, Z +7, L2], covering the Port flank ground zone. Treat 2.5 m as a starting range and verify it in game.

#### Terminal Configuration & Sliders
* Hinge: `MAS Crew Ladder Hinge` (set conservative travel limits for stowed/deployed positions; tune torque and speed during commissioning)
* Sensor: `MAS Sensor Ladder` (optional PB input · 2.5 m starting detection radius; verify in the installed game version)
* Control: retain manual deploy/retract through a named local control. A future PB may use sensor, handbrake, and wheel-speed state only if those inputs are available and tested; do not assign numeric hinge positioning to an Event Controller.

**✓ Check:** Manually cycle the ladder from stowed to deployed with the vehicle secured. Confirm ground contact, hull clearance, and that the ladder cannot deploy into a moving wheel or obstacle. Sensor-triggered operation is not supplied.

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
* [ ] Measure `MAS Ore Detector` response and range in the installed game/mod version; 150 m is an unverified target.
* [ ] Confirm CTC block maps mouse yaw to `MAS Turret Azimuth` and mouse pitch to `MAS Shoulder Pitch`.
* [ ] Confirm `MAS Comm Laser` and `MAS Comm Radio` establish two-way IGC telemetry ping.
* [ ] Verify `MAS Crew Ladder Hinge` can be deployed and stowed manually without contacting the ground, wheels, or boarding route.
* [ ] Verify `MAS Turret Bow` and `MAS Turret Flank Port/Stbd` receive NATO 25×184mm ammunition through conveyor spine.
* [ ] Verify the installed multitool mod can weld a damaged test block using components from the intended conveyor path.

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
       Coordinate Range: Utility body Z −26 to −9; interface tongue at Z −8
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
* 2× Large Oxygen Tank (`MAS Tank O2 1, 2`, capacity to be verified, Palette 4: Processing Teal)
* 1× Small Cargo Container (`MAS Hold Ice LifeSupport`, 15,000 L Quarantined Ice, Palette 4)
* 2× Large Hydrogen Tank (`MAS Tank H2 1, 2`, capacity to be verified, Palette 6)
* 2× O2/H2 Generator (`MAS Gen O2/H2 1, 2`, Palette 4)
* 2× Air Vent (`MAS Vent Atmo Intake Port/Stbd`, Palette 0: Interior Vent)
* 1× Large Cargo Container (3×3×3 B, `MAS Hold Components`, Palette 5: Logistics Emerald)
* 1× Large Projector (`MAS Projector Master`, Palette 7: Fleet Command Navy)
* 1× Programmable Block (`[MAS-U] PB Utility`, Palette 7)
* 1× Remote Control (`MAS-U Remote FOB`, Palette 7)
* 2× Large Conveyor Sorter (`MAS Sorter Ammo Fwd`, `MAS Sorter Parts Aft`, Palette 4)
* 3× Sliding Door (`MAS Door Gangway Aft`, `MAS Door Stern Inner`, `MAS Door Stern Outer`, Palette 9)
* 14× Grated Catwalk Block, 6× Narrow Ladder Block (Palette 9: High-Viz Safety Yellow)

---

### Step 10.29 · Utility Carriage Box Girders, Keel Ballast Skids & Breakaway Deadman (L1-L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Utility Chassis Box Girders & Ballast  ·  Level: L1-L2          │
│ [VIEWPORT] Body: Z −26 to −9; interface tongue at Z −8 · Plan view Y = L1-L2         │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.29 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Heavy Armor Full Block | 72 | Slot 0 (`#2B303A`) | 61.2 t / 72 | Utility body girders at X ±1 from Z −9 to −26; interface tongue at Z −8; ties at body stations |
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
           │ [BALLAST 1]   │ 40× HEAVY ARMOR BALLAST       │  ◄── Low-Slung Keel Mass
    Z: −15 │ [BALLAST 2]   │ CO-LOCATED BETWEEN GIRDERS    │      Measure final center of mass
    Z: −18 │ [KEEL SKID 1] │ [BREAKAWAY BEACON] [KEEL 2]   │  ◄── Downward Ground Clamping Skids
    Z: −20 │ [BALLAST 3]   │                               │
    Z: −22 ├───────────────┼───────────────┼───────────────┤
           │               │ AFT SUSPENSION TIE BEAM       │  ◄── Axle 5 Cross-Member
    Z: −26 └───────────────┴───────────────┴───────────────┘  ◄── Stern Collision Push Bumper
```

#### Placement Instructions
1. Establish the utility-body box girders at X = −1 and X = +1 from Z = −9 to Z = −26 at Level L2. Build the separate tongue header at the Z = −8 interface without blocking hitch articulation.
2. Fabricate transverse cross-ties at Z = −13 (Axle 4 bolster), Z = −18 (mid-keel bolster), Z = −22 (Axle 5 bolster), and Z = −26 (stern push-bumper); build the Z = −8 tongue header as interface hardware.
3. Lay the 40× Heavy Armor Full Blocks into the Level L1 central trough between X = −1 and X = +1 from Z = −12 to Z = −22. Treat the listed ballast mass as an estimate and measure the completed carriage center of mass and stability before loading.
4. Mount 2× Large Grid Magnetic Plates (`MAS Utility Brake Keel 1, 2`) at [X −2, Z −18, Y = L1] and [X +2, Z −18, Y = L1] pointing directly downward toward ground voxels.
5. Install Radio Antenna `MAS-FOB-01 [EMERGENCY BREAKAWAY]` in the armored keel pocket at [X0, Z −18, Y = L1].
6. Place Event Controller `MAS-U EC Breakaway` at [X0, Z −10, Y = L2] inside the forward chassis sill.

#### Terminal Configuration & Sliders
* Magnetic Plates: `MAS Utility Brake Keel 1, 2` (configure Auto-Lock for the tested operating mode; no lock-strength setting is assumed)
* Antenna: `MAS-FOB-01 [EMERGENCY BREAKAWAY]` (50 km is an unverified broadcast-range target · Enable Broadcasting: `OFF` · Show in HUD: `ON`)
* Event Controller: `MAS-U EC Breakaway` (placeholder; configure only conditions and actions exposed by the installed block. A lost-merge/heartbeat detector and the coordinated brake/skid response require PB logic.)

✓ Check: Measure the loaded and unloaded center of mass, verify ground clearance, and test any brake/skid response on level ground with the carriage secured.

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
5. Build Axle 4 & 5 Flared Mudguards (L2): Mount Heavy Armor Half-Blocks directly above the suspension hubs at X = ±5, capped with Heavy Armor Half-Slopes sloping downward at 45° to match the Tractor mudguard profile. Blend the mudguard ends into the Carriage 2 sidewalls using Heavy Armor Inverted Corner blocks to shield DPU steering actuators from stone projectiles.

#### Terminal Configuration & Sliders
* Group: `MAS Utility Wheels`
  - Design maximum-speed target: `50 km/h` (unverified; commission at low speed before increasing)
  - Speed Limit: `ON`
  - Propulsion: `100%`
  - Brake Multiplier: `100%`
* Dynamic Traction & Braking Rules (`MAS EC DPU Drive` / `[MAS] PB Excavator`):
  - Hill-Climb Traction: When ascending grades > 5°, Axles 4 and 5 apply 100% matching tractive push synchronized with Tractor throttle, distributing load evenly across all 10 wheels.
  - Dynamic Tension Braking: When descending grades or decelerating, Carriage 2 wheels dynamically apply counter-braking torque 100 ms prior to Tractor braking bite, keeping the hitch in continuous longitudinal tension and preventing trailer jackknifing.
  - Automatic Traction Control (ATC): On loose voxel detection, wheel torque drops by 20% increments until slip ceases.

✓ Check: Axle 4 tracks dead-ahead and Axle 5 counter-steers as configured. Measure turning clearance at low speed before setting the operating limit.

---

### Step 10.31 · Sub-Floor Utility Battery Vault, Shore Power Bus & Hardened Ammo Bunker (L2)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Sub-Floor Battery Vault & Ammo Bunker  ·  Level: L2             │
│ [VIEWPORT] Grid Zone: [X −1 to +1, Z −17 to −15]  ·  Plan view Y = L2 (▲ Bow +Z)       │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.31 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Large Battery | 3 | Slot 6 (`#C0392B`) | 25.5 t / 240 | `MAS Battery 4, 5, 6` in sub-floor well at [X −1, 0, +1, Z −16, L2] |
| Small Cargo Container | 1 | Slot 7 (`#1B263B`) | 1.0 t / 25 | `MAS Hold Munitions` at [X0, Z −15, L2], sub-floor flash-shielded bunker |
| Heavy Armor Full Block | 12 | Slot 0 (`#2B303A`) | 10.2 t / 12 | Armored vault perimeter enclosure and structural top ceiling deck |

```
Plan view, Y = L2 (Utility Sub-Floor Battery Vault & Ammo Bunker)
        X: −1             0             X: +1
    Z: −15 ┌───────────────┬─────────────┬───────────────┐
           │ [SOLID ARMOR] │ [AMMO HOLD] │ [SOLID ARMOR] │  ◄── MAS Hold Munitions (Sub-Floor Bunker)
    Z: −16 ├───────────────┴─────────────┴───────────────┤
           │ [MAS BATTERY 4] [BATTERY 5] [BATTERY 6]     │  ◄── 9.0 MWh Energy Storage Buffer
    Z: −17 │ (3.0 MWh)       (3.0 MWh)   (3.0 MWh)       │      36.0 MW Peak Discharge Bus
           └─────────────────────────────────────────────┘
```

#### Placement Instructions
1. Excavate and frame a 3-block wide by 1-block long by 1-block deep recessed bay at [X −1 to +1, Z −16, Y = L2] directly beneath the Level L3 workshop floor.
2. Install 3× Large Batteries side-by-side:
   - `MAS Battery 4` at [X −1, Z −16, Y = L2]
   - `MAS Battery 5` at [X0, Z −16, Y = L2]
   - `MAS Battery 6` at [X +1, Z −16, Y = L2]
3. Enclose the battery well perimeter with Heavy Armor Full Blocks. The top faces form the foundation walkway for Level L3.
4. Tie battery terminals into the consist high-voltage tractive bus, supplying instantaneous float charge to the Basic Refinery, Large Assembler, and Axles 4-5 drive motors.
5. Install Small Cargo Container `MAS Hold Munitions` at [X0, Z −15, Y = L2] immediately forward of the central battery bank beneath the workshop corridor. Plumb top conveyor port to the central conveyor line, and enclose the lateral and lower faces within 40× heavy armor ballast plates. This establishes an isolated, flash-shielded munitions magazine preventing sympathetic detonation from penetrating the upper habitable workshop.

#### Terminal Configuration & Sliders
* Batteries: `MAS Battery 4, 5, 6`
  - Initial Mode: `Auto`
  - Max Stored Energy: `3.0 MWh each (9.0 MWh vault total)`
  - Peak Power Output: `12.0 MW each (36.0 MW vault total)`
  - Low Power Threshold: `15%` (Triggers audio alert on Bridge console)
* Cargo Container: `MAS Hold Munitions`
  - Subsystem: Sub-Floor Munitions Bunker
  - Target SML Quotas: 250× NATO 25×184mm boxes, 100× NATO 5.56×45mm magazines
  - Conveyor System: `ON`
* Consist Power Totals: Vault brings total Mastodon MKI energy storage to 18.0 MWh across 6 Large Batteries with 72.0 MW combined instantaneous discharge.

✓ Check: Batteries deliver 36.0 MW peak output; total consist energy reaches 18.0 MWh; battery well and ammo bunker remain fully armored beneath workshop floor plate.

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
| Armory Locker | 1 | Slot 7 (`#1B263B`) | 0.8 t / 10 | `MAS Armory Workshop` at [X −2, Z −13, L3]; Port workshop bulkhead facing aisle |
| Medical Room | 1 | Slot 7 (`#1B263B`) | 12.8 t / 30 | `MAS Medical Bay` at [X +2, Z −14, L3] plumbed to O2 life support trunk |
| Cryo Chamber | 2 | Slot 7 (`#1B263B`) | 3.2 t / 40 | `MAS Cryo Pod 1, 2` at [X +2, Z −12 to −13, L3] facing central aisle |
| Large Oxygen Tank | 2 | Slot 4 (`#16A085`) | 6.4 t / 50 | `MAS Tank O2 1, 2` at [X −2, Z −16 to −17, L3-L4]; verify installed tank capacity in the terminal |
| Small Cargo Container | 1 | Slot 4 (`#16A085`) | 0.8 t / 10 | `MAS Hold Ice LifeSupport` at [X0, Z −16, L3]; 15,000 L Quarantined Ice |
| Large Hydrogen Tank | 2 | Slot 6 (`#C0392B`) | 16.2 t / 50 | `MAS Tank H2 1, 2` at [X ±2, Z −18, L3]; verify installed tank capacity in the terminal |
| O2/H2 Generator | 2 | Slot 4 (`#16A085`) | 5.4 t / 44 | `MAS Gen O2/H2 1, 2` at [X0, Z −18, L3]; converts ice to fuel and air |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Gangway Aft` at [X +2, Z −9, L3], isolating the workshop from the external gangway; pressure-test any room boundary |

```
Plan view, Y = L3 (Mobile FOB Workshop Core, Medical Bay & Life Support Farm)
        X: −3           X: −2           0           X: +2           X: +3
    Z:  −9 ┌───────────────────────────[DOOR GANGWAY AFT]──────────────────────────┐
    Z: −10 │ FORWARD WORKSHOP ENTRY & FLIGHT LADDERWELL AISLE                      │
    Z: −12 │ [REFINERY L3]             │ [AISLE]    │ [MAS CRYO POD 1]             │
    Z: −13 │ [MAS ARMORY WRK]          │ [AISLE]    │ [MAS CRYO POD 2]             │
    Z: −14 │ [MAS UTILITY ASSEMBLER]   │ [AISLE]    │ [MAS MEDICAL BAY & CHARGER]  │
    Z: −15 │ (Dual Ports Face Aisle)   │ [AISLE]    │ (Trauma Respawn Station)     │
    Z: −16 │ [MAS TANK O2 1]           │ [ICE HOLD] │ [MAS TANK O2 2]              │
    Z: −17 │ (100,000 L O2)            │ (15,000 L) │ (100,000 L O2)               │
    Z: −18 │ [MAS TANK H2 1 (5,000kL)] │ [O2/H2 1,2]│ [MAS TANK H2 2 (5,000kL)]    │
           └───────────────────────────┴────────────┴──────────────────────────────┘
```

#### Placement Instructions
1. Install Sliding Door `MAS Door Gangway Aft` at [X +2, Z −9, Y = L3] to isolate the workshop from the external gangway. Pressure-test the completed room if airtightness is required.
2. Install Large Assembler `MAS Utility Assembler` at [X −2, Z −14, Y = L3] oriented so its dual physical conveyor/inventory doors face the central aisle at X0.
3. Install Large Grid Armory Locker `MAS Armory Workshop` at [X −2, Z −13, Y = L3] on the Port bulkhead, pre-stocked with Tier 4 Elite toolsets, automatic rifles, 5.56mm ammo, and spare suit bottles.
4. Install Medical Room `MAS Medical Bay` at [X +2, Z −14, Y = L3] with conveyor port connected to the life support trunk, equipped with a dedicated suit recharging terminal.
5. Mount 2× Cryo Chambers (`MAS Cryo Pod 1, 2`) at [X +2, Z −12, L3] and [X +2, Z −13, L3] facing inward toward the central walkway.
6. Install 2× Large Oxygen Tanks (`MAS Tank O2 1, 2`) at [X −2, Z −16, L3] and [X +2, Z −16, L3].
7. Place Small Cargo Container `MAS Hold Ice LifeSupport` at [X0, Z −16, Y = L3] directly between the O2 tanks, isolated by a one-way conveyor check to safeguard emergency ice (15,000 L reserve supplying 120+ hours offline).
8. Install 2× Large Hydrogen Tanks (`MAS Tank H2 1, 2`) at [X −2, Z −18, L3] and [X +2, Z −18, L3] with twin O2/H2 Generators (`MAS Gen O2/H2 1, 2`) mounted at [X0, Z −18, L3].

#### Terminal Configuration & Sliders
* Assembler: `MAS Utility Assembler` (Cooperative Mode: `ON` · Repeat: `OFF`)
* Armory Locker: `MAS Armory Workshop` (Pre-stocked: Tier 4 Tools, Rifles, Ammo, O2/H2 Bottles)
* Medical Bay: `MAS Medical Bay` (Respawn: `Enabled` · Healing: `Enabled` · Suit Recharging: `Active`)
* Cryo Chambers: `MAS Cryo Pod 1, 2` (Conveyor Connection: `ON`)
* Oxygen Tanks: `MAS Tank O2 1, 2` (Auto-Refill: configure as intended · Stockpile: `OFF` · verify terminal capacity)
* Hydrogen Tanks: `MAS Tank H2 1, 2` (Stockpile: `OFF` · verify terminal capacity)
* Ice Buffer: `MAS Hold Ice LifeSupport` (Custom Name: `[QUARANTINED] MAS Hold Ice LifeSupport`)

✓ Check: Pressure-test intended Workshop compartments in game. Verify assembler access, armory contents, medical/cryo power and conveyor links, and ice-hold isolation.

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
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Ammo` at [X −1, Z −15, L3] monitoring SML munitions quotas & restock |

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
5. Mount Event Controller `MAS EC Ammo` at [X −1, Z −15, Y = L3] on the port workshop bulkhead adjacent to `MAS Utility Assembler`. Configure to monitor ammunition inventory across `MAS Hold Munitions` and `MAS Hold Components`. When combined NATO 25×184mm ammunition drops below 60% of quota (150 boxes / 21,000 rounds) or NATO 5.56×45mm magazines drop below 60% of quota (60 magazines / 1,800 rounds), trigger automated production queue in `MAS Utility Assembler` at maximum priority.

#### Terminal Configuration & Sliders
* Basic Refinery: `MAS Field Refinery` (Use Conveyor System: `ON` · Smelting: Fe, Ni, Co, Si, Mg)
* Sorter: `MAS Sorter Ammo Fwd` (Mode: `Whitelist` · Items: `NATO_25x184mm`, `NATO_5_56x45mm` · Drain All: `ON`)
* Sorter: `MAS Sorter Parts Aft` (Mode: `Whitelist` · Items: `Scrap` · Drain All: `ON`)
* Event Controller: `MAS EC Ice`
  - Monitored Target: `MAS Tank H2 1, 2` (Hydrogen Fill Level)
  - Condition: Fill $< 90\%$
  - Action Slot 1: Enable Ice Sorter & Power ON `MAS Gen O2/H2 1, 2`
  - Action Slot 2: Standby mode when tanks $\ge 95\%$
* Event Controller: `MAS EC Ammo`
  - Monitored Target: `MAS Hold Munitions` + `MAS Hold Components` (Munitions Quotas)
  - Condition: Ammo Inventory $< 60\%$ (25mm $< 150$ boxes OR 5.56mm $< 60$ mags)
  - Action Slot 1: Trigger Assembler Ammo Fabrication Queue (Mg + Fe + Ni ingots)
  - Action Slot 2: Local quota monitoring and IGC requests require PB logic. If implemented, use Chapter 9 `TMS.RESUPPLY.REQ` with `REQ;unit_id;kit_type;urgency`.

✓ Check: Mined raw ore automatically smelts into ingots; scrap metal routes straight to refinery; assembler pulls ingots autonomously; forward sorter keeps bow and flank turrets fully loaded with ammo.

---

### Step 10.34 · Vertical Flight Deck & Stern Dump Dual-Door Vestibules (L2-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Vertical Egress & Airlock Vestibules  ·  Level: L2-L6           │
│ [VIEWPORT] Grid Zone: [X +2, Z −25 to −10]  ·  Profile view facing Port (▲ Zenith +Y)│
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.34 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Narrow Ladder Block | 6 | Slot 9 (`#F1C40F`) | 0.6 t / 6 | 4× ladder segments at [X +2, Z −10, L3-L5]; 2× segments at [X +2, Z −24, L2-L3] |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Flight Deck Inner` at [X +2, Z −10, L5], sealing workshop ascent |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Roof Outer` at [X +2, Z −10, L6], outer flight deck hatch seal |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Stern Inner` at [X +2, Z −25, L3], sealing workshop aft bulkhead |
| Sliding Door | 1 | Slot 9 (`#F1C40F`) | 1.2 t / 15 | `MAS Door Stern Outer` at [X +2, Z −25, L2], exterior stern egress door |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Airlock Roof` at [X +1, Z −10, L5], pressure control; test tank recovery |
| Air Vent | 1 | Slot 7 (`#1B263B`) | 0.5 t / 25 | `MAS Vent Airlock Stern` at [X +1, Z −25, L3], pressure control; test tank recovery |
| Interior Light | 2 | Slot 9 (`#F1C40F`) | 0.04 t / 50 | `MAS Light Decon Roof` [X +3, Z −10, L5], `MAS Light Decon Stern` [X +3, Z −25, L3] (standard work lights) |
| Event Controller | 2 | Slot 7 (`#1B263B`) | 0.2 t / 50 | Optional: `MAS EC Airlock Roof` [X0, Z −10, L5], `MAS EC Airlock Stern` [X0, Z −25, L3] |
| Sci-Fi 4-Button Panel | 2 | Slot 9 (`#F1C40F`) | 0.2 t / 16 | `MAS BP Flight Deck` at [X +3, Z −10, L6]; `MAS BP Stern Dump` at [X +4, Z −25, L2] |

```
Profile elevation looking Starboard, X = +2 (Dual Vertical Airlock Circulation)
       Z: −10 (Forward Flight Deck Ascent)             Z: −25 (Aft Stern Dump Descent)
  L6   [MAS DOOR ROOF OUTER] ── [MAS BP FLIGHT DECK]   ═══════════════════════════════
       │                                                                │
  L5   [MAS DOOR FLIGHT INNER] · [VENT ROOF] · [LIGHT] ═══════════════════════════════
       │ (Vertical Ladderwell Trunk)                                    │
  L4   │                                               ═══════════════════════════════
       │                                                                │
  L3   [WORKSHOP ENTRY THRESHOLD]                      [MAS DOOR STERN INNER] · [VENT] · [LIGHT]
                                                       │ (Vertical Ladderwell Trunk)
  L2   ═══════════════════════════════════════════════ [MAS DOOR STERN OUTER] ── [MAS BP]
```

#### Placement Instructions
1. Install an enclosed vertical ladder trunk at [X +2, Z −10] ascending from the forward workshop floor (Level L3) through Level L4 up to Level L5.
2. Install inner Sliding Door `MAS Door Flight Deck Inner` at Level L5 and outer Sliding Door `MAS Door Roof Outer` at Level L6. Pressure-test the finished vestibule; do not assume the ladder trunk and doors form an airtight room.
3. Mount Air Vent `MAS Vent Airlock Roof` at X +1, work light `MAS Light Decon Roof` at X +3, and optional Event Controller `MAS EC Airlock Roof` at X0 inside the Level L5 chamber. Mount button panel `MAS BP Flight Deck` at [X +3, Z −10, L6].
4. Install exterior Sliding Door `MAS Door Stern Outer` at [X +2, Z −25, L2] and connect it to the descending ladder trunk. Confirm the opened door clears the bumper and ground approach.
5. Install the ladder trunk between Level L3 and Level L2, with inner Sliding Door `MAS Door Stern Inner` at Level L3 and outer door at Level L2.
6. Install Air Vent `MAS Vent Airlock Stern` at X +1, work light `MAS Light Decon Stern` at X +3, and optional Event Controller `MAS EC Airlock Stern` at X0, all at Z −25, Level L3. Mount button panel `MAS BP Stern Dump` at [X +4, Z −25, L2] adjacent to the rear dump bumper.

#### Terminal Configuration & Sliders
* Doors: `MAS Door Flight Deck Inner`, `MAS Door Roof Outer`, `MAS Door Stern Inner`, `MAS Door Stern Outer` (Auto-Close: `ON` · Delay: `2.0 s`)
* Vents: `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern` (set for the intended pressure task; verify tank recovery and room pressure in game)
* Work Lights: `MAS Light Decon Roof`, `MAS Light Decon Stern` (standard illumination only)
* Event Controllers: `MAS EC Airlock Roof`, `MAS EC Airlock Stern` (optional interlocks; use only available block conditions/actions, otherwise add a compatible PB/timer sequence)
* Button Panels:
  - `MAS BP Flight Deck`: Btn 1 = Cycle Flight Deck Hatch · Btn 2 = Toggle Roof Floodlights · Btn 3 = Mule Cradle Clamp · Btn 4 = Emergency Suit Recharge
  - `MAS BP Stern Dump`: Btn 1 = Cycle Stern Hatch · Btn 2 = Toggle Stern Sorter Drain · Btn 3 = Lock/Unlock Stern Conn · Btn 4 = Toggle Stern Work Light

✓ Check: Cycle each vestibule with the consist parked and secured. Confirm door interlocks, measure pressure and oxygen recovery, and verify the ladder route is safe. Do not assume zero oxygen loss or a pressure-rated ladder trunk.

---

### Step 10.35 · Atmosphere-Dependent Vent Testing & Life-Support Mode (L4)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Life-Support Vent Test  ·  Level: L4                            │
│ [VIEWPORT] Grid Zone: [X ±3, Z −16]  ·  Flank profile looking Inward (▲ Zenith +Y)    │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.35 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Air Vent | 2 | Slot 0 (`#2B303A`) | 1.0 t / 50 | `MAS Vent Atmo Intake Port` at [X −3, Z −16, L4]; `MAS Vent Atmo Intake Stbd` at [X +3, Z −16, L4]; install facing into the intended interior spaces |
| Event Controller | 1 | Slot 7 (`#1B263B`) | 0.2 t / 25 | `MAS EC Atmosphere Mode` at [X0, Z −15, L3], optional mode indicator/control |
| Heavy Armor Slope | 4 | Slot 0 (`#2B303A`) | 3.4 t / 4 | Protective vent surrounds; verify they do not obstruct the vent openings |

```
Flank elevation cross-section, Y = L4, Z = −16 (Interior Vent Locations)
    Port Flank (X −3)                                               Starboard Flank (X +3)
    [INTERIOR SPACE]                                                [INTERIOR SPACE]
          │                                                               │
    ┌─────┴──────────────────┐                               ┌────────────┴─────────────┐
    │ [MAS VENT ATMO PORT]   │     Test pressurize /         │ [MAS VENT ATMO STBD]     │
    │ (Interior vent)        │     depressurize behavior     │ (Interior vent)          │
    └────────────────────────┘                               └──────────────────────────┘
```

#### Placement Instructions
1. Install Air Vents `MAS Vent Atmo Intake Port` and `MAS Vent Atmo Intake Stbd` facing the intended interior rooms. Connect them to the oxygen system if required by the selected operating mode.
2. Keep the vent faces unobstructed. Fit the listed slope surrounds only where they do not block airflow or interfere with room sealing.
3. Install Event Controller `MAS EC Atmosphere Mode` at [X0, Z −15, Y = L3] only if a supported terminal condition/action is useful; otherwise omit it.

#### Terminal Configuration & Sliders
* Air Vents: set `Depressurize` for the intended task. Record whether atmospheric oxygen can pressurize the tested room in the installed game version; do not claim that vents fill storage tanks from ambient air.
* Event Controller: `MAS EC Atmosphere Mode` (configure only built-in conditions/actions that exist in the installed version; it cannot identify arbitrary toxic gases or command PB/IGC logic directly)

✓ Check: Test vent pressurization and depressurization in a sealed room and in a breathable planetary atmosphere. Verify the actual tank and room response; do not assume automatic vacuum shutoff, toxic-atmosphere detection, or independent sealing of every compartment.

---

### Step 10.35B · Environmental Limitations, Optional Mast & Meteor Alert (L3-L6)

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ [LOCATOR] Subsystem: Environmental Operations & Meteor Alert · Level: L3-L6          │
│ [VIEWPORT] Grid Zone: [X −2 to +2, Z −18 to +6]  ·  Consist Profile & Plan View      │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.35B Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Decoy Block | 1 | Slot 0 (`#2B303A`) | 0.5 t / 170 | Optional visual/targeting decoy; not a lightning diverter |
| Interior Light | 1 | Slot 9 (`#F1C40F`) | 0.02 t / 25 | Optional gangway-area work light; not a UV sterilizer |

```
[OPTIONAL MAST, WORK LIGHT & METEOR-ALERT CIRCUIT]

              [OPTIONAL DECOY / VISUAL MAST] (Level L6 · X0, Z +6)
              (No lightning-protection function)
                                      │
            └───────────────► Optional work light / antenna mount

  CIWS TURRETS (`MAS Turret Bow`, `MAS Turret Stern`)
            └───────────────► Target Meteors = ON; test ammunition, arcs, and range
```

#### Placement Instructions
1. If desired for appearance, install `MAS Decoy Mast` on the dorsal crown at [X0, Z +6, Y = L6]. A Decoy does not divert lightning or provide electrical grounding.
2. Do not label the workshop aisle a radiation shelter. Armor and hydrogen tanks do not attenuate radiation in the listed game mechanics.
3. Omit the proposed heat-exchanger and UV-decontamination functions; the listed blocks have no such interfaces or effects.
4. Set `Target Meteors = ON` on the Gatling turrets if supported by the installed game version. Confirm ammunition feed, unobstructed firing arcs, and actual engagement range on a test world; turret settings do not guarantee complete coverage.
5. Configure Event Controller `MAS EC Meteor` only for conditions and terminal actions it actually exposes. If an emergency boom stow is desired, route the event through an installed PB or timer chain:
   - Sounds high-decibel early warning siren `MAS Alarm Meteor` ([X0, Z +8, L4]).
   - A configured timer/PB chain can attempt `STOW`, close appropriate doors, and secure the landing clamps. Measure completion time and verify that the sequence does not trap crew or collide with the hitch.
6. Keep ammunition routing and fleet resupply as separate commissioned systems. A sorter can filter inventory; a PB must implement inventory thresholds and any IGC request. Do not promise a fixed transfer rate or response time.

#### Terminal Configuration & Sliders
* Optional Decoy: `MAS Decoy Mast` (targeting behavior only; no lightning function)
* Gangway Light: standard work light only; no decontamination function
* CIWS Turrets: `MAS Turret Bow`, `MAS Turret Stern`
  - Target Meteors: `ON`
  - Target Missiles: `ON`
  - Target Small Ships / Characters: `ON`
* Siren: `MAS Alarm Meteor` (Sound: `Alert 2` · 150 m is an unverified range target · Volume: `100%`)
* Event Controller: `MAS EC Meteor` (meteor condition and supported terminal actions only)
  - Monitored Target: `MAS Turret Bow`, `MAS Turret Stern` (Target Lock / Meteor Acquisition)
  - Condition: Target Detected (Meteors / Missiles / Enemy Grids)
  - Action Slot 1: Trigger configured alarm and only the door/clamp actions available in the terminal.
  - Action Slot 2: Reset the alarm when the monitored condition clears.
  - PB arguments and IGC messages require a timer/PB integration; an Event Controller cannot issue them directly.
* `MAS EC Atmosphere Mode` can select a vent state only if its available condition and actions support it. It cannot detect/filter arbitrary toxic gases.

✓ Check: Optional decoy and work light behave as configured. Verify meteor-turret ammunition, firing arcs, and response on a test world; no lightning protection, radiation shelter, thermal jacket, UV decontamination, or automatic consist hardening is provided by the listed blocks.

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
3. When a blueprint has been captured from a verified build, load it into `MAS Projector Master`, align the projection in game, and check the offsets and subgrid settings. This chapter does not supply a reference blueprint.
4. Mount Programmable Block `[MAS-U] PB Utility` at [X −1, Z −13, Y = L3].
5. Mount Remote Control `MAS-U Remote FOB` at [X +1, Z −13, Y = L3] aligned facing forward (+Z).

#### Terminal Configuration & Sliders
* Projector: `MAS Projector Master`
  - Pitch: `0.0°` · Roll: `0.0°` · Yaw: `0.0°`
  - Offset X: `0` · Offset Y: `0` · Offset Z: `0`
  - Show Only Buildable: `ON`
* Autonomous FOB C2 (`[MAS-U] PB Utility` / `MAS-U Remote FOB`):
  - Standalone decouple behavior is a software objective, not a native PB/Remote Control feature. Implement and test startup, local control, and safety interlocks before relying on it.
  - Optional regional status message: requires a documented channel/payload and PB implementation; Chapter 9 currently defines `TMS.C2.HANDOVER`, not this heartbeat example.
  - Workshop production, life support policy, battery management, and rear CIWS behavior require compatible local scripts and should be commissioned independently of the Tractor.

#### Pre-Enclosure Inspection Gate 10.5A: Field Workshop Fabrication, Sub-Floor Batteries & Circulation Check
* [ ] Confirm Utility sub-floor battery bank (`MAS Battery 4, 5, 6`) delivers 9.0 MWh storage and 36.0 MW discharge.
* [ ] Verify `MAS Field Refinery` ingests raw mined ore and deposits refined ingots into `MAS Utility Assembler`.
* [ ] Verify `MAS Utility Assembler` pulls ingots and queues components without manual transfer.
* [ ] Confirm physical cargo access ports on Basic Refinery, Assembler, and Component Hold allow manual inventory access.
* [ ] Confirm the oxygen tanks, vents, and conveyor system operate as intended. Measure pressure in each actual sealed room; the gangway is not a pressurized zone.
* [ ] Test airlock gas recovery in a safe environment and record the pressure and tank changes; do not assume complete recovery before the outer door opens.
* [ ] After a verified build has been captured as a blueprint, test projector alignment and buildability. No reference blueprint is supplied with this chapter.
* [ ] Confirm `MAS Hold Ice LifeSupport` remains quarantined from bulk ore dumps.
* [ ] Verify vertical flight ladderwell provides unobstructed ascent to Level L6 Flight Deck.
* [ ] Confirm stern ladder provides safe descent to Level L2 dump bumper.
* [ ] Confirm crew medical bay and twin cryo pods allow successful respawn, healing, and hibernation.
* [ ] Confirm O2/H2 tanks maintain airtight seals and supply hydrogen lines.
* [ ] Test the configured generator/sorter response with a small ice batch; automatic routing requires compatible PB logic.
* [ ] Test both air vents in a sealed room and a breathable atmosphere; do not assume ambient oxygen is stored in tanks or that `Depressurize = ON` is an intake mode.
* [ ] If utility-carriage autonomy is implemented, verify `[MAS-U] PB Utility` startup and all required local systems after a secured, level-ground decouple.
* [ ] Test any breakaway alarm/brake/skid actions on level ground. Do not rely on an unverified deadman response or sever the hitch on a slope.
* [ ] Verify `MAS Sorter Ammo Fwd` draws ammunition to forward turrets without manual handling.
* [ ] Verify the sorter route with a small test inventory. Confirm the installed refinery accepts the routed material before relying on scrap processing.
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
       · Flush Solar Array: [X ±5, Z −22 to −20, Y = L6]
       · Stern Bumper: [X0, Z −26, Y = L2] · Telescoping Caterpillar Dump Connector: [X0, Z −27, Y = L2]
       · Stern CIWS Defense Turret: [X0, Z −26, Y = L5]
```

### Pre-Step Master Parts Tray: Phase 10-6
* 49× Heavy Armor Full Block (Palette 1: Hazard Amber · Mule Blast Deck)
* 4× Large Grid Solar Panel (`MAS Solar 1..4`, 1×2 B, Palette 0)
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
7. If desired, add chamfered guide coamings around the deck using Heavy Armor slopes and corner transitions. Keep the connector and landing path clear; the guides do not guarantee alignment, restraint, or protection from wheel run-off.

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
│ [VIEWPORT] Grid Zone: [X ±5, Z −22 to −20]  ·  Plan view Y = L6 (▲ Bow +Z)            │
└──────────────────────────────────────────────────────────────────────────────────────┘

#### Step 10.38 Parts Tray
| Block Type | Qty | Palette Slot | Mass / PCU | Critical Placement & Facing |
|---|---|---|---|---|
| Solar Panel (Large Grid) | 4 | Slot 0 (`#2B303A`) | 20.4 t / 32 | `MAS Solar 1..4` (1×2 B) flush with roof at X ±5 from Z −22 to −20 |
| Heavy Armor Slope | 8 | Slot 0 (`#2B303A`) | 6.8 t / 8 | Recessed border coaming providing 1-block thruster buffer |

```
Plan view, Y = L6 (Mule Blast Deck and outboard solar panels)
                  Port catwalk  Port solar     Mule receiving pad      Starboard solar  Starboard catwalk
    Z: −22            X −4        X −5       [CLAMP P] [MP CONN] [CLAMP S]       X +5             X +4
    Z: −20            X −4        X −5          Hazard / chevrons                X +5             X +4
                     └── panels remain serviceable from the catwalks without entering the landing footprint ──┘
```

#### Placement Instructions
1. Lay 2× Large Grid Solar Panels (`MAS Solar 1, 2`) along the Port roof wing at X = −5 spanning Z = −22 to −20, recessed flush with the Level L6 deck surface and accessible from the X = −4 catwalk.
2. Lay 2× Large Grid Solar Panels (`MAS Solar 3, 4`) along the Starboard roof wing at X = +5 spanning Z = −22 to −20, recessed flush with the Level L6 deck surface and accessible from the X = +4 catwalk.
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
* Data Bus: A compatible PB may forward docked-drone survey data (`TMS.SCOUT.VEIN`) to the Station 3 display; docking alone does not initiate an IGC transfer.

✓ Check: Test an unoccupied Kestrel approach at low speed; verify the landing geometry, connector state, and any PB data transfer independently.

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

✓ Check: Test `MAS Lights` in darkness and confirm useful illumination of the work area, flight decks, and underbelly. Record effective visibility; do not assume a fixed radius or complete perimeter coverage.

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
| Large Connector | 1 | Slot 2 (`#8B0000`) | 4.1 t / 25 | `MAS Stern Dump Conn` mounted on piston head at [X0, Z −27, L2] |
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
2. Install Large Connector `MAS Stern Dump Conn` directly onto the piston head at [X0, Z −27, Y = L2]. Configure centerline height at 2.5 m above ground plane matching the Caterpillar locomotive bow connector; validate stroke and connector alignment in game.
3. Construct Heavy Rail Push-Bumper & Guide Horns (L2): Across the stern transom (Z = −26, X −3 to +3), install Heavy Armor Blast Door Blocks and Heavy Armor Slope 2×1×1 Base blocks angled aft (−Z) painted Palette Slot 2 (Heavy Rail Crimson). Flank with 45° Heavy Armor Corner slopes at X = ±3 to form rigid guide horns that absorb shunting kinetic energy and funnel incoming CATERPILLAR-01 trains into soft-dock alignment.
4. Mount 2× Large Grid Pistons vertically at [X −3, Z −25, Y = L2] (`MAS Outrigger Utility Port`) and [X +3, Z −25, Y = L2] (`MAS Outrigger Utility Stbd`) extending downward through Level L1.
5. Mount Large Magnetic Plates (`MAS Seismic Pad Rear Port/Stbd`) on the piston heads facing ground.
6. Install Large Conveyor Sorter `MAS Sorter Tailings Stern` and 4× Conveyor Ejectors (`MAS Ejector Tailings Stern 1..4`) along the rear bumper at Level L1, angled 45° downward/aft.

#### Terminal Configuration & Sliders
* Piston: `MAS Piston Stern Dock` (Max Distance: `2.5 m` · Velocity: `0.2 m/s` · Share Inertia Tensor: `ON`)
* Connector: `MAS Stern Dump Conn` (Auto-Connect: `OFF` · Strength: `0.015`)
* Sorter: `MAS Stern Sorter` (Mode: `Whitelist` · Items: `Iron`, `Nickel`, `Silicon`, `Stone` · Drain All: `ON`)
* Outriggers: `MAS Outrigger Utility Port/Stbd` (Max Distance: `1.5 m` · Velocity: `0.5 m/s` · Share Inertia Tensor: `ON`)
* Tailings Sorter: `MAS Sorter Tailings Stern` (Mode: `Whitelist` · Items: `Stone`, `Gravel` · Drain All: `ON`)

✓ Check: Measure the stern piston travel and verify connector alignment at low speed. Test sorter transfer with a small batch, and verify support stability on the intended surface before increasing load.

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
  * Configure mode, range, and target settings only if exposed by the installed mod version.
  * Test on a sacrificial block; do not assume repair of docked craft, separate grids, or all blocks in the gantry's line of sight.
* Sorter: `MAS Sorter Resupply Export`
  * Default State: `OFF` · Drain All: `OFF`
  * Whitelist filter automatically populated by `[MAS-U] PB Utility` upon resupply kit staging.
* Event Controller: `MAS EC Resupply`
  * Use only supported local block actions, such as an indicator light if available.
  * PB logic must monitor inventory, configure sorter filters, and send IGC; an Event Controller cannot broadcast an IGC message or populate a sorter whitelist from a kit request.

#### Standardized Mule Peer Resupply Sortie Protocol
* Standardized Pallet Kits:
  * `KIT_MOBILITY`: 200 Steel Plates, 100 Motors, 50 Large Tubes, 50 Small Tubes.
  * `KIT_POWER`: 100 Steel Plates, 80 Construction Comps, 40 Computers, 50 Large Tubes.
  * `KIT_MUNITIONS`: 60 NATO 25×184mm ammo boxes, 40 Interior Plates, 20 Construction Comps.
  * `KIT_HULL_REPAIR`: 500 Steel Plates, 100 Interior Plates, 50 Construction Comps.
* Resupply Execution Cycle (requires compatible PB scripts):
  1. A requester may use the Chapter 9 `TMS.RESUPPLY.REQ` channel with `REQ;unit_id;kit_type;urgency`.
  2. PB logic must map the kit name to an inventory list, configure supported sorter actions, and verify the export hopper contents.
  3. Dock MULE-01 and test a small transfer. Connector inventory behavior and throughput must be measured in game.
  4. Any offer, dispatch, route, delivery, and completion messages need payload definitions before the full workflow can be commissioned.

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
* [ ] Measure `MAS Piston Stern Dock` travel and test connector alignment with both vehicles secured; do not allow vehicle impact.
* [ ] Test `MAS Outrigger Utility Port/Stbd` on the intended surface and measure stability before any Mule landing test.
* [ ] Verify `MAS Sorter Tailings Stern` discharges stone aggregate safely behind stern bumper.
* [ ] Test `MAS MT Turret Stern` coverage at the Mule pad and stern approach with a damaged test block. Record any blind spots and verify the installed mod supports the intended target grids.
* [ ] Test `MAS Sorter Resupply Export` with a small known inventory and verify the receiving grid and actual transfer rate.

```
PHASE 10-6 VISUAL MILESTONE · COMPLETE EXPEDITIONARY MINING TRAIN FOB
                        [KESTREL PADS]         [MULE PAD & MT ARCH L7]
                              │                        │
                    [TRACTOR BRIDGE L4]       [UTILITY WORKSHOP L3]
                              │                        │
       [35 m REACH TARGET]    │         [HITCH L2]     │         [STERN DUMP L2]
              \               │              ║         │                /
      ═════════╧══════════════╧══════════════╩═════════╧════════════════╩════════
           CARRIAGE 1: LOCOMOTIVE / EXCAVATOR   CARRIAGE 2: FOB / UTILITY MODULE
```

---

## Phase 10-7 · Master Control Logic, Dual-PB Architecture & RIG//OS-Lite Contract

This is a proposed two-PB software architecture, not a supplied script implementation. The physical build can be commissioned without fleet automation; each item below that requires PB logic must be implemented and tested separately. For messages already defined in Chapter 9, use its channel and payload exactly.

### 1. Dual-PB Compute Architecture (Tractor L3)
1. **`[MAS] PB Excavator` (Local Machine Control · Update10):**
   * May implement traction coordination and drawbar control; no controller code or verified traction-sharing behavior is supplied here.
   * May sequence four outrigger deployments; automatic leveling and voxel-lock detection require tested PB logic and suitable blocks.
   * Requires a compatible controller for 5-DOF boom inverse kinematics, Custom Turret Controller input, and automated sweep macros (Bench, Shaft, Trench). The block list alone does not provide those functions.
   * Run physics and actuator supervision on `Update10`; keep the loop allocation-aware and preserve recoverable state in `Storage`.
2. **`[MAS] PB RIG//OS-Lite` (Regional Fleet Orchestration & C2 Hub · Update100):**
   * May coordinate fleet roles when a shared, documented election implementation is installed. Chapter 9 defines `TMS.C2.HANDOVER`; it does not define a heartbeat channel.
   * Dispatches Mule requests using `TMS.MULE.CALL` and its Chapter 9 payload. Caterpillar dispatch needs a separately defined message.
   * Receives `TMS.SCOUT.VEIN` reports and may display GPS markers if implemented in the PB.
   * Subordinate commands and automatic root relinquishment require compatible protocols and scripts not supplied here.

```
Future C2 integration boundary (no election or heartbeat implementation supplied)

 [Local startup]
       │
       ├──► Initialize local controls; preserve driver/operator takeover
       │
       ├──► `TMS.C2.HANDOVER` ──► Validate and handle only as specified in Chapter 9
       │
       ├──► `TMS.MULE.CALL` ────► Issue/handle requests using the Chapter 9 payload
       │
       └──► `TMS.SCOUT.VEIN` ───► Consume reports using the Chapter 9 payload

 Any other role, election, heartbeat, registration, acknowledgement, or dispatch
 behavior requires a separately specified contract and is outside this design.
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
* `MAS DPU`: Axles 4 and 5 suspension wheels on Carriage 2; distributed traction logic is an unimplemented controller objective.
* `MAS FOB`: Carriage 2 command hardware (`[MAS-U] PB Utility`, `MAS-U Remote FOB`, `MAS-U EC Breakaway`); autonomous operation requires a commissioned script.
* `MAS Tailings Ejectors`: 4× Ejectors on stern bumper (`MAS Ejector Tailings Stern 1..4`).
* `MAS Multitool Turrets`: 4× Large Laser Multitool Turrets (`MAS MT Turret Bow`, `MAS MT Turret Port`, `MAS MT Turret Stbd`, `MAS MT Turret Stern`).
* `MAS Resupply`: `MAS Hold Resupply Export`, `MAS Sorter Resupply Export`, `MAS MP Conn`.
* `MAS Armory`: `MAS Armory Bridge`, `MAS Armory Workshop`, `MAS Locker Survival Gear`.
* `MAS Airlocks`: Vestibule doors (`MAS Door Boarding Outer/Inner`, `MAS Door Flight Deck Inner`, `MAS Door Roof Outer`, `MAS Door Stern Outer/Inner`). Gangway doors are isolation doors, not airlocks.
* `MAS Decon`: legacy group name for four standard work lights; they do not decontaminate.
* `MAS Vents Cabin`: Multi-zone cabin air circulation vents (`MAS Vent Bridge`, `MAS Vent Crawlway`, `MAS Vent Gangway`, `MAS Vent Workshop`).
* `MAS Vents Airlock`: Airlock pressure-control vents (`MAS Vent Airlock Port`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern`); gas recovery depends on tank connection and measured operation.
* `MAS Munitions`: Sub-floor munitions bunker (`MAS Hold Munitions`), forward ammo sorter (`MAS Sorter Ammo Fwd`), ammo Event Controller (`MAS EC Ammo`), and early warning siren (`MAS Alarm Meteor`).


### 3. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Programmable Block | `[MAS] PB Excavator` | Local kinematics and machine control · Update10 target · manual takeover required |
| Programmable Block | `[MAS] PB RIG//OS-Lite` | Fleet C2 interface · Update100 target · Chapter 9 channels only |
| Programmable Block | `[MAS-U] PB Utility` | Utility-grid supervisory interface · Update10/Update100 by workload · autonomous operation not supplied |
| Remote Control | `MAS-U Remote FOB` | Carriage 2 autonomous waypoint, defense, and antenna link |
| Control Station | `MAS Helm` | Station 1 (Port) · Pilot traction, steering, outrigger, power control |
| Control Station | `MAS Station Boom` | Station 2 (Center) · Excavation Master · CTC mouse aim & macros |
| Control Station | `MAS Station Fleet` | Station 3 (Stbd) · Tactical Fleet Director · Drone dispatch & C2 |
| Laser Antenna | `MAS Comm Laser` | 40 km point-to-point line-of-sight data link (L6 mast) |
| Radio Antenna | `MAS Comm Radio` | Fleet radio link; measure range and coverage in the intended world |
| Radio Antenna | `MAS Beacon Distress` | Optional distress beacon; test backup power, activation, and measured range |
| Battery (Small) | `MAS Battery BlackBox` | Isolated cold-standby power cell for distress beacon |
| Advanced Rotor | `MAS Hitch Yaw` | Articulated hitch yaw joint at [X0, Z −8, L2] · ±45° yaw |
| Motor Hinge | `MAS Hitch Pitch` | Articulated hitch pitch joint at [X0, Z −8, L3] · ±25° pitch |
| Advanced Rotor | `MAS Hitch Roll` | Articulated hitch roll joint at [X0, Z −9, L2] · ±20° roll along Z-axis |
| Magnetic Plate (Large) | `MAS Hitch Clamp Port/Stbd` | Parked alignment restraints; keep unlocked during travel |
| Merge Block | `MAS Hitch Sever Tongue` | Manual separation interface at [X0, Z −7, L3]; no pyrotechnic action or automatic separation is implied |
| Decoy | `MAS Decoy Flank Port/Stbd`, `MAS Decoy Stern` | Lower armor skirt ballistic decoys diverting turret fire |
| Gyroscope (Large) | `MAS Gyro Recovery 1, 2` | Optional chassis roll-override gyros at [X ±1, Z +6, L2]; recovery behavior requires a controlled test |
| Projector (Large) | `MAS Projector Master` | Complete consist blueprint self-repair projection core at [X0, Z −14, L4] |
| Warhead (Large) | `MAS Warhead Scuttle Bridge`, `MAS Warhead Scuttle Power` | Anti-capture technology denial scuttle charges (L3) |
| Timer Block | `MAS Timer Scuttle` | 60-second native scuttle countdown loop with manual abort |
| Oxygen Tank (Large) | `MAS Tank O2 1, 2` | Oxygen reserve; verify installed capacity and measured consumption |
| Small Cargo Container | `MAS Hold Ice LifeSupport` | 15,000 L quarantined emergency life support ice buffer |
| Cryo Chamber | `MAS Cryo Pod 1, 2` | Medical bay crew hibernation pods (zero food/suit drain) |
| Air Vent (Large) | `MAS Vent Bridge`, `MAS Vent Workshop`, `MAS Vent Crawlway` | Multi-zone pressurized cabin air vents (Zones 1, 2, 4) |
| Air Vent (Large) | `MAS Vent Airlock Port`, `MAS Vent Airlock Roof`, `MAS Vent Airlock Stern` | Pressure-control vents; test gas recovery with the installed tank/conveyor layout |
| Button Panel | `MAS BP Ground Boarding`, `MAS BP Airlock Vestibule`, `MAS BP Power Vault` | Physical in-situ crew control panels (Tractor L1-L3) |
| Button Panel | `MAS BP Scuttle Bridge`, `MAS BP Field Workshop`, `MAS BP Flight Deck`, `MAS BP Stern Dump` | Physical in-situ crew control panels (Bridge L4, FOB L2-L6) |
| Gatling Turret | `MAS Turret Bow` | Carriage 1 dorsal crown CIWS; verify firing arc and obstructions in game |
| Gatling Turret | `MAS Turret Stern` | Carriage 2 stern gantry CIWS; verify firing arc and obstructions in game |
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
| Air Vent (Large) | `MAS Vent Atmo Intake Port`, `MAS Vent Atmo Intake Stbd` | Interior pressure-control vents; test operation in planetary atmosphere and do not assume ambient tank filling |
| Motor Hinge | `MAS Crew Ladder Hinge` | Port flank boarding ladder articulator · 0° stowed to 90° down |
| Sensor | `MAS Sensor Ladder` | 2.5 m crew proximity sensor on Port flank (auto-deploy) |
| Advanced Rotor | `MAS Turret Azimuth` | Dorsal boom turntable · test rotation and clearances; no torque rating established |
| Motor Hinge | `MAS Shoulder Pitch` | Boom elevation joint · design travel −20° to +75°; test limits and load response |
| Motor Hinge | `MAS Elbow Pitch` | Boom mid-joint · design travel −100° to +45°; test limits and load response |
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
| Sliding Door | `MAS Door Boarding Outer`, `MAS Door Boarding Inner` | Port dual-door boarding airlock threshold and cabin isolation |
| Sliding Door | `MAS Door Flight Deck Inner`, `MAS Door Roof Outer` | Utility flight deck dual-door ascent airlock (L5-L6) |
| Sliding Door | `MAS Door Stern Inner` | Inner door of utility stern vestibule at L3 |
| Sliding Door | `MAS Door Stern Outer` | Exterior door of utility stern vestibule at L2 |
| Sliding Door | `MAS Door Vault` | Power Core Vault maintenance entrance from Jefferies tube |
| Sliding Door | `MAS Door Gangway Fwd` | Tractor Aft Bulkhead gangway isolation door |
| Sliding Door | `MAS Door Gangway Aft` | Utility Forward Bulkhead gangway isolation door |
| Armory Locker | `MAS Armory Bridge` | Large Grid armory locker at [X +2, Z +10, L4] · Tier 4 tools & munitions |
| Armory Locker | `MAS Armory Workshop` | Large Grid armory locker at [X −2, Z −13, L3] · Tier 4 tools & spare bottles |
| Equipment Locker | `MAS Locker Survival Gear` | Equipment locker at [X +2, Z +9, L4] · Suit O2/H2 bottles & emergency packs |
| Decoy | `MAS Decoy Mast` | Optional dorsal target decoy at [X0, Z +6, L6]; no lightning-protection function |
| Interior Light | `MAS Light Decon Port/Roof/Stern/Gangway` | Standard work lights; no UV or decontamination function |
| Laser Multitool Turret | `MAS MT Turret Bow` | Forward crown multitool turret at [X0, Z +9, L5]; test coverage and repair behavior |
| Laser Multitool Turret | `MAS MT Turret Port` | Forward port shoulder repair turret at [X −3, Z −10, L4] · Kestrel pad & hitch repair |
| Laser Multitool Turret | `MAS MT Turret Stbd` | Forward stbd shoulder repair turret at [X +3, Z −10, L4] · Kestrel pad & hitch repair |
| Laser Multitool Turret | `MAS MT Turret Stern` | Elevated gantry multitool turret at [X0, Z −23, L7]; test coverage and repair behavior |
| Small Cargo Container | `MAS Hold Resupply Export` | Outbound peer resupply staging hopper at [X0, Z −20, L5] directly under `MAS MP Conn` |
| Small Cargo Container | `MAS Hold Munitions` | Sub-floor flash-shielded munitions bunker at [X0, Z −15, L2] · 250× NATO 25mm / 100× 5.56mm quota |
| Sound Block | `MAS Alarm Meteor` | High-decibel early warning meteor & combat alert siren at [X0, Z +8, L4] |
| Conveyor Sorter | `MAS Sorter Resupply Export` | SML resupply kit staging sorter at [X0, Z −20, L4] |
| Event Controller | `MAS EC Airlock Port/Roof/Stern` | Optional local door interlock; test supported actions; sequence pressure operations with PB/timer logic |
| Event Controller | `MAS EC Resupply` | Optional local staging indication; inventory policy and IGC require PB logic |
| Event Controller | `MAS EC Meteor` | Optional local warning; no guaranteed intercept or consist-hardening behavior |
| Event Controller | `MAS EC Ammo` | Optional inventory indication; PB logic required for quotas, production queues, and requests |

| Event Controller | `MAS EC Stabilizers` | Optional support-state indication; PB logic required for boom permissives |
| Event Controller | `MAS EC Level` | Controller placeholder; automatic leveling requires tested PB logic |
| Event Controller | `MAS EC Ladder` | Optional sensor-to-hinge action; test coverage and manual override |
| Event Controller | `MAS EC Tailings` | Optional inventory-threshold action; test sorter/ejector behavior |
| Event Controller | `MAS EC Hitch Speed` | Controller placeholder; numeric hitch control requires PB logic |
| Event Controller | `MAS EC Hitch Rev` | Controller placeholder; steering and yaw control require PB logic |
| Event Controller | `MAS EC DPU Drive` | Controller placeholder; wheel-torque sharing requires PB logic |
| Event Controller | `MAS-U EC Breakaway` | Optional local warning/brake action; no slope-safe breakaway guarantee |
| Event Controller | `MAS EC Holds` | Optional inventory indication; no rated payload limit established |
| Event Controller | `MAS EC Handover` | Placeholder; protocol and compatible C2 PBs required |
| Event Controller | `MAS EC Power` | Optional generator switching; test fuel conditions, startup, and load response |
| Event Controller | `MAS EC Ice` | Optional generator/sorter switching; test actual gas and inventory routing |
| Event Controller | `MAS EC Combat` | Optional local warning; combat state and fleet messages require PB logic |
| Event Controller | `MAS EC Seismic` | Mod/controller placeholder; scans and reports are not native EC actions |
| Event Controller | `MAS EC Drone Repair` | Optional welder toggle; test with the installed mod and add a cutoff |
| Event Controller | `MAS EC LifeSupport` | Optional pressure/alarm action; no automatic breach isolation guarantee |
| Event Controller | `MAS EC BlackBox` | Placeholder; power-loss detection and backup signaling require tested logic |
| Event Controller | `MAS EC Atmosphere Mode` | Placeholder; no arbitrary atmosphere classification or scavenging control |

### 4. Event Controller Matrix

**Commissioning note:** This matrix is a proposal, not a configured control program. Event Controllers can only use conditions and terminal actions exposed by the installed game version. They do not provide arbitrary numeric control, PB arguments, IGC broadcasts, inverse kinematics, or multi-step airlock sequencing by themselves. Configure and record each controller in game; use PBs, timers, and operator checks for behavior beyond the available actions.

| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `MAS EC Airlock Port/Roof/Stern` | Local door state, if exposed | Configured interlock | Use only tested, supported door actions | No UV cycle or complete gas-recovery guarantee |
| `MAS EC Stabilizers` | Outrigger state, if exposed | Support state confirmed | Local warning or supported block action | PB logic required for boom permissives and leveling |
| `MAS EC Level` | None specified | Requires PB sensor/controller | No automatic leveling action specified | Manual operation until a controller is commissioned |
| `MAS EC Ladder` | `MAS Sensor Ladder` | Player detected, if exposed | Deploy ladder using supported hinge action | Test sensor coverage and manual override |
| `MAS EC Tailings` | Buffer inventory, if exposed | Configured fill threshold | Toggle sorter/ejectors if supported | Test a small stationary batch; clear crew and wheels |
| `MAS EC Hitch Speed/Rev` | Speed/gear state, if exposed | Configured condition | Warning or supported on/off action only | Numeric joint control and counter-steering require PB logic |
| `MAS EC DPU Drive` | None specified | Requires PB sensor/controller | No automatic wheel-torque action specified | Manual operation until a controller is commissioned |
| `MAS-U EC Breakaway` | Connection state, if exposed | Consist disconnected | Local alarm or supported brake action | No slope-safe severing guarantee |
| `MAS EC Holds` | Inventory fill, if exposed | Configured threshold | Local warning only | IGC request requires PB logic |
| `MAS EC MP/Train` | Connector state, if exposed | Connected/disconnected | Toggle a local sorter if supported | Test coupled inventories and clamp clearance |
| `MAS EC Resupply/Ammo` | Inventory state, if exposed | Configured threshold | Local status indication only | IGC, quotas, and production queues require PB logic |
| `MAS EC Handover` | No shared heartbeat condition specified | Requires documented protocol | No PB argument or IGC action available directly | Use Chapter 9 protocol through compatible PBs |
| `MAS EC Power/Ice` | Fuel or tank state, if exposed | Configured threshold | Toggle local generators if supported | Measure failover and inventory routing; no timing guarantee |
| `MAS EC Combat/Meteor` | Only installed-version conditions | Tested alert condition | Local alarm or supported turret action | Fleet coordination and automatic stow require PB/timer logic |
| `MAS EC Seismic` | Modded block state, if exposed | Installed mod condition | Local indication only | Scan and IGC reports require mod/controller support |
| `MAS EC Drone Repair` | Dock state, if exposed | Connector connected | Toggle welders if supported | Add tested cutoff; confirm welder/mod repairs intended grid |
| `MAS EC LifeSupport/Atmosphere Mode/BlackBox` | Only installed-version conditions | Measured sensor/state | Local alarm or supported block action | No arbitrary atmosphere classification, power-loss logic, or automatic isolation assumed |

### 5. IGC Channel Subscriptions (Chapter 9 Contract)

Only use the shared Chapter 9 channel names and payload schemas as implemented. The Mastodon-specific messages below are not additional supported channels.

* Mule request: `TMS.MULE.CALL` — `requester_id;pad_id;cargo_type;urgency`.
* Scout task: `TMS.SCOUT.TASK` — `TASK;zone_id;gps_center;radius_m`.
* Scout report: `TMS.SCOUT.VEIN` — `VEIN;ore_type;depth_m;gps_coords`.
* Base demand: `TMS.BASE.DEMAND` — `DEMAND;deficit_ore_type;deficit_kg`.
* C2 handover: `TMS.C2.HANDOVER` — `HANDOVER;target_node;timestamp`.
* Resupply request: `TMS.RESUPPLY.REQ` — `REQ;unit_id;kit_type;urgency`.

No Chapter 9 payload is currently defined here for heartbeat election, Caterpillar dispatch, airlock/meteor telemetry, traction synchronization, fleet status, resupply offers/dispatch/completion, or zone claims. Treat those as future protocol proposals: document the schema in Chapter 9 and update all participating PBs before testing them.

---

## Phase 10-8 · Commissioning & Field Acceptance Checklist (RIG Test)

### A · Articulated Consist Powertrain Trials
* [ ] **Tractive Articulation Test:** Begin at low speed on a flat test area. Verify the 3-axis drawbar clears both bodies through its yaw, pitch, and roll limits before increasing speed; record the safe operating limit.
* [ ] **Counter-Steer Tracking:** Verify Axle 5 counter-steers. Measure the minimum safe turn radius on a clear test surface and mark that value as the operating limit.
* [ ] **Slope Holding:** Begin on level ground, then test progressively steeper slopes with the consist unloaded and secured. Record the maximum safe grade; do not assume friction settings prevent rolling.

### B · Outrigger Anchoring & Automated Boom Macros
* [ ] **Outrigger Contact Test:** Deploy all four feet on suitable terrain. Verify the pistons stop within their limits, feet remain attached, and the chassis is stable; do not assume a magnetic plate locks to voxel terrain.
* [ ] **Travel Stow (if controller installed):** Test the configured stow sequence at low speed in an open area. Confirm joints and clamp clear the chassis and bridge; measure actual transit height before driving under an obstruction.
* [ ] **Measured Flank Reach:** Slew boom to each side and measure usable tool reach from the turntable datum. Confirm both sides clear wheels and hull; record the measured maximum rather than assuming the 35 m design target.
* [ ] **Vertical Bore (if controller installed):** Run the commissioned `BORE` controller at a test waypoint. Verify tool attitude and collision stops across the tested reach; otherwise operate the joints manually.
* [ ] **Bench Quarry Macro Test (if controller installed):** Test the configured sequence in an open area at low speed. Record its joint range and clearances; confirm the tool head does not collide with the chassis or terrain.
* [ ] **Collision Sensor Stop (if configured):** Test `MAS Sensor Boom` against a stationary target at low speed. Confirm the commissioned PB/timer logic stops motion; do not assume a sensor alone controls the joints.

### C · Mobile FOB & Dual Logistics Verification
* [ ] **Mule Deck Load Test:** Do not land MULE-01 until the parked consist, deployed supports, landing deck, and clamps have passed a measured load test. Start with an empty shuttle and increase load incrementally.
* [ ] **Caterpillar Transfer Test:** Couple the matching connector and transfer a small test inventory first. Confirm the sorter whitelist and record the actual transfer rate and safe load before attempting bulk transfer.
* [ ] **Field Workshop Operations:** Queue a small Steel Plate batch in `MAS Utility Assembler`. Verify conveyor access to required ingots and confirm the assembler completes the queue before testing larger production runs.

### D · C2 Fleet Orchestration & Handover Trials
* [ ] **Mothership Heartbeat Broadcast (if RIG//OS installed):** Confirm the PB script broadcasts the documented heartbeat over the configured antenna; radio and laser links are separate paths with their own range/line-of-sight limits.
* [ ] **Subordinate Worker Binding (after PB integration):** Test with a compatible subordinate script and documented protocol. Confirm the Mastodon receives and displays the expected status; no automatic binding or Tactical Video Wall integration is supplied by the blocks alone.
* [ ] **Logistics Task Dispatch (after PB integration):** Verify a Mule request uses the documented `TMS.MULE.CALL` payload. Caterpillar dispatch requires a separately documented and implemented interface.
* [ ] **Ground Base Handover (after protocol integration):** Test handover only after both PBs implement a shared, documented heartbeat and role-transfer payload; do not treat the state diagram as native block behavior.

### E · 5-Point Immersive Automation Verification
* [ ] **Auto-Leveling (if controller installed):** On a secured test slope, deploy the supports and verify the script's measured leveling tolerance. Do not extend beyond the piston stroke or use the supports as anchors unless the feet are designed for that surface.
* [ ] **Retractable Boarding Ladder (if actuated):** Test ladder deployment and stow manually at low risk. Automatic movement requires a compatible controller; verify clearance before enabling it.
* [ ] **Tool-Head Leveling (if controller installed):** Compare tool attitude with a level reference through the intended joint range. Do not assume automatic counter-rotation without commissioned logic.
* [ ] **Tailings Discharge:** Test the sorter and ejectors with a small stone batch while stationary. Verify the discharge direction and keep loose material clear of wheels and crew.
* [ ] **Drawbar Control (if controller installed):** Test at low speed in an open area. Do not assume a torque limit or yaw clamp; establish operating limits from measured clearance and handling.

### F · Tri-Fuel Power Redundancy & Closed-Loop Field Metallurgy Acceptance
* [ ] **Metallurgy Cycle:** Process a small iron-ore batch and verify refinery output, conveyor transfer, and assembler recipe. Test automatic production only after compatible PB logic is installed.
* [ ] **Power Failover:** In a safe test, disable the reactors and verify the configured hydrogen engine and battery response. Record startup delay and available loads; do not assume full traction/boom capability from the 5 MW engine rating.
* [ ] **Solar Watchkeeping:** With reactors and H2 engine off in daylight, monitor `MAS Solar 1..4`, life-support loads, and battery state of charge. Record actual generation and charging; do not assume solar can sustain all loads or charge a full battery bank.
* [ ] **Ice Processing:** Feed a small ice batch to the configured O2/H2 generators and verify gas output and tank fill in the terminal. Confirm inventory routing separately; do not assume an unverified tank capacity or automatic priority.

### G · Player Circulation, Maintenance Access & Hybrid Power Modes Acceptance
* [ ] **Ground-to-Bridge Boarding Flow:** Step onto Port L2 boarding ladder from ground level (L0). Verify ladder elevates crew to L2, airlock door cycles, and interior ladder leads up to Station 1 Helm.
* [ ] **Jefferies Tube Traversal:** Walk full length of Port maintenance crawlway from Z +6 to Z −7. Confirm physical cargo hatch on `MAS Tractor Buffer` opens and allows manual inventory transfer.
* [ ] **Power Vault Ingress:** Open `MAS Door Vault`. Step inside power compartment; confirm walk-up access to manual fuel hatches on `MAS Reactor 1, 2` and manual shutoff valve on `MAS Engine H2`.
* [ ] **Inter-Carriage Isolation:** With the consist parked and secured, confirm both carriage doors close and pressure-test independently. Do not walk across the external hitch gap or attempt passage while moving.
* [ ] **Flight Deck Access & Catwalk Inspection:** Climb forward ladderwell from workshop to Level L6. Walk full perimeter catwalk with safety railings around Mule pad, forward to Kestrel pads, and along solar inspection gangways.
* [ ] **Stern Dump Descent:** Descend aft ladderwell to Level L2 stern bumper. Confirm safe physical access to `MAS Stern Dump Conn` and `MAS Stern Sorter`.
* [ ] **Hybrid Power 5-Mode Sequencing:**
  - Test Mode 1 (Silent Watchkeeping): Reactors and H2 engine OFF; monitor solar generation, life-support loads, and battery state of charge.
  - Test Mode 2 (Tractive Cruise): Single reactor active; verify batteries smooth acceleration spikes.
  - Test Mode 3 (Heavy Excavation): Twin reactors active; measure generation and load with boom, outriggers, and workshop in operation.
  - Test Mode 4 (Emergency Failover, if wired): Safely simulate reactor loss and verify only the commissioned PB/timer logic starts `MAS Engine H2` or sheds loads; measure the response.
  - Test Mode 5 (Shore Power): Connect to an external grid and verify battery charging from the terminal; record the observed rate.

### H · Perimeter Defense, Combat Alert & Concrete Recycling Acceptance
* [ ] **Combat Alert (if PB/timer integration installed):** Trigger the monitored condition in a controlled test. Verify only the configured native actions occur and that any PB/IGC response is performed by the commissioned script or timer chain.
* [ ] **Bow & Stern CIWS Firing Arcs:** In a controlled test, verify each turret's actual tracking arcs and engagement behavior. Inspect for clipping against the bridge canopy, boom pedestal, and landing approach; record observed coverage rather than assuming a degree range.
* [ ] **Flank Turret Coverage:** In a controlled test, verify the installed turrets' target selection and clearances around the boarding ladder and wheel wells; do not treat them as guaranteed personnel protection.
* [ ] **Ammunition Conveyor Test:** Load a small amount of the correct ammunition and verify conveyor access to each turret. Test assembler production or automatic replenishment only after its recipe and PB/timer logic are configured.
* [ ] **Concrete Component Recycling (Decision 60, if mod installed):** Process a small stone batch and verify gravel routing and the installed mod's Concrete Component recipe before planning roadbed or foundation production.

### I · Field Logistics, Crane Grapple, Seismic Voxel Scan & Floodlight Acceptance
* [ ] **Seismic Survey (Decision 62):** Confirm each installed modded seismic block is recognized and reports data as documented by that mod. Compare the reported result with a known test site; do not assume a scan depth, accuracy, acoustic pulse, or IGC output unless the installed mod and PB script provide it.
* [ ] **Recovery Attachment Test (Decision 62):** Test the grapple on a small, secured load first. Increase load only after verifying joint torque, subgrid stability, and chassis clearance; do not use an unverified rating to right a vehicle.
* [ ] **Flight Deck Drone Welder Test (Decision 62, if configured):** Dock a small test grid and verify welder access, conveyor supply, and the intended grid's repair behavior. Test any PB/timer cutoff and confirm it cannot weld unintended blocks; do not assume a fixed pulse duration or full repair coverage.
* [ ] **Atmosphere Venting & Ground EVA Lockers (Decision 62):** In a breathable atmosphere, test the installed vents in both pressurize and depressurize modes and record the actual room/tank behavior. Do not assume exterior air fills storage tanks. Verify the ground-level lockers contain the intended tools and suit supplies.
* [ ] **Worksite Lighting (Decision 62):** Toggle group `MAS Lights` and inspect the work area at night. Record actual useful illumination and reposition lights to reduce shadows and glare.

### J · Custom Bridge, Universal Life Support & Emergency Failsafes Acceptance (Decision 64)
* [ ] **Custom Walk-In Bridge Circulation:** Walk from aft airlock door through open aisle to all 3 Control Stations (`MAS Helm`, `MAS Station Boom`, `MAS Station Fleet`). Verify sightlines through the installed armor-glass canopy and walk-around access to Survival Kit and Tactical Video Wall.
* [ ] **Multi-Page Hotbar Verification:** Test Hotbars 1, 2, and 3 on all three operator stations. Confirm Station 1 controls traction/leveling (HB1), power modes/reactors (HB2), and boarding/lights (HB3). Confirm Station 2 controls boom joints/lasers (HB1), automated macros (HB2), and buffers/ejectors (HB3). Confirm Station 3 controls logistics calls (HB1), drone slave links (HB2), and CIWS defense (HB3).
* [ ] **Life-Support Pressure & Isolation:** Pressure-test each completed compartment separately. Verify doors, vents, and tanks behave as configured; do not claim automatic breach sealing unless a compatible controller is installed and tested.
* [ ] **Airlock Cycling:** Test each airlock from a safe environment. Confirm the outer door cannot open while the inner door is open and measure oxygen recovery; do not assume 100% gas recovery or toxic-gas protection.
* [ ] **Cryo Hibernation Verification:** Enter `MAS Cryo Pod 1` and `MAS Cryo Pod 2` in Medical Bay. Confirm direct conveyor link supplies O2 and suspends suit life support drain.
* [ ] **Quarantined Emergency Ice Buffer:** Mine 20 t raw ice. Confirm ice diverts into `MAS Hold Ice LifeSupport` (15,000 L) and is preserved even when main ore buffers are dumped into the Caterpillar land train.
* [ ] **Hitch Separation Test:** With both carriages parked, unloaded, and secured, test the actual separation mechanism. Confirm the utility carriage retains the power, controls, braking, and life support needed for its intended standalone role before relying on autonomous FOB operation.
* [ ] **Recovery Gyros:** Verify gyro control on a small test grid first. Do not deliberately roll the completed consist; recovery torque and stability are unverified.
* [ ] **Emergency Beacon (if configured):** Test the beacon and its backup power independently in a safe environment. Record actual range and activation behavior; do not assume 50 km coverage or automatic PB wake-up.
* [ ] **Projector Alignment (after a blueprint exists):** Load a blueprint captured from the final build, align it in-game, and verify the projected blocks before enabling welders. No 1,039-block reference blueprint is supplied with this chapter.
* [ ] **Scuttle Alarm & Abort (if configured):** Test the countdown, warning indicators, and abort path in a safe Creative test world with warheads absent or permanently disarmed. Verify the abort cannot be bypassed; never test detonation on the operational build.

### K · Tri-Mode Laser Multitool Salvage & Remote Welding Acceptance (Decision 65)
* [ ] **Tri-Mode Operational Cycle:** Cycle `MAS Laser 1, 2` via Station 2 Hotbar 1 Slot 8. Confirm HUD feedback confirms mode switching across `Mining Beam (Drill)` -> `Salvage Beam (Grinder)` -> `Fabrication Beam (Welder)` -> `Mining Beam`.
* [ ] **Planar Salvage Sweep (if controller installed):** Test on a sacrificial target at low speed. Verify the controller respects joint limits and collision stops; otherwise use manual tool control.
* [ ] **Component Recovery Test:** Disassemble one sacrificial block with the installed mod and observe the recovered components, conveyor behavior, and floating debris. Record actual recovery; do not assume full retention.
* [ ] **Assembler Disassembly Test:** If disassembly is supported by the installed assembler and blueprint, test a small quantity and verify the resulting inventory before relying on a recycling loop.
* [ ] **Scrap Processing Test (if supported):** Confirm that the installed refinery accepts the stated material and produces the stated output before adding scrap processing to the survival workflow.
* [ ] **Remote Fabrication (if supported by installed mod/controller):** Test on a sacrificial projection at increasing standoff distances and record actual completion behavior. Do not rely on an assumed 35 m reach or remote welding in survival until confirmed.

### L · Multi-Tier Fleet Comms, Predictive JIT Logistics & Swarm Synergy Acceptance (Decision 66)
* [ ] **Mule Logistics Request (after PB integration):** Trigger the commissioned request flow and verify `TMS.MULE.CALL` matches the Chapter 9 payload (`requester_id;pad_id;cargo_type;urgency`). Buffer prediction, automatic dispatch, and Caterpillar tasking are not defined by that message alone.
* [ ] **Logistics Triage:** Test each sorter whitelist with a small mixed batch. Confirm actual destination inventories before enabling any automated transfer or ejector sequence.
* [ ] **Scout IGC Interface:** If the Kestrel controller is installed, verify its task message uses the Chapter 9 `TMS.SCOUT.TASK` schema and its report uses `TMS.SCOUT.VEIN`. Seismic scanning and its output require separate mod/controller tests.
* [ ] **Relay Link (if configured):** Test radio and laser links separately at measured ranges and line-of-sight conditions. A Kestrel relay mode requires compatible onboard PB software; no seamless packet delivery is assumed.
* [ ] **Base Demand IGC Interface:** Test `TMS.BASE.DEMAND` using the Chapter 9 schema `DEMAND;deficit_ore_type;deficit_kg`. Survey lookup, target selection, and display behavior require separate Mastodon PB implementation.
* [ ] **Optional Fleet Coordination:** Do not commission zone claims or fleet-defense messages until their payload schemas and compatible PB behavior are documented. Native turrets do not share target state through IGC.

### M · Consist Mechanics, 3-Axis Hitch, DPU Powertrain & FOB Autonomy Acceptance (Decision 67)
* [ ] **3-Axis Gimbal Hitch Compliance:** Test yaw, pitch, and roll one axis at a time at low speed, then on controlled uneven terrain. Stop if a subgrid binds, contacts armor, or lifts a wheel; set practical limits below any collision point.
* [ ] **Hitch and Clamp Clearance:** With both carriages secured, inspect the hitch through its intended yaw, pitch, and roll range. Do not engage clamps against moving or unintended surfaces; test only at safe speed and record the result.
* [ ] **Carriage Traction and Braking:** Test each carriage independently at low speed, then with the unloaded coupled consist. Torque matching, regenerative descent braking, and anti-jackknife response require tested controller logic.
* [ ] **FOB Detachment (if commissioned):** On level ground, secure both carriages and test the actual decoupling method. Verify the utility carriage retains the power, braking, control, and life support required for its intended standalone role.
* [ ] **Breakaway and Stability Test:** Do not deliberately sever a connection on a slope. Test configured brake/skid actions on level ground, then measure stability and center of mass with a secured, incrementally loaded consist.
* [ ] **Stern Support Load Test:** With the consist parked, deploy the stern supports and incrementally load the deck using a controlled test mass. Do not attempt a 650 t touchdown unless a structural test has established that rating.
* [ ] **Seismic Equipment Test:** Verify each installed seismic mod block using the mod's documented procedure. Record actual behavior; do not assume a 42 m baseline, scan depth, or accuracy.
* [ ] **Caterpillar Interface Test:** Align and couple the actual connector at low speed with both vehicles secured. Test inventory transfer with a small batch before attempting bulk loads.
* [ ] **Stern Tailings Discharge:** With the consist stationary and clear of people, test the sorter/ejectors using a small stone batch. Verify the discharge direction and keep material clear of wheels; ejectors do not compact a roadbed.

### N · 4× Multitool Repair Turrets, SML Auto-Production & Peer Resupply Sortie Acceptance (Decision 68)
* [ ] **Repair-Turret Coverage:** Test each turret against a damaged sacrificial block at several points. Record line-of-sight and functional limits; do not assume spherical coverage or repair across separate grids.
* [ ] **Docked-Craft Maintenance:** If the installed multitool mod supports it, test repair on a secured, unoccupied craft with required parts available. Confirm which subgrids and blocks are targeted before relying on unattended servicing.
* [ ] **SML Quota Monitoring (if PB installed):** Change a test inventory below a configured quota. Verify the utility PB queues only the deficit and does not create an unbounded or duplicate queue.
* [ ] **Closed-Loop Metallurgy to Component Path:** Feed a small iron-ore batch through the configured refinery and assembler. Verify conveyor paths and recipe availability, then document any PB or sorter configuration needed for automatic production.
* [ ] **Resupply Kit Assembly & Staging:** Issue command `RESUPPLY KIT_MOBILITY`. Confirm `MAS Sorter Resupply Export` powers ON with Drain All = ON, pulling exactly 200 Steel Plates, 100 Motors, 50 Large Tubes, and 50 Small Tubes into `MAS Hold Resupply Export` ([X0, Z −20, L5]), then shuts OFF.
* [ ] **Mule Resupply Transfer:** Dock MULE-01 and transfer a small test kit first. Verify connector compatibility, sorter whitelist, and inventory destination; measure throughput before scaling the load.
* [ ] **Peer Resupply IGC (after integration):** Use the Chapter 9 `TMS.RESUPPLY.REQ` payload (`REQ;unit_id;kit_type;urgency`). Offers, dispatch, and automatic staging require separately documented PB behavior.

### O · Extended Survival Ergonomics, Airlock Tests & Environmental Limits (Decision 69)
* [ ] **Bridge & Workshop Armory Loadout:** Verify `MAS Armory Bridge` ([X +2, Z +10, L4]) and `MAS Armory Workshop` ([X −2, Z −13, L3]) are fully stocked with Tier 4 Elite tools (welder, grinder, drill), automatic rifles, 5.56mm ammo, and spare suit bottles. Verify `MAS Locker Survival Gear` ([X +2, Z +9, L4]) is stocked with emergency O2/H2 suit bottles and data pads.
* [ ] **Suit Recharge:** Test suit recharge at the Survival Kit and Medical Room in game, recording actual recharge time and available resources.
* [ ] **Emergency Ice Reserve:** Verify ice quantity, sorter isolation, and generator consumption on the completed grid. Calculate endurance from measured consumption rather than assuming a 120-hour duration.
* [ ] **Independent Compartment Pressure Tests:** Test the Bridge and Workshop separately. Do not count the articulated gangway as a pressurized zone; document any areas that cannot be isolated by installed doors and bulkheads.
* [ ] **Atmosphere Vent Test:** Test the vents in vacuum and a breathable atmosphere. Record observed room and tank behavior; do not assume automatic toxic-gas detection, vacuum shutoff, or ambient oxygen storage.
* [ ] **Airlock Cycling:** Test each vestibule in a safe environment. Confirm configured door interlocks and measure pressure/oxygen recovery; the work lights do not decontaminate.
* [ ] **Flight Deck and Stern Access:** Test the roof and stern doors/ladder routes with the consist parked. Configure event-controller interlocks only if supported by the installed block actions.
* [ ] **Environmental Limits:** Do not treat the decoy mast as lightning protection, the workshop as a radiation shelter, or refinery waste heat as ice-tank heating. These functions are not implemented by the listed blocks.
* [ ] **Meteor Defense (if supported):** Test turret meteor targeting, ammunition, firing arcs, and engagement range in a controlled world; do not assume every meteor will be intercepted.

### P · Automated Meteor Defense Grid, Closed-Loop Ammo Fabrication & Dynamic Restock Acceptance (Decision 70)
* [ ] **Meteor Turret Test:** If the installed game version supports meteor targeting, test each turret's arcs, ammunition use, and actual engagement range in a controlled environment. Do not assume 360° overlap or a minimum interception distance.
* [ ] **Meteor Alert Integration (if scripted):** Verify the alarm's native trigger. Test any PB/timer-driven stow and lockdown sequence independently, measure its duration, and confirm it cannot close an occupied airlock or collide with a docked craft.
* [ ] **Autonomous Closed-Loop Munitions Manufacturing:** Withdraw 120 NATO 25×184mm ammo boxes from `MAS Hold Munitions` (dropping stock below 60% of the 250-box SML quota). Confirm `MAS EC Ammo` triggers `MAS Utility Assembler` to immediately queue 120 NATO 25mm boxes and 50 NATO 5.56mm magazines from refined Magnesium, Iron, and Nickel ingots with top combat priority over structural plate production.
* [ ] **Munitions Storage Layout:** Verify `MAS Hold Munitions` ([X0, Z −15, L2]) is separated from the workshop inventory and its conveyor path is correct. Armor placement is not a tested explosion or sympathetic-detonation rating.
* [ ] **Ammo Conveyor Test:** Verify the installed sorter whitelist and conveyor path with a small ammunition batch. Confirm the intended turret inventories receive ammo; do not assume transfer across an uncoupled connector or articulated hitch.
* [ ] **Mule Munitions Resupply (after PB integration):** Test a request using Chapter 9 `TMS.RESUPPLY.REQ` and `REQ;unit_id;kit_type;urgency`. Kit selection, automatic staging, dispatch, and delivery require separately implemented PB behavior and a compatible Mule transfer interface.
