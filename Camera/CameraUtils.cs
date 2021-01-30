using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CameraUtils
    {
        public static (int x, int y) GetMovementOffsets(Block block,int tileSize)
        {
            var offset = GetMovementOffset(block,tileSize);
            return (block.MovementData.XOffset * offset, block.MovementData.YOffset * offset);
        }

        public static int GetMovementOffset(Block block, int tileSize)
        {
            if (block.IsMoving)
            {
                return (int)(((block.MovementData.MovementPosition) / (float)(block.Speed)) * tileSize);
            }

            return 0;
        }

        public static (int x, int y) GetBlockOffsetFromOrigin(Block block,int tileSize)
        {
            var (offsetX, offsetY) = GetMovementOffsets(block, tileSize);

            var XPos = (block.Location.X * tileSize) + offsetX;
            var YPos = (block.Location.Y * tileSize) + offsetY;

            return (XPos, YPos);
        }
    }
}
