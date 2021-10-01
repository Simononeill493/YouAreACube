using System.Collections.Generic;

namespace IAmACube
{
    public class SpriteCycleAnimation : Animation<SpriteScreenItem>
    {
        private List<string> _sprites;
        private int _currentIndex;

        public SpriteCycleAnimation(Trigger trigger, Ticker ticker, List<string> sprites): base(trigger,ticker)
        {
            _sprites = sprites;
        }

        protected override void _do(SpriteScreenItem item)
        {
            var nextIndex = (_currentIndex + 1) >= _sprites.Count ? 0 : _currentIndex + 1;
            item.SpriteName = _sprites[nextIndex];

            _currentIndex = nextIndex;
        }
    }

}
