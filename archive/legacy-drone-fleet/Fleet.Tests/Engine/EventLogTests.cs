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
