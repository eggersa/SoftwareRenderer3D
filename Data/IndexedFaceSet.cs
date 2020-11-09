using Sr3D.SrMath;
using System.Collections.Generic;

namespace SoftwareRenderer3D.Data
{
    public class IndexedFaceSet
    {
        public List<Vector4> Vertices { get; }

        public List<int> Indices { get; }

        public IndexedFaceSet()
        {
            Vertices = new List<Vector4>();
            Indices = new List<int>();
        }

        public IndexedFaceSet(Vector4[] vertices, int[] indices)
        {
            Vertices = new List<Vector4>(vertices);
            Indices = new List<int>(indices);
        }
    }
}
