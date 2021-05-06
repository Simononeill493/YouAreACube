using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class GetNeighbouringBlocksChip : OutputPin<List<Block>>
    {
        public override void Run(Block actor, UserInput input,ActionsList actions)
        {
            var neighbours = new List<Block>();
            var blocks = actor.Location.Adjacent.Values.Select(l => l.GetBlocks());
            foreach(var b in blocks)
            {
                neighbours.AddRange(b);
            }
            SetOutput(neighbours);
        }
    }
}