import json
import sys
import tempfile
import unittest
from pathlib import Path


sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
import chapter_indexer


class ChapterIndexerTests(unittest.TestCase):
    def test_existing_models_validate_and_match_registry(self):
        models, errors = chapter_indexer.validate_all()
        self.assertEqual(errors, [])
        registry = chapter_indexer.read_json(chapter_indexer.DATA_DIR / "chapters.json")
        self.assertEqual(
            {model["file"] for _, model in models},
            {entry["file"] for entry in registry},
        )
        self.assertEqual(
            {model["id"] for _, model in models},
            {entry["id"] for entry in registry},
        )

    def test_mastodon_design_interfaces_reference_real_steps_and_unverified_targets(self):
        model = chapter_indexer.read_json(chapter_indexer.DATA_DIR / "mastodon.json")
        errors = chapter_indexer.validate_model(model, "mastodon.json")
        self.assertEqual(errors, [])
        self.assertEqual(model["designFigureStatus"]["status"], "unverified")
        step_ids = {str(step["step"]) for step in model["steps"]}
        covered_steps = {
            step_id
            for subsystem in model["designSubsystems"]
            for step_id in subsystem["steps"]
        }
        self.assertTrue(step_ids.issubset(covered_steps))
        self.assertEqual(model["stats"]["totalBlocks"], 1128)
        self.assertEqual(model["stats"]["dryMassTonnes"], 1258.2)
        self.assertEqual(model["stats"]["pcu"], 10080)

    def test_mastodon_directional_placements_are_audited_with_manual_evidence(self):
        model = chapter_indexer.read_json(chapter_indexer.DATA_DIR / "mastodon.json")
        audited = [
            block
            for step in model["steps"]
            for block in step["blocks"]
            if chapter_indexer.is_orientation_sensitive(block)
        ]
        self.assertTrue(audited)
        self.assertTrue(all(block.get("orientationStatus") in chapter_indexer.ORIENTATION_STATUSES for block in audited))
        self.assertTrue(all(block.get("orientationEvidence", "").strip() for block in audited))

        unresolved = [block for block in audited if block["orientationStatus"] == "needs-review"]
        self.assertTrue(unresolved)
        self.assertTrue(all("rot" not in block for block in unresolved))

        dynamic_turrets = [block for block in audited if block["orientationStatus"] == "dynamic"]
        self.assertTrue(dynamic_turrets)
        self.assertTrue(all(block["shape"] == "turret" for block in dynamic_turrets))

        by_name = {
            block.get("name"): block
            for step in model["steps"]
            for block in step["blocks"]
            if block.get("name")
        }
        for name, expected in (
            ("MAS Hitch Yaw", [-90, 0, 0]),
            ("MAS Boom Shoulder", [0, 0, 90]),
            ("MAS Crew Ladder Hinge", [90, 0, 0]),
            ("MAS MP Conn", [-90, 0, 0]),
            ("MAS Welder Pad Kestrel 1", [-90, 0, 0]),
        ):
            with self.subTest(name=name):
                self.assertEqual(by_name[name]["orientationStatus"], "documented")
                self.assertEqual(by_name[name]["rot"], expected)

        ladder_steps = next(step for step in model["steps"] if step["step"] == "10.27")
        ladder_segments = [block for block in ladder_steps["blocks"] if block.get("subtype") == "NarrowLadderBlock"]
        self.assertEqual(len(ladder_segments), 2)
        self.assertTrue(all(block["rot"] == [90, 0, 0] for block in ladder_segments))
        self.assertTrue(all(block["orientationStatus"] == "documented" for block in ladder_segments))

    def test_mastodon_validator_rejects_unaudited_directional_placement(self):
        model = chapter_indexer.read_json(chapter_indexer.DATA_DIR / "mastodon.json")
        block = next(
            block
            for step in model["steps"]
            for block in step["blocks"]
            if block.get("shape") == "wheel5x5"
        )
        del block["orientationStatus"]
        del block["orientationEvidence"]
        del block["orientationFeature"]
        errors = chapter_indexer.validate_model(model, "mastodon.json")
        self.assertTrue(any("missing Mastodon orientation audit status" in error for error in errors))

    def test_rejects_unknown_design_subsystem_step(self):
        model = {
            "id": "TEST-01",
            "name": "Test",
            "chapter": 99,
            "gridSize": "Small",
            "blockMeters": 0.5,
            "stats": {
                "totalBlocks": 1,
                "dryMassTonnes": 1,
                "pcu": 1,
                "dimensionsBlocks": [1, 1, 1],
                "dimensionsMeters": [0.5, 0.5, 0.5],
            },
            "bounds": {"min": [0, 1, 0], "max": [0, 1, 0]},
            "palette": [{"slot": 0, "name": "Steel", "hex": "#112233"}],
            "steps": [{
                "step": "99.1",
                "title": "Test placement",
                "phase": "99-1",
                "level": "L1",
                "blocks": [{"pos": [0, 1, 0], "shape": "cube", "paletteSlot": 0}],
            }],
            "designSubsystems": [{
                "id": "test-interface",
                "label": "Test interface",
                "steps": ["99.2"],
                "physicalInterface": "Test mount",
                "futurePB": "Test script",
                "manualFallback": "Manual test",
                "commissioning": "Test in game",
            }],
        }
        errors = chapter_indexer.validate_model(model, "test.json")
        self.assertTrue(any("references unknown step '99.2'" in error for error in errors))

    def test_rejects_out_of_bounds_placement(self):
        model = {
            "id": "TEST-01",
            "name": "Test",
            "chapter": 99,
            "gridSize": "Small",
            "blockMeters": 0.5,
            "stats": {
                "totalBlocks": 1,
                "dryMassTonnes": 1,
                "pcu": 1,
                "dimensionsBlocks": [1, 1, 1],
                "dimensionsMeters": [0.5, 0.5, 0.5],
            },
            "bounds": {"min": [0, 0, 0], "max": [0, 0, 0]},
            "palette": [{"slot": 0, "name": "Steel", "hex": "#112233"}],
            "steps": [{
                "step": "99.1",
                "title": "Test placement",
                "phase": "99-1",
                "level": "L1",
                "blocks": [{"pos": [1, 0, 0], "shape": "cube", "paletteSlot": 0}],
            }],
        }
        errors = chapter_indexer.validate_model(model, "test.json")
        self.assertTrue(any("outside declared bounds" in error for error in errors))

    def test_rejects_mastodon_dimension_blocks_when_not_matching_bounds_spans(self):
        model = {
            "id": "MAS-01",
            "name": "Test",
            "chapter": 99,
            "gridSize": "Small",
            "blockMeters": 0.5,
            "stats": {
                "totalBlocks": 1,
                "dryMassTonnes": 1,
                "pcu": 1,
                "dimensionsBlocks": [2, 1, 1],
                "dimensionsMeters": [1.0, 0.5, 0.5],
            },
            "bounds": {"min": [0, 0, 0], "max": [0, 0, 0]},
            "palette": [{"slot": 0, "name": "Steel", "hex": "#112233"}],
            "steps": [{
                "step": "99.1",
                "title": "Test placement",
                "phase": "99-1",
                "level": "L1",
                "blocks": [{"pos": [0, 0, 0], "shape": "cube", "paletteSlot": 0}],
            }],
        }
        errors = chapter_indexer.validate_model(model, "test.json")
        self.assertTrue(any("MAS-01 'stats.dimensionsBlocks' must match bounds spans" in error for error in errors))

    def test_rejects_dimension_meters_when_not_matching_block_scale(self):
        model = {
            "id": "TEST-01",
            "name": "Test",
            "chapter": 99,
            "gridSize": "Small",
            "blockMeters": 0.5,
            "stats": {
                "totalBlocks": 1,
                "dryMassTonnes": 1,
                "pcu": 1,
                "dimensionsBlocks": [1, 1, 1],
                "dimensionsMeters": [0.6, 0.5, 0.5],
            },
            "bounds": {"min": [0, 0, 0], "max": [0, 0, 0]},
            "palette": [{"slot": 0, "name": "Steel", "hex": "#112233"}],
            "steps": [{
                "step": "99.1",
                "title": "Test placement",
                "phase": "99-1",
                "level": "L1",
                "blocks": [{"pos": [0, 0, 0], "shape": "cube", "paletteSlot": 0}],
            }],
        }
        errors = chapter_indexer.validate_model(model, "test.json")
        self.assertTrue(any("dimensionsMeters' must equal dimensionsBlocks × blockMeters" in error for error in errors))

    def test_compile_joins_manual_prose_without_changing_placements(self):
        model = {
            "id": "TEST-01",
            "name": "Test",
            "chapter": 99,
            "gridSize": "Small",
            "blockMeters": 0.5,
            "stats": {
                "totalBlocks": 1,
                "dryMassTonnes": 1,
                "pcu": 1,
                "dimensionsBlocks": [1, 1, 1],
                "dimensionsMeters": [0.5, 0.5, 0.5],
            },
            "bounds": {"min": [0, 1, 0], "max": [0, 1, 0]},
            "palette": [{"slot": 0, "name": "Steel", "hex": "#112233"}],
            "steps": [{
                "step": "99.1",
                "phase": "99-1",
                "title": "Old title",
                "level": "L1",
                "subsystem": "structure",
                "blocks": [{"pos": [0, 1, 0], "shape": "cube", "paletteSlot": 0}],
            }],
        }
        manual = """### Step 99.1 · Test Frame (L1)

#### Step 99.1 Parts Tray
| Block | Qty | Palette | Mass / PCU | Notes |
|---|---:|---|---|---|
| Steel Block | 1 | Slot 0 | 1 t / 1 | Frame |

#### Placement Instructions
1. Place the frame at the origin.

**✓ Check:** Confirm alignment.
"""
        with tempfile.TemporaryDirectory() as directory:
            directory = Path(directory)
            model_path = directory / "model.json"
            manual_path = directory / "manual.md"
            model_path.write_text(json.dumps(model), encoding="utf-8")
            manual_path.write_text(manual, encoding="utf-8")

            chapter_indexer.compile_manual(manual_path, model_path)
            compiled = json.loads(model_path.read_text(encoding="utf-8"))

        self.assertEqual(compiled["steps"][0]["title"], "Test Frame (L1)")
        self.assertEqual(compiled["steps"][0]["blocks"], model["steps"][0]["blocks"])
        self.assertEqual(compiled["steps"][0]["instructions"], ["1. Place the frame at the origin."])
        self.assertEqual(compiled["steps"][0]["partsTray"][0]["type"], "Steel Block")
        self.assertEqual(compiled["steps"][0]["checks"], ["Confirm alignment."])

    def test_compile_rejects_manual_step_without_explicit_geometry(self):
        model = {
            "id": "TEST-01",
            "name": "Test",
            "chapter": 99,
            "gridSize": "Small",
            "blockMeters": 0.5,
            "stats": {
                "totalBlocks": 0,
                "dryMassTonnes": 0,
                "pcu": 0,
                "dimensionsBlocks": [1, 1, 1],
                "dimensionsMeters": [0.5, 0.5, 0.5],
            },
            "bounds": {"min": [0, 0, 0], "max": [0, 0, 0]},
            "palette": [{"slot": 0, "name": "Steel", "hex": "#112233"}],
            "steps": [{"step": "99.1", "title": "Empty", "blocks": []}],
        }
        with tempfile.TemporaryDirectory() as directory:
            directory = Path(directory)
            model_path = directory / "model.json"
            manual_path = directory / "manual.md"
            model_path.write_text(json.dumps(model), encoding="utf-8")
            manual_path.write_text("### Step 99.1 · Empty (L1)\n", encoding="utf-8")

            with self.assertRaisesRegex(ValueError, "explicitly authored block placements"):
                chapter_indexer.compile_manual(manual_path, model_path)

    def test_new_chapter_scaffold_creates_matching_manual_and_model(self):
        original_data_dir = chapter_indexer.DATA_DIR
        original_manuals_dir = chapter_indexer.MANUALS_DIR
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            chapter_indexer.DATA_DIR = root / "data"
            chapter_indexer.MANUALS_DIR = root / "manuals"
            chapter_indexer.DATA_DIR.mkdir()
            try:
                chapter_indexer.create_chapter(
                    99, "test-drone", "TEST-99", "Test Drone", "Small", "Survey"
                )
                model = chapter_indexer.read_json(chapter_indexer.DATA_DIR / "test-drone.json")
                manual = (chapter_indexer.MANUALS_DIR / "99-test-drone.md").read_text(encoding="utf-8")
                self.assertEqual(model["blockMeters"], 0.5)
                self.assertEqual(model["steps"], [])
                self.assertIn("### Step 99.1", manual)
                with self.assertRaisesRegex(ValueError, "model ID .* already exists"):
                    chapter_indexer.create_chapter(
                        100, "test-drone-2", "TEST-99", "Duplicate", "Small", "Survey"
                    )
            finally:
                chapter_indexer.DATA_DIR = original_data_dir
                chapter_indexer.MANUALS_DIR = original_manuals_dir


if __name__ == "__main__":
    unittest.main()
