using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class GetEphemeralNeighboursChip : OutputPin<List<EphemeralBlock>>
    {
        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            var neighbours = actor.Location.Adjacent.Where(l => l.Value.HasEphemeral).Select(l => l.Value.Ephemeral).ToList();
            SetOutput(neighbours);
        }
    }
}

