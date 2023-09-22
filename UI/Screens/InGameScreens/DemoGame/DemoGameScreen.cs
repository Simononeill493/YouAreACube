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
            var (demoWorld1, demoPlayer) = WorldGen.GenerateDemoWorld1();

            var tutorialKernel = new Kernel();
            tutorialKernel.SetHost(demoPlayer);

            tutorialKernel.AddKnownBlock(BlockDataDatabase.BlockDataDict["Move"]);
            tutorialKernel.AddKnownBlock(BlockDataDatabase.BlockDataDict["IfKeyPressed"]);

            return new DemoGameScreen(switchScreen, tutorialKernel, demoWorld1);
        }

        public DemoGameScreen(Action<ScreenType> switchScreen,Kernel kernel,World demoWorld) : base(ScreenType.DemoGame, switchScreen, kernel,demoWorld)
        {
            AddKeyJustReleasedEvent(Keys.Escape, (i) => { SwitchScreen(ScreenType.MainMenu); });

            _playerCamera.SetScale(6);
        }

    }
}
