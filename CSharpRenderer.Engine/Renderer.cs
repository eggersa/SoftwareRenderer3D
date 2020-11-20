using CSharpRenderer.Core;
using Sr3D.SrMath;
using System.Diagnostics.Contracts;

namespace CSharpRenderer.Graphics
{
    public enum RenderMode { Solid, Wireframe }

    public class Renderer
    {
        private readonly ITriangleRasterizer rasterizer;
        private readonly Surface surface;
        private readonly ScreenTransformer screenTransformer;

        public RenderMode Mode { get; set; } = RenderMode.Solid;

        public Renderer(Surface surface)
        {
            this.surface = surface;
            rasterizer = new EdgeRasterizer(surface);
            screenTransformer = new ScreenTransformer(surface.Width, surface.Height);
        }

        public void Render(Scene scene, RenderMode mode = RenderMode.Solid)
        {
            Contract.Requires(scene?.Model != null);

            var model = scene.Model;
            var transform = scene.Transform ?? Matrix4x4.Identity;
            var origin = new Vector3();

            surface.ClearScreen();

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
                var p0 = screenTransformer.Transform(a);
                var p1 = screenTransformer.Transform(b);
                var p2 = screenTransformer.Transform(c);

                var uv0 = new Int32Point();
                var uv1 = new Int32Point();
                var uv2 = new Int32Point();

                // Rasterization
                //
                if (Mode == RenderMode.Wireframe)
                {
                    surface.DrawLine(System.Drawing.Color.White, p0, p1);
                    surface.DrawLine(System.Drawing.Color.White, p1, p2);
                    surface.DrawLine(System.Drawing.Color.White, p2, p0);
                }
                else
                {
                    var color = System.Drawing.Color.White;
                    if (i == 0)
                    {
                        color = System.Drawing.Color.Red;
                    }
                    else if (i == 3)
                    {
                        color = System.Drawing.Color.Blue;
                    }
                    else if (i == 6)
                    {
                        color = System.Drawing.Color.Yellow;
                    }
                    else if (i == 9)
                    {
                        color = System.Drawing.Color.Green;
                    }

                    rasterizer.FillTriangle(color,
                        new Int32Point[] { p0, p1, p2 },
                        new Int32Point[] { uv0, uv1, uv2 });
                }
            }
        }
    }
}
