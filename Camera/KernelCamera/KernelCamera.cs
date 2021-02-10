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
        protected Point _kernelScreenPos;

        public KernelCamera(Kernel kernel) : base()
        {
            _kernel = kernel;
        }

        protected override void _update(UserInput input)
        {
            _kernelScreenPos = _config.GetPosOnScreen(_kernel.Host);
            _kernelCameraUpdate(input);
        }

        public override void Draw(DrawingInterface drawingInterface, World world)
        {
            base.Draw(drawingInterface, world);
            _drawManager.DrawHUD(_kernel);
        }

        protected abstract void _kernelCameraUpdate(UserInput input);
    }
}
