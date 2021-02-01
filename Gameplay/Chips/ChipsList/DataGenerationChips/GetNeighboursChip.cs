using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class GetNeighboursChip : OutputPin<IEnumerable<SurfaceBlock>>
    {
        public override void Run(Block actor, UserInput input,ActionsList actions)
        {
            var neighbours = actor.Location.Adjacent.Values.Where(l => (l.HasSurface)).Select(l => l.Surface);
            SetOutput(neighbours);
        }
    }
}