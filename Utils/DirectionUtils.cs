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
        }
    }
}
