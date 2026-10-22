#!/usr/bin/env python3
"""Compatibility entry point for compiling the CH10 manual with explicit placements."""

import sys
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))

from chapter_indexer import main


if __name__ == "__main__":
    sys.argv[1:] = [
        "--compile",
        str(ROOT.parent / "manuals" / "chapters" / "10-mastodon-train.md"),
        "--model",
        str(ROOT / "data" / "mastodon.json"),
    ]
    sys.exit(main())
