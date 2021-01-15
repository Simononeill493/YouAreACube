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
            var newGameButton = new MenuItem() { SpriteName = "NewGameMenu", XPercentage = 50, YPercentage = 25, Scale = 3};
            var loadGameButton = new MenuItem() { SpriteName = "LoadGameMenu", XPercentage = 50, YPercentage = 50, Scale = 3 };

            MenuItems.Add(newGameButton);
            MenuItems.Add(loadGameButton);

        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            this.DrawBackgroundAndMenuItems(drawingInterface);

            //drawingInterface.DrawSprite("grass",0,0);
            //drawingInterface.DrawSprite("grass", 5, 5);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            this.MenuScreenUpdate(mouseState, keyboardState);

        }
    }
}
