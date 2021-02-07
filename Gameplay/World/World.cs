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
        public WorldTicker _ticker;

        public Dictionary<Point, Sector> SectorGrid;
        public Sector Centre;
        public Sector Focus;

        public Random Random;
        private int _seed;

        public World(int seed,Sector centre)
        {
            _seed = seed;
            Random = new Random(_seed);

            Centre = centre;

            SectorGrid = new Dictionary<Point, Sector>();
            SectorGrid[new Point(0, 0)] = Centre;

            _ticker = new WorldTicker();
        }

        public void Tick(UserInput input) => _ticker.TickWorld(this, input);

        public List<Sector> GetUpdatingSectors(TickManager tickCounter)
        {
            var list = new List<Sector>();

            list.Add(Focus);
            list.AddRange(Focus.Neighbours);

            return list;
        }

        public void FocusOn(Block block)
        {
            Focus = GetSector(block.Location.SectorID);
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
            _ticker.AddSector(sector);

            AttachmentUtils.AttachToWorld(this, sector);
        }
        public bool HasSector(Point sectorCoords)
        {
            return SectorGrid.ContainsKey(sectorCoords);
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
