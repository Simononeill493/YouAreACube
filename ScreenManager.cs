using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ScreenManager
    {
        public ScreenManager()
        {
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {

        }

        public void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawGrid();
            drawingInterface.DrawGrass();
        }


    }
}
