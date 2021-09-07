using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class MenuItem : IHasDrawLayer, IDisposable
    {
        public bool Visible = true;
        public bool Enabled = true;

        public virtual void HideAndDisable() => Visible = Enabled = false;
        public virtual void ShowAndEnable() => Visible = Enabled = true;

        public bool Disposed { get; private set; } = false;

        public float DrawLayer 
        { 
            get 
            { 
                return _drawLayer; 
            }
            set 
            {
                _drawLayer = value;

                if(_drawLayer<-0.001 | _drawLayer>1.001)
                {
                    throw new Exception();
                }
            } 
        }
        private float _drawLayer;

        public event Action<UserInput> OnMouseReleased;
        public event Action<UserInput> OnMousePressed;
        public event Action<UserInput> OnMouseStartHover;
        public event Action<UserInput> OnMouseEndHover;
        public event Action<UserInput> OnMouseDraggedOn;

        public bool MouseHovering { get; private set; } = false;
        protected bool _mousePressedOn = false;
        protected IntPoint _lastMousePressedLocation = IntPoint.Zero;

        protected bool _mousePressedForClick;

        public MenuItem(IHasDrawLayer parentDrawLayer)
        {
            DrawLayer = parentDrawLayer.DrawLayer - DrawLayers.MinLayerDistance;
        }

        public void Draw(DrawingInterface drawingInterface)
        {
            if (Disposed) { throw new ObjectDisposedException("Drawing disposed MenuItem"); }
            if(!Visible) { return; }

            var oldDrawLayer = DrawLayer;
            if(_thisOrParentDragged)
            {
                DrawLayer -= DrawLayers.MenuDragOffset;
            }

            _drawSelf(drawingInterface);
            _drawChildren(drawingInterface);

            DrawLayer = oldDrawLayer;
        }
        public virtual void Update(UserInput input)
        {
            if (Disposed) { throw new ObjectDisposedException("Updating disposed MenuItem"); }
            if(!Enabled) { return; }

            var oldHoverState = MouseHovering;
            var newHoverState = _isMouseOver(input);

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
                if(_mousePressedOn & (_lastMousePressedLocation != input.MousePos) & !MenuScreen.IsUserDragging)
                {
                    OnMouseDraggedOn?.Invoke(input);
                }

                if (input.MouseLeftJustPressed) //Mouse Pressed
                {
                    _mousePressedOn = true;
                    _lastMousePressedLocation = input.MousePos;
                    OnMousePressed?.Invoke(input);

                    TryStartDragAtMousePosition(input);
                }
                else if(input.MouseLeftReleased) //Mouse released
                {
                    if(_mousePressedOn)
                    {
                        _mousePressedOn = false;
                        OnMouseReleased?.Invoke(input);
                    }
                }
            }
            else
            {
                _mousePressedOn = false;
                _mousePressedForClick = false;
            }

            _updateChildren(input);
            _addAndRemoveQueuedChildren();

            _dragUpdate(input);
        }

        public void SetDragStateCascade(bool state)
        {
            _thisOrParentDragged = state;
            _children.ForEach(c => c.SetDragStateCascade(state));
        }
        public void UpdateDrawLayerCascade(float newLayer)
        {
            DrawLayer = newLayer;
            _children.ForEach(child => child.UpdateDrawLayerCascade(newLayer - DrawLayers.MinLayerDistance));
        }
        
        protected virtual bool _isMouseOver(UserInput input) 
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

            Disposed = true;
        }

        protected virtual void _drawSelf(DrawingInterface drawingInterface) { }

        public void FillRestOfScreen(DrawingInterface drawingInterface,float layer,Color color,int pixelPadding)
        {
            pixelPadding *= Scale;

            var size = GetCurrentSize();

            var topOrigin = IntPoint.Zero;
            var topSize = new IntPoint(MonoGameWindow.CurrentSize.X, ActualLocation.Y + pixelPadding);

            var bottomOrigin = new IntPoint(0, ActualLocation.Y + size.Y - pixelPadding);
            var bottomSize = new IntPoint(MonoGameWindow.CurrentSize.X, MonoGameWindow.CurrentSize.Y - (ActualLocation.Y + size.Y) + pixelPadding);

            var leftOrigin  = new IntPoint(0,ActualLocation.Y);
            var leftSize = new IntPoint(ActualLocation.X + pixelPadding, size.Y);

            var rightOrigin = new IntPoint(ActualLocation.X+size.X - pixelPadding, ActualLocation.Y);
            var rightSize = new IntPoint(MonoGameWindow.CurrentSize.X - (ActualLocation.X + size.X) + pixelPadding, size.Y);

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
