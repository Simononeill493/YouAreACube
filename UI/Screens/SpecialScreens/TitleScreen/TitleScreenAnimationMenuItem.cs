using System;
using System.Collections.Generic;
using System.Linq;
namespace IAmACube
{
    class TitleScreenAnimationMenuItem : FullScreenMenuItem
    {
        private TitleScreenAnimationFloaterScroll floaterContainer;

        public TitleScreenAnimationMenuItem(IHasDrawLayer parent, Func<IntPoint> parentSizeProvider) : base(parent, parentSizeProvider)
        {
            var youAreA = new SpriteScreenItem(this, TitleAnimationSprites.YouAreA);
            youAreA.SetLocationConfig(50, 20, CoordinateMode.ParentPercentage, centered: true);
            youAreA.MultiplyScale(0.25f);
            AddChild(youAreA);

            var title = new TitleScreenCubeNameMenuItem(this);
            title.SetLocationConfig(50, 45, CoordinateMode.ParentPercentage, centered: true);
            AddChild(title);

            floaterContainer = new TitleScreenAnimationFloaterScroll(this,GetBaseSize);
            floaterContainer.SetLocationConfig(0, 0, CoordinateMode.Absolute, false);
            AddChild(floaterContainer);
        }


        public void SetScatteredFloaters() => floaterContainer.SetScatteredFloaters();
    }


}
