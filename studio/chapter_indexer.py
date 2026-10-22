#!/usr/bin/env python3
"""Validate chapter models, compile manual prose, and generate the chapter registry."""

import argparse
import json
import math
import re
import sys
from pathlib import Path


ROOT = Path(__file__).resolve().parent
DATA_DIR = ROOT / "data"
MANUALS_DIR = ROOT.parent / "manuals" / "chapters"
STEP_HEADING = re.compile(
    r"^###\s+Step\s+([0-9A-Za-z][0-9A-Za-z.-]*)\s*[·:]\s*(.+?)\s*$",
    re.MULTILINE,
)
PALETTE_COLOR = re.compile(r"^#[0-9A-Fa-f]{6}$")
VALID_SHAPES = {
    "assembler", "battery", "cargo", "cargo_small", "catwalk", "cockpit", "conveyor",
    "cube", "engine", "half", "halfslope", "hinge", "piston", "reactor", "refinery",
    "rotor", "slope2x1", "solar", "turret", "welder", "wheel5x5",
}
VALID_SUBSYSTEMS = {"avionics", "boom", "conveyor", "flightdeck", "power", "propulsion", "structure"}


def read_json(path):
    try:
        return json.loads(path.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        raise ValueError(f"{path}: {exc}") from exc


def is_int(value):
    return isinstance(value, int) and not isinstance(value, bool)


def validate_model(model, path):
    errors = []
    label = str(path)

    def error(message):
        errors.append(f"{label}: {message}")

    if not isinstance(model, dict):
        return [f"{label}: model root must be an object"]

    for key in ("id", "name", "chapter", "gridSize", "blockMeters", "stats", "bounds", "palette", "steps"):
        if key not in model:
            error(f"missing required field {key!r}")

    if not isinstance(model.get("id"), str) or not re.fullmatch(r"[A-Z0-9-]+", model["id"]):
        error("'id' must contain only uppercase letters, digits, and hyphens")
    if not isinstance(model.get("name"), str) or not model["name"].strip():
        error("'name' must be a non-empty string")
    if not is_int(model.get("chapter")) or model["chapter"] < 1:
        error("'chapter' must be a positive integer")
    if "chapterLabel" in model and (not isinstance(model["chapterLabel"], str) or not model["chapterLabel"].strip()):
        error("'chapterLabel' must be a non-empty string when provided")

    grid = model.get("gridSize")
    if grid not in ("Large", "Small"):
        error("'gridSize' must be 'Large' or 'Small'")
    expected_meters = {"Large": 2.5, "Small": 0.5}.get(grid)
    if not isinstance(model.get("blockMeters"), (int, float)) or model.get("blockMeters") != expected_meters:
        error("'blockMeters' must match the Space Engineers grid size (Large: 2.5, Small: 0.5)")

    stats = model.get("stats")
    if not isinstance(stats, dict):
        error("'stats' must be an object")
    else:
        for key in ("totalBlocks", "dryMassTonnes", "pcu"):
            value = stats.get(key)
            if not isinstance(value, (int, float)) or isinstance(value, bool) or not math.isfinite(value) or value < 0:
                error(f"'stats.{key}' must be a finite non-negative number")

        dims_blocks = stats.get("dimensionsBlocks")
        valid_dims_blocks = (
            isinstance(dims_blocks, list) and len(dims_blocks) == 3 and
            all(is_int(value) and value > 0 for value in dims_blocks)
        )
        if not valid_dims_blocks:
            error("'stats.dimensionsBlocks' must be a 3-element positive integer array ordered as [X, Z, Y]")

        dims_meters = stats.get("dimensionsMeters")
        valid_dims_meters = (
            isinstance(dims_meters, list) and len(dims_meters) == 3 and
            all(isinstance(value, (int, float)) and not isinstance(value, bool) and math.isfinite(value) and value > 0
                for value in dims_meters)
        )
        if dims_meters is not None and not valid_dims_meters:
            error("'stats.dimensionsMeters' must be a 3-element positive numeric array ordered as [X, Z, Y]")

    bounds = model.get("bounds")
    valid_bounds = isinstance(bounds, dict)
    if valid_bounds:
        for key in ("min", "max"):
            vector = bounds.get(key)
            if not isinstance(vector, list) or len(vector) != 3 or not all(is_int(v) for v in vector):
                error(f"'bounds.{key}' must be a 3-element integer array")
                valid_bounds = False
        if valid_bounds and any(bounds["min"][i] > bounds["max"][i] for i in range(3)):
            error("'bounds.min' must not exceed 'bounds.max'")
            valid_bounds = False
    else:
        error("'bounds' must be an object")

    if isinstance(stats, dict):
        dims_blocks = stats.get("dimensionsBlocks")
        dims_meters = stats.get("dimensionsMeters")
        valid_dims_blocks = (
            isinstance(dims_blocks, list) and len(dims_blocks) == 3 and
            all(is_int(value) and value > 0 for value in dims_blocks)
        )
        valid_dims_meters = (
            isinstance(dims_meters, list) and len(dims_meters) == 3 and
            all(isinstance(value, (int, float)) and not isinstance(value, bool) and math.isfinite(value) and value > 0
                for value in dims_meters)
        )
        if model.get("id") == "MAS-01" and valid_bounds and valid_dims_blocks:
            expected_dims_blocks = [
                bounds["max"][0] - bounds["min"][0] + 1,  # X span
                bounds["max"][2] - bounds["min"][2] + 1,  # Z span
                bounds["max"][1] - bounds["min"][1] + 1,  # Y span
            ]
            if dims_blocks != expected_dims_blocks:
                error(
                    "MAS-01 'stats.dimensionsBlocks' must match bounds spans in [X, Z, Y] order; "
                    f"expected {expected_dims_blocks}, got {dims_blocks}"
                )
        if valid_dims_blocks and valid_dims_meters and isinstance(model.get("blockMeters"), (int, float)):
            expected_dims_meters = [dims_blocks[index] * model["blockMeters"] for index in range(3)]
            if any(not math.isclose(expected_dims_meters[index], dims_meters[index], rel_tol=0.0, abs_tol=1e-6)
                   for index in range(3)):
                error(
                    "'stats.dimensionsMeters' must equal dimensionsBlocks × blockMeters in [X, Z, Y] order; "
                    f"expected {expected_dims_meters}, got {dims_meters}"
                )

    palette = model.get("palette")
    palette_slots = set()
    if not isinstance(palette, list) or not palette:
        error("'palette' must be a non-empty array")
        palette = []
    for index, color in enumerate(palette):
        if not isinstance(color, dict):
            error(f"'palette[{index}]' must be an object")
            continue
        slot = color.get("slot")
        if not is_int(slot) or slot < 0:
            error(f"'palette[{index}].slot' must be a non-negative integer")
        elif slot in palette_slots:
            error(f"duplicate palette slot {slot}")
        else:
            palette_slots.add(slot)
        if not isinstance(color.get("name"), str) or not color["name"]:
            error(f"'palette[{index}].name' must be a non-empty string")
        if not isinstance(color.get("hex"), str) or not PALETTE_COLOR.fullmatch(color["hex"]):
            error(f"'palette[{index}].hex' must be a six-digit hex color")

    steps = model.get("steps")
    if not isinstance(steps, list) or not steps:
        error("'steps' must be a non-empty array")
        steps = []
    step_ids = set()
    for step_index, step in enumerate(steps):
        prefix = f"'steps[{step_index}]'"
        if not isinstance(step, dict):
            error(f"{prefix} must be an object")
            continue
        step_id = step.get("step")
        if not isinstance(step_id, (str, int)) or not str(step_id).strip():
            error(f"{prefix}.step must be a non-empty string or integer")
        elif str(step_id) in step_ids:
            error(f"duplicate step id {step_id!r}")
        else:
            step_ids.add(str(step_id))
        if not isinstance(step.get("title"), str) or not step["title"].strip():
            error(f"{prefix}.title must be a non-empty string")
        if not isinstance(step.get("phase"), str) or not step["phase"].strip():
            error(f"{prefix}.phase must be a non-empty string")
        if not isinstance(step.get("level"), str) or not re.fullmatch(r"L\d+(?:-L?\d+)?", step["level"]):
            error(f"{prefix}.level must use the L<number> or L<number>-L<number> format")
        blocks = step.get("blocks")
        if not isinstance(blocks, list):
            error(f"{prefix}.blocks must be an array of explicit placements")
            continue
        if not blocks:
            error(f"{prefix}.blocks must contain at least one explicit placement")
        for block_index, block in enumerate(blocks):
            block_prefix = f"{prefix}.blocks[{block_index}]"
            if not isinstance(block, dict):
                error(f"{block_prefix} must be an object")
                continue
            pos = block.get("pos")
            if not isinstance(pos, list) or len(pos) != 3 or not all(is_int(v) for v in pos):
                error(f"{block_prefix}.pos must be a 3-element integer array")
                continue
            if valid_bounds and any(pos[i] < bounds["min"][i] or pos[i] > bounds["max"][i] for i in range(3)):
                error(f"{block_prefix}.pos {pos} is outside declared bounds")
            slot = block.get("paletteSlot", 0)
            if not is_int(slot) or slot not in palette_slots:
                error(f"{block_prefix}.paletteSlot {slot!r} is not defined in the palette")
            shape = block.get("shape")
            if shape not in VALID_SHAPES:
                error(f"{block_prefix}.shape {shape!r} is not supported by the 3D renderer")
            subsystem = block.get("subsystem", "structure")
            if subsystem not in VALID_SUBSYSTEMS:
                error(f"{block_prefix}.subsystem {subsystem!r} is not supported")
            for vector_name in ("rot", "scale"):
                vector = block.get(vector_name)
                if vector is not None:
                    if (not isinstance(vector, list) or len(vector) != 3 or
                            not all(isinstance(value, (int, float)) and not isinstance(value, bool) and math.isfinite(value) for value in vector)):
                        error(f"{block_prefix}.{vector_name} must be a finite 3-element numeric array")
                    elif vector_name == "scale" and any(value <= 0 for value in vector):
                        error(f"{block_prefix}.scale values must be positive")

    figure_status = model.get("designFigureStatus")
    if figure_status is not None:
        if (not isinstance(figure_status, dict) or
                figure_status.get("status") != "unverified" or
                not isinstance(figure_status.get("label"), str) or
                not figure_status["label"].strip()):
            error("'designFigureStatus' must identify unverified figures with a non-empty label")

    design_subsystems = model.get("designSubsystems")
    if design_subsystems is not None:
        if not isinstance(design_subsystems, list):
            error("'designSubsystems' must be an array")
        else:
            subsystem_ids = set()
            required_fields = ("label", "physicalInterface", "futurePB", "manualFallback", "commissioning")
            for index, subsystem in enumerate(design_subsystems):
                prefix = f"'designSubsystems[{index}]'"
                if not isinstance(subsystem, dict):
                    error(f"{prefix} must be an object")
                    continue
                subsystem_id = subsystem.get("id")
                if not isinstance(subsystem_id, str) or not subsystem_id.strip():
                    error(f"{prefix}.id must be a non-empty string")
                elif subsystem_id in subsystem_ids:
                    error(f"duplicate design subsystem id {subsystem_id!r}")
                else:
                    subsystem_ids.add(subsystem_id)
                for field in required_fields:
                    if not isinstance(subsystem.get(field), str) or not subsystem[field].strip():
                        error(f"{prefix}.{field} must be a non-empty string")
                referenced_steps = subsystem.get("steps")
                if (not isinstance(referenced_steps, list) or not referenced_steps or
                        not all(isinstance(value, (str, int)) for value in referenced_steps)):
                    error(f"{prefix}.steps must be a non-empty array of step IDs")
                else:
                    for referenced_step in referenced_steps:
                        if str(referenced_step) not in step_ids:
                            error(f"{prefix}.steps references unknown step {referenced_step!r}")

    return errors


def parse_parts_tray(section):
    parts = []
    rows = [line.strip() for line in section.splitlines() if line.strip().startswith("|")]
    for line in rows[2:]:
        columns = [column.strip() for column in line.split("|")[1:-1]]
        if len(columns) < 4:
            continue
        quantity = re.search(r"\d+", columns[1])
        slot = re.search(r"Slot\s+(\d+)", columns[2], re.IGNORECASE)
        parts.append({
            "type": columns[0],
            "qty": int(quantity.group()) if quantity else 1,
            "slot": int(slot.group(1)) if slot else 0,
            "massPcu": columns[3],
            "note": columns[4] if len(columns) > 4 else "",
        })
    return parts


def parse_step_prose(step_text, step_id):
    def section(name):
        step_prefix = rf"(?:Step\s+{re.escape(step_id)}\s+)?" if name == "Parts Tray" else ""
        match = re.search(
            rf"^####\s+{step_prefix}{re.escape(name)}\s*\n([\s\S]*?)(?=^####\s|^\*\*✓ Check:\*\*|^---|\Z)",
            step_text,
            re.MULTILINE | re.IGNORECASE,
        )
        return match.group(1).strip() if match else ""

    instructions_text = section("Placement Instructions")
    instructions = [line.strip() for line in instructions_text.splitlines() if line.strip()]
    parts = parse_parts_tray(section("Parts Tray"))
    terminal = section("Terminal Configuration & Sliders")
    check_match = re.search(r"\*\*✓ Check:\*\*\s*(.+?)(?=\n\n|\n---|^####|\Z)", step_text, re.DOTALL | re.MULTILINE)
    checks = [check_match.group(1).strip().replace("\n", " ")] if check_match else []
    checks.extend(re.findall(r"^\s*\*\s*\[[ x]\]\s*(.+)$", step_text, re.MULTILINE))
    return instructions, parts, terminal, checks


def compile_manual(manual_path, model_path):
    manual_text = manual_path.read_text(encoding="utf-8")
    model = read_json(model_path)
    matches = list(STEP_HEADING.finditer(manual_text))
    if not matches:
        raise ValueError(f"{manual_path}: no `### Step <id> · <title>` headings found")

    source_steps = {str(step.get("step")): step for step in model.get("steps", []) if isinstance(step, dict)}
    compiled_steps = []
    for index, match in enumerate(matches):
        step_id, title = match.group(1), match.group(2).strip()
        end = matches[index + 1].start() if index + 1 < len(matches) else len(manual_text)
        step_text = manual_text[match.end():end]
        source = source_steps.get(step_id)
        if source is None:
            raise ValueError(f"{manual_path}: step {step_id!r} has no matching explicit placement entry in {model_path}")
        if not isinstance(source.get("blocks"), list) or not source["blocks"]:
            raise ValueError(f"{model_path}: step {step_id!r} must contain explicitly authored block placements")

        instructions, parts, terminal, checks = parse_step_prose(step_text, step_id)
        compiled = dict(source)
        compiled["step"] = step_id
        compiled["title"] = title
        level = re.search(r"\((L[0-9]+(?:-[0-9A-Za-z]+)?)\)", title)
        if level:
            compiled["level"] = level.group(1)
        compiled["instructions"] = instructions
        compiled["partsTray"] = parts
        compiled["terminalConfig"] = terminal
        compiled["checks"] = checks
        compiled_steps.append(compiled)

    if set(source_steps) != {step["step"] for step in compiled_steps}:
        missing = sorted(set(source_steps) - {step["step"] for step in compiled_steps})
        raise ValueError(f"{manual_path}: model steps not represented by manual headings: {', '.join(missing)}")
    model["steps"] = compiled_steps
    errors = validate_model(model, model_path)
    if errors:
        raise ValueError("\n".join(errors))
    write_json(model_path, model)
    print(f"Compiled {len(compiled_steps)} steps from {manual_path} into {model_path}")


def write_json(path, value):
    path.parent.mkdir(parents=True, exist_ok=True)
    temporary = path.with_suffix(path.suffix + ".tmp")
    temporary.write_text(json.dumps(value, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    temporary.replace(path)


def existing_registry():
    path = DATA_DIR / "chapters.json"
    if not path.exists():
        return {}
    try:
        return {entry.get("id"): entry for entry in read_json(path) if isinstance(entry, dict)}
    except (ValueError, TypeError):
        return {}


def registry_entry(model, previous):
    stats = model["stats"]
    return {
        "id": model["id"],
        "chapter": model["chapter"],
        "chapterLabel": model.get("chapterLabel", str(model["chapter"])),
        "name": model["name"],
        "file": model.get("file", ""),
        "grid": model["gridSize"],
        "tier": model.get("tier") or previous.get(model["id"], {}).get("tier", ""),
        "blocks": stats["totalBlocks"],
        "mass": f'{stats["dryMassTonnes"]:.1f} t',
        "pcu": stats["pcu"],
    }


def load_models():
    paths = sorted(
        path for path in DATA_DIR.glob("*.json")
        if path.name != "chapters.json" and not path.name.startswith("notes_")
    )
    if not paths:
        raise ValueError(f"no chapter models found in {DATA_DIR}")
    models = []
    for path in paths:
        model = read_json(path)
        model["file"] = path.name
        models.append((path, model))
    return models


def validate_all():
    errors = []
    seen_ids = set()
    seen_chapters = set()
    models = load_models()
    for path, model in models:
        errors.extend(validate_model(model, path))
        if isinstance(model, dict):
            if model.get("id") in seen_ids:
                errors.append(f"{path}: duplicate model id {model.get('id')!r}")
            seen_ids.add(model.get("id"))
            if model.get("chapter") in seen_chapters:
                errors.append(f"{path}: duplicate chapter number {model.get('chapter')!r}")
            seen_chapters.add(model.get("chapter"))
    return models, errors


def generate_registry():
    models, errors = validate_all()
    if errors:
        raise ValueError("\n".join(errors))
    previous = existing_registry()
    entries = [registry_entry(model, previous) for _, model in models]
    entries.sort(key=lambda entry: entry["chapter"])
    write_json(DATA_DIR / "chapters.json", entries)
    print(f"Validated {len(entries)} models and generated {DATA_DIR / 'chapters.json'}")


def create_chapter(chapter, slug, model_id, name, grid, tier):
    if chapter < 1:
        raise ValueError("chapter number must be positive")
    if not re.fullmatch(r"[a-z0-9]+(?:-[a-z0-9]+)*", slug):
        raise ValueError("slug must use lowercase letters, digits, and hyphens")
    if not re.fullmatch(r"[A-Z0-9-]+", model_id):
        raise ValueError("model ID must contain only uppercase letters, digits, and hyphens")
    if not name.strip():
        raise ValueError("model name must not be empty")
    manual_path = MANUALS_DIR / f"{chapter:02d}-{slug}.md"
    model_path = DATA_DIR / f"{slug}.json"
    if manual_path.exists() or model_path.exists():
        raise ValueError(f"chapter scaffold already exists: {manual_path} or {model_path}")
    existing_paths = (
        path for path in DATA_DIR.glob("*.json")
        if path.name != "chapters.json" and not path.name.startswith("notes_")
    )
    for existing_path in existing_paths:
        existing_model = read_json(existing_path)
        if existing_model.get("id") == model_id:
            raise ValueError(f"model ID {model_id!r} already exists in {existing_path}")
        if existing_model.get("chapter") == chapter:
            raise ValueError(f"chapter number {chapter} already exists in {existing_path}")
    meters = {"Large": 2.5, "Small": 0.5}[grid]
    model = {
        "id": model_id,
        "name": name,
        "chapter": chapter,
        "chapterLabel": str(chapter),
        "gridSize": grid,
        "blockMeters": meters,
        "tier": tier,
        "stats": {
            "totalBlocks": 0,
            "dryMassTonnes": 0,
            "pcu": 0,
            "dimensionsBlocks": [0, 0, 0],
            "dimensionsMeters": [0, 0, 0],
        },
        "bounds": {"min": [0, 0, 0], "max": [0, 0, 0]},
        "palette": [],
        "phases": [],
        "steps": [],
    }
    manual = (
        f"# Chapter {chapter} · {name}\n\n"
        "> Canonical build prose. Use one `### Step <id> · <title>` heading for each placement step.\n"
        "> Add explicit integer-coordinate placements to the matching `steps[].blocks` array in "
        f"`studio/data/{slug}.json`; the compiler never invents geometry.\n\n"
        f"### Step {chapter}.1 · First build step (L1)\n\n"
        "#### Parts Tray\n\n"
        "| Block | Qty | Palette | Mass / PCU | Notes |\n"
        "|---|---:|---|---|---|\n"
        "| Example block | 1 | Slot 0 | TBD | Replace with the authored bill of materials |\n\n"
        "#### Placement Instructions\n\n"
        "1. Replace this text with the canonical placement instructions.\n\n"
        "**✓ Check:** Confirm the step matches the in-game reference.\n"
    )
    write_json(model_path, model)
    manual_path.parent.mkdir(parents=True, exist_ok=True)
    manual_path.write_text(manual, encoding="utf-8")
    print(f"Created {manual_path} and {model_path}")


def print_stats():
    models, errors = validate_all()
    if errors:
        raise ValueError("\n".join(errors))
    print(f"Models: {len(models)}")
    for _, model in models:
        block_count = sum(len(step["blocks"]) for step in model["steps"])
        print(f"{model['id']} · CH {model['chapter']}: {len(model['steps'])} steps, {block_count} placements")


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--validate", metavar="MODEL", help="validate one model JSON file")
    parser.add_argument("--compile", metavar="MANUAL", help="compile manual prose into a model's matching steps")
    parser.add_argument("--model", help="model JSON path used with --compile")
    parser.add_argument("--all", action="store_true", help="validate all models and regenerate the chapter registry")
    parser.add_argument("--stats", action="store_true", help="print model and placement counts")
    parser.add_argument("--new", nargs=6, metavar=("CHAPTER", "SLUG", "ID", "NAME", "GRID", "TIER"),
                        help="scaffold a manual and explicit-placement model source")
    args = parser.parse_args()

    try:
        if args.new:
            chapter, slug, model_id, name, grid, tier = args.new
            if grid not in ("Large", "Small"):
                raise ValueError("grid must be Large or Small")
            create_chapter(int(chapter), slug, model_id, name, grid, tier)
        elif args.compile:
            if not args.model:
                parser.error("--compile requires --model")
            compile_manual(Path(args.compile).resolve(), Path(args.model).resolve())
        elif args.validate:
            path = Path(args.validate).resolve()
            errors = validate_model(read_json(path), path)
            if errors:
                raise ValueError("\n".join(errors))
            print(f"Valid model: {path}")
        elif args.all:
            generate_registry()
        elif args.stats:
            print_stats()
        else:
            parser.print_help()
            return 2
    except (OSError, ValueError, TypeError) as exc:
        print(f"ERROR: {exc}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
