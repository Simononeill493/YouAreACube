using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipTopStandard : ChipTop
    {
        public string OutputName => _outputLabel.TextBox.Text;

        private ChipItemOutputLabel _outputLabel;
        private List<ChipMiddleSection> _inputSections = new List<ChipMiddleSection>();

        public ChipTopStandard(IHasDrawLayer parent, ChipData data) : base(parent, data)
        {
            _tryCreateOutputLabel();
            _createInputSections(Chip);
        }
        private void _tryCreateOutputLabel()
        {
            if (Chip.OutputType != null)
            {
                _outputLabel = new ChipItemOutputLabel(this, Chip);
                _outputLabel.SetLocationConfig(100, 0, CoordinateMode.ParentPercentageOffset);
                _outputLabel.TextBox.OnTextChanged += OutputNameChanged;
                AddChild(_outputLabel);
            }
        }
        private void _createInputSections(ChipData chip)
        {
            for(int i=1;i<chip.NumInputs+1;i++)
            {
                var section = ChipSectionFactory.Create(this,i);
                section.DropdownSelectedCallback = _middleSectionDropdownChanged;

                _addSection(section);
                _inputSections.Add(section);
            }

            _updateChildDimensions();
        }

        public override void SetConnectionsFromAbove(List<ChipTop> chipsAbove) =>_inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));
        public void _middleSectionDropdownChanged(ChipInputDropdown dropdown,ChipInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Generic)
            {
                var genericOption = (ChipInputOptionGeneric)optionSelected;
                Chip.SetOutputTypeFromGeneric(genericOption.BaseOutput);
            }
            else if(optionSelected.OptionType == InputOptionType.Base)
            {
                Chip.ResetOutputType();
            }

            _outputLabel?.SetOutputDataTypeLabel(Chip.OutputTypeCurrent);
        }

        public override Point GetFullBaseSize()
        {
            var size = base.GetFullBaseSize();

            if(_outputLabel!=null)
            {
                size.X += _outputLabel.GetBaseSize().X;
            }

            return size;
        }

        public override void RefreshText() => _inputSections.ForEach(m => m.RefreshText());
        private void OutputNameChanged(string newName) => OutputTextChangedCallback.Invoke();

        public override bool IsMouseOverAnySection()
        {
            if (MouseHovering)
            {
                return true;
            }

            foreach (var section in _inputSections)
            {
                if (section.MouseHovering)
                {
                    return true;
                }
            }

            return false;
        }
        public override bool IsMouseOverBottomSection()
        {
            if (_inputSections.Count == 0)
            {
                return true;
            }

            return _inputSections.Last().MouseHovering;
        }
    }
}
