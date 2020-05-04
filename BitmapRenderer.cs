using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoftwareRenderer3D
{
    public abstract class BitmapRenderer
    {
        private readonly PixelFormat PixelFormat = PixelFormats.Bgr32;
        private readonly int BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;
        private readonly Image RenderTarget;

        private int color = 0;
        private WriteableBitmap bitmap;

        public BitmapRenderer(Image renderTarget)
        {
            RenderTarget = renderTarget;
        }

        public void Resize(int width, int height)
        {
            bitmap = new WriteableBitmap((int)width, (int)height, 96, 96, PixelFormat, null);
            RenderTarget.Source = bitmap;
        }

        public void Render()
        {
            try
            {
                bitmap.Lock();
                RenderCore();
                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            }
            finally
            {
                bitmap.Unlock();
            }
        }

        protected abstract void RenderCore();

        protected void ClearBitmap()
        {
            NativeWin32.RtlZeroMemory(bitmap.BackBuffer, bitmap.PixelWidth * bitmap.PixelHeight * BytesPerPixel);
        }

        protected void SetColor(byte r, byte g, byte b)
        {
            color = r << 16;
            color |= g << 8;
            color |= b << 0;
        }

        protected void DrawLine(Point start, Point end)
        {
            // Implementation of the Bresenham line algorithm derived from the ideas given in
            // https://de.wikipedia.org/wiki/Bresenham-Algorithmus
            // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm

            // Get absolute coordinates
            //
            int x1 = (int)(start.X * bitmap.PixelWidth);
            int y1 = (int)(start.Y * bitmap.PixelHeight);
            int x2 = (int)(end.X * bitmap.PixelWidth);
            int y2 = (int)(end.Y * bitmap.PixelHeight);

            int dy = y2 - y1;
            int dx = x2 - x1;
            int xinc = Math.Sign(dx);
            int yinc = Math.Sign(dy);
            dx *= xinc; // make dx always positive
            dy *= yinc; // make dy always positive

            if (dy == 0) // horizontal line
            {
                FillRect(new Int32Rect((x2 < x1 ? x2 : x1), y1, Math.Abs(x2 - x1), 1));
            }
            else if (dx == 0) // vertical line
            {
                FillRect(new Int32Rect(x1, (y2 < y1 ? y2 : y1), 1, Math.Abs(y2 - y1)));
            }

            else if (dy <= dx) // between pi/4 and -pi/4 (including)
            {
                int error = -dx / 2;
                for (; x1 != x2; x1 += xinc)
                {
                    DrawPixel(x1, y1);
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
                    DrawPixel(x1, y1);
                    error += dx;
                    if (error >= 0)
                    {
                        x1 += xinc;
                        error -= dy;
                    }
                }
            }
        }

        protected void DrawPixel(int x, int y)
        {
            unsafe
            {
                IntPtr pBackBuffer = bitmap.BackBuffer + y * bitmap.BackBufferStride + x * BytesPerPixel;
                *((int*)pBackBuffer) = color;
            }
        }

        protected void FillRect(Int32Rect rect)
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
                            pBackBuffer += x * BytesPerPixel;
                        }

                        pBackBuffer += BytesPerPixel;
                        *((int*)pBackBuffer) = color;
                    }
                }
            }
        }
    }
}
