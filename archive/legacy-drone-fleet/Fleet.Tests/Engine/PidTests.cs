using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class PidTests
    {
        [Test]
        public void Proportional()
        {
            var p = new Pid(2, 0, 0, 10);
            Assert.That(p.Update(3, 0.1), Is.EqualTo(6).Within(1e-9));
        }

        [Test]
        public void Integral_Accumulates()
        {
            var p = new Pid(0, 1, 0, 10);
            p.Update(2, 0.5);
            Assert.That(p.Update(2, 0.5), Is.EqualTo(2).Within(1e-9));
            Assert.That(p.Integral, Is.EqualTo(2).Within(1e-9));
        }

        [Test]
        public void Integral_IsClamped()
        {
            var p = new Pid(0, 1, 0, 1);
            p.Update(10, 1);
            Assert.That(p.Integral, Is.EqualTo(1).Within(1e-9));
            p.Update(-100, 1);
            Assert.That(p.Integral, Is.EqualTo(-1).Within(1e-9));
        }

        [Test]
        public void Derivative_NotOnFirstCall()
        {
            var p = new Pid(0, 0, 1, 10);
            Assert.That(p.Update(5, 0.5), Is.EqualTo(0).Within(1e-9));
            Assert.That(p.Update(7, 0.5), Is.EqualTo(4).Within(1e-9));
        }

        [Test]
        public void ZeroDt_NoIntegralNoDerivative()
        {
            var p = new Pid(1, 1, 1, 10);
            p.Update(1, 0.5);
            Assert.That(p.Update(3, 0), Is.EqualTo(3 + 0.5).Within(1e-9)); // kp*3 + ki*0.5 (unchanged) + 0
        }

        [Test]
        public void Reset_ClearsState()
        {
            var p = new Pid(0, 1, 1, 10);
            p.Update(4, 1); p.Update(4, 1);
            p.Reset();
            Assert.That(p.Integral, Is.EqualTo(0));
            Assert.That(p.Update(4, 1), Is.EqualTo(4).Within(1e-9)); // integral 4, no derivative after reset
        }

        [Test]
        public void Vector_PerComponent_WithClamp()
        {
            var p = new PidVec(1, 1, 0, 1);
            var o = p.Update(new Vector3D(2, -3, 0.5), 1);
            // kp*e + ki*clamp(e*dt, 1): (2+1, -3-1, 0.5+0.5)
            Assert.That(Vector3D.Distance(o, new Vector3D(3, -4, 1)), Is.LessThan(1e-9));
            Assert.That(Vector3D.Distance(p.Integral, new Vector3D(1, -1, 0.5)), Is.LessThan(1e-9));
            p.Reset();
            Assert.That(p.Integral.Length(), Is.EqualTo(0));
        }
    }
}