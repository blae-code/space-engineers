using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class Gps
        {
            // [+-]digits[.digits][(e|E)[+-]digits]; ".5" and "5." are valid; at least one mantissa digit; no spaces.
            public static bool TryParseNumber(string s, out double value)
            {
                value = 0;
                if (string.IsNullOrEmpty(s)) return false;
                int i = 0;
                int n = s.Length;
                bool neg = false;
                if (s[i] == '+' || s[i] == '-')
                {
                    neg = s[i] == '-';
                    i++;
                }

                double mant = 0;
                int digits = 0;
                int fracDigits = 0;
                while (i < n && s[i] >= '0' && s[i] <= '9')
                {
                    mant = mant * 10 + (s[i] - '0');
                    digits++;
                    i++;
                }
                if (i < n && s[i] == '.')
                {
                    i++;
                    while (i < n && s[i] >= '0' && s[i] <= '9')
                    {
                        mant = mant * 10 + (s[i] - '0');
                        digits++;
                        fracDigits++;
                        i++;
                    }
                }
                if (digits == 0) return false;

                int exp = 0;
                if (i < n && (s[i] == 'e' || s[i] == 'E'))
                {
                    i++;
                    bool eneg = false;
                    if (i < n && (s[i] == '+' || s[i] == '-'))
                    {
                        eneg = s[i] == '-';
                        i++;
                    }
                    int expDigits = 0;
                    while (i < n && s[i] >= '0' && s[i] <= '9')
                    {
                        if (exp < 10000) exp = exp * 10 + (s[i] - '0');
                        expDigits++;
                        i++;
                    }
                    if (expDigits == 0) return false;
                    if (eneg) exp = -exp;
                }
                if (i != n) return false;

                int scale = exp - fracDigits;
                double v = mant;
                if (scale > 0) v = mant * Math.Pow(10, scale);
                else if (scale < 0) v = mant / Math.Pow(10, -scale);
                value = neg ? -v : v;
                return true;
            }

            // "GPS:<name>:<x>:<y>:<z>:" optionally followed by "<colour>:". Name may contain spaces.
            public static bool TryParse(string s, out Vector3D pos)
            {
                pos = Vector3D.Zero;
                if (s == null) return false;
                s = s.Trim();
                if (!s.StartsWith("GPS:", StringComparison.Ordinal)) return false;
                string[] parts = s.Split(':');
                if (parts.Length < 5) return false;
                double x, y, z;
                if (!TryParseNumber(parts[2], out x)) return false;
                if (!TryParseNumber(parts[3], out y)) return false;
                if (!TryParseNumber(parts[4], out z)) return false;
                pos = new Vector3D(x, y, z);
                return true;
            }
        }

        public class GpsRoute
        {
            public enum Phase { Space, Climb, Cruise, Descend, Done }
            public const double CruiseStep = 100;     // carrot distance ahead while cruising (m)
            public const double DescendRadius = 20;   // start descending when horizontally this close (m)

            private Phase _phase = Phase.Done;
            private Vector3D _target;
            private double _remaining;

            public Phase Current { get { return _phase; } }
            public double Remaining { get { return _remaining; } }
            public Vector3D Target { get { return _target; } }

            // Slice 2: move the end point (a moving carrier) without restarting climb / cruise / descend.
            public void Retarget(Vector3D targetWorld)
            {
                _target = targetWorld;
            }

            public void Start(Vector3D targetWorld, bool inGravity)
            {
                _target = targetWorld;
                _phase = inGravity ? Phase.Climb : Phase.Space;
                _remaining = 0;
            }

            // Returns the world point the helm should fly to this tick.
            public Vector3D Update(Vector3D pos, Vector3D gravity, double elevation, double safeAltitude, double reachDist)
            {
                bool hasGravity = gravity.LengthSquared() >= 1e-6;
                Vector3D up = Vector3D.Zero;
                if (hasGravity) up = -Vector3D.Normalize(gravity);

                Vector3D toTarget = _target - pos;
                double distance = toTarget.Length();

                // At most one transition per tick.
                if ((_phase == Phase.Climb || _phase == Phase.Cruise) && !hasGravity)
                {
                    _phase = Phase.Space;
                }
                else
                {
                    switch (_phase)
                    {
                        case Phase.Space:
                            if (distance <= reachDist) _phase = Phase.Done;
                            break;
                        case Phase.Climb:
                            if (elevation >= 0.95 * safeAltitude) _phase = Phase.Cruise;
                            break;
                        case Phase.Cruise:
                            {
                                Vector3D horizC = toTarget - up * Vector3D.Dot(toTarget, up);
                                if (horizC.Length() <= DescendRadius) _phase = Phase.Descend;
                            }
                            break;
                        case Phase.Descend:
                            if (distance <= reachDist) _phase = Phase.Done;
                            break;
                    }
                }

                if (_phase == Phase.Climb || _phase == Phase.Cruise)
                {
                    double vert = Vector3D.Dot(toTarget, up);
                    Vector3D horiz = toTarget - up * vert;
                    double h = horiz.Length();
                    _remaining = h + Math.Abs(vert);
                    if (_phase == Phase.Climb)
                        return pos + up * (safeAltitude - elevation + 5);
                    Vector3D dir = h > 1e-9 ? horiz / h : Vector3D.Zero;
                    return pos + dir * Math.Min(CruiseStep, h) + up * (safeAltitude - elevation);
                }

                _remaining = distance;
                return _target;
            }
        }
    }
}
