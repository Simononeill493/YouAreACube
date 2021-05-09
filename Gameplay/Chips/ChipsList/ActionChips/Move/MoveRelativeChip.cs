using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class MoveRelativeChip : InputPin<RelativeDirection>
    {
        public string Name { get; set; }

        public RelativeDirection ChipInput1 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddMoveAction(actor, ChipInput1);
        }
    }
}
