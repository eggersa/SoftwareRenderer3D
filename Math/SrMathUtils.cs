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

        public static float DegreeToRadian(float degree)
        {
            return (float)(degree * System.Math.PI / 180);
        }

        public static float Perp(System.Numerics.Vector2 a, System.Numerics.Vector2 b)
        {
            return System.Numerics.Vector2.Dot(new System.Numerics.Vector2(-a.Y, a.X), b);
        }

        /// <summary>
        /// The perp product is equal to |a|*|b|*sin(alpha). Sometimes referred to as the 2D equivalent of the cross product.
        /// See <seealso cref="https://www.geogebra.org/calculator/whbtnbex"/> for simulation.
        /// </summary>
        /// <param name="a">The vector a.</param>
        /// <param name="b">The vector b.</param>
        /// <returns>The perp product of vector a and b.</returns>
        /// <remarks>https://mathworld.wolfram.com/PerpDotProduct.html</remarks>
        public static float Perp(SoftwareRenderer3D.SrMath.Vector2 a, SoftwareRenderer3D.SrMath.Vector2 b)
        {
            return SoftwareRenderer3D.SrMath.Vector2.Dot(new SoftwareRenderer3D.SrMath.Vector2(-a.Y, a.X), b);
        }
    }
}
