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
        private Camera _camera;
        private Game _game;

        public GameScreen(Save save)
        {
            _camera = new Camera(save.World);
            _game = new Game(save);
        }

        public override void Update(UserInput input)
        {
            _game.Update(input);
            _camera.Update(input);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            _camera.Draw(drawingInterface);
        }
    }
}
