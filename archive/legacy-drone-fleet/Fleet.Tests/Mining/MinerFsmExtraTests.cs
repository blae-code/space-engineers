using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    // Behaviour added to MinerFsm after the 2026-09-30 integration review (C23's own tests are verbatim).
    [TestFixture]
    public class MinerFsmExtraTests
    {
        static readonly Settings S = new Settings();

        static MinerInput Cmd_(Cmd c) { return new MinerInput { Command = c, HasJob = true, Connected = false }; }

        [Test]
        public void HomeDuringRetract_FinishesRetract_ThenRoutesBack()
        {
            var f = new MinerFsm(MinerState.Retract);
            Assert.That(f.Step(Cmd_(Cmd.Home), S), Is.False, "no state change: keep pulling out");
            Assert.That(f.State, Is.EqualTo(MinerState.Retract));
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.Manual));
            f.Step(new MinerInput { RetractDone = true, HasJob = true }, S);
            Assert.That(f.State, Is.EqualTo(MinerState.RouteBack));
        }

        [TestCase(MinerState.Drill)]
        [TestCase(MinerState.Retract)]
        public void HomeFromHold_InsideAHole_RetractsFirst(MinerState heldIn)
        {
            var f = new MinerFsm(heldIn);
            f.Step(Cmd_(Cmd.Stop), S);
            Assert.That(f.State, Is.EqualTo(MinerState.Hold));
            f.Step(Cmd_(Cmd.Home), S);
            Assert.That(f.State, Is.EqualTo(MinerState.Retract));
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.Manual));
        }

        [Test]
        public void HomeFromHold_OnRoute_StillRoutesBack()
        {
            var f = new MinerFsm(MinerState.RouteOut);
            f.Step(Cmd_(Cmd.Stop), S);
            f.Step(Cmd_(Cmd.Home), S);
            Assert.That(f.State, Is.EqualTo(MinerState.RouteBack));
        }

        [Test]
        public void Restore_LetsContResumeAfterReload()
        {
            var f = new MinerFsm(MinerState.Idle);
            f.Force(MinerState.Hold, "was holding before reload");
            f.Restore(MinerState.Retract, ReturnReason.Damage);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.Damage));
            f.Step(Cmd_(Cmd.Cont), S);
            Assert.That(f.State, Is.EqualTo(MinerState.Retract));
        }
    }
}
