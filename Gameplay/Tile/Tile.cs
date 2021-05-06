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
        public IntPoint LocationInSector { get; }
        public IntPoint SectorID { get; }
        public bool InSector(Sector sector) => sector.AbsoluteLocation.Equals(SectorID);

        public GroundBlock Ground { get; set; }
        public SurfaceBlock Surface { get; set; }
        public EphemeralBlock Ephemeral { get; set; }
        public Block GetBlock(BlockMode blockMode)
        {
            switch (blockMode)
            {
                case BlockMode.Surface:
                    return Surface;
                case BlockMode.Ground:
                    return Ground;
                case BlockMode.Ephemeral:
                    return Ephemeral;
            }

            throw new Exception("Unrecognized block type");
        }
        public List<Block> GetBlocks()
        {
            var output = new List<Block>() { Ground };
            if(HasSurface) { output.Add(Surface); }
            if (HasEphemeral) { output.Add(Ephemeral); }
            return output;
        }

        public virtual bool HasThisSurface(SurfaceBlock surface) => surface == Surface;
        public virtual bool HasThisGround(GroundBlock ground) => ground == Ground;
        public virtual bool HasThisEphemeral(EphemeralBlock ephemeral) => ephemeral == Ephemeral;

        public bool HasSurface => (Surface != null);
        public bool HasEphemeral => (Ephemeral != null);
        public bool HasBlock(BlockMode blockMode)
        {
            switch (blockMode)
            {
                case BlockMode.Surface:
                    return HasSurface;
                case BlockMode.Ground:
                    return true;
                case BlockMode.Ephemeral:
                    return HasEphemeral;
            }
            return false;
        }

        public bool IsEdge { get; private set; }
        public bool IsCorner { get; private set; }

        public Tile(IntPoint sectorOffset,IntPoint worldOffset,IntPoint sectorID,int sectorSize) : base(worldOffset)
        {
            LocationInSector = sectorOffset;
            SectorID = sectorID;

            IsEdge = (LocationInSector.X == 0) | (LocationInSector.X == sectorSize - 1) | (LocationInSector.Y == 0) | (LocationInSector.Y == sectorSize - 1);
            IsCorner = ((LocationInSector.X == 0) | (LocationInSector.X == sectorSize - 1)) & ((LocationInSector.Y == 0) | (LocationInSector.Y == sectorSize - 1));
        }

        public bool ContainsBlockType(BlockMode blockType)
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

            throw new Exception("Tried to scan a tile for an unrecognized block type: " + blockType);
        }
        public void ClearBlock(Block block)
        {
            switch (block.BlockType)
            {
                case BlockMode.Surface:
                    _clearSurface(block);
                    return;
                case BlockMode.Ephemeral:
                    _clearEphemeral(block);
                    return;
            }

            throw new Exception("Tried to clear a block type which cannot be cleared: " + block.BlockType);
        }

        private void _clearSurface(Block block)
        {
            if (block != Surface)
            {
                throw new Exception("Deleted a surface block whose tile thinks it's not there.");
            }
            Surface = null;
        }
        private void _clearEphemeral(Block block)
        {
            if (block == Ephemeral)
            {
                throw new Exception("Ephemeral block should have been cleared when it faded, but it's still in its tile.");
            }
        }

        #region dummyTile
        public static Tile Dummy = new DummyTile();
        public bool IsDummy => AbsoluteLocation == Dummy.AbsoluteLocation;
        #endregion
    }
}
