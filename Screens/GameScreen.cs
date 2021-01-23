using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class GameScreen : Screen
    {
        private Save _save;
        private Camera _camera;
        private TickCounter _tickCounter;

        public GameScreen(Save save)
        {
            _save = save;

            _camera = new Camera();
            _tickCounter = new TickCounter();
        }


        public override void Update(UserInput input)
        {
            _tickCounter.Tick();

            _save.World.Update(input, _tickCounter);
            _camera.Update(input);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            _camera.Draw(drawingInterface,_save.World.Current);
        }
    }
}
