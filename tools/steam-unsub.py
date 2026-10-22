#!/usr/bin/env python3
"""Unsubscribe Space Engineers Workshop items listed in a file, through the running Steam client.

Uses the Steamworks client library (ISteamUGC::UnsubscribeItem), so no browser and no login token.
Steam must be running and logged in. While it runs, Steam shows you as playing Space Engineers.

  python3 tools/steam-unsub.py --check   # read-only: compare the list with your live subscriptions
  python3 tools/steam-unsub.py --one     # unsubscribe the first listed item, then verify
  python3 tools/steam-unsub.py --all     # unsubscribe every listed item, then verify

List file (default docs/rig/mods-unsubscribe.txt): one item per line, id first, '#' lines ignored.
Library: $STEAM_API_LIB, else the 64-bit Linux libsteam_api.so shipped with Stellaris.
"""
import argparse, ctypes, os, sys, tempfile, time

APP_ID = "244850"  # Space Engineers
DEFAULT_LIB = os.path.expanduser("~/.local/share/Steam/steamapps/common/Stellaris/libsteam_api.so")
DEFAULT_LIST = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "docs", "rig", "mods-unsubscribe.txt")


def load_ids(path):
    ids = []
    for line in open(path, encoding="utf-8"):
        line = line.strip()
        if not line or line.startswith("#"):
            continue
        ids.append(int(line.split()[0]))
    return ids


class Steam:
    def __init__(self, lib_path):
        self.lib = ctypes.CDLL(lib_path)
        L = self.lib
        L.SteamAPI_Init.restype = ctypes.c_bool
        L.SteamAPI_SteamUGC_v016.restype = ctypes.c_void_p
        L.SteamAPI_ISteamUGC_GetNumSubscribedItems.argtypes = [ctypes.c_void_p]
        L.SteamAPI_ISteamUGC_GetNumSubscribedItems.restype = ctypes.c_uint32
        L.SteamAPI_ISteamUGC_GetSubscribedItems.argtypes = [ctypes.c_void_p, ctypes.POINTER(ctypes.c_uint64), ctypes.c_uint32]
        L.SteamAPI_ISteamUGC_GetSubscribedItems.restype = ctypes.c_uint32
        L.SteamAPI_ISteamUGC_UnsubscribeItem.argtypes = [ctypes.c_void_p, ctypes.c_uint64]
        L.SteamAPI_ISteamUGC_UnsubscribeItem.restype = ctypes.c_uint64
        # SteamAPI_Init reads steam_appid.txt from the working directory.
        self._cwd = os.getcwd()
        self._tmp = tempfile.mkdtemp(prefix="steam-unsub-")
        with open(os.path.join(self._tmp, "steam_appid.txt"), "w") as f:
            f.write(APP_ID)
        os.chdir(self._tmp)
        os.environ["SteamAppId"] = APP_ID
        if not L.SteamAPI_Init():
            sys.exit("SteamAPI_Init failed: is Steam running and logged in?")
        self.ugc = L.SteamAPI_SteamUGC_v016()
        if not self.ugc:
            sys.exit("Could not get ISteamUGC")

    def subscribed(self):
        n = self.lib.SteamAPI_ISteamUGC_GetNumSubscribedItems(self.ugc)
        buf = (ctypes.c_uint64 * max(n, 1))()
        got = self.lib.SteamAPI_ISteamUGC_GetSubscribedItems(self.ugc, buf, n)
        return set(buf[i] for i in range(got))

    def unsubscribe(self, item):
        return self.lib.SteamAPI_ISteamUGC_UnsubscribeItem(self.ugc, item)

    def pump(self, seconds):
        end = time.time() + seconds
        while time.time() < end:
            self.lib.SteamAPI_RunCallbacks()
            time.sleep(0.1)

    def close(self):
        self.lib.SteamAPI_Shutdown()
        os.chdir(self._cwd)


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    mode = ap.add_mutually_exclusive_group(required=True)
    mode.add_argument("--check", action="store_true", help="read-only comparison")
    mode.add_argument("--one", action="store_true", help="unsubscribe the first listed item only")
    mode.add_argument("--all", action="store_true", help="unsubscribe every listed item")
    ap.add_argument("--list", default=DEFAULT_LIST)
    ap.add_argument("--lib", default=os.environ.get("STEAM_API_LIB", DEFAULT_LIB))
    a = ap.parse_args()

    listed = load_ids(a.list)
    steam = Steam(a.lib)
    try:
        before = steam.subscribed()
        targets = [i for i in listed if i in before]
        print(f"subscribed now: {len(before)} · listed: {len(listed)} · listed AND subscribed: {len(targets)}"
              f" · would remain: {len(before) - len(targets)}")
        if a.check:
            missing = [i for i in listed if i not in before]
            if missing:
                print(f"listed but not subscribed (skipped): {len(missing)}")
            return
        if a.one:
            targets = targets[:1]
        if not targets:
            print("nothing to do")
            return
        for i, item in enumerate(targets, 1):
            steam.unsubscribe(item)
            steam.pump(0.05)
            if i % 25 == 0:
                print(f"  sent {i}/{len(targets)}")
        print(f"sent {len(targets)} request(s); waiting for Steam to apply them ...")
        steam.pump(5)
        after = steam.subscribed()
        still = [t for t in targets if t in after]
        print(f"done: subscribed {len(before)} → {len(after)} · unsubscribed {len(targets) - len(still)}/{len(targets)}")
        if still:
            print("still subscribed (re-run --all to retry): " + " ".join(str(s) for s in still))
    finally:
        steam.close()


if __name__ == "__main__":
    main()
