using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TextBoxMenuItem : MenuItem
    {
        public string Text => text.Text;
        public int MaxTextLength = 9;

        public bool Typeable;
        public bool Focused;

        protected SpriteMenuItem box;
        protected TextMenuItem text;

        public event Action<string> OnTextChanged;

        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer,string initialString) : base(parentDrawLayer)
        {
            box = new SpriteMenuItem(this,"EmptyMenuRectangleMedium");
            text = new TextMenuItem(this) { Text = initialString };

            box.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, centered: false);
            text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            //text.ScaleOffset = -1;
            text.DrawLayer = box.DrawLayer - DrawLayers.MinLayerDistance;

            AddChild(box);
            box.AddChild(text);
        }

        public void SetText(string textToSet)
        {
            text.Text = textToSet;
        }

        public void RescaleText(int offset)
        {
            text.ScaleOffset += offset;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(input.LeftButton == ButtonState.Pressed)
            {
                Focused = _mouseHovering;
            }

            if(Typeable & Focused)
            {
                foreach (var key in input.KeysJustPressed)
                {
                    var size = this.GetSize();

                    if (_doTypeChar(key))
                    {
                        text.Text = text.Text + KeyUtils.KeyToChar(key);
                        text.UpdateLocation(ActualLocation, size);
                        OnTextChanged?.Invoke(Text);
                    }
                    else if (_doBackspace(key))
                    {
                        text.Text = text.Text.Substring(0, text.Text.Length - 1);
                        text.UpdateLocation(ActualLocation, size);
                        OnTextChanged?.Invoke(Text);
                    }
                }
            }
        }

        public override Point GetSize()
        {
            return box.GetSize();
        }

        public override bool IsMouseOver(UserInput input)
        {
            return box.IsMouseOver(input);
        }

        private bool _doTypeChar(Keys key) => (KeyUtils.IsAlphanumeric(key) && text.Text.Length < MaxTextLength);
        private bool _doBackspace(Keys key) => (key == Keys.Back && text.Text.Length > 0);
    }
}
