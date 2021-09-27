using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class GameScreen : Screen
    {
        public Game Game;
        private bool _paused;

        protected Camera _currentCamera;
        protected FixedCamera _adminCamera;
        protected KernelTrackingCamera _playerCamera;

        public GameScreen(ScreenType screenType,Action<ScreenType> switchScreen, Kernel kernel, World world) : base(screenType, switchScreen)
        {
            Game = _generateGame(kernel, world);

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
            AddKeyJustPressedEvent(Keys.Space, (i) => _paused = !_paused);
            AddKeyJustPressedEvent(Keys.OemPeriod, _moveOneFrame);

            _paused = false;
        }

        protected virtual Game _generateGame(Kernel kernel, World world) => new Game(kernel, world);

        public override void _update(UserInput input)
        {
            base._update(input);
            _currentCamera.AssignMouseHover(input, Game.World);

            if (!_paused)
            {
                Game.Update(input);
            }
            _currentCamera.Update(input);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            _currentCamera.Draw(drawingInterface, Game.World);
        }

        protected void _moveOneFrame(UserInput input)
        {
            if(_paused)
            {
                _currentCamera.AssignMouseHover(input, Game.World);
                Game.Update(input);
            }
        }

    }
}
