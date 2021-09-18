using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TitleScreen : MenuScreen
    {
        public TitleScreen(Action<ScreenType> switchScreen) : base(ScreenType.Title, switchScreen)
        {
            _manualResizeEnabled = false;

            var title = new TitleScreenAnimationMenuItem(this,()=>(_currentScreenDimensions/Scale));
            title.SetLocationConfig(0, 0, CoordinateMode.Absolute, centered: false);
            _addMenuItem(title);
        }

        protected override int _getReccomendedScale() => Math.Max(base._getReccomendedScale() * 3, 4);
    }

    class TitleScreenAnimationMenuItem : ContainerMenuItem
    {
        private Func<IntPoint> _getScreenSize;
        public TitleScreenAnimationMenuItem(IHasDrawLayer parent,Func<IntPoint> parentSIzeProvider) : base(parent)
        {
            _getScreenSize = parentSIzeProvider;

            var youAreA = new SpriteMenuItem(this, TitleAnimationSprites.YouAreA);
            youAreA.SetLocationConfig(50, 20, CoordinateMode.ParentPercentage, centered: true);
            youAreA.MultiplyScale(0.25f);
            AddChild(youAreA);

            var title = new TitleSprite(this);
            title.SetLocationConfig(50, 45, CoordinateMode.ParentPercentage, centered: true);
            AddChild(title);
        }

        public override IntPoint GetBaseSize() => _getScreenSize();
    }

    public class TitleSprite : ContainerMenuItem
    {
        public TitleSprite(IHasDrawLayer parent) : base(parent)
        {
            var c = new SpriteMenuItem(this, TitleAnimationSprites.C);
            var u = new SpriteMenuItem(this, TitleAnimationSprites.U);
            var b = new SpriteMenuItem(this, TitleAnimationSprites.B);
            var e = new SpriteMenuItem(this, TitleAnimationSprites.E);

            c.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, centered: false);
            u.SetLocationConfig(20, 0, CoordinateMode.ParentPixel, centered: false);
            b.SetLocationConfig(40, 0, CoordinateMode.ParentPixel, centered: false);
            e.SetLocationConfig(60, 0, CoordinateMode.ParentPixel, centered: false);

            AddChild(c);
            AddChild(u);
            AddChild(b);
            AddChild(e);

            _size.X = 60 + (e.GetBaseSize().X);
            _size.Y = c.GetBaseSize().Y;
        }

        private IntPoint _size = IntPoint.Zero;
        public override IntPoint GetBaseSize()
        {
            return _size;
        }
    }
}
