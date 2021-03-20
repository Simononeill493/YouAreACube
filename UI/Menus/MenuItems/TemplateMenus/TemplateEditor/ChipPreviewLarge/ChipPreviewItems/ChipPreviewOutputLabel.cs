using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipPreviewOutputLabel : SpriteMenuItem
    {
        public bool Extended = true;

        private SpriteMenuItem _popButton;
        private TextBoxMenuItem _textBox;
        private TextMenuItem _outputDataTypeLabel;

        public ChipPreviewOutputLabel(IHasDrawLayer parent,ChipData chip) : base(parent, "BlankPixel")
        {
            _textBox = new TextBoxMenuItem(this, chip.Name + "_out");
            _textBox.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            AddChild(_textBox);

            _outputDataTypeLabel = new TextMenuItem(_textBox);
            _outputDataTypeLabel.Text = chip.Output;
            _outputDataTypeLabel.MultiplyScale(0.5f);
            _outputDataTypeLabel.Color = Microsoft.Xna.Framework.Color.Black;
            _outputDataTypeLabel.SetLocationConfig(50, 15, CoordinateMode.ParentPercentageOffset, true);
            //_outputDataTypeLabel.SetLocationConfig(0, 0, CoordinateMode.ParentPercentageOffset, false);
            _textBox.AddChild(_outputDataTypeLabel);

            _popButton = new SpriteMenuItem(ManualDrawLayer.Create(DrawLayer - (DrawLayers.MinLayerDistance * 5)), "MinusButton");
            _popButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            _popButton.OnMouseReleased += (i) => PopInOrOut();
            AddChild(_popButton);

            _textBox.Editable = true;
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
    }
}
