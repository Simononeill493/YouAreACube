using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockSectionFactory
    {
        public static BlockInputSection CreateInputSection(BlockTop parent, int sectionIndex)
        {
            var parentDrawLayer = ManualDrawLayer.InFrontOf(parent, sectionIndex);
            var dataType = parent.BlockData.GetInputTypes(sectionIndex);
            var displayName = parent.BlockData.GetInputDisplayName(sectionIndex);

            var inputSection = new BlockInputSectionWithDropdown(parentDrawLayer, dataType, displayName);
            inputSection.ColorMask = parent.ColorMask;

            return inputSection;
        }
    }
}
