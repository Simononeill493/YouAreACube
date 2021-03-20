using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipPreviewLargeMiddleSection : SpriteMenuItem
    {
        public string InputType;
        public ChipDataDropdown Dropdown;

        public ChipPreviewLargeMiddleSection(IHasDrawLayer parent,string inputType) : base(parent, "BlueChipFullMiddle") 
        {
            InputType = inputType;

            var textItem = new TextMenuItem(this);
            textItem.Text = inputType;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Microsoft.Xna.Framework.Color.White;
            textItem.SetLocationConfig(4, 40, CoordinateMode.ParentPercentageOffset, false);

            AddChild(textItem);
        }

        public void AddConnectionsFromAbove(List<ChipPreviewLarge> chipsAbove)
        {
            Dropdown.ResetToDefaults();
            var aboveChipsToAdd = new List<ChipInputPinDropdownSelection>();

            foreach (var chipAbove in chipsAbove)
            {
                var data = chipAbove.Chip;
                if (data.OutputType != null)
                {
                    if (Dropdown.DataType.Equals(data.OutputType))
                    {
                        var selectionToAdd = new ChipInputPinDropdownSelectionChipReference(chipAbove);
                        aboveChipsToAdd.Add(selectionToAdd);
                    }
                }
            }

            Dropdown.AddItems(aboveChipsToAdd);
        }

        public void RefreshText() => Dropdown.RefreshText();
    }
}
