using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class SetVariableChip<TVariableType> : InputPin2<int,TVariableType>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            var num = ChipInput1(actor);
            var val = ChipInput2(actor);
            actor.Variables[num] = val;
        }
    }
}
