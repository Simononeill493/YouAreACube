using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class CreateEphemeralChip : InputPin<CardinalDirection>, InputPin2<TemplateVersionList>, InputPin3<int>
    {
        public string Name { get; set; }

        public CardinalDirection ChipInput1 { get; set; }
        public TemplateVersionList ChipInput2 { get; set; }
        public int ChipInput3 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.CreateBlock(actor, ChipInput2, ChipInput3, BlockMode.Ephemeral,ChipInput1);
        }
    }
}
