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

        public void MenuScreenUpdate(UserInput input)
        {
            foreach (var item in MenuItems)
            {
                item.Hovering = _isMouseOverItem(input, item);

                if (item.Hovering)
                {
                    if (input.LeftButton == ButtonState.Pressed)
                    {
                        item.ClickedOn = true;
                    }
                    else if (input.LeftButton == ButtonState.Released & item.ClickedOn & item.Clickable)
                    {
                        item.ClickAction();

                    }
                    else if (!item.Hovering)
                    {
                        item.ClickedOn = false;
                    }

                }
            }
        }

        private bool _isMouseOverItem(UserInput input,MenuItem menuItem)
        {
            var rect = DrawingInterface.GetMenuItemRectangle(menuItem);
            return rect.Contains(input.MouseX, input.MouseY);
        }
    }
}
