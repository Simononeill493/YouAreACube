using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class ChipsetCollection
    {
        public void AssertSanityTest()
        {
            var sanityResult = SanityTest();
            if (sanityResult.Count > 0)
            {
                throw new Exception("ChipsetCollection failed sanity test");
            }
        }

        public List<string> SanityTest()
        {
            var output = new List<string>();

            if (!Modes.ContainsKey(0))
            {
                output.Add("No initial set");
            }

            foreach (var mode in Modes)
            {
                if(mode.Key<0)
                {
                    output.Add("Mode key for " + mode.Value.Name + " is less than zero");
                }

                var chipsetSanityOutput = mode.Value.SanityTest();
                output.AddRange(chipsetSanityOutput);
            }

            return output;
        }
    }
}
