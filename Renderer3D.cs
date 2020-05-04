using SoftwareRenderer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SoftwareRenderer3D
{
    public class Renderer3D : BitmapRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer3D"/> class.
        /// </summary>
        /// <param name="renderTarget"></param>
        public Renderer3D(Image renderTarget) : base(renderTarget)
        {
        }

        protected override void RenderCore()
        {
            ClearBitmap();
            SetColor(255, 255, 255);

            Point center = new Point(0.5, 0.5);
            DrawLine(new Point(center.X - 0.2, center.Y), new Point(center.X + 0.2, center.Y));
            DrawLine(new Point(center.X, center.Y - 0.2), new Point(center.X, center.Y + 0.2));

            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(150));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(120));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(60));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(30));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(-30));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(-60));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(-120));
            DrawVector(center, 0.2f, MathUtils.DegreeToRadian(-150));
        }

        private void DrawVector(Point p, float length, float angle)
        {
            float dy = (float)(-Math.Sin(angle) * length);
            float dx = (float)(Math.Cos(angle) * length);
            DrawLine(p, new Point(p.X + dx, p.Y + dy));
        }
    }
}
