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

        public EffectsList Update(UserInput input,TickCounter tickCounter)
        {
            var effects = new EffectsList();

            Current.Update(input, effects,tickCounter);

            return effects;
        }
    }
}
