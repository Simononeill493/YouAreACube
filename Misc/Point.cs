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
        public static Point Zero => new Point(0, 0);

        public int X;
        public int Y;

        public Point(int x,int y)
        {
            X = x;
            Y = y;
        }

        public Point Absolute => new Point(Math.Abs(X), Math.Abs(Y));

        public static Point operator +(Point p) => p;
        public static Point operator -(Point p) => new Point(-p.X,-p.Y);

        public static Point operator +(Point a, Point b) => new Point(a.X+b.X,a.Y+b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

        public static Point operator *(Point a, Point b) => new Point(a.X * b.X, a.Y * b.Y);
        public static Point operator /(Point a, Point b) => new Point(a.X / b.X, a.Y / b.Y);

        public static Point operator *(Point p,int i) => new Point(p.X*i, p.Y*i);
        public static Point operator /(Point p, int i) => new Point(p.X / i, p.Y / i);

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

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public CardinalDirection ApproachDirection(Point other)
        {
            if (other.X > X)
            {
                if (other.Y > Y)
                {
                    return CardinalDirection.SouthEast;
                }
                else if (other.Y < Y)
                {
                    return CardinalDirection.NorthEast;
                }

                return CardinalDirection.East;
            }
            else if (other.X < X)
            {
                if (other.Y > Y)
                {
                    return CardinalDirection.SouthWest;
                }
                else if (other.Y < Y)
                {
                    return CardinalDirection.NorthWest;
                }

                return CardinalDirection.West;
            }
            else if (other.Y > Y)
            {
                return CardinalDirection.South;
            }

            return CardinalDirection.North;
        }
        public CardinalDirection FleeDirection(Point other)
        {
            return ApproachDirection(other).Reverse();
        }

        public bool InBoundsExclusive(int x, int y, int x2, int y2)
        {
            return InBoundsInclusive(x+1,y+1,x2-1,y2-1);
        }
        public bool InBoundsInclusive(int x,int y,int x2,int y2)
        {
            return ((X >= x) & (X <= x2) & (Y >= y) & (Y <= y2));
        }
    }
}
