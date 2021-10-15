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
        public static DemoGameScreen Generate(Action<ScreenType> switchScreen)
        {
            var (demoWorld, demoPlayer) = WorldGen.GenerateDemoWorld();
            var demoKernel = new Kernel();
            demoKernel.SetHost(demoPlayer);

            return new DemoGameScreen(switchScreen, demoKernel, demoWorld);
        }

        public DemoGameScreen(Action<ScreenType> switchScreen,Kernel kernel,World demoWorld) : base(ScreenType.DemoGame, switchScreen, kernel,demoWorld)
        {
            AddKeyJustReleasedEvent(Keys.Escape, (i) => { SwitchScreen(ScreenType.MainMenu); });

            _playerCamera.SetScale(3);
        }

    }
}
