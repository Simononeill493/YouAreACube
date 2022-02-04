using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TutorialGameScreen : GameScreen
    {
        public static TutorialGameScreen Generate(Action<ScreenType> switchScreen)
        {
            var (tutorialWorld, tutorialPlayer) = WorldGen.GenerateTutorialWorld();
            var tutorialKernel = new Kernel();
            tutorialKernel.SetHost(tutorialPlayer);

            tutorialKernel.AddKnownBlock(BlockDataDatabase.BlockDataDict["Move"]);
            tutorialKernel.AddKnownBlock(BlockDataDatabase.BlockDataDict["IfKeyPressed"]);

            return new TutorialGameScreen(switchScreen, tutorialKernel, tutorialWorld);
        }

        public TutorialGameScreen(Action<ScreenType> switchScreen,Kernel kernel,World demoWorld) : base(ScreenType.DemoGame, switchScreen, kernel,demoWorld)
        {
            AddKeyJustReleasedEvent(Keys.Escape, (i) => { SwitchScreen(ScreenType.MainMenu); });

            _playerCamera.SetScale(6);
        }

    }
}
