using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class ChipDatabase
    {
        public static List<ChipData> BuiltInChips;

        public static void Load()
        {
            BuiltInChips = new List<ChipData>();
        }

        public class ChipData
        {
            public string Name;

            public override string ToString()
            {
                return Name;
            }
        }

    }
}
