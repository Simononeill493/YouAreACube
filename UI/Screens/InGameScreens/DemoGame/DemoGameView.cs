using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    //DELETEME

    /*class DemoGameView : GameView
    {
        public DemoGameView(IHasDrawLayer parent,Action<ScreenType> switchScreen) : base(parent, switchScreen, new Kernel(), WorldGen.GenerateDemoWorld())
        {
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (input.IsKeyJustReleased(Keys.Escape))
            {
                _switchScreen(ScreenType.MainMenu);
            }
        }

        protected override Game _generateGame(Kernel kernel, World world)
        {
            SaveManager.AddKernelToWorld(kernel, world);

            return base._generateGame(kernel, world);
        }
    }*/
}
