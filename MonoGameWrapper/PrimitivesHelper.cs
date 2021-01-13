using Microsoft.Xna.Framework;
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
        private Texture2D _myBasicTexture;
        private SpriteBatch _spriteBatch;

        public PrimitivesHelper(GraphicsDevice graphicsDevice,SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _myBasicTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);

            //Revisit this: this color is never used, but needs to be set for the texture to be visible.
            _myBasicTexture.SetData<Color>(new[] { Color.White });
        }

        public void DrawHorizontalLine(int x1, int y1, int length)
        //Draws a horizontal line extending right.
        {
            _spriteBatch.Draw(_myBasicTexture, new Rectangle(x1, y1, length, 1), null, Color.Green);
        }

        public void DrawVericalLine(int x1, int y1, int length)
        //Draws a vertical line extending down.
        {
            _spriteBatch.Draw(_myBasicTexture, new Rectangle(x1, y1, 1, length), null, Color.Green);
        }

        public void DrawGrid(int width, int height, int squareSize)
        {
            for(int i=0;i<width+1;i++)
            {
                DrawVericalLine(i * squareSize, 0, height * squareSize);
            }

            for (int i = 0; i < height+1; i++)
            {
                DrawHorizontalLine(0, i * squareSize,width*squareSize);
            }
        }
    }
}
