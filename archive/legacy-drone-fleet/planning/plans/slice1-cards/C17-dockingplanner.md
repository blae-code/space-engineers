# C17 — DockingPlanner

Milestone B · Difficulty: medium · Executor: local

## Goal
Plan docking onto the home connector in phases: fly to a stand-off point in front of it, align connector
to connector, creep in slowly (asking for high-rate `Update1` control in the last 5 m), then connect.
Also provide the undock back-off point. Everything is computed from the connector's **live** world matrix,
so a moved carrier is handled automatically.

## Files
- Create: `Fleet.Flight/Flight/DockingPlanner.cs`
- Create: `Fleet.Tests/Flight/DockingPlannerTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/PoseTarget.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. No allocation.

## Geometry
- The home connector's `WorldMatrix.Forward` points **out of** its face; `homePos = homeConn.Translation`.
- Stand-off point: `homePos + homeFwd * dockDist`.
- The drone connector must face the home connector: target forward = `-homeFwd`, target up = `homeUp`.
- Angle between two unit vectors: `Math.Acos(clamp(Vector3D.Dot(a, b), -1, 1))`.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class DockingPlanner
        {
            public enum Phase { Approach, Align, Creep, Connect, Done, Failed }
            public const double StandoffTolerance = 0.5;   // m
            public const double AlignTolerance = 0.0349;   // rad (2 degrees)
            public const double PrecisionRange = 5;        // m: request Update1 inside this distance while creeping
            public const double CreepTimeout = 30;         // s
            public Phase Current { get; }
            public void Start(double now);                 // Current = Approach
            public PoseTarget Update(double now, MatrixD homeConn, Vector3D droneConnPos, Vector3D droneConnFwd,
                bool connectable, bool connected, double dockDist, double approachSpeed, double dockSpeed);
            public static Vector3D UndockTarget(MatrixD homeConn, double clearance); // homePos + homeFwd * clearance
        }
    }
}
```

## Behaviour — Update
First apply **at most one** transition, then build the target for the resulting phase.
- Transitions (checked in this order):
  1. Any phase except Done: `connected` → Done.
  2. Approach: `Distance(droneConnPos, standoff) <= StandoffTolerance` → Align.
  3. Align: angle(`droneConnFwd`, `-homeFwd`) `<= AlignTolerance` → Creep; remember `creepStart = now`.
  4. Creep: `connectable` → Connect; else `now - creepStart > CreepTimeout` → Failed.
- Targets (every target has `Forward = -homeFwd`, `Up = homeUp`, `BrakeDist = -1`):
  - Approach: `Position = standoff`, `SpeedCap = approachSpeed`, `Precision = false`.
  - Align: `Position = standoff`, `SpeedCap = approachSpeed`, `Precision = false`.
  - Creep: `Position = homePos`, `SpeedCap = dockSpeed`, `Precision = Distance(droneConnPos, homePos) < PrecisionRange`.
  - Connect: `Position = droneConnPos`, `SpeedCap = 0`, `Precision = true`.
  - Done, Failed: `Position = droneConnPos`, `SpeedCap = 0`, `Precision = false`.

## Tests — create `Fleet.Tests/Flight/DockingPlannerTests.cs` verbatim
```csharp
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class DockingPlannerTests
    {
        // Home connector at origin, face pointing world +X, up +Y.
        static readonly MatrixD Home = MatrixD.CreateWorld(Vector3D.Zero, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
        static readonly Vector3D Facing = new Vector3D(-1, 0, 0);
        static void Near(Vector3D a, Vector3D e) { Assert.That(Vector3D.Distance(a, e), Is.LessThan(1e-6), "actual " + a + " expected " + e); }

        PoseTarget Step(DockingPlanner d, double now, Vector3D pos, Vector3D fwd, bool connectable = false, bool connected = false)
        {
            return d.Update(now, Home, pos, fwd, connectable, connected, 5, 5, 0.5);
        }

        [Test]
        public void Approach_TargetsStandoff()
        {
            var d = new DockingPlanner(); d.Start(0);
            var t = Step(d, 0, new Vector3D(20, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Approach));
            Near(t.Position, new Vector3D(5, 0, 0));
            Near(t.Forward, Facing);
            Near(t.Up, new Vector3D(0, 1, 0));
            Assert.That(t.SpeedCap, Is.EqualTo(5));
            Assert.That(t.Precision, Is.False);
            Assert.That(t.BrakeDist, Is.EqualTo(-1));
        }

        [Test]
        public void Align_ThenCreep_WithPrecisionInside5m()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5.3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Align));
            var t = Step(d, 1, new Vector3D(5.3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Creep));
            Near(t.Position, Vector3D.Zero);
            Assert.That(t.SpeedCap, Is.EqualTo(0.5));
            Assert.That(t.Precision, Is.False);
            t = Step(d, 2, new Vector3D(4, 0, 0), Facing);
            Assert.That(t.Precision, Is.True);
        }

        [Test]
        public void Misaligned_StaysInAlign()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5, 0, 0), Facing);
            Step(d, 1, new Vector3D(5, 0, 0), new Vector3D(0, 0, -1));
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Align));
        }

        [Test]
        public void Connectable_ThenConnected()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5, 0, 0), Facing);
            Step(d, 1, new Vector3D(5, 0, 0), Facing);
            var t = Step(d, 2, new Vector3D(1.5, 0, 0), Facing, connectable: true);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Connect));
            Assert.That(t.SpeedCap, Is.EqualTo(0));
            Assert.That(t.Precision, Is.True);
            Step(d, 3, new Vector3D(1.5, 0, 0), Facing, connectable: false, connected: true);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Done));
        }

        [Test]
        public void Creep_TimesOut()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5, 0, 0), Facing);
            Step(d, 1, new Vector3D(5, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Creep));
            Step(d, 30, new Vector3D(3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Creep));
            Step(d, 31.5, new Vector3D(3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Failed));
        }

        [Test]
        public void AlreadyConnected_IsDone()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(1.5, 0, 0), Facing, connected: true);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Done));
        }

        [Test]
        public void UndockTarget()
        {
            Near(DockingPlanner.UndockTarget(Home, 10), new Vector3D(10, 0, 0));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~DockingPlannerTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'DockingPlanner' could not be found".
- [ ] 3. Create `Fleet.Flight/Flight/DockingPlanner.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/DockingPlanner.cs Fleet.Tests/Flight/DockingPlannerTests.cs; git commit -m "feat(flight): docking planner"`

## Done when
All DockingPlannerTests pass.
