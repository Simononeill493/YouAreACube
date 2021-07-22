using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipsetComparer
    {
        public static bool Equivalent(Chipset b1, Chipset b2)
        {
            if (!b1.Name.Equals(b2.Name)) { return false; }
            if (b1.Chips.Count != b2.Chips.Count) { return false; }
            if (b1.ControlChips.Count != b2.ControlChips.Count) { return false; }

            for (int i = 0; i < b1.Chips.Count; i++)
            {
                if (!b1.Chips[i].Name.Equals(b2.Chips[i].Name))
                {
                    return false;
                }
            }

            var b1SubBlocks = b1.GetSubBlocks();
            var b2SubBlocks = b2.GetSubBlocks();

            if (b1SubBlocks.Count != b2SubBlocks.Count) { return false; }

            for (int i = 0; i < b1SubBlocks.Count; i++)
            {
                if (!Equivalent(b1SubBlocks[i],b2SubBlocks[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
