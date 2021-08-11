using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace IAmACube
{
    [Serializable()]
    public abstract class CubeSpriteData
    {
        public CubeSpriteDataType SpriteDataType { get; }
        public string CurrentSprite { get; protected set; }
        public XnaColors ColorMask;

        public bool HorizontalFlip;
        public bool VerticalFlip;
        public float RadianRotation;

        public CubeSpriteData(CubeSpriteDataType spriteDataType)
        {
            SpriteDataType = spriteDataType;
            ColorMask = XnaColors.ClearColorMask;
        }

        public virtual void AttachSpriteToNeighbours(CubeMode cubeMode,Dictionary<CardinalDirection, Tile> adjacents){}
    }
}
