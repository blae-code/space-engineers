using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class PathFollower
        {
            public const double CornerFreeAngle = 0.349;  // radians (20 degrees): no slowdown below this turn
            public const double MinCornerSpeed = 3;       // m/s
            
            private List<Vector3D> _path;
            private bool _reverse;
            private int _index;
            private bool _done;

            public void Start(List<Vector3D> path, bool reverse)
            {
                _path = path;
                _reverse = reverse;
                _index = 0;
                _done = (path == null || path.Count == 0);
            }

            public void StartNearest(List<Vector3D> path, bool reverse, Vector3D localPos)
            {
                _path = path;
                _reverse = reverse;
                
                if (path == null || path.Count == 0)
                {
                    _index = 0;
                    _done = true;
                    return;
                }
                
                // Find the nearest point
                double minDist = double.MaxValue;
                int nearestIndex = 0;
                
                for (int i = 0; i < path.Count; i++)
                {
                    double dist = Vector3D.Distance(localPos, path[i]);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestIndex = i;
                    }
                }
                
                // Set index to the traversal index of the nearest point
                _index = reverse ? path.Count - 1 - nearestIndex : nearestIndex;
                _done = false;
            }

            public bool Done { get { return _done; } }
            public int Index { get { return _index; } }

            // Returns the current waypoint (local frame).
            public Vector3D Update(Vector3D localPos, double reachDist, double maxSpeed, out double speedCap, out double remaining)
            {
                speedCap = maxSpeed;
                remaining = 0;

                if (_path == null || _path.Count == 0)
                {
                    return localPos;
                }

                // Skip reached points
                int last = _path.Count - 1;
                while (_index < last && Vector3D.Distance(localPos, GetPathPoint(_index)) <= reachDist)
                {
                    _index++;
                }

                // Check if we've reached the final point
                if (_index == last && Vector3D.Distance(localPos, GetPathPoint(_index)) <= reachDist)
                {
                    _done = true;
                }

                // Calculate remaining distance
                remaining = 0;
                for (int i = _index; i < last; i++)
                {
                    remaining += Vector3D.Distance(GetPathPoint(i), GetPathPoint(i + 1));
                }
                remaining += Vector3D.Distance(localPos, GetPathPoint(_index));

                // Calculate speed cap based on corner angle
                if (_index < last)
                {
                    Vector3D incoming;
                    if (_index == 0)
                    {
                        // For the first point, use localPos as the previous point
                        incoming = GetPathPoint(_index) - localPos;
                    }
                    else
                    {
                        incoming = GetPathPoint(_index) - GetPathPoint(_index - 1);
                    }
                    
                    Vector3D outgoing = GetPathPoint(_index + 1) - GetPathPoint(_index);
                    
                    // Handle edge case where either vector is too short
                    if (incoming.Length() < 1e-6 || outgoing.Length() < 1e-6)
                    {
                        speedCap = maxSpeed;
                    }
                    else
                    {
                        double angle = Vector3D.Angle(incoming, outgoing);
                        
                        if (angle < CornerFreeAngle)
                        {
                            speedCap = maxSpeed;
                        }
                        else
                        {
                            speedCap = System.Math.Max(MinCornerSpeed, maxSpeed * (1 - angle / System.Math.PI));
                        }
                    }
                }

                return GetPathPoint(_index);
            }

            private Vector3D GetPathPoint(int index)
            {
                if (_reverse)
                {
                    return _path[_path.Count - 1 - index];
                }
                else
                {
                    return _path[index];
                }
            }
        }
    }
}