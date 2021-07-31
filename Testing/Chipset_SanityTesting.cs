using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class Chipset
    {
        public void AssertSanityTest()
        {
            var sanityResult = SanityTest();
            if (sanityResult.Count>0)
            {
                throw new Exception("Chipset failed sanity test");
            }
        }

        public List<string> SanityTest()
        {
            var output = new List<string>();

            if (Name == null)
            {
                output.Add("Name is null");
            }
            if (Chips == null)
            {
                output.Add("Chips is null");
            }
            if (ControlChips == null)
            {
                output.Add("ControlChips is null");
            }
            if (Chips.Count==0)
            {
                output.Add("Chips is empty");
            }

            foreach(var chip in Chips)
            {
                var chipSanityOutput = chip.SanityTest();
                output.AddRange(chipSanityOutput);
            }

            return output;
        }
    }
}
