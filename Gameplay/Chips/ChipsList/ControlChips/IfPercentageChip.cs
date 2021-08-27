using System;
using System.Collections.Generic;

namespace IAmACube
{
    [Serializable()]
    internal class IfPercentageChip : InputPin2<int,int>, IControlChip, IIfChip
    {
        public Chipset Yes { get; set; }
        public Chipset No { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            if (RandomUtils.RandomNumber(ChipInput2(actor)+1) <= ChipInput1(actor))
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

            if (Yes != null)
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