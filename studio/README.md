# Space Engineers · 3D/2D Blueprint Studio & PWA

Offline interactive 3D WebGL blueprint visualizer, 2D orthographic deck slice engine, and collaborative build assistant for the Space Engineers Floating Rig, Mastodon MKI and Logistics Fleet.

The models are instructional voxel previews. Their placement subsets and reported stats are not validated in-game blueprints; verify geometry, mass, and PCU against a trusted `.sbc` source before operational use.

---

## 1. Quick Start

Run the zero-dependency local HTTP server from the repo root:
```bash
python3 studio/serve.py
```
Open your browser to:
```
http://localhost:8080
```

* **Installable PWA:** Use **Install** in the app toolbar when the browser offers installation, or use the browser's install/share menu. Serve from `localhost` or HTTPS; opening `index.html` as a file does not enable PWA features.
* **Offline use:** On the first online visit, wait until the toolbar reports **offline viewer ready**. The service worker caches the app shell and bundled chapter models. The online chapter list refreshes the cached models when available; locally authored notes and checklist state remain in browser storage.
* **Updates:** New service-worker versions wait while the current viewer is open. Choose **Update & refresh** when ready to activate the update and reload the viewer.
* **Data limits:** Bundled chapters remain available offline. User-provided/imported build data is not persisted by this PWA.
* **REST API:**
  - `GET /api/status`: Service health & available models
  - `GET /api/chapters`: Fleet chapter registry
  - `GET /api/model/:id`: Full 3D Voxel JSON model (e.g. `mastodon`, `orca`, `remora`)
  - `GET /api/slice/:id?layer=L2&step=4`: 2D ASCII slice text matrix for AI agents
  - `GET /api/notes/:id`: Retrieve saved collaborative notes
  - `POST /api/note/:id?step=10.1`: Save per-step engineering annotations

---

## 2. Directory Structure

```
studio/
├── index.html                 # Main PWA application viewport & controls
├── serve.py                   # Zero-dependency Python 3 HTTP/API server
├── manifest.json, sw.js       # Install metadata & offline Service Worker
├── icon.svg, icon-*.png       # Vector source and install/touch icons
├── css/app.css                # Industrial brutalist dark UI theme
├── js/
│   ├── app.js                 # UI event coordinator & chapter state controller
│   ├── VoxelWorld.js          # Three.js 3D viewport, ghost hologram renderer & orbit controls
│   ├── Slicer2D.js            # Orthographic 2D vector SVG & ASCII deck matrix generator
│   └── SnapshotApi.js         # PNG high-res capture & JSON export engine
├── vendor/                    # Local Three.js r171 & OrbitControls modules
├── data/                      # Compiled chapter models (chapters.json, mastodon.json, orca.json, etc.)
├── chapter_indexer.py         # Shared validator, manual compiler, and registry builder
└── tools/                     # Compatibility entry points to the shared chapter pipeline
```

## 3. Chapter Data Workflow

Chapter manuals in `manuals/chapters/` are the canonical source for build prose. Each model JSON stores explicit integer voxel placements keyed to the same step IDs. The compiler joins matching prose and authored coordinates; it never invents geometry.

Mastodon model metadata also records unverified design targets and the physical interfaces, future PB responsibilities, manual fallbacks, and commissioning checks for each bespoke subsystem. These entries describe planned integration only; they do not imply that scripts or in-game validation are supplied.

### Start a chapter

Create a manual and model scaffold. The scaffold is intentionally incomplete until its data is authored:
```bash
python3 studio/chapter_indexer.py --new 14 kestrel KES-02 "KESTREL-02 Survey Drone" Small "Air Tier · Survey"
```

Add the palette, bounds, stats, phases, and explicit `steps[].blocks` placements to `studio/data/kestrel.json`. Use one `### Step <id> · <title>` heading for each model step and keep the IDs identical.

### Compile and validate

Compile the manual prose into the matching model without changing its coordinate arrays:
```bash
python3 studio/chapter_indexer.py \
  --compile manuals/chapters/14-kestrel.md \
  --model studio/data/kestrel.json
```

Validate one model, or validate the database and regenerate its runtime registry:
```bash
python3 studio/chapter_indexer.py --validate studio/data/kestrel.json
python3 studio/chapter_indexer.py --all
python3 studio/chapter_indexer.py --stats
```

`--all` fails without updating the registry if any model has invalid metadata, out-of-bounds coordinates, or an undefined palette slot. Chapter options and offline precache entries are derived from `studio/data/chapters.json`, so adding a chapter requires no frontend or service-worker list edit. The `studio/tools/compile_mastodon.py` and `studio/tools/compile_fleet.py` scripts remain as compatibility commands and delegate to the shared pipeline.
