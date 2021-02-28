using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class MenuItem : IHasDrawLayer
    {
        public bool Visible = true;
        public float DrawLayer { get; set; }

        public event Action<UserInput> OnClick;
        public event Action<UserInput> OnMousePressed;
        public event Action<UserInput> OnMouseStartHover;
        public event Action<UserInput> OnMouseEndHover;

        protected bool _mousePressedOn = false;
        protected bool _mouseHovering = false;

        public MenuItem(IHasDrawLayer parentDrawLayer)
        {
            DrawLayer = parentDrawLayer.DrawLayer - DrawLayers.MinLayerDistance;
        }

        public void Draw(DrawingInterface drawingInterface)
        {
            if(Visible)
            {
                _drawSelf(drawingInterface);
                _drawChildren(drawingInterface);
            }
        }
        public virtual void Update(UserInput input)
        {
            var oldHoverState = _mouseHovering;
            var newHoverState = IsMouseOver(input);

            if(!oldHoverState & newHoverState)
            {
                OnMouseStartHover?.Invoke(input);
            }
            else if(oldHoverState & !newHoverState)
            {
                OnMouseEndHover?.Invoke(input);
            }

            _mouseHovering = newHoverState;

            if (_mouseHovering)
            {
                if (input.MouseLeftPressed)
                {
                    _mousePressedOn = true;
                    OnMousePressed?.Invoke(input);
                }
                else if(_mousePressedOn & input.MouseLeftReleased)
                {
                    _mousePressedOn = false;
                    OnClick?.Invoke(input);
                }
            }
            else
            {
                _mousePressedOn = false;
            }

            _updateChildren(input);
            _addAndRemoveQueuedChildren();
        }
        public void UpdateDrawLayerCascade(float newLayer)
        {
            DrawLayer = newLayer;
            foreach (var child in _children)
            {
                child.UpdateDrawLayerCascade(newLayer - DrawLayers.MinLayerDistance);
            }
        }
        public virtual bool IsMouseOver(UserInput input) 
        {
            var size = GetCurrentSize();
            var rect = new Rectangle(ActualLocation.X, ActualLocation.Y, size.X, size.Y);

            return rect.Contains(input.MouseX, input.MouseY);
        }


        protected virtual void _drawSelf(DrawingInterface drawingInterface) { }
    }
}
