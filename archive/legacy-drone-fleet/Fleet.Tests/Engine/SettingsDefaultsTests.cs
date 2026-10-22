using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    // Pins every Settings default to the spec value (spec §7.1).
    [TestFixture]
    public class SettingsDefaultsTests
    {
        [Test]
        public void Defaults_MatchSpec()
        {
            var s = new Settings();
            Assert.Multiple(() =>
            {
                Assert.That(s.Name, Is.EqualTo("Miner-01"));
                Assert.That(s.Tag, Is.EqualTo("[FM]"));
                Assert.That(s.Channel, Is.EqualTo("FM"));
                Assert.That(s.Width, Is.EqualTo(5));
                Assert.That(s.Height, Is.EqualTo(5));
                Assert.That(s.Depth, Is.EqualTo(30));
                Assert.That(s.StartDepth, Is.EqualTo(0));
                Assert.That(s.WorkSpeed, Is.EqualTo(1.5));
                Assert.That(s.RetractSpeed, Is.EqualTo(4));
                Assert.That(s.MaxLoad, Is.EqualTo(90));
                Assert.That(s.MinLiftMargin, Is.EqualTo(1.3));
                Assert.That(s.Eject, Is.EqualTo(new[] { "Stone" }));
                Assert.That(s.Loop, Is.True);
                Assert.That(s.OnDamage, Is.EqualTo(DamagePolicy.Home));
                Assert.That(s.MaxSpeed, Is.EqualTo(60));
                Assert.That(s.ApproachSpeed, Is.EqualTo(5));
                Assert.That(s.DockSpeed, Is.EqualTo(0.5));
                Assert.That(s.SafeAltitude, Is.EqualTo(150));
                Assert.That(s.StuckSeconds, Is.EqualTo(5));
                Assert.That(s.MinBattery, Is.EqualTo(20));
                Assert.That(s.MinHydrogen, Is.EqualTo(30));
                Assert.That(s.MinUranium, Is.EqualTo(5));
                Assert.That(s.OnReload, Is.EqualTo(ReloadPolicy.Resume));
            });
        }

        [Test]
        public void NameTables_CoverEveryEnumValue()
        {
            Assert.That(Names.State.Length, Is.EqualTo((int)MinerState.Safe + 1));
            Assert.That(Names.Reason.Length, Is.EqualTo((int)ReturnReason.Manual + 1));
        }
    }
}
