using Microsoft.Xna.Framework;

namespace IAmACube
{
    class BlockTop : SpriteScreenItem
    {
        public BlockTop(IHasDrawLayer parent,string name) : base(parent,MenuSprites.Block)
        {
            var title = _addStaticTextItem(name, 7, 6, CoordinateMode.ParentPixel, false);
            title.SetConstantColor(Color.White);
        }
    }
}
