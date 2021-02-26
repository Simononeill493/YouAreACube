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

        public DraggableMenuItem(IHasDrawLayer parent, string sprite) : base(parent, sprite)
        {
            OnMousePressed += _beginDrag;
            OnClick += _endDrag;

            DrawLayer = DrawLayers.MenuDragLayer;
        }

        private void _beginDrag(UserInput input)
        {
            if(!_dragging & !MenuScreen.UserDragging)
            {
                var mouseOffset = GetLocationOffset(input.MousePos);
                AttachToMouse(mouseOffset);

                OnStartDrag?.Invoke(input);
            }
        }

        public void AttachToMouse(Point offset)
        {
            MenuScreen.UserDragging = true;
            _dragging = true;
            _dragOffset = offset;
        }

        private void _endDrag(UserInput input)
        {
            MenuScreen.UserDragging = false;
            _dragging = false;

            OnEndDrag?.Invoke(input);
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
