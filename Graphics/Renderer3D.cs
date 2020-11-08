using SoftwareRenderer3D.Graphics;
using Sr3D.Core;
using Sr3D.SrMath;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sr3D.Graphics
{
    public class Renderer3D : BitmapRenderer
    {
        private readonly Scene scene;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer3D"/> class.
        /// </summary>
        /// <param name="renderTarget"></param>
        public Renderer3D(Image renderTarget, Scene scene) : base(renderTarget)
        {
            this.scene = scene;
        }

        protected override void RenderCore(IDrawingContext context)
        {
            context.ClearScreen();

            //var rasterizer = new EdgeRasterizer(context);

            //Point[] data = new Point[] 
            //{ 
            //    new Point(0.2, 0.2),
            //    new Point(0.8, 0.4),
            //    new Point(0.5, 0.6),
            //};

            var lines = scene.GetLineSet();
            var transform = scene.Transform;

            var pubeTransform = new PubeScreenTransformer(ScreenWidth, ScreenHeight);

            var points = new List<Int32Point>();
            foreach (var vertex in lines.Vertices)
            {
                Vector4 v = vertex;
                if(transform != null)
                {
                    v = transform * v;
                }

                points.Add(pubeTransform.Transform(v));
            }

            foreach (var index in lines.Indices)
            {
                context.DrawLine(Colors.White, points[index[0]], points[index[1]]);
            }

            //rasterizer.FillTriangle(Colors.White,
            //    data.Select(p => MathUtils.DeviceToScreenCoordinates(p, PixelWidth, PixelHeight)).ToArray());
        }
    }
}
