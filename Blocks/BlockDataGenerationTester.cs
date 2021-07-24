using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockDataGenerationTester
    {
        public static void Test()
        {
            foreach (var data in BlockDataDatabase.GraphicalChips.Values)
            {
                data.GenerateChip();
            }
        }
    }
}
