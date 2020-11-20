using CSharpRenderer.Graphics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sr3D.Graphics
{
    public abstract class BitmapRenderer
    {
        private readonly PixelFormat PixelFormat = PixelFormats.Bgr32;

        private readonly Image RenderTarget;

        private WriteableBitmap bitmap;

        public int ScreenWidth
        {
            get => bitmap.PixelWidth;
        }

        public int ScreenHeight
        {
            get => bitmap.PixelHeight;
        }

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
                var context = new BitmapContext(bitmap);
                RenderCore(context);
                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            }
            finally
            {
                bitmap.Unlock();
            }
        }

        protected abstract void RenderCore(IDrawingContext context);
    }
}
