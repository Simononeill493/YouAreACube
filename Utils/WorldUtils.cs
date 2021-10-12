using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class WorldUtils
    {
        public static IntPoint WorldCoordsToSectorLocation(IntPoint worldCoords,IntPoint sectorSize)
        {
            var ret = (worldCoords / sectorSize);

            if (worldCoords.X < 0 & (-worldCoords.X % sectorSize.X != 0))
            {
                ret.X -= 1;
            }
            if (worldCoords.Y < 0 & (-worldCoords.Y % sectorSize.Y != 0))
            {
                ret.Y -= 1;
            }

            return ret;
        }
        public static IntPoint WorldCoordsToInternalSectorCoords(IntPoint worldCoords,IntPoint sectorSize)
        {
            var ret = IntPoint.Zero;

            if (worldCoords.X >= 0)
            {
                ret.X = worldCoords.X % sectorSize.X;
            }
            else
            {
                ret.X = (sectorSize.X - 1) - ((-worldCoords.X - 1) % sectorSize.X);
            }

            if (worldCoords.Y >= 0)
            {
                ret.Y = worldCoords.Y % sectorSize.Y;
            }
            else
            {
                ret.Y = (sectorSize.Y - 1) - ((-worldCoords.Y - 1) % sectorSize.Y);
            }

            return ret;
        }
    }
}
