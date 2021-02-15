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
        public string Background;
        private List<MenuItem> MenuItems = new List<MenuItem>();

        public int Scale = Config.MenuItemScale;

        public MenuScreen(Action<ScreenType> switchScreen) : base(switchScreen) {}

        public override void Draw(DrawingInterface drawingInterface)
        {
            if(Background!=null)
            {
                drawingInterface.DrawBackground(Background);
            }

            foreach (var item in MenuItems)
            {
                item.Draw(drawingInterface);
            }
        }

        public override void Update(UserInput input)
        {
            foreach (var item in MenuItems)
            {
                item.Update(input);
            }

            if(input.IsKeyJustReleased(Keys.P))
            {
                Scale++;
                _manuallyUpdateDimensions();
            }
            if (input.IsKeyJustReleased(Keys.O))
            {
                Scale--;
                _manuallyUpdateDimensions();
            }
        }

        protected void _manuallyUpdateDimensions()
        {
            foreach (var item in MenuItems)
            {
                item.UpdateThisAndChildDimensions(Scale);
            }
        }

        protected void _addMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
            item.UpdateThisAndChildDimensions(Scale);
        }

    }
}
