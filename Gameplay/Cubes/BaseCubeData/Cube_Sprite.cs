using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class Cube
    {
        public CubeSpriteData SpriteData;
        //public string Sprite => SpriteData => CurrentSprite;

        public void AttachSpriteToNeighbours() => SpriteData.AttachSpriteToNeighbours(this.CubeMode,Location.Adjacent);

        public void UpdateNeighbourSprites() => Location.UpdateNeighbourSprites(CubeMode);

    }
}
