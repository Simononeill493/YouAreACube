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

        public bool Dragging { get; private set; } = false;

        private Point _dragOffset;
        private float _drawLayerTemp;

        public DraggableMenuItem(IHasDrawLayer parent, string sprite) : base(parent, sprite)
        {
            OnMousePressed += _tryStartDragAtMousePosition;
        }

        private List<MenuItem> _draggableChildren = new List<MenuItem>();
        public void SetDraggableFrom(MenuItem item)
        {
            item.OnMousePressed += this._tryStartDragAtMousePosition;
            _draggableChildren.Add(item);
        }
        public void SetNotDraggableFrom(MenuItem item)
        {
            item.OnMousePressed -= this._tryStartDragAtMousePosition;
            _draggableChildren.Remove(item);
        }
        public override void Dispose()
        {
            base.Dispose();
            foreach(var draggableChild in _draggableChildren)
            {
                draggableChild.OnMousePressed -= this._tryStartDragAtMousePosition;
            }
        }

        public override void Update(UserInput input) 
        {
            base.Update(input);

            _draggableUpdateLocation(input);
            _checkForEndDrag(input);
        }

        public virtual bool TryStartDrag(UserInput input, Point offset)
        {
            if (!Dragging & !MenuScreen.UserDragging)
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
            Dragging = true;
            _dragOffset = offset;

            _draggableUpdateLocation(input);
            OnStartDrag?.Invoke(input);
        }
        private void _endDrag(UserInput input)
        {
            UpdateDrawLayerCascade(_drawLayerTemp);

            MenuScreen.UserDragging = false;
            Dragging = false;

            OnEndDrag?.Invoke(input);
        }

        private void _tryStartDragAtMousePosition(UserInput input)
        {
            var mouseOffset = GetLocationOffset(input.MousePos);
            TryStartDrag(input, mouseOffset);
        }
        private void _draggableUpdateLocation(UserInput input)
        {
            if (Dragging)
            {
                this.SetLocationConfig(input.MousePos - _dragOffset, CoordinateMode.Absolute);
                this.UpdateDimensionsCascade(Point.Zero, Point.Zero);
            }
        }
        private void _checkForEndDrag(UserInput input)
        {
            if (Dragging & !input.MouseLeftPressed)
            {
                _endDrag(input);
            }
        }
    }
}
