using System.Numerics;

namespace Sr3D.Utils
{
    public static class MathUtils
    {
        public static float DegreeToRadian(float degree)
        {
            return (float)(degree * System.Math.PI / 180);
        }

        public static float Perp(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(new Vector2(-a.Y, a.X), b);
        }
    }
}
