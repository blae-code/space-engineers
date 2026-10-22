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