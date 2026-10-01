#!/usr/bin/env python3
"""Mechanical gate for one task card: run after the local model finishes a card, before committing.

    python3 tools/card-check.py C01              # working tree: card's tests + static checks + scope
    python3 tools/card-check.py C01 --committed  # card already committed: scope read from its commit
    python3 tools/card-check.py C01 --no-test    # skip dotnet test (checkpoint runs the suite once)
    python3 tools/card-check.py C01 --staged     # scope = the git index (used by the pre-commit hook)

FAIL (exit 1): tests fail · the card's verbatim test file was edited · a file outside the card's
`## Files` list changed · a banned namespace in script code · a token the card's "Done when" forbids.
WARN (exit 0): allocation-shaped tokens in script code. They are legal on rare user-driven paths
(commands, config, load/save), so a human reads the listed lines and decides.
"""
import re
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
CARDS = ROOT / "docs/superpowers/plans/slice1-cards"

BANNED = re.compile(r"\bSystem\.(Linq|Threading|IO|Reflection|Net|Globalization)\b")
ALLOC = [("ToString(", r"\.ToString\("), ('$"', r'\$"'), ('+ "', r'\+\s*"'), ('" +', r'"\s*\+'),
         ("string.Format", r"\bstring\.Format\b"), ("string.Concat", r"\bstring\.Concat\b")]

results = []  # (level, name, detail)


def report(level, name, detail=""):
    results.append((level, name, detail))


def run(cmd):
    return subprocess.run(cmd, cwd=ROOT, capture_output=True, text=True)


def norm(text):
    """Compare code ignoring BOM, line endings, trailing whitespace and trailing blank lines."""
    lines = [l.rstrip() for l in text.lstrip("﻿").replace("\r\n", "\n").split("\n")]
    while lines and not lines[-1]:
        lines.pop()
    return lines


def section(md, title):
    m = re.search(r"^## " + re.escape(title) + r".*?$(.*?)(?=^## |\Z)", md, re.S | re.M)
    return m.group(1) if m else ""


def card_path(card_id):
    hits = sorted(CARDS.glob(card_id.upper() + "-*.md"))
    if not hits:
        sys.exit("no card matching " + card_id + " in " + str(CARDS))
    return hits[0]


def method_body(src, name):
    """Brace-matched body of the first method declaration called `name`, or None."""
    m = re.search(r"^[ \t]*(?:public|private|internal|protected|static)[^\n;=]*\b" + re.escape(name) + r"\s*\(", src, re.M)
    if not m:
        return None
    i = src.find("{", m.end())
    depth = 0
    for j in range(i, len(src)):
        depth += {"{": 1, "}": -1}.get(src[j], 0)
        if depth == 0:
            return src[i:j + 1]
    return None


def main():
    args = sys.argv[1:]
    if not args or args[0].startswith("-"):
        sys.exit(__doc__)
    card_file = card_path(args[0])
    committed, no_test, staged = "--committed" in args, "--no-test" in args, "--staged" in args
    md = card_file.read_text()
    files = [f for f in re.findall(r"`([^`]+)`", section(md, "Files")) if "/" in f]
    scripts = [f for f in files if not f.startswith("Fleet.Tests/")]
    print("card " + card_file.stem + " — files: " + ", ".join(files))

    # 1. tests
    if not no_test:
        m = re.search(r'--filter "(FullyQualifiedName~\w+)"', md)
        cmd = ["dotnet", "test", "Fleet.Tests"] + (["--filter", m.group(1)] if m else [])
        out = run(cmd)
        text = out.stdout + out.stderr
        summary = re.findall(r"^\s*(Passed!|Failed!).*$", text, re.M)
        tail = [l for l in text.splitlines() if re.search(r"error|Failed |Passed!|Failed!", l)][-12:]
        if out.returncode == 0 and summary:
            report("PASS", "tests", re.search(r"(Passed!|Failed!).*", text).group(0).strip())
        else:
            report("FAIL", "tests", "\n".join(tail) or text[-1500:])

    # 2. verbatim test file untouched
    tm = re.search(r"^## Tests — create `([^`]+)` verbatim\s*```csharp\n(.*?)```", md, re.S | re.M)
    if tm:
        path = ROOT / tm.group(1)
        if not path.exists():
            report("FAIL", "verbatim tests", tm.group(1) + " missing")
        elif norm(path.read_text()) != norm(tm.group(2)):
            want, got = norm(tm.group(2)), norm(path.read_text())
            first = next((i for i in range(min(len(want), len(got))) if want[i] != got[i]), min(len(want), len(got)))
            report("FAIL", "verbatim tests",
                   tm.group(1) + " differs from the card at line " + str(first + 1) +
                   " — tests must not be edited to pass")
        else:
            report("PASS", "verbatim tests", tm.group(1))

    # 3. scope: only the card's files changed
    if committed:
        log = run(["git", "log", "--diff-filter=A", "--format=%H", "--", scripts[0] if scripts else files[0]])
        shas = log.stdout.split()
        changed = run(["git", "show", "--name-only", "--format=", shas[-1]]).stdout.split() if shas else None
        where = "commit " + shas[-1][:8] if shas else ""
    elif staged:
        changed, where = run(["git", "diff", "--cached", "--name-only"]).stdout.split(), "staged"
    else:
        st = run(["git", "status", "--porcelain", "-uall"]).stdout.splitlines()
        changed, where = [l[3:].split(" -> ")[-1] for l in st], "working tree"
    if changed is None:
        report("FAIL", "scope", "no commit adds " + (scripts or files)[0])
    else:
        extra = [c for c in changed if c not in files]
        missing = [f for f in files if not (ROOT / f).exists()]
        if extra or missing:
            detail = (["outside the card: " + ", ".join(extra)] if extra else []) + \
                     (["not created: " + ", ".join(missing)] if missing else [])
            report("FAIL", "scope (" + where + ")", "; ".join(detail))
        else:
            report("PASS", "scope (" + where + ")", str(len(changed)) + " file(s), all listed")

    # 4. banned namespaces + allocation tokens in script code
    for f in scripts:
        p = ROOT / f
        if not p.exists():
            continue
        lines = p.read_text().splitlines()
        bad = [str(i + 1) for i, l in enumerate(lines) if BANNED.search(l) and not l.strip().startswith("//")]
        if bad:
            report("FAIL", "banned namespace", f + " line(s) " + ", ".join(bad))
        for label, rx in ALLOC:
            hits = [str(i + 1) for i, l in enumerate(lines) if re.search(rx, l) and not l.strip().startswith("//")]
            if hits:
                report("WARN", "alloc " + label, f + " line(s) " + ", ".join(hits) + " — OK only on rare paths")

    # 5. tokens the card's "Done when" forbids: "`X` contains no `a`, `b` or `c`"
    done = section(md, "Done when")
    for subj, toks in re.findall(r"`([^`]+)` contains no ((?:`[^`]+`[, ]*(?:or )?)+)", done):
        tokens = re.findall(r"`([^`]+)`", toks)
        if subj.endswith(".cs"):
            target = next((f for f in scripts if f.endswith("/" + subj)), None)
            body = (ROOT / target).read_text() if target and (ROOT / target).exists() else None
        else:
            src = "\n".join((ROOT / f).read_text() for f in scripts if (ROOT / f).exists())
            body = method_body(src, subj)
        if body is None:
            report("FAIL", "done-when " + subj, "could not locate " + subj)
            continue
        code = "\n".join(l for l in body.splitlines() if not l.strip().startswith("//"))
        found = [t for t in tokens if (re.search(r"\bnew\b", code) if t == "new" else t in code)]
        report("FAIL" if found else "PASS", "done-when " + subj,
               ("contains " + ", ".join(found)) if found else "none of " + ", ".join(tokens))

    width = max(len(n) for _, n, _ in results)
    for level, name, detail in results:
        head = level.ljust(4) + "  " + name.ljust(width)
        lines = detail.splitlines() or [""]
        print(head + "  " + lines[0])
        for l in lines[1:]:
            print(" " * (len(head) + 2) + l)
    fails = sum(1 for r in results if r[0] == "FAIL")
    warns = sum(1 for r in results if r[0] == "WARN")
    if not results:
        print("FAIL  no checks ran")  # a gate that checked nothing must not read as clean
        sys.exit(1)
    print("RESULT: " + ("FAIL" if fails else "PASS") + " — " + str(fails) + " fail, " + str(warns) + " warn")
    sys.exit(1 if fails else 0)


if __name__ == "__main__":
    main()
