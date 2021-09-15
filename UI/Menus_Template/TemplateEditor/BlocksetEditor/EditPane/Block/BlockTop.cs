using Microsoft.Xna.Framework;

namespace IAmACube
{
    class BlockTop : SpriteMenuItem
    {
        public BlockTop(IHasDrawLayer parent,string name) : base(parent,BuiltInMenuSprites.Block)
        {
            var title = _addStaticTextItem(name, 7, 6, CoordinateMode.ParentPixel, false);
            title.Color = Color.White;
        }
    }
}
