# C25 — ShipIO (the adapter between the helm and real blocks)

Milestone D · Difficulty: hard · Executor: **Claude** (a local model may attempt it; Claude reviews line by line)

## Goal
Implement `IShipIO` over real Space Engineers blocks: group thrusters by the direction they push the ship,
read effective thrust, write overrides, drive gyros, and release everything safely. Also provide the
`GYROTEST` measurement that verifies gyro signs and units in-game (Review Focus 5).

## Files
- Create: `Fleet.Flight/Io/ShipIO.cs`
- Create: `Fleet.Tests/Flight/ShipIOTests.cs` (pure helpers only)

## Attach in Continue
This card, `Fleet.Flight/Io/IShipIO.cs`, `Fleet.Flight/Flight/GyroMath.cs`, `Fleet.Engine/Util/SbFormat.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. Only `Refresh` may allocate. All per-tick members must not allocate.

## Space Engineers API you need
```csharp
using Sandbox.ModAPI.Ingame; using VRageMath; using System.Collections.Generic;
IMyShipController ctrl;                 // cockpit or remote control
MatrixD m = ctrl.WorldMatrix;           // orientation; ctrl.CenterOfMass is the world CoM
MyShipVelocities v = ctrl.GetShipVelocities(); // v.LinearVelocity, v.AngularVelocity (world, rad/s)
Vector3D g = ctrl.GetNaturalGravity();
double mass = ctrl.CalculateShipMass().PhysicalMass;
double elev; bool ok = ctrl.TryGetPlanetElevation(MyPlanetElevation.Surface, out elev);
ctrl.DampenersOverride = true;          // inertia dampeners on
IMyThrust t;  t.MaxEffectiveThrust (float, N, already scaled for atmosphere/altitude)
              t.ThrustOverridePercentage = 0.5f;   // 0..1; 0 = no override
              t.WorldMatrix.Backward               // direction the thruster pushes the ship
              t.IsFunctional, t.Enabled
IMyGyro gy;   gy.GyroOverride = true; gy.Pitch = x; gy.Yaw = y; gy.Roll = z;   // floats
              gy.WorldMatrix
Base6Directions.GetClosestDirection((Vector3)localVector)  // -> Base6Directions.Direction
```
Gyro override values are believed to be **rad/s**; `GyroScale` exists so GYROTEST can correct this if
the game turns out to use RPM (ratio would read ≈ 9.55).

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class ShipIO : IShipIO
        {
            public double GyroScale = 1.0;      // multiply override values; set from GYROTEST if needed
            public ShipIO();
            // Rebinds blocks (call after every block scan). Groups thrusters by push direction in ctrl's frame.
            public void Refresh(IMyShipController ctrl, List<IMyThrust> thrusters, List<IMyGyro> gyros);
            public bool TryGetElevation(out double elevation);
            // pure helper, unit-tested: which ship-local group a thruster belongs to
            public static int GroupOf(MatrixD shipWorld, Vector3D thrusterPushWorld);
            // IShipIO members (see the interface file): WorldMatrix (orientation of ctrl, Translation = CenterOfMass),
            // LinearVelocity, AngularVelocity, Gravity, Mass, GetMaxThrust, SetThrust, SetGyro, ReleaseControls.
            // GYROTEST support:
            public void BeginGyroTest(double now);   // commands 0.5 rad/s about ship Up for 2 s
            public bool GyroTestRunning { get; }
            // Call every Update10 while running. When it finishes, writes into 'report' e.g.
            // "GYROTEST yaw: measured/commanded = 0.98 (OK)" or "... = -1.02 (SIGN FLIPPED)" or "... = 9.5 (UNITS: RPM?)".
            public void UpdateGyroTest(double now, System.Text.StringBuilder report);
        }
    }
}
```

## Behaviour
- `GroupOf`: `local = TransformNormal(pushWorld, Transpose(shipWorld))`, return
  `(int)Base6Directions.GetClosestDirection((Vector3)local)`.
- `Refresh`: store the controller; clear six preallocated `List<IMyThrust>`; add each thruster to
  `GroupOf(ctrl.WorldMatrix, t.WorldMatrix.Backward)`; keep the gyro list reference.
- `GetMaxThrust`: sum `MaxEffectiveThrust` of functional, enabled thrusters per group.
- `SetThrust`: every thruster in group *g* gets `(float)ratio[g]`.
- `SetGyro`: for each gyro: `o = GyroMath.ToGyroOverride(omega, gy.WorldMatrix) * GyroScale`;
  `GyroOverride = true`; assign Pitch/Yaw/Roll.
- `ReleaseControls`: all thrust overrides 0; all gyros `GyroOverride = false` and Pitch/Yaw/Roll 0;
  `ctrl.DampenersOverride = true`. Must be safe to call when `Refresh` was never called (no controller).
- Before `Refresh` (no controller) every property returns a safe default — `WorldMatrix` = Identity,
  vectors Zero, `Mass` = 0 — and `GetMaxThrust` fills zeros; nothing throws.
- The GYROTEST report **replaces** the StringBuilder's content (`report.Clear()` first); write the ratio
  with `SbFormat.AppendFixed(report, ratio, 2)`.
- GYROTEST: while running, `SetGyro(shipUp * 0.5)` each call; after 2 s compare
  `Dot(AngularVelocity, shipUp)` with 0.5 and report the ratio with 2 decimals; then `ReleaseControls()`.
  Ratio 0.7..1.3 → `OK`; −1.3..−0.7 → `SIGN FLIPPED`; > 5 → `UNITS: RPM?`; otherwise `UNEXPECTED`.

## Tests — create `Fleet.Tests/Flight/ShipIOTests.cs` verbatim
```csharp
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class ShipIOTests
    {
        const int F = 0, B = 1, L = 2, R = 3, U = 4, D = 5;

        [Test]
        public void GroupOf_IdentityShip()
        {
            Assert.That(ShipIO.GroupOf(MatrixD.Identity, new Vector3D(0, 0, -1)), Is.EqualTo(F));
            Assert.That(ShipIO.GroupOf(MatrixD.Identity, new Vector3D(0, 1, 0)), Is.EqualTo(U));
            Assert.That(ShipIO.GroupOf(MatrixD.Identity, new Vector3D(-1, 0, 0)), Is.EqualTo(L));
        }

        [Test]
        public void GroupOf_RotatedShip()
        {
            // Ship facing world +X: a thruster pushing world +X pushes the ship Forward.
            var ship = MatrixD.CreateWorld(Vector3D.Zero, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
            Assert.That(ShipIO.GroupOf(ship, new Vector3D(1, 0, 0)), Is.EqualTo(F));
            Assert.That(ShipIO.GroupOf(ship, new Vector3D(0, 0, 1)), Is.EqualTo(R));
        }

        [Test]
        public void ReleaseWithoutRefresh_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new ShipIO().ReleaseControls());
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly; run
      `dotnet test Fleet.Tests --filter "FullyQualifiedName~ShipIOTests" 2>&1 | tail -n 25` → expect a build error.
- [ ] 2. Create `Fleet.Flight/Io/ShipIO.cs`.
- [ ] 3. Run the same command → `Passed!`.
- [ ] 4. `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15` → "successfully deployed" (proves the
      game API calls compile and pass MDK's analyzers).
- [ ] 5. Commit: `git add Fleet.Flight/Io/ShipIO.cs Fleet.Tests/Flight/ShipIOTests.cs; git commit -m "feat(flight): ShipIO adapter with GYROTEST"`

## Done when
Tests pass, Release build is clean. In-game behaviour is verified at checklist item G1.
