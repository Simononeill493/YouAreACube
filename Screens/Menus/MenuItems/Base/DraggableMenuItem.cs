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

        public DraggableMenuItem(IHasDrawLayer parent, string sprite) : base(parent, sprite)
        {
            OnMousePressed += _beginDrag;
            OnClick += _endDrag;

            DrawLayer = DrawLayers.MenuDragLayer;
        }

        public void ManuallyAttachToMouse(Point offset) => _attachToMouse(offset);

        private void _beginDrag(UserInput input)
        {
            if(!_dragging & !MenuScreen.UserDragging)
            {
                _attachToMouse(input.MousePos - ActualLocation);
            }
        }

        private void _attachToMouse(Point offset)
        {
            MenuScreen.UserDragging = true;
            _dragging = true;
            _dragOffset = offset;
        }

        private void _endDrag(UserInput input)
        {
            MenuScreen.UserDragging = false;
            _dragging = false;
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
