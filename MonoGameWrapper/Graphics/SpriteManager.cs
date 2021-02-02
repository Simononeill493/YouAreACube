using Microsoft.Xna.Framework.Content;
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
        public static Dictionary<string, Texture2D> _sprites;
        private static ContentManager _contentManager;

        public static void LoadContent(ContentManager contentManager)
        {
            _sprites = new Dictionary<string, Texture2D>();
            _contentManager = contentManager;
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
    }
}
