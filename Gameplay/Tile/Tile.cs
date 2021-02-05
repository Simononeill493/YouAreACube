using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Tile : LocationWithNeighbors<Tile>
    {
        public Point LocationInSector;

        public bool HasSurface => (Surface != null);
        public bool HasEphemeral => (Ephemeral != null);

        public GroundBlock Ground { get; set; }
        public SurfaceBlock Surface { get; set; }
        public EphemeralBlock Ephemeral { get; set; }

        public bool IsEdge => ((LocationInSector.X == 0) | (LocationInSector.X == Config.SectorSize-1) | (LocationInSector.Y == 0) | (LocationInSector.Y == Config.SectorSize-1));
        public bool IsCorner => (((LocationInSector.X == 0) | (LocationInSector.X == Config.SectorSize - 1)) & ((LocationInSector.Y == 0) | (LocationInSector.Y == Config.SectorSize - 1)));

        public Tile(Point sectorOffs,Point worldOffs) : base(worldOffs)
        {
            LocationInSector = sectorOffs;
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
        public void ClearBlock(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Surface:
                    Surface = null;
                    break;
                case BlockType.Ground:
                    Console.WriteLine("Warning: you can't clear the ground!");
                    break;
                case BlockType.Ephemeral:
                    Ephemeral = null;
                    break;
            }
        }
    }
}
