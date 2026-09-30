# C04 — ConfigLoader (Custom Data ⇄ Settings)

Milestone A · Difficulty: medium · Executor: local

## Goal
Custom Data is the configuration source of truth. Parse it into a `Settings` object with range checks,
and write back a complete config (every key present) while preserving the user's values and any unknown
sections. Bad input never throws: it falls back to the default and records a warning.

## Files
- Create: `Fleet.Engine/Config/ConfigLoader.cs`
- Create: `Fleet.Tests/Engine/ConfigLoaderTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Config/Settings.cs`, `Fleet.Engine/Core/Enums.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. No `System.Globalization`. Allocation is allowed here (runs only when Custom Data changes).
- Use `MyIni` (`using VRage.Game.ModAPI.Ingame.Utilities;`) for all parsing and writing.

## MyIni API you need
```csharp
var ini = new MyIni();
MyIniParseResult r; bool ok = ini.TryParse(text, out r);      // false on malformed text
bool has = ini.ContainsKey("Miner", "Width");
MyIniValue v = ini.Get("Miner", "Width");
int i;    bool okI = v.TryGetInt32(out i);                       // false if present but not an integer
double d; bool okD = v.TryGetDouble(out d);
bool b;   bool okB = v.TryGetBoolean(out b);
string s = v.ToString("");                                       // raw text
ini.Set("Miner", "Width", 5);  ini.Set("Miner", "Loop", true);  ini.Set("Fleet", "Name", "x"); ini.Set("Miner","Depth", 30.0);
string text = ini.ToString();
```

## Interface (implement exactly)
```csharp
using System.Collections.Generic;

namespace IngameScript
{
    public partial class Program
    {
        public static class ConfigLoader
        {
            // Returns false if text is not valid INI (s untouched, one warning "Custom Data: not valid INI").
            // Empty or whitespace text is valid (all defaults, no warnings).
            public static bool Load(string text, Settings s, List<string> warnings);
            // Returns INI text containing every key, values taken from s; unknown sections/keys in text are kept.
            public static string Write(string text, Settings s);
        }
    }
}
```

## Keys, types and valid ranges
| Section | Key | Type | Range / values |
|---|---|---|---|
| Fleet | Name | string | non-empty |
| Fleet | Tag | string | non-empty |
| Miner | Width, Height | int | 1..50 |
| Miner | Depth | double | 1..500 |
| Miner | StartDepth | double | 0..500 |
| Miner | WorkSpeed | double | 0.1..10 |
| Miner | RetractSpeed | double | 0.1..20 |
| Miner | MaxLoad | double | 10..100 |
| Miner | MinLiftMargin | double | 1.05..5 |
| Miner | Eject | string list | comma-separated, entries trimmed, empty entries skipped; empty value = eject nothing |
| Miner | Loop | bool | |
| Miner | OnDamage | enum | Home / Job / Stop (case-insensitive) |
| Flight | MaxSpeed | double | 1..500 |
| Flight | ApproachSpeed | double | 0.5..50 |
| Flight | DockSpeed | double | 0.1..2 |
| Flight | SafeAltitude | double | 20..5000 |
| Flight | StuckSeconds | double | 1..60 |
| Energy | MinBattery | double | 0..90 |
| Energy | MinHydrogen | double | 0..90 |
| Energy | MinUranium | double | 0..1000 |
| Reload | OnReload | enum | Resume / ReturnHome / Hold (case-insensitive) |

## Behaviour
- Missing key → keep the current value in `s`, no warning.
- Present but unparseable, out of range, or empty string for Name/Tag → keep the current value and add a
  warning formatted exactly `"<Section>.<Key>: <reason>"`, where reason is `"not a number"`,
  `"not an integer"`, `"not true/false"`, `"out of range <range>"` where `<range>` is a **string literal
  copied exactly from the table's Range column** (e.g. `out of range 1..50`, `out of range 0.1..10` — never
  format the numbers at runtime, which would be culture-dependent), `"must not be empty"`, or
  `"unknown value '<raw>'"` for enums.
- Enum values are trimmed before matching (`" hold "` = `Hold`); `<raw>` in the warning is the untrimmed text.
- `Eject`: replace the contents of `s.Eject` (the list instance is readonly — use `Clear()` + `Add`).
- `Write`: parse `text` (if it fails to parse, start from an empty `MyIni`), then `Set` every key from the
  table using the value in `s` (Eject joined with `","`, enums by name), return `ini.ToString()`.

## Tests — create `Fleet.Tests/Engine/ConfigLoaderTests.cs` verbatim
```csharp
using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class ConfigLoaderTests
    {
        List<string> w;
        Settings s;
        [SetUp] public void Init() { w = new List<string>(); s = new Settings(); }

        [Test]
        public void EmptyText_AllDefaults_NoWarnings()
        {
            Assert.That(ConfigLoader.Load("", s, w), Is.True);
            Assert.That(w, Is.Empty);
            Assert.That(s.Width, Is.EqualTo(5));
            Assert.That(s.OnReload, Is.EqualTo(ReloadPolicy.Resume));
        }

        [Test]
        public void ReadsValues()
        {
            var text = "[Miner]\nWidth=7\nDepth=42.5\nLoop=false\nOnDamage=stop\nEject=Stone, Ice ,\n[Flight]\nSafeAltitude=300\n[Reload]\nOnReload=Hold\n[Fleet]\nName=Alpha\n";
            Assert.That(ConfigLoader.Load(text, s, w), Is.True);
            Assert.That(w, Is.Empty);
            Assert.That(s.Width, Is.EqualTo(7));
            Assert.That(s.Depth, Is.EqualTo(42.5));
            Assert.That(s.Loop, Is.False);
            Assert.That(s.OnDamage, Is.EqualTo(DamagePolicy.Stop));
            Assert.That(s.Eject, Is.EqualTo(new[] { "Stone", "Ice" }));
            Assert.That(s.SafeAltitude, Is.EqualTo(300));
            Assert.That(s.OnReload, Is.EqualTo(ReloadPolicy.Hold));
            Assert.That(s.Name, Is.EqualTo("Alpha"));
        }

        [Test]
        public void BadValues_KeepDefault_AndWarn()
        {
            var text = "[Miner]\nWidth=abc\nHeight=0\nWorkSpeed=fast\nLoop=maybe\nOnDamage=Explode\n[Fleet]\nName=\n";
            Assert.That(ConfigLoader.Load(text, s, w), Is.True);
            Assert.That(s.Width, Is.EqualTo(5));
            Assert.That(s.Height, Is.EqualTo(5));
            Assert.That(s.WorkSpeed, Is.EqualTo(1.5));
            Assert.That(s.Loop, Is.True);
            Assert.That(s.OnDamage, Is.EqualTo(DamagePolicy.Home));
            Assert.That(s.Name, Is.EqualTo("Miner-01"));
            Assert.That(w, Does.Contain("Miner.Width: not an integer"));
            Assert.That(w, Does.Contain("Miner.Height: out of range 1..50"));
            Assert.That(w, Does.Contain("Miner.WorkSpeed: not a number"));
            Assert.That(w, Does.Contain("Miner.Loop: not true/false"));
            Assert.That(w, Does.Contain("Miner.OnDamage: unknown value 'Explode'"));
            Assert.That(w, Does.Contain("Fleet.Name: must not be empty"));
        }

        [Test]
        public void EmptyEject_MeansEjectNothing()
        {
            ConfigLoader.Load("[Miner]\nEject=\n", s, w);
            Assert.That(s.Eject, Is.Empty);
        }

        [Test]
        public void MalformedIni_ReturnsFalse_AndKeepsSettings()
        {
            s.Width = 9;
            Assert.That(ConfigLoader.Load("this is not ini\n=== [", s, w), Is.False);
            Assert.That(s.Width, Is.EqualTo(9));
            Assert.That(w, Does.Contain("Custom Data: not valid INI"));
        }

        [Test]
        public void Write_FillsEveryKey_FromEmpty()
        {
            var text = ConfigLoader.Write("", s);
            foreach (var key in new[] { "Name=", "Tag=", "Width=5", "Height=5", "Depth=30", "StartDepth=0", "WorkSpeed=1.5",
                "RetractSpeed=4", "MaxLoad=90", "MinLiftMargin=1.3", "Eject=Stone", "Loop=true", "OnDamage=Home",
                "MaxSpeed=60", "ApproachSpeed=5", "DockSpeed=0.5", "SafeAltitude=150", "StuckSeconds=5",
                "MinBattery=20", "MinHydrogen=30", "MinUranium=5", "OnReload=Resume" })
                Assert.That(text, Does.Contain(key), key);
            foreach (var sec in new[] { "[Fleet]", "[Miner]", "[Flight]", "[Energy]", "[Reload]" })
                Assert.That(text, Does.Contain(sec), sec);
        }

        [Test]
        public void Write_PreservesUserValuesAndUnknownSections()
        {
            var original = "[Miner]\nWidth=7\n[Custom]\nMyKey=hello\n";
            ConfigLoader.Load(original, s, w);
            var text = ConfigLoader.Write(original, s);
            Assert.That(text, Does.Contain("Width=7"));
            Assert.That(text, Does.Contain("[Custom]"));
            Assert.That(text, Does.Contain("MyKey=hello"));
            var s2 = new Settings();
            Assert.That(ConfigLoader.Load(text, s2, new List<string>()), Is.True);
            Assert.That(s2.Width, Is.EqualTo(7));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~ConfigLoaderTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'ConfigLoader' does not exist".
- [ ] 3. Create `Fleet.Engine/Config/ConfigLoader.cs`. Suggested private helpers:
      `ReadInt(ini, sec, key, ref int field, int min, int max, warnings)`,
      `ReadDouble(...)`, `ReadBool(...)`, `ReadString(...)`, and enum readers comparing with
      `string.Equals(raw, "Home", StringComparison.OrdinalIgnoreCase)`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Config/ConfigLoader.cs Fleet.Tests/Engine/ConfigLoaderTests.cs; git commit -m "feat(engine): Custom Data config loader"`

## Done when
All ConfigLoaderTests pass.
