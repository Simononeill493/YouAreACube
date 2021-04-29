﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class SpriteManager
    {
        public static bool IsInitialized = false;

        public static Dictionary<string, Texture2D> _sprites;
        private static ContentManager _contentManager;
        private static SpriteFont _gameFont;

        public static void LoadContent(ContentManager contentManager, SpriteFont gameFont)
        {
            _sprites = new Dictionary<string, Texture2D>();
            _contentManager = contentManager;
            _gameFont = gameFont;

            IsInitialized = true;
        }

        public static Texture2D GetSprite(string spriteName)
        {
            if (_sprites.ContainsKey(spriteName))
            {
                return _sprites[spriteName];
            }
            else
            {
                var sprite = _contentManager.Load<Texture2D>(spriteName);
                _sprites[spriteName] = sprite;
                return sprite;
            }
        }

        public static IntPoint GetSpriteSize(string spriteName)
        {
            if(!IsInitialized)
            {
                return IntPoint.Zero;
            }

            var sprite = GetSprite(spriteName);
            return new IntPoint(sprite.Width, sprite.Height);
        }

        public static IntPoint GetTextSize(string text)
        {
            if (text == null | !IsInitialized)
            {
                return IntPoint.Zero;
            }

            var dims = _gameFont.MeasureString(text);
            return new IntPoint((int)dims.X, (int)dims.Y);
        }

    }
}
