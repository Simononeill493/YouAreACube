namespace IAmACube
{
    public abstract class Ticker
    {
        public Ticker() 
        {
        }

        public bool Tick()
        {
            return _tick();
        }

        protected abstract bool _tick();
    }
}
