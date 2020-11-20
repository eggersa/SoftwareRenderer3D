namespace Sr3D.SrMath
{
    public struct Vector4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4(float value) : this(value, value, value, 1) { }

        public Vector4(float x, float y, float z) : this(x, y, z, 1) { }

        public Vector4(Vector3 v, float w)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString()
        {
            return $"[{X} {Y} {Z} {W}]";
        }
    }
}
