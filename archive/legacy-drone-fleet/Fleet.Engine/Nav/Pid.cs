using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class Pid
        {
            public double Kp, Ki, Kd, IntegralLimit;
            public double Integral { get; private set; }
            
            private double _previousError;
            private bool _hasPreviousError;

            public Pid(double kp, double ki, double kd, double integralLimit)
            {
                Kp = kp;
                Ki = ki;
                Kd = kd;
                IntegralLimit = integralLimit;
                Integral = 0;
                _previousError = 0;
                _hasPreviousError = false;
            }

            public double Update(double error, double dt)
            {
                // Update integral
                if (dt > 0)
                {
                    Integral += error * dt;
                    // Clamp integral
                    if (Integral > IntegralLimit)
                        Integral = IntegralLimit;
                    else if (Integral < -IntegralLimit)
                        Integral = -IntegralLimit;
                }

                // Calculate derivative
                double derivative = 0;
                if (_hasPreviousError && dt > 0)
                {
                    derivative = (error - _previousError) / dt;
                }

                // Store previous error
                _previousError = error;
                _hasPreviousError = true;

                // Return PID output
                return Kp * error + Ki * Integral + Kd * derivative;
            }

            public void Reset()
            {
                Integral = 0;
                _hasPreviousError = false;
            }
        }

        public class PidVec
        {
            public double Kp, Ki, Kd, IntegralLimit;
            public Vector3D Integral { get; private set; }
            
            private Vector3D _previousError;
            private bool _hasPreviousError;

            public PidVec(double kp, double ki, double kd, double integralLimit)
            {
                Kp = kp;
                Ki = ki;
                Kd = kd;
                IntegralLimit = integralLimit;
                Integral = Vector3D.Zero;
                _previousError = Vector3D.Zero;
                _hasPreviousError = false;
            }

            public Vector3D Update(Vector3D error, double dt)
            {
                // Update integral for each component
                if (dt > 0)
                {
                    Integral += error * dt;
                    // Clamp integral for each component
                    double x = Integral.X;
                    if (x > IntegralLimit)
                        x = IntegralLimit;
                    else if (x < -IntegralLimit)
                        x = -IntegralLimit;
                        
                    double y = Integral.Y;
                    if (y > IntegralLimit)
                        y = IntegralLimit;
                    else if (y < -IntegralLimit)
                        y = -IntegralLimit;
                        
                    double z = Integral.Z;
                    if (z > IntegralLimit)
                        z = IntegralLimit;
                    else if (z < -IntegralLimit)
                        z = -IntegralLimit;
                        
                    Integral = new Vector3D(x, y, z);
                }

                // Calculate derivative for each component
                Vector3D derivative = Vector3D.Zero;
                if (_hasPreviousError && dt > 0)
                {
                    derivative = (error - _previousError) / dt;
                }

                // Store previous error
                _previousError = error;
                _hasPreviousError = true;

                // Return PID output for each component
                return new Vector3D(
                    Kp * error.X + Ki * Integral.X + Kd * derivative.X,
                    Kp * error.Y + Ki * Integral.Y + Kd * derivative.Y,
                    Kp * error.Z + Ki * Integral.Z + Kd * derivative.Z
                );
            }

            public void Reset()
            {
                Integral = Vector3D.Zero;
                _hasPreviousError = false;
            }
        }
    }
}