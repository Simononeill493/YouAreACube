using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Game
    {
        private Save _save;
        public Kernel Kernel => _save.Kernel;
        public World World => _save.World;

        public Game(Save save)
        {
            _save = save;
        }

        public void Update(UserInput input)
        {
            Kernel.SupplyPowerToHost();

            World.FocusOn(Kernel.Host);
            World.Tick(input);

            if(Config.KernelUnlimitedEnergy)
            {
                Kernel.Host.AddEnergy(Kernel.Host.EnergyCap);
            }
        }
    }
}
