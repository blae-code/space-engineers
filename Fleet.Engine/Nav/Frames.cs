using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class Frames
        {
            public static Vector3D ToLocalPoint(MatrixD frame, Vector3D worldPoint)
            {
                return Vector3D.TransformNormal(worldPoint - frame.Translation, MatrixD.Transpose(frame));
            }

            public static Vector3D ToWorldPoint(MatrixD frame, Vector3D localPoint)
            {
                return Vector3D.TransformNormal(localPoint, frame) + frame.Translation;
            }

            public static Vector3D ToLocalDir(MatrixD frame, Vector3D worldDir)
            {
                return Vector3D.TransformNormal(worldDir, MatrixD.Transpose(frame));
            }

            public static Vector3D ToWorldDir(MatrixD frame, Vector3D localDir)
            {
                return Vector3D.TransformNormal(localDir, frame);
            }
        }
    }
}