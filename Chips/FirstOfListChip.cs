using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class FirstOfListChip<T> : OutputPin<T>, InputPin<IEnumerable<T>>
    {
        public IEnumerable<T> ChipInput { get => _input; set => _input = value; }
        private IEnumerable<T> _input;

        public override void Run(Block actor, UserInput input)
        {
            SetOutput(ChipInput.FirstOrDefault());
        }
    }
}