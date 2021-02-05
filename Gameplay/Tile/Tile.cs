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
        public (int X,int Y) WorldOffset;
        public (int X, int Y) SectorOffset;

        public Dictionary<CardinalDirection, Tile> Adjacent;

        public bool HasSurface => (Surface != null);
        public bool HasEphemeral => (Ephemeral != null);

        public GroundBlock Ground { get; set; }
        public SurfaceBlock Surface { get; set; }
        public EphemeralBlock Ephemeral { get; set; }

        public bool IsEdge => ((SectorOffset.X == 0) | (SectorOffset.X == Config.SectorSize-1) | (SectorOffset.Y == 0) | (SectorOffset.Y == Config.SectorSize-1));
        public bool IsCorner => (((SectorOffset.X == 0) | (SectorOffset.X == Config.SectorSize - 1)) & ((SectorOffset.Y == 0) | (SectorOffset.Y == Config.SectorSize - 1)));

        public Tile(int sectorX, int sectorY,int worldX,int worldY)
        {
            SectorOffset = (sectorX, sectorY);
            WorldOffset = (worldX, worldY);

            Adjacent = new Dictionary<CardinalDirection, Tile>();
        }

        public bool DirectionIsValid(CardinalDirection direction)
        {
            return Adjacent.ContainsKey(direction);
        }

        public bool ContainsBlock(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Surface:
                    return HasSurface;
                case BlockType.Ground:
                    return true;
                case BlockType.Ephemeral:
                    return HasEphemeral;
            }

            Console.WriteLine("Warning: tried to scan a tile for  an unrecognized block type: " + blockType);
            return false;
        }

        public CardinalDirection ApproachDirection(Tile other)
        {
            if(other.WorldOffset.X> WorldOffset.X)
            {
                if(other.WorldOffset.Y > WorldOffset.Y)
                {
                    return CardinalDirection.SouthEast;
                }
                else if(other.WorldOffset.Y < WorldOffset.Y)
                {
                    return CardinalDirection.NorthEast;
                }

                return CardinalDirection.East;
            }
            else if(other.WorldOffset.X < WorldOffset.X)
            {
                if (other.WorldOffset.Y > WorldOffset.Y)
                {
                    return CardinalDirection.SouthWest;
                }
                else if (other.WorldOffset.Y < WorldOffset.Y)
                {
                    return CardinalDirection.NorthWest ;
                }

                return CardinalDirection.West;
            }
            else if(other.WorldOffset.Y > WorldOffset.Y)
            {
                return CardinalDirection.South;
            }

            return CardinalDirection.North;
        }
        public CardinalDirection FleeDirection(Tile other)
        {
            return ApproachDirection(other).Reverse();
        }


        public bool HasNeighbour(CardinalDirection direction)
        {
            return Adjacent.ContainsKey(direction);
        }

        public override string ToString()
        {
            return "(" + WorldOffset.X + " " + WorldOffset.Y + ")" + " " + Adjacent.Count;
        }
    }
}
