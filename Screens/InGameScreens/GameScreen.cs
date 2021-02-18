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
        public Game Game;
        private Camera _currentCamera;

        private AdminCamera _adminCamera;
        private DynamicCamera _dynamicCamera;

        public GameScreen(Action<ScreenType> switchScreen,Save save) : base(ScreenType.Game,switchScreen)
        {
            Game = new Game(save);

            _adminCamera = new AdminCamera();
            _dynamicCamera = new DynamicCamera(save.Kernel);
            _currentCamera = _dynamicCamera;
        }

        public override void Update(UserInput input)
        {
            _readKeys(input);

            Game.Update(input);
            _currentCamera.Update(input);
        }

        public override void Draw(DrawingInterface drawingInterface) => _currentCamera.Draw(drawingInterface,Game.World); 

        private void _readKeys(UserInput input)
        {
            if(input.IsKeyJustPressed(Keys.M))
            {
                _currentCamera = _adminCamera;
            }
            if (input.IsKeyJustPressed(Keys.N))
            {
                _currentCamera = _dynamicCamera;
            }
            if(input.IsKeyJustPressed(Keys.Escape))
            {
                var save = Game.SaveAndQuit();
                SaveManager.SaveToFile(save);
                SwitchScreen(ScreenType.LoadGame);
            }
            if (input.IsKeyJustPressed(Keys.Tab))
            {
                SwitchScreen(ScreenType.TemplateExplorer);
            }
        }
    }
}
