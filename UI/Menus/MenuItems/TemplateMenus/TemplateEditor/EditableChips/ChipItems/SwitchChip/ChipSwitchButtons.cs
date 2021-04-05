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
        private ChipSwitchButton _selectedButton;
        private ChipSwitchButton _leftButton;
        private ChipSwitchButton _rightButton;

        private int _indexOffset = 0;
        private List<string> _switchSections;

        private Action _switchButtonClearedCallback;
        private Action<string> _switchButtonSetCallback;

        public ChipSwitchButtons(IHasDrawLayer parent,Color parentColor,Action switchButtonClearedCallback,Action<string> switchButtonSetCallback) : base(parent, "ChipFullEnd")
        {
            _switchButtonClearedCallback = switchButtonClearedCallback;
            _switchButtonSetCallback = switchButtonSetCallback;

            ColorMask = parentColor;
            _switchSections = new List<string>();

            _leftButton = new ChipSwitchButton(this, "",0);
            _leftButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset);
            _leftButton.OnMouseReleased += (i) => _buttonClicked(_leftButton);
            AddChild(_leftButton);

            _rightButton = new ChipSwitchButton(this, "",1);
            _rightButton.SetLocationConfig(70, 0, CoordinateMode.ParentPixelOffset);
            _rightButton.OnMouseReleased += (i) => _buttonClicked(_rightButton);
            AddChild(_rightButton);
        }

        public void AddSwitchSection(string name) => _switchSections.Add(name);

        public void UpdateButtonText()
        {
            var leftButtonText = (_indexOffset < _switchSections.Count()) ? _switchSections[_indexOffset] : "";
            _leftButton.SetText(leftButtonText);

            var rightButtontext = (_indexOffset+1 < _switchSections.Count()) ? _switchSections[_indexOffset+1] : "";
            _rightButton.SetText(rightButtontext);
        }

        private void _buttonClicked(ChipSwitchButton button)
        {
            _leftButton.ColorMask = _rightButton.ColorMask = Color.White;
            if(button!=_selectedButton)
            {
                if (button.Text.Equals("")) { return; }

                button.ColorMask = Color.LightGreen;
                _selectedButton = button;
                _switchButtonSetCallback(button.Text);
            }
            else
            {
                _selectedButton = null;
                _switchButtonClearedCallback();
            }
        }
    }

    class ChipSwitchButton : TextBoxMenuItem
    {
        public int ButtonIndex;

        public ChipSwitchButton(IHasDrawLayer parentDrawLayer, string initialString,int buttonIndex) : base(parentDrawLayer, initialString)
        {
            SpriteName = "IfChipSwitchButton";
            ButtonIndex = buttonIndex;
            Editable = false;
        }
    }
}
