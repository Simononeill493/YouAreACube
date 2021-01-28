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
        private int _cameraScrollBoundary => _tileSizeScaled * _scrollEdge;
        private int _scrollBoundaryTop => _cameraScrollBoundary - _tileSizeScaled;
        private int _scrollBoundaryBottom => (MonoGameWindow.CurrentHeight - _cameraScrollBoundary);
        private int _scrollBoundaryLeft => _cameraScrollBoundary - _tileSizeScaled;
        private int _scrollBoundaryRight => (MonoGameWindow.CurrentWidth - _cameraScrollBoundary);

        public DynamicCamera(Kernel kernel) : base(kernel)
        {
            Scale = 2;
            _scrollEdge = 4;

            _setScreenScaling();
            _clamptoKernel();

        }

        protected override void _kernelCameraUpdate(UserInput input)
        {
            if (_isKernelOutOfCameraBounds())
            {
                if (!IsScrolling)
                {
                    _startScrolling();
                }
            }
            else
            {
                IsScrolling = false;
            }

            if (IsScrolling)
            {
                _clamptoKernel(-clampOffsetX, -clampOffsetY);
            }
        }

        protected void _startScrolling()
        {
            var xMidPoint = ((_visibleGridWidth / 2 * _tileSizeScaled));
            var yMidPoint = ((_visibleGridHeight / 2 * _tileSizeScaled));

            clampOffsetX = kernelScreenPos.x - xMidPoint;
            clampOffsetY = kernelScreenPos.y - yMidPoint;

            IsScrolling = true;

            _moveBackFromCameraBounds();
        }

        private void _moveBackFromCameraBounds()
        {
            if (kernelScreenPos.x <= _scrollBoundaryLeft)
            {
                var difference = (_scrollBoundaryLeft - kernelScreenPos.x);
                clampOffsetX += (difference);
            }
            if (kernelScreenPos.x >= _scrollBoundaryRight)
            {
                var difference = (kernelScreenPos.x - _scrollBoundaryRight);
                clampOffsetX -= (difference);
            }
            if (kernelScreenPos.y <= _scrollBoundaryTop)
            {
                var difference = (_scrollBoundaryTop - kernelScreenPos.y);
                clampOffsetY += (difference);
            }
            if (kernelScreenPos.y >= _scrollBoundaryBottom)
            {
                var difference = (kernelScreenPos.y - _scrollBoundaryBottom);
                clampOffsetY -= (difference);
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

        protected override void _draw(DrawingInterface drawingInterface, World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }
    }
}
