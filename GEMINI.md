# GEMINI.md · rules for Gemini (Antigravity) on this repo

Three agents share this repo (Blae, 2026-10-17). Your lane is the token-heavy drafting; Claude reviews it.

| Agent | Does | Does not |
| --- | --- | --- |
| **Gemini** (you) | specs, plans, card decks, docs, research, bulk reading, bulk code drafts | edit gates or rule files; self-approve |
| **Claude Code** | reviews your work, fixes small gaps, owns `tools/` gates, tests, game-API adapters, in-game debugging | bulk drafting |
| **Qwen** (local, Continue) | executes reviewed task cards | uncarded code |

## 1. Read first, in this order
1. `CLAUDE(RIG).md`: scope, hard constraints C1–C7, how to work with Blae, register.
2. `CLAUDE.md`: build/test/deploy, card kinds, MDK landmines, PB sandbox rules.
3. `docs/rig/CHANGELOG.md`: Blae's decisions. **Decisions are canon; the guides in `reference/` are only guides.** Never argue a decision back toward a guide.

Where these files and this one disagree, they win; flag the conflict in the review queue.

## 2. Handoff (every task)
- Commit on the working branch. Every commit message ends with the trailer line `Agent: Gemini`.
- Then add one row to `docs/review-queue.md` with status `pending`. Claude sets it to `reviewed`, `fixed` or `bounced`.
- `bounced` = Claude left a reason and, for code, a failing test. Fix to that test; don't re-argue the reason in code.
- A card deck you draft is **not executed** (by Qwen or by you) until its queue row reads `reviewed`. One agent must not write both the tests and the code.

## 3. Never touch
`tools/` (the gates) · `.continue/` · `.agent/` · `CLAUDE.md` · `CLAUDE(RIG).md` · `GEMINI.md` · `mdk.local.ini` · `global.json`.
The `commit-msg` hook refuses an `Agent: Gemini` commit that touches these. Need a change there → write it up in the queue row's note.

**Never delete anything without asking Blae first**: tracked or untracked, including `.bak-*` backups, caches and "leftovers". Propose the list; delete only after he says yes.

## 4. Commits and gates
- Hooks are on (`core.hooksPath tools/hooks`). **Never `--no-verify`.**
- Script code changes only through a task card that passes `python3 tools/card-check.py <Cxx>`. One card per commit.
- Docs/specs commit freely (no gate), but still go in the queue.
- Before queueing code: `dotnet build <project> -c Release`, `dotnet test Fleet.Tests`, `fish tools/check-size.fish`. Paste the result lines in the queue note.

## 5. Script rules (PB sandbox, MDK2). A breach = bounce
- **C# 6** in script code (tests may use 7.3). All code in `namespace IngameScript`, inside `public partial class Program`, nested `public` types. No other namespaces.
- **Banned:** `System.Threading`, `System.IO`, `System.Reflection`, `System.Net`, `System.Globalization`, and **LINQ**.
- **Zero-GC** in `Main()` and `Update1/10/100`: no `new`, no string concat/interpolation, no `ToString()` on numbers or enums. Preallocate in `Program()`, `.Clear()` to reuse. Numbers → `SbFormat`; enum names → `Names.*` tables.
- **`minify=full` renames every identifier.** Never depend on a name at runtime: no `enum.ToString()`, `Enum.Parse/GetValues`, `nameof`. Storage, INI and IGC keys are string literals.
- Grid scoping on every block query: `b => b.CubeGrid == Me.CubeGrid`.
- Pure core, thin adapters: logic in pure tested classes; only `Io/` and `Subsystems/` touch blocks.
- C4: no safety function may depend on a PB. C2: vanilla + DLC blocks only (Defense Shields on the Rig is the one exception).

## 6. Writing for Blae
- Terse spec-sheet register: tables, bullets, `≈ → ·`, ASCII diagrams. **No em-dash-laced, AI-sounding narrative.**
- Mark every unverified Space Engineers number with `†` and add it to a Verify list. Never present a guessed game value as fact.
- Ask Blae before anything hard to reverse or that changes design intent (geometry, block policy, section boundaries, mods).
- No speculative roadmaps or features vanilla SE lacks (weather, radiation, wear). That work was reverted once already.
