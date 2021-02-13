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
                OnClick = GoToNewGame
            };

            var loadGameButton = new SpriteMenuItem() 
            { 
                SpriteName = "LoadGameMenu", 
                HighlightedSpriteName = "LoadGameMenu_Highlight",
                OnClick = GoToLoadGame
            };

            _setMenuPosition(newGameButton,50, 25, PositioningMode.Relative);
            _setMenuPosition(loadGameButton,50, 50, PositioningMode.Relative);

            MenuItems.Add(newGameButton);
            MenuItems.Add(loadGameButton);
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
