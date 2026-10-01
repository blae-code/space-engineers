using VRageMath;
using System;

namespace IngameScript
{
    public partial class Program
    {
        public static class ThrustAllocator
        {
            public const double MinOverride = 1e-5;

            // localForce in newtons (ship-local). maxThrust[6]: newtons each group can push now.
            // Writes ratios[6] in [MinOverride, 1]. Returns saturation = max over used axes of |F_axis| / maxThrust[group]
            // (0 for a zero force; double.PositiveInfinity if a needed group has maxThrust <= 0).
            public static double Allocate(Vector3D localForce, double[] maxThrust, double[] ratios)
            {
                double saturation = 0.0;

                // 1. Initialize all ratios to MinOverride
                for (int i = 0; i < 6; i++)
                {
                    ratios[i] = MinOverride;
                }

                // --- X Axis (Right/Left) ---
                if (Math.Abs(localForce.X) > 1e-9)
                {
                    int groupIndex;
                    double maxThrustValue;
                    bool infiniteSaturation = false;

                    if (localForce.X > 0) // Right -> Group 3
                    {
                        groupIndex = 3;
                        maxThrustValue = maxThrust[groupIndex];
                    }
                    else // Left -> Group 2
                    {
                        groupIndex = 2;
                        maxThrustValue = maxThrust[groupIndex];
                    }

                    double r;
                    if (maxThrustValue <= 0)
                    {
                        saturation = double.PositiveInfinity;
                        r = MinOverride; // Ratio stays MinOverride as per requirement
                        infiniteSaturation = true;
                    }
                    else
                    {
                        r = Math.Abs(localForce.X) / maxThrustValue;
                        double clampedR = Math.Max(MinOverride, Math.Min(1.0, r));
                        ratios[groupIndex] = clampedR;
                        saturation = Math.Max(saturation, r);
                    }

                    if (infiniteSaturation)
                    {
                        // If saturation is already infinite, we don't need to update it again.
                        // But we must ensure the ratio remains MinOverride.
                        ratios[groupIndex] = MinOverride; 
                    }
                }

                // --- Y Axis (Up/Down) ---
                if (Math.Abs(localForce.Y) > 1e-9)
                {
                    int groupIndex;
                    double maxThrustValue;
                    bool infiniteSaturation = false;

                    if (localForce.Y > 0) // Up -> Group 4
                    {
                        groupIndex = 4;
                        maxThrustValue = maxThrust[groupIndex];
                    }
                    else // Down -> Group 5
                    {
                        groupIndex = 5;
                        maxThrustValue = maxThrust[groupIndex];
                    }

                    double r;
                    if (maxThrustValue <= 0)
                    {
                        saturation = double.PositiveInfinity;
                        r = MinOverride;
                        infiniteSaturation = true;
                    }
                    else
                    {
                        r = Math.Abs(localForce.Y) / maxThrustValue;
                        double clampedR = Math.Max(MinOverride, Math.Min(1.0, r));
                        ratios[groupIndex] = clampedR;
                        saturation = Math.Max(saturation, r);
                    }

                    if (infiniteSaturation)
                    {
                        ratios[groupIndex] = MinOverride; 
                    }
                }

                // --- Z Axis (Forward/Backward) ---
                if (Math.Abs(localForce.Z) > 1e-9)
                {
                    int groupIndex;
                    double maxThrustValue;
                    bool infiniteSaturation = false;

                    if (localForce.Z < 0) // Forward -> Group 0
                    {
                        groupIndex = 0;
                        maxThrustValue = maxThrust[groupIndex];
                    }
                    else // Backward -> Group 1
                    {
                        groupIndex = 1;
                        maxThrustValue = maxThrust[groupIndex];
                    }

                    double r;
                    if (maxThrustValue <= 0)
                    {
                        saturation = double.PositiveInfinity;
                        r = MinOverride;
                        infiniteSaturation = true;
                    }
                    else
                    {
                        r = Math.Abs(localForce.Z) / maxThrustValue;
                        double clampedR = Math.Max(MinOverride, Math.Min(1.0, r));
                        ratios[groupIndex] = clampedR;
                        saturation = Math.Max(saturation, r);
                    }

                    if (infiniteSaturation)
                    {
                        ratios[groupIndex] = MinOverride; 
                    }
                }

                return saturation;
            }

            // Largest force magnitude achievable along the unit ship-local direction localDir:
            // min over components with |d_i| > 1e-9 of maxThrust[group(d_i)] / |d_i|. 0 if localDir is ~zero or any needed group is 0.
            public static double MaxForceAlong(Vector3D localDir, double[] maxThrust)
            {
                double minRatio = double.PositiveInfinity;
                bool foundDirectionComponent = false;

                // X Axis (Right/Left)
                if (Math.Abs(localDir.X) > 1e-9)
                {
                    foundDirectionComponent = true;
                    int groupIndex = localDir.X < 0 ? 2 : 3; // Left=2, Right=3
                    double ratio = maxThrust[groupIndex] / Math.Abs(localDir.X);
                    minRatio = Math.Min(minRatio, ratio);
                }

                // Y Axis (Up/Down)
                if (Math.Abs(localDir.Y) > 1e-9)
                {
                    foundDirectionComponent = true;
                    int groupIndex = localDir.Y > 0 ? 4 : 5; // Up=4, Down=5
                    double ratio = maxThrust[groupIndex] / Math.Abs(localDir.Y);
                    minRatio = Math.Min(minRatio, ratio);
                }

                // Z Axis (Forward/Backward)
                if (Math.Abs(localDir.Z) > 1e-9)
                {
                    foundDirectionComponent = true;
                    int groupIndex = localDir.Z < 0 ? 0 : 1; // Forward=0, Backward=1
                    double ratio = maxThrust[groupIndex] / Math.Abs(localDir.Z);
                    minRatio = Math.Min(minRatio, ratio);
                }

                if (!foundDirectionComponent)
                {
                    return 0.0;
                }

                // Check for zero capacity groups required by the direction vector components
                if (Math.Abs(localDir.X) > 1e-9 && maxThrust[localDir.X < 0 ? 2 : 3] <= 0) return 0.0;
                if (Math.Abs(localDir.Y) > 1e-9 && maxThrust[localDir.Y > 0 ? 4 : 5] <= 0) return 0.0;
                if (Math.Abs(localDir.Z) > 1e-9 && maxThrust[localDir.Z < 0 ? 0 : 1] <= 0) return 0.0;


                return minRatio;
            }
        }
    }
}