using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class AddToListChip<T> : InputPin<T>, InputPin2<List<T>>
    {
        public T ChipInput { get; set; }
        public List<T> ChipInput2 { get; set; }

        public  void Run(Block actor, UserInput input, ActionsList actions)
        {
            ChipInput2.Add(ChipInput);
        }
    }
}
