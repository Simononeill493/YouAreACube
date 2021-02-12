using Microsoft.Xna.Framework;
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

        public virtual void Draw(DrawingInterface drawingInterface, World world)
        {
            _drawingInterface = drawingInterface;
            _drawTiles(world);
        }

        protected abstract void _update(UserInput input);
        protected void _drawTiles(World world)
        {
            var screenGridPos = Point.Zero;
            for (screenGridPos.X = -1; screenGridPos.X < _config.VisibleGridWidth+1; screenGridPos.X++)
            {
                for (screenGridPos.Y=-1; screenGridPos.Y < _config.VisibleGridHeight + 1; screenGridPos.Y++)
                {
                    var drawPos = (screenGridPos * _config.TileSizeScaled) - _config.PartialGridOffset;
                    var tileLocation = screenGridPos + _config.GridPosition;

                    _drawThisLocation(world, drawPos, tileLocation);
                }
            }
        }

        private void _drawThisLocation(World world,Point drawPos,Point tileLocation)
        {
            if (world.HasTile(tileLocation))
            {
                var outerTile = world.GetTile(tileLocation);
                _drawingInterface.DrawTile(outerTile, drawPos, _config);
            }
            else
            {
                _drawingInterface.DrawVoid(drawPos, _config);
            }
        }

        private void _readKeys(UserInput input)
        {
            if (input.IsKeyDown(Keys.Home))//up
            {
                _config.PartialGridOffset.Y -= 15;
            }
            if (input.IsKeyDown(Keys.End))//down
            {
                _config.PartialGridOffset.Y += 15;
            }
            if (input.IsKeyDown(Keys.Delete))//left
            {
                _config.PartialGridOffset.X -= 15;
            }
            if (input.IsKeyDown(Keys.PageDown))//right
            {
                _config.PartialGridOffset.X += 15;
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
    }
}
