# Kestrel · Visual Build Instructions

## Chapter 12 · KESTREL-01 Prospector Scout Drone (KES-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the KESTREL-01 (KES-01) Fast Autonomous Prospector / Deposit Scout Drone.
Each step features: an exact ASCII blueprint, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical conveyor/connector orientations, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

Source specs:
* Authority: Blae Decisions 25 (Earth 1 g), 31 (Orca support craft complement: Bay P2), 41 (3× Inventory), 50 (Mastodon forward scout pads: P1 & S1), 52 (Dual-tier logistics telemetry)
* Role: High-speed autonomous geological prospecting, ore vein reconnaissance, and long-range GPS waypoint survey for both the ORCA-01 carrier and the MASTODON-01 heavy excavator.
* Grid: Small Grid (1 sb = 0.5 m) · Bounding Envelope: 7 W × 9 L × 5 H sb (3.5 × 4.5 × 2.5 m)
* Dry Mass: ≈ 4.8 t · Hover Thrust: 320 kN (T/W ≈ 6.8 at Earth 1 g)

Last updated: 2026-10-17

---

## 0 · Reference Grid, In-Game Paint Palette & Drone Envelope

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Geometric center of the belly docking connector at Level L1.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. (Range: X −3 to +3 sb).
* **Y (Elevation):** Belly landing skids and dock port are Level L1, advancing upward to Avionics Roof at Level L5.
* **Z (Length / Longitudinal):** Stern thruster box is negative (−), Bow sensor nose is positive (+). (Range: Z −4 to +4 sb).

```
Spatial Alignment Triad:
       [+Y: Zenith / Aero Roof L5]
              │
              │   [+Z: Bow / Sensor Nose & Camera]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Main Thrusters & Antenna]
              │
       [−Y: Belly Dock / Ground Skids L1]
```

### 0.2 In-Game Paint Palette & Functional Color Coding

All structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 8** | Scout Flight White | `#ECF0F1` | Clean White | Aerodynamic outer fuselage, wing sponsons, top roof fairing |
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Belly landing skids, thruster exhaust casings, underbody frame |
| **Slot 1** | Hazard Amber | `#E6A100` | Bright Amber | Wingtip navigation beacons, dock alignment crosshairs |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Geological Ore Detector pod, forward survey sensor block |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Power Core (2× Small Grid Batteries, 1× Small Reactor) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Central Avionics Core, Remote Control, Gyro, PB Prospector |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Maintenance grab points, battery charge diagnostic port |

---

### 0.3 Master Profile Elevation (Looking Starboard, X = 0)

```
        Z:  −4 (Stern)      −2          0          +2        +4 (Bow)
    L5      ┌────────────────┬──────────────────────┬───────────────┐  Roof Fairing
            │ [OMNI ANTENNA] │ Aero Spine Cover     │ [RECON CAM]   │  (Palette 8)
    L4      ├────────────────┼──────────────────────┼───────────────┤
            │ Main Drive Aft │ [PB] [GYRO] [REMOTE] │ [SEARCHLIGHT] │  Avionics Core
    L3      ├────────────────┼──────────────────────┼───────────────┤  (Palette 7)
            │ [REACTOR]      │ [BATTERY 1 & 2]      │ [ORE DETECTOR]│  Power & Sensor
    L2      ├────────────────┼──────────────────────┼───────────────┤  (Palette 4 & 6)
            │ [DAMP THRUSTER]│ [KES DOCK CONN]      │ [GROUND SENS] │  Belly Skid
    L1      └───[SKID P/S]───┴──────[CONN ↓]────────┴────[SKID P/S]─┘  (Palette 0)
```

---

### 0.4 Master Conveyor & Avionics Schematic

```
                          BOW RECONNAISSANCE HEAD
             [KES Cam Nose] + [KES Ore Detector] + [Searchlights]
                                     │
                                     ▼
                     [KES Remote Control] + [KES PB]
                                     │
                                     ▼
        ┌─────────────────────────────────────────────────────────────┐
        │ POWER & INTERNAL SERVICE MANIFOLD (Level L2)                │
        │ 2× Small Batteries + 1× Small Reactor + 1× Small Cargo Hold │
        └──────────────────────────────┬──────────────────────────────┘
                                       │
                                       ▼ (Direct Belly Port)
                            [KES Dock Conn] (Level L1)
                                       │
                                       ▼ (Offloads Data & Recharges)
                  [MASTODON SCOUT PAD] / [ORCA BAY P2]
```

---

### 0.5 Master Electrical & Thrust Budget

* **Power Generation**: 2× Small Grid Battery (2.0 MW max discharge, 1.0 MWh capacity) + 1× Small Reactor (0.5 MW continuous backup) = **2.5 MW Peak Power**.
* **Propulsion Power Draw**:
  * 4× Downward Lift Thrusters: 4 × 80 kN = 320 kN (Draw: 4 × 560 kW = 2.24 MW at 100% burst).
  * 2× Forward Thrusters (Aft-facing): 2 × 80 kN = 160 kN.
  * 2× Reverse Thrusters (Bow-facing): 2 × 80 kN = 160 kN.
  * 6× Maneuver Thrusters (Port/Stbd/Zenith): 6 × 80 kN = 480 kN total authority.
  * Cruise Hover Draw at Earth 1 g: 47.1 kN required / 320 kN available ≈ **14.7% Throttle (330 kW)**.
  * Cruise Endurance: $> 3.0\text{ hours}$ continuous flight on battery power alone.

---

### 0.6 Master Pre-Flight Bill of Materials (BOM)

| System Category | Block Types & Specifications | Total Qty | Est. Mass | PCU |
|---|---|---|---|---|
| **Fuselage & Skids** | Small armor blocks, half-blocks, slopes (Palette 8 & 0) | 38 | 1.8 t | 38 |
| **Atmospheric Thrusters**| 14× Small Atmospheric Thrusters (small grid) | 14 | 1.4 t | 210 |
| **Power Plant** | 2× Small Grid Batteries, 1× Small Reactor (Palette 6) | 3 | 0.8 t | 85 |
| **Avionics & Automation**| 1× Remote Control, 1× Gyro, 1× PB, 1× Event Controller | 4 | 0.3 t | 95 |
| **Sensors & Recon** | 1× Ore Detector, 2× Cameras, 2× Searchlights, 1× Sensor | 6 | 0.3 t | 140 |
| **Comms & Docking** | 1× Radio Antenna (10 km), 1× Small Connector, 1× Cargo | 3 | 0.2 t | 65 |
| **TOTAL (DRY)** | Complete Scout Drone (KESTREL-01) | **68** | **≈ 4.8 t** | **633** |

---

## Phase 1 · Belly Docking Skid & Sensor Foundation (Levels L1-L2)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 9 L × 2 H sb (3.5 × 4.5 × 1.0 m)
       Coordinate Range: X −3 to +3, Z −4 to +4, Y = L1 to L2
       Key Datums:
       · Belly Docking Connector: [X0, Z0, Y = L1] (Faces Downward −Y)
       · Ore Detector Pod: [X0, Z +2, Y = L2]
       · Port Landing Skid: X = −2, spanning Z −3 to +3
       · Starboard Landing Skid: X = +2, spanning Z −3 to +3
```

### Step 12.1 · Belly Connector & Twin Landing Skids (L1)

Construct the reinforced lower landing skids using heavy armor half-blocks at X = ±2 sb. Mount the small grid docking connector at the center origin (X0, Z0, L1) facing downward (−Y) to mate with the carrier landing pads.

```
Plan view, Y = L1 (Belly Skids & Connector)
        X: −2   −1    0   +1   +2
    Z: +3  [SKID P] ······ [SKID S]  ◄── Bow Skid Contact Points (Palette 0: Slate)
           │                  │
    Z:  0  │      [CONN ↓]    │      ◄── Small Grid Connector (`KES Dock Conn`)
           │    (Facing Down) │          (Centered X0, Z0, L1)
    Z: −3  [SKID P] ······ [SKID S]  ◄── Stern Skid Contact Points
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.1 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Placement & Alignment           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Armor Half-Blk│ 12  │ 0.48 t / 12   │ Palette Slot 0 (Slate); skid base rails  │
│ Connector (Small)   │ 1   │ 0.16 t / 25   │ `KES Dock Conn` (Centered X0, Z0, face −Y│
│ Conveyor Junction sb│ 2   │ 0.08 t / 10   │ Center conveyor routing above connector  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 12.2 · Geological Ore Detector & Downward Lift Cluster (L1-L2)

Mount the small-grid Ore Detector in the forward belly bay (X0, Z +2) and install the 4 vertical atmospheric lift thrusters into the skid recesses.

```
Plan view, Y = L1-L2 (Lift Thrusters & Sensor Mount)
        X: −2   −1    0   +1   +2
    Z: +3  [LIFT 1] ────── [LIFT 2]  ◄── Forward Downward Thrusters (Face −Y)
                    [ORE DET]        ◄── Ore Detector (Palette Slot 4: Teal)
    Z:  0         [CONN ↓]
    Z: −3  [LIFT 3] ────── [LIFT 4]  ◄── Aft Downward Thrusters (Face −Y)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.2 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Operating Settings         │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Ore Detector (sb)   │ 1   │ 0.12 t / 15   │ `KES Ore Detector` (Range: 50 m max)     │
│ Small Atmo Thruster │ 4   │ 0.40 t / 60   │ `KES Thruster Lift 1..4` (Face DOWN −Y)  │
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `KES Cam Ground` (Downward docking align)│
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 12.1A: DOCKING CLEARANCE & SENSOR CONDUIT**
> Before building Level L2 decks:
> 1. Ensure `KES Dock Conn` sits flush with the skid baseline (0.0 to 0.1 m clearance) so it locks effortlessly when skids rest on landing pads.
> 2. Verify `KES Ore Detector` conveyor pass-through connects directly to the internal cargo trunk.
> 3. Verify all 4 lift thrusters have a clear 1 sb exhaust gap beneath the nozzles.

---

#### Visual Milestone 12-1 · Belly Skid & Sensor Pod Complete

```
Visual Checkpoint Silhouette (Looking Starboard at Belly Skid):
              [ORE DETECTOR L2]       [CONVEYOR HUB]
                     \                      /
     ═══════[LIFT 1]──[SKID RUNNER]──[LIFT 3]═══════ (Level L1)
                             │
                      [KES DOCK CONN ↓]
```

* [ ] Skids rest completely stable on level decks without wobbling.
* [ ] Dock connector engages lock state immediately upon touching a carrier pad.
* [ ] Ore detector active scan confirms zero local grid interference.

---

## Phase 2 · Power Core & Propulsion Ring (Levels L2-L3)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 9 L × 2 H sb (3.5 × 4.5 × 1.0 m)
       Coordinate Range: X −3 to +3, Z −4 to +4, Y = L2 to L3
       Key Datums:
       · Battery Bank: X = ±1, Z 0, Y = L2
       · Small Reactor: X0, Z −2, Y = L2
       · Main Forward Thrusters: X = ±2, Z −4, Y = L3 (Face Aft)
       · Reverse Thrusters: X = ±2, Z +3, Y = L3 (Face Bow)
```

### Step 12.3 · Power Vault & Fuel Storage (L2)

Install 2 Small Grid Batteries and 1 Small Nuclear Reactor directly above the conveyor junction cubes. Add 1 Small Cargo Container for spare uranium and emergency geological core samples.

```
Plan view, Y = L2 (Power & Internal Hold)
        X: −1    0   +1
    Z: +1  ┌───────────┐
           │[CARGO HOLD│  ◄── Small Cargo Container (Centered X0, Z +1)
    Z:  0  │[BAT 1][BAT│  ◄── 2× Small Batteries (Palette Slot 6: Red)
    Z: −2  │ [REACTOR] │  ◄── 1× Small Nuclear Reactor (15 kW / 0.5 MW burst)
           └───────────┘
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.3 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Capacity & Performance            │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Grid Battery  │ 2   │ 0.50 t / 40   │ `KES Battery 1, 2` (1.0 MWh total cap)   │
│ Small Reactor (sb)  │ 1   │ 0.15 t / 25   │ `KES Reactor` (0.5 MW emergency backup)  │
│ Small Cargo Box     │ 1   │ 0.10 t / 10   │ `KES Hold Samples` (Sample ore cache)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 12.4 · 10-Thruster Omnidirectional Maneuver Ring (L2-L3)

Mount the 10 horizontal and vertical maneuvering thrusters into the fuselage outriggers:
* 2× Forward Thrusters (facing Aft −Z at Z −4): Primary cruise drive (160 kN).
* 2× Reverse Thrusters (facing Bow +Z at Z +3): High-speed braking (160 kN).
* 2× Port Thrusters (facing Port −X at X −3): Lateral translation (160 kN).
* 2× Starboard Thrusters (facing Stbd +X at X +3): Lateral translation (160 kN).
* 2× Down-Push Thrusters (facing Zenith +Y at Z 0): Inverted terrain huggers (160 kN).

```
Plan view, Y = L3 (Thruster Vector Alignment)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +3       [REV 1]           [REV 2]       ◄── Facing Bow +Z (Braking)
    Z: +1  [PORT 1]                   [STBD 1]  ◄── Facing Outward (Lateral)
    Z: −1  [PORT 2]                   [STBD 2]
    Z: −4       [FWD 1]           [FWD 2]       ◄── Facing Aft −Z (Main Drive)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.4 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Vector Orientation & Role                │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Small Atmo Thruster │ 10  │ 1.00 t / 150  │ `KES Thruster Fwd/Rev/Port/Stbd/Zenith`  │
│ Small Armor Slopes  │ 16  │ 0.32 t / 16   │ Aerodynamic outrigger fairing frames     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 12.2A: THRUSTER AUTHORITY & POWER BUDGET**
> 1. Verify all 14 atmospheric thrusters respond correctly to manual flight override in all 6 degrees of freedom.
> 2. Confirm total power draw under 100% all-thruster burst does not exceed 2.4 MW (covered by 2× Batteries + Reactor).
> 3. Verify zero thruster exhaust impinges on armor blocks within 2 sb of nozzle exits.

---

## Phase 3 · Avionics Core & Aerodynamic Shell (Levels L3-L5)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Envelope: 7 W × 9 L × 3 H sb (3.5 × 4.5 × 1.5 m)
       Coordinate Range: X −3 to +3, Z −4 to +4, Y = L3 to L5
       Avionics Stack:
       · Remote Control: [X0, Z +1, Y = L3] (Faces Bow +Z, Up +Y)
       · Micro Gyroscope: [X0, Z 0, Y = L3]
       · PB Prospector: [X0, Z −1, Y = L3]
       · Long-Range Antenna: [X0, Z −3, Y = L4]
       · Forward Recon Camera: [X0, Z +4, Y = L4]
```

### Step 12.5 · Autonomous Avionics Rack & Sensors (L3-L4)

Install the flight control avionics, autopilot brain, and forward reconnaissance optics along the vehicle centerline.

```
Elevation, looking West through Centerline (X0, Y = L3-L5):
       L5  ═════════[AERO SPINE HOOD]═════════  Roof Fairing (Palette 8: White)
       L4  [ANTENNA]             [CAM NOSE]     ◄── Forward Recon Optics & Comms
       L3  [PB PROSPECT] [GYRO] [REMOTE CTRL]   ◄── Central Avionics Brain (Palette 7)
       L2  [REACTOR]   [BAT 1]   [CARGO HOLD]   ◄── Power & Hold Deck
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 12.5 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Configuration & Operating Specs          │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Remote Control (sb) │ 1   │ 0.05 t / 25   │ `KES Remote Control` (Primary Autopilot) │
│ Gyroscope (sb)      │ 1   │ 0.08 t / 15   │ `KES Gyro` (Roll/Pitch/Yaw Stabilization)│
│ Programmable Block  │ 1   │ 0.08 t / 25   │ `KES PB Prospector` (IGC & Waypoint Nav) │
│ Event Controller    │ 1   │ 0.03 t / 20   │ `KES EC AutoDock` (Failsafe RTH logic)   │
│ Radio Antenna (sb)  │ 1   │ 0.04 t / 25   │ `KES Antenna` (Range: 10,000 m broadcast)│
│ Camera (Small)      │ 1   │ 0.02 t / 15   │ `KES Cam Nose` (High-zoom target camera) │
│ Spotlight (Small)   │ 2   │ 0.06 t / 20   │ `KES Light Port / Stbd` (Forward beams)  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 12.6 · Aerodynamic Fuselage Fairing (L4-L5)

Enclose the drone in lightweight sloped armor fairings (Palette Slot 8: Clean White) with amber hazard wingtip markers. The aerodynamic wedge profile reduces drag and shields internal components from dust storms and minor tree collisions.

```
Plan view, Y = L4-L5 (Fuselage Canopy & Wing Fairings)
        X: −3   −2   −1     0    +1   +2   +3
    Z: +4       [LIGHT P] [NOSE CAM] [LIGHT S]   ◄── Tapered Forward Wedge
    Z: +2  [TIP P] ┌─────────────────┐ [TIP S]   ◄── Amber Wingtip Markers (Palette 1)
    Z:  0  │       │  [AERO SPINE]   │       │
    Z: −2  │       │                 │       │
    Z: −4       [AFT]   [ANTENNA]   [AFT]        ◄── Dual Thruster Fairings
```

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 12.3A: AVIONICS ALIGNMENT & SCRIPT INITIALIZATION**
> 1. Verify `KES Remote Control` forward orientation points strictly toward Bow (+Z).
> 2. Check `KES PB Prospector`: confirm script compiles with zero allocations under MDK2 PB rules.
> 3. Verify `KES Antenna` broadcast range is set to `10,000 m` with HUD display name set to `KESTREL-01`.

---

#### Visual Milestone 12-2 · Complete KESTREL-01 Scout Drone Commissioned

```
Visual Checkpoint Silhouette (Looking Port at Complete Drone):
                         [AERO SPINE HOOD (L5)]
                                 │
         [ANTENNA]               │                 [NOSE CAM]
             \                   │                    /
      ════════╧══════════════════╧════════════════════╧═════════
          7 W × 9 L × 5 H sb FAST PROSPECTOR DRONE (KESTREL-01)
                             │
                      [KES DOCK CONN ↓]
```

* [ ] Drone dry mass confirmed: `≈ 4.8 t` in terminal info tab.
* [ ] T/W ratio verified: `≈ 6.8` (provides instant 0-100 m/s acceleration in ~3 seconds).
* [ ] Ore detector highlights all subsurface veins up to 50 m depth directly to the HUD.

---

## Phase 4 · Master Control Logic & Hardware Scripting Contract (Claude Code)

### Step 12.7 · Terminal Namespace & Group Taxonomy

#### 1. Group Taxonomy
* `KES Thrusters All` : All 14 atmospheric thrusters.
* `KES Thrusters Lift` : 4× Downward lift thrusters (`KES Thruster Lift 1..4`).
* `KES Batteries` : 2× Small Grid Batteries (`KES Battery 1, 2`).
* `KES Lights` : 2× Forward Searchlights + 2× Wingtip Beacons.
* `KES Cameras` : `KES Cam Nose` (forward recon) + `KES Cam Ground` (docking align).

#### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `KES EC Battery` | `KES Batteries` Stored Power | Stored Charge $< 20\%$ | Trigger RTH (Return to Base) | Normal Survey |
| `KES EC Dock` | `KES Dock Conn` State | Connector: Connected | Batteries $\rightarrow$ Recharge · Thrusters OFF | Batteries $\rightarrow$ Auto · Thrusters ON |
| `KES EC VeinDetect`| `KES Ore Detector` | Ore Vein Detected | PB: Save GPS & Broadcast IGC | Continue Search |

#### 3. Inter-Grid Communication (IGC) Protocol (Fleet Recon Bus)

| IGC Tag / Channel | Transceiver | Direction | Message Payload Format | Functional Purpose |
|---|---|---|---|---|
| `TMS.SCOUT.STATUS` | Radio Antenna | Outbound | `drone_id;status;gps_coords;bat_pct` | Real-time position & telemetry to C2 Video Wall |
| `TMS.SCOUT.VEIN` | Radio Antenna | Outbound | `ore_type;depth_m;gps_coords;est_yield`| Transmits verified deposit GPS to Orca & Mastodon |
| `TMS.SCOUT.RECALL` | Radio Antenna | Inbound | `target_carrier_id;pad_id;abort_cmd` | Emergency recall command from C2 Bridge |

---

## Phase 5 · Commissioning Checklist & Acceptance Trials (RIG Test)

### A · High-G Flight & Dynamics Trials
* [ ] **Vertical Climb Rate:** From stationary hover at sea level, apply 100% upward lift; verify climb rate reaches 100 m/s in $< 2.5\text{ seconds}$.
* [ ] **Terrain Contour Hugging:** Fly at 80 km/h at 10 m altitude over rolling hills; confirm autopilot maintains altitude without ground strike.

### B · Prospecting & Target Tracking
* [ ] **50 m Vein Acquisition:** Fly over buried Uranium and Iron deposits; confirm `KES Ore Detector` registers both veins and transmits GPS coordinates via `TMS.SCOUT.VEIN`.
* [ ] **Recon Camera Zoom:** Target a terrain feature 2,000 m away; engage `KES Cam Nose` zoom; confirm crisp visual recognition without camera jitter.

### C · Carrier Recovery & Docking Acceptance
* [ ] **Mastodon Scout Pad Dock:** Autopilot descent onto Mastodon Forward Pad 1 (`MAS Kestrel Dock 1`); verify `KES Dock Conn` locks on first attempt; verify battery switches to Recharge.
* [ ] **Orca Bay P2 Entry:** Approach Orca at hover; enter Bay P2; confirm clearance margin is $\ge 2\text{ B}$ on all sides; verify connector lock and power shutdown.
