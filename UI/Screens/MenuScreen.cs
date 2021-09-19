using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    public abstract class MenuScreen : Screen, IHasDrawLayer
    {
        public static int Scale { 
            get => _scale; 
            set 
            {
                if(value<2) { value = 2; }
                _scale = value; 
            } 
        }
        private static int _scale = Config.DefaultMenuItemScale;
        protected bool _manualResizeEnabled = true;

        public event Action<IntPoint> OnScreenSizeChanged;


        public static ScreenItem DraggedItem = null;
        public static bool IsUserDragging => (DraggedItem != null);

        public float DrawLayer { get; } = DrawLayers.MenuBaseLayer;

        public string Background;

        private List<ScreenItem> _menuItems = new List<ScreenItem>();
        protected IntPoint _currentScreenDimensions;

        public MenuScreen(ScreenType screenType, Action<ScreenType> switchScreen) : base(screenType, switchScreen)
        {
            _currentScreenDimensions = MonoGameWindow.CurrentSize;
            _setScaleToReccomended();
        }

        protected virtual int _getReccomendedScale() => (_currentScreenDimensions.Min / 400) * 2;
        private void _setScaleToReccomended()
        {
            Scale = _getReccomendedScale();
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
            if (Background != null)
            {
                drawingInterface.DrawBackground(Background);
            }

            foreach (var item in _menuItems)
            {
                item.Draw(drawingInterface);
            }
        }
        public override void Update(UserInput input)
        {
            foreach (var item in _menuItems)
            {
                item.Update(input);
                item.UpdateLocationCascade(IntPoint.Zero, _currentScreenDimensions);
            }

            if(_manualResizeEnabled)
            {
                if (input.ScrollDirection == 1)
                {
                    Scale += 2;
                    Console.WriteLine(Scale);
                    _updateAllItemPositions();
                }
                if (input.ScrollDirection == -1)
                {
                    if (Scale > 2)
                    {
                        Scale -= 2;
                        Console.WriteLine(Scale);
                        _updateAllItemPositions();
                    }
                }
            }

            _checkScreenSizeChanged();
        }

        protected void _addMenuItem(ScreenItem item)
        {
            _menuItems.Add(item);
            item.ScaleProvider = new ScreenItemScaleProviderMenuScreen();
            item.UpdateLocationCascade(IntPoint.Zero, _currentScreenDimensions);
        }
        protected void _updateAllItemPositions() => _menuItems.ForEach(item => item.UpdateLocationCascade(IntPoint.Zero, _currentScreenDimensions));

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
    }
}
