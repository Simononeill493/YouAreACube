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
        public List<Sector> Regions;

        public World(Sector centre)
        {
            Centre = centre;
            Regions = new List<Sector>() { Centre };
        }
    }
}
