# C03 — EventLog (state-transition history for the LCD)

Milestone A · Difficulty: easy · Executor: local

## Goal
Keep the last N state transitions ("why did it come home?") and render them, newest first, into a
`StringBuilder` without allocating.

## Files
- Create: `Fleet.Engine/Core/EventLog.cs`
- Create: `Fleet.Tests/Engine/EventLogTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/Enums.cs`, `Fleet.Engine/Util/SbFormat.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. **`Add` and `Render` must not allocate**: use a preallocated struct array; display
  names come from `Names.State[(int)state]` and `Names.Reason[(int)reason]` (never `enum.ToString()`);
  numbers via `SbFormat`.

## Interface (implement exactly)
```csharp
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public class EventLog
        {
            public EventLog(int capacity);
            public int Count { get; }
            // note: an optional constant string (e.g. a Hold reason); may be null
            public void Add(double time, MinerState from, MinerState to, ReturnReason reason, string note);
            // newest first, at most maxLines lines, each ending in '\n'
            public void Render(StringBuilder sb, int maxLines);
            public void Clear();
        }
    }
}
```

## Behaviour
- Store entries in a private `struct Entry { public double Time; public MinerState From, To; public ReturnReason Reason; public string Note; }`
  array of length `capacity` used as a ring buffer; when full, the oldest entry is overwritten.
- Line format: `mm:ss From>To` then, if `reason != None`, a space and the reason name, then, if `note` is
  non-null and non-empty, a space and the note, then `\n`. Time via `SbFormat.AppendTime`.
  Example: `00:05 Drill>Retract CargoFull\n`, `01:10 Dock>Hold docking failed\n`.

## Tests — create `Fleet.Tests/Engine/EventLogTests.cs` verbatim
```csharp
using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class EventLogTests
    {
        [Test]
        public void RendersNewestFirst_WithReasonAndNote()
        {
            var log = new EventLog(5);
            log.Add(5, MinerState.Drill, MinerState.Retract, ReturnReason.CargoFull, null);
            log.Add(70, MinerState.Dock, MinerState.Hold, ReturnReason.None, "docking failed");
            var sb = new StringBuilder();
            log.Render(sb, 10);
            Assert.That(sb.ToString(), Is.EqualTo("01:10 Dock>Hold docking failed\n00:05 Drill>Retract CargoFull\n"));
        }

        [Test]
        public void DropsOldest_WhenFull()
        {
            var log = new EventLog(3);
            log.Add(1, MinerState.Idle, MinerState.Undock, ReturnReason.None, null);
            log.Add(2, MinerState.Undock, MinerState.DockPathOut, ReturnReason.None, null);
            log.Add(3, MinerState.DockPathOut, MinerState.RouteOut, ReturnReason.None, null);
            log.Add(4, MinerState.RouteOut, MinerState.Position, ReturnReason.None, null);
            Assert.That(log.Count, Is.EqualTo(3));
            var sb = new StringBuilder();
            log.Render(sb, 10);
            Assert.That(sb.ToString(), Does.Not.Contain("Idle>Undock"));
            Assert.That(sb.ToString(), Does.StartWith("00:04 RouteOut>Position\n"));
        }

        [Test]
        public void Render_RespectsMaxLines()
        {
            var log = new EventLog(5);
            log.Add(1, MinerState.Idle, MinerState.Undock, ReturnReason.None, null);
            log.Add(2, MinerState.Undock, MinerState.DockPathOut, ReturnReason.None, null);
            var sb = new StringBuilder();
            log.Render(sb, 1);
            Assert.That(sb.ToString(), Is.EqualTo("00:02 Undock>DockPathOut\n"));
        }

        [Test]
        public void EmptyNote_IsOmitted_AndClearEmpties()
        {
            var log = new EventLog(2);
            log.Add(0, MinerState.Idle, MinerState.Hold, ReturnReason.None, "");
            var sb = new StringBuilder();
            log.Render(sb, 5);
            Assert.That(sb.ToString(), Is.EqualTo("00:00 Idle>Hold\n"));
            log.Clear();
            Assert.That(log.Count, Is.EqualTo(0));
            sb.Clear(); log.Render(sb, 5);
            Assert.That(sb.ToString(), Is.EqualTo(""));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~EventLogTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'EventLog' could not be found".
- [ ] 3. Create `Fleet.Engine/Core/EventLog.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Core/EventLog.cs Fleet.Tests/Engine/EventLogTests.cs; git commit -m "feat(engine): transition event log"`

## Done when
All EventLogTests pass; `EventLog.cs` contains no `ToString(`, `+ "` or `$"`.
