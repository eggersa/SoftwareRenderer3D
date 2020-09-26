using Sr3D.Core;
using Sr3D.Utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Sr3D.Graphics
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

            var p1 = new Point(0.2, 0.2);
            var p2 = new Point(0.5, 0.6);
            var p3 = new Point(0.1, 0.4);

            DrawTriangle(p1, p2, p3);
        }

        private void DrawVector(Point p, float length, float angle)
        {
            float dy = (float)(-Math.Sin(angle) * length);
            float dx = (float)(Math.Cos(angle) * length);
            DrawLine(p, new Point(p.X + dx, p.Y + dy));
        }

        private void DrawTriangle(Point p1, Point p2, Point p3)
        {
            // Map points to screen space
            //
            var pn1 = new Int32Point((int)(p1.X * PixelWidth), (int)(p1.Y * PixelHeight));
            var pn2 = new Int32Point((int)(p2.X * PixelWidth), (int)(p2.Y * PixelHeight));
            var pn3 = new Int32Point((int)(p3.X * PixelWidth), (int)(p3.Y * PixelHeight));

            SortPoints(ref pn1, ref pn2, ref pn3);

            // For visual debugging
            SetColor(255, 0, 0);
            DrawPixel(pn1.X, pn1.Y);
            SetColor(0, 255, 0);
            DrawPixel(pn2.X, pn2.Y);
            SetColor(0, 0, 255);
            DrawPixel(pn3.X, pn3.Y);

            SetColor(255, 255, 255);

            var ml = pn3.Y < pn1.Y;
            var mr = pn3.Y > pn1.Y;
            if (!ml)
            {
                DrawTriangleCore(pn1, pn2, pn3, ml, mr);
            }
        }

        private void DrawTriangleCore(Int32Point p1, Int32Point p2, Int32Point p3, bool mr, bool ml)
        {
            int currentLine, bottom;
            bool flatTop = (mr && p1.Y == p2.Y) || (ml && p2.Y == p3.Y) || (p1.Y == p3.Y),
                 flatBottom = p1.Y == p3.Y && p2.Y < p1.Y;
            BresenhamEnumerator leftSide, rightSide;

            if (flatTop && mr)
            {
                currentLine = p1.Y;
                leftSide = new BresenhamEnumerator(p1, p3);
                rightSide = new BresenhamEnumerator(p2, p3);
                bottom = p3.Y;
            }
            else if (flatTop && ml)
            {
                return;
            }
            else
            {
                currentLine = p2.Y;
                bottom = Math.Min(p1.Y, p3.Y);
                leftSide = new BresenhamEnumerator(p2, p1);
                rightSide = new BresenhamEnumerator(p2, p3);
            }

            int leftX = leftSide.Current.X,
                rightX = rightSide.Current.X;

            while (currentLine < bottom)
            {
                // Draw current scan line
                FillRect(new Int32Rect(leftX, currentLine, rightX - leftX + 1, 1));

                // Advance to next scan line
                //
                leftSide.MoveNextY();
                rightSide.MoveNextY();
                currentLine++;

                leftX = leftSide.Current.X;
                rightX = rightSide.Current.X;
            }

            if (flatTop || flatBottom)
            {
                // Draw last line
                FillRect(new Int32Rect(leftX, currentLine, rightX - leftX + 1, 1));
            }
            else if (mr)
            {
                // Draw bottom triangle
                DrawTriangleCore(p1, new Int32Point(rightX, currentLine), p3, mr, ml);
            }
            else if (ml)
            {
                // Draw bottom triangle
                // DrawTriangleCore(new Int32Point(leftX, currentLine), p3, mr, ml);
            }
        }

        /// <summary>
        /// Orders points by x and then by y. Uses a fast 3-input sorting network.
        /// </summary>
        /// <param name="a">Point a.</param>
        /// <param name="b">Point b.</param>
        /// <param name="c">Point c.</param>
        static void SortPoints(ref Int32Point a, ref Int32Point b, ref Int32Point c)
        {
            if (a.X > b.X || (a.X == b.X && a.Y > b.Y)) SwapPoints(ref a, ref b);
            if (b.X > c.X || (b.X == c.X && b.Y > c.Y)) SwapPoints(ref b, ref c);
            if (a.X > b.X || (a.X == b.X && a.Y > b.Y)) SwapPoints(ref a, ref b);
        }

        /// <summary>
        /// Swaps two points.
        /// </summary>
        /// <param name="a">Point a.</param>
        /// <param name="b">Point b.</param>
        static void SwapPoints(ref Int32Point a, ref Int32Point b)
        {
            Int32Point tmp = a;
            a = b;
            b = tmp;
        }
    }
}
