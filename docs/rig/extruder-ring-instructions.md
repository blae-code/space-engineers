## Chapter 5 · Rig Linear Extruder Ring

The heavy automated shipyard that prints the Floating Rig's flown sections:
1. **S0 HUB Base (H4–H6):** Flown first on H6 lift.
2. **S1–S6 Pylons (8 W × 7 H × 22 L B):** Printed sequentially, fly up to dock at H5 sockets.

### 5.0 · Reference Grid & Operating Principle

Per **Decision 47**, the extruder uses a stationary U-shaped welder ring. A hydraulic piston pushes the projected blueprint horizontally outward over low-friction ground skid rails.

```
Extruder Elevation (Looking Starboard, X = 0):
                 Stationary Welder Ring
                     ┌──[WELDERS]──┐
                     │    (L1–L8)  │
  [HYD PISTON 1..6] ═╡             │═════════════════════════════════► Launch Direction (+Z)
  [◄ Push Tail ───]  │   [PYLON    │    [Completed Pylon 22 B Span]
  [Projector Head]   │  Extruding] │    [On Skid Rails L1]
  ═══════════════════╧═════════════╧═════════════════════════════════
```

---

### Step 5.1 · 8 W × 7 H Welder U-Ring (Y = L1 to L8)

Build a rigid arch framing an 8 W × 7 H opening. Mount ship welders pointing inward on all 3 interior faces.

```
Front Elevation, looking down the launch axis (Z = 0)
        X: −5  −4  −3  −2  −1   0  +1  +2  +3  +4  +5
    L8     ■═══■═══■═══■═══■═══■═══■═══■═══■═══■═══■  Roof Arch
    L7     ║  [W] [W] [W] [W] [W] [W] [W] [W]  ║  (8 welders pointing DOWN)
    L6     ║[W]                             [W]║
    L5     ║[W]      CLEAR PRINT THROAT     [W]║  (7 welders pointing STBD)
    L4     ║[W]       (8 W × 7 H blocks)    [W]║  (7 welders pointing PORT)
    L3     ║[W]                             [W]║
    L2     ║[W]                             [W]║
    L1     ■═══■═══[SKID ROLLER TRACK]══■═══■═══■  Tarmac Base
```

```
2.5D Isometric Callout · 8 W × 7 H Welder Arch Architecture

                   ┌───────────────────────────────────┐
                   │  Top Welder Rake (8× Ship Welders)│
                   └─┬───────────────────────────────┬─┘
                     │                               │
        ┌────────────┴────┐                     ┌────┴────────────┐
        │ Port Welder Rake│  CLEAR PRINT ENVELOPE│ Stbd Welder Rake│
        │ (7× Ship Welders│   (8 W × 7 H blocks)│ (7× Ship Welders│
        │  Facing STBD)   │                     │  Facing PORT)   │
        └────────────┬────┘                     └────┬────────────┘
                     │                               │
        ═════════════╧═══════════════════════════════╧═════════════
                         Low-Friction Skid Roller Track (L1)
```

**Blocks:**
* 22 × Ship Welder (large grid)  · 
  * 8 × Top Welder (facing DOWN at L7, X −4 to +3)
  * 7 × Port Welder (facing STBD at X −5, L2 to L8)
  * 7 × Starboard Welder (facing PORT at X +4, L2 to L8)
  * Group ──► `EXT Welder Ring`
* 24 × Conveyor Junction & Heavy Frame blocks supplying parts from `GND STR Components`

**✓ Check:** Welders provide overlapping 1.75 m active weld spheres covering the entire 8 × 7 perimeter and interior blocks.

---

### Step 5.2 · Hydraulic Pusher & Projector Carriage

A horizontal pusher rail at the tail advances the blueprint through the welder throat.

```
Plan view, Y = L2 (Pusher Axis)
        Z: −25         −15          −5        0 (Welder Ring)
  Rail:  ■──[PIST 1]──[PIST 2]──[PIST 3]──[HEAD] ──► Pushes toward +Z
```

```
2.5D Isometric Callout · Hydraulic Pusher & Merge Head

  [ANCHOR WALL]
        ║
        ╠══[PISTON 1]══[PISTON 2]══[PISTON 3]══[PISTON 4]══[PISTON 5]══[PISTON 6]
        ║                                                              │
        ║                                                              ▼
        ║                                                    [EXT PRJ Projector]
        ║                                                    [EXT MRG Release]
        ║                                                              │
        ╚══════════════════════════════════════════════════════════════╪═══════
                                                                       ▼
                                                             [PYLON ROOT COLLAR]
```

**Blocks:**
* 6 × Piston (large grid, mounted in series along Z-axis) · `EXT Pusher 1` … `EXT Pusher 6` ──► group `EXT Pushers`
  * Settings: Velocity = `0.05 m/s`, Share Inertia Tensor = `ON`, Max Distance = `10.0 m` each (60 m total stroke)
* 1 × Projector (large grid) · `EXT PRJ Head` (mounted on pusher tip)
* 1 × Merge Block · `EXT MRG Release` (connects pusher head to pylon root)

**✓ Check:** Pistons extend smoothly at 0.05 m/s without binding. `EXT MRG Release` cleanly disconnects the welded pylon when complete.

---

### Step 5.3 · Low-Friction Skid Roller Track (Z 0 to +25)

A 25 B long ground launch way built from heavy-armor half blocks and blast plates supports the pylon as it emerges.

```
Cross-section, looking toward Launch (+Z)
    L2   [Extruded Pylon Keel]
    L1   [BLAST][HALF] ··········· [HALF][BLAST]  ◄── 2 low-friction guide skids
    L0   ═══════════════════════════════════════  Foundation Tarmac
```

**Blocks:**
* 50 × Heavy Armor Half Block (2 parallel guide tracks along X = −3 and X = +2)
* 50 × Blast Door Block (low friction contact surface)

**✓ Check:** Pylon keel slides freely along the blast door rails with zero deformation or snagging.

---

### Step 5.4 · Pylon Flight Commissioning Sequence

When the 22 B pylon reaches full extension:
1. **Conveyor Lock:** Keel conveyor ports mate with temporary refueling line; fill H2 tanks to 100%.
2. **Seed Kit Activation:** Turn on pylon battery bank, Remote Control, gyros, and antenna.
3. **Lift Thruster Test:** Spool 7 Large Atmo thrusters to 100% override for 2 seconds to confirm lift authority.
4. **Disconnect:** Turn `EXT MRG Release` OFF. Pylon is now an independent grid.
5. **Ascent:** Remote Control engages vertical climb to 500 m altitude; navigates autonomously to the Floating Rig H5 socket.

---

### Step 5.5 · Master AI Scripting Contract & Terminal Namespace (Claude Code)

Every automated device on the Ground Base and Extruder Ring conforms to this exact naming and grouping dictionary:

#### 1. Group Taxonomy
* `GND Refineries` : 4× Large Refineries (`GND REF 1` … `4`).
* `GND Assemblers` : 4× Large Assemblers (`GND ASM 1` … `4`).
* `GND Fleet Displays` : 6× Large LCD Panels (`GND LCD Video Wall 1` … `6`).
* `EXT Welder Ring` : 22× Ship Welders on the printer arch.
* `EXT Pushers` : 6× Hydraulic Extension Pistons.

#### 2. Event Controller Matrix
| Controller | Monitored Target | Threshold / Condition | Action Slot 1 (On True) | Action Slot 2 (On False) |
|---|---|---|---|---|
| `GND EC PAD` | `GND PAD Sensor` | Mule Detected at 15 m | `GND PAD Cradle` Angle $
ightarrow$ 30° · `GND PAD Conn` Lock ON | `GND PAD Cradle` Angle $
ightarrow$ 0° · `GND PAD Conn` Lock OFF |
| `EXT EC Print` | `EXT Pusher 6` Position | Extension = Max (60 m) | `EXT Welder Ring` OFF · Sound Print Chime | `EXT Welder Ring` ON |

#### 3. IGC Channel Subscriptions (Fleet Operations Hub)
* Inbound: `TMS.ORCA.STATUS` (Carrier hold capacity, fuel burn, active leases).
* Inbound: `TMS.MULE.STATUS` (Shuttle payload, battery percentage, transit ETA).
* Outbound: `TMS.MULE.CALL` (Automated dispatch of empty/laden freight ferries).
* Outbound: `TMS.BASE.QUOTAS` (Refined component thresholds for shipyard printing).

---
