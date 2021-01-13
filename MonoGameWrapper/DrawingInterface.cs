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

        private Texture2D _grass;

        public void LoadContent(GraphicsDevice graphicsDevice,ContentManager contentManager)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _spriteFont = contentManager.Load<SpriteFont>("Arial");

            _primitivesHelper = new PrimitivesHelper(graphicsDevice, _spriteBatch);
            _grass = contentManager.Load<Texture2D>("grass");
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

        public void DrawGrass()
        {
            _spriteBatch.Draw(_grass, Vector2.Zero);
        }

        public void EndDrawFrame()
        {
            _spriteBatch.End();
        }

    }
}
