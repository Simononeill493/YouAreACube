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
        private IfElseChipExtensionMode Mode = IfElseChipExtensionMode.None;

        public ChipIfElseSwitchSection(IHasDrawLayer parent,Color parentColor) : base(parent, "ChipFullMiddle")
        {
            ColorMask = parentColor;

            var yesButton = new TextBoxMenuItem(this, "Yes") {SpriteName= "IfChipSwitchButton" };
            yesButton.SetLocationConfig(0, 1, CoordinateMode.ParentPixelOffset);
            yesButton.OnMouseReleased += (i) => _yesButtonClicked();
            AddChild(yesButton);

            var noButton = new TextBoxMenuItem(this, "No") { SpriteName = "IfChipSwitchButton" };
            noButton.SetLocationConfig(80, 1, CoordinateMode.ParentPixelOffset);
            noButton.OnMouseReleased += (i) => _noButtonClicked();

            AddChild(noButton);
        }

        private void _yesButtonClicked()
        {
            switch (Mode)
            {
                case IfElseChipExtensionMode.None:
                case IfElseChipExtensionMode.No:
                    _switchMode(IfElseChipExtensionMode.Yes);
                    break;
                case IfElseChipExtensionMode.Yes:
                    _switchMode(IfElseChipExtensionMode.None);
                    break;
            }
        }
        private void _noButtonClicked()
        {
            switch (Mode)
            {
                case IfElseChipExtensionMode.None:
                case IfElseChipExtensionMode.Yes:
                    _switchMode(IfElseChipExtensionMode.No);
                    break;
                case IfElseChipExtensionMode.No:
                    _switchMode(IfElseChipExtensionMode.None);
                    break;
            }
        }

        private void _switchMode(IfElseChipExtensionMode mode)
        {
            Mode = mode;
        }
    }

    public enum IfElseChipExtensionMode
    {
        None,
        Yes,
        No
    }
}
