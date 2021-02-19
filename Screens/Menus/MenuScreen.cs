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
        public float DrawLayer { get; } = DrawLayers.MenuBaseLayer;

        public static int Scale = Config.MenuItemScale;
        public string Background;

        private List<MenuItem> _menuItems = new List<MenuItem>();
        private Point _currentScreenDimensions;

        public MenuScreen(ScreenType screenType,Action<ScreenType> switchScreen) : base(screenType,switchScreen) 
        {
            _currentScreenDimensions = new Point(MonoGameWindow.CurrentWidth, MonoGameWindow.CurrentHeight);
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Background!=null)
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
            }

            if(input.IsKeyJustReleased(Keys.P))
            {
                Scale++;
                _updateAllItemPositions();
            }
            if (input.IsKeyJustReleased(Keys.O))
            {
                Scale--;
                _updateAllItemPositions();
            }

            _refreshIfScreenSizeChanged();
        }

        protected void _addMenuItem(MenuItem item)
        {
            _menuItems.Add(item);
            item.UpdateThisAndChildLocations(Point.Zero, _currentScreenDimensions);
        }

        protected void _updateAllItemPositions()
        {
            foreach (var item in _menuItems)
            {
                item.UpdateThisAndChildLocations(Point.Zero, _currentScreenDimensions);
            }
        }

        private void _refreshIfScreenSizeChanged()
        {
            var newScreenDimensions = new Point(MonoGameWindow.CurrentWidth, MonoGameWindow.CurrentHeight);
            if (newScreenDimensions != _currentScreenDimensions)
            {
                _currentScreenDimensions = newScreenDimensions;
                _updateAllItemPositions();
            }
        }
    }
}
