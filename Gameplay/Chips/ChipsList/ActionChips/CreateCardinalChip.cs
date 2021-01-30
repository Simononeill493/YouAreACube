using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class CreateSurfaceCardinalChip : InputPin<CardinalDirection>, InputPin2<BlockTemplate>
    {
        public CardinalDirection ChipInput { get; set; }
        public BlockTemplate ChipInput2 { get; set; }

        public void Run(Block actor, UserInput userInput, EffectsList effects)
        {
            effects.CreateBlock(actor, ChipInput2, BlockType.Surface, ChipInput);
        }
    }
}
