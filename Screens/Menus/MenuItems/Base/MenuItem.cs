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
        public Point Location;
        public int Scale => MenuScreen.Scale;
        protected (Point loc, CoordinateMode mode, bool centered) _locationConfig;

        public event System.Action OnClick;
        protected bool _clickedOn = false;
        protected bool _mousePressedOn = false;
        protected bool _mouseHovering = false;

        protected List<MenuItem> _children = new List<MenuItem>();

        protected float _layer = DrawLayers.MenuBaseLayer;

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
            item.SetLocation(_locationConfig.loc,_locationConfig.mode, _locationConfig.centered);
            _children.Add(item);
        }

        public void SetLocation(int x, int y, CoordinateMode coordinateMode, bool centered = false)
        {
            SetLocation(new Point(x, y), coordinateMode, centered);
        }
        public void SetLocation(Point location, CoordinateMode coordinateMode, bool centered = false)
        {
            _locationConfig = (location, coordinateMode, centered);
            if (coordinateMode == CoordinateMode.Relative)
            {
                location = DrawUtils.ScreenPercentageToCoords(location);
            }
            if(centered)
            {
                location = location - (GetSize() / 2);
            }
            Location = location;
        }

        public virtual void RefreshDimensions()
        {
            _refreshOwnDimensions();
            foreach (var child in _children)
            {
                child.RefreshDimensions();
            }
        }

        public abstract Point GetSize();
        public abstract bool IsMouseOver(UserInput input);

        protected void _refreshOwnDimensions()
        {
            SetLocation(_locationConfig.loc, _locationConfig.mode, _locationConfig.centered);
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
