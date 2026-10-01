#!/usr/bin/env fish
# Size gate for every deployed PB script: the in-game limit is 100,000 characters.
# FAIL (exit 1) at >= 100000, WARN at >= 90000. Run after a Release build.
set -l root (status dirname)/..
set -l failed 0
for proj in Fleet.Drone.Miner Fleet.Console
    set -l ini $root/$proj/mdk.local.ini
    if not test -f $ini
        echo "SKIP  $proj: no mdk.local.ini"
        continue
    end
    set -l out (string replace -r '^output=' '' (string match -r '^output=.*' < $ini))
    set -l script "$out/$proj/script.cs"
    if not test -f "$script"
        echo "FAIL  $proj: $script not found — run: dotnet build $proj -c Release"
        set failed 1
        continue
    end
    set -l chars (wc -m < "$script" | string trim)
    if test "$chars" -ge 100000
        echo "FAIL  $proj script.cs is $chars chars (limit 100000)"
        set failed 1
    else if test "$chars" -ge 90000
        echo "WARN  $proj script.cs is $chars chars (warning line 90000, limit 100000)"
    else
        echo "OK    $proj script.cs is $chars chars (limit 100000)"
    end
end
exit $failed
