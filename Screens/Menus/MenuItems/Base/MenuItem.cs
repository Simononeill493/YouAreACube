using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class MenuItem : IHasDrawLayer
    {
        public Point ActualLocation { get; private set; }
        protected (Point loc, CoordinateMode mode, bool centered) _locationConfig;

        public int Scale => MenuScreen.Scale + ScaleOffset;
        public int ScaleOffset = 0;

        public float DrawLayer { get; private set; }

        public event System.Action OnClick;
        public event System.Action OnMouseStartHover;
        public event System.Action OnMouseEndHover;

        protected bool _clickedOn = false;
        protected bool _mousePressedOn = false;
        protected bool _mouseHovering = false;

        public bool Visible = true;

        protected List<MenuItem> _children = new List<MenuItem>();

        public MenuItem(IHasDrawLayer parentDrawLayer)
        {
            DrawLayer = parentDrawLayer.DrawLayer - 0.0001f;
        }

        public void Draw(DrawingInterface drawingInterface)
        {
            if(Visible)
            {
                _drawSelf(drawingInterface);
                _drawChildren(drawingInterface);
            }
        }
        protected virtual void _drawSelf(DrawingInterface drawingInterface) { }

        public virtual void Update(UserInput input)
        {
            var oldHoverState = _mouseHovering;
            var newHoverState = IsMouseOver(input);

            if(!oldHoverState & newHoverState)
            {
                OnMouseStartHover?.Invoke();
            }
            else if(oldHoverState & !newHoverState)
            {
                OnMouseEndHover?.Invoke();
            }

            _mouseHovering = newHoverState;

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
        public void RemoveChild(MenuItem item)
        {
            _children.Remove(item);
        }
        public void UpdateDrawLayer(float newLayer)
        {
            DrawLayer = newLayer;
            foreach (var child in _children)
            {
                child.UpdateDrawLayer(newLayer - 0.0001f);
            }
        }

        public void SetLocationConfig(int x, int y, CoordinateMode coordinateMode, bool centered = false)
        {
            SetLocationConfig(new Point(x, y), coordinateMode, centered);
        }
        public void SetLocationConfig(Point location, CoordinateMode coordinateMode, bool centered = false)
        {
            _locationConfig = (location, coordinateMode, centered);
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
            Point location = _locationConfig.loc;

            if (_locationConfig.mode == CoordinateMode.ParentOffset)
            {
                location = parentlocation + location;
            }
            else if (_locationConfig.mode == CoordinateMode.ParentRelative)
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
