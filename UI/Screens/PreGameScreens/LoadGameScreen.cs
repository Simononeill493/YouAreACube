using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class LoadGameScreen : MenuScreen
    {
        private List<string> _saves;
        private Action<Kernel, World> _loadSaveToScreen;

        public LoadGameScreen(Action<ScreenType> switchScreen,Action<Kernel, World> loadSaveToScreen) : base(ScreenType.LoadGame,switchScreen)
        {
            _loadSaveToScreen = loadSaveToScreen;
            Background = MenuSprites.TitleBackground;

            _saves = Directory.GetFiles(ConfigFiles.SaveDirectory).Where(s => s.Contains(ConfigFiles.SaveWorldExtension)).ToList();
            _generateTextBoxes();

            AddKeyJustReleasedEvent(Keys.Escape, (i) => SwitchScreen(ScreenType.MainMenu));
        }

        public void ClickSaveFile(int saveNumber)
        {
            var name = Path.GetFileNameWithoutExtension(_saves[saveNumber]);

            var world = SaveManager.LoadWorld(name);
            var kernel = SaveManager.LoadKernel(world.Name);

            _loadSaveToScreen(kernel,world);
            SwitchScreen(ScreenType.Game);
        }

        private void _generateTextBoxes()
        {
            for (int i = 0; i < 4; i++)
            {
                var cur = i;
                string fileName = "";
                if (_saves.Count() > i)
                {
                    fileName = Path.GetFileNameWithoutExtension(_saves[i]);
                }

                var fileSlot = new TextBoxMenuItem(this, fileName);
                fileSlot.SetLocationConfig(50, 15 + (i * 15), CoordinateMode.ParentPercentage, centered: true);
                fileSlot.OnMouseReleased += (input) => ClickSaveFile(cur);

                _addMenuItem(fileSlot);
            }
        }

    }
}
