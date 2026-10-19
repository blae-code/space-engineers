# C18 — Helm (the only code that commands thrusters and gyros)

Milestone B · Difficulty: hard · Executor: local+review

## Goal
Given a target pose for a **reference point** (the ship's centre, its connector, or its drill face), fly
there: a braking-curve velocity target, a PI loop on velocity with gravity compensation, force split across
the thruster groups, and gyro alignment of the reference. All game access goes through `IShipIO`, so the
tests use a fake ship.

## Files
- Create: `Fleet.Flight/Flight/Helm.cs`
- Create: `Fleet.Tests/Flight/HelmTests.cs`

## Attach in Continue
This card, `Fleet.Flight/Io/IShipIO.cs`, `Fleet.Engine/Core/PoseTarget.cs`, `Fleet.Flight/Flight/GyroMath.cs`,
`Fleet.Flight/Flight/ThrustAllocator.cs`, `Fleet.Flight/Flight/BrakingCurve.cs`, `Fleet.Engine/Nav/Pid.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. **`Update` must not allocate**: create the two `double[6]` arrays and the `PidVec`
  in the constructor.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class Helm
        {
            public Helm(IShipIO io);
            public double RotationGain = 2.0;     // rad/s per rad of error
            public double MaxRotationRate = 1.5;  // rad/s
            public double BrakeMargin = 0.8;
            public readonly PidVec VelocityPid = new PidVec(2.0, 0.2, 0.0, 2.0);
            public void SetReference(MatrixD refInShip);  // reference pose in ship-local coords; default MatrixD.Identity
            public void Engage(PoseTarget target);        // sets the target and Active = true (does NOT reset the PID)
            public void Release();                        // io.ReleaseControls(), VelocityPid.Reset(), Active = false
            public bool Active { get; }
            public double Distance { get; }               // reference point to target, set by Update
            public double AlignmentError { get; }         // rad between reference forward and target forward, set by Update
            public void Update(double dt);                // does nothing unless Active
        }
    }
}
```

## Algorithm — `Update(dt)` (write it in this order)
```
ship     = io.WorldMatrix                      // orientation + centre of mass
refWorld = refInShip * ship                    // MatrixD multiply: local * parent = world
toTarget = target.Position - refWorld.Translation
Distance = toTarget.Length()
dir      = Distance > 1e-6 ? toTarget / Distance : Vector3D.Zero
brakeDist = target.BrakeDist >= 0 ? target.BrakeDist : Distance
io.GetMaxThrust(maxThrust)
mass = io.Mass;  g = io.Gravity
// braking capability along -dir (thrust + the part of gravity that helps)
brakeLocal = Vector3D.TransformNormal(-dir, MatrixD.Transpose(ship))
aBrake = ThrustAllocator.MaxForceAlong(brakeLocal, maxThrust) / mass + Vector3D.Dot(g, -dir)
if (aBrake < 0) aBrake = 0
speed = BrakingCurve.SafeSpeed(brakeDist, aBrake, target.SpeedCap, BrakeMargin)
desiredVel = dir * speed
accel = VelocityPid.Update(desiredVel - io.LinearVelocity, dt)
force = (accel - g) * mass                     // cancel gravity
ThrustAllocator.Allocate(Vector3D.TransformNormal(force, MatrixD.Transpose(ship)), maxThrust, ratios)
io.SetThrust(ratios)
// rotation
up = GyroMath.PerpendicularUp(target.Forward, target.Up, refWorld.Up)
GyroMath.ReferenceToShip(refInShip, target.Forward, up, out shipFwd, out shipUp)
io.SetGyro(GyroMath.AlignRate(ship.Forward, ship.Up, shipFwd, shipUp, RotationGain, MaxRotationRate))
AlignmentError = GyroMath.AngleBetween(refWorld.Forward, target.Forward)
```

## Tests — create `Fleet.Tests/Flight/HelmTests.cs` verbatim
```csharp
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    class FakeShipIO : IShipIO
    {
        public MatrixD World = MatrixD.Identity;
        public Vector3D Vel, AngVel, Grav;
        public double M = 1000;
        public double[] Max = { 20000, 20000, 20000, 20000, 20000, 20000 };
        public double[] LastRatios = new double[6];
        public Vector3D LastOmega;
        public int SetThrustCalls;
        public bool Released;
        public MatrixD WorldMatrix { get { return World; } }
        public Vector3D LinearVelocity { get { return Vel; } }
        public Vector3D AngularVelocity { get { return AngVel; } }
        public Vector3D Gravity { get { return Grav; } }
        public double Mass { get { return M; } }
        public void GetMaxThrust(double[] maxByDir) { System.Array.Copy(Max, maxByDir, 6); }
        public void SetThrust(double[] ratioByDir) { System.Array.Copy(ratioByDir, LastRatios, 6); SetThrustCalls++; }
        public void SetGyro(Vector3D worldOmega) { LastOmega = worldOmega; }
        public void ReleaseControls() { Released = true; }
    }

    [TestFixture]
    public class HelmTests
    {
        const int F = 0, B = 1, U = 4;
        const double Dt = 1.0 / 6;
        FakeShipIO io; Helm helm;

        [SetUp] public void Init() { io = new FakeShipIO(); helm = new Helm(io); }

        static PoseTarget At(Vector3D pos, double cap = 10)
        {
            return new PoseTarget { Position = pos, Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), SpeedCap = cap, BrakeDist = -1 };
        }

        [Test]
        public void Hover_CancelsGravity()
        {
            io.Grav = new Vector3D(0, -9.81, 0);
            helm.Engage(At(Vector3D.Zero));
            helm.Update(Dt);
            Assert.That(io.LastRatios[U], Is.EqualTo(9810.0 / 20000).Within(0.01));
        }

        [Test]
        public void FarTargetAhead_ThrustsForward()
        {
            helm.Engage(At(new Vector3D(0, 0, -100)));
            helm.Update(Dt);
            Assert.That(io.LastRatios[F], Is.GreaterThan(0.9));
            Assert.That(io.LastRatios[B], Is.EqualTo(ThrustAllocator.MinOverride));
            Assert.That(helm.Distance, Is.EqualTo(100).Within(1e-9));
        }

        [Test]
        public void TooFastNearTarget_Brakes()
        {
            io.Vel = new Vector3D(0, 0, -50);
            helm.Engage(At(new Vector3D(0, 0, -10), 100));
            helm.Update(Dt);
            Assert.That(io.LastRatios[B], Is.GreaterThan(0.9));
        }

        [Test]
        public void NoBrakingThrust_DoesNotAccelerate()
        {
            io.Max[B] = 0;   // nothing can push the ship backwards, so it could never stop
            helm.Engage(At(new Vector3D(0, 0, -100)));
            helm.Update(Dt);
            Assert.That(io.LastRatios[F], Is.EqualTo(ThrustAllocator.MinOverride));
        }

        [Test]
        public void TurnsTowardTargetForward()
        {
            var t = At(Vector3D.Zero);
            t.Forward = new Vector3D(1, 0, 0);
            helm.Engage(t);
            helm.Update(Dt);
            Assert.That(io.LastOmega.Y, Is.LessThan(-0.1));
            Assert.That(helm.AlignmentError, Is.EqualTo(System.Math.PI / 2).Within(1e-6));
        }

        [Test]
        public void BellyReference_AlreadyAligned_NoRotation()
        {
            helm.SetReference(MatrixD.CreateWorld(new Vector3D(0, -2, 0), new Vector3D(0, -1, 0), new Vector3D(0, 0, -1)));
            helm.Engage(new PoseTarget { Position = new Vector3D(0, -2, 0), Forward = new Vector3D(0, -1, 0), Up = new Vector3D(0, 0, -1), SpeedCap = 1, BrakeDist = -1 });
            helm.Update(Dt);
            Assert.That(helm.Distance, Is.LessThan(1e-9));
            Assert.That(io.LastOmega.Length(), Is.LessThan(1e-6));
            Assert.That(helm.AlignmentError, Is.LessThan(1e-6));
        }

        [Test]
        public void Release_StopsCommanding()
        {
            helm.Engage(At(new Vector3D(0, 0, -100)));
            helm.Release();
            Assert.That(io.Released, Is.True);
            Assert.That(helm.Active, Is.False);
            helm.Update(Dt);
            Assert.That(io.SetThrustCalls, Is.EqualTo(0));
        }

        [Test]
        public void BrakeDistOverride_IsUsed()
        {
            // Target only 1 m away, but 200 m of path remain: must not creep at the 1 m braking speed.
            var t = At(new Vector3D(0, 0, -1), 50);
            t.BrakeDist = 200;
            helm.Engage(t);
            helm.Update(Dt);
            Assert.That(io.LastRatios[F], Is.GreaterThan(0.9));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~HelmTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'Helm' could not be found".
- [ ] 3. Create `Fleet.Flight/Flight/Helm.cs` following the algorithm exactly.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Run all tests: `dotnet test Fleet.Tests 2>&1 | tail -n 25`. Expected: `Passed!`.
- [ ] 6. Commit: `git add Fleet.Flight/Flight/Helm.cs Fleet.Tests/Flight/HelmTests.cs; git commit -m "feat(flight): helm"`

## Done when
All tests pass. Do not tune gains or change the algorithm to make a test pass — report instead.
