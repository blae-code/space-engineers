#!/usr/bin/env python3
"""Print the next card to hand to the local model, and the status of every card.

    python3 tools/next-card.py          # status table + the next card
A card is DONE when a commit adds its implementation file and card-check passes on it (static checks
only; tests are run by the hook at commit time and by checkpoint.py). Cards the plan assigns to Claude
are listed but never offered as "next".
"""
import re
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
PLAN = ROOT / "docs/superpowers/plans/2026-09-29-slice1-core-miner.md"
CARDS = ROOT / "docs/superpowers/plans/slice1-cards"

executor = {m.group(1): m.group(2).strip() for m in
            re.finditer(r"^\| (C\d\d) \|[^|]*\|[^|]*\| ([^|]+)\|", PLAN.read_text(), re.M)}
deps = {m.group(1): re.findall(r"C\d\d", m.group(2)) for m in
        re.finditer(r"^\| (C\d\d) \|[^|]*\| ([^|]*)\|", PLAN.read_text(), re.M)}

status, nxt = {}, None
for card in sorted(CARDS.glob("C*.md")):
    cid = card.stem.split("-")[0]
    out = subprocess.run(["python3", str(ROOT / "tools/card-check.py"), cid, "--committed", "--no-test"],
                         cwd=ROOT, capture_output=True, text=True).stdout
    if "no commit adds" in out:
        status[cid] = "todo"
    else:
        status[cid] = "DONE" if out.rstrip().splitlines()[-1].startswith("RESULT: PASS") else "BROKEN"
    ready = all(status.get(d) == "DONE" for d in deps.get(cid, []) if d.startswith("C"))
    who = executor.get(cid, "?")
    print(cid.ljust(4) + "  " + status[cid].ljust(6) + "  " + who.ljust(13) + "  " + card.stem)
    if nxt is None and status[cid] != "DONE" and ready and who != "Claude":
        nxt = card
print()
print("next: " + (str(nxt.relative_to(ROOT)) if nxt else "none for the local model — bring it to Claude Code"))
