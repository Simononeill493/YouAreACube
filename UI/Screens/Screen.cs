using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class Screen : IHasDrawLayer
    {
        public static int UniversalScale
        {
            get => _universalScale;
            set
            {
                if (value < 2) { value = 2; }
                _universalScale = value;
            }
        }
        private static int _universalScale = Config.DefaultMenuItemScale;
        public static ScreenItem DraggedItem = null;
        public static bool IsUserDragging => (DraggedItem != null);

        public readonly ScreenType ScreenType;
        public Action<ScreenType> SwitchScreen;

        public float DrawLayer { get; } = DrawLayers.MenuBaseLayer;
        public string Background;
        protected event Action<IntPoint> OnScreenSizeChanged;
        public IntPoint _currentScreenDimensions;
        protected bool _manualResizeEnabled = true;

        public List<(Keys, Action<UserInput>)> _keysJustReleasedEvents;
        public List<(Keys, Action<UserInput>)> _keysJustPressedEvents;
        private List<ScreenItem> _menuItems = new List<ScreenItem>();

        public Screen(ScreenType screenType,Action<ScreenType> switchScreen)
        {
            ScreenType = screenType;
            SwitchScreen = switchScreen;
            _keysJustReleasedEvents = new List<(Keys, Action<UserInput>)>();
            _keysJustPressedEvents = new List<(Keys, Action<UserInput>)>();
            _currentScreenDimensions = MonoGameWindow.CurrentSize;

            _setScaleToReccomended();
        }

        public void Update(UserInput input)
        {
            foreach(var keyEvent in _keysJustReleasedEvents)
            {
                if(input.IsKeyJustReleased(keyEvent.Item1))
                {
                    keyEvent.Item2(input);
                }
            }

            foreach (var keyEvent in _keysJustPressedEvents)
            {
                if (input.IsKeyJustPressed(keyEvent.Item1))
                {
                    keyEvent.Item2(input);
                }
            }

            foreach (var item in _menuItems)
            {
                item.Update(input);
                item.UpdateLocationCascade(IntPoint.Zero, _currentScreenDimensions);
            }

            if (_manualResizeEnabled)
            {
                if (input.ScrollDirection == 1)
                {
                    UniversalScale += 2;
                    _updateAllItemPositions();
                }
                if (input.ScrollDirection == -1)
                {
                    if (UniversalScale > 2)
                    {
                        UniversalScale -= 2;
                        _updateAllItemPositions();
                    }
                }
            }

            _update(input);
            _checkScreenSizeChanged();
        }
        public virtual void _update(UserInput input) { }

        public virtual void Draw(DrawingInterface drawingInterface)
        {
            if (Background != null)
            {
                drawingInterface.DrawBackground(Background);
            }

            foreach (var item in _menuItems)
            {
                item.Draw(drawingInterface);
            }
        }

        public void AddKeyJustReleasedEvent(Keys key, Action<UserInput> action)
        {
            _keysJustReleasedEvents.Add((key, action));
        }

        public void AddKeyJustPressedEvent(Keys key, Action<UserInput> action)
        {
            _keysJustPressedEvents.Add((key, action));
        }

        private void _checkScreenSizeChanged()
        {
            if (MonoGameWindow.CurrentSize != _currentScreenDimensions)
            {
                _currentScreenDimensions = MonoGameWindow.CurrentSize;
                _setScaleToReccomended();
                _updateAllItemPositions();

                OnScreenSizeChanged?.Invoke(_currentScreenDimensions);
            }
        }

        protected virtual int _getReccomendedScale() => (_currentScreenDimensions.Min / 400) * 2;
        private void _setScaleToReccomended()
        {
            UniversalScale = _getReccomendedScale();
        }

        protected void _addMenuItem(ScreenItem item)
        {
            _menuItems.Add(item);
            item.ScaleProvider = new ScreenItemScaleProviderMenuScreen();
            item.UpdateLocationCascade(IntPoint.Zero, _currentScreenDimensions);
        }
        protected void _updateAllItemPositions() => _menuItems.ForEach(item => item.UpdateLocationCascade(IntPoint.Zero, _currentScreenDimensions));
    }
}
