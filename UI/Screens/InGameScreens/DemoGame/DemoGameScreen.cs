using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DemoGameScreen : GameScreen
    {
        public DemoGameScreen(Action<ScreenType> switchScreen) : base(ScreenType.DemoGame, switchScreen, new Kernel(), WorldGen.GenerateDemoWorld())
        {
            AddKeyJustReleasedEvent(Keys.Escape, (i) => { SwitchScreen(ScreenType.MainMenu); });
        }

        protected override Game _generateGame(Kernel kernel, World world)
        {
            SaveManager.AddKernelToWorld(kernel, world);

            return base._generateGame(kernel, world);
        }
    }
}
