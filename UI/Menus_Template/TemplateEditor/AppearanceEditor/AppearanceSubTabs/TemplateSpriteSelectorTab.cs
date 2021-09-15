using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateSpriteSelectorTab : ContainerMenuItem
    {
        public TemplateSpriteSelectorTab(IHasDrawLayer parent,List<(string,CubeSpriteDataType)> sprites,Action<string,CubeSpriteDataType> spriteSelected) : base(parent)
        {
            var spritesMatrix = new SpriteBoxMatrix<SpriteSelectorSpriteBox>(this, 6, 6);
            spritesMatrix.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, false);
            spritesMatrix.AddBoxes(_createSpriteBoxes(sprites,spriteSelected));
            spritesMatrix.PadToCapacity(_createSpriteBoxBlank);
            AddChild(spritesMatrix);
        }

        private List<SpriteSelectorSpriteBox> _createSpriteBoxes(List<(string, CubeSpriteDataType)> sprites, Action<string, CubeSpriteDataType> spriteSelected)
        {
            return sprites.Select(s => _createSpriteBox(s.Item1, s.Item2, spriteSelected)).ToList();
        }

        private SpriteSelectorSpriteBox _createSpriteBox(string sprite,CubeSpriteDataType spriteType, Action<string, CubeSpriteDataType> spriteSelected)
        {
            var box = new SpriteSelectorSpriteBox(this, sprite, spriteType, spriteSelected);
            box.HighlightedSpriteName = BuiltInMenuSprites.SpriteBox_Highlighted;

            return box;
        }

        private SpriteSelectorSpriteBox _createSpriteBoxBlank()
        {
            var box = new SpriteSelectorSpriteBox(this);
            box.HighlightedSpriteName = BuiltInMenuSprites.SpriteBox_Highlighted;

            return box;
        }
    }
}
