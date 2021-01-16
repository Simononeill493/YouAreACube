using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class GameScreen : Screen
    {
        public Save Save;

        public GameScreen(Save save)
        {
            Save = save;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawBackground("Grass");
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState,List<Keys> keysUp)
        {
        }
    }
}
