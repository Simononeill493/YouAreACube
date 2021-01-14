using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class World
    {
        public Sector Centre;
        public List<Sector> Sectors;

        public World(Sector centre)
        {
            Centre = centre;
            Sectors = new List<Sector>() { Centre };
        }
    }
}
