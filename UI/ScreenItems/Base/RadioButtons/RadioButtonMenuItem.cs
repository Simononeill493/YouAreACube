using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RadioButtonMenuItem<T> : ScreenItem
    {
        public T Option;
        public int ButtonIndex;

        private IntPoint _baseSize;

        private SpriteScreenItem _buttonSprite;

        public RadioButtonMenuItem(IHasDrawLayer parent,T option, string text) : base(parent)
        {
            Option = option;

            _buttonSprite = new SpriteScreenItem(this, MenuSprites.UncheckedRadioButton);
            _buttonSprite.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, false);
            AddChild(_buttonSprite);

            var radioButtonText = _addStaticTextItem(text, _buttonSprite.GetBaseSize().X + 3, 0, CoordinateMode.ParentPixel, false);
            _baseSize = _buttonSprite.GetBaseSize();
            _baseSize.X += radioButtonText.GetBaseSize().X;
        }

        public void SetButton() => _buttonSprite.SpriteName = MenuSprites.CheckedRadioButton;
        public void ClearButton() => _buttonSprite.SpriteName = MenuSprites.UncheckedRadioButton;
        public override IntPoint GetBaseSize() => _baseSize;
    }
}
