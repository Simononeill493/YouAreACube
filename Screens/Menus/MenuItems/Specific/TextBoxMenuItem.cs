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
        public bool Typeable;

        private SpriteMenuItem box;
        private TextMenuItem text;

        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer,string initialString,bool typeable) : base(parentDrawLayer)
        {
            box = new SpriteMenuItem(this,"EmptyMenuRectangleMedium");
            text = new TextMenuItem(this) { Text = initialString };

            box.SetLocationConfig(0, 0, CoordinateMode.ParentOffset, centered: false);
            text.SetLocationConfig(50, 50, CoordinateMode.ParentRelative, centered: true);
            text.ScaleOffset = -1;

            AddChild(box);
            box.AddChild(text);

            Typeable = typeable;
        }

        public void SetText(string textToSet)
        {
            text.Text = textToSet;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(Typeable)
            {
                foreach (var key in input.KeysJustPressed)
                {
                    var size = this.GetSize();

                    if (_doTypeChar(key))
                    {
                        text.Text = text.Text + KeyUtils.KeyToChar(key);
                        text.UpdateLocation(ActualLocation, size);
                    }
                    else if (_doBackspace(key))
                    {
                        text.Text = text.Text.Substring(0, text.Text.Length - 1);
                        text.UpdateLocation(ActualLocation, size);
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

        private bool _doTypeChar(Keys key) => (KeyUtils.IsAlphanumeric(key) && text.Text.Length < 9);
        private bool _doBackspace(Keys key) => (key == Keys.Back && text.Text.Length > 0);
    }
}
