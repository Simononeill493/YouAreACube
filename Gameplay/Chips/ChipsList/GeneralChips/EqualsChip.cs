using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.GeneralChips
{
    internal class EqualsChip : OutputPin<bool>, InputPin<int>, InputPin2<int>
    {
        public int ChipInput1 { get; set; }
        public int ChipInput2 { get; set; }

        public override void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(ChipInput1 == ChipInput2);
        }
    }
}
