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
            if (input.IsKeyDown(Keys.Home))//up
            {
                _config.YPartialGridOffset -= 15;
            }
            if (input.IsKeyDown(Keys.End))//down
            {
                _config.YPartialGridOffset += 15;
            }
            if (input.IsKeyDown(Keys.Delete))//left
            {
                _config.XPartialGridOffset -= 15;
            }
            if (input.IsKeyDown(Keys.PageDown))//right
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
            var sector = world.Centre;
            _drawTiles(world,world.Centre);
        }

        protected void _drawTiles(World world,Sector sector)
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
                        if(world.HasTile(i + _config.XGridPosition, j + _config.YGridPosition))
                        {
                            var outerTile = world.GetTile(i + _config.XGridPosition, j + _config.YGridPosition);
                            _drawTile(outerTile, xDrawPos, yDrawPos);
                        }
                        else
                        {
                            _drawingInterface.DrawSprite("Black", xDrawPos, yDrawPos, _config.Scale);
                        }
                        continue;
                    }

                    _drawTile(tile, xDrawPos, yDrawPos);
                }
            }
        }
        protected void _drawTile(Tile tile,int xDrawPos, int yDrawPos)
        {
            _drawTileSprite(tile.Ground, xDrawPos, yDrawPos,CameraDrawLayer.GroundLayer);

            if (tile.HasSurface)
            {
                _drawTileSprite(tile.Surface, xDrawPos, yDrawPos, CameraDrawLayer.SurfaceLayer);
            }
            if (tile.HasEphemeral)
            {
                _drawTileSprite(tile.Ephemeral, xDrawPos, yDrawPos, CameraDrawLayer.EphemeralLayer);
            }
        }
        protected void _drawTileSprite(Block block,int xDrawPos,int yDrawPos,float layer)
        {
            var (offsetX,offsetY) = CameraUtils.GetMovementOffsets(block,_config.TileSizeScaled);

            _drawingInterface.DrawSprite(block.Sprite, xDrawPos + offsetX, yDrawPos + offsetY, layer, _config.Scale);
        }
    }
}
