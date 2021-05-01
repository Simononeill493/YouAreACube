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
        protected Kernel _kernel;

        public Camera(Kernel kernel)
        {
            _kernel = kernel;
            _config = new CameraConfiguration();
        }

        protected virtual void _update(UserInput input) { }
        protected void _handleUserInput(UserInput input) => _config.HandleUserInput(input);
        public void SetMouseHover(UserInput input, World world) => _config.SetMouseHover(input, world);


        public void Update(UserInput input)
        {
            _handleUserInput(input);
            _config.UpdateScaling();

            _update(input);
            _config.UpdateGridOffsets();
        }

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

                    _drawThisTile(world, drawPos, tileLocation);
                }
            }
        }

        protected void _drawSectorBoundaries(World world) => world.Focus.Neighbours.ForEach(s => _drawingInterface.DrawSectorGridOverlay(s.AbsoluteLocation,world.SectorSize, 3, _config));

        private void _drawMouseHoverPos()
        {
            var pos = (_config.MouseHoverPosition * _config.TileSizePixels) - _config.PixelOffset;
            _drawingInterface.DrawRectangle(pos, _config.TileSizePixels, _config.TileSizePixels, DrawLayers.MouseTileHoverLayer, new Color(128, 128, 128, 128));
        }

        private void _drawThisTile(World world, IntPoint drawPos, IntPoint tileLocation)
        {
            if (world.HasTile(tileLocation))
            {
                _drawingInterface.DrawTile(world.GetTile(tileLocation), drawPos, _config);
                return;
            }

            _drawingInterface.DrawVoid(drawPos, _config);
        }

    }
}
