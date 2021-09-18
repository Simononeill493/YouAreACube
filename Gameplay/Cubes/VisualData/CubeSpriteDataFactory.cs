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
                case CubeSpriteDataType.Attach4:
                    return GenerateAttach4SpriteData(template);
                default:
                    throw new Exception("generating cube sprite data with unkown type");
            }
        }

        public static CubeSpriteDataSingle GenerateSingleSpriteData(CubeTemplate template)
        {
            return new CubeSpriteDataSingle(template.Sprite);
        }

        public static CubeSpriteDataAttach4 GenerateAttach4SpriteData(CubeTemplate template)
        {
            var spriteSet = Tilesets.Attach4Spritesets[template.Sprite];
            return new CubeSpriteDataAttach4(spriteSet);
        }

    }
}