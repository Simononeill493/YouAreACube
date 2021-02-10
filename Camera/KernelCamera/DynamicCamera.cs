using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DynamicCamera : KernelCamera
    {
        private bool _isScrolling;
        private Point _clampOffset;

        private int _borderNumTiles;
        private ScrollBoundaries _boundary;

        public DynamicCamera(Kernel kernel) : base(kernel)
        {
            _config.Scale = 4;
            _borderNumTiles = 4;

            _config.SetScreenScaling();
            _config.SnapToBlock(kernel.Host,Point.Zero);
        }

        protected override void _kernelCameraUpdate(UserInput input)
        {
            _boundary.Update(_borderNumTiles, _config.TileSizeScaled);

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
