#!/usr/bin/env fish
# Size gate for the deployed PB script: the in-game limit is 100,000 characters.
# FAIL (exit 1) at >= 100000, WARN at >= 90000. Run after a Release build.
set -l root (status dirname)/..
set -l ini $root/Fleet.Drone.Miner/mdk.local.ini
set -l out (string replace -r '^output=' '' (string match -r '^output=.*' < $ini))
if test -z "$out"
    echo "FAIL  no output= line in $ini"
    exit 1
end
set -l script "$out/Fleet.Drone.Miner/script.cs"
if not test -f "$script"
    echo "FAIL  $script not found — run: dotnet build Fleet.Drone.Miner -c Release"
    exit 1
end
set -l chars (wc -m < "$script" | string trim)
if test "$chars" -ge 100000
    echo "FAIL  script.cs is $chars chars (limit 100000)"
    exit 1
else if test "$chars" -ge 90000
    echo "WARN  script.cs is $chars chars (warning line 90000, limit 100000)"
else
    echo "OK    script.cs is $chars chars (limit 100000)"
end
