﻿using Microsoft.Xna.Framework;
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

        public event Action<UserInput> OnMouseReleased;
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

        public void FillRestOfScreen(DrawingInterface drawingInterface,float layer,Color color,int pixelPadding)
        {
            pixelPadding *= Scale;

            var size = GetCurrentSize();

            var topOrigin = Point.Zero;
            var topSize = new Point(MonoGameWindow.CurrentWidth, ActualLocation.Y + pixelPadding);

            var bottomOrigin = new Point(0, ActualLocation.Y + size.Y - pixelPadding);
            var bottomSize = new Point(MonoGameWindow.CurrentWidth, MonoGameWindow.CurrentHeight - (ActualLocation.Y + size.Y) + pixelPadding);

            var leftOrigin  = new Point(0,ActualLocation.Y);
            var leftSize = new Point(ActualLocation.X + pixelPadding, size.Y);

            var rightOrigin = new Point(ActualLocation.X+size.X - pixelPadding, ActualLocation.Y);
            var rightSize = new Point(MonoGameWindow.CurrentWidth-(ActualLocation.X + size.X) + pixelPadding, size.Y);

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
