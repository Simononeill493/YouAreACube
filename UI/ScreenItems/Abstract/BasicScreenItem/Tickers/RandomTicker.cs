namespace IAmACube
{
    class RandomTicker : Ticker
    {
        private int _odds;
        public RandomTicker(int odds) : base()
        {
            _odds = odds;
        }

        protected override bool _tick()
        {
            return RandomUtils.RandomNumber(_odds) == 0;
        }
    }

}
