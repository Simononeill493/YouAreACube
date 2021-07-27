using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockTopWithOutput : BlockTop
    {
        public string OutputName => _outputLabel.OutputName;
        private BlockItemOutputLabel _outputLabel;

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


        public BlockTopWithOutput(string name,IHasDrawLayer parent, BlockData data) : base(name,parent, data)
        {
            _createOutputLabel();

            OutputTypeBase = data.Output;
            OutputTypeCurrent = OutputTypeBase;
        }
        private void _createOutputLabel()
        {
            _outputLabel = new BlockItemOutputLabel(this, Name);
            _outputLabel.SetLocationConfig(100, 0, CoordinateMode.ParentPercentageOffset);
            _outputLabel.RefreshTextCallback = _outputLabelTextChanged;
            AddChild(_outputLabel);
        }

        private void _outputLabelTextChanged(string newText)
        {
            Name = newText;
            BlocksetRefreshText();
        }

        protected override void _inputSectionDropdownChanged(BlockInputSection section,BlockInputDropdown dropdown, BlockInputOption optionSelected)
        {
            base._inputSectionDropdownChanged(section, dropdown, optionSelected);

            OutputTypeCurrent = OutputTypeBase.Replace("Variable", CurrentTypeArguments.First());

            /*if(OutputTypeBase.Contains("Cube"))
            {
                if(optionSelected.BaseType.Equals(nameof(BlockMode)))
                {
                    OutputTypeCurrent = OutputTypeBase.Replace("Cube", optionSelected.ToString() + "Cube");
                }
            }*/

            _topLevelRefreshAll_Delayed();
        }
    }
}
