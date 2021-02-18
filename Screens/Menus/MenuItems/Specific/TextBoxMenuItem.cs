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
        
        public TextBoxMenuItem(string initialString,bool typeable)
        {
            box = new SpriteMenuItem("EmptyMenuRectangleMedium");
            text = new TextMenuItem() { Text = initialString };

            box.SetLocationConfig(0, 0, CoordinateMode.Absolute, centered: false);
            text.SetLocationConfig(50, 50, CoordinateMode.Relative, centered: true);

            AddChild(box);
            AddChild(text);

            Typeable = typeable;
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
