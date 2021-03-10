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

            if(_isDiscreteType(dataType))
            {
                _addSetDropdown(section, dataType);
            }

            if (_isTextEntryType(dataType))
            {
                _addEditableDropdown(section, dataType);
            }

            return section;
        }

        private static void _addEditableDropdown(ChipPreviewLargeMiddleSection section, string dataType)
        {
            var dropdown = new ChipDataDropdown(section);

            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.Editable = true;
            dropdown.BaseType = _getType(dataType);

            section.AddChild(dropdown);
        }

        private static void _addSetDropdown(ChipPreviewLargeMiddleSection section, string dataType)
        {
            var dropdown = new ChipDataDropdown(section);

            dropdown.AddItems(ChipInputPinDropdownSelectionBase.GetBasicSelections(dataType).Cast<ChipInputPinDropdownSelection>().ToList());
            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.BaseType = _getType(dataType);

            section.AddChild(dropdown);
        }

        private static Type _getType(string dataType)
        {
            if (dataType.Equals("CardinalDirection")) { return typeof(CardinalDirection); }
            if (dataType.Equals("RelativeDirection")) { return typeof(RelativeDirection); }
            if (dataType.Equals("BlockType")) { return typeof(BlockType); }
            if (dataType.Equals("bool")) { return typeof(bool); }
            if (dataType.Equals("int")) { return typeof(int); }
            if (dataType.Equals("string")) { return typeof(string); }
            if (dataType.Equals("Template")) { return typeof(BlockTemplate); }
            throw new Exception();
        }
        private static bool _isDiscreteType(string dataType)
        {
            return dataType.Equals("CardinalDirection") | dataType.Equals("RelativeDirection") | dataType.Equals("BlockType") | dataType.Equals("bool");
        }
        private static bool _isTextEntryType(string dataType)
        {
            return dataType.Equals("int") | dataType.Equals("string");
        }
    }
}
