using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSpriteSelectorTab : ContainerMenuItem
    {
        public TemplateSpriteSelectorTab(IHasDrawLayer parent) : base(parent)
        {
            var sprites = new SpriteBoxMatrix<CubeSpriteBox>(this, 6, 6);
            sprites.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            sprites.PadToCapacity(() => new CubeSpriteBox(this));
            AddChild(sprites);
        }
    }
}
