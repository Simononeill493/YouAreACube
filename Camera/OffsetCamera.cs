﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class OffsetCamera : Camera
    {
        private int _cameraSpeed = 1;

        public OffsetCamera()
        {
            Scale = 2;
        }

        protected override void _update(UserInput input)
        {
            /*if (input.IsKeyDown(Keys.Up))
            {
                CameraYPartialGridOffset -= _cameraSpeed;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                CameraYPartialGridOffset += _cameraSpeed;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                CameraXPartialGridOffset -= _cameraSpeed;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                CameraXPartialGridOffset += _cameraSpeed;
            }
            if (input.IsKeyJustPressed(Keys.L))
            {
                _cameraSpeed++;
            }
            if (input.IsKeyJustPressed(Keys.K))
            {
                if (_cameraSpeed > 1) { _cameraSpeed--; }
            }*/
        }

        protected override void _draw(DrawingInterface drawingInterface, World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }
    }
}