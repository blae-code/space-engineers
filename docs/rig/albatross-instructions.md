# Albatross · Visual Build Instructions

## Chapter 13 · ALBATROSS-01 Utility & Field-Repair Craft (ALB-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the ALBATROSS-01 (ALB-01) Autonomous Field Utility, Survey & Repair Craft.
Each step features: an exact ASCII blueprint, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical conveyor/connector orientations, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

Source specs:
* Authority: Blae Decisions 25 (Earth 1 g), 31 (Orca support craft complement: Bay S2), 41 (3× Inventory), 50 (Mastodon utility resupply), 52 (Dual-tier logistics telemetry)
* Role: Autonomous field-repair, precision structural welding, drone recovery, component ferry, and external damage survey for the ORCA-01 mining carrier, REMORA-01 drillers, and MASTODON-01 excavator.
* Grid: Small Grid (1 sb = 0.5 m) · Bounding Envelope: 9 W × 11 L × 5 H sb (4.5 × 5.5 × 2.5 m)
* Dry Mass: ≈ 8.5 t · Laden Mass (max cargo): ≈ 18.5 t · Hover Thrust: 640 kN (T/W ≈ 7.5 empty, 3.5 laden at Earth 1 g)

Last updated: 2026-10-17

---

## 0 · Reference Grid, In-Game Paint Palette & Craft Envelope

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the belly docking connector at Level L1.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. (Range: X −4 to +4 sb).
* **Y (Elevation):** Belly landing skids and dock port are Level L1, advancing upward to Avionics Roof at Level L5.
* **Z (Length / Longitudinal):** Stern thruster block is negative (−), Bow welder head is positive (+). (Range: Z −5 to +5 sb).

```
Spatial Alignment Triad:
       [+Y: Zenith / Utility Roof L5]
              │
              │   [+Z: Bow / Welder Head & Work Spotlights]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Main Drive Thrusters & Power]
              │
       [−Y: Belly Dock / Ground Skids L1]
```

### 0.2 In-Game Paint Palette & Functional Color Coding

All structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 4** | Manufacturing Cobalt | `#2980B9` | Cobalt Blue | Main utility hull armor, forward tool sponsons, side outriggers |
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Lower skid frames, thruster exhaust cowlings, underbody armor |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Bow welder nose housing, wingtip safety lights, dock alignment marks |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Medium Cargo Container (component buffer hold), conveyor hub |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Core (4× Small Grid Batteries, 1× Small Nuclear Reactor) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Central Avionics Pod, Remote Control, Gyro, PB Utility |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Personnel handrails, tow eyelets, side rub rails |

---

### 0.3 Master Profile Elevation (Looking Starboard, X = 0)

```
        Z:  −5 (Stern)      −2          0          +2        +5 (Bow)
    L5      ┌────────────────┬──────────────────────┬───────────────┐  Roof Fairing
            │ [OMNI ANTENNA] │ Aero Spine Cover     │ [WORK LIGHTS] │  (Palette 4)
    L4      ├────────────────┼──────────────────────┼───────────────┤
            │ [PB] [GYRO]    │ [REMOTE CONTROL]     │ [WELDER RET]  │  Avionics Core
    L3      ├────────────────┼──────────────────────┼───────────────┤  (Palette 7)
            │ [4× BATTERIES] │ [MEDIUM CARGO HOLD]  │ [SHIP WELDER] │  Cargo & Tool
    L2      ├────────────────┼──────────────────────┼───────────────┤  (Palette 1 & 5)
            │ [LIFT 5..8]    │ [CONVEYOR HUB]       │ [LIFT 1..4]   │  Propulsion Ring
    L1      └───[SKID P/S]───┴──────[CONN ↓]────────┴────[SKID P/S]─┘  (Palette 0)
```

---

### 0.4 Master Conveyor & Tool Head Flow Schematic

```
                          BOW WORK TOOL HEAD (Level L2-L3)
                [ALB Welder Nose] (Small-Grid Industrial Welder)
                [ALB Cam Welder] (Precision Reticle Target Camera)
                                     │
                                     ▼ (Large Conveyor Pass-Through)
        ┌─────────────────────────────────────────────────────────────┐
        │ MEDIUM CARGO CONTAINER (3×3×3 sb · Centered X0, Z0, L2-L4)  │
        │ Capacity: 3,375 L · Holds Steel Plates, Motors, Computers   │
        └──────────────────────────────┬──────────────────────────────┘
                                       │
         ┌─────────────────────────────┴─────────────────────────────┐
         │ (Internal Fuel Line)                                      │ (Belly Port)
         ▼                                                           ▼
  [ALB Reactor] (0.5 MW)                                    [ALB Dock Conn] (L1, Face ↓)
  (Automated Uranium Pull)                                  (Mates to Orca Bay S2 / Pad)
```

---

### 0.5 Master Electrical & Thrust Budget

* **Power Generation**: 4× Small Grid Batteries (4.0 MW max burst, 2.0 MWh capacity) + 1× Small Nuclear Reactor (0.5 MW continuous backup) = **4.5 MW Peak Power**.
* **Propulsion Power Draw**:
  * 8× Downward Lift Thrusters: 8 × 80 kN = 640 kN (Draw: 8 × 560 kW = 4.48 MW at 100% burst).
  * 4× Forward Thrusters (Aft-facing): 4 × 80 kN = 320 kN.
  * 4× Reverse Thrusters (Bow-facing): 4 × 80 kN = 320 kN.
  * 6× Maneuver Thrusters (Port/Stbd/Zenith): 6 × 80 kN = 480 kN.
  * Empty Hover Draw at Earth 1 g: 83.4 kN required / 640 kN available ≈ **13.0% Throttle (580 kW)**.
  * Fully Laden Hover Draw (18.5 t): 181.5 kN required / 640 kN available ≈ **28.4% Throttle (1.27 MW)**.
  * Endurance: $> 1.5\text{ hours}$ continuous field welding and transport on battery power.

---

### 0.6 Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Fuselage & Skids** | Small armor blocks, half-blocks, slopes (Palette 4 & 0) | 58 | 2.8 t | 58 |
| **Atmospheric Thrusters**| 22× Small Atmospheric Thrusters (small grid) | 22 | 2.2 t | 330 |
| **Tool Head** | 1× Ship Welder, 1× Target Camera, 2× Spotlights | 4 | 0.5 t | 165 |
| **Cargo & Logistics** | 1× Medium Cargo (3×3×3 sb), 4× Conveyor Junctions | 5 | 0.6 t | 50 |
| **Power Plant** | 4× Small Grid Batteries, 1× Small Reactor (Palette 6) | 5 | 1.2 t | 125 |
| **Avionics & Automation**| 1× Remote Control, 1× Gyro, 1× PB, 1× Event Controller | 4 | 0.3 t | 95 |
| **Comms & Docking** | 1× Radio Antenna (15 km), 1× Small Connector, 1× Sensor | 3 | 0.2 t | 65 |
| **TOTAL (DRY)** | Complete Field Utility Craft (ALBATROSS-01) | **101** | **≈ 8.5 t** | **888** |

---

## Phase 1 · Belly Docking Skid & Cargo Core (Levels L1-L2)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 9 W × 11 L × 2 H sb (4.5 × 5.5 × 1.0 m)
       Coordinate Range: X −4 to +4, Z −5 to +5, Y = L1 to L2
       Key Datums:
       · Belly Docking Connector: [X0, Z0, Y = L1] (Faces Downward −Y)
       · Medium Cargo Container Center: [X0, Z0, Y = L2-L4]
       · Port Landing Skid: X = −3, spanning Z −4 to +4
       · Starboard Landing Skid: X = +3, spanning Z −4 to +4
```

### Step 13.1 · Belly Connector & Reinforced Skids (L1)

Construct the reinforced dual landing skids at X = ±3 sb using heavy armor half-blocks. Mount the small grid docking connector at origin (X0, Z0, L1) facing downward (−Y) to mate with the Orca Bay S2 floor dock or ground gantry pads.

```
Plan view, Y = L1 (Belly Skids & Connector)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +4  [SKID P] ················· [SKID S]  ◄── Bow Skid Contact Points (Palette 0)
           │                             │
    Z:  0  │           [CONN ↓]          │      ◄── Small Grid Connector (`ALB Dock Conn`)
           │        (Facing Down)        │          (Centered X0, Z0, L1)
    Z: −4  [SKID P] ················· [SKID S]  ◄── Stern Skid Contact Points
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.1 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Armor Half-Blk│ 16  │ 0.64 t / 16   │ Palette Slot 0 (Slate); skid base rails  │
│ Connector (Small)   │ 1   │ 0.16 t / 25   │ `ALB Dock Conn` (Centered X0, Z0, face −Y│
│ Conveyor Junction sb│ 4   │ 0.16 t / 20   │ Large-port conveyor routing cubes        │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 13.2 · Medium Cargo Container & 8-Thruster Lift Array (L1-L2)

Mount 1 Medium Cargo Container (3×3×3 sb) directly above the belly connector. Install 8 vertical atmospheric lift thrusters into the port and starboard skid outriggers (4 port, 4 starboard) facing downward (−Y).

```
Plan view, Y = L1-L2 (Lift Thrusters & Cargo Core)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3  [LIFT 1][LIFT 2] ┌───┐ [LIFT 3][LIFT 4]  ◄── Forward Downward Thrusters
    Z:  0         │         │MED│         │          ◄── Medium Cargo Hold (3×3×3 sb)
    Z: −3  [LIFT 5][LIFT 6] └───┘ [LIFT 7][LIFT 8]  ◄── Aft Downward Thrusters
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.2 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Capacity & Performance            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Medium Cargo (sb)   │ 1   │ 0.45 t / 30   │ `ALB Hold Components` (3,375 L volume)   │
│ Small Atmo Thruster │ 8   │ 0.80 t / 120  │ `ALB Thruster Lift 1..8` (Face DOWN −Y)  │
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `ALB Cam Belly` (Downward docking camera)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 13.1A: DOCKING CLEARANCE & LIFT GAP**
> Before building Level L3 structure:
> 1. Ensure `ALB Dock Conn` face is flush with the skid baseline so it locks effortlessly when landing.
> 2. Verify large conveyor ports on Medium Cargo connect directly to the bottom connector and the forward tool port.
> 3. Verify all 8 downward lift thrusters have a clear 1 sb exhaust gap beneath the nozzles.

---

#### Visual Milestone 13-1 · Belly Skids & Cargo Hold Complete

```
Visual Checkpoint Silhouette (Looking Starboard at Cargo Core):
               [MEDIUM CARGO HOLD L2-L4]
                          │
     ═══════[LIFT 1..2]──[SKID RUNNER]──[LIFT 5..6]═══════ (Level L1)
                          │
                   [ALB DOCK CONN ↓]
```

* [ ] Skids support the craft evenly with 0.5 m belly ground clearance.
* [ ] Medium cargo container accessible from terminal; inventory reads 3,375 L capacity.
* [ ] All 8 lift thrusters provide balanced upward thrust centered through the loaded CoM.

---

## Phase 2 · Bow Welder Head & Propulsion Ring (Levels L2-L4)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 9 W × 11 L × 3 H sb (4.5 × 5.5 × 1.5 m)
       Coordinate Range: X −4 to +4, Z −5 to +5, Y = L2 to L4
       Key Datums:
       · Ship Welder Tool Face: [X0, Z +5, Y = L2-L3] (Faces Bow +Z)
       · Work Lights: X = ±2, Z +5, Y = L4
       · Main Forward Thrusters (4×): X = ±2, Z −5, Y = L2-L3 (Face Aft)
       · Reverse Thrusters (4×): X = ±2, Z +4, Y = L2-L3 (Face Bow)
```

### Step 13.3 · Industrial Welder Tool Head & Precision Recon Pod (L2-L3)

Mount the small-grid Ship Welder on the bow centerline (X0, Z +4 to +5) mating directly to the forward large conveyor port of the Medium Cargo Container. Flank the tool tip with twin high-intensity searchlights and a precision alignment reticle camera.

```
Front elevation, looking Stern at Welder Tool Head (Z = +5):
       L4       [SPOTLIGHT P]  [ALB CAM RETICLE]  [SPOTLIGHT S]
       L3     ┌─────────────────────────────────────────────────┐
       L2     │          [ALB WELDER NOSE (1×2×2 sb)]           │
       L1     ■═════════════════════════════════════════════════■
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.3 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Placement                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Ship Welder (sb)    │ 1   │ 0.40 t / 150  │ `ALB Welder Nose` (Centered X0, Z +5)    │
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `ALB Cam Welder` (Precision aim reticle) │
│ Spotlight (Small)   │ 2   │ 0.06 t / 20   │ `ALB Light Work Port / Stbd` (Floodlight)│
│ Sensor (Small)      │ 1   │ 0.03 t / 15   │ `ALB Sensor WeldZone` (2 m trigger field)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 13.4 · 14-Thruster 6-DOF Maneuvering Ring (L2-L4)

Install the 14 horizontal and vertical translation thrusters across the fuselage outriggers:
* 4× Forward Thrusters (facing Aft −Z at Z −5): 320 kN main cruise drive.
* 4× Reverse Thrusters (facing Bow +Z at Z +4): 320 kN high-speed precision braking.
* 2× Port Thrusters (facing Port −X at X −4): 160 kN lateral strafe.
* 2× Starboard Thrusters (facing Stbd +X at X +4): 160 kN lateral strafe.
* 2× Zenith Thrusters (facing Up +Y at Z 0): 160 kN downward push for inverted terrain docking.

```
Plan view, Y = L3 (Horizontal Thruster Layout)
        X: −4   −3   −2     0    +2   +3   +4
    Z: +4  [REV 1]   [REV 2] [WELDER] [REV 3]   [REV 4]  ◄── Facing Bow +Z (Braking)
    Z: +1  [PORT 1]                           [STBD 1]   ◄── Facing Outward (Lateral)
    Z: −1  [PORT 2]                           [STBD 2]
    Z: −5  [FWD 1]   [FWD 2]         [FWD 3]   [FWD 4]  ◄── Facing Aft −Z (Main Drive)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.4 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Vector Orientation & Operating Specs     │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Atmo Thruster │ 14  │ 1.40 t / 210  │ `ALB Thruster Fwd/Rev/Port/Stbd/Zenith`  │
│ Small Armor Slopes  │ 20  │ 0.40 t / 20   │ Palette Slot 4 (Cobalt); nacelle fairing │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 13.2A: WELDER PULL & PRECISION HOVER CHECK**
> 1. Deposit 500 Steel Plates into `ALB Hold Components`; verify `ALB Welder Nose` draws components automatically when welding test blocks.
> 2. Test reverse thruster brake distance: confirm craft decelerates from 50 km/h to dead stop in $< 12\text{ m}$.
> 3. Verify `ALB Sensor WeldZone` triggers welder activation only when damaged blocks are within 2.0 m.

---

## Phase 3 · Power Core, Avionics & Armor Hull (Levels L3-L5)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 9 W × 11 L × 3 H sb (4.5 × 5.5 × 1.5 m)
       Coordinate Range: X −4 to +4, Z −5 to +5, Y = L3 to L5
       Avionics & Power Datums:
       · 4× Small Batteries: X = ±1, Z −2 to −3, Y = L3
       · Small Reactor: X0, Z −4, Y = L3
       · Remote Control: [X0, Z +2, Y = L4] (Faces Bow +Z)
       · Micro Gyroscope: [X0, Z +1, Y = L4]
       · PB Utility: [X0, Z −1, Y = L4]
       · Comms Antenna: [X0, Z −4, Y = L5]
```

### Step 13.5 · Power Vault & Autonomous Avionics Rack (L3-L4)

Install 4 Small Grid Batteries and 1 Small Nuclear Reactor directly aft of the Medium Cargo Hold. Mount the flight avionics, gyroscope, and RIG//OS Programmable Block along the upper center spine.

```
Plan view, Y = L4 (Power Vault & Avionics Core)
        X: −2   −1    0   +1   +2
    Z: +2  ┌─────┬───────────┬─────┐
           │     │ [REMOTE]  │     │  ◄── Autopilot Brain (`ALB Remote Control`)
    Z:  0  │     │  [GYRO]   │     │  ◄── Precision Stabilization Gyro
    Z: −1  │     │ [PB UTIL] │     │  ◄── Zero-GC Maintenance Script
    Z: −3  │[BAT 1][BAT 2][BAT 3][BAT 4]  ◄── 4× Small Batteries (Palette Slot 6: Red)
    Z: −4  └─────┴─[REACTOR]─┴─────┘  ◄── Small Nuclear Reactor (0.5 MW backup)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 13.5 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Operating Settings & Performance         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Grid Battery  │ 4   │ 1.00 t / 80   │ `ALB Battery 1..4` (2.0 MWh total cap)   │
│ Small Reactor (sb)  │ 1   │ 0.15 t / 25   │ `ALB Reactor` (0.5 MW continuous backup) │
│ Remote Control (sb) │ 1   │ 0.05 t / 25   │ `ALB Remote Control` (Autopilot helm)    │
│ Gyroscope (sb)      │ 1   │ 0.08 t / 15   │ `ALB Gyro` (Precision station keeping)   │
│ Programmable Block  │ 1   │ 0.08 t / 25   │ `ALB PB Utility` (Repair & logistics)    │
│ Event Controller    │ 1   │ 0.03 t / 20   │ `ALB EC CargoWatch` (Stock alert monitor)│
│ Radio Antenna (sb)  │ 1   │ 0.04 t / 25   │ `ALB Antenna` (Range: 15,000 m broadcast)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 13.6 · Heavy Protective Hull & Service Fairings (L4-L5)

Encase the craft in reinforced sloped armor fairings (Palette Slot 4: Manufacturing Cobalt) with high-visibility hazard yellow rub rails along the flanks. Add top armor louvers to shield the avionics pod from falling debris during vertical shaft rescue operations.

```
Plan view, Y = L5 (Utility Roof Canopy & Rub Rails)
        X: −4   −2    0   +2   +4
    Z: +5       ┌────[WELDER]────┐       ◄── Heavy Nose Bumper
    Z: +3  [RAIL]┌──────────────┐[RAIL]  ◄── High-Viz Yellow Side Rub Rails (Palette 9)
    Z:  0  │    │  ROOF CANOPY  │    │
    Z: −3  │    │ (COBALT BLUE) │    │
    Z: −5  [RAIL]└──[ANTENNA]───┘[RAIL]  ◄── Long-Range Comms Mast
```

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 13.3A: AVIONICS RIGGING & POWER FLOAT CHECK**
> 1. Verify `ALB Remote Control` horizon matches level landing baseline (pitch 0°, roll 0°).
> 2. Ensure `ALB PB Utility` compiles clean under MDK2 with zero runtime memory allocations.
> 3. Verify total electrical surge output delivers 4.5 MW peak with zero brownout under simultaneous all-thruster burst and welder operation.

---

#### Visual Milestone 13-2 · Complete ALBATROSS-01 Utility Craft Commissioned

```
Visual Checkpoint Silhouette (Looking Port at Complete Craft):
                         [ROOF CANOPY L5]
                                │
        [ANTENNA]               │                 [WELDER HEAD & CAM]
            \                   │                         /
     ════════╧══════════════════╧═════════════════════════╧═════════
          9 W × 11 L × 5 H sb UTILITY & REPAIR CRAFT (ALBATROSS-01)
                                │
                         [ALB DOCK CONN ↓]
```

* [ ] Craft empty mass confirmed: `≈ 8.5 t` via info tab.
* [ ] 22 atmospheric thrusters deliver instantaneous, vibration-free 6-DOF station keeping.
* [ ] Welder tool head operates with 100% component feed from the Medium Cargo hold.

---

## Phase 4 · Master Control Logic & Hardware Scripting Contract (Claude Code)

### Step 13.7 · Terminal Namespace & Group Taxonomy

#### 1. Group Taxonomy
* `ALB Thrusters All` : All 22 atmospheric thrusters.
* `ALB Thrusters Lift` : 8× Downward lift thrusters (`ALB Thruster Lift 1..8`).
* `ALB Batteries` : 4× Small Grid Batteries (`ALB Battery 1..4`).
* `ALB Lights` : 2× Work Spotlights + 2× Wingtip Beacons.
* `ALB Tools` : `ALB Welder Nose` (main repair head).
* `ALB Cameras` : `ALB Cam Welder` (reticle target) + `ALB Cam Belly` (docking align).

#### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `ALB EC Battery` | `ALB Batteries` Stored Power | Stored Charge $< 25\%$ | Trigger RTH (Return to Base) | Normal Operation |
| `ALB EC Cargo` | `ALB Hold Components` Mass | Volume Filled $> 90\%$ | Alert C2 Bridge: Hold Full | Normal Capacity |
| `ALB EC Dock` | `ALB Dock Conn` State | Connector: Connected | Batteries $\rightarrow$ Recharge · Thrusters OFF | Batteries $\rightarrow$ Auto · Thrusters ON |
| `ALB EC Welder` | `ALB Sensor WeldZone` | Damaged Block $\le 2\text{ m}$ | `ALB Welder Nose` Turn ON | Welder Turn OFF |

#### 3. Inter-Grid Communication (IGC) Protocol (Fleet Maintenance Bus)

| IGC Tag / Channel | Transceiver | Direction | Message Payload Format | Functional Purpose |
|---|---|---|---|---|
| `TMS.UTIL.STATUS` | Radio Antenna | Outbound | `craft_id;status;gps_coords;hold_pct;bat_pct`| Telemetry feed to C2 Bridge Video Wall |
| `TMS.REPAIR.CALL` | Radio Antenna | Inbound | `caller_id;target_gps;damage_type;urgency` | Dispatches Albatross to damaged drone |
| `TMS.SUPPLY.RESTOCK`| Radio Antenna | Outbound | `craft_id;req_plates;req_motors;req_comps` | Orders component restock from Orca Bay S2 |

---

## Phase 5 · Commissioning Checklist & Acceptance Trials (RIG Test)

### A · Precision Hover & Station Keeping Trials
* [ ] **Point-Blank Hover Stability:** Hover 1.5 m from an Orca hull block; release flight sticks; verify craft drifts $< 0.05\text{ m}$ over 60 seconds in 1 g wind.
* [ ] **Laden Flight Dynamics:** Fill Medium Cargo with 10 t Steel Plates (total mass ≈ 18.5 t); confirm upward lift thrusters maintain hover at 28.4% throttle without thermal fade.

### B · Field Repair & Salvage Operations
* [ ] **Automated Welding Trial:** Approach a damaged Remora drill head; confirm `ALB Sensor WeldZone` detects damage at 2.0 m and auto-ignites `ALB Welder Nose`; verify full repair completes in $< 10\text{ seconds}$.
* [ ] **Component Restock at Orca:** Dock to Orca Bay S2 connector; verify sorter pulls 500 Steel Plates, 100 Construction Components, and 50 Motors into `ALB Hold Components`.

### C · Carrier Recovery & Pad Validation
* [ ] **Orca Bay S2 Ingress:** Approach Orca Bay S2 at hover; align with bay guides; verify clearance margin is $\ge 1.5\text{ B}$ (3 sb) on all sides; verify belly connector lock.
* [ ] **Ground Base Gantry Landing:** Land on Ground Base gantry pad; confirm `ALB Dock Conn` locks flush and initiates rapid battery recharge.
