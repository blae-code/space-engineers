# Master Inter-Grid Communication (IGC) Protocol Specification

Project-wide communication standard, channel namespaces, and payload schemas for the Space Engineers 1 **Floating Rig / RIG//OS** and **Terrestrial Logistics Fleet**.

Authority: Blae Decisions 8, 38, 42, 53, 54, 56, 62, 63, 65, 66, 67, 68, 69, 70 (Canon) · `CLAUDE(RIG).md` §8 Phase 4 · `docs/rig/lego-instructions-v1.0.md`

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
| `TMS.C2.HANDOVER` | Current C2 root -> Fleet roots | `HANDOVER;<target_node>;<timestamp>` | Transfers root authority to the named commissioned node. All participating roots apply the same target; the old root becomes a worker. |
| `TMS.MULE.STATUS` | Mule -> Base / Orca / Mastodon | `<mule_id>;<state>;<cargo_t>;<battery_pct>;<destination_gps>` | Shuttle state, payload mass in tonnes, battery percentage, and active destination. |
| `TMS.MULE.CALL` | Mastodon / Base / Orca -> logistics dispatcher | `CALL;<requesting_grid>;<target_pad>;<priority>;<payload_type>` | Requests a Mule sortie; this is not the Mule's dispatch command. |
| `TMS.MULE.DISPATCH` | Current C2 root -> assigned Mule | `DISPATCH;<mule_id>;<pickup_gps>;<delivery_gps>;<payload_type>;<request_id>` | Assigns a specific Mule pickup and refinery delivery route after a request is accepted. |
| `TMS.CAT.DISPATCH` | Current C2 root -> assigned Caterpillar | `DISPATCH;<cat_id>;<destination_id>;<destination_gps>;<ore_type>;<request_id>` | Assigns a bulk-haul destination and rendezvous coordinates after a ground-tier logistics request. |
| `TMS.SCOUT.VEIN` | Kestrel -> Base / C2 | `VEIN;<ore_type>;<gps_x>;<gps_y>;<gps_z>;<est_depth_m>` | Broadcasts verified ore vein coordinates discovered by prospector drones. |
| `TMS.SCOUT.STATUS` | Kestrel -> Base / Orca | `<state>;<battery_pct>;<scan_radius_m>;<current_waypoint>` | Prospector operational telemetry during geological survey passes. |
| `TMS.SCOUT.TASK` | Mastodon / Base -> Kestrel | `TASK;<drone_id>;<pattern:GRID\|CONE>;<gps_center>;<radius_m>;<priority_ore>` | Vectors prospector scout to high-probability geological survey sectors. |
| `TMS.SEISMIC.SURVEY` | Mastodon -> Base / Fleet | `SURVEY;<grid_id>;<depth_m>;<vein_type>;<centroid_gps>;<volume_m3>` | Shares 3D subterranean deposit geometry mapped by acoustic outriggers. |
| `TMS.REPAIR.CALL` | Any Craft -> Albatross | `REPAIR;<grid_name>;<gps_x>;<gps_y>;<gps_z>;<damage_pct>;<severity>` | Emergency SOS call requesting automated field repair and welding dispatch. |
| `TMS.REPAIR.STATUS` | Albatross -> Base / C2 | `<state>;<target_grid>;<parts_hold_pct>;<eta_sec>` | Field repair craft task status (`DISPATCHED`, `WELDING`, `RETURNING`). |
| `TMS.BASE.QUOTAS` | Base C2 -> Yard / Fleet | `QUOTA;<steel_plates>;<motors>;<tubes>;<computers>;<h2_liters>` | Ground Base component manufacturing thresholds and material reserves. |
| `TMS.BASE.DEMAND` | Base C2 -> Mastodon / Orca | `DEMAND;<ore_type>;<priority:1..5>;<deficit_kg>;<target_stock_kg>` | Measured ingot reserve deficit for pull-economy planning. |
| `TMS.LOGISTICS.JIT` | Mastodon / Base -> logistics dispatcher | `JIT;<requester_id>;<tier:AIR\|GND>;<eta_target_sec>;<ore_type>;<est_mass_kg>;<source_gps>` | Requests transport before a source hold saturates; the dispatcher emits a separate assignment with route coordinates. |
| `TMS.MAS.STATUS` | Mastodon -> Base / Fleet | `<mastodon_id>;<state>;<active_macro>;<cargo_kg>;<rate_kg_s>;<power_mode>;<speed_kmh>` | Excavator state, cargo, extraction rate, power mode, and vehicle speed telemetry. |
| `TMS.MAS.ORE_TARGET` | RIG//OS-Lite -> Mastodon | `TARGET;<ore_type>;<priority>` or `TARGET;NONE;0` | Demand-selected ore label only; Mastodon queues changes during excavation and applies them at a safe operator boundary. |
| `TMS.CAT.STATUS` | Caterpillar -> Base / C2 / Mastodon | `<cat_id>;<state>;<cargo_kg>;<consist_length_b>;<dump_ready_bool>` | Land-train state, payload, consist length, and terminal unload readiness. |
| `TMS.CONVOY.SYNC` | Mastodon -> Caterpillar | `CONVOY;<lead_id>;<speed_ms>;<heading_deg>;<ejector_active_bool>` | Synchronizes Caterpillar trailing speed behind Mastodon roadbed spreader. |
| `TMS.ZONE.CLAIM` | Mastodon -> Peer Mastodons | `ZONE;<grid_id>;<state>;<gps_bounding_min>;<gps_bounding_max>;<macro_mode>` | Deconflicts heavy excavation envelopes in multi-rig quarry operations. |
| `TMS.DEFENSE.TARGET` | Tactical Lead -> Fleet CIWS | `ENGAGE;<target_entity_id>;<target_gps>;<threat_level:LOW\|MED\|HIGH>;<assigned_grid>` | Distributes anti-air and counter-missile defense targets across consist turrets. |
| `TMS.CONSIST.SYNC` | Mastodon Tractor -> Utility | `SYNC;<source_id>;<hitch_torque>;<clamp_state>;<airlock_state>` | Synchronizes drawbar magnetic clamps and gangway airlock sequencing. |
| `TMS.CONSIST.TRACTION` | Mastodon Tractor -> Utility | `DPU;<mode:PUSH\|BRAKE\|COAST>;<torque_pct>;<axle4_slip>;<axle5_slip>` | Directs Distributed Power Unit (DPU) traction on Axles 4 & 5. |
| `TMS.CONSIST.SEVER` | Mastodon Consist -> Fleet | `SEVER;<source_id>;<reason>;<keel_skids_engaged>` | Broadcasts emergency drawbar decoupling and deadman brake activation. |
| `TMS.RESUPPLY.REQUEST` | Distressed Rig -> Fleet | `RESUPPLY;<target_grid>;<gps>;<kit_type:MOBILITY\|POWER\|AMMO\|HULL>;<priority>` | Emergency call for prefabricated repair, fuel, or munitions kit. |
| `TMS.RESUPPLY.OFFER` | Mastodon -> Distressed Rig | `OFFER;<offering_grid>;<target_grid>;<kit_type>;<eta_sec>` | Confirms kit availability and estimated delivery timeline. |
| `TMS.RESUPPLY.DISPATCH` | Mastodon -> Mule Shuttle | `DISPATCH;<shuttle_id>;<pickup_pad>;<dest_grid>;<dest_gps>;<kit_type>` | Orders Mule air ferry to load export hopper and launch rapid sortie. |
| `TMS.RESUPPLY.COMPLETE` | Receiving Rig -> Fleet | `COMPLETE;<receiving_grid>;<delivering_shuttle>;<kit_type>;<status>` | Acknowledges receipt of parts and successful field repair completion. |
| `TMS.ENV.MODE` | Mastodon C2 -> Fleet / Consist | `ENV;<mode:EARTHLIKE\|VACUUM\|TOXIC\|STORM>;<exterior_o2_pct>;<rad_alert>` | Environmental mode and hazardous planetary condition broadcast. |
| `TMS.AIRLOCK.CYCLE` | Control Panel -> Airlock | `CYCLE;<airlock:PORT\|ROOF\|STERN\|GANGWAY>;<dir:IN\|OUT>;<decon_bool>` | Initiates automated gas reclamation and UV decontamination cycle. |
| `TMS.METEOR.ALERT` | Mastodon CIWS -> Fleet / Consist | `METEOR;<intensity:LIGHT\|MED\|HEAVY>;<azimuth_deg>;<elevation_deg>;<impact_eta_sec>` | Early warning meteor swarm detection, CIWS dome lock, and consist lockdown trigger. |
| `TMS.AMMO.STATUS` | Mastodon Ammo EC -> Consist / Logistics | `AMMO;<gatling_box_count>;<interior_mag_count>;<mg_ingot_kg>;<deficit_box_count>` | Consist munitions telemetry, magazine capacity, and automated resupply trigger. |

`TMS.LOGISTICS.JIT`, `TMS.MULE.DISPATCH`, `TMS.CAT.DISPATCH`, and `TMS.BASE.DEMAND` use the exact
field counts in the table. Empty fields and extra fields are invalid. Dispatches include a request ID;
the assigned vehicle accepts only its own ID and must not interpret a JIT request as an assignment.
For JIT, `source_gps` is tier-specific: `AIR` uses the grid-scoped `MAS MP Conn` position; `GND`
uses the Mastodon PB/grid position as a coarse rendezvous waypoint. The GND coordinate is not a
docking target; final Caterpillar-to-Mastodon alignment requires relative control.

The active Base C2 broadcasts one `TMS.BASE.DEMAND` message for each configured mineral every
Update100, including zero deficits. `deficit_kg` is `max(0, ceil(target_stock_kg - current_stock_kg))`.
Current stock sums the matching ingot subtype across inventories on the Base PB's grid only. The
configured `IronTons` reserve is converted to kg. Current priorities are 5 for Uranium, Platinum,
Gold, and Magnesium, and 3 for Iron. RIG//OS-Lite retains each valid entry for at most 30 Update10
calls without refresh and displays the fresh deficits. It selects only fresh positive deficits by
priority descending, deficit descending, then fixed mineral order (Uranium, Platinum, Gold,
Magnesium, Iron). Changes are broadcast on `TMS.MAS.ORE_TARGET`; `TARGET;NONE;0` clears the selected
label when no fresh deficit remains. The label does not identify a deposit or command movement,
boom, or tools. The operator positions Mastodon over the deposit. During excavation, Mastodon holds
the latest update pending until the next operator-issued `MACRO` command or until it leaves
excavation.


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
