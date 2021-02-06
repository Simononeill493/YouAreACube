using Microsoft.Xna.Framework;
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
        public Dictionary<Point, Sector> SectorGrid;
        public Sector Centre;

        public Random Random;
        private int _seed;

        public World(int seed,Sector centre)
        {
            _seed = seed;
            Random = new Random(_seed);

            Centre = centre;

            SectorGrid = new Dictionary<Point, Sector>();
            SectorGrid[new Point(0, 0)] = Centre;
        }

        public List<Sector> GetUpdatingSectors(TickManager tickCounter)
        {
            var list = new List<Sector> { SectorGrid[Kernel.HostStatic.Location.SectorID] };
            return list;
            //var list =  new List<Sector> { SectorGrid[new Point(0,0)] };

            if(tickCounter.WorldTicks% 2==0)
            {
                list.Add(SectorGrid[new Point(-1, 0)]);
            }
            if (tickCounter.WorldTicks% 3 == 0)
            {
                list.Add(SectorGrid[new Point(-2, 0)]);
            }
            if (tickCounter.WorldTicks % 4 == 0)
            {
                list.Add(SectorGrid[new Point(-3, 0)]);
            }

            return list;
        }

        public bool HasTile(Point worldCoords)
        {
            var sectorLocation = WorldUtils.GetLocationOfSector(worldCoords);
            return HasSector(sectorLocation);
        }
        public Tile GetTile(Point worldCoords)
        {
            var sectorLocation = WorldUtils.GetLocationOfSector(worldCoords);
            var sector = GetSector(sectorLocation);

            var sectorTileCoords = WorldUtils.ConvertToSectorCoords(worldCoords);
            var tile = sector.TileGrid[sectorTileCoords.X, sectorTileCoords.Y];

            return tile;
        }

        public void AddSector(Sector sector)
        {
            var location = sector.AbsoluteLocation;
            if(HasSector(location))
            {
                throw new Exception("Sector already exists at this location");
            }

            SectorGrid[location] = sector;
            AttachmentUtils.AttachToWorld(this, sector);
        }
        public bool HasSector(Point coords)
        {
            return SectorGrid.ContainsKey(coords);
        }
        public Sector GetSector(Point coord)
        {
            return SectorGrid[coord];
        }
        public Sector GetSector(Tile tile)
        {
            var secCoords = WorldUtils.GetLocationOfSector(tile.AbsoluteLocation);
            var sector = SectorGrid[secCoords];
            return sector;
        }
    }
}
