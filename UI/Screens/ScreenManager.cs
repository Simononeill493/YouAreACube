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
        }

        public void SwitchScreen(ScreenType screenType)
        {
            MenuScreen.UserDragging = false;

            switch (screenType)
            {
                case ScreenType.Title:
                    CurrentScreen = new TitleScreen(SwitchScreen);
                    break;
                case ScreenType.NewGame:
                    CurrentScreen = new NewGameScreen(SwitchScreen);
                    break;
                case ScreenType.LoadGame:
                    CurrentScreen = new LoadGameScreen(SwitchScreen, LoadSaveToScreen);
                    break;
                case ScreenType.Game:
                    if(CurrentGame==null)
                    {
                        Console.WriteLine("Warning: tried to open the game screen but no game was loaded.");
                    }
                    else
                    {
                        CurrentScreen = CurrentGame;
                    }
                    break;
                case ScreenType.TemplateExplorer:
                    CurrentScreen = new TemplateExplorerScreen(SwitchScreen, OpenTemplateForEditing, CurrentGame);
                    break;
            }
        }

        public void LoadSaveToScreen(Save save)
        {
            CurrentGame = new GameScreen(SwitchScreen, save);
        }

        public void OpenTemplateForEditing(BlockTemplate template)
        {
            CurrentScreen = new TemplateEditScreen(SwitchScreen, CurrentGame, template);
        }

        public void Update(UserInput input)
        {
            if (!Initialized) 
            {
                Console.WriteLine("Warning: screenManager updating before initialization");
                return; 
            }

            if(input.IsKeyJustReleased(Keys.PageUp))
            {
                var save = SaveManager.LoadFromFile("test.cubesave");

                LoadSaveToScreen(save);
                SwitchScreen(ScreenType.Game);
            }

            CurrentScreen.Update(input);
        }

        public void Draw(DrawingInterface drawingInterface)
        {
            if (!Initialized)
            {
                Console.WriteLine("Warning: screenManager drawing before initialization");
                return;
            }

            CurrentScreen.Draw(drawingInterface);
        }
    }

    public enum ScreenType
    {
        Title,
        NewGame,
        LoadGame,
        Game,
        TemplateExplorer,
        TemplateEdit
    }
    
}