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
        protected override void _draw(World world)
        {
            base._draw(world);
            _drawHUD();
        }

        protected void _drawHUD()
        {
            _drawingInterface.DrawHUD(_kernel,MonoGameWindow.CurrentWidth/16, MonoGameWindow.CurrentHeight / 16);
        }
        protected abstract void _kernelCameraUpdate(UserInput input);
    }
}
