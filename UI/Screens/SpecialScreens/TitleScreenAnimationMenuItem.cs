using System;
using System.Collections.Generic;

namespace IAmACube
{
    class TitleScreenAnimationMenuItem : FullScreenMenuItem
    {
        private Random _r;
        private List<string> _twinkleSprites = new List<string>() { TitleAnimationSprites.Star, TitleAnimationSprites.StarTwinkle };
        private List<string> _sleeperSprites = new List<string>() { TitleAnimationSprites.Sleeper1, TitleAnimationSprites.Sleeper2, TitleAnimationSprites.Sleeper3 };

        private int _maxFloaters = 150;

        public TitleScreenAnimationMenuItem(IHasDrawLayer parent, Func<IntPoint> parentSizeProvider) : base(parent, parentSizeProvider)
        {
            _r = new Random();
            _floaters = new List<SpriteScreenItem>();

            var youAreA = new SpriteScreenItem(this, TitleAnimationSprites.YouAreA);
            youAreA.SetLocationConfig(50, 20, CoordinateMode.ParentPercentage, centered: true);
            youAreA.MultiplyScale(0.25f);
            AddChild(youAreA);

            var title = new TitleScreenCubeNameMenuItem(this);
            title.SetLocationConfig(50, 45, CoordinateMode.ParentPercentage, centered: true);
            AddChild(title);

        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            if (_r.Next(1, 20) == 1 & _floaters.Count < _maxFloaters)
            {
                _addFloaterToEdge();
            }

            _cleanUpFloaters();

            Console.WriteLine(_floaters.Count + " " + _children.Count);
        }

        private void _cleanUpFloaters()
        {
            var toRemove = new List<SpriteScreenItem>();
            var curScreenSize = _getScreenSize() * Scale;
            foreach (var floater in _floaters)
            {
                var floaterSize = floater.GetCurrentSize();
                if (floater.ActualLocation.X < -floaterSize.X | floater.ActualLocation.Y < -floaterSize.Y | floater.ActualLocation.X > curScreenSize.X + floaterSize.X | floater.ActualLocation.Y > curScreenSize.Y + floaterSize.Y)
                {
                    toRemove.Add(floater);
                }
            }

            RemoveChildrenAfterUpdate(toRemove);
            toRemove.ForEach(s => _floaters.Remove(s));
        }

        private List<SpriteScreenItem> _floaters;

        public void SetScatteredFloaters()
        {
            RemoveChildrenAfterUpdate(_floaters);
            _floaters.Clear();

            var curScreenSize = _getScreenSize() * Scale;

            for (int i = 0; i < _maxFloaters; i++)
            {
                //bool startsAtLeft = _r.Next(2) == 1;
                bool startsAtLeft = false;

                var startLocation = new IntPoint(_r.Next(curScreenSize.X), _r.Next(curScreenSize.Y));

                var floater = _generateFloater(startsAtLeft);
                floater.SetLocationConfig(startLocation, CoordinateMode.Absolute, centered: false);
            }
        }

        private void _addFloaterToEdge()
        {
            var curScreenSize = _getScreenSize() * Scale;
            //bool startsAtLeft = _r.Next(2) == 1;
            bool startsAtLeft = false;

            var startLocation = new IntPoint(startsAtLeft ? -5 : curScreenSize.X + 5, _r.Next(curScreenSize.Y));

            var floater = _generateFloater(startsAtLeft);
            floater.SetLocationConfig(startLocation, CoordinateMode.Absolute, centered: false);
        }

        private SpriteScreenItem _generateFloater(bool startsAtLeft)
        {
            var isSleeper = _r.Next(1, 20) == 1;
            var floater = new SpriteScreenItem(ManualDrawLayer.Create(DrawLayers.BackgroundLayer), isSleeper ? _sleeperSprites[_r.Next(0, 3)] : TitleAnimationSprites.Star);
            floater.ScaleProvider = new ScreenItemScaleProviderStatic(_r.Next(2, 4));
            floater.FlipHorizontal = _r.Next(1, 3) == 1;
            _floaters.Add(floater);
            AddChild(floater);

            var moveDirection = startsAtLeft ? new IntPoint(1, 0) : new IntPoint(-1, 0);
            var moveAnimation = new MovementAnimation<SpriteScreenItem>(Triggers.Cyclic(2), moveDirection);
            floater.AddAnimation(moveAnimation);


            if(!isSleeper)
            {
                var cycleShort = _r.Next(1, 60);
                var cycleLong = cycleShort + _r.Next(1, 120);

                var twinkleAnimation = new SpriteCycleAnimation(Triggers.CyclicVaried(cycleShort, cycleLong), _twinkleSprites);
                floater.AddAnimation(twinkleAnimation);
            }

            return floater;
        }

    }


}
