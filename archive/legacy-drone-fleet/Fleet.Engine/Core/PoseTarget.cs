using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public struct PoseTarget
        {
            public Vector3D Position;  // world: where the reference point should go
            public Vector3D Forward;   // world unit vector for the reference forward
            public Vector3D Up;        // world unit vector for the reference up
            public double SpeedCap;    // m/s
            public double BrakeDist;   // metres to brake against; < 0 = straight-line distance to Position
            public bool Precision;     // true = request Update1 (final docking)
            public Vector3D Velocity;  // world m/s of the frame the target moves with (moving carrier); zero = static
        }
    }
}
