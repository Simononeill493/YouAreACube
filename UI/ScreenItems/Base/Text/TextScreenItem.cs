using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextScreenItem : ScreenItem
    {
        public string GetText()
        {
            var fetchedText = _textProvider();
            if(fetchedText == null)
            {
                return "_null_";
            }

            return fetchedText;
        }
        private Func<string> _textProvider;

        public Color Color = Config.DefaultTextColor;
        public event Action<string> OnTextTyped;

        public TextScreenItem(IHasDrawLayer parentDrawLayer, Func<string> textProvider) : base(parentDrawLayer)
        {
            _textProvider = textProvider;
        }

        public void TextTyped(string newValue) => OnTextTyped.Invoke(newValue);

        public void AppendKey(char toAppend) => TextTyped(_textProvider() + toAppend);
        public void Backspace()
        {
            var oldText = _textProvider();
            var newText = oldText.Substring(0, oldText.Length - 1);
            TextTyped(newText);
        }

        public bool KeyboardEdit(UserInput input,int maxLength)
        {
            bool edited = false;
            foreach (var key in input.KeysJustPressed)
            {
                var size = this.GetCurrentSize();

                if (_shouldTypeCharacter(key, maxLength))
                {
                    AppendKey(KeyUtils.KeyToChar(key));
                    edited = true;
                }
                else if (_shouldTypeBackspace(key))
                {
                    Backspace();
                    edited = true;
                }
            }

            return edited;
        }

        private bool _shouldTypeCharacter(Keys key,int maxLength) => (KeyUtils.IsTypeable(key) && GetText().Length < maxLength);
        private bool _shouldTypeBackspace(Keys key) => (key == Keys.Back && GetText().Length > 0);

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawText(GetText(), ActualLocation.X, ActualLocation.Y, Scale, DrawLayer, Color);
        }

        protected override bool _isMouseOver(UserInput input) => false;
        public override IntPoint GetBaseSize() => SpriteManager.GetTextSize(GetText());
    }
}
