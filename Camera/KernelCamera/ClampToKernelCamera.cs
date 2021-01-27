using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ClampToKernelCamera : KernelCamera
    {
        public ClampToKernelCamera(Kernel kernel) : base(kernel)
        {
            Scale = 3;
        }

        protected override void _kernelCameraUpdate(UserInput input)
        {
            _clamptoKernel();
        }

        protected override void _draw(DrawingInterface drawingInterface, World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }
    }
}
