# Repository Archive & Historical Catalog

Authoritative index and catalog of all archived, superseded, non-canon, and deprecated material preserved in the repository.

Per project rules and Blae's explicit directive (2026-10-19), **no material is discarded or deleted**; all historical iterations, earlier design snapshots, and frozen codebases are preserved here with full context and git traceability.

---

## 1. Archive Directory Taxonomy

| Directory | Contents & Historical Context | Precedence / Status |
|---|---|---|
| **[`reference-guides/`](reference-guides/)** | Early illustrated build guides, prototypes, and sandboxes exported from claude.ai (rev 87). | **Superseded & Non-Canon** (Blae decisions in `docs/CHANGELOG.md` override all reference guides). |
| **[`legacy-drone-fleet/`](legacy-drone-fleet/)** | The deep-space asteroid mining fleet (`Fleet.*`, PAM-successor miner, carrier, console) frozen at tag `fleet-freeze` (2026-10-16), along with slices 1–3 planning cards (`planning/`) and build specs (`deploy/`). | **Frozen & Legacy** (Superseded by Terrestrial Fleet and Rig ecosystem in `scripts/` and `manuals/`). |
| **[`merge-audit/`](merge-audit/)** | Historical audit records (`01-inventory.md`, `02-mapping.md`, `03-decisions.md`) generated during the 2026-10-16 merge of the deep-space fleet into the Rig architecture. | **Historical Audit** (Completed 2026-10-16). |
| **[`prototypes/`](prototypes/)** | Deprecated standalone single-file HTML mockups (e.g. early Mastodon interactive guide), superseded by the modular PWA in `studio/`. | **Deprecated Prototypes**. |
| **[`backups/`](backups/)** | Empty folder backups (`Mastodon-Build-Guide/`) and hook backup files (`commit-msg.bak-*`) retained per Rule 3. | **Preserved Backups**. |

---

## 2. Technical Precedence Rule

Per `CLAUDE(RIG).md` §1 and `docs/CHANGELOG.md`:
> **Decisions are canon; reference guides are guides only.**

Where an archived guide, legacy card, or early prototype departs from a logged decision in `docs/CHANGELOG.md`, the decision is absolute authority. Archived materials must never be used to argue against active canon.
