import json
import struct
import unittest
from pathlib import Path


STUDIO_DIR = Path(__file__).resolve().parents[1]


class PwaAssetTests(unittest.TestCase):
    def test_manifest_contains_installable_icons_and_navigation_scope(self):
        manifest = json.loads((STUDIO_DIR / "manifest.json").read_text(encoding="utf-8"))
        self.assertEqual(manifest["display"], "standalone")
        self.assertEqual(manifest["scope"], "./")
        self.assertEqual(manifest["start_url"], "./index.html")

        icons = {
            icon["sizes"]: icon
            for icon in manifest["icons"]
            if icon["type"] == "image/png"
        }
        for size in ("192x192", "512x512"):
            with self.subTest(size=size):
                icon = icons[size]
                icon_path = STUDIO_DIR / icon["src"]
                self.assertTrue(icon_path.is_file())
                data = icon_path.read_bytes()
                self.assertEqual(data[:8], b"\x89PNG\r\n\x1a\n")
                self.assertEqual(struct.unpack(">II", data[16:24]), tuple(map(int, size.split("x"))))

        registry = json.loads((STUDIO_DIR / "data/chapters.json").read_text(encoding="utf-8"))
        for chapter in registry:
            with self.subTest(chapter=chapter["file"]):
                self.assertTrue((STUDIO_DIR / "data" / chapter["file"]).is_file())

    def test_document_links_install_metadata_and_pwa_controls(self):
        html = (STUDIO_DIR / "index.html").read_text(encoding="utf-8")
        self.assertIn('<link rel="manifest" href="./manifest.json">', html)
        self.assertIn('<link rel="apple-touch-icon" href="./icon-180.png">', html)
        self.assertIn('id="btn-install-app"', html)
        self.assertIn('id="btn-update-app"', html)
        self.assertIn('id="pwa-status"', html)


if __name__ == "__main__":
    unittest.main()
