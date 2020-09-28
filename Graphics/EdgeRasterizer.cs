using Microsoft.VisualBasic;
using Sr3D.Core;
using Sr3D.Utils;
using System;
using System.Numerics;
using System.Windows.Media;

namespace SoftwareRenderer3D.Graphics
{
    public class EdgeRasterizer : ITriangleRasterizer
    {
        private readonly IDrawingContext context;

        public EdgeRasterizer(IDrawingContext context)
        {
            this.context = context;
        }

        public void FillTriangle(Color c, Int32Point[] vertices)
        {
            int xmin = Math.Min(Math.Min(vertices[0].X, vertices[1].X), vertices[2].X);
            int ymin = Math.Min(Math.Min(vertices[0].Y, vertices[1].Y), vertices[2].Y);
            int xmax = Math.Max(Math.Max(vertices[0].X, vertices[1].X), vertices[2].X);
            int ymax = Math.Max(Math.Max(vertices[0].Y, vertices[1].Y), vertices[2].Y);

            var av = new Vector2(vertices[0].X, vertices[0].Y);
            var bv = new Vector2(vertices[1].X, vertices[1].Y);
            var cv = new Vector2(vertices[2].X, vertices[2].Y);

            var p = new Vector2();

            for (p.Y = ymin; p.Y <= ymax; p.Y++)
            {
                for (p.X = xmin; p.X <= xmax; p.X++)
                {
                    bool inside = MathUtils.Perp(bv - av, p - av) >= 0 &&
                                  MathUtils.Perp(cv - bv, p - bv) >= 0 &&
                                  MathUtils.Perp(av - cv, p - cv) >= 0;
                    if (inside)
                    {
                        context.DrawPixel(c, (int)p.X, (int)p.Y);
                    }
                }
            }
        }
    }
}
