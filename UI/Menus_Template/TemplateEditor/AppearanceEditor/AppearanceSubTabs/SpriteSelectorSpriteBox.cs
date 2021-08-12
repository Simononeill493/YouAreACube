using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class SpriteSelectorSpriteBox : CubeSpriteBox
    {
        CubeSpriteDataType _spriteType;
        Action<string, CubeSpriteDataType> _spriteSelectedCallback;

        public SpriteSelectorSpriteBox(IHasDrawLayer parent,string sprite,CubeSpriteDataType spriteType, Action<string, CubeSpriteDataType> spriteSelected) :base(parent,sprite)
        {
            _spriteType = spriteType;
            _spriteSelectedCallback = spriteSelected;
            this.OnMouseReleased += _onClick;
        }

        public SpriteSelectorSpriteBox(IHasDrawLayer parent) : base(parent)
        {
            _spriteType = CubeSpriteDataType.Undefined;
        }

        private void _onClick(UserInput i)
        {
            _spriteSelectedCallback(_sprite.SpriteName, _spriteType);
        }

    }
}
