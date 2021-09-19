using System;
using System.Collections.Generic;

namespace IAmACube
{
    class TitleScreenAnimationMenuItem : FullScreenMenuItem
    {
        private Random _r;
        private List<string> _twinkleSprites = new List<string>() { TitleAnimationSprites.Star, TitleAnimationSprites.StarTwinkle };

        private int _maxStars = 100;

        public TitleScreenAnimationMenuItem(IHasDrawLayer parent, Func<IntPoint> parentSizeProvider) : base(parent, parentSizeProvider)
        {
            _r = new Random();
            _stars = new List<SpriteScreenItem>();

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
            if (_r.Next(1, 20) == 1 & _stars.Count < _maxStars)
            {
                _addStarToEdge();
            }

            _cleanUpStars();

            Console.WriteLine(_stars.Count + " " + _children.Count);
        }

        private void _cleanUpStars()
        {
            var toRemove = new List<SpriteScreenItem>();
            var curScreenSize = _getScreenSize() * Scale;
            foreach (var star in _stars)
            {
                if (star.ActualLocation.X < -10 | star.ActualLocation.Y < -10 | star.ActualLocation.X > curScreenSize.X + 10 | star.ActualLocation.Y > curScreenSize.Y + 10)
                {
                    toRemove.Add(star);
                }
            }

            RemoveChildrenAfterUpdate(toRemove);
            toRemove.ForEach(s => _stars.Remove(s));
        }

        private List<SpriteScreenItem> _stars;

        public void SetScatteredStars()
        {
            RemoveChildrenAfterUpdate(_stars);
            _stars.Clear();

            var curScreenSize = _getScreenSize() * Scale;

            for (int i = 0; i < _maxStars; i++)
            {
                bool startsAtLeft = _r.Next(2) == 1;
                var startLocation = new IntPoint(_r.Next(curScreenSize.X), _r.Next(curScreenSize.Y));

                var star = _generateStar(startsAtLeft);
                star.SetLocationConfig(startLocation, CoordinateMode.Absolute, centered: false);
            }
        }

        private void _addStarToEdge()
        {
            var curScreenSize = _getScreenSize() * Scale;
            bool startsAtLeft = _r.Next(2) == 1;
            var startLocation = new IntPoint(startsAtLeft ? -5 : curScreenSize.X + 5, _r.Next(curScreenSize.Y));

            var star = _generateStar(startsAtLeft);
            star.SetLocationConfig(startLocation, CoordinateMode.Absolute, centered: false);
        }

        private SpriteScreenItem _generateStar(bool startsAtLeft)
        {
            var star = new SpriteScreenItem(this, TitleAnimationSprites.Star);
            star.ScaleProvider = new ScreenItemScaleProviderStatic(_r.Next(2, 4));
            _stars.Add(star);
            AddChild(star);

            var moveDirection = startsAtLeft ? new IntPoint(1, 0) : new IntPoint(-1, 0);
            var moveAnimation = new MovementAnimation<SpriteScreenItem>(Triggers.Cyclic(1), moveDirection);
            star.AddAnimation(moveAnimation);

            var cycleShort = _r.Next(1, 60);
            var cycleLong = cycleShort + _r.Next(1, 120);

            var twinkleAnimation = new SpriteCycleAnimation(Triggers.CyclicVaried(cycleShort, cycleLong), _twinkleSprites);
            star.AddAnimation(twinkleAnimation);
            return star;
        }

    }


}
