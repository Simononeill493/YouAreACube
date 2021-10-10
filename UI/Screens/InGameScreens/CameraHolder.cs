using System;

namespace IAmACube
{
    class CameraHolder : ContainerScreenItem
    {
        public Camera Camera;

        private Func<IntPoint> _getSize;
        public CameraHolder(IHasDrawLayer parent,Func<IntPoint> getSize) : base(parent)
        {
            _getSize = getSize;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            Camera.Update(input);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            Camera.Draw(drawingInterface);
        }

        public override IntPoint GetBaseSize() => throw new NotImplementedException();
        public override IntPoint GetCurrentSize() => _getSize();
    }
}
