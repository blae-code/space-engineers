# Space Engineers 1 · Engineering Documentation Architecture

Authoritative reading order, directory taxonomy, and specification map for the Space Engineers 1 **Floating Rig & Terrestrial Fleet** repository.

**Authority:** Blae Decisions 1–72 (Canon) · `CLAUDE(RIG).md` · [`manuals/MASTER-BUILD-MANUAL.md`](../manuals/MASTER-BUILD-MANUAL.md)

---

## 1. Authority Hierarchy & Reading Order

Per `CLAUDE(RIG).md` §1 and `GEMINI.md` §1, technical authority follows this strict hierarchy:
> **Blae's Decisions** ([`docs/CHANGELOG.md`](CHANGELOG.md)) > **Hard Constraints** (`CLAUDE(RIG).md`) > **Engineering Rules** (`CLAUDE.md`) > **Master Construction Manual** ([`manuals/MASTER-BUILD-MANUAL.md`](../manuals/MASTER-BUILD-MANUAL.md)) > **Archived Guides** ([`archive/reference-guides/`](../archive/reference-guides/)).

Decisions are canon; reference guides are guides only.

---

## 2. Repository Architecture & Directory Taxonomy

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
│   ├── README.md                  # This master documentation index & reading map
│   ├── CHANGELOG.md               # Canon decisions log (Decisions 1-72, Canon Authority)
│   ├── project-map.md             # Master system breakdown & build order
│   ├── verify.md                  # In-game verification tracking checklist (†)
│   ├── mods.md                    # Active mod whitelist & configuration rules
│   ├── cascading-workflow.md      # 12-pillar Mastodon benchmark standard
│   ├── review-queue.md            # Multi-agent handoff review queue
│   ├── ideas.md                   # Backlog of unscheduled proposals
│   ├── protocol/                  # Communications standards
│   │   └── igc.md                 # Master Inter-Grid Communication (IGC) Standard
│   ├── spec/                      # Schemas
│   │   └── blueprint-voxel-schema.md # Master 3D Voxel/Blueprint Schema
│   └── config/                    # Mod config lists
│       ├── mods-unsubscribe.txt
│       └── testworld-mods.txt
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

## 3. Core Working Documents Summary

### Engineering Governance
* **Canon Decisions Log ([`docs/CHANGELOG.md`](CHANGELOG.md)):** Historical and active rulings by Blae (Decisions 1–72). Every decision logged here overrides any previous guide or specification.
* **Project Status Map ([`docs/project-map.md`](project-map.md)):** Living system status breakdown across air, ground, surface yard, and floating fortress tiers.
* **Verification Checklist ([`docs/verify.md`](verify.md)):** Itemized list of physics assumptions, thrust ratings, and game values tagged with `†` awaiting in-game creative testing.
* **Active Mod Whitelist ([`docs/mods.md`](mods.md)):** Strict mod scoping rules under Constraint C2.
* **Cascading Workflow Engine ([`docs/cascading-workflow.md`](cascading-workflow.md)):** 12-pillar Mastodon benchmark quality standard.

### Communications & Network Standards
* **IGC Protocol Specification ([`docs/protocol/igc.md`](protocol/igc.md)):** Definitive semicolon-separated (`;`) message format, channel namespaces (`RIG.*`, `FLT.*`, `TMS.*`), and heartbeat watchdog rules.
* **Blueprint Voxel Schema ([`docs/spec/blueprint-voxel-schema.md`](spec/blueprint-voxel-schema.md)):** 3D voxel coordinate data contract for the interactive studio.

### Workflow & Collaboration
* **Review Queue ([`docs/review-queue.md`](review-queue.md)):** Multi-agent coordination board. Gemini drafts; Claude Code reviews and signs off; Qwen executes reviewed task cards.
* **Ideas Backlog ([`docs/ideas.md`](ideas.md)):** Unscheduled brainstorm ideas awaiting specification or carding.
