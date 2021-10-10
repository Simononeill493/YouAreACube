using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract partial class ScreenItem 
    {
        public event System.Action<UserInput> OnStartDrag;
        public event System.Action<UserInput> OnEndDrag;
        public event System.Action<UserInput> OnMoved;

        public bool Dragging { get; private set; } = false;
        private bool _thisOrParentDragged;

        public bool Draggable = false;

        private IntPoint _dragOffset;

        public void _dragUpdate(UserInput input)
        {
            if(_thisOrParentDragged)
            {
                SetDragStateCascade(true);
            }

            _draggableUpdateLocation(input);
            _checkForEndDrag(input);
        }

        public virtual bool TryStartDrag(UserInput input, IntPoint offset)
        {
            if (_canStartDragging())
            {
                _startDrag(input, offset);
                return true;
            }

            return false;
        }

        protected virtual bool _canStartDragging() => (Draggable & !Dragging & !Screen.IsUserDragging);

        protected void _startDrag(UserInput input, IntPoint offset)
        {
            if (Dragging) { throw new Exception(); }

            SetDragStateCascade(true);

            Screen.DraggedItem = this;
            Dragging = true;
            _dragOffset = offset;

            _draggableUpdateLocation(input);
            OnStartDrag?.Invoke(input);
        }
        private void _endDrag(UserInput input)
        {
            if (!Dragging) { throw new Exception(); }

            SetDragStateCascade(false);

            Screen.DraggedItem = null;
            Dragging = false;

            OnEndDrag?.Invoke(input);
        }

        public virtual void TryStartDragAtMousePosition(UserInput input) => TryStartDrag(input, GetLocationOffset(input.MousePos));

        public void MoveToMousePosition(UserInput input) => SetLocationConfig(input.MousePos, CoordinateMode.Absolute);

        private void _draggableUpdateLocation(UserInput input)
        {
            if (Dragging)
            {
                var oldLocation = ActualLocation;

                SetLocationConfig(input.MousePos - _dragOffset, CoordinateMode.Absolute);
                UpdateLocationCascade(IntPoint.Zero, IntPoint.Zero);

                if (ActualLocation != oldLocation)
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

        public void ManuallyEndDrag(UserInput input) => _endDrag(input);
    }
}
