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
        public string SaveName = "test";
        private TextBoxMenuItem textBox;

        public NewGameScreen(Action<ScreenType> switchScreen) : base(ScreenType.NewGame, switchScreen)
        {
            Background = MenuSprites.TitleBackground;

            textBox = new TextBoxMenuItem(this, ()=> SaveName, (s)=> { SaveName = s; }) { Editable = true, Focused = true };
            var okButton = new SpriteScreenItem(this, MenuSprites.MainMenuOkButton);
            var cancelButton = new SpriteScreenItem(this, MenuSprites.MainMenuCancelButton);

            okButton.OnMouseReleased += (i) => NewGameClicked();
            cancelButton.OnMouseReleased += (i) => BackToMainMenu();

            textBox.SetLocationConfig(50, 40, CoordinateMode.ParentPercentage, centered: true);
            okButton.SetLocationConfig(25, 65, CoordinateMode.ParentPercentage, centered: true);
            cancelButton.SetLocationConfig(65, 65, CoordinateMode.ParentPercentage, centered: true);

            _addMenuItem(textBox);
            _addMenuItem(okButton);
            _addMenuItem(cancelButton);

            AddKeyJustReleasedEvent(Keys.Escape, (i) => BackToMainMenu());
        }

        public void NewGameClicked()
        {
            var (kernel,world) = SaveManager.GenerateTestSave(SaveName);

            SaveManager.SaveKernel(kernel);
            SaveManager.SaveWorld(world);

            SwitchScreen(ScreenType.LoadGame);
        }

        public void BackToMainMenu() => SwitchScreen(ScreenType.MainMenu);
    }
}
