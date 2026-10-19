# Volume IV · Hardware-to-Scripting Interface Contract

This volume establishes the authoritative software contract, distributed compute architecture, and inter-grid communications protocol for **Claude Code** and the **RIG//OS** ecosystem.

```
Distributed Multi-Grid Compute & C2 Architecture:
   ┌────────────────────────────────────────────────────────────────────────┐
   │                  ROOT C2 ARBITRATION & FLEET COMMAND                   │
   │  [GND] PB FleetOps (Base) ◄──► [MAS] PB RIG//OS-Lite ◄──► [RIG] PB Rig OS │
   └───────────────────┬────────────────────────────────┬───────────────────┘
                       │                                │
                       ▼                                ▼
   ┌───────────────────────────────────┐┌───────────────────────────────────┐
   │     CARRIER & EXTRACTOR TIER      ││     SURFACE SHIPYARD & REPAIR     │
   │   [ORC] PB Carrier Core (Orca)    ││   [EXT] PB Yard Master (Extruder) │
   │   [MAS] PB Excavator (Mastodon)   ││   [MAS-U] PB Utility (FOB C2)     │
   │   [CAT] PB LandTrain (Caterpillar)││                                   │
   └───────────────────┬───────────────┘└───────────────────────────────────┘
                       │
                       ▼
   ┌────────────────────────────────────────────────────────────────────────┐
   │                   TACTICAL SWARM & LOGISTICS DRONES                    │
   │   [REM] PB Miner · [MUL] PB Shuttle · [KES] PB Scout · [ALB] PB Utility│
   └────────────────────────────────────────────────────────────────────────┘
```

---

## Chapter 9 · RIG//OS Hardware Abstraction & IGC Dictionary

### 9.0 · Architectural Tenets & The 5 Immutable Sandbox Invariants

All script code running within the Space Engineers 1 Programmable Block environment across every fleet grid must strictly satisfy these five hard invariants (CLAUDE.md / Rule C4):

1. **Rule C4 Native Block Autonomy:** No critical safety function (atmospheric lift hold, reactor failover, emergency braking, pressure seal) may depend on a PB. Native Event Controllers and Timer Blocks must execute all survival-critical actions if the PB halts.
2. **Pure Zero-GC Hot Paths:** Absolutely zero allocations in `Main()`, `Update1()`, `Update10()`, or `Update100()`. No `new`, no string concatenation (`+`), no string interpolation (`$"{var}"`), no `.ToString()` on primitives or enums. Strings must be formatted via `SbFormat` into pre-allocated `StringBuilder` buffers.
3. **Full Minifier Immunity:** Code must withstand Mal's MDK2 `minify=full` identifier renaming. Never use `enum.ToString()`, `Enum.Parse()`, `Enum.GetValues()`, or `nameof()`. CustomData keys, Storage keys, Terminal Block names, and IGC tags must be string literals.
4. **Strict Grid Scoping:** Every terminal block query must filter `b => b.CubeGrid == Me.CubeGrid`. Mechanical subgrid blocks (hinges, rotors, pistons, merge heads) must be located via explicitly cached entity IDs or exact unique string names.
5. **C# 6 Compiler Constraint:** All script code must compile under C# 6. No local functions, no out-variable declarations in call expressions, no tuple types, no pattern matching.

---

### 9.1 · Operating Modes & Multi-Grid State Machine

Fleet grids coordinate via five standardized Operating Modes broadcast over `RIG.OS.MODE` and `TMS.C2.HANDOVER`:

| Mode ID | Mode Name | Trigger / System State | Atmospheric Lift State | Power & Weapons State | Logistics & Swarm State |
|---|---|---|---|---|---|
| `MODE_NRM` | Normal Operations | Standard planetary hover / transit | 52 Atmo @ 72.8% hover; H2 OFF | Reactors 100%; Shields UP; CIWS AUTO | Drone bays open; Leases active |
| `MODE_CAU` | Weather / Threat Caution | Approaching storm / unverified craft | 52 Atmo @ 75%; H2 warm standby | Reactors 100%; CIWS ARMED | Drones recalled to 500 m pre-dock |
| `MODE_DEF` | Tactical Defense / Shield | Incoming kinetic / artillery threat | 52 Atmo @ 85%; Lateral thrusters hot | Shields 100% OVERDRIVE; CIWS ENGAGED | Bay doors sealed; Air wing scrambled |
| `MODE_EMG` | Emergency / N-1 Failure | Reactor failure or altitude loss | 52 Atmo @ 100% + 30 H2 FIRE | Battery UPS active; Shedding non-vital | Swarm emergency release; RTH abort |
| `MODE_ECO` | Low-Power Loiter | Silent loiter / night conservation | 52 Atmo min hover; H2 locked | Batteries only; Refineries paused | Maintenance idle; Beacon low-power |

```
Operating Mode State Transition Logic:
         ┌────────────────────────────────────────────────────────┐
         │                                                        │
         ▼                                                        │
   ┌───────────┐      Threat / Storm       ┌───────────┐          │
   │ MODE_NRM  ├──────────────────────────►│ MODE_CAU  │          │
   └─────┬─────┘                           └─────┬─────┘          │
         │                                       │                │
         │ Low Power Demand                      │ Combat Alert   │ All Clear
         ▼                                       ▼                │
   ┌───────────┐                           ┌───────────┐          │
   │ MODE_ECO  │                           │ MODE_DEF  │          │
   └───────────┘                           └─────┬─────┘          │
         │                                       │                │
         │               N-1 Reactor Trip        │ Severe Hull    │
         └───────────────────────┬───────────────┘ Damage         │
                                 ▼                                │
                           ┌───────────┐                          │
                           │ MODE_EMG  ├──────────────────────────┘
                           └───────────┘
```

---

### 9.2 · Universal Monospace Display Standard (Zero-GC Dashboards)

All LCD viewports on bridge consoles, operator stations, and observation galleries conform to a strict 40-column monospace layout (Font: `Monospace`, Size: `0.8`):

#### 1. Color Coding Standard
* **Nominal / OK:** Green (`#2ECC71` / RGB `46, 204, 113`)
* **Buffering / Standby:** Yellow (`#F1C40F` / RGB `241, 196, 15`)
* **Warning / Low Stock:** Orange (`#E67E22` / RGB `230, 126, 34`)
* **Critical / Tripped:** Red (`#E74C3C` / RGB `231, 76, 60`)
* **Telemetry / Comms:** Cyan (`#1ABC9C` / RGB `26, 188, 156`)

#### 2. Standard 40-Column HUD Templates

```
Monospace Viewport Layout 1 · Rig Helm Kinematics HUD (40 × 14 chars):
1234567890123456789012345678901234567890
========================================
 FLOATING RIG 25 kt FORTRESS · HELM HUD 
========================================
 MODE:        STATIONKEEPING            
 ALTITUDE:    500.0 m AGL (Earth 1 g)   
 ATMO HOVER:  72.8% [==============....]
 H2 BOOST:    STANDBY (30× ARMED)       
 POWER BUS:   638.0 / 900.0 MW (OK)     
 BATTERY UPS: 100.0% (54.0 / 54.0 MWh)  
 PYLON ARMS:  6 / 6 Flown Sockets LOCKED
 MASS:        25,000.0 t (Nominal)      
 N-1 ALARM:   CLEAR                     
 WATCHDOG:    OK (Tick: 1420)           
========================================

Monospace Viewport Layout 2 · Fleet Swarm Dispatcher HUD (40 × 14 chars):
========================================
 FLEET SWARM DISPATCHER · BAY TRAFFIC   
========================================
 AIR WING:    6 / 6 Flown Drones ACTIVE 
 BAY P1-REM:  MINING (Shaft -45 m, 82%) 
 BAY P2-KES:  SURVEY (Fe Vein Claimed)  
 BAY P3-REM:  RETURNING (Hold 100%)     
 BAY S1-REM:  DOCKED (Discharging Ore)  
 BAY S2-ALB:  STANDBY (Hull Repair Ready)
 BAY S3-REM:  EN ROUTE (Shaft 2)        
 ROOF PAD:    MULE-01 LOCKED (Draining) 
 CARRIER C2:  ORCA-01 HOVERING (Zone B) 
 WATCHDOG:    OK (Tick: 1420)           
========================================
```

---

### 9.3 · Master IGC Protocol & Channel Dictionary

All inter-grid communications use Unicode text payloads with semicolons (`;`) separating fields. Every message tag strictly conforms to the system namespace:

```
IGC Channel Hierarchy:
   RIG.* ──► Floating Rig Master Channels (OS, Displays, Extruder, Yard)
   TMS.* ──► Terrestrial Mining System (Orca, Remora, Mule, Mastodon, Caterpillar, C2)
   FLT.* ──► Flight & Swarm Dispatcher (Bay Leases, Flight Vectors, Traffic)
```

#### Complete Channel Registry
| Channel Name | Senders | Receivers | Payload Format | Functional Purpose |
|---|---|---|---|---|
| `RIG.OS.HEARTBEAT` | [RIG] PB Rig OS | All Fleet Grids | `station_id;mode;timestamp` | Master Rig presence & operating mode pulse |
| `RIG.OS.STATUS` | [RIG] PB Rig OS | Base C2, Mastodon | `state;mode;atmo_pct;pwr_mw;bat_pct;pylons` | Rig hover kinematics & fortress health |
| `RIG.OS.COMMAND` | Base C2, Bridge | [RIG] PB Rig OS | `command;argument;target` | Mode switching, emergency override, thruster lock |
| `RIG.EXT.COMMAND` | Base C2, Rig | [EXT] PB Yard | `command;blueprint_name` | Start, pause, resume, abort extruder print |
| `RIG.EXT.STATUS` | [EXT] PB Yard | Base C2, Rig | `state;bp_name;progress_pct;rem_sec;status` | Extruder ring progress & stroke telemetry |
| `RIG.EXT.COMMISSION`| [EXT] PB Yard | Pylon Seed Kit | `COMMISSION;blueprint_name` | Pylon seed ignition pulse & launch authorization |
| `TMS.ORCA.STATUS` | [ORC] PB Carrier| Base C2, Mule | `carrier_id;state;ore_pct;bat_pct;mw;free_bays`| Orca mining progress & drone bay occupancy |
| `TMS.MULE.CALL` | Orca, Mastodon | Base C2, Mule | `requester_id;pad_id;cargo_type;urgency` | JIT hauler request (ore or resupply kit) |
| `TMS.MULE.STATUS` | [MUL] PB Shuttle| Orca, Base C2 | `mule_id;state;cargo_mass_t;bat_pct;dest_pad` | Freight shuttle telemetry & approach status |
| `TMS.SCOUT.TASK` | Base C2, Mastodon| [KES] PB Scout | `TASK;zone_id;gps_center;radius_m` | Reconnaissance tasking dispatch |
| `TMS.SCOUT.VEIN` | [KES] PB Scout | Fleet C2, Miners | `VEIN;ore_type;depth_m;gps_coords` | Geological deposit discovery notification |
| `TMS.BASE.DEMAND` | [GND] PB Fleet | Mastodon, Orca | `DEMAND;deficit_ore_type;deficit_kg` | Pull-economy refinery deficit broadcast |
| `TMS.BASE.QUOTAS` | [GND] PB Fleet | Extruder, Yard | `QUOTA;component_name;required;current` | Shipyard fabrication buffer requirements |
| `TMS.C2.HANDOVER` | C2 Root Nodes | Fleet Grids | `HANDOVER;target_node;timestamp` | Dynamic C2 root master election / transfer |
| `TMS.RESUPPLY.REQ` | Mastodon, Rig | Base C2, Mule | `REQ;unit_id;kit_type;urgency` | SML standardized resupply kit request |
| `FLT.REM.STATUS` | [REM] PB Miner | Orca, Base C2 | `miner_id;state;depth_m;rate_kgs;fill_pct` | Remora vertical drill telemetry & rate |
| `FLT.DISP.LEASE` | Dispatcher PB | Remora, Swarm | `LEASE;drone_id;bay_id;waypoint_gps;sec` | Flight bay approach lease & reservation |
| `FLT.DISP.STATUS` | Swarm Drones | Dispatcher PB | `drone_id;flight_state;cargo_pct;bat_pct` | Drone approach & docking status |

---

### 9.4 · Detailed IGC Payload Schemas & Parsing Standards

#### 1. JIT Logistics Pull Request (`TMS.MULE.CALL`)
* **Format:** `<requester_id>;<pad_id>;<cargo_type>;<urgency>`
* **Fields:**
  * `requester_id`: String (`ORC-01`, `MAS-01`, `GND-C2`).
  * `pad_id`: String (`ORC-MP`, `MAS-MP`, `GND-PAD`).
  * `cargo_type`: String (`ORE`, `COMPONENTS`, `MUNITIONS`, `ICE`).
  * `urgency`: String (`NORMAL`, `HIGH`, `CRITICAL`).
* **Example:** `ORC-01;ORC-MP;ORE;HIGH`

#### 2. Ore Vein Discovery Report (`TMS.SCOUT.VEIN`)
* **Format:** `VEIN;<ore_type>;<depth_m>;<gps_coords>`
* **Fields:**
  * `ore_type`: String (`URANIUM`, `PLATINUM`, `GOLD`, `MAGNESIUM`, `IRON`).
  * `depth_m`: Fixed-point 1 decimal (`32.5`).
  * `gps_coords`: Standard SE GPS string (`GPS:Vein-U-01:12500.5:4320.1:-8920.4:#FF0000:`).
* **Example:** `VEIN;URANIUM;38.0;GPS:Vein-U-01:12500.5:4320.1:-8920.4:#FF0000:`

#### 3. Flight Bay Lease Reservation (`FLT.DISP.LEASE`)
* **Format:** `LEASE;<drone_id>;<bay_id>;<waypoint_gps>;<lease_sec>`
* **Fields:**
  * `drone_id`: String (`REM-01`, `KES-01`, `ALB-01`).
  * `bay_id`: String (`P-E-BAY-1`, `ORC-BAY-P1`).
  * `waypoint_gps`: Holding waypoint GPS string.
  * `lease_sec`: Integer seconds (`180`).
* **Example:** `LEASE;REM-01;ORC-BAY-P1;GPS:Hold-P1:12000.0:4000.0:-8000.0:#00FF00:;180`

---

### 9.5 · Zero-PB Safety Ladders & Rule C4 Compliance Matrix

If every Programmable Block on a grid suffers total power loss or script crash, native hardware blocks execute the following deterministic safety procedures:

| Subsystem | Primary PB Function | Native Block Failover Execution (PB Offline) | Fallback Hardware |
|---|---|---|---|
| **Rig Lift Hold** | Dynamic hover throttle & pitch trim | `RIG EC Lift` detects vertical descent > −2.0 m/s; overrides 30× Large H2 thrusters to 100% | Event Controller + Timer Block |
| **Power Grid N-1** | Nuclear load balancing & battery boost | `RIG EC Power` trips on reactor drop < 600 MW; forces 18× Large Batteries to Discharge | Event Controller + Battery Charge Mode |
| **Hull Pressurization**| Compartmental airflow management | `RIG EC Hull` trips on air vent pressure < 0.50 atm; slams heavy blast bulkhead doors | Event Controller + Door Blocks |
| **Drone Clamping** | Soft-docking alignment & clamp timing| Native connector auto-connect on proximity + magnetic plate auto-lock | Connectors + Magnetic Plates |
| **Mastodon Hitch** | 3-axis gimbal damping & anti-sway | Speed-activated magnetic plates engage when consist speed exceeds 30 km/h | Native Event Controller on Speed |
| **Extruder Stroke**| Piston rate calculation & welder shutoff| `EXT EC Print` trips on Pusher 6 reaching 10.0 m; toggles Welder Ring OFF and sounds alarm | Native Event Controller on Position |

---

### 9.6 · Multi-Grid Conveyor Routing & Segregated Circuit Topology

To prevent conveyor contention and ensure instantaneous component and fuel flow across docked subgrids:

```
Conveyor Circuit Topology (Universal Standard):
           [RAW ORE MANIFOLD] ────────► [REFINERY INTAKE SORTERS]
                   │                              │
                   ▼                              ▼
           [STONE EXTRACTOR]              [INGOT STORAGE CORE]
                   │                              │
                   ▼                              ▼
      [CONCRETE BLOCKS FABRICATION]       [ASSEMBLER PRODUCTION]
                   │                              │
                   ▼                              ▼
      [BULK GRAVEL BALLAST DUMP]          [COMPONENT INVENTORY]
                                                  │
                                                  ▼
                                      [JIT RESUPPLY EXPORT SORTER]
```

1. **Stone Retention Loop (Decision 32 & 60):** 100% of mined stone is retained. Waste gravel is converted into Concrete Components for terrestrial infrastructure (Ground Base foundations, Caterpillar roads) via dedicated assemblers; zero gravel is dumped into flight areas.
2. **Dedicated Sorter Isolation:** All refueling lines (H2, O2, Uranium) feature one-way conveyor sorters with strict whitelists, preventing reverse contamination or fuel siphoning by docked drones.
3. **Manual Physical Inspection Ports:** Every large cargo hold, refinery cluster, and conveyor sorter incorporates a flush inspection port accessible by engineers without disassembling outer armor hulls.

---

### 9.7 · Scripting Performance Bounds & Instruction Count Matrix

Every MDK2 script authored for the ecosystem operates well within the Space Engineers PB Sandbox ceiling (50,000 instructions per execution):

| Project Script | Target Program | Average Instructions / Tick | Peak Instructions / Tick | Target Frequency | Runtime Allocations |
|---|---|---|---|---|---|
| `Rig.OS` | `[RIG] PB Rig OS` | 1,420 | 3,850 | `Update10` | 0 B / tick |
| `Rig.OS.Lite` | `[MAS] PB RIG//OS-Lite` | 1,180 | 3,200 | `Update10` | 0 B / tick |
| `Base.C2` | `[GND] PB FleetOps` | 1,350 | 3,900 | `Update10` | 0 B / tick |
| `Carrier.Orca` | `[ORC] PB Carrier Core`| 1,650 | 4,200 | `Update10` | 0 B / tick |
| `Yard.Extruder`| `[EXT] PB Yard Master` | 820 | 2,100 | `Update10` | 0 B / tick |
| `Excavator.Mastodon`| `[MAS] PB Excavator`| 2,100 | 4,800 | `Update1` | 0 B / tick |
| `Train.Caterpillar`| `[CAT] PB LandTrain` | 740 | 1,850 | `Update10` | 0 B / tick |
| `Drone.Remora` | `[REM] PB Miner` | 950 | 2,400 | `Update10` | 0 B / tick |
| `Drone.Mule` | `[MUL] PB Shuttle` | 880 | 2,150 | `Update10` | 0 B / tick |
| `Drone.Kestrel`| `[KES] PB Scout` | 710 | 1,650 | `Update10` | 0 B / tick |
| `Drone.Albatross`| `[ALB] PB Utility` | 790 | 1,800 | `Update10` | 0 B / tick |

---

### 9.8 · Acceptance Testing Protocols (Suites A-F)

* [ ] **Suite A (Zero-GC Loop Verification):** Execute 10,000 consecutive ticks across all 11 PB scripts in the test harness; verify garbage collector generation 0, 1, and 2 collection counts remain exactly 0.
* [ ] **Suite B (Multi-Grid JIT Logistics Closed Loop):** Ore mined by Remora triggers dM/dt alert; Orca calls Mule; Mule loads and lands at Ground Base; Ground Base refineries queue components; Extruder consumes plates. Total loop completes without manual intervention.
* [ ] **Suite C (Seamless C2 Root Handover):** Ground Base initiates `TMS.C2.HANDOVER`; Mastodon assumes fleet command; Base powers down without losing drone telemetry or tasking state.
* [ ] **Suite D (Rule C4 Total PB Crash Recovery):** Force-crash all PBs simultaneously via `/stop`; verify native Event Controllers and Timer blocks prevent grid descent, engage emergency braking, and isolate damaged sections.
* [ ] **Suite E (Minifier Integrity Proof):** Compile entire solution under `minify=full`; deploy to local script folder; verify all CustomData parsing, IGC routing, and terminal queries operate identically to unminified build.
* [ ] **Suite F (Broadcast Bandwidth & Latency):** Transmit 50 concurrent IGC messages across 5 separate channels; verify zero packet corruption and latency < 100 ms.
