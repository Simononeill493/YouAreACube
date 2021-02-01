using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IAmACube
{
    public class WorldTicker
    {
        private ActionManager _effectManager;
        public WorldTicker()
        {
            _effectManager = new ActionManager();
        }

        public void TickWorld(World world,UserInput input,TickCounter tickCounter)
        {
            var sectorsToUpdate = world.GetUpdatingSectors(tickCounter);
            foreach (var sector in sectorsToUpdate)
            {
                TickSector(sector,input, tickCounter);
            }
        }

        public void TickSector(Sector sector,UserInput input,TickCounter tickCounter)
        {
            var actions = sector.UpdateBlocks(input, tickCounter);
            _effectManager.ProcessActions(sector, actions);
        }
    }
}
