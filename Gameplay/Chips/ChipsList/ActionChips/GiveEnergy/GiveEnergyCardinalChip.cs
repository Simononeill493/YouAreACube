using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class GiveEnergyCardinalChip : InputPin3<CardinalDirection, CubeMode, int>
    {

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddGiveEnergyAction(actor, ChipInput2, ChipInput1, ChipInput3);
        }
    }
}
