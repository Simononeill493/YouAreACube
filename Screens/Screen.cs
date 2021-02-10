using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class Screen
    {
        protected Action<ScreenType> SwitchScreen;
        public Screen(Action<ScreenType> switchScreen)
        {
            SwitchScreen = switchScreen;
        }

        public abstract void Update(UserInput input);
        public abstract void Draw(DrawingInterface drawingInterface);
    }
}
