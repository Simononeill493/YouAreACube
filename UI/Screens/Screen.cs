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

        protected int _drawTimer;

        public Screen(ScreenType screenType,Action<ScreenType> switchScreen)
        {
            ScreenType = screenType;
            SwitchScreen = switchScreen;
        }

        public abstract void Update(UserInput input);
        public virtual void Draw(DrawingInterface drawingInterface)
        {
            _drawTimer++;
        }
    }
}
