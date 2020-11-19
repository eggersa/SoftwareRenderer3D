using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace SoftwareRenderer3D.SrMath
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float value) : this(value, value) { }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static unsafe float Dot(Vector2 a, Vector2 b)
        {
            float[] vecA = new float[] { a.X, a.Y };

            //if (Avx2.IsSupported)
            //{
            //    fixed (float* pA = vecA)
            //    {
            //        Sse41.DotProduct
            //        Avx2.DotProduct()
            //    }
            //}
            //else
            //{
            //}

            return a.X * b.X + a.Y * b.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 a, float b) => new Vector2(a.X * b, a.Y * b);
    }
}
