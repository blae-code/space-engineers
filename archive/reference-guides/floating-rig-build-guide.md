# Floating Rig Build Guide

Oct 15, 2026 · @Blae

## Key

&#91;embedded content: key sheet · symbols and abbreviations\]

- Naming: `[ID] Fn n`, e.g. `[P-NE] A 3`.
- Groups per ID: `Atmo Lift` · `H2 Lift` · `Reserve Tanks` · `Lateral` · `Ops` · `Shed` · `Repair`.
- Rig-wide groups: `All H2 Lift` · `All Reserve Tanks` · `All Shed` · `[HGR] Doors` · `[HGR] Connectors`.
- Figures ≈, example rig 20 kt @ 1 g. All DLC available.

## Rig overview

&#91;embedded content: rig plan · hex HUB, 6 P, 6 T sectors\]

10 sections, each designed and built on its own: S0 HUB base (H4–H6) flies first → S1–S6 pylons dock in opposite pairs (E + W, NE + SW, NW + SE) → S7–S9 decks H3, H2, H1 stack on the base · every seam MRG + L CON · T sectors stay empty.

## Rules

- **R1** Practical only: each block carries load, moves material, makes power or protects.
- **R2** Mass = cost. Light armour default; heavy on R vault, MC, SPK tanks, P roots, corner spines.
- **R3** Pressurize H1–H2 only.
- **R4** A carries hover @ ≈58% T% (NRM); HUB ≈19% of A lift, each P ≈13%.
- **R5** Blackout: H alone hovers @ ≈91%; H OFF until EMG.
- **R6** Any 2 P lost → hovers on A (≈80%); any 3 lost → A + H.
- **R7** A + H on every lift surface.
- **R8** Every flown section (HUB base, P) carries a seed kit: own lift, RC, gyros, BAT, antenna. Stacked decks carry none.
- **R9** Every section seam = MRG + L CON; no welds across; any section can be released.
- **R10** 2 conveyor paths to every critical system.
- **R11** Exhaust clear of decks, bays and lanes; T sectors = exhaust-free shafts.
- **R12** Native blocks = safety; PB = decisions.
- **R13** Test every section in creative before survival.
- **R14** One grid: diagonal P run stepped 1:2, clad in 1×2 slopes; nothing off-grid.

## Core hub (HUB)

Hex 16 × 17 B (flat E–W, vertex N–S, 1×2 slope faces) · 28 B (70 m) · 6 corner spines H6 → roof · H4–H6 armoured base = S0 (flown) · H3, H2, H1 = stacked sections S7–S9.

&#91;embedded content: HUB elevation · H1-H6, 28 B, not to scale\]

- P sockets @ H5, one per hex face: flat MRG plate + 2 L CON; diagonal sockets face N or S → P and HUB thrusters ≈ one plane.
- R, tanks, gyros low and central (base).
- Spines: heavy armour, H6 → roof, split at every seam; each section carries its own segment. Trunk A in WNW spine, B in ESE spine; joined across seams by L CON; either alone feeds every deck. Other 4: ladders, spare trunks.

&#91;embedded content: H5 junction plan · 6 P roots · H6 lift slots (10 A + 6 H)\]

- HUB lift: 10 A (≈65 MN) + 6 H (≈43 MN†) on 16 slots, 3 × 3 B face; the hex fits 16, not 25.

**Build**

1. Stand or scaffold: H6 clears ground.
2. H6 → H5 (gyros, ring) → H4 (1 R online, RC + antenna on H5 = S0 seed kit).
3. Hover test, base only.
4. Dock P × 6 in opposite pairs: E + W → NE + SW → NW + SE (Pylon).
5. Stack H3 → H2 → H1: each by PRJ from its own blueprint, MRG seam below.
6. Commission → climb to station altitude.

### H1 Command

Pressurized · 4 B · MC, comms, panels, logic racks.

&#91;embedded content: H1 plan · 7 zones, hex 16 × 17 B\]

&#91;embedded content: H1 rack map · 31 EC, 14 TMR, 4 PB, 17 spare\]

- Logic room: heavy-armour box. Slot order = ladder rung order.

&#91;embedded content: H1 panel layout · 2 mode consoles, 9-row section wall\]

- Status: LCD apps (Energy & H2, Horizon, Gravity, Weather†) · mode lamps · EMO · BC.

| Block | n |
| --- | --- |
| MC or Bridge Helm | 1 |
| LCD | 6–8 |
| Light (mode lamp) | 7 |
| Emotion Controller | 1 |
| 4-button panel | 11 |
| BC · AR · antenna · beacon | 1 each |
| EC · TMR · PB | 31 · 14 · 4 |
| RC (backup) | 1 |
| Air vent | 2–3 |

**Build**

1. PRJ above H2; airtight; armour logic room.
2. Racks in slot order; name before wiring.
3. CMD TMRs → ECs in rung order.
4. MC, LCDs, lamps, EMO.
5. Panels.
6. Comms.
7. Pressurize; test every mode button.

### H2 Habitat

Pressurized · 4 B.

&#91;embedded content: H2 plan · 7 zones, hex 16 × 17 B\]

&#91;embedded content: food chain · algae and crops to meals\]

- ☢ Damaged R: r ≈ 60 m through structures → crew > 60 m until repaired.

| Block | n |
| --- | --- |
| Medical room · Survival Kit II | 1 · 1 |
| Bed / cryo | 1 per crew + spare |
| Food Processor · cargo S (food) | 1 · 1 |
| Farm Plot | n |
| Irrigation System | 1 |
| Collector (locked hinge) | 1–2 |
| O2 tank · air vent | 2 · 3–4 |
| Algae Farm (H1 roof) | n |

**Build**

1. PRJ above H3; airtight; airlock ↓ H3.
2. Vents + O2; pressurize.
3. Medical + kit (respawn).
4. Quarters.
5. Galley + 2 EC.
6. Farm bay.
7. Algae on H1 roof.

### H3 Gas & utility

Unpressurized · 5 B.

&#91;embedded content: H3 plan · 6 zones, hex 16 × 17 B\]

&#91;embedded content: gas flow · ice → O2 + H2 → consumers\]

- Shop: ASM 1 + refinery 1 → core self-sufficient before MFG arms.
- Core PRJ: HUB + P blueprint, Keep Projection, OFF until REP. Rebuilds within WLD reach only → WLD arrays on H6 (A/H), H4 (R), H5 (gyros).
- H3 lost → no new gas; H5 + P reserves hold on SPK until EMG.

| Block | n |
| --- | --- |
| GEN | 4+ (until H2 work ≥ 70% in NRM) |
| Cargo L (ice) | 2 |
| H2 tank L (working) · O2 tank | 4 · 2 |
| SRT | ≈ 8 |
| ASM · refinery | 1 · 1 |
| Cargo M (stock) | 3–4 |
| PRJ | 1 |

**Build**

1. PRJ above H4.
2. Manifold: junctions to trunks A + B, SRT bank.
3. Ice + GEN; verify fill.
4. Tank farm (armoured); fill H5 reserve → SPK.
5. Shop + quota ECs.
6. PRJ: load, OFF.
7. Airlock ↑ H2.

### H4 Power

Armoured base · 5 B.

&#91;embedded content: H4 plan · R vault, WB, RB, H2E bay, hex 16 × 17 B\]

&#91;embedded content: power priority · draw order, standby, loads\]

| Item | Value |
| --- | --- |
| A power | 16.8 MW / 6.48 MN → ≈ 2.6 MW/MN |
| Hover (196 MN) | ≈ 510 MW |
| +1 kt mass | ≈ +25 MW |
| Full A (52 A) | ≈ 874 MW |
| R × 3 | 900 MW · N−1 = 600 MW ≥ hover |
| U @ hover | ≈ 510 kg/h (1 kg = 1 MWh) |
| BAT | 12 MW · 3 MWh |
| H2E | 5 MW · 500 L/s |

- Lift on BAT or H2E: no (> 40 BAT, ≈ 100 H2E). Blackout → H lift (R5).
- WB: RCH ≤ 20%, AUTO ≥ 95%. H2E OFF until R fail. RB: RCH; DIS on R fail.
- ☢ r ≈ 60 m → vault WLD ring in REP.

| Block | n |
| --- | --- |
| R (3 × 3 × 3 B, 74 t) | 3 |
| WLD (vault ring) | ≈ 8 |
| Cargo + SRT (U) | 1 + 1 |
| BAT WB · RB | 8 · 6 |
| H2E | 4 |

**Build**

1. PRJ above H5.
2. Vault shell → R × 3 + U links.
3. Load U; 1 R online.
4. WB AUTO · RB RCH.
5. H2E OFF.
6. WLD ring OFF.
7. Wire PWR rungs + F3.

## Pylon template (P × 6)

≈ 13% of A lift each · flown section: built on a ground pad, flies up on its own lift, docks at an H5 socket · drone hangars in the arm · defence bays in the cross-head · 3 hand builds (P-E, P-NE, P-NW), 3 by PRJ rotated 180°; PRJ cannot mirror.

&#91;embedded content: P plan + section · bays, keel, cross-head\]

&#91;embedded content: P docking · 4 moves at the H5 socket\]

| Item | Spec |
| --- | --- |
| Section | W = bay depth = drone L + 4 B, max 9 B (hex face: wider arms collide at the root) · H = 1 B deck + bay H + 2 B keel (example 8 × 7 B) |
| L | 2 B collar + bays × bay W + 5 B cross-head (example 3 × 5 + 7 = 22 B, 55 m) |
| Bays | standard bay, full arm width; doors alternate sides → both flanking T sectors; 3–4 per arm |
| Keel | under the bays: 2 conveyor lines, H2 tanks (heavy, SPK) |
| A | 7 · ≈ 45 MN · always ON |
| H | 4 · ≈ 29 MN† · OFF until EMG |
| Cross-head | plus-shaped tip: A out + both sides (lateral), H out; 2 INT bays, roof hatches; turrets top + bottom; CON pad |
| Root | MRG plate + 2 L CON on the H5 socket; heavy armour collar |
| Stepped P | NE, NW, SW, SE: run 1:2 on the hub grid, 1×2 slope cladding, bays stay square and stagger |
| Seed kit | RC 1 · gyro 2 · BAT bank† · antenna; lifts the arm alone at T% < 80† |

**Build**

1. Root collar, heavy armour; MRG plate + 2 L CON facing the socket.
2. Keel to L: 2 conveyor lines → HUB ring, tanks, SPK.
3. Bay row over the keel: floors, walls, CONs, doors on alternating side walls.
4. A, then H (H disabled).
5. Cross-head: lateral A + H, INT bays, turrets, CON pad.
6. X-link conveyors at tip.
7. Seed kit.
8. Name + groups.
9. Blueprint → PRJ: E → W, NE → SW, NW → SE. Fly up, dock (sequence above).

**Tests**

- [ ] 1 P A OFF → CAU, altitude holds.
- [ ] 2 opposite P OFF → hovers on A.
- [ ] 3 adjacent P OFF → EMG, A + H hold.
- [ ] 1 conveyor line cut → H2 still flows.
- [ ] Drone out of each bay door → T sector, no exhaust contact.
- [ ] Release MRG (creative) → P hovers on its seed kit†.

## Traffic sectors (T × 6)

&#91;embedded content: T sector · plan + section, exhaust-free shaft\]

- T-N, T-ENE, T-ESE, T-S, T-WSW, T-WNW: the open air between pylons, centred on the HUB corners. No structure, ever.
- Lift exhaust falls only under the arms and the HUB → each T is a clear vertical shaft to the surface and to orbit.
- Each T serves the bay doors of its 2 flanking arms: 1 descent lane, 1 climb lane; Dispatcher queues per T.
- T-N and T-S are narrower (≈53° vs ≈63°): a 1×2-slope hex is not quite regular.

## Hangars (HGR)

&#91;embedded content: bay board · 6 arms, 12 INT, 2 PKT (proposal)\]

- Work drones: bays in the arms, doors on side walls into the flanking T. Defence: INT bays in each cross-head, roof hatches. Pickets: 2 pads on the HUB roof.
- Nothing opens ↓ (exhaust). Before bays are fitted: drones on tip CON pads.

&#91;embedded content: standard bay · plan, not to scale\]

- Bay = drone bbox + 1 B all round + 2 B ahead of CON. WLD + SRTs always ON.
- Local logic: rung HG1 (1 EC/HGR) · SNS/bay (doorway → door OPEN) · watchdog → `[HGR] Doors` OPEN.
- Launch/recovery: IGC sequence, Scripts.

## Control modes

&#91;embedded content: mode ladder · T% A bands, not to scale\]

Mode TMRs NRM · CAU · SHD · EMG · CBT · REP · REC: set states only; REC via TMR delay. F1–F3 (H5) never disabled.

## Automation & wiring

&#91;embedded content: automation stack · sense, decide, act, report\]

&#91;embedded content: subsystem interconnection · flows, power, command\]

&#91;embedded content: EC wiring ladder · 33 rungs, 8 groups\]

- **W1** 1 event per EC; pair ECs for ↑/↓ thresholds. **W2** Set actions only (ECs can re-trigger). **W3** > 1 action → TMR, Trigger Now. **W4** ∧ all, ∨ any. **W5** EC sees own + merged/connected grids; AR for the rest. **W6** `[CODE] EC sig` · `[CODE] TMR resp`.

&#91;embedded content: quota cell · rungs M1 + M2\]

- CF% reads the whole container → 1 product per container.

&#91;embedded content: drone cycle · rungs D1-D4, native fallback\]

&#91;embedded content: combat chain · 3 sources, TMR CBT, 5 responses\]

- Decoys on P tips.

## Scripts (PB)

&#91;embedded content: control layers · 4 rig PBs over the native layer\]

| PB | Role | Rate | Acts via |
| --- | --- | --- | --- |
| `[CMD] PB Rig OS` | lift margin · modes · anchor hold · heartbeat | U10 | mode TMRs |
| `[MFG] PB Inventory` | quotas · sorting · ASM queues | U100 | ASM, SRT |
| `[LOG] PB Dispatcher` | jobs · bays · launch/land queue | msg + U100 | IGC, doors, CON |
| `[CMD] PB Displays` | dashboards | U100 | LCD |

- Drone PBs: miner (PAM-like) · ice (miner variant) · interceptor (script or AI Offensive) · picket (none: AI Defensive + AR) · repair (later). All keep D1–D4 + AR RECALL.

&#91;embedded content: IGC sequence · launch, recovery, broadcast\]

- **S1** Native safety never depends on a PB. **S2** Watchdog TMR 30 s; Rig OS Stop → Start each run; 0 → BC + fallback ECs ON. **S3** Fallback ECs OFF while PBs healthy. **S4** Act via TMRs and groups. **S5** Config in Custom Data. **S6** Cache blocks; U100 default. **S7** 1 rack slot per PB.
- Off-the-shelf: Isy's Inventory Manager · MMaster's Automatic LCDs 2.

## Commissioning (per section)

- [ ] MRG engaged; no welds across seam.
- [ ] Conveyor round trip, 1 item.
- [ ] Names `[ID]`; groups complete.
- [ ] Panel row assigned.
- [ ] Section RC OFF; MC in control.
- [ ] T% A in NRM band.
- [ ] EMG test: sheds, H ON.
- [ ] REC via TMR delay.
- [ ] Release test (creative)†.

## Open

- [ ] Planet (assumed Earthlike, 1 g).
- [ ] Survival or creative.
- [ ] Altitude band.
- [ ] Target mass (example 20 kt; hangar arms add mass).
- [ ] Drone sizes → bay size → arm section and P length (drone L ≤ 5 B fits the 9 B arm limit).
- [ ] MFG home: P-NW + P-SE arms (proposed) or HUB.
- [ ] Deck sections: stacked by PRJ on the base (current) or delivered by tug; swap procedure.
- [ ] Bay allocation (board = proposal).
- [ ] H5, H6 deck chapters.
- [ ] Mass reduction pass.

**Verify†**

- [ ] Thrust acts through CoM (1 P OFF, no roll).
- [ ] Severed P keeps dampeners.
- [ ] H2 drain time at full H.
- [ ] Dampener drift vs drone routes.
- [ ] Consumers pull through SRT.
- [ ] Large H thrust value.

**Log**

| Date | Change |
| --- | --- |
| 2026-10-16 | Every module a section: S0 HUB base, S1–S6 flown pylons docking at H5 MRG sockets, S7–S9 stacked decks |
| 2026-10-16 | Hex core + corner spines; 6 P with hangar arms and INT cross-heads; T sectors replace Q; lift 52 A + 30 H, hover now in NRM (was ≈65%, CAU band) |
| 2026-10-15 | Refactor: spec sheets, EC ladder, key, new drawings |
| 2026-10-15 | Guide created |

## Sources

- [Event Controller](https://spaceengineers.wiki.gg/wiki/Event_Controller) · [Large Atmospheric Thruster](https://spaceengineers.wiki.gg/wiki/Large_Atmospheric_Thruster) · [Large Reactor](https://spaceengineers.wiki.gg/wiki/Large_Reactor) · [Battery](https://spaceengineers.wiki.gg/wiki/Battery) · [Hydrogen Engine](https://spaceengineers.wiki.gg/wiki/Hydrogen_Engine) (Space Engineers Wiki)
- [Apex Survival](https://www.spaceengineersgame.com/space-engineers-apex-survival-live-now/) · [Update 1.207 notes](https://support.keenswh.com/spaceengineers/xbox/announcement/update-1-207-apex-survival_2) (Keen Software House)
