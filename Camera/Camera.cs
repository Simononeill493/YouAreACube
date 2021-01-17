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

        public void DrawWorld(DrawingInterface drawingInterface,Sector sector)
        {
            _drawingInterface = drawingInterface;

            _tileSizeScaled = Config.TileSizeActual * Scale;
            _visibleGridWidth = (MonoGameWindow.CurrentWidth / _tileSizeScaled) + 1;
            _visibleGridHeight = (MonoGameWindow.CurrentHeight / _tileSizeScaled) + 1;

            DrawTiles(sector);
        }

        public void DrawTiles(Sector sector)
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

        public void KeyInput(KeyboardState keyboardState, List<Keys> keysUp)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                CameraYPosition--;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                CameraYPosition++;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                CameraXPosition--;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
               CameraXPosition++;
            }
            if (keysUp.Contains(Keys.P))
            {
                Scale++;
            }
            if (keysUp.Contains(Keys.O))
            {
                if (Scale > 1) { Scale--; }
            }

            //Console.WriteLine(XGridPos + " " + YGridPos + " " + Scale);
        }
    }
}
