# C09 — CommandParser (run arguments → Cmd)

Milestone A · Difficulty: easy · Executor: local

## Goal
Turn the programmable block's run argument (typed by the player or bound to a toolbar button) into a
`Cmd` value plus any remaining text (for `GOTO <GPS>`).

## Files
- Create: `Fleet.Engine/Core/CommandParser.cs`
- Create: `Fleet.Tests/Engine/CommandParserTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/Enums.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. Allocation is allowed (commands are rare, user-driven).

## Interface (implement exactly)
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public static class CommandParser
        {
            public static Cmd Parse(string argument, out string rest);
        }
    }
}
```

## Behaviour
- `null`, empty or whitespace → `Cmd.None`, `rest = ""`.
- Trim the argument; split into the first word and the remainder (words separated by any whitespace).
  Matching is case-insensitive.
- Single-word commands: `START STOP HOME CONT NEXT PREV FULL STOPREC SETJOB GOTO REBOOT RESET GYROTEST
  UP DOWN APPLY BACK` → the `Cmd` of the same name (`STOPREC` → `StopRec`, `SETJOB` → `SetJob`,
  `GYROTEST` → `GyroTest`). `rest` = the remainder, trimmed (may be "").
- Two-word commands: first word `RECORD` with second word `DOCK` → `RecordDock`, `JOB` → `RecordJob`;
  `rest` = anything after the second word, trimmed. `RECORD` with no or another second word → `Unknown`.
- Anything else → `Cmd.Unknown`, `rest` = the whole trimmed argument.

## Tests — create `Fleet.Tests/Engine/CommandParserTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class CommandParserTests
    {
        [TestCase("START", Cmd.Start)]
        [TestCase("  start  ", Cmd.Start)]
        [TestCase("Stop", Cmd.Stop)]
        [TestCase("HOME", Cmd.Home)]
        [TestCase("cont", Cmd.Cont)]
        [TestCase("NEXT", Cmd.Next)]
        [TestCase("PREV", Cmd.Prev)]
        [TestCase("FULL", Cmd.Full)]
        [TestCase("STOPREC", Cmd.StopRec)]
        [TestCase("SETJOB", Cmd.SetJob)]
        [TestCase("REBOOT", Cmd.Reboot)]
        [TestCase("RESET", Cmd.Reset)]
        [TestCase("gyrotest", Cmd.GyroTest)]
        [TestCase("UP", Cmd.Up)]
        [TestCase("DOWN", Cmd.Down)]
        [TestCase("APPLY", Cmd.Apply)]
        [TestCase("BACK", Cmd.Back)]
        [TestCase("record dock", Cmd.RecordDock)]
        [TestCase("RECORD   JOB", Cmd.RecordJob)]
        public void Recognises(string arg, Cmd expected)
        {
            string rest;
            Assert.That(CommandParser.Parse(arg, out rest), Is.EqualTo(expected));
            Assert.That(rest, Is.EqualTo(""));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Empty_IsNone(string arg)
        {
            string rest;
            Assert.That(CommandParser.Parse(arg, out rest), Is.EqualTo(Cmd.None));
            Assert.That(rest, Is.EqualTo(""));
        }

        [Test]
        public void Goto_KeepsGpsText()
        {
            string rest;
            Assert.That(CommandParser.Parse("goto GPS:Ore:1:2:3:", out rest), Is.EqualTo(Cmd.Goto));
            Assert.That(rest, Is.EqualTo("GPS:Ore:1:2:3:"));
        }

        [Test]
        public void Goto_KeepsSpacesInsideRest()
        {
            string rest;
            CommandParser.Parse("GOTO  GPS:Big Rock:1:2:3: ", out rest);
            Assert.That(rest, Is.EqualTo("GPS:Big Rock:1:2:3:"));
        }

        [TestCase("banana")]
        [TestCase("RECORD")]
        [TestCase("RECORD banana")]
        public void Unknown_ReturnsWholeArgument(string arg)
        {
            string rest;
            Assert.That(CommandParser.Parse(arg, out rest), Is.EqualTo(Cmd.Unknown));
            Assert.That(rest, Is.EqualTo(arg.Trim()));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~CommandParserTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'CommandParser' does not exist".
- [ ] 3. Create `Fleet.Engine/Core/CommandParser.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Core/CommandParser.cs Fleet.Tests/Engine/CommandParserTests.cs; git commit -m "feat(engine): command parser"`

## Done when
All CommandParserTests pass.
