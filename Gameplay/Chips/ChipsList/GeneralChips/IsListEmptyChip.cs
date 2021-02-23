using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class IsListEmptyChip<T> : OutputPin<bool>, InputPin<List<T>>
    {
        public List<T> ChipInput { get; set; }

        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            SetOutput(!ChipInput.Any());
        }
    }
}