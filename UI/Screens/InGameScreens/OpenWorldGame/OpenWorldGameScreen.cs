using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class OpenWorldGameScreen : GameScreen
    {
        private SectorGenerator _sectorGenerator;

        public OpenWorldGameScreen(Action<ScreenType> switchScreen,Kernel kernel, World world) : base(ScreenType.OpenWorldGame,switchScreen,kernel,world)
        {
            _sectorGenerator = new SectorGenerator();

            AddKeyJustPressedEvent(Keys.Escape, (i) => _saveAndQuit());
        }

        public override void _update(UserInput input)
        {
            base._update(input);
            //_sectorGenerator.GenerateAdjacentSectors(Game.World);
        }

        private void _saveAndQuit()
        {
            var (kernel, world) = Game.SaveAndQuit();

            SaveManager.SaveKernel(kernel);
            SaveManager.SaveWorld(world);

            SwitchScreen(ScreenType.LoadGameOpenWorld);
        }
    }
}
