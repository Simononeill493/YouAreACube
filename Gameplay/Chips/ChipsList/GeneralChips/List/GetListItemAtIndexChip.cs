using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class GetListItemAtIndexChip<T> : InputPin2<List<T>, int>, OutputPin<T>
    {
        public T Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            Value = (ChipInput1(actor)[ChipInput2(actor)]);
        }
    }
}
