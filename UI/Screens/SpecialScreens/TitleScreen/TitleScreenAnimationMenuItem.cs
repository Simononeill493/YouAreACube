using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    class TitleScreenAnimationMenuItem : FullScreenMenuItem
    {
        private TitleScreenAnimationFloaterScroll _floaterContainer;

        public TitleScreenAnimationMenuItem(IHasDrawLayer parent, Func<IntPoint> parentSizeProvider) : base(parent, parentSizeProvider)
        {
            _floaterContainer = new TitleScreenAnimationFloaterScroll(this, GetBaseSize);
            _floaterContainer.SetLocationConfig(0, 0, CoordinateMode.Absolute, false);
            AddChild(_floaterContainer);

            var titleContainer = new TitleContainer(this);
            titleContainer.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, centered: true);
            titleContainer.MultiplyScale(0.75f);

            AddChild(titleContainer);
        }

        public class TitleContainer : ContainerScreenItem
        {
            private IntPoint _size = new IntPoint(90, 70);
            public override IntPoint GetBaseSize() => _size;

            public TitleContainer(IHasDrawLayer parent) : base(parent)
            {
                var youAreA = new SpriteScreenItem(this, TitleAnimationSprites.YouAreA);
                youAreA.SetLocationConfig(50, 5, CoordinateMode.ParentPercentage, centered: true);
                youAreA.MultiplyScale(0.25f);
                youAreA.ColorMask = new Color(255, 255, 255, 0);
                AddChild(youAreA);

                var title = new TitleScreenCubeNameMenuItem(this);
                title.SetLocationConfig(50, 33, CoordinateMode.ParentPercentage, centered: true);
                title.MultiplyScale(1.5f);
                AddChild(title);

                var versionBox = new SpriteScreenItem(this, TitleAnimationSprites.VersionBox);
                versionBox.SetLocationConfig(50, 63, CoordinateMode.ParentPercentage, centered: true);
                versionBox.MultiplyScale(0.5f);
                AddChild(versionBox);

                var lilCube = new SpriteScreenItem(this, TitleAnimationSprites.TitleCharacter);
                lilCube.SetLocationConfig(50, 90, CoordinateMode.ParentPercentage, centered: true);
                AddChild(lilCube);
            }
        }

        public void SetScatteredFloaters() => _floaterContainer.SetScatteredFloaters();
    }
}
