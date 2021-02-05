﻿using System;
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
        private static Dictionary<CardinalDirection, (int,int)> _XYOffsetDict;

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

            _XYOffsetDict = new Dictionary<CardinalDirection, (int, int)>
            {
                [CardinalDirection.North] = (0, -1),
                [CardinalDirection.South] = (0, 1),
                [CardinalDirection.West] = (-1, 0),
                [CardinalDirection.East] = (1, 0),
                [CardinalDirection.NorthEast] = (1, -1),
                [CardinalDirection.SouthEast] = (1, 1),
                [CardinalDirection.NorthWest] = (-1, -1),
                [CardinalDirection.SouthWest] = (-1, 1)
            };
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
    
        public static (int x,int y) GetCoords((int X,int Y) coord,CardinalDirection direction)
        {
            var offs = _XYOffsetDict[direction];
            return (coord.X + offs.Item1, coord.Y + offs.Item2);
        }

        public static List<(CardinalDirection,int, int)> GetAdjacentCoords((int X, int Y) coord)
        {
            return new List<(CardinalDirection,int, int)>()
            {
                (CardinalDirection.North,coord.X+0,coord.Y-1),
                (CardinalDirection.South,coord.X+0,coord.Y+1),
                (CardinalDirection.West,coord.X-1,coord.Y+0),
                (CardinalDirection.East,coord.X+1,coord.Y+0),
                (CardinalDirection.NorthEast,coord.X+1,coord.Y-1),
                (CardinalDirection.SouthEast,coord.X+1,coord.Y+1),
                (CardinalDirection.NorthWest,coord.X-1,coord.Y-1),
                (CardinalDirection.SouthWest,coord.X-1,coord.Y+1),
            };
        }
    }
}
