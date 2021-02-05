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
        private Point clampOffset;

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
            _config.ClampToBlock(kernel.Host,new Point(0,0));
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
                _config.ClampToBlock(_kernel.Host,clampOffset);
            }
        }

        protected void _startScrollingWithPlayerMovement()
        {
            var centre = _config.GetCameraCentre();

            clampOffset.X = centre.x - kernelScreenPos.X;
            clampOffset.Y = centre.y - kernelScreenPos.Y;

            IsScrolling = true;

            _moveBackFromCameraBounds();
        }

        private void _moveBackFromCameraBounds()
        {
            if (kernelScreenPos.X <= _scrollBoundaryLeft)
            {
                var difference = (_scrollBoundaryLeft - kernelScreenPos.X);
                clampOffset.X -= (difference);
            }
            if (kernelScreenPos.X >= _scrollBoundaryRight)
            {
                var difference = (kernelScreenPos.X - _scrollBoundaryRight);
                clampOffset.X += (difference);
            }
            if (kernelScreenPos.Y <= _scrollBoundaryTop)
            {
                var difference = (_scrollBoundaryTop - kernelScreenPos.Y);
                clampOffset.Y -= (difference);
            }
            if (kernelScreenPos.Y >= _scrollBoundaryBottom)
            {
                var difference = (kernelScreenPos.Y - _scrollBoundaryBottom);
                clampOffset.Y += (difference);
            }
        }

        private bool _isKernelOutOfCameraBounds()
        {
            var isOutOfBounds =
            (
                kernelScreenPos.X < _scrollBoundaryLeft |
                kernelScreenPos.Y < _scrollBoundaryTop |
                kernelScreenPos.X > _scrollBoundaryRight |
                kernelScreenPos.Y > _scrollBoundaryBottom
            );

            return isOutOfBounds;
        }
    }
}
