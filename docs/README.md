# Space Engineers 1 · Documentation Architecture & Map

Authoritative reading order, directory taxonomy, and specification map for the Space Engineers 1 **Floating Rig & Terrestrial Fleet** repository.

Authority: Blae Decisions 1-54 (Canon) · `CLAUDE(RIG).md` · `docs/rig/lego-instructions-v1.0.md`

---

## 1. Document Reading Order & Authority Hierarchy

Per `CLAUDE(RIG).md` §1 and `GEMINI.md` §1, technical authority follows this strict hierarchy:
> **Blae's Decisions** (`docs/rig/CHANGELOG.md`) > **Hard Constraints** (`CLAUDE(RIG).md`) > **Engineering Rules** (`CLAUDE.md`) > **Master Construction Manual** (`docs/rig/lego-instructions-v1.0.md`) > **Reference Guides** (`reference/`).

Decisions are canon; reference guides are guides only.

---

## 2. Directory Layout & Document Map

```
docs/
├── README.md                      # This master documentation index & reading map
├── ideas.md                       # Backlog of unscheduled proposals & feature concepts
├── review-queue.md                # Multi-agent handoff review queue (Gemini -> Claude -> Qwen)
│
├── rig/                           # Active Floating Rig & Terrestrial Fleet Engineering Specs
│   ├── lego-instructions-v1.0.md  # AUTHORITATIVE MASTER BUILD MANUAL (All 13 Chapters)
│   ├── cascading-workflow.md      # Autonomous cascading workflow & 12-pillar Mastodon benchmark
│   ├── CHANGELOG.md               # Canon decisions log (Decisions 1-70)
│   ├── project-map.md             # Living system breakdown, status map & build order
│   ├── verify.md                  # In-game verification tracking checklist (†)
│   ├── mods.md                    # Active mod whitelist & configuration rules
│   ├── mods-unsubscribe.txt       # Tool-referenced workshop unsubscription list
│   ├── testworld-mods.txt         # Tool-referenced RIG Test creative world mod list
│   ├── chapters/                  # 11 modular chapter instruction sources
│   │   ├── orca-instructions.md          # Ch 1 · ORCA-01 Heavy Mining Carrier (17 × 19 × 7 B)
│   │   ├── remora-instructions.md        # Ch 2 · REMORA-01 Vertical Shaft Driller (11 × 11 sb)
│   │   ├── mule-instructions.md          # Ch 3 · MULE-01 Heavy Freight Shuttle (7 × 7 B)
│   │   ├── ground-base-instructions.md   # Ch 4 · Ground Base, Refinery Complex & C2 Hub
│   │   ├── extruder-ring-instructions.md # Ch 5 · Rig Linear Extruder Ring Shipyard
│   │   ├── floating-rig-instructions.md  # Ch 6-8 · Floating Rig Fortress (S0, S1-S6, S7-S9)
│   │   ├── mastodon-instructions.md      # Ch 10 · MASTODON-01 Heavy Excavator (11 × 25 × 6 B)
│   │   ├── caterpillar-instructions.md   # Ch 11 · CATERPILLAR-01 Land Train (42 B Consist)
│   │   ├── kestrel-instructions.md       # Ch 12 · KESTREL-01 Prospector Scout (7 × 9 × 5 sb)
│   │   ├── albatross-instructions.md     # Ch 13 · ALBATROSS-01 Utility & Repair Craft (9 × 11 sb)
│   │   └── rig-os-contract.md            # Ch 9 · RIG//OS Hardware Abstraction & IGC Contract
│   └── archive/                   # Superseded analytical research
│       └── xu1-analysis.md        # Reverse-engineering teardown of Workshop PAM miner
│
├── protocol/                      # Communications & Network Specifications
│   └── igc.md                     # Master Inter-Grid Communication (IGC) Standard
│
├── merge/                         # Frozen Fleet-to-Rig Merge Audit Records
│   ├── 01-inventory.md            # Inventory of legacy Fleet codebase & scripts
│   ├── 02-mapping.md              # Mapping of Fleet components into Rig architecture
│   └── 03-decisions.md            # Decision brief answered by Blae (2026-10-16)
│
├── build-specs/                   # Tool-Gated In-Game Deployment Files
│   ├── mining-drone.script.cs     # Snapshot script compiled by checkpoint.py / snapshot.fish
│   └── mining-drone.md            # Assembly guide for the legacy single-drone miner
│
└── superpowers/                   # Legacy Drone Fleet Plans & Cards (Frozen at fleet-freeze)
    ├── plans/                     # Slices 1-3 plans and card decks (C01-C32)
    └── specs/                     # Slices 1-3 architectural design specifications
```

---

## 3. Core Working Documents Summary

### Active Specifications & Manuals
* **Master Build Manual ([`docs/rig/lego-instructions-v1.0.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/lego-instructions-v1.0.md)):** Unified 9,200+ line visual construction manual covering all terrestrial vehicles, installations, the 25 kt floating station, and AI terminal contracts.
* **Cascading Workflow Engine ([`docs/rig/cascading-workflow.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/cascading-workflow.md)):** Autonomous cascading refinement workflow using Chapter 10 as the 12-pillar standard across all chapters.
* **Canon Decisions Log ([`docs/rig/CHANGELOG.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/CHANGELOG.md)):** Historical and active rulings by Blae. Every decision logged here overrides any previous guide or specification.
* **Project Status Map ([`docs/rig/project-map.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/project-map.md)):** High-level view of all active, open, and deferred components across air, ground, and orbital tiers.
* **Verification Checklist ([`docs/rig/verify.md`](file:///home/blae/Documents/Code/space-engineers/docs/rig/verify.md)):** Itemized list of physics assumptions, thrust ratings, and game values tagged with `†` awaiting testing in the RIG Test creative world.

### Communications & Network Standards
* **IGC Protocol Specification ([`docs/protocol/igc.md`](file:///home/blae/Documents/Code/space-engineers/docs/protocol/igc.md)):** Definitive semicolon-separated (`;`) message format, channel namespaces (`RIG.*`, `FLT.*`, `TMS.*`), and heartbeat watchdog rules.

### Workflow & Governance
* **Review Queue ([`docs/review-queue.md`](file:///home/blae/Documents/Code/space-engineers/docs/review-queue.md)):** Multi-agent coordination board. Gemini drafts and logs commits with `pending` status; Claude Code reviews and signs off; Qwen executes reviewed task cards.
* **Ideas Backlog ([`docs/ideas.md`](file:///home/blae/Documents/Code/space-engineers/docs/ideas.md)):** Unscheduled brainstorm ideas awaiting specification or carding.

