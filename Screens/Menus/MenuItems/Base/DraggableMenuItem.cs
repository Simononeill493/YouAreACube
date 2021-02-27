using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DraggableMenuItem : SpriteMenuItem
    {
        private bool _dragging = false;
        private Point _dragOffset;

        public event System.Action<UserInput> OnStartDrag;
        public event System.Action<UserInput> OnEndDrag;

        private float _drawLayerTemp;

        public DraggableMenuItem(IHasDrawLayer parent, string sprite) : base(parent, sprite)
        {
            OnMousePressed += _tryBeginDragAtMousePosition;
            OnClick += _endDrag;
        }

        public bool TryBeginDrag(UserInput input, Point offset)
        {
            if (!_dragging & !MenuScreen.UserDragging)
            {
                _drawLayerTemp = DrawLayer;
                UpdateDrawLayerCascade(DrawLayers.MenuDragLayer);

                MenuScreen.UserDragging = true;
                _dragging = true;
                _dragOffset = offset;

                OnStartDrag?.Invoke(input);
                return true;
            }

            return false;
        }

        private void _endDrag(UserInput input)
        {
            UpdateDrawLayerCascade(_drawLayerTemp);

            MenuScreen.UserDragging = false;
            _dragging = false;

            OnEndDrag?.Invoke(input);
        }


        public void SetDraggableFrom(MenuItem item)
        {
            item.OnMousePressed += this._tryBeginDragAtMousePosition;
            item.OnClick += this._endDrag;
        }

        private void _tryBeginDragAtMousePosition(UserInput input)
        {
            var mouseOffset = GetLocationOffset(input.MousePos);
            TryBeginDrag(input, mouseOffset);
        }


        public override void Update(UserInput input)
        {
            base.Update(input);

            if(_dragging)
            {
                this.SetLocationConfig(input.MousePos-_dragOffset, CoordinateMode.Absolute);
                this.UpdateThisAndChildLocations(Point.Zero, Point.Zero);
            }
        }
    }
}
