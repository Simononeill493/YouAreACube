using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class Chip_SanityTesting
    {
        public static List<string> SanityTest(this IChip chip)
        {
            var output = new List<string>();

            if (chip.Name == null)
            {
                output.Add("Chip name is null");
            }

            return output;
        }

    }
}
