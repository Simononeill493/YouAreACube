using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class AppearanceEditSpriteBox : CubeSpriteBox
    {
        public AppearanceEditSpriteBox(IHasDrawLayer parent) : base(parent)
        {
        }

        public void SetSpriteToSingleImage(string spriteName)
        {
            _sprite.SpriteName = spriteName;
        }
    }
}
