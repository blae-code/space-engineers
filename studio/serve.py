#!/usr/bin/env python3
"""Local server for the Space Engineers 3D Studio and its read/write API.

Usage:
    python3 studio/serve.py [port]      # Defaults to 8080

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
import re
import socketserver
import sys
import urllib.parse
from datetime import datetime, timezone

PORT = int(sys.argv[1]) if len(sys.argv) > 1 else 8080
WEB_DIR = os.path.dirname(os.path.abspath(__file__))
DATA_DIR = os.path.join(WEB_DIR, "data")
NOTES_DIR = os.path.join(WEB_DIR, "notes")


def load_registry():
    with open(os.path.join(DATA_DIR, "chapters.json"), "r", encoding="utf-8") as registry_file:
        registry = json.load(registry_file)
    if not isinstance(registry, list) or any(
        not isinstance(entry, dict) or
        not isinstance(entry.get("id"), str) or
        not isinstance(entry.get("file"), str)
        for entry in registry
    ):
        raise ValueError("chapter registry must be an array of entries with an id and file")
    return registry


def resolve_model(model_key):
    if not re.fullmatch(r"[A-Za-z0-9_-]+", model_key):
        return None
    for entry in load_registry():
        if model_key in (entry.get("id"), os.path.splitext(entry.get("file", ""))[0]):
            filename = entry.get("file", "")
            if os.path.basename(filename) != filename or not filename.endswith(".json"):
                raise ValueError(f"invalid chapter model file in registry: {filename!r}")
            return os.path.join(DATA_DIR, filename)
    return None


def get_notes_path(model_key):
    if not re.fullmatch(r"[A-Za-z0-9_-]+", model_key):
        return None
    model_path = resolve_model(model_key)
    if model_path is None:
        return None
    model_id = os.path.splitext(os.path.basename(model_path))[0]
    return os.path.join(NOTES_DIR, f"notes_{model_id}.json")


class SE3DRequestHandler(http.server.SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=WEB_DIR, **kwargs)

    def send_json(self, value):
        body = json.dumps(value, indent=2, ensure_ascii=False).encode("utf-8")
        self.send_response(200)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def send_text(self, value):
        body = value.encode("utf-8")
        self.send_response(200)
        self.send_header("Content-Type", "text/plain; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def send_file(self, path, content_type):
        try:
            with open(path, "rb") as content_file:
                body = content_file.read()
        except OSError as error:
            self.send_error(500, f"Unable to read file: {error}")
            return
        self.send_response(200)
        self.send_header("Content-Type", f"{content_type}; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

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
            try:
                registry = load_registry()
            except (OSError, json.JSONDecodeError, ValueError) as error:
                self.send_error(500, f"Unable to load chapter registry: {error}")
                return
            resp = {
                "status": "online",
                "service": "Space Engineers 3D Studio",
                "modelsAvailable": [entry.get("id") for entry in registry if entry.get("id")],
            }
            self.send_json(resp)
            return

        if path == "/api/chapters":
            try:
                self.send_json(load_registry())
            except (OSError, json.JSONDecodeError, ValueError) as error:
                self.send_error(500, f"Unable to load chapter registry: {error}")
            return

        if path.startswith("/api/model/"):
            model_key = urllib.parse.unquote(path[len("/api/model/"):])
            if model_key.endswith(".json"):
                model_key = model_key[:-5]
            try:
                data_file = resolve_model(model_key)
            except (OSError, json.JSONDecodeError, ValueError) as error:
                self.send_error(500, f"Unable to resolve chapter model: {error}")
                return
            if data_file is None:
                self.send_error(404, "Model not found")
                return
            self.send_file(data_file, "application/json")
            return

        if path.startswith("/api/slice/"):
            model_key = urllib.parse.unquote(path[len("/api/slice/"):])
            if model_key.endswith(".json"):
                model_key = model_key[:-5]
            layer = query.get("layer", ["L2"])[0]
            raw_step = query.get("step", ["9999"])[0]
            if not re.fullmatch(r"L?\d+", layer):
                self.send_error(400, "Layer must be an integer or L-prefixed integer")
                return
            if not re.fullmatch(r"\d+", raw_step):
                self.send_error(400, "Step must be a non-negative integer")
                return
            try:
                data_file = resolve_model(model_key)
            except (OSError, json.JSONDecodeError, ValueError) as error:
                self.send_error(500, f"Unable to resolve chapter model: {error}")
                return
            if data_file is None:
                self.send_error(404, "Model not found")
                return
            try:
                with open(data_file, "r", encoding="utf-8") as model_file:
                    data = json.load(model_file)
            except (OSError, json.JSONDecodeError) as error:
                self.send_error(500, f"Unable to read chapter model: {error}")
                return

            level_num = int(layer.removeprefix("L"))
            if not data["bounds"]["min"][1] <= level_num <= data["bounds"]["max"][1]:
                self.send_error(400, "Layer is outside the model's declared vertical bounds")
                return
            step_limit = int(raw_step)
            min_x, max_x = data["bounds"]["min"][0], data["bounds"]["max"][0]
            min_z, max_z = data["bounds"]["min"][2], data["bounds"]["max"][2]
            grid = {}
            for s_idx, step in enumerate(data.get("steps", [])):
                if s_idx > step_limit:
                    break
                for block in step.get("blocks", []):
                    if block["pos"][1] == level_num:
                        grid[(block["pos"][0], block["pos"][2])] = block.get("name", block["shape"])

            lines = [
                f"SE 2D SLICE · {data['id']} · Level L{level_num} · Step Limit <= {step_limit}",
                "Orientation: ▲ North/Bow (+Z) · ◄ Port (-X) · Starboard (+X) ►",
            ]
            header = " Z \\ X  " + "".join((f" +{x}" if x >= 0 else f" {x}").ljust(4) for x in range(min_x, max_x + 1))
            lines.extend([header, "-" * len(header)])
            for z in range(max_z, min_z - 1, -1):
                row = (f" +{z}" if z >= 0 else f" {z}").rjust(5) + " │ "
                has_block = False
                for x in range(min_x, max_x + 1):
                    if (x, z) in grid:
                        row += " ■  "
                        has_block = True
                    else:
                        row += " ·  "
                if has_block or z % 5 == 0:
                    lines.append(row)
            self.send_text("\n".join(lines))
            return

        if path.startswith("/api/notes/"):
            model_key = urllib.parse.unquote(path[len("/api/notes/"):])
            try:
                notes_file = get_notes_path(model_key)
            except (OSError, json.JSONDecodeError, ValueError) as error:
                self.send_error(500, f"Unable to resolve notes: {error}")
                return
            if notes_file is None:
                self.send_error(404, "Model not found")
                return
            try:
                with open(notes_file, "r", encoding="utf-8") as notes_stream:
                    notes = json.load(notes_stream)
            except FileNotFoundError:
                notes = {}
            except (OSError, json.JSONDecodeError) as error:
                self.send_error(500, f"Unable to read notes: {error}")
                return
            self.send_json(notes)
            return

        return super().do_GET()

    def do_POST(self):
        parsed = urllib.parse.urlparse(self.path)
        path = parsed.path
        query = urllib.parse.parse_qs(parsed.query)

        if path.startswith("/api/note/"):
            model_key = urllib.parse.unquote(path[len("/api/note/"):])
            try:
                notes_file = get_notes_path(model_key)
            except (OSError, json.JSONDecodeError, ValueError) as error:
                self.send_error(500, f"Unable to resolve notes: {error}")
                return
            if notes_file is None:
                self.send_error(404, "Model not found")
                return

            step_id = query.get("step", ["general"])[0]
            if not step_id or len(step_id) > 128:
                self.send_error(400, "Step ID must contain 1 to 128 characters")
                return
            try:
                content_length = int(self.headers.get("Content-Length", 0))
            except ValueError:
                self.send_error(400, "Invalid Content-Length")
                return
            if content_length < 0 or content_length > 1_000_000:
                self.send_error(413, "Note body exceeds the 1 MB limit")
                return
            try:
                body = self.rfile.read(content_length).decode("utf-8").strip()
            except UnicodeDecodeError:
                self.send_error(400, "Note body must be UTF-8 text")
                return

            try:
                with open(notes_file, "r", encoding="utf-8") as notes_stream:
                    notes = json.load(notes_stream)
            except FileNotFoundError:
                notes = {}
            except (OSError, json.JSONDecodeError) as error:
                self.send_error(500, f"Unable to read notes: {error}")
                return
            if not isinstance(notes, dict):
                self.send_error(500, "Saved notes must be a JSON object")
                return

            notes[step_id] = {
                "text": body,
                "author": self.headers.get("X-Author", "Collaborator"),
                "timestamp": datetime.now(timezone.utc).isoformat(),
            }
            try:
                os.makedirs(NOTES_DIR, exist_ok=True)
                temporary_file = f"{notes_file}.tmp"
                with open(temporary_file, "w", encoding="utf-8") as notes_stream:
                    json.dump(notes, notes_stream, indent=2, ensure_ascii=False)
                    notes_stream.write("\n")
                os.replace(temporary_file, notes_file)
            except OSError as error:
                self.send_error(500, f"Unable to save notes: {error}")
                return

            self.send_json({"status": "saved", "step": step_id})
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
        print(f"Studio interface: http://localhost:{PORT}/")
        print(f"----------------------------------------------------------------")
        try:
            httpd.serve_forever()
        except KeyboardInterrupt:
            print("\nShutting down server.")


if __name__ == "__main__":
    run()
