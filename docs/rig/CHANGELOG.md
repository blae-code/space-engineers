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
| 2026-10-16 | Precedence: Blae's decisions are canon; guides are guides (decisions > CLAUDE(RIG).md > guides) |
| 2026-10-16 | Crawler concept (Blae): wide, low hull; corner stabilizers lift and level it over the deposit; two articulated, extendable arms (a laser arm reaching most angles that extracts the whole deposit systematically, and a crane arm that loads and unloads containers on Mules); directed manually, remotely or automatically; many Crawlers and Mules concurrently; a future prospector probe marks deposits and the base prioritises by supply and demand. Supersedes the guide's kneel, pan-tilt head and drive-over pod pickup |
| 2026-10-16 | Crawler (Blae): **crane dropped**. Cargo stays aboard in whatever containers fit the Crawler's design best. Second arm = an **articulated "hose"**: an arm whose tip connects to a Mule's cargo grid and fills it, so loading and unloading work on uneven, difficult terrain |
| 2026-10-16 | Mule (Blae): **medium wheeled freight drones** that dock with the Crawler's hose, take raw ore to the right refinery terminal, and come back. Many per Crawler. Supersedes the drive-over Bulk Pod concept |
| 2026-10-16 | Crawler operating radius (Blae): 10–15 km around the refinery. Long-range miners are a later, separate design |
| 2026-10-16 | Concept change (Blae): **flying drones + carriers**. **Orca** = large, pure-atmo mining carrier and hub for 4–6 vertical mining drones (XU-1 style) that mine a deposit in concert and redock to offload. **Mule** = cargo carrier that docks with the Orca and ferries ore to the refinery. Swap-body containers dropped: every craft has built-in cargo and moves ore through docked connectors. Crawler status open |
| 2026-10-16 | Orca chassis (Blae): **A · Flatbed, 2 × 3 bays**: central spine (hold + power), 3 downward-opening drone bays each side, lift thrusters on corner outriggers |
| 2026-10-16 | Orca materials (Blae): **drone bays are open truss frames** (weight first). Guidance: Beam Blocks for the skeleton, Structural Frame 3×1×3 for the roof deck, Diagonal Beams for outrigger bracing, heavy armour only at the bridge and power core. SE has no structural load physics, so mass is the only cost of a frame |
