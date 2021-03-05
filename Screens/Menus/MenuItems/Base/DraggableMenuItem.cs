using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DraggableMenuItem : SpriteMenuItem
    {
        public event System.Action<UserInput> OnStartDrag;
        public event System.Action<UserInput> OnEndDrag;

        private bool _dragging = false;
        private Point _dragOffset;
        private float _drawLayerTemp;

        public DraggableMenuItem(IHasDrawLayer parent, string sprite) : base(parent, sprite)
        {
            OnMousePressed += _tryStartDragAtMousePosition;
        }

        public void SetDraggableFrom(MenuItem item)
        {
            item.OnMousePressed += this._tryStartDragAtMousePosition;
        }
        public void SetNotDraggableFrom(MenuItem item)
        {
            item.OnMousePressed -= this._tryStartDragAtMousePosition;
        }   

        public override void Update(UserInput input)
        {
            base.Update(input);
            _draggableUpdateLocation(input);
            _checkForEndDrag(input);
        }

        public virtual bool TryStartDrag(UserInput input, Point offset)
        {
            if (!_dragging & !MenuScreen.UserDragging)
            {
                _startDrag(input,offset);
                return true;
            }

            return false;
        }
        
        protected void _startDrag(UserInput input,Point offset)
        {
            _drawLayerTemp = DrawLayer;
            UpdateDrawLayerCascade(DrawLayers.MenuDragLayer);

            MenuScreen.UserDragging = true;
            _dragging = true;
            _dragOffset = offset;

            _draggableUpdateLocation(input);
            OnStartDrag?.Invoke(input);
        }
        private void _endDrag(UserInput input)
        {
            UpdateDrawLayerCascade(_drawLayerTemp);

            MenuScreen.UserDragging = false;
            _dragging = false;

            OnEndDrag?.Invoke(input);
        }

        private void _tryStartDragAtMousePosition(UserInput input)
        {
            var mouseOffset = GetLocationOffset(input.MousePos);
            TryStartDrag(input, mouseOffset);
        }
        private void _draggableUpdateLocation(UserInput input)
        {
            if (_dragging)
            {
                this.SetLocationConfig(input.MousePos - _dragOffset, CoordinateMode.Absolute);
                this.UpdateDimensionsCascade(Point.Zero, Point.Zero);
            }
        }
        private void _checkForEndDrag(UserInput input)
        {
            if (_dragging & !input.MouseLeftPressed)
            {
                _endDrag(input);
            }
        }
    }
}
