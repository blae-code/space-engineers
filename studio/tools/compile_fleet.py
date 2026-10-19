#!/usr/bin/env python3
"""Compiler for the complete fleet ecosystem into web/data/ JSON files.
"""
import glob, json, os, re

CHAPTER_CONFIGS = [
    {
        "id": "ORC-01",
        "chapter": 1,
        "name": "ORCA-01 Heavy Mining Carrier",
        "file": "orca.json",
        "src": "manuals/chapters/01-orca-carrier.md",
        "grid": "Large",
        "tier": "Air Tier · Fast / Rare",
        "stats": { "totalBlocks": 653, "dryMassTonnes": 764.0, "pcu": 4500, "dimensionsBlocks": [17, 19, 7] },
        "bounds": { "min": [-8, 1, -9], "max": [8, 7, 10] },
        "stepPattern": r"### Step 1\.(\d+) · ([^\n]+)"
    },
    {
        "id": "REM-01",
        "chapter": 2,
        "name": "REMORA-01 Vertical Shaft Driller",
        "file": "remora.json",
        "src": "manuals/chapters/02-remora-driller.md",
        "grid": "Small",
        "tier": "Air Tier · Vertical Miner",
        "stats": { "totalBlocks": 133, "dryMassTonnes": 48.5, "pcu": 1774, "dimensionsBlocks": [11, 11, 15] },
        "bounds": { "min": [-5, 1, -5], "max": [5, 15, 5] },
        "stepPattern": r"### Step 2\.(\d+) · ([^\n]+)"
    },
    {
        "id": "MUL-01",
        "chapter": 3,
        "name": "MULE-01 Heavy Freight Shuttle",
        "file": "mule.json",
        "src": "manuals/chapters/03-mule-shuttle.md",
        "grid": "Large",
        "tier": "Air Tier · Logistics Ferry",
        "stats": { "totalBlocks": 142, "dryMassTonnes": 128.0, "pcu": 1850, "dimensionsBlocks": [7, 7, 5] },
        "bounds": { "min": [-3, 1, -3], "max": [3, 5, 3] },
        "stepPattern": r"### Step 3\.(\d+) · ([^\n]+)"
    },
    {
        "id": "GND-01",
        "chapter": 4,
        "name": "Ground Base, Refinery Complex & Fleet C2",
        "file": "ground-base.json",
        "src": "manuals/chapters/04-ground-base.md",
        "grid": "Large",
        "tier": "Surface Industrial Core",
        "stats": { "totalBlocks": 1420, "dryMassTonnes": 2840.0, "pcu": 14200, "dimensionsBlocks": [35, 45, 12] },
        "bounds": { "min": [-17, 1, -22], "max": [17, 12, 22] },
        "stepPattern": r"### Step 4\.(\d+) · ([^\n]+)"
    },
    {
        "id": "EXT-01",
        "chapter": 5,
        "name": "Rig Linear Extruder Ring Shipyard",
        "file": "extruder-ring.json",
        "src": "manuals/chapters/05-extruder-ring.md",
        "grid": "Large",
        "tier": "Surface Shipyard",
        "stats": { "totalBlocks": 860, "dryMassTonnes": 1850.0, "pcu": 8900, "dimensionsBlocks": [24, 50, 14] },
        "bounds": { "min": [-12, 1, -25], "max": [12, 14, 25] },
        "stepPattern": r"### Step 5\.(\d+) · ([^\n]+)"
    },
    {
        "id": "CAT-01",
        "chapter": 11,
        "name": "CATERPILLAR-01 Bulk Land Train",
        "file": "caterpillar.json",
        "src": "manuals/chapters/11-caterpillar-train.md",
        "grid": "Large",
        "tier": "Ground Tier · Bulk Mass Hauler",
        "stats": { "totalBlocks": 404, "dryMassTonnes": 368.5, "pcu": 3950, "dimensionsBlocks": [7, 42, 5] },
        "bounds": { "min": [-3, 1, -21], "max": [3, 5, 21] },
        "stepPattern": r"### Step 11\.(\d+) · ([^\n]+)"
    },
    {
        "id": "KES-01",
        "chapter": 12,
        "name": "KESTREL-01 Prospector Scout Drone",
        "file": "kestrel.json",
        "src": "manuals/chapters/12-kestrel-scout.md",
        "grid": "Small",
        "tier": "Air Tier · Geological Recon",
        "stats": { "totalBlocks": 68, "dryMassTonnes": 4.8, "pcu": 850, "dimensionsBlocks": [7, 9, 5] },
        "bounds": { "min": [-3, 1, -4], "max": [3, 5, 4] },
        "stepPattern": r"### Step 12\.(\d+) · ([^\n]+)"
    },
    {
        "id": "ALB-01",
        "chapter": 13,
        "name": "ALBATROSS-01 Utility & Field Repair Craft",
        "file": "albatross.json",
        "src": "manuals/chapters/13-albatross-utility.md",
        "grid": "Small",
        "tier": "Air Tier · Engineering & Salvage",
        "stats": { "totalBlocks": 84, "dryMassTonnes": 8.5, "pcu": 1120, "dimensionsBlocks": [9, 11, 5] },
        "bounds": { "min": [-4, 1, -5], "max": [4, 5, 5] },
        "stepPattern": r"### Step 13\.(\d+) · ([^\n]+)"
    },
    {
        "id": "RIG-01",
        "chapter": 6,
        "name": "Floating Rig Structure (25 kt Fortress)",
        "file": "floating-rig.json",
        "src": "manuals/chapters/06-floating-rig.md",
        "grid": "Large",
        "tier": "Fortress Tier · Aerial Fleet Hub",
        "stats": { "totalBlocks": 4980, "dryMassTonnes": 25000.0, "pcu": 29500, "dimensionsBlocks": [36, 36, 28] },
        "bounds": { "min": [-18, 1, -18], "max": [18, 28, 18] },
        "stepPattern": r"### (?:Step|Section|Deck)\s+([0-9\.]+|S[0-9]+|H[0-9]+) · ([^\n]+)"
    }
]

PALETTE = [
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

def compile_cfg(cfg):
    src_file = cfg["src"]
    if not os.path.exists(src_file):
        print(f"Skipping {src_file} (not found)")
        return None

    with open(src_file, "r", encoding="utf-8") as f:
        content = f.read()

    # Split steps
    pattern = r"\n### (?:Step\s+([0-9\.]+|[0-9A-Za-z]+)|Phase\s+([0-9\-]+)|Section\s+([0-9\.]+)) · ([^\n]+)\n"
    matches = list(re.finditer(pattern, content))
    
    steps = []
    min_x, max_x = cfg["bounds"]["min"][0], cfg["bounds"]["max"][0]
    min_z, max_z = cfg["bounds"]["min"][2], cfg["bounds"]["max"][2]

    for i, m in enumerate(matches):
        step_id = m.group(1) or m.group(2) or m.group(3) or str(i+1)
        title = m.group(4).strip()
        start_idx = m.end()
        end_idx = matches[i+1].start() if i+1 < len(matches) else len(content)
        body = content[start_idx:end_idx]

        # Extract instructions, checks, parts
        inst = [l.strip() for l in re.findall(r"^\d+\.\s+(.+)$", body, re.MULTILINE)]
        checks = re.findall(r"\*\s*\[[ x]\]\s*(.+)", body)
        m_chk = re.search(r"\*\*✓ Check:\*\*\s*(.+?)(?=\n\n|\n---|####|\Z)", body, re.DOTALL)
        if m_chk: checks.append(m_chk.group(1).strip().replace("\n", " "))

        # Synthesize blocks for this step within bounds
        sub = "structure"
        tl = (title + " " + body).lower()
        if "conveyor" in tl or "pipe" in tl: sub = "conveyor"
        elif "power" in tl or "reactor" in tl or "battery" in tl: sub = "power"
        elif "thrust" in tl or "atmo" in tl or "engine" in tl or "wheel" in tl: sub = "propulsion"
        elif "cockpit" in tl or "bridge" in tl or "pb" in tl: sub = "avionics"

        step_blocks = []
        z_pos = int(min_z + (max_z - min_z) * (i / max(1, len(matches) - 1)))
        y_lvl = 2 + (i % 3)
        for x in range(max(min_x, -2), min(max_x + 1, 3)):
            step_blocks.append({
                "pos": [x, y_lvl, z_pos],
                "shape": "cube" if sub != "conveyor" else "conveyor",
                "subtype": "LargeBlockArmorBlock",
                "paletteSlot": 0 if sub == "structure" else 4 if sub == "conveyor" else 8,
                "subsystem": sub,
                "name": f"{cfg['id']} {title[:20]} [{x}, {y_lvl}, {z_pos}]"
            })

        steps.append({
            "step": step_id,
            "phase": f"{cfg['chapter']}-1",
            "title": title,
            "level": f"L{y_lvl}",
            "subsystem": sub,
            "partsTray": [{ "type": "Armor Full Block", "qty": len(step_blocks), "slot": 0, "mass": 1.2, "note": "Primary section module" }],
            "instructions": inst if inst else ["Assemble section according to master spatial datum."],
            "checks": checks if checks else ["Verify physical clearances and continuous conveyor flow."],
            "blocks": step_blocks
        })

    model = {
        "$schema": "https://json-schema.org/draft/2020-12/schema",
        "id": cfg["id"],
        "name": cfg["name"],
        "chapter": cfg["chapter"],
        "gridSize": cfg["grid"],
        "blockMeters": 2.5 if cfg["grid"] == "Large" else 0.5,
        "stats": cfg["stats"],
        "bounds": cfg["bounds"],
        "palette": PALETTE,
        "steps": steps
    }

    out_path = os.path.join("studio", "data", cfg["file"])
    with open(out_path, "w", encoding="utf-8") as f:
        json.dump(model, f, indent=2)

    print(f"Compiled {cfg['id']} -> {out_path} ({len(steps)} steps)")
    return {
        "id": cfg["id"],
        "chapter": cfg["chapter"],
        "name": cfg["name"],
        "file": cfg["file"],
        "grid": cfg["grid"],
        "tier": cfg["tier"],
        "blocks": cfg["stats"]["totalBlocks"],
        "mass": f"{cfg['stats']['dryMassTonnes']} t",
        "pcu": cfg["stats"]["pcu"]
    }

def main():
    registry = [
        {
            "id": "MAS-01",
            "chapter": 10,
            "name": "MASTODON-01 MKI Expeditionary Excavator & Mobile FOB",
            "file": "mastodon.json",
            "grid": "Large",
            "tier": "Ground Tier · Heavy Extraction",
            "blocks": 1126,
            "mass": "1151.8 t",
            "pcu": 10015
        }
    ]

    for cfg in CHAPTER_CONFIGS:
        res = compile_cfg(cfg)
        if res:
            registry.append(res)

    # Sort by chapter number
    registry.sort(key=lambda x: x["chapter"])
    with open("studio/data/chapters.json", "w", encoding="utf-8") as f:
        json.dump(registry, f, indent=2)

    print(f"Master chapters registry generated with {len(registry)} models in studio/data/chapters.json")

if __name__ == "__main__":
    main()
