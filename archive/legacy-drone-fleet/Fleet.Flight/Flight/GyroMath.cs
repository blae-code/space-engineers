using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class GyroMath
        {
            // Derived from the right-hand rule; VERIFIED IN-GAME by GYROTEST (checklist G1). Change only after G1.
            public const double PitchSign = 1, YawSign = -1, RollSign = -1;

            // Unit inputs; radians 0..PI (the dot product is clamped before Acos).
            public static double AngleBetween(Vector3D a, Vector3D b)
            {
                double d = Vector3D.Dot(a, b);
                if (d > 1) d = 1;
                else if (d < -1) d = -1;
                return Math.Acos(d);
            }

            // Rotation vector (unit axis * angle) taking unit 'from' onto unit 'to'.
            // Parallel -> Zero. Anti-parallel -> axis = fallbackAxis made perpendicular to 'from' (if that is
            // degenerate, the longer of Cross(from, X) / Cross(from, Y)), angle PI.
            public static Vector3D AxisAngle(Vector3D from, Vector3D to, Vector3D fallbackAxis)
            {
                Vector3D c = Vector3D.Cross(from, to);
                double len = c.Length();
                if (len < 1e-9)
                {
                    if (Vector3D.Dot(from, to) > 0) return Vector3D.Zero;
                    Vector3D axis = fallbackAxis - from * Vector3D.Dot(fallbackAxis, from);
                    if (axis.Length() < 1e-6)
                    {
                        Vector3D cx = Vector3D.Cross(from, Vector3D.Right);
                        Vector3D cy = Vector3D.Cross(from, Vector3D.Up);
                        axis = cx.Length() >= cy.Length() ? cx : cy;
                    }
                    return Vector3D.Normalize(axis) * Math.PI;
                }
                return (c / len) * AngleBetween(from, to);
            }

            // World angular-velocity command (rad/s), clamped to maxRate.
            public static Vector3D AlignRate(Vector3D curFwd, Vector3D curUp, Vector3D tgtFwd, Vector3D tgtUp, double gain, double maxRate)
            {
                Vector3D e = AxisAngle(curFwd, tgtFwd, curUp) + AxisAngle(curUp, tgtUp, curFwd);
                Vector3D omega = e * gain;
                double len = omega.Length();
                if (len > maxRate && len > 0) omega = omega * (maxRate / len);
                return omega;
            }

            // World omega -> one gyro's (pitch, yaw, roll) override.
            public static Vector3D ToGyroOverride(Vector3D worldOmega, MatrixD gyroWorld)
            {
                Vector3D l = Vector3D.TransformNormal(worldOmega, MatrixD.Transpose(gyroWorld));
                return new Vector3D(PitchSign * l.X, YawSign * l.Y, RollSign * l.Z);
            }

            // refInShip = pose of the reference block in ship-local coordinates (world = refInShip * shipWorld).
            // Returns the ship forward/up (world) that make the reference face tgtRefFwd with up tgtRefUp.
            public static void ReferenceToShip(MatrixD refInShip, Vector3D tgtRefFwd, Vector3D tgtRefUp, out Vector3D shipFwd, out Vector3D shipUp)
            {
                MatrixD tgt = MatrixD.CreateWorld(Vector3D.Zero, tgtRefFwd, tgtRefUp);
                MatrixD inv = MatrixD.Transpose(refInShip);
                shipFwd = Vector3D.TransformNormal(Vector3D.TransformNormal(Vector3D.Forward, inv), tgt);
                shipUp = Vector3D.TransformNormal(Vector3D.TransformNormal(Vector3D.Up, inv), tgt);
            }

            // Unit vector = up minus its component along fwd; falls back to fallbackUp, then to any perpendicular.
            public static Vector3D PerpendicularUp(Vector3D fwd, Vector3D up, Vector3D fallbackUp)
            {
                Vector3D p = up - fwd * Vector3D.Dot(up, fwd);
                if (p.Length() >= 1e-3) return Vector3D.Normalize(p);
                p = fallbackUp - fwd * Vector3D.Dot(fallbackUp, fwd);
                if (p.Length() >= 1e-3) return Vector3D.Normalize(p);
                Vector3D cx = Vector3D.Cross(fwd, Vector3D.Right);
                Vector3D cy = Vector3D.Cross(fwd, Vector3D.Up);
                return Vector3D.Normalize(cx.Length() >= cy.Length() ? cx : cy);
            }
        }
    }
}
