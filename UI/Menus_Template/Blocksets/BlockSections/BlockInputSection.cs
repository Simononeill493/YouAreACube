using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockInputSection : SpriteMenuItem
    {
        public List<string> InputBaseTypes;
        public Action<BlockInputSection, BlockInputDropdown, BlockInputOption> DropdownSelectedCallback;
        public BlockInputOption CurrentlySelected => _dropdown.SelectedItem;

        private BlockInputDropdown _dropdown;

        public BlockInputSection(IHasDrawLayer parent,List<string> inputTypes,string inputDisplayName) : base(parent, "ChipFullMiddle") 
        {
            InputBaseTypes = inputTypes;

            var textItem = _addTextItem(inputDisplayName, 4, 40, CoordinateMode.ParentPercentageOffset, false);
            textItem.MultiplyScale(0.5f);
            textItem.Color = Color.White;

            _dropdown = BlockDropdownFactory.Create(this, inputTypes);
            _dropdown.SetLocationConfig(74, 50, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.OnSelectedChanged += DropdownItemSelected;
            AddChild(_dropdown);
        }

        public void ManuallySetDropdown(BlockInputOption option)
        {
            _dropdown.ManuallySetItem(option);
            DropdownItemSelected(option);
        }

        public void DropdownItemSelected(BlockInputOption optionSelected) => DropdownSelectedCallback(this,_dropdown, optionSelected);

        public void SetConnectionsFromAbove(List<BlockTop> chipsAbove)
        {
            _dropdown.SetItems(_getValidInputsFromAbove(chipsAbove));
            foreach(var typeName in InputBaseTypes)
            {
                _dropdown.AddItems(ChipDropdownUtils.GetDefaultItems(typeName));
            }
        }

        private List<BlockInputOption> _getValidInputsFromAbove(List<BlockTop> chipsAbove)
        {
            var output = new List<BlockInputOption>();
            foreach (var chip in TemplateEditUtils.GetChipsWithOutput(chipsAbove))
            {
                if(_isValidInput(chip))
                {
                    output.Add(new BlockInputOptionReference(chip));
                }
            }

            return output;
        }

        private bool _isValidInput(BlockTopWithOutput chipAbove)
        {
            var chipAboveOutput = chipAbove.OutputTypeCurrent;
            foreach(var inputType in InputBaseTypes)
            {
                if(TemplateEditUtils.IsValidInputFor(chipAboveOutput,inputType))
                {
                    return true;
                }
            }

            return false;
        }

        public void RefreshText() => _dropdown.RefreshText();
    }
}
