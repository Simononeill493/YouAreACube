using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    [Serializable()]
    public class WorldTicker
    {
        private WorldTickManager _tickManager;
        public WorldTicker() => _tickManager = new WorldTickManager();

        public void AddSector(Sector sector) => _tickManager.AddSector(sector);
        public void TickWorld(World world,UserInput input)
        {
            var emmigrants = _tickSectors(world, input);
            _addSectorEmmigrants(world, emmigrants);

            _tickManager.IncrementWorldTimer();
        }

        private SectorEmmigrantsList _tickSectors(World world, UserInput input)
        {
            var emmigrantsFullList = new SectorEmmigrantsList();
            var sectorsToUpdate = world.GetUpdatingSectors(_tickManager);

            foreach (var sector in sectorsToUpdate)
            {
                var emmigrants = _tickSector(sector, input);
                emmigrantsFullList.AddList(emmigrants);
            }

            return emmigrantsFullList;
        }  
        private SectorEmmigrantsList _tickSector(Sector sector,UserInput input)
        {
            var emmigrants = sector.Tick(input, _tickManager);
            _tickManager.IncrementSectorTimer(sector);
            return emmigrants;
        }
        private void _addSectorEmmigrants(World world, SectorEmmigrantsList emmigrants)
        {
            foreach (var emmigrant in emmigrants.GetAll())
            {
                if (!emmigrant.Block.IsMovingBetweenSectors)
                {
                    throw new Exception("Block moved between sectors, but not marked as such");
                }
                emmigrant.Block.IsMovingBetweenSectors = false;

                world.GetSector(emmigrant.SectorLocation).AddBlockToSector(emmigrant.Block);
            }
        }
    }
}
