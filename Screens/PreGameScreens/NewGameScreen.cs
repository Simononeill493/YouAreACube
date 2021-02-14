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
        public string WorldName { get { return text.Text; } set { text.Text = value; } }
        private TextMenuItem text;

        public NewGameScreen(Action<ScreenType> switchScreen) : base(switchScreen)
        {
            Background = "TitleBackground";

            text = new TextMenuItem() { Text = "test" };
            
            var textBox = new SpriteMenuItem()
            {
                SpriteName = "EmptyMenuRectangleMedium",
            };

            var okButton = new SpriteMenuItem()
            {
                SpriteName = "OkButton",
                OnClick = NewGameClicked
            };

            var cancelButton = new SpriteMenuItem()
            {
                SpriteName = "CancelButton",
                OnClick = BackToTitleScreen
            };

            textBox.SetPositioningConfig(50, 40, CoordinateMode.Relative);
            okButton.SetPositioningConfig(25, 65, CoordinateMode.Relative);
            cancelButton.SetPositioningConfig(65, 65, CoordinateMode.Relative);
            textBox.AddChild(text);

            AddMenuItem(textBox);
            AddMenuItem(okButton);
            AddMenuItem(cancelButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
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
            save.Name = WorldName;
            SaveManager.SaveToFile(save);

            SwitchScreen(ScreenType.LoadGame);
        }

        public void BackToTitleScreen()
        {
            SwitchScreen(ScreenType.Title);
        }

        private bool _doTypeChar(Keys key) => (KeyUtils.IsAlphanumeric(key) && WorldName.Length < 9);
        private bool _doBackspace(Keys key) => (key == Keys.Back && WorldName.Length > 0);
    }
}
