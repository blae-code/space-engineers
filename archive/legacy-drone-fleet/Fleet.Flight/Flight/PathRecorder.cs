using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class PathRecorder
        {
            public const double MinSpacing = 5;   // metres
            public const double SpacingPerSpeed = 1.0; // metres per (m/s)

            private readonly int _maxPoints;

            public readonly List<Vector3D> Points;

            public PathRecorder(int maxPoints)
            {
                _maxPoints = maxPoints;
                Points = new List<Vector3D>(maxPoints);
            }

            public bool IsFull
            {
                get { return Points.Count >= _maxPoints; }
            }

            public void Begin(Vector3D localPos)
            {
                Points.Clear();
                if (_maxPoints > 0) Points.Add(localPos);
            }

            public bool Update(Vector3D localPos, double speed)
            {
                if (IsFull) return false;
                if (Points.Count == 0)
                {
                    Points.Add(localPos);
                    return true;
                }
                double spacing = speed * SpacingPerSpeed;
                if (spacing < MinSpacing) spacing = MinSpacing;
                if (Vector3D.Distance(localPos, Points[Points.Count - 1]) >= spacing)
                    Points.Add(localPos);
                return true;
            }

            public void End(Vector3D localPos)
            {
                if (IsFull) return;
                if (Points.Count == 0 || Vector3D.Distance(localPos, Points[Points.Count - 1]) > 0.5)
                    Points.Add(localPos);
            }
        }
    }
}
