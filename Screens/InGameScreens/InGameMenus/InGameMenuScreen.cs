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
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            _gameScreen.Draw(drawingInterface);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustPressed(Keys.Tab))
            {
                SwitchScreen(ScreenType.Game);
            }
        }
    }
}
