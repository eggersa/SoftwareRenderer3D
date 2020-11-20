#define USE_NUMERICS

using System;
using SrMathUtils = CSharpRenderer.SrMath.SrMathUtils;
using System.Drawing;
using CSharpRenderer.Core;

#if USE_NUMERICS
using System.Numerics;
#else
using CSharpRenderer.SrMath;
#endif

namespace CSharpRenderer.Graphics
{
    // https://www.cs.drexel.edu/~david/Classes/Papers/comp175-06-pineda.pdf
    // https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage
    public class EdgeRasterizer : ITriangleRasterizer
    {
        private readonly Surface surface;

        public EdgeRasterizer(Surface surface)
        {
            this.surface = surface;
        }

        public void FillTriangle(Color c, Int32Point[] vertices, Int32Point[] uv)
        {
            OrderCounterclockWise(ref vertices[0], ref vertices[1], ref vertices[2]);

            // Compute axis-aligned bounding box (AABB) of triangle
            int xmin = Math.Min(Math.Min(vertices[0].X, vertices[1].X), vertices[2].X);
            int ymin = Math.Min(Math.Min(vertices[0].Y, vertices[1].Y), vertices[2].Y);
            int xmax = Math.Max(Math.Max(vertices[0].X, vertices[1].X), vertices[2].X);
            int ymax = Math.Max(Math.Max(vertices[0].Y, vertices[1].Y), vertices[2].Y);

            var av = new Vector2(vertices[0].X, vertices[0].Y);
            var bv = new Vector2(vertices[1].X, vertices[1].Y);
            var cv = new Vector2(vertices[2].X, vertices[2].Y);

            var p = new Vector2();

            // Iterate rows inside bounding box
            for (p.Y = ymin; p.Y <= ymax; p.Y++)
            {
                // Iterate columns inside bounding box
                for (p.X = xmin; p.X <= xmax; p.X++)
                {
                    // Edges point ccw.
                    var edgeA = bv - av;
                    var edgeB = cv - bv;
                    var edgeC = av - cv;

                    // Checks for each edge if the pixel is fully inside the positive half-space or satisfies to top-left rule.
                    // 
                    // See https://docs.microsoft.com/en-us/windows/win32/direct3d11/d3d10-graphics-programming-guide-rasterizer-stage-rules#triangle-rasterization-rules-without-multisampling
                    //
                    float e1 = 0, e2 = 0, e3 = 0; // compute inside if-condition to skip remaining edges if one test fails
                    var inside = ((e1 = SrMathUtils.Perp(edgeA, p - av)) > 0 || e1 == 0 && edgeA.Y <= 0) &&
                                 ((e2 = SrMathUtils.Perp(edgeB, p - bv)) > 0 || e2 == 0 && edgeB.Y <= 0) &&
                                 ((e3 = SrMathUtils.Perp(edgeC, p - cv)) > 0 || e3 == 0 && edgeC.Y <= 0);
                    if (inside)
                    {
                        var areaInv = 1 / SrMathUtils.Perp(edgeA, edgeB); // actually double the area, but will cancel out anyway later on

                        // Interpolate color value using barycentrc coordinates
                        //
                        var barycentricA = e1 * areaInv;
                        var barycentricB = e2 * areaInv;
                        var barycentricC = e3 * areaInv;

                        int interpolated = (int)(255 * barycentricA) << 16 |
                                           (int)(255 * barycentricB) << 8 |
                                           (int)(255 * barycentricC) << 0;

                        surface.DrawPixel(interpolated, (int)p.X, (int)p.Y);
                    }
                }
            }
        }

        /// <summary>
        /// Orders the given points in counter-clockwise order.
        /// </summary>
        /// <param name="a">A reference to point a.</param>
        /// <param name="b">A reference to point b.</param>
        /// <param name="c">A reference to point c.</param>
        private static void OrderCounterclockWise(ref Int32Point a, ref Int32Point b, ref Int32Point c)
        {
            // Order points by x descending e.g. a will be the right most point.
            if (a.X < b.X) Swap(ref a, ref b);
            if (a.X < c.X) Swap(ref b, ref c);
            if (b.X < c.X) Swap(ref b, ref c);

            // Checks if sin b-a-c is positiv or negativ using the perp product of ac and ab.
            // Might have been simpler by comparing slopes, but seems that divison is much more expensive than multiplication.
            if ((b.X - a.X) * (a.Y - c.Y) + (b.Y - a.Y) * (c.X - a.X) > 0) Swap(ref b, ref c);
        }

        /// <summary>
        /// Swaps the two given points.
        /// </summary>
        /// <param name="a">Point a.</param>
        /// <param name="b">Point b.</param>
        private static void Swap(ref Int32Point a, ref Int32Point b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }
    }
}
