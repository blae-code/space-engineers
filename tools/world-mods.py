#!/usr/bin/env python3
"""Show or set a Space Engineers world's mod list (Sandbox_config.sbc AND Sandbox.sbc).

  python3 tools/world-mods.py --list                      # worlds on this machine
  python3 tools/world-mods.py "RIG Test" --show           # current mod list
  python3 tools/world-mods.py "RIG Test" --set docs/rig/testworld-mods.txt

--set backs up both files (suffix .bak-YYYYmmdd-HHMMSS), replaces the <Mods> block in each, and
refuses to run while Space Engineers is running (the game would overwrite the change on save).
Mod list file: one mod per line, 'id<TAB>name', '#' lines ignored.
"""
import argparse, glob, html, os, re, subprocess, sys, time

SAVES = os.path.expanduser("~/.local/share/Steam/steamapps/compatdata/244850/pfx/drive_c/users/"
                           "steamuser/AppData/Roaming/SpaceEngineers/Saves")
FILES = ("Sandbox_config.sbc", "Sandbox.sbc")
MODS_RE = re.compile(r"( *)<Mods>[\s\S]*?</Mods>|( *)<Mods\s*/>")


def worlds():
    return sorted(glob.glob(os.path.join(SAVES, "*", "*", "Sandbox_config.sbc")))


def find_world(name):
    hits = [os.path.dirname(p) for p in worlds() if os.path.basename(os.path.dirname(p)) == name]
    if len(hits) != 1:
        sys.exit(f"world {name!r}: {len(hits)} matches; use --list")
    return hits[0]


def read_list(path):
    mods = []
    for line in open(path, encoding="utf-8"):
        line = line.rstrip("\n")
        if not line.strip() or line.startswith("#"):
            continue
        parts = line.split("\t", 1)
        mods.append((parts[0].strip(), parts[1].strip() if len(parts) > 1 else parts[0].strip()))
    return mods


def mods_block(mods, indent):
    out = [f"{indent}<Mods>"]
    for mid, name in mods:
        out += [f'{indent}  <ModItem FriendlyName="{html.escape(name, quote=True)}">',
                f"{indent}    <Name>{mid}.sbm</Name>",
                f"{indent}    <PublishedFileId>{mid}</PublishedFileId>",
                f"{indent}    <PublishedServiceName>Steam</PublishedServiceName>",
                f"{indent}  </ModItem>"]
    out.append(f"{indent}</Mods>")
    return "\n".join(out)


def game_running():
    r = subprocess.run(["pgrep", "-f", "SpaceEngineers.exe"], capture_output=True)
    return r.returncode == 0


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("world", nargs="?")
    g = ap.add_mutually_exclusive_group(required=True)
    g.add_argument("--list", action="store_true")
    g.add_argument("--show", action="store_true")
    g.add_argument("--set", metavar="MODLIST")
    a = ap.parse_args()

    if a.list:
        for p in worlds():
            d = os.path.dirname(p)
            n = open(p, encoding="utf-8", errors="ignore").read().count("<ModItem")
            print(f"{os.path.basename(d):40} mods={n:3}  saved {time.strftime('%Y-%m-%d %H:%M', time.localtime(os.path.getmtime(p)))}")
        return
    if not a.world:
        sys.exit("world name required")
    wdir = find_world(a.world)

    if a.show:
        s = open(os.path.join(wdir, FILES[0]), encoding="utf-8").read()
        for name, mid in re.findall(r'<ModItem FriendlyName="([^"]*)">[\s\S]*?<PublishedFileId>(\d+)', s):
            print(f"{mid}\t{html.unescape(name)}")
        return

    if game_running():
        sys.exit("Space Engineers is running: close it first (it rewrites these files on save)")
    mods = read_list(a.set)
    stamp = time.strftime("%Y%m%d-%H%M%S")
    for fn in FILES:
        path = os.path.join(wdir, fn)
        s = open(path, encoding="utf-8").read()
        m = MODS_RE.search(s)
        if not m:
            sys.exit(f"{fn}: no <Mods> element found; nothing changed")
        indent = m.group(1) if m.group(1) is not None else (m.group(2) or "")
        new = s[:m.start()] + mods_block(mods, indent) + s[m.end():]
        open(path + ".bak-" + stamp, "w", encoding="utf-8").write(s)
        open(path, "w", encoding="utf-8").write(new)
        print(f"{fn}: {s.count('<ModItem')} → {new.count('<ModItem')} mods (backup .bak-{stamp})")


if __name__ == "__main__":
    main()
