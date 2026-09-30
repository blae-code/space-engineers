# C08 — Pid and PidVec (feedback controllers)

Milestone A · Difficulty: easy · Executor: local

## Goal
A standard PID controller for scalars and for 3-D vectors, with an integral clamp (anti-windup). The helm
uses `PidVec` to turn a velocity error into an acceleration command.

## Files
- Create: `Fleet.Engine/Nav/Pid.cs` (both classes in this one file)
- Create: `Fleet.Tests/Engine/PidTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, classes nested and `public`.
- C# 6 only. No LINQ. `Update` must not allocate.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class Pid
        {
            public Pid(double kp, double ki, double kd, double integralLimit);
            public double Kp, Ki, Kd, IntegralLimit;
            public double Integral { get; }
            public double Update(double error, double dt);
            public void Reset();
        }

        public class PidVec
        {
            public PidVec(double kp, double ki, double kd, double integralLimit);
            public double Kp, Ki, Kd, IntegralLimit;   // limit applies to each component
            public Vector3D Integral { get; }
            public Vector3D Update(Vector3D error, double dt);
            public void Reset();
        }
    }
}
```

## Behaviour (both classes, per component for PidVec)
- If `dt > 0`: `integral += error * dt`, then clamp to `[-IntegralLimit, +IntegralLimit]`.
- Derivative = `(error - previousError) / dt` only if there was a previous call **and** `dt > 0`; else 0.
- Store `previousError = error` on every call.
- Return `Kp*error + Ki*integral + Kd*derivative`.
- `Reset()`: integral = 0, forget the previous error (next call has no derivative).

## Tests — create `Fleet.Tests/Engine/PidTests.cs` verbatim
```csharp
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
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~PidTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'Pid' could not be found".
- [ ] 3. Create `Fleet.Engine/Nav/Pid.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Nav/Pid.cs Fleet.Tests/Engine/PidTests.cs; git commit -m "feat(engine): PID controllers"`

## Done when
All PidTests pass.
