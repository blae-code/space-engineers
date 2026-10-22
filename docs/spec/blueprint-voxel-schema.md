# Voxel JSON Schema Specification · SE 3D Chapter Visualizer & AI Sight Engine

Master specification for digital 3D/2D representation of Space Engineers blueprints, build steps, and machine-readable spatial datasets across the Floating Rig and Logistics Fleet ecosystem.

---

## 1. System Overview & Dual-Sight Contract

The Space Engineers 3D Chapter Visualizer provides a unified spatial bridge between two audiences:
1. **Human Engineers**: Interactive 3D WebGL viewport (Three.js), step-by-step build scrubber (mimicking the in-game SE Projector with ghosted hologram blocks), 2D layer deck floorplans, subsystem x-ray filtering, and exploded layer animations.
2. **Programmatic consumers**: Integer voxel coordinates `[X, Y, Z]`, validated model JSON, 2D ASCII slices, SVG projections, and browser-generated PNG snapshots.

```
                     ┌─────────────────────────────────────────┐
                     │ CHAPTER BUILD SPECIFICATION (Markdown)  │
                     └────────────────────┬────────────────────┘
                                          │
                                          ▼
                     ┌─────────────────────────────────────────┐
                     │   UNIFIED VOXEL JSON SCHEMA (Tier 1)    │
                     └────────┬───────────────────────┬────────┘
                              │                       │
                              ▼                       ▼
               ┌────────────────────────┐   ┌────────────────────────┐
               │ 2D SLICE ENGINE        │   │ 3D WEBGL ENGINE        │
               │ (Slicer2D.js)          │   │ (VoxelWorld.js)        │
               └──────────┬─────────────┘   └──────────┬─────────────┘
                          │                            │
            ┌─────────────┴─────────────┐              ├───────────────────────────┐
            ▼                           ▼              ▼                           ▼
     [2D ASCII SLICE]             [2D SVG SLICE]  [INTERACTIVE PWA]      [HEADLESS SNAPSHOT API]
     · In-context matrix          · Scalable      · Three.js Canvas      · PNG base64 export
     · Token-efficient            · Visual line   · Build scrubber       · Multimodal AI sight
     (Tier 2: AI Text Sight)      (Tier 2: Dual)  (Tier 3: Human Sight)  (Tier 4: AI Vision)
```

---

## 2. Grid Standards & Coordinate Triad

All coordinates follow the Space Engineers Large/Small grid conventions:
* **Large Grid (B):** 1 block = 2.5 m (Mastodon, Caterpillar, Orca, Mule, Ground Base, Floating Rig).
* **Small Grid (sb):** 1 block = 0.5 m (Remora, Kestrel, Albatross).
* **Coordinate Triad:**
  * **X (Beam):** Port is negative (`-X`), Starboard is positive (`+X`), Centerline is `X = 0`.
  * **Y (Vertical / Deck):** Keel/Lowest Plane is `L1` (`Y = 1`), advancing upward (`+Y`).
  * **Z (Length):** Aft/Stern is negative (`-Z`), Bow/Forward is positive (`+Z`), Midline is `Z = 0`.
* **Three.js World Mapping:**
  * `Three.X = SE.X * blockMeters`
  * `Three.Y = (SE.Y - 0.5) * blockMeters`
  * `Three.Z = -SE.Z * blockMeters` (SE `+Z` is Forward; Three.js `-Z` is Forward).
  * `blockMeters` is `2.5` for Large Grid and `0.5` for Small Grid.
* **Block Rotation Mapping:**
  * `rot`, when present, is a block-local Space Engineers Euler vector `[X, Y, Z]` in **degrees**, applied in `XYZ` order. An absent `rot` is zero rotation.
  * The reflected `Z` axis means the renderer must not pass these values directly to Three.js. For Three.js `XYZ` Euler rotation, convert to radians as `[-X, -Y, +Z] × π/180`; translation remains the coordinate mapping above.
  * `orientationStatus` is `documented`, `dynamic`, or `needs-review`. Each audited placement carries `orientationEvidence`. `dynamic` means there is no fixed bearing (for example, a traversing turret); `needs-review` deliberately means the manual/model does not justify guessing a rotation.
  * `orientationFeature` adds a procedural facing/axis cue to the simplified preview mesh; it does not claim to reproduce the block's full in-game model.
  * Mastodon rendering highlights the number of current-step placements needing review. Selecting a block exposes its status, degree rotation, and evidence in the inspector.

---

## 3. Voxel JSON Schema Specification (Tier 1)

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "id": "MAS-01",
  "name": "MASTODON-01 MKI Expeditionary Excavator & Mobile FOB",
  "chapter": 10,
  "gridSize": "Large",
  "blockMeters": 2.5,
  "stats": {
    "totalBlocks": 1128,
    "dryMassTonnes": 1258.2,
    "pcu": 10080,
    "dimensionsMeters": [27.5, 105.0, 17.5],
    "dimensionsBlocks": [11, 42, 7],
    "toolReachTargetMeters": 35.0,
    "powerBaseloadMW": 30.0,
    "powerPeakMW": 72.0,
    "batteryCapacityMWh": 18.0,
    "orePayloadTonnes": 2500.0
  },
  "bounds": {
    "min": [-5, 1, -29],
    "max": [5, 7, 12]
  },
  "palette": [
    { "slot": 0, "name": "Citadel Charcoal", "hex": "#1A1D20", "rgb": [26, 29, 32] },
    { "slot": 1, "name": "Structural Slate", "hex": "#42464D", "rgb": [66, 70, 77] },
    { "slot": 2, "name": "Warning Amber", "hex": "#E06D14", "rgb": [224, 109, 20] },
    { "slot": 3, "name": "Tactical White", "hex": "#D4D8DD", "rgb": [212, 216, 221] },
    { "slot": 4, "name": "Processing Teal", "hex": "#1ABC9C", "rgb": [26, 188, 156] },
    { "slot": 5, "name": "Habitat White", "hex": "#E8ECF0", "rgb": [232, 236, 240] },
    { "slot": 6, "name": "Machine Olive", "hex": "#4A5643", "rgb": [74, 86, 67] },
    { "slot": 7, "name": "Cryo Cyan", "hex": "#1E6B7B", "rgb": [30, 107, 123] },
    { "slot": 8, "name": "Mechanical Gunmetal", "hex": "#2B2E33", "rgb": [43, 46, 51] },
    { "slot": 9, "name": "Grate Steel", "hex": "#50555C", "rgb": [80, 85, 92] }
  ],
  "steps": [
    {
      "step": "10.1",
      "phase": "10-1",
      "level": "L2",
      "title": "Twin Longitudinal Chassis Box Girders & Transverse Ties",
      "subsystem": "structure",
      "partsTray": [
        { "type": "Heavy Armor Full Block", "qty": 40, "slot": 0, "mass": 34.0, "pcu": 40, "note": "Longitudinal runs at X = ±3" }
      ],
      "checks": [
        "Two parallel box girders running 20 blocks from Z -8 to Z +12."
      ],
      "blocks": [
        {
          "pos": [-3, 2, 12],
          "shape": "cube",
          "subtype": "LargeHeavyBlockArmorBlock",
          "paletteSlot": 0,
          "subsystem": "structure"
        }
      ]
    }
  ]
}
```

---

## 4. Subsystem Taxonomies & Morphologies

| Subsystem Code | Layer Name | Represented Blocks | Visual Color |
|---|---|---|---|
| `structure` | Armor & Hull | Full blocks, 2×1 slopes, half blocks, corner chamfers | Charcoal / Slate (`#1A1D20` / `#42464D`) |
| `conveyor` | Conveyor Pipeline | Tubes, curved pipes, junctions, sorters, ejectors | Processing Teal (`#1ABC9C`) |
| `power` | Power & Fuel | Small/Large reactors, batteries, H2 engines, H2 tanks | Warning Amber / Cryo Cyan (`#E06D14` / `#1E6B7B`) |
| `propulsion` | Powertrain / Wheels | 5×5 wheel suspensions, outrigger pistons, drawbar hitch | Mechanical Gunmetal (`#2B2E33`) |
| `avionics` | Bridge & Control | Helm cockpits, programmable blocks, event controllers, CIWS | Habitat White / Tactical White (`#E8ECF0`) |
| `flightdeck` | Flight Infrastructure | Mule 7×7 pad, Kestrel pads, pad maintenance welders | Heavy Armor / White Chevrons (`#D4D8DD`) |
| `boom` | Articulated Tools | Turret azimuth rotor, shoulder/elbow hinges, laser multitools | Hazard Gold / Amber (`#C8A114`) |

---

## 5. Machine-Readable 2D Sight Projections (Tier 2)

For text-based AI models, the visualizer generates pure ASCII layer slices:

```
========================================================================
SE 2D SLICE REPORT · MAS-01 · Level L2 (Y = 2)
Orientation: ▲ North/Bow (+Z) · ◄ Port (-X) · Starboard (+X) ►
========================================================================
 Z \ X    -4  -3  -2  -1   0  +1  +2  +3  +4
---------------------------------------------
 +12 │    ·   ■   ■   ■   ■   ■   ■   ■   ·   ◄── Bow Bulkhead
 +11 │    ·   ■   ·   ·   ·   ·   ·   ■   ·
  +9 │    W   ■   ■   ■   ■   ■   ■   ■   W   ◄── Axle 1 Cross-Tie & Wheels
  +4 │    W   ■   ■   ■   ■   ■   ■   ■   W   ◄── Axle 2 Cross-Tie & Wheels
  +2 │    ·   ■   ·   ·   C   ·   ·   ■   ·   ◄── Ore Buffer Sump
  -1 │    W   ■   ■   ■   ■   ■   ■   ■   W   ◄── Axle 3 Cross-Tie & Wheels
  -8 │    ·   ■   ■   ■   H   ■   ■   ■   ·   ◄── Aft Hitch Bulkhead & Rotor
---------------------------------------------
Legend: ■ Armor Block  ○ Conveyor  ▲ Half/Bevel  W 5x5 Wheel  R Reactor
        B Battery      C Cargo     K Cockpit     T Turret     H Rotor/Hinge
```

---

## 6. Local Server REST API Endpoints

The zero-dependency Python server (`studio/serve.py`) serves the static PWA and provides programmatically queryable endpoints:

| Endpoint | Method | Format | Purpose |
|---|---|---|---|
| `/` | `GET` | HTML | Interactive PWA interface with 3D/2D views |
| `/api/status` | `GET` | JSON | Service health and registered chapter models |
| `/api/chapters` | `GET` | JSON | Master fleet chapter registry index |
| `/api/model/:id` | `GET` | JSON | Complete 3D Voxel JSON schema for vehicle |
| `/api/slice/:id?layer=L2&step=4` | `GET` | Text | Deterministic 2D ASCII floorplan for AI consumption |
| `/api/notes/:id` | `GET` | JSON | Retrieve collaborative engineering notes |
| `/api/note/:id?step=10.1` | `POST` | JSON | Append / update collaborative note for step |

Command to start:
```bash
python3 studio/serve.py 8080
```

---

## 7. Collaborative Human & AI Workflow Protocol

To enable seamless pairing between human engineers and AI agents:
1. **Interactive Verification Checklist**: Checkboxes for step checks and phase inspection gates persist in browser `localStorage`.
2. **Engineering Notes & Observations**: Freeform text annotations per step save in real-time, allowing humans to note clearances or adjustments and AI to record verification passes.
3. **Markdown Collaboration Export**: `Collab Log` button downloads a timestamped markdown report (`<MODEL>-collaboration-report.md`) detailing verification status and notes across all steps.
4. **Dynamic Fleet Switcher**: Switch between vehicles/structures via the header selector without page reloads.

---

## 8. Current Chapter Registry (`studio/data/chapters.json`)

The registry contains reported model statistics. These values and the authored placements are design data, not proof of a validated in-game blueprint.

| Chapter | Code | Model Name | Grid | Total Blocks | Dry Mass | PCU |
|---|---|---|---|---|---|---|
| **Ch 1** | `ORC-01` | ORCA-01 Heavy Mining Carrier | Large | 653 | 764.0 t | 4,500 |
| **Ch 2** | `REM-01` | REMORA-01 Vertical Shaft Driller | Small | 133 | 48.5 t | 1,774 |
| **Ch 3** | `MUL-01` | MULE-01 Heavy Freight Shuttle | Large | 142 | 128.0 t | 1,850 |
| **Ch 4** | `GND-01` | Ground Base, Refinery Complex & Fleet C2 | Large | 1,420 | 2,840.0 t | 14,200 |
| **Ch 5** | `EXT-01` | Rig Linear Extruder Ring Shipyard | Large | 860 | 1,850.0 t | 8,900 |
| **Ch 6-8**| `RIG-01` | Floating Rig Structure (25 kt Fortress) | Large | 4,980 | 25,000.0 t | 29,500 |
| **Ch 10** | `MAS-01` | MASTODON-01 MKI Excavator & Mobile FOB | Large | 1,128 | 1,258.2 t | 10,080 |
| **Ch 11** | `CAT-01` | CATERPILLAR-01 Bulk Land Train | Large | 404 | 368.5 t | 3,950 |
| **Ch 12** | `KES-01` | KESTREL-01 Prospector Scout Drone | Small | 68 | 4.8 t | 850 |
| **Ch 13** | `ALB-01` | ALBATROSS-01 Utility & Field Repair Craft | Small | 84 | 8.5 t | 1,120 |

---

## 9. Validation Status

| Item | Status | Verification Detail |
|---|---|---|
| Model schema and coordinate bounds | Automated | `python3 studio/chapter_indexer.py --all` validates the current 10 model files |
| CH10 manual compiler | Automated | Tests confirm 47 manual steps compile while preserving explicit placements |
| Browser rendering and responsive layout | Requires browser QA | Procedural meshes are illustrative previews, not game-rendered block geometry |
| Offline installation and cache behavior | Requires browser QA | Service-worker precache entries are derived from the chapter registry |
| In-game geometry, mass, PCU, and capacities | Not verified | Compare against a trusted `.sbc` blueprint and validate in Creative before operational use |
| SBC XML export | Draft only | Export contains the authored visual placement subset and is not a validated game blueprint |
