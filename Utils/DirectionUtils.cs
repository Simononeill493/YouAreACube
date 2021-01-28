using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DirectionUtils
    {
        public static Dictionary<Direction,Direction> Reverse;
        public static Dictionary<Direction, Tuple<int,int>> XYOffset;

        public static void Init()
        {
            Reverse = new Dictionary<Direction, Direction>();
            Reverse[Direction.Top] = Direction.Bottom;
            Reverse[Direction.Bottom] = Direction.Top;
            Reverse[Direction.Left] = Direction.Right;
            Reverse[Direction.Right] = Direction.Left;
            Reverse[Direction.TopRight] = Direction.BottomLeft;
            Reverse[Direction.BottomRight] = Direction.TopLeft;
            Reverse[Direction.TopLeft] = Direction.BottomRight;
            Reverse[Direction.BottomLeft] = Direction.TopRight;

            XYOffset = new Dictionary<Direction, Tuple<int, int>>();
            XYOffset[Direction.Top] = new Tuple<int, int>(0, -1);
            XYOffset[Direction.Bottom] = new Tuple<int, int>(0, 1);
            XYOffset[Direction.Left] = new Tuple<int, int>(-1, 0);
            XYOffset[Direction.Right] = new Tuple<int, int>(1, 0);
            XYOffset[Direction.TopRight] = new Tuple<int, int>(1, -1);
            XYOffset[Direction.BottomRight] = new Tuple<int, int>(1, 1);
            XYOffset[Direction.TopLeft] = new Tuple<int, int>(-1, -1);
            XYOffset[Direction.BottomLeft] = new Tuple<int, int>(-1, 1);

        }
        
        public static (Direction left,Direction right) Parallel(Direction dir)
        {
            var left = _underflowMod(((int)dir - 2), 8);
            var right = _underflowMod(((int)dir + 2), 8);

            return ((Direction)left, (Direction)right);
        }

        public static bool IsDiagonal(Direction dir)
        {
            return !((int)dir % 2 == 0);
        }

        static int _underflowMod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
