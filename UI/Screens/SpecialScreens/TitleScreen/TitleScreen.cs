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
        public TitleScreen(Action<ScreenType> switchScreen) : base(ScreenType.Title, switchScreen)
        {
            _manualResizeEnabled = false;

            var title = new TitleScreenAnimationMenuItem(this,()=>(_currentScreenDimensions/Scale));
            title.SetLocationConfig(0, 0, CoordinateMode.Absolute, centered: false);
            _addMenuItem(title);

            OnScreenSizeChanged += (s) => title.SetScatteredFloaters();
            title.SetScatteredFloaters();

            AddKeyJustReleasedEvent(Keys.Enter,(i)=>SwitchScreen(ScreenType.MainMenu));
        }

        protected override int _getReccomendedScale() => Math.Max(base._getReccomendedScale() * 3, 4);
    }
}
