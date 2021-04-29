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
        public int WorldTicks;
        public int TickInCycle;

        public Dictionary<IntPoint, int> SectorTicks;

        public TickManager()
        {
            WorldTicks = 0;
            SectorTicks = new Dictionary<IntPoint, int>();
        }

        public void IncrementWorldTimer()
        {
            WorldTicks++;
            TickInCycle = WorldTicks % Config.TickCycleLength;
        }

        public void IncrementSectorTimer(Sector sector) => SectorTicks[sector.AbsoluteLocation]++;
        public void AddSector(Sector sector) => SectorTicks[sector.AbsoluteLocation] = 0;
        public IEnumerable<Block> GetUpdatingBlocks(Sector sector, List<Block> blocks) => blocks.Where(b => ((SectorTicks[sector.AbsoluteLocation] + b.SpeedOffset) % b.Speed == 0) & b.CanUpdate);
    }
}
