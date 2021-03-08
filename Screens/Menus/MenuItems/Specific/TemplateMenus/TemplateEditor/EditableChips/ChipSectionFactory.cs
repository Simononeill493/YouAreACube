using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipSectionFactory
    {
        public static ChipPreviewLargeMiddleSection Create(IHasDrawLayer parentDrawLayer,string dataType)
        {
            var section = new ChipPreviewLargeMiddleSection(parentDrawLayer, dataType);

            if(_isDropdownType(dataType))
            {
                _addDropdown(section, dataType);
            }

            return section;
        }

        private static bool _isDropdownType(string dataType)
        {
            return dataType.Equals("CardinalDirection") | dataType.Equals("RelativeDirection") | dataType.Equals("BlockType");
        }

        private static void _addDropdown(ChipPreviewLargeMiddleSection section, string dataType)
        {
            MenuItem dropdown = null;

            if(dataType.Equals("CardinalDirection"))
            {
                dropdown = new DropdownMenuItem<CardinalDirection>(section);
                ((DropdownMenuItem<CardinalDirection>)dropdown).SetItems(typeof(CardinalDirection).GetEnumValues().Cast<CardinalDirection>().ToList());
            }
            else if (dataType.Equals("RelativeDirection"))
            {
                dropdown = new DropdownMenuItem<RelativeDirection>(section);
                ((DropdownMenuItem<RelativeDirection>)dropdown).SetItems(typeof(RelativeDirection).GetEnumValues().Cast<RelativeDirection>().ToList());

            }
            else if (dataType.Equals("BlockType"))
            {
                dropdown = new DropdownMenuItem<BlockType>(section);
                ((DropdownMenuItem<BlockType>)dropdown).SetItems(typeof(BlockType).GetEnumValues().Cast<BlockType>().ToList());
            }

            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            section.AddChild(dropdown);
        }
    }
}
