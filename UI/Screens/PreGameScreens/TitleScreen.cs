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
            Background = BuiltInMenuSprites.TitleBackground;

            var newGameButton = new SpriteMenuItem(this, BuiltInMenuSprites.MainMenuNewGameButton) { HighlightedSpriteName = BuiltInMenuSprites.MainMenuNewGameButton_Highlighted };
            var loadGameButton = new SpriteMenuItem(this, BuiltInMenuSprites.MainMenuLoadGameButton) { HighlightedSpriteName = BuiltInMenuSprites.MainMenuLoadGameButton_Highlighted };

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
