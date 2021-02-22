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
        public string WorldName => textBox.Text;
        private TextBoxMenuItem textBox;

        public NewGameScreen(Action<ScreenType> switchScreen) : base(ScreenType.NewGame, switchScreen)
        {
            Background = "TitleBackground";

            textBox = new TextBoxMenuItem(this, "test") { Typeable = true, Focused = true };
            var okButton = new SpriteMenuItem(this, "OkButton");
            var cancelButton = new SpriteMenuItem(this, "CancelButton");

            okButton.OnClick += NewGameClicked;
            cancelButton.OnClick += BackToTitleScreen;

            textBox.SetLocationConfig(50, 40, CoordinateMode.ParentPercentageOffset, centered: true);
            okButton.SetLocationConfig(25, 65, CoordinateMode.ParentPercentageOffset, centered: true);
            cancelButton.SetLocationConfig(65, 65, CoordinateMode.ParentPercentageOffset, centered: true);

            _addMenuItem(textBox);
            _addMenuItem(okButton);
            _addMenuItem(cancelButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

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
    }
}
