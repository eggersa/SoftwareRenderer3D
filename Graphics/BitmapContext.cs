using Sr3D.Core;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSharpRenderer.Graphics
{
    public class BitmapContext : IDrawingContext
    {
        private readonly int BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;
        private readonly WriteableBitmap bitmap;

        public BitmapContext(WriteableBitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void DrawLine(Color color, Int32Point start, Int32Point end)
        {
            // Implementation of the Bresenham line algorithm derived from the ideas given in
            // https://de.wikipedia.org/wiki/Bresenham-Algorithmus
            // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm

            int x1 = start.X;
            int y1 = start.Y;
            int x2 = end.X;
            int y2 = end.Y;

            int dy = y2 - y1;
            int dx = x2 - x1;
            int xinc = Math.Sign(dx);
            int yinc = Math.Sign(dy);
            dx *= xinc; // make dx always positive
            dy *= yinc; // make dy always positive

            if (dy == 0) // horizontal line
            {
                FillRect(color, new Int32Rect((x2 < x1 ? x2 : x1), y1, Math.Abs(x2 - x1), 1));
            }
            else if (dx == 0) // vertical line
            {
                FillRect(color, new Int32Rect(x1, (y2 < y1 ? y2 : y1), 1, Math.Abs(y2 - y1)));
            }

            else if (dy <= dx) // between pi/4 and -pi/4 (including)
            {
                int error = -dx / 2;
                for (; x1 != x2; x1 += xinc)
                {
                    DrawPixel(color, x1, y1);
                    error += dy;
                    if (error >= 0)
                    {
                        y1 += yinc;
                        error -= dx;
                    }
                }
            }
            else
            {
                int error = -dy / 2; // between pi/2 and pi/4 or between -pi/4 and -pi/2 (excluding)
                for (; y1 != y2; y1 += yinc)
                {
                    DrawPixel(color, x1, y1);
                    error += dx;
                    if (error >= 0)
                    {
                        x1 += xinc;
                        error -= dy;
                    }
                }
            }
        }

        public void DrawPixel(Color color, int x, int y)
        {
            DrawPixel(ColorToInt(color), x, y);
        }

        public void DrawPixel(int color, int x, int y)
        {
            unsafe
            {
                IntPtr pBackBuffer = bitmap.BackBuffer + y * bitmap.BackBufferStride + x * BytesPerPixel;
                *(int*)pBackBuffer = color;
            }
        }

        public void FillRect(Color color, Int32Rect rect)
        {
            unsafe
            {
                IntPtr pBackBuffer = IntPtr.Zero;
                for (int y = rect.Y; y < rect.Height + rect.Y; y++)
                {
                    pBackBuffer = bitmap.BackBuffer + y * bitmap.BackBufferStride;
                    for (int x = rect.X; x < rect.Width + rect.X; x++)
                    {
                        if (x == rect.X)
                        {
                            // left offset
                            pBackBuffer += x * BytesPerPixel;
                        }

                        *((int*)pBackBuffer) = ColorToInt(color);
                        pBackBuffer += BytesPerPixel;
                    }
                }
            }
        }

        public void ClearScreen()
        {
            NativeWin32.RtlZeroMemory(bitmap.BackBuffer, bitmap.PixelWidth * bitmap.PixelHeight * BytesPerPixel);
        }

        private static int ColorToInt(Color color)
        {
            int c = color.R << 16;
            c |= color.G << 8;
            c |= color.B << 0;

            return c;
        }
    }
}
