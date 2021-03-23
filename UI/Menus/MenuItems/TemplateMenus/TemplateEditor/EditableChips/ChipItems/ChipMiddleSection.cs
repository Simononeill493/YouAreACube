﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipMiddleSection : SpriteMenuItem
    {
        public Action<ChipInputDropdown, ChipInputOption> DropdownSelectedCallback;

        private ChipInputDropdown _dropdown;
        private string _inputType;

        public ChipMiddleSection(IHasDrawLayer parent,string inputType) : base(parent, "BlueChipFullMiddle") 
        {
            _inputType = inputType;

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

        public void DropdownItemSelected(ChipInputOption optionSelected)
        {
            DropdownSelectedCallback(_dropdown, optionSelected);
        }

        public void SetConnectionsFromAbove(List<ChipTopSection> chipsAbove)
        {
            _dropdown.ResetToDefaults(_inputType);
            _dropdown.AddItems(_getInputsToAddToDropdown(chipsAbove));
        }

        private List<ChipInputOption> _getInputsToAddToDropdown(List<ChipTopSection> chipsAbove)
        {
            var aboveChipsToAdd = new List<ChipInputOption>();

            foreach (var chipAbove in chipsAbove)
            {
                var data = chipAbove.Chip;
                var (canFeed, generic, baseOutput) = data.CanFeedOutputInto(_inputType);
                if (canFeed)
                {
                    if(generic)
                    {
                        var selectionToAdd = new ChipInputOptionGeneric(chipAbove, baseOutput);
                        aboveChipsToAdd.Add(selectionToAdd);
                    }
                    else
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