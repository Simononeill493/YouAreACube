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
        public static ChipPreviewLargeMiddleSection Create(IHasDrawLayer parentDrawLayer,string dataType)
        {
            var section = new ChipPreviewLargeMiddleSection(parentDrawLayer, dataType);

            var dropdown = new ChipDataDropdown(section, dataType);
            SetDefaultItems(dropdown, dataType);

            if (_isTextEntryType(dataType))
            {
                dropdown.Editable = true;
            }

            dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            dropdown.BaseType = _getType(dataType);
            section.AddChild(dropdown);

            section.Dropdown = dropdown;

            return section;
        }

        public static void SetDefaultItems(ChipDataDropdown dropdown, string dataType)
        {
            if (_isDiscreteType(dataType))
            {
                dropdown.SetItems(ChipInputPinDropdownSelectionBase.GetBasicSelections(dataType).Cast<ChipInputPinDropdownSelection>().ToList());
            }
            else if (dataType.Equals("Template"))
            {
                var templates = ChipInputPinDropdownSelectionBase.Create(Templates.BlockTemplates.Values.ToList());
                dropdown.SetItems(templates.Cast<ChipInputPinDropdownSelection>().ToList());
            }
            else
            {
                dropdown.SetItems(new List<ChipInputPinDropdownSelection>());
            }
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
            if (dataType.Equals("Tile")) { return typeof(Tile); }
            if (dataType.Equals("List<Variable>")) { return typeof(List<object>); }
            if (dataType.Equals("Keys")) { return typeof(Keys); }
            if (dataType.Equals("SurfaceBlock")) { return typeof(SurfaceBlock); }
            if (dataType.Equals("GroundBlock")) { return typeof(GroundBlock); }
            if (dataType.Equals("EphemeralBlock")) { return typeof(EphemeralBlock); }

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
