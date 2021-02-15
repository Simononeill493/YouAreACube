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
        public TitleScreen(Action<ScreenType> switchScreen) : base(switchScreen)
        {
            Background = "TitleBackground";

            var newGameButton = new SpriteMenuItem() 
            { 
                SpriteName = "NewGameMenu", 
                HighlightedSpriteName = "NewGameMenu_Highlight",
            };

            var loadGameButton = new SpriteMenuItem() 
            { 
                SpriteName = "LoadGameMenu", 
                HighlightedSpriteName = "LoadGameMenu_Highlight",
            };

            newGameButton.OnClick += GoToNewGame;
            loadGameButton.OnClick += GoToLoadGame;

            newGameButton.SetLocationConfig(50, 25, CoordinateMode.Relative);
            loadGameButton.SetLocationConfig(50, 50, CoordinateMode.Relative);

            AddMenuItem(newGameButton);
            AddMenuItem(loadGameButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
        }

        public void GoToNewGame()
        {
            SwitchScreen(ScreenType.NewGame);
        }

        public void GoToLoadGame()
        {
            SwitchScreen(ScreenType.LoadGame);
        }
    }
}
