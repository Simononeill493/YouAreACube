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
        protected MenuItemDimensions Dimensions = new MenuItemDimensions();
        public int X => Dimensions.ActualLocation.X;
        public int Y => Dimensions.ActualLocation.Y;

        public event System.Action OnClick;
        protected List<MenuItem> _children = new List<MenuItem>();

        protected bool _mouseHovering = false;
        protected bool _mousePressedOn = false;
        protected bool _clickedOn = false;

        public virtual void Draw(DrawingInterface drawingInterface)
        {
            _drawChildren(drawingInterface);
        }
        public void Update(UserInput input)
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
                    OnClick();
                    _clickedOn = false;
                }
            }
            else
            {
                _mousePressedOn = false;
            }

            UpdateDimensions(Dimensions.Scale);
            _updateChildren(input);
        }
        public abstract bool IsMouseOver(UserInput input);
        public void AddChild(MenuItem item)
        {
            item.Dimensions = Dimensions.ShallowCopy();
            _children.Add(item);
        }

        public void UpdateThisAndChildDimensions(int scale)
        {
            UpdateDimensions(scale);
            foreach (var child in _children)
            {
                child.UpdateThisAndChildDimensions(scale);
            }
        }
        public void UpdateDimensions(int scale) => Dimensions.Update(scale);

        public void SetLocationConfig(int x, int y, CoordinateMode positioningMode) => Dimensions.SetLocationConfig(x, y, positioningMode);

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
