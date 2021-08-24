using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class MoveRelativeChip : InputPin1<RelativeDirection>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddMoveAction(actor, ChipInput1(actor));
        }
    }
}
