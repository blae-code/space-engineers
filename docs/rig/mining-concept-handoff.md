# Handoff · terrestrial mining concept (state at 2026-10-16)

For an agent that knows the RIG project (`CLAUDE(RIG).md`, the hex Rig, the original TMS Crawler and Mule guides in `reference/`) but not today's changes. † = unverified game value.

## 1 · Ground rules that changed today
- **Precedence: Blae's decisions are canon** > `CLAUDE(RIG).md` > the guides (`CLAUDE(RIG).md` §1). Every decision is logged in `docs/rig/CHANGELOG.md`. Never argue a decision back toward a guide; raise engineering limits as facts to design around.
- **C2 amended:** Defense Shields is the only Rig block exception. **C3 amended:** all PB scripts are custom (no Isy's, no Automatic LCDs 2, no PAM).
- **Scope: terrestrial operations first.** Order: resource extraction → refinement → materials production. Orbital, survey and combat work is deferred.
- **Status: design phase (brainstorming), not approved.** No product code has been written for the new concept.

## 2 · What we're building (replaces the TMS Crawler / straddle-Mule / Bulk Pod plan for phase 1)

```
  ORCA (uncrewed carrier, hovers over a deposit)
   ├─ 6 belly bays ──► mining drones ×4–6 drill vertically, in concert, one shared hole grid
   │                    └─ full / done / needs service → rise back into a bay, unload ore via connector
   └─ spine hold ◄── ore
        └─► MULE (cargo carrier, built-in hold) docks, takes ore → refinery terminal → returns
                         [v1 open: Orca may fly its own full hold home; Mules added later]
  REFINERY / FACTORY COMPLEX (stationary ground base; builds the Rig): not designed yet
```

**Success criterion (Blae):** one complete loop running unattended: mine → offload → ferry → refine → return. Creative world first, then survival.

### Orca (mining carrier and swarm hub)
- **Flight:** large grid, pure atmospheric, **uncrewed**: flown remotely or autonomously. No cockpit, no life support.
- **Chassis A · flatbed (chosen):**
  - Core 17 × 19 B (42.5 × 47.5 m), 7 B tall (17.5 m).
  - Central spine 3 B wide holding the ore hold and power, on the centre line.
  - **6 bays**, 3 per side: 5 × 5 B, 6 B tall, **open truss frames**, open below, one connector centred in each ceiling facing down.
  - Mirror-symmetric. Assign bays in opposite pairs to stay level.
- **Lift (first pass†):** 4 large atmo thrusters on corner outriggers, facing down, ≈ 25.9 MN, ≈ 2,000 t total at T/W 1.3. 2 spare mid-length outrigger slots. Exhaust outboard of the drone lanes.
- **Lateral thrust** on all 4 horizontal faces, in symmetric pairs. Landing gear under the outriggers.
- **Materials:** Beam Blocks for the skeleton, Structural Frame 3×1×3 for the roof deck, Diagonal Beams for outrigger bracing, heavy armour only at the control core and power core. SE has no structural load physics: mass is the only cost of a frame.
- **Control core** (armoured, bow): 2 Remote Controls (one spare), PB, antenna, timer/EC rack. Cameras are the cockpit (bow, belly, bay rows, outriggers). **Native link-loss safety:** EC on link or heartbeat loss → hold on dampeners → RC autopilot home (C4).
- **Open:** power source (reactor vs batteries vs H2; depends on the planet's uranium and ice), gyro count, final hold size (lift sets it, not volume).
- Starter-frame drawing: brainstorm companion screen `orca-starter-frame.html`, under `.superpowers/brainstorm/…/content/` (git-ignored).

### Mining drones
- **XU-1-style vertical miners** (reference blueprint Workshop 2705006131): small grid, 5.5 × 5.5 × 11 m, 62.3 t dry, nose-down, 8 drills in a ring, 4 large atmo for lift, docks **upward** via a top connector.
- **Full analysis:** `docs/rig/drones/xu1-analysis.md`. Key adaptations:
  - Replace PAM with our miner script.
  - Return on **lift margin / tonnes**: at 3× inventory a full hold leaves T/W ≈ 1.09.
  - Add a native fallback (EC rungs → RC to the Orca).
  - Decide a stone policy: ejecting under a swarm fouls neighbouring holes.
  - Test that the drone's small-grid connector locks to the bay's ceiling connector (port size†).

### Mule (cargo carrier)
- **Swap-body containers were dropped.** Every craft has built-in cargo, and ore moves through docked connectors.
- **First build, "Mule and Container Sprue 1.0":** reviewed in `docs/rig/drones/mule-sprue-1.0-review.md`. It was built for the dropped container concept, so most of it will be redesigned.
- **Lessons that carry over:**
  - Thrust on all horizontal axes. Sprue 1.0 has all 4 lateral thrusters facing the same way.
  - Lift centred on the payload.
  - Size lift for payload in tonnes.

### Crawler
- **Status open.** Last concept: a wide, low hull with corner stabilizers, an articulated laser arm and an articulated "hose" arm that docks to Mules. The crane was dropped. It worked within a 10–15 km radius.
- Superseded for phase 1 by the flying system unless Blae says otherwise.

## 3 · Code: most of the old Fleet project applies again
- **The Fleet code is frozen** at tag `fleet-freeze` (a7efeb7), plus C31 MiningStats (`2eea7b8`). It is 498 NUnit tests, C# 6 / MDK2.
- **It maps almost 1:1 onto the swarm:**
  - **Slice 1:** a vertical hole-grid miner with cargo, energy and damage returns and a recorded dock path.
  - **Slice 2:** docking to a **moving carrier**. The carrier broadcasts its bay poses, and the drone dead-reckons its home.
  - **Slice 3:** a shared job split by **hole leases**, shared bays with a queue and holding slots, and a `Dispatcher`. The carrier integration (C44) and drone fleet mode (C46) are still unbuilt.
- **Merge workflow:** `docs/merge/01-inventory.md`, `02-mapping.md`, `03-decisions.md`.
- **Answered:** IGC payloads become `;`-separated, with tags `FLT.<ch>.*`, `RIG.<ch>.*` and `TMS.*`.
- **Decided, not yet done:** move the code with `git mv` into the `CLAUDE(RIG).md` §9 `scripts/` layout (Shared, Drone.Miner, Rig.Dispatcher, Rig.Displays).
- **Workflow (repo `CLAUDE.md`):** Claude writes the contracts (specs, cards, tests), and the local model (qwen3-coder-30b) writes the code. Claude may also call Qwen directly at `:1234` for bulk reading when it is resident.

## 4 · World, mods, tools
- **Real Solar Systems, read from its source:** voxel planets are **static inside their zone**; only the scaled proxies move. Zone crossings teleport non-static grids. **Surface operations keep world GPS** (no planet-relative navigation needed). Real Orbits changes gravity falloff to inverse-square†.
- **Final mod list:** `docs/rig/mods.md`. 131 subscriptions were pruned with `tools/steam-unsub.py`.
- **Test world "RIG Test":** creative, 3× inventory, one EarthLike planet, block limits off. Its 13 mods (TMS set + build/HUD tools) were written in by `tools/world-mods.py`.
- **Blueprint review:** `tools/bp-inspect.py "<name>" [--svg out.html]` reads local and Steam Cloud blueprints (gzip). It reports grids, joints, item-flow reach, mass and CoM, power, wheels, cargo, names and modded blocks.

## 5 · Open questions (Blae's to answer)
1. v1 loop: does the Orca fly its own hold home first, with Mules added later?
2. Is the Crawler shelved?
3. Orca power source.
4. Drone count per Orca: 4 or 6 (6 bays drawn).
5. Drone stone policy.
6. Refinery/factory complex design.
7. Final world inventory multiplier and planet.
