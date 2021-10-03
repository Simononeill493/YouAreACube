using System;
using System.Collections.Generic;
using System.Linq;
namespace IAmACube
{
    class CubesFloater : ContainerScreenItem
    {
        public const string SpriteName = "TitleBox";
        public const string SpriteNameHighlight = "TitleBoxGlowing";

        private Dictionary<IntPoint, CubeFloater> _contents;

        private IntPoint _size;

        public CubesFloater(IHasDrawLayer parent, int adjacentTries) : base(parent)
        {
            _size = IntPoint.Zero;
            _contents = new Dictionary<IntPoint, CubeFloater>();
            _tryAddAtLoc(IntPoint.Zero);

            for (int i = 0; i < adjacentTries; i++)
            {
                _addRandAdj();
            }
        }

        public void _addRandAdj()
        {
            var rand = RandomUtils.GetRandom(_contents.Values.ToList());
            var loc = rand.Loc.GetAdjacentCoords(RandomUtils.RandomCardinalNoDiagonals());
            _tryAddAtLoc(loc);

        }

        private void _tryAddAtLoc(IntPoint loc)
        {
            if (_contents.ContainsKey(loc) | loc.X < 0 | loc.Y < 0)
            {
                return;
            }

            var spriteSize = SpriteManager.GetSpriteSize(SpriteName);

            var item = new CubeFloater(this, loc) { HighlightedSpriteName = SpriteNameHighlight };
            item.SetLocationConfig(loc * spriteSize, CoordinateMode.ParentPixel, false);
            AddChild(item);

            _contents[loc] = item;

            _size.X = Math.Max(_size.X, (loc.X + 1) * (spriteSize.X));
            _size.Y = Math.Max(_size.Y, (loc.Y + 1) * (spriteSize.Y));
        }

        public override IntPoint GetBaseSize()
        {
            return _size;
        }
    }

    class CubeFloater : SpriteScreenItem
    {
        public IntPoint Loc;
        public CubeFloater(IHasDrawLayer parent, IntPoint loc) : base(parent, CubesFloater.SpriteName)
        {
            Loc = loc;
        }
    }
}
