using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipSectionFactory
    {
        public static ChipInputSection Create(ChipTop parent, int sectionIndex)
        {
            var parentDrawLayer = ManualDrawLayer.InFrontOf(parent, sectionIndex);
            var dataType = parent.Chip.GetInputType(sectionIndex+1);

            var section = new ChipInputSection(parentDrawLayer, dataType, parent.ColorMask);
            return section;
        }
    }
}
