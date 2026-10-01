using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class Blackboard
        {
            public double Time;                 // seconds since script start
            public int SensingTicks;            // clean Update10 sensing passes since boot
            public MatrixD ShipMatrix = MatrixD.Identity; // controller orientation, Translation = centre of mass
            public Vector3D Velocity, AngularVelocity, Gravity;
            public bool InGravity;
            public double Mass;
            public double Elevation; public bool HasElevation;
            public double CargoFill;            // 0..1
            public double BatteryFill = 1, HydrogenFill = 1, UraniumKg;
            public bool HasBattery, HasHydrogen, HasReactor;
            public double LiftMargin = double.PositiveInfinity; // upward thrust / weight; +inf in space
            public bool Damaged;
            public bool HasHome; public long HomeConnectorId; public MatrixD HomeMatrix = MatrixD.Identity;
            public bool Connected, Connectable;
            public Vector3D HomeVelocity;       // world m/s of the home bay (moving carrier, slice 2)
            public bool HomeTracked, HomeLost;  // pose from a fresh carrier beacon / beacon heard, now stale
            public double ShipSize = 5;         // metres, bounding-box diagonal / 2
        }
    }
}
