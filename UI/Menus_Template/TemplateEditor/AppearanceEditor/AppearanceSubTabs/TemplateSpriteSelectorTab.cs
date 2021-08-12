using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSpriteSelectorTab : ContainerMenuItem
    {
        public TemplateSpriteSelectorTab(IHasDrawLayer parent,List<string> sprites) : base(parent)
        {

            var spritesMatrix = new SpriteBoxMatrix<CubeSpriteBox>(this, 6, 6);
            spritesMatrix.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            spritesMatrix.AddBoxes(sprites.Select(s => new CubeSpriteBox(this, s) { HighlightedSpriteName = BuiltInMenuSprites.SpriteBox_Highlighted } ).ToList());
            spritesMatrix.PadToCapacity(() => new CubeSpriteBox(this) { HighlightedSpriteName = BuiltInMenuSprites.SpriteBox_Highlighted });
            AddChild(spritesMatrix);
        }
    }
}
