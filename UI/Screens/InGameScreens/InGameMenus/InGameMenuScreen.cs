using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class InGameMenuScreen : Screen
    {
        protected GameScreen _gameScreen;

        public InGameMenuScreen(ScreenType screenType,Action<ScreenType> switchScreen, GameScreen gameScreen) : base(screenType,switchScreen)
        {
            _gameScreen = gameScreen;

            AddKeyJustPressedEvent(Keys.Tab, (i) => _returnToGame());
        }

        protected void _returnToGame()
        {
            _gameScreen._gameHolder.Game.Kernel.UpdateCompanionTemplates();
            SwitchScreen(ScreenType.OpenWorldGame);
        }
    }
}
