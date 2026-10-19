# C02 — Profiler (instruction and run-time statistics)

Milestone A · Difficulty: easy · Executor: local

## Goal
Track how many instructions and milliseconds each script run costs: the latest value, a rolling average
over a fixed window, and the peak. The status screen shows these values so efficiency stays visible.

## Files
- Create: `Fleet.Engine/Core/Profiler.cs`
- Create: `Fleet.Tests/Engine/ProfilerTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. **`Record` must not allocate** (arrays created in the constructor only).

## Interface (implement exactly)
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public class Profiler
        {
            public Profiler(int window);                // window >= 1: number of samples averaged
            public void Record(int instructions, double ms);
            public int Last { get; }                    // last recorded instruction count (0 before any)
            public double Average { get; }              // mean instructions over the samples held (0 before any)
            public int Peak { get; }                    // max instructions since construction or ResetPeak
            public double AverageMs { get; }            // mean ms over the samples held (0 before any)
            public double PeakMs { get; }               // max ms since construction or ResetPeak
            public int Count { get; }                   // samples held, <= window
            public void ResetPeak();
        }
    }
}
```

## Behaviour
- Two ring buffers (`int[]` and `double[]`) of length `window`, a write index, and a count.
- `Record` overwrites the oldest sample once `window` samples are held; keep running sums so `Average`
  and `AverageMs` are O(1): subtract the overwritten value, add the new one.
- `ResetPeak` sets `Peak` and `PeakMs` to 0; the averages are unchanged.

## Tests — create `Fleet.Tests/Engine/ProfilerTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class ProfilerTests
    {
        [Test]
        public void Empty_IsZero()
        {
            var p = new Profiler(3);
            Assert.That(p.Last, Is.EqualTo(0));
            Assert.That(p.Average, Is.EqualTo(0));
            Assert.That(p.AverageMs, Is.EqualTo(0));
            Assert.That(p.Peak, Is.EqualTo(0));
            Assert.That(p.Count, Is.EqualTo(0));
        }

        [Test]
        public void AveragesOverWindow_AndDropsOldest()
        {
            var p = new Profiler(3);
            p.Record(10, 1.0); p.Record(20, 2.0); p.Record(30, 3.0);
            Assert.That(p.Average, Is.EqualTo(20).Within(1e-9));
            Assert.That(p.AverageMs, Is.EqualTo(2.0).Within(1e-9));
            p.Record(40, 4.0);
            Assert.That(p.Count, Is.EqualTo(3));
            Assert.That(p.Average, Is.EqualTo(30).Within(1e-9));
            Assert.That(p.AverageMs, Is.EqualTo(3.0).Within(1e-9));
            Assert.That(p.Last, Is.EqualTo(40));
        }

        [Test]
        public void Peak_TracksMax_AndResets()
        {
            var p = new Profiler(2);
            p.Record(50, 0.5); p.Record(10, 0.1); p.Record(20, 0.2);
            Assert.That(p.Peak, Is.EqualTo(50));
            Assert.That(p.PeakMs, Is.EqualTo(0.5).Within(1e-9));
            p.ResetPeak();
            Assert.That(p.Peak, Is.EqualTo(0));
            Assert.That(p.PeakMs, Is.EqualTo(0));
            Assert.That(p.Average, Is.EqualTo(15).Within(1e-9));
            p.Record(5, 0.05);
            Assert.That(p.Peak, Is.EqualTo(5));
        }

        [Test]
        public void PartialWindow_AveragesOnlyHeldSamples()
        {
            var p = new Profiler(10);
            p.Record(4, 1); p.Record(6, 3);
            Assert.That(p.Count, Is.EqualTo(2));
            Assert.That(p.Average, Is.EqualTo(5).Within(1e-9));
            Assert.That(p.AverageMs, Is.EqualTo(2).Within(1e-9));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~ProfilerTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'Profiler' could not be found".
- [ ] 3. Create `Fleet.Engine/Core/Profiler.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Core/Profiler.cs Fleet.Tests/Engine/ProfilerTests.cs; git commit -m "feat(engine): instruction profiler"`

## Done when
All ProfilerTests pass; `Record` contains no `new`.
