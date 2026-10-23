# Ideas backlog

One line per idea. Anyone (Blae, Claude, the local model) may append here. Nothing on this list is
scheduled until Claude turns it into a spec section or a task card.

- Per-job mining stats on the drone LCD: holes done, trips, ore kg delivered, kg/h. Scheduled: C31/C32.
- Ore-type priorities when unloading (e.g. keep ice, eject stone): slice 4 adjacent ("smarter mining").
- Connector queuing and holding points when several drones share one carrier: slice 3.
- Crawler: Unshelved 2026-10-17 as MASTODON-01 (10-wheeled twin laser tusk excavator) + CATERPILLAR-01 (articulated bulk land train). Designed in docs/rig/ (Ch 10 & 11).
- Approved chapter-drafting brief: bring only Ch 3 MULE-01, Ch 4 Ground Base/refinery, and Ch 11 CATERPILLAR-01 to Ch 10's Studio standard. For every manual build step, author a matching Studio step with explicit document-grounded voxel placements; replace the current five-placeholder placements per step, reconcile the manual and model dimensions, bounds, registry, Master Build Manual, and manuals index. The detailed manuals are design intent; where their coordinate ranges conflict with stated envelopes, the envelope dimensions win. Align the MULE/base pad and ore-transfer path, Caterpillar/Mastodon loading and base dump/collector path, and the base's refinery/manufacturing flow. Keep all figures and placements unverified until checked against an in-game build or trusted blueprint; mark unsupported orientations/locations for review rather than guessing. Leave Ch 10 unchanged and exclude the other chapters, shipyard, Floating Rig, and RIG//OS. Validate with `python3 studio/chapter_indexer.py --all` and existing Studio tests.
