using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipIfElseSwitchSection : SpriteMenuItem
    {
        private TextBoxMenuItem _yesButton;
        private TextBoxMenuItem _noButton;

        private Action _switchChangedCallback;

        public ChipIfElseSwitchSection(IHasDrawLayer parent,Color parentColor, Action switchChangedCallback) : base(parent, "ChipFullMiddle")
        {
            ColorMask = parentColor;

            _yesButton = new TextBoxMenuItem(this, "Yes") {SpriteName= "IfChipSwitchButton" };
            _yesButton.SetLocationConfig(0, 1, CoordinateMode.ParentPixelOffset);
            _yesButton.OnMouseReleased += _yesButton_OnMouseReleased;
            AddChild(_yesButton);

            _noButton = new TextBoxMenuItem(this, "No") { SpriteName = "IfChipSwitchButton" };
            _noButton.SetLocationConfig(80, 1, CoordinateMode.ParentPixelOffset);
            _noButton.OnMouseReleased += _noButton_OnMouseReleased;
            AddChild(_noButton);

            _switchChangedCallback = switchChangedCallback;
        }

        private void _noButton_OnMouseReleased(UserInput obj)
        {

        }

        private void _yesButton_OnMouseReleased(UserInput obj)
        {

        }
    }

}
