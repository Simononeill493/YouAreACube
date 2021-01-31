using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class TickCounter
    {
        public int TickInCycle;
        public int TotalTicks;

        public TickCounter()
        {
            TotalTicks = 0;
        }

        public void Tick()
        {
            TotalTicks++;
            TickInCycle = TotalTicks % Config.TickCycleLength;
        }
    }
}
