using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class Screen
    {
        public abstract void Update(MouseState mouseState, KeyboardState keyboardState);
        public abstract void Draw(DrawingInterface drawingInterface);
    }
}
