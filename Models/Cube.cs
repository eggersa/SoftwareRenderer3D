using Sr3D.SrMath;

namespace Sr3D.Models
{
    public class Cube
    {
        private readonly float size;
        private readonly IndexedLineSet lines = new IndexedLineSet();

        public Cube(float size)
        {
            this.size = size;
            ComposeModel();
        }

        private void ComposeModel()
        {
            var s = size / 2;

            lines.Vertices.Add(new Vector4(-s, -s, -s, 1));
            lines.Vertices.Add(new Vector4(s, -s, -s, 1));
            lines.Vertices.Add(new Vector4(s, s, -s, 1));
            lines.Vertices.Add(new Vector4(-s, s, -s, 1));
            lines.Vertices.Add(new Vector4(s, -s, s, 1));
            lines.Vertices.Add(new Vector4(s, s, s, 1));
            lines.Vertices.Add(new Vector4(-s, s, s, 1));
            lines.Vertices.Add(new Vector4(-s, -s, s, 1));

            lines.Indices.Add(new int[] { 0, 1 });
            lines.Indices.Add(new int[] { 1, 2 });
            lines.Indices.Add(new int[] { 2, 3 });
            lines.Indices.Add(new int[] { 3, 0 });
            lines.Indices.Add(new int[] { 4, 5 });
            lines.Indices.Add(new int[] { 5, 6 });
            lines.Indices.Add(new int[] { 6, 7 });
            lines.Indices.Add(new int[] { 7, 4 });
            lines.Indices.Add(new int[] { 0, 4 });
            lines.Indices.Add(new int[] { 1, 5 });
            lines.Indices.Add(new int[] { 2, 6 });
            lines.Indices.Add(new int[] { 3, 7 });
        }

        public IndexedLineSet GetLines()
        {
            return lines;
        }
    }
}
