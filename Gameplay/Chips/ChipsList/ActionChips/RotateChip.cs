using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class RotateChip : InputPin1<int>
    {

        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actions.AddRotationAction(actor, ChipInput1(actor));
        }
    }
}
