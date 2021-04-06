using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipTopWithOutput : ChipTop
    {
        public string OutputName => _outputLabel.OutputName;
        private ChipItemOutputLabel _outputLabel;

        public string OutputTypeCurrent 
        { 
            get 
            { 
                return _outputTypeCurrent; 
            } 
            set 
            { 
                _outputTypeCurrent = value;
                _outputLabel.SetOutputDataTypeLabel(_outputTypeCurrent);
            } 
        }
        private string _outputTypeCurrent;
        public string OutputTypeBase;

        public ChipTopWithOutput(string name,IHasDrawLayer parent, ChipData data) : base(name,parent, data)
        {
            _createOutputLabel();

            OutputTypeBase = data.OutputType;
            OutputTypeCurrent = OutputTypeBase;
        }
        private void _createOutputLabel()
        {
            _outputLabel = new ChipItemOutputLabel(this, Name);
            _outputLabel.SetLocationConfig(100, 0, CoordinateMode.ParentPercentageOffset);
            _outputLabel.RefreshTextCallback = _outputLabelTextChanged;
            AddChild(_outputLabel);
        }

        private void _outputLabelTextChanged(string newText)
        {
            Name = newText;
            ChipsetRefreshText();
        }

        protected override void _inputSectionDropdownChanged(ChipInputSection section,ChipInputDropdown dropdown, ChipInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Reference)
            {
                var referenceOption = (ChipInputOptionReference)optionSelected;

                var inputBase = section.InputBaseType;
                var dataTypeFeedingIn = referenceOption.ChipReference.OutputTypeCurrent;

                if (inputBase.Equals("List<Variable>"))
                {
                    var afterOpeningList = dataTypeFeedingIn.Substring(5);
                    var extracted = afterOpeningList.Substring(0, afterOpeningList.Length - 1);

                    OutputTypeCurrent = OutputTypeBase.Replace("Variable", extracted);
                }
                else if(inputBase.Equals("Variable"))
                {
                    OutputTypeCurrent = OutputTypeBase.Replace("Variable", dataTypeFeedingIn);
                }
            }

            _topLevelRefreshAll_Delayed();
        }

    }
}
