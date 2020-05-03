using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoftwareRenderer
{
    public partial class MainWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern void RtlZeroMemory(IntPtr dst, int length);

        private readonly PixelFormat PixelFormat = PixelFormats.Bgr32;
        private readonly int BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;

        private WriteableBitmap writeableBitmap = null;
        private int color = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitWriteableBitmap(new Size(Width, Height));

            Loaded += MainWindow_Loaded;
            display.MouseWheel += Display_MouseWheel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawInternal();
        }

        private void Display_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Matrix m = display.RenderTransform.Value;

            if (e.Delta > 0)
            {
                m.ScaleAt(1.5, 1.5, e.GetPosition(display).X, e.GetPosition(display).Y);
            }
            else
            {
                m.ScaleAt(1.0 / 1.5, 1.0 / 1.5, e.GetPosition(display).X, e.GetPosition(display).Y);
            }

            display.RenderTransform = new MatrixTransform(m);
        }

        private void InitWriteableBitmap(Size size)
        {
            writeableBitmap = new WriteableBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Bgr32, null);
            display.Source = writeableBitmap;
        }

        private void DrawInternal()
        {
            if (writeableBitmap == null)
            {
                return;
            }

            try
            {
                writeableBitmap.Lock();
                Draw();
                writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
            }
            finally
            {
                writeableBitmap.Unlock();
            }
        }

        private void Draw()
        {
            Clear();

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

        private void SetColor(byte r, byte g, byte b)
        {
            color = r << 16;
            color |= g << 8;
            color |= b << 0;
        }

        private void DrawLine(Point start, Point end)
        {
            // Implementation of the Bresenham Linedrawing Algortihm derived from the ideas given in
            // https://de.wikipedia.org/wiki/Bresenham-Algorithmus
            // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm

            // Get absolute coordinates
            //
            int x1 = (int)(start.X * writeableBitmap.PixelWidth);
            int y1 = (int)(start.Y * writeableBitmap.PixelHeight);
            int x2 = (int)(end.X * writeableBitmap.PixelWidth);
            int y2 = (int)(end.Y * writeableBitmap.PixelHeight);

            int dy = y2 - y1;
            int dx = x2 - x1;
            int xinc = Math.Sign(dx);
            int yinc = Math.Sign(dy);
            dx *= xinc; // make dx always positive
            dy *= yinc; // make dy always positive

            if (dy == 0) // horizontal line
            {
                DrawPixelsUnsafe(new Int32Rect(x1, y1, x2 - x1, 1));
            }
            else if (dx == 0) // vertical line
            {
                DrawPixelsUnsafe(new Int32Rect(x1, y1, 1, y2 - y1));
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
        
        private void DrawPixel(int x, int y)
        {
            DrawPixelsUnsafe(new Int32Rect(x, y, 1, 1));
        }

        private void DrawPixelsUnsafe(Int32Rect rect)
        {
            unsafe
            {
                IntPtr pBackBuffer = IntPtr.Zero;
                for (int y = rect.Y; y < rect.Height + rect.Y; y++)
                {
                    pBackBuffer = writeableBitmap.BackBuffer + y * writeableBitmap.BackBufferStride;
                    for (int x = rect.X; x < rect.Width + rect.X; x++)
                    {
                        if(x == rect.X)
                        {
                            pBackBuffer += x * BytesPerPixel;
                        }

                        pBackBuffer += BytesPerPixel;
                        *((int*)pBackBuffer) = color;
                    }
                }
            }
        }

        private void Clear()
        {
            RtlZeroMemory(writeableBitmap.BackBuffer, writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * BytesPerPixel);
        }

        private void ErasePixel(MouseEventArgs e)
        {
            if (writeableBitmap == null)
            {
                return;
            }

            byte[] ColorData = { 0, 0, 0, 0 }; // B G R
            Int32Rect rect = new Int32Rect((int)(e.GetPosition(display).X), (int)(e.GetPosition(display).Y), 1, 1);
            writeableBitmap.WritePixels(rect, ColorData, 4, 0);
        }

        private void DrawVector(Point p, float length, float angle)
        {
            float dy = (float)(-Math.Sin(angle) * length);
            float dx = (float)(Math.Cos(angle) * length);

            DrawLine(p, new Point(p.X + dx, p.Y + dy));
        }
    }
}
