using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputSection : SpriteMenuItem
    {
        public Action<ChipInputDropdown, ChipInputOption> DropdownSelectedCallback;

        private ChipInputDropdown _dropdown;
        private string _inputType;

        public ChipInputSection(IHasDrawLayer parent,string inputType,Color color) : base(parent, "ChipFullMiddle") 
        {
            _inputType = inputType;
            ColorMask = color;

            var textItem = new TextMenuItem(this);
            textItem.Text = inputType;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Microsoft.Xna.Framework.Color.White;
            textItem.SetLocationConfig(4, 40, CoordinateMode.ParentPercentageOffset, false);
            AddChild(textItem);

            _dropdown = ChipDropdownFactory.Create(this,_inputType);
            _dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.OnSelectedChanged += DropdownItemSelected;
            AddChild(_dropdown);
        }

        public void DropdownItemSelected(ChipInputOption optionSelected) => DropdownSelectedCallback(_dropdown, optionSelected);


        public void SetConnectionsFromAbove(List<ChipTop> chipsAbove)
        {
            _dropdown.SetItems(_getInputsToAddToDropdown(chipsAbove));
            _dropdown.AddItems(ChipDropdownUtils.GetDefaultItems(_inputType));            
        }

        private List<ChipInputOption> _getInputsToAddToDropdown(List<ChipTop> chipsAbove)
        {
            var aboveChipsToAdd = new List<ChipInputOption>();

            foreach (var chipAbove in chipsAbove)
            {
                var data = chipAbove.Chip;
                var (canFeed, generic, baseOutput) = data.CanFeedOutputInto(_inputType);
                if (canFeed)
                {
                    var standardChip = (ChipTopStandard)chipAbove;

                    if (generic)
                    {
                        var selectionToAdd = new ChipInputOptionGeneric(standardChip, baseOutput);
                        aboveChipsToAdd.Add(selectionToAdd);
                    }
                    else
                    {
                        var selectionToAdd = new ChipInputOptionReference(standardChip);
                        aboveChipsToAdd.Add(selectionToAdd);
                    }
                }
            }

            return aboveChipsToAdd;
        }

        public void RefreshText() => _dropdown.RefreshText();
    }
}
