using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class GetNeighboursChip : OutputPin<IEnumerable<SurfaceBlock>>
    {
        public override void Run(Block actor, UserInput input)
        {
            var neighbours = actor.Location.Adjacent.Values.Where(l => l.Contents != null).Select(l => l.Contents);
            SetOutput(neighbours);
        }
    }
}