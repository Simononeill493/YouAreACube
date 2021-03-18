using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class TitleScreen : MenuScreen
    {
        public TitleScreen(Action<ScreenType> switchScreen) : base(ScreenType.Title,switchScreen)
        {
            Background = "TitleBackground";

            var newGameButton = new SpriteMenuItem(this, "NewGameMenu") 
            { 
                HighlightedSpriteName = "NewGameMenu_Highlight"
            };

            var loadGameButton = new SpriteMenuItem(this, "LoadGameMenu") 
            { 
                HighlightedSpriteName = "LoadGameMenu_Highlight"
            };

            newGameButton.OnMouseReleased += (i)=>GoToNewGame();
            loadGameButton.OnMouseReleased += (i) => GoToLoadGame();

            newGameButton.SetLocationConfig(50, 25, CoordinateMode.ParentPercentageOffset, centered: true);
            loadGameButton.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);

            _addMenuItem(newGameButton);
            _addMenuItem(loadGameButton);
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
