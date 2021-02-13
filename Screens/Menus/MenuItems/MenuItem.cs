using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class MenuItem
    {
        public Point LocationOnScreen { get; private set; }
        public System.Action OnClick;

        public bool IsCentered;
        public string Text;

        protected bool _mouseHovering = false;
        protected bool _clickedOn = false;

        public void SetLocation(int x,int y,PositioningMode positioningMode)
        {
            SetLocation(new Point(x, y), positioningMode);
        }
        public void SetLocation(Point point,PositioningMode positioningMode)
        {
            switch (positioningMode)
            {
                case PositioningMode.Absolute:
                    LocationOnScreen = point;
                    break;
                case PositioningMode.Relative:
                    LocationOnScreen =  DrawUtils.ScreenPercentageToCoords(point);
                    break;
            }
        }

        public void Update(UserInput input)
        {
            _mouseHovering = IsMouseOver(input);

            if (_mouseHovering)
            {
                if (input.LeftButton == ButtonState.Pressed)
                {
                    _clickedOn = true;
                }
                else if (input.LeftButton == ButtonState.Released & _clickedOn & OnClick!=null)
                {
                    OnClick();
                }
            }
        }

        public abstract void Draw(DrawingInterface drawingInterface);

        public abstract bool IsMouseOver(UserInput input);
    }

}
