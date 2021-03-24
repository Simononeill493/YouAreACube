using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class ShuffleListChip<T> : InputPin<List<T>>
    {
        public List<T> ChipInput { get; set; }

        public void Run(Block actor, UserInput input, ActionsList actions)
        {
            ChipInput = RandomUtils.Shuffle(ChipInput);
        }
    }
}
