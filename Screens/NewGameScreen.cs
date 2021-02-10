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
            base.Draw(drawingInterface);
            drawingInterface.DrawText(WorldName, 50, 40, 2);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            foreach(var key in input.KeysJustPressed)
            {
                if (_doTypeChar(key))
                {
                    WorldName = WorldName + KeyUtils.KeyToChar(key);
                }
                else if (_doBackspace(key))
                {
                    WorldName = WorldName.Substring(0, WorldName.Length - 1);
                }
            }

            if (input.IsKeyDown(Keys.Escape))
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

        private bool _doTypeChar(Keys key) => (KeyUtils.IsAlphanumeric(key) && WorldName.Length < 9);
        private bool _doBackspace(Keys key) => (key == Keys.Back && WorldName.Length > 0);
    }
}
