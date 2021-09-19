using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class CubeSpriteBox : SpriteScreenItem
    {
        protected SpriteScreenItem _sprite;

        public CubeSpriteBox(IHasDrawLayer parentDrawLayer,string sprite = null) : base(parentDrawLayer, MenuSprites.SpriteBox)
        {
            if(sprite==null) { sprite = MenuSprites.BlankPixel; }

            _sprite = new SpriteScreenItem(this, sprite);
            _sprite.SetLocationConfig(14, 14, CoordinateMode.ParentPercentage);
            AddChild(_sprite);
        }
    }
}
