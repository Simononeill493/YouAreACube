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
        public GraphicsDevice graphicsDevice;

        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        private PrimitivesHelper _primitivesHelper;

        //Helper class to draw 2D primitive shapes; not a lot of native support for this

        private ContentManager _contentManager;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            //_spriteFont = contentManager.Load<SpriteFont>("Connection3");
            //_spriteFont = contentManager.Load<SpriteFont>("ConnectionSerif");
            _spriteFont = contentManager.Load<SpriteFont>("PressStart2P");
            //_spriteFont = contentManager.Load<SpriteFont>("Arial");

            _primitivesHelper = new PrimitivesHelper(graphicsDevice, _spriteBatch);
            _contentManager = contentManager;
            SpriteManager.LoadContent(contentManager);
        }

        public void BeginDrawFrame()
        {
            graphicsDevice.Clear(Color.Black);
            //_spriteBatch.Begin();

           _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,SamplerState.PointClamp);

        }
        public void EndDrawFrame()
        {
            _spriteBatch.End();
        }

        public void DrawBackground(string background)
        {
            var backgroundSprite = SpriteManager.GetSprite(background);
            var horizontalScale = graphicsDevice.Viewport.Width / (float)backgroundSprite.Width;
            var verticallScale = graphicsDevice.Viewport.Height / (float)backgroundSprite.Height;
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), scale: new Vector2(horizontalScale, verticallScale));
        }
        public void DrawSprite(string spriteName, int x, int y, int scale = 1)
        {
            var sprite = SpriteManager.GetSprite(spriteName);
            _spriteBatch.Draw(sprite, new Vector2(x, y), scale: new Vector2(scale, scale));
        }
        public void DrawSpriteCentered(string spriteName, int x, int y, int scale = 1)
        {
            var sprite = SpriteManager.GetSprite(spriteName);
            var (xOffset, yoffset) = _transformCoordsToCenteredCoords(sprite.Width,sprite.Height, x, y, scale);

            _spriteBatch.Draw(sprite, new Vector2(xOffset, yoffset), scale: new Vector2(scale, scale));
        }
        public void DrawMenuItem(MenuItem item)
        {
            var (x, y) = _screenPercentageToCoords(item.XPercentage, item.YPercentage);

            //All menu items are drawn centered. 
            if(item.Highlightable & item.Hovering)
            {
                DrawSpriteCentered(item.HighlightedSpriteName, x, y, item.Scale);
            }
            else
            {
                DrawSpriteCentered(item.SpriteName, x, y, item.Scale);
            }

            if(item.HasText)
            {
                DrawText(item.Text, item.XPercentage, item.YPercentage, 2);
            }
        }
        public void DrawText(string text,int xPercentage,int yPercentage,int scale)
        {
            var (x, y) = _screenPercentageToCoords(xPercentage, yPercentage);
            var dims = _spriteFont.MeasureString(text);
            var (xOffs, yOffs) = _transformCoordsToCenteredCoords((int)dims.X, (int)dims.Y, x, y, scale);

            //_spriteBatch.DrawString(_spriteFont, text, new Vector2(xOffs, yOffs),Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(xOffs, yOffs), Color.Black, 0, Vector2.Zero, scale,SpriteEffects.None,0);

        }

        public static Rectangle GetMenuItemRectangle(MenuItem item)
        //public static (int x1, int y1, int x2, int y2) GetMenuItemRectangle(MenuItem item)
        {
            var sprite = SpriteManager.GetSprite(item.SpriteName);

            var (x, y) = _screenPercentageToCoords(item.XPercentage, item.YPercentage);
            var (x1, y1) = _transformCoordsToCenteredCoords(sprite.Width,sprite.Height, x, y, item.Scale);

            var x2 = x1 + sprite.Width * item.Scale;
            var y2 = y1 + sprite.Height * item.Scale;

            return new Rectangle(x1, y1, sprite.Width * item.Scale, sprite.Height * item.Scale);
            //return (x1, y1, x2, y2);
        }
        private static (int xOffset, int yOffset) _transformCoordsToCenteredCoords(int width,int height, int x, int y, int scale)
        {
            var xOffset = x - ((int)((width / 2.0) * scale));
            var yOffset = y - ((int)((height/ 2.0) * scale));

            return (xOffset, yOffset);
        }
        private static (int x, int y) _screenPercentageToCoords(int xPercentage, int yPercentage)
        {
            var x = (int)(MonoGameWindow.CurrentWidth * (xPercentage / 100.0));
            var y = (int)(MonoGameWindow.CurrentHeight * (yPercentage / 100.0));

            return (x, y);
        }
    }
}
