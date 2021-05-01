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
        public static Tile Dummy = new DummyTile();

        public IntPoint LocationInSector;
        public IntPoint SectorID;

        public bool IsEdge { get; private set; }
        public bool IsCorner { get; private set; }

        public GroundBlock Ground { get; set; }
        public SurfaceBlock Surface { get; set; }
        public EphemeralBlock Ephemeral { get; set; }

        public virtual bool HasThisSurface(SurfaceBlock surface) => surface == Surface;
        public virtual bool HasThisGround(GroundBlock ground) => ground == Ground;
        public virtual bool HasThisEphemeral(EphemeralBlock ephemeral) => ephemeral == Ephemeral;
        public bool InSector(Sector sector) => sector.AbsoluteLocation.Equals(SectorID);

        public bool HasSurface => (Surface != null);
        public bool HasEphemeral => (Ephemeral != null);

        public Tile(IntPoint sectorOffs,IntPoint worldOffs,IntPoint sectorID,int sectorSize) : base(worldOffs)
        {
            LocationInSector = sectorOffs;
            SectorID = sectorID;

            IsEdge = (LocationInSector.X == 0) | (LocationInSector.X == sectorSize - 1) | (LocationInSector.Y == 0) | (LocationInSector.Y == sectorSize - 1);
            IsCorner = ((LocationInSector.X == 0) | (LocationInSector.X == sectorSize - 1)) & ((LocationInSector.Y == 0) | (LocationInSector.Y == sectorSize - 1));
        }

        public bool ContainsBlock(BlockMode blockType)
        {
            switch (blockType)
            {
                case BlockMode.Surface:
                    return HasSurface;
                case BlockMode.Ground:
                    return true;
                case BlockMode.Ephemeral:
                    return HasEphemeral;
            }

            Console.WriteLine("Warning: tried to scan a tile for  an unrecognized block type: " + blockType);
            return false;
        }

        public void ClearBlock(Block block)
        {
            switch (block.BlockType)
            {
                case BlockMode.Surface:
                    if(block == Surface)
                    {
                        Console.WriteLine("Warning: deleted a surface block which wasn't in its last stored location. This shoudn't happen.");
                        Surface = null;
                    }
                    break;
                case BlockMode.Ground:
                    Console.WriteLine("Warning: you can't clear the ground!");
                    break;
                case BlockMode.Ephemeral:
                    if (block == Ephemeral)
                    {
                        throw new Exception("Destroyed an ephemeral that wasn't removed from its location properly.");
                    }
                    break;
            }
        }


    }
}
