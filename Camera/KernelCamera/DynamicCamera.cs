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
        public DynamicCamera(Kernel kernel) : base(kernel)
        {
            Scale = 4;
            _clamptoKernel();
        }

        protected override void _kernelCameraUpdate(UserInput input)
        {
            if (input.IsKeyDown(Keys.Up))
            {
                CameraYPartialGridOffset -= 1;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                CameraYPartialGridOffset += 1;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                CameraXPartialGridOffset -= 1;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                CameraXPartialGridOffset += 1;
            }

            Console.WriteLine(_kernelXPos);
        }

        protected override void _draw(DrawingInterface drawingInterface, World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }
    }
}
