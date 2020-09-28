using Sr3D.Core;
using System;
using System.Numerics;
using System.Windows;

namespace Sr3D.Utils
{
    public static class MathUtils
    {
        public static float DegreeToRadian(float degree)
        {
            return (float)(degree * Math.PI / 180);
        }

        /// <summary>
        /// Maps coordinates from device space (-1.0 and 1.0) to the specified screen space.
        /// </summary>
        /// <param name="p">The device coordinate.</param>
        /// <param name="screenWidth">The width of the screen in pixels.</param>
        /// <param name="screenHeight">The height of the screen in pixels.</param>
        /// <returns>A screen coordinate.</returns>
        public static Int32Point DeviceToScreenCoordinates(Point p, int screenWidth, int screenHeight)
        {
            return new Int32Point((int)(p.X * screenWidth), (int)(p.Y * screenHeight));
        }

        public static float Perp(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(new Vector2(-a.Y, a.X), b);
        }
    }
}
