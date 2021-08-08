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
                if(value<1) { value = 1; }
                _scale = value; 
            } 
        }
        private static int _scale = Config.DefaultMenuItemScale;

        public static DraggableMenuItem DraggedItem = null;
        public static bool IsUserDragging => (DraggedItem != null);

        public float DrawLayer { get; } = DrawLayers.MenuBaseLayer;

        public string Background;

        private List<MenuItem> _menuItems = new List<MenuItem>();
        private IntPoint _currentScreenDimensions;

        public MenuScreen(ScreenType screenType, Action<ScreenType> switchScreen) : base(screenType, switchScreen)
        {
            _currentScreenDimensions = MonoGameWindow.CurrentSize;
            _setScaleToReccomended();
        }

        private void _setScaleToReccomended()
        {
            var sc = (_currentScreenDimensions.Min / 400) * 2;
            if(sc<2) { sc = 2; }
            Scale = sc;
        }

        public override void Draw(DrawingInterface drawingInterface)
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
        public override void Update(UserInput input)
        {
            foreach (var item in _menuItems)
            {
                item.Update(input);
                item.UpdateDimensionsCascade(IntPoint.Zero, _currentScreenDimensions);
            }

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

            _refreshIfScreenSizeChanged();
        }

        protected void _addMenuItem(MenuItem item)
        {
            _menuItems.Add(item);
            item.UpdateDimensionsCascade(IntPoint.Zero, _currentScreenDimensions);
        }
        protected void _updateAllItemPositions() => _menuItems.ForEach(item => item.UpdateDimensionsCascade(IntPoint.Zero, _currentScreenDimensions));

        private void _refreshIfScreenSizeChanged()
        {
            if (MonoGameWindow.CurrentSize != _currentScreenDimensions)
            {
                _currentScreenDimensions = MonoGameWindow.CurrentSize;
                _setScaleToReccomended();
                _updateAllItemPositions();
            }
        }
    }
}
