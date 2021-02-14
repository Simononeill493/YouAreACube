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
        public Point LocationOnScreen { get; protected set; }
        public System.Action OnClick;

        public bool IsCentered;
        protected bool _mouseHovering = false;
        protected bool _clickedOn = false;

        protected (Point loc, CoordinateMode mode) _positioningConfig;
        protected List<MenuItem> _children = new List<MenuItem>();

        public virtual void Draw(DrawingInterface drawingInterface)
        {
            _drawChildren(drawingInterface);
        }
        public virtual void Update(UserInput input)
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

            UpdateLocation();
            _updateChildren(input);
        }
        public abstract bool IsMouseOver(UserInput input);
        public virtual void UpdateLocation()
        {
            switch (_positioningConfig.mode)
            {
                case CoordinateMode.Absolute:
                    LocationOnScreen = _positioningConfig.loc;
                    break;
                case CoordinateMode.Relative:
                    LocationOnScreen = DrawUtils.ScreenPercentageToCoords(_positioningConfig.loc);
                    break;
            }
        }

        public void AddChild(MenuItem item)
        {
            item.SetPositioningConfig(_positioningConfig.loc, _positioningConfig.mode);
            _children.Add(item);
        }

        public void SetPositioningConfig(int x, int y, CoordinateMode positioningMode)
        {
            SetPositioningConfig(new Point(x, y), positioningMode);
        }
        public void SetPositioningConfig(Point pos, CoordinateMode positioningMode)
        {
            _positioningConfig = (pos, positioningMode);
        }
        public void UpdateThisAndChildLocations()
        {
            UpdateLocation();
            foreach (var child in _children)
            {
                child.UpdateThisAndChildLocations();
            }
        }

        private void _updateChildren(UserInput input)
        {
            foreach(var child in _children)
            {
                child.Update(input);
            }
        }
        private void _drawChildren(DrawingInterface drawingInterface)
        {
            foreach (var child in _children)
            {
                child.Draw(drawingInterface);
            }
        }
    }
}
