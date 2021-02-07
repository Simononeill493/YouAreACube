using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class TickManager
    {
        public Dictionary<Point, int> SectorTicks;

        public int TickInCycle;
        public int WorldTicks;

        public TickManager()
        {
            WorldTicks = 0;
            SectorTicks = new Dictionary<Point, int>() { [new Point(0, 0)] = 0 };
        }

        public void IncrementWorldTimer()
        {
            WorldTicks++;
            TickInCycle = WorldTicks % Config.TickCycleLength;
        }

        public void IncrementSectorTimer(Sector sector)
        {
            SectorTicks[sector.AbsoluteLocation]++;
        }

        public void AddSector(Sector sector)
        {
            SectorTicks[sector.AbsoluteLocation] = 0;
        }

        public IEnumerable<Block> GetUpdatingBlocks(Sector sector,List<Block> blocks)
        {
            return blocks.Where(b => (SectorTicks[sector.AbsoluteLocation] + b.SpeedOffset) % b.Speed == 0);
        }
    }
}
