using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class DraggableMenuItem : SpriteMenuItem
    {
        public event System.Action<UserInput> OnStartDrag;
        public event System.Action<UserInput> OnEndDrag;
        public event System.Action<UserInput> OnMoved;

        public bool Dragging { get; private set; } = false;
        public bool Draggable = true;

        private IntPoint _dragOffset;

        public DraggableMenuItem(IHasDrawLayer parent, string sprite) : base(parent, sprite)
        {
            OnMousePressed += TryStartDragAtMousePosition;
        }

        public override void Update(UserInput input) 
        {
            base.Update(input);

            _draggableUpdateLocation(input);
            _checkForEndDrag(input);
        }

        public virtual bool TryStartDrag(UserInput input, IntPoint offset)
        {
            if (Draggable & !Dragging & !MenuScreen.IsUserDragging)
            {
                _startDrag(input,offset);
                return true;
            }

            return false;
        }
        
        protected void _startDrag(UserInput input,IntPoint offset)
        {
            OffsetDrawLayerCascade(-DrawLayers.MenuDragOffset);

            MenuScreen.DraggedItem = this;
            Dragging = true;
            _dragOffset = offset;

            _draggableUpdateLocation(input);
            OnStartDrag?.Invoke(input);
        }
        private void _endDrag(UserInput input)
        {
            OffsetDrawLayerCascade(DrawLayers.MenuDragOffset);

            MenuScreen.DraggedItem = null;
            Dragging = false;

            OnEndDrag?.Invoke(input);
        }

        public void TryStartDragAtMousePosition(UserInput input) => TryStartDrag(input, GetLocationOffset(input.MousePos));

        private void _draggableUpdateLocation(UserInput input)
        {
            if (Dragging)
            {
                var oldLocation = ActualLocation;

                SetLocationConfig(input.MousePos - _dragOffset, CoordinateMode.Absolute);
                UpdateDimensionsCascade(IntPoint.Zero, IntPoint.Zero);

                if(ActualLocation!=oldLocation)
                {
                    OnMoved?.Invoke(input);
                }
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
