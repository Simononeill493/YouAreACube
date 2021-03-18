using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TextBoxMenuItem : SpriteMenuItem
    {
        public string Text => text.Text;
        public int MaxTextLength = 9;

        public bool Editable;
        public bool Focused;

        protected TextMenuItem text;

        public event Action<string> OnTextChanged;

        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer,string initialString) : base(parentDrawLayer,"EmptyMenuRectangleMedium")
        {
            text = new TextMenuItem(this) { Text = initialString };
            text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(text);
        }

        public void SetText(string textToSet)
        {
            text.Text = textToSet;
        }

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
                        text.Text = text.Text + KeyUtils.KeyToChar(key);
                        text.UpdateDimensions(ActualLocation, size);
                        OnTextChanged?.Invoke(Text);
                    }
                    else if (_shouldTypeBackspace(key))
                    {
                        text.Text = text.Text.Substring(0, text.Text.Length - 1);
                        text.UpdateDimensions(ActualLocation, size);
                        OnTextChanged?.Invoke(Text);
                    }
                }
            }
        }

        private bool _shouldTypeCharacter(Keys key) => (KeyUtils.IsAlphanumeric(key) && text.Text.Length < MaxTextLength);
        private bool _shouldTypeBackspace(Keys key) => (key == Keys.Back && text.Text.Length > 0);
    }
}
