using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockTop_2 : SpriteMenuItem
    {
        public BlockTop_2(IHasDrawLayer parent,string name) : base(parent,BuiltInMenuSprites.Block)
        {
            var title = _addTextItem(name, 7, 6, CoordinateMode.ParentPixelOffset, false);
            title.Color = Color.White;
        }
    }
}
