using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Camera
    {
        public int Scale = 1;
        public int CameraXPosition;
        public int CameraYPosition;

        private DrawingInterface _drawingInterface;

        private int _tileSizeScaled;
        private int _visibleGridWidth;
        private int _visibleGridHeight;

        public void Update(UserInput input)
        {
            if (input.IsKeyDown(Keys.Up))
            {
                CameraYPosition--;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                CameraYPosition++;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                CameraXPosition--;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                CameraXPosition++;
            }
            if (input.IsKeyJustPressed(Keys.P))
            {
                Scale++;
            }
            if (input.IsKeyJustPressed(Keys.O))
            {
                if (Scale > 1) { Scale--; }
            }

            //Console.WriteLine(XGridPos + " " + YGridPos + " " + Scale);
        }
        
        public void Draw(DrawingInterface drawingInterface,Sector sector)
        {
            _drawingInterface = drawingInterface;
            _setScreenScaling();

            _drawTiles(sector);
        }

        private void _drawTiles(Sector sector)
        {
            for (int i = 0; i < _visibleGridWidth; i++)
            {
                for (int j = 0; j < _visibleGridHeight; j++)
                {
                    int xDrawOffset = i * _tileSizeScaled;
                    int yDrawOffset = j * _tileSizeScaled;

                    var (tile, hasTile) = sector.TryGetTile(i + CameraXPosition, j + CameraYPosition);
                    if (!hasTile)
                    {
                        _drawTileSprite("Black", xDrawOffset, yDrawOffset);
                        continue;
                    }

                    _drawTile(tile, xDrawOffset, yDrawOffset);
                }
            }
        }
        private void _drawTile(Tile tile,int xDrawOffset, int yDrawOffset)
        {
            _drawTileSprite(tile.Ground.Sprite, xDrawOffset, yDrawOffset);

            if (tile.Contents != null)
            {
                _drawTileSprite(tile.Contents.Sprite, xDrawOffset, yDrawOffset);
            }

        }
        private void _drawTileSprite(string spriteName,int x,int y)
        {
            _drawingInterface.DrawSprite(spriteName, x, y, Scale);
        }

        private void _setScreenScaling()
        {
            if(Scale<1)
            {
                Console.WriteLine("Warning: Camera scale is set to less than 1 (" + Scale + ").");
            }

            _tileSizeScaled = Config.TileSizeActual * Scale;
            _visibleGridWidth = (MonoGameWindow.CurrentWidth / _tileSizeScaled) + 1;
            _visibleGridHeight = (MonoGameWindow.CurrentHeight / _tileSizeScaled) + 1;
        }
    }
}
