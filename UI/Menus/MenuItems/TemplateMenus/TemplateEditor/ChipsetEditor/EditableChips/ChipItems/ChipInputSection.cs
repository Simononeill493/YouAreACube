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
        public List<string> InputBaseTypes;
        public Action<ChipInputSection, ChipInputDropdown, ChipInputOption> DropdownSelectedCallback;
        public ChipInputOption CurrentlySelected => _dropdown.SelectedItem;

        private ChipInputDropdown _dropdown;

        public ChipInputSection(IHasDrawLayer parent,List<string> inputTypes,string inputDisplayName) : base(parent, "ChipFullMiddle") 
        {
            InputBaseTypes = inputTypes;

            var textItem = _addTextItem(inputDisplayName, 4, 40, CoordinateMode.ParentPercentageOffset, false);
            textItem.MultiplyScale(0.5f);
            textItem.Color = Color.White;

            _dropdown = ChipDropdownFactory.Create(this, inputTypes);
            _dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.OnSelectedChanged += DropdownItemSelected;
            AddChild(_dropdown);
        }

        public void ManuallySetDropdown(ChipInputOption option)
        {
            _dropdown.ManuallySetItem(option);
            DropdownItemSelected(option);
        }

        public void DropdownItemSelected(ChipInputOption optionSelected) => DropdownSelectedCallback(this,_dropdown, optionSelected);

        public void SetConnectionsFromAbove(List<ChipTop> chipsAbove)
        {
            _dropdown.SetItems(_getValidInputsFromAbove(chipsAbove));
            foreach(var typeName in InputBaseTypes)
            {
                _dropdown.AddItems(ChipDropdownUtils.GetDefaultItems(typeName));
            }
        }

        private List<ChipInputOption> _getValidInputsFromAbove(List<ChipTop> chipsAbove)
        {
            var output = new List<ChipInputOption>();
            foreach (var chip in chipsAbove.Where(c=>c.HasOutput).Cast<ChipTopWithOutput>())
            {
                if(_isValidInput(chip))
                {
                    output.Add(new ChipInputOptionReference(chip));
                }
            }

            return output;
        }

        private bool _isValidInput(ChipTopWithOutput chipAbove)
        {
            var chipAboveOutput = chipAbove.OutputTypeCurrent;
            foreach(var inputType in InputBaseTypes)
            {
                if(TypeUtils.IsValidInputFor(chipAboveOutput,inputType))
                {
                    return true;
                }
            }

            return false;
        }

        public void RefreshText() => _dropdown.RefreshText();
    }
}
