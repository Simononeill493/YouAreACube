using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TickManager
    {
        public int TickInCycle;
        public int TotalTicks;

        public TickManager()
        {
            TotalTicks = 0;
        }

        public void Tick()
        {
            TotalTicks++;
            TickInCycle = TotalTicks % Config.TickCycleLength;
        }

        public IEnumerable<Block> GetUpdatingBlocks(List<Block> blocks)
        {
            return blocks.Where(b => (TotalTicks + b.SpeedOffset) % b.Speed == 0);
        }
    }
}
