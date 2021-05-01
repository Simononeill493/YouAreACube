﻿using System;
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
        private Action<Kernel, World> _loadSaveToScreen;

        public LoadGameScreen(Action<ScreenType> switchScreen,Action<Kernel, World> loadSaveToScreen) : base(ScreenType.LoadGame,switchScreen)
        {
            _loadSaveToScreen = loadSaveToScreen;

            Background = "TitleBackground";
            var files = Directory.GetFiles(ConfigFiles.SaveDirectory);
            saves = files.Where(s => s.Contains(ConfigFiles.SaveWorldExtension)).ToList();

            for(int i=0;i<4;i++)
            {
                var cur = i;
                string fileName = "";
                if (saves.Count() > i)
                {
                    fileName = Path.GetFileNameWithoutExtension(saves[i]);
                }

                var fileSlot = new TextBoxMenuItem(this,fileName);
                fileSlot.SetLocationConfig(50, 15 + (i * 15), CoordinateMode.ParentPercentageOffset, centered: true);
                fileSlot.OnMouseReleased += (input) => ClickSaveFile(cur);

                _addMenuItem(fileSlot);
            }
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
            var name = Path.GetFileNameWithoutExtension(saves[saveNumber]);

            var world = SaveManager.LoadWorld(name);
            var kernel = SaveManager.LoadKernel(world.Name);

            _loadSaveToScreen(kernel,world);
            SwitchScreen(ScreenType.Game);
        }
    }
}
