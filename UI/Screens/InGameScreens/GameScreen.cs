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

            AddKeyJustPressedEvent(Keys.M, (i) => _currentCamera = _adminCamera);
            AddKeyJustPressedEvent(Keys.N, (i) => _currentCamera = _playerCamera);
            AddKeyJustPressedEvent(Keys.Tab, (i) => SwitchScreen(ScreenType.TemplateExplorer));
            AddKeyJustPressedEvent(Keys.Escape, (i) => _saveAndQuit());
        }

        public override void _update(UserInput input)
        {
            base._update(input);
            _currentCamera.AssignMouseHover(input, Game.World);

            Game.Update(input);
            _currentCamera.Update(input);
        }
        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            _currentCamera.Draw(drawingInterface, Game.World);
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
