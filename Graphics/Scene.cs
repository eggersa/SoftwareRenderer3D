using CSharpRenderer.Data;
using Sr3D.SrMath;

namespace Sr3D.Graphics
{
    public class Scene
    {
        public const float RotInc = 0.1047f;

        public float RotX = 0.0f;

        public float RotY = 0.0f;

        public IndexedFaceSet Model { get; set; }

        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
    }
}
