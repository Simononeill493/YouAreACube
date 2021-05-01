using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class GetSurfaceNeighboursChip : OutputPin<List<SurfaceBlock>>
    {
        public override void Run(Block actor, UserInput input,ActionsList actions)
        {
            var neighbours = actor.Location.Adjacent.Where(l => l.Value.HasSurface).Select(l => l.Value.Surface).ToList();
            SetOutput(neighbours);
        }
    }
}