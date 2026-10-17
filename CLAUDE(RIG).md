# CLAUDE(RIG).md · Floating Rig, RIG//OS and drone fleet

Context and working rules for Claude Code on Blae's Space Engineers 1 project. The design was developed in claude.ai on 2026-10-15 and 2026-10-16; this file and the `reference/` folder are the handoff snapshot.

> Setup: put this file at the repo root and either rename it `CLAUDE.md` or add a line `See CLAUDE(RIG).md` to the existing `CLAUDE.md`. Put `reference/` next to it.

---

## 1. Read first

| File | What it is | Status |
| --- | --- | --- |
| `reference/floating-rig-build-guide.md` | Markdown export of the illustrated build guide (rev 87). 27 drawings are replaced by `[embedded content: …]` placeholders; the captions say what each one showed. | **Canonical spec** |
| `reference/rig-sandbox/rig-sandbox.html` | Single-file Three.js 3D planner (hex, box and wedge shapes, yaw, overlap check). It reads and writes `modules` and `meta/settings` through `window.claude.use('db')`, and falls back to local state outside claude.ai. | Working tool |
| `reference/rig-sandbox/db/modules/*.json` | Current module records: hub plus 6 pylons. | Data snapshot |
| `reference/rig-os/Main.dc.html` | RIG//OS bridge console prototype: interactive ASCII LCD mock-up. | UI prototype |
| `reference/rig-os/FleetOps.dc.html` | RIG//OS fleet-ops wall prototype: remote drone monitoring. | UI prototype |

Live originals on claude.ai (Blae can open them; you cannot):

- Build guide: https://claude.ai/code/artifact/a5f1ebc5-c6a9-4e2d-90e5-e0844d6e4bbf
- RIG//OS canvas: https://claude.ai/artifact/KJT9DLwUtwVdigk6QFi9Tp
- Rig Sandbox: https://claude.ai/artifact/VjLJDoaBSC9W7dwmsCbard
- Crawler and Mule guides (terrestrial mining subsystem, §7): https://claude.ai/code/artifact/54c534f0-673e-4612-bf9a-3c2d6aeb22fb · https://claude.ai/code/artifact/e1198451-2950-4fc2-b2b3-ea1a439ee3af

If a spec here conflicts with the build guide, the guide wins. Flag the conflict; do not silently pick one.

---

## 2. Working with Blae

- Very visual. Explain with plans, sections, tables and ASCII or SVG diagrams wherever you can; avoid walls of prose.
- Deliverables and docs: educated, specific, parsimonious bullets organised for flow. **No em-dash-laced, AI-sounding narrative.** Use the guide's terse spec-sheet register (`≈`, `→`, `·`, abbreviations from §4).
- Mark unverified game numbers with `†` and add them to a Verify list. Never present a guessed SE value as fact.
- Ask before decisions that are hard to reverse or that change the design intent (geometry, block policy, section boundaries, mods). Make routine engineering choices yourself and say which ones you made.
- No more concept art. Effort goes into the design document, specs, scripts and tools.

---

## 3. Hard constraints

| # | Constraint |
| --- | --- |
| C1 | **Space Engineers 1** (not SE2), on PC via Linux/Proton. All DLC owned. |
| C2 | **Rig and its drones use vanilla + DLC blocks only.** Mods that add no blocks (planets, universe, QoL) are fine. *Amended 2026-10-16 (Blae):* one exception, **Defense Shields** on the Rig. Universe = **Real Solar Systems (moving planets) + Real Stars + Real Orbits** + planet packs: 3–4 large planets with 1–10 moons each. Moving planets void the static 1 g / world-GPS assumptions†. QoL rule: keep a mod only if it does something we cannot feasibly do better ourselves. Full list: `docs/rig/mods.md`. |
| C3 | Scripts are allowed. Blae writes his own drone logic (PAM-like miner, with Claude's help). *Amended 2026-10-16 (Blae):* **all PB scripts are custom**, including `[MFG] PB Inventory` and the dashboards; Isy's Inventory Manager and Automatic LCDs 2 are dropped. Third-party scripts are fine as reference material only. |
| C4 | **Native blocks are the safety layer; PBs make decisions.** No safety function may depend on a PB (rules R12, S1). |
| C5 | Everything on **one grid**, with section seams joined by merge blocks + large connectors (R9, R14). No rotors or pistons in the structure. |
| C6 | Practical industrial look. Every block carries load, moves material, makes power or protects (R1). Mass is cost (R2). |
| C7 | Test every section in creative before survival (R13). |

PB code must fit the SE1 Programmable Block sandbox:
- Whitelisted .NET API, C# 6-era syntax (MDK2 enforces this).
- Instruction-count limit; no file I/O, threads or reflection.
- Config in Custom Data (`MyIni`), cache blocks at init, default `Update100`, IGC for inter-grid messages.

---

## 4. Abbreviations (from the guide key)

| Group | Codes |
| --- | --- |
| Structure | `HUB` hex core · `P` pylon (P-E, P-NE, P-NW, P-W, P-SW, P-SE) · `T` traffic sector · `H1`–`H6` hub decks · `S0`–`S9` build sections · `HGR` hangar |
| Lift and power | `A` large atmo thruster · `H` large H2 thruster · `R` large reactor · `BAT` battery (`WB` working bank, `RB` reserve bank) · `H2E` hydrogen engine · `GEN` O2/H2 generator · `SPK` stockpile (tank) |
| Joining and build | `MRG` merge block · `CON` connector (`L CON` large) · `PRJ` projector · `WLD` welder · `RC` remote control |
| Logic | `EC` event controller · `TMR` timer · `PB` programmable block · `AR` action relay · `BC` broadcast controller · `SNS` sensor · `SRT` sorter · `ASM` assembler · `MC` main cockpit · `EMO` emotion controller |
| Modes | `NRM` · `CAU` · `SHD` shed · `EMG` · `CBT` combat · `REP` repair · `REC` recover; `F1`–`F3` failsafes on H5 (never disabled) |
| Bay types | `MNR` miner · `ICE` ice miner · `RLY` relay · `REP` repair · `FAB` fabricator · `MFG` manufacturing · `INT` interceptor · `PKT` picket |
| Gauges | `T%` thrust override/effort · `CF%` cargo filled · `B` large block (2.5 m) |

Naming: `[ID] Fn n`, e.g. `[P-NE] A 3`. Groups per ID: `Atmo Lift` · `H2 Lift` · `Reserve Tanks` · `Lateral` · `Ops` · `Shed` · `Repair`. Rig-wide groups: `All H2 Lift`, `All Reserve Tanks`, `All Shed`, `[HGR] Doors`, `[HGR] Connectors`.

---

## 5. The Rig (summary of the guide)

### Concept
- A floating factory city and drone mothership that hovers high in a planet's atmosphere on real thrust. Planet assumed Earthlike at 1 g; example mass 20 kt.
- It services two fleets:
  - **Planetary** miners working the surface below.
  - **Orbital** ice and uranium miners in low orbit.
- Built as **10 sections**, each designed and built on its own and releasable at any time.

### Geometry
```
            T-N
     P-NW         P-NE          HUB: hex 16 (E–W, flat) × 17 (N–S, vertex) B, 28 B tall
 T-WNW    ┌──────┐    T-ENE     vertices (8,0)(16,4)(16,13)(8,17)(0,13)(0,4), 1×2 slope faces
  P-W ────┤ HUB  ├──── P-E      6 corner spines (heavy armour), split per section
 T-WSW    └──────┘    T-ESE     6 P, one per hex face, rooted at H5
     P-SW         P-SE          6 T sectors = empty air between arms, centred on hub corners
            T-S
```
- Diagonal P run stepped 1:2 on the hub grid, clad in 1×2 slopes; bays stay square and staggered. T-N and T-S are ≈53°, the others ≈63°.
- Sandbox coordinates: x = east, z = south, y = up, in blocks; hub at origin. P-E/P-W at x = ±19; diagonal P at x ±8.92, z ±16.34, yaw 63.435 / 116.565 / 243.435 / 296.565°.

### Hub decks (top → bottom)
| Deck | Role | Notes |
| --- | --- | --- |
| H1 Command | Pressurised, 4 B | MC, comms, panels; armoured logic room (31 EC, 14 TMR, 4 PB, 17 spare slots; slot order = ladder rung order) |
| H2 Habitat | Pressurised, 4 B | Medical, Survival Kit, quarters, galley, farms; algae on H1 roof |
| H3 Gas & utility | 5 B | Ice → GEN → H2/O2 tank farm; core shop (ASM 1 + refinery 1); core PRJ (Keep Projection, OFF until REP) |
| H4 Power | Armoured base, 5 B | R × 3 in vault, WB 8, RB 6, H2E 4, WLD ring |
| H5 Junction | Armoured base | 6 P sockets (flat MRG plate + 2 L CON each), gyros, reserve SPK |
| H6 Lift | Armoured base | 16 slots, 3 × 3 B faces: 10 A + 6 H |

### Sections
| Section | Content | How it joins |
| --- | --- | --- |
| S0 | HUB base H4–H6 | Flies first; seed kit = H6 lift, RC + antenna on H5, gyros, BAT |
| S1–S6 | Pylons | Built on a ground pad, fly up on own lift, dock at H5 sockets in opposite pairs: E + W → NE + SW → NW + SE. Diagonal sockets face N/S; last 3 B of approach along N/S |
| S7–S9 | Decks H3, H2, H1 | Stacked on the base by PRJ from their own blueprints, MRG seam below; no lift of their own |

- R8: every flown section carries a seed kit (own lift, RC, gyros, BAT, antenna).
- R9: every seam is MRG + L CON, with no welds across it.
- Conveyor trunk A runs in the WNW spine and trunk B in the ESE spine. Both join across seams by L CON, and either one alone feeds every deck (R10).

### Lift and power
| Item | Value |
| --- | --- |
| Totals | 52 A + 30 H (HUB 10 A + 6 H; each P 7 A + 4 H) |
| Large atmo thruster | 6.48 MN, 16.8 MW, 3×3×5 B |
| Large H2 thruster | ≈7.2 MN† |
| Hover (20 kt) | 196 MN → A at ≈58% T% (NRM band); HUB ≈19% of A lift, each P ≈13% |
| Blackout | H alone hovers at ≈91% (R5); H OFF until EMG |
| Pylons lost | Any 2 → hover on A at ≈80%; any 3 → A + H (R6) |
| Power | Hover ≈510 MW; full A ≈874 MW ≤ R × 3 = 900 MW; N−1 = 600 MW ≥ hover; U ≈510 kg/h at hover |

### Pylon template (P × 6)
- **Arm:** W = bay depth = drone L + 4 B, max 9 B, because wider arms collide at the root.
  - H = 1 B deck + bay H + 2 B keel.
  - Example 8 W × 7 H × 22 L B: 2 B collar + 3 bays × 5 + 7 B cross-head.
  - Drone length ≤ 5 B.
- **Bays:** full arm width, doors on alternating side walls into both flanking T sectors. Nothing opens downward (exhaust).
- **Keel:** 2 conveyor lines and heavy H2 tanks (SPK).
- **Cross-head:** plus-shaped tip.
  - Lateral A out + both sides, H out.
  - 2 INT bays with roof hatches.
  - Turrets top and bottom, CON pad.
- **Build method:** P-E, P-NE and P-NW are hand-built; the opposite three are made by PRJ rotated 180°. A PRJ cannot mirror.

### Traffic sectors and hangars
- T sectors are exhaust-free vertical shafts to the surface and to orbit (R11). No structure in them, ever.
- Each T serves the bay doors of its two flanking arms: 1 descent lane, 1 climb lane, and a Dispatcher queue per T.
- Proposed bay board (not final):

| Arm | Bays |
| --- | --- |
| P-E | 3 MNR |
| P-W | 3 MNR |
| P-NE | 2 ICE + 1 RLY |
| P-SW | 2 REP + 1 FAB |
| P-NW, P-SE | MFG (proposed home for manufacturing) |
| Every tip | INT × 2 (12 total) |
| HUB roof | PKT × 2 |

- Standard bay = drone bounding box + 1 B all round + 2 B ahead of the CON. WLD + SRT always ON. Local logic: rung HG1, one SNS per bay, watchdog opens `[HGR] Doors`.

### Control and automation
- **Mode TMRs** NRM, CAU, SHD, EMG, CBT, REP, REC set states only. REC goes through a TMR delay.
- **Wiring rules:**
  - W1: one event per EC; pair ECs for ↑/↓ thresholds.
  - W2: Set actions only.
  - W3: more than one action → TMR Trigger Now.
  - W4: ∧ all, ∨ any.
  - W5: an EC sees its own and merged/connected grids; AR for the rest.
  - W6: names `[CODE] EC sig`, `[CODE] TMR resp`.
- **EC ladder:** 33 rungs in 8 groups, including quota cell M1–M2, drone cycle D1–D4 with native fallback, and a combat chain.
- **Rig PBs:**

| PB | Role | Rate | Acts via |
| --- | --- | --- | --- |
| `[CMD] PB Rig OS` | lift margin, modes, anchor hold, heartbeat | U10 | mode TMRs |
| `[MFG] PB Inventory` | quotas, sorting, ASM queues | U100 | ASM, SRT |
| `[LOG] PB Dispatcher` | jobs, bays, launch/land queue per T | msg + U100 | IGC, doors, CON |
| `[CMD] PB Displays` | dashboards (RIG//OS screens) | U100 | LCD |

- **Script rules S1–S7:**
  - S1: native safety never depends on a PB.
  - S2: watchdog TMR 30 s; Rig OS does Stop → Start on each run; if it hits 0 → BC + fallback ECs ON.
  - S3: fallback ECs OFF while PBs are healthy.
  - S4: act through TMRs and groups.
  - S5: config in Custom Data.
  - S6: cache blocks; U100 by default.
  - S7: one rack slot per PB.
- **Drone PBs:**
  - Miner (PAM-like); ice (miner variant).
  - Interceptor (script or AI Offensive); picket (no PB: AI Defensive + AR).
  - Repair (later).
  - All keep D1–D4 + AR RECALL as a native fallback.

---

## 6. RIG//OS

- **What it is:** a gritty, sci-fi, ASCII-styled LCD operating system for the Rig. It is navigable and configurable from button panels or the cockpit toolbar, and rendered by `[CMD] PB Displays` (and later per-section displays).
- **Main bridge console prototype** (`Main.dc.html`): home, lift by section, lift margin, mode ladder, power (WB/RB, U stock), gas/LS, quotas, storage, bays and dock traffic, Dispatcher queue, jobs, active alerts, failsafe rack, PB modules, linked rules, link tree, comms, config (UI, alarm sound, fleet-on-link-lost).
- **Fleet-ops wall prototype** (`FleetOps.dc.html`): remote monitoring of drones far from the Rig.
  - Tabs: ROSTER · SECTOR · COMMS · JOBS.
  - Planetary miners `PM-01…`, orbital miners `OM-01…`, relays `RLY-1/2`.
  - Zones `SURF S-07`, `ORB ICE-A`, `ORB U-C`; altitude bands SURFACE / ATMO EDGE / LOW ORBIT.
  - Drone states PROSPECT, TRANSIT, MINING, ASCENT, DOCKED; job IDs `J-0139…`; STALE flag when a drone misses its status beat.
- **Prototype format:** `.dc.html` design-canvas boards (HTML with `sc-if`/`sc-for` template attributes and a small logic script). They specify behaviour and layout. They are **not** game code.
- **Known stale items** (prototype predates the hex redesign):
  - It shows 4 pylons `P-N/P-E/P-S/P-W`; update it to the 6 hex pylons, the T sectors and the bay board.
  - Blae will refine the layout after he designs the Central Command interior (H1). Do not lock the H1 screen grid before then.
- **Still to define:**
  - The drone status message (IGC payload and beat interval) that feeds ROSTER and STALE.
  - The relay chain: antenna ranges† from high atmosphere down to the surface and up to low orbit.

---

## 7. Sister subsystem: Terrestrial Mining Subsystem (TMS)

Designed in a separate claude.ai chat. Summary only; ask Blae to export the Crawler and Mule guides to Markdown into `reference/` before you work on TMS.

- **Units:**

| Code | Unit |
| --- | --- |
| CR | Crawler: wheeled mining vehicle with a pan-tilt laser-drill head that rasters an inverted-cone pit |
| MU | Mule: kneeling straddle carrier that moves pods |
| BP | Bulk Pod: Economy 2 Bulk Cargo Container, 6×3×3 B, 820,000 L |
| GD | Ground depot (silo/refinery) |
| — | Lifter drone that carries pods up to the Rig |

- **Mod exception:** TMS uses mods (Seismic Surveying + Definition Extension API + seismic landing gear, Simple Laser Multitools + ToolCore). That makes it an exception to C2, confined to TMS vehicles. *Confirmed 2026-10-16:* laser head = Simple Laser Multitools (not Adjustable Mining Laser); drivetrain = vanilla wheels (no Tank Tracks).
  - TMS vehicles never dock into Rig bays.
  - They hand off at the pod: the Rig side touches only BP, a vanilla block.
  - Radio Spectrometry reads asteroids only, so it belongs to the orbital ops.
- **IGC namespace:** `TMS.POD`, `TMS.ACK`, `TMS.SWAP`, `TMS.STAT`. Payloads are `;`-separated; `TMS.STAT` is broadcast every 10 s.
- **Site handoff:** a GPS pasted into Crawler PB Custom Data `[site]`. A future scripted survey writes the same block.

---

## 8. Task: merge the deep-space drone fleet into this project

Blae's earlier SE1 work designed a **deep-space drone fleet**. That work is in this repo or will be added to it. Fold it into the Rig + RIG//OS project as the fleet the Rig hosts and commands.

### What is known about the earlier fleet (from Blae's notes; confirm against the files)
- **Chain:** survey probe finds ore → logistics vessel (carrier) brings mining drones and collects their cargo → cargo drones ferry loads to the mothership refineries.
- Asteroid mining was the backbone; no salvage gameplay.
- Custom PB scripts throughout. PAM was ruled out ("we can do better"), though known-working scripts may be studied.
- **Combat arm:** carriers loaded with combat drones that swarm enemies.
- **Identity:** the fleet is an esoteric ancient intelligence with an industrial focus.
  - Aesthetic references: Stargate SG-1 Replicators and EVE Minmatar.
  - The mothership "Mother-Forge" was styled like an EVE Amarr titan (cathedral silhouette).
- **Mods:** earlier mod lists included block mods (WeaponCore packs, Energy Shields, Automated Beam Drill, and others). These conflict with C2 for anything that docks with the Rig.

### Phase 1: Inventory (read-only; write `docs/merge/01-inventory.md`)
- List every script, doc, blueprint note and data file from the fleet work.
- For each drone or ship class, record:
  - Role, grid size, dimensions in B, thrust types and environment (space-only ion vs atmo/H2).
  - Cargo, power, docking method, PB(s), IGC tags and payloads, Custom Data schema.
  - Every modded block it uses.
- Record shared code: navigation, docking, IGC, config parsing, state machines, LCD output. Note quality, PB budget use, and any third-party code copied in.

### Phase 2: Map and find conflicts (write `docs/merge/02-mapping.md`)
Start from this draft mapping and correct it against the inventory:

| Fleet element | Rig home | Notes |
| --- | --- | --- |
| Mother-Forge (mothership) | **The Rig** | Rig replaces it as command, refining and manufacturing hub. Keep the lore and naming flavour if Blae wants (ask). Amarr-cathedral styling vs the Rig's practical industrial rule (C6): ask. |
| Asteroid / space miners | Orbital ops: `OM-*`, ICE bays on P-NE | Low-orbit ice and U. Space-only thrust cannot enter the Rig's atmosphere; they need atmo/H2 capability or an orbital handoff. |
| Survey probe | RLY / survey role; FleetOps `PROSPECT` state | Orbital survey may use Radio Spectrometry (adds no blocks?† confirm). |
| Logistics vessel / carrier | **Orbital tender** working between low orbit and the Rig through a T sector | Open: does a tender exist, or do miners fly home? Must fit the T-sector lanes. |
| Cargo drones | Dispatcher jobs; land in MNR/ICE bays or tip CON pads | ≤ 5 B long to fit bays. |
| Combat carriers + swarm drones | INT bays (12, cross-heads) and PKT (HUB roof); TMR CBT combat chain | Swarm size limited by INT bay count unless carriers are added; ask. |
| Fleet scripts | Drone PBs + `[LOG] PB Dispatcher` + RIG//OS FleetOps | Keep native fallback D1–D4 + AR RECALL on every drone (S1). |

Flag every conflict with a proposed resolution. At minimum check:
1. Modded blocks on anything that docks with the Rig (C2).
2. Drone size vs the 5 B / 9 B bay limit.
3. Space-only thrusters vs atmospheric operation.
4. IGC tag collisions and inconsistent payload formats.
5. Naming vs `[ID] Fn n`.
6. PBs doing safety work (violates S1).
7. Scripts duplicating what native blocks (EC, AI blocks, BC) already do.
8. Third-party code vs C3.

### Phase 3: Decision brief (stop here and wait)
- Write `docs/merge/03-decisions.md`: a short table of open decisions, each with options and a recommendation.
- Present it to Blae. **Do not restructure or rewrite code until he answers.**

### Phase 4: Integrate (after approval)
- **Protocol:** write `docs/protocol/igc.md`, one IGC spec for the whole project.
  - Namespaced tags: proposed `RIG.*` (Rig and Dispatcher), `FLT.*` (fleet drones), existing `TMS.*`.
  - `;`-separated payloads, versioned, consistent with TMS.
  - It must cover the drone status beat that feeds RIG//OS ROSTER/STALE, and launch/recovery by bay and T-sector lane.
- **Shared library:** Custom Data config (`MyIni`), block cache, IGC send/receive with ack/retry, heartbeat/watchdog, state-machine helper, LCD text renderer for RIG//OS.
- **Port the fleet scripts** onto the library, one PB project per drone class. Keep behaviour; change only what the decisions require.
- **Update RIG//OS** to the 6 pylons and the merged drone classes. Implement the screens as `[CMD] PB Displays` output on real LCD surfaces (monospace text, optional sprites); keep the `.dc.html` files as the visual spec.
- **Keep the old work:** do not delete it. Move it under `legacy/drone-fleet/` and let git history carry the rest.
- **Log every spec change** in `docs/rig/CHANGELOG.md` in the guide's Log format (`| date | change |`) so Blae can sync it back to the claude.ai guide.

---

## 9. Suggested repo layout (adjust to what exists)

```
CLAUDE.md                 # or CLAUDE(RIG).md
reference/                # snapshot from claude.ai (read-only)
docs/
  rig/                    # spec notes, CHANGELOG.md, verify list
  protocol/igc.md
  merge/01-inventory.md 02-mapping.md 03-decisions.md
scripts/                  # MDK2 PB projects (C#)
  Shared/                 # shared library (MDK2 mixin/shared project)
  Rig.OS/  Rig.Dispatcher/  Rig.Inventory/  Rig.Displays/
  Drone.Miner/  Drone.Ice/  Drone.Interceptor/  Drone.Tender/  ...
tools/
  rig-sandbox/            # sandbox HTML (+ local JSON load/save outside claude.ai)
  rig-os-proto/           # .dc.html prototypes
legacy/drone-fleet/
```

- Tooling: MDK2 (Malware's Development Kit for SE PB scripts; .NET SDK, `dotnet new` templates) for VS Code. Confirm the current install steps on its GitHub page before setup†.
- Each PB project has a header comment with: block name, grid, required blocks and groups, Custom Data schema, arguments, IGC tags, and its update rate.

---

## 10. Open decisions (do not assume)
- Planet and altitude band; survival vs creative; target Rig mass (example 20 kt).
- Drone classes and sizes → bay size → arm section and P length.
- MFG home: P-NW + P-SE arms (proposed) or HUB.
- Bay allocation (the board in §5 is a proposal).
- Deck sections: stacked by PRJ (current) or delivered/swapped by tug; swap procedure.
- H5 and H6 deck chapters not yet written; mass-reduction pass pending.
- Drone status message format; relay chain.
- Fleet merge decisions from Phase 3.

## 11. Verify in game (†)
- Thrust acts through CoM (1 P OFF, no roll).
- A severed P keeps its dampeners.
- H2 drain time at full H; large H thrust value.
- Damper drift vs drone routes.
- Consumers pull through SRT.
- MRG pull-in tolerance for P docking.
- Antenna/IGC range for the relay chain.
