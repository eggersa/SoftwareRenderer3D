namespace CSharpRenderer.Core
{
    public struct Int32Point
    {
        public Int32Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Int32Point Empty { get; } = new Int32Point();

        public int X;

        public int Y;

        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public static bool Equals(Int32Point int32Point1, Int32Point int32RPoint2)
        {
            return int32Point1.X == int32RPoint2.X && int32Point1.Y == int32RPoint2.Y;
        }
  
        public bool Equals(Int32Point value)
        {
            return X == value.X && Y == value.Y;
        }

        public override bool Equals(object o)
        {
            // Check for null and compare run-time types.
            if ((o == null) || !o.GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                Int32Point p = (Int32Point)o;
                return (X == p.X) && (Y == p.Y);
            }
        }

        public override int GetHashCode()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netcore-3.1
            return (X << 2) ^ Y;
        }
        
        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public static bool operator ==(Int32Point left, Int32Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Int32Point left, Int32Point right)
        {
            return !(left == right);
        }
    }
}
