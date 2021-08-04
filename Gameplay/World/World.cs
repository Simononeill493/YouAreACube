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
        public string Name;

        public WorldTicker _ticker;
        public WorldKernel WorldKernel;

        public Dictionary<IntPoint, Sector> SectorGrid;
        public Sector Focus;

        public Random Random;
        private int _seed;

        public int SectorSize;

        public World(int seed,int sectorSize)
        {
            SectorSize = sectorSize;
            _seed = seed;
            Random = new Random(_seed);

            SectorGrid = new Dictionary<IntPoint, Sector>();
            _ticker = new WorldTicker();
            WorldKernel = new WorldKernel(_ticker);
        }

        public void Tick(UserInput input) => _ticker.TickWorld(this, input);

        public List<Sector> GetUpdatingSectors(WorldTickManager tickCounter)
        {
            var list = new List<Sector> { Focus };
            list.AddRange(Focus.Neighbours);

            return list;
        }

        public void FocusOn(Cube block) => Focus = GetSector(block.Location.SectorID);


        public void InitializeSession()
        {
            var sectorsActiveBlockLists = SectorGrid.Values.Select(s => s.ActiveBlocks);
            foreach(var blockList in sectorsActiveBlockLists)
            {
                foreach(var block in blockList)
                {
                    block.SetTemplateToRuntime();
                }
            }

        }

        public bool HasTile(IntPoint worldCoords) => HasSector(WorldUtils.WorldCoordsToSectorLocation(worldCoords,SectorSize));

        public Tile GetTile(IntPoint worldCoords)
        {
            var sectorLocation = WorldUtils.WorldCoordsToSectorLocation(worldCoords,SectorSize);
            var sector = GetSector(sectorLocation);

            var sectorTileCoords = WorldUtils.WorldCoordsToInternalSectorCoords(worldCoords,SectorSize);
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

        public bool HasSector(IntPoint sectorCoords) => SectorGrid.ContainsKey(sectorCoords);
        public Sector GetSector(IntPoint coord) => SectorGrid[coord];
        public Sector GetSector(Tile tile) => SectorGrid[WorldUtils.WorldCoordsToSectorLocation(tile.AbsoluteLocation,SectorSize)];
        public List<Sector> GetAllSectors() => SectorGrid.Values.ToList();
    }
}
