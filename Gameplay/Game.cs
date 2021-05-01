using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Game
    {
        public Kernel Kernel;
        public World World;

        private SectorGenerator _sectorGenerator;

        public Game(Kernel kernel, World world)
        {
            Kernel = kernel;
            World = world;

            _sectorGenerator = new SectorGenerator();

            _initializeSession();
        }

        public void Update(UserInput input)
        {
            Kernel.SupplyPowerToHost();

            World.FocusOn(Kernel.Host);
            World.Tick(input);

            //_sectorGenerator.GenerateAdjacentSectors(World);

            if (Config.KernelUnlimitedEnergy)
            {
                Kernel.Host.AddEnergy(Kernel.Host.EnergyCap);
            }
        }

        private void _initializeSession()
        {
            Kernel.InitializeSession();
            World.InitializeSession();

            var hostTile = World.GetTile(Kernel.Host.Location.AbsoluteLocation);
            var liveHost = hostTile.Surface;
            Kernel.SetHost(liveHost);
        }

        public (Kernel,World) SaveAndQuit()
        {
            return (Kernel,World);
        }
    }
}
