using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipSwitchButtons : SpriteMenuItem
    {
        private ChipSwitchButton _leftButton;
        private ChipSwitchButton _rightButton;

        public ChipSwitchButtons(IHasDrawLayer parent,Color parentColor) : base(parent, "ChipFullMiddle")
        {
            ColorMask = parentColor;

            _leftButton = new ChipSwitchButton(this, "");
            _leftButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset);
            _leftButton.OnMouseReleased += (i) => _buttonClicked(_leftButton);
            AddChild(_leftButton);

            _rightButton = new ChipSwitchButton(this, "");
            _rightButton.SetLocationConfig(70, 0, CoordinateMode.ParentPixelOffset);
            _rightButton.OnMouseReleased += (i) => _buttonClicked(_rightButton);
            AddChild(_rightButton);
        }

        public void AddItem()
        {

        }

        private void _buttonClicked(ChipSwitchButton button)
        {

        }
    }

    class ChipSwitchButton : TextBoxMenuItem
    {
        public ChipSwitchButton(IHasDrawLayer parentDrawLayer, string initialString) : base(parentDrawLayer, initialString)
        {
            SpriteName = "IfChipSwitchButton";
        }
    }


}
