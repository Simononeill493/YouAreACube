using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class GetNeighbouringCubesChip : OutputPin<List<Cube>>
    {
        public override void Run(Cube actor, UserInput input,ActionsList actions)
        {
            var neighbours = new List<Cube>();
            var blocks = actor.Location.Adjacent.Values.Select(l => l.GetBlocks());
            foreach(var b in blocks)
            {
                neighbours.AddRange(b);
            }
            SetOutput(neighbours);
        }
    }
}