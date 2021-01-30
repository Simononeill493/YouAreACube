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
        protected CameraConfiguration _config;
        protected DrawingInterface _drawingInterface;

        public Camera()
        {
            _config = new CameraConfiguration();
        }

        public void Update(UserInput input)
        {
            _readKeys(input);

            _config.SetScreenScaling();
            _update(input);
            _config.RollOverPartialOffsets();
        }

        private void _readKeys(UserInput input)
        {
            if (input.IsKeyDown(Keys.Up))
            {
                _config.YPartialGridOffset -= 15;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                _config.YPartialGridOffset += 15;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                _config.XPartialGridOffset -= 15;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                _config.XPartialGridOffset += 15;
            }

            if (input.IsKeyJustPressed(Keys.P))
            {
                _config.Scale++;
            }
            if (input.IsKeyJustPressed(Keys.O))
            {
                if (_config.Scale > 1) { _config.Scale--; }
            }
        }

        public void Draw(DrawingInterface drawingInterface, World world)
        {
            _drawingInterface = drawingInterface;
            _draw(world);
        }

        protected abstract void _update(UserInput input);
        protected virtual void _draw(World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }

        protected void _drawTiles(Sector sector)
        {
            for (int i = -1; i < _config.VisibleGridWidth+1; i++)
            {
                for (int j = -1; j < _config.VisibleGridHeight + 1; j++)
                {
                    int xDrawPos = (i * _config.TileSizeScaled) - _config.XPartialGridOffset;
                    int yDrawPos = (j * _config.TileSizeScaled) - _config.YPartialGridOffset;

                    var (tile, hasTile) = sector.TryGetTile(i + _config.XGridPosition, j + _config.YGridPosition);
                    if (!hasTile)
                    {
                        _drawingInterface.DrawSprite("Black", xDrawPos, yDrawPos, _config.Scale);
                        continue;
                    }

                    _drawTile(tile, xDrawPos, yDrawPos);
                }
            }
        }
        protected void _drawTile(Tile tile,int xDrawPos, int yDrawPos)
        {
            _drawTileSprite(tile.Ground, xDrawPos, yDrawPos,1);

            if (tile.HasSurface)
            {
                _drawTileSprite(tile.Contents, xDrawPos, yDrawPos,0);
            }
        }
        protected void _drawTileSprite(Block block,int xDrawPos,int yDrawPos,int layer)
        {
            var (offsetX,offsetY) = CameraUtils.GetMovementOffsets(block,_config.TileSizeScaled);

            _drawingInterface.DrawSprite(block.Sprite, xDrawPos + offsetX, yDrawPos + offsetY, layer, _config.Scale);
        }
    }
}
