using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class GetNeighbouringBlocksOfTypeChip: OutputPin<List<Block>>, InputPin<BlockMode>
    {
        public BlockMode ChipInput1 { get; set; }

        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            var neighbours = actor.Location.Adjacent.Where(l => l.Value.HasBlock(ChipInput1)).Select(l => l.Value.GetBlock(ChipInput1)).ToList();
            SetOutput(neighbours);
        }
    }
}

