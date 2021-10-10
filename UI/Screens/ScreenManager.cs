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
            //CurrentScreen = new PreDemoScreen(SwitchScreen);
            CurrentScreen = new TitleScreen(SwitchScreen);

            Initialized = true;

            //SoundInterface.PlayWind();
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
            if (input.IsKeyJustReleased(Keys.PageDown))
            {
                CurrentScreen = new PreDemoScreen(SwitchScreen);
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
            Screen.DraggedItem = null;

            switch (screenType)
            {
                case ScreenType.Title:
                    CurrentScreen = new TitleScreen(SwitchScreen);
                    break;
                case ScreenType.MainMenu:
                    CurrentScreen = new MainMenuScreen(SwitchScreen);
                    break;
                case ScreenType.NewGameOpenWorld:
                    CurrentScreen = new NewGameScreen(SwitchScreen);
                    break;
                case ScreenType.LoadGameOpenWorld:
                    CurrentScreen = new LoadGameScreen(SwitchScreen, LoadOpenWorldGameScreen);
                    break;
                case ScreenType.OpenWorldGame:
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
                case ScreenType.DemoGame:
                    CurrentGame = new DemoGameScreen(SwitchScreen);
                    CurrentScreen = CurrentGame;
                    break;
                case ScreenType.PreDemo:
                    CurrentScreen = new PreDemoScreen(SwitchScreen);
                    break;


            }
        }
        public void LoadOpenWorldGameScreen(Kernel kernel,World world) => CurrentGame = new OpenWorldGameScreen(SwitchScreen, kernel, world);
        public void LoadTemplateEditScreen(CubeTemplate template) => CurrentScreen = new TemplateEditScreen(SwitchScreen, CurrentGame, template);


        private void _autoLoadTestWorld()
        {
            var kernel = SaveManager.LoadKernel("test");
            var world = SaveManager.LoadWorld("test");

            LoadOpenWorldGameScreen(kernel, world);
            SwitchScreen(ScreenType.OpenWorldGame);
        }

        private void _autoGenerateAndLoadTestWorld()
        {
            var (kernel,world) = SaveManager.GenerateTestSave();

            LoadOpenWorldGameScreen(kernel, world);
            SwitchScreen(ScreenType.OpenWorldGame);
        }

    }

}