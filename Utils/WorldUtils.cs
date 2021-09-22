using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class WorldUtils
    {
        public static IntPoint WorldCoordsToSectorLocation(IntPoint worldCoords,int sectorSize)
        {
            var ret = new IntPoint(worldCoords.X / sectorSize, worldCoords.Y / sectorSize);

            if (worldCoords.X < 0 & (-worldCoords.X % sectorSize != 0))
            {
                ret.X -= 1;
            }
            if (worldCoords.Y < 0 & (-worldCoords.Y % sectorSize != 0))
            {
                ret.Y -= 1;
            }

            return ret;
        }
        public static IntPoint WorldCoordsToInternalSectorCoords(IntPoint worldCoords,int sectorSize)
        {
            var ret = IntPoint.Zero;

            if (worldCoords.X >= 0)
            {
                ret.X = worldCoords.X % sectorSize;
            }
            else
            {
                ret.X = (sectorSize - 1) - ((-worldCoords.X - 1) % sectorSize);
            }

            if (worldCoords.Y >= 0)
            {
                ret.Y = worldCoords.Y % sectorSize;
            }
            else
            {
                ret.Y = (sectorSize - 1) - ((-worldCoords.Y - 1) % sectorSize);
            }

            return ret;
        }
    }
}
