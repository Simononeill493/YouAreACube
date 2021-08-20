using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class AddToListChip<T> : InputPin2<T,List<T>>
    {
        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            ChipInput2.Add(ChipInput1);
        }
    }
}
