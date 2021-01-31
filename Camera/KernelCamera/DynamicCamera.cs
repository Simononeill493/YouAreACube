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
        private bool IsScrolling;
        private int clampOffsetX;
        private int clampOffsetY;

        private int _scrollEdge;
        private int _cameraScrollBoundary => _config.TileSizeScaled * _scrollEdge;
        private int _scrollBoundaryTop => _cameraScrollBoundary - _config.TileSizeScaled;
        private int _scrollBoundaryBottom => (MonoGameWindow.CurrentHeight - _cameraScrollBoundary);
        private int _scrollBoundaryLeft => _cameraScrollBoundary - _config.TileSizeScaled;
        private int _scrollBoundaryRight => (MonoGameWindow.CurrentWidth - _cameraScrollBoundary);

        public DynamicCamera(Kernel kernel) : base(kernel)
        {
            _config.Scale = 4;
            _scrollEdge = 4;

            _config.SetScreenScaling();
            _config.ClampToBlock(kernel.Host);
        }

        protected override void _kernelCameraUpdate(UserInput input)
        {
            if (_isKernelOutOfCameraBounds())
            {
                if (!IsScrolling)
                {
                    _startScrollingWithPlayerMovement();
                }
            }
            else
            {
                IsScrolling = false;
            }

            if (IsScrolling)
            {
                _config.ClampToBlock(_kernel.Host,clampOffsetX,clampOffsetY);
            }
        }

        protected void _startScrollingWithPlayerMovement()
        {
            var centre = _config.GetCameraCentre();

            clampOffsetX = centre.x - kernelScreenPos.x;
            clampOffsetY = centre.y - kernelScreenPos.y;

            IsScrolling = true;

            _moveBackFromCameraBounds();
        }

        private void _moveBackFromCameraBounds()
        {
            if (kernelScreenPos.x <= _scrollBoundaryLeft)
            {
                var difference = (_scrollBoundaryLeft - kernelScreenPos.x);
                clampOffsetX -= (difference);
            }
            if (kernelScreenPos.x >= _scrollBoundaryRight)
            {
                var difference = (kernelScreenPos.x - _scrollBoundaryRight);
                clampOffsetX += (difference);
            }
            if (kernelScreenPos.y <= _scrollBoundaryTop)
            {
                var difference = (_scrollBoundaryTop - kernelScreenPos.y);
                clampOffsetY -= (difference);
            }
            if (kernelScreenPos.y >= _scrollBoundaryBottom)
            {
                var difference = (kernelScreenPos.y - _scrollBoundaryBottom);
                clampOffsetY += (difference);
            }
        }

        private bool _isKernelOutOfCameraBounds()
        {
            var isOutOfBounds =
            (
                kernelScreenPos.x < _scrollBoundaryLeft |
                kernelScreenPos.y < _scrollBoundaryTop |
                kernelScreenPos.x > _scrollBoundaryRight |
                kernelScreenPos.y > _scrollBoundaryBottom
            );

            return isOutOfBounds;
        }
    }
}
