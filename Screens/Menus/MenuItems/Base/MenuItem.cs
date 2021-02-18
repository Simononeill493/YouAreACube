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
        public Point ActualLocation;
        public int Scale => MenuScreen.Scale;
        protected (Point loc, CoordinateMode mode, bool centered, bool attachedToParent) _locationConfig;

        public event System.Action OnClick;
        protected bool _clickedOn = false;
        protected bool _mousePressedOn = false;
        protected bool _mouseHovering = false;

        protected List<MenuItem> _children = new List<MenuItem>();

        public float DrawLayer = DrawLayers.MenuBaseLayer;

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
                    _mousePressedOn = true;
                }
                else if(_mousePressedOn & input.LeftButton == ButtonState.Released)
                {
                    _clickedOn = true;
                }

                if (_clickedOn)
                {
                    OnClick?.Invoke();
                    _clickedOn = false;
                }
            }
            else
            {
                _mousePressedOn = false;
            }

            _updateChildren(input);
        }
       
        public void AddChild(MenuItem item)
        {
            _children.Add(item);
        }

        public void SetLocationConfig(int x, int y, CoordinateMode coordinateMode, bool centered = false, bool attachedToParent = false)
        {
            SetLocationConfig(new Point(x, y), coordinateMode, centered, attachedToParent);
        }
        public void SetLocationConfig(Point location, CoordinateMode coordinateMode, bool centered = false, bool attachedToParent = false)
        {
            _locationConfig = (location, coordinateMode, centered, attachedToParent);
        }

        public virtual void UpdateThisAndChildLocations(Point parentlocation, Point parentSize)
        {
            UpdateLocation(parentlocation,parentSize);
            foreach (var child in _children)
            {
                var size = GetSize();
                child.UpdateThisAndChildLocations(ActualLocation,size);
            }
        }
        public void UpdateLocation(Point parentlocation, Point parentSize)
        {
            if(_locationConfig.attachedToParent)
            {
                _updateLocation_hasParent(parentlocation,parentSize);
            }
            else
            {
                _updateLocation_noParent();
            }
        }

        private void _updateLocation_hasParent(Point parentlocation, Point parentSize)
        {
            Point location = _locationConfig.loc;

            if (_locationConfig.mode == CoordinateMode.Absolute)
            {
                location = parentlocation + location;
            }
            else if (_locationConfig.mode == CoordinateMode.Relative)
            {
                int widthPercent = (int)(parentSize.X * (location.X / 100.0));
                int heightPercent = (int)(parentSize.Y * (location.Y / 100.0));
                var percentageOffset = new Point(widthPercent, heightPercent);

                location = parentlocation + percentageOffset;
            }

            if (_locationConfig.centered)
            {
                location = location - (GetSize() / 2);
            }

            ActualLocation = location;
        }
        private void _updateLocation_noParent()
        {
            Point location = _locationConfig.loc;

            if (_locationConfig.mode == CoordinateMode.Relative)
            {
                location = DrawUtils.ScreenPercentageToCoords(location);
            }
            if (_locationConfig.centered)
            {
                location = location - (GetSize() / 2);
            }

            ActualLocation = location;
        }

        public abstract Point GetSize();
        public abstract bool IsMouseOver(UserInput input);

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
