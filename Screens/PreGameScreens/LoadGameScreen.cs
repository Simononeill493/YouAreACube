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
        private Action<Save> _loadSaveToScreen;

        public LoadGameScreen(Action<ScreenType> switchScreen,Action<Save> loadSaveToScreen) : base(switchScreen)
        {
            _loadSaveToScreen = loadSaveToScreen;

            Background = "TitleBackground";
            var files = Directory.GetFiles(Config.SaveDirectory);
            saves = files.Where(s => s.Contains(Config.SaveExtension)).ToList();

            for(int i=0;i<4;i++)
            {
                var fileSlot = new MenuItem()
                {
                    SpriteName = "EmptyMenuRectangleMedium",
                    XPercentage = 50,
                    YPercentage = 15+(i*15),
                    Scale = 3,
                };

                var cur = i;

                if(saves.Count()>i)
                {
                    Console.WriteLine("Fetched save " + i + ":\t" + saves[i]);
                    fileSlot.Clickable = true;
                    fileSlot.ClickAction = () => ClickSaveFile(cur);

                    fileSlot.HasText = true;
                    fileSlot.Text = Path.GetFileNameWithoutExtension(saves[i]);
                }

                MenuItems.Add(fileSlot);
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