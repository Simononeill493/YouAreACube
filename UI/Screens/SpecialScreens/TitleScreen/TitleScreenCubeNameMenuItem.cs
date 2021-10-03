namespace IAmACube
{
    class TitleScreenCubeNameMenuItem : ContainerScreenItem
    {
        public TitleScreenCubeNameMenuItem(IHasDrawLayer parent) : base(parent)
        {
            var c = new SpriteScreenItem(this, TitleAnimationSprites.C);
            var u = new SpriteScreenItem(this, TitleAnimationSprites.U);
            var b = new SpriteScreenItem(this, TitleAnimationSprites.B);
            var e = new SpriteScreenItem(this, TitleAnimationSprites.E);

            c.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, centered: false);
            u.SetLocationConfig(19, 0, CoordinateMode.ParentPixel, centered: false);
            b.SetLocationConfig(38, 0, CoordinateMode.ParentPixel, centered: false);
            e.SetLocationConfig(57, 0, CoordinateMode.ParentPixel, centered: false);

            AddChild(c);
            AddChild(u);
            AddChild(b);
            AddChild(e);

            //c.ColorMask = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);
            //u.ColorMask = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);
            //b.ColorMask = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);
            //e.ColorMask = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);

            _size.X = 57 + (e.GetBaseSize().X);
            _size.Y = c.GetBaseSize().Y;
        }

        private IntPoint _size = IntPoint.Zero;
        public override IntPoint GetBaseSize()
        {
            return _size;
        }
    }
}
