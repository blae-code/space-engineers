#!/usr/bin/env python3
"""Mechanical half of a checkpoint: full test suite, Release build, size gate, and card-check on every
card of the milestone. Run it before bringing a checkpoint to Claude and paste the output.

    python3 tools/checkpoint.py A      # milestones: A C01-C09 · B C10-C18 · C C19-C24 · D C25-C30
"""
import re
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
MILESTONES = {"A": range(1, 10), "B": range(10, 19), "C": range(19, 25), "D": range(25, 31)}


def run(cmd):
    out = subprocess.run(cmd, cwd=ROOT, capture_output=True, text=True)
    return out.returncode, out.stdout + out.stderr


def main():
    if len(sys.argv) != 2 or sys.argv[1].upper() not in MILESTONES:
        sys.exit(__doc__)
    ms = sys.argv[1].upper()
    rows = []

    code, text = run(["dotnet", "test", "Fleet.Tests"])
    m = re.search(r"(Passed!|Failed!).*", text)
    rows.append(("PASS" if code == 0 and m else "FAIL", "full test suite", m.group(0).strip() if m else text[-800:]))

    code, text = run(["dotnet", "build", "Fleet.Drone.Miner", "-c", "Release"])
    warn = re.search(r"(\d+) Warning\(s\)", text)
    ok = code == 0 and "successfully deployed" in text
    rows.append(("PASS" if ok else "FAIL", "release build + deploy",
                 (warn.group(0) if warn else "") if ok else "\n".join(text.splitlines()[-15:])))

    code, text = run(["fish", "tools/check-size.fish"])
    rows.append(("FAIL" if code else ("WARN" if text.startswith("WARN") else "PASS"), "size gate", text.strip()))

    for n in MILESTONES[ms]:
        cid = "C%02d" % n
        code, text = run(["python3", "tools/card-check.py", cid, "--committed", "--no-test"])
        detail = [l for l in text.splitlines() if l.startswith(("FAIL", "WARN"))]
        rows.append(("FAIL" if code else ("WARN" if detail else "PASS"), "card " + cid, "\n".join(detail)))

    print("CHECKPOINT " + ms)
    for level, name, detail in rows:
        lines = detail.splitlines() or [""]
        print(level.ljust(4) + "  " + name.ljust(24) + "  " + lines[0])
        for l in lines[1:]:
            print(" " * 32 + l)
    fails = sum(1 for r in rows if r[0] == "FAIL")
    print("RESULT: " + ("FAIL" if fails else "PASS") + " — " + str(fails) + " fail(s)")
    sys.exit(1 if fails else 0)


if __name__ == "__main__":
    main()
