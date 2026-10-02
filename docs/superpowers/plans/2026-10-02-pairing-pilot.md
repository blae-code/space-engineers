# Pairing pilot — mining stats as cards (C31–C32)

Proves the 2026-10-02 workflow upgrades end to end (card kinds, hot-path gate, `Card:` trailer,
multi-deck tools) on a small, useful feature. It replaces the uncarded Mining Stats change that was
reverted in `a680677`. Workflow rules: repo `CLAUDE.md`, "Division of labour".

Both cards were validated before hand-out: a reference implementation passed all their tests (suite
366/366), the wire card's red step was measured (2 failed, 1 passed), and the drone script grew
~1.1k chars. The reference is not committed; the local model writes the code.

| Card | File(s) | Depends on | Executor |
|---|---|---|---|
| C31 | `Fleet.Drone.Miner/Mining/MiningStats.cs` | — | local |
| C32 | `Fleet.Drone.Miner/Subsystems/MinerSubsystem.cs`, `UiSubsystem.cs` | C31 | local |

Hand-out: `python3 tools/next-card.py`, then in Continue (Agent mode) `/card` with the card attached.
After C32: `dotnet build Fleet.Drone.Miner -c Release`, `fish tools/snapshot.fish`, then
`python3 tools/checkpoint.py C31-C32`, and check in-game that the drone LCD shows a `trips …` line
after its first unload.

## Checkpoint log
