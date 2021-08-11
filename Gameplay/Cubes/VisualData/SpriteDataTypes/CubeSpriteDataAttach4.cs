using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class CubeSpriteDataAttach4 : CubeSpriteData
    {
        private Attach4SpriteSet _spriteSet;

        public CubeSpriteDataAttach4(Attach4SpriteSet spriteSet) : base(CubeSpriteDataType.Attach4)
        {
            _spriteSet = spriteSet;

            CurrentSprite = _spriteSet.sprite0;
        }

        public override void AttachSpriteToNeighbours(CubeMode cubeMode, Dictionary<CardinalDirection, Tile> adjacents)
        {
            var directionFlags = new bool[8];
            var neighbours = adjacents.Where(a => a.Value.HasCube(cubeMode));
            foreach(var neighbour in neighbours)
            {
                directionFlags[(int)neighbour.Key] = true;
            }

            bool rotate90 = false;
            (CurrentSprite, HorizontalFlip, VerticalFlip, rotate90) = _spriteSet.GetSpriteFromNeighbourFlags(directionFlags);

            if(rotate90)
            {
                RadianRotation = (float)Math.PI / 2;
            }
            else
            {
                RadianRotation = 0;
            }
        }

    }
}
