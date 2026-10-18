# Review queue

Gemini appends a row per finished task (`pending`). Claude reviews and sets the status.

- `reviewed` · accepted as is. A card deck may now be executed.
- `fixed` · Claude fixed it (≤ ≈10 lines); note says what.
- `bounced` · back to Gemini; note gives the reason and, for code, the failing test.

| date | agent | commit(s) | what | status | note |
| --- | --- | --- | --- | --- | --- |
| 2026-10-17 | Gemini | d547b66 | repo cleanup: rm retired `RIG/`, add `README.md`, gitignore `.claude/`, delete local `.bak-*` + `__pycache__` | pending | docs-only, no code changes. Phase 4 renames (`superpowers/`→`planning/`, `build-specs/`→`deploy/`, `merge/`→`rig/merge/`) deferred — would require edits to `CLAUDE.md`, `CLAUDE(RIG).md` and `tools/` which Gemini can't touch. |
| 2026-10-17 | Gemini | e6d7324 | docs: Terrestrial Fleet & Base specs (lego-instructions-v1.0.md Ch 1-4, 10-13) + cleanup | pending | Complete visual Lego manuals for ORCA-01, REMORA-01, MULE-01, GROUND BASE & C2, MASTODON-01, CATERPILLAR-01, KESTREL-01, ALBATROSS-01. All 19 TOC anchors verified; zero em/en-dashes; Decisions 1-54 logged; removed redundant Category A early drafts and root .bak file. |
| 2026-10-17 | Gemini | 43960f5 | docs: Rig Linear Extruder Ring shipyard overhaul (Ch 5) & master manual update | pending | Second-pass human comprehension overhaul for Chapter 5 (docs/rig/extruder-ring-instructions.md & lego-instructions-v1.0.md): 10-slot SE paint palette, spatial locator HUDs, pre-enclosure inspection gates (5.1A-5.5A), phase-end visual milestones (125 m launch way, 22x welder U-ring arch, 6-stage pusher spine), pylon flight commissioning sequence, and zero-GC AI scripting contract. Master manual re-compiled (6,453 lines) with zero em/en-dashes. |
