using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class KernelTrackingCamera : Camera
    {
        private Kernel _kernel;
        private Point _kernelScreenPos;

        private bool _isScrolling;
        private Point _clampOffset;

        private int _borderNumTiles;
        private ScrollBoundaries _boundary;

        public KernelTrackingCamera(Kernel kernel) : base()
        {
            _kernel = kernel;

            _config.Scale = 4;
            _borderNumTiles = 3;

            _config.UpdateScaling();
            _config.SnapToBlock(kernel.Host,Point.Zero);
        }

        protected override void _update(UserInput input)
        {
            _kernelScreenPos = _config.GetPosOnScreen(_kernel.Host);
            _kernelCameraUpdate(input);
        }

        protected void _kernelCameraUpdate(UserInput input)
        {
            _boundary.Update(_borderNumTiles, _config.TileSizeActual);

            if(_boundary.WithinBoundary(_kernelScreenPos))
            {
                _isScrolling = false;
            }
            else if(!_isScrolling)
            {
                _startScrolling();
            }

            if (_isScrolling)
            {
                _config.SnapToBlock(_kernel.Host, _clampOffset);
            }

            /*if(input.ScrolledDown | input.ScrolledUp)
            {
                _clampOffset += _boundary.GetBoundaryPushBack(_kernelScreenPos);
            }*/
        }


        protected void _startScrolling()
        {
            var centre = _config.GetCameraCentre();

            _clampOffset.X = centre.x - _kernelScreenPos.X;
            _clampOffset.Y = centre.y - _kernelScreenPos.Y;
            _clampOffset += _boundary.GetBoundaryPushBack(_kernelScreenPos);

            _isScrolling = true;
        }
    }
}
