# Caterpillar · Visual Build Instructions

## Chapter 11 · CATERPILLAR-01 Land Train (CAT-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the CATERPILLAR-01 (CAT-01) Heavy Wheeled Land Train.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

Source specs:
* Authority: Blae Decisions 10 (Vanilla 5×5 wheels), 25 (Earth 1 g), 35 (Mechanical subgrids / hinges permitted), 41 (3× Inventory), 51 (Caterpillar bulk land train), 52 (Dual-tier logistics)
* Master Manual Reference: `docs/rig/lego-instructions-v1.0.md` Chapter 11
* Grid: Large Grid (1 B = 2.5 m) · Total Consist Length: 42 B (105.0 m) for 3-car consist · Envelope: 7 W × 42 L × 4 H blocks (17.5 × 105.0 × 10.0 m)

Last updated: 2026-10-17

---

## 0 · Reference Grid, In-Game Paint Palette & Consist Envelope

### 0.1 Coordinate Triad & Axis Conventions
* **Consist Reference Origin (X0, Y0, Z0):** Locomotive center chassis at Level L1 (midpoint of locomotive wheelbase).
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0.
* **Y (Elevation):** Wheel hubs / belly frame is Level L1, advancing upward to Cab/Hopper Roof at Level L4.
* **Z (Length / Longitudinal):** Stern discharge bumper is negative (−), Bow push bumper is positive (+).

```
Spatial Alignment Triad:
       [+Y: Zenith / Consist Roof L4]
              │
              │   [+Z: Bow / Locomotive Push Bumper]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Final Car Discharge Port]
              │
       [−Y: Ground Contact / Level L0]
```

### 0.2 In-Game Paint Palette & Functional Color Coding

To eliminate builder confusion across a 105 m multi-unit train, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Locomotive chassis, hopper undercarriages, wheel well frames |
| **Slot 1** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Locomotive forward push bumper, final car rear dump bumper |
| **Slot 2** | Hazard Amber | `#E6A100` | Bright Amber | 2-Axis Drawbar Hitches (`CAT Hitch Yaw & Pitch`), jumper hoses |
| **Slot 3** | Freight Transport Orange | `#D35400` | Deep Orange | Modular Hopper Car outer hull plating, top funnel intake chutes |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Train Conveyor Spine, large-port flexible drawbar pipes |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Modular Large Cargo Container holds (`CAT Hold 1, 2, 3`) |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Locomotive Power Core (1× Small Reactor, 4× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Locomotive Armored Cab shell, driver console framing |
| **Slot 8** | Clean Armor White | `#ECF0F1` | Clean White | Consist unit numbers, roof warning stripes, cab beacon base |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Side running boards, grab irons, brake reservoir tanks |

---

### 0.3 Consist Profile Elevation & Unit Layout

```
Consist Layout (Looking Starboard, Total Length: 42 B / 105.0 m):
     Z: +25 (Bow)      +17      +15      +8       +6       −1       −3      −10 (Stern)
  L4    ┌───────────────┐        ┌────────┐        ┌────────┐        ┌────────┐
        │ LOCOMOTIVE    │        │ HOPPER │        │ HOPPER │        │ TENDER │
  L3    │ ARMOR CAB     │        │ CAR 1  │        │ CAR 2  │        │ CAR 3  │
  L2    ├───────────────┤ [HITCH]├────────┤ [HITCH]├────────┤ [HITCH]├────────┤
        │[BUMP][FRONT C]│ ═══╦═══│[HOLD 1]│ ═══╦═══│[HOLD 2]│ ═══╦═══│[HOLD 3]│[DUMP]
  L1    └──[WH 1]─[WH 2]┘    ║   └──[W][W]┘    ║   └──[W][W]┘    ║   └──[W][W]┘
        ◄── 7W×9L×4H B ─►    ▼   ◄5W×7L×4H►    ▼   ◄5W×7L×4H►    ▼   ◄5W×7L×4H►
                           Drawbar 1         Drawbar 2         Drawbar 3
```

---

### 0.4 Master Train Conveyor Flow Schematic

The train functions as a single continuous conveyor pipeline from the locomotive front push bumper through all three hopper holds to the final car discharge connector:

```
                   [MASTODON STERN BUMPER]
                              │
                              ▼
      [CAT Loco Front Conn] (Locomotive Push Bumper, L2, X0)
                              │
                              ▼ (Locomotive Underbody Spine)
      [DRAWBAR HITCH 1] (Flexible Large Conveyor Pass-Through)
                              │
                              ▼
      [CAT Car 1 Hold] (Large Cargo Container 3×3×3 B)
                              │
                              ▼
      [DRAWBAR HITCH 2] (Flexible Large Conveyor Pass-Through)
                              │
                              ▼
      [CAT Car 2 Hold] (Large Cargo Container 3×3×3 B)
                              │
                              ▼
      [DRAWBAR HITCH 3] (Flexible Large Conveyor Pass-Through)
                              │
                              ▼
      [CAT Car 3 Hold] (Large Cargo Container 3×3×3 B)
                              │
                              ▼ (Stern Car Drain Line)
      [CAT Train Sorter] (Conveyor Sorter · Drain All = ON)
                              │
                              ▼
      [CAT Train Unload Conn] (Final Car Rear Bumper Connector, L2)
                              │
                              ▼ (Dumps 2,500 t ore in < 90 s)
                   [GROUND BASE RECEIVING HOPPER]
```

---

### 0.5 Master Electrical Single-Line Schematic

```
               LOCOMOTIVE POWER PLANT (1× Small Reactor + 4× Large Batteries)
                    15 MW Nuclear · 12 MWh Battery · 48 MW Max Discharge
                                      │
                                      ▼
                             TRAIN-WIDE POWER BUS
        ┌─────────────────────────────┼─────────────────────────────┐
        │                             │                             │
        ▼                             ▼                             ▼
  LOCOMOTIVE DRIVE              HOPPER CAR DRIVE MOTORS       AUTOMATION & BRAKES
  4× 5×5 Heavy Wheels           12× 5×5 Wheel Drive Motors    Synchronized Air Brakes
  (8.0 MW Max Hill Climb)       (24.0 MW Distributed Traction) C2 Comms Relay (0.1 MW)
  [Cruising Load: 2.2 MW]       [100% Shared Torque Assist]   Unload Sorter Pumps (0.5 MW)
```

---

### 0.6 Master Pre-Flight Bill of Materials (BOM)

| Consist Unit | Dimensions | Block Count | Dry Mass | PCU | Bulk Capacity |
|---|---|---|---|---|---|
| **Tractor Locomotive** | 7 W × 9 L × 4 H B | 142 | 146.5 t | 1,220 | Power, Cab & Steering |
| **Drawbar Hitch (ea ×3)**| 1 W × 2 L × 1 H B | 6 (18 total) | 8.2 t (24.6 t) | 200 (600) | 2-Axis Conveyor Link |
| **Hopper Car (ea ×3)** | 5 W × 7 L × 4 H B | 78 (234 total)| 64.0 t (192.0 t)| 650 (1,950)| 1× Large Cargo (833 t ore) |
| **COMPLETE 3-CAR TRAIN**| Total Length: 42 B | **394** | **≈ 363.1 t** | **3,770** | **≈ 2,500 t Bulk Ore** |

---

## Phase 1 · Tractor Locomotive Construction (Levels L1-L4)

```
       [PHASE 1 SPATIAL LOCATOR HUD]
       Unit: Tractor Locomotive (CAT-LOCO)
       Envelope: 7 W × 9 L × 4 H B (17.5 × 22.5 × 10.0 m)
       Coordinate Range: X −3 to +3, Z +17 to +25, Y = L1 to L4
       Key Datums:
       · Front Bumper Face: Z = +25, Level L1-L2
       · Rear Hitch Bulkhead: Z = +17, Level L1-L2
       · Front Steering Axle: Z = +23 (X = ±3)
       · Rear Drive Axle: Z = +19 (X = ±3)
       · Engineer Cab Center: [X0, Z +22, Y = L3]
```

### Step 11.1 · Locomotive Heavy Chassis & 4-Wheel Powertrain (L1-L2)

Build the 7 W × 9 L heavy-armor box chassis spanning Z +17 to +25. Mount 4× 5×5 heavy suspension wheels outward at X = ±3.

```
Plan view, Y = L1 (Locomotive Wheel Placement & Steering)
        X: −3          0          +3
    Z: +23 [WHEEL 1 (P)] ═══════ [WHEEL 2 (S)]   ◄── Front Steering Axle (±32.0°)
           ║                     ║
    Z: +21 ║   POWER VAULT CORE  ║
           ║                     ║
    Z: +19 [WHEEL 3 (P)] ═══════ [WHEEL 4 (S)]   ◄── Rear Fixed Axle (0.0° · Locked)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.1 PARTS TRAY & SUSPENSION TUNING GUIDE                                       │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Axle Settings & Parameters               │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 42  │ 126.0 t / 42  │ Palette Slot 0 (Slate); perimeter box    │
│ Wheel 5×5 Susp (L)  │ 2   │  16.0 t / 200 │ Port side (X = −3), face outward (−X)    │
│ Wheel 5×5 Susp (R)  │ 2   │  16.0 t / 200 │ Stbd side (X = +3), face outward (+X)    │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings for `CAT Loco Wheels`:**
* Front Axle (`WH 1, 2`): Steering = `ON` (Angle = `32.0°`), Propulsion = `ON` (Torque = `100%`), Friction = `70%`, Strength = `65%`
* Rear Axle (`WH 3, 4`): Steering = `OFF` (Locked 0°), Propulsion = `ON` (Torque = `100%`), Friction = `75%`, Strength = `70%`
* Speed Limit: `70 km/h`

---

### Step 11.2 · Armored Engineer Cab & Power Plant (L2-L4)

Install the nuclear power generator, battery bank, and elevated operator cab. The power core delivers 15 MW continuous baseline with 48 MW battery surge capacity to feed all trailing hopper cars.

```
Elevation, looking Port at Locomotive (X = 0):
       L4  ┌───────────────────┐  Roof Deck (Beacon & Antenna)
           │ [INDUSTRIAL CAB]  │  (Palette Slot 7: Navy Blue)
       L3  ├───────────────────┤
           │ [RCT] [BAT 1..4]  │  ◄── Power Vault (Palette Slot 6: Red)
       L2  ├───────────────────┤
           │ [CONVEYOR SPINE]  │  ◄── Level L2 Conveyor Pass-Through
       L1  ═════[WH 3]═══[WH 1]═  Ground Contact Baseline
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.2 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Output                     │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Industrial Cockpit  │ 1   │   3.5 t / 15  │ `CAT Loco Helm` (Centered X0, Z +22, L3) │
│ Small Reactor       │ 1   │   1.6 t / 25  │ `CAT Loco Reactor` (15 MW baseline power)│
│ Large Battery       │ 4   │  20.0 t / 80  │ `CAT Loco Battery 1..4` (12 MWh, 48 MW)† │
│ Radio Antenna (Lrg) │ 1   │   8.0 t / 100 │ `CAT Loco Antenna` (Range: 25,000 m)     │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

---

### Step 11.3 · Bow Push Bumper & Mastodon Docking Port (L1-L2)

Construct the reinforced forward push-bumper and large receiving connector. This allows the train to mate directly with the Mastodon stern bumper to take on bulk ore in the field.

```
Front elevation, looking Stern at Bow Bumper (Z = +25)
        X: −2     0    +2
    L3     ┌─────────────┐  ◄── Armored Cab Panoramic Viewport
    L2     ■──[CAT FRONT]─■  ◄── Large Connector (`CAT Loco Front Conn`)
    L1     ■═════════════■  ◄── Heavy Push Bumper (Palette Slot 1: Crimson)
```

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 11.1A: LOCOMOTIVE SPINE & POWER INTEGRITY**
> Before enclosing the locomotive side armor:
> 1. Verify uninterrupted large-conveyor flow from `CAT Loco Front Conn` (Z +25) straight through to the rear hitch bulkhead (Z +17).
> 2. Ensure reactor automated uranium pull is operational.
> 3. Verify driver cockpit controls: Handbrake, Group `CAT Train Wheels`, Group `CAT Hitches`, Connector lock/unlock.

---

#### Visual Milestone 11-1 · Tractor Locomotive Complete

```
Visual Checkpoint Silhouette (Looking Port Side at Locomotive):
                       [ARMORED CAB L3]
                              │
      [PUSH BUMPER L1-L2]     │             [REAR HITCH BULKHEAD]
             \                │                     /
     ═════════╧═══════════════╧═════════════════════╧═════════
                 7 W × 9 L × 4 H TRACTOR LOCOMOTIVE (L1)
```

* [ ] Locomotive empty mass verified: `≈ 146.5 t`.
* [ ] Front wheels steer smoothly through ±32° without chassis binding.
* [ ] Power plant delivers 63 MW combined surge power to the main bus.

---

## Phase 2 · 2-Axis Articulated Conveyor Drawbar Hitch (Levels L1-L2)

```
       [PHASE 2 SPATIAL LOCATOR HUD]
       Unit: 2-Axis Drawbar Articulated Hitch (CAT-HITCH)
       Envelope: 1 W × 2 L × 1 H B (2.5 × 5.0 × 2.5 m)
       Coordinate Range: X0, Z spans 2 B between vehicle bulkheads, Y = Level L2
       Kinematic Axes:
       · Yaw Hinge (Vertical Pivot): Mounted to leading car rear bulkhead
       · Pitch Hinge (Horizontal Pivot): Mounted to trailing car forward bulkhead
       · Conveyor Tube: Flexible curved large pipes bridging the joint
```

### Step 11.4 · 2-Axis Motorized Conveyor Drawbar Assembly

Install the dual motorized hinge mechanism connecting the locomotive to Hopper Car 1, and each hopper car to the next. The joint provides complete 2-axis articulation (horizontal steering yaw and vertical terrain cresting pitch) while carrying an airtight large-port conveyor line.

```
2.5D Isometric Callout · 2-Axis Conveyor Drawbar Hitch Kinematics

  Leading Unit Rear Bulkhead (Level L2)
            ║
            ╠══[CAT Hitch Yaw] (Motorized Hinge · Vertical Axis: −45° to +45°)
            ║         │
            ║         ▼
            ║    [Drawbar Link 1 B] (Palette Slot 2: Hazard Amber)
            ║         │
            ║         ▼
            ║   [CAT Hitch Pitch] (Motorized Hinge · Horizontal Axis: −25° to +25°)
            ║         │
            ║         ▼ (Flexible Large Curved Conveyor Tube Pass-Through)
            ╚═════════╪═════════════════════════════════════════════════════════════
                      ▼
        Trailing Car Forward Bulkhead (Level L2)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.4 PARTS TRAY & HITCH RIGGING GUIDE                                           │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Critical Joint Settings                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Motor Hinge (Large) │ 2   │   8.0 t / 200 │ Share Inertia Tensor = ON · Torque: 30 MN│
│ Curved Conveyor Pipe│ 2   │   1.6 t / 20  │ Large-port flexible piping through hitch │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

#### Joint Settings for `CAT Hitch Yaw` & `CAT Hitch Pitch`:
* **Share Inertia Tensor = `ON`** (MANDATORY: eliminates trailer sway, phantom physics wobble, and violent oscillation across subgrids).
* **Yaw Hinge (`CAT Hitch Yaw`):**
  * Axis: Vertical (Yaw / Azimuth).
  * Limits: Min = `−45.0°`, Max = `+45.0°` (enables tight 90° intersection turns).
  * Braking Torque: `30 MN·m`†.
* **Pitch Hinge (`CAT Hitch Pitch`):**
  * Axis: Horizontal (Pitch / Elevation).
  * Limits: Min = `−25.0°` (hill trough flex), Max = `+25.0°` (hill crest flex).
  * Braking Torque: `30 MN·m`†.

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 11.2A: DRAWBAR JOINT ALIGNMENT**
> 1. Set `Share Inertia Tensor = ON` on BOTH hinges before connecting trailing car power.
> 2. Verify large conveyor pipe passes freely through the hinge body with zero port misalignment.
> 3. Perform manual flex test: pivot yaw through ±45° and pitch through ±25°; ensure zero block collision between vehicle bulkheads.

---

#### Visual Milestone 11-2 · 2-Axis Drawbar Hitch Rigged

```
Visual Checkpoint Silhouette (Top-Down View of Articulated Joint):
       [LEADING UNIT] ═══[YAW HINGE]──[LINK]──[PITCH HINGE]═══ [TRAILING CAR]
                                 \              /
                          Articulated Conveyor Jumper Line
```

* [ ] Drawbar allows smooth horizontal pivoting and vertical flex under load.
* [ ] Zero phantom physics torque observed while parked on slopes.
* [ ] Airtight conveyor vacuum maintained through the drawbar link.

---

## Phase 3 · Modular Hopper Car Assembly (Per Unit) (Levels L1-L4)

```
       [PHASE 3 SPATIAL LOCATOR HUD]
       Unit: Modular Hopper Car (CAT-CAR)
       Envelope: 5 W × 7 L × 4 H B (12.5 × 17.5 × 10.0 m)
       Coordinate Range: X −2 to +2, Z spans 7 B per car, Y = L1 to L4
       Hold Datum:
       · Large Cargo Container (3×3×3 B): Centered at X0, Y = L2-L4
       · Axle 1: Front of car (X = ±2)
       · Axle 2: Rear of car (X = ±2)
```

### Step 11.5 · Hopper Car Chassis & 4-Wheel Traction Assist (L1-L2)

Build a 5 W × 7 L heavy-armor chassis. Install 4× 5×5 suspension wheels outward at X = ±2. Configure all hopper wheels for distributed traction drive: steering is locked at 0° while drive motors provide 100% torque assist to prevent dragging dead weight.

```
Plan view, Y = L1 (Hopper Car Wheel Layout)
        X: −2          0          +2
    Z: +3  [WHEEL P1] ═══════════ [WHEEL S1]   ◄── Front Fixed Axle (0.0° · Propulsion ON)
           ║                     ║
        0  ║   CARGO HOLD BAY    ║
           ║                     ║
    Z: −3  [WHEEL P2] ═══════════ [WHEEL S2]   ◄── Rear Fixed Axle (0.0° · Propulsion ON)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.5 PARTS TRAY & WHEEL CONFIGURATION                                           │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Role & Placement                  │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Heavy Armor Block   │ 32  │  96.0 t / 32  │ Palette Slot 0 (Slate); hopper frame box │
│ Wheel 5×5 Susp (L/R)│ 4   │  32.0 t / 400 │ 2 pairs (Axles grouped at Z ±2, X = ±2)  │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

**Settings for `CAT Car Wheels`:**
* Steering = `OFF` (All hopper wheels track fixed straight ahead).
* Propulsion = `ON` (Distributes driving torque across all consist wheels, 100% power assist).
* Friction = `75%` · Strength = `70%` · Speed Limit = `70 km/h`.

---

### Step 11.6 · Large Cargo Hold & Top Funnel Loading Chutes (L2-L4)

Mount 1 standard Large Cargo Container (3×3×3 B) centered in the car chassis. Frame the top intake port with angled heavy armor slopes to create an open funnel hopper chute for top-loading from overhead conveyor chutes.

```
Plan view, Y = L2 (Hopper Car Midline)
        X: −2     0    +2
    Z: +3  ■═════╤═════■  ◄── Drawbar Hitch Inbound (Level L2)
           │ ╲   │   ╱ │
        0  │  LARGE    │  ◄── Large Cargo Container (3×3×3 B · Hold)
           │  CARGO    │      (Palette Slot 5: Logistics Emerald)
           │ ╱   │   ╲ │      Top: Funnel Hopper Chutes (Palette Slot 3: Orange)
    Z: −3  ■═════╧═════■  ◄── Drawbar Hitch Outbound (Level L2)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.6 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ System Capacity & Mounting               │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Large Cargo (3×3×3) │ 1   │  28.5 t / 250 │ `CAT Hold` (1,386,000 L @ 3× Inventory)  │
│ Conveyor Junction   │ 2   │   2.6 t / 26  │ Front and rear drawbar tie junctions     │
│ Heavy Armor Slopes  │ 24  │  36.0 t / 24  │ Palette Slot 3 (Orange); funnel chutes   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!NOTE]
> **PRE-ENCLOSURE INSPECTION GATE 11.3A: HOPPER CONVEYOR FLOW & TRACTION TEST**
> 1. Check hold capacity: verify each car adds `1,386,000 L` cargo volume (total 3-car hold = 4,158,000 L).
> 2. Ensure hopper wheel drive motors engage simultaneously with the locomotive throttle.
> 3. Verify top funnel intake connects directly to the container's upward large port.

---

#### Visual Milestone 11-3 · Modular Hopper Car Complete

```
Visual Checkpoint Silhouette (Looking Port at Single Hopper Car):
                       [TOP FUNNEL CHUTE L4]
                                 │
                       [LARGE CARGO HOLD L2-L3]
                                 │
     ═══════[WH P2]══════════════╧══════════════[WH P1]═══════
                 5 W × 7 L × 4 H MODULAR HOPPER CAR UNIT
```

* [ ] Car dry mass verified: `≈ 64.0 t`.
* [ ] All 4 wheels provide active propulsion assist under throttle command.
* [ ] Conveyor junctions provide airtight pass-through along the central axis.

---

## Phase 4 · High-Speed Terminal Discharge & Stern Bumper (Levels L1-L2)

```
       [PHASE 4 SPATIAL LOCATOR HUD]
       Unit: Stern Dump Interface (CAT-UNLOAD)
       Location: Aft face of Hopper Car 3 (Z = −10, Level L2, X0)
       Interface Height: 2.5 m above ground (matches Ground Base Hopper Connector)
```

### Step 11.7 · Rear Train Discharge Connector & Sorter Pump (L1-L2)

Equip the rear bumper of the final hopper car (Car 3) with a high-capacity discharge connector and dedicated conveyor sorter pump.

```
Rear elevation, looking Bow at Final Car Bumper (Z = −10)
        X: −2     0    +2
    L3     ■═════╤═════■  ◄── Rear Collision Bumper (Palette Slot 1: Crimson)
    L2     │ [UNLOAD]  │  ◄── Large Connector (`CAT Train Unload Conn`)
    L1     ■═════╧═════■
           ◄── 1.25 m Ground Clearance ──►
```

```
2.5D Isometric Callout · Train Docking to Ground Base Receiving Hopper

   Final Hopper Car Bumper (Z −10)
                 │
                 ▼
     [CAT Train Unload Conn] (Connector Facing Stern −Z)
                 │
                 ▼ (Reversing into Terminal Dock)
     [GND CAT Dump Conn] (Ground Base Receiving Connector, Facing North +Z)
                 │
   ══════════════╪══════════════════════════════════════════════════════════
     [COLL 1] [COLL 2] [COLL 3] [COLL 4] [COLL 5] (5× In-Deck Collectors)
                 │
                 ▼ (Drains 2,500 t Bulk Ore in < 90 Seconds)
         Ground Base Industrial Refining Loop (Chapter 4)
```

┌──────────────────────────────────────────────────────────────────────────────────────┐
│ STEP 11.7 PARTS TRAY                                                                 │
├─────────────────────┬─────┬───────────────┬──────────────────────────────────────────┤
│ Block Type          │ Qty │ Mass / PCU    │ Role & Operating Configuration           │
├─────────────────────┼─────┼───────────────┼──────────────────────────────────────────┤
│ Connector (Large)   │ 1   │   3.1 t / 150 │ `CAT Train Unload Conn` (Facing Stern −Z)│
│ Conveyor Sorter     │ 1   │   1.2 t / 40  │ `CAT Train Sorter` (Drain All = ON)      │
│ Heavy Armor Slopes  │ 8   │  12.0 t / 8   │ Palette Slot 1 (Crimson); push bumpers   │
└─────────────────────┴─────┴───────────────┴──────────────────────────────────────────┘

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 11.4A: DISCHARGE DRAIN VERIFICATION**
> 1. Set `CAT Train Sorter` whitelist to: `Iron`, `Nickel`, `Silicon`, `Cobalt`, and `Stone`.
> 2. Verify `CAT Train Unload Conn` centerline height is exactly at Level L2 (2.5 m above tarmac).
> 3. Verify sorter drain pulls from all 3 cars simultaneously when connector is locked.

---

#### Visual Milestone 11-4 · Complete 3-Car Land Train Consist Commissioned

```
Visual Checkpoint Silhouette (Looking Port at Complete 42 B Consist):
     [LOCOMOTIVE] ══[HITCH 1]══ [CAR 1] ══[HITCH 2]══ [CAR 2] ══[HITCH 3]══ [CAR 3/DUMP]
       4× Wheels                  4× Wheels              4× Wheels              4× Wheels
     ═════════════════════════════════════════════════════════════════════════════════════
                 CATERPILLAR-01 42 B LAND TRAIN (16 WHEELS TOTAL)
```

* [ ] Total consist dry mass verified: `≈ 363.1 t`.
* [ ] All 16 suspension wheels share synchronized drive torque across 4 subgrids.
* [ ] 3-car consist tracks through S-curves without trailer collision or jackknifing.

---

## Phase 5 · Master Control Logic & Hardware Scripting Contract (Claude Code)

### Step 11.8 · Terminal Namespace & Group Taxonomy

#### 1. Group Taxonomy
* `CAT Train Wheels` : All 16 wheel units across locomotive and 3 hopper cars.
* `CAT Train Brakes` : Synchronized handbrake and parking brake group.
* `CAT Hitches` : All 6 motorized hitch hinges linking the consist.
* `CAT Holds` : All 3 Large Cargo Containers across the consist.
* `CAT Loco Power` : 1× Small Nuclear Reactor + 4× Large Batteries.

#### 2. Event Controller Matrix
| Controller | Monitored Target | Condition / Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `CAT EC Dump` | `CAT Train Unload Conn` | Connector: Connected | `CAT Train Sorter` Drain ON | Sorter OFF |
| `CAT EC AntiJackknife`| Vehicle Velocity | Velocity $\ge 45\text{ km/h}$ | `CAT Hitches` Damping $\rightarrow 85\%$ (Anti-sway) | Damping $\rightarrow 20\%$ (Flex) |
| `CAT EC Traction` | Consist Cargo Mass | Total Mass $\ge 1,500\text{ t}$ | `CAT Car Wheels` Torque $\rightarrow 100\%$ | Torque $\rightarrow 60\%$ |

#### 3. IGC Channel Subscriptions (Ground Fleet Bus)
* Outbound: `TMS.CATER.STATUS`: `train_id;ore_mass_t;car_count;speed_kmh;route_status`
  * Example: `CAT-01;2480.0;3;52.4;EN_ROUTE_BASE`
* Inbound: `TMS.BASE.RECEIVING`: signals Ground Base terminal hopper is clear to receive.
* Inbound: `TMS.MAST.TRANSFER`: signals Mastodon stern transfer is active.

---

## Phase 6 · Commissioning Checklist & Acceptance Trials (RIG Test)

### A · Articulation & Maneuvering Trials
* [ ] **S-Curve Clearance:** Drive the 42 B consist through an S-curve at 30 km/h; verify zero wheel binding or chassis contact between cars.
* [ ] **Hill-Climb Under Full Load:** Fill all 3 hoppers with 2,500 t stone; climb a 20° voxel incline; confirm all 16 wheels share propulsion torque without stalling or spinning out.

### B · High-Speed Stability & Anti-Jackknife
* [ ] **70 km/h Highway Run:** Drive along a flat surface at 70 km/h; confirm `CAT EC AntiJackknife` engages 85% hinge damping, preventing trailer sway or whip.
* [ ] **Emergency Brake Stop:** Apply full brakes from 60 km/h; confirm all trailers brake in unison without shoving the locomotive sideways.

### C · Fast-Dump Terminal Validation
* [ ] **Field Loading at Mastodon:** Back locomotive push bumper into Mastodon stern connector; verify 2,500 t bulk ore fills all 3 hopper cars.
* [ ] **Terminal Discharge at Base:** Reverse consist into Ground Base receiving dock; verify `CAT Train Unload Conn` locks and empties 2,500 t of ore in $< 90\text{ seconds}$.
