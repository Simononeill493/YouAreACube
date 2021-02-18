﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    abstract class MenuScreen : Screen
    {
        public static int Scale = Config.MenuItemScale;
        public string Background;

        private List<MenuItem> _menuItems = new List<MenuItem>();
        private Point _currentScreenDimensions;

        public MenuScreen(ScreenType screenType,Action<ScreenType> switchScreen) : base(screenType,switchScreen) 
        {
            _currentScreenDimensions = new Point(0, 0);
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
                _refreshAllItems();
            }
            if (input.IsKeyJustReleased(Keys.O))
            {
                Scale--;
                _refreshAllItems();
            }

            _refreshIfScreenSizeChanged();
        }

        protected void _addMenuItem(MenuItem item)
        {
            _menuItems.Add(item);
        }
        protected void _refreshAllItems()
        {
            foreach (var item in _menuItems)
            {
                item.RefreshDimensions();
            }
        }

        private void _refreshIfScreenSizeChanged()
        {
            var newScreenDimensions = new Point(MonoGameWindow.CurrentWidth, MonoGameWindow.CurrentHeight);
            if (newScreenDimensions != _currentScreenDimensions)
            {
                _refreshAllItems();
            }
            _currentScreenDimensions = newScreenDimensions;
        }
    }
}
