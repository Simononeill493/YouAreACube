﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TextBoxMenuItem : SpriteMenuItem
    {
        public string Text => TextItem.Text;
        public TextMenuItem TextItem;

        public int MaxTextLength = 9;

        public bool Editable;
        public bool Focused;

        public event Action<string> OnTextChanged;

        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer,string initialString) : base(parentDrawLayer, "TextBoxMenuRectangle")
        {
            TextItem = new TextMenuItem(this) { Text = initialString };
            TextItem.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(TextItem);
        }

        public void SetText(string textToSet) => TextItem.Text = textToSet;


        public override void Update(UserInput input)
        {
            base.Update(input);

            if(input.MouseLeftPressed)
            {
                Focused = MouseHovering;
            }

            if(Editable & Focused)
            {
                foreach (var key in input.KeysJustPressed)
                {
                    var size = this.GetCurrentSize();

                    if (_shouldTypeCharacter(key))
                    {
                        TextItem.Text = TextItem.Text + KeyUtils.KeyToChar(key);
                        TextItem.UpdateDimensions(ActualLocation, size);
                        OnTextChanged?.Invoke(Text);
                    }
                    else if (_shouldTypeBackspace(key))
                    {
                        TextItem.Text = TextItem.Text.Substring(0, TextItem.Text.Length - 1);
                        TextItem.UpdateDimensions(ActualLocation, size);
                        OnTextChanged?.Invoke(Text);
                    }
                }
            }
        }

        private bool _shouldTypeCharacter(Keys key) => (KeyUtils.IsTypeable(key) && TextItem.Text.Length < MaxTextLength);
        private bool _shouldTypeBackspace(Keys key) => (key == Keys.Back && TextItem.Text.Length > 0);
    }
}