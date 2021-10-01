using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public struct IntPoint
    {
        public static IntPoint Zero => new IntPoint(0, 0);
        public static IntPoint One => new IntPoint(1, 1);

        public static IntPoint Up => new IntPoint(0, -1);
        public static IntPoint Down => new IntPoint(0, 1);
        public static IntPoint Left => new IntPoint(-1, 0);
        public static IntPoint Right => new IntPoint(1, 0);

        public static IntPoint MinValue => new IntPoint(int.MinValue, int.MinValue);
        public static IntPoint MaxValue => new IntPoint(int.MaxValue, int.MaxValue);


        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public IntPoint Absolute => new IntPoint(Math.Abs(X), Math.Abs(Y));
        public IntPoint ToOnes()
        {
            var x = X == 0 ? 0 : X > 0 ? 1 : -1;
            var y = Y == 0 ? 0 : Y > 0 ? 1 : -1;

            return new IntPoint(x, y);
        }

        public static IntPoint operator +(IntPoint p) => p;
        public static IntPoint operator -(IntPoint p) => new IntPoint(-p.X,-p.Y);

        public static IntPoint operator +(IntPoint a, IntPoint b) => new IntPoint(a.X+b.X,a.Y+b.Y);
        public static IntPoint operator -(IntPoint a, IntPoint b) => new IntPoint(a.X - b.X, a.Y - b.Y);
        public static IntPoint operator *(IntPoint a, IntPoint b) => new IntPoint(a.X * b.X, a.Y * b.Y);
        public static IntPoint operator /(IntPoint a, IntPoint b) => new IntPoint(a.X / b.X, a.Y / b.Y);

        public static FloatPoint operator +(IntPoint a, FloatPoint b) => new FloatPoint(a.X + b.X, a.Y + b.Y);
        public static FloatPoint operator -(IntPoint a, FloatPoint b) => new FloatPoint(a.X - b.X, a.Y - b.Y);
        public static FloatPoint operator *(IntPoint a, FloatPoint b) => new FloatPoint(a.X * b.X, a.Y * b.Y);
        public static FloatPoint operator /(IntPoint a, FloatPoint b) => new FloatPoint(a.X / b.X, a.Y / b.Y);

        public static IntPoint operator *(IntPoint p, int i) => new IntPoint(p.X * i, p.Y * i);
        public static IntPoint operator /(IntPoint p, int i) => new IntPoint(p.X / i, p.Y / i);
        public static FloatPoint operator *(IntPoint a, float b) => new FloatPoint(a.X * b, a.Y * b);
        public static FloatPoint operator /(IntPoint a, float b) => new FloatPoint(a.X / b, a.Y / b);

        public FloatPoint ToFloat() => new FloatPoint(X, Y);

        public static bool operator ==(IntPoint a, IntPoint b)=>a.Equals(b);
        public static bool operator !=(IntPoint a, IntPoint b)=>!a.Equals(b);

        public int Max => Math.Max(X, Y);
        public int Min => Math.Min(X, Y);


        public int DistanceFrom(IntPoint other)
        {
            var distancePoint = (this - other).Absolute;
            var output = distancePoint.X + distancePoint.Y;
            return output;
        }

        public List<IntPoint> GetAdjacentPoints()
        {
            return new List<IntPoint>() {
                new IntPoint(X+1, Y),
                new IntPoint(X-1, Y),
                new IntPoint(X, Y+1),
                new IntPoint(X, Y-1),
                new IntPoint(X+1, Y+1),
                new IntPoint(X-1, Y-1),
                new IntPoint(X+1, Y-1),
                new IntPoint(X-1, Y+1)
            };
        }



        public override bool Equals(object obj) => (obj is IntPoint) && Equals((IntPoint)obj);
        public bool Equals(IntPoint other) => (X == other.X) && (Y == other.Y);
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }

        }
        public override string ToString() => X + " " + Y;


        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
        public bool InBoundsExclusive(int x, int y, int x2, int y2) => InBoundsInclusive(x + 1, y + 1, x2 - 1, y2 - 1);
        public bool InBoundsInclusive(int x, int y, int x2, int y2) => (X >= x) & (X <= x2) & (Y >= y) & (Y <= y2);
    }
}
