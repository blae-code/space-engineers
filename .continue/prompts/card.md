---
name: card
description: Execute one attached task card exactly (Fleet slice 1)
invokable: true
---

Execute the attached task card exactly.

1. Create or modify only the files the card lists under `## Files`.
2. Follow the steps in order. Run every command the card gives, exactly as written.
3. If a command's result differs from the card's "Expected", STOP and report the command and its output.
   Do not edit the test file, do not change the interface, do not invent a workaround.
4. Create the test file byte-for-byte from the card's `## Tests` block.
5. When the card's tests pass, run `python3 tools/card-check.py <card-id>` (e.g. `C01`) and show its full
   output. The card is done only when it prints `RESULT: PASS`. Read any WARN lines: an allocation token
   inside an `Update*`/`Main` path is a defect — fix it; one in a constructor or command/config path is fine.
6. Do NOT commit. Stop and report: files changed, the card-check output, and anything ambiguous in the card.
