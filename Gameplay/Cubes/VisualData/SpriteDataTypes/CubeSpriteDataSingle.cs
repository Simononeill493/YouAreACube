using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class CubeSpriteDataSingle :CubeSpriteData
    {
        public CubeSpriteDataSingle(string spriteName) : base(CubeSpriteDataType.SingleSprite)
        {
            CurrentSprite = spriteName;
        }
    }
}
