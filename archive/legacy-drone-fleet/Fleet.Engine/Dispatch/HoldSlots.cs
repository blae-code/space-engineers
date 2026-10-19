using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // Where a waiting drone holds (slice 3 spec §5): slot n sits (n + 1) * spacing above the carrier's
        // "[FM] Hold" marker block, along the marker's up axis. The marker travels in the bay beacon as BayId -1.
        public static class HoldSlots
        {
            public const long MarkerId = -1;

            public static Vector3D Position(MatrixD markerPose, int slot, double spacing)
            {
                return markerPose.Translation + Vector3D.Normalize(markerPose.Up) * ((slot + 1) * spacing);
            }

            // A WAIT reply's slot: false means hold in place (slot -1 = no marker or queue full; no marker heard).
            public static bool TryPosition(bool haveMarker, MatrixD markerPose, long slot, double spacing, out Vector3D position)
            {
                position = Vector3D.Zero;
                if (!haveMarker || slot < 0) return false;
                position = Position(markerPose, (int)slot, spacing);
                return true;
            }
        }
    }
}
