using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class DirectionUtils
    {
        private static Dictionary<CardinalDirection,CardinalDirection> _reverseDict;
        private static Dictionary<CardinalDirection, (int,int)> _XYOffsetDict;

        public static void Init()
        {
            _reverseDict = new Dictionary<CardinalDirection, CardinalDirection>();
            _reverseDict[CardinalDirection.North] = CardinalDirection.South;
            _reverseDict[CardinalDirection.South] = CardinalDirection.North;
            _reverseDict[CardinalDirection.West] = CardinalDirection.East;
            _reverseDict[CardinalDirection.East] = CardinalDirection.West;
            _reverseDict[CardinalDirection.NorthEast] = CardinalDirection.SouthWest;
            _reverseDict[CardinalDirection.SouthEast] = CardinalDirection.NorthWest;
            _reverseDict[CardinalDirection.NorthWest] = CardinalDirection.SouthEast;
            _reverseDict[CardinalDirection.SouthWest] = CardinalDirection.NorthEast;

            _XYOffsetDict = new Dictionary<CardinalDirection, (int, int)>();
            _XYOffsetDict[CardinalDirection.North] = (0, -1);
            _XYOffsetDict[CardinalDirection.South] = (0, 1);
            _XYOffsetDict[CardinalDirection.West] = (-1, 0);
            _XYOffsetDict[CardinalDirection.East] = (1, 0);
            _XYOffsetDict[CardinalDirection.NorthEast] = (1, -1);
            _XYOffsetDict[CardinalDirection.SouthEast] = (1, 1);
            _XYOffsetDict[CardinalDirection.NorthWest] = (-1, -1);
            _XYOffsetDict[CardinalDirection.SouthWest] = (-1, 1);
        }

        public static CardinalDirection ToCardinal(Orientation orientation, RelativeDirection relativeDirection)
        {
            var res = _underflowMod((int)orientation, (int)relativeDirection);
            return (CardinalDirection)res;
        }
        public static Orientation Rotate(this Orientation orientation, int rotation)
        {
            var res = _underflowMod((int)orientation + rotation, 8);
            return (Orientation)res;
        }

        public static CardinalDirection Reverse(this CardinalDirection direction) => _reverseDict[direction];
        public static (int x,int y) XYOffset(this CardinalDirection direction) => _XYOffsetDict[direction];
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
    }
}
