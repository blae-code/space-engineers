# C06 — Kernel (tick routing, fail-safe, profiling)

Milestone A · Difficulty: medium · Executor: local

## Goal
Route each script run to the subsystems by update type, measure its cost, and — if any subsystem throws —
enter **SAFE** exactly once: stop ticking, remember which subsystem failed and why, and call a hook that
releases the ship's controls. `ResetSafe` resumes.

## Files
- Create: `Fleet.Engine/Core/Kernel.cs`
- Create: `Fleet.Tests/Engine/KernelTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/ISubsystem.cs`, `Fleet.Engine/Core/Profiler.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. **`Tick` must not allocate on the normal path** (use `for` loops over the list, not
  `foreach`, which is fine on `List<T>` but keep it simple and explicit). The exception path may allocate.
- `UpdateType` is in `Sandbox.ModAPI.Ingame` and is a `[Flags]` enum (`Update1`, `Update10`, `Update100`,
  `Terminal`, `Trigger`, ...).

## Interface (implement exactly)
```csharp
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class Kernel
        {
            public Kernel(List<ISubsystem> subs, Profiler profiler, Func<int> instructionCount);
            public bool IsSafe { get; }
            public string SafeReason { get; }   // "" when not safe
            public Action OnSafe;               // invoked once each time SAFE is entered; may be null
            public void Tick(UpdateType src, double lastRunMs);
            public void EnterSafe(string reason);
            public void ResetSafe();
        }
    }
}
```

## Behaviour
- `Tick`: if `IsSafe`, return immediately (no calls, no profiling). Otherwise, inside one `try`:
  1. if `(src & UpdateType.Update1) != 0` call `Update1()` on every subsystem in list order;
  2. then likewise `Update10()` for `Update10`;
  3. then `Update100()` for `Update100`.
  Keep a field with the subsystem currently being called. After the updates (also when no update flag
  was set, e.g. `Terminal`), call `profiler.Record(instructionCount(), lastRunMs)`.
- `catch (Exception e)`: `EnterSafe(<current subsystem Name> + ": " + e.Message)`. A tick that threw records
  **no** profiler sample (the `Record` call is the last statement inside the `try`).
- `EnterSafe(reason)`: if already safe, do nothing. Else set `IsSafe = true`, `SafeReason = reason`, then
  invoke `OnSafe` if not null.
- `ResetSafe()`: `IsSafe = false`, `SafeReason = ""`.

## Tests — create `Fleet.Tests/Engine/KernelTests.cs` verbatim
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    class CountingSub : ISubsystem
    {
        public CountingSub(string name, List<string> calls) { Name = name; Calls = calls; }
        public string Name { get; private set; }
        public int StorageVersion { get { return 1; } }
        public List<string> Calls;
        public bool ThrowOn10;
        public void Update1() { Calls.Add(Name + ".1"); }
        public void Update10() { if (ThrowOn10) throw new InvalidOperationException("boom"); Calls.Add(Name + ".10"); }
        public void Update100() { Calls.Add(Name + ".100"); }
        public void HandleMessage(MyIGCMessage msg) { }
        public void Save(MyIni ini) { }
        public bool Load(MyIni ini, int savedVersion) { return true; }
        public void Status(StringBuilder sb) { }
    }

    [TestFixture]
    public class KernelTests
    {
        List<string> calls;
        CountingSub a, b;
        Profiler prof;
        Kernel k;

        [SetUp]
        public void Init()
        {
            calls = new List<string>();
            a = new CountingSub("A", calls);
            b = new CountingSub("B", calls);
            prof = new Profiler(4);
            k = new Kernel(new List<ISubsystem> { a, b }, prof, () => 123);
        }

        [Test]
        public void RoutesByFlag_InOrder()
        {
            k.Tick(UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100, 0.5);
            Assert.That(calls, Is.EqualTo(new[] { "A.1", "B.1", "A.10", "B.10", "A.100", "B.100" }));
        }

        [Test]
        public void OnlyRequestedFlags()
        {
            k.Tick(UpdateType.Update10, 0);
            Assert.That(calls, Is.EqualTo(new[] { "A.10", "B.10" }));
        }

        [Test]
        public void Records_Profile_EvenForTerminalRuns()
        {
            k.Tick(UpdateType.Terminal, 0.25);
            Assert.That(calls, Is.Empty);
            Assert.That(prof.Last, Is.EqualTo(123));
            Assert.That(prof.AverageMs, Is.EqualTo(0.25).Within(1e-9));
        }

        [Test]
        public void Exception_EntersSafeOnce_AndStopsTicking()
        {
            int safeCalls = 0;
            k.OnSafe = () => safeCalls++;
            b.ThrowOn10 = true;
            k.Tick(UpdateType.Update10, 0);
            Assert.That(k.IsSafe, Is.True);
            Assert.That(k.SafeReason, Is.EqualTo("B: boom"));
            Assert.That(safeCalls, Is.EqualTo(1));

            calls.Clear();
            k.Tick(UpdateType.Update10 | UpdateType.Update100, 0);
            Assert.That(calls, Is.Empty);
            k.EnterSafe("again");
            Assert.That(safeCalls, Is.EqualTo(1));
            Assert.That(k.SafeReason, Is.EqualTo("B: boom"));
        }

        [Test]
        public void ResetSafe_Resumes()
        {
            b.ThrowOn10 = true;
            k.Tick(UpdateType.Update10, 0);
            b.ThrowOn10 = false;
            k.ResetSafe();
            Assert.That(k.IsSafe, Is.False);
            Assert.That(k.SafeReason, Is.EqualTo(""));
            calls.Clear();
            k.Tick(UpdateType.Update10, 0);
            Assert.That(calls, Is.EqualTo(new[] { "A.10", "B.10" }));
        }

        [Test]
        public void OnSafe_MayBeNull()
        {
            b.ThrowOn10 = true;
            Assert.DoesNotThrow(() => k.Tick(UpdateType.Update10, 0));
            Assert.That(k.IsSafe, Is.True);
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~KernelTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'Kernel' could not be found".
- [ ] 3. Create `Fleet.Engine/Core/Kernel.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Core/Kernel.cs Fleet.Tests/Engine/KernelTests.cs; git commit -m "feat(engine): kernel with fail-safe and profiling"`

## Done when
All KernelTests pass.
