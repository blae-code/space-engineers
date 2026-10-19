using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class MenuTests
    {
        Settings _s;
        Menu _m;
        MenuPage _root, _cmds;

        [SetUp]
        public void Build()
        {
            _s = new Settings();
            _root = new MenuPage("Miner-01");
            _cmds = new MenuPage("Commands");
            _cmds.AddCommand("Start", "START").AddCommand("Stop", "STOP", "Stop the drone here?");
            _root.AddPage("Commands", _cmds).AddSettingsPages();
            _m = new Menu(_root, new SettingsValues(_s));
        }

        string Render(int lines = 20)
        {
            var sb = new StringBuilder();
            _m.Render(sb, lines);
            return sb.ToString();
        }

        MenuAction Press(params Cmd[] cmds)
        {
            var a = new MenuAction();
            foreach (var c in cmds) a = _m.Handle(c);
            return a;
        }

        [Test]
        public void Root_ListsCommandsAndEverySettingsGroup()
        {
            Assert.That(Render(), Is.EqualTo(
                "Miner-01\n> Commands >\n  Job settings >\n  Behaviour settings >\n  Flight settings >\n  Energy settings >\n"));
        }

        [Test]
        public void UpDown_WrapAround()
        {
            Assert.That(Press(Cmd.Up).Kind, Is.EqualTo(MenuActionKind.Redraw));
            Assert.That(_root.Cursor, Is.EqualTo(4));
            Press(Cmd.Down);
            Assert.That(_root.Cursor, Is.EqualTo(0));
        }

        [Test]
        public void ApplyOpensPage_BackReturns_TitleIsBreadcrumb()
        {
            Press(Cmd.Down, Cmd.Apply);
            Assert.That(_m.Current.Title, Is.EqualTo("Job"));
            Assert.That(Render(), Does.StartWith("Miner-01 > Job\n> Width           5\n"));
            Press(Cmd.Back);
            Assert.That(_m.Current, Is.SameAs(_root));
            Assert.That(_root.Cursor, Is.EqualTo(1));
            Assert.That(Press(Cmd.Back).Kind, Is.EqualTo(MenuActionKind.None));
        }

        [Test]
        public void Command_ReturnsRunArgumentText()
        {
            var a = Press(Cmd.Apply, Cmd.Apply);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.Command));
            Assert.That(a.Command, Is.EqualTo("START"));
        }

        [Test]
        public void DestructiveCommand_AsksFirst_ApplyConfirms()
        {
            Assert.That(Press(Cmd.Apply, Cmd.Down, Cmd.Apply).Kind, Is.EqualTo(MenuActionKind.Redraw));
            Assert.That(_m.Confirming, Is.True);
            Assert.That(Render(), Is.EqualTo("Miner-01 > Commands\n  Stop the drone here?\n  APPLY = yes   BACK = no\n"));
            var a = Press(Cmd.Apply);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.Command));
            Assert.That(a.Command, Is.EqualTo("STOP"));
        }

        [Test]
        public void DestructiveCommand_AnyOtherKeyCancels()
        {
            Press(Cmd.Apply, Cmd.Down, Cmd.Apply);
            Assert.That(Press(Cmd.Down).Kind, Is.EqualTo(MenuActionKind.Redraw));
            Assert.That(_m.Confirming, Is.False);
            Assert.That(_cmds.Cursor, Is.EqualTo(1), "the cancelling key is consumed, not acted on");
        }

        [Test]
        public void Number_EditStepsClampsAndApplies()
        {
            Press(Cmd.Down, Cmd.Apply, Cmd.Apply);   // Job page, edit Width
            Assert.That(_m.Editing, Is.True);
            Press(Cmd.Up);
            Assert.That(Render(), Does.Contain("> Width           [6]\n"));
            var a = Press(Cmd.Apply);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.SetValue));
            Assert.That(a.Field, Is.EqualTo(SettingsSchema.Width));
            Assert.That(a.Value, Is.EqualTo(6));
            Assert.That(_s.Width, Is.EqualTo(5), "the menu never applies edits itself");
        }

        [Test]
        public void Number_BackCancelsEdit()
        {
            Press(Cmd.Down, Cmd.Apply, Cmd.Apply, Cmd.Up);
            Assert.That(Press(Cmd.Back).Kind, Is.EqualTo(MenuActionKind.Redraw));
            Assert.That(_m.Editing, Is.False);
            Assert.That(_m.Current.Title, Is.EqualTo("Job"), "Back while editing does not leave the page");
        }

        [Test]
        public void Number_RepeatedPressesAccelerate_DirectionChangeResets()
        {
            Press(Cmd.Down, Cmd.Apply, Cmd.Down, Cmd.Down, Cmd.Apply);   // edit Depth (30, step 5)
            for (int i = 0; i < 3; i++) Press(Cmd.Up);
            Assert.That(_m.EditValue, Is.EqualTo(45));                   // 3 x 5
            Press(Cmd.Up);
            Assert.That(_m.StepMultiplier, Is.EqualTo(5));
            Assert.That(_m.EditValue, Is.EqualTo(70));                   // + 25
            Assert.That(Render(), Does.Contain("[70] x5"));
            Press(Cmd.Up, Cmd.Up, Cmd.Up);
            Assert.That(_m.StepMultiplier, Is.EqualTo(10));
            Assert.That(_m.EditValue, Is.EqualTo(170));                  // 70 + 25 + 25 + 50
            Press(Cmd.Down);
            Assert.That(_m.StepMultiplier, Is.EqualTo(1));
        }

        [Test]
        public void Number_ClampsToRange()
        {
            Press(Cmd.Down, Cmd.Apply, Cmd.Apply);                       // Width 5, min 1
            for (int i = 0; i < 10; i++) Press(Cmd.Down);
            Assert.That(_m.EditValue, Is.EqualTo(1));
        }

        [Test]
        public void Decimal_RoundsAwayFloatNoise()
        {
            Press(Cmd.Down, Cmd.Apply);
            for (int i = 0; i < 7; i++) Press(Cmd.Down);                 // MinLiftMargin 1.30, step 0.05
            Press(Cmd.Apply);
            for (int i = 0; i < 3; i++) Press(Cmd.Up);
            Assert.That(_m.EditValue, Is.EqualTo(1.45));
        }

        [Test]
        public void Toggle_FlipsImmediately()
        {
            Press(Cmd.Down, Cmd.Down, Cmd.Apply);                        // Behaviour page
            Assert.That(Render(), Does.Contain("> Loop job        On\n"));
            var a = Press(Cmd.Apply);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.SetValue));
            Assert.That(a.Field, Is.EqualTo(SettingsSchema.Loop));
            Assert.That(a.Value, Is.EqualTo(0));
            Assert.That(_m.Editing, Is.False);
        }

        [Test]
        public void Choice_CyclesWithWrap()
        {
            Press(Cmd.Down, Cmd.Down, Cmd.Apply, Cmd.Down, Cmd.Apply);    // OnDamage = Home
            Press(Cmd.Down);
            Assert.That(_m.EditValue, Is.EqualTo(2));                    // wraps to Stop
            Assert.That(Render(), Does.Contain("> On damage       [Stop]\n"));
            Press(Cmd.Up, Cmd.Up);
            Assert.That(_m.EditValue, Is.EqualTo(1));
        }

        [Test]
        public void Scrolling_KeepsCursorVisible_WithMoreMarkers()
        {
            Press(Cmd.Down, Cmd.Apply);                                  // Job page: 8 items
            Assert.That(Render(5), Is.EqualTo(
                "Miner-01 > Job\n\n> Width           5\n  Height          5\n  v 6 more\n"));
            for (int i = 0; i < 4; i++) Press(Cmd.Down);
            Assert.That(Render(5), Is.EqualTo(
                "Miner-01 > Job\n  ^ 3 more\n  Start depth m   0\n> Drill speed     1.5\n  v 3 more\n"));
            Press(Cmd.Up, Cmd.Up, Cmd.Up, Cmd.Up, Cmd.Up);               // wraps to the last item
            Assert.That(Render(5), Is.EqualTo(
                "Miner-01 > Job\n  ^ 6 more\n  Max load %      90\n> Min lift        1.30\n\n"));
        }

        class FakeValues : IMenuValues
        {
            public bool Known, Pending;
            public bool TryGet(int field, out double value) { value = 7; return Known; }
            public bool IsPending(int field) { return Pending; }
        }

        [Test]
        public void UnknownValues_RenderDashes_AndCannotBeEdited()
        {
            var v = new FakeValues();
            _m.Values = v;
            Press(Cmd.Down, Cmd.Apply);
            Assert.That(Render(), Does.Contain("> Width           --\n"));
            Assert.That(Press(Cmd.Apply).Kind, Is.EqualTo(MenuActionKind.None));
            Assert.That(_m.Editing, Is.False);
            v.Known = true; v.Pending = true;
            Assert.That(Render(), Does.Contain("> Width           7 *\n"));
        }

        class Roster : IMenuList
        {
            public int N;
            public int Count { get { return N; } }
            public void AppendEntry(StringBuilder sb, int i) { sb.Append("drone ").Append((char)('A' + i)); }
        }

        [Test]
        public void ListPage_RendersEntries_ApplySelects()
        {
            var roster = new Roster();
            var fleet = new MenuPage("Fleet") { List = roster };
            var m = new Menu(fleet, null);
            var sb = new StringBuilder();
            m.Render(sb, 10);
            Assert.That(sb.ToString(), Is.EqualTo("Fleet\n  (none)\n"));
            Assert.That(m.Handle(Cmd.Apply).Kind, Is.EqualTo(MenuActionKind.None));
            roster.N = 3;
            m.Handle(Cmd.Up);
            var a = m.Handle(Cmd.Apply);
            Assert.That(a.Kind, Is.EqualTo(MenuActionKind.Select));
            Assert.That(a.Index, Is.EqualTo(2));
            sb.Clear(); m.Render(sb, 10);
            Assert.That(sb.ToString(), Is.EqualTo("Fleet\n  drone A\n  drone B\n> drone C\n"));
        }

        [Test]
        public void ListShrinking_ClampsCursor()
        {
            var roster = new Roster { N = 3 };
            var fleet = new MenuPage("Fleet") { List = roster };
            var m = new Menu(fleet, null);
            m.Handle(Cmd.Up);
            roster.N = 1;
            var sb = new StringBuilder();
            m.Render(sb, 10);
            Assert.That(sb.ToString(), Is.EqualTo("Fleet\n> drone A\n"));
        }

        [Test]
        public void Open_JumpsToPage_AndCancelsEdit()
        {
            Press(Cmd.Down, Cmd.Apply, Cmd.Apply);
            _m.Open(_cmds);
            Assert.That(_m.Editing, Is.False);
            Assert.That(_m.Current, Is.SameAs(_cmds));
        }
    }
}
