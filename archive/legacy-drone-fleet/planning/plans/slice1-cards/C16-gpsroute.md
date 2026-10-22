# C16 — Gps parsing and GpsRoute

Milestone B · Difficulty: medium · Executor: local

## Goal
Accept a Space Engineers GPS string (`GPS:Name:X:Y:Z:` with an optional colour field) and fly to it:
straight in space; on a planet climb to a safe altitude, cruise holding that altitude, then descend.
Number parsing is hand-written because culture-aware parsing is not allowed in scripts.

## Files
- Create: `Fleet.Flight/Flight/GpsRoute.cs` (contains both `Gps` and `GpsRoute`)
- Create: `Fleet.Tests/Flight/GpsRouteTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, classes nested and `public`.
- C# 6 only. No LINQ. **No `System.Globalization`, no `double.Parse`/`TryParse`** — use `TryParseNumber`.
- `Gps` may allocate (commands are rare). `GpsRoute.Update` must not allocate.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class Gps
        {
            // [+-]digits[.digits][(e|E)[+-]digits]; ".5" and "5." are valid; must contain at least one digit
            // before the exponent; nothing else allowed (no spaces).
            public static bool TryParseNumber(string s, out double value);
            // "GPS:<name>:<x>:<y>:<z>:" optionally followed by "<colour>:". Name may contain spaces.
            public static bool TryParse(string s, out Vector3D pos);
        }

        public class GpsRoute
        {
            public enum Phase { Space, Climb, Cruise, Descend, Done }
            public const double CruiseStep = 100;     // carrot distance ahead while cruising (m)
            public const double DescendRadius = 20;   // start descending when horizontally this close (m)
            public Phase Current { get; }
            public double Remaining { get; }          // metres left, for braking
            public Vector3D Target { get; }
            public void Start(Vector3D targetWorld, bool inGravity);   // Current = inGravity ? Climb : Space
            // Returns the world point the helm should fly to this tick.
            public Vector3D Update(Vector3D pos, Vector3D gravity, double elevation, double safeAltitude, double reachDist);
        }
    }
}
```

## Behaviour — Gps.TryParse
Trim the input first. The prefix `GPS:` is case-sensitive (the game always writes it upper-case).
Fields after z (the colour, or anything else) are ignored. Name may be empty.

## Behaviour — GpsRoute.Update
A route started in Space stays a straight-line route even if gravity appears later (slice 1 decision:
`Start` decides the mode once).
First apply **at most one** phase transition for the current phase, then compute the output for the
(possibly new) phase. `up = -Normalize(gravity)` when gravity is non-zero.
- Transitions:
  - Climb/Cruise with `gravity.LengthSquared() < 1e-6` → Space.
  - Space: `Distance(pos, Target) <= reachDist` → Done.
  - Climb: `elevation >= 0.95 * safeAltitude` → Cruise.
  - Cruise: horizontal distance `h <= DescendRadius` → Descend, where `horiz = (Target - pos) - up * Dot(Target - pos, up)`, `h = horiz.Length()`.
  - Descend: `Distance(pos, Target) <= reachDist` → Done.
- Outputs:
  - Space, Descend, Done: return `Target`; `Remaining = Distance(pos, Target)`.
  - Climb: return `pos + up * (safeAltitude - elevation + 5)`; `Remaining = h + |Dot(Target - pos, up)|`.
  - Cruise: return `pos + Normalize(horiz) * Math.Min(CruiseStep, h) + up * (safeAltitude - elevation)`;
    `Remaining = h + |Dot(Target - pos, up)|`.

## Tests — create `Fleet.Tests/Flight/GpsRouteTests.cs` verbatim
```csharp
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class GpsRouteTests
    {
        static void Near(Vector3D a, Vector3D e) { Assert.That(Vector3D.Distance(a, e), Is.LessThan(1e-6), "actual " + a + " expected " + e); }

        [TestCase("100.5", 100.5)]
        [TestCase("-20", -20)]
        [TestCase("3e2", 300)]
        [TestCase("+1.5E-1", 0.15)]
        [TestCase(".5", 0.5)]
        [TestCase("5.", 5)]
        [TestCase("0", 0)]
        public void ParseNumber_Valid(string s, double expected)
        {
            double v;
            Assert.That(Gps.TryParseNumber(s, out v), Is.True);
            Assert.That(v, Is.EqualTo(expected).Within(1e-9));
        }

        [TestCase("")]
        [TestCase("-")]
        [TestCase("1.2.3")]
        [TestCase("abc")]
        [TestCase("1e")]
        [TestCase(" 1")]
        [TestCase("e5")]
        public void ParseNumber_Invalid(string s)
        {
            double v;
            Assert.That(Gps.TryParseNumber(s, out v), Is.False);
        }

        [Test]
        public void ParseGps()
        {
            Vector3D p;
            Assert.That(Gps.TryParse("GPS:Ore:100.5:-20:3e2:", out p), Is.True);
            Near(p, new Vector3D(100.5, -20, 300));
            Assert.That(Gps.TryParse("GPS:Big Rock:1:2:3:#FF75C9F1:", out p), Is.True);
            Near(p, new Vector3D(1, 2, 3));
            Assert.That(Gps.TryParse("GPS:bad:x:2:3:", out p), Is.False);
            Assert.That(Gps.TryParse("hello", out p), Is.False);
            Assert.That(Gps.TryParse("GPS:Ore:1:2:", out p), Is.False);
        }

        [Test]
        public void Space_FliesStraight_ThenDone()
        {
            var r = new GpsRoute();
            r.Start(new Vector3D(100, 0, 0), false);
            var t = r.Update(Vector3D.Zero, Vector3D.Zero, 0, 150, 2);
            Near(t, new Vector3D(100, 0, 0));
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Space));
            Assert.That(r.Remaining, Is.EqualTo(100).Within(1e-9));
            r.Update(new Vector3D(99, 0, 0), Vector3D.Zero, 0, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Done));
        }

        [Test]
        public void Planet_Climb_Cruise_Descend_Done()
        {
            var g = new Vector3D(0, -9.81, 0);
            var r = new GpsRoute();
            r.Start(new Vector3D(1000, 0, 0), true);

            var t = r.Update(Vector3D.Zero, g, 50, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Climb));
            Near(t, new Vector3D(0, 105, 0));

            t = r.Update(new Vector3D(0, 100, 0), g, 145, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Cruise));
            Near(t, new Vector3D(100, 105, 0));
            Assert.That(r.Remaining, Is.EqualTo(1000 + 100).Within(1e-6));

            t = r.Update(new Vector3D(990, 150, 0), g, 150, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Descend));
            Near(t, new Vector3D(1000, 0, 0));

            r.Update(new Vector3D(1000, 1, 0), g, 1, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Done));
        }

        [Test]
        public void LeavingGravity_SwitchesToSpace()
        {
            var r = new GpsRoute();
            r.Start(new Vector3D(0, 5000, 0), true);
            r.Update(Vector3D.Zero, Vector3D.Zero, 0, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Space));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~GpsRouteTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'Gps' does not exist".
- [ ] 3. Create `Fleet.Flight/Flight/GpsRoute.cs`. `TryParse`: must start with `"GPS:"`, split on `':'`
      (`s.Split(':')`), need at least 5 parts: `[0]="GPS"`, `[1]=name`, `[2..4]=x,y,z`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/GpsRoute.cs Fleet.Tests/Flight/GpsRouteTests.cs; git commit -m "feat(flight): GPS parsing and route"`

## Done when
All GpsRouteTests pass; `GpsRoute.cs` contains no `double.Parse`, `double.TryParse` or `CultureInfo`.
