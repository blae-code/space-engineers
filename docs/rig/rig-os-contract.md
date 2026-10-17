# Volume IV · Hardware-to-Scripting Interface Contract

This volume defines the software contract for **Claude Code** and the **RIG//OS** Programmable Blocks.

```
RIG//OS Architecture:
  ┌─────────────────────────────────────────────────────────────┐
  │ [CMD] PB Rig OS (Core State Machine, Lift, Modes, Heartbeat)│
  └──────────────┬──────────────────────────────┬───────────────┘
                 │                              │
                 ▼                              ▼
  ┌─────────────────────────────┐┌──────────────────────────────┐
  │ [LOG] PB Dispatcher         ││ [MFG] PB Inventory           │
  │ (Bay Queue, Leases, Comms)  ││ (Quotas, Sorting, Assemblies)│
  └──────────────┬──────────────┘└──────────────┬───────────────┘
                 │                              │
                 └──────────────┬───────────────┘
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │ [CMD] PB Displays (Zero-GC LCD Dashboards & Fleet Status)   │
  └─────────────────────────────────────────────────────────────┘
```

---

## Chapter 9 · RIG//OS Hardware Abstraction & IGC Dictionary

### 9.1 Operating Modes & Timer Table
Modes are driven by hardware Timer Blocks setting system states:

| Mode | Trigger / Event | Lift State | Shield / Weapons | Bay Doors |
|---|---|---|---|---|
| **`NRM` (Normal)** | Standard station ops | 52 A @ 73% hover; H2 OFF | Shields UP; Weapons AUTO | Standard watchdog |
| **`CAU` (Caution)** | Approaching storm / unverified craft | 52 A @ 73%; H2 preheated | Shields UP; Weapons ARMED | Bays recall drones |
| **`SHD` (Shield)** | Incoming kinetic / artillery fire | Atmo hover | Shields 100% OVERDRIVE | All bays SEALED |
| **`EMG` (Emergency)** | Reactor failure / pylon loss | 52 A @ 100% + 30 H2 FIRE | Auxiliary batteries ON | Recall or abandon |
| **`CBT` (Combat)** | Station under active assault | High mobility thruster override | All turrets TARGETING | Interceptors SCRAMBLED |
| **`REP` (Repair)** | Station damage detected | Hover locked | Turrets defensive | Welder grids ACTIVE |
| **`REC` (Recovery)** | Re-entry / post-combat return | Stabilized descent | Standby | Recovery lights ON |

---

### 9.2 IGC Broadcast Channels & Message Schemas
All IGC messages use `;`-separated payloads. Tags strictly follow project namespaces:

#### Channel: `RIG.CMD.*` (Command & Heartbeat)
* `RIG.CMD.BEAT`: `station_id;timestamp;mode;power_pct;shield_pct`
  * Example: `RIG-01;123456.78;NRM;71.0;100.0`
* `RIG.CMD.SCRAMBLE`: `target_gps;threat_level;flight_lead`

#### Channel: `FLT.DISP.*` (Dispatcher & Bay Management)
* `FLT.DISP.LEASE`: `drone_id;bay_id;approach_waypoint;lease_time_s`
  * Example: `REM-01;P-E-BAY-1;GPS:Predock:...:;180`
* `FLT.DISP.STATUS`: `drone_id;status_enum;cargo_pct;battery_pct`
  * Example: `MUL-01;APPROACHING;100.0;88.5`

#### Channel: `TMS.*` (Terrestrial Mining System)
* `TMS.ORCA.STATUS`: `orca_id;hold_mass_t;ore_cap_t;drones_active`
  * Example: `ORC-01;450.2;650.0;4`
* `TMS.MULE.CALL`: `requester_id;pad_id;payload_type;urgency`
  * Example: `ORC-01;ORC-MP;ORE;NORMAL`

---

### 9.3 Hardware Failover Logic (Zero-PB Safety)
If all Programmable Blocks fail or crash:
1. **Watchdog Timer:** `RIG TMR Watchdog` expires after 5.0 s without PB pulse.
2. **Failover Execution:**
   * Drills & welders forced OFF.
   * Remote Control engaging station dampeners in 1 g position hold.
   * `RIG EC Power` trips: if reactor output drops < 500 MW, 30 Large H2 thrusters ignite automatically.
   * Interceptors and mining drones fall back to native `PREDOCK` hovering slots.
