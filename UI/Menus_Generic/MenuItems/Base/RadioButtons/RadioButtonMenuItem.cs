﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RadioButtonMenuItem<T> : MenuItem
    {
        public T Option;
        public int ButtonIndex;

        private IntPoint _baseSize;

        private SpriteMenuItem _buttonSprite;

        public RadioButtonMenuItem(IHasDrawLayer parent,T option, string text) : base(parent)
        {
            Option = option;

            _buttonSprite = new SpriteMenuItem(this, "RadioButtonUnchecked");
            _buttonSprite.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            AddChild(_buttonSprite);

            var radioButtonText = _addTextItem(text, _buttonSprite.GetBaseSize().X + 3, 0, CoordinateMode.ParentPixelOffset, false);
            _baseSize = _buttonSprite.GetBaseSize();
            _baseSize.X += radioButtonText.GetBaseSize().X;
        }

        public void SetButton() => _buttonSprite.SpriteName = "RadioButtonChecked";
        public void ClearButton() => _buttonSprite.SpriteName = "RadioButtonUnchecked";
        public override IntPoint GetBaseSize() => _baseSize;
    }
}