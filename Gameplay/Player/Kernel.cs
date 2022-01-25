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
        public virtual int Age { get; private set; }

        public SurfaceCube Host { get; private set; }
        public HashSet<TemplateVersionDictionary> KnownTemplates { get; private set; }
        public HashSet<BlockData> KnownBlocks { get; private set; }
        public List<Cube> Companions { get; private set; }


        public Kernel()
        {
            Companions = new List<Cube>();
            KnownTemplates = new HashSet<TemplateVersionDictionary>();
            KnownBlocks = new HashSet<BlockData>();
            LearnAllBlocks();
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
            Age++;
            if(Age%10==0)
            {
                Host.AddEnergy(1);
            }


            HostLoc = Host.Location.AbsoluteLocation;
        }

        public void SetHost(SurfaceCube block)
        {
            Companions.Remove(Host);
            Host = block;
            Companions.Add(block);

            if (Config.KernelHostInvincible)
            {
                Host.Invincible = true;
            }

        }


        public void AddKnownTemplate(TemplateVersionDictionary template) => KnownTemplates.Add(template);
        public void LearnAllLoadedTemplates() => Templates.Database.GetAllVersionLists().ForEach(t => AddKnownTemplate(t));
        public void UpdateCompanionTemplates() => Companions.ForEach(t => t.UpdateAsCompanion());
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



        public void AddKnownBlock(BlockData block) => KnownBlocks.Add(block);
        public void LearnAllBlocks()
        {
            BlockDataDatabase.BlockDataDict.ToList().ForEach(b => KnownBlocks.Add(b.Value));
        }
    }
}