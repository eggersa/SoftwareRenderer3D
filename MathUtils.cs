using System;

namespace SoftwareRenderer
{
    public static class MathUtils
    {
        public static float DegreeToRadian(float degree)
        {
            return (float)(degree * Math.PI / 180);
        }
    }
}
