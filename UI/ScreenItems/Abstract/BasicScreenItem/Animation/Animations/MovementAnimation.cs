namespace IAmACube
{
    class MovementAnimation : Animation
    {
        private IntPoint _offset;

        public MovementAnimation(Trigger trigger, Ticker ticker, IntPoint offset) : base(trigger,ticker)
        {
            _offset = offset;
        }

        protected override void _do(ScreenItem item)
        {
            item.OffsetLocationConfig(_offset);
        }
    }

}
