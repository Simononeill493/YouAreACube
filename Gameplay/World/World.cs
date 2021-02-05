using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class World
    {
        public Sector Centre;
        public Sector Focus;

        public Dictionary<(int,int),Sector> SectorGrid;

        public Random Random;
        private int _seed;

        public World(int seed,Sector centre)
        {
            _seed = seed;
            Random = new Random(_seed);

            Centre = centre;
            Focus = centre;

            SectorGrid = new Dictionary<(int, int), Sector>();
            SectorGrid[(0, 0)] = Centre;
        }

        public List<Sector> GetUpdatingSectors(TickCounter tickCounter)
        {
            return new List<Sector> { Centre };
        }

        public void AddSector(int xOffs,int yOffs,Sector sector)
        {
            if(HasSector(xOffs,yOffs))
            {
                throw new Exception("Sector already exists in this world");
            }

            SectorGrid[(xOffs, yOffs)] = sector;
            AttachmentUtils.AttachToWorld(this, sector);
        }
        public Sector GetSector(int xOffs, int yOffs)
        {
            return SectorGrid[(xOffs, yOffs)];
        }
        public bool HasSector(int xOffs,int yOffs)
        {
            return SectorGrid.ContainsKey((xOffs, yOffs));
        }
    
        public bool HasTile(int worldX,int worldY)
        {
            var sectorCoords = _getSectorOfWorldCoords(worldX, worldY);
            return HasSector(sectorCoords.x, sectorCoords.y);
        }
        public Tile GetTile(int worldX,int worldY)
        {
            var sectorCoords = _getSectorOfWorldCoords(worldX, worldY);
            var sector = GetSector(sectorCoords.x, sectorCoords.y);

            var sectorTileCoords = _getInternalSectorCoords(worldX, worldY);
            var tile = sector.Tiles[sectorTileCoords.x, sectorTileCoords.y];

            return tile;
        }
        public Sector GetContainingSector(Tile t)
        {
            var secCoords = _getSectorOfWorldCoords(t.WorldOffset.X, t.WorldOffset.Y);
            var sector = GetSector(secCoords.x, secCoords.y);
            return sector;
        }

        private (int x,int y) _getSectorOfWorldCoords(int worldX, int worldY)
        {
            var ret = (worldX / Config.SectorSize, worldY / Config.SectorSize);

            if (worldX < 0 & (-worldX % Config.SectorSize != 0)) 
            { 
                ret.Item1 -= 1; 
            }
            if (worldY < 0 & (-worldY % Config.SectorSize != 0)) 
            { 
                ret.Item2 -= 1; 
            }

            return ret;
        }

        private (int x, int y) _getInternalSectorCoords(int worldX, int worldY)
        {
            int x, y = 0;

            if(worldX>=0)
            {
                x = worldX % Config.SectorSize;
            }
            else
            {
                x = (Config.SectorSize-1)-((-worldX -1) % Config.SectorSize);
            }

            if (worldY >= 0)
            {
                y = worldY % Config.SectorSize;
            }
            else
            {
                y = (Config.SectorSize - 1) - ((-worldY - 1) % Config.SectorSize);
            }

            var ret = (x, y);
            return ret;

        }

    }
}
