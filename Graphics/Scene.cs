using SoftwareRenderer3D.Data;
using Sr3D.SrMath;

namespace Sr3D.Graphics
{
    public class Scene
    {
        public IndexedFaceSet Model { get; set; }

        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
    }
}
