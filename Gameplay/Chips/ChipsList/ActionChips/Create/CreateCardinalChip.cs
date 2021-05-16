using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class CreateCardinalChip : InputPin<CardinalDirection>, InputPin2<BlockTemplate>, InputPin3<BlockMode>
    {
        public string Name { get; set; }

        public CardinalDirection ChipInput1 { get; set; }
        public BlockTemplate ChipInput2 { get; set; }
        public BlockMode ChipInput3 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddCreationAction(actor, ChipInput2, ChipInput3,ChipInput1);
        }
    }
}
