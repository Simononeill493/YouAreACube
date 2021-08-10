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
        public string SaveName => textBox.Text;
        private TextBoxMenuItem textBox;

        public NewGameScreen(Action<ScreenType> switchScreen) : base(ScreenType.NewGame, switchScreen)
        {
            Background = BuiltInMenuSprites.TitleBackground;

            textBox = new TextBoxMenuItem(this, "test") { Editable = true, Focused = true };
            var okButton = new SpriteMenuItem(this, BuiltInMenuSprites.MainMenuOkButton);
            var cancelButton = new SpriteMenuItem(this, BuiltInMenuSprites.MainMenuCancelButton);

            okButton.OnMouseReleased += (i) => NewGameClicked();
            cancelButton.OnMouseReleased += (i) => BackToTitleScreen();

            textBox.SetLocationConfig(50, 40, CoordinateMode.ParentPercentageOffset, centered: true);
            okButton.SetLocationConfig(25, 65, CoordinateMode.ParentPercentageOffset, centered: true);
            cancelButton.SetLocationConfig(65, 65, CoordinateMode.ParentPercentageOffset, centered: true);

            _addMenuItem(textBox);
            _addMenuItem(okButton);
            _addMenuItem(cancelButton);
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
            var (kernel,world) = SaveManager.GenerateTestSave(SaveName);

            SaveManager.SaveKernel(kernel);
            SaveManager.SaveWorld(world);

            SwitchScreen(ScreenType.LoadGame);
        }

        public void BackToTitleScreen() => SwitchScreen(ScreenType.Title);
    }
}
