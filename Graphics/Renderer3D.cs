using SoftwareRenderer3D.Graphics;
using Sr3D.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        protected override void RenderCore(IDrawingContext context)
        {
            context.ClearScreen();

            var rasterizer = new EdgeRasterizer(context);

            Point[] data = new Point[] 
            { 
                new Point(0.1, 0.4),
                new Point(0.2, 0.2),
                new Point(0.5, 0.6),
            };

            rasterizer.FillTriangle(Colors.White,
                data.Select(p => MathUtils.DeviceToScreenCoordinates(p, PixelWidth, PixelHeight)).ToArray());
        }
    }
}
