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
        public int CameraYGridPosition;

        public int CameraXPartialGridOffset;
        public int CameraYPartialGridOffset;

        public int CameraXOffsetTrue => (_tileSizeScaled * CameraXGridPosition) + CameraXPartialGridOffset;
        public int CameraYOffsetTrue => (_tileSizeScaled * CameraYGridPosition) + CameraYPartialGridOffset;

        protected int _tileSizeScaled;
        protected int _visibleGridWidth;
        protected int _visibleGridHeight;

        protected DrawingInterface _drawingInterface;

        public void Update(UserInput input)
        {
            _readKeys(input);

            _setScreenScaling();
            _update(input);
            _rollOverPartialOffsets();
        }
        private void _readKeys(UserInput input)
        {
            if (input.IsKeyDown(Keys.Up))
            {
                CameraYPartialGridOffset -= 15;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                CameraYPartialGridOffset += 15;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                CameraXPartialGridOffset -= 15;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                CameraXPartialGridOffset += 15;
            }

            if (input.IsKeyJustPressed(Keys.P))
            {
                Scale++;
            }
            if (input.IsKeyJustPressed(Keys.O))
            {
                if (Scale > 1) { Scale--; }
            }
        }

        public void Draw(DrawingInterface drawingInterface, World world)
        {
            _drawingInterface = drawingInterface;
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
                    int xDrawPos = (i * _tileSizeScaled) - CameraXPartialGridOffset;
                    int yDrawPos = (j * _tileSizeScaled) - CameraYPartialGridOffset;

                    var (tile, hasTile) = sector.TryGetTile(i + CameraXGridPosition, j + CameraYGridPosition);
                    if (!hasTile)
                    {
                        _drawingInterface.DrawSprite("Black", xDrawPos, yDrawPos, Scale);
                        continue;
                    }

                    _drawTile(tile, xDrawPos, yDrawPos);
                }
            }
        }
        protected void _drawTile(Tile tile,int xDrawPos, int yDrawPos)
        {
            _drawTileSprite(tile.Ground, xDrawPos, yDrawPos,1);

            if (tile.Contents != null)
            {
                _drawTileSprite(tile.Contents, xDrawPos, yDrawPos,0);
            }
        }
        protected void _drawTileSprite(Block block,int xDrawPos,int yDrawPos,int layer)
        {
            var (offsetX,offsetY) = _getMovementOffsets(block);

            _drawingInterface.DrawSprite(block.Sprite, xDrawPos + offsetX, yDrawPos + offsetY, layer, Scale);
        }

        protected void _rollOverPartialOffsets()
        {
            if (CameraXPartialGridOffset > _tileSizeScaled)
            {
                CameraXGridPosition += (CameraXPartialGridOffset / _tileSizeScaled);
                CameraXPartialGridOffset = CameraXPartialGridOffset % _tileSizeScaled;
            }
            if (CameraYPartialGridOffset > _tileSizeScaled)
            {
                CameraYGridPosition += (CameraYPartialGridOffset / _tileSizeScaled);
                CameraYPartialGridOffset = CameraYPartialGridOffset % _tileSizeScaled;
            }
            if (CameraXPartialGridOffset < 0)
            {
                CameraXGridPosition -= ((-CameraXPartialGridOffset / _tileSizeScaled) + 1);
                CameraXPartialGridOffset = _tileSizeScaled - ((-CameraXPartialGridOffset % _tileSizeScaled));
            }
            if (CameraYPartialGridOffset < 0)
            {
                CameraYGridPosition -= ((-CameraYPartialGridOffset / _tileSizeScaled) + 1);
                CameraYPartialGridOffset = _tileSizeScaled - ((-CameraYPartialGridOffset % _tileSizeScaled));
            }
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

        protected bool _isOnScreen(Block block)
        {
            var pos = _getPosOnScreen(block);
            return _isOnScreen(pos.x, pos.y);
        }
        protected bool _isOnScreen(int x,int y)
        {
            return (
            x >= 0 &
            y >= 0 &
            x <= (MonoGameWindow.CurrentWidth) &
            y <= (MonoGameWindow.CurrentHeight));

        }

        protected (int x, int y) _getScaledPosInSector(Block block)
        {
            var (offsetX, offsetY) = _getMovementOffsets(block);

            var XPos = (block.Location.X * _tileSizeScaled) + offsetX;
            var YPos = (block.Location.Y * _tileSizeScaled) + offsetY;

            return (XPos, YPos);
        }
        protected (int x,int y) _getPosOnScreen(Block block)
        {
            var (XPos, YPos) = _getScaledPosInSector(block);
            return (XPos-CameraXOffsetTrue, YPos-CameraYOffsetTrue);
        }
        protected (int x,int y) _getMovementOffsets(Block block)
        {
            var offset = _getMovementOffset(block);
            return (block.MovementData.XOffset * offset, block.MovementData.YOffset * offset);
        }
        protected int _getMovementOffset(Block block)
        {
            if (block.IsMoving)
            {
                return (int)(((block.MovementData.MovementPosition) / (float)(block.Speed)) * _tileSizeScaled);
            }

            return 0;
        }
    }
}
