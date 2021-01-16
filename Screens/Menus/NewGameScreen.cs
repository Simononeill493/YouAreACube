using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class NewGameScreen : MenuScreen
    {
        public string WorldName = "test";

        public NewGameScreen()
        {
            Background = "TitleBackground";

            var textBox = new MenuItem()
            {
                SpriteName = "EmptyMenuRectangleMedium",
                XPercentage = 50,
                YPercentage = 40,
                Scale = 3,
            };

            var okButton = new MenuItem()
            {
                SpriteName = "OkButton",
                HighlightedSpriteName = "OkButton",
                XPercentage = 25,
                YPercentage = 65,
                Scale = 3,
                Highlightable = true,

                Clickable = true,
                ClickAction = NewGameClicked
            };

            var cancelButton = new MenuItem()
            {
                SpriteName = "CancelButton",
                HighlightedSpriteName = "CancelButton",
                XPercentage = 65,
                YPercentage = 65,
                Scale = 3,
                Highlightable = true,

                Clickable = true,
                ClickAction = BackToTitleScreen
            };

            MenuItems.Add(textBox);
            MenuItems.Add(okButton);
            MenuItems.Add(cancelButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            this.DrawBackgroundAndMenuItems(drawingInterface);
            drawingInterface.DrawText(WorldName, 50, 40, 2);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState, List<Keys> keysUp)
        {
            this.MenuScreenUpdate(mouseState, keyboardState);

            foreach(var key in keysUp)
            {
                if (KeyUtils.IsAlphanumeric(key) && WorldName.Length < 9)
                {
                    WorldName = WorldName + KeyUtils.KeyToChar(key);
                }
                else if (key == Keys.Back && WorldName.Length > 0)
                {
                    WorldName = WorldName.Substring(0, WorldName.Length - 1);
                }
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                BackToTitleScreen();
            }
        }

        public void NewGameClicked()
        {
            var save = SaveManager.FreshSave();
            SaveManager.SaveToFile(save, WorldName);

            ScreenManager.CurrentScreen = new LoadGameScreen();
        }

        public void BackToTitleScreen()
        {
            ScreenManager.CurrentScreen = new TitleScreen();
        }
    }
}
