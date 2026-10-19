# Slice 3 — Fleet orchestration: several drones, one job, shared bays

Status: design approved in conversation 2026-10-02 (Blae); this document awaits review.
Builds on slice 1 (`2026-09-29-slice1-core-miner-design.md`, incl. §10 remote console) and slice 2
(`2026-09-30-slice2-moving-carrier-design.md`).

## 1. Goal and decisions

**Outcome:** the operator defines one mining job and presses one button. N small-grid drones based on
one carrier then mine that job together. They share the carrier's connectors without colliding, and
the job survives reloads, link loss and a lost drone.

| Decision (Blae, 2026-10-02) | Choice |
|---|---|
| What drones do together | **Split one job**: N drones share one hole grid at one site |
| Dispatcher | **The carrier PB** (`Fleet.Carrier`). `Fleet.Mothership` is deferred until there are several carriers |
| Drone ↔ connector | **Any free bay.** Drones may outnumber bays. Waiting drones hold at numbered slots |
| Job source | **Both manual and probe:** a lead drone's job (SETJOB / GOTO + SHARE), or a GPS job request from the console or, later, a probe |
| Hole allocation | **Leases**: small batches of spiral indices, expiry by silence |

Unchanged rules: drones are commanded, not piloted. **A drone never depends on the link mid-trip:**
a missing reply delays a launch or causes a Hold, and never sends a drone anywhere new. A drone that
hears no dispatcher behaves exactly as in slices 1–2 (solo mode).

**Out of scope:** a probe script (only the message a probe will send is specified here), several
carriers or a mothership, several simultaneous jobs, approach-lane deconfliction between adjacent bays
(bays are assumed spaced so their dock paths do not cross), mixed drill layouts (see §6), and obstacle
avoidance (slice 5).

Vanilla constraint: a programmable block cannot read ore-detector results, so a probe's "discovery"
reaches the fleet as a GPS, not as an ore reading.

## 2. Roles

| Piece | Where | Responsibility |
|---|---|---|
| `Dispatcher` (pure) | `Fleet.Engine/Dispatch/` | Owns the shared job, `LeaseTable`, `BayQueue`, and the drone roster (reuses `Roster`). Turns inbound messages into outbound replies; Save/Load |
| Carrier `Program` | `Fleet.Carrier/` | Thin adapter: IGC listeners, connector state, hold-marker beacon, Storage, LCD line. Existing bay beacons unchanged |
| Drone fleet mode | `Fleet.Drone.Miner/` | `LeaseCursor` (pure) picks the next hole from the lease. `MinerSubsystem` mines leased holes, returns when the lease is done, requests bays, holds at slots |
| Console | `Fleet.Console/` | Commands only (`SHARE`, `JOB`, `FLEET START/STOP`), plus a fleet summary line |

Pure code that the carrier or console need lives in `Fleet.Engine`, as `Roster` does today:
`Fleet.Tests` can reference only one PB project (the drone), and trim keeps unused types out of every
script.

## 3. Protocol (IGC, channel `FLEET/<Channel>/…`)

All payloads are versioned `|`-separated text lines, like `status` (`FleetLink.Version` → 2). Numbers
are integers only: positions and spacing in millimetres, directions in millionths (culture-free). Every command stays **unicast**, as in §10.2 of
slice 1. The one broadcast addition is `jobreq`, which only asks the carrier to act and starts nothing
on its own.

| Tag | Direction | Payload |
|---|---|---|
| `job` | drone → carrier, unicast | `J|2|jobId|ox|oy|oz|fx|fy|fz|ux|uy|uz|W|H|spacing|gps|nRoute|route xyz…|nDock|dock xyz…`. The grid in world space, the job route in world space, and the lead's dock path local to its connector. The drone sends jobId 0; **the carrier mints the id** and sends the job back (carrier → drone) to the lead and to every drone before its first lease |
| `jobreq` | console/probe → broadcast | `Q|2|GPS:name:x:y:z:|W|H` (W/H 0 = the chosen drone's settings) |
| `lease` | drone → carrier, unicast | `N|2|heldJobId`: request a lease (0 = no job held) |
| `lease` | carrier → drone, unicast | `L|2|jobId|leaseId|n|i1…in` (n ≤ 4 spiral indices). `n = 0` means none left: the job is complete |
| `dock` | drone → carrier, unicast | `R|2` (request a bay) |
| `dock` | carrier → drone, unicast | `G|2|bayId` (go) or `W|2|slot` (wait at slot; `-1` = no hold marker, hold in place) |
| `status` | drone → broadcast (existing) | v2 appends `jobId|leaseId|doneMask` (bit k = the lease's k-th index). The decoder still accepts v1 lines (new fields 0) |
| `bay` beacon | carrier → broadcast (existing) | unchanged; plus one entry for the hold marker, with reserved `BayId = -1` |

The drone learns the carrier's address from the `Source` of its bay beacons.

**Job intake:**
- *Manual, lead drone:* SETJOB or `GOTO GPS:…` on a drone, as today. Then `SHARE` (a new drone command,
  sent from the console like any other) makes it unicast `job` to the carrier.
- *GPS request (console now, probe later):* `jobreq` makes the carrier pick an idle, docked drone from
  its roster and unicast `GOTO <gps>` followed by `SHARE` to it. `GOTO` builds the grid at command time
  (`SetJobGps` → `DefineJob`), so no flight happens before the share.
- A new `job` replaces the active one (new `jobId`). Drones finish their current trip, then receive
  leases on the new job.

**Fleet start:** `FLEET START` is unicast console → carrier. The carrier then unicasts `START` to each
docked roster drone that knows the job (it hears the jobId on that drone's status line).
`FLEET STOP` is the same, with `STOP`.

**Shared dock path:** drones with no recorded dock path use the lead's, from the `job` message.
Identical bays make one connector-local path valid on every bay.

## 4. Lease table (`Fleet.Engine/Dispatch/LeaseTable.cs`)

- Holes are spiral indices `0 … W·H−1` (`HoleGrid.Spiral`). Arrays are preallocated for the settings
  maximum (50 × 50 = 2,500) and cleared per job.
- Each index is `Free`, `Leased`(leaseId, drone) or `Done`. Each lease records its drone and
  `lastHeard`.
- `Grant(drone, now)`: if the drone holds a lease with unfinished holes, return **that same lease**
  (idempotent, so a lost reply is harmless). Otherwise take the next `LeaseSize` free indices (setting,
  default 3, range 1–4) in free order, mint a new lease id, and return it. With none free, return the
  empty lease (job complete once nothing is leased either).
- `Heard(drone, leaseId, doneMask, now)` (from each status line): refresh `lastHeard`, and mark Done
  every index whose mask bit is set. Idempotent.
- `Expire(now)`: a lease silent for `SilentSeconds` (setting, default 300) frees its unfinished indices.
  They go to the **end** of the free order, so a drone that was only out of range rarely meets a new
  lessee at the same hole. A drone that reappears with an expired lease gets a new one on its next
  `Grant`. A hole drilled twice is harmless.
- Counts for the UI: done / leased / free.

## 5. Bay queue and holding (`Fleet.Engine/Dispatch/BayQueue.cs`, `HoldSlots.cs`)

- Bays are the carrier's tagged connectors (ids = entity ids, as in the beacon). Each is `Free`,
  `Reserved`(drone, until) or `Occupied`; the adapter reports occupancy from the connector status.
- `Request(drone, now)`: a drone that already holds a reservation gets the same `GO` (idempotent).
  Otherwise, with a free bay, it reserves the bay for `ReserveSeconds` (setting, default 90) and
  replies `GO bayId`. With no free bay, it appends the drone to a FIFO queue and replies `WAIT slot`,
  where slot = queue position.
- `Tick(now)`: drop expired reservations. When a bay is free and the queue is not, reserve it for the
  head of the queue and emit `GO` to that drone (push, unicast). Remaining drones move up a slot (a
  new `WAIT` is sent only when the slot number changes).
- Drone side: send `R` when **entering RouteBack** (and every 10 s until answered), so the answer
  arrives before the carrier does. On `GO`, set home to that bay (`Home.SetHome(bayId)`, existing),
  then dock as today along the connector-local dock path. On `WAIT n`, fly to slot `n`, hold there, and
  wait for `GO`.
- **Hold marker:** the operator tags one carrier block `[FM] Hold`. The carrier broadcasts it as beacon
  entry `BayId = -1`. Slot `n` pose = marker position + `(n + 1) · HoldSpacing` (so slot 0 is clear of the marker block) (setting, default 15 m)
  along the marker's up axis, with the marker's velocity, extrapolated exactly like a tracked bay
  (`HomeTracker`).
- No marker while drones are waiting: the carrier shows a setup diagnostic and replies `WAIT -1`. The
  drone then holds where it is: at the start of RouteBack, out at the job, not in the dock lane.

## 6. Drone fleet mode

- **Entering:** the lead enters fleet mode when it sends `job` (on `SHARE`). Every other drone enters
  when the carrier sends it `job`, which the carrier always sends immediately before that drone's first
  `lease` for a given jobId. A lease whose jobId the drone does not hold is ignored, and the drone keeps
  requesting.
- **Hole choice:** `LeaseCursor` (pure, `Fleet.Drone.Miner/Mining/`) holds the lease's indices and
  done mask. `Next()` gives the first unfinished index; `MarkDone()` sets the bit. It replaces the
  sequential `HoleIndex` walk while a fleet job is active. Solo mode is unchanged.
- **Lease done:** with every leased hole done, the drone returns with reason `JobDone` (no new enum
  value: in fleet mode the FSM's `JobComplete` means "lease complete").
- **At home**, after unload and charge, the drone requests a lease. A non-empty lease relaunches it
  automatically. An empty lease leaves it Idle, "job complete". No reply within 10 s leaves it docked
  in Hold, "awaiting lease", retrying every 10 s.
- **Mixed drill layouts:** the job carries the lead's spacing; drones with other drill layouts mine at
  that spacing (overlap or gaps). Documented, not solved.
- Storage: jobId, the lease, and the done mask are saved. On reload, `Reconcile` resumes the lease.

## 7. Failure handling

| Event | Behaviour |
|---|---|
| Carrier PB off or destroyed | Drones out finish their lease. `R` is retried every 10 s; the slice-2 "carrier beacon lost" Hold applies as before. Docked drones hold "awaiting lease" and never launch blind |
| Drone destroyed or out of range | Its lease expires after `SilentSeconds`, and its reservation after `ReserveSeconds` |
| Carrier reload | Job, lease table and queue are saved. Leases reload with `lastHeard = now` (grace period); reservations and the queue are dropped, and drones re-request |
| Drone reload | jobId, lease and mask are saved; the carrier recognises the lease id on the next status line |
| New job while one is active | It replaces the old one; see §3 |
| Lost `GO` push | The drone keeps retrying `R` every 10 s, and `Request` is idempotent |

## 8. Budgets

Estimates, measured by `check-size.fish` at each checkpoint: carrier ~6k → ~18k chars; drone ~68k
(after C32) → ~74k; console ~22k → ~25k. All stay under the 90,000 warning line. Dispatcher work runs
on carrier `Update10` and is O(drones + bays) per tick. Lease grants scan the free order, which is
O(holes) at worst and happens only on a request. Zero-GC rules apply to all per-tick paths; message
handling is a rare, user-driven path and may allocate.

## 9. Testing

- Unit tests for every pure piece: codecs, `LeaseTable`, `BayQueue`, `HoldSlots`, `LeaseCursor`,
  and `Dispatcher` (scripted message sequences in, expected replies out, including reload via
  Save/Load).
- `cases` tables for the lease and queue edge cases (idempotent re-requests, expiry ordering,
  reservation timeout, queue promotion).
- Drone smoke tests on the real `Program` with a faked IGC (the `ProgramSmokeTests` pattern): a lease
  arrives, the drone reports the lease on its status line, and the drone's SHARE emits a `job`.
- The carrier `Program` has no test harness (one PB project per test assembly); it stays a thin
  adapter and is verified in-game.

**In-game checklist (with Blae):**
- F1 Two drones, one carrier with two bays: SHARE from one drone, then FLEET START; both mine
  disjoint holes and the job completes.
- F2 Three drones, two bays: the third waits at slot 0 at the hold marker, then docks when a bay frees.
- F3 `JOB GPS:…` from the console: the carrier picks a lead, and the fleet mines the GPS site.
- F4 Destroy a drone mid-lease: after `SilentSeconds` its holes are re-leased and completed.
- F5 Reload the world mid-job: leases, queue and drones recover; no collision.
- F6 Carrier PB off mid-job: drones finish their trips and hold docked "awaiting lease"; PB back on →
  the fleet resumes.
- F7 Moving carrier (slice 2) with two drones: holding slots move with the carrier.
- F8 A drone with no recorded dock path docks using the shared path.

## 10. Implementation status

Amended 2026-10-02 after implementation: the integer number format, the `N` lease request, carrier-minted job
ids, and slot `(n + 1)` above are what the code does. The pure layer (codecs, status v2, `LeaseTable`,
`BayQueue`, `HoldSlots`, `LeaseCursor`, `Dispatcher` with Save/Load) is **done** (`a641030`, `fc34871`,
`2a7f22c`, 489 tests). It was written by Claude while proving the cards, and committed instead of being
re-implemented by the local model (Blae's decision). The remaining work and its cards are in
`docs/superpowers/plans/2026-10-02-slice3-fleet.md`.
