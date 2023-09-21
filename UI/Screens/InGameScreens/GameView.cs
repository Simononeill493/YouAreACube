using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    //DELETEME

    /*abstract class GameView : ScreenItem
    {
        public Game Game;
        private bool _paused;

        protected Camera _currentCamera;
        protected FixedCamera _adminCamera;
        protected KernelTrackingCamera _playerCamera;

        protected Action<ScreenType> _switchScreen;
        public Func<IntPoint> GetSize;

        public GameView(IHasDrawLayer parent, Action<ScreenType> switchScreen, Kernel kernel, World world) : base(parent)
        {
            _switchScreen = switchScreen;

            Game = _generateGame(kernel, world);

            _adminCamera = new FixedCamera(kernel);
            _playerCamera = new KernelTrackingCamera(kernel);
            _currentCamera = _playerCamera;

#if DEBUG
            //_currentCamera = _adminCamera;
            CameraConfiguration.DebugMode = true;
#endif
            _paused = false;
        }

        protected virtual Game _generateGame(Kernel kernel, World world) => new Game(kernel, world);

        public override void Update(UserInput input)
        {
            base.Update(input);
            _keyShortcuts(input);
            _currentCamera.AssignMouseHover(input, Game.World);

            if (!_paused)
            {
                Game.Update(input);
            }
            _currentCamera.Update(input);
        }

        private void _keyShortcuts(UserInput input)
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
                _switchScreen(ScreenType.TemplateExplorer);
            }
            if (input.IsKeyJustPressed(Keys.Space))
            {
                _paused = !_paused;
            }
            if (input.IsKeyJustPressed(Keys.OemPeriod))
            {
                _moveOneFrame(input);
            }
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            _currentCamera.Draw(drawingInterface, Game.World);
        }

        protected void _moveOneFrame(UserInput input)
        {
            if (_paused)
            {
                _currentCamera.AssignMouseHover(input, Game.World);
                Game.Update(input);
            }
        }

        public override IntPoint GetBaseSize() => GetSize();

    }*/
}
