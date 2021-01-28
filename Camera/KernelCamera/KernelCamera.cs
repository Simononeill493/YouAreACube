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
        protected (int x, int y) kernelScreenPos;

        public KernelCamera(Kernel kernel)
        {
            _kernel = kernel;
        }

        protected override void _update(UserInput input)
        {
            kernelScreenPos = _getPosOnScreen(_kernel.Host);
            _kernelCameraUpdate(input);
        }
        protected abstract void _kernelCameraUpdate(UserInput input);

        protected void _clamptoKernel(int xOffset=0,int yOffset=0)
        {
            var host = _kernel.Host;

            CameraXGridPosition = host.Location.X - (this._visibleGridWidth / 2);
            CameraYGridPosition = host.Location.Y - (this._visibleGridHeight / 2);

            (CameraXPartialGridOffset, CameraYPartialGridOffset) = _getMovementOffsets(host);

            CameraXPartialGridOffset += xOffset;
            CameraYPartialGridOffset += yOffset; 
        }
    }
}
