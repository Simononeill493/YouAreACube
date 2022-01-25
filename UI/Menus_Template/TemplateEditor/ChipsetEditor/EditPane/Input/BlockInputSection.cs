using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputSection : SpriteScreenItem
    {
        public BlockInputSection(IHasDrawLayer parent,string name): base(parent,MenuSprites.BlockMiddle)
        {
            var textItem = _addStaticTextItem(name, 4, 40, CoordinateMode.ParentPercentage, false);
            textItem.MultiplyScale(0.5f);
            textItem.SetConstantColor(Color.White);
        }
    }
}
