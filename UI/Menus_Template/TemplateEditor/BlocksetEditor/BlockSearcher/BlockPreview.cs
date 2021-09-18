using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace IAmACube
{
    class BlockPreview : SpriteMenuItem
    {
        public BlockData Block;
        private TextMenuItem _text;

        public BlockPreview(IHasDrawLayer parent, BlockData chip) : base(parent, MenuSprites.PreviewBlock)
        {
            Block = chip;
            ColorMask = Block.ChipDataType.GetColor();

            _text = new TextMenuItem(this, ()=>chip.Name);
            _text.Color = Color.White;
            _text.MultiplyScale(0.5f);

            _text.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, true);
            AddChild(_text);
        }
    }
}
