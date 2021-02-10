using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class MenuItem
    {
        public string SpriteName;
        public int XPercentage;
        public int YPercentage;
        public int Scale = 1;

        public bool Hovering = false;

        public string HighlightedSpriteName;
        public bool Highlightable = false;

        public bool Clickable = false;
        public bool ClickedOn = false;
        public System.Action ClickAction;

        public bool HasText = false;
        public string Text;

        public void Update(UserInput input)
        {
            Hovering = IsMouseOver(input);

            if (Hovering)
            {
                if (input.LeftButton == ButtonState.Pressed)
                {
                    ClickedOn = true;
                }
                else if (input.LeftButton == ButtonState.Released & ClickedOn & Clickable)
                {
                    ClickAction();

                }
                else if (!Hovering)
                {
                    ClickedOn = false;
                }
            }

        }

        public bool IsMouseOver(UserInput input)
        {
            var rect = DrawingInterface.GetMenuItemRectangle(this);
            return rect.Contains(input.MouseX, input.MouseY);
        }
    }
}
