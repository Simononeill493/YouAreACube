using System;
using System.Collections.Generic;

namespace IAmACube
{
    [Serializable()]
    internal class IfChip : InputPin1<bool>, IControlChip
    {

        public Chipset Yes;
        public Chipset No;

        public override void Run(Cube actor,UserInput input,ActionsList actions)
        {
            if(ChipInput1(actor))
            {
                Yes.Execute(actor, input, actions);
            }
            else
            {
                No.Execute(actor, input, actions);
            }
        }

        public List<Chipset> GetSubChipsets()
        {
            var output = new List<Chipset>();

            if(Yes!=null) 
            {
                output.AddRange(Yes.GetChipsetAndSubChipsets());
            }
            if (No != null)
            {
                output.AddRange(No.GetChipsetAndSubChipsets());
            }

            return output;
        }
    }
}