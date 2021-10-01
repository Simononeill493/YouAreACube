namespace IAmACube
{
    public abstract class Animation
    {
        private bool _triggered;
        private Trigger _trigger;
        private Ticker _ticker;

        protected bool _completed;

        public Animation(Trigger trigger,Ticker ticker)
        {
            _trigger = trigger;
            _ticker = ticker;
        }

        public void Do(ScreenItem item)
        {
            if(_completed)
            {
                return;
            }

            if (!_triggered)
            {
                _triggered = _trigger.Check();
                if(_triggered)
                {
                    Begin(item);
                }
            }

            if (_triggered)
            {
                if (_ticker.Tick())
                {
                    _do(item);
                }
            }
        }

        public virtual void Begin(ScreenItem item) { }

        protected abstract void _do(ScreenItem item);
    }

   public abstract class Animation<T> : Animation where T : ScreenItem
   {
        public Animation(Trigger trigger, Ticker ticker) : base(trigger,ticker) { }

        protected override void _do(ScreenItem item) => _do((T)item);
        protected abstract void _do(T item);
    }

}
