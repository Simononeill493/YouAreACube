using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class TitleScreen : MenuScreen
    {

        public TitleScreen()
        {
            Background = "TitleBackground";
            var newGameButton = new MenuItem() { SpriteName = "NewGameMenu", XPos = 20, YPos = 20 };
            MenuItems.Add(newGameButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            this.DrawBackgroundAndMenuItems(drawingInterface);

            //drawingInterface.DrawGrid();
            //drawingInterface.DrawGrass(0, 0);
            //drawingInterface.DrawGrass(16, 16);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {

        }
    }
}
