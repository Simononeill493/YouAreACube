using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public struct FloatPoint
    {
        public static FloatPoint Zero => new FloatPoint(0, 0);
        public static FloatPoint MinValue => new FloatPoint(float.MinValue, float.MinValue);

        public float X;
        public float Y;

        public FloatPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public FloatPoint Absolute => new FloatPoint(Math.Abs(X), Math.Abs(Y));
        public IntPoint ToOnes()
        {
            var x = X == 0 ? 0 : X > 0 ? 1 : -1;
            var y = Y == 0 ? 0 : Y > 0 ? 1 : -1;

            return new IntPoint(x, y);
        }

        public IntPoint Ceiling => new IntPoint((int)Math.Ceiling(X), (int)Math.Ceiling(Y));
        public IntPoint Floor => new IntPoint((int)Math.Floor(X), (int)Math.Floor(Y));
        public IntPoint Round => new IntPoint((int)Math.Round(X), (int)Math.Round(Y));
        public float Max => Math.Max(X, Y);
        public float Min => Math.Min(X, Y);


        public static FloatPoint operator +(FloatPoint p) => p;
        public static FloatPoint operator -(FloatPoint p) => new FloatPoint(-p.X, -p.Y);

        public static FloatPoint operator +(FloatPoint a, FloatPoint b) => new FloatPoint(a.X + b.X, a.Y + b.Y);
        public static FloatPoint operator -(FloatPoint a, FloatPoint b) => new FloatPoint(a.X - b.X, a.Y - b.Y);
        public static FloatPoint operator *(FloatPoint a, FloatPoint b) => new FloatPoint(a.X * b.X, a.Y * b.Y);
        public static FloatPoint operator /(FloatPoint a, FloatPoint b) => new FloatPoint(a.X / b.X, a.Y / b.Y);

        public static FloatPoint operator +(FloatPoint a, IntPoint b) => new FloatPoint(a.X + b.X, a.Y + b.Y);
        public static FloatPoint operator -(FloatPoint a, IntPoint b) => new FloatPoint(a.X - b.X, a.Y - b.Y);
        public static FloatPoint operator *(FloatPoint a, IntPoint b) => new FloatPoint(a.X * b.X, a.Y * b.Y);
        public static FloatPoint operator /(FloatPoint a, IntPoint b) => new FloatPoint(a.X / b.X, a.Y / b.Y);

        public static FloatPoint operator *(FloatPoint p, int i) => new FloatPoint(p.X * i, p.Y * i);
        public static FloatPoint operator /(FloatPoint p, int i) => new FloatPoint(p.X / i, p.Y / i);
        public static FloatPoint operator *(FloatPoint p, float i) => new FloatPoint(p.X * i, p.Y * i);
        public static FloatPoint operator /(FloatPoint p, float i) => new FloatPoint(p.X / i, p.Y / i);

        public static bool operator ==(FloatPoint a, FloatPoint b)=>a.Equals(b);
        public static bool operator !=(FloatPoint a, FloatPoint b)=>!a.Equals(b);
        public override bool Equals(object obj)=>(obj is FloatPoint) && Equals((FloatPoint)obj);
        public bool Equals(FloatPoint other)=> ((X == other.X) && (Y == other.Y));
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
        public override string ToString()=> X + " " + Y;
        

        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }
        public bool InBoundsExclusive(int x, int y, int x2, int y2) => InBoundsInclusive(x + 1, y + 1, x2 - 1, y2 - 1);
        public bool InBoundsInclusive(int x, int y, int x2, int y2) => (X >= x) & (X <= x2) & (Y >= y) & (Y <= y2);
    }
}
