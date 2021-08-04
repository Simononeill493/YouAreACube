
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
            throw new NotImplementedException();

            foreach(var dir in world.Focus.GetEmptyAdjacents())
            {
                var point = world.Focus.AbsoluteLocation + DirectionUtils.XYOffset(dir);
                var sector = WorldGen.GetTestSector(world.Random,point,world.SectorSize,world.WorldKernel);
                //WorldGen.AddEntities(sector, world.Random);

                world.AddSector(sector);
            }
        }
    }
}
