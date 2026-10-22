---
name: Fleet MDK constraints
alwaysApply: true
description: Hard rules for Space Engineers programmable-block script code in this repo
---

# Space Engineers PB script — hard rules

You are working in an MDK2 repo. Script code is compiled by the game's C# 6 compiler inside a sandbox.
Breaking any rule below either fails the build or breaks the script in-game.

## Script code (Fleet.Engine/, Fleet.Flight/, Fleet.Drone.Miner/)
- Every file: `namespace IngameScript { public partial class Program { ... } }`. Types are **nested inside
  Program and `public`**. Never add another namespace.
- **C# 6 only.** Forbidden: `out var`, tuples, local functions, pattern matching, `is var`, throw
  expressions, discards `_`, `default` literal, digit separators, `in` parameters, `readonly struct`,
  ref returns.
- **Banned namespaces:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`,
  `System.Globalization`. **No LINQ** (`System.Linq`) anywhere in script code.
- **Zero-GC after construction:** inside `Update1`/`Update10`/`Update100`/`Main` paths there must be no
  `new`, no string concatenation or interpolation, no `ToString()` on numbers or enums, no LINQ.
  Preallocate in constructors and `.Clear()` to reuse. Rare user-driven paths (commands, config edits,
  save at FSM transitions, load) may allocate.
- Numbers go to text via `SbFormat` (appends into a `StringBuilder`); enum names come from
  `Names.State[]` / `Names.Reason[]`, never `enum.ToString()`.
- Block queries (adapters only) are filtered with `b.CubeGrid == Me.CubeGrid`.

## Tests (Fleet.Tests/)
- C# 7.3 allowed. Files start with `using static IngameScript.Program;`.
- **Test files given in a card are created verbatim and never edited to make a test pass.** If a test
  fails, the implementation is wrong — fix the implementation or stop and report.

## No card, no code
- **Never create or edit script code (`Fleet.Engine/`, `Fleet.Flight/`, `Fleet.Drone.Miner/`,
  `Fleet.Console/`, `Fleet.Carrier/`) without an attached task card.** The pre-commit hook refuses
  uncarded script code, so that work is thrown away.
- Asked for ideas, a roadmap or "what could we add"? Append **one line per idea** to `docs/ideas.md`
  and stop. Do not write plans, size estimates or code sketches into new files.
- Never estimate script size. Measure it: `fish tools/check-size.fish` (after a Release build).
- Never make backup copies (`*.backup`, `*.original`); git is the backup.
- Space Engineers facts you are not certain of (block APIs, game mechanics) are questions for Claude
  Code, not guesses. Vanilla SE has no weather, radiation or component wear.

## Working a card
- Create or modify **only** the files the card lists under `## Files`.
- Run each command in the card; if a result differs from its "Expected", stop and report — do not
  improvise a workaround.
- Implement interfaces exactly as the card specifies (names, signatures, visibility).
- Shell is **fish**: no heredocs, no `VAR=value cmd`, no `&&` chains needed — one command per step.
- The final step of every card is `python3 tools/card-check.py <card-id>`; it must print `RESULT: PASS`.
