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

        public ChipTopStandard(IHasDrawLayer parent, ChipData data) : base(parent, data)
        {
            _tryCreateOutputLabel();
            _createInputSections(Chip);

            if (_inputSections.Count > 0)
            {
                _inputSections[_inputSections.Count - 1].SpriteName = "ChipFullEnd";
            }
        }
        private void _tryCreateOutputLabel()
        {
            if (Chip.OutputType != null)
            {
                _outputLabel = new ChipItemOutputLabel(this, Chip);
                _outputLabel.SetLocationConfig(100, 0, CoordinateMode.ParentPercentageOffset);
                _outputLabel.TextBox.OnTextChanged += _outputNameChanged;
                AddChild(_outputLabel);
            }
        }

        protected override void _inputSectionDropdownChanged(ChipInputDropdown dropdown,ChipInputOption optionSelected)
        {
            base._inputSectionDropdownChanged(dropdown, optionSelected);
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

        private void _outputNameChanged(string newName) => ChipsetRefreshTextCallback.Invoke();

    }
}
