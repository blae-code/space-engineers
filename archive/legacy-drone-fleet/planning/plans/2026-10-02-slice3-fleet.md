# Slice 3 — Fleet orchestration Implementation Plan

> **For agentic workers:** this repo executes plans as task cards (see `CLAUDE.md`, "Division of
> labour"). Local cards run through Continue's `/card`; Claude cards run in Claude Code. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** N drones from one carrier mine one shared job, share the carrier's bays, and survive
reloads, link loss and lost drones.

**Architecture:** a pure `Dispatcher` (in `Fleet.Engine/Dispatch/`) owns the job, the leases and the
bay queue, and runs inside a thin carrier `Program`. Drones run a fleet mode in which a `LeaseCursor`
replaces the sequential hole walk. The console only sends commands.

**Tech stack:** MDK2 PB scripts (C# 6), NUnit on Mono, IGC.

**Spec:** `docs/superpowers/specs/2026-10-02-slice3-fleet-orchestration-design.md` (§10 records what is
already built).

## Global constraints
- Script code is C# 6, with no LINQ and none of the banned namespaces (`CLAUDE.md`, "API Restrictions").
- Zero-GC in `Update1/10/100` and `Main`. Message handlers are rare paths and may allocate.
- Every command is unicast. `jobreq` is the only new broadcast, and it starts nothing by itself.
- A drone never depends on the link mid-trip. A missing reply delays a launch or causes a Hold.
- No drone or carrier ever flies at a guessed connector (slice 2 rule).
- Each script stays under 90,000 chars (`fish tools/check-size.fish`).

## Done (2026-10-02, Claude, committed instead of carded — Blae's decision)
`a641030` protocol (`JobCodec`, `FleetMsg`, `FleetLink` v2) · `fc34871` `LeaseTable`, `BayQueue`,
`HoldSlots`, `Dispatcher` incl. Save/Load · `2a7f22c` `LeaseCursor`. Suite 489/489. The spec's card ids
C33–C43 are retired; no card files exist for them.

## Remaining cards
Card files for the gated cards are written **when the gate opens**. They edit drone and console files
that the in-game checklists will probably change first, so anchors written now would go stale.

| Card | File(s) | Depends on | Executor |
|---|---|---|---|
| C44 | `Fleet.Carrier/Program.cs`, `Fleet.Engine/Dispatch/BayQueue.cs`, `Fleet.Engine/Net/FleetMsg.cs` | gate | Claude |
| C45 | `Fleet.Drone.Miner/Subsystems/RemoteSubsystem.cs`, `Fleet.Engine/Core/Enums.cs`, `Fleet.Engine/Core/CommandParser.cs` | gate | local |
| C46 | `Fleet.Drone.Miner/Subsystems/MinerSubsystem.cs`, `Fleet.Drone.Miner/Mining/MinerFsm.cs` | C44, C45 | Claude |
| C47 | `Fleet.Console/Program.cs` | C44 | local |
| C48 | `docs/build-specs/carrier.md`, `docs/build-specs/mining-drone.md` | C46, C47 | local |

**Gate:** the slice-1 checklist G1–G12 (`2026-09-29-slice1-core-miner.md`) and the slice-2 checklist
M1–M8 (`2026-09-30-slice2-moving-carrier.md`) pass in-game with Blae. Until then nothing below starts:
every remaining card touches flight or the live scripts.

### C44 — Carrier integration (Claude)
**Interfaces.** Consumes `Dispatcher` (`OnJob/OnLeaseRequest/OnDock/OnStatus/OnJobRequest/FleetStart/
FleetStop/Tick/Save/Load`, `Outbox`), `BayQueue.SetBays/SetOccupied`, and `HoldSlots.MarkerId`.
Produces the carrier's IGC surface from spec §3.
- [ ] Register unicast handling for tags `job`, `lease`, `dock` and `cmd` (`FLEET START|STOP`), plus
      broadcast listeners for `status` and `jobreq`. Route each to its `Dispatcher` method. Every
      `Update10`: `Tick`, then drain `Outbox` through `IGC.SendUnicastMessage`.
- [ ] Bays: tagged connectors → `SetBays` on setup/rescan. Each `Update10`: `SetOccupied(id,
      status == Connected)`.
- [ ] `[FM] Hold` marker: broadcast as one more beacon entry with `BayId = HoldSlots.MarkerId`;
      `Dispatcher.HasMarker` = the marker exists.
- [ ] Storage: `Dispatcher.Save/Load`. LCD line: job id, done/leased/free, queue length, `Note`.
- [ ] **Review-focus fix (carrier reload during a dock):** `R|2|heldBayId` (a drone already flying a
      dock path re-requests the bay it holds). `FleetMsg.EncodeDock/DecodeDock` accept the optional
      bay. `BayQueue.Request(drone, now, reserve, heldBay, out arg)` re-reserves `heldBay` when it is
      neither occupied nor reserved by another drone. Tests in `BayQueueTests`, `FleetMsgTests` and
      `DispatcherPersistenceTests` (reload, then `R|2|11` from the docking drone → `G|2|11`, and `R|2`
      from another drone → not 11).
- [ ] Settings in the carrier's Custom Data: `LeaseSize` 1–4 (3), `SilentSeconds` 60–1800 (300),
      `ReserveSeconds` 30–600 (90), `HoldSpacing` 5–100 (15).
- [ ] `dotnet build Fleet.Carrier -c Release`; size gate; commit with the test output.

### C45 — Drone inbox + `SHARE` (local, wire)
**Interfaces.** Produces a `FleetInbox` on the drone: the last `job` (`FleetJob`), the last `lease`
(`Lease`), the last dock reply (type + arg), and the carrier address (from beacon `Source`). Adds
`Cmd.Share` (enum, `Names`, `CommandParser`). `SHARE` unicasts `JobCodec.Encode` of the drone's job
(jobId 0) to the carrier. Oracle: smoke tests on the real `Program` with faked IGC (the
`ProgramSmokeTests` pattern). Cases: a lease arrives → inbox holds it; a `job` arrives → inbox holds
it; `SHARE` with a job → one `job` unicast; `SHARE` without a job → a note, and nothing sent.

### C46 — Drone fleet mode (Claude)
- [ ] With a fleet job: the next hole comes from `LeaseCursor`, and `FinishHole` calls `MarkDone`.
      `JobComplete` means `LeaseCursor.Complete`. Status v2 carries jobId/leaseId/mask.
- [ ] Docked after unload and charge: send `N|2|jobId` every 10 s until a lease arrives. A non-empty
      lease relaunches; an empty one → Idle "job complete"; silence → Hold "awaiting lease". A lease
      whose jobId the drone does not hold is ignored.
- [ ] Entering RouteBack: send `R|2` every 10 s until answered. `GO` → `Home.SetHome(bayId)` and dock.
      `WAIT n` → fly to `HoldSlots.Position` (marker tracked by a second `HomeTracker`), hold, wait for
      `GO`. `WAIT -1` → hold in place. While on the dock path: re-send `R|2|bayId` (C44's fix).
- [ ] Shared dock path: a drone with no recorded dock path uses the job's `DockPath`.
- [ ] Storage: `LeaseCursor.Save/Load` and the job. `Reconcile` resumes the lease.
- [ ] Smoke tests and the full suite; size gate (drone < 90,000); commit.

### C47 — Console commands (local, wire)
`SHARE` (selected drone → unicast `SHARE`), `JOB GPS:…[ W H]` (broadcast `jobreq` via
`FleetMsg.EncodeJobRequest`; a null result shows "bad GPS"), and `FLEET START|STOP` (unicast to the
carrier, whose address comes from its beacons). A fleet line on the console screen: carrier heard or
not, job id, done/total, waiting. Oracle: console logic lives in `Fleet.Engine` where it needs tests.

### C48 — Docs (local, doc)
`docs/build-specs/carrier.md`: parts, `[FM]` bays and the `[FM] Hold` marker, carrier settings,
the fleet start procedure. `mining-drone.md`: a fleet section (SHARE, leases, waiting at slots).
Copy the F-checklist below into the checklist section of the slice-2 plan.

## Review focus
The conditions most likely to bite in play that no test exercises yet, each with the card that
owns its test:
1. **Carrier reload while a drone flies a dock path:** reservations are dropped, so another drone
   could get the same bay. Owner: C44 (`R|2|heldBayId`) and C46 (re-send it on the dock path).
2. **A `lease` arriving before its `job`** (IGC order is not a contract): the drone must ignore it
   and keep requesting, never mine indices of an unknown grid. Owner: C46 smoke test.
3. **A drone out of antenna range longer than `SilentSeconds` while drilling:** its hole is
   re-leased and may be met by a second drone. Mitigated (re-leased holes go last; 300 s default);
   check in-game F4, and show the expiry on the carrier LCD (C44).
4. **More than 16 drones:** `Roster.Capacity` is 16, so drone 17 gets leases but never a
   `FLEET START`, and never appears on the LCD. Owner: C44, which shows "roster full" on the LCD.
5. **`jobreq` while a job is running:** it replaces the job after the lead shares. Expected: the
   operator is warned. Owner: C47 (the console asks for `JOB … FORCE` while a job is active).

## In-game checklist (with Blae, after C44–C47)
- [ ] F1 Two drones, two bays: SHARE from one, FLEET START; both mine disjoint holes; the job completes.
- [ ] F2 Three drones, two bays: the third waits at slot 0 above the hold marker, then docks.
- [ ] F3 `JOB GPS:…` from the console: the carrier picks a lead; the fleet mines the site.
- [ ] F4 Destroy a drone mid-lease: after `SilentSeconds` its holes are re-leased and completed.
- [ ] F5 Reload mid-job: leases, queue and drones recover; no collision (incl. Review focus 1).
- [ ] F6 Carrier PB off mid-job: drones finish their trip and hold docked "awaiting lease"; PB on → resume.
- [ ] F7 Moving carrier with two drones: holding slots move with the carrier.
- [ ] F8 A drone with no recorded dock path docks using the shared path.

## Checkpoint log
- 2026-10-02 — Pure layer built and committed by Claude (see "Done"). Process change recorded in
  `CLAUDE.md`: do not write a full reference implementation to validate a logic card. Every test
  expectation in this deck held on the first run, so the reference was duplicate work.
