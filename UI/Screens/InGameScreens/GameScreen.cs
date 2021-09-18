using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    public class GameScreen : Screen
    {
        public Game Game;

        private Camera _currentCamera;
        private FixedCamera _adminCamera;
        private KernelTrackingCamera _playerCamera;

        public GameScreen(Action<ScreenType> switchScreen,Kernel kernel, World world) : base(ScreenType.Game,switchScreen)
        {
            Game = new Game(kernel, world);

            _adminCamera = new FixedCamera(kernel);
            _playerCamera = new KernelTrackingCamera(kernel);

            _currentCamera = _playerCamera;

#if DEBUG
            //_currentCamera = _adminCamera;
            CameraConfiguration.DebugMode = true;
#endif
        }

        public override void Update(UserInput input)
        {
            _readKeys(input);
            _currentCamera.AssignMouseHover(input, Game.World);

            Game.Update(input);
            _currentCamera.Update(input);
        }
        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            _currentCamera.Draw(drawingInterface, Game.World);
        }

        private void _readKeys(UserInput input)
        {
            if(input.IsKeyJustPressed(Keys.M))
            {
                _currentCamera = _adminCamera;
            }
            if (input.IsKeyJustPressed(Keys.N))
            {
                _currentCamera = _playerCamera;
            }
            if (input.IsKeyJustPressed(Keys.Tab))
            {
                SwitchScreen(ScreenType.TemplateExplorer);
            }
            if (input.IsKeyJustPressed(Keys.Escape))
            {
                _saveAndQuit();
                return;
            }
        }
        private void _saveAndQuit()
        {
            var (kernel, world) = Game.SaveAndQuit();

            SaveManager.SaveKernel(kernel);
            SaveManager.SaveWorld(world);

            SwitchScreen(ScreenType.LoadGame);
        }
    }
}
