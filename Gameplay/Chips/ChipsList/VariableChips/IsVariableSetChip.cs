using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.VariableChips
{
    [Serializable()]
    class IsVariableSetChip : InputPin1<int>, OutputPin<bool>
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = !(actor.Variables[ChipInput1(actor)] == null);
        }
    }
}
