using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class GridCamera : Camera
    {
        protected override void _update(UserInput input)
        {
            /*if (input.IsKeyDown(Keys.Up))
            {
                CameraYGridPosition--;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                CameraYGridPosition++;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                CameraXGridPosition--;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                CameraXGridPosition++;
            }*/
        }

        protected override void _draw(DrawingInterface drawingInterface,World world)
        {
            var sector = world.Current;
            _drawTiles(sector);
        }
    }
}
