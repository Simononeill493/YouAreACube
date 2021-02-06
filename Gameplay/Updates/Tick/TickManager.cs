using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TickManager
    {
        public Dictionary<Point, int> SectorTicks;

        public int TickInCycle;
        public int WorldTicks;

        public TickManager()
        {
            WorldTicks = 0;
            SectorTicks = new Dictionary<Point, int>();
        }

        public void WorldTick()
        {
            WorldTicks++;
            TickInCycle = WorldTicks % Config.TickCycleLength;
        }

        public void SectorTick(Sector sector)
        {
            if(SectorTicks.ContainsKey(sector.AbsoluteLocation))
            {
                SectorTicks[sector.AbsoluteLocation]++;
            }
            else
            {
                SectorTicks[sector.AbsoluteLocation] = 1;
            }
        }

        public IEnumerable<Block> GetUpdatingBlocks(Sector sector,List<Block> blocks)
        {
            return blocks.Where(b => (SectorTicks[sector.AbsoluteLocation] + b.SpeedOffset) % b.Speed == 0);
        }
    }
}
