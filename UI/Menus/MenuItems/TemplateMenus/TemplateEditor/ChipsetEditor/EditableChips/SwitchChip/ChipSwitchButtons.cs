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
        public List<string> SwitchSectionsNames;

        private Action _switchSectionClosedCallback;
        private Action<int> _switchSectionOpenedCallback;

        public ChipSwitchButtons(IHasDrawLayer parent,Color parentColor,Action switchSectionClosedCallback,Action<int> switchSectionOpenedCallback) : base(parent, "ChipFullEnd")
        {
            _switchSectionClosedCallback = switchSectionClosedCallback;
            _switchSectionOpenedCallback = switchSectionOpenedCallback;

            ColorMask = parentColor;
            SwitchSectionsNames = new List<string>();

            _leftButton = new ChipSwitchButton(this, "",0);
            _leftButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset);
            _leftButton.OnMouseReleased += (i) => _buttonClicked(_leftButton);
            AddChild(_leftButton);

            _rightButton = new ChipSwitchButton(this, "",1);
            _rightButton.SetLocationConfig(70, 0, CoordinateMode.ParentPixelOffset);
            _rightButton.OnMouseReleased += (i) => _buttonClicked(_rightButton);
            AddChild(_rightButton);

            _addArrowButtons();
        }

        private void _addArrowButtons()
        {
            var switchArrowButtonLeft = new SpriteMenuItem(this, "SwitchChipSideArrow");
            switchArrowButtonLeft.SetLocationConfig(140, 0, CoordinateMode.ParentPixelOffset);
            switchArrowButtonLeft.OnMouseReleased += (i) => { UpdateButtonOffset(-1); };
            AddChild(switchArrowButtonLeft);

            var switchArrowButtonRight = new SpriteMenuItem(this, "SwitchChipSideArrow");
            switchArrowButtonRight.FlipHorizontal = true;
            switchArrowButtonRight.SetLocationConfig(140 + switchArrowButtonLeft.GetBaseSize().X, 0, CoordinateMode.ParentPixelOffset);
            switchArrowButtonRight.OnMouseReleased += (i) => { UpdateButtonOffset(1); };
            AddChild(switchArrowButtonRight);
        }

        public void UpdateButtonOffset(int offsetChange)
        {
            var newOffset = _indexOffset + offsetChange;
            if(newOffset < 0 | newOffset>SwitchSectionsNames.Count-2)
            {
                return;
            }

            var prevSelectedButton = _selectedButton;

            _indexOffset = newOffset;
            _closeSwitchSection();

            if(prevSelectedButton!=null)
            {
                _openSwitchSection(prevSelectedButton);
            }
        }

        public void AddSwitchSection(string name) => SwitchSectionsNames.Add(name);

        public void UpdateButtonText()
        {
            var leftButtonText = (_indexOffset < SwitchSectionsNames.Count()) ? SwitchSectionsNames[_indexOffset] : "";
            _leftButton.SetText(leftButtonText);

            var rightButtontext = (_indexOffset+1 < SwitchSectionsNames.Count()) ? SwitchSectionsNames[_indexOffset+1] : "";
            _rightButton.SetText(rightButtontext);
        }

        private void _buttonClicked(ChipSwitchButton button)
        {
            _leftButton.ColorMask = _rightButton.ColorMask = Color.White;
            if(button!=_selectedButton)
            {
                _openSwitchSection(button);
            }
            else
            {
                _closeSwitchSection();
            }
        }

        private void _closeSwitchSection()
        {
            _selectedButton = null;
            _switchSectionClosedCallback();
        }

        private void _openSwitchSection(ChipSwitchButton button)
        {
            if (button.Text.Equals("")) { return; }

            button.ColorMask = Color.LightGreen;
            _selectedButton = button;
            _switchSectionOpenedCallback(button.ButtonIndex + _indexOffset);
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
