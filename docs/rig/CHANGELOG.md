# RIG spec changelog

Format matches the build guide's Log, for syncing back to the claude.ai guide.

| Date | Change |
| --- | --- |
| 2026-10-16 | C2 amended: Defense Shields allowed on the Rig (only Rig block exception) |
| 2026-10-16 | C2 amended: universe = Real Solar Systems (moving planets) + Real Stars + Real Orbits; 3–4 large planets, 1–10 moons each. Static 1 g and world-GPS assumptions now †; nav to become planet-relative |
| 2026-10-16 | C3 amended: all PB scripts custom; Isy's Inventory Manager and Automatic LCDs 2 dropped |
| 2026-10-16 | TMS confirmed: Simple Laser Multitools head, vanilla 5×5 wheels |
| 2026-10-16 | NPCs: MES + only encounter packs whose ships use vanilla + DLC blocks |
| 2026-10-16 | Fleet project frozen (tag fleet-freeze) and merged into the Rig per CLAUDE(RIG).md §8 |
| 2026-10-16 | Scope: terrestrial operations first. Crawler → Mule → ground base (refinery/factory for building the Rig). Orbital, survey and combat classes deferred |
| 2026-10-16 | IGC: `;`-separated payloads project-wide; tags `FLT.<ch>.*`, `RIG.<ch>.*`, `TMS.*` |
| 2026-10-16 | RSS read from source (`ZoneManager.cs`, `GpsManager.cs`): real voxel planets sit **static** at `trueBodyPosition`, each in its own zone; only the scaled proxies move, and the sun turns. Within a surface zone, world GPS is stable → **no planet-relative nav needed for TMS, the ground base or a Rig inside the surface zone**†. Non-static grids crossing zones (surface ↔ orbit ↔ proxy space) are **teleported**; static grids are never moved. Cross-zone nav is deferred with the orbital work |
