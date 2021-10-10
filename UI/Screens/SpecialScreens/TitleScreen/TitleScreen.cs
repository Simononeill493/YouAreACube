using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class TitleScreen : Screen
    {
        public TitleScreen(Action<ScreenType> switchScreen) : base(ScreenType.Title, switchScreen)
        {
            _scrollButtonScaleEnabled = false;

            var title = new TitleScreenAnimationMenuItem(this);
            title.SetLocationConfig(0, 0, CoordinateMode.Absolute, centered: false);
            _addMenuItem(title);

            OnScreenSizeChanged += (s) => title.SetScatteredFloaters();
            title.SetScatteredFloaters();

            AddKeyJustReleasedEvent(Keys.Enter,(i)=>GoToMainMenu());
        }

        public override void _update(UserInput input)
        {
            base._update(input);
            if(input.MouseLeftJustReleased)
            {
                GoToMainMenu();
            }
        }

        public void GoToMainMenu() => SwitchScreen(ScreenType.MainMenu);

        protected override int _getReccomendedScale() => Math.Max(base._getReccomendedScale() * 3, 4);
    }
}
