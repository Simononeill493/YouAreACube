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
        public string InputBaseType;
        public Action<ChipInputSection, ChipInputDropdown, ChipInputOption> DropdownSelectedCallback;

        private ChipInputDropdown _dropdown;

        public ChipInputSection(IHasDrawLayer parent,string inputType,Color color) : base(parent, "ChipFullMiddle") 
        {
            InputBaseType = inputType;
            ColorMask = color;

            var textItem = new TextMenuItem(this);
            textItem.Text = inputType;
            textItem.MultiplyScale(0.5f);
            textItem.Color = Color.White;
            textItem.SetLocationConfig(4, 40, CoordinateMode.ParentPercentageOffset, false);
            AddChild(textItem);

            _dropdown = ChipDropdownFactory.Create(this,InputBaseType);
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
            _dropdown.AddItems(ChipDropdownUtils.GetDefaultItems(InputBaseType));            
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

            if(InputBaseType.Equals(chipAboveOutput))
            {
                return true;
            }
            else if(InputBaseType.Equals("Variable"))
            {
                return true;
            }
            else if(chipAboveOutput.StartsWith("List<") & InputBaseType.Equals("List<Variable>"))
            {
                return true;
            }

            return false; 
        }

        public void RefreshText() => _dropdown.RefreshText();
    }
}
