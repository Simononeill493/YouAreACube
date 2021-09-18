using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(Action<ScreenType> switchScreen) : base(ScreenType.MainMenu,switchScreen)
        {
            Background = MenuSprites.TitleBackground;

            var newGameButton = new SpriteMenuItem(this, MenuSprites.MainMenuNewGameButton) { HighlightedSpriteName = MenuSprites.MainMenuNewGameButton_Highlighted };
            var loadGameButton = new SpriteMenuItem(this, MenuSprites.MainMenuLoadGameButton) { HighlightedSpriteName = MenuSprites.MainMenuLoadGameButton_Highlighted };

            newGameButton.OnMouseReleased += (i) => GoToNewGame();
            loadGameButton.OnMouseReleased += (i) => GoToLoadGame();

            newGameButton.SetLocationConfig(50, 25, CoordinateMode.ParentPercentage, centered: true);
            loadGameButton.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, centered: true);

            _addMenuItem(newGameButton);
            _addMenuItem(loadGameButton);
        }

        public void GoToNewGame() => SwitchScreen(ScreenType.NewGame);
        public void GoToLoadGame() => SwitchScreen(ScreenType.LoadGame);
    }
}
