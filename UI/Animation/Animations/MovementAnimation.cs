namespace IAmACube
{
    class MovementAnimation : Animation
    {
        private IntPoint _offset;
        private int _remainingTicks;

        public MovementAnimation(Trigger trigger, Ticker ticker, IntPoint offset,int numTicks = int.MaxValue) : base(trigger,ticker)
        {
            _offset = offset;
            _remainingTicks = numTicks;
        }

        protected override void _do(ScreenItem item)
        {
            item.OffsetLocationConfig(_offset);
            if(_remainingTicks--<=0)
            {
                _completed = true;
            }

        }
    }

}
