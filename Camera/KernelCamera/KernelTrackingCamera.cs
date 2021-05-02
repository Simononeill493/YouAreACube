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
        private bool _isScrolling;

        private IntPoint _clampOffset;
        private IntPoint _kernelPositionOnScreen;

        private ScrollBoundary _scrollBoundary;

        public KernelTrackingCamera(Kernel kernel) : base(kernel)
        {
            _scrollBoundary.BoundaryNumTiles = 3;

            _config.Scale = 4;
            _config.UpdateScaling();
            _config.SnapToBlock(kernel.Host,IntPoint.Zero);
        }

        protected override void _update(UserInput input)
        {
            _kernelPositionOnScreen = _config.GetPosOnScreen(_kernel.Host);
            _kernelCameraUpdate(input);
        }

        protected void _kernelCameraUpdate(UserInput input)
        {
            _scrollBoundary.Update(_config.TileSizePixels);
            if(_scrollBoundary.WithinBoundary(_kernelPositionOnScreen))
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
            _clampOffset = _config.GetCameraCentre() - _kernelPositionOnScreen;
            _clampOffset += _scrollBoundary.GetBoundaryPushBack(_kernelPositionOnScreen);

            _isScrolling = true;
        }        
    }
}
