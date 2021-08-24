using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class FirstOfListChip<T> : InputPin1<List<T>>, OutputPin<T>
    {
        public T Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = (ChipInput1(actor).FirstOrDefault());
        }
    }
}