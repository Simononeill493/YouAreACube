using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputSection : SpriteMenuItem
    {
        public BlockInputSection(IHasDrawLayer parent,string name): base(parent,BuiltInMenuSprites.BlockMiddle)
        {
            var textItem = _addStaticTextItem(name, 4, 40, CoordinateMode.ParentPercentage, false);
            textItem.MultiplyScale(0.5f);
            textItem.Color = Color.White;
        }
    }
}
