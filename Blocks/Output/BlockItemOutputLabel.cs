using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockItemOutputLabel : SpriteMenuItem
    {
        public string OutputName => _textBox.Text;

        public bool Extended = true;
        public Action<string> RefreshTextCallback;

        private SpriteMenuItem _popButton;
        private TextBoxMenuItem _textBox;
        private TextMenuItem _outputDataTypeLabel;

        public BlockItemOutputLabel(IHasDrawLayer parent,string chipName) : base(parent, "BlankPixel")
        {
            _textBox = _addTextBox(chipName, 0, 0, CoordinateMode.ParentPixelOffset, false);
            _textBox.OnTextChanged += OutputLabelTextTyped;

            _outputDataTypeLabel = new TextMenuItem(_textBox);
            _outputDataTypeLabel.MultiplyScale(0.5f);
            _outputDataTypeLabel.Color = Microsoft.Xna.Framework.Color.Black;
            _outputDataTypeLabel.SetLocationConfig(50, 18, CoordinateMode.ParentPercentageOffset, true);
            _textBox.AddChild(_outputDataTypeLabel);

            _popButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayer - (DrawLayers.MinLayerDistance * 5)), "MinusButton");
            _popButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            _popButton.OnMouseReleased += (i) => PopInOrOut();
            AddChild(_popButton);

            _retract();
        }

        public void PopInOrOut()
        {
            if(Extended)
            {
                _retract();
            }
            else
            {
                _extend();
            }
        }

        private void _extend()
        {
            _popButton.SpriteName = "MinusButton";

            _textBox.Visible = true;
            _textBox.Editable = true;

            Extended = true;
        }

        private void _retract()
        {
            _popButton.SpriteName = "PlusButton";

            _textBox.Visible = false;
            _textBox.Editable = false;
            _textBox.Focused = false;

            Extended = false;
        }

        public void OutputLabelTextTyped(string newText) => RefreshTextCallback(newText);
        public void SetOutputDataTypeLabel(string labeltext) => _outputDataTypeLabel.Text = labeltext;
        public override IntPoint GetBaseSize() => Extended ? _textBox.GetBaseSize() : _popButton.GetBaseSize();
    }
}
