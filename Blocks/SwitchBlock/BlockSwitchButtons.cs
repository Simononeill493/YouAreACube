using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockSwitchButtons : SpriteMenuItem
    {
        private BlockSwitchButton _selectedButton;
        private BlockSwitchButton _leftButton;
        private BlockSwitchButton _rightButton;

        private int _indexOffset = 0;
        public List<string> SwitchSectionsNames;

        private Action _switchSectionClosedCallback;
        private Action<int> _switchSectionOpenedCallback;

        public BlockSwitchButtons(IHasDrawLayer parent,Color parentColor,Action switchSectionClosedCallback,Action<int> switchSectionOpenedCallback) : base(parent,BuiltInMenuSprites.BlockBottom)
        {
            _switchSectionClosedCallback = switchSectionClosedCallback;
            _switchSectionOpenedCallback = switchSectionOpenedCallback;

            ColorMask = parentColor;
            SwitchSectionsNames = new List<string>();

            _leftButton = new BlockSwitchButton(this, "",0);
            _leftButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset);
            _leftButton.OnMouseReleased += (i) => _buttonClicked(_leftButton);
            AddChild(_leftButton);

            _rightButton = new BlockSwitchButton(this, "",1);
            _rightButton.SetLocationConfig(70, 0, CoordinateMode.ParentPixelOffset);
            _rightButton.OnMouseReleased += (i) => _buttonClicked(_rightButton);
            AddChild(_rightButton);

            _addArrowButtons();
        }

        private void _addArrowButtons()
        {
            var switchArrowButtonLeft = new SpriteMenuItem(this, BuiltInMenuSprites.SwitchBlockSideArrow);
            switchArrowButtonLeft.SetLocationConfig(140, 0, CoordinateMode.ParentPixelOffset);
            switchArrowButtonLeft.OnMouseReleased += (i) => { UpdateButtonOffset(-1); };
            AddChild(switchArrowButtonLeft);

            var switchArrowButtonRight = new SpriteMenuItem(this, BuiltInMenuSprites.SwitchBlockSideArrow);
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

        private void _buttonClicked(BlockSwitchButton button)
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

        private void _openSwitchSection(BlockSwitchButton button)
        {
            if (button.Text.Equals("")) { return; }

            button.ColorMask = Color.LightGreen;
            _selectedButton = button;
            _switchSectionOpenedCallback(button.ButtonIndex + _indexOffset);
        }

    }

    class BlockSwitchButton : TextBoxMenuItem
    {
        public int ButtonIndex;

        public BlockSwitchButton(IHasDrawLayer parentDrawLayer, string initialString,int buttonIndex) : base(parentDrawLayer, initialString)
        {
            SpriteName = BuiltInMenuSprites.IfBlockSwitchButton;
            ButtonIndex = buttonIndex;
            Editable = false;
        }
    }
}
