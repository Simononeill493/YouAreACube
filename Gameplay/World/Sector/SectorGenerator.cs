
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
            foreach(var dir in world.Focus.EmptyAdjacents)
            {
                var point = world.Focus.AbsoluteLocation + DirectionUtils.XYOffset(dir);
                var sector = WorldGen.GetTestSector(point);
                world.AddSector(sector);
            }
        }
    }
}
