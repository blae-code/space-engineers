#!/usr/bin/env python3
"""Unified Chapter Indexer & Blueprint Compiler for Space Engineers 3D Studio.

Ingests chapter markdown instructions (e.g. docs/rig/chapters/*-instructions.md)
and compiles them into standardized, validated Voxel JSON datasets for WebGL
visualization, 2D deck floorplans, and machine-readable AI agent inspection.

Usage:
  python3 web/chapter_indexer.py docs/rig/chapters/mastodon-instructions.md
  python3 web/chapter_indexer.py --all
  python3 web/chapter_indexer.py --validate web/data/mastodon.json
  python3 web/chapter_indexer.py --stats
"""

import argparse
import glob
import json
import os
import re
import sys

# Standard 10-Slot Space Engineers Paint Palette
MASTER_PALETTE = [
    { "slot": 0, "name": "Citadel Charcoal", "hex": "#1A1D20", "rgb": [26, 29, 32], "role": "Heavy Armor Hull & Structural Longerons" },
    { "slot": 1, "name": "Structural Slate", "hex": "#42464D", "rgb": [66, 70, 77], "role": "Bulkheads, Deck Frames & Workshop Ribs" },
    { "slot": 2, "name": "Warning Amber", "hex": "#E06D14", "rgb": [224, 109, 20], "role": "Hatches, Outriggers, Hitch & Bumper Guides" },
    { "slot": 3, "name": "Tactical White", "hex": "#D4D8DD", "rgb": [212, 216, 221], "role": "Flight Deck Markings & Alignment Chevrons" },
    { "slot": 4, "name": "Processing Teal", "hex": "#1ABC9C", "rgb": [26, 188, 156], "role": "Conveyors, Sorters, Ejectors & Basic Refinery" },
    { "slot": 5, "name": "Habitat White", "hex": "#E8ECF0", "rgb": [232, 236, 240], "role": "Bridge Canopy Interior & Medical Station" },
    { "slot": 6, "name": "Machine Olive", "hex": "#4A5643", "rgb": [74, 86, 67], "role": "Large Assembler, Sorter Manifolds & Tool Stores" },
    { "slot": 7, "name": "Cryo Cyan", "hex": "#1E6B7B", "rgb": [30, 107, 123], "role": "Hydrogen Tanks & O2/H2 Decon Systems" },
    { "slot": 8, "name": "Mechanical Gunmetal", "hex": "#2B2E33", "rgb": [43, 46, 51], "role": "Hitch Rotors, Axle Mounts & Boom Pedestal" },
    { "slot": 9, "name": "Grate Steel", "hex": "#50555C", "rgb": [80, 85, 92], "role": "Jefferies Grating, Catwalks & Access Stairs" }
]

CHAPTER_CONFIGS = {
    "mastodon-instructions.md": {
        "id": "MAS-01",
        "chapter": 10,
        "name": "MASTODON-01 MKI Expeditionary Excavator & Mobile FOB",
        "file": "mastodon.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Ground Tier · Heavy Extraction",
        "bounds": { "min": [-5, 1, -26], "max": [5, 7, 12] },
        "stats": {
            "totalBlocks": 1126,
            "dryMassTonnes": 1151.8,
            "pcu": 10015,
            "dimensionsBlocks": [11, 42, 7],
            "dimensionsMeters": [27.5, 105.0, 17.5]
        },
        "palette": [
            { "slot": 0, "name": "Industrial Dark Gray", "hex": "#2B303A", "rgb": [43, 48, 58], "role": "Lower chassis box girders, wheel arch frames, belly skid plates" },
            { "slot": 1, "name": "Hazard Amber", "hex": "#E6A100", "rgb": [230, 161, 0], "role": "Roof Mule Gantry Pad (7x7 B), cradle arms, outrigger warning chevrons" },
            { "slot": 2, "name": "Heavy Rail Crimson", "hex": "#9E2A2B", "rgb": [158, 42, 43], "role": "Stern train dump bumper, rear alignment push-blocks, drawbar hitches" },
            { "slot": 3, "name": "Excavator Ochre", "hex": "#E67E22", "rgb": [230, 126, 34], "role": "45 m Long-Reach Articulated Boom, arm casings, laser tool head" },
            { "slot": 4, "name": "Processing Teal", "hex": "#1ABC9C", "rgb": [26, 188, 156], "role": "Central Ore Conveyor Spine, bow intake throat, manifold junctions" },
            { "slot": 5, "name": "Logistics Emerald", "hex": "#27AE60", "rgb": [39, 174, 96], "role": "Tractor ore buffer and Utility component storage holds" },
            { "slot": 6, "name": "High-Voltage Alert Red", "hex": "#C0392B", "rgb": [192, 57, 43], "role": "Tractor Power Vault (2x Small Nuclear Reactors, 2x Large Batteries)" },
            { "slot": 7, "name": "Fleet Command Navy", "hex": "#1B263B", "rgb": [27, 38, 59], "role": "Forward Armored C2 Bridge exterior shell, operator consoles" },
            { "slot": 8, "name": "Scout Flight White", "hex": "#ECF0F1", "rgb": [236, 240, 241], "role": "Forward twin Kestrel scout drone landing pads (P1, S1) on Utility roof" },
            { "slot": 9, "name": "High-Viz Safety Yellow", "hex": "#F1C40F", "rgb": [241, 196, 15], "role": "Exterior catwalks, maintenance ladders, corner outrigger foot pads" }
        ]
    },
    "orca-instructions.md": {
        "id": "ORC-01",
        "chapter": 1,
        "name": "ORCA-01 Heavy Mining Carrier",
        "file": "orca.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Air Tier · Fast / Rare",
        "bounds": { "min": [-8, 1, -9], "max": [8, 7, 10] },
        "stats": {
            "totalBlocks": 653,
            "dryMassTonnes": 764.0,
            "pcu": 4500,
            "dimensionsBlocks": [17, 19, 7],
            "dimensionsMeters": [42.5, 47.5, 17.5]
        }
    },
    "remora-instructions.md": {
        "id": "REM-01",
        "chapter": 2,
        "name": "REMORA-01 Vertical Shaft Driller",
        "file": "remora.json",
        "grid": "Small",
        "blockMeters": 0.5,
        "tier": "Air Tier · Vertical Miner",
        "bounds": { "min": [-5, 1, -5], "max": [5, 15, 5] },
        "stats": {
            "totalBlocks": 133,
            "dryMassTonnes": 48.5,
            "pcu": 1774,
            "dimensionsBlocks": [11, 11, 15],
            "dimensionsMeters": [5.5, 5.5, 7.5]
        }
    },
    "mule-instructions.md": {
        "id": "MUL-01",
        "chapter": 3,
        "name": "MULE-01 Heavy Freight Shuttle",
        "file": "mule.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Air Tier · Logistics Ferry",
        "bounds": { "min": [-3, 1, -3], "max": [3, 5, 3] },
        "stats": {
            "totalBlocks": 142,
            "dryMassTonnes": 128.0,
            "pcu": 1850,
            "dimensionsBlocks": [7, 7, 5],
            "dimensionsMeters": [17.5, 17.5, 12.5]
        }
    },
    "ground-base-instructions.md": {
        "id": "GND-01",
        "chapter": 4,
        "name": "Ground Base, Refinery Complex & Fleet C2",
        "file": "ground-base.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Surface Industrial Core",
        "bounds": { "min": [-17, 1, -22], "max": [17, 12, 22] },
        "stats": {
            "totalBlocks": 1420,
            "dryMassTonnes": 2840.0,
            "pcu": 14200,
            "dimensionsBlocks": [35, 45, 12],
            "dimensionsMeters": [87.5, 112.5, 30.0]
        }
    },
    "extruder-ring-instructions.md": {
        "id": "EXT-01",
        "chapter": 5,
        "name": "Rig Linear Extruder Ring Shipyard",
        "file": "extruder-ring.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Surface Shipyard",
        "bounds": { "min": [-12, 1, -25], "max": [12, 14, 25] },
        "stats": {
            "totalBlocks": 860,
            "dryMassTonnes": 1850.0,
            "pcu": 8900,
            "dimensionsBlocks": [24, 50, 14],
            "dimensionsMeters": [60.0, 125.0, 35.0]
        }
    },
    "floating-rig-instructions.md": {
        "id": "RIG-01",
        "chapter": 6,
        "name": "Floating Rig Structure (25 kt Fortress)",
        "file": "floating-rig.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Fortress Tier · Aerial Fleet Hub",
        "bounds": { "min": [-18, 1, -18], "max": [18, 28, 18] },
        "stats": {
            "totalBlocks": 4980,
            "dryMassTonnes": 25000.0,
            "pcu": 29500,
            "dimensionsBlocks": [36, 36, 28],
            "dimensionsMeters": [90.0, 90.0, 70.0]
        }
    },
    "caterpillar-instructions.md": {
        "id": "CAT-01",
        "chapter": 11,
        "name": "CATERPILLAR-01 Bulk Land Train",
        "file": "caterpillar.json",
        "grid": "Large",
        "blockMeters": 2.5,
        "tier": "Ground Tier · Bulk Mass Hauler",
        "bounds": { "min": [-3, 1, -21], "max": [3, 5, 21] },
        "stats": {
            "totalBlocks": 404,
            "dryMassTonnes": 368.5,
            "pcu": 3950,
            "dimensionsBlocks": [7, 42, 5],
            "dimensionsMeters": [17.5, 105.0, 12.5]
        }
    },
    "kestrel-instructions.md": {
        "id": "KES-01",
        "chapter": 12,
        "name": "KESTREL-01 Prospector Scout Drone",
        "file": "kestrel.json",
        "grid": "Small",
        "blockMeters": 0.5,
        "tier": "Air Tier · Geological Recon",
        "bounds": { "min": [-3, 1, -4], "max": [3, 5, 4] },
        "stats": {
            "totalBlocks": 68,
            "dryMassTonnes": 4.8,
            "pcu": 850,
            "dimensionsBlocks": [7, 9, 5],
            "dimensionsMeters": [3.5, 4.5, 2.5]
        }
    },
    "albatross-instructions.md": {
        "id": "ALB-01",
        "chapter": 13,
        "name": "ALBATROSS-01 Utility & Field Repair Craft",
        "file": "albatross.json",
        "grid": "Small",
        "blockMeters": 0.5,
        "tier": "Air Tier · Engineering & Salvage",
        "bounds": { "min": [-4, 1, -5], "max": [4, 5, 5] },
        "stats": {
            "totalBlocks": 84,
            "dryMassTonnes": 8.5,
            "pcu": 1120,
            "dimensionsBlocks": [9, 11, 5],
            "dimensionsMeters": [4.5, 5.5, 2.5]
        }
    }
}

def parse_parts_tray(tray_text):
    """Parses markdown table into parts tray entries."""
    parts = []
    lines = [l.strip() for l in tray_text.strip().split("\n") if l.strip().startswith("|")]
    if len(lines) >= 3:
        for line in lines[2:]:
            cols = [c.strip() for c in line.split("|")[1:-1]]
            if len(cols) >= 3:
                b_type = cols[0]
                qty_str = cols[1]
                slot_str = cols[2] if len(cols) > 2 else "Slot 0"
                mass_pcu = cols[3] if len(cols) > 3 else ""
                note = cols[4] if len(cols) > 4 else ""
                try:
                    qty = int(re.search(r"\d+", qty_str).group())
                except:
                    qty = 1
                slot = 0
                slot_match = re.search(r"Slot\s+(\d+)", slot_str)
                if slot_match:
                    slot = int(slot_match.group(1))
                parts.append({
                    "type": b_type,
                    "qty": qty,
                    "slot": slot,
                    "massPcu": mass_pcu,
                    "note": note
                })
    return parts

def parse_checks(step_text):
    """Extracts verification checklist items."""
    checks = []
    m_check = re.search(r"\*\*✓ Check:\*\*\s*(.+?)(?=\n\n|\n---|####|\Z)", step_text, re.DOTALL)
    if m_check:
        checks.append(m_check.group(1).strip().replace("\n", " "))
    gates = re.findall(r"\*\s*\[[ x]\]\s*(.+)", step_text)
    for g in gates:
        checks.append(g.strip())
    return checks

def parse_instructions(step_text):
    """Extracts step-by-step assembly instructions."""
    m = re.search(r"#### Placement Instructions\s*\n([\s\S]+?)(?=\n####|\n\*\*✓|\n---|```|\Z)", step_text)
    if m:
        return [l.strip() for l in m.group(1).strip().split("\n") if l.strip()]
    lines = [l.strip() for l in re.findall(r"^\d+\.\s+(.+)$", step_text, re.MULTILINE)]
    return lines if lines else ["Assemble section per spatial coordinate triad."]

def parse_terminal_config(step_text):
    """Extracts terminal block configuration blocks."""
    m = re.search(r"#### Terminal Configuration & Sliders\s*\n([\s\S]+?)(?=\n####|\n\*\*✓|\n---|```|\Z)", step_text)
    if m:
        return m.group(1).strip()
    return ""

def classify_subsystem(text):
    """Classifies subsystem layer from text content."""
    t = text.lower()
    if any(k in t for k in ["boom", "tool head", "drill head", "welder head", "grapple"]):
        return "boom"
    if any(k in t for k in ["flight deck", "pad", "hangar", "apron", "runway"]):
        return "flightdeck"
    if any(k in t for k in ["cockpit", "bridge", "helm", "avionics", "pb", "programmable block", "remote control"]):
        return "avionics"
    if any(k in t for k in ["reactor", "battery", "engine", "hydrogen tank", "o2/h2", "tank h2", "power"]):
        return "power"
    if any(k in t for k in ["wheel", "suspension", "outrigger", "hitch", "dpu", "thruster", "atmo", "ion"]):
        return "propulsion"
    if any(k in t for k in ["conveyor", "sorter", "ejector", "cargo", "refinery", "assembler", "pipe"]):
        return "conveyor"
    return "structure"

def determine_level(title):
    """Extracts Level string (e.g. L1, L2, L1-L2) from title."""
    m = re.search(r"\((L[0-9]+(?:-[0-9A-Za-z]+)?)\)", title)
    return m.group(1) if m else "L2"

def compile_chapter(md_path, out_json_path=None, custom_cfg=None):
    """Compiles a single markdown chapter instructions file into Voxel JSON."""
    basename = os.path.basename(md_path)
    cfg = custom_cfg or CHAPTER_CONFIGS.get(basename)

    if not cfg:
        # Generate default config derived from filename
        ch_num = 1
        m_num = re.search(r"\d+", basename)
        if m_num: ch_num = int(m_num.group())
        short_id = basename.replace("-instructions.md", "").upper()[:4] + "-01"
        cfg = {
            "id": short_id,
            "chapter": ch_num,
            "name": basename.replace("-instructions.md", "").replace("-", " ").title(),
            "file": basename.replace("-instructions.md", ".json"),
            "grid": "Large",
            "blockMeters": 2.5,
            "tier": "Logistics Tier",
            "bounds": { "min": [-5, 1, -10], "max": [5, 6, 10] },
            "stats": { "totalBlocks": 100, "dryMassTonnes": 120.0, "pcu": 1000, "dimensionsBlocks": [11, 21, 6] }
        }

    with open(md_path, "r", encoding="utf-8") as f:
        content = f.read()

    # Match Step headings: ### Step X · Title or ### Step X: Title or ### Section X · Title
    step_pattern = r"\n### (?:Step\s+([0-9A-Za-z\.]+)|Phase\s+([0-9\-]+)|Section\s+([0-9\.]+))(?:\s*[:·]\s*)([^\n]+)\n"
    matches = list(re.finditer(step_pattern, content))

    steps = []
    min_x, max_x = cfg["bounds"]["min"][0], cfg["bounds"]["max"][0]
    min_z, max_z = cfg["bounds"]["min"][2], cfg["bounds"]["max"][2]

    # For Mastodon (Chapter 10), import the exact 47-step block coordinates if available
    is_mastodon = (cfg["id"] == "MAS-01")
    mastodon_step_blocks = None
    if is_mastodon and os.path.exists("web/compile_mastodon.py"):
        try:
            import importlib.util
            spec = importlib.util.spec_from_file_location("compile_mastodon", "web/compile_mastodon.py")
            mod = importlib.util.module_from_spec(spec)
            spec.loader.exec_module(mod)
            mastodon_step_blocks = mod.generate_blocks_for_step
        except Exception:
            mastodon_step_blocks = None

    for i, m in enumerate(matches):
        step_id = m.group(1) or m.group(2) or m.group(3) or str(i + 1)
        title = m.group(4).strip()
        start_idx = m.end()
        end_idx = matches[i + 1].start() if i + 1 < len(matches) else len(content)
        body = content[start_idx:end_idx]

        level_str = determine_level(title)
        subsystem = classify_subsystem(title + " " + body)
        if is_mastodon:
            try:
                num = float(re.search(r"10\.(\d+)", step_id).group(1))
                if num <= 5: phase_num = "10-1"
                elif num <= 14: phase_num = "10-2"
                elif num <= 20: phase_num = "10-3"
                elif num <= 28: phase_num = "10-4"
                elif num <= 36: phase_num = "10-5"
                else: phase_num = "10-6"
            except Exception:
                phase_num = f"{cfg['chapter']}-{1 + (i // 8)}"
        else:
            phase_num = f"{cfg['chapter']}-{1 + (i // 8)}"
        instructions = parse_instructions(body)
        checks = parse_checks(body)
        term_config = parse_terminal_config(body)

        # Parts tray
        tray_match = re.search(r"####\s*(?:Pre-Step\s+)?(?:Step\s+[0-9A-Za-z\.]+\s+)?Parts Tray\s*\n([\s\S]+?)(?=\n####|\n\*\*✓|\n---|```|\Z)", body)
        parts_tray = parse_parts_tray(tray_match.group(1)) if tray_match else []
        if not parts_tray:
            if "macro" in title.lower():
                parts_tray = [{ "type": "Software / Kinematics Macro", "qty": 1, "slot": 0, "massPcu": "0.0 t / 25 PCU", "note": "Kinematic supervisory configuration" }]
            else:
                parts_tray = [{ "type": "Armor Full Block", "qty": 4, "slot": 0, "massPcu": "1.2 t / 25 PCU", "note": "Primary section framing" }]

        # Blocks generation
        if is_mastodon and mastodon_step_blocks:
            norm_id = f"Step 10.{step_id.replace('Step ', '').replace('10.', '')}"
            blocks = mastodon_step_blocks(norm_id, subsystem, level_str)
        else:
            # Deterministic procedural coordinates for step based on section bounds
            blocks = []
            z_pos = int(min_z + (max_z - min_z) * (i / max(1, len(matches) - 1)))
            lvl_num = int(re.search(r"\d+", level_str).group()) if re.search(r"\d+", level_str) else 2
            palette_slot = 0 if subsystem == "structure" else 4 if subsystem == "conveyor" else 2 if subsystem == "power" else 8
            shape = "conveyor" if subsystem == "conveyor" else "cube"
            subtype = "LargeBlockArmorBlock" if cfg["grid"] == "Large" else "SmallBlockArmorBlock"

            for x in range(max(min_x, -2), min(max_x + 1, 3)):
                blocks.append({
                    "pos": [x, lvl_num, z_pos],
                    "shape": shape,
                    "subtype": subtype,
                    "paletteSlot": palette_slot,
                    "subsystem": subsystem,
                    "name": f"{cfg['id']} {title[:20]} [{x}, {lvl_num}, {z_pos}]"
                })

        step_dict = {
            "step": step_id,
            "phase": phase_num,
            "level": level_str,
            "title": title,
            "subsystem": subsystem,
            "partsTray": parts_tray,
            "instructions": instructions,
            "checks": checks if checks else ["Verify physical clearances and conveyor line continuity."],
            "blocks": blocks
        }
        if term_config:
            step_dict["terminalConfig"] = term_config

        steps.append(step_dict)

    model = {
        "$schema": "https://json-schema.org/draft/2020-12/schema",
        "id": cfg["id"],
        "name": cfg["name"],
        "chapter": cfg["chapter"],
        "gridSize": cfg["grid"],
        "blockMeters": cfg["blockMeters"],
        "stats": cfg["stats"],
        "bounds": cfg["bounds"],
        "palette": cfg.get("palette", MASTER_PALETTE),
        "steps": steps
    }

    out_file = out_json_path or os.path.join("web", "data", cfg["file"])
    os.makedirs(os.path.dirname(out_file), exist_ok=True)
    with open(out_file, "w", encoding="utf-8") as f:
        json.dump(model, f, indent=2)

    print(f"Successfully compiled {cfg['id']} ({len(steps)} steps) -> {out_file}")
    return model

def validate_model(json_path):
    """Validates a model JSON against schema invariants."""
    if not os.path.exists(json_path):
        print(f"ERROR: File not found: {json_path}")
        return False

    with open(json_path, "r", encoding="utf-8") as f:
        try:
            m = json.load(f)
        except Exception as e:
            print(f"ERROR: Invalid JSON in {json_path}: {e}")
            return False

    required_keys = ["$schema", "id", "name", "chapter", "gridSize", "blockMeters", "stats", "bounds", "palette", "steps"]
    missing = [k for k in required_keys if k not in m]
    if missing:
        print(f"FAIL: Missing top-level keys: {missing}")
        return False

    bounds = m["bounds"]
    min_b = bounds["min"]
    max_b = bounds["max"]

    total_blocks = 0
    oob_blocks = 0
    invalid_palettes = 0

    valid_subsystems = {"structure", "conveyor", "power", "propulsion", "avionics", "flightdeck", "boom"}

    for s_idx, step in enumerate(m["steps"]):
        for b in step["blocks"]:
            total_blocks += 1
            pos = b["pos"]
            if pos[0] < min_b[0] or pos[0] > max_b[0] or \
               pos[1] < min_b[1] or pos[1] > max_b[1] or \
               pos[2] < min_b[2] or pos[2] > max_b[2]:
                oob_blocks += 1

            slot = b.get("paletteSlot", 0)
            if slot < 0 or slot >= len(m["palette"]):
                invalid_palettes += 1

            sub = b.get("subsystem", "structure")
            if sub not in valid_subsystems:
                print(f"WARNING: Unknown subsystem '{sub}' at step {step['step']}")

    print(f"Validation Result · {m['id']} ({m['name']}):")
    print(f"  · Grid Metric: {m['gridSize']} ({m['blockMeters']} m)")
    print(f"  · Steps: {len(m['steps'])}")
    print(f"  · Voxel Blocks: {total_blocks}")
    print(f"  · Out-of-Bounds Blocks: {oob_blocks}")
    print(f"  · Palette Violations: {invalid_palettes}")

    if oob_blocks > 0 or invalid_palettes > 0:
        print("STATUS: FAILED invariants check.")
        return False

    print("STATUS: PASSED schema and spatial invariants.")
    return True

def compile_all():
    """Compiles all registered fleet chapters and updates master registry."""
    registry = []
    print("Compiling all chapters in docs/rig/chapters/...")

    for fname, cfg in CHAPTER_CONFIGS.items():
        md_path = os.path.join("docs", "rig", "chapters", fname)
        if os.path.exists(md_path):
            m = compile_chapter(md_path, custom_cfg=cfg)
            registry.append({
                "id": m["id"],
                "chapter": m["chapter"],
                "name": m["name"],
                "file": cfg["file"],
                "grid": m["gridSize"],
                "tier": cfg["tier"],
                "blocks": m["stats"]["totalBlocks"],
                "mass": f"{m['stats']['dryMassTonnes']} t",
                "pcu": m["stats"]["pcu"]
            })
        else:
            print(f"Notice: Chapter file {md_path} not found on disk, skipping.")

    registry.sort(key=lambda x: x["chapter"])
    reg_path = os.path.join("web", "data", "chapters.json")
    with open(reg_path, "w", encoding="utf-8") as f:
        json.dump(registry, f, indent=2)

    print(f"Master fleet index generated with {len(registry)} models in {reg_path}")

def print_stats():
    """Prints tabular overview of all compiled fleet models."""
    reg_path = os.path.join("web", "data", "chapters.json")
    if not os.path.exists(reg_path):
        print("No registry found. Run with --all first.")
        return

    with open(reg_path, "r", encoding="utf-8") as f:
        registry = json.load(f)

    print(f"{'CH':<4} | {'ID':<8} | {'NAME':<42} | {'GRID':<6} | {'BLOCKS':<7} | {'MASS':<10} | {'PCU':<6}")
    print("-" * 92)
    for r in registry:
        print(f"{r['chapter']:<4} | {r['id']:<8} | {r['name'][:40]:<42} | {r['grid']:<6} | {r['blocks']:<7} | {r['mass']:<10} | {r['pcu']:<6}")

def main():
    parser = argparse.ArgumentParser(description="Space Engineers 3D Studio Chapter Indexer & Compiler")
    parser.add_argument("path", nargs="?", help="Path to markdown instructions file (e.g. docs/rig/chapters/mastodon-instructions.md)")
    parser.add_argument("--out", "-o", help="Custom output JSON path")
    parser.add_argument("--all", "-a", action="store_true", help="Compile all fleet chapters and update master registry")
    parser.add_argument("--validate", "-v", help="Validate a compiled JSON model against schema")
    parser.add_argument("--stats", "-s", action="store_true", help="Print summary table of compiled fleet models")

    args = parser.parse_args()

    if args.stats:
        print_stats()
        return

    if args.validate:
        valid = validate_model(args.validate)
        sys.exit(0 if valid else 1)

    if args.all:
        compile_all()
        return

    if args.path:
        compile_chapter(args.path, out_json_path=args.out)
        return

    parser.print_help()

if __name__ == "__main__":
    main()
