using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class OpenWorldGameView : GameView
    {
        private SectorGenerator _sectorGenerator;

        public OpenWorldGameView(IHasDrawLayer parent,Action<ScreenType> switchScreen,Kernel kernel, World world) : base(parent,switchScreen,kernel,world)
        {
            _sectorGenerator = new SectorGenerator();
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(input.IsKeyJustReleased(Keys.Escape))
            {
                _saveAndQuit();
            }
            //_sectorGenerator.GenerateAdjacentSectors(Game.World);
        }

        private void _saveAndQuit()
        {
            var (kernel, world) = Game.SaveAndQuit();

            SaveManager.SaveKernel(kernel);
            SaveManager.SaveWorld(world);

            _switchScreen(ScreenType.LoadGameOpenWorld);
        }
    }
}
