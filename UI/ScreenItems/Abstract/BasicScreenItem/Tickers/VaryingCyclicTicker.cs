namespace IAmACube
{
    public class VaryingCyclicTicker : Ticker
    {
        private int _cycleLengthLower;
        private int _cycleLengthUpper;
        private int _cycleLengthCurent;

        private int _cyclePosition;

        public VaryingCyclicTicker(int cycleLengthLower,int cycleLengthUpper) : base()
        {
            _cycleLengthLower = cycleLengthLower;
            _cycleLengthUpper = cycleLengthUpper;
            _cycleLengthCurent = _getNewCycle();
        }

        protected override bool _tick()
        {
            if (_cyclePosition++ >= _cycleLengthCurent)
            {
                _cyclePosition = 0;
                _cycleLengthCurent = _getNewCycle();
                return true;
            }

            return false;
        }

        private int _getNewCycle()
        {
            return RandomUtils.RandomNumber(_cycleLengthLower, _cycleLengthUpper);
        }
    }

}
