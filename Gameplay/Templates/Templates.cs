using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class Templates
    {
        public static TemplateDatabase Database { get; private set; }

        public static void Load() 
        {
            Database = new TemplateDatabase();

            _loadBuiltInTemplates();
            _testCode();
        }

        private static void _loadBuiltInTemplates()
        {
            var templatesToken = FileUtils.LoadJson(ConfigFiles.TemplatesPath)["blocks"];
            var builtInTemplates = TemplateParser.ParseTemplates(templatesToken);

            foreach (var builtInTemplate in builtInTemplates)
            {
                var versions = new TemplateVersionDictionary(builtInTemplate.Name, builtInTemplate);
                Database[builtInTemplate.Name] = versions;
            }
        }
        private static void _testCode()
        {
            ChipTester.SetTestBlocks(Database);
            TemplateParsingTester.TestParsingRoundTrips(Database);
        }

        public static BlockTemplate GetRuntimeVersion(BlockTemplate savedVersion) => Database[savedVersion.Versions.Name][savedVersion.Version];
        public static Block Generate(string name,int version,BlockMode blockType) => Database[name][version].Generate(blockType);
        public static SurfaceBlock GenerateSurface(string name, int version) => Database[name][version].GenerateSurface();
        public static GroundBlock GenerateGround(string name, int version) => Database[name][version].GenerateGround();
        public static EphemeralBlock GenerateEphemeral(string name, int version) => Database[name][version].GenerateEphemeral();
    }
}
