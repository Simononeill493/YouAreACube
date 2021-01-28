using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DirectionUtils
    {
        public static Dictionary<CardinalDirection,CardinalDirection> Reverse;
        public static Dictionary<CardinalDirection, Tuple<int,int>> XYOffset;

        public static void Init()
        {
            Reverse = new Dictionary<CardinalDirection, CardinalDirection>();
            Reverse[CardinalDirection.North] = CardinalDirection.South;
            Reverse[CardinalDirection.South] = CardinalDirection.North;
            Reverse[CardinalDirection.West] = CardinalDirection.East;
            Reverse[CardinalDirection.East] = CardinalDirection.West;
            Reverse[CardinalDirection.NorthEast] = CardinalDirection.SouthWest;
            Reverse[CardinalDirection.SouthEast] = CardinalDirection.NorthWest;
            Reverse[CardinalDirection.NorthWest] = CardinalDirection.SouthEast;
            Reverse[CardinalDirection.SouthWest] = CardinalDirection.NorthEast;

            XYOffset = new Dictionary<CardinalDirection, Tuple<int, int>>();
            XYOffset[CardinalDirection.North] = new Tuple<int, int>(0, -1);
            XYOffset[CardinalDirection.South] = new Tuple<int, int>(0, 1);
            XYOffset[CardinalDirection.West] = new Tuple<int, int>(-1, 0);
            XYOffset[CardinalDirection.East] = new Tuple<int, int>(1, 0);
            XYOffset[CardinalDirection.NorthEast] = new Tuple<int, int>(1, -1);
            XYOffset[CardinalDirection.SouthEast] = new Tuple<int, int>(1, 1);
            XYOffset[CardinalDirection.NorthWest] = new Tuple<int, int>(-1, -1);
            XYOffset[CardinalDirection.SouthWest] = new Tuple<int, int>(-1, 1);

        }

        public static CardinalDirection ToCardinal(Orientation orientation, MovementDirection movementDirection)
        {
            var res = _underflowMod((int)orientation, (int)movementDirection);
            return (CardinalDirection)res;
        }

        public static Orientation Rotate(Orientation orientation, int rotation)
        {
            var res = _underflowMod((int)orientation+rotation, 8);
            return (Orientation)res;
        }

        public static (CardinalDirection left,CardinalDirection right) Parallel(CardinalDirection dir)
        {
            var left = _underflowMod(((int)dir - 2), 8);
            var right = _underflowMod(((int)dir + 2), 8);

            return ((CardinalDirection)left, (CardinalDirection)right);
        }

        public static bool IsDiagonal(CardinalDirection dir)
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
