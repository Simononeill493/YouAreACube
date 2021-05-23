using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.ActionChips
{
    [Serializable()]
    class ApproachBlockChip<TBlock> : InputPin<TBlock> where TBlock : Block
    {
        public string Name { get; set; }
        public TBlock ChipInput1 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddApproachAction(actor, ChipInput1);
        }
    }
}
