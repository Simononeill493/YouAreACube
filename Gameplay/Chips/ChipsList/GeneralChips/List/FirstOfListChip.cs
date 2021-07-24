using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class FirstOfListChip<T> : OutputPin<T>, InputPin1<List<T>>
    {
        public List<T> ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            SetOutput(ChipInput1.FirstOrDefault());
        }
    }
}