using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class PrimitivesHelper
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Texture2D _standardTexture;

        public int ViewportWidth => _graphicsDevice.Viewport.Width;
        public int ViewportHeight => _graphicsDevice.Viewport.Height;

        public PrimitivesHelper() { }
        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _spriteFont = contentManager.Load<SpriteFont>("PressStart2P");

            _standardTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _standardTexture.SetData(new[] { Color.White });
            //Revisit this: this color is never used, but needs to be set for the texture to be visible.

            SpriteManager.LoadContent(contentManager);
        }

        public void BeginDrawFrame()
        {
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
        }
        public void EndDrawFrame()
        {
            _spriteBatch.End();
        }

        public void DrawHorizontalLine(int x1, int y1, int length,float layer,Color color)
        {
            _spriteBatch.Draw(_standardTexture, destinationRectangle: new Rectangle(x1, y1, length, 1), color: color, layerDepth: layer);
        }
        public void DrawVericalLine(int x1, int y1, int length,float layer,Color color)
        {
            _spriteBatch.Draw(_standardTexture, destinationRectangle: new Rectangle(x1, y1, 1, length), color: color, layerDepth: layer);
        }
        public void DrawRectangle(int x, int y, int width, int height, float layer, Color color,bool centered)
        {
            if(centered)
            {
                (x, y) = DrawUtils.GetCenteredCoords(width, height, x, y, 1);
            }

            _spriteBatch.Draw(_standardTexture, destinationRectangle: new Rectangle(x, y, width, height),color: color, layerDepth: layer);
        }
        public void DrawGrid(int width, int height, int squareSize,float layer,Color color)
        {
            for(int i=0;i<width+1;i++)
            {
                DrawVericalLine(i * squareSize, 0, height * squareSize,layer,color);
            }

            for (int i = 0; i < height+1; i++)
            {
                DrawHorizontalLine(0, i * squareSize,width*squareSize, layer, color);
            }
        }
       
        public void DrawSprite(string spriteName, int x, int y, int scale, float layer,bool centered)
        {
            var sprite = SpriteManager.GetSprite(spriteName);
            if (centered)
            {
                (x,y) = DrawUtils.GetCenteredCoords(sprite.Width, sprite.Height, x, y, scale);
            }

            _spriteBatch.Draw(sprite, new Vector2(x, y), scale: new Vector2(scale, scale), layerDepth: layer);
        }


        public void DrawText(string text, int x, int y, int scale, float layer,bool centered)
        {
            if(centered)
            {
                var dims = _spriteFont.MeasureString(text);
                (x, y) = DrawUtils.GetCenteredCoords((int)dims.X, (int)dims.Y, x, y, scale);
            }

            _spriteBatch.DrawString(_spriteFont, text, new Vector2(x,y), Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public void DrawStretchedToScreen(string spriteName)
        {
            var backgroundSprite = SpriteManager.GetSprite(spriteName);
            var horizontalScale = ViewportWidth / (float)backgroundSprite.Width;
            var verticallScale = ViewportHeight / (float)backgroundSprite.Height;

            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), scale: new Vector2(horizontalScale, verticallScale), layerDepth: DrawLayers.BackgroundLayer);
        }

    }
}
