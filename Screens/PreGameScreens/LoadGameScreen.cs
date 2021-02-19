using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class LoadGameScreen : MenuScreen
    {
        private List<string> saves;
        private Action<Save> _loadSaveToScreen;

        public LoadGameScreen(Action<ScreenType> switchScreen,Action<Save> loadSaveToScreen) : base(ScreenType.LoadGame,switchScreen)
        {
            _loadSaveToScreen = loadSaveToScreen;

            Background = "TitleBackground";
            var files = Directory.GetFiles(Config.SaveDirectory);
            saves = files.Where(s => s.Contains(Config.SaveExtension)).ToList();

            for(int i=0;i<4;i++)
            {
                var cur = i;
                string fileName = "";
                if (saves.Count() > i)
                {
                    fileName = Path.GetFileNameWithoutExtension(saves[i]);
                }

                var fileSlot = new TextBoxMenuItem(this,fileName);
                fileSlot.SetLocationConfig(50, 15 + (i * 15), CoordinateMode.ParentRelative, centered: true);
                fileSlot.OnClick += () => ClickSaveFile(cur);

                _addMenuItem(fileSlot);
            }
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
                SwitchScreen(ScreenType.Title);
            }
        }

        public void ClickSaveFile(int saveNumber)
        {
            var savePath = saves[saveNumber];
            var save = SaveManager.LoadFromFile(savePath);

            _loadSaveToScreen(save);
            SwitchScreen(ScreenType.Game);
        }
    }
}
