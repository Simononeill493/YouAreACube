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
        public SurfaceBlock Host { get; private set; }
        public List<Block> Companions;

        public List<TemplateAllVersions> KnownTemplates = new List<TemplateAllVersions>();

        public Kernel()
        {
            _loadKnownTemplates();
            Companions = new List<Block>();
        }

        public void SetHost(SurfaceBlock block)
        {
            Companions.Remove(Host);

            Host = block;
            Companions.Add(block);
        }

        public void UpdateCompanions() => Companions.ForEach(t => t.SetTemplateToMain());

        public void InitializeSession()
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

        public void AddKnownTemplate(TemplateAllVersions template)
        {
            KnownTemplates.Add(template);
        }
    }
}
