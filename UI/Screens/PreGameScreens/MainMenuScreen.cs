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
        public const string CursorBuddy = "CursorBuddy";
        public const string CursorBuddySad = "CursorBuddySad";

        public MainMenuScreen(Action<ScreenType> switchScreen) : base(ScreenType.MainMenu,switchScreen)
        {
            Background = MenuSprites.TitleBackground;

            var newGameGreyOut = new SpriteScreenItem(this, MenuSprites.MainMenuGreyedOutButton);
            var loadGameGreyOut = new SpriteScreenItem(this, MenuSprites.MainMenuGreyedOutButton);
            var demoButton = new SpriteScreenItem(this, MenuSprites.MainMenuDemoButton) { HighlightedSpriteName = MenuSprites.MainMenuDemoButton_Highlighted };
            var newGameButton = new SpriteScreenItem(this, MenuSprites.MainMenuNewGameButton) { HighlightedSpriteName = MenuSprites.MainMenuNewGameButton_Highlighted };
            var loadGameButton = new SpriteScreenItem(this, MenuSprites.MainMenuLoadGameButton) { HighlightedSpriteName = MenuSprites.MainMenuLoadGameButton_Highlighted };
            var cursorBuddy = new SpriteScreenItem(this, CursorBuddy);

            demoButton.OnMouseReleased += (i) => GoToDemo();
            newGameButton.OnMouseReleased += (i) => GoToNewGame();
            loadGameButton.OnMouseReleased += (i) => GoToLoadGame();

            demoButton.OnMouseStartHover += (i) => cursorBuddy.SetLocationConfig(15, 25, CoordinateMode.ParentPercentage, centered: true);
            newGameButton.OnMouseStartHover += (i) => cursorBuddy.SetLocationConfig(15, 50, CoordinateMode.ParentPercentage, centered: true);
            loadGameButton.OnMouseStartHover += (i) => cursorBuddy.SetLocationConfig(15, 75, CoordinateMode.ParentPercentage, centered: true);

            demoButton.SetLocationConfig(60, 25, CoordinateMode.ParentPercentage, centered: true);
            newGameButton.SetLocationConfig(60, 50, CoordinateMode.ParentPercentage, centered: true);
            newGameGreyOut.SetLocationConfig(60, 50, CoordinateMode.ParentPercentage, centered: true);
            loadGameButton.SetLocationConfig(60, 75, CoordinateMode.ParentPercentage, centered: true);
            loadGameGreyOut.SetLocationConfig(60, 75, CoordinateMode.ParentPercentage, centered: true);
            cursorBuddy.SetLocationConfig(15, 25, CoordinateMode.ParentPercentage, centered: true);

            _addMenuItem(demoButton);
            _addMenuItem(newGameButton);
            _addMenuItem(newGameGreyOut);
            _addMenuItem(loadGameButton);
            _addMenuItem(loadGameGreyOut);
            _addMenuItem(cursorBuddy);

            demoButton.MultiplyScale(2.0f);
            newGameButton.MultiplyScale(2.0f);
            newGameGreyOut.MultiplyScale(2.0f);
            loadGameButton.MultiplyScale(2.0f);
            loadGameGreyOut.MultiplyScale(2.0f);
            cursorBuddy.MultiplyScale(2.0f);

            AddKeyJustReleasedEvent(Keys.Escape, (i) => SwitchScreen(ScreenType.Title));
        }

        public void GoToDemo() => throw new NotImplementedException();
        public void GoToNewGame() => SwitchScreen(ScreenType.NewGame);
        public void GoToLoadGame() => SwitchScreen(ScreenType.LoadGame);
    }
}
