#!/usr/bin/env python3
"""Local server for Space Engineers 3D Chapter Visualizer & AI Sight API.

Usage:
    python3 web/serve.py [port]      # Defaults to 8080

Endpoints:
    GET  /                                    # PWA Web Interface
    GET  /api/status                          # Service health
    GET  /api/chapters                        # Master fleet chapter registry
    GET  /api/model/:id                       # Full Voxel JSON schema (e.g. mastodon, orca)
    GET  /api/slice/:id?layer=L2&step=4       # 2D ASCII slice text for AI agents
    GET  /api/notes/:id                       # Get collaborative notes for model
    POST /api/note/:id?step=10.1              # Post collaborative note for model step
"""
import http.server
import json
import os
import socketserver
import sys
import urllib.parse

PORT = int(sys.argv[1]) if len(sys.argv) > 1 else 8080
WEB_DIR = os.path.dirname(os.path.abspath(__file__))
DATA_DIR = os.path.join(WEB_DIR, "data")


class SE3DRequestHandler(http.server.SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=WEB_DIR, **kwargs)

    def end_headers(self):
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        super().end_headers()

    def do_OPTIONS(self):
        self.send_response(200)
        self.end_headers()

    def do_GET(self):
        parsed = urllib.parse.urlparse(self.path)
        path = parsed.path
        query = urllib.parse.parse_qs(parsed.query)

        if path == "/api/status":
            self.send_response(200)
            self.send_header("Content-Type", "application/json")
            self.end_headers()
            resp = {
                "status": "online",
                "service": "Space Engineers 3D Chapter Studio & AI Sight API",
                "modelsAvailable": [f.replace(".json", "") for f in os.listdir(DATA_DIR) if f.endswith(".json") and f != "chapters.json"],
                "activeChapter": "CH 10 MASTODON-01 MKI"
            }
            self.wfile.write(json.dumps(resp, indent=2).encode("utf-8"))
            return

        if path == "/api/chapters":
            chap_file = os.path.join(DATA_DIR, "chapters.json")
            if os.path.exists(chap_file):
                self.send_response(200)
                self.send_header("Content-Type", "application/json")
                self.end_headers()
                with open(chap_file, "rb") as f:
                    self.wfile.write(f.read())
                return

        if path.startswith("/api/model/"):
            model_id = path.replace("/api/model/", "").replace(".json", "")
            data_file = os.path.join(DATA_DIR, f"{model_id}.json")
            if not os.path.exists(data_file):
                # Check with alias/prefix
                for f in os.listdir(DATA_DIR):
                    if f.startswith(model_id) and f.endswith(".json"):
                        data_file = os.path.join(DATA_DIR, f)
                        break

            if os.path.exists(data_file):
                self.send_response(200)
                self.send_header("Content-Type", "application/json")
                self.end_headers()
                with open(data_file, "rb") as f:
                    self.wfile.write(f.read())
                return
            else:
                self.send_response(404)
                self.end_headers()
                self.wfile.write(b"Model not found")
                return

        if path.startswith("/api/slice/"):
            model_id = path.replace("/api/slice/", "").replace(".json", "")
            layer = query.get("layer", ["L2"])[0]
            step_limit = int(query.get("step", [9999])[0])

            data_file = os.path.join(DATA_DIR, f"{model_id}.json")
            if not os.path.exists(data_file):
                for f in os.listdir(DATA_DIR):
                    if f.startswith(model_id) and f.endswith(".json"):
                        data_file = os.path.join(DATA_DIR, f)
                        break

            if os.path.exists(data_file):
                with open(data_file, "r", encoding="utf-8") as f:
                    data = json.load(f)

                level_num = int(layer.replace("L", "")) if "L" in layer else 2
                min_x = data["bounds"]["min"][0]
                max_x = data["bounds"]["max"][0]
                min_z = data["bounds"]["min"][2]
                max_z = data["bounds"]["max"][2]

                grid = {}
                for s_idx, step in enumerate(data.get("steps", [])):
                    if s_idx > step_limit:
                        break
                    for b in step.get("blocks", []):
                        if b["pos"][1] == level_num:
                            grid[(b["pos"][0], b["pos"][2])] = b.get("name", b["shape"])

                lines = []
                lines.append(f"SE 2D SLICE · {data['id']} · Level {layer} · Step Limit <= {step_limit}")
                lines.append("Orientation: ▲ North/Bow (+Z) · ◄ Port (-X) · Starboard (+X) ►")
                header = " Z \\ X  " + "".join((f" +{x}" if x >= 0 else f" {x}").ljust(4) for x in range(min_x, max_x + 1))
                lines.append(header)
                lines.append("-" * len(header))

                for z in range(max_z, min_z - 1, -1):
                    row = (f" +{z}" if z >= 0 else f" {z}").rjust(5) + " │ "
                    has_block = False
                    for x in range(min_x, max_x + 1):
                        name = grid.get((x, z))
                        if name:
                            row += " ■  "
                            has_block = True
                        else:
                            row += " ·  "
                    if has_block or z % 5 == 0:
                        lines.append(row)

                ascii_text = "\n".join(lines)
                self.send_response(200)
                self.send_header("Content-Type", "text/plain; charset=utf-8")
                self.end_headers()
                self.wfile.write(ascii_text.encode("utf-8"))
                return

        if path.startswith("/api/notes/"):
            model_id = path.replace("/api/notes/", "")
            notes_file = os.path.join(DATA_DIR, f"notes_{model_id}.json")
            notes = {}
            if os.path.exists(notes_file):
                with open(notes_file, "r", encoding="utf-8") as f:
                    notes = json.load(f)
            self.send_response(200)
            self.send_header("Content-Type", "application/json")
            self.end_headers()
            self.wfile.write(json.dumps(notes, indent=2).encode("utf-8"))
            return

        return super().do_GET()

    def do_POST(self):
        parsed = urllib.parse.urlparse(self.path)
        path = parsed.path
        query = urllib.parse.parse_qs(parsed.query)

        if path.startswith("/api/note/"):
            model_id = path.replace("/api/note/", "")
            step_id = query.get("step", ["general"])[0]

            content_length = int(self.headers.get("Content-Length", 0))
            body = self.rfile.read(content_length).decode("utf-8")

            notes_file = os.path.join(DATA_DIR, f"notes_{model_id}.json")
            notes = {}
            if os.path.exists(notes_file):
                try:
                    with open(notes_file, "r", encoding="utf-8") as f:
                        notes = json.load(f)
                except:
                    notes = {}

            notes[step_id] = {
                "text": body.strip(),
                "author": self.headers.get("X-Author", "Collaborator"),
                "timestamp": urllib.parse.quote(str(os.times()))
            }

            with open(notes_file, "w", encoding="utf-8") as f:
                json.dump(notes, f, indent=2)

            self.send_response(200)
            self.send_header("Content-Type", "application/json")
            self.end_headers()
            self.wfile.write(b'{"status": "saved"}')
            return

        self.send_response(404)
        self.end_headers()


def run():
    socketserver.TCPServer.allow_reuse_address = True
    with socketserver.TCPServer(("", PORT), SE3DRequestHandler) as httpd:
        print(f"----------------------------------------------------------------")
        print(f"SE 3D Studio & AI Sight Server listening on http://localhost:{PORT}")
        print(f"Serving local directory: {WEB_DIR}")
        print(f"Fleet Registry: http://localhost:{PORT}/api/chapters")
        print(f"Chapter 10 Mastodon: http://localhost:{PORT}/index.html")
        print(f"----------------------------------------------------------------")
        try:
            httpd.serve_forever()
        except KeyboardInterrupt:
            print("\nShutting down server.")


if __name__ == "__main__":
    run()
