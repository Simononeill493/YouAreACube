namespace IAmACube
{
    class ConstantTicker : Ticker
    {
        public ConstantTicker() : base()
        {

        }

        protected override bool _tick() => true;
    }

}
