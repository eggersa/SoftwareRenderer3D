using Sr3D.Core;
using System.Windows;

namespace SoftwareRenderer3D.Graphics
{
    public class PubeScreenTransformer
    {
        private float xFactor, yFactor;

        public PubeScreenTransformer(int screenWidth, int screenHeight)
        {
            xFactor = screenWidth / 2;
            yFactor = screenHeight / 2;
        }

        public Int32Point Transform(Point p)
        {
            return new Int32Point((int)((p.X + 1) * xFactor), (int)((-p.Y + 1) * yFactor));
        }
    }
}
