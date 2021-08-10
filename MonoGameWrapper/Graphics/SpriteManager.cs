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
            _contentManager = contentManager;
            Sprites = _loadSpritesDict();

            _gameFont = gameFont;
            _initialized = true;
        }

        private static Dictionary<string, Texture2D> _loadSpritesDict()
        {
            var spritesDict = new Dictionary<string, Texture2D>();
            foreach((string fullname,string friendlyName) in BuiltInSprites.AllSprites)
            {
                var sprite = _contentManager.Load<Texture2D>(fullname);
                spritesDict[friendlyName] = sprite;
            }

            return spritesDict;
        }


        public static Texture2D GetSprite(string spriteName) => Sprites[spriteName];

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
