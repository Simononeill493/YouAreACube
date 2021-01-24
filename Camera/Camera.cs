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
        private World _world;

        public int Scale = 1;
        public int CameraXPosition;
        public int CameraYPosition;

        private DrawingInterface _drawingInterface;

        private int _tileSizeScaled;
        private int _visibleGridWidth;
        private int _visibleGridHeight;

        public Camera(World world)
        {
            _world = world;
        }

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
        
        public void Draw(DrawingInterface drawingInterface)
        {
            var sector = _world.Current;

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
                        _drawingInterface.DrawSprite("Black", xDrawOffset, yDrawOffset, Scale);
                        continue;
                    }

                    _drawTile(tile, xDrawOffset, yDrawOffset);
                }
            }
        }
        private void _drawTile(Tile tile,int xDrawOffset, int yDrawOffset)
        {
            _drawTileSprite(tile.Ground, xDrawOffset, yDrawOffset,1);

            if (tile.Contents != null)
            {
                _drawTileSprite(tile.Contents, xDrawOffset, yDrawOffset,0);
            }
        }

        private void _drawTileSprite(Block block,int xOffset,int yOffset,int layer)
        {
            if(block.IsMoving)
            {
                var data = block.MovementData;
                var movingOffset = ((float)data.MovementPosition / block.Speed) * _tileSizeScaled;

                xOffset += (int)(data.XOffset * movingOffset);
                yOffset += (int)(data.YOffset * movingOffset);
            }

            _drawingInterface.DrawSprite(block.Sprite, xOffset, yOffset, layer,Scale);
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
