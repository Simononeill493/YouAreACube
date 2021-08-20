using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class ShuffleListChip<T> : InputPin1<List<T>>, OutputPin<List<T>>
    {
        public List<T> Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            var shuffled = RandomUtils.GetShuffledList(ChipInput1);
            Value = (shuffled);
        }
    }
}
