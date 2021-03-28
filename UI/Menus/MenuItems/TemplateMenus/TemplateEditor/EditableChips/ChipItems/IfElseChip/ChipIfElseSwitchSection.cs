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
        public IfElseChipExtensionMode CurrentMode = IfElseChipExtensionMode.None;

        private TextBoxMenuItem _yesButton;
        private TextBoxMenuItem _noButton;

        private Action<IfElseChipExtensionMode> _switchChangedCallback;

        public ChipIfElseSwitchSection(IHasDrawLayer parent,Color parentColor, Action<IfElseChipExtensionMode> switchChangedCallback) : base(parent, "ChipFullMiddle")
        {
            ColorMask = parentColor;

            _yesButton = new TextBoxMenuItem(this, "Yes") {SpriteName= "IfChipSwitchButton" };
            _yesButton.SetLocationConfig(0, 1, CoordinateMode.ParentPixelOffset);
            _yesButton.OnMouseReleased += (i) => _yesButtonClicked();
            AddChild(_yesButton);

            _noButton = new TextBoxMenuItem(this, "No") { SpriteName = "IfChipSwitchButton" };
            _noButton.SetLocationConfig(80, 1, CoordinateMode.ParentPixelOffset);
            _noButton.OnMouseReleased += (i) => _noButtonClicked();
            AddChild(_noButton);

            _switchChangedCallback = switchChangedCallback;
        }

        private void _yesButtonClicked()
        {
            switch (CurrentMode)
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
            switch (CurrentMode)
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

        private void _switchMode(IfElseChipExtensionMode newMode)
        {
            switch (newMode)
            {
                case IfElseChipExtensionMode.None:
                    _yesButton.ColorMask = Color.White;
                    _noButton.ColorMask = Color.White;
                    break;
                case IfElseChipExtensionMode.No:
                    _yesButton.ColorMask = Color.White;
                    _noButton.ColorMask = Color.LightGreen;
                    break;
                case IfElseChipExtensionMode.Yes:
                    _yesButton.ColorMask = Color.LightGreen;
                    _noButton.ColorMask = Color.White;
                    break;
            }
            _switchChangedCallback(newMode);
            CurrentMode = newMode;
        }
    }

    public enum IfElseChipExtensionMode
    {
        None,
        Yes,
        No
    }
}
