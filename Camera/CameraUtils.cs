using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CameraUtils
    {
        public static Point GetMovementOffsets(Block block,int tileSize)
        {
            var offset = GetMovementOffset(block,tileSize);
            return (block.MovementData.Offset * offset);
        }

        public static int GetMovementOffset(Block block, int tileSize)
        {
            if (block.IsMoving)
            {
                return (int)(((block.MovementData.MovementPosition) / (float)(block.MovementData.MoveSpeed)) * tileSize);
            }

            return 0;
        }

        public static Point GetBlockOffsetFromOrigin(Block block,int tileSize)
        {
            var offset = GetMovementOffsets(block, tileSize);
            var pos = (block.Location.AbsoluteLocation * tileSize) + offset;

            return pos;
        }
    }
}
