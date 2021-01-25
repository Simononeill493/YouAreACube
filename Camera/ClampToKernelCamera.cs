using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ClampToKernelCamera : Camera
    {
        private Kernel _kernel;

        public ClampToKernelCamera(Kernel kernel)
        {
            _kernel = kernel;
            Scale = 3;
        }

        protected override void _update(UserInput input)
        {
            var host = _kernel.Host;
            var location = host.Location;

            CameraXGridPosition = location.X - (this._visibleGridWidth / 2);
            CameraYGridPosition = location.Y - (this._visibleGridHeight / 2);

            if (host.IsMoving)
            {
                var hostMovement = host.MovementData;
                var movementOffset = (int)(((hostMovement.MovementPosition) / (float)(host.Speed)) * _tileSizeScaled);

                CameraXGridOffset = hostMovement.XOffset * movementOffset;
                CameraYGridOffset = hostMovement.YOffset * movementOffset;
            }
            else
            {
                CameraXGridOffset = 0;
                CameraYGridOffset = 0;
            }
        }

        protected override void _draw(DrawingInterface drawingInterface, World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }
    }
}
