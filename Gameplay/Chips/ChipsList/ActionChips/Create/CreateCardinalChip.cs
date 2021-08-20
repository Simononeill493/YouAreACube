using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class CreateCardinalChip : InputPin3<CardinalDirection,CubeTemplate,CubeMode>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddCreationAction(actor, ChipInput2, ChipInput3,ChipInput1);
        }
    }
}
