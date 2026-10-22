# C24 — MenuModel (LCD menu driven by UP / DOWN / APPLY / BACK)

Milestone C · Difficulty: medium · Executor: local

## Goal
A three-page menu for the cockpit LCD: a main page, a **Job** page that edits numeric settings, and a
**Commands** page that fires commands. It never edits settings itself — it returns an action that the
integration writes back into Custom Data (the source of truth).

## Files
- Create: `Fleet.Drone.Miner/Ui/MenuModel.cs`
- Create: `Fleet.Tests/Mining/MenuModelTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/Enums.cs`, `Fleet.Engine/Config/Settings.cs`, `Fleet.Engine/Util/SbFormat.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, types nested and `public`.
- C# 6 only. No LINQ. **`Render` must not allocate**: constant label strings, numbers via `SbFormat.AppendFixed`.

## Interface (implement exactly)
```csharp
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public enum MenuActionKind : byte { None, Command, SetValue }

        public struct MenuAction
        {
            public MenuActionKind Kind;
            public Cmd Command;       // when Kind == Command
            public string Section;    // when Kind == SetValue
            public string Key;
            public double Value;
        }

        public class MenuModel
        {
            public const int PageMain = 0, PageJob = 1, PageCommands = 2;
            public int Page { get; }
            public int Cursor { get; }
            public bool Editing { get; }
            public double EditValue { get; }
            public MenuAction Handle(Cmd c, Settings s);   // only Up, Down, Apply, Back do anything
            public void Render(StringBuilder sb, Settings s);
        }
    }
}
```

## Pages and items
- **Main** (title `== Menu ==`): `Job settings`, `Commands`.
- **Job** (title `== Job ==`), all in section `Miner`:

  | Label / Key | Settings field | Min | Max | Step | Decimals |
  |---|---|---|---|---|---|
  | Width | Width | 1 | 50 | 1 | 0 |
  | Height | Height | 1 | 50 | 1 | 0 |
  | Depth | Depth | 1 | 500 | 5 | 0 |
  | StartDepth | StartDepth | 0 | 500 | 1 | 0 |
  | WorkSpeed | WorkSpeed | 0.1 | 10 | 0.1 | 1 |
  | RetractSpeed | RetractSpeed | 0.1 | 20 | 0.5 | 1 |
  | MaxLoad | MaxLoad | 10 | 100 | 5 | 0 |
  | MinLiftMargin | MinLiftMargin | 1.05 | 5 | 0.05 | 2 |

- **Commands** (title `== Commands ==`): `Start`→Start, `Home`→Home, `Stop`→Stop, `Continue`→Cont,
  `Next hole`→Next, `Previous hole`→Prev, `Simulate full`→Full, `Record dock path`→RecordDock,
  `Record job route`→RecordJob, `Stop recording`→StopRec, `Set job here`→SetJob, `Gyro test`→GyroTest.

## Behaviour — `Handle`
- Not editing:
  - `Up` / `Down`: move the cursor by −1 / +1 with **wrap-around** within the page's items.
  - `Apply`: Main → open Job (cursor 0) or Commands (cursor 0); Job → start editing (`EditValue` = the
    field's current value in `s`); Commands → return `{ Kind = Command, Command = <item's Cmd> }`.
  - `Back`: on Job/Commands → Main with the cursor on that page's main item (Job → 0, Commands → 1).
- Editing:
  - `Up` / `Down`: `EditValue ± step`, clamped to [min, max], then `Math.Round(EditValue, 2)`.
  - `Apply`: stop editing, return `{ Kind = SetValue, Section = "Miner", Key = <key>, Value = EditValue }`.
  - `Back`: stop editing, return None (cancel).
- Anything else returns `{ Kind = None }`.

## Behaviour — `Render`
Title line, then one line per item: `"> "` before the cursor item, `"  "` before the others. Job items
render as `Label: value` using the item's decimals; the item being edited renders its value in brackets:
`> Width: [6]`. Every line ends with `\n`.

## Tests — create `Fleet.Tests/Mining/MenuModelTests.cs` verbatim
```csharp
using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class MenuModelTests
    {
        Settings s; MenuModel m;
        [SetUp] public void Init() { s = new Settings(); m = new MenuModel(); }
        string Render() { var sb = new StringBuilder(); m.Render(sb, s); return sb.ToString(); }

        [Test]
        public void MainPage_Renders()
        {
            Assert.That(Render(), Is.EqualTo("== Menu ==\n> Job settings\n  Commands\n"));
        }

        [Test]
        public void CursorWraps()
        {
            m.Handle(Cmd.Up, s);
            Assert.That(m.Cursor, Is.EqualTo(1));
            m.Handle(Cmd.Down, s);
            Assert.That(m.Cursor, Is.EqualTo(0));
        }

        [Test]
        public void JobPage_RendersValuesWithDecimals()
        {
            m.Handle(Cmd.Apply, s);
            Assert.That(m.Page, Is.EqualTo(MenuModel.PageJob));
            Assert.That(Render(), Is.EqualTo(
                "== Job ==\n> Width: 5\n  Height: 5\n  Depth: 30\n  StartDepth: 0\n  WorkSpeed: 1.5\n" +
                "  RetractSpeed: 4.0\n  MaxLoad: 90\n  MinLiftMargin: 1.30\n"));
        }

        [Test]
        public void Edit_Increment_Apply_ReturnsSetValue()
        {
            m.Handle(Cmd.Apply, s);          // open Job
            m.Handle(Cmd.Apply, s);          // edit Width
            Assert.That(m.Editing, Is.True);
            m.Handle(Cmd.Up, s);
            Assert.That(Render(), Does.Contain("> Width: [6]\n"));
            var a = m.Handle(Cmd.Apply, s);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.SetValue));
            Assert.That(a.Section, Is.EqualTo("Miner"));
            Assert.That(a.Key, Is.EqualTo("Width"));
            Assert.That(a.Value, Is.EqualTo(6));
            Assert.That(m.Editing, Is.False);
        }

        [Test]
        public void Edit_ClampsAndRounds()
        {
            s.MinLiftMargin = 1.05;
            m.Handle(Cmd.Apply, s);
            for (int k = 0; k < 7; k++) m.Handle(Cmd.Down, s);   // cursor to MinLiftMargin (index 7)
            m.Handle(Cmd.Apply, s);
            m.Handle(Cmd.Down, s);
            Assert.That(m.EditValue, Is.EqualTo(1.05));
            m.Handle(Cmd.Up, s); m.Handle(Cmd.Up, s); m.Handle(Cmd.Up, s);
            Assert.That(m.EditValue, Is.EqualTo(1.2));
        }

        [Test]
        public void Edit_BackCancels()
        {
            m.Handle(Cmd.Apply, s);
            m.Handle(Cmd.Apply, s);
            m.Handle(Cmd.Up, s);
            var a = m.Handle(Cmd.Back, s);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.None));
            Assert.That(m.Editing, Is.False);
            Assert.That(Render(), Does.Contain("> Width: 5\n"));
        }

        [Test]
        public void Commands_ReturnCommand_AndBackReturnsToMain()
        {
            m.Handle(Cmd.Down, s);           // Commands
            m.Handle(Cmd.Apply, s);
            Assert.That(m.Page, Is.EqualTo(MenuModel.PageCommands));
            m.Handle(Cmd.Down, s);           // Home
            var a = m.Handle(Cmd.Apply, s);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.Command));
            Assert.That(a.Command, Is.EqualTo(Cmd.Home));
            m.Handle(Cmd.Back, s);
            Assert.That(m.Page, Is.EqualTo(MenuModel.PageMain));
            Assert.That(m.Cursor, Is.EqualTo(1));
        }

        [Test]
        public void CommandsPage_LastItemIsGyroTest()
        {
            m.Handle(Cmd.Down, s);
            m.Handle(Cmd.Apply, s);
            m.Handle(Cmd.Up, s);             // wraps to the last item
            Assert.That(m.Handle(Cmd.Apply, s).Command, Is.EqualTo(Cmd.GyroTest));
        }

        [Test]
        public void OtherCommands_DoNothing()
        {
            Assert.That(m.Handle(Cmd.Start, s).Kind, Is.EqualTo(MenuActionKind.None));
            Assert.That(m.Page, Is.EqualTo(MenuModel.PageMain));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~MenuModelTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'MenuModel' could not be found".
- [ ] 3. Create `Fleet.Drone.Miner/Ui/MenuModel.cs`. Keep the Job items in static readonly parallel arrays
      (labels, mins, maxs, steps, decimals) and read/write the value through a `switch` on the item index.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Ui/MenuModel.cs Fleet.Tests/Mining/MenuModelTests.cs; git commit -m "feat(miner): LCD menu model"`

## Done when
All MenuModelTests pass.
