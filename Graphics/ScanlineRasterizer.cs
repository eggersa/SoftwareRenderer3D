using Sr3D.Core;
using Sr3D.Utils;
using System;
using System.Windows;
using System.Windows.Media;

namespace SoftwareRenderer3D.Graphics
{
    public class ScanlineRasterizer : ITriangleRasterizer
    {
        private readonly IDrawingContext context;

        public ScanlineRasterizer(IDrawingContext context)
        {
            this.context = context;
        }

        public void FillTriangle(Color c, Int32Point[] vertices, Int32Point[] uv)
        {
            var p1 = vertices[0];
            var p2 = vertices[1];
            var p3 = vertices[2];

            SortPoints(ref p1, ref p2, ref p3);

            var ml = p3.Y < p1.Y;
            var mr = p3.Y > p1.Y;
            if (!ml)
            {
                DrawTriangleCore(c, p1, p2, p3, ml, mr);
            }

        }

        private void DrawTriangleCore(Color c, Int32Point p1, Int32Point p2, Int32Point p3, bool mr, bool ml)
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
                context.FillRect(c, new Int32Rect(leftX, currentLine, rightX - leftX + 1, 1));

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
                context.FillRect(c, new Int32Rect(leftX, currentLine, rightX - leftX + 1, 1));
            }
            else if (mr)
            {
                // Draw bottom triangle
                DrawTriangleCore(c, p1, new Int32Point(rightX, currentLine), p3, mr, ml);
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
        private static void SortPoints(ref Int32Point a, ref Int32Point b, ref Int32Point c)
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
        private static void SwapPoints(ref Int32Point a, ref Int32Point b)
        {
            Int32Point tmp = a;
            a = b;
            b = tmp;
        }
    }
}
