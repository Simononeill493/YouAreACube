using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class CreateCardinalChip : InputPin<CardinalDirection>, InputPin2<BlockTemplate>, InputPin3<BlockType>
    {
        public string Name { get; set; }

        public CardinalDirection ChipInput { get; set; }
        public BlockTemplate ChipInput2 { get; set; }
        public BlockType ChipInput3 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.CreateBlock(actor, ChipInput2, ChipInput3, ChipInput);
        }
    }
}
