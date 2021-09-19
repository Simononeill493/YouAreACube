namespace IAmACube
{
    public class TitleScreenCubeNameMenuItem : ContainerMenuItem
    {
        public TitleScreenCubeNameMenuItem(IHasDrawLayer parent) : base(parent)
        {
            var c = new SpriteScreenItem(this, TitleAnimationSprites.C);
            var u = new SpriteScreenItem(this, TitleAnimationSprites.U);
            var b = new SpriteScreenItem(this, TitleAnimationSprites.B);
            var e = new SpriteScreenItem(this, TitleAnimationSprites.E);

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
