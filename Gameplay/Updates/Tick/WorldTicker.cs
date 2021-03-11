using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    [Serializable()]
    public class WorldTicker
    {
        private TickManager _tickManager;
        public WorldTicker()
        {
            _tickManager = new TickManager();
        }


        public void TickWorld(World world,UserInput input)
        {
            var emmigrants = _tickSectors(world, input);
            _addSectorEmmigrants(world, emmigrants);

            _tickManager.IncrementWorldTimer();
        }

        private EmmigrantsList _tickSectors(World world, UserInput input)
        {
            var sectorEmmigrants = new EmmigrantsList();
            var sectorsToUpdate = world.GetUpdatingSectors(_tickManager);

            foreach (var sector in sectorsToUpdate)
            {
                _tickSector(sector, input, sectorEmmigrants);
            }

            return sectorEmmigrants;
        }  
        private void _tickSector(Sector sector,UserInput input, EmmigrantsList emmigrantsList)
        {
            var actions = sector.GetBlockActions(input, _tickManager);
            sector.Update(actions);

            var emmigrants = sector.PopSectorEmmigrants();
            
            emmigrantsList.AddAll(emmigrants);

            _tickManager.IncrementSectorTimer(sector);
        }

        private void _addSectorEmmigrants(World world, EmmigrantsList emmigrants)
        {
            foreach (var emmigrant in emmigrants.Created)
            {
                var sector = world.GetSector(emmigrant.Item2);
                sector.AddToSector(emmigrant.Item1);
            }

            foreach (var emmigrant in emmigrants.Moved)
            {
                var sector = world.GetSector(emmigrant.Item3);
                sector.AddMovingBlockToSector(emmigrant.Item1,emmigrant.Item2);
            }

        }

        public void AddSector(Sector sector) => _tickManager.AddSector(sector);
    }

    [Serializable()]
    public class EmmigrantsList
    {
        public List<(Block, BlockMovementData, Point)> Moved = new List<(Block, BlockMovementData, Point)>();
        public List<(Block, Point)> Created = new List<(Block, Point)>();

        public void AddAll((List<(Block, BlockMovementData, Point)> moved, List<(Block, Point)> created) toAdd)
        {
            Moved.AddRange(toAdd.moved);
            Created.AddRange(toAdd.created);
        }
    }

}
