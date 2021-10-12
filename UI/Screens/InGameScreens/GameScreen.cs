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
        public GameHolder _gameHolder;
        protected FixedCamera _adminCamera;
        protected KernelTrackingCamera _playerCamera;

        public GameScreen(ScreenType screenType,Action<ScreenType> switchScreen, Kernel kernel, World world) : base(screenType, switchScreen)
        {
            _scrollButtonScaleEnabled = false;

            _adminCamera = new FixedCamera(kernel,world);
            _playerCamera = new KernelTrackingCamera(kernel,world);

            _gameHolder = new GameHolder(this, () => _currentScreenDimensions);
            _gameHolder.SetLocationConfig(0,0, CoordinateMode.ParentAbsolute, false);
            _addMenuItem(_gameHolder);

            _gameHolder.Camera = _playerCamera;
            _gameHolder.Game = _generateGame(kernel, world);

#if DEBUG
            //_currentCamera = _adminCamera;
            CameraConfiguration.DebugMode = true;
#endif
            AddKeyJustPressedEvent(Keys.M, (i) => _gameHolder.Camera = _adminCamera);
            AddKeyJustPressedEvent(Keys.N, (i) => _gameHolder.Camera = _playerCamera);
            AddKeyJustPressedEvent(Keys.Tab, (i) => SwitchScreen(ScreenType.TemplateExplorer));
            AddKeyJustPressedEvent(Keys.Space, (i) => _gameHolder.Paused = !_gameHolder.Paused);
            AddKeyJustPressedEvent(Keys.OemPeriod, _gameHolder.MoveFrameWhilePaused);
        }

        protected virtual Game _generateGame(Kernel kernel, World world) => new Game(kernel, world);
    }
}
