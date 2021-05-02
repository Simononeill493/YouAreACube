using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class Screen
    {
        public readonly ScreenType ScreenType;
        protected Action<ScreenType> SwitchScreen;

        public Screen(ScreenType screenType,Action<ScreenType> switchScreen)
        {
            ScreenType = screenType;
            SwitchScreen = switchScreen;
        }

        public abstract void Update(UserInput input);
        public abstract void Draw(DrawingInterface drawingInterface);
    }
}
