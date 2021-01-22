using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Tile
    {
        public int X;
        public int Y;
        public Dictionary<Direction, Tile> Adjacent;
        public GroundBlock Ground;
        public Block Contents;

        public Tile(int x,int y)
        {
            X = x;
            Y = y;
            Adjacent = new Dictionary<Direction, Tile>();
        }

        public Direction ApproachDirection(Tile other)
        {
            if(other.X>X)
            {
                if(other.Y>Y)
                {
                    return Direction.BottomRight;
                }
                else if(other.Y<Y)
                {
                    return Direction.TopRight;
                }

                return Direction.Right;
            }
            else if(other.X<X)
            {
                if (other.Y > Y)
                {
                    return Direction.BottomLeft;
                }
                else if (other.Y < Y)
                {
                    return Direction.TopLeft ;
                }

                return Direction.Left;
            }
            else if(other.Y>Y)
            {
                return Direction.Bottom;
            }

            return Direction.Top;
        }
        public Direction FleeDirection(Tile other)
        {
            return DirectionUtils.Reverse[ApproachDirection(other)];
        }
    }
}
