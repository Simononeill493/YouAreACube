using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.GeneralChips
{
    internal class EqualsChip : InputPin2<int, int>, OutputPin<bool>
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = (ChipInput1(actor) == ChipInput2(actor));
        }
    }
}
