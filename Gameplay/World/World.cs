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

        public World(Sector centre)
        {
            Centre = centre;
            Sectors = new List<Sector>() { Centre };

            Current = Centre;
        }

        public void Update(UserInput input,TickCounter tickCounter)
        {
            Current.Update(input,tickCounter);
        }
    }
}
