using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class NewGameScreen : MenuScreen
    {
        public NewGameScreen()
        {
            Background = "Grass";
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            this.DrawBackgroundAndMenuItems(drawingInterface);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            this.MenuScreenUpdate(mouseState, keyboardState);

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.CurrentScreen = new TitleScreen();
            }

        }
    }
}
