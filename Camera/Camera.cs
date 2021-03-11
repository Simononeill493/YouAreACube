using Microsoft.Xna.Framework;
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
        protected CameraConfiguration _config;
        protected DrawingInterface _drawingInterface;

        public Camera()
        {
            _config = new CameraConfiguration();
        }

        public void Update(UserInput input)
        {
            _handleUserInput(input);
            _config.UpdateScaling();

            _update(input);
            _config.UpdateGridOffsets();
        }
        protected virtual void _update(UserInput input) { }

        public virtual void Draw(DrawingInterface drawingInterface, World world)
        {
            _drawingInterface = drawingInterface;
            _drawTiles(world);
        }

        protected void _drawTiles(World world)
        {
            var screenGridPos = Point.Zero;
            for (screenGridPos.X = -1; screenGridPos.X < _config.VisibleGrid.X+2; screenGridPos.X++)
            {
                for (screenGridPos.Y=-1; screenGridPos.Y < _config.VisibleGrid.Y + 2; screenGridPos.Y++)
                {
                    var drawPos = (screenGridPos * _config.TileSizeActual) - _config.PartialGridOffset;
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

        private void _handleUserInput(UserInput input)
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

            if (input.IsKeyJustPressed(Keys.P) | input.ScrolledUp)
            {
                _config.Scale++;
            }
            if (input.IsKeyJustPressed(Keys.O) | input.ScrolledDown)
            {
                if (_config.Scale > 1) { _config.Scale--; }
            }
        }
    }
}
