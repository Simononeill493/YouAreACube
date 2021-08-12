using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class AppearanceEditSpriteBox : CubeSpriteBox
    {
        private CubeSpriteDataType _spriteType;

        public AppearanceEditSpriteBox(IHasDrawLayer parent) : base(parent) { }

        public void SetSpriteToSingleSpriteType(string spriteName, CubeSpriteDataType spriteType)
        {
            _sprite.SpriteName = spriteName;
            _spriteType = spriteType;
        }

        public (string, CubeSpriteDataType) GenerateSpriteDataForTemplate()
        {
            return (_sprite.SpriteName, _spriteType);
        }
    }
}
