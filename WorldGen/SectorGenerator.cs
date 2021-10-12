
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class SectorGenerator
    {
        public void GenerateAdjacentSectors(World world)
        {
            //throw new NotImplementedException();

            foreach(var dir in world.Focus.GetEmptyAdjacents())
            {
                var point = world.Focus.AbsoluteLocation + DirectionUtils.XYOffset(dir);
                //var sector = WorldGen.GetFilledTestSector(world.Random,point,world.SectorSize,world.WorldKernel);
                var sector = WorldGen.GetEmptyTestSector(point, world.SectorSize);

                //WorldGen.AddEntities(sector, world.Random);

                world.AddSector(sector);
            }
        }
    }
}
