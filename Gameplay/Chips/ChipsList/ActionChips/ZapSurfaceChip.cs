using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class ZapSurfaceChip : IChip
    {
        public string Name { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            if(actor.Location.HasSurface & !actor.ToBeDeleted())
            {
                var energy = actor.Energy;

                actor.TakeEnergy(energy);
                actor.Location.Surface.DealDamage(energy);
            }
        }
    }
}
