using Sr3D.Models;
using Sr3D.SrMath;

namespace Sr3D.Graphics
{
    public class Scene
    {
        public Cube Cube { get; set; }

        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;

        public IndexedLineSet GetLineSet()
        {
            return Cube.GetLines();
        }
    }
}
