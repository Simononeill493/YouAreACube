using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class IsListEmptyChip<T> : InputPin1<List<T>>, OutputPin<bool>
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = (!ChipInput1(actor).Any());
        }
    }
}