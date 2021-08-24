using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class GiveEnergyRelativeChip : InputPin3<RelativeDirection, CubeMode, int>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddGiveEnergyAction(actor, ChipInput2(actor), ChipInput1(actor), ChipInput3(actor));
        }
    }
}
