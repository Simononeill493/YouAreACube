using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class CubeSpriteDataFactory
    {
        public static CubeSpriteData Generate(CubeTemplate template)
        {
            switch (template.SpriteType)
            {
                case CubeSpriteDataType.SingleSprite:
                    return GenerateSingleSpriteData(template);
                default:
                    throw new Exception("generating cube sprite data with unkown type");
            }
        }

        public static CubeSpriteData GenerateSingleSpriteData(CubeTemplate template)
        {
            return new CubeSpriteDataSingle(template.Sprite);
        }
    }
}