using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TitleScreenAnimationFloaterScroll : FullScreenMenuItem
    {
        private Random _r;
        private List<string> _twinkleSprites = new List<string>() { TitleAnimationSprites.Star, TitleAnimationSprites.StarTwinkle };
        private List<string> _sleeperSprites = new List<string>() { TitleAnimationSprites.Sleeper1, TitleAnimationSprites.Sleeper2, TitleAnimationSprites.Sleeper3 };

        private List<ScreenItem> _floaters;
        private int _maxFloaters = 150;
        private float _floaterScaleMultiplier = 0.25f;
        private int _scaledCubeSize => (int)(16.0f * ((int)(Scale * _floaterScaleMultiplier)));

        private int _scrollTick;

        public TitleScreenAnimationFloaterScroll(Screen parent) : base(parent)
        {
            _r = new Random();
            _floaters = new List<ScreenItem>();
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            if (_r.Next(1, 15) == 1 & _floaters.Count < _maxFloaters)
            {
                _addFloaterToEdge();
            }

            _tickScroll();
        }

        private void _tickScroll()
        {
            _floaters.ForEach(f => f.OffsetLocationConfig(IntPoint.Left));
            _scrollTick++;
            _scrollTick = _scrollTick % _scaledCubeSize;
        }

        public void SetScatteredFloaters()
        {
            RemoveChildrenAfterUpdate(_floaters);
            _floaters.Clear();

            var gridPositions = GetCurrentSize() / _scaledCubeSize;

            for (int i = 0; i < (_maxFloaters*0.9); i++)
            {
                var floater = _generateFloater();
                var startLocation = new IntPoint(_r.Next(gridPositions.X)*_scaledCubeSize, _r.Next(gridPositions.Y)*_scaledCubeSize);

                floater.SetLocationConfig(startLocation, CoordinateMode.Absolute, centered: false);
            }

            _scrollTick = 0;
        }

        private void _addFloaterToEdge()
        {
            var floater = _generateFloater();

            var currentSize = GetCurrentSize();
            var gridPositions = currentSize / _scaledCubeSize;
            var startLocation = new IntPoint(currentSize.X+ _scaledCubeSize-_scrollTick, _r.Next(gridPositions.Y) * _scaledCubeSize);

            floater.SetLocationConfig(startLocation, CoordinateMode.Absolute, centered: false);
        }

        private ScreenItem _generateFloater()
        {
            var floater = (_r.Next(1, 8) == 1) ? _generateCubesFloater() : _generateNonCubesFloater();
            floater.MultiplyScale(_floaterScaleMultiplier);

            _floaters.Add(floater);
            AddChild(floater);
            return floater;
        }

        private ScreenItem _generateCubesFloater()
        {
            var cubesFloater = new CubesFloater(ManualDrawLayer.Create(DrawLayers.BackgroundDecorationLayer), _r.Next(3, 5));
            return cubesFloater;
        }
        private ScreenItem _generateNonCubesFloater()
        {
            var isSleeper = _r.Next(1, 20) == 1;
            var floater = new SpriteScreenItem(ManualDrawLayer.Create(DrawLayers.BackgroundDecorationLayer), isSleeper ? _sleeperSprites[_r.Next(0, _sleeperSprites.Count)] : _twinkleSprites[_r.Next(0, _twinkleSprites.Count)]);
            floater.FlipHorizontal = _r.Next(1, 3) == 1;

            if (!isSleeper)
            {
                var cycleShort = _r.Next(40, 300);
                var cycleLong = cycleShort + _r.Next(1, 60);

                var twinkleAnimation = new SpriteCycleAnimation(Triggers.Instant,Tickers.CyclicVaried(cycleShort, cycleLong), _twinkleSprites);
                floater.AddAnimation(twinkleAnimation);
                floater.DrawLayer += DrawLayers.MinLayerDistance;
            }

            return floater;
        }

        protected override List<ScreenItem> _removeOffscreenChildren()
        {
            var toRemove = base._removeOffscreenChildren();
            toRemove.ForEach(s => _floaters.Remove(s));
            return toRemove;
        }
    }
}
