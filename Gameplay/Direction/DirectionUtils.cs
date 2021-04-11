using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class DirectionUtils
    {
        public static List<CardinalDirection> Cardinals;

        private static Dictionary<CardinalDirection,CardinalDirection> _reverseDict;
        private static Dictionary<CardinalDirection, Point> _XYOffsetDict;

        public static void Init()
        {
            Cardinals = typeof(CardinalDirection).GetEnumValues().Cast<CardinalDirection>().ToList();

            _reverseDict = new Dictionary<CardinalDirection, CardinalDirection>
            {
                [CardinalDirection.North] = CardinalDirection.South,
                [CardinalDirection.South] = CardinalDirection.North,
                [CardinalDirection.West] = CardinalDirection.East,
                [CardinalDirection.East] = CardinalDirection.West,
                [CardinalDirection.NorthEast] = CardinalDirection.SouthWest,
                [CardinalDirection.SouthEast] = CardinalDirection.NorthWest,
                [CardinalDirection.NorthWest] = CardinalDirection.SouthEast,
                [CardinalDirection.SouthWest] = CardinalDirection.NorthEast
            };

            _XYOffsetDict = new Dictionary<CardinalDirection, Point>
            {
                [CardinalDirection.North] = new Point(0, -1),
                [CardinalDirection.South] = new Point(0, 1),
                [CardinalDirection.West] = new Point(-1, 0),
                [CardinalDirection.East] = new Point(1, 0),
                [CardinalDirection.NorthEast] = new Point(1, -1),
                [CardinalDirection.SouthEast] = new Point(1, 1),
                [CardinalDirection.NorthWest] = new Point(-1, -1),
                [CardinalDirection.SouthWest] = new Point(-1, 1)
            };
        }

        public static CardinalDirection ToCardinal(Orientation orientation, RelativeDirection relativeDirection)
        {
            var res = _underflowMod((int)relativeDirection + (int)orientation,8);
            return (CardinalDirection)res;
        }
        public static Orientation Rotate(this Orientation orientation, int rotation)
        {
            var res = _underflowMod((int)orientation + rotation, 8);
            return (Orientation)res;
        }

        public static CardinalDirection Reverse(this CardinalDirection direction) => _reverseDict[direction];
        public static Point XYOffset(this CardinalDirection direction) => _XYOffsetDict[direction];
        public static (CardinalDirection left,CardinalDirection right) Parallel(this CardinalDirection dir)
        {
            var left = _underflowMod(((int)dir - 2), 8);
            var right = _underflowMod(((int)dir + 2), 8);

            return ((CardinalDirection)left, (CardinalDirection)right);
        }
        public static bool IsDiagonal(this CardinalDirection dir)
        {
            return !((int)dir % 2 == 0);
        }

        static int _underflowMod(int x, int m)
        {
            if (m==0) { return x; }
            int res = x % m;

            return res >= 0 ? res : res + m; 
        }
    
        public static Point GetCoords(Point coord,CardinalDirection direction)
        {
            var offs = _XYOffsetDict[direction];
            return coord + offs;
        }

        public static List<(CardinalDirection,Point)> GetAdjacentCoords(Point coord)
        {
            return new List<(CardinalDirection, Point)>()
            {
                (CardinalDirection.North,new Point(coord.X+0,coord.Y-1)),
                (CardinalDirection.South,new Point(coord.X+0,coord.Y+1)),
                (CardinalDirection.West,new Point(coord.X-1,coord.Y+0)),
                (CardinalDirection.East,new Point(coord.X+1,coord.Y+0)),
                (CardinalDirection.NorthEast,new Point(coord.X+1,coord.Y-1)),
                (CardinalDirection.SouthEast,new Point(coord.X+1,coord.Y+1)),
                (CardinalDirection.NorthWest,new Point(coord.X-1,coord.Y-1)),
                (CardinalDirection.SouthWest,new Point(coord.X-1,coord.Y+1))
            };
        }
    }
}
