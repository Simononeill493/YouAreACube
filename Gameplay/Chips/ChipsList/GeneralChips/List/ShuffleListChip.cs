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
        public string Name { get; set; }

        public List<T> ChipInput1 { get; set; }

        public void Run(Block actor, UserInput input, ActionsList actions)
        {
            ChipInput1 = RandomUtils.Shuffle(ChipInput1);
        }
    }
}
