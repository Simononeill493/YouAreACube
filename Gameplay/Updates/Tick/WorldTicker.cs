using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IAmACube
{
    public class WorldTicker
    {
        public List<(Block, Point)> ToAddToSectorTracking = new List<(Block, Point)>();

        public void TickWorld(World world,UserInput input,TickManager tickCounter)
        {
            ToAddToSectorTracking.Clear();

            _tickSectors(world, input, tickCounter);

            foreach(var block in ToAddToSectorTracking)
            {
                var sector = world.GetSector(block.Item2);
                sector.AddToSector(block.Item1);

                if(block.Item1.IsMoving)
                {
                    sector.UpdateManager.MoveManager.AddMovingBlockFromOtherSector(block.Item1);
                }
            }
        }
        private void _tickSectors(World world, UserInput input, TickManager tickCounter)
        {
            var sectorsToUpdate = world.GetUpdatingSectors(tickCounter);
            foreach (var sector in sectorsToUpdate)
            {
                _tickSector(sector, input, tickCounter);
            }
        }  
        private void _tickSector(Sector sector,UserInput input,TickManager tickCounter)
        {
            tickCounter.SectorTick(sector);
            var actions = sector.GetBlockActions(input, tickCounter);
            sector.UpdateManager.Update(actions);

            ToAddToSectorTracking.AddRange(sector.UpdateManager.MoveManager.MovedOutOfSector);
            ToAddToSectorTracking.AddRange(sector.UpdateManager.CreationManager.CreatedOutOfSector);

            sector.UpdateManager.MoveManager.MovedOutOfSector.Clear();
            sector.UpdateManager.CreationManager.CreatedOutOfSector.Clear();
        }
    }
}
