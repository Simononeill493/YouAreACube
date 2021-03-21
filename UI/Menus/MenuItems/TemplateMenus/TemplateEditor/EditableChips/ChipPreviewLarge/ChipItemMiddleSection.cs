using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipItemMiddleSection : SpriteMenuItem
    {
        public string InputType;
        public ChipInputDropdown Dropdown;

        public ChipItemMiddleSection(IHasDrawLayer parent,string inputType) : base(parent, "BlueChipFullMiddle") 
        {
            InputType = inputType;

            var textItem = new TextMenuItem(this);
            textItem.Text = inputType;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Microsoft.Xna.Framework.Color.White;
            textItem.SetLocationConfig(4, 40, CoordinateMode.ParentPercentageOffset, false);

            AddChild(textItem);
        }

        public void AddConnectionsFromAbove(List<ChipItem> chipsAbove)
        {
            Dropdown.ResetToDefaults();
            var aboveChipsToAdd = new List<ChipInputOption>();

            foreach (var chipAbove in chipsAbove)
            {
                var data = chipAbove.Chip;
                if (data.OutputType != null)
                {
                    if (Dropdown.DataType.Equals(data.OutputType))
                    {
                        var selectionToAdd = new ChipInputOptionReference(chipAbove);
                        aboveChipsToAdd.Add(selectionToAdd);
                    }
                }
            }

            Dropdown.AddItems(aboveChipsToAdd);
        }

        public void RefreshText() => Dropdown.RefreshText();
    }
}
