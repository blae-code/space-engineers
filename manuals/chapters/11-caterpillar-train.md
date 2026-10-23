# Caterpillar · Visual Build Instructions

## Chapter 11 · CATERPILLAR-01 Land Train (CAT-01)

Lego-style, step-by-step visual build manual and hardware-to-scripting interface contract for the **CATERPILLAR-01 (CAT-01)** Heavy Wheeled Bulk Land Train.
Each step features: an exact ASCII schematic, Spatial Locator HUD, in-game paint palette assignments, pre-step parts tray, critical joint/conveyor settings, Pre-Enclosure Inspection Gates ("Points of No Return"), and Phase-End Visual Milestones.

### Design Directives & Operational Scope (Decisions 10, 25, 35, 41, 51, 52, 60, 66, 67 & 70)
* **Role:** High-volume terrestrial bulk ore and aggregate hauler operating on designated planetary surface corridors between the **MASTODON-01 MKI** heavy excavator and the **Ground Base Refinery Complex (GND-01)**. Serves as the high-tonnage Ground Tier transport backbone of the dual-tier logistics architecture (Decision 52), complementing the agile Air Tier MULE-01 freight shuttle.
* **Architecture:** 4-Unit Articulated Consist (42 B / 105.0 m total length):
  1. **Tractor Locomotive (`CAT-01-L`): 7 W × 9 L × 4 H B (4 Wheels · Axles 1 & 2):** Heavy tractive engine, forward-sloped armored panoramic single-operator engineer cab (`CAT Loco Helm`), 15 MW nuclear reactor vault + 12 MWh battery buffer across 4× Large Batteries (48 MW surge discharge), 25 km radio antenna mast, forward heavy push-bumper with large receiving connector (`CAT Loco Front Conn`) matching Mastodon's stern dump port, and aft 3-axis heavy motorized drawbar hitch.
  2. **Hopper Car 1 (`CAT-01-C1`): 5 W × 7 L × 4 H B (4 Wheels · Axles 3 & 4):** Modular bulk hopper, 4-wheel distributed power unit (DPU) smart traction assist, 1× Large Cargo Container (`CAT Hold 1`, 3×3×3 B, 1,386,000 L @ 3× Inventory), top-loading funnel chute, side inspection ports, and inter-car gangway plates.
  3. **Hopper Car 2 (`CAT-01-C2`): 5 W × 7 L × 4 H B (4 Wheels · Axles 5 & 6):** Identical modular bulk hopper unit (`CAT Hold 2`), mid-consist conveyor repeater line, DPU smart traction assist, and dual 3-axis drawbar joints.
  4. **Hopper Car 3 / Tender (`CAT-01-C3`): 5 W × 9 L × 4 H B (4 Wheels · Axles 7 & 8):** Final hopper unit (`CAT Hold 3`), stern high-speed terminal discharge manifold with pressurized Conveyor Sorter (`CAT Train Sorter`, Drain All = ON), stern large connector (`CAT Train Unload Conn`), and heavy collision bumper with alignment guide horns matching Ground Base Bulk Receiving Dock.
* **Grid:** Large Grid (1 B = 2.5 m) · **Consist Envelope:** 7 W × 42 L × 4 H B (17.5 × 105.0 × 10.0 m).
* **Mass:** Consist Dry Mass `≈ 368.5 t` (Locomotive: 148.2 t · Drawbars: 24.6 t · 3× Hoppers: 195.7 t) · Max Laden Mass `≈ 2,868.5 t` (Earth 1 g).
* **Payload Capacity:** 2,500 t bulk ore across 3× Large Cargo Container holds (4,158,000 L combined volume @ 3× Inventory).
* **Mechanical & Powertrain Suite (Decisions 10, 35 & 67):** 16× 5×5 Off-Road Suspension Wheels across 8 axles. Front steering axle on locomotive (±32.0°); all 14 trailing wheels track straight with 100% synchronized propulsion torque. 3× 3-Axis Motorized Conveyor Drawbars (Yaw Rotor ±45°, Pitch Hinge ±25°, Roll Rotor ±20°) with `Share Inertia Tensor = ON` and magnetic anti-sway clamps (`CAT Hitch Clamp Port/Stbd`) active at speed > 40 km/h.
* **Fast-Dump Terminal Performance (Decision 51):** High-speed pressurized discharge manifold empties 2,500 t of bulk ore into Ground Base in `< 90 seconds` via stern connector and in-deck collectors.
* **Operating Environment:** Earth 1 g baseline, surface roadbed, quarry benches, and cross-country haulage corridors.

> [!NOTE]
> Dimensions, mass, BOM and placements are provisional design intent, not a validated blueprint. The 42-block envelope governs: Z −16..+25 contains the 32 blocks of vehicle bodies, six blocks of drawbar joints, and a four-block stern terminal extension at Z −16..−13. The tender body occupies Z −12..−4. Verify the consist geometry, terminal alignment and all design figures against a trusted blueprint or in-game build.

---

## 0 · Reference Grid, In-Game Paint Palette & Master Consist Layout

### 0.1 Coordinate Triad & Axis Conventions
* **Origin (X0, Y0, Z0):** Midpoint of the Tractor Locomotive wheelbase at Level L1, along centerline X0.
* **X (Beam / Transverse):** Port is negative (−), Starboard is positive (+), Centerline is 0. Spans X −3.5 to +3.5 on Locomotive (7 B wide = 17.5 m) and X −2.5 to +2.5 on Hopper Cars (5 B wide = 12.5 m).
* **Y (Elevation):** Wheel contact patch is Level L0 (Y = −1.25 m); wheel hubs and chassis box girders are Level L1 (Y = 0); cab deck and hopper midline are Level L2; hopper holds are Level L3; cab/hopper roof is Level L4 (10.0 m total height).
* **Z (Length / Longitudinal):** Stern discharge bumper is negative (−), Bow push-bumper is positive (+). Spans Z −16 to +25 (42 blocks total = 105.0 m).
  * Tractor Locomotive (`CAT-01-L`): Z +17 to +25 (9 blocks length = 22.5 m). Wheelbase datum Z0 is at Z +21.
  * Drawbar Hitch 1: Z +15 to +16 (2 blocks mechanical span = 5.0 m).
  * Hopper Car 1 (`CAT-01-C1`): Z +8 to +14 (7 blocks length = 17.5 m).
  * Drawbar Hitch 2: Z +6 to +7 (2 blocks mechanical span = 5.0 m).
  * Hopper Car 2 (`CAT-01-C2`): Z −1 to +5 (7 blocks length = 17.5 m).
  * Drawbar Hitch 3: Z −3 to −2 (2 blocks mechanical span = 5.0 m).
  * Hopper Car 3 / Tender (`CAT-01-C3`): Z −12 to −4 (9 blocks length = 22.5 m).
  * Stern terminal extension: Z −16 to −13 (4 blocks, including the sorter, unload connector, and collision bumper).

```
Spatial Alignment Triad:
       [+Y: Zenith / Consist Roof L4]
              │
              │   [+Z: Bow / Locomotive Push-Bumper]
              │   /
              │  /
              │ /
  [−X: Port] ─┼───────── [+X: Starboard]
             /│
            / │
           /  │
  [−Z: Stern / Tender Discharge Bumper]
              │
       [−Y: Ground Contact / Level L0]
```

---

### 0.2 In-Game Paint Palette & Functional Color Coding

To eliminate builder confusion across a 105 m multi-unit consist, all structural armor and functional blocks MUST be painted during assembly using standard Space Engineers palette slots:

| Palette Slot | Color Name | Hex Code | Visual Swatch | Functional Zone / System Component |
|---|---|---|---|---|
| **Slot 0** | Industrial Dark Gray | `#2B303A` | Dark Slate | Locomotive chassis, hopper undercarriages, wheel well frames, belly skids |
| **Slot 1** | Heavy Rail Crimson | `#9E2A2B` | Deep Crimson | Locomotive bow push-bumper, final car stern dump bumper, crash horns |
| **Slot 2** | Hazard Amber | `#E6A100` | Bright Amber | 3-Axis Drawbar Hitches (`CAT Hitch Yaw/Pitch/Roll`), jumper hoses, brake skids |
| **Slot 3** | Freight Transport Orange | `#D35400` | Deep Orange | Modular Hopper Car outer hull plating, top funnel intake chutes |
| **Slot 4** | Processing Teal | `#1ABC9C` | Bright Teal | Central Train Conveyor Spine, large-port flexible drawbar pipes, sorters |
| **Slot 5** | Logistics Emerald | `#27AE60` | Rich Green | Modular Large Cargo Container holds (`CAT Hold 1, 2, 3`) |
| **Slot 6** | High-Voltage Alert Red | `#C0392B` | Alert Red | Locomotive Power Vault (1× Small Reactor, 4× Large Batteries) |
| **Slot 7** | Fleet Command Navy | `#1B263B` | Deep Navy | Locomotive Armored Cab shell, driver console framing, beacon mounts |
| **Slot 8** | Clean Armor White | `#ECF0F1` | Clean White | Consist unit numbers (`LOCO-01`, `CAR-01..03`), roof warning stripes |
| **Slot 9** | High-Viz Safety Yellow | `#F1C40F` | Canary Yellow | Side running boards, grab irons, maintenance ladders, brake reservoirs |

---

### 0.2B Exterior Livery & Two-Tone Color Blocking Map

To achieve a cohesive industrial locomotive aesthetic matching the Mastodon MKI and Ground Base, paint must follow strict functional zone lines across the consist:

```
[EXTERIOR ELEVATION LIVERY MAP · STARBOARD PROFILE (LOOKING PORT)]
Level
L4  │         [SLOT 7: NAVY CAB]               [SLOT 3: ORANGE TOP FUNNELS & CATWALKS (CAR 1-3)]
L3  │ ═══════[SLOT 7: NAVY CAB ROOF]════════════[SLOT 3: ORANGE HOPPER CAR ARMOR CASING]══════════
L2  │ ───────────────────[SLOT 1: CRIMSON BELTLINE STRIPE (Z +25 to Z −16)]──────────────────────
L1  │ ═══════[SLOT 0: INDUSTRIAL DARK GRAY CHASSIS BOX GIRDERS & WHEEL MOUNTS]════════════════════
L0  │    (O)=======(O)       [SLOT 2: DRAWBAR]      (O)=======(O)       [SLOT 2]      (O)=======(O)
        Axle 1    Axle 2                           Axle 3    Axle 4                  Axle 5    Axle 6
        ◄── LOCOMOTIVE ──►                         ◄── HOPPER 1 ──►                  ◄── HOPPER 2 ──►
```

| Zone / Component | Primary Palette Slot | Hex Code | Visual Application & Boundaries |
|---|---|---|---|
| **Chassis Box Girders** | Slot 0 (Industrial Dark Gray) | `#2B303A` | Level L1 structural sills, axle mounts, belly deflector plates across all 4 units |
| **Beltline Rail Stripe** | Slot 1 (Heavy Rail Crimson) | `#9E2A2B` | Level L2 continuous 1-block stripe running along flanks from bow bumper to stern dump |
| **Armored Driver Cab** | Slot 7 (Fleet Command Navy) | `#1B263B` | Levels L3-L4 forward cab shell, brow visor, roof crown, and rear cab bulkhead |
| **Hopper Car Envelopes** | Slot 3 (Freight Orange) | `#D35400` | Levels L2-L4 hopper hull side plates, top funnel chamfers, and corner transitions |
| **3-Axis Drawbar Hitches**| Slot 2 (Hazard Amber) | `#E6A100` | Levels L1-L2 articulated hinges, drawbar tongues, and magnetic clamp arms |
| **Crash & Alignment Bumpers**| Slot 1 (Heavy Rail Crimson) | `#9E2A2B` | Bow push-bumper (Z +25) and stern terminal dump bumper (Z −16) with yellow striping |

---

### 0.3 Master Consist Top Plan View (Levels L1-L4)

```
[MASTER CONSIST PLAN VIEW · COMPOSITE LEVELS L1-L4]
         Port (−X)                 Centerline (X0)               Starboard (+X)
Z: +25   ┌────────────────────────[CAT FRONT CONN]────────────────────────┐   ◄── Bow Push Bumper (L2)
         │ [CRIMSON BUMPER]        [CONVEYOR SPINE]      [CRIMSON BUMPER] │
Z: +23  (O) [WH 1: STEER P]       [POWER VAULT CORE]    (O) [WH 2: STEER S]   ◄── Axle 1 (±32° Steer)
Z: +22   │                         [CABIN: CAT HELM]                      │
Z: +21   │ [LARGE BATTERY 1,2]     [SMALL REACTOR]       [LARGE BATTERY 3,4]│  ◄── Wheelbase Datum Z0
Z: +19  (O) [WH 3: DRIVE P]       [CONVEYOR SPINE]      (O) [WH 4: DRIVE S]   ◄── Axle 2 (Fixed Drive)
Z: +17   └─────────────────────────[HITCH 1 BULKHEAD]────────────────────┘
Z: +15   ══════════════════════════[DRAWBAR HITCH 1]══════════════════════   ◄── 3-Axis Joint 1
Z: +14   ┌─────────────────────────[CAR 1 FRONT BULKHEAD]─────────────────┐
Z: +13  (O) [WH 5: DPU P]         [CONVEYOR RISER]      (O) [WH 6: DPU S]     ◄── Axle 3 (Hopper 1 Fwd)
Z: +11   │  ╲                     │  CAT HOLD 1  │                     ╱  │   ◄── Large Cargo (3×3×3 B)
Z: +12   │   TOP FUNNEL CHUTE     │ (1,386,000 L)│     TOP FUNNEL CHUTE   │       Top Funnel Intake (L4)
Z: +8   (O) [WH 7: DPU P]         │              │      (O) [WH 8: DPU S]     ◄── Axle 4 (Hopper 1 Aft)
Z: +8    └────────────────────────┴──────────────┴────────────────────────┘
Z: +7    ══════════════════════════[DRAWBAR HITCH 2]══════════════════════   ◄── 3-Axis Joint 2
Z: +5    ┌─────────────────────────[CAR 2 FRONT BULKHEAD]─────────────────┐
Z: +4   (O) [WH 9: DPU P]         [CONVEYOR RISER]      (O) [WH 10: DPU S]    ◄── Axle 5 (Hopper 2 Fwd)
Z: +3    │ ╲                      ┌──────────────┐                      ╱ │
Z: +2    │  ╲                     │  CAT HOLD 2  │                     ╱  │   ◄── Large Cargo (3×3×3 B)
Z: +1    │   TOP FUNNEL CHUTE     │ (1,386,000 L)│     TOP FUNNEL CHUTE   │
Z: −1   (O) [WH 11: DPU P]        │              │      (O) [WH 12: DPU S]    ◄── Axle 6 (Hopper 2 Aft)
Z: −2    ══════════════════════════[DRAWBAR HITCH 3]══════════════════════   ◄── 3-Axis Joint 3
Z: −3    ══════════════════════════════════════════════════════════════════
Z: −4    ┌─────────────────────────[CAR 3 FRONT BULKHEAD]─────────────────┐
Z: −6   (O) [WH 13: DPU P]        [CONVEYOR RISER]      (O) [WH 14: DPU S]    ◄── Axle 7 (Tender Fwd)
Z: −8    │  ╲                     │  CAT HOLD 3  │                     ╱  │   ◄── Large Cargo (3×3×3 B)
Z: −9    │   TOP FUNNEL CHUTE     │ (1,386,000 L)│     TOP FUNNEL CHUTE   │
Z: −10  (O) [WH 15: DPU P]        └──────┬───────┘      (O) [WH 16: DPU S]    ◄── Axle 8 (Tender Aft)
Z: −12   └────────────────────────┴──────┴───────────────────────────┘
Z: −13   │                    STERN TERMINAL EXTENSION                   │
Z: −15   │                        [CAT TRAIN SORTER]                      │   ◄── Drain All = ON
Z: −16   └────────────────────────[CAT TRAIN UNLOAD]──────────────────────┘   ◄── Stern Dump Bumper (L2)
```

---

### 0.4 Master Consist Profile Elevation (Looking Starboard, X0 Plane)

```
[MASTER CONSIST PROFILE ELEVATION · TOTAL LENGTH: 42 B / 105.0 m]
      Z: +25 (Bow)      +21      +17      +15      +11      +8      +6       +2      −1      −3       −8      −12    −16 (Stern)
Level
L4       ┌───────────────┐               ┌───────────────┐      ┌───────────────┐      ┌─────────────────────────┐
         │ INDUSTRIAL    │               │ TOP FUNNEL    │      │ TOP FUNNEL    │      │ TOP FUNNEL & CATWALK    │
L3       │ CABIN (NAVY)  │               │ CHUTE (CAR 1) │      │ CHUTE (CAR 2) │      │ CHUTE (CAR 3 / TENDER)  │
         ├───────────────┤   [HITCH 1]   ├───────────────┤[HITCH]├───────────────┤[HITCH]├────────────────┬────────┤
L2       │[BUMP][FRONT C]│═══[YAW/PITCH]═│  CAT HOLD 1   │══╦═══│  CAT HOLD 2   │══╦═══│  CAT HOLD 3    │[SORTER]│[DUMP]
         ├───────────────┤               ├───────────────┤  ║   ├───────────────┤  ║   ├────────────────┴────────┤
L1       └──[WH 1]─[WH 2]┘               └──[WH 3]─[WH 4]┘  ║   └──[WH 5]─[WH 6]┘  ║   └──[WH 7]───────[WH 8]────┘
L0    ──────(O)─────(O)─────────────────────(O)─────(O)─────▼──────(O)─────(O)─────▼──────(O)───────────(O)───────
         ◄── 7W×9L×4H B ─►               ◄── 5W×7L×4H B ─►      ◄── 5W×7L×4H B ─►      ◄────── 5W×9L×4H B ──────►
          LOCOMOTIVE (L)                  HOPPER CAR 1 (C1)      HOPPER CAR 2 (C2)       HOPPER CAR 3 / TENDER (C3)
```

---

### 0.4B Consist 3/4 Isometric Perspectives & Stance Wireframes

```
[3/4 ISOMETRIC WIREFRAME · TRACTOR LOCOMOTIVE BOW & CABIN]
                      +Y (Zenith L4)
                            ▲
                            │       ┌───────────────────────┐  ◄── Cab Brow Visor (L4, Z +23)
                            │      ╱                       ╱│
                            │     ┌───────────────────────┐ │
                            │     │   PANORAMIC CABIN     │ │  ◄── Navy Industrial Cab (L3)
                            │     │   [CAT LOCO HELM]     │ │
             -X (Port) ◄────┼────►+X (Starboard)          │ │
                           ╱│     ├───────────────────────┤ │  ◄── Heavy Push Bumper Sill (L2)
                          ╱ │     │[BUMP] [FRONT CONN]    │ │  ◄── Large Receiving Port (Z +25)
                         ╱  │     │                       │╱
                        ▼   │     └──(O)─────────────(O)──┘    ◄── Axles 1 & 2 (5×5 Wheels L1)
                   -Z (Stern)        Axle 1          Axle 2

[3/4 ISOMETRIC WIREFRAME · HOPPER CAR & 3-AXIS DRAWBAR JOINT]
                            ▲
                            │     ┌───[TOP FUNNEL CHUTE]───┐   ◄── 45° Inward Loading Chutes (L4)
                            │    ╱                        ╱│
                            │   ┌────────────────────────┐ │
                            │   │     CAT HOLD 1 (3×3)   │ │   ◄── Large Cargo (L2-L3)
                            │   │  (LOGISTICS EMERALD)   │ │
                            │   ├────────────────────────┤ │
  [LEADING CAR BULKHEAD]    │   │   CONVEYOR SPINE       │ │
            ║               │   └───(O)────────────(O)───┘ │   ◄── Axles 3 & 4 (5×5 Wheels L1)
            ╠══[YAW ROTOR]──┼────────┼──────────────┼──────┘
            ║       │       │        ▼              ▼
            ║  [PITCH HINGE]│      Axle 3         Axle 4
            ║       │       │
            ║  [ROLL ROTOR] ◄── 3-Axis Gimbal Drawbar Joint (Hazard Amber, Level L2)
```

---

### 0.4C Transverse Hull Station Profiles & Chamfer Specifications

To ensure exact block-by-block structural fidelity during survival printing, transverse cross-sections are specified at 5 critical consist stations:

```
Station 1 · Bow Push-Bumper (Z = +25, Looking Stern)
Level
L3  │               ┌─────────┐
    │               │ CAB EYE │   ◄── Industrial Cockpit Forward Viewport (X0)
L2  │   ┌───────────┴─────────┴───────────┐
    │   │[CRIMSON BUMPER] [FRONT C] [BUMP]│   ◄── Large Connector at X0, Blast Door Sills at X ±1 to ±3
L1  │   └─────┬─────────────────────┬─────┘
    │        (O) [5×5 SUSPENSION]  (O)        ◄── Flared Mudguards Over Axle 1 (X = ±3.5)
L0  │    ◄── 1.25 m Belly Skid Clearance ──►

Station 2 · Locomotive Mid-Section Power Core (Z = +21, Looking Stern)
Level
L4  │               ┌─────────┐
    │               │ CAB ROOF│   ◄── Heavy Armor Slopes Angled 45° to Beacon
L3  │   ┌───────────┼─────────┼───────────┐
    │   │ [BATTERY] │ [REACT] │ [BATTERY] │   ◄── 15 MW Small Reactor at X0; 4× Large Batteries at X ±1 to ±2
L2  │   ├───────────┴─────────┴───────────┤
    │   │ ═══════[CONVEYOR SPINE]════════ │   ◄── Large Conveyor Junction at X0, Beltline Crimson at X ±3
L1  │   └─────┬─────────────────────┬─────┘
    │        (O) [FIXED WHEELS]    (O)        ◄── Axle 2 (Fixed Straight-Line Drive)

Station 3 · 3-Axis Drawbar Articulation Joint (Z = +15, Looking Stern)
Level
L3  │          [FLEXIBLE CONVEYOR HOSE]       ◄── Large Curved Conveyor Bridge Loop
L2  │       ┌──[CAT HITCH PITCH HINGE]──┐
    │       │ ◄── YAW ADVANCED ROTOR ──►│     ◄── 2-Axis Gimbal Joint (Amber, X0, Level L2)
L1  │   ════╧═══════════════════════════╧════ ◄── Low-Profile Tension Drawbar Tongue
L0  │    ◄────── 2.5 m Ground Clearance ─────►

Station 4 · Hopper Car Cargo Body (Z = +11, Looking Stern)
Level
L4  │   ┌───╲                     ╱───┐       ◄── Heavy Armor Slopes Form 45° Top Funnel Chute
L3  │   │    ┌───────────────────┐    │
    │   │    │  CAT HOLD (3×3)   │    │       ◄── Large Cargo Container (3×3×3 B, Centered X0)
L2  │   │    │ (LOGISTICS GREEN) │    │       ◄── Orange Hull Shell (X = ±2) + Crimson Beltline
L1  │   └──┬─┴───────────────────┴─┬──┘
    │     (O) [DPU TRACTION WHEELS](O)        ◄── 5×5 Suspension Wheels (X = ±2.5)

Station 5 · Stern Unload Bumper & Dump Manifold (Z = −16, Looking Bow)
Level
L3  │   ┌─────────────────────────────┐
    │   │ REAR COLLISION BLUNT BUMPER │       ◄── Heavy Armor Blast Doors & Crimson Slopes
L2  │   │ [GUIDE] [UNLOAD CONN] [GUIDE│       ◄── Large Connector at X0, Funnel Alignment Wedges at X ±2
L1  │   └──────┬───────────────┬──────┘
    │         (O) [CAT SORTER] (O)            ◄── Conveyor Sorter Angled Forward to Unload Connector
L0  │    ◄── 1.25 m Tarmac Ground Line ──►
```

---

### 0.5 Master Conveyor Routing Schematic & Physical Maintenance Ports

The train functions as a single continuous conveyor pipeline from the locomotive front push-bumper through all three hopper holds to the final car discharge connector:

```
                   [MASTODON MKI STERN BUMPER]
                                │
                                ▼
        [CAT Loco Front Conn] (Locomotive Push-Bumper, L2, X0)
                                │
                                ▼ (Locomotive Underbody Spine)
        [CAT Large Conveyor Riser 1] (Level L1 to L2 Junction)
                                │
                                ▼
        [DRAWBAR HITCH 1] (Flexible Large Conveyor Loop across Yaw/Pitch Joint)
                                │
                                ▼
        [CAT Hold 1] (Hopper Car 1 · Large Cargo 3×3×3 B, L2-L3)
          │ ◄── Physical Manual Inspection Hatch (Port L2)
          ▼ (Underbody Inter-Car Riser)
        [DRAWBAR HITCH 2] (Flexible Large Conveyor Loop across Joint 2)
                                │
                                ▼
        [CAT Hold 2] (Hopper Car 2 · Large Cargo 3×3×3 B, L2-L3)
          │ ◄── Physical Manual Inspection Hatch (Port L2)
          ▼ (Underbody Inter-Car Riser)
        [DRAWBAR HITCH 3] (Flexible Large Conveyor Loop across Joint 3)
                                │
                                ▼
        [CAT Hold 3] (Hopper Car 3 / Tender · Large Cargo 3×3×3 B, L2-L3)
          │ ◄── Physical Manual Inspection Hatch (Port L2)
          ▼
        [CAT Train Sorter] (Conveyor Sorter · Drain All = ON · Whitelist Bulk)
                                │
                                ▼
        [CAT Train Unload Conn] (Final Car Rear Bumper Connector, L2, X0)
                                │
                                ▼ (Dumps 2,500 t ore in < 90 s)
                   [GROUND BASE RECEIVING DOCK]
```

### Physical Maintenance Ports & Manual Access Hatches
To comply with survival maintenance standards and allow physical clearing of conveyor blockages:
* **Port 1 (Locomotive Power Core):** Manual small conveyor access hatch at [X −2, Z +21, L2] for loading enriched uranium rods into `CAT Loco Reactor` directly from personal inventory.
* **Ports 2, 3, 4 (Hopper Holds 1, 2, 3):** Standard large-port maintenance hatches exposed along the lower Port sill (X = −2, Level L2) on each hopper car, allowing manual clearing or component retrieval without disassembly.
* **Port 5 (Stern Discharge Manifold):** Inspection junction at [X0, Z −15, L2] upstream of `CAT Train Sorter` to clear unlisted items or oversized salvage debris.

---

### 0.6 Master Electrical Single-Line Schematic & 5-Mode Energy Management Suite

```
[MASTER ELECTRICAL SINGLE-LINE SCHEMATIC]
PRIMARY GENERATION:
  [CAT Loco Reactor] (1× Small Nuclear Reactor, Centered [X0, Z +21, L3]) ──► 15.0 MW Continuous
ENERGY STORAGE BUFFER:
  [CAT Loco Battery 1..4] (4× Large Batteries, L2-L3) ───────────────────────► 12.0 MWh Capacity / 48.0 MW Discharge
TRAIN-WIDE ELECTRICAL BUS (High-Voltage Subgrid Pass-Through):
  ├─────────────────────────────────────────────────────────────────────────► 63.0 MW Peak Instantaneous
  │
  ├─► [CAT Loco Wheels] (4× 5×5 Suspension Drive Motors) ───────────────────► 8.0 MW Max Draw (Tractor)
  ├─► [CAT Car Wheels] (12× 5×5 Suspension DPU Drive Motors) ───────────────► 24.0 MW Max Draw (Hoppers 1-3)
  ├─► [CAT Hitches] (6× Motorized Hinge / Rotor Actuators) ─────────────────► 0.6 MW Active Steering/Damping
  ├─► [CAT Train Sorter] (1× Conveyor Sorter Discharge Pump) ───────────────► 0.5 MW Fast Drain
  └─► [CAT Avionics & Comms] (Antenna, Cockpit, Beacon, Lights) ────────────► 0.2 MW Baseline Load
```

### 5 Formal Operational Energy Management Modes
1. **Mode 1 · Silent Watchkeeping (Parked / Standby):**
   * Output: `0.2 MW`. Reactor idling; batteries set to `Auto`; all wheel suspension propulsion `OFF`; brake locks engaged.
2. **Mode 2 · Empty Shunting & Alignment (Speed < 30 km/h):**
   * Output: `4.5 MW`. Reactor powers locomotive steering and creep propulsion; hopper car DPU motors operate at 25% torque assist; hitch damping set to 20% for tight yard radius flex.
3. **Mode 3 · Laden Highway Cruise (Speed 50-70 km/h, Mass 2,868 t):**
   * Output: `18.5 MW` (15 MW Nuclear + 3.5 MW Battery assist). All 16 wheel drive motors engaged with synchronized torque; hitch damping stiffened to 85% to eliminate harmonic trailer whip.
4. **Mode 4 · Heavy Hill-Climb (20° Incline, Peak Tractive Draw):**
   * Output: `32.0 MW` (15 MW Nuclear + 17 MW Battery surge). All 16 wheel suspension motors commanded to 100% torque; DPU smart traction engages slip control.
5. **Mode 5 · Terminal Fast Discharge (Docked to Ground Base):**
   * Output: `3.5 MW`. Engine throttle cut; parking brakes locked; `CAT Train Sorter` commanded `Drain All = ON`; 2,500 t bulk ore purged across connector in < 90 s. Batteries recharge from Ground Base grid via locked connector.

---

### 0.7 Master Consist Bill of Materials (BOM) & Mass Budget

| Subsystem / Unit | Dimensions (m) | Block Count | Dry Mass (t) | PCU | Power Draw (MW) | Notes & Key Blocks |
|---|---|---|---|---|---|---|
| **Tractor Locomotive (`CAT-01-L`)** | 17.5 × 22.5 × 10.0 | 146 | 148.2 | 1,265 | 15.0 MW gen / 8.0 MW draw | 1× Ind Cockpit, 1× Sm Reactor, 4× Lrg Battery, 4× 5×5 Wheels, Push-Bumper |
| **Drawbar Hitch 1 (Loco to Car 1)**| 2.5 × 5.0 × 2.5 | 6 | 8.2 | 200 | 0.2 MW | 1× Adv Rotor (Yaw), 1× Motor Hinge (Pitch), 1× Adv Rotor (Roll), Clamps |
| **Hopper Car 1 (`CAT-01-C1`)** | 12.5 × 17.5 × 10.0 | 78 | 64.0 | 650 | 8.0 MW draw | 1× Lrg Cargo (3×3×3 B), 4× 5×5 Wheels, 45° Funnel Chutes, Gangways |
| **Drawbar Hitch 2 (Car 1 to Car 2)**| 2.5 × 5.0 × 2.5 | 6 | 8.2 | 200 | 0.2 MW | Identical 3-axis motorized flexible conveyor joint |
| **Hopper Car 2 (`CAT-01-C2`)** | 12.5 × 17.5 × 10.0 | 78 | 64.0 | 650 | 8.0 MW draw | Identical modular hopper car unit |
| **Drawbar Hitch 3 (Car 2 to Car 3)**| 2.5 × 5.0 × 2.5 | 6 | 8.2 | 200 | 0.2 MW | Identical 3-axis motorized flexible conveyor joint |
| **Hopper Car 3 / Tender (`CAT-01-C3`)**| 12.5 × 22.5 × 10.0 | 84 | 67.7 | 785 | 8.5 MW draw | 1× Lrg Cargo, 4× 5×5 Wheels, Sorter, Unload Connector, Crash Bumper |
| **MASTER CONSIST TOTALS** | **17.5 × 105.0 × 10.0** | **404** | **368.5 t** | **3,950** | **15 MW Gen / 63 MW Surge** | **Earth 1 g Laden Mass: 2,868.5 t (2,500 t Ore)** |

* All wheel suspension ratings, braking torques, and structural masses calculated at Earth 1 g baseline.
* Game values tagged with `†` awaiting in-game confirmation in RIG Test:
  * Small Reactor peak continuous output = `15.0 MW`†.
  * Large Battery peak discharge rate = `12.0 MW` per unit (48.0 MW combined)†.
  * 5×5 Suspension Wheel maximum braking torque = `30.0 MN·m`†.

---

### 0.8 Master Player Circulation, Access Corridors & Maintenance Walkways

To ensure 100% physical accessibility for survival engineers in field conditions:
1. **Pressurized Operator Cab (Locomotive Level L3):** Enclosed single-seat cockpit (`CAT Loco Helm`) accessed via Port-side exterior ladder at [X −3, Z +22, L2]. Cabin features survival suit life support, oxygen pressurization, and emergency survival kit.
2. **Continuous Side Running Boards (Levels L1-L2):** Non-skid steel catwalk plates run along both Port (X = −3.5) and Starboard (X = +3.5) sills of the locomotive, allowing walk-around inspection while parked.
3. **Inter-Car Articulated Footplates (Level L2):** Overlapping Heavy Armor Half-Blocks mounted above each drawbar hitch provide a continuous walking bridge between cars across the flexible joint gap.
4. **Hopper Catwalks & Grab Irons (Levels L3-L4):** Vertical ladders mounted on the forward bulkhead of each hopper car climb to elevated perimeter catwalks bordering the top loading funnel chutes, providing full visual inspection of bulk ore intake.

---

### 0.9 CATERPILLAR-02 (MKII) Heavy Armored Rail / Combat Upgrade Roadmap

* **Operational Context:** In advanced survival scenarios or contested hostile sectors subject to Modular Encounter Spawner (MES) pirate raids, the CATERPILLAR-01 consist can be upgraded to the **MKII Armored Combat Rail Consist**:
  1. **Locomotive Armored Prow & CIWS Crown:** Replace bow push-bumper with a faceted heavy armor snowplow ram; mount 1× Large Grid Gatling Turret (`CAT Turret Cab`) atop Level L4 roof crown tied to ammo sorters.
  2. **Hopper Armor Skirts:** Extend heavy armor blast skirts downward along Level L1 to shield all 16 suspension wheel stators from small-arms and explosive shrapnel.
  3. **Tender Rear Defense Emplacement:** Mount 1× Large Grid Autocannon Turret atop the tender aft gantry covering the rear convoy approach sector.
  4. **Conveyor Armor Encapsulation:** Enclose all flexible drawbar jumper loops within overlapping heavy armor collar rings.

---

## Phase 11-1 · Tractor Locomotive Heavy Chassis & 4-Wheel Powertrain (Levels L1-L2)

```
       [PHASE 11-1 SPATIAL LOCATOR HUD]
       Unit: Tractor Locomotive (CAT-01-L)
       Envelope: 7 W × 9 L × 2 H B (17.5 × 22.5 × 5.0 m)
       Coordinate Range: X −3.5 to +3.5, Z +17 to +25, Level L1 to L2
       Key Datums:
       · Front Push-Bumper Datum: Z = +25, Level L1-L2
       · Rear Hitch Attachment Bulkhead: Z = +16, Level L1-L2
       · Front Steering Axle 1: Z = +23, X = ±3.5 (5×5 Wheels)
       · Rear Fixed Drive Axle 2: Z = +19, X = ±3.5 (5×5 Wheels)
       · Chassis Centerline Box Girder: X0, Z +17 to +25
```

### Pre-Step Master Parts Tray: Phase 11-1
* 42× Heavy Armor Blocks (Palette Slot 0: `#2B303A`, 126.0 t, 42 PCU)
* 18× Heavy Armor Half-Slopes (Palette Slot 0, 18.0 t, 18 PCU)
* 8× Heavy Armor Slopes (Palette Slot 0, 16.0 t, 8 PCU)
* 2× Wheel Suspension 5×5 Left (16.0 t, 200 PCU)
* 2× Wheel Suspension 5×5 Right (16.0 t, 200 PCU)
* 9× Large Conveyor Tubes (Palette Slot 4: `#1ABC9C`, 7.2 t, 90 PCU)
* 2× Large Conveyor Junctions (Palette Slot 4, 2.6 t, 26 PCU)

---

### Step 11.1 · Twin Longitudinal Box Girders & Underbody Skid Keel (L1)
Build the primary foundation spine of the locomotive from Z +17 to +25:
1. Lay two parallel Heavy Armor box girders at X = −2 and X = +2 spanning Z +17 to +25 at Level L1.
2. Tie the girders together with transverse cross-beams at Z = +17, Z = +21, and Z = +25.
3. Install continuous Heavy Armor Half-Blocks along centerline X0, forming a flat underside skid plate providing 1.25 m clearance above ground.
4. Bevel the girder outer edges using Heavy Armor Half-Slopes sloping inward toward X0.

```
Plan view, Level L1 (Locomotive Chassis Box Girders)
         X: −2            X0            +2
Z: +25   ■════════════════╤════════════════■   ◄── Bow Bumper Tie Beam
Z: +23   │  [BOX GIRDER]  │  [BOX GIRDER]  │
Z: +21   ■════════════════╪════════════════■   ◄── Midline Tie Beam (Datum Z0)
Z: +19   │  [BOX GIRDER]  │  [BOX GIRDER]  │
Z: +16   ■════════════════╧════════════════■   ◄── Aft Drawbar Tie Beam
```

---

### Step 11.2 · Central Conveyor Raceway & Manifold Junctions (L1-L2)
Install the primary ore transport conduit through the chassis core:
1. Lay 7× Large Conveyor Tubes along centerline X0 from Z +17 to Z +24 at Level L1.
2. Mount 1× Large Conveyor Junction at [X0, Z +25, L1] angled upward to feed the bow push-bumper connector.
3. Mount 1× Large Conveyor Junction at [X0, Z +17, L2] angled aft (−Z) to feed Drawbar Hitch 1.

---

### Step 11.3 · 5×5 Suspension Wheel Mounts & Axles 1 & 2 Geometry (L1)
Install the locomotive tractive gear:
1. Mount 2× Wheel Suspension 5×5 Left units on the Port chassis sill at [X −3, Z +23, L1] (`WH 1: Steer P`) and [X −3, Z +19, L1] (`WH 3: Drive P`), facing outward (−X).
2. Mount 2× Wheel Suspension 5×5 Right units on the Starboard sill at [X +3, Z +23, L1] (`WH 2: Steer S`) and [X +3, Z +19, L1] (`WH 4: Drive S`), facing outward (+X).
3. Ensure suspension stators orient with steering pivot pins strictly vertical (Y-axis).

#### Terminal Configuration & Tuning Sliders (`CAT Loco Wheels`):
* **Axle 1 (`WH 1, 2` · Front Steering):** Steering = `ON` (Angle = `32.0°`), Propulsion = `ON` (Torque = `100%`), Friction = `70%`, Strength = `65%`, Height Offset = `−10 cm` (lowers CoM).
* **Axle 2 (`WH 3, 4` · Rear Drive):** Steering = `OFF` (Locked 0.0°), Propulsion = `ON` (Torque = `100%`), Friction = `75%`, Strength = `70%`, Height Offset = `−10 cm`.
* **Speed Limit:** Set to `70.0 km/h` project-wide.

---

### Step 11.4 · Flared Wheel Arch Mudguards & Side Running Boards (L2)
1. Mount Heavy Armor Half-Blocks directly above each 5×5 wheel hub at X = ±3 to ±4, Level L2.
2. Cap the outer mudguard sills with Heavy Armor Half-Slopes angled 45° downward, protecting suspension stators from flying ballast stone.
3. Lay catwalk plates along X = ±3.5 between the wheel arches to create continuous side running boards for engineer access.

> [!CAUTION]
> **PRE-ENCLOSURE INSPECTION GATE 11.1A: CHASSIS & POWERTRAIN INTEGRITY**
> Before proceeding to cab deck framing:
> 1. Verify uninterrupted large-conveyor flow along centerline X0 from Z +25 to Z +16.
> 2. Confirm Axle 1 steers freely through ±32° without clipping adjacent chassis armor blocks.
> 3. Verify all 4 suspension wheel stators belong to group `CAT Loco Wheels` and respond to brake input.

---

#### Visual Milestone 11-1 · Locomotive Chassis Complete

```
Visual Checkpoint Silhouette (Looking Port at Locomotive Chassis L1-L2):
            [BOW BUMPER MOUNT Z +25]             [DRAWBAR BULKHEAD Z +16]
                       \                                   /
         ═══════════════╧═════════════════════════════════╧═══════════════
               (O) [AXLE 1: STEER]               (O) [AXLE 2: DRIVE]
```

* [ ] Subgrid dry mass verified: `≈ 148.2 t`.
* [ ] 1.25 m belly skid clearance maintained under full suspension compression.

---

## Phase 11-2 · Locomotive Power Core, Armored Cab & Forward Push-Bumper (Levels L2-L4)

```
       [PHASE 11-2 SPATIAL LOCATOR HUD]
       Unit: Locomotive Upper Hull & Cab (CAT-01-L)
       Envelope: 7 W × 9 L × 3 H B (17.5 × 22.5 × 7.5 m)
       Coordinate Range: X −3.5 to +3.5, Z +17 to +25, Level L2 to L4
       Key Datums:
       · Armored Cab Center: [X0, Z +22, Level L3]
       · Nuclear Reactor Vault: [X0, Z +21, Level L3]
       · Battery Vault: [X ±2, Z +21, Level L2-L3]
       · Forward Push-Bumper: [X0, Z +25, Level L2]
```

### Pre-Step Master Parts Tray: Phase 11-2
* 1× Industrial Cockpit (Palette Slot 7: `#1B263B`, 3.5 t, 15 PCU)
* 1× Small Nuclear Reactor (Palette Slot 6: `#C0392B`, 1.6 t, 25 PCU)
* 4× Large Batteries (Palette Slot 6, 20.0 t, 80 PCU)
* 1× Large Connector (Palette Slot 1: `#9E2A2B`, 3.1 t, 150 PCU)
* 1× Radio Antenna (Large Grid) (Palette Slot 7, 8.0 t, 100 PCU)
* 2× Heavy Armor Blast Door Blocks (Palette Slot 1, 4.8 t, 2 PCU)
* 28× Heavy Armor Slopes 2×1×1 (Palette Slot 7, 21.0 t, 28 PCU)
* 1× Programmable Block (Palette Slot 7, 0.5 t, 25 PCU)

---

### Step 11.5 · Nuclear Power Core & 4× Large Battery Vault (L2-L3)
1. Mount 1× Small Nuclear Reactor `CAT Loco Reactor` centered at [X0, Z +21, Level L3] directly over the conveyor spine junction.
2. Install 2× Large Batteries `CAT Loco Battery 1, 2` on the Port flank at [X −2, Z +21, Level L2-L3].
3. Install 2× Large Batteries `CAT Loco Battery 3, 4` on the Starboard flank at [X +2, Z +21, Level L2-L3].
4. Enclose the power vault with Heavy Armor Blocks painted Palette Slot 6 (High-Voltage Red).

---

### Step 11.6 · Single-Operator Armored Cab & Programmable Block Rack (L3-L4)
1. Mount 1× Industrial Cockpit `CAT Loco Helm` at [X0, Z +23, Level L3] facing forward (+Z).
2. Install Programmable Block `[CAT] PB Train Master` in the sub-cab avionics bay at [X0, Z +22, Level L2].
3. Frame the cab with Heavy Armor Slopes 2×1×1 sloping inward from the L2 beltline to the L4 roofline, forming chiseled lateral tumblehome.
4. Mount an overhanging cab brow visor at Level L4 (Z +24) to eliminate planetary solar glare and protect viewport glass from falling stone spall.

---

### Step 11.7 · Heavy Bow Push-Bumper & Soft-Capture Receiving Port (L1-L2)
1. Across the forward face (Z = +25, Level L1-L2), weld Heavy Armor Blast Door Blocks painted Palette Slot 1 (Heavy Rail Crimson).
2. Mount 1× Large Connector `CAT Loco Front Conn` at [X0, Z +25, Level L2] facing forward (+Z), plumbed to the central conveyor junction. Height: exactly 2.5 m above ground.
3. Flank the connector with Heavy Armor Corner 2×1×1 slopes at X = ±2, forming rigid guide horns that funnel the incoming Mastodon stern dock into lock alignment.

#### Terminal Configuration & Sliders:
* Connector: `CAT Loco Front Conn` (Auto-Connect: `OFF` · Collect All: `OFF` · Connect Strength: `0.015`).

---

### Step 11.8 · Comms Mast, Roof Warning Beacon & Emergency Decouple Merge (L3-L4)
1. Mount 1× Large Grid Radio Antenna `CAT Loco Antenna` at [X0, Z +18, Level L4]. Set broadcast range to 25,000 m.
2. Mount 1× Rotating Warning Beacon `CAT Loco Beacon` at [X0, Z +20, Level L4]. Set color to Amber (`#E6A100`) and spin rate to 30 rpm.
3. Install 1× Merge Block `CAT Hitch Decouple 1` at [X −2, Z +16, Level L2] facing aft (−Z) to provide pyrotechnic consist emergency severance if trailers become pinned.

> [!IMPORTANT]
> **PRE-ENCLOSURE INSPECTION GATE 11.2A: CAB & POWER ACCEPTANCE**
> 1. Verify reactor draws uranium automatically through the central conveyor riser.
> 2. Ensure cockpit displays populate battery charge status and power reserves (63 MW combined surge capacity).
> 3. Verify driver controls: Hotbar 1 wheel brake, connector lock/unlock, and emergency decouple toggle.

---

## Phase 11-3 · 3-Axis Articulated Conveyor Drawbar Hitches (Levels L1-L3)

```
       [PHASE 11-3 SPATIAL LOCATOR HUD]
       Unit: 3-Axis Conveyor Drawbar Hitch (CAT-HITCH 1..3)
       Envelope: 1 W × 2 L × 2 H B (2.5 × 5.0 × 5.0 m per joint)
       Coordinate Range: X0, Z spans 2 B between vehicle bulkheads, Level L1 to L3
       Kinematic Axes:
       · Yaw Advanced Rotor: Vertical Axis (±45.0° horizontal articulation)
       · Pitch Motor Hinge: Transverse Axis (±25.0° vertical terrain flex)
       · Roll Advanced Rotor: Longitudinal Axis (±20.0° cross-slope flex)
       · Large Conveyor Pass-Through: Continuous airtight flexible jumper loop
```

### Pre-Step Master Parts Tray: Phase 11-3 (Per Drawbar Joint ×3)
* 1× Advanced Rotor (Yaw) (Palette Slot 2: `#E6A100`, 8.2 t, 100 PCU)
* 1× Motor Hinge (Pitch) (Palette Slot 2, 8.0 t, 100 PCU)
* 1× Advanced Rotor (Roll) (Palette Slot 2, 8.2 t, 100 PCU)
* 2× Large Magnetic Plates (Palette Slot 2, 4.0 t, 50 PCU)
* 4× Large Curved Conveyor Tubes (Palette Slot 4: `#1ABC9C`, 3.2 t, 40 PCU)
* 2× Heavy Armor Half-Blocks (Footplates) (Palette Slot 9: `#F1C40F`, 4.0 t, 2 PCU)

---

### Step 11.9 · Kinematic 3-Axis Gimbal Drawbar Assembly (L2)
Assemble the 3-axis articulated mechanical link between leading and trailing units:
1. Mount 1× Advanced Rotor `CAT Hitch Yaw 1` on the leading car rear bulkhead at [X0, Z +16, Level L2] with rotor stator vertical (Y-axis), providing horizontal steering yaw from −45.0° to +45.0°.
2. Weld 1× Motor Hinge `CAT Hitch Pitch 1` onto the yaw rotor head with hinge pin horizontal (X-axis), providing vertical terrain cresting/trough pitch from −25.0° to +25.0°.
3. Mount 1× Advanced Rotor `CAT Hitch Roll 1` onto the hinge head with rotor stator aligned longitudinally (Z-axis), providing axial roll compliance from −20.0° to +20.0° across uneven ground.
4. Plumb large curved conveyor tubes across the joint to maintain continuous airtight conveyance.
5. Install twin magnetic stabilizer clamps `CAT Hitch Clamp Port/Stbd` at X = ±1.5 to eliminate high-speed highway oscillation.

#### Joint Parameters & Slider Settings (MANDATORY):
* **Share Inertia Tensor = `ON`** on ALL rotors and hinges across all 3 drawbars (eliminates subgrid sway and phantom physics Klang).
* **`CAT Hitch Yaw`:** Limits = `−45.0°` to `+45.0°` · Braking Torque = `30.0 MN·m`†.
* **`CAT Hitch Pitch`:** Limits = `−25.0°` to `+25.0°` · Braking Torque = `30.0 MN·m`†.
* **`CAT Hitch Roll`:** Limits = `−20.0°` to `+20.0°` · Braking Torque = `15.0 MN·m`†.

```
3-Axis Gimbal Drawbar Articulation Diagram (Looking Port at Joint, Level L2)
  Leading Car Aft Bulkhead
            ║
            ╠══[CAT HITCH YAW] (Advanced Rotor · Vertical Axis ±45°)
            ║        │
            ║        ▼
            ║   [CAT HITCH PITCH] (Motor Hinge · Transverse Axis ±25°)
            ║        │
            ║        ▼
            ║   [CAT HITCH ROLL] (Advanced Rotor · Longitudinal Axis ±20°)
            ║        │
            ║        ▼ (Flexible Large Curved Conveyor Pipe Pass-Through)
            ╚════════╪══════════════════════════════════════════════════════
                     ▼
         Trailing Car Forward Bulkhead
```

> [!NOTE]
> Repeat Step 11.9 for Drawbar Hitch 2 (Z +6 to +7) and Drawbar Hitch 3 (Z −3 to −2).

---

## Phase 11-4 · Modular Hopper Cars 1 & 2 Construction (Levels L1-L4)

```
       [PHASE 11-4 SPATIAL LOCATOR HUD]
       Unit: Modular Hopper Cars 1 & 2 (CAT-01-C1 & CAT-01-C2)
       Envelope: 5 W × 7 L × 4 H B (12.5 × 17.5 × 10.0 m per car)
       Coordinate Range:
       · Car 1: X −2.5 to +2.5, Z +8 to +14, Level L1 to L4
       · Car 2: X −2.5 to +2.5, Z −1 to +5, Level L1 to L4
       Hold Centroid:
       · Car 1 Hold: [X0, Z +11, Level L2-L3] (Large Cargo 3×3×3 B)
       · Car 2 Hold: [X0, Z +2, Level L2-L3] (Large Cargo 3×3×3 B)
```

### Pre-Step Master Parts Tray: Phase 11-4 (Per Hopper Car)
* 1× Large Cargo Container (Palette Slot 5: `#27AE60`, 28.5 t, 250 PCU)
* 4× Wheel Suspension 5×5 (2 Left, 2 Right) (Palette Slot 0, 32.0 t, 400 PCU)
* 2× Large Conveyor Junctions (Palette Slot 4, 2.6 t, 26 PCU)
* 24× Heavy Armor Slopes 2×1×1 (Palette Slot 3: `#D35400`, 18.0 t, 24 PCU)
* 32× Heavy Armor Blocks (Palette Slot 0, 96.0 t, 32 PCU)
* 4× Steel Catwalk Plates (Palette Slot 9, 1.6 t, 4 PCU)

---

### Step 11.10 · Hopper Car 1 Chassis & DPU Traction Wheels (L1)
1. Construct the 5 W × 7 L heavy armor chassis spanning Z +8 to +14 at Level L1.
2. Mount 2× Wheel Suspension 5×5 Left units at [X −2.5, Z +13, L1] (`WH 5: DPU P`) and [X −2.5, Z +8, L1] (`WH 7: DPU P`).
3. Mount 2× Wheel Suspension 5×5 Right units at [X +2.5, Z +13, L1] (`WH 6: DPU S`) and [X +2.5, Z +8, L1] (`WH 8: DPU S`).
4. Lock steering at `0.0°` (fixed straight-line tracking). Set propulsion torque to `100%` and assign to group `CAT Car Wheels`.

---

### Step 11.11 · Hopper Car 1 Large Cargo Hold & Top Funnel Chutes (L2-L4)
1. Mount 1× Large Cargo Container `CAT Hold 1` (3×3×3 B) centered at [X0, Z +11, Level L2-L3]. Capacity: 1,386,000 L.
2. Tie the forward bottom port to the inbound conveyor riser at [X0, Z +13, L2]; tie the aft bottom port to the outbound riser at [X0, Z +7, L2].
3. Frame the container's upward-facing large port with Heavy Armor Slopes 2×1×1 angled inward at 45° painted Palette Slot 3 (Orange), forming a wide top-loading funnel chute for filling from quarry hoppers.
4. Install side maintenance access ports on the Port sill at [X −2.5, Z +11, L2].

> [!NOTE]
> Repeat Steps 11.10 and 11.11 for Hopper Car 2 spanning Z −1 to +5 (`CAT Hold 2`, Axles 5 & 6, Wheels `WH 9..12` at Z +4 and −1; hold center [X0, Z +2]).

---

## Phase 11-5 · Hopper Car 3 / Tender & Stern High-Speed Terminal Discharge Bumper (Levels L1-L4)

```
       [PHASE 11-5 SPATIAL LOCATOR HUD]
       Unit: Hopper Car 3 / Tender (CAT-01-C3)
       Envelope: 5 W × 9 L × 4 H B (12.5 × 22.5 × 10.0 m)
       Coordinate Range: X −2.5 to +2.5, Z −12 to −4, Level L1 to L4
       Key Datums:
       · Car 3 Hold Centroid: [X0, Z −8, Level L2-L3]
       · Sorter Discharge Pump: [X0, Z −15, Level L2]
       · Stern Terminal Extension: Z −16 to −13 (4 B)
       · Stern Unload Connector: [X0, Z −16, Level L2] (Height: 2.5 m)
       · Collision Push-Bumper: Z = −16, Level L1-L2
```

### Pre-Step Master Parts Tray: Phase 11-5
* 1× Large Cargo Container (Palette Slot 5: `#27AE60`, 28.5 t, 250 PCU)
* 4× Wheel Suspension 5×5 (2 Left, 2 Right) (Palette Slot 0, 32.0 t, 400 PCU)
* 1× Large Conveyor Sorter (Palette Slot 4: `#1ABC9C`, 1.2 t, 40 PCU)
* 1× Large Connector (Palette Slot 1: `#9E2A2B`, 3.1 t, 150 PCU)
* 2× Heavy Armor Blast Door Blocks (Palette Slot 1, 4.8 t, 2 PCU)
* 1× Event Controller `CAT EC Dump` (Palette Slot 7, 0.2 t, 25 PCU)

---

### Step 11.12 · Tender Hold 3 & DPU Axles 7 & 8 (L1-L3)
1. Build the extended 5 W × 9 L chassis spanning Z −12 to −4 at Level L1.
2. Install Axles 7 and 8 with 4× 5×5 suspension wheels (`WH 13..16`) at Z = −6 and Z = −10. Configure for 100% propulsion assist.
3. Mount Large Cargo Container `CAT Hold 3` centered at [X0, Z −8, Level L2-L3].
4. Install top funnel chutes and catwalk framing matching Cars 1 and 2.

---

### Step 11.13 · High-Speed Pressurized Discharge Sorter & Stern Connector (L2)
Install the automated terminal dump system:
1. Mount 1× Large Conveyor Sorter `CAT Train Sorter` horizontally at [X0, Z −15, Level L2] with flow arrow pointing stern (−Z).
2. Configure sorter whitelist with bulk minerals: `Iron`, `Nickel`, `Silicon`, `Cobalt`, `Magnesium`, and `Stone`. Set `Drain All = OFF` by default (triggered by docking EC).
3. Mount 1× Large Connector `CAT Train Unload Conn` at [X0, Z −16, Level L2] facing stern (−Z), plumbed directly to the sorter outlet. Centerline height: exactly 2.5 m above tarmac.
4. Mount Event Controller `CAT EC Dump` adjacent to the sorter at [X −1, Z −15, L2].

---

### Step 11.14 · Heavy Collision Bumper & Ground Base Alignment Horns (L1-L2)
1. Across the rear transom (Z = −16, Level L1-L2), weld Heavy Armor Blast Door Blocks painted Palette Slot 1 (Crimson).
2. Install Heavy Armor Corner 2×1×1 slopes at X = ±2, forming rigid guide wedges angled aft (−Z) to absorb docking kinetic shock and funnel the connector into the Ground Base receiving dock.
3. Install yellow warning chevrons (Palette Slot 9) across the bumper face.

---

## Phase 11-6 · Consist Catwalks, Lighting Matrix & DPU Synchronized Traction (Levels L1-L4)

### Step 11.15 · Continuous Consist Maintenance Catwalk Network (L4)
1. Connect all hopper top loading platforms with continuous steel catwalks spanning the inter-car drawbar gaps using articulated hinged footplates.
2. Install safety handrails and yellow grab irons along all ladderwells.
3. Mount 4× Worksite LED Spotlights `CAT Floodlight 1..4` along the hopper sills angled downward 45° to illuminate nighttime quarry transfer operations.

---

## Phase 11-7 · Master Control Logic, MDK2 Zero-GC Architecture & Terminal Dictionary

### 1. Dual Compute Architecture
* **Locomotive Master PB (`[CAT] PB Train Master`):**
  * Target Block: Located in Locomotive Sub-Cab at [X0, Z +22, Level L2].
  * Grid: Large Grid (42 B Consist · 16 Wheels) · Update: `Update1` (Traction/Braking) + `Update100` (Telemetry).
  * Managed Groups: `CAT Train Wheels`, `CAT Hitches`, `CAT Holds`, `CAT Loco Power`.
  * Roles: Distributed traction coordination, speed-dependent anti-jackknife drawbar stiffening, and automatic terminal dump sequencing.

### 2. Group Taxonomy
* `CAT Train Wheels` : All 16 wheel suspension units across locomotive and 3 hopper cars.
* `CAT Train Brakes` : Synchronized parking and emergency brake group.
* `CAT Hitches` : All 6 motorized hitch rotors and hinges linking the 4 consist units.
* `CAT Holds` : All 3 Large Cargo Containers (`CAT Hold 1`, `CAT Hold 2`, `CAT Hold 3`).
* `CAT Loco Power` : 1× Small Nuclear Reactor + 4× Large Batteries.
* `CAT Lights` : Bow headlights, cabin interior lights, running board markers, and floodlights.

### 3. Block Naming & Terminal Dictionary
* `CAT Loco Helm` : Industrial Cockpit in locomotive cab [X0, Z +23, L3].
* `CAT Loco Front Conn` : Large Connector on bow push-bumper [X0, Z +25, L2].
* `CAT Loco Reactor` : 15 MW Small Nuclear Reactor [X0, Z +21, L3].
* `CAT Loco Battery 1..4` : 4× Large Batteries in locomotive vault [X ±2, Z +21, L2-L3].
* `CAT Loco Antenna` : 25 km Radio Antenna mast [X0, Z +18, L4].
* `CAT Loco Beacon` : Amber Rotating Warning Beacon [X0, Z +20, L4].
* `CAT Hitch Yaw 1..3` : Advanced Rotors on leading car rear bulkheads (Vertical Axis).
* `CAT Hitch Pitch 1..3` : Motor Hinges on drawbar links (Horizontal Transverse Axis).
* `CAT Hitch Roll 1..3` : Advanced Rotors on trailing car bulkheads (Longitudinal Axis).
* `CAT Hitch Clamp Port/Stbd 1..3` : Magnetic anti-sway stabilizer clamps on drawbars.
* `CAT Hold 1, 2, 3` : Large Cargo Containers in Hoppers 1, 2, and 3 [Level L2-L3].
* `CAT Train Sorter` : Large Conveyor Sorter on tender rear manifold [X0, Z −15, L2].
* `CAT Train Unload Conn` : Large Connector on stern bumper [X0, Z −16, L2].
* `CAT EC Dump` : Event Controller automating terminal fast-dump sequence.
* `CAT EC AntiJackknife` : Event Controller automating speed-based drawbar damping.
* `CAT EC Traction` : Event Controller modulating DPU wheel torque based on payload mass.

---

### 4. Event Controller Matrix (Rule C4 Compliance)

| Event Controller | Monitored Block / Variable | Condition & Threshold | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| **`CAT EC Dump`** | `CAT Train Unload Conn` | Connector: `Connected` | (1) `CAT Train Sorter` Drain All = `ON`<br/>(2) `CAT Train Wheels` Handbrake = `ON`<br/>(3) Sound `CAT Alarm Dump` | (1) `CAT Train Sorter` Drain All = `OFF`<br/>(2) `CAT Train Wheels` Handbrake = `OFF` |
| **`CAT EC AntiJackknife`** | Vehicle Velocity | Speed $\ge 40.0\text{ km/h}$ | (1) Group `CAT Hitches` Damping $\rightarrow 85\%$<br/>(2) Group `CAT Hitch Clamps` Auto-Lock = `ON` | (1) Group `CAT Hitches` Damping $\rightarrow 20\%$<br/>(2) Group `CAT Hitch Clamps` Unlock |
| **`CAT EC Traction`** | Consist Gross Mass | Gross Mass $\ge 1,500\text{ t}$ | (1) Group `CAT Car Wheels` Torque $\rightarrow 100\%$<br/>(2) Power Mode $\rightarrow$ `MODE 4: HILL_CLIMB` | (1) Group `CAT Car Wheels` Torque $\rightarrow 50\%$<br/>(2) Power Mode $\rightarrow$ `MODE 3: CRUISE` |
| **`CAT EC Decouple`** | `CAT Hitch Decouple 1` | Merge Block: `Disconnected` | (1) Trailing Axles Emergency Brake = `ON`<br/>(2) Broadcast `TMS.CONSIST.SEVER` | None |

---

### 5. IGC Channel Subscriptions (Ground Fleet Bus)

```
[IGC GROUND BUS TOPOLOGY · SEMICOLON DELIMITED]
  ┌─────────────────────────────────────────────────────────┐
  │                 GROUND BASE C2 (GND-01)                 │
  └────────────────────────────┬────────────────────────────┘
                               │
            ┌──────────────────┴──────────────────┐
            ▼ (TMS.BASE.RECEIVING)                ▲ (TMS.CAT.STATUS)
  ┌─────────────────────────────────┐   ┌─────────────────────────────────┐
  │ CATERPILLAR-01 (CAT-01-L)       │   │ MASTODON-01 MKI (MAS-01)        │
  │ Status: SPEED;MASS;HITCH;STATE  │   │ Sync: PISTON_DOCK;TRANSFER_RATE │
  └────────────────┬────────────────┘   └────────────────┬────────────────┘
                   │                                     │
                   └──────────────◄──────────────────────┘
                           TMS.CONVOY.SYNC / TMS.MAST.TRANSFER
```

* **Outbound Channels:**
  * `TMS.CAT.STATUS`: `train_id;gross_mass_t;payload_ore_t;speed_kmh;hitch_status;route_status`
    * Example: `CAT-01;2840.5;2472.0;58.2;LOCKED_85PCT;EN_ROUTE_BASE`
  * `TMS.CONSIST.SEVER`: `SEVER;CAT-01;DRAWBAR_1;EMERGENCY_DECOUPLE`
* **Inbound Channels:**
  * `TMS.BASE.RECEIVING`: `GND-01;CLEAR_TO_DUMP;TERMINAL_DOCK_1;READY`
  * `TMS.MAST.TRANSFER`: `MAS-01;SOFT_CAPTURE_LOCK;TRANSFER_ACTIVE;2500_T`
  * `TMS.CONVOY.SYNC`: `CONVOY;MAS-01;45.0;180.0;ROADBED_SPREADER_ON`
  * `TMS.LOGISTICS.JIT`: `JIT;MAS-01;GND;180;IRON;2500000`

---

### 6. Driver Cockpit Hotbar Layouts (`CAT Loco Helm`)

#### Hotbar 1 · Driving, Brakes & Powertrain
* [1] `CAT Train Wheels` Handbrake Toggle
* [2] `CAT Train Wheels` Propulsion Increase
* [3] `CAT Train Wheels` Propulsion Decrease
* [4] `CAT Loco Front Conn` Switch Lock (Mastodon Push-Dock)
* [5] `CAT Train Unload Conn` Switch Lock (Ground Base Dump Dock)
* [6] `CAT Hitches` Damping Increase (Manual Stiffen)
* [7] `CAT Hitches` Damping Decrease (Manual Flex)
* [8] `CAT Loco Power` Battery Recharge Toggle
* [9] `CAT Lights` Toggle On/Off

#### Hotbar 2 · Drawbar Hitches, DPU Traction & Diagnostics
* [1] `CAT Hitch Clamps` Auto-Lock Toggle
* [2] `CAT Hitch Decouple 1` Decouple (Emergency Sever)
* [3] `CAT Car Wheels` Propulsion Override Increase
* [4] `CAT Car Wheels` Propulsion Override Decrease
* [5] `CAT Train Sorter` Drain All Toggle
* [6] `[CAT] PB Train Master` Run: `MODE_CRUISE`
* [7] `[CAT] PB Train Master` Run: `MODE_HILL_CLIMB`
* [8] `[CAT] PB Train Master` Run: `RELOAD`
* [9] `CAT Loco Beacon` Toggle On/Off

#### Hotbar 3 · Fleet Comms & Logistics Dispatch
* [1] `[CAT] PB Train Master` Run: `IGC_SEND;TMS.BASE.RECEIVING;REQUEST_DOCK`
* [2] `[CAT] PB Train Master` Run: `IGC_SEND;TMS.MAST.TRANSFER;READY_TO_RECEIVE`
* [3] `CAT Floodlights` Toggle On/Off
* [4] `CAT Alarm Dump` Toggle On/Off
* [5] `CAT Loco Antenna` Range Increase
* [6] `CAT Loco Antenna` Range Decrease
* [7] `CAT Loco Reactor` Toggle On/Off
* [8] Group `CAT Holds` Show on HUD Toggle
* [9] `CAT Loco Helm` Dampeners Toggle

---

## Phase 11-8 · Commissioning & Field Acceptance Checklist (RIG Test)

### Suite A · Consist Powertrain & Steering Geometry Trials
* [ ] **Turn Radius Verification:** Drive the 42 B consist through a 90° intersection at 25 km/h; verify Axle 1 steers smoothly through ±32° and all 3 trailing cars track within an 18 m swept path without clipping curbs.
* [ ] **Independent DPU Traction Verification:** With locomotive reactor powered OFF, engage battery power; confirm all 12 hopper suspension wheels provide active tractive effort and propel the empty consist to 40 km/h.

### Suite B · 3-Axis Drawbar Articulation & Stability
* [ ] **3-Axis Terrain Compliance:** Navigate the consist over an uneven rocky ridge; confirm Drawbar Hitches 1-3 flex independently through pitch (±25°), yaw (±45°), and roll (±20°) without chassis block collision or subgrid shearing.
* [ ] **Share Inertia Tensor Audit:** Inspect all 6 drawbar rotors/hinges in terminal; confirm `Share Inertia Tensor = ON` on all joints; verify ZERO phantom physics oscillation while stationary on 15° slopes.

### Suite C · Full Laden Hill-Climb & Regenerative Braking
* [ ] **2,500 t Full Payload Hill-Climb:** Fill all 3 hopper containers with stone (2,868 t gross consist mass); ascend a 20° voxel incline from a dead stop; verify consist accelerates smoothly to 45 km/h without wheel spin or reactor overload.
* [ ] **Controlled Descent Braking:** Descend a 20° slope at 50 km/h; apply full service brakes; confirm all 16 wheels brake synchronously, halting the consist in $< 65\text{ m}$ with zero jackknifing.

### Suite D · High-Speed 70 km/h Highway Run & Anti-Jackknife
* [ ] **70 km/h Highway Test:** Cruise on flat terrain at 70 km/h; confirm `CAT EC AntiJackknife` triggers at 40 km/h, stiffening hitch damping to 85% and engaging magnetic stabilizer clamps; confirm zero trailer whip or harmonic sway.

### Suite E · Mastodon Soft-Capture Field Docking (< 90 s)
* [ ] **Push-Dock Mating:** Back locomotive bow bumper into Mastodon MKI stern alignment guide horns; confirm `CAT Loco Front Conn` locks smoothly at 2.5 m elevation.
* [ ] **2,500 t Bulk Ore Fill:** Engage transfer sorters from Mastodon hold; verify all 3 hopper cars fill evenly to 100% capacity in $< 90\text{ seconds}$ without conveyor bottleneck.

### Suite F · Ground Base Terminal Discharge & In-Deck Collector Flow
* [ ] **Reverse Terminal Docking:** Reverse consist into Ground Base Bulk Receiving Dock; verify stern alignment wedges funnel `CAT Train Unload Conn` into lock with the base terminal connector.
* [ ] **High-Speed Gravity/Pump Dump:** Verify `CAT EC Dump` automatically engages `CAT Train Sorter` (Drain All = ON); confirm 2,500 t of ore purges into the 5× in-deck collectors in $< 90\text{ seconds}$.

### Suite G · Player Circulation & Survival Ergonomics
* [ ] **Continuous Walk-Around:** Walk from the locomotive cab running boards across all 3 drawbar footplates to the tender rear bumper; confirm zero lethal fall gaps.
* [ ] **Manual Port Inspection:** Access all 3 hopper maintenance ports from ground level; verify ability to manually transfer items into cargo holds.

### Suite H · Emergency Consist Decoupling & Deadman Braking
* [ ] **Emergency Severance Test:** While cruising at 40 km/h, trigger `CAT Hitch Decouple 1`; confirm locomotive breaks free cleanly while trailing hopper cars immediately engage emergency friction brakes and come to a safe stop.
