using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class DamageScannerTests
    {
        [Test]
        public void IsDamaged()
        {
            Assert.That(DamageScanner.IsDamaged(10, 1000, 10, 1000), Is.False);
            Assert.That(DamageScanner.IsDamaged(10, 1000, 9, 1000), Is.True);
            Assert.That(DamageScanner.IsDamaged(10, 1000, 10, 981), Is.False);
            Assert.That(DamageScanner.IsDamaged(10, 1000, 10, 979), Is.True);
        }

        [Test]
        public void NoBaseline_NeverDamaged()
        {
            var d = new DamageScanner();
            Assert.That(d.HasBaseline, Is.False);
            Assert.That(d.Step(), Is.False);
        }
    }
}
