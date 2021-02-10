﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class TitleScreen : MenuScreen
    {
        public TitleScreen(Action<ScreenType> switchScreen) : base(switchScreen)
        {
            Background = "TitleBackground";

            var newGameButton = new MenuItem() 
            { 
                SpriteName = "NewGameMenu", 
                HighlightedSpriteName = "NewGameMenu_Highlight", 
                XPercentage = 50, 
                YPercentage = 25, 
                Scale = 3, 
                Highlightable = true, 

                Clickable = true,
                ClickAction = GoToNewGame
            };

            var loadGameButton = new MenuItem() 
            { 
                SpriteName = "LoadGameMenu", 
                HighlightedSpriteName = "LoadGameMenu_Highlight",
                XPercentage = 50,
                YPercentage = 50, 
                Scale = 3, 
                Highlightable = true,

                Clickable = true,
                ClickAction = GoToLoadGame
            };

            MenuItems.Add(newGameButton);
            MenuItems.Add(loadGameButton);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
        }

        public void GoToNewGame()
        {
            SwitchScreen(ScreenType.NewGame);
        }

        public void GoToLoadGame()
        {
            SwitchScreen(ScreenType.LoadGame);
        }
    }
}