---
name: card
description: Execute ONE attached task card exactly (Fleet slice 1)
invokable: true
---

Execute the ONE attached task card exactly. This chat is for that card only.

Hard limits:
- Work on the attached card ONLY. Never start, plan, or "get ahead on" another card, even if this one
  finishes early or the user's message mentions several. When this card is done, STOP.
- Create or modify only the files the card lists under `## Files`. Never touch `Program.cs`, any other
  card's files, `tools/`, `docs/`, or project files. If the card seems to need another file, STOP and say so.
- Never claim a result you did not see in a command's output in THIS chat.

The card's header names its `Kind`:
- `logic`: create a new file to a given interface. The tests are verbatim.
- `wire`: edit existing files at the anchors the card names, and change nothing else in them. The
  tests are verbatim.
- `cases`: YOU write the test file. Write one `[TestCase]` (or `[Test]`) per row of the card's
  `## Cases` table, with the row id (`K1`, `K2`, …) in its `TestName` or a comment on its line.
  Every row becomes a test, with exactly the expected value in the table.
- `doc`: edit only the listed docs, as the bullets say. There are no tests; run card-check.
- `## Hot paths` lists methods that run every tick. Inside them, `card-check` FAILs any `ToString(`,
  `$"`, string `+` or `string.Format`. Use `SbFormat` and preallocated fields.

Steps:
1. Create the test file byte-for-byte from the card's `## Tests` block (kind `cases`: from the table).
   Never edit a verbatim test file afterwards.
2. Follow the card's steps in order, running every command exactly as written.
3. If a command's result differs from the card's "Expected", STOP and report the command and its output.
   Do not change the interface, the tests, or the build to get round it.
4. If the implementation file already exists (a previous failed attempt), read it, then fix it — the
   failing test names and messages tell you what is wrong.
5. When the card's tests pass, run `python3 tools/card-check.py <card-id>` (e.g. `C05`).
   Read any WARN lines: an allocation token inside an `Update*`/`Main` path is a defect — fix it and
   re-run; one in a constructor or a command/config/save path is fine.
6. Do NOT commit (a pre-commit hook re-runs the check; Blae commits).

Your final message must contain, in this order:
- the files you created or changed;
- the COMPLETE output of the last `card-check` run, pasted verbatim (not summarised);
- anything in the card you found ambiguous.
If that output does not end in `RESULT: PASS`, say plainly that the card is NOT done.
