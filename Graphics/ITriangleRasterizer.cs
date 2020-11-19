using Sr3D.Core;
using System.Windows.Media;

namespace SoftwareRenderer3D.Graphics
{
    public interface ITriangleRasterizer
    {
        /// <summary>
        /// Fills the interior of a triangle specified by 3 vertices.
        /// </summary>
        /// <param name="c">The fill color.</param>
        /// <param name="vertices">An array with 3 screen coordinates in counter-clockwise order that define a triangle.</param>
        void FillTriangle(Color c, Int32Point[] vertices, Int32Point[] uv);
    }
}
