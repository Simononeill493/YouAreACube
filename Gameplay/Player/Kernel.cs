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
        public string Name;

        public SurfaceBlock Host { get; private set; }
        public HashSet<TemplateVersionDictionary> KnownTemplates { get; private set; }
        public List<Block> Companions { get; private set; }


        public Kernel()
        {
            Companions = new List<Block>();
            KnownTemplates = new HashSet<TemplateVersionDictionary>();
        }
        public void InitializeSession()
        {
            _refreshKnownTemplates();

            if(Config.KernelLearnAllTemplates)
            {
                LearnAllLoadedTemplates();
            }
        }


        public void Update()
        {
            Host.AddEnergy(1);
        }

        public void SetHost(SurfaceBlock block)
        {
            Companions.Remove(Host);
            Host = block;
            Companions.Add(block);
        }


        public void AddKnownTemplate(TemplateVersionDictionary template) => KnownTemplates.Add(template);
        public void LearnAllLoadedTemplates() => Templates.Database.GetAllVersionLists().ForEach(t => AddKnownTemplate(t));
        public void UpdateCompanionTemplates() => Companions.ForEach(t => t.SetTemplateToMain());
        private void _refreshKnownTemplates()
        {
            var templatesList = KnownTemplates.ToList();
            KnownTemplates.Clear();
            foreach (var template in templatesList)
            {
                var runtimeVersion = template.GetRuntimeVersion();
                if(runtimeVersion!=null)
                {
                    KnownTemplates.Add(runtimeVersion);
                }
                else
                {
                    KnownTemplates.Add(template);

                }
            }
        }
    }
}
