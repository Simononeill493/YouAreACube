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
            Callbacks.ParentRefreshText();
        }

        protected override void _inputSectionSelectionChanged(BlockInputSection section, BlockInputOption optionSelected)
        {
            base._inputSectionSelectionChanged(section, optionSelected);

            OutputTypeCurrent = OutputTypeBase.Replace("Variable", CurrentTypeArguments.First());
            _topLevelRefreshAll_Delayed();
        }
    }
}
