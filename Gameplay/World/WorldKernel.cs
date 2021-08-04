using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class WorldKernel : Kernel
    {
        public override int Age => _ticker.WorldTicks;
        private WorldTicker _ticker;

        public WorldKernel(WorldTicker ticker)
        {
            _ticker = ticker;
        }
    }
}
