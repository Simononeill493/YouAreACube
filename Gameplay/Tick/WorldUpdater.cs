using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class WorldUpdater
    {
        private EffectManager _effectManager;
        public WorldUpdater()
        {
            _effectManager = new EffectManager();
        }

        public void UpdateWorld(World world,UserInput input,TickCounter tickCounter)
        {
            var sectorsToUpdate = world.GetUpdatingSectors(tickCounter);
            foreach (var sector in sectorsToUpdate)
            {
                UpdateSector(sector,input, tickCounter);
            }
        }

        public void UpdateSector(Sector sector,UserInput input,TickCounter tickCounter)
        {
            var effects = new EffectsList();

            sector.Update(input, effects, tickCounter);
            _effectManager.ProcessEffects(sector, effects);
        }
    }
}
