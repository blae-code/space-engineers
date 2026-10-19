# Space Engineers 1 · Floating Rig & Terrestrial Fleet

Autonomous industrial logistics fleet, mobile factory fortress, and multi-agent AI automation scripts for **Space Engineers 1** on Linux/Proton.

Built with [MDK2](https://github.com/malware-dev/MDK-SE) (C# 6, zero-GC, `minify=full`). Technical authority governed by Blae's Canon Decisions ([`docs/CHANGELOG.md`](docs/CHANGELOG.md)).

---

## 1. Quick Navigation

* **[Construction Manuals & Build Guides](manuals/README.md):** Complete, human-facing Lego-style assembly manuals for all 13 craft and installations.
  - Master manual: [`manuals/MASTER-BUILD-MANUAL.md`](manuals/MASTER-BUILD-MANUAL.md) (9,200+ lines, visual cross-sections, parts trays, inspection gates)
* **[Interactive 3D/2D Blueprint Studio](studio/README.md):** Offline desktop PWA with 3D ghost hologram step scrubber, exploded views, and 2D orthographic deck slices (`python3 studio/serve.py`).
* **[Programmable Block Scripts](scripts/README.md):** Active C# 6 zero-GC MDK2 scripts for all 10 fleet units and Rig OS.
* **[Engineering Documentation](docs/README.md):** Canon decisions log, living project status map, verification list, mod rules, and master IGC protocol dictionary.
* **[Historical Archive](archive/README.md):** Fully preserved non-canon reference guides, legacy deep-space drone fleet codebase (`Fleet.*`), and early prototypes.

---

## 2. Fleet & Station Summary

| Designation | Name & Role | Class & Tier | Manual | Script Project |
|---|---|---|---|---|
| **ORC-01** | Heavy Mining Carrier | Large Grid · Air Tier | [`manuals/01-orca-carrier.md`](manuals/chapters/01-orca-carrier.md) | [`scripts/Carrier.Orca`](scripts/Carrier.Orca/) |
| **REM-01** | Vertical Shaft Driller | Small Grid · Air Tier | [`manuals/02-remora-driller.md`](manuals/chapters/02-remora-driller.md) | [`scripts/Drone.Remora`](scripts/Drone.Remora/) |
| **MUL-01** | Heavy Freight Shuttle | Large Grid · Air Tier | [`manuals/03-mule-shuttle.md`](manuals/chapters/03-mule-shuttle.md) | [`scripts/Drone.Mule`](scripts/Drone.Mule/) |
| **GND-01** | Ground Base & Fleet C2 | Large Grid · Surface Core | [`manuals/04-ground-base.md`](manuals/chapters/04-ground-base.md) | [`scripts/Base.C2`](scripts/Base.C2/) |
| **EXT-01** | Extruder Ring Shipyard | Large Grid · Surface Yard | [`manuals/05-extruder-ring.md`](manuals/chapters/05-extruder-ring.md) | [`scripts/Yard.Extruder`](scripts/Yard.Extruder/) |
| **RIG-01** | Floating Rig Fortress (S0–S9) | 25 kt Large Grid · Station | [`manuals/06-floating-rig.md`](manuals/chapters/06-floating-rig.md) | [`scripts/Rig.OS`](scripts/Rig.OS/) |
| **RIG//OS** | Operating System & IGC | Logic & Abstraction | [`manuals/09-rig-os.md`](manuals/chapters/09-rig-os.md) | [`docs/protocol/igc.md`](docs/protocol/igc.md) |
| **MAS-01** | MASTODON-01 MKI Excavator | 10× 5×5 Large Grid Train | [`manuals/10-mastodon-train.md`](manuals/chapters/10-mastodon-train.md) | [`scripts/Excavator.Mastodon`](scripts/Excavator.Mastodon/) |
| **CAT-01** | CATERPILLAR-01 Bulk Land Train | 16-wheel Large Grid Train | [`manuals/11-caterpillar-train.md`](manuals/chapters/11-caterpillar-train.md) | [`scripts/Train.Caterpillar`](scripts/Train.Caterpillar/) |
| **KES-01** | KESTREL-01 Prospector Scout | Small Grid · Air Recon | [`manuals/12-kestrel-scout.md`](manuals/chapters/12-kestrel-scout.md) | [`scripts/Drone.Kestrel`](scripts/Drone.Kestrel/) |
| **ALB-01** | ALBATROSS-01 Field Utility & Repair | Small Grid · Air Repair | [`manuals/13-albatross-utility.md`](manuals/chapters/13-albatross-utility.md) | [`scripts/Drone.Albatross`](scripts/Drone.Albatross/) |

---

## 3. Directory Layout

```
space-engineers/
├── manuals/                       # Human-Facing Vehicle & Station Construction Manuals
│   ├── README.md                  # Quick index of all craft manuals & build order
│   ├── MASTER-BUILD-MANUAL.md     # Authoritative Master Construction Manual (all 13 chapters)
│   └── chapters/                  # 11 Modular Lego-style craft assembly guides
│
├── studio/                        # Interactive 3D/2D Blueprint Studio PWA & REST API
│   ├── index.html                 # 3D viewport, 2D floorplans, step scrubber, collab mode
│   ├── serve.py                   # Zero-dependency local server (python3 studio/serve.py)
│   ├── manifest.json, sw.js       # Offline desktop PWA support
│   └── data/                      # 10 compiled chapter JSON models
│
├── scripts/                       # Active In-Game Programmable Block Scripts (MDK2 C# 6)
│   ├── Scripts.sln                # Master solution linking all 10 PB projects
│   ├── Shared/                    # Shared library (Net, Util, Math)
│   └── <Project>/                 # Individual PB projects (Carrier.Orca, Drone.Remora, etc.)
│
├── docs/                          # Engineering Governance, Specifications & Protocols
│   ├── README.md                  # Master documentation index & reading map
│   ├── CHANGELOG.md               # Canon decisions log (Decisions 1-72, Canon Authority)
│   ├── project-map.md             # Master system breakdown & build order
│   ├── verify.md                  # In-game verification tracking checklist (†)
│   ├── mods.md                    # Active mod whitelist & configuration rules
│   ├── cascading-workflow.md      # 12-pillar Mastodon benchmark standard
│   ├── review-queue.md            # Multi-agent handoff review queue
│   ├── ideas.md                   # Backlog of unscheduled proposals
│   ├── protocol/                  # Communications standards (igc.md)
│   ├── spec/                      # Schemas (blueprint-voxel-schema.md)
│   └── config/                    # Mod config lists
│
├── tools/                         # Active Development & Deployment Tools
│   ├── bp-inspect.py, world-mods.py, steam-unsub.py
│   ├── check-size.fish, snapshot.fish
│   ├── card-check.py, cards.py, checkpoint.py, next-card.py
│   └── hooks/
│
└── archive/                       # Archived Non-Canon, Deprecated & Historical Material
    ├── README.md                  # Master archive catalog
    ├── reference-guides/          # Early non-canon Claude.ai snapshots
    ├── legacy-drone-fleet/        # Frozen deep-space fleet codebase, cards & specs
    ├── merge-audit/               # Historical Fleet-to-Rig merge records (2026-10-16)
    ├── prototypes/                # Deprecated standalone HTML prototypes
    └── backups/                   # Preserved backups and empty placeholders
```

---

## 4. Engineering Invariants & Sandbox Rules

* **Rule C4 — Native safety layer:** PBs make decisions; native blocks provide failsafe operation. No critical life support or ship survival function depends solely on a PB.
* **Pure core, thin adapters:** All algorithmic flight math and state machines reside in pure, testable classes; only `Io/` and `Subsystems/` touch game terminal blocks.
* **Zero-GC in the main loop:** Zero heap allocations inside `Main()`, `Update1`, `Update10`, or `Update100`. Preallocate in constructor, reuse buffers, use `SbFormat` for numbers, and literal tables for enums.
* **`minify=full` immunity:** MDK2 fully minifies and renames all symbols. Code never depends on identifier names, runtime reflection, or `ToString()` on numbers/enums.
* **Grid scoping:** Every block query strictly scopes via `b => b.CubeGrid == Me.CubeGrid`.
* **C# 6 compiler constraints:** Scripts conform strictly to C# 6 (`<LangVersion>6</LangVersion>`). Banned: LINQ, `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`, `System.Globalization`.

---

## 5. Multi-Agent Development Workflow

Work on this repository is divided among three collaborating agents:
* **Gemini (Antigravity):** High-throughput drafting, specifications, master manuals, 3D/2D visualizer data, bulk code architecture. Rules: [`GEMINI.md`](GEMINI.md).
* **Claude Code:** Architecture review, gate verification, game-API adapters, gate ownership (`tools/`). Rules: [`CLAUDE.md`](CLAUDE.md).
* **Qwen (local Continue):** Task card execution from reviewed plans.
