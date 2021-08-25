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
        public static List<BlockInputSection> CreateInputSectionsBasic(BlockTop parent, int numSections)
        {
            var output = new List<BlockInputSection>();

            for(int i=0;i<numSections;i++)
            {
                var section = CreateInputSection(parent, i);
                output.Add(section);
            }

            return output;
        }

        public static List<BlockInputSection> CreateInputSectionsVariableSetter(BlockTopVariableSetter parent)
        {
            var variableAccessSection = new BlockInputSectionVariableAccess(ManualDrawLayer.InFrontOf(parent, 0), "Variable", (o) => parent.VariableDropdownChanged(o));
            variableAccessSection.ColorMask = parent.ColorMask;

            var dynamicTypeSection = new BlockInputSectionDynamicType(ManualDrawLayer.InFrontOf(parent, 1), "Set to");
            dynamicTypeSection.ColorMask = parent.ColorMask;

            return new List<BlockInputSection>() { variableAccessSection, dynamicTypeSection };
        }


        public static BlockInputSection CreateInputSection(BlockTop parent, int sectionIndex)
        {
            var parentDrawLayer = ManualDrawLayer.InFrontOf(parent, sectionIndex);
            var dataType = parent.GetInputTypes(sectionIndex);
            var displayName = parent.GetInputDisplayName(sectionIndex);

            var inputSection = new BlockInputSectionWithDropdown(parentDrawLayer, dataType, displayName);
            inputSection.ColorMask = parent.ColorMask;

            return inputSection;
        }
    }
}
