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
        public MainMenuScreen(Action<ScreenType> switchScreen) : base(ScreenType.MainMenu, switchScreen)
        {
            Background = MenuSprites.TitleBackground;
            AddKeyJustReleasedEvent(Keys.Escape, (i) => SwitchScreen(ScreenType.Title));

            //var title = new MainMenuScreenContainer(this,SwitchScreen);
            //title.SetLocationConfig(0, 0, CoordinateMode.Absolute, centered: false);
            //_addMenuItem(title);

            var mainMenuBox = new MainMenuBox(this,SwitchScreen);
            mainMenuBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, centered: true);
            _addMenuItem(mainMenuBox);
            mainMenuBox.MultiplyScale(2.0f);
        }
    }

    class MainMenuBox : SpriteScreenItem
    {
        public const string CursorBuddy = "CursorBuddy";
        public const string CursorBuddySad = "CursorBuddySad";

        private Action<ScreenType> _parentSwitchScreen;

        public MainMenuBox(MenuScreen parent,Action<ScreenType> parentSwitchScreen) : base(parent, MenuSprites.MainMenuBox)
        {
            _parentSwitchScreen = parentSwitchScreen;

            var demoButton = new SpriteScreenItem(this, MenuSprites.MainMenuDemoButton) { HighlightedSpriteName = MenuSprites.MainMenuDemoButton_Highlighted };
            var newGameButton = new SpriteScreenItem(this, MenuSprites.MainMenuNewGameButton) { HighlightedSpriteName = MenuSprites.MainMenuNewGameButton_Highlighted };
            var newGameGreyOut = new SpriteScreenItem(ManualDrawLayer.InFrontOf(newGameButton), MenuSprites.MainMenuGreyedOutButton);
            var loadGameButton = new SpriteScreenItem(this, MenuSprites.MainMenuLoadGameButton) { HighlightedSpriteName = MenuSprites.MainMenuLoadGameButton_Highlighted };
            var loadGameGreyOut = new SpriteScreenItem(ManualDrawLayer.InFrontOf(loadGameButton), MenuSprites.MainMenuGreyedOutButton);
            var cursorBuddy = new SpriteScreenItem(this, CursorBuddy) { VisualParent = demoButton };

            demoButton.OnMouseReleased += DemoButtonClicked;
            newGameButton.OnMouseReleased += NewGameButtonClicked;
            loadGameButton.OnMouseReleased += LoadGameButtonClicked;

            demoButton.OnMouseStartHover += (i) =>  _snapBuddyToButton(cursorBuddy,demoButton);
            newGameButton.OnMouseStartHover += (i) => _snapBuddyToButton(cursorBuddy, newGameButton);
            loadGameButton.OnMouseStartHover += (i) => _snapBuddyToButton(cursorBuddy, loadGameButton);

            demoButton.SetLocationConfig(35, 7, CoordinateMode.ParentPixel, centered: false);
            newGameButton.SetLocationConfig(35, 34, CoordinateMode.ParentPixel, centered: false);
            newGameGreyOut.SetLocationConfig(35, 34, CoordinateMode.ParentPixel, centered: false);
            loadGameButton.SetLocationConfig(35, 61, CoordinateMode.ParentPixel, centered: false);
            loadGameGreyOut.SetLocationConfig(35, 61, CoordinateMode.ParentPixel, centered: false);
            cursorBuddy.SetLocationConfig(-27, 1, CoordinateMode.VisualParentPixel, centered: false);

            AddChild(demoButton);
            AddChild(newGameButton);
            //AddChild(newGameGreyOut);
            AddChild(loadGameButton);
            //AddChild(loadGameGreyOut);
            AddChild(cursorBuddy);

            //demoButton.MultiplyScale(2.0f);
            //newGameButton.MultiplyScale(2.0f);
            //newGameGreyOut.MultiplyScale(2.0f);
            //loadGameButton.MultiplyScale(2.0f);
            //loadGameGreyOut.MultiplyScale(2.0f);
            //cursorBuddy.MultiplyScale(2.0f);
        }

        private void _snapBuddyToButton(SpriteScreenItem buddy,SpriteScreenItem item)
        {
            buddy.VisualParent = item;
        }

        public void DemoButtonClicked(UserInput i)
        {
            throw new NotImplementedException();
        }

        public void NewGameButtonClicked(UserInput i)
        {
            if(i.IsKeyDown(Keys.LeftShift))
            {
                _parentSwitchScreen(ScreenType.NewGame);
            }
        }

        public void LoadGameButtonClicked(UserInput i)
        {
            if (i.IsKeyDown(Keys.LeftShift))
            {
                _parentSwitchScreen(ScreenType.LoadGame);
            }
        }
    }
}
