# Chapter Indexing & Blueprint Conversion Guide · SE 3D Studio

Standard operating procedure for converting, indexing, and validating Space Engineers markdown build manuals into machine-readable Voxel JSON datasets for dual human-AI spatial inspection.

---

## 1. System Architecture & Dual-Sight Pipeline

```
  ┌────────────────────────────────────────────────────────────────────────┐
  │                 SPACE ENGINEERS BUILD MANUAL (Markdown)                │
  │                  docs/rig/chapters/*-instructions.md                   │
  └───────────────────────────────────┬────────────────────────────────────┘
                                      │
                     [web/chapter_indexer.py]
                                      │
                                      ▼
  ┌────────────────────────────────────────────────────────────────────────┐
  │                    UNIFIED VOXEL JSON SPECIFICATION                    │
  │                           web/data/*.json                              │
  └───────────────┬───────────────────────────────┬────────────────────────┘
                  │                               │
                  ▼                               ▼
     ┌────────────────────────┐      ┌────────────────────────┐
     │ 2D PROJECTION ENGINE   │      │ 3D WEBGL STUDIO        │
     │ Slicer2D.js            │      │ VoxelWorld.js          │
     │ · SVG Vector Floorplan │      │ · Procedural Geometry  │
     │ · ASCII Text Matrix    │      │ · Projector Hologram   │
     │ · Level Slicing (L1-L7)│      │ · Cutaways & X-Ray     │
     └────────────────────────┘      └────────────────────────┘
```

---

## 2. Command-Line Indexing Operations

The primary compiler utility is `web/chapter_indexer.py`.

### A. Compile a Single Chapter
```bash
python3 web/chapter_indexer.py docs/rig/chapters/mastodon-instructions.md
```
* Ingests the specified markdown file.
* Parses steps, parts trays, instructions, terminal settings, checks, and voxel coordinates.
* Generates validated JSON in `web/data/<consist>.json`.

### B. Compile the Entire Fleet & Update Registry
```bash
python3 web/chapter_indexer.py --all
```
* Compiles all 10 registered fleet chapters across `docs/rig/chapters/`.
* Writes `web/data/chapters.json` master index.

### C. Validate Schema & Coordinate Invariants
```bash
python3 web/chapter_indexer.py --validate web/data/mastodon.json
```
Verifies:
* 100% schema conformance (`id`, `stats`, `bounds`, `palette`, `steps`).
* Zero out-of-bounds blocks (`min <= pos <= max`).
* Zero palette violations (`0 <= slot < 10`).
* Subsystem layer integrity.

### D. Display Fleet Summary Table
```bash
python3 web/chapter_indexer.py --stats
```
Outputs formatted terminal table with chapter number, consist ID, name, grid size, block count, dry mass, and PCU.

---

## 3. Chapter Markdown Conventions

To ensure zero-error automated indexing, author chapter markdown files following these structural standards:

### 3.1 Step Header Format
Every build step must use a third-level heading (`###`) adhering to this format:
```markdown
### Step 10.4 · 5×5 Suspension Wheel Assemblies & Outriggers (L1)
```
* **Step Number:** Numeric or dotted decimal (e.g. `Step 1`, `Step 10.4`, `Step 6.2`).
* **Separator:** Interpunct (`·`) or colon (`:`).
* **Title:** Terse description of the module or assembly.
* **Level Tag:** Parenthetical deck designator at the end (e.g. `(L1)`, `(L2)`, `(L1-L3)`).

### 3.2 Pre-Step Parts Tray Table
Precede assembly instructions with a standard markdown table:
```markdown
#### Pre-Step Parts Tray
| Block Type | Qty | Hotbar / Slot | Mass / PCU | Notes / Functions |
| :--- | :--- | :--- | :--- | :--- |
| Heavy Armor Full Block | 40 | Slot 0 | 34.0 t / 40 PCU | Longitudinal chassis box girders |
| 5×5 Wheel Suspension Left | 4 | Slot 8 | 48.0 t / 400 PCU | Port traction bogies |
| Large Conveyor Junction | 6 | Slot 4 | 7.2 t / 150 PCU | Spine tie points |
```
* Column 1: Block Type name.
* Column 2: Integer quantity.
* Column 3: Hotbar paint palette slot (0 through 9).
* Column 4: Physical budget (tonnes and PCU).
* Column 5: Operational role.

### 3.3 Placement Instructions
Numbered sequential steps detailing spatial alignment:
```markdown
#### Placement Instructions
1. Align chassis girder at X = ±3 relative to centerline datum.
2. Advance longitudinal frame from station Z -8 forward to Z +12.
3. Install transverse reinforcement ties at stations Z = 9, 4, -1, 12, -8.
```

### 3.4 Terminal Configuration & Sliders
When configuring programmable blocks, rotors, hinges, or wheel suspensions:
```markdown
#### Terminal Configuration & Sliders
- Suspension Max Steer Angle: 18.0 deg
- Suspension Strength: 24 %
- Suspension Power: 60 %
- Friction: 45 %
- Speed Limit: 54 km/h
```

### 3.5 Verification Checks
Every step must contain inspection gates using checkbox or checkmark syntax:
```markdown
**✓ Check:** Longitudinal girders form a continuous 20-block parallel beam with 5-block internal transverse clearance.
- [ ] Verify conveyor port alignment matches lower keel trunk.
- [ ] Confirm no pinch points against wheel travel envelopes.
```

---

## 4. Coordinate System & Spatial Datum

Space Engineers blueprints adhere to a right-handed orthogonal coordinate triad:

```
                          Zenith (+Y)
                              ▲
                              │   ▲ Bow / Forward (+Z)
                              │  /
                              │ /
      Port (-X) ◄─────────────┼─────────────► Starboard (+X)
                             /│
                            / │
               Aft / Stern /  │
                 (-Z) ▼       ▼
                             Keel
```

### 4.1 Coordinate Conversions
| Axis | Direction | Large Grid Block Metric | Small Grid Block Metric |
| :--- | :--- | :--- | :--- |
| **X** | Port (`-X`) / Starboard (`+X`) | 1 block = 2.5 m | 1 block = 0.5 m |
| **Y** | Keel (`Y = 1`) / Zenith (`+Y`) | Level L1 = 1, L2 = 2, ... | Level L1 = 1, L2 = 2, ... |
| **Z** | Stern (`-Z`) / Bow (`+Z`) | Fore/Aft stations | Fore/Aft stations |

### 4.2 Three.js WebGL Mapping
Because Three.js standard coordinates place camera looking toward `-Z`:
* `Three.X = SE.X * BlockMeters`
* `Three.Y = (SE.Y - 0.5) * BlockMeters`
* `Three.Z = -SE.Z * BlockMeters`

### 4.3 Standard 10-Slot Paint Palette
| Slot | Name | Hex Code | Space Engineers Role |
| :--- | :--- | :--- | :--- |
| **0** | Citadel Charcoal | `#1A1D20` | Heavy Armor Hull, Longerons, Frame Keel |
| **1** | Structural Slate | `#42464D` | Light Armor Bulkheads, Decks, Workshop Ribs |
| **2** | Warning Amber | `#E06D14` | Outriggers, Hitch Points, Hazard Coamings |
| **3** | Tactical White | `#D4D8DD` | Flight Deck Chevrons, Landing Pads, Striping |
| **4** | Processing Teal | `#1ABC9C` | Conveyor Lines, Sorters, Refining Equipment |
| **5** | Habitat White | `#E8ECF0` | Bridge Interiors, Medical Bays, Quarters |
| **6** | Machine Olive | `#4A5643` | Large Assemblers, Machining Tools, Ammo Stores |
| **7** | Cryo Cyan | `#1E6B7B` | Hydrogen Tanks, O2/H2 Generators, Venting |
| **8** | Gunmetal Gray | `#2B2E33` | Wheel Hubs, Rotor Bases, Pedestals, Bogies |
| **9** | Grate Steel | `#50555C` | Catwalks, Stairs, Jefferies Grating |

### 4.4 Subsystem Classifications
All blocks belong to one of 7 canonical subsystem layers:
1. `structure`: Outer hull, bulkheads, decks, frame ties, armor slopes.
2. `conveyor`: Tubes, junctions, sorters, ejectors, cargo containers.
3. `power`: Nuclear reactors, batteries, hydrogen engines, fuel tanks.
4. `propulsion`: Wheel suspensions, atmospheric thrusters, ion thrusters, gyroscopes.
5. `avionics`: Cockpits, bridge consoles, programmable blocks, antennas, beacons.
6. `flightdeck`: Landing pads, arrestor connectors, blast shields, flight lighting.
7. `boom`: Articulated booms, drill heads, tool heads, rotor/hinge chains.

---

## 5. Iterative Conversion Checklist for New Chapters

When adding or updating a blueprint chapter:

1. **Author Markdown Manual:**
   - Draft `docs/rig/chapters/<consist>-instructions.md`.
   - Ensure step headers use `### Step <ID> · <Title> (<Level>)`.
   - Ensure parts trays have 5-column layout.
   - Include verification checks for each step.

2. **Register Consist Metadata:**
   - Open `web/chapter_indexer.py`.
   - Add entry to `CHAPTER_CONFIGS` with `id`, `chapter`, `name`, `grid`, `blockMeters`, `tier`, `bounds`, and `stats`.

3. **Execute Indexer:**
   ```bash
   python3 web/chapter_indexer.py docs/rig/chapters/<consist>-instructions.md
   ```

4. **Validate Output:**
   ```bash
   python3 web/chapter_indexer.py --validate web/data/<consist>.json
   ```
   Confirm 0 out-of-bounds blocks and 0 palette violations.

5. **Visual Inspection:**
   - Open SE 3D Studio in browser (`http://localhost:8080`).
   - Switch chapter dropdown to newly indexed model.
   - Scrub through all steps, verifying:
     - Projector hologram appearance on active step.
     - Section cutaway slicing in X, Y, and Z.
     - 2D deck floorplans on levels L1 through L7.
     - Subsystem layer toggle filtering.
     - Center of Mass (CoM) tracking.

6. **Regenerate Master Registry:**
   ```bash
   python3 web/chapter_indexer.py --all
   python3 web/chapter_indexer.py --stats
   ```

