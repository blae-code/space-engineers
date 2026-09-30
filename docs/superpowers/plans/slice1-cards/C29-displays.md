# C29 — Displays (find tagged screens, write text)

Milestone D · Difficulty: medium · Executor: local+review

## Goal
Find the screens the operator tagged — LCD panels with the fleet tag (e.g. `[FM]`) in their name, and
cockpit screens tagged `[FM:n]` meaning screen *n* of that block — and write status/menu text to them.

## Files
- Create: `Fleet.Engine/Ui/Displays.cs`
- Create: `Fleet.Tests/Engine/DisplaysTests.cs` (pure helper only)

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. `Refresh` may allocate (name parsing); `Write` must not.

## Space Engineers API you need
```csharp
using Sandbox.ModAPI.Ingame; using VRage.Game.GUI.TextPanel; using System.Text;
IMyTextPanel p;                     // is itself an IMyTextSurface
IMyTextSurfaceProvider prov;        // cockpits, PBs: prov.SurfaceCount, prov.GetSurface(i)
IMyTextSurface s; s.ContentType = ContentType.TEXT_AND_IMAGE; s.Font = "Monospace"; s.FontSize = 0.8f;
s.WriteText(stringBuilder);         // replaces the text (overload taking StringBuilder)
```

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class Displays
        {
            public Displays();
            // blocks: terminal blocks on the drone's grid. Collects panels whose name contains tag and provider
            // surfaces named with "[<tagCore>:n]" (tag "[FM]" -> tagCore "FM"). Sets text mode, Monospace, 0.8.
            public void Refresh(List<IMyTerminalBlock> blocks, string tag);
            public int Count { get; }
            public void Write(StringBuilder text);       // same text to every surface
            // pure helper (unit-tested): index n from a name containing "[<tagCore>:n]"; -1 if absent/invalid.
            public static int SurfaceIndex(string name, string tag);
        }
    }
}
```

## Behaviour — Refresh
- Check `IMyTextPanel` **first**: a text panel is also an `IMyTextSurfaceProvider`, so testing the provider
  first would treat panels wrongly. A panel qualifies if its name contains the full tag.
- Otherwise, for an `IMyTextSurfaceProvider` with a valid `SurfaceIndex(name, tag)`: skip it if the index is
  `>= SurfaceCount`; else add `GetSurface(index)`.

## Tests — create `Fleet.Tests/Engine/DisplaysTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class DisplaysTests
    {
        [TestCase("Cockpit [FM:1]", "[FM]", 1)]
        [TestCase("Cockpit [FM:0] left", "[FM]", 0)]
        [TestCase("Cockpit [FM:12]", "[FM]", 12)]
        [TestCase("Cockpit [FM]", "[FM]", -1)]
        [TestCase("Cockpit [FM:x]", "[FM]", -1)]
        [TestCase("Cockpit [FM:]", "[FM]", -1)]
        [TestCase("Cockpit", "[FM]", -1)]
        [TestCase("Seat [MINER:2]", "[MINER]", 2)]
        public void SurfaceIndex(string name, string tag, int expected)
        {
            Assert.That(Displays.SurfaceIndex(name, tag), Is.EqualTo(expected));
        }

        [Test]
        public void EmptyDisplays_WriteDoesNothing()
        {
            var d = new Displays();
            Assert.That(d.Count, Is.EqualTo(0));
            Assert.DoesNotThrow(() => d.Write(new System.Text.StringBuilder("x")));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file; run `dotnet test Fleet.Tests --filter "FullyQualifiedName~DisplaysTests" 2>&1 | tail -n 25` → build error.
- [ ] 2. Create `Fleet.Engine/Ui/Displays.cs`. `SurfaceIndex`: tagCore = tag without its first and last
      character; find `"[" + tagCore + ":"`; read digits up to `]`; at least one digit, else -1.
- [ ] 3. Run the same command → `Passed!`.
- [ ] 4. `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15` → "successfully deployed".
- [ ] 5. Commit: `git add Fleet.Engine/Ui/Displays.cs Fleet.Tests/Engine/DisplaysTests.cs; git commit -m "feat(engine): tagged displays"`

## Done when
Tests pass and the Release build is clean. In-game: checklist G2.
