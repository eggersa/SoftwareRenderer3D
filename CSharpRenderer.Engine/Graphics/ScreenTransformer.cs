using CSharpRenderer.Core;
using Sr3D.Core;
using Sr3D.SrMath;

namespace CSharpRenderer.Graphics
{
    public class ScreenTransformer
    {
        private float xFactor, yFactor;

        public ScreenTransformer(int screenWidth, int screenHeight)
        {
            xFactor = screenWidth / 2;
            yFactor = screenHeight / 2;
        }

        public Int32Point Transform(Vector3 v)
        {
            var zInv = 1 / (v.Z); // z-inverse for perspective transform
            return new Int32Point((int)((v.X * zInv + 1) * xFactor), (int)((-v.Y * zInv + 1) * yFactor));
        }
    }
}
