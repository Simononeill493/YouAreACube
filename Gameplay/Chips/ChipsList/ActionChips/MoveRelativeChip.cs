using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class MoveRelativeChip : InputPin<MovementDirection>
    {
        public MovementDirection ChipInput { get; set; }

        public void Run(Block actor, UserInput userInput, EffectsList effects)
        {
            effects.StartMove(actor, ChipInput);
        }
    }
}
