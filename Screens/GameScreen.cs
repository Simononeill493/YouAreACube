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

        private GridCamera _adminCamera;
        private OffsetCamera _offsetCamera;
        private ClampToKernelCamera _kernelCamera;
        private DynamicCamera _dynamicCamera;

        public GameScreen(Save save)
        {
            _adminCamera = new GridCamera();
            _offsetCamera = new OffsetCamera();
            _kernelCamera = new ClampToKernelCamera(save.Kernel);
            _dynamicCamera = new DynamicCamera(save.Kernel);

            _camera = new GridCamera();
            _game = new Game(save);
        }

        public override void Update(UserInput input)
        {
            _readKeys(input);

            _game.Update(input);
            _camera.Update(input);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            _camera.Draw(drawingInterface,_game.World);
        }

        private void _readKeys(UserInput input)
        {
            if(input.IsKeyJustPressed(Keys.M))
            {
                _camera = _adminCamera;
            }
            if (input.IsKeyJustPressed(Keys.N))
            {
                _camera = _offsetCamera;
            }
            if (input.IsKeyJustPressed(Keys.B))
            {
                _camera = _kernelCamera;
            }
            if (input.IsKeyJustPressed(Keys.V))
            {
                _camera = _dynamicCamera;
            }

        }
    }
}
