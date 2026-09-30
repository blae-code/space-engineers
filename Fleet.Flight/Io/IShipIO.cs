using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public interface IShipIO
        {
            MatrixD WorldMatrix { get; }       // controller orientation, Translation = centre of mass
            Vector3D LinearVelocity { get; }   // world m/s
            Vector3D AngularVelocity { get; }  // world rad/s
            Vector3D Gravity { get; }          // world m/s^2, zero in space
            double Mass { get; }               // kg
            void GetMaxThrust(double[] maxByDir); // [6] by (int)Base6Directions.Direction: N the group can push the ship that way now
            void SetThrust(double[] ratioByDir);  // [6] override fraction 0..1
            void SetGyro(Vector3D worldOmega);    // desired world angular velocity rad/s (enables override)
            void ReleaseControls();               // overrides off, dampeners on
        }
    }
}
