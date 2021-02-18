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
        public TitleScreen(Action<ScreenType> switchScreen) : base(ScreenType.Title,switchScreen)
        {
            Background = "TitleBackground";

            var newGameButton = new SpriteMenuItem("NewGameMenu") 
            { 
                HighlightedSpriteName = "NewGameMenu_Highlight"
            };

            var loadGameButton = new SpriteMenuItem("LoadGameMenu") 
            { 
                HighlightedSpriteName = "LoadGameMenu_Highlight"
            };

            newGameButton.OnClick += GoToNewGame;
            loadGameButton.OnClick += GoToLoadGame;

            newGameButton.SetLocation(50, 25, CoordinateMode.Relative, centered: true);
            loadGameButton.SetLocation(50, 50, CoordinateMode.Relative, centered: true);

            _addMenuItem(newGameButton);
            _addMenuItem(loadGameButton);
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
