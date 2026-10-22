import json
import socketserver
import sys
import tempfile
import threading
import unittest
import urllib.error
import urllib.request
from pathlib import Path


STUDIO_DIR = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(STUDIO_DIR))
original_argv = sys.argv
sys.argv = ["serve.py", "0"]
import serve
sys.argv = original_argv


class StudioApiTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.notes_directory = tempfile.TemporaryDirectory()
        serve.NOTES_DIR = cls.notes_directory.name
        cls.server = socketserver.ThreadingTCPServer(("127.0.0.1", 0), serve.SE3DRequestHandler)
        cls.server.daemon_threads = True
        cls.thread = threading.Thread(target=cls.server.serve_forever, daemon=True)
        cls.thread.start()
        cls.base_url = f"http://127.0.0.1:{cls.server.server_address[1]}"

    @classmethod
    def tearDownClass(cls):
        cls.server.shutdown()
        cls.server.server_close()
        cls.thread.join(timeout=2)
        cls.notes_directory.cleanup()

    def request_json(self, path, method="GET", body=None):
        data = body.encode("utf-8") if body is not None else None
        request = urllib.request.Request(
            self.base_url + path,
            data=data,
            method=method,
            headers={"Content-Type": "text/plain; charset=utf-8"} if data is not None else {},
        )
        with urllib.request.urlopen(request) as response:
            return response.status, json.loads(response.read())

    def test_status_and_model_routes_use_registry(self):
        status, payload = self.request_json("/api/status")
        self.assertEqual(status, 200)
        self.assertEqual(len(payload["modelsAvailable"]), 10)
        status, model = self.request_json("/api/model/MAS-01")
        self.assertEqual(status, 200)
        self.assertEqual(model["id"], "MAS-01")

    def test_slice_validates_queries_and_preserves_bow_up_orientation(self):
        with urllib.request.urlopen(self.base_url + "/api/slice/MAS-01?layer=L2&step=0") as response:
            self.assertEqual(response.status, 200)
            body = response.read().decode("utf-8")
        self.assertIn("Bow (+Z)", body)
        with self.assertRaises(urllib.error.HTTPError) as error:
            urllib.request.urlopen(self.base_url + "/api/slice/MAS-01?step=invalid")
        self.assertEqual(error.exception.code, 400)
        error.exception.close()

    def test_notes_are_saved_and_read_by_model_id(self):
        status, payload = self.request_json(
            "/api/note/MAS-01?step=10.1",
            method="POST",
            body="Clearance verified",
        )
        self.assertEqual(status, 200)
        self.assertEqual(payload["status"], "saved")
        status, notes = self.request_json("/api/notes/MAS-01")
        self.assertEqual(status, 200)
        self.assertEqual(notes["10.1"]["text"], "Clearance verified")

    def test_unknown_model_is_not_resolved_by_prefix(self):
        with self.assertRaises(urllib.error.HTTPError) as error:
            urllib.request.urlopen(self.base_url + "/api/model/MA")
        self.assertEqual(error.exception.code, 404)
        error.exception.close()


if __name__ == "__main__":
    unittest.main()
