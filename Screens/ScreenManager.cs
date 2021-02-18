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
        public Screen CurrentScreen;
        public GameScreen CurrentGame;

        public ScreenManager()
        {
            if(false)
            {
                var save = SaveManager.LoadFromFile("test.cubesave");

                LoadSaveToScreen(save);
                SwitchScreen(ScreenType.Game);
            }
            else
            {
                CurrentScreen = new TitleScreen(SwitchScreen);
            }
        }

        public void SwitchScreen(ScreenType screenType)
        {
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