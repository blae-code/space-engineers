# Archived Legacy Deep-Space Drone Fleet

This directory houses the original deep-space asteroid mining fleet codebase, card decks, and deployment specifications developed prior to 2026-10-16.

**Status:** **FROZEN AT TAG `fleet-freeze` (Decision 12).**
**Active Successor:** Terrestrial Fleet & Rig ecosystem in `scripts/` and `manuals/`.

---

## 1. Directory Structure

```
archive/legacy-drone-fleet/
├── Fleet.sln                  # Legacy solution file linking Fleet projects
├── Fleet.Drone.Miner/         # Single PAM-successor asteroid mining drone PB script
├── Fleet.Carrier/             # Carrier bay beacon script
├── Fleet.Console/             # Mothership remote console script
├── Fleet.Engine/              # MDK2 mixin (Kernel, Blackboard, Config, Dispatch, Ui)
├── Fleet.Flight/              # MDK2 mixin (Thrust allocator, Helm, Docking, Gyro)
├── Fleet.Tests/               # NUnit test fixtures (498 passing unit tests)
├── deploy/                    # In-game build specs & script snapshot (formerly docs/build-specs/)
│   ├── mining-drone.md
│   └── mining-drone.script.cs
└── planning/                  # Task cards and design specifications (formerly docs/superpowers/)
    ├── plans/                 # Slices 1–3 implementation plans & cards (C01–C32)
    └── specs/                 # Slice 1–3 design specifications
```

---

## 2. Integration into Active Project

Per `CLAUDE(RIG).md` §8 and Decisions 12–14:
- The deep-space fleet logic has been incorporated into the unified Terrestrial Fleet architecture.
- Active C# 6 zero-GC MDK2 scripts for all active craft are located in [`scripts/`](../../scripts/).
- Master IGC protocol definitions are formalized in [`docs/protocol/igc.md`](../../docs/protocol/igc.md).
