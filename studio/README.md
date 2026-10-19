# Space Engineers · 3D/2D Blueprint Studio & PWA

Offline interactive 3D WebGL blueprint visualizer, 2D orthographic deck slice engine, and collaborative build assistant for the Space Engineers Floating Rig and Logistics Fleet.

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

* **Offline PWA:** Click the browser install icon or enable offline caching via `sw.js` to run 100% offline.
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
├── manifest.json, sw.js       # Offline ServiceWorker & Web App Manifest
├── icon.svg                   # Vector application icon
├── css/app.css                # Industrial brutalist dark UI theme
├── js/
│   ├── app.js                 # UI event coordinator & chapter state controller
│   ├── VoxelWorld.js          # Three.js 3D viewport, ghost hologram renderer & orbit controls
│   ├── Slicer2D.js            # Orthographic 2D vector SVG & ASCII deck matrix generator
│   └── SnapshotApi.js         # PNG high-res capture & JSON export engine
├── vendor/                    # Local Three.js r171 & OrbitControls modules
├── data/                      # Compiled chapter models (chapters.json, mastodon.json, orca.json, etc.)
└── tools/                     # Model compile scripts (compile_mastodon.py, compile_fleet.py)
```

---

## 3. Recompiling Models

To update model datasets from markdown chapter manuals:
```bash
python3 studio/tools/compile_mastodon.py
python3 studio/tools/compile_fleet.py
```
All outputs are written directly to `studio/data/`.
