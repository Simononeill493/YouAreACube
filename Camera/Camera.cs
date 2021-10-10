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
        protected World _currentWorld;
        protected DrawingInterface _drawingInterface;
        protected CameraConfiguration _config;

        public Camera(Kernel kernel,World world = null)
        {
            _kernel = kernel;
            _config = new CameraConfiguration();

            if(world!=null)
            {
                SetWorld(world);
            }
        }

        public void SetWorld(World world)
        {
            _currentWorld = world;
        }

        public void Update(UserInput input)
        {
            _config.Update(input, _currentWorld);
            _update(input);

            _config.RollOverGridOffsets();
        }
        protected abstract void _update(UserInput input);


        public virtual void Draw(DrawingInterface drawingInterface)
        {
            _drawingInterface = drawingInterface;

            _drawTiles(_currentWorld);
            _drawMouseHoverPos();

            if (CameraConfiguration.DebugMode)
            {
                _drawSectorBoundaries(_currentWorld);
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

        public void GetMouseHover(UserInput input) => _config.GetMouseHover(input);
    }
}
