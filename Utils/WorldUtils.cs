using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class WorldUtils
    {
        public static IntPoint GetLocationOfSector(IntPoint worldCoords)
        {
            var ret = new IntPoint(worldCoords.X / Config.SectorSize, worldCoords.Y / Config.SectorSize);

            if (worldCoords.X < 0 & (-worldCoords.X % Config.SectorSize != 0))
            {
                ret.X -= 1;
            }
            if (worldCoords.Y < 0 & (-worldCoords.Y % Config.SectorSize != 0))
            {
                ret.Y -= 1;
            }

            return ret;
        }
        public static IntPoint ConvertToSectorCoords(IntPoint worldCoords)
        {
            var ret = new IntPoint(0, 0);

            if (worldCoords.X >= 0)
            {
                ret.X = worldCoords.X % Config.SectorSize;
            }
            else
            {
                ret.X = (Config.SectorSize - 1) - ((-worldCoords.X - 1) % Config.SectorSize);
            }

            if (worldCoords.Y >= 0)
            {
                ret.Y = worldCoords.Y % Config.SectorSize;
            }
            else
            {
                ret.Y = (Config.SectorSize - 1) - ((-worldCoords.Y - 1) % Config.SectorSize);
            }

            return ret;
        }
    }
}
