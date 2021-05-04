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
        public static ChipInputSection CreateInputSection(ChipTop parent, int sectionIndex)
        {
            var parentDrawLayer = ManualDrawLayer.InFrontOf(parent, sectionIndex);
            var dataType = parent.ChipData.GetInputTypes(sectionIndex);
            var displayName = parent.ChipData.GetInputDisplayName(sectionIndex);

            var inputSection = new ChipInputSection(parentDrawLayer, dataType, displayName);
            inputSection.ColorMask = parent.ColorMask;

            return inputSection;
        }
    }
}
