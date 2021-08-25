using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class SetVariableChip<TVariableType> : InputPin2<int,TVariableType>
    {
        public override void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actor.Variables[ChipInput1(actor)] = ChipInput2(actor);
        }
    }
}
