using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipMiddleSection : SpriteMenuItem
    {
        private ChipInputDropdown _dropdown;

        public ChipMiddleSection(IHasDrawLayer parent,string inputType) : base(parent, "BlueChipFullMiddle") 
        {
            var textItem = new TextMenuItem(this);
            textItem.Text = inputType;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Microsoft.Xna.Framework.Color.White;
            textItem.SetLocationConfig(4, 40, CoordinateMode.ParentPercentageOffset, false);
            AddChild(textItem);

            _dropdown = ChipDropdownFactory.Create(this, inputType);
            _dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_dropdown);
        }

        public void SetConnectionsFromAbove(List<ChipTopSection> chipsAbove)
        {
            _dropdown.ResetToDefaults();
            _dropdown.AddItems(_getInputsToAddToDropdown(chipsAbove));
        }

        private List<ChipInputOption> _getInputsToAddToDropdown(List<ChipTopSection> chipsAbove)
        {
            var aboveChipsToAdd = new List<ChipInputOption>();

            foreach (var chipAbove in chipsAbove)
            {
                var data = chipAbove.Chip;
                if (data.OutputType != null)
                {
                    if (_dropdown.DataType.Equals(data.OutputType))
                    {
                        var selectionToAdd = new ChipInputOptionReference(chipAbove);
                        aboveChipsToAdd.Add(selectionToAdd);
                    }
                }
            }

            return aboveChipsToAdd;
        }

        public void RefreshText() => _dropdown.RefreshText();
    }
}
