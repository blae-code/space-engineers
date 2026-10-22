#!/usr/bin/env python3
"""Compatibility entry point for validating models and rebuilding the chapter registry."""

import sys
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))

from chapter_indexer import main


if __name__ == "__main__":
    sys.argv[1:] = ["--all"]
    sys.exit(main())
