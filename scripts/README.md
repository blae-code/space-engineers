# Space Engineers 1 · Terrestrial Fleet & RIG//OS Scripts Architecture

Authoritative directory layout, naming contracts, and interface specifications for the Space Engineers 1 **Floating Rig / RIG//OS** and **Terrestrial Mining & Logistics Fleet**.

Authority: Blae Decisions 1-54 (Canon) · `CLAUDE(RIG).md` · `docs/rig/lego-instructions-v1.0.md`

---

## 1. Directory Layout & Project Registry

All programmable block projects in this directory target **Space Engineers 1 MDK2** (`netframework48`, C# 6, `minify=full`):

```
scripts/
├── README.md                      # Master architecture & interface contract (this file)
├── Shared/                        # Shared Project (.shproj): Core, Nav, Flight, Net, Config, Util
├── Carrier.Orca/                  # [ORC] PB Carrier Core · 17 × 19 × 7 B Heavy Mining Carrier
├── Drone.Remora/                  # [REM] PB Miner · 11 × 11 sb Vertical Shaft Driller (Retain Stone)
├── Drone.Mule/                    # [MUL] PB Shuttle · 7 × 7 B Heavy Electric Freight Shuttle
├── Excavator.Mastodon/            # [MAS] PB Excavator · 11 × 25 × 6 B Heavy Wheeled Excavator
├── Train.Caterpillar/             # [CAT] PB Train · 42 B Articulated Bulk Land Train Consist
├── Drone.Kestrel/                 # [KES] PB Scout · 7 × 9 × 5 sb High-Speed Prospector Drone
├── Drone.Albatross/               # [ALB] PB Utility · 9 × 11 × 5 sb Field Repair & Ferry Craft
├── Base.C2/                       # [GND] PB FleetOps · Ground Base & Multi-Fleet C2 Bridge Hub
├── Yard.Extruder/                 # [EXT] PB Yard Master · Rig Linear Extruder Ring Shipyard
└── Rig.OS/                        # [RIG] PB Rig OS · 25 kt Floating Rig Central Operating System
```

---

## 2. Universal PB Sandbox & Code Rules

All script code in `scripts/` MUST strictly satisfy these hard engineering constraints (Breach = Bounce):

| Rule | Constraint | Details |
|---|---|---|
| **Language Version** | **C# 6 Only** | No C# 7+ features (no tuples, no pattern matching, no out variables, no local functions). |
| **Namespaces** | **`namespace IngameScript`** | All types nested inside `public partial class Program : MyGridProgram`. No other namespaces. |
| **Banned APIs** | **Strictly Forbidden** | `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`, `System.Globalization`, and **LINQ**. |
| **Zero-GC Hot Paths** | **Zero Allocations** | No `new`, no string formatting/concatenation in `Update1`, `Update10`, `Update100`, or `Main()`. Numbers formatted via `SbFormat`; enum names mapped to static string tables. |
| **Minifier Immunity** | **`minify=full`** | All identifiers are renamed. Never depend on runtime names: no `enum.ToString()`, `Enum.Parse()`, or `nameof()`. CustomData keys, Storage keys, and IGC tags must be string literals. |
| **Grid Scoping** | **Strictly Enforced** | Every block query MUST filter `b => b.CubeGrid == Me.CubeGrid`. Subgrid blocks (pistons, hinges, rotors) tracked by cached entity IDs. |
| **Hardware Safety** | **Rule C4 & S1** | Native blocks (Event Controllers, Timers, Sensors) provide hardware safety; PBs only make strategic decisions. |

---

## 3. Project Matrix & Hardware Contracts

| Project | Grid | Target PB Block | Update Rate | Primary Subsystems / Managed Hardware | IGC Channels |
|---|---|---|---|---|---|
| **`Shared`** | Any | Shared Library | N/A | Math, PID, Frames, SbFormat, IGC codecs, Blackboard, Kernel | Common protocol |
| **`Carrier.Orca`** | Large | `[ORC] PB Carrier Core` | `Update10` | 6 Drone Bays (P1-P3, S1-S3), Guide Jaws, Mag Clamps, Mule Pad | `TMS.ORCA.*`, `FLT.ORC.*` |
| **`Drone.Remora`** | Small | `[REM] PB Miner` | `Update1` | 9×9 sb Drill Head, Vertical Hole Grid, Retain Stone, Top Dock | `FLT.REM.*`, `TMS.MINER.*` |
| **`Drone.Mule`** | Large | `[MUL] PB Shuttle` | `Update10` | 4 Large Flat Atmos, Point-to-Point Nav, Pad Cradle Clamp | `TMS.MULE.*`, `FLT.MUL.*` |
| **`Excavator.Mastodon`** | Large | `[MAS] PB Excavator` | `Update1` | Twin 3-DOF Laser Tusks, 10× 5×5 Wheels, Rear Dump Bumper | `TMS.MAS.*`, `FLT.MAS.*` |
| **`Train.Caterpillar`** | Large | `[CAT] PB Train` | `Update1` | 4-Axle Locomotive, 2-Axis Drawbar Hinges, Stern Dump Sorter | `TMS.CAT.*`, `FLT.CAT.*` |
| **`Drone.Kestrel`** | Small | `[KES] PB Scout` | `Update10` | Ore Detector, Geological Recon Survey, Vein Broadcast | `TMS.SCOUT.*`, `FLT.KES.*` |
| **`Drone.Albatross`** | Small | `[ALB] PB Utility` | `Update10` | Ship Welder Nose, Field Repair Dispatch, Component Hold | `TMS.REPAIR.*`, `FLT.ALB.*` |
| **`Base.C2`** | Large | `[GND] PB FleetOps` | `Update10` | 6-Panel Video Wall, Sorter Manifolds, Multi-Fleet Quotas | `TMS.BASE.*`, `RIG.C2.*` |
| **`Yard.Extruder`** | Large | `[EXT] PB Yard Master` | `Update10` | 22× Welder Ring, 6-Stage Hydraulic Pusher, Bunkering Boom | `RIG.EXT.*`, `TMS.BASE.*` |
| **`Rig.OS`** | Large | `[RIG] PB Rig OS` | `Update10` | 52 Large Atmos, 3× Large Reactors, Defense Shields, Pylons S1-S6 | `RIG.OS.*`, `RIG.DISP.*` |

---

## 4. Master Inter-Grid Communication (IGC) Dictionary

All messages use semicolon-separated (`;`) payloads project-wide per Decision 8 & Decision 38:

### Channel Tags:
* `TMS.ORCA.STATUS` : Carrier hold capacity, fuel burn, active leases (`<capacity_pct>;<battery_pct>;<h2_pct>;<active_bays>`).
* `TMS.MULE.STATUS` : Shuttle payload, battery percentage, transit ETA (`<state>;<cargo_kg>;<bat_pct>;<eta_sec>`).
* `TMS.MULE.CALL` : Automated dispatch request (`CALL;<target_pad>;<priority>;<payload_type>`).
* `TMS.SCOUT.VEIN` : Prospector vein discovery (`VEIN;<ore_type>;<gps_x>;<gps_y>;<gps_z>;<est_depth_m>`).
* `TMS.REPAIR.CALL` : Damaged craft emergency call (`REPAIR;<grid_name>;<gps_x>;<gps_y>;<gps_z>;<severity>`).
* `RIG.EXT.STATUS` : Extruder progress (`EXTRUDING;<section_id>;<progress_pct>;<est_time_remaining_sec>`).
* `RIG.EXT.COMMAND` : Shipyard command pulse (`START;<blueprint_name>`, `PAUSE`, `RESUME`, `ABORT`, `COMMISSION`).
* `RIG.OS.HEARTBEAT`: 1.0 s master station heartbeat broadcast (`BEAT;<timestamp>;<alert_mode>;<lift_pct>`).

---

## 5. Build, Minification & Deployment Workflow

Compile and deploy using the .NET SDK (Linux / Proton):

```bash
# Build specific PB script in Release mode (compiles vs Bin64, minifies, deploys to game)
dotnet build scripts/Carrier.Orca -c Release
dotnet build scripts/Drone.Remora -c Release
dotnet build scripts/Drone.Mule -c Release

# Check script character budget (hard limit: 100,000 characters)
fish tools/check-size.fish
```
