using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class HasSameKernelChip<TCube> : OutputPin<bool>, InputPin1<TCube> where TCube : Cube
    {
        public TCube ChipInput1 { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            if(ChipInput1==null)
            {
                SetOutput(false);
            }
            else
            {
                SetOutput(actor.Source.Equals(ChipInput1.Source));
            }
        }
    }
}
