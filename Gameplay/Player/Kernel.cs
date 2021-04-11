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
        public List<TemplateVersionList> KnownTemplates = new List<TemplateVersionList>();

        public Kernel()
        {
            _loadKnownTemplates();
        }

        public void StartSession()
        {
            _loadKnownTemplates();
        }

        private void _loadKnownTemplates()
        {
            KnownTemplates = Templates.BlockTemplates.GetAllTemplates();
        }

        public void SupplyPowerToHost()
        {
            Host.AddEnergy(1);
        }
    }
}
