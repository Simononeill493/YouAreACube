using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class RotationChip : InputPin<int>
    {
        public int ChipInput { get; set; }

        public void Run(Block actor, UserInput userInput, EffectsList effects)
        {
            effects.StartRotation(actor, ChipInput);
        }
    }
}
