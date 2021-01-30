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
            kernelScreenPos = _config.GetPosOnScreen(_kernel.Host);
            _kernelCameraUpdate(input);
        }

        protected abstract void _kernelCameraUpdate(UserInput input);
    }
}
