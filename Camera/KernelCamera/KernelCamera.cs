using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class KernelCamera : Camera
    {
        protected Kernel _kernel;
        protected int _kernelXPos = 0;
        protected int _kernelYPos = 0;
        protected int _kernelXChange;
        protected int _kernelYChange;

        public KernelCamera(Kernel kernel)
        {
            _kernel = kernel;
        }

        protected override void _update(UserInput input)
        {
            _updateKernelPosition();
            _kernelCameraUpdate(input);
        }
        protected abstract void _kernelCameraUpdate(UserInput input);

        protected void _updateKernelPosition()
        {
            var (newKernelXPos, newKernelYPos) = _getPosOnScreen(_kernel.Host);

            _kernelXChange = newKernelXPos - _kernelXPos;
            _kernelYChange = newKernelYPos - _kernelYPos;

            _kernelXPos = newKernelXPos;
            _kernelYPos = newKernelYPos;
        }

        protected void _clamptoKernel()
        {
            var host = _kernel.Host;
            var location = host.Location;

            CameraXGridPosition = location.X - (this._visibleGridWidth / 2);
            CameraYGridPosition = location.Y - (this._visibleGridHeight / 2);

            (CameraXPartialGridOffset, CameraYPartialGridOffset) = _getMovementOffsets(host);
        }
    }
}
