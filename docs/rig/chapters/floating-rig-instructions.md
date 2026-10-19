# Volume III · Floating Rig Structure (25 kt Fortress)

This volume details the physical construction of the Floating Rig: a 25,000 t heavy armored aerial fortress stationed at Earth 1 g (Decision 48).

```
Master Elevation Profile · Floating Rig 25 kt Heavy Fortress (Looking North):
                       [H1 Command Bridge & Roof Pickets]           (S9 · L21-L24)
                       [H2 Crew Citadel & Life Support]             (S8 · L17-L20)
                       [H3 Gas Farm & Core Machine Shop]            (S7 · L13-L16)
  ◄─── Pylon S1 ───►  ╔═══════════════════════════════════════════╗  ◄─── Pylon S2 ───►
  (8 W × 7 H × 22 L)  ║ H4 Nuclear Power Vault (3× Large React)   ║  (8 W × 7 H × 22 L)
  [7 Atmo + 4 H2]     ║ H5 Pylon Sockets, Gyros & Trunk Ring      ║  [7 Atmo + 4 H2]
  [3 Modular Bays]    ║ H6 Central Lift Ring (10 Atmo + 6 H2)     ║  [3 Modular Bays]
                      ╚═══════════════════════════════════════════╝
                                   ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
                            [Central Downward Lift Plume]
```

---

## Chapter 6 · Section S0: HUB Base (H4-H6)

The HUB base is the primary core module of the Floating Rig: the first structural volume printed by the Extruder Ring shipyard and the first section to fly. It carries the primary 900 MW nuclear power plant, the central lift ring, and the docking sockets for all 6 pylon arms.

### 6.0 · Reference Envelope & Master Geometry
* **Footprint:** Hexagonal prism, 18 B diameter across flats (45.0 m) × 12 B height (30.0 m, Levels L1 to L12).
* **Mass (dry base):** ≈ 7,200 t (including double-walled heavy armor citadel and reactor shielding).
* **Grid:** Large Grid (1 B = 2.5 m).
* **Seed Kit Avionics:** H6 lift thrusters, 6 Large Batteries, Remote Control on H5, 12 Attitude Gyroscopes, Fleet Radio Antenna.
* **Operating Environment:** Earth 1 g baseline, surface hover up to 18 km ceiling†.

```
Coordinate Triad & Axis Conventions:
       [+Y: Zenith / Up]
              │
              │   [+Z: North / Forward]
              │   /
              │  /
              │ /
  [−X: West] ─┼───────── [+X: East]
             /│
            / │
           /  │
  [−Z: South / Aft]
Origin (X0, Y0, Z0): Centerline of the Rig HUB Base, Level L1 at the lowest thruster nozzle plane (Y = 0).
```

#### 6.0.2 · In-Game Paint Palette & Functional Livery Map
| Slot | Color Name | Hex Code | RGB | In-Game Material | Structural Application |
|---|---|---|---|---|---|
| Color 0 | Citadel Charcoal | `#1A1D20` | 26, 29, 32 | Heavy Armor Plate | External citadel shell, reactor containment, pylon keels |
| Color 1 | Structural Slate | `#42464D` | 66, 70, 77 | Carbon / Clean Armor | Hangar bay walls, deck bulkheads, structural ribs |
| Color 2 | Warning Orange   | `#E06D14` | 224, 109, 20| Battered Armor | Hangar door perimeters, socket alignment guides |
| Color 3 | Tactical White   | `#D4D8DD` | 212, 216, 221| Clean Armor | Flight deck markings, drone alignment chevrons |
| Color 4 | Hazard Striping  | `#C8A114` | 200, 161, 20| Hazard Stripes | Thruster exclusion perimeters, lift ring coamings |
| Color 5 | Habitat White    | `#E8ECF0` | 232, 236, 240| Clean Interior | H1 Command Bridge, H2 Crew Citadel interior |
| Color 6 | Machine Olive    | `#4A5643` | 74, 86, 67 | Worn Metal | Refineries, assemblers, conveyor sorters |
| Color 7 | Cryo Cyan        | `#1E6B7B` | 30, 107, 123| Clean Armor | Hydrogen tanks, O2/H2 generators |
| Color 8 | Mechanical Gunmetal| `#2B2E33`| 43, 46, 51 | Corrugated Metal | Pylon socket merge heads, hinge pivots |
| Color 9 | Grate Steel      | `#50555C` | 80, 85, 92 | Grated Catwalk | Walkways, maintenance stairs, Jefferies tubes |

```
Exterior Livery Profile · HUB Base (H4-H6):
   L12 ┌────────────────────────────────────────────────────────┐ [Citadel Charcoal]
   L10 │   H4 NUCLEAR CITADEL (Double-Walled Heavy Armor)       │ [Slot 0]
   L8  ├────────────────────────────────────────────────────────┤
   L6  │ ◄[SOCKET P-W]   H5 PYLON SOCKET & GYRO RING  [P-E]►   │ [Structural Slate / Orange Trim]
   L4  ├────────────────────────────────────────────────────────┤ [Slots 1 & 2]
   L2  │   H6 CENTRAL LIFT RING (10× Atmo + 6× H2 Thrusters)    │ [Citadel Charcoal / Hazard Stripes]
   L1  └───────[▼▼▼]──────────────[▼▼▼]──────────────[▼▼▼]──────┘ [Slots 0 & 4]
```

---

### Step 6.1 · H6 Central Lift Ring (Levels L1-L4)

Mount the central lift cluster providing 19% of the Rig's total atmospheric lift (64.8 MN) and blackout emergency hover (43.2 MN H2).

```
Plan View · Y = L2 (H6 Central Lift Deck Core, 18 B Hexagon):
        X: −6   −4   −2    0   +2   +4   +6
    Z: +6  ···  ···  [A1] [H1] [A2] ···  ···
    Z: +4  ···  [A3] ···  [A4] ···  [A5] ···
    Z: +2  [A6] ···  [H2] ···  [H3] ···  [A7]
    Z:  0  ···  [H4] ···  [CC] ···  [H5] ···   ◄── [CC] Central Conveyor Well (3×3 B)
    Z: −2  [A8] ···  [H6] ···  ···  ···  [A9]
    Z: −4  ···  [A10]···  ···  ···  ···  ···
    Z: −6  ···  ···  ···  ···  ···  ···  ···
```

#### Pre-Step Parts Tray 6.1
* 10 × Large Atmospheric Thruster (large grid, 6.48 MN each = 64.8 MN total) · `RIG H6 Lift Atmo 1..10` → group `RIG Lift Atmo HUB`
* 6 × Large Hydrogen Thruster (large grid, 7.20 MN each = 43.2 MN total) · `RIG H6 Lift H2 1..6` → group `RIG Lift H2 HUB`
* 72 × Heavy Armor Slopes & Inverted Corners framing thruster exhaust nozzles.
* 12 × Heavy Armor Blast Deflectors (Level L1 nozzle rim protection).

#### Placement Instructions
1. Position 10× Large Atmospheric Thrusters pointing straight down (−Y) with 4-block exhaust clearance.
2. Interleave 6× Large Hydrogen Thrusters pointing straight down (−Y) adjacent to central conveyor well.
3. Frame thruster cluster with heavy armor triangular webs linking outer hexagonal rim to the central spine.
4. Route 2× dual large conveyor loops from central well to H2 thruster fuel inlet ports.

**✓ Check:** Nozzles face straight down (D). Center of lift aligns within 0.1 m of the central vertical axis (0, 0).

---

### Step 6.2 · H5 Junction, Gyroscope Deck & Pylon Socket Ring (Levels L5-L8)

Houses the six pylon docking sockets, master conveyor trunks, attitude gyroscopes, and avionics.

```
Plan View · Y = L6 (H5 Hexagonal Pylon Socket Deck):
                 [SOCKET P-NW]            [SOCKET P-NE]
                        ╲                      ╱
                         ┌────────────────────┐
          [SOCKET P-W] ══╡  TRUNK RING LOOPS  ╞══ [SOCKET P-E]
                         │  12× LARGE GYROS   │
                         │  [RC SEED] [RADIO] │
                         └────────────────────┘
                        ╱                      ╲
                 [SOCKET P-SW]            [SOCKET P-SE]
```

#### Pre-Step Parts Tray 6.2
* 12 × Merge Block (Large Grid, 2 per socket face) · `RIG H5 Socket P-E MRG 1, 2` …
* 6 × Large Grid Connector (1 per socket center) · `RIG H5 Socket P-E Conn` …
* 12 × Large Gyroscope (attentive horizon stabilization) · `RIG H5 Gyro 1..12` → group `RIG Gyros`
* 2 × Conveyor Loop Ring · Trunk A (WNW spine) and Trunk B (ESE spine).
* 1 × Remote Control (`RIG H5 RC Seed`) + 1× Radio Antenna (`RIG H5 Antenna 50km`).
* 6 × Spark Array Blocks (visual docking alignment rungs).

#### Placement Instructions
1. Construct 6 structural socket bulkheads facing exactly 0°, 60°, 120°, 180°, 240°, and 300°.
2. Each socket presents an 8 W × 7 H receiving face matching the pylon root collar:
   * 2× Merge Blocks at top corners (L7, X ±3 relative to pylon centerline).
   * 1× Large Grid Connector dead-center at L4 (conveyor and fuel transfer).
   * 2× Heavy Armor Guide Horns (sloped coamings) guiding incoming pylon during docking.
3. Install 12× Large Gyroscopes inside the central equipment core.
4. Mount Remote Control block oriented Forward (+Z / North) and Up (+Y / Zenith).

**✓ Check:** Each of the 6 sockets presents an 8 W × 7 H receiving face with 2 Merge Blocks and 1 central Large Connector.

---

### Step 6.3 · H4 Nuclear Power Vault (Levels L9-L12)

Houses three Large Nuclear Reactors enclosed in compartmentalized double-walled heavy armor vaults.

```
Plan View · Y = L10 (H4 Nuclear Reactor Deck):
         X: −5    −2     0    +2    +5
     Z: +4  ···   ···   [R1]  ···   ···    ◄── Large Nuclear Reactor 1 (300 MW)
     Z: +2  ···  [BAT]   │   [BAT]  ···
     Z:  0 [BAT]  ─── [CORE] ───  [BAT]   ◄── [CORE] Conveyor Fuel Feed
     Z: −2  ···  [BAT]   │   [BAT]  ···
     Z: −4  [R2]  ···   ···   ···   [R3]   ◄── Reactors 2 & 3 (300 MW each)
```

#### Pre-Step Parts Tray 6.3
* 3 × Large Nuclear Reactor (300 MW each = 900 MW total) · `RIG H4 Reactor 1, 2, 3` → group `RIG Reactors`
* 6 × Large Battery (emergency buffer, 18 MWh total) · `RIG H4 Battery 1..6` → group `RIG Batteries`
* 2 × Hydrogen Engine (auxiliary cold-start, 10 MW) · `RIG H4 H2 Engine 1, 2`
* 96 × Heavy Armor Blocks & Composite Blast Bulkheads.

#### Placement Instructions
1. Seat 3× Large Reactors on isolated structural heavy armor pads at 120° offsets.
2. Build double-walled heavy armor bulkheads separating each reactor compartment.
3. Route dedicated uranium fuel conveyor lines with redundant sorters from central trunk.
4. Ring the reactor core with 6× Large Batteries providing instantaneous UPS buffer during peak loads.

#### Pre-Enclosure Inspection Gate 6.1A: H4-H6 HUB Base Mechanical, Power & Fueling Integrity
Prior to authorizing extruder launch and autonomous flight of Section S0:
* [ ] **Conveyor Loop Integrity:** Confirm Trunk A and Trunk B maintain closed loops with 0 unpiped ports.
* [ ] **Reactor Full-Load Test:** Spin up Reactors 1-3 to 100% capacity (900 MW); confirm zero overload on 52 Atmo thrusters.
* [ ] **Blackout Battery UPS:** Disconnect all reactors; verify 6× Large Batteries sustain H6 hover for ≥ 90 seconds.
* [ ] **Socket Lock Geometry:** Confirm all 12 merge blocks and 6 connectors toggle Lock/Unlock cleanly.

---

## Chapter 7 · Sections S1-S6: Pylon Arms & Modular Bays

Six identical cantilevered arms (8 W × 7 H × 22 L blocks ≈ 20.0 × 17.5 × 55.0 m) radiating from the H5 sockets.

### 7.0 · Pylon Architecture & Clear Envelopes
* **Build Symmetry:** Pylons printed sequentially by the Extruder Ring shipyard, cold-launched from the skid runway, and flown autonomously to dock at H5.
* **Structural Envelope:**
  * Levels L1-L2: Reinforced Keel (Trunk A & B conveyors, 2× Large H2 tanks, 3× Large Atmo thrusters).
  * Levels L3-L6: Modular Hangar Deck (3 universal bays, 6 W × 4 H × 5 L clear box each).
  * Level L7: Armored Roof Deck (CIWS turret platforms, crew access gallery).
  * Tip Cross-Head (Levels L1-L7, Z +17 to +22): Plus-shaped end cap with 4× Large Atmo lift thrusters, 2× Interceptor bays, and defense turrets.

```
Transverse Cross-Section · Pylon Arm (Looking Forward toward Cross-Head Tip Z = +10):
   L7   [CIWS] ■═══════════════[ROOF GALLERY]═══════════════■ [CIWS]
   L6   ║  ┌──────────────────────────────────────────────┐  ║
   L5   ║  │       MODULAR HANGAR BAY (6 W × 4 H B)       │  ║ [Outer Armor Plate]
   L4   ║  │       Clear Volume: 15.0 × 10.0 × 12.5 m     │  ║
   L3   ║  └──────────────────────────────────────────────┘  ║
   L2   ║  [TRUNK A]   [LARGE H2 TANK 1]   [LARGE H2 TANK 2]  ║ [TRUNK B]
   L1   ■═════════════════════[V-KEEL ARMOR]═════════════════■
               [▼▼▼]                               [▼▼▼]
         [Keel Lift Atmo 1]                  [Keel Lift Atmo 2]
```

---

### Step 7.1 · Pylon Keel & Conveyor Trunks (Levels L1-L2)

```
Plan View · Y = L2 (Pylon Keel & Fuel Foundation):
        Z: 0 (Root)      5          10          15       20 (Tip)
   X: -3   [TRUNK A]═════[TRUNK A]═════[TRUNK A]═════[TRUNK A]════► [Cross-Head]
   X: -1   [MRG] [H2 TANK 1 (5.0 m)]   [ATMO LIFT 1] [H2 LIFT 1]
   X: +1   [MRG] [H2 TANK 2 (5.0 m)]   [ATMO LIFT 2] [H2 LIFT 2]
   X: +3   [TRUNK B]═════[TRUNK B]═════[TRUNK B]═════[TRUNK B]════► [Cross-Head]
```

#### Pre-Step Parts Tray 7.1
* 2 × Continuous Large Conveyor lines (Trunk A on Port, Trunk B on Starboard).
* 2 × Large Hydrogen Tank (5.0 m diameter, 5,000,000 L capacity each) · `RIG P<n> Tank 1, 2`.
* 3 × Large Atmospheric Thruster (D) · `RIG P<n> Lift Atmo 1..3` (embedded in keel flank pockets).
* 2 × Large Hydrogen Thruster (D) · `RIG P<n> Lift H2 1, 2` (keel center).
* 44 × Heavy Armor Sloped Blocks forming deflective V-hull keel.

#### Placement Instructions
1. Lay heavy armor V-keel along bottom centerline (Level L1).
2. Seat twin Large H2 tanks end-to-end between Z +2 and Z +12 along the pylon centerline.
3. Flank tanks with continuous Large Conveyor Trunks A and B at X ±3.
4. Recess 3× Large Atmo lift thrusters into downward-facing keel pockets.

**✓ Check:** Both conveyor trunks run uninterrupted from root collar to tip cross-head.

---

### Step 7.2 · Modular Hangar Bays (Levels L3-L6, Bays 1-3)

Three universal drone bays per arm, opening alternately into flanking T-Sectors.

```
Plan View · Y = L4 (Pylon Hangar Level):
        Z: 0 (Root)      6           11          16       22 (Tip)
           ┌──────────┬───────────┬───────────┬───────────┬───────┐
    Port   │  COLLAR  │ [BAY 1]   │ Bay Wall  │ [BAY 3]   │ CROSS │
           │  (2 B)   │ (Door P)  │           │ (Door P)  │ HEAD  │
           ├──────────┤           ├───────────┤           │ (6 B) │
    Stbd   │  SOCKET  │ Bay Wall  │ [BAY 2]   │ Bay Wall  │       │
           │  COLLAR  │           │ (Door S)  │           │       │
           └──────────┴───────────┴───────────┴───────────┴───────┘
```

#### Pre-Step Parts Tray 7.2 (Per Pylon · ×3 Bays)
* 3 × Large Grid Connector (ceiling recessed) · `RIG P<n> Bay<m> Conn`.
* 3 × Ship Welder (maintenance & repair) · `RIG P<n> Bay<m> Welder`.
* 3 × Sensor Block (approach automation) · `RIG P<n> Bay<m> Sensor`.
* 12 × Airtight Hangar Door Blocks · `RIG P<n> Bay<m> Doors`.
* 3 × Spotlight (docking lighting) · `RIG P<n> Bay<m> Light`.

#### Placement Instructions
1. Bay 1 opens Port (Z +2 to +7); Bay 2 opens Starboard (Z +7 to +12); Bay 3 opens Port (Z +12 to +17).
2. Install 4× Hangar Door blocks along outer opening, sliding flush into ceiling frame.
3. Mount ceiling connector recessed 1 block above clear envelope with guide coamings.
4. Mount ship welder flush behind protective grated floor plate.

**✓ Check:** Clear interior: 6 B wide × 4 B high × 5 B deep (15.0 × 10.0 × 12.5 m). Accommodates Remora, Kestrel, or Albatross.

---

### Step 7.3 · Pylon Lift Pods & Tip Cross-Head (Levels L1-L7)

```
Plan View · Y = L4 (Tip Cross-Head Pod, Z +17 to +22):
         X: −4    −2     0    +2    +4
     Z: +22 ···  [CIWS] [FWD] [CIWS] ···    ◄── Tip Forward Defense & Prow
     Z: +20 [LAT] [A4]  [COR]  [A5] [LAT]   ◄── Lateral Atmo + Tip Lift Thrusters
     Z: +18 ···   [A6]  [COR]  [A7]  ···    ◄── 4× Large Atmo Lift Pods (25.92 MN)
     Z: +16 ···   [H3]  [COR]  [H4]  ···    ◄── 2× Large H2 Emergency Lift (14.4 MN)
```

#### Pre-Step Parts Tray 7.3
* 4 × Large Atmospheric Thruster (D) · `RIG P<n> Lift Atmo 4..7` (cross-head cluster, 25.92 MN).
* 2 × Large Hydrogen Thruster (D) · `RIG P<n> Lift H2 3, 4` (cross-head emergency boost, 14.4 MN).
* 4 × Small Atmospheric Thruster · `RIG P<n> Thruster Lat 1..4` (lateral stationkeeping).
* 2 × Interceptor Hatch Bays (roof launch tubes).
* 2 × Gatling CIWS Turrets (1 dorsal at L7, 1 ventral at L1).

#### Placement Instructions
1. Construct the plus-shaped cross-head end cap projecting 4 blocks beyond hangar bay 3.
2. Cluster 4× Large Atmo lift thrusters in symmetrical 2×2 box around central corridor.
3. Mount dorsal and ventral CIWS turrets providing 360° hemispherical defense over pylon tip.
4. Install lateral stationkeeping thrusters on outer beam facets.

#### Pre-Enclosure Inspection Gate 7.1A: Flown Pylon Flight Authority & Docking Handshake
Prior to authorizing extruder merge release and autonomous flight of any Pylon:
* [ ] **Lift Authority Proof:** Under 100% test override, 7× Large Atmo thrusters deliver 45.36 MN upward force, producing T/W ≥ 1.35 against pylon dry mass (≈ 2,900 t).
* [ ] **Fuel Capacity:** Verify twin Large H2 tanks register 100% fill level (10,000,000 L).
* [ ] **Autonomous Docking Telemetry:** Remote Control locks onto target H5 socket beacon; merge blocks arm cleanly.

---

## Chapter 8 · Sections S7-S9: Upper Decks (H1-H3)

Stacked vertically atop the H4 Nuclear Vault to complete the Rig citadel.

### 8.0 · Upper Citadel Layout & Master Elevation
* **Section S7 (Level L13-L16 · H3):** Industrial Core, O2/H2 Gas Farm & Heavy Maintenance Shop.
* **Section S8 (Level L17-L20 · H2):** Crew Citadel, Medical Center, Hydroponics & Life Support.
* **Section S9 (Level L21-L24 · H1):** Master Armored Command Bridge, Avionics Citadel & Roof Picket Deck.

```
Longitudinal Centerline Profile · Upper Citadel (H1-H3):
   L24 ┌───────────────────[ROOF PICKET FLIGHT DECK]───────────────────┐
   L22 │   H1 MASTER COMMAND BRIDGE · 4× RIG//OS PROGRAMMABLE BLOCKS   │ [Section S9]
   L20 ├───────────────────────────────────────────────────────────────┤
   L18 │   H2 CREW CITADEL · MEDICAL BAY · CRYO PODS · GRAVITY CORE    │ [Section S8]
   L16 ├───────────────────────────────────────────────────────────────┤
   L14 │   H3 GAS FARM (8× H2 TANKS) · 4× O2/H2 GENS · CORE SHOP      │ [Section S7]
   L12 └───────────────────[H4 NUCLEAR CORE CITADEL]───────────────────┘
```

---

### Step 8.1 · Section S7: H3 Gas Farm & Core Machine Shop (Levels L13-L16)

```
Plan View · Y = L14 (H3 Gas Farm & Machine Shop):
         X: −5    −2     0    +2    +5
     Z: +4  [GEN1] [H2 TANK 1..4]   [GEN2]
     Z: +2  [ASM1]  ─── [CORE] ───  [ASM2]  ◄── 2× Advanced Assemblers (400% speed)
     Z:  0  [REF1]  ─── [WELL] ───  [REF2]  ◄── 2× Industrial Refineries (200% yield)
     Z: −2  [ASM3]  ─── [CORE] ───  [ASM4]
     Z: −4  [GEN3] [H2 TANK 5..8]   [GEN4]
```

#### Pre-Step Parts Tray 8.1
* 8 × Large Hydrogen Tank (gas farm reserve, 40,000,000 L total) · `RIG H3 Tank 1..8`.
* 4 × Large O2/H2 Generator · `RIG H3 O2H2 Gen 1..4`.
* 2 × Refinery (Yield modules) · `RIG H3 Refinery 1, 2`.
* 4 × Assembler (Speed modules) · `RIG H3 Assembler 1..4`.
* 1 × Large Cargo Container (`RIG H3 Hold Components`).

#### Placement Instructions
1. Install 8× Large H2 tanks in symmetrical perimeter rings.
2. Center 4× O2/H2 generators adjacent to central conveyor well.
3. Install compact maintenance refinery and assembler cells for autonomous parts replenishment.

**✓ Check:** Gas farm plumbed into Trunk A & B loops. Ice processing capacity ≥ 2,000 kg/s.

---

### Step 8.2 · Section S8: H2 Crew Citadel, Medical Center & Life Support (Levels L17-L20)

```
Plan View · Y = L18 (H2 Crew Citadel Deck):
         X: −5    −2     0    +2    +5
     Z: +4  [CREW QUARTERS]   │   [HYDROPONICS 1]
     Z: +2  [MESS HALL]     [AIR] [HYDROPONICS 2]
     Z:  0  [MED BAY]      [GRAV] [O2 TANKS 1..2]  ◄── [GRAV] Artificial Gravity Core
     Z: −2  [CRYO PODS 1-4] [AIR] [SURVIVAL KIT]
     Z: −4  [ARMORY LOCKERS]  │   [DECON AIRLOCK]
```

#### Pre-Step Parts Tray 8.2
* 1 × Medical Room (`RIG H2 MedBay`) + 4× Cryo Chambers (`RIG H2 Cryo 1..4`).
* 1 × Gravity Generator (`RIG H2 GravGen`, 1.0 g downward acceleration).
* 2 × Large O2 Tank (200,000 L life support reserve) · `RIG H2 Tank O2 1, 2`.
* 4 × Air Vent (hermetic pressurization) · `RIG H2 Vent 1..4`.
* 2 × Armory Lockers + 1× Survival Kit.

#### Placement Instructions
1. Mount Gravity Generator dead-center at X0, Z0, L18 to provide uniform 1.0 g across H1-H3.
2. Construct hermetic crew quarters and medical triage suite on Port flank.
3. Install automated gas-reclamation decontamination airlock at elevator stairwell head.

**✓ Check:** H2 Citadel holds 100% O2 pressure (1.00 atm) with zero leakage when sealed.

---

### Step 8.3 · Section S9: H1 Master Command Bridge, Avionics & Roof Pickets (Levels L21-L24)

```
Plan View · Y = L22 (H1 Master Command Bridge):
         X: −5    −2     0    +2    +5
     Z: +4  ···  [C2-1] [HELM] [C2-2] ···   ◄── Forward Panoramic Armored Glazing
     Z: +2  ···  [PB-1] [PB-2] [PB-3] ···   ◄── [PB 1-4] RIG//OS MDK2 Compute Rack
     Z:  0  ···  [PB-4] [DISP] [SEAT] ···   ◄── Central Command Consoles
     Z: −2  ···  [RAD1] [WELL] [RAD2] ···   ◄── Avionics Comms Racks
     Z: −4  ···  ···   [AIRLOCK] ···  ···   ◄── Stairwell to Roof Flight Deck
```

#### Pre-Step Parts Tray 8.3
* 1 × Main Cockpit / Helm (`RIG H1 Helm Master`).
* 2 × Operator Consoles (`RIG H1 Console Fleet`, `RIG H1 Console Yard`).
* 4 × Programmable Block (`[RIG] PB Rig OS`, `[RIG] PB Dispatcher`, `[RIG] PB Inventory`, `[RIG] PB Displays`).
* 2 × Picket Drone Landing Pads (Level L24 roof) with recessed connectors (`RIG Roof Pad 1, 2 Conn`).
* 4 × Point Defense Gatling Turrets (`RIG Turret Roof 1..4`).
* 1 × Laser Antenna (`RIG H1 Laser C2`) + 1× Radio Antenna (`RIG H1 Antenna 50km`).

#### Placement Instructions
1. Center 3-operator bridge forward at Z +4 overlooking North approaches through sloped heavy glass.
2. Install 4-rack Programmable Block computer core in shielded armored avionics bay.
3. Finish Level L24 roof deck with twin Picket Drone landing pads and 360° CIWS defense perimeter.

#### Pre-Enclosure Inspection Gate 8.1A: Citadel Pressurization, Avionics & C2 Relay Integrity
* [ ] **Full-Hull Pressurization:** Confirm H1, H2, and H3 maintain 100% O2 pressure indefinitely.
* [ ] **Gravity Alignment:** Confirm Gravity Generator produces exactly 1.0 g downward across all bridge consoles.
* [ ] **C2 Laser Uplink:** Establish laser link to Ground Base C2 spire with zero packet loss.

---

## 8.4 · Master Electrical Single-Line & Power Modes

### 1. Power Generation Hierarchy
* Primary Base: 3 × Large Nuclear Reactor (H4) = 900.0 MW.
* Auxiliary Support: 2 × Hydrogen Engine (H4) = 10.0 MW.
* Dynamic Buffer: 18 × Large Battery (6 in HUB, 2 per Pylon) = 54.0 MWh capacity (216.0 MW peak surge).
* **Total Peak Power Available:** 1,126.0 MW.

### 2. Operational Energy Modes
| Mode | Operational State | Total Generation | Total Load | Net Power Margin | Primary Source |
|---|---|---|---|---|---|
| Mode 1 | Static Stationkeeping Hover | 900.0 MW | 638.0 MW | +262.0 MW | 3× Nuclear Reactors |
| Mode 2 | High-Speed Flight Transit / Climb | 1,116.0 MW | 780.0 MW | +336.0 MW | Reactors + Battery Assist |
| Mode 3 | Silent Loiter / Minimum Power | 216.0 MW | 45.0 MW | +171.0 MW | 18× Batteries Only |
| Mode 4 | Full Fleet Refit & Production | 900.0 MW | 750.0 MW | +150.0 MW | Reactors (O2/H2 + Asm) |
| Mode 5 | Battle Stations / N-1 Failover | 600.0 MW | 638.0 MW | −38.0 MW (UPS on Bat) | 2× Reactors + Batteries |

---

## 8.5 · Master BOM & Earth 1 g Lift Physics Budget (Decision 48)

### 1. Master Bill of Materials (Complete 25 kt Fortress)
| Subsystem / Section | Block Count | Dry Mass (t) | PCU | Primary Thrusters | Power Generation |
|---|---|---|---|---|---|
| S0 HUB Base (H4-H6) | 1,420 | 7,200.0 | 8,500 | 10 Atmo + 6 H2 | 3× Large Reactors (900 MW) |
| S1-S6 Pylons (×6 arms) | 2,880 (480 ea) | 14,400.0 (2,400 ea) | 16,800 (2,800 ea) | 42 Atmo + 24 H2 (7 A + 4 H ea) | 12 Large Batteries (36 MWh) |
| S7-S9 Upper Citadel (H1-H3)| 680 | 3,400.0 | 4,200 | None | 2 H2 Engines (10 MW) |
| **Complete Floating Rig** | **4,980** | **25,000.0 t** | **29,500** | **52 Large Atmo + 30 Large H2** | **910 MW Nuclear/H2 + 54 MWh Bat** |

### 2. Earth 1 g Lift Physics Budget
* **Fortress Gross Mass ($M$):** 25,000,000 kg (25,000 t).
* **Gravity Force ($F_g$):** $25,000,000 \text{ kg} \times 9.81 \text{ m/s}^2 = 245.25 \text{ MN}$.
* **Atmospheric Lift Array (52× Large Atmo Thrusters):**
  * HUB H6: 10 × 6.48 MN = 64.80 MN.
  * Pylons S1-S6: 6 × (7 × 6.48 MN) = 272.16 MN.
  * Total Atmo Downward Thrust = 336.96 MN.
  * **Hover Throttle Margin:** $245.25 \text{ MN} / 336.96 \text{ MN} = 72.78\% \approx 72.8\%^\dagger$ (Decision 48 verified).
* **Emergency Hydrogen Lift Array (30× Large H2 Thrusters):**
  * HUB H6: 6 × 7.20 MN = 43.20 MN.
  * Pylons S1-S6: 6 × (4 × 7.20 MN) = 172.80 MN.
  * Total Hydrogen Downward Thrust = 216.00 MN.
* **Combined All-Thruster Maximum Lift:** $336.96 \text{ MN} + 216.00 \text{ MN} = 552.96 \text{ MN}$.
* **Maximum Emergency T/W Ratio:** $552.96 \text{ MN} / 245.25 \text{ MN} = 2.254$ (Earth 1 g).

---

## 8.6 · Master AI Scripting Contract & Terminal Namespace (Claude Code / RIG//OS)

Every device on the Floating Rig conforms to this exact naming and grouping dictionary:

### 1. Group Taxonomy
* `RIG Lift Atmo HUB`: 10× Large Atmospheric Thrusters on H6.
* `RIG Lift Atmo Pylons`: 42× Large Atmospheric Thrusters on Pylons S1-S6.
* `RIG Lift Atmo All`: All 52× Large Atmospheric Thrusters.
* `RIG Lift H2 All`: All 30× Large Hydrogen Thrusters.
* `RIG Reactors`: 3× Large Nuclear Reactors on H4.
* `RIG Batteries`: All 18× Large Batteries across HUB and Pylons.
* `RIG Gyros`: All 24× Large Gyroscopes.
* `RIG Pylon Sockets`: All 12× Merge Blocks and 6× Connectors on H5.

### 2. Block Naming & Terminal Dictionary
| Block Type | Exact Terminal Name | Subsystem Role & Functional Configuration |
|---|---|---|
| Large Atmo Thruster | `RIG H6 Lift Atmo 1` … `10` | Central HUB primary lift array |
| Large H2 Thruster | `RIG H6 Lift H2 1` … `6` | Central HUB emergency lift boost |
| Large Atmo Thruster | `RIG P<1..6> Lift Atmo 1` … `7`| Pylon primary lift thrusters (keel + tip pod) |
| Large H2 Thruster | `RIG P<1..6> Lift H2 1` … `4` | Pylon emergency lift boost |
| Large Nuclear Reactor | `RIG H4 Reactor 1` … `3` | Primary 900 MW heavy nuclear power plant |
| Large Battery | `RIG H4 Battery 1` … `6` | HUB core UPS emergency buffer |
| Large Battery | `RIG P<1..6> Battery 1` … `2` | Pylon independent flight power buffer |
| Merge Block | `RIG H5 Socket P-<Dir> MRG 1, 2`| Pylon root structural locking merge heads |
| Large Connector | `RIG H5 Socket P-<Dir> Conn` | Pylon root fuel and conveyor umbilical connector |
| Programmable Block | `[RIG] PB Rig OS` | Core state machine, hover maintenance, mode selection |
| Programmable Block | `[RIG] PB Dispatcher` | Flight bay traffic control, drone lease orchestration |
| Programmable Block | `[RIG] PB Inventory` | Fleet quota tracking, refinery sorting, assembler queues |
| Programmable Block | `[RIG] PB Displays` | Zero-GC monospace bridge and deck telemetry dashboards |

### 3. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `RIG EC Lift` | Grid Vertical Velocity | Velocity < −2.0 m/s (Altitude Loss) | `RIG Lift H2 All` ON · Throttle = 100% | `RIG Lift H2 All` OFF (Atmo hover only) |
| `RIG EC Power` | `RIG H4 Reactor 1..3` | Combined Output < 600 MW (N-1 Event)| `RIG Batteries` Set Discharge · Sound Alert | `RIG Batteries` Set Auto |
| `RIG EC Hull` | Compartment Air Vents | Pressure < 0.50 atm (Hull Breach) | Lock Emergency Bulkhead Doors · Klaxon ON | Bulkhead Doors Auto |

### 4. IGC Channel Subscriptions & Broadcast Protocols
* Inbound: `RIG.OS.COMMAND` (`MODE;<name>`, `LIFT;<pct>`, `EMERGENCY`, `RELOAD`).
* Inbound: `TMS.BASE.HANDOVER` (C2 root election and operational transfer).
* Outbound: `RIG.OS.STATUS` (Broadcasts `HOVER;<altitude>;<atmo_throttle_pct>;<power_margin_mw>`).
* Outbound: `RIG.OS.HEARTBEAT` (Cyclic 1.0 s watchdog ping to fleet).

---

## 8.7 · Acceptance Testing Protocols (Suites A-J)

* [ ] **Suite A (Static Hover Stability):** Hover at 500 m AGL; confirm atmospheric throttle sits within 72.8% ± 1.5%† at sea level Earth 1 g.
* [ ] **Suite B (N-1 Nuclear Loss Failover):** Power down Reactor 1; confirm Batteries instantly supply shortfall without losing altitude.
* [ ] **Suite C (Pylon Cold Docking Handshake):** Disconnect Pylon S1; fly 50 m clear; return and dock under Remote Control with 0 subgrid damage.
* [ ] **Suite D (Simultaneous Swarm Launch):** Cycle 6 modular hangar bays simultaneously; verify zero conveyor lockups or door conflicts.
* [ ] **Suite E (Atmospheric Ceiling Climb):** Ascend to 18 km ceiling†; confirm smooth transition to H2 emergency thrusters as atmospheric density decays.
