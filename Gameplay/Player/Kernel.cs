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
        public static IntPoint HostLoc;

        public string Name;

        public SurfaceCube Host { get; private set; }
        public HashSet<TemplateVersionDictionary> KnownTemplates { get; private set; }
        public List<Cube> Companions { get; private set; }


        public Kernel()
        {
            Companions = new List<Cube>();
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
            HostLoc = Host.Location.AbsoluteLocation;
        }

        public void SetHost(SurfaceCube block)
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
