#!/usr/bin/env python3
"""Inspect a Space Engineers blueprint (bp.sbc) for design review.

  python3 tools/bp-inspect.py "My Crawler"                # by name, from Blueprints/local
  python3 tools/bp-inspect.py path/to/bp.sbc --svg out.html
  python3 tools/bp-inspect.py "My Crawler" --json         # machine-readable

Report: grids and how they join (rotor/hinge/piston chains), mass and centre of mass from the game's
own component masses, overall size, power supply vs. listed consumers, wheels, cargo, item-flow
reach across joints, naming and groups, PBs, and every block not in vanilla + DLC (with its mod).
--svg writes plan / side / front silhouettes (assembled across subgrids) as an HTML fragment.

Assumptions (marked † in the report): blocks are complete (blueprints are); conveyor networks inside
one grid are treated as connected; hinges do not pass items.
"""
import argparse, glob, html, json, math, os, re, sys
import xml.etree.ElementTree as ET

STEAM = os.path.expanduser("~/.local/share/Steam/steamapps")
GAME_DATA = os.path.join(STEAM, "common/SpaceEngineers/Content/Data")
WORKSHOP = os.path.join(STEAM, "workshop/content/244850")
APPDATA = os.path.join(STEAM, "compatdata/244850/pfx/drive_c/users/steamuser/AppData/Roaming/SpaceEngineers")
CACHE = os.path.expanduser("~/.cache/rig-bp-inspect-defs.json")
XSI = "{http://www.w3.org/2001/XMLSchema-instance}type"
BLOCK_M = {"Large": 2.5, "Small": 0.5}

DIRS = {"Forward": (0, 0, -1), "Backward": (0, 0, 1), "Left": (-1, 0, 0), "Right": (1, 0, 0),
        "Up": (0, 1, 0), "Down": (0, -1, 0)}

# ---------------------------------------------------------------- definitions index (vanilla + mods)

def _sbc_texts(root):
    for f in glob.glob(os.path.join(root, "**", "*.sbc"), recursive=True):
        try:
            yield f, open(f, encoding="utf-8", errors="ignore").read()
        except OSError:
            pass


def _resx(path):
    names = {}
    if os.path.exists(path):
        s = open(path, encoding="utf-8", errors="ignore").read()
        for k, v in re.findall(r'<data name="([^"]+)"[^>]*>\s*<value>([^<]*)</value>', s):
            names[k] = v
    return names


def build_defs(mod_ids):
    loc = _resx(os.path.join(GAME_DATA, "Localization", "MyTexts.resx"))
    comps, blocks = {}, {}
    sources = [("vanilla", GAME_DATA)] + [(m, os.path.join(WORKSHOP, m)) for m in mod_ids]
    for src, root in sources:
        for f, s in _sbc_texts(root):
            for m in re.finditer(r"<Component>\s*<Id>\s*<TypeId>Component</TypeId>\s*<SubtypeId>(\w+)</SubtypeId>[\s\S]*?<Mass>([\d.]+)</Mass>", s):
                comps.setdefault(m.group(1), float(m.group(2)))
            if "<CubeBlocks>" not in s and "CubeBlockDefinition" not in s:
                continue
            for d in re.findall(r"<Definition\b[^>]*>([\s\S]*?)</Definition>", s):
                idm = re.search(r"<Id>\s*<TypeId>\s*(?:MyObjectBuilder_)?(\w+)\s*</TypeId>\s*(?:<SubtypeId>\s*([^<]*?)\s*</SubtypeId>|<SubtypeId\s*/>)?", d)
                if not idm or "<CubeSize>" not in d:
                    continue
                key = idm.group(1) + "/" + (idm.group(2) or "")
                size = re.search(r'<Size x="(\d+)" y="(\d+)" z="(\d+)"', d)
                dn = re.search(r"<DisplayName>([^<]*)</DisplayName>", d)
                inv = re.search(r"<InventorySize>\s*<X>([\d.]+)</X>\s*<Y>([\d.]+)</Y>\s*<Z>([\d.]+)</Z>", d)
                def num(tag):
                    m = re.search(rf"<{tag}>([\d.]+)</{tag}>", d)
                    return float(m.group(1)) if m else None
                cube = re.search(r"<CubeSize>(\w+)</CubeSize>", d)
                blocks[key] = {
                    "src": src, "cube": cube.group(1) if cube else "Large",
                    "name": loc.get(dn.group(1), dn.group(1)) if dn else key,
                    "size": [int(x) for x in size.groups()] if size else [1, 1, 1],
                    "comps": [(c, int(n)) for c, n in re.findall(r'<Component Subtype="(\w+)" Count="(\d+)"', d)],
                    "inv_l": (float(inv.group(1)) * float(inv.group(2)) * float(inv.group(3)) * 1000) if inv else None,
                    "out_mw": num("MaxPowerOutput"),
                    "in_mw": num("RequiredPowerInput") or num("OperationalPowerConsumption"),
                    "force_n": num("ForceMagnitude"),
                }
    for b in blocks.values():
        b["mass"] = sum(comps.get(c, 0.0) * n for c, n in b.pop("comps"))
    return blocks


def load_defs(mod_ids, rebuild=False):
    key = sorted(mod_ids)
    if not rebuild and os.path.exists(CACHE):
        try:
            c = json.load(open(CACHE))
            if c.get("mods") == key and c.get("game_mtime") == os.path.getmtime(GAME_DATA):
                return c["blocks"]
        except (OSError, ValueError):
            pass
    blocks = build_defs(mod_ids)
    os.makedirs(os.path.dirname(CACHE), exist_ok=True)
    json.dump({"mods": key, "game_mtime": os.path.getmtime(GAME_DATA), "blocks": blocks}, open(CACHE, "w"))
    return blocks


def mod_titles():
    p = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "docs", "rig", "testworld-mods.txt")
    t = {}
    if os.path.exists(p):
        for line in open(p, encoding="utf-8"):
            if line.strip() and not line.startswith("#"):
                a, _, b = line.rstrip("\n").partition("\t")
                t[a.strip()] = b.strip()
    return t

# ---------------------------------------------------------------- geometry

def v_add(a, b): return (a[0] + b[0], a[1] + b[1], a[2] + b[2])
def v_mul(a, k): return (a[0] * k, a[1] * k, a[2] * k)
def cross(a, b): return (a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0])
def dot(a, b): return a[0] * b[0] + a[1] * b[1] + a[2] * b[2]
def norm(a):
    l = math.sqrt(dot(a, a)) or 1.0
    return (a[0] / l, a[1] / l, a[2] / l)


def extents(size, fwd, up):
    """World-axis extents (in cells) of a block with local size, given its Forward/Up directions."""
    f, u = DIRS[fwd], DIRS[up]
    r = cross(f, u)
    e = [0, 0, 0]
    for local, d in ((size[0], r), (size[1], u), (size[2], f)):
        for i in range(3):
            if d[i]:
                e[i] = local
    return e


class Grid:
    def __init__(self, el):
        self.el = el
        self.size = el.findtext("GridSizeEnum") or "Large"
        self.m = BLOCK_M.get(self.size, 2.5)
        self.name = el.findtext("DisplayName") or el.findtext("EntityId")
        pos = el.find("PositionAndOrientation")
        p = pos.find("Position") if pos is not None else None
        self.pos = (float(p.get("x")), float(p.get("y")), float(p.get("z"))) if p is not None else (0, 0, 0)
        def vec(tag, dflt):
            v = pos.find(tag) if pos is not None else None
            return norm((float(v.get("x")), float(v.get("y")), float(v.get("z")))) if v is not None else dflt
        self.fwd = vec("Forward", (0, 0, -1))
        self.up = vec("Up", (0, 1, 0))
        self.right = cross(self.fwd, self.up)
        self.blocks = []

    def to_world(self, local_m):
        x, y, z = local_m  # grid local: +X right, +Y up, +Z backward
        w = v_add(v_add(v_mul(self.right, x), v_mul(self.up, y)), v_mul(self.fwd, -z))
        return v_add(self.pos, w)


def parse_blueprint(path):
    raw = open(path, "rb").read()
    if raw[:2] == b"\x1f\x8b":  # Steam Cloud stores blueprints gzip-compressed
        import gzip
        raw = gzip.decompress(raw)
    root = ET.fromstring(raw)
    bp = root.find(".//ShipBlueprint")
    title = bp.find("Id").get("Subtype") if bp is not None and bp.find("Id") is not None else os.path.basename(os.path.dirname(path))
    grids = [Grid(g) for g in root.iter("CubeGrid")]
    for g in grids:
        cb = g.el.find("CubeBlocks")
        for b in (cb if cb is not None else []):
            t = (b.get(XSI) or "MyObjectBuilder_CubeBlock").replace("MyObjectBuilder_", "")
            st = b.findtext("SubtypeName") or ""
            mn = b.find("Min")
            ori = b.find("BlockOrientation")
            g.blocks.append({
                "type": t, "sub": st, "key": f"{t}/{st}",
                "min": (int(mn.get("x", 0)), int(mn.get("y", 0)), int(mn.get("z", 0))) if mn is not None else (0, 0, 0),
                "fwd": ori.get("Forward", "Forward") if ori is not None else "Forward",
                "up": ori.get("Up", "Up") if ori is not None else "Up",
                "name": b.findtext("CustomName"),
                "eid": b.findtext("EntityId"),
                "top": b.findtext("TopBlockId"),
                "custom_data": bool((b.findtext("CustomData") or "").strip()),
                "program": bool((b.findtext("Program") or "").strip()),
            })
        g.groups = [(bg.findtext("Name"), len(bg.findall(".//Vector3I"))) for bg in g.el.iter("MyObjectBuilder_BlockGroup")]
    return title, grids

# ---------------------------------------------------------------- analysis

JOINT_BASE = {"MotorStator": "rotor", "MotorAdvancedStator": "adv rotor", "ExtendedPistonBase": "piston",
              "MotorSuspension": "wheel"}
ITEM_PASSING = {"adv rotor": True, "piston": True, "rotor": False, "hinge": False, "wheel": False}

CATS = [  # (category, colour, matcher on TypeId)
    ("wheel", "#222222", {"MotorSuspension", "Wheel", "RealWheel"}),
    ("joint", "#27ae60", {"MotorStator", "MotorAdvancedStator", "MotorRotor", "MotorAdvancedRotor", "ExtendedPistonBase", "PistonTop"}),
    ("cargo", "#d4a017", {"CargoContainer"}),
    ("item flow", "#2471a3", {"Conveyor", "ConveyorConnector", "ConveyorSorter", "ShipConnector", "Collector"}),
    ("tool", "#c0392b", {"Drill", "ShipGrinder", "ShipWelder"}),
    ("power", "#8e44ad", {"BatteryBlock", "Reactor", "HydrogenEngine", "SolarPanel", "WindTurbine"}),
    ("gas", "#16a085", {"OxygenTank", "HydrogenTank", "OxygenGenerator"}),
    ("thrust", "#e67e22", {"Thrust"}),
    ("gear", "#7f8c8d", {"LandingGear", "MagneticPlate"}),
    ("control", "#f1c40f", {"Cockpit", "RemoteControl", "MyProgrammableBlock", "TimerBlock", "EventControllerBlock",
                             "SensorBlock", "ButtonPanel", "TextPanel", "LCDPanelsBlock", "CameraBlock", "OreDetector",
                             "RadioAntenna", "Beacon", "LaserAntenna", "Gyro", "ShipController"}),
]
ARMOR = ("armour", "#95a5a6")
MODDED = ("modded", "#ff2fd0")


def category(b, d):
    if d and d["src"] != "vanilla":
        return MODDED
    for name, col, types in CATS:
        if b["type"] in types:
            if b["type"] == "MotorAdvancedStator" and "Hinge" in b["sub"]:
                return ("joint", col)
            return (name, col)
    return ARMOR


def joint_kind(b):
    k = JOINT_BASE.get(b["type"])
    if b["type"] == "MotorAdvancedStator" and "Hinge" in b["sub"]:
        k = "hinge"
    return k


def analyse(title, grids, defs, titles):
    eid_grid = {}
    for gi, g in enumerate(grids):
        for b in g.blocks:
            if b["eid"]:
                eid_grid[b["eid"]] = gi
    rep = {"title": title, "grids": [], "joints": [], "modded": {}, "unknown": {}, "names": {"named": 0, "tagged": 0},
           "pbs": [], "power": {"supply_mw": 0.0, "by": {}}, "wheels": {}, "cargo_l": 0.0, "cargo_unknown": 0,
           "groups": [], "thrust": {}}
    tot_m, com = 0.0, (0.0, 0.0, 0.0)
    cells = []  # for svg: (world_min, world_max, colour)
    for gi, g in enumerate(grids):
        gm, mn, mx = 0.0, [1e9] * 3, [-1e9] * 3
        for b in g.blocks:
            d = defs.get(b["key"])
            if d is None:
                rep["unknown"][b["key"]] = rep["unknown"].get(b["key"], 0) + 1
                size, mass = [1, 1, 1], 0.0
            else:
                size, mass = d["size"], d["mass"]
                if d["src"] != "vanilla":
                    k = (d["src"], d["name"])
                    rep["modded"][k] = rep["modded"].get(k, 0) + 1
            e = extents(size, b["fwd"], b["up"])
            lo = [b["min"][i] * g.m - g.m / 2 for i in range(3)]
            hi = [(b["min"][i] + e[i]) * g.m - g.m / 2 for i in range(3)]
            for i in range(3):
                mn[i] = min(mn[i], b["min"][i]); mx[i] = max(mx[i], b["min"][i] + e[i] - 1)
            c_local = tuple((lo[i] + hi[i]) / 2 for i in range(3))
            c_world = g.to_world(c_local)
            gm += mass
            com = v_add(com, v_mul(c_world, mass))
            corners = [g.to_world((x, y, z)) for x in (lo[0], hi[0]) for y in (lo[1], hi[1]) for z in (lo[2], hi[2])]
            cells.append((corners, category(b, d)[1]))
            # tallies
            if b["name"]:
                rep["names"]["named"] += 1
                if re.match(r"^\[[^\]]+\]", b["name"]):
                    rep["names"]["tagged"] += 1
            if b["type"] == "MyProgrammableBlock":
                rep["pbs"].append({"name": b["name"], "program": b["program"], "custom_data": b["custom_data"]})
            if d and d.get("out_mw") and b["type"] in ("BatteryBlock", "Reactor", "HydrogenEngine", "SolarPanel", "WindTurbine"):
                rep["power"]["supply_mw"] += d["out_mw"]
                rep["power"]["by"][d["name"]] = rep["power"]["by"].get(d["name"], 0) + 1
            if b["type"] == "MotorSuspension":
                nm = d["name"] if d else b["sub"]
                rep["wheels"][nm] = rep["wheels"].get(nm, 0) + 1
            if b["type"] == "Thrust":
                nm = d["name"] if d else b["sub"]
                rep["thrust"][nm] = rep["thrust"].get(nm, 0) + 1
            if b["type"] == "CargoContainer":
                if d and d.get("inv_l"):
                    rep["cargo_l"] += d["inv_l"]
                elif d:  # game rule when no InventorySize: cells x cell volume (matches 421,875 L large cargo)
                    cm = BLOCK_M.get(d.get("cube", "Large"), 2.5)
                    rep["cargo_l"] += d["size"][0] * d["size"][1] * d["size"][2] * cm ** 3 * 1000
                    rep["cargo_derived"] = True
                else:
                    rep["cargo_unknown"] += 1
            jk = joint_kind(b)
            if jk and jk != "wheel":
                rep["joints"].append({"kind": jk, "name": b["name"] or (d["name"] if d else b["key"]), "from": gi,
                                      "to": eid_grid.get(b["top"]) if b["top"] and b["top"] != "0" else None})
        tot_m += gm
        dims = [(mx[i] - mn[i] + 1) for i in range(3)] if g.blocks else [0, 0, 0]
        rep["grids"].append({"i": gi, "name": g.name, "size": g.size, "blocks": len(g.blocks), "mass_kg": round(gm),
                             "dims_blocks": dims, "dims_m": [round(x * g.m, 1) for x in dims]})
        for gn, n in g.groups:
            rep["groups"].append({"grid": gi, "name": gn, "blocks": n})
    rep["mass_kg"] = round(tot_m)
    com = v_mul(com, 1.0 / tot_m) if tot_m else (0, 0, 0)
    # express everything in the main (largest) grid's frame
    main = max(range(len(grids)), key=lambda i: len(grids[i].blocks)) if grids else 0
    G = grids[main]
    def to_main(w):
        d = (w[0] - G.pos[0], w[1] - G.pos[1], w[2] - G.pos[2])
        return (dot(d, G.right), dot(d, G.up), -dot(d, G.fwd))
    rep["main_grid"] = main
    rep["com_m"] = [round(x, 2) for x in to_main(com)]
    pts = []
    out_cells = []
    for corners, col in cells:
        cs = [to_main(c) for c in corners]
        a = [min(c[i] for c in cs) for i in range(3)]; b = [max(c[i] for c in cs) for i in range(3)]
        out_cells.append((a, b, col)); pts += [a, b]
    if pts:
        rep["bbox_m"] = [round(max(p[i] for p in pts) - min(p[i] for p in pts), 1) for i in range(3)]
        rep["bbox_min"] = [min(p[i] for p in pts) for i in range(3)]
    # item flow reach: grids joined by item-passing joints form one network
    parent = list(range(len(grids)))
    def find(x):
        while parent[x] != x:
            parent[x] = parent[parent[x]]; x = parent[x]
        return x
    for j in rep["joints"]:
        if j["to"] is not None and ITEM_PASSING.get(j["kind"]):
            parent[find(j["from"])] = find(j["to"])
    nets = {}
    for gi, g in enumerate(grids):
        r = find(gi)
        n = nets.setdefault(r, {"grids": [], "has": set()})
        n["grids"].append(gi)
        for b in g.blocks:
            d = defs.get(b["key"])
            c = category(b, d)[0]
            if c in ("cargo", "tool", "modded") or b["type"] == "ShipConnector":
                n["has"].add("connector" if b["type"] == "ShipConnector" else (d["name"] if c == "modded" and d else c))
    rep["item_networks"] = [{"grids": n["grids"], "has": sorted(n["has"])} for n in nets.values()]
    rep["_cells"] = out_cells
    return rep

# ---------------------------------------------------------------- output

def fmt_report(rep, titles):
    L = [f"# {rep['title']}", ""]
    bb = rep.get("bbox_m", [0, 0, 0])
    L.append(f"**Mass** {rep['mass_kg']/1000:,.1f} t dry · **size** {bb[0]} W × {bb[1]} H × {bb[2]} L m · "
             f"**CoM** x {rep['com_m'][0]} · y {rep['com_m'][1]} · z {rep['com_m'][2]} m (main-grid frame, +y up, +z aft)")
    L.append("")
    L.append("| Grid | Size | Blocks | Mass | Dims (blocks W×H×L) | Dims m |")
    L.append("| --- | --- | ---: | ---: | --- | --- |")
    for g in rep["grids"]:
        star = " ★ main" if g["i"] == rep["main_grid"] else ""
        L.append(f"| {g['i']} {g['name']}{star} | {g['size']} | {g['blocks']} | {g['mass_kg']/1000:,.1f} t | "
                 f"{'×'.join(map(str, g['dims_blocks']))} | {'×'.join(map(str, g['dims_m']))} |")
    if rep["joints"]:
        L += ["", "**Joints** (base grid → top grid; item flow across the joint)", ""]
        for j in rep["joints"]:
            flow = {True: "items ✓", False: "items ✗" + ("†" if j["kind"] == "hinge" else "")}[bool(ITEM_PASSING.get(j["kind"]))]
            to = j["to"] if j["to"] is not None else "— (no head)"
            L.append(f"- {j['kind']} `{j['name']}`: grid {j['from']} → {to} · {flow}")
    L += ["", "**Item networks** (grids reachable by conveyor; in-grid connectivity assumed†)", ""]
    for n in rep["item_networks"]:
        L.append(f"- grids {n['grids']}: {', '.join(n['has']) or 'no cargo/tool/connector'}")
    p = rep["power"]
    L += ["", f"**Power supply** {p['supply_mw']:.1f} MW max · " + (", ".join(f"{v}× {k}" for k, v in p["by"].items()) or "none")]
    if rep["wheels"]:
        L.append("**Wheels** " + ", ".join(f"{v}× {k}" for k, v in rep["wheels"].items()))
    if rep["thrust"]:
        L.append("**Thrusters** " + ", ".join(f"{v}× {k}" for k, v in rep["thrust"].items()))
    cargo = f"{rep['cargo_l']:,.0f} L" + ("†" if rep.get("cargo_derived") else "") + (f" + {rep['cargo_unknown']} unknown container(s)" if rep["cargo_unknown"] else "")
    L.append(f"**Cargo** {cargo} (at 1× inventory)")
    L.append(f"**PBs** {len(rep['pbs'])}" + "".join(f" · `{b['name']}` (program {'yes' if b['program'] else 'no'}, Custom Data {'yes' if b['custom_data'] else 'no'})" for b in rep["pbs"]))
    nm = rep["names"]
    L.append(f"**Names** {nm['named']} renamed blocks, {nm['tagged']} with a `[ID]` tag · **groups** " +
             (", ".join(f"`{g['name']}` ({g['blocks']})" for g in rep["groups"]) or "none"))
    if rep["modded"]:
        L += ["", "**Non-vanilla blocks**", ""]
        for (src, name), n in sorted(rep["modded"].items()):
            L.append(f"- {n}× {name} · mod {src} {titles.get(src, '')}")
    if rep["unknown"]:
        L += ["", "**Unknown blocks** (no definition found: a mod not in the test-world list?)", ""]
        for k, n in sorted(rep["unknown"].items()):
            L.append(f"- {n}× `{k}`")
    return "\n".join(L)


def fmt_svg(rep):
    cells = rep["_cells"]
    if not cells:
        return "<p>no blocks</p>"
    lo = [min(c[0][i] for c in cells) for i in range(3)]
    hi = [max(c[1][i] for c in cells) for i in range(3)]
    span = [hi[i] - lo[i] or 1 for i in range(3)]
    views = [("Plan (from above) · x → , z ↓ aft", 0, 2, 1, False),
             ("Side (from starboard) · z → aft, y ↑", 2, 1, 0, True),
             ("Front (from bow) · x →, y ↑", 0, 1, 2, True)]
    W = 300
    out = [f"<h2>{html.escape(rep['title'])}</h2>",
           f"<p class='subtitle'>{rep['mass_kg']/1000:,.1f} t dry · {rep['bbox_m'][0]} × {rep['bbox_m'][1]} × {rep['bbox_m'][2]} m (W × H × L) · ◆ = centre of mass</p>",
           "<div style='display:flex;gap:16px;flex-wrap:wrap'>"]
    for label, a, b, depth, flip in views:
        s = W / max(span[a], span[b])
        w, h = span[a] * s, span[b] * s
        rects = []
        for c0, c1, col in sorted(cells, key=lambda c: (c[1][depth] if depth != 1 else c[1][1])):
            x = (c0[a] - lo[a]) * s
            y = ((hi[b] - c1[b]) if flip else (c0[b] - lo[b])) * s
            rects.append(f"<rect x='{x:.1f}' y='{y:.1f}' width='{max((c1[a]-c0[a])*s,0.6):.1f}' height='{max((c1[b]-c0[b])*s,0.6):.1f}' fill='{col}' fill-opacity='.75' stroke='#0003' stroke-width='.3'/>")
        cm = rep["com_m"]
        cx = (cm[a] - lo[a]) * s
        cy = ((hi[b] - cm[b]) if flip else (cm[b] - lo[b])) * s
        out.append(f"<div class='mockup'><div class='mockup-header'>{label}</div><div class='mockup-body'>"
                   f"<svg viewBox='-4 -4 {w+8:.0f} {h+8:.0f}' width='{min(w+8, 320):.0f}'>{''.join(rects)}"
                   f"<text x='{cx:.1f}' y='{cy+4:.1f}' text-anchor='middle' font-size='12' fill='#000'>◆</text></svg></div></div>")
    out.append("</div><p class='label'>Legend</p><p>" + " · ".join(
        f"<span style='color:{c}'>■</span> {n}" for n, c, _ in CATS) + f" · <span style='color:{ARMOR[1]}'>■</span> armour/other · "
        f"<span style='color:{MODDED[1]}'>■</span> modded</p>")
    return "\n".join(out)


def resolve(arg):
    if os.path.isfile(arg):
        return arg
    bases = [os.path.join(APPDATA, "Blueprints", "local"), os.path.join(APPDATA, "Blueprints", "cloud")]
    # cloud blueprints keep only thumb.png in AppData; the bp.sbc lives in Steam's userdata remote store
    bases += glob.glob(os.path.expanduser("~/.local/share/Steam/userdata/*/244850/remote/Blueprints/cloud"))
    for base in bases:
        p = os.path.join(base, arg, "bp.sbc")
        if os.path.isfile(p):
            return p
    sys.exit(f"blueprint {arg!r} not found (looked in Blueprints/local, Blueprints/cloud and Steam Cloud userdata)")


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("blueprint", help="blueprint name (Blueprints/local) or path to bp.sbc")
    ap.add_argument("--svg", metavar="OUT.html", help="write plan/side/front silhouettes (HTML fragment)")
    ap.add_argument("--json", action="store_true")
    ap.add_argument("--rebuild-defs", action="store_true", help="re-scan game + mod definitions")
    a = ap.parse_args()
    titles = mod_titles()
    defs = load_defs(list(titles.keys()), a.rebuild_defs)
    title, grids = parse_blueprint(resolve(a.blueprint))
    rep = analyse(title, grids, defs, titles)
    if a.svg:
        open(a.svg, "w", encoding="utf-8").write(fmt_svg(rep))
    if a.json:
        r = dict(rep); r.pop("_cells")
        r["modded"] = [{"mod": k[0], "block": k[1], "n": v} for k, v in rep["modded"].items()]
        print(json.dumps(r, indent=1, default=list))
    else:
        print(fmt_report(rep, titles))


if __name__ == "__main__":
    main()
