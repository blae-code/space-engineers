#!/usr/bin/env python3
"""Print the next card to hand to the local model, and the status of every card.

    python3 tools/next-card.py          # status table + the next card
Cards come from every `docs/superpowers/plans/*-cards/` deck (see tools/cards.py). A card is DONE when
its commit exists (the `Card: Cxx` trailer, or for older cards the commit that adds its implementation
file) and card-check passes on it (static checks only; tests are run by the hook at commit time and by checkpoint.py). Cards the plan assigns to Claude
are listed but never offered as "next".
"""
import subprocess

from cards import ROOT, card_files, card_id, plan_rows

rows = plan_rows()

status, nxt = {}, None
for card in card_files():
    cid = card_id(card)
    out = subprocess.run(["python3", str(ROOT / "tools/card-check.py"), cid, "--committed", "--no-test"],
                         cwd=ROOT, capture_output=True, text=True).stdout
    if "no commit adds" in out:
        status[cid] = "todo"
    else:
        status[cid] = "DONE" if out.rstrip().splitlines()[-1].startswith("RESULT: PASS") else "BROKEN"
    deps, who = rows.get(cid, ([], "?"))
    ready = all(status.get(d) == "DONE" for d in deps)
    print(cid.ljust(4) + "  " + status[cid].ljust(6) + "  " + who.ljust(13) + "  " + card.stem)
    if nxt is None and status[cid] != "DONE" and ready and who not in ("Claude", "superseded"):
        nxt = card
print()
print("next: " + (str(nxt.relative_to(ROOT)) if nxt else "none for the local model — bring it to Claude Code"))
