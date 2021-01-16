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
        //private Dictionary<int, MenuItem> _slots = new Dictionary<int, MenuItem>();
        private List<string> saves;

        public LoadGameScreen()
        {
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
                    Console.WriteLine(i + " " + saves[i]);
                    fileSlot.Clickable = true;
                    fileSlot.ClickAction = () => ClickSaveFile(cur);

                    fileSlot.HasText = true;
                    fileSlot.Text = Path.GetFileNameWithoutExtension(saves[i]);
                }

                MenuItems.Add(fileSlot);
                //_slots[i] = fileSlot;
            }
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            this.DrawBackgroundAndMenuItems(drawingInterface);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState, List<Keys> keysUp)
        {
            this.MenuScreenUpdate(mouseState, keyboardState);

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.CurrentScreen = new TitleScreen();
            }
        }

        public void ClickSaveFile(int saveNumber)
        {
            var savePath = saves[saveNumber];
            var save = SaveManager.LoadFromFile(savePath);

            ScreenManager.CurrentScreen = new GameScreen(save);
        }
    }
}
