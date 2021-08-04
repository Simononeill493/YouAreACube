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
        public string Sprite = "Grass";
        public IntPoint LocationInSector { get; }
        public IntPoint SectorID { get; }
        public bool InSector(Sector sector) => sector.AbsoluteLocation.Equals(SectorID);

        public GroundCube Ground { get; set; }
        public SurfaceCube Surface { get; set; }
        public EphemeralCube Ephemeral { get; set; }
        public Cube GetBlock(CubeMode blockMode)
        {
            switch (blockMode)
            {
                case CubeMode.Surface:
                    return Surface;
                case CubeMode.Ground:
                    return Ground;
                case CubeMode.Ephemeral:
                    return Ephemeral;
            }

            throw new Exception("Unrecognized block type");
        }
        public List<Cube> GetBlocks()
        {
            var output = new List<Cube>();
            if (HasGround) { output.Add(Ground); }
            if (HasSurface) { output.Add(Surface); }
            if (HasEphemeral) { output.Add(Ephemeral); }
            return output;
        }

        public virtual bool HasThisSurface(SurfaceCube surface) => surface == Surface;
        public virtual bool HasThisGround(GroundCube ground) => ground == Ground;
        public virtual bool HasThisEphemeral(EphemeralCube ephemeral) => ephemeral == Ephemeral;

        public bool HasGround => (Ground != null);
        public bool HasSurface => (Surface != null);
        public bool HasEphemeral => (Ephemeral != null);
        public bool HasBlock(CubeMode blockMode)
        {
            switch (blockMode)
            {
                case CubeMode.Surface:
                    return HasSurface;
                case CubeMode.Ground:
                    return HasGround;
                case CubeMode.Ephemeral:
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

        public bool ContainsBlockType(CubeMode blockType)
        {
            switch (blockType)
            {
                case CubeMode.Surface:
                    return HasSurface;
                case CubeMode.Ground:
                    return HasGround;
                case CubeMode.Ephemeral:
                    return HasEphemeral;
            }

            throw new Exception("Tried to scan a tile for an unrecognized block type: " + blockType);
        }
        public void ClearBlock(Cube block)
        {
            switch (block.CubeMode)
            {
                case CubeMode.Surface:
                    _clearSurface(block);
                    return;
                case CubeMode.Ephemeral:
                    _clearEphemeral(block);
                    return;
                case CubeMode.Ground:
                    _clearGround(block);
                    return;

            }

            throw new Exception("Tried to clear a block type which cannot be cleared: " + block.CubeMode);
        }

        private void _clearSurface(Cube block)
        {
            if (block != Surface)
            {
                throw new Exception("Deleted a surface block whose tile thinks it's not there.");
            }
            Surface = null;
        }
        private void _clearEphemeral(Cube block)
        {
            if (block == Ephemeral)
            {
                throw new Exception("Ephemeral block should have been cleared when it faded, but it's still in its tile.");
            }
        }
        private void _clearGround(Cube block)
        {
            if (block != Ground)
            {
                throw new Exception("Deleted a ground block whose tile thinks it's not there.");
            }
            Ground = null;
        }

        #region dummyTile
        public static Tile Dummy = new DummyTile();
        public bool IsDummy => AbsoluteLocation == Dummy.AbsoluteLocation;
        #endregion
    }
}
