using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IAmACube
{
    public class WorldTicker
    {
        private ActionManager _actionManager;
        private DestructionManager _destructionManager;

        public WorldTicker()
        {
            var moveManager = new MoveManager();
            var creationManager = new CreationManager();

            _actionManager = new ActionManager(moveManager,creationManager);
            _destructionManager = new DestructionManager(moveManager);
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
            _actionManager.ProcessActions(sector, actions);
            _destructionManager.DestroyDoomedBlocks(sector);
        }
    }
}
