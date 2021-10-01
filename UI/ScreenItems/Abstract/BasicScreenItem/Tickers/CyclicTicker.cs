namespace IAmACube
{
    class CyclicTicker : Ticker
    {
        private int _cycleLength;
        private int _cyclePosition;

        public CyclicTicker(int cycleLength) : base()
        {
            _cycleLength = cycleLength;
        }

        protected override bool _tick()
        { 
            if(_cyclePosition++>=_cycleLength)
            {
                _cyclePosition = 0;
                return true;
            }

            return false;
        }
    }

}
