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
        private static Dictionary<CardinalDirection, IntPoint> _XYOffsetDict;

        public static CardinalDirection ToCardinal(Orientation orientation, RelativeDirection relativeDirection) => (CardinalDirection)_underflowMod((int)relativeDirection + (int)orientation, 8);
        public static Orientation Rotate(this Orientation orientation, int rotation) => (Orientation)_underflowMod((int)orientation + rotation, 8);
        public static CardinalDirection Reverse(this CardinalDirection direction) => _reverseDict[direction];
        public static IntPoint XYOffset(this CardinalDirection direction) => _XYOffsetDict[direction];
        public static (CardinalDirection left, CardinalDirection right) Parallel(this CardinalDirection dir) => ((CardinalDirection)_underflowMod((int)dir - 2, 8), (CardinalDirection)_underflowMod((int)dir + 2, 8));
        public static bool IsDiagonal(this CardinalDirection dir) => !((int)dir % 2 == 0);
        public static IntPoint GetCoords(IntPoint coord, CardinalDirection direction) => coord + _XYOffsetDict[direction];


        static int _underflowMod(int x, int m)
        {
            if (m == 0) { return x; }
            int res = x % m;

            return res >= 0 ? res : res + m;
        }

        public static List<(CardinalDirection, IntPoint)> GetAdjacentCoords(IntPoint coord)
        {
            return new List<(CardinalDirection, IntPoint)>()
            {
                (CardinalDirection.North,new IntPoint(coord.X+0,coord.Y-1)),
                (CardinalDirection.South,new IntPoint(coord.X+0,coord.Y+1)),
                (CardinalDirection.West,new IntPoint(coord.X-1,coord.Y+0)),
                (CardinalDirection.East,new IntPoint(coord.X+1,coord.Y+0)),
                (CardinalDirection.NorthEast,new IntPoint(coord.X+1,coord.Y-1)),
                (CardinalDirection.SouthEast,new IntPoint(coord.X+1,coord.Y+1)),
                (CardinalDirection.NorthWest,new IntPoint(coord.X-1,coord.Y-1)),
                (CardinalDirection.SouthWest,new IntPoint(coord.X-1,coord.Y+1))
            };
        }

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

            _XYOffsetDict = new Dictionary<CardinalDirection, IntPoint>
            {
                [CardinalDirection.North] = new IntPoint(0, -1),
                [CardinalDirection.South] = new IntPoint(0, 1),
                [CardinalDirection.West] = new IntPoint(-1, 0),
                [CardinalDirection.East] = new IntPoint(1, 0),
                [CardinalDirection.NorthEast] = new IntPoint(1, -1),
                [CardinalDirection.SouthEast] = new IntPoint(1, 1),
                [CardinalDirection.NorthWest] = new IntPoint(-1, -1),
                [CardinalDirection.SouthWest] = new IntPoint(-1, 1)
            };
        }
    }
}
