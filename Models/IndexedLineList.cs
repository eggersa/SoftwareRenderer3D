using Sr3D.SrMath;
using System.Collections.Generic;

namespace Sr3D.Models
{
    public class IndexedLineSet
    {
        public List<Vector4> Vertices { get; } = new List<Vector4>();

        public List<int[]> Indices { get; } = new List<int[]>();
    }
}
