# Merge 03 · Decision brief

2026-10-16 · CLAUDE(RIG).md §8 phase 3. **No code moves until Blae answers the open items.**

## Decided (Blae, 2026-10-16)

| # | Decision |
| --- | --- |
| 1 | C31 MiningStats fixed + committed (`2eea7b8`) |
| 2 | `Rig.*` placeholders deleted |
| 3 | All PB scripts custom (C3 amended) |
| 4 | Code moves by `git mv` into the §9 `scripts/` layout; old slice docs → `legacy/drone-fleet/` |
| 5 | TMS: Simple Laser Multitools, vanilla wheels |
| 6 | Rig blocks vanilla + DLC, except Defense Shields |
| 7 | NPCs: MES + vanilla-block packs (15 scanned clean) |
| 8 | Universe: Real Solar Systems (moving) + Real Stars + Real Orbits; 3–4 large planets, 1–10 moons each |
| 9 | Mechanics: Automatic Ore Pickup only |
| 10 | QoL: only what we can't feasibly do better ourselves |

## Open

| # | Decision | Options | Recommendation |
| --- | --- | --- | --- |
| D1 | IGC payload separator | **a** `;` everywhere (matches TMS) · **b** keep `|` for FLT/RIG, `;` for TMS | **a**: one style project-wide. It's a codec constant plus test fixtures, done once, before any v2 line is live in game |
| D2 | Tag scheme | `FLT.STATUS`, `FLT.CMD`, `RIG.BAY`, `RIG.DOCK`, `RIG.JOB`, `RIG.LEASE`… · channel in the tag (`FLT.<ch>.STATUS`) or in the payload | Channel **in the tag**: it filters before parsing, and two Rigs never cross |
| D3 | Lore: Mother-Forge / ancient-intelligence flavour | keep as names + RIG//OS boot text · drop | Keep as **flavour only** (boot screen, drone callsigns); guide names stay canonical |
| D4 | Amarr-cathedral styling vs C6 | C6 wins · styled shell | **C6 wins**. Practical industrial |
| D5 | Orbital tender | none: OM-\* fly home through T sectors · tender between orbit and Rig | **None for v1**. OM-\* are H2 or atmo+ion and dock in ICE bays |
| D6 | Drone native fallback D1–D4 | Blae exports the guide drawing · Claude proposes rungs | Claude proposes: D1 BAT ≤ x → AR RECALL · D2 H2 ≤ x → RECALL · D3 damage → RECALL · D4 PB heartbeat lost → RECALL + beacon. Blae approves |
| D7 | Classes in scope now | miner (PM/OM) only · + survey · + interceptor · + cargo | **Miner only** for the first Rig slice; survey and interceptor next; cargo dropped (miners haul their own ore) |
| D8 | Swarm size | 12 INT + 2 PKT · add combat carriers | **12 INT + 2 PKT** v1 (bay-limited, AI blocks) |
| D9 | Order of work after the move | protocol → nav (planet-relative) → Rig.Dispatcher → Rig.OS → Displays | As listed. Nav depends on the RSS verify run in creative |
