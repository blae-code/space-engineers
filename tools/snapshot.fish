#!/usr/bin/env fish
# Refresh the paste-ready drone script in docs/build-specs/ from the deployed Release build, then run
# the size gate. Run after any `dotnet build Fleet.Drone.Miner -c Release` that changed the script;
# tools/checkpoint.py FAILs while the snapshot differs from what is deployed.
set -l root (status dirname)/..
set -l ini $root/Fleet.Drone.Miner/mdk.local.ini
if not test -f $ini
    echo "FAIL  no Fleet.Drone.Miner/mdk.local.ini"
    exit 1
end
set -l out (string replace -r '^output=' '' (string match -r '^output=.*' < $ini))
set -l script "$out/Fleet.Drone.Miner/script.cs"
if not test -f "$script"
    echo "FAIL  $script not found — run: dotnet build Fleet.Drone.Miner -c Release"
    exit 1
end
cp "$script" $root/docs/build-specs/mining-drone.script.cs
echo "OK    docs/build-specs/mining-drone.script.cs refreshed ("(wc -m < "$script" | string trim)" chars)"
fish $root/tools/check-size.fish
