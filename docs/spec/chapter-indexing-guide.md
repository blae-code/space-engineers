# SE 3D Studio Chapter Data Workflow

The Studio separates canonical build prose from spatial model data:

- `manuals/chapters/` contains the canonical human-readable instructions.
- `studio/data/<model>.json` contains model metadata and explicitly authored voxel placements.
- `studio/data/chapters.json` is a generated runtime registry used by the frontend and service worker.

The compiler joins steps by their IDs. It does not infer or generate block coordinates.

## Add a chapter

Scaffold the manual and model JSON:

```bash
python3 studio/chapter_indexer.py --new 14 kestrel KES-02 \
  "KESTREL-02 Survey Drone" Small "Air Tier · Survey"
```

This creates `manuals/chapters/14-kestrel.md` and `studio/data/kestrel.json`. The JSON is deliberately incomplete until the author fills in the model's palette, bounds, statistics, phases, and explicit placements.

For every placement step:

1. Add a `### Step <id> · <title>` section to the manual.
2. Add a model `steps[]` object with the same `step` ID and a non-empty `blocks[]` array.
3. Record integer Space Engineers grid coordinates, a supported palette slot, shape, and subsystem for every visual placement.
4. Ensure `bounds` enclose every placement and dimensions/statistics match the in-game reference.

Then compile the prose fields into the model:

```bash
python3 studio/chapter_indexer.py \
  --compile manuals/chapters/14-kestrel.md \
  --model studio/data/kestrel.json
```

Compilation fails if the manual and model step IDs differ or if a step lacks explicit placements. Existing coordinate arrays are preserved.

## Validate and publish the registry

```bash
python3 studio/chapter_indexer.py --validate studio/data/kestrel.json
python3 studio/chapter_indexer.py --all
python3 studio/chapter_indexer.py --stats
```

`--all` checks every chapter model before writing the sorted chapter registry. Validation checks required metadata, grid scale, palette uniqueness/colors, integer coordinates, palette references, duplicate step IDs, and declared spatial bounds. A failed validation leaves `chapters.json` unchanged.

The chapter selector and service-worker precache both read `studio/data/chapters.json`; do not maintain parallel chapter lists in HTML or JavaScript.

## Coordinate and scale convention

Model placement coordinates are integer grid positions in Space Engineers coordinates:

- Large grid blocks: `blockMeters = 2.5`.
- Small grid blocks: `blockMeters = 0.5`.
- `Three.X = SE.X * blockMeters`.
- `Three.Y = (SE.Y - 0.5) * blockMeters`.
- `Three.Z = -SE.Z * blockMeters`.

The sign change on Z keeps the bow (+Z) at the top of a plan-view slice and in front of the viewer. Camera framing, selection bounds, clipping, and procedural block geometry use the model's grid scale.

## Current data limitation

Coordinates are only as reliable as the explicit source placements and reference they were authored from. A syntactically valid model is not proof that a blueprint matches an in-game ship; verify dimensions, orientation, palette, PCU, and mass against a trusted blueprint before describing a model as blueprint-accurate.
