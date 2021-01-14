using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DrawingInterface
    {
        public GraphicsDevice GraphicsDevice;

        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        private PrimitivesHelper _primitivesHelper;

        //Helper class to draw 2D primitive shapes; not a lot of native support for this

        private ContentManager _contentManager;
        private Dictionary<string, Texture2D> _sprites;

        public void LoadContent(GraphicsDevice graphicsDevice,ContentManager contentManager)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _spriteFont = contentManager.Load<SpriteFont>("Arial");

            _primitivesHelper = new PrimitivesHelper(graphicsDevice, _spriteBatch);
            _contentManager = contentManager;
            _sprites = new Dictionary<string, Texture2D>();
        }

        public void BeginDrawFrame()
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
        }

        public void DrawGrid()
        {
            _primitivesHelper.DrawGrid(10, 10, 10);
        }

        public void DrawGrass(int x, int y)
        {
            DrawSprite("grass", x, y);
        }

        public void DrawSprite(string spriteName,int x, int y)
        {
            var sprite = _getSprite(spriteName);
            _spriteBatch.Draw(sprite, new Vector2(x, y));
        }

        private Texture2D _getSprite(string spriteName)
        {
            if(_sprites.ContainsKey(spriteName))
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

        public void EndDrawFrame()
        {

            _spriteBatch.End();
        }

        internal void DrawBackground(string background)
        {
            _spriteBatch.Draw(_getSprite(background),new Vector2(0,0),scale:new Vector2(16,16));
            //DrawSprite(background, 0, 0);
        }

        internal void DrawMenuItem(MenuItem item)
        {
            DrawSprite(item.SpriteName, item.XPos, item.YPos);
        }

    }
}
