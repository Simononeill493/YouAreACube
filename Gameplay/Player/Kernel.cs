using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Kernel
    {
        public SurfaceBlock Host;
        public List<BlockTemplate> KnownTemplates = new List<BlockTemplate>();

        public Kernel()
        {
            KnownTemplates = Templates.BlockTemplates.Values.ToList();
        }

        public void SupplyPowerToHost()
        {
            Host.AddEnergy(1);
        }
    }
}
