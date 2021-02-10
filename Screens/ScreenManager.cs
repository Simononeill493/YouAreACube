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
            CurrentScreen = new TitleScreen(SwitchScreen);
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
                    CurrentScreen = CurrentGame;
                    break;
                case ScreenType.TemplateExplorer:
                    CurrentScreen = new TemplateExplorerScreen(SwitchScreen,CurrentGame);
                    break;
            }
        }

        public void LoadSaveToScreen(Save save) 
        {
            CurrentGame = new GameScreen(SwitchScreen,save);
        }

        public void Update(UserInput input)
        {
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
        TemplateExplorer
    }
    
}