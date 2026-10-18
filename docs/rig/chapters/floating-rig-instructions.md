# Volume III · Floating Rig Structure (25 kt Fortress)

This volume details the physical construction of the Floating Rig: an aerial fortress stationed at Earth 1 g.

```
Rig Master Elevation (Looking North):
                       [H1 Command & Roof Pickets]          (S9)
                       [H2 Habitat & Life Support]          (S8)
                       [H3 Gas Farm & Core Shop]            (S7)
  ◄── Pylon S1 ──► ╔═════════════════════════════════╗ ◄── Pylon S2 ──►
  (8 W × 7 H × 22 L)║ H4 Power Core (3× Large React) ║ (8 W × 7 H × 22 L)
                    ║ H5 Pylon Sockets & Gyros       ║
                    ║ H6 Lift Ring (10 A + 6 H)      ║
                    ╚═════════════════════════════════╝
                                 ▼▼▼▼▼
```

---

## Chapter 6 · Section S0: HUB Base (H4–H6)

The HUB base is the first section welded and the first to fly. It carries the primary power plant, main lift ring, and the docking sockets for all 6 pylons.

### 6.0 · Reference Envelope & Master Geometry
* **Footprint:** Hexagonal core, 18 B diameter (45 m) × 12 B height (30 m).
* **Mass (dry base):** ≈ 7,200 t (including heavy armor shielding).
* **Seed Kit:** H6 lift thrusters, 4 Large Batteries, Remote Control on H5, 6 Gyroscopes, Antenna.

---

### Step 6.1 · H6 Lift Ring (Levels L1–L4)

Mount the central lift cluster providing 19% of the Rig's total atmospheric lift and blackout emergency hover.

```
Plan view, Y = L2 (Lift Deck Core)
        X: −4   −2    0   +2   +4
    Z: +4  ···  [A]  [H]  [A]  ···
    Z: +2  [A]  ···  [A]  ···  [A]
    Z:  0  [H]  [A]  [H]  [A]  [H]   ◄── 10× Large Atmo [A] + 6× Large H2 [H]
    Z: −2  [A]  ···  [A]  ···  [A]
    Z: −4  ···  [A]  [H]  [A]  ···
```

**Blocks:**
* 10 × Large Atmospheric Thruster (large grid, 6.48 MN each = 64.8 MN total) · `RIG H6 Lift Atmo 1..10` → group `RIG Lift Atmo HUB`
* 6 × Large Hydrogen Thruster (large grid, 7.2 MN each = 43.2 MN total) · `RIG H6 Lift H2 1..6` → group `RIG Lift H2 HUB`
* 72 × Heavy Armor Slopes & Corners framing the thruster exhaust nozzles.

**✓ Check:** Nozzles face straight down (D). Center of lift aligns within 0.1 m of the central vertical axis (0, 0).

---

### Step 6.2 · H5 Junction & Pylon Socket Deck (Levels L5–L8)

Houses the six pylon docking sockets, master conveyor trunks, and attitude gyroscopes.

```
Plan view, Y = L6 (Socket Ring)
             [SOCKET P-NW]       [SOCKET P-NE]
                    ╲                 ╱
                     ┌───────────────┐
      [SOCKET P-W] ══╡  TRUNK RING   ╞══ [SOCKET P-E]
                     │  12× GYROS    │
                     └───────────────┘
                    ╱                 ╲
             [SOCKET P-SW]       [SOCKET P-SE]
```

**Blocks:**
* 12 × Merge Block · 2 per socket (`RIG H5 Socket P-E MRG 1, 2` …)
* 6 × Large Grid Connector · 1 per socket center (`RIG H5 Socket P-E Conn` …)
* 2 × Conveyor Loop Ring · Trunk A (WNW spine) and Trunk B (ESE spine) feeding every deck
* 12 × Large Gyroscope (large grid) · `RIG H5 Gyro 1..12` → group `RIG Gyros`
* 6 × Spark Array Blocks (visual docking rungs)

**✓ Check:** Each of the 6 sockets presents an 8 W × 7 H receiving face with 2 Merge Blocks and 1 central Large Connector.

---

### Step 6.3 · H4 Nuclear Power Vault (Levels L9–L12)

Houses three Large Nuclear Reactors enclosed in double-walled heavy armor vaults.

```
Plan view, Y = L10 (Reactor Deck)
        X: −4    0   +4
    Z: +3  ···  [R1] ···
    Z: −2  [R2] ···  [R3]   ◄── 3× Large Nuclear Reactors (300 MW each = 900 MW)
```

**Blocks:**
* 3 × Large Nuclear Reactor · `RIG H4 Reactor 1, 2, 3` → group `RIG Reactors`
* 6 × Large Battery (emergency buffer) · `RIG H4 Battery 1..6` → group `RIG Batteries`
* 2 × Hydrogen Engine (auxiliary power) · `RIG H4 H2 Engine 1, 2`
* Double-walled Heavy Armor bulkheads isolating each reactor compartment with blast doors.

**✓ Check:** Total power output = 900 MW. $N-1$ loss leaves 600 MW, covering normal station hover with battery assistance.

---

## Chapter 7 · Sections S1–S6: Pylon Arms & Modular Bays

Six identical cantilevered arms (8 W × 7 H × 22 L blocks ≈ 20 × 17.5 × 55 m) radiating from the H5 sockets.

### 7.0 · Pylon Architecture & Clear Envelopes
* **Build Symmetry:** P-E, P-NE, and P-NW printed by extruder; opposite 3 rotated 180°.
* **Cross-Section:**
  * L1–L2: Reinforced Keel (Trunk A & B conveyors, 2 Large H2 tanks).
  * L3–L6: Hangar Bay Deck (3 modular bays, 6 W × 4 H × 5 L clear box each).
  * L7: Armored Roof Deck (Turret platforms, access gallery).
* **Cross-head Tip:** Plus-shaped end cap with lateral thrusters, 2 Interceptor bays, and defense turrets.

---

### Step 7.1 · Pylon Keel & Conveyor Trunks (L1–L2)

```
Cross-section, looking toward cross-head tip (Z = +10)
    L2   [TRUNK A]  [LARGE H2 TANK 1]  [LARGE H2 TANK 2]  [TRUNK B]
    L1   ■════════════════════════════════════════════════════════■  Keel Armor
```

**Blocks:**
* 2 × Continuous Large Conveyor lines (Trunk A on Port side, Trunk B on Stbd side)
* 2 × Large Hydrogen Tank · `RIG P1 Tank 1, 2` (embedded in center keel)
* 44 × Heavy Armor Sloped Blocks forming a deflective V-hull keel.

**✓ Check:** Both conveyor trunks run uninterrupted from the root socket collar to the tip cross-head.

---

### Step 7.2 · Modular Hangar Bays (L3–L6, Bays 1–3)

Three universal bays per arm, opening alternately into the flanking T-Sectors.

```
Plan view, Y = L4 (Hangar Level)
        Z: 0 (Root)      6           11          16       22 (Tip)
           ┌──────────┬───────────┬───────────┬───────────┬───────┐
    Port   │  COLLAR  │ [BAY 1]   │ Bay Wall  │ [BAY 3]   │ CROSS │
           │  (2 B)   │ (Door P)  │           │ (Door P)  │ HEAD  │
           ├──────────┤           ├───────────┤           │ (6 B) │
    Stbd   │  SOCKET  │ Bay Wall  │ [BAY 2]   │ Bay Wall  │       │
           │  COLLAR  │           │ (Door S)  │           │       │
           └──────────┴───────────┴───────────┴───────────┴───────┘
```

**Blocks per Bay (×3 bays per arm):**
* 1 × Large Grid Connector (wall or ceiling mounted) · `RIG P<n> Bay<m> Conn`
* 1 × Ship Welder (recessed behind grate) · `RIG P<n> Bay<m> Welder` (repair & maintenance)
* 1 × Sensor · `RIG P<n> Bay<m> Sensor` (detects incoming drone at 10 m)
* 4 × Hangar Door Blocks · `RIG P<n> Bay<m> Doors` (slides into roof/floor)
* 1 × Spotlight · `RIG P<n> Bay<m> Light` (switched by sensor)

**✓ Check:** Clear bay interior: 6 B wide × 4 B high × 5 B deep. Drone length $\le$ 5 B clears all door frames.

---

### Step 7.3 · Pylon Lift Pods & Tip Cross-Head (L1–L7)

Mount 7 Large Atmo thrusters and 4 Large H2 thrusters along the pylon span.

**Thruster Allocation per Pylon:**
* 7 × Large Atmospheric Thruster (D) · 3 along keel flanks + 4 in tip cross-head pod (45.36 MN lift).
* 4 × Large Hydrogen Thruster (D) · embedded in keel (28.8 MN emergency lift).
* 2 × Interceptor Bay (roof hatches) in the plus-shaped cross-head tip.
* 2 × Defense Turrets (1 top, 1 bottom) at the cross-head extremity.

**✓ Check:** Combined pylon lift = 45.36 MN Atmo (at sea level). Pylon can hover independently during transit to the HUB.

---

## Chapter 8 · Sections S7–S9: Upper Decks (H1–H3)

Stacked on the base by the Rig's construction projector or assembled in place.

### Step 8.1 · Section S7: H3 Gas & Utility Deck
* **Contents:** 4 Large O2/H2 Generators, 8 Hydrogen Tanks (gas farm), core machine shop, component buffer.
* **Conveyor Interface:** Feeds Trunk A & Trunk B across the MRG seam.

### Step 8.2 · Section S8: H2 Habitat & Life Support
* **Contents:** Pressurized crew quarters, medical room, cryo pods, hydroponics farm, Gravity Generator.
* **Armor:** Light armor exterior with interior composite blast bulkheads.

### Step 8.3 · Section S9: H1 Command Deck & Avionics
* **Contents:** Master armored bridge, Remote Control core, Antenna Mast, RIG//OS Programmable Block Rack (4 custom PBs).
* **Roof Deck:** 2 × Picket Drone landing pads (`PKT × 2`) + Point Defense Gatling Turrets.

---


---

---

---

---

---
