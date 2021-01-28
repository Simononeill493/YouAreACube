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
        public Dictionary<CardinalDirection, Tile> Adjacent;
        public GroundBlock Ground;
        public SurfaceBlock Contents;

        public Tile(int x,int y)
        {
            X = x;
            Y = y;
            Adjacent = new Dictionary<CardinalDirection, Tile>();
        }

        public CardinalDirection ApproachDirection(Tile other)
        {
            if(other.X>X)
            {
                if(other.Y>Y)
                {
                    return CardinalDirection.SouthEast;
                }
                else if(other.Y<Y)
                {
                    return CardinalDirection.NorthEast;
                }

                return CardinalDirection.East;
            }
            else if(other.X<X)
            {
                if (other.Y > Y)
                {
                    return CardinalDirection.SouthWest;
                }
                else if (other.Y < Y)
                {
                    return CardinalDirection.NorthWest ;
                }

                return CardinalDirection.West;
            }
            else if(other.Y>Y)
            {
                return CardinalDirection.South;
            }

            return CardinalDirection.North;
        }
        public CardinalDirection FleeDirection(Tile other)
        {
            return DirectionUtils.Reverse[ApproachDirection(other)];
        }
    }
}
