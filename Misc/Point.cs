using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x,int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p) => p;
        public static Point operator -(Point p) => new Point(-p.X,-p.Y);
        public static Point operator +(Point a, Point b) => new Point(a.X+b.X,a.Y+b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
            return (obj is Point) && Equals((Point)obj);
        }
        public bool Equals(Point other)
        {
            return ((X == other.X) && (Y == other.Y));
        }
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
        public override string ToString()
        {
            return "(" + X + " " + Y + ")";
        }
    }
}
