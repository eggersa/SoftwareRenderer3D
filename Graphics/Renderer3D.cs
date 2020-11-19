using SoftwareRenderer3D.Graphics;
using Sr3D.Core;
using Sr3D.SrMath;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sr3D.Graphics
{
    public enum RenderMode { Solid, Wireframe }

    public class Renderer3D : BitmapRenderer
    {
        private readonly Scene scene;

        public RenderMode Mode { get; set; }

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

            if(scene.Model == null)
            {
                return;
            }

            ITriangleRasterizer rasterizer = new EdgeRasterizer(context);

            var model = scene.Model;
            var transform = scene.Transform ?? Matrix4x4.Identity;
            var pubes = new PubeScreenTransformer(ScreenWidth, ScreenHeight);
            var origin = new Vector3();

            for (int i = 0; i < model.Indices.Count; i += 3)
            {
                // Model transformation
                //
                var a_ = transform * new Vector4(model.Vertices[model.Indices[i]], 1);
                var b_ = transform * new Vector4(model.Vertices[model.Indices[i + 1]], 1);
                var c_ = transform * new Vector4(model.Vertices[model.Indices[i + 2]], 1);

                var a = new Vector3(a_.X, a_.Y, a_.Z);
                var b = new Vector3(b_.X, b_.Y, b_.Z);
                var c = new Vector3(c_.X, c_.Y, c_.Z);

                // Backface culling
                //
                var r = a - origin;
                var n = Vector3.Cross(c - a, b - a);
                if (Vector3.Dot(r, n) < 0)
                {
                    continue;
                }

                // Perspective projection
                //
                var p0 = pubes.Transform(a);
                var p1 = pubes.Transform(b);
                var p2 = pubes.Transform(c);

                var uv0 = new Int32Point();
                var uv1 = new Int32Point();
                var uv2 = new Int32Point();

                // Rasterization
                //
                if (Mode == RenderMode.Wireframe)
                {
                    context.DrawLine(Colors.White, p0, p1);
                    context.DrawLine(Colors.White, p1, p2);
                    context.DrawLine(Colors.White, p2, p0);
                }
                else
                {
                    var color = Colors.White;
                    if (i == 0)
                    {
                        color = Colors.Red;
                    }
                    else if (i == 3)
                    {
                        color = Colors.Blue;
                    }
                    else if (i == 6)
                    {
                        color = Colors.Yellow;
                    }
                    else if (i == 9)
                    {
                        color = Colors.Green;
                    }

                    rasterizer.FillTriangle(color, 
                        new Int32Point[] { p0, p1, p2 }, 
                        new Int32Point[] { uv0, uv1, uv2 });
                }
            }
        }
    }
}
