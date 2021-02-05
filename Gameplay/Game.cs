using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Game
    {
        public Kernel Kernel => _save.Kernel;
        public World World => _save.World;

        private Save _save;

        private TickCounter _tickCounter;
        private WorldTicker _updater;

        public Game(Save save)
        {
            _save = save;
            _tickCounter = new TickCounter();
            _updater = new WorldTicker();
        }

        public void Update(UserInput input)
        {
            Kernel.SupplyPowerToHost();
            //World.Focus = World.GetContainingSector(Kernel.Host.Location);

            _updater.TickWorld(World, input, _tickCounter);
            _tickCounter.Tick();

            if(Config.KernelUnlimitedEnergy)
            {
                Kernel.Host.AddEnergy(Kernel.Host.EnergyCap);
            }
        }
    }
}
