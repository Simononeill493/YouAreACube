using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class AppearanceEditSpriteBox : CubeSpriteBox
    {
        public AppearanceEditSpriteBox(IHasDrawLayer parent,string originalSprite) : base(parent)
        {
            _sprite.SpriteName = originalSprite;
        }
    }
}
