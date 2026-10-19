# Crawler Build Guide

Oct 16, 2026 · @Blae

CR-1 is the TMS mining vehicle: it surveys semi-manually, parks over a deep node, cuts an inverted cone with a pan-tilt laser, and fills Bulk Pods at its stern for MU-1 to haul.

## Key

**Shared TMS codes** (identical in the Mule guide)

| Code | Meaning | Code | Meaning |
| --- | --- | --- | --- |
| TMS | Terrestrial Mining Subsystem | GD | Ground Depot (sort, refine, silo) |
| CR | Crawler, mining vehicle | LP | Lift Pad at GD (Rig pickup) |
| MU | Mule, pod hauler | RL | Rig Lifter (heavy-lift drone) |
| BP | Bulk Pod (pod sled + bulk container) | FP | Fill Port (CR stern dock) |
| HD | Laser head (boom + pan-tilt) | SP | Saddle Port (MU pod dock) |
| LZ | Laser multitool (fixed) | TH | Thumper (Seismic Surveyor) |
| OD | Ore detector | WS / DS | Wide Survey / Detailed Scan LCD |
| AR | Advanced rotor | SRT | Conveyor sorter |
| CON | Connector | MRG | Merge block |
| LG | Landing gear | EC | Event Controller |
| TB | Timer block | PB | Programmable block |
| IGC | Inter-grid comms (PB) | CD | PB Custom Data |
| BAT | Battery | H2E | Hydrogen engine |
| ANT | Antenna | BCN | Beacon |

**Symbols**

| Sym | Meaning | Sym | Meaning |
| --- | --- | --- | --- |
| ⊕ | Connector | ⊞ | Merge block |
| ▼ | Landing gear, locked | ◎ | Rotor axis |
| ⇢ | Conveyor path | ⚡ | Power source |
| ◆ | Scripted control point | ▲ | Native-only safety |
| ⌀ | Diameter | r | Radius |
| D | Depth below collar | h | Head height above collar |

**Units:** m, t, L, kW, MW, MN. Cargo figures assume 1× inventory; scale by the world multiplier.

**Mods in TMS**

| Mod | ID | Role | Needs |
| --- | --- | --- | --- |
| \[QoL\] Seismic Surveying | 2821219378 | TH, WS, DS | DEA, Seismic gear |
| Seismic surveying landing gear | 2819263781 | TH models | — |
| Definition Extension API (DEA) | 2756894170 | Dependency | — |
| Simple Laser Multitools | 3175774286 | LZ, weld turret | ToolCore |
| ToolCore | 3040438530 | Dependency | — |
| \[QoL\] Radio Spectrometry | 2687324923 | Not used in TMS (asteroids only); Rig orbital ops | DEA |

Vanilla + all DLC otherwise. Economy 2 Pack required for BP.

## Subsystem context

Every TMS unit touches ore only through the Bulk Pod; CR and MU are the two units these guides cover.

&#91;embedded content: TMS material path · 5 units, 1 return loop\]

Rare ores refine at GD (low yield makes lifting raw ore wasteful); common ores may go raw to the Rig.

**BP standard** (shared, identical in the Mule guide)

| Item | Spec |
| --- | --- |
| Container | Bulk Cargo Container A/B/C (Economy 2 Pack), L grid, 6 × 3 × 3 blocks (15 × 7.5 × 7.5 m) |
| Capacity / empty mass | 820,000 L / 5,198 kg |
| Native ports | 1 per short end, 2 on one long side |
| ⊕ CON-F | Short end A port; fill (CR) and drain (GD) |
| ⊞ MRG-T | Roof centreline, 2 blocks; MU saddle |
| ▼ LG ×4 | Belly corners; parks BP on ground |
| Long-side ports | Spare; GD floor-unload option (Mule guide D-M4) |
| BCN + small BAT | Name = BP id (`BP-07`); keeps beacon lit off-grid |
| Rule | No PB, no ECs on BP; vehicles watch BP through ⊕/⊞ |

## Design rules

| # | Rule | Why |
| --- | --- | --- |
| C1 | BP standard is the only cargo interface; CR, MU, GD, RL all dock to it | Vehicles evolve independently |
| C2 | ▲ native blocks own safety; ◆ PB owns aim and sequencing | Script crash = beam off, never beam loose |
| C3 | LZ fires only if TH ▼ locked, grid speed < 0.1 m/s, dead-man TB alive | Anchored, stationary, supervised |
| C4 | Head motion = 2 AR only; no pistons, no hinges in the cut chain | Clang risk; AR passes conveyor |
| C5 | Conveyor chain unbroken: LZ ⇢ AR-T ⇢ AR-P ⇢ boom ⇢ SRT-D ⇢ buffer | Ore lands in LZ inventory |
| C6 | Fill BP by mass target (CD `fill_pct`, default 10%), never by volume | Full BP ≈ 2,200 t at 1× |
| C7 | Two fill ports, FP-A / FP-B; cutting never waits on MU | Swap one while the other fills |
| C8 | Collar under the head, never under the hull: wheel contact ≥ collar r + 4 m from collar ⌀ centre | Hull never undermined |
| C9 | Rays aimed only into the ore band; depth closed-loop by camera raycast | Bounded waste, no blind digging |
| C10 | Beam duty-cycled (REST state) | Laser drilling can drag sim speed |
| C11 | CR never refines; GD refines | Keeps CR light and mobile |
| C12 | Every EC/TB action *sets* a state (On, Off, Lock), never toggles | EC can re-trigger the same slot |
| C13 | One FP piston (≤ 2 m stroke) allowed for final connector engagement | Only piston on CR |

## General arrangement

CR-1 is a 16 × 7-block six-wheeler: the cut chain hangs off a bow boom, two BP slots sit aft, and the hull kneels to anchor.

&#91;embedded content: CR-1 GA · side elevation + plan · 9 callouts\]

**Callouts**

| # | Item | Note |
| --- | --- | --- |
| 1 | BP-A on 4 ▼ LG skids | Separate grid; BP-B alongside (plan) |
| 2 | FP ⊕ on 1 piston ≤ 2 m | Only piston on CR (C13) |
| 3 | Wheel Suspension 5×5 ×6 | 3 per side, outboard |
| 4 | Keel: TH centreline + LG ×2 | Hidden; touches ground when CR kneels |
| 5 | OD (Large) | Cab roof, clear of boom |
| 6 | Cab | Cockpit, LCD WS / DS / STAT |
| 7 | Boom | 5 b beyond bow; conveyor inside |
| 8 | Drop mast ⇢ AR-P ⇢ AR-T ⇢ LZ + CAM-HD | Head ≈ 4 m above ground |
| 9 | Collar | r ≈ 6 m; forms itself |

**Principal dimensions**

| Dim | Value |
| --- | --- |
| Hull L × W | 16 × 7 b (40 × 17.5 m) |
| Wheel ⌀ | ≈ 5 b assumed for L-grid 5×5 (T13) |
| Boom reach past bow | 5 b (12.5 m) |
| Head height h | ≈ 1.6 b (4 m) |
| Front tyre → collar edge | 6.5 m ≥ 4 m (C8) |
| BP slots | 2 aft, centres ± 2 b off centreline, 0.5 b gap |
| Anchoring | Kneel (suspension height) until TH + LG ▼ (T14) |

## Head assembly

Two advanced rotors give pan and tilt; both pass items, so ore never leaves the conveyor between the LZ and the hull.

&#91;embedded content: Head assembly · front elevation · ore path\]

**Parts**

| Part | Block | Spec | Settings |
| --- | --- | --- | --- |
| Drop mast | Armor + conveyor tube | 2 b below boom tip | Sets h ≈ 4 m |
| AR-P | Advanced Rotor (L), axis vertical | 1,006 kg, 2 kW, items pass | Limits ±180°; Lock when idle; max brake torque |
| Bracket | Armor + conveyor junction | Offsets AR-T 2 b | — |
| AR-T | Advanced Rotor (L), axis horizontal | Same as AR-P | Limits −90° (straight down) to −20° |
| LZ | Simple Laser Multitool (L) | 300 m, r 2.5 m, 0.5 MW; secondary = wider, slower | Drill mode via PB |
| CAM-HD | Camera | Co-axial with LZ, faces down-beam | Raycast enabled by PB |
| SRT-D | Conveyor Sorter | Drain All | At boom root (Material flow) |

**Aim servo** (◆ PB, per AR)

- Velocity = k · (θ\_target − θ), clamped to ± 2 RPM; k = 0.5 to start.
- Within 0.2°: velocity 0 · Lock · LZ On.
- Unlock only to move; locked AR is the resting state (C2).

## Survey package

Survey is manual in v1: TH finds the node at range, OD pins it, you hand CR a GPS. The handoff (CD `[site]`) is the seam a scripted survey replaces later.

**Hardware**

| Block | Qty | Spec | Mount |
| --- | --- | --- | --- |
| TH (Seismic Surveyor, L) | 1 | Range 3,000 m; must be ▼ locked to a planet; Scan action | Belly, centreline, arrows → bow |
| LG | 2 | Anchors; ▼ with TH while cutting | Belly, port + stbd, mid-hull |
| OD (Large) | 1 | ≈150 m HUD markers | Roof, clear of boom |
| LCD WS | 1 | Seismic Wide Survey script | Cab, centre |
| LCD DS | 1 | Seismic Detailed Scan script | Cab, right |
| LCD STAT | 1 | PB status (state, ray n/N, pod %) | Cab, left |
| Cockpit toolbar | — | TH Scan · Rotate L5/L45/R5/R45 · OD on/off | Page 1 |

**Loop**

1. Drive; ▼ TH; Scan.
2. WS: rotate scan line onto strongest node; DS: confirm ore (spikes at all 3 key points).
3. Unlock; drive on bearing \~1 km; repeat 1–2 until the OD marker appears (≤150 m).
4. Park CR so the marker sits ahead of the bow, under the head's reach (C8).
5. Read marker distance with CR stopped over it ⇒ approx D to ore top; probe a second marker for ore bottom if visible.
6. Create GPS at the marker → paste into CD (see Script).

**Unknowns** (see Commissioning): DS depth readout; WS multi-scanner display; PB access to TH data.

## Excavation

Each ray is one straight cut from the head to the ore floor; rays target only the ore band, so the stone removed is the narrow cone above it (≈ 26% of volume in the example).

&#91;embedded content: Inverted cone · section + ray plan · worked example\]

**Geometry rules**

- Collar r ≈ ore\_r · h / (h + D\_mid) + 2.5 m beam r ⇒ ≈ 4.7 m in the example; GA allows 6 m.
- Waste fraction ≈ ((h + D\_top) / (h + D\_bot))³ ⇒ (42 / 66)³ ≈ 0.26.
- Lower h widens the bell; raise h to narrow it.
- SE1 voxels do not collapse; the undercut bell holds.
- Ray time = L\_stop / cut rate (T8); total ≈ 177 × ray time × (duty\_on + duty\_off) / duty\_on.

&#91;embedded content: CR-OS laser states · 10 states\]

HOLD, FAULT and MANUAL can interrupt AIM or CUT at any time; only `resume` or `auto` re-enters AIM, never CUT directly.

## Material flow

Ore lands in the LZ's own inventory; SRT-D drains it through both AR into a 4 × Small Cargo Container buffer (62,500 L, ≈169 t ore; sized to ride out one BP swap, not to store), and the active fill port pushes it into the docked BP.

```
 LZ(inv) ⇢ AR-T ⇢ AR-P ⇢ BOOM ⇢ SRT-D ──⇢ BUF [SCC×4] ──┬─⇢ SRT-A ⇢ FP-A ⊕ ⇢ BP-A
  ◆ aim     ◎tilt   ◎pan          drain-all   ▲EC1 ≥90%   ├─⇢ SRT-B ⇢ FP-B ⊕ ⇢ BP-B
                                                         └─⇢ SRT-S ⇢ EJ ⊕  (stone, if T3)
```

**Sorters**

| Block | Mode | Filter | Switched by |
| --- | --- | --- | --- |
| SRT-D | Drain All | Whitelist: all ore + stone | Always on |
| SRT-A | Drain All | Whitelist: all ore | EC: FP-A connected / BP-A full |
| SRT-B | Drain All | Whitelist: all ore | EC: FP-B connected / BP-B full |
| SRT-S | Drain All | Whitelist: stone | Off unless T3 shows stone collected |
| EJ | Connector, Throw Out | — | Paired with SRT-S |

**BP fill targets** (1× inventory; ore 0.37 L/kg; BP 820,000 L)

| `fill_pct` | Volume | Ore mass | Use |
| --- | --- | --- | --- |
| 5% | 41,000 L | ≈111 t | MU first trials |
| 10% | 82,000 L | ≈222 t | Default |
| 20% | 164,000 L | ≈443 t | If MU tests pass (Mule guide) |
| 100% | 820,000 L | ≈2,216 t | Static silo only, never hauled |

EC *Cargo Filled %* watches the docked BP directly: an EC monitors connected and merged grids, not only its own.

## Power and mass budget

CR runs two power modes that never overlap: DRIVE (≤ 9.5 MW peak, wheels) and CUT (≈ 1 MW, laser + head). Two large batteries cover both; one H2E keeps CUT running indefinitely.

**Loads**

| Load | Qty | Each | DRIVE | CUT |
| --- | --- | --- | --- | --- |
| Wheel Suspension 5×5 (L) | 6 | 1.5 MW | 9.0 MW | 0 (▼ locked) |
| LZ (L, multitool) | 1 | 0.5 MW | 0 | 0.5 MW |
| AR (L) | 2 | 0.002 MW | 0 | 0.004 MW |
| EC (L) | ≈10 | 0.0005 MW | 0.005 MW | 0.005 MW |
| TH, OD, PB, LCD, ANT, camera | — | measure | < 0.5 MW | < 0.5 MW |
| **Total** |  |  | **≤ 9.5 MW** | **≈ 1.0 MW** |

**Supply**

| Source | Qty | Output | Store | Role |
| --- | --- | --- | --- | --- |
| Battery (L) | 2 | 12 MW each | 3 MWh each | DRIVE peaks; CUT ≈ 6 h alone |
| H2E (L) | 1 | 5 MW | H2 tank | Recharges BAT during CUT ⚡ |
| Solar / wind | opt. | — | — | Trickle on roof; never a primary |

**Known masses** (rest measured from the in-game Info tab after blueprint)

| Block | Qty | Each | Subtotal |
| --- | --- | --- | --- |
| Wheel Suspension 5×5 (L) | 6 | 2,820 kg | 16.9 t |
| Battery (L) | 2 | 3,845 kg | 7.7 t |
| AR (L) | 2 | 1,006 kg | 2.0 t |
| EC (L) | ≈10 | 304 kg | 3.0 t |
| Buffer ore at EC1 trip (90%) | — | — | ≈ 152 t |

The buffer at trip point outweighs the rest of CR; keep the buffer between the axles.

## Control logic

Thirteen native rungs (▲) guarantee the beam stops on any unsafe state; the PB (◆) only decides where to aim and when to step. Every slot *sets* a state (C12).

**EC / TB ladder**

| Rung | Block | Event · condition | Watches | Slot 1 (true) | Slot 2 (false) | Type |
| --- | --- | --- | --- | --- | --- | --- |
| R1 | EC-BUF | Cargo Filled ≥ 90% (any) | SCC ×4 | LZ Off · PB `hold` | PB `resume` | ▲ |
| R2 | EC-LZI | Cargo Filled ≥ 90% | LZ inventory | LZ Off | — | ▲ |
| R3 | EC-PA | Cargo Filled ≥ `fill_pct` | BP-A container | SRT-A Off · SRT-B On · BCN-A On · PB `fullA` | — | ◆▲ |
| R4 | EC-PB | Cargo Filled ≥ `fill_pct` | BP-B container | SRT-B Off · SRT-A On · BCN-B On · PB `fullB` | — | ◆▲ |
| R5 | EC-FA | Connector Connected | FP-A | PB `dockA` | SRT-A Off · PB `undockA` | ◆ |
| R6 | EC-FB | Connector Connected | FP-B | PB `dockB` | SRT-B Off · PB `undockB` | ◆ |
| R7 | EC-RTL | Connector Ready to Lock (any) | FP-A, FP-B | FP-A Lock · FP-B Lock | — | ▲ |
| R8 | EC-ANC | Landing Gear Locked (AND) | TH, LG ×2 | PB `anchored` | LZ Off · AR-P Lock · AR-T Lock · PB `loose` | ▲ |
| R9 | EC-SPD | Grid Speed ≥ 0.1 m/s | — | LZ Off | — | ▲ |
| R10 | EC-PWL | Stored Power ≤ 20% (AND) | BAT ×2 | H2E On · LZ Off | — | ▲ |
| R11 | EC-PWH | Stored Power ≥ 80% (AND) | BAT ×2 | H2E Off | — | ▲ |
| R12 | EC-DMG | Block Integrity ≤ 90% (any) | LZ, AR-P, AR-T, boom | LZ Off · Sound On · PB `fault` | — | ▲ |
| R13 | EC-SEAT | Cockpit Occupied | Cab seat | PB `manual` | PB `auto` | ◆ |
| DM | TB-DM | Timer 10 s, re-armed by PB every run | — | LZ Off · AR-P Lock · AR-T Lock · BCN `CR FAULT` | — | ▲ |

**Interlock** (all true or LZ stays off): R8 anchored · R9 false · R1/R2 false · R12 false · TB-DM armed.

EC count 13 × 0.0005 MW; a small-grid EC/TB cluster on a locked rotor saves space and components.

## Script

One PB, `CR-OS`, turns the CD site block into a ray list, aims the head ray by ray, and stops each ray by camera raycast at the ore floor. Written from scratch (your rule); all safety stays in the ladder.

**CD schema** (pasted once per site)

```ini
[site]
gps=GPS:Fe-01:x:y:z:   ; OD marker, ore centre
ore_top=38              ; D to ore top, m
ore_bottom=62           ; D to ore bottom, m
ore_r=30                ; ore band radius, m
aim_step=4              ; ray spacing at ore mid-depth, m
fill_pct=10             ; BP mass target, %
duty_on=4               ; s beam on
duty_off=1              ; s REST
[head]
h=4                     ; head height above ground, m
pan=AR-P
tilt=AR-T
laser=LZ
cam=CAM-HD              ; camera on AR-T, co-axial with LZ
```

**Ray plan** (polar rings at ore mid-depth D\_mid; ring k radius r\_k = k · s)

```latex
n_k = \max\left(1,\ \mathrm{round}\left(\frac{2\pi r_k}{s}\right)\right) \qquad L_{stop} = |\vec v|\,\frac{h + D_{bot}}{h + D_{mid}}
```

- ◆ Target P on ring k at angle φ; v = P − O\_head (grid frame).
- ◆ Pan = atan2(v·right, v·fwd); tilt = atan2(−v·up, |v\_horizontal|).
- ◆ CUT until camera raycast hit distance ≥ L\_stop, then next ray.
- Example: ore\_r 30 m, s 4 m ⇒ 7 rings + centre, 177 rays.
- The collar forms itself: every ray passes within ≈ h · tan θ\_max + 2.5 m of the head's foot.

**PB arguments** (from EC/TB slots or toolbar)

`plan` · `start` · `stop` · `hold` · `resume` · `fullA` · `fullB` · `dockA` · `dockB` · `undockA` · `undockB` · `anchored` · `loose` · `fault` · `manual` · `auto`

**IGC** (tags shared with MU; Rig joins later)

| Tag | Direction | Payload | Trigger |
| --- | --- | --- | --- |
| `TMS.POD` | CR → broadcast | `FULL;A;<slot GPS>;<t>` | R3 / R4 |
| `TMS.ACK` | MU → CR | `ENROUTE;A;<eta s>` | MU accepts job |
| `TMS.SWAP` | MU → CR | `ARRIVED;A` | MU at slot A |
| `TMS.SWAP` | CR → MU | `RELEASED;A` | SRT-A Off, FP-A Unlock done |
| `TMS.SWAP` | MU → CR | `PLACED;A` | Empty BP set in slot A |
| `TMS.STAT` | CR → broadcast, 10 s | `<state>;<ray i/N>;<A%>;<B%>;<BAT%>` | Always |

**Run rate:** Update10 in AIM/CUT; Update100 otherwise. Each run re-arms TB-DM.

## Commissioning

Two of twelve tests are already closed; the head test stand clears five more before any hull is built.

**Tests**

| # | Question | Method | Result |
| --- | --- | --- | --- |
| T1 | Ore lands in LZ inventory? | Creative cut | Yes (confirmed) |
| T2 | PB controls LZ modes and actions? | Creative | Yes (confirmed) |
| T3 | Beam collects stone? | Cut stone-only patch, watch LZ inv | Open → SRT-S / EJ |
| T4 | DS shows node depth? | Read DS on known patch | Open |
| T5 | TH data readable by PB? | `seismic_probe.cs`, or read mod source | Open → survey automation |
| T6 | WS shows other grids' TH? | Second rover, 500 m out | Open |
| T7 | Camera raycast returns voxel distance down a narrow bore? | Stand + PB echo | Open |
| T8 | Cut rate (m of ray / s) and sim speed at 4/1 duty | Stand, stopwatch, sim % | Open → `duty_on/off` |
| T9 | Aim error at 80 m slant ≤ 1 m? | Paint target, compare | Open |
| T10 | Ore passes LZ ⇢ AR-T ⇢ AR-P ⇢ SRT-D? | Stand | Open |
| T11 | FP piston closes connector gap ≤ 2 m reliably? | Stand + dummy BP | Open |
| T12 | EC reads connected BP Cargo Filled %? | Dock BP, trip R3 | Open (wiki says yes) |
| T13 | L-grid 5×5 wheel diameter ≈ 5 b? | Place one, measure | Open → GA dims |
| T14 | Kneel travel (suspension height) puts keel TH + LG on ground? | Hull mock-up, flat and 10° slope | Open → else piston outriggers |

**Build order**

- [ ] Mods loaded: DEA → Seismic gear → Seismic Surveying → ToolCore → Laser Multitools
- [ ] Creative Earthlike test world at 1× inventory
- [ ] Head test stand (boom, AR-P, AR-T, LZ, CAM-HD, SRT-D, 1 SCC): T7–T10
- [ ] CR hull + wheels + power; wire R1–R13, TB-DM
- [ ] Force each interlock condition; LZ must drop every time
- [ ] Load `CR-OS`; `plan` over a known patch; dry run with LZ Off (aim only)
- [ ] Live cut 10 rays; check BUF fill and SRT-D drain
- [ ] Dock BP-A; fill to 5%; confirm R3 swaps to B (T11, T12)
- [ ] Survival trial with MU

## Open decisions

| # | Decision | Options | Blocked by |
| --- | --- | --- | --- |
| D1 | World inventory multiplier | 1× · 3× · 10× (rescales every mass figure) | — |
| D2 | Wheel count | 6 × 5×5 · 8 × 5×5 | Hull mass + 152 t buffer |
| D3 | Stone handling | Eject at site · keep for gravel | T3 |
| D4 | Who repairs CR | MU-mounted weld LZ · small repair drone | ToolCore own-grid limit |
| D5 | Survey automation path | PB reads TH · small companion mod via DEA · stay manual | T5 |
| D6 | H2 supply | Tank only · ice from the cut ⇢ O2/H2 generator | Ore type at site |
| D7 | BP sled frame | Specified in the Mule guide (Role in TMS) | — |

## Sources

- [\[QoL\] Seismic Surveying](https://steamcommunity.com/workshop/filedetails/?id=2821219378) · [Seismic surveying landing gear](https://steamcommunity.com/workshop/filedetails/?id=2819263781) · [Definition Extension API](https://steamcommunity.com/workshop/filedetails/?id=2756894170) · [\[QoL\] Radio Spectrometry](https://steamcommunity.com/sharedfiles/filedetails/?id=2687324923)
- [Simple Laser Multitools](https://steamcommunity.com/sharedfiles/filedetails/?id=3175774286) · [ToolCore](https://steamcommunity.com/workshop/filedetails/?id=3040438530)
- [Bulk Cargo Containers](https://spaceengineers.wiki.gg/wiki/Bulk_Cargo_Containers) · [Economy 2 Update & Pack](https://www.spaceengineersgame.com/space-engineers-economy-2/) · [Small Cargo Container](https://spaceengineers.wiki.gg/wiki/Small_Cargo_Container)
- [Advanced Rotor](https://spaceengineers.wiki.gg/wiki/Advanced_Rotor) · [Wheel Suspension 5x5](https://spaceengineers.wiki.gg/wiki/Wheel_Suspension_5x5) · [Battery](https://spaceengineers.wiki.gg/wiki/Battery) · [Event Controller](https://spaceengineers.wiki.gg/wiki/Event_Controller) · [Camera](https://spaceengineers.wiki.gg/wiki/Camera)
