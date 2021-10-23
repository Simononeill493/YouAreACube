using System;

namespace IAmACube
{
    class GameHolder : ContainerScreenItem
    {
        public Camera Camera;
        public Game CurrentGame;

        public bool Paused;


        private Func<IntPoint> _getSize;
        public GameHolder(IHasDrawLayer parent,Func<IntPoint> getSize) : base(parent)
        {
            _getSize = getSize;

            Paused = false;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            _updateMouseHover(input);
            if (!Paused)
            {
                CurrentGame.Update(input);
            }
            Camera.Update(input);
        }

        public void MoveFrameWhilePaused(UserInput input)
        {
            _updateMouseHover(input);
            CurrentGame.Update(input);
            Camera.Update(input);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            Camera.Draw(drawingInterface);
        }

        public override IntPoint GetBaseSize() => throw new NotImplementedException();
        public override IntPoint GetCurrentSize() => _getSize();

        private void _updateMouseHover(UserInput input) => input.MouseHoverTile = Camera.GetMouseHoverTile(input.MousePos, Game.World);
    }
}
