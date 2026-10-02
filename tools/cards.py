"""Card discovery shared by card-check, next-card, checkpoint and the git hooks.

Cards live in any `docs/superpowers/plans/*-cards/` deck and are named `C<nn>-<slug>.md`, numbered
across decks (slice 1 is C01-C30; later decks continue from C31). The executor/dependency table rows
(`| C31 | files | deps | executor |`) may sit in any plan file under `docs/superpowers/plans/`.
"""
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
PLANS = ROOT / "docs/superpowers/plans"
CARD_ID = re.compile(r"C\d{2,3}")
# Script code: everything the in-game compiler sees. Tests and tools are not script code.
SCRIPT = re.compile(r"^Fleet\.(Engine|Flight|Drone\.Miner|Console|Carrier|Mothership)/.*\.cs$")


def card_id(path):
    return path.stem.split("-")[0]


def card_files():
    """Every card file in every deck, ordered by card number."""
    return sorted(PLANS.glob("*-cards/C*.md"), key=lambda p: int(card_id(p)[1:]))


def card_path(cid):
    hits = [p for p in card_files() if card_id(p) == cid.upper()]
    return hits[0] if hits else None


def section(md, title):
    m = re.search(r"^## " + re.escape(title) + r".*?$(.*?)(?=^## |\Z)", md, re.S | re.M)
    return m.group(1) if m else ""


def kind(md):
    """`Kind: logic | wire | cases | doc` on the card's header line; cards before C31 are logic."""
    m = re.search(r"\bKind:\s*(logic|wire|cases|doc)\b", md)
    return m.group(1) if m else "logic"


def files(md):
    return [f for f in re.findall(r"`([^`]+)`", section(md, "Files")) if "/" in f]


def owners():
    """file -> id of the highest-numbered card listing it (a later wire card owns a file it edits)."""
    own = {}
    for card in card_files():
        for f in files(card.read_text()):
            own[f] = card_id(card)
    return own


def plan_rows():
    """card id -> (deps, executor) from the card tables in every plan file."""
    rows = {}
    for plan in sorted(PLANS.glob("*.md")):
        for m in re.finditer(r"^\| (C\d{2,3}) \|[^|]*\| ([^|]*)\| ([^|]+)\|", plan.read_text(), re.M):
            rows[m.group(1)] = (CARD_ID.findall(m.group(2)), m.group(3).strip())
    return rows
