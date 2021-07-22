using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ScreenManager
    {
        public bool Initialized = false;

        public Screen CurrentScreen;
        public GameScreen CurrentGame;

        public void Init()
        {
            CurrentScreen = new TitleScreen(SwitchScreen);
            Initialized = true;

            SoundInterface.PlayWind();
        }

        public void Update(UserInput input)
        {
            if (!Initialized)
            {
                Console.WriteLine("Warning: ScreenManager updating before initialization");
                return;
            }

            if (input.IsKeyJustReleased(Keys.PageUp))
            {
                _autoLoadTestWorld();
            }
            if (input.IsKeyJustReleased(Keys.Pause))
            {
                _autoGenerateAndLoadTestWorld();
            }


            CurrentScreen.Update(input);
        }
        public void Draw(DrawingInterface drawingInterface)
        {
            if (!Initialized)
            {
                Console.WriteLine("Warning: ScreenManager drawing before initialization");
                return;
            }

            CurrentScreen.Draw(drawingInterface);
        }

        public void SwitchScreen(ScreenType screenType)
        {
            MenuScreen.DraggedItem = null;

            switch (screenType)
            {
                case ScreenType.Title:
                    CurrentScreen = new TitleScreen(SwitchScreen);
                    break;
                case ScreenType.NewGame:
                    CurrentScreen = new NewGameScreen(SwitchScreen);
                    break;
                case ScreenType.LoadGame:
                    CurrentScreen = new LoadGameScreen(SwitchScreen, LoadGameScreen);
                    break;
                case ScreenType.Game:
                    if (CurrentGame == null)
                    {
                        Console.WriteLine("Warning: tried to open the game screen but no game was loaded.");
                    }
                    else
                    {
                        CurrentScreen = CurrentGame;
                    }
                    break;
                case ScreenType.TemplateExplorer:
                    CurrentScreen = new TemplateExplorerScreen(SwitchScreen, LoadTemplateEditScreen, CurrentGame);
                    break;
                case ScreenType.TemplateEdit:
                    throw new NotImplementedException("Tried to switch to template edit screen without loading a template");
            }
        }
        public void LoadGameScreen(Kernel kernel,World world) => CurrentGame = new GameScreen(SwitchScreen, kernel, world);
        public void LoadTemplateEditScreen(CubeTemplate template) => CurrentScreen = new TemplateEditScreen(SwitchScreen, CurrentGame, template);


        private void _autoLoadTestWorld()
        {
            var kernel = SaveManager.LoadKernel("test");
            var world = SaveManager.LoadWorld("test");

            LoadGameScreen(kernel, world);
            SwitchScreen(ScreenType.Game);
        }

        private void _autoGenerateAndLoadTestWorld()
        {
            var (kernel,world) = SaveManager.GenerateTestSave();

            LoadGameScreen(kernel, world);
            SwitchScreen(ScreenType.Game);
        }

    }

}