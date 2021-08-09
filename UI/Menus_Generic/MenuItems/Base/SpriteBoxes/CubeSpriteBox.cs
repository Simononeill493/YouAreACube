using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class CubeSpriteBox : SpriteMenuItem
    {
        protected SpriteMenuItem _sprite;

        public CubeSpriteBox(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "TemplateItemContainer")
        {
            _sprite = new SpriteMenuItem(this, "Blank");
            _sprite.SetLocationConfig(14, 14, CoordinateMode.ParentPercentageOffset);
            AddChild(_sprite);
        }
    }
}
