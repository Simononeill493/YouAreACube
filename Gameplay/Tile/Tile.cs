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
        public Point SectorID;

        public GroundBlock Ground { get; set; }
        public SurfaceBlock Surface { get; set; }
        public EphemeralBlock Ephemeral { get; set; }

        public bool HasSurface => (Surface != null);
        public bool HasEphemeral => (Ephemeral != null);
        public bool IsEdge => ((LocationInSector.X == 0) | (LocationInSector.X == Config.SectorSize-1) | (LocationInSector.Y == 0) | (LocationInSector.Y == Config.SectorSize-1));
        public bool IsCorner => (((LocationInSector.X == 0) | (LocationInSector.X == Config.SectorSize - 1)) & ((LocationInSector.Y == 0) | (LocationInSector.Y == Config.SectorSize - 1)));

        public Tile(Point sectorOffs,Point worldOffs,Point sectorID) : base(worldOffs)
        {
            LocationInSector = sectorOffs;
            SectorID = sectorID;
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
    
        public bool InSector(Sector sector)
        {
            return sector.AbsoluteLocation.Equals(SectorID);
        }

        public void AddBlock(Block block)
        {
            block.Location = this;

            switch (block.BlockType)
            {
                case BlockType.Surface:
                    AddSurface((SurfaceBlock)block);
                    break;
                case BlockType.Ground:
                    AddGround((GroundBlock)block);
                    break;
                case BlockType.Ephemeral:
                    AddEphemeral((EphemeralBlock)block);
                    break;
            }
        }
        public void AddSurface(SurfaceBlock block)
        {
            block.Location = this;

            if (HasSurface)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            Surface = block;
        }
        public void AddEphemeral(EphemeralBlock block)
        {
            block.Location = this;

            if (HasEphemeral)
            {
                Console.WriteLine("Warning: adding a surface block to a sector at a tile that has already been filled.");
            }

            Ephemeral = block;
        }
        public void AddGround(GroundBlock block)
        {
            block.Location = this;

            if (Ground != null)
            {
                Console.WriteLine("Warning: adding ground to a sector at a tile that has already been filled.");
            }

            Ground = block;
        }
    }
}
