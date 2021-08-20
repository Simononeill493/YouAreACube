using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class HasSameKernelChip<TCube> : InputPin1<TCube>, OutputPin<bool> where TCube : Cube
    {
        public bool Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            if(ChipInput1==null)
            {
                Value = (false);
            }
            else
            {
                Value = (actor.Source.Equals(ChipInput1.Source));
            }
        }
    }
}
