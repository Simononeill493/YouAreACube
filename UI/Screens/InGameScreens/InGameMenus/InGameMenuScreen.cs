using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class InGameMenuScreen : MenuScreen
    {
        protected GameScreen _gameScreen;

        public InGameMenuScreen(ScreenType screenType,Action<ScreenType> switchScreen, GameScreen gameScreen) : base(screenType,switchScreen)
        {
            _gameScreen = gameScreen;

            AddKeyJustPressedEvent(Keys.Tab, (i) => _returnToGame());
        }

        protected void _returnToGame()
        {
            _gameScreen.Game.Kernel.UpdateCompanionTemplates();
            SwitchScreen(ScreenType.Game);
        }
    }
}
