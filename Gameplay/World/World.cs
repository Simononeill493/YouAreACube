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
        public Sector Current;
        public Sector Centre;
        public List<Sector> Sectors;

        public Random Random;
        private int _seed;

        public World(int seed,Sector centre)
        {
            _seed = seed;
            Random = new Random(_seed);

            Centre = centre;
            Current = centre;
            Sectors = new List<Sector>() { centre };
        }

        public List<Sector> GetUpdatingSectors(TickCounter tickCounter)
        {
            return new List<Sector> { Current };
        }
    }
}
