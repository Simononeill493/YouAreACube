using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class IfChip : InputPin1<bool>, IControlChip, IIfChip
    {
        public Chipset Yes { get; set; }
        public Chipset No { get; set; }

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

        public List<Chipset> GetSubChipsetsCascade()
        {
            var output = new List<Chipset>();

            if(Yes!=null) 
            {
                output.AddRange(Yes.GetThisAndAllChipsetsCascade());
            }
            if (No != null)
            {
                output.AddRange(No.GetThisAndAllChipsetsCascade());
            }

            return output;
        }

        public List<(string, Chipset)> GetSubChipsets() => new List<(string, Chipset)>() { ("Yes", Yes), ("No", No) };

        public void SetSubChipsets(List<(string, Chipset)> subChipsets)
        {
            if(subChipsets.Count!=2) { throw new Exception(); }

            Yes = subChipsets.Where(s => s.Item1 == "Yes").First().Item2;
            No = subChipsets.Where(s => s.Item1 == "No").First().Item2;
        }

    }
}