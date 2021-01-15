using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class LoadWorldScreen : MenuScreen
    {
        public override void Draw(DrawingInterface drawingInterface)
        {
            this.DrawBackgroundAndMenuItems(drawingInterface);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            this.MenuScreenUpdate(mouseState, keyboardState);

            throw new NotImplementedException();
        }
    }
}
