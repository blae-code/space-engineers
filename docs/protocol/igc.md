# Master Inter-Grid Communication (IGC) Protocol Specification

Project-wide communication standard, channel namespaces, and payload schemas for the Space Engineers 1 **Floating Rig / RIG//OS** and **Terrestrial Logistics Fleet**.

Authority: Blae Decisions 8, 38, 42, 53, 54 (Canon) · `CLAUDE(RIG).md` §8 Phase 4 · `docs/rig/lego-instructions-v1.0.md`

---

## 1. Universal Protocol Invariants

1. **Payload Delimiter (Decisions 8 & 38):**
   * All IGC unicast and broadcast payloads MUST use the semicolon (`;`) character as the field delimiter.
   * Pipe (`|`) delimiters from legacy pre-merge fleet code are deprecated.
2. **Channel Naming Taxonomy:**
   * `RIG.<system>.*`: Station-level communications between Rig internal PBs and station displays.
   * `FLT.<craft>.*`: Drone-to-carrier tactical flight, docking, and status feeds.
   * `TMS.*`: Terrestrial Mining System logistics dispatch, resource quotas, and telemetry.
3. **Minifier Immunity:**
   * All channel tags and command verb strings MUST be string literals.
   * Under `minify=full`, `nameof()`, `enum.ToString()`, and `Enum.Parse()` are broken.
4. **Zero-GC Message Parsing:**
   * Inbound message strings are parsed using pre-allocated `List<string>` buffers without runtime heap allocations.
   * Outbound messages are formatted using pre-allocated `StringBuilder` instances and `SbFormat`.

---

## 2. Heartbeat & Hardware Failover Contract (Decision 42)

Autonomous operations across moving atmospheric carriers (Orca), vertical miners (Remora), and freight shuttles (Mule) depend on a synchronized heartbeat watchdog:

* **Broadcast Interval:** Exactly `1.0 s` (60 ticks / Update100 or 6× Update10).
* **Timeout Threshold:** Exactly `5.0 s` (300 ticks).
* **Hardware Safety Layer (Rule C4 / S1):**
  * When the 5.0 s watchdog expires, native blocks execute the safety failover:
    * Event Controllers (D1-D4) detect heartbeat timeout.
    * Mining drills and thruster overrides are immediately cut.
    * Dampeners engage to halt drift.
    * Native Autopilot / Action Relay engages emergency Return-to-Home (HOME) or hold at stand-off (PREDOCK).

---

## 3. Master Channel & Message Dictionary

### 3.1 Terrestrial Mining System (`TMS.*`)

| Channel Tag | Direction | Payload Schema | Description |
|---|---|---|---|
| `TMS.ORCA.STATUS` | Orca -> Base / Fleet | `<hold_cap_pct>;<battery_pct>;<h2_pct>;<active_bays>;<ore_mass_kg>` | Broadcasts Orca carrier hold capacity and operational readiness every 1.0 s. |
| `TMS.MULE.STATUS` | Mule -> Base / Orca | `<state>;<cargo_kg>;<battery_pct>;<eta_sec>;<target_pad>` | Real-time shuttle flight state (`IDLE`, `ENROUTE`, `DOCKING`, `TRANSFER`). |
| `TMS.MULE.CALL` | Orca / Base -> Mule | `CALL;<requesting_grid>;<target_pad>;<priority>;<payload_type>` | Dispatches an available Mule shuttle to a specific touchdown gantry pad. |
| `TMS.SCOUT.VEIN` | Kestrel -> Base / C2 | `VEIN;<ore_type>;<gps_x>;<gps_y>;<gps_z>;<est_depth_m>` | Broadcasts verified ore vein coordinates discovered by prospector drones. |
| `TMS.SCOUT.STATUS` | Kestrel -> Base / Orca | `<state>;<battery_pct>;<scan_radius_m>;<current_waypoint>` | Prospector operational telemetry during geological survey passes. |
| `TMS.REPAIR.CALL` | Any Craft -> Albatross | `REPAIR;<grid_name>;<gps_x>;<gps_y>;<gps_z>;<damage_pct>;<severity>` | Emergency SOS call requesting automated field repair and welding dispatch. |
| `TMS.REPAIR.STATUS` | Albatross -> Base / C2 | `<state>;<target_grid>;<parts_hold_pct>;<eta_sec>` | Field repair craft task status (`DISPATCHED`, `WELDING`, `RETURNING`). |
| `TMS.BASE.QUOTAS` | Base C2 -> Yard / Fleet | `QUOTA;<steel_plates>;<motors>;<tubes>;<computers>;<h2_liters>` | Ground Base component manufacturing thresholds and material reserves. |
| `TMS.MAS.STATUS` | Mastodon -> Base / Cat | `<state>;<cargo_kg>;<tusk_sweep_pct>;<wheel_speed_ms>` | Heavy wheeled excavator payload and 3-DOF trench carving telemetry. |
| `TMS.CAT.STATUS` | Caterpillar -> Base / C2 | `<state>;<cargo_kg>;<consist_length_b>;<dump_ready_bool>` | Land train consist status, drawbar articulation, and dump queue readiness. |

---

### 3.2 Fleet Flight & Tactical Control (`FLT.*`)

| Channel Tag | Direction | Payload Schema | Description |
|---|---|---|---|
| `FLT.REM.STATUS` | Remora -> Orca | `<drone_id>;<state>;<hole_index>;<cut_depth_m>;<cargo_kg>;<bat_pct>` | Vertical shaft driller progress inside designated carrier bay shaft. |
| `FLT.REM.CMD` | Orca -> Remora | `<drone_id>;<cmd>;<arg1>;<arg2>` | Tactical commands (`START_HOLE`, `ABORT`, `RECALL`, `JETTISON`). |
| `FLT.ORC.BAY` | Orca -> Remora | `<bay_id>;<state>;<jaw_status>;<conn_lock_bool>` | Bay guide jaw status (`OPEN`, `CLAMPING`, `LOCKED`) for docking handshake. |
| `FLT.MUL.PAD` | Orca / Base -> Mule | `<pad_id>;<state>;<cradle_angle_deg>;<mag_clamp_bool>` | Touchdown gantry cradle telemetry coordinating belly connector lock. |

---

### 3.3 Rig Station & Shipyard Extruder (`RIG.*`)

| Channel Tag | Direction | Payload Schema | Description |
|---|---|---|---|
| `RIG.OS.HEARTBEAT` | Rig OS -> Station / Fleet | `BEAT;<timestamp>;<alert_mode>;<hover_lift_pct>;<reactor_mw>` | 1.0 s master Floating Rig heartbeat and defensive readiness broadcast. |
| `RIG.OS.MODE` | Rig Bridge -> All PBs | `MODE;<NRM|CAU|SHD|EMG|CBT|REP|REC>` | Global station alert mode transition command. |
| `RIG.EXT.STATUS` | Extruder -> Base C2 | `EXTRUDING;<section_id>;<layer_num>;<progress_pct>;<eta_sec>` | Shipyard linear extrusion progress, welder draw, and layer cycle timing. |
| `RIG.EXT.COMMAND` | Base C2 -> Extruder | `CMD;<START|PAUSE|RESUME|ABORT|COMMISSION>;<blueprint>` | Operator command to initiate, pause, or commission section printing. |
| `RIG.EXT.COMMISSION` | Extruder -> Pylon Seed | `IGNITE;<section_id>;<target_h5_socket>` | Avionics activation pulse to completed section prior to merge release. |
| `RIG.DISP.STATUS` | Rig PBs -> Displays | `<screen_id>;<line_index>;<formatted_text>` | Internal display bus driving panoramic video walls and tactical LCDs. |

---

## 4. Zero-GC Reference Implementation (C# 6)

All MDK2 PB scripts in `scripts/` utilize the shared parsing logic provided in `scripts/Shared/Net/IgcProtocol.cs`:

```csharp
namespace IngameScript
{
    public static class IgcProtocol
    {
        public const char Sep = ';';

        public static void Split(string payload, List<string> output)
        {
            output.Clear();
            if (payload == null) return;
            int start = 0;
            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i] == Sep)
                {
                    output.Add(payload.Substring(start, i - start));
                    start = i + 1;
                }
            }
            if (start <= payload.Length)
                output.Add(payload.Substring(start));
        }

        public static StringBuilder Build(StringBuilder sb, string tag, string arg1, string arg2)
        {
            return sb.Clear().Append(tag).Append(Sep).Append(arg1).Append(Sep).Append(arg2);
        }
    }
}
```

