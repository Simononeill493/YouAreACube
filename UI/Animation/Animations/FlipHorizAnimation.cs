namespace IAmACube
{
    class FlipHorizAnimation : Animation<SpriteScreenItem>
    {
        public FlipHorizAnimation(Trigger trigger,Ticker ticker) : base(trigger,ticker) { }

        protected override void _do(SpriteScreenItem item)
        {
            item.FlipHorizontal = !item.FlipHorizontal;
        }
    }

}
