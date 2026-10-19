using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class SbFormatTests
    {
        static string Run(System.Func<StringBuilder, StringBuilder> f) { return f(new StringBuilder()).ToString(); }

        [TestCase(0, "0")]
        [TestCase(7, "7")]
        [TestCase(12345, "12345")]
        [TestCase(-42, "-42")]
        [TestCase(2147483647L, "2147483647")]
        public void AppendInt(long v, string expected) { Assert.That(Run(sb => SbFormat.AppendInt(sb, v)), Is.EqualTo(expected)); }

        [Test]
        public void AppendInt_AppendsToExistingContent()
        {
            var sb = new StringBuilder("x=");
            var ret = SbFormat.AppendInt(sb, 5);
            Assert.That(sb.ToString(), Is.EqualTo("x=5"));
            Assert.That(ret, Is.SameAs(sb));
        }

        [TestCase(1.25, 1, "1.3")]
        [TestCase(-1.25, 1, "-1.3")]
        [TestCase(3.14159, 2, "3.14")]
        [TestCase(2.0, 0, "2")]
        [TestCase(1234.5, 0, "1235")]
        [TestCase(0.05, 1, "0.1")]
        [TestCase(-0.04, 1, "0.0")]
        [TestCase(-0.25, 1, "-0.3")]
        [TestCase(7.0, 3, "7.000")]
        [TestCase(0.5, 2, "0.50")]
        public void AppendFixed(double v, int d, string expected) { Assert.That(Run(sb => SbFormat.AppendFixed(sb, v, d)), Is.EqualTo(expected)); }

        [TestCase(0, "00")]
        [TestCase(7, "07")]
        [TestCase(12, "12")]
        public void AppendPad2(int v, string expected) { Assert.That(Run(sb => SbFormat.AppendPad2(sb, v)), Is.EqualTo(expected)); }

        [TestCase(0.0, "0%")]
        [TestCase(0.905, "91%")]
        [TestCase(1.0, "100%")]
        public void AppendPercent(double v, string expected) { Assert.That(Run(sb => SbFormat.AppendPercent(sb, v)), Is.EqualTo(expected)); }

        [TestCase(0.0, "00:00")]
        [TestCase(125.7, "02:05")]
        [TestCase(3725.0, "62:05")]
        [TestCase(6000.0, "100:00")]
        public void AppendTime(double v, string expected) { Assert.That(Run(sb => SbFormat.AppendTime(sb, v)), Is.EqualTo(expected)); }
    }
}