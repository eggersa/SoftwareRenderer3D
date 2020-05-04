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

            var p1 = new Point(0.4, 0.5);
            var p2 = new Point(0.5, 0.3);
            var p3 = new Point(0.6, 0.7);

            SetColor(255, 255, 255);
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
            DrawTriangleCore(pn1, pn2, pn3);

            // For visual debugging
            SetColor(255, 0, 0);
            DrawPixel(pn1.X, pn1.Y);
            DrawPixel(pn2.X, pn2.Y);
            DrawPixel(pn3.X, pn3.Y);
        }

        private void DrawTriangleCore(Int32Point p1, Int32Point p2, Int32Point p3)
        {
            int currentLine;
            bool flatTop = p1.Y == p3.Y && p2.Y > p1.Y,
                 flatBottom = p1.Y == p3.Y && p2.Y < p1.Y;
            BresenhamEnumerator leftSide, rightSide;

            if (flatTop)
            {
                currentLine = p1.Y;
                leftSide = new BresenhamEnumerator(p1, p2);
                rightSide = new BresenhamEnumerator(p3, p2);
            }
            else
            {
                currentLine = p2.Y;
                leftSide = new BresenhamEnumerator(p2, p1);
                rightSide = new BresenhamEnumerator(p2, p3);
            }

            bool end = false;
            while (true)
            {
                // Draw our scan line
                FillRect(new Int32Rect(leftSide.Current.X, currentLine, rightSide.Current.X - leftSide.Current.X + 1, 1));

                // Advance to next scan line
                while ((end |= !leftSide.MoveNext()) && leftSide.Current.Y == currentLine) ;
                while ((end |= !rightSide.MoveNext()) && rightSide.Current.Y == currentLine) ;

                // Check if we reached the end of either the left or right side
                if (end)
                {
                    break;
                }
                
                currentLine++;
            }

            if (!flatTop && !flatBottom)
            {
                // Draw bottom triangle
                DrawTriangleCore(p1, p3, new Int32Point(rightSide.Current.X, currentLine - 1));
            }
            else if (flatBottom)
            {
                // Draw bottom line
                FillRect(new Int32Rect(leftSide.Current.X, currentLine, rightSide.Current.X - leftSide.Current.X + 1, 1));
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
