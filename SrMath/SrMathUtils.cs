using System;

namespace SoftwareRenderer3D.SrMath
{
    public class SrMathUtils
    {
        public const float TwoPI = 2 * (float)Math.PI;
        public const float PI = (float)Math.PI;

        public static float SanitizeAngle(float angle)
        {
            angle %= TwoPI;
            if(angle > PI)
            {
                angle -= TwoPI;
            }

            return angle;
        }
    }
}
