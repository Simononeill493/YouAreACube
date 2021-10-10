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

        protected CameraHolder _currentCamera;
        protected FixedCamera _adminCamera;
        protected KernelTrackingCamera _playerCamera;

        public GameScreen(ScreenType screenType,Action<ScreenType> switchScreen, Kernel kernel, World world) : base(screenType, switchScreen)
        {
            _scrollButtonScaleEnabled = false;

            Game = _generateGame(kernel, world);

            _adminCamera = new FixedCamera(kernel,world);
            _playerCamera = new KernelTrackingCamera(kernel,world);

            _currentCamera = new CameraHolder(this, () => _currentScreenDimensions);
            _currentCamera.SetLocationConfig(0,0, CoordinateMode.ParentAbsolute, false);
            _addMenuItem(_currentCamera);

            _currentCamera.Camera = _playerCamera;

#if DEBUG
            //_currentCamera = _adminCamera;
            CameraConfiguration.DebugMode = true;
#endif
            AddKeyJustPressedEvent(Keys.M, (i) => _currentCamera.Camera = _adminCamera);
            AddKeyJustPressedEvent(Keys.N, (i) => _currentCamera.Camera = _playerCamera);
            AddKeyJustPressedEvent(Keys.Tab, (i) => SwitchScreen(ScreenType.TemplateExplorer));
            AddKeyJustPressedEvent(Keys.Space, (i) => _paused = !_paused);
            AddKeyJustPressedEvent(Keys.OemPeriod, _moveOneFrame);

            _paused = false;
        }

        protected virtual Game _generateGame(Kernel kernel, World world) => new Game(kernel, world);

        public override void _update(UserInput input)
        {
            base._update(input);
            if (!_paused)
            {
                Game.Update(input);
            }
        }


        protected void _moveOneFrame(UserInput input)
        {
            if(_paused)
            {
                _currentCamera.Update(input);
                Game.Update(input);
            }
        }

    }
}
