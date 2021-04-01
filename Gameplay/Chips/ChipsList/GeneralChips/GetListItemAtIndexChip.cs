using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class GetListItemAtIndexChip<T> : OutputPin<T>, InputPin<List<T>>, InputPin2<int>
    {
        public List<T> ChipInput1 { get ; set; }
        public int ChipInput2 { get; set; }

        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            SetOutput(ChipInput1[ChipInput2]);
        }
    }
}
