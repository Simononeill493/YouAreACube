using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class WorldTickManager
    {
        public int WorldTicks { get; private set; }
        private Dictionary<IntPoint, int> _sectorTicks;

        public WorldTickManager()
        {
            WorldTicks = 0;
            _sectorTicks = new Dictionary<IntPoint, int>();
        }

        public void IncrementWorldTimer() => WorldTicks++;
        public void IncrementSectorTimer(Sector sector) => _sectorTicks[sector.AbsoluteLocation]++;
        
        
        public void AddSector(Sector sector) => _sectorTicks[sector.AbsoluteLocation] = 0;
        public IEnumerable<Block> GetUpdatingBlocks(Sector sector) => sector.ActiveBlocks.Where(b => ((_sectorTicks[sector.AbsoluteLocation] + b.SpeedOffset) % b.Speed == 0) & b.CanUpdate);
    }
}
