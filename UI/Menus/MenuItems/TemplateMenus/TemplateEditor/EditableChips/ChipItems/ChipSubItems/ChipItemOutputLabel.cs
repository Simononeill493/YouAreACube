using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipItemOutputLabel : SpriteMenuItem
    {
        public bool Extended = true;

        private SpriteMenuItem _popButton;
        public TextBoxMenuItem TextBox;
        private TextMenuItem _outputDataTypeLabel;

        public void SetOutputDataTypeLabel(string newLabel)
        {
            _outputDataTypeLabel.Text = newLabel;
        }

        public ChipItemOutputLabel(IHasDrawLayer parent,ChipData chip) : base(parent, "BlankPixel")
        {
            TextBox = new TextBoxMenuItem(this, chip.Name + "_out");
            TextBox.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            AddChild(TextBox);

            _outputDataTypeLabel = new TextMenuItem(TextBox);
            _outputDataTypeLabel.Text = chip.OutputType;
            _outputDataTypeLabel.MultiplyScale(0.5f);
            _outputDataTypeLabel.Color = Microsoft.Xna.Framework.Color.Black;
            _outputDataTypeLabel.SetLocationConfig(50, 18, CoordinateMode.ParentPercentageOffset, true);
            TextBox.AddChild(_outputDataTypeLabel);

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

            TextBox.Visible = true;
            TextBox.Editable = true;

            Extended = true;
        }

        private void _retract()
        {
            _popButton.SpriteName = "PlusButton";

            TextBox.Visible = false;
            TextBox.Editable = false;
            TextBox.Focused = false;

            Extended = false;
        }

        public override Point GetBaseSize()
        {
            return Extended ? TextBox.GetBaseSize() : _popButton.GetBaseSize();
        }
    }
}
