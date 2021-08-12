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
        public void InitializeSpriteData() => SpriteData = CubeSpriteDataFactory.Generate(Template);

        public void AttachSpriteToNeighbours() => SpriteData.AttachSpriteToNeighbours(this.CubeMode,Location.Adjacent);

        public void UpdateNeighbourSprites() => Location.UpdateNeighbourSprites(CubeMode);

    }
}
