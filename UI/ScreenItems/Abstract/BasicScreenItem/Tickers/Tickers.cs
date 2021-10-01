namespace IAmACube
{
    static class Tickers
    {
        public static RandomTicker Random(int odds) => new RandomTicker(odds);
        public static CyclicTicker Cyclic(int cycleLength) => new CyclicTicker(cycleLength);
        public static VaryingCyclicTicker CyclicVaried(int lowerbound,int upperBound) => new VaryingCyclicTicker(lowerbound,upperBound);
        public static ConstantTicker Constant = new ConstantTicker();
    }

}
