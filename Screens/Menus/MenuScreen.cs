using System;
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
        public List<MenuItem> MenuItems = new List<MenuItem>();

        public void DrawBackgroundAndMenuItems(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawBackground(Background);

            foreach(var item in MenuItems)
            {
                drawingInterface.DrawMenuItem(item);
            }
        }

        public void MenuScreenUpdate(MouseState mouseState, KeyboardState keyboardState)
        {
            foreach(var item in MenuItems)
            {
                if(_isMouseOverItem(mouseState,item))
                {
                    Console.WriteLine("Hovering over " + item.SpriteName);
                }
            }
        }

        private bool _isMouseOverItem(MouseState mouseState,MenuItem menuItem)
        {
            var rect = DrawingInterface.GetMenuItemRectangle(menuItem);
            return rect.Contains(mouseState.X, mouseState.Y);
        }
    }
}
