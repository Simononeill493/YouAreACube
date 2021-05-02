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
        protected Kernel _kernel;
        protected CameraConfiguration _config;
        protected DrawingInterface _drawingInterface;

        public Camera(Kernel kernel)
        {
            _kernel = kernel;
            _config = new CameraConfiguration();
        }

        public void Update(UserInput input)
        {
            _config.HandleUserInput(input);
            _config.UpdateScaling();

            _update(input);
            _config.RollOverGridOffsets();
        }
        protected abstract void _update(UserInput input);


        public virtual void Draw(DrawingInterface drawingInterface, World world)
        {
            _drawingInterface = drawingInterface;
            _drawTiles(world);
            _drawMouseHoverPos();

            if (_config.DebugMode)
            {
                _drawSectorBoundaries(world);
            }
        }

        protected void _drawTiles(World world)
        {
            var screenGridPos = IntPoint.Zero;
            for (screenGridPos.X = -1; screenGridPos.X < _config.VisibleGrid.X + 2; screenGridPos.X++)
            {
                for (screenGridPos.Y = -1; screenGridPos.Y < _config.VisibleGrid.Y + 2; screenGridPos.Y++)
                {
                    var drawPos = (screenGridPos * _config.TileSizePixels) - _config.PartialGridOffset;
                    var tileLocation = screenGridPos + _config.GridPosition;

                    _drawTile(world, drawPos, tileLocation);
                }
            }
        }
        protected void _drawTile(World world, IntPoint drawPos, IntPoint tileLocation)
        {
            if (world.HasTile(tileLocation))
            {
                _drawingInterface.DrawTile(world.GetTile(tileLocation), drawPos, _config);
                return;
            }

            _drawingInterface.DrawVoid(drawPos, _config);
        }

        protected void _drawMouseHoverPos()
        {
            var pos = (_config.MouseHoverPosition * _config.TileSizePixels) - _config.PixelOffset;
            _drawingInterface.DrawRectangle(pos, _config.TileSizePixels, _config.TileSizePixels, DrawLayers.MouseTileHoverLayer, new Color(128, 128, 128, 128));
        }
        protected void _drawSectorBoundaries(World world) => world.Focus.Neighbours.ForEach(s => _drawingInterface.DrawSectorGridOverlay(s.AbsoluteLocation,world.SectorSize, 3, _config));

        public void AssignMouseHover(UserInput input, World world) => _config.AssignMouseHover(input, world);
        public void GetMouseHover(UserInput input) => _config.GetMouseHover(input);
    }
}
