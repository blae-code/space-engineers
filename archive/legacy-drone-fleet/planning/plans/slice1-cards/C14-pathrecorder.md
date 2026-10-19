# C14 — PathRecorder and PathCodec

Milestone B · Difficulty: easy · Executor: local

## Goal
While the player flies a route, record points (in the home-connector frame) at a spacing that grows with
speed, up to a fixed maximum. Save and load those points to/from `Storage` through `MyIni`, which parses
numbers culture-independently.

## Files
- Create: `Fleet.Flight/Flight/PathRecorder.cs`
- Create: `Fleet.Flight/Flight/PathCodec.cs`
- Create: `Fleet.Tests/Flight/PathRecorderTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, classes nested and `public`.
- C# 6 only. No LINQ. `PathRecorder.Update` must not allocate (the list is created with its capacity in the
  constructor). `PathCodec` may allocate (runs on save/load only). No `System.Globalization`.

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class PathRecorder
        {
            public const double MinSpacing = 5;   // metres
            public const double SpacingPerSpeed = 1.0; // metres per (m/s): spacing = max(MinSpacing, speed * SpacingPerSpeed)
            public PathRecorder(int maxPoints);
            public readonly List<Vector3D> Points;    // local-frame points; created in the constructor with capacity maxPoints
            public bool IsFull { get; }
            public void Begin(Vector3D localPos);     // clears Points, adds localPos
            public bool Update(Vector3D localPos, double speed); // adds localPos if >= spacing from the last point; returns false if full (and adds nothing)
            public void End(Vector3D localPos);       // adds localPos if > 0.5 m from the last point and not full
        }

        public static class PathCodec
        {
            // Writes key "n" = count and keys "x<i>", "y<i>", "z<i>" = coordinates rounded to 2 decimals, into section.
            public static void Save(MyIni ini, string section, List<Vector3D> points);
            // Clears points. Returns false (points left empty) if the section or "n" is missing, or any coordinate
            // key is missing or not a number. n = 0 -> true with an empty list.
            public static bool Load(MyIni ini, string section, List<Vector3D> points);
        }
    }
}
```

## MyIni API you need
`ini.Set(section, key, double)`, `ini.Set(section, key, int)`, `ini.ContainsSection(section)`,
`ini.ContainsKey(section, key)`, `ini.Get(section, key).TryGetDouble(out d)`, `.TryGetInt32(out i)`.
Round with `Math.Round(v, 2)`.

## Tests — create `Fleet.Tests/Flight/PathRecorderTests.cs` verbatim
```csharp
using System.Collections.Generic;
using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class PathRecorderTests
    {
        [Test]
        public void AddsAtMinSpacing_WhenSlow()
        {
            var r = new PathRecorder(100);
            r.Begin(Vector3D.Zero);
            Assert.That(r.Update(new Vector3D(3, 0, 0), 1), Is.True);
            Assert.That(r.Points.Count, Is.EqualTo(1));
            r.Update(new Vector3D(5, 0, 0), 1);
            Assert.That(r.Points.Count, Is.EqualTo(2));
        }

        [Test]
        public void SpacingGrowsWithSpeed()
        {
            var r = new PathRecorder(100);
            r.Begin(Vector3D.Zero);
            r.Update(new Vector3D(7, 0, 0), 10);       // needs 10 m
            Assert.That(r.Points.Count, Is.EqualTo(1));
            r.Update(new Vector3D(10.5, 0, 0), 10);
            Assert.That(r.Points.Count, Is.EqualTo(2));
        }

        [Test]
        public void StopsWhenFull()
        {
            var r = new PathRecorder(3);
            r.Begin(Vector3D.Zero);
            r.Update(new Vector3D(5, 0, 0), 0);
            r.Update(new Vector3D(10, 0, 0), 0);
            Assert.That(r.IsFull, Is.True);
            Assert.That(r.Update(new Vector3D(15, 0, 0), 0), Is.False);
            Assert.That(r.Points.Count, Is.EqualTo(3));
        }

        [Test]
        public void End_AddsFinalPoint_UnlessTooClose()
        {
            var r = new PathRecorder(10);
            r.Begin(Vector3D.Zero);
            r.End(new Vector3D(0.3, 0, 0));
            Assert.That(r.Points.Count, Is.EqualTo(1));
            r.End(new Vector3D(2, 0, 0));
            Assert.That(r.Points.Count, Is.EqualTo(2));
        }

        [Test]
        public void Begin_Clears()
        {
            var r = new PathRecorder(10);
            r.Begin(Vector3D.Zero); r.Update(new Vector3D(6, 0, 0), 0);
            r.Begin(new Vector3D(1, 1, 1));
            Assert.That(r.Points.Count, Is.EqualTo(1));
            Assert.That(r.Points[0], Is.EqualTo(new Vector3D(1, 1, 1)));
        }

        [Test]
        public void Codec_RoundTrip()
        {
            var pts = new List<Vector3D> { new Vector3D(1.234, -5, 0), new Vector3D(100.5, 2.25, -3.333), new Vector3D(0, 0, 0) };
            var ini = new MyIni();
            PathCodec.Save(ini, "Path.Dock", pts);
            var ini2 = new MyIni();
            Assert.That(ini2.TryParse(ini.ToString()), Is.True);
            var back = new List<Vector3D>();
            Assert.That(PathCodec.Load(ini2, "Path.Dock", back), Is.True);
            Assert.That(back.Count, Is.EqualTo(3));
            for (int i = 0; i < 3; i++)
                Assert.That(Vector3D.Distance(back[i], pts[i]), Is.LessThan(0.01));
        }

        [Test]
        public void Codec_MissingSection_IsFalseAndEmpty()
        {
            var back = new List<Vector3D> { Vector3D.One };
            Assert.That(PathCodec.Load(new MyIni(), "Path.Job", back), Is.False);
            Assert.That(back, Is.Empty);
        }

        [Test]
        public void Codec_MissingCoordinate_IsFalseAndEmpty()
        {
            var ini = new MyIni();
            ini.TryParse("[P]\nn=2\nx0=1\ny0=2\nz0=3\nx1=4\ny1=5\n");
            var back = new List<Vector3D>();
            Assert.That(PathCodec.Load(ini, "P", back), Is.False);
            Assert.That(back, Is.Empty);
        }

        [Test]
        public void Codec_ZeroPoints_IsTrue()
        {
            var ini = new MyIni();
            PathCodec.Save(ini, "P", new List<Vector3D>());
            var back = new List<Vector3D> { Vector3D.One };
            Assert.That(PathCodec.Load(ini, "P", back), Is.True);
            Assert.That(back, Is.Empty);
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~PathRecorderTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'PathRecorder' could not be found".
- [ ] 3. Create `Fleet.Flight/Flight/PathRecorder.cs` and `Fleet.Flight/Flight/PathCodec.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/PathRecorder.cs Fleet.Flight/Flight/PathCodec.cs Fleet.Tests/Flight/PathRecorderTests.cs; git commit -m "feat(flight): path recorder and codec"`

## Done when
All PathRecorderTests pass.
