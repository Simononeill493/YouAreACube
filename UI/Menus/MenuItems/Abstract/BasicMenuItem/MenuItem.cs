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

        public bool Disposed { get; private set; } = false;

        public event Action<UserInput> OnMouseReleased;
        public event Action<UserInput> OnMousePressed;
        public event Action<UserInput> OnMouseStartHover;
        public event Action<UserInput> OnMouseEndHover;
        public event Action<UserInput> OnMouseDraggedOn;

        public bool MouseHovering { get; private set; } = false;
        protected bool _mousePressedOn = false;
        protected Point _lastMouseClickLocation = Point.Zero;

        public MenuItem(IHasDrawLayer parentDrawLayer)
        {
            DrawLayer = parentDrawLayer.DrawLayer - DrawLayers.MinLayerDistance;
        }

        public void Draw(DrawingInterface drawingInterface)
        {
            if (Disposed) { throw new ObjectDisposedException("Drawing disposed MenuItem"); }

            if (Visible)
            {
                _drawSelf(drawingInterface);
                _drawChildren(drawingInterface);
            }
        }
        public virtual void Update(UserInput input)
        {
            if (Disposed) { throw new ObjectDisposedException("Updating disposed MenuItem"); }

            var oldHoverState = MouseHovering;
            var newHoverState = IsMouseOver(input);

            if(!oldHoverState & newHoverState)
            {
                OnMouseStartHover?.Invoke(input);
            }
            else if(oldHoverState & !newHoverState)
            {
                OnMouseEndHover?.Invoke(input);
            }

            MouseHovering = newHoverState;

            if (MouseHovering)
            {
                if(_mousePressedOn & (_lastMouseClickLocation != input.MousePos))
                {
                    OnMouseDraggedOn?.Invoke(input);
                }

                if (input.MouseLeftPressed) //Mouse Pressed
                {
                    _mousePressedOn = true;
                    _lastMouseClickLocation = input.MousePos;
                    OnMousePressed?.Invoke(input);
                }
                else if(_mousePressedOn & input.MouseLeftReleased) //Mouse released
                {
                    _mousePressedOn = false;
                    OnMouseReleased?.Invoke(input);
                }
            }
            else
            {
                _mousePressedOn = false;
            }

            _updateChildren(input);
            _addAndRemoveQueuedChildren();
        }


        public void OffsetDrawLayerCascade(float offset)
        {
            DrawLayer += offset;
            foreach (var child in _children)
            {
                child.OffsetDrawLayerCascade(offset);
            }
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

        public virtual void Dispose() 
        {
            if(Disposed)
            {
                throw new Exception("MenuItem disposed twice!");
            }

            _children.Clear();
            _toAdd.Clear();
            _toRemove.Clear();

            Disposed = true;
        }

        protected virtual void _drawSelf(DrawingInterface drawingInterface) { }

        public void FillRestOfScreen(DrawingInterface drawingInterface,float layer,Color color,int pixelPadding)
        {
            pixelPadding *= Scale;

            var size = GetCurrentSize();

            var topOrigin = Point.Zero;
            var topSize = new Point(MonoGameWindow.CurrentSize.X, ActualLocation.Y + pixelPadding);

            var bottomOrigin = new Point(0, ActualLocation.Y + size.Y - pixelPadding);
            var bottomSize = new Point(MonoGameWindow.CurrentSize.X, MonoGameWindow.CurrentSize.Y - (ActualLocation.Y + size.Y) + pixelPadding);

            var leftOrigin  = new Point(0,ActualLocation.Y);
            var leftSize = new Point(ActualLocation.X + pixelPadding, size.Y);

            var rightOrigin = new Point(ActualLocation.X+size.X - pixelPadding, ActualLocation.Y);
            var rightSize = new Point(MonoGameWindow.CurrentSize.X - (ActualLocation.X + size.X) + pixelPadding, size.Y);

            if (topSize.Y <= 0) { topSize.Y = -1; }
            if (bottomSize.Y <= 0) { bottomSize.Y = -1; }
            if (leftSize.X <= 0) { leftSize.X = -1; }
            if (rightSize.X <= 0) { rightSize.X = -1; }

            drawingInterface.DrawRectangle(topOrigin.X, topOrigin.Y, topSize.X, topSize.Y,layer, color);
            drawingInterface.DrawRectangle(bottomOrigin.X, bottomOrigin.Y, bottomSize.X, bottomSize.Y, layer, color);
            drawingInterface.DrawRectangle(leftOrigin.X, leftOrigin.Y, leftSize.X, leftSize.Y, layer, color);
            drawingInterface.DrawRectangle(rightOrigin.X, rightOrigin.Y, rightSize.X, rightSize.Y, layer, color);
        }
    }
}
