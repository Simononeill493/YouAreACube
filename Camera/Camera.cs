using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class Camera
    {
        public int Scale = 1;
        public int CameraXGridPosition;
        public int CameraXGridOffset;

        public int CameraYGridPosition;
        public int CameraYGridOffset;

        protected DrawingInterface _drawingInterface;

        protected int _tileSizeScaled;
        protected int _visibleGridWidth;
        protected int _visibleGridHeight;

        public void Update(UserInput input)
        {
            if(CameraXGridOffset>_tileSizeScaled)
            {
                CameraXGridPosition += (CameraXGridOffset / _tileSizeScaled);
                CameraXGridOffset = CameraXGridOffset%_tileSizeScaled;
            }
            if (CameraYGridOffset > _tileSizeScaled)
            {
                CameraYGridPosition += (CameraYGridOffset / _tileSizeScaled);
                CameraYGridOffset = CameraYGridOffset % _tileSizeScaled;
            }
            if (CameraXGridOffset < 0)
            {
                CameraXGridPosition-= ((-CameraXGridOffset / _tileSizeScaled)+1);
                CameraXGridOffset = _tileSizeScaled - ((-CameraXGridOffset%_tileSizeScaled));
            }
            if (CameraYGridOffset < 0)
            {
                CameraYGridPosition -= ((-CameraYGridOffset / _tileSizeScaled) + 1);
                CameraYGridOffset = _tileSizeScaled - ((-CameraYGridOffset % _tileSizeScaled));
            }

            Console.WriteLine(CameraXGridOffset);
            _update(input);
        }

        public void Draw(DrawingInterface drawingInterface, World world)
        {
            _drawingInterface = drawingInterface;
            _setScreenScaling();

            _draw(drawingInterface,world);
        }

        protected abstract void _update(UserInput input);
        protected abstract void _draw(DrawingInterface drawingInterface, World world);


        protected void _drawTiles(Sector sector)
        {
            for (int i = -1; i < _visibleGridWidth+1; i++)
            {
                for (int j = -1; j < _visibleGridHeight+1; j++)
                {
                    int xDrawOffset = (i * _tileSizeScaled) - CameraXGridOffset;
                    int yDrawOffset = (j * _tileSizeScaled) - CameraYGridOffset;

                    var (tile, hasTile) = sector.TryGetTile(i + CameraXGridPosition, j + CameraYGridPosition);
                    if (!hasTile)
                    {
                        _drawingInterface.DrawSprite("Black", xDrawOffset, yDrawOffset, Scale);
                        continue;
                    }

                    _drawTile(tile, xDrawOffset, yDrawOffset);
                }
            }
        }
        protected void _drawTile(Tile tile,int xDrawOffset, int yDrawOffset)
        {
            _drawTileSprite(tile.Ground, xDrawOffset, yDrawOffset,1);

            if (tile.Contents != null)
            {
                _drawTileSprite(tile.Contents, xDrawOffset, yDrawOffset,0);
            }
        }
        protected void _drawTileSprite(Block block,int xOffset,int yOffset,int layer)
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


        protected void _setScreenScaling()
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
