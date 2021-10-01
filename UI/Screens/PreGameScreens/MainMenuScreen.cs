using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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

        private SpriteScreenItem _cursorBuddy;
        private Action<ScreenType> _parentSwitchScreen;

        private RectangleMenuItem _underConstructionBox;

        public MainMenuBox(MenuScreen parent, Action<ScreenType> parentSwitchScreen) : base(parent, MenuSprites.MainMenuBox)
        {
            _parentSwitchScreen = parentSwitchScreen;

            var demoButton = new SpriteScreenItem(this, MenuSprites.MainMenuDemoButton) { HighlightedSpriteName = MenuSprites.MainMenuDemoButton_Highlighted };
            var newGameButton = new SpriteScreenItem(this, MenuSprites.MainMenuNewGameButton) { HighlightedSpriteName = MenuSprites.MainMenuNewGameButton_Highlighted };
            var newGameGreyOut = new SpriteScreenItem(ManualDrawLayer.InFrontOf(newGameButton), MenuSprites.MainMenuGreyedOutButton);
            var loadGameButton = new SpriteScreenItem(this, MenuSprites.MainMenuLoadGameButton) { HighlightedSpriteName = MenuSprites.MainMenuLoadGameButton_Highlighted };
            var loadGameGreyOut = new SpriteScreenItem(ManualDrawLayer.InFrontOf(loadGameButton), MenuSprites.MainMenuGreyedOutButton);
            _cursorBuddy = new SpriteScreenItem(this, CursorBuddy) { VisualParent = demoButton };

            demoButton.OnMouseReleased += DemoButtonClicked;
            newGameButton.OnMouseReleased += NewGameButtonClicked;
            loadGameButton.OnMouseReleased += LoadGameButtonClicked;

            demoButton.OnMouseStartHover += (i) => _snapBuddyToButton(demoButton);
            newGameButton.OnMouseStartHover += (i) => _snapBuddyToButton(newGameButton);
            loadGameButton.OnMouseStartHover += (i) => _snapBuddyToButton(loadGameButton);

            demoButton.SetLocationConfig(35, 7, CoordinateMode.ParentPixel, centered: false);
            newGameButton.SetLocationConfig(35, 34, CoordinateMode.ParentPixel, centered: false);
            newGameGreyOut.SetLocationConfig(35, 34, CoordinateMode.ParentPixel, centered: false);
            loadGameButton.SetLocationConfig(35, 61, CoordinateMode.ParentPixel, centered: false);
            loadGameGreyOut.SetLocationConfig(35, 61, CoordinateMode.ParentPixel, centered: false);
            _cursorBuddy.SetLocationConfig(-27, 1, CoordinateMode.VisualParentPixel, centered: false);

            AddChild(demoButton);
            AddChild(newGameButton);
            AddChild(newGameGreyOut);
            AddChild(loadGameButton);
            AddChild(loadGameGreyOut);
            AddChild(_cursorBuddy);

            _underConstructionBox = new RectangleMenuItem(this);
            _underConstructionBox.Color = new Color(37,35,35);
            _underConstructionBox.Size = new IntPoint(138, 9);
            _underConstructionBox.SetLocationConfig(3, 84, CoordinateMode.ParentPixel, false);
            AddChild(_underConstructionBox);

            var text = new TextMenuItem(ManualDrawLayer.InFrontOf(_underConstructionBox), () => "Under construction!");
            text.MultiplyScale(0.75f);
            text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, true);
            text.Color = new Color(152, 123, 94);
            _underConstructionBox.AddChild(text);
        }

        private void _snapBuddyToButton(SpriteScreenItem item)
        {
            _cursorBuddy.VisualParent = item;
            _cursorBuddy.SpriteName = CursorBuddy;
            _underConstructionBox.Visible = false;
        }

        public void DemoButtonClicked(UserInput i)
        {
            _parentSwitchScreen(ScreenType.PreDemo);
        }

        public void NewGameButtonClicked(UserInput i)
        {
            if(i.IsKeyDown(Keys.LeftShift))
            {
                _parentSwitchScreen(ScreenType.NewGameOpenWorld);
            }
            else
            {
                _cursorBuddy.SpriteName = CursorBuddySad;
                _underConstructionBox.Visible = true;
            }
        }

        public void LoadGameButtonClicked(UserInput i)
        {
            if (i.IsKeyDown(Keys.LeftShift))
            {
                _parentSwitchScreen(ScreenType.LoadGameOpenWorld);
            }
            else
            {
                _cursorBuddy.SpriteName = CursorBuddySad;
                _underConstructionBox.Visible = true;
            }
        }
    }
}
