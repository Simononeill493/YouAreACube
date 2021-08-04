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
            ChipTester.SetTestChipsets(Database);
            TemplateParsingTester.TestParsingRoundTrips(Database);
        }

        public static CubeTemplate GetRuntimeTemplate(CubeTemplate savedVersion) => Database[savedVersion.Versions.Name][savedVersion.Version];

        public static Cube Generate(string name,int version,Kernel source,CubeMode blockType) => Database[name][version].Generate(source,blockType);
        public static SurfaceCube GenerateSurface(string name, int version, Kernel source) => Database[name][version].GenerateSurface(source);
        public static GroundCube GenerateGround(string name, int version, Kernel source) => Database[name][version].GenerateGround(source);
        public static EphemeralCube GenerateEphemeral(string name, int version, Kernel source) => Database[name][version].GenerateEphemeral(source);
    }
}
