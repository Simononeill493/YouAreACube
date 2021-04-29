using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CameraUtils
    {
        public static IntPoint GetMovementOffsets(Block block, int tileSize)
        {
            if (block.IsMoving)
            {
                return block.MovementData.MovementOffset * ((int)(block.MovementData.MovementOffsetPercentage * tileSize));
            }

            return IntPoint.Zero;
        }

        public static IntPoint GetBlockOffsetFromOrigin(Block block,int tileSize)
        {
            var offset = GetMovementOffsets(block, tileSize);
            var pos = (block.Location.AbsoluteLocation * tileSize) + (offset);

            return pos;
        }
    }
}
