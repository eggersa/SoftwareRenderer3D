using System;

namespace Sr3D.SrMath
{
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float value) : this(value, value, value) { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Norm() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

        public static Vector3 Cross(Vector3 a, Vector3 b) => new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static Vector3 Normalize(Vector3 a)
        {
            var norm = a.Norm();
            return new Vector3(a.X / norm, a.Y / norm, a.Z / norm);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public override string ToString()
        {
            return $"[{X} {Y} {Z}]";
        }
    }
}
