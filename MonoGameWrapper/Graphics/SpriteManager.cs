using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class SpriteManager
    {
        public static Dictionary<string, Texture2D> Sprites;

        private static bool _initialized = false;
        private static ContentManager _contentManager;
        private static SpriteFont _gameFont;

        public static void LoadContent(ContentManager contentManager, SpriteFont gameFont)
        {
            Sprites = new Dictionary<string, Texture2D>();
            _contentManager = contentManager;
            _gameFont = gameFont;

            _initialized = true;
        }

        public static Texture2D GetSprite(string spriteName)
        {
            if (Sprites.ContainsKey(spriteName))
            {
                return Sprites[spriteName];
            }
            else
            {
                var sprite = _contentManager.Load<Texture2D>(spriteName);
                Sprites[spriteName] = sprite;
                return sprite;
            }
        }
        public static IntPoint GetSpriteSize(string spriteName)
        {
            if(!_initialized)
            {
                return IntPoint.Zero;
            }

            var sprite = GetSprite(spriteName);
            return new IntPoint(sprite.Width, sprite.Height);
        }
        public static IntPoint GetTextSize(string text)
        {
            if (text == null | !_initialized)
            {
                return IntPoint.Zero;
            }

            var dims = _gameFont.MeasureString(text);
            return new IntPoint((int)dims.X, (int)dims.Y);
        }
    }
}
